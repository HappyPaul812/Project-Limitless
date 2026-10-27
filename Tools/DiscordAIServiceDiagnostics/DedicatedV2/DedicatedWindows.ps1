# 이 adapter의 정의 로드만으로 OS는 바뀌지 않습니다. Install 승인 경계에서만 호출합니다.
function Get-D3Groups([string]$Sid){@(Get-LocalGroup|ForEach-Object{$g=$_;if(@(Get-LocalGroupMember -SID $g.SID -ErrorAction Stop|Where-Object {$_.SID.Value -ceq $Sid}).Count){$g.SID.Value}})}
function New-D3WindowsApi($Manifest,$Payloads,[Security.SecureString]$BotPassword,[Security.SecureString]$WorkerPassword){
 $pins=[Text.Encoding]::UTF8.GetString($Payloads['Deployment/Preservation.json'])|ConvertFrom-Json -AsHashtable
 $api=[pscustomobject]@{m=$Manifest;pins=$pins;botPassword=$BotPassword;workerPassword=$WorkerPassword;created=@{};consumeDirectory=$null;bindingSha=$null}
 $api|Add-Member ScriptMethod Context {
  $id=[Security.Principal.WindowsIdentity]::GetCurrent();@{admin=([Security.Principal.WindowsPrincipal]::new($id)).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator);admin_sid=$id.User.Value;now=[datetimeoffset]::UtcNow.ToUnixTimeSeconds();preexisting_sids=@(Get-LocalUser|ForEach-Object{$_.SID.Value})}
 }
 $api|Add-Member ScriptMethod Snapshot {
  $old=Get-LocalUser -Name LIMITLESSRelaySvc -ErrorAction Stop;$bot=Get-LocalUser -Name LIMITLESSBotTrial -ErrorAction Stop
  if($old.SID.Value -cne [DedicatedContract]::OldWorker -or -not $old.Enabled -or $bot.SID.Value -cne [DedicatedContract]::OldBot -or $bot.Enabled){throw 'D3_EXISTING_ACCOUNTS_HOLD'}
  $root='C:\Program Files\LimitlessRelayTrial';$all=@(Get-ChildItem -LiteralPath $root -Recurse -File -Force -ErrorAction Stop)
  if($all.Count -ne 855 -or $this.pins.v1_files.Count -ne 855){throw 'D3_V1_COUNT_HOLD'}
  foreach($name in $this.pins.v1_files.Keys){$p=Join-Path $root $name;if((Get-Item -LiteralPath $p -Force).Attributes -band [IO.FileAttributes]::ReparsePoint -or (Get-FileHash -LiteralPath $p).Hash.ToLowerInvariant() -cne $this.pins.v1_files[$name]){throw 'D3_V1_SHA_HOLD'}}
  $scheduler=New-Object -ComObject Schedule.Service;$scheduler.Connect();$state=@()
  foreach($name in $this.pins.onboot.Keys){$task=$scheduler.GetFolder('\').GetTask($name);$hash=D3Hash ([Text.Encoding]::UTF8.GetBytes([string]$task.Xml));if($hash -cne $this.pins.onboot[$name]){throw 'D3_ONBOOT_PIN_HOLD'};$state+=$hash}
  foreach($path in $this.pins.r11_artifacts.Keys){if((Get-FileHash -LiteralPath $path).Hash.ToLowerInvariant() -cne $this.pins.r11_artifacts[$path]){throw 'D3_R11_ORIGINAL_CHANGED'}}
  $state+=@($old.SID.Value,$old.Enabled,$bot.SID.Value,$bot.Enabled,(Get-Acl -LiteralPath $root).Sddl,((Get-D3Groups $old.SID.Value|Sort-Object)-join ','),(([DedicatedAccountPolicy]::Read($old.SID.Value)|Sort-Object)-join ','))
  @{known=$true;preserved=$true;fingerprint=(D3Hash ([Text.Encoding]::UTF8.GetBytes(($state -join '|'))))}
 }
 $api|Add-Member ScriptMethod Fresh {param($m)
  if(@(Get-LocalUser|Where-Object Name -In @($m.bot_account,$m.worker_account)).Count){throw 'D3_ACCOUNT_COLLISION'}
  foreach($p in @($m.code_root,$m.state_root)){if(Test-Path -LiteralPath $p -ErrorAction Stop){throw 'D3_PATH_COLLISION'}}
  $s=New-Object -ComObject Schedule.Service;$s.Connect();if(@($s.GetFolder('\').GetTasks(1)|Where-Object Name -IEQ $m.task_name).Count){throw 'D3_TASK_COLLISION'}
  if(@([IO.Directory]::GetFiles('\\.\pipe\')|Where-Object{[IO.Path]::GetFileName($_).StartsWith($m.pipe_name,[StringComparison]::OrdinalIgnoreCase)}).Count){throw 'D3_PIPE_COLLISION'}
  [LimitlessV2Publish]::Ancestors('C:\Program Files');[LimitlessV2Publish]::Ancestors('C:\ProgramData')
 }
 $api|Add-Member ScriptMethod Consume {param($m,$ms)
  [LimitlessV2Publish]::Verify($m.approval_root,[DedicatedContract]::FsSddl('private',$true,'',''))
  $this.consumeDirectory=Join-Path $m.approval_root $m.approval_id;[LimitlessV2Publish]::NewDirectory($this.consumeDirectory,[DedicatedContract]::FsSddl('private',$true,'',''))
  [LimitlessV2Publish]::NewFile((Join-Path $this.consumeDirectory 'consumed.json'),[Text.UTF8Encoding]::new($false).GetBytes((@{schema=3;epoch=$m.epoch;manifest_sha256=$ms;consumed=$true}|ConvertTo-Json -Compress)),[DedicatedContract]::FsSddl('private',$false,'',''))
 }
 $api|Add-Member ScriptMethod InstallLease {param($m)
  $p=Join-Path $this.consumeDirectory 'install.lease';[LimitlessV2Publish]::NewFile($p,[byte[]]::new(0),[DedicatedContract]::FsSddl('private',$false,'',''));[IO.File]::Open($p,[IO.FileMode]::Open,[IO.FileAccess]::ReadWrite,[IO.FileShare]::None)
 }
 $api|Add-Member ScriptMethod CreateUser {param($name,$package,$role)
  $password=if($role -ceq 'bot'){$this.botPassword}else{$this.workerPassword}
  $user=New-LocalUser -Name $name -Password $password -Disabled -AccountExpires (Get-Date).AddHours(24) -Description ('D3:'+ $package+':'+$role) -ErrorAction Stop
  $this.created[$role]=$user.SID.Value
  if(@(Get-LocalGroupMember -SID 'S-1-5-32-545' -ErrorAction Stop|Where-Object {$_.SID.Value -ceq $user.SID.Value}).Count -eq 0){Add-LocalGroupMember -SID 'S-1-5-32-545' -Member $user -ErrorAction Stop}
  $again=Get-LocalUser -SID $user.SID -ErrorAction Stop;$groups=@(Get-D3Groups $user.SID.Value)
  @{sid=$again.SID.Value;name=$again.Name;created_new=($null -eq $again.LastLogon -and $again.Description -ceq ('D3:'+ $package+':'+$role));disabled=(-not $again.Enabled);users_only=($groups.Count -eq 1 -and $groups[0] -ceq 'S-1-5-32-545')}
 }
 $api|Add-Member ScriptMethod Rights {param($bot,$worker,$admin)
  [DedicatedAccountPolicy]::Grant($bot,$this.created.bot,$worker,$admin);[DedicatedAccountPolicy]::Grant($worker,$this.created.worker,$bot,$admin)
  foreach($sid in @($bot,$worker)){if((@([DedicatedAccountPolicy]::Read($sid)|Sort-Object)-join ',') -cne (@([DedicatedContract]::Rights|Sort-Object)-join ',')){throw 'D3_RIGHTS_READBACK'}}
 }
 $api|Add-Member ScriptMethod Publish {param($m,$manifest,$payload,$bot,$worker)
  $sd=[DedicatedContract]::FsSddl('code',$true,$worker,$bot);$file=[DedicatedContract]::FsSddl('code',$false,$worker,$bot)
  [LimitlessV2Publish]::NewDirectory($m.code_root,$sd);$dirs=@{}
  foreach($n in $payload.Keys){$p=Split-Path (Join-Path $m.code_root $n) -Parent;while($p -cne $m.code_root){$dirs[$p]=$true;$p=Split-Path $p -Parent}}
  foreach($p in @($dirs.Keys|Sort-Object Length)){[LimitlessV2Publish]::NewDirectory($p,$sd)}
  foreach($n in $payload.Keys){[LimitlessV2Publish]::NewFile((Join-Path $m.code_root $n),$payload[$n],$file)}
  [LimitlessV2Publish]::NewFile((Join-Path $m.code_root 'HostManifest.json'),$manifest,$file)
  [LimitlessV2Publish]::NewDirectory($m.state_root,[DedicatedContract]::FsSddl('container',$true,$worker,''))
  foreach($suffix in @('Control','Auth')){[LimitlessV2Publish]::NewDirectory((Join-Path $m.state_root $suffix),[DedicatedContract]::FsSddl('container',$true,$worker,''))}
  foreach($suffix in @('Data','Temp','Control\Ledger','Auth\CodexHome')){[LimitlessV2Publish]::NewDirectory((Join-Path $m.state_root $suffix),[DedicatedContract]::FsSddl('state',$true,$worker,''))}
  [LimitlessV2Publish]::NewFile((Join-Path $m.state_root 'Control\worker.lease'),[byte[]]::new(0),[DedicatedContract]::FsSddl('state',$false,$worker,''))
 }
 $api|Add-Member ScriptMethod Binding {param($bytes,$bot,$worker)
  $this.bindingSha=D3Hash $bytes;[LimitlessV2Publish]::NewFile((Join-Path $this.m.code_root 'InstalledBinding.json'),$bytes,[DedicatedContract]::FsSddl('code',$false,$worker,$bot))
 }
 $api|Add-Member ScriptMethod EnableWorker {param($sid)
  if($sid -cne $this.created.worker -or $sid -ceq [DedicatedContract]::OldWorker -or $sid -ceq [DedicatedContract]::OldBot){throw 'D3_ONLY_CREATED_WORKER_ENABLE'}
  $u=Get-LocalUser -SID ([Security.Principal.SecurityIdentifier]::new($sid)) -ErrorAction Stop
  if($u.Name -cne $this.m.worker_account -or $u.Description -cne ('D3:'+ $this.m.package_id+':worker')){throw 'D3_NEW_WORKER_BINDING_CHANGED'}
  Enable-LocalUser -SID $u.SID -ErrorAction Stop
  if(-not (Get-LocalUser -SID $u.SID).Enabled){throw 'D3_NEW_WORKER_ENABLE_UNKNOWN'}
 }
 $api|Add-Member ScriptMethod Task {param($m,$worker,$ms,$bs)
  $s=New-Object -ComObject Schedule.Service;$s.Connect();$d=$s.NewTask(0)
  $d.Principal.UserId=$worker;$d.Principal.LogonType=1;$d.Principal.RunLevel=0
  $d.Settings.Enabled=$false;$d.Settings.RestartCount=0;$d.Settings.MultipleInstances=2;$d.Settings.ExecutionTimeLimit='PT90S';$d.Settings.AllowHardTerminate=$true
  $a=$d.Actions.Create(0);$a.Path='C:\Program Files\PowerShell\7\pwsh.exe';$a.WorkingDirectory=$m.code_root;$a.Arguments='-NoProfile -NonInteractive -File "'+(Join-Path $m.code_root 'Code/V2/start-host-v2.ps1')+'" -ManifestSha '+$ms+' -BindingSha '+$bs
  $plain=$null;try{$plain=[pscredential]::new($m.worker_account,$this.workerPassword).GetNetworkCredential().Password;$task=$s.GetFolder('\').RegisterTaskDefinition($m.task_name,$d,50,$worker,$plain,1,[DedicatedContract]::TaskSddl($worker))}finally{$plain=$null}
  $p=$task.Definition.Principal.UserId;if($p -notlike 'S-1-*'){$p=([Security.Principal.NTAccount]::new($p)).Translate([Security.Principal.SecurityIdentifier]).Value}
  @{disabled=(-not $task.Enabled);triggers=$task.Definition.Triggers.Count;retries=$task.Definition.Settings.RestartCount;principal=$p;run_level=$task.Definition.Principal.RunLevel;logon_type=$task.Definition.Principal.LogonType;seconds=[Xml.XmlConvert]::ToTimeSpan($task.Definition.Settings.ExecutionTimeLimit).TotalSeconds;instances=$task.Definition.Settings.MultipleInstances;sddl=$task.GetSecurityDescriptor(7);hard_terminate=$task.Definition.Settings.AllowHardTerminate;action_pinned=($task.Definition.Actions.Count -eq 1 -and $task.Definition.Actions.Item(1).Path -ceq $a.Path -and $task.Definition.Actions.Item(1).Arguments -ceq $a.Arguments -and $task.Definition.Actions.Item(1).WorkingDirectory -ceq $m.code_root)}
 }
 $api|Add-Member ScriptMethod Verify {param($m,$b,$payload,$bs)
  foreach($n in $payload.Keys){$p=Join-Path $m.code_root $n;[LimitlessV2Publish]::Verify($p,[DedicatedContract]::FsSddl('code',$false,$b.worker_sid,$b.bot_sid));if((Get-FileHash -LiteralPath $p).Hash.ToLowerInvariant() -cne $m.files_sha256[$n]){throw 'D3_INSTALLED_SHA'}}
  if((Get-FileHash -LiteralPath (Join-Path $m.code_root 'InstalledBinding.json')).Hash.ToLowerInvariant() -cne $bs){throw 'D3_BINDING_SHA'}
  foreach($name in @($m.bot_account,$m.worker_account)){$u=Get-LocalUser -Name $name;$g=@(Get-D3Groups $u.SID.Value);$expectedSid=if($name -ceq $m.bot_account){$b.bot_sid}else{$b.worker_sid};$enabled=($name -ceq $m.worker_account);if($u.SID.Value -cne $expectedSid -or $u.Enabled -ne $enabled -or $g.Count -ne 1 -or $g[0] -cne 'S-1-5-32-545' -or $u.AccountExpires -le (Get-Date) -or (@([DedicatedAccountPolicy]::Read($u.SID.Value)|Sort-Object)-join ',') -cne (@([DedicatedContract]::Rights|Sort-Object)-join ',')){throw 'D3_ACCOUNT_FINAL'}}
  [LimitlessV2Publish]::Verify($m.codex_home,[DedicatedContract]::FsSddl('auth',$true,$b.worker_sid,''));if(@(Get-ChildItem -LiteralPath $m.codex_home -Force -ErrorAction Stop).Count){throw 'D3_AUTH_NOT_EMPTY'}
 }
 $api|Add-Member ScriptMethod Record {param($r)
  $r.partial_new_sids=$this.created;[LimitlessV2Publish]::NewFile((Join-Path $this.consumeDirectory ('Result-'+[guid]::NewGuid().ToString('N')+'.json')),[Text.UTF8Encoding]::new($false).GetBytes(($r|ConvertTo-Json -Depth 12)),[DedicatedContract]::FsSddl('private',$false,'',''))
 }
 $api
}

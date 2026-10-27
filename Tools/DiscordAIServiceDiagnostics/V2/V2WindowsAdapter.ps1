# 실제 adapter는 별도 승인된 Install에서만 생성합니다. 오프라인 시험은 이 함수를 호출하지 않습니다.
function Get-V2WorkerSnapshot([string]$WorkerSid,[int[]]$AllowedPids=@()){
 $known=$true;$count=0;$launchers=0;$codes=@();$stage='SID';$shape=$null;$taskCounter=@{count=0}
 try{
  if(-not [LimitlessV2Contract]::Sid($WorkerSid)){throw 'V2_WORKER_SID_HOLD'}
  $stage='ACCOUNT_LOOKUP';$accounts=@(Get-CimInstance Win32_UserAccount -Filter "SID='$WorkerSid'" -ErrorAction Stop)
  if($accounts.Count -ne 1){throw 'V2_WORKER_ACCOUNT_QUERY_HOLD'}
  $seen=@{}
  $stage='PROCESS_ASSOCIATION'
  foreach($session in Get-CimAssociatedInstance -InputObject $accounts[0] -Association Win32_LoggedOnUser -ResultClassName Win32_LogonSession -ErrorAction Stop){
   foreach($p in Get-CimAssociatedInstance -InputObject $session -Association Win32_SessionProcess -ResultClassName Win32_Process -ErrorAction Stop){$seen[[int]$p.ProcessId]=$true}
  }
  foreach($id in $seen.Keys){if($id -notin $AllowedPids){$count++}}
  if($AllowedPids.Count -gt 0 -and @($AllowedPids|Where-Object{-not $seen.ContainsKey($_)}).Count){$known=$false}
  $stage='TASK_ENUMERATION';$s=New-Object -ComObject 'Schedule.Service';$s.Connect()
  function Visit-V2Tasks($Folder){
   foreach($t in $Folder.GetTasks(1)){
    $u=$t.Definition.Principal.UserId
    # Group principal/빈 인수를 사용자 이름으로 변환하지 않습니다. 불명은 HOLD합니다.
    $taskCounter.shape=@{user_type=if($null -eq $u){'null'}else{$u.GetType().FullName};user_length=([string]$u).Length;group_length=([string]$t.Definition.Principal.GroupId).Length;logon_type=[int]$t.Definition.Principal.LogonType}
    if($u -isnot [string] -or [string]::IsNullOrWhiteSpace($u)){throw 'V2_TASK_USER_PRINCIPAL_UNKNOWN_HOLD'}
    $sid=if($u -like 'S-1-*'){$u}else{
     $taskCounter.function='NTAccount.ctor(string)';$account=[Security.Principal.NTAccount]::new($u)
     $taskCounter.function='NTAccount.Translate(Type)';$account.Translate([Security.Principal.SecurityIdentifier]).Value
    }
    if($sid -ceq $WorkerSid -and $t.Enabled -and $t.Path -cne ('\'+[LimitlessV2Contract]::TaskName)){$taskCounter.count++}
   }
   foreach($f in $Folder.GetFolders(0)){Visit-V2Tasks $f}
  }
  Visit-V2Tasks ($s.GetFolder('\'));$launchers=$taskCounter.count
 }catch{$known=$false;$codes=[LimitlessV2Contract]::ErrorCodes($_.Exception)}
 return @{known=$known;foreign_processes=$count;other_launchable_tasks=$launchers;errors=$codes;stage=$stage;failed_function=if($known){$null}else{$taskCounter.function};argument_shape=$taskCounter.shape}
}
function New-V2WindowsAdapter([string]$Bundle,[Security.SecureString]$NewBotPassword,[pscredential]$WorkerCredential,[hashtable]$VerifiedPayloads=@{}){
 $preservation=if($VerifiedPayloads.ContainsKey('Deployment/V1Preservation.json')){[Text.Encoding]::UTF8.GetString($VerifiedPayloads['Deployment/V1Preservation.json'])|ConvertFrom-Json -AsHashtable}else{@{}}
 $operational=if($VerifiedPayloads.ContainsKey('Deployment/OperationalPins.json')){[Text.Encoding]::UTF8.GetString($VerifiedPayloads['Deployment/OperationalPins.json'])|ConvertFrom-Json -AsHashtable}else{@{}}
 $api=[pscustomobject]@{bundle=$Bundle;password=$NewBotPassword;credential=$WorkerCredential;consumeRoot=$null;readerContext=$null;createdSid=$null;bindingSha=$null;manifest=$null;preservation=$preservation;operationalPins=$operational;lastWorkerState=$null}
 $api|Add-Member ScriptMethod Context {
  $id=[Security.Principal.WindowsIdentity]::GetCurrent();$admin=([Security.Principal.WindowsPrincipal]::new($id)).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
  if(-not $admin){throw 'V2_ADMIN_REQUIRED'}
  $bootPath='C:\ProgramData\LimitlessRelay\boot-approval.json'
  if((Get-FileHash -LiteralPath $bootPath).Hash.ToLowerInvariant() -cne [LimitlessV2Contract]::BootSha){throw 'V2_BOOT_PIN_HOLD'}
  $boot=Get-Content -Raw -LiteralPath $bootPath|ConvertFrom-Json
  $worker=Get-LocalUser -Name 'LIMITLESSRelaySvc' -ErrorAction Stop
  if($worker.SID.Value -cne $boot.service_sid -or $this.credential.UserName -ine 'PAUL-PC\LIMITLESSRelaySvc'){throw 'V2_WORKER_BINDING_HOLD'}
  # Scheduler가 기존 Worker에 Batch 권한을 새로 부여할 여지를 만들지 않습니다.
  $rights=@([LimitlessV2PolicyReadback]::Read($worker.SID.Value)|Sort-Object -Unique)
  $groups=@(Get-LocalGroup|ForEach-Object{$g=$_;if(@(Get-LocalGroupMember -SID $g.SID -ErrorAction Stop|Where-Object {$_.SID.Value -ceq $worker.SID.Value}).Count){$g.SID.Value}})
  $batch=('SeBatchLogonRight' -cin $rights -and 'SeDenyBatchLogonRight' -cnotin $rights)
  $users=($worker.Enabled -and $groups.Count -eq 1 -and $groups[0] -ceq 'S-1-5-32-545')
  $this.readerContext=@{admin=$admin;admin_sid=$id.User.Value;worker_sid=$worker.SID.Value;worker_batch_present=$batch;worker_groups_users_only=$users;worker_rights=$rights;now=[DateTimeOffset]::UtcNow.ToUnixTimeSeconds()};return $this.readerContext
 }
 $api|Add-Member ScriptMethod Snapshot {
  $old=Get-LocalUser -Name 'LIMITLESSBotTrial' -ErrorAction Stop
  if($old.SID.Value -cne [LimitlessV2Contract]::OldBotSid -or $old.Enabled){throw 'V2_OLD_ACCOUNT_HOLD'}
  $inventory=$this.preservation;if($inventory.Count -ne 855){throw 'V2_PRESERVATION_MEMORY_PIN_HOLD'}
  $root='C:\Program Files\LimitlessRelayTrial';$found=@(Get-ChildItem -LiteralPath $root -File -Recurse -Force -ErrorAction Stop)
  if($found.Count -ne $inventory.Count){throw 'V2_OLD_LAYOUT_HOLD'}
  foreach($name in $inventory.Keys){
   $p=Join-Path $root $name;$item=Get-Item -Force -LiteralPath $p
   if($item.Attributes -band [IO.FileAttributes]::ReparsePoint -or (Get-FileHash -LiteralPath $p).Hash.ToLowerInvariant() -cne $inventory[$name]){throw 'V2_OLD_CODE_SHA_HOLD'}
  }
  $s=New-Object -ComObject 'Schedule.Service';$s.Connect();$folder=$s.GetFolder('\');$hash=@{}
  foreach($n in @('LIMITLESS-Bot-OnBoot','LIMITLESS-Art-OnBoot')){$hash[$n]=Get-V2Hash ([Text.Encoding]::UTF8.GetBytes($folder.GetTask($n).Xml))}
  $pins=$this.operationalPins
  foreach($n in $hash.Keys){if($hash[$n] -cne $pins[$n]){throw 'V2_ONBOOT_PIN_HOLD'}}
  $rootAcl=(Get-Acl -LiteralPath $root).Sddl
  [LimitlessV2Contract]::ExactSddl($rootAcl,[LimitlessV2Contract]::FsSddl('private',$true,'',''))
  $actualBoot=(Get-FileHash -LiteralPath 'C:\ProgramData\LimitlessRelay\boot-approval.json').Hash.ToLowerInvariant()
  if($actualBoot -cne [LimitlessV2Contract]::BootSha -or (Get-LocalUser -Name 'LIMITLESSRelaySvc').SID.Value -cne $this.readerContext.worker_sid){throw 'V2_OPERATIONAL_PIN_CHANGED_HOLD'}
  $homeHash=Get-V2Hash ([Text.Encoding]::UTF8.GetBytes([string]$env:CODEX_HOME))
  $workerRights=@([LimitlessV2PolicyReadback]::Read($this.readerContext.worker_sid)|Sort-Object -Unique)
  if(($workerRights -join ',') -cne ($this.readerContext.worker_rights -join ',')){throw 'V2_EXISTING_WORKER_RIGHTS_CHANGED_HOLD'}
  $fingerprint=Get-V2Hash ([Text.Encoding]::UTF8.GetBytes((@($old.SID.Value,$old.Enabled,$old.AccountExpires.ToUniversalTime().ToString('o'),$rootAcl,$hash['LIMITLESS-Bot-OnBoot'],$hash['LIMITLESS-Art-OnBoot'],$actualBoot,$this.readerContext.worker_sid,($workerRights -join ','),$homeHash) -join '|')))
  return @{known=$true;old_bot_disabled=$true;old_code_verified=$true;operational_preserved=$true;fingerprint=$fingerprint}
 }
 $api|Add-Member ScriptMethod Fresh {param($m)
  if((Test-Path -LiteralPath $m.code_root) -or (Test-Path -LiteralPath $m.state_root) -or @((Get-LocalUser)|Where-Object Name -IEQ $m.bot_account).Count){throw 'V2_FRESH_ONLY_HOLD'}
  $s=New-Object -ComObject 'Schedule.Service';$s.Connect();if(@($s.GetFolder('\').GetTasks(1)|Where-Object Name -IEQ $m.task_name).Count){throw 'V2_TASK_COLLISION_HOLD'}
  [LimitlessV2Publish]::Ancestors('C:\Program Files');[LimitlessV2Publish]::Ancestors('C:\ProgramData')
 }
 $api|Add-Member ScriptMethod WorkerState {$this.lastWorkerState=Get-V2WorkerSnapshot $this.readerContext.worker_sid;return $this.lastWorkerState}
 $api|Add-Member ScriptMethod Consume {param($m,$sha)
  $this.manifest=$m;$private=[LimitlessV2Contract]::FsSddl('private',$true,'','')
  # 승인 root는 승인자가 사전에 보호 생성해야 합니다. 여기서는 기존 root ACL을 완화하지 않습니다.
  [LimitlessV2Publish]::Verify([LimitlessV2Contract]::ApprovalRoot,$private)
  $this.consumeRoot=Join-Path ([LimitlessV2Contract]::ApprovalRoot) $m.approval_id
  [LimitlessV2Publish]::NewDirectory($this.consumeRoot,$private)
  $b=[Text.UTF8Encoding]::new($false).GetBytes((@{schema=2;epoch=$m.epoch;manifest_sha256=$sha;consumed=$true}|ConvertTo-Json -Compress))
  [LimitlessV2Publish]::NewFile((Join-Path $this.consumeRoot 'consumed.json'),$b,[LimitlessV2Contract]::FsSddl('private',$false,'',''))
 }
 $api|Add-Member ScriptMethod AcquireLease {param($sid)
  $p=Join-Path ([LimitlessV2Contract]::ApprovalRoot) ('Worker-'+$sid+'.lease')
  $sddl=[LimitlessV2Contract]::FsSddl('state',$false,$sid,'')
  if(-not (Test-Path -LiteralPath $p)){[LimitlessV2Publish]::NewFile($p,[byte[]]::new(0),$sddl)}
  [LimitlessV2Publish]::Verify($p,$sddl)
  return [IO.File]::Open($p,[IO.FileMode]::Open,[IO.FileAccess]::ReadWrite,[IO.FileShare]::None)
 }
 $api|Add-Member ScriptMethod CreateAccount {param($name,$package)
  $u=New-LocalUser -Name $name -Password $this.password -Disabled -AccountExpires (Get-Date).AddHours(24) -Description ('LIMITLESS_V2:'+ $package) -ErrorAction Stop
  $this.createdSid=$u.SID.Value
  Add-LocalGroupMember -SID 'S-1-5-32-545' -Member $u -ErrorAction Stop
  $again=Get-LocalUser -SID $u.SID -ErrorAction Stop
  $groups=@(Get-LocalGroup|ForEach-Object{$g=$_;if(@(Get-LocalGroupMember -SID $g.SID -ErrorAction Stop|Where-Object {$_.SID.Value -ceq $u.SID.Value}).Count){$g.SID.Value}})
  return @{sid=$again.SID.Value;name=$again.Name;disabled=(-not $again.Enabled);users_only=($groups.Count -eq 1 -and $groups[0] -ceq 'S-1-5-32-545');fresh=($null -eq $again.LastLogon -and $again.Description -ceq ('LIMITLESS_V2:'+ $package))}
 }
 $api|Add-Member ScriptMethod GrantBatch {param($sid,$worker,$admin)
  if($sid -cne $this.createdSid){throw 'V2_CREATED_SID_ONLY_HOLD'}
  [LimitlessV2Contract]::Distinct($sid,$worker,$admin)
  [LimitlessTrialAccountPolicyV1]::GrantNewBotBatch($sid,$this.createdSid,$worker,$admin)
  $rights=@([LimitlessV2PolicyReadback]::Read($sid));if($rights.Count -ne 1 -or $rights[0] -cne 'SeBatchLogonRight'){throw 'V2_NEW_SID_DIRECT_RIGHTS_HOLD'}
 }
 $api|Add-Member ScriptMethod Publish {param($m,$manifestBytes,$payloads,$bot,$worker)
  $codeSd=[LimitlessV2Contract]::FsSddl('code',$true,$worker,$bot);$fileSd=[LimitlessV2Contract]::FsSddl('code',$false,$worker,$bot)
  [LimitlessV2Publish]::NewDirectory($m.code_root,$codeSd)
  $dirs=@{}
  foreach($n in $payloads.Keys){$p=Split-Path (Join-Path $m.code_root $n) -Parent;while($p -cne $m.code_root){$dirs[$p]=$true;$p=Split-Path $p -Parent}}
  foreach($p in @($dirs.Keys|Sort-Object Length)){[LimitlessV2Publish]::NewDirectory($p,$codeSd)}
  foreach($n in @($payloads.Keys|Sort-Object)){[LimitlessV2Publish]::NewFile((Join-Path $m.code_root $n),$payloads[$n],$fileSd)}
  [LimitlessV2Publish]::NewFile((Join-Path $m.code_root 'HostManifest.json'),$manifestBytes,$fileSd)
  [LimitlessV2Publish]::NewDirectory($m.state_root,[LimitlessV2Contract]::FsSddl('state',$true,$worker,''))
 }
 $api|Add-Member ScriptMethod WriteBinding {param($bytes,$bot,$worker)
  [LimitlessV2Publish]::NewFile((Join-Path ([LimitlessV2Contract]::CodeRoot) 'InstalledBinding.json'),$bytes,[LimitlessV2Contract]::FsSddl('code',$false,$worker,$bot));$this.bindingSha=Get-V2Hash $bytes
 }
 $api|Add-Member ScriptMethod RegisterTask {param($m,$worker,$manifestSha,$bindingSha,$sddl,$flags)
  if($flags -ne 50){throw 'V2_CREATE_ONLY_FLAGS_HOLD'}
  $s=New-Object -ComObject 'Schedule.Service';$s.Connect();$d=$s.NewTask(0)
  $d.RegistrationInfo.Description='LIMITLESS_V2:'+ $m.package_id;$d.Principal.UserId=$worker;$d.Principal.LogonType=1;$d.Principal.RunLevel=0
  $d.Settings.Enabled=$false;$d.Settings.RestartCount=0;$d.Settings.MultipleInstances=2;$d.Settings.ExecutionTimeLimit='PT90S'
  $d.Settings.AllowHardTerminate=$true;$d.Settings.DisallowStartIfOnBatteries=$false;$d.Settings.StopIfGoingOnBatteries=$false
  $a=$d.Actions.Create(0);$a.Path='C:\Program Files\PowerShell\7\pwsh.exe';$a.WorkingDirectory=$m.code_root
  $a.Arguments='-NoProfile -NonInteractive -File "'+(Join-Path $m.code_root 'Code/V2/start-host-v2.ps1')+'" -ManifestSha '+$manifestSha+' -BindingSha '+$bindingSha
  $plain=$null
  try{$plain=$this.credential.GetNetworkCredential().Password;$t=$s.GetFolder('\').RegisterTaskDefinition($m.task_name,$d,50,$this.credential.UserName,$plain,1,$sddl)}finally{$plain=$null}
  if($null -eq $t){throw 'V2_TASK_RESPONSE_UNKNOWN_HOLD'}
  $principal=$t.Definition.Principal.UserId;if($principal -notlike 'S-1-*'){$principal=([Security.Principal.NTAccount]::new($principal)).Translate([Security.Principal.SecurityIdentifier]).Value}
  return @{name=$t.Name;disabled=(-not $t.Enabled);triggers=$t.Definition.Triggers.Count;run_level=$t.Definition.Principal.RunLevel;retries=$t.Definition.Settings.RestartCount;principal=$principal;time_limit_seconds=[Xml.XmlConvert]::ToTimeSpan($t.Definition.Settings.ExecutionTimeLimit).TotalSeconds;multiple_instances=$t.Definition.Settings.MultipleInstances;allow_hard_terminate=$t.Definition.Settings.AllowHardTerminate;action_pinned=($t.Definition.Actions.Count -eq 1 -and $t.Definition.Actions.Item(1).Path -ieq $a.Path -and $t.Definition.Actions.Item(1).Arguments -ceq $a.Arguments -and $t.Definition.Actions.Item(1).WorkingDirectory -ieq $m.code_root);sddl=$t.GetSecurityDescriptor(7)}
 }
 $api|Add-Member ScriptMethod Verify {param($m,$binding,$payloads,$bindingSha)
  foreach($n in $payloads.Keys){$p=Join-Path $m.code_root $n;[LimitlessV2Publish]::Verify($p,[LimitlessV2Contract]::FsSddl('code',$false,$binding.worker_sid,$binding.bot_sid));if((Get-FileHash -LiteralPath $p).Hash.ToLowerInvariant() -cne $m.files_sha256[$n]){throw 'V2_INSTALLED_SHA_HOLD'}}
  if((Get-FileHash -LiteralPath (Join-Path $m.code_root 'InstalledBinding.json')).Hash.ToLowerInvariant() -cne $bindingSha){throw 'V2_BINDING_READBACK_HOLD'}
  [LimitlessV2Publish]::Verify($m.state_root,[LimitlessV2Contract]::FsSddl('state',$true,$binding.worker_sid,''))
  $u=Get-LocalUser -SID ([Security.Principal.SecurityIdentifier]::new($binding.bot_sid));if($u.Enabled -or $u.Name -cne $m.bot_account -or $u.AccountExpires -le (Get-Date) -or $u.AccountExpires -gt (Get-Date).AddHours(24).AddMinutes(1)){throw 'V2_DISABLED_ACCOUNT_HOLD'}
  $rights=@([LimitlessV2PolicyReadback]::Read($binding.bot_sid));if($rights.Count -ne 1 -or $rights[0] -cne 'SeBatchLogonRight'){throw 'V2_INSTALLED_RIGHTS_HOLD'}
 }
 $api|Add-Member ScriptMethod Record {param($result)
  $p=Join-Path $this.consumeRoot ('Result-'+[guid]::NewGuid().ToString('N')+'.json')
  [LimitlessV2Publish]::NewFile($p,[Text.UTF8Encoding]::new($false).GetBytes(($result|ConvertTo-Json -Depth 12)),[LimitlessV2Contract]::FsSddl('private',$false,'',''))
 }
 return $api
}

param([ValidateSet('Inspect','Install')][string]$Action='Inspect',[switch]$ConfirmR2AccountsTaskInstallOnce)
$ErrorActionPreference='Stop';Set-StrictMode -Version Latest
$phase='R2_PIN_BOOTSTRAP';$stageSession=$null;$rootHandle=$null;$r1Handle=$null;$dummy=$null;$nativeApi=$null
$p=@{package='3effaac30bf44760986fee9e6019b283';id='dedicated-install-3effaac30bf44760986fee9e6019b283';epoch='20261027-dedicated-3effaac30bf44760986fee9e6019b283';ms='7c8410145ed84f06fc9901e93f78e18d0cc103ad89757172e08c84f91d8a3976';ins='83c97d781fb2d9aed797b41ea6bc799b83e939ad3ffa6f7af6a2d26f1080d68f';zip='4efa0aa8a38b32fa40fa5a2dfb4042e028eeb07b1ab8d73c713a1d80efe028d0'}
function Get-R2InstallSha256([byte[]]$bytes){[Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($bytes)).ToLowerInvariant()}
function Get-R2OldEvidenceFingerprint($Fs,[string]$Root,[string]$SkipFile,[string]$SkipDir,[string]$SkipJournal){
 $parts=[Collections.Generic.List[string]]::new();$pending=[Collections.Generic.Stack[string]]::new();$pending.Push($Root);$count=0
 while($pending.Count){$dir=$pending.Pop();foreach($file in $Fs.Children($dir)){
  if($file -ceq $SkipFile -or $file -ceq $SkipDir -or $file -ceq $SkipJournal){continue};if(++$count -gt 512){throw 'R2_OLD_EVIDENCE_BOUND'}
  $h=$Fs.Open($file);try{$i=$h.Info();[LimitlessV2Staging]::Protected($i,$file,$i.Directory);$part=[R2StageBoundary]::Stamp($i);if($i.Directory){$pending.Push($file)}else{$bytes=$h.Read();if($bytes.Length -gt 1MB){throw 'R2_OLD_RECORD_BOUND'};$part+='|'+(Get-R2InstallSha256 $bytes)};$parts.Add($part)}finally{$h.Dispose()}
 }};Get-R2InstallSha256 ([Text.Encoding]::UTF8.GetBytes((@($parts|Sort-Object)-join "`n")))
}
try{
 if($PSVersionTable.PSVersion.Major -ne 7 -or -not [Environment]::Is64BitProcess){throw 'R2_PS7_X64'}
 if('DedicatedContract' -as [type]){throw 'R2_FRESH_PROCESS'}
 if($Action -eq 'Install' -and -not $ConfirmR2AccountsTaskInstallOnce){throw 'R2_EXPLICIT_INSTALL_ONLY_REQUIRED'}
 $zipPath='F:\study\codex\Project-Limitless\Temp\Session0DedicatedV2Candidate20261027-r2.zip'
 if((Get-Item -LiteralPath $zipPath).Length -gt 128MB){throw 'R2_ZIP_BOUND'};$zb=[IO.File]::ReadAllBytes($zipPath)
 if((Get-R2InstallSha256 $zb) -cne $p.zip){throw 'R2_ZIP_PIN'}
 $mem=[IO.MemoryStream]::new($zb,$false);$z=[IO.Compression.ZipArchive]::new($mem,[IO.Compression.ZipArchiveMode]::Read)
 try{
  function Read-R2InstallEntry([string]$name){$a=@($z.Entries|Where-Object FullName -CEQ $name);if($a.Count -ne 1 -or $a[0].Length -gt 64MB){throw 'R2_ENTRY'};$s=$a[0].Open();$o=[IO.MemoryStream]::new();try{$s.CopyTo($o);return ,$o.ToArray()}finally{$s.Dispose();$o.Dispose()}}
  $mb=Read-R2InstallEntry 'HostManifest.json';if((Get-R2InstallSha256 $mb) -cne $p.ms){throw 'R2_MANIFEST_PIN'};$m=[Text.Encoding]::UTF8.GetString($mb)|ConvertFrom-Json -AsHashtable
  $cs=Read-R2InstallEntry 'Code/V2/InstallAssembly.cs';if((Get-R2InstallSha256 $cs) -cne $m.files_sha256['Code/V2/InstallAssembly.cs']){throw 'R2_ASSEMBLY_PIN'}
 }finally{$z.Dispose();$mem.Dispose()}
 $boundary=[IO.File]::ReadAllBytes('F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\Tools\DiscordAIServiceDiagnostics\R2Staging\R2StageBoundary.cs');if((Get-R2InstallSha256 $boundary) -cne '7203eec5d26fe02b18fb9379e1ad12cb36fb8fea7169a58c6987e012f4cf3664'){throw 'R2_BOUNDARY_PIN'}
 Add-Type -TypeDefinition ([Text.Encoding]::UTF8.GetString($cs)+"`n"+[Text.Encoding]::UTF8.GetString($boundary))
 $id='';$all=[LimitlessV2Staging]::Archive($zb,$p.ms,$p.ins,$p.zip,766,[ref]$id)
 if($id -cne $p.package -or $m.approval_id -cne $p.id -or $m.epoch -cne $p.epoch -or $m.pipe_name -cne ('LimitlessDedicatedV2-'+$p.package)){throw 'R2_PACKAGE_BINDING'}
 $payloads=@{};foreach($name in $m.files_sha256.Keys){$payloads[$name]=$all[$name]}
 $phase='ACTUAL_NATIVE_ADAPTER_CONSTRUCTOR'
 . ([scriptblock]::Create([Text.Encoding]::UTF8.GetString($payloads['Code/V2/DedicatedCore.ps1'])))
 . ([scriptblock]::Create([Text.Encoding]::UTF8.GetString($payloads['Code/V2/DedicatedWindows.ps1'])))
 $dummy=[Security.SecureString]::new();$dummy.MakeReadOnly();$nativeApi=New-D3WindowsApi $m $payloads $dummy $dummy
 if($null -eq $nativeApi.PSObject.Properties['consumeDirectory'] -or $null -ne $nativeApi.PSObject.Properties['consume']){throw 'R2_CONSUME_PROPERTY_CONTRACT'}
 foreach($name in @('Context','Snapshot','Fresh','Consume','InstallLease','CreateUser','Rights','Publish','Binding','EnableWorker','Task','Verify','Record')){if($nativeApi.PSObject.Members[$name].MemberType -ne 'ScriptMethod'){throw 'R2_NATIVE_METHOD_CONTRACT'}}
 $flow=[IO.File]::ReadAllBytes('F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\Tools\DiscordAIServiceDiagnostics\R2Install\R2InstallControl.ps1');if((Get-R2InstallSha256 $flow) -cne 'e51dc17dab7ab8c07a50ba5d8b809c8752ac15dfbbb34d7f7e9ed9eb853792e5'){throw 'R2_FLOW_PIN'}
 . ([scriptblock]::Create([Text.Encoding]::UTF8.GetString($flow)))
 if($Action -eq 'Inspect'){@{schema=3;kind='R2_INSTALL_ONLY';status='R2_OFFLINE_ENTRY_NATIVE_CONSTRUCTOR_PASS_NOT_INSTALLED';package_id=$id;payload_count=766;native_methods=13;native_constructor=$true;account_calls=0;task_calls=0;approval_writes=0;install_calls=0;password_reads=0}|ConvertTo-Json}
 else{
  $phase='R2_PROTECTED_STAGE_READONLY';$fs=[V2NativeStageFs]::new();$bundle=[R2StageBoundary]::Target+'\Bundle';$stageSession=[LimitlessV2Staging]::Verify($fs,$bundle,$p.ms,$p.ins,$p.zip,766);[R2StageBoundary]::ExactCopies($fs,$stageSession,$zb)
  foreach($name in $payloads.Keys){if(-not [Linq.Enumerable]::SequenceEqual[byte]($payloads[$name],$stageSession.Bytes[$name])){throw 'R2_SEALED_STAGE_MISMATCH'}}
  $root=[DedicatedContract]::ApprovalRoot;$rootHandle=$fs.Open($root);[LimitlessV2Staging]::Protected($rootHandle.Info(),$root,$true)
  $r1Handle=$fs.Open([R2StageBoundary]::R1);[LimitlessV2Staging]::Protected($r1Handle.Info(),[R2StageBoundary]::R1,$true)
  $ctl=[pscustomobject]@{api=$nativeApi;fs=$fs;session=$stageSession;root=$root;r1=$r1Handle;r1Stamp=[R2StageBoundary]::Stamp($r1Handle.Info());pins=$p;m=$m;approval=(Join-Path $root ($p.id+'.json'));consumption=(Join-Path $root $p.id);journal=(Join-Path $root ('Entry-'+$p.package+'.json'));before=$null;oldEvidence=$null;botPw=$null;workerPw=$null}
  $ctl|Add-Member ScriptMethod NoR2Records {
   foreach($path in @($this.approval,$this.consumption,$this.journal)){if($this.fs.Exists($path)){throw 'R2_EXISTING_APPROVAL_CONSUMPTION_HOLD'}}
  }
  $ctl|Add-Member ScriptMethod OldFingerprint {Get-R2OldEvidenceFingerprint $this.fs $this.root $this.approval $this.consumption $this.journal}
  $ctl|Add-Member ScriptMethod Gates {
   $c=$this.api.Context();if(-not $c.admin){throw 'R2_ADMIN_REQUIRED'}
   if([Console]::IsInputRedirected -or [Console]::IsOutputRedirected){throw 'R2_REAL_CONSOLE_REQUIRED'}
   $this.NoR2Records();$this.api.Fresh($this.m);$this.before=$this.api.Snapshot();if(-not $this.before.known -or -not $this.before.preserved){throw 'R2_OLD_ASSETS_UNKNOWN'};$this.oldEvidence=$this.OldFingerprint()
   @{admin=$true;console=$true;stage=$true;native_constructor=$true;fresh_namespace=$true;r2_records_absent=$true;old_preserved=$true}
  }
  $ctl|Add-Member ScriptMethod Passwords {
   $this.botPw=Read-Host 'r2 새 Bot 암호 (로컬 보안 입력)' -AsSecureString;$this.workerPw=Read-Host 'r2 새 Worker 암호 (로컬 보안 입력)' -AsSecureString
   if($this.botPw.Length -eq 0 -or $this.workerPw.Length -eq 0){throw 'R2_EMPTY_PASSWORD_HOLD'}
   $this.api.botPassword=$this.botPw;$this.api.workerPassword=$this.workerPw
  }
  $ctl|Add-Member ScriptMethod Revalidate {
   $this.session.Recheck();$this.NoR2Records();$this.api.Fresh($this.m)
   $now=$this.api.Snapshot();if(-not $now.known -or $now.fingerprint -cne $this.before.fingerprint -or $this.OldFingerprint() -cne $this.oldEvidence -or [R2StageBoundary]::Stamp($this.r1.Info()) -cne $this.r1Stamp){throw 'R2_OLD_ASSETS_CHANGED'}
   $this.api.Context()
  }
  $ctl|Add-Member ScriptMethod Approve {param($raw)
   [LimitlessV2Publish]::NewFile($this.approval,[Text.UTF8Encoding]::new($false).GetBytes($raw),[DedicatedContract]::FsSddl('private',$false,'',''))
  }
  $ctl|Add-Member ScriptMethod Install {param($bytes,$payload,$raw,$zs)Invoke-D3Install $this.api $bytes $payload $raw $zs}
  $ctl|Add-Member ScriptMethod Audit {param($native)
   $this.session.Recheck();if($this.OldFingerprint() -cne $this.oldEvidence -or [R2StageBoundary]::Stamp($this.r1.Info()) -cne $this.r1Stamp){throw 'R2_R1_EVIDENCE_CHANGED'}
   $bound=Get-Content -LiteralPath (Join-Path $this.m.code_root 'InstalledBinding.json') -Raw|ConvertFrom-Json -AsHashtable
   $accounts=@();foreach($role in @('bot','worker')){$name=if($role -ceq 'bot'){$this.m.bot_account}else{$this.m.worker_account};$u=Get-LocalUser -Name $name -ErrorAction Stop;$sid=$bound[$role+'_sid'];$groups=@(Get-D3Groups $u.SID.Value);$rights=@([DedicatedAccountPolicy]::Read($u.SID.Value)|Sort-Object);$enabled=($role -ceq 'worker');if($u.SID.Value -cne $sid -or $u.Enabled -ne $enabled -or $groups.Count -ne 1 -or $groups[0] -cne 'S-1-5-32-545' -or ($rights -join ',') -cne ((@([DedicatedContract]::Rights|Sort-Object))-join ',')){throw 'R2_ACCOUNT_AUDIT_HOLD'};$accounts+=@{name=$name;sid=$sid;enabled=$u.Enabled;group_sids=$groups;direct_rights=$rights}}
   $paths=@();foreach($pair in @(@('code',$this.m.code_root,'code'),@('state',$this.m.state_root,'container'),@('auth',$this.m.codex_home,'auth'))){$bot=if($pair[2] -ceq 'code'){$bound.bot_sid}else{''};[LimitlessV2Publish]::Verify($pair[1],[DedicatedContract]::FsSddl($pair[2],$true,$bound.worker_sid,$bot));$sd=[Security.AccessControl.RawSecurityDescriptor]::new((Get-Acl -LiteralPath $pair[1]).Sddl);$paths+=@{kind=$pair[0];path=$pair[1];owner_sid=$sd.Owner.Value;group_sid=$sd.Group.Value;sddl=$sd.GetSddlForm([Security.AccessControl.AccessControlSections]14)}}
   if(@(Get-ChildItem -LiteralPath $this.m.codex_home -Force -ErrorAction Stop).Count){throw 'R2_AUTH_NOT_EMPTY'}
   $manifestHash=(Get-FileHash -LiteralPath (Join-Path $this.m.code_root 'HostManifest.json')).Hash.ToLowerInvariant();$bindingHash=(Get-FileHash -LiteralPath (Join-Path $this.m.code_root 'InstalledBinding.json')).Hash.ToLowerInvariant();if($manifestHash -cne $this.pins.ms -or $bindingHash -cne $native.binding_sha256 -or @(Get-ChildItem -LiteralPath $this.m.code_root -Recurse -File -Force -ErrorAction Stop).Count -ne 768){throw 'R2_CODE_BINDING_COUNT_HOLD'}
   $scheduler=New-Object -ComObject Schedule.Service;$scheduler.Connect();$task=$scheduler.GetFolder('\').GetTask($this.m.task_name);$d=$task.Definition;$principal=$d.Principal.UserId;if($principal -notlike 'S-1-*'){$principal=([Security.Principal.NTAccount]::new($principal)).Translate([Security.Principal.SecurityIdentifier]).Value}
   $sddl=$task.GetSecurityDescriptor(7);[DedicatedContract]::ExactSddl($sddl,[DedicatedContract]::TaskSddl($bound.worker_sid))
   if($task.Enabled -or $d.Triggers.Count -ne 0 -or $d.Settings.RestartCount -ne 0 -or $principal -cne $bound.worker_sid -or $d.Principal.LogonType -ne 1 -or $d.Principal.RunLevel -ne 0 -or $d.Settings.MultipleInstances -ne 2 -or [Xml.XmlConvert]::ToTimeSpan($d.Settings.ExecutionTimeLimit).TotalSeconds -ne 90){throw 'R2_TASK_AUDIT_HOLD'}
   if(-not $this.fs.Exists((Join-Path $this.consumption 'consumed.json'))){throw 'R2_CONSUMPTION_NOT_FOUND'}
   @{verified=$true;accounts=$accounts;paths=$paths;auth_empty=$true;task=@{name=$this.m.task_name;principal_sid=$principal;enabled=$task.Enabled;triggers=$d.Triggers.Count;logon_type=$d.Principal.LogonType;run_level=$d.Principal.RunLevel;sddl=$sddl;retries=$d.Settings.RestartCount;seconds=90;multiple_instances=$d.Settings.MultipleInstances};manifest_sha256=$manifestHash;binding_sha256=$bindingHash;code_file_count=768;payload_sha_verified_by_native_core=$true;r1_approval_evidence_preserved=$true;r1_stage_directory_preserved=$true;consumed=$true;worker_effective_token_verified=$false;session0_ipc_verified=$false}
  }
  $ctl|Add-Member ScriptMethod Record {param($record)
   $record.partial_new_sids=$this.api.created;[LimitlessV2Publish]::NewFile($this.journal,[Text.UTF8Encoding]::new($false).GetBytes(($record|ConvertTo-Json -Depth 18)),[DedicatedContract]::FsSddl('private',$false,'',''))
  }
  $ctl|Add-Member ScriptMethod ClearPasswords {
   $this.api.botPassword=$null;$this.api.workerPassword=$null;if($this.botPw){$this.botPw.Dispose()};if($this.workerPw){$this.workerPw.Dispose()}
  }
  $phase='R2_EXPLICIT_INSTALL_ONCE';Invoke-R2InstallOnce $ctl $mb $payloads $p|ConvertTo-Json -Depth 20
 }
}catch{@{schema=3;kind='R2_INSTALL_ONLY';status='HOLD';phase=$phase;errors=if('DedicatedContract' -as [type]){[DedicatedContract]::ErrorCodes($_.Exception)}else{@{type=$_.Exception.GetType().FullName;hresult=$_.Exception.HResult}};automatic_retries=0;partial_assets_preserve_no_retry=$true}|ConvertTo-Json -Depth 12}
finally{if($nativeApi){$nativeApi.botPassword=$null;$nativeApi.workerPassword=$null};if($dummy){$dummy.Dispose()};foreach($h in @($stageSession,$r1Handle,$rootHandle)){if($h){try{$h.Dispose()}catch{@{schema=3;kind='R2_INSTALL_CLOSE';status='HOLD';errors=@{type=$_.Exception.GetType().FullName;hresult=$_.Exception.HResult};automatic_retries=0}|ConvertTo-Json}}}}

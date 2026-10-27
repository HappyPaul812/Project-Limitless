param([ValidateSet('Preflight','Stage','Install')][string]$Step='Preflight',[string]$ApprovalScope)
# 고정 r1의 외부 관리자 절차입니다. 기본값은 읽기 전용이며 후보 바이트는 수정하지 않습니다.
$ErrorActionPreference='Stop';Set-StrictMode -Version Latest
$zipPath='F:\study\codex\Project-Limitless\Temp\Session0DedicatedV2Candidate20261027-r1.zip'
$ms='43f6afcc91eae196e015393670cd9cd4ff37194099b313c2b901177226cd32e3'
$ins='83c97d781fb2d9aed797b41ea6bc799b83e939ad3ffa6f7af6a2d26f1080d68f'
$zs='be58f8cdbe0befdcc64cd67287865c6cd6fc2611b4a706d26cda022a8014ca25'
$package='a5af3087b78b45d1baae46067d5b7ac9'
$bundle='C:\Program Files\LimitlessRelayDedicatedV2Staging20261027\'+$package+'\Bundle'
$checks=[Collections.Generic.List[object]]::new();$session=$null;$phase='LOAD_FIXED_ARCHIVE'
$result=@{schema=3;step=$Step;status='HOLD';checks=$checks;actual_creation_attempted=$false;automatic_retries=0;ipc_calls=0;model_calls=0;deferred=@('OS_ISSUED_NEW_SIDS','NEW_ACCOUNT_GROUPS_EFFECTIVE_BATCH_RIGHTS','NEW_WORKER_TOKEN_LOGON_SESSION','NEW_SID_GROUP_TASK_APPLICABILITY','INSTALLED_OWNER_DACL_SHA_TASK','IPC_EXCLUSIVITY_JOB_CLEANUP');v1_status='REFERENCE_VERIFICATION_HOLD'}
function Hash-R1([byte[]]$b){[Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($b)).ToLowerInvariant()}
function Check-R1([string]$name,[scriptblock]$fn){
 try{& $fn;$checks.Add(@{name=$name;status='PASS'})}catch{$checks.Add(@{name=$name;status='HOLD';errors=[DedicatedContract]::ErrorCodes($_.Exception)})}
}
function Assert-R1Absent([string]$p){try{$null=Get-Item -LiteralPath $p -Force -ErrorAction Stop;throw 'R1_RESOURCE_EXISTS'}catch [Management.Automation.ItemNotFoundException]{} }
function Parent-R1([string]$p,[bool]$direct){
 $h=[V2NativeStageFs]::new().Open($p)
 try{
  $i=$h.Info();[LimitlessV2Staging]::Basic($i,$p,$true)
  $sd=[Security.AccessControl.RawSecurityDescriptor]::new($i.Sddl)
  $allowed=@('S-1-5-18','S-1-5-32-544','S-1-5-80-956008885-3418522649-1831038044-1853292631-2271478464')
  if($null -eq $sd.Owner -or $sd.Owner.Value -notin $allowed -or $null -eq $sd.DiscretionaryAcl){throw 'R1_PARENT_OWNER_DACL'}
  foreach($ace in $sd.DiscretionaryAcl){
   if($ace -isnot [Security.AccessControl.CommonAce] -or $ace.IsCallback){throw 'R1_PARENT_ACE_UNKNOWN'}
   if($ace.AceQualifier -ne 'AccessAllowed' -or ($ace.AceFlags -band [Security.AccessControl.AceFlags]::InheritOnly) -or $ace.SecurityIdentifier.Value -in $allowed){continue}
   [long]$bad=0x10000000L -bor 0x40000000L -bor 0x10000L -bor 0x40L -bor 0x40000L -bor 0x80000L
   if($direct){$bad=$bad -bor 0x2L -bor 0x4L -bor 0x10L -bor 0x100L}
   if(([long]$ace.AccessMask -band $bad) -ne 0){throw 'R1_PARENT_WRITER'}
  }
 }finally{$h.Dispose()}
}
try{
 if($PSVersionTable.PSVersion.Major -ne 7 -or -not [Environment]::Is64BitProcess){throw 'R1_POWERSHELL7_X64_REQUIRED'}
 if('DedicatedContract' -as [type]){throw 'R1_FRESH_NO_PROFILE_PROCESS_REQUIRED'}
 $sf=[IO.File]::Open($zipPath,[IO.FileMode]::Open,[IO.FileAccess]::Read,[IO.FileShare]::Read)
 try{if($sf.Length -gt 128MB){throw 'R1_ZIP_BOUNDS'};$memory=[IO.MemoryStream]::new();try{$sf.CopyTo($memory);$zb=$memory.ToArray()}finally{$memory.Dispose()}}finally{$sf.Dispose()}
 if((Hash-R1 $zb) -cne $zs){throw 'R1_ZIP_PIN'}
 $memory=[IO.MemoryStream]::new($zb,$false);$zip=[IO.Compression.ZipArchive]::new($memory,[IO.Compression.ZipArchiveMode]::Read)
 try{
  if($zip.Entries.Count -ne 767){throw 'R1_ENTRY_COUNT'}
  function Read-R1Entry([string]$name){$entries=@($zip.Entries|Where-Object FullName -CEQ $name);if($entries.Count -ne 1 -or $entries[0].Length -gt 64MB){throw 'R1_ENTRY'};$s=$entries[0].Open();$d=[IO.MemoryStream]::new();try{$s.CopyTo($d);return ,$d.ToArray()}finally{$s.Dispose();$d.Dispose()}}
  $mb=Read-R1Entry 'HostManifest.json';$ib=Read-R1Entry 'Deployment/install-session0-v2.ps1'
  if((Hash-R1 $mb) -cne $ms -or (Hash-R1 $ib) -cne $ins){throw 'R1_FIXED_BYTES'}
  $m=[Text.Encoding]::UTF8.GetString($mb)|ConvertFrom-Json -AsHashtable
  $assembly=Read-R1Entry 'Code/V2/InstallAssembly.cs';if((Hash-R1 $assembly) -cne $m.files_sha256['Code/V2/InstallAssembly.cs']){throw 'R1_ASSEMBLY_PIN'}
 }finally{$zip.Dispose();$memory.Dispose()}
 Add-Type -TypeDefinition ([Text.Encoding]::UTF8.GetString($assembly))
 $id='';$all=[LimitlessV2Staging]::Archive($zb,$ms,$ins,$zs,766,[ref]$id)
 if($id -cne $package){throw 'R1_PACKAGE_ID'}
 $checks.Add(@{name='R1_FIXED_ZIP_MANIFEST_INSTALLER_766_PLUS_1';status='PASS'})
 $phase='READONLY_PRECONDITIONS'
 $identity=[Security.Principal.WindowsIdentity]::GetCurrent()
 $admin=([Security.Principal.WindowsPrincipal]::new($identity)).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
 $result.admin=$admin
 Check-R1 'ADMIN_POWERSHELL7_X64' {if(-not $admin){throw 'R1_ADMIN_REQUIRED'}}
 Check-R1 'NEW_ACCOUNT_NAMES_ABSENT' {$local=@(Get-LocalUser -ErrorAction Stop);if(@($local|Where-Object Name -In @($m.bot_account,$m.worker_account)).Count){throw 'R1_ACCOUNT_EXISTS'}}
 Check-R1 'NEW_CODE_STATE_ABSENT' {Assert-R1Absent $m.code_root;Assert-R1Absent $m.state_root}
 Check-R1 'NEW_TASK_NAME_ABSENT' {$scheduler=New-Object -ComObject Schedule.Service;$scheduler.Connect();if(@($scheduler.GetFolder('\').GetTasks(1)|Where-Object Name -IEQ $m.task_name).Count){throw 'R1_TASK_EXISTS'}}
 Check-R1 'NEW_PIPE_NAMES_ABSENT_NO_CONNECT' {if(@([IO.Directory]::GetFiles('\\.\pipe\')|Where-Object{[IO.Path]::GetFileName($_).StartsWith($m.pipe_name,[StringComparison]::OrdinalIgnoreCase)}).Count){throw 'R1_PIPE_EXISTS'}}
 Check-R1 'SYSTEM_ROOT_PARENT_SAFE' {Parent-R1 'C:\' $false}
 Check-R1 'STAGING_CODE_PARENT_SAFE' {Parent-R1 'C:\Program Files' $true}
 Check-R1 'STATE_APPROVAL_PARENT_NO_TAKEOVER' {Parent-R1 'C:\ProgramData' $false}
 Check-R1 'OLD_WORKER_V1_ACCOUNT_ID_STATE' {$old=Get-LocalUser -Name LIMITLESSRelaySvc -ErrorAction Stop;$v1=Get-LocalUser -Name LIMITLESSBotTrial -ErrorAction Stop;if($old.SID.Value -cne [DedicatedContract]::OldWorker -or -not $old.Enabled -or $v1.SID.Value -cne [DedicatedContract]::OldBot -or $v1.Enabled){throw 'R1_OLD_ID_STATE_HOLD'}}
 $result.install_preconditions_status=if(@($checks|Where-Object status -eq HOLD).Count){'HOLD'}else{'PASS_READONLY_SNAPSHOT'}
 $result.runtime_status='HOLD_NEW_TOKEN_GROUP_EXCLUSIVITY_NOT_VERIFIED'
 if($Step -eq 'Preflight'){
  Check-R1 'NEW_STAGING_ROOT_ABSENT' {Assert-R1Absent $m.staging_root}
  Check-R1 'NEW_APPROVAL_ROOT_ABSENT' {Assert-R1Absent $m.approval_root}
  $result.status=if(@($checks|Where-Object status -eq HOLD).Count){'HOLD'}else{'PREFLIGHT_PASS_POSTINSTALL_CHECKS_PENDING'}
 }else{
  if(@($checks|Where-Object status -eq HOLD).Count){throw 'R1_PRECONDITIONS_HOLD'}
  if($Step -eq 'Stage'){
   if($ApprovalScope -cne 'STAGE_R1_ONLY_NO_INSTALL'){throw 'R1_SEPARATE_STAGE_APPROVAL_REQUIRED'}
   Assert-R1Absent $m.staging_root
   $phase='STAGE_CREATE_NEW_RESTRICTED_DACL';$result.actual_creation_attempted=$true
   $session=[LimitlessV2Staging]::Create([V2NativeStageFs]::new(),$zipPath,$ms,$ins,$zs,766);$session.Recheck()
   $result.status='STAGING_CREATED_VERIFIED_NOT_INSTALLED';$result.bundle=$session.Bundle
  }else{
   if($ApprovalScope -cne 'INSTALL_R1_ONCE_TWO_NEW_ACCOUNTS_BATCH_WORKER_DISABLED_TASK'){throw 'R1_SEPARATE_INSTALL_APPROVAL_REQUIRED'}
   $phase='STAGING_INSTALL_TIME_REVERIFY'
   $session=[LimitlessV2Staging]::Verify([V2NativeStageFs]::new(),$bundle,$ms,$ins,$zs,766)
   # 승인 저장소는 독립 CREATE_NEW이며, 이미 있으면 임의 재사용·삭제·재시도하지 않습니다.
   Assert-R1Absent $m.approval_root
   $phase='PROTECTED_INSTALL_APPROVAL_CREATE_NEW';$result.actual_creation_attempted=$true
   [LimitlessV2Publish]::NewDirectory($m.approval_root,[DedicatedContract]::FsSddl('private',$true,'',''))
   $approvalPath=Join-Path $m.approval_root ($m.approval_id+'.json')
   $now=[datetimeoffset]::UtcNow.ToUnixTimeSeconds()
   $approval=@{schema=3;kind='DEDICATED_INSTALL_ONCE';approved=$true;approval_id=$m.approval_id;epoch=$m.epoch;manifest_sha256=$ms;installer_sha256=$ins;zip_sha256=$zs;admin_sid=$identity.User.Value;expires_unix=$now+900;preserve_existing=$true;create_bot=$m.bot_account;create_worker=$m.worker_account;worker_install_state='ENABLED_BATCH_ONLY'}
   $raw=$approval|ConvertTo-Json -Compress
   [DedicatedContract]::InstallApproval($raw,$ms,$ins,$zs,$m.approval_id,$m.epoch,$identity.User.Value,$now)
   [LimitlessV2Publish]::NewFile($approvalPath,[Text.UTF8Encoding]::new($false).GetBytes($raw),[DedicatedContract]::FsSddl('private',$false,'',''))
   $session.Recheck();$phase='FIXED_INSTALLER_FRESH_CHILD'
   # 부모 프로세스는 staging handle을 유지합니다. 새 PowerShell은 고정 설치기에서만 로컬 SecureString을 입력받습니다.
   $json=& 'C:\Program Files\PowerShell\7\pwsh.exe' -NoLogo -NoProfile -File (Join-Path $bundle 'Deployment/install-session0-v2.ps1') -Action Install -Bundle $bundle -ManifestSha $ms -InstallerSha $ins -ZipSha $zs -ApprovalPath $approvalPath -ConfirmSystemChanges
   $native=$json|ConvertFrom-Json -AsHashtable;$result.installer_result=$native
   if($native.status -cne 'DEDICATED_INSTALLED_BOT_DISABLED_WORKER_BATCH_ONLY_TASK_DISABLED'){throw 'R1_INSTALLER_RESULT_HOLD'}
   $session.Recheck();$result.installer_result=$native;$result.status='INSTALL_REPORTED_COMPLETE_POSTINSTALL_AUDIT_REQUIRED';$result.runtime_status='HOLD_NO_START'
  }
 }
}catch{
 $result.status='HOLD';$result.phase=$phase;$result.partial_assets_preserve_no_retry=$true
 $result.errors=if('DedicatedContract' -as [type]){[DedicatedContract]::ErrorCodes($_.Exception)}else{@{type=$_.Exception.GetType().FullName;hresult=$_.Exception.HResult}}
}finally{if($session){try{$session.Dispose()}catch{$result.status='HOLD';$result.close_error=@{type=$_.Exception.GetType().FullName;hresult=$_.Exception.HResult}}}}
$result|ConvertTo-Json -Depth 15

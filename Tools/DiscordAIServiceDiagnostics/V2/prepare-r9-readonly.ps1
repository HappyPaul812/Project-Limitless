param([string]$OutputDirectory='F:\study\codex\Project-Limitless\Temp\V2R9Preflight20261027')
$ErrorActionPreference='Stop'
$root='F:\study\codex\Project-Limitless\Temp\Session0V2Candidate20261027-r9'
$result=[ordered]@{schema=2;status='HOLD';utc=[DateTimeOffset]::UtcNow.ToString('o');checks=@();installed=$false;system_changes=0;v1_reference_status='REFERENCE_VERIFICATION_HOLD'}
function Check([string]$Name,[scriptblock]$Body){
 try{$value=& $Body;if($value -ne $true){throw 'CHECK_NOT_TRUE'};$script:result.checks+=@{name=$Name;status='PASS'}}
 catch{$codes=@();$e=$_.Exception;while($e){$c=@{type=$e.GetType().FullName;hresult=$e.HResult};if($e -is [ComponentModel.Win32Exception]){$c.win32=$e.NativeErrorCode};$codes+=$c;$e=$e.InnerException};$script:result.checks+=@{name=$Name;status='HOLD';errors=$codes}}
}
function Sha([byte[]]$b){[Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($b)).ToLowerInvariant()}
$payload=@{};$m=$null
Check 'R9_PACKAGE_766_PLUS_1_AND_PINNED_BYTES' {
 $mb=[IO.File]::ReadAllBytes((Join-Path $root 'HostManifest.json'))
 if((Sha $mb) -cne '250bab2d46010841353a319fbcf34581784366ac3042e6bc0fdc97aadd17c09e'){throw 'MANIFEST_PIN'}
 $script:m=[Text.Encoding]::UTF8.GetString($mb)|ConvertFrom-Json -AsHashtable
 if($m.files_sha256.Count -ne 766 -or @(Get-ChildItem -LiteralPath $root -File -Force -Recurse).Count -ne 767){throw 'COUNT'}
 foreach($n in $m.files_sha256.Keys){if($n.Contains('..') -or $n.Contains(':') -or $n.Contains('\')){throw 'PATH'};$b=[IO.File]::ReadAllBytes((Join-Path $root $n));if((Sha $b) -cne $m.files_sha256[$n]){throw 'SHA'};$payload[$n]=$b}
 if((Sha $payload['Deployment/install-session0-v2.ps1']) -cne '1a1d671d1cf86aab56c9c2392ed4ba35a8240e348134e6a467f8eb8ec81f17a0'){throw 'INSTALLER_PIN'}
 $zb=[IO.File]::ReadAllBytes($root+'.zip');if((Sha $zb) -cne 'a4a652bdf73c9a1007bd1e761ae539d3459896b46d8a021fa1459012a1d15c41'){throw 'ZIP_PIN'}
 $ms=[IO.MemoryStream]::new($zb,$false);$z=[IO.Compression.ZipArchive]::new($ms,[IO.Compression.ZipArchiveMode]::Read)
 try{if($z.Entries.Count -ne 767){throw 'ZIP_COUNT'};$seen=@{};foreach($entry in $z.Entries){if($seen.ContainsKey($entry.FullName)){throw 'DUPLICATE'};$seen[$entry.FullName]=$true;$s=$entry.Open();$mem=[IO.MemoryStream]::new();try{$s.CopyTo($mem);$expected=if($entry.FullName -ceq 'HostManifest.json'){'250bab2d46010841353a319fbcf34581784366ac3042e6bc0fdc97aadd17c09e'}else{$m.files_sha256[$entry.FullName]};if((Sha $mem.ToArray()) -cne $expected){throw 'ZIP_ENTRY'}}finally{$s.Dispose();$mem.Dispose()}}}finally{$z.Dispose();$ms.Dispose()};$true
}
$admin=([Security.Principal.WindowsPrincipal]::new([Security.Principal.WindowsIdentity]::GetCurrent())).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
$result.admin=$admin
if($result.checks[0].status -eq 'PASS' -and $admin){
 Check 'READONLY_DEFINITIONS' {Add-Type -TypeDefinition ([Text.Encoding]::UTF8.GetString($payload['Code/V2/InstallAssembly.cs']));. ([scriptblock]::Create([Text.Encoding]::UTF8.GetString($payload['Code/V2/V2InstallCore.ps1'])));$true}
 # adapter 정의만으로 Windows를 변경하지 않습니다. 읽기 전용 context의 사용자명 확인용 빈 자격 객체입니다.
 . ([scriptblock]::Create([Text.Encoding]::UTF8.GetString($payload['Code/V2/V2WindowsAdapter.ps1'])))
 function Get-V2Hash([byte[]]$Bytes){Sha $Bytes}
 $dummy=[pscredential]::new('PAUL-PC\LIMITLESSRelaySvc',[Security.SecureString]::new())
 $api=New-V2WindowsAdapter $root ([Security.SecureString]::new()) $dummy $payload
 Check 'WORKER_SID_BATCH_GROUPS_BOOT_APPROVAL' {$c=$api.Context();$result.worker_sid=$c.worker_sid;$c.worker_sid.EndsWith('-1007') -and $c.worker_batch_present -and $c.worker_groups_users_only}
 Check 'V1_855_OWNER_DACL_ONBOOT_OPERATIONAL_PINS' {$s=$api.Snapshot();$s.known -and $s.old_bot_disabled -and $s.old_code_verified -and $s.operational_preserved}
 Check 'V2_ACCOUNT_PATH_TASK_ABSENCE' {$api.Fresh($m);$true}
 Check 'WORKER_IDLE_AND_OTHER_TASKS' {$w=$api.WorkerState();$result.worker_state=$w;$w.known -and $w.foreign_processes -eq 0 -and $w.other_launchable_tasks -eq 0}
 Check 'V2_PIPE_NAMESPACE_ABSENT_NO_CONNECT' {$pipes=[IO.Directory]::GetFiles('\\.\pipe\');@($pipes|Where-Object{[IO.Path]::GetFileName($_).StartsWith($m.pipe_name,[StringComparison]::OrdinalIgnoreCase)}).Count -eq 0}
 Check 'CANDIDATE_OWNER_AND_WRITERS' {
  $bad=0;$owners=@{}
  foreach($p in @($root,$root+'.zip')+@(Get-ChildItem -LiteralPath $root -Recurse -Force|ForEach-Object FullName)){
   $i=Get-Item -LiteralPath $p -Force;if($i.Attributes -band [IO.FileAttributes]::ReparsePoint){$bad++;continue}
   $a=Get-Acl -LiteralPath $p;$o=$a.GetOwner([Security.Principal.SecurityIdentifier]).Value;$owners[$o]=$true
   if($o -notin @('S-1-5-18','S-1-5-32-544')){$bad++}
   foreach($r in $a.GetAccessRules($true,$true,[Security.Principal.SecurityIdentifier])){if($r.AccessControlType -eq 'Allow' -and $r.IdentityReference.Value -notin @('S-1-5-18','S-1-5-32-544') -and ($r.FileSystemRights -band [Security.AccessControl.FileSystemRights]::Write -or $r.FileSystemRights -band [Security.AccessControl.FileSystemRights]::Delete -or $r.FileSystemRights -band [Security.AccessControl.FileSystemRights]::ChangePermissions -or $r.FileSystemRights -band [Security.AccessControl.FileSystemRights]::TakeOwnership)){$bad++}}
  };$result.candidate_acl=@{owners=@($owners.Keys);violations=$bad};$bad -eq 0
 }
}else{$result.checks+=@{name='ADMIN_WINDOWS_PREFLIGHT';status='HOLD';reason='ADMIN_REQUIRED_OR_PACKAGE_HOLD'}}
if(@($result.checks|Where-Object status -ne 'PASS').Count -eq 0){$result.status='READONLY_PREFLIGHT_PASS_NOT_INSTALL_APPROVAL'}
# 결과만 프로젝트 Temp에 CREATE_NEW합니다. 운영 경로·승인·lease·계정·Task를 만들지 않습니다.
$allowed='F:\study\codex\Project-Limitless\Temp\V2R9Preflight20261027'
if([IO.Path]::GetFullPath($OutputDirectory) -cne $allowed){throw 'OUTPUT_SCOPE_HOLD'}
[IO.Directory]::CreateDirectory($allowed)|Out-Null
$path=Join-Path $allowed ([guid]::NewGuid().ToString('N')+'.json')
$bytes=[Text.UTF8Encoding]::new($false).GetBytes(($result|ConvertTo-Json -Depth 16));$f=[IO.File]::Open($path,[IO.FileMode]::CreateNew,[IO.FileAccess]::Write,[IO.FileShare]::None);try{$f.Write($bytes)}finally{$f.Dispose()}
@{status=$result.status;admin=$admin;result_path=$path;checks=$result.checks}|ConvertTo-Json -Depth 16

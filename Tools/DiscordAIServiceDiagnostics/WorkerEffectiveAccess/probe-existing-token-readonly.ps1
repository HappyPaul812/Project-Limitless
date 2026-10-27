param([switch]$ConfirmApprovedReadOnly,[int]$TargetProcessId=0,[string]$ExpectedCreatedUtc='')
$ErrorActionPreference='Stop';Set-StrictMode -Version Latest
if(-not $ConfirmApprovedReadOnly){throw 'SEPARATE_READONLY_APPROVAL_REQUIRED'}
$root='F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree'
$result=@{schema=1;kind='R2_EXISTING_TOKEN_READONLY';status='HOLD_EXISTING_TOKEN_TARGET_REQUIRED';changes=0;new_logons=0;install_calls=0;ipc_calls=0;model_calls=0;automatic_retries=0;runtime_admitted=$false;task_descriptor_status='PRIOR_PASS_REUSED_NOT_REPEATED';job_exclusivity='UNKNOWN';old_worker_conflict='UNKNOWN'}
function Get-R2TokenSha([byte[]]$Bytes){[Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($Bytes)).ToLowerInvariant()}
try{
 if($PSVersionTable.PSVersion.Major -ne 7 -or -not [Environment]::Is64BitProcess){throw 'PS7_X64_REQUIRED'}
 $identity=[Security.Principal.WindowsIdentity]::GetCurrent();if(-not ([Security.Principal.WindowsPrincipal]::new($identity)).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)){throw 'ADMIN_READONLY_REQUIRED'}
 if($TargetProcessId -gt 0 -and -not [string]::IsNullOrWhiteSpace($ExpectedCreatedUtc)){
  $native=[IO.File]::ReadAllBytes((Join-Path $root 'Tools/DiscordAIServiceDiagnostics/WorkerEffectiveAccess/ExistingTokenRead.cs'));if((Get-R2TokenSha $native) -cne 'a00de5e9a49c550a1aa2c261a5c735d37938d897a5670eaaa238469a3fecc365'){throw 'TOKEN_NATIVE_SHA_HOLD'}
  if('R2ExistingTokenRead' -as [type]){throw 'FRESH_PROCESS_REQUIRED'};Add-Type -TypeDefinition ([Text.Encoding]::UTF8.GetString($native))
  $policy=[IO.File]::ReadAllBytes((Join-Path $root 'Tools/DiscordAIServiceDiagnostics/WorkerEffectiveAccess/TokenPolicy.ps1'));if((Get-R2TokenSha $policy) -cne '567f01ea08b1eb6c3fbe7ef47c8f7626e9cab00d1889ce894cd126a1fad3eb10'){throw 'TOKEN_POLICY_SHA_HOLD'};. ([scriptblock]::Create([Text.Encoding]::UTF8.GetString($policy)))
  $baseline=[IO.File]::ReadAllBytes((Join-Path $root '문서/00_프로젝트/팀운영/Evidence/TaskSecurityReadback20261027/Admin-acd610e6e04348c88b881e8c0f17fe33.json'));if((Get-R2TokenSha $baseline) -cne 'e22bb23bdce8e6780faeaee70a2a400266d0f3ad21dc639e86f8f58de9ff0fae'){throw 'PRIOR_DESCRIPTOR_PROOF_SHA_HOLD'}
  $old=[Text.Encoding]::UTF8.GetString($baseline)|ConvertFrom-Json -AsHashtable
  $result.stage='CURRENT_SD_FOR_TOKEN_ACCESSCHECK';$scheduler=New-Object -ComObject Schedule.Service;$scheduler.Connect();$task=$scheduler.GetFolder('\').GetTask('LIMITLESS-DedicatedV2-20261027-Host');if($task.Path -cne '\LIMITLESS-DedicatedV2-20261027-Host'){throw 'TASK_PATH_HOLD'}
  $sd=$task.GetSecurityDescriptor(7);if($sd -cne $old.task.sddl){throw 'DESCRIPTOR_CHANGED_SINCE_PASS_HOLD'}
  $result.stage='EXISTING_PROCESS_TOKEN';$result.facts=[R2ExistingTokenRead]::Read($TargetProcessId,$ExpectedCreatedUtc,$sd)
  if($task.GetSecurityDescriptor(7) -cne $sd){throw 'DESCRIPTOR_CHANGED_DURING_TOKEN_QUERY_HOLD'}
  $result.policy=Test-R2ExistingTokenPolicy $result.facts;$result.status=$result.policy.status
 }
}catch{$result.status='HOLD';$result.errors=@();for($e=$_.Exception;$null -ne $e;$e=$e.InnerException){$r=@{type=$e.GetType().FullName;hresult=$e.HResult};if($e -is [ComponentModel.Win32Exception]){$r.win32=$e.NativeErrorCode};if($e.Data.Contains('operation')){$r.operation=$e.Data['operation']};$result.errors+=$r}}
$result.collected_utc=[datetime]::UtcNow.ToString('o')
$path=Join-Path $root ('문서/00_프로젝트/팀운영/Evidence/WorkerEffectiveAccess20261027/Admin-'+[guid]::NewGuid().ToString('N')+'.json');$result.result_path=$path
$bytes=[Text.UTF8Encoding]::new($false).GetBytes(($result|ConvertTo-Json -Depth 18));$file=[IO.File]::Open($path,[IO.FileMode]::CreateNew,[IO.FileAccess]::Write,[IO.FileShare]::None);try{$file.Write($bytes)}finally{$file.Dispose()}
@{status=$result.status;result_path=$path;changes=0;new_logons=0;ipc_calls=0;runtime_admitted=$false}|ConvertTo-Json

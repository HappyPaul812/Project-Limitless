param()
$ErrorActionPreference='Stop';Set-StrictMode -Version Latest
$stage='CANDIDATE_COMPILE'
$result=@{schema=1;kind='TASK_READ_VERIFIER_OFFLINE_CANDIDATE';status='HOLD';windows_reverified=$false;runtime_admitted=$false;admin_queries=0;changes=0;install_calls=0;ipc_calls=0;model_calls=0;automatic_retries=0}
try{
 Add-Type -Path (Join-Path $PSScriptRoot 'TaskReadSecurity.cs')
 $stage='PINNED_HISTORICAL_SNAPSHOT'
 $root='F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree'
 $bytes=[IO.File]::ReadAllBytes((Join-Path $root '문서/00_프로젝트/팀운영/Evidence/DedicatedV2R2Readonly20261027/Admin-33a67728035f4fe88db7f539e5b648dc.json'))
 $proof=[Limitless.TaskSecurityCandidate.DedicatedTaskReadContract]::FromPinnedSnapshot($bytes)
 $d=[Text.Encoding]::UTF8.GetString($bytes)|ConvertFrom-Json -AsHashtable
 $observed=($d.queries|Where-Object stage -EQ TASK_SECURITY_DESCRIPTOR).value.sddl
 $stage='OFFLINE_TASK_SPECIFIC_COMPARE'
 $result.verdict=[Limitless.TaskSecurityCandidate.DedicatedTaskReadContract]::VerifyOffline($observed,$proof.ObjectKind,[Limitless.TaskSecurityCandidate.DedicatedTaskReadContract]::TaskPath,[Limitless.TaskSecurityCandidate.DedicatedTaskReadContract]::Worker,$proof)
 $result.status=$result.verdict.Status;$result.mapping_evidence_sha256=$proof.SourceSha
 # 후보 PASS와 별도로 과거 설치 원시 오류 코드를 보존합니다.
 $original=@($d.queries|Where-Object {$_.stage -match '^RECORD_Result-'})
 $result.original_native_errors=@($original[0].value.sanitized.errors)
 $result.original_install_status='HOLD';$result.task_changes=0;$result.original_contract_changed=$false
}catch{
 $result.status='HOLD';$result.errors=@();for($e=$_.Exception;$null -ne $e;$e=$e.InnerException){$row=@{type=$e.GetType().FullName;hresult=$e.HResult};if($e -is [ComponentModel.Win32Exception]){$row.win32=$e.NativeErrorCode};$result.errors+=$row}
}
$result.stage=$stage;$result|ConvertTo-Json -Depth 12

param()
$ErrorActionPreference='Stop';Set-StrictMode -Version Latest
$root='F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree'
$folder=Join-Path $root 'Tools/DiscordAIServiceDiagnostics/TaskSecurityCandidate'
Add-Type -Path (Join-Path $folder 'TaskReadSecurity.cs')
$snapshot=[IO.File]::ReadAllBytes((Join-Path $root '문서/00_프로젝트/팀운영/Evidence/DedicatedV2R2Readonly20261027/Admin-33a67728035f4fe88db7f539e5b648dc.json'))
$proof=[Limitless.TaskSecurityCandidate.DedicatedTaskReadContract]::FromPinnedSnapshot($snapshot)
$worker=[Limitless.TaskSecurityCandidate.DedicatedTaskReadContract]::Worker
$taskPath=[Limitless.TaskSecurityCandidate.DedicatedTaskReadContract]::TaskPath
$sddl='O:BAG:SYD:P(A;;FA;;;SY)(A;;FA;;;BA)(A;;GR;;;'+$worker+')'
$script:rows=@()
function Verify([string]$Sd,$Evidence=$proof,[string]$Kind='SCHEDULED_TASK_FILE_RIGHTS',[string]$Path=$taskPath,[string]$Sid=$worker){[Limitless.TaskSecurityCandidate.DedicatedTaskReadContract]::VerifyOffline($Sd,$Kind,$Path,$Sid,$Evidence)}
function Test-Candidate([string]$Name,[scriptblock]$Test){try{if(-not (& $Test)){throw 'ASSERT'};$script:rows+=@{name=$Name;status='PASS'}}catch{$script:rows+=@{name=$Name;status='FAIL';type=$_.Exception.GetType().FullName;hresult=$_.Exception.HResult}}}
function Reject([scriptblock]$Test){try{& $Test|Out-Null;return $false}catch{return $true}}
Test-Candidate 'GENERIC_READ_WITH_BOUND_PROOF' {(Verify $sddl).NormalizedWorkerMask -eq 0x120089}
Test-Candidate 'SPECIFIC_FILE_READ_WITH_BOUND_PROOF' {(Verify ($sddl.Replace(';;GR;',';;FR;'))).NormalizedWorkerMask -eq 0x120089}
Test-Candidate 'OBSERVED_SNAPSHOT_ONLY_OFFLINE_PASS' {$d=[Text.Encoding]::UTF8.GetString($snapshot)|ConvertFrom-Json;$sd=($d.queries|Where-Object stage -EQ TASK_SECURITY_DESCRIPTOR).value.sddl;$v=Verify $sd;$v.Status -eq 'OFFLINE_CANDIDATE_PASS_NOT_WINDOWS_REVERIFIED' -and -not $v.LiveVerified -and -not $v.RuntimeAdmitted}
Test-Candidate 'REDUNDANT_GENERIC_READ_NO_EXPANSION' {(Verify ($sddl.Replace(';;GR;',';;0x80120089;'))).NormalizedWorkerMask -eq 0x120089}
Test-Candidate 'MISSING_MAPPING_PROOF_HOLD' {Reject {Verify ($sddl.Replace(';;GR;',';;FR;')) $null}}
Test-Candidate 'WRONG_OBJECT_MAPPING_HOLD' {Reject {Verify $sddl $proof 'REGISTRY_KEY'}}
Test-Candidate 'WRONG_TASK_SCOPE_HOLD' {Reject {Verify $sddl $proof 'SCHEDULED_TASK_FILE_RIGHTS' '\OTHER'}}
Test-Candidate 'WRONG_WORKER_BINDING_HOLD' {Reject {Verify $sddl $proof 'SCHEDULED_TASK_FILE_RIGHTS' $taskPath 'S-1-5-18'}}
Test-Candidate 'TAMPERED_MAPPING_EVIDENCE_HOLD' {$copy=[byte[]]$snapshot.Clone();$copy[0]=$copy[0] -bxor 1;Reject {[Limitless.TaskSecurityCandidate.DedicatedTaskReadContract]::FromPinnedSnapshot($copy)}}
foreach($bit in @([uint32]0x2,[uint32]0x4,[uint32]0x10,[uint32]0x20,[uint32]0x40,[uint32]0x100,[uint32]0x10000,[uint32]0x40000,[uint32]0x80000,[uint32]0x1000000,[uint32]0x2000000,[uint32]0x10000000,[uint32]0x20000000,[uint32]0x40000000,[uint32]0x200,[uint32]0x800000)){
 $mask=[uint32]0x120089 -bor $bit;$text=';;0x'+$mask.ToString('x8')+';'
 Test-Candidate ('REJECT_ADDED_BIT_0x'+$bit.ToString('x8')) {Reject {Verify ($sddl.Replace(';;GR;',$text))}}
}
foreach($bit in @([uint32]1,[uint32]8,[uint32]0x80,[uint32]0x20000,[uint32]0x100000)){
 $mask=[uint32]0x120089 -bxor $bit;$text=';;0x'+$mask.ToString('x8')+';'
 Test-Candidate ('REJECT_MISSING_READ_BIT_0x'+$bit.ToString('x8')) {Reject {Verify ($sddl.Replace(';;GR;',$text))}}
}
Test-Candidate 'WORKER_FULL_CONTROL_REJECT' {Reject {Verify ($sddl.Replace(';;GR;',';;FA;'))}}
Test-Candidate 'UNEXPECTED_SID_REJECT' {Reject {Verify ($sddl.Replace($worker,'S-1-5-32-545'))}}
Test-Candidate 'ADDED_ACE_REJECT' {Reject {Verify ($sddl+'(A;;FR;;;WD)')}}
Test-Candidate 'DUPLICATE_WORKER_ACE_REJECT' {Reject {Verify ($sddl+'(A;;FR;;;'+$worker+')')}}
Test-Candidate 'OWNER_MISMATCH_REJECT' {Reject {Verify ($sddl.Replace('O:BA','O:SY'))}}
Test-Candidate 'GROUP_MISMATCH_REJECT' {Reject {Verify ($sddl.Replace('G:SY','G:BA'))}}
Test-Candidate 'PROTECTED_DACL_MISSING_REJECT' {Reject {Verify ($sddl.Replace('D:P','D:'))}}
Test-Candidate 'UNEXPECTED_CONTROL_FLAGS_REJECT' {Reject {Verify ($sddl.Replace('D:P','D:PAR'))}}
Test-Candidate 'ACE_ORDER_CHANGE_REJECT' {Reject {Verify ($sddl.Replace('(A;;FA;;;SY)(A;;FA;;;BA)','(A;;FA;;;BA)(A;;FA;;;SY)'))}}
Test-Candidate 'ACE_INHERIT_FLAGS_REJECT' {Reject {Verify ($sddl.Replace('(A;;GR;','(A;OI;GR;'))}}
Test-Candidate 'DENY_ACE_REJECT' {Reject {Verify ($sddl.Replace('(A;;GR;','(D;;GR;'))}}
Test-Candidate 'SYSTEM_RIGHTS_CHANGE_REJECT' {Reject {Verify ($sddl.Replace('FA;;;SY','FR;;;SY'))}}
Test-Candidate 'ADMIN_UNKNOWN_BIT_REJECT' {Reject {Verify ($sddl.Replace('FA;;;BA','0x009f01ff;;;BA'))}}
Test-Candidate 'NULL_DACL_REJECT' {Reject {Verify 'O:BAG:SYD:NO_ACCESS_CONTROL'}}
Test-Candidate 'EMPTY_DACL_REJECT' {Reject {Verify 'O:BAG:SYD:P'}}
Test-Candidate 'MALFORMED_DESCRIPTOR_REJECT' {Reject {Verify 'INVALID'}}
Test-Candidate 'NULL_DESCRIPTOR_REJECT' {Reject {Verify $null}}
Test-Candidate 'OBJECT_SPECIFIC_ACE_REJECT' {Reject {Verify ($sddl.Replace('(A;;GR;;;','(OA;;GR;;;'))}}
@{kind='TASK_READ_SECURITY_CANDIDATE_OFFLINE';pass=@($script:rows|Where-Object status -EQ PASS).Count;fail=@($script:rows|Where-Object status -EQ FAIL).Count;tests=$script:rows;windows_reverified=$false;admin_queries=0;changes=0;install_calls=0;ipc_calls=0;model_calls=0}|ConvertTo-Json -Depth 8

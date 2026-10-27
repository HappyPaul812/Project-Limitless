param()
$ErrorActionPreference='Stop';Set-StrictMode -Version Latest
$root='F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree';$dir=Join-Path $root 'Tools/DiscordAIServiceDiagnostics/WorkerEffectiveAccess'
Add-Type -Path (Join-Path $dir 'ExistingTokenRead.cs');. (Join-Path $dir 'TokenPolicy.ps1')
$script:rows=@()
function Check-Token($Name,[scriptblock]$Test){try{if(-not (& $Test)){throw 'ASSERT'};$script:rows+=@{name=$Name;status='PASS'}}catch{$script:rows+=@{name=$Name;status='FAIL';type=$_.Exception.GetType().FullName}}}
function Fake-Facts {
 $checks=@(@{requested=0x120089;allowed=$true;granted=0x120089;privileges_used=0},@{requested=0x2000000;allowed=$true;granted=0x120089;privileges_used=0});foreach($mask in @(0x20,2,4,0x10,0x100,0x10000,0x40000,0x80000)){$checks+=@{requested=$mask;allowed=$false;granted=0;privileges_used=0}}
 @{token_accesscheck_known=$true;token_user_sid='S-1-5-21-2858504392-1958168942-1662906075-1010';token_elevated=$false;process_token_stable=$true;checks=$checks;logon_query_status='READ_OK';security_logon_type=4;session_id=0}
}
Check-Token 'REAL_TOKEN_RESULT_SHAPE_PASS_STILL_RUNTIME_HOLD' {$f=Fake-Facts;$r=Test-R2ExistingTokenPolicy $f;$r.accesscheck_status -eq 'EXISTING_PROCESS_TOKEN_DACL_MODEL_PASS' -and -not $r.runtime_admitted -and $r.job_exclusivity -eq 'UNKNOWN'}
foreach($mask in @(0x20,2,4,0x10,0x100,0x10000,0x40000,0x80000)){Check-Token ('INDIRECT_GROUP_OR_DIRECT_OVERGRANT_HOLD_'+$mask) {$f=Fake-Facts;($f.checks|Where-Object requested -EQ $mask).allowed=$true;(Test-R2ExistingTokenPolicy $f).status -eq 'HOLD'}}
Check-Token 'DENY_ACE_OR_DENY_ONLY_GROUP_READ_DENIED_HOLD' {$f=Fake-Facts;$f.checks[0].allowed=$false;(Test-R2ExistingTokenPolicy $f).status -eq 'HOLD'}
Check-Token 'RESTRICTED_TOKEN_READ_DENIED_HOLD' {$f=Fake-Facts;$f.checks[0].allowed=$false;(Test-R2ExistingTokenPolicy $f).accesscheck_status -eq 'UNKNOWN'}
Check-Token 'GROUP_LIST_WITHOUT_REAL_TOKEN_NEVER_PASS' {$f=Fake-Facts;$f.token_accesscheck_known=$false;$f.groups=@('S-1-5-32-545');(Test-R2ExistingTokenPolicy $f).status -eq 'HOLD'}
Check-Token 'WRONG_SID_HOLD' {$f=Fake-Facts;$f.token_user_sid='S-1-5-21-2858504392-1958168942-1662906075-1007';(Test-R2ExistingTokenPolicy $f).status -eq 'HOLD'}
Check-Token 'GENERATION_OR_TOKEN_CHANGED_HOLD' {$f=Fake-Facts;$f.process_token_stable=$false;(Test-R2ExistingTokenPolicy $f).status -eq 'HOLD'}
Check-Token 'PRIVILEGE_USED_HOLD' {$f=Fake-Facts;$f.checks[1].privileges_used=1;(Test-R2ExistingTokenPolicy $f).status -eq 'HOLD'}
Check-Token 'UNKNOWN_MAXIMUM_RIGHTS_HOLD' {$f=Fake-Facts;$f.checks[1].granted=0x120289;(Test-R2ExistingTokenPolicy $f).status -eq 'HOLD'}
Check-Token 'ELEVATED_TOKEN_HOLD' {$f=Fake-Facts;$f.token_elevated=$true;(Test-R2ExistingTokenPolicy $f).status -eq 'HOLD'}
Check-Token 'GROUP_LOGON4_IS_NOT_SECURITY_BATCH4' {$f=Fake-Facts;$f.security_logon_type=2;(Test-R2ExistingTokenPolicy $f).session0_batch_status -eq 'UNKNOWN'}
Check-Token 'NONZERO_SESSION_NOT_SESSION0' {$f=Fake-Facts;$f.session_id=1;(Test-R2ExistingTokenPolicy $f).session0_batch_status -eq 'UNKNOWN'}
Check-Token 'LOGON_QUERY_UNKNOWN_NOT_BATCH_PASS' {$f=Fake-Facts;$f.logon_query_status='UNKNOWN';(Test-R2ExistingTokenPolicy $f).session0_batch_status -eq 'UNKNOWN'}
Check-Token 'NO_CHECK_RESULT_HOLD' {$f=Fake-Facts;$f.checks=@();(Test-R2ExistingTokenPolicy $f).status -eq 'HOLD'}
Check-Token 'PID_ZERO_REJECTED_BEFORE_NATIVE_CALL' {try{[R2ExistingTokenRead]::Read(0,'2026-10-27T00:00:00Z','O:BAG:SYD:P')|Out-Null;$false}catch{$true}}
Check-Token 'ENTRY_PARSE_AND_NO_FORBIDDEN_APIS' {$p=Join-Path $dir 'probe-existing-token-readonly.ps1';$t=$null;$e=$null;[void][Management.Automation.Language.Parser]::ParseFile($p,[ref]$t,[ref]$e);$source=[IO.File]::ReadAllText((Join-Path $dir 'ExistingTokenRead.cs'));$e.Count -eq 0 -and $source -notmatch 'extern.*(LogonUser|CreateProcess|AdjustToken|Impersonate|SetThreadToken|TerminateProcess|CreateJob|AssignProcess)'}
# 테스트 전용 단순 DACL 모델입니다. 실권한 판정에는 이 모델 대신 Windows AccessCheck를 씁니다.
function Fake-DaclAccess($Aces,$Groups,[uint32]$Desired){$remaining=$Desired;foreach($ace in $Aces){$g=@($Groups|Where-Object sid -CEQ $ace.sid);if(-not $g.Count){continue};$matches=if($ace.kind -eq 'deny'){$g[0].enabled -or $g[0].deny_only}else{$g[0].enabled -and -not $g[0].deny_only};if(-not $matches){continue};if($ace.kind -eq 'deny' -and ($remaining -band $ace.mask)){return $false};if($ace.kind -eq 'allow'){$remaining=$remaining -band (-bnot $ace.mask)};if($remaining -eq 0){return $true}};return $false}
Check-Token 'MOCK_USERS_GROUP_WRITE_GRANT_DETECTED' {Fake-DaclAccess @(@{sid='USERS';kind='allow';mask=2}) @(@{sid='USERS';enabled=$true;deny_only=$false}) 2}
Check-Token 'MOCK_DENY_ONLY_GROUP_CANNOT_ALLOW' {-not (Fake-DaclAccess @(@{sid='USERS';kind='allow';mask=1}) @(@{sid='USERS';enabled=$false;deny_only=$true}) 1)}
Check-Token 'MOCK_DENY_ONLY_GROUP_CAN_DENY' {-not (Fake-DaclAccess @(@{sid='USERS';kind='deny';mask=1},@{sid='WORKER';kind='allow';mask=1}) @(@{sid='USERS';enabled=$false;deny_only=$true},@{sid='WORKER';enabled=$true;deny_only=$false}) 1)}
Check-Token 'MOCK_DISABLED_GROUP_CANNOT_ALLOW' {-not (Fake-DaclAccess @(@{sid='USERS';kind='allow';mask=1}) @(@{sid='USERS';enabled=$false;deny_only=$false}) 1)}
Check-Token 'MOCK_RESTRICTED_SECOND_CHECK_REQUIRED' {$normal=Fake-DaclAccess @(@{sid='WORKER';kind='allow';mask=1}) @(@{sid='WORKER';enabled=$true;deny_only=$false}) 1;$restricted=Fake-DaclAccess @(@{sid='WORKER';kind='allow';mask=1}) @(@{sid='RESTRICTED';enabled=$true;deny_only=$false}) 1;$normal -and -not $restricted}
Check-Token 'IN_ANY_JOB_IS_NOT_JOB_EXCLUSIVITY' {$f=Fake-Facts;$f.in_any_job=$true;(Test-R2ExistingTokenPolicy $f).job_exclusivity -ceq 'UNKNOWN'}
Check-Token 'ABSENT_LEASE_DOES_NOT_PROVE_OLD_WORKER_IDLE' {$f=Fake-Facts;$f.lease='ABSENT';(Test-R2ExistingTokenPolicy $f).old_worker_conflict -ceq 'UNKNOWN'}
Check-Token 'SESSION0_BATCH_OBSERVATION_IS_NOT_RUNTIME_ADMISSION' {$r=Test-R2ExistingTokenPolicy (Fake-Facts);$r.session0_batch_status -ceq 'OBSERVED_EXISTING_TOKEN_ONLY' -and -not $r.runtime_admitted -and $r.future_worker_execution -ceq 'UNVERIFIED'}
@{kind='EXISTING_TOKEN_POLICY_NEW_OFFLINE';pass=@($script:rows|Where-Object status -EQ PASS).Count;fail=@($script:rows|Where-Object status -EQ FAIL).Count;tests=$script:rows;native_token_queries=0;admin_queries=0;new_logons=0;changes=0;existing_48_22_repeated=$false}|ConvertTo-Json -Depth 8

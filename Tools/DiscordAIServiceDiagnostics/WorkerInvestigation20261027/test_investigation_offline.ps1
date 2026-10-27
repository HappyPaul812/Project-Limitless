param([string]$Output)
$ErrorActionPreference='Stop'
Add-Type -Path (Join-Path $PSScriptRoot 'WorkerReadOnlyNative.cs')
. (Join-Path $PSScriptRoot 'InvestigationModel.ps1')
$tests=[Collections.Generic.List[object]]::new();$worker='S-1-5-21-101-202-303-1007';$time='2026-10-23T17:40:25.1963220Z'
function Need($value){if(-not $value){throw 'ASSERT'}}
function State {
 @{reuse=$false;denied=$false;missing=$false;cim_missing=$false;cim_time=$time;parent_pid=0;parent_time='2026-10-23T17:40:24.0000000Z';children=@();tasks=@();security_type=4;resolve_error=0;resolve_calls=0;group_sid='S-1-5-32-999';group_use=4;group_category='BUILTIN_ALIAS';logon_unknown=$false
 native=@{identity='MATCH';actual_created_utc=$time;owner_sid=$worker;token_query_known=$true;session_id=0;authentication_id_decimal='1234';authentication_luid='00000000000004d2';token_groups=@(@{sid='S-1-5-32-999';attributes=[uint32]4;enabled=$true;deny_only=$false});restricted_sids=@();image_query_path='PRIVATE_IMAGE_PATH_SENTINEL';in_any_job=$true;job_identity='UNKNOWN_NO_FOREIGN_JOB_HANDLE_OPENED';exited_during_query=$false}
 image=@{name='conhost.exe';location='WINDOWS';sha256=('a'*64);signature_status='VALID_AT_CACHE_POLICY';signature_code=0;catalog_enumeration_performed=$false};cim_times=@{};image_error=$false;task_error=$false;lease_error=$false;lease_status='ABSENT';child_calls=0}
}
function Provider {
 @{
 Process={param($id,$created)
  if($f.denied){throw [WorkerQueryFailure]::new('OpenProcess_QUERY_LIMITED_INFORMATION',5)}
  if($f.missing){throw [WorkerQueryFailure]::new('OpenProcess_QUERY_LIMITED_INFORMATION',87)}
  $p=$f.native.Clone();$p.pid=$id;$p.actual_created_utc=if($f.reuse){[datetimeoffset]::Parse($created).AddSeconds(1).ToString('o')}else{$created};$p.identity=if($f.reuse){'PID_REUSED'}else{'MATCH'};$p
 }
 Cim={param($id)if(-not $f.cim_missing){$created=if($f.cim_times.ContainsKey($id)){$f.cim_times[$id]}else{$f.cim_time};[pscustomobject]@{CreationDate=[datetime]::Parse($created);ParentProcessId=$f.parent_pid;SessionId=0}}}
 Image={param($path)if($f.image_error){throw [WorkerQueryFailure]::new('Image_CreateFile_READ_SHARE_READ',5)};$f.image}
 Parent={param($id)@{pid=$id;created_utc=$f.parent_time;exited=$false}}
 Children={param($id)$f.child_calls++;$f.children|Where-Object ParentProcessId -eq $id}
 Logon={param($id)if($f.logon_unknown){throw [WorkerQueryFailure]::new('Logon_query',5)};@{LogonId=$id;LogonType=$f.security_type}}
 Resolve={param($value)$f.resolve_calls++;if($f.resolve_error){throw [WorkerQueryFailure]::new('LookupAccountNameW',$f.resolve_error)};if($value -ceq $worker){@{resolved=$true;sid=$worker;sid_name_use=1;category='LOCAL_USER';name='FAKE_WORKER';domain='FAKE_HOST'}}else{@{resolved=$true;sid=$f.group_sid;sid_name_use=$f.group_use;category=$f.group_category;name='FAKE_GROUP';domain='FAKE_AUTHORITY'}}}
 Tasks={if($f.task_error){throw [WorkerQueryFailure]::new('Task_metadata',5)};$f.tasks}
 TaskDetail={param($task)@{action_images=@('pwsh.exe');running_instances=@(@{engine_pid=1;state=4;instance_id='11111111-1111-1111-1111-111111111111'})}}
 Leases={if($f.lease_error){throw [WorkerQueryFailure]::new('Lease_metadata',5)};@{coverage='KNOWN_V2_PATHS_ONLY_NOT_GLOBAL';entries=@{worker_lease=@{status=$f.lease_status}};contents_read=$false;locks_acquired=0}}
 }
}
function Baseline {,@{pid=1;created_utc=$time}}
function Run {Invoke-WorkerInvestigation (Baseline) $worker (Provider)}
function Task($user,$group,$type){@{Principal=@{UserId=$user;GroupId=$group;LogonType=$type};task_id_hash=('f'*64);enabled=$true;state=4}}
function Test($name,[scriptblock]$body){$f=State;try{& $body;$tests.Add(@{name=$name;status='PASS'})}catch{$tests.Add(@{name=$name;status='FAIL';type=$_.Exception.GetType().FullName})}}
Test 'source-microsecond-matches-native-100ns' {Need ([WorkerReadOnlyNative]::CreationMatches($time,'2026-10-23T17:40:25.1963229Z'))}
Test 'next-microsecond-rejected' {Need (-not [WorkerReadOnlyNative]::CreationMatches($time,'2026-10-23T17:40:25.1963230Z'))}
Test 'timezone-equivalence-not-tolerance' {Need ([WorkerReadOnlyNative]::CreationMatches($time,'2026-10-24T02:40:25.1963220+09:00'))}
Test 'json-datetime-preserves-subsecond-before-native-binding' {$expected=@{pid=1;created_utc=[datetime]::Parse($time)};$r=Read-InvestigationProcess $expected $worker (Provider);Need ($r.expected_created_utc -ceq $time -and $r.actual_created_utc -ceq $time -and $r.identity -ceq 'MATCH')}
Test 'datetimeoffset-preserves-timezone-and-fraction' {Need ((Get-InvestigationCreatedUtc ([datetimeoffset]::Parse('2026-10-24T02:40:25.1963220+09:00'))) -ceq $time)}
Test 'pid-reuse-is-unknown-not-reassigned' {$f.reuse=$true;$r=Run;Need ($r.targets[0].identity -ceq 'PID_REUSED' -and -not $r.targets[0].image -and $r.status -ceq 'HOLD')}
Test 'access-denied-native-code-preserved' {$f.denied=$true;$r=Run;Need ($r.targets[0].identity -ceq 'ACCESS_DENIED' -and @($r.targets[0].errors[0].codes|Where-Object win32 -eq 5).Count -gt 0)}
Test 'exit-requires-open-notfound-and-cim-absence' {$f.missing=$true;$f.cim_missing=$true;$r=Run;Need ($r.targets[0].identity -ceq 'PROCESS_EXITED_AT_SNAPSHOT')}
Test 'notfound-with-current-object-remains-unknown' {$f.missing=$true;$r=Run;Need ($r.targets[0].identity -ceq 'UNKNOWN')}
Test 'cim-generation-race-blocks-parent-relations' {$f.cim_time='2026-10-23T17:40:26.1963220Z';$f.parent_pid=9;$r=Run;Need (-not $r.targets[0].cim_consistent -and $r.parents.Count -eq 0)}
Test 'owner-is-verified-not-logon-association-assumption' {$f.native.owner_sid='S-1-5-18';$r=Run;Need ($r.targets[0].assessment.role -ceq 'OTHER_ACCOUNT_ASSOCIATION' -and $r.status -ceq 'HOLD')}
Test 'owner-denied-remains-unknown' {$f.native.Remove('owner_sid');$f.native.token_query_known=$false;$r=Run;Need ($r.targets[0].assessment.role -ceq 'OWNER_UNKNOWN')}
Test 'codex-image-is-indicator-not-model-call-proof' {$f.image.name='codex.exe';$r=Run;Need ($r.ai_or_cli_image_indicators.Count -eq 1 -and -not $r.actual_ai_request_verified -and $r.status -ceq 'HOLD')}
Test 'runtime-role-not-guessed-ai-or-safe' {$f.image.name='python.exe';$r=Run;Need ($r.targets[0].assessment.role -ceq 'GENERAL_RUNTIME_OR_LAUNCHER_UNKNOWN' -and -not $r.targets[0].assessment.safe_to_exclude)}
Test 'signed-windows-resident-not-whitelisted' {$r=Run;Need ($r.targets[0].assessment.role -ceq 'SIGNED_WINDOWS_RESIDENT_CANDIDATE' -and $r.status -ceq 'HOLD' -and -not $r.global_exclusive_execution_verified)}
Test 'system-looking-name-untrusted-location' {$f.image.location='OTHER_PATH_REDACTED';$r=Run;Need ($r.targets[0].assessment.role -ceq 'WINDOWS_NAME_ONLY_NOT_VERIFIED')}
Test 'signature-unknown-not-safe' {$f.image.signature_status='NOT_VERIFIED_BY_THIS_METHOD';$r=Run;Need (-not $r.targets[0].assessment.safe_to_exclude -and $r.targets[0].assessment.role -ceq 'WINDOWS_NAME_ONLY_NOT_VERIFIED')}
Test 'image-denied-not-swallowed-as-normal' {$f.image_error=$true;$r=Run;Need ($r.targets[0].assessment.role -ceq 'IMAGE_UNKNOWN')}
Test 'exited-during-query-not-active-cli-proof' {$f.native.exited_during_query=$true;$f.image.name='codex.exe';$r=Run;Need ($r.targets[0].assessment.role -ceq 'EXITED_DURING_QUERY' -and $r.ai_or_cli_image_indicators.Count -eq 0)}
Test 'group-id-null-user-resolved-through-os-provider' {$f.tasks=@((Task $null 'FIXTURE_GROUP_NAME' 4));$r=Run;Need ($r.tasks[0].principal.group_id -ceq 'FIXTURE_GROUP_NAME' -and $r.tasks[0].principal.group_sid -ceq $f.group_sid)}
Test 'scheduler-group4-versus-security-batch4' {$f.tasks=@((Task $null 'FIXTURE_GROUP_NAME' 4));$r=Run;Need ($r.tasks[0].principal.task_scheduler_logon_label -ceq 'GROUP' -and $r.targets[0].security_logon_label -ceq 'BATCH' -and -not $r.tasks[0].principal.execution_confirmed)}
foreach($category in @('LOCAL_GROUP','BUILTIN_ALIAS','WELL_KNOWN_GROUP','DOMAIN_GROUP_OR_DIRECTORY_GROUP')){Test ('group-category-observed-'+$category) {$f.group_category=$category;$f.group_use=if($category -ceq 'WELL_KNOWN_GROUP'){5}elseif($category -ceq 'DOMAIN_GROUP_OR_DIRECTORY_GROUP'){2}else{4};$f.tasks=@((Task $null 'FIXTURE_GROUP_NAME' 4));$r=Run;Need ($r.tasks[0].principal.group_category -ceq $category -and $r.tasks[0].principal.membership -ceq 'ENABLED_IN_OBSERVED_WORKER_PRIMARY_TOKEN')}}
foreach($code in @(5,1332,1722)){Test ('group-resolver-error-hold-'+$code) {$f.resolve_error=$code;$f.tasks=@((Task $null 'FIXTURE_GROUP_NAME' 4));$r=Run;Need ($r.tasks[0].principal.status -ceq 'UNKNOWN' -and @($r.tasks[0].principal.errors.codes|Where-Object win32 -eq $code).Count -gt 0 -and $r.status -ceq 'HOLD')}}
Test 'empty-group-no-null-name-lookup' {$f.tasks=@((Task $null $null 4));$r=Run;Need ($r.tasks[0].principal.status -ceq 'UNKNOWN' -and $f.resolve_calls -eq 0)}
Test 'both-user-group-ambiguous' {$f.tasks=@((Task $worker 'FIXTURE_GROUP_NAME' 4));$r=Run;Need ($r.tasks[0].principal.status -ceq 'UNKNOWN' -and $f.resolve_calls -eq 0)}
Test 'group-not-used-as-batch-principal' {$f.tasks=@((Task $null 'FIXTURE_GROUP_NAME' 1));$r=Run;Need ($r.tasks[0].principal.status -ceq 'UNKNOWN')}
Test 'non-group-os-sid-use-rejected' {$f.group_use=1;$f.tasks=@((Task $null 'FIXTURE_GROUP_NAME' 4));$r=Run;Need ($r.tasks[0].principal.status -ceq 'UNKNOWN')}
Test 'actual-token-group-not-inferred-from-name' {$f.native.token_groups=@();$f.tasks=@((Task $null 'FIXTURE_GROUP_NAME' 4));$r=Run;Need ($r.tasks[0].principal.membership -ceq 'NOT_PRESENT_IN_OBSERVED_TOKENS_NOT_FUTURE_EXCLUSION')}
Test 'deny-only-attributes-do-not-enable-group' {$f.native.token_groups[0].attributes=[uint32]20;$f.native.token_groups[0].enabled=$true;$f.native.token_groups[0].deny_only=$false;$f.tasks=@((Task $null 'FIXTURE_GROUP_NAME' 4));$r=Run;Need ($r.tasks[0].principal.membership -ceq 'DENY_ONLY_IN_OBSERVED_TOKEN')}
Test 'disabled-token-group-not-enabled' {$f.native.token_groups[0].attributes=[uint32]0;$f.tasks=@((Task $null 'FIXTURE_GROUP_NAME' 4));$r=Run;Need ($r.tasks[0].principal.enabled_token_evidence.Count -eq 0)}
Test 'restricted-token-context-not-unconditional-membership' {$f.native.restricted_sids=@(@{sid='S-1-0-0';attributes=0});$f.tasks=@((Task $null 'FIXTURE_GROUP_NAME' 4));$r=Run;Need ($r.tasks[0].principal.membership -ceq 'ENABLED_GROUP_RESTRICTED_TOKEN_CONTEXT_UNKNOWN')}
Test 'token-denied-group-membership-unknown' {$f.native.token_query_known=$false;$f.tasks=@((Task $null 'FIXTURE_GROUP_NAME' 4));$r=Run;Need ($r.tasks[0].principal.membership -ceq 'UNKNOWN_NO_VERIFIED_WORKER_TOKEN')}
Test 'group-membership-not-a-future-token-guarantee' {$f.tasks=@((Task $null 'FIXTURE_GROUP_NAME' 4));$r=Run;Need ($r.tasks[0].principal.future_token_membership -ceq 'UNKNOWN' -and $r.status -ceq 'HOLD')}
Test 'configured-user-principal-bound-to-worker-sid' {$f.tasks=@((Task $worker $null 1));$r=Run;Need ($r.tasks[0].principal.worker_configured_user -and $r.tasks[0].principal.task_scheduler_logon_label -ceq 'PASSWORD')}
Test 'parent-pid-reuse-detected' {$f.parent_pid=99;$f.parent_time='2026-10-24T00:00:00Z';$r=Run;Need ($r.parents[0].relation -ceq 'PARENT_PID_REUSED')}
Test 'parent-order-not-launch-proof' {$f.parent_pid=99;$r=Run;Need ($r.parents[0].relation -ceq 'GENERATION_ORDER_COMPATIBLE_NOT_LAUNCH_PROOF')}
Test 'direct-child-scope-and-creation' {$f.children=@(@{ProcessId=2;ParentProcessId=1;CreationDate=[datetime]::Parse($time)});$r=Run;Need ($r.children.Count -eq 1 -and $r.children[0].scope -ceq 'DIRECT_CHILD')}
Test 'direct-child-limit-hold-not-global-scan' {$f.children=@(2..70|ForEach-Object{@{ProcessId=$_;ParentProcessId=1;CreationDate=[datetime]::Parse($time)}});$r=Run;Need ($r.children.Count -eq 64 -and $r.errors.Count -gt 0)}
Test 'child-before-parent-generation-hold' {$f.children=@(@{ProcessId=2;ParentProcessId=1;CreationDate=[datetime]'2026-10-22T00:00:00Z'});$r=Run;Need ($r.children.Count -eq 0 -and $r.errors.Count -gt 0)}
Test 'any-job-membership-not-v2-job-proof' {$r=Run;Need ($r.targets[0].in_any_job -and $r.targets[0].job_identity -ceq 'UNKNOWN_NO_FOREIGN_JOB_HANDLE_OPENED' -and $r.status -ceq 'HOLD')}
Test 'task-engine-pid-not-action-or-job-proof' {$f.tasks=@((Task $worker $null 1));$r=Run;Need ($r.tasks[0].engine_pid_relation -ceq 'ENGINE_PID_IS_NOT_ACTION_PID_OR_JOB_PROOF')}
Test 'task-denied-preserves-process-evidence' {$f.task_error=$true;$r=Run;Need ($r.targets[0].identity -ceq 'MATCH' -and $r.errors.Count -gt 0 -and $r.status -ceq 'HOLD')}
Test 'lease-absence-not-exclusivity' {$r=Run;Need ($r.lease_metadata.entries.worker_lease.status -ceq 'ABSENT' -and -not $r.global_exclusive_execution_verified)}
Test 'lease-present-does-not-acquire-or-clear' {$f.lease_status='PRESENT_NOT_OPENED';$r=Run;Need ($r.lease_metadata.locks_acquired -eq 0 -and $r.status -ceq 'HOLD')}
Test 'lease-denied-hold' {$f.lease_error=$true;$r=Run;Need ($r.errors.Count -gt 0 -and $r.status -ceq 'HOLD')}
Test 'logon-luid-denied-keeps-context-unknown' {$f.logon_unknown=$true;$r=Run;Need ($r.targets[0].errors.Count -gt 0 -and -not $r.targets[0].security_logon_label)}
Test 'eleven-target-rows-preserved-no-force-zero' {$baseline=@(1..11|ForEach-Object{@{pid=$_;created_utc=$time}});$r=Invoke-WorkerInvestigation $baseline $worker (Provider);Need ($r.targets.Count -eq 11 -and $r.baseline_process_count -eq 11 -and $r.r11_foreign_zero_guard_unchanged -and -not $r.ContainsKey('foreign_processes'))}
Test 'private-path-extra-fields-never-serialized' {$f.native.secret_extra='SECRET_SENTINEL';$f.native.token_groups[0].secret_extra='SECRET_SENTINEL';$f.image.secret_extra='SECRET_SENTINEL';$r=Run;$json=$r|ConvertTo-Json -Depth 24;Need ($json -notmatch 'SECRET_SENTINEL|PRIVATE_IMAGE_PATH_SENTINEL|image_query_path')}
Test 'exception-message-not-serialized' {$f.task_error=$false;$p=Provider;$p.Tasks={throw 'SECRET_SENTINEL'};$r=Invoke-WorkerInvestigation (Baseline) $worker $p;Need (($r|ConvertTo-Json -Depth 20) -notmatch 'SECRET_SENTINEL')}
Test 'runner-parse-and-no-forbidden-members-or-commands' {
 $tokens=$null;$errors=$null;$a=[Management.Automation.Language.Parser]::ParseFile((Join-Path $PSScriptRoot 'investigate-worker-readonly.ps1'),[ref]$tokens,[ref]$errors);Need ($errors.Count -eq 0)
 Need (@($a.FindAll({param($n)$n -is [Management.Automation.Language.ExitStatementAst]},$true)).Count -eq 0)
 Need (@($a.FindAll({param($n)$n -is [Management.Automation.Language.MemberExpressionAst] -and $n.Member.Value -match '^(CommandLine|Arguments|Xml|Credential|Password|EnvironmentVariables|Run|RegisterTaskDefinition|Terminate|Kill)$'},$true)).Count -eq 0)
 Need (@($a.FindAll({param($n)$n -is [Management.Automation.Language.CommandAst] -and $n.GetCommandName() -match '^(Stop-|Start-Process|Set-Acl|Remove-|New-LocalUser|Register-ScheduledTask|Get-WinEvent|Invoke-RestMethod|Invoke-WebRequest|Get-LocalGroupMember)'},$true)).Count -eq 0)
}
Test 'native-only-query-access-and-no-dangerous-apis' {
 $text=[IO.File]::ReadAllText((Join-Path $PSScriptRoot 'WorkerReadOnlyNative.cs'))
 Need ($text.Contains('OpenProcess(0x1000,false,pid)') -and $text.Contains('OpenProcessToken(h,8,out token)') -and $text.Contains('CreateFileW(path,0x80000000,1,IntPtr.Zero,3'))
 Need ($text -notmatch 'AdjustTokenPrivileges|SeDebugPrivilege|ReadProcessMemory|WriteProcessMemory|TerminateProcess|CreateProcess|LogonUser|Impersonate|TokenInfo\(token,3,')
 Need ($text.Contains('Flags=0x1010') -and $text.Contains('Handle=h.DangerousGetHandle()'))
}
$summary=@{passed=@($tests|Where-Object status -eq 'PASS').Count;failed=@($tests|Where-Object status -eq 'FAIL').Count;tests=@($tests);live_process_queries=0;live_token_queries=0;live_scheduler_queries=0;signature_api_calls=0;system_changes=0}
$json=$summary|ConvertTo-Json -Depth 8;if($Output){[IO.File]::WriteAllText($Output,$json,[Text.UTF8Encoding]::new($false))};$json

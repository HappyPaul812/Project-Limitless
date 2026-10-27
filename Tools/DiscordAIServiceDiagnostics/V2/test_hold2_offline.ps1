param([string]$SourceRoot=$PSScriptRoot)
$ErrorActionPreference='Stop'
. (Join-Path $SourceRoot 'V2WorkerQuery.ps1')
. (Join-Path $SourceRoot 'V2StagingPlan.ps1')
$tests=[Collections.Generic.List[object]]::new()
function Test($name,[scriptblock]$body){try{& $body;$tests.Add(@{name=$name;status='PASS'})}catch{$tests.Add(@{name=$name;status='FAIL';type=$_.Exception.GetType().FullName})}}
function Need($value){if(-not $value){throw 'TEST_FAIL'}}
$worker='S-1-5-21-2858504392-1958168942-1662906075-1007'
function Provider {
 @{
 Account={param($s)[pscustomobject]@{SID=$s}};Sessions={[pscustomobject]@{LogonId='fake'}};Processes={@()};Tasks={@()};Translate={param($v)if($v -ceq 'fake-worker'){$worker}else{throw [Security.Principal.IdentityNotMappedException]::new('SECRET')}};GroupMember={param($g,$s)$false};Lease={'ABSENT'}
 }
}
Test 'original-empty-NTAccount-exact-error' {try{[Security.Principal.NTAccount]::new('');throw 'EXPECTED'}catch{Need ($_.Exception.InnerException -is [ArgumentException]);Need ($_.Exception.InnerException.HResult -eq -2147024809)}}
Test 'empty-principal-rejected-before-translation' {$p=Provider;$p.Tasks={@{Principal=@{UserId='';GroupId='';LogonType=4};Enabled=$true;State=3}};$p.Translate={throw 'MUST_NOT_CALL'};$r=Invoke-V2WorkerQuery $worker $p;Need ($r.status -ceq 'HOLD' -and $r.errors[0].argument_shape.user_length -eq 0 -and $r.lease_query_known)}
Test 'null-principal-hold' {$p=Provider;$p.Tasks={@{Principal=@{UserId=$null;GroupId=$null;LogonType=0};Enabled=$false;State=3}};Need ((Invoke-V2WorkerQuery $worker $p).status -ceq 'HOLD')}
Test 'typed-principal-hold' {try{Resolve-V2QueryPrincipal @{UserId=5;GroupId='';LogonType=1} {throw 'SECRET'};throw 'EXPECTED'}catch{Need ($_.Exception.Message -ceq 'USER_TYPE_HOLD')}}
Test 'group-principal-resolved-no-empty-constructor' {$r=Resolve-V2QueryPrincipal @{UserId='';GroupId='S-1-5-32-545';LogonType=4} {throw 'MUST_NOT_CALL'};Need ($r.kind -ceq 'group')}
Test 'group-logon-invalid' {try{Resolve-V2QueryPrincipal @{UserId='';GroupId='S-1-5-32-545';LogonType=1} {};throw 'EXPECTED'}catch{Need ($_.Exception.Message -ceq 'GROUP_LOGON_HOLD')}}
Test 'both-user-group-hold' {try{Resolve-V2QueryPrincipal @{UserId=$worker;GroupId='S-1-5-32-545';LogonType=4} {};throw 'EXPECTED'}catch{Need ($_.Exception.Message -ceq 'PRINCIPAL_AMBIGUOUS_HOLD')}}
Test 'fake-idle-pass' {Need ((Invoke-V2WorkerQuery $worker (Provider)).status -ceq 'PASS')}
Test 'eleven-processes-busy-not-query-failure' {$p=Provider;$p.Processes={1..11|ForEach-Object{[pscustomobject]@{ProcessId=$_;CreationDate=[datetime]'2026-10-27T00:00:00Z'}}};$r=Invoke-V2WorkerQuery $worker $p;Need ($r.known -and $r.foreign_processes -eq 11 -and $r.status -ceq 'HOLD')}
Test 'pid-time-reuse-hold' {$p=Provider;$p.Processes={@{ProcessId=5;CreationDate=[datetime]'2026-10-27T00:00:00Z'},@{ProcessId=5;CreationDate=[datetime]'2026-10-27T00:00:01Z'}};Need ((Invoke-V2WorkerQuery $worker $p).status -ceq 'HOLD')}
Test 'process-time-missing-hold' {$p=Provider;$p.Processes={@{ProcessId=5;CreationDate=$null}};Need ((Invoke-V2WorkerQuery $worker $p).status -ceq 'HOLD')}
Test 'account-sid-mismatch-hold' {$p=Provider;$p.Account={@{SID='S-1-5-18'}};Need ((Invoke-V2WorkerQuery $worker $p).status -ceq 'HOLD')}
Test 'task-worker-enabled-conflict' {$p=Provider;$p.Tasks={@{Principal=@{UserId=$worker;GroupId='';LogonType=1};Enabled=$true;State=3}};Need ((Invoke-V2WorkerQuery $worker $p).other_launchable_tasks -eq 1)}
Test 'task-worker-disabled-running-conflict' {$p=Provider;$p.Tasks={@{Principal=@{UserId=$worker;GroupId='';LogonType=1};Enabled=$false;State=4}};Need ((Invoke-V2WorkerQuery $worker $p).status -ceq 'HOLD')}
Test 'task-unmapped-hold-no-secret' {$p=Provider;$p.Tasks={@{Principal=@{UserId='unmapped';GroupId='';LogonType=1};Enabled=$true;State=3}};$r=Invoke-V2WorkerQuery $worker $p;Need ($r.status -ceq 'HOLD' -and ($r|ConvertTo-Json -Depth 12) -notmatch 'SECRET')}
Test 'group-worker-conflict' {$p=Provider;$p.Tasks={@{Principal=@{UserId='';GroupId='S-1-5-32-545';LogonType=4};Enabled=$true;State=3}};$p.GroupMember={$true};Need ((Invoke-V2WorkerQuery $worker $p).other_launchable_tasks -eq 1)}
Test 'group-membership-unknown-hold' {$p=Provider;$p.Tasks={@{Principal=@{UserId='';GroupId='S-1-5-32-545';LogonType=4};Enabled=$true;State=3}};$p.GroupMember={$null};Need ((Invoke-V2WorkerQuery $worker $p).status -ceq 'HOLD')}
Test 'lease-present-never-probed-hold' {$p=Provider;$p.Lease={'PRESENT_NOT_PROBED'};Need ((Invoke-V2WorkerQuery $worker $p).status -ceq 'HOLD')}
Test 'lease-denied-hold' {$p=Provider;$p.Lease={throw [ComponentModel.Win32Exception]::new(5)};Need ((Invoke-V2WorkerQuery $worker $p).status -ceq 'HOLD')}
# 변경한 원래 함수 자체도 fake CIM/Scheduler로 메모리 호출합니다.
Add-Type 'public static class LimitlessV2Contract { public const string TaskName="LIMITLESS-Trial-V2-20261027-S0Host"; public static bool Sid(string s){return s!=null&&s.StartsWith("S-1-");} public static object[] ErrorCodes(System.Exception e){return new object[]{e.GetType().FullName,e.HResult};}}'
$tok=$null;$err=$null;$tree=[Management.Automation.Language.Parser]::ParseFile((Join-Path $SourceRoot 'V2WindowsAdapter.ps1'),[ref]$tok,[ref]$err)
. ([scriptblock]::Create($tree.Find({param($n)$n -is [Management.Automation.Language.FunctionDefinitionAst] -and $n.Name -eq 'Get-V2WorkerSnapshot'},$true).Extent.Text))
function Get-CimInstance {param($Class,$Filter,$ErrorAction)@{SID=$worker}}
function Get-CimAssociatedInstance {param($InputObject,$Association,$ResultClassName,$ErrorAction)if($ResultClassName -eq 'Win32_LogonSession'){@{id='FAKE'}}else{@()}}
function Get-LocalGroupMember {param($SID,$ErrorAction)[pscustomobject]@{ObjectClass='Group';SID=[pscustomobject]@{Value='S-1-5-32-545'}}}
function New-Object {param($ComObject)
 $folder=[pscustomobject]@{}
 $folder|Add-Member ScriptMethod GetTasks {param($flags)$fixtureTasks}
 $folder|Add-Member ScriptMethod GetFolders {param($flags)@()}
 $scheduler=[pscustomobject]@{folder=$folder};$scheduler|Add-Member ScriptMethod Connect {};$scheduler|Add-Member ScriptMethod GetFolder {param($path)$this.folder};$scheduler
}
Test 'original-function-empty-principal-hold-with-shape' {$fixtureTasks=@(@{Definition=@{Principal=@{UserId='';GroupId='S-1-5-32-545';LogonType=4}};Enabled=$true;Path='fake'});$r=Get-V2WorkerSnapshot $worker;Need (-not $r.known -and $r.errors[0].argument_shape.user_length -eq 0 -and $r.stage -ceq 'TASK_PRINCIPAL_RESOLUTION')}
Test 'original-function-memory-task-counter-two' {$fixtureTasks=@(1..2|ForEach-Object{@{Definition=@{Principal=@{UserId=$worker;GroupId='';LogonType=1}};Enabled=$true;Path='fake'}});$r=Get-V2WorkerSnapshot $worker;Need ($r.known -and $r.other_launchable_tasks -eq 2)}
$good=@{path='C:\Program Files\LimitlessV2Staging20261027\R9-01f8a36c9bfe4241b24b7eaa6caaddc2';created_new=$true;source_preserved=$true;parent_owner_verified=$true;parent_write_safe=$true;handle_ancestors_pinned=$true;creation_sddl='O:BAG:SYD:P(A;OICI;FA;;;SY)(A;OICI;FA;;;BA)';zip_sha='a4a652bdf73c9a1007bd1e761ae539d3459896b46d8a021fa1459012a1d15c41';manifest_sha='250bab2d46010841353a319fbcf34581784366ac3042e6bc0fdc97aadd17c09e';payload_count=766;entry_count=767;exact_set=$true;all_payload_sha=$true;all_owners_ba_sy=$true;all_dacl_exact=$true;readback_verified=$true;reparse_count=0;hardlink_count_max=1;ads_count=0;unexpected_count=0;case_alias_count=0;exclusive_source_bytes=$true;atomic_create_new=$true;install_revalidation_required=$true}
Test 'staging-fake-good-no-create' {$r=Test-V2StagingProof $good;Need ($r.status -ceq 'OFFLINE_PLAN_PASS_NOT_CREATED' -and -not $r.created -and -not $r.native_verified)}
foreach($key in $good.Keys){Test ('staging-reject-'+$key) {$bad=$good.Clone();$bad[$key]=switch($good[$key].GetType().Name){Boolean {$false};Int32 {-1};default {'WRONG'}};Need ((Test-V2StagingProof $bad).status -ceq 'HOLD')}}
@{passed=@($tests|Where-Object status -eq 'PASS').Count;failed=@($tests|Where-Object status -eq 'FAIL').Count;tests=@($tests);native_calls=0;staging_created=$false}|ConvertTo-Json -Depth 8

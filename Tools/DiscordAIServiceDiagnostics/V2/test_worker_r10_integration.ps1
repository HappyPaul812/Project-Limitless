param([string]$Output)
$ErrorActionPreference='Stop'
. (Join-Path $PSScriptRoot 'V2WorkerQuery.ps1')
$tok=$null;$err=$null;$tree=[Management.Automation.Language.Parser]::ParseFile((Join-Path $PSScriptRoot 'V2WindowsAdapter.ps1'),[ref]$tok,[ref]$err)
. ([scriptblock]::Create($tree.Find({param($n)$n -is [Management.Automation.Language.FunctionDefinitionAst] -and $n.Name -eq 'Get-V2WorkerSnapshot'},$true).Extent.Text))
$results=[Collections.Generic.List[object]]::new();$worker='S-1-5-21-2858504392-1958168942-1662906075-1007'
function Need($ok){if(-not $ok){throw 'ASSERT'}}
function Test($name,[scriptblock]$body){$fixture=@{tasks=@();processes=@();group_member=$false;group_unknown=$false;denied=$false};try{& $body;$results.Add(@{name=$name;status='PASS'})}catch{$results.Add(@{name=$name;status='FAIL';type=$_.Exception.GetType().FullName})}}
function Get-CimInstance {param($Class,$Filter,$ErrorAction)if($fixture.denied){throw [ComponentModel.Win32Exception]::new(5)};[pscustomobject]@{SID=$worker}}
function Get-CimAssociatedInstance {param($InputObject,$Association,$ResultClassName,$ErrorAction)if($ResultClassName -ceq 'Win32_LogonSession'){@{id='fake'}}else{$fixture.processes}}
function Get-LocalGroupMember {param($SID,$ErrorAction)if($fixture.group_unknown){throw 'SECRET'};if($fixture.group_member){[pscustomobject]@{ObjectClass='User';SID=[pscustomobject]@{Value=$worker}}}}
function New-Object {param($ComObject)
 if($ComObject -cne 'Schedule.Service'){throw 'UNEXPECTED_NATIVE'}
 $folder=[pscustomobject]@{};$folder|Add-Member ScriptMethod GetTasks {param($flags)$fixture.tasks};$folder|Add-Member ScriptMethod GetFolders {param($flags)@()}
 $scheduler=[pscustomobject]@{folder=$folder};$scheduler|Add-Member ScriptMethod Connect {};$scheduler|Add-Member ScriptMethod GetFolder {param($path)$this.folder};$scheduler
}
function Task($user,$group,$logon,$enabled,$state){[pscustomobject]@{Definition=@{Principal=@{UserId=$user;GroupId=$group;LogonType=$logon}};Enabled=$enabled;State=$state}}
Test 'adapter-idle-uses-shared-query' {$r=Get-V2WorkerSnapshot $worker;Need ($r.known -and $r.foreign_processes -eq 0 -and $r.other_launchable_tasks -eq 0 -and $r.lease_checked_by_install_core)}
Test 'adapter-enabled-user-tasks-two' {$fixture.tasks=@((Task $worker '' 1 $true 3),(Task $worker '' 1 $true 3));$r=Get-V2WorkerSnapshot $worker;Need ($r.known -and $r.other_launchable_tasks -eq 2)}
Test 'adapter-group-empty-user-resolves-with-membership' {$fixture.tasks=@((Task '' 'S-1-5-32-545' 4 $true 3));$fixture.group_member=$true;$r=Get-V2WorkerSnapshot $worker;Need ($r.known -and $r.other_launchable_tasks -eq 1)}
Test 'adapter-unrelated-group-no-conflict' {$fixture.tasks=@((Task '' 'S-1-5-32-544' 4 $true 3));$r=Get-V2WorkerSnapshot $worker;Need ($r.known -and $r.other_launchable_tasks -eq 0)}
Test 'adapter-group-unknown-hold-no-secret' {$fixture.tasks=@((Task '' 'S-1-5-32-544' 4 $true 3));$fixture.group_unknown=$true;$r=Get-V2WorkerSnapshot $worker;Need (-not $r.known -and ($r|ConvertTo-Json -Depth 10) -notmatch 'SECRET')}
Test 'adapter-disabled-running-conflict' {$fixture.tasks=@((Task $worker '' 1 $false 4));$r=Get-V2WorkerSnapshot $worker;Need ($r.known -and $r.other_launchable_tasks -eq 1)}
Test 'adapter-eleven-processes-busy' {$fixture.processes=@(1..11|ForEach-Object{@{ProcessId=$_;CreationDate=[datetime]'2026-10-27T00:00:00Z'}});$r=Get-V2WorkerSnapshot $worker;Need ($r.known -and $r.foreign_processes -eq 11)}
Test 'adapter-denied-real-code-no-idle-pass' {$fixture.denied=$true;$r=Get-V2WorkerSnapshot $worker;Need (-not $r.known -and @($r.errors[0].codes|Where-Object win32 -eq 5).Count -eq 1)}
Test 'adapter-no-allowed-pid-bypass' {$r=Get-V2WorkerSnapshot $worker @(5);Need (-not $r.known)}
Test 'installer-staging-before-password-and-core' {$text=[IO.File]::ReadAllText((Join-Path $PSScriptRoot 'install-session0-v2.ps1'));$guard=$text.IndexOf('[LimitlessV2Staging]::Verify');Need ($guard -gt 0 -and $guard -lt $text.IndexOf('$password=Read-Host') -and $guard -lt $text.IndexOf('Invoke-V2InstallCore $api'));Need ($text.Contains("Code/V2/V2WorkerQuery.ps1"))}
Test 'probe-original-sha-preserved' {Need ((Get-FileHash -LiteralPath (Join-Path $PSScriptRoot 'probe-worker-hold2-readonly.ps1')).Hash.ToLowerInvariant() -ceq '0fa7933f00ad42ab7fcd20dc2d07063d6b076aa72718841948aab57500d85307')}
$summary=@{passed=@($results|Where-Object status -eq 'PASS').Count;failed=@($results|Where-Object status -eq 'FAIL').Count;tests=@($results);native_calls=0;actual_workers=0}
$json=$summary|ConvertTo-Json -Depth 8;if($Output){[IO.File]::WriteAllText($Output,$json,[Text.UTF8Encoding]::new($false))};$json

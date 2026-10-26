$ErrorActionPreference='Stop'
$path=Join-Path $PSScriptRoot 'probe-partial-install-readonly.ps1'
$tokens=$null;$errors=$null
$ast=[Management.Automation.Language.Parser]::ParseFile($path,[ref]$tokens,[ref]$errors)
if($errors.Count){throw 'DIAGNOSTIC_SYNTAX_FAIL'}
foreach($name in @('Safe-Error','Read-Probe','Presence','Read-TrialTask','Event-Projection')){
 $n=$ast.Find({param($a)$a -is [Management.Automation.Language.FunctionDefinitionAst] -and $a.Name -eq $name},$true)
 . ([scriptblock]::Create($n.Extent.Text))
}
$script:pass=0;$script:fail=0
function Require($Condition){if(-not $Condition){throw 'ASSERTION_FAIL'}}
function Test($Name,[scriptblock]$Body){try{& $Body;$script:pass++;"PASS $Name"}catch{$script:fail++;"FAIL $Name : $($_.Exception.GetType().Name)"}}
Test 'no-exit-statement' {Require ($null -eq $ast.Find({param($a)$a -is [Management.Automation.Language.ExitStatementAst]},$true))}
Test 'no-install-or-system-mutation-commands' {
 $cmds=$ast.FindAll({param($a)$a -is [Management.Automation.Language.CommandAst]},$true)|ForEach-Object{$_.GetCommandName()}
 Require (@($cmds|Where-Object{$_ -match '^(Set-|Remove-|Register-|Unregister-|Enable-|Disable-|Start-|Stop-|Restart-|New-Local|Add-Local|Read-Host|Get-Credential|Invoke-Expression|Invoke-Command)'}).Count -eq 0)
}
Test 'no-mutating-com-methods' {
 $members=$ast.FindAll({param($a)$a -is [Management.Automation.Language.InvokeMemberExpressionAst]},$true)|ForEach-Object{$_.Member.Value}
 Require (@($members|Where-Object{$_ -in @('RegisterTaskDefinition','SetSecurityDescriptor','Run','RunEx','DeleteTask','CreateFolder','GrantNewBotBatch')}).Count -eq 0)
}
Test 'readonly-lsa-reader-compiles-no-native-call' {
 $n=$ast.Find({param($a)$a -is [Management.Automation.Language.StringConstantExpressionAst] -and $a.Value.StartsWith('using System;')},$true)
 Require ($n.Value -notmatch 'LsaAddAccountRights|LsaRemoveAccountRights' -and $n.Value.Contains('0x800,out handle'))
 Add-Type -TypeDefinition $n.Value
}
Test 'inner-hresult-preserved-without-message' {
 $inner=[ComponentModel.Win32Exception]::new(5,'SYNTHETIC_SECRET_NOT_FOR_OUTPUT')
 $record=[Management.Automation.ErrorRecord]::new([Exception]::new('SYNTHETIC_OTHER_SECRET',$inner),'FAKE','NotSpecified',$null)
 $r=Safe-Error $record;$j=$r|ConvertTo-Json -Depth 8
 Require ($r.errors.Count -eq 2 -and $r.errors[1].native_error_code -eq 5 -and $j -notmatch 'SYNTHETIC_')
}
Test 'read-probe-denied-is-unknown' {$r=Read-Probe {throw [UnauthorizedAccessException]::new('SYNTHETIC_SECRET')};Require ($r.status -eq 'UNKNOWN_READ_FAILED')}
Test 'absent-path-requires-readable-parent' {
 function Get-Item {throw [UnauthorizedAccessException]::new()}
 function Get-ChildItem {throw [UnauthorizedAccessException]::new()}
 $r=Presence 'X:/protected/Missing';Require ($r.status -eq 'UNKNOWN_READ_FAILED')
}
Test 'absence-with-successful-parent-list' {
 function Get-Item {throw [IO.FileNotFoundException]::new()}
 function Get-ChildItem {return @('Other')}
 $r=Presence 'X:/parent/Missing';Require ($r.status -eq 'ABSENT_FROM_READABLE_PARENT')
}
function Fake-Folder([int]$Hresult,[bool]$HasName=$false,[bool]$EnumFails=$false){
 $f=[pscustomobject]@{hr=$Hresult;hasName=$HasName;enumFails=$EnumFails;enumCalls=0}
 $f|Add-Member ScriptMethod GetTask {param($n)throw [Runtime.InteropServices.COMException]::new('SYNTHETIC_SECRET',$this.hr)}
 $f|Add-Member ScriptMethod GetTasks {param($flags)$this.enumCalls++;if($flags -ne 1 -or $this.enumFails){throw 'SYNTHETIC_ENUM_ERROR'};if($this.hasName){return [pscustomobject]@{Name='LIMITLESS-Trial-S0Host'}};return @()}
 return $f
}
Test 'nonadmin-not-found-stays-unknown' {$f=Fake-Folder -2147024894;$r=Read-TrialTask $f $false;Require ($r.status -eq 'UNKNOWN_READ_FAILED' -and $f.enumCalls -eq 0)}
Test 'admin-notfound-with-enumeration-confirms-absence' {$f=Fake-Folder -2147024894;$r=Read-TrialTask $f $true;Require ($r.status -eq 'ABSENT_ADMIN_ENUM_AND_NOT_FOUND' -and $f.enumCalls -eq 1)}
Test 'admin-enumeration-failure-stays-unknown' {$f=Fake-Folder -2147024894 $false $true;$r=Read-TrialTask $f $true;Require ($r.status -eq 'UNKNOWN_READ_FAILED')}
Test 'contradictory-task-result-stays-unknown' {$f=Fake-Folder -2147024894 $true;$r=Read-TrialTask $f $true;Require ($r.status -eq 'UNKNOWN_READ_FAILED')}
Test 'task-denied-is-not-absence' {$f=Fake-Folder -2147024891;$r=Read-TrialTask $f $true;Require ($r.status -eq 'UNKNOWN_READ_FAILED' -and $f.enumCalls -eq 0)}
Test 'event-projection-removes-secrets' {
 $r=Event-Projection 'install-session0-trial password=SYNTHETIC_PASSWORD token=SYNTHETIC_TOKEN CS0103 0x80070005 Access denied GrantNewBotBatch'
 $j=$r|ConvertTo-Json -Depth 5
 Require ($j -notmatch 'SYNTHETIC_|password|token=' -and 'CS0103' -in $r.error_tokens -and '0x80070005' -in $r.error_tokens -and 'GrantNewBotBatch' -in $r.known_functions)
}
Test 'unrelated-event-excluded' {Require ($null -eq (Event-Projection 'unrelated password=SYNTHETIC_SECRET'))}
Test 'event-text-bounded' {$r=Event-Projection ('x'*32768+' install-session0-trial CS9999');Require ($null -eq $r)}
@{passed=$script:pass;failed=$script:fail;actual_scheduler_calls=0;actual_lsa_calls=0;system_changes=0;installer_executions=0}|ConvertTo-Json -Compress
if($script:fail){throw 'OFFLINE_READONLY_TEST_HOLD'}

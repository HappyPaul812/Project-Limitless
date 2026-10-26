$ErrorActionPreference='Stop'
$diag=$PSScriptRoot
$installer=Join-Path $diag 'install-session0-trial.ps1'
$runner=Join-Path $diag 'run-atomic-trial-install.ps1'
$old='F:/study/codex/Project-Limitless/Temp/Session0TaskSecurityAtomicCandidate20261026/Deployment/install-session0-trial.ps1'
function Parse([string]$Path){$t=$null;$e=$null;$a=[Management.Automation.Language.Parser]::ParseFile($Path,[ref]$t,[ref]$e);if($e.Count){throw 'PARSE_HOLD'};return $a}
$ast=Parse $installer;$runnerAst=Parse $runner;$oldAst=Parse $old
function Fn($Tree,[string]$Name){$Tree.Find({param($n)$n -is [Management.Automation.Language.FunctionDefinitionAst] -and $n.Name -eq $Name},$true)}
foreach($name in @('Assert-TrialFreshState','Get-InstallFailureSummary','Assert-InstallLogSecurity','Get-TrialTaskSddl','Assert-TrialTaskSecurity','Register-TrialTaskAtomic')){. ([scriptblock]::Create((Fn $ast $name).Extent.Text))}
foreach($name in @('Get-RunnerFailure','Get-LogicalExitCode')){. ([scriptblock]::Create((Fn $runnerAst $name).Extent.Text))}
$source=[IO.File]::ReadAllText((Join-Path $diag 'TrialAccountPolicy.cs'))
Add-Type -TypeDefinition ($source+"`n"+[IO.File]::ReadAllText((Join-Path $diag 'LsaRepairOfflineTests.cs')))
$script:pass=0;$script:fail=0
function Need($v){if(-not $v){throw 'ASSERT_HOLD'}}
function Reject([scriptblock]$body){$hit=$false;try{& $body|Out-Null}catch{$hit=$true};Need $hit}
function Test([string]$Name,[scriptblock]$Body){try{& $Body;$script:pass++;"PASS $Name"}catch{$script:fail++;"FAIL $Name : $($_.Exception.GetType().Name)"}}
Test 'utf16-name-34-max36-null' {Need ([LimitlessLsaOffline.Tests]::ExactBuffer())}
Test 'unicode-byte-length-not-character-count' {Need ([LimitlessLsaOffline.Tests]::UnicodeBuffer())}
Test 'null-name-rejected' {Need ([LimitlessLsaOffline.Tests]::RejectName($null))}
Test 'empty-name-rejected' {Need ([LimitlessLsaOffline.Tests]::RejectName(''))}
Test 'embedded-null-rejected' {Need ([LimitlessLsaOffline.Tests]::RejectName("A`0B"))}
Test 'malformed-surrogate-rejected' {Need ([LimitlessLsaOffline.Tests]::RejectName([string][char]0xd800))}
Test 'ushort-boundary-before-allocation' {Need ([LimitlessLsaOffline.Tests]::Range())}
Test 'dispose-clears-buffer-and-is-idempotent' {Need ([LimitlessLsaOffline.Tests]::Dispose())}
Test 'fake-lsa-success-one-open-add-close' {Need ([LimitlessLsaOffline.Tests]::Success())}
Test 'fake-lsa-add-ntstatus-win32-preserved' {Need ([LimitlessLsaOffline.Tests]::AddFailure())}
Test 'fake-lsa-open-failure-no-add' {Need ([LimitlessLsaOffline.Tests]::OpenFailure())}
Test 'fake-lsa-close-failure-preserved' {Need ([LimitlessLsaOffline.Tests]::CloseFailure())}
Test 'fake-lsa-primary-error-not-masked-by-cleanup' {Need ([LimitlessLsaOffline.Tests]::PrimaryPreserved())}
Test 'fake-lsa-null-handle-blocked' {Need ([LimitlessLsaOffline.Tests]::NullHandle())}
Test 'public-sid-boundary-still-rejects-before-native' {Reject {[LimitlessTrialAccountPolicyV1]::GrantNewBotBatch('a','b','c','d')}}
Test 'public-admin-guard-and-native-access-unchanged' {Need ($source.Contains('IsInRole(WindowsBuiltInRole.Administrator)') -and $source.Contains('ref attr,0x810,out handle') -and $source.Contains('GrantBatchCore(bytes,new NativeApi())'))}
Test 'installer-nested-error-types-native-codes-no-secret' {
 $r=Get-InstallFailureSummary ([pscustomobject]@{Exception=[LimitlessLsaOffline.Tests]::Nested()}) 'GRANT_BATCH_RIGHT'
 $j=$r|ConvertTo-Json -Depth 10
 Need ($r.stage -eq 'GRANT_BATCH_RIGHT' -and $r.exceptions.Count -eq 2 -and $r.exceptions[1].ntstatus -eq [uint32]3221225568 -and $r.exceptions[1].win32 -eq 1313 -and $j -notmatch 'SYNTHETIC_TOKEN_SECRET')
}
Test 'runner-preserves-sanitized-install-error-through-inner' {
 $safe=Get-InstallFailureSummary ([pscustomobject]@{Exception=[LimitlessLsaOffline.Tests]::Nested()}) 'GRANT_BATCH_RIGHT'
 $e=[InvalidOperationException]::new('SYNTHETIC_PASSWORD');$e.Data['LimitlessInstallFailureV2']=$safe
 $r=Get-RunnerFailure ([pscustomobject]@{Exception=[Exception]::new('SYNTHETIC_TOKEN',$e)})
 Need ($r.install_failure.stage -eq 'GRANT_BATCH_RIGHT' -and ($r|ConvertTo-Json -Depth 12) -notmatch 'SYNTHETIC_')
}
Test 'unknown-native-code-not-invented' {$r=Get-InstallFailureSummary ([pscustomobject]@{Exception=[Exception]::new('SYNTHETIC_SECRET')}) 'DEFINE_TASK';Need (-not $r.exceptions[0].Contains('ntstatus') -and -not $r.exceptions[0].Contains('win32'))}
Test 'cleanup-native-codes-survive-sanitized-summary' {
 $e=[LimitlessLsaOffline.Tests]::Nested();$e.InnerException.Data['cleanup_ntstatus']=[uint32]3221225506;$e.InnerException.Data['cleanup_win32']=[uint32]5
 $r=Get-InstallFailureSummary ([pscustomobject]@{Exception=$e}) 'GRANT_BATCH_RIGHT'
 Need ($r.exceptions[1].ntstatus -eq [uint32]3221225568 -and $r.exceptions[1].cleanup_ntstatus -eq [uint32]3221225506 -and $r.exceptions[1].cleanup_win32 -eq 5)
}
$private='O:BAG:SYD:P(A;;FA;;;SY)(A;;FA;;;BA)'
Test 'private-log-exact-protected-two-aces' {Assert-InstallLogSecurity $private}
foreach($bad in @($private.Replace('D:P','D:'),$private.Replace('O:BA','O:SY'),($private+'(A;;GR;;;BU)'))){Test 'private-log-untrusted-dacl-rejected' {Reject {Assert-InstallLogSecurity $bad}}}
Test 'private-log-atomic-protected-no-append' {
 $text=[IO.File]::ReadAllText($installer)
 Need ($text.Contains('NewPrivateFile(string path,byte[] data){WriteFile(path,data,true);}') -and $text.Contains('(protect?"P":"")') -and $text.Contains('ref security,1,0x80'))
}
Test 'native-publisher-new-source-compiles-without-calls' {
 $n=$ast.Find({param($n)$n -is [Management.Automation.Language.StringConstantExpressionAst] -and $n.Value.Contains('public static class LimitlessAtomicTrialPublishV1')},$true)
 Add-Type -TypeDefinition $n.Value
}
foreach($case in @(@($true,$false,$true,$false),@($false,$true,$false,$false),@($false,$false,$false,$true))){Test 'partial-state-rejected-before-fake-effects' {$calls=0;Reject {Assert-TrialFreshState $case[0] $case[1] $case[2] $case[3];$calls++};Need ($calls -eq 0)}}
Test 'fresh-state-guard-allows-only-empty' {Assert-TrialFreshState $false $false $false $false}
$worker='S-1-5-21-111111111-222222222-333333333-1003'
function Fake-Folder([string]$Mode){
 $f=[pscustomobject]@{mode=$Mode;calls=0;reads=0;sddl=(Get-TrialTaskSddl $worker)}
 $f|Add-Member ScriptMethod RegisterTaskDefinition {
  param($name,$d,$flags,$u,$p,$logon,$sddl)
  $this.calls++;if($flags -ne 50 -or $sddl -cne $this.sddl -or $logon -ne 1){throw 'FAKE_FLAGS_HOLD'}
  if($this.mode -eq 'collision'){throw 'SYNTHETIC_NAME_EXISTS'}
  $r=[pscustomobject]@{Path=('\'+$name);Enabled=$false;Definition=$d;folder=$this}
  $r|Add-Member ScriptMethod GetSecurityDescriptor {param($mask)$this.folder.reads++;if($mask -ne 7){throw 'FAKE_MASK'};if($this.folder.mode -eq 'mismatch'){return $this.folder.sddl+'(A;;GR;;;BU)'};return $this.folder.sddl}
  return $r
 }
 return $f
}
foreach($mode in @('ok','collision','mismatch')){Test ('focused-fake-scheduler-'+$mode) {
 $d=[pscustomobject]@{Principal=[pscustomobject]@{UserId=$worker;LogonType=1;RunLevel=0};Settings=[pscustomobject]@{Enabled=$false;RestartCount=0};Triggers=[pscustomobject]@{Count=0}}
 $f=Fake-Folder $mode;$a=@{attempted=$false;verified=$false}
 $call={Register-TrialTaskAtomic $f $d 'LIMITLESS-Trial-S0Host' $worker 'PAUL-PC\LIMITLESSRelaySvc' 'SYNTHETIC_PASSWORD' $a}
 if($mode -eq 'ok'){$null=& $call;Need $a.verified}else{Reject $call;Need (-not $a.verified)}
 Reject $call;Need ($f.calls -eq 1)
}}
Test 'existing-atomic-sddl-four-function-bodies-preserved' {
 foreach($n in @('Get-TrialTaskSddl','Assert-TrialTaskSecurity','New-TrialTaskDefinition','Register-TrialTaskAtomic')){Need (((Fn $ast $n).Extent.Text.Replace("`r`n","`n")) -ceq ((Fn $oldAst $n).Extent.Text.Replace("`r`n","`n")))}
}
Test 'runner-no-exit-statement' {Need ($null -eq $runnerAst.Find({param($n)$n -is [Management.Automation.Language.ExitStatementAst]},$true))}
Test 'logical-hold-code-and-session-survives' {$v=Get-LogicalExitCode 'HOLD';$after='ALIVE';Need ($v -eq 2 -and $after -eq 'ALIVE' -and (Get-LogicalExitCode 'INSTALL_PASS_DISABLED_FAKE_ONLY_NO_EXECUTION') -eq 0)}
@{passed=$script:pass;failed=$script:fail;actual_lsa_calls=0;actual_scheduler_calls=0;windows_changes=0;installer_body_executions=0}|ConvertTo-Json -Compress
if($script:fail){throw 'NEW_LSA_OFFLINE_HOLD'}

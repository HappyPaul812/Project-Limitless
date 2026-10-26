param([string]$Installer=(Join-Path $PSScriptRoot 'install-session0-trial.ps1'))
$ErrorActionPreference='Stop'
# AST에서 순수 함수만 가져옵니다. 설치기 본문 및 실제 Scheduler COM은 실행하지 않습니다.
$tokens=$null;$errors=$null
$ast=[Management.Automation.Language.Parser]::ParseFile($Installer,[ref]$tokens,[ref]$errors)
if($errors.Count){throw 'INSTALLER_PARSE_FAIL'}
foreach($name in @('Get-TrialTaskSddl','Assert-TrialTaskSecurity','New-TrialTaskDefinition','Register-TrialTaskAtomic')){
 $node=$ast.Find({param($n) $n -is [Management.Automation.Language.FunctionDefinitionAst] -and $n.Name -eq $name},$true)
 if(-not $node){throw "FUNCTION_MISSING:$name"}
 . ([scriptblock]::Create($node.Extent.Text))
}
$worker='S-1-5-21-111111111-222222222-333333333-1003'
$good=Get-TrialTaskSddl $worker
$script:passed=0;$script:failed=0
function Check($Name,[scriptblock]$Body){
 try{& $Body;$script:passed++;Write-Output "PASS $Name"}catch{$script:failed++;Write-Output "FAIL $Name : $($_.Exception.Message)"}
}
function Must-Reject([scriptblock]$Body){$rejected=$false;try{& $Body|Out-Null}catch{$rejected=$true};if(-not $rejected){throw 'EXPECTED_HOLD'}}
function Require($Value){if(-not $Value){throw 'ASSERTION_FAILED'}}
function Fake-Scheduler {
 $scheduler=[pscustomobject]@{calls=0}
 $scheduler|Add-Member ScriptMethod NewTask {
  param($flags)
  $this.calls++
  $actions=[pscustomobject]@{item=$null}
  $actions|Add-Member ScriptMethod Create {param($type) if($type -ne 0){throw 'NOT_EXEC'};$this.item=[pscustomobject]@{Path='';Arguments='';WorkingDirectory=''};return $this.item}
  return [pscustomobject]@{RegistrationInfo=[pscustomobject]@{Description=''};Principal=[pscustomobject]@{UserId='';LogonType=0;RunLevel=1};Settings=[pscustomobject]@{Enabled=$true;MultipleInstances=0;ExecutionTimeLimit='';AllowHardTerminate=$false;DisallowStartIfOnBatteries=$true;StopIfGoingOnBatteries=$true;RestartCount=4};Triggers=[pscustomobject]@{Count=0};Actions=$actions}
 }
 return $scheduler
}
function Definition {
 return New-TrialTaskDefinition (Fake-Scheduler) $worker 'SYNTHETIC_FIXED_ARGUMENTS' 'X:/SYNTHETIC' 'LIMITLESS_TRIAL_BOUNDARY_V1'
}
function Fake-Folder([string]$Mode='ok',[string]$Readback=$good){
 $folder=[pscustomobject]@{calls=0;mode=$Mode;readback=$Readback;flags=$null;submitted=$null;reads=0;last=$null;existing='UNCHANGED'}
 $folder|Add-Member ScriptMethod RegisterTaskDefinition {
  param($name,$definition,$flags,$user,$password,$logon,$sddl)
  $this.calls++;$this.flags=$flags;$this.submitted=$sddl
  if($flags -ne 50 -or $logon -ne 1 -or $definition.Settings.Enabled -ne $false -or $user -ne 'PAUL-PC\LIMITLESSRelaySvc'){throw 'FAKE_BAD_REGISTRATION'}
  if($this.mode -in @('collision','failure','unknown-after-create')){throw 'SYNTHETIC_REGISTER_HOLD'}
  if($this.mode -eq 'null'){return $null}
  $registered=[pscustomobject]@{Path=('\'+$name);Enabled=$false;Definition=$definition;folder=$this}
  if($this.mode -eq 'wrong-path'){$registered.Path='\UNEXPECTED'}
  if($this.mode -eq 'enabled'){$registered.Enabled=$true}
  $registered|Add-Member ScriptMethod GetSecurityDescriptor {
   param($information)
   $this.folder.reads++
   if($information -ne 7){throw 'FAKE_SECURITY_INFORMATION'}
   if($this.folder.mode -eq 'read-failure'){throw 'SYNTHETIC_READ_HOLD'}
   return $this.folder.readback
  }
  $this.last=$registered;return $registered
 }
 return $folder
}
function Register-Fake($Folder,$Attempt,$Def=(Definition)){
 return Register-TrialTaskAtomic $Folder $Def 'LIMITLESS-Trial-S0Host' $worker 'PAUL-PC\LIMITLESSRelaySvc' 'SYNTHETIC_NOT_A_SECRET' $Attempt
}
Check 'definition-disabled-no-trigger-no-restart-limited-password' {
 $d=Definition;Require ($d.Settings.Enabled -eq $false -and $d.Triggers.Count -eq 0 -and $d.Settings.RestartCount -eq 0 -and $d.Principal.RunLevel -eq 0 -and $d.Principal.LogonType -eq 1 -and $d.Settings.MultipleInstances -eq 2 -and $d.Settings.ExecutionTimeLimit -eq 'PT15M')
}
Check 'single-create-with-sddl-and-immediate-readback' {
 $f=Fake-Folder;$a=@{attempted=$false;verified=$false};$null=Register-Fake $f $a
 Require ($f.calls -eq 1 -and $f.flags -eq 50 -and $f.submitted -ceq $good -and $f.reads -eq 1 -and $a.verified)
}
Check 'reordered-equivalent-aces-accepted' {Assert-TrialTaskSecurity ('O:BAG:SYD:P(A;;GR;;;'+$worker+')(A;;FA;;;BA)(A;;FA;;;SY)') $worker}
$bad=@{
 owner=$good.Replace('O:BA','O:SY');group=$good.Replace('G:SY','G:BA')
 unprotected=$good.Replace('D:P','D:');missing_dacl='O:BAG:SY'
 null_dacl='O:BAG:SYD:NO_ACCESS_CONTROL';empty_dacl='O:BAG:SYD:P'
 extra_principal=($good+'(A;;GR;;;BU)');duplicate=($good+'(A;;GR;;;'+$worker+')')
 worker_write=$good.Replace('(A;;GR;;;','(A;;GW;;;');worker_execute=$good.Replace('(A;;GR;;;','(A;;GX;;;')
 worker_full=$good.Replace('(A;;GR;;;','(A;;FA;;;');system_read=$good.Replace('(A;;FA;;;SY)','(A;;GR;;;SY)')
 deny=$good.Replace('(A;;GR;;;','(D;;GR;;;');inherited=$good.Replace('(A;;GR;;;','(A;ID;GR;;;')
 unknown_mapping=$good.Replace('(A;;GR;;;','(A;;0x120089;;;');malformed='NOT_SDDL'
}
foreach($pair in $bad.GetEnumerator()){
 Check ('readback-reject-'+$pair.Key) {
  $f=Fake-Folder 'ok' $pair.Value;$a=@{attempted=$false;verified=$false}
  Must-Reject {Register-Fake $f $a};Require ($f.calls -eq 1 -and $f.reads -eq 1 -and -not $a.verified)
 }
}
foreach($mode in @('collision','failure','unknown-after-create','null','read-failure','wrong-path','enabled')){
 Check ('hold-no-retry-'+$mode) {
  $f=Fake-Folder $mode;$a=@{attempted=$false;verified=$false}
  Must-Reject {Register-Fake $f $a};Must-Reject {Register-Fake $f $a}
  Require ($f.calls -eq 1 -and -not $a.verified -and $f.existing -eq 'UNCHANGED')
 }
}
Check 'duplicate-after-success-blocked' {
 $f=Fake-Folder;$a=@{attempted=$false;verified=$false};$null=Register-Fake $f $a;Must-Reject {Register-Fake $f $a};Require ($f.calls -eq 1)
}
foreach($property in @('enabled','trigger','logon','principal','restart','runlevel')){
 Check ('definition-reject-'+$property) {
  $d=Definition
  switch($property){enabled{$d.Settings.Enabled=$true} trigger{$d.Triggers.Count=1} logon{$d.Principal.LogonType=3} principal{$d.Principal.UserId='S-1-5-18'} restart{$d.Settings.RestartCount=1} runlevel{$d.Principal.RunLevel=1}}
  $f=Fake-Folder;$a=@{attempted=$false;verified=$false};Must-Reject {Register-Fake $f $a $d};Require ($f.calls -eq 0)
 }
}
Check 'worker-builtins-rejected' {Must-Reject {Get-TrialTaskSddl 'S-1-5-18'};Must-Reject {Get-TrialTaskSddl 'S-1-5-32-544'}}
Check 'no-post-create-security-repair-or-legacy-register' {
 $text=[IO.File]::ReadAllText($Installer)
 Require ($text -notmatch '\bSetSecurityDescriptor\s*\(' -and $text -notmatch '\bRegister-ScheduledTask\s')
}
@{status=if($script:failed){'FAIL'}else{'PASS'};passed=$script:passed;failed=$script:failed;actual_scheduler_calls=0;actual_accounts_created=0;model_calls=0;official_usage_reads=0;discord_calls=0}|ConvertTo-Json -Compress
if($script:failed){exit 1}

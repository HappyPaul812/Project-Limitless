param([Parameter(Mandatory)][string]$OutputPath)
$ErrorActionPreference='Stop';Set-StrictMode -Version Latest
$base='F:\study\codex\Project-Limitless\Temp\';$r1=$base+'Session0DedicatedV2Candidate20261027-r1';$r2=$base+'Session0DedicatedV2Candidate20261027-r2'
$oldManifest=Get-Content ($r1+'\HostManifest.json') -Raw|ConvertFrom-Json -AsHashtable;$newManifest=Get-Content ($r2+'\HostManifest.json') -Raw|ConvertFrom-Json -AsHashtable
$rows=@();$fake=[Security.SecureString]::new();$fake.MakeReadOnly()
function Read-PinnedAdapter($root,$manifest){$b=[IO.File]::ReadAllBytes($root+'\Code\V2\DedicatedWindows.ps1');if([Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($b)).ToLowerInvariant() -cne $manifest.files_sha256['Code/V2/DedicatedWindows.ps1']){throw 'ADAPTER_SOURCE_PIN'};[Text.Encoding]::UTF8.GetString($b)}
function Check($name,$body){try{if(-not (& $body)){throw 'ASSERT'};$script:rows+=@{name=$name;status='PASS'}}catch{$script:rows+=@{name=$name;status='FAIL';type=$_.Exception.GetType().FullName}}}
try{
 Check 'FROZEN_R1_EXACT_MEMBER_COLLISION_REPRODUCED' {
  . ([scriptblock]::Create((Read-PinnedAdapter $r1 $oldManifest)))
  try{$null=New-D3WindowsApi $oldManifest @{'Deployment/Preservation.json'=[IO.File]::ReadAllBytes($r1+'\Deployment\Preservation.json')} $fake $fake;$false}
  catch{$_.Exception.GetType().FullName -ceq 'System.InvalidOperationException' -and $_.Exception.HResult -eq -2146233079 -and $_.FullyQualifiedErrorId -ceq 'MemberAlreadyExists,Microsoft.PowerShell.Commands.AddMemberCommand'}
 }
 Check 'R2_ACTUAL_NATIVE_ADAPTER_CONSTRUCTS_WITH_STOP' {
  . ([scriptblock]::Create((Read-PinnedAdapter $r2 $newManifest)))
  $api=New-D3WindowsApi $newManifest @{'Deployment/Preservation.json'=[IO.File]::ReadAllBytes($r2+'\Deployment\Preservation.json')} $fake $fake
  $null -ne $api -and $api.created.Count -eq 0 -and $api.PSObject.Properties['consumeDirectory'].MemberType -eq 'NoteProperty' -and $api.PSObject.Members['Consume'].MemberType -eq 'ScriptMethod'
 }
 Check 'R2_ALL_THIRTEEN_NATIVE_METHODS_REGISTER_NO_FORCE' {
  . ([scriptblock]::Create((Read-PinnedAdapter $r2 $newManifest)))
  $api=New-D3WindowsApi $newManifest @{'Deployment/Preservation.json'=[IO.File]::ReadAllBytes($r2+'\Deployment\Preservation.json')} $fake $fake
  foreach($name in @('Context','Snapshot','Fresh','Consume','InstallLease','CreateUser','Rights','Publish','Binding','EnableWorker','Task','Verify','Record')){if($api.PSObject.Members[$name].MemberType -ne 'ScriptMethod'){return $false}}
  $null -eq $api.PSObject.Properties['consume'] -and (Read-PinnedAdapter $r2 $newManifest) -notmatch '(?i)Add-Member[^\r\n]*-Force'
 }
 Check 'ONLY_ADAPTER_PAYLOAD_CHANGED_FROM_R1' {
  $changed=@($newManifest.files_sha256.Keys|Where-Object {$newManifest.files_sha256[$_] -cne $oldManifest.files_sha256[$_]})
  $changed.Count -eq 1 -and $changed[0] -ceq 'Code/V2/DedicatedWindows.ps1'
 }
 Check 'EXACT_MINIMAL_RENAME_NO_SECURITY_RELAXATION' {
  $old=Read-PinnedAdapter $r1 $oldManifest;$new=Read-PinnedAdapter $r2 $newManifest
  $old.Replace('consume=$null','consumeDirectory=$null').Replace('$this.consume','$this.consumeDirectory') -ceq $new
 }
 Check 'NEW_PACKAGE_APPROVAL_EPOCH_PIPE_ISOLATED' {
  $newManifest.package_id -cne $oldManifest.package_id -and $newManifest.approval_id -cne $oldManifest.approval_id -and $newManifest.epoch -cne $oldManifest.epoch -and $newManifest.pipe_name -cne $oldManifest.pipe_name
 }
 Check 'CANDIDATE_FAKE_DISABLED_UNAPPROVED_AUTH_EMPTY_POLICY' {
  -not $newManifest.approved -and -not $newManifest.auto_start -and -not $newManifest.real_ai_enabled -and $newManifest.execution_mode -ceq 'FAKE_ONLY' -and $newManifest.auth_policy -ceq 'EMPTY_NO_IMPORT_NO_LOGIN' -and $newManifest.max_seconds -eq 90 -and $newManifest.max_parallel_jobs -eq 1
 }
 Check 'ADAPTER_SOURCE_PS_PARSE' {$t=$null;$e=$null;$null=[Management.Automation.Language.Parser]::ParseFile(($r2+'\Code\V2\DedicatedWindows.ps1'),[ref]$t,[ref]$e);$e.Count -eq 0}
}finally{$fake.Dispose()}
$r=@{kind='ACTUAL_NATIVE_ADAPTER_CONSTRUCTOR_ONLY';pass=@($rows|Where-Object status -eq PASS).Count;fail=@($rows|Where-Object status -eq FAIL).Count;tests=$rows;actual_backend_methods_invoked=0;actual_windows_changes=0;real_password_reads=0;model_calls=0}
$r|ConvertTo-Json -Depth 8|Set-Content -LiteralPath $OutputPath -Encoding utf8NoBOM;$r|Select-Object pass,fail|ConvertTo-Json
if($r.fail){throw 'NATIVE_ADAPTER_CONSTRUCTOR_TEST_HOLD'}

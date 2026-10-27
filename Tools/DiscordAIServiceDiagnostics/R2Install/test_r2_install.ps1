param([Parameter(Mandatory)][string]$OutputPath)
$ErrorActionPreference='Stop';Set-StrictMode -Version Latest
$base='F:\study\codex\Project-Limitless\Temp\Session0DedicatedV2Candidate20261027-r2'
$m=Get-Content ($base+'\HostManifest.json') -Raw|ConvertFrom-Json -AsHashtable
$fakeCs=[IO.File]::ReadAllText((Join-Path $PSScriptRoot '../V2/test_V2StagingFake.cs')) -replace '(?m)^using [A-Za-z0-9_.]+;',''
Add-Type -TypeDefinition ([IO.File]::ReadAllText($base+'\Code\V2\InstallAssembly.cs')+"`n"+[IO.File]::ReadAllText((Join-Path $PSScriptRoot '../R2Staging/R2StageBoundary.cs'))+"`n"+$fakeCs)
. ([scriptblock]::Create([IO.File]::ReadAllText($base+'\Code\V2\DedicatedWindows.ps1')))
. (Join-Path $PSScriptRoot 'R2InstallControl.ps1')
$pins=@{id=$m.approval_id;epoch=$m.epoch;ms='7c8410145ed84f06fc9901e93f78e18d0cc103ad89757172e08c84f91d8a3976';ins='83c97d781fb2d9aed797b41ea6bc799b83e939ad3ffa6f7af6a2d26f1080d68f';zip='4efa0aa8a38b32fa40fa5a2dfb4042e028eeb07b1ab8d73c713a1d80efe028d0'}
$rows=@()
function Check($n,$f){try{if(-not (& $f)){throw 'ASSERT'};$script:rows+=@{name=$n;status='PASS'}}catch{$script:rows+=@{name=$n;status='FAIL';type=$_.Exception.GetType().FullName}}}
function New-FakeControl($fault=''){
 $a=[pscustomobject]@{fault=$fault;calls=[Collections.Generic.List[string]]::new();used=$false;approval=$null}
 $a|Add-Member ScriptMethod Gates {$f=@{admin=$true;console=$true;stage=$true;native_constructor=$true;fresh_namespace=$true;r2_records_absent=(-not $this.used);old_preserved=$true};if($this.fault -in $f.Keys){$f[$this.fault]=$false};$f}
 $a|Add-Member ScriptMethod Passwords {$this.calls.Add('Passwords');if($this.fault -eq 'PASSWORD'){throw 'DUMMY_SECRET'}}
 $a|Add-Member ScriptMethod Revalidate {$this.calls.Add('Revalidate');if($this.fault -eq 'REVALIDATE'){throw 'DUMMY_SECRET'};@{admin_sid='S-1-5-21-1-2-3-1100';now=1000}}
 $a|Add-Member ScriptMethod Approve {param($raw)$this.calls.Add('Approve');$this.used=$true;$this.approval=$raw|ConvertFrom-Json -AsHashtable;if($this.fault -eq 'APPROVAL'){throw 'DUMMY_SECRET'}}
 $a|Add-Member ScriptMethod Install {param($mb,$payload,$raw,$zs)$this.calls.Add('Install');@{status=if($this.fault -eq 'NATIVE'){'HOLD'}else{'DEDICATED_INSTALLED_BOT_DISABLED_WORKER_BATCH_ONLY_TASK_DISABLED'};consumed=$true}}
 $a|Add-Member ScriptMethod Audit {param($r)$this.calls.Add('Audit');if($this.fault -eq 'AUDIT'){throw 'DUMMY_SECRET'};@{verified=$true}}
 $a|Add-Member ScriptMethod Record {param($r)$this.calls.Add('Record')}
 $a|Add-Member ScriptMethod ClearPasswords {$this.calls.Add('Clear')}
 $a
}
Check 'R2_ONE_INSTALL_APPROVAL_SCOPE_AND_CALL_ORDER' {$a=New-FakeControl;$r=Invoke-R2InstallOnce $a ([byte[]]@(1)) @{} $pins;$r.status -ceq 'R2_INSTALLED_VERIFIED_DISABLED_NO_START' -and ($a.calls -join ',') -ceq 'Passwords,Revalidate,Approve,Install,Audit,Record,Clear' -and $a.approval.approval_id -ceq $m.approval_id -and $a.approval.epoch -ceq $m.epoch -and $a.approval.expires_unix -eq 1900}
foreach($fault in @('admin','console','stage','native_constructor','fresh_namespace','r2_records_absent','old_preserved')){Check ('GATE_BEFORE_PASSWORD_AND_CREATION_'+$fault) {$a=New-FakeControl $fault;$r=Invoke-R2InstallOnce $a ([byte[]]@(1)) @{} $pins;$r.status -ceq 'HOLD' -and ($a.calls -join ',') -ceq 'Clear' -and $r.install_calls -eq 0}}
foreach($fault in @('PASSWORD','REVALIDATE','APPROVAL','NATIVE','AUDIT')){Check ('FAIL_HOLD_PRESERVE_NO_RETRY_'+$fault) {$a=New-FakeControl $fault;$r=Invoke-R2InstallOnce $a ([byte[]]@(1)) @{} $pins;$r.status -ceq 'HOLD' -and $r.automatic_retries -eq 0 -and @($a.calls|Where-Object {$_ -eq 'Install'}).Count -le 1 -and ($r|ConvertTo-Json -Depth 15) -notmatch 'DUMMY_SECRET'}}
Check 'DUPLICATE_APPROVAL_BLOCKED' {$a=New-FakeControl;$null=Invoke-R2InstallOnce $a ([byte[]]@(1)) @{} $pins;$r=Invoke-R2InstallOnce $a ([byte[]]@(1)) @{} $pins;$r.status -ceq 'HOLD' -and @($a.calls|Where-Object {$_ -eq 'Install'}).Count -eq 1}
Check 'R1_APPROVAL_ID_CANNOT_PASS_R2_NEW_PINS' {
 $old=@{schema=3;kind='DEDICATED_INSTALL_ONCE';approved=$true;approval_id='dedicated-install-a5af3087b78b45d1baae46067d5b7ac9';epoch='20261027-dedicated-a5af3087b78b45d1baae46067d5b7ac9';manifest_sha256=$pins.ms;installer_sha256=$pins.ins;zip_sha256=$pins.zip;admin_sid='S-1-5-21-1-2-3-1100';expires_unix=1900;preserve_existing=$true;create_bot='LIMITLESSBotV2D';create_worker='LIMITLESSWorkV2D';worker_install_state='ENABLED_BATCH_ONLY'}
 try{[DedicatedContract]::InstallApproval(($old|ConvertTo-Json -Compress),$pins.ms,$pins.ins,$pins.zip,$pins.id,$pins.epoch,$old.admin_sid,1000);$false}catch{$true}
}
Check 'ACTUAL_COORDINATOR_MEMBERS_CONSTRUCT_WITHOUT_COLLISION' {
 $source=[IO.File]::ReadAllText((Join-Path $PSScriptRoot 'install-r2-only.ps1'));$fs=[V2FakeStageFs]::new([byte[]]@(1));$fs.Add([R2StageBoundary]::R1,$true,$null);$r1Handle=$fs.Open([R2StageBoundary]::R1)
 $d=[Security.SecureString]::new();try{
  $nativeApi=New-D3WindowsApi $m @{'Deployment/Preservation.json'=[IO.File]::ReadAllBytes($base+'\Deployment\Preservation.json')} $d $d
  $p=$pins.Clone();$p.package=$m.package_id;$root=$m.approval_root;$stageSession=$null
  $start=$source.IndexOf('  $ctl=[pscustomobject]');$stop=$source.IndexOf("  `$phase='R2_EXPLICIT_INSTALL_ONCE'")
  . ([scriptblock]::Create($source.Substring($start,$stop-$start)))
  foreach($name in @('NoR2Records','OldFingerprint','Gates','Passwords','Revalidate','Approve','Install','Audit','Record','ClearPasswords')){if($ctl.PSObject.Members[$name].MemberType -ne 'ScriptMethod'){return $false}};$true
 }finally{$r1Handle.Dispose();$d.Dispose()}
}
Check 'ACTUAL_OLD_EVIDENCE_FINGERPRINT_PRESERVES_R1_DETECTS_CHANGE' {
 $t=$null;$e=$null;$ast=[Management.Automation.Language.Parser]::ParseFile((Join-Path $PSScriptRoot 'install-r2-only.ps1'),[ref]$t,[ref]$e)
 foreach($name in @('Get-R2InstallSha256','Get-R2OldEvidenceFingerprint')){$fn=$ast.Find({param($n)$n -is [Management.Automation.Language.FunctionDefinitionAst] -and $n.Name -ceq $name},$true);. ([scriptblock]::Create($fn.Extent.Text))}
 $fs=[V2FakeStageFs]::new([byte[]]@(1));$root=$m.approval_root;$fs.Add($root,$true,$null);$oldPath=$root+'\r1-preserved.json';$fs.Add($oldPath,$false,[byte[]]@(1,2,3));$fs.Add(($root+'\r1-empty.lease'),$false,[byte[]]::new(0));$newFile=$root+'\'+$m.approval_id+'.json';$newDir=$root+'\'+$m.approval_id;$journal=$root+'\Entry-'+$m.package_id+'.json'
 $before=Get-R2OldEvidenceFingerprint $fs $root $newFile $newDir $journal;$fs.Add($newFile,$false,[byte[]]@(4));$fs.Add($newDir,$true,$null)
 $after=Get-R2OldEvidenceFingerprint $fs $root $newFile $newDir $journal;$fs.Nodes[$oldPath].Bytes[0]=9;$changed=Get-R2OldEvidenceFingerprint $fs $root $newFile $newDir $journal
 $before -ceq $after -and $changed -cne $before
}

# 실제 packaged Task 메서드를 fake Scheduler에만 연결합니다. 계정/Task OS API를 실행하지 않습니다.
$dummy=[Security.SecureString]::new();$dummy.MakeReadOnly();$native=New-D3WindowsApi $m @{'Deployment/Preservation.json'=[IO.File]::ReadAllBytes($base+'\Deployment\Preservation.json')} $dummy $dummy
$script:fakeAction=[pscustomobject]@{Path='';WorkingDirectory='';Arguments=''}
$actions=[pscustomobject]@{Count=1};$actions|Add-Member ScriptMethod Create {param($kind)$script:fakeAction};$actions|Add-Member ScriptMethod Item {param($index)$script:fakeAction}
$settings=[pscustomobject]@{Enabled=$true;RestartCount=99;MultipleInstances=0;ExecutionTimeLimit='PT1H';AllowHardTerminate=$false}
$script:fakeDefinition=[pscustomobject]@{Principal=[pscustomobject]@{UserId='';LogonType=0;RunLevel=1};Settings=$settings;Actions=$actions;Triggers=[pscustomobject]@{Count=0}}
$script:fakeFolder=[pscustomobject]@{flags=0;creates=0;existing=$false;sd=''}
$fakeFolder|Add-Member ScriptMethod RegisterTaskDefinition {param($name,$definition,$flags,$sid,$password,$logon,$sddl)$this.flags=$flags;if($this.existing){throw 'FAKE_EXISTS'};$this.creates++;$this.sd=$sddl;$task=[pscustomobject]@{Enabled=$definition.Settings.Enabled;Definition=$definition;sd=$sddl};$task|Add-Member ScriptMethod GetSecurityDescriptor {param($bits)$this.sd};$task}
$script:fakeScheduler=[pscustomobject]@{};$fakeScheduler|Add-Member ScriptMethod Connect {};$fakeScheduler|Add-Member ScriptMethod NewTask {param($flags)$script:fakeDefinition};$fakeScheduler|Add-Member ScriptMethod GetFolder {param($path)$script:fakeFolder}
function New-Object {param([string]$ComObject)if($ComObject -cne 'Schedule.Service'){throw 'UNEXPECTED_FAKE_COM'};$script:fakeScheduler}
try{
 Check 'ACTUAL_PACKAGED_TASK_CREATE_ONLY_ATOMIC_SDDL_DISABLED' {$t=$native.Task($m,'S-1-5-21-1-2-3-2200',$pins.ms,('b'*64));$t.disabled -and $t.triggers -eq 0 -and $t.retries -eq 0 -and $t.seconds -eq 90 -and $t.instances -eq 2 -and $t.action_pinned -and $t.logon_type -eq 1 -and $fakeFolder.flags -eq 50 -and ($fakeFolder.flags -band 4) -eq 0 -and $fakeFolder.creates -eq 1 -and $t.sddl -ceq [DedicatedContract]::TaskSddl('S-1-5-21-1-2-3-2200')}
 Check 'ACTUAL_PACKAGED_TASK_EXISTING_REJECT_NO_UPDATE' {$fakeFolder.existing=$true;$count=$fakeFolder.creates;try{$null=$native.Task($m,'S-1-5-21-1-2-3-2200',$pins.ms,('b'*64));$false}catch{$fakeFolder.creates -eq $count -and ($fakeFolder.flags -band 4) -eq 0}}
}finally{$dummy.Dispose()}
Check 'NEW_ENTRY_AND_CONTROL_PARSE' {foreach($file in @('install-r2-only.ps1','R2InstallControl.ps1')){$t=$null;$e=$null;$null=[Management.Automation.Language.Parser]::ParseFile((Join-Path $PSScriptRoot $file),[ref]$t,[ref]$e);if($e.Count){return $false}};$true}
$out=@{kind='R2_INSTALL_NEW_GATES_AND_ACTUAL_TASK_METHOD_FAKE';pass=@($rows|Where-Object status -eq PASS).Count;fail=@($rows|Where-Object status -eq FAIL).Count;tests=$rows;real_accounts=0;real_task_registrations=0;approval_writes=0;real_password_reads=0;actual_installs=0}
$out|ConvertTo-Json -Depth 10|Set-Content -LiteralPath $OutputPath -Encoding utf8NoBOM;$out|Select-Object pass,fail|ConvertTo-Json;if($out.fail){throw 'R2_INSTALL_TEST_HOLD'}

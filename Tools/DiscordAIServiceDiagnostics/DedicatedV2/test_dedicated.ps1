param([Parameter(Mandatory)][string]$Candidate,[Parameter(Mandatory)][string]$OutputPath)
$ErrorActionPreference='Stop';Set-StrictMode -Version Latest
$p=Get-Content ($Candidate+'.package.json') -Raw|ConvertFrom-Json -AsHashtable
$fake=[IO.File]::ReadAllText((Join-Path $PSScriptRoot '../V2/test_V2StagingFake.cs')) -replace '(?m)^using [A-Za-z0-9_.]+;',''
Add-Type -TypeDefinition (([IO.File]::ReadAllText((Join-Path $Candidate 'Code/V2/InstallAssembly.cs')))+$fake)
. (Join-Path $PSScriptRoot 'DedicatedCore.ps1')
$mb=[IO.File]::ReadAllBytes((Join-Path $Candidate 'HostManifest.json'));$m=[Text.Encoding]::UTF8.GetString($mb)|ConvertFrom-Json -AsHashtable
$payload=@{};foreach($n in $m.files_sha256.Keys){$payload[$n]=[IO.File]::ReadAllBytes((Join-Path $Candidate $n))}
$script:rows=@();$script:admin='S-1-5-21-1-2-3-1100';$script:bot='S-1-5-21-1-2-3-1200';$script:worker='S-1-5-21-1-2-3-1201'
function Check($Name,[scriptblock]$Body){try{if(-not (& $Body)){throw 'ASSERT'};$script:rows+=@{name=$Name;status='PASS'}}catch{$script:rows+=@{name=$Name;status='FAIL';type=$_.Exception.GetType().FullName}}}
function Approval { @{schema=3;kind='DEDICATED_INSTALL_ONCE';approved=$true;approval_id=$m.approval_id;epoch=$m.epoch;manifest_sha256=$p.manifest_sha256;installer_sha256=$p.installer_sha256;zip_sha256=$p.zip_sha256;admin_sid=$script:admin;expires_unix=1300;preserve_existing=$true;create_bot=$m.bot_account;create_worker=$m.worker_account;worker_install_state='ENABLED_BATCH_ONLY'} }
function Fake($fault=''){
 $a=[pscustomobject]@{fault=$fault;calls=[Collections.Generic.List[string]]::new();used=$false;enabled=$false;closed=$false}
 $a|Add-Member ScriptMethod Context { @{admin=($this.fault -ne 'ADMIN');admin_sid=$script:admin;now=1000;preexisting_sids=@([DedicatedContract]::OldWorker,[DedicatedContract]::OldBot,'S-1-5-21-1-2-3-1199')} }
 $a|Add-Member ScriptMethod Snapshot { @{known=($this.fault -ne 'UNKNOWN');preserved=$true;fingerprint=if($this.fault -eq 'CHANGED' -and $this.enabled){'changed'}else{'old11-task4-onboot2-V1-r11'}} }
 $a|Add-Member ScriptMethod Fresh {param($m)if($this.fault -in @('PATH','TASK','PIPE','ACCOUNT')){throw 'COLLISION'};$this.calls.Add('Fresh')}
 $a|Add-Member ScriptMethod Consume {param($m,$s)if($this.used){throw 'CONSUMED'};$this.used=$true;$this.calls.Add('Consume')}
 $a|Add-Member ScriptMethod InstallLease {param($m)if($this.fault -eq 'LEASE'){throw 'LOCKED'};[IO.MemoryStream]::new()}
 $a|Add-Member ScriptMethod CreateUser {param($n,$p,$r)$this.calls.Add('Create-'+$r);$s=if($r -eq 'bot'){$script:bot}else{$script:worker};if($r -eq 'worker'){switch($this.fault){'OLD_SID'{$s=[DedicatedContract]::OldWorker};'SAME_SID'{$s=$script:bot};'ADMIN_SID'{$s=$script:admin};'EXISTING_SID'{$s='S-1-5-21-1-2-3-1199'}}};@{sid=$s;name=$n;created_new=$true;disabled=$true;users_only=($this.fault -ne 'GROUPS') }}
 $a|Add-Member ScriptMethod Rights {param($b,$w,$a)$this.calls.Add('Rights');if($this.fault -eq 'RIGHTS'){throw 'RIGHTS'}}
 $a|Add-Member ScriptMethod Publish {param($m,$mb,$p,$b,$w)$this.calls.Add('Publish');if($this.fault -in @('ACL','AUTH','COPY')){throw 'PUBLISH_HOLD'}}
 $a|Add-Member ScriptMethod Binding {param($b,$bot,$worker)$this.calls.Add('Binding')}
 $a|Add-Member ScriptMethod EnableWorker {param($sid)$this.calls.Add('Enable');$this.enabled=$true}
 $a|Add-Member ScriptMethod Task {param($m,$w,$ms,$bs)$this.calls.Add('Task');if($this.fault -eq 'REGISTER'){throw 'REGISTER'};@{disabled=($this.fault -ne 'ENABLED_TASK');triggers=if($this.fault -eq 'TRIGGER'){1}else{0};retries=0;principal=$w;run_level=0;logon_type=1;seconds=90;instances=2;hard_terminate=$true;action_pinned=$true;sddl=if($this.fault -eq 'TASK_ACL'){[DedicatedContract]::TaskSddl($w)+'(A;;FA;;;BU)'}else{[DedicatedContract]::TaskSddl($w)}} }
 $a|Add-Member ScriptMethod Verify {param($m,$b,$p,$s)$this.calls.Add('Verify');if($this.fault -eq 'READBACK'){throw 'UNKNOWN'}}
 $a|Add-Member ScriptMethod Record {param($r)$this.calls.Add('Record')}
 $a
}
Check 'NEW_INSTALL_NEW_OS_SIDS_PRESERVES_OLD11_TASK4' {$a=Fake;$r=Invoke-D3Install $a $mb $payload ((Approval)|ConvertTo-Json -Compress) $p.zip_sha256;$r.status -like 'DEDICATED_INSTALLED*' -and $r.existing_preserved -and $r.created_accounts.Count -eq 2 -and $a.calls.IndexOf('Rights') -lt $a.calls.IndexOf('Enable') -and $a.calls.IndexOf('Publish') -lt $a.calls.IndexOf('Enable')}
foreach($fault in @('ADMIN','UNKNOWN','PATH','TASK','PIPE','ACCOUNT','LEASE','OLD_SID','SAME_SID','ADMIN_SID','EXISTING_SID','GROUPS','RIGHTS','ACL','AUTH','COPY','REGISTER','TASK_ACL','ENABLED_TASK','TRIGGER','READBACK','CHANGED')){Check ('INSTALL_HOLD_'+$fault) {$a=Fake $fault;$r=Invoke-D3Install $a $mb $payload ((Approval)|ConvertTo-Json -Compress) $p.zip_sha256;$r.status -eq 'HOLD' -and $r.automatic_retries -eq 0 -and $r.ipc_calls -eq 0}}
Check 'CONSUMED_RETRY_REJECTED' {$a=Fake;$raw=(Approval)|ConvertTo-Json -Compress;$null=Invoke-D3Install $a $mb $payload $raw $p.zip_sha256;$r=Invoke-D3Install $a $mb $payload $raw $p.zip_sha256;$r.status -eq 'HOLD' -and @($a.calls|Where-Object {$_ -eq 'Create-worker'}).Count -eq 1}
foreach($key in @('schema','epoch','admin_sid','manifest_sha256','installer_sha256','zip_sha256','create_worker','worker_install_state','preserve_existing')){Check ('APPROVAL_REJECT_'+$key) {$a=Fake;$v=Approval;$v[$key]=if($key -eq 'schema'){2}elseif($key -eq 'preserve_existing'){$false}else{'wrong'};$r=Invoke-D3Install $a $mb $payload ($v|ConvertTo-Json -Compress) $p.zip_sha256;$r.status -eq 'HOLD' -and $a.calls.Count -eq 0}}
Check 'BAD_PAYLOAD_SHA_BEFORE_MUTATION' {$a=Fake;$bad=$payload.Clone();$bad['Code/V2/v2_contract.py']=[byte[]]@(1);$r=Invoke-D3Install $a $mb $bad ((Approval)|ConvertTo-Json -Compress) $p.zip_sha256;$r.status -eq 'HOLD' -and $a.calls.Count -eq 0}
$b=@{bot_sid=$script:bot;worker_sid=$script:worker;admin_sid=$script:admin}
$facts=@{worker_sid=$script:worker;security_logon_type=4;session=0;admin=$false;elevated=$false;users_only=$true;rights_exact=$true;credential_scope_unshared=$true;launcher_control_verified=$true;process_generation_verified=$true;no_foreign_dedicated_processes=$true;no_other_dedicated_tasks=$true;codex_home=$m.codex_home;auth_empty=$true;other_auth_imported=$false}
Check 'DEDICATED_FAKE_RUNTIME_ADMISSION' {Assert-D3RuntimeAdmission $m $b $facts}
foreach($key in $facts.Keys){Check ('ADMISSION_REJECT_'+$key) {$f=$facts.Clone();$f[$key]=if($f[$key] -is [bool]){-not $f[$key]}elseif($f[$key] -is [int]){99}else{'wrong'};try{$null=Assert-D3RuntimeAdmission $m $b $f;$false}catch{$true}}}
Check 'PROTECTED_CODE_BOT_NO_WRITE_AUTH_NO_BOT_OLD' {$sd=[DedicatedContract]::FsSddl('code',$true,$script:worker,$script:bot);$auth=[DedicatedContract]::FsSddl('auth',$true,$script:worker,'');$sd.Contains('0x1200a9;;;'+$script:bot) -and -not $auth.Contains($script:bot) -and -not $auth.Contains([DedicatedContract]::OldWorker)}
Check 'LSA_UTF16_BYTE_LENGTH' {$n=[DedicatedAccountPolicy]::UnicodeLength('SeBatchLogonRight');$n[0] -eq 34 -and $n[1] -eq 36}
$zip=[IO.File]::ReadAllBytes($Candidate+'.zip')
Check 'NEW_STAGING_MEMORY_CREATE_VERIFY' {$fs=[V2FakeStageFs]::new($zip);$s=[LimitlessV2Staging]::Create($fs,[V2FakeStageFs]::Source,$p.manifest_sha256,$p.installer_sha256,$p.zip_sha256,$m.files_sha256.Count);try{$s.Recheck();$s.Bytes.Count -eq 767 -and $s.Bundle.StartsWith([DedicatedContract]::StagingRoot)}finally{$s.Dispose()}}
foreach($fault in @('EXISTS_DENIED','OPEN_DENIED','MUTATION_FAIL','CORRUPT_COPY','TARGET_HARDLINK','TARGET_ADS','TARGET_REPARSE','TARGET_OWNER','TARGET_GROUP','TARGET_WRITER','TARGET_UNPROTECTED','TARGET_PENDING','TARGET_FINALPATH','TARGET_NONNTFS','EXTRA_ENTRY','OUTSIDE_ENTRY','READ_DENIED')){Check ('STAGING_HOLD_'+$fault) {$fs=[V2FakeStageFs]::new($zip);$fs.Fault=$fault;try{$s=[LimitlessV2Staging]::Create($fs,[V2FakeStageFs]::Source,$p.manifest_sha256,$p.installer_sha256,$p.zip_sha256,$m.files_sha256.Count);$s.Dispose();$false}catch{$true}}}
Check 'PS_PARSE_ALL_SOURCE' {$all=@(Get-ChildItem -LiteralPath $PSScriptRoot -Filter *.ps1);foreach($f in $all){$e=$null;$t=$null;$null=[Management.Automation.Language.Parser]::ParseFile($f.FullName,[ref]$t,[ref]$e);if($e.Count){return $false}};$true}
$out=@{schema=3;kind='DEDICATED_FAKE_ADAPTER_TESTS';pass=@($rows|Where-Object status -eq PASS).Count;fail=@($rows|Where-Object status -eq FAIL).Count;tests=$rows;actual_windows_changes=0;real_ipc_calls=0;real_worker_calls=0;model_calls=0}
$out|ConvertTo-Json -Depth 10|Set-Content -LiteralPath $OutputPath -Encoding utf8NoBOM
$out|Select-Object pass,fail,actual_windows_changes|ConvertTo-Json
if($out.fail){throw 'OFFLINE_TEST_FAILURE'}

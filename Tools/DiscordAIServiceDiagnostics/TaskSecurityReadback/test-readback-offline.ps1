param()
$ErrorActionPreference='Stop';Set-StrictMode -Version Latest
$root='F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree'
$base=Join-Path $root 'Tools/DiscordAIServiceDiagnostics/TaskSecurityReadback'
Add-Type -Path (Join-Path $root 'Tools/DiscordAIServiceDiagnostics/TaskSecurityCandidate/TaskReadSecurity.cs')
$snapshot=[IO.File]::ReadAllBytes((Join-Path $root '문서/00_프로젝트/팀운영/Evidence/DedicatedV2R2Readonly20261027/Admin-33a67728035f4fe88db7f539e5b648dc.json'))
$proof=[Limitless.TaskSecurityCandidate.DedicatedTaskReadContract]::FromPinnedSnapshot($snapshot)
. (Join-Path $base 'ReadbackCore.ps1')
$script:rows=@()
function Check-Readback($Name,[scriptblock]$Test){try{if(-not (& $Test)){throw 'ASSERT'};$script:rows+=@{name=$Name;status='PASS'}}catch{$script:rows+=@{name=$Name;status='FAIL';type=$_.Exception.GetType().FullName;hresult=$_.Exception.HResult}}}
function New-FakeReadback([string]$Fault=''){
 $a=[pscustomobject]@{calls=[Collections.Generic.List[string]]::new();fault=$Fault;readCount=0;prepareCount=0}
 $a|Add-Member ScriptMethod Prepare {$this.calls.Add('Prepare');$this.prepareCount++;if($this.fault -eq 'DENIED'){throw [ComponentModel.Win32Exception]::new(5)};@{verified=($this.fault -ne 'SOURCE');fingerprint=if($this.fault -eq 'SOURCE_CHANGED' -and $this.prepareCount -gt 1){'CHANGED'}else{'FIXED'}}}
 $a|Add-Member ScriptMethod Connect {$this.calls.Add('Connect')}
 $a|Add-Member ScriptMethod Read {$this.calls.Add('Read');$this.readCount++;if($this.fault -eq 'GETTASK'){throw [Runtime.InteropServices.COMException]::new('SECRET',-2147024891)};$sd='O:BAG:SYD:PAI(A;;FA;;;SY)(A;;FA;;;BA)(A;;FR;;;S-1-5-21-2858504392-1958168942-1662906075-1010)';if($this.fault -eq 'OVERGRANT'){$sd=$sd.Replace(';;FR;',';;FA;')};$f=@{sddl=$sd;fingerprint=if($this.fault -eq 'TASK_CHANGED' -and $this.readCount -gt 1){'CHANGED'}else{'FIXED'}};foreach($k in @('path_match','principal_sid_match','disabled','trigger0','state_disabled','policy_settings_match','action_match','xml_baseline_match')){$f[$k]=($this.fault -cne $k)};$f}
 return $a
}
Check-Readback 'TARGETED_READONLY_ORDER_AND_EFFECTIVE_HOLD' {$a=New-FakeReadback;$r=Invoke-R2TaskSecurityReadback $a $proof;$r.descriptor_readback_status -eq 'PASS' -and $r.effective_worker_access -eq 'UNKNOWN_TOKEN_NOT_CHECKED' -and -not $r.runtime_admitted -and ($a.calls -join ',') -ceq 'Prepare,Connect,Read,Read,Prepare'}
foreach($fault in @('SOURCE','DENIED','GETTASK','OVERGRANT','SOURCE_CHANGED','TASK_CHANGED','path_match','principal_sid_match','disabled','trigger0','state_disabled','policy_settings_match','action_match','xml_baseline_match')){Check-Readback ('HOLD_'+$fault) {$a=New-FakeReadback $fault;$r=Invoke-R2TaskSecurityReadback $a $proof;$r.status -eq 'HOLD' -and $r.automatic_retries -eq 0 -and $r.install_calls -eq 0 -and $r.read_attempts -le 2}}
Check-Readback 'RAW_HRESULT_PRESERVED_NO_SECRET' {$a=New-FakeReadback GETTASK;$r=Invoke-R2TaskSecurityReadback $a $proof;@($r.errors|Where-Object hresult -EQ -2147024891).Count -eq 1 -and ($r|ConvertTo-Json -Depth 12) -notmatch 'SECRET'}
Check-Readback 'MISSING_EVIDENCE_HOLD' {(Invoke-R2TaskSecurityReadback (New-FakeReadback) $null).status -eq 'HOLD'}
$source=[IO.File]::ReadAllText((Join-Path $base 'probe-task-security-readonly.ps1'))
Check-Readback 'ENTRY_PARSE_NO_RESERVED_HOST_ASSIGNMENT' {$t=$null;$e=$null;[void][Management.Automation.Language.Parser]::ParseInput($source,[ref]$t,[ref]$e);$e.Count -eq 0 -and $source -notmatch '(?i)\$host\s*='}
# 실제 adapter 정의를 사용하되 Windows 연결 메서드는 실행하지 않습니다.
$begin=$source.IndexOf('$backend=[pscustomobject]');$end=$source.IndexOf('$result=Invoke-R2TaskSecurityReadback');$block=$source.Substring($begin,$end-$begin)
$old=[Text.Encoding]::UTF8.GetString($snapshot)|ConvertFrom-Json -AsHashtable
function Get-R2TaskSha([byte[]]$Bytes){[Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($Bytes)).ToLowerInvariant()}
. ([scriptblock]::Create($block))
Check-Readback 'ACTUAL_BACKEND_CONSTRUCTS_NO_CONNECT_NO_COLLISION' {$backend.scheduler -eq $null -and $backend.folder -eq $null -and @(@('File','Prepare','Connect','Read')|Where-Object {$backend.PSObject.Members[$_].MemberType -ne 'ScriptMethod'}).Count -eq 0}
Check-Readback 'NO_MUTATION_METHODS_IN_ENTRY' {$source -notmatch '\.(RegisterTaskDefinition|SetSecurityDescriptor|RunEx?|DeleteTask)\(' -and $source -notmatch '\b(New-LocalUser|Set-LocalUser|Set-Acl|Start-ScheduledTask)\b'}
Check-Readback 'ACTUAL_COM_READ_METHOD_FAKE_TASK_AND_SID_NO_OS' {
 $task=[pscustomobject]@{Path='\LIMITLESS-DedicatedV2-20261027-Host';Enabled=$false;State=1;Xml='FAKE_TASK_XML';Definition=[pscustomobject]@{Principal=[pscustomobject]@{UserId='S-1-5-21-2858504392-1958168942-1662906075-1010';GroupId=$null;LogonType=1;RunLevel=0};Triggers=[pscustomobject]@{Count=0};Settings=[pscustomobject]@{RestartCount=0;MultipleInstances=2;AllowHardTerminate=$true;ExecutionTimeLimit='PT90S'};Actions=[pscustomobject]@{Count=0}}}
 $task|Add-Member ScriptMethod GetSecurityDescriptor {param($flags)if($flags -ne 7){throw 'FLAGS'};'O:BAG:SYD:PAI(A;;FA;;;SY)(A;;FA;;;BA)(A;;FR;;;S-1-5-21-2858504392-1958168942-1662906075-1010)'}
 $fakeFolder=[pscustomobject]@{task=$task;paths=[Collections.Generic.List[string]]::new()};$fakeFolder|Add-Member ScriptMethod GetTask {param($path)$this.paths.Add($path);$this.task};$backend.folder=$fakeFolder
 $first=$backend.Read();$second=$backend.Read()
 $first.principal_sid_match -and $first.disabled -and $first.trigger0 -and -not $first.action_match -and -not $first.xml_baseline_match -and $first.fingerprint -ceq $second.fingerprint -and @($fakeFolder.paths|Where-Object {$_ -cne 'LIMITLESS-DedicatedV2-20261027-Host'}).Count -eq 0
}
Check-Readback 'ACTUAL_PREPARE_WITH_FAKE_FILES_AND_FAKE_LOCALUSER' {
 function Get-LocalUser {param($Name,$ErrorAction)if($Name -cne 'LIMITLESSWorkV2D'){throw 'SCOPE'};[pscustomobject]@{SID=[pscustomobject]@{Value='S-1-5-21-2858504392-1958168942-1662906075-1010'};Enabled=$true;AccountExpires=(Get-Date).AddHours(1)}}
 $prepare=[pscustomobject]@{code=$backend.code;worker_sid=$backend.worker_sid}
 $prepare|Add-Member ScriptMethod File {param($Path,$Sha)$b=@{package_id='3effaac30bf44760986fee9e6019b283';worker_sid=$this.worker_sid;bot_sid='S-1-5-21-2858504392-1958168942-1662906075-1009';manifest_sha256='7c8410145ed84f06fc9901e93f78e18d0cc103ad89757172e08c84f91d8a3976'};@{sha256=$Sha;sddl='FAKE_SD';bytes=[Text.Encoding]::UTF8.GetBytes(($b|ConvertTo-Json))}}
 $prepare|Add-Member ScriptMethod Prepare $backend.PSObject.Members['Prepare'].Script
 $value=$prepare.Prepare();$value.verified -and $value.worker_enabled -and $value.worker_sid -ceq $backend.worker_sid
}
@{kind='TARGETED_TASK_READBACK_NEW_INTEGRATION_ONLY';pass=@($script:rows|Where-Object status -EQ PASS).Count;fail=@($script:rows|Where-Object status -EQ FAIL).Count;tests=$script:rows;admin_queries=0;changes=0;existing_48_tests_repeated=$false;install_calls=0;ipc_calls=0;model_calls=0}|ConvertTo-Json -Depth 8

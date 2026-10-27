# 고정 검증기의 guard 상수만 기록합니다. 승인·소비·계정·Task·암호에는 쓰지 않습니다.
$ErrorActionPreference='Stop';Set-StrictMode -Version Latest
$phase='BOOTSTRAP';$session=$null;$rows=[Collections.Generic.List[object]]::new();$allow=[Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
$ctx=[pscustomobject]@{session=$null;console=$null;state=$null}
$ms='43f6afcc91eae196e015393670cd9cd4ff37194099b313c2b901177226cd32e3';$ins='83c97d781fb2d9aed797b41ea6bc799b83e939ad3ffa6f7af6a2d26f1080d68f';$zs='be58f8cdbe0befdcc64cd67287865c6cd6fc2611b4a706d26cda022a8014ca25'
$bundle='C:\Program Files\LimitlessRelayDedicatedV2Staging20261027\a5af3087b78b45d1baae46067d5b7ac9\Bundle'
$approval='C:\ProgramData\LimitlessRelayDedicatedV2Approvals\dedicated-install-a5af3087b78b45d1baae46067d5b7ac9.json'
function Get-R1GuardProbeSha256([byte[]]$b){[Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($b)).ToLowerInvariant()}
function Get-R1GuardProbeError($errorObject){
 $out=@();for($n=0;$errorObject -and $n -lt 8;$n++){
  $r=@{type=$errorObject.GetType().FullName;hresult=$errorObject.HResult}
  if($errorObject -is [ComponentModel.Win32Exception]){$r.win32=$errorObject.NativeErrorCode}
  # 원시 예외문은 출력하지 않습니다. 고정 SHA 소스에서 추출한 정확한 상수만 허용합니다.
  if($allow.Contains($errorObject.Message)){$r.guard=$errorObject.Message}
  $out+=$r;$errorObject=$errorObject.InnerException
 };$out
}
function Add-R1GuardProbeCheck([string]$tag,[scriptblock]$body){try{& $body;$rows.Add(@{name=$tag;status='PASS'})}catch{$rows.Add(@{name=$tag;status='HOLD';errors=@(Get-R1GuardProbeError $_.Exception)})}}
try{
 $me=[Security.Principal.WindowsIdentity]::GetCurrent()
 if(-not ([Security.Principal.WindowsPrincipal]::new($me)).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)){throw 'PROBE_ADMIN_REQUIRED'}
 if('DedicatedContract' -as [type]){throw 'PROBE_FRESH_PROCESS_REQUIRED'}
 $zb=[IO.File]::ReadAllBytes('F:\study\codex\Project-Limitless\Temp\Session0DedicatedV2Candidate20261027-r1.zip');if((Get-R1GuardProbeSha256 $zb) -cne $zs){throw 'PROBE_ZIP_PIN'}
 $memory=[IO.MemoryStream]::new($zb,$false);$zip=[IO.Compression.ZipArchive]::new($memory,[IO.Compression.ZipArchiveMode]::Read)
 try{
  function Read-R1GuardProbeEntry([string]$name){$entries=@($zip.Entries|Where-Object FullName -CEQ $name);if($entries.Count -ne 1 -or $entries[0].Length -gt 64MB){throw 'PROBE_ENTRY'};$s=$entries[0].Open();$o=[IO.MemoryStream]::new();try{$s.CopyTo($o);return ,$o.ToArray()}finally{$s.Dispose();$o.Dispose()}}
  $mb=Read-R1GuardProbeEntry 'HostManifest.json';if((Get-R1GuardProbeSha256 $mb) -cne $ms){throw 'PROBE_MANIFEST_PIN'};$m=[Text.Encoding]::UTF8.GetString($mb)|ConvertFrom-Json -AsHashtable
  $assembly=Read-R1GuardProbeEntry 'Code/V2/InstallAssembly.cs';if((Get-R1GuardProbeSha256 $assembly) -cne $m.files_sha256['Code/V2/InstallAssembly.cs']){throw 'PROBE_ASSEMBLY_PIN'}
 }finally{$zip.Dispose();$memory.Dispose()}
 $cs=[Text.Encoding]::UTF8.GetString($assembly)
 foreach($match in [regex]::Matches($cs,'"((?:STAGE_|D3_|V2_)[A-Z0-9_]+)"')){$null=$allow.Add($match.Groups[1].Value)}
 Add-Type -TypeDefinition $cs
 $phase='READONLY_STAGE_VERIFY'
 Add-R1GuardProbeCheck 'NATIVE_STAGE_VERIFY' {$ctx.session=[LimitlessV2Staging]::Verify([V2NativeStageFs]::new(),$bundle,$ms,$ins,$zs,766)}
 Add-R1GuardProbeCheck 'CURRENT_APPROVAL_FILE_PROTECTION' {[LimitlessV2Publish]::Verify($approval,[DedicatedContract]::FsSddl('private',$false,'',''))}
 if($ctx.session){
  Add-R1GuardProbeCheck 'SEALED_CORE_WINDOWS_DEFINITION_ONLY' {
   foreach($name in @('Code/V2/DedicatedCore.ps1','Code/V2/DedicatedWindows.ps1')){. ([scriptblock]::Create([Text.Encoding]::UTF8.GetString($ctx.session.Bytes[$name])))}
   # 함수는 정의만 로드하고 New-D3WindowsApi/Invoke-D3Install은 호출하지 않습니다.
  }
  Add-R1GuardProbeCheck 'FINAL_STAGE_METADATA_RECHECK' {$ctx.session.Recheck()}
 }
 $phase='CONSOLE_METADATA_NO_PASSWORD'
 $ctx.console=@{input_redirected=[Console]::IsInputRedirected;output_redirected=[Console]::IsOutputRedirected;error_redirected=[Console]::IsErrorRedirected;console_host=($Host.Name -ceq 'ConsoleHost');raw_ui_present=($null -ne $Host.UI.RawUI);secure_input_not_executed=$true}
 $phase='READ_ONLY_RECOVERY_SNAPSHOT'
 Add-R1GuardProbeCheck 'CONSUMPTION_ACCOUNT_TASK_STATE_READONLY' {
  $reader=[IO.File]::ReadAllBytes('F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\Tools\DiscordAIServiceDiagnostics\R1Preparation\probe-install-hold-readonly.ps1')
  if((Get-R1GuardProbeSha256 $reader) -cne 'c34be6d7bd023c3575b2cf16a2b53f4a06c3225209fcfff42edf8916ffad5874'){throw 'PROBE_READER_PIN'}
  $stateJson=& ([scriptblock]::Create([Text.Encoding]::UTF8.GetString($reader)))
  $ctx.state=$stateJson|ConvertFrom-Json -AsHashtable
 }
 $phase='COMPLETE_READONLY'
}catch{$rows.Add(@{name=$phase;status='HOLD';errors=@(Get-R1GuardProbeError $_.Exception)})}
finally{if($ctx.session){try{$ctx.session.Dispose()}catch{$rows.Add(@{name='STAGE_CLOSE';status='HOLD';errors=@(Get-R1GuardProbeError $_.Exception)})}}}
@{schema=3;kind='R1_NATIVE_GUARD_READONLY';status='HOLD_DIAGNOSTIC_ONLY_NO_RETRY';guards_status=if(@($rows|Where-Object status -eq HOLD).Count){'HOLD'}else{'READONLY_GUARDS_PASS_EXACT_INSTALLER_THROW_STILL_UNVERIFIED'};checks=$rows;console=$ctx.console;state_snapshot=$ctx.state;phase=$phase;password_reads=0;approval_changes=0;consume_changes=0;install_calls=0;ipc_calls=0;model_calls=0;automatic_retries=0}|ConvertTo-Json -Depth 12

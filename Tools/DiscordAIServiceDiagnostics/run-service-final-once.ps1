param([switch]$Child,[ValidatePattern('^[a-f0-9]{32}$')][string]$RequestId,
 [ValidatePattern('^[a-f0-9]{64}$')][string]$ScriptSha,[ValidatePattern('^[a-f0-9]{64}$')][string]$RunnerSha)
# 대표의 새 단발 추론 승인 전용입니다. 기존 실패/소비 슬롯과 부팅 프로그램은 변경하지 않습니다.
$ErrorActionPreference='Stop'
$repo=Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
$tool=Join-Path $repo 'Tools/DiscordAICollab'
$runner=Join-Path $PSScriptRoot 'service-final-once.py'
$out=Join-Path $tool 'data/service-final-once-20261024'
$approvalPath='C:/ProgramData/LimitlessRelay/boot-approval.json'
$expectedCli=Join-Path $tool 'data/runtime/codex/official-0.164.0-alpha.11.1-x64/bin/codex.exe'
$expectedSha='4fd2f057bad603bf53dc202457ffd0c3591de62e867a8b01e73816e3fbce5a73'
function Sha([string]$p){(Get-FileHash -LiteralPath $p -Algorithm SHA256).Hash.ToLower()}
function Verify-Runtime {
 if((Sha $approvalPath) -ne '3e032ebcce3d91bd762a8dec81048d04607ddde761afd62489c0fa2b8e49140f'){throw 'OPERATING_APPROVAL_SHA_HOLD'}
 $a=Get-Content -LiteralPath $approvalPath -Raw|ConvertFrom-Json
 if($a.tool_root -ne $tool -or $a.service_codex.path -ine $expectedCli -or $a.service_codex.sha256 -ne $expectedSha -or (Sha $expectedCli) -ne $expectedSha){throw 'OPERATING_CLI_REFERENCE_HOLD'}
 foreach($p in $a.runtime_sha256.PSObject.Properties){if((Sha (Join-Path $tool $p.Name)) -ne $p.Value){throw 'EXISTING_RUNTIME_SHA_HOLD'}}
 if(@(Get-ChildItem -LiteralPath (Join-Path $tool 'collab') -File -Filter '*.py').Count -ne 42){throw 'EXISTING_42_REQUIRED'}
 $manifestPath=Join-Path $repo '문서/00_프로젝트/Evidence/ServiceCLI_OfficialPackage_20261024.json'
 if((Sha $manifestPath) -ne '04cecc241628c7a17a46ef9fc3befbe32e287792f938ecc4f854f20a40eb9833'){throw 'PACKAGE_MANIFEST_HOLD'}
 $manifest=Get-Content -LiteralPath $manifestPath -Raw|ConvertFrom-Json
 $package=Split-Path (Split-Path $expectedCli -Parent) -Parent
 if($manifest.files.Count -ne 45 -or @(Get-ChildItem -LiteralPath $package -Recurse -File).Count -ne 45){throw 'PACKAGE_COUNT_HOLD'}
 foreach($f in $manifest.files){if((Sha (Join-Path $package $f.path)) -ne $f.sha256){throw 'PACKAGE_SHA_HOLD'}}
 foreach($rel in @('bin/codex.exe','bin/codex-code-mode-host.exe','codex-resources/codex-command-runner.exe','codex-resources/codex-windows-sandbox-setup.exe')){
  $s=Get-AuthenticodeSignature -LiteralPath (Join-Path $package $rel)
  if($s.Status -ne 'Valid' -or $s.SignerCertificate.Subject -notmatch 'OpenAI OpCo'){throw 'SIGNATURE_HOLD'}
 }
 return $a
}
function Boot-Snapshot {
 $s=@{}
 foreach($name in @('LIMITLESS-Bot-OnBoot','LIMITLESS-Art-OnBoot')){
  $xml=Export-ScheduledTask -TaskName $name -TaskPath '\'
  $s[$name]=[Convert]::ToHexString([Security.Cryptography.SHA256]::HashData([Text.Encoding]::Unicode.GetBytes($xml)))
 }
 return $s
}
if($Child){
 $saved=$null;$pointer=[IntPtr]::Zero
 try {
  if(-not $RequestId -or (Sha $PSCommandPath) -ne $ScriptSha -or (Sha $runner) -ne $RunnerSha){throw 'TRIAL_ENTRYPOINT_SHA_HOLD'}
  $a=Verify-Runtime;$id=[Security.Principal.WindowsIdentity]::GetCurrent()
  if($id.User.Value -ne $a.service_sid -or $id.Name -ne 'PAUL-PC\LIMITLESSRelaySvc'){throw 'SERVICE_CONTEXT_HOLD'}
  Import-Module (Join-Path $tool 'BatchReadOnlyMetadata.psm1') -Force
  $logon=[LimitlessBatchMetadataV1]::CurrentLogon()
  if($logon[0] -ne 4 -or $logon[1] -ne 0 -or [Environment]::UserInteractive){throw 'SESSION_ZERO_BATCH_HOLD'}
  Import-Module (Join-Path $tool 'DiscordSecretStore.psm1') -Force
  $saved=Read-LimitlessDiscordCredential (Join-Path $tool 'data/private/boot-discord')
  $pointer=[Runtime.InteropServices.Marshal]::SecureStringToBSTR($saved.Token)
  $env:DISCORD_BOT_TOKEN=[Runtime.InteropServices.Marshal]::PtrToStringBSTR($pointer)
  $env:LIMITLESS_OWNER_IDS=$saved.Owners;$env:LIMITLESS_GUILD_ID=$saved.Guild;$env:LIMITLESS_CHANNEL_IDS=$saved.Channels
  $env:LIMITLESS_BATCH_REQUEST_ID=$RequestId;$env:LIMITLESS_BATCH_SESSION_ID='0';$env:LIMITLESS_BATCH_LOGON_TYPE='4'
  $env:PYTHONIOENCODING='utf-8';$env:PYTHONDONTWRITEBYTECODE='1'
  @{request_id=$RequestId;account=$id.Name;session_id=0;logon_type=4;actual_ai_calls=0}|ConvertTo-Json|Set-Content (Join-Path $out 'batch-context.json') -Encoding utf8
  Set-Location -LiteralPath $tool
  & (Join-Path $tool '.venv/Scripts/python.exe') $runner | Out-Null
  exit $LASTEXITCODE
 } catch {
  @{status='NOT_VERIFIED';request_id=$RequestId;reason='SERVICE_PRELAUNCH_HOLD_NO_RAW_ERROR';actual_ai_calls=0;error_type=$_.Exception.GetType().Name}|ConvertTo-Json|Set-Content (Join-Path $out 'prelaunch-hold.json') -Encoding utf8
  exit 2
 } finally {
  if($pointer -ne [IntPtr]::Zero){[Runtime.InteropServices.Marshal]::ZeroFreeBSTR($pointer)}
  if($saved){$saved.Token.Dispose()};$env:DISCORD_BOT_TOKEN=$null
 }
}
$identity=[Security.Principal.WindowsIdentity]::GetCurrent()
if(-not ([Security.Principal.WindowsPrincipal]::new($identity)).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)){
 Write-Output 'HOLD_ADMIN_LOCAL_SERVICE_CREDENTIAL_REQUIRED_NO_AI_CALL';exit 2
}
if((Test-Path (Join-Path $out 'attempt-consumed')) -or (Test-Path (Join-Path $out 'launch-consumed'))){
 Write-Output 'HOLD_APPROVED_TRIAL_ALREADY_CONSUMED_NO_RETRY';exit 2
}
$nonce=[guid]::NewGuid().ToString('N');$name='LIMITLESS-ServiceFinalAI-'+$nonce;$marker='SERVICE_FINAL_ONE_APPROVED_'+$nonce
$r=@{status='NOT_VERIFIED';request_id=$nonce;actual_ai_calls=0;automatic_retries=0;temporary_task_removed=$false;boot_tasks_unchanged=$false}
$registered=$false;$credential=$null;$plain=$null
try {
 $null=Verify-Runtime;$before=Boot-Snapshot;$approvalBefore=Sha $approvalPath
 New-Item -ItemType Directory -Path $out -Force|Out-Null
 # 단발 출력 폴더만 서비스가 수정할 수 있게 하며 기존 승인/게임/프로필 ACL은 유지합니다.
 $acl=[Security.AccessControl.DirectorySecurity]::new();$acl.SetAccessRuleProtection($true,$false);$acl.SetOwner($identity.User)
 foreach($sid in @($identity.User.Value,'S-1-5-18','S-1-5-32-544')){$acl.AddAccessRule([Security.AccessControl.FileSystemAccessRule]::new([Security.Principal.SecurityIdentifier]::new($sid),'FullControl','ContainerInherit,ObjectInherit','None','Allow'))}
 $acl.AddAccessRule([Security.AccessControl.FileSystemAccessRule]::new([Security.Principal.SecurityIdentifier]::new('S-1-5-21-2858504392-1958168942-1662906075-1007'),'Modify','ContainerInherit,ObjectInherit','None','Allow'));Set-Acl -LiteralPath $out -AclObject $acl
 $account=$env:COMPUTERNAME+'\LIMITLESSRelaySvc'
 $credential=Get-Credential -UserName $account -Message '승인된 실제 AI 시험 최대1회/90초입니다. 서비스 Windows 암호를 로컬 보안 입력에만 제공하세요.'
 if(-not $credential -or $credential.UserName -ine $account){throw 'LOCAL_SERVICE_CREDENTIAL_REQUIRED'}
 $arguments='-NoProfile -NonInteractive -WindowStyle Hidden -File "'+$PSCommandPath+'" -Child -RequestId '+$nonce+' -ScriptSha '+(Sha $PSCommandPath)+' -RunnerSha '+(Sha $runner)
 $a=New-ScheduledTaskAction -Execute (Get-Command pwsh).Source -Argument $arguments -WorkingDirectory $tool
 $settings=New-ScheduledTaskSettingsSet -ExecutionTimeLimit (New-TimeSpan -Minutes 6) -MultipleInstances IgnoreNew -DisallowHardTerminate
 $plain=$credential.GetNetworkCredential().Password
 Register-ScheduledTask -TaskName $name -TaskPath '\' -Description $marker -Action $a -Settings $settings -User $account -Password $plain -RunLevel Limited|Out-Null
 $registered=$true;$plain=$null
 $t=Get-ScheduledTask -TaskName $name -TaskPath '\'
 if($t.Principal.LogonType.ToString() -ne 'Password' -or $t.Principal.RunLevel.ToString() -ne 'Limited'){throw 'TASK_LOGON_POLICY_HOLD'}
 $stream=[IO.File]::Open((Join-Path $out 'launch-consumed'),[IO.FileMode]::CreateNew);$stream.Dispose()
 Start-ScheduledTask -TaskName $name -TaskPath '\'
 for($i=0;$i -lt 78;$i++){
  Start-Sleep -Seconds 5;$t=Get-ScheduledTask -TaskName $name -TaskPath '\';$info=Get-ScheduledTaskInfo -TaskName $name -TaskPath '\'
  if($t.State.ToString() -ne 'Running' -and $info.LastRunTime.Year -gt 2000){break}
 }
 $r.task_exit_code=$info.LastTaskResult
 if(Test-Path (Join-Path $out 'result.json')){
  $r.result=Get-Content (Join-Path $out 'result.json') -Raw|ConvertFrom-Json
  if($r.result.request_id -ne $nonce){throw 'RESULT_REQUEST_ID_HOLD'}
  $r.actual_ai_calls=$r.result.actual_ai_calls
  if($r.result.status -eq 'VERIFIED' -and $r.result.discord_status -eq 'MESSAGE_SENT' -and $info.LastTaskResult -eq 0){$r.status='VERIFIED'}
 }
 if(Test-Path (Join-Path $out 'prelaunch-hold.json')){$r.prelaunch=Get-Content (Join-Path $out 'prelaunch-hold.json') -Raw|ConvertFrom-Json}
} catch {$r.reason='SERVICE_FINAL_BATCH_HOLD_NO_RETRY';$r.error_type=$_.Exception.GetType().Name}
finally {
 $plain=$null;if($credential){$credential.Password.Dispose()};$credential=$null
 try {
  if($registered){
   $t=Get-ScheduledTask -TaskName $name -TaskPath '\'
   if($t.Description -ne $marker -or -not $t.Actions[0].Arguments.Contains($nonce)){throw 'CLEANUP_IDENTITY_HOLD'}
   if($t.State.ToString() -ne 'Running'){Unregister-ScheduledTask -TaskName $name -TaskPath '\' -Confirm:$false;$r.temporary_task_removed=$true}
  }
  $after=Boot-Snapshot;$same=$true;foreach($n in $before.Keys){if($before[$n] -ne $after[$n]){$same=$false}}
  $r.boot_tasks_unchanged=$same;$r.approval_unchanged=((Sha $approvalPath) -eq $approvalBefore)
 } catch {$r.protection_check='NOT_VERIFIED'}
 if(-not $r.temporary_task_removed -or -not $r.boot_tasks_unchanged -or -not $r.approval_unchanged){$r.status='NOT_VERIFIED'}
 if(Test-Path -LiteralPath $out){$r|ConvertTo-Json -Depth 15|Set-Content (Join-Path $out 'batch-summary.json') -Encoding utf8}
}
$r|ConvertTo-Json -Depth 15
if($r.status -eq 'VERIFIED'){exit 0}else{exit 2}

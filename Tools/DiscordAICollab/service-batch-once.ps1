param([switch]$Child,[ValidatePattern('^[a-f0-9]{32}$')][string]$RequestId)
# 대표의 2026-10-24 승인 전용입니다. 기존 부팅 작업·모드·Codex 인증은 수정하지 않습니다.
$ErrorActionPreference='Stop'
$root=$PSScriptRoot
$out=Join-Path $root 'data/service-batch-once-20261024'
if($Child){
 $saved=$null;$pointer=[IntPtr]::Zero
 try{
  $approval=Get-Content 'C:/ProgramData/LimitlessRelay/boot-approval.json' -Raw|ConvertFrom-Json
  $identity=[Security.Principal.WindowsIdentity]::GetCurrent()
  if($identity.User.Value -ne $approval.service_sid -or $root -ne $approval.tool_root){throw 'SERVICE_CONTEXT_REQUIRED'}
  Import-Module (Join-Path $root 'BatchReadOnlyMetadata.psm1') -Force
  $logon=[LimitlessBatchMetadataV1]::CurrentLogon()
  if($logon[0] -ne 4 -or $logon[1] -ne 0 -or [Environment]::UserInteractive){throw 'BATCH_SESSION_ZERO_REQUIRED'}
  New-Item -ItemType Directory -Force -Path $out|Out-Null
  @{request_id=$RequestId;account=$identity.Name;logon_type=$logon[0];session_id=$logon[1];user_interactive=[Environment]::UserInteractive}|ConvertTo-Json|Set-Content (Join-Path $out 'batch-context.json') -Encoding utf8
  Import-Module (Join-Path $root 'DiscordSecretStore.psm1') -Force
  # 서비스 계정 자신의 DPAPI 저장소만 엽니다. 토큰은 출력하거나 인수로 전달하지 않습니다.
  $saved=Read-LimitlessDiscordCredential (Join-Path $root 'data/private/boot-discord')
  $pointer=[Runtime.InteropServices.Marshal]::SecureStringToBSTR($saved.Token)
  $env:DISCORD_BOT_TOKEN=[Runtime.InteropServices.Marshal]::PtrToStringBSTR($pointer)
  $env:LIMITLESS_OWNER_IDS=$saved.Owners;$env:LIMITLESS_GUILD_ID=$saved.Guild;$env:LIMITLESS_CHANNEL_IDS=$saved.Channels
  $env:PYTHONIOENCODING='utf-8'
  $env:LIMITLESS_BATCH_REQUEST_ID=$RequestId
  $env:LIMITLESS_BATCH_SESSION_ID=[string]$logon[1]
  $env:LIMITLESS_BATCH_LOGON_TYPE=[string]$logon[0]
  Set-Location -LiteralPath $root
  & (Join-Path $root '.venv/Scripts/python.exe') -m collab.service_batch_once | Out-Null
  $code=$LASTEXITCODE
  exit $code
 }catch{
  # 예외 메시지·바인딩 값·토큰을 내보내지 않습니다.
  Write-Output 'SERVICE_BATCH_PRELAUNCH_HOLD_NO_RETRY';exit 2
 }finally{
  if($pointer -ne [IntPtr]::Zero){[Runtime.InteropServices.Marshal]::ZeroFreeBSTR($pointer)}
  if($saved){$saved.Token.Dispose()}
  $env:DISCORD_BOT_TOKEN=$null
 }
}
$admin=([Security.Principal.WindowsPrincipal]::new([Security.Principal.WindowsIdentity]::GetCurrent())).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
if(-not $admin){Write-Output 'HOLD_ADMIN_LOCAL_EXECUTION_REQUIRED';exit 2}
if(Test-Path (Join-Path $out 'attempt-consumed')){Write-Output 'HOLD_ATTEMPT_ALREADY_CONSUMED_NO_RETRY';exit 2}
function BootHashes{
 $snapshot=@{}
 foreach($taskName in @('LIMITLESS-Bot-OnBoot','LIMITLESS-Art-OnBoot')){
  $xml=Export-ScheduledTask -TaskName $taskName -TaskPath '\'
  $snapshot[$taskName]=[Convert]::ToHexString([Security.Cryptography.SHA256]::HashData([Text.Encoding]::Unicode.GetBytes($xml)))
 }
 return ($snapshot|ConvertTo-Json -Compress)
}
$before=BootHashes
$nonce=[guid]::NewGuid().ToString('N');$name='LIMITLESS-ServiceAI-Once-'+$nonce
$account=$env:COMPUTERNAME+'\LIMITLESSRelaySvc'
$registered=$false;$credential=$null
$summary=@{status='NOT_VERIFIED';request_id=$nonce;temporary_task_removed=$false;boot_tasks_unchanged=$false;mode_changed=$false}
try{
 New-Item -ItemType Directory -Force -Path $out|Out-Null
 $credential=Get-Credential -UserName $account -Message '승인된 서비스 배치 AI 1회 전용입니다. Windows 암호를 이 로컬 입력창에만 입력하세요.'
 if(-not $credential -or $credential.UserName -ine $account){throw 'SERVICE_CREDENTIAL_REQUIRED'}
 $arguments='-NoProfile -NonInteractive -WindowStyle Hidden -File "'+$PSCommandPath+'" -Child -RequestId '+$nonce
 $action=New-ScheduledTaskAction -Execute (Get-Command pwsh).Source -Argument $arguments -WorkingDirectory $root
 $settings=New-ScheduledTaskSettingsSet -ExecutionTimeLimit (New-TimeSpan -Minutes 5) -MultipleInstances IgnoreNew -DisallowHardTerminate
 Register-ScheduledTask -TaskName $name -Action $action -Settings $settings -User $account -Password $credential.GetNetworkCredential().Password -RunLevel Limited|Out-Null
 $registered=$true;$credential=$null
 Start-ScheduledTask -TaskName $name
 # 단발 작업만 기다립니다. 기존 Bot/Art/부팅 작업은 시작·중지·재등록하지 않습니다.
 for($i=0;$i -lt 52;$i++){
  Start-Sleep -Seconds 5
  $task=Get-ScheduledTask -TaskName $name
  $info=Get-ScheduledTaskInfo -TaskName $name
  if($task.State.ToString() -ne 'Running' -and $info.LastRunTime.Year -gt 2000){break}
 }
 $summary.task_exit_code=$info.LastTaskResult
 if(Test-Path (Join-Path $out 'result.json')){
  $summary.result=Get-Content (Join-Path $out 'result.json') -Raw|ConvertFrom-Json
  $summary.status=if($summary.result.status -eq 'VERIFIED' -and $summary.result.discord_status -eq 'MESSAGE_SENT' -and $info.LastTaskResult -eq 0){'VERIFIED'}else{'NOT_VERIFIED'}
 }
 if(Test-Path (Join-Path $out 'batch-context.json')){$summary.batch=Get-Content (Join-Path $out 'batch-context.json') -Raw|ConvertFrom-Json}
}catch{
 $summary.reason='BATCH_REGISTRATION_OR_EXECUTION_HOLD_NO_RETRY'
 $summary.error_type=$_.Exception.GetType().Name
}finally{
 $credential=$null
 if($registered){
  $task=Get-ScheduledTask -TaskName $name
  if($task.State.ToString() -ne 'Running'){Unregister-ScheduledTask -TaskName $name -Confirm:$false;$summary.temporary_task_removed=$true}
 }
 $summary.boot_tasks_unchanged=($before -ceq (BootHashes))
 if(-not $summary.boot_tasks_unchanged -or -not $summary.temporary_task_removed){$summary.status='NOT_VERIFIED'}
 $summary|ConvertTo-Json -Depth 12|Set-Content (Join-Path $out 'batch-summary.json') -Encoding utf8
 $summary|ConvertTo-Json -Depth 12
}

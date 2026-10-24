param([ValidateSet('ProbeHere','RunOnceAsService')][string]$Action='ProbeHere',[string]$RequestId='local')
# 기존 OnBoot 작업/봇/아트/자격 증명은 변경하지 않는 단발 읽기 점검입니다.
$ErrorActionPreference='Stop'
$root=$PSScriptRoot
if($RequestId -ne 'local' -and $RequestId -notmatch '^[a-f0-9]{32}$') {throw 'INVALID_REQUEST_ID'}
if($Action -eq 'ProbeHere') {
 Set-Location -LiteralPath $root
 & (Join-Path $root '.venv/Scripts/python.exe') -m collab.service_ai --request-id $RequestId
 exit $LASTEXITCODE
}
# 별도 계정 실행은 Windows의 정상 배치 로그온을 사용합니다. 토큰 복제·다른 계정 Codex 비밀 복사는 없습니다.
if(-not ([Security.Principal.WindowsPrincipal]::new([Security.Principal.WindowsIdentity]::GetCurrent())).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)) {Write-Output 'HOLD_ADMIN_LOCAL_EXECUTION_REQUIRED';exit 2}
$name='LIMITLESS-AI-Readiness-Once'
if(Get-ScheduledTask -TaskName $name -ErrorAction SilentlyContinue) {Write-Output 'HOLD_EXISTING_READINESS_TASK_REVIEW_REQUIRED';exit 2}
$nonce=[guid]::NewGuid().ToString('N')
$account=$env:COMPUTERNAME+'\LIMITLESSRelaySvc'
$credential=Get-Credential -UserName $account -Message 'LIMITLESSRelaySvc 단발 읽기 점검용 Windows 암호를 이 로컬 창에만 입력하세요. Codex/Discord 비밀을 입력하지 마세요.'
if($null -eq $credential -or $credential.UserName -ne $account) {Write-Output 'HOLD_SERVICE_CREDENTIAL_REQUIRED';exit 2}
$registered=$false
try {
 $arguments='-NoProfile -WindowStyle Hidden -File "'+$PSCommandPath+'" -Action ProbeHere -RequestId '+$nonce
 $actionSpec=New-ScheduledTaskAction -Execute (Get-Command pwsh).Source -Argument $arguments -WorkingDirectory $root
 $settings=New-ScheduledTaskSettingsSet -ExecutionTimeLimit (New-TimeSpan -Minutes 2) -MultipleInstances IgnoreNew -DisallowHardTerminate
 # Windows 등록 API에만 메모리 내 암호를 전달하며 파일/로그/명령줄로 저장하지 않습니다.
 Register-ScheduledTask -TaskName $name -Action $actionSpec -Settings $settings -User $account -Password $credential.GetNetworkCredential().Password -RunLevel Limited | Out-Null
 $registered=$true
 Start-ScheduledTask -TaskName $name
 $resultPath=Join-Path $root ('data/boot/service-ai-probe-'+$nonce+'.json')
 for($attempt=0;$attempt -lt 11;$attempt++) {
  Start-Sleep -Seconds 5
  if(Test-Path -LiteralPath $resultPath) {
   $result=Get-Content -LiteralPath $resultPath -Raw | ConvertFrom-Json
   if($result.request_id -ne $nonce) {throw 'READINESS_REQUEST_MISMATCH'}
   $result | ConvertTo-Json -Depth 6
   break
  }
 }
 if(-not (Test-Path -LiteralPath $resultPath)) {Write-Output 'HOLD_SERVICE_PROBE_RESULT_NOT_VERIFIED'}
} catch {Write-Output 'HOLD_SERVICE_LOGON_OR_READINESS_NOT_VERIFIED / NO_SECRET_DETAILS'}
finally {
 $credential=$null
 if($registered) {
  $task=Get-ScheduledTask -TaskName $name -ErrorAction SilentlyContinue
  if($null -ne $task -and $task.State.ToString() -ne 'Running') {Unregister-ScheduledTask -TaskName $name -Confirm:$false}
  else {Write-Output '단발 점검 작업이 아직 실행 중입니다. 강제 종료하지 않았으며 완료 후 이 점검 작업만 정리하세요.'}
 }
}

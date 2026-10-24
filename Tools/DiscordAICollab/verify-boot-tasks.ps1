param([switch]$StartTest)
$ErrorActionPreference='Stop'
$root=$PSScriptRoot
if(-not ([Security.Principal.WindowsPrincipal]::new([Security.Principal.WindowsIdentity]::GetCurrent())).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)) {Write-Output 'ADMIN_POWERSHELL_REQUIRED';exit 2}
Set-Location $root
$result=@{registered_count=0;tasks=@();actual_prelogin_boot='NOT_VERIFIED';mode='MANUAL_REQUIRED';ai='HOLD';start_test='NOT_RUN';at=[datetime]::UtcNow.ToString('o')}
try {
 $account=Get-LocalUser -Name LIMITLESSRelaySvc
 $result.account_enabled=$account.Enabled
 $result.account_is_admin=@(Get-LocalGroupMember -Group (Get-LocalGroup -SID 'S-1-5-32-544') | Where-Object {$_.SID.Value -eq $account.SID.Value}).Count -gt 0
 if(-not $account.Enabled -or $result.account_is_admin) {throw 'ACCOUNT_POLICY_MISMATCH'}
 foreach($role in @('Bot','Art')) {
  $name='LIMITLESS-'+$role+'-OnBoot'
  $task=Get-ScheduledTask -TaskName $name
  [xml]$xml=Export-ScheduledTask -TaskName $name
  $ns=[xml.xmlnamespacemanager]::new($xml.NameTable);$ns.AddNamespace('t','http://schemas.microsoft.com/windows/2004/02/mit/task')
  $info=Get-ScheduledTaskInfo -TaskName $name
  $result.tasks+=@{name=$name;state=$task.State.ToString();user=$task.Principal.UserId;logon=$task.Principal.LogonType.ToString();run_level=$task.Principal.RunLevel.ToString();boot_trigger=($null -ne $xml.SelectSingleNode('//t:BootTrigger',$ns));enabled=$task.Settings.Enabled;execute=$task.Actions[0].Execute;arguments=$task.Actions[0].Arguments;last_result=$info.LastTaskResult}
 }
 $result.registered_count=$result.tasks.Count
 if($StartTest) {
  & (Join-Path $root '.venv/Scripts/python.exe') -c "import sqlite3; c=sqlite3.connect('file:data/phase2/relay.sqlite?mode=ro',uri=True); assert c.execute('SELECT mode FROM operation_mode').fetchone()[0]=='MANUAL'; assert c.execute('SELECT count(*) FROM control_resume').fetchone()[0]==0"
  if($LASTEXITCODE -ne 0) {throw 'MANUAL_EMPTY_RESUME_QUEUE_REQUIRED'}
  # 대표 승인 범위의 최소 시작 시험. 기존 실행기는 marker로만 안전 중지하고 강제 종료하지 않습니다.
  'STOP' | Set-Content data/phase2/bot-stop-request -Encoding ascii
  'STOP' | Set-Content data/phase3/stop-request -Encoding ascii
  $stopped=$false
  for($i=0;$i -lt 20;$i++) {
   $bot=Get-Content data/phase2/bot-service-status.json -Raw | ConvertFrom-Json
   $art=Get-Content data/phase3/service-status.json -Raw | ConvertFrom-Json
   if($bot.status -eq 'STOPPED' -and $art.status -eq 'STOPPED') {$stopped=$true;break}
   Start-Sleep -Seconds 2
  }
  if(-not $stopped) {throw 'EXISTING_SERVICES_NOT_SAFELY_STOPPED'}
  foreach($role in @('Bot','Art')) {
   $name='LIMITLESS-'+$role+'-OnBoot'
   $task=Get-ScheduledTask -TaskName $name
   if($task.State.ToString() -ne 'Running') {Start-ScheduledTask -TaskName $name}
  }
  $healthy=$false
  for($i=0;$i -lt 18;$i++) {
   Start-Sleep -Seconds 3
   $bot=Get-Content data/phase2/bot-service-status.json -Raw | ConvertFrom-Json
   $art=Get-Content data/phase3/service-status.json -Raw | ConvertFrom-Json
   $now=[datetimeoffset]::UtcNow.ToUnixTimeSeconds()
   if($bot.status -eq 'RUNNING' -and $art.status -eq 'RUNNING' -and $now-$bot.last_tick -lt 20 -and $now-$art.last_tick -lt 20 -and -not $bot.approved_readonly_resume_only) {$healthy=$true;break}
  }
  $result.start_test=if($healthy){'PASS_NONINTERACTIVE_BATCH_ONLY'}else{'HOLD_RUNTIME_REVIEW_REQUIRED'}
  $result.bot=$bot;$result.art=$art
 }
 $result.status='VERIFIED_REGISTERED'
} catch {$result.status='HOLD';$result.reason='TASK_ACCOUNT_OR_SAFE_START_REVIEW_REQUIRED'}
$result | ConvertTo-Json -Depth 7 | Set-Content data/boot/admin-task-verification.json -Encoding utf8
Write-Output ('등록 '+$result.registered_count+'/2 / 시작시험 '+$result.start_test+' / 실제 PIN 전 부팅 NOT_VERIFIED')

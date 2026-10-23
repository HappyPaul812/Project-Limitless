param(
    [ValidateSet('Preview','Status','StartIfEnabled','Stop')][string]$Action='Status',
    [ValidateSet('Art','Bot')][string]$Service='Art'
)
# 등록/enable 기능은 없습니다. 별도 승인된 파일이 없으면 시작하지 않습니다.
$ErrorActionPreference='Stop'
$runtimeRoot=Join-Path $PSScriptRoot 'data/operations'
New-Item -ItemType Directory -Force -Path $runtimeRoot | Out-Null
$phase=if($Service -eq 'Art'){'phase3'}else{'phase2'}
$statusName=if($Service -eq 'Art'){'service-status.json'}else{'bot-service-status.json'}
$runAction=if($Service -eq 'Art'){'Phase3Run'}else{'Run'}
$stopName=if($Service -eq 'Art'){'stop-request'}else{'bot-stop-request'}
$taskName='LIMITLESS-'+$Service+'-OnLogon'
$disabled=Join-Path $runtimeRoot ($Service+'-autostart-disabled')
$approvalFile=Join-Path $runtimeRoot ($Service+'-autostart-approval.json')
$wrapper=Join-Path $PSScriptRoot 'register-discord.ps1'
function Write-SafeEvent([string]$Code) {
    @{at=[datetime]::UtcNow.ToString('o');service=$Service;code=$Code} | ConvertTo-Json -Compress | Add-Content -LiteralPath (Join-Path $runtimeRoot 'service-events.jsonl') -Encoding utf8
}
try {
    $sid=[Security.Principal.WindowsIdentity]::GetCurrent().User.Value
    if($Action -eq 'Preview') {
        $pwsh=(Get-Command pwsh -ErrorAction Stop).Source
        $arguments='-NoProfile -WindowStyle Hidden -File "'+$PSCommandPath+'" -Action StartIfEnabled -Service '+$Service
        $entry=New-ScheduledTaskAction -Execute $pwsh -Argument $arguments -WorkingDirectory $PSScriptRoot
        $trigger=New-ScheduledTaskTrigger -AtLogOn -User $sid
        $trigger.Delay='PT30S'
        $principal=New-ScheduledTaskPrincipal -UserId $sid -LogonType Interactive -RunLevel Limited
        $settings=New-ScheduledTaskSettingsSet -MultipleInstances IgnoreNew -Disable -DisallowHardTerminate -ExecutionTimeLimit ([timespan]::Zero) -RestartCount 3 -RestartInterval (New-TimeSpan -Minutes 1) -AllowStartIfOnBatteries -DontStopIfGoingOnBatteries
        $task=New-ScheduledTask -Action $entry -Trigger $trigger -Principal $principal -Settings $settings -Description 'LIMITLESS 수령/조회 전용. AI 새 승인·Unity·PC 종료·Push 없음.'
        Export-ScheduledTask -InputObject $task | Set-Content -LiteralPath (Join-Path $runtimeRoot ($taskName+'.xml')) -Encoding Unicode
        $plan=@{task_name=$taskName;registered=$false;trigger='same user logon +30seconds';run_level='Limited';identity='CURRENT_DPAPI_USER';multiple_instances='IgnoreNew';force_termination=$false;restart_count=3;restart_minutes=1;enabled=$false;service=$Service;wrapper_sha256=(Get-FileHash -LiteralPath $wrapper -Algorithm SHA256).Hash.ToLower();starter_sha256=(Get-FileHash -LiteralPath $PSCommandPath -Algorithm SHA256).Hash.ToLower();argument_template=$arguments}
        $plan | ConvertTo-Json | Set-Content -LiteralPath (Join-Path $runtimeRoot ($Service+'-autostart-plan.json')) -Encoding utf8
        Write-SafeEvent 'PREVIEW_EXPORTED_NOT_REGISTERED'
        Write-Output '미등록 자동 시작 XML/설정안 생성 완료. 승인 파일 없음·시작 비활성.'
        return
    }
    if($Action -eq 'Status') {
        $statusPath=Join-Path $PSScriptRoot ('data/'+$phase+'/'+$statusName)
        $state=if(Test-Path -LiteralPath $statusPath){Get-Content -LiteralPath $statusPath -Raw | ConvertFrom-Json}else{$null}
        @{service=$Service;approval_present=(Test-Path -LiteralPath $approvalFile);disabled=(Test-Path -LiteralPath $disabled);status=$state} | ConvertTo-Json -Depth 3
        return
    }
    if($Action -eq 'Stop') {
        Set-Content -LiteralPath $disabled -Value 'DISABLED_BY_OWNER' -Encoding ascii
        Set-Content -LiteralPath (Join-Path $PSScriptRoot ('data/'+$phase+'/'+$stopName)) -Value 'STOP' -Encoding ascii
        Write-SafeEvent 'STOP_REQUESTED_AUTOSTART_DISABLED'
        Write-Output '수령/조회 서비스 안전 중지 요청·자동 시작 비활성. 업무/파일 강제 종료 없음.'
        return
    }
    if((Test-Path -LiteralPath $disabled) -or -not (Test-Path -LiteralPath $approvalFile)) {
        Write-SafeEvent 'HOLD_AUTOSTART_NOT_APPROVED_OR_DISABLED'
        Write-Output 'HOLD: 자동 시작 별도 승인 없음 또는 대표가 중지했습니다.'
        return
    }
    $approval=Get-Content -LiteralPath $approvalFile -Raw | ConvertFrom-Json
    if($approval.kind -ne 'EXPLICIT_OWNER_AUTOSTART_APPROVAL' -or $approval.service -ne $Service -or $approval.user_sid -ne $sid -or $approval.wrapper_sha256 -ne (Get-FileHash -LiteralPath $wrapper -Algorithm SHA256).Hash.ToLower() -or $approval.starter_sha256 -ne (Get-FileHash -LiteralPath $PSCommandPath -Algorithm SHA256).Hash.ToLower()) {
        Write-SafeEvent 'HOLD_AUTOSTART_SCOPE_OR_SHA_MISMATCH'
        return
    }
    $heartbeatPath=Join-Path $PSScriptRoot ('data/'+$phase+'/'+$statusName)
    if(Test-Path -LiteralPath $heartbeatPath) {
        $heartbeat=Get-Content -LiteralPath $heartbeatPath -Raw | ConvertFrom-Json
        if($heartbeat.status -eq 'RUNNING' -and ([datetimeoffset]::UtcNow.ToUnixTimeSeconds()-$heartbeat.last_tick) -lt 30) {
            Write-SafeEvent 'SERVICE_ALREADY_RUNNING'
            return
        }
    }
    # 같은 사용자의 DPAPI·ACL 검증은 기존 wrapper가 수행합니다. 토큰은 명령줄에 넣지 않습니다.
    $pwsh=(Get-Command pwsh -ErrorAction Stop).Source
    $launchArguments='-NoProfile -File "'+$wrapper+'" -Action '+$runAction
    $stamp=[datetime]::UtcNow.Ticks.ToString()
    $worker=Start-Process -FilePath $pwsh -ArgumentList $launchArguments -WorkingDirectory $PSScriptRoot -WindowStyle Hidden -RedirectStandardOutput (Join-Path $runtimeRoot ($Service+'-stdout-'+$stamp+'-local.log')) -RedirectStandardError (Join-Path $runtimeRoot ($Service+'-stderr-'+$stamp+'-local.log')) -PassThru
    Write-SafeEvent 'APPROVED_START_REQUESTED'
    $worker.WaitForExit()
    if($worker.ExitCode -ne 0) { Write-SafeEvent 'SERVICE_EXIT_FAILED'; exit 2 }
    Write-SafeEvent 'SERVICE_EXITED_SAFELY'
} catch {
    Write-SafeEvent 'OPERATION_FAILED_PRIVATE_LOG_REVIEW_REQUIRED'
    Write-Output '안전 시작/중지 처리 실패·원문 비밀정보 출력 없음. 고정 이벤트 로그를 확인하세요.'
    exit 2
}

param([ValidateSet('Setup','Status','Test','Run','HandoffPrepare','HandoffFinish','AIReadyReport','AIResultReport','PlanningApprovedOnce','PlanningFinalRetest','PlanningRetestReport','CodexIssue63Once','CodexIssue63Report','AutoHandoff63Once','ArtRolesOnce','ArtRolesFailureReport','NewArtRetestOnce','RenewalOnce','FourRolePilotOnce','Phase3Inspect','Phase3Setup','Phase3Run','Phase3LiveTest')][string]$Action='Setup',[switch]$Replace)
$ErrorActionPreference = 'Stop'
Set-Location -LiteralPath $PSScriptRoot
Import-Module (Join-Path $PSScriptRoot 'DiscordSecretStore.psm1') -Force
$secretRoot = Join-Path $PSScriptRoot 'data/private/discord'
$secureInput = $null; $saved = $null; $bstr = [IntPtr]::Zero
$oldEnvironment = @{}
$names = @('DISCORD_BOT_TOKEN','LIMITLESS_OWNER_IDS','LIMITLESS_GUILD_ID','LIMITLESS_CHANNEL_IDS','LIMITLESS_ALLOW_DISCORD','LIMITLESS_HANDOFF_FINISH')
foreach ($name in $names) { $oldEnvironment[$name] = [Environment]::GetEnvironmentVariable($name,'Process') }
try {
    if ($Action -eq 'Status') {
        Get-LimitlessDiscordCredentialStatus $secretRoot | ConvertTo-Json -Compress
        return
    }
    if ($Action -eq 'Setup') {
        Write-Output '토큰은 숨김 입력 후 현재 Windows 사용자 전용으로 암호화 저장합니다. 토큰을 채팅/GitHub에 보내지 마세요.'
        $owners = Read-Host '바울 대표 사용자 숫자 ID (여럿이면 쉼표)'
        $guild = Read-Host '초대 완료한 테스트 서버 숫자 ID'
        $channels = Read-Host '허용 텍스트 채널 숫자 ID (첫 채널에 고정 테스트 메시지1개 전송)'
        $secureInput = Read-Host 'Bot Token (입력 숨김)' -AsSecureString
        Save-LimitlessDiscordCredential -Root $secretRoot -Token $secureInput -Owners $owners -Guild $guild -Channels $channels -Replace:$Replace
    }
    $saved = Read-LimitlessDiscordCredential $secretRoot
    $bstr = [Runtime.InteropServices.Marshal]::SecureStringToBSTR($saved.Token)
    $env:DISCORD_BOT_TOKEN = [Runtime.InteropServices.Marshal]::PtrToStringBSTR($bstr)
    $env:LIMITLESS_OWNER_IDS = $saved.Owners
    $env:LIMITLESS_GUILD_ID = $saved.Guild
    $env:LIMITLESS_CHANNEL_IDS = $saved.Channels
    $env:LIMITLESS_ALLOW_DISCORD = '1'
    if ($Action -in @('Phase3Setup','Phase3Run','Phase3LiveTest')) {
        $phase3Action = @{Phase3Setup='setup';Phase3Run='run';Phase3LiveTest='live-test'}[$Action]
        & '.venv/Scripts/python.exe' -m collab.delivery_discord $phase3Action
    } elseif ($Action -eq 'Phase3Inspect') {
        & '.venv/Scripts/python.exe' -m collab.delivery_discord inspect
    } elseif ($Action -eq 'FourRolePilotOnce') {
        & '.venv/Scripts/python.exe' -m collab.four_role_pilot
    } elseif ($Action -eq 'RenewalOnce') {
        & '.venv/Scripts/python.exe' -m collab.renewal_once
    } elseif ($Action -eq 'NewArtRetestOnce') {
        & '.venv/Scripts/python.exe' -m collab.newart_retest
    } elseif ($Action -eq 'ArtRolesFailureReport') {
        & '.venv/Scripts/python.exe' -m collab.ai_ready art-failure-result-report
    } elseif ($Action -eq 'ArtRolesOnce') {
        & '.venv/Scripts/python.exe' -m collab.art_roles_once
    } elseif ($Action -eq 'AutoHandoff63Once') {
        & '.venv/Scripts/python.exe' -m collab.auto63
    } elseif ($Action -eq 'CodexIssue63Once') {
        & '.venv/Scripts/python.exe' -m collab.codex_issue63_once
    } elseif ($Action -eq 'CodexIssue63Report') {
        & '.venv/Scripts/python.exe' -m collab.ai_ready codex63-result-report
    } elseif ($Action -eq 'PlanningFinalRetest') {
        & '.venv/Scripts/python.exe' -m collab.planning_retest
    } elseif ($Action -eq 'PlanningRetestReport') {
        & '.venv/Scripts/python.exe' -m collab.ai_ready retest-result-report
    } elseif ($Action -eq 'PlanningApprovedOnce') {
        & '.venv/Scripts/python.exe' -m collab.planning_once
    } elseif ($Action -in @('AIReadyReport','AIResultReport')) {
        $reportAction = if ($Action -eq 'AIResultReport') { 'result-report' } else { 'report' }
        & '.venv/Scripts/python.exe' -m collab.ai_ready $reportAction
    } elseif ($Action -in @('HandoffPrepare','HandoffFinish')) {
        $env:LIMITLESS_HANDOFF_FINISH = if ($Action -eq 'HandoffFinish') { '1' } else { '0' }
        & '.venv/Scripts/python.exe' -m collab.handoff_live
    } elseif ($Action -eq 'Run') {
        & '.venv/Scripts/python.exe' -m collab discord --data data/phase2
    } else {
        # Setup 직후 또는 Test는 최대45초 동안 연결/명령9종/메시지1개만 검사하고 닫습니다.
        & '.venv/Scripts/python.exe' -m collab.discord_check --data data/phase2 --timeout 45
    }
    if ($LASTEXITCODE -ne 0) {
        if ($Action -eq 'PlanningApprovedOnce') {
            Write-Output 'PlanningAgent 결과 미검증. data/ai-ready/planning-actual-result.json을 확인하세요. 자동 재실행하지 않습니다.'
        } else {
            Write-Output '연결 검증 미완료. 비밀 없는 해당 작업 결과 파일을 확인하세요.'
        }
        exit 2
    }
} catch {
    # 예외 원문이나 암호문도 터미널/대화에 복사하지 않습니다.
    Write-Output '설정/읽기 실패: 숫자 ID, 현재 Windows 사용자, 기존 등록 여부를 확인하세요. 처음이면 Setup, 교체는 Setup -Replace를 사용하세요.'
    exit 2
} finally {
    if ($bstr -ne [IntPtr]::Zero) { [Runtime.InteropServices.Marshal]::ZeroFreeBSTR($bstr) }
    if ($null -ne $secureInput) { $secureInput.Dispose() }
    if ($null -ne $saved) { $saved.Token.Dispose() }
    foreach ($name in $names) { [Environment]::SetEnvironmentVariable($name,$oldEnvironment[$name],'Process') }
}

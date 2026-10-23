$ErrorActionPreference = 'Stop'
Set-Location -LiteralPath $PSScriptRoot
# 바울 대표가 직접 실행합니다. 토큰은 화면/파일/셸 명령 이력에 넣지 않습니다.
$phase2Token = $null
$phase2Bstr = [IntPtr]::Zero
$phase2OldEnvironment = @{}
$phase2Names = @('DISCORD_BOT_TOKEN','LIMITLESS_OWNER_IDS','LIMITLESS_GUILD_ID','LIMITLESS_CHANNEL_IDS','LIMITLESS_ALLOW_DISCORD')
foreach ($phase2Name in $phase2Names) {
    $phase2OldEnvironment[$phase2Name] = [Environment]::GetEnvironmentVariable($phase2Name,'Process')
}
try {
    $env:LIMITLESS_OWNER_IDS = Read-Host '바울 대표 Discord 사용자 숫자 ID (여럿이면 쉼표)'
    $env:LIMITLESS_GUILD_ID = Read-Host '테스트 서버 숫자 ID'
    $env:LIMITLESS_CHANNEL_IDS = Read-Host '허용 텍스트 채널 숫자 ID (여럿이면 쉼표)'
    $phase2Token = Read-Host 'Bot Token (입력은 숨겨집니다)' -AsSecureString
    $phase2Bstr = [Runtime.InteropServices.Marshal]::SecureStringToBSTR($phase2Token)
    $env:DISCORD_BOT_TOKEN = [Runtime.InteropServices.Marshal]::PtrToStringBSTR($phase2Bstr)
    $env:LIMITLESS_ALLOW_DISCORD = '1'
    $phase2Check = & '.venv/Scripts/python.exe' -m collab.phase2 preflight | ConvertFrom-Json
    if ($LASTEXITCODE -ne 0) { throw '설정 확인 실패' }
    if ($phase2Check.status -ne 'LOCAL_CONFIG_READY') { throw '숫자 ID·토큰·연결 스위치 설정을 확인하세요.' }
    Write-Output '로컬 설정 확인 완료. Discord 테스트 서버에 연결하고 명령9종을 등록합니다.'
    & '.venv/Scripts/python.exe' -m collab discord --data data/phase2
    if ($LASTEXITCODE -ne 0) { throw '실연결 미완료. 토큰·초대·ID·권한을 로컬에서 확인하세요.' }
} finally {
    if ($phase2Bstr -ne [IntPtr]::Zero) { [Runtime.InteropServices.Marshal]::ZeroFreeBSTR($phase2Bstr) }
    if ($null -ne $phase2Token) { $phase2Token.Dispose() }
    foreach ($phase2Name in $phase2Names) {
        [Environment]::SetEnvironmentVariable($phase2Name,$phase2OldEnvironment[$phase2Name],'Process')
    }
}

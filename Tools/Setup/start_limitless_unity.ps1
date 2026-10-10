param(
    [string]$PythonEnv = "E:\Program Files\anaconda3\envs\py3_12",
    [switch]$DryRun
)

$ErrorActionPreference = "Stop"

# 이 스크립트 위치(Tools/Setup)를 기준으로 저장소와 Unity 프로젝트를 찾습니다.
$repoRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..")).Path
$projectPath = Join-Path $repoRoot "Unity\Client"
$projectVersionFile = Join-Path $projectPath "ProjectSettings\ProjectVersion.txt"

if (-not (Test-Path $projectVersionFile)) {
    throw "Unity 프로젝트 버전 파일을 찾지 못했습니다: $projectVersionFile"
}

# MCP for Unity가 요구하는 Python 3.10+와 uv를 현재 Unity 프로세스 PATH에 넣습니다.
$pythonExe = Join-Path $PythonEnv "python.exe"
$pythonScripts = Join-Path $PythonEnv "Scripts"

if (-not (Test-Path $pythonExe)) {
    throw "py3_12 Python을 찾지 못했습니다: $pythonExe"
}

$env:Path = "$PythonEnv;$pythonScripts;$env:Path"
# uv가 서버용 환경을 만들 때도 지정한 py3_12 인터프리터를 사용합니다.
$env:UV_PYTHON = $pythonExe

$pythonVersionText = (& $pythonExe --version 2>&1 | Out-String).Trim()
if ($pythonVersionText -notmatch '^Python\s+(\d+)\.(\d+)') {
    throw "Python 버전을 확인하지 못했습니다: $pythonVersionText"
}

$pythonMajor = [int]$Matches[1]
$pythonMinor = [int]$Matches[2]
if ($pythonMajor -lt 3 -or ($pythonMajor -eq 3 -and $pythonMinor -lt 10)) {
    throw "MCP for Unity는 Python 3.10 이상이 필요합니다. 현재: $pythonVersionText"
}

$uvCommand = Get-Command uv.exe -ErrorAction SilentlyContinue
if (-not $uvCommand) {
    throw "uv.exe를 찾지 못했습니다. '$pythonExe -m pip install uv'를 한 번 실행해 주세요."
}

$uvVersionText = (& $uvCommand.Source --version 2>&1 | Out-String).Trim()
if ($LASTEXITCODE -ne 0) { throw "uv 실행에 실패했습니다: $uvVersionText" }
$uvxExe = Join-Path $pythonScripts "uvx.exe"
if (-not (Test-Path -LiteralPath $uvxExe)) {
    throw "MCP 서버 실행에 필요한 uvx.exe를 찾지 못했습니다: $uvxExe"
}
$uvxVersionText = (& $uvxExe --version 2>&1 | Out-String).Trim()
if ($LASTEXITCODE -ne 0) { throw "uvx 실행에 실패했습니다: $uvxVersionText" }

# ProjectVersion.txt에서 이 프로젝트가 요구하는 정확한 Unity 버전을 읽습니다.
$versionLine = Get-Content $projectVersionFile | Where-Object { $_ -match '^m_EditorVersion:' } | Select-Object -First 1
if (-not $versionLine) {
    throw "ProjectVersion.txt에서 m_EditorVersion을 찾지 못했습니다."
}
$unityVersion = ($versionLine -split ':', 2)[1].Trim()

# 사용자 PC의 일반적인 Unity Hub Editor 설치 위치에서 정확한 버전을 찾습니다.
$editorCandidates = @(
    "E:\Program Files\Unity\Hub\Editor\$unityVersion\Editor\Unity.exe",
    "C:\Program Files\Unity\Hub\Editor\$unityVersion\Editor\Unity.exe",
    (Join-Path $env:ProgramFiles "Unity\Hub\Editor\$unityVersion\Editor\Unity.exe")
) | Select-Object -Unique

$unityExe = $editorCandidates | Where-Object { Test-Path $_ } | Select-Object -First 1

Write-Host ""
Write-Host "LIMITLESS Unity 개발환경 실행" -ForegroundColor Cyan
Write-Host "  Project : $projectPath"
Write-Host "  Unity   : $unityVersion"
Write-Host "  Python  : $pythonVersionText"
Write-Host "  uv      : $uvVersionText"
Write-Host "  uvx     : $uvxVersionText ($uvxExe)"
Write-Host "  Editor  : $unityExe"
Write-Host ""

if ($DryRun) {
    if (-not $unityExe) { throw "Unity $unityVersion Editor를 찾지 못했습니다. DryRun에서는 Hub를 실행하지 않습니다." }
    Write-Host "DryRun 완료: Python/uv/uvx와 정확한 Editor 경로를 확인했습니다. 프로세스는 시작하지 않았습니다."
    return
}

# 이미 실행 중인 Editor가 있으면 새 Editor를 띄우지 않습니다. Hub의 unity.exe CLI는 제외합니다.
$runningEditors = @(Get-Process -Name Unity -ErrorAction SilentlyContinue | Where-Object {
    -not $_.Path -or $_.Path -notmatch 'Unity Hub[\\/]resources[\\/]unity\.exe$'
})
if ($runningEditors.Count -gt 0) {
    throw "실행 중인 Unity Editor가 있습니다. 기존 작업을 저장하고 직접 종료한 뒤 실행해 주세요. 강제 종료하지 않습니다."
}

if ($unityExe) {
    Write-Host "Unity Editor를 직접 실행합니다." -ForegroundColor Green
    Start-Process -FilePath $unityExe -ArgumentList @('-projectPath', ('"' + $projectPath + '"'))
    Write-Host "실행 완료. MCP for Unity의 Auto-Start on Editor Load가 켜져 있으면 MCP 서버도 자동으로 연결됩니다." -ForegroundColor Green
    exit 0
}

# Unity Editor 경로를 찾지 못한 경우에도 PATH를 유지한 채 Hub를 실행할 수 있도록 fallback 합니다.
$hubCandidates = @(
    "E:\Program Files\Unity Hub\Unity Hub.exe",
    "C:\Program Files\Unity Hub\Unity Hub.exe",
    (Join-Path $env:ProgramFiles "Unity Hub\Unity Hub.exe")
) | Select-Object -Unique

$hubExe = $hubCandidates | Where-Object { Test-Path $_ } | Select-Object -First 1
if (-not $hubExe) {
    throw "Unity $unityVersion Editor와 Unity Hub를 찾지 못했습니다. 설치 위치를 확인해 주세요."
}

Write-Warning "Unity $unityVersion Editor의 기본 설치 경로를 찾지 못해 Unity Hub를 실행합니다. Hub에서 Unity/Client 프로젝트를 열어 주세요."
Start-Process -FilePath $hubExe

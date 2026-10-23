$ErrorActionPreference = 'Stop'
# 기존 conda 패키지를 건드리지 않고 이 도구 아래에만 가상 환경을 만듭니다.
Set-Location -LiteralPath $PSScriptRoot
$taskPython = 'E:/Program Files/anaconda3/envs/py3_12/python.exe'
if (-not (Test-Path -LiteralPath $taskPython)) { throw 'Python 3.12 경로를 setup.ps1에서 지정하세요.' }
New-Item -ItemType Directory -Force -Path 'data/tmp' | Out-Null
$env:TEMP = (Resolve-Path -LiteralPath 'data/tmp').Path
$env:TMP = $env:TEMP
if (-not (Test-Path -LiteralPath '.venv/Scripts/python.exe')) {
    & $taskPython -m venv .venv
    if ($LASTEXITCODE -ne 0) { throw 'venv 생성 실패' }
}
& '.venv/Scripts/python.exe' -m pip install --disable-pip-version-check -r requirements.lock.txt
if ($LASTEXITCODE -ne 0) { throw '독립 venv 의존성 설치 실패' }
Write-Output '설치 완료. python -m collab demo는 로컬 모의 실행입니다. Discord·AI는 연결하지 않았습니다.'

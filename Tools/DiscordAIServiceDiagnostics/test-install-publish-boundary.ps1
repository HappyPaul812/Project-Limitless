param([Parameter(Mandatory)][string]$OutputPath)
# 설치기를 실행하지 않고 고정된 내장 게시기만 compile합니다. 비관리자는 생성 전에 거부돼야 합니다.
$ErrorActionPreference='Stop'
$source=[IO.File]::ReadAllText((Join-Path $PSScriptRoot 'install-session0-trial.ps1'))
$match=[regex]::Match($source,"(?s)Add-Type -TypeDefinition @'\r?\n(.*?)\r?\n'@")
if(-not $match.Success){throw 'ATOMIC_SOURCE_NOT_FOUND'}
Add-Type -TypeDefinition $match.Groups[1].Value
$target='F:/study/codex/Project-Limitless/Temp/AtomicPublishMustNotExist'
if(Test-Path -LiteralPath $target){throw 'NEGATIVE_TEST_TARGET_ALREADY_EXISTS'}
$denied=$false
try{[LimitlessAtomicTrialPublishV1]::NewDirectory($target)}catch{$denied=$_.Exception.ToString().Contains('ATOMIC_ADMIN_REQUIRED')}
$result=@{checks=@{atomic_publisher_compiles=$true;nonadmin_rejected_before_creation=($denied -and -not (Test-Path -LiteralPath $target))};system_changes=0}
$result|ConvertTo-Json -Depth 4|Set-Content -LiteralPath $OutputPath -Encoding utf8
$result|ConvertTo-Json -Depth 4
if(-not $result.checks.nonadmin_rejected_before_creation){exit 2}

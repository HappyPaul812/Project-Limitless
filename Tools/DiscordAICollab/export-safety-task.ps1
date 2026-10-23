param([Parameter(Mandatory=$true)][string]$At)
# XML 미리보기만 생성합니다. Register-ScheduledTask나 시스템 종료를 호출하지 않습니다.
$ErrorActionPreference = 'Stop'
$zone = [TimeZoneInfo]::FindSystemTimeZoneById('Korea Standard Time')
$localTime = [datetime]::ParseExact($At,'yyyy-MM-dd HH:mm',[Globalization.CultureInfo]::InvariantCulture)
$utcTime = [TimeZoneInfo]::ConvertTimeToUtc([datetime]::SpecifyKind($localTime,[DateTimeKind]::Unspecified),$zone)
if ($utcTime -le [datetime]::UtcNow) { throw '미래 서울 시각을 입력하세요.' }
$sid = [Security.Principal.WindowsIdentity]::GetCurrent().User.Value
$python = Join-Path $PSScriptRoot '.venv/Scripts/python.exe'
if (-not (Test-Path -LiteralPath $python -PathType Leaf)) { throw '격리 Python이 없습니다.' }
$action = New-ScheduledTaskAction -Execute $python -Argument '-m collab.safety_tick' -WorkingDirectory $PSScriptRoot
$trigger = New-ScheduledTaskTrigger -Once -At $utcTime.ToLocalTime()
$principal = New-ScheduledTaskPrincipal -UserId $sid -LogonType Interactive -RunLevel Limited
$settings = New-ScheduledTaskSettingsSet -MultipleInstances IgnoreNew -DisallowHardTerminate -ExecutionTimeLimit (New-TimeSpan -Minutes 1)
$task = New-ScheduledTask -Action $action -Trigger $trigger -Principal $principal -Settings $settings -Description 'LIMITLESS 모의 안전 점검만. AI·종료·Git 실행 없음.'
$folder = Join-Path $PSScriptRoot 'data/safety'
New-Item -ItemType Directory -Force -Path $folder | Out-Null
$output = Join-Path $folder 'task-preview.xml'
Export-ScheduledTask -InputObject $task | Set-Content -LiteralPath $output -Encoding Unicode
Write-Output "미등록 XML 미리보기: $output"

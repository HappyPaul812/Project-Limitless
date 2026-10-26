param([Parameter(Mandatory)][string]$Root,[ValidateSet('Host','Stop')][string]$Action='Host')
Set-StrictMode -Version Latest
$ErrorActionPreference='Stop'
$result=@{status='HOLD';model_calls=0;official_usage_reads=0;operational_sqlite_operations=0}
$saved=$null
$validatedRoot=$false
try {
    $full=[IO.Path]::GetFullPath($Root)
    $allowed='F:\study\codex\Project-Limitless\Temp\MobileLive\'
    if (-not $full.StartsWith($allowed,[StringComparison]::OrdinalIgnoreCase) -or [IO.Path]::GetFileName($full) -notmatch '^[0-9a-f]{32}$') {throw 'TRIAL_ROOT_HOLD'}
    $part=$full
    while ($part) {
        if ((Test-Path -LiteralPath $part) -and ((Get-Item -LiteralPath $part -Force).Attributes -band [IO.FileAttributes]::ReparsePoint)) {throw 'LINK_HOLD'}
        $parent=[IO.Directory]::GetParent($part);if($null -eq $parent){break};$part=$parent.FullName
    }
    $validatedRoot=$true
    if ($Action -eq 'Stop') {
        [IO.File]::WriteAllText((Join-Path $full 'stop-request'),'STOP',[Text.Encoding]::ASCII)
        Write-Output 'TRIAL_STOP_REQUESTED';exit 0
    }
    if (Test-Path -LiteralPath (Join-Path $full 'parent-launched.json')) {throw 'PARENT_ALREADY_LAUNCHED_NO_RETRY'}
    $config=Get-Content -LiteralPath (Join-Path $full 'run-config.json') -Raw|ConvertFrom-Json
    if ([string]$config.trial_application_id -ne '1564156403571105906' -or [string]$config.production_application_id -ne '1563102982386749440') {throw 'APP_PAIR_HOLD'}
    foreach($p in $config.host_sha256.PSObject.Properties) {
        if ((Get-FileHash -LiteralPath (Join-Path $full $p.Name) -Algorithm SHA256).Hash.ToLowerInvariant() -ne $p.Value) {throw 'HOST_SHA_HOLD'}
    }
    foreach($p in $config.package_sha256.PSObject.Properties) {
        if ((Get-FileHash -LiteralPath (Join-Path (Join-Path $full 'Package') $p.Name) -Algorithm SHA256).Hash.ToLowerInvariant() -ne $p.Value) {throw 'PACKAGE_SHA_HOLD'}
    }
    $setupPath='F:/study/codex/Project-Limitless/Temp/MobileTrialCredentials20261026/LimitlessTest/setup-result.json'
    $setup=Get-Content -LiteralPath $setupPath -Raw|ConvertFrom-Json
    if ($setup.status -ne 'TRIAL_TOKEN_READY_HOST_NOT_STARTED' -or [Security.Principal.WindowsIdentity]::GetCurrent().Name -ne $setup.current_windows_identity) {throw 'SAME_WINDOWS_ACCOUNT_REQUIRED'}
    $module=Join-Path $full 'Package/Tools/DiscordAICollab/DiscordSecretStore.psm1'
    Import-Module -Name $module -Force
    $saved=Read-LimitlessDiscordCredential $setup.secret_root
    $saved.Token.Dispose();$saved=$null
    $aPath='C:/ProgramData/LimitlessRelay/boot-approval.json'
    if ((Get-FileHash -LiteralPath $aPath -Algorithm SHA256).Hash.ToLowerInvariant() -ne '3e032ebcce3d91bd762a8dec81048d04607ddde761afd62489c0fa2b8e49140f') {throw 'BOOT_APPROVAL_SHA_HOLD'}
    $a=Get-Content -LiteralPath $aPath -Raw|ConvertFrom-Json
    $venvConfig=Join-Path $a.tool_root '.venv/pyvenv.cfg'
    if ((Get-FileHash -LiteralPath $venvConfig -Algorithm SHA256).Hash.ToLowerInvariant() -ne $config.venv_config_sha256) {throw 'VENV_CONFIG_SHA_HOLD'}
    $baseLine=@(Get-Content -LiteralPath $venvConfig|Where-Object {$_ -match '^executable = '})
    if ($baseLine.Count -ne 1) {throw 'BASE_PYTHON_PATH_HOLD'}
    $python=$baseLine[0].Substring('executable = '.Length)
    if ([IO.Path]::GetFullPath($python) -ne [IO.Path]::GetFullPath($config.python_path)) {throw 'BASE_PYTHON_PIN_HOLD'}
    if ((Get-FileHash -LiteralPath $python -Algorithm SHA256).Hash.ToLowerInvariant() -ne $config.python_sha256) {throw 'PYTHON_SHA_HOLD'}
    $result.current_windows_identity=[Security.Principal.WindowsIdentity]::GetCurrent().Name
    # 다른 서비스나 시스템 환경변수는 바꾸지 않고 이 일회성 부모의 자식 환경만 정리합니다.
    foreach($name in @(Get-ChildItem Env:|Where-Object {$_.Name -match '^(CODEX_|OPENAI_|DISCORD_|LIMITLESS_)'}|Select-Object -ExpandProperty Name)) {
        [Environment]::SetEnvironmentVariable($name,$null,'Process')
    }
    [IO.File]::WriteAllText((Join-Path $full 'parent-launched.json'),(@{pid=$PID;started=[DateTimeOffset]::Now.ToString('o')}|ConvertTo-Json),[Text.UTF8Encoding]::new($false))
    Add-Type -Path (Join-Path $full 'MobileTrialJob.cs')
    $worker=Join-Path $full 'mobile-trial-host.py'
    $job=[LimitlessMobileTrialJobV1]::Run($python,@('-I','-B','-X','utf8',$worker,'--root',$full),$full,1230000)
    $result.job=@{exit_code=$job.ExitCode;timed_out=$job.TimedOut;active_processes=$job.ActiveProcesses;cleanup_verified=$job.CleanupVerified;total_processes=$job.TotalProcesses}
    # worker가 정리 전에 강제 종료됐으면 기록된 반환 ID만 별도 정리합니다. DELETE 재시도는 없습니다.
    if ((Test-Path -LiteralPath (Join-Path $full 'registered.json')) -and -not (Test-Path -LiteralPath (Join-Path $full 'delete-attempted.json'))) {
        $clean=[LimitlessMobileTrialJobV1]::Run($python,@('-I','-B','-X','utf8',$worker,'--root',$full,'--cleanup-only'),$full,40000)
        $result.recovery_cleanup_job=@{exit_code=$clean.ExitCode;active_processes=$clean.ActiveProcesses;cleanup_verified=$clean.CleanupVerified}
    }
    $result.status='HOST_EXITED_REVIEW_SUMMARY'
    $result|ConvertTo-Json -Depth 6|Set-Content -LiteralPath (Join-Path $full 'parent-result.json') -Encoding utf8
} catch {
    $result.error_type=$_.Exception.GetType().Name;$result.status='HOST_PARENT_HOLD'
    if ($validatedRoot -and (Test-Path -LiteralPath $full)) {$result|ConvertTo-Json -Depth 6|Set-Content -LiteralPath (Join-Path $full 'parent-result.json') -Encoding utf8}
} finally {
    if ($null -ne $saved) {$saved.Token.Dispose()}
}

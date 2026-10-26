param([Parameter(Mandatory)][string]$OutputPath)
# 새 익명 stdin 연결만 가짜 PowerShell로 시험합니다. 실제 서비스/IPC/모델은 시작하지 않습니다.
$ErrorActionPreference='Stop'
Add-Type -Path @((Join-Path $PSScriptRoot 'BrokerJob90.cs'),(Join-Path $PSScriptRoot 'Session0PipeBoundary.cs'),(Join-Path $PSScriptRoot 'Session0Host.cs'),(Join-Path $PSScriptRoot 'TrialAccountPolicy.cs'),(Join-Path $PSScriptRoot 'HostJoiningNativeTests.cs'))
$exe='C:/Program Files/PowerShell/7/pwsh.exe';$args=@('-NoProfile','-NonInteractive','-Command','$v=[Console]::In.ReadToEnd();[Console]::Out.Write($v)')
$inputBytes=[Text.Encoding]::UTF8.GetBytes('{"fake":"HOST_WORKER_PRIVATE_STDIN"}')
$none=[Threading.CancellationToken]::None
$one=[LimitlessBrokerJob90V1]::RunInput($exe,$args,$PSScriptRoot,5000,$none,$inputBytes)
$pair=[JoinNativeTest]::Parallel($exe,$args,$PSScriptRoot,$inputBytes)
$stop=[Threading.CancellationTokenSource]::new();$stop.CancelAfter(500)
$cancelled=[LimitlessBrokerJob90V1]::RunInput($exe,@('-NoProfile','-NonInteractive','-Command','[Console]::In.ReadToEnd()|Out-Null;Start-Sleep -Seconds 30'),$PSScriptRoot,5000,$stop.Token,$inputBytes)
$stop.Dispose()
$expected=[Text.Encoding]::UTF8.GetString($inputBytes)
$checks=[ordered]@{host_assembly_compiles=$true;stdin_roundtrip=($one.Stdout -ceq $expected -and $one.CleanupVerified -and $one.ActiveProcesses -eq 0);parallel_private_stdin=(@($pair|Where-Object{$_.Stdout -cne $expected -or -not $_.CleanupVerified -or $_.ActiveProcesses -ne 0}).Count -eq 0);cancel_input_tree=($cancelled.Cancelled -and $cancelled.CleanupVerified -and $cancelled.ActiveProcesses -eq 0)}
$checks.nonservice_cannot_change_query_acl=$false
try{[LimitlessSession0PipeV1]::GrantIdentityQueryForBot('S-1-5-21-1-1001','S-1-5-21-1-1002')}
catch{$checks.nonservice_cannot_change_query_acl=$_.Exception.ToString().Contains('QUERY_SERVICE_CONTEXT_HOLD')}
@{checks=$checks;actual_service_ipc=$false;actual_models=0;actual_usage=0}|ConvertTo-Json -Depth 5|Set-Content -LiteralPath $OutputPath -Encoding utf8
$checks|ConvertTo-Json
if(@($checks.Values|Where-Object{-not $_}).Count){exit 2}

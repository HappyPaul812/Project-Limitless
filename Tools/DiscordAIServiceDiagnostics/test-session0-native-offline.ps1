param([Parameter(Mandatory)][string]$OutputPath)
# 새 Job/pipe helper만 검증합니다. 실제 서비스·Codex·Discord를 실행하지 않습니다.
$ErrorActionPreference='Stop'
Add-Type -Path (Join-Path $PSScriptRoot 'BrokerJob90.cs')
Add-Type -Path (Join-Path $PSScriptRoot 'Session0PipeBoundary.cs')
$pwshPath='C:/Program Files/PowerShell/7/pwsh.exe'
$none=[Threading.CancellationToken]::None
$normal=[LimitlessBrokerJob90V1]::Run($pwshPath,@('-NoProfile','-NonInteractive','-Command','exit 0'),$PSScriptRoot,5000,$none)
$timeout=[LimitlessBrokerJob90V1]::Run($pwshPath,@('-NoProfile','-NonInteractive','-Command','Start-Sleep -Seconds 30'),$PSScriptRoot,3000,$none)
$stop=[Threading.CancellationTokenSource]::new();$stop.CancelAfter(2500)
$fakeTree='$p=Start-Process -FilePath "C:/Program Files/PowerShell/7/pwsh.exe" -ArgumentList @("-NoProfile","-NonInteractive","-Command","Start-Sleep -Seconds 30") -WindowStyle Hidden -PassThru; Write-Output $p.Id; Start-Sleep -Seconds 30'
$cancelled=[LimitlessBrokerJob90V1]::Run($pwshPath,@('-NoProfile','-NonInteractive','-Command',$fakeTree),$PSScriptRoot,7000,$stop.Token)
$stop.Dispose()
$peer=[LimitlessSession0PipeV1]::Current()
$checks=[ordered]@{
 normal=($normal.ExitCode -eq 0 -and $normal.CleanupVerified -and $normal.ActiveProcesses -eq 0)
 timeout=($timeout.TimedOut -and $timeout.CleanupVerified -and $timeout.ActiveProcesses -eq 0)
 cancel_tree=($cancelled.Cancelled -and $cancelled.CleanupVerified -and $cancelled.ActiveProcesses -eq 0 -and $cancelled.TotalProcesses -ge 2)
 limited_admin_rejected=(-not [LimitlessSession0PipeV1]::CanArm($peer,$peer.Sid))
 compile=$true
 worktree_rejected=$false
 nonservice_cannot_listen=$false
 oversize_frame_rejected=$false
}
try{[LimitlessSession0PipeV1]::VerifyProtectedPath($PSScriptRoot,$false,'S-1-5-21-1-1002')}
catch{$checks.worktree_rejected=$_.Exception.ToString().Contains('PROTECTED_ROOT_HOLD')}
try{$pipe=[LimitlessSession0PipeV1]::CreateServicePipe(('LimitlessPlanning-'+('a'*32)+'-work'),'S-1-5-21-1-1001','S-1-5-21-1-1002');$pipe.Dispose()}
catch{$checks.nonservice_cannot_listen=$_.Exception.ToString().Contains('SERVICE_CONTEXT_HOLD')}
$stream=[IO.MemoryStream]::new([BitConverter]::GetBytes(16385))
try{[LimitlessSession0PipeV1]::ReadFrame($stream,$none).GetAwaiter().GetResult()}
catch{$checks.oversize_frame_rejected=$_.Exception.ToString().Contains('FRAME_SIZE_HOLD')}
finally{$stream.Dispose()}
$result=@{checks=$checks;actual_service_ipc_verified=$false;distinct_bot_account_configured=$false
 protected_install_exists=(Test-Path -LiteralPath 'C:/Program Files/LimitlessRelayTrial')
 model_calls=0;usage_reads=0;normal_active=$normal.ActiveProcesses;timeout_active=$timeout.ActiveProcesses
 cancel_active=$cancelled.ActiveProcesses;cancel_total=$cancelled.TotalProcesses}
$result|ConvertTo-Json -Depth 5|Set-Content -LiteralPath $OutputPath -Encoding utf8
$result|ConvertTo-Json -Depth 5
if(@($checks.Values|Where-Object{-not $_}).Count){exit 2}

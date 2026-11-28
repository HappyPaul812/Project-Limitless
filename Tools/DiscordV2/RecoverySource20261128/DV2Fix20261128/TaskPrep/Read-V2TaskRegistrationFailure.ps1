#Requires -Version 7.0
$ErrorActionPreference='Stop'
$status=(& (Join-Path $PSScriptRoot 'Invoke-V2TaskPrepared.ps1') -Mode Inspect)|ConvertFrom-Json
$code='C:\ProgramData\Limitless\DiscordV2\Code'
$evidence='C:\ProgramData\Limitless\DiscordV2\Evidence'
$trial=Get-Content -LiteralPath (Join-Path $code 'TrialContract.json') -Raw|ConvertFrom-Json
if($trial.TrialId -cnotmatch '^(?!0{32}$)[0-9a-f]{32}$'){throw 'HOLD_REGISTER_DIAGNOSTIC_TRIAL_ID'}
$prefix=Join-Path $evidence ($trial.TrialId+'.admin.')
$intentPath=$prefix+'register-intent'
$registeredPath=$prefix+'registered'
$holds=@(Get-ChildItem -LiteralPath $evidence -Force -File | Where-Object Name -Match (
    '^'+[regex]::Escape($trial.TrialId)+'\.admin\.hold-[0-9a-f]{32}$'))
$operationPath=Join-Path $code ($trial.TrialId+'.register-only.approval.json')
$approvalPresent=Test-Path -LiteralPath $operationPath -PathType Leaf
$operationExpiry=$null
if($approvalPresent){
    $op=Get-Content -LiteralPath $operationPath -Raw|ConvertFrom-Json
    if($op.TrialId -cne $trial.TrialId -or $op.Operation -cne '--register-only'){throw 'HOLD_REGISTER_DIAGNOSTIC_OPERATION_BINDING'}
    $operationExpiry=([DateTimeOffset]$op.ExpiresUtc).ToUniversalTime().ToString('o')
}
[ordered]@{
    WindowsUtc=$status.WindowsUtc
    WorkerDisabled=$status.WorkerDisabled
    TaskPresent=$status.TaskPresent
    Task=$status.Task
    RegisterIntentPresent=(Test-Path -LiteralPath $intentPath -PathType Leaf)
    RegisteredReceiptPresent=(Test-Path -LiteralPath $registeredPath -PathType Leaf)
    HoldRecords=$holds.Count
    RegisterApprovalInstalled=$approvalPresent
    RegisterApprovalExpiresUtc=$operationExpiry
    RegisterApprovalExpired=if($operationExpiry){[DateTimeOffset]::UtcNow -ge [DateTimeOffset]$operationExpiry}else{$null}
    TrialAndTrustLive=$status.TrialAndTrustLive
    RemainingProductProcesses=$status.RemainingProductProcesses
    UnresolvedProcessPaths=$status.UnresolvedProcessPaths
    EffectiveBatchToken='NOT_TESTED'
} | ConvertTo-Json -Depth 5

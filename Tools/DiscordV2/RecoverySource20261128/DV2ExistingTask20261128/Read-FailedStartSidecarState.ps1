#Requires -Version 7.5
$ErrorActionPreference='Stop'
$prep=Join-Path $PSScriptRoot '../DV2Fix20261128/TaskPrep'
$status=& (Join-Path $prep 'Invoke-V2TaskPrepared.ps1') -Mode Inspect | ConvertFrom-Json
$root='C:\ProgramData\Limitless\DiscordV2'
$code=Join-Path $root 'Code'
$pending=Join-Path $root 'Control/Approvals/PendingOperations'
$ledger=Join-Path $root 'Control/DeploymentLedger'
$trial=Get-Content -LiteralPath (Join-Path $code 'TrialContract.json') -Raw|ConvertFrom-Json
if($trial.TrialId -cnotmatch '^[a-f0-9]{32}$'){throw 'HOLD_START_DIAGNOSTIC_TRIAL'}
$names=@(($trial.TrialId+'.start-once.approval.json'),'FakeOnlyExecution.v2.json','FakeOnlyExecution.v2.sig')
$staged=@($names|Where-Object {Test-Path -LiteralPath (Join-Path $pending $_) -PathType Leaf}).Count
$installed=@($names|Where-Object {Test-Path -LiteralPath (Join-Path $code $_) -PathType Leaf}).Count
$consumed=$null;$offset=$null
if(Test-Path -LiteralPath (Join-Path $pending $names[0]) -PathType Leaf){
    $operation=Get-Content -LiteralPath (Join-Path $pending $names[0]) -Raw|ConvertFrom-Json -DateKind String
    if($operation.ApprovalId -cnotmatch '^[a-f0-9]{32}$' -or $operation.Operation -cne '--start-once' -or $operation.TrialId -cne $trial.TrialId){throw 'HOLD_START_DIAGNOSTIC_APPROVAL'}
    $consumed=Test-Path -LiteralPath (Join-Path $ledger ($operation.ApprovalId+'.deploy-consumed')) -PathType Leaf
}
if(Test-Path -LiteralPath (Join-Path $pending $names[1]) -PathType Leaf){
    $envelope=Get-Content -LiteralPath (Join-Path $pending $names[1]) -Raw|ConvertFrom-Json -DateKind String
    $offset=([DateTimeOffset]$envelope.ExpiresUtc).Offset.ToString()
}
[ordered]@{WorkerDisabled=$status.WorkerDisabled;TaskDisabled=$status.Task.Disabled;Instances=$status.Task.Instances
    PendingFiles=$staged;InstalledFiles=$installed;StartApprovalConsumptionPresent=$consumed;SignedExpiryOffset=$offset
    StartIntentPresent=$status.Evidence.'.admin.start-intent';RemainingProductProcesses=$status.RemainingProductProcesses
} | ConvertTo-Json

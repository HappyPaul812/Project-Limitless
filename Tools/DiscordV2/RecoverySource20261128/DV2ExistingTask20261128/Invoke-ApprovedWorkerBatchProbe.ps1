#Requires -Version 7.0
$ErrorActionPreference='Stop'
$prep=Join-Path $PSScriptRoot '../DV2Fix20261128/TaskPrep'
$status=& (Join-Path $prep 'Invoke-V2TaskPrepared.ps1') -Mode Inspect | ConvertFrom-Json
if(!$status.WorkerDisabled -or !$status.TaskPresent -or !$status.Task.Disabled -or $status.Task.TriggerCount -ne 0 -or
    $status.Task.Instances -ne 0 -or $status.RemainingProductProcesses -ne 0 -or $status.UnresolvedProcessPaths -ne 0){
    throw 'HOLD_P_TASK_STATE'
}
& (Join-Path $prep 'Invoke-V2WorkerBatchProbe.ps1') -ProbeAfterExplicitApproval

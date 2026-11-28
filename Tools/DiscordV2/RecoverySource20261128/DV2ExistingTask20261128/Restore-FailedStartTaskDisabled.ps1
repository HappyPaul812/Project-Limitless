#Requires -Version 7.0
$ErrorActionPreference='Stop'
Import-Module (Join-Path $env:windir 'System32/WindowsPowerShell/v1.0/Modules/Microsoft.PowerShell.LocalAccounts/1.0.0.0/Microsoft.PowerShell.LocalAccounts.psd1')
$prep=Join-Path $PSScriptRoot '../DV2Fix20261128/TaskPrep'
$output=@(& (Join-Path $prep 'Read-TaskWorkerBinding.ps1'))
if($output[0] -cne 'PRODUCT_NATIVE_BINDING_LOCAL_VALUES_VERIFIED_ORIGINAL_RECEIPT_UNVERIFIED'){throw 'HOLD_F_RESTORE_BINDING'}
$b=$output[-1]|ConvertFrom-Json
if(!$b.WorkerDisabled -or $b.TaskPath -cnotmatch '^\\[^\\]+$'){throw 'HOLD_F_RESTORE_SCOPE'}
$path=Join-Path $b.CodeRoot 'TrialContract.json'
$raw=[IO.File]::ReadAllBytes($path)
$sha=[Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($raw)).ToLowerInvariant()
$c=[Text.Encoding]::UTF8.GetString($raw)|ConvertFrom-Json
if($c.TrialId -cnotmatch '^[a-f0-9]{32}$' -or $c.Scope.TaskPath -cne $b.TaskPath -or $c.Scope.WorkerSid -cne $b.Worker){throw 'HOLD_F_RESTORE_TRIAL'}
$i=Get-Content -LiteralPath (Join-Path $b.EvidenceRoot ($c.TrialId+'.admin.start-intent')) -Raw|ConvertFrom-Json
if($i.Status -cne 'START_CONSUMED' -or $i.ContractSha -cne $sha -or $i.TrialId -cne $c.TrialId){throw 'HOLD_F_RESTORE_INTENT'}
$svc=New-Object -ComObject Schedule.Service
$svc.Connect()
$folder=$svc.GetFolder('\')
$task=$folder.GetTask($b.TaskPath.Substring(1))
$d=$task.Definition
$user=[string]$d.Principal.UserId
if($user -notlike 'S-1-*'){$user=([Security.Principal.NTAccount]::new($user)).Translate([Security.Principal.SecurityIdentifier]).Value}
if($task.Path -cne $b.TaskPath -or $d.Triggers.Count -ne 0 -or $task.GetInstances(0).Count -ne 0 -or
    $d.Actions.Count -ne 1 -or $user -cne $b.Worker -or $d.Principal.LogonType -ne 1 -or $d.Principal.RunLevel -ne 0){
    throw 'HOLD_F_RESTORE_TASK_IDENTITY'
}
$action=$d.Actions.Item(1)
if($action.Path -cne (Join-Path $b.CodeRoot 'Supervisor.exe') -or $action.WorkingDirectory -cne $b.CodeRoot -or
    $action.Arguments -cnotmatch ('^'+[regex]::Escape('--supervise --contract "'+$path+'" --contract-sha ')+'[a-f0-9]{64}$')){
    throw 'HOLD_F_RESTORE_ACTION'
}
if($task.Enabled){$task.Enabled=$false}
$after=$folder.GetTask($b.TaskPath.Substring(1))
if($after.Enabled -or $after.GetInstances(0).Count -ne 0 -or (Get-LocalUser -SID ([Security.Principal.SecurityIdentifier]::new($b.Worker))).Enabled){
    throw 'HOLD_F_RESTORE_READBACK'
}
'F_TASK_AND_WORKER_DISABLED_READBACK_NO_START'

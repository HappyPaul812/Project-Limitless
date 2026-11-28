#Requires -Version 7.0
[CmdletBinding()]
param([ValidateSet('Inspect','Register','Run')][string]$Mode='Inspect',[switch]$RegisterWithTemporaryWorkerActivation,[switch]$UseExistingTaskRecovery)
$ErrorActionPreference='Stop'
Import-Module -Name (Join-Path $env:windir 'System32/WindowsPowerShell/v1.0/Modules/Microsoft.PowerShell.LocalAccounts/1.0.0.0/Microsoft.PowerShell.LocalAccounts.psd1') -ErrorAction Stop
$root='C:\ProgramData\Limitless\DiscordV2'
$code=Join-Path $root 'Code'
$evidence=Join-Path $root 'Evidence'
$control=Join-Path $root 'Control'
$tool=Join-Path $control 'DeploymentTool/Limitless.DeploymentAdmin.exe'
$supervisor=Join-Path $code 'Supervisor.exe'
$trialPath=Join-Path $code 'TrialContract.json'
$actor=[Security.Principal.WindowsIdentity]::GetCurrent()
try {
    $approved=[Security.Principal.NTAccount]::new([Environment]::MachineName,'windows').Translate([Security.Principal.SecurityIdentifier])
    if (!$actor.User.Equals($approved) -or !([Security.Principal.WindowsPrincipal]::new($actor)).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)) {throw 'HOLD_ADMIN_TOKEN'}
} finally {$actor.Dispose()}
$bound=@(& (Join-Path $PSScriptRoot 'Read-TaskWorkerBinding.ps1'))
if ($bound[0] -cne 'PRODUCT_NATIVE_BINDING_LOCAL_VALUES_VERIFIED_ORIGINAL_RECEIPT_UNVERIFIED') {throw 'HOLD_BINDING'}
$binding=$bound[-1] | ConvertFrom-Json
$expiry=[DateTimeOffset]::MinValue
$trustUntil=[DateTimeOffset]::MinValue
$trialRaw=[IO.File]::ReadAllBytes($trialPath)
$trial=[Text.Encoding]::UTF8.GetString($trialRaw) | ConvertFrom-Json
$sha=[Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($trialRaw)).ToLowerInvariant()
if (!$trial.Approved -or $trial.TrialId -cnotmatch '^[0-9a-f]{32}$' -or $trial.Scope.WorkerSid -cne $binding.Worker -or
    $trial.Scope.TaskPath -cne $binding.TaskPath -or $trial.Scope.CodeRoot -cne $code -or $trial.Scope.EvidenceRoot -cne $evidence -or
    $trial.Scope.Mode -cne 'FAKE_ONLY' -or !$trial.Scope.NoAi -or !$trial.Scope.NoIpc -or $trial.Scope.JobSeconds -ne 90 -or
    $trial.Scope.SupervisorCount -ne 1 -or $trial.Scope.ProbeCount -ne 1) {throw 'HOLD_FAKE_ONLY_TRIAL_SCOPE'}
$trustFile=Join-Path $code ($trial.TrialId+'.fake-only-trust.json')
$trustRaw=[IO.File]::ReadAllBytes($trustFile)
if ([Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($trustRaw)).ToLowerInvariant() -cne $trial.FakeOnlyIsolation.TrustEvidenceSha256) {throw 'HOLD_TRUST_HASH'}
$trust=[Text.Encoding]::UTF8.GetString($trustRaw) | ConvertFrom-Json
$expiry=[DateTimeOffset]$trial.ExpiresUtc
$observed=[DateTimeOffset]$trust.ObservedUtc
$trustUntil=[DateTimeOffset]$trust.ExpiresUtc
if ($observed.AddMinutes(15) -lt $trustUntil) {$trustUntil=$observed.AddMinutes(15)}
$workerSid=[Security.Principal.SecurityIdentifier]::new($binding.Worker)
$worker=Get-LocalUser -SID $workerSid
$service=New-Object -ComObject Schedule.Service
$service.Connect()
if ($binding.TaskPath -cnotmatch '^\\[^\\]+$') {throw 'HOLD_ROOT_TASK_NAME'}
$folder=$service.GetFolder('\')
$task=$null
try {$task=$folder.GetTask($binding.TaskPath.Substring(1))}
catch {
    $missing=$false
    for($e=$_.Exception;$e;$e=$e.InnerException){if($e.HResult -eq -2147024894){$missing=$true}}
    if (!$missing){throw}
}
$now=[DateTimeOffset]::UtcNow
$live=$now -lt $expiry -and $observed -le $now -and $now -lt $trustUntil
$recovery=$null
$taskArgumentSha=$sha
if($UseExistingTaskRecovery){
    if($Mode -eq 'Register' -or !$task -or $worker.Enabled){throw 'HOLD_EXISTING_TASK_RECOVERY_WRAPPER_STATE'}
    $recoveryReader='F:\study\codex\Project-Limitless\Temp\DV2ExistingTask20261128\Migration\Read-ExistingTaskRecoveryReady.ps1'
    if((Get-FileHash -LiteralPath $recoveryReader).Hash -cne '2D0B7B7ED7F11C208B783D13758907399AC22C6A8C27BD1139618654429C8023'){throw 'HOLD_EXISTING_TASK_RECOVERY_READER_PIN'}
    $recovery=& $recoveryReader
    if(!$recovery.RecoveryReady -or $recovery.CurrentTrialSha -cne $sha){throw 'HOLD_EXISTING_TASK_RECOVERY_WRAPPER_BINDING'}
    $taskArgumentSha=$recovery.PriorTrialSha
}
$registered=$null
if($null -ne $task) {
    $definition=$task.Definition
    $action=$definition.Actions
    $actualIdentity=$definition.Principal.UserId
    if($actualIdentity -notmatch '^S-1-') {$actualIdentity=([Security.Principal.NTAccount]::new($actualIdentity)).Translate([Security.Principal.SecurityIdentifier]).Value}
    $duration=[string]$definition.Settings.ExecutionTimeLimit
    $limit90=$false
    if($duration.StartsWith('PT',[StringComparison]::Ordinal)){
        try{$limit90=[Xml.XmlConvert]::ToTimeSpan($duration) -eq [TimeSpan]::FromSeconds(90)}
        catch [FormatException], [OverflowException] {}
    }
    $registered=[ordered]@{
        Disabled=!$task.Enabled;TriggerCount=$definition.Triggers.Count;LeastPrivilege=($definition.Principal.RunLevel -eq 0)
        PasswordLogon=($definition.Principal.LogonType -eq 1);Limit90Seconds=$limit90
        ExecutionTimeLimitRaw=$duration
        WorkerSidMatch=($actualIdentity -ceq $binding.Worker);Instances=$task.GetInstances(0).Count
        ProtectedActionMatch=($action.Count -eq 1 -and $action.Item(1).Path -ceq $supervisor -and
            $action.Item(1).WorkingDirectory -ceq $code -and
            $action.Item(1).Arguments -ceq ('--supervise --contract "'+$trialPath+'" --contract-sha '+$taskArgumentSha))
    }
}
if ($Mode -eq 'Inspect') {
    $names=@('.admin.registered','.admin.start-intent','.admin.instance','.created-once','.generation','.exclusivity','.final')
    $evidenceStatus=[ordered]@{}
    foreach($name in $names){$evidenceStatus[$name]=Test-Path -LiteralPath (Join-Path $evidence ($trial.TrialId+$name)) -PathType Leaf}
    $final=Join-Path $evidence ($trial.TrialId+'.final')
    $completion=if($evidenceStatus['.final']){
        if([IO.File]::ReadAllText($final) -ceq ('TOKEN_PROBE_ONLY_COMPLETE'+[char]10)){'SUPERVISOR_PROVED_PROBE_EXIT_125_AND_JOB_DRAIN'}else{'HOLD_FINAL_EVIDENCE'}
    }else{'NO_FINAL_EVIDENCE'}
    $liveProcesses=@(Get-CimInstance Win32_Process -Filter "Name='Supervisor.exe' OR Name='Probe.exe'")
    $matching=@($liveProcesses | Where-Object {$_.ExecutablePath -ieq $supervisor -or $_.ExecutablePath -ieq (Join-Path $code 'Probe.exe')}).Count
    $unresolved=@($liveProcesses | Where-Object {!$_.ExecutablePath}).Count
    [ordered]@{WindowsUtc=$now.ToString('o');TrialExpiresUtc=$expiry.ToUniversalTime().ToString('o');TrustUntilUtc=$trustUntil.ToUniversalTime().ToString('o')
        TrialAndTrustLive=$live;WorkerDisabled=!$worker.Enabled;TaskPresent=($null -ne $task);Task=$registered
        SchedulerLastTaskResult=if($task){$task.LastTaskResult}else{$null}
        ExistingTaskRecoveryVerified=($null -ne $recovery);ExistingTaskRecoveryExpiresUtc=if($recovery){$recovery.ExpiresUtc}else{$null}
        Evidence=$evidenceStatus;Completion=$completion;RemainingProductProcesses=$matching;UnresolvedProcessPaths=$unresolved
        EffectiveBatchToken='NOT_TESTED_BY_THIS_COMMAND'} | ConvertTo-Json -Depth 6
    return
}
if (!$live) {throw 'HOLD_TRIAL_OR_TRUST_EXPIRED'}
if ($Mode -eq 'Register') {
    if (!$RegisterWithTemporaryWorkerActivation){throw 'HOLD_TEMPORARY_WORKER_ACTIVATION_APPROVAL_REQUIRED'}
    if ($worker.Enabled -or $task) {throw 'HOLD_REGISTER_NEEDS_DISABLED_WORKER_AND_ABSENT_TASK'}
    if ((Read-Host '별도 일시 활성화·등록·비활성 복귀 승인 후 REGISTER-V2-TASK-AND-RESTORE-WORKER 입력') -cne 'REGISTER-V2-TASK-AND-RESTORE-WORKER'){
        throw 'HOLD_REGISTER_APPROVAL'
    }
    $signingManifest='C:\ProgramData\LimitlessDiscordV2SupervisorActivationSigning20261128\ProductSigningManifest.json'
    $signing=Get-Content -LiteralPath $signingManifest -Raw | ConvertFrom-Json
    $signedRows=@($signing.Results)
    if($signedRows.Count -ne 1 -or $signedRows[0].Name -cne 'Supervisor.exe' -or $signedRows[0].Status -cne 'PASS' -or
        $signedRows[0].SignedSha256 -cne $trial.SupervisorSha256 -or
        (Get-FileHash -LiteralPath $supervisor).Hash.ToLowerInvariant() -cne $trial.SupervisorSha256){
        throw 'HOLD_REGISTER_ACTIVATION_SUPERVISOR_NOT_DEPLOYED'
    }
    & (Join-Path $PSScriptRoot 'Test-V2ArchivedEvidenceWriteBoundary.ps1')
    & $tool --install-operation --register-only
    if ($LASTEXITCODE -ne 0){throw 'HOLD_REGISTER_SIDECARS_NO_RETRY'}
    $approvalPath=Join-Path $code ($trial.TrialId+'.register-only.approval.json')
    $approval=Get-Content -LiteralPath $approvalPath -Raw | ConvertFrom-Json
    if($approval.TrialId -cne $trial.TrialId -or $approval.Operation -cne '--register-only' -or
        $approval.ContractSha -cne $sha){throw 'HOLD_REGISTER_APPROVAL_BINDING'}
    $now=[DateTimeOffset]::UtcNow
    $deadline=@($now.AddSeconds(90),$expiry,$trustUntil,[DateTimeOffset]$approval.ExpiresUtc) | Sort-Object | Select-Object -First 1
    if($deadline -le $now.AddSeconds(15)){throw 'HOLD_REGISTER_INSUFFICIENT_TIME'}
    if((Get-LocalUser -SID $workerSid).Enabled){throw 'HOLD_WORKER_STATE_CHANGED'}
    Write-Host 'Supervisor 안내가 뜨면 기존 Worker 암호를 직접 입력하고 Enter를 누르세요. 입력마다 -가 표시됩니다.'
    try {
        Enable-LocalUser -SID $workerSid
        if(!(Get-LocalUser -SID $workerSid).Enabled){throw 'HOLD_WORKER_ENABLE_READBACK'}
        $registerExit = & (Join-Path $PSScriptRoot 'Write-V2RegistrationDiagnostic.ps1') -Supervisor $supervisor -TrialPath $trialPath -TrialSha $sha -Control $control -Deadline $deadline
        if ($registerExit -ne 0){throw 'HOLD_REGISTER_NO_RETRY'}
    } finally {
        if((Get-LocalUser -SID $workerSid).Enabled){Disable-LocalUser -SID $workerSid}
        if((Get-LocalUser -SID $workerSid).Enabled){throw 'HOLD_WORKER_DISABLE_READBACK'}
        'WORKER_DISABLED_READBACK'
    }
    'REGISTER_COMMAND_EXIT_0_VERIFY_DISABLED_TASK'
    return
}
if ($worker.Enabled -or !$task -or !$registered.Disabled -or $registered.TriggerCount -ne 0 -or
    !$registered.LeastPrivilege -or !$registered.PasswordLogon -or !$registered.Limit90Seconds -or
    !$registered.WorkerSidMatch -or !$registered.ProtectedActionMatch -or $registered.Instances -ne 0) {throw 'HOLD_START_PREREQUISITES'}
if ((Read-Host 'Token PASS와 별도 활성화·시작·원복 승인 후 START-ONE-FAKE-ONLY 입력') -cne 'START-ONE-FAKE-ONLY'){throw 'HOLD_START_APPROVAL'}
if($UseExistingTaskRecovery){
    $latest=& $recoveryReader
    if(!$latest.RecoveryReady -or $latest.CurrentTrialSha -cne $sha -or $latest.PriorTrialSha -cne $taskArgumentSha -or
        [DateTimeOffset]$latest.ExpiresUtc -le [DateTimeOffset]::UtcNow.AddSeconds(100)){
        throw 'HOLD_EXISTING_TASK_RECOVERY_TIME_OR_BINDING'
    }
}
if (([DateTimeOffset]::UtcNow.AddSeconds(100)) -ge $expiry -or ([DateTimeOffset]::UtcNow.AddSeconds(100)) -ge $trustUntil) {
    throw 'HOLD_INSUFFICIENT_TRIAL_TIME_FOR_RUN'
}
try {
    & $tool --install-operation --start-once
    if ($LASTEXITCODE -ne 0){throw 'HOLD_START_SIDECARS_NO_RETRY'}
    & (Join-Path $PSScriptRoot 'Test-V2ArchivedEvidenceWriteBoundary.ps1')
    Enable-LocalUser -SID $workerSid
    if (!(Get-LocalUser -SID $workerSid).Enabled){throw 'HOLD_WORKER_ENABLE_READBACK'}
    & $supervisor --start-once --contract $trialPath --contract-sha $sha
    if ($LASTEXITCODE -ne 0){throw 'HOLD_START_NO_RETRY'}
    'START_CONTROLLER_EXIT_0_CHECK_SUPERVISOR_EVIDENCE'
    $watch=[Diagnostics.Stopwatch]::StartNew()
    $finalPath=Join-Path $evidence ($trial.TrialId+'.final')
    do {
        $nowTask=$folder.GetTask($binding.TaskPath.Substring(1))
        if ($nowTask.Enabled) {throw 'HOLD_TASK_DISABLE_NOT_OBSERVED'}
        if ($nowTask.GetInstances(0).Count -eq 0 -and (Test-Path -LiteralPath $finalPath -PathType Leaf)) {break}
        Start-Sleep -Milliseconds 250
    } while($watch.Elapsed.TotalSeconds -lt 92)
    if ($nowTask.GetInstances(0).Count -ne 0 -or !(Test-Path -LiteralPath $finalPath -PathType Leaf)) {
        throw 'HOLD_EXECUTION_DRAIN_OR_FINAL_EVIDENCE'
    }
    if ($nowTask.LastTaskResult -ne 0 -or [IO.File]::ReadAllText($finalPath) -cne ('TOKEN_PROBE_ONLY_COMPLETE'+[char]10)) {
        throw 'HOLD_EXECUTION_RESULT'
    }
    'SUPERVISOR_EXIT_0_PROBE_EXIT_125_AND_JOB_DRAIN_EVIDENCE_VERIFIED'
} finally {
    # 검증된 기존 Worker만 비활성으로 되돌립니다.
    if ((Get-LocalUser -SID $workerSid).Enabled) { Disable-LocalUser -SID $workerSid }
    if ((Get-LocalUser -SID $workerSid).Enabled) {throw 'HOLD_WORKER_DISABLE_READBACK'}
    'WORKER_DISABLED_READBACK'
}

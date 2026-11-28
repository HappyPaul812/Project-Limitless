#Requires -Version 7.0
param([Parameter(Mandatory)][ValidateSet('Register','Start')][string]$Operation,[switch]$IssueAfterExplicitApproval,[switch]$UseExistingTaskRecovery)
$ErrorActionPreference='Stop'
$actor=[Security.Principal.WindowsIdentity]::GetCurrent()
try {
    $expected=[Security.Principal.NTAccount]::new([Environment]::MachineName,'windows').Translate([Security.Principal.SecurityIdentifier])
    if (!$actor.User.Equals($expected) -or !([Security.Principal.WindowsPrincipal]::new($actor)).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)) {throw 'HOLD_ADMIN_TOKEN'}
    $sid=$actor.User.Value
} finally {$actor.Dispose()}
$ready=(& (Join-Path $PSScriptRoot 'Invoke-V2TaskPrepared.ps1') -Mode Inspect -UseExistingTaskRecovery:$UseExistingTaskRecovery) | ConvertFrom-Json
if (!$ready.TrialAndTrustLive -or !$ready.WorkerDisabled -or $ready.UnresolvedProcessPaths -ne 0 -or $ready.RemainingProductProcesses -ne 0) {throw 'HOLD_OPERATION_READINESS'}
if($Operation -eq 'Register' -and $ready.TaskPresent){throw 'HOLD_TASK_ALREADY_PRESENT'}
if($Operation -eq 'Start' -and (!$ready.TaskPresent -or !$ready.Task.Disabled -or $ready.Task.Instances -ne 0 -or
    $ready.Task.TriggerCount -ne 0 -or !$ready.Task.LeastPrivilege -or !$ready.Task.Limit90Seconds -or !$ready.Task.PasswordLogon -or
    !$ready.Task.WorkerSidMatch -or !$ready.Task.ProtectedActionMatch -or (!$ready.Evidence.'.admin.registered' -and !$ready.ExistingTaskRecoveryVerified) -or
    $ready.Evidence.'.admin.start-intent')){throw 'HOLD_START_TASK_PROOF'}
if($UseExistingTaskRecovery -and ($Operation -ne 'Start' -or !$ready.ExistingTaskRecoveryVerified)){
    throw 'HOLD_EXISTING_TASK_RECOVERY_ISSUE_STATE'
}
$deadline=[DateTimeOffset]$ready.TrialExpiresUtc
$trust=[DateTimeOffset]$ready.TrustUntilUtc
if($trust -lt $deadline){$deadline=$trust}
if(!$IssueAfterExplicitApproval){'OPERATION_SIDECAR_READINESS_NO_SIGNING_NO_WRITES';return}
if((Read-Host '별도 해당 동작 발급 승인 후 ISSUE-V2-OPERATION-SIDECARS 입력') -cne 'ISSUE-V2-OPERATION-SIDECARS'){
    throw 'HOLD_OPERATION_ISSUE_APPROVAL'
}
$code='C:\ProgramData\Limitless\DiscordV2\Code'
$approvalRoot='C:\ProgramData\Limitless\DiscordV2\Control\Approvals'
$requestName=Join-Path $approvalRoot 'ProductDeploymentRequest.v2.json'
$cert=Get-Item -LiteralPath 'Cert:\LocalMachine\My\437423567AD4B703578F231E34A06592B38DB4EB'
try {
    $public=[Security.Cryptography.X509Certificates.RSACertificateExtensions]::GetRSAPublicKey($cert)
    $key=[Security.Cryptography.X509Certificates.RSACertificateExtensions]::GetRSAPrivateKey($cert)
    try {
        function Sha([byte[]]$bytes){[Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($bytes)).ToLowerInvariant()}
        $sha=[Security.Cryptography.HashAlgorithmName]::SHA256
        $pss=[Security.Cryptography.RSASignaturePadding]::Pss
        $utf8=[Text.UTF8Encoding]::new($false,$true)
        if(!$key -or !$public -or (Sha $cert.RawData) -cne '7dba5310d6ded0035595e5e7ab8b8b73422f2925b49c633426995c322940672c' -or
            (Sha ($public.ExportSubjectPublicKeyInfo())) -cne '2a6d0a7d31e7642df209b6cbe12fa628500663f540eb6bd557cac2aca2d31f7c' -or
            (Get-Date) -lt $cert.NotBefore -or (Get-Date) -ge $cert.NotAfter){throw 'HOLD_OPERATION_ISSUER_PIN'}
        $renewalLoader=Join-Path $PSScriptRoot '../TrialRenewal/Load-TrialRenewalTools.ps1'
        if((Get-FileHash -LiteralPath $renewalLoader).Hash -cne 'B8C1F647BC5DC3A9DE40D357B2335094881CC5BA73F60F66BB9BDFBB3DF5432D'){throw 'HOLD_RENEWAL_LOADER_SHA'}
        . $renewalLoader
        $renewalSession=Open-ApprovedTrialRenewalStore
        try {
            if(!$renewalSession.Binding.WorkerDisabled){throw 'HOLD_OPERATION_WORKER_ENABLED'}
            $head=Resolve-ProtectedTrialHead $renewalSession.Store $renewalSession.Binding $public $trialRenewalSpki ([DateTimeOffset]::UtcNow)
        } finally {$renewalSession.Store.Dispose()}
        $trialSha=$head.TrialSha
        $trial=$head.Trial
        $deadline=Read-TrialRenewalUtc $head.Trial.ExpiresUtc
        $trustDeadline=Read-TrialRenewalUtc $head.Trust.ExpiresUtc
        $observedDeadline=(Read-TrialRenewalUtc $head.Trust.ObservedUtc).AddMinutes(15)
        if($trustDeadline -lt $deadline){$deadline=$trustDeadline}
        if($observedDeadline -lt $deadline){$deadline=$observedDeadline}
        if($UseExistingTaskRecovery){
            $liveRecovery=(& (Join-Path $PSScriptRoot 'Invoke-V2TaskPrepared.ps1') -Mode Inspect -UseExistingTaskRecovery) | ConvertFrom-Json
            if(!$liveRecovery.ExistingTaskRecoveryVerified -or !$liveRecovery.Task.ProtectedActionMatch){
                throw 'HOLD_EXISTING_TASK_RECOVERY_ISSUE_STATE'
            }
            $recoveryEnd=([DateTimeOffset]$liveRecovery.ExistingTaskRecoveryExpiresUtc).ToUniversalTime()
            if($recoveryEnd -lt $deadline){$deadline=$recoveryEnd}
        }
        $request=[ordered]@{ToolSha256=''}
        $mode=if($Operation -eq 'Register'){'--register-only'}else{'--start-once'}
        $base=if($Operation -eq 'Register'){'FakeOnlyExecution.v2.register-only'}else{'FakeOnlyExecution.v2'}
        $names=@(($trial.TrialId+'.'+$mode.Substring(2)+'.approval.json'),($base+'.sig'),($base+'.json'))
        $store=Join-Path $PSScriptRoot 'OperationStore.cs'
        $adapter=Join-Path $PSScriptRoot 'OperationPreparation.cs'
        if((Get-FileHash -LiteralPath $store).Hash -cne 'E6781DF18E53B8DF9F7D285EF7C977E602658B2B74488E92316C04EBBA2FA6F5' -or (Get-FileHash -LiteralPath $adapter).Hash -cne '04105BB6E7ECC35FF0827C38037A7BED46E6A20760981ACB427C9612CEB53D26'){throw 'HOLD_OPERATION_ADAPTER_SHA'}
        Add-Type -Path $store,$adapter -CompilerOptions '/define:INSTALLER_SIGNING'
        $space=[LimitlessV2OperationIssuer20261128+ProductDeploymentPreparation]::new($sid,$request.ToolSha256)
        try {
            $now=[DateTimeOffset]::UtcNow
            $expires=$now.AddMinutes(5)
            if($deadline -lt $expires){$expires=$deadline}
            $expires=$expires.ToUniversalTime()
            if(($expires-$now).TotalSeconds -lt 110 -or $cert.NotAfter.ToUniversalTime() -le $expires.UtcDateTime){throw 'HOLD_OPERATION_TIME_INSUFFICIENT'}
            $operationJson=$utf8.GetBytes(([ordered]@{
                Approved=$true;ApprovalId=[Guid]::NewGuid().ToString('N');TrialId=$trial.TrialId;ContractSha=$trialSha
                Operation=$mode;ExpiresUtc=$expires.ToString('o')
            }|ConvertTo-Json -Compress))
            $contractJson=$utf8.GetBytes(([ordered]@{
                Profile='DISCORD_V2_FAKE_ONLY_EXECUTION_V2';Approved=$true
                WorkerSid=$trial.Scope.WorkerSid;TaskPath=$trial.Scope.TaskPath;CodeRoot=$trial.Scope.CodeRoot;EvidenceRoot=$trial.Scope.EvidenceRoot
                SupervisorSha256=$trial.SupervisorSha256;ProbeSha256=$trial.ProbeSha256;ImageSignerSpkiSha256='2a6d0a7d31e7642df209b6cbe12fa628500663f540eb6bd557cac2aca2d31f7c'
                NotBeforeUtc=$now.ToString('o');ExpiresUtc=$expires.ToString('o')
                Mode='FAKE_ONLY';NoAi=$true;NoIpc=$true;JobSeconds=90;SupervisorCount=1;ProbeCount=1
                TrialContractSha256=$trialSha;OperationApprovalSha256=(Sha $operationJson)
            }|ConvertTo-Json -Compress))
            $serializedContract=Read-TrialRenewalJson $contractJson
            $serializedOperation=Read-TrialRenewalJson $operationJson
            $signedStart=Read-TrialRenewalUtc $serializedContract.NotBeforeUtc
            $signedEnd=Read-TrialRenewalUtc $serializedContract.ExpiresUtc
            $signedOperationEnd=Read-TrialRenewalUtc $serializedOperation.ExpiresUtc
            if($signedStart -ne $now -or $signedEnd -ne $signedOperationEnd -or $signedEnd -gt $deadline -or
                ($signedEnd-[DateTimeOffset]::UtcNow).TotalSeconds -lt 110 -or ($signedEnd-$signedStart).TotalMinutes -gt 5){
                throw 'HOLD_OPERATION_SERIALIZED_TIME_BOUNDARY'
            }
            $signature=$key.SignData($contractJson,$sha,$pss)
            if(!$public.VerifyData($contractJson,$signature,$sha,$pss)){throw 'HOLD_OPERATION_SIGNATURE_VERIFY'}
            $space.PrepareOperationsDirectory()
            foreach($name in $names){$space.RequireApprovalAbsent($name)}
            $space.WriteApproval($names[0],$operationJson,$expires)
            $space.WriteApproval($names[1],$signature,$expires)
            $space.WriteApproval($names[2],$contractJson,$expires)
            'OPERATION_SIDECARS_ISSUED_PENDING_NO_TASK_CHANGE'
            "OPERATION_EXPIRES_UTC=$($expires.ToString('o'))"
        } finally {$space.Dispose()}
    } finally {if($key){$key.Dispose()};if($public){$public.Dispose()}}
} finally {$cert.Dispose()}

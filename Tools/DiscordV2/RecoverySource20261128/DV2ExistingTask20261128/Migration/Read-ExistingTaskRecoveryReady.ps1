#Requires -Version 7.5
$ErrorActionPreference='Stop'
. (Join-Path $PSScriptRoot 'Load-TrialRenewalTools.ps1')
. (Join-Path $PSScriptRoot 'ExistingTaskRecoverySignature.ps1')
$session=Open-ApprovedTrialRenewalStore
try {
    if(!$session.Binding.WorkerDisabled){throw 'HOLD_EXISTING_TASK_RECOVERY_WORKER_ENABLED'}
    $cert=Get-Item -LiteralPath "Cert:\LocalMachine\Root\$trialRenewalThumb"
    try {
        if((Get-TrialRenewalSha $cert.RawData) -cne $trialRenewalDer){throw 'HOLD_EXISTING_TASK_RECOVERY_CERT'}
        $rsa=[Security.Cryptography.X509Certificates.RSACertificateExtensions]::GetRSAPublicKey($cert)
        try {
            if(!$rsa){throw 'HOLD_EXISTING_TASK_RECOVERY_KEY'}
            $now=[DateTimeOffset]::UtcNow
            $store=$session.Store
            $b=$session.Binding
            $head=Resolve-ProtectedTrialHead $store $b $rsa $trialRenewalSpki $now
            $raw=Read-TrialRenewalStream ($store.ReadExistingTaskRecovery('ExistingTaskRecovery.v1.json')) 8192
            $sig=Read-TrialRenewalStream ($store.ReadExistingTaskRecovery('ExistingTaskRecovery.v1.sig')) 4096
            $r=Read-TrialRenewalJson $raw
            # 서명 및 엄격한 형식을 먼저 확인한 다음에만 이전 SHA를 읽기 전용 검사에 넘깁니다.
            $null=Assert-ExistingTaskRecoverySignature $raw $sig $rsa $trialRenewalSpki $r $now
            if($r.PriorTrialSha256 -cnotmatch '^[0-9a-f]{64}$'){throw 'HOLD_EXISTING_TASK_RECOVERY_PRIOR_SHA'}
            $observed=[ordered]@{}
            foreach($name in $r.Keys){$observed[$name]=$r[$name]}
            $observed.TrialId=$head.Identity.TrialId
            $observed.NextTrialSha256=$head.TrialSha
            $observed.NextSupervisorSha256=$head.Identity.SupervisorSha256
            $observed.ProbeSha256=$head.Identity.ProbeSha256
            $observed.WorkerSid=$b.Worker; $observed.TaskPath=$b.TaskPath
            $observed.CodeRoot=$b.CodeRoot; $observed.EvidenceRoot=$b.EvidenceRoot
            $observed.NextTrialExpiresUtc=$head.Trial.ExpiresUtc
            $priorImage=Read-TrialRenewalStream ($store.ReadCase($r.PriorTrialSha256,'Prior-Supervisor.exe')) 16777216
            $observed.PriorSupervisorSha256=Get-TrialRenewalSha $priorImage
            $old=Read-TrialRenewalStream ($store.ReadCase($r.PriorTrialSha256,'Prior-TrialContract.json')) 4096
            $observed.PriorTrialSha256=Get-TrialRenewalSha $old
            $record=Read-TrialRenewalJson (Read-TrialRenewalStream ($store.ReadCase($r.PriorTrialSha256,'FailedRegistrationEvidence.json')))
            if($record.TrialSha256 -cne $observed.PriorTrialSha256){throw 'HOLD_EXISTING_TASK_RECOVERY_ARCHIVE_BINDING'}
            $issuerIntent=@($record.Files|Where-Object {$_.Area -ceq 'Evidence' -and $_.Name -ceq ($r.TrialId+'.admin.register-intent')})
            $issuerHold=@($record.Files|Where-Object {$_.Area -ceq 'Evidence' -and $_.Name -cmatch ('^'+$r.TrialId+'\.admin\.hold-[a-f0-9]{32}$')})
            if($issuerIntent.Count -ne 1 -or $issuerHold.Count -ne 1){throw 'HOLD_EXISTING_TASK_RECOVERY_ARCHIVE_FILES'}
            $observed.RegisterIntentSha256=$issuerIntent[0].Sha256
            $observed.HoldEvidenceSha256=$issuerHold[0].Sha256
            $observed.PriorApprovalId=$record.ApprovalId
            $observed.PriorConsumptionSha256=Get-TrialRenewalSha (Read-TrialRenewalStream ($store.ReadOriginalConsumption($record.ApprovalId)) 512)
            $observed.RegistrationDiagnosticSha256=Get-TrialRenewalSha (Read-TrialRenewalStream ($store.ReadRegistrationDiagnostic()) 16384)
            $fingerprint=@(& (Join-Path $PSScriptRoot '../Inspection/Read-ExistingTaskProductFingerprint.ps1') -PriorTaskArgumentSha $r.PriorTrialSha256 |
                Where-Object {$_ -cmatch '^TASK_FINGERPRINT_SHA256=[a-f0-9]{64}$'})
            if($fingerprint.Count -ne 1){throw 'HOLD_EXISTING_TASK_RECOVERY_FINGERPRINT_UNAVAILABLE'}
            $observed.TaskFingerprintSha256=$fingerprint[0].Substring('TASK_FINGERPRINT_SHA256='.Length)
            $null=Assert-ExistingTaskRecoverySignature $raw $sig $rsa $trialRenewalSpki $observed ([DateTimeOffset]::UtcNow)
            [pscustomobject]@{RecoveryReady=$true;PriorTrialSha=$r.PriorTrialSha256;CurrentTrialSha=$head.TrialSha;ExpiresUtc=$r.ExpiresUtc}
        } finally {if($rsa){$rsa.Dispose()}}
    } finally {$cert.Dispose()}
} finally {$session.Store.Dispose()}

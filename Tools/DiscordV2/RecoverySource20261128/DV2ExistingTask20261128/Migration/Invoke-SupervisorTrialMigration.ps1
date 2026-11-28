#Requires -Version 7.5
param([switch]$MigrateAfterExplicitApproval)
$ErrorActionPreference='Stop'
$hostPath='C:\Program Files\PowerShell\7\pwsh.exe'
if (![Environment]::Is64BitProcess -or (Get-Process -Id $PID).Path -ine $hostPath){throw 'HOLD_MIGRATION_HOST'}
$sig=Get-AuthenticodeSignature -LiteralPath $hostPath
if($sig.Status -ne 'Valid' -or $sig.SignerCertificate.Subject -notmatch '(^|,\s*)O=Microsoft Corporation(,|$)'){throw 'HOLD_MIGRATION_HOST_SIGNER'}
$loader=Join-Path $PSScriptRoot 'Load-TrialRenewalTools.ps1'
if((Get-FileHash -LiteralPath $loader).Hash -cne 'E4A45081C45E05CDDACAA6BF8A52C4B0771444D8B693CEE8B88BCD10CB949B03'){throw 'HOLD_MIGRATION_LOADER_SHA'}
. $loader
$buildPath=Join-Path $PSScriptRoot '../CandidateEvidence.json'
if((Get-FileHash -LiteralPath $buildPath).Hash -cne 'A9957496150F16EE589ED0459F9F0A9171A1CAF8E86C1C39472D209588E2A127'){throw 'HOLD_MIGRATION_BUILD_MANIFEST_SHA'}
$build=Get-Content -LiteralPath $buildPath -Raw|ConvertFrom-Json
$taskInspect='F:\study\codex\Project-Limitless\Temp\DV2Fix20261128\TaskPrep\Invoke-V2TaskPrepared.ps1'
$approvedFingerprint='38808125a5b7df12165cb5565999c03ea6ca117d48cadccd47502b9c9cd84cc1'
$fingerprintInspector=Join-Path $PSScriptRoot '../Inspection/Read-ExistingTaskProductFingerprint.ps1'
$recoveryVerifier=Join-Path $PSScriptRoot 'ExistingTaskRecoverySignature.ps1'
if((Get-FileHash -LiteralPath $recoveryVerifier).Hash -cne 'A0FB36184A297FDF35A43C0921F5CFF22D22F6DA075D4826AE32BD64CAD8E160'){throw 'HOLD_RECOVERY_VALIDATOR_PIN'}
. $recoveryVerifier
function Assert-ExistingTaskFingerprint([string]$priorSha) {
    $report=if($priorSha){@(& $fingerprintInspector -PriorTaskArgumentSha $priorSha)}else{@(& $fingerprintInspector)}
    if($report -cnotcontains "TASK_FINGERPRINT_SHA256=$approvedFingerprint"){throw 'HOLD_EXISTING_TASK_FINGERPRINT_CHANGED'}
}
function Assert-MigrationIdle {
    $state=(& $taskInspect -Mode Inspect)|ConvertFrom-Json
    if(!$state.TaskPresent -or !$state.WorkerDisabled -or $state.RemainingProductProcesses -ne 0 -or
        $state.UnresolvedProcessPaths -ne 0 -or @($state.Evidence.PSObject.Properties | Where-Object Value).Count -ne 0){
        throw 'HOLD_MIGRATION_NOT_IDLE'
    }
}
Assert-MigrationIdle
$session=Open-ApprovedTrialRenewalStore
$store=$session.Store
try {
    if(!$session.Binding.WorkerDisabled){throw 'HOLD_MIGRATION_WORKER_ENABLED'}
    $store.EnsureNoOperationInputs()
    $store.EnsureExistingTaskRecoveryAbsent()
    $cert=Get-Item -LiteralPath "Cert:\LocalMachine\Root\$trialRenewalThumb"
    try {
        $public=[Security.Cryptography.X509Certificates.RSACertificateExtensions]::GetRSAPublicKey($cert)
        try {
            if(!$public -or (Get-TrialRenewalSha $cert.RawData) -cne $trialRenewalDer){throw 'HOLD_MIGRATION_CERT_PIN'}
            $head=Resolve-ProtectedTrialHead $store $session.Binding $public $trialRenewalSpki ([DateTimeOffset]::UtcNow)
            $expired=Read-TrialRenewalUtc $head.Trial.ExpiresUtc
            if([DateTimeOffset]::UtcNow -lt $expired){throw 'HOLD_MIGRATION_PRIOR_TRIAL_LIVE'}
            $prior=Read-TrialRenewalStream ($store.ReadCode('Supervisor.exe')) 16777216
            if((Get-TrialRenewalSha $prior) -cne $head.Identity.SupervisorSha256){throw 'HOLD_MIGRATION_PRIOR_IMAGE'}
            $probe=$store.ReadCode('Probe.exe')
            try {
                function Hash-Stream($f) {
                    $f.Position=0
                    [Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($f)).ToLowerInvariant()
                }
                if((Hash-Stream $probe) -cne $head.Identity.ProbeSha256){throw 'HOLD_MIGRATION_PROBE_HASH'}
                $manifestRaw=Read-TrialRenewalStream ($store.ReadRegistrationArtifact($true))
                $manifest=Read-TrialRenewalJson $manifestRaw
                $rows=@($manifest.Results)
                if($manifest.Schema -cne 'LIMITLESS_DISCORD_V2_PRODUCT_SIGNING_MANIFEST_V1' -or
                    $manifest.SourceCommit -cne $build.Source -or $rows.Count -ne 1 -or
                    $rows[0].Name -cne 'Supervisor.exe' -or $rows[0].UnsignedSha256 -cne $build.Sha256 -or
                    $rows[0].Status -cne 'PASS' -or $rows[0].SignTool -cne 'PASS' -or $rows[0].Authenticode -cne 'Valid' -or
                    $rows[0].RsaSpkiSha256 -cne $trialRenewalSpki -or $rows[0].CertificateDerSha256 -cne $trialRenewalDer){
                    throw 'HOLD_MIGRATION_SIGNING_MANIFEST_BINDING'
                }
                $signed=$store.ReadRegistrationArtifact($false)
                try {
                    if($signed.Length -ne $rows[0].SignedBytes -or $signed.Length -le 0 -or $signed.Length -gt 16777216){throw 'HOLD_MIGRATION_NEW_SIZE'}
                    $newImage=[byte[]]::new([int]$signed.Length)
                    $signed.ReadExactly($newImage)
                    $newSha=Get-TrialRenewalSha $newImage
                    if($newSha -cne $rows[0].SignedSha256 -or $newSha -ceq $head.Identity.SupervisorSha256){throw 'HOLD_MIGRATION_NEW_SHA'}
                    $signer=Get-AuthenticodeSignature -LiteralPath 'C:\ProgramData\LimitlessDiscordV2SupervisorRecoverySigning20261128\Supervisor.exe'
                    if($signer.Status -ne 'Valid' -or !$signer.SignerCertificate -or
                        $signer.SignerCertificate.Thumbprint -cne $trialRenewalThumb -or
                        (Get-TrialRenewalSha $signer.SignerCertificate.RawData) -cne $trialRenewalDer){
                        throw 'HOLD_MIGRATION_NEW_AUTHENTICODE'
                    }
                    $archiveRaw=Read-TrialRenewalStream ($store.ReadFailedRegistrationEvidence())
                    $archive=Read-TrialRenewalJson $archiveRaw
                    if($archive.Profile -cne 'DISCORD_V2_FAILED_REGISTRATION_ARCHIVE_V1' -or $archive.TrialSha256 -cne $head.TrialSha -or
                        @($archive.Files).Count -ne 8){throw 'HOLD_MIGRATION_REGISTRATION_ARCHIVE'}
                    $failedLedgerRaw=Read-TrialRenewalStream ($store.ReadOriginalConsumption($archive.ApprovalId)) 512
                    if((Get-TrialRenewalSha $failedLedgerRaw) -cne $archive.LedgerSha256){throw 'HOLD_MIGRATION_FAILED_LEDGER'}
                    $priorIntent=@($archive.Files | Where-Object {$_.Area -ceq 'Evidence' -and $_.Name -ceq ($head.Identity.TrialId+'.admin.register-intent')})
                    $priorHold=@($archive.Files | Where-Object {$_.Area -ceq 'Evidence' -and $_.Name -cmatch ('^'+$head.Identity.TrialId+'\.admin\.hold-[a-f0-9]{32}$')})
                    if($priorIntent.Count -ne 1 -or $priorHold.Count -ne 1){throw 'HOLD_RECOVERY_ARCHIVE_EVIDENCE'}
                    $diagnosticRaw=Read-TrialRenewalStream ($store.ReadRegistrationDiagnostic()) 16384
                    $diagnosticLines=@([Text.Encoding]::UTF8.GetString($diagnosticRaw) -split '\r?\n' | Where-Object {$_} | ForEach-Object {Read-TrialRenewalJson ([Text.Encoding]::UTF8.GetBytes($_))})
                    if(@($diagnosticLines|Where-Object {$_.Channel -ceq 'stderr' -and $_.Status -ceq 'HOLD_REGISTER_READBACK_REJECTED'}).Count -ne 1 -or
                        @($diagnosticLines|Where-Object {$_.Channel -ceq 'process' -and $_.Status -ceq 'EXIT' -and $_.ExitCode -eq 70}).Count -ne 1){
                        throw 'HOLD_RECOVERY_DIAGNOSTIC_CONTENT'
                    }
                    Assert-ExistingTaskFingerprint $head.TrialSha
                    if(!$MigrateAfterExplicitApproval) {
                        'EXISTING_TASK_MIGRATION_READINESS_VERIFIED_NO_SIGNING_NO_WRITES'
                        return
                    }
                    $basis=Read-Host '새 신뢰·위험 검토 근거를 32~2000자로 입력. 비밀값 입력 금지'
                    if($basis.Trim().Length -lt 32 -or $basis.Length -gt 2000){throw 'HOLD_MIGRATION_BASIS'}
                    if((Read-Host 'C 별도 승인 확인 후 RECOVER-EXISTING-TASK-AND-PROMOTE-TRIAL 입력') -cne 'RECOVER-EXISTING-TASK-AND-PROMOTE-TRIAL'){
                        throw 'HOLD_MIGRATION_APPROVAL'
                    }
                    Assert-MigrationIdle
                    $store.EnsureNoOperationInputs()
    $store.EnsureExistingTaskRecoveryAbsent()
                    $privateCert=Get-Item -LiteralPath "Cert:\LocalMachine\My\$trialRenewalThumb"
                    try {
                        $key=[Security.Cryptography.X509Certificates.RSACertificateExtensions]::GetRSAPrivateKey($privateCert)
                        try {
                            $now=[DateTimeOffset]::UtcNow
                            $deadline=$now.AddMinutes(5)
                            $trialEnd=$now.AddMinutes(15)
                            if(!$key -or (Get-TrialRenewalSha $privateCert.RawData) -cne $trialRenewalDer -or
                                $now.UtcDateTime -lt $privateCert.NotBefore.ToUniversalTime() -or
                                $trialEnd.UtcDateTime -ge $privateCert.NotAfter.ToUniversalTime()){
                                throw 'HOLD_MIGRATION_SIGNING_KEY'
                            }
                            $utf8=[Text.UTF8Encoding]::new($false,$true)
                            function Encode($o){return ,$utf8.GetBytes(($o|ConvertTo-Json -Compress -Depth 12))}
                            $nextBasis=$utf8.GetBytes($basis)
                            $nextTrust=Read-TrialRenewalJson $head.TrustRaw
                            $nextTrust.SupervisorSha256=$newSha
                            $nextTrust.ObservedUtc=$now.ToString('o')
                            $nextTrust.ExpiresUtc=$trialEnd.ToString('o')
                            $nextTrust.BasisSha256=Get-TrialRenewalSha $nextBasis
                            $trustRaw=Encode $nextTrust
                            $next=Read-TrialRenewalJson $head.TrialRaw
                            $next.SupervisorSha256=$newSha
                            $next.ExpiresUtc=$trialEnd.ToString('o')
                            $next.FakeOnlyIsolation.TrustEvidenceSha256=Get-TrialRenewalSha $trustRaw
                            $nextRaw=Encode $next
                            $newIdentity=[ordered]@{}
                            foreach($name in $head.Identity.Keys){$newIdentity[$name]=$head.Identity[$name]}
                            $newIdentity.SupervisorSha256=$newSha
                            Assert-TrialRenewalDocuments $next $nextTrust $trustRaw $nextBasis $newIdentity
                            $proof=[ordered]@{
                                Profile='DISCORD_V2_TRIAL_SUPERVISOR_MIGRATION_V1';Approved=$true;RenewalId=[Guid]::NewGuid().ToString('N')
                                PriorTrialSha256=$head.TrialSha;PriorTrustSha256=$head.TrustSha;PriorBasisSha256=$head.BasisSha
                                NextTrialSha256=(Get-TrialRenewalSha $nextRaw);NextTrustSha256=(Get-TrialRenewalSha $trustRaw)
                                NextBasisSha256=(Get-TrialRenewalSha $nextBasis);NextSupervisorSha256=$newSha
                                SigningManifestSha256=(Get-TrialRenewalSha $manifestRaw);RegistrationArchiveSha256=(Get-TrialRenewalSha $archiveRaw)
                                SupervisorSourceCommit=$build.Source;IssuedUtc=$now.ToString('o')
                                PromoteBeforeUtc=$deadline.ToString('o');TrialExpiresUtc=$trialEnd.ToString('o')
                            }
                            foreach($name in $head.Identity.Keys){$proof[$name]=$head.Identity[$name]}
                            $proofRaw=Encode $proof
                            $signature=$key.SignData($proofRaw,[Security.Cryptography.HashAlgorithmName]::SHA256,[Security.Cryptography.RSASignaturePadding]::Pss)
                            $verified=Read-VerifiedTrialRenewalReceipt $proofRaw $signature $public $trialRenewalSpki $head.TrialSha $head.Identity
                            Assert-TrialRenewalPromotion $verified ([DateTimeOffset]::UtcNow) $expired
                            $recovery=[ordered]@{
                                Profile='DISCORD_V2_EXISTING_TASK_RECOVERY_V1';TrialId=$head.Identity.TrialId
                                PriorTrialSha256=$head.TrialSha;NextTrialSha256=$verified.NextTrialSha256
                                PriorSupervisorSha256=$head.Identity.SupervisorSha256;NextSupervisorSha256=$newSha;ProbeSha256=$head.Identity.ProbeSha256
                                WorkerSid=$session.Binding.Worker;TaskPath=$session.Binding.TaskPath
                                CodeRoot=$session.Binding.CodeRoot;EvidenceRoot=$session.Binding.EvidenceRoot
                                TaskFingerprintSha256=$approvedFingerprint;RegisterIntentSha256=$priorIntent[0].Sha256;HoldEvidenceSha256=$priorHold[0].Sha256
                                PriorConsumptionSha256=$archive.LedgerSha256;PriorApprovalId=$archive.ApprovalId
                                RegistrationDiagnosticSha256=(Get-TrialRenewalSha $diagnosticRaw);RecoveryId=[Guid]::NewGuid().ToString('N')
                                IssuedUtc=$now.ToString('o');ExpiresUtc=$deadline.ToString('o');NextTrialExpiresUtc=$trialEnd.ToString('o')
                            }
                            $recoveryRaw=Encode $recovery
                            $recoverySig=$key.SignData($recoveryRaw,[Security.Cryptography.HashAlgorithmName]::SHA256,[Security.Cryptography.RSASignaturePadding]::Pss)
                            $null=Assert-ExistingTaskRecoverySignature $recoveryRaw $recoverySig $public $trialRenewalSpki $recovery ([DateTimeOffset]::UtcNow)
                            Assert-MigrationIdle
                            Assert-ExistingTaskFingerprint $head.TrialSha
                            $store.BeginSupervisorMigration($head.TrialSha,$deadline)
                            $store.WriteCase('Renewal.sig',$signature,$deadline)
                            $store.WriteCase('Renewal.json',$proofRaw,$deadline)
                            $store.WriteCase('SupervisorSigningManifest.json',$manifestRaw,$deadline)
                            $store.WriteCase('FailedRegistrationEvidence.json',$archiveRaw,$deadline)
                            $store.ArchiveTrialFile('Supervisor.exe',$head.Identity.SupervisorSha256,$deadline)
                            $store.ArchiveTrialFile('TrialContract.json',$head.TrialSha,$deadline)
                            $store.ArchiveTrialFile($head.TrustName,$head.TrustSha,$deadline)
                            $store.ArchiveTrialFile($head.BasisName,$head.BasisSha,$deadline)
                            $store.WriteSupervisor($newImage,$deadline)
                            $store.WriteCode($head.BasisName,$nextBasis,$deadline)
                            $store.WriteCode($head.TrustName,$trustRaw,$deadline)
                            $store.WriteCode('TrialContract.json',$nextRaw,$deadline)
                            $store.WriteExistingTaskRecovery('ExistingTaskRecovery.v1.sig',$recoverySig,$deadline)
                            $store.WriteExistingTaskRecovery('ExistingTaskRecovery.v1.json',$recoveryRaw,$deadline)
                            $persistedRecovery=Read-TrialRenewalStream ($store.ReadExistingTaskRecovery('ExistingTaskRecovery.v1.json')) 8192
                            $persistedSignature=Read-TrialRenewalStream ($store.ReadExistingTaskRecovery('ExistingTaskRecovery.v1.sig')) 4096
                            $null=Assert-ExistingTaskRecoverySignature $persistedRecovery $persistedSignature $public $trialRenewalSpki $recovery ([DateTimeOffset]::UtcNow)
                            Assert-ExistingTaskFingerprint $head.TrialSha
                            $at=[DateTimeOffset]::UtcNow
                            Assert-TrialRenewalPromotion $verified $at $expired
                            $commit=Encode ([ordered]@{Profile='DISCORD_V2_TRIAL_RENEWAL_COMMIT_V1';ReceiptSha256=(Get-TrialRenewalSha $proofRaw);PromotedUtc=$at.ToString('o')})
                            $store.WriteCase('PromotionCommit.json',$commit,$deadline)
                            $confirmed=Resolve-ProtectedTrialHead $store $session.Binding $public $trialRenewalSpki ([DateTimeOffset]::UtcNow)
                            $current=Read-TrialRenewalStream ($store.ReadCode('Supervisor.exe')) 16777216
                            $oldLedger=Read-TrialRenewalStream ($store.ReadOriginalConsumption($archive.ApprovalId)) 512
                            if($confirmed.TrialSha -cne $verified.NextTrialSha256 -or $confirmed.Identity.SupervisorSha256 -cne $newSha -or
                                (Get-TrialRenewalSha $current) -cne $newSha -or (Hash-Stream $probe) -cne $head.Identity.ProbeSha256 -or
                                (Get-TrialRenewalSha $oldLedger) -cne $archive.LedgerSha256){throw 'HOLD_MIGRATION_FINAL_READBACK'}
                            'EXISTING_TASK_RECOVERY_PROMOTED_NO_TASK_EXECUTION'
                            "RECOVERY_EXPIRES_UTC=$($deadline.ToString('o'))"
                            "TRIAL_EXPIRES_UTC=$($trialEnd.ToString('o'))"
                        } finally {if($key){$key.Dispose()}}
                    } finally {$privateCert.Dispose()}
                } finally {$signed.Dispose()}
            } finally {$probe.Dispose()}
        } finally {if($public){$public.Dispose()}}
    } finally {$cert.Dispose()}
} finally {$store.Dispose()}

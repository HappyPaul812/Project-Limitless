#Requires -Version 7.5
param([switch]$PromoteAfterExplicitApproval)
$ErrorActionPreference='Stop'
$hostPath='C:\Program Files\PowerShell\7\pwsh.exe'
if (![Environment]::Is64BitProcess -or (Get-Process -Id $PID).Path -ine $hostPath) {throw 'HOLD_RENEWAL_HOST'}
$hostSig=Get-AuthenticodeSignature -LiteralPath $hostPath
if($hostSig.Status -ne 'Valid' -or $hostSig.SignerCertificate.Subject -notmatch '(^|,\s*)O=Microsoft Corporation(,|$)'){throw 'HOLD_RENEWAL_HOST_SIGNER'}
. (Join-Path $PSScriptRoot 'Load-TrialRenewalTools.ps1')
. (Join-Path $PSScriptRoot 'ExistingTaskRecoverySignature.ps1')
$historyReader=Join-Path $PSScriptRoot 'Read-ExpiredRecoveryForRenewal.ps1'
$session=Open-ApprovedTrialRenewalStore
$store=$session.Store
try {
    if(!$session.Binding.WorkerDisabled){throw 'HOLD_RENEWAL_WORKER_ENABLED'}
    $taskInspect=Join-Path $PSScriptRoot '../../../DV2Fix20261128/TaskPrep/Invoke-V2TaskPrepared.ps1'
    function Assert-RenewalIdle {
        $state=(& $taskInspect -Mode Inspect)|ConvertFrom-Json
        if(!$state.TaskPresent -or !$state.Task.Disabled -or $state.Task.Instances -ne 0 -or !$state.WorkerDisabled -or $state.RemainingProductProcesses -ne 0 -or
            $state.UnresolvedProcessPaths -ne 0 -or @($state.Evidence.PSObject.Properties | Where-Object Value).Count -ne 0){
            throw 'HOLD_RENEWAL_TASK_PROCESS_OR_EVIDENCE'
        }
    }
    Assert-RenewalIdle
    $store.EnsureNoOperationInputs()
    $cert=Get-Item -LiteralPath "Cert:\LocalMachine\Root\$trialRenewalThumb"
    try {
        $public=[Security.Cryptography.X509Certificates.RSACertificateExtensions]::GetRSAPublicKey($cert)
        try {
            if(!$public -or (Get-TrialRenewalSha $cert.RawData) -cne $trialRenewalDer){throw 'HOLD_RENEWAL_CERT_PIN'}
            $head=Resolve-ProtectedTrialHead $store $session.Binding $public $trialRenewalSpki ([DateTimeOffset]::UtcNow)
            $oldExpiry=Read-TrialRenewalUtc $head.Trial.ExpiresUtc
            if([DateTimeOffset]::UtcNow -lt $oldExpiry){throw 'HOLD_RENEWAL_PRIOR_STILL_LIVE'}
            $historic=& $historyReader
            if(!$historic -or $historic.CurrentTrialSha -cne $head.TrialSha){throw 'HOLD_RENEWAL_RECOVERY_PRIOR_BINDING'}
            $supervisor=$store.ReadCode('Supervisor.exe')
            try {
                $probe=$store.ReadCode('Probe.exe')
                try {
                    function Hash-Stream($file) {
                        $file.Position=0
                        [Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($file)).ToLowerInvariant()
                    }
                    if((Hash-Stream $supervisor) -cne $head.Identity.SupervisorSha256 -or
                        (Hash-Stream $probe) -cne $head.Identity.ProbeSha256){throw 'HOLD_RENEWAL_PRODUCT_SHA'}
                    if(!$PromoteAfterExplicitApproval){
                        'EXISTING_TASK_RECOVERY_RENEWAL_READINESS_VERIFIED_NO_SIGNING_NO_WRITES'
                        return
                    }
                    $basis=Read-Host '새 확인 시각의 미공유·다른 실행 경로/미신뢰 코드 확인과 잔여 위험 수용 근거를 32~2000자로 입력. 비밀값 입력 금지'
                    if($basis.Trim().Length -lt 32 -or $basis.Length -gt 2000){throw 'HOLD_RENEWAL_TRUST_BASIS'}
                    if((Read-Host '사실 확인 및 별도 보호 갱신 승인 후 RENEW-SIGNED-EXISTING-TASK-RECOVERY 입력') -cne 'RENEW-SIGNED-EXISTING-TASK-RECOVERY'){
                        throw 'HOLD_RENEWAL_CONFIRMATION'
                    }
                    Assert-RenewalIdle
                    $store.EnsureNoOperationInputs()
                    $privateCert=Get-Item -LiteralPath "Cert:\LocalMachine\My\$trialRenewalThumb"
                    try {
                        $private=[Security.Cryptography.X509Certificates.RSACertificateExtensions]::GetRSAPrivateKey($privateCert)
                        try {
                            if(!$private -or (Get-TrialRenewalSha $privateCert.RawData) -cne $trialRenewalDer){throw 'HOLD_RENEWAL_SIGNING_KEY'}
                            $now=[DateTimeOffset]::UtcNow
                            $deadline=$now.AddMinutes(5)
                            $trialEnd=$now.AddMinutes(15)
                            if($now.UtcDateTime -lt $privateCert.NotBefore.ToUniversalTime() -or $trialEnd.UtcDateTime -ge $privateCert.NotAfter.ToUniversalTime()){
                                throw 'HOLD_RENEWAL_SIGNER_TIME'
                            }
                            $utf8=[Text.UTF8Encoding]::new($false,$true)
                            function Encode($object){return ,$utf8.GetBytes(($object|ConvertTo-Json -Compress -Depth 12))}
                            $nextBasis=$utf8.GetBytes($basis)
                            $nextTrust=Read-TrialRenewalJson $head.TrustRaw
                            $nextTrust.ObservedUtc=$now.ToString('o')
                            $nextTrust.ExpiresUtc=$trialEnd.ToString('o')
                            $nextTrust.BasisSha256=Get-TrialRenewalSha $nextBasis
                            $nextTrustRaw=Encode $nextTrust
                            $next=Read-TrialRenewalJson $head.TrialRaw
                            $next.ExpiresUtc=$trialEnd.ToString('o')
                            $next.FakeOnlyIsolation.TrustEvidenceSha256=Get-TrialRenewalSha $nextTrustRaw
                            $nextRaw=Encode $next
                            Assert-TrialRenewalDocuments $next $nextTrust $nextTrustRaw $nextBasis $head.Identity
                            $receipt=[ordered]@{
                                Profile='DISCORD_V2_TRIAL_RENEWAL_V1';Approved=$true;RenewalId=[Guid]::NewGuid().ToString('N')
                                PriorTrialSha256=$head.TrialSha;PriorTrustSha256=$head.TrustSha;PriorBasisSha256=$head.BasisSha
                                NextTrialSha256=(Get-TrialRenewalSha $nextRaw);NextTrustSha256=(Get-TrialRenewalSha $nextTrustRaw)
                                NextBasisSha256=(Get-TrialRenewalSha $nextBasis);IssuedUtc=$now.ToString('o')
                                PromoteBeforeUtc=$deadline.ToString('o');TrialExpiresUtc=$trialEnd.ToString('o')
                            }
                            foreach($name in $head.Identity.Keys){$receipt[$name]=$head.Identity[$name]}
                            foreach($field in 'IssuedUtc','PromoteBeforeUtc','TrialExpiresUtc'){$null=Read-TrialRenewalUtc $receipt[$field]}
                            $null=Read-TrialRenewalUtc $next.ExpiresUtc
                            $null=Read-TrialRenewalUtc $nextTrust.ObservedUtc
                            $null=Read-TrialRenewalUtc $nextTrust.ExpiresUtc
                            if($now -ge $deadline -or $deadline -ge $trialEnd){throw 'HOLD_RECOVERY_RENEWAL_TIMES'}
                            $fresh=[ordered]@{}
                            foreach($field in $historic.Contract.Keys){$fresh[$field]=$historic.Contract[$field]}
                            $fresh.NextTrialSha256=$receipt.NextTrialSha256
                            $fresh.RecoveryId=[Guid]::NewGuid().ToString('N')
                            $fresh.IssuedUtc=$now.ToString('o')
                            $fresh.ExpiresUtc=$deadline.ToString('o')
                            $fresh.NextTrialExpiresUtc=$trialEnd.ToString('o')
                            foreach($field in 'IssuedUtc','ExpiresUtc','NextTrialExpiresUtc'){$null=Read-TrialRenewalUtc $fresh[$field]}
                            $freshRaw=Encode $fresh
                            $freshSig=$private.SignData($freshRaw,[Security.Cryptography.HashAlgorithmName]::SHA256,[Security.Cryptography.RSASignaturePadding]::Pss)
                            $null=Assert-ExistingTaskRecoverySignature $freshRaw $freshSig $public $trialRenewalSpki $fresh ([DateTimeOffset]::UtcNow)
                            $receiptRaw=Encode $receipt
                            $sig=$private.SignData($receiptRaw,[Security.Cryptography.HashAlgorithmName]::SHA256,[Security.Cryptography.RSASignaturePadding]::Pss)
                            $proof=Read-VerifiedTrialRenewalReceipt $receiptRaw $sig $public $trialRenewalSpki $head.TrialSha $head.Identity
                            Assert-TrialRenewalPromotion $proof ([DateTimeOffset]::UtcNow) $oldExpiry
                            Assert-RenewalIdle
                            $rechecked=& $historyReader
                            if($rechecked.CurrentTrialSha -cne $head.TrialSha -or $rechecked.PayloadSha -cne $historic.PayloadSha -or
                                $rechecked.SignatureSha -cne $historic.SignatureSha){throw 'HOLD_RECOVERY_HISTORY_CHANGED'}
                            $store.Begin($head.TrialSha,$deadline)
                            $store.WriteCase('Renewal.sig',$sig,$deadline)
                            $store.WriteCase('Renewal.json',$receiptRaw,$deadline)
                            $store.ArchiveExistingTaskRecovery('ExistingTaskRecovery.v1.json',$historic.PayloadSha,$deadline)
                            $store.ArchiveExistingTaskRecovery('ExistingTaskRecovery.v1.sig',$historic.SignatureSha,$deadline)
                            $store.ArchiveTrialFile('TrialContract.json',$head.TrialSha,$deadline)
                            $store.ArchiveTrialFile($head.TrustName,$head.TrustSha,$deadline)
                            $store.ArchiveTrialFile($head.BasisName,$head.BasisSha,$deadline)
                            $store.WriteCode($head.BasisName,$nextBasis,$deadline)
                            $store.WriteCode($head.TrustName,$nextTrustRaw,$deadline)
                            $store.WriteCode('TrialContract.json',$nextRaw,$deadline)
                            $store.WriteExistingTaskRecovery('ExistingTaskRecovery.v1.sig',$freshSig,$deadline)
                            $store.WriteExistingTaskRecovery('ExistingTaskRecovery.v1.json',$freshRaw,$deadline)
                            $persisted=Read-TrialRenewalStream ($store.ReadExistingTaskRecovery('ExistingTaskRecovery.v1.json')) 8192
                            $persistedSig=Read-TrialRenewalStream ($store.ReadExistingTaskRecovery('ExistingTaskRecovery.v1.sig')) 4096
                            $null=Assert-ExistingTaskRecoverySignature $persisted $persistedSig $public $trialRenewalSpki $fresh ([DateTimeOffset]::UtcNow)
                            $fingerprint=@(& (Join-Path $PSScriptRoot '../../Inspection/Read-ExistingTaskProductFingerprint.ps1') -PriorTaskArgumentSha $fresh.PriorTrialSha256)
                            if($fingerprint -cnotcontains ('TASK_FINGERPRINT_SHA256='+$fresh.TaskFingerprintSha256)){throw 'HOLD_RECOVERY_FINGERPRINT_CHANGED'}
                            $promoted=[DateTimeOffset]::UtcNow
                            Assert-TrialRenewalPromotion $proof $promoted $oldExpiry
                            $commit=Encode ([ordered]@{Profile='DISCORD_V2_TRIAL_RENEWAL_COMMIT_V1';ReceiptSha256=(Get-TrialRenewalSha $receiptRaw);PromotedUtc=$promoted.ToString('o')})
                            $store.WriteCase('PromotionCommit.json',$commit,$deadline)
                            $confirmed=Resolve-ProtectedTrialHead $store $session.Binding $public $trialRenewalSpki ([DateTimeOffset]::UtcNow)
                            if($confirmed.TrialSha -cne $proof.NextTrialSha256 -or (Hash-Stream $supervisor) -cne $head.Identity.SupervisorSha256 -or
                                (Hash-Stream $probe) -cne $head.Identity.ProbeSha256){throw 'HOLD_RENEWAL_FINAL_READBACK'}
                            'EXISTING_TASK_RECOVERY_RENEWED_NO_TASK_EXECUTION'
                            "RECOVERY_EXPIRES_UTC=$($deadline.ToString('o'))"
                            "TRIAL_EXPIRES_UTC=$($trialEnd.ToString('o'))"
                        } finally {if($private){$private.Dispose()}}
                    } finally {$privateCert.Dispose()}
                } finally {$probe.Dispose()}
            } finally {$supervisor.Dispose()}
        } finally {if($public){$public.Dispose()}}
    } finally {$cert.Dispose()}
} finally {$store.Dispose()}

param([string]$Policy)
$ErrorActionPreference='Stop'
. $Policy
class IsolatedStore {
    [hashtable]$Original=@{}
    [hashtable]$Cases=@{}
    [hashtable]$Code=@{}
    [IO.MemoryStream]ReadOriginalRequest([bool]$signature){return [IO.MemoryStream]::new([byte[]]$this.Original[[string]$signature],$false)}
    [IO.MemoryStream]ReadOriginalConsumption([string]$id){return [IO.MemoryStream]::new([byte[]]$this.Code['test-ledger'],$false)}
    [IO.MemoryStream]ReadOriginalManifest(){return [IO.MemoryStream]::new([byte[]]$this.Original['manifest'],$false)}
    [bool]CaseExists([string]$prior){return $this.Cases.ContainsKey($prior)}
    [IO.MemoryStream]ReadCase([string]$prior,[string]$name){
        if(!$this.Cases[$prior].ContainsKey($name)){throw 'HOLD_SYNTHETIC_PARTIAL_OR_COLLISION'}
        return [IO.MemoryStream]::new([byte[]]$this.Cases[$prior][$name],$false)
    }
    [IO.MemoryStream]ReadCode([string]$name){return [IO.MemoryStream]::new([byte[]]$this.Code[$name],$false)}
}
function Raw($o){return ,([Text.Encoding]::UTF8.GetBytes(($o|ConvertTo-Json -Compress -Depth 12)))}
function Expect-Hold([scriptblock]$what,[string]$message) {
    try { $null=& $what } catch { if($_.Exception.Message -like "*$message*") {return};throw }
    throw "Expected $message"
}
$now=[DateTimeOffset]'2026-11-28T05:00:00Z'
$key=[Security.Cryptography.RSA]::Create(2048)
try {
    $pin=Get-TrialRenewalSha ($key.ExportSubjectPublicKeyInfo())
    function Sign([byte[]]$b){
        return ,$key.SignData($b,[Security.Cryptography.HashAlgorithmName]::SHA256,[Security.Cryptography.RSASignaturePadding]::Pss)
    }
    $fake=[IsolatedStore]::new()
    $binding=[ordered]@{Worker='synthetic';TaskPath='\Synthetic';CodeRoot='C:\Synthetic\Code';EvidenceRoot='C:\Synthetic\Evidence'}
    $trialId='1'*32
    $name="$trialId.fake-only-trust.json"
    $basisName="$trialId.fake-only-trust-basis.txt"
    $super='a'*64;$probe='b'*64
    function Docs([DateTimeOffset]$when,[string]$basisText){
        $basis=[Text.Encoding]::UTF8.GetBytes($basisText)
        $trust=Raw ([ordered]@{
            Approved=$true;TrialId=$trialId;WorkerSid=$binding.Worker;TaskPath=$binding.TaskPath
            CodeRoot=$binding.CodeRoot;EvidenceRoot=$binding.EvidenceRoot;SupervisorSha256=$super;ProbeSha256=$probe
            ObservedUtc=$when.ToString('o');ExpiresUtc=$when.AddMinutes(15).ToString('o');BasisSha256=(Get-TrialRenewalSha $basis)
            GlobalSidAbsence='UNKNOWN';CredentialsNotShared=$true;NoUntrustedSameSidCode=$true;NoOtherLaunchers=$true;ResidualRiskAccepted=$true
        })
        $trial=Raw ([ordered]@{
            Approved=$true;TrialId=$trialId;SupervisorSha256=$super;ProbeSha256=$probe;ExpiresUtc=$when.AddMinutes(15).ToString('o')
            Scope=[ordered]@{WorkerSid=$binding.Worker;TaskPath=$binding.TaskPath;CodeRoot=$binding.CodeRoot;EvidenceRoot=$binding.EvidenceRoot
                Mode='FAKE_ONLY';NoAi=$true;NoIpc=$true;JobSeconds=90;SupervisorCount=1;ProbeCount=1;Register=$true;Start=$true;Cleanup=$true}
            FakeOnlyIsolation=[ordered]@{Profile='FAKE_ONLY_EXECUTION_V1';GlobalSidAbsence='UNKNOWN';TrustEvidenceSha256=(Get-TrialRenewalSha $trust)}
        })
        [pscustomobject]@{Trial=$trial;Trust=$trust;Basis=$basis}
    }
    $prior=Docs ($now.AddMinutes(-20)) 'Synthetic evidence of the prior frozen trial.'
    $next=Docs $now 'Synthetic evidence of the newly reviewed trial.'
    $original=Get-TrialRenewalSha $prior.Trial
    $fake.Code=@{'TrialContract.json'=$prior.Trial;$name=$prior.Trust;$basisName=$prior.Basis;
        'Supervisor.exe'=[byte[]]@(1,2,3);'Probe.exe'=[byte[]]@(4,5,6);'test-ledger'=[byte[]]@(7,8,9)}
    $manifestRaw=Raw ([ordered]@{
        TrialId=$trialId;WorkerSid=$binding.Worker;TaskPath=$binding.TaskPath;CodeRoot=$binding.CodeRoot;EvidenceRoot=$binding.EvidenceRoot
        Files=@(@{Name='Supervisor.exe';Sha256=$super},@{Name='Probe.exe';Sha256=$probe},
            @{Name=$name;Sha256=(Get-TrialRenewalSha $prior.Trust)},@{Name=$basisName;Sha256=(Get-TrialRenewalSha $prior.Basis)})
    })
    $fake.Original['manifest']=$manifestRaw
    $originalRequest=Raw ([ordered]@{Profile='INDEPENDENT_ADMIN_DEPLOY_V2';Approved=$true;TrialId=$trialId;
        DeploymentId=('5'*32);ContractSha256=$original;ManifestSha256=(Get-TrialRenewalSha $manifestRaw)})
    $fake.Original['False']=$originalRequest;$fake.Original['True']=Sign $originalRequest
    $ln=[string][char]10
    $fake.Code['test-ledger']=[Text.Encoding]::UTF8.GetBytes(('5'*32)+$ln+(Get-TrialRenewalSha $manifestRaw)+$ln)
    $head=Resolve-ProtectedTrialHead $fake $binding $key $pin $now
    if($head.TrialSha -cne $original -or $head.Depth -ne 0){throw 'Original head not pinned'}
    $r=[ordered]@{
        Profile='DISCORD_V2_TRIAL_RENEWAL_V1';Approved=$true;RenewalId=('2'*32)
        PriorTrialSha256=$original;PriorTrustSha256=$head.TrustSha;PriorBasisSha256=$head.BasisSha
        NextTrialSha256=(Get-TrialRenewalSha $next.Trial);NextTrustSha256=(Get-TrialRenewalSha $next.Trust)
        NextBasisSha256=(Get-TrialRenewalSha $next.Basis);IssuedUtc=$now.ToString('o')
        PromoteBeforeUtc=$now.AddMinutes(5).ToString('o');TrialExpiresUtc=$now.AddMinutes(15).ToString('o')
    }
    foreach($k in $head.Identity.Keys){$r[$k]=$head.Identity[$k]}
    $rr=Raw $r;$sig=Sign $rr
    $proof=Read-VerifiedTrialRenewalReceipt $rr $sig $key $pin $original $head.Identity
    Assert-TrialRenewalPromotion $proof $now (Read-TrialRenewalUtc $head.Trial.ExpiresUtc)
    Expect-Hold {Assert-TrialRenewalPromotion $proof ($now.AddMinutes(5)) ($now.AddMinutes(-1))} 'HOLD_RENEWAL_PROMOTION_TIME'
    Expect-Hold {Assert-TrialRenewalPromotion $proof $now ($now.AddSeconds(1))} 'HOLD_RENEWAL_PROMOTION_TIME'
    'PASS exact expiry and prior-still-live rejected'
    $tampered=[byte[]]$rr.Clone();$tampered[8]=$tampered[8]-bxor 1
    Expect-Hold {Read-VerifiedTrialRenewalReceipt $tampered $sig $key $pin $original $head.Identity} 'HOLD_RENEWAL_SIGNATURE'
    Expect-Hold {Read-VerifiedTrialRenewalReceipt $rr $sig $key $pin $r.NextTrialSha256 $head.Identity} 'HOLD_RENEWAL_PRIOR_OR_ID'
    'PASS tamper and replay against a different head rejected'
    $digest=@{}
    foreach($file in 'Supervisor.exe','Probe.exe','test-ledger'){$digest[$file]=Get-TrialRenewalSha $fake.Code[$file]}
    $fake.Cases[$original]=@{'Renewal.json'=$rr;'Renewal.sig'=$sig;'Prior-TrialContract.json'=$prior.Trial;
        "Prior-$name"=$prior.Trust;"Prior-$basisName"=$prior.Basis}
    Expect-Hold {Resolve-ProtectedTrialHead $fake $binding $key $pin $now} 'HOLD_SYNTHETIC_PARTIAL_OR_COLLISION'
    if((Get-TrialRenewalSha $fake.Cases[$original]['Prior-TrialContract.json']) -cne $original){throw 'Partial erased prior'}
    'PASS partial or occupied case does not advance and old evidence survives'
    $fake.Cases[$original]['PromotionCommit.json']=Raw ([ordered]@{Profile='DISCORD_V2_TRIAL_RENEWAL_COMMIT_V1';ReceiptSha256=(Get-TrialRenewalSha $rr);PromotedUtc=$now.ToString('o')})
    $fake.Code['TrialContract.json']=$next.Trial;$fake.Code[$name]=$next.Trust;$fake.Code[$basisName]=$next.Basis
    $renewed=Resolve-ProtectedTrialHead $fake $binding $key $pin ($now.AddMinutes(6))
    if($renewed.Depth -ne 1 -or $renewed.TrialSha -cne $r.NextTrialSha256){throw 'Renewed head not resolved'}
    'PASS renewed protected head resolved using signed committed transition'
    foreach($file in $digest.Keys){if((Get-TrialRenewalSha $fake.Code[$file]) -cne $digest[$file]){throw 'Unrelated fixture changed'}}
    'PASS existing image and ledger bytes unchanged'
    $fake.Code[$name]=$prior.Trust
    Expect-Hold {Resolve-ProtectedTrialHead $fake $binding $key $pin ($now.AddMinutes(6))} 'HOLD_RENEWAL_CURRENT_HEAD'
    'PASS current deployed trust tampering rejected'
} finally {$key.Dispose()}

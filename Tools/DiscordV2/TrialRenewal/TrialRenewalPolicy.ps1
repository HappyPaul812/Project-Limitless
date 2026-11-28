# Trial 보호 갱신 무결성 정책. 운영 파일을 쓰는 코드는 별도 어댑터에서만 호출합니다.
Set-StrictMode -Version Latest

function Get-TrialRenewalSha([byte[]]$Bytes) {
    [Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($Bytes)).ToLowerInvariant()
}

function Assert-TrialRenewalFields($Object,[string[]]$Names) {
    if ($Object -isnot [Collections.IDictionary] -or $Object.Count -ne $Names.Count) { throw 'HOLD_RENEWAL_FIELDS' }
    foreach ($key in $Object.Keys) {
        if ($key -cnotin $Names) { throw 'HOLD_RENEWAL_FIELDS' }
    }
}

function Read-TrialRenewalJson([byte[]]$Bytes) {
    if ($Bytes.Length -le 0 -or $Bytes.Length -gt 16384) { throw 'HOLD_RENEWAL_SIZE' }
    $doc=[System.Text.Json.JsonDocument]::Parse([ReadOnlyMemory[byte]]::new($Bytes))
    try {
        if ($doc.RootElement.ValueKind -ne [System.Text.Json.JsonValueKind]::Object) { throw 'HOLD_RENEWAL_JSON' }
        $names=[Collections.Generic.HashSet[string]]::new([StringComparer]::OrdinalIgnoreCase)
        foreach ($p in $doc.RootElement.EnumerateObject()) {
            if (!$names.Add($p.Name)) { throw 'HOLD_RENEWAL_DUPLICATE_FIELD' }
        }
    } finally {$doc.Dispose()}
    [Text.UTF8Encoding]::new($false,$true).GetString($Bytes) | ConvertFrom-Json -AsHashtable -DateKind String
}

function Read-TrialRenewalUtc([string]$Value) {
    $result=[DateTimeOffset]::MinValue
    if (![DateTimeOffset]::TryParseExact($Value,'o',[Globalization.CultureInfo]::InvariantCulture,
        [Globalization.DateTimeStyles]::None,[ref]$result) -or $result.Offset -ne [TimeSpan]::Zero) { throw 'HOLD_RENEWAL_UTC' }
    return $result
}

function Read-VerifiedTrialRenewalReceipt(
    [byte[]]$ReceiptBytes,[byte[]]$Signature,[Security.Cryptography.RSA]$Verifier,
    [string]$ExpectedSigner,[string]$PriorSha,$Identity
) {
    if ((Get-TrialRenewalSha ($Verifier.ExportSubjectPublicKeyInfo())) -cne $ExpectedSigner -or
        $Signature.Length -le 0 -or $Signature.Length -gt 4096 -or
        !$Verifier.VerifyData($ReceiptBytes,$Signature,[Security.Cryptography.HashAlgorithmName]::SHA256,
            [Security.Cryptography.RSASignaturePadding]::Pss)) { throw 'HOLD_RENEWAL_SIGNATURE' }
    $r=Read-TrialRenewalJson $ReceiptBytes
    Assert-TrialRenewalFields $r @('Profile','Approved','RenewalId','PriorTrialSha256','PriorTrustSha256','PriorBasisSha256',
        'NextTrialSha256','NextTrustSha256','NextBasisSha256','SourceRequestSha256','TrialId','WorkerSid','TaskPath','CodeRoot',
        'EvidenceRoot','SupervisorSha256','ProbeSha256','IssuedUtc','PromoteBeforeUtc','TrialExpiresUtc')
    if ($r.Profile -cne 'DISCORD_V2_TRIAL_RENEWAL_V1' -or $r.Approved -isnot [bool] -or !$r.Approved -or
        $r.RenewalId -cnotmatch '^(?!0{32}$)[0-9a-f]{32}$' -or $r.PriorTrialSha256 -cne $PriorSha -or
        $r.NextTrialSha256 -ceq $PriorSha) { throw 'HOLD_RENEWAL_PRIOR_OR_ID' }
    foreach ($field in 'PriorTrialSha256','PriorTrustSha256','PriorBasisSha256','NextTrialSha256','NextTrustSha256','NextBasisSha256') {
        if ($r[$field] -cnotmatch '^[0-9a-f]{64}$') { throw 'HOLD_RENEWAL_HASH' }
    }
    foreach ($field in 'SourceRequestSha256','TrialId','WorkerSid','TaskPath','CodeRoot','EvidenceRoot','SupervisorSha256','ProbeSha256') {
        if ($r[$field] -cne $Identity[$field]) { throw 'HOLD_RENEWAL_IDENTITY' }
    }
    $issued=Read-TrialRenewalUtc $r.IssuedUtc
    $promote=Read-TrialRenewalUtc $r.PromoteBeforeUtc
    $expires=Read-TrialRenewalUtc $r.TrialExpiresUtc
    if ($issued -ge $promote -or $promote -gt $expires -or ($promote-$issued).TotalMinutes -gt 5 -or
        ($expires-$issued).TotalMinutes -gt 15) { throw 'HOLD_RENEWAL_WINDOW' }
    return $r
}

function Assert-TrialRenewalPromotion($Receipt,[DateTimeOffset]$Now,[DateTimeOffset]$PriorExpiry) {
    $issued=Read-TrialRenewalUtc $Receipt.IssuedUtc
    $end=Read-TrialRenewalUtc $Receipt.PromoteBeforeUtc
    if ($PriorExpiry -gt $issued -or $Now -lt $issued -or $Now -ge $end) { throw 'HOLD_RENEWAL_PROMOTION_TIME' }
}

function Assert-TrialRenewalCommit($Receipt,[byte[]]$ReceiptBytes,[byte[]]$CommitBytes,[DateTimeOffset]$Now) {
    $commit=Read-TrialRenewalJson $CommitBytes
    Assert-TrialRenewalFields $commit @('Profile','ReceiptSha256','PromotedUtc')
    $at=Read-TrialRenewalUtc $commit.PromotedUtc
    if ($commit.Profile -cne 'DISCORD_V2_TRIAL_RENEWAL_COMMIT_V1' -or
        $commit.ReceiptSha256 -cne (Get-TrialRenewalSha $ReceiptBytes) -or
        $at -lt (Read-TrialRenewalUtc $Receipt.IssuedUtc) -or
        $at -ge (Read-TrialRenewalUtc $Receipt.PromoteBeforeUtc) -or $at -gt $Now) { throw 'HOLD_RENEWAL_COMMIT' }
}

function Read-TrialRenewalStream($Stream,[int]$Limit=16384) {
    try {
        if ($Stream.Length -le 0 -or $Stream.Length -gt $Limit) { throw 'HOLD_RENEWAL_STREAM_SIZE' }
        $bytes=[byte[]]::new([int]$Stream.Length)
        $Stream.ReadExactly($bytes)
        return ,$bytes
    } finally {$Stream.Dispose()}
}

function Assert-TrialRenewalDocuments($Trial,$Trust,[byte[]]$TrustBytes,[byte[]]$BasisBytes,$Identity) {
    foreach ($field in 'TrialId','SupervisorSha256','ProbeSha256') {
        if ($Trial[$field] -cne $Identity[$field] -or $Trust[$field] -cne $Identity[$field]) { throw 'HOLD_RENEWAL_DOC_IDENTITY' }
    }
    foreach ($field in 'WorkerSid','TaskPath','CodeRoot','EvidenceRoot') {
        if ($Trial.Scope[$field] -cne $Identity[$field] -or $Trust[$field] -cne $Identity[$field]) { throw 'HOLD_RENEWAL_DOC_BINDING' }
    }
    if ($Trial.Approved -cne $true -or $Trust.Approved -cne $true -or $Trial.Scope.Mode -cne 'FAKE_ONLY' -or
        $Trial.Scope.NoAi -cne $true -or $Trial.Scope.NoIpc -cne $true -or $Trial.Scope.JobSeconds -ne 90 -or
        $Trial.Scope.SupervisorCount -ne 1 -or $Trial.Scope.ProbeCount -ne 1 -or
        $Trial.FakeOnlyIsolation.Profile -cne 'FAKE_ONLY_EXECUTION_V1' -or
        $Trial.FakeOnlyIsolation.GlobalSidAbsence -cne 'UNKNOWN' -or $Trust.GlobalSidAbsence -cne 'UNKNOWN' -or
        $Trial.FakeOnlyIsolation.TrustEvidenceSha256 -cne (Get-TrialRenewalSha $TrustBytes) -or
        $Trust.BasisSha256 -cne (Get-TrialRenewalSha $BasisBytes) -or $BasisBytes.Length -lt 32 -or
        $Trust.CredentialsNotShared -cne $true -or $Trust.NoUntrustedSameSidCode -cne $true -or
        $Trust.NoOtherLaunchers -cne $true -or $Trust.ResidualRiskAccepted -cne $true) {throw 'HOLD_RENEWAL_DOC_SCOPE'}
    $ends=Read-TrialRenewalUtc $Trial.ExpiresUtc
    $observed=Read-TrialRenewalUtc $Trust.ObservedUtc
    $trustEnd=Read-TrialRenewalUtc $Trust.ExpiresUtc
    if($observed -ge $trustEnd -or $trustEnd -gt $ends -or ($ends-$observed).TotalMinutes -gt 15){
        throw 'HOLD_RENEWAL_DOC_WINDOW'
    }
}

function Resolve-ProtectedTrialHead($Store,$Binding,[Security.Cryptography.RSA]$Verifier,[string]$SignerPin,[DateTimeOffset]$Now) {
    if ((Get-TrialRenewalSha ($Verifier.ExportSubjectPublicKeyInfo())) -cne $SignerPin) {throw 'HOLD_RENEWAL_SIGNER_PIN'}
    $requestRaw=Read-TrialRenewalStream ($Store.ReadOriginalRequest($false)) 8192
    $requestSig=Read-TrialRenewalStream ($Store.ReadOriginalRequest($true)) 4096
    if (!$Verifier.VerifyData($requestRaw,$requestSig,[Security.Cryptography.HashAlgorithmName]::SHA256,
        [Security.Cryptography.RSASignaturePadding]::Pss)) {throw 'HOLD_RENEWAL_ORIGINAL_SIGNATURE'}
    $request=Read-TrialRenewalJson $requestRaw
    $manifestRaw=Read-TrialRenewalStream ($Store.ReadOriginalManifest())
    if ($request.Profile -cne 'INDEPENDENT_ADMIN_DEPLOY_V2' -or $request.Approved -cne $true -or
        $request.ContractSha256 -cnotmatch '^[0-9a-f]{64}$' -or
        $request.ManifestSha256 -cne (Get-TrialRenewalSha $manifestRaw)) {throw 'HOLD_RENEWAL_ORIGINAL_MANIFEST'}
    $consumed=Read-TrialRenewalStream ($Store.ReadOriginalConsumption($request.DeploymentId)) 512
    $line=[string][char]10
    if([Text.UTF8Encoding]::new($false,$true).GetString($consumed) -cne ($request.DeploymentId+$line+$request.ManifestSha256+$line)){
        throw 'HOLD_RENEWAL_ORIGINAL_CONSUMPTION'
    }
    $manifest=Read-TrialRenewalJson $manifestRaw
    if ($manifest.TrialId -cne $request.TrialId -or $manifest.WorkerSid -cne $Binding.Worker -or
        $manifest.TaskPath -cne $Binding.TaskPath -or $manifest.CodeRoot -cne $Binding.CodeRoot -or
        $manifest.EvidenceRoot -cne $Binding.EvidenceRoot) {throw 'HOLD_RENEWAL_ORIGINAL_BINDING'}
    $supervisor=@($manifest.Files | Where-Object Name -CEQ 'Supervisor.exe')
    $probe=@($manifest.Files | Where-Object Name -CEQ 'Probe.exe')
    $trustName=$request.TrialId+'.fake-only-trust.json'
    $basisName=$request.TrialId+'.fake-only-trust-basis.txt'
    $firstTrust=@($manifest.Files | Where-Object Name -CEQ $trustName)
    $firstBasis=@($manifest.Files | Where-Object Name -CEQ $basisName)
    if($supervisor.Count -ne 1 -or $probe.Count -ne 1 -or $firstTrust.Count -ne 1 -or $firstBasis.Count -ne 1){
        throw 'HOLD_RENEWAL_ORIGINAL_FILES'
    }
    $id=[ordered]@{
        SourceRequestSha256=(Get-TrialRenewalSha $requestRaw);TrialId=$request.TrialId;WorkerSid=$Binding.Worker
        TaskPath=$Binding.TaskPath;CodeRoot=$Binding.CodeRoot;EvidenceRoot=$Binding.EvidenceRoot
        SupervisorSha256=$supervisor[0].Sha256;ProbeSha256=$probe[0].Sha256
    }
    $tip=$request.ContractSha256
    $expectedTrust=$firstTrust[0].Sha256
    $expectedBasis=$firstBasis[0].Sha256
    $sequence=0
    $seen=[Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
    while ($Store.CaseExists($tip)){
        if(!$seen.Add($tip) -or ++$sequence -gt 16){throw 'HOLD_RENEWAL_CHAIN'}
        $raw=Read-TrialRenewalStream ($Store.ReadCase($tip,'Renewal.json'))
        $sig=Read-TrialRenewalStream ($Store.ReadCase($tip,'Renewal.sig')) 4096
        $commit=Read-TrialRenewalStream ($Store.ReadCase($tip,'PromotionCommit.json'))
        $r=Read-VerifiedTrialRenewalReceipt $raw $sig $Verifier $SignerPin $tip $id
        Assert-TrialRenewalCommit $r $raw $commit $Now
        if($r.PriorTrustSha256 -cne $expectedTrust -or $r.PriorBasisSha256 -cne $expectedBasis){throw 'HOLD_RENEWAL_CHAIN_PRIOR'}
        foreach($check in @(@('TrialContract.json',$tip),@($trustName,$expectedTrust),@($basisName,$expectedBasis))){
            $prior=Read-TrialRenewalStream ($Store.ReadCase($tip,('Prior-'+$check[0])))
            if((Get-TrialRenewalSha $prior) -cne $check[1]){throw 'HOLD_RENEWAL_ARCHIVE_HASH'}
        }
        $tip=$r.NextTrialSha256
        $expectedTrust=$r.NextTrustSha256
        $expectedBasis=$r.NextBasisSha256
    }
    $trialRaw=Read-TrialRenewalStream ($Store.ReadCode('TrialContract.json')) 4096
    $trustRaw=Read-TrialRenewalStream ($Store.ReadCode($trustName)) 4096
    $basisRaw=Read-TrialRenewalStream ($Store.ReadCode($basisName))
    if ((Get-TrialRenewalSha $trialRaw) -cne $tip -or (Get-TrialRenewalSha $trustRaw) -cne $expectedTrust -or
        (Get-TrialRenewalSha $basisRaw) -cne $expectedBasis) {throw 'HOLD_RENEWAL_CURRENT_HEAD'}
    $trial=Read-TrialRenewalJson $trialRaw
    $trust=Read-TrialRenewalJson $trustRaw
    Assert-TrialRenewalDocuments $trial $trust $trustRaw $basisRaw $id
    if($sequence -gt 0 -and $trial.ExpiresUtc -cne $r.TrialExpiresUtc){throw 'HOLD_RENEWAL_CURRENT_EXPIRY'}
    [pscustomobject]@{Identity=$id;TrialSha=$tip;TrustSha=$expectedTrust;BasisSha=$expectedBasis;Trial=$trial;Trust=$trust;
        TrialRaw=$trialRaw;TrustRaw=$trustRaw;BasisRaw=$basisRaw;TrustName=$trustName;BasisName=$basisName;Depth=$sequence}
}

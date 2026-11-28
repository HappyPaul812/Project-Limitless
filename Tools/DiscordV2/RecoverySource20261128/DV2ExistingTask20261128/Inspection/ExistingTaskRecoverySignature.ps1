function Assert-ExistingTaskRecoverySignature {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)][byte[]]$Payload,
        [Parameter(Mandatory)][byte[]]$Signature,
        [Parameter(Mandatory)][Security.Cryptography.RSA]$Signer,
        [Parameter(Mandatory)][string]$SignerSpkiSha256,
        [Parameter(Mandatory)][System.Collections.IDictionary]$Observed,
        [Parameter(Mandatory)][DateTimeOffset]$Now
    )
    if($Payload.Length -gt 8192 -or $Signature.Length -ne ($Signer.KeySize / 8)){
        throw 'HOLD_TASK_RECOVERY_SIZE'
    }
    $pin=[Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($Signer.ExportSubjectPublicKeyInfo())).ToLowerInvariant()
    if($pin -cne $SignerSpkiSha256 -or !$Signer.VerifyData($Payload,$Signature,[Security.Cryptography.HashAlgorithmName]::SHA256,[Security.Cryptography.RSASignaturePadding]::Pss)){
        throw 'HOLD_TASK_RECOVERY_SIGNATURE'
    }
    $bound=@('TrialId','PriorTrialSha256','NextTrialSha256','PriorSupervisorSha256','NextSupervisorSha256','ProbeSha256',
        'WorkerSid','TaskPath','CodeRoot','EvidenceRoot','TaskFingerprintSha256','RegisterIntentSha256','HoldEvidenceSha256',
        'PriorConsumptionSha256','PriorApprovalId','RegistrationDiagnosticSha256','RecoveryId','NextTrialExpiresUtc')
    $keys=@('Profile','IssuedUtc','ExpiresUtc')+$bound
    $doc=[Text.Json.JsonDocument]::Parse([ReadOnlyMemory[byte]]::new($Payload))
    try {
        $r=$doc.RootElement
        if($r.ValueKind -ne [Text.Json.JsonValueKind]::Object){throw 'HOLD_TASK_RECOVERY_FIELDS'}
        $seen=[Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
        foreach($field in $r.EnumerateObject()){
            if($field.Name -cnotin $keys -or !$seen.Add($field.Name) -or $field.Value.ValueKind -ne [Text.Json.JsonValueKind]::String){
                throw 'HOLD_TASK_RECOVERY_FIELDS'
            }
        }
        if($seen.Count -ne $keys.Count -or $r.GetProperty('Profile').GetString() -cne 'DISCORD_V2_EXISTING_TASK_RECOVERY_V1'){
            throw 'HOLD_TASK_RECOVERY_FIELDS'
        }
        foreach($name in $bound){
            $value=$r.GetProperty($name).GetString()
            if([string]::IsNullOrEmpty($value) -or $value -cne [string]$Observed[$name] -or
                ($name.EndsWith('Sha256') -and $value -cnotmatch '^[0-9a-f]{64}$')){throw 'HOLD_TASK_RECOVERY_BINDING'}
        }
        $issued=[DateTimeOffset]::ParseExact($r.GetProperty('IssuedUtc').GetString(),'o',[Globalization.CultureInfo]::InvariantCulture)
        $expires=[DateTimeOffset]::ParseExact($r.GetProperty('ExpiresUtc').GetString(),'o',[Globalization.CultureInfo]::InvariantCulture)
        $trialEnd=[DateTimeOffset]::ParseExact($r.GetProperty('NextTrialExpiresUtc').GetString(),'o',[Globalization.CultureInfo]::InvariantCulture)
        if($issued -gt $Now -or $Now -ge $expires -or $expires -gt $trialEnd -or ($expires-$issued).TotalMinutes -gt 5){
            throw 'HOLD_TASK_RECOVERY_WINDOW'
        }
        if($r.GetProperty('PriorTrialSha256').GetString() -ceq $r.GetProperty('NextTrialSha256').GetString() -or
            $r.GetProperty('RecoveryId').GetString() -ceq $r.GetProperty('PriorApprovalId').GetString()){
            throw 'HOLD_TASK_RECOVERY_REUSE'
        }
        'EXISTING_TASK_RECOVERY_SIGNATURE_AND_OBSERVATIONS_MATCH'
    } finally {$doc.Dispose()}
}

#Requires -Version 7.0
$ErrorActionPreference='Stop'
Set-StrictMode -Version Latest
$hostPath='C:\Program Files\PowerShell\7\pwsh.exe'
if (-not [Environment]::Is64BitProcess -or (Get-Process -Id $PID).Path -ine $hostPath) { throw 'HOLD_UNAPPROVED_HOST' }
$env:PSModulePath=Join-Path $PSHOME 'Modules'
Import-Module (Join-Path $PSHOME 'Modules/Microsoft.PowerShell.Security/Microsoft.PowerShell.Security.psd1')
function Assert-MicrosoftSigned([string]$file) {
    $signature=Get-AuthenticodeSignature -LiteralPath $file
    if ($signature.Status -ne 'Valid' -or $signature.SignerCertificate.Subject -notmatch '(^|,\s*)O=Microsoft Corporation(,|$)') { throw 'HOLD_MICROSOFT_SIGNATURE' }
}
Assert-MicrosoftSigned $hostPath
$actor=[Security.Principal.WindowsIdentity]::GetCurrent()
try {
    $expected=[Security.Principal.NTAccount]::new([Environment]::MachineName,'windows').Translate([Security.Principal.SecurityIdentifier])
    if (-not $expected.Equals($actor.User) -or -not ([Security.Principal.WindowsPrincipal]::new($actor)).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)) { throw 'HOLD_SIGNER_TOKEN' }
    $sid=$actor.User.Value
} finally { $actor.Dispose() }
$thumb='437423567AD4B703578F231E34A06592B38DB4EB'
$der='7dba5310d6ded0035595e5e7ab8b8b73422f2925b49c633426995c322940672c'
$spki='2a6d0a7d31e7642df209b6cbe12fa628500663f540eb6bd557cac2aca2d31f7c'
$toolOrigin='E:\Windows Kits\10\bin\10.0.26100.0\x64\signtool.exe'
$toolSha='0d0189af3356554a462c2a60d0d59f6006b68d743f5c0cc93c530b083706f075'
function Get-ByteSha([byte[]]$b) {
    [Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($b)).ToLowerInvariant()
}
function Read-PinnedInput([string]$path,[string]$sha) {
    $stream=[IO.File]::Open($path,'Open','Read','Read')
    try {
        if ($stream.Length -lt 1 -or $stream.Length -gt 16777216) { throw 'HOLD_INPUT_SIZE' }
        $b=[byte[]]::new([int]$stream.Length)
        $stream.ReadExactly($b)
        if ((Get-ByteSha $b) -cne $sha) { throw 'HOLD_INPUT_SHA' }
        return ,$b
    } finally { $stream.Dispose() }
}
function Verify-Certificate($cert,[bool]$needPrivateKey) {
    if ($null -eq $cert -or $cert.Thumbprint -cne $thumb -or ($needPrivateKey -and -not $cert.HasPrivateKey)) { throw 'HOLD_CERT_THUMBPRINT_OR_KEY' }
    $rsa=[Security.Cryptography.X509Certificates.RSACertificateExtensions]::GetRSAPublicKey($cert)
    try {
        $now=Get-Date
        $eku=@($cert.Extensions | Where-Object { $_ -is [Security.Cryptography.X509Certificates.X509EnhancedKeyUsageExtension] } |
            ForEach-Object EnhancedKeyUsages | ForEach-Object Value)
        if ($null -eq $rsa -or (Get-ByteSha $cert.RawData) -cne $der -or
            (Get-ByteSha ($rsa.ExportSubjectPublicKeyInfo())) -cne $spki -or $eku -notcontains '1.3.6.1.5.5.7.3.3' -or
            $now -lt $cert.NotBefore -or $now -ge $cert.NotAfter) { throw 'HOLD_CERT_PINS_USAGE_OR_TIME' }
    } finally { if ($rsa) { $rsa.Dispose() } }
}
$cert=Get-Item -LiteralPath "Cert:\LocalMachine\My\$thumb"
try { Verify-Certificate $cert $true } finally { $cert.Dispose() }
$manifest=[Text.Encoding]::UTF8.GetString((Read-PinnedInput (Join-Path $PSScriptRoot '../CandidateEvidence.json') 'a9957496150f16ee589ed0459f9f0a9171a1caf8e86c1c39472d209588e2a127')) | ConvertFrom-Json
if (@($manifest).Count -ne 1 -or $manifest.Name -cne 'Supervisor.exe' -or
    $manifest.Source -cne '5d93b885a0a564ac2ef06b8cbea909e2dc7ff486') { throw 'HOLD_MANIFEST_COUNT_OR_SOURCE' }
$issue=@($manifest)
$inputs=@{}
foreach($i in $issue) {
    $c=@($manifest | Where-Object Name -CEQ $i.Name)
    if ($c.Count -ne 1 -or $i.Sha256 -cne $c[0].Sha256 -or $i.Bytes -ne $c[0].Bytes -or $c[0].Signed) { throw 'HOLD_MANIFEST_DISAGREEMENT' }
    $b=Read-PinnedInput $c[0].Path $i.Sha256
    if ($b.Length -ne $i.Bytes) { throw 'HOLD_LENGTH' }
    $inputs.Add($i.Name,$b)
}
$toolBytes=Read-PinnedInput $toolOrigin $toolSha
Assert-MicrosoftSigned $toolOrigin
$sourcePaths=@((Join-Path $PSScriptRoot 'VerifierStorePreparation.cs'),(Join-Path $PSScriptRoot 'ProductSigningWorkspace.cs'))
$sourcePins=@('eb9d7a09c53e7df4f5fde183ffab6c91172a632298c033a553441b016ded069e','157a6f0c8b03eddf0fa8063e825a977e6c36ecc3a7c6114e31c8432d1740dafa')
$sourceHandles=[Collections.Generic.List[IO.FileStream]]::new()
try {
    for($i=0;$i -lt $sourcePaths.Length;$i++) {
        $h=[IO.File]::Open($sourcePaths[$i],'Open','Read','Read')
        $sourceHandles.Add($h)
        if ([Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($h)).ToLowerInvariant() -cne $sourcePins[$i]) { throw 'HOLD_ADAPTER_SHA' }
    }
    Add-Type -Path $sourcePaths -CompilerOptions '/define:INSTALLER_SIGNING'
} finally { foreach($h in $sourceHandles){$h.Dispose()} }
if ((Read-Host '별도 수정 Supervisor 서명 승인 확인 후 SIGN-RECOVERY-SUPERVISOR 입력') -cne 'SIGN-RECOVERY-SUPERVISOR') { throw 'HOLD_SIGNING_NOT_CONFIRMED' }
$now=[DateTimeOffset]::UtcNow
$end=$now.AddMinutes(5)
$space=[LimitlessSupervisorRecoverySigning20261128+SigningWorkspace]::new($sid,$now,$end,$false)
try {
    $toolName='signtool.exe'
    $space.CopyVerifiedBytes($toolName,$toolBytes)
    foreach($i in $issue) { $space.CopyVerifiedBytes($i.Name,$inputs[$i.Name]) }
    $tool=Join-Path $space.DirectoryPath $toolName
    function Invoke-Tool([string[]]$toolArgs,[string]$failure) {
        $procInfo=[Diagnostics.ProcessStartInfo]::new()
        $procInfo.FileName=$tool
        $procInfo.WorkingDirectory=$space.DirectoryPath
        $procInfo.UseShellExecute=$false
        $procInfo.Environment['PATH']=[Environment]::SystemDirectory
        foreach($arg in $toolArgs) { $procInfo.ArgumentList.Add($arg) }
        $proc=[Diagnostics.Process]::Start($procInfo)
        if ($null -eq $proc) { throw $failure }
        try { $proc.WaitForExit(); if ($proc.ExitCode -ne 0) { throw $failure } } finally { $proc.Dispose() }
    }
    function Get-HandleSha($stream) {
        $stream.Position=0
        [Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($stream)).ToLowerInvariant()
    }
    $toolHandle=$space.OpenVerifiedFile($toolName)
    try {
        if ((Get-HandleSha $toolHandle) -cne $toolSha) { throw 'HOLD_PROTECTED_SIGNTOOL_SHA' }
        Assert-MicrosoftSigned $tool
        $results=foreach($i in $issue) {
            $r=[ordered]@{Name=$i.Name;UnsignedSha256=$i.Sha256;UnsignedBytes=$i.Bytes;Status='HOLD';Attempted=$false}
            try {
                $path=Join-Path $space.DirectoryPath $i.Name
                $unsigned=$space.OpenVerifiedFile($i.Name)
                try {
                    if ((Get-HandleSha $unsigned) -cne $i.Sha256 -or (Get-AuthenticodeSignature -LiteralPath $path).Status -ne 'NotSigned') { throw 'HOLD_PROTECTED_UNSIGNED' }
                } finally { $unsigned.Dispose() }
                $space.Check()
                if ([DateTimeOffset]::UtcNow -ge $end) { throw 'HOLD_SIGN_WINDOW' }
                $space.MarkSigningAttempt($i.Name)
                $r.Attempted=$true
                Invoke-Tool @('sign','/sm','/s','My','/sha1',$thumb,'/fd','SHA256',$path) 'HOLD_SIGN_NO_RETRY'
                $signed=$space.OpenVerifiedFile($i.Name)
                try {
                    Invoke-Tool @('verify','/pa','/all',$path) 'HOLD_SIGNTOOL_VERIFY'
                    $sig=Get-AuthenticodeSignature -LiteralPath $path
                    if ($sig.Status -ne 'Valid') { throw 'HOLD_AUTHENTICODE' }
                    Verify-Certificate $sig.SignerCertificate $false
                    $after=Get-HandleSha $signed
                    if ($after -ceq $i.Sha256) { throw 'HOLD_SIGNED_IMAGE_UNCHANGED' }
                    $space.Check()
                    $r.SignedSha256=$after
                    $r.SignedBytes=$signed.Length
                    $r.CertificateThumbprint=$thumb
                    $r.CertificateDerSha256=$der
                    $r.RsaSpkiSha256=$spki
                    $r.CertificateNotBeforeUtc=$sig.SignerCertificate.NotBefore.ToUniversalTime().ToString('o')
                    $r.CertificateNotAfterUtc=$sig.SignerCertificate.NotAfter.ToUniversalTime().ToString('o')
                    $r.SignTool='PASS'
                    $r.Authenticode='Valid'
                    $r.Status='PASS'
                } finally { $signed.Dispose() }
            } catch {
                $message=$_.Exception.GetBaseException().Message
                $r.Reason=if($message -match '^HOLD_[A-Z_]+$'){$message}else{'HOLD_NATIVE_SIGNING_OR_READBACK'}
            }
            [pscustomobject]$r
        }
    } finally { $toolHandle.Dispose() }
    $report=[ordered]@{Schema='LIMITLESS_DISCORD_V2_PRODUCT_SIGNING_MANIFEST_V1';SourceCommit=$manifest.Source;WorkspaceReceipt=$space.Receipt;Results=@($results)}
    $manifestName='ProductSigningManifest.json'
    $reportBytes=[Text.UTF8Encoding]::new($false).GetBytes(($report | ConvertTo-Json -Depth 6))
    $space.CopyVerifiedBytes($manifestName,$reportBytes)
    $readback=$space.OpenVerifiedFile($manifestName)
    try { if((Get-HandleSha $readback) -cne (Get-ByteSha $reportBytes)){throw 'HOLD_MANIFEST_READBACK'} }
    finally { $readback.Dispose() }
    $results | Select-Object Name,Status,Attempted,SignedSha256,Reason | ConvertTo-Json -Depth 4
    "PRODUCT_SIGNING_MANIFEST=$(Join-Path $space.DirectoryPath $manifestName)"
    if (@($results | Where-Object Status -NE 'PASS').Count) { 'PRODUCT_SIGNING_PARTIAL_HOLD_NO_RETRY' } else { 'RECOVERY_SUPERVISOR_SIGNED_VERIFIED' }
} finally { $space.Dispose() }

#Requires -Version 7.0
[CmdletBinding()]
param(
    [Parameter(Mandatory)][string]$ApprovalSha,
    [Parameter(Mandatory)][string]$ApprovalSignatureSha,
    [switch]$InstallAfterApproval
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
if (-not $InstallAfterApproval) { throw 'HOLD_INSTALL_NOT_APPROVED' }
foreach ($digest in @($ApprovalSha,$ApprovalSignatureSha)) {
    if ($digest -cnotmatch '^[a-f0-9]{64}$') { throw 'HOLD_INSTALL_APPROVAL_SHA_FORMAT' }
}

$hostPath = 'C:\Program Files\PowerShell\7\pwsh.exe'
$runtimeRoot = 'C:\Program Files\dotnet'
$runtimeVersion = '10.0.14'
$path = 'C:\ProgramData\LimitlessDiscordV2InstallerRuntime\Limitless.InitialStagingInstaller.exe'
$expectedResult = 'STAGE_CREATED_AND_READBACK'
$expectedImage = '52694e574cee3068e0c84131dcd9b93d19a2b59a42a63cf322c87a596c91806d'
$thumb = '437423567AD4B703578F231E34A06592B38DB4EB'
$derSha = '7dba5310d6ded0035595e5e7ab8b8b73422f2925b49c633426995c322940672c'
$spkiSha = '2a6d0a7d31e7642df209b6cbe12fa628500663f540eb6bd557cac2aca2d31f7c'
$systemSid = 'S-1-5-18'
$adminSid = 'S-1-5-32-544'
$trustedOwners = @($systemSid, $adminSid, 'S-1-5-80-956008885-3418522649-1831038044-1853292631-2271478464')
$sidType = [Security.Principal.SecurityIdentifier]

if (-not [Environment]::Is64BitProcess -or (Get-Process -Id $PID).Path -ine $hostPath) { throw 'HOLD_UNAPPROVED_HOST' }
$env:PSModulePath = Join-Path $PSHOME 'Modules'
Import-Module (Join-Path $PSHOME 'Modules\Microsoft.PowerShell.Security\Microsoft.PowerShell.Security.psd1')
$id = [Security.Principal.WindowsIdentity]::GetCurrent()
try {
    $expectedSid = ([Security.Principal.NTAccount]::new([Environment]::MachineName, 'windows')).Translate($sidType)
    if (-not $expectedSid.Equals($id.User) -or
        -not ([Security.Principal.WindowsPrincipal]::new($id)).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)) {
        throw 'HOLD_INSTALL_TOKEN'
    }
    $actorSid = $id.User.Value
} finally { $id.Dispose() }

function Get-StreamSha([IO.Stream]$stream) {
    $stream.Position = 0
    [Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($stream)).ToLowerInvariant()
}

function Assert-NoReparse([string]$file) {
    for ($item = Get-Item -LiteralPath $file -Force; $null -ne $item; ) {
        if (($item.Attributes -band [IO.FileAttributes]::ReparsePoint) -ne 0) { throw 'HOLD_PATH_REPARSE' }
        if ($item -is [IO.FileInfo]) { $item = $item.Directory } else { $item = $item.Parent }
    }
}

function Assert-ProtectedImageAcl([string]$file) {
    $acl = Get-Acl -LiteralPath $file
    $entries = @($acl.GetAccessRules($true, $true, $sidType))
    if ($acl.GetOwner($sidType).Value -ne $adminSid -or $acl.GetGroup($sidType).Value -ne $systemSid -or
        -not $acl.AreAccessRulesProtected -or $entries.Count -ne 2) { throw 'HOLD_DEPLOYMENT_ACL' }
    foreach ($sid in @($systemSid,$adminSid)) {
        $match = @($entries | Where-Object { $_.IdentityReference.Value -eq $sid -and
            $_.AccessControlType -eq 'Allow' -and -not $_.IsInherited -and
            $_.InheritanceFlags -eq 'None' -and $_.PropagationFlags -eq 'None' -and
            [int64]$_.FileSystemRights -eq 0x1F01FF })
        if ($match.Count -ne 1) { throw 'HOLD_DEPLOYMENT_ACL' }
    }
}

function Assert-TrustedChain([string]$file, [int64]$denyMask, [int64]$ancestorMask=$denyMask) {
    Assert-NoReparse $file
    $mask = $denyMask
    for ($item = Get-Item -LiteralPath $file -Force; $null -ne $item; ) {
        $acl = Get-Acl -LiteralPath $item.FullName
        if ($acl.GetOwner($sidType).Value -notin $trustedOwners) { throw 'HOLD_TRUSTED_PATH_OWNER' }
        foreach ($entry in $acl.GetAccessRules($true, $true, $sidType)) {
            if ($entry.AccessControlType -eq 'Allow' -and
                ($entry.PropagationFlags -band [Security.AccessControl.PropagationFlags]::InheritOnly) -eq 0 -and
                $entry.IdentityReference.Value -notin $trustedOwners -and
                ([int64]$entry.FileSystemRights -band $mask) -ne 0) { throw 'HOLD_TRUSTED_PATH_WRITE' }
        }
        if ($item -is [IO.FileInfo]) { $item = $item.Directory } else { $item = $item.Parent }
        $mask = $ancestorMask
    }
}

function Assert-MicrosoftFile([string]$file, [string]$expectedHash) {
    Assert-TrustedChain $file 0x100D0156 0x100D0040
    $fileLock = [IO.File]::Open($file, 'Open', 'Read', 'Read')
    try {
        $sig = Get-AuthenticodeSignature -LiteralPath $file
        if ((Get-StreamSha $fileLock) -cne $expectedHash -or $sig.Status -ne 'Valid' -or
            $sig.SignerCertificate.Subject -notmatch '(^|,\s*)O=Microsoft Corporation(,|$)') {
            throw 'HOLD_MICROSOFT_FILE'
        }
    } finally { $fileLock.Dispose() }
}

Assert-MicrosoftFile $hostPath 'e94166ed9f376559c1359bafb08bc49e463a494c330f67644ec1d9f1ffeb5ee5'
$fxr = Join-Path $runtimeRoot 'host\fxr'
$shared = Join-Path $runtimeRoot 'shared\Microsoft.NETCore.App'
foreach ($versions in @($fxr,$shared)) {
    Assert-TrustedChain $versions 0x100D0156 0x100D0040
    $dirs = @(Get-ChildItem -LiteralPath $versions -Directory -Force)
    if ($dirs.Count -ne 1 -or $dirs[0].Name -cne $runtimeVersion) { throw 'HOLD_RUNTIME_VERSION_SET' }
    Assert-TrustedChain $dirs[0].FullName 0x100D0156 0x100D0040
}
$runtimePath = Join-Path $shared $runtimeVersion
Assert-MicrosoftFile (Join-Path $fxr "$runtimeVersion\hostfxr.dll") '5b9358572daa76176f97f1bb80fe764b24ee1bc2cce0579230e7f9a28e3fbbed'
Assert-MicrosoftFile (Join-Path $runtimePath 'coreclr.dll') '1bc65b8d45474b19adf3ca85ce881087aff599ffc72b2c1a323e878227757b65'
Assert-MicrosoftFile (Join-Path $runtimePath 'hostpolicy.dll') '84275dc570382246b4b36ebb3094c2b38190c2fd80998d1da20b95dfcae1c546'

Assert-TrustedChain $path 0x100D0156 0x100D0040
$parent = Split-Path -Parent $path
Assert-ProtectedImageAcl $parent
$image = [IO.File]::Open($path, 'Open', 'Read', 'Read')
try {
    Assert-ProtectedImageAcl $path
    if ((Get-StreamSha $image) -cne $expectedImage) { throw 'HOLD_DEPLOYED_IMAGE_SHA' }
    $sig = Get-AuthenticodeSignature -LiteralPath $path
    $leaf = $sig.SignerCertificate
    if ($sig.Status -ne 'Valid' -or $null -eq $leaf -or $leaf.Thumbprint -cne $thumb) { throw 'HOLD_DEPLOYED_IMAGE_SIGNATURE' }
    $rsa = [Security.Cryptography.X509Certificates.RSACertificateExtensions]::GetRSAPublicKey($leaf)
    try {
        $eku = @($leaf.Extensions | Where-Object { $_ -is [Security.Cryptography.X509Certificates.X509EnhancedKeyUsageExtension] } |
            ForEach-Object EnhancedKeyUsages | ForEach-Object Value)
        $now = Get-Date
        if ($null -eq $rsa -or
            $leaf.GetCertHashString([Security.Cryptography.HashAlgorithmName]::SHA256).ToLowerInvariant() -cne $derSha -or
            [Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($rsa.ExportSubjectPublicKeyInfo())).ToLowerInvariant() -cne $spkiSha -or
            $eku -notcontains '1.3.6.1.5.5.7.3.3' -or $now -lt $leaf.NotBefore -or $now -ge $leaf.NotAfter) {
            throw 'HOLD_DEPLOYED_SIGNER'
        }
    } finally { if ($null -ne $rsa) { $rsa.Dispose() } }
    $approvalPath = Join-Path $parent 'InstallApproval.json'
    $approvalSignaturePath = Join-Path $parent 'InstallApproval.sig'
    Assert-TrustedChain $approvalPath 0x100D0156 0x100D0040
    Assert-TrustedChain $approvalSignaturePath 0x100D0156 0x100D0040
    $approvalFile = [IO.File]::Open($approvalPath, 'Open', 'Read', 'Read')
    try {
        $approvalSignatureFile = [IO.File]::Open($approvalSignaturePath, 'Open', 'Read', 'Read')
        try {
            Assert-ProtectedImageAcl $approvalPath
            Assert-ProtectedImageAcl $approvalSignaturePath
            if ($approvalFile.Length -lt 1 -or $approvalFile.Length -gt 8192 -or
                $approvalSignatureFile.Length -lt 1 -or $approvalSignatureFile.Length -gt 4096) {
                throw 'HOLD_INSTALL_APPROVAL_INPUT_SIZE'
            }
            $approvalBytes = [byte[]]::new([int]$approvalFile.Length)
            $approvalSignatureBytes = [byte[]]::new([int]$approvalSignatureFile.Length)
            $approvalFile.ReadExactly($approvalBytes)
            $approvalSignatureFile.ReadExactly($approvalSignatureBytes)
            if ([Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($approvalBytes)).ToLowerInvariant() -cne $ApprovalSha -or
                [Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($approvalSignatureBytes)).ToLowerInvariant() -cne $ApprovalSignatureSha) {
                throw 'HOLD_INSTALL_APPROVAL_SHA'
            }
            $signer = [Security.Cryptography.X509Certificates.RSACertificateExtensions]::GetRSAPublicKey($leaf)
            if ($null -eq $signer) { throw 'HOLD_INSTALL_APPROVAL_SIGNER' }
            try {
                if (-not $signer.VerifyData($approvalBytes,$approvalSignatureBytes,
                    [Security.Cryptography.HashAlgorithmName]::SHA256,[Security.Cryptography.RSASignaturePadding]::Pss)) {
                    throw 'HOLD_INSTALL_APPROVAL_SIGNATURE'
                }
            } finally { $signer.Dispose() }
            $doc = [Text.Json.JsonDocument]::Parse([ReadOnlyMemory[byte]]::new($approvalBytes))
            try {
                $json = $doc.RootElement
                $scope = $json.GetProperty('Approval')
                $issued = $scope.GetProperty('NotBefore').GetDateTimeOffset()
                $expires = $scope.GetProperty('Expires').GetDateTimeOffset()
                $utc = [DateTimeOffset]::UtcNow
                if ($json.GetProperty('InstallerSha256').GetString() -cne $expectedImage -or
                    $scope.GetProperty('Profile').GetString() -cne 'DISCORD_V2_INITIAL_STAGING_INSTALL_V1' -or
                    $scope.GetProperty('AdministratorSid').GetString() -cne $actorSid -or
                    $issued.Offset -ne [TimeSpan]::Zero -or $expires.Offset -ne [TimeSpan]::Zero -or
                    $utc -lt $issued -or $utc -ge $expires -or ($expires - $issued).TotalMinutes -gt 5) {
                    throw 'HOLD_INSTALL_APPROVAL_WINDOW_OR_SCOPE'
                }
            } finally { $doc.Dispose() }

            $info = [Diagnostics.ProcessStartInfo]::new()
            $info.FileName = $path
            $info.WorkingDirectory = $parent
            $info.UseShellExecute = $false
            $info.CreateNoWindow = $true
            $info.RedirectStandardOutput = $true
            $info.RedirectStandardError = $true
            $info.ArgumentList.Add('--install')
            # Start with a clean runtime environment; no startup hooks, profiler, or user runtime overrides.
            $info.Environment.Clear()
            $info.Environment['SystemRoot'] = 'C:\Windows'
            $info.Environment['WINDIR'] = 'C:\Windows'
            $info.Environment['SystemDrive'] = 'C:'
            $info.Environment['PATH'] = 'C:\Windows\System32'
            $info.Environment['DOTNET_ROOT'] = $runtimeRoot
            $info.Environment['DOTNET_ROOT_X64'] = $runtimeRoot
            $info.Environment['DOTNET_ROLL_FORWARD'] = 'LatestPatch'
            $proc = [Diagnostics.Process]::Start($info)
            if ($null -eq $proc) { throw 'HOLD_INSTALL_START' }
            try {
                $stdoutTask = $proc.StandardOutput.ReadToEndAsync()
                $stderrTask = $proc.StandardError.ReadToEndAsync()
                if (-not $proc.WaitForExit(20000)) { $proc.Kill($true); throw 'HOLD_INSTALL_TIMEOUT_NO_RETRY' }
                $stdout = $stdoutTask.GetAwaiter().GetResult().Trim()
                $stderr = $stderrTask.GetAwaiter().GetResult().Trim()
                if ($proc.ExitCode -ne 0 -or $stdout -cne $expectedResult -or $stderr.Length -ne 0) {
                    if ($stderr -match '^HOLD [A-Z_]+$') { throw $stderr }
                    throw ("HOLD_INSTALL_RESULT_EXIT_{0}" -f $proc.ExitCode)
                }
                'STAGE_CREATED_AND_READBACK'
            } finally { $proc.Dispose() }
        } finally { $approvalSignatureFile.Dispose() }
    } finally { $approvalFile.Dispose() }
} finally { $image.Dispose() }

#Requires -Version 7.0
[CmdletBinding()]
param(
    [Parameter(Mandatory)][string]$ApprovedSignerSid,
    [Parameter(Mandatory)][string]$CandidatePath
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

# SID는 별도의 로컬 승인 기록에서 확인합니다. 계정 이름이나 관리자 역할로 대체하지 않습니다.
$expectedSid = [Security.Principal.SecurityIdentifier]::new($ApprovedSignerSid)
$identity = [Security.Principal.WindowsIdentity]::GetCurrent()
try {
    if ($null -eq $identity.User -or -not $identity.User.Equals($expectedSid)) {
        throw 'HOLD_SIGNER_SID_MISMATCH'
    }
} finally { $identity.Dispose() }

$expectedHash = '145F3921E88AB2CABA98AFD4C5BB39EB7F9706AA6AD5E3FCA5D5891F9F73ADBB'
$thumb = '437423567AD4B703578F231E34A06592B38DB4EB'
$derSha = '7DBA5310D6DED0035595E5E7AB8B8B73422F2925B49C633426995C322940672C'
$spkiSha = '2A6D0A7D31E7642DF209B6CBE12FA628500663F540EB6BD557CAC2ACA2D31F7C'
if ((Get-FileHash -LiteralPath $CandidatePath -Algorithm SHA256).Hash -cne $expectedHash) {
    throw 'HOLD_CANDIDATE_SHA'
}

if (-not ('LimitlessSigningKeyLink' -as [type])) {
    Add-Type -TypeDefinition @"
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

public static class LimitlessSigningKeyLink
{
    [StructLayout(LayoutKind.Sequential)]
    private struct KeyProviderInfo
    {
        public IntPtr Container, Provider;
        public uint Type, Flags, ParameterCount;
        public IntPtr Parameters;
        public uint KeySpec;
    }

    [DllImport("crypt32.dll", SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool CertGetCertificateContextProperty(
        IntPtr certificate, uint property, IntPtr data, ref uint length);

    public static string ReadMachineContainer(IntPtr certificate)
    {
        const uint KeyProviderProperty = 2, MachineKey = 0x20;
        uint length = 0;
        if (!CertGetCertificateContextProperty(certificate, KeyProviderProperty, IntPtr.Zero, ref length))
            throw new Win32Exception(Marshal.GetLastWin32Error());
        if (length < Marshal.SizeOf<KeyProviderInfo>() || length > 65536)
            throw new InvalidOperationException("HOLD_KEY_PROVIDER_PROPERTY");
        IntPtr buffer = Marshal.AllocHGlobal((int)length);
        try
        {
            if (!CertGetCertificateContextProperty(certificate, KeyProviderProperty, buffer, ref length))
                throw new Win32Exception(Marshal.GetLastWin32Error());
            var info = Marshal.PtrToStructure<KeyProviderInfo>(buffer);
            string provider = Marshal.PtrToStringUni(info.Provider);
            string container = Marshal.PtrToStringUni(info.Container);
            if (info.Type != 0 || info.KeySpec != 0 || (info.Flags & MachineKey) == 0 ||
                provider != "Microsoft Software Key Storage Provider" || string.IsNullOrWhiteSpace(container))
                throw new InvalidOperationException("HOLD_KEY_PROVIDER_OR_SCOPE");
            return container;
        }
        finally { Marshal.FreeHGlobal(buffer); }
    }
}
"@
}

$certificate = Get-Item -LiteralPath "Cert:\LocalMachine\My\$thumb"
try {
    $eku = @($certificate.Extensions |
        Where-Object { $_ -is [Security.Cryptography.X509Certificates.X509EnhancedKeyUsageExtension] } |
        ForEach-Object { $_.EnhancedKeyUsages } | ForEach-Object { $_.Value })
    $now = Get-Date
    if ($certificate.Thumbprint -cne $thumb -or -not $certificate.HasPrivateKey -or
        $certificate.GetCertHashString([Security.Cryptography.HashAlgorithmName]::SHA256) -cne $derSha -or
        $eku -notcontains '1.3.6.1.5.5.7.3.3' -or
        $now -lt $certificate.NotBefore -or $now -ge $certificate.NotAfter) {
        throw 'HOLD_SIGNING_CERTIFICATE'
    }

    $container = [LimitlessSigningKeyLink]::ReadMachineContainer($certificate.Handle)
    $provider = [Security.Cryptography.CngProvider]::MicrosoftSoftwareKeyStorageProvider
    $options = [Security.Cryptography.CngKeyOpenOptions]::MachineKey -bor [Security.Cryptography.CngKeyOpenOptions]::Silent
    try { $key = [Security.Cryptography.CngKey]::Open($container, $provider, $options) }
    catch {
        throw ('HOLD_KEY_OPEN_0x{0:X8}' -f $_.Exception.GetBaseException().HResult)
    }
    try {
        if ($key.ExportPolicy -ne [Security.Cryptography.CngExportPolicies]::None) {
            throw 'HOLD_KEY_EXPORT_POLICY'
        }
        $rsa = [Security.Cryptography.RSACng]::new($key)
        try {
            # 내보내는 것은 인증서와 대조할 공개 SPKI의 메모리 바이트뿐입니다.
            $sha = [Convert]::ToHexString(
                [Security.Cryptography.SHA256]::HashData($rsa.ExportSubjectPublicKeyInfo()))
            if ($sha -cne $spkiSha) { throw 'HOLD_KEY_SPKI_MISMATCH' }
        } finally { $rsa.Dispose() }
    } finally { $key.Dispose() }
} finally { $certificate.Dispose() }

'PASS_SIGNER_SID_KEY_SPKI_READ_ONLY'

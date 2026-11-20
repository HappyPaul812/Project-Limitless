using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

internal static partial class StageSecurityApi
{
    private const string ApprovedCertificateSha = "7dba5310d6ded0035595e5e7ab8b8b73422f2925b49c633426995c322940672c";
    private const string CodeSigningOid = "1.3.6.1.5.5.7.3.3";
    private static readonly Guid GenericVerify = new("00aac56b-cd44-11d0-8cc2-00c04fc295ee");

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    private struct TrustFile
    {
        internal uint Size;
        [MarshalAs(UnmanagedType.LPWStr)] internal string Path;
        internal IntPtr Handle;
        internal IntPtr Subject;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct TrustData
    {
        internal uint Size;
        internal IntPtr PolicyCallback, SipClient;
        internal uint Ui, Revocation, Choice;
        internal IntPtr File;
        internal uint StateAction;
        internal IntPtr State, Url;
        internal uint Flags, UiContext;
        internal IntPtr SignatureSettings;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct ProviderCertificatePrefix
    {
        internal uint Size;
        internal IntPtr Context;
    }

    [DllImport("wintrust.dll", ExactSpelling = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    private static extern int WinVerifyTrust(IntPtr window, in Guid action, ref TrustData data);

    [DllImport("wintrust.dll", ExactSpelling = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    private static extern IntPtr WTHelperProvDataFromStateData(IntPtr state);

    [DllImport("wintrust.dll", ExactSpelling = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    private static extern IntPtr WTHelperGetProvSignerFromChain(IntPtr data, uint index, int counterSigner, uint counterIndex);

    [DllImport("wintrust.dll", ExactSpelling = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    private static extern IntPtr WTHelperGetProvCertFromChain(IntPtr signer, uint index);

    // Windows 정책 결과의 실제 서명자에서 공개키를 얻습니다. 별도 입력 키나 자체 서명만으로는 통과하지 않습니다.
    internal static byte[] VerifyAuthenticode(FileStream source, string certificateSha, string publicKeySha)
    {
        BootstrapPolicy.Need(BootstrapPolicy.Sha(certificateSha) && BootstrapPolicy.Sha(publicKeySha), "STAGE_TRUST_HOLD");
        var handle = source.SafeFileHandle;
        var file = new TrustFile { Size = (uint)Marshal.SizeOf<TrustFile>(), Path = source.Name, Handle = handle.DangerousGetHandle() };
        IntPtr filePointer = Marshal.AllocHGlobal(Marshal.SizeOf<TrustFile>());
        try
        {
            Marshal.StructureToPtr(file, filePointer, false);
            try
            {
                var data = new TrustData
                {
                    Size = (uint)Marshal.SizeOf<TrustData>(),
                    Ui = 2, // WTD_UI_NONE
                    Choice = 1, // WTD_CHOICE_FILE
                    File = filePointer,
                    StateAction = 1, // WTD_STATEACTION_VERIFY
                    Flags = 0x1000 // WTD_CACHE_ONLY_URL_RETRIEVAL
                };
                try
                {
                    BootstrapPolicy.Need(WinVerifyTrust(new IntPtr(-1), in GenericVerify, ref data) == 0,
                        "STAGE_AUTHENTICODE");
                    IntPtr provider = WTHelperProvDataFromStateData(data.State);
                    BootstrapPolicy.Need(provider != IntPtr.Zero, "STAGE_AUTHENTICODE_SIGNER");
                    IntPtr signer = WTHelperGetProvSignerFromChain(provider, 0, 0, 0);
                    BootstrapPolicy.Need(signer != IntPtr.Zero, "STAGE_AUTHENTICODE_SIGNER");
                    IntPtr certificate = WTHelperGetProvCertFromChain(signer, 0);
                    BootstrapPolicy.Need(certificate != IntPtr.Zero, "STAGE_AUTHENTICODE_SIGNER");
                    IntPtr context = Marshal.PtrToStructure<ProviderCertificatePrefix>(certificate).Context;
                    BootstrapPolicy.Need(context != IntPtr.Zero, "STAGE_AUTHENTICODE_SIGNER");
                    using var leaf = new X509Certificate2(context);
                    using RSA? rsa = leaf.GetRSAPublicKey();
                    byte[] key = rsa?.ExportSubjectPublicKeyInfo() ?? [];
                    ValidateLocalSigner(
                        BootstrapPolicy.Hash(leaf.RawData), certificateSha,
                        BootstrapPolicy.Hash(key), publicKeySha,
                        leaf.Extensions.OfType<X509EnhancedKeyUsageExtension>().Any(
                            extension => extension.EnhancedKeyUsages.Cast<Oid>().Any(oid => oid.Value == CodeSigningOid)),
                        DateTimeOffset.UtcNow, leaf.NotBefore, leaf.NotAfter);
                    return key;
                }
                finally
                {
                    data.StateAction = 2; // WTD_STATEACTION_CLOSE
                    WinVerifyTrust(new IntPtr(-1), in GenericVerify, ref data);
                    GC.KeepAlive(handle);
                }
            }
            finally { Marshal.DestroyStructure<TrustFile>(filePointer); }
        }
        finally { Marshal.FreeHGlobal(filePointer); }
    }

    internal static void ValidateLocalSigner(string actualCertificateSha, string certificateSha,
        string actualKeySha, string publicKeySha, bool codeSigning, DateTimeOffset now,
        DateTimeOffset notBefore, DateTimeOffset notAfter)
    {
        BootstrapPolicy.Need(BootstrapPolicy.Sha(certificateSha) && actualCertificateSha == certificateSha
            && BootstrapPolicy.Sha(publicKeySha) && actualKeySha == publicKeySha, "STAGE_SIGNER_PIN");
        BootstrapPolicy.Need(codeSigning && now >= notBefore && now < notAfter, "STAGE_SIGNER_USAGE_OR_TIME");
    }
}

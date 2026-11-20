using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Win32.SafeHandles;

internal static partial class StageSecurityApi
{
    internal const string InitialLedger = @"C:\ProgramData\LimitlessDiscordV2BootstrapLedger";
    internal const string Profile = "DISCORD_V2_INITIAL_STAGING_V2";

    // 승인된 릴리스에서만 독립 배포 채널의 공개키 SHA를 봉인합니다.
    // 소스 트리 또는 명령행으로 운영 trust anchor를 설정하는 경로는 없습니다.
    private const string ApprovedSignerSha = "";

    internal sealed record InstallationApproval(
        string Profile, string ApprovalId, string Stage, string Ledger, string LedgerKey,
        string AdministratorSid, DateTimeOffset NotBefore, DateTimeOffset Expires,
        string SourceSha, string[] ParentKeys, string[] ParentSecurityHashes);

    [DllImport("kernel32.dll", ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern SafeFileHandle CreateFileW(string path, uint access, uint share,
        ref SecurityAttributes security, uint disposition, uint flags, IntPtr template);

    // 독립 검증기의 실행 허용은 OS 배포 정책에서 이 호출 이전에 적용해야 합니다.
    // 이 빌드는 운영 키 미승인 및 기존 계약 만료 상태로 어떤 Windows 변경도 허용하지 않습니다.
    internal static string InstallFromApproval(string sourcePath, byte[] approvalBytes,
        byte[] approvalSignature, byte[] sourceSignature, byte[] publicKey)
    {
        BootstrapPolicy.Need(BootstrapPolicy.Sha(ApprovedSignerSha), "STAGE_TRUST_HOLD");
        DemandTime(DateTimeOffset.UtcNow, BootstrapPolicy.Cutoff);
        using var host = new WindowsBootstrapHost();
        using var source = new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        return Admit(source, approvalBytes, approvalSignature, sourceSignature, publicKey,
            ApprovedSignerSha, BootstrapPolicy.Cutoff, () => DateTimeOffset.UtcNow,
            approval => { ObserveWindows(host, approval); },
            approval => ConsumeWindowsApproval(host, approval),
            approval => CreateAndReadBack(host, approval));
    }

    // 이 연결 경계는 승인 원문과 검증된 스크립트를 데이터로만 읽습니다.
    // 파일 시스템 변경은 사전 검사, 영구 소비, 재검사를 모두 거쳐야 합니다.
    internal static string Admit(Stream source, byte[] approvalBytes, byte[] approvalSignature,
        byte[] sourceSignature, byte[] publicKey, string signerSha, DateTimeOffset contractCutoff,
        Func<DateTimeOffset> now, Action<InstallationApproval> observe,
        Action<InstallationApproval> consume, Action<InstallationApproval> create)
    {
        BootstrapPolicy.Need(BootstrapPolicy.Sha(signerSha) && publicKey.Length is > 0 and <= 4096, "STAGE_TRUST_HOLD");
        var key = (byte[])publicKey.Clone();
        BootstrapPolicy.Need(BootstrapPolicy.Hash(key) == signerSha, "STAGE_TRUST_HOLD");
        BootstrapPolicy.Need(approvalBytes.Length is > 0 and <= 8192, "STAGE_APPROVAL_SIZE");
        using var signer = RSA.Create();
        signer.ImportSubjectPublicKeyInfo(key, out int read);
        var signedApproval = (byte[])approvalBytes.Clone();
        BootstrapPolicy.Need(read == key.Length && signer.KeySize >= 2048
            && signer.VerifyData(signedApproval, approvalSignature, HashAlgorithmName.SHA256, RSASignaturePadding.Pss),
            "STAGE_APPROVAL_SIGNATURE");
        using var document = JsonDocument.Parse(signedApproval);
        var fields = new HashSet<string>(StringComparer.Ordinal);
        foreach (var property in document.RootElement.EnumerateObject())
            BootstrapPolicy.Need(fields.Add(property.Name), "STAGE_APPROVAL_FIELDS");
        var approval = document.RootElement.Deserialize<InstallationApproval>(new JsonSerializerOptions
        {
            UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow
        }) ?? throw new InvalidOperationException("STAGE_APPROVAL_FIELDS");
        BootstrapPolicy.Need(approval.Profile == Profile && approval.Stage == Stage
            && approval.Ledger == InitialLedger && !string.IsNullOrWhiteSpace(approval.LedgerKey)
            && !string.IsNullOrWhiteSpace(approval.AdministratorSid)
            && IsApprovalId(approval.ApprovalId) && approval.SourceSha is not null
            && approval.ParentKeys?.Length == BootstrapPolicy.Ancestors.Count
            && approval.ParentKeys.All(k => !string.IsNullOrWhiteSpace(k))
            && approval.ParentSecurityHashes?.Length == BootstrapPolicy.Ancestors.Count
            && approval.ParentSecurityHashes.All(h => h is not null && BootstrapPolicy.Sha(h)),
            "STAGE_APPROVAL_SCOPE");
        void CheckWindow()
        {
            var time = now();
            BootstrapPolicy.Need(approval.NotBefore != default && time >= approval.NotBefore
                && time < approval.Expires && approval.Expires <= contractCutoff, "STAGE_EXPIRED");
        }
        CheckWindow();
        _ = VerifySignedSource(source, approval.SourceSha!, signer, sourceSignature);
        observe(approval);
        CheckWindow();
        consume(approval);
        observe(approval);
        CheckWindow();
        create(approval);
        return "STAGE_CREATED_AND_READBACK";
    }

    private static bool IsApprovalId(string? input)
        => Guid.TryParseExact(input, "N", out var id) && id != Guid.Empty && id.ToString("N") == input;

    private static ulong ObserveWindows(WindowsBootstrapHost host, InstallationApproval approval)
    {
        DemandTime(DateTimeOffset.UtcNow, approval.Expires);
        BootstrapPolicy.Need(host.FixedNtfsVolume && WindowsBootstrapHost.Administrator() == approval.AdministratorSid,
            "STAGE_ACTOR_OR_VOLUME");
        ulong volume = ValidateParents(host, approval.ParentKeys, approval.ParentSecurityHashes);
        var ledger = host.PinAndRead(InitialLedger);
        BootstrapPolicy.Node(ledger, InitialLedger);
        BootstrapPolicy.AdministrativeSecurity(ledger.Security, BootstrapPolicy.AdminSid);
        BootstrapPolicy.Need(ledger.Volume == volume && ledger.Key == approval.LedgerKey, "STAGE_LEDGER_CHANGED");
        BootstrapPolicy.Need(!host.Exists(Stage), "STAGE_EXISTS");
        return volume;
    }

    private static void ConsumeWindowsApproval(WindowsBootstrapHost host, InstallationApproval approval)
    {
        IntPtr descriptor = AllocateDescriptor();
        try
        {
            var security = InitialSecurity(descriptor);
            ulong volume = ObserveWindows(host, approval);
            string path = Path.Combine(InitialLedger, approval.ApprovalId + ".used");
            // CREATE_NEW는 경쟁에서 한 프로세스만 통과시킵니다. 빈 기록도 보존합니다.
            using var marker = CreateFileW(path, 0x40020000, 0, ref security, 1, 0x80200000, IntPtr.Zero);
            if (marker.IsInvalid)
                throw new Win32Exception(Marshal.GetLastWin32Error());
            var node = WindowsBootstrapHost.Read(marker, path, false);
            BootstrapPolicy.Need(node.Volume == volume, "STAGE_LEDGER_VOLUME");
            BootstrapPolicy.AdministrativeSecurity(node.Security, BootstrapPolicy.AdminSid);
            RandomAccess.Write(marker, new byte[] { 1 }, 0);
            RandomAccess.FlushToDisk(marker);
        }
        finally { LocalFree(descriptor); }
    }

    private static void CreateAndReadBack(WindowsBootstrapHost host, InstallationApproval approval)
    {
        IntPtr descriptor = AllocateDescriptor();
        try
        {
            var security = InitialSecurity(descriptor);
            ulong volume = ObserveWindows(host, approval);
            if (!CreateDirectoryW(Stage, ref security))
                throw new Win32Exception(Marshal.GetLastWin32Error());
            ValidateCreated(host.PinAndRead(Stage), volume);
            BootstrapPolicy.Need(ValidateParents(host, approval.ParentKeys, approval.ParentSecurityHashes)
                == volume, "STAGE_VOLUME_CHANGED");
        }
        finally { LocalFree(descriptor); }
    }

    private static IntPtr AllocateDescriptor()
    {
        if (!ConvertStringSecurityDescriptorToSecurityDescriptorW(Sddl, 1, out IntPtr descriptor, out _))
            throw new Win32Exception(Marshal.GetLastWin32Error());
        return descriptor;
    }

    private static SecurityAttributes InitialSecurity(IntPtr descriptor) => new()
    {
        Length = (uint)Marshal.SizeOf<SecurityAttributes>(),
        Descriptor = descriptor,
        InheritHandle = 0
    };
}

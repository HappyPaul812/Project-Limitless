using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

// 최초 staging 전용 Windows 경계입니다. 기존 Bootstrap의 대상/실행 파일은 변경하지 않습니다.
internal static class StageSecurityApi
{
    internal const string Stage = @"C:\ProgramData\LimitlessDiscordV2BootstrapStaging";
    private const string Sddl = "O:BAG:SYD:P(A;;FA;;;SY)(A;;FA;;;BA)";

    [StructLayout(LayoutKind.Sequential)]
    private struct SecurityAttributes
    {
        internal uint Length;
        internal IntPtr Descriptor;
        internal int InheritHandle;
    }

    [DllImport("kernel32.dll", ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool CreateDirectoryW(string path, ref SecurityAttributes security);

    [DllImport("advapi32.dll", ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool ConvertStringSecurityDescriptorToSecurityDescriptorW(
        string text, uint revision, out IntPtr descriptor, out uint size);

    [DllImport("kernel32.dll", ExactSpelling = true)]
    private static extern IntPtr LocalFree(IntPtr pointer);

    // 운영 원장과 신뢰된 승인 공급자가 승인되기 전에는 이 경계를 실행기에 연결하지 않습니다.
    // 만료된 계약은 native 호출 전에도 거부합니다. 생성 직후의 HANDLE은 검증이 끝날 때까지 유지합니다.
    private static void CreateAndReadBack(string administratorSid, DateTimeOffset expires,
        string[] parentKeys, string[] parentSecurityHashes)
    {
        using var host = new WindowsBootstrapHost();
        DemandTime(DateTimeOffset.UtcNow, expires);
        BootstrapPolicy.Need(host.FixedNtfsVolume, "STAGE_VOLUME");

        // Bootstrap의 실제 상승·비위장 토큰 검사는 내부 API를 공용 소스에서 재사용합니다.
        BootstrapPolicy.Need(WindowsBootstrapHost.Administrator() == administratorSid, "STAGE_ACTOR_CHANGED");
        ulong volume = ValidateParents(host, parentKeys, parentSecurityHashes);
        BootstrapPolicy.Need(!host.Exists(Stage), "STAGE_EXISTS");

        if (!ConvertStringSecurityDescriptorToSecurityDescriptorW(Sddl, 1, out IntPtr descriptor, out _))
            throw new Win32Exception(Marshal.GetLastWin32Error());
        try
        {
            var security = new SecurityAttributes
            {
                Length = (uint)Marshal.SizeOf<SecurityAttributes>(),
                Descriptor = descriptor,
                InheritHandle = 0
            };
            BootstrapPolicy.Need(ValidateParents(host, parentKeys, parentSecurityHashes) == volume, "STAGE_VOLUME_CHANGED");
            BootstrapPolicy.Need(WindowsBootstrapHost.Administrator() == administratorSid, "STAGE_ACTOR_CHANGED");
            DemandTime(DateTimeOffset.UtcNow, expires);
            // 경로가 그 사이에 생기면 OS가 실패시킵니다. 기존 항목으로 이어가거나 복구하지 않습니다.
            if (!CreateDirectoryW(Stage, ref security))
                throw new Win32Exception(Marshal.GetLastWin32Error());

            var created = host.PinAndRead(Stage);
            ValidateCreated(created, volume);
            BootstrapPolicy.Need(ValidateParents(host, parentKeys, parentSecurityHashes) == volume, "STAGE_VOLUME_CHANGED");
        }
        finally
        {
            LocalFree(descriptor);
        }
    }

    internal static void DemandTime(DateTimeOffset now, DateTimeOffset expires)
        => BootstrapPolicy.Need(now <= BootstrapPolicy.Cutoff && expires > now
            && expires <= BootstrapPolicy.Cutoff, "STAGE_EXPIRED");

    internal static ulong ValidateParents(IBootstrapHost host, string[] keys, string[] securityHashes)
    {
        BootstrapPolicy.Need(keys.Length == BootstrapPolicy.Ancestors.Count
            && securityHashes.Length == keys.Length, "STAGE_PARENT_BINDINGS");
        ulong volume = 0;
        for (int i = 0; i < keys.Length; i++)
        {
            string path = BootstrapPolicy.Ancestors[i];
            var parent = host.PinAndRead(path);
            BootstrapPolicy.Ancestor(parent, path);
            BootstrapPolicy.Need(parent.Key == keys[i]
                && BootstrapPolicy.Hash(parent.Security) == securityHashes[i], "STAGE_PARENT_CHANGED");
            if (i == 0)
                volume = parent.Volume;
            BootstrapPolicy.Need(parent.Volume == volume, "STAGE_PARENT_VOLUME");
        }
        return volume;
    }

    internal static void ValidateCreated(BootstrapNode node, ulong volume)
    {
        BootstrapPolicy.Node(node, Stage);
        BootstrapPolicy.Need(node.Volume == volume, "STAGE_CREATED_VOLUME");
        BootstrapPolicy.AdministrativeSecurity(node.Security, BootstrapPolicy.AdminSid);
    }

    // 승인된 운영 원장 위치가 없으므로 격리 시험에서만 사용합니다. 호출자가 동일 원장의 보호를 책임집니다.
    // 빈 파일도 소비로 간주합니다. 생성 후 중단이나 flush 실패가 발생해도 예약을 되돌리지 않습니다.
    internal static void ConsumeIsolatedApproval(string ledger, string approvalId)
    {
        BootstrapPolicy.Need(Guid.TryParseExact(approvalId, "N", out var id)
            && id != Guid.Empty && id.ToString("N") == approvalId, "STAGE_APPROVAL_ID");
        using var marker = new FileStream(Path.Combine(ledger, approvalId + ".used"),
            FileMode.CreateNew, FileAccess.Write, FileShare.None, 1, FileOptions.WriteThrough);
        marker.WriteByte(1);
        marker.Flush(flushToDisk: true);
    }

    // 신뢰된 비상승 호출자가 승인 채널에서 공용키를 받아야 합니다. 작업 폴더의 키는 승인 근거가 아닙니다.
    // 서명과 승인 SHA를 검사한 그대로의 메모리만 반환하며 경로를 다시 열거나 PowerShell을 실행하지 않습니다.
    internal static string VerifySignedSource(Stream pinnedSource, string expectedSha, RSA trustedSigner, byte[] signature)
    {
        BootstrapPolicy.Need(BootstrapPolicy.Sha(expectedSha)
            && pinnedSource.Length is > 0 and <= 1048576, "STAGE_SOURCE_INPUT");
        var bytes = new byte[pinnedSource.Length];
        pinnedSource.ReadExactly(bytes);
        BootstrapPolicy.Need(BootstrapPolicy.Hash(bytes) == expectedSha, "STAGE_SOURCE_SHA");
        BootstrapPolicy.Need(trustedSigner.KeySize >= 2048
            && trustedSigner.VerifyData(bytes, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pss),
            "STAGE_SOURCE_SIGNER");
        return new System.Text.UTF8Encoding(false, true).GetString(bytes);
    }
}

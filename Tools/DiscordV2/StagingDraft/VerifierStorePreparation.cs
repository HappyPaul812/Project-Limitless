using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using Microsoft.Win32.SafeHandles;

public static partial class LimitlessVerifierStorePreparation
{
    private const string Root = @"C:\";
    private const string Parent = @"C:\ProgramData";
    private const string Target = @"C:\ProgramData\LimitlessDiscordV2Verifier";
    private const string SystemSid = "S-1-5-18";
    private const string AdminSid = "S-1-5-32-544";
    private const string InstallerSid = "S-1-5-80-956008885-3418522649-1831038044-1853292631-2271478464";
    private const string InitialSddl = "O:BAG:SYD:P(A;;FA;;;SY)(A;;FA;;;BA)";

    [StructLayout(LayoutKind.Sequential)]
    private struct SecurityAttributes { internal uint Size; internal IntPtr Descriptor; internal int Inherit; }
    [StructLayout(LayoutKind.Sequential)]
    private struct FileId { internal ulong Volume, Low, High; }
    [StructLayout(LayoutKind.Sequential)]
    private struct FileTag { internal uint Attributes, Tag; }

    [DllImport("kernel32.dll", ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool CreateDirectoryW(string path, ref SecurityAttributes security);
    [DllImport("kernel32.dll", ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    private static extern SafeFileHandle CreateFileW(string path, uint access, uint share, IntPtr sec, uint creation, uint flags, IntPtr template);
    [DllImport("kernel32.dll", EntryPoint = "GetFileInformationByHandleEx", ExactSpelling = true, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool ReadFileId(SafeFileHandle handle, int kind, out FileId info, uint size);
    [DllImport("kernel32.dll", EntryPoint = "GetFileInformationByHandleEx", ExactSpelling = true, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool ReadTag(SafeFileHandle handle, int kind, out FileTag info, uint size);
    [DllImport("kernel32.dll", ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    private static extern uint GetFinalPathNameByHandleW(SafeFileHandle handle, StringBuilder path, uint size, uint flags);
    [DllImport("advapi32.dll", ExactSpelling = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    private static extern uint GetSecurityInfo(SafeFileHandle handle, int kind, uint flags, out IntPtr owner, out IntPtr group, out IntPtr dacl, out IntPtr sacl, out IntPtr security);
    [DllImport("advapi32.dll", ExactSpelling = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    private static extern uint GetSecurityDescriptorLength(IntPtr security);
    [DllImport("advapi32.dll", ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool ConvertStringSecurityDescriptorToSecurityDescriptorW(string sddl, uint version, out IntPtr security, out uint length);
    [DllImport("kernel32.dll", ExactSpelling = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    private static extern IntPtr LocalFree(IntPtr memory);
    [DllImport("kernel32.dll", ExactSpelling = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    private static extern IntPtr GetCurrentThread();
    [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool OpenThreadToken(IntPtr thread, uint access, bool asSelf, out SafeFileHandle token);
    [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetTokenInformation(IntPtr token, int kind, out int value, uint size, out uint returned);

    private static void Need(bool ok, string tag) { if (!ok) throw new InvalidOperationException(tag); }
    private static void Win(bool ok) { if (!ok) throw new Win32Exception(Marshal.GetLastWin32Error()); }

    private static void DemandActor(string sid, DateTimeOffset start, DateTimeOffset end)
    {
        SafeFileHandle threadToken;
        bool impersonated = OpenThreadToken(GetCurrentThread(), 8, true, out threadToken);
        int error = Marshal.GetLastWin32Error();
        if (threadToken != null) threadToken.Dispose();
        Need(!impersonated && error == 1008, "HOLD_THREAD_TOKEN");
        using (var identity = WindowsIdentity.GetCurrent())
        {
            Need(identity.User != null && identity.User.Value == sid
                && new WindowsPrincipal(identity).IsInRole(WindowsBuiltInRole.Administrator), "HOLD_SIGNER_TOKEN");
            int elevated;
            uint length;
            Win(GetTokenInformation(identity.Token, 20, out elevated, 4, out length));
            Need(elevated == 1, "HOLD_NOT_ELEVATED");
        }
        var now = DateTimeOffset.UtcNow;
        Need(start <= now && now < end, "HOLD_APPROVAL_WINDOW");
    }

    private static SafeFileHandle OpenPath(string path)
    {
        // OPEN_REPARSE_POINT를 사용하고 쓰기·삭제 공유를 허용하지 않습니다.
        return CreateFileW(path, 0x120080, 1, IntPtr.Zero, 3, 0x02200000, IntPtr.Zero);
    }

    private static SafeFileHandle Pin(string path)
    {
        var handle = OpenPath(path);
        if (!handle.IsInvalid) return handle;
        int error = Marshal.GetLastWin32Error();
        handle.Dispose();
        throw new Win32Exception(error);
    }

    private static bool TrustedAncestorSid(string sid)
    {
        return sid == SystemSid || sid == AdminSid || sid == InstallerSid;
    }

    private static byte[] ReadDescriptor(SafeFileHandle handle)
    {
        IntPtr owner, group, dacl, sacl, descriptor;
        uint error = GetSecurityInfo(handle, 1, 7, out owner, out group, out dacl, out sacl, out descriptor);
        if (error != 0) throw new Win32Exception((int)error);
        try
        {
            uint size = GetSecurityDescriptorLength(descriptor);
            Need(size > 0 && size <= 65536, "HOLD_DESCRIPTOR_SIZE");
            byte[] raw = new byte[size];
            Marshal.Copy(descriptor, raw, 0, (int)size);
            return raw;
        }
        finally { LocalFree(descriptor); }
    }

    private static string ReadPinned(SafeFileHandle handle, string path, bool ancestor, out ulong volume, bool directory = true)
    {
        FileTag tag;
        Win(ReadTag(handle, 9, out tag, 8));
        Need((tag.Attributes & 0x410) == (directory ? 0x10U : 0U), "HOLD_REPARSE_OR_NOT_DIRECTORY");
        FileId id;
        Win(ReadFileId(handle, 18, out id, 24));
        Need(id.Low != 0 || id.High != 0, "HOLD_FILE_ID");
        var full = new StringBuilder(1024);
        uint length = GetFinalPathNameByHandleW(handle, full, (uint)full.Capacity, 0);
        Need(length > 0 && length < full.Capacity &&
            full.ToString().Equals(@"\\?\" + path, StringComparison.OrdinalIgnoreCase), "HOLD_FINAL_PATH");
        byte[] bytes = ReadDescriptor(handle);
        var sd = new RawSecurityDescriptor(bytes, 0);
        Need(sd.Owner != null && sd.DiscretionaryAcl != null
            && (sd.ControlFlags & ControlFlags.DiscretionaryAclPresent) != 0, "HOLD_DACL_ABSENT");

        if (ancestor)
        {
            Need(TrustedAncestorSid(sd.Owner.Value), "HOLD_PARENT_OWNER");
            foreach (GenericAce item in sd.DiscretionaryAcl)
            {
                var ace = item as CommonAce;
                Need(ace != null && !ace.IsCallback &&
                    (ace.AceQualifier == AceQualifier.AccessAllowed || ace.AceQualifier == AceQualifier.AccessDenied),
                    "HOLD_PARENT_ACE");
                if (ace.AceQualifier == AceQualifier.AccessAllowed && (ace.AceFlags & AceFlags.InheritOnly) == 0
                    && !TrustedAncestorSid(ace.SecurityIdentifier.Value))
                    Need((unchecked((uint)ace.AccessMask) & 0xF00D0040U) == 0, "HOLD_PARENT_REPLACE");
            }
        }
        else
        {
            Need(sd.Owner.Value == AdminSid && sd.Group != null && sd.Group.Value == SystemSid
                && (sd.ControlFlags & ControlFlags.DiscretionaryAclProtected) != 0
                && sd.DiscretionaryAcl.Count == 2, "HOLD_CREATED_DESCRIPTOR");
            bool system = false, admin = false;
            foreach (GenericAce item in sd.DiscretionaryAcl)
            {
                var ace = item as CommonAce;
                Need(ace != null && !ace.IsCallback && ace.AceFlags == AceFlags.None
                    && ace.AceQualifier == AceQualifier.AccessAllowed && ace.AccessMask == 0x1F01FF, "HOLD_CREATED_ACE");
                system |= ace.SecurityIdentifier.Value == SystemSid;
                admin |= ace.SecurityIdentifier.Value == AdminSid;
            }
            Need(system && admin, "HOLD_CREATED_PRINCIPALS");
        }
        volume = id.Volume;
        using (var sha = SHA256.Create())
            return id.Volume.ToString("x16") + ":" + id.High.ToString("x16") + id.Low.ToString("x16")
                + ":" + BitConverter.ToString(sha.ComputeHash(bytes)).Replace("-", "").ToLowerInvariant();
    }

    // 새 소스의 부모 HANDLE 판독만 검증할 수 있습니다. 경로 관찰에도 쓰기 API를 호출하지 않습니다.
    public static void CheckParentsOnly()
    {
        using (var root = Pin(Root))
        using (var parent = Pin(Parent))
        {
            ulong rootVolume, parentVolume;
            ReadPinned(root, Root, true, out rootVolume);
            ReadPinned(parent, Parent, true, out parentVolume);
            Need(rootVolume == parentVolume, "HOLD_PARENT_VOLUME");
        }
    }

#if !DESIGNATION_ONLY && !DESIGNATION_READ_ONLY && !SIGNING_ONCE && !VERIFIER_DEPLOY
    public static string CreateOnce(string approvedSid, DateTimeOffset start, DateTimeOffset end)
    {
        // 승인 기록의 SID와 로컬 windows 계정이 모두 일치해야 합니다.
        var account = new NTAccount(Environment.MachineName, "windows");
        Need(account.Translate(typeof(SecurityIdentifier)).Value == approvedSid, "HOLD_APPROVED_SID");
        DemandActor(approvedSid, start, end);
        var drive = new DriveInfo(Root);
        Need(drive.DriveType == DriveType.Fixed && drive.DriveFormat == "NTFS", "HOLD_FIXED_NTFS");
        using (var root = Pin(Root))
        using (var parent = Pin(Parent))
        {
            ulong rootVolume, parentVolume, rereadVolume;
            string rootKey = ReadPinned(root, Root, true, out rootVolume);
            string parentKey = ReadPinned(parent, Parent, true, out parentVolume);
            Need(rootVolume == parentVolume, "HOLD_PARENT_VOLUME");
            using (var existing = OpenPath(Target))
            {
                int error = Marshal.GetLastWin32Error();
                Need(existing.IsInvalid && error == 2, "HOLD_TARGET_NOT_ABSENT");
            }
            IntPtr descriptor;
            uint size;
            Win(ConvertStringSecurityDescriptorToSecurityDescriptorW(InitialSddl, 1, out descriptor, out size));
            try
            {
                Need(ReadPinned(root, Root, true, out rereadVolume) == rootKey
                    && ReadPinned(parent, Parent, true, out rereadVolume) == parentKey, "HOLD_PARENT_CHANGED");
                DemandActor(approvedSid, start, end);
                var security = new SecurityAttributes
                {
                    Size = (uint)Marshal.SizeOf(typeof(SecurityAttributes)), Descriptor = descriptor, Inherit = 0
                };
                if (!CreateDirectoryW(Target, ref security))
                    throw new Win32Exception(Marshal.GetLastWin32Error(), "HOLD_CREATE_NO_RETRY");
                using (var created = Pin(Target))
                {
                    ulong createdVolume;
                    string record = ReadPinned(created, Target, false, out createdVolume);
                    Need(createdVolume == parentVolume && ReadPinned(root, Root, true, out rereadVolume) == rootKey
                        && ReadPinned(parent, Parent, true, out rereadVolume) == parentKey, "HOLD_POST_CREATE_PARENT");
                    return record;
                }
            }
            finally { LocalFree(descriptor); }
        }
    }
#endif
}

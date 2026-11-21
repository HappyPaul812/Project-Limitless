using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

public static partial class LimitlessVerifierStorePreparation
{
    private const string ApprovalPath = Target + @"\SigningAccountDesignation.v1.json";

    // 기존 저장소와 부모의 HANDLE을 유지하고, 승인 파일을 초기 보호 ACL과 함께 새로 생성합니다.
    public static string WriteDesignation(string sid, DateTimeOffset start, DateTimeOffset end,
        string expectedStoreReceipt, byte[] record)
    {
        var account = new System.Security.Principal.NTAccount(Environment.MachineName, "windows");
        Need(account.Translate(typeof(System.Security.Principal.SecurityIdentifier)).Value == sid, "HOLD_APPROVED_SID");
        DemandActor(sid, start, end);
        Need(record != null && record.Length > 0 && record.Length <= 8192, "HOLD_RECORD_SIZE");
        var drive = new DriveInfo(Root);
        Need(drive.DriveType == DriveType.Fixed && drive.DriveFormat == "NTFS", "HOLD_FIXED_NTFS");
        using (var root = Pin(Root))
        using (var parent = Pin(Parent))
        using (var store = Pin(Target))
        {
            ulong rootVolume, parentVolume, storeVolume, observed;
            string rootKey = ReadPinned(root, Root, true, out rootVolume);
            string parentKey = ReadPinned(parent, Parent, true, out parentVolume);
            string storeReceipt = ReadPinned(store, Target, false, out storeVolume);
            Need(rootVolume == parentVolume && parentVolume == storeVolume &&
                storeReceipt == expectedStoreReceipt, "HOLD_STORE_RECEIPT");

            IntPtr descriptor;
            uint size;
            Win(ConvertStringSecurityDescriptorToSecurityDescriptorW(InitialSddl, 1, out descriptor, out size));
            try
            {
                var attrs = new SecurityAttributes
                {
                    Size = (uint)Marshal.SizeOf(typeof(SecurityAttributes)), Descriptor = descriptor, Inherit = 0
                };
                Need(ReadPinned(root, Root, true, out observed) == rootKey &&
                    ReadPinned(parent, Parent, true, out observed) == parentKey &&
                    ReadPinned(store, Target, false, out observed) == storeReceipt, "HOLD_PARENT_CHANGED");
                DemandActor(sid, start, end);
                // CREATE_NEW 원자 검사로 기존 기록과 동일 승인 재사용을 거부합니다.
                using (var handle = CreateRecord(ApprovalPath, 0xc0020000, 0, ref attrs, 1, 0x80200000, IntPtr.Zero))
                {
                    if (handle.IsInvalid)
                        throw new Win32Exception(Marshal.GetLastWin32Error(), "HOLD_RECORD_CREATE_NO_RETRY");
                    string fileReceipt = ReadPinned(handle, ApprovalPath, false, out observed, false);
                    Need(observed == storeVolume, "HOLD_RECORD_VOLUME");
                    using (var stream = new FileStream(handle, FileAccess.ReadWrite))
                    {
                        stream.Write(record, 0, record.Length);
                        stream.Flush(true);
                        stream.Position = 0;
                        int next;
                        foreach (byte expected in record)
                        {
                            next = stream.ReadByte();
                            Need(next == expected, "HOLD_RECORD_READBACK");
                        }
                        Need(stream.ReadByte() == -1 && ReadPinned(handle, ApprovalPath, false, out observed, false) == fileReceipt &&
                            ReadPinned(store, Target, false, out observed) == storeReceipt &&
                            ReadPinned(root, Root, true, out observed) == rootKey &&
                            ReadPinned(parent, Parent, true, out observed) == parentKey, "HOLD_POST_WRITE_READBACK");
                        using (var sha = SHA256.Create())
                            return BitConverter.ToString(sha.ComputeHash(record)).Replace("-", "").ToLowerInvariant();
                    }
                }
            }
            finally { LocalFree(descriptor); }
        }
    }

    [DllImport("kernel32.dll", EntryPoint = "CreateFileW", ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    private static extern Microsoft.Win32.SafeHandles.SafeFileHandle CreateRecord(
        string path, uint access, uint share, ref SecurityAttributes attributes, uint disposition, uint flags, IntPtr template);
}

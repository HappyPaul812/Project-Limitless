using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

public static partial class LimitlessVerifierStorePreparation
{
    public sealed class InstallApprovalStore : IDisposable
    {
        public const string RuntimePath = @"C:\ProgramData\LimitlessDiscordV2InstallerRuntime";
        public const string LedgerPath = @"C:\ProgramData\LimitlessDiscordV2BootstrapLedger";
        public const string StagePath = @"C:\ProgramData\LimitlessDiscordV2BootstrapStaging";
        private readonly string sid;
        private readonly DateTimeOffset start, end;
        private readonly SafeFileHandle root, parent, runtime, ledger;
        private readonly string rootReceipt, parentReceipt, runtimeReceipt, ledgerReceipt;
        private readonly ulong volume;
        public string RootReceipt { get { return rootReceipt; } }
        public string ParentReceipt { get { return parentReceipt; } }
        public string LedgerReceipt { get { return ledgerReceipt; } }

        public InstallApprovalStore(string approvedSid, DateTimeOffset start, DateTimeOffset end)
        {
            sid = approvedSid;
            this.start = start;
            this.end = end;
            DemandActor(sid, start, end);
            var drive = new DriveInfo(Root);
            Need(drive.DriveType == DriveType.Fixed && drive.DriveFormat == "NTFS", "HOLD_FIXED_NTFS");
            try
            {
                root = Pin(Root);
                parent = Pin(Parent);
                runtime = Pin(RuntimePath);
                ledger = Pin(LedgerPath);
                ulong parentVol, runtimeVol, ledgerVol;
                rootReceipt = ReadPinned(root, Root, true, out volume);
                parentReceipt = ReadPinned(parent, Parent, true, out parentVol);
                runtimeReceipt = ReadPinned(runtime, RuntimePath, false, out runtimeVol);
                ledgerReceipt = ReadPinned(ledger, LedgerPath, false, out ledgerVol);
                Need(volume == parentVol && volume == runtimeVol && volume == ledgerVol, "HOLD_APPROVAL_VOLUME");
                Check();
            }
            catch { Dispose(); throw; }
        }

        public void Check()
        {
            ulong ignored;
            Need(ReadPinned(root, Root, true, out ignored) == rootReceipt &&
                ReadPinned(parent, Parent, true, out ignored) == parentReceipt &&
                ReadPinned(runtime, RuntimePath, false, out ignored) == runtimeReceipt &&
                ReadPinned(ledger, LedgerPath, false, out ignored) == ledgerReceipt, "HOLD_APPROVAL_PARENT_CHANGED");
        }

        private static string ApprovalFile(string name)
        {
            Need(name == "InstallApproval.json" || name == "InstallApproval.sig", "HOLD_APPROVAL_OUTPUT_NAME");
            return Path.Combine(RuntimePath, name);
        }

        private static string InputFile(string name)
        {
            if (name == "Limitless.InitialStagingInstaller.exe" || name == "StagingSource.ps1" ||
                name == "StagingSource.sig")
                return Path.Combine(RuntimePath, name);
            return ApprovalFile(name);
        }

        private static void RequireAbsent(string path)
        {
            using (var handle = OpenPath(path))
            {
                int error = Marshal.GetLastWin32Error();
                Need(handle.IsInvalid && error == 2, "HOLD_APPROVAL_PATH_COLLISION");
            }
        }

        public void CheckPreparation()
        {
            Check();
            RequireAbsent(StagePath);
            RequireAbsent(ApprovalFile("InstallApproval.json"));
            RequireAbsent(ApprovalFile("InstallApproval.sig"));
        }

        public void CheckUnused(string id)
        {
            Guid value;
            Need(Guid.TryParseExact(id, "N", out value) && value != Guid.Empty &&
                value.ToString("N") == id, "HOLD_APPROVAL_ID");
            Check();
            RequireAbsent(Path.Combine(LedgerPath, id + ".used"));
        }

        public FileStream Open(string name)
        {
            Check();
            string path = InputFile(name);
            var handle = CreateFileW(path, 0x120089, 1, IntPtr.Zero, 3, 0x02200000, IntPtr.Zero);
            try
            {
                Win(!handle.IsInvalid);
                ulong observed;
                ReadPinned(handle, path, false, out observed, false);
                Need(observed == volume, "HOLD_APPROVAL_FILE_VOLUME");
                return new FileStream(handle, FileAccess.Read);
            }
            catch { handle.Dispose(); throw; }
        }

        public string Receipt(FileStream stream, string name)
        {
            Check();
            ulong observed;
            string receipt = ReadPinned(stream.SafeFileHandle, InputFile(name), false, out observed, false);
            Need(observed == volume, "HOLD_APPROVAL_FILE_VOLUME");
            return receipt;
        }

        // 발급 전용: 원장을 읽기만 하며 승인 JSON과 분리 서명 이름만 쓸 수 있습니다.
        public string WriteNew(string name, byte[] bytes, DateTimeOffset expires)
        {
            string path = ApprovalFile(name);
            int cap = name == "InstallApproval.sig" ? 4096 : 8192;
            Need(bytes != null && bytes.Length > 0 && bytes.Length <= cap, "HOLD_APPROVAL_OUTPUT_SIZE");
            Check();
            DemandActor(sid, start, end);
            Need(DateTimeOffset.UtcNow < expires, "HOLD_APPROVAL_EXPIRED");
            RequireAbsent(StagePath);
            IntPtr descriptor;
            uint size;
            Win(ConvertStringSecurityDescriptorToSecurityDescriptorW(InitialSddl, 1, out descriptor, out size));
            try
            {
                var attrs = new SecurityAttributes
                {
                    Size = (uint)Marshal.SizeOf(typeof(SecurityAttributes)), Descriptor = descriptor, Inherit = 0
                };
                using (var handle = CreateApprovalFile(path, 0xc0020000, 0, ref attrs, 1, 0x80200000, IntPtr.Zero))
                {
                    Win(!handle.IsInvalid);
                    using (var stream = new FileStream(handle, FileAccess.ReadWrite))
                    {
                        string receipt = Receipt(stream,name);
                        stream.Write(bytes, 0, bytes.Length);
                        stream.Flush(true);
                        Need(Receipt(stream,name) == receipt, "HOLD_APPROVAL_RECORD_CHANGED");
                        return receipt;
                    }
                }
            }
            finally { LocalFree(descriptor); }
        }

        public void Dispose()
        {
            if (ledger != null) ledger.Dispose();
            if (runtime != null) runtime.Dispose();
            if (parent != null) parent.Dispose();
            if (root != null) root.Dispose();
        }

        [DllImport("kernel32.dll", EntryPoint = "CreateFileW", ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = true)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        private static extern SafeFileHandle CreateApprovalFile(string path, uint access, uint share,
            ref SecurityAttributes security, uint disposition, uint flags, IntPtr template);
    }
}

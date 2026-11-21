using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

public static partial class LimitlessVerifierStorePreparation
{
    public sealed class StagingInputTransfer : IDisposable
    {
        public const string SourceFolder = @"C:\ProgramData\LimitlessDiscordV2StagingSourceSigningOnce";
        public const string RuntimeFolder = @"C:\ProgramData\LimitlessDiscordV2InstallerRuntime";
        public const string InstallerName = "Limitless.InitialStagingInstaller.exe";
        private readonly SafeFileHandle root, parent, source, runtime;
        private readonly string rootKey, parentKey, sourceKey, runtimeKey;
        private readonly ulong volume;
        private readonly string actor;
        private readonly DateTimeOffset start, end;

        public StagingInputTransfer(string sid, DateTimeOffset start, DateTimeOffset end)
        {
            actor = sid;
            this.start = start;
            this.end = end;
            DemandActor(sid, start, end);
            var drive = new DriveInfo(Root);
            Need(drive.DriveType == DriveType.Fixed && drive.DriveFormat == "NTFS", "HOLD_FIXED_NTFS");
            try
            {
                root = Pin(Root);
                parent = Pin(Parent);
                source = Pin(SourceFolder);
                runtime = Pin(RuntimeFolder);
                ulong parentVolume, sourceVolume, runtimeVolume;
                rootKey = ReadPinned(root, Root, true, out volume);
                parentKey = ReadPinned(parent, Parent, true, out parentVolume);
                sourceKey = ReadPinned(source, SourceFolder, false, out sourceVolume);
                runtimeKey = ReadPinned(runtime, RuntimeFolder, false, out runtimeVolume);
                Need(volume == parentVolume && volume == sourceVolume && volume == runtimeVolume, "HOLD_DEPLOY_VOLUME");
                Check();
            }
            catch { Dispose(); throw; }
        }

        public void Check()
        {
            ulong ignored;
            Need(ReadPinned(root, Root, true, out ignored) == rootKey &&
                ReadPinned(parent, Parent, true, out ignored) == parentKey &&
                ReadPinned(source, SourceFolder, false, out ignored) == sourceKey &&
                ReadPinned(runtime, RuntimeFolder, false, out ignored) == runtimeKey, "HOLD_DEPLOY_PARENT_CHANGED");
        }

        private static string InputPath(string folder, string name)
        {
            Need(name == "StagingSource.ps1" || name == "StagingSource.sig", "HOLD_DEPLOY_NAME");
            return Path.Combine(folder, name);
        }

        public void DemandInputsAbsent()
        {
            Check();
            foreach (string name in new[] { "StagingSource.ps1", "StagingSource.sig" })
            {
                using (var h = OpenPath(InputPath(RuntimeFolder, name)))
                {
                    int error = Marshal.GetLastWin32Error();
                    Need(h.IsInvalid && error == 2, "HOLD_DEPLOY_INPUT_COLLISION");
                }
            }
        }

        public FileStream OpenInput(string name, bool deployed)
        {
            return OpenFile(InputPath(deployed ? RuntimeFolder : SourceFolder, name));
        }

        public FileStream OpenInstaller()
        {
            return OpenFile(Path.Combine(RuntimeFolder, InstallerName));
        }

        private FileStream OpenFile(string path)
        {
            Check();
            var h = CreateFileW(path, 0x120089, 1, IntPtr.Zero, 3, 0x02200000, IntPtr.Zero);
            try
            {
                Win(!h.IsInvalid);
                ulong fileVolume;
                ReadPinned(h, path, false, out fileVolume, false);
                Need(fileVolume == volume, "HOLD_DEPLOY_FILE_VOLUME");
                return new FileStream(h, FileAccess.Read);
            }
            catch { h.Dispose(); throw; }
        }

        public string ReadReceipt(FileStream stream, string name, bool deployed)
        {
            Check();
            string folder = deployed ? RuntimeFolder : SourceFolder;
            string path = deployed && name == InstallerName ? Path.Combine(folder, name) : InputPath(folder, name);
            ulong fileVolume;
            string receipt = ReadPinned(stream.SafeFileHandle, path, false, out fileVolume, false);
            Need(fileVolume == volume, "HOLD_DEPLOY_FILE_VOLUME");
            return receipt;
        }

        // 보호 승인 자료나 설치 이미지는 쓰기 대상으로 허용하지 않습니다.
        public string CopyInputNew(string name, byte[] bytes)
        {
            string path = InputPath(RuntimeFolder, name);
            int cap = name == "StagingSource.sig" ? 4096 : 1048576;
            Need(bytes != null && bytes.Length > 0 && bytes.Length <= cap, "HOLD_DEPLOY_INPUT_SIZE");
            Check();
            DemandActor(actor, start, end);
            IntPtr descriptor;
            uint size;
            Win(ConvertStringSecurityDescriptorToSecurityDescriptorW(InitialSddl, 1, out descriptor, out size));
            try
            {
                var attrs = new SecurityAttributes
                {
                    Size = (uint)Marshal.SizeOf(typeof(SecurityAttributes)), Descriptor = descriptor, Inherit = 0
                };
                using (var h = CreateInputFile(path, 0xc0020000, 0, ref attrs, 1, 0x80200000, IntPtr.Zero))
                {
                    Win(!h.IsInvalid); // CREATE_NEW: 기존 항목은 언제나 실패입니다.
                    using (var stream = new FileStream(h, FileAccess.ReadWrite))
                    {
                        string receipt = ReadReceipt(stream, name, true);
                        stream.Write(bytes, 0, bytes.Length);
                        stream.Flush(true);
                        Need(ReadReceipt(stream, name, true) == receipt, "HOLD_DEPLOY_FILE_CHANGED");
                        return receipt;
                    }
                }
            }
            finally { LocalFree(descriptor); }
        }

        public void Dispose()
        {
            if (runtime != null) runtime.Dispose();
            if (source != null) source.Dispose();
            if (parent != null) parent.Dispose();
            if (root != null) root.Dispose();
        }

        [DllImport("kernel32.dll", EntryPoint = "CreateFileW", ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = true)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        private static extern SafeFileHandle CreateInputFile(string path, uint access, uint share,
            ref SecurityAttributes security, uint disposition, uint flags, IntPtr template);
    }
}

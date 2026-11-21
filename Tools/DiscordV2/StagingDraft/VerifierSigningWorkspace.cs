using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

public static partial class LimitlessVerifierStorePreparation
{
    public sealed class SigningWorkspace : IDisposable
    {
        private const string Folder = @"C:\ProgramData\LimitlessDiscordV2SigningOnce";
        private readonly SafeFileHandle root, parent, folder;
        private readonly string rootKey, parentKey, folderKey;
        private readonly ulong volume;
        public string DirectoryPath { get { return Folder; } }

        public SigningWorkspace(string sid, DateTimeOffset start, DateTimeOffset end)
        {
            DemandActor(sid, start, end);
            var drive = new DriveInfo(Root);
            Need(drive.DriveType == DriveType.Fixed && drive.DriveFormat == "NTFS", "HOLD_FIXED_NTFS");
            try
            {
                root = Pin(Root);
                parent = Pin(Parent);
                ulong parentVolume;
                rootKey = ReadPinned(root, Root, true, out volume);
                parentKey = ReadPinned(parent, Parent, true, out parentVolume);
                Need(volume == parentVolume, "HOLD_PARENT_VOLUME");
                IntPtr descriptor;
                uint size;
                Win(ConvertStringSecurityDescriptorToSecurityDescriptorW(InitialSddl, 1, out descriptor, out size));
                try
                {
                    var attrs = AttributesFor(descriptor);
                    ulong ignored;
                    Need(ReadPinned(root, Root, true, out ignored) == rootKey &&
                        ReadPinned(parent, Parent, true, out ignored) == parentKey, "HOLD_PARENT_CHANGED");
                    DemandActor(sid, start, end);
                    // 기존 위치는 채택하지 않습니다. 새 작업 경로가 이 서명 1회 시도의 표식입니다.
                    Win(CreateDirectoryW(Folder, ref attrs));
                }
                finally { LocalFree(descriptor); }
                folder = Pin(Folder);
                ulong folderVolume;
                folderKey = ReadPinned(folder, Folder, false, out folderVolume);
                Need(volume == folderVolume, "HOLD_WORKSPACE_VOLUME");
                Check();
            }
            catch { Dispose(); throw; }
        }

        private static SecurityAttributes AttributesFor(IntPtr descriptor)
        {
            return new SecurityAttributes
            {
                Size = (uint)Marshal.SizeOf(typeof(SecurityAttributes)), Descriptor = descriptor, Inherit = 0
            };
        }

        public void Check()
        {
            ulong ignored;
            Need(ReadPinned(root, Root, true, out ignored) == rootKey &&
                ReadPinned(parent, Parent, true, out ignored) == parentKey &&
                ReadPinned(folder, Folder, false, out ignored) == folderKey, "HOLD_WORKSPACE_CHANGED");
        }

        private static string FilePath(string name)
        {
            Need(name == "signtool.exe" || name == "Limitless.StagingVerifier.exe", "HOLD_WORKSPACE_NAME");
            return Path.Combine(Folder, name);
        }

        public void CopyVerifiedBytes(string name, byte[] bytes)
        {
            Check();
            Need(bytes != null && bytes.Length > 0 && bytes.Length <= 16777216, "HOLD_COPY_SIZE");
            string path = FilePath(name);
            IntPtr descriptor;
            uint size;
            Win(ConvertStringSecurityDescriptorToSecurityDescriptorW(InitialSddl, 1, out descriptor, out size));
            try
            {
                var attrs = AttributesFor(descriptor);
                using (var handle = CreateProtectedFile(path, 0xc0020000, 0, ref attrs, 1, 0x80200000, IntPtr.Zero))
                {
                    Win(!handle.IsInvalid);
                    ulong fileVolume;
                    ReadPinned(handle, path, false, out fileVolume, false);
                    Need(fileVolume == volume, "HOLD_COPY_VOLUME");
                    using (var stream = new FileStream(handle, FileAccess.ReadWrite))
                    {
                        stream.Write(bytes, 0, bytes.Length);
                        stream.Flush(true);
                    }
                }
            }
            finally { LocalFree(descriptor); }
        }

        public FileStream OpenVerifiedFile(string name)
        {
            Check();
            string path = FilePath(name);
            var handle = CreateFileW(path, 0x120089, 1, IntPtr.Zero, 3, 0x02200000, IntPtr.Zero);
            try
            {
                Win(!handle.IsInvalid);
                ulong fileVolume;
                ReadPinned(handle, path, false, out fileVolume, false);
                Need(fileVolume == volume, "HOLD_COPY_VOLUME");
                return new FileStream(handle, FileAccess.Read);
            }
            catch { handle.Dispose(); throw; }
        }

        public void Dispose()
        {
            if (folder != null) folder.Dispose();
            if (parent != null) parent.Dispose();
            if (root != null) root.Dispose();
        }

        [DllImport("kernel32.dll", EntryPoint = "CreateFileW", ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = true)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        private static extern SafeFileHandle CreateProtectedFile(string path, uint access, uint share,
            ref SecurityAttributes security, uint disposition, uint flags, IntPtr template);
    }
}

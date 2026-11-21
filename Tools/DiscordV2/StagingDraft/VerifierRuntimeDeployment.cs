using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

public static partial class LimitlessVerifierStorePreparation
{
    // 원본 서명 폴더와 조상도 열어 둔 상태에서 Authenticode와 원문 SHA를 읽습니다.
    public sealed class SignedVerifierSource : IDisposable
    {
        private const string SourceFolder = @"C:\ProgramData\LimitlessDiscordV2SigningOnce";
        private const string SourceFile = SourceFolder + @"\Limitless.StagingVerifier.exe";
        private readonly SafeFileHandle root, parent, folder;
        private readonly string rootKey, parentKey, folderKey, fileKey;
        public FileStream Stream { get; private set; }
        public string FilePath { get { return SourceFile; } }

        public SignedVerifierSource()
        {
            try
            {
                var drive = new DriveInfo(Root);
                Need(drive.DriveType == DriveType.Fixed && drive.DriveFormat == "NTFS", "HOLD_FIXED_NTFS");
                root = Pin(Root);
                parent = Pin(Parent);
                folder = Pin(SourceFolder);
                ulong rootVolume, parentVolume, folderVolume, fileVolume;
                rootKey = ReadPinned(root, Root, true, out rootVolume);
                parentKey = ReadPinned(parent, Parent, true, out parentVolume);
                folderKey = ReadPinned(folder, SourceFolder, false, out folderVolume);
                var h = CreateFileW(SourceFile, 0x120089, 1, IntPtr.Zero, 3, 0x02200000, IntPtr.Zero);
                try
                {
                    Win(!h.IsInvalid);
                    fileKey = ReadPinned(h, SourceFile, false, out fileVolume, false);
                    Need(rootVolume == parentVolume && parentVolume == folderVolume && folderVolume == fileVolume,
                        "HOLD_SOURCE_VOLUME");
                    Stream = new FileStream(h, FileAccess.Read);
                }
                catch { h.Dispose(); throw; }
                Check();
            }
            catch { Dispose(); throw; }
        }

        public void Check()
        {
            ulong ignored;
            Need(ReadPinned(root, Root, true, out ignored) == rootKey &&
                ReadPinned(parent, Parent, true, out ignored) == parentKey &&
                ReadPinned(folder, SourceFolder, false, out ignored) == folderKey &&
                ReadPinned(Stream.SafeFileHandle, SourceFile, false, out ignored, false) == fileKey,
                "HOLD_SIGNED_SOURCE_CHANGED");
        }

        public void Dispose()
        {
            if (Stream != null) Stream.Dispose();
            if (folder != null) folder.Dispose();
            if (parent != null) parent.Dispose();
            if (root != null) root.Dispose();
        }
    }
}

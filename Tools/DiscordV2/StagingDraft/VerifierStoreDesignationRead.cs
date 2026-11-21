using System;
using System.IO;
using System.Security.Cryptography;

public static partial class LimitlessVerifierStorePreparation
{
    // 기록을 바꾸지 않고 조상, 저장소와 승인 파일을 연 HANDLE에서 검사합니다.
    public static byte[] ReadDesignation(string sid, string expectedStoreReceipt, string expectedRecordSha)
    {
        var now = DateTimeOffset.UtcNow;
        DemandActor(sid, now, now.AddMinutes(1));
        var drive = new DriveInfo(Root);
        Need(drive.DriveType == DriveType.Fixed && drive.DriveFormat == "NTFS", "HOLD_FIXED_NTFS");
        string path = Target + @"\SigningAccountDesignation.v1.json";
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
            using (var handle = CreateFileW(path, 0x120089, 1, IntPtr.Zero, 3, 0x02200000, IntPtr.Zero))
            {
                Win(!handle.IsInvalid);
                string fileKey = ReadPinned(handle, path, false, out observed, false);
                Need(observed == storeVolume, "HOLD_RECORD_VOLUME");
                using (var stream = new FileStream(handle, FileAccess.Read))
                {
                    long length = stream.Length;
                    Need(length > 0 && length <= 8192, "HOLD_RECORD_SIZE");
                    byte[] data = new byte[length];
                    int offset = 0;
                    while (offset < data.Length)
                    {
                        int read = stream.Read(data, offset, data.Length - offset);
                        Need(read > 0, "HOLD_RECORD_READ");
                        offset += read;
                    }
                    Need(stream.ReadByte() == -1 && ReadPinned(handle, path, false, out observed, false) == fileKey
                        && ReadPinned(store, Target, false, out observed) == storeReceipt
                        && ReadPinned(root, Root, true, out observed) == rootKey
                        && ReadPinned(parent, Parent, true, out observed) == parentKey, "HOLD_RECORD_READBACK");
                    using (var sha = SHA256.Create())
                        Need(BitConverter.ToString(sha.ComputeHash(data)).Replace("-", "").ToLowerInvariant()
                            == expectedRecordSha, "HOLD_RECORD_SHA");
                    return data;
                }
            }
        }
    }
}

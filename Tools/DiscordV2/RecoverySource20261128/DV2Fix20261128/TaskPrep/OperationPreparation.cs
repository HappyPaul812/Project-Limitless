using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Microsoft.Win32.SafeHandles;

public static partial class LimitlessV2OperationIssuer20261128
{
    [DllImport("kernel32.dll", EntryPoint="CreateFileW", CharSet=CharSet.Unicode, SetLastError=true)]
    private static extern SafeFileHandle CreateProductFile(string path,uint access,uint share,ref SecurityAttributes security,uint creation,uint flags,IntPtr template);

    public sealed class ProductDeploymentPreparation : IDisposable
    {
        const string SignedRoot = @"C:\ProgramData\LimitlessDiscordV2ProductSigning20261128";
        const string Runtime = @"C:\ProgramData\Limitless\DiscordV2";
        const string ToolFolder = Runtime + @"\Control\DeploymentTool";
        const string Tool = ToolFolder + @"\Limitless.DeploymentAdmin.exe";
        readonly System.Collections.Generic.Dictionary<string,(SafeFileHandle Handle,string Receipt,bool Ancestor)> parents
            = new System.Collections.Generic.Dictionary<string,(SafeFileHandle,string,bool)>(StringComparer.OrdinalIgnoreCase);
        readonly string sid;
        readonly DateTimeOffset start,end;
        readonly string expectedToolSha;
        public ProductDeploymentPreparation(string sid, string expectedToolSha)
        {
            this.sid=sid;this.expectedToolSha=expectedToolSha;
            start=DateTimeOffset.UtcNow;end=start.AddMinutes(5);
            DemandActor(sid,start,end);
            var drive=new DriveInfo(@"C:\");
            Need(drive.DriveType==DriveType.Fixed && drive.DriveFormat=="NTFS","HOLD_FIXED_NTFS");
            try
            {
                foreach(string path in new[] { @"C:\", @"C:\ProgramData", @"C:\ProgramData\Limitless", Runtime })
                    Capture(path,true);
                foreach(string path in new[] { SignedRoot,ToolFolder,Runtime+@"\Control",Runtime+@"\Control\Approvals",Runtime+@"\Control\DeploymentLedger" })
                    Capture(path,false);
            }
            catch { Dispose();throw; }
        }
        void Capture(string path,bool ancestor)
        {
            var handle=Pin(path);
            try { ulong volume; var receipt=ReadPinned(handle,path,ancestor,out volume);parents.Add(path,(handle,receipt,ancestor)); }
            catch {handle.Dispose();throw;}
        }
        void Check()
        {
            DemandActor(sid,start,end);
            foreach(var entry in parents)
            {
                ulong volume;
                Need(ReadPinned(entry.Value.Handle,entry.Key,entry.Value.Ancestor,out volume)==entry.Value.Receipt,"HOLD_PRODUCT_PARENT_CHANGED");
            }
        }
        SafeFileHandle ReadFile(string path) => CreateFileW(path,0x120089,1,IntPtr.Zero,3,0x02200000,IntPtr.Zero);
        static bool Absent(string path)
        {
            using(var file=OpenPath(path))
            {
                int error=Marshal.GetLastWin32Error();
                if(!file.IsInvalid)return false;
                Need(error==2,"HOLD_PRODUCT_PATH_UNKNOWN");
                return true;
            }
        }
        public string PathObservations()
        {
            Check();
            return (Absent(Tool)?"TOOL_ABSENT":"TOOL_PRESENT")+";"+
                (Absent(Runtime+@"\Code")?"CODE_ABSENT":"CODE_PRESENT")+";"+
                (Absent(Runtime+@"\Evidence")?"EVIDENCE_ABSENT":"EVIDENCE_PRESENT");
        }
        public FileStream OpenSigned(string name)
        {
            Check();
            Need(name=="Supervisor.exe" || name=="Probe.exe" || name=="Limitless.DeploymentAdmin.exe" ||
                name=="ProductSigningManifest.json","HOLD_SIGNED_FILE_NAME");
            string path=Path.Combine(SignedRoot,name);
            var handle=ReadFile(path);
            try
            {
                Win(!handle.IsInvalid);
                ulong volume;ReadPinned(handle,path,false,out volume,false);
                return new FileStream(handle,FileAccess.Read);
            }
            catch {handle.Dispose();throw;}
        }
        static string Hash(Stream stream)
        {
            using(var sha=SHA256.Create())
                return BitConverter.ToString(sha.ComputeHash(stream)).Replace("-","").ToLowerInvariant();
        }
        const string ApprovalRoot = Runtime + @"\Control\Approvals\PendingOperations";
        public void PrepareOperationsDirectory()
        {
            Check();
            using(var prior=OpenPath(ApprovalRoot))
            {
                int error=Marshal.GetLastWin32Error();
                if(!prior.IsInvalid)
                {
                    Capture(ApprovalRoot,false);
                    return;
                }
                Need(error==2,"HOLD_PENDING_DIRECTORY_STATUS");
            }
            IntPtr descriptor;uint size;
            Win(ConvertStringSecurityDescriptorToSecurityDescriptorW(InitialSddl,1,out descriptor,out size));
            try
            {
                var attrs=new SecurityAttributes{Size=(uint)Marshal.SizeOf(typeof(SecurityAttributes)),Descriptor=descriptor,Inherit=0};
                Check();Win(CreateDirectoryW(ApprovalRoot,ref attrs));
                Capture(ApprovalRoot,false);
                Check();
            }
            finally {LocalFree(descriptor);}
        }
        const string TrialId = "deef7539de5b4efcb30e062f9b734342";
        string ApprovalPath(string name)
        {
            Need(name==TrialId+".register-only.approval.json" || name==TrialId+".start-once.approval.json" ||
                name=="FakeOnlyExecution.v2.register-only.json" || name=="FakeOnlyExecution.v2.register-only.sig" ||
                name=="FakeOnlyExecution.v2.json" || name=="FakeOnlyExecution.v2.sig","HOLD_APPROVAL_FILENAME");
            return Path.Combine(ApprovalRoot,name);
        }
        public void RequireApprovalAbsent(string name)
        {
            Check();Need(Absent(ApprovalPath(name)),"HOLD_APPROVAL_ALREADY_EXISTS");
        }
        public void WriteApproval(string name, byte[] bytes, DateTimeOffset expires)
        {
            var path=ApprovalPath(name);
            Check();
            Need(DateTimeOffset.UtcNow < expires && bytes!=null && bytes.Length>0 && bytes.Length<=16384,
                "HOLD_APPROVAL_SIZE_OR_EXPIRY");
            IntPtr descriptor;uint size;
            Win(ConvertStringSecurityDescriptorToSecurityDescriptorW(InitialSddl,1,out descriptor,out size));
            try
            {
                var attrs=new SecurityAttributes{Size=(uint)Marshal.SizeOf(typeof(SecurityAttributes)),Descriptor=descriptor,Inherit=0};
                Check();Need(DateTimeOffset.UtcNow<expires,"HOLD_APPROVAL_EXPIRED");
                using(var handle=CreateProductFile(path,0xc0020000,0,ref attrs,1,0x80200000,IntPtr.Zero))
                {
                    Win(!handle.IsInvalid);
                    ulong volume;string receipt=ReadPinned(handle,path,false,out volume,false);
                    using(var file=new FileStream(handle,FileAccess.ReadWrite))
                    using(var expected=new MemoryStream(bytes,false))
                    {
                        file.Write(bytes,0,bytes.Length);file.Flush(true);file.Position=0;
                        Need(file.Length==bytes.Length && Hash(file)==Hash(expected) &&
                            ReadPinned(handle,path,false,out volume,false)==receipt,"HOLD_APPROVAL_READBACK");
                    }
                }
            }
            finally { LocalFree(descriptor); }
        }
        public void Dispose()
        {
            foreach(var entry in parents)entry.Value.Handle.Dispose();
            parents.Clear();
        }
    }
}

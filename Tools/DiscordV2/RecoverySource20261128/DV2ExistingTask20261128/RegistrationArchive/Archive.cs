using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Win32.SafeHandles;

public static partial class LimitlessFailedRegistrationArchive20261128
{
    [DllImport("kernel32.dll", EntryPoint="CreateFileW", CharSet=CharSet.Unicode, SetLastError=true)]
    private static extern SafeFileHandle CreateReplacement(string path,uint access,uint share,ref SecurityAttributes security,uint creation,uint flags,IntPtr template);
    [DllImport("kernel32.dll",SetLastError=true)]
    [return:MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetFileInformationByHandle(SafeFileHandle file,int informationClass,byte[] information,uint bytes);

    private static void RenameWithoutReplacement(SafeFileHandle handle,string destination)
    {
        Need(IntPtr.Size==8,"HOLD_X64_REQUIRED");
        byte[] name=Encoding.Unicode.GetBytes(destination);
        // FILE_RENAME_INFO x64: ReplaceIfExists=false, RootDirectory=NULL, 이름 길이와 절대 경로.
        byte[] info=new byte[20+name.Length+2];
        Array.Copy(BitConverter.GetBytes(name.Length),0,info,16,4);
        Array.Copy(name,0,info,20,name.Length);
        Win(SetFileInformationByHandle(handle,3,info,(uint)info.Length));
    }
    public sealed class Archive : IDisposable
    {
        const string Runtime=@"C:\ProgramData\Limitless\DiscordV2";
        const string Control=Runtime+@"\Control";
        const string Code=Runtime+@"\Code";
        const string Evidence=Runtime+@"\Evidence";
        const string Approvals=Control+@"\Approvals";
        const string Pending=Approvals+@"\PendingOperations";
        const string Consumption=Control+@"\DeploymentLedger";
        const string ArchiveRoot=Control+@"\ExistingTaskFailedRegistrationArchive20261128";
        const string Trial="deef7539de5b4efcb30e062f9b734342";
        readonly List<(string Path,SafeFileHandle Handle,string Identity,bool Ancestor)> pinned=new List<(string,SafeFileHandle,string,bool)>();
        readonly string actor,worker;
        readonly DateTimeOffset opened,deadline;
        readonly ulong volume;
        bool begun;

        public Archive(string actor,string worker)
        {
            this.actor=actor;this.worker=worker;
            opened=DateTimeOffset.UtcNow;deadline=opened.AddMinutes(5);
            DemandActor(actor,opened,deadline);
            var drive=new DriveInfo(@"C:\");
            Need(drive.DriveType==DriveType.Fixed&&drive.DriveFormat=="NTFS","HOLD_REG_ARCHIVE_NTFS");
            try
            {
                volume=Add(Root,true);
                foreach(var p in new[]{Parent,@"C:\ProgramData\Limitless",Runtime,Code,Evidence})
                    Need(Add(p,true)==volume,"HOLD_REG_ARCHIVE_VOLUME");
                foreach(var p in new[]{Control,Approvals,Pending,Consumption})
                    Need(Add(p,false)==volume,"HOLD_REG_ARCHIVE_VOLUME");
            }
            catch{Dispose();throw;}
        }
        ulong Add(string path,bool ancestor)
        {
            var h=Pin(path);
            try
            {
                string receipt=ReadPinned(h,path,ancestor,out ulong current);
                pinned.Add((path,h,receipt,ancestor));return current;
            }
            catch{h.Dispose();throw;}
        }
        void Check()
        {
            DemandActor(actor,opened,deadline);
            foreach(var p in pinned)
            {
                string current=ReadPinned(p.Handle,p.Path,p.Ancestor,out ulong v);
                Need(v==volume&&current==p.Identity,"HOLD_REG_ARCHIVE_PARENT_CHANGED");
            }
        }
        static void Absent(string path)
        {
            using(var h=OpenPath(path))
            {
                int error=Marshal.GetLastWin32Error();
                Need(h.IsInvalid&&error==2,"HOLD_REG_ARCHIVE_NOT_ABSENT");
            }
        }
        static string Resolve(string area,string name)
        {
            bool operation=name==Trial+".register-only.approval.json" ||
                name=="FakeOnlyExecution.v2.register-only.json" || name=="FakeOnlyExecution.v2.register-only.sig";
            if(area=="Code" && operation)return Path.Combine(Code,name);
            if(area=="Pending" && operation)return Path.Combine(Pending,name);
            bool record=name==Trial+".admin.register-intent" ||
                Regex.IsMatch(name,@"\A"+Trial+@"\.admin\.hold-[0-9a-f]{32}\z");
            if(area=="Evidence" && record)return Path.Combine(Evidence,name);
            throw new InvalidOperationException("HOLD_REG_ARCHIVE_SCOPE");
        }
        string Identity(SafeFileHandle h,string path,string area)
        {
            bool workerReads=area=="Code"||area=="Evidence";
            string identity=ReadPinned(h,path,workerReads,out ulong current,false);
            Need(current==volume,"HOLD_REG_ARCHIVE_VOLUME");
            if(!workerReads)return identity;
            var sd=new RawSecurityDescriptor(ReadDescriptor(h),0);
            bool ownerRights=area=="Evidence";
            Need(sd.Owner.Value==AdminSid&&(sd.ControlFlags&ControlFlags.DiscretionaryAclProtected)!=0 &&
                sd.DiscretionaryAcl.Count==(ownerRights?4:3),"HOLD_REG_ARCHIVE_DACL");
            var trustees=new HashSet<string>(StringComparer.Ordinal){AdminSid,SystemSid,worker};
            if(ownerRights)trustees.Add("S-1-3-4");
            foreach(GenericAce entry in sd.DiscretionaryAcl)
            {
                var ace=entry as CommonAce;
                Need(ace!=null&&!ace.IsCallback&&ace.AceFlags==AceFlags.None&&trustees.Remove(ace.SecurityIdentifier.Value),
                    "HOLD_REG_ARCHIVE_ACE");
                string sid=ace.SecurityIdentifier.Value;
                if(sid=="S-1-3-4")Need(ace.AceQualifier==AceQualifier.AccessDenied&&ace.AccessMask==0x000d0000,
                    "HOLD_REG_ARCHIVE_OWNER_ACE");
                else Need(ace.AceQualifier==AceQualifier.AccessAllowed&&
                    (sid==worker?ace.AccessMask==0x120089:ace.AccessMask==0x1f01ff||ownerRights&&ace.AccessMask==0x10000000),
                    "HOLD_REG_ARCHIVE_RIGHTS");
            }
            return identity;
        }
        static string Hash(Stream file)
        {
            file.Position=0;
            using(var sha=SHA256.Create())
                return BitConverter.ToString(sha.ComputeHash(file)).Replace("-","").ToLowerInvariant();
        }
        public string Snapshot(string area,string name,string expectedSha)
        {
            Check();string path=Resolve(area,name);
            using(var h=CreateFileW(path,0x120089,1,IntPtr.Zero,3,0x02200000,IntPtr.Zero))
            {
                Win(!h.IsInvalid);string identity=Identity(h,path,area);
                using(var file=new FileStream(h,FileAccess.Read))
                {
                    Need(Hash(file)==expectedSha,"HOLD_REG_ARCHIVE_INPUT_SHA");
                    return identity;
                }
            }
        }
        public FileStream OpenLedger(string approval)
        {
            Check();
            Need(Guid.TryParseExact(approval,"N",out Guid id)&&id!=Guid.Empty&&id.ToString("N")==approval,"HOLD_REG_ARCHIVE_APPROVAL_ID");
            string path=Path.Combine(Consumption,approval+".deploy-consumed");
            var h=CreateFileW(path,0x120089,1,IntPtr.Zero,3,0x02200000,IntPtr.Zero);
            try{Win(!h.IsInvalid);Identity(h,path,"Ledger");return new FileStream(h,FileAccess.Read);}
            catch{h.Dispose();throw;}
        }
        public void Begin(byte[] evidence)
        {
            Check();Need(!begun&&evidence!=null&&evidence.Length>0&&evidence.Length<=65536,"HOLD_REG_ARCHIVE_BEGIN");
            Absent(ArchiveRoot);
            IntPtr sd;uint size;
            Win(ConvertStringSecurityDescriptorToSecurityDescriptorW(InitialSddl,1,out sd,out size));
            try
            {
                var attrs=new SecurityAttributes{Size=(uint)Marshal.SizeOf(typeof(SecurityAttributes)),Descriptor=sd,Inherit=0};
                Check();Win(CreateDirectoryW(ArchiveRoot,ref attrs));
                Need(Add(ArchiveRoot,false)==volume,"HOLD_REG_ARCHIVE_VOLUME");
                string path=Path.Combine(ArchiveRoot,"RegistrationEvidence.json");
                using(var h=CreateReplacement(path,0xc0020000,0,ref attrs,1,0x80200000,IntPtr.Zero))
                {
                    Win(!h.IsInvalid);string receipt=Identity(h,path,"Record");
                    using(var f=new FileStream(h,FileAccess.ReadWrite))
                    using(var expected=new MemoryStream(evidence,false))
                    {
                        f.Write(evidence,0,evidence.Length);f.Flush(true);
                        Need(f.Length==expected.Length&&Hash(f)==Hash(expected)&&Identity(h,path,"Record")==receipt,
                            "HOLD_REG_ARCHIVE_EVIDENCE_READBACK");
                    }
                }
                begun=true;
            }
            finally{LocalFree(sd);}
        }
        public void Move(string area,string name,string expectedSha,string expectedIdentity)
        {
            Check();Need(begun,"HOLD_REG_ARCHIVE_NOT_STARTED");
            string src=Resolve(area,name),dst=Path.Combine(ArchiveRoot,area+"-"+name);
            Absent(dst);
            using(var h=CreateFileW(src,0x130089,0,IntPtr.Zero,3,0x02200000,IntPtr.Zero))
            {
                Win(!h.IsInvalid);
                using(var f=new FileStream(h,FileAccess.Read))
                {
                    Need(Identity(h,src,area)==expectedIdentity&&Hash(f)==expectedSha,"HOLD_REG_ARCHIVE_BEFORE_MOVE");
                    Check();RenameWithoutReplacement(h,dst);
                    Need(Identity(h,dst,area)==expectedIdentity&&Hash(f)==expectedSha,"HOLD_REG_ARCHIVE_AFTER_MOVE");
                }
            }
            Absent(src);
        }
        public void Dispose()
        {
            for(int i=pinned.Count-1;i>=0;i--)pinned[i].Handle.Dispose();
            pinned.Clear();
        }
    }
}

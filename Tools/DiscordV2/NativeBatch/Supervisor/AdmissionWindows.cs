using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using Microsoft.Win32.SafeHandles;

// 실제 Windows 보안 API 어댑터입니다. 이번 단위 테스트에서는 호출하지 않습니다.
internal static class AdmissionWindows
{
    internal enum KernelKind { Process,Thread,Token,Job }
    [StructLayout(LayoutKind.Sequential)] internal struct Attributes { internal uint Size; internal nint Descriptor; internal int Inherit; }
    [StructLayout(LayoutKind.Sequential)] internal struct Mapping { internal uint Read, Write, Execute, All; }
    [DllImport("kernel32.dll", CharSet=CharSet.Unicode, SetLastError=true)]
    private static extern SafeFileHandle CreateFileW(string path, uint access, uint share, nint attributes, uint disposition, uint flags, nint template);
    [DllImport("kernel32.dll", CharSet=CharSet.Unicode, SetLastError=true)]
    private static extern SafeFileHandle CreateFileW(string path, uint access, uint share, ref Attributes attributes, uint disposition, uint flags, nint template);
    [DllImport("kernel32.dll", CharSet=CharSet.Unicode, SetLastError=true)] private static extern uint GetFinalPathNameByHandleW(SafeFileHandle handle, StringBuilder path, uint length, uint flags);
    [DllImport("kernel32.dll", SetLastError=true)] private static extern bool GetFileInformationByHandleEx(SafeFileHandle handle, int kind, out FileAttributesInfo info, uint size);
    [StructLayout(LayoutKind.Sequential)] internal struct FileAttributesInfo { internal uint Attributes, ReparseTag; }
    [DllImport("advapi32.dll")] private static extern uint GetSecurityInfo(nint handle, int kind, uint information, out nint owner, out nint group, out nint dacl, out nint sacl, out nint descriptor);
    [DllImport("advapi32.dll")] private static extern uint GetSecurityDescriptorLength(nint descriptor);
    [DllImport("advapi32.dll", SetLastError=true)] private static extern bool AccessCheck(byte[] descriptor, nint token, uint desired, ref Mapping mapping, nint privileges, ref uint length, out uint granted, out int allowed);
    [DllImport("advapi32.dll", SetLastError=true)] private static extern bool DuplicateToken(nint token, int level, out nint duplicate);
    [DllImport("advapi32.dll", CharSet=CharSet.Unicode, SetLastError=true)] private static extern bool ConvertStringSecurityDescriptorToSecurityDescriptorW(string text, uint revision, out nint descriptor, out uint length);
    [DllImport("advapi32.dll", SetLastError=true)] private static extern bool SetKernelObjectSecurity(nint handle, uint information, nint descriptor);
    [DllImport("advapi32.dll", CharSet=CharSet.Unicode, SetLastError=true)] private static extern bool LookupPrivilegeNameW(string? system, ref Native.Luid luid, StringBuilder name, ref uint size);
    [DllImport("kernel32.dll")] private static extern nint LocalFree(nint memory);
    [DllImport("kernel32.dll", SetLastError=true)] internal static extern bool DuplicateHandle(nint sourceProcess, nint source, nint targetProcess, out nint target, uint access, bool inherit, uint options);
    [DllImport("kernel32.dll")] internal static extern uint GetCurrentProcessId();
    [DllImport("kernel32.dll", SetLastError=true)] internal static extern nint OpenProcess(uint access, bool inherit, uint pid);
    internal static void Check(bool value) { if (!value) throw new Win32Exception(Marshal.GetLastWin32Error()); }

    internal static byte[] Descriptor(nint handle, int kind)
    {
        // AccessCheck에는 Owner와 Group SID가 모두 필요합니다. DACL만 맞아도 Group 누락은1338입니다.
        uint error = GetSecurityInfo(handle, kind, 7, out _, out _, out _, out _, out var descriptor);
        if (error != 0) throw new Win32Exception((int)error);
        try
        {
            uint size = GetSecurityDescriptorLength(descriptor);
            AdmissionPolicy.Require(size is > 0 and <= 65536, "SECURITY_DESCRIPTOR_SIZE");
            byte[] bytes = new byte[size]; Marshal.Copy(descriptor, bytes, 0, bytes.Length); return bytes;
        }
        finally { LocalFree(descriptor); }
    }

    internal static bool Allowed(byte[] descriptor, nint primary, uint desired)
        => AccessDecision(descriptor,primary,desired).Allowed;
    internal static (bool Allowed,uint Granted,int Status,int Error) AccessDecision(byte[] descriptor,nint primary,uint desired)
    {
        Check(DuplicateToken(primary, 2, out var token));
        nint privileges = Marshal.AllocHGlobal(65536);
        try
        {
            uint length = 65536;
            var mapping = new Mapping { Read=0x120089, Write=0x120116, Execute=0x1200a0, All=0x1f01ff };
            Check(AccessCheck(descriptor, token, desired, ref mapping, privileges, ref length, out uint granted, out int allowed));
            int error=Marshal.GetLastWin32Error();
            return (allowed!=0 && (granted & desired)==desired,granted,allowed,error);
        }
        finally { Marshal.FreeHGlobal(privileges); Native.CloseHandle(token); }
    }

    internal static nint Token(nint process)
    {
        Check(Native.OpenProcessToken(process, 0x6000a, out nint token)); // QUERY/DUPLICATE/READ_CONTROL/WRITE_DAC, 자기 token DACL 보호용
        return token;
    }

    internal static void LimitedToken(nint token)
    {
        TokenPolicy.NoAdministrativeGroups(token);
        byte[] bytes = TokenBytes(token, 3);
        uint count = BitConverter.ToUInt32(bytes);
        AdmissionPolicy.Require(count <= 256 && 4L+count*12L <= bytes.Length, "PRIVILEGE_LAYOUT");
        // disabled로 보유한 위험 특권도 나중에 다시 켤 수 있으므로 거부합니다.
        string[] denied = ["SeDebugPrivilege","SeImpersonatePrivilege","SeAssignPrimaryTokenPrivilege","SeTcbPrivilege","SeBackupPrivilege","SeRestorePrivilege","SeTakeOwnershipPrivilege","SeLoadDriverPrivilege","SeSecurityPrivilege","SeCreateTokenPrivilege","SeRelabelPrivilege","SeDelegateSessionUserImpersonatePrivilege"];
        for (int i=0; i<count; i++)
        {
            int offset=4+i*12; var luid = new Native.Luid { Low=BitConverter.ToUInt32(bytes,offset), High=BitConverter.ToInt32(bytes,offset+4) };
            var name=new StringBuilder(256); uint size=256;
            Check(LookupPrivilegeNameW(null,ref luid,name,ref size));
            AdmissionPolicy.Require(!denied.Contains(name.ToString(),StringComparer.Ordinal), "DANGEROUS_PRIVILEGE_PRESENT");
        }
    }
    private static byte[] TokenBytes(nint token, int kind)
    {
        bool first=Native.GetTokenInformation(token,kind,0,0,out uint size);
        AdmissionPolicy.Require(!first && Marshal.GetLastWin32Error()==122 && size is > 0 and <= 65536,"TOKEN_QUERY_SIZE");
        nint buffer=Marshal.AllocHGlobal((int)size);
        try { Check(Native.GetTokenInformation(token,kind,buffer,size,out uint actual)); AdmissionPolicy.Require(actual<=size,"TOKEN_QUERY_LENGTH"); byte[] data=new byte[actual]; Marshal.Copy(buffer,data,0,data.Length); return data; }
        finally { Marshal.FreeHGlobal(buffer); }
    }

    internal static SafeFileHandle Pin(string path, bool directory)
    {
        var file=CreateFileW(path,0x120080U|(directory?0U:1U),1,0,3,0x00200000U|(directory?0x02000000U:0U),0);
        if(file.IsInvalid){int error=Marshal.GetLastWin32Error();file.Dispose();throw new Win32Exception(error);}
        try
        {
            Check(GetFileInformationByHandleEx(file,9,out var info,8));
            AdmissionPolicy.Require((info.Attributes&0x400)==0 && ((info.Attributes&0x10)!=0)==directory,"REPARSE_OR_FILE_KIND");
            var final=new StringBuilder(32768); uint length=GetFinalPathNameByHandleW(file,final,(uint)final.Capacity,0);
            AdmissionPolicy.Require(length>0 && length<final.Capacity && string.Equals(final.ToString(),"\\\\?\\"+Path.GetFullPath(path),StringComparison.OrdinalIgnoreCase),"FINAL_PATH_CHANGED");
            Check(Native.GetHandleInformation(file.DangerousGetHandle(),out uint flags));
            AdmissionPolicy.Require((flags&1)==0,"FILE_HANDLE_INHERIT"); return file;
        }
        catch { file.Dispose(); throw; }
    }

    internal sealed class KernelSecurity : IDisposable
    {
        internal nint Pointer;
        internal KernelSecurity(string worker, bool writableFile=false, bool queryToken=false, bool ownerAdmin=false)
        {
            // OWNER RIGHTS는 생성자가 소유자라는 이유로 DACL/삭제 권리를 되찾는 것을 차단합니다.
            string workerAce=writableFile?$"(A;;0x00120003;;;{worker})":queryToken?$"(A;;0x0002000a;;;{worker})":ownerAdmin?$"(A;;0x00120089;;;{worker})":"";
            Check(ConvertStringSecurityDescriptorToSecurityDescriptorW((ownerAdmin?"O:BA":"")+"D:P(D;;0x000d0000;;;OW)(A;;GA;;;SY)(A;;GA;;;BA)"+workerAce,1,out Pointer,out _));
        }
        internal Attributes Attributes => new() { Size=(uint)Marshal.SizeOf<Attributes>(), Descriptor=Pointer, Inherit=0 };
        internal void Harden(nint handle,KernelKind kind)
        {
            Check(SetKernelObjectSecurity(handle,0x80000004,Pointer));
            byte[] actual=Descriptor(handle,6);
            var sd=new RawSecurityDescriptor(actual,0);
            AdmissionPolicy.Require((sd.ControlFlags&ControlFlags.DiscretionaryAclProtected)!=0 && sd.DiscretionaryAcl is not null,"KERNEL_DACL_READBACK");
            // 정확한 DACL 바이트가 적용되지 않았다면 계속하지 않습니다.
            byte[] expected=new byte[GetSecurityDescriptorLength(Pointer)]; Marshal.Copy(Pointer,expected,0,expected.Length);
            var mapped=new RawSecurityDescriptor(expected,0);
            uint all=kind switch{KernelKind.Process or KernelKind.Thread=>0x1fffff,KernelKind.Token=>0xf01ff,KernelKind.Job=>0x1f003f,_=>throw new InvalidOperationException("KERNEL_OBJECT_KIND")};
            // Windows가 객체별로 확장한 GENERIC_ALL과 같은 의미로 비교합니다. ACE를 허용/삭제하지 않습니다.
            foreach(GenericAce item in mapped.DiscretionaryAcl!)
            {
                AdmissionPolicy.Require(item is CommonAce {IsCallback:false},"KERNEL_EXPECTED_ACE");var ace=(CommonAce)item;
                uint mask=unchecked((uint)ace.AccessMask);
                if((mask&0xf0000000)!=0){AdmissionPolicy.Require(mask==0x10000000,"KERNEL_UNEXPECTED_GENERIC_MASK");ace.AccessMask=unchecked((int)all);}
            }
            AdmissionPolicy.Require(sd.DiscretionaryAcl!.BinaryLength==mapped.DiscretionaryAcl!.BinaryLength,"KERNEL_DACL_LENGTH");
            byte[] a=new byte[sd.DiscretionaryAcl.BinaryLength], b=new byte[a.Length];
            sd.DiscretionaryAcl.GetBinaryForm(a,0);mapped.DiscretionaryAcl.GetBinaryForm(b,0);
            AdmissionPolicy.Require(a.SequenceEqual(b),"KERNEL_DACL_CHANGED");
        }
        public void Dispose(){if(Pointer!=0){LocalFree(Pointer);Pointer=0;}}
    }

    internal static Stream CreateRecord(string path)=>CreateRecord(path,false);
    internal static Stream CreateAdminRecord(string path)=>CreateRecord(path,true);
    private static Stream CreateRecord(string path,bool admin)
    {
        using var security=new KernelSecurity(Native.Worker,!admin,ownerAdmin:admin); var attributes=security.Attributes;
        var handle=CreateFileW(path,0x120003,0,ref attributes,1,0x80000000,0); // CREATE_NEW / write-through / 공유 없음
        if(handle.IsInvalid){int error=Marshal.GetLastWin32Error();handle.Dispose();throw new Win32Exception(error);}
        try
        {
            Check(Native.GetHandleInformation(handle.DangerousGetHandle(),out uint flags));
            AdmissionPolicy.Require((flags&1)==0,"RECORD_HANDLE_INHERIT");
            var sd=new RawSecurityDescriptor(Descriptor(handle.DangerousGetHandle(),1),0);
            if(admin)
            {
                AdmissionPolicy.Require(AdmissionPolicy.TrustedOwner(sd.Owner?.Value??"") && (sd.ControlFlags&ControlFlags.DiscretionaryAclProtected)!=0 && sd.DiscretionaryAcl is not null,"ADMIN_RECORD_SECURITY");
                foreach(GenericAce item in sd.DiscretionaryAcl!)
                    AdmissionPolicy.Require(item is CommonAce {IsCallback:false} a && (AdmissionPolicy.TrustedOwner(a.SecurityIdentifier.Value) || a.SecurityIdentifier.Value==AdmissionPolicy.OwnerRights && a.AceQualifier==AceQualifier.AccessDenied || a.SecurityIdentifier.Value==Native.Worker&&a.AceQualifier==AceQualifier.AccessAllowed&&unchecked((uint)a.AccessMask)==0x120089),"ADMIN_RECORD_ACE");
            }
            else AdmissionPolicy.RecordDescriptor(sd,Native.Worker);
            return new FileStream(handle,FileAccess.ReadWrite);
        }
        catch { handle.Dispose(); throw; }
    }
}

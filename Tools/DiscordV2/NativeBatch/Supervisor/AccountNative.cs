using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;

// 로컬 계정과 이미 존재하는 권리만 조회합니다. 계정/암호/그룹/LSA 변경 API는 없습니다.
internal static class AccountNative
{
    [StructLayout(LayoutKind.Sequential)] internal struct User4
    {
        internal nint Name,Password;internal uint PasswordAge,Privilege;internal nint Home,Comment;internal uint Flags;internal nint Script;internal uint AuthFlags;
        internal nint FullName,UserComment,Parameters,Workstations;internal uint LastLogon,LastLogoff,Expires,MaxStorage,UnitsPerWeek;
        internal nint LogonHours;internal uint BadPasswords,Logons;internal nint LogonServer;internal uint Country,CodePage;
        internal nint Sid;internal uint PrimaryGroup;internal nint Profile,HomeDrive;internal uint PasswordExpired;
    }
    [StructLayout(LayoutKind.Sequential)] internal struct ObjectAttributes
    {internal uint Length;internal nint Root,Name;internal uint Attributes;internal nint Descriptor,Qos;}
    [DllImport("advapi32.dll",CharSet=CharSet.Unicode,SetLastError=true)]private static extern bool LookupAccountSidW(string? system,byte[] sid,StringBuilder name,ref uint names,StringBuilder domain,ref uint domains,out int use);
    [DllImport("advapi32.dll",CharSet=CharSet.Unicode,SetLastError=true)]private static extern bool LookupAccountNameW(string? system,string name,byte[] sid,ref uint size,StringBuilder domain,ref uint domains,out int use);
    [DllImport("netapi32.dll",CharSet=CharSet.Unicode)]private static extern uint NetUserGetInfo(string? server,string name,uint level,out nint buffer);
    [DllImport("netapi32.dll",CharSet=CharSet.Unicode)]private static extern uint NetUserGetLocalGroups(string? server,string name,uint level,uint flags,out nint buffer,uint max,out uint entries,out uint total);
    [DllImport("netapi32.dll",CharSet=CharSet.Unicode)]private static extern uint NetUserGetGroups(string? server,string name,uint level,out nint buffer,uint max,out uint entries,out uint total);
    [DllImport("netapi32.dll")]private static extern uint NetApiBufferFree(nint buffer);
    [DllImport("advapi32.dll")]private static extern uint LsaOpenPolicy(nint system,ref ObjectAttributes attributes,uint access,out nint policy);
    [DllImport("advapi32.dll")]private static extern uint LsaEnumerateAccountsWithUserRight(nint policy,ref Native.LsaText right,out nint buffer,out uint count);
    [DllImport("advapi32.dll")]private static extern uint LsaFreeMemory(nint buffer);
    [DllImport("advapi32.dll")]private static extern uint LsaClose(nint policy);
    [DllImport("advapi32.dll")]private static extern uint LsaNtStatusToWinError(uint status);
    private static void Net(uint error){if(error!=0)throw new Win32Exception((int)error);}
    private static void Lsa(uint error){if(error!=0)throw new Win32Exception((int)LsaNtStatusToWinError(error));}
    private static byte[] SidBytes(string text){var sid=new SecurityIdentifier(text);byte[] bytes=new byte[sid.BinaryLength];sid.GetBinaryForm(bytes,0);return bytes;}
    internal static AccountObservation Read(DateTimeOffset now,bool requireDirectBatch)
    {
        var name=new StringBuilder(256);var domain=new StringBuilder(256);uint nc=256,dc=256;
        AdmissionWindows.Check(LookupAccountSidW(null,SidBytes(Native.Worker),name,ref nc,domain,ref dc,out int use));
        AdmissionPolicy.Require(use==1 && domain.ToString().Equals(Environment.MachineName,StringComparison.OrdinalIgnoreCase),"ACCOUNT_NOT_LOCAL_USER");
        Net(NetUserGetInfo(null,name.ToString(),4,out nint info));
        User4 user;
        try{user=Marshal.PtrToStructure<User4>(info);AdmissionPolicy.Require(user.Sid!=0 && new SecurityIdentifier(user.Sid).Value==Native.Worker,"ACCOUNT_SID_MISMATCH");}
        finally{NetApiBufferFree(info);}
        var groups=new HashSet<string>(StringComparer.Ordinal){Native.Worker,"S-1-1-0","S-1-5-11","S-1-5-3","S-1-5-113"};
        void AddGroups(bool local)
        {
            uint status;uint count,total;nint buffer;
            if(local)status=NetUserGetLocalGroups(null,name.ToString(),0,1,out buffer,uint.MaxValue,out count,out total);
            else status=NetUserGetGroups(null,name.ToString(),0,out buffer,uint.MaxValue,out count,out total);
            Net(status);
            try
            {
                AdmissionPolicy.Require(count==total && count<=1024,"ACCOUNT_GROUPS_INCOMPLETE");
                for(int i=0;i<count;i++)
                {
                    string group=Marshal.PtrToStringUni(Marshal.ReadIntPtr(buffer,i*nint.Size)) ?? throw new InvalidOperationException("GROUP_NULL");
                    byte[] bytes=new byte[256];uint size=256,domainSize=256;var groupDomain=new StringBuilder(256);
                    AdmissionWindows.Check(LookupAccountNameW(null,group,bytes,ref size,groupDomain,ref domainSize,out int type));
                    AdmissionPolicy.Require(type is 2 or 4 or 5 && (groupDomain.ToString().Equals(Environment.MachineName,StringComparison.OrdinalIgnoreCase)||groupDomain.ToString().Equals("BUILTIN",StringComparison.OrdinalIgnoreCase)),"GROUP_DOMAIN_UNKNOWN");
                    groups.Add(new SecurityIdentifier(bytes,0).Value);
                }
            }
            finally{if(buffer!=0)NetApiBufferFree(buffer);}
        }
        AddGroups(true);AddGroups(false);
        AdmissionPolicy.Require(!groups.Overlaps(new[]{"S-1-5-32-544","S-1-5-32-547","S-1-5-32-548","S-1-5-32-549","S-1-5-32-550","S-1-5-32-551"}),"ACCOUNT_PRIVILEGED_GROUP");
        var attributes=new ObjectAttributes{Length=(uint)Marshal.SizeOf<ObjectAttributes>()};Lsa(LsaOpenPolicy(0,ref attributes,0x801,out nint policy));
        try
        {
            var allow=Rights(policy,"SeBatchLogonRight");var deny=Rights(policy,"SeDenyBatchLogonRight");
            var batch=Batch(groups,allow,deny,KnownNonMember);
            // Password 등록이 간접권리를 직접권리로 바꾸는 경로를 허용하지 않습니다.
            if(requireDirectBatch)AdmissionPolicy.Require(allow.Contains(Native.Worker),"DIRECT_BATCH_REQUIRED_NO_GRANT");
            DateTimeOffset expiry=user.Expires==uint.MaxValue?DateTimeOffset.MaxValue:DateTimeOffset.FromUnixTimeSeconds(user.Expires);
            return new(Native.Worker,(user.Flags&2)==0 && user.PasswordExpired==0,(user.Flags&0x10)!=0,expiry,batch.Known,batch.Allowed);
        }
        finally{LsaClose(policy);}
    }
    // 실제 logon token에 붙을 문맥 SID를 NetUser 그룹 목록만으로 부재라고 단정하지 않습니다.
    // 회원 아님을 확인할 수 없는 allow/deny SID는 UNKNOWN/HOLD로 남깁니다.
    internal static (bool Known,bool Allowed) Batch(HashSet<string> members,HashSet<string> allow,HashSet<string> deny,Func<string,bool> knownNonMember)
    {
        if(deny.Overlaps(members))return(true,false);
        if(deny.Any(sid=>!members.Contains(sid)&&!knownNonMember(sid)))return(false,false);
        if(allow.Overlaps(members))return(true,true);
        if(allow.Any(sid=>!knownNonMember(sid)))return(false,false);
        return(true,false);
    }
    private static bool KnownNonMember(string sid)
    {
        var name=new StringBuilder(256);var domain=new StringBuilder(256);uint nc=256,dc=256;
        if(!LookupAccountSidW(null,SidBytes(sid),name,ref nc,domain,ref dc,out int use))return false;
        return use is 1 or 2 or 4 && (domain.ToString().Equals(Environment.MachineName,StringComparison.OrdinalIgnoreCase)||domain.ToString().Equals("BUILTIN",StringComparison.OrdinalIgnoreCase));
    }
    private static HashSet<string> Rights(nint policy,string right)
    {
        nint text=Marshal.StringToHGlobalUni(right),buffer=0;
        try
        {
            var name=new Native.LsaText{Length=checked((ushort)(right.Length*2)),Maximum=checked((ushort)((right.Length+1)*2)),Buffer=text};
            uint status=LsaEnumerateAccountsWithUserRight(policy,ref name,out buffer,out uint count);
            if(status==0x8000001a)return [];
            Lsa(status);AdmissionPolicy.Require(count<=65536,"RIGHTS_COUNT");var result=new HashSet<string>();
            for(int i=0;i<count;i++){nint sid=Marshal.ReadIntPtr(buffer,i*nint.Size);AdmissionPolicy.Require(sid!=0,"RIGHTS_SID_UNKNOWN");result.Add(new SecurityIdentifier(sid).Value);}
            return result;
        }
        finally{if(buffer!=0)LsaFreeMemory(buffer);Marshal.FreeHGlobal(text);}
    }
}

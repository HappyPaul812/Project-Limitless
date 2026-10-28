using System.Runtime.InteropServices;
using System.Security.Principal;

[assembly: DefaultDllImportSearchPaths(DllImportSearchPath.System32)]

// TOKEN_QUERY만으로 관리자/위험 그룹을 거부합니다. primary token에 대한 역할 확인을 위해
// 불필요하게 TOKEN_DUPLICATE를 요구하지 않으며 deny-only 관리자 SID도 허용하지 않습니다.
internal static class TokenPolicy
{
    internal static void Groups(IEnumerable<string> groups)
    {
        foreach(string sid in groups)
            Native.Need(sid is not ("S-1-5-32-544" or "S-1-5-32-547" or "S-1-5-32-548" or "S-1-5-32-549" or "S-1-5-32-550" or "S-1-5-32-551"),"PRIVILEGED_GROUP");
    }
    internal static void NoAdministrativeGroups(nint token)
    {
        bool first=Native.GetTokenInformation(token,2,0,0,out uint size);
        Native.Need(!first && Marshal.GetLastWin32Error()==122 && size is >0 and <=65536,"GROUP_QUERY_SIZE");
        nint p=Marshal.AllocHGlobal((int)size);
        try
        {
            Native.Check(Native.GetTokenInformation(token,2,p,size,out uint actual));int count=Marshal.ReadInt32(p);
            int stride=nint.Size==8?16:8,start=nint.Size;
            Native.Need(count>=0 && start+(long)count*stride<=actual && actual<=size,"GROUP_LAYOUT");
            var groups=new List<string>();for(int i=0;i<count;i++)groups.Add(new SecurityIdentifier(Marshal.ReadIntPtr(p,start+i*stride)).Value);
            Groups(groups);
        }
        finally{Marshal.FreeHGlobal(p);}
    }
}

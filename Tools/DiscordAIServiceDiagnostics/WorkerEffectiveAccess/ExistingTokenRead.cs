// 기존 프로세스 토큰만 조회합니다. 로그온/프로세스 생성/impersonation/권한 활성화 API는 없습니다.
using System;using System.Collections.Generic;using System.ComponentModel;using System.Runtime.InteropServices;using System.Security.Principal;using System.Security.AccessControl;using Microsoft.Win32.SafeHandles;
public static class R2ExistingTokenRead {
 public const string Worker="S-1-5-21-2858504392-1958168942-1662906075-1010";
 [StructLayout(LayoutKind.Sequential)]struct Ft{public uint Low,High;public long Value=>((long)High<<32)|Low;}
 [StructLayout(LayoutKind.Sequential)]struct Luid{public uint Low;public int High;public string Id=>unchecked((uint)High).ToString("x8")+Low.ToString("x8");}
 [StructLayout(LayoutKind.Sequential)]struct Stat{public Luid Id,Auth;public long Expires;public int Type,Level;public uint Charged,Available,Groups,Privileges;public Luid Modified;}
 [StructLayout(LayoutKind.Sequential)]struct SidAttr{public IntPtr Sid;public uint Attributes;}
 [StructLayout(LayoutKind.Sequential)]struct GroupHead{public uint Count;public SidAttr First;}
 [StructLayout(LayoutKind.Sequential)]struct Text{public ushort Length,Max;public IntPtr Buffer;}
 [StructLayout(LayoutKind.Sequential)]struct Logon{public uint Size;public Luid Id;public Text User,Domain,AuthenticationPackage;public uint Type,Session;public IntPtr Sid;public long Time;}
 [StructLayout(LayoutKind.Sequential)]struct Mapping{public uint Read,Write,Execute,All;}
 [DllImport("kernel32.dll",SetLastError=true)]static extern SafeProcessHandle OpenProcess(uint access,bool inherit,int pid);
 [DllImport("kernel32.dll",SetLastError=true)]static extern bool GetProcessTimes(SafeProcessHandle h,out Ft c,out Ft e,out Ft k,out Ft u);
 [DllImport("kernel32.dll",SetLastError=true)]static extern bool IsProcessInJob(SafeProcessHandle h,IntPtr job,out bool present);
 [DllImport("advapi32.dll",SetLastError=true)]static extern bool OpenProcessToken(SafeProcessHandle h,uint access,out SafeAccessTokenHandle token);
 [DllImport("advapi32.dll",SetLastError=true)]static extern bool GetTokenInformation(SafeAccessTokenHandle h,int kind,IntPtr buffer,uint size,out uint needed);
 [DllImport("advapi32.dll",SetLastError=true)]static extern bool DuplicateToken(SafeAccessTokenHandle token,int level,out SafeAccessTokenHandle duplicate);
 [DllImport("advapi32.dll",SetLastError=true)]static extern bool AccessCheck(byte[] sd,SafeAccessTokenHandle token,uint desired,ref Mapping mapping,IntPtr privileges,ref uint length,out uint granted,[MarshalAs(UnmanagedType.Bool)]out bool allowed);
 [DllImport("secur32.dll")]static extern uint LsaGetLogonSessionData(ref Luid id,out IntPtr data);
 [DllImport("secur32.dll")]static extern uint LsaFreeReturnBuffer(IntPtr data);
 [DllImport("advapi32.dll")]static extern uint LsaNtStatusToWinError(uint status);
 static void Need(bool ok,string tag){if(!ok)throw new InvalidOperationException(tag);}
 static void Check(bool ok,string operation){if(!ok){var e=new Win32Exception(Marshal.GetLastWin32Error());e.Data["operation"]=operation;throw e;}}
 static IntPtr Info(SafeAccessTokenHandle token,int kind,out uint length){bool ok=GetTokenInformation(token,kind,IntPtr.Zero,0,out length);int code=Marshal.GetLastWin32Error();if(ok||code!=122||length==0||length>1048576){var e=new Win32Exception(code);e.Data["operation"]="GetTokenInformation_SIZE_"+kind;throw e;}IntPtr p=Marshal.AllocHGlobal((int)length);try{Check(GetTokenInformation(token,kind,p,length,out length),"GetTokenInformation_"+kind);return p;}catch{Marshal.FreeHGlobal(p);throw;}}
 static T Value<T>(SafeAccessTokenHandle token,int kind)where T:struct{uint size;IntPtr p=Info(token,kind,out size);try{Need(size>=Marshal.SizeOf<T>(),"TOKEN_STRUCT_BOUND");return Marshal.PtrToStructure<T>(p);}finally{Marshal.FreeHGlobal(p);}}
 static List<object> Groups(SafeAccessTokenHandle token,int kind){uint size;IntPtr p=Info(token,kind,out size);try{uint count=unchecked((uint)Marshal.ReadInt32(p));int offset=(int)Marshal.OffsetOf<GroupHead>("First"),stride=Marshal.SizeOf<SidAttr>();Need(count<=4096&&(count==0||(long)offset+count*stride<=size),"TOKEN_GROUP_BOUND");var rows=new List<object>();for(int n=0;n<count;n++){var s=Marshal.PtrToStructure<SidAttr>(IntPtr.Add(p,offset+n*stride));rows.Add(new Dictionary<string,object>{{"sid",new SecurityIdentifier(s.Sid).Value},{"attributes",s.Attributes},{"enabled",(s.Attributes&4)!=0},{"deny_only",(s.Attributes&16)!=0}});}return rows;}finally{Marshal.FreeHGlobal(p);}}
 static object Test(byte[] sd,SafeAccessTokenHandle token,uint desired){var mapping=new Mapping{Read=0x120089,Write=0x120116,Execute=0x1200a0,All=0x1f01ff};uint length=4096;IntPtr p=Marshal.AllocHGlobal((int)length);try{Marshal.WriteInt32(p,0);uint granted;bool allowed;Check(AccessCheck(sd,token,desired,ref mapping,p,ref length,out granted,out allowed),"AccessCheck_"+desired.ToString("x8"));return new Dictionary<string,object>{{"requested",desired},{"allowed",allowed},{"granted",granted},{"privileges_used",unchecked((uint)Marshal.ReadInt32(p))}};}finally{Marshal.FreeHGlobal(p);}}
 public static object Read(int pid,string createdUtc,string sddl){
  Need(pid>0,"EXISTING_PID_REQUIRED");var expected=DateTimeOffset.Parse(createdUtc);Need(expected.Offset==TimeSpan.Zero,"UTC_REQUIRED");
  var descriptor=new RawSecurityDescriptor(sddl);Need(descriptor.Owner!=null&&descriptor.Group!=null&&descriptor.DiscretionaryAcl!=null,"CURRENT_SD_REQUIRED");byte[] binary=new byte[descriptor.BinaryLength];descriptor.GetBinaryForm(binary,0);
  using(var process=OpenProcess(0x1000,false,pid)){Check(!process.IsInvalid,"OpenProcess_QUERY_LIMITED_INFORMATION");Ft c,e,k,u;Check(GetProcessTimes(process,out c,out e,out k,out u),"GetProcessTimes_START");Need(e.Value==0&&c.Value/10==expected.UtcDateTime.ToFileTimeUtc()/10,"PID_REUSED_OR_EXITED_HOLD");
   SafeAccessTokenHandle opened;Check(OpenProcessToken(process,8,out opened),"OpenProcessToken_QUERY");using(opened){
    uint size;IntPtr p=Info(opened,1,out size);string sid;try{sid=new SecurityIdentifier(Marshal.ReadIntPtr(p)).Value;}finally{Marshal.FreeHGlobal(p);}Need(sid==Worker,"WRONG_TOKEN_USER_HOLD");
    var start=Value<Stat>(opened,10);Need(start.Type==1,"PRIMARY_PROCESS_TOKEN_REQUIRED");var groups=Groups(opened,2);var restricted=Groups(opened,11);int session=Value<int>(opened,12);int elevated=Value<int>(opened,20);
    SafeAccessTokenHandle duplicable;Check(OpenProcessToken(process,10,out duplicable),"OpenProcessToken_QUERY_DUPLICATE");using(duplicable){var duplicateSource=Value<Stat>(duplicable,10);Need(duplicateSource.Id.Id==start.Id.Id&&duplicateSource.Modified.Id==start.Modified.Id,"PRIMARY_TOKEN_REPLACED_HOLD");SafeAccessTokenHandle dup;Check(DuplicateToken(duplicable,2,out dup),"DuplicateToken_ACCESSCHECK_ONLY");using(dup){
     var copied=Value<Stat>(dup,10);Need(copied.Auth.Id==start.Auth.Id,"TOKEN_AUTH_CHANGED_HOLD");
     var checks=new List<object>();foreach(uint desired in new uint[]{0x120089,0x20,2,4,0x10,0x100,0x10000,0x40000,0x80000,0x2000000})checks.Add(Test(binary,dup,desired));
     uint logonType=0;string logonStatus="UNKNOWN";uint rawNt=0;IntPtr logon=IntPtr.Zero;var auth=start.Auth;try{rawNt=LsaGetLogonSessionData(ref auth,out logon);if(rawNt==0&&logon!=IntPtr.Zero){var data=Marshal.PtrToStructure<Logon>(logon);Need(data.Size>=Marshal.SizeOf<Logon>()&&data.Sid!=IntPtr.Zero&&new SecurityIdentifier(data.Sid).Value==Worker,"LOGON_DATA_SID_HOLD");logonType=data.Type;Need(data.Session==session,"LOGON_SESSION_MISMATCH");logonStatus="READ_OK";}}finally{if(logon!=IntPtr.Zero)LsaFreeReturnBuffer(logon);}
     bool inJob;Check(IsProcessInJob(process,IntPtr.Zero,out inJob),"IsProcessInJob_READ_ONLY");var end=Value<Stat>(opened,10);SafeAccessTokenHandle current;Check(OpenProcessToken(process,8,out current),"OpenProcessToken_RECHECK");using(current){Need(Value<Stat>(current,10).Id.Id==start.Id.Id,"PRIMARY_TOKEN_REPLACED_AT_END_HOLD");}Ft endC,endE;Check(GetProcessTimes(process,out endC,out endE,out k,out u),"GetProcessTimes_END");Need(endC.Value==c.Value&&endE.Value==0&&end.Auth.Id==start.Auth.Id&&end.Modified.Id==start.Modified.Id,"PROCESS_OR_TOKEN_CHANGED_HOLD");
     return new Dictionary<string,object>{{"token_accesscheck_known",true},{"token_user_sid",sid},{"token_elevated",elevated!=0},{"pid",pid},{"created_utc",DateTime.FromFileTimeUtc(c.Value).ToString("o")},{"generation_precision","CIM_MICROSECOND_PIN_PLUS_SAME_PROCESS_HANDLE"},{"authentication_luid",start.Auth.Id},{"session_id",session},{"security_logon_type",logonType},{"logon_query_status",logonStatus},{"logon_ntstatus",rawNt},{"logon_win32",LsaNtStatusToWinError(rawNt)},{"token_groups",groups},{"restricted_sids",restricted},{"checks",checks},{"process_token_stable",true},{"in_any_job",inJob},{"job_identity_exclusivity","UNKNOWN"},{"thread_impersonation_tokens","NOT_QUERIED"},{"new_logons",0},{"impersonation_performed",false}};
    }}
   }
  }
 }
}

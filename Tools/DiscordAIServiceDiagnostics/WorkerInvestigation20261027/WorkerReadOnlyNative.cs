// PID/생성시각에 고정한 읽기 전용 조사입니다. 명령줄/메모리/환경/privilege 변경 API는 없습니다.
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Security.Cryptography;
using Microsoft.Win32.SafeHandles;

public sealed class WorkerQueryFailure:Win32Exception {
 public string Operation{get;private set;}public WorkerQueryFailure(string op,int code):base(code){Operation=op;}
}
public sealed class WorkerReadHandle:SafeHandleZeroOrMinusOneIsInvalid {
 public int CloseError;public WorkerReadHandle():base(true){}
 protected override bool ReleaseHandle(){bool ok=WorkerReadOnlyNative.CloseHandle(handle);if(!ok)CloseError=Marshal.GetLastWin32Error();return ok;}
}
public static class WorkerReadOnlyNative {
 [StructLayout(LayoutKind.Sequential)]struct Ft{public uint Low,High;public long Value{get{return ((long)High<<32)|Low;}}}
 [StructLayout(LayoutKind.Sequential)]struct SidAttr{public IntPtr Sid;public uint Attributes;}
 [StructLayout(LayoutKind.Sequential)]struct GroupHead{public uint Count;public SidAttr First;}
 [StructLayout(LayoutKind.Sequential)]struct FileInfo{public uint Attributes;public Ft C,A,W;public uint Volume,SizeHigh,SizeLow,Links,IndexHigh,IndexLow;}
 [StructLayout(LayoutKind.Sequential)]struct TrustFile{public uint Size;public IntPtr Name,Handle,Subject;}
 [StructLayout(LayoutKind.Sequential)]struct TrustData{public uint Size;public IntPtr Callback,Client;public uint Ui,Revocation,Choice;public IntPtr File;public uint StateAction;public IntPtr State,Url;public uint Flags,Context;public IntPtr SignatureSettings;}
 [DllImport("kernel32.dll",SetLastError=true)]static extern WorkerReadHandle OpenProcess(uint access,bool inherit,int pid);
 [DllImport("kernel32.dll",SetLastError=true)]public static extern bool CloseHandle(IntPtr h);
 [DllImport("kernel32.dll",SetLastError=true)]static extern bool GetProcessTimes(WorkerReadHandle h,out Ft creation,out Ft exit,out Ft kernel,out Ft user);
 [DllImport("kernel32.dll",CharSet=CharSet.Unicode,SetLastError=true)]static extern bool QueryFullProcessImageName(WorkerReadHandle h,uint flags,StringBuilder path,ref uint length);
 [DllImport("kernel32.dll",SetLastError=true)]static extern bool IsProcessInJob(WorkerReadHandle process,IntPtr job,out bool present);
 [DllImport("advapi32.dll",SetLastError=true)]static extern bool OpenProcessToken(WorkerReadHandle process,uint access,out WorkerReadHandle token);
 [DllImport("advapi32.dll",SetLastError=true)]static extern bool GetTokenInformation(WorkerReadHandle token,int kind,IntPtr info,uint size,out uint needed);
 [DllImport("advapi32.dll",ExactSpelling=true,CharSet=CharSet.Unicode,SetLastError=true)]static extern bool LookupAccountNameW(string system,string name,byte[] sid,ref uint size,StringBuilder domain,ref uint domainSize,out int use);
 [DllImport("advapi32.dll",ExactSpelling=true,CharSet=CharSet.Unicode,SetLastError=true)]static extern bool LookupAccountSidW(string system,byte[] sid,StringBuilder name,ref uint size,StringBuilder domain,ref uint domainSize,out int use);
 [DllImport("kernel32.dll",ExactSpelling=true,CharSet=CharSet.Unicode,SetLastError=true)]static extern WorkerReadHandle CreateFileW(string path,uint access,uint share,IntPtr sa,uint creation,uint flags,IntPtr template);
 [DllImport("kernel32.dll",SetLastError=true)]static extern bool GetFileInformationByHandle(WorkerReadHandle h,out FileInfo info);
 [DllImport("kernel32.dll",ExactSpelling=true,CharSet=CharSet.Unicode,SetLastError=true)]static extern uint GetFinalPathNameByHandleW(WorkerReadHandle h,StringBuilder name,uint size,uint flags);
 [DllImport("wintrust.dll",ExactSpelling=true)]static extern int WinVerifyTrust(IntPtr window,ref Guid action,ref TrustData data);
 static void Check(bool ok,string op){if(!ok)throw new WorkerQueryFailure(op,Marshal.GetLastWin32Error());}
 static void Need(bool ok,string reason){if(!ok)throw new InvalidOperationException(reason);}
 static Dictionary<string,object> Error(Exception e){var r=new Dictionary<string,object>{{"type",e.GetType().FullName},{"hresult",e.HResult}};if(e is Win32Exception w)r["win32"]=w.NativeErrorCode;if(e is WorkerQueryFailure q)r["operation"]=q.Operation;return r;}
 public static bool CreationMatches(string expected,string actual){return DateTimeOffset.Parse(expected).UtcTicks/10==DateTimeOffset.Parse(actual).UtcTicks/10;}
 static string Created(WorkerReadHandle h,out bool exited){Ft c,e,k,u;Check(GetProcessTimes(h,out c,out e,out k,out u),"GetProcessTimes");exited=e.Value!=0;return DateTime.FromFileTimeUtc(c.Value).ToString("o");}
 static IntPtr TokenInfo(WorkerReadHandle token,int kind,out uint needed){
  bool ok=GetTokenInformation(token,kind,IntPtr.Zero,0,out needed);int code=Marshal.GetLastWin32Error();if(!ok&&code!=122)throw new WorkerQueryFailure("GetTokenInformation_size_"+kind,code);Need(!ok&&needed>0&&needed<=1048576,"TOKEN_BUFFER_CONTRACT");
  IntPtr buffer=Marshal.AllocHGlobal((int)needed);try{Check(GetTokenInformation(token,kind,buffer,needed,out needed),"GetTokenInformation_"+kind);return buffer;}catch{Marshal.FreeHGlobal(buffer);throw;}
 }
 static List<Dictionary<string,object>> Groups(WorkerReadHandle token,int kind){uint size;IntPtr b=TokenInfo(token,kind,out size);try{
  uint count=unchecked((uint)Marshal.ReadInt32(b));int start=(int)Marshal.OffsetOf<GroupHead>("First"),stride=Marshal.SizeOf<SidAttr>();Need(count<=4096&&((long)start+count*stride<=size||count==0),"TOKEN_GROUPS_BOUND");var rows=new List<Dictionary<string,object>>();
  for(int n=0;n<count;n++){var a=Marshal.PtrToStructure<SidAttr>(IntPtr.Add(b,start+n*stride));rows.Add(new Dictionary<string,object>{{"sid",new SecurityIdentifier(a.Sid).Value},{"attributes",a.Attributes},{"enabled",(a.Attributes&4)!=0&&(a.Attributes&16)==0},{"deny_only",(a.Attributes&16)!=0},{"logon_sid",(a.Attributes&0xc0000000)==0xc0000000}});}return rows;
 }finally{Marshal.FreeHGlobal(b);}}
 public static Dictionary<string,object> Resolve(string identity){
  Need(!string.IsNullOrWhiteSpace(identity)&&identity.Length<=512,"IDENTITY_EMPTY_OR_BOUND");string sid;int use;
  if(identity.StartsWith("S-1-",StringComparison.Ordinal)){sid=new SecurityIdentifier(identity).Value;}else{
   uint size=0,domainSize=0;bool ok=LookupAccountNameW(null,identity,null,ref size,null,ref domainSize,out use);int code=Marshal.GetLastWin32Error();if(ok||code!=122)throw new WorkerQueryFailure("LookupAccountNameW_size",code);Need(size>0&&size<=1024&&domainSize<=4096,"LOOKUP_NAME_BOUND");
   var bytes=new byte[size];var domain=new StringBuilder((int)Math.Max(domainSize,1));domainSize=(uint)domain.Capacity;Check(LookupAccountNameW(null,identity,bytes,ref size,domain,ref domainSize,out use),"LookupAccountNameW");sid=new SecurityIdentifier(bytes,0).Value;
  }
  var id=new SecurityIdentifier(sid);var binary=new byte[id.BinaryLength];id.GetBinaryForm(binary,0);uint nameSize=0,dSize=0;bool found=LookupAccountSidW(null,binary,null,ref nameSize,null,ref dSize,out use);int error=Marshal.GetLastWin32Error();if(found||error!=122)throw new WorkerQueryFailure("LookupAccountSidW_size",error);Need(nameSize>0&&nameSize<=4096&&dSize<=4096,"LOOKUP_SID_BOUND");
  var name=new StringBuilder((int)nameSize);var d=new StringBuilder((int)Math.Max(dSize,1));dSize=(uint)d.Capacity;Check(LookupAccountSidW(null,binary,name,ref nameSize,d,ref dSize,out use),"LookupAccountSidW");
  bool local=d.ToString().Equals(Environment.MachineName,StringComparison.OrdinalIgnoreCase);
  string category=use==5?"WELL_KNOWN_GROUP":(use==4&&d.ToString().Equals("BUILTIN",StringComparison.OrdinalIgnoreCase)?"BUILTIN_ALIAS":(local&&use==1?"LOCAL_USER":(local&&(use==2||use==4)?"LOCAL_GROUP":(use==2?"DOMAIN_GROUP_OR_DIRECTORY_GROUP":"OTHER_OR_UNKNOWN"))));
  return new Dictionary<string,object>{{"sid",sid},{"name",name.ToString()},{"domain",d.ToString()},{"sid_name_use",use},{"category",category},{"resolved",true}};
 }
 public static Dictionary<string,object> Read(int pid,string expected){
  var r=new Dictionary<string,object>{{"pid",pid},{"expected_created_utc",expected},{"creation_comparison_precision","SOURCE_WMI_MICROSECOND"}};
  var h=OpenProcess(0x1000,false,pid);if(h.IsInvalid){int code=Marshal.GetLastWin32Error();h.Dispose();throw new WorkerQueryFailure("OpenProcess_QUERY_LIMITED_INFORMATION",code);}
  try{
   bool exited;string creation=Created(h,out exited);r["actual_created_utc"]=creation;r["identity"]=CreationMatches(expected,creation)?"MATCH":"PID_REUSED";r["exited_during_query"]=exited;
   if((string)r["identity"]!="MATCH"||exited)return r;
   WorkerReadHandle token=null;try{
    Check(OpenProcessToken(h,8,out token),"OpenProcessToken_QUERY");uint size;IntPtr b=TokenInfo(token,1,out size);string owner;try{owner=new SecurityIdentifier(Marshal.ReadIntPtr(b)).Value;}finally{Marshal.FreeHGlobal(b);}r["owner_sid"]=owner;
    try{r["owner_account"]=Resolve(owner);}catch(Exception e){r["owner_lookup_error"]=Error(e);}
    b=TokenInfo(token,10,out size);try{Need(size>=16,"TOKEN_STATISTICS_BOUND");uint lo=unchecked((uint)Marshal.ReadInt32(b,8)),hi=unchecked((uint)Marshal.ReadInt32(b,12));ulong luid=((ulong)hi<<32)|lo;r["authentication_luid"]=luid.ToString("x16");r["authentication_id_decimal"]=luid.ToString();}finally{Marshal.FreeHGlobal(b);}
    b=TokenInfo(token,12,out size);try{Need(size>=4,"TOKEN_SESSION_BOUND");r["session_id"]=Marshal.ReadInt32(b);}finally{Marshal.FreeHGlobal(b);}
    r["token_groups"]=Groups(token,2);r["restricted_sids"]=Groups(token,11);r["token_query_known"]=true;
   }catch(Exception e){r["token_query_known"]=false;r["token_error"]=Error(e);}finally{if(token!=null){token.Dispose();if(token.CloseError!=0)r["token_close_win32"]=token.CloseError;}}
   try{uint length=32768;var path=new StringBuilder((int)length);Check(QueryFullProcessImageName(h,0,path,ref length),"QueryFullProcessImageNameW");r["image_query_path"]=path.ToString();}catch(Exception e){r["image_error"]=Error(e);}
   try{bool inJob;Check(IsProcessInJob(h,IntPtr.Zero,out inJob),"IsProcessInJob");r["in_any_job"]=inJob;r["job_identity"]="UNKNOWN_NO_FOREIGN_JOB_HANDLE_OPENED";}catch(Exception e){r["job_error"]=Error(e);}
   bool finalExited;Need(Created(h,out finalExited)==creation,"PROCESS_CREATION_CHANGED");r["exited_during_query"]=finalExited;return r;
  }finally{h.Dispose();if(h.CloseError!=0)r["process_close_win32"]=h.CloseError;}
 }
 public static Dictionary<string,object> Parent(int pid){var h=OpenProcess(0x1000,false,pid);if(h.IsInvalid){int error=Marshal.GetLastWin32Error();h.Dispose();throw new WorkerQueryFailure("Parent_OpenProcess_QUERY_LIMITED_INFORMATION",error);}try{bool exited;return new Dictionary<string,object>{{"pid",pid},{"created_utc",Created(h,out exited)},{"exited",exited}};}finally{h.Dispose();}}
 public static Dictionary<string,object> Image(string path){
  // 동일 file handle의 SHA와 signature를 연결합니다. 원시 파일 bytes는 저장하지 않습니다.
  var h=CreateFileW(path,0x80000000,1,IntPtr.Zero,3,0x00200000,IntPtr.Zero);if(h.IsInvalid){int error=Marshal.GetLastWin32Error();h.Dispose();throw new WorkerQueryFailure("Image_CreateFile_READ_SHARE_READ",error);}
  try{
   FileInfo info;Check(GetFileInformationByHandle(h,out info),"Image_GetFileInformationByHandle");Need((info.Attributes&0x410)==0&&info.Links==1,"IMAGE_REPARSE_DIRECTORY_HARDLINK_UNKNOWN");Need((((ulong)info.SizeHigh<<32)|info.SizeLow)<=256*1024*1024,"IMAGE_SIZE_BOUND");
   var name=new StringBuilder(32768);uint len=GetFinalPathNameByHandleW(h,name,(uint)name.Capacity,0);Check(len>0,"Image_GetFinalPathNameByHandle");Need(len<name.Capacity,"IMAGE_FINAL_PATH_BOUND");string final=name.ToString();Need(final.StartsWith(@"\\?\"),"IMAGE_FINAL_PATH_UNKNOWN");final=final.Substring(4);
   string sha;bool pe;using(var stream=new FileStream(new SafeFileHandle(h.DangerousGetHandle(),false),FileAccess.Read)){
    byte[] head=new byte[64];int read=stream.Read(head,0,head.Length);pe=read>=64&&head[0]==0x4d&&head[1]==0x5a;stream.Position=0;sha=Convert.ToHexString(SHA256.HashData(stream)).ToLowerInvariant();stream.Position=0;
   }
   IntPtr fileName=Marshal.StringToHGlobalUni(final),filePtr=IntPtr.Zero;var data=new TrustData{Size=(uint)Marshal.SizeOf<TrustData>(),Ui=2,Choice=1,StateAction=1,Flags=0x1010};var action=new Guid("00AAC56B-CD44-11d0-8CC2-00C04FC295EE");int trust;
   try{var file=new TrustFile{Size=(uint)Marshal.SizeOf<TrustFile>(),Name=fileName,Handle=h.DangerousGetHandle()};filePtr=Marshal.AllocHGlobal(Marshal.SizeOf<TrustFile>());Marshal.StructureToPtr(file,filePtr,false);data.File=filePtr;trust=WinVerifyTrust(new IntPtr(-1),ref action,ref data);}finally{if(data.State!=IntPtr.Zero){data.StateAction=2;WinVerifyTrust(new IntPtr(-1),ref action,ref data);}if(filePtr!=IntPtr.Zero)Marshal.FreeHGlobal(filePtr);Marshal.FreeHGlobal(fileName);}
   string location=final.StartsWith(@"C:\Windows\",StringComparison.OrdinalIgnoreCase)?"WINDOWS":(final.StartsWith(@"C:\Program Files\",StringComparison.OrdinalIgnoreCase)||final.StartsWith(@"C:\Program Files (x86)\",StringComparison.OrdinalIgnoreCase)?"PROGRAM_FILES":"OTHER_PATH_REDACTED");
   return new Dictionary<string,object>{{"name",Path.GetFileName(final)},{"location",location},{"sha256",sha},{"file_id",info.Volume.ToString("x8")+":"+info.IndexHigh.ToString("x8")+info.IndexLow.ToString("x8")},{"image_kind",pe?"MZ_IMAGE_CANDIDATE":"UNKNOWN"},{"signature_policy","WINVERIFYTRUST_FILE_HANDLE_CACHE_ONLY_NO_ONLINE_REVOCATION"},{"signature_code",trust},{"signature_status",trust==0?"VALID_AT_CACHE_POLICY":"NOT_VERIFIED_BY_THIS_METHOD"},{"catalog_enumeration_performed",false}};
  }finally{h.Dispose();}
 }
 [DllImport("kernel32.dll",ExactSpelling=true,CharSet=CharSet.Unicode,SetLastError=true)]static extern uint GetFileAttributesW(string path);
 public static Dictionary<string,object> PathMetadata(string path){uint attr=GetFileAttributesW(path);if(attr==uint.MaxValue){int code=Marshal.GetLastWin32Error();if(code==2||code==3)return new Dictionary<string,object>{{"status","ABSENT"}};throw new WorkerQueryFailure("GetFileAttributesW_metadata_only",code);}return new Dictionary<string,object>{{"status",(attr&0x400)!=0?"REPARSE_UNKNOWN":"PRESENT_NOT_OPENED"},{"attributes",attr}};}
}

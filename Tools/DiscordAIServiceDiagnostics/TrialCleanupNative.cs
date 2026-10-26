// 승인된 옛 Code root 검사/보호 백업/파일별 정리에만 쓰는 도구입니다. 설치기나 Worker를 호출하지 않습니다.
using System;
using System.IO;
using System.Text;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Security.Cryptography;
using Microsoft.Win32.SafeHandles;
public static class LimitlessTrialCleanupV1 {
 public static string Root {get{return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),"LimitlessRelayTrial");}}
 public static string Backup {get{return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),"LimitlessRelayTrialCleanup20261027");}}
 public const string PrivateSddl="O:BAG:SYD:P(A;OICI;FA;;;SY)(A;OICI;FA;;;BA)";
 [StructLayout(LayoutKind.Sequential)]struct SA{public int Length;public IntPtr Descriptor;public int Inherit;}
 [StructLayout(LayoutKind.Sequential)]struct Info{public uint Attributes;public System.Runtime.InteropServices.ComTypes.FILETIME Created,Accessed,Written;public uint Volume,SizeHigh,SizeLow,Links,IndexHigh,IndexLow;}
 [StructLayout(LayoutKind.Sequential)]struct Disposition{[MarshalAs(UnmanagedType.Bool)]public bool Delete;}
 [StructLayout(LayoutKind.Sequential)]struct Text{public ushort Length,Max;public IntPtr Buffer;}
 [StructLayout(LayoutKind.Sequential)]struct Attr{public int Length;public IntPtr Root,Object;public uint Flags;public IntPtr Descriptor,Quality;}
 [DllImport("advapi32.dll",CharSet=CharSet.Unicode,SetLastError=true)]static extern bool ConvertStringSecurityDescriptorToSecurityDescriptor(string text,uint version,out IntPtr descriptor,out uint size);
 [DllImport("kernel32.dll")]static extern IntPtr LocalFree(IntPtr value);
 [DllImport("kernel32.dll",CharSet=CharSet.Unicode,SetLastError=true)]static extern bool CreateDirectory(string path,ref SA security);
 [DllImport("kernel32.dll",CharSet=CharSet.Unicode,SetLastError=true)]static extern SafeFileHandle CreateFile(string path,uint access,uint share,ref SA security,uint creation,uint flags,IntPtr template);
 [DllImport("kernel32.dll",SetLastError=true)]static extern bool GetFileInformationByHandle(SafeFileHandle handle,out Info info);
 [DllImport("kernel32.dll",CharSet=CharSet.Unicode,SetLastError=true)]static extern uint GetFinalPathNameByHandle(SafeFileHandle handle,StringBuilder text,uint count,uint flags);
 [DllImport("kernel32.dll",CharSet=CharSet.Unicode,SetLastError=true)]static extern uint GetShortPathName(string path,StringBuilder text,uint count);
 [DllImport("kernel32.dll",SetLastError=true)]static extern bool SetFileInformationByHandle(SafeFileHandle handle,int kind,ref Disposition data,uint size);
 [DllImport("advapi32.dll")]static extern uint LsaOpenPolicy(IntPtr name,ref Attr attr,uint access,out IntPtr handle);
 [DllImport("advapi32.dll")]static extern uint LsaEnumerateAccountRights(IntPtr handle,byte[] sid,out IntPtr rights,out uint count);
 [DllImport("advapi32.dll")]static extern uint LsaFreeMemory(IntPtr value);
 [DllImport("advapi32.dll")]static extern uint LsaClose(IntPtr handle);
 [DllImport("advapi32.dll")]static extern uint LsaNtStatusToWinError(uint status);
 static void Admin(){using(var id=WindowsIdentity.GetCurrent()){if(id.User.Value!="S-1-5-21-2858504392-1958168942-1662906075-1001" || !new WindowsPrincipal(id).IsInRole(WindowsBuiltInRole.Administrator))throw new UnauthorizedAccessException("FIXED_ADMIN_REQUIRED");}}
 public static string Scoped(string root,string path){
  root=Path.GetFullPath(root);path=Path.GetFullPath(path);
  if(!path.Equals(root,StringComparison.OrdinalIgnoreCase) && !path.StartsWith(root+Path.DirectorySeparatorChar,StringComparison.OrdinalIgnoreCase))throw new UnauthorizedAccessException("PATH_SCOPE_HOLD");
  return path;
 }
 public static void NoReparseAncestors(string path){
  for(string p=Path.GetFullPath(path);p!=null;p=Path.GetDirectoryName(p)){if((File.GetAttributes(p)&FileAttributes.ReparsePoint)!=0)throw new UnauthorizedAccessException("REPARSE_HOLD");}
 }
 static SafeFileHandle OpenPinned(string path,bool directory,bool delete){
  Admin();path=Scoped(Root,path);NoReparseAncestors(path);
  var sa=new SA{Length=Marshal.SizeOf<SA>()};uint access=directory?0x80u:0x80000000u;if(delete)access|=0x10000u;
  var h=CreateFile(path,access,0,ref sa,3,0x200000u|(directory?0x2000000u:0u),IntPtr.Zero);
  if(h.IsInvalid){int e=Marshal.GetLastWin32Error();h.Dispose();throw new Win32Exception(e);}
  try{
   Info info;if(!GetFileInformationByHandle(h,out info))throw new Win32Exception(Marshal.GetLastWin32Error());
   if((info.Attributes&0x400)!=0 || (!directory && info.Links!=1))throw new UnauthorizedAccessException("REPARSE_OR_HARDLINK_HOLD");
   var name=new StringBuilder(32768);uint count=GetFinalPathNameByHandle(h,name,(uint)name.Capacity,0);
   if(count==0 || count>=name.Capacity)throw new Win32Exception(Marshal.GetLastWin32Error());
   string final=name.ToString();if(final.StartsWith(@"\\?\"))final=final.Substring(4);
   if(!final.Equals(path,StringComparison.OrdinalIgnoreCase))throw new UnauthorizedAccessException("HANDLE_PATH_HOLD");
   return h;
  }catch{h.Dispose();throw;}
 }
 public static void ProbeDirectory(string path){using(var h=OpenPinned(path,true,false)) {}}
 static byte[] Read(SafeFileHandle h,string expected){
  using(var stream=new FileStream(h,FileAccess.Read)){using(var memory=new MemoryStream()){
   stream.CopyTo(memory);byte[] data=memory.ToArray();string hash=Convert.ToHexString(SHA256.HashData(data)).ToLowerInvariant();
   if(hash!=expected)throw new UnauthorizedAccessException("FILE_SHA_HOLD");return data;
  }}
 }
 public static byte[] ReadPinned(string path,string expected){using(var h=OpenPinned(path,false,false)){return Read(h,expected);}}
 public static void DeletePinned(string path,string expected){
  using(var h=OpenPinned(path,false,true)){
   // FileStream이 handle을 닫기 전에 삭제 표시까지 처리합니다. 검증한 바로 그 파일 handle만 대상으로 합니다.
   using(var stream=new FileStream(h,FileAccess.Read)){
    string hash=Convert.ToHexString(SHA256.HashData(stream)).ToLowerInvariant();if(hash!=expected)throw new UnauthorizedAccessException("DELETE_SHA_HOLD");
    var data=new Disposition{Delete=true};if(!SetFileInformationByHandle(h,4,ref data,(uint)Marshal.SizeOf<Disposition>()))throw new Win32Exception(Marshal.GetLastWin32Error());
   }
  }
 }
 public static void DeleteEmptyDirectory(string path){Admin();path=Scoped(Root,path);ProbeDirectory(path);Directory.Delete(path,false);}
 static SA Descriptor(bool directory,out IntPtr memory){
  uint size;string sddl=directory?PrivateSddl:"O:BAG:SYD:P(A;;FA;;;SY)(A;;FA;;;BA)";
  if(!ConvertStringSecurityDescriptorToSecurityDescriptor(sddl,1,out memory,out size))throw new Win32Exception(Marshal.GetLastWin32Error());
  return new SA{Length=Marshal.SizeOf<SA>(),Descriptor=memory};
 }
 public static void NewBackupDirectory(string path){
  Admin();path=Scoped(Backup,path);NoReparseAncestors(Path.GetDirectoryName(path));IntPtr memory;var sa=Descriptor(true,out memory);
  try{if(!CreateDirectory(path,ref sa))throw new Win32Exception(Marshal.GetLastWin32Error());}finally{LocalFree(memory);}
 }
 public static void NewBackupFile(string path,byte[] data){
  Admin();path=Scoped(Backup,path);if(path==Backup)throw new UnauthorizedAccessException("BACKUP_FILE_SCOPE_HOLD");NoReparseAncestors(Path.GetDirectoryName(path));
  IntPtr memory;var sa=Descriptor(false,out memory);
  try{using(var h=CreateFile(path,0x40000000,1,ref sa,1,0x80,IntPtr.Zero)){
   if(h.IsInvalid)throw new Win32Exception(Marshal.GetLastWin32Error());using(var stream=new FileStream(h,FileAccess.Write)){stream.Write(data,0,data.Length);stream.Flush(true);}
  }}finally{LocalFree(memory);}
 }
 public static string ShortRoot(){Admin();var s=new StringBuilder(32768);uint n=GetShortPathName(Root,s,(uint)s.Capacity);if(n==0 || n>=s.Capacity)throw new Win32Exception(Marshal.GetLastWin32Error());return s.ToString();}
 public static string[] DirectRights(string sid){
  Admin();if(sid!="S-1-5-21-2858504392-1958168942-1662906075-1008")throw new UnauthorizedAccessException("FIXED_BOT_SID_HOLD");
  var id=new SecurityIdentifier(sid);var bytes=new byte[id.BinaryLength];id.GetBinaryForm(bytes,0);IntPtr handle=IntPtr.Zero,rights=IntPtr.Zero;
  try{
   var attr=new Attr{Length=Marshal.SizeOf<Attr>()};uint status=LsaOpenPolicy(IntPtr.Zero,ref attr,0x800,out handle);if(status!=0)throw new Win32Exception((int)LsaNtStatusToWinError(status));
   uint count;status=LsaEnumerateAccountRights(handle,bytes,out rights,out count);if(status!=0)throw new Win32Exception((int)LsaNtStatusToWinError(status));
   var found=new System.Collections.Generic.List<string>();for(int i=0;i<count;i++){var t=Marshal.PtrToStructure<Text>(IntPtr.Add(rights,checked(i*Marshal.SizeOf<Text>())));found.Add(Marshal.PtrToStringUni(t.Buffer,t.Length/2));}return found.ToArray();
  }finally{if(rights!=IntPtr.Zero)LsaFreeMemory(rights);if(handle!=IntPtr.Zero)LsaClose(handle);}
 }
}

// 보호 staging의 동일 순서를 native/fake 파일 시스템이 공유합니다. 삭제/ACL 수정/설치 API는 없습니다.
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Runtime.InteropServices;
using System.ComponentModel;
using Microsoft.Win32.SafeHandles;

public sealed class V2StageInfo {
 public string Path,Id,Volume,Sddl,FileSystem;public bool Directory,Reparse,DeletePending;public uint Links;public string[] Streams;
}
public abstract class V2StageHandle:IDisposable {
 public abstract V2StageInfo Info();public abstract byte[] Read();public abstract void Dispose();
}
public interface IV2StageFs {
 bool Exists(string path);V2StageHandle Open(string path);void Directory(string path,string sddl);void File(string path,byte[] bytes,string sddl);string[] Children(string path);
}
public sealed class V2StageSession:IDisposable {
 internal List<V2StageHandle> Handles=new List<V2StageHandle>();
 internal Dictionary<V2StageHandle,string> Ids=new Dictionary<V2StageHandle,string>();
 public Dictionary<string,byte[]> Bytes=new Dictionary<string,byte[]>(StringComparer.Ordinal);
 public string Bundle,PackageId;public int PayloadCount;
 static string Stamp(V2StageInfo i){return string.Join("|",i.Volume,i.Id,i.Path,i.Sddl,i.Directory,i.Links,i.FileSystem,string.Join(",",i.Streams??Array.Empty<string>()));}
 internal void Add(V2StageHandle h){Handles.Add(h);Ids.Add(h,Stamp(h.Info()));}
 public void Recheck(){foreach(var h in Handles){var i=h.Info();LimitlessV2Staging.Need(!i.Reparse&&!i.DeletePending&&Ids[h]==Stamp(i),"STAGE_ID_OR_METADATA_CHANGED");}}
 public void Dispose(){Exception failure=null;for(int n=Handles.Count-1;n>=0;n--){try{Handles[n].Dispose();}catch(Exception e){failure=e;}}Handles.Clear();if(failure!=null)throw new InvalidOperationException("STAGE_CLOSE_HOLD",failure);}
}
public static class LimitlessV2Staging {
 public const string Root=@"C:\Program Files\LimitlessV2Staging20261027";
 public const string DirSd="O:BAG:SYD:P(A;OICI;FA;;;SY)(A;OICI;FA;;;BA)",FileSd="O:BAG:SYD:P(A;;FA;;;SY)(A;;FA;;;BA)";
 const string TI="S-1-5-80-956008885-3418522649-1831038044-1853292631-2271478464";
 public static void Need(bool ok,string reason){if(!ok)throw new InvalidOperationException(reason);}
 public static string Hash(byte[] b){return Convert.ToHexString(SHA256.HashData(b)).ToLowerInvariant();}
 static string Canon(string p){string full=Path.GetFullPath(p);Need(p==full&&!p.StartsWith(@"\\"),"STAGE_CANONICAL_LOCAL_PATH");return full;}
 public static void Basic(V2StageInfo i,string path,bool directory){
  Need(i!=null&&i.Path.Equals(Canon(path),StringComparison.OrdinalIgnoreCase)&&i.Directory==directory&&!i.Reparse&&!i.DeletePending,"STAGE_PATH_KIND_REPARSE");
  Need(i.FileSystem=="NTFS"&&!string.IsNullOrEmpty(i.Id)&&!string.IsNullOrEmpty(i.Volume),"STAGE_FILE_ID_FS");
  if(!directory)Need(i.Links==1&&i.Streams!=null&&i.Streams.SequenceEqual(new[]{"::$DATA"}),"STAGE_HARDLINK_ADS");
  else Need(i.Streams!=null&&i.Streams.Length==0,"STAGE_DIRECTORY_ADS");
 }
 public static void Protected(V2StageInfo i,string path,bool directory){Basic(i,path,directory);LimitlessV2Contract.ExactSddl(i.Sddl,directory?DirSd:FileSd);}
 static void Ancestor(V2StageInfo i,string path,bool direct){
  Basic(i,path,true);var sd=new RawSecurityDescriptor(i.Sddl);Need(sd.Owner!=null&&new[]{"S-1-5-18","S-1-5-32-544",TI}.Contains(sd.Owner.Value)&&sd.DiscretionaryAcl!=null,"STAGE_ANCESTOR_OWNER_DACL");
  foreach(GenericAce generic in sd.DiscretionaryAcl){var a=generic as CommonAce;Need(a!=null&&!a.IsCallback,"STAGE_ANCESTOR_ACE");if(a.AceQualifier!=AceQualifier.AccessAllowed||(a.AceFlags&AceFlags.InheritOnly)!=0)continue;
   if(new[]{"S-1-5-18","S-1-5-32-544",TI}.Contains(a.SecurityIdentifier.Value))continue;
   uint mask=unchecked((uint)a.AccessMask),bad=0x10000000u|0x40000000u|0x10000u|0x40u|0x40000u|0x80000u;
   if(direct)bad|=0x2u|0x4u|0x10u|0x100u;
   Need((mask&bad)==0,"STAGE_ANCESTOR_WRITER");
  }
 }
 static void PinParents(IV2StageFs fs,string path,V2StageSession session,bool secure){
  string direct=Path.GetDirectoryName(Canon(path));var names=new Stack<string>();for(string p=direct;p!=null;p=Path.GetDirectoryName(p))names.Push(p);
  while(names.Count>0){string p=names.Pop();var h=fs.Open(p);session.Add(h);if(secure)Ancestor(h.Info(),p,p==direct);else Basic(h.Info(),p,true);}
 }
 // ZIP bytes만 대상으로 하는 순수 검사입니다. native 파일 API를 호출하지 않습니다.
 public static Dictionary<string,byte[]> Archive(byte[] zip,string manifestSha,string installerSha,string zipSha,int count,out string package){
  Need(LimitlessV2Contract.Sha(manifestSha)&&LimitlessV2Contract.Sha(installerSha)&&LimitlessV2Contract.Sha(zipSha)&&zip.Length<=128*1024*1024&&Hash(zip)==zipSha,"STAGE_ZIP_PIN");
  var all=new Dictionary<string,byte[]>(StringComparer.Ordinal);var seen=new HashSet<string>(StringComparer.OrdinalIgnoreCase);long total=0;
  using(var ms=new MemoryStream(zip,false))using(var z=new ZipArchive(ms,ZipArchiveMode.Read)){
   Need(z.Entries.Count==count+1,"STAGE_ZIP_COUNT");foreach(var e in z.Entries){
    string n=e.FullName;Need(n=="HostManifest.json"||n.StartsWith("Code/",StringComparison.Ordinal)||n.StartsWith("Runtime/",StringComparison.Ordinal)||n.StartsWith("Deployment/",StringComparison.Ordinal),"STAGE_ENTRY_ROOT");
    if(n!="HostManifest.json")LimitlessV2Contract.Relative(n);
    Need(!n.EndsWith("/")&&seen.Add(n),"STAGE_ENTRY_DUPLICATE");
    uint attr=unchecked((uint)e.ExternalAttributes);uint kind=(attr>>16)&0xf000;
    Need((kind==0||kind==0x8000)&&(attr&0x410)==0,"STAGE_ZIP_LINK_ATTRIBUTE");
    total=checked(total+e.Length);Need(e.Length<=64*1024*1024&&total<=256*1024*1024,"STAGE_ZIP_BOUNDS");
    using(var s=e.Open())using(var output=new MemoryStream()){var buffer=new byte[65536];int read;long actual=0;while((read=s.Read(buffer,0,buffer.Length))>0){actual+=read;Need(actual<=e.Length,"STAGE_ZIP_LENGTH");output.Write(buffer,0,read);}Need(actual==e.Length,"STAGE_ZIP_LENGTH");all.Add(n,output.ToArray());}
   }
  }
  Need(all.ContainsKey("HostManifest.json")&&Hash(all["HostManifest.json"])==manifestSha,"STAGE_MANIFEST_PIN");
  string raw=new UTF8Encoding(false,true).GetString(all["HostManifest.json"]);LimitlessV2Contract.Manifest(raw);
  using(var d=JsonDocument.Parse(raw)){
   var m=d.RootElement;package=m.GetProperty("package_id").GetString();var entries=m.GetProperty("files_sha256").EnumerateObject().ToArray();Need(entries.Length==count,"STAGE_PAYLOAD_COUNT");
   foreach(var p in entries)Need(all.ContainsKey(p.Name)&&Hash(all[p.Name])==p.Value.GetString(),"STAGE_PAYLOAD_SHA");
  }
  Need(all.ContainsKey("Deployment/install-session0-v2.ps1")&&Hash(all["Deployment/install-session0-v2.ps1"])==installerSha,"STAGE_INSTALLER_PIN");return all;
 }
 static V2StageHandle OpenProtected(IV2StageFs fs,string p,bool directory,V2StageSession s){var h=fs.Open(p);s.Add(h);Protected(h.Info(),p,directory);return h;}
 static string[] Directories(string root,IEnumerable<string> files){var dirs=new HashSet<string>(StringComparer.OrdinalIgnoreCase){root};foreach(string f in files){for(string p=Path.GetDirectoryName(Path.Combine(root,f.Replace('/', '\\')));p!=Path.GetDirectoryName(root);p=Path.GetDirectoryName(p))dirs.Add(p);}return dirs.OrderBy(x=>x.Length).ToArray();}
 static void CheckTree(IV2StageFs fs,string folder,HashSet<string> expected){foreach(var child in fs.Children(folder)){Need(Path.GetDirectoryName(child).Equals(folder,StringComparison.OrdinalIgnoreCase)&&expected.Remove(child),"STAGE_UNEXPECTED_OR_ALIAS");if(fs.Open(child) is V2StageHandle h){using(h){if(h.Info().Directory)CheckTree(fs,child,expected);}}}}
 public static V2StageSession Create(IV2StageFs fs,string sourceZip,string manifestSha,string installerSha,string zipSha,int count){
  var s=new V2StageSession();try{
   PinParents(fs,sourceZip,s,false);var source=fs.Open(sourceZip);s.Add(source);Basic(source.Info(),sourceZip,false);byte[] zip=source.Read();string id;var all=Archive(zip,manifestSha,installerSha,zipSha,count,out id);
   PinParents(fs,Root,s,true);
   if(fs.Exists(Root))OpenProtected(fs,Root,true,s);else{fs.Directory(Root,DirSd);OpenProtected(fs,Root,true,s);}
   string package=Path.Combine(Root,id);Need(!fs.Exists(package),"STAGE_NEW_PACKAGE_ONLY");fs.Directory(package,DirSd);OpenProtected(fs,package,true,s);
   string bundle=Path.Combine(package,"Bundle");foreach(string p in Directories(bundle,all.Keys)){fs.Directory(p,DirSd);OpenProtected(fs,p,true,s);}
   var expected=new HashSet<string>(StringComparer.OrdinalIgnoreCase);
   foreach(string p in Directories(bundle,all.Keys))if(p!=bundle)expected.Add(p);
   foreach(var f in all){string p=Path.Combine(bundle,f.Key.Replace('/', '\\'));fs.File(p,f.Value,FileSd);var h=OpenProtected(fs,p,false,s);Need(Hash(h.Read())==Hash(f.Value),"STAGE_COPY_READBACK");expected.Add(p);}
   string zipPath=bundle+".zip";fs.File(zipPath,zip,FileSd);var zh=OpenProtected(fs,zipPath,false,s);Need(Hash(zh.Read())==zipSha,"STAGE_ZIP_COPY_READBACK");
   CheckTree(fs,bundle,expected);Need(expected.Count==0,"STAGE_MISSING_ENTRY");Need(fs.Children(package).OrderBy(x=>x).SequenceEqual(new[]{bundle,zipPath}.OrderBy(x=>x)),"STAGE_PACKAGE_EXTRA");
   s.Bundle=bundle;s.PackageId=id;s.PayloadCount=count;s.Bytes=all;s.Recheck();return s;
  }catch(Exception e){try{s.Dispose();}catch(Exception close){e.Data["stage_close_hresult"]=close.HResult;}throw;}
 }
 public static V2StageSession Verify(IV2StageFs fs,string bundle,string manifestSha,string installerSha,string zipSha,int count){
  var s=new V2StageSession();try{
   Need(System.Text.RegularExpressions.Regex.IsMatch(bundle??"",@"\AC:\\Program Files\\LimitlessV2Staging20261027\\[a-f0-9]{32}\\Bundle\z"),"STAGE_INSTALL_SCOPE");
   PinParents(fs,Root,s,true);OpenProtected(fs,Root,true,s);
   string package=Path.GetDirectoryName(bundle);OpenProtected(fs,package,true,s);
   var zip=OpenProtected(fs,bundle+".zip",false,s);string id;var all=Archive(zip.Read(),manifestSha,installerSha,zipSha,count,out id);
   Need(bundle==Path.Combine(Root,id,"Bundle"),"STAGE_BUNDLE_BINDING");
   var expected=new HashSet<string>(StringComparer.OrdinalIgnoreCase);
   foreach(string d in Directories(bundle,all.Keys)){OpenProtected(fs,d,true,s);if(d!=bundle)expected.Add(d);}
   foreach(var f in all){string p=Path.Combine(bundle,f.Key.Replace('/', '\\'));var h=OpenProtected(fs,p,false,s);Need(Hash(h.Read())==Hash(f.Value),"STAGE_INSTALL_READBACK");expected.Add(p);}
   CheckTree(fs,bundle,expected);Need(expected.Count==0,"STAGE_MISSING_ENTRY");Need(fs.Children(package).OrderBy(x=>x).SequenceEqual(new[]{bundle,bundle+".zip"}.OrderBy(x=>x)),"STAGE_PACKAGE_EXTRA");
   s.Bundle=bundle;s.PackageId=id;s.PayloadCount=count;s.Bytes=all;s.Recheck();return s;
  }catch(Exception e){try{s.Dispose();}catch(Exception close){e.Data["stage_close_hresult"]=close.HResult;}throw;}
 }
}

// native adapter: 경로를 따라 수정하지 않고 handle에서 실제 경로/ID/SD/stream을 읽습니다.
public sealed class V2NativeStageFs:IV2StageFs {
 [StructLayout(LayoutKind.Sequential)]struct SA{public int Length;public IntPtr Sd;public int Inherit;}
 [StructLayout(LayoutKind.Sequential)]struct BY{public uint Attributes;public System.Runtime.InteropServices.ComTypes.FILETIME C,A,W;public uint Volume,SizeHi,SizeLo,Links,IndexHi,IndexLo;}
 [DllImport("kernel32.dll",CharSet=CharSet.Unicode,SetLastError=true)]static extern SafeFileHandle CreateFile(string p,uint access,uint share,ref SA sa,uint mode,uint flags,IntPtr template);
 [DllImport("kernel32.dll",CharSet=CharSet.Unicode,SetLastError=true)]static extern bool CreateDirectory(string p,ref SA sa);
 [DllImport("kernel32.dll",CharSet=CharSet.Unicode,SetLastError=true)]static extern uint GetFileAttributes(string p);
 [DllImport("kernel32.dll",SetLastError=true)]static extern bool GetFileInformationByHandle(SafeFileHandle h,out BY info);
 [DllImport("kernel32.dll",SetLastError=true)]static extern bool GetFileInformationByHandleEx(SafeFileHandle h,int cls,IntPtr buffer,uint size);
 [DllImport("kernel32.dll",CharSet=CharSet.Unicode,SetLastError=true)]static extern uint GetFinalPathNameByHandle(SafeFileHandle h,StringBuilder text,uint size,uint flags);
 [DllImport("kernel32.dll",CharSet=CharSet.Unicode,SetLastError=true)]static extern bool GetVolumeInformationByHandleW(SafeFileHandle h,StringBuilder label,uint size,out uint serial,out uint max,out uint flags,StringBuilder fs,uint fsSize);
 [DllImport("advapi32.dll",CharSet=CharSet.Unicode,SetLastError=true)]static extern bool ConvertStringSecurityDescriptorToSecurityDescriptor(string s,uint rev,out IntPtr sd,out uint size);
 [DllImport("advapi32.dll")]static extern uint GetSecurityInfo(SafeFileHandle h,int type,uint sections,out IntPtr owner,out IntPtr group,out IntPtr dacl,out IntPtr sacl,out IntPtr sd);
 [DllImport("advapi32.dll")]static extern uint GetSecurityDescriptorLength(IntPtr sd);
 [DllImport("kernel32.dll")]static extern IntPtr LocalFree(IntPtr p);
 [DllImport("kernel32.dll",SetLastError=true)]static extern bool SetFilePointerEx(SafeFileHandle h,long offset,out long position,uint method);
 [DllImport("kernel32.dll",SetLastError=true)]static extern bool ReadFile(SafeFileHandle h,byte[] b,uint size,out uint read,IntPtr overlapped);
 [DllImport("kernel32.dll",SetLastError=true)]static extern bool WriteFile(SafeFileHandle h,byte[] b,uint size,out uint written,IntPtr overlapped);
 [DllImport("kernel32.dll",SetLastError=true)]static extern bool FlushFileBuffers(SafeFileHandle h);
 static void Ok(bool ok){if(!ok)throw new Win32Exception(Marshal.GetLastWin32Error());}
 static void Admin(){using(var i=WindowsIdentity.GetCurrent())LimitlessV2Staging.Need(new WindowsPrincipal(i).IsInRole(WindowsBuiltInRole.Administrator),"STAGE_ADMIN_REQUIRED");}
 public static void WriteScope(string p){LimitlessV2Staging.Need(p!=null&&Path.GetFullPath(p)==p&&!p.Substring(2).Contains(':')&&(p==LimitlessV2Staging.Root||p.StartsWith(LimitlessV2Staging.Root+"\\",StringComparison.Ordinal)),"STAGE_WRITE_SCOPE");}
 static void Scope(string p){Admin();WriteScope(p);}
 static SA Security(string s,out IntPtr sd){uint size;Ok(ConvertStringSecurityDescriptorToSecurityDescriptor(s,1,out sd,out size));return new SA{Length=Marshal.SizeOf<SA>(),Sd=sd};}
 public bool Exists(string p){uint a=GetFileAttributes(p);if(a!=uint.MaxValue)return true;int error=Marshal.GetLastWin32Error();if(error==2||error==3)return false;throw new Win32Exception(error);}
 public V2StageHandle Open(string p){Admin();var a=new SA{Length=Marshal.SizeOf<SA>()};var h=CreateFile(p,0x80020000,1,ref a,3,0x02200000,IntPtr.Zero);if(h.IsInvalid){int error=Marshal.GetLastWin32Error();h.Dispose();throw new Win32Exception(error);}return new NativeHandle(h);}
 public void Directory(string p,string sd){Scope(p);LimitlessV2Staging.Need(sd==LimitlessV2Staging.DirSd,"STAGE_CREATE_SD");IntPtr memory;var a=Security(sd,out memory);try{Ok(CreateDirectory(p,ref a));}finally{LocalFree(memory);}}
 public void File(string p,byte[] bytes,string sd){Scope(p);LimitlessV2Staging.Need(sd==LimitlessV2Staging.FileSd,"STAGE_CREATE_SD");IntPtr memory;var a=Security(sd,out memory);try{using(var h=CreateFile(p,0xc0020000,1,ref a,1,0x00200080,IntPtr.Zero)){if(h.IsInvalid)throw new Win32Exception(Marshal.GetLastWin32Error());uint written;Ok(WriteFile(h,bytes,(uint)bytes.Length,out written,IntPtr.Zero));LimitlessV2Staging.Need(written==bytes.Length,"STAGE_SHORT_WRITE");Ok(FlushFileBuffers(h));}}finally{LocalFree(memory);}}
 public string[] Children(string p){Admin();return System.IO.Directory.GetFileSystemEntries(p);}
 sealed class NativeHandle:V2StageHandle {
  SafeFileHandle h;public NativeHandle(SafeFileHandle value){h=value;}
  public override V2StageInfo Info(){
   BY by;Ok(GetFileInformationByHandle(h,out by));var name=new StringBuilder(32768);uint size=GetFinalPathNameByHandle(h,name,(uint)name.Capacity,0);Ok(size>0&&size<name.Capacity);string path=name.ToString();LimitlessV2Staging.Need(path.StartsWith(@"\\?\"),"STAGE_FINAL_PATH");path=path.Substring(4);if(path.Length==2)path+="\\";
   var fs=new StringBuilder(64);uint serial,max,flags;Ok(GetVolumeInformationByHandleW(h,null,0,out serial,out max,out flags,fs,(uint)fs.Capacity));
   IntPtr owner,group,dacl,sacl,sd;uint error=GetSecurityInfo(h,1,7,out owner,out group,out dacl,out sacl,out sd);if(error!=0)throw new Win32Exception((int)error);string sddl;try{byte[] raw=new byte[GetSecurityDescriptorLength(sd)];Marshal.Copy(sd,raw,0,raw.Length);sddl=new RawSecurityDescriptor(raw,0).GetSddlForm(AccessControlSections.Owner|AccessControlSections.Group|AccessControlSections.Access);}finally{LocalFree(sd);}
   IntPtr buffer=Marshal.AllocHGlobal(65536);var streams=new List<string>();bool pending;try{
    Ok(GetFileInformationByHandleEx(h,1,buffer,64));pending=Marshal.ReadByte(buffer,20)!=0;
    if(GetFileInformationByHandleEx(h,7,buffer,65536)){
     int offset=0;while(true){int next=Marshal.ReadInt32(buffer,offset),len=Marshal.ReadInt32(buffer,offset+4);LimitlessV2Staging.Need(len>=0&&len%2==0&&offset+24+len<=65536,"STAGE_STREAM_BUFFER");streams.Add(Marshal.PtrToStringUni(IntPtr.Add(buffer,offset+24),len/2));if(next==0)break;LimitlessV2Staging.Need(next>=24&&offset+next<65536,"STAGE_STREAM_OFFSET");offset+=next;}
    }else{int code=Marshal.GetLastWin32Error();if(code!=38)throw new Win32Exception(code);}
   }finally{Marshal.FreeHGlobal(buffer);}
   bool directory=(by.Attributes&0x10)!=0;
   // directory의 기본 unnamed stream은 데이터 ADS가 아니므로 별도 목록에서 제외합니다.
   if(directory)streams.RemoveAll(x=>x=="::$DATA");
   return new V2StageInfo{Path=path,Id=by.IndexHi.ToString("x8")+by.IndexLo.ToString("x8"),Volume=by.Volume.ToString("x8"),Sddl=sddl,Directory=directory,Reparse=(by.Attributes&0x400)!=0,DeletePending=pending,Links=by.Links,Streams=streams.ToArray(),FileSystem=fs.ToString()};
  }
  public override byte[] Read(){var i=Info();LimitlessV2Staging.Need(!i.Directory,"STAGE_READ_FILE_ONLY");long pos;Ok(SetFilePointerEx(h,0,out pos,0));using(var ms=new MemoryStream()){var b=new byte[65536];uint read;while(true){Ok(ReadFile(h,b,(uint)b.Length,out read,IntPtr.Zero));if(read==0)break;LimitlessV2Staging.Need(ms.Length+read<=128*1024*1024,"STAGE_READ_BOUND");ms.Write(b,0,(int)read);}return ms.ToArray();}}
  public override void Dispose(){h.Dispose();}
 }
}

// 새 V2 경로만 CREATE_NEW하고 생성 시점의 protected SDDL을 확인합니다. 삭제/ACL 수정 함수는 없습니다.
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Security.AccessControl;
using Microsoft.Win32.SafeHandles;
public static class LimitlessV2Publish {
 [StructLayout(LayoutKind.Sequential)]struct Security{public int Length;public IntPtr Descriptor;public int Inherit;}
 [DllImport("advapi32.dll",CharSet=CharSet.Unicode,SetLastError=true)]static extern bool ConvertStringSecurityDescriptorToSecurityDescriptor(string text,uint revision,out IntPtr descriptor,out uint size);
 [DllImport("kernel32.dll",CharSet=CharSet.Unicode,SetLastError=true)]static extern bool CreateDirectory(string path,ref Security security);
 [DllImport("kernel32.dll",CharSet=CharSet.Unicode,SetLastError=true)]static extern SafeFileHandle CreateFile(string path,uint access,uint share,ref Security security,uint creation,uint flags,IntPtr template);
 [DllImport("kernel32.dll")]static extern IntPtr LocalFree(IntPtr memory);
 static void Scope(string path){
  using(var id=WindowsIdentity.GetCurrent())LimitlessV2Contract.Need(new WindowsPrincipal(id).IsInRole(WindowsBuiltInRole.Administrator),"V2_ADMIN_REQUIRED");
  LimitlessV2Contract.Need(LimitlessV2Contract.Inside(path,LimitlessV2Contract.CodeRoot)||LimitlessV2Contract.Inside(path,LimitlessV2Contract.StateRoot)||LimitlessV2Contract.Inside(path,LimitlessV2Contract.ApprovalRoot),"V2_WRITE_SCOPE_HOLD");
  Ancestors(Path.GetDirectoryName(Path.GetFullPath(path)));
 }
 public static void Ancestors(string path){
  for(string p=Path.GetFullPath(path);p!=null;p=Path.GetDirectoryName(p)){
   var i=new DirectoryInfo(p);LimitlessV2Contract.Need(i.Exists&&(i.Attributes&FileAttributes.ReparsePoint)==0,"V2_PARENT_REPARSE_OR_MISSING_HOLD");
   var acl=i.GetAccessControl();
   foreach(FileSystemAccessRule r in acl.GetAccessRules(true,true,typeof(SecurityIdentifier))){
    if((r.PropagationFlags&PropagationFlags.InheritOnly)!=0)continue;
    string sid=r.IdentityReference.Value;
    if(sid=="S-1-5-18"||sid=="S-1-5-32-544"||sid=="S-1-5-80-956008885-3418522649-1831038044-1853292631-2271478464")continue;
    var mask=FileSystemRights.Delete|FileSystemRights.DeleteSubdirectoriesAndFiles|FileSystemRights.ChangePermissions|FileSystemRights.TakeOwnership;
    LimitlessV2Contract.Need(r.AccessControlType!=AccessControlType.Allow||(r.FileSystemRights&mask)==0,"V2_PARENT_WRITER_HOLD");
   }
  }
 }
 static Security Descriptor(string sddl,out IntPtr memory){uint size;if(!ConvertStringSecurityDescriptorToSecurityDescriptor(sddl,1,out memory,out size))throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());return new Security{Length=Marshal.SizeOf<Security>(),Descriptor=memory};}
 public static void NewDirectory(string path,string sddl){Scope(path);IntPtr memory;var sd=Descriptor(sddl,out memory);try{if(!CreateDirectory(path,ref sd))throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());}finally{LocalFree(memory);}Verify(path,sddl);}
 public static void NewFile(string path,byte[] bytes,string sddl){
  Scope(path);IntPtr memory;var sd=Descriptor(sddl,out memory);
  try{using(var h=CreateFile(path,0x40000000,0,ref sd,1,0x80,IntPtr.Zero)){if(h.IsInvalid)throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());using(var s=new FileStream(h,FileAccess.Write)){s.Write(bytes,0,bytes.Length);s.Flush(true);}}}finally{LocalFree(memory);}Verify(path,sddl);
 }
 public static void Verify(string path,string sddl){
  var attr=File.GetAttributes(path);LimitlessV2Contract.Need((attr&FileAttributes.ReparsePoint)==0,"V2_TARGET_REPARSE_HOLD");
  FileSystemSecurity acl=(attr&FileAttributes.Directory)!=0?new DirectoryInfo(path).GetAccessControl():new FileInfo(path).GetAccessControl();
  LimitlessV2Contract.ExactSddl(acl.GetSecurityDescriptorSddlForm(AccessControlSections.Owner|AccessControlSections.Group|AccessControlSections.Access),sddl);
 }
}

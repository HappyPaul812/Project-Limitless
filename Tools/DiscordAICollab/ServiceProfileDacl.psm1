# 서비스 계정 자기 프로필의 DACL만 설정합니다. 소유자/그룹/SACL·토큰 권한은 변경하지 않습니다.
if(-not ('LimitlessProfileDaclV1' -as [type])) {
 Add-Type -TypeDefinition @"
using System;
using System.Runtime.InteropServices;
public static class LimitlessProfileDaclV1 {
 [DllImport("advapi32.dll", CharSet=CharSet.Unicode, SetLastError=true)]
 [return: MarshalAs(UnmanagedType.Bool)]
 private static extern bool ConvertStringSecurityDescriptorToSecurityDescriptorW(string text, uint revision, out IntPtr sd, out uint size);
 [DllImport("advapi32.dll", SetLastError=true)]
 [return: MarshalAs(UnmanagedType.Bool)]
 private static extern bool GetSecurityDescriptorDacl(IntPtr sd, [MarshalAs(UnmanagedType.Bool)] out bool present, out IntPtr dacl, [MarshalAs(UnmanagedType.Bool)] out bool defaulted);
 [DllImport("advapi32.dll", CharSet=CharSet.Unicode, ExactSpelling=true)]
 private static extern uint SetNamedSecurityInfoW(string name, int type, uint info, IntPtr owner, IntPtr group, IntPtr dacl, IntPtr sacl);
 [DllImport("kernel32.dll")]
 private static extern IntPtr LocalFree(IntPtr memory);
 public static uint SetProtectedDacl(string path, string sid) {
  IntPtr descriptor=IntPtr.Zero; uint size;
  string text="D:P(A;OICI;FA;;;"+sid+")(A;OICI;FA;;;SY)(A;OICI;FA;;;BA)";
  if(!ConvertStringSecurityDescriptorToSecurityDescriptorW(text,1,out descriptor,out size)) return (uint)Marshal.GetLastWin32Error();
  try {
   bool present,defaulted; IntPtr dacl;
   if(!GetSecurityDescriptorDacl(descriptor,out present,out dacl,out defaulted) || !present || dacl==IntPtr.Zero) return 87;
   // DACL_SECURITY_INFORMATION | PROTECTED_DACL_SECURITY_INFORMATION only.
   return SetNamedSecurityInfoW(path,1,0x80000004u,IntPtr.Zero,IntPtr.Zero,dacl,IntPtr.Zero);
  } finally {LocalFree(descriptor);}
 }
}
"@
}
function Set-LimitlessProfileDacl {
 param([string]$Path,[string]$ServiceSid)
 $current=[Security.Principal.WindowsIdentity]::GetCurrent().User.Value
 $expected=Join-Path ([Environment]::GetFolderPath([Environment+SpecialFolder]::UserProfile)) '.codex'
 if($current -ne $ServiceSid -or [IO.Path]::GetFullPath($Path) -ine [IO.Path]::GetFullPath($expected)) {throw 'FIXED_OWN_SERVICE_PROFILE_REQUIRED'}
 $before=Get-Acl -LiteralPath $Path
 if($before.GetOwner([Security.Principal.SecurityIdentifier]).Value -ne $current) {throw 'SERVICE_HOME_OWNER_REVIEW_REQUIRED'}
 $code=[LimitlessProfileDaclV1]::SetProtectedDacl($Path,$current)
 $after=Get-Acl -LiteralPath $Path
 return @{native_error=$code;owner_preserved=($after.GetOwner([Security.Principal.SecurityIdentifier]).Value -eq $current);protected=$after.AreAccessRulesProtected}
}
Export-ModuleMember -Function Set-LimitlessProfileDacl

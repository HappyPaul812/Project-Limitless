# 읽기 전용 LSA 계정 권한과 현재 로그온 유형 조회. 권한 추가/토큰 복제 함수는 없습니다.
if(-not ('LimitlessBatchMetadataV1' -as [type])) {
Add-Type -TypeDefinition @"
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.ComponentModel;
public static class LimitlessBatchMetadataV1 {
 [StructLayout(LayoutKind.Sequential)] struct Luid { public uint Low; public int High; }
 [StructLayout(LayoutKind.Sequential)] struct Text { public ushort Length,MaxLength; public IntPtr Buffer; }
 [StructLayout(LayoutKind.Sequential)] struct Attr { public int Length; public IntPtr Root,ObjectName; public uint Attributes; public IntPtr Descriptor,Quality; }
 [StructLayout(LayoutKind.Sequential)] struct Stats { public Luid TokenId,AuthenticationId; public long Expiration; public int Type,Level; public uint Charged,Available,Groups,Privileges; public Luid Modified; }
 [StructLayout(LayoutKind.Sequential)] struct Logon { public uint Size; public Luid Id; public Text User,Domain,Package; public uint Type,Session; }
 [DllImport("advapi32.dll")] static extern uint LsaOpenPolicy(IntPtr name,ref Attr attr,uint access,out IntPtr handle);
 [DllImport("advapi32.dll")] static extern uint LsaEnumerateAccountRights(IntPtr handle,IntPtr sid,out IntPtr rights,out uint count);
 [DllImport("advapi32.dll")] static extern uint LsaNtStatusToWinError(uint status);
 [DllImport("advapi32.dll")] static extern uint LsaFreeMemory(IntPtr memory);
 [DllImport("advapi32.dll")] static extern uint LsaClose(IntPtr handle);
 [DllImport("advapi32.dll",SetLastError=true)] [return:MarshalAs(UnmanagedType.Bool)]
 static extern bool GetTokenInformation(IntPtr token,int kind,IntPtr data,int length,out int required);
 [DllImport("secur32.dll")] static extern uint LsaGetLogonSessionData(ref Luid id,out IntPtr data);
 [DllImport("secur32.dll")] static extern uint LsaFreeReturnBuffer(IntPtr data);
 public static string[] Rights(string sidText) {
  var sid=new SecurityIdentifier(sidText);var bytes=new byte[sid.BinaryLength];sid.GetBinaryForm(bytes,0);
  var pinned=GCHandle.Alloc(bytes,GCHandleType.Pinned);IntPtr handle=IntPtr.Zero,rights=IntPtr.Zero;
  try {
   var attr=new Attr();attr.Length=Marshal.SizeOf<Attr>();
   uint status=LsaOpenPolicy(IntPtr.Zero,ref attr,0x800,out handle);
   if(status!=0)throw new Win32Exception((int)LsaNtStatusToWinError(status));
   uint count;status=LsaEnumerateAccountRights(handle,pinned.AddrOfPinnedObject(),out rights,out count);
   if(status==0xc0000034)return new string[0];
   if(status!=0)throw new Win32Exception((int)LsaNtStatusToWinError(status));
   var result=new List<string>();int step=Marshal.SizeOf<Text>();
   for(int i=0;i<count;i++){var text=Marshal.PtrToStructure<Text>(IntPtr.Add(rights,i*step));result.Add(Marshal.PtrToStringUni(text.Buffer,text.Length/2));}
   return result.ToArray();
  } finally {if(rights!=IntPtr.Zero)LsaFreeMemory(rights);if(handle!=IntPtr.Zero)LsaClose(handle);pinned.Free();}
 }
 public static int[] CurrentLogon() {
  using(var identity=WindowsIdentity.GetCurrent()) {
   int size;GetTokenInformation(identity.Token,10,IntPtr.Zero,0,out size);
   IntPtr stats=Marshal.AllocHGlobal(size),data=IntPtr.Zero;
   try {
    if(!GetTokenInformation(identity.Token,10,stats,size,out size))throw new Win32Exception(Marshal.GetLastWin32Error());
    var value=Marshal.PtrToStructure<Stats>(stats);uint status=LsaGetLogonSessionData(ref value.AuthenticationId,out data);
    if(status!=0)throw new Win32Exception((int)LsaNtStatusToWinError(status));
    var logon=Marshal.PtrToStructure<Logon>(data);return new int[]{(int)logon.Type,(int)logon.Session};
   } finally {if(data!=IntPtr.Zero)LsaFreeReturnBuffer(data);Marshal.FreeHGlobal(stats);}
  }
 }
}
"@
}

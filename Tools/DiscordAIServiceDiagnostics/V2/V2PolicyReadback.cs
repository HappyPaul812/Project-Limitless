// 새 SID의 직접 권한만 읽습니다. 설치가 부여한 SeBatchLogonRight 외의 권한이면 HOLD합니다.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Principal;
public static class LimitlessV2PolicyReadback {
 [StructLayout(LayoutKind.Sequential)]struct Text{public ushort Length,Max;public IntPtr Buffer;}
 [StructLayout(LayoutKind.Sequential)]struct Attr{public int Length;public IntPtr Root,Object;public uint Flags;public IntPtr Descriptor,Quality;}
 [DllImport("advapi32.dll")]static extern uint LsaOpenPolicy(IntPtr name,ref Attr attr,uint access,out IntPtr handle);
 [DllImport("advapi32.dll")]static extern uint LsaEnumerateAccountRights(IntPtr handle,byte[] sid,out IntPtr rights,out uint count);
 [DllImport("advapi32.dll")]static extern uint LsaFreeMemory(IntPtr memory);
 [DllImport("advapi32.dll")]static extern uint LsaClose(IntPtr handle);
 [DllImport("advapi32.dll")]static extern uint LsaNtStatusToWinError(uint status);
 public static string[] Read(string sid){
  var id=new SecurityIdentifier(sid);var bytes=new byte[id.BinaryLength];id.GetBinaryForm(bytes,0);IntPtr h=IntPtr.Zero,r=IntPtr.Zero;
  try{var a=new Attr{Length=Marshal.SizeOf<Attr>()};uint s=LsaOpenPolicy(IntPtr.Zero,ref a,0x800,out h);if(s!=0)throw new Win32Exception((int)LsaNtStatusToWinError(s));uint count;s=LsaEnumerateAccountRights(h,bytes,out r,out count);if(s!=0)throw new Win32Exception((int)LsaNtStatusToWinError(s));var list=new List<string>();for(int i=0;i<count;i++){var t=Marshal.PtrToStructure<Text>(IntPtr.Add(r,checked(i*Marshal.SizeOf<Text>())));list.Add(Marshal.PtrToStringUni(t.Buffer,t.Length/2));}return list.ToArray();}
  finally{if(r!=IntPtr.Zero)LsaFreeMemory(r);if(h!=IntPtr.Zero)LsaClose(h);}
 }
}

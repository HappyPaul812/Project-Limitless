// 새로 생성한 두 SID만 대상으로 합니다. UTF-16 byte Length/MaxLength와 원시 오류를 보존합니다.
using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.ComponentModel;
public static class DedicatedAccountPolicy {
 [StructLayout(LayoutKind.Sequential)]struct Text{public ushort Length,Max;public IntPtr Buffer;}
 [StructLayout(LayoutKind.Sequential)]struct Attr{public int Length;public IntPtr Root,Object;public uint Flags;public IntPtr Descriptor,Quality;}
 [DllImport("advapi32.dll")]static extern uint LsaOpenPolicy(IntPtr name,ref Attr attr,uint access,out IntPtr handle);
 [DllImport("advapi32.dll")]static extern uint LsaAddAccountRights(IntPtr h,byte[] sid,Text[] rights,uint count);
 [DllImport("advapi32.dll")]static extern uint LsaEnumerateAccountRights(IntPtr h,byte[] sid,out IntPtr rights,out uint count);
 [DllImport("advapi32.dll")]static extern uint LsaFreeMemory(IntPtr p);
 [DllImport("advapi32.dll")]static extern uint LsaClose(IntPtr h);
 [DllImport("advapi32.dll")]static extern uint LsaNtStatusToWinError(uint status);
 public sealed class PolicyError:Win32Exception{public uint NtStatus{get;}public string Operation{get;}public PolicyError(string op,uint s):base((int)LsaNtStatusToWinError(s)){NtStatus=s;Operation=op;}}
 public static int[] UnicodeLength(string value){int bytes=checked(value.Length*2);DedicatedContract.Need(bytes<=ushort.MaxValue-2,"D3_LSA_BOUND");return new[]{bytes,bytes+2};}
 public static void Grant(string sid,string recorded,string other,string admin){
  DedicatedContract.Distinct(sid,other,admin);DedicatedContract.Need(sid==recorded,"D3_RECORDED_NEW_SID_ONLY");using(var i=WindowsIdentity.GetCurrent())DedicatedContract.Need(new WindowsPrincipal(i).IsInRole(WindowsBuiltInRole.Administrator),"D3_ADMIN_LSA");
  var identity=new SecurityIdentifier(sid);var binary=new byte[identity.BinaryLength];identity.GetBinaryForm(binary,0);IntPtr h=IntPtr.Zero;var texts=new Text[DedicatedContract.Rights.Length];Exception original=null;
  try{
   for(int n=0;n<texts.Length;n++){var lengths=UnicodeLength(DedicatedContract.Rights[n]);texts[n]=new Text{Length=(ushort)lengths[0],Max=(ushort)lengths[1],Buffer=Marshal.StringToHGlobalUni(DedicatedContract.Rights[n])};}
   var attr=new Attr{Length=Marshal.SizeOf<Attr>()};uint status=LsaOpenPolicy(IntPtr.Zero,ref attr,0x810,out h);if(status!=0)throw new PolicyError("OPEN",status);status=LsaAddAccountRights(h,binary,texts,(uint)texts.Length);if(status!=0)throw new PolicyError("ADD_RIGHTS",status);
  }catch(Exception e){original=e;throw;}finally{
   foreach(var t in texts)if(t.Buffer!=IntPtr.Zero)Marshal.FreeHGlobal(t.Buffer);
   if(h!=IntPtr.Zero){uint status=LsaClose(h);if(status!=0){if(original==null)throw new PolicyError("CLOSE",status);original.Data["cleanup_ntstatus"]=status;original.Data["cleanup_win32"]=LsaNtStatusToWinError(status);}}
  }
 }
 public static string[] Read(string sid){var identity=new SecurityIdentifier(sid);var binary=new byte[identity.BinaryLength];identity.GetBinaryForm(binary,0);IntPtr h=IntPtr.Zero,r=IntPtr.Zero;Exception original=null;
  try{var a=new Attr{Length=Marshal.SizeOf<Attr>()};uint s=LsaOpenPolicy(IntPtr.Zero,ref a,0x800,out h);if(s!=0)throw new PolicyError("READ_OPEN",s);uint count;s=LsaEnumerateAccountRights(h,binary,out r,out count);if(s!=0)throw new PolicyError("READ_RIGHTS",s);var all=new string[count];for(int n=0;n<count;n++){var t=Marshal.PtrToStructure<Text>(IntPtr.Add(r,n*Marshal.SizeOf<Text>()));all[n]=Marshal.PtrToStringUni(t.Buffer,t.Length/2);}return all;}catch(Exception e){original=e;throw;}finally{if(r!=IntPtr.Zero)LsaFreeMemory(r);if(h!=IntPtr.Zero){uint s=LsaClose(h);if(s!=0){if(original==null)throw new PolicyError("READ_CLOSE",s);original.Data["cleanup_ntstatus"]=s;original.Data["cleanup_win32"]=LsaNtStatusToWinError(s);}}}
 }
}

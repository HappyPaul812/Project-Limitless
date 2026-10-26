// 관리자 설치 시에만 새 시험 봇 SID에 Batch 권한을 줍니다. 기존 서비스 SID는 수정하지 않습니다.
using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.ComponentModel;
public static class LimitlessTrialAccountPolicyV1 {
 [StructLayout(LayoutKind.Sequential)]struct Text{public ushort Length,Max;public IntPtr Buffer;}
 [StructLayout(LayoutKind.Sequential)]struct Attr{public int Length;public IntPtr Root,Object;public uint Flags;public IntPtr Descriptor,Quality;}
 [DllImport("advapi32.dll")]static extern uint LsaOpenPolicy(IntPtr name,ref Attr attr,uint access,out IntPtr handle);
 [DllImport("advapi32.dll")]static extern uint LsaAddAccountRights(IntPtr handle,byte[] sid,Text[] rights,uint count);
 [DllImport("advapi32.dll")]static extern uint LsaClose(IntPtr handle);
 [DllImport("advapi32.dll")]static extern uint LsaNtStatusToWinError(uint status);
 public static void GrantNewBotBatch(string newSid,string recordedNewSid,string workerSid,string adminSid){
  if(newSid!=recordedNewSid || newSid==workerSid || newSid==adminSid)throw new UnauthorizedAccessException("NEW_BOT_SID_HOLD");
  using(var current=WindowsIdentity.GetCurrent()){if(!new WindowsPrincipal(current).IsInRole(WindowsBuiltInRole.Administrator))throw new UnauthorizedAccessException();}
  var sid=new SecurityIdentifier(newSid);var bytes=new byte[sid.BinaryLength];sid.GetBinaryForm(bytes,0);
  IntPtr handle=IntPtr.Zero,text=Marshal.StringToHGlobalUni("SeBatchLogonRight");
  try{
   var attr=new Attr{Length=Marshal.SizeOf<Attr>()};uint status=LsaOpenPolicy(IntPtr.Zero,ref attr,0x810,out handle);
   if(status!=0)throw new Win32Exception((int)LsaNtStatusToWinError(status));
   var right=new Text{Length=32,Max=34,Buffer=text};status=LsaAddAccountRights(handle,bytes,new[]{right},1);
   if(status!=0)throw new Win32Exception((int)LsaNtStatusToWinError(status));
  }finally{if(handle!=IntPtr.Zero)LsaClose(handle);Marshal.FreeHGlobal(text);}
 }
}

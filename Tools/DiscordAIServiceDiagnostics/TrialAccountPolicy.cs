// 관리자 설치 시에만 새 시험 봇 SID에 Batch 권한을 줍니다. 기존 서비스 SID는 수정하지 않습니다.
using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.ComponentModel;
using System.Text;
public static class LimitlessTrialAccountPolicyV1 {
 [StructLayout(LayoutKind.Sequential)]internal struct Text{public ushort Length,Max;public IntPtr Buffer;}
 [StructLayout(LayoutKind.Sequential)]struct Attr{public int Length;public IntPtr Root,Object;public uint Flags;public IntPtr Descriptor,Quality;}
 [DllImport("advapi32.dll")]static extern uint LsaOpenPolicy(IntPtr name,ref Attr attr,uint access,out IntPtr handle);
 [DllImport("advapi32.dll")]static extern uint LsaAddAccountRights(IntPtr handle,byte[] sid,Text[] rights,uint count);
 [DllImport("advapi32.dll")]static extern uint LsaClose(IntPtr handle);
 [DllImport("advapi32.dll")]static extern uint LsaNtStatusToWinError(uint status);
 // 오류 번호만 보존합니다. 계정 암호나 입력 자료는 예외에 넣지 않습니다.
 public sealed class LsaPolicyException : Win32Exception {
  public uint NtStatus {get;}
  public string Operation {get;}
  internal LsaPolicyException(string operation,uint status,uint win32):base(unchecked((int)win32),"LSA_"+operation+"_FAILED"){NtStatus=status;Operation=operation;}
 }
 // 주입 경계는 assembly 내부 전용입니다. 실제 공개 진입점의 관리자/SID 검사를 우회하지 않습니다.
 internal interface ILsaApi {
  uint Open(out IntPtr handle);
  uint Add(IntPtr handle,byte[] sid,Text[] rights);
  uint Close(IntPtr handle);
  uint Win32(uint status);
 }
 sealed class NativeApi : ILsaApi {
  public uint Open(out IntPtr handle){var attr=new Attr{Length=Marshal.SizeOf<Attr>()};return LsaOpenPolicy(IntPtr.Zero,ref attr,0x810,out handle);}
  public uint Add(IntPtr handle,byte[] sid,Text[] rights){return LsaAddAccountRights(handle,sid,rights,1);}
  public uint Close(IntPtr handle){return LsaClose(handle);}
  public uint Win32(uint status){return LsaNtStatusToWinError(status);}
 }
 internal sealed class UnicodeRight : IDisposable {
  internal Text Value;
  internal UnicodeRight(string name){
   if(string.IsNullOrEmpty(name) || name.IndexOf('\0')>=0)throw new ArgumentException("LSA_NAME_HOLD");
   int count=new UnicodeEncoding(false,false,true).GetByteCount(name);
   if(count>ushort.MaxValue-2)throw new ArgumentOutOfRangeException(nameof(name),"LSA_LENGTH_HOLD");
   IntPtr memory=Marshal.StringToHGlobalUni(name);
   try{
    if(memory==IntPtr.Zero || Marshal.PtrToStringUni(memory,count/2)!=name || Marshal.ReadInt16(memory,count)!=0)throw new InvalidOperationException("LSA_BUFFER_HOLD");
    Value=new Text{Length=checked((ushort)count),Max=checked((ushort)(count+2)),Buffer=memory};
   }catch{if(memory!=IntPtr.Zero)Marshal.FreeHGlobal(memory);throw;}
  }
  public void Dispose(){var memory=Value.Buffer;Value=default(Text);if(memory!=IntPtr.Zero)Marshal.FreeHGlobal(memory);}
 }
 internal static void GrantBatchCore(byte[] sid,ILsaApi api){
  IntPtr handle=IntPtr.Zero;Exception failure=null;
  using(var name=new UnicodeRight("SeBatchLogonRight")){
   try{
    uint status=api.Open(out handle);
    if(status!=0)throw new LsaPolicyException("OPEN",status,api.Win32(status));
    if(handle==IntPtr.Zero)throw new InvalidOperationException("LSA_NULL_HANDLE_HOLD");
    status=api.Add(handle,sid,new[]{name.Value});
    if(status!=0)throw new LsaPolicyException("ADD_RIGHT",status,api.Win32(status));
   }catch(Exception error){failure=error;throw;}
   finally{if(handle!=IntPtr.Zero){uint status=api.Close(handle);if(status!=0){
    if(failure==null)throw new LsaPolicyException("CLOSE",status,api.Win32(status));
    failure.Data["cleanup_ntstatus"]=status;failure.Data["cleanup_win32"]=api.Win32(status);
   }}}
  }
 }
 public static void GrantNewBotBatch(string newSid,string recordedNewSid,string workerSid,string adminSid){
  if(newSid!=recordedNewSid || newSid==workerSid || newSid==adminSid)throw new UnauthorizedAccessException("NEW_BOT_SID_HOLD");
  using(var current=WindowsIdentity.GetCurrent()){if(!new WindowsPrincipal(current).IsInRole(WindowsBuiltInRole.Administrator))throw new UnauthorizedAccessException();}
  var sid=new SecurityIdentifier(newSid);var bytes=new byte[sid.BinaryLength];sid.GetBinaryForm(bytes,0);
  GrantBatchCore(bytes,new NativeApi());
 }
}

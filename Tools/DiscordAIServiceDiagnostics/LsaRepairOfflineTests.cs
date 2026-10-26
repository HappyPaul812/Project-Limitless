// 같은 테스트 assembly에만 포함하는 가짜 LSA입니다. 설치 payload에는 넣지 않습니다.
namespace LimitlessLsaOffline {
 using System;
 using System.Runtime.InteropServices;
 using System.Text;
 using P=LimitlessTrialAccountPolicyV1;
 public static class Tests {
  sealed class Fake : P.ILsaApi {
   internal uint OpenStatus,AddStatus,CloseStatus;internal bool NullHandle;
   internal int Opens,Adds,Closes;internal string Name;internal ushort Length,Max;internal bool NullTerminated;
   public uint Open(out IntPtr handle){Opens++;handle=(OpenStatus!=0 || NullHandle)?IntPtr.Zero:new IntPtr(123);return OpenStatus;}
   public uint Add(IntPtr handle,byte[] sid,P.Text[] rights){
    Adds++;if(rights.Length!=1 || handle!=new IntPtr(123))throw new Exception("FAKE_SCHEMA_HOLD");
    var r=rights[0];Name=Marshal.PtrToStringUni(r.Buffer,r.Length/2);Length=r.Length;Max=r.Max;NullTerminated=Marshal.ReadInt16(r.Buffer,r.Length)==0;return AddStatus;
   }
   public uint Close(IntPtr handle){Closes++;return CloseStatus;}
   public uint Win32(uint status){return status==0xc0000060u?1313u:5u;}
  }
  static void Need(bool value){if(!value)throw new Exception("OFFLINE_ASSERT");}
  public static bool ExactBuffer(){using(var r=new P.UnicodeRight("SeBatchLogonRight")){Need(r.Value.Length==34 && r.Value.Max==36);Need(Marshal.PtrToStringUni(r.Value.Buffer,17)=="SeBatchLogonRight" && Marshal.ReadInt16(r.Value.Buffer,34)==0);}return true;}
  public static bool UnicodeBuffer(){string name="권한\U0001F600";using(var r=new P.UnicodeRight(name)){Need(r.Value.Length==Encoding.Unicode.GetByteCount(name) && r.Value.Max==r.Value.Length+2);Need(Marshal.PtrToStringUni(r.Value.Buffer,r.Value.Length/2)==name);}return true;}
  public static bool RejectName(string name){try{using(var r=new P.UnicodeRight(name)){}return false;}catch(ArgumentException){return true;}}
  public static bool Range(){using(var r=new P.UnicodeRight(new string('x',32766))){Need(r.Value.Length==65532 && r.Value.Max==65534);}return RejectName(new string('x',32767));}
  public static bool Dispose(){var r=new P.UnicodeRight("SeBatchLogonRight");r.Dispose();Need(r.Value.Buffer==IntPtr.Zero && r.Value.Length==0);r.Dispose();return true;}
  public static bool Success(){var f=new Fake();P.GrantBatchCore(new byte[]{1},f);Need(f.Opens==1 && f.Adds==1 && f.Closes==1 && f.Name=="SeBatchLogonRight" && f.Length==34 && f.Max==36 && f.NullTerminated);return true;}
  public static bool AddFailure(){var f=new Fake{AddStatus=0xc0000060u};try{P.GrantBatchCore(new byte[]{1},f);return false;}catch(P.LsaPolicyException e){Need(e.NtStatus==0xc0000060u && e.NativeErrorCode==1313 && e.Operation=="ADD_RIGHT" && f.Adds==1 && f.Closes==1);return true;}}
  public static bool OpenFailure(){var f=new Fake{OpenStatus=0xc0000022u};try{P.GrantBatchCore(new byte[]{1},f);return false;}catch(P.LsaPolicyException e){Need(e.NtStatus==0xc0000022u && e.NativeErrorCode==5 && f.Opens==1 && f.Adds==0 && f.Closes==0);return true;}}
  public static bool CloseFailure(){var f=new Fake{CloseStatus=0xc0000022u};try{P.GrantBatchCore(new byte[]{1},f);return false;}catch(P.LsaPolicyException e){Need(e.Operation=="CLOSE" && e.NtStatus==0xc0000022u && f.Adds==1 && f.Closes==1);return true;}}
  public static bool PrimaryPreserved(){var f=new Fake{AddStatus=0xc0000060u,CloseStatus=0xc0000022u};try{P.GrantBatchCore(new byte[]{1},f);return false;}catch(P.LsaPolicyException e){Need(e.Operation=="ADD_RIGHT" && e.NativeErrorCode==1313 && (uint)e.Data["cleanup_ntstatus"]==0xc0000022u && f.Adds==1);return true;}}
  public static bool NullHandle(){var f=new Fake{NullHandle=true};try{P.GrantBatchCore(new byte[]{1},f);return false;}catch(InvalidOperationException){Need(f.Opens==1 && f.Adds==0 && f.Closes==0);return true;}}
  public static Exception Nested(){return new Exception("SYNTHETIC_TOKEN_SECRET",new P.LsaPolicyException("ADD_RIGHT",0xc0000060u,1313));}
 }
}

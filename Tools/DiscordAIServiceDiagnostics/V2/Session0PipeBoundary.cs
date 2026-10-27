// 격리 후보: Windows pipe의 실제 토큰을 읽습니다. JSON의 SID/admin 값을 신뢰하지 않습니다.
using System;
using System.IO;
using System.IO.Pipes;
using System.Security.Principal;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Security.AccessControl;
using System.Collections.Generic;
using Microsoft.Win32.SafeHandles;

public static class LimitlessSession0PipeV2 {
 [StructLayout(LayoutKind.Sequential)] struct Security { public int Length; public IntPtr Descriptor; public int Inherit; }
 [StructLayout(LayoutKind.Sequential)] struct Luid { public uint Low; public int High; }
 [StructLayout(LayoutKind.Sequential)] struct Text { public ushort Length,MaxLength; public IntPtr Buffer; }
 [StructLayout(LayoutKind.Sequential)] struct Stats { public Luid Id,AuthenticationId; public long Expiration; public int Type,Level; public uint Charged,Available,Groups,Privileges; public Luid Modified; }
 [StructLayout(LayoutKind.Sequential)] struct Logon { public uint Size; public Luid Id; public Text User,Domain,Package; public uint Type,Session; }
 [DllImport("secur32.dll")] static extern uint LsaGetLogonSessionData(ref Luid id,out IntPtr data);
 [DllImport("secur32.dll")] static extern uint LsaFreeReturnBuffer(IntPtr data);
 [DllImport("advapi32.dll",SetLastError=true)] static extern bool GetTokenInformation(IntPtr token,int kind,IntPtr data,int size,out int required);
 [DllImport("advapi32.dll",CharSet=CharSet.Unicode,SetLastError=true)] static extern bool ConvertStringSecurityDescriptorToSecurityDescriptor(string text,uint revision,out IntPtr descriptor,out uint size);
 [DllImport("kernel32.dll")] static extern IntPtr LocalFree(IntPtr value);
 [DllImport("kernel32.dll",CharSet=CharSet.Unicode,SetLastError=true)] static extern SafePipeHandle CreateNamedPipe(string name,uint mode,uint pipeMode,uint instances,uint output,uint input,uint timeout,ref Security security);
 [DllImport("kernel32.dll",CharSet=CharSet.Unicode,SetLastError=true)] static extern SafePipeHandle CreateFile(string name,uint access,uint share,IntPtr security,uint creation,uint flags,IntPtr template);
 [DllImport("kernel32.dll",SetLastError=true)] static extern bool GetNamedPipeServerProcessId(SafePipeHandle pipe,out uint pid);
 [DllImport("kernel32.dll",SetLastError=true)] static extern bool GetNamedPipeClientProcessId(SafePipeHandle pipe,out uint pid);
 [DllImport("kernel32.dll",SetLastError=true)] static extern IntPtr OpenProcess(uint access,bool inherit,uint pid);
 [DllImport("advapi32.dll",SetLastError=true)] static extern bool OpenProcessToken(IntPtr process,uint access,out IntPtr token);
 [DllImport("kernel32.dll")] static extern bool CloseHandle(IntPtr handle);
 [DllImport("kernel32.dll")] static extern IntPtr GetCurrentProcess();
 [DllImport("advapi32.dll",SetLastError=true)] static extern bool GetKernelObjectSecurity(IntPtr handle,uint information,byte[] descriptor,uint length,out uint required);
 [DllImport("advapi32.dll",SetLastError=true)] static extern bool SetKernelObjectSecurity(IntPtr handle,uint information,byte[] descriptor);
 public sealed class Peer {
  public string Sid { get; private set; }
  public bool Elevated { get; private set; }
  public bool Administrator { get; private set; }
  public bool Local { get; private set; }
  public int Integrity { get; private set; }
  private Peer() {}
  internal static Peer Capture(WindowsIdentity identity) {
   return new Peer{Sid=identity.User.Value,Elevated=ReadInt(identity.Token,20)!=0,
    Administrator=new WindowsPrincipal(identity).IsInRole(WindowsBuiltInRole.Administrator),
    Local=true,Integrity=IntegrityLevel(identity.Token)};
  }
 }
 static void Check(bool good) { if(!good)throw new Win32Exception(Marshal.GetLastWin32Error()); }
 static IntPtr Information(IntPtr token,int kind) {
  int size;GetTokenInformation(token,kind,IntPtr.Zero,0,out size);
  if(size<=0 || size>65536)throw new InvalidOperationException("TOKEN_SIZE_HOLD");
  IntPtr memory=Marshal.AllocHGlobal(size);
  if(!GetTokenInformation(token,kind,memory,size,out size)){Marshal.FreeHGlobal(memory);throw new Win32Exception(Marshal.GetLastWin32Error());}
  return memory;
 }
 static int ReadInt(IntPtr token,int kind) {IntPtr memory=Information(token,kind);try{return Marshal.ReadInt32(memory);}finally{Marshal.FreeHGlobal(memory);}}
 static int IntegrityLevel(IntPtr token) {
  IntPtr memory=Information(token,25);
  try { var sid=new SecurityIdentifier(Marshal.ReadIntPtr(memory));var parts=sid.Value.Split('-');return int.Parse(parts[parts.Length-1]); }
  finally{Marshal.FreeHGlobal(memory);}
 }
 public static Peer Current() {using(var identity=WindowsIdentity.GetCurrent()){return Peer.Capture(identity);}}
 public static int[] CurrentLogon() {
  using(var identity=WindowsIdentity.GetCurrent()) {
   IntPtr stats=Information(identity.Token,10),data=IntPtr.Zero;
   try {
    var value=Marshal.PtrToStructure<Stats>(stats);
    uint status=LsaGetLogonSessionData(ref value.AuthenticationId,out data);
    if(status!=0)throw new InvalidOperationException("LSA_LOGON_QUERY_HOLD");
    var logon=Marshal.PtrToStructure<Logon>(data);
    return new int[]{(int)logon.Type,(int)logon.Session};
   } finally {if(data!=IntPtr.Zero)LsaFreeReturnBuffer(data);Marshal.FreeHGlobal(stats);}
  }
 }
 public static bool CanArm(Peer peer,string allowedSid) {
  return peer!=null && peer.Local && peer.Sid==allowedSid && peer.Elevated && peer.Administrator && peer.Integrity>=12288;
 }
 static void AddQueryOnly(IntPtr handle,string sid,int mask) {
  uint size;GetKernelObjectSecurity(handle,4,null,0,out size);
  if(size<1 || size>65536)throw new UnauthorizedAccessException("QUERY_DACL_SIZE_HOLD");
  var bytes=new byte[size];Check(GetKernelObjectSecurity(handle,4,bytes,size,out size));
  var descriptor=new RawSecurityDescriptor(bytes,0);
  if(descriptor.DiscretionaryAcl==null)throw new UnauthorizedAccessException("QUERY_DACL_HOLD");
  descriptor.DiscretionaryAcl.InsertAce(descriptor.DiscretionaryAcl.Count,new CommonAce(AceFlags.None,AceQualifier.AccessAllowed,mask,new SecurityIdentifier(sid),false,null));
  var output=new byte[descriptor.BinaryLength];descriptor.GetBinaryForm(output,0);Check(SetKernelObjectSecurity(handle,4,output));
 }
 public static void GrantIdentityQueryForBot(string botSid,string serviceSid) {
  var current=Current();
  if(current.Sid!=serviceSid || botSid==serviceSid || current.Administrator || current.Elevated)
   throw new UnauthorizedAccessException("QUERY_SERVICE_CONTEXT_HOLD");
  var logon=CurrentLogon();if(logon[0]!=4 || logon[1]!=0)throw new UnauthorizedAccessException("QUERY_LOGON_HOLD");
  // 새 Host 자신에게만 QueryLimitedInformation / TokenQuery를 줍니다. 기존 프로세스·계정·파일 ACL은 수정하지 않습니다.
  IntPtr process=GetCurrentProcess(),token=IntPtr.Zero;
  AddQueryOnly(process,botSid,0x1000);
  Check(OpenProcessToken(process,0x60008,out token));
  try{AddQueryOnly(token,botSid,8);}finally{CloseHandle(token);}
 }
 public static void VerifyProtectedPath(string path,bool mutableState,string serviceSid) {
  string root=mutableState?LimitlessV2Contract.StateRoot:LimitlessV2Contract.CodeRoot;
  string full=Path.GetFullPath(path);
  LimitlessV2Contract.Need(LimitlessV2Contract.Inside(full,root),"V2_PROTECTED_ROOT_HOLD");
  // root 내부의 State writer는 별도 규칙으로 검사하고, 외곽 부모만 공통 검사합니다.
  LimitlessV2Publish.Ancestors(Path.GetDirectoryName(root));
  for(string p=full;LimitlessV2Contract.Inside(p,root);p=Path.GetDirectoryName(p)) {
   var attr=File.GetAttributes(p);LimitlessV2Contract.Need((attr&FileAttributes.ReparsePoint)==0,"V2_REPARSE_HOLD");
   FileSystemSecurity acl=(attr&FileAttributes.Directory)!=0?new DirectoryInfo(p).GetAccessControl():new FileInfo(p).GetAccessControl();
   var trusted=new HashSet<string>{"S-1-5-18","S-1-5-32-544"};if(mutableState)trusted.Add(serviceSid);
   LimitlessV2Contract.Need(acl.GetOwner(typeof(SecurityIdentifier)).Value=="S-1-5-32-544" || (mutableState&&!p.Equals(root,StringComparison.OrdinalIgnoreCase)&&acl.GetOwner(typeof(SecurityIdentifier)).Value==serviceSid),"V2_PATH_OWNER_HOLD");
   foreach(FileSystemAccessRule r in acl.GetAccessRules(true,true,typeof(SecurityIdentifier))) {
    if((r.PropagationFlags&PropagationFlags.InheritOnly)!=0)continue;
    var mask=FileSystemRights.WriteData|FileSystemRights.AppendData|FileSystemRights.Delete|FileSystemRights.DeleteSubdirectoriesAndFiles|FileSystemRights.ChangePermissions|FileSystemRights.TakeOwnership;
    LimitlessV2Contract.Need(r.AccessControlType!=AccessControlType.Allow||trusted.Contains(r.IdentityReference.Value)||(r.FileSystemRights&mask)==0,"V2_PATH_WRITER_HOLD");
   }
   if(p.Equals(root,StringComparison.OrdinalIgnoreCase))break;
  }
 }
 public static NamedPipeServerStream CreateServicePipe(string name,string botSid,string serviceSid) {
  // 실제 서비스 계정의 별도 프로세스만 서버를 만들 수 있습니다. 기존 task/ACL은 수정하지 않습니다.
  var service=Current();
  if(service.Sid!=serviceSid || serviceSid==botSid || service.Administrator || System.Diagnostics.Process.GetCurrentProcess().SessionId!=0)
   throw new UnauthorizedAccessException("SERVICE_CONTEXT_HOLD");
  var logon=CurrentLogon();if(logon[0]!=4 || logon[1]!=0)throw new UnauthorizedAccessException("SESSION0_LOGON4_HOLD");
  LimitlessV2Contract.Pipe(name,true);
  new SecurityIdentifier(botSid);new SecurityIdentifier(serviceSid);
  IntPtr descriptor;uint size;
  // 클라이언트에는 create-instance 권한을 주지 않습니다. OS가 원격 연결을 거부합니다.
  string sddl="O:"+serviceSid+"G:SYD:P(A;;GA;;;SY)(A;;GA;;;"+serviceSid+")(A;;0x120003;;;"+botSid+")(A;;0x120003;;;BA)";
  Check(ConvertStringSecurityDescriptorToSecurityDescriptor(sddl,1,out descriptor,out size));
  try {
   var security=new Security{Length=Marshal.SizeOf<Security>(),Descriptor=descriptor,Inherit=0};
   // duplex / overlapped / FIRST_PIPE_INSTANCE; message byte mode + REJECT_REMOTE_CLIENTS.
   var handle=CreateNamedPipe(@"\\.\pipe\"+name,3|0x40000000|0x00080000,8,1,16384,16384,0,ref security);
   if(handle.IsInvalid){handle.Dispose();throw new Win32Exception(Marshal.GetLastWin32Error());}
   return new NamedPipeServerStream(PipeDirection.InOut,true,false,handle);
  } finally {LocalFree(descriptor);}
 }
 public static Peer ReadConnectedPeer(NamedPipeServerStream pipe) {
  uint pid;Check(GetNamedPipeClientProcessId(pipe.SafePipeHandle,out pid));
  if(pid==0)throw new UnauthorizedAccessException("LOCAL_PEER_HOLD");
  Peer result=null;
  pipe.RunAsClient(()=>{using(var identity=WindowsIdentity.GetCurrent(true)){
   if(identity==null || identity.ImpersonationLevel<TokenImpersonationLevel.Identification)throw new UnauthorizedAccessException("PEER_TOKEN_HOLD");
   result=Peer.Capture(identity);
  }});
  return result;
 }
 public static void VerifyServer(NamedPipeClientStream pipe,string serviceSid) {
  uint pid;Check(GetNamedPipeServerProcessId(pipe.SafePipeHandle,out pid));
  IntPtr process=OpenProcess(0x1000,false,pid),token=IntPtr.Zero;
  Check(process!=IntPtr.Zero);
  try {
   Check(OpenProcessToken(process,8,out token));
   using(var identity=new WindowsIdentity(token)) {
    if(identity.User.Value!=serviceSid || ReadInt(token,12)!=0 || new WindowsPrincipal(identity).IsInRole(WindowsBuiltInRole.Administrator))
     throw new UnauthorizedAccessException("PIPE_SERVER_IDENTITY_HOLD");
   }
  } finally {if(token!=IntPtr.Zero)CloseHandle(token);CloseHandle(process);}
 }
 public static NamedPipeClientStream Connect(string name,string serviceSid) {
  LimitlessV2Contract.Pipe(name,true);
  // 제한된 read/write + SQOS Identification. 파일/네트워크 경로는 받지 않습니다. Busy도 재시도하지 않습니다.
  var handle=CreateFile(@"\\.\pipe\"+name,0x120003,0,IntPtr.Zero,3,0x40000000|0x00100000|0x00010000,IntPtr.Zero);
  if(handle.IsInvalid){handle.Dispose();throw new Win32Exception(Marshal.GetLastWin32Error());}
  var pipe=new NamedPipeClientStream(PipeDirection.InOut,true,true,handle);
  try {VerifyServer(pipe,serviceSid);return pipe;}catch{pipe.Dispose();throw;}
 }
 static async Task Exact(Stream stream,byte[] data,CancellationToken cancel) {
  int offset=0;while(offset<data.Length){int read=await stream.ReadAsync(data,offset,data.Length-offset,cancel);if(read==0)throw new EndOfStreamException();offset+=read;}
 }
 public static async Task<byte[]> ReadFrame(Stream stream,CancellationToken cancel) {
  var header=new byte[4];await Exact(stream,header,cancel);
  int size=BitConverter.ToInt32(header,0);if(size<2 || size>16384)throw new InvalidDataException("FRAME_SIZE_HOLD");
  var data=new byte[size];await Exact(stream,data,cancel);return data;
 }
 public static async Task WriteFrame(Stream stream,byte[] data,CancellationToken cancel) {
  if(data==null || data.Length<2 || data.Length>16384)throw new InvalidDataException("FRAME_SIZE_HOLD");
  var header=BitConverter.GetBytes(data.Length);await stream.WriteAsync(header,0,4,cancel);await stream.WriteAsync(data,0,data.Length,cancel);await stream.FlushAsync(cancel);
 }
 public static async Task ServeChannel(string name,string kind,string botSid,string adminSid,string serviceSid,
   Func<Peer,byte[],CancellationToken,Task<byte[]>> handler,CancellationToken stop) {
  if(kind!="arm" && kind!="work" && kind!="control")throw new ArgumentException("CHANNEL_HOLD");
  // UAC 분할 토큰만을 서로 다른 보안 주체로 취급하지 않습니다.
  if(botSid==adminSid || botSid==serviceSid || adminSid==serviceSid)throw new UnauthorizedAccessException("DISTINCT_ACCOUNT_SIDS_REQUIRED");
  using(var pipe=CreateServicePipe(name+"-"+kind,botSid,serviceSid)) {
   while(!stop.IsCancellationRequested) {
    try {
     await pipe.WaitForConnectionAsync(stop);
     using(var frameLimit=CancellationTokenSource.CreateLinkedTokenSource(stop)) {
      frameLimit.CancelAfter(2000);
      var frame=await ReadFrame(pipe,frameLimit.Token);
      var peer=ReadConnectedPeer(pipe);
      bool permitted=kind=="arm" ? CanArm(peer,adminSid) :
       peer.Local && peer.Sid==botSid && !peer.Elevated && !peer.Administrator;
      if(!permitted)throw new UnauthorizedAccessException("PEER_PERMISSION_HOLD");
      using(var json=System.Text.Json.JsonDocument.Parse(frame)) {
       string op=json.RootElement.GetProperty("op").GetString();
       if((kind=="arm" && op!="arm") || (kind=="work" && op!="submit") ||
         (kind=="control" && op!="cancel" && op!="result"))throw new InvalidDataException("CHANNEL_OPERATION_HOLD");
      }
      // 실행/취소는 별도 channel이라 실행 중에도 취소가 접수됩니다. handler는 고정 설치 코드만 제공합니다.
      using(var requestLimit=CancellationTokenSource.CreateLinkedTokenSource(stop)) {
       requestLimit.CancelAfter(kind=="work" ? 90000 : 2000);
       var response=await handler(peer,frame,requestLimit.Token);
       await WriteFrame(pipe,response,requestLimit.Token);
      }
     }
    } catch(OperationCanceledException) { if(stop.IsCancellationRequested)break; }
      catch { /* 자유문 예외/토큰/서비스 경로는 pipe에 보내지 않습니다. 연결 종료는 클라이언트 HOLD입니다. */ }
    finally {if(pipe.IsConnected)pipe.Disconnect();}
   }
  }
 }
}

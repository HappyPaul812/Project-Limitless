// 고정 Host와 Python 중개기를 익명 파이프로 연결합니다. wire 입력으로 실행 경로를 받지 않습니다.
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

public sealed class LimitlessSession0HostV1 {
 readonly string root,bot,admin,worker,python,entry;
 readonly CancellationTokenSource stop=new CancellationTokenSource();
 public LimitlessSession0HostV1(string install,string botSid,string adminSid,string workerSid) {
  root=Path.GetFullPath(install);bot=botSid;admin=adminSid;worker=workerSid;
  python=Path.Combine(root,"Runtime","python.exe");entry=Path.Combine(root,"Code","Tools","DiscordAIServiceDiagnostics","broker-host-entry.py");
 }
 byte[] Gateway(LimitlessSession0PipeV1.Peer peer,byte[] raw,string action,CancellationToken cancel,int budget) {
  var envelope=JsonSerializer.SerializeToUtf8Bytes(new {peer=peer,message=Convert.ToBase64String(raw)});
  var result=LimitlessBrokerJob90V1.RunInput(python,new[]{"-I","-B","-X","utf8",entry,"--action",action},root,budget,cancel,envelope);
  if(result.ExitCode!=0 || result.Cancelled || result.TimedOut || !result.CleanupVerified || result.ActiveProcesses!=0 || result.StdoutTruncated || result.StderrTruncated)
   return Encoding.UTF8.GetBytes("{\"status\":\"HOLD\",\"reason\":\"HOST_GATEWAY_HOLD_NO_RETRY\"}");
  var bytes=Encoding.UTF8.GetBytes(result.Stdout??"");
  if(bytes.Length<2 || bytes.Length>16384)throw new InvalidDataException("HOST_REPLY_HOLD");
  return bytes;
 }
 public void Run(string pipeName,int lifetimeSeconds) {
  if(lifetimeSeconds<1 || lifetimeSeconds>900 || bot==admin || bot==worker || admin==worker)throw new InvalidOperationException("HOST_POLICY_HOLD");
  LimitlessSession0PipeV1.VerifyProtectedPath(root,false,worker);
  LimitlessSession0PipeV1.VerifyProtectedPath(python,false,worker);
  LimitlessSession0PipeV1.VerifyProtectedPath(entry,false,worker);
  var me=LimitlessSession0PipeV1.Current();var logon=LimitlessSession0PipeV1.CurrentLogon();
  if(me.Sid!=worker || me.Administrator || me.Elevated || logon[0]!=4 || logon[1]!=0)throw new UnauthorizedAccessException("HOST_SERVICE_CONTEXT_HOLD");
  LimitlessSession0PipeV1.GrantIdentityQueryForBot(bot,worker);
  // startup에서만 불명 요청/미사용 arm을 폐기합니다. 개별 RPC subprocess는 다시 복구하지 않습니다.
  var init=Gateway(null,Encoding.UTF8.GetBytes("{}"),"initialize",stop.Token,10000);
  using(var value=JsonDocument.Parse(init)){if(value.RootElement.GetProperty("status").GetString()!="READY")throw new InvalidOperationException("HOST_INIT_HOLD");}
  stop.CancelAfter(TimeSpan.FromSeconds(lifetimeSeconds));
  Func<LimitlessSession0PipeV1.Peer,byte[],CancellationToken,Task<byte[]>> callback=(peer,raw,cancel)=>Task.Run(()=> {
   var watch=Stopwatch.StartNew();
   using(var doc=JsonDocument.Parse(raw)) {
    string op=doc.RootElement.GetProperty("op").GetString();int budget=5000;
    if(op=="submit") {
     double until=doc.RootElement.GetProperty("ticket").GetProperty("deadline").GetDouble();
     budget=(int)Math.Min(85000,Math.Max(1,(until-DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()/1000.0)*1000-2000-watch.ElapsedMilliseconds));
    }
    return Gateway(peer,raw,"dispatch",cancel,budget);
   }
  },cancel);
  var tasks=new[]{LimitlessSession0PipeV1.ServeChannel(pipeName,"arm",bot,admin,worker,callback,stop.Token),
   LimitlessSession0PipeV1.ServeChannel(pipeName,"work",bot,admin,worker,callback,stop.Token),
   LimitlessSession0PipeV1.ServeChannel(pipeName,"control",bot,admin,worker,callback,stop.Token)};
  try {Task.WhenAny(tasks).GetAwaiter().GetResult();}finally{stop.Cancel();}
  Task.WhenAll(tasks).GetAwaiter().GetResult();
 }
 public void Stop(){stop.Cancel();}
}

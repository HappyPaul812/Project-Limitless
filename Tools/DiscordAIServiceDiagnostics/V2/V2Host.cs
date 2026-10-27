// 네이티브 인증 Pipe와 새 FAKE_ONLY Python runtime의 고정 접합입니다. 실행 경로는 입력으로 받지 않습니다.
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
public sealed class LimitlessV2Host {
 readonly string bot,admin,worker,pipe,manifestSha,bindingSha;
 readonly CancellationTokenSource stop=new CancellationTokenSource();
 public LimitlessV2Host(string botSid,string adminSid,string workerSid,string pipeName,string ms,string bs){LimitlessV2Contract.Distinct(botSid,workerSid,adminSid);LimitlessV2Contract.Pipe(pipeName,false);bot=botSid;admin=adminSid;worker=workerSid;pipe=pipeName;manifestSha=ms;bindingSha=bs;}
 byte[] Gateway(LimitlessSession0PipeV2.Peer peer,byte[] raw,string action,CancellationToken cancel,int budget){
  if(!LimitlessV2WorkerFence.Clear(worker,System.Diagnostics.Process.GetCurrentProcess().Id))throw new InvalidOperationException("V2_SHARED_WORKER_HOLD");
  using(var doc=JsonDocument.Parse(raw)){
   byte[] input=JsonSerializer.SerializeToUtf8Bytes(new {peer=peer,message=doc.RootElement});
   var r=LimitlessBrokerJob90V1.RunInput(Path.Combine(LimitlessV2Contract.CodeRoot,"Runtime","python.exe"),new[]{"-I","-B","-X","utf8",Path.Combine(LimitlessV2Contract.CodeRoot,"Code","V2","entry-v2.py"),"--action",action,"--manifest-sha",manifestSha,"--binding-sha",bindingSha},LimitlessV2Contract.CodeRoot,budget,cancel,input);
   if(r.ExitCode!=0||r.TimedOut||r.Cancelled||!r.CleanupVerified||r.ActiveProcesses!=0||r.StdoutTruncated||r.StderrTruncated)throw new InvalidOperationException("V2_GATEWAY_JOB_HOLD");
   var bytes=Encoding.UTF8.GetBytes(r.Stdout??"");if(bytes.Length<2||bytes.Length>16384)throw new InvalidDataException("V2_GATEWAY_SIZE_HOLD");return bytes;
  }
 }
 public void Run(int lifetimeMs){
  LimitlessV2Contract.Need(lifetimeMs>0&&lifetimeMs<=90000,"V2_HOST_LIFETIME_HOLD");
  var me=LimitlessSession0PipeV2.Current();var logon=LimitlessSession0PipeV2.CurrentLogon();
  LimitlessV2Contract.Need(me.Sid==worker&&!me.Administrator&&!me.Elevated&&logon[0]==4&&logon[1]==0,"V2_HOST_TOKEN_HOLD");
  LimitlessSession0PipeV2.GrantIdentityQueryForBot(bot,worker);
  stop.CancelAfter(lifetimeMs);
  using(var monitor=new Timer(_=>{if(!LimitlessV2WorkerFence.Clear(worker,System.Diagnostics.Process.GetCurrentProcess().Id))stop.Cancel();},null,0,250)){
   using(var init=JsonDocument.Parse(Gateway(null,Encoding.UTF8.GetBytes("{}"),"initialize",stop.Token,10000))){LimitlessV2Contract.Need(init.RootElement.GetProperty("status").GetString()=="READY_FAKE_ONLY","V2_INIT_HOLD");}
   Func<LimitlessSession0PipeV2.Peer,byte[],CancellationToken,Task<byte[]>> callback=(peer,raw,cancel)=>Task.Run(()=>Gateway(peer,raw,"dispatch",cancel,85000),cancel);
   var tasks=new[]{LimitlessSession0PipeV2.ServeChannel(pipe,"arm",bot,admin,worker,callback,stop.Token),LimitlessSession0PipeV2.ServeChannel(pipe,"work",bot,admin,worker,callback,stop.Token),LimitlessSession0PipeV2.ServeChannel(pipe,"control",bot,admin,worker,callback,stop.Token)};
   try{Task.WhenAny(tasks).GetAwaiter().GetResult();}finally{stop.Cancel();}
   Task.WhenAll(tasks).GetAwaiter().GetResult();
  }
 }
 public void Stop(){stop.Cancel();}
}

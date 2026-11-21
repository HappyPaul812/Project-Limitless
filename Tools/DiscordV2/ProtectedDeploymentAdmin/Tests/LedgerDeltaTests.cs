using System.Collections.Concurrent;
using System.Diagnostics;
using System.Security.AccessControl;

internal static partial class DeploymentAdminTests
{
    // 원장/보안 descriptor는 합성입니다. 실제 보호 경로와 Windows backend를 사용하지 않습니다.
    private sealed class MemoryLedger(ConcurrentDictionary<string,MemoryStream> records):IDeploymentLedgerIo
    {
        internal bool BadProtection,FailFlush;
        internal int Creates;
        public void PinAndValidate()
        {
            DeploymentLedgerPolicy.Security(new RawSecurityDescriptor(BadProtection
                ?DeploymentLedgerPolicy.Sddl.Replace("O:BA","O:"+Native.Worker):DeploymentLedgerPolicy.Sddl));
        }
        public Stream CreateNew(string path)
        {
            var file=FailFlush?new LedgerFailFlush():new MemoryStream();
            if(!records.TryAdd(path,file)){file.Dispose();throw new IOException("SYNTHETIC_CREATE_NEW_COLLISION");}
            Creates++;return file;
        }
    }
    private sealed class LedgerFailFlush:MemoryStream
    {public override void Flush()=>throw new IOException("SYNTHETIC_FLUSH_FAILED");}
    private static DeploymentAdmission Admission(Fixture f)
    {byte[] raw=f.Raw();return new DeploymentAdmission(raw,Hash(raw),Binding,f.Host);}
    private static void ConsumeRequest(Fixture f,string requestPath,MemoryLedger io)
    {
        f.Host.Inputs[requestPath]=f.Raw();byte[] raw=f.Host.ReadProtected(requestPath,8192);
        var a=new DeploymentAdmission(raw,Hash(raw),Binding,f.Host);
        DeploymentLedger.Consume(f.Host.LedgerRoot,a.Request.DeploymentId,a.Request.ManifestSha256,io,a.Validate);
    }
    private static readonly string QaRoot=Path.Combine(AppContext.BaseDirectory,"SyntheticLedgerQA");
    private sealed class QaFileLedger(string root):IDeploymentLedgerIo
    {
        public void PinAndValidate()
        {
            // 테스트 DLL의 출력 하위 합성 폴더만 허용합니다. 운영 원장/보호 경로로 전환할 수 없습니다.
            Check(Path.GetFullPath(root)==QaRoot&&Directory.Exists(root));
            DeploymentLedgerPolicy.Security(new(DeploymentLedgerPolicy.Sddl));
        }
        public Stream CreateNew(string path)
        {
            Check(Path.GetDirectoryName(path)==QaRoot);
            return new FileStream(path,FileMode.CreateNew,FileAccess.ReadWrite,FileShare.Read,4096,FileOptions.WriteThrough);
        }
    }
    private static int LedgerPeer(string[] args)
    {
        if(args.Length!=3||Path.GetFullPath(args[1])!=QaRoot)return 70;
        try{DeploymentLedger.Consume(QaRoot,args[2],new('a',64),new QaFileLedger(QaRoot),()=>{});return 0;}
        catch(IOException e) when((e.HResult&0xffff) is 80 or 183){return 23;}
        catch{return 70;}
    }
    private static int Peer(string id)
    {
        var start=new ProcessStartInfo(@"C:\Program Files\dotnet\dotnet.exe") {UseShellExecute=false,CreateNoWindow=true,WindowStyle=ProcessWindowStyle.Hidden};
        start.ArgumentList.Add(typeof(DeploymentAdminTests).Assembly.Location);
        start.ArgumentList.Add("--ledger-peer");start.ArgumentList.Add(QaRoot);start.ArgumentList.Add(id);
        using var process=Process.Start(start)??throw new Exception("TEST_PEER_NOT_STARTED");
        Check(process.WaitForExit(10000));return process.ExitCode;
    }
    private static int LedgerDelta()
    {
        Test("cs0236-clock-uses-outer-fixed-time",()=>Check(new Fake().Clock==DeploymentAdminTests.Now));
        Test("same-id-different-request-folders-denied-existing-bytes-unchanged",()=>{
            var f=new Fixture();var records=new ConcurrentDictionary<string,MemoryStream>();var first=new MemoryLedger(records);
            ConsumeRequest(f,@"X:\RequestsA\request.json",first);string key=DeploymentLedgerPolicy.RecordPath(f.Host.LedgerRoot,f.Request.DeploymentId);
            byte[] before=records[key].ToArray();Deny(()=>ConsumeRequest(new Fixture(),@"X:\RequestsB\request.json",new(records)));
            Check(records.Count==1&&records[key].ToArray().SequenceEqual(before));
        });
        Test("unique-deployment-id-uses-distinct-record",()=>{
            var records=new ConcurrentDictionary<string,MemoryStream>();var a=new Fixture();ConsumeRequest(a,@"X:\A\request",new(records));
            var b=new Fixture();b.Request.DeploymentId=b.Approval.DeploymentId="22222222222222222222222222222222";b.Seal();ConsumeRequest(b,@"X:\B\request",new(records));Check(records.Count==2);
        });
        Test("failure-after-create-keeps-consumed-marker-and-denies-reuse",()=>{
            var f=new Fixture();var records=new ConcurrentDictionary<string,MemoryStream>();var io=new MemoryLedger(records);var a=Admission(f);int checks=0;
            Deny(()=>DeploymentLedger.Consume(f.Host.LedgerRoot,f.Request.DeploymentId,f.Request.ManifestSha256,io,()=>{a.Validate();if(++checks==2)throw new IOException("SYNTHETIC_AFTER_CREATE_FAILURE");}));
            Check(records.Count==1&&records.Single().Value.ToArray().Length==0);Deny(()=>ConsumeRequest(new Fixture(),@"X:\OTHER\request",new(records)));Check(records.Count==1);
        });
        Test("flush-failure-keeps-original-record-immutable",()=>{
            var f=new Fixture();var records=new ConcurrentDictionary<string,MemoryStream>();var io=new MemoryLedger(records){FailFlush=true};Deny(()=>ConsumeRequest(f,@"X:\A\request",io));
            byte[] before=records.Single().Value.ToArray();Check(before.Length>0);Deny(()=>ConsumeRequest(new Fixture(),@"X:\B\request",new(records)));Check(records.Single().Value.ToArray().SequenceEqual(before));
        });
        foreach(int kind in new[]{0,1,2,3})Test("expiry-tamper-nonadmin-impersonation-denied-"+kind,()=>{
            var f=new Fixture();var records=new ConcurrentDictionary<string,MemoryStream>();
            if(kind==0)f.Host.Clock=f.Request.ExpiresUtc;
            if(kind==1)f.Request.ManifestSha256=new('e',64);
            if(kind==2)f.Host.Actor=f.Host.Actor with{ElevatedAdministrator=false};
            if(kind==3)f.Host.Actor=f.Host.Actor with{NotImpersonating=false};
            Deny(()=>ConsumeRequest(f,@"X:\Request\request",new(records)));Check(records.IsEmpty);
        });
        Test("fixed-production-binding-rejects-alternate-root",()=>{
            Check(DeploymentLedgerBinding.Root==@"C:\ProgramData\Limitless\DiscordV2\Control\DeploymentLedger");Deny(()=>DeploymentLedgerPolicy.Root(@"X:\GuessedLedger",DeploymentLedgerBinding.Root,Binding));
        });
        foreach(string root in new[]{@"X:\OtherLedger",@"\\server\share\Ledger",@"X:\FixedAdminLedger\..\Redirect",Native.CodeRoot})
            Test("ledger-redirect-denied-"+root,()=>Deny(()=>DeploymentLedgerPolicy.Root(root,@"X:\FixedAdminLedger",Binding)));
        Test("weak-ledger-protection-blocks-before-create",()=>{
            var records=new ConcurrentDictionary<string,MemoryStream>();var f=new Fixture();var a=Admission(f);var io=new MemoryLedger(records){BadProtection=true};
            Deny(()=>DeploymentLedger.Consume(f.Host.LedgerRoot,f.Request.DeploymentId,f.Request.ManifestSha256,io,a.Validate));Check(records.IsEmpty&&io.Creates==0);
        });
        foreach(var change in new (string,Func<string,string>)[]{("unprotected",s=>s.Replace("D:P","D:")),("worker-ace",s=>s+"(A;;FR;;;"+Native.Worker+")"),("wrong-group",s=>s.Replace("G:SY","G:BA")),("inherited",s=>s.Replace("A;;FA;;;BA","A;ID;FA;;;BA")),("missing-admin",s=>s.Replace("(A;;FA;;;BA)",""))})
            Test("ledger-owner-dacl-denial-"+change.Item1,()=>Deny(()=>DeploymentLedgerPolicy.Security(new(change.Item2(DeploymentLedgerPolicy.Sddl)))));
        Test("ledger-admin-system-full-only-accepted",()=>{
            DeploymentLedgerPolicy.Security(new(DeploymentLedgerPolicy.Sddl));
            DeploymentLedgerPolicy.Security(new(DeploymentLedgerPolicy.Sddl.Replace("O:BA","O:SY")));
        });
        Test("same-id-concurrent-create-new-has-one-winner",()=>{
            var records=new ConcurrentDictionary<string,MemoryStream>();int wins=0,rejected=0;
            Parallel.For(0,2,_=>{try{DeploymentLedger.Consume(@"X:\FixedAdminLedger","33333333333333333333333333333333",new('a',64),new MemoryLedger(records),()=>{});Interlocked.Increment(ref wins);}catch(IOException){Interlocked.Increment(ref rejected);}});
            Check(wins==1&&rejected==1&&records.Count==1);
        });
        Test("real-managed-test-peer-same-id-denied-new-id-accepted-existing-sha-unchanged",()=>{
            Directory.CreateDirectory(QaRoot);string id=Guid.NewGuid().ToString("N"),path=DeploymentLedgerPolicy.RecordPath(QaRoot,id);
            DeploymentLedger.Consume(QaRoot,id,new('a',64),new QaFileLedger(QaRoot),()=>{});byte[] before=File.ReadAllBytes(path);
            Check(Peer(id)==23&&File.ReadAllBytes(path).SequenceEqual(before));string fresh=Guid.NewGuid().ToString("N");Check(Peer(fresh)==0&&File.Exists(DeploymentLedgerPolicy.RecordPath(QaRoot,fresh)));
        });
        Test("existing-marker-not-reset-after-managed-peer-restart",()=>{
            Directory.CreateDirectory(QaRoot);string id=Guid.NewGuid().ToString("N");Check(Peer(id)==0);string path=DeploymentLedgerPolicy.RecordPath(QaRoot,id);byte[] before=File.ReadAllBytes(path);
            Check(Peer(id)==23&&Peer(id)==23&&File.ReadAllBytes(path).SequenceEqual(before));
        });
        Console.WriteLine($"FIXED_LEDGER_DELTA: {pass} PASS / {fail} FAIL; TOOL_MAIN=0; WINDOWS_BACKEND=0; PROTECTED_WINDOWS_MUTATIONS=0; TOOL_BUILDS=0; OLD_SUITES=0");
        Console.WriteLine("PRODUCTION_LEDGER_LOCATION_UNAPPROVED_HOLD; synthetic workspace QA files only; protected root/ACL unchanged");
        return fail==0?0:1;
    }
}

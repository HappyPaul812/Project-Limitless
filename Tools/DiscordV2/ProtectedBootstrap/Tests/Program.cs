using System.Security.AccessControl;
using System.Text;
using System.Text.Json;

// WindowsBootstrapHost/제품 Main은 실행하지 않습니다. 모든 디렉터리와 token은 메모리 모형입니다.
internal static class BootstrapTests
{
    private static readonly DateTimeOffset Now=new(2026,10,31,10,0,0,TimeSpan.Zero);
    private const string User="S-1-5-21-1-2-3-1001";
    private static int passed,failed;
    private static void Check(bool ok){if(!ok)throw new Exception("ASSERT");}
    private static void Deny(Action action){try{action();}catch{return;}throw new Exception("EXPECTED_DENIAL");}
    private static void Test(string name,Action action){try{action();passed++;Console.WriteLine("PASS "+name);}catch(Exception e){failed++;Console.WriteLine("FAIL "+name+" "+e.GetType().Name+":"+e.Message);}}
    private static byte[] Sd(string text){var sd=new RawSecurityDescriptor(text);byte[] bytes=new byte[sd.BinaryLength];sd.GetBinaryForm(bytes,0);return bytes;}
    private sealed class Fake:IBootstrapHost
    {
        internal readonly Dictionary<string,BootstrapNode> Nodes=[];
        internal BootstrapActor Actor=new(true,true,false,User,new('a',64));
        internal DateTimeOffset Clock=BootstrapTests.Now;
        internal bool Ntfs=true;
        internal int Creates,NativeAttempts,Reads,FailCreateAt;
        internal Action<string>? BeforeCreate,AfterCreate,OnRead;
        private BootstrapWriteGate? gate;
        public DateTimeOffset Now=>Clock;
        public bool FixedNtfsVolume=>Ntfs;
        internal Fake()
        {
            foreach(string path in BootstrapPolicy.Ancestors)
                Nodes.Add(path,new(path,"v1:"+path,1,true,false,0,Sd("O:SYG:SYD:P(A;;FA;;;SY)(A;;FA;;;BA)(A;;0x1200a9;;;BU)")));
        }
        public BootstrapActor ObserveActor(){return Actor;}
        public bool Exists(string path)=>Nodes.ContainsKey(path);
        public BootstrapNode PinAndRead(string path){Reads++;OnRead?.Invoke(path);return Nodes.TryGetValue(path,out var node)?node:throw new IOException("PIN_NOT_FOUND");}
        public void Bind(BootstrapWriteGate value){if(gate is not null)throw new IOException("BOUND");gate=value;}
        public void CreateProtected(string path,string sddl)
        {
            BeforeCreate?.Invoke(path);(gate??throw new IOException("GATE_MISSING")).BeforeNativeCreate(path,sddl);
            NativeAttempts++;if(FailCreateAt==NativeAttempts)throw new IOException("SYNTHETIC_CREATE_FAILED");
            if(!Nodes.TryAdd(path,new(path,"v1:new"+NativeAttempts,1,true,false,0,Sd(sddl))))throw new IOException("CREATE_NEW_COLLISION");
            Creates++;AfterCreate?.Invoke(path);
        }
        public void Dispose(){ }
    }
    private sealed class Fixture
    {
        internal readonly Fake Host=new();
        internal readonly BootstrapAuthorization Auth;
        internal Fixture()
        {
            Auth=new(){Approved=true,Profile="DISCORD_V2_PROTECTED_BOOTSTRAP_V1",ApprovalId="11111111111111111111111111111111",
                ToolSha256=Host.Actor.ToolSha256,AdministratorSid=User,OwnerSid=BootstrapPolicy.AdminSid,ExpiresUtc=Now.AddHours(1),Targets=BootstrapPolicy.Targets.ToArray(),
                AncestorSecuritySha256=BootstrapPolicy.Ancestors.Select(p=>BootstrapPolicy.Hash(Host.Nodes[p].Security)).ToArray(),
                AncestorFileKeys=BootstrapPolicy.Ancestors.Select(p=>Host.Nodes[p].Key).ToArray()};
        }
        internal byte[] Raw()=>JsonSerializer.SerializeToUtf8Bytes(Auth,BootstrapJson.Default.BootstrapAuthorization);
        internal int Run(bool create=true,CancellationToken cancellation=default){byte[] raw=Raw();return BootstrapController.Run(raw,BootstrapPolicy.Hash(raw),Host,create,cancellation);}
        internal void NoWrites()=>Check(Host.Creates==0&&Host.NativeAttempts==0&&Host.Nodes.Count==2);
    }
    static int Main()
    {
        Test("valid-exact-six-initial-descriptor-readback",()=>{var f=new Fixture();Check(f.Run()==6&&f.Host.Creates==6);foreach(string p in BootstrapPolicy.Targets)BootstrapPolicy.AdministrativeSecurity(f.Host.Nodes[p].Security,BootstrapPolicy.AdminSid);});
        Test("review-readonly-no-create-or-bind",()=>{var f=new Fixture();Check(f.Run(false)==0);f.NoWrites();});
        Test("system-owner-explicitly-authorized-model",()=>{var f=new Fixture();f.Auth.OwnerSid=BootstrapPolicy.SystemSid;Check(f.Run()==6);});
        foreach(var change in new (string,Action<BootstrapAuthorization>)[]{("unapproved",a=>a.Approved=false),("profile",a=>a.Profile="OTHER"),("empty-id",a=>a.ApprovalId=new('0',32)),("scope",a=>a.Targets[0]=@"C:\ESCAPE"),("target-order",a=>Array.Reverse(a.Targets)),("extra-target",a=>a.Targets=[..a.Targets,@"C:\ESCAPE"]),("owner",a=>a.OwnerSid=User),("tool-sha",a=>a.ToolSha256=new('b',64)),("admin-sid",a=>a.AdministratorSid="S-1-5-18"),("expired",a=>a.ExpiresUtc=Now),("after-cutoff",a=>a.ExpiresUtc=BootstrapPolicy.Cutoff.AddTicks(1)),("parent-hash",a=>a.AncestorSecuritySha256[0]=new('b',64)),("parent-key",a=>a.AncestorFileKeys[1]="OTHER")})
            Test("deny-before-writes-"+change.Item1,()=>{var f=new Fixture();change.Item2(f.Auth);Deny(()=>f.Run());f.NoWrites();});
        foreach(int kind in new[]{0,1,2})Test("nonadmin-nonelevated-impersonation-denied-"+kind,()=>{var f=new Fixture();f.Host.Actor=kind switch{0=>f.Host.Actor with{Administrator=false},1=>f.Host.Actor with{Elevated=false},_=>f.Host.Actor with{Impersonating=true}};Deny(()=>f.Run());f.NoWrites();});
        Test("cutoff-no-automatic-extension",()=>{var f=new Fixture();f.Host.Clock=BootstrapPolicy.Cutoff.AddTicks(1);Deny(()=>f.Run());f.NoWrites();});
        Test("wrong-raw-sha-denied",()=>{var f=new Fixture();Deny(()=>BootstrapController.Run(f.Raw(),new('b',64),f.Host,true));f.NoWrites();});
        Test("duplicate-json-denied",()=>{var f=new Fixture();byte[] raw=Encoding.UTF8.GetBytes(Encoding.UTF8.GetString(f.Raw()).Insert(1,"\"Approved\":true,"));Deny(()=>BootstrapController.Run(raw,BootstrapPolicy.Hash(raw),f.Host,true));f.NoWrites();});
        foreach(string target in BootstrapPolicy.Targets)Test("preexisting-target-preserved-"+target,()=>{var f=new Fixture();var existing=new BootstrapNode(target,"EXISTING",1,true,false,0,Sd(BootstrapPolicy.Sddl(BootstrapPolicy.AdminSid)));f.Host.Nodes.Add(target,existing);Deny(()=>f.Run());Check(f.Host.Creates==0&&ReferenceEquals(f.Host.Nodes[target],existing));});
        foreach(int kind in new[]{0,1,2,3,4})Test("ancestor-owner-delete-deletechild-reparse-inherit-handle-denied-"+kind,()=>{var f=new Fixture();string p=BootstrapPolicy.Ancestors[1];var n=f.Host.Nodes[p];f.Host.Nodes[p]=kind switch{
            0=>n with{Security=Sd("O:"+User+"G:SYD:P(A;;FA;;;SY)(A;;FA;;;BA)")},
            1=>n with{Security=Sd("O:SYG:SYD:P(A;;FA;;;SY)(A;;FA;;;BA)(A;;SD;;;AU)")},
            2=>n with{Security=Sd("O:SYG:SYD:P(A;;FA;;;SY)(A;;FA;;;BA)(A;;0x40;;;AU)")},
            3=>n with{Reparse=true},_=>n with{HandleFlags=1}};Deny(()=>f.Run());f.NoWrites();});
        Test("parent-swapped-after-preflight-denied",()=>{var f=new Fixture();f.Host.BeforeCreate=_=>{string p=BootstrapPolicy.Ancestors[1];f.Host.Nodes[p]=f.Host.Nodes[p] with{Key="REPLACED"};};Deny(()=>f.Run());f.NoWrites();});
        Test("race-target-appears-before-native-no-adoption",()=>{var f=new Fixture();string p=BootstrapPolicy.Targets[0];f.Host.BeforeCreate=_=>f.Host.Nodes[p]=new(p,"RACER",1,true,false,0,Sd(BootstrapPolicy.Sddl(BootstrapPolicy.AdminSid)));Deny(()=>f.Run());Check(f.Host.NativeAttempts==0&&f.Host.Nodes[p].Key=="RACER");});
        Test("expiry-in-native-boundary-gap-no-create",()=>{var f=new Fixture();f.Host.BeforeCreate=_=>f.Host.Clock=f.Auth.ExpiresUtc;Deny(()=>f.Run());f.NoWrites();});
        Test("expiry-during-metadata-read-no-create",()=>{var f=new Fixture();f.Host.OnRead=_=>f.Host.Clock=f.Auth.ExpiresUtc;Deny(()=>f.Run());f.NoWrites();});
        Test("admin-lost-during-metadata-read-no-create",()=>{var f=new Fixture();f.Host.OnRead=_=>f.Host.Actor=f.Host.Actor with{Elevated=false};Deny(()=>f.Run());f.NoWrites();});
        Test("cancel-during-read-no-create",()=>{var f=new Fixture();using var cancel=new CancellationTokenSource();f.Host.OnRead=_=>cancel.Cancel();Deny(()=>f.Run(cancellation:cancel.Token));f.NoWrites();});
        foreach(int kind in new[]{0,1,2,3,4,5})Test("created-owner-dacl-finalpath-reparse-handle-volume-readback-fails-hold-"+kind,()=>{var f=new Fixture();f.Host.AfterCreate=p=>{var n=f.Host.Nodes[p];f.Host.Nodes[p]=kind switch{
            0=>n with{Security=Sd(BootstrapPolicy.Sddl(BootstrapPolicy.SystemSid))},
            1=>n with{Security=Sd(BootstrapPolicy.Sddl(BootstrapPolicy.AdminSid)+"(A;;FA;;;BU)")},
            2=>n with{Path=@"C:\ESCAPE"},3=>n with{Reparse=true},4=>n with{HandleFlags=1},_=>n with{Volume=2}};};Deny(()=>f.Run());Check(f.Host.Creates==1&&f.Host.Nodes.ContainsKey(BootstrapPolicy.Targets[0]));});
        Test("partial-native-failure-no-delete-no-retry",()=>{var f=new Fixture();f.Host.FailCreateAt=2;Deny(()=>f.Run());Check(f.Host.Creates==1&&f.Host.NativeAttempts==2&&f.Host.Nodes.Count==3);Deny(()=>f.Run());Check(f.Host.NativeAttempts==2&&f.Host.Nodes.Count==3);});
        Test("same-gate-native-attempt-not-retried",()=>{var f=new Fixture();var gate=new BootstrapWriteGate(BootstrapPolicy.Parse(f.Raw(),BootstrapPolicy.Hash(f.Raw())),f.Host,default);gate.Validate();f.Host.Bind(gate);f.Host.FailCreateAt=1;Deny(()=>f.Host.CreateProtected(BootstrapPolicy.Targets[0],gate.Sddl));Deny(()=>f.Host.CreateProtected(BootstrapPolicy.Targets[0],gate.Sddl));Check(f.Host.NativeAttempts==1);});
        Test("created-directory-changed-before-next-create-holds",()=>{var f=new Fixture();f.Host.BeforeCreate=p=>{if(p==BootstrapPolicy.Targets[1]){string previous=BootstrapPolicy.Targets[0];f.Host.Nodes[previous]=f.Host.Nodes[previous] with{Key="CHANGED"};}};Deny(()=>f.Run());Check(f.Host.Creates==1);});
        Test("successful-replay-keeps-all-existing-objects",()=>{var f=new Fixture();f.Run();var before=f.Host.Nodes.ToDictionary(x=>x.Key,x=>x.Value);Deny(()=>f.Run());Check(f.Host.Creates==6&&before.All(x=>ReferenceEquals(x.Value,f.Host.Nodes[x.Key])));});
        Test("unbound-backend-model-denies-no-effects",()=>{using var fake=new Fake();Deny(()=>fake.CreateProtected(BootstrapPolicy.Targets[0],BootstrapPolicy.Sddl(BootstrapPolicy.AdminSid)));Check(fake.NativeAttempts==0);});
        Test("out-of-scope-path-or-security-denied",()=>{var f=new Fixture();var gate=new BootstrapWriteGate(BootstrapPolicy.Parse(f.Raw(),BootstrapPolicy.Hash(f.Raw())),f.Host,default);gate.Validate();Deny(()=>gate.BeforeNativeCreate(@"C:\ESCAPE",gate.Sddl));Deny(()=>gate.BeforeNativeCreate(BootstrapPolicy.Targets[0],"D:(A;;FA;;;WD)"));f.NoWrites();});
        Test("wrong-volume-refused",()=>{var f=new Fixture();f.Host.Ntfs=false;Deny(()=>f.Run());f.NoWrites();});
        foreach(string mode in new[]{"--force","--deploy","--task","--cleanup"})Test("unsupported-cli-mode-"+mode,()=>Deny(()=>ProtectedBootstrapProgram.Arguments([mode,"--authorization",@"C:\Protected\approval.json","--authorization-sha",new('a',64)])));
        Test("extra-cli-arguments-denied",()=>Deny(()=>ProtectedBootstrapProgram.Arguments(["--create","--authorization",@"C:\Protected\approval.json","--authorization-sha",new('a',64),"--force"])));
        Console.WriteLine($"PROTECTED_BOOTSTRAP_DELTA: {passed} PASS / {failed} FAIL; PRODUCT_MAIN=0; WINDOWS_BACKEND=0; REAL_DIRECTORY_ACL_CHANGES=0; NATIVEAOT_BUILDS=0; OLD_SUITES=0");
        return failed==0?0:1;
    }
}

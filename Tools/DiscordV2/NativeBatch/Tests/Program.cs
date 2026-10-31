using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

// 신규 구현을 직접 시험합니다. Scheduler/계정/ACL 변경, Supervisor.Main, Job/Probe는 호출하지 않습니다.
internal static partial class AdmissionTests
{
    private static int passed,failed;
    private static string? filter;
    private static readonly AdmissionBinding Binding=new("S-1-5-21-1-2-3-1010",@"\UnitTrial",@"X:\Unit\Code",@"X:\Unit\Evidence");
    private static readonly DateTimeOffset Now=DateTimeOffset.Parse("2026-10-28T12:00:00+09:00");
    private static void Test(string name,Action action)
    {if(filter is not null&&!name.Contains(filter,StringComparison.Ordinal))return;try{action();passed++;Console.WriteLine("PASS "+name);}catch(Exception e){failed++;Console.WriteLine("FAIL "+name+" "+e.GetType().Name+":"+e.Message);}}
    private static void Check(bool value){if(!value)throw new Exception("ASSERT");}
    private static void Reject(Action action,string? tag=null)
    {try{action();}catch(Exception e){if(tag is not null)Check(e.Message==tag);return;}throw new Exception("EXPECTED_REJECTION");}
    private static TrialContract Contract()=>new(){Approved=true,TrialId="1234567890abcdef1234567890abcdef",SupervisorSha256=new('a',64),ProbeSha256=new('b',64),ExpiresUtc=Now.AddHours(1),Scope=new(){WorkerSid=Binding.WorkerSid,TaskPath=Binding.TaskPath,CodeRoot=Binding.CodeRoot,EvidenceRoot=Binding.EvidenceRoot,Mode="FAKE_ONLY",NoIpc=true,NoAi=true,JobSeconds=90,SupervisorCount=1,ProbeCount=1,Register=true,Start=true,Cleanup=true}};
    private static byte[] Raw(TrialContract c)=>JsonSerializer.SerializeToUtf8Bytes(c,ContractJson.Default.TrialContract);
    private static string Hash(byte[] raw)=>Convert.ToHexString(SHA256.HashData(raw)).ToLowerInvariant();
    private static RawSecurityDescriptor Sd(string dacl,string owner="BA")=>new("O:"+owner+"G:SYD:P"+dacl);
    private static void Policy(TrialContract c)=>AdmissionPolicy.Contract(Raw(c),Now,Binding);
    private static int Execute(FakeBackend backend,string mode,TrialContract? contract=null,Guid? instance=null,CancellationToken cancellation=default)
    {byte[] raw=Raw(contract??Contract());return AdminController.Execute(mode,raw,Hash(raw),Binding,backend,instance,cancellation);}
    static int Main(string[] args)
    {
        if(args.FirstOrDefault()=="--windows-boundary")return WindowsBoundaryTests.Run(args.Skip(1).ToArray());
        if(args.FirstOrDefault()=="--windows-handle-peer")return WindowsBoundaryTests.Peer(args.Skip(1).ToArray());
        if(args is ["--entry-gate-delta"])return EntryGateDelta();
        if(args is ["--restricted-gate-delta"])return RestrictedGateDelta();
        if(args is ["--task-mutation-delta"])return TaskMutationDelta();
        if(args is ["--task-mutation-late-read-delta"])return TaskMutationLateReadDelta();
        if(args is ["--restricted-gate-expiry-risk"])return RestrictedGateExpiryRisk();
        if(args is ["--fake-only-contract"])return FakeOnlyContracts();
        if(args is ["--interop-trim-contract"])return InteropTrimContract();
        if(args.Length>0&&args[0]=="--windows-object-peer")return WindowsBoundaryTests.ObjectPeer(args.Skip(1).ToArray());
        filter=args.SingleOrDefault();
        Test("contract-valid-scope",()=>Policy(Contract()));
        Test("contract-duplicate",()=>{string json=Encoding.UTF8.GetString(Raw(Contract()));Reject(()=>AdmissionPolicy.Contract(Encoding.UTF8.GetBytes(json.Insert(1,"\"Approved\":false,")),Now,Binding),"CONTRACT_UNKNOWN_OR_DUPLICATE");});
        Test("contract-empty-or-oversize",()=>{Reject(()=>AdmissionPolicy.Contract([],Now,Binding));Reject(()=>AdmissionPolicy.Contract(new byte[4097],Now,Binding));});
        Test("contract-type-error",()=>Reject(()=>AdmissionPolicy.Contract(Encoding.UTF8.GetBytes("[]"),Now,Binding)));
        Test("approval-false",()=>{var c=Contract();c.Approved=false;Reject(()=>Policy(c),"APPROVAL_HOLD");});
        Test("trial-empty-guid",()=>{var c=Contract();c.TrialId=new('0',32);Reject(()=>Policy(c),"TRIAL_ID");});
        Test("image-sha-not-normalized",()=>{var c=Contract();c.ProbeSha256=new('B',64);Reject(()=>Policy(c),"CONTRACT_IMAGE_SHA");});
        Test("expiry-and-deadline",()=>{var c=Contract();c.ExpiresUtc=Now;Reject(()=>Policy(c));c.ExpiresUtc=AdmissionPolicy.Deadline.AddSeconds(1);Reject(()=>Policy(c));});
        Test("scope-no-ipc-no-ai",()=>{var c=Contract();c.Scope!.NoIpc=false;Reject(()=>Policy(c),"SCOPE_MODE");c.Scope.NoIpc=true;c.Scope.NoAi=false;Reject(()=>Policy(c),"SCOPE_MODE");});
        Test("scope-fixed-path-and-sid",()=>{var c=Contract();c.Scope!.TaskPath="\\Other";Reject(()=>Policy(c),"SCOPE_BINDING");c=Contract();c.Scope!.WorkerSid="S-1-5-18";Reject(()=>Policy(c),"SCOPE_BINDING");});
        Test("scope-exact-one-and-90",()=>{var c=Contract();c.Scope!.ProbeCount=2;Reject(()=>Policy(c));c=Contract();c.Scope!.JobSeconds=91;Reject(()=>Policy(c));});
        Test("token-deny-only-admin-and-privileged-groups",()=>{TokenPolicy.Groups(["S-1-5-32-545"]);Reject(()=>TokenPolicy.Groups(["S-1-5-32-544"]),"PRIVILEGED_GROUP");Reject(()=>TokenPolicy.Groups(["S-1-5-32-551"]),"PRIVILEGED_GROUP");});
        Test("acl-readonly-and-create-only",()=>{AdmissionPolicy.Descriptor(Sd("(A;;FA;;;SY)(A;;FA;;;BA)(A;;0x1200a9;;;"+Binding.WorkerSid+")"),Binding.WorkerSid,false);AdmissionPolicy.Descriptor(Sd("(A;;FA;;;SY)(A;;FA;;;BA)(A;;0x1200ab;;;"+Binding.WorkerSid+")"),Binding.WorkerSid,true);});
        Test("acl-delete-child-or-write-rejected",()=>{Reject(()=>AdmissionPolicy.Descriptor(Sd("(A;;0x40;;;WD)"),Binding.WorkerSid,true));Reject(()=>AdmissionPolicy.Descriptor(Sd("(A;;FW;;;WD)"),Binding.WorkerSid,false));});
        Test("acl-null-or-unprotected-owner",()=>{Reject(()=>AdmissionPolicy.Descriptor(new RawSecurityDescriptor("O:BAG:SYD:NO_ACCESS_CONTROL"),Binding.WorkerSid,false));Reject(()=>AdmissionPolicy.Descriptor(Sd("(A;;FR;;;WD)",Binding.WorkerSid),Binding.WorkerSid,false));Reject(()=>AdmissionPolicy.Descriptor(new RawSecurityDescriptor("O:BAG:SYD:(A;;FR;;;WD)"),Binding.WorkerSid,false));});
        Test("record-owner-rights-exact",()=>{string a="(D;;0xd0000;;;OW)(A;;FA;;;SY)(A;;FA;;;BA)(A;;0x120003;;;"+Binding.WorkerSid+")";AdmissionPolicy.RecordDescriptor(Sd(a,Binding.WorkerSid),Binding.WorkerSid);Reject(()=>AdmissionPolicy.RecordDescriptor(Sd(a.Replace("(D;;0xd0000;;;OW)",""),Binding.WorkerSid),Binding.WorkerSid));Reject(()=>AdmissionPolicy.RecordDescriptor(Sd(a.Replace("0x120003","0x1f01ff"),Binding.WorkerSid),Binding.WorkerSid));});
        Test("consume-atomic-and-permanent",()=>
        {
            string root=Path.Combine(Environment.GetEnvironmentVariable("LIMITLESS_TEST_ROOT")??throw new Exception("TEST_ROOT_REQUIRED"),Guid.NewGuid().ToString("N"));Directory.CreateDirectory(root);
            Stream Create(string p)=>new FileStream(p,FileMode.CreateNew,FileAccess.ReadWrite,FileShare.None,4096,FileOptions.WriteThrough);
            int wins=0,losses=0;Parallel.For(0,2,_=>{try{using var lease=AdmissionConsumption.Acquire(root,[1],Create);Interlocked.Increment(ref wins);}catch(IOException){Interlocked.Increment(ref losses);}});
            Check(wins==1&&losses==1);Reject(()=>AdmissionConsumption.Acquire(root,[1],Create));Check(File.Exists(Path.Combine(root,".supervisor-once")));
        });
        Test("consume-flush-failure-is-not-reusable",()=>{bool existed=false;Stream Create(string _){if(existed)throw new IOException("exists");existed=true;return new FailFlush();}Reject(()=>AdmissionConsumption.Acquire("unused",[1],Create));Check(existed);Reject(()=>AdmissionConsumption.Acquire("unused",[1],Create));});
        Test("handle-parser-truncation",()=>{Reject(()=>HandleCensus.Parse(new byte[15]));byte[] bytes=new byte[56];BitConverter.GetBytes(2UL).CopyTo(bytes,0);Reject(()=>HandleCensus.Parse(bytes));});
        Test("handle-parser-x64-entry",()=>{byte[] bytes=new byte[56];BitConverter.GetBytes(1UL).CopyTo(bytes,0);BitConverter.GetBytes(55UL).CopyTo(bytes,16);BitConverter.GetBytes(9UL).CopyTo(bytes,24);BitConverter.GetBytes(8UL).CopyTo(bytes,32);BitConverter.GetBytes(0x40U).CopyTo(bytes,40);Check(HandleCensus.Parse(bytes).Single()==new HandleEntry(55,9,8,0x40));});
        Test("foreign-process-create-process-and-quota",()=>{foreach(uint bit in new[]{0x80U,0x100U,0x40U,0x40000U})Reject(()=>AdmissionPolicy.ForeignHandles([new(55,2,3,bit)],new Dictionary<ulong,uint>{{55,AdmissionPolicy.ProcessDanger}},1));AdmissionPolicy.ForeignHandles([new(55,2,3,0x1000)],new Dictionary<ulong,uint>{{55,AdmissionPolicy.ProcessDanger}},1);});
        Test("foreign-token-duplicate-and-redaction",()=>{Reject(()=>AdmissionPolicy.ForeignHandles([new(55,2,3,2)],new Dictionary<ulong,uint>{{55,AdmissionPolicy.TokenDanger}},1));Reject(()=>AdmissionPolicy.ForeignHandles([],new Dictionary<ulong,uint>{{55,1}},1));Reject(()=>AdmissionPolicy.ForeignHandles([new(0,1,3,8)],new Dictionary<ulong,uint>{{0,1}},1));});
        Test("foreign-job-query-or-zero-rights-still-breaks-kill-on-close",()=>{foreach(uint rights in new[]{0U,4U,0x100000U})Reject(()=>AdmissionPolicy.ForeignHandles([new(55,2,3,rights)],new Dictionary<ulong,uint>{{55,AdmissionPolicy.JobDanger}},1));AdmissionPolicy.ForeignHandles([new(55,1,3,0)],new Dictionary<ulong,uint>{{55,AdmissionPolicy.JobDanger}},1);});
        Test("x64-managed-native-layout",()=>
        {
            Check(nint.Size==8);Check(Marshal.SizeOf<Native.Startup>()==104&&Marshal.SizeOf<Native.StartupEx>()==112&&Marshal.SizeOf<Native.ProcessInfo>()==24);
            Check(Marshal.SizeOf<Native.BasicLimits>()==64&&Marshal.SizeOf<Native.ExtendedLimits>()==144&&Marshal.SizeOf<Native.Accounting>()==48);
            Check(Marshal.SizeOf<Native.TokenStats>()==56&&Marshal.SizeOf<Native.LogonData>()==88&&Marshal.OffsetOf<Native.LogonData>("Time")==80);
            Check(Marshal.SizeOf<AdmissionWindows.Attributes>()==24&&Marshal.SizeOf<TaskSchedulerNative.Variant>()==24);
            Check(Marshal.SizeOf<AccountNative.User4>()==192&&Marshal.OffsetOf<AccountNative.User4>("Sid")==152);
            Check(Marshal.SizeOf<AdmissionWindows.Mapping>()==16&&Marshal.SizeOf<AdmissionWindows.FileAttributesInfo>()==8&&Marshal.SizeOf<AccountNative.ObjectAttributes>()==48);
            Check(Marshal.SizeOf<HandleCensus.ProcessEntry>()==568&&Marshal.OffsetOf<HandleCensus.ProcessEntry>("Name")==44&&Marshal.SizeOf<Native.LsaText>()==16);
        });
        Test("accesscheck-memory-read-allow-write-deny",()=>
        {
            Native.Check(Native.OpenProcessToken(Native.GetCurrentProcess(),0xa,out nint token));
            try
            {
                // 실행 도구의 제한 token은 일반 SID와 restricting SID의 두 접근검사를 수행합니다.
                // 현재 token의 restricting SID도 메모리 시험 SD에만 읽기 허용하여 양성 사례를 만듭니다.
                Native.GetTokenInformation(token,11,0,0,out uint length);Check(length is >0 and <=65536);
                using var identity=new System.Security.Principal.WindowsIdentity(token);
                nint p=Marshal.AllocHGlobal((int)length);string acl="(A;;FR;;;"+identity.User!.Value+")(A;;FR;;;AC)";
                try
                {
                    Native.Check(Native.GetTokenInformation(token,11,p,length,out _));int count=Marshal.ReadInt32(p);Check(count>=0&&8L+count*16L<=length);
                    for(int i=0;i<count;i++)acl+="(A;;FR;;;"+new System.Security.Principal.SecurityIdentifier(Marshal.ReadIntPtr(p,8+i*16)).Value+")";
                    Console.WriteLine("INFO memory-ACL restricting-SID-count="+count+" app-container="+Native.Info<int>(token,29));
                }
                finally{Marshal.FreeHGlobal(p);}
                var descriptor=Sd(acl);byte[] sd=new byte[descriptor.BinaryLength];descriptor.GetBinaryForm(sd,0);
                var read=AdmissionWindows.AccessDecision(sd,token,1);var write=AdmissionWindows.AccessDecision(sd,token,2);if(!read.Allowed||write.Allowed)throw new Exception($"READ={read} WRITE={write}; ACE={((CommonAce)descriptor.DiscretionaryAcl![0]).AccessMask:x}");
            }
            finally{Native.CloseHandle(token);}
        });
        Test("admin-register-readback-and-no-update",()=>{var f=new FakeBackend();Check(Execute(f,"--register-only")==0&&f.RegisterCalls==1&&!f.Enabled&&f.RunCalls==0);Reject(()=>Execute(f,"--register-only"));Check(f.RegisterCalls==1);});
        Test("admin-scope-and-account-stop-before-mutation",()=>{var f=new FakeBackend();var c=Contract();c.Scope!.Register=false;Reject(()=>Execute(f,"--register-only",c));Check(f.RegisterCalls==0);f.AccountState=f.AccountState with{Sid="S-1-5-18"};Reject(()=>Execute(f,"--register-only"));Check(f.RegisterCalls==0);});
        Test("admin-register-grant-cannot-authorize-start",()=>{var f=Registered();f.ApprovalOperation="--register-only";Reject(()=>Execute(f,"--start-once"),"OPERATION_APPROVAL_BINDING");Check(f.RunCalls==0);});
        Test("worker-needs-admin-start-intent-and-instance",()=>
        {
            var f=Registered();Execute(f,"--start-once");string p=Contract().TrialId+".admin.";string sha=Hash(Raw(Contract()));
            var approval=OperationApproval.Validate(f.Approval("--start-once",Contract().TrialId),Contract().TrialId,sha,"--start-once",Now,false);
            var intent=f.Records[p+"start-intent"];ReceiptPolicy.Start(f.Records[p+"registered"],intent,approval,Contract().TrialId,sha);ReceiptPolicy.Instance(f.Records[p+"instance"],intent);
            intent.ApprovalId="different";Reject(()=>ReceiptPolicy.Start(f.Records[p+"registered"],intent,approval,Contract().TrialId,sha));Reject(()=>ReceiptPolicy.Instance(f.Records[p+"instance"],intent));
        });
        Test("admin-unapproved-or-expired-operation",()=>{var f=new FakeBackend{GrantApproved=false};Reject(()=>Execute(f,"--register-only"));Check(f.RegisterCalls==0);f.GrantApproved=true;f.GrantExpiry=Now;Reject(()=>Execute(f,"--register-only"));Check(f.RegisterCalls==0);});
        Test("admin-unknown-batch-and-expiry",()=>{var f=new FakeBackend();f.AccountState=f.AccountState with{BatchKnown=false};Reject(()=>Execute(f,"--register-only"));f.AccountState=f.AccountState with{BatchKnown=true,ExpiresUtc=Now};Reject(()=>Execute(f,"--register-only"));Check(f.RegisterCalls==0);});
        Test("batch-unresolved-context-sid-is-unknown-not-absent",()=>{HashSet<string> members=[Binding.WorkerSid];Check(AccountNative.Batch(members,[Binding.WorkerSid],["unknown-context"],_=>false)==(false,false));Check(AccountNative.Batch(members,[Binding.WorkerSid],[Binding.WorkerSid],_=>false)==(true,false));Check(AccountNative.Batch(members,[Binding.WorkerSid],["other-local-user"],_=>true)==(true,true));});
        Test("admin-start-once-and-disable",()=>{var f=Registered();Check(Execute(f,"--start-once")==0&&f.RunCalls==1&&!f.Enabled);Reject(()=>Execute(f,"--start-once"));Check(f.RunCalls==1);});
        Test("admin-run-response-loss-keeps-consumption",()=>{var f=Registered();f.FailRun=true;Reject(()=>Execute(f,"--start-once"));Check(!f.Enabled&&f.RunCalls==1&&f.Records.Keys.Any(k=>k.EndsWith("start-intent")));f.Instances=[];Reject(()=>Execute(f,"--start-once"));Check(f.RunCalls==1);});
        Test("admin-cancel-after-enable-disables",()=>{var f=Registered();using var cancel=new CancellationTokenSource();f.OnEnable=()=>cancel.Cancel();Reject(()=>Execute(f,"--start-once",cancellation:cancel.Token));Check(!f.Enabled&&f.RunCalls==0);});
        Test("admin-descriptor-change-blocks-start",()=>{var f=Registered();f.Xml=f.Xml.Replace("PT90S","PT91S");Reject(()=>Execute(f,"--start-once"));Check(f.RunCalls==0);});
        Test("admin-queued-state-without-instance-is-unknown",()=>{var f=Registered();f.StateOverride=2;Reject(()=>Execute(f,"--start-once"),"TASK_STATE_OR_INSTANCE_UNKNOWN");Check(f.RunCalls==0);});
        Test("admin-wrong-guid-does-not-stop",()=>{var f=Registered();Execute(f,"--start-once");Reject(()=>Execute(f,"--stop-owned-instance",instance:Guid.NewGuid()));Check(f.StopCalls==0);});
        Test("admin-stop-owned-instance-only",()=>{var f=Registered();Execute(f,"--start-once");Check(Execute(f,"--stop-owned-instance",instance:f.Instances.Single())==0&&f.StopCalls==1&&f.Instances.Length==0);});
        Test("admin-remove-needs-cleanup",()=>{var f=Registered();Execute(f,"--start-once");f.Instances=[];Reject(()=>Execute(f,"--remove-owned-task"));Check(f.RemoveCalls==0);f.Clean=true;Check(Execute(f,"--remove-owned-task")==0&&f.RemoveCalls==1&&f.Records.Count>0);});
        Test("admin-idle-never-started-remove",()=>{var f=Registered();Check(Execute(f,"--remove-owned-task")==0&&f.RemoveCalls==1);});
        Test("admin-expired-contract-cleanup-only",()=>{var f=Registered();f.Clock=Now.AddHours(2);Reject(()=>Execute(f,"--start-once"));Check(Execute(f,"--remove-owned-task")==0);});
        Test("admin-registration-readback-failure-preserves-task",()=>{var f=new FakeBackend{BadReadback=true};Reject(()=>Execute(f,"--register-only"));Check(f.RegisterCalls==1&&f.RemoveCalls==0&&f.Records.Values.Any(r=>r.Status=="HOLD"));});
        Test("task-xml-triggers-and-foreign-action-rejected",()=>{var c=Contract();string sha=Hash(Raw(c));string xml=TaskPolicy.Xml(c,sha,Binding);Reject(()=>TaskPolicy.Validate(new(xml.Replace("<Triggers />","<Triggers><BootTrigger /></Triggers>"),TaskPolicy.Sddl,false,[]),c,sha,Binding));Reject(()=>TaskPolicy.Validate(new(xml.Replace("Supervisor.exe","Other.exe"),TaskPolicy.Sddl,false,[]),c,sha,Binding));});
        Test("task-security-worker-write-rejected",()=>{var c=Contract();string sha=Hash(Raw(c));Reject(()=>TaskPolicy.Validate(new(TaskPolicy.Xml(c,sha,Binding),TaskPolicy.Sddl+"(A;;FA;;;"+Binding.WorkerSid+")",false,[]),c,sha,Binding));});
        Test("task-security-admin-ace-rights-or-duplicate-rejected",()=>{var c=Contract();string sha=Hash(Raw(c));Reject(()=>TaskPolicy.Validate(new(TaskPolicy.Xml(c,sha,Binding),"O:BAG:SYD:P(A;;FA;;;BA)(A;;FA;;;BA)",false,[]),c,sha,Binding));Reject(()=>TaskPolicy.Validate(new(TaskPolicy.Xml(c,sha,Binding),"O:BAG:SYD:P(A;;FR;;;SY)(A;;FA;;;BA)",false,[]),c,sha,Binding));});
        Test("job-force-10s-drain-and-code125",()=>{var f=new FakeJob();var r=JobMonitor.Run(f,default);Check(r.Completed&&r.Terminated&&r.Drained&&f.Code==125&&f.Clock>=10000&&f.Clock<90000&&f.Terminations==1);});
        Test("job-cancellation-still-terminates-own-job",()=>{var f=new FakeJob();using var c=new CancellationTokenSource();c.Cancel();var r=JobMonitor.Run(f,c.Token);Check(!r.Completed&&r.Terminated&&r.Drained&&f.Code==126);});
        Test("job-early-exit-is-hold-not-success",()=>{var f=new FakeJob{EarlyExit=true};var r=JobMonitor.Run(f,default);Check(!r.Completed&&r.Terminated&&r.Drained);});
        Test("job-late-cutoff-and-90s-drain-bound",()=>{var f=new FakeJob{Clock=85000,NeverDrains=true};var r=JobMonitor.Run(f,default);Check(!r.Completed&&!r.Drained&&f.Clock>=90000&&f.Terminations==1);});
        Test("job-multiple-process-count-or-kill-failure-hold",()=>{var f=new FakeJob{Total=2};Check(!JobMonitor.Run(f,default).Completed);f=new FakeJob{FailTerminate=true};var r=JobMonitor.Run(f,default);Check(!r.Completed&&!r.Terminated&&!r.Drained);});
        Test("job-live-isolation-failure-forces-hold-cleanup",()=>{var f=new FakeJob{FailIsolation=true};var r=JobMonitor.Run(f,default);Check(!r.Completed&&r.Terminated&&r.Drained&&f.Code==126&&f.IsolationCalls==1);});
        Console.WriteLine($"RESULT PASS={passed} FAIL={failed}; native-control-execution=0");return failed==0&&passed>0?0:1;
    }
    private static FakeBackend Registered(){var f=new FakeBackend();Execute(f,"--register-only");return f;}
    private sealed class FailFlush:MemoryStream {public override void Flush()=>throw new IOException("flush-failed");}
    private sealed class FakeJob:IJobOperations
    {
        internal long Clock;internal uint Code=259,Total=1;internal int Terminations,IsolationCalls;internal bool EarlyExit,NeverDrains,FailTerminate,FailIsolation;
        public long Elapsed=>Clock;
        public uint Wait(uint milliseconds){Clock+=milliseconds;return Code!=259||EarlyExit?0:Native.WaitTimeout;}
        public (uint Active,uint Total) Counts()=>(NeverDrains||Code==259?1U:0U,Total);
        public void Terminate(uint code){Terminations++;if(FailTerminate)throw new IOException("terminate-failed");Code=code;}
        public uint ExitCode()=>Code;
        public void CheckIsolation(){IsolationCalls++;if(FailIsolation)throw new IOException("isolation-unknown");Check(Total==1);}
    }
    private sealed class FakeBackend:IAdminBackend
    {
        internal DateTimeOffset Clock=AdmissionTests.Now;public DateTimeOffset Now=>Clock;
        internal AccountObservation AccountState=new(Binding.WorkerSid,true,false,AdmissionPolicy.Deadline.AddDays(1),true,true);
        internal readonly Dictionary<string,ControlReceipt> Records=[];
        internal string Xml="";internal bool Present,Enabled,FailRun,Clean,BadReadback;
        internal Guid[] Instances=[];internal int RegisterCalls,RunCalls,StopCalls,RemoveCalls;internal int? StateOverride;internal Action? OnEnable;
        internal string? ApprovalOperation;internal bool GrantApproved=true;internal DateTimeOffset? GrantExpiry;private TrialContract? contract;internal Action<TrialContract>? OnPreflight;internal Action? BeforeRegister;internal Action? BeforeRegisterCommit;internal Action<TaskMutation>? BeforeMutation;internal Func<byte[]>? CurrentContractOverride;internal TaskMutationBoundary? MutationBoundary;internal int EnableCalls,DisableCalls;
        internal readonly Dictionary<string,byte[]> IsolationFiles=[];
        public byte[] IsolationEvidence(string name,int limit)=>IsolationFiles.TryGetValue(name,out var raw)&&raw.Length<=limit?raw:throw new InvalidOperationException("FAKE_TRUST_EVIDENCE_MISSING_HOLD");
        public void Preflight(TrialContract value,bool cleanup) {contract=value;OnPreflight?.Invoke(value);}
        public byte[] Approval(string operation,string trial)=>JsonSerializer.SerializeToUtf8Bytes(new OperationApproval{Approved=GrantApproved,ApprovalId="abcdefabcdefabcdefabcdefabcdefab",TrialId=trial,ContractSha=Hash(Raw(contract!)),Operation=ApprovalOperation??operation,ExpiresUtc=GrantExpiry??contract!.ExpiresUtc},ApprovalJson.Default.OperationApproval);
        public byte[] CurrentContract()=>CurrentContractOverride?.Invoke()??Raw(contract!);
        public void BindMutationBoundary(TaskMutationBoundary boundary)=>MutationBoundary=boundary;
        public AccountObservation Account()=>AccountState;
        public TaskObservation? Observe()
        {
            if(!Present)return null;
            var xml=XElement.Parse(Xml);xml.Descendants().Single(e=>e.Name.LocalName=="Enabled").Value=Enabled?"true":"false";
            return new(xml.ToString(SaveOptions.DisableFormatting),TaskPolicy.Sddl,Enabled,Instances,StateOverride??(Instances.Length>0?4:Enabled?3:1));
        }
        public ControlReceipt? Receipt(string name)=>Records.GetValueOrDefault(name);
        public void Save(string name,ControlReceipt receipt){if(!Records.TryAdd(name,receipt))throw new IOException("CREATE_NEW_COLLISION");}
        public void Register(string xml,string sddl,CancellationToken cancellation){cancellation.ThrowIfCancellationRequested();BeforeRegister?.Invoke();MutationBoundary?.AfterPassword();BeforeRegisterCommit?.Invoke();MutationBoundary?.Check(TaskMutation.Register);Check(!Present);RegisterCalls++;Xml=BadReadback?xml.Replace("PT90S","PT91S"):xml;Present=true;}
        public void Enable(bool value){var m=value?TaskMutation.Enable:TaskMutation.SafetyDisable;BeforeMutation?.Invoke(m);MutationBoundary?.Check(m);if(value)EnableCalls++;else DisableCalls++;Enabled=value;if(value)OnEnable?.Invoke();}
        public Guid Run(){BeforeMutation?.Invoke(TaskMutation.Run);MutationBoundary?.Check(TaskMutation.Run);RunCalls++;Instances=[Guid.NewGuid()];if(FailRun)throw new IOException("run-response-lost");return Instances[0];}
        public void Stop(Guid instance){BeforeMutation?.Invoke(TaskMutation.Stop);MutationBoundary?.Check(TaskMutation.Stop,instance);Check(Instances.Length==1&&Instances[0]==instance);StopCalls++;Instances=[];}
        public bool CleanupProven(string _)=>Clean;
        public void Remove(){BeforeMutation?.Invoke(TaskMutation.Remove);MutationBoundary?.Check(TaskMutation.Remove);RemoveCalls++;Present=false;}
    }
}

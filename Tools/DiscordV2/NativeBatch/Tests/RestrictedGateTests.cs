using System.Text;
using System.Text.Json;

// 이번 변경 경계만 실행합니다. Native Main/Windows backend/실제 Task·ACL API는 호출하지 않습니다.
internal static partial class AdmissionTests
{
    private static TrialContract Candidate()
    {
        var c=Contract();c.TrialId=NativeTrialPolicy.CandidateTrialId;
        c.FakeOnlyIsolation=new(){Profile=FakeOnlyPolicy.ProfileName,GlobalSidAbsence="UNKNOWN",TrustEvidenceSha256=new('c',64)};return c;
    }
    private static OperationApproval Grant(TrialContract c,string mode="--start-once")=>new(){Approved=true,ApprovalId="abcdefabcdefabcdefabcdefabcdefab",TrialId=c.TrialId,ContractSha=Hash(Raw(c)),Operation=mode,ExpiresUtc=c.ExpiresUtc};
    private static FakeBackend Trusted(TrialContract c)
    {
        var f=new FakeBackend();byte[] basis=Encoding.UTF8.GetBytes("MOCK_ONLY: dedicated identity reviewed; NOT LIVE TRUST OR APPROVAL");
        var trust=new Dictionary<string,object?>{{"Approved",true},{"TrialId",c.TrialId},{"WorkerSid",Binding.WorkerSid},{"TaskPath",Binding.TaskPath},{"CodeRoot",Binding.CodeRoot},{"EvidenceRoot",Binding.EvidenceRoot},{"SupervisorSha256",c.SupervisorSha256},{"ProbeSha256",c.ProbeSha256},{"ObservedUtc",Now},{"ExpiresUtc",c.ExpiresUtc},{"BasisSha256",Hash(basis)},{"NoUntrustedSameSidCode",true},{"CredentialsNotShared",true},{"NoOtherLaunchers",true},{"ResidualRiskAccepted",true},{"GlobalSidAbsence","UNKNOWN"}};
        var raw=JsonSerializer.SerializeToUtf8Bytes(trust);f.IsolationFiles.Add(c.TrialId+".fake-only-trust.json",raw);f.IsolationFiles.Add(c.TrialId+".fake-only-trust-basis.txt",basis);c.FakeOnlyIsolation!.TrustEvidenceSha256=Hash(raw);return f;
    }
    private static int NativeAdmin(FakeBackend f,string mode,TrialContract c,Guid? instance=null)
    {byte[] raw=Raw(c);return AdminController.ExecuteNative(mode,raw,Hash(raw),Binding,f,instance,default);}
    private static int RestrictedGateExpiryRisk()
    {
        // 실제 암호/콘솔/Task 없이 대기 후 시각만 전진시킵니다. 미해결 위험을 숨기지 않고 FAIL로 보존합니다.
        Test("native-register-must-deny-after-password-wait-expiry",()=> {
            var c=Candidate();var f=Trusted(c);f.BeforeRegister=()=>f.Clock=c.ExpiresUtc.AddSeconds(1);
            bool rejected=false;try{NativeAdmin(f,"--register-only",c);}catch{rejected=true;}
            Console.WriteLine($"MOCK_EXPIRY_EVIDENCE rejected={rejected}; registration_calls={f.RegisterCalls}; WINDOWS_WRITES=0");
            Check(rejected&&f.RegisterCalls==0);
        });
        Console.WriteLine($"RESTRICTED_GATE_EXPIRY_RISK: {passed} PASS / {failed} FAIL; BUILD_BLOCKED; NATIVE_MAIN=0");
        return failed==0?0:1;
    }
    private static int RestrictedGateDelta()
    {
        Test("native-windows-boundary-stays-held",()=>Reject(NativeTrialPolicy.RequireWindowsBoundary,"HOLD_NATIVE_MUTATION_DEADLINE_REVIEW"));
        Test("trial-valid-authorized-supervisor-effect",()=>{var c=Candidate();int effect=0;NativeTrialPolicy.AuthorizeThen(c,Hash(Raw(c)),"--supervise",Grant(c),new(Binding.WorkerSid,true,false,AdmissionPolicy.Deadline,true,true),Now,Binding,()=>effect++);Check(effect==1);});
        foreach(var change in new (string,Action<TrialContract>)[]{("other-id",c=>c.TrialId="1234567890abcdef1234567890abcdef"),("unapproved",c=>c.Approved=false),("null-profile",c=>c.FakeOnlyIsolation=null),("global-pass",c=>c.FakeOnlyIsolation!.GlobalSidAbsence="PASS"),("wrong-profile",c=>c.FakeOnlyIsolation!.Profile="LIVE"),("ipc",c=>c.Scope!.NoIpc=false),("ai",c=>c.Scope!.NoAi=false),("count",c=>c.Scope!.ProbeCount=2),("time-budget",c=>c.Scope!.JobSeconds=91),("start-not-granted",c=>c.Scope!.Start=false),("expiry",c=>c.ExpiresUtc=Now)})
            Test("trial-deny-"+change.Item1+"-no-hardening-or-consume",()=>{var c=Candidate();change.Item2(c);int effects=0;Reject(()=>NativeTrialPolicy.AuthorizeThen(c,Hash(Raw(c)),"--supervise",Grant(c),new(Binding.WorkerSid,true,false,AdmissionPolicy.Deadline,true,true),Now,Binding,()=>effects++));Check(effects==0);});
        foreach(string mode in new[]{"--force","--deploy","LIVE",""})Test("trial-unknown-mode-"+mode,()=>{var c=Candidate();Reject(()=>NativeTrialPolicy.Shape(c,mode,Now));});
        Test("trial-expired-operation-no-side-effect",()=>{var c=Candidate();var g=Grant(c);g.ExpiresUtc=Now;int n=0;Reject(()=>NativeTrialPolicy.AuthorizeThen(c,Hash(Raw(c)),"--supervise",g,new(Binding.WorkerSid,true,false,AdmissionPolicy.Deadline,true,true),Now,Binding,()=>n++));Check(n==0);});
        Test("trial-wrong-operation-and-contract-sha",()=>{var c=Candidate();foreach(int kind in new[]{0,1,2}){var g=Grant(c);if(kind==0)g.Operation="--register-only";if(kind==1)g.ContractSha=new('d',64);if(kind==2)g.ApprovalId=new('0',32);Reject(()=>NativeTrialPolicy.Authorize(c,Hash(Raw(c)),"--supervise",g,new(Binding.WorkerSid,true,false,AdmissionPolicy.Deadline,true,true),Now,Binding));}});
        foreach(int kind in new[]{0,1,2,3,4})Test("trial-account-denial-"+kind,()=>{var c=Candidate();var u=new AccountObservation(Binding.WorkerSid,true,false,AdmissionPolicy.Deadline,true,true);u=kind switch{0=>u with{Sid="S-1-5-18"},1=>u with{Enabled=false},2=>u with{Locked=true},3=>u with{BatchKnown=false},_=>u with{ExpiresUtc=Now}};Reject(()=>NativeTrialPolicy.Authorize(c,Hash(Raw(c)),"--supervise",Grant(c),u,Now,Binding));});
        Test("trial-cutoff-overrides-cleanup-expiry-exception",()=>{var c=Candidate();c.ExpiresUtc=AdmissionPolicy.Deadline;var g=Grant(c,"--remove-owned-task");Reject(()=>NativeTrialPolicy.Authorize(c,Hash(Raw(c)),"--remove-owned-task",g,new(Binding.WorkerSid,false,true,Now,false,false),AdmissionPolicy.Deadline.AddSeconds(1),Binding));});
        Test("trial-fresh-cleanup-retains-disabled-account-safety",()=>{var c=Candidate();NativeTrialPolicy.Authorize(c,Hash(Raw(c)),"--remove-owned-task",Grant(c,"--remove-owned-task"),new(Binding.WorkerSid,false,true,Now,false,false),Now,Binding);});
        Test("native-admin-register-start-and-duplicate-consumption",()=>{var c=Candidate();var f=Trusted(c);Check(NativeAdmin(f,"--register-only",c)==0&&f.RegisterCalls==1&&f.RunCalls==0);Check(NativeAdmin(f,"--start-once",c)==0&&f.RunCalls==1);f.Instances=[];Reject(()=>NativeAdmin(f,"--start-once",c));Check(f.RunCalls==1);});
        Test("native-admin-unapproved-trial-no-record",()=>{var c=Candidate();c.TrialId="1234567890abcdef1234567890abcdef";var f=Trusted(c);Reject(()=>NativeAdmin(f,"--register-only",c));Check(f.Records.Count==0&&f.RegisterCalls==0);});
        Test("native-admin-missing-trust-no-record",()=>{var c=Candidate();var f=new FakeBackend();Reject(()=>NativeAdmin(f,"--register-only",c));Check(f.Records.Count==0&&f.RegisterCalls==0);});
        Test("native-admin-tampered-trust-no-record",()=>{var c=Candidate();var f=Trusted(c);f.IsolationFiles[c.TrialId+".fake-only-trust.json"][0]=0;Reject(()=>NativeAdmin(f,"--register-only",c));Check(f.Records.Count==0&&f.RegisterCalls==0);});
        Test("native-admin-image-or-protected-path-rejection-before-write",()=>{var c=Candidate();var f=Trusted(c);f.OnPreflight=_=>throw new IOException("IMAGE_OR_PATH_HOLD");Reject(()=>NativeAdmin(f,"--register-only",c));Check(f.Records.Count==0&&f.RegisterCalls==0);});
        Test("native-admin-failed-run-keeps-consumption-and-disables",()=>{var c=Candidate();var f=Trusted(c);NativeAdmin(f,"--register-only",c);f.FailRun=true;Reject(()=>NativeAdmin(f,"--start-once",c));Check(!f.Enabled&&f.RunCalls==1&&f.Records.Keys.Any(k=>k.EndsWith("start-intent")));f.Instances=[];Reject(()=>NativeAdmin(f,"--start-once",c));Check(f.RunCalls==1);});
        Test("native-admin-invalid-account-before-mutation",()=>{var c=Candidate();var f=Trusted(c);f.AccountState=f.AccountState with{BatchKnown=false};Reject(()=>NativeAdmin(f,"--register-only",c));Check(f.Records.Count==0&&f.RegisterCalls==0);});
        Test("native-admin-wrong-operation-before-mutation",()=>{var c=Candidate();var f=Trusted(c);f.ApprovalOperation="--start-once";Reject(()=>NativeAdmin(f,"--register-only",c));Check(f.Records.Count==0&&f.RegisterCalls==0);});
        Test("native-admin-rejects-duplicate-json-before-mutation",()=>{var c=Candidate();var f=Trusted(c);byte[] raw=Encoding.UTF8.GetBytes(Encoding.UTF8.GetString(Raw(c)).Insert(1,"\"Approved\":true,"));Reject(()=>AdminController.ExecuteNative("--register-only",raw,Hash(raw),Binding,f,null,default));Check(f.Records.Count==0);});
        Test("native-admin-stale-trust-before-mutation",()=>{var c=Candidate();var f=Trusted(c);f.Clock=Now.AddMinutes(16);Reject(()=>NativeAdmin(f,"--register-only",c));Check(f.Records.Count==0&&f.RegisterCalls==0);});
        DeploymentDelta();
        Console.WriteLine($"RESTRICTED_GATE_DELTA: {passed} PASS / {failed} FAIL; NATIVE_MAIN=0; WINDOWS_WRITES=0; LIVE_APPROVAL=0");return failed==0?0:1;
    }
    private sealed class DeployFake:IProtectedDeployment
    {
        internal DeploymentApproval Approval=new(){Approved=true,Profile="PROTECTED_DEPLOYMENT_V1",DeploymentId="11111111111111111111111111111111",TrialId=NativeTrialPolicy.CandidateTrialId,ExpiresUtc=AdmissionTests.Now.AddMinutes(10)};
        internal HashSet<string> Directories=[@"X:\"];internal HashSet<string> Consumed=[];
        internal int DirectoryWrites,Copies,Consumes;internal bool BadAncestor,BadSource,FailCreate,FailCopy,FileAtRoot;
        public DateTimeOffset Now=>AdmissionTests.Now;
        public byte[] ReadAdministratorApproval()=>JsonSerializer.SerializeToUtf8Bytes(Approval,DeploymentJson.Default.DeploymentApproval);
        public bool DirectoryExists(string p)=>Directories.Contains(p);
        public bool FileExists(string p)=>FileAtRoot&&p==Binding.CodeRoot;
        public void PinAncestor(string p){if(BadAncestor)throw new IOException("ANCESTOR_HOLD");}
        public void PinSource(DeploymentFile f){if(BadSource)throw new IOException("SOURCE_HASH_HOLD");}
        public void ConsumeApproval(string id,string sha){if(!Consumed.Add(id))throw new IOException("CONSUMED");Consumes++;}
        public void CreateDirectoryNew(string p,bool e){if(FailCreate)throw new IOException("CREATE_FAILED");if(!Directories.Add(p))throw new IOException("EXISTS");DirectoryWrites++;}
        public void CopyNew(string c,DeploymentFile f){if(FailCopy)throw new IOException("COPY_FAILED");Copies++;}
    }
    private static DeploymentPlan DeployPlan()=>new(){TrialId=NativeTrialPolicy.CandidateTrialId,WorkerSid=Binding.WorkerSid,CodeRoot=Binding.CodeRoot,EvidenceRoot=Binding.EvidenceRoot,TaskPath=Binding.TaskPath,ExpiresUtc=Now.AddMinutes(10),Files=[new(){Name="Supervisor.exe",Source=@"X:\Source\Supervisor.exe",Sha256=new('a',64),Bytes=10},new(){Name="Probe.exe",Source=@"X:\Source\Probe.exe",Sha256=new('b',64),Bytes=20}]};
    private static void Deploy(DeploymentPlan p,DeployFake f)
    {byte[] raw=JsonSerializer.SerializeToUtf8Bytes(p,DeploymentJson.Default.DeploymentPlan);f.Approval.ManifestSha256=ProtectedDeployment.Hash(raw);ProtectedDeployment.Execute(raw,f.Approval.ManifestSha256,Binding,f);}
    private static void DeploymentDelta()
    {
        Test("deploy-mock-create-new-exact-two-images",()=>{var f=new DeployFake();Deploy(DeployPlan(),f);Check(f.Copies==2&&f.DirectoryWrites==3&&f.Consumes==1);});
        foreach(var change in new (string,Action<DeploymentPlan>)[]{("wrong-trial",p=>p.TrialId="1234567890abcdef1234567890abcdef"),("expired",p=>p.ExpiresUtc=Now),("binding",p=>p.WorkerSid="S-1-5-18"),("source-overlap",p=>p.Files[0].Source=Path.Combine(p.CodeRoot,"input.exe")),("path-escape",p=>p.Files[0].Name="../escape.exe"),("duplicate",p=>p.Files[1].Name=p.Files[0].Name),("consumption-name",p=>p.Files[0].Name=".supervisor-once"),("missing-probe",p=>p.Files[1].Name="TrialContract.json"),("invalid-sha",p=>p.Files[0].Sha256="BAD")})
            Test("deploy-deny-"+change.Item1+"-before-write",()=>{var p=DeployPlan();change.Item2(p);var f=new DeployFake();Reject(()=>Deploy(p,f));Check(f.DirectoryWrites==0&&f.Copies==0&&f.Consumes==0);});
        foreach(int kind in new[]{0,1,2,3,4})Test("deploy-approval-denial-"+kind,()=>{var f=new DeployFake();if(kind==0)f.Approval.Approved=false;if(kind==1)f.Approval.ExpiresUtc=Now;if(kind==2)f.Approval.Profile="WRONG";if(kind==3)f.Approval.TrialId="WRONG";if(kind==4)f.Approval.DeploymentId=new('0',32);Reject(()=>Deploy(DeployPlan(),f));Check(f.DirectoryWrites==0&&f.Consumes==0);});
        Test("deploy-existing-root-never-adopted",()=>{var f=new DeployFake();f.Directories.Add(Binding.CodeRoot);Reject(()=>Deploy(DeployPlan(),f));Check(f.DirectoryWrites==0&&f.Consumes==0);});
        Test("deploy-file-at-root-blocks-before-write",()=>{var f=new DeployFake{FileAtRoot=true};Reject(()=>Deploy(DeployPlan(),f));Check(f.DirectoryWrites==0&&f.Consumes==0);});
        Test("deploy-ancestor-rejection-no-partial-write",()=>{var f=new DeployFake{BadAncestor=true};Reject(()=>Deploy(DeployPlan(),f));Check(f.DirectoryWrites==0&&f.Consumes==0);});
        Test("deploy-source-hash-rejection-no-partial-write",()=>{var f=new DeployFake{BadSource=true};Reject(()=>Deploy(DeployPlan(),f));Check(f.DirectoryWrites==0&&f.Consumes==0);});
        Test("deploy-failed-create-consumption-not-reset",()=>{var f=new DeployFake{FailCreate=true};Reject(()=>Deploy(DeployPlan(),f));Check(f.Consumes==1&&f.DirectoryWrites==0);f.FailCreate=false;Reject(()=>Deploy(DeployPlan(),f));Check(f.DirectoryWrites==0&&f.Consumes==1);});
        Test("deploy-failed-copy-preserves-partial-no-rollback",()=>{var f=new DeployFake{FailCopy=true};Reject(()=>Deploy(DeployPlan(),f));Check(f.DirectoryWrites==3&&f.Consumes==1&&f.Copies==0);Reject(()=>Deploy(DeployPlan(),f));Check(f.DirectoryWrites==3);});
        Test("deploy-tampered-manifest-hash-no-effects",()=>{var f=new DeployFake();byte[] raw=JsonSerializer.SerializeToUtf8Bytes(DeployPlan(),DeploymentJson.Default.DeploymentPlan);Reject(()=>ProtectedDeployment.Execute(raw,new('0',64),Binding,f));Check(f.Consumes==0&&f.DirectoryWrites==0);});
    }
}

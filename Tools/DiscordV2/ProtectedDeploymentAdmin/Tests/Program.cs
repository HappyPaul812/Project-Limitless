using System.Text;
using System.Text.Json;
using System.Security.AccessControl;

// 메모리 host만 실행합니다. 독립 도구 Main/Windows backend/실제 이미지·계정·Task를 호출하지 않습니다.
internal static class DeploymentAdminTests
{
    private static readonly DateTimeOffset Now=new(2026,10,31,6,0,0,TimeSpan.Zero);
    private static readonly AdmissionBinding Binding=new(Native.Worker,Native.TaskPath,Native.CodeRoot,Native.EvidenceRoot);
    private static int pass,fail;
    private static string Hash(byte[] b)=>ProtectedDeployment.Hash(b);
    private static void Check(bool ok){if(!ok)throw new Exception("ASSERT");}
    private static void Deny(Action action){try{action();}catch{return;}throw new Exception("EXPECTED_REJECTION");}
    private static void Test(string name,Action action){try{action();pass++;Console.WriteLine("PASS "+name);}catch(Exception e){fail++;Console.WriteLine("FAIL "+name+" "+e.GetType().Name+":"+e.Message);}}
    private sealed class Fixture
    {
        internal readonly Fake Host=new();
        internal readonly string ContractPath=@"X:\Staging\TrialContract.json";
        internal readonly string ManifestPath=@"X:\Staging\manifest.json";
        internal readonly string ApprovalPath=@"X:\Staging\approval.json";
        internal readonly string RiskPath=@"X:\Staging\review.txt";
        internal TrialContract Contract;
        internal DeploymentPlan Plan;
        internal DeploymentApproval Approval;
        internal DeploymentRequest Request;
        internal Fixture()
        {
            var expiry=Now.AddHours(1);string trial=NativeTrialPolicy.CandidateTrialId;
            byte[] basis=Encoding.UTF8.GetBytes("MOCK ONLY: no live trust assertion; dedicated account/credential/launcher review simulated.");
            var trust=new Dictionary<string,object?>{{"Approved",true},{"TrialId",trial},{"WorkerSid",Binding.WorkerSid},{"TaskPath",Binding.TaskPath},{"CodeRoot",Binding.CodeRoot},{"EvidenceRoot",Binding.EvidenceRoot},{"SupervisorSha256",DeploymentAdmission.SupervisorSha},{"ProbeSha256",DeploymentAdmission.ProbeSha},{"ObservedUtc",Now},{"ExpiresUtc",expiry},{"BasisSha256",Hash(basis)},{"NoUntrustedSameSidCode",true},{"CredentialsNotShared",true},{"NoOtherLaunchers",true},{"ResidualRiskAccepted",true},{"GlobalSidAbsence","UNKNOWN"}};
            byte[] trustRaw=JsonSerializer.SerializeToUtf8Bytes(trust);
            string trustPath=@"X:\Staging\"+trial+".fake-only-trust.json",basisPath=@"X:\Staging\"+trial+".fake-only-trust-basis.txt";
            Host.Inputs[trustPath]=trustRaw;Host.Inputs[basisPath]=basis;Host.Inputs[RiskPath]=basis;
            Contract=new(){Approved=true,TrialId=trial,SupervisorSha256=DeploymentAdmission.SupervisorSha,ProbeSha256=DeploymentAdmission.ProbeSha,ExpiresUtc=expiry,
                Scope=new(){WorkerSid=Binding.WorkerSid,TaskPath=Binding.TaskPath,CodeRoot=Binding.CodeRoot,EvidenceRoot=Binding.EvidenceRoot,Mode="FAKE_ONLY",NoIpc=true,NoAi=true,JobSeconds=90,SupervisorCount=1,ProbeCount=1,Register=true,Start=true,Cleanup=true},
                FakeOnlyIsolation=new(){Profile=FakeOnlyPolicy.ProfileName,GlobalSidAbsence="UNKNOWN",TrustEvidenceSha256=Hash(trustRaw)}};
            Plan=new(){TrialId=trial,WorkerSid=Binding.WorkerSid,CodeRoot=Binding.CodeRoot,EvidenceRoot=Binding.EvidenceRoot,TaskPath=Binding.TaskPath,ExpiresUtc=expiry,
                Files=[new(){Name="Supervisor.exe",Source=@"X:\Images\Supervisor.exe",Sha256=DeploymentAdmission.SupervisorSha,Bytes=7559680},new(){Name="Probe.exe",Source=@"X:\Images\Probe.exe",Sha256=DeploymentAdmission.ProbeSha,Bytes=1892352},
                    new(){Name="TrialContract.json",Source=ContractPath},new(){Name=trial+".fake-only-trust.json",Source=trustPath,Sha256=Hash(trustRaw),Bytes=trustRaw.Length},new(){Name=trial+".fake-only-trust-basis.txt",Source=basisPath,Sha256=Hash(basis),Bytes=basis.Length}]};
            Approval=new(){Approved=true,Profile="PROTECTED_DEPLOYMENT_V1",DeploymentId="11111111111111111111111111111111",TrialId=trial,ExpiresUtc=expiry};
            Request=new(){Approved=true,Profile="INDEPENDENT_ADMIN_DEPLOY_V1",DeploymentId=Approval.DeploymentId,TrialId=trial,ToolSha256=new('d',64),ManifestPath=ManifestPath,ApprovalPath=ApprovalPath,ContractPath=ContractPath,RiskBasisPath=RiskPath,ExpiresUtc=expiry,CreateRoots=true,CopyFiles=true,CredentialsReviewed=true,OtherLaunchPathsReviewed=true,ImageTrustReviewed=true,PrivilegedToolRiskAccepted=true};
            Seal();
        }
        internal void Seal()
        {
            byte[] contract=JsonSerializer.SerializeToUtf8Bytes(Contract,ContractJson.Default.TrialContract);Host.Inputs[ContractPath]=contract;
            var f=Plan.Files.SingleOrDefault(f=>f.Name=="TrialContract.json");if(f is not null){f.Sha256=Hash(contract);f.Bytes=contract.Length;}
            byte[] manifest=JsonSerializer.SerializeToUtf8Bytes(Plan,DeploymentJson.Default.DeploymentPlan);Host.Inputs[ManifestPath]=manifest;
            Approval.ManifestSha256=Hash(manifest);byte[] approval=JsonSerializer.SerializeToUtf8Bytes(Approval,DeploymentJson.Default.DeploymentApproval);Host.Inputs[ApprovalPath]=approval;
            Request.ManifestSha256=Hash(manifest);Request.ApprovalSha256=Hash(approval);Request.ContractSha256=Hash(contract);Request.RiskBasisSha256=Hash(Host.Inputs[RiskPath]);
        }
        internal byte[] Raw()=>JsonSerializer.SerializeToUtf8Bytes(Request,DeploymentRequestJson.Default.DeploymentRequest);
        internal int Run(string mode="--apply"){byte[] raw=Raw();return DeploymentController.Run(mode,raw,Hash(raw),Binding,Host);}
        internal void NoWrites()=>Check(Host.Consumes==0&&Host.DirectoryWrites==0&&Host.Copies==0);
    }
    private sealed class Fake:IDeploymentHost
    {
        internal readonly Dictionary<string,byte[]> Inputs=[];
        internal HashSet<string> Directories=[@"X:\"],Files=[],Consumed=[];
        internal DateTimeOffset Clock=Now;
        internal DeploymentActor Actor=new(true,true,new('d',64));
        internal int Consumes,DirectoryWrites,Copies;
        internal int Deletes=>0;
        internal bool BadProtected,BadAncestor,BadSource,FailDirectory,FailCopyReadback;
        internal Action<DeploymentStep>? BeforeNative;
        internal Action? OnActor;
        private DeploymentWriteBoundary? boundary;
        public DateTimeOffset Now=>Clock;
        public byte[] ReadProtected(string path,int limit){if(BadProtected||!Inputs.TryGetValue(path,out var b)||b.Length>limit)throw new IOException("PROTECTED_INPUT_HOLD");return (byte[])b.Clone();}
        public DeploymentActor ObserveActor(){OnActor?.Invoke();return Actor;}
        public void BindBoundary(DeploymentWriteBoundary b){if(boundary is not null)throw new IOException("BOUND");boundary=b;}
        private void Guard(DeploymentStep s){BeforeNative?.Invoke(s);(boundary??throw new IOException("UNBOUND")).Check(s);}
        public byte[] ReadAdministratorApproval()=>throw new Exception("USE_CONTROLLER");
        public bool DirectoryExists(string path)=>Directories.Contains(path);
        public bool FileExists(string path)=>Files.Contains(path);
        public void PinAncestor(string path){if(BadAncestor)throw new IOException("UNTRUSTED_ANCESTOR");}
        public void PinSource(DeploymentFile file){if(BadSource)throw new IOException("SOURCE_SHA");if(!file.Name.EndsWith(".exe")){Check(Inputs.TryGetValue(file.Source,out var b)&&Hash(b)==file.Sha256&&b.Length==file.Bytes);}}
        public void ConsumeApproval(string id,string sha){Guard(new(DeploymentStepKind.Consume,id,Sha:sha));if(!Consumed.Add(id))throw new IOException("CREATE_NEW_COLLISION");Consumes++;}
        public void CreateDirectoryNew(string path,bool evidence){Guard(new(DeploymentStepKind.Directory,path,evidence));if(FailDirectory)throw new IOException("CREATE_FAILED");if(!Directories.Add(path))throw new IOException("EXISTS");DirectoryWrites++;}
        public void CopyNew(string root,DeploymentFile file){string path=Path.Combine(root,file.Name);Guard(new(DeploymentStepKind.Copy,path,File:file));if(!Files.Add(path))throw new IOException("EXISTS");Copies++;if(FailCopyReadback)throw new IOException("SHA_OR_DACL_READBACK_FAILED");}
    }
    static int Main()
    {
        Test("review-full-plan-no-consume-or-writes",()=>{var f=new Fixture();Check(f.Run("--review")>5);f.NoWrites();});
        Test("apply-exact-plan-five-files-and-create-new",()=>{var f=new Fixture();f.Run();Check(f.Host.Consumes==1&&f.Host.DirectoryWrites==3&&f.Host.Copies==5&&f.Host.Deletes==0);});
        foreach(var change in new (string,Action<DeploymentRequest>)[]{("unapproved",r=>r.Approved=false),("profile",r=>r.Profile="OTHER"),("deployment-id",r=>r.DeploymentId=new('0',32)),("trial-id",r=>r.TrialId="11111111111111111111111111111111"),("tool-sha",r=>r.ToolSha256=new('a',64)),("manifest-sha",r=>r.ManifestSha256=new('a',64)),("approval-sha",r=>r.ApprovalSha256=new('a',64)),("contract-sha",r=>r.ContractSha256=new('a',64)),("risk-sha",r=>r.RiskBasisSha256=new('a',64)),("expired",r=>r.ExpiresUtc=Now),("create-scope",r=>r.CreateRoots=false),("copy-scope",r=>r.CopyFiles=false),("credentials-unknown",r=>r.CredentialsReviewed=false),("launch-paths-unknown",r=>r.OtherLaunchPathsReviewed=false),("image-trust-unknown",r=>r.ImageTrustReviewed=false),("tool-risk-unaccepted",r=>r.PrivilegedToolRiskAccepted=false)})
            Test("deny-"+change.Item1+"-no-writes",()=>{var f=new Fixture();change.Item2(f.Request);Deny(()=>f.Run());f.NoWrites();});
        foreach(int kind in new[]{0,1,2})Test("deny-actor-"+kind,()=>{var f=new Fixture();f.Host.Actor=kind switch{0=>f.Host.Actor with{ElevatedAdministrator=false},1=>f.Host.Actor with{NotImpersonating=false},_=>f.Host.Actor with{ToolSha256=new('c',64)}};Deny(()=>f.Run());f.NoWrites();});
        foreach(string root in new[]{Native.CodeRoot,Native.EvidenceRoot})Test("existing-root-preserved-"+root,()=>{var f=new Fixture();f.Host.Directories.Add(root);Deny(()=>f.Run());f.NoWrites();Check(f.Host.Directories.Contains(root));});
        Test("file-at-root-denied",()=>{var f=new Fixture();f.Host.Files.Add(Native.CodeRoot);Deny(()=>f.Run());f.NoWrites();});
        foreach(int kind in new[]{0,1,2})Test("protected-input-ancestor-source-failure-"+kind,()=>{var f=new Fixture();if(kind==0)f.Host.BadProtected=true;if(kind==1)f.Host.BadAncestor=true;if(kind==2)f.Host.BadSource=true;Deny(()=>f.Run());f.NoWrites();});
        Test("request-duplicate-json-no-writes",()=>{var f=new Fixture();byte[] b=Encoding.UTF8.GetBytes(Encoding.UTF8.GetString(f.Raw()).Insert(1,"\"Approved\":true,"));Deny(()=>DeploymentController.Run("--apply",b,Hash(b),Binding,f.Host));f.NoWrites();});
        Test("request-wrong-raw-sha",()=>{var f=new Fixture();Deny(()=>DeploymentController.Run("--apply",f.Raw(),new('a',64),Binding,f.Host));f.NoWrites();});
        Test("approval-id-must-match-request",()=>{var f=new Fixture();f.Approval.DeploymentId="22222222222222222222222222222222";f.Seal();Deny(()=>f.Run());f.NoWrites();});
        Test("image-substitution-denied",()=>{var f=new Fixture();f.Plan.Files[0].Sha256=new('e',64);f.Seal();Deny(()=>f.Run());f.NoWrites();});
        Test("contract-image-mismatch-denied",()=>{var f=new Fixture();f.Contract.ProbeSha256=new('e',64);f.Seal();Deny(()=>f.Run());f.NoWrites();});
        Test("extra-executable-denied",()=>{var f=new Fixture();f.Plan.Files=[..f.Plan.Files,new(){Name="Other.exe",Source=@"X:\Images\Other.exe",Sha256=new('e',64),Bytes=1}];f.Seal();Deny(()=>f.Run());f.NoWrites();});
        Test("expiry-between-controller-and-native-consume-denied",()=>{var f=new Fixture();f.Host.BeforeNative=s=>{if(s.Kind==DeploymentStepKind.Consume)f.Host.Clock=f.Request.ExpiresUtc;};Deny(()=>f.Run());f.NoWrites();});
        Test("expiry-during-final-actor-read-denied",()=>{var f=new Fixture();f.Host.OnActor=()=>f.Host.Clock=f.Request.ExpiresUtc;Deny(()=>f.Run());f.NoWrites();});
        Test("cutoff-not-extended",()=>{var f=new Fixture();f.Host.Clock=AdmissionPolicy.Deadline.AddTicks(1);Deny(()=>f.Run());f.NoWrites();});
        Test("failed-create-consumption-retained-and-no-reuse",()=>{var f=new Fixture();f.Host.FailDirectory=true;Deny(()=>f.Run());Check(f.Host.Consumes==1&&f.Host.DirectoryWrites==0);var second=new Fixture();second.Host.Consumed=f.Host.Consumed;Deny(()=>second.Run());Check(second.Host.Consumes==0&&second.Host.DirectoryWrites==0&&second.Host.Copies==0&&f.Host.Consumed.Count==1);});
        Test("failed-readback-keeps-partial-evidence-and-original-inputs",()=>{var f=new Fixture();int originals=f.Host.Inputs.Count;f.Host.FailCopyReadback=true;Deny(()=>f.Run());Check(f.Host.Consumes==1&&f.Host.Copies==1&&f.Host.Files.Count==1&&f.Host.Inputs.Count==originals&&f.Host.Deletes==0);});
        Test("boundary-unavailable-outside-authorized-step",()=>{var f=new Fixture();f.Run();int n=f.Host.DirectoryWrites;Deny(()=>f.Host.CreateDirectoryNew(@"X:\ESCAPE",false));Check(f.Host.DirectoryWrites==n);});
        foreach(string mode in new[]{"--force","--deploy","--apply --force",""})Test("unknown-mode-"+mode,()=>{var f=new Fixture();Deny(()=>f.Run(mode));f.NoWrites();});
        Test("entry-arguments-no-extra-or-relative-input",()=>{Deny(()=>AdminDeploymentProgram.Arguments(["--apply","--request","relative","--request-sha",new('a',64)]));Deny(()=>AdminDeploymentProgram.Arguments(["--apply","--request",@"X:\request","--request-sha",new('a',64),"--force"]));});
        foreach(string mask in new[]{"0x001200a9","0x001200ab","0x00120089"})Test("descriptor-readback-"+mask,()=>{string text=DeploymentSecurityPolicy.Sddl(mask,Native.Worker);var expected=new RawSecurityDescriptor(text);DeploymentSecurityPolicy.Readback(new(text),expected,Native.Worker,mask=="0x001200ab");Deny(()=>DeploymentSecurityPolicy.Readback(new(text.Replace("D:P","D:")),expected,Native.Worker,mask=="0x001200ab"));});
        Test("descriptor-worker-write-owner-rejected",()=>{string text=DeploymentSecurityPolicy.Sddl("0x001200a9",Native.Worker);Deny(()=>DeploymentSecurityPolicy.Readback(new(text.Replace("0x001200a9","FA")),new(text),Native.Worker,false));});
        Test("managed-or-malformed-tool-image-rejected",()=>{Deny(()=>DeploymentSecurityPolicy.NativeImage(new byte[400]));byte[] b=new byte[400];BitConverter.GetBytes((ushort)0x5a4d).CopyTo(b,0);BitConverter.GetBytes(64).CopyTo(b,0x3c);BitConverter.GetBytes(0x4550).CopyTo(b,64);BitConverter.GetBytes((ushort)0x8664).CopyTo(b,68);BitConverter.GetBytes((ushort)0x20b).CopyTo(b,88);DeploymentSecurityPolicy.NativeImage(b);b[64+24+112+14*8]=1;Deny(()=>DeploymentSecurityPolicy.NativeImage(b));});
        Console.WriteLine($"INDEPENDENT_DEPLOYMENT_DELTA: {pass} PASS / {fail} FAIL; TOOL_MAIN=0; WINDOWS_BACKEND=0; REAL_WRITES=0; TOOL_BUILDS=0; OLD_TESTS=0");
        return fail==0?0:1;
    }
}

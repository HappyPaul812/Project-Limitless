using System.Text;
using System.Text.Json;

// 새 계약 차이만 검증합니다. 기존52개 및 Windows7개를 호출하지 않습니다.
internal static partial class AdmissionTests
{
    private static int FakeOnlyContracts()
    {
        (TrialContract c,Dictionary<string,object?> trust,Dictionary<string,byte[]> files) Fixture()
        {
            var c=Contract();byte[] basis=Encoding.UTF8.GetBytes("UNIT_FIXTURE_ONLY: reviewed dedicated identity and protected assets, no hostile peer; NOT LIVE EVIDENCE");
            var trust=new Dictionary<string,object?>{{"Approved",true},{"TrialId",c.TrialId},{"WorkerSid",Binding.WorkerSid},{"TaskPath",Binding.TaskPath},{"CodeRoot",Binding.CodeRoot},{"EvidenceRoot",Binding.EvidenceRoot},{"SupervisorSha256",c.SupervisorSha256},{"ProbeSha256",c.ProbeSha256},{"ObservedUtc",Now},{"ExpiresUtc",Now.AddMinutes(10)},{"BasisSha256",Hash(basis)},{"NoUntrustedSameSidCode",true},{"CredentialsNotShared",true},{"NoOtherLaunchers",true},{"ResidualRiskAccepted",true},{"GlobalSidAbsence","UNKNOWN"}};
            var files=new Dictionary<string,byte[]>{{c.TrialId+".fake-only-trust-basis.txt",basis}};
            c.FakeOnlyIsolation=new(){Profile=FakeOnlyPolicy.ProfileName,GlobalSidAbsence="UNKNOWN"};Seal(c,trust,files);return(c,trust,files);
        }
        void Seal(TrialContract c,Dictionary<string,object?> trust,Dictionary<string,byte[]> files)
        {var raw=JsonSerializer.SerializeToUtf8Bytes(trust);files[c.TrialId+".fake-only-trust.json"]=raw;c.FakeOnlyIsolation!.TrustEvidenceSha256=Hash(raw);}
        void Evidence(TrialContract c,Dictionary<string,byte[]> files, bool cleanup=false)
        {FakeOnlyPolicy.Evidence(c,Now,(name,limit)=>files.TryGetValue(name,out var raw)?raw:throw new InvalidOperationException("MISSING"),cleanup);}
        Test("fake-legacy-global-contract-unchanged",()=>{var c=Contract();Policy(c);Check(!Encoding.UTF8.GetString(Raw(c)).Contains("FakeOnlyIsolation"));bool called=false;FakeOnlyPolicy.Exclusivity(c,Now,(_,_)=>throw new Exception("READ"),()=>called=true);Check(called);});
        Test("fake-scoped-evidence-global-remains-unknown",()=>{var(c,_,files)=Fixture();Policy(c);FakeOnlyPolicy.Exclusivity(c,Now,(name,_)=>files[name],()=>throw new Exception("GLOBAL_CALLED"));Check(c.FakeOnlyIsolation!.GlobalSidAbsence=="UNKNOWN");});
        Test("fake-reject-profile-null-duplicate-unknown",()=>{var(c,_,_)=Fixture();string raw=Encoding.UTF8.GetString(Raw(c));Reject(()=>AdmissionPolicy.Contract(Encoding.UTF8.GetBytes(raw.Replace("\"FakeOnlyIsolation\":{","\"FakeOnlyIsolation\":null,\"FakeOnlyIsolation\":{")),Now,Binding));c.FakeOnlyIsolation!.Profile="OPERATING";Reject(()=>Policy(c));});
        Test("fake-reject-global-pass",()=>{var(c,_,_)=Fixture();c.FakeOnlyIsolation!.GlobalSidAbsence="PASS";Reject(()=>Policy(c));});
        Test("fake-reject-ipc-ai-count-relaxation",()=>{foreach(int kind in new[]{0,1,2,3}){var(c,_,files)=Fixture();if(kind==0)c.Scope!.NoIpc=false;if(kind==1)c.Scope!.NoAi=false;if(kind==2)c.Scope!.ProbeCount=2;if(kind==3)c.Scope!.Mode="LIVE";Reject(()=>Policy(c));Reject(()=>Evidence(c,files));}});
        Test("fake-missing-evidence-blocks-global-substitution",()=>{var(c,_,files)=Fixture();files.Remove(c.TrialId+".fake-only-trust.json");bool global=false;Reject(()=>FakeOnlyPolicy.Exclusivity(c,Now,(name,_)=>files[name],()=>global=true));Check(!global);});
        Test("fake-tampered-trust-hash",()=>{var(c,_,files)=Fixture();files[c.TrialId+".fake-only-trust.json"][0]=(byte)' ';Reject(()=>Evidence(c,files),"FAKE_TRUST_HASH");});
        Test("fake-reject-each-unconfirmed-assumption",()=>{foreach(string key in new[]{"Approved","NoUntrustedSameSidCode","CredentialsNotShared","NoOtherLaunchers","ResidualRiskAccepted"}){var(c,t,files)=Fixture();t[key]=false;Seal(c,t,files);Reject(()=>Evidence(c,files),"FAKE_TRUST_ASSUMPTIONS_HOLD");}});
        Test("fake-reject-each-rebound-identity-image-path",()=>{foreach(string key in new[]{"TrialId","WorkerSid","TaskPath","CodeRoot","EvidenceRoot","SupervisorSha256","ProbeSha256"}){var(c,t,files)=Fixture();t[key]="other";Seal(c,t,files);Reject(()=>Evidence(c,files),"FAKE_TRUST_BINDING");}});
        Test("fake-reject-stale-future-expired",()=>{foreach(int kind in new[]{0,1,2,3}){var(c,t,files)=Fixture();if(kind==0)t["ObservedUtc"]=Now.AddMinutes(-16);if(kind==1)t["ObservedUtc"]=Now.AddSeconds(1);if(kind==2)t["ExpiresUtc"]=Now;if(kind==3)t["ExpiresUtc"]=c.ExpiresUtc.AddSeconds(1);Seal(c,t,files);Reject(()=>Evidence(c,files),"FAKE_TRUST_STALE_OR_EXPIRED");}});
        Test("fake-cleanup-expired-proof-not-new-start",()=>{var(c,t,files)=Fixture();t["ObservedUtc"]=Now.AddMinutes(-20);t["ExpiresUtc"]=Now.AddMinutes(-1);Seal(c,t,files);Reject(()=>Evidence(c,files));Evidence(c,files,true);});
        Test("fake-missing-or-tampered-basis",()=>{var(c,_,files)=Fixture();files[c.TrialId+".fake-only-trust-basis.txt"]=[1];Reject(()=>Evidence(c,files),"FAKE_TRUST_BASIS");});
        Test("fake-trust-duplicate-unknown-and-global-pass",()=>{foreach(int kind in new[]{0,1,2}){var(c,t,files)=Fixture();if(kind==2){t["GlobalSidAbsence"]="PASS";Seal(c,t,files);Reject(()=>Evidence(c,files),"FAKE_GLOBAL_NOT_PASS");continue;}var key=c.TrialId+".fake-only-trust.json";var text=Encoding.UTF8.GetString(files[key]);text=text.Insert(1,kind==0?"\"Approved\":true,":"\"Unknown\":true,");files[key]=Encoding.UTF8.GetBytes(text);c.FakeOnlyIsolation!.TrustEvidenceSha256=Hash(files[key]);Reject(()=>Evidence(c,files),"FAKE_UNKNOWN_OR_DUPLICATE");}});
        Test("fake-admin-missing-proof-before-registration",()=>{var(c,_,_)=Fixture();var backend=new FakeBackend();Reject(()=>Execute(backend,"--register-only",c),"FAKE_TRUST_EVIDENCE_MISSING_HOLD");Check(backend.RegisterCalls==0&&backend.RunCalls==0&&backend.Records.Count==0);});
        Test("fake-admin-register-and-start-bound-to-profile",()=>{var(c,_,files)=Fixture();var backend=new FakeBackend();foreach(var p in files)backend.IsolationFiles.Add(p.Key,p.Value);Check(Execute(backend,"--register-only",c)==0&&backend.RegisterCalls==1);Check(Execute(backend,"--start-once",c)==0&&backend.RunCalls==1);});
        Test("fake-admin-start-missing-proof-does-not-consume",()=>{var(c,_,files)=Fixture();var backend=new FakeBackend();foreach(var p in files)backend.IsolationFiles.Add(p.Key,p.Value);Check(Execute(backend,"--register-only",c)==0);int records=backend.Records.Count;backend.IsolationFiles.Clear();Reject(()=>Execute(backend,"--start-once",c),"FAKE_TRUST_EVIDENCE_MISSING_HOLD");Check(backend.RunCalls==0&&backend.Records.Count==records);});
        Test("fake-old-operation-approval-not-reusable",()=>{var old=Contract();var(c,_,_)=Fixture();var backend=new FakeBackend();backend.Preflight(old,false);Reject(()=>OperationApproval.Validate(backend.Approval("--start-once",old.TrialId),c.TrialId,Hash(Raw(c)),"--start-once",Now,false),"OPERATION_APPROVAL_BINDING");});
        Console.WriteLine($"FAKE_ONLY_CONTRACT: {passed} PASS / {failed} FAIL; LIVE_EXECUTION=0; GLOBAL_SID_ABSENCE=UNKNOWN");return failed==0?0:1;
    }
}

using System.Text.Json;

internal static partial class AdmissionTests
{
    private static string[] EntryArgs(string mode,TrialContract c)
    {
        string[] common=[mode,"--contract",Path.Combine(Binding.CodeRoot,"TrialContract.json"),"--contract-sha",Hash(Raw(c))];
        return mode=="--register-only"?[..common,"--password-console"]:common;
    }
    private static int EntryGateDelta()
    {
        // Main 대신 동일한 분기/승인 코드를 메모리에서만 검증합니다. Windows backend는 만들지 않습니다.
        Test("entry-dispatch-admin-register",()=>{var c=Candidate();var f=Trusted(c);Check(TrialEntryGate.Dispatch(EntryArgs("--register-only",c),Binding,()=>NativeAdmin(f,"--register-only",c),()=>throw new Exception("WRONG_ROUTE"))==0);Check(f.RegisterCalls==1);});
        Test("entry-supervisor-full-read-validation-before-mock-effects",()=>{var c=Candidate();var f=Trusted(c);f.Preflight(c,false);int effects=0;Check(TrialEntryGate.Dispatch(EntryArgs("--supervise",c),Binding,()=>throw new Exception("WRONG_ROUTE"),()=>{ValidateEntry(c,f,Grant(c));effects++;return 0;})==0);Check(effects==1);});
        foreach(string mode in new[]{"--force","--deploy","LIVE","--register-only --force"})
            Test("entry-deny-mode-"+mode,()=>{var c=Candidate();int n=0;Reject(()=>TrialEntryGate.Dispatch(EntryArgs(mode,c),Binding,()=>++n,()=>++n));Check(n==0);});
        Test("entry-deny-extra-arguments-no-route",()=>{var c=Candidate();int n=0;Reject(()=>TrialEntryGate.Dispatch([..EntryArgs("--supervise",c),"--force"],Binding,()=>++n,()=>++n));Check(n==0);});
        Test("entry-deny-contract-path-no-route",()=>{var c=Candidate();var args=EntryArgs("--supervise",c);args[2]="OTHER";int n=0;Reject(()=>TrialEntryGate.Dispatch(args,Binding,()=>++n,()=>++n));Check(n==0);});
        foreach(var change in new (string,Action<TrialContract>)[]{("other-trial",c=>c.TrialId="1234567890abcdef1234567890abcdef"),("profile",c=>c.FakeOnlyIsolation!.Profile="LIVE"),("expired",c=>c.ExpiresUtc=Now),("unapproved",c=>c.Approved=false),("start-scope",c=>c.Scope!.Start=false),("ipc",c=>c.Scope!.NoIpc=false)})
            Test("entry-supervisor-deny-"+change.Item1+"-no-effect",()=>{var c=Candidate();var f=Trusted(c);change.Item2(c);int n=0;Reject(()=>{ValidateEntry(c,f,Grant(c));n++;});Check(n==0);});
        Test("entry-supervisor-tampered-contract-sha",()=>{var c=Candidate();var f=Trusted(c);f.Preflight(c,false);Reject(()=>TrialEntryGate.Validate(Raw(c),new('d',64),"--supervise",JsonSerializer.SerializeToUtf8Bytes(Grant(c),ApprovalJson.Default.OperationApproval),Binding,()=>f.Now,f.IsolationEvidence,x=>f.Preflight(x,false),f.Account));});
        foreach(int kind in new[]{0,1,2,3})Test("entry-supervisor-operation-deny-"+kind,()=>{var c=Candidate();var f=Trusted(c);var g=Grant(c);if(kind==0)g.Operation="--register-only";if(kind==1)g.ExpiresUtc=Now;if(kind==2)g.ContractSha=new('d',64);if(kind==3)g.ApprovalId=new('0',32);int n=0;Reject(()=>{ValidateEntry(c,f,g);n++;});Check(n==0);});
        Test("entry-supervisor-trust-tamper-no-effect",()=>{var c=Candidate();var f=Trusted(c);f.IsolationFiles[c.TrialId+".fake-only-trust.json"][0]=0;Reject(()=>ValidateEntry(c,f,Grant(c)));});
        Test("entry-supervisor-image-check-failure-no-effect",()=>{var c=Candidate();var f=Trusted(c);f.OnPreflight=_=>throw new IOException("IMAGE_SHA");Reject(()=>ValidateEntry(c,f,Grant(c)));});
        Test("entry-supervisor-account-deny-no-effect",()=>{var c=Candidate();var f=Trusted(c);f.AccountState=f.AccountState with{BatchAllowed=false};Reject(()=>ValidateEntry(c,f,Grant(c)));});
        Test("entry-supervisor-expiry-during-read-no-effect",()=>{var c=Candidate();var f=Trusted(c);f.OnPreflight=_=>f.Clock=c.ExpiresUtc;Reject(()=>ValidateEntry(c,f,Grant(c)));});
        Test("entry-supervisor-trust-changed-during-read-no-effect",()=>{var c=Candidate();var f=Trusted(c);f.OnPreflight=_=>f.IsolationFiles[c.TrialId+".fake-only-trust.json"][0]=0;Reject(()=>ValidateEntry(c,f,Grant(c)));});
        Test("entry-register-expired-before-intent-no-record",()=>{var c=Candidate();var f=Trusted(c);int preflights=0;f.OnPreflight=_=>{if(++preflights==2)f.Clock=c.ExpiresUtc;};Reject(()=>NativeAdmin(f,"--register-only",c));Check(f.RegisterCalls==0&&f.Records.Count==0);});
        Test("entry-register-contract-swap-before-intent-no-record",()=>{var c=Candidate();var f=Trusted(c);f.CurrentContractOverride=()=>[0];Reject(()=>NativeAdmin(f,"--register-only",c));Check(f.RegisterCalls==0&&f.Records.Count==0);});
        Test("entry-start-register-approval-cannot-start",()=>{var c=Candidate();var f=Trusted(c);NativeAdmin(f,"--register-only",c);int before=f.Records.Count;f.ApprovalOperation="--register-only";Reject(()=>NativeAdmin(f,"--start-once",c));Check(f.Records.Count==before&&f.RunCalls==0);});
        Test("entry-start-duplicate-consumption-not-reset",()=>{var c=Candidate();var f=Trusted(c);NativeAdmin(f,"--register-only",c);NativeAdmin(f,"--start-once",c);f.Instances=[];Reject(()=>NativeAdmin(f,"--start-once",c));Check(f.RunCalls==1&&f.Records.Keys.Count(k=>k.EndsWith("start-intent"))==1);});
        Test("entry-cleanup-needs-distinct-operation",()=>{var c=Candidate();var f=Trusted(c);NativeAdmin(f,"--register-only",c);f.ApprovalOperation="--start-once";int before=f.Records.Count;Reject(()=>NativeAdmin(f,"--remove-owned-task",c));Check(f.RemoveCalls==0&&f.Records.Count==before);});
        Test("entry-cleanup-valid-own-task-only",()=>{var c=Candidate();var f=Trusted(c);NativeAdmin(f,"--register-only",c);Check(NativeAdmin(f,"--remove-owned-task",c)==0&&f.RemoveCalls==1);});
        Test("entry-supervisor-duplicate-memory-marker-no-reset",()=>{var c=Candidate();var f=Trusted(c);var markers=new HashSet<string>();int n=0;void Consume(){ValidateEntry(c,f,Grant(c));using var lease=AdmissionConsumption.Acquire(Binding.EvidenceRoot,Raw(c),path=>{if(!markers.Add(path))throw new IOException("CONSUMED");return new MemoryStream();});n++;}Consume();Reject(Consume);Check(n==1&&markers.Count==1);});
        Test("entry-cutoff-denies-cleanup",()=>{var c=Candidate();var f=Trusted(c);f.Clock=AdmissionPolicy.Deadline.AddTicks(1);Reject(()=>NativeAdmin(f,"--remove-owned-task",c));Check(f.Records.Count==0&&f.RemoveCalls==0);});
        Test("entry-cancel-during-read-no-effect",()=>{var c=Candidate();var f=Trusted(c);using var cancel=new CancellationTokenSource();Reject(()=>TrialEntryGate.Validate(Raw(c),Hash(Raw(c)),"--supervise",JsonSerializer.SerializeToUtf8Bytes(Grant(c),ApprovalJson.Default.OperationApproval),Binding,()=>f.Now,f.IsolationEvidence,x=>cancel.Cancel(),f.Account,cancel.Token));});
        Test("entry-start-expired-before-consumption-no-record",()=>{var c=Candidate();var f=Trusted(c);NativeAdmin(f,"--register-only",c);int n=0,before=f.Records.Count;f.OnPreflight=_=>{if(++n==2)f.Clock=c.ExpiresUtc;};Reject(()=>NativeAdmin(f,"--start-once",c));Check(f.Records.Count==before&&f.EnableCalls==0&&f.RunCalls==0);});
        Test("entry-cleanup-expired-before-intent-no-record",()=>{var c=Candidate();var f=Trusted(c);NativeAdmin(f,"--register-only",c);int n=0,before=f.Records.Count;f.OnPreflight=_=>{if(++n==2)f.Clock=c.ExpiresUtc;};Reject(()=>NativeAdmin(f,"--remove-owned-task",c));Check(f.Records.Count==before&&f.RemoveCalls==0);});
        Test("entry-run-failure-keeps-consumption-and-safe-disables",()=>{var c=Candidate();var f=Trusted(c);NativeAdmin(f,"--register-only",c);f.FailRun=true;Reject(()=>NativeAdmin(f,"--start-once",c));Check(!f.Enabled&&f.RunCalls==1&&f.Records.Keys.Any(k=>k.EndsWith("start-intent")));f.Instances=[];Reject(()=>NativeAdmin(f,"--start-once",c));Check(f.RunCalls==1);});
        Console.WriteLine($"ENTRY_GATE_DELTA: {passed} PASS / {failed} FAIL; NATIVE_MAIN=0; WINDOWS_MUTATIONS=0; REAL_CONSUMPTION=0; NATIVEAOT_BUILDS=0");
        return failed==0?0:1;
    }
    private static void ValidateEntry(TrialContract c,FakeBackend f,OperationApproval grant)
        =>TrialEntryGate.Validate(Raw(c),Hash(Raw(c)),"--supervise",JsonSerializer.SerializeToUtf8Bytes(grant,ApprovalJson.Default.OperationApproval),Binding,
            ()=>f.Now,f.IsolationEvidence,x=>f.Preflight(x,false),f.Account);
}

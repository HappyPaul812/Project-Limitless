using System.Text;

// 실제 암호·콘솔·COM·계정·ACL을 사용하지 않고 동일 변경 경계에 입력/시각 변화만 주입합니다.
internal static partial class AdmissionTests
{
    private static int TaskMutationLateReadDelta()
    {
        Test("mutation-register-expiry-during-image-account-refresh",()=>{var c=Candidate();var f=Trusted(c);f.BeforeRegister=()=>f.OnPreflight=_=>f.Clock=c.ExpiresUtc.AddSeconds(1);Reject(()=>NativeAdmin(f,"--register-only",c));Check(f.RegisterCalls==0);});
        Test("mutation-register-trust-change-between-password-and-COM",()=>{var c=Candidate();var f=Trusted(c);f.BeforeRegisterCommit=()=>f.IsolationFiles[c.TrialId+".fake-only-trust.json"][0]=0;Reject(()=>NativeAdmin(f,"--register-only",c));Check(f.RegisterCalls==0);});
        Test("mutation-register-cancel-during-final-refresh",()=>{var c=Candidate();var f=Trusted(c);using var cancel=new CancellationTokenSource();f.BeforeRegisterCommit=()=>f.OnPreflight=_=>cancel.Cancel();Reject(()=>NativeAdmin(f,"--register-only",c,cancellation:cancel.Token));Check(f.RegisterCalls==0);});
        Console.WriteLine($"TASK_MUTATION_LATE_READ_DELTA: {passed} PASS / {failed} FAIL; NATIVE_MAIN=0; REAL_WINDOWS_MUTATIONS=0; PASSWORD_INPUT=0");return failed==0?0:1;
    }
    private static int TaskMutationDelta()
    {
        RestrictedGateExpiryRisk(); // 기존 FAIL1만 표적으로 다시 확인합니다. 기존58개는 호출하지 않습니다.
        Test("mutation-register-valid-delay",()=>{var c=Candidate();var f=Trusted(c);f.BeforeRegister=()=>f.Clock=Now.AddSeconds(1);Check(NativeAdmin(f,"--register-only",c)==0&&f.RegisterCalls==1);});
        Test("mutation-register-expiry-between-password-and-COM",()=>{var c=Candidate();var f=Trusted(c);f.BeforeRegisterCommit=()=>f.Clock=c.ExpiresUtc.AddSeconds(1);Reject(()=>NativeAdmin(f,"--register-only",c));Check(f.RegisterCalls==0&&!f.Present&&f.Records.Values.Any(r=>r.Status=="HOLD"));});
        Test("mutation-register-cutoff-during-password",()=>{var c=Candidate();c.ExpiresUtc=AdmissionPolicy.Deadline;var f=Trusted(c);f.BeforeRegister=()=>f.Clock=AdmissionPolicy.Deadline.AddSeconds(1);Reject(()=>NativeAdmin(f,"--register-only",c));Check(f.RegisterCalls==0);});
        Test("mutation-register-operation-expiry-during-password",()=>{var c=Candidate();var f=Trusted(c);f.GrantExpiry=Now.AddMinutes(1);f.BeforeRegister=()=>f.Clock=Now.AddMinutes(2);Reject(()=>NativeAdmin(f,"--register-only",c));Check(f.RegisterCalls==0);});
        Test("mutation-register-stale-trust-during-password",()=>{var c=Candidate();var f=Trusted(c);f.BeforeRegister=()=>f.Clock=Now.AddMinutes(16);Reject(()=>NativeAdmin(f,"--register-only",c));Check(f.RegisterCalls==0);});
        Test("mutation-register-cancel-during-password",()=>{var c=Candidate();var f=Trusted(c);using var cancel=new CancellationTokenSource();f.BeforeRegister=cancel.Cancel;Reject(()=>NativeAdmin(f,"--register-only",c,cancellation:cancel.Token));Check(f.RegisterCalls==0);});
        Test("mutation-register-cancel-before-COM",()=>{var c=Candidate();var f=Trusted(c);using var cancel=new CancellationTokenSource();f.BeforeRegisterCommit=cancel.Cancel;Reject(()=>NativeAdmin(f,"--register-only",c,cancellation:cancel.Token));Check(f.RegisterCalls==0);});
        foreach(string change in new[]{"contract","trial","operation","trust","missing-trust","account","image"})
            Test("mutation-register-revalidate-"+change,()=>{
                var c=Candidate();var f=Trusted(c);f.BeforeRegister=()=>{
                    if(change is "contract" or "trial"){var other=Candidate();if(change=="trial")other.TrialId="1234567890abcdef1234567890abcdef";else other.ProbeSha256=new('e',64);f.CurrentContractOverride=()=>Raw(other);}
                    if(change=="operation")f.ApprovalOperation="--start-once";
                    if(change=="trust")f.IsolationFiles[c.TrialId+".fake-only-trust.json"][0]=0;
                    if(change=="missing-trust")f.IsolationFiles.Clear();
                    if(change=="account")f.AccountState=f.AccountState with{Enabled=false};
                    if(change=="image")f.OnPreflight=_=>throw new IOException("IMAGE_SHA_HOLD");
                };Reject(()=>NativeAdmin(f,"--register-only",c));Check(f.RegisterCalls==0&&!f.Present);
            });
        Test("mutation-register-new-task-during-password-not-overwritten",()=>{var c=Candidate();var f=Trusted(c);string xml=TaskPolicy.Xml(c,Hash(Raw(c)),Binding);f.BeforeRegisterCommit=()=>{f.Present=true;f.Xml=xml;};Reject(()=>NativeAdmin(f,"--register-only",c));Check(f.RegisterCalls==0&&f.Present&&f.Xml==xml);});
        Test("mutation-register-after-password-authority-loss-before-COM",()=>{var c=Candidate();var f=Trusted(c);f.BeforeRegisterCommit=()=>f.OnPreflight=_=>throw new IOException("ADMIN_IDENTITY_OR_PATH_HOLD");Reject(()=>NativeAdmin(f,"--register-only",c));Check(f.RegisterCalls==0);});
        Test("mutation-register-original-intent-remains-no-repeat",()=>{var c=Candidate();var f=Trusted(c);f.BeforeRegister=()=>f.Clock=c.ExpiresUtc.AddSeconds(1);Reject(()=>NativeAdmin(f,"--register-only",c));int count=f.Records.Count;Check(f.Records.Values.Any(r=>r.Status=="REGISTER_INTENT")&&f.Records.Values.Any(r=>r.Status=="HOLD"));f.Clock=Now;f.BeforeRegister=null;Reject(()=>NativeAdmin(f,"--register-only",c));Check(f.RegisterCalls==0&&f.Records.Count>=count);});
        Test("mutation-enable-expired-no-task-change",()=>{var c=Candidate();var f=Trusted(c);NativeAdmin(f,"--register-only",c);f.BeforeMutation=m=>{if(m==TaskMutation.Enable)f.Clock=c.ExpiresUtc.AddSeconds(1);};Reject(()=>NativeAdmin(f,"--start-once",c));Check(!f.Enabled&&f.EnableCalls==0&&f.DisableCalls==0&&f.RunCalls==0);});
        Test("mutation-run-expired-still-safely-disables",()=>{var c=Candidate();var f=Trusted(c);NativeAdmin(f,"--register-only",c);f.BeforeMutation=m=>{if(m==TaskMutation.Run)f.Clock=c.ExpiresUtc.AddSeconds(1);};Reject(()=>NativeAdmin(f,"--start-once",c));Check(!f.Enabled&&f.EnableCalls==1&&f.DisableCalls==1&&f.RunCalls==0);});
        Test("mutation-run-cancel-still-safely-disables",()=>{var c=Candidate();var f=Trusted(c);NativeAdmin(f,"--register-only",c);using var cancel=new CancellationTokenSource();f.BeforeMutation=m=>{if(m==TaskMutation.Run)cancel.Cancel();};Reject(()=>NativeAdmin(f,"--start-once",c,cancellation:cancel.Token));Check(!f.Enabled&&f.DisableCalls==1&&f.RunCalls==0);});
        Test("mutation-run-trust-loss-still-safely-disables",()=>{var c=Candidate();var f=Trusted(c);NativeAdmin(f,"--register-only",c);f.BeforeMutation=m=>{if(m==TaskMutation.Run)f.IsolationFiles.Clear();};Reject(()=>NativeAdmin(f,"--start-once",c));Check(!f.Enabled&&f.DisableCalls==1&&f.RunCalls==0);});
        Test("mutation-run-response-loss-consumed-and-safely-disabled",()=>{var c=Candidate();var f=Trusted(c);NativeAdmin(f,"--register-only",c);f.FailRun=true;Reject(()=>NativeAdmin(f,"--start-once",c));Check(!f.Enabled&&f.DisableCalls==1&&f.RunCalls==1&&f.Records.Values.Any(r=>r.Status=="START_CONSUMED"));f.Instances=[];Reject(()=>NativeAdmin(f,"--start-once",c));Check(f.RunCalls==1);});
        Test("mutation-stop-expired-instance-unchanged",()=>{var c=Candidate();var f=Trusted(c);NativeAdmin(f,"--register-only",c);NativeAdmin(f,"--start-once",c);Guid id=f.Instances.Single();f.BeforeMutation=m=>{if(m==TaskMutation.Stop)f.Clock=c.ExpiresUtc.AddSeconds(1);};Reject(()=>NativeAdmin(f,"--stop-owned-instance",c,id));Check(f.StopCalls==0&&f.Instances.SequenceEqual(new[]{id}));});
        Test("mutation-stop-wrong-instance-during-query-denied",()=>{var c=Candidate();var f=Trusted(c);NativeAdmin(f,"--register-only",c);NativeAdmin(f,"--start-once",c);Guid id=f.Instances.Single();f.BeforeMutation=m=>{if(m==TaskMutation.Stop)f.Instances=[Guid.NewGuid()];};Reject(()=>NativeAdmin(f,"--stop-owned-instance",c,id));Check(f.StopCalls==0);});
        Test("mutation-stop-fresh-approval-disabled-account-cleanup",()=>{var c=Candidate();var f=Trusted(c);NativeAdmin(f,"--register-only",c);NativeAdmin(f,"--start-once",c);Guid id=f.Instances.Single();f.AccountState=f.AccountState with{Enabled=false};Check(NativeAdmin(f,"--stop-owned-instance",c,id)==0&&f.StopCalls==1&&f.Instances.Length==0);});
        Test("mutation-remove-expired-task-retained",()=>{var c=Candidate();var f=Trusted(c);NativeAdmin(f,"--register-only",c);f.BeforeMutation=m=>{if(m==TaskMutation.Remove)f.Clock=c.ExpiresUtc.AddSeconds(1);};Reject(()=>NativeAdmin(f,"--remove-owned-task",c));Check(f.Present&&f.RemoveCalls==0);});
        Test("mutation-remove-task-changed-not-deleted",()=>{var c=Candidate();var f=Trusted(c);NativeAdmin(f,"--register-only",c);f.BeforeMutation=m=>{if(m==TaskMutation.Remove)f.Xml=f.Xml.Replace("PT90S","PT91S");};Reject(()=>NativeAdmin(f,"--remove-owned-task",c));Check(f.Present&&f.RemoveCalls==0);});
        Test("mutation-remove-fresh-exact-idle-task",()=>{var c=Candidate();var f=Trusted(c);NativeAdmin(f,"--register-only",c);Check(NativeAdmin(f,"--remove-owned-task",c)==0&&!f.Present&&f.RemoveCalls==1&&f.Records.Values.Any(r=>r.Status=="REMOVED_EVIDENCE_RETAINED"));});
        Test("mutation-safety-disable-cannot-be-new-authorization",()=>{var c=Candidate();var f=Trusted(c);NativeAdmin(f,"--register-only",c);Reject(()=>f.Enable(false));Check(f.DisableCalls==0&&f.RunCalls==0);});
        Test("mutation-valid-register-start-still-once",()=>{var c=Candidate();var f=Trusted(c);Check(NativeAdmin(f,"--register-only",c)==0);Check(NativeAdmin(f,"--start-once",c)==0&&f.RunCalls==1&&!f.Enabled);f.Instances=[];Reject(()=>NativeAdmin(f,"--start-once",c));Check(f.RunCalls==1);});
        Console.WriteLine($"TASK_MUTATION_DELTA: {passed} PASS / {failed} FAIL; NATIVE_MAIN=0; REAL_WINDOWS_MUTATIONS=0; PASSWORD_INPUT=0");
        return failed==0?0:1;
    }
}

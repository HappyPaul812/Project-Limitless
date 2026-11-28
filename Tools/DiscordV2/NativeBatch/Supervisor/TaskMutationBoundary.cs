using System.Security.Cryptography;
using System.Text.Json;

internal enum TaskMutation { Register, Enable, Run, Stop, Remove, SafetyDisable }

// 관리자 진입 때의 승인만 재사용하지 않고 실제 변경 호출 바로 앞에서 보호 입력을 다시 읽습니다.
// 시간/계약/신뢰 검증은 실제 backend와 모의 backend가 같은 코드로 수행합니다.
internal sealed class TaskMutationBoundary
{
    private readonly string mode,sha,approvalSha;
    private readonly AdmissionBinding binding;
    private readonly IAdminBackend backend;
    private readonly CancellationToken cancellation;
    private readonly TrialContract original;
    private readonly CurrentFakeOnlyContract.ExecutionWindow? window;
    private readonly DateTimeOffset operationExpiry;
    private bool enableLease,runAttempted,registerAttempted;
    private string ownedFingerprint="";
    internal TaskMutationBoundary(string mode,string sha,byte[] raw,byte[] operationRaw,AdmissionBinding binding,IAdminBackend backend,CancellationToken cancellation,CurrentFakeOnlyContract.ExecutionWindow? window=null)
    {
        this.mode=mode;this.sha=sha;this.binding=binding;this.backend=backend;this.cancellation=cancellation;this.window=window;
        original=JsonSerializer.Deserialize(raw,ContractJson.Default.TrialContract)!;approvalSha=Hash(operationRaw);
        operationExpiry=(JsonSerializer.Deserialize(operationRaw,ApprovalJson.Default.OperationApproval)
            ?? throw new InvalidOperationException("OPERATION_APPROVAL_NULL")).ExpiresUtc;
    }
    private static string Hash(byte[] value)=>Convert.ToHexString(SHA256.HashData(value)).ToLowerInvariant();
    // 암호를 기다리는 동안 고정된 승인 시간만 확인합니다. COM 호출 전 전체 재검증은 유지합니다.
    internal void CheckPasswordWindow()
    {
        cancellation.ThrowIfCancellationRequested();
        AdmissionPolicy.Require(mode=="--register-only","MUTATION_MODE_MISMATCH");
        RequirePasswordWindow(backend.Now,original.ExpiresUtc,operationExpiry,window);
    }
    internal static void RequirePasswordWindow(DateTimeOffset now,DateTimeOffset trialExpiry,
        DateTimeOffset approvalExpiry,CurrentFakeOnlyContract.ExecutionWindow? window)
    {
        bool valid=window is null
            ? now<=AdmissionPolicy.Deadline && approvalExpiry<=AdmissionPolicy.Deadline
            : window.Allows(now,approvalExpiry);
        AdmissionPolicy.Require(valid && now<approvalExpiry && approvalExpiry<=trialExpiry,
            "REGISTER_PASSWORD_WINDOW_HOLD");
    }
    internal void AfterPassword()=>Validate(TaskMutation.Register,null,false);
    internal void Check(TaskMutation mutation,Guid? instance=null)=>Validate(mutation,instance,true);
    private ControlReceipt Receipt(string suffix)
    {
        var r=backend.Receipt(original.TrialId+".admin."+suffix)??throw new InvalidOperationException("MUTATION_RECEIPT_MISSING");
        AdmissionPolicy.Require(r.TrialId==original.TrialId&&r.ContractSha==sha,"MUTATION_RECEIPT_BINDING");return r;
    }
    private string Owned(TaskObservation observed)
    {
        var registered=Receipt("registered");string fingerprint=backend.TaskFingerprint(observed,original,sha,binding);
        AdmissionPolicy.Require(registered.Status=="REGISTERED_DISABLED"&&registered.Fingerprint==fingerprint,"MUTATION_TASK_CHANGED");return fingerprint;
    }
    private void Validate(TaskMutation mutation,Guid? instance,bool commit)
    {
        if(mutation==TaskMutation.SafetyDisable)
        {
            // 유효한 시작 승인으로 자기 Task를 enable하려던 경우에만 허용합니다.
            // 취소/만료/신뢰자료 유실이 자기 Task 재비활성화를 막지는 않습니다.
            AdmissionPolicy.Require(mode=="--start-once"&&enableLease,"MUTATION_SAFETY_LEASE_MISSING");
            var observed=backend.Observe()??throw new InvalidOperationException("MUTATION_TASK_MISSING");
            AdmissionPolicy.Require(Owned(observed)==ownedFingerprint,"MUTATION_SAFETY_TASK_CHANGED");return;
        }
        cancellation.ThrowIfCancellationRequested();
        string expected=mutation switch {TaskMutation.Register=>"--register-only",TaskMutation.Enable or TaskMutation.Run=>"--start-once",TaskMutation.Stop=>"--stop-owned-instance",TaskMutation.Remove=>"--remove-owned-task",_=>""};
        AdmissionPolicy.Require(mode==expected,"MUTATION_MODE_MISMATCH");
        byte[] raw=backend.CurrentContract();
        AdmissionPolicy.Require(Hash(raw)==sha,"MUTATION_CONTRACT_CHANGED");
        AdmissionPolicy.Contract(raw,backend.Now,binding,window:window); // cleanup도 새 관리자 변경은 현재 만료/cutoff를 검사합니다.
        var current=JsonSerializer.Deserialize(raw,ContractJson.Default.TrialContract)!;
        AdmissionPolicy.Require(current.TrialId==original.TrialId,"MUTATION_TRIAL_CHANGED");
        NativeTrialPolicy.Shape(current,mode,backend.Now,window);
        byte[] operationRaw=backend.Approval(mode,current.TrialId);
        AdmissionPolicy.Require(Hash(operationRaw)==approvalSha,"MUTATION_APPROVAL_CHANGED");
        var operation=OperationApproval.Validate(operationRaw,current.TrialId,sha,mode,backend.Now,false,window);
        FakeOnlyPolicy.Evidence(current,backend.Now,backend.IsolationEvidence,window:window);
        backend.Preflight(current,mode is "--stop-owned-instance" or "--remove-owned-task"); // 경로·두 이미지 SHA·실제 계정 재조회
        var account=backend.Account();
        var task=backend.Observe();
        string fingerprintForEnable="";
        if(mutation==TaskMutation.Register)
        {
            AdmissionPolicy.Require(task is null&&!registerAttempted,"MUTATION_REGISTER_NOT_ABSENT");
            AdmissionPolicy.Require(NativeTrialPolicy.AccountAllowed(account,current.ExpiresUtc),"MUTATION_REGISTER_ACCOUNT_HOLD");
        }
        else
        {
            AdmissionPolicy.Require(task is not null,"MUTATION_TASK_MISSING");
            string fingerprint=Owned(task!);
            if(mutation is TaskMutation.Enable or TaskMutation.Run)
            {
                var intent=Receipt("start-intent");ReceiptPolicy.Start(Receipt("registered"),intent,operation,current.TrialId,sha);
                AdmissionPolicy.Require(task!.Instances.Length==0&&!runAttempted,"MUTATION_START_NOT_IDLE");
                AdmissionPolicy.Require(mutation==TaskMutation.Enable?!task.Enabled&&!enableLease:task.Enabled&&enableLease&&ownedFingerprint==fingerprint,"MUTATION_START_PHASE");
                fingerprintForEnable=fingerprint;
            }
            if(mutation==TaskMutation.Stop)
            {
                var started=Receipt("instance");
                AdmissionPolicy.Require(started.Status=="INSTANCE_RETURNED"&&started.Fingerprint==fingerprint&&instance.HasValue
                    && instance.Value.ToString("D")==started.Instance&&task!.Instances.Length==1&&task.Instances[0]==instance.Value,"MUTATION_STOP_INSTANCE");
            }
            if(mutation==TaskMutation.Remove)
            {
                AdmissionPolicy.Require(!task!.Enabled&&task.Instances.Length==0,"MUTATION_REMOVE_NOT_IDLE");
                if(backend.Receipt(current.TrialId+".admin.start-intent") is not null)
                    AdmissionPolicy.Require(backend.CleanupProven(current.TrialId),"MUTATION_CLEANUP_UNKNOWN");
            }
        }
        // 고비용 읽기/대기 뒤 시각을 다시 취합니다. 이 검사 실패 시 아래 실제 변경 호출은 하지 않습니다.
        FakeOnlyPolicy.Evidence(current,backend.Now,backend.IsolationEvidence,window:window);
        NativeTrialPolicy.Authorize(current,sha,mode,operation,account,backend.Now,binding,window);
        cancellation.ThrowIfCancellationRequested();
        if(!commit)return;
        if(mutation==TaskMutation.Enable){enableLease=true;ownedFingerprint=fingerprintForEnable;}
        if(mutation==TaskMutation.Run)runAttempted=true; // 응답 유실도 재시도 권한으로 바뀌지 않습니다.
        if(mutation==TaskMutation.Register)registerAttempted=true;
    }
}

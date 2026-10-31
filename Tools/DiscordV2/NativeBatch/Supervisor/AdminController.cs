using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

internal sealed class ControlReceipt
{
    public string TrialId { get; set; } = "";
    public string ContractSha { get; set; } = "";
    public string Fingerprint { get; set; } = "";
    public string Instance { get; set; } = "";
    public string Status { get; set; } = "";
    public string ApprovalId { get; set; } = "";
    internal static ControlReceipt Parse(byte[] bytes)
    {
        using var doc=System.Text.Json.JsonDocument.Parse(bytes);
        AdmissionPolicy.Require(doc.RootElement.ValueKind==System.Text.Json.JsonValueKind.Object,"RECEIPT_OBJECT");
        string[] fields=["TrialId","ContractSha","Fingerprint","Instance","Status","ApprovalId"];var seen=new HashSet<string>();
        foreach(var p in doc.RootElement.EnumerateObject())AdmissionPolicy.Require(fields.Contains(p.Name,StringComparer.Ordinal)&&seen.Add(p.Name),"RECEIPT_DUPLICATE_OR_UNKNOWN");
        AdmissionPolicy.Require(seen.Count==fields.Length,"RECEIPT_MISSING");
        return System.Text.Json.JsonSerializer.Deserialize(bytes,ControlJson.Default.ControlReceipt)??throw new InvalidOperationException("RECEIPT_NULL");
    }
}
internal static class ReceiptPolicy
{
    internal static void Start(ControlReceipt registered,ControlReceipt intent,OperationApproval approval,string trial,string sha)
    {
        AdmissionPolicy.Require(registered.TrialId==trial&&intent.TrialId==trial&&registered.ContractSha==sha&&intent.ContractSha==sha,"WORKER_RECEIPT_BINDING");
        AdmissionPolicy.Require(registered.Status=="REGISTERED_DISABLED"&&intent.Status=="START_CONSUMED"&&AdmissionPolicy.Sha(registered.Fingerprint)&&intent.Fingerprint==registered.Fingerprint&&intent.Instance=="","WORKER_START_INTENT");
        AdmissionPolicy.Require(intent.ApprovalId==approval.ApprovalId,"WORKER_START_APPROVAL_ID");
    }
    internal static void Instance(ControlReceipt instance,ControlReceipt intent)
    {
        AdmissionPolicy.Require(instance.Status=="INSTANCE_RETURNED"&&instance.TrialId==intent.TrialId&&instance.ContractSha==intent.ContractSha&&instance.Fingerprint==intent.Fingerprint&&instance.ApprovalId==intent.ApprovalId&&Guid.TryParseExact(instance.Instance,"D",out var id)&&id!=Guid.Empty,"WORKER_INSTANCE_BINDING");
    }
}
[JsonSerializable(typeof(ControlReceipt))]
internal partial class ControlJson : JsonSerializerContext { }
internal sealed record TaskObservation(string Xml, string Sddl, bool Enabled, Guid[] Instances, int State=1);
internal sealed record AccountObservation(string Sid, bool Enabled, bool Locked, DateTimeOffset ExpiresUtc, bool BatchKnown, bool BatchAllowed);

// 실제 제어 흐름을 모의 시험에도 그대로 사용합니다. Windows 구현을 호출하는 별도 시험 진입점은 없습니다.
internal interface IAdminBackend
{
    DateTimeOffset Now { get; }
    void Preflight(TrialContract contract, bool cleanup);
    byte[] IsolationEvidence(string name,int limit)=>throw new InvalidOperationException("FAKE_TRUST_EVIDENCE_MISSING_HOLD");
    byte[] Approval(string operation,string trialId);
    byte[] CurrentContract()=>throw new InvalidOperationException("CURRENT_CONTRACT_REQUIRED");
    void BindMutationBoundary(TaskMutationBoundary boundary)=>throw new InvalidOperationException("MUTATION_BOUNDARY_REQUIRED");
    AccountObservation Account();
    TaskObservation? Observe();
    ControlReceipt? Receipt(string name);
    void Save(string name, ControlReceipt receipt); // CREATE_NEW + flush, 덮어쓰기/삭제 없음
    void Register(string xml, string sddl, CancellationToken cancellation);
    void Enable(bool enabled);
    Guid Run();
    void Stop(Guid instance);
    bool CleanupProven(string trialId);
    void Remove();
}

internal static class AdminController
{
    internal static bool IsMode(string mode) => mode is "--register-only" or "--start-once" or "--stop-owned-instance" or "--remove-owned-task";
    internal static int Execute(string mode, byte[] raw, string sha, AdmissionBinding binding, IAdminBackend backend, Guid? instance, CancellationToken cancellation)
        => ExecuteCore(mode,raw,sha,binding,backend,instance,cancellation,null,null);
    // 실제 Windows 어댑터는 이 진입점만 사용합니다. CLI/환경변수로 모의 경로를 선택할 수 없습니다.
    internal static int ExecuteNative(string mode,byte[] raw,string sha,AdmissionBinding binding,IAdminBackend backend,Guid? instance,CancellationToken cancellation)
        => ExecuteCore(mode,raw,sha,binding,backend,instance,cancellation,NativeTrialPolicy.Shape,NativeTrialPolicy.Authorize);
    private static int ExecuteCore(string mode,byte[] raw,string sha,AdmissionBinding binding,IAdminBackend backend,Guid? instance,CancellationToken cancellation,
        Action<TrialContract,string,DateTimeOffset>? shape,Action<TrialContract,string,string,OperationApproval,AccountObservation,DateTimeOffset,AdmissionBinding>? authorize)
    {
        AdmissionPolicy.Require(IsMode(mode), "ADMIN_MODE");
        bool cleanup = mode is "--stop-owned-instance" or "--remove-owned-task";
        AdmissionPolicy.Require(AdmissionPolicy.Sha(sha) && Convert.ToHexString(SHA256.HashData(raw)).ToLowerInvariant() == sha, "CONTROL_CONTRACT_SHA");
        AdmissionPolicy.Contract(raw, backend.Now, binding, cleanup);
        var contract = System.Text.Json.JsonSerializer.Deserialize(raw, ContractJson.Default.TrialContract)!;
        var scope = contract.Scope!;
        shape?.Invoke(contract,mode,backend.Now);
        AdmissionPolicy.Require(mode == "--register-only" ? scope.Register : mode == "--start-once" ? scope.Start : scope.Cleanup, "OPERATION_NOT_APPROVED");
        cancellation.ThrowIfCancellationRequested();
        FakeOnlyPolicy.Evidence(contract,backend.Now,backend.IsolationEvidence,cleanup);
        backend.Preflight(contract, cleanup);
        byte[] operationRaw=backend.Approval(mode,contract.TrialId);
        var approval=OperationApproval.Validate(operationRaw,contract.TrialId,sha,mode,backend.Now,cleanup);
        var account = backend.Account();
        AdmissionPolicy.Require(account.Sid == binding.WorkerSid, "ACCOUNT_SID_CHANGED");
        if (!cleanup)
            AdmissionPolicy.Require(account.Enabled && !account.Locked && account.ExpiresUtc >= contract.ExpiresUtc && account.BatchKnown && account.BatchAllowed, "ACCOUNT_OR_BATCH_HOLD");
        var observation = backend.Observe();
        authorize?.Invoke(contract,sha,mode,approval,account,backend.Now,binding);
        if(authorize is not null)backend.BindMutationBoundary(new TaskMutationBoundary(mode,sha,raw,operationRaw,binding,backend,cancellation));
        string prefix = contract.TrialId + ".admin.";
        ControlReceipt Make(string status, string fingerprint = "", string id = "") => new() { TrialId=contract.TrialId,ContractSha=sha,Status=status,Fingerprint=fingerprint,Instance=id,ApprovalId=approval.ApprovalId };
        ControlReceipt Read(string suffix)
        {
            var receipt=backend.Receipt(prefix+suffix) ?? throw new InvalidOperationException("MISSING_OWNERSHIP_RECEIPT");
            AdmissionPolicy.Require(receipt.TrialId==contract.TrialId && receipt.ContractSha==sha,"RECEIPT_BINDING");return receipt;
        }
        try
        {
            if(mode=="--register-only")
            {
                AdmissionPolicy.Require(observation is null,"TASK_ALREADY_EXISTS");
                backend.Save(prefix+"register-intent",Make("REGISTER_INTENT"));
                cancellation.ThrowIfCancellationRequested();
                backend.Register(TaskPolicy.Xml(contract,sha,binding),TaskPolicy.Sddl,cancellation);
                cancellation.ThrowIfCancellationRequested();
                var after=backend.Observe() ?? throw new InvalidOperationException("REGISTER_READBACK_MISSING");
                string fingerprint=TaskPolicy.Validate(after,contract,sha,binding);
                AdmissionPolicy.Require(!after.Enabled && after.Instances.Length==0,"REGISTER_NOT_DISABLED_IDLE");
                backend.Save(prefix+"registered",Make("REGISTERED_DISABLED",fingerprint));return 0;
            }
            var owned=Read("registered");
            AdmissionPolicy.Require(owned.Status=="REGISTERED_DISABLED" && observation is not null,"REGISTER_OWNERSHIP_HOLD");
            string current=TaskPolicy.Validate(observation!,contract,sha,binding);
            AdmissionPolicy.Require(current==owned.Fingerprint,"TASK_FINGERPRINT_CHANGED");
            if(mode=="--start-once")
            {
                AdmissionPolicy.Require(!observation!.Enabled && observation.Instances.Length==0,"TASK_NOT_DISABLED_IDLE");
                // 이 기록을 먼저 영구 소비합니다. Run의 응답 유실/실패도 재실행 권리가 되지 않습니다.
                backend.Save(prefix+"start-intent",Make("START_CONSUMED",current));
                cancellation.ThrowIfCancellationRequested();
                try
                {
                    backend.Enable(true);
                    cancellation.ThrowIfCancellationRequested();
                    Guid running=backend.Run();AdmissionPolicy.Require(running!=Guid.Empty,"START_INSTANCE_UNKNOWN");
                    backend.Save(prefix+"instance",Make("INSTANCE_RETURNED",current,running.ToString("D")));
                    cancellation.ThrowIfCancellationRequested();
                }
                finally { backend.Enable(false); }
                var after=backend.Observe() ?? throw new InvalidOperationException("START_READBACK_MISSING");
                AdmissionPolicy.Require(!after.Enabled && TaskPolicy.Validate(after,contract,sha,binding)==current,"START_DISABLE_OR_BINDING_HOLD");
                return 0;
            }
            if(mode=="--stop-owned-instance")
            {
                var started=Read("instance");
                AdmissionPolicy.Require(started.Status=="INSTANCE_RETURNED" && started.Fingerprint==current && Guid.TryParseExact(started.Instance,"D",out _) && instance.HasValue && instance.Value!=Guid.Empty && instance.Value.ToString("D")==started.Instance,"WRONG_INSTANCE");
                AdmissionPolicy.Require(!observation!.Enabled && observation.Instances.Length==1 && observation.Instances[0]==instance,"INSTANCE_SET_CHANGED");
                backend.Save(prefix+"stop-intent",Make("STOP_INTENT",current,started.Instance));
                cancellation.ThrowIfCancellationRequested();
                backend.Stop(instance!.Value); // Task.Stop로 전체 instance를 중지하지 않습니다.
                var after=backend.Observe() ?? throw new InvalidOperationException("STOP_READBACK_MISSING");
                AdmissionPolicy.Require(!after.Enabled && after.Instances.Length==0 && TaskPolicy.Validate(after,contract,sha,binding)==current,"STOP_NOT_DRAINED");
                backend.Save(prefix+"stopped",Make("INSTANCE_STOPPED",current,started.Instance));return 0;
            }
            AdmissionPolicy.Require(!observation!.Enabled && observation.Instances.Length==0,"REMOVE_NOT_DISABLED_IDLE");
            // 한 번이라도 시작 의도가 기록됐으면 정리 실증 없이는 삭제하지 않습니다.
            if(backend.Receipt(prefix+"start-intent") is not null)
                AdmissionPolicy.Require(backend.CleanupProven(contract.TrialId),"CLEANUP_UNKNOWN_KEEP_TASK");
            backend.Save(prefix+"remove-intent",Make("REMOVE_INTENT",current));
            cancellation.ThrowIfCancellationRequested();backend.Remove();
            AdmissionPolicy.Require(backend.Observe() is null,"REMOVE_READBACK_PRESENT");
            backend.Save(prefix+"removed",Make("REMOVED_EVIDENCE_RETAINED",current));return 0;
        }
        catch
        {
            // 실패 증거 쓰기 실패로 기존 오류를 덮지 않으며, 어떠한 자동 재시도도 하지 않습니다.
            try {backend.Save(prefix+"hold-"+Guid.NewGuid().ToString("N"),Make("HOLD"));}catch { }
            throw;
        }
    }
}

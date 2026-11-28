using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;

internal sealed class DeploymentFile
{
    public string Name { get; set; }="";
    public string Source { get; set; }="";
    public string Sha256 { get; set; }="";
    public long Bytes { get; set; }
}
internal sealed class DeploymentPlan
{
    public string TrialId { get; set; }="";
    public string WorkerSid { get; set; }="";
    public string CodeRoot { get; set; }="";
    public string EvidenceRoot { get; set; }="";
    public string TaskPath { get; set; }="";
    public DateTimeOffset ExpiresUtc { get; set; }
    public DeploymentFile[] Files { get; set; }=[];
}
internal sealed class DeploymentApproval
{
    public bool Approved { get; set; }
    public string Profile { get; set; }="";
    public string DeploymentId { get; set; }="";
    public string TrialId { get; set; }="";
    public string ManifestSha256 { get; set; }="";
    public DateTimeOffset ExpiresUtc { get; set; }
}
[JsonSerializable(typeof(DeploymentPlan))]
[JsonSerializable(typeof(DeploymentApproval))]
internal partial class DeploymentJson : JsonSerializerContext { }

// 이 도구는 Task/계정/기존 ACL을 관리하지 않습니다. 실제 backend는 명시 승인 후 별도 호출해야 합니다.
internal interface IProtectedDeployment
{
    DateTimeOffset Now { get; }
    byte[] ReadAdministratorApproval();
    bool DirectoryExists(string path); // 접근 거부를 not-found로 바꾸지 않습니다.
    bool FileExists(string path);
    void PinAncestor(string path);
    void PinSource(DeploymentFile file);
    void ConsumeApproval(string deploymentId,string manifestSha);
    void CreateDirectoryNew(string path,bool evidence);
    void CopyNew(string codeRoot,DeploymentFile file);
}
internal static class ProtectedDeployment
{
    internal static string Hash(byte[] raw)=>Convert.ToHexString(SHA256.HashData(raw)).ToLowerInvariant();
    private static void Fields(JsonElement value,params string[] names)
    {
        AdmissionPolicy.Require(value.ValueKind==JsonValueKind.Object,"DEPLOY_OBJECT");var seen=new HashSet<string>();
        foreach(var p in value.EnumerateObject())AdmissionPolicy.Require(names.Contains(p.Name,StringComparer.Ordinal)&&seen.Add(p.Name),"DEPLOY_FIELDS");
        AdmissionPolicy.Require(seen.Count==names.Length,"DEPLOY_FIELDS");
    }
    private static bool Inside(string path,string root)=>path.Equals(root,StringComparison.OrdinalIgnoreCase)||path.StartsWith(root+Path.DirectorySeparatorChar,StringComparison.OrdinalIgnoreCase);
    internal static DeploymentPlan ReadManifest(byte[] manifest,string manifestSha)
    {
        AdmissionPolicy.Require(manifest.Length is >0 and <=16384 && AdmissionPolicy.Sha(manifestSha)&&Hash(manifest)==manifestSha,"DEPLOY_MANIFEST_SHA");
        using(var doc=JsonDocument.Parse(manifest))
        {
            Fields(doc.RootElement,"TrialId","WorkerSid","CodeRoot","EvidenceRoot","TaskPath","ExpiresUtc","Files");
            AdmissionPolicy.Require(doc.RootElement.GetProperty("Files").ValueKind==JsonValueKind.Array,"DEPLOY_FILES");
            foreach(var f in doc.RootElement.GetProperty("Files").EnumerateArray())Fields(f,"Name","Source","Sha256","Bytes");
        }
        var plan=JsonSerializer.Deserialize(manifest,DeploymentJson.Default.DeploymentPlan)!;
        return plan;
    }
    internal static void Execute(byte[] manifest,string manifestSha,AdmissionBinding binding,IProtectedDeployment backend,DateTimeOffset? signedProductExpiry=null)
    {
        var plan=ReadManifest(manifest,manifestSha);
        bool CurrentPlan(DateTimeOffset now) => now < plan.ExpiresUtc && (signedProductExpiry is { } productEnd
            ? now < productEnd : now <= AdmissionPolicy.Deadline && plan.ExpiresUtc <= AdmissionPolicy.Deadline);
        AdmissionPolicy.Require(plan.TrialId==NativeTrialPolicy.CandidateTrialId && CurrentPlan(backend.Now),"DEPLOY_TRIAL_TIME");
        AdmissionPolicy.Require(plan.WorkerSid==binding.WorkerSid&&plan.CodeRoot==binding.CodeRoot&&plan.EvidenceRoot==binding.EvidenceRoot&&plan.TaskPath==binding.TaskPath,"DEPLOY_BINDING");
        string code=Path.GetFullPath(plan.CodeRoot),evidence=Path.GetFullPath(plan.EvidenceRoot);
        AdmissionPolicy.Require(Path.IsPathFullyQualified(plan.CodeRoot)&&Path.IsPathFullyQualified(plan.EvidenceRoot)
            && !Inside(code,evidence)&&!Inside(evidence,code),"DEPLOY_ROOTS");
        byte[] approvalRaw=backend.ReadAdministratorApproval();
        AdmissionPolicy.Require(approvalRaw.Length is >0 and <=4096,"DEPLOY_APPROVAL_SIZE");
        using(var doc=JsonDocument.Parse(approvalRaw))Fields(doc.RootElement,"Approved","Profile","DeploymentId","TrialId","ManifestSha256","ExpiresUtc");
        var approval=JsonSerializer.Deserialize(approvalRaw,DeploymentJson.Default.DeploymentApproval)!;
        AdmissionPolicy.Require(approval.Approved&&approval.Profile=="PROTECTED_DEPLOYMENT_V1"
            && Guid.TryParseExact(approval.DeploymentId,"N",out var id)&&id!=Guid.Empty&&approval.TrialId==plan.TrialId
            && approval.ManifestSha256==manifestSha&&approval.ExpiresUtc>backend.Now&&approval.ExpiresUtc<=plan.ExpiresUtc,"DEPLOY_APPROVAL_HOLD");
        AdmissionPolicy.Require(plan.Files.Length is >=2 and <=10,"DEPLOY_FILES");
        var names=new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var controls=new HashSet<string>(new[]{"TrialContract.json",plan.TrialId+".fake-only-trust.json",plan.TrialId+".fake-only-trust-basis.txt"}
            .Concat(new[]{"register-only","start-once","stop-owned-instance","remove-owned-task"}.Select(m=>plan.TrialId+"."+m+".approval.json")),StringComparer.Ordinal);
        foreach(var file in plan.Files)
        {
            AdmissionPolicy.Require(names.Add(file.Name)&&(file.Name is "Supervisor.exe" or "Probe.exe"||controls.Contains(file.Name))
                && file.Name==Path.GetFileName(file.Name)&&Path.IsPathFullyQualified(file.Source)&&AdmissionPolicy.Sha(file.Sha256)
                && file.Bytes is >0 and <=268435456,"DEPLOY_FILE_HOLD");
            AdmissionPolicy.Require(!Inside(Path.GetFullPath(file.Source),code)&&!Inside(Path.GetFullPath(file.Source),evidence),"DEPLOY_SOURCE_OVERLAP");
        }
        AdmissionPolicy.Require(names.Contains("Supervisor.exe")&&names.Contains("Probe.exe"),"DEPLOY_IMAGES_REQUIRED");
        // 어느 항목도 쓰기 전에 전체 경로/원본을 pin하고 검사합니다. 이미 있으면 채택/교체하지 않습니다.
        var directories=new List<(string Path,bool Evidence)>();
        foreach(var target in new[]{(code,false),(evidence,true)})
        {
            AdmissionPolicy.Require(!backend.DirectoryExists(target.Item1)&&!backend.FileExists(target.Item1),"DEPLOY_ROOT_EXISTS");
            var missing=new Stack<string>();string? p=target.Item1;
            while(p is not null&&!backend.DirectoryExists(p))
            { AdmissionPolicy.Require(!backend.FileExists(p)&&missing.Count<16,"DEPLOY_PATH_HOLD");missing.Push(p);p=Path.GetDirectoryName(p); }
            AdmissionPolicy.Require(p is not null,"DEPLOY_ANCESTOR_MISSING");
            for(string? parent=p;parent is not null;parent=Path.GetDirectoryName(parent))backend.PinAncestor(parent);
            while(missing.Count>0){string d=missing.Pop();if(!directories.Any(x=>x.Path.Equals(d,StringComparison.OrdinalIgnoreCase)))directories.Add((d,d==evidence));}
        }
        foreach(var file in plan.Files)backend.PinSource(file);
        // 시간 초과나 사전검사 실패를 승인된 적용으로 바꾸지 않습니다.
        AdmissionPolicy.Require(backend.Now<approval.ExpiresUtc&&CurrentPlan(backend.Now),"DEPLOY_EXPIRED_BEFORE_WRITE");
        backend.ConsumeApproval(approval.DeploymentId,manifestSha);
        foreach(var directory in directories)
        { AdmissionPolicy.Require(backend.Now<approval.ExpiresUtc&&CurrentPlan(backend.Now),"DEPLOY_EXPIRED_DURING_WRITE");backend.CreateDirectoryNew(directory.Path,directory.Evidence); }
        foreach(var file in plan.Files)
        { AdmissionPolicy.Require(backend.Now<approval.ExpiresUtc&&CurrentPlan(backend.Now),"DEPLOY_EXPIRED_DURING_WRITE");backend.CopyNew(code,file); }
        // 실패 항목을 자동 삭제/권한 복구하지 않습니다. 정확한 별도 정리 승인 전 증거를 보존합니다.
    }
}

using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

// 기존 두 게임 진단 이미지와 별개인 특권 도구의 승인입니다. 실제 승인 파일은 이번에 만들지 않습니다.
internal sealed class DeploymentRequest
{
    public bool Approved { get; set; }
    public string Profile { get; set; }="";
    public string DeploymentId { get; set; }="";
    public string TrialId { get; set; }="";
    public string ToolSha256 { get; set; }="";
    public string ManifestPath { get; set; }="";
    public string ManifestSha256 { get; set; }="";
    public string ApprovalPath { get; set; }="";
    public string ApprovalSha256 { get; set; }="";
    public string ContractPath { get; set; }="";
    public string ContractSha256 { get; set; }="";
    public string RiskBasisPath { get; set; }="";
    public string RiskBasisSha256 { get; set; }="";
    public DateTimeOffset ExpiresUtc { get; set; }
    public bool CreateRoots { get; set; }
    public bool CopyFiles { get; set; }
    public bool CredentialsReviewed { get; set; }
    public bool OtherLaunchPathsReviewed { get; set; }
    public bool ImageTrustReviewed { get; set; }
    public bool PrivilegedToolRiskAccepted { get; set; }
}
[JsonSerializable(typeof(DeploymentRequest))]
internal partial class DeploymentRequestJson:JsonSerializerContext { }
internal sealed record DeploymentActor(bool ElevatedAdministrator,bool NotImpersonating,string ToolSha256);
internal enum DeploymentStepKind { Consume, Directory, Copy }
internal sealed record DeploymentStep(DeploymentStepKind Kind,string Target,bool Evidence=false,DeploymentFile? File=null,string Sha="");

// 읽기와 변경을 분리합니다. 시험은 메모리 구현만 사용하고 Windows backend를 만들지 않습니다.
internal interface IDeploymentHost:IProtectedDeployment
{
    byte[] ReadProtected(string path,int limit);
    DeploymentActor ObserveActor();
    void BindBoundary(DeploymentWriteBoundary boundary);
}

internal sealed class DeploymentAdmission
{
    internal const string SupervisorSha="6df94e47c301c20eab537eb6ad49c680129b80787536358e6bdc78ed50a842e7";
    internal const string ProbeSha="0babc05a2e0b24b1e6563d049b8faabd7375f737b2acd5aedeaf840fc5e59322";
    internal readonly DeploymentRequest Request;
    internal readonly DeploymentPlan Plan;
    internal readonly byte[] Manifest,Approval;
    private readonly byte[] raw,contract,risk;
    private readonly string rawSha;
    private readonly AdmissionBinding binding;
    private readonly IDeploymentHost host;
    internal static T Strict<T>(byte[] bytes,int limit,JsonTypeInfo<T> info)
    {
        AdmissionPolicy.Require(bytes.Length>0&&bytes.Length<=limit,"DEPLOY_INPUT_SIZE");
        using var doc=JsonDocument.Parse(bytes);
        AdmissionPolicy.Require(doc.RootElement.ValueKind==JsonValueKind.Object,"DEPLOY_INPUT_OBJECT");
        var fields=info.Properties.Select(p=>p.Name).ToHashSet(StringComparer.Ordinal);var seen=new HashSet<string>();
        foreach(var p in doc.RootElement.EnumerateObject())AdmissionPolicy.Require(fields.Contains(p.Name)&&seen.Add(p.Name),"DEPLOY_INPUT_FIELDS");
        AdmissionPolicy.Require(seen.SetEquals(fields),"DEPLOY_INPUT_FIELDS");
        return JsonSerializer.Deserialize(bytes,info)??throw new InvalidOperationException("DEPLOY_INPUT_NULL");
    }
    internal DeploymentAdmission(byte[] raw,string sha,AdmissionBinding binding,IDeploymentHost host)
    {
        this.raw=(byte[])raw.Clone();rawSha=sha;this.binding=binding;this.host=host;
        AdmissionPolicy.Require(AdmissionPolicy.Sha(sha)&&ProtectedDeployment.Hash(raw)==sha,"DEPLOY_REQUEST_SHA");
        Request=Strict(raw,8192,DeploymentRequestJson.Default.DeploymentRequest);
        // 보호된 읽기만 수행하며 backend는 원본 HANDLE을 계속 보유하여 교체를 막습니다.
        foreach(string p in new[]{Request.ManifestPath,Request.ApprovalPath,Request.ContractPath,Request.RiskBasisPath})
            AdmissionPolicy.Require(Path.IsPathFullyQualified(p),"DEPLOY_INPUT_PATH");
        Manifest=host.ReadProtected(Request.ManifestPath,16384);Approval=host.ReadProtected(Request.ApprovalPath,4096);
        contract=host.ReadProtected(Request.ContractPath,4096);risk=host.ReadProtected(Request.RiskBasisPath,16384);
        Plan=JsonSerializer.Deserialize(Manifest,DeploymentJson.Default.DeploymentPlan)??throw new InvalidOperationException("DEPLOY_PLAN_NULL");
        Validate();
    }
    internal void Validate()
    {
        var r=Request;
        AdmissionPolicy.Require(ProtectedDeployment.Hash(raw)==rawSha&&r.Approved&&r.Profile=="INDEPENDENT_ADMIN_DEPLOY_V1"
            && Guid.TryParseExact(r.DeploymentId,"N",out var id)&&id!=Guid.Empty&&id.ToString("N")==r.DeploymentId
            && r.TrialId==NativeTrialPolicy.CandidateTrialId,"DEPLOY_REQUEST_HOLD");
        AdmissionPolicy.Require(r.CreateRoots&&r.CopyFiles&&r.CredentialsReviewed&&r.OtherLaunchPathsReviewed
            &&r.ImageTrustReviewed&&r.PrivilegedToolRiskAccepted,"DEPLOY_REVIEW_UNKNOWN_HOLD");
        AdmissionPolicy.Require(AdmissionPolicy.Sha(r.ToolSha256)&&ProtectedDeployment.Hash(Manifest)==r.ManifestSha256
            &&ProtectedDeployment.Hash(Approval)==r.ApprovalSha256&&ProtectedDeployment.Hash(contract)==r.ContractSha256
            &&risk.Length is >=32 and <=16384&&ProtectedDeployment.Hash(risk)==r.RiskBasisSha256,"DEPLOY_INPUT_SHA");
        var a=Strict(Approval,4096,DeploymentJson.Default.DeploymentApproval);
        AdmissionPolicy.Require(a.DeploymentId==r.DeploymentId&&a.TrialId==r.TrialId&&a.ManifestSha256==r.ManifestSha256
            &&a.Approved&&a.Profile=="PROTECTED_DEPLOYMENT_V1","DEPLOY_APPROVAL_BINDING");
        AdmissionPolicy.Contract(contract,host.Now,binding);
        var c=JsonSerializer.Deserialize(contract,ContractJson.Default.TrialContract)!;
        NativeTrialPolicy.Shape(c,"--supervise",host.Now);
        AdmissionPolicy.Require(c.SupervisorSha256==SupervisorSha&&c.ProbeSha256==ProbeSha&&c.TrialId==r.TrialId,"DEPLOY_IMAGES_BINDING");
        AdmissionPolicy.Require(Plan.TrialId==r.TrialId&&Plan.WorkerSid==binding.WorkerSid&&Plan.CodeRoot==binding.CodeRoot
            &&Plan.EvidenceRoot==binding.EvidenceRoot&&Plan.TaskPath==binding.TaskPath,"DEPLOY_PLAN_BINDING");
        // 배포 승인과 실행 승인은 분리합니다. 파일 hash가 승인돼도 임의 계약이나 추가 실행 파일을 넣지 않습니다.
        foreach(var f in Plan.Files)
        {
            if(f.Name=="Supervisor.exe")AdmissionPolicy.Require(f.Sha256==SupervisorSha&&f.Bytes==7559680,"DEPLOY_SUPERVISOR_IMAGE");
            if(f.Name=="Probe.exe")AdmissionPolicy.Require(f.Sha256==ProbeSha&&f.Bytes==1892352,"DEPLOY_PROBE_IMAGE");
            if(f.Name=="TrialContract.json")AdmissionPolicy.Require(f.Sha256==r.ContractSha256&&f.Source==r.ContractPath,"DEPLOY_CONTRACT_FILE");
        }
        AdmissionPolicy.Require(Plan.Files.Count(f=>f.Name=="TrialContract.json")==1,"DEPLOY_CONTRACT_REQUIRED");
        FakeOnlyPolicy.Evidence(c,host.Now,(name,limit)=> {
            var f=Plan.Files.SingleOrDefault(f=>f.Name==name)??throw new InvalidOperationException("DEPLOY_TRUST_MISSING");
            byte[] bytes=host.ReadProtected(f.Source,limit);AdmissionPolicy.Require(ProtectedDeployment.Hash(bytes)==f.Sha256&&bytes.Length==f.Bytes,"DEPLOY_TRUST_SHA");return bytes;
        });
        var actor=host.ObserveActor();
        AdmissionPolicy.Require(actor.ElevatedAdministrator&&actor.NotImpersonating&&actor.ToolSha256==r.ToolSha256,"DEPLOY_ACTOR_HOLD");
        // 신뢰 조회와 관리자/자기 이미지 확인 뒤 현재 시각을 다시 검사합니다.
        var now=host.Now;
        AdmissionPolicy.Require(now<=AdmissionPolicy.Deadline&&now<r.ExpiresUtc&&r.ExpiresUtc<=a.ExpiresUtc
            &&a.ExpiresUtc<=Plan.ExpiresUtc&&Plan.ExpiresUtc<=c.ExpiresUtc&&now<c.ExpiresUtc,"DEPLOY_CURRENT_TIME_HOLD");
        FakeOnlyPolicy.Evidence(c,now,(name,limit)=>host.ReadProtected(Plan.Files.Single(f=>f.Name==name).Source,limit));
        now=host.Now;
        AdmissionPolicy.Require(now<=AdmissionPolicy.Deadline&&now<r.ExpiresUtc&&now<a.ExpiresUtc&&now<Plan.ExpiresUtc&&now<c.ExpiresUtc,"DEPLOY_FINAL_TIME_HOLD");
    }
}

// 단순 bool/임의 callback으로 Windows backend를 여는 대신 검증된 계획의 현재 단계만 증명합니다.
internal sealed class DeploymentWriteBoundary(DeploymentAdmission admission,IReadOnlyList<DeploymentStep> steps)
{
    private int index;
    private DeploymentStep? active;
    private bool failed;
    internal void Begin(DeploymentStep step)
    {
        AdmissionPolicy.Require(!failed&&active is null&&index<steps.Count&&step==steps[index],"DEPLOY_STEP_ORDER");
        admission.Validate();active=step;
    }
    internal void Check(DeploymentStep step)
    {
        AdmissionPolicy.Require(!failed&&active is not null&&step==active,"DEPLOY_OUT_OF_PLAN");
        admission.Validate();
    }
    internal void End(bool success){active=null;if(success)index++;else failed=true;}
}

internal static class DeploymentController
{
    // 1차 계획은 기존 Execute를 그대로 사용하되 모든 쓰기를 메모리 목록으로 바꿉니다.
    // apply도 이 완전한 읽기 검증을 끝내기 전에는 승인 소비/폴더 생성/복사를 하지 않습니다.
    internal static int Run(string mode,byte[] request,string sha,AdmissionBinding binding,IDeploymentHost host)
    {
        AdmissionPolicy.Require(mode is "--review" or "--apply","DEPLOY_MODE_HOLD");
        var admission=new DeploymentAdmission(request,sha,binding,host);
        var plan=new PlanningBackend(host,admission);
        ProtectedDeployment.Execute(admission.Manifest,admission.Request.ManifestSha256,binding,plan);
        admission.Validate();
        if(mode=="--review")return plan.Steps.Count;
        // 소비 직전 대상이 생겼으면 기존 폴더를 채택하지 않습니다. 실제 생성도 CREATE_NEW입니다.
        foreach(string root in new[]{binding.CodeRoot,binding.EvidenceRoot})
            AdmissionPolicy.Require(!host.DirectoryExists(root)&&!host.FileExists(root),"DEPLOY_ROOT_EXISTS");
        var boundary=new DeploymentWriteBoundary(admission,plan.Steps.ToArray());
        host.BindBoundary(boundary);
        foreach(var step in plan.Steps)
        {
            boundary.Begin(step);bool success=false;
            try {
                switch(step.Kind) {
                    case DeploymentStepKind.Consume:host.ConsumeApproval(step.Target,admission.Request.ManifestSha256);break;
                    case DeploymentStepKind.Directory:host.CreateDirectoryNew(step.Target,step.Evidence);break;
                    case DeploymentStepKind.Copy:host.CopyNew(binding.CodeRoot,step.File!);break;
                }
                success=true;
            } finally {boundary.End(success);}
        }
        return plan.Steps.Count;
    }
    private sealed class PlanningBackend(IDeploymentHost host,DeploymentAdmission admission):IProtectedDeployment
    {
        internal readonly List<DeploymentStep> Steps=[];
        public DateTimeOffset Now=>host.Now;
        public byte[] ReadAdministratorApproval()=>admission.Approval;
        public bool DirectoryExists(string path)=>host.DirectoryExists(path);
        public bool FileExists(string path)=>host.FileExists(path);
        public void PinAncestor(string path)=>host.PinAncestor(path);
        public void PinSource(DeploymentFile file)=>host.PinSource(file);
        private void Add(DeploymentStep step){admission.Validate();Steps.Add(step);}
        public void ConsumeApproval(string id,string sha){AdmissionPolicy.Require(sha==admission.Request.ManifestSha256,"DEPLOY_PLAN_SHA");Add(new(DeploymentStepKind.Consume,id,Sha:sha));}
        public void CreateDirectoryNew(string path,bool evidence)=>Add(new(DeploymentStepKind.Directory,path,evidence));
        public void CopyNew(string root,DeploymentFile file)=>Add(new(DeploymentStepKind.Copy,Path.Combine(root,file.Name),File:file));
    }
}

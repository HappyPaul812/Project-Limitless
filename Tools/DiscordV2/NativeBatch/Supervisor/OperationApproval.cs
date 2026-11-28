using System.Text.Json;
using System.Text.Json.Serialization;

// TrialContract는 고정 시험 범위, 이 별도 봉인 파일은 각 관리자 동작의 실제 승인을 나타냅니다.
// 등록 승인만으로 시작 권리가 생기거나, 나중 승인 때문에 기존 Task Action hash를 바꾸지 않습니다.
internal sealed class OperationApproval
{
    public bool Approved { get; set; }
    public string ApprovalId { get; set; }="";
    public string TrialId { get; set; }="";
    public string ContractSha { get; set; }="";
    public string Operation { get; set; }="";
    public DateTimeOffset ExpiresUtc { get; set; }
    internal static string FileName(string trial,string operation)
    {AdmissionPolicy.Require(AdminController.IsMode(operation),"APPROVAL_OPERATION");return trial+"."+operation[2..]+".approval.json";}
    internal static OperationApproval Validate(byte[] raw,string trial,string sha,string operation,DateTimeOffset now,bool cleanup, CurrentFakeOnlyContract.ExecutionWindow? window=null)
    {
        AdmissionPolicy.Require(raw.Length is >0 and <=4096,"OPERATION_APPROVAL_SIZE");
        window?.RequireInputs(sha,raw);
        using var doc=JsonDocument.Parse(raw);AdmissionPolicy.Require(doc.RootElement.ValueKind==JsonValueKind.Object,"OPERATION_APPROVAL_OBJECT");
        string[] fields=["Approved","ApprovalId","TrialId","ContractSha","Operation","ExpiresUtc"];var seen=new HashSet<string>();
        foreach(var property in doc.RootElement.EnumerateObject())AdmissionPolicy.Require(fields.Contains(property.Name,StringComparer.Ordinal)&&seen.Add(property.Name),"OPERATION_APPROVAL_DUPLICATE_OR_UNKNOWN");
        AdmissionPolicy.Require(seen.Count==fields.Length,"OPERATION_APPROVAL_MISSING");
        var approval=JsonSerializer.Deserialize(raw,ApprovalJson.Default.OperationApproval)??throw new InvalidOperationException("OPERATION_APPROVAL_NULL");
        AdmissionPolicy.Require(approval.Approved && Guid.TryParseExact(approval.ApprovalId,"N",out var id)&&id!=Guid.Empty,"OPERATION_NOT_APPROVED");
        AdmissionPolicy.Require(approval.TrialId==trial&&approval.ContractSha==sha&&approval.Operation==operation,"OPERATION_APPROVAL_BINDING");
        AdmissionPolicy.Require(window is null ? approval.ExpiresUtc<=AdmissionPolicy.Deadline&&(cleanup||approval.ExpiresUtc>now) : window.Allows(now, approval.ExpiresUtc),"OPERATION_APPROVAL_EXPIRED");
        return approval;
    }
}
[JsonSerializable(typeof(OperationApproval))]
internal partial class ApprovalJson:JsonSerializerContext { }

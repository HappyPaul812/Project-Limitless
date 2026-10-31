using System.Security.AccessControl;
using System.Text;

// 승인된 실제 위치가 아직 없습니다. CLI/환경변수/요청 폴더로 채우지 않으며 미발급이면 HOLD합니다.
// 향후 검토된 private binding 파일이 Supply를 구현해야 합니다. 원장 bootstrap은 별도 관리자 승인입니다.
internal static partial class DeploymentLedgerBinding
{
    internal static readonly string Root=Read();
    private static string Read(){string root="";Supply(ref root);return root;}
    static partial void Supply(ref string root);
}
internal static class DeploymentLedgerPolicy
{
    internal const string Sddl="O:BAG:SYD:P(A;;FA;;;SY)(A;;FA;;;BA)";
    internal static void Root(string requested,string bound,AdmissionBinding scope)
    {
        AdmissionPolicy.Require(!string.IsNullOrWhiteSpace(bound)&&Path.IsPathFullyQualified(bound)
            &&!bound.StartsWith(@"\\",StringComparison.Ordinal)&&bound.Length>3
            &&Path.GetFullPath(bound).TrimEnd(Path.DirectorySeparatorChar)==bound
            &&bound[1]==':'&&bound.IndexOf(':',2)<0,"DEPLOY_LEDGER_BINDING_HOLD");
        AdmissionPolicy.Require(string.Equals(requested,bound,StringComparison.Ordinal),"DEPLOY_LEDGER_REDIRECT_HOLD");
        foreach(string target in new[]{scope.CodeRoot,scope.EvidenceRoot})
            AdmissionPolicy.Require(!Inside(bound,target)&&!Inside(target,bound),"DEPLOY_LEDGER_OVERLAP_HOLD");
    }
    private static bool Inside(string path,string parent)=>path.Equals(parent,StringComparison.OrdinalIgnoreCase)
        ||path.StartsWith(parent+Path.DirectorySeparatorChar,StringComparison.OrdinalIgnoreCase);
    internal static string RecordPath(string bound,string id)
    {
        AdmissionPolicy.Require(Guid.TryParseExact(id,"N",out var g)&&g!=Guid.Empty&&id==g.ToString("N"),"DEPLOY_LEDGER_ID");
        return Path.Combine(bound,id+".deploy-consumed");
    }
    internal static void Security(RawSecurityDescriptor actual)
    {
        AdmissionPolicy.Require(AdmissionPolicy.TrustedOwner(actual.Owner?.Value??"")&&actual.Group?.Value==AdmissionPolicy.SystemSid
            &&(actual.ControlFlags&ControlFlags.DiscretionaryAclProtected)!=0&&actual.DiscretionaryAcl is {Count:2},"DEPLOY_LEDGER_SECURITY_HOLD");
        var seen=new HashSet<string>();
        foreach(GenericAce item in actual.DiscretionaryAcl!)
            AdmissionPolicy.Require(item is CommonAce {IsCallback:false,AceFlags:AceFlags.None,AceQualifier:AceQualifier.AccessAllowed} a
                &&AdmissionPolicy.TrustedOwner(a.SecurityIdentifier.Value)&&a.AccessMask==0x1f01ff&&seen.Add(a.SecurityIdentifier.Value),"DEPLOY_LEDGER_DACL_HOLD");
        AdmissionPolicy.Require(seen.SetEquals([AdmissionPolicy.SystemSid,AdmissionPolicy.AdminSid]),"DEPLOY_LEDGER_DACL_HOLD");
    }
}
internal interface IDeploymentLedgerIo
{
    void PinAndValidate(); // 이미 존재하는 고정 원장/부모를 보호 확인합니다. 만들거나 권한을 바꾸지 않습니다.
    Stream CreateNew(string path); // 원자 CREATE_NEW, 초기 보호 SD/readback, 기존 record 채택/수정 금지
}
internal static class DeploymentLedger
{
    // 같은 정책을 Windows backend와 합성 파일/메모리 시험에서 사용합니다.
    internal static void Consume(string bound,string id,string sha,IDeploymentLedgerIo io,Action authorize)
    {
        AdmissionPolicy.Require(AdmissionPolicy.Sha(sha),"DEPLOY_LEDGER_SHA");
        string path=DeploymentLedgerPolicy.RecordPath(bound,id);
        io.PinAndValidate();authorize();
        using var file=io.CreateNew(path);
        // CREATE_NEW 이후 실패해도 빈 marker를 지우지 않습니다. 불확실한 소비를 재사용하지 않습니다.
        authorize();file.Write(Encoding.UTF8.GetBytes(id+"\n"+sha+"\n"));
        if(file is FileStream durable)durable.Flush(true);else file.Flush();
    }
}

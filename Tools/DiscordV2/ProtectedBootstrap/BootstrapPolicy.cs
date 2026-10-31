using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;

// 배포/Task/계정/원장 소비와 분리된, 고정 6개 디렉터리만 생성하는 승인입니다.
internal sealed class BootstrapAuthorization
{
    public bool Approved { get; set; }
    public string Profile { get; set; }="";
    public string ApprovalId { get; set; }="";
    public string ToolSha256 { get; set; }="";
    public string AdministratorSid { get; set; }="";
    public string OwnerSid { get; set; }="";
    public string[] Targets { get; set; }=[];
    public string[] AncestorSecuritySha256 { get; set; }=[];
    public string[] AncestorFileKeys { get; set; }=[];
    public DateTimeOffset ExpiresUtc { get; set; }
}
[JsonSerializable(typeof(BootstrapAuthorization))]
internal partial class BootstrapJson:JsonSerializerContext { }
internal sealed record BootstrapActor(bool Administrator,bool Elevated,bool Impersonating,string UserSid,string ToolSha256);
internal sealed record BootstrapNode(string Path,string Key,ulong Volume,bool Directory,bool Reparse,uint HandleFlags,byte[] Security);
internal interface IBootstrapHost:IDisposable
{
    DateTimeOffset Now { get; }
    bool FixedNtfsVolume { get; }
    BootstrapActor ObserveActor();
    bool Exists(string path); // 2/3 not-found만 false. 권한/공유/조회 불명은 예외입니다.
    BootstrapNode PinAndRead(string path);
    void Bind(BootstrapWriteGate gate);
    void CreateProtected(string path,string sddl);
}
internal static class BootstrapPolicy
{
    internal const string SystemSid="S-1-5-18",AdminSid="S-1-5-32-544";
    private const string Installer="S-1-5-80-956008885-3418522649-1831038044-1853292631-2271478464";
    internal static readonly DateTimeOffset Cutoff=new(2026,11,1,23,59,59,TimeSpan.FromHours(9));
    internal static readonly IReadOnlyList<string> Ancestors=Array.AsReadOnly(new[]{@"C:\",@"C:\ProgramData"});
    internal static readonly IReadOnlyList<string> Targets=Array.AsReadOnly(new[]{
        @"C:\ProgramData\Limitless",@"C:\ProgramData\Limitless\DiscordV2",@"C:\ProgramData\Limitless\DiscordV2\Control",
        @"C:\ProgramData\Limitless\DiscordV2\Control\DeploymentTool",@"C:\ProgramData\Limitless\DiscordV2\Control\Approvals",@"C:\ProgramData\Limitless\DiscordV2\Control\DeploymentLedger"});
    internal static void Need(bool value,string tag){if(!value)throw new InvalidOperationException(tag);}
    internal static string Hash(byte[] raw)=>Convert.ToHexString(SHA256.HashData(raw)).ToLowerInvariant();
    internal static bool Sha(string s)=>s.Length==64&&s.All(c=>c is >= '0' and <= '9' or >= 'a' and <= 'f');
    internal static string Sddl(string owner)=>"O:"+owner+"G:SYD:P(A;;FA;;;SY)(A;;FA;;;BA)";
    internal static BootstrapAuthorization Parse(byte[] raw,string sha)
    {
        Need(raw.Length is >0 and <=8192&&Sha(sha)&&Hash(raw)==sha,"BOOTSTRAP_AUTH_SHA");
        using var doc=JsonDocument.Parse(raw);Need(doc.RootElement.ValueKind==JsonValueKind.Object,"BOOTSTRAP_AUTH_OBJECT");
        var fields=BootstrapJson.Default.BootstrapAuthorization.Properties.Select(p=>p.Name).ToHashSet();var seen=new HashSet<string>();
        foreach(var p in doc.RootElement.EnumerateObject())Need(fields.Contains(p.Name)&&seen.Add(p.Name),"BOOTSTRAP_AUTH_FIELDS");
        Need(fields.SetEquals(seen),"BOOTSTRAP_AUTH_FIELDS");
        return JsonSerializer.Deserialize(raw,BootstrapJson.Default.BootstrapAuthorization)??throw new InvalidOperationException("BOOTSTRAP_AUTH_NULL");
    }
    internal static void Authorization(BootstrapAuthorization a,DateTimeOffset now)
    {
        Need(a.Approved&&a.Profile=="DISCORD_V2_PROTECTED_BOOTSTRAP_V1"&&Guid.TryParseExact(a.ApprovalId,"N",out var id)
            &&id!=Guid.Empty&&id.ToString("N")==a.ApprovalId,"BOOTSTRAP_NOT_AUTHORIZED");
        Need(a.Targets.SequenceEqual(Targets,StringComparer.Ordinal)&&a.OwnerSid is AdminSid or SystemSid,"BOOTSTRAP_SCOPE");
        Need(Sha(a.ToolSha256)&&!string.IsNullOrWhiteSpace(a.AdministratorSid)&&a.AncestorSecuritySha256.Length==2
            &&a.AncestorSecuritySha256.All(Sha)&&a.AncestorFileKeys.Length==2&&a.AncestorFileKeys.All(k=>!string.IsNullOrWhiteSpace(k)),"BOOTSTRAP_BINDING");
        Need(now<=Cutoff&&a.ExpiresUtc>now&&a.ExpiresUtc<=Cutoff,"BOOTSTRAP_EXPIRED");
    }
    internal static void Node(BootstrapNode n,string path)
        =>Need(n.Path.Equals(path,StringComparison.OrdinalIgnoreCase)&&n.Directory&&!n.Reparse&&(n.HandleFlags&1)==0&&!string.IsNullOrWhiteSpace(n.Key),"BOOTSTRAP_PATH_HANDLE");
    internal static void Ancestor(BootstrapNode n,string path)
    {
        Node(n,path);var sd=new RawSecurityDescriptor(n.Security,0);
        Need(sd.Owner?.Value is SystemSid or AdminSid or Installer&&sd.DiscretionaryAcl is not null,"BOOTSTRAP_ANCESTOR_OWNER");
        foreach(GenericAce item in sd.DiscretionaryAcl!)
        {
            Need(item is CommonAce {IsCallback:false},"BOOTSTRAP_ANCESTOR_ACE");var a=(CommonAce)item;
            Need(a.AceQualifier is AceQualifier.AccessAllowed or AceQualifier.AccessDenied,"BOOTSTRAP_ANCESTOR_ACE");
            if((a.AceFlags&AceFlags.InheritOnly)!=0||a.AceQualifier==AceQualifier.AccessDenied)continue;
            if(a.SecurityIdentifier.Value is SystemSid or AdminSid or Installer)continue;
            Need((unchecked((uint)a.AccessMask)&0xf00d0040U)==0,"BOOTSTRAP_ANCESTOR_REPLACE");
        }
    }
    internal static void AdministrativeSecurity(byte[] raw,string? expectedOwner=null)
    {
        var sd=new RawSecurityDescriptor(raw,0);
        Need(sd.Owner?.Value is AdminSid or SystemSid&&sd.Group?.Value==SystemSid
            &&(sd.ControlFlags&ControlFlags.DiscretionaryAclProtected)!=0&&sd.DiscretionaryAcl is {Count:2},"BOOTSTRAP_SECURITY");
        if(expectedOwner is not null)Need(sd.Owner!.Value==expectedOwner,"BOOTSTRAP_OWNER_READBACK");
        var seen=new HashSet<string>();
        foreach(GenericAce item in sd.DiscretionaryAcl!)
            Need(item is CommonAce {IsCallback:false,AceFlags:AceFlags.None,AceQualifier:AceQualifier.AccessAllowed} a
                &&a.SecurityIdentifier.Value is AdminSid or SystemSid&&a.AccessMask==0x1f01ff&&seen.Add(a.SecurityIdentifier.Value),"BOOTSTRAP_DACL_READBACK");
        Need(seen.SetEquals([AdminSid,SystemSid]),"BOOTSTRAP_DACL_READBACK");
    }
}

// Native 호출 직전에도 같은 검증을 수행합니다. 임의 callback/bool로 Windows 쓰기를 열지 않습니다.
internal sealed class BootstrapWriteGate
{
    private readonly BootstrapAuthorization authorization;
    private readonly IBootstrapHost host;
    private readonly CancellationToken cancellation;
    private readonly Dictionary<string,BootstrapNode> created=[];
    private readonly Dictionary<string,BootstrapNode> parents=[];
    private int next;
    private bool attempted;
    internal BootstrapWriteGate(BootstrapAuthorization authorization,IBootstrapHost host,CancellationToken cancellation)
    {this.authorization=authorization;this.host=host;this.cancellation=cancellation;}
    internal void RequireHost(IBootstrapHost expected)=>BootstrapPolicy.Need(ReferenceEquals(host,expected),"BOOTSTRAP_GATE_HOST_MISMATCH");
    internal string Sddl=>BootstrapPolicy.Sddl(authorization.OwnerSid);
    internal void Validate()
    {
        cancellation.ThrowIfCancellationRequested();BootstrapPolicy.Authorization(authorization,host.Now);
        var actor=host.ObserveActor();
        BootstrapPolicy.Need(actor.Administrator&&actor.Elevated&&!actor.Impersonating&&actor.UserSid==authorization.AdministratorSid
            &&actor.ToolSha256==authorization.ToolSha256,"BOOTSTRAP_ADMIN_OR_IMAGE");
        BootstrapPolicy.Need(host.FixedNtfsVolume,"BOOTSTRAP_VOLUME");
        for(int i=0;i<BootstrapPolicy.Ancestors.Count;i++)
        {
            string path=BootstrapPolicy.Ancestors[i];var node=host.PinAndRead(path);BootstrapPolicy.Ancestor(node,path);
            BootstrapPolicy.Need(BootstrapPolicy.Hash(node.Security)==authorization.AncestorSecuritySha256[i]
                &&node.Key==authorization.AncestorFileKeys[i],"BOOTSTRAP_PARENT_CHANGED");
            if(parents.TryGetValue(path,out var before))BootstrapPolicy.Need(node.Volume==before.Volume&&node.Key==before.Key,"BOOTSTRAP_PARENT_CHANGED");
            else parents.Add(path,node);
        }
        BootstrapPolicy.Need(parents.Values.Select(n=>n.Volume).Distinct().Count()==1,"BOOTSTRAP_VOLUME");
        foreach(var entry in created)
        {
            var node=host.PinAndRead(entry.Key);VerifyNew(node,entry.Key);
            BootstrapPolicy.Need(node.Key==entry.Value.Key,"BOOTSTRAP_CREATED_REPLACED");
        }
        foreach(string path in BootstrapPolicy.Targets.Skip(next))BootstrapPolicy.Need(!host.Exists(path),"BOOTSTRAP_TARGET_EXISTS");
        // 토큰/메타데이터 조회가 끝난 뒤 시각과 취소를 다시 검사합니다.
        actor=host.ObserveActor();
        BootstrapPolicy.Need(actor.Administrator&&actor.Elevated&&!actor.Impersonating&&actor.UserSid==authorization.AdministratorSid
            &&actor.ToolSha256==authorization.ToolSha256,"BOOTSTRAP_ADMIN_OR_IMAGE");
        BootstrapPolicy.Authorization(authorization,host.Now);cancellation.ThrowIfCancellationRequested();
    }
    private void VerifyNew(BootstrapNode node,string path)
    {
        BootstrapPolicy.Node(node,path);BootstrapPolicy.AdministrativeSecurity(node.Security,authorization.OwnerSid);
        BootstrapPolicy.Need(node.Volume==parents[BootstrapPolicy.Ancestors[0]].Volume,"BOOTSTRAP_VOLUME");
    }
    internal void BeforeNativeCreate(string path,string sddl)
    {
        BootstrapPolicy.Need(next<BootstrapPolicy.Targets.Count&&!attempted&&path==BootstrapPolicy.Targets[next]&&sddl==Sddl,"BOOTSTRAP_OUT_OF_SCOPE");
        Validate();attempted=true; // 실패/응답 유실도 같은 단계 재시도 권한이 아닙니다.
    }
    internal void Accept(string path)
    {
        BootstrapPolicy.Need(attempted&&path==BootstrapPolicy.Targets[next],"BOOTSTRAP_CREATE_PHASE");
        var node=host.PinAndRead(path);VerifyNew(node,path);created.Add(path,node);next++;attempted=false;
    }
    internal void Finish(){BootstrapPolicy.Need(next==BootstrapPolicy.Targets.Count,"BOOTSTRAP_INCOMPLETE");Validate();}
}
internal static class BootstrapController
{
    internal static int Run(byte[] raw,string sha,IBootstrapHost host,bool create,CancellationToken cancellation=default)
    {
        var gate=new BootstrapWriteGate(BootstrapPolicy.Parse(raw,sha),host,cancellation);gate.Validate();
        if(!create)return 0;
        host.Bind(gate);
        foreach(string path in BootstrapPolicy.Targets){host.CreateProtected(path,gate.Sddl);gate.Accept(path);}
        gate.Finish();return BootstrapPolicy.Targets.Count;
    }
}

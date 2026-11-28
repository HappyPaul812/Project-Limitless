using System.Security.Cryptography;
using System.Text.Json;

// 운영 계약의 기본 전역 조건은 유지합니다. 이 명시적 확장만 제한된 진단 계약입니다.
internal sealed class FakeOnlyIsolation
{
    public string Profile { get; set; }="";
    public string GlobalSidAbsence { get; set; }="";
    public string TrustEvidenceSha256 { get; set; }="";
}
internal static class FakeOnlyPolicy
{
    internal const string ProfileName="FAKE_ONLY_EXECUTION_V1";
    internal const string GlobalState="UNKNOWN";
    internal static string Hash(byte[] raw)=>Convert.ToHexString(SHA256.HashData(raw)).ToLowerInvariant();
    private static void Fields(JsonElement root,string[] names)
    {
        AdmissionPolicy.Require(root.ValueKind==JsonValueKind.Object,"FAKE_OBJECT");var seen=new HashSet<string>();
        foreach(var p in root.EnumerateObject())AdmissionPolicy.Require(names.Contains(p.Name,StringComparer.Ordinal)&&seen.Add(p.Name),"FAKE_UNKNOWN_OR_DUPLICATE");
        AdmissionPolicy.Require(seen.Count==names.Length,"FAKE_MISSING");
    }
    internal static void Profile(JsonElement root)
    {
        Fields(root,["Profile","GlobalSidAbsence","TrustEvidenceSha256"]);
        AdmissionPolicy.Require(root.GetProperty("Profile").GetString()==ProfileName&&root.GetProperty("GlobalSidAbsence").GetString()==GlobalState,"FAKE_PROFILE_SCOPE");
        AdmissionPolicy.Require(AdmissionPolicy.Sha(root.GetProperty("TrustEvidenceSha256").GetString()??""),"FAKE_TRUST_SHA");
    }
    internal static void Evidence(TrialContract c,DateTimeOffset now,Func<string,int,byte[]> read,bool cleanup=false,CurrentFakeOnlyContract.ExecutionWindow? window=null)
    {
        if(c.FakeOnlyIsolation is null)return;
        var profile=c.FakeOnlyIsolation;var s=c.Scope;
        AdmissionPolicy.Require(profile.Profile==ProfileName&&profile.GlobalSidAbsence==GlobalState&&s is {Mode:"FAKE_ONLY",NoIpc:true,NoAi:true,JobSeconds:90,SupervisorCount:1,ProbeCount:1},"FAKE_PROFILE_SCOPE");
        AdmissionPolicy.Require(AdmissionPolicy.Sha(profile.TrustEvidenceSha256),"FAKE_TRUST_SHA");
        // 파일명은 계약의 정규 trialID에서만 만들며 임의 경로를 입력받지 않습니다.
        AdmissionPolicy.Require(Guid.TryParseExact(c.TrialId,"N",out var id)&&id!=Guid.Empty&&id.ToString("N")==c.TrialId,"FAKE_TRIAL_ID");
        byte[] raw=read(c.TrialId+".fake-only-trust.json",4096);
        AdmissionPolicy.Require(raw.Length is >0 and <=4096&&Hash(raw)==profile.TrustEvidenceSha256,"FAKE_TRUST_HASH");
        using var doc=JsonDocument.Parse(raw);var root=doc.RootElement;
        Fields(root,["Approved","TrialId","WorkerSid","TaskPath","CodeRoot","EvidenceRoot","SupervisorSha256","ProbeSha256","ObservedUtc","ExpiresUtc","BasisSha256","NoUntrustedSameSidCode","CredentialsNotShared","NoOtherLaunchers","ResidualRiskAccepted","GlobalSidAbsence"]);
        AdmissionPolicy.Require(root.GetProperty("Approved").GetBoolean()&&root.GetProperty("ResidualRiskAccepted").GetBoolean()&&root.GetProperty("NoUntrustedSameSidCode").GetBoolean()&&root.GetProperty("CredentialsNotShared").GetBoolean()&&root.GetProperty("NoOtherLaunchers").GetBoolean(),"FAKE_TRUST_ASSUMPTIONS_HOLD");
        AdmissionPolicy.Require(root.GetProperty("GlobalSidAbsence").GetString()==GlobalState,"FAKE_GLOBAL_NOT_PASS");
        foreach(var pair in new Dictionary<string,string>{{"TrialId",c.TrialId},{"WorkerSid",s!.WorkerSid},{"TaskPath",s.TaskPath},{"CodeRoot",s.CodeRoot},{"EvidenceRoot",s.EvidenceRoot},{"SupervisorSha256",c.SupervisorSha256},{"ProbeSha256",c.ProbeSha256}})
            AdmissionPolicy.Require(root.GetProperty(pair.Key).GetString()==pair.Value,"FAKE_TRUST_BINDING");
        var observed=root.GetProperty("ObservedUtc").GetDateTimeOffset();var expiry=root.GetProperty("ExpiresUtc").GetDateTimeOffset();
        AdmissionPolicy.Require(observed<=now&&observed<expiry&&expiry<=c.ExpiresUtc&&(window is null ? expiry<=AdmissionPolicy.Deadline : window.AllowsTrial(now, expiry))&&(cleanup||(expiry>now&&now-observed<=TimeSpan.FromMinutes(15))),"FAKE_TRUST_STALE_OR_EXPIRED");
        string basisSha=root.GetProperty("BasisSha256").GetString()??"";
        byte[] basis=read(c.TrialId+".fake-only-trust-basis.txt",16384);
        AdmissionPolicy.Require(AdmissionPolicy.Sha(basisSha)&&basis.Length is >=32 and <=16384&&Hash(basis)==basisSha,"FAKE_TRUST_BASIS");
        // 해시는 자료의 결합/불변성만 확인합니다. 진술의 사실성은 보호된 관리자 검토의 신뢰 가정입니다.
    }
    internal static void Exclusivity(TrialContract c,DateTimeOffset now,Func<string,int,byte[]> read,Action global,bool cleanup=false,CurrentFakeOnlyContract.ExecutionWindow? window=null)
    {
        if(c.FakeOnlyIsolation is null){global();return;}
        Evidence(c,now,read,cleanup,window); // 전역 검사를 PASS로 바꾸지 않고 별도 계약/근거를 요구합니다.
    }
}

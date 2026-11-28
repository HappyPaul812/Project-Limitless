using System.Security.Cryptography;
using System.Security.Principal;
using System.Text.Json;

// 새 제품 계약을 검증해도 기존 실행 정책이나 개별 작업 승인까지 통과한 것은 아닙니다.
internal static class CurrentFakeOnlyContract
{
    internal const string Root = @"C:\ProgramData\Limitless\DiscordV2";
    internal const string Code = Root + @"\Code";
    internal const string Evidence = Root + @"\Evidence";
    internal const string Tool = Root + @"\Control\DeploymentTool\Limitless.DeploymentAdmin.exe";
    internal const string Approvals = Root + @"\Control\Approvals";
    internal const string Signer = "2a6d0a7d31e7642df209b6cbe12fa628500663f540eb6bd557cac2aca2d31f7c";

    internal static void RequireBinding(AdmissionBinding binding)
    {
        AdmissionPolicy.Require(binding.CodeRoot == Code && binding.EvidenceRoot == Evidence &&
            binding.WorkerSid is { Length: > 0 } && binding.TaskPath is { Length: > 1 } &&
            binding.TaskPath.StartsWith('\\') && !binding.TaskPath.Contains("..", StringComparison.Ordinal) &&
            !binding.TaskPath.EndsWith('\\') && !binding.TaskPath.Contains(@"\\", StringComparison.Ordinal) &&
            !binding.TaskPath.EndsWith(@"\LIMITLESS-Bot-OnBoot", StringComparison.OrdinalIgnoreCase) &&
            !binding.TaskPath.EndsWith(@"\LIMITLESS-Art-OnBoot", StringComparison.OrdinalIgnoreCase),
            "FAKE_V2_BINDING_HOLD");
        var sid = new SecurityIdentifier(binding.WorkerSid);
        AdmissionPolicy.Require(sid.IsAccountSid() && sid.Value == binding.WorkerSid, "FAKE_V2_WORKER_SID");
    }

    internal static DateTimeOffset Validate(byte[] raw, byte[] signature, RSA signer, string expectedSigner,
        AdmissionBinding binding, DateTimeOffset now, string supervisorSha, string probeSha)
    {
        RequireBinding(binding);
        string observedSigner = ProtectedDeployment.Hash(signer.ExportSubjectPublicKeyInfo());
        AdmissionPolicy.Require(raw.Length is > 0 and <= 4096 && signature.Length is > 0 and <= 4096 &&
            observedSigner == expectedSigner && signer.VerifyData(raw, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pss),
            "FAKE_V2_CONTRACT_SIGNATURE");
        using var doc = JsonDocument.Parse(raw);
        var contract = doc.RootElement;
        var fields = new HashSet<string>(StringComparer.Ordinal)
        {
            "Profile","Approved","WorkerSid","TaskPath","CodeRoot","EvidenceRoot","SupervisorSha256","ProbeSha256",
            "ImageSignerSpkiSha256","NotBeforeUtc","ExpiresUtc","Mode","NoAi","NoIpc","JobSeconds","SupervisorCount","ProbeCount"
        };
        AdmissionPolicy.Require(contract.ValueKind == JsonValueKind.Object &&
            contract.EnumerateObject().All(property => fields.Remove(property.Name)) && fields.Count == 0, "FAKE_V2_CONTRACT_FIELDS");
        string? Get(string name) => contract.GetProperty(name).GetString();
        AdmissionPolicy.Require(Get("Profile") == "DISCORD_V2_FAKE_ONLY_PRODUCT_V1" && contract.GetProperty("Approved").GetBoolean() &&
            Get("Mode") == "FAKE_ONLY" && contract.GetProperty("NoAi").GetBoolean() && contract.GetProperty("NoIpc").GetBoolean() &&
            contract.GetProperty("JobSeconds").GetInt32() == 90 && contract.GetProperty("SupervisorCount").GetInt32() == 1 &&
            contract.GetProperty("ProbeCount").GetInt32() == 1, "FAKE_V2_SCOPE_HOLD");
        AdmissionPolicy.Require(Get("WorkerSid") == binding.WorkerSid && Get("TaskPath") == binding.TaskPath &&
            Get("CodeRoot") == binding.CodeRoot && Get("EvidenceRoot") == binding.EvidenceRoot, "FAKE_V2_SCOPE_BINDING");
        AdmissionPolicy.Require(AdmissionPolicy.Sha(supervisorSha) && AdmissionPolicy.Sha(probeSha) &&
            Get("SupervisorSha256") == supervisorSha && Get("ProbeSha256") == probeSha &&
            Get("ImageSignerSpkiSha256") == expectedSigner, "FAKE_V2_IMAGE_BINDING");
        var starts = contract.GetProperty("NotBeforeUtc").GetDateTimeOffset();
        var expires = contract.GetProperty("ExpiresUtc").GetDateTimeOffset();
        AdmissionPolicy.Require(starts.Offset == TimeSpan.Zero && expires.Offset == TimeSpan.Zero &&
            starts <= now && now < expires && expires - starts <= TimeSpan.FromMinutes(5), "FAKE_V2_WINDOW_HOLD");
        return expires;
    }
}

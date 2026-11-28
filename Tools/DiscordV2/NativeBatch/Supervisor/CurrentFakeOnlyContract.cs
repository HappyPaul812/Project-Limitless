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

    // 서명된 제품 창이 실제 작업을 승인하려면 두 보호 입력의 해시도 서명에 포함되어야 합니다.
    internal sealed class ExecutionWindow
    {
        internal DateTimeOffset Starts { get; }
        internal DateTimeOffset Expires { get; }

        private readonly string trialSha, operationSha;
        private ExecutionWindow(DateTimeOffset starts, DateTimeOffset expires, string trialSha, string operationSha)
        { Starts = starts; Expires = expires; this.trialSha = trialSha; this.operationSha = operationSha; }

        internal static ExecutionWindow Verify(byte[] raw, byte[] signature, RSA signer, string expectedSigner,
            AdmissionBinding binding, DateTimeOffset now, string supervisorSha, string probeSha,
            string trialSha, string operationSha)
        {
            AdmissionPolicy.Require(AdmissionPolicy.Sha(trialSha) && AdmissionPolicy.Sha(operationSha), "FAKE_V2_EXECUTION_SHA");
            var window = ValidateCore(raw, signature, signer, expectedSigner, binding, now,
                supervisorSha, probeSha, trialSha, operationSha);
            return new ExecutionWindow(window.Starts, window.Expires, trialSha, operationSha);
        }

        internal void RequireInputs(string trial, byte[] operation)
            => AdmissionPolicy.Require(trial == trialSha && ProtectedDeployment.Hash(operation) == operationSha,
                "FAKE_V2_EXECUTION_INPUT_CHANGED");

        // 서명된 승인 기간과 보호된 Trial 기간을 둘 다 검사합니다.
        internal bool AllowsTrial(DateTimeOffset now, DateTimeOffset expiry)
            => Starts <= now && now < Expires && now < expiry;

        internal bool Allows(DateTimeOffset now, DateTimeOffset expiry)
            => AllowsTrial(now, expiry) && expiry <= Expires;
    }

    internal static DateTimeOffset Validate(byte[] raw, byte[] signature, RSA signer, string expectedSigner,
        AdmissionBinding binding, DateTimeOffset now, string supervisorSha, string probeSha)
        => ValidateCore(raw, signature, signer, expectedSigner, binding, now, supervisorSha, probeSha, null, null).Expires;

    private static (DateTimeOffset Starts, DateTimeOffset Expires) ValidateCore(byte[] raw, byte[] signature, RSA signer, string expectedSigner,
        AdmissionBinding binding, DateTimeOffset now, string supervisorSha, string probeSha, string? trialSha, string? operationSha)
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
        if (trialSha is not null)
        {
            fields.Add("TrialContractSha256");
            fields.Add("OperationApprovalSha256");
        }
        AdmissionPolicy.Require(contract.ValueKind == JsonValueKind.Object &&
            contract.EnumerateObject().All(property => fields.Remove(property.Name)) && fields.Count == 0, "FAKE_V2_CONTRACT_FIELDS");
        string? Get(string name) => contract.GetProperty(name).GetString();
        AdmissionPolicy.Require(Get("Profile") == (trialSha is null ? "DISCORD_V2_FAKE_ONLY_PRODUCT_V1" : "DISCORD_V2_FAKE_ONLY_EXECUTION_V2") && contract.GetProperty("Approved").GetBoolean() &&
            Get("Mode") == "FAKE_ONLY" && contract.GetProperty("NoAi").GetBoolean() && contract.GetProperty("NoIpc").GetBoolean() &&
            contract.GetProperty("JobSeconds").GetInt32() == 90 && contract.GetProperty("SupervisorCount").GetInt32() == 1 &&
            contract.GetProperty("ProbeCount").GetInt32() == 1, "FAKE_V2_SCOPE_HOLD");
        AdmissionPolicy.Require(Get("WorkerSid") == binding.WorkerSid && Get("TaskPath") == binding.TaskPath &&
            Get("CodeRoot") == binding.CodeRoot && Get("EvidenceRoot") == binding.EvidenceRoot, "FAKE_V2_SCOPE_BINDING");
        AdmissionPolicy.Require(AdmissionPolicy.Sha(supervisorSha) && AdmissionPolicy.Sha(probeSha) &&
            Get("SupervisorSha256") == supervisorSha && Get("ProbeSha256") == probeSha &&
            Get("ImageSignerSpkiSha256") == expectedSigner, "FAKE_V2_IMAGE_BINDING");
        if (trialSha is not null)
            AdmissionPolicy.Require(Get("TrialContractSha256") == trialSha &&
                Get("OperationApprovalSha256") == operationSha, "FAKE_V2_EXECUTION_BINDING");
        var starts = contract.GetProperty("NotBeforeUtc").GetDateTimeOffset();
        var expires = contract.GetProperty("ExpiresUtc").GetDateTimeOffset();
        AdmissionPolicy.Require(starts.Offset == TimeSpan.Zero && expires.Offset == TimeSpan.Zero &&
            starts <= now && now < expires && expires - starts <= TimeSpan.FromMinutes(5), "FAKE_V2_WINDOW_HOLD");
        return (starts, expires);
    }
}

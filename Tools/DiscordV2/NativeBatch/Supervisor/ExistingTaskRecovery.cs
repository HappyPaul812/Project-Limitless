using System.Globalization;
using System.Security.Cryptography;
using System.Text.Json;

// 복구 서명은 기존 Task 인자를 식별하는 데만 사용합니다.
internal static class ExistingTaskRecovery
{
    internal const string PayloadName = "ExistingTaskRecovery.v1.json";
    internal const string SignatureName = "ExistingTaskRecovery.v1.sig";
    private static readonly string[] HashFields = [
        "PriorTrialSha256", "NextTrialSha256", "PriorSupervisorSha256", "NextSupervisorSha256",
        "ProbeSha256", "TaskFingerprintSha256", "RegisterIntentSha256", "HoldEvidenceSha256",
        "PriorConsumptionSha256", "RegistrationDiagnosticSha256"];
    private static readonly string[] TextFields = [
        "Profile", "TrialId", "WorkerSid", "TaskPath", "CodeRoot", "EvidenceRoot",
        "PriorApprovalId", "RecoveryId", "IssuedUtc", "ExpiresUtc", "NextTrialExpiresUtc"];

    internal sealed record Verified(string PriorTrialSha, ControlReceipt Receipt);

    internal static Verified Verify(byte[] payload, byte[] signature, RSA signer, string expectedSigner,
        AdmissionBinding binding, TrialContract current, string currentSha, string? taskArgumentSha, DateTimeOffset now)
    {
        AdmissionPolicy.Require(payload.Length is > 0 and <= 8192 && signature.Length == signer.KeySize / 8 &&
            ProtectedDeployment.Hash(signer.ExportSubjectPublicKeyInfo()) == expectedSigner &&
            signer.VerifyData(payload, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pss),
            "TASK_RECOVERY_SIGNATURE");
        using var doc = JsonDocument.Parse(payload);
        var fields = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var field in doc.RootElement.EnumerateObject())
        {
            AdmissionPolicy.Require((HashFields.Contains(field.Name) || TextFields.Contains(field.Name)) &&
                field.Value.ValueKind == JsonValueKind.String &&
                fields.TryAdd(field.Name, field.Value.GetString()!), "TASK_RECOVERY_FIELDS");
        }
        AdmissionPolicy.Require(fields.Count == HashFields.Length + TextFields.Length &&
            HashFields.All(name => AdmissionPolicy.Sha(fields[name])), "TASK_RECOVERY_FIELDS");
        AdmissionPolicy.Require(fields["Profile"] == "DISCORD_V2_EXISTING_TASK_RECOVERY_V1" &&
            fields["TrialId"] == current.TrialId && fields["WorkerSid"] == binding.WorkerSid &&
            fields["TaskPath"] == binding.TaskPath && fields["CodeRoot"] == binding.CodeRoot &&
            fields["EvidenceRoot"] == binding.EvidenceRoot &&
            (taskArgumentSha is null || fields["PriorTrialSha256"] == taskArgumentSha) && fields["NextTrialSha256"] == currentSha &&
            fields["PriorTrialSha256"] != currentSha && fields["NextSupervisorSha256"] == current.SupervisorSha256 &&
            fields["ProbeSha256"] == current.ProbeSha256, "TASK_RECOVERY_BINDING");
        AdmissionPolicy.Require(Guid.TryParseExact(fields["RecoveryId"], "N", out var recovery) && recovery != Guid.Empty &&
            Guid.TryParseExact(fields["PriorApprovalId"], "N", out var prior) && prior != Guid.Empty && prior != recovery,
            "TASK_RECOVERY_ID");
        DateTimeOffset ReadTime(string name) => DateTimeOffset.ParseExact(fields[name], "o", CultureInfo.InvariantCulture);
        var issued = ReadTime("IssuedUtc");
        var expires = ReadTime("ExpiresUtc");
        AdmissionPolicy.Require(ReadTime("NextTrialExpiresUtc") == current.ExpiresUtc &&
            issued <= now && now < expires && expires <= current.ExpiresUtc &&
            expires - issued <= TimeSpan.FromMinutes(5), "TASK_RECOVERY_WINDOW");
        return new Verified(fields["PriorTrialSha256"], new ControlReceipt
        {
            Status = "REGISTERED_DISABLED", TrialId = current.TrialId, ContractSha = currentSha,
            Fingerprint = fields["TaskFingerprintSha256"], ApprovalId = fields["RecoveryId"]
        });
    }
}

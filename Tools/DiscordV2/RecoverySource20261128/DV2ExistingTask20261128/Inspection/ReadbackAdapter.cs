global using System;
global using System.Collections.Generic;
global using System.IO;
global using System.Linq;

internal sealed class TrialContract { public string TrialId { get; init; } = ""; }
internal readonly record struct AdmissionBinding(string WorkerSid, string TaskPath, string CodeRoot, string EvidenceRoot);
internal sealed record TaskObservation(string Xml, string Sddl, bool Enabled, Guid[] Instances, int State);
internal static class AdmissionPolicy
{
    internal static void Require(bool value, string code)
    {
        if (!value) throw new InvalidOperationException(code);
    }
    internal static bool TrustedOwner(string sid) => sid is "S-1-5-18" or "S-1-5-32-544";
}
public static class V2TaskReadbackInspection
{
    public static string Fingerprint(string xml, string sddl, bool enabled, int state, Guid[] instances,
        string trialId, string trialSha, string sid, string taskPath, string code, string evidence)
    {
        AdmissionPolicy.Require(!enabled && instances.Length == 0, "TASK_NOT_DISABLED_IDLE");
        return TaskPolicy.Validate(new(xml, sddl, enabled, instances, state),
            new() { TrialId = trialId }, trialSha, new(sid, taskPath, code, evidence));
    }
}

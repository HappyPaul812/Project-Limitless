using System.Security.AccessControl;
using System.Security.Principal;
using System.Text.Json;

// OS 호출과 분리한 실제 승인 정책입니다. 테스트도 이 구현을 직접 사용합니다.
internal static class AdmissionPolicy
{
    internal const uint Delete = 0x10000, WriteDac = 0x40000, WriteOwner = 0x80000;
    internal const uint OwnerMutation = Delete | WriteDac | WriteOwner;
    internal const string SystemSid = "S-1-5-18", AdminSid = "S-1-5-32-544", OwnerRights = "S-1-3-4";
    // 외부 HANDLE에는 조회/동기화 외 모든 권리를 위험 권리로 봅니다. 개별 비트 누락을 막습니다.
    internal const uint ProcessDanger = ~(0x1000U | 0x400U | 0x20000U | 0x100000U);
    internal const uint ThreadDanger = ~(0x800U | 0x40U | 0x20000U | 0x100000U);
    internal const uint TokenDanger = ~(8U | 0x20000U);
    // 조회 전용/권리0 HANDLE도 Job 수명을 연장하여 kill-on-close를 깨므로 완전 독점입니다.
    internal const uint JobDanger = uint.MaxValue;
    internal static readonly DateTimeOffset Deadline = new(2026,11,1,23,59,59,TimeSpan.FromHours(9));
    internal static void Require(bool condition, string tag)
    { if (!condition) throw new InvalidOperationException(tag); }
    internal static bool Sha(string value) => value.Length == 64 && value.All(c => c is >= '0' and <= '9' or >= 'a' and <= 'f');

    internal static void Contract(byte[] raw, DateTimeOffset now, AdmissionBinding binding, bool cleanup = false)
    {
        Require(raw.Length is > 0 and <= 4096, "CONTRACT_SIZE");
        using var doc = JsonDocument.Parse(raw);
        Require(doc.RootElement.ValueKind == JsonValueKind.Object, "CONTRACT_OBJECT");
        string[] fields = ["Approved", "TrialId", "SupervisorSha256", "ProbeSha256", "ExpiresUtc", "Scope"];
        var seen = new HashSet<string>(StringComparer.Ordinal);
        foreach (var property in doc.RootElement.EnumerateObject())
            Require(fields.Contains(property.Name, StringComparer.Ordinal) && seen.Add(property.Name), "CONTRACT_UNKNOWN_OR_DUPLICATE");
        Require(seen.Count == fields.Length, "CONTRACT_MISSING");
        var root = doc.RootElement;
        Require(root.GetProperty("Approved").GetBoolean(), "APPROVAL_HOLD");
        string id = root.GetProperty("TrialId").GetString() ?? "";
        Require(Guid.TryParseExact(id, "N", out var guid) && guid != Guid.Empty && id == guid.ToString("N"), "TRIAL_ID");
        Require(Sha(root.GetProperty("SupervisorSha256").GetString() ?? "") && Sha(root.GetProperty("ProbeSha256").GetString() ?? ""), "CONTRACT_IMAGE_SHA");
        var expiry = root.GetProperty("ExpiresUtc").GetDateTimeOffset();
        Require(expiry <= Deadline && (cleanup || expiry > now), "APPROVAL_EXPIRED_OR_DEADLINE");
        var scope = root.GetProperty("Scope");
        string[] scopeFields = ["WorkerSid", "TaskPath", "CodeRoot", "EvidenceRoot", "Mode", "NoIpc", "NoAi", "JobSeconds", "SupervisorCount", "ProbeCount", "Register", "Start", "Cleanup"];
        Require(scope.ValueKind == JsonValueKind.Object, "SCOPE_OBJECT");
        var scopeSeen = new HashSet<string>(StringComparer.Ordinal);
        foreach (var property in scope.EnumerateObject())
            Require(scopeFields.Contains(property.Name, StringComparer.Ordinal) && scopeSeen.Add(property.Name), "SCOPE_UNKNOWN_OR_DUPLICATE");
        Require(scopeSeen.Count == scopeFields.Length, "SCOPE_MISSING");
        Require(scope.GetProperty("WorkerSid").GetString() == binding.WorkerSid && scope.GetProperty("TaskPath").GetString() == binding.TaskPath &&
            scope.GetProperty("CodeRoot").GetString() == binding.CodeRoot && scope.GetProperty("EvidenceRoot").GetString() == binding.EvidenceRoot, "SCOPE_BINDING");
        Require(scope.GetProperty("Mode").GetString() == "FAKE_ONLY" && scope.GetProperty("NoIpc").GetBoolean() && scope.GetProperty("NoAi").GetBoolean(), "SCOPE_MODE");
        Require(scope.GetProperty("JobSeconds").GetInt32() == 90 && scope.GetProperty("SupervisorCount").GetInt32() == 1 && scope.GetProperty("ProbeCount").GetInt32() == 1, "SCOPE_LIMITS");
        _ = scope.GetProperty("Register").GetBoolean(); _ = scope.GetProperty("Start").GetBoolean(); _ = scope.GetProperty("Cleanup").GetBoolean();
    }

    internal static bool TrustedOwner(string sid) => sid is SystemSid or AdminSid;

    internal static void RecordDescriptor(RawSecurityDescriptor descriptor, string worker)
    {
        Require(descriptor.Owner?.Value == worker, "RECORD_OWNER");
        Require((descriptor.ControlFlags & ControlFlags.DiscretionaryAclProtected) != 0 && descriptor.DiscretionaryAcl is not null, "RECORD_DACL_UNPROTECTED");
        bool ownerGuard = false;
        foreach (GenericAce item in descriptor.DiscretionaryAcl!)
        {
            Require(item is CommonAce { IsCallback: false }, "RECORD_ACE");
            var ace = (CommonAce)item;
            Require(ace.AceFlags == AceFlags.None, "RECORD_ACE_FLAGS");
            if (ace.AceQualifier == AceQualifier.AccessDenied)
            {
                Require(ace.SecurityIdentifier.Value == OwnerRights && unchecked((uint)ace.AccessMask) == OwnerMutation, "RECORD_DENY_ACE");
                ownerGuard = true;
            }
            else
            {
                Require(ace.AceQualifier == AceQualifier.AccessAllowed, "RECORD_ACE_TYPE");
                if (!TrustedOwner(ace.SecurityIdentifier.Value))
                    Require(ace.SecurityIdentifier.Value == worker && (unchecked((uint)ace.AccessMask) & ~0x120003U) == 0, "RECORD_MUTATION");
            }
        }
        Require(ownerGuard, "RECORD_OWNER_RIGHTS");
    }

    // DACL의 허용 ACE를 보수적으로 검사합니다. 알 수 없는/object/callback ACE는 허용하지 않습니다.
    // 유효 권한은 아래 정적 판정에 더해 Windows AccessCheck로 다시 확인합니다.
    internal static void Descriptor(RawSecurityDescriptor descriptor, string worker, bool evidenceDirectory, bool ancestor = false)
    {
        string owner = descriptor.Owner?.Value ?? "";
        // 시스템 상위 경로에는 TrustedInstaller 소유가 허용되지만 시험 자산 소유는 BA/SYSTEM만 허용합니다.
        const string installer = "S-1-5-80-956008885-3418522649-1831038044-1853292631-2271478464";
        Require(TrustedOwner(owner) || (ancestor && owner == installer), "UNTRUSTED_OWNER");
        Require(descriptor.DiscretionaryAcl is not null, "NULL_DACL");
        if (!ancestor) Require((descriptor.ControlFlags & ControlFlags.DiscretionaryAclProtected) != 0, "UNPROTECTED_DACL");
        uint allowedWorker = evidenceDirectory ? 0x1200abU : 0x1200a9U; // 디렉터리 create-file만 추가, delete-child/쓰기/ACL 변경 제외
        foreach (GenericAce item in descriptor.DiscretionaryAcl!)
        {
            Require(item is CommonAce { IsCallback: false }, "UNSUPPORTED_ACE");
            var ace = (CommonAce)item;
            Require(ace.AceQualifier is AceQualifier.AccessAllowed or AceQualifier.AccessDenied, "UNSUPPORTED_ACE_TYPE");
            if ((ace.AceFlags & AceFlags.InheritOnly) != 0) continue;
            if (ace.AceQualifier == AceQualifier.AccessDenied) continue;
            string sid = ace.SecurityIdentifier.Value;
            uint mask = unchecked((uint)ace.AccessMask);
            if (TrustedOwner(sid) || (ancestor && sid == installer)) continue;
            if (ancestor)
            {
                // 상위 디렉터리의 새 파일 생성 권리와 이미 존재하는 경로 교체 권리를 구분합니다.
                Require((mask & (OwnerMutation | 0x40U | 0xf0000000U)) == 0, "ANCESTOR_REPLACE_ACCESS");
            }
            else
            {
                uint allowed = sid == worker ? allowedWorker : 0x1200a9U;
                Require((mask & ~allowed) == 0, "UNTRUSTED_MUTATION_ACE");
            }
        }
    }

    internal static void ForeignHandles(IReadOnlyList<HandleEntry> entries, IReadOnlyDictionary<ulong, uint> protectedObjects, ulong self)
    {
        Require(entries.Count > 0 && protectedObjects.Count > 0 && protectedObjects.Keys.All(k => k != 0), "HANDLE_OBJECT_UNKNOWN");
        foreach (var entry in entries)
            if (entry.Pid != self && protectedObjects.TryGetValue(entry.Object, out uint dangerous))
                Require(dangerous != JobDanger && (entry.Access & dangerous) == 0, "EXTERNAL_DANGEROUS_HANDLE");
    }
}

internal readonly record struct HandleEntry(ulong Object, ulong Pid, ulong Handle, uint Access);
internal readonly record struct AdmissionBinding(string WorkerSid, string TaskPath, string CodeRoot, string EvidenceRoot)
{
    internal static AdmissionBinding Current => new(Native.Worker, Native.TaskPath, Native.CodeRoot, Native.EvidenceRoot);
}

// 소비 기록은 성공 여부와 무관하게 삭제/초기화하지 않습니다. 충돌·부분 쓰기도 재사용 금지입니다.
internal static class AdmissionConsumption
{
    internal static IDisposable Acquire(string prefix, byte[] intent, Func<string, Stream> createNew)
    {
        Stream? lease = null;
        try
        {
            lease = createNew(Path.Combine(prefix, ".supervisor-once"));
            lease.Write(intent);
            if (lease is FileStream file) file.Flush(true); else lease.Flush();
            return lease;
        }
        catch { lease?.Dispose(); throw; }
    }
}

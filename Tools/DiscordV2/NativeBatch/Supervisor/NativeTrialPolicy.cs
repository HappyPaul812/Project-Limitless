// 전역 운영 gate와 별개인 단 하나의 미승인 후보 시험 ID입니다.
// ID를 아는 것만으로 실행 권리가 생기지 않습니다. 보호된 계약/신뢰/동작 승인 검증이 모두 필요합니다.
internal static class NativeTrialPolicy
{
    internal const string CandidateTrialId = "deef7539de5b4efcb30e062f9b734342";
    // 암호 입력 대기 뒤 실제 Task 변경 직전의 시간 재검증이 아직 없습니다.
    // 이 정적 위험이 해소/검증되기 전에는 순수 정책/모의 시험만 가능하고 실제 어댑터는 닫습니다.
    internal static void RequireWindowsBoundary()
        => throw new InvalidOperationException("HOLD_NATIVE_MUTATION_DEADLINE_REVIEW");
    internal static void Shape(TrialContract c, string mode, DateTimeOffset now)
    {
        AdmissionPolicy.Require(now <= AdmissionPolicy.Deadline && c.ExpiresUtc > now && c.ExpiresUtc <= AdmissionPolicy.Deadline, "TRIAL_TIME_HOLD");
        AdmissionPolicy.Require(c.Approved && c.TrialId == CandidateTrialId, "TRIAL_ID_NOT_ADMITTED");
        AdmissionPolicy.Require(c.FakeOnlyIsolation is { Profile: FakeOnlyPolicy.ProfileName, GlobalSidAbsence: FakeOnlyPolicy.GlobalState } f
            && AdmissionPolicy.Sha(f.TrustEvidenceSha256), "TRIAL_PROFILE_HOLD");
        AdmissionPolicy.Require(c.Scope is { Mode: "FAKE_ONLY", NoIpc: true, NoAi: true, JobSeconds: 90, SupervisorCount: 1, ProbeCount: 1 }, "TRIAL_SCOPE_HOLD");
        bool allowed = mode switch {
            "--register-only" => c.Scope!.Register,
            "--start-once" or "--supervise" => c.Scope!.Start,
            "--stop-owned-instance" or "--remove-owned-task" => c.Scope!.Cleanup,
            _ => false };
        AdmissionPolicy.Require(allowed, "TRIAL_MODE_HOLD");
    }
    // 읽기 전용 검증 뒤, 최초 쓰기/보안 변경 바로 앞에서 재확인합니다.
    internal static void Authorize(TrialContract c, string sha, string mode, OperationApproval a,
        AccountObservation account, DateTimeOffset now, AdmissionBinding binding)
    {
        Shape(c, mode, now);
        var s = c.Scope!;
        AdmissionPolicy.Require(s.WorkerSid == binding.WorkerSid && s.CodeRoot == binding.CodeRoot
            && s.EvidenceRoot == binding.EvidenceRoot && s.TaskPath == binding.TaskPath, "TRIAL_BINDING_HOLD");
        AdmissionPolicy.Require(AdmissionPolicy.Sha(sha) && AdmissionPolicy.Sha(c.SupervisorSha256) && AdmissionPolicy.Sha(c.ProbeSha256), "TRIAL_SHA_HOLD");
        string operation = mode == "--supervise" ? "--start-once" : mode;
        AdmissionPolicy.Require(a.Approved && Guid.TryParseExact(a.ApprovalId, "N", out var id) && id != Guid.Empty
            && a.TrialId == c.TrialId && a.ContractSha == sha && a.Operation == operation
            && a.ExpiresUtc > now && a.ExpiresUtc <= c.ExpiresUtc, "TRIAL_OPERATION_HOLD");
        AdmissionPolicy.Require(account.Sid == s.WorkerSid, "TRIAL_ACCOUNT_HOLD");
        // 정리는 기존 계약처럼 계정 비활성화 때문에 자기 객체를 남기지 않습니다.
        // 그래도 새 cleanup 승인/정확 소유 증표/현재 cutoff는 반드시 요구합니다.
        if (mode is not "--stop-owned-instance" and not "--remove-owned-task")
            AdmissionPolicy.Require(account.Enabled && !account.Locked && account.ExpiresUtc >= c.ExpiresUtc
                && account.BatchKnown && account.BatchAllowed, "TRIAL_ACCOUNT_HOLD");
    }
    internal static void AuthorizeThen(TrialContract c, string sha, string mode, OperationApproval approval,
        AccountObservation account, DateTimeOffset now, AdmissionBinding binding, Action action)
    { Authorize(c, sha, mode, approval, account, now, binding); action(); }
}

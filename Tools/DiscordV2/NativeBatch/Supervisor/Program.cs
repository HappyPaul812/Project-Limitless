using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

internal sealed class TrialContract
{
    public bool Approved { get; set; }
    public string TrialId { get; set; } = "";
    public string SupervisorSha256 { get; set; } = "";
    public string ProbeSha256 { get; set; } = "";
    public DateTimeOffset ExpiresUtc { get; set; }
    public TrialScope? Scope { get; set; }
    [JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]
    public FakeOnlyIsolation? FakeOnlyIsolation { get; set; }
}
internal sealed class TrialScope
{
    public string WorkerSid { get; set; } = "";
    public string TaskPath { get; set; } = "";
    public string CodeRoot { get; set; } = "";
    public string EvidenceRoot { get; set; } = "";
    public string Mode { get; set; } = "";
    public bool NoIpc { get; set; }
    public bool NoAi { get; set; }
    public int JobSeconds { get; set; }
    public int SupervisorCount { get; set; }
    public int ProbeCount { get; set; }
    public bool Register { get; set; }
    public bool Start { get; set; }
    public bool Cleanup { get; set; }
}
[JsonSerializable(typeof(TrialContract))]
internal partial class ContractJson : JsonSerializerContext { }

internal static class Program
{
    // 핵심 admission 구현은 아래 실제 어댑터에 연결되어 있습니다. 이 gate는 Windows 실검증 전 유지합니다.
    // 관리자 모드도 같은 실행 봉인 뒤에서만 접근합니다. 이번 시험은 모의 backend만 사용합니다.
    private static readonly bool ReleaseAdmissionImplemented = false;
    private static readonly byte[] Intent = Encoding.UTF8.GetBytes("TOKEN_PROBE_INTENT_ONLY\n");
    private static readonly byte[] Complete = Encoding.UTF8.GetBytes("TOKEN_PROBE_ONLY_COMPLETE\n");
    private static readonly byte[] Hold = Encoding.UTF8.GetBytes("HOLD_CLEANUP_NOT_PROVEN\n");

    static int Main(string[] args)
    {
        var wall = Stopwatch.StartNew();
        try
        {
            if (args.Length > 0 && AdminController.IsMode(args[0]))
            {
                Native.Need(ReleaseAdmissionImplemented, "HOLD_NATIVE_ADMIN_VALIDATION_PENDING");
                return AdminWindows.Run(args);
            }
            Native.Need(args.Length == 5 && args[0] == "--supervise" && args[1] == "--contract" && args[3] == "--contract-sha", "MODE_NOT_IMPLEMENTED");
            Native.Need(Environment.ProcessPath == Path.Combine(Native.CodeRoot, "Supervisor.exe"), "SUPERVISOR_LOCATION");
            Native.Need(args[2] == Path.Combine(Native.CodeRoot, "TrialContract.json"), "CONTRACT_LOCATION");
            // 핵심 구현은 연결했지만 실제 Windows admission/정리 검증 전에는 진입을 열지 않습니다.
            Native.Need(ReleaseAdmissionImplemented, "HOLD_NATIVE_ADMISSION_VALIDATION_PENDING");
            using var admission = ReleaseAdmission.Acquire(args[2], args[4]);
            Native.Need(wall.ElapsedMilliseconds < 5000, "PRECREATE_BUDGET");
            return Supervise(admission, admission.Identity, wall);
        }
        catch { return 70; }
    }
    private static void DurableNew(string path, byte[] bytes)
    {
        // CREATE_NEW 실패를 성공으로 덮어쓰지 않으며 기존 승인소비와 별개 trial 기록만 씁니다.
        using var file = AdmissionWindows.CreateRecord(path);
        file.Write(bytes); ((FileStream)file).Flush(true);
    }
    private static int Supervise(ReleaseAdmission admission, Native.TokenStats own, Stopwatch wall)
    {
        TrialContract contract = admission.Contract;
        nint job = 0, attributes = 0, jobValue = 0, policyValue = 0, environment = 0, securityAttributes = 0;
        Native.ProcessInfo child = default;
        bool attributesReady = false, terminated = false, cleanup = false, completed = false;
        string prefix = Path.Combine(Native.EvidenceRoot, contract.TrialId);
        // 폴더/ACL을 런타임에 생성·수정하지 않습니다. 사전 승인된 staging이 없으면 실패합니다.
        DurableNew(prefix + ".created-once", Intent);
        try
        {
            var security = admission.Security.Attributes;
            securityAttributes = Marshal.AllocHGlobal(Marshal.SizeOf<AdmissionWindows.Attributes>());
            Marshal.StructureToPtr(security, securityAttributes, false);
            job = Native.CreateJobObjectW(securityAttributes, null); Native.Check(job != 0);
            admission.Security.Harden(job,AdmissionWindows.KernelKind.Job);
            Native.Check(Native.SetHandleInformation(job, 1, 0));
            Native.Check(Native.GetHandleInformation(job, out uint handleFlags)); Native.Need((handleFlags & 1) == 0, "INHERITABLE_JOB");
            var limits = new Native.ExtendedLimits(); limits.Basic.Flags = 0x2008; limits.Basic.Active = 1;
            Native.Check(Native.SetInformationJobObject(job, 9, ref limits, (uint)Marshal.SizeOf<Native.ExtendedLimits>()));
            Native.Check(Native.QueryLimits(job, 9, out var observed, (uint)Marshal.SizeOf<Native.ExtendedLimits>(), 0));
            Native.Need(observed.Basic.Flags == 0x2008 && observed.Basic.Active == 1, "JOB_LIMIT_READBACK");
            nuint size = 0; bool queried = Native.InitializeProcThreadAttributeList(0, 2, 0, ref size);
            Native.Need(!queried && Marshal.GetLastWin32Error() == 122 && size > 0 && size < 65536, "ATTRIBUTE_SIZE");
            attributes = Marshal.AllocHGlobal(checked((int)size));
            Native.Check(Native.InitializeProcThreadAttributeList(attributes, 2, 0, ref size)); attributesReady = true;
            jobValue = Marshal.AllocHGlobal(nint.Size); Marshal.WriteIntPtr(jobValue, job);
            policyValue = Marshal.AllocHGlobal(4); Marshal.WriteInt32(policyValue, 1);
            // JOB_LIST는 생성시 편입, CHILD_PROCESS_POLICY는 대상의 자식 생성을 제한합니다.
            Native.Check(Native.UpdateProcThreadAttribute(attributes, 0, 0x2000d, jobValue, (nuint)nint.Size, 0, 0));
            Native.Check(Native.UpdateProcThreadAttribute(attributes, 0, 0x2000e, policyValue, 4, 0, 0));
            string systemRoot = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
            environment = Marshal.StringToHGlobalUni("SystemRoot=" + systemRoot + "\0WINDIR=" + systemRoot + "\0\0");
            var startup = new Native.StartupEx { Start = new Native.Startup { Size = (uint)Marshal.SizeOf<Native.StartupEx>() }, Attributes = attributes };
            string probe = Path.Combine(Native.CodeRoot, "Probe.exe");
            Native.Need(wall.ElapsedMilliseconds < 7000, "CREATE_BUDGET");
            Native.Check(Native.CreateProcessW(probe, new StringBuilder("\"" + probe + "\""), securityAttributes, securityAttributes, false, 0x08080404, environment, Native.CodeRoot, ref startup, out child));
            Native.Check(Native.IsProcessInJob(child.Process, job, out bool inOurJob)); Native.Need(inOurJob, "WRONG_JOB");
            Native.Check(Native.QueryAccounting(job, 1, out var accounting, (uint)Marshal.SizeOf<Native.Accounting>(), 0));
            Native.Need(accounting.Active == 1 && accounting.Total == 1, "JOB_COUNT");
            var target = Native.Identity(child.Process);
            Native.Need(Native.Id(target.AuthenticationId) == Native.Id(own.AuthenticationId), "AUTH_LUID_CHANGED");
            Native.Check(Native.GetProcessTimes(child.Process, out long created, out long exited, out _, out _)); Native.Need(created > 0 && exited == 0, "GENERATION");
            // PIDだけではなく作成時に返されたhandleと実tokenをそのまま保持します。
            DurableNew(prefix + ".generation", Encoding.UTF8.GetBytes(child.Pid + "\n" + created + "\n" + Native.Id(target.TokenId) + "\n" + Native.Id(target.AuthenticationId) + "\n"));
            admission.BeforeResume(job, child);
            Native.Need(wall.ElapsedMilliseconds < 8000, "RESUME_BUDGET");
            Native.Check(Native.ResumeThread(child.Thread) != uint.MaxValue);
            var result = JobMonitor.Run(new NativeJobOperations(job, child.Process, wall, () => admission.CheckIsolation(child.Pid)), CancellationToken.None);
            terminated = result.Terminated; cleanup = result.Drained; completed = result.Completed;
            Native.Need(completed, "EXIT_OR_DRAIN_UNKNOWN");
        }
        finally
        {
            // 성공/오류/취소시에도 자기Job에만 작용합니다. 강제 프로세스종료시 finally는 가정하지 않습니다.
            if (job != 0 && child.Process != 0 && !cleanup)
            {
                if (!terminated) Native.TerminateJobObject(job, 126);
                cleanup = JobMonitor.Drain(new NativeJobOperations(job, child.Process, wall));
            }
            if (child.Thread != 0) cleanup &= Native.CloseHandle(child.Thread);
            if (child.Process != 0) cleanup &= Native.CloseHandle(child.Process);
            if (attributesReady) Native.DeleteProcThreadAttributeList(attributes);
            if (attributes != 0) Marshal.FreeHGlobal(attributes);
            if (jobValue != 0) Marshal.FreeHGlobal(jobValue);
            if (policyValue != 0) Marshal.FreeHGlobal(policyValue);
            if (environment != 0) Marshal.FreeHGlobal(environment);
            if (securityAttributes != 0) Marshal.FreeHGlobal(securityAttributes);
            // 마지막 handle 닫힘이 커널 kill-on-close의 경계이며 Worker에 이 handle을 넘기지 않습니다.
            if (job != 0) cleanup &= Native.CloseHandle(job);
            completed &= cleanup && terminated && wall.ElapsedMilliseconds < 90000;
            DurableNew(prefix + ".final", completed ? Complete : Hold);
        }
        return completed ? 0 : 72;
    }
}

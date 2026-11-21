// 시험용 두 프로그램이 공통으로 쓰는 Windows 신원·Job 자료형입니다.
// 로그온/계정 수정/Task 등록/IPC API를 포함하지 않습니다.
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security.Principal;

internal static partial class Native
{
    internal const uint WaitTimeout = 258;
    [StructLayout(LayoutKind.Sequential)] internal struct Luid { public uint Low; public int High; }
    [StructLayout(LayoutKind.Sequential)] internal struct TokenStats
    {
        public Luid TokenId, AuthenticationId; public long Expiration;
        public int Type, Impersonation; public uint Charged, Available, Groups, Privileges; public Luid ModifiedId;
    }
    [StructLayout(LayoutKind.Sequential)] internal struct LsaText { public ushort Length, Maximum; public nint Buffer; }
    [StructLayout(LayoutKind.Sequential)] internal struct LogonData
    {
        public uint Size; public Luid Id; public LsaText User, Domain, Package; public uint LogonType, Session; public nint Sid; public long Time;
    }
    [StructLayout(LayoutKind.Sequential)] internal struct BasicLimits
    {
        public long ProcessTime, JobTime; public uint Flags; public nuint Minimum, Maximum;
        public uint Active; public nuint Affinity; public uint Priority, Scheduling;
    }
    [StructLayout(LayoutKind.Sequential)] internal struct IoCounters { public ulong A, B, C, D, E, F; }
    [StructLayout(LayoutKind.Sequential)] internal struct ExtendedLimits
    {
        public BasicLimits Basic; public IoCounters Io; public nuint ProcessMemory, JobMemory, PeakProcess, PeakJob;
    }
    [StructLayout(LayoutKind.Sequential)] internal struct Accounting
    {
        public long User, Kernel, PeriodUser, PeriodKernel; public uint Faults, Total, Active, Terminated;
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)] internal struct Startup
    {
        public uint Size; public nint Reserved; [MarshalAs(UnmanagedType.LPWStr)] public string? Desktop;
        public nint Title; public uint X, Y, XSize, YSize, XChars, YChars, Fill, Flags;
        public ushort Show, ReservedSize; public nint Reserved2, Input, Output, Error;
    }
    [StructLayout(LayoutKind.Sequential)] internal struct StartupEx { public Startup Start; public nint Attributes; }
    [StructLayout(LayoutKind.Sequential)] internal struct ProcessInfo { public nint Process, Thread; public uint Pid, Tid; }
    [DllImport("kernel32.dll")] internal static extern nint GetCurrentProcess();
    [DllImport("kernel32.dll")] internal static extern nint GetCurrentThread();
    [DllImport("kernel32.dll")] internal static extern ulong GetTickCount64();
    [DllImport("kernel32.dll", SetLastError = true)] internal static extern bool CloseHandle(nint handle);
    [DllImport("advapi32.dll", SetLastError = true)] internal static extern bool OpenProcessToken(nint process, uint access, out nint token);
    [DllImport("advapi32.dll", SetLastError = true)] internal static extern bool OpenThreadToken(nint thread, uint access, bool openAsSelf, out nint token);
    [DllImport("advapi32.dll", SetLastError = true)] internal static extern bool GetTokenInformation(nint token, int kind, nint buffer, uint size, out uint needed);
    [DllImport("secur32.dll")] internal static extern uint LsaGetLogonSessionData(ref Luid id, out nint data);
    [DllImport("secur32.dll")] internal static extern uint LsaFreeReturnBuffer(nint data);
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)] internal static extern nint CreateJobObjectW(nint attributes, string? name);
    [DllImport("kernel32.dll", SetLastError = true)] internal static extern bool SetInformationJobObject(nint job, int kind, ref ExtendedLimits data, uint size);
    [DllImport("kernel32.dll", EntryPoint = "QueryInformationJobObject", SetLastError = true)] internal static extern bool QueryLimits(nint job, int kind, out ExtendedLimits data, uint size, nint returned);
    [DllImport("kernel32.dll", EntryPoint = "QueryInformationJobObject", SetLastError = true)] internal static extern bool QueryAccounting(nint job, int kind, out Accounting data, uint size, nint returned);
    [DllImport("kernel32.dll", SetLastError = true)] internal static extern bool IsProcessInJob(nint process, nint job, out bool present);
    [DllImport("kernel32.dll", SetLastError = true)] internal static extern bool SetHandleInformation(nint handle, uint mask, uint flags);
    [DllImport("kernel32.dll", SetLastError = true)] internal static extern bool GetHandleInformation(nint handle, out uint flags);
    [DllImport("kernel32.dll", SetLastError = true)] internal static extern bool InitializeProcThreadAttributeList(nint list, int count, uint flags, ref nuint size);
    [DllImport("kernel32.dll", SetLastError = true)] internal static extern bool UpdateProcThreadAttribute(nint list, uint flags, nuint attribute, nint value, nuint size, nint previous, nint returned);
    [DllImport("kernel32.dll")] internal static extern void DeleteProcThreadAttributeList(nint list);
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)] internal static extern bool CreateProcessW(string application, System.Text.StringBuilder command, nint processAttributes, nint threadAttributes, bool inheritHandles, uint flags, nint environment, string cwd, ref StartupEx startup, out ProcessInfo process);
    [DllImport("kernel32.dll", SetLastError = true)] internal static extern uint ResumeThread(nint thread);
    [DllImport("kernel32.dll", SetLastError = true)] internal static extern uint WaitForSingleObject(nint handle, uint milliseconds);
    [DllImport("kernel32.dll", SetLastError = true)] internal static extern bool TerminateJobObject(nint job, uint exitCode);
    [DllImport("kernel32.dll", SetLastError = true)] internal static extern bool GetExitCodeProcess(nint process, out uint exitCode);
    [DllImport("kernel32.dll", SetLastError = true)] internal static extern bool GetProcessTimes(nint process, out long created, out long exited, out long kernel, out long user);
    internal static void Need(bool value, string tag) { if (!value) throw new InvalidOperationException(tag); }
    internal static void Check(bool value) { if (!value) throw new Win32Exception(Marshal.GetLastWin32Error()); }
    // PtrToStructure의 생성자 보존 요구를 실제 형식 T로 전달합니다. 네이티브 배치는 변경하지 않습니다.
    internal static T Info<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] T>(nint token, int kind) where T : struct
    {
        int length = Marshal.SizeOf<T>(); nint p = Marshal.AllocHGlobal(length);
        try { Check(GetTokenInformation(token, kind, p, (uint)length, out uint needed)); Need(needed <= length, "TOKEN_SIZE"); return Marshal.PtrToStructure<T>(p); }
        finally { Marshal.FreeHGlobal(p); }
    }
    internal static string Id(Luid id) => unchecked((uint)id.High).ToString("x8") + id.Low.ToString("x8");
    // Task 설정값 대신 대상 process의 primary token과 LSA 실데이터로 Batch4/Session0을 확인합니다.
    internal static TokenStats Identity(nint process)
    {
        Check(OpenProcessToken(process, 8, out nint token));
        try
        {
            using var identity = new WindowsIdentity(token);
            Need(identity.User?.Value == Worker, "WRONG_SID");
            TokenPolicy.NoAdministrativeGroups(token);
            Need(Info<int>(token, 12) == 0 && Info<int>(token, 20) == 0, "SESSION_OR_ELEVATION");
            TokenStats stats = Info<TokenStats>(token, 10); Need(stats.Type == 1, "PRIMARY_TOKEN");
            uint status = LsaGetLogonSessionData(ref stats.AuthenticationId, out nint data);
            Need(status == 0 && data != 0, "LSA_UNKNOWN");
            try
            {
                var lsa = Marshal.PtrToStructure<LogonData>(data);
                Need(lsa.Size >= Marshal.SizeOf<LogonData>() && lsa.LogonType == 4 && lsa.Session == 0 && lsa.Sid != 0 && new SecurityIdentifier(lsa.Sid).Value == Worker, "BATCH4_SESSION0");
            }
            finally { LsaFreeReturnBuffer(data); }
            return stats;
        }
        finally { CloseHandle(token); }
    }
    internal static void NoImpersonation()
    {
        if (OpenThreadToken(GetCurrentThread(), 8, true, out nint token)) { CloseHandle(token); throw new InvalidOperationException("THREAD_TOKEN"); }
        Need(Marshal.GetLastWin32Error() == 1008, "THREAD_TOKEN_UNKNOWN");
    }
}

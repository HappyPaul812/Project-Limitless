using System.ComponentModel;
using System.Runtime.InteropServices;

// 외부 HANDLE은 기존 권한을 DACL 변경만으로 회수할 수 없으므로 별도 관측합니다.
// 정보 class64의 Windows 내부 ABI/주소 노출은 공개 안정 계약이 아닙니다.
// 버퍼/주소/관측을 신뢰할 수 없으면 빈 결과를 성공으로 간주하지 않고 HOLD합니다.
internal static class HandleCensus
{
    [DllImport("ntdll.dll")] private static extern int NtQuerySystemInformation(int kind,nint buffer,uint length,out uint needed);
    [DllImport("kernel32.dll",SetLastError=true)] private static extern nint CreateToolhelp32Snapshot(uint flags,uint pid);
    [DllImport("kernel32.dll",CharSet=CharSet.Unicode,SetLastError=true)] private static extern bool Process32FirstW(nint snapshot,ref ProcessEntry entry);
    [DllImport("kernel32.dll",CharSet=CharSet.Unicode,SetLastError=true)] private static extern bool Process32NextW(nint snapshot,ref ProcessEntry entry);
    [StructLayout(LayoutKind.Sequential,CharSet=CharSet.Unicode)] private struct ProcessEntry
    {
        internal uint Size,Usage,Pid; internal nuint Heap; internal uint Module,Threads,Parent;
        internal int Priority; internal uint Flags; [MarshalAs(UnmanagedType.ByValTStr,SizeConst=260)] internal string Name;
    }
    internal static List<HandleEntry> Parse(byte[] data)
    {
        AdmissionPolicy.Require(data.Length>=16,"HANDLE_HEADER");
        ulong count=BitConverter.ToUInt64(data);
        AdmissionPolicy.Require(count>0 && count<=(ulong)(data.Length-16)/40,"HANDLE_COUNT");
        var entries=new List<HandleEntry>((int)count);
        for(int i=0;i<(int)count;i++)
        {
            int offset=16+i*40;
            entries.Add(new(BitConverter.ToUInt64(data,offset),BitConverter.ToUInt64(data,offset+8),BitConverter.ToUInt64(data,offset+16),BitConverter.ToUInt32(data,offset+24)));
        }
        return entries;
    }
    internal static void Check(IReadOnlyDictionary<nint,uint> handles)
    {
        AdmissionPolicy.Require(nint.Size==8,"HANDLE_ABI_X64_REQUIRED");
        uint size=1024*1024;
        for(int attempt=0;attempt<5;attempt++)
        {
            AdmissionPolicy.Require(size<=32*1024*1024,"HANDLE_BUFFER_LIMIT");
            nint p=Marshal.AllocHGlobal((int)size);
            try
            {
                int status=NtQuerySystemInformation(64,p,size,out uint needed);
                if(status==unchecked((int)0xc0000004)){size=checked(Math.Max(size*2,needed+65536));continue;}
                AdmissionPolicy.Require(status==0 && needed>=16 && needed<=size,"HANDLE_QUERY_UNKNOWN");
                byte[] bytes=new byte[needed];Marshal.Copy(p,bytes,0,bytes.Length);
                var entries=Parse(bytes); ulong self=AdmissionWindows.GetCurrentProcessId();
                var objects=new Dictionary<ulong,uint>();
                foreach(var pair in handles)
                {
                    var found=entries.Where(e=>e.Pid==self && e.Handle==(ulong)(nuint)pair.Key).ToArray();
                    AdmissionPolicy.Require(found.Length==1 && found[0].Object!=0,"HANDLE_IDENTITY_REDACTED_OR_UNKNOWN");
                    objects[found[0].Object]=objects.GetValueOrDefault(found[0].Object)|pair.Value;
                }
                // SYSTEM/관리자라는 추측으로 외부 handle을 예외 처리하지 않습니다.
                // Scheduler가 위험 권리 handle을 유지하는 경우도 실제 시험은 HOLD가 됩니다.
                AdmissionPolicy.ForeignHandles(entries,objects,self);return;
            }
            finally{Marshal.FreeHGlobal(p);}
        }
        throw new InvalidOperationException("HANDLE_CENSUS_UNSTABLE");
    }

    internal static void Exclusive(uint? child)
    {
        nint snapshot=CreateToolhelp32Snapshot(2,0); AdmissionWindows.Check(snapshot!=new nint(-1));
        try
        {
            var entry=new ProcessEntry { Size=(uint)Marshal.SizeOf<ProcessEntry>(),Name="" };
            AdmissionWindows.Check(Process32FirstW(snapshot,ref entry));
            bool seenSelf=false,seenChild=child is null; uint self=AdmissionWindows.GetCurrentProcessId();
            do
            {
                if(entry.Pid==self){seenSelf=true;continue;}
                if(entry.Pid==child){seenChild=true;continue;}
                if(entry.Pid==0)continue; // Idle은 사용자 process가 아닙니다.
                nint process=AdmissionWindows.OpenProcess(0x1000,false,entry.Pid);
                if(process==0)throw new Win32Exception(Marshal.GetLastWin32Error(),"PROCESS_CENSUS_UNKNOWN");
                try
                {
                    Native.Check(Native.OpenProcessToken(process,8,out nint token));
                    try {using var identity=new System.Security.Principal.WindowsIdentity(token);AdmissionPolicy.Require(identity.User is not null && identity.User.Value!=Native.Worker,"EXTRA_WORKER_PROCESS");}
                    finally{Native.CloseHandle(token);}
                }
                finally{Native.CloseHandle(process);}
            }while(Process32NextW(snapshot,ref entry));
            AdmissionPolicy.Require(Marshal.GetLastWin32Error()==18 && seenSelf && seenChild,"PROCESS_CENSUS_INCOMPLETE");
        }
        finally{Native.CloseHandle(snapshot);}
    }
}

using System.Diagnostics;
using System.Runtime.InteropServices;

internal interface IJobOperations
{
    long Elapsed { get; }
    uint Wait(uint milliseconds);
    (uint Active,uint Total) Counts();
    void Terminate(uint code);
    uint ExitCode();
    void CheckIsolation();
}
internal readonly record struct JobResult(bool Completed,bool Terminated,bool Drained);

// 실제 감독 루프와 단위 시험이 공유하는 deadline/정리 로직입니다.
// OS 정지/API block까지 절대 wall-clock 보장으로 확대하지 않습니다.
internal static class JobMonitor
{
    internal static JobResult Run(IJobOperations job,CancellationToken cancellation)
    {
        bool terminated=false,drained=false,completed=false;
        try
        {
            AdmissionPolicy.Require(job.Elapsed>=0,"NEGATIVE_MONOTONIC_TIME");
            cancellation.ThrowIfCancellationRequested();
            long nextIsolationCheck=job.Elapsed+1000;
            while(job.Elapsed<10000)
            {
                cancellation.ThrowIfCancellationRequested();
                if(job.Elapsed>=nextIsolationCheck){job.CheckIsolation();nextIsolationCheck=job.Elapsed+1000;}
                AdmissionPolicy.Require(job.Wait(20)==Native.WaitTimeout,"PROBE_EXITED_BEFORE_FORCE_CHECK");
            }
            job.CheckIsolation();
            AdmissionPolicy.Require(job.Elapsed<85000,"WATCHDOG_LATE");
            job.Terminate(125);terminated=true;drained=Drain(job);
            AdmissionPolicy.Require(drained&&job.ExitCode()==125&&job.Elapsed<90000,"EXIT_OR_DRAIN_UNKNOWN");completed=true;
        }
        catch { /* 실패는 성공으로 승격하지 않고, 아래 자기 Job 정리를 한 번 수행합니다. */ }
        finally
        {
            if(!completed)
            {
                try{if(!terminated){job.Terminate(126);terminated=true;}drained=Drain(job);}
                catch{drained=false;}
            }
        }
        return new(completed,terminated,drained);
    }
    internal static bool Drain(IJobOperations job)
    {
        while(job.Elapsed>=0&&job.Elapsed<90000)
        {
            var count=job.Counts();uint wait=job.Wait(20);
            if(wait==0&&count.Active==0&&count.Total==1)return true;
            if(wait!=0&&wait!=Native.WaitTimeout)return false;
            if(count.Total!=1)return false;
        }
        return false;
    }
}
internal sealed class NativeJobOperations(nint job,nint process,Stopwatch wall,Action? isolation=null):IJobOperations
{
    public long Elapsed=>wall.ElapsedMilliseconds;
    public uint Wait(uint milliseconds)=>Native.WaitForSingleObject(process,milliseconds);
    public (uint Active,uint Total) Counts()
    {Native.Check(Native.QueryAccounting(job,1,out var count,(uint)Marshal.SizeOf<Native.Accounting>(),0));return(count.Active,count.Total);}
    public void Terminate(uint code)=>Native.Check(Native.TerminateJobObject(job,code));
    public uint ExitCode(){Native.Check(Native.GetExitCodeProcess(process,out uint code));return code;}
    public void CheckIsolation()
    {
        var count=Counts();AdmissionPolicy.Require(count.Active==1&&count.Total==1,"JOB_LIVE_COUNT");
        (isolation??throw new InvalidOperationException("ISOLATION_CALLBACK_REQUIRED"))();
    }
}

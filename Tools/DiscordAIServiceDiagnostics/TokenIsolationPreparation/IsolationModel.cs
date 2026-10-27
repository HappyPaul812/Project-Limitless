// 가상 시계/핸들만 사용하는 사전 준비 모델입니다. Windows 실행 API가 없습니다.
using System;using System.Collections.Generic;
namespace Limitless.TokenIsolationPreparation {
 public sealed class Gates {
  public bool Session0LauncherVerified,PrivilegesAlreadyAvailable,AtomicJobList,ChildPolicy,NoForeignPrivilegedHandles,ImageSealed,PrivateNonInheritedJob,CleanEnvironment,PreservationKnown;
 }
 public sealed class Result {
  public string Status="HOLD",Stage="GATES",Cleanup="NOT_NEEDED";
  public int FakeLogons,FakeProcesses,FakeActiveProcesses,FakeOpenHandles,PeakProcesses;
  public int WorkerTerminateDeadlineSeconds=85,OverallDeadlineSeconds=90;
  public bool DurableIntent,FinalEvidence,KernelCloseOnly,NativeExecuted=false,RuntimeAdmitted=false;
  public List<string> Trace=new List<string>();
 }
 public static class IsolationModel {
  public static Result Simulate(Gates g,string fault){
   var r=new Result();bool job=false,worker=false,closedByKernel=false;
   try{
    if(g==null||!g.Session0LauncherVerified||!g.PrivilegesAlreadyAvailable||!g.AtomicJobList||!g.ChildPolicy||!g.NoForeignPrivilegedHandles||!g.ImageSealed||!g.PrivateNonInheritedJob||!g.CleanEnvironment||!g.PreservationKnown)throw new InvalidOperationException("GATE_HOLD");
    r.Stage="DURABLE_INTENT";r.DurableIntent=true;r.Trace.Add("INTENT_NEW_TRIAL_ONLY");
    if(fault=="JOURNAL_FAILURE"){r.DurableIntent=false;throw new InvalidOperationException();}
    r.Stage="PRIVATE_JOB";job=true;r.FakeOpenHandles=1;r.Trace.Add("KILL_ON_CLOSE_ACTIVE_LIMIT1_NO_BREAKAWAY");
    if(fault=="JOB_READBACK_FAILURE")throw new InvalidOperationException();
    r.Stage="FAKE_BATCH_LOGON";r.FakeLogons=1;r.FakeOpenHandles++;r.Trace.Add("BATCH4_TOKEN_SID1010_SESSION0_REQUIRED");
    if(fault=="LOGON_FAILURE"||fault=="SESSION_NOT_ZERO"||fault=="WRONG_SID"||fault=="PRIVILEGE_MISSING")throw new InvalidOperationException();
    r.Stage="ATOMIC_JOB_AND_CHILD_RESTRICTED_CREATE";r.FakeProcesses=1;r.FakeActiveProcesses=1;r.PeakProcesses=1;r.FakeOpenHandles+=2;worker=true;r.Trace.Add("ATOMIC_JOB_LIST_CREATE_SUSPENDED");
    if(fault=="CREATE_READBACK_FAILURE"||fault=="PID_REUSE"||fault=="TOKEN_MUTATION")throw new InvalidOperationException();
    if(fault=="WINDOW_CLOSE"){r.KernelCloseOnly=true;closedByKernel=true;r.Trace.Add("OWNER_EXIT_LAST_JOB_HANDLE_CLOSED");throw new InvalidOperationException();}
    r.Stage="TOKEN_OBSERVE";r.Trace.Add("SID_PID_CREATION_AUTH_BATCH4_SESSION0_GROUPS_RESTRICTED_PRIVILEGES_ACCESSCHECK");
    if(fault=="CANCEL"||fault=="ACCESSCHECK_FAILURE"||fault=="TIMEOUT"||fault=="CHILD_ATTEMPT"){if(fault=="TIMEOUT")r.Trace.Add("MONOTONIC_85S_TERMINATE_RESERVED_5S_DRAIN");throw new InvalidOperationException();}
    r.FakeActiveProcesses=0;worker=false;r.Status="OFFLINE_MODEL_COMPLETED_NOT_WINDOWS_VERIFIED";
   }catch{r.Status="HOLD";}finally{
    if(closedByKernel){r.Trace.Add("KERNEL_MODEL_LAST_JOB_HANDLE_KILLS_WORKER_NO_USER_FINALLY");r.FakeActiveProcesses=0;r.FakeOpenHandles=0;r.Cleanup="KERNEL_CLOSE_MODEL_ONLY_NOT_NATIVE_PROOF";worker=false;job=false;}
    else{
     if(worker&&job){r.Trace.Add("TERMINATE_ONLY_OWN_JOB");r.FakeActiveProcesses=0;worker=false;}
     if(job){r.Trace.Add("CLOSE_JOB_TOKEN_THREAD_PROCESS_HANDLES");r.FakeOpenHandles=0;r.Cleanup="OFFLINE_OWN_JOB_EMPTY_HANDLES_ZERO";}
    }
    r.FinalEvidence=r.DurableIntent&&!closedByKernel;
    if(fault=="FINAL_JOURNAL_FAILURE"){r.FinalEvidence=false;r.Status="HOLD";}
    if(fault=="CLEANUP_UNCONFIRMED"){r.FakeActiveProcesses=-1;r.FakeOpenHandles=-1;r.Cleanup="UNKNOWN_REVIEW_NO_AUTOMATIC_RETRY";r.Status="HOLD";}
   }
   return r;
  }
 }
}

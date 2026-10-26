// 모델만 호출합니다. 실제 파일 사용 조회/Scheduler/Worker/계정 API는 구현하지 않습니다.
using System;
using System.Collections.Generic;
using System.Linq;
namespace LimitlessTrialAlternativesOffline {
 public sealed class FakeRm:IRm {
  public int StartCode,RegisterCode,EndCode,Starts,Registers,Lists,Ends,RegisteredCount;
  public bool ThrowRegister,ThrowEnd;
  public Queue<RmReply> Replies=new Queue<RmReply>();
  public int Start(){Starts++;return StartCode;}
  public int Register(string[] paths){Registers++;RegisteredCount=paths.Length;if(ThrowRegister)throw new Exception("SYNTHETIC_SECRET");return RegisterCode;}
  public RmReply GetList(int capacity){Lists++;return Replies.Dequeue();}
  public int End(){Ends++;if(ThrowEnd)throw new Exception("SYNTHETIC_SECRET");return EndCode;}
 }
 public static class AlternativeTests {
  static readonly List<string> log=new List<string>();
  static void Need(bool b){if(!b)throw new InvalidOperationException("ASSERT_FAILED");}
  static void Test(string name,Action body){try{body();log.Add("PASS "+name);}catch{log.Add("FAIL "+name);}}
  static FileEvidence[] Copy(FileEvidence[] f){return f.Select(x=>new FileEvidence{Relative=x.Relative,ExpectedSha=x.ExpectedSha,ObservedSha=x.ObservedSha}).ToArray();}
  static void BadResource(FileEvidence[] source,Action<FileEvidence[]> mutate){var f=Copy(source);mutate(f);var api=new FakeRm();bool hit=false;try{ResourceModel.Evaluate(f,api);}catch{hit=true;}Need(hit && api.Starts==0 && api.Registers==0);}
  static FakeRm Empty(){var api=new FakeRm();api.Replies.Enqueue(new RmReply());return api;}
  static FakeWorld World(){return new FakeWorld{NamespaceSeparationAcknowledged=true};}
  static string Snapshot(FakeWorld w){return string.Join("|",w.ProtectedAssets.OrderBy(x=>x.Key).Select(x=>x.Key+"="+x.Value));}
  static void BadPlan(FileEvidence[] f,Action<V2Plan,FakeWorld> mutate){var p=new V2Plan();var w=World();mutate(p,w);var before=Snapshot(w);var r=V2Model.Simulate(p,f,w);Need(r.Status.EndsWith("HOLD") && w.Attempts==0 && before==Snapshot(w) && w.ActualCalls==0 && !r.InstallAuthorized);}
  public static string[] Run(FileEvidence[] baseline){
   log.Clear();
   Test("RM-exact855-single-registration-empty-still-HOLD",()=>{var a=Empty();var r=ResourceModel.Evaluate(Copy(baseline),a);Need(a.Registers==1 && a.RegisteredCount==855 && a.Ends==1 && r.Status=="RM_NO_USERS_REPORTED_HOLD" && !r.ReferenceResolved);});
   Test("RM-busy-growth-reports-and-HOLD",()=>{var a=new FakeRm();a.Replies.Enqueue(new RmReply{Code=234,Needed=2});a.Replies.Enqueue(new RmReply{Count=2,Needed=2});var r=ResourceModel.Evaluate(Copy(baseline),a);Need(r.Busy && r.ReportedUsers==2 && r.Status=="RM_USERS_REPORTED_HOLD" && a.Ends==1);});
   Test("RM-growth-race-bounded-three-calls",()=>{var a=new FakeRm();for(int i=1;i<=3;i++)a.Replies.Enqueue(new RmReply{Code=234,Needed=i});var r=ResourceModel.Evaluate(Copy(baseline),a);Need(a.Lists==3 && a.Ends==1 && r.Status=="RM_LIST_UNSTABLE_HOLD");});
   Test("RM-session-limit-no-end-without-start",()=>{var a=new FakeRm{StartCode=353};var r=ResourceModel.Evaluate(Copy(baseline),a);Need(r.Code==353 && a.Ends==0 && a.Registers==0);});
   foreach(int code in new[]{5,6,29,121,160,1223}){int c=code;Test("RM-list-code-"+c+"-HOLD",()=>{var a=new FakeRm();a.Replies.Enqueue(new RmReply{Code=c});var r=ResourceModel.Evaluate(Copy(baseline),a);Need(r.Code==c && r.Status.EndsWith("HOLD") && a.Ends==1);});}
   Test("RM-register-failure-finally-end",()=>{var a=new FakeRm{RegisterCode=29};var r=ResourceModel.Evaluate(Copy(baseline),a);Need(r.Code==29 && a.Ends==1 && a.Lists==0);});
   Test("RM-register-exception-sanitized-finally-end",()=>{var a=new FakeRm{ThrowRegister=true};var r=ResourceModel.Evaluate(Copy(baseline),a);Need(r.Status=="RM_EXCEPTION_HOLD" && a.Ends==1);});
   Test("RM-end-failure-retains-busy",()=>{var a=new FakeRm{EndCode=29};a.Replies.Enqueue(new RmReply{Code=234,Needed=1});a.Replies.Enqueue(new RmReply{Count=1});var r=ResourceModel.Evaluate(Copy(baseline),a);Need(r.Busy && r.EndCode==29 && r.Status=="RM_END_FAILED_HOLD");});
   Test("RM-end-exception-HOLD",()=>{var a=Empty();a.ThrowEnd=true;Need(ResourceModel.Evaluate(Copy(baseline),a).Status=="RM_END_EXCEPTION_HOLD");});
   Test("RM-unreasonable-buffer-cap-HOLD",()=>{var a=new FakeRm();a.Replies.Enqueue(new RmReply{Code=234,Needed=4097});Need(ResourceModel.Evaluate(Copy(baseline),a).Status=="RM_BUFFER_UNKNOWN_HOLD");});
   Test("RM-reboot-reason-no-reboot-action",()=>{var a=new FakeRm();a.Replies.Enqueue(new RmReply{RebootReasons=4});Need(ResourceModel.Evaluate(Copy(baseline),a).Status=="RM_REBOOT_REASON_HOLD");});
   Test("RM-directory-rejected-before-session",()=>BadResource(baseline,f=>f[0].PlainFile=false));
   Test("RM-missing-file-rejected",()=>BadResource(baseline,f=>f[0].Exists=false));
   Test("RM-sha-mismatch-rejected",()=>BadResource(baseline,f=>f[0].ObservedSha=new string('f',64)));
   Test("RM-reparse-rejected",()=>BadResource(baseline,f=>f[0].Reparse=true));
   Test("RM-hardlink-unknown-alias-rejected",()=>BadResource(baseline,f=>f[0].Hardlink=true));
   Test("RM-protection-unknown-rejected",()=>BadResource(baseline,f=>f[0].Protected=false));
   foreach(string path in new[]{"../worker.exe","Code/../worker.exe","Code/a:stream","Code/a\\b","Code/x?","Code/x.","Code//x"}){string n=path;Test("RM-invalid-path-"+n,()=>BadResource(baseline,f=>f[0].Relative=n));}
   Test("RM-duplicate-case-insensitive-rejected",()=>BadResource(baseline,f=>f[1].Relative=f[0].Relative.ToUpperInvariant()));
   Test("RM-854-not855-rejected",()=>{var a=Empty();bool hit=false;try{ResourceModel.Evaluate(Copy(baseline).Take(854).ToArray(),a);}catch{hit=true;}Need(hit && a.Starts==0);});
   Test("V2-simulation-old-assets-worker-auth-boot-preserved-no-execution",()=>{var w=World();var before=Snapshot(w);var r=V2Model.Simulate(new V2Plan(),Copy(baseline),w);Need(r.Status=="V2_OFFLINE_SIMULATED_NOT_INSTALLED" && before==Snapshot(w) && w.FakeAccounts==1 && w.FakeTasks==1 && w.NewAccountDisabled && w.NewTaskDisabled && w.FakeWorkerCalls==0 && w.ActualCalls==0 && r.OldReferenceHold && !r.InstallAuthorized);});
   Test("V2-no-parallel-approval-HOLD",()=>BadPlan(baseline,(p,w)=>w.NamespaceSeparationAcknowledged=false));
   Test("V2-code-collision-old-root-HOLD",()=>BadPlan(baseline,(p,w)=>p.CodeRoot=ResourceModel.OldRoot));
   Test("V2-state-collision-HOLD",()=>BadPlan(baseline,(p,w)=>p.StateRoot=@"C:\ProgramData\LimitlessRelayTrial"));
   Test("V2-old-account-HOLD",()=>BadPlan(baseline,(p,w)=>p.Account="LIMITLESSBotTrial"));
   Test("V2-old-task-HOLD",()=>BadPlan(baseline,(p,w)=>p.Task="LIMITLESS-Trial-S0Host"));
   Test("V2-old-pipe-HOLD",()=>BadPlan(baseline,(p,w)=>p.Pipe="LimitlessPlanning-0123456789abcdef0123456789abcdef"));
   Test("V2-existing-namespace-HOLD",()=>BadPlan(baseline,(p,w)=>w.NewNamesAbsent=false));
   Test("V2-path-alias-HOLD",()=>BadPlan(baseline,(p,w)=>w.NewPathsCanonical=false));
   Test("V2-reparse-HOLD",()=>BadPlan(baseline,(p,w)=>w.NoReparse=false));
   Test("V2-hardlink-HOLD",()=>BadPlan(baseline,(p,w)=>w.NoHardlinks=false));
   Test("V2-no-exclusive-create-HOLD",()=>BadPlan(baseline,(p,w)=>w.ExclusiveCreate=false));
   foreach(string sid in new[]{"S-1-5-18","S-1-5-21-111-222-333-1001","S-1-5-21-111-222-333-1007","S-1-5-21-111-222-333-1008"}){string s=sid;Test("V2-invalid-new-sid-"+s,()=>BadPlan(baseline,(p,w)=>p.BotSid=s));}
   Test("V2-worker-rebinding-HOLD",()=>BadPlan(baseline,(p,w)=>p.WorkerSid="S-1-5-21-111-222-333-1020"));
   Test("V2-stale-approval-HOLD",()=>BadPlan(baseline,(p,w)=>w.ApprovalFresh=false));
   Test("V2-old-consumed-epoch-HOLD",()=>BadPlan(baseline,(p,w)=>w.ApprovalConsumed=true));
   Test("V2-manifest-sha-HOLD",()=>BadPlan(baseline,(p,w)=>p.ManifestSha=new string('b',64)));
   Test("V2-fake-file-sha-HOLD-before-effects",()=>{var f=Copy(baseline);f[0].ObservedSha=new string('e',64);var w=World();var r=V2Model.Simulate(new V2Plan(),f,w);Need(r.Status.EndsWith("HOLD") && w.Attempts==0);});
   Test("V2-account-enabled-HOLD",()=>BadPlan(baseline,(p,w)=>p.Disabled=false));
   Test("V2-account-creation-enabled-window-HOLD",()=>BadPlan(baseline,(p,w)=>p.AccountDisabledAtCreation=false));
   Test("V2-task-enabled-HOLD",()=>BadPlan(baseline,(p,w)=>p.TaskEnabled=true));
   Test("V2-triggers-HOLD",()=>BadPlan(baseline,(p,w)=>p.Triggers=1));
   Test("V2-elevated-task-HOLD",()=>BadPlan(baseline,(p,w)=>p.RunLevel=1));
   Test("V2-retry-setting-HOLD",()=>BadPlan(baseline,(p,w)=>p.RestartCount=1));
   Test("V2-update-task-flags-HOLD",()=>BadPlan(baseline,(p,w)=>p.RegisterFlags=6));
   Test("V2-real-mode-HOLD",()=>BadPlan(baseline,(p,w)=>p.Mode="OWNER_APPROVED_READONLY_ONCE"));
   Test("V2-admin-group-HOLD",()=>BadPlan(baseline,(p,w)=>p.Group="Administrators"));
   Test("V2-debug-right-HOLD",()=>BadPlan(baseline,(p,w)=>p.Right="SeDebugPrivilege"));
   Test("V2-code-worker-write-HOLD",()=>BadPlan(baseline,(p,w)=>p.CodeWriters=new[]{"SY","BA","WORKER"}));
   Test("V2-state-bot-write-HOLD",()=>BadPlan(baseline,(p,w)=>p.StateWriters=new[]{"SY","BA","WORKER","BOT"}));
   Test("V2-task-bot-read-or-run-HOLD",()=>BadPlan(baseline,(p,w)=>p.TaskReaders=new[]{"WORKER","BOT"}));
   Test("V2-preservation-unknown-HOLD",()=>BadPlan(baseline,(p,w)=>w.PreservationKnown=false));
   Test("V2-new-reference-unknown-HOLD",()=>BadPlan(baseline,(p,w)=>w.NewReferencesKnown=false));
   Test("V2-new-reference-busy-HOLD",()=>BadPlan(baseline,(p,w)=>w.NewReferencesBusy=true));
   foreach(string stage in new[]{"publish","account","scheduler","post"}){string s=stage;Test("V2-install-failure-"+s+"-preserve-no-retry",()=>{
    var w=World();w.FailPublish=s=="publish";w.FailAccount=s=="account";w.FailRegister=s=="scheduler";w.FailPostReadback=s=="post";
    var before=Snapshot(w);var p=new V2Plan();var r=V2Model.Simulate(p,Copy(baseline),w);Need(r.Status.EndsWith("HOLD") && w.Attempts==1 && w.ApprovalConsumed && before==Snapshot(w));
    if(w.FakeAccounts>0)Need(w.NewAccountDisabled);if(w.FakeTasks>0)Need(w.NewTaskDisabled);
    r=V2Model.Simulate(p,Copy(baseline),w);Need(r.Status.EndsWith("HOLD") && w.Attempts==1 && before==Snapshot(w) && w.FakeWorkerCalls==0 && w.ActualCalls==0);
   });}
   return log.ToArray();
  }
 }
}

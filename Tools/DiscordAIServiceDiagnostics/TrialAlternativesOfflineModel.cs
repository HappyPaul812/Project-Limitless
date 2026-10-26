// 설계 검증 전용입니다. Windows 호출/설치/IPC 구현은 없으며 실제 승인 근거로 사용할 수 없습니다.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
namespace LimitlessTrialAlternativesOffline {
 public sealed class FileEvidence {
  public string Relative,ExpectedSha,ObservedSha;
  public bool Exists=true,PlainFile=true,Protected=true,Reparse=false,Hardlink=false;
 }
 public sealed class RmReply { public int Code,Needed,Count,RebootReasons; }
 // 실제 DLL에 연결하지 않는 주입 경계입니다. 허용된 네 동작만 모델링합니다.
 public interface IRm {
  int Start(); int Register(string[] paths); RmReply GetList(int capacity); int End();
 }
 public sealed class RmResult {
  public string Status="RM_UNKNOWN_HOLD";
  public int Code,EndCode,Registered,ReportedUsers;
  public bool Busy,ReferenceResolved=false;
 }
 public static class ResourceModel {
  public const string OldRoot=@"C:\Program Files\LimitlessRelayTrial";
  public static void Need(bool value,string code){if(!value)throw new InvalidOperationException(code);}
  public static string[] Validate(FileEvidence[] files){
   Need(files!=null && files.Length==855,"EXACT855_HOLD");
   var paths=new HashSet<string>(StringComparer.OrdinalIgnoreCase);
   foreach(var f in files){
    Need(f!=null && f.Exists && f.PlainFile && f.Protected && !f.Reparse && !f.Hardlink,"RESOURCE_STATE_HOLD");
    string n=f.Relative;
    Need(n!=null && Regex.IsMatch(n,@"\A(Code|Runtime|Legacy|Deployment)/[^\x00-\x1f\\:*?<>|]+\z"),"RESOURCE_PATH_HOLD");
    Need(!n.Split('/').Any(p=>p=="" || p=="." || p==".." || p.EndsWith(".") || p.EndsWith(" ")),"RESOURCE_ALIAS_HOLD");
    Need(f.ExpectedSha!=null && Regex.IsMatch(f.ExpectedSha,@"\A[a-f0-9]{64}\z") && f.ExpectedSha==f.ObservedSha,"RESOURCE_SHA_HOLD");
    Need(paths.Add(OldRoot+"\\"+n.Replace('/','\\')),"RESOURCE_DUPLICATE_HOLD");
   }
   return paths.OrderBy(x=>x,StringComparer.OrdinalIgnoreCase).ToArray();
  }
  public static RmResult Evaluate(FileEvidence[] files,IRm api){
   // manifest/경로/가짜 파일 증거 검증을 통과하기 전에는 세션도 시작하지 않습니다.
   var paths=Validate(files);var result=new RmResult();bool started=false;
   try{
    result.Code=api.Start();if(result.Code!=0)return result;started=true;
    result.Code=api.Register(paths);if(result.Code!=0)return result;result.Registered=paths.Length;
    int capacity=0;
    // 234 버퍼 성장 경쟁만 최대3회 처리합니다. 전체 진단 자동재실행과 구분합니다.
    for(int i=0;i<3;i++){
     var reply=api.GetList(capacity);result.Code=reply.Code;
     if(reply.Count>0 || reply.Needed>0){result.Busy=true;result.ReportedUsers=Math.Max(result.ReportedUsers,Math.Max(reply.Count,reply.Needed));}
     if(reply.Code==234){
      if(reply.Needed<=capacity || reply.Needed>4096){result.Status="RM_BUFFER_UNKNOWN_HOLD";return result;}
      capacity=reply.Needed;continue;
     }
     if(reply.Code!=0)return result;
     if(reply.Count<0 || reply.Needed<0 || reply.Count>capacity){result.Status="RM_COUNT_UNKNOWN_HOLD";return result;}
     result.Status=result.Busy?"RM_USERS_REPORTED_HOLD":(reply.RebootReasons!=0?"RM_REBOOT_REASON_HOLD":"RM_NO_USERS_REPORTED_HOLD");
     return result;
    }
    result.Status="RM_LIST_UNSTABLE_HOLD";
   }catch{result.Status="RM_EXCEPTION_HOLD";}
   finally{
    // 시작 성공 시 예외·등록 실패에도 세션 종료를 반드시 시도합니다. 원문 예외는 보존하지 않습니다.
    if(started){try{result.EndCode=api.End();if(result.EndCode!=0)result.Status="RM_END_FAILED_HOLD";}catch{result.Status="RM_END_EXCEPTION_HOLD";}}
   }
   return result;
  }
 }
 public sealed class V2Plan {
  public string Epoch="20261027-v2-01",Account="LIMITLESSBotTrialV2";
  public string CodeRoot=@"C:\Program Files\LimitlessRelayTrialV2_20261027";
  public string StateRoot=@"C:\ProgramData\LimitlessRelayTrialV2_20261027";
  public string Task="LIMITLESS-Trial-V2-20261027-S0Host";
  public string Pipe="LimitlessPlanningV2-0123456789abcdef0123456789abcdef";
  public string ManifestSha=new string('a',64),ApprovedManifestSha=new string('a',64);
  public string BotSid="S-1-5-21-111-222-333-1010",WorkerSid="S-1-5-21-111-222-333-1007",AdminSid="S-1-5-21-111-222-333-1001",OldBotSid="S-1-5-21-111-222-333-1008";
  public string Mode="FAKE_ONLY",Right="SeBatchLogonRight",Group="Users";
  public bool Disabled=true,TaskEnabled=false,AccountDisabledAtCreation=true;
  public int Triggers=0,RunLevel=0,RestartCount=0,RegisterFlags=50;
  public string[] CodeWriters={"SY","BA"},StateWriters={"SY","BA","WORKER"},TaskReaders={"WORKER"};
 }
 public sealed class FakeWorld {
  // 실제 OS 상태가 아닌 합성 상태입니다. 운영 자산은 변경 대상에 포함하지 않습니다.
  public readonly Dictionary<string,string> ProtectedAssets=new Dictionary<string,string>{
   {"OldCode855","HASH_OLD"},{"OldBot1008","DISABLED"},{"Install1","CONSUMED"},{"OldManifest","PINNED"},
   {"WorkerAuth","UNCHANGED"},{"CODEX_HOME","UNCHANGED"},{"BotOnBoot","PINNED"},{"ArtOnBoot","PINNED"},{"BootApproval","PINNED"},{"OperationalSQLite","UNOPENED"}};
  public bool NewNamesAbsent=true,NewPathsCanonical=true,NoReparse=true,NoHardlinks=true,ExclusiveCreate=true;
  public bool PreservationKnown=true,NewReferencesKnown=true,NewReferencesBusy=false,ApprovalFresh=true,ApprovalConsumed=false;
  public bool NamespaceSeparationAcknowledged=false;
  public string PinnedWorkerSid="S-1-5-21-111-222-333-1007",PinnedAdminSid="S-1-5-21-111-222-333-1001",PinnedOldBotSid="S-1-5-21-111-222-333-1008";
  public bool FailPublish=false,FailAccount=false,FailRegister=false,FailPostReadback=false;
  public int Attempts,FakeFiles,FakeAccounts,FakeTasks,FakeWorkerCalls,ActualCalls;
  public bool NewAccountDisabled,NewTaskDisabled;
 }
 public sealed class V2Result {public string Status="V2_UNKNOWN_HOLD";public bool OldReferenceHold=true,InstallAuthorized=false;}
 public static class V2Model {
  static bool Exact(string[] a,params string[] expected){return a!=null && a.Length==expected.Length && a.OrderBy(x=>x).SequenceEqual(expected.OrderBy(x=>x));}
  public static void Validate(V2Plan p,FakeWorld w){
   var n=new V2Plan();
   ResourceModel.Need(p.Epoch==n.Epoch && p.Account==n.Account && p.CodeRoot==n.CodeRoot && p.StateRoot==n.StateRoot && p.Task==n.Task,"V2_FIXED_NAMESPACE_HOLD");
   ResourceModel.Need(Regex.IsMatch(p.Pipe,@"\ALimitlessPlanningV2-[a-f0-9]{32}\z"),"V2_PIPE_HOLD");
   ResourceModel.Need(w.NewNamesAbsent && w.NewPathsCanonical && w.NoReparse && w.NoHardlinks && w.ExclusiveCreate,"V2_COLLISION_OR_ALIAS_HOLD");
   ResourceModel.Need(new[]{p.BotSid,p.WorkerSid,p.AdminSid,p.OldBotSid}.All(s=>s!=null && Regex.IsMatch(s,@"\AS-1-5-21-\d+-\d+-\d+-\d+\z")) && new[]{p.BotSid,p.WorkerSid,p.AdminSid,p.OldBotSid}.Distinct().Count()==4,"V2_DISTINCT_SID_HOLD");
   ResourceModel.Need(p.WorkerSid==w.PinnedWorkerSid && p.AdminSid==w.PinnedAdminSid && p.OldBotSid==w.PinnedOldBotSid,"V2_PINNED_IDENTITY_HOLD");
   ResourceModel.Need(p.ManifestSha!=null && Regex.IsMatch(p.ManifestSha,@"\A[a-f0-9]{64}\z") && p.ManifestSha==p.ApprovedManifestSha && w.ApprovalFresh && !w.ApprovalConsumed,"V2_APPROVAL_HOLD");
   ResourceModel.Need(p.Mode=="FAKE_ONLY" && p.Disabled && p.AccountDisabledAtCreation && !p.TaskEnabled && p.Triggers==0 && p.RunLevel==0 && p.RestartCount==0 && p.RegisterFlags==50 && p.Group=="Users" && p.Right=="SeBatchLogonRight","V2_POLICY_HOLD");
   ResourceModel.Need(Exact(p.CodeWriters,"SY","BA") && Exact(p.StateWriters,"SY","BA","WORKER") && Exact(p.TaskReaders,"WORKER"),"V2_ACL_EXPANSION_HOLD");
   ResourceModel.Need(w.PreservationKnown && w.NewReferencesKnown && !w.NewReferencesBusy,"V2_EVIDENCE_HOLD");
   // 기존 참조 HOLD를 해결한 척하지 않습니다. 별도 이름공간 병행을 승인받기 전에도 HOLD입니다.
   ResourceModel.Need(w.NamespaceSeparationAcknowledged,"V2_PARALLEL_SCOPE_APPROVAL_REQUIRED_HOLD");
  }
  public static V2Result Simulate(V2Plan p,FileEvidence[] files,FakeWorld w){
   var result=new V2Result();var before=w.ProtectedAssets.OrderBy(x=>x.Key).ToArray();
   try{
    Validate(p,w);ResourceModel.Validate(files);ResourceModel.Need(w.Attempts==0,"V2_NO_RETRY_HOLD");
    w.Attempts++;w.ApprovalConsumed=true;
    if(w.FailPublish)throw new InvalidOperationException();w.FakeFiles=855;
    if(w.FailAccount)throw new InvalidOperationException();w.FakeAccounts=1;w.NewAccountDisabled=true;
    if(w.FailRegister)throw new InvalidOperationException();w.FakeTasks=1;w.NewTaskDisabled=true;
    if(w.FailPostReadback)throw new InvalidOperationException();
    result.Status="V2_OFFLINE_SIMULATED_NOT_INSTALLED";
   }catch{result.Status="V2_UNKNOWN_HOLD";}
   finally{if(!before.SequenceEqual(w.ProtectedAssets.OrderBy(x=>x.Key)))result.Status="V2_PRESERVATION_HOLD";}
   // 실행 경로는 존재하지 않으므로 성공 시에도 설치/IPC 권한을 발급하지 않습니다.
   return result;
  }
 }
}

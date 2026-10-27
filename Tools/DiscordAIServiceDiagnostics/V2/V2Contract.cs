// V2의 모든 Windows 진입점이 공유하는 고정 계약입니다. Validate 계열은 OS를 변경하지 않습니다.
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Security.Principal;
using System.Security.AccessControl;
public static class LimitlessV2Contract {
 public const string CodeRoot=@"C:\Program Files\LimitlessRelayTrialV2_20261027";
 public const string StateRoot=@"C:\ProgramData\LimitlessRelayTrialV2_20261027";
 public const string ApprovalRoot=@"C:\ProgramData\LimitlessRelayV2Approvals";
 public const string Account="LIMITLESSBotTrialV2",TaskName="LIMITLESS-Trial-V2-20261027-S0Host";
 public const string BootSha="3e032ebcce3d91bd762a8dec81048d04607ddde761afd62489c0fa2b8e49140f";
 public const string OldBotSid="S-1-5-21-2858504392-1958168942-1662906075-1008";
 public static void Need(bool good,string code){if(!good)throw new InvalidOperationException(code);}
 public static bool Sha(string s){return s!=null && Regex.IsMatch(s,@"\A[a-f0-9]{64}\z");}
 public static bool Sid(string s){return s!=null && Regex.IsMatch(s,@"\AS-1-5-21-\d+-\d+-\d+-\d+\z");}
 public static void Distinct(string bot,string worker,string admin){
  Need(Sid(bot)&&Sid(worker)&&Sid(admin)&&new[]{bot,worker,admin,OldBotSid}.Distinct().Count()==4,"V2_DISTINCT_SID_HOLD");
 }
 static void Keys(JsonElement e,params string[] names){
  Need(e.ValueKind==JsonValueKind.Object,"V2_OBJECT_HOLD");
  var keys=e.EnumerateObject().Select(p=>p.Name).ToArray();
  Need(keys.Length==names.Length && keys.Distinct().Count()==keys.Length && keys.OrderBy(x=>x).SequenceEqual(names.OrderBy(x=>x)),"V2_KEYS_HOLD");
 }
 public static bool Inside(string path,string root){
  string full=Path.GetFullPath(path);return full.Equals(root,StringComparison.OrdinalIgnoreCase)||full.StartsWith(root+"\\",StringComparison.OrdinalIgnoreCase);
 }
 public static void Pipe(string name,bool channel){Need(name!=null&&Regex.IsMatch(name,channel?@"\ALimitlessPlanningV2-[a-f0-9]{32}-(arm|work|control)\z":@"\ALimitlessPlanningV2-[a-f0-9]{32}\z"),"V2_PIPE_HOLD");}
 public static void Relative(string n){
  Need(n!=null&&Regex.IsMatch(n,@"\A(Code|Runtime|Deployment)/[^\x00-\x1f\\:*?<>|]+\z"),"V2_RELATIVE_HOLD");
  Need(!n.Split('/').Any(p=>p==""||p=="."||p==".."||p.EndsWith(".")||p.EndsWith(" ")||Regex.IsMatch(p,@"\A(CON|PRN|AUX|NUL|COM[1-9]|LPT[1-9])(\.|\z)",RegexOptions.IgnoreCase)),"V2_PATH_ALIAS_HOLD");
 }
 public static void Manifest(string raw){
  using(var d=JsonDocument.Parse(raw)){var e=d.RootElement;
   Keys(e,"schema","kind","package_id","epoch","approval_id","approved","auto_start","execution_mode","max_seconds","bot_account","code_root","state_root","task_name","pipe_name","boot_approval_sha256","files_sha256");
   string id=e.GetProperty("package_id").GetString();
   Need(id!=null&&Regex.IsMatch(id,@"\A[a-f0-9]{32}\z"),"V2_PACKAGE_ID_HOLD");
   Need(e.GetProperty("schema").GetInt32()==2&&e.GetProperty("kind").GetString()=="LIMITLESS_SESSION0_V2_CANDIDATE"&&e.GetProperty("epoch").GetString()=="20261027-v2-"+id&&e.GetProperty("approval_id").GetString()=="v2-install-"+id,"V2_SCHEMA_EPOCH_HOLD");
   Need(e.GetProperty("approved").ValueKind==JsonValueKind.False&&e.GetProperty("auto_start").ValueKind==JsonValueKind.False&&e.GetProperty("execution_mode").GetString()=="FAKE_ONLY"&&e.GetProperty("max_seconds").GetInt32()==90,"V2_CANDIDATE_POLICY_HOLD");
   Need(e.GetProperty("code_root").GetString()==CodeRoot&&e.GetProperty("state_root").GetString()==StateRoot&&e.GetProperty("bot_account").GetString()==Account&&e.GetProperty("task_name").GetString()==TaskName&&e.GetProperty("boot_approval_sha256").GetString()==BootSha,"V2_NAMESPACE_HOLD");
   Pipe(e.GetProperty("pipe_name").GetString(),false);
   Need(e.GetProperty("pipe_name").GetString()=="LimitlessPlanningV2-"+id,"V2_PIPE_PACKAGE_HOLD");
   var files=e.GetProperty("files_sha256");Need(files.ValueKind==JsonValueKind.Object,"V2_FILES_HOLD");var seen=new HashSet<string>(StringComparer.OrdinalIgnoreCase);
   foreach(var p in files.EnumerateObject()){Relative(p.Name);Need(seen.Add(p.Name)&&Sha(p.Value.GetString()),"V2_FILE_SHA_OR_DUPLICATE_HOLD");}
   Need(seen.Count>8&&seen.Count<=2048,"V2_FILE_COUNT_HOLD");
  }
 }
 public static void Approval(string raw,string manifestSha,string installerSha,string zipSha,string approvalId,string epoch,string workerSid,string adminSid,long now){
  using(var d=JsonDocument.Parse(raw)){var e=d.RootElement;
   Keys(e,"schema","kind","approved","approval_id","epoch","manifest_sha256","installer_sha256","zip_sha256","worker_sid","admin_sid","expires_unix","preserve_v1_hold");
   Need(e.GetProperty("schema").GetInt32()==2&&e.GetProperty("kind").GetString()=="INSTALL_V2"&&e.GetProperty("approved").ValueKind==JsonValueKind.True&&e.GetProperty("preserve_v1_hold").ValueKind==JsonValueKind.True,"V2_INSTALL_APPROVAL_HOLD");
   Need(Sha(manifestSha)&&Sha(installerSha)&&Sha(zipSha)&&e.GetProperty("manifest_sha256").GetString()==manifestSha&&e.GetProperty("installer_sha256").GetString()==installerSha&&e.GetProperty("zip_sha256").GetString()==zipSha&&e.GetProperty("approval_id").GetString()==approvalId&&e.GetProperty("epoch").GetString()==epoch,"V2_APPROVAL_PIN_HOLD");
   Need(Sid(workerSid)&&Sid(adminSid)&&workerSid!=adminSid&&workerSid!=OldBotSid&&adminSid!=OldBotSid&&e.GetProperty("worker_sid").GetString()==workerSid&&e.GetProperty("admin_sid").GetString()==adminSid,"V2_APPROVER_SID_HOLD");
   long until=e.GetProperty("expires_unix").GetInt64();Need(until>now&&until<=now+3600,"V2_APPROVAL_EXPIRY_HOLD");
  }
 }
 public static void Binding(string raw,string manifestRaw,string manifestSha){
  Manifest(manifestRaw);using(var md=JsonDocument.Parse(manifestRaw))using(var d=JsonDocument.Parse(raw)){
   var m=md.RootElement;var e=d.RootElement;
   Keys(e,"schema","kind","package_id","epoch","approval_id","manifest_sha256","bot_sid","worker_sid","admin_sid","execution_mode","auto_start");
   Need(e.GetProperty("schema").GetInt32()==2&&e.GetProperty("kind").GetString()=="LIMITLESS_V2_INSTALLED_BINDING"&&e.GetProperty("manifest_sha256").GetString()==manifestSha,"V2_BINDING_HOLD");
   foreach(var k in new[]{"package_id","epoch","approval_id"})Need(e.GetProperty(k).GetString()==m.GetProperty(k).GetString(),"V2_BINDING_NAMESPACE_HOLD");
   Need(e.GetProperty("execution_mode").GetString()=="FAKE_ONLY"&&e.GetProperty("auto_start").ValueKind==JsonValueKind.False,"V2_BINDING_MODE_HOLD");
   Distinct(e.GetProperty("bot_sid").GetString(),e.GetProperty("worker_sid").GetString(),e.GetProperty("admin_sid").GetString());
  }
 }
 public static string FsSddl(string kind,bool directory,string worker,string bot){
  Need(kind=="private"||kind=="code"||kind=="state","V2_ACL_KIND_HOLD");
  string flags=directory?"OICI":"",s="O:BAG:SYD:P(A;"+flags+";FA;;;SY)(A;"+flags+";FA;;;BA)";
  if(kind!="private"){Need(Sid(worker),"V2_WORKER_SID_HOLD");s+="(A;"+flags+";"+(kind=="code"?"0x1200a9":"0x1301bf")+";;;"+worker+")";}
  if(kind=="code"){Need(Sid(bot)&&bot!=worker&&bot!=OldBotSid,"V2_BOT_SID_HOLD");s+="(A;"+flags+";0x1200a9;;;"+bot+")";}
  return s;
 }
 public static void IpcApproval(string raw,string epoch,string ms,string bs,long now){
  using(var d=JsonDocument.Parse(raw)){var e=d.RootElement;Keys(e,"schema","kind","approved","epoch","manifest_sha256","binding_sha256","expires_unix");
   Need(e.GetProperty("schema").GetInt32()==2&&e.GetProperty("kind").GetString()=="IPC_V2_ONCE"&&e.GetProperty("approved").ValueKind==JsonValueKind.True&&e.GetProperty("epoch").GetString()==epoch&&Sha(ms)&&Sha(bs)&&e.GetProperty("manifest_sha256").GetString()==ms&&e.GetProperty("binding_sha256").GetString()==bs,"V2_IPC_APPROVAL_HOLD");
   long until=e.GetProperty("expires_unix").GetInt64();Need(until>now&&until<=now+90,"V2_IPC_APPROVAL_EXPIRY_HOLD");
  }
 }
 public static string TaskSddl(string worker){Need(Sid(worker)&&worker!=OldBotSid,"V2_TASK_WORKER_HOLD");return "O:BAG:SYD:P(A;;FA;;;SY)(A;;FA;;;BA)(A;;GR;;;"+worker+")";}
 public static void ExactSddl(string actual,string expected){
  var a=new RawSecurityDescriptor(actual);var b=new RawSecurityDescriptor(expected);
  Need(a.Owner.Value==b.Owner.Value&&a.Group.Value==b.Group.Value&&(a.ControlFlags&ControlFlags.DiscretionaryAclProtected)!=0&&a.DiscretionaryAcl!=null&&a.DiscretionaryAcl.Count==b.DiscretionaryAcl.Count,"V2_SD_HEADER_HOLD");
  var ar=new List<string>();var br=new List<string>();
  foreach(GenericAce g in a.DiscretionaryAcl){var x=g as CommonAce;Need(x!=null&&!x.IsCallback&&x.AceQualifier==AceQualifier.AccessAllowed,"V2_ACE_KIND_HOLD");ar.Add(x.SecurityIdentifier.Value+":"+x.AccessMask+":"+(int)x.AceFlags);}
  foreach(CommonAce x in b.DiscretionaryAcl)br.Add(x.SecurityIdentifier.Value+":"+x.AccessMask+":"+(int)x.AceFlags);
  Need(ar.OrderBy(x=>x).SequenceEqual(br.OrderBy(x=>x)),"V2_ACE_RIGHTS_HOLD");
 }
 // 오류 자유문/stack/credential을 수집하지 않고 실제 형식과 숫자만 보존합니다.
 public static object[] ErrorCodes(Exception ex){
  var r=new List<object>();
  for(int i=0;ex!=null&&i<8;i++,ex=ex.InnerException){
   var n=new Dictionary<string,object>{{"type",ex.GetType().FullName},{"hresult",ex.HResult}};
   if(ex is System.ComponentModel.Win32Exception w)n["win32"]=w.NativeErrorCode;
   var status=ex.GetType().GetProperty("NtStatus");if(status!=null&&status.GetValue(ex) is uint value)n["ntstatus"]=value;
   var operation=ex.GetType().GetProperty("Operation");var op=operation?.GetValue(ex) as string;if(op=="OPEN"||op=="ADD_RIGHT"||op=="CLOSE")n["operation"]=op;
   // LSA의 원래 실패와 close 실패 번호를 둘 다 보존하되 오류 자유문은 수집하지 않습니다.
   foreach(var k in new[]{"cleanup_ntstatus","cleanup_win32"})if(ex.Data.Contains(k)&&(ex.Data[k] is uint||ex.Data[k] is int))n[k]=ex.Data[k];
   r.Add(n);
  }
  return r.ToArray();
 }
}

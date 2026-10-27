// 배포 schema3와 재사용 wire schema2를 분리합니다. SID는 설치 후 OS 결과로만 바인딩합니다.
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Security.Principal;
using System.Security.AccessControl;
public static class DedicatedContract {
 public const string CodeRoot=@"C:\Program Files\LimitlessRelayDedicatedV2_20261027";
 public const string StateRoot=@"C:\ProgramData\LimitlessRelayDedicatedV2_20261027";
 public const string AuthRoot=StateRoot+@"\Auth\CodexHome";
 public const string ApprovalRoot=@"C:\ProgramData\LimitlessRelayDedicatedV2Approvals";
 public const string StagingRoot=@"C:\Program Files\LimitlessRelayDedicatedV2Staging20261027";
 public const string Bot="LIMITLESSBotV2D",Worker="LIMITLESSWorkV2D",Task="LIMITLESS-DedicatedV2-20261027-Host";
 public const string OldWorker="S-1-5-21-2858504392-1958168942-1662906075-1007",OldBot="S-1-5-21-2858504392-1958168942-1662906075-1008";
 public static readonly string[] Rights={"SeBatchLogonRight","SeDenyInteractiveLogonRight","SeDenyRemoteInteractiveLogonRight","SeDenyNetworkLogonRight","SeDenyServiceLogonRight"};
 public static void Need(bool ok,string tag){if(!ok)throw new InvalidOperationException(tag);}
 public static bool Sha(string s){return s!=null&&Regex.IsMatch(s,@"\A[a-f0-9]{64}\z");}
 public static bool Sid(string s){return s!=null&&Regex.IsMatch(s,@"\AS-1-5-21-[0-9]+-[0-9]+-[0-9]+-[0-9]+\z");}
 public static void Distinct(string bot,string worker,string admin){Need(new[]{bot,worker,admin}.All(Sid)&&new[]{bot,worker,admin,OldBot,OldWorker}.Distinct().Count()==5,"D3_DISTINCT_NEW_SIDS");}
 public static void Keys(JsonElement e,params string[] keys){Need(e.ValueKind==JsonValueKind.Object,"D3_OBJECT");var actual=e.EnumerateObject().Select(p=>p.Name).ToArray();Need(actual.Length==keys.Length&&actual.Distinct().Count()==actual.Length&&actual.OrderBy(x=>x).SequenceEqual(keys.OrderBy(x=>x)),"D3_EXACT_KEYS");}
 public static void Relative(string name){
  Need(name!=null&&Regex.IsMatch(name,@"\A(Code|Runtime|Deployment)/[^\x00-\x1f\\:*?<>|]+\z"),"D3_RELATIVE");
  foreach(string p in name.Split('/'))Need(p!=""&&p!="."&&p!=".."&&!p.EndsWith(".")&&!p.EndsWith(" ")&&!Regex.IsMatch(p,@"\A(CON|PRN|AUX|NUL|COM[1-9]|LPT[1-9])(\.|\z)",RegexOptions.IgnoreCase),"D3_PATH_ALIAS");
 }
 public static void Pipe(string name,bool channel){Need(name!=null&&Regex.IsMatch(name,channel?@"\ALimitlessDedicatedV2-[a-f0-9]{32}-(arm|work|control)\z":@"\ALimitlessDedicatedV2-[a-f0-9]{32}\z"),"D3_PIPE");}
 public static bool StageBundle(string path){string prefix=StagingRoot+"\\";return path!=null&&path.StartsWith(prefix,StringComparison.Ordinal)&&Regex.IsMatch(path.Substring(prefix.Length),@"\A[a-f0-9]{32}\\Bundle\z");}
 public static void Manifest(string raw){using(var d=JsonDocument.Parse(raw)){
  var m=d.RootElement;Keys(m,"schema","wire_schema","kind","package_id","epoch","approval_id","approved","auto_start","execution_mode","max_seconds","max_parallel_jobs","bot_account","worker_account","code_root","state_root","codex_home","approval_root","staging_root","task_name","pipe_name","auth_policy","real_ai_enabled","worker_install_state","files_sha256");
  string id=m.GetProperty("package_id").GetString();Need(id!=null&&Regex.IsMatch(id,@"\A[a-f0-9]{32}\z"),"D3_GUID");
  Need(m.GetProperty("schema").GetInt32()==3&&m.GetProperty("wire_schema").GetInt32()==2&&m.GetProperty("kind").GetString()=="LIMITLESS_DEDICATED_V2_CANDIDATE","D3_SCHEMA");
  Need(m.GetProperty("epoch").GetString()=="20261027-dedicated-"+id&&m.GetProperty("approval_id").GetString()=="dedicated-install-"+id&&m.GetProperty("pipe_name").GetString()=="LimitlessDedicatedV2-"+id,"D3_NAMESPACE");
  Need(m.GetProperty("approved").ValueKind==JsonValueKind.False&&m.GetProperty("auto_start").ValueKind==JsonValueKind.False&&m.GetProperty("real_ai_enabled").ValueKind==JsonValueKind.False&&m.GetProperty("execution_mode").GetString()=="FAKE_ONLY"&&m.GetProperty("auth_policy").GetString()=="EMPTY_NO_IMPORT_NO_LOGIN"&&m.GetProperty("worker_install_state").GetString()=="ENABLED_BATCH_ONLY"&&m.GetProperty("max_seconds").GetInt32()==90&&m.GetProperty("max_parallel_jobs").GetInt32()==1,"D3_FAKE_POLICY");
  foreach(var pair in new Dictionary<string,string>{{"bot_account",Bot},{"worker_account",Worker},{"code_root",CodeRoot},{"state_root",StateRoot},{"codex_home",AuthRoot},{"approval_root",ApprovalRoot},{"staging_root",StagingRoot},{"task_name",Task}})Need(m.GetProperty(pair.Key).GetString()==pair.Value,"D3_FIXED_RESOURCE");
  var files=m.GetProperty("files_sha256");Need(files.ValueKind==JsonValueKind.Object,"D3_FILES");var seen=new HashSet<string>(StringComparer.OrdinalIgnoreCase);foreach(var p in files.EnumerateObject()){Relative(p.Name);Need(seen.Add(p.Name)&&Sha(p.Value.GetString()),"D3_PAYLOAD");}Need(seen.Count>8&&seen.Count<=2048,"D3_COUNT");
 }}
 public static void Binding(string raw,string manifest,string ms){Manifest(manifest);using(var md=JsonDocument.Parse(manifest))using(var d=JsonDocument.Parse(raw)){
  var m=md.RootElement;var b=d.RootElement;Keys(b,"schema","kind","package_id","epoch","approval_id","manifest_sha256","bot_sid","worker_sid","admin_sid","execution_mode","auto_start");
  Need(b.GetProperty("schema").GetInt32()==3&&b.GetProperty("kind").GetString()=="DEDICATED_V2_OS_BINDING"&&b.GetProperty("manifest_sha256").GetString()==ms&&Sha(ms),"D3_BINDING");foreach(var k in new[]{"package_id","epoch","approval_id"})Need(b.GetProperty(k).GetString()==m.GetProperty(k).GetString(),"D3_BINDING_NAMESPACE");
  Need(b.GetProperty("execution_mode").GetString()=="FAKE_ONLY"&&b.GetProperty("auto_start").ValueKind==JsonValueKind.False,"D3_BINDING_MODE");Distinct(b.GetProperty("bot_sid").GetString(),b.GetProperty("worker_sid").GetString(),b.GetProperty("admin_sid").GetString());
 }}
 public static void InstallApproval(string raw,string ms,string ins,string zip,string id,string epoch,string admin,long now){using(var d=JsonDocument.Parse(raw)){
  var a=d.RootElement;Keys(a,"schema","kind","approved","approval_id","epoch","manifest_sha256","installer_sha256","zip_sha256","admin_sid","expires_unix","preserve_existing","create_bot","create_worker","worker_install_state");
  Need(a.GetProperty("schema").GetInt32()==3&&a.GetProperty("kind").GetString()=="DEDICATED_INSTALL_ONCE"&&a.GetProperty("approved").ValueKind==JsonValueKind.True&&a.GetProperty("preserve_existing").ValueKind==JsonValueKind.True,"D3_INSTALL_APPROVAL");
  Need(a.GetProperty("approval_id").GetString()==id&&a.GetProperty("epoch").GetString()==epoch&&a.GetProperty("manifest_sha256").GetString()==ms&&a.GetProperty("installer_sha256").GetString()==ins&&a.GetProperty("zip_sha256").GetString()==zip&&Sha(ms)&&Sha(ins)&&Sha(zip)&&a.GetProperty("admin_sid").GetString()==admin&&Sid(admin)&&admin!=OldWorker&&admin!=OldBot&&a.GetProperty("create_bot").GetString()==Bot&&a.GetProperty("create_worker").GetString()==Worker&&a.GetProperty("worker_install_state").GetString()=="ENABLED_BATCH_ONLY","D3_APPROVED_BYTES_AND_ROLES");long until=a.GetProperty("expires_unix").GetInt64();Need(until>now&&until<=now+3600,"D3_EXPIRY");
 }}
 public static void IpcApproval(string raw,string epoch,string ms,string bs,long now){using(var d=JsonDocument.Parse(raw)){
  var a=d.RootElement;Keys(a,"schema","kind","approved","epoch","manifest_sha256","binding_sha256","expires_unix","credential_scope_unshared","noncooperative_launcher_control_verified");
  Need(a.GetProperty("schema").GetInt32()==3&&a.GetProperty("kind").GetString()=="DEDICATED_FAKE_IPC_ONCE"&&a.GetProperty("approved").ValueKind==JsonValueKind.True&&a.GetProperty("credential_scope_unshared").ValueKind==JsonValueKind.True&&a.GetProperty("noncooperative_launcher_control_verified").ValueKind==JsonValueKind.True,"D3_IPC_SCOPE_HOLD");Need(a.GetProperty("epoch").GetString()==epoch&&a.GetProperty("manifest_sha256").GetString()==ms&&a.GetProperty("binding_sha256").GetString()==bs,"D3_IPC_PINS");long until=a.GetProperty("expires_unix").GetInt64();Need(until>now&&until<=now+90,"D3_IPC_EXPIRY");
 }}
 public static string FsSddl(string kind,bool directory,string worker,string bot){string f=directory?"OICI":"",s="O:BAG:SYD:P(A;"+f+";FA;;;SY)(A;"+f+";FA;;;BA)";Need(new[]{"private","code","container","state","auth"}.Contains(kind),"D3_SD_KIND");if(kind!="private"){Need(Sid(worker)&&worker!=OldWorker&&worker!=OldBot,"D3_NEW_WORKER");s+="(A;"+f+";"+(kind=="code"||kind=="container"?"0x1200a9":"0x1301bf")+";;;"+worker+")";}if(kind=="code"){Need(Sid(bot)&&bot!=worker&&bot!=OldBot&&bot!=OldWorker,"D3_NEW_BOT");s+="(A;"+f+";0x1200a9;;;"+bot+")";}return s;}
 public static void ExactSddl(string actual,string expected){var a=new RawSecurityDescriptor(actual);var e=new RawSecurityDescriptor(expected);Need(a.Owner.Value==e.Owner.Value&&a.Group.Value==e.Group.Value&&(a.ControlFlags&ControlFlags.DiscretionaryAclProtected)!=0&&a.DiscretionaryAcl!=null&&a.DiscretionaryAcl.Count==e.DiscretionaryAcl.Count,"D3_SD_HEADER");var aa=new List<string>();var ee=new List<string>();foreach(GenericAce x in a.DiscretionaryAcl){var c=x as CommonAce;Need(c!=null&&!c.IsCallback&&c.AceQualifier==AceQualifier.AccessAllowed,"D3_SD_ACE");aa.Add(c.SecurityIdentifier.Value+":"+c.AccessMask+":"+(int)c.AceFlags);}foreach(CommonAce c in e.DiscretionaryAcl)ee.Add(c.SecurityIdentifier.Value+":"+c.AccessMask+":"+(int)c.AceFlags);Need(aa.OrderBy(x=>x).SequenceEqual(ee.OrderBy(x=>x)),"D3_SD_RIGHTS");}
 public static string TaskSddl(string worker){Need(Sid(worker)&&worker!=OldWorker&&worker!=OldBot,"D3_TASK_SID");return "O:BAG:SYD:P(A;;FA;;;SY)(A;;FA;;;BA)(A;;GR;;;"+worker+")";}
 public static object[] ErrorCodes(Exception error){var all=new List<object>();for(int n=0;error!=null&&n<8;n++,error=error.InnerException){var row=new Dictionary<string,object>{{"type",error.GetType().FullName},{"hresult",error.HResult}};if(error is System.ComponentModel.Win32Exception w)row["win32"]=w.NativeErrorCode;foreach(string p in new[]{"NtStatus","Operation"}){var prop=error.GetType().GetProperty(p);if(prop!=null)row[p.ToLowerInvariant()]=prop.GetValue(error);}foreach(string key in new[]{"cleanup_ntstatus","cleanup_win32"})if(error.Data.Contains(key))row[key]=error.Data[key];all.Add(row);}return all.ToArray();}
}
// 이전 native 계층이 의존하는 인터페이스만 제공합니다. 전역 문자열 치환으로 root/schema를 만들지 않습니다.
public static class LimitlessV2Contract {
 public const string CodeRoot=DedicatedContract.CodeRoot,StateRoot=DedicatedContract.StateRoot,ApprovalRoot=DedicatedContract.ApprovalRoot,OldBotSid=DedicatedContract.OldBot;
 public static bool Sid(string s)=>DedicatedContract.Sid(s);public static bool Sha(string s)=>DedicatedContract.Sha(s);
 public static void Need(bool b,string t)=>DedicatedContract.Need(b,t);public static void Manifest(string s)=>DedicatedContract.Manifest(s);public static void Relative(string s)=>DedicatedContract.Relative(s);public static void Pipe(string s,bool c)=>DedicatedContract.Pipe(s,c);public static void Distinct(string b,string w,string a)=>DedicatedContract.Distinct(b,w,a);
 public static bool Inside(string p,string root){string full=Path.GetFullPath(p);return full.Equals(root,StringComparison.OrdinalIgnoreCase)||full.StartsWith(root+"\\",StringComparison.OrdinalIgnoreCase);}
 public static string FsSddl(string k,bool d,string w,string b)=>DedicatedContract.FsSddl(k,d,w,b);public static void ExactSddl(string a,string b)=>DedicatedContract.ExactSddl(a,b);public static object[] ErrorCodes(Exception e)=>DedicatedContract.ErrorCodes(e);
}

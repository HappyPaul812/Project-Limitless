// r2 설치물에 적용하지 않는 순수 오프라인 Task 보안 검증 후보입니다.
using System;
using System.Linq;
using System.Text.Json;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Principal;

namespace Limitless.TaskSecurityCandidate {
 public static class DedicatedTaskReadContract {
  public const string TaskPath=@"\LIMITLESS-DedicatedV2-20261027-Host";
  public const string Worker="S-1-5-21-2858504392-1958168942-1662906075-1010";
  public const string SnapshotSha="4de232b3d5af7aa6662ce4e4ec6a4d28ddc529b5c0d50656a9114f71e679140e";
  public const uint GenericRead=0x80000000;
  // Task Scheduler가 문서화한 파일 읽기 권리: DATA|EA|ATTRIBUTES|READ_CONTROL|SYNCHRONIZE.
  // 레지스트리/프로세스/디렉터리의 Generic Mapping으로 재사용하면 안 됩니다.
  public const uint TaskFileRead=0x00000001|0x00000008|0x00000080|0x00020000|0x00100000;
  public sealed class Evidence {
   internal Evidence(){}
   public string ObjectKind {get {return "SCHEDULED_TASK_FILE_RIGHTS";}}
   public string SourceSha {get {return SnapshotSha;}}
   public bool LiveVerified {get {return false;}}
  }
  public sealed class Verdict {
   public string Status {get;internal set;}
   public uint RawWorkerMask {get;internal set;}
   public uint NormalizedWorkerMask {get;internal set;}
   public bool LiveVerified {get {return false;}}
   public bool RuntimeAdmitted {get {return false;}}
  }
  static void Need(bool ok,string tag){if(!ok)throw new InvalidOperationException(tag);}
  static JsonElement Query(JsonElement root,string stage){
   var matches=root.GetProperty("queries").EnumerateArray().Where(x=>x.GetProperty("stage").GetString()==stage).ToArray();
   Need(matches.Length==1&&matches[0].GetProperty("status").GetString()=="READ_OK","TASK_MAPPING_QUERY_UNKNOWN");return matches[0].GetProperty("value");
  }
  // 임의의 bool 대신 SHA로 고정된 과거 JSON에서 Task 종류와 SID의 연결을 확인합니다.
  public static Evidence FromPinnedSnapshot(byte[] bytes){
   Need(bytes!=null&&bytes.Length<=4*1024*1024,"TASK_MAPPING_EVIDENCE_BOUND");
   Need(Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant()==SnapshotSha,"TASK_MAPPING_EVIDENCE_SHA");
   using(var d=JsonDocument.Parse(bytes)){
    var r=d.RootElement;Need(r.GetProperty("admin").GetBoolean()&&r.GetProperty("task_presence").GetString()=="PRESENT","TASK_MAPPING_CONTEXT");
    Need(Query(r,"TASK_GET_EXACT").GetProperty("path").GetString()==TaskPath,"TASK_MAPPING_OBJECT");
    var user=Query(r,"ACCOUNT_NETAPI_LIMITLESSWorkV2D");Need(user.GetProperty("expected_sid_match").GetBoolean()&&user.GetProperty("facts").GetProperty("sid").GetString()==Worker,"TASK_MAPPING_SID");
    var sd=Query(r,"TASK_SECURITY_DESCRIPTOR").GetProperty("sddl").GetString();
    var descriptor=new RawSecurityDescriptor(sd);Need(descriptor.DiscretionaryAcl!=null,"TASK_MAPPING_DACL");
    var ace=descriptor.DiscretionaryAcl.OfType<CommonAce>().Where(x=>x.SecurityIdentifier.Value==Worker).ToArray();
    Need(ace.Length==1&&unchecked((uint)ace[0].AccessMask)==TaskFileRead,"TASK_MAPPING_OBSERVATION");
   }
   return new Evidence();
  }
  // 이 Task의 Generic Read만 구체적 읽기 권리로 펼칩니다. 다른 비트는 지우지 않아 추가 권한을 거부합니다.
  static uint MapWorkerRead(uint raw,Evidence evidence){
   Need(evidence!=null,"TASK_MAPPING_EVIDENCE_REQUIRED");
   uint mapped=(raw&GenericRead)!=0?(raw&~GenericRead)|TaskFileRead:raw;
   Need(mapped==TaskFileRead,"TASK_READ_RIGHTS_HOLD");return mapped;
  }
  public static Verdict VerifyOffline(string actual,string objectKind,string taskPath,string worker,Evidence evidence){
   Need(evidence!=null,"TASK_MAPPING_EVIDENCE_REQUIRED");
   Need(objectKind==evidence.ObjectKind&&taskPath==TaskPath&&worker==Worker,"TASK_OBJECT_SCOPE_HOLD");
   Need(actual!=null&&actual.Length<=8192,"TASK_SD_BOUND");var sd=new RawSecurityDescriptor(actual);
   Need(sd.Owner!=null&&sd.Owner.Value=="S-1-5-32-544"&&sd.Group!=null&&sd.Group.Value=="S-1-5-18","TASK_SD_OWNER_GROUP_HOLD");
   // 관측된 PAI와 입력 P의 차이만 허용합니다. ACE의 상속 플래그나 순서 변경은 거부합니다.
   var required=ControlFlags.SelfRelative|ControlFlags.DiscretionaryAclPresent|ControlFlags.DiscretionaryAclProtected;
   var allowed=required|ControlFlags.DiscretionaryAclAutoInherited;
   Need((sd.ControlFlags&required)==required&&(sd.ControlFlags&~allowed)==0&&sd.SystemAcl==null,"TASK_SD_CONTROL_HOLD");
   Need(sd.DiscretionaryAcl!=null&&sd.DiscretionaryAcl.Count==3&&sd.DiscretionaryAcl.Revision==2,"TASK_SD_ACE_COUNT_HOLD");
   string[] sids={"S-1-5-18","S-1-5-32-544",Worker};uint rawWorker=0;
   for(int n=0;n<3;n++){
    var a=sd.DiscretionaryAcl[n] as CommonAce;
    Need(a!=null&&!a.IsCallback&&a.AceQualifier==AceQualifier.AccessAllowed&&a.AceFlags==AceFlags.None,"TASK_SD_ACE_TYPE_FLAGS_HOLD");
    Need(a.SecurityIdentifier.Value==sids[n],"TASK_SD_SID_ORDER_HOLD");uint mask=unchecked((uint)a.AccessMask);
    if(n<2)Need(mask==0x001f01ff,"TASK_SD_ADMIN_RIGHTS_HOLD");else rawWorker=mask;
   }
   uint normalized=MapWorkerRead(rawWorker,evidence);
   return new Verdict{Status="OFFLINE_CANDIDATE_PASS_NOT_WINDOWS_REVERIFIED",RawWorkerMask=rawWorker,NormalizedWorkerMask=normalized};
  }
 }
}

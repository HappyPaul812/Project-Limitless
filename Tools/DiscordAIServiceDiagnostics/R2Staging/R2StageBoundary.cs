// 고정 r2 package 하위만 생성합니다. 공유 부모/r1/승인 경로 쓰기는 이 경계가 거부합니다.
public sealed class R2StageBoundary : IV2StageFs {
 public const string Package="3effaac30bf44760986fee9e6019b283";
 public const string Target=DedicatedContract.StagingRoot+@"\"+Package;
 public const string R1=DedicatedContract.StagingRoot+@"\a5af3087b78b45d1baae46067d5b7ac9";
 readonly IV2StageFs inner;
 public int FileCreationAttempts{get;private set;}public int DirectoryCreationAttempts{get;private set;}
 public R2StageBoundary(IV2StageFs fs){inner=fs;}
 public static void WriteTarget(string p){
  bool ok=p!=null&&System.IO.Path.GetFullPath(p)==p&&!p.Substring(2).Contains(":")&&(p==Target||p.StartsWith(Target+@"\",System.StringComparison.Ordinal));
  DedicatedContract.Need(ok,"R2_STAGE_WRITE_SCOPE");
 }
 public bool Exists(string p)=>inner.Exists(p);
 public V2StageHandle Open(string p)=>inner.Open(p);
 public string[] Children(string p)=>inner.Children(p);
 public void Directory(string p,string sd){WriteTarget(p);DirectoryCreationAttempts++;inner.Directory(p,sd);}
 public void File(string p,byte[] b,string sd){WriteTarget(p);FileCreationAttempts++;inner.File(p,b,sd);}
 public static string Stamp(V2StageInfo i){return string.Join("|",i.Path,i.Id,i.Volume,i.Sddl,i.Directory,i.Reparse,i.DeletePending,i.Links,i.FileSystem,string.Join(",",i.Streams));}
 public static void ExactCopies(IV2StageFs fs,V2StageSession session,byte[] originalZip){
  DedicatedContract.Need(session.PackageId==Package&&session.Bundle==System.IO.Path.Combine(Target,"Bundle")&&session.PayloadCount==766&&session.Bytes.Count==767,"R2_STAGE_EXACT_SESSION");
  foreach(var entry in session.Bytes){string p=System.IO.Path.Combine(session.Bundle,entry.Key.Replace('/','\\'));using(var h=fs.Open(p)){LimitlessV2Staging.Protected(h.Info(),p,false);DedicatedContract.Need(System.Linq.Enumerable.SequenceEqual(h.Read(),entry.Value),"R2_STAGE_EXACT_BYTES");}}
  string z=session.Bundle+".zip";using(var h=fs.Open(z)){LimitlessV2Staging.Protected(h.Info(),z,false);DedicatedContract.Need(System.Linq.Enumerable.SequenceEqual(h.Read(),originalZip),"R2_STAGE_EXACT_ZIP_BYTES");}
  session.Recheck();
 }
}

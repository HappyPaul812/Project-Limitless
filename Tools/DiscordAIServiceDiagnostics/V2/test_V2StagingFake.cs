// 실제 API 없이 native와 같은 Create/Verify 순서를 실행하는 메모리 파일 시스템입니다.
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
public sealed class V2FakeStageFs:IV2StageFs {
 public sealed class Node{public string Path,Id,Sd;public bool Dir;public byte[] Bytes;public uint Links=1;public bool Reparse,Pending;public string[] Streams;public string Fs="NTFS";}
 public Dictionary<string,Node> Nodes=new Dictionary<string,Node>(StringComparer.OrdinalIgnoreCase);
 public string Fault="";public int FailNumber=1,MutationCalls,Opens,Live,DirectoriesCreated,FilesCreated;public List<string> CreationDescriptors=new List<string>();
 int serial;
 public const string Source=@"F:\Fake\candidate.zip";
 public V2FakeStageFs(byte[] zip){Add(@"C:\",true,null);Add(@"C:\Program Files",true,null);Add(@"F:\",true,null);Add(@"F:\Fake",true,null);Add(Source,false,zip);}
 public void Add(string p,bool dir,byte[] bytes){Nodes.Add(p,new Node{Path=p,Id=(++serial).ToString(),Dir=dir,Bytes=bytes==null?Array.Empty<byte>():bytes.ToArray(),Sd=dir?LimitlessV2Staging.DirSd:LimitlessV2Staging.FileSd,Streams=dir?Array.Empty<string>():new[]{"::$DATA"}});}
 public bool Exists(string p){if(Fault=="EXISTS_DENIED")throw new UnauthorizedAccessException("SECRET");return Nodes.ContainsKey(p);}
 public V2StageHandle Open(string p){Opens++;if(Fault=="OPEN_DENIED"&&Opens==FailNumber)throw new UnauthorizedAccessException("SECRET");if(!Nodes.ContainsKey(p))throw new FileNotFoundException("SECRET");Live++;return new Handle(this,Nodes[p]);}
 void Mutate(){MutationCalls++;if(Fault=="MUTATION_FAIL"&&MutationCalls==FailNumber)throw new IOException("SECRET");}
 public void Directory(string p,string sd){Mutate();if(Nodes.ContainsKey(p))throw new IOException("EXISTS");Add(p,true,null);Nodes[p].Sd=sd;DirectoriesCreated++;CreationDescriptors.Add(sd);}
 public void File(string p,byte[] bytes,string sd){Mutate();if(Nodes.ContainsKey(p))throw new IOException("EXISTS");Add(p,false,bytes);Nodes[p].Sd=sd;FilesCreated++;CreationDescriptors.Add(sd);if(Fault=="CORRUPT_COPY"&&bytes.Length>0)Nodes[p].Bytes[0]^=1;}
 public string[] Children(string p){var children=Nodes.Keys.Where(x=>!x.Equals(p,StringComparison.OrdinalIgnoreCase)&&Path.GetDirectoryName(x)!=null&&Path.GetDirectoryName(x).Equals(p,StringComparison.OrdinalIgnoreCase)).ToList();if(Fault=="EXTRA_ENTRY"&&p.EndsWith("\\Bundle"))children.Add(Path.Combine(p,"unexpected"));if(Fault=="OUTSIDE_ENTRY")children.Add(@"C:\outside");return children.ToArray();}
 sealed class Handle:V2StageHandle {
  V2FakeStageFs fs;Node node;bool closed;
  public Handle(V2FakeStageFs f,Node n){fs=f;node=n;}
  public override V2StageInfo Info(){
   var i=new V2StageInfo{Path=node.Path,Id=node.Id,Volume=node.Path.Substring(0,1),Sddl=node.Sd,Directory=node.Dir,Reparse=node.Reparse,DeletePending=node.Pending,Links=node.Links,Streams=node.Streams.ToArray(),FileSystem=node.Fs};
   bool target=node.Path.StartsWith(LimitlessV2Staging.Root,StringComparison.OrdinalIgnoreCase);
   if(target){switch(fs.Fault){case "TARGET_HARDLINK":if(!node.Dir)i.Links=2;break;case "TARGET_ADS":i.Streams=new[]{"::$DATA",":secret:$DATA"};break;case "TARGET_REPARSE":i.Reparse=true;break;case "TARGET_OWNER":i.Sddl=i.Sddl.Replace("O:BA","O:BU");break;case "TARGET_GROUP":i.Sddl=i.Sddl.Replace("G:SY","G:BU");break;case "TARGET_WRITER":i.Sddl+="(A;;FA;;;BU)";break;case "TARGET_UNPROTECTED":i.Sddl=i.Sddl.Replace("D:P","D:");break;case "TARGET_CALLBACK":i.Sddl="O:BAG:SYD:P(XA;;FA;;;SY)";break;case "TARGET_PENDING":i.DeletePending=true;break;case "TARGET_FINALPATH":i.Path=@"C:\outside";break;case "TARGET_NONNTFS":i.FileSystem="FAT32";break;}}
   return i;
  }
  public override byte[] Read(){if(fs.Fault=="READ_DENIED")throw new UnauthorizedAccessException("SECRET");return node.Bytes.ToArray();}
  public override void Dispose(){if(!closed){closed=true;fs.Live--;}}
 }
}

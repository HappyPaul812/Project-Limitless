// Worker SID의 로그온 관계만 조회합니다. Windows 보호 프로세스 토큰을 열거나 권한을 우회하지 않습니다.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Globalization;
public static class LimitlessV2WorkerFence {
 static readonly object errorLock=new object();
 public static object[] FailureCodes {get;private set;}=Array.Empty<object>();
 static object Get(object o,string key){return o.GetType().InvokeMember(key,BindingFlags.GetProperty,null,o,null);}
 static object Call(object o,string key,params object[] args){return o.GetType().InvokeMember(key,BindingFlags.InvokeMethod|BindingFlags.OptionalParamBinding,null,o,args);}
 static IEnumerable Query(object service,string sql){return (IEnumerable)Call(service,"ExecQuery",sql,"WQL",48);}
 static void Release(object o){try{if(o!=null&&Marshal.IsComObject(o))Marshal.FinalReleaseComObject(o);}catch{}}
 static long Created(string s){
  if(s==null||!System.Text.RegularExpressions.Regex.IsMatch(s,@"\A[0-9]{14}\.[0-9]{6}[+-][0-9]{3}\z"))throw new InvalidOperationException("V2_CREATION_UNKNOWN_HOLD");
  var t=DateTime.ParseExact(s.Substring(0,14),"yyyyMMddHHmmss",CultureInfo.InvariantCulture,DateTimeStyles.None).AddTicks(long.Parse(s.Substring(15,6),CultureInfo.InvariantCulture)*10);
  int offset=int.Parse(s.Substring(22,3),CultureInfo.InvariantCulture)*(s[21]=='-'?-1:1);
  return t.AddMinutes(-offset).Ticks;
 }
 public static bool AcceptSnapshot(Dictionary<int,int> parents,Dictionary<int,long> created,int rootPid){
  if(rootPid<=0||parents==null||created==null||!parents.ContainsKey(rootPid)||!created.ContainsKey(rootPid))return false;
  foreach(int pid in parents.Keys){int node=pid;var seen=new HashSet<int>();bool own=false;if(!created.ContainsKey(pid)||created[pid]<created[rootPid])return false;while(seen.Add(node)){if(node==rootPid){own=true;break;}int parent;if(!parents.TryGetValue(node,out parent)||!created.ContainsKey(parent)||created[parent]>created[node])break;node=parent;}if(!own)return false;}
  return true;
 }
 public static bool Clear(string workerSid,int rootPid){
  if(!LimitlessV2Contract.Sid(workerSid))return false;
  object locator=null,service=null;
  try{
   locator=Activator.CreateInstance(Type.GetTypeFromProgID("WbemScripting.SWbemLocator"));
   service=Call(locator,"ConnectServer",".","root\\cimv2");
   string accountPath=null;int accounts=0;
   foreach(object account in Query(service,"SELECT * FROM Win32_UserAccount WHERE SID='"+workerSid+"'")){
    try{accounts++;accountPath=(string)Get(Get(account,"Path_"),"RelPath");}finally{Release(account);}
   }
   if(accounts!=1||accountPath==null)return false;
   var parents=new Dictionary<int,int>();var created=new Dictionary<int,long>();
   foreach(object session in Query(service,"ASSOCIATORS OF {"+accountPath+"} WHERE AssocClass=Win32_LoggedOnUser ResultClass=Win32_LogonSession")){
    try{
     string path=(string)Get(Get(session,"Path_"),"RelPath");
     foreach(object process in Query(service,"ASSOCIATORS OF {"+path+"} WHERE AssocClass=Win32_SessionProcess ResultClass=Win32_Process")){
      try{int pid=Convert.ToInt32(Get(process,"ProcessId")),parent=Convert.ToInt32(Get(process,"ParentProcessId"));long time=Created((string)Get(process,"CreationDate"));if(parents.TryGetValue(pid,out int old)&&(old!=parent||created[pid]!=time))return false;parents[pid]=parent;created[pid]=time;}finally{Release(process);}
     }
    }finally{Release(session);}
   }
   if(!created.ContainsKey(rootPid)||Math.Abs(created[rootPid]-System.Diagnostics.Process.GetCurrentProcess().StartTime.ToUniversalTime().Ticks)>10000)return false;
   return AcceptSnapshot(parents,created,rootPid);
  }catch(Exception error){lock(errorLock){if(FailureCodes.Length==0)FailureCodes=LimitlessV2Contract.ErrorCodes(error);}return false;}finally{Release(service);Release(locator);}
 }
}

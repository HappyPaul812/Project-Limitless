// 추론 시험 전용입니다. 준비·usage·exec를 포함한 Python 트리에 절대 예산과 kill-on-close를 적용합니다.
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

public static class LimitlessInferenceJob90V1 {
 [StructLayout(LayoutKind.Sequential)] struct Security { public int Length; public IntPtr Descriptor; public int Inherit; }
 [StructLayout(LayoutKind.Sequential,CharSet=CharSet.Unicode)] struct Startup {
  public int Size; public string Reserved,Desktop,Title; public uint X,Y,XSize,YSize,XChars,YChars,Fill,Flags;
  public short Show,ReservedBytes; public IntPtr ReservedData,Input,Output,Error;
 }
 [StructLayout(LayoutKind.Sequential)] struct ProcessInfo { public IntPtr Process,Thread; public uint Pid,Tid; }
 [StructLayout(LayoutKind.Sequential)] struct BasicLimit {
  public long ProcessTime,JobTime; public uint Flags; public UIntPtr MinWorking,MaxWorking; public uint Active;
  public UIntPtr Affinity; public uint Priority,Scheduling;
 }
 [StructLayout(LayoutKind.Sequential)] struct Counters { public ulong A,B,C,D,E,F; }
 [StructLayout(LayoutKind.Sequential)] struct ExtendedLimit {
  public BasicLimit Basic; public Counters Io; public UIntPtr ProcessMemory,JobMemory,PeakProcess,PeakJob;
 }
 [StructLayout(LayoutKind.Sequential)] struct Accounting {
  public long User,Kernel,PeriodUser,PeriodKernel; public uint Faults,Total,Active,Terminated;
 }
 [DllImport("kernel32.dll",SetLastError=true,CharSet=CharSet.Unicode)] static extern IntPtr CreateJobObject(IntPtr attr,string name);
 [DllImport("kernel32.dll",SetLastError=true)] static extern bool SetInformationJobObject(IntPtr job,int kind,ref ExtendedLimit info,int size);
 [DllImport("kernel32.dll",SetLastError=true)] static extern bool QueryInformationJobObject(IntPtr job,int kind,out Accounting info,int size,IntPtr returned);
 [DllImport("kernel32.dll",SetLastError=true)] static extern bool AssignProcessToJobObject(IntPtr job,IntPtr process);
 [DllImport("kernel32.dll",SetLastError=true)] static extern bool TerminateJobObject(IntPtr job,uint code);
 [DllImport("kernel32.dll",SetLastError=true)] static extern bool CreatePipe(out IntPtr read,out IntPtr write,ref Security attr,uint size);
 [DllImport("kernel32.dll",SetLastError=true)] static extern bool SetHandleInformation(IntPtr handle,uint mask,uint flags);
 [DllImport("kernel32.dll",SetLastError=true,CharSet=CharSet.Unicode)] static extern IntPtr CreateFile(string name,uint access,uint share,ref Security attr,uint mode,uint flags,IntPtr template);
 [DllImport("kernel32.dll",SetLastError=true,CharSet=CharSet.Unicode)] static extern bool CreateProcess(string app,StringBuilder command,IntPtr pa,IntPtr ta,bool inherit,uint flags,IntPtr env,string cwd,ref Startup startup,out ProcessInfo info);
 [DllImport("kernel32.dll",SetLastError=true)] static extern uint ResumeThread(IntPtr thread);
 [DllImport("kernel32.dll",SetLastError=true)] static extern uint WaitForSingleObject(IntPtr handle,uint ms);
 [DllImport("kernel32.dll",SetLastError=true)] static extern bool GetExitCodeProcess(IntPtr process,out uint code);
 [DllImport("kernel32.dll",SetLastError=true)] static extern bool TerminateProcess(IntPtr process,uint code);
 [DllImport("kernel32.dll")] static extern bool CloseHandle(IntPtr handle);
 public sealed class Result {
  public uint ExitCode,TotalProcesses,ActiveProcesses; public bool TimedOut,CleanupVerified; public long StdoutBytes,StderrBytes;
  public string Stdout,StderrSummary; public bool StdoutTruncated,StderrTruncated;
 }
 sealed class Capture {
  public byte[] Bytes; public long Total;
 }
 static void Check(bool ok) { if(!ok)throw new Win32Exception(Marshal.GetLastWin32Error()); }
 static void Close(ref IntPtr handle) { if(handle!=IntPtr.Zero && handle!=new IntPtr(-1))CloseHandle(handle);handle=IntPtr.Zero; }
 static Capture Drain(IntPtr handle,int cap) {
  using(var stream=new FileStream(new SafeFileHandle(handle,true),FileAccess.Read,4096,false))
  using(var memory=new MemoryStream()) {
   var buffer=new byte[4096];long total=0;int n;
   while((n=stream.Read(buffer,0,buffer.Length))>0) {
    total+=n;int room=cap-(int)memory.Length;if(room>0)memory.Write(buffer,0,Math.Min(room,n));
   }
   Array.Clear(buffer,0,buffer.Length);return new Capture{Bytes=memory.ToArray(),Total=total};
  }
 }
 static string Quote(string value) {
  var b=new StringBuilder("\"");int slashes=0;
  foreach(char c in value) {
   if(c=='\\'){slashes++;continue;}
   if(c=='\"'){b.Append('\\',slashes*2+1);b.Append(c);slashes=0;continue;}
   b.Append('\\',slashes);slashes=0;b.Append(c);
  }
  b.Append('\\',slashes*2);b.Append('"');return b.ToString();
 }
 static string Sanitize(string text) {
  // Python 시작 오류도 자유문/경로/토큰을 출력하지 않고 진단 어휘만 남깁니다.
  var words=new System.Collections.Generic.HashSet<string>(("error failed failure access denied permission authentication unauthorized invalid token expired missing required file not found path process python codex timeout timed out connection closed read write status exit code internal server request response protocol json usage limit exceeded").Split(' '),StringComparer.OrdinalIgnoreCase);
  text=Regex.Replace(text,@"(?im)^\s*(authorization|cookie|set-cookie)\s*:.*$"," ");
  text=Regex.Replace(text,@"(?i)\bBearer\s+\S+|\bsk-\S+|\beyJ\S+|(?<!\S)[^\s@]+@[^\s@]+|[A-Za-z]:[\\/]\S+"," ");
  var b=new StringBuilder();foreach(Match m in Regex.Matches(text,@"[A-Za-z]+|[^\sA-Za-z]+")) {
   b.Append(words.Contains(m.Value)?m.Value:"[REDACTED]");b.Append(' ');if(b.Length>=4096)break;
  }
  return b.ToString(0,Math.Min(b.Length,4096));
 }
 public static Result Run(string executable,string[] args,string cwd,int milliseconds) {
  if(milliseconds<1 || milliseconds>85000)throw new ArgumentOutOfRangeException("milliseconds");
  var deadlineWatch=System.Diagnostics.Stopwatch.StartNew();
  IntPtr job=IntPtr.Zero,outputRead=IntPtr.Zero,outputWrite=IntPtr.Zero,errorRead=IntPtr.Zero,errorWrite=IntPtr.Zero,input=IntPtr.Zero;
  ProcessInfo child=new ProcessInfo();Task<Capture> stdout=null,stderr=null;bool assigned=false;
  var result=new Result();
  try {
   job=CreateJobObject(IntPtr.Zero,null);Check(job!=IntPtr.Zero);
   var limits=new ExtendedLimit();limits.Basic.Flags=0x2000;
   Check(SetInformationJobObject(job,9,ref limits,Marshal.SizeOf<ExtendedLimit>()));
   var security=new Security{Length=Marshal.SizeOf<Security>(),Inherit=1};
   Check(CreatePipe(out outputRead,out outputWrite,ref security,0));Check(SetHandleInformation(outputRead,1,0));
   Check(CreatePipe(out errorRead,out errorWrite,ref security,0));Check(SetHandleInformation(errorRead,1,0));
   input=CreateFile("NUL",0x80000000,3,ref security,3,0,IntPtr.Zero);Check(input!=new IntPtr(-1));
   var startup=new Startup{Size=Marshal.SizeOf<Startup>(),Flags=0x100,Input=input,Output=outputWrite,Error=errorWrite};
   var command=new StringBuilder(Quote(executable));foreach(string arg in args)command.Append(' ').Append(Quote(arg));
   // 새 프로세스를 정지 상태로 만든 뒤 Job에 넣습니다. 배치 전에 실행되는 자식은 없습니다.
   Check(CreateProcess(executable,command,IntPtr.Zero,IntPtr.Zero,true,0x08000004,IntPtr.Zero,cwd,ref startup,out child));
   Close(ref outputWrite);Close(ref errorWrite);Close(ref input);
   Check(AssignProcessToJobObject(job,child.Process));assigned=true;
   IntPtr op=outputRead,ep=errorRead;outputRead=errorRead=IntPtr.Zero;
   stdout=Task.Run(()=>Drain(op,65536));stderr=Task.Run(()=>Drain(ep,8192));
   Check(ResumeThread(child.Thread)!=0xffffffff);
   uint wait=WaitForSingleObject(child.Process,(uint)Math.Max(1,milliseconds-deadlineWatch.ElapsedMilliseconds-1500));
   if(wait==258){result.TimedOut=true;Check(TerminateJobObject(job,124));WaitForSingleObject(child.Process,400);}
   else Check(wait==0);
   Accounting accounting;Check(QueryInformationJobObject(job,1,out accounting,Marshal.SizeOf<Accounting>(),IntPtr.Zero));
   if(accounting.Active>0){Check(TerminateJobObject(job,125));}
   for(int i=0;i<40;i++) {
    Check(QueryInformationJobObject(job,1,out accounting,Marshal.SizeOf<Accounting>(),IntPtr.Zero));
    if(accounting.Active==0)break;Thread.Sleep(10);
   }
   result.TotalProcesses=accounting.Total;result.ActiveProcesses=accounting.Active;result.CleanupVerified=accounting.Active==0;
   Check(GetExitCodeProcess(child.Process,out result.ExitCode));
   bool drained=Task.WaitAll(new Task[]{stdout,stderr},500);result.CleanupVerified &= drained;
   if(drained) {
    var o=stdout.Result;var e=stderr.Result;
    result.Stdout=Encoding.UTF8.GetString(o.Bytes);result.StderrSummary=Sanitize(Encoding.UTF8.GetString(e.Bytes));
    result.StdoutBytes=o.Total;result.StderrBytes=e.Total;result.StdoutTruncated=o.Total>o.Bytes.Length;result.StderrTruncated=e.Total>e.Bytes.Length;
    Array.Clear(o.Bytes,0,o.Bytes.Length);Array.Clear(e.Bytes,0,e.Bytes.Length);
   }
   return result;
  } finally {
   if(child.Process!=IntPtr.Zero && !assigned)TerminateProcess(child.Process,126);
   // PowerShell 작업이 강제 종료되어도 OS가 이 handle을 닫아 자식 트리를 정리합니다.
   Close(ref job);Close(ref child.Thread);Close(ref child.Process);
   Close(ref outputRead);Close(ref outputWrite);Close(ref errorRead);Close(ref errorWrite);Close(ref input);
  }
 }
}

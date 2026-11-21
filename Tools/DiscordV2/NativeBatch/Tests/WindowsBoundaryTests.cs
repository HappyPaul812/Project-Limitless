using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text.Json;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Win32.SafeHandles;

// 기존 시험 프로젝트의 별도 live 경로입니다. 합성52개/운영 entry point를 호출하지 않습니다.
// 새 fixture 파일·빈 Job·token clone·시험 thread/process만 만들고 기존 자원은 조회만 합니다.
internal static class WindowsBoundaryTests
{
    [DllImport("kernel32.dll",CharSet=CharSet.Unicode,SetLastError=true)]private static extern bool CreateDirectoryW(string path,nint attributes);
    [DllImport("kernel32.dll",CharSet=CharSet.Unicode,SetLastError=true)]private static extern SafeFileHandle CreateFileW(string path,uint access,uint share,nint attributes,uint disposition,uint flags,nint template);
    [DllImport("kernel32.dll",CharSet=CharSet.Unicode,SetLastError=true)]private static extern SafeFileHandle CreateFileW(string path,uint access,uint share,ref AdmissionWindows.Attributes attributes,uint disposition,uint flags,nint template);
    [DllImport("advapi32.dll",CharSet=CharSet.Unicode,SetLastError=true)]private static extern bool ConvertStringSecurityDescriptorToSecurityDescriptorW(string text,uint revision,out nint descriptor,out uint size);
    [DllImport("kernel32.dll")]private static extern nint LocalFree(nint pointer);
    [DllImport("kernel32.dll",SetLastError=true)]private static extern nint OpenThread(uint access,bool inherit,uint tid);
    [DllImport("kernel32.dll")]private static extern uint GetCurrentThreadId();
    [DllImport("advapi32.dll",SetLastError=true)]private static extern bool DuplicateTokenEx(nint existing,uint access,nint attributes,int level,int type,out nint duplicate);
    [DllImport("kernel32.dll",SetLastError=true)]private static extern nint CreateToolhelp32Snapshot(uint flags,uint pid);
    [DllImport("kernel32.dll",CharSet=CharSet.Unicode,SetLastError=true)]private static extern bool Process32FirstW(nint snapshot,ref HandleCensus.ProcessEntry entry);
    [DllImport("kernel32.dll",CharSet=CharSet.Unicode,SetLastError=true)]private static extern bool Process32NextW(nint snapshot,ref HandleCensus.ProcessEntry entry);
    [DllImport("advapi32.dll",SetLastError=true)]private static extern bool CreateRestrictedToken(nint original,uint flags,uint disabled,nint disabledSids,uint removed,nint privileges,uint restricted,ref SidAttribute sids,out nint result);
    [StructLayout(LayoutKind.Sequential)]private struct SidAttribute{internal nint Sid;internal uint Flags;}
    private sealed class Failure(string message):Exception(message) { }
    private static void Need(bool value,string tag){if(!value)throw new Failure(tag);}
    private static readonly List<Dictionary<string,object?>> Rows=[];
    private static void Case(string name,Action<Dictionary<string,object?>> run)
    {
        var row=new Dictionary<string,object?>{{"name",name},{"status","RUNNING"}};Rows.Add(row);
        try{run(row);if(string.Equals(row["status"] as string,"RUNNING",StringComparison.Ordinal))row["status"]="PASS";}
        catch(Exception e)
        {
            int? win32=e is Win32Exception w?w.NativeErrorCode:null;
            row["status"]=e is Failure||win32 is 87 or 1338?"FAIL":"UNKNOWN_HOLD";
            row["error"]=e.GetType().Name;row["message"]=e.Message;row["hresult"]=e.HResult;row["win32"]=win32;
        }
        Console.WriteLine(name+": "+row["status"]);
    }
    internal static int Run(string[] args)
    {
        Need(args.Length is 2 or 3,"WINDOWS_BOUNDARY_ARGUMENTS");
        string input=Path.GetFullPath(args[0]),root=Path.GetFullPath(args[1]);
        Need(Path.GetFileName(root).StartsWith("WindowsBoundary-",StringComparison.Ordinal),"FIXTURE_ROOT_NAME");
        AdmissionWindows.Check(CreateDirectoryW(root,0)); // ERROR_ALREADY_EXISTS도 실패: 기존 폴더 재사용 금지
        using var configuration=JsonDocument.Parse(File.ReadAllBytes(input));
        string taskPath=configuration.RootElement.GetProperty("task_path").GetString()!;
        string[]? only=args.Length==3?args[2].Split(','):null;
        bool Select(string name)=>only is null||only.Contains(name,StringComparer.Ordinal);
        nint token=0,job=0,duplicate=0;
        var metadata=new Dictionary<string,object?>{{"baseline","d6d8c007deb754c2685f2a9542c0f0a76612cb40"},{"root",root},{"synthetic_tests_run",0},{"operational_mutations",0},{"native_aot_attempts",0}};
        try
        {
            Native.Check(Native.OpenProcessToken(Native.GetCurrentProcess(),0xa,out token));
            using var identity=new WindowsIdentity(token);string user=identity.User!.Value;
            metadata["actor_appcontainer"]=Native.Info<int>(token,29);metadata["actor_elevated"]=Native.Info<int>(token,20);metadata["actor_session"]=Native.Info<int>(token,12);
            metadata["actor_is_target_sid"]=user==Native.Worker;metadata["process_bits"]=nint.Size*8;
            Need(nint.Size==8,"ACTUAL_X64_REQUIRED");
            byte[] appContainer=AppContainerSid(token);
            string package=appContainer.Length==0?"":new SecurityIdentifier(appContainer,0).Value;
            // 양성 fixture에 현재 package SID만 추가합니다. production ACL을 완화하지 않습니다.
            string Ace(uint access)=>"(A;;0x"+access.ToString("x")+";;;"+user+")"+(package.Length==0?"":"(A;;0x"+access.ToString("x")+";;;"+package+")");
            if(Select("file-owner-rights"))Case("file-owner-rights",row=>
            {
                bool? baseline=null,guarded=null;
                foreach(bool ownerRights in new[]{false,true})
                {
                    string path=Path.Combine(root,ownerRights?"owner-guarded.bin":"owner-baseline.bin");
                    using(var file=NewFile(path,"O:"+user+"D:P"+(ownerRights?"(D;;0xd0000;;;OW)":"")+Ace(0x120003)))
                    {
                        byte[] sd=AdmissionWindows.Descriptor(file.DangerousGetHandle(),1);
                        var descriptor=new RawSecurityDescriptor(sd,0);row[ownerRights?"guard_owner_matches":"baseline_owner_matches"]=descriptor.Owner?.Value==user;
                        row[ownerRights?"guard_group_present":"baseline_group_present"]=descriptor.Group is not null;
                        Need(descriptor.Owner?.Value==user,"ACTUAL_OWNER_CHANGED");
                        bool writeData=AdmissionWindows.Allowed(sd,token,2),writeDac=AdmissionWindows.Allowed(sd,token,AdmissionPolicy.WriteDac);
                        Need(writeData,"POSITIVE_FILE_DATA_ACCESS_DENIED");if(ownerRights)guarded=writeDac;else baseline=writeDac;
                        row[ownerRights?"guard_accesscheck_write_dac":"baseline_accesscheck_write_dac"]=writeDac;
                    }
                    using var opened=CreateFileW(path,AdmissionPolicy.WriteDac,7,0,3,0,0);int error=opened.IsInvalid?Marshal.GetLastWin32Error():0;
                    row[ownerRights?"guard_kernel_open_error":"baseline_kernel_open_error"]=error;
                    Need(ownerRights?opened.IsInvalid&&error==5:!opened.IsInvalid,"OWNER_RIGHTS_KERNEL_COMPARISON");
                }
                Need(baseline==true&&guarded==false,"OWNER_RIGHTS_ACCESSCHECK_COMPARISON");
            });
            if(Select("file-sharing-lock"))Case("file-sharing-lock",row=>
            {
                string path=Path.Combine(root,"sharing.bin");using(var file=NewFile(path,"O:"+user+"D:P"+Ace(0x130003))){ }
                using(var pin=AdmissionWindows.Pin(path,false))
                {
                    Native.Check(Native.GetHandleInformation(pin.DangerousGetHandle(),out uint flags));Need((flags&1)==0,"PIN_INHERITABLE");
                    foreach(uint access in new[]{2U,AdmissionPolicy.Delete})
                    {using var contender=CreateFileW(path,access,7,0,3,0,0);int error=contender.IsInvalid?Marshal.GetLastWin32Error():0;row["locked_open_"+access]=error;Need(contender.IsInvalid&&error==32,"FILE_SHARE_NOT_ENFORCED");}
                }
                using var after=CreateFileW(path,2|AdmissionPolicy.Delete,7,0,3,0,0);Need(!after.IsInvalid,"POSITIVE_AFTER_PIN_CLOSE_FAILED");
            });
            if(Select("restricted-token-denial"))Case("restricted-token-denial",row=>
            {
                string path=Path.Combine(root,"restricted.bin");using var file=NewFile(path,"O:"+user+"D:P"+Ace(0x120003));
                byte[] sd=AdmissionWindows.Descriptor(file.DangerousGetHandle(),1);Need(AdmissionWindows.Allowed(sd,token,1),"UNRESTRICTED_POSITIVE_DENIED");
                var restrictedSid=new SecurityIdentifier("S-1-5-12");byte[] bytes=new byte[restrictedSid.BinaryLength];restrictedSid.GetBinaryForm(bytes,0);
                nint pointer=Marshal.AllocHGlobal(bytes.Length),restricted=0;
                try
                {
                    Marshal.Copy(bytes,0,pointer,bytes.Length);var attribute=new SidAttribute{Sid=pointer};
                    Native.Check(CreateRestrictedToken(token,1,0,0,0,0,1,ref attribute,out restricted));
                    bool read=AdmissionWindows.Allowed(sd,restricted,1);row["restricted_read"]=read;Need(!read,"RESTRICTING_SID_NOT_ENFORCED");
                }
                finally{if(restricted!=0)Native.CloseHandle(restricted);Marshal.FreeHGlobal(pointer);}
            });
            if(Select("empty-job-kernel-abi-acl"))Case("empty-job-kernel-abi-acl",row=>
            {
                job=Native.CreateJobObjectW(0,null);Native.Check(job!=0);
                var limits=new Native.ExtendedLimits();limits.Basic.Flags=0x2008;limits.Basic.Active=1;
                Native.Check(Native.SetInformationJobObject(job,9,ref limits,(uint)Marshal.SizeOf<Native.ExtendedLimits>()));
                Native.Check(Native.QueryLimits(job,9,out var actual,(uint)Marshal.SizeOf<Native.ExtendedLimits>(),0));Need(actual.Basic.Flags==0x2008&&actual.Basic.Active==1,"ACTUAL_JOB_LIMIT_LAYOUT");
                Native.Check(Native.QueryAccounting(job,1,out var account,(uint)Marshal.SizeOf<Native.Accounting>(),0));Need(account.Total==0&&account.Active==0,"NONEMPTY_FIXTURE_JOB");
                row["active"]=account.Active;row["total"]=account.Total;row["limits_flags"]=actual.Basic.Flags;row["native_job_structures"]=true;
                using var security=new AdmissionWindows.KernelSecurity(user);
                try{security.Harden(job,AdmissionWindows.KernelKind.Job);row["dacl_readback"]=true;}
                finally{var sd=new RawSecurityDescriptor(AdmissionWindows.Descriptor(job,6),0);row["actual_dacl_control"]=(int)sd.ControlFlags;row["actual_kernel_sddl"]=sd.GetSddlForm(AccessControlSections.Owner|AccessControlSections.Access);}
            });
            if(Select("isolated-kernel-object-kinds"))Case("isolated-kernel-object-kinds",row=>
            {
                using var security=new AdmissionWindows.KernelSecurity(user);
                nint clone=0,threadHandle=0;using var ready=new ManualResetEventSlim();using var finish=new ManualResetEventSlim();uint threadId=0;
                var thread=new Thread(()=>{threadId=GetCurrentThreadId();ready.Set();finish.Wait(10000);}){IsBackground=true};
                try
                {
                    Native.Check(DuplicateTokenEx(token,0xf01ff,0,2,2,out clone));security.Harden(clone,AdmissionWindows.KernelKind.Token);row["isolated_duplicate_token_dacl"]=true;
                    thread.Start();Need(ready.Wait(2000),"FIXTURE_THREAD_READY");threadHandle=OpenThread(0x1fffff,false,threadId);Native.Check(threadHandle!=0);
                    security.Harden(threadHandle,AdmissionWindows.KernelKind.Thread);row["isolated_new_thread_dacl"]=true;
                    Need(Process.GetCurrentProcess().SessionId!=0,"NO_SESSION0_FIXTURE_PROCESS");
                    var info=new ProcessStartInfo(Environment.ProcessPath!){UseShellExecute=false,CreateNoWindow=true};info.ArgumentList.Add(typeof(AdmissionTests).Assembly.Location);info.ArgumentList.Add("--windows-object-peer");string processReady=Path.Combine(root,"object-peer-ready.txt");info.ArgumentList.Add(processReady);
                    using var peer=Process.Start(info)??throw new Failure("FIXTURE_PROCESS_START");
                    try{var wait=Stopwatch.StartNew();while(!File.Exists(processReady)&&!peer.HasExited&&wait.ElapsedMilliseconds<4000)Thread.Sleep(20);Need(File.Exists(processReady)&&File.ReadAllText(processReady)==peer.Id.ToString(),"FIXTURE_PROCESS_READY");security.Harden(peer.Handle,AdmissionWindows.KernelKind.Process);row["isolated_new_process_dacl"]=true;}
                    finally{if(!peer.WaitForExit(7000)){peer.Kill();peer.WaitForExit(1000);}}
                    row["fixture_process_exit"]=peer.ExitCode;Need(peer.ExitCode==0,"FIXTURE_PROCESS_EXIT");
                }
                finally{finish.Set();if(thread.IsAlive)thread.Join(3000);if(threadHandle!=0)Native.CloseHandle(threadHandle);if(clone!=0)Native.CloseHandle(clone);}
            });
            if(only is not null&&Select("handle-peer-publication"))Case("handle-peer-publication",row=>
            {
                if(job==0){job=Native.CreateJobObjectW(0,null);Native.Check(job!=0);}
                WithPeer(root,job,row,()=>{});
            });
            if(Select("actual-handle-census"))Case("actual-handle-census",row=>
            {
                if(job==0){job=Native.CreateJobObjectW(0,null);Native.Check(job!=0);}
                Native.Check(AdmissionWindows.DuplicateHandle(Native.GetCurrentProcess(),job,Native.GetCurrentProcess(),out duplicate,0,false,0));
                var statuses=new List<string>();row["query_statuses"]=statuses;
                HandleCensus.Check(new Dictionary<nint,uint>{{job,AdmissionPolicy.JobDanger},{duplicate,AdmissionPolicy.JobDanger}},(status,length)=>statuses.Add("0x"+unchecked((uint)status).ToString("x8")+"/"+length));
                row["own_real_handles_matched"]=2;
                WithPeer(root,job,row,()=>
                {
                    try{HandleCensus.Check(new Dictionary<nint,uint>{{job,AdmissionPolicy.JobDanger}});throw new Failure("FOREIGN_JOB_HANDLE_NOT_DETECTED");}
                    catch(InvalidOperationException e)when(e.Message=="EXTERNAL_DANGEROUS_HANDLE"){row["foreign_test_handle_transfer"]="OBSERVED_AND_REJECTED_ZERO_RIGHTS";}
                });
            });
            if(Select("process-denial-block"))Case("process-denial-block",row=>
            {
                int count=0;bool systemSeen=false;nint snapshot=CreateToolhelp32Snapshot(2,0);Native.Check(snapshot!=new nint(-1));
                try{var e=new HandleCensus.ProcessEntry{Size=(uint)Marshal.SizeOf<HandleCensus.ProcessEntry>(),Name=""};Native.Check(Process32FirstW(snapshot,ref e));do{count++;if(e.Pid==4)systemSeen=true;}while(Process32NextW(snapshot,ref e));}
                finally{Native.CloseHandle(snapshot);}
                row["snapshot_count"]=count;row["pid4_in_snapshot"]=systemSeen;
                nint direct=AdmissionWindows.OpenProcess(0x1000,false,4);row["direct_pid4_open_error"]=direct==0?Marshal.GetLastWin32Error():0;if(direct!=0)Native.CloseHandle(direct);
                // 기존 프로세스는 조회만 합니다. 열 수 없으면 production 경로가 HOLD하는지 확인합니다.
                var observations=new List<object>();row["process_observations"]=observations;
                try{HandleCensus.Exclusive(null,(pid,name,stage,error)=>observations.Add(new{pid,name,stage,error}));row["complete_census"]=true;}
                catch(InvalidOperationException e)when(e.Message=="PROCESS_CENSUS_APPCONTAINER_VIEW_UNKNOWN"){row["safe_block"]=true;row["complete_census"]="UNKNOWN_FILTERED_VIEW";row["status"]="UNKNOWN_HOLD";}
                catch(InvalidOperationException e)when(e.Message=="PROCESS_CENSUS_OBSERVATIONS_UNKNOWN"){row["safe_block"]=true;row["complete_census"]="UNKNOWN_PARTIAL_OBSERVATIONS";row["status"]="UNKNOWN_HOLD";}
                catch(Win32Exception e)when(e.NativeErrorCode==5){row["safe_block"]=true;row["complete_census"]="UNKNOWN_ACCESS_DENIED";row["win32"]=5;row["status"]="UNKNOWN_HOLD";}
            });
            if(Select("task-com-readonly"))Case("task-com-readonly",row=>
            {
                using var scheduler=new TaskSchedulerNative(taskPath);var task=scheduler.Observe();Need(task is not null,"BASELINE_TASK_NOT_FOUND");
                using var reader=XmlReader.Create(new StringReader(task!.Xml),new XmlReaderSettings{DtdProcessing=DtdProcessing.Prohibit,XmlResolver=null,MaxCharactersInDocument=1048576});
                var xml=XDocument.Load(reader);XNamespace ns="http://schemas.microsoft.com/windows/2004/02/mit/task";
                int triggers=xml.Root!.Element(ns+"Triggers")?.Elements().Count()??0;
                Need(!task.Enabled&&task.State==1&&task.Instances.Length==0&&triggers==0,"BASELINE_TASK_CONTRACT_CHANGED");
                row["enabled"]=task.Enabled;row["state"]=task.State;row["trigger_count"]=triggers;row["instances"]=task.Instances.Length;
                row["xml_sha256"]=Hash(System.Text.Encoding.UTF8.GetBytes(task.Xml));row["sddl_sha256"]=Hash(System.Text.Encoding.UTF8.GetBytes(task.Sddl));
                row["comparison"]="EXISTING_DISABLED_TRIGGER0_SUBSET_ONLY_NOT_R1_REGISTRATION_PROOF";
            });
        }
        finally
        {
            if(duplicate!=0)Native.CloseHandle(duplicate);if(job!=0)Native.CloseHandle(job);if(token!=0)Native.CloseHandle(token);
            metadata["rows"]=Rows;metadata["fixture_cleanup"]="kernel handles closed; files retained, no ACL reset/delete";
            metadata["release_gate"]=(bool)typeof(Program).GetField("ReleaseAdmissionImplemented",BindingFlags.NonPublic|BindingFlags.Static)!.GetValue(null)!;
            bool complete=Rows.Count==8&&Rows.All(r=>(r["status"] as string)=="PASS")&&Rows.Any(r=>(r["name"] as string)=="actual-handle-census"&&r.ContainsKey("foreign_test_handle_transfer"));
            metadata["status"]=complete?"PASS_CURRENT_CONTEXT_ONLY":"HOLD";
            metadata["native_aot_boundary_conditions_passed"]=complete;
            File.WriteAllText(Path.Combine(root,"Result.json"),JsonSerializer.Serialize(metadata,new JsonSerializerOptions{WriteIndented=true}));
        }
        return Rows.Count==8&&Rows.All(r=>(r["status"] as string)=="PASS")?0:2;
    }
    internal static int ObjectPeer(string[] args){if(args.Length!=1||Process.GetCurrentProcess().SessionId==0)return 3;string ready=Path.GetFullPath(args[0]);if(!Path.GetFileName(Path.GetDirectoryName(ready)!).StartsWith("WindowsBoundary-",StringComparison.Ordinal)||File.Exists(ready))return 4;File.WriteAllText(ready,Environment.ProcessId.ToString());Thread.Sleep(4000);return 0;}
    private static void WithPeer(string root,nint job,Dictionary<string,object?> row,Action whileHeld)
    {
        Need(Process.GetCurrentProcess().SessionId!=0,"NO_SESSION0_FIXTURE_PROCESS");
        string ready=Path.Combine(root,$"peer-{Environment.ProcessId}-{Guid.NewGuid():N}.ready.json");
        var info=new ProcessStartInfo(Environment.ProcessPath!){UseShellExecute=false,CreateNoWindow=true};
        foreach(string argument in new[]{typeof(AdmissionTests).Assembly.Location,"--windows-handle-peer",Environment.ProcessId.ToString(),((nuint)job).ToString(),ready})info.ArgumentList.Add(argument);
        using var peer=Process.Start(info)??throw new Failure("FIXTURE_PEER_START");
        try
        {
            var watch=Stopwatch.StartNew();while(!File.Exists(ready)&&!peer.HasExited&&watch.ElapsedMilliseconds<4000)Thread.Sleep(20);
            Need(File.Exists(ready),"FIXTURE_PEER_DUPLICATION_UNAVAILABLE");
            // ready는 자식의 write/flush/close 이후에만 게시됩니다. 공유 조건을 완화하지 않습니다.
            using var proof=JsonDocument.Parse(File.ReadAllBytes(ready));Need(proof.RootElement.GetProperty("pid").GetInt32()==peer.Id&&proof.RootElement.GetProperty("zero_rights_duplicate").GetBoolean(),"FIXTURE_PEER_ID");
            row["peer_publication"]="CLOSED_THEN_MOVED_UNIQUE_PARENT_NONCE_CHILD_TEMP";
            whileHeld();
        }
        finally
        {
            try{using var release=new FileStream(ready+".release",FileMode.CreateNew,FileAccess.Write,FileShare.Read);release.Flush(true);}
            finally
            {
                // release 기록 실패도 자신이 생성한 시험 process의 정리를 건너뛰지 않습니다.
                if(!peer.WaitForExit(12000)){peer.Kill();peer.WaitForExit(1000);}
            }
        }
        row["peer_exit"]=peer.ExitCode;Need(peer.ExitCode==0,"FIXTURE_PEER_EXIT");
    }
    internal static int Peer(string[] args)
    {
        nint parent=0,copy=0,snapshot=0;
        try
        {
            Need(args.Length==3 && uint.TryParse(args[0],out _ ) && ulong.TryParse(args[1],out _),"PEER_ARGUMENTS");
            uint expected=uint.Parse(args[0]);Need(Process.GetCurrentProcess().SessionId!=0,"NO_SESSION0_PEER");
            Need(System.Text.RegularExpressions.Regex.IsMatch(Path.GetFileName(args[2]),"^peer-"+expected+"-[a-f0-9]{32}\\.ready\\.json$")&&Path.GetFileName(Path.GetDirectoryName(Path.GetFullPath(args[2]))!).StartsWith("WindowsBoundary-",StringComparison.Ordinal),"PEER_FIXTURE_PATH");
            snapshot=CreateToolhelp32Snapshot(2,0);Native.Check(snapshot!=new nint(-1));
            var entry=new HandleCensus.ProcessEntry{Size=(uint)Marshal.SizeOf<HandleCensus.ProcessEntry>(),Name=""};Native.Check(Process32FirstW(snapshot,ref entry));uint actualParent=0;
            do{if(entry.Pid==Environment.ProcessId){actualParent=entry.Parent;break;}}while(Process32NextW(snapshot,ref entry));
            Need(actualParent==expected,"PEER_ONLY_ACTUAL_PARENT");
            parent=AdmissionWindows.OpenProcess(0x40,false,expected);Native.Check(parent!=0);
            Native.Check(AdmissionWindows.DuplicateHandle(parent,(nint)(nuint)ulong.Parse(args[1]),Native.GetCurrentProcess(),out copy,0,false,0));
            string writing=args[2]+".writing."+Environment.ProcessId;
            using(var file=new FileStream(writing,FileMode.CreateNew,FileAccess.Write,FileShare.Read))
            {JsonSerializer.Serialize(file,new{pid=Environment.ProcessId,zero_rights_duplicate=true});file.Flush(true);}
            File.Move(writing,args[2],false); // 같은 디렉터리에서 닫힌 결과만 게시하고 기존 파일은 덮어쓰지 않습니다.
            var watch=Stopwatch.StartNew();while(!File.Exists(args[2]+".release")&&watch.ElapsedMilliseconds<10000)Thread.Sleep(20);
            return File.Exists(args[2]+".release")?0:5;
        }
        catch{return 3;}
        finally{if(copy!=0)Native.CloseHandle(copy);if(parent!=0)Native.CloseHandle(parent);if(snapshot!=0&&snapshot!=new nint(-1))Native.CloseHandle(snapshot);}
    }
    private static string Hash(byte[] raw)=>Convert.ToHexString(SHA256.HashData(raw)).ToLowerInvariant();
    private static SafeFileHandle NewFile(string path,string sddl)
    {
        AdmissionWindows.Check(ConvertStringSecurityDescriptorToSecurityDescriptorW(sddl,1,out nint descriptor,out _));
        try{var attributes=new AdmissionWindows.Attributes{Size=(uint)Marshal.SizeOf<AdmissionWindows.Attributes>(),Descriptor=descriptor};var file=CreateFileW(path,0x120003,7,ref attributes,1,0,0);if(file.IsInvalid){int error=Marshal.GetLastWin32Error();file.Dispose();throw new Win32Exception(error);}return file;}
        finally{LocalFree(descriptor);}
    }
    private static byte[] AppContainerSid(nint token)
    {
        if(Native.Info<int>(token,29)==0)return [];
        Native.GetTokenInformation(token,31,0,0,out uint length);Need(length is >0 and <=65536,"APP_CONTAINER_SIZE");nint buffer=Marshal.AllocHGlobal((int)length);
        try{Native.Check(Native.GetTokenInformation(token,31,buffer,length,out _));var sid=new SecurityIdentifier(Marshal.ReadIntPtr(buffer));byte[] bytes=new byte[sid.BinaryLength];sid.GetBinaryForm(bytes,0);return bytes;}
        finally{Marshal.FreeHGlobal(buffer);}
    }
}

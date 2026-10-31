using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using Microsoft.Win32.SafeHandles;

// 실제 어댑터는 실행 봉인 뒤에 있습니다. 이번에는 controller + fake backend만 실행합니다.
internal sealed class AdminWindows : IAdminBackend,IDisposable
{
    private readonly List<SafeFileHandle> pins=[];
    private TaskSchedulerNative? scheduler;
    private AccountObservation? account;
    private TrialContract? preparedContract;
    private readonly string expectedSha;
    public DateTimeOffset Now=>DateTimeOffset.UtcNow;
    private TaskSchedulerNative Scheduler=>scheduler??=new(Native.TaskPath);
    private AdminWindows(string sha){expectedSha=sha;}
    internal static int Run(string[] args)
    {
        bool registration=args[0]=="--register-only",stop=args[0]=="--stop-owned-instance";
        AdmissionPolicy.Require(args.Length==(registration?6:stop?7:5) && args[1]=="--contract" && args[3]=="--contract-sha","ADMIN_ARGUMENTS");
        if(registration)AdmissionPolicy.Require(args[5]=="--password-console","ADMIN_PASSWORD_ARGUMENT");
        Guid? instance=null;
        if(stop){AdmissionPolicy.Require(args[5]=="--instance" && Guid.TryParseExact(args[6],"D",out _),"INSTANCE_ARGUMENT");instance=Guid.Parse(args[6]);}
        AdmissionPolicy.Require(args[2]==Path.Combine(Native.CodeRoot,"TrialContract.json") && AdmissionPolicy.Sha(args[4]),"ADMIN_CONTRACT_LOCATION");
        Native.NoImpersonation();
        using var identity=WindowsIdentity.GetCurrent();
        AdmissionPolicy.Require(new WindowsPrincipal(identity).IsInRole(WindowsBuiltInRole.Administrator),"ELEVATED_ADMIN_REQUIRED");
        Native.Check(Native.OpenProcessToken(Native.GetCurrentProcess(),8,out nint token));
        try{AdmissionPolicy.Require(Native.Info<int>(token,20)==1,"ADMIN_NOT_ELEVATED");}finally{Native.CloseHandle(token);}
        using var backend=new AdminWindows(args[4]);
        backend.PinTree(Native.CodeRoot,false);backend.PinTree(Native.EvidenceRoot,true);
        byte[] raw=backend.Read(args[2],4096,false);
        using var cancel=new CancellationTokenSource();
        ConsoleCancelEventHandler handler=(_,e)=>{e.Cancel=true;cancel.Cancel();};Console.CancelKeyPress+=handler;
        try{return AdminController.ExecuteNative(args[0],raw,args[4],AdmissionBinding.Current,backend,instance,cancel.Token);}
        finally{Console.CancelKeyPress-=handler;}
    }
    public void Preflight(TrialContract contract,bool cleanup)
    {
        preparedContract=contract;
        AdmissionPolicy.Require(Environment.ProcessPath==Path.Combine(Native.CodeRoot,"Supervisor.exe"),"ADMIN_IMAGE_LOCATION");
        AdmissionPolicy.Require(FileHash(Path.Combine(Native.CodeRoot,"Supervisor.exe"))==contract.SupervisorSha256 && FileHash(Path.Combine(Native.CodeRoot,"Probe.exe"))==contract.ProbeSha256,"ADMIN_IMAGE_SHA");
        account=AccountNative.Read(Now,!cleanup && contract.Scope!.Register);
    }
    public AccountObservation Account()=>account??throw new InvalidOperationException("PREFLIGHT_REQUIRED");
    public byte[] IsolationEvidence(string name,int limit)
    {
        AdmissionPolicy.Require(name==Path.GetFileName(name),"FAKE_TRUST_PATH");
        return Read(Path.Combine(Native.CodeRoot,name),limit,true);
    }
    public byte[] Approval(string operation,string trialId)=>Read(Path.Combine(Native.CodeRoot,OperationApproval.FileName(trialId,operation)),4096,false);
    public TaskObservation? Observe()=>Scheduler.Observe();
    public ControlReceipt? Receipt(string name)
    {
        string path=RecordPath(name);
        // File.Exists의 접근거부를 부재로 해석하지 않도록 Open의 정확한 not-found만 허용합니다.
        byte[] bytes;
        try{bytes=Read(path,65536,true);}catch(System.ComponentModel.Win32Exception e) when(e.NativeErrorCode==2){return null;}
        return ControlReceipt.Parse(bytes);
    }
    public void Save(string name,ControlReceipt receipt)
    {
        AdmissionPolicy.Require(receipt.ContractSha==expectedSha,"RECEIPT_WRITE_BINDING");
        using var stream=AdmissionWindows.CreateAdminRecord(RecordPath(name));
        byte[] bytes=System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(receipt,ControlJson.Default.ControlReceipt);
        stream.Write(bytes);((FileStream)stream).Flush(true);
    }
    public void Register(string xml,string sddl,CancellationToken cancellation)=>Scheduler.Register(xml,sddl,cancellation);
    public void Enable(bool enabled)=>Scheduler.Enable(enabled);
    public Guid Run()=>Scheduler.Run();
    public void Stop(Guid instance)=>Scheduler.Stop(instance);
    public void Remove()=>Scheduler.Remove();
    public bool CleanupProven(string trialId)
    {
        byte[] final=Read(Path.Combine(Native.EvidenceRoot,trialId+".final"),128,false,true);
        if(!final.AsSpan().SequenceEqual(Encoding.UTF8.GetBytes("TOKEN_PROBE_ONLY_COMPLETE\n")))return false;
        // 기본 계약은 전역 검사를 유지합니다. 별도 FAKE_ONLY 계약만 보호된 신뢰 근거를 다시 요구합니다.
        FakeOnlyPolicy.Exclusivity(preparedContract??throw new InvalidOperationException("PREFLIGHT_REQUIRED"),Now,IsolationEvidence,()=>HandleCensus.Exclusive(null),cleanup:true);return true;
    }
    private static string RecordPath(string name)
    {
        AdmissionPolicy.Require(name.Length is >0 and <200 && name.All(c=>c is >= '0' and <= '9' or >= 'a' and <= 'z' or '.' or '-'),"RECORD_NAME");
        return Path.Combine(Native.EvidenceRoot,name);
    }
    private void PinTree(string path,bool evidence)
    {
        var chain=new Stack<string>();for(DirectoryInfo? directory=new(path);directory is not null;directory=directory.Parent)chain.Push(directory.FullName);
        while(chain.Count>0)
        {
            string directory=chain.Pop();bool leaf=chain.Count==0;var handle=AdmissionWindows.Pin(directory,true);pins.Add(handle);
            AdmissionPolicy.Descriptor(new RawSecurityDescriptor(AdmissionWindows.Descriptor(handle.DangerousGetHandle(),1),0),Native.Worker,leaf&&evidence,!leaf);
        }
    }
    private SafeFileHandle Pin(string path,bool receipt,bool workerRecord=false)
    {
        var handle=AdmissionWindows.Pin(path,false);pins.Add(handle);
        var sd=new RawSecurityDescriptor(AdmissionWindows.Descriptor(handle.DangerousGetHandle(),1),0);
        if(workerRecord)AdmissionPolicy.RecordDescriptor(sd,Native.Worker);
        else AdmissionPolicy.Descriptor(sd,Native.Worker,false);
        if(receipt)AdmissionPolicy.Require(AdmissionPolicy.TrustedOwner(sd.Owner?.Value??""),"RECEIPT_OWNER");
        return handle;
    }
    private byte[] Read(string path,int limit,bool receipt,bool workerRecord=false)
    {
        var file=Pin(path,receipt,workerRecord);long size=RandomAccess.GetLength(file);
        AdmissionPolicy.Require(size>0 && size<=limit,"INPUT_SIZE");byte[] bytes=new byte[size];int done=0;
        while(done<bytes.Length){int read=RandomAccess.Read(file,bytes.AsSpan(done),done);AdmissionPolicy.Require(read>0,"INPUT_SHORT_READ");done+=read;}
        return bytes;
    }
    private string FileHash(string path)
    {
        var file=Pin(path,false);long size=RandomAccess.GetLength(file);AdmissionPolicy.Require(size is >0 and <=268435456,"IMAGE_SIZE");
        using var hash=IncrementalHash.CreateHash(HashAlgorithmName.SHA256);byte[] bytes=new byte[65536];long offset=0;
        while(offset<size){int read=RandomAccess.Read(file,bytes.AsSpan(0,(int)Math.Min(bytes.Length,size-offset)),offset);AdmissionPolicy.Require(read>0,"IMAGE_SHORT_READ");hash.AppendData(bytes,0,read);offset+=read;}
        return Convert.ToHexString(hash.GetHashAndReset()).ToLowerInvariant();
    }
    public void Dispose(){scheduler?.Dispose();for(int i=pins.Count-1;i>=0;i--)pins[i].Dispose();pins.Clear();}
}

using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Win32.SafeHandles;

// Supervisor 전체 수명 동안 경로/파일/소비 marker/토큰 HANDLE을 보유합니다.
// 여기에는 Task 등록이나 사용자 권리 변경, 디렉터리 ACL 수정이 없습니다.
internal sealed class ReleaseAdmission : IDisposable
{
    private readonly List<SafeFileHandle> pins=[];
    private readonly Dictionary<nint,uint> watched=[];
    private IDisposable? consumption;
    private readonly Dictionary<string,byte[]> trustInputs=[];
    private nint token,process,thread,childToken;
    private OperationApproval startOperation=null!;
    private string expectedContractSha="";
    private ControlReceipt startIntent=null!;
    internal TrialContract Contract { get; private set; }=null!;
    internal Native.TokenStats Identity { get; private set; }
    internal AdmissionWindows.KernelSecurity Security { get; }=new(Native.Worker);

    internal static ReleaseAdmission Acquire(string contractPath,string contractSha)
    {
        var admission=new ReleaseAdmission();
        try {admission.Prepare(contractPath,contractSha);return admission;}
        catch {admission.Dispose();throw;}
    }
    private void Prepare(string contractPath,string contractSha)
    {
        AdmissionPolicy.Require(AdmissionPolicy.Sha(contractSha),"CONTRACT_HASH_FORMAT");
        Native.NoImpersonation();Identity=Native.Identity(Native.GetCurrentProcess());
        token=AdmissionWindows.Token(Native.GetCurrentProcess());AdmissionWindows.LimitedToken(token);

        PinDirectoryTree(Native.CodeRoot,false);PinDirectoryTree(Native.EvidenceRoot,true);
        var contract=PinFile(contractPath);byte[] raw=ReadSmall(contract,4096);
        AdmissionPolicy.Require(Hash(raw)==contractSha,"CONTRACT_SHA");
        AdmissionPolicy.Contract(raw,DateTimeOffset.UtcNow,AdmissionBinding.Current);
        Contract=System.Text.Json.JsonSerializer.Deserialize(raw,ContractJson.Default.TrialContract) ?? throw new InvalidOperationException("CONTRACT_NULL");
        NativeTrialPolicy.Shape(Contract,"--supervise",DateTimeOffset.UtcNow);
        expectedContractSha=contractSha;
        AdmissionPolicy.Require(Contract.Scope!.Start,"SUPERVISOR_START_NOT_APPROVED");
        var startApproval=PinFile(Path.Combine(Native.CodeRoot,OperationApproval.FileName(Contract.TrialId,"--start-once")));
        byte[] operationRaw=ReadSmall(startApproval,4096);
        var operation=OperationApproval.Validate(operationRaw,Contract.TrialId,contractSha,"--start-once",DateTimeOffset.UtcNow,false);
        startOperation=operation;
        var registration=ControlRecord("registered");startIntent=ControlRecord("start-intent");
        ReceiptPolicy.Start(registration,startIntent,operation,Contract.TrialId,contractSha);
        var supervisor=PinFile(Path.Combine(Native.CodeRoot,"Supervisor.exe"));
        var probe=PinFile(Path.Combine(Native.CodeRoot,"Probe.exe"));
        AdmissionPolicy.Require(Hash(supervisor)==Contract.SupervisorSha256 && Hash(probe)==Contract.ProbeSha256,"IMAGE_SHA");
        CheckScope(null);
        // 계약·이미지·신뢰 자료·실제 계정을 읽기 검증하기 전에는 DACL을 바꾸지 않습니다.
        void ValidateEntry()=>TrialEntryGate.Validate(raw,contractSha,"--supervise",operationRaw,AdmissionBinding.Current,
            ()=>DateTimeOffset.UtcNow,ReadTrust,c=> {
                AdmissionPolicy.Require(Hash(supervisor)==c.SupervisorSha256 && Hash(probe)==c.ProbeSha256,"IMAGE_SHA");
                CheckScope(null);
            },()=>AccountNative.Read(DateTimeOffset.UtcNow,false));
        // 읽기 HANDLE만 복제하여 외부 위험 HANDLE을 먼저 검사합니다. 아직 DACL/영구 기록은 바꾸지 않습니다.
        AdmissionWindows.Check(AdmissionWindows.DuplicateHandle(Native.GetCurrentProcess(),Native.GetCurrentProcess(),Native.GetCurrentProcess(),out process,0,false,2));
        AdmissionWindows.Check(AdmissionWindows.DuplicateHandle(Native.GetCurrentProcess(),Native.GetCurrentThread(),Native.GetCurrentProcess(),out thread,0,false,2));
        watched.Add(process,AdmissionPolicy.ProcessDanger);watched.Add(thread,AdmissionPolicy.ThreadDanger);watched.Add(token,AdmissionPolicy.TokenDanger);
        HandleCensus.Check(watched);
        ValidateEntry();
        // 완전한 읽기 승인 후 첫 영구 변경입니다. 중복은 CREATE_NEW에서 거부하여 DACL 변경도 하지 않습니다.
        // 실패/취소/프로세스종료 후에도 남기며 기존 소비 기록은 삭제/초기화하지 않습니다.
        consumption=AdmissionConsumption.Acquire(Native.EvidenceRoot,Encoding.UTF8.GetBytes(Contract.TrialId+"\n"+contractSha+"\n"),AdmissionWindows.CreateRecord);
        ValidateEntry();
        Security.Harden(process,AdmissionWindows.KernelKind.Process);Security.Harden(thread,AdmissionWindows.KernelKind.Thread);
        using(var tokenSecurity=new AdmissionWindows.KernelSecurity(Native.Worker,queryToken:true))tokenSecurity.Harden(token,AdmissionWindows.KernelKind.Token);
        foreach(var handle in watched.Keys)NonInherited(handle);
        HandleCensus.Check(watched);
        ValidateEntry();
        if(Contract.FakeOnlyIsolation is not null)
        {
            using var evidence=AdmissionWindows.CreateRecord(Path.Combine(Native.EvidenceRoot,Contract.TrialId+".exclusivity"));
            evidence.Write(Encoding.UTF8.GetBytes("FAKE_ONLY_EXECUTION_V1\nGLOBAL_SID_ABSENCE_UNKNOWN\n"));((FileStream)evidence).Flush(true);
        }
    }
    private void PinDirectoryTree(string path,bool evidence)
    {
        var chain=new Stack<string>();for(DirectoryInfo? d=new(path);d is not null;d=d.Parent)chain.Push(d.FullName);
        while(chain.Count>0)
        {
            string directory=chain.Pop();bool leaf=chain.Count==0;
            var file=AdmissionWindows.Pin(directory,true);pins.Add(file);
            byte[] sd=AdmissionWindows.Descriptor(file.DangerousGetHandle(),1);
            AdmissionPolicy.Descriptor(new RawSecurityDescriptor(sd,0),Native.Worker,leaf&&evidence,!leaf);
            uint mutation=AdmissionPolicy.OwnerMutation|0x40U;
            if(leaf)mutation|=evidence?0x114U:0x116U;
            for(uint bit=1;bit!=0;bit<<=1)
                if((mutation&bit)!=0)AdmissionPolicy.Require(!AdmissionWindows.Allowed(sd,token,bit),"EFFECTIVE_DIRECTORY_MUTATION");
            if(leaf&&evidence)AdmissionPolicy.Require(AdmissionWindows.Allowed(sd,token,2),"EVIDENCE_CREATE_DENIED");
        }
    }
    private SafeFileHandle PinFile(string path)
    {
        AdmissionPolicy.Require(string.Equals(Path.GetDirectoryName(path),Native.CodeRoot,StringComparison.OrdinalIgnoreCase),"CODE_PATH_ESCAPE");
        var file=AdmissionWindows.Pin(path,false);pins.Add(file);
        byte[] sd=AdmissionWindows.Descriptor(file.DangerousGetHandle(),1);
        AdmissionPolicy.Descriptor(new RawSecurityDescriptor(sd,0),Native.Worker,false);
        foreach(uint access in new uint[]{2,4,0x10,0x100,AdmissionPolicy.Delete,AdmissionPolicy.WriteDac,AdmissionPolicy.WriteOwner})
            AdmissionPolicy.Require(!AdmissionWindows.Allowed(sd,token,access),"EFFECTIVE_FILE_MUTATION");
        AdmissionPolicy.Require(AdmissionWindows.Allowed(sd,token,1),"FILE_READ_DENIED");return file;
    }
    internal void BeforeResume(nint job,Native.ProcessInfo child)
    {
        // 승인 파일만 읽고 관리자 시작 순서를 건너뛰는 수동 실행은 허용하지 않습니다.
        // 응답 기록이 아직 없거나 유실됐으면 suspended Probe를 재개하지 않고 HOLD합니다.
        ReceiptPolicy.Instance(ControlRecord("instance"),startIntent);
        NativeTrialPolicy.Authorize(Contract,expectedContractSha,"--supervise",startOperation,AccountNative.Read(DateTimeOffset.UtcNow,false),DateTimeOffset.UtcNow,AdmissionBinding.Current);
        Security.Harden(child.Process,AdmissionWindows.KernelKind.Process);Security.Harden(child.Thread,AdmissionWindows.KernelKind.Thread);
        AdmissionPolicy.Require(childToken==0,"CHILD_TOKEN_ALREADY_BOUND");childToken=AdmissionWindows.Token(child.Process);
        using(var tokenSecurity=new AdmissionWindows.KernelSecurity(Native.Worker,queryToken:true))tokenSecurity.Harden(childToken,AdmissionWindows.KernelKind.Token);
        AdmissionWindows.LimitedToken(childToken);
        watched.Add(job,AdmissionPolicy.JobDanger);watched.Add(child.Process,AdmissionPolicy.ProcessDanger);watched.Add(child.Thread,AdmissionPolicy.ThreadDanger);watched.Add(childToken,AdmissionPolicy.TokenDanger);
        foreach(var handle in watched.Keys)NonInherited(handle);
        CheckIsolation(child.Pid);
        // 파일/디렉터리 HANDLE은 계속 열려 있어 swap/rename/write 공유를 차단합니다.
        AdmissionPolicy.Require(Contract.ExpiresUtc>DateTimeOffset.UtcNow,"APPROVAL_EXPIRED_BEFORE_RESUME");
    }
    private ControlReceipt ControlRecord(string suffix)
    {
        var file=AdmissionWindows.Pin(Path.Combine(Native.EvidenceRoot,Contract.TrialId+".admin."+suffix),false);pins.Add(file);
        byte[] sd=AdmissionWindows.Descriptor(file.DangerousGetHandle(),1);
        AdmissionPolicy.Descriptor(new RawSecurityDescriptor(sd,0),Native.Worker,false);
        foreach(uint access in new uint[]{2,4,0x10,0x100,AdmissionPolicy.Delete,AdmissionPolicy.WriteDac,AdmissionPolicy.WriteOwner})
            AdmissionPolicy.Require(!AdmissionWindows.Allowed(sd,token,access),"ADMIN_RECEIPT_WORKER_MUTATION");
        return ControlReceipt.Parse(ReadSmall(file,4096));
    }
    internal void CheckIsolation(uint childPid)
    {CheckScope(childPid);HandleCensus.Check(watched);}
    private void CheckScope(uint? childPid)=>FakeOnlyPolicy.Exclusivity(Contract,DateTimeOffset.UtcNow,
        ReadTrust,()=>HandleCensus.Exclusive(childPid));
    private byte[] ReadTrust(string name,int limit)
    {
        // 최종 경로를 검증한 공유읽기 HANDLE을 수명 내내 보유하고, 주기 검사에서 HANDLE을 늘리지 않습니다.
        if(!trustInputs.TryGetValue(name,out var bytes)){bytes=ReadSmall(PinFile(Path.Combine(Native.CodeRoot,name)),limit);trustInputs.Add(name,bytes);}
        AdmissionPolicy.Require(bytes.Length<=limit,"FAKE_TRUST_SIZE");return bytes;
    }
    private static void NonInherited(nint handle)
    {Native.Check(Native.SetHandleInformation(handle,1,0));Native.Check(Native.GetHandleInformation(handle,out uint flags));AdmissionPolicy.Require((flags&1)==0,"INHERITABLE_ADMISSION_HANDLE");}
    private static byte[] ReadSmall(SafeFileHandle file,int limit)
    {long size=RandomAccess.GetLength(file);AdmissionPolicy.Require(size>0&&size<=limit,"FILE_SIZE");byte[] raw=new byte[size];int read=RandomAccess.Read(file,raw,0);AdmissionPolicy.Require(read==raw.Length,"SHORT_READ");return raw;}
    private static string Hash(byte[] bytes)=>Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();
    private static string Hash(SafeFileHandle file)
    {
        long length=RandomAccess.GetLength(file);AdmissionPolicy.Require(length is >0 and <=268435456,"IMAGE_SIZE");
        using var hash=IncrementalHash.CreateHash(HashAlgorithmName.SHA256);byte[] buffer=new byte[65536];long offset=0;
        while(offset<length){int count=RandomAccess.Read(file,buffer.AsSpan(0,(int)Math.Min(buffer.Length,length-offset)),offset);AdmissionPolicy.Require(count>0,"IMAGE_SHORT_READ");hash.AppendData(buffer,0,count);offset+=count;}
        return Convert.ToHexString(hash.GetHashAndReset()).ToLowerInvariant();
    }
    public void Dispose()
    {
        consumption?.Dispose();consumption=null;
        for(int i=pins.Count-1;i>=0;i--)pins[i].Dispose();pins.Clear();
        if(childToken!=0){Native.CloseHandle(childToken);childToken=0;}if(thread!=0){Native.CloseHandle(thread);thread=0;}if(process!=0){Native.CloseHandle(process);process=0;}if(token!=0){Native.CloseHandle(token);token=0;}
        Security.Dispose();
    }
}

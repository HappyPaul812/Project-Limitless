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
    private nint token,process,thread;
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
        AdmissionWindows.Check(AdmissionWindows.DuplicateHandle(Native.GetCurrentProcess(),Native.GetCurrentProcess(),Native.GetCurrentProcess(),out process,0,false,2));
        AdmissionWindows.Check(AdmissionWindows.DuplicateHandle(Native.GetCurrentProcess(),Native.GetCurrentThread(),Native.GetCurrentProcess(),out thread,0,false,2));
        Security.Harden(process);Security.Harden(thread);
        using(var tokenSecurity=new AdmissionWindows.KernelSecurity(Native.Worker,queryToken:true))tokenSecurity.Harden(token);
        watched.Add(process,0x000d0a7b);watched.Add(thread,0x000d03fb);watched.Add(token,0x000d01ef);
        foreach(var handle in watched.Keys)NonInherited(handle);

        PinDirectoryTree(Native.CodeRoot,false);PinDirectoryTree(Native.EvidenceRoot,true);
        var contract=PinFile(contractPath);byte[] raw=ReadSmall(contract,4096);
        AdmissionPolicy.Require(Hash(raw)==contractSha,"CONTRACT_SHA");
        AdmissionPolicy.Contract(raw,DateTimeOffset.UtcNow);
        Contract=System.Text.Json.JsonSerializer.Deserialize(raw,ContractJson.Default.TrialContract) ?? throw new InvalidOperationException("CONTRACT_NULL");
        var supervisor=PinFile(Path.Combine(Native.CodeRoot,"Supervisor.exe"));
        var probe=PinFile(Path.Combine(Native.CodeRoot,"Probe.exe"));
        AdmissionPolicy.Require(Hash(supervisor)==Contract.SupervisorSha256 && Hash(probe)==Contract.ProbeSha256,"IMAGE_SHA");
        HandleCensus.Exclusive(null);HandleCensus.Check(watched);
        // 검증을 모두 마친 뒤에만 영구 marker를 원자 생성하고 flush합니다.
        // 실패/취소/프로세스종료 후에도 남기며 기존 소비 기록은 건드리지 않습니다.
        consumption=AdmissionConsumption.Acquire(Native.EvidenceRoot,Encoding.UTF8.GetBytes(Contract.TrialId+"\n"+contractSha+"\n"),AdmissionWindows.CreateRecord);
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
        Security.Harden(child.Process);Security.Harden(child.Thread);
        using var childToken=new TokenLease(AdmissionWindows.Token(child.Process));
        using(var tokenSecurity=new AdmissionWindows.KernelSecurity(Native.Worker,queryToken:true))tokenSecurity.Harden(childToken.Handle);
        AdmissionWindows.LimitedToken(childToken.Handle);
        var current=new Dictionary<nint,uint>(watched) { [job]=0x000d001f,[child.Process]=0x000d0a7b,[child.Thread]=0x000d03fb,[childToken.Handle]=0x000d01ef };
        foreach(var handle in current.Keys)NonInherited(handle);
        HandleCensus.Exclusive(child.Pid);HandleCensus.Check(current);
        // 파일/디렉터리 HANDLE은 계속 열려 있어 swap/rename/write 공유를 차단합니다.
        AdmissionPolicy.Require(Contract.ExpiresUtc>DateTimeOffset.UtcNow,"APPROVAL_EXPIRED_BEFORE_RESUME");
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
    private sealed class TokenLease(nint handle):IDisposable {internal nint Handle=handle;public void Dispose(){if(Handle!=0){Native.CloseHandle(Handle);Handle=0;}}}
    public void Dispose()
    {
        consumption?.Dispose();consumption=null;
        for(int i=pins.Count-1;i>=0;i--)pins[i].Dispose();pins.Clear();
        if(thread!=0){Native.CloseHandle(thread);thread=0;}if(process!=0){Native.CloseHandle(process);process=0;}if(token!=0){Native.CloseHandle(token);token=0;}
        Security.Dispose();
    }
}

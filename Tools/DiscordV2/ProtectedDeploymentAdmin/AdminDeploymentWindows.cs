using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using Microsoft.Win32.SafeHandles;

// 기존 Windows 후보를 독립 도구에 복제·분리했습니다. 원본 Supervisor 소스는 변경하지 않습니다.
// 테스트는 이 객체를 만들거나 Windows 쓰기 API를 호출하지 않고 모의 backend를 사용합니다.
internal sealed class AdminDeploymentWindows : IDeploymentHost,IDisposable
{
    private readonly string requestPath,requestSha;
    private readonly Dictionary<string,byte[]> protectedInputs=new(StringComparer.OrdinalIgnoreCase);
    private readonly HashSet<string> ancestors=new(StringComparer.OrdinalIgnoreCase);
    private DeploymentWriteBoundary? boundary;
    private readonly List<SafeFileHandle> pins=[];
    private readonly Dictionary<string,SafeFileHandle> sources=new(StringComparer.Ordinal);
    public DateTimeOffset Now=>DateTimeOffset.UtcNow;
    internal AdminDeploymentWindows(string path,string expectedSha)
    { requestPath=Path.GetFullPath(path);requestSha=expectedSha; }
    public void BindBoundary(DeploymentWriteBoundary value)
    { AdmissionPolicy.Require(boundary is null,"DEPLOY_BOUNDARY_ALREADY_BOUND");boundary=value; }
    private static void EnsureAdmin()
    {
        Native.NoImpersonation();using var user=WindowsIdentity.GetCurrent();
        AdmissionPolicy.Require(new WindowsPrincipal(user).IsInRole(WindowsBuiltInRole.Administrator),"DEPLOY_ADMIN_REQUIRED");
        Native.Check(Native.OpenProcessToken(Native.GetCurrentProcess(),8,out nint token));
        try{AdmissionPolicy.Require(Native.Info<int>(token,20)==1,"DEPLOY_ELEVATION_REQUIRED");}finally{Native.CloseHandle(token);}
    }
    private void Boundary(DeploymentStep step)
    { EnsureAdmin();(boundary??throw new InvalidOperationException("DEPLOY_BOUNDARY_REQUIRED")).Check(step); }
    public byte[] ReadProtected(string path,int limit)
    {
        EnsureAdmin();path=Path.GetFullPath(path);
        if(protectedInputs.TryGetValue(path,out var cached)){AdmissionPolicy.Require(cached.Length<=limit,"DEPLOY_INPUT_SIZE");return (byte[])cached.Clone();}
        for(string? p=Path.GetDirectoryName(path);p is not null;p=Path.GetDirectoryName(p))PinAncestor(p);
        var file=AdmissionWindows.Pin(path,false);pins.Add(file);
        AdmissionPolicy.Descriptor(new RawSecurityDescriptor(AdmissionWindows.Descriptor(file.DangerousGetHandle(),1),0),Native.Worker,false);
        long size=RandomAccess.GetLength(file);AdmissionPolicy.Require(size>0&&size<=limit,"DEPLOY_INPUT_SIZE");
        byte[] raw=new byte[size];AdmissionPolicy.Require(RandomAccess.Read(file,raw,0)==raw.Length,"DEPLOY_INPUT_SHORT");
        if(path.Equals(requestPath,StringComparison.OrdinalIgnoreCase))AdmissionPolicy.Require(ProtectedDeployment.Hash(raw)==requestSha,"DEPLOY_REQUEST_SHA");
        protectedInputs.Add(path,raw);return (byte[])raw.Clone();
    }
    public DeploymentActor ObserveActor()
    {
        EnsureAdmin();string path=Environment.ProcessPath??throw new InvalidOperationException("DEPLOY_TOOL_PATH");
        // 자기 실행 파일과 모든 부모도 보호된 경로여야 합니다. 작업자 쓰기 가능 폴더의 도구는 거부합니다.
        AdmissionPolicy.Require(Path.GetFileName(path)=="Limitless.DeploymentAdmin.exe","DEPLOY_TOOL_NATIVE_REQUIRED");
        byte[] raw=ReadProtected(path,268435456);DeploymentSecurityPolicy.NativeImage(raw);
        return new(true,true,ProtectedDeployment.Hash(raw));
    }
    [DllImport("kernel32.dll",CharSet=CharSet.Unicode,SetLastError=true)]
    [return:MarshalAs(UnmanagedType.Bool)] private static extern bool CreateDirectoryW(string path,ref AdmissionWindows.Attributes attributes);
    [DllImport("kernel32.dll",CharSet=CharSet.Unicode,SetLastError=true)]
    private static extern SafeFileHandle CreateFileW(string path,uint access,uint share,ref AdmissionWindows.Attributes attributes,uint disposition,uint flags,nint template);
    [DllImport("advapi32.dll",CharSet=CharSet.Unicode,SetLastError=true)]
    [return:MarshalAs(UnmanagedType.Bool)] private static extern bool ConvertStringSecurityDescriptorToSecurityDescriptorW(string text,uint revision,out nint pointer,out uint length);
    [DllImport("kernel32.dll")] private static extern nint LocalFree(nint pointer);
    private sealed class Security : IDisposable
    {
        internal nint Pointer;
        internal string Text;
        internal Security(string mask)
        {
            Text=DeploymentSecurityPolicy.Sddl(mask,Native.Worker);
            AdmissionWindows.Check(ConvertStringSecurityDescriptorToSecurityDescriptorW(Text,1,out Pointer,out _));
        }
        internal AdmissionWindows.Attributes Attributes=>new(){Size=(uint)Marshal.SizeOf<AdmissionWindows.Attributes>(),Descriptor=Pointer,Inherit=0};
        public void Dispose(){if(Pointer!=0){LocalFree(Pointer);Pointer=0;}}
    }
    public byte[] ReadAdministratorApproval()=>throw new InvalidOperationException("USE_DEPLOYMENT_CONTROLLER");
    private static bool Exists(string path,bool directory)
    {
        try{using var file=AdmissionWindows.Pin(path,directory);return true;}
        catch(Win32Exception e) when(e.NativeErrorCode is 2 or 3){return false;}
        // 잘못된 파일 종류/접근 거부/공유 충돌은 부재가 아니므로 그대로 HOLD합니다.
    }
    public bool DirectoryExists(string path)=>Exists(path,true);
    public bool FileExists(string path)=>Exists(path,false);
    public void PinAncestor(string path)
    {
        if(!ancestors.Add(Path.GetFullPath(path)))return;
        var file=AdmissionWindows.Pin(path,true);pins.Add(file);
        AdmissionPolicy.Descriptor(new RawSecurityDescriptor(AdmissionWindows.Descriptor(file.DangerousGetHandle(),1),0),Native.Worker,false,true);
    }
    public void PinSource(DeploymentFile source)
    {
        var file=AdmissionWindows.Pin(source.Source,false);pins.Add(file);
        AdmissionPolicy.Require(RandomAccess.GetLength(file)==source.Bytes,"DEPLOY_SOURCE_SIZE");
        string hash=Hash(file);
        AdmissionPolicy.Require(hash==source.Sha256,"DEPLOY_SOURCE_SHA");
        // 원본 read 핸들은 전체 적용 동안 유지하여 write/delete 공유를 허용하지 않습니다.
        sources.Add(source.Name,file);
    }
    public void ConsumeApproval(string id,string sha)
    {
        var step=new DeploymentStep(DeploymentStepKind.Consume,id,Sha:sha);Boundary(step);
        string path=Path.Combine(Path.GetDirectoryName(requestPath)!,id+".deploy-consumed");
        using var file=AdmissionWindows.CreateAdminRecord(path);
        Boundary(step);file.Write(Encoding.UTF8.GetBytes(id+"\n"+sha+"\n"));((FileStream)file).Flush(true);
    }
    public void CreateDirectoryNew(string path,bool evidence)
    {
        var step=new DeploymentStep(DeploymentStepKind.Directory,path,evidence);Boundary(step);
        using var security=new Security(evidence?"0x001200ab":"0x001200a9");var a=security.Attributes;
        Boundary(step);AdmissionWindows.Check(CreateDirectoryW(path,ref a)); // 이미 존재하면 오류, 채택/수정하지 않습니다.
        var handle=AdmissionWindows.Pin(path,true);pins.Add(handle);Verify(handle,security.Text,evidence);
    }
    public void CopyNew(string codeRoot,DeploymentFile source)
    {
        string path=Path.Combine(codeRoot,source.Name);
        var step=new DeploymentStep(DeploymentStepKind.Copy,path,File:source);Boundary(step);
        using var security=new Security(source.Name.EndsWith(".exe",StringComparison.Ordinal)?"0x001200a9":"0x00120089");var a=security.Attributes;
        Boundary(step);var handle=CreateFileW(path,0x120003,0,ref a,1,0x80200000,0);
        if(handle.IsInvalid){int error=Marshal.GetLastWin32Error();handle.Dispose();throw new Win32Exception(error);}
        using(handle)
        {
            Verify(handle,security.Text,false);
            using var target=new FileStream(handle,FileAccess.ReadWrite,65536,false);var input=sources[source.Name];
            byte[] buffer=new byte[65536];long offset=0;
            while(offset<source.Bytes){int n=RandomAccess.Read(input,buffer.AsSpan(0,(int)Math.Min(buffer.Length,source.Bytes-offset)),offset);AdmissionPolicy.Require(n>0,"DEPLOY_SOURCE_SHORT");Boundary(step);target.Write(buffer,0,n);offset+=n;}
            Boundary(step);target.Flush(true);target.Position=0;
            string hash=Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(target)).ToLowerInvariant();
            AdmissionPolicy.Require(target.Length==source.Bytes&&hash==source.Sha256,"DEPLOY_COPY_SHA");
        }
        var pinned=AdmissionWindows.Pin(path,false);pins.Add(pinned);Verify(pinned,security.Text,false);
        AdmissionPolicy.Require(RandomAccess.GetLength(pinned)==source.Bytes&&Hash(pinned)==source.Sha256,"DEPLOY_FINAL_SHA");
    }
    private static string Hash(SafeFileHandle file)
    {
        using var hash=System.Security.Cryptography.IncrementalHash.CreateHash(System.Security.Cryptography.HashAlgorithmName.SHA256);
        byte[] data=new byte[65536];long offset=0,length=RandomAccess.GetLength(file);
        while(offset<length){int n=RandomAccess.Read(file,data,offset);AdmissionPolicy.Require(n>0,"DEPLOY_SOURCE_SHORT");hash.AppendData(data,0,n);offset+=n;}
        return Convert.ToHexString(hash.GetHashAndReset()).ToLowerInvariant();
    }
    private static void Verify(SafeFileHandle handle,string sddl,bool evidence)
    {
        var actual=new RawSecurityDescriptor(AdmissionWindows.Descriptor(handle.DangerousGetHandle(),1),0);var expected=new RawSecurityDescriptor(sddl);
        AdmissionPolicy.Descriptor(actual,Native.Worker,evidence);
        DeploymentSecurityPolicy.Readback(actual,expected,Native.Worker,evidence);
        AdmissionWindows.Check(Native.GetHandleInformation(handle.DangerousGetHandle(),out uint flags));AdmissionPolicy.Require((flags&1)==0,"DEPLOY_INHERIT_HANDLE");
    }
    public void Dispose(){for(int i=pins.Count-1;i>=0;i--)pins[i].Dispose();pins.Clear();sources.Clear();protectedInputs.Clear();ancestors.Clear();}
}

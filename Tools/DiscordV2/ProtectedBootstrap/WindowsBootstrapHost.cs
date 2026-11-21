using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using Microsoft.Win32.SafeHandles;

// 디렉터리 생성 이외의 Windows 변경 API는 없습니다. 모의 테스트는 이 타입을 생성하지 않습니다.
internal sealed class WindowsBootstrapHost:IBootstrapHost
{
    private readonly Dictionary<string,SafeFileHandle> pins=new(StringComparer.OrdinalIgnoreCase);
    private BootstrapWriteGate? gate;
    public DateTimeOffset Now=>DateTimeOffset.UtcNow;
    public bool FixedNtfsVolume {get {var drive=new DriveInfo(@"C:\");return drive.DriveType==DriveType.Fixed&&drive.DriveFormat=="NTFS";}}
    [StructLayout(LayoutKind.Sequential)] private struct Attributes {internal uint Size;internal nint Descriptor;internal int Inherit;}
    [StructLayout(LayoutKind.Sequential)] private struct FileId {internal ulong Volume,Low,High;}
    [StructLayout(LayoutKind.Sequential)] private struct FileAttributes {internal uint Attributes,Tag;}
    [DllImport("kernel32.dll",CharSet=CharSet.Unicode,SetLastError=true)] private static extern bool CreateDirectoryW(string path,ref Attributes security);
    [DllImport("kernel32.dll",CharSet=CharSet.Unicode,SetLastError=true)] private static extern SafeFileHandle CreateFileW(string path,uint access,uint share,nint security,uint disposition,uint flags,nint template);
    [DllImport("kernel32.dll",SetLastError=true)] private static extern bool GetFileInformationByHandleEx(SafeFileHandle handle,int kind,out FileAttributes info,uint size);
    [DllImport("kernel32.dll",EntryPoint="GetFileInformationByHandleEx",SetLastError=true)] private static extern bool GetFileId(SafeFileHandle handle,int kind,out FileId info,uint size);
    [DllImport("kernel32.dll",CharSet=CharSet.Unicode,SetLastError=true)] private static extern uint GetFinalPathNameByHandleW(SafeFileHandle handle,StringBuilder path,uint length,uint flags);
    [DllImport("kernel32.dll",SetLastError=true)] private static extern bool GetHandleInformation(SafeFileHandle handle,out uint flags);
    [DllImport("advapi32.dll")] private static extern uint GetSecurityInfo(SafeFileHandle handle,int kind,uint flags,out nint owner,out nint group,out nint dacl,out nint sacl,out nint descriptor);
    [DllImport("advapi32.dll")] private static extern uint GetSecurityDescriptorLength(nint descriptor);
    [DllImport("advapi32.dll",CharSet=CharSet.Unicode,SetLastError=true)] private static extern bool ConvertStringSecurityDescriptorToSecurityDescriptorW(string text,uint revision,out nint descriptor,out uint size);
    [DllImport("kernel32.dll")] private static extern nint LocalFree(nint pointer);
    [DllImport("kernel32.dll")] private static extern nint GetCurrentProcess();
    [DllImport("kernel32.dll")] private static extern nint GetCurrentThread();
    [DllImport("kernel32.dll")] private static extern bool CloseHandle(nint handle);
    [DllImport("advapi32.dll",SetLastError=true)] private static extern bool OpenThreadToken(nint thread,uint access,bool asSelf,out nint token);
    [DllImport("advapi32.dll",SetLastError=true)] private static extern bool OpenProcessToken(nint process,uint access,out nint token);
    [DllImport("advapi32.dll",SetLastError=true)] private static extern bool GetTokenInformation(nint token,int kind,out int value,uint size,out uint needed);
    private static void Check(bool ok){if(!ok)throw new Win32Exception(Marshal.GetLastWin32Error());}
    internal static string Administrator()
    {
        if(OpenThreadToken(GetCurrentThread(),8,true,out nint threadToken)){CloseHandle(threadToken);throw new InvalidOperationException("BOOTSTRAP_IMPERSONATION");}
        BootstrapPolicy.Need(Marshal.GetLastWin32Error()==1008,"BOOTSTRAP_THREAD_TOKEN_UNKNOWN");
        using var identity=WindowsIdentity.GetCurrent();var principal=new WindowsPrincipal(identity);
        BootstrapPolicy.Need(principal.IsInRole(WindowsBuiltInRole.Administrator),"BOOTSTRAP_ADMIN_REQUIRED");
        Check(OpenProcessToken(GetCurrentProcess(),8,out nint token));
        try{Check(GetTokenInformation(token,20,out int elevated,4,out _));BootstrapPolicy.Need(elevated==1,"BOOTSTRAP_ELEVATED_REQUIRED");}
        finally{CloseHandle(token);}
        return identity.User?.Value??throw new InvalidOperationException("BOOTSTRAP_ADMIN_SID");
    }
    private static string Canonical(string path)
    {
        BootstrapPolicy.Need(Path.IsPathFullyQualified(path)&&!path.StartsWith(@"\\",StringComparison.Ordinal)
            &&Path.GetFullPath(path).Equals(path,StringComparison.OrdinalIgnoreCase),"BOOTSTRAP_CANONICAL_PATH");return path;
    }
    private static SafeFileHandle Open(string path,bool file)
    {
        var handle=CreateFileW(Canonical(path),file?0x120089U:0x120080U,1,0,3,0x02200000,0);
        if(handle.IsInvalid){int error=Marshal.GetLastWin32Error();handle.Dispose();throw new Win32Exception(error);}return handle;
    }
    private static byte[] Security(SafeFileHandle handle)
    {
        uint error=GetSecurityInfo(handle,1,7,out _,out _,out _,out _,out nint sd);if(error!=0)throw new Win32Exception((int)error);
        try{uint size=GetSecurityDescriptorLength(sd);BootstrapPolicy.Need(size is >0 and <=65536,"BOOTSTRAP_SD_SIZE");byte[] raw=new byte[size];Marshal.Copy(sd,raw,0,(int)size);return raw;}
        finally{LocalFree(sd);}
    }
    internal static BootstrapNode Read(SafeFileHandle handle,string path,bool directory)
    {
        Check(GetFileInformationByHandleEx(handle,9,out var attributes,8));Check(GetFileId(handle,18,out var id,24));Check(GetHandleInformation(handle,out uint flags));
        var final=new StringBuilder(32768);uint length=GetFinalPathNameByHandleW(handle,final,(uint)final.Capacity,0);
        BootstrapPolicy.Need(length>0&&length<final.Capacity&&final.ToString().Equals(@"\\?\"+path,StringComparison.OrdinalIgnoreCase),"BOOTSTRAP_FINAL_PATH");
        bool isDirectory=(attributes.Attributes&0x10)!=0;BootstrapPolicy.Need(isDirectory==directory,"BOOTSTRAP_FILE_KIND");
        var node=new BootstrapNode(path,$"{id.Volume:x16}:{id.High:x16}{id.Low:x16}",id.Volume,isDirectory,(attributes.Attributes&0x400)!=0,flags,Security(handle));
        BootstrapPolicy.Need(!node.Reparse&&(flags&1)==0,"BOOTSTRAP_REPARSE_OR_INHERIT");return node;
    }
    public BootstrapNode PinAndRead(string path)
    {
        if(!pins.TryGetValue(path,out var handle)){handle=Open(path,false);pins.Add(path,handle);}return Read(handle,path,true);
    }
    public bool Exists(string path)
    {
        try{using var handle=Open(path,false);return true;}
        catch(Win32Exception e) when(e.NativeErrorCode is 2 or 3){return false;}
    }
    internal SafeFileHandle ProtectedFile(string path)
    {
        Canonical(path);BootstrapPolicy.Need(!BootstrapPolicy.Targets.Any(t=>path.Equals(t,StringComparison.OrdinalIgnoreCase)
            ||path.StartsWith(t+Path.DirectorySeparatorChar,StringComparison.OrdinalIgnoreCase)),"BOOTSTRAP_INPUT_IN_NEW_TARGET");
        var chain=new Stack<string>();for(DirectoryInfo? p=new FileInfo(path).Directory;p is not null;p=p.Parent)chain.Push(p.FullName);
        foreach(string parent in chain)BootstrapPolicy.Ancestor(PinAndRead(parent),parent);
        if(!pins.TryGetValue(path,out var handle)){handle=Open(path,true);pins.Add(path,handle);}
        var file=Read(handle,path,false);BootstrapPolicy.AdministrativeSecurity(file.Security);return handle;
    }
    public byte[] ReadProtectedAuthorization(string path)
    {
        Administrator();var file=ProtectedFile(path);long size=RandomAccess.GetLength(file);BootstrapPolicy.Need(size is >0 and <=8192,"BOOTSTRAP_AUTH_SIZE");
        byte[] raw=new byte[size];BootstrapPolicy.Need(RandomAccess.Read(file,raw,0)==raw.Length,"BOOTSTRAP_AUTH_SHORT");return raw;
    }
    public BootstrapActor ObserveActor()
    {
        string user=Administrator(),path=Environment.ProcessPath??throw new InvalidOperationException("BOOTSTRAP_TOOL_PATH");
        BootstrapPolicy.Need(Path.GetFileName(path)=="Limitless.ProtectedBootstrap.exe","BOOTSTRAP_TOOL_NATIVE_PATH");
        var file=ProtectedFile(path);long size=RandomAccess.GetLength(file);BootstrapPolicy.Need(size is >0 and <=268435456,"BOOTSTRAP_TOOL_SIZE");
        using var hash=System.Security.Cryptography.IncrementalHash.CreateHash(System.Security.Cryptography.HashAlgorithmName.SHA256);
        byte[] buffer=new byte[65536];long offset=0;
        while(offset<size){int count=RandomAccess.Read(file,buffer.AsSpan(0,(int)Math.Min(buffer.Length,size-offset)),offset);BootstrapPolicy.Need(count>0,"BOOTSTRAP_TOOL_SHORT");hash.AppendData(buffer,0,count);offset+=count;}
        BootstrapPolicy.Need(Administrator()==user,"BOOTSTRAP_ADMIN_CHANGED_DURING_IMAGE_READ");
        return new(true,true,false,user,Convert.ToHexString(hash.GetHashAndReset()).ToLowerInvariant());
    }
    public void Bind(BootstrapWriteGate value){BootstrapPolicy.Need(gate is null,"BOOTSTRAP_ALREADY_BOUND");value.RequireHost(this);gate=value;}
    public void CreateProtected(string path,string sddl)
    {
        var boundary=gate??throw new InvalidOperationException("BOOTSTRAP_GATE_REQUIRED");
        Check(ConvertStringSecurityDescriptorToSecurityDescriptorW(sddl,1,out nint pointer,out _));
        try {
            var attributes=new Attributes{Size=(uint)Marshal.SizeOf<Attributes>(),Descriptor=pointer,Inherit=0};
            boundary.BeforeNativeCreate(path,sddl);
            Check(CreateDirectoryW(path,ref attributes)); // 기존/경합은 오류입니다. 기존 항목 채택이나 ACL 수정은 하지 않습니다.
        }finally{LocalFree(pointer);}
    }
    public void Dispose(){foreach(var handle in pins.Values.Reverse())handle.Dispose();pins.Clear();}
}

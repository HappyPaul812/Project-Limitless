using System.ComponentModel;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
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
    public string LedgerRoot=>DeploymentLedgerBinding.Root;
    private SafeFileHandle? ledgerHandle;
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
    public void VerifyRequestSignature(byte[] raw)
    {
        byte[] signature=ReadProtected(requestPath+".sig",4096);
        using var signer=OpenApprovalSigner();
        AdmissionPolicy.Require(signer.VerifyData(raw,signature,HashAlgorithmName.SHA256,RSASignaturePadding.Pss),
            "DEPLOY_APPROVAL_SIGNATURE_HOLD");
    }

    public DateTimeOffset? VerifyCurrentProductContract(byte[] request,AdmissionBinding binding)
    {
        var scope=DeploymentAdmission.Strict(request,8192,DeploymentRequestJson.Default.DeploymentRequest);
        string path=Path.Combine(CurrentFakeOnlyContract.Approvals,"FakeOnlyProduct.json");
        byte[] contract=ReadProtected(path,4096);
        byte[] signature=ReadProtected(path+".sig",4096);
        byte[] manifest=ReadProtected(scope.ManifestPath,16384);
        AdmissionPolicy.Require(ProtectedDeployment.Hash(contract)==scope.ProductContractSha256 &&
            ProtectedDeployment.Hash(manifest)==scope.ManifestSha256,"FAKE_V2_REQUEST_BINDING");
        var plan=ProtectedDeployment.ReadManifest(manifest,scope.ManifestSha256);
        var images=DeploymentAdmission.ReviewSignedManifest(scope,plan,binding);
        byte[] approvalRaw=ReadProtected(scope.ApprovalPath,4096);
        var approval=DeploymentAdmission.Strict(approvalRaw,4096,DeploymentJson.Default.DeploymentApproval);
        AdmissionPolicy.Require(approval.Approved && approval.Profile == "PROTECTED_DEPLOYMENT_V1" &&
            approval.DeploymentId == scope.DeploymentId && approval.TrialId == scope.TrialId &&
            approval.ManifestSha256 == scope.ManifestSha256 &&
            ProtectedDeployment.Hash(approvalRaw) == scope.ApprovalSha256, "DEPLOY_REVIEW_APPROVAL_BINDING");
        using var signer=OpenApprovalSigner();
        var productExpiry = CurrentFakeOnlyContract.Validate(contract,signature,signer,CurrentFakeOnlyContract.Signer,binding,Now,
            images.Supervisor.Sha256,
            images.Probe.Sha256);
        foreach (var image in new[] { images.Supervisor, images.Probe })
        {
            byte[] bytes=ReadProtected(image.Source,268435456);
            DeploymentSecurityPolicy.NativeImage(bytes);
            PinSource(image);
            ProductAuthenticode.VerifyAuthenticode(sources[image.Name],image.Source,
                ProductAuthenticode.ApprovedCertificateSha,CurrentFakeOnlyContract.Signer);
        }
        byte[] trial=ReadProtected(scope.ContractPath,4096);
        byte[] risk=ReadProtected(scope.RiskBasisPath,16384);
        AdmissionPolicy.Require(ProtectedDeployment.Hash(trial) == scope.ContractSha256 &&
            risk.Length >= 32 && ProtectedDeployment.Hash(risk) == scope.RiskBasisSha256,"DEPLOY_REVIEW_INPUT_SHA");
        DeploymentLedgerPolicy.Root(scope.LedgerRoot,LedgerRoot,binding);
        PinLedger();
        var actor=ObserveActor();
        AdmissionPolicy.Require(actor.ElevatedAdministrator && actor.NotImpersonating &&
            actor.UserSid == scope.AdministratorSid && actor.ToolSha256 == scope.ToolSha256,"DEPLOY_REVIEW_ACTOR");
        var now=Now;
        DeploymentAdmission.RequireSignedPeriods(now, scope.ExpiresUtc, productExpiry);
        AdmissionPolicy.Require(now < approval.ExpiresUtc && now < plan.ExpiresUtc &&
            scope.ExpiresUtc <= approval.ExpiresUtc && approval.ExpiresUtc <= plan.ExpiresUtc,
            "DEPLOY_REVIEW_APPROVAL_EXPIRED");
        return productExpiry;
    }

    // 작업 승인은 배포와 별개의 서명·소비 기록을 사용합니다.
    internal void InstallOperationSidecars(string mode, AdmissionBinding binding)
    {
        EnsureAdmin();
        AdmissionPolicy.Require(AdminController.IsMode(mode),"SIDECAR_OPERATION");
        _=ObserveActor();
        PinLedger();
        var trialPath=Path.Combine(binding.CodeRoot,"TrialContract.json");
        var trial=ReadProtected(trialPath,4096);
        string trialSha=ProtectedDeployment.Hash(trial);
        var contract=System.Text.Json.JsonSerializer.Deserialize(trial,ContractJson.Default.TrialContract)
            ?? throw new InvalidOperationException("SIDECAR_TRIAL_NULL");
        AdmissionPolicy.Require(contract.TrialId==NativeTrialPolicy.CandidateTrialId,"SIDECAR_TRIAL_ID");

        string supervisorPath=Path.Combine(binding.CodeRoot,"Supervisor.exe");
        string probePath=Path.Combine(binding.CodeRoot,"Probe.exe");
        foreach (var image in new[] { (supervisorPath,contract.SupervisorSha256),(probePath,contract.ProbeSha256) })
        {
            byte[] bytes=ReadProtected(image.Item1,268435456);
            AdmissionPolicy.Require(ProtectedDeployment.Hash(bytes)==image.Item2,"SIDECAR_IMAGE_SHA");
            DeploymentSecurityPolicy.NativeImage(bytes);
            var handle=AdmissionWindows.Pin(image.Item1,false);pins.Add(handle);
            ProductAuthenticode.VerifyAuthenticode(handle,image.Item1,
                ProductAuthenticode.ApprovedCertificateSha,CurrentFakeOnlyContract.Signer);
        }

        string name=mode=="--start-once"?"FakeOnlyExecution.v2":"FakeOnlyExecution.v2."+mode[2..];
        string basePath=Path.Combine(CurrentFakeOnlyContract.Approvals,"PendingOperations");
        var names=new[] { OperationApproval.FileName(contract.TrialId,mode),name+".json",name+".sig" };
        var files=new List<DeploymentFile>();
        var bytesByName=new List<byte[]>();
        foreach (string fileName in names)
        {
            string source=Path.Combine(basePath,fileName);
            byte[] bytes=ReadProtected(source,4096);
            var file=new DeploymentFile { Name=fileName,Source=source,Bytes=bytes.Length,Sha256=ProtectedDeployment.Hash(bytes) };
            AdmissionPolicy.Require(!FileExists(Path.Combine(binding.CodeRoot,fileName)),"SIDECAR_TARGET_EXISTS");
            PinSource(file);
            files.Add(file);
            bytesByName.Add(bytes);
        }
        using var signer=OpenApprovalSigner();
        var window=CurrentFakeOnlyContract.ExecutionWindow.Verify(bytesByName[1],bytesByName[2],signer,
            CurrentFakeOnlyContract.Signer,binding,Now,contract.SupervisorSha256,contract.ProbeSha256,trialSha,files[0].Sha256);
        OperationApproval CheckApproval()
        {
            var now=Now;
            AdmissionPolicy.Contract(trial,now,binding,window:window);
            NativeTrialPolicy.Shape(contract,mode,now,window);
            var approval=OperationApproval.Validate(bytesByName[0],contract.TrialId,trialSha,mode,now,false,window);
            AdmissionPolicy.Require(approval.ExpiresUtc<=contract.ExpiresUtc,"SIDECAR_OPERATION_TRIAL_EXPIRY");
            return approval;
        }
        var operation=CheckApproval();
        var steps=new List<DeploymentStep> { new(DeploymentStepKind.Consume,operation.ApprovalId,Sha:files[1].Sha256) };
        steps.AddRange(files.Select(file=>new DeploymentStep(DeploymentStepKind.Copy,Path.Combine(binding.CodeRoot,file.Name),File:file)));
        var guard=new DeploymentWriteBoundary(()=>{ _=CheckApproval(); },steps);
        BindBoundary(guard);
        foreach (var step in steps)
        {
            guard.Begin(step);
            bool success=false;
            try
            {
                if(step.Kind==DeploymentStepKind.Consume)ConsumeApproval(step.Target,step.Sha);
                else CopyNew(binding.CodeRoot,step.File!);
                success=true;
            }
            finally { guard.End(success); }
        }
    }

    private RSA OpenApprovalSigner()
    {
        using var store=new X509Store(StoreName.Root,StoreLocation.LocalMachine);
        store.Open(OpenFlags.ReadOnly|OpenFlags.OpenExistingOnly);
        var certificates=store.Certificates;
        try
        {
            var cert=certificates.Cast<X509Certificate2>().SingleOrDefault(candidate =>
                ProtectedDeployment.Hash(candidate.RawData)=="7dba5310d6ded0035595e5e7ab8b8b73422f2925b49c633426995c322940672c")
                ?? throw new InvalidOperationException("DEPLOY_APPROVAL_CERTIFICATE_HOLD");
            var now=Now.UtcDateTime;
            AdmissionPolicy.Require(cert.NotBefore.ToUniversalTime()<=now && now<cert.NotAfter.ToUniversalTime() &&
                cert.Extensions.OfType<X509EnhancedKeyUsageExtension>().Any(eku =>
                    eku.EnhancedKeyUsages.Cast<Oid>().Any(oid => oid.Value=="1.3.6.1.5.5.7.3.3")),
                "DEPLOY_APPROVAL_CERTIFICATE_POLICY_HOLD");
            using var signer=cert.GetRSAPublicKey() ?? throw new InvalidOperationException("DEPLOY_APPROVAL_KEY_HOLD");
            AdmissionPolicy.Require(ProtectedDeployment.Hash(signer.ExportSubjectPublicKeyInfo())==CurrentFakeOnlyContract.Signer,
                "DEPLOY_APPROVAL_KEY_PIN_HOLD");
            return RSA.Create(signer.ExportParameters(false));
        }
        finally { foreach(var cert in certificates)cert.Dispose(); }
    }

    public DeploymentActor ObserveActor()
    {
        EnsureAdmin();string path=Environment.ProcessPath??throw new InvalidOperationException("DEPLOY_TOOL_PATH");
        // 자기 실행 파일과 모든 부모도 보호된 경로여야 합니다. 작업자 쓰기 가능 폴더의 도구는 거부합니다.
        AdmissionPolicy.Require(path.Equals(CurrentFakeOnlyContract.Tool,StringComparison.OrdinalIgnoreCase),"DEPLOY_TOOL_NATIVE_REQUIRED");
        byte[] raw=ReadProtected(path,268435456);DeploymentSecurityPolicy.NativeImage(raw);
        using var actor=WindowsIdentity.GetCurrent();
        return new(true,true,ProtectedDeployment.Hash(raw),actor.User?.Value??"");
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
        internal Security(string mask,bool ledger=false)
        {
            Text=ledger?DeploymentLedgerPolicy.Sddl:DeploymentSecurityPolicy.Sddl(mask,Native.Worker);
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
        if (sources.TryGetValue(source.Name,out var existing))
        {
            AdmissionPolicy.Require(RandomAccess.GetLength(existing)==source.Bytes && Hash(existing)==source.Sha256,
                "DEPLOY_SOURCE_CHANGED");
            return;
        }
        var file=AdmissionWindows.Pin(source.Source,false);pins.Add(file);
        AdmissionPolicy.Require(RandomAccess.GetLength(file)==source.Bytes,"DEPLOY_SOURCE_SIZE");
        string hash=Hash(file);
        AdmissionPolicy.Require(hash==source.Sha256,"DEPLOY_SOURCE_SHA");
        // 원본 read 핸들은 전체 적용 동안 유지하여 write/delete 공유를 허용하지 않습니다.
        sources.Add(source.Name,file);
    }
    public void PinLedger()
    {
        EnsureAdmin();DeploymentLedgerPolicy.Root(LedgerRoot,DeploymentLedgerBinding.Root,AdmissionBinding.Current);
        for(string? p=Path.GetDirectoryName(LedgerRoot);p is not null;p=Path.GetDirectoryName(p))PinAncestor(p);
        if(ledgerHandle is null){ledgerHandle=AdmissionWindows.Pin(LedgerRoot,true);pins.Add(ledgerHandle);}
        DeploymentLedgerPolicy.Security(new RawSecurityDescriptor(AdmissionWindows.Descriptor(ledgerHandle.DangerousGetHandle(),1),0));
    }
    [DllImport("kernel32.dll",CharSet=CharSet.Unicode,SetLastError=true)]
    private static extern uint GetFinalPathNameByHandleW(SafeFileHandle handle,StringBuilder path,uint length,uint flags);
    private sealed class LedgerIo(AdminDeploymentWindows owner,DeploymentStep step):IDeploymentLedgerIo
    {
        public void PinAndValidate()=>owner.PinLedger();
        public Stream CreateNew(string path)
        {
            AdmissionPolicy.Require(path==DeploymentLedgerPolicy.RecordPath(owner.LedgerRoot,step.Target),"DEPLOY_LEDGER_REDIRECT_HOLD");
            using var security=new Security("",ledger:true);var a=security.Attributes;
            owner.Boundary(step);
            var handle=CreateFileW(path,0x120003,0,ref a,1,0x80200000,0);
            if(handle.IsInvalid){int error=Marshal.GetLastWin32Error();handle.Dispose();throw new Win32Exception(error);}
            try {
                DeploymentLedgerPolicy.Security(new RawSecurityDescriptor(AdmissionWindows.Descriptor(handle.DangerousGetHandle(),1),0));
                Native.Check(Native.GetHandleInformation(handle.DangerousGetHandle(),out uint flags));AdmissionPolicy.Require((flags&1)==0,"DEPLOY_LEDGER_INHERIT_HANDLE");
                var final=new StringBuilder(32768);uint length=GetFinalPathNameByHandleW(handle,final,(uint)final.Capacity,0);
                AdmissionPolicy.Require(length>0&&length<final.Capacity&&string.Equals(final.ToString(),@"\\?\"+path,StringComparison.OrdinalIgnoreCase),"DEPLOY_LEDGER_FINAL_PATH");
                return new FileStream(handle,FileAccess.ReadWrite);
            } catch {handle.Dispose();throw;} // 생성된 기록은 삭제하지 않으며 불확실한 소비를 재사용하지 않습니다.
        }
    }
    public void ConsumeApproval(string id,string sha)
    {
        var step=new DeploymentStep(DeploymentStepKind.Consume,id,Sha:sha);Boundary(step);
        DeploymentLedger.Consume(LedgerRoot,id,sha,new LedgerIo(this,step),()=>Boundary(step));
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
            VerifyNewPath(handle,path);
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
    private static void VerifyNewPath(SafeFileHandle handle,string path)
    {
        var final=new StringBuilder(32768);
        uint length=GetFinalPathNameByHandleW(handle,final,(uint)final.Capacity,0);
        AdmissionPolicy.Require(length>0 && length<final.Capacity &&
            final.ToString().Equals(@"\?"+path,StringComparison.OrdinalIgnoreCase),"DEPLOY_NEW_PATH");
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

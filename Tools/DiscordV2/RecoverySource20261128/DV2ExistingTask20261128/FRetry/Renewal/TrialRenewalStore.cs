using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Win32.SafeHandles;

public static partial class LimitlessTrialRenewal20261128
{
    [DllImport("kernel32.dll", EntryPoint="CreateFileW", CharSet=CharSet.Unicode, SetLastError=true)]
    private static extern SafeFileHandle CreateReplacement(string path,uint access,uint share,ref SecurityAttributes security,uint creation,uint flags,IntPtr template);
    [DllImport("kernel32.dll",SetLastError=true)]
    [return:MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetFileInformationByHandle(SafeFileHandle file,int informationClass,byte[] information,uint bytes);

    private static void RenameWithoutReplacement(SafeFileHandle handle,string destination)
    {
        Need(IntPtr.Size==8,"HOLD_X64_REQUIRED");
        byte[] name=Encoding.Unicode.GetBytes(destination);
        // FILE_RENAME_INFO x64: ReplaceIfExists=false, RootDirectory=NULL, 이름 길이와 절대 경로.
        byte[] info=new byte[20+name.Length+2];
        Array.Copy(BitConverter.GetBytes(name.Length),0,info,16,4);
        Array.Copy(name,0,info,20,name.Length);
        Win(SetFileInformationByHandle(handle,3,info,(uint)info.Length));
    }
    // 제품 경로를 직접 받지 않습니다. 실제 쓰기는 아래 세 계약 파일과 새 보호 영수증으로 제한됩니다.
    public sealed class Store : IDisposable
    {
        const string Runtime = @"C:\ProgramData\Limitless\DiscordV2";
        const string Code = Runtime + @"\Code";
        const string Control = Runtime + @"\Control";
        const string Approvals = Control + @"\Approvals";
        const string RecoveryArchive = Control + @"\ExistingTaskFailedRegistrationArchive20261128";
        const string RegistrationSigning = @"C:\ProgramData\LimitlessDiscordV2SupervisorRecoverySigning20261128";
        const string Renewals = Control + @"\TrialRenewals";
        const string DeploymentLedger = Control + @"\DeploymentLedger";
        const string TrialId = "deef7539de5b4efcb30e062f9b734342";
        readonly List<(string Path, SafeFileHandle Handle, string Identity, bool Ancestor, int WorkerMask)> pins =
            new List<(string, SafeFileHandle, string, bool, int)>();
        readonly string actor, worker;
        readonly ulong volume;
        readonly DateTimeOffset opened;
        string activeCase;
        bool supervisorMigration;

        public Store(string actor, string worker)
        {
            this.actor = actor;
            this.worker = worker;
            opened = DateTimeOffset.UtcNow;
            DemandActor(actor, opened, opened.AddMinutes(5));
            var drive = new DriveInfo(@"C:\");
            Need(drive.DriveType == DriveType.Fixed && drive.DriveFormat == "NTFS", "HOLD_RENEWAL_NTFS");
            try
            {
                var root = Pin(Root);
                try
                {
                    string identity = ReadPinned(root, Root, true, out ulong observed);
                    volume = observed;
                    pins.Add((Root, root, identity, true, 0));
                }
                catch { root.Dispose(); throw; }
                foreach (string path in new[] { Parent, @"C:\ProgramData\Limitless", Runtime }) Add(path, true);
                foreach (string path in new[] { Control, Approvals, DeploymentLedger }) Add(path, false);
                Add(Code, false, 0x1200a9);
            }
            catch { Dispose(); throw; }
        }

        string Identity(SafeFileHandle handle, string path, bool directory, bool ancestor, int workerMask)
        {
            string identity = ReadPinned(handle, path, ancestor || workerMask != 0, out ulong observed, directory);
            Need(observed == volume, "HOLD_RENEWAL_VOLUME");
            if (workerMask == 0) return identity;
            var sd = new RawSecurityDescriptor(ReadDescriptor(handle), 0);
            Need(sd.Owner.Value == AdminSid && sd.Group.Value == SystemSid &&
                (sd.ControlFlags & ControlFlags.DiscretionaryAclProtected) != 0 && sd.DiscretionaryAcl.Count == 3,
                "HOLD_RENEWAL_CODE_DACL");
            var expected = new Dictionary<string, int> { { SystemSid, 0x1f01ff }, { AdminSid, 0x1f01ff }, { worker, workerMask } };
            foreach (GenericAce entry in sd.DiscretionaryAcl)
            {
                var ace = entry as CommonAce;
                Need(ace != null && !ace.IsCallback && ace.AceFlags == AceFlags.None &&
                    ace.AceQualifier == AceQualifier.AccessAllowed &&
                    expected.TryGetValue(ace.SecurityIdentifier.Value, out int mask) && mask == ace.AccessMask,
                    "HOLD_RENEWAL_CODE_ACE");
                expected.Remove(ace.SecurityIdentifier.Value);
            }
            return identity;
        }

        void Add(string path, bool ancestor, int workerMask = 0)
        {
            var handle = Pin(path);
            try { pins.Add((path, handle, Identity(handle, path, true, ancestor, workerMask), ancestor, workerMask)); }
            catch { handle.Dispose(); throw; }
        }

        void Check()
        {
            DemandActor(actor, opened, opened.AddMinutes(5));
            foreach (var pin in pins)
                Need(Identity(pin.Handle, pin.Path, true, pin.Ancestor, pin.WorkerMask) == pin.Identity,
                    "HOLD_RENEWAL_PARENT_CHANGED");
        }

        static bool Exists(string path)
        {
            using (var handle = OpenPath(path))
            {
                int error = Marshal.GetLastWin32Error();
                if (!handle.IsInvalid) return true;
                Need(error == 2 || error == 3, "HOLD_RENEWAL_PATH_STATUS");
                return false;
            }
        }

        static bool TrialFile(string name) => name == "TrialContract.json" ||
            name == TrialId + ".fake-only-trust.json" || name == TrialId + ".fake-only-trust-basis.txt";

        static string CasePath(string prior)
        {
            Need(prior.Length == 64 && System.Text.RegularExpressions.Regex.IsMatch(prior, @"\A[0-9a-f]{64}\z"),
                "HOLD_RENEWAL_CASE_SHA");
            return Path.Combine(Renewals, prior);
        }

        FileStream OpenRead(string path, int workerMask)
        {
            var handle = CreateFileW(path, 0x120089, 1, IntPtr.Zero, 3, 0x02200000, IntPtr.Zero);
            try
            {
                Win(!handle.IsInvalid);
                Identity(handle, path, false, false, workerMask);
                return new FileStream(handle, FileAccess.Read);
            }
            catch { handle.Dispose(); throw; }
        }

        public FileStream ReadCode(string name)
        {
            Check();
            bool image = name == "Supervisor.exe" || name == "Probe.exe";
            Need(image || TrialFile(name), "HOLD_RENEWAL_CODE_NAME");
            return OpenRead(Path.Combine(Code, name), image ? 0x1200a9 : 0x120089);
        }

        public FileStream ReadOriginalConsumption(string id)
        {
            Check();
            Need(Guid.TryParseExact(id, "N", out Guid approvalId) && approvalId != Guid.Empty &&
                approvalId.ToString("N") == id, "HOLD_RENEWAL_DEPLOYMENT_ID");
            return OpenRead(Path.Combine(DeploymentLedger,id+".deploy-consumed"),0);
        }

        public FileStream ReadOriginalManifest()
        {
            Check();
            return OpenRead(Path.Combine(Approvals, "ProductDeploymentManifest.v2.json"), 0);
        }

        public FileStream ReadOriginalRequest(bool signature)
        {
            Check();
            return OpenRead(Path.Combine(Approvals, signature ?
                "ProductDeploymentRequest.v2.json.sig" : "ProductDeploymentRequest.v2.json"), 0);
        }

        public FileStream ReadRegistrationArtifact(bool manifest)
        {
            Check();
            using (var directory=Pin(RegistrationSigning))
                Identity(directory,RegistrationSigning,true,false,0);
            return OpenRead(Path.Combine(RegistrationSigning,manifest?"ProductSigningManifest.json":"Supervisor.exe"),0);
        }

        public FileStream ReadFailedRegistrationEvidence()
        {
            Check();
            using (var directory=Pin(RecoveryArchive))
                Identity(directory,RecoveryArchive,true,false,0);
            return OpenRead(Path.Combine(RecoveryArchive,"RegistrationEvidence.json"),0);
        }

        public bool CaseExists(string prior)
        {
            Check();
            string path = CasePath(prior);
            if (!Exists(Renewals)) return false;
            using (var root = Pin(Renewals)) Identity(root, Renewals, true, false, 0);
            return Exists(path);
        }

        public FileStream ReadCase(string prior, string name)
        {
            Check();
            bool receipt = name == "Renewal.json" || name == "Renewal.sig" || name == "PromotionCommit.json" ||
                name == "SupervisorSigningManifest.json" || name == "FailedRegistrationEvidence.json";
            bool archive = name.StartsWith("Prior-", StringComparison.Ordinal) && TrialFile(name.Substring(6));
            bool image = name=="Prior-Supervisor.exe";
            Need(receipt || archive || image, "HOLD_RENEWAL_RECEIPT_NAME");
            string path = CasePath(prior);
            using (var root = Pin(Renewals))
            using (var folder = Pin(path))
            {
                Identity(root, Renewals, true, false, 0);
                Identity(folder, path, true, false, 0);
                return OpenRead(Path.Combine(path, name), image ? 0x1200a9 : archive ? 0x120089 : 0);
            }
        }

        void CreateDirectoryNew(string path)
        {
            IntPtr sd; uint size;
            Win(ConvertStringSecurityDescriptorToSecurityDescriptorW(InitialSddl, 1, out sd, out size));
            try
            {
                var a = new SecurityAttributes { Size=(uint)Marshal.SizeOf(typeof(SecurityAttributes)),Descriptor=sd,Inherit=0 };
                Check(); Win(CreateDirectoryW(path, ref a)); Add(path, false);
            }
            finally { LocalFree(sd); }
        }

        public void EnsureNoOperationInputs()
        {
            Check();
            string pending = Path.Combine(Approvals, "PendingOperations");
            foreach (string operation in new[] { "register-only", "start-once", "stop-owned-instance", "remove-owned-task" })
            {
                string baseName = operation == "start-once" ? "FakeOnlyExecution.v2" : "FakeOnlyExecution.v2." + operation;
                foreach (string name in new[]{ TrialId+"."+operation+".approval.json",baseName+".json",baseName+".sig" })
                    Need(!Exists(Path.Combine(Code,name)) && !Exists(Path.Combine(pending,name)), "HOLD_EXISTING_OPERATION_INPUT");
            }
        }

        public void Begin(string prior, DateTimeOffset deadline)
        {
            Check();
            Need(activeCase == null && DateTimeOffset.UtcNow < deadline, "HOLD_RENEWAL_ALREADY_STARTED_OR_EXPIRED");
            EnsureNoOperationInputs();
            if (!Exists(Renewals)) CreateDirectoryNew(Renewals);
            else Add(Renewals, false);
            string path = CasePath(prior);
            Need(!Exists(path), "HOLD_RENEWAL_ALREADY_USED_OR_COLLISION");
            Need(DateTimeOffset.UtcNow < deadline,"HOLD_RENEWAL_PROMOTION_TIME");
            CreateDirectoryNew(path);
            activeCase = path;
        }

        public void BeginSupervisorMigration(string prior, DateTimeOffset deadline)
        {
            Begin(prior,deadline);
            supervisorMigration=true;
        }

        static string Hash(Stream file)
        {
            file.Position = 0;
            using (var sha = SHA256.Create())
                return BitConverter.ToString(sha.ComputeHash(file)).Replace("-", "").ToLowerInvariant();
        }

        public void ArchiveTrialFile(string name, string expectedSha, DateTimeOffset deadline)
            => ArchiveRenewalInput(name,expectedSha,deadline,false);

        public void ArchiveExistingTaskRecovery(string name,string expectedSha,DateTimeOffset deadline)
        {
            Need(ExistingTaskRecoveryFile(name),"HOLD_RECOVERY_ROTATION_ARCHIVE_SCOPE");
            ArchiveRenewalInput(name,expectedSha,deadline,true);
        }

        void ArchiveRenewalInput(string name,string expectedSha,DateTimeOffset deadline,bool recovery)
        {
            Check();
            int mask=name=="Supervisor.exe" && supervisorMigration?0x1200a9:0x120089;
            Need(activeCase != null && (TrialFile(name) || mask==0x1200a9 || recovery && ExistingTaskRecoveryFile(name)),"HOLD_RENEWAL_ARCHIVE_SCOPE");
            string source = Path.Combine(Code, name);
            string destination = Path.Combine(activeCase, "Prior-" + name);
            Need(!Exists(destination), "HOLD_RENEWAL_ARCHIVE_EXISTS");
            using (var handle = CreateFileW(source, 0x130089, 0, IntPtr.Zero, 3, 0x02200000, IntPtr.Zero))
            {
                Win(!handle.IsInvalid);
                string identity = Identity(handle, source, false, false, mask);
                using (var file = new FileStream(handle, FileAccess.Read))
                {
                    Need(Hash(file) == expectedSha, "HOLD_RENEWAL_PRIOR_CHANGED");
                    Check();
                    Need(DateTimeOffset.UtcNow < deadline,"HOLD_RENEWAL_PROMOTION_TIME");
                    RenameWithoutReplacement(handle, destination);
                    Need(Identity(handle, destination, false, false, mask) == identity &&
                        Hash(file) == expectedSha, "HOLD_RENEWAL_ARCHIVE_READBACK");
                }
            }
            Need(!Exists(source), "HOLD_RENEWAL_PRIOR_STILL_PRESENT");
        }

        public void WriteCase(string name, byte[] bytes, DateTimeOffset deadline)
        {
            Need(activeCase != null && (name=="Renewal.json" || name=="Renewal.sig" || name=="PromotionCommit.json" ||
                supervisorMigration && (name=="SupervisorSigningManifest.json" || name=="FailedRegistrationEvidence.json")),
                "HOLD_RENEWAL_WRITE_CASE_SCOPE");
            WriteNew(Path.Combine(activeCase,name), bytes, deadline, 0);
        }

        public void WriteCode(string name, byte[] bytes, DateTimeOffset deadline)
        {
            Need(activeCase != null && TrialFile(name), "HOLD_RENEWAL_WRITE_CODE_SCOPE");
            WriteNew(Path.Combine(Code,name), bytes, deadline, 0x120089);
        }

        static bool ExistingTaskRecoveryFile(string name) =>
            name=="ExistingTaskRecovery.v1.json" || name=="ExistingTaskRecovery.v1.sig";

        public FileStream ReadExistingTaskRecovery(string name)
        {
            Check();
            Need(ExistingTaskRecoveryFile(name),"HOLD_EXISTING_TASK_RECOVERY_FILE");
            return OpenRead(Path.Combine(Code,name),0x120089);
        }

        public FileStream ReadRegistrationDiagnostic()
        {
            Check();
            string path=Path.Combine(Control,"RegisterDiagnostic-7f4b163e5bf94a46a475f20a785f8168.jsonl");
            var handle=CreateFileW(path,0x120089,1,IntPtr.Zero,3,0x02200000,IntPtr.Zero);
            try
            {
                Win(!handle.IsInvalid);
                // 진단 파일은 생성 시 그룹을 지정하지 않았습니다. 관리자/SYSTEM DACL은 그대로 엄격히 확인합니다.
                ReadPinned(handle,path,false,out ulong observed,false,false);
                Need(observed==volume,"HOLD_RECOVERY_DIAGNOSTIC_VOLUME");
                return new FileStream(handle,FileAccess.Read);
            }
            catch {handle.Dispose();throw;}
        }

        public void EnsureExistingTaskRecoveryAbsent()
        {
            Check();
            Need(!Exists(Path.Combine(Code,"ExistingTaskRecovery.v1.json")) &&
                !Exists(Path.Combine(Code,"ExistingTaskRecovery.v1.sig")),"HOLD_EXISTING_TASK_RECOVERY_ALREADY_PRESENT");
        }
        public void WriteExistingTaskRecovery(string name, byte[] bytes, DateTimeOffset deadline)
        {
            Need(activeCase != null && ExistingTaskRecoveryFile(name),"HOLD_EXISTING_TASK_RECOVERY_WRITE_SCOPE");
            WriteNew(Path.Combine(Code,name),bytes,deadline,0x120089);
        }
        public void WriteSupervisor(byte[] bytes, DateTimeOffset deadline)
        {
            Need(activeCase != null && supervisorMigration,"HOLD_RENEWAL_SUPERVISOR_SCOPE");
            WriteNew(Path.Combine(Code,"Supervisor.exe"),bytes,deadline,0x1200a9);
        }

        void WriteNew(string path, byte[] bytes, DateTimeOffset deadline, int mask)
        {
            Check();
            Need(DateTimeOffset.UtcNow < deadline && bytes != null && bytes.Length > 0 && bytes.Length <= (mask==0x1200a9?16777216:16384),
                "HOLD_RENEWAL_WRITE_WINDOW");
            string sddl = mask == 0 ? InitialSddl : InitialSddl + $"(A;;0x{mask:x8};;;{worker})";
            IntPtr sd; uint size;
            Win(ConvertStringSecurityDescriptorToSecurityDescriptorW(sddl, 1, out sd, out size));
            try
            {
                var a = new SecurityAttributes { Size=(uint)Marshal.SizeOf(typeof(SecurityAttributes)),Descriptor=sd,Inherit=0 };
                Check();
                Need(DateTimeOffset.UtcNow < deadline, "HOLD_RENEWAL_WRITE_WINDOW");
                using (var handle = CreateReplacement(path, 0xc0020000, 0, ref a, 1, 0x80200000, IntPtr.Zero))
                {
                    Win(!handle.IsInvalid);
                    string identity = Identity(handle,path,false,false,mask);
                    using (var file = new FileStream(handle,FileAccess.ReadWrite))
                    using (var expected = new MemoryStream(bytes,false))
                    {
                        file.Write(bytes,0,bytes.Length);
                        file.Flush(true);
                        Need(file.Length == bytes.Length && Hash(file) == Hash(expected) &&
                            Identity(handle,path,false,false,mask) == identity,"HOLD_RENEWAL_WRITE_READBACK");
                    }
                }
            }
            finally { LocalFree(sd); }
        }

        public void Dispose()
        {
            for (int i=pins.Count-1;i>=0;i--) pins[i].Handle.Dispose();
            pins.Clear();
        }
    }
}

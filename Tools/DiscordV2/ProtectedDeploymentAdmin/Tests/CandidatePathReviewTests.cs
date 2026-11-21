using System.Security.AccessControl;
using System.Text.Json;

internal static partial class DeploymentAdminTests
{
    // PowerShell로 읽기 수집한 ACL snapshot만 평가합니다. 경로 생성/ACL 적용/Windows backend 호출은 없습니다.
    private static int CandidatePathReview(string input)
    {
        using var doc=JsonDocument.Parse(File.ReadAllBytes(input));var root=doc.RootElement;
        string candidate=root.GetProperty("Candidate").GetString()!;
        var scope=new AdmissionBinding(Native.Worker,Native.TaskPath,root.GetProperty("NativeCodeRoot").GetString()!,root.GetProperty("NativeEvidenceRoot").GetString()!);
        var parents=root.GetProperty("Parents").EnumerateArray().ToArray();
        Test("candidate-canonical-fixed-local-ntfs-and-no-native-root-overlap",()=>{
            Check(candidate==@"E:\Program Files\Limitless\DiscordV2\Control\DeploymentLedger"
                &&root.GetProperty("DriveType").GetString()=="Fixed"&&root.GetProperty("FileSystem").GetString()=="NTFS");
            DeploymentLedgerPolicy.Root(candidate,candidate,scope);
        });
        Test("observed-existing-parents-system-owned-no-reparse",()=>{
            Check(parents.Length==2);
            foreach(var parent in parents)Check(parent.GetProperty("OwnerSid").GetString()==AdmissionPolicy.SystemSid&&!parent.GetProperty("Reparse").GetBoolean());
        });
        foreach(var parent in parents)
        {
            string path=parent.GetProperty("Path").GetString()!,sddl=parent.GetProperty("SDDL").GetString()!;
            Test("existing-ancestor-replacement-policy-correctly-rejects-"+path,()=>{
                var sd=new RawSecurityDescriptor(sddl);bool found=false;
                foreach(GenericAce item in sd.DiscretionaryAcl!)
                    if(item is CommonAce a&&a.AceQualifier==AceQualifier.AccessAllowed&&(a.AceFlags&AceFlags.InheritOnly)==0
                        &&a.SecurityIdentifier.Value=="S-1-5-11"&&(a.AccessMask&0x10000)!=0)found=true;
                Check(found);bool denied=false;
                try{AdmissionPolicy.Descriptor(sd,Native.Worker,false,ancestor:true);}
                catch(InvalidOperationException e) when(e.Message=="ANCESTOR_REPLACE_ACCESS"){denied=true;}
                Check(denied);
            });
        }
        Test("candidate-descendants-missing-no-ledger-security-claim",()=>Check(root.GetProperty("Missing").GetArrayLength()==4));
        Test("unsafe-candidate-not-installed-in-production-binding",()=>Check(DeploymentLedgerBinding.Root!=candidate));
        Console.WriteLine($"LEDGER_CANDIDATE_REVIEW: {pass} PASS / {fail} FAIL; CANDIDATE_ADOPTION=HOLD; PRIVATE_BINDING_NOT_CREATED; WINDOWS_MUTATIONS=0; OLD_SUITES=0");
        return fail==0?0:1;
    }
}

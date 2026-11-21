using System.Security.AccessControl;

// Windows를 변경하지 않는 보안 descriptor/이미지 정책입니다. 실제 readback도 같은 검사를 사용합니다.
internal static class DeploymentSecurityPolicy
{
    internal static string Sddl(string mask,string worker)=>"O:BAG:SYD:P(A;;FA;;;SY)(A;;FA;;;BA)(A;;"+mask+";;;"+worker+")";
    internal static void Readback(RawSecurityDescriptor actual,RawSecurityDescriptor expected,string worker,bool evidence)
    {
        AdmissionPolicy.Descriptor(actual,worker,evidence);
        var a=actual.DiscretionaryAcl??throw new InvalidOperationException("DEPLOY_NULL_DACL");
        var b=expected.DiscretionaryAcl??throw new InvalidOperationException("DEPLOY_EXPECTED_NULL_DACL");
        AdmissionPolicy.Require(actual.Owner?.Value==expected.Owner?.Value&&a.BinaryLength==b.BinaryLength,"DEPLOY_SECURITY_READBACK");
        byte[] x=new byte[a.BinaryLength],y=new byte[b.BinaryLength];a.GetBinaryForm(x,0);b.GetBinaryForm(y,0);
        AdmissionPolicy.Require(x.SequenceEqual(y),"DEPLOY_DACL_READBACK");
    }
    internal static void NativeImage(byte[] raw)
    {
        AdmissionPolicy.Require(raw.Length>=256&&BitConverter.ToUInt16(raw,0)==0x5a4d,"DEPLOY_TOOL_PE");
        int pe=BitConverter.ToInt32(raw,0x3c);
        AdmissionPolicy.Require(pe>=64&&pe<=raw.Length-264&&BitConverter.ToUInt32(raw,pe)==0x4550
            &&BitConverter.ToUInt16(raw,pe+4)==0x8664&&BitConverter.ToUInt16(raw,pe+24)==0x20b
            &&BitConverter.ToUInt64(raw,pe+24+112+14*8)==0,"DEPLOY_TOOL_NATIVE_REQUIRED");
    }
}

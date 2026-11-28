// 독립 도구 전용 진입점입니다. Supervisor.Main과 운영 게이트는 변경하지 않습니다.
internal static class AdminDeploymentProgram
{
    internal static (string Mode,string Path,string Sha) Arguments(string[] args)
    {
        AdmissionPolicy.Require(args.Length==5&&(args[0] is "--review" or "--apply")&&args[1]=="--request"
            &&Path.IsPathFullyQualified(args[2])&&args[3]=="--request-sha"&&AdmissionPolicy.Sha(args[4]),"DEPLOY_ARGUMENTS_HOLD");
        return(args[0],args[2],args[4]);
    }
    static int Main(string[] args)
    {
        try {
            var input=Arguments(args);
            using var host=new AdminDeploymentWindows(input.Path,input.Sha);
            var binding=AdmissionBinding.Current;
            CurrentFakeOnlyContract.RequireBinding(binding);
            byte[] raw=host.ReadProtected(input.Path,8192);
            host.VerifyRequestSignature(raw);
            host.VerifyCurrentProductContract(raw,binding);
            AdmissionPolicy.Require(input.Mode=="--review","FAKE_V2_PRODUCT_APPLY_HOLD");
            Console.WriteLine("SIGNED_PRODUCT_INPUTS_REVIEWED_NO_CHANGES");return 0;
        } catch {Console.Error.WriteLine("HOLD_DEPLOYMENT_NOT_COMPLETED_KEEP_EVIDENCE");return 70;}
    }
}

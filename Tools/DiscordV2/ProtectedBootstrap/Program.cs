internal static class ProtectedBootstrapProgram
{
    internal static (bool Create,string Authorization,string Sha) Arguments(string[] args)
    {
        BootstrapPolicy.Need(args.Length==5&&(args[0] is "--review" or "--create")&&args[1]=="--authorization"
            &&Path.IsPathFullyQualified(args[2])&&args[3]=="--authorization-sha"&&BootstrapPolicy.Sha(args[4]),"BOOTSTRAP_ARGUMENTS");
        return(args[0]=="--create",args[2],args[4]);
    }
    static int Main(string[] args)
    {
        try {
            var input=Arguments(args);using var host=new WindowsBootstrapHost();
            byte[] raw=host.ReadProtectedAuthorization(input.Authorization);
            using var cancel=new CancellationTokenSource();ConsoleCancelEventHandler handler=(_,e)=>{e.Cancel=true;cancel.Cancel();};
            Console.CancelKeyPress+=handler;
            try {
                int count=BootstrapController.Run(raw,input.Sha,host,input.Create,cancel.Token);
                Console.WriteLine(input.Create?$"BOOTSTRAP_CREATED_AND_VERIFIED={count}":"BOOTSTRAP_REVIEW_ONLY_NO_CHANGES");return 0;
            }finally{Console.CancelKeyPress-=handler;}
        }catch{Console.Error.WriteLine("HOLD_BOOTSTRAP_INCOMPLETE_PRESERVE_PARTIAL_STATE_NO_RETRY");return 70;}
    }
}

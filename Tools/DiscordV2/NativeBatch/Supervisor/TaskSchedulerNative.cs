using System.Runtime.InteropServices;
using System.Security;

internal enum RegistrationStage { Admission, Console, Password, FinalAdmission, SchedulerNewTask, SchedulerXml, SchedulerRegister, Readback }

// 예외 메시지에는 시스템 경로나 입력이 들어갈 수 있습니다. 승인된 단계와 숫자만 화면에 냅니다.
internal sealed class TaskRegistrationFailure(RegistrationStage stage, string reason, int? nativeCode=null) : Exception
{
    internal string SafeStatus => nativeCode is int code
        ? $"HOLD_REGISTER_{stage.ToString().ToUpperInvariant()}_{reason}_HRESULT_0x{unchecked((uint)code):X8}"
        : $"HOLD_REGISTER_{stage.ToString().ToUpperInvariant()}_{reason}";

    internal static TaskRegistrationFailure From(RegistrationStage stage,Exception error)
    {
        string reason=error switch
        {
            OperationCanceledException=>"CANCELLED",
            InvalidOperationException {Message:"REGISTER_PASSWORD_WINDOW_HOLD" or "FAKE_V2_WINDOW_HOLD" or
                "APPROVAL_EXPIRED_OR_DEADLINE" or "OPERATION_APPROVAL_EXPIRED" or "TRIAL_TIME_HOLD"}=>"APPROVAL_EXPIRED",
            InvalidOperationException {Message:"PASSWORD_CONSOLE_REQUIRED"}=>"CONSOLE_REQUIRED",
            InvalidOperationException {Message:"EMPTY_PASSWORD"}=>"EMPTY_PASSWORD",
            InvalidOperationException {Message:"PASSWORD_LENGTH"}=>"PASSWORD_LENGTH",
            _=>"REJECTED"
        };
        return new(stage,reason);
    }

    internal static void CheckSchedulerResult(RegistrationStage stage,int hr)
    {
        if(hr<0)throw new TaskRegistrationFailure(stage,"COM",hr);
    }
}

// 설치된 Windows SDK taskschd.h의 IDispatch 기반 vtable을 명시적으로 호출합니다.
// RCW/dynamic/내장 COM marshaller를 사용하지 않아 NativeAOT 경계가 드러납니다.
// 이 어댑터는 이번 시험에서 실행하지 않으며 Program의 봉인이 유지됩니다.
internal sealed unsafe class TaskSchedulerNative : IDisposable
{
    [StructLayout(LayoutKind.Explicit,Size=24)] internal struct Variant
    {
        [FieldOffset(0)] internal ushort Type;
        [FieldOffset(8)] internal nint Pointer;
        [FieldOffset(8)] internal int Number;
        internal static Variant Bstr(nint pointer)=>new(){Type=8,Pointer=pointer};
        internal static Variant Index(int value)=>new(){Type=3,Number=value};
    }
    [DllImport("ole32.dll")] private static extern int CoInitializeEx(nint reserved,uint flags);
    [DllImport("ole32.dll")] private static extern void CoUninitialize();
    [DllImport("ole32.dll")] private static extern int CoCreateInstance(in Guid clsid,nint outer,uint context,in Guid iid,out nint result);
    private nint service,folder;
    private bool initialized;
    private readonly string task;
    private readonly Func<TaskMutationBoundary> mutationBoundary;
    private static nint Slot(nint obj,int index) {AdmissionPolicy.Require(obj!=0,"COM_NULL");return (*(nint**)obj)[index];}
    private static void H(int error){if(error<0)Marshal.ThrowExceptionForHR(error);}
    private static void Release(ref nint obj){if(obj!=0){((delegate* unmanaged[Stdcall]<nint,uint>)Slot(obj,2))(obj);obj=0;}}
    private static string String(nint obj,int index)
    {nint b=0;H(((delegate* unmanaged[Stdcall]<nint,nint*,int>)Slot(obj,index))(obj,&b));try{return Marshal.PtrToStringBSTR(b);}finally{Marshal.FreeBSTR(b);}}
    private static short Short(nint obj,int index)
    {short value=0;H(((delegate* unmanaged[Stdcall]<nint,short*,int>)Slot(obj,index))(obj,&value));return value;}
    internal TaskSchedulerNative(string taskPath):this(taskPath,()=>throw new InvalidOperationException("MUTATION_BOUNDARY_REQUIRED")) { }
    internal TaskSchedulerNative(string taskPath,Func<TaskMutationBoundary> authorization)
    {
        task=taskPath;mutationBoundary=authorization;
        try
        {
            H(CoInitializeEx(0,0));initialized=true;
            H(CoCreateInstance(new Guid("0f87369f-a4e5-4cfc-bd3e-73e6154572dd"),0,1,new Guid("2faba4c7-4da9-4013-9697-20cc3fd40f85"),out service));
            H(((delegate* unmanaged[Stdcall]<nint,Variant,Variant,Variant,Variant,int>)Slot(service,10))(service,default,default,default,default));
            nint root=Marshal.StringToBSTR("\\");
            try {nint f=0;H(((delegate* unmanaged[Stdcall]<nint,nint,nint*,int>)Slot(service,7))(service,root,&f));folder=f;}
            finally{Marshal.FreeBSTR(root);}
        }
        catch{Dispose();throw;}
    }
    private nint Task(bool missingAllowed=false)
    {
        nint name=Marshal.StringToBSTR(task);nint result=0;
        try
        {
            int hr=((delegate* unmanaged[Stdcall]<nint,nint,nint*,int>)Slot(folder,13))(folder,name,&result);
            if(missingAllowed && hr==unchecked((int)0x80070002))return 0;
            if(hr<0){Release(ref result);H(hr);}return result;
        }
        finally{Marshal.FreeBSTR(name);}
    }
    internal TaskObservation? Observe()
    {
        nint t=Task(true);if(t==0)return null;
        try
        {
            string xml=String(t,20);short enabled=Short(t,10);nint sd=0;
            int state=0;H(((delegate* unmanaged[Stdcall]<nint,int*,int>)Slot(t,9))(t,&state));
            H(((delegate* unmanaged[Stdcall]<nint,int,nint*,int>)Slot(t,21))(t,5,&sd));
            string sddl;try{sddl=Marshal.PtrToStringBSTR(sd);}finally{Marshal.FreeBSTR(sd);}
            return new(xml,sddl,enabled!=0,Instances(t).Select(v=>v.Id).ToArray(),state);
        }
        finally{Release(ref t);}
    }
    private static List<(Guid Id,nint Pointer)> Instances(nint task,bool retain=false)
    {
        nint collection=0;var result=new List<(Guid,nint)>();
        try
        {
            H(((delegate* unmanaged[Stdcall]<nint,int,nint*,int>)Slot(task,14))(task,0,&collection));
            int count=0;H(((delegate* unmanaged[Stdcall]<nint,int*,int>)Slot(collection,7))(collection,&count));
            AdmissionPolicy.Require(count is >=0 and <=1,"COM_INSTANCE_LIMIT");
            for(int i=1;i<=count;i++)
            {
                nint running=0;
                try
                {
                    H(((delegate* unmanaged[Stdcall]<nint,Variant,nint*,int>)Slot(collection,8))(collection,Variant.Index(i),&running));
                    string text=String(running,8);AdmissionPolicy.Require(Guid.TryParse(text,out var id)&&id!=Guid.Empty,"COM_INSTANCE_GUID");
                    result.Add((id,retain?running:0));if(retain)running=0;
                }
                finally{Release(ref running);}
            }
            return result;
        }
        catch{foreach(var entry in result){nint ptr=entry.Item2;Release(ref ptr);}throw;}
        finally{Release(ref collection);}
    }
    internal void Register(string xml,string sddl,CancellationToken cancellation)
    {
        nint existing=Task(true);bool exists=existing!=0;Release(ref existing);AdmissionPolicy.Require(!exists,"TASK_EXISTS_CREATE_ONLY");
        nint definition=0,registered=0,xmlBstr=0,name=0,user=0,security=0,password=0;
        using var secret=new SecureString();
        RegistrationStage stage=RegistrationStage.Console;
        try
        {
            AdmissionPolicy.Require(!Console.IsInputRedirected,"PASSWORD_CONSOLE_REQUIRED");
            stage=RegistrationStage.Password;
            var boundary=mutationBoundary();
            boundary.CheckPasswordWindow();
            Console.WriteLine("Worker 암호를 입력하고 Enter를 누르세요. 입력은 표시되지 않습니다. Esc로 취소할 수 있습니다.");
            // 암호를 managed immutable string/인수/환경/로그로 만들지 않습니다.
            while(true)
            {
                cancellation.ThrowIfCancellationRequested();
                while(!Console.KeyAvailable){boundary.CheckPasswordWindow();Thread.Sleep(25);}
                boundary.CheckPasswordWindow();
                var key=Console.ReadKey(true);
                if(key.Key==ConsoleKey.Escape)throw new OperationCanceledException("PASSWORD_CANCELLED");
                if(key.Key==ConsoleKey.Enter)break;
                if(key.Key==ConsoleKey.Backspace){if(secret.Length>0)secret.RemoveAt(secret.Length-1);continue;}
                AdmissionPolicy.Require(secret.Length<256,"PASSWORD_LENGTH");
                if(!char.IsControl(key.KeyChar))secret.AppendChar(key.KeyChar);
            }
            AdmissionPolicy.Require(secret.Length>0,"EMPTY_PASSWORD");secret.MakeReadOnly();
            stage=RegistrationStage.FinalAdmission;
            boundary.AfterPassword(); // 비밀값을 읽거나 기록하지 않고 승인 상태만 재검증합니다.
            password=Marshal.SecureStringToBSTR(secret);
            stage=RegistrationStage.SchedulerNewTask;
            TaskRegistrationFailure.CheckSchedulerResult(stage,
                ((delegate* unmanaged[Stdcall]<nint,uint,nint*,int>)Slot(service,9))(service,0,&definition));
            xmlBstr=Marshal.StringToBSTR(xml);
            stage=RegistrationStage.SchedulerXml;
            TaskRegistrationFailure.CheckSchedulerResult(stage,
                ((delegate* unmanaged[Stdcall]<nint,nint,int>)Slot(definition,20))(definition,xmlBstr));
            name=Marshal.StringToBSTR(task);user=Marshal.StringToBSTR(Native.Worker);security=Marshal.StringToBSTR(sddl);
            cancellation.ThrowIfCancellationRequested();
            stage=RegistrationStage.FinalAdmission;
            mutationBoundary().Check(TaskMutation.Register);
            stage=RegistrationStage.SchedulerRegister;
            TaskRegistrationFailure.CheckSchedulerResult(stage,
                ((delegate* unmanaged[Stdcall]<nint,nint,nint,int,Variant,Variant,int,Variant,nint*,int>)Slot(folder,17))
                (folder,name,definition,0x12,Variant.Bstr(user),Variant.Bstr(password),1,Variant.Bstr(security),&registered));
        }
        catch(TaskRegistrationFailure){throw;}
        catch(Exception error){throw TaskRegistrationFailure.From(stage,error);}
        finally
        {
            if(password!=0)Marshal.ZeroFreeBSTR(password);
            if(xmlBstr!=0)Marshal.FreeBSTR(xmlBstr);if(name!=0)Marshal.FreeBSTR(name);if(user!=0)Marshal.FreeBSTR(user);if(security!=0)Marshal.FreeBSTR(security);
            Release(ref registered);Release(ref definition);
        }
    }
    internal void Enable(bool enabled)
    {nint t=Task();try{mutationBoundary().Check(enabled?TaskMutation.Enable:TaskMutation.SafetyDisable);H(((delegate* unmanaged[Stdcall]<nint,short,int>)Slot(t,11))(t,enabled?(short)-1:(short)0));}finally{Release(ref t);}}
    internal Guid Run()
    {
        nint t=Task(),running=0;
        try
        {
            mutationBoundary().Check(TaskMutation.Run);
            H(((delegate* unmanaged[Stdcall]<nint,Variant,nint*,int>)Slot(t,12))(t,default,&running));
            AdmissionPolicy.Require(String(running,9)==task,"RUN_TASK_PATH");
            AdmissionPolicy.Require(Guid.TryParse(String(running,8),out var id)&&id!=Guid.Empty,"RUN_GUID_UNKNOWN");return id;
        }
        finally{Release(ref running);Release(ref t);}
    }
    internal void Stop(Guid instance)
    {
        nint t=Task();List<(Guid Id,nint Pointer)> running=[];
        try
        {
            running=Instances(t,true);
            AdmissionPolicy.Require(running.Count==1 && running[0].Id==instance && String(running[0].Pointer,9)==task,"STOP_INSTANCE_CHANGED");
            mutationBoundary().Check(TaskMutation.Stop,instance);
            H(((delegate* unmanaged[Stdcall]<nint,int>)Slot(running[0].Pointer,12))(running[0].Pointer));
        }
        finally{foreach(var entry in running){nint ptr=entry.Pointer;Release(ref ptr);}Release(ref t);}
    }
    internal void Remove()
    {nint name=Marshal.StringToBSTR(task);try{mutationBoundary().Check(TaskMutation.Remove);H(((delegate* unmanaged[Stdcall]<nint,nint,int,int>)Slot(folder,15))(folder,name,0));}finally{Marshal.FreeBSTR(name);}}
    public void Dispose(){Release(ref folder);Release(ref service);if(initialized){CoUninitialize();initialized=false;}}
}

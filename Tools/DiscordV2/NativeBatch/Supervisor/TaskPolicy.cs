using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Linq;

internal static class TaskPolicy
{
    internal const string Sddl="O:BAG:SYD:P(A;;FA;;;SY)(A;;FA;;;BA)";
    private static readonly XNamespace Ns="http://schemas.microsoft.com/windows/2004/02/mit/task";
    internal static string Arguments(string contractSha,AdmissionBinding b) => "--supervise --contract \""+Path.Combine(b.CodeRoot,"TrialContract.json")+"\" --contract-sha "+contractSha;
    internal static string Xml(TrialContract c,string sha,AdmissionBinding b)
    {
        var task=new XElement(Ns+"Task",new XAttribute("version","1.4"),
            new XElement(Ns+"RegistrationInfo",new XElement(Ns+"Description","FAKE_ONLY / "+c.TrialId)),new XElement(Ns+"Triggers"),
            new XElement(Ns+"Principals",new XElement(Ns+"Principal",new XAttribute("id","ProbeWorker"),new XElement(Ns+"UserId",b.WorkerSid),new XElement(Ns+"LogonType","Password"),new XElement(Ns+"RunLevel","LeastPrivilege"))),
            new XElement(Ns+"Settings",new XElement(Ns+"MultipleInstancesPolicy","IgnoreNew"),new XElement(Ns+"DisallowStartIfOnBatteries",false),new XElement(Ns+"StopIfGoingOnBatteries",false),new XElement(Ns+"AllowHardTerminate",true),new XElement(Ns+"StartWhenAvailable",false),new XElement(Ns+"RunOnlyIfNetworkAvailable",false),new XElement(Ns+"AllowStartOnDemand",true),new XElement(Ns+"Enabled",false),new XElement(Ns+"Hidden",true),new XElement(Ns+"RunOnlyIfIdle",false),new XElement(Ns+"WakeToRun",false),new XElement(Ns+"ExecutionTimeLimit","PT90S"),new XElement(Ns+"Priority",7)),
            new XElement(Ns+"Actions",new XAttribute("Context","ProbeWorker"),new XElement(Ns+"Exec",new XElement(Ns+"Command",Path.Combine(b.CodeRoot,"Supervisor.exe")),new XElement(Ns+"Arguments",Arguments(sha,b)),new XElement(Ns+"WorkingDirectory",b.CodeRoot))));
        return task.ToString(SaveOptions.DisableFormatting);
    }
    private static bool IsNinetySecondLimit(string value)
    {
        // Windows Scheduler가 PT90S를 PT1M30S로 반환할 수 있습니다.
        if(!value.StartsWith("PT",StringComparison.Ordinal))return false;
        try{return XmlConvert.ToTimeSpan(value)==TimeSpan.FromSeconds(90);}
        catch(FormatException){return false;}
        catch(OverflowException){return false;}
    }
    internal static string Validate(TaskObservation task,TrialContract c,string sha,AdmissionBinding b)
    {
        AdmissionPolicy.Require(task.Instances.Length<=1 && task.Instances.All(id=>id!=Guid.Empty),"TASK_INSTANCE_LIMIT");
        AdmissionPolicy.Require(task.Instances.Length==0?task.State==(task.Enabled?3:1):task.State is 2 or 4,"TASK_STATE_OR_INSTANCE_UNKNOWN");
        var settings=new XmlReaderSettings {DtdProcessing=DtdProcessing.Prohibit,XmlResolver=null,MaxCharactersInDocument=65536};
        using var reader=XmlReader.Create(new StringReader(task.Xml),settings);
        var root=XDocument.Load(reader).Root ?? throw new InvalidOperationException("TASK_XML_EMPTY");
        AdmissionPolicy.Require(root.Name==Ns+"Task" && (string?)root.Attribute("version")=="1.4","TASK_XML_NAMESPACE_OR_VERSION");
        foreach(var e in root.Elements()) AdmissionPolicy.Require(e.Name.Namespace==Ns && e.Name.LocalName is "RegistrationInfo" or "Triggers" or "Principals" or "Settings" or "Actions","TASK_XML_EXTRA");
        XElement One(XContainer parent,string name)
        {var values=parent.Elements(Ns+name).ToArray();AdmissionPolicy.Require(values.Length==1,"TASK_XML_CARDINALITY");return values[0];}
        var triggers=One(root,"Triggers");AdmissionPolicy.Require(!triggers.Elements().Any(),"TASK_TRIGGERS");
        var principals=One(root,"Principals");var principal=One(principals,"Principal");
        AdmissionPolicy.Require(principals.Elements().Count()==1 && (string?)principal.Attribute("id")=="ProbeWorker","TASK_PRINCIPAL");
        AdmissionPolicy.Require(principal.Elements().Count()==3 && One(principal,"UserId").Value==b.WorkerSid && One(principal,"LogonType").Value=="Password" && One(principal,"RunLevel").Value=="LeastPrivilege","TASK_LOGON");
        var actions=One(root,"Actions");var exec=One(actions,"Exec");
        AdmissionPolicy.Require(actions.Elements().Count()==1 && (string?)actions.Attribute("Context")=="ProbeWorker" && exec.Elements().Count()==3,"TASK_ACTION_COUNT");
        AdmissionPolicy.Require(One(exec,"Command").Value==Path.Combine(b.CodeRoot,"Supervisor.exe") && One(exec,"Arguments").Value==Arguments(sha,b) && One(exec,"WorkingDirectory").Value==b.CodeRoot,"TASK_ACTION_BINDING");
        var observed=One(root,"Settings");var expected=XElement.Parse(Xml(c,sha,b)).Element(Ns+"Settings")!;
        foreach(var e in expected.Elements())
        {
            string value=One(observed,e.Name.LocalName).Value;
            bool matches=e.Name.LocalName switch
            {
                "Enabled"=>value==(task.Enabled?"true":"false"),
                "ExecutionTimeLimit"=>IsNinetySecondLimit(value),
                _=>value==e.Value
            };
            AdmissionPolicy.Require(matches,"TASK_SETTING");
        }
        // Scheduler의 추가 기본값도 모르는 설정이면 거부합니다. 등록 후 readback 불명은 삭제/재등록하지 않습니다.
        foreach(var e in observed.Elements())AdmissionPolicy.Require(expected.Element(e.Name) is not null,"TASK_SETTING_EXTRA_HOLD");
        var sd=new RawSecurityDescriptor(task.Sddl);
        AdmissionPolicy.Require(AdmissionPolicy.TrustedOwner(sd.Owner?.Value??"") && (sd.ControlFlags&ControlFlags.DiscretionaryAclProtected)!=0 && sd.DiscretionaryAcl is not null,"TASK_SECURITY");
        var trustees=new HashSet<string>();
        foreach(GenericAce e in sd.DiscretionaryAcl!)
        {
            AdmissionPolicy.Require(e is CommonAce {IsCallback:false,AceQualifier:AceQualifier.AccessAllowed,AceFlags:AceFlags.None} a && AdmissionPolicy.TrustedOwner(a.SecurityIdentifier.Value),"TASK_NONADMIN_ACE");
            var ace=(CommonAce)e;AdmissionPolicy.Require(trustees.Add(ace.SecurityIdentifier.Value) && unchecked((uint)ace.AccessMask) is 0x1f01ff or 0x10000000,"TASK_ADMIN_RIGHTS");
        }
        AdmissionPolicy.Require(sd.DiscretionaryAcl!.Count==2,"TASK_SECURITY_COUNT");
        var registered=One(root,"RegistrationInfo");AdmissionPolicy.Require(One(registered,"Description").Value=="FAKE_ONLY / "+c.TrialId,"TASK_TRIAL_ID");
        // Enable/Disable만 예외로 하는 fingerprint입니다. 다른 모든 XML/보안 변경은 실패합니다.
        One(observed,"Enabled").Value="false";
        string canonical=root.ToString(SaveOptions.DisableFormatting)+"\n"+sd.GetSddlForm(AccessControlSections.Owner|AccessControlSections.Access);
        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(canonical))).ToLowerInvariant();
    }
}

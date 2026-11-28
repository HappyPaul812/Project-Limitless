#Requires -Version 7.0
param([ValidatePattern('^[0-9a-f]{64}$')][string]$PriorTaskArgumentSha)
$ErrorActionPreference='Stop'
$policy=Join-Path $PSScriptRoot 'TaskPolicy.cs'
$adapter=Join-Path $PSScriptRoot 'ReadbackAdapter.cs'
if((Get-FileHash -LiteralPath $policy).Hash -cne '02DBC38CD36D69280D8399B52BFEC639340DC7CC8BC3574D9A4C215EED196A32' -or (Get-FileHash -LiteralPath $adapter).Hash -cne '2EAA87D8C3900793913D9485750B275380C5B3746DB7325119344243E1C07234'){
    throw 'HOLD_EXISTING_TASK_INSPECTION_SOURCE'
}
Import-Module -Name (Join-Path $env:windir 'System32/WindowsPowerShell/v1.0/Modules/Microsoft.PowerShell.LocalAccounts/1.0.0.0/Microsoft.PowerShell.LocalAccounts.psd1') -ErrorAction Stop
$reader=Join-Path $PSScriptRoot '../../DV2Fix20261128/TaskPrep/Read-TaskWorkerBinding.ps1'
$snapshot=@(& $reader)
if($snapshot[0] -cne 'PRODUCT_NATIVE_BINDING_LOCAL_VALUES_VERIFIED_ORIGINAL_RECEIPT_UNVERIFIED'){throw 'HOLD_BINDING'}
$b=$snapshot[-1]|ConvertFrom-Json
if(!$b.WorkerDisabled -or $b.TaskPath -cnotmatch '^\\[^\\]+$'){throw 'HOLD_TASK_INSPECTION_BOUNDARY'}
$path=Join-Path $b.CodeRoot 'TrialContract.json'
$raw=[IO.File]::ReadAllBytes($path)
$sha=[Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($raw)).ToLowerInvariant()
$c=[Text.Encoding]::UTF8.GetString($raw)|ConvertFrom-Json
if($c.TrialId -cnotmatch '^[a-f0-9]{32}$' -or $c.Scope.WorkerSid -cne $b.Worker -or
    $c.Scope.TaskPath -cne $b.TaskPath -or $c.Scope.CodeRoot -cne $b.CodeRoot -or
    $c.Scope.EvidenceRoot -cne $b.EvidenceRoot){throw 'HOLD_CURRENT_TRIAL_BINDING'}
if (!('V2TaskReadbackInspection' -as [type])) {Add-Type -Path $adapter,$policy -CompilerOptions '/nullable:enable'}
$scheduler=New-Object -ComObject Schedule.Service
$scheduler.Connect()
$task=$scheduler.GetFolder('\').GetTask($b.TaskPath.Substring(1))
if($task.Enabled -or $task.GetInstances(0).Count -ne 0){throw 'HOLD_EXISTING_TASK_NOT_DISABLED_IDLE'}
$xml=[string]$task.Xml
$sddl=[string]$task.GetSecurityDescriptor(5)
$settings=[Xml.XmlReaderSettings]::new()
$settings.DtdProcessing=[Xml.DtdProcessing]::Prohibit
$settings.XmlResolver=$null
$settings.MaxCharactersInDocument=65536
$r=[Xml.XmlReader]::Create([IO.StringReader]::new($xml),$settings)
try {
    $doc=[Xml.Linq.XDocument]::Load($r)
} finally {$r.Dispose()}
$ns=[Xml.Linq.XNamespace]::Get('http://schemas.microsoft.com/windows/2004/02/mit/task')
$principals=@($doc.Root.Elements($ns.GetName('Principals')))
$principal=if($principals.Count -eq 1){@($principals[0].Elements($ns.GetName('Principal')))}
$comPrincipal=$task.Definition.Principal
if([int]$comPrincipal.RunLevel -ne 0 -or [int]$comPrincipal.LogonType -ne 1){throw 'HOLD_EXISTING_TASK_COM_PRINCIPAL'}
$policyXml=$xml
$normalized=$false
if(@($principal).Count -eq 1 -and @($principal[0].Elements($ns.GetName('RunLevel'))).Count -eq 0){
    # XML에 없는 필드만 실제 COM의 낮은 권한 값으로 보완합니다.
    $principal[0].Add([Xml.Linq.XElement]::new($ns.GetName('RunLevel'),'LeastPrivilege'))
    $policyXml=$doc.ToString([Xml.Linq.SaveOptions]::DisableFormatting)
    $normalized=$true
}
$knownDefaults=[ordered]@{
    AllowHardTerminate=@('AllowHardTerminate',$true,'true')
    StartWhenAvailable=@('StartWhenAvailable',$false,'false')
    RunOnlyIfNetworkAvailable=@('RunOnlyIfNetworkAvailable',$false,'false')
    AllowStartOnDemand=@('AllowDemandStart',$true,'true')
    RunOnlyIfIdle=@('RunOnlyIfIdle',$false,'false')
    WakeToRun=@('WakeToRun',$false,'false')
    Priority=@('Priority',7,'7')
}
$xmlSettings=@($doc.Root.Elements($ns.GetName('Settings')))
function Assert-EffectiveSettings($actual) {
    foreach($name in $knownDefaults.Keys){
        $rule=$knownDefaults[$name]
        $comName=$rule[0]
        $value=$actual.$comName
        if(($rule[1] -is [bool] -and $value -isnot [bool]) -or $value -ne $rule[1]){
            throw ('HOLD_EXISTING_TASK_COM_SETTING_'+$name)
        }
    }
}
if($xmlSettings.Count -eq 1){
    Assert-EffectiveSettings $task.Definition.Settings
    foreach($name in $knownDefaults.Keys){
        $xname=$ns.GetName($name)
        if(@($xmlSettings[0].Elements($xname)).Count -eq 0){
            $xmlSettings[0].Add([Xml.Linq.XElement]::new($xname,$knownDefaults[$name][2]))
        }
    }
    $policyXml=$doc.ToString([Xml.Linq.SaveOptions]::DisableFormatting)
}
try {
    $fingerprint=[V2TaskReadbackInspection]::Fingerprint($policyXml,$sddl,[bool]$task.Enabled,[int]$task.State,[Guid[]]@(),
        $c.TrialId,($PriorTaskArgumentSha ? $PriorTaskArgumentSha : $sha),$b.Worker,$b.TaskPath,$b.CodeRoot,$b.EvidenceRoot)
} catch {
    $status=$_.Exception.GetBaseException().Message
    if($status -cmatch '^TASK_(?:XML_CARDINALITY|SETTING|SETTING_EXTRA_HOLD)$'){
        $counts=[Collections.Generic.List[string]]::new()
        function Read-ExpectedXmlFields($parent,[string]$section,[string[]]$names){
            if($null -eq $parent){return}
            foreach($name in $names){
                $count=@($parent.Elements($ns.GetName($name))).Count
                if($count -ne 1){$counts.Add("$section/$name=$count")}
            }
        }
        Read-ExpectedXmlFields $doc.Root 'Task' @('RegistrationInfo','Triggers','Principals','Settings','Actions')
        $registration=$doc.Root.Element($ns.GetName('RegistrationInfo'))
        Read-ExpectedXmlFields $registration 'RegistrationInfo' @('Description')
        $principalNode=$doc.Root.Element($ns.GetName('Principals'))
        Read-ExpectedXmlFields $principalNode 'Principals' @('Principal')
        if($null -ne $principalNode){
            Read-ExpectedXmlFields ($principalNode.Element($ns.GetName('Principal'))) 'Principal' @('UserId','LogonType','RunLevel')
        }
        $actions=$doc.Root.Element($ns.GetName('Actions'))
        Read-ExpectedXmlFields $actions 'Actions' @('Exec')
        if($null -ne $actions){
            Read-ExpectedXmlFields ($actions.Element($ns.GetName('Exec'))) 'Exec' @('Command','Arguments','WorkingDirectory')
        }
        $expected=[ordered]@{
            MultipleInstancesPolicy='IgnoreNew';DisallowStartIfOnBatteries='false';StopIfGoingOnBatteries='false'
            AllowHardTerminate='true';StartWhenAvailable='false';RunOnlyIfNetworkAvailable='false'
            AllowStartOnDemand='true';Enabled='false';Hidden='true';RunOnlyIfIdle='false';WakeToRun='false'
            ExecutionTimeLimit='PT90S';Priority='7'
        }
        $settingsNode=$doc.Root.Element($ns.GetName('Settings'))
        Read-ExpectedXmlFields $settingsNode 'Settings' @($expected.Keys)
        $different=@()
        $extraCount=0
        if($null -ne $settingsNode){
            $different=@(foreach($name in $expected.Keys){
                $element=$settingsNode.Element($ns.GetName($name))
                if($null -eq $element){continue}
                if($name -ceq 'ExecutionTimeLimit'){
                    try {
                        if(!$element.Value.StartsWith('PT',[StringComparison]::Ordinal) -or
                            [Xml.XmlConvert]::ToTimeSpan($element.Value) -ne [TimeSpan]::FromSeconds(90)){$name}
                    } catch {$name}
                } elseif($element.Value -cne $expected[$name]){$name}
            })
            $extraCount=@($settingsNode.Elements() | Where-Object {
                $_.Name.Namespace -ne $ns -or $_.Name.LocalName -cnotin $expected.Keys
            }).Count
        }
        $comSettings=$task.Definition.Settings
        $effective=[ordered]@{}
        foreach($name in 'AllowHardTerminate','StartWhenAvailable','RunOnlyIfNetworkAvailable','AllowDemandStart','RunOnlyIfIdle','WakeToRun','Priority'){
            try {
                $value=$comSettings.$name
                $effective[$name]=if($value -is [bool] -or $value -is [int] -or $value -is [short]){$value}else{'UNAVAILABLE'}
            } catch {$effective[$name]='UNAVAILABLE'}
        }
        $extras=@()
        if($null -ne $settingsNode){
            $extras=@(foreach($node in $settingsNode.Elements()){
                if($node.Name.Namespace -eq $ns -and $node.Name.LocalName -cin $expected.Keys){continue}
                [ordered]@{
                    Name=$node.Name.LocalName
                    Scalar=if(!$node.HasElements -and $node.Value -cmatch '^(true|false|[0-9]{1,4}|PT[0-9HMS.]{1,24})$'){$node.Value}else{$null}
                    Children=@(foreach($child in $node.Elements()){
                        [ordered]@{Name=$child.Name.LocalName;Scalar=if(!$child.HasElements -and $child.Value -cmatch '^(true|false|[0-9]{1,4}|PT[0-9HMS.]{1,24})$'){$child.Value}else{$null}}
                    })
                }
            })
        }
        $sdStatus='UNCLASSIFIED'
        try {
            $descriptor=[Security.AccessControl.RawSecurityDescriptor]::new($sddl)
            $seen=[Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
            $valid=$descriptor.Owner.Value -cin 'S-1-5-18','S-1-5-32-544' -and
                ($descriptor.ControlFlags -band [Security.AccessControl.ControlFlags]::DiscretionaryAclProtected) -and
                $null -ne $descriptor.DiscretionaryAcl -and $descriptor.DiscretionaryAcl.Count -eq 2
            if($valid){
                foreach($ace in $descriptor.DiscretionaryAcl){
                    if($ace -isnot [Security.AccessControl.CommonAce] -or $ace.IsCallback -or
                        $ace.AceQualifier -ne [Security.AccessControl.AceQualifier]::AccessAllowed -or $ace.AceFlags -ne 0 -or
                        $ace.SecurityIdentifier.Value -cnotin 'S-1-5-18','S-1-5-32-544' -or
                        !$seen.Add($ace.SecurityIdentifier.Value) -or $ace.AccessMask -notin 0x1f01ff,0x10000000){
                        $valid=$false
                        break
                    }
                }
            }
            $sdStatus=if($valid){'PASS'}else{'HOLD'}
        } catch {$sdStatus='HOLD'}
        [ordered]@{XmlFieldCountsDiffer=@($counts.ToArray());XmlSettingValuesDiffer=$different;ExtraXmlSettingsCount=$extraCount
            ComSettings=$effective;ExtraXmlSettings=$extras;SddlPolicy=$sdStatus} |
            ConvertTo-Json -Depth 8 -Compress
    }
    if($status -cmatch '^TASK_[A-Z_]+$'){throw ('HOLD_EXISTING_TASK_XML_SDDL_'+$status)}
    throw 'HOLD_EXISTING_TASK_XML_SDDL_UNCLASSIFIED'
}
if($task.Enabled -or $task.GetInstances(0).Count -ne 0 -or [string]$task.Xml -cne $xml -or
    [int]$task.Definition.Principal.RunLevel -ne 0 -or [int]$task.Definition.Principal.LogonType -ne 1 -or
    [string]$task.GetSecurityDescriptor(5) -cne $sddl -or (Get-FileHash -LiteralPath $path).Hash.ToLowerInvariant() -cne $sha){
    throw 'HOLD_EXISTING_TASK_READBACK_CHANGED'
}
Assert-EffectiveSettings $task.Definition.Settings
'EXISTING_TASK_COMPATIBILITY_XML_SDDL_FINGERPRINT_VERIFIED_READ_ONLY'
"TASK_FINGERPRINT_SHA256=$fingerprint"
"TASK_RUNLEVEL_NORMALIZED_FROM_COM=$normalized"
'REGISTRATION_RECEIPT_NOT_CREATED'

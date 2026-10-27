param([ValidateSet('SelfTest','Collect')][string]$Mode='SelfTest',[switch]$ConfirmReadOnly)
$ErrorActionPreference='Stop';Set-StrictMode -Version Latest
# 설치 코드를 로드하지 않습니다. 기본 분기는 합성 검사이며 OS 조회는 Collect에서만 수행합니다.
function Get-R2ProbeErrors([Exception]$ErrorObject){
 $rows=@();for($e=$ErrorObject;$null -ne $e;$e=$e.InnerException){$r=@{type=$e.GetType().FullName;hresult=$e.HResult;hex=('0x{0:X8}' -f $e.HResult)};if($e -is [ComponentModel.Win32Exception]){$r.win32=$e.NativeErrorCode};foreach($key in @('ntstatus','operation')){if($e.Data.Contains($key)){$r[$key]=$e.Data[$key]}};$rows+=$r};return ,$rows
}
function Get-R2ProbeFailureClass($Errors){
 $codes=@($Errors|ForEach-Object hex);if($codes -contains '0x80070005' -or @($Errors|Where-Object {$_.Contains('win32') -and $_.win32 -eq 5}).Count){return 'ACCESS_DENIED'}
 if($codes -contains '0x80070002'){return 'NOT_FOUND'}
 if($codes -contains '0x800705B4' -or $codes -contains '0x8001011F'){return 'COM_TIMEOUT'};return 'UNKNOWN'
}
function Get-R2ProbeTaskSecurity($Task){
 $sd=$Task.GetSecurityDescriptor(7);$s=[Security.AccessControl.RawSecurityDescriptor]::new($sd)
 $worker='S-1-5-21-2858504392-1958168942-1662906075-1010'
 $expected=[Security.AccessControl.RawSecurityDescriptor]::new(('O:BAG:SYD:P(A;;FA;;;SY)(A;;FA;;;BA)(A;;GR;;;'+$worker+')'))
 $header=$null -ne $s.Owner -and $null -ne $s.Group -and $s.Owner.Value -ceq $expected.Owner.Value -and $s.Group.Value -ceq $expected.Group.Value -and ($s.ControlFlags -band [Security.AccessControl.ControlFlags]::DiscretionaryAclProtected) -ne 0 -and $null -ne $s.DiscretionaryAcl -and $s.DiscretionaryAcl.Count -eq $expected.DiscretionaryAcl.Count
 $actual=@();$wanted=@();$aceKnown=$true
 foreach($a in $s.DiscretionaryAcl){if($a -isnot [Security.AccessControl.CommonAce] -or $a.IsCallback -or $a.AceQualifier -ne [Security.AccessControl.AceQualifier]::AccessAllowed){$aceKnown=$false}else{$actual+=($a.SecurityIdentifier.Value+':'+$a.AccessMask+':'+[int]$a.AceFlags)}}
 foreach($a in $expected.DiscretionaryAcl){$wanted+=($a.SecurityIdentifier.Value+':'+$a.AccessMask+':'+[int]$a.AceFlags)}
 $rights=$aceKnown -and ((@($actual|Sort-Object)-join '|') -ceq (@($wanted|Sort-Object)-join '|'))
 @{sddl=$sd;owner_sid=if($s.Owner){$s.Owner.Value}else{$null};group_sid=if($s.Group){$s.Group.Value}else{$null};dacl_protected=($s.ControlFlags -band [Security.AccessControl.ControlFlags]::DiscretionaryAclProtected) -ne 0;frozen_header_matches=$header;frozen_rights_match=$rights;frozen_exact_sddl_matches=($header -and $rights);historical_failure_proven=$false}
}
function Invoke-R2ProbeQuery([string]$Stage,[scriptblock]$Read){
 try{@{stage=$Stage;status='READ_OK';value=(& $Read)}}catch{$codes=Get-R2ProbeErrors $_.Exception;@{stage=$Stage;status='UNKNOWN';failure=(Get-R2ProbeFailureClass $codes);errors=$codes}}
}
function Get-R2ProbeHash([byte[]]$Bytes){[Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($Bytes)).ToLowerInvariant()}
function Assert-R2ProbeNoReparse([string]$Path){
 $p=$Path;while($p){$i=Get-Item -LiteralPath $p -Force -ErrorAction Stop;if($i.Attributes -band [IO.FileAttributes]::ReparsePoint){throw 'R2_REPARSE_HOLD'};$parent=Split-Path $p -Parent;if($parent -eq $p){break};$p=$parent}
}
function Get-R2ProbeRecord($Raw){
 # 증거의 임의 문자열과 오류 본문은 제외하고 허용된 형식의 필드만 출력합니다.
 $out=@{};foreach($key in @('schema','install_calls','task_attempts','account_attempts','automatic_retries','ipc_calls','model_calls','auth_imports')){if($Raw.Contains($key) -and $Raw[$key] -is [ValueType]){$out[$key]=$Raw[$key]}}
 foreach($key in @('consumed','approval_created','existing_preserved','partial_assets_preserve_no_retry')){if($Raw.Contains($key) -and $Raw[$key] -is [bool]){$out[$key]=$Raw[$key]}}
 foreach($key in @('status','stage','phase','kind')){if($Raw.Contains($key) -and [string]$Raw[$key] -cmatch '\A[A-Z0-9_]{1,120}\z'){$out[$key]=$Raw[$key]}}
 if($Raw.Contains('errors')){$out.errors=@(foreach($e in $Raw.errors){$row=@{};foreach($k in @('hresult','win32','ntstatus')){if($e.Contains($k) -and $e[$k] -is [ValueType]){$row[$k]=$e[$k]}};if($e.Contains('type') -and $e.type -cmatch '\A[A-Za-z0-9_.+]{1,180}\z'){$row.type=$e.type};$row})}
 if($Raw.Contains('native_result') -and $Raw.native_result -is [Collections.IDictionary]){$out.native_result=Get-R2ProbeRecord $Raw.native_result};return $out
}
function Get-R2ProbeEvent($Xml,[string]$TaskPath){
 $settings=[Xml.XmlReaderSettings]::new();$settings.DtdProcessing=[Xml.DtdProcessing]::Prohibit;$settings.XmlResolver=$null
 $reader=[Xml.XmlReader]::Create([IO.StringReader]::new($Xml),$settings);try{$d=[Xml.XmlDocument]::new();$d.XmlResolver=$null;$d.Load($reader)}finally{$reader.Dispose()}
 $ns=[Xml.XmlNamespaceManager]::new($d.NameTable);$ns.AddNamespace('e','http://schemas.microsoft.com/win/2004/08/events/event')
 $name=$d.SelectSingleNode('//e:Data[@Name="TaskName"]',$ns);if($null -eq $name -or $name.InnerText -cne $TaskPath){return $null}
 $out=@{id=[int]$d.SelectSingleNode('/e:Event/e:System/e:EventID',$ns).InnerText;record_id=[long]$d.SelectSingleNode('/e:Event/e:System/e:EventRecordID',$ns).InnerText;time_utc=$d.SelectSingleNode('/e:Event/e:System/e:TimeCreated',$ns).GetAttribute('SystemTime')}
 foreach($key in @('ResultCode','ErrorValue','ErrorCode')){$n=$d.SelectSingleNode('//e:Data[@Name="'+$key+'"]',$ns);if($null -ne $n -and $n.InnerText -cmatch '\A(0x[0-9A-Fa-f]{1,8}|-?[0-9]{1,12})\z'){$out[$key]=$n.InnerText}};return $out
}
$nativeSource=@'
// 읽기 전용 NetAPI/LSA 및 핸들 속성만 조회합니다. 권한 부여나 계정 변경 API는 선언하지 않습니다.
using System;using System.Collections.Generic;using System.ComponentModel;using System.Runtime.InteropServices;using System.Security.Principal;using Microsoft.Win32.SafeHandles;
public static class R2PartialReadNative {
 [StructLayout(LayoutKind.Sequential)]struct User4{public IntPtr name,password;public uint age,priv;public IntPtr home,comment;public uint flags;public IntPtr script;public uint auth;public IntPtr full,usr,parms,stations;public uint laston,lastoff,expires,storage,units;public IntPtr hours;public uint bad,logons;public IntPtr server;public uint country,code;public IntPtr sid;public uint primary;public IntPtr profile,drive;public uint expired;}
 [StructLayout(LayoutKind.Sequential)]struct Text{public ushort Length,Max;public IntPtr Buffer;}
 [StructLayout(LayoutKind.Sequential)]struct Attr{public int Length;public IntPtr Root,Object;public uint Flags;public IntPtr Descriptor,Quality;}
 [StructLayout(LayoutKind.Sequential)]struct Info{public uint attributes;public System.Runtime.InteropServices.ComTypes.FILETIME creation,access,write;public uint volume,sizeHigh,sizeLow,links,indexHigh,indexLow;}
 [DllImport("netapi32.dll",CharSet=CharSet.Unicode)]static extern uint NetUserGetInfo(string server,string user,uint level,out IntPtr p);
 [DllImport("netapi32.dll",CharSet=CharSet.Unicode)]static extern uint NetUserGetLocalGroups(string server,string user,uint level,uint flags,out IntPtr p,uint max,out uint read,out uint total);
 [DllImport("netapi32.dll")]static extern uint NetApiBufferFree(IntPtr p);
 [DllImport("advapi32.dll")]static extern uint LsaOpenPolicy(IntPtr name,ref Attr attr,uint access,out IntPtr handle);
 [DllImport("advapi32.dll")]static extern uint LsaEnumerateAccountRights(IntPtr h,byte[] sid,out IntPtr rights,out uint count);
 [DllImport("advapi32.dll")]static extern uint LsaFreeMemory(IntPtr p);
 [DllImport("advapi32.dll")]static extern uint LsaClose(IntPtr p);
 [DllImport("advapi32.dll")]static extern uint LsaNtStatusToWinError(uint s);
 [DllImport("kernel32.dll",CharSet=CharSet.Unicode,SetLastError=true)]static extern SafeFileHandle CreateFileW(string p,uint access,uint share,IntPtr security,uint mode,uint flags,IntPtr template);
 [DllImport("kernel32.dll",SetLastError=true)]static extern bool GetFileInformationByHandle(SafeFileHandle h,out Info i);
 static Exception Error(string op,uint code){var e=new Win32Exception((int)code);e.Data["operation"]=op;return e;}
 static void SidOnly(string sid){if(!System.Text.RegularExpressions.Regex.IsMatch(sid,@"\AS-1-5-21-2858504392-1958168942-1662906075-(1009|1010)\z"))throw new InvalidOperationException("R2_SID_SCOPE");}
 public static object User(string name){if(name!="LIMITLESSBotV2D"&&name!="LIMITLESSWorkV2D"&&name!="LIMITLESSRelaySvc"&&name!="LIMITLESSBotTrial")throw new InvalidOperationException("R2_USER_SCOPE");IntPtr p=IntPtr.Zero;uint s=NetUserGetInfo(null,name,4,out p);try{if(s!=0)throw Error("NetUserGetInfo4",s);var u=Marshal.PtrToStructure<User4>(p);bool restricted=false;if(u.hours!=IntPtr.Zero){int bytes=checked((int)((u.units+7)/8));if(bytes>1260)throw new InvalidOperationException("R2_HOURS_BOUND");for(int n=0;n<bytes;n++)if(Marshal.ReadByte(u.hours,n)!=255)restricted=true;}return new Dictionary<string,object>{{"sid",new SecurityIdentifier(u.sid).Value},{"enabled",(u.flags&2)==0},{"flags",u.flags},{"privilege_level",u.priv},{"expires_unix",u.expires},{"password_expired",u.expired!=0},{"locked_out",(u.flags&16)!=0},{"logon_hours_restricted",restricted},{"logon_units_per_week",u.units},{"workstation_restricted",u.stations!=IntPtr.Zero&&!String.IsNullOrEmpty(Marshal.PtrToStringUni(u.stations))}};}finally{if(p!=IntPtr.Zero)NetApiBufferFree(p);}}
 public static string[] Groups(string name){if(name!="LIMITLESSBotV2D"&&name!="LIMITLESSWorkV2D")throw new InvalidOperationException("R2_USER_SCOPE");IntPtr p=IntPtr.Zero;uint read,total;uint s=NetUserGetLocalGroups(null,name,0,1,out p,0xffffffff,out read,out total);try{if(s!=0)throw Error("NetUserGetLocalGroups",s);if(read!=total||read>256)throw new InvalidOperationException("R2_GROUP_BOUND");var all=new List<string>();for(int n=0;n<read;n++){string g=Marshal.PtrToStringUni(Marshal.ReadIntPtr(p,n*IntPtr.Size));all.Add(((SecurityIdentifier)new NTAccount(g).Translate(typeof(SecurityIdentifier))).Value);}return all.ToArray();}finally{if(p!=IntPtr.Zero)NetApiBufferFree(p);}}
 public static string[] Rights(string sid){SidOnly(sid);var id=new SecurityIdentifier(sid);var binary=new byte[id.BinaryLength];id.GetBinaryForm(binary,0);IntPtr h=IntPtr.Zero,p=IntPtr.Zero;try{var a=new Attr{Length=Marshal.SizeOf<Attr>()};uint s=LsaOpenPolicy(IntPtr.Zero,ref a,0x800,out h);if(s!=0){var e=Error("LsaOpenPolicy_READ",LsaNtStatusToWinError(s));e.Data["ntstatus"]=s;throw e;}uint count;s=LsaEnumerateAccountRights(h,binary,out p,out count);if(s!=0){var e=Error("LsaEnumerateAccountRights",LsaNtStatusToWinError(s));e.Data["ntstatus"]=s;throw e;}var all=new string[count];for(int n=0;n<count;n++){var t=Marshal.PtrToStructure<Text>(IntPtr.Add(p,n*Marshal.SizeOf<Text>()));all[n]=Marshal.PtrToStringUni(t.Buffer,t.Length/2);}return all;}finally{if(p!=IntPtr.Zero)LsaFreeMemory(p);if(h!=IntPtr.Zero)LsaClose(h);}}
 public static object FileInfo(string path){using(var h=CreateFileW(path,0x80,7,IntPtr.Zero,3,0x02200000,IntPtr.Zero)){if(h.IsInvalid)throw Error("CreateFile_READ_ATTRIBUTES",(uint)Marshal.GetLastWin32Error());Info i;if(!GetFileInformationByHandle(h,out i))throw Error("GetFileInformationByHandle",(uint)Marshal.GetLastWin32Error());return new Dictionary<string,object>{{"links",i.links},{"volume",i.volume.ToString("x8")},{"file_id",i.indexHigh.ToString("x8")+i.indexLow.ToString("x8")},{"attributes",i.attributes}};}}
}
'@
if(-not ('R2PartialReadNative' -as [type])){Add-Type -TypeDefinition $nativeSource}
if($Mode -eq 'SelfTest'){
 $tests=@();function Test-R2Probe($Name,[scriptblock]$Test){try{if(-not (& $Test)){throw 'ASSERT'};$script:tests+=@{name=$Name;status='PASS'}}catch{$script:tests+=@{name=$Name;status='FAIL';errors=(Get-R2ProbeErrors $_.Exception)}}}
 Test-R2Probe 'ACCESS_DENIED_NOT_ABSENT' { $q=Invoke-R2ProbeQuery 'GetTask' {throw [Runtime.InteropServices.COMException]::new('SECRET',-2147024891)};$q.status -eq 'UNKNOWN' -and $q.failure -eq 'ACCESS_DENIED'}
 Test-R2Probe 'EXACT_NOT_FOUND_CODE' {(Get-R2ProbeFailureClass @(@{hex='0x80070002'})) -eq 'NOT_FOUND'}
 Test-R2Probe 'UNKNOWN_GENERIC_HRESULT' {(Get-R2ProbeFailureClass @(@{hex='0x80131509'})) -eq 'UNKNOWN'}
 Test-R2Probe 'COM_TIMEOUT_NOT_ABSENT' {(Get-R2ProbeFailureClass @(@{hex='0x8001011F'})) -eq 'COM_TIMEOUT'}
 Test-R2Probe 'INNER_RAW_CODE_NO_MESSAGE' {$q=Invoke-R2ProbeQuery 'SD_READ' {throw [InvalidOperationException]::new('SECRET',[ComponentModel.Win32Exception]::new(5))};$j=$q|ConvertTo-Json -Depth 8; $j -notmatch 'SECRET' -and $j -match 'win32'}
 Test-R2Probe 'RECORD_ALLOWLIST_NO_SECRET' {$r=Get-R2ProbeRecord @{status='HOLD';command='SECRET';password='SECRET';errors=@(@{type='System.Exception';message='SECRET';hresult=-1})};($r|ConvertTo-Json -Depth 8) -notmatch 'SECRET|password|command|message'}
 Test-R2Probe 'EVENT_EXACT_TASK_NO_ACTION' {$xml='<Event xmlns="http://schemas.microsoft.com/win/2004/08/events/event"><System><EventID>106</EventID><EventRecordID>3</EventRecordID><TimeCreated SystemTime="2026-10-27T00:00:00Z"/></System><EventData><Data Name="TaskName">\R2</Data><Data Name="Command">SECRET</Data></EventData></Event>';$e=Get-R2ProbeEvent $xml '\R2';$e.id -eq 106 -and ($e|ConvertTo-Json) -notmatch 'SECRET'}
 Test-R2Probe 'OTHER_TASK_NOT_EXPORTED' {$xml='<Event xmlns="http://schemas.microsoft.com/win/2004/08/events/event"><EventData><Data Name="TaskName">\Other</Data></EventData></Event>';$null -eq (Get-R2ProbeEvent $xml '\R2')}
 Test-R2Probe 'NATIVE_SCOPE_REJECT_BEFORE_OS' {try{[R2PartialReadNative]::User('Other');$false}catch{$true}}
 Test-R2Probe 'RIGHTS_OTHER_SID_REJECT_BEFORE_OS' {try{[R2PartialReadNative]::Rights('S-1-5-18');$false}catch{$true}}
 Test-R2Probe 'ACTUAL_SECURITY_READER_EXACT_FAKE' {$t=[pscustomobject]@{};$t|Add-Member ScriptMethod GetSecurityDescriptor {param($flags)if($flags -ne 7){throw 'FLAGS'};'O:BAG:SYD:P(A;;FA;;;SY)(A;;FA;;;BA)(A;;GR;;;S-1-5-21-2858504392-1958168942-1662906075-1010)'};(Get-R2ProbeTaskSecurity $t).frozen_exact_sddl_matches}
 Test-R2Probe 'ACTUAL_SECURITY_READER_HEADER_MISMATCH_FAKE' {$t=[pscustomobject]@{};$t|Add-Member ScriptMethod GetSecurityDescriptor {param($flags)'O:SYG:SYD:P(A;;FA;;;SY)(A;;FA;;;BA)(A;;GR;;;S-1-5-21-2858504392-1958168942-1662906075-1010)'};$s=Get-R2ProbeTaskSecurity $t; -not $s.frozen_header_matches -and $s.frozen_rights_match -and -not $s.historical_failure_proven}
 Test-R2Probe 'ACTUAL_SECURITY_READER_ACCESS_DENIED_FAKE' {$t=[pscustomobject]@{};$t|Add-Member ScriptMethod GetSecurityDescriptor {param($flags)throw [Runtime.InteropServices.COMException]::new('SECRET',-2147024891)};$q=Invoke-R2ProbeQuery 'TASK_SECURITY_DESCRIPTOR' {Get-R2ProbeTaskSecurity $t};$q.status -eq 'UNKNOWN' -and $q.failure -eq 'ACCESS_DENIED'}
 Test-R2Probe 'WIN32_ACCESS_DENIED_PRESERVED' {$q=Invoke-R2ProbeQuery 'NetAPI' {throw [ComponentModel.Win32Exception]::new(5)};$q.failure -eq 'ACCESS_DENIED'}
 Test-R2Probe 'EVENT_DTD_REJECT' {try{Get-R2ProbeEvent '<!DOCTYPE Event [<!ENTITY x SYSTEM "file:///secret">]><Event>&x;</Event>' '\R2';$false}catch{$true}}
 Test-R2Probe 'RAW_RECORD_NESTED_SECRET_REMOVED' {$o=Get-R2ProbeRecord @{native_result=@{status='HOLD';errors=@(@{type='System.Exception';hresult=-1;message='SECRET'});environment='SECRET'}};($o|ConvertTo-Json -Depth 12) -notmatch 'SECRET|environment|message'}
 Test-R2Probe 'NATIVE_IMPORT_READONLY_ALLOWLIST' {$imports=@([regex]::Matches($nativeSource,'extern\s+(?:uint|bool|SafeFileHandle)\s+([A-Za-z0-9_]+)\(')|ForEach-Object {$_.Groups[1].Value});$allowed=@('NetUserGetInfo','NetUserGetLocalGroups','NetApiBufferFree','LsaOpenPolicy','LsaEnumerateAccountRights','LsaFreeMemory','LsaClose','LsaNtStatusToWinError','CreateFileW','GetFileInformationByHandle');$imports.Count -eq $allowed.Count -and @($imports|Where-Object {$_ -cnotin $allowed}).Count -eq 0 -and $nativeSource.Contains('CreateFileW(path,0x80,7,IntPtr.Zero,3,0x02200000,IntPtr.Zero)')}
 @{kind='R2_READONLY_SYNTHETIC';pass=@($tests|Where-Object status -EQ PASS).Count;fail=@($tests|Where-Object status -EQ FAIL).Count;tests=$tests;os_queries=0;changes=0;install_calls=0;password_reads=0}|ConvertTo-Json -Depth 12;return
}
if(-not $ConfirmReadOnly){throw 'R2_READONLY_EXPLICIT_CONFIRM_REQUIRED'}
if($PSVersionTable.PSVersion.Major -ne 7 -or -not [Environment]::Is64BitProcess){throw 'R2_PS7_X64_REQUIRED'}
$identity=[Security.Principal.WindowsIdentity]::GetCurrent();if(-not ([Security.Principal.WindowsPrincipal]::new($identity)).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)){throw 'R2_ADMIN_READONLY_REQUIRED'}
$package='3effaac30bf44760986fee9e6019b283';$taskName='LIMITLESS-DedicatedV2-20261027-Host';$taskPath='\'+$taskName
$code='C:\Program Files\LimitlessRelayDedicatedV2_20261027';$state='C:\ProgramData\LimitlessRelayDedicatedV2_20261027';$approvalRoot='C:\ProgramData\LimitlessRelayDedicatedV2Approvals';$approvalId='dedicated-install-'+$package
$r=@{schema=3;kind='R2_PARTIAL_INSTALL_READONLY';status='HOLD_DIAGNOSTIC_ONLY_NO_RETRY';admin=$true;collected_utc=[datetime]::UtcNow.ToString('o');changes=0;automatic_retries=0;install_calls=0;ipc_calls=0;model_calls=0;password_reads=0;queries=@();task_presence='UNKNOWN';historical_failure_cause='UNKNOWN';runtime_status='HOLD_NEW_TOKEN_GROUP_EXCLUSIVITY_NOT_VERIFIED';v1_status='REFERENCE_VERIFICATION_HOLD'}
function Add-R2ProbeQuery([string]$Stage,[scriptblock]$Read){$q=Invoke-R2ProbeQuery $Stage $Read;$script:r.queries+=,$q;return $q}
function Read-R2ProbeFile([string]$Path,[string]$Expected=''){
 # 상위 경로의 reparse를 거부하고 읽기 핸들의 파일 ID와 링크 수도 기록합니다.
 Assert-R2ProbeNoReparse $Path
 $item=Get-Item -LiteralPath $Path -Force -ErrorAction Stop;$sd=[Security.AccessControl.RawSecurityDescriptor]::new((Get-Acl -LiteralPath $Path -ErrorAction Stop).Sddl)
 $out=@{path=$Path;directory=$item.PSIsContainer;owner_sid=$sd.Owner.Value;group_sid=$sd.Group.Value;sddl=$sd.GetSddlForm([Security.AccessControl.AccessControlSections]14);creation_utc=$item.CreationTimeUtc.ToString('o');write_utc=$item.LastWriteTimeUtc.ToString('o');handle=[R2PartialReadNative]::FileInfo($Path)}
 if(-not $item.PSIsContainer){$out.length=$item.Length;$out.sha256=(Get-FileHash -LiteralPath $Path -Algorithm SHA256 -ErrorAction Stop).Hash.ToLowerInvariant();$out.sha_match=if($Expected){$out.sha256 -ceq $Expected}else{$null};$streams=@(Get-Item -LiteralPath $Path -Stream * -ErrorAction Stop);$out.ads_count=@($streams|Where-Object Stream -NE ':$DATA').Count;if($out.handle.links -ne 1 -or $out.ads_count -ne 0){throw 'R2_LINK_OR_ADS_HOLD'}};return $out
}
# 원본 JSON은 허용 필드만 정제합니다. 임의 기록 파일의 내용은 출력하지 않습니다.
$recordPaths=@((Join-Path $approvalRoot ('Entry-'+$package+'.json')),(Join-Path $approvalRoot ($approvalId+'.json')))
$consume=Join-Path $approvalRoot $approvalId
$null=Add-R2ProbeQuery 'R2_CONSUMPTION_CHILDREN' {Assert-R2ProbeNoReparse $consume;$children=@(Get-ChildItem -LiteralPath $consume -Force -ErrorAction Stop);if($children.Count -gt 128){throw 'R2_RECORD_BOUND'};foreach($i in $children){if(-not $i.PSIsContainer -and $i.Name -cmatch '\A(Result-[a-f0-9]{32}\.json|consumed\.json)\z'){$script:recordPaths+=$i.FullName};@{name=if($i.Name -cmatch '\A[A-Za-z0-9_.-]{1,120}\z'){$i.Name}else{'REDACTED_NAME'};directory=$i.PSIsContainer;length=if($i.PSIsContainer){$null}else{$i.Length}}}}
$null=Add-R2ProbeQuery 'APPROVAL_SIBLING_EVIDENCE_METADATA_ONLY' {Assert-R2ProbeNoReparse $approvalRoot;$children=@(Get-ChildItem -LiteralPath $approvalRoot -Force -ErrorAction Stop);if($children.Count -gt 128){throw 'R2_RECORD_BOUND'};foreach($i in $children){Read-R2ProbeFile $i.FullName}}
foreach($path in $recordPaths){$null=Add-R2ProbeQuery ('RECORD_'+[IO.Path]::GetFileName($path)) {$meta=Read-R2ProbeFile $path;if($meta.length -gt 2MB){throw 'R2_RECORD_SIZE_BOUND'};$raw=[IO.File]::ReadAllText($path)|ConvertFrom-Json -AsHashtable -Depth 24;@{metadata=$meta;sanitized=(Get-R2ProbeRecord $raw)}}}
$connect=Add-R2ProbeQuery 'TASK_COM_CONNECT' {$script:scheduler=New-Object -ComObject Schedule.Service;$script:scheduler.Connect();$true}
if($connect.status -eq 'READ_OK'){
 $folder=Add-R2ProbeQuery 'TASK_GET_ROOT_FOLDER' {$script:taskFolder=$script:scheduler.GetFolder('\');$true}
 if($folder.status -eq 'READ_OK'){
  $get=Add-R2ProbeQuery 'TASK_GET_EXACT' {$script:targetTask=$script:taskFolder.GetTask($taskName);$script:r.task_presence='PRESENT';@{name=$script:targetTask.Name;path=$script:targetTask.Path}}
  if($get.status -ne 'READ_OK' -and $get.failure -eq 'NOT_FOUND'){$r.task_presence='ABSENT'}
  $null=Add-R2ProbeQuery 'TASK_HIDDEN_ENUM_EXACT' {$all=$script:taskFolder.GetTasks(1);$found=@();foreach($t in $all){if($t.Path -ceq $taskPath){$found+=@{name=$t.Name;path=$t.Path}}};@{matching=$found;enumerated=$all.Count;enumeration_absence_not_proof=$true}}
  if($get.status -eq 'READ_OK'){
   $null=Add-R2ProbeQuery 'TASK_SECURITY_DESCRIPTOR' {Get-R2ProbeTaskSecurity $script:targetTask}
   $null=Add-R2ProbeQuery 'TASK_STATE_PRINCIPAL' {$d=$script:targetTask.Definition;@{enabled=$script:targetTask.Enabled;state=$script:targetTask.State;triggers=$d.Triggers.Count;user_id=$d.Principal.UserId;group_id=$d.Principal.GroupId;logon_type=$d.Principal.LogonType;run_level=$d.Principal.RunLevel;restarts=$d.Settings.RestartCount;multiple_instances=$d.Settings.MultipleInstances;seconds=[Xml.XmlConvert]::ToTimeSpan($d.Settings.ExecutionTimeLimit).TotalSeconds;registration_date_declared=$d.RegistrationInfo.Date;actual_registration_time='UNKNOWN_USE_EVENTS';last_run_time=$script:targetTask.LastRunTime.ToString('o');last_task_result=$script:targetTask.LastTaskResult}}
   $null=Add-R2ProbeQuery 'TASK_ACTION_PIN_COMPARE_NO_ARGUMENT_OUTPUT' {$d=$script:targetTask.Definition;if($d.Actions.Count -ne 1){throw 'R2_ACTION_COUNT'};$a=$d.Actions.Item(1);$bindingHash=(Get-FileHash -LiteralPath (Join-Path $code 'InstalledBinding.json') -ErrorAction Stop).Hash.ToLowerInvariant();$expected='-NoProfile -NonInteractive -File "'+(Join-Path $code 'Code/V2/start-host-v2.ps1')+'" -ManifestSha 7c8410145ed84f06fc9901e93f78e18d0cc103ad89757172e08c84f91d8a3976 -BindingSha '+$bindingHash;@{path_match=($a.Path -ceq 'C:\Program Files\PowerShell\7\pwsh.exe');working_directory_match=($a.WorkingDirectory -ceq $code);arguments_match=($a.Arguments -ceq $expected);host_metadata=(Read-R2ProbeFile (Join-Path $code 'Code/V2/start-host-v2.ps1'));executable_sha256=(Get-FileHash -LiteralPath 'C:\Program Files\PowerShell\7\pwsh.exe').Hash.ToLowerInvariant();executable_sha_baseline='NOT_PINNED_IN_PACKAGE'}}
   $null=Add-R2ProbeQuery 'TASK_XML_HASH_ONLY' {@{sha256=(Get-R2ProbeHash ([Text.Encoding]::UTF8.GetBytes([string]$script:targetTask.Xml)));raw_xml_not_saved=$true}}
  }
 }
}
foreach($name in @('LIMITLESSBotV2D','LIMITLESSWorkV2D')){
 $null=Add-R2ProbeQuery ('ACCOUNT_LOCALUSER_'+$name) {$u=Get-LocalUser -Name $name -ErrorAction Stop;@{sid=$u.SID.Value;enabled=$u.Enabled}}
 $null=Add-R2ProbeQuery ('ACCOUNT_NETAPI_'+$name) {$u=[R2PartialReadNative]::User($name);$expected=if($name -ceq 'LIMITLESSBotV2D'){'S-1-5-21-2858504392-1958168942-1662906075-1009'}else{'S-1-5-21-2858504392-1958168942-1662906075-1010'};@{facts=$u;expected_sid_match=($u['sid'] -ceq $expected);distinct_from_admin=($u['sid'] -cne $identity.User.Value)}}
 $null=Add-R2ProbeQuery ('ACCOUNT_GROUPS_'+$name) {@{effective_local_group_sids=@([R2PartialReadNative]::Groups($name));token_groups_verified=$false}}
 $sid=if($name -ceq 'LIMITLESSBotV2D'){'S-1-5-21-2858504392-1958168942-1662906075-1009'}else{'S-1-5-21-2858504392-1958168942-1662906075-1010'}
 $null=Add-R2ProbeQuery ('ACCOUNT_DIRECT_RIGHTS_'+$name) {$rights=@([R2PartialReadNative]::Rights($sid));@{sid=$sid;direct_rights=$rights;direct_batch=($rights -contains 'SeBatchLogonRight');effective_batch_token_verified=$false}}
}
$zipPath='F:\study\codex\Project-Limitless\Temp\Session0DedicatedV2Candidate20261027-r2.zip'
$pins=Add-R2ProbeQuery 'FIXED_R2_ARCHIVE_READ_DEFINITIONS_ONLY' {if((Get-FileHash -LiteralPath $zipPath).Hash.ToLowerInvariant() -cne '4efa0aa8a38b32fa40fa5a2dfb4042e028eeb07b1ab8d73c713a1d80efe028d0'){throw 'R2_ZIP_SHA'};$z=[IO.Compression.ZipFile]::OpenRead($zipPath);try{$e=$z.GetEntry('HostManifest.json');$s=$e.Open();$b=[IO.MemoryStream]::new();try{$s.CopyTo($b);$bytes=$b.ToArray()}finally{$s.Dispose();$b.Dispose()};if((Get-R2ProbeHash $bytes) -cne '7c8410145ed84f06fc9901e93f78e18d0cc103ad89757172e08c84f91d8a3976'){throw 'R2_MANIFEST_SHA'};$script:manifest=[Text.Encoding]::UTF8.GetString($bytes)|ConvertFrom-Json -AsHashtable;$p=$z.GetEntry('Deployment/Preservation.json');$reader=[IO.StreamReader]::new($p.Open());try{$script:preserve=$reader.ReadToEnd()|ConvertFrom-Json -AsHashtable}finally{$reader.Dispose()};@{package_id=$script:manifest.package_id;payload=$script:manifest.files_sha256.Count}}finally{$z.Dispose()}}
foreach($p in @($code,$state,(Join-Path $state 'Auth'),(Join-Path $state 'Auth/CodexHome'),(Join-Path $state 'Control'),(Join-Path $state 'Control/worker.lease'),$approvalRoot,$consume,(Join-Path $code 'InstalledBinding.json'))){$null=Add-R2ProbeQuery ('FILESYSTEM_'+$p) {Read-R2ProbeFile $p}}
$null=Add-R2ProbeQuery 'AUTH_EMPTY_NO_CONTENT_READ' {Assert-R2ProbeNoReparse (Join-Path $state 'Auth/CodexHome');@{entry_count=@(Get-ChildItem -LiteralPath (Join-Path $state 'Auth/CodexHome') -Force -ErrorAction Stop).Count}}
$null=Add-R2ProbeQuery 'LEASE_JOB_RECORD_NAMES_ONLY' {Assert-R2ProbeNoReparse (Join-Path $state 'Control');@(Get-ChildItem -LiteralPath (Join-Path $state 'Control') -Force -ErrorAction Stop|ForEach-Object {@{name=if($_.Name -cmatch '\A[A-Za-z0-9_.-]{1,100}\z'){$_.Name}else{'REDACTED_NAME'};directory=$_.PSIsContainer;bytes=if($_.PSIsContainer){$null}else{$_.Length}}})}
if($pins.status -eq 'READ_OK'){
 $null=Add-R2ProbeQuery 'INSTALLED_MANIFEST' {Read-R2ProbeFile (Join-Path $code 'HostManifest.json') '7c8410145ed84f06fc9901e93f78e18d0cc103ad89757172e08c84f91d8a3976'}
 $r.installed_files=@();foreach($n in $script:manifest.files_sha256.Keys){$q=Invoke-R2ProbeQuery ('PAYLOAD_'+$n) {Read-R2ProbeFile (Join-Path $code $n) $script:manifest.files_sha256[$n]};$r.installed_files+=,$q}
 $null=Add-R2ProbeQuery 'INSTALLED_CODE_ENTRY_COUNT' {Assert-R2ProbeNoReparse $code;$pending=[Collections.Generic.Stack[string]]::new();$pending.Push($code);$files=@();while($pending.Count){$dir=$pending.Pop();foreach($i in Get-ChildItem -LiteralPath $dir -Force -ErrorAction Stop){if($i.Attributes -band [IO.FileAttributes]::ReparsePoint){throw 'R2_REPARSE_HOLD'};if($i.PSIsContainer){$pending.Push($i.FullName)}else{$files+=$i.FullName.Substring($code.Length+1).Replace('\','/')}};if($files.Count -gt 2048){throw 'R2_CODE_ENTRY_BOUND'}};$expected=@($script:manifest.files_sha256.Keys)+@('HostManifest.json','InstalledBinding.json');@{files=$files.Count;expected=768;unexpected_count=@($files|Where-Object {$_ -cnotin $expected}).Count;missing_count=@($expected|Where-Object {$_ -cnotin $files}).Count}}
 if($connect.status -eq 'READ_OK'){foreach($name in $script:preserve.onboot.Keys){$null=Add-R2ProbeQuery ('ONBOOT_XML_PIN_'+$name) {$t=$script:scheduler.GetFolder('\').GetTask($name);@{name=$name;sha_match=((Get-R2ProbeHash ([Text.Encoding]::UTF8.GetBytes([string]$t.Xml))) -ceq $script:preserve.onboot[$name]);enabled=$t.Enabled}}}}
}
foreach($name in @('LIMITLESSRelaySvc','LIMITLESSBotTrial')){$null=Add-R2ProbeQuery ('OLD_ACCOUNT_NETAPI_'+$name) {[R2PartialReadNative]::User($name)}}
foreach($p in @('C:\Program Files\LimitlessRelayTrial','C:\Program Files\LimitlessRelayDedicatedV2Staging20261027\a5af3087b78b45d1baae46067d5b7ac9','C:\Program Files\LimitlessRelayDedicatedV2Staging20261027\3effaac30bf44760986fee9e6019b283')){$null=Add-R2ProbeQuery ('PRESERVED_ROOT_METADATA_'+$p) {Read-R2ProbeFile $p}}
$null=Add-R2ProbeQuery 'TASK_OPERATIONAL_LOG_CONFIGURATION' {$l=Get-WinEvent -ListLog 'Microsoft-Windows-TaskScheduler/Operational' -ErrorAction Stop;@{enabled=$l.IsEnabled;record_count=$l.RecordCount;log_mode=[string]$l.LogMode}}
$null=Add-R2ProbeQuery 'TASK_OPERATIONAL_EXACT_20261027_UTC' {$xpath="*[System[TimeCreated[@SystemTime >= '2026-10-26T15:00:00.000Z']]] and *[EventData[Data[@Name='TaskName']='$taskPath']]";$events=@(Get-WinEvent -LogName 'Microsoft-Windows-TaskScheduler/Operational' -FilterXPath $xpath -MaxEvents 128 -ErrorAction Stop);@{possibly_truncated=($events.Count -eq 128);events=@(foreach($e in $events){Get-R2ProbeEvent $e.ToXml() $taskPath});no_event_is_not_absence_proof=$true}}
$r.final=@{task_presence=$r.task_presence;historical_failure_cause='UNKNOWN_REVIEW_RECORDS_EVENTS_AND_CURRENT_SD';native_existing_preserved_claim=$true;independent_full_operational_audit=$false;old_855_success_scan_not_repeated=$true;job_objects_enumerated=$false;recovery_authorized=$false}
$output='F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\문서\00_프로젝트\팀운영\Evidence\DedicatedV2R2Readonly20261027\Admin-'+[guid]::NewGuid().ToString('N')+'.json'
$r.result_path=$output;$raw=[Text.UTF8Encoding]::new($false).GetBytes(($r|ConvertTo-Json -Depth 24));$f=[IO.File]::Open($output,[IO.FileMode]::CreateNew,[IO.FileAccess]::Write,[IO.FileShare]::None);try{$f.Write($raw)}finally{$f.Dispose()}
@{status=$r.status;result_path=$output;task_presence=$r.task_presence;queries=$r.queries.Count;changes=0;install_calls=0;ipc_calls=0;model_calls=0;automatic_retries=0}|ConvertTo-Json

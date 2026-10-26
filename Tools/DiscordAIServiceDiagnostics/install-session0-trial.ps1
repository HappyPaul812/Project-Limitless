param([ValidateSet('Plan','Install','Verify','StartFake','Stop','Remove')][string]$Action='Plan',
 [string]$Bundle,[ValidatePattern('^[a-f0-9]{64}$')][string]$BundleManifestSha,[switch]$ConfirmSystemChanges)
# 기본은 계획만 출력합니다. 시스템 변경은 대표의 별도 승인 뒤 명시적 switch가 있어야 가능합니다.
$ErrorActionPreference='Stop'
$install=Join-Path $env:ProgramFiles 'LimitlessRelayTrial'
$state=Join-Path $env:ProgramData 'LimitlessRelayTrial'
$account='LIMITLESSBotTrial';$taskName='LIMITLESS-Trial-S0Host';$marker='LIMITLESS_TRIAL_BOUNDARY_V1'
function Digest([byte[]]$bytes){return [Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($bytes)).ToLower()}
function Assert-TrialFreshState([bool]$CodeExists,[bool]$StateExists,[bool]$BotExists,[bool]$TaskExists){
 if($CodeExists -or $StateExists -or $BotExists -or $TaskExists){throw 'NEW_TRIAL_ONLY_NO_OVERWRITE'}
}
function Get-InstallFailureSummary($Record,[string]$Stage){
 $chain=@();$ex=$Record.Exception
 for($i=0;$null -ne $ex -and $i -lt 8;$i++){
  $node=[ordered]@{type=$ex.GetType().FullName;hresult=$ex.HResult}
  if($ex -is [ComponentModel.Win32Exception]){$node.win32=$ex.NativeErrorCode}
  if($ex.PSObject.Properties['NtStatus']){$node.ntstatus=[uint32]$ex.NtStatus}
  if($ex.PSObject.Properties['Operation'] -and $ex.Operation -in @('OPEN','ADD_RIGHT','CLOSE')){$node.operation=$ex.Operation}
  foreach($key in @('cleanup_ntstatus','cleanup_win32')){if($ex.Data.Contains($key)){$node[$key]=[uint32]$ex.Data[$key]}}
  $chain+=$node;$ex=$ex.InnerException
 }
 # 일반 오류 message/stack/InvocationInfo에는 입력값이 섞일 수 있어 기록하지 않습니다.
 return [ordered]@{schema=2;status='HOLD';stage=$Stage;summary='INSTALL_STEP_FAILED_MESSAGES_REDACTED';exceptions=$chain;automatic_retries=0}
}
function Assert-InstallLogSecurity([string]$Sddl){
 $sd=[Security.AccessControl.RawSecurityDescriptor]::new($Sddl)
 if($sd.Owner.Value -ne 'S-1-5-32-544' -or $sd.Group.Value -ne 'S-1-5-18' -or -not ($sd.ControlFlags -band [Security.AccessControl.ControlFlags]::DiscretionaryAclProtected) -or $sd.DiscretionaryAcl.Count -ne 2){throw 'PRIVATE_LOG_ACL_HOLD'}
 $seen=@{}
 foreach($ace in $sd.DiscretionaryAcl){
  if($ace -isnot [Security.AccessControl.CommonAce] -or $ace.IsCallback -or $ace.AceFlags -ne [Security.AccessControl.AceFlags]::None -or $ace.AceQualifier -ne [Security.AccessControl.AceQualifier]::AccessAllowed -or $ace.SecurityIdentifier.Value -notin @('S-1-5-18','S-1-5-32-544') -or $seen.ContainsKey($ace.SecurityIdentifier.Value) -or $ace.AccessMask -ne 2032127){throw 'PRIVATE_LOG_ACE_HOLD'}
  $seen[$ace.SecurityIdentifier.Value]=$true
 }
}
function Write-InstallFailureLog($Failure,[string]$Root){
 [LimitlessAtomicTrialPublishV1]::EnsureDirectory($Root,$Root)
 $path=Join-Path $Root ('InstallFailure-'+[guid]::NewGuid().ToString('N')+'.json')
 [LimitlessAtomicTrialPublishV1]::NewPrivateFile($path,[Text.UTF8Encoding]::new($false).GetBytes(($Failure|ConvertTo-Json -Depth 12)))
 Assert-InstallLogSecurity ((Get-Acl -LiteralPath $path).Sddl)
 return @{status='PROTECTED_LOG_VERIFIED';path=$path}
}
function Get-TrialTaskSddl([string]$WorkerSid){
 $sid=[Security.Principal.SecurityIdentifier]::new($WorkerSid)
 if($sid.Value -in @('S-1-5-18','S-1-5-32-544') -or -not $sid.IsAccountSid()){throw 'TASK_WORKER_SID_HOLD'}
 return 'O:BAG:SYD:P(A;;FA;;;SY)(A;;FA;;;BA)(A;;GR;;;'+$sid.Value+')'
}
function Assert-TrialTaskSecurity([string]$Sddl,[string]$WorkerSid){
 $null=Get-TrialTaskSddl $WorkerSid
 if(-not $Sddl){throw 'TASK_SD_MISSING_HOLD'}
 $sd=[Security.AccessControl.RawSecurityDescriptor]::new($Sddl)
 if($sd.Owner.Value -ne 'S-1-5-32-544' -or $sd.Group.Value -ne 'S-1-5-18'){throw 'TASK_OWNER_GROUP_HOLD'}
 $flags=[Security.AccessControl.ControlFlags]
 if(-not ($sd.ControlFlags -band $flags::DiscretionaryAclPresent) -or -not ($sd.ControlFlags -band $flags::DiscretionaryAclProtected) -or $null -eq $sd.DiscretionaryAcl -or $sd.DiscretionaryAcl.Count -ne 3){throw 'TASK_DACL_HOLD'}
 $seen=@{}
 foreach($ace in $sd.DiscretionaryAcl){
  if($ace -isnot [Security.AccessControl.CommonAce] -or $ace.IsCallback -or $ace.AceQualifier -ne [Security.AccessControl.AceQualifier]::AccessAllowed -or $ace.AceFlags -ne [Security.AccessControl.AceFlags]::None){throw 'TASK_ACE_KIND_HOLD'}
  $sid=$ace.SecurityIdentifier.Value
  if($seen.ContainsKey($sid) -or $sid -notin @('S-1-5-18','S-1-5-32-544',$WorkerSid)){throw 'TASK_ACE_SID_HOLD'}
  $seen[$sid]=$true
  # Task Scheduler의 실제 반환 mask를 추측하여 확대하지 않습니다. 알 수 없는 정규화는 HOLD입니다.
  $mask=[BitConverter]::ToUInt32([BitConverter]::GetBytes([int]$ace.AccessMask),0)
  $expected=if($sid -eq $WorkerSid){[uint32]2147483648}else{[uint32]2032127}
  if($mask -ne $expected){throw 'TASK_ACE_RIGHTS_HOLD'}
 }
}
function New-TrialTaskDefinition($Scheduler,[string]$WorkerSid,[string]$Arguments,[string]$WorkingDirectory,[string]$Description){
 $definition=$Scheduler.NewTask(0)
 $definition.RegistrationInfo.Description=$Description
 $definition.Principal.UserId=$WorkerSid
 $definition.Principal.LogonType=1 # TASK_LOGON_PASSWORD. Windows 실행 로그에서는 배치 logon4입니다.
 $definition.Principal.RunLevel=0
 $definition.Settings.Enabled=$false
 $definition.Settings.MultipleInstances=2 # TASK_INSTANCES_IGNORE_NEW
 $definition.Settings.ExecutionTimeLimit='PT15M'
 $definition.Settings.AllowHardTerminate=$true
 $definition.Settings.DisallowStartIfOnBatteries=$false
 $definition.Settings.StopIfGoingOnBatteries=$false
 $definition.Settings.RestartCount=0
 if($definition.Triggers.Count -ne 0){throw 'TASK_TRIGGER_HOLD'}
 $action=$definition.Actions.Create(0)
 $action.Path='C:/Program Files/PowerShell/7/pwsh.exe'
 $action.Arguments=$Arguments;$action.WorkingDirectory=$WorkingDirectory
 return $definition
}
function Register-TrialTaskAtomic($Folder,$Definition,[string]$Name,[string]$WorkerSid,[string]$UserName,[string]$Password,[hashtable]$Attempt){
 if($Attempt.attempted){throw 'TASK_ALREADY_ATTEMPTED_NO_RETRY'}
 if($Name -cne 'LIMITLESS-Trial-S0Host' -or $UserName -ine 'PAUL-PC\LIMITLESSRelaySvc' -or $Definition.Principal.UserId -ne $WorkerSid -or $Definition.Principal.LogonType -ne 1 -or $Definition.Principal.RunLevel -ne 0 -or $Definition.Settings.Enabled -ne $false -or $Definition.Triggers.Count -ne 0 -or $Definition.Settings.RestartCount -ne 0){throw 'TASK_DEFINITION_HOLD'}
 $sddl=Get-TrialTaskSddl $WorkerSid
 Assert-TrialTaskSecurity $sddl $WorkerSid
 # CREATE(2) + DONT_ADD_PRINCIPAL_ACE(16) + IGNORE_REGISTRATION_TRIGGERS(32). UPDATE는 포함하지 않습니다.
 # 예외나 응답 불명에도 재등록하지 않습니다. 기존 이름과의 충돌은 Scheduler의 CREATE가 거부합니다.
 $Attempt.attempted=$true
 $registered=$Folder.RegisterTaskDefinition($Name,$Definition,50,$UserName,$Password,1,$sddl)
 if($null -eq $registered){throw 'TASK_REGISTRATION_UNKNOWN_HOLD'}
 # 소유자(1)·그룹(2)·DACL(4)을 읽어 대조하며, 생성 후 권한을 수정하지 않습니다.
 Assert-TrialTaskSecurity ($registered.GetSecurityDescriptor(7)) $WorkerSid
 if($registered.Path -cne ('\'+$Name) -or $registered.Enabled -ne $false -or $registered.Definition.Triggers.Count -ne 0){throw 'TASK_REGISTERED_STATE_HOLD'}
 $Attempt.verified=$true
 return $registered
}
if($Action -eq 'Plan'){
 @{status='PLAN_ONLY';account=$account;groups=@('Users');new_right='SeBatchLogonRight';task=$taskName;task_enabled=$false;auto_start=$false;execution_mode='FAKE_ONLY';token_copy=$false;system_changes=0}|ConvertTo-Json
 return
}
$identity=[Security.Principal.WindowsIdentity]::GetCurrent()
if(-not ([Security.Principal.WindowsPrincipal]::new($identity)).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)){throw 'ADMIN_REQUIRED'}
if($Action -ne 'Verify' -and -not $ConfirmSystemChanges){throw 'SEPARATE_APPROVAL_AND_EXPLICIT_SWITCH_REQUIRED'}
function Private-Acl([string]$path,[string]$workerSid,[string]$botSid,[switch]$State){
 $acl=[Security.AccessControl.DirectorySecurity]::new();$acl.SetAccessRuleProtection($true,$false)
 $acl.SetOwner([Security.Principal.SecurityIdentifier]::new('S-1-5-32-544'))
 foreach($sid in @('S-1-5-18','S-1-5-32-544')){$acl.AddAccessRule([Security.AccessControl.FileSystemAccessRule]::new([Security.Principal.SecurityIdentifier]::new($sid),'FullControl','ContainerInherit,ObjectInherit','None','Allow'))}
 if($workerSid){$right=if($State){'Modify'}else{'ReadAndExecute'};$acl.AddAccessRule([Security.AccessControl.FileSystemAccessRule]::new([Security.Principal.SecurityIdentifier]::new($workerSid),$right,'ContainerInherit,ObjectInherit','None','Allow'))}
 if($botSid -and -not $State){$acl.AddAccessRule([Security.AccessControl.FileSystemAccessRule]::new([Security.Principal.SecurityIdentifier]::new($botSid),'ReadAndExecute','ContainerInherit,ObjectInherit','None','Allow'))}
 Set-Acl -LiteralPath $path -AclObject $acl
}
if($Action -eq 'Install'){
 if(-not $Bundle -or -not $BundleManifestSha){throw 'REVIEWED_BUNDLE_SHA_REQUIRED'}
 Assert-TrialFreshState (Test-Path -LiteralPath $install) (Test-Path -LiteralPath $state) ([bool](Get-LocalUser -Name $account -ErrorAction SilentlyContinue)) ([bool](Get-ScheduledTask -TaskName $taskName -ErrorAction SilentlyContinue))
 $bytes=[IO.File]::ReadAllBytes((Join-Path $Bundle 'HostManifest.json'))
 if((Digest $bytes) -ne $BundleManifestSha){throw 'BUNDLE_MANIFEST_SHA_HOLD'}
 $config=[Text.Encoding]::UTF8.GetString($bytes)|ConvertFrom-Json
 if($config.execution_mode -ne 'FAKE_ONLY' -or $config.auto_start -ne $false -or $config.bot_account -ne $account -or $config.task_name -ne $taskName){throw 'INSTALL_FAKE_DISABLED_ONLY'}
 $bootPath='C:/ProgramData/LimitlessRelay/boot-approval.json'
 if((Get-FileHash -LiteralPath $bootPath -Algorithm SHA256).Hash.ToLower() -ne '3e032ebcce3d91bd762a8dec81048d04607ddde761afd62489c0fa2b8e49140f'){throw 'BOOT_APPROVAL_HOLD'}
 $boot=Get-Content -LiteralPath $bootPath -Raw|ConvertFrom-Json
 # 생성과 동시에 BA 소유권을 설정합니다. 일반 사용자 소유 파일을 만든 뒤 권한을 고치는 창을 만들지 않습니다.
 Add-Type -TypeDefinition @'
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Security.AccessControl;
using Microsoft.Win32.SafeHandles;
public static class LimitlessAtomicTrialPublishV1 {
 [StructLayout(LayoutKind.Sequential)]struct Security{public int Length;public IntPtr Descriptor;public int Inherit;}
 [DllImport("advapi32.dll",CharSet=CharSet.Unicode,SetLastError=true)]static extern bool ConvertStringSecurityDescriptorToSecurityDescriptor(string text,uint version,out IntPtr descriptor,out uint size);
 [DllImport("kernel32.dll",CharSet=CharSet.Unicode,SetLastError=true)]static extern bool CreateDirectory(string path,ref Security security);
 [DllImport("kernel32.dll",CharSet=CharSet.Unicode,SetLastError=true)]static extern SafeFileHandle CreateFile(string path,uint access,uint share,ref Security security,uint creation,uint flags,IntPtr template);
 [DllImport("kernel32.dll")]static extern IntPtr LocalFree(IntPtr value);
 static void Admin(){using(var id=WindowsIdentity.GetCurrent()){if(!new WindowsPrincipal(id).IsInRole(WindowsBuiltInRole.Administrator))throw new UnauthorizedAccessException("ATOMIC_ADMIN_REQUIRED");}}
 static Security Descriptor(bool directory,out IntPtr memory,bool protect=false){
  uint size;string ace=directory?"OICI":"";
  if(!ConvertStringSecurityDescriptorToSecurityDescriptor("O:BAG:SYD:"+(protect?"P":"")+"(A;"+ace+";FA;;;SY)(A;"+ace+";FA;;;BA)",1,out memory,out size))throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
  return new Security{Length=Marshal.SizeOf<Security>(),Descriptor=memory,Inherit=0};
 }
 public static void NewDirectory(string path){
  Admin();IntPtr memory;var security=Descriptor(true,out memory);
  try{if(!CreateDirectory(path,ref security))throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());}finally{LocalFree(memory);}
 }
 public static void EnsureDirectory(string root,string path){
  Admin();root=Path.GetFullPath(root);path=Path.GetFullPath(path);
  if(path!=root && !path.StartsWith(root+Path.DirectorySeparatorChar,StringComparison.OrdinalIgnoreCase))throw new UnauthorizedAccessException("ATOMIC_SCOPE_HOLD");
  if(!Directory.Exists(path)) {EnsureDirectory(root,Path.GetDirectoryName(path));NewDirectory(path);}
  var item=new DirectoryInfo(path);
  if((item.Attributes&FileAttributes.ReparsePoint)!=0 || item.GetAccessControl().GetOwner(typeof(SecurityIdentifier)).Value!="S-1-5-32-544")throw new UnauthorizedAccessException("ATOMIC_PARENT_HOLD");
 }
 public static void NewFile(string path,byte[] data){WriteFile(path,data,false);}
 public static void NewPrivateFile(string path,byte[] data){WriteFile(path,data,true);}
 static void WriteFile(string path,byte[] data,bool protect){
  Admin();IntPtr memory;var security=Descriptor(false,out memory,protect);
  try{using(var handle=CreateFile(path,0x40000000,1,ref security,1,0x80,IntPtr.Zero)){
   if(handle.IsInvalid)throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
   using(var stream=new FileStream(handle,FileAccess.Write)){stream.Write(data,0,data.Length);stream.Flush(true);}
  }}finally{LocalFree(memory);}
 }
}
'@
 # 기존 암호/토큰을 읽지 않습니다. 필요한 암호는 별도 승인 후 이 로컬 입력에서만 받습니다.
 $botPassword=Read-Host '새 시험 봇 계정의 새 Windows 암호 (채팅에 보내지 마세요)' -AsSecureString
 $credential=Get-Credential -UserName 'PAUL-PC\LIMITLESSRelaySvc' -Message '기존 서비스 계정 암호를 로컬 보안 입력으로만 입력하세요. 인증파일/토큰은 복사하지 않습니다.'
 if($credential.UserName -ine 'PAUL-PC\LIMITLESSRelaySvc'){throw 'FIXED_SERVICE_ACCOUNT_REQUIRED'}
 $created=$null;$plain=$null;$installStage='CREATE_CODE_ROOT'
 try {
  [LimitlessAtomicTrialPublishV1]::NewDirectory($install);Private-Acl $install '' ''
  # hash를 확인한 바로 그 bytes를 씁니다. mutable source의 재복사/실행은 하지 않습니다.
  $installStage='PUBLISH_PAYLOAD'
  foreach($entry in $config.files_sha256.PSObject.Properties){
   if($entry.Name -notmatch '^(Code|Legacy|Runtime|Deployment)/' -or $entry.Name.Contains('..') -or $entry.Name.Contains('\') -or $entry.Name.Contains(':')){throw 'BUNDLE_PATH_HOLD'}
   $data=[IO.File]::ReadAllBytes((Join-Path $Bundle $entry.Name))
   if((Digest $data) -ne $entry.Value){throw 'BUNDLE_FILE_SHA_HOLD'}
   $target=[IO.Path]::GetFullPath((Join-Path $install $entry.Name))
   if(-not $target.StartsWith($install+[IO.Path]::DirectorySeparatorChar,[StringComparison]::OrdinalIgnoreCase)){throw 'INSTALL_SCOPE_HOLD'}
   [LimitlessAtomicTrialPublishV1]::EnsureDirectory($install,(Split-Path $target -Parent))
   [LimitlessAtomicTrialPublishV1]::NewFile($target,$data)
  }
  $installStage='CREATE_WORKSPACE'
  [LimitlessAtomicTrialPublishV1]::NewDirectory((Join-Path $install 'EmptyWorkspace'))
  $installStage='CREATE_BOT'
  $created=New-LocalUser -Name $account -Password $botPassword -Description $marker -AccountExpires (Get-Date).AddDays(1)
  $installStage='ADD_USERS_GROUP'
  Add-LocalGroupMember -SID 'S-1-5-32-545' -Member $created
  $installStage='CHECK_DISTINCT_SIDS'
  if($created.SID.Value -in @($boot.service_sid,$identity.User.Value)){throw 'DISTINCT_SID_HOLD'}
  $installStage='COMPILE_BATCH_POLICY'
  Add-Type -Path (Join-Path $install 'Code/Tools/DiscordAIServiceDiagnostics/TrialAccountPolicy.cs')
  $installStage='GRANT_BATCH_RIGHT'
  [LimitlessTrialAccountPolicyV1]::GrantNewBotBatch($created.SID.Value,$created.SID.Value,$boot.service_sid,$identity.User.Value)
  $installStage='BIND_MANIFEST'
  $config.bot_sid=$created.SID.Value;$config.worker_sid=$boot.service_sid;$config.admin_sid=$identity.User.Value;$config.state_root=$state
  $manifest=Join-Path $install 'HostManifest.json';$manifestBytes=[Text.UTF8Encoding]::new($false).GetBytes(($config|ConvertTo-Json -Depth 30));[LimitlessAtomicTrialPublishV1]::NewFile($manifest,$manifestBytes)
  $installStage='INSTALL_CODE_ACL'
  Private-Acl $install $config.worker_sid $config.bot_sid
  foreach($item in Get-ChildItem -LiteralPath $install -Recurse -Force){$acl=Get-Acl -LiteralPath $item.FullName;$acl.SetAccessRuleProtection($false,$true);Set-Acl -LiteralPath $item.FullName -AclObject $acl}
  $installStage='CREATE_STATE_ROOT'
  [LimitlessAtomicTrialPublishV1]::NewDirectory($state);Private-Acl $state $config.worker_sid '' -State
  $sha=Digest $manifestBytes
  if((Get-FileHash -LiteralPath $manifest -Algorithm SHA256).Hash.ToLower() -ne $sha){throw 'INSTALLED_MANIFEST_HOLD'}
  $script=Join-Path $install 'Code/Tools/DiscordAIServiceDiagnostics/start-session0-host.ps1'
  $arguments='-NoProfile -NonInteractive -File "'+$script+'" -ManifestSha '+$sha
  $installStage='DEFINE_TASK'
  $scheduler=New-Object -ComObject 'Schedule.Service';$scheduler.Connect()
  $definition=New-TrialTaskDefinition $scheduler $config.worker_sid $arguments $install $marker
  $plain=$credential.GetNetworkCredential().Password
  $attempt=@{attempted=$false;verified=$false}
  $installStage='REGISTER_TASK'
  $registered=Register-TrialTaskAtomic ($scheduler.GetFolder('\')) $definition $taskName $config.worker_sid $credential.UserName $plain $attempt
  $installStage='WRITE_DEPLOYMENT_RECORD'
  $record=@{marker=$marker;bot_sid=$config.bot_sid;installed_manifest_sha256=$sha;source_bundle_sha256=$BundleManifestSha;task=$taskName;execution_mode='FAKE_ONLY'}
  [LimitlessAtomicTrialPublishV1]::NewFile((Join-Path $install 'DeploymentRecord.json'),[Text.UTF8Encoding]::new($false).GetBytes(($record|ConvertTo-Json)))
  @{status='INSTALLED_DISABLED_FAKE_ONLY';manifest_sha256=$sha;task_enabled=$false;auto_start=$false;token_registered=$false}|ConvertTo-Json
 } catch {
  $failure=Get-InstallFailureSummary $_ $installStage
  if($created){try{Disable-LocalUser -SID $created.SID}catch{$failure.cleanup_failure=Get-InstallFailureSummary $_ 'DISABLE_NEW_BOT'}}
  try{$failure.private_log=Write-InstallFailureLog $failure $install}catch{$failure.private_log=@{status='UNAVAILABLE_OR_NOT_VERIFIED';error=(Get-InstallFailureSummary $_ 'WRITE_PRIVATE_LOG')}}
  $hold=[InvalidOperationException]::new('INSTALL_HOLD_NEW_TRIAL_ONLY_NO_RETRY_CHECK_PARTIAL_INSTALL')
  $hold.Data['LimitlessInstallFailureV2']=$failure
  throw $hold
 } finally {$plain=$null;$credential=$null;$botPassword=$null}
 return
}
$manifest=Join-Path $install 'HostManifest.json';$record=Get-Content -LiteralPath (Join-Path $install 'DeploymentRecord.json') -Raw|ConvertFrom-Json
if($record.marker -ne $marker -or (Get-FileHash -LiteralPath $manifest -Algorithm SHA256).Hash.ToLower() -ne $record.installed_manifest_sha256){throw 'INSTALLED_RECORD_HOLD'}
$config=Get-Content -LiteralPath $manifest -Raw|ConvertFrom-Json
if($config.execution_mode -ne 'FAKE_ONLY' -or $config.bot_sid -ne $record.bot_sid){throw 'FAKE_INSTALL_ONLY'}
$existing=Get-ScheduledTask -TaskName $taskName
if($existing.Description -ne $marker -or $existing.Actions[0].Arguments -notlike ('*'+$record.installed_manifest_sha256)){throw 'OWN_TASK_ONLY_HOLD'}
if($Action -eq 'Verify'){
 $scheduler=New-Object -ComObject 'Schedule.Service';$scheduler.Connect()
 Assert-TrialTaskSecurity ($scheduler.GetFolder('\').GetTask($taskName).GetSecurityDescriptor(7)) $config.worker_sid
 Add-Type -Path (Join-Path $install 'Code/Tools/DiscordAIServiceDiagnostics/Session0PipeBoundary.cs')
 [LimitlessSession0PipeV1]::VerifyProtectedPath($install,$false,$config.worker_sid)
 [LimitlessSession0PipeV1]::VerifyProtectedPath($state,$true,$config.worker_sid)
 if((Get-LocalUser -Name $account).SID.Value -ne $record.bot_sid){throw 'BOT_SID_HOLD'}
 @{status='LOCAL_INSTALL_VERIFY_ONLY';disabled=(-not $existing.Settings.Enabled);triggers=@($existing.Triggers).Count;mode='FAKE_ONLY';real_ipc_verified=$false}|ConvertTo-Json
} elseif($Action -eq 'StartFake'){
 $scheduler=New-Object -ComObject 'Schedule.Service';$scheduler.Connect()
 Assert-TrialTaskSecurity ($scheduler.GetFolder('\').GetTask($taskName).GetSecurityDescriptor(7)) $config.worker_sid
 if(@($existing.Triggers).Count -ne 0){throw 'NO_AUTOSTART_TRIGGERS_ALLOWED'}
 Enable-ScheduledTask -TaskName $taskName|Out-Null;Start-ScheduledTask -TaskName $taskName
 'FAKE_HOST_STARTED_ONCE_NO_AI_NO_DISCORD'
} elseif($Action -in @('Stop','Remove')){
 $tree=@();$known=$false
 $readyPath=Join-Path $state 'host-ready.safe.json'
 if(Test-Path -LiteralPath $readyPath){
  $ready=Get-Content -LiteralPath $readyPath -Raw|ConvertFrom-Json
  $processes=@(Get-CimInstance Win32_Process)
  $rootProcess=$processes|Where-Object{$_.ProcessId -eq $ready.host_pid}
  if($rootProcess){
   if(-not $rootProcess.CommandLine.Contains($record.installed_manifest_sha256)){throw 'STOP_HOST_PID_IDENTITY_HOLD'}
  }
  $tree=@([int]$ready.host_pid)
  do{$next=@($processes|Where-Object{$_.ParentProcessId -in $tree -and $_.ProcessId -notin $tree}|ForEach-Object{[int]$_.ProcessId});$tree+=$next}while($next.Count)
  $known=$true
 }
 Stop-ScheduledTask -TaskName $taskName -ErrorAction SilentlyContinue;Disable-ScheduledTask -TaskName $taskName|Out-Null
 if($known){
  $watch=[Diagnostics.Stopwatch]::StartNew()
  do{$remaining=@($tree|Where-Object{Get-Process -Id $_ -ErrorAction SilentlyContinue});if($remaining.Count){Start-Sleep -Milliseconds 100}}while($remaining.Count -and $watch.Elapsed.TotalSeconds -lt 10)
  if($remaining.Count){throw 'STOP_REMAINING_PROCESSES_HOLD'}
 }
 if($Action -eq 'Remove'){
  Unregister-ScheduledTask -TaskName $taskName -Confirm:$false
  if((Get-LocalUser -Name $account).SID.Value -ne $record.bot_sid){throw 'REMOVE_NEW_BOT_ONLY_HOLD'}
  Disable-LocalUser -SID $record.bot_sid
 }
 # 증거/소비/프로필을 삭제하지 않습니다. 기존 task/서비스에는 접근하지 않습니다.
 @{status='TRIAL_STOPPED_RECORDS_PRESERVED';process_tree_zero_verified=$known;recorded_processes=$tree.Count}|ConvertTo-Json
}

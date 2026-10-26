param([ValidateSet('Plan','Install','Verify','StartFake','Stop','Remove')][string]$Action='Plan',
 [string]$Bundle,[ValidatePattern('^[a-f0-9]{64}$')][string]$BundleManifestSha,[switch]$ConfirmSystemChanges)
# 기본은 계획만 출력합니다. 시스템 변경은 대표의 별도 승인 뒤 명시적 switch가 있어야 가능합니다.
$ErrorActionPreference='Stop'
$install=Join-Path $env:ProgramFiles 'LimitlessRelayTrial'
$state=Join-Path $env:ProgramData 'LimitlessRelayTrial'
$account='LIMITLESSBotTrial';$taskName='LIMITLESS-Trial-S0Host';$marker='LIMITLESS_TRIAL_BOUNDARY_V1'
function Digest([byte[]]$bytes){return [Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($bytes)).ToLower()}
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
 if((Test-Path -LiteralPath $install) -or (Test-Path -LiteralPath $state) -or (Get-LocalUser -Name $account -ErrorAction SilentlyContinue) -or (Get-ScheduledTask -TaskName $taskName -ErrorAction SilentlyContinue)){throw 'NEW_TRIAL_ONLY_NO_OVERWRITE'}
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
 static Security Descriptor(bool directory,out IntPtr memory){
  uint size;string ace=directory?"OICI":"";
  if(!ConvertStringSecurityDescriptorToSecurityDescriptor("O:BAG:SYD:(A;"+ace+";FA;;;SY)(A;"+ace+";FA;;;BA)",1,out memory,out size))throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
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
 public static void NewFile(string path,byte[] data){
  Admin();IntPtr memory;var security=Descriptor(false,out memory);
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
 $created=$null;$plain=$null
 try {
  [LimitlessAtomicTrialPublishV1]::NewDirectory($install);Private-Acl $install '' ''
  # hash를 확인한 바로 그 bytes를 씁니다. mutable source의 재복사/실행은 하지 않습니다.
  foreach($entry in $config.files_sha256.PSObject.Properties){
   if($entry.Name -notmatch '^(Code|Legacy|Runtime|Deployment)/' -or $entry.Name.Contains('..') -or $entry.Name.Contains('\') -or $entry.Name.Contains(':')){throw 'BUNDLE_PATH_HOLD'}
   $data=[IO.File]::ReadAllBytes((Join-Path $Bundle $entry.Name))
   if((Digest $data) -ne $entry.Value){throw 'BUNDLE_FILE_SHA_HOLD'}
   $target=[IO.Path]::GetFullPath((Join-Path $install $entry.Name))
   if(-not $target.StartsWith($install+[IO.Path]::DirectorySeparatorChar,[StringComparison]::OrdinalIgnoreCase)){throw 'INSTALL_SCOPE_HOLD'}
   [LimitlessAtomicTrialPublishV1]::EnsureDirectory($install,(Split-Path $target -Parent))
   [LimitlessAtomicTrialPublishV1]::NewFile($target,$data)
  }
  [LimitlessAtomicTrialPublishV1]::NewDirectory((Join-Path $install 'EmptyWorkspace'))
  $created=New-LocalUser -Name $account -Password $botPassword -Description $marker -AccountExpires (Get-Date).AddDays(1)
  Add-LocalGroupMember -SID 'S-1-5-32-545' -Member $created
  if($created.SID.Value -in @($boot.service_sid,$identity.User.Value)){throw 'DISTINCT_SID_HOLD'}
  Add-Type -Path (Join-Path $install 'Code/Tools/DiscordAIServiceDiagnostics/TrialAccountPolicy.cs')
  [LimitlessTrialAccountPolicyV1]::GrantNewBotBatch($created.SID.Value,$created.SID.Value,$boot.service_sid,$identity.User.Value)
  $config.bot_sid=$created.SID.Value;$config.worker_sid=$boot.service_sid;$config.admin_sid=$identity.User.Value;$config.state_root=$state
  $manifest=Join-Path $install 'HostManifest.json';$manifestBytes=[Text.UTF8Encoding]::new($false).GetBytes(($config|ConvertTo-Json -Depth 30));[LimitlessAtomicTrialPublishV1]::NewFile($manifest,$manifestBytes)
  Private-Acl $install $config.worker_sid $config.bot_sid
  foreach($item in Get-ChildItem -LiteralPath $install -Recurse -Force){$acl=Get-Acl -LiteralPath $item.FullName;$acl.SetAccessRuleProtection($false,$true);Set-Acl -LiteralPath $item.FullName -AclObject $acl}
  [LimitlessAtomicTrialPublishV1]::NewDirectory($state);Private-Acl $state $config.worker_sid '' -State
  $sha=Digest $manifestBytes
  if((Get-FileHash -LiteralPath $manifest -Algorithm SHA256).Hash.ToLower() -ne $sha){throw 'INSTALLED_MANIFEST_HOLD'}
  $script=Join-Path $install 'Code/Tools/DiscordAIServiceDiagnostics/start-session0-host.ps1'
  $arguments='-NoProfile -NonInteractive -File "'+$script+'" -ManifestSha '+$sha
  $taskAction=New-ScheduledTaskAction -Execute 'C:/Program Files/PowerShell/7/pwsh.exe' -Argument $arguments -WorkingDirectory $install
  $principal=New-ScheduledTaskPrincipal -UserId $config.worker_sid -LogonType Password -RunLevel Limited
  $settings=New-ScheduledTaskSettingsSet -Disable -MultipleInstances IgnoreNew -ExecutionTimeLimit (New-TimeSpan -Minutes 15) -AllowStartIfOnBatteries -DontStopIfGoingOnBatteries
  $settings.RestartCount=0
  $definition=New-ScheduledTask -Action $taskAction -Principal $principal -Settings $settings -Description $marker
  $plain=$credential.GetNetworkCredential().Password
  $null=Register-ScheduledTask -TaskName $taskName -InputObject $definition -User $credential.UserName -Password $plain
  $scheduler=New-Object -ComObject 'Schedule.Service';$scheduler.Connect();$registered=$scheduler.GetFolder('\').GetTask($taskName)
  $registered.SetSecurityDescriptor(('O:BAG:SYD:P(A;;FA;;;SY)(A;;FA;;;BA)(A;;GR;;;'+$config.worker_sid+')'),0)
  $record=@{marker=$marker;bot_sid=$config.bot_sid;installed_manifest_sha256=$sha;source_bundle_sha256=$BundleManifestSha;task=$taskName;execution_mode='FAKE_ONLY'}
  [LimitlessAtomicTrialPublishV1]::NewFile((Join-Path $install 'DeploymentRecord.json'),[Text.UTF8Encoding]::new($false).GetBytes(($record|ConvertTo-Json)))
  @{status='INSTALLED_DISABLED_FAKE_ONLY';manifest_sha256=$sha;task_enabled=$false;auto_start=$false;token_registered=$false}|ConvertTo-Json
 } catch {
  if($created){Disable-LocalUser -SID $created.SID}
  throw 'INSTALL_HOLD_NEW_TRIAL_ONLY_NO_RETRY_CHECK_PARTIAL_INSTALL'
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
 Add-Type -Path (Join-Path $install 'Code/Tools/DiscordAIServiceDiagnostics/Session0PipeBoundary.cs')
 [LimitlessSession0PipeV1]::VerifyProtectedPath($install,$false,$config.worker_sid)
 [LimitlessSession0PipeV1]::VerifyProtectedPath($state,$true,$config.worker_sid)
 if((Get-LocalUser -Name $account).SID.Value -ne $record.bot_sid){throw 'BOT_SID_HOLD'}
 @{status='LOCAL_INSTALL_VERIFY_ONLY';disabled=(-not $existing.Settings.Enabled);triggers=@($existing.Triggers).Count;mode='FAKE_ONLY';real_ipc_verified=$false}|ConvertTo-Json
} elseif($Action -eq 'StartFake'){
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

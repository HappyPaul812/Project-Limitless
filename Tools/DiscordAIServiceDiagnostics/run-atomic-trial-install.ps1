param([ValidateSet('Preflight','Install','Inspect')][string]$Action='Preflight')
# 이번 승인의 설치/읽기 전용 검증만 제공합니다. Host 시작·IPC·AI·Discord 동작은 없습니다.
$ErrorActionPreference='Stop'
$bundle='F:/study/codex/Project-Limitless/Temp/Session0TaskSecurityAtomicCandidate20261026'
$repo='F:/study/codex/Project-Limitless/Temp/DiscordUsageDiagnostics20261026/Worktree'
$install=Join-Path $env:ProgramFiles 'LimitlessRelayTrial'
$state=Join-Path $env:ProgramData 'LimitlessRelayTrial'
$bootPath='C:/ProgramData/LimitlessRelay/boot-approval.json'
$expectedInstaller='117065bbfca686474f9104bc0dff20fa6528dfb9c204e5a2f2720d91994958e9'
$expectedManifest='6efbd7839bd4e452d9fbca8a465290533472e87cde71845a73bd37bdb21493ab'
$expectedZip='4b2c56edf5bb8edc6ee7a40a10504f537223bc77874b43a36e61be735c318f83'
$expectedBoot='3e032ebcce3d91bd762a8dec81048d04607ddde761afd62489c0fa2b8e49140f'
$taskName='LIMITLESS-Trial-S0Host'
$result=[ordered]@{status='HOLD';stage='PACKAGE_PREFLIGHT';action=$Action;install_attempts=0;automatic_retries=0;host_starts=0;ipc_calls=0;fake_worker_calls=0;model_calls=0;official_usage_reads=0;discord_calls=0;sqlite_operations=0;mode='MANUAL_UNCHANGED';a_mode='HOLD_UNCHANGED';request_id=([guid]::NewGuid().ToString('N'))}
function Hash-Bytes([byte[]]$Bytes){[Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($Bytes)).ToLower()}
function Require($Value,[string]$Code){if(-not $Value){throw $Code}}
function Plain-File([string]$Path){
 $p=Get-Item -LiteralPath $Path -Force
 Require (-not $p.PSIsContainer -and -not ($p.Attributes -band [IO.FileAttributes]::ReparsePoint)) 'FILE_REPARSE_HOLD'
 return ,([IO.File]::ReadAllBytes($Path))
}
function Boot-TaskHashes {
 $output=@{}
 foreach($name in @('LIMITLESS-Bot-OnBoot','LIMITLESS-Art-OnBoot')){
  # 원본 XML은 저장하거나 표시하지 않습니다. 인수에 비밀정보가 있어도 hash만 기록합니다.
  $xml=Export-ScheduledTask -TaskName $name -TaskPath '\'
  Require ([bool]$xml) 'BOOT_TASK_EXPORT_HOLD'
  $output[$name]=Hash-Bytes ([Text.Encoding]::UTF8.GetBytes($xml))
 }
 return $output
}
function Read-NewBotRights([string]$Sid){
 # 읽기 전용 LSA handle만 엽니다. 승인 설치기가 부여한 신규 SID의 직접 권한을 대조합니다.
 if(-not ('LimitlessTrialRightsReadOnlyV1' -as [type])){
 Add-Type -TypeDefinition @'
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Principal;
public static class LimitlessTrialRightsReadOnlyV1 {
 [StructLayout(LayoutKind.Sequential)]struct Text {public ushort Length,Max;public IntPtr Buffer;}
 [StructLayout(LayoutKind.Sequential)]struct Attr {public int Length;public IntPtr Root,Object;public uint Flags;public IntPtr Descriptor,Quality;}
 [DllImport("advapi32.dll")]static extern uint LsaOpenPolicy(IntPtr name,ref Attr attr,uint access,out IntPtr handle);
 [DllImport("advapi32.dll")]static extern uint LsaEnumerateAccountRights(IntPtr handle,byte[] sid,out IntPtr rights,out uint count);
 [DllImport("advapi32.dll")]static extern uint LsaFreeMemory(IntPtr value);
 [DllImport("advapi32.dll")]static extern uint LsaClose(IntPtr handle);
 [DllImport("advapi32.dll")]static extern uint LsaNtStatusToWinError(uint status);
 public static string[] Read(string sid) {
  var identifier=new SecurityIdentifier(sid);var bytes=new byte[identifier.BinaryLength];identifier.GetBinaryForm(bytes,0);
  IntPtr handle=IntPtr.Zero,rights=IntPtr.Zero;
  try {
   var attr=new Attr{Length=Marshal.SizeOf<Attr>()};uint status=LsaOpenPolicy(IntPtr.Zero,ref attr,0x800,out handle);
   if(status!=0)throw new Win32Exception((int)LsaNtStatusToWinError(status));
   uint count;status=LsaEnumerateAccountRights(handle,bytes,out rights,out count);
   if(status!=0)throw new Win32Exception((int)LsaNtStatusToWinError(status));
   var result=new List<string>();int size=Marshal.SizeOf<Text>();
   for(int i=0;i<count;i++){var text=Marshal.PtrToStructure<Text>(IntPtr.Add(rights,checked(i*size)));result.Add(Marshal.PtrToStringUni(text.Buffer,text.Length/2));}
   return result.ToArray();
  }finally{if(rights!=IntPtr.Zero)LsaFreeMemory(rights);if(handle!=IntPtr.Zero)LsaClose(handle);}
 }
}
'@
 }
 return [LimitlessTrialRightsReadOnlyV1]::Read($Sid)
}
function Read-Installed {
 param($Config,$Boot,$Identity,$ApprovedScript)
 $record=Get-Content -LiteralPath (Join-Path $install 'DeploymentRecord.json') -Raw|ConvertFrom-Json
 Require ($record.marker -ceq 'LIMITLESS_TRIAL_BOUNDARY_V1' -and $record.source_bundle_sha256 -ceq $expectedManifest -and $record.task -ceq $taskName) 'DEPLOYMENT_RECORD_HOLD'
 $manifestBytes=Plain-File (Join-Path $install 'HostManifest.json')
 Require ((Hash-Bytes $manifestBytes) -ceq $record.installed_manifest_sha256) 'INSTALLED_MANIFEST_HOLD'
 $installed=[Text.Encoding]::UTF8.GetString($manifestBytes)|ConvertFrom-Json
 Require ($installed.execution_mode -ceq 'FAKE_ONLY' -and $installed.auto_start -eq $false -and $installed.worker_sid -ceq $Boot.service_sid -and $installed.admin_sid -ceq $Identity.User.Value -and $installed.bot_sid -ceq $record.bot_sid) 'INSTALLED_SCOPE_HOLD'
 $bot=Get-LocalUser -Name 'LIMITLESSBotTrial'
 $worker=Get-LocalUser -Name 'LIMITLESSRelaySvc'
 Require ($bot.SID.Value -ceq $record.bot_sid -and $worker.SID.Value -ceq $Boot.service_sid -and @($bot.SID.Value,$worker.SID.Value,$Identity.User.Value|Select-Object -Unique).Count -eq 3) 'DISTINCT_OS_SIDS_HOLD'
 $groups=@(Get-LocalGroup|ForEach-Object{$g=$_;if(@(Get-LocalGroupMember -SID $g.SID|Where-Object{$_.SID.Value -ceq $bot.SID.Value}).Count){$g.SID.Value}})
 Require ($groups.Count -eq 1 -and $groups[0] -ceq 'S-1-5-32-545') 'USERS_ONLY_HOLD'
 $rights=@(Read-NewBotRights $bot.SID.Value)
 Require ($rights.Count -eq 1 -and $rights[0] -ceq 'SeBatchLogonRight') 'NEW_BOT_DIRECT_RIGHTS_HOLD'
 Require ($null -ne $bot.AccountExpires -and $bot.AccountExpires -gt (Get-Date) -and $bot.AccountExpires -le (Get-Date).AddHours(24).AddMinutes(2)) 'ACCOUNT_EXPIRY_HOLD'
 Require ($bot.Enabled -eq $true) 'BOT_ACCOUNT_DISABLED_HOLD'
 $mismatches=@()
 foreach($entry in $Config.files_sha256.PSObject.Properties){
  $path=Join-Path $install $entry.Name
  if((Hash-Bytes (Plain-File $path)) -cne $entry.Value){$mismatches+=$entry.Name}
 }
 Require ($mismatches.Count -eq 0) 'INSTALLED_PAYLOAD_SHA_HOLD'
 Require (@(Get-ChildItem -LiteralPath $install -Recurse -File -Force).Count -eq 857) 'INSTALLED_EXACT_SCOPE_HOLD'
 # 승인된 설치기의 Verify는 실제 SD/설치 코드·상태 경로 ACL을 읽기만 합니다.
 $verify=& $ApprovedScript -Action Verify
 $verified=($verify -join "`n")|ConvertFrom-Json
 Require ($verified.status -ceq 'LOCAL_INSTALL_VERIFY_ONLY' -and $verified.disabled -eq $true -and $verified.triggers -eq 0 -and $verified.mode -ceq 'FAKE_ONLY') 'INSTALLER_VERIFY_HOLD'
 $scheduler=New-Object -ComObject 'Schedule.Service';$scheduler.Connect()
 $task=$scheduler.GetFolder('\').GetTask($taskName)
 $definition=$task.Definition
 $actualPrincipal=if($definition.Principal.UserId.StartsWith('S-1-')){$definition.Principal.UserId}else{([Security.Principal.NTAccount]::new($definition.Principal.UserId)).Translate([Security.Principal.SecurityIdentifier]).Value}
 Require ($actualPrincipal -ceq $worker.SID.Value -and $definition.Principal.LogonType -eq 1 -and $definition.Principal.RunLevel -eq 0 -and $task.Enabled -eq $false -and $definition.Triggers.Count -eq 0 -and $definition.Settings.RestartCount -eq 0 -and $definition.Actions.Count -eq 1) 'TASK_PRINCIPAL_SETTINGS_HOLD'
 $exec=$definition.Actions.Item(1)
 $fixedScript=Join-Path $install 'Code/Tools/DiscordAIServiceDiagnostics/start-session0-host.ps1'
 $fixedArguments='-NoProfile -NonInteractive -File "'+$fixedScript+'" -ManifestSha '+$record.installed_manifest_sha256
 Require ($exec.Path -ieq 'C:/Program Files/PowerShell/7/pwsh.exe' -and $exec.Arguments -ceq $fixedArguments -and $exec.WorkingDirectory -ieq $install) 'TASK_FIXED_ACTION_HOLD'
 $sddl=$task.GetSecurityDescriptor(7)
 $sd=[Security.AccessControl.RawSecurityDescriptor]::new($sddl)
 return [ordered]@{bot_sid=$bot.SID.Value;worker_sid=$worker.SID.Value;admin_sid=$Identity.User.Value;distinct_sids=$true;bot_local_group_sids=$groups;bot_direct_rights=$rights;account_expires_utc=$bot.AccountExpires.ToUniversalTime().ToString('o');installed_payload_files=855;installed_payload_sha_mismatches=0;installed_manifest_sha256=$record.installed_manifest_sha256;task_path=$task.Path;task_enabled=$task.Enabled;triggers=$definition.Triggers.Count;task_logon_type=1;windows_logon4_configured=$true;actual_session0_run_observed=$false;owner_sid=$sd.Owner.Value;group_sid=$sd.Group.Value;task_sddl=$sddl;execution_mode='FAKE_ONLY';code_acl_verified=$true;state_acl_verified=$true;actual_ipc_verified=$false}
}
$before=$null;$approvedScript=$null
try {
 $manifestBytes=Plain-File (Join-Path $bundle 'HostManifest.json')
 $installerBytes=Plain-File (Join-Path $bundle 'Deployment/install-session0-trial.ps1')
 Require ((Hash-Bytes $manifestBytes) -ceq $expectedManifest -and (Hash-Bytes $installerBytes) -ceq $expectedInstaller -and (Hash-Bytes (Plain-File ($bundle+'.zip'))) -ceq $expectedZip) 'APPROVED_PACKAGE_SHA_HOLD'
 $config=[Text.Encoding]::UTF8.GetString($manifestBytes)|ConvertFrom-Json
 $entries=@($config.files_sha256.PSObject.Properties)
 Require ($entries.Count -eq 855 -and @(Get-ChildItem -LiteralPath $bundle -Recurse -File -Force).Count -eq 856) 'EXACT855_HOLD'
 foreach($entry in $entries){
  Require ($entry.Name -cmatch '^(Code|Runtime|Legacy|Deployment)/' -and -not $entry.Name.Contains('..') -and -not $entry.Name.Contains(':') -and -not $entry.Name.Contains([char]92)) 'PACKAGE_PATH_HOLD'
  Require ((Hash-Bytes (Plain-File (Join-Path $bundle $entry.Name))) -ceq $entry.Value) 'PACKAGE_FILE_SHA_HOLD'
 }
 $oldManifestBytes=Plain-File 'F:/study/codex/Project-Limitless/Temp/Session0HostBundle20261026Ready/HostManifest.json'
 Require ((Hash-Bytes $oldManifestBytes) -ceq '9aa4a60d6dd57c320f2d268ef96dcf86694e53ca9dfdcbb6a3e97d1f33dfe7ec') 'ORIGINAL_MANIFEST_HOLD'
 $original=([Text.Encoding]::UTF8.GetString($oldManifestBytes)|ConvertFrom-Json).files_sha256
 $changed=@($entries|Where-Object{$original.($_.Name) -cne $_.Value})
 Require ($changed.Count -eq 1 -and $changed[0].Name -ceq 'Deployment/install-session0-trial.ps1' -and $config.execution_mode -ceq 'FAKE_ONLY' -and $config.auto_start -eq $false) 'EXACT854_PRESERVED_HOLD'
 $result.package='PASS_855_EXACT_854_PRESERVED';$result.installer_sha256=$expectedInstaller;$result.manifest_sha256=$expectedManifest;$result.zip_sha256=$expectedZip
 $identity=[Security.Principal.WindowsIdentity]::GetCurrent()
 $admin=([Security.Principal.WindowsPrincipal]::new($identity)).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
 $result.parent_account=$identity.Name;$result.parent_sid=$identity.User.Value;$result.parent_session_id=(Get-Process -Id $PID).SessionId;$result.parent_is_admin=$admin
 if($Action -eq 'Preflight'){$result.status='PACKAGE_PASS_ADMIN_LOCAL_INPUT_REQUIRED';$result.stage='WAIT_ADMIN_INSTALL';return}
 Require $admin 'ADMIN_LOCAL_SECURE_INPUT_REQUIRED'
 $result.stage='ADMIN_PREFLIGHT'
 Require ((Hash-Bytes (Plain-File $bootPath)) -ceq $expectedBoot) 'BOOT_APPROVAL_HOLD'
 $boot=Get-Content -LiteralPath $bootPath -Raw|ConvertFrom-Json
 $worker=Get-LocalUser -Name 'LIMITLESSRelaySvc'
 Require ($worker.SID.Value -ceq $boot.service_sid -and $identity.User.Value -cne $worker.SID.Value) 'WORKER_ADMIN_OS_SID_HOLD'
 $before=Boot-TaskHashes
 $result.boot_tasks_before=$before
 $approvedScript=[scriptblock]::Create([Text.Encoding]::UTF8.GetString($installerBytes))
 if($Action -eq 'Install'){
  Require (-not (Test-Path -LiteralPath $install) -and -not (Test-Path -LiteralPath $state) -and -not (Get-LocalUser -Name 'LIMITLESSBotTrial' -ErrorAction SilentlyContinue)) 'NEW_INSTALL_ONLY_HOLD'
  $service=New-Object -ComObject 'Schedule.Service';$service.Connect()
  $names=@($service.GetFolder('\').GetTasks(1)|ForEach-Object{$_.Name})
  Require ($taskName -notin $names) 'EXISTING_TASK_NO_OVERWRITE_HOLD'
  $result.stage='APPROVED_INSTALL_ONCE';$result.install_attempts=1
  # hash를 확인한 bytes만 실행합니다. 승인된 설치기는 수정하지 않습니다.
  $messages=& $approvedScript -Action Install -Bundle $bundle -BundleManifestSha $expectedManifest -ConfirmSystemChanges
  $installed=($messages -join "`n")|ConvertFrom-Json
  Require ($installed.status -ceq 'INSTALLED_DISABLED_FAKE_ONLY') 'INSTALL_RESULT_UNKNOWN_HOLD'
 }
 $result.stage='READ_ONLY_POST_INSTALL_VERIFY'
 $result.installed=Read-Installed $config $boot $identity $approvedScript
 $result.status='INSTALL_PASS_DISABLED_FAKE_ONLY_NO_EXECUTION';$result.stage='COMPLETE'
} catch {
 $result.status='HOLD';$result.error_type=$_.Exception.GetType().Name;$result.hresult=$_.Exception.HResult
 # 오류 본문/스택에 credential이 섞일 수 있으므로 출력하거나 저장하지 않습니다.
 if($_.Exception.Message -cmatch '^[A-Z][A-Z0-9_]{2,100}$'){$result.safe_error_code=$_.Exception.Message}
 $result.reason='NO_RETRY_NO_AUTOMATIC_DELETE_REVIEW_PARTIAL_STATE'
 if($result.install_attempts -eq 1){
  $result.partial_install_path_exists=Test-Path -LiteralPath $install
  $result.partial_state_path_exists=Test-Path -LiteralPath $state
  try{$u=Get-LocalUser -Name 'LIMITLESSBotTrial' -ErrorAction Stop;$result.partial_bot=@{sid=$u.SID.Value;enabled=$u.Enabled}}catch{$result.partial_bot_read='UNKNOWN_OR_ABSENT'}
  try{
   $s=New-Object -ComObject 'Schedule.Service';$s.Connect();$t=$s.GetFolder('\').GetTask($taskName)
   $result.partial_task_unverified=@{path=$t.Path;description=$t.Definition.RegistrationInfo.Description;enabled=$t.Enabled;principal=$t.Definition.Principal.UserId;logon_type=$t.Definition.Principal.LogonType;sddl=$t.GetSecurityDescriptor(7)}
  }catch{$result.partial_task_read='UNKNOWN_OR_ABSENT'}
 }
} finally {
 if($before){
  try{
   $after=Boot-TaskHashes;$result.boot_tasks_after=$after
   $result.boot_tasks_unchanged=(@($before.Keys|Where-Object{$before[$_] -cne $after[$_]}).Count -eq 0)
   $result.approval_unchanged=((Hash-Bytes (Plain-File $bootPath)) -ceq $expectedBoot)
   if(-not $result.boot_tasks_unchanged -or -not $result.approval_unchanged){$result.status='HOLD';$result.stage='OPERATIONAL_PRESERVATION_HOLD'}
  }catch{$result.status='HOLD';$result.stage='OPERATIONAL_READBACK_UNKNOWN_HOLD'}
 }
 $receiptRoot=Join-Path $repo '문서/00_프로젝트/팀운영/Evidence/Session0AtomicInstall20261026'
 $null=New-Item -ItemType Directory -Path $receiptRoot -Force
 $receipt=Join-Path $receiptRoot ($result.request_id+'.json')
 $result.receipt_path=$receipt
 $json=$result|ConvertTo-Json -Depth 12
 [IO.File]::WriteAllText($receipt,$json,[Text.UTF8Encoding]::new($false))
 Write-Output $json
 $approvedScript=$null;$installerBytes=$null
}
if($result.status -eq 'HOLD'){exit 2}

param([switch]$Inspect)
# 관리자 로컬 전용 등록기. 비밀번호/토큰/원본 게임 파일을 저장·복사하지 않습니다.
$ErrorActionPreference='Stop'
$toolRoot=$PSScriptRoot
$deployment='C:\ProgramData\LimitlessRelay'
$accountName='LIMITLESSRelaySvc'
$taskNames=@('LIMITLESS-Bot-OnBoot','LIMITLESS-Art-OnBoot')
$identity=[Security.Principal.WindowsIdentity]::GetCurrent()
$isAdmin=([Security.Principal.WindowsPrincipal]::new($identity)).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
if(-not $isAdmin) {Write-Output 'ADMIN_POWERSHELL_REQUIRED: PowerShell을 관리자 권한으로 열어 실행하세요.';exit 2}
Set-Location -LiteralPath $toolRoot
$resultFile=Join-Path $toolRoot 'data/boot/registration-result.json'
function Inspect-Tasks {
 $items=@()
 foreach($name in $taskNames) {
  $task=Get-ScheduledTask -TaskName $name -ErrorAction SilentlyContinue
  if($task) {
   [xml]$xml=Export-ScheduledTask -TaskName $name
   $ns=[xml.xmlnamespacemanager]::new($xml.NameTable);$ns.AddNamespace('t','http://schemas.microsoft.com/windows/2004/02/mit/task')
   $items+=@{name=$name;state=$task.State.ToString();user=$task.Principal.UserId;logon=$task.Principal.LogonType.ToString();run_level=$task.Principal.RunLevel.ToString();boot_trigger=($null -ne $xml.SelectSingleNode('//t:BootTrigger',$ns));boot_delay=$xml.SelectSingleNode('//t:BootTrigger/t:Delay',$ns).InnerText;execute=$task.Actions[0].Execute;arguments=$task.Actions[0].Arguments;enabled=$task.Settings.Enabled;force_terminate=$task.Settings.AllowHardTerminate;instances=$task.Settings.MultipleInstances.ToString()}
  }
 }
 return $items
}
if($Inspect) {Inspect-Tasks | ConvertTo-Json -Depth 5;return}
$stage='PREFLIGHT';$createdTasks=[Collections.Generic.List[string]]::new();$password=$null;$confirm=$null;$credential=$null;$plainPassword=$null;$pointer=[IntPtr]::Zero
function Grant-Access([string]$Path,[string]$Sid,[string]$Rights,[bool]$Inherit=$true) {
 $acl=Get-Acl -LiteralPath $Path
 $inheritance=if($Inherit -and (Get-Item -LiteralPath $Path).PSIsContainer){[Security.AccessControl.InheritanceFlags]'ContainerInherit,ObjectInherit'}else{[Security.AccessControl.InheritanceFlags]::None}
 $rule=[Security.AccessControl.FileSystemAccessRule]::new([Security.Principal.SecurityIdentifier]::new($Sid),[Security.AccessControl.FileSystemRights]$Rights,$inheritance,[Security.AccessControl.PropagationFlags]::None,[Security.AccessControl.AccessControlType]::Allow)
 $acl.SetAccessRule($rule);Set-Acl -LiteralPath $Path -AclObject $acl
}
try {
 if(@(Inspect-Tasks).Count -ne 0) {throw 'EXISTING_TASKS_REQUIRE_INSPECTION_NO_OVERWRITE'}
 $originalSecret=Join-Path $toolRoot 'data/private/discord'
 if((Test-Path $originalSecret) -and -not (Get-Acl $originalSecret).AreAccessRulesProtected) {throw 'EXISTING_SECRET_ACL_REVIEW_REQUIRED'}
 # 부팅 프로세스는 현재 승인된 MANUAL만 사용합니다. 모드를 변경하지 않습니다.
 & (Join-Path $toolRoot '.venv/Scripts/python.exe') -c "import sqlite3; c=sqlite3.connect('file:data/phase2/relay.sqlite?mode=ro',uri=True); assert c.execute('SELECT mode FROM operation_mode').fetchone()[0]=='MANUAL'"
 if($LASTEXITCODE -ne 0) {throw 'MANUAL_OR_RUNTIME_REQUIRED'}
 $stage='PUBLIC_IDS_AND_ACCOUNT'
 $owners=Read-Host '바울 대표 Discord 사용자 숫자 ID (1명)'
 $guild=Read-Host 'Discord 서버 숫자 ID'
 $channels=Read-Host '기존 봇 허용 채널 숫자 ID (여럿은 쉼표)'
 if($owners -notmatch '^\d+$' -or $guild -notmatch '^\d+$' -or $channels -notmatch '^\d+(,\d+)*$') {throw 'NUMERIC_IDS_REQUIRED'}
 $existing=Get-LocalUser -Name $accountName -ErrorAction SilentlyContinue
 $password=Read-Host 'LIMITLESSRelaySvc 서비스 계정 암호 (Windows PIN 아님, 숨김 입력)' -AsSecureString
 if($password.Length -eq 0) {throw 'SERVICE_PASSWORD_REQUIRED'}
 if(-not $existing) {
  $confirm=Read-Host '서비스 계정 암호 확인 (숨김 입력)' -AsSecureString
  $first=[IntPtr]::Zero;$second=[IntPtr]::Zero
  try {$first=[Runtime.InteropServices.Marshal]::SecureStringToBSTR($password);$second=[Runtime.InteropServices.Marshal]::SecureStringToBSTR($confirm);if([Runtime.InteropServices.Marshal]::PtrToStringBSTR($first) -cne [Runtime.InteropServices.Marshal]::PtrToStringBSTR($second)) {throw 'PASSWORD_CONFIRMATION_MISMATCH'}} finally {if($first -ne [IntPtr]::Zero){[Runtime.InteropServices.Marshal]::ZeroFreeBSTR($first)};if($second -ne [IntPtr]::Zero){[Runtime.InteropServices.Marshal]::ZeroFreeBSTR($second)}}
  $existing=New-LocalUser -Name $accountName -Password $password -Description 'LIMITLESS boot relay only; MANUAL; no AI worker'
  $users=Get-LocalGroup -SID 'S-1-5-32-545';Add-LocalGroupMember -Group $users -Member $existing
 }
 $sid=$existing.SID.Value
 $admins=Get-LocalGroup -SID 'S-1-5-32-544'
 if(@(Get-LocalGroupMember -Group $admins | Where-Object {$_.SID.Value -eq $sid}).Count) {throw 'SERVICE_ACCOUNT_MUST_NOT_BE_ADMIN'}
 if(-not $existing.Enabled) {throw 'SERVICE_ACCOUNT_DISABLED_REVIEW_REQUIRED'}
 $stage='BATCH_LOGON_RIGHT'
 if(-not ('LimitlessBatchRight' -as [type])) {
 Add-Type -TypeDefinition @"
using System;
using System.Runtime.InteropServices;
public static class LimitlessBatchRight {
 [StructLayout(LayoutKind.Sequential)] struct OA {public uint Length;public IntPtr Root;public IntPtr Name;public uint Attributes;public IntPtr Descriptor;public IntPtr Quality;}
 [StructLayout(LayoutKind.Sequential)] struct US {public ushort Length;public ushort MaximumLength;public IntPtr Buffer;}
 [DllImport("advapi32.dll")] static extern uint LsaOpenPolicy(IntPtr system,ref OA attributes,uint access,out IntPtr policy);
 [DllImport("advapi32.dll")] static extern uint LsaAddAccountRights(IntPtr policy,byte[] sid,US[] rights,uint count);
 [DllImport("advapi32.dll")] static extern uint LsaEnumerateAccountRights(IntPtr policy,byte[] sid,out IntPtr rights,out uint count);
 [DllImport("advapi32.dll")] static extern uint LsaFreeMemory(IntPtr buffer);
 [DllImport("advapi32.dll")] static extern uint LsaClose(IntPtr policy);
 public static bool Ensure(byte[] sid) {
  IntPtr policy=IntPtr.Zero,buffer=IntPtr.Zero,text=IntPtr.Zero;
  try {
   OA attributes=new OA();attributes.Length=(uint)Marshal.SizeOf(typeof(OA));
   if(LsaOpenPolicy(IntPtr.Zero,ref attributes,0x810,out policy)!=0) throw new Exception("BATCH_POLICY_OPEN_FAILED");
   string name="SeBatchLogonRight";text=Marshal.StringToHGlobalUni(name);
   US right=new US();right.Length=(ushort)(name.Length*2);right.MaximumLength=(ushort)(right.Length+2);right.Buffer=text;
   if(LsaAddAccountRights(policy,sid,new US[]{right},1)!=0) throw new Exception("BATCH_RIGHT_GRANT_FAILED");
   uint count;if(LsaEnumerateAccountRights(policy,sid,out buffer,out count)!=0) return false;
   for(int i=0;i<count;i++){US item=(US)Marshal.PtrToStructure(IntPtr.Add(buffer,i*Marshal.SizeOf(typeof(US))),typeof(US));if(Marshal.PtrToStringUni(item.Buffer,item.Length/2)==name)return true;}
   return false;
  } finally {if(buffer!=IntPtr.Zero)LsaFreeMemory(buffer);if(text!=IntPtr.Zero)Marshal.FreeHGlobal(text);if(policy!=IntPtr.Zero)LsaClose(policy);}
 }
}
"@
 }
 $sidBytes=New-Object byte[] $existing.SID.BinaryLength;$existing.SID.GetBinaryForm($sidBytes,0)
 if(-not [LimitlessBatchRight]::Ensure($sidBytes)) {throw 'BATCH_LOGON_RIGHT_NOT_VERIFIED'}
 $stage='NARROW_PATH_ACLS'
 # 기존 비밀 폴더는 별도 보호 ACL을 유지합니다. 부모에는 새 폴더 생성/통과만 허용합니다.
 $privateRoot=Join-Path $toolRoot 'data/private';$privateAcl=Get-Acl $privateRoot;$privateAcl.SetAccessRuleProtection($true,$true);Set-Acl $privateRoot $privateAcl
 Grant-Access $privateRoot $sid 'ReadAndExecute,CreateDirectories' $false
 Grant-Access $toolRoot $sid 'ReadAndExecute'
 foreach($sub in @('data/phase2','data/phase3','data/boot')) {Grant-Access (Join-Path $toolRoot $sub) $sid 'Modify'}
 $config=Get-Content 'data/phase3/config.json' -Raw | ConvertFrom-Json
 Grant-Access $config.inbox $sid 'Modify'
 @{owners=$owners;guild=$guild;channels=$channels} | ConvertTo-Json | Set-Content data/boot/setup-public.json -Encoding utf8
 Remove-Item -LiteralPath data/boot/service-setup-result.json -ErrorAction SilentlyContinue
 $credential=[Management.Automation.PSCredential]::new($env:COMPUTERNAME+'\'+$accountName,$password)
 $stage='SERVICE_ACCOUNT_SECURE_INPUT'
 Write-Host '별도 창에서 서비스 계정 Bot Token을 숨김 입력하세요. 기존 봇은 계속 실행됩니다.'
 $setupArgs='-NoProfile -File "'+(Join-Path $toolRoot 'boot-secret-setup.ps1')+'" -ToolRoot "'+$toolRoot+'"'
 # 대표가 요청한 로컬 보안 입력용 창만 표시합니다. 암호는 Credential 객체로 전달합니다.
 $setup=Start-Process -FilePath (Get-Command pwsh).Source -ArgumentList $setupArgs -Credential $credential -LoadUserProfile -UseNewEnvironment -WorkingDirectory $toolRoot -WindowStyle Normal -PassThru -Wait
 if($setup.ExitCode -ne 0 -or -not (Test-Path data/boot/service-setup-result.json)) {throw 'SERVICE_SETUP_NOT_COMPLETE'}
 $setupResult=Get-Content data/boot/service-setup-result.json -Raw | ConvertFrom-Json
 if($setupResult.status -ne 'PASS') {throw 'SERVICE_SETUP_HOLD'}
 $stage='BOOTSTRAP_DEPLOYMENT'
 # 승인 파일과 bootstrap은 서비스 계정이 수정할 수 없는 관리자 소유 ACL로 배포합니다.
 New-Item -ItemType Directory -Force $deployment | Out-Null
 $deployAcl=[Security.AccessControl.DirectorySecurity]::new();$deployAcl.SetAccessRuleProtection($true,$false);$deployAcl.SetOwner($identity.User)
 foreach($entry in @(@($identity.User.Value,'FullControl'),@('S-1-5-18','FullControl'),@('S-1-5-32-544','FullControl'),@($sid,'ReadAndExecute'))) {$deployAcl.AddAccessRule([Security.AccessControl.FileSystemAccessRule]::new([Security.Principal.SecurityIdentifier]::new($entry[0]),[Security.AccessControl.FileSystemRights]$entry[1],[Security.AccessControl.InheritanceFlags]'ContainerInherit,ObjectInherit',[Security.AccessControl.PropagationFlags]::None,[Security.AccessControl.AccessControlType]::Allow))}
 Set-Acl -LiteralPath $deployment -AclObject $deployAcl
 Copy-Item -LiteralPath (Join-Path $toolRoot 'boot-service.ps1') -Destination (Join-Path $deployment 'boot-service.ps1')
 $runtime=@{}
 Get-ChildItem collab -Filter '*.py' -File | ForEach-Object {$runtime['collab/'+$_.Name]=(Get-FileHash $_.FullName -Algorithm SHA256).Hash.ToLower()}
 $runtime['DiscordSecretStore.psm1']=(Get-FileHash DiscordSecretStore.psm1 -Algorithm SHA256).Hash.ToLower()
 $approval=@{kind='EXPLICIT_BOOT_RELAY_OWNER_APPROVAL';service_sid=$sid;tool_root=$toolRoot;bootstrap_sha256=(Get-FileHash (Join-Path $deployment 'boot-service.ps1') -Algorithm SHA256).Hash.ToLower();wrapper_sha256=(Get-FileHash register-discord.ps1 -Algorithm SHA256).Hash.ToLower();supervisor_sha256=(Get-FileHash collab/boot_relay.py -Algorithm SHA256).Hash.ToLower();runtime_sha256=$runtime}
 $approvalFile=Join-Path $deployment 'boot-approval.json';$approval | ConvertTo-Json -Depth 5 | Set-Content $approvalFile -Encoding utf8
 $approvalAcl=Get-Acl $approvalFile;$approvalAcl.SetAccessRuleProtection($true,$true);$approvalAcl.SetOwner($identity.User);Set-Acl $approvalFile $approvalAcl
 $stage='REGISTER_TWO_TASKS'
 # 새 자격 증명으로 OS에 등록하되, 현재 실행 중 서비스는 시작/종료하지 않습니다.
 $pointer=[Runtime.InteropServices.Marshal]::SecureStringToBSTR($password)
 $plainPassword=[Runtime.InteropServices.Marshal]::PtrToStringBSTR($pointer)
 foreach($role in @('Bot','Art')) {
  & (Join-Path $toolRoot 'boot-service.ps1') -Action Preview -Service $role | Out-Null
  [xml]$xml=Get-Content ('data/boot/LIMITLESS-'+$role+'-OnBoot.xml') -Raw
  $ns=[xml.xmlnamespacemanager]::new($xml.NameTable);$ns.AddNamespace('t','http://schemas.microsoft.com/windows/2004/02/mit/task')
  $xml.SelectSingleNode('//t:Settings/t:Enabled',$ns).InnerText='true'
  $name='LIMITLESS-'+$role+'-OnBoot'
  Register-ScheduledTask -TaskName $name -Xml $xml.OuterXml -User ($env:COMPUTERNAME+'\'+$accountName) -Password $plainPassword | Out-Null
  $createdTasks.Add($name)
 }
 $stage='VERIFY_REGISTERED_TASKS'
 $tasks=@(Inspect-Tasks)
 if($tasks.Count -ne 2 -or @($tasks | Where-Object {-not $_.boot_trigger -or -not $_.enabled -or $_.logon -ne 'Password' -or $_.run_level -ne 'Limited' -or $_.force_terminate}).Count) {throw 'REGISTERED_TASK_SETTINGS_MISMATCH'}
 @{status='REGISTERED';tasks=$tasks;service_account_is_admin=$false;batch_logon_right_verified=$true;mode='MANUAL';ai='HOLD';current_services_restarted=$false;actual_prelogin_boot='NOT_VERIFIED';reboot_or_shutdown=$false;at=[datetime]::UtcNow.ToString('o')} | ConvertTo-Json -Depth 6 | Set-Content $resultFile -Encoding utf8
 Write-Host '등록 완료: 2/2. 다음 부팅용 작업만 등록했고 현재 봇은 재시작하지 않았습니다. 실제 PIN 전 부팅 동작은 NOT_VERIFIED입니다.'
} catch {
 foreach($name in $createdTasks) {Disable-ScheduledTask -TaskName $name -ErrorAction SilentlyContinue | Out-Null}
 @{status='HOLD';created_tasks=@($createdTasks);reason=$stage;raw_secret_logged=$false;at=[datetime]::UtcNow.ToString('o')} | ConvertTo-Json -Depth 4 | Set-Content $resultFile -Encoding utf8
 Write-Host '등록 보류: 이번에 생성된 작업은 비활성화했습니다. 비밀정보를 제외한 상태 파일을 Codex가 확인해야 합니다.'
 exit 2
} finally {
 if($pointer -ne [IntPtr]::Zero) {[Runtime.InteropServices.Marshal]::ZeroFreeBSTR($pointer)}
 $plainPassword=$null;$credential=$null
 if($null -ne $password) {$password.Dispose()}
 if($null -ne $confirm) {$confirm.Dispose()}
}

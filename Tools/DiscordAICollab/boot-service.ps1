param(
 [ValidateSet('Preview','Inspect','StartIfApproved')][string]$Action='Inspect',
 [ValidateSet('Bot','Art')][string]$Service='Bot',
 [string]$ToolRoot=$PSScriptRoot
)
# 등록·계정 생성·암호 입력·Windows 자동 로그인·종료 기능은 없습니다.
$ErrorActionPreference='Stop'
$deploymentRoot='C:\ProgramData\LimitlessRelay'
$approvalPath=Join-Path $deploymentRoot 'boot-approval.json'
if($Action -eq 'Preview') {
 $output=Join-Path $PSScriptRoot 'data/boot'
 New-Item -ItemType Directory -Force $output | Out-Null
 $pwsh=(Get-Command pwsh).Source
 $account=$env:COMPUTERNAME+'\LIMITLESSRelaySvc'
 $escape={param($value) [Security.SecurityElement]::Escape($value)}
 $exe=& $escape $pwsh
 $arguments=& $escape ('-NoProfile -WindowStyle Hidden -File "'+$deploymentRoot+'\boot-service.ps1" -Action StartIfApproved -Service '+$Service+' -ToolRoot "'+$ToolRoot+'"')
 $principal=& $escape $account
 $xml=@"
<?xml version="1.0" encoding="UTF-16"?>
<Task version="1.4" xmlns="http://schemas.microsoft.com/windows/2004/02/mit/task">
 <RegistrationInfo><Description>LIMITLESS boot relay only; MANUAL; AI HOLD; registration requires separate approval.</Description></RegistrationInfo>
 <Triggers><BootTrigger><Enabled>true</Enabled><Delay>PT30S</Delay></BootTrigger></Triggers>
 <Principals><Principal id="Relay"><UserId>$principal</UserId><LogonType>Password</LogonType><RunLevel>LeastPrivilege</RunLevel></Principal></Principals>
 <Settings><MultipleInstancesPolicy>IgnoreNew</MultipleInstancesPolicy><DisallowStartIfOnBatteries>false</DisallowStartIfOnBatteries><StopIfGoingOnBatteries>false</StopIfGoingOnBatteries><AllowHardTerminate>false</AllowHardTerminate><StartWhenAvailable>true</StartWhenAvailable><RunOnlyIfNetworkAvailable>true</RunOnlyIfNetworkAvailable><Enabled>false</Enabled><ExecutionTimeLimit>PT0S</ExecutionTimeLimit><RestartOnFailure><Interval>PT1M</Interval><Count>3</Count></RestartOnFailure></Settings>
 <Actions Context="Relay"><Exec><Command>$exe</Command><Arguments>$arguments</Arguments><WorkingDirectory>$deploymentRoot</WorkingDirectory></Exec></Actions>
</Task>
"@
 $xml | Set-Content -LiteralPath (Join-Path $output ('LIMITLESS-'+$Service+'-OnBoot.xml')) -Encoding Unicode
 @{service=$Service;trigger='BOOT+30s';logon='Password';run_level='LeastPrivilege';registered=$false;enabled=$false;account_created=$false;ai='HOLD';mode='MANUAL';bootstrap=$deploymentRoot;storage_dependency=$ToolRoot;service_account_setup='REQUIRED';dpapi_profile='NEW_ACCOUNT_OWNED_BOOT_STORE';existing_user_credentials='NOT_USED'} | ConvertTo-Json | Set-Content (Join-Path $output ($Service+'-boot-plan.json')) -Encoding utf8
 Write-Output 'BOOT XML PREPARED / NOT REGISTERED / NEW ACCOUNT SETUP REQUIRED'
 return
}
if($Action -eq 'Inspect') {
 @{approval_present=(Test-Path $approvalPath);staged_only=$true;registered_by_this_script=$false;ai='HOLD';mode='MANUAL_REQUIRED'} | ConvertTo-Json
 return
}
if(-not (Test-Path -LiteralPath $approvalPath)) {Write-Output 'HOLD_BOOT_REGISTRATION_AND_ACCOUNT_SETUP_REQUIRED';return}
try {
 $approval=Get-Content -LiteralPath $approvalPath -Raw | ConvertFrom-Json
 $sid=[Security.Principal.WindowsIdentity]::GetCurrent().User.Value
 # 서비스 계정이 승인 파일을 스스로 쓰거나 소유해서 승인할 수 없게 합니다.
 $approvalAcl=Get-Acl -LiteralPath $approvalPath
 $approvalOwner=$approvalAcl.GetOwner([Security.Principal.SecurityIdentifier]).Value
 if(-not $approvalAcl.AreAccessRulesProtected -or $approvalOwner -eq $sid) {throw 'READ_ONLY_ADMIN_OWNED_APPROVAL_REQUIRED'}
 $writeMask=[Security.AccessControl.FileSystemRights]::Write -bor [Security.AccessControl.FileSystemRights]::Delete -bor [Security.AccessControl.FileSystemRights]::ChangePermissions -bor [Security.AccessControl.FileSystemRights]::TakeOwnership
 foreach($rule in $approvalAcl.GetAccessRules($true,$true,[Security.Principal.SecurityIdentifier])) {
   if($rule.AccessControlType -eq 'Allow' -and ($rule.FileSystemRights -band $writeMask) -ne 0 -and $rule.IdentityReference.Value -notin @($approvalOwner,'S-1-5-18','S-1-5-32-544')) {throw 'APPROVAL_WRITABLE_BY_UNTRUSTED_IDENTITY'}
 }
 if($approval.kind -ne 'EXPLICIT_BOOT_RELAY_OWNER_APPROVAL' -or $approval.service_sid -ne $sid -or $approval.tool_root -ne $ToolRoot -or $approval.bootstrap_sha256 -ne (Get-FileHash $PSCommandPath -Algorithm SHA256).Hash.ToLower()) {throw 'APPROVAL_MISMATCH'}
 $account=[Security.Principal.WindowsIdentity]::GetCurrent().Name
 if($account.Split('\')[-1] -ne 'LIMITLESSRelaySvc') {throw 'DEDICATED_STANDARD_ACCOUNT_REQUIRED'}
 if(([Security.Principal.WindowsPrincipal]::new([Security.Principal.WindowsIdentity]::GetCurrent())).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)) {throw 'ADMIN_SERVICE_REJECTED'}
 # F: 늦은 인식은 C: bootstrap에서 먼저 기다립니다. 없으면 사용자/원본에 쓰지 않고 HOLD합니다.
 for($attempt=0;$attempt -lt 12 -and -not (Test-Path -LiteralPath $ToolRoot);$attempt++) {Start-Sleep -Seconds 30}
 if(-not (Test-Path -LiteralPath $ToolRoot)) {throw 'STORAGE_UNAVAILABLE'}
 if($approval.wrapper_sha256 -ne (Get-FileHash (Join-Path $ToolRoot 'register-discord.ps1') -Algorithm SHA256).Hash.ToLower() -or $approval.supervisor_sha256 -ne (Get-FileHash (Join-Path $ToolRoot 'collab/boot_relay.py') -Algorithm SHA256).Hash.ToLower()) {throw 'DEPLOYMENT_SHA_MISMATCH'}
 foreach($sourceFile in Get-ChildItem -LiteralPath (Join-Path $ToolRoot 'collab') -Filter '*.py' -File) {
   $relativePath='collab/'+$sourceFile.Name
   $expected=$approval.runtime_sha256.PSObject.Properties[$relativePath]
   if($null -eq $expected -or $expected.Value -ne (Get-FileHash -LiteralPath $sourceFile.FullName -Algorithm SHA256).Hash.ToLower()) {throw 'RUNTIME_SOURCE_SHA_MISMATCH'}
 }
 $secretModule=$approval.runtime_sha256.PSObject.Properties['DiscordSecretStore.psm1']
 if($null -eq $secretModule -or $secretModule.Value -ne (Get-FileHash (Join-Path $ToolRoot 'DiscordSecretStore.psm1') -Algorithm SHA256).Hash.ToLower()) {throw 'SECRET_MODULE_SHA_MISMATCH'}
 Set-Location -LiteralPath $ToolRoot
 & (Join-Path $ToolRoot '.venv/Scripts/python.exe') -m collab.boot_relay $Service
 exit $LASTEXITCODE
} catch {Write-Output 'HOLD_BOOT_SCOPE_IDENTITY_STORAGE_OR_SETUP / NO SECRET DETAILS';exit 0}

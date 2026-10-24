param([switch]$Child,[ValidatePattern('^[a-f0-9]{32}$')][string]$RequestId,[ValidatePattern('^[a-f0-9]{64}$')][string]$ScriptSha)
# 격리 공식 CLI의 version/help 전용입니다. 모델·로그인·기존 작업·운영 참조를 실행/변경하지 않습니다.
$ErrorActionPreference='Stop'
$repo=Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
$tool=Join-Path $repo 'Tools/DiscordAICollab'
$package=Join-Path $tool 'data/runtime/codex/official-0.164.0-alpha.11.1-x64'
$evidence=Join-Path $repo '문서/00_프로젝트/Evidence/ServiceCLI_OfficialPackage_20261024.json'
$metadata=Join-Path $tool 'BatchReadOnlyMetadata.psm1'
$approvalPath='C:/ProgramData/LimitlessRelay/boot-approval.json'
$out=Join-Path $tool 'data/service-isolated-batch-check'
$serviceSid='S-1-5-21-2858504392-1958168942-1662906075-1007'
function Sha([string]$p){(Get-FileHash -LiteralPath $p -Algorithm SHA256).Hash.ToLower()}
function Verify-Package {
 if((Sha $evidence) -ne '04cecc241628c7a17a46ef9fc3befbe32e287792f938ecc4f854f20a40eb9833'){throw 'EVIDENCE_SHA_HOLD'}
 if((Sha $metadata) -ne '09458bfc9a2b5516784efa7e9bda7013506dc0c21673b8aad7aec84240b60faf'){throw 'METADATA_SHA_HOLD'}
 $manifest=Get-Content -LiteralPath $evidence -Raw|ConvertFrom-Json
 if($manifest.files.Count -ne 45 -or @(Get-ChildItem -LiteralPath $package -File -Recurse).Count -ne 45){throw 'FILE_COUNT_HOLD'}
 foreach($f in $manifest.files){if((Sha (Join-Path $package $f.path)) -ne $f.sha256){throw 'PACKAGE_SHA_HOLD'}}
 foreach($name in @('bin/codex.exe','bin/codex-code-mode-host.exe','codex-resources/codex-command-runner.exe','codex-resources/codex-windows-sandbox-setup.exe')){
  $sig=Get-AuthenticodeSignature -LiteralPath (Join-Path $package $name)
  if($sig.Status -ne 'Valid' -or $sig.SignerCertificate.Subject -notmatch 'OpenAI OpCo'){throw 'SIGNATURE_HOLD'}
 }
}
function Snapshot-Boot {
 $r=@{}
 foreach($n in @('LIMITLESS-Bot-OnBoot','LIMITLESS-Art-OnBoot')){
  $xml=Export-ScheduledTask -TaskName $n -TaskPath '\'
  $r[$n]=[Convert]::ToHexString([Security.Cryptography.SHA256]::HashData([Text.Encoding]::Unicode.GetBytes($xml))).ToLower()
 }
 return $r
}
if($Child){
 $r=@{status='HOLD';request_id=$RequestId;actual_ai_calls=0;commands=@('--version','--help')}
 try {
  if(-not $RequestId -or (Sha $PSCommandPath) -ne $ScriptSha){throw 'SCRIPT_PIN_HOLD'}
  $id=[Security.Principal.WindowsIdentity]::GetCurrent()
  if($id.User.Value -ne $serviceSid){throw 'SERVICE_IDENTITY_HOLD'}
  Import-Module $metadata -Force
  $logon=[LimitlessBatchMetadataV1]::CurrentLogon()
  if($logon[0] -ne 4 -or $logon[1] -ne 0 -or [Environment]::UserInteractive){throw 'SESSION_ZERO_BATCH_HOLD'}
  $r.account=$id.Name;$r.session_id=$logon[1];$r.logon_type=$logon[0]
  $homeBefore=$env:CODEX_HOME
  if($homeBefore -and $homeBefore -ine 'C:\Users\LIMITLESSRelaySvc\.codex'){throw 'EXISTING_CODEX_HOME_HOLD'}
  if($env:USERPROFILE -ine 'C:\Users\LIMITLESSRelaySvc'){throw 'SERVICE_PROFILE_HOLD'}
  Verify-Package
  $exe=Join-Path $package 'bin/codex.exe'
  $r.cli_path=$exe;$r.cli_sha256=Sha $exe;$r.files_verified=45
  # 고정된 두 인수만 실행하고 출력에서는 고정된 검증값만 채택합니다.
  $v=& $exe --version 2>$null;$r.version_exit=$LASTEXITCODE
  $h=& $exe --help 2>$null;$r.help_exit=$LASTEXITCODE
  $r.version_matches=(($v -join '') -eq 'codex-cli 0.164.0-alpha.11.1')
  $r.help_usage_present=(($h -join "`n") -match 'Usage:')
  $r.codex_home_unchanged=($homeBefore -ceq $env:CODEX_HOME)
  if($r.version_exit -eq 0 -and $r.help_exit -eq 0 -and $r.version_matches -and $r.help_usage_present -and $r.codex_home_unchanged){$r.status='PASS'}
 } catch {$r.error='SERVICE_BOOTSTRAP_HOLD_NO_RAW_ERROR_LOGGED'}
 $r|ConvertTo-Json -Depth 5|Set-Content -LiteralPath (Join-Path $out ('child-'+$RequestId+'.json')) -Encoding utf8
 if($r.status -eq 'PASS'){exit 0}else{exit 2}
}
$id=[Security.Principal.WindowsIdentity]::GetCurrent()
if(-not ([Security.Principal.WindowsPrincipal]::new($id)).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)){
 Write-Output 'HOLD_ADMIN_POWERSHELL_REQUIRED_NO_CHANGES';exit 2
}
$nonce=[guid]::NewGuid().ToString('N');$name='LIMITLESS-IsolatedCLI-'+$nonce;$marker='ISOLATED_VERSION_HELP_ONLY_'+$nonce
$r=@{status='HOLD';request_id=$nonce;task_name=$name;actual_ai_calls=0;automatic_retries=0}
$credential=$null;$plain=$null;$registered=$false
try {
 Verify-Package
 $approval=Get-Content -LiteralPath $approvalPath -Raw|ConvertFrom-Json
 if($approval.service_sid -ne $serviceSid -or $approval.tool_root -ne $tool){throw 'DEPLOYMENT_IDENTITY_HOLD'}
 $before=Snapshot-Boot;$approvalBefore=Sha $approvalPath
 New-Item -ItemType Directory -Path $out -Force|Out-Null
 # 새 진단 출력 폴더에만 서비스 Modify를 부여하고 비밀/인증은 저장하지 않습니다.
 $acl=[Security.AccessControl.DirectorySecurity]::new();$acl.SetAccessRuleProtection($true,$false);$acl.SetOwner($id.User)
 foreach($s in @($id.User.Value,'S-1-5-18','S-1-5-32-544')){$acl.AddAccessRule([Security.AccessControl.FileSystemAccessRule]::new([Security.Principal.SecurityIdentifier]::new($s),'FullControl','ContainerInherit,ObjectInherit','None','Allow'))}
 $acl.AddAccessRule([Security.AccessControl.FileSystemAccessRule]::new([Security.Principal.SecurityIdentifier]::new($serviceSid),'Modify','ContainerInherit,ObjectInherit','None','Allow'));Set-Acl -LiteralPath $out -AclObject $acl
 $account=$env:COMPUTERNAME+'\LIMITLESSRelaySvc'
 $credential=Get-Credential -UserName $account -Message '격리 CLI --version/--help만 검증합니다. 서비스 Windows 암호를 이 로컬 보안 창에 입력하세요. 채팅에 보내지 마세요.'
 if(-not $credential -or $credential.UserName -ine $account){throw 'LOCAL_CREDENTIAL_HOLD'}
 $arguments='-NoProfile -NonInteractive -WindowStyle Hidden -File "'+$PSCommandPath+'" -Child -RequestId '+$nonce+' -ScriptSha '+(Sha $PSCommandPath)
 $a=New-ScheduledTaskAction -Execute (Get-Command pwsh).Source -Argument $arguments -WorkingDirectory $tool
 $settings=New-ScheduledTaskSettingsSet -ExecutionTimeLimit (New-TimeSpan -Minutes 2) -MultipleInstances IgnoreNew
 $plain=$credential.GetNetworkCredential().Password
 Register-ScheduledTask -TaskName $name -TaskPath '\' -Description $marker -Action $a -Settings $settings -User $account -Password $plain -RunLevel Limited|Out-Null
 $registered=$true;$plain=$null
 $t=Get-ScheduledTask -TaskName $name -TaskPath '\'
 if($t.Principal.LogonType.ToString() -ne 'Password' -or $t.Principal.RunLevel.ToString() -ne 'Limited'){throw 'TASK_POLICY_HOLD'}
 Start-ScheduledTask -TaskName $name -TaskPath '\'
 $childPath=Join-Path $out ('child-'+$nonce+'.json')
 for($n=0;$n -lt 70;$n++){Start-Sleep -Seconds 1;$t=Get-ScheduledTask -TaskName $name -TaskPath '\';if((Test-Path -LiteralPath $childPath) -and $t.State.ToString() -ne 'Running'){break}}
 $r.task_exit_code=(Get-ScheduledTaskInfo -TaskName $name -TaskPath '\').LastTaskResult
 if(Test-Path -LiteralPath $childPath){$r.child=Get-Content -LiteralPath $childPath -Raw|ConvertFrom-Json}
 if($r.child.request_id -eq $nonce -and $r.child.status -eq 'PASS' -and $r.task_exit_code -eq 0){$r.status='PASS'}
} catch {$r.error='BATCH_CHECK_HOLD_NO_RAW_ERROR_LOGGED'}
finally {
 $plain=$null;if($credential){$credential.Password.Dispose()};$credential=$null
 try {
  if($registered){$t=Get-ScheduledTask -TaskName $name -TaskPath '\';if($t.Description -ne $marker -or -not $t.Actions[0].Arguments.Contains($nonce)){throw 'CLEANUP_IDENTITY_HOLD'}
   if($t.State.ToString() -eq 'Running'){$r.cleanup='RUNNING_NOT_STOPPED';$r.status='HOLD'}else{Unregister-ScheduledTask -TaskName $name -TaskPath '\' -Confirm:$false;$r.cleanup='REMOVED'}
  }else{$r.cleanup='NOT_REGISTERED'}
  $after=Snapshot-Boot;$same=$true;foreach($n in $before.Keys){if($before[$n] -ne $after[$n]){$same=$false}}
  $r.boot_tasks_unchanged=$same;$r.approval_unchanged=((Sha $approvalPath) -eq $approvalBefore)
  if(-not $same -or -not $r.approval_unchanged){$r.status='HOLD'}
 }catch{$r.protection_or_cleanup='NOT_VERIFIED';$r.status='HOLD'}
}
if(Test-Path -LiteralPath $out){$r|ConvertTo-Json -Depth 7|Set-Content -LiteralPath (Join-Path $out ('run-'+$nonce+'.json')) -Encoding utf8}
$r|ConvertTo-Json -Depth 7
if($r.status -eq 'PASS'){exit 0}else{exit 2}

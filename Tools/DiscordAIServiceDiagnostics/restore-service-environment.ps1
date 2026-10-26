param([switch]$Child,
 [ValidatePattern('^[a-f0-9]{32}$')][string]$RequestId,
 [ValidatePattern('^[a-f0-9]{64}$')][string]$ScriptSha)
# 과거 PASS의 Password/Limited 임시 작업 방식만 재사용합니다. 이 스크립트에는 usage/AI 실행 경로가 없습니다.
$ErrorActionPreference='Stop'
$approvalPath='C:/ProgramData/LimitlessRelay/boot-approval.json'
$approvalSha='3e032ebcce3d91bd762a8dec81048d04607ddde761afd62489c0fa2b8e49140f'
$metadataSha='09458bfc9a2b5516784efa7e9bda7013506dc0c21673b8aad7aec84240b60faf'
$baseDir='C:/ProgramData/LimitlessRelay/UsageEnvironmentChecks'
function Sha([string]$p){(Get-FileHash -LiteralPath $p -Algorithm SHA256).Hash.ToLower()}
function No-Link([string]$p){if((Get-Item -LiteralPath $p -Force).Attributes -band [IO.FileAttributes]::ReparsePoint){throw 'ENV_LINK_HOLD'}}
function Read-Approval {
 if((Sha $approvalPath) -ne $approvalSha){throw 'ENV_APPROVAL_CHANGED_HOLD'}
 return Get-Content -LiteralPath $approvalPath -Raw|ConvertFrom-Json
}
function Boot-Snapshot {
 $r=@{}
 foreach($n in @('LIMITLESS-Bot-OnBoot','LIMITLESS-Art-OnBoot')){
  $xml=Export-ScheduledTask -TaskName $n -TaskPath '\'
  $r[$n]=[Convert]::ToHexString([Security.Cryptography.SHA256]::HashData([Text.Encoding]::Unicode.GetBytes($xml))).ToLower()
 }
 return $r
}
function Safe-Error([Management.Automation.ErrorRecord]$record){
 $chain=@();$e=$record.Exception
 for($depth=0;$e -and $depth -lt 5;$depth++){
  $item=@{type=$e.GetType().Name;hresult=$e.HResult}
  if($e.PSObject.Properties['NativeErrorCode']){$item.native_error_code=[string]$e.NativeErrorCode}
  if($e.PSObject.Properties['StatusCode']){$item.cim_status_code=[string]$e.StatusCode}
  $chain+=,$item;$e=$e.InnerException
 }
 return $chain
}
function Ledger-Snapshot([string]$tool){
 # 부모의 보호 확인에서만 readonly로 연결합니다. 자식 진단 작업에는 SQLite 조작이 없습니다.
 $db=Join-Path $tool 'data/phase2/relay.sqlite'
 $code='import sys,json,sqlite3,hashlib;from pathlib import Path;p=Path(sys.argv[1]);assert not Path(str(p)+"-wal").exists();c=sqlite3.connect(p.resolve().as_uri()+"?mode=ro&immutable=1",uri=True);m=c.execute("SELECT mode FROM operation_mode").fetchone()[0];v={n:hashlib.sha256(json.dumps(c.execute("SELECT * FROM "+n+" ORDER BY rowid").fetchall(),default=str).encode()).hexdigest() for n in ["tasks","approvals","operation_mode"]};c.close();print(json.dumps({"mode":m,"tables":v},sort_keys=True))'
 $si=[Diagnostics.ProcessStartInfo]::new();$si.FileName=Join-Path $tool '.venv/Scripts/python.exe'
 $si.UseShellExecute=$false;$si.CreateNoWindow=$true;$si.RedirectStandardOutput=$true;$si.RedirectStandardError=$true
 foreach($arg in @('-X','utf8','-c',$code,$db)){$si.ArgumentList.Add($arg)}
 $proc=[Diagnostics.Process]::new();$proc.StartInfo=$si
 try{
  $null=$proc.Start();$stdout=$proc.StandardOutput.ReadToEndAsync();$stderr=$proc.StandardError.ReadToEndAsync()
  if(-not $proc.WaitForExit(5000)){$proc.Kill($true);throw 'ENV_LEDGER_TIMEOUT_HOLD'}
  if($proc.ExitCode -ne 0){throw 'ENV_LEDGER_READ_HOLD'}
  $text=$stdout.GetAwaiter().GetResult();$parsed=$text|ConvertFrom-Json
  if($parsed.mode -ne 'MANUAL'){throw 'ENV_MANUAL_REQUIRED_HOLD'}
  return $text.Trim()
 }finally{$proc.Dispose()}
}
if($Child){
 $out=Join-Path (Join-Path $baseDir $RequestId) 'Output'
 $r=@{status='HOLD';stage='IDENTITY';request_id=$RequestId;usage_calls=0;model_calls=0;discord_posts=0;sqlite_operations=0;automatic_retries=0}
 try{
  if(-not $RequestId -or (Sha $PSCommandPath) -ne $ScriptSha){throw 'ENV_CHILD_SCRIPT_SHA_HOLD'}
  No-Link $PSScriptRoot;No-Link $out
  $a=Read-Approval;$id=[Security.Principal.WindowsIdentity]::GetCurrent()
  if($id.User.Value -ne $a.service_sid -or $id.Name -ne 'PAUL-PC\LIMITLESSRelaySvc' -or ([Security.Principal.WindowsPrincipal]::new($id)).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)){throw 'ENV_SERVICE_IDENTITY_HOLD'}
  $module=Join-Path $PSScriptRoot 'BatchReadOnlyMetadata.psm1'
  if((Sha $module) -ne $metadataSha){throw 'ENV_METADATA_SHA_HOLD'}
  Import-Module $module -Force
  $logon=[LimitlessBatchMetadataV1]::CurrentLogon()
  if($logon[0] -ne 4 -or $logon[1] -ne 0 -or [Environment]::UserInteractive){throw 'ENV_SESSION0_LOGON4_HOLD'}
  # SID 원문은 이 보호된 Output에만 남깁니다. 부모의 공용 결과에는 일치 여부만 남깁니다.
  $r.account=$id.Name;$r.sid=$id.User.Value;$r.sid_matches=$true;$r.session_id=$logon[1];$r.logon_type=$logon[0]
  $r.stage='PROFILE_AND_CLI'
  $beforeHome=$env:CODEX_HOME;$profile=[Environment]::GetFolderPath([Environment+SpecialFolder]::UserProfile)
  if($profile -ine 'C:\Users\LIMITLESSRelaySvc' -or $env:USERPROFILE -ine $profile){throw 'ENV_PROFILE_HOLD'}
  $effectiveHome=Join-Path $profile '.codex'
  if($beforeHome -and $beforeHome -ine $effectiveHome){throw 'ENV_CODEX_HOME_HOLD'}
  No-Link $effectiveHome
  $r.codex_home_exists=Test-Path -LiteralPath $effectiveHome -PathType Container
  if(-not $r.codex_home_exists){throw 'ENV_CODEX_HOME_MISSING_HOLD'}
  if((Sha $a.service_codex.path) -ne $a.service_codex.sha256){throw 'ENV_CLI_SHA_HOLD'}
  $r.cli_sha_matches=$true;$r.cli_executed=$false
  $r.stage='PROTECTED_PATH_PROBE'
  $probe=Join-Path $out 'service-write-probe.txt'
  [IO.File]::WriteAllText($probe,'ENVIRONMENT_ONLY_NO_USAGE')
  if([IO.File]::ReadAllText($probe) -ne 'ENVIRONMENT_ONLY_NO_USAGE'){throw 'ENV_PROTECTED_IO_HOLD'}
  $r.protected_path_read_write=$true;$r.codex_home_unchanged=($beforeHome -ceq $env:CODEX_HOME)
  if(-not $r.codex_home_unchanged){throw 'ENV_HOME_CHANGED_HOLD'}
  $r.status='PASS';$r.stage='ENVIRONMENT_ONLY_COMPLETE'
 }catch{$r.reason=if($_.Exception.Message -match '^ENV_[A-Z0-9_]+$'){$_.Exception.Message}else{'ENV_CHILD_HOLD'};$r.error_chain=Safe-Error $_}
 $r|ConvertTo-Json -Depth 5|Set-Content -LiteralPath (Join-Path $out 'child-private.json') -Encoding utf8
 if($r.status -eq 'PASS'){exit 0}else{exit 2}
}
$id=[Security.Principal.WindowsIdentity]::GetCurrent()
if(-not ([Security.Principal.WindowsPrincipal]::new($id)).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)){
 Write-Output 'HOLD_ADMIN_POWERSHELL_REQUIRED_NO_CHANGES_NO_USAGE';exit 2
}
$nonce=[guid]::NewGuid().ToString('N');$name='LIMITLESS-UsageEnvironment-'+$nonce;$marker='ENVIRONMENT_ONLY_NO_USAGE_'+$nonce
$registered=$false;$credential=$null;$plain=$null;$before=$null;$ledgerBefore=$null;$out=$null
$r=@{status='HOLD';stage='ADMIN_PREFLIGHT';request_id=$nonce;usage_calls=0;model_calls=0;discord_posts=0;child_sqlite_operations=0;automatic_retries=0;parent_is_admin=$true;parent_session_id=(Get-Process -Id $PID).SessionId}
try{
 $a=Read-Approval;$before=Boot-Snapshot;$ledgerBefore=Ledger-Snapshot $a.tool_root
 if(Test-Path -LiteralPath 'C:/ProgramData/LimitlessRelay/usage-only-approval.json'){throw 'ENV_EXISTING_ACTIVE_USAGE_PERMIT_HOLD'}
 $metadataSource=Join-Path $a.tool_root 'BatchReadOnlyMetadata.psm1'
 if((Sha $metadataSource) -ne $metadataSha){throw 'ENV_METADATA_SOURCE_HOLD'}
 No-Link 'C:/ProgramData/LimitlessRelay'
 if(Test-Path -LiteralPath $baseDir){No-Link $baseDir}else{$null=New-Item -ItemType Directory -Path $baseDir}
 $folder=Join-Path $baseDir $nonce
 if(Test-Path -LiteralPath $folder){throw 'ENV_FOLDER_EXISTS_HOLD'}
 $null=New-Item -ItemType Directory -Path $folder
 $childScript=Join-Path $folder 'restore-service-environment.ps1'
 Copy-Item -LiteralPath $PSCommandPath -Destination $childScript
 Copy-Item -LiteralPath $metadataSource -Destination (Join-Path $folder 'BatchReadOnlyMetadata.psm1')
 if((Sha $childScript) -ne (Sha $PSCommandPath) -or (Sha (Join-Path $folder 'BatchReadOnlyMetadata.psm1')) -ne $metadataSha){throw 'ENV_COPY_HOLD'}
 $out=Join-Path $folder 'Output';$null=New-Item -ItemType Directory -Path $out
 # 새 진단 Output만 서비스 Modify, 관리자/소유자/SYSTEM FullControl. 기존 ACL은 손대지 않습니다.
 $acl=[Security.AccessControl.DirectorySecurity]::new();$acl.SetAccessRuleProtection($true,$false);$acl.SetOwner($id.User)
 foreach($sid in @($id.User.Value,'S-1-5-18','S-1-5-32-544')){$acl.AddAccessRule([Security.AccessControl.FileSystemAccessRule]::new([Security.Principal.SecurityIdentifier]::new($sid),'FullControl','ContainerInherit,ObjectInherit','None','Allow'))}
 $acl.AddAccessRule([Security.AccessControl.FileSystemAccessRule]::new([Security.Principal.SecurityIdentifier]::new($a.service_sid),'Modify','ContainerInherit,ObjectInherit','None','Allow'));Set-Acl -LiteralPath $out -AclObject $acl
 $r.stage='LOCAL_SECURE_INPUT'
 $credential=Get-Credential -UserName 'PAUL-PC\LIMITLESSRelaySvc' -Message '환경 확인 전용입니다. Windows 서비스 계정 암호는 이 로컬 보안 입력에만 입력하세요. 사용량/AI 호출은 없습니다.'
 if(-not $credential -or $credential.UserName -ine 'PAUL-PC\LIMITLESSRelaySvc'){throw 'ENV_LOCAL_SERVICE_CREDENTIAL_HOLD'}
 $arguments='-NoProfile -NonInteractive -WindowStyle Hidden -File "'+$childScript+'" -Child -RequestId '+$nonce+' -ScriptSha '+(Sha $childScript)
 $action=New-ScheduledTaskAction -Execute (Get-Command pwsh).Source -Argument $arguments -WorkingDirectory $folder
 $settings=New-ScheduledTaskSettingsSet -ExecutionTimeLimit (New-TimeSpan -Seconds 20) -MultipleInstances IgnoreNew
 $r.stage='REGISTER_ENVIRONMENT_TASK'
 $plain=$credential.GetNetworkCredential().Password
 Register-ScheduledTask -TaskName $name -TaskPath '\' -Description $marker -Action $action -Settings $settings -User $credential.UserName -Password $plain -RunLevel Limited|Out-Null
 $registered=$true;$plain=$null
 $task=Get-ScheduledTask -TaskName $name -TaskPath '\'
 $limit=[Xml.XmlConvert]::ToTimeSpan($task.Settings.ExecutionTimeLimit).TotalSeconds
 $r.task_policy=@{logon_type=$task.Principal.LogonType.ToString();run_level=$task.Principal.RunLevel.ToString();limit_seconds=$limit;restart_count=$task.Settings.RestartCount;allow_hard_terminate=$task.Settings.AllowHardTerminate}
 if($task.Principal.LogonType.ToString() -ne 'Password' -or $task.Principal.RunLevel.ToString() -ne 'Limited' -or $limit -ne 20 -or $task.Settings.RestartCount -ne 0 -or -not $task.Settings.AllowHardTerminate){throw 'ENV_TASK_POLICY_HOLD'}
 $r.task_limit_seconds=$limit;$r.scheduler_retry_count=$task.Settings.RestartCount
 $r.stage='WAIT_SERVICE_ENVIRONMENT'
 Start-ScheduledTask -TaskName $name -TaskPath '\'
 $private=Join-Path $out 'child-private.json'
 for($i=0;$i -lt 35;$i++){
  Start-Sleep -Seconds 1;$task=Get-ScheduledTask -TaskName $name -TaskPath '\';$info=Get-ScheduledTaskInfo -TaskName $name -TaskPath '\'
  if($task.State.ToString() -ne 'Running' -and $info.LastRunTime.Year -gt 2000){break}
 }
 $r.task_exit_code=$info.LastTaskResult
 if(Test-Path -LiteralPath $private){
  $c=Get-Content -LiteralPath $private -Raw|ConvertFrom-Json
  $r.child=@{status=$c.status;stage=$c.stage;reason=$c.reason;error_chain=$c.error_chain;account=$c.account;sid_matches=$c.sid_matches;session_id=$c.session_id;logon_type=$c.logon_type;cli_sha_matches=$c.cli_sha_matches;codex_home_unchanged=$c.codex_home_unchanged;protected_path_read_write=$c.protected_path_read_write}
  if($c.request_id -eq $nonce -and $c.status -eq 'PASS' -and $c.sid -eq $a.service_sid -and $c.session_id -eq 0 -and $c.logon_type -eq 4 -and $r.task_exit_code -eq 0){$r.status='ENVIRONMENT_PASS_USAGE_NOT_ATTEMPTED'}
 }
}catch{$r.reason=if($_.Exception.Message -match '^ENV_[A-Z0-9_]+$'){$_.Exception.Message}else{'ENVIRONMENT_PRECHECK_HOLD'};$r.error_chain=Safe-Error $_}
finally{
 $plain=$null;if($credential){$credential.Password.Dispose()};$credential=$null
 try{
  if($registered){
   $t=Get-ScheduledTask -TaskName $name -TaskPath '\'
   if($t.Description -ne $marker -or -not $t.Actions[0].Arguments.Contains($nonce)){throw 'ENV_CLEANUP_IDENTITY_HOLD'}
   if($t.State.ToString() -eq 'Running'){
    # 이번에 만든 일치하는 진단 작업만 정리합니다. 기존 OnBoot/프로세스에는 접근하지 않습니다.
    Stop-ScheduledTask -TaskName $name -TaskPath '\';Start-Sleep -Seconds 2;$t=Get-ScheduledTask -TaskName $name -TaskPath '\';$r.status='HOLD';$r.timeout_cleanup=$true
   }
   if($t.State.ToString() -eq 'Running'){throw 'ENV_TASK_STILL_RUNNING_HOLD'}
   Unregister-ScheduledTask -TaskName $name -TaskPath '\' -Confirm:$false;$r.cleanup='REMOVED'
  }else{$r.cleanup='NOT_REGISTERED'}
  if($before){$after=Boot-Snapshot;$same=$true;foreach($n in $before.Keys){if($before[$n] -ne $after[$n]){$same=$false}};$r.boot_tasks_unchanged=$same;if(-not $same){throw 'ENV_BOOT_CHANGED_HOLD'}}
  $r.approval_unchanged=((Sha $approvalPath) -eq $approvalSha)
  if(-not $r.approval_unchanged){throw 'ENV_APPROVAL_CHANGED_HOLD'}
  if($ledgerBefore){$r.ledger_unchanged=((Ledger-Snapshot $a.tool_root) -ceq $ledgerBefore);if(-not $r.ledger_unchanged){throw 'ENV_LEDGER_CHANGED_HOLD'}}
 }catch{$r.status='HOLD';$r.cleanup_or_protection='NOT_VERIFIED';$r.cleanup_error_type=$_.Exception.GetType().Name;$r.cleanup_hresult=$_.Exception.HResult}
 $r.usage_permit_created=$false;$r.usage_calls=0
 if($out){$r|ConvertTo-Json -Depth 7|Set-Content -LiteralPath (Join-Path $out 'summary.json') -Encoding utf8}
}
$r|ConvertTo-Json -Depth 7
if($r.status -eq 'ENVIRONMENT_PASS_USAGE_NOT_ATTEMPTED'){exit 0}else{exit 2}

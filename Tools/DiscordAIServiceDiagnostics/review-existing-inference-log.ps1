param([switch]$Child,[ValidatePattern('^[a-f0-9]{32}$')][string]$ReviewId,
 [ValidatePattern('^[a-f0-9]{64}$')][string]$ScriptSha,[ValidatePattern('^[a-f0-9]{64}$')][string]$ProjectorSha)
# 모델 시험이 아니라 소비된717a의 기존 로그 읽기입니다. CLI/usage/모델/Discord/DB 업무는 없습니다.
$ErrorActionPreference='Stop'
$approvalPath='C:/ProgramData/LimitlessRelay/boot-approval.json'
$approvalSha='3e032ebcce3d91bd762a8dec81048d04607ddde761afd62489c0fa2b8e49140f'
$baseDir='C:/ProgramData/LimitlessRelay/InferenceLogReviews'
$sourceRequest='717a0afa735d485daf790579b964e8c2'
$helper=Join-Path $PSScriptRoot $(if($Child){'ProtectionHelpers.ps1'}else{'restore-service-environment.ps1'})
if((Get-FileHash -LiteralPath $helper -Algorithm SHA256).Hash.ToLower() -ne 'f0b3589cadf63a2c47d363b614453ecf4a72bc565769436ac8ebb9f5b4eca7a0'){throw 'LOG_HELPER_SHA_HOLD'}
$tokens=$null;$errors=$null;$ast=[Management.Automation.Language.Parser]::ParseFile($helper,[ref]$tokens,[ref]$errors)
if($errors.Count){throw 'LOG_HELPER_PARSE_HOLD'}
$names=@('Sha','No-Link','Read-Approval','Safe-Error','Boot-Snapshot','Ledger-Snapshot')
$defs=@($ast.FindAll({param($n) $n -is [Management.Automation.Language.FunctionDefinitionAst] -and $n.Name -in $names},$true))
if($defs.Count -ne 6){throw 'LOG_HELPER_SET_HOLD'}
foreach($definition in $defs){. ([scriptblock]::Create($definition.Extent.Text))}
function Set-ReviewAcl([string]$path,[string]$serviceSid,[string]$serviceRight){
 $identity=[Security.Principal.WindowsIdentity]::GetCurrent()
 $acl=[Security.AccessControl.DirectorySecurity]::new();$acl.SetAccessRuleProtection($true,$false);$acl.SetOwner($identity.User)
 foreach($sid in @($identity.User.Value,'S-1-5-18','S-1-5-32-544')){$acl.AddAccessRule([Security.AccessControl.FileSystemAccessRule]::new([Security.Principal.SecurityIdentifier]::new($sid),'FullControl','ContainerInherit,ObjectInherit','None','Allow'))}
 $acl.AddAccessRule([Security.AccessControl.FileSystemAccessRule]::new([Security.Principal.SecurityIdentifier]::new($serviceSid),$serviceRight,'ContainerInherit,ObjectInherit','None','Allow'))
 Set-Acl -LiteralPath $path -AclObject $acl
 if(-not (Get-Acl -LiteralPath $path).AreAccessRulesProtected){throw 'LOG_REVIEW_ACL_HOLD'}
}
if($Child){
 $watch=[Diagnostics.Stopwatch]::StartNew();$folder=Join-Path $baseDir $ReviewId;$out=Join-Path $folder 'Output'
 $r=@{status='HOLD';review_id=$ReviewId;source_request_id=$sourceRequest;model_calls=0;official_usage_reads=0;sqlite_work_operations=0;discord_posts=0}
 try{
  if(-not $ReviewId -or (Sha $PSCommandPath) -ne $ScriptSha){throw 'LOG_CHILD_SCRIPT_PIN_HOLD'}
  $a=Read-Approval;$identity=[Security.Principal.WindowsIdentity]::GetCurrent()
  if($identity.User.Value -ne $a.service_sid -or $identity.Name -ne 'PAUL-PC\LIMITLESSRelaySvc' -or ([Security.Principal.WindowsPrincipal]::new($identity)).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)){throw 'LOG_SERVICE_IDENTITY_HOLD'}
  $metadata=Join-Path $folder 'BatchReadOnlyMetadata.psm1'
  if((Sha $metadata) -ne '09458bfc9a2b5516784efa7e9bda7013506dc0c21673b8aad7aec84240b60faf'){throw 'LOG_METADATA_PIN_HOLD'}
  Import-Module $metadata -Force;$logon=[LimitlessBatchMetadataV1]::CurrentLogon()
  if($logon[0] -ne 4 -or $logon[1] -ne 0 -or [Environment]::UserInteractive){throw 'LOG_SESSION0_LOGON4_HOLD'}
  $r.account=$identity.Name;$r.sid_matches=$true;$r.session_id=0;$r.logon_type=4
  $projector=Join-Path $folder 'project-existing-inference-log.py'
  if((Sha $projector) -ne $ProjectorSha){throw 'LOG_PROJECTOR_PIN_HOLD'}
  $jobSource=Join-Path $folder 'UsageOnlyJob.cs'
  if((Sha $jobSource) -ne '4be51b7fdbabb681bf822c5d22cd624f64bcf73f483fd017575a6e81759fd155'){throw 'LOG_JOB_PIN_HOLD'}
  Add-Type -Path $jobSource
  $budget=[int][Math]::Min(14000,17000-$watch.ElapsedMilliseconds)
  if($budget -lt 1000){throw 'LOG_PREPARATION_DEADLINE_HOLD'}
  $j=[LimitlessUsageJobV1]::Run((Join-Path $a.tool_root '.venv/Scripts/python.exe'),@('-I','-B','-X','utf8',$projector,'--review-id',$ReviewId),$folder,$budget)
  $r.job=@{exit_code=$j.ExitCode;timed_out=$j.TimedOut;cleanup_verified=$j.CleanupVerified;active_processes=$j.ActiveProcesses;stdout_truncated=$j.StdoutTruncated;stderr_truncated=$j.StderrTruncated}
  $v=$j.Stdout|ConvertFrom-Json
  if($v.status -eq 'SANITIZED_LOG_PROJECTION_WRITTEN' -and $v.review_id -eq $ReviewId -and $v.source_request_id -eq $sourceRequest -and $v.model_calls -eq 0 -and $v.official_usage_reads -eq 0 -and $j.ExitCode -eq 0 -and -not $j.TimedOut -and -not $j.StdoutTruncated -and $j.CleanupVerified -and $j.ActiveProcesses -eq 0 -and (Sha (Join-Path $out 'projection.json')) -eq $v.projection_sha256){$r.status='SANITIZED_LOG_PROJECTION_WRITTEN';$r.projection_sha256=$v.projection_sha256;$r.source_logs_unchanged=$v.source_logs_unchanged}
  else{$r.reason='LOG_PROJECTION_HOLD';$r.projector_reason=if($v.reason -match '^[A-Z0-9_]+$'){$v.reason}else{'UNCONFIRMED'}}
 }catch{$r.reason=if($_.Exception.Message -match '^LOG_[A-Z0-9_]+$'){$_.Exception.Message}else{'LOG_CHILD_HOLD'};$r.error_chain=Safe-Error $_}
 $r|ConvertTo-Json -Depth 10|Set-Content -LiteralPath (Join-Path $out 'child-summary.json') -Encoding utf8
 if($r.status -eq 'SANITIZED_LOG_PROJECTION_WRITTEN'){exit 0}else{exit 2}
}
$identity=[Security.Principal.WindowsIdentity]::GetCurrent()
if(-not ([Security.Principal.WindowsPrincipal]::new($identity)).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)){Write-Output 'HOLD_ADMIN_REQUIRED_EXISTING_LOGS_ONLY_NO_MODEL_NO_USAGE';exit 2}
$nonce=[guid]::NewGuid().ToString('N');$name='LIMITLESS-ExistingLogReview-'+$nonce;$marker='EXISTING_LOGS_ONLY_NO_AI_'+$nonce
$registered=$false;$credential=$null;$plain=$null;$out=$null;$before=$null;$ledgerBefore=$null
$r=@{status='HOLD';review_id=$nonce;source_request_id=$sourceRequest;model_calls=0;official_usage_reads=0;sqlite_work_operations=0;discord_posts=0;new_ai_permit_created=$false}
try{
 $a=Read-Approval;$before=Boot-Snapshot;$ledgerBefore=Ledger-Snapshot $a.tool_root
 $inferenceMarker='C:/ProgramData/LimitlessRelay/InferenceOnce90-'+$sourceRequest+'.launch-consumed.json';$markerBefore=Sha $inferenceMarker
 if((Test-Path -LiteralPath 'C:/ProgramData/LimitlessRelay/inference-once90-approval.json') -or (Test-Path -LiteralPath 'C:/ProgramData/LimitlessRelay/usage-only-approval.json')){throw 'LOG_ACTIVE_AI_PERMIT_HOLD'}
 No-Link 'C:/ProgramData/LimitlessRelay'
 if(Test-Path -LiteralPath $baseDir){No-Link $baseDir}else{$null=New-Item -ItemType Directory -Path $baseDir}
 $folder=Join-Path $baseDir $nonce;if(Test-Path -LiteralPath $folder){throw 'LOG_REVIEW_FOLDER_EXISTS_HOLD'};$null=New-Item -ItemType Directory -Path $folder
 Set-ReviewAcl $folder $a.service_sid 'ReadAndExecute'
 $childScript=Join-Path $folder 'review-existing-inference-log.ps1';Copy-Item -LiteralPath $PSCommandPath -Destination $childScript
 Copy-Item -LiteralPath $helper -Destination (Join-Path $folder 'ProtectionHelpers.ps1')
 $projector=Join-Path $PSScriptRoot 'project-existing-inference-log.py';Copy-Item -LiteralPath $projector -Destination (Join-Path $folder 'project-existing-inference-log.py')
 $jobSource=Join-Path $PSScriptRoot 'UsageOnlyJob.cs';Copy-Item -LiteralPath $jobSource -Destination (Join-Path $folder 'UsageOnlyJob.cs')
 $metadata=Join-Path $a.tool_root 'BatchReadOnlyMetadata.psm1';Copy-Item -LiteralPath $metadata -Destination (Join-Path $folder 'BatchReadOnlyMetadata.psm1')
 if((Sha $childScript) -ne (Sha $PSCommandPath) -or (Sha (Join-Path $folder 'project-existing-inference-log.py')) -ne (Sha $projector)){throw 'LOG_COPY_SHA_HOLD'}
 $out=Join-Path $folder 'Output';$null=New-Item -ItemType Directory -Path $out;Set-ReviewAcl $out $a.service_sid 'Modify'
 $credential=Get-Credential -UserName 'PAUL-PC\LIMITLESSRelaySvc' -Message '소비된717a 시험의 기존 로그 읽기 전용입니다. 모델/usage 조회는 없습니다. 서비스 Windows 암호는 로컬 입력에만 입력하세요.'
 if(-not $credential -or $credential.UserName -ine 'PAUL-PC\LIMITLESSRelaySvc'){throw 'LOG_LOCAL_CREDENTIAL_HOLD'}
 $arguments='-NoProfile -NonInteractive -WindowStyle Hidden -File "'+$childScript+'" -Child -ReviewId '+$nonce+' -ScriptSha '+(Sha $childScript)+' -ProjectorSha '+(Sha $projector)
 $action=New-ScheduledTaskAction -Execute (Get-Command pwsh).Source -Argument $arguments -WorkingDirectory $folder
 $settings=New-ScheduledTaskSettingsSet -ExecutionTimeLimit (New-TimeSpan -Seconds 20) -MultipleInstances IgnoreNew
 $plain=$credential.GetNetworkCredential().Password
 Register-ScheduledTask -TaskName $name -TaskPath '\' -Description $marker -Action $action -Settings $settings -User $credential.UserName -Password $plain -RunLevel Limited|Out-Null
 $registered=$true;$plain=$null;$task=Get-ScheduledTask -TaskName $name -TaskPath '\'
 if($task.Principal.LogonType.ToString() -ne 'Password' -or $task.Principal.RunLevel.ToString() -ne 'Limited' -or [Xml.XmlConvert]::ToTimeSpan($task.Settings.ExecutionTimeLimit).TotalSeconds -ne 20 -or $task.Settings.RestartCount -ne 0 -or -not $task.Settings.AllowHardTerminate){throw 'LOG_TASK_POLICY_HOLD'}
 Start-ScheduledTask -TaskName $name -TaskPath '\'
 for($poll=0;$poll -lt 28;$poll++){Start-Sleep -Seconds 1;$task=Get-ScheduledTask -TaskName $name -TaskPath '\';$info=Get-ScheduledTaskInfo -TaskName $name -TaskPath '\';if($task.State.ToString() -ne 'Running' -and $info.LastRunTime.Year -gt 2000){break}}
 $r.task_exit_code=$info.LastTaskResult;$childSummaryPath=Join-Path $out 'child-summary.json'
 if(Test-Path -LiteralPath $childSummaryPath){$c=Get-Content -LiteralPath $childSummaryPath -Raw|ConvertFrom-Json;if($c.review_id -ne $nonce){throw 'LOG_RESULT_ID_HOLD'};$r.child=$c;if($c.status -eq 'SANITIZED_LOG_PROJECTION_WRITTEN' -and $r.task_exit_code -eq 0){$r.status=$c.status;$r.projection_path=Join-Path $out 'projection.json';$r.projection_sha256=$c.projection_sha256}}
}catch{$r.reason=if($_.Exception.Message -match '^LOG_[A-Z0-9_]+$'){$_.Exception.Message}else{'LOG_PARENT_HOLD'};$r.error_chain=Safe-Error $_}
finally{
 $plain=$null;if($credential){$credential.Password.Dispose()};$credential=$null
 try{
  if($registered){$task=Get-ScheduledTask -TaskName $name -TaskPath '\';if($task.Description -ne $marker -or -not $task.Actions[0].Arguments.Contains($nonce)){throw 'LOG_CLEANUP_ID_HOLD'};if($task.State.ToString() -eq 'Running'){Stop-ScheduledTask -TaskName $name -TaskPath '\';Start-Sleep -Seconds 2;$r.status='HOLD';$task=Get-ScheduledTask -TaskName $name -TaskPath '\'};if($task.State.ToString() -eq 'Running'){throw 'LOG_TASK_RUNNING_HOLD'};Unregister-ScheduledTask -TaskName $name -TaskPath '\' -Confirm:$false;$r.cleanup='REMOVED'}else{$r.cleanup='NOT_REGISTERED'}
  $r.approval_unchanged=((Sha $approvalPath) -eq $approvalSha);if(-not $r.approval_unchanged){throw 'LOG_APPROVAL_CHANGED_HOLD'}
  if($markerBefore){$r.inference_consumption_marker_preserved=((Sha $inferenceMarker) -eq $markerBefore);if(-not $r.inference_consumption_marker_preserved){throw 'LOG_CONSUMPTION_MARKER_CHANGED_HOLD'}}
  if($before){$after=Boot-Snapshot;$same=$true;foreach($n in $before.Keys){if($before[$n] -ne $after[$n]){$same=$false}};$r.boot_tasks_unchanged=$same;if(-not $same){throw 'LOG_BOOT_CHANGED_HOLD'}}
  if($ledgerBefore){$r.ledger_unchanged=((Ledger-Snapshot $a.tool_root) -ceq $ledgerBefore);if(-not $r.ledger_unchanged){throw 'LOG_LEDGER_CHANGED_HOLD'}}
 }catch{$r.status='HOLD';$r.cleanup_or_protection='NOT_VERIFIED';$r.cleanup_error=Safe-Error $_}
 if($out){$r|ConvertTo-Json -Depth 12|Set-Content -LiteralPath (Join-Path $out 'summary.json') -Encoding utf8}
}
$r|ConvertTo-Json -Depth 12
if($r.status -eq 'SANITIZED_LOG_PROJECTION_WRITTEN'){exit 0}else{exit 2}

param([switch]$Child,[ValidatePattern('^[a-f0-9]{32}$')][string]$RequestId,
 [ValidatePattern('^[a-f0-9]{64}$')][string]$ScriptSha,[ValidatePattern('^[a-f0-9]{64}$')][string]$ManifestSha)
# 관리자 직접 명령으로만 새 격리 시험을 시작합니다. 운영 runner/OnBoot/SQLite 업무는 호출하지 않습니다.
$ErrorActionPreference='Stop'
$approvalPath='C:/ProgramData/LimitlessRelay/boot-approval.json'
$approvalSha='3e032ebcce3d91bd762a8dec81048d04607ddde761afd62489c0fa2b8e49140f'
$permitPath='C:/ProgramData/LimitlessRelay/inference-once90-approval.json'
$trialBase='C:/ProgramData/LimitlessRelay/InferenceTrials'
$helper=Join-Path $PSScriptRoot $(if($Child){'ProtectionHelpers.ps1'}else{'restore-service-environment.ps1'})
if((Get-FileHash -LiteralPath $helper -Algorithm SHA256).Hash.ToLower() -ne 'f0b3589cadf63a2c47d363b614453ecf4a72bc565769436ac8ebb9f5b4eca7a0'){throw 'INFERENCE_HELPER_SHA_HOLD'}
$tokens=$null;$errors=$null;$ast=[Management.Automation.Language.Parser]::ParseFile($helper,[ref]$tokens,[ref]$errors)
if($errors.Count){throw 'INFERENCE_HELPER_PARSE_HOLD'}
$names=@('Sha','No-Link','Read-Approval','Safe-Error','Boot-Snapshot','Ledger-Snapshot')
$defs=@($ast.FindAll({param($n) $n -is [Management.Automation.Language.FunctionDefinitionAst] -and $n.Name -in $names},$true))
if($defs.Count -ne 6){throw 'INFERENCE_HELPER_SET_HOLD'}
foreach($definition in $defs){. ([scriptblock]::Create($definition.Extent.Text))}
function New-FileJson([string]$path,$value){
 $bytes=[Text.UTF8Encoding]::new($false).GetBytes(($value|ConvertTo-Json -Depth 24))
 $stream=[IO.File]::Open($path,[IO.FileMode]::CreateNew,[IO.FileAccess]::Write,[IO.FileShare]::Read)
 try{$stream.Write($bytes,0,$bytes.Length)}finally{$stream.Dispose()}
}
function Set-TrialAcl([string]$path,[string]$serviceSid,[string]$serviceRight){
 # 이번 새 trial/Output에만 적용하며 기존 서비스/ProgramData ACL은 변경하지 않습니다.
 $acl=[Security.AccessControl.DirectorySecurity]::new();$acl.SetAccessRuleProtection($true,$false)
 $identity=[Security.Principal.WindowsIdentity]::GetCurrent();$acl.SetOwner($identity.User)
 foreach($sid in @($identity.User.Value,'S-1-5-18','S-1-5-32-544')){
  $acl.AddAccessRule([Security.AccessControl.FileSystemAccessRule]::new([Security.Principal.SecurityIdentifier]::new($sid),'FullControl','ContainerInherit,ObjectInherit','None','Allow'))
 }
 $acl.AddAccessRule([Security.AccessControl.FileSystemAccessRule]::new([Security.Principal.SecurityIdentifier]::new($serviceSid),$serviceRight,'ContainerInherit,ObjectInherit','None','Allow'))
 Set-Acl -LiteralPath $path -AclObject $acl
 $actual=Get-Acl -LiteralPath $path
 if(-not $actual.AreAccessRulesProtected){throw 'INFERENCE_TRIAL_ACL_HOLD'}
 $allow=@($identity.User.Value,'S-1-5-18','S-1-5-32-544',$serviceSid)
 foreach($rule in $actual.GetAccessRules($true,$true,[Security.Principal.SecurityIdentifier])){
  if($rule.IdentityReference.Value -notin $allow -or $rule.AccessControlType.ToString() -ne 'Allow'){throw 'INFERENCE_TRIAL_ACL_SCOPE_HOLD'}
 }
}
function Verify-Policy($p){
 if($p.application_retries -ne 0 -or $p.inference_exec_max -ne 1 -or $p.usage_app_server_max -ne 1 -or
    $p.internal_http_retries -ne 'OFFICIAL_BUILTIN_DEFAULT_ALLOWED' -or $p.internal_stream_reconnects -ne 'OFFICIAL_BUILTIN_DEFAULT_ALLOWED' -or
    $p.remote_requests_exactly_one_guaranteed -ne $false -or $p.local_limit_seconds -ne 90 -or $p.discord_posts -ne 0 -or $p.sqlite_work_operations -ne 0 -or $p.mode -ne 'MANUAL'){throw 'INFERENCE_POLICY_HOLD'}
}
function Get-InferenceBudgets([double]$elapsedMs){
 # 서비스 준비에 쓴 시간을 다시 주지 않고 부모의 permit/task 정리에도 시간을 남깁니다.
 $nativeMs=[int][Math]::Min(75000,[Math]::Floor(77000-$elapsedMs))
 if($nativeMs -lt 6000){throw 'INFERENCE_PREPARATION_DEADLINE_HOLD'}
 return @{native_ms=$nativeMs;worker_ms=[int][Math]::Min(70000,$nativeMs-3000)}
}
if($Child){
 $watch=[Diagnostics.Stopwatch]::StartNew();$folder=Join-Path $trialBase $RequestId;$out=Join-Path $folder 'Output'
 $r=@{status='NOT_VERIFIED';request_id=$RequestId;stage='SERVICE_IDENTITY';inference_exec_launches=$null;usage_app_server_launches=$null;application_retries=0;discord_posts=0;sqlite_work_operations=0}
 $private=$null;$homeBefore=$env:CODEX_HOME
 try{
  if(-not $RequestId -or (Sha $PSCommandPath) -ne $ScriptSha -or (Sha (Join-Path $folder 'Manifest.json')) -ne $ManifestSha){throw 'INFERENCE_CHILD_PIN_HOLD'}
  No-Link $folder;No-Link $out
  $m=Get-Content -LiteralPath (Join-Path $folder 'Manifest.json') -Raw|ConvertFrom-Json
  if($m.request_id -ne $RequestId){throw 'INFERENCE_REQUEST_HOLD'};Verify-Policy $m.permit.policy
  $a=Read-Approval;$identity=[Security.Principal.WindowsIdentity]::GetCurrent()
  if($identity.Name -ne 'PAUL-PC\LIMITLESSRelaySvc' -or $identity.User.Value -ne $a.service_sid -or ([Security.Principal.WindowsPrincipal]::new($identity)).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)){throw 'INFERENCE_SERVICE_IDENTITY_HOLD'}
  $metadata=Join-Path $folder 'BatchReadOnlyMetadata.psm1'
  if((Sha $metadata) -ne $m.metadata_sha256){throw 'INFERENCE_METADATA_SHA_HOLD'}
  Import-Module $metadata -Force;$logon=[LimitlessBatchMetadataV1]::CurrentLogon()
  if($logon[0] -ne 4 -or $logon[1] -ne 0 -or [Environment]::UserInteractive){throw 'INFERENCE_SESSION0_LOGON4_HOLD'}
  $r.account=$identity.Name;$r.sid_matches=$true;$r.session_id=0;$r.logon_type=4
  $serviceProfile=[Environment]::GetFolderPath([Environment+SpecialFolder]::UserProfile)
  if($serviceProfile -ine 'C:\Users\LIMITLESSRelaySvc' -or $env:USERPROFILE -ine $serviceProfile){throw 'INFERENCE_PROFILE_HOLD'}
  $serviceCodexHome=Join-Path $serviceProfile '.codex';No-Link $serviceCodexHome
  if($homeBefore -and $homeBefore -ine $serviceCodexHome){throw 'INFERENCE_CODEX_HOME_HOLD'}
  $privateRoot=Join-Path $serviceCodexHome 'limitless-inference-supervisor'
  if(Test-Path -LiteralPath $privateRoot){No-Link $privateRoot}else{$null=New-Item -ItemType Directory -Path $privateRoot}
  $private=Join-Path $privateRoot $RequestId
  if(Test-Path -LiteralPath $private){throw 'INFERENCE_PRIVATE_EXISTS_HOLD'};$null=New-Item -ItemType Directory -Path $private
  $permit=Get-Content -LiteralPath $permitPath -Raw|ConvertFrom-Json
  $launch=Get-Content -LiteralPath (Join-Path 'C:/ProgramData/LimitlessRelay' ('InferenceOnce90-'+$RequestId+'.launch-consumed.json')) -Raw|ConvertFrom-Json
  if($permit.approved -ne $true -or $permit.request_id -ne $RequestId -or $permit.kind -ne 'INFERENCE_ONCE90' -or $launch.request_id -ne $RequestId){throw 'INFERENCE_PERMIT_HOLD'}
  $package=Join-Path $folder 'Package'
  foreach($p in $m.files_sha256.PSObject.Properties){if((Sha (Join-Path $package $p.Name)) -ne $p.Value){throw 'INFERENCE_PACKAGE_SHA_HOLD'}}
  $jobSource=Join-Path $folder 'InferenceJob90.cs'
  if((Sha $jobSource) -ne $m.job_sha256){throw 'INFERENCE_JOB_SHA_HOLD'};Add-Type -Path $jobSource
  $env:LIMITLESS_BATCH_LOGON_TYPE='4';$env:LIMITLESS_BATCH_SESSION_ID='0'
  New-FileJson (Join-Path $out 'child-run-consumed.json') @{request_id=$RequestId;kind='INFERENCE_ONCE90_NO_RETRY'}
  $budgets=Get-InferenceBudgets $watch.Elapsed.TotalMilliseconds
  $budget=$budgets.native_ms;$workerBudget=$budgets.worker_ms
  $entry=Join-Path $package 'Tools/DiscordAIServiceDiagnostics/inference-once90.py'
  $tool=Join-Path $package 'Tools/DiscordAICollab';$python=Join-Path $a.tool_root '.venv/Scripts/python.exe'
  $r.stage='JOB_SUPERVISED_INFERENCE';$r.supervisor_budget_ms=$budget;$r.worker_budget_ms=$workerBudget
  $j=[LimitlessInferenceJob90V1]::Run($python,@('-I','-B','-X','utf8',$entry,'--request-id',$RequestId,'--budget-ms',[string]$workerBudget),$tool,$budget)
  $r.job=@{timed_out=$j.TimedOut;exit_code=$j.ExitCode;active_processes=$j.ActiveProcesses;cleanup_verified=$j.CleanupVerified;stdout_truncated=$j.StdoutTruncated;stderr_truncated=$j.StderrTruncated}
  $v=$null
  try{
   $v=$j.Stdout|ConvertFrom-Json;Verify-Policy $v.policy
   if($v.request_id -ne $RequestId -or $v.status -notin @('VERIFIED','NOT_VERIFIED') -or $v.reason -notmatch '^[A-Z0-9_]{1,100}$' -or $v.inference_exec_launches -notin @(0,1) -or $v.usage_app_server_launches -notin @(0,1) -or $v.application_retries -ne 0 -or $v.discord_posts -ne 0 -or $v.sqlite_work_operations -ne 0 -or $v.mode_changed -ne $false){throw 'INFERENCE_OUTPUT_SCHEMA_HOLD'}
   # 공개 결과는 고정 응답·숫자·허용 진단만 담고 자유문 stderr는 자기 보호 프로필에만 둡니다.
   $r.entry_result=$v;$r.inference_exec_launches=$v.inference_exec_launches;$r.usage_app_server_launches=$v.usage_app_server_launches
  }catch{$v=$null;$r.output_status='INFERENCE_OUTPUT_NOT_VERIFIED'}
  New-FileJson (Join-Path $private 'supervisor.json') @{stage=$r.stage;request_id=$RequestId;job=$r.job;stderr=$j.StderrSummary}
  $r.codex_home_unchanged=($homeBefore -ceq $env:CODEX_HOME)
  if($v -and $v.status -eq 'VERIFIED' -and $v.inference_exec_launches -eq 1 -and $v.usage_app_server_launches -eq 1 -and $v.validation.status -eq 'PASS' -and $v.validation.final_response -ceq 'LIMITLESS 기획 담당 AI입니다. 읽기 전용 연결 시험에 성공했습니다.' -and $j.ExitCode -eq 0 -and -not $j.TimedOut -and -not $j.StdoutTruncated -and $j.CleanupVerified -and $j.ActiveProcesses -eq 0 -and $r.codex_home_unchanged){$r.status='VERIFIED'}
  else{$r.reason='INFERENCE_NOT_VERIFIED_NO_RETRY'}
 }catch{
  $r.reason=if($_.Exception.Message -match '^INFERENCE_[A-Z0-9_]+$'){$_.Exception.Message}else{'INFERENCE_CHILD_HOLD'};$r.error_chain=Safe-Error $_
  if($private){try{New-FileJson (Join-Path $private 'failure.json') $r}catch{}}
 }
 $r.child_elapsed_ms=$watch.ElapsedMilliseconds
 if($watch.ElapsedMilliseconds -ge 90000){$r.status='NOT_VERIFIED';$r.reason='INFERENCE_CHILD_DEADLINE_HOLD'}
 $r|ConvertTo-Json -Depth 20|Set-Content -LiteralPath (Join-Path $out 'child-summary.json') -Encoding utf8
 if($r.status -eq 'VERIFIED'){exit 0}else{exit 2}
}
$identity=[Security.Principal.WindowsIdentity]::GetCurrent()
if(-not ([Security.Principal.WindowsPrincipal]::new($identity)).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)){Write-Output 'HOLD_ADMIN_POWERSHELL_REQUIRED_NO_USAGE_NO_INFERENCE';exit 2}
$repo=Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
$prepared=Join-Path (Split-Path $repo -Parent) 'InferenceTrial90AuthFixPrepared20261026'
$manifestPath=Join-Path $prepared 'Manifest.json';$m=Get-Content -LiteralPath $manifestPath -Raw|ConvertFrom-Json
if($m.kind -ne 'PREPARED_INFERENCE_ONCE90' -or $m.request_id -notmatch '^[a-f0-9]{32}$' -or $m.permit.approved -ne $false -or (Sha $PSCommandPath) -ne $m.launcher_sha256){throw 'INFERENCE_PREPARED_MANIFEST_HOLD'}
Verify-Policy $m.permit.policy
$authProofPath='C:/ProgramData/LimitlessRelay/InferenceLogReviews/93fa150e749d4d6ea661d6040e3aed8b/Output/projection.json'
if((Sha $authProofPath) -ne '20b0e1b09c31b3f772b46f773448e4d30662482a839c21e1e0f58816824fcb15'){throw 'INFERENCE_AUTHFIX_PROOF_SHA_HOLD'}
$authProof=Get-Content -LiteralPath $authProofPath -Raw|ConvertFrom-Json
if($authProof.current_auth_config_metadata.sha256 -ne '818c2e7b52f1c574fd1f9e0ac49dd876ad10746ff5ad95ce427d636938747bc3' -or $authProof.current_auth_config_metadata.credential_store -ne 'keyring' -or $authProof.current_auth_config_metadata.forced_login_method -ne 'chatgpt' -or -not $authProof.service_sid_matches -or -not $authProof.source_logs_unchanged){throw 'INFERENCE_AUTHFIX_SERVICE_PROOF_HOLD'}
if($m.permit.entry_sha256 -ne 'b841cf6557046ada96b67564be7e093dcb0666980577de8ab37273dd0c22b1ab'){throw 'INFERENCE_AUTHFIX_CANDIDATE_SHA_HOLD'}
$nonce=$m.request_id;$fuse=Join-Path 'C:/ProgramData/LimitlessRelay' ('InferenceOnce90-'+$nonce+'.launch-consumed.json')
if(Test-Path -LiteralPath $fuse){Write-Output 'HOLD_THIS_APPROVAL_ALREADY_LAUNCHED_NO_RETRY';exit 2}
$name='LIMITLESS-Inference90-'+$nonce;$marker='OWNER_APPROVED_INFERENCE90_'+$nonce
$r=@{status='NOT_VERIFIED';stage='DEPLOYMENT_PREFLIGHT';request_id=$nonce;inference_exec_launches=$null;usage_app_server_launches=$null;application_retries=0;discord_posts=0;sqlite_work_operations=0;parent_is_admin=$true;parent_session_id=(Get-Process -Id $PID).SessionId;trial_task_launches=0}
$before=$null;$ledgerBefore=$null;$registered=$false;$permitCreated=$false;$out=$null;$folder=$null;$credential=$null;$plain=$null;$permitSha=$null;$executionWatch=$null
try{
 $a=Read-Approval;$before=Boot-Snapshot;$ledgerBefore=Ledger-Snapshot $a.tool_root
 foreach($p in $a.runtime_sha256.PSObject.Properties){if((Sha (Join-Path $a.tool_root $p.Name)) -ne $p.Value){throw 'INFERENCE_OPERATING_RUNTIME_SHA_HOLD'}}
 foreach($p in $m.previous_markers_sha256.PSObject.Properties){if((Sha $p.Name) -ne $p.Value){throw 'INFERENCE_PREVIOUS_MARKER_HOLD'}}
 if((Sha $a.service_codex.path) -ne $a.service_codex.sha256 -or $a.service_codex.sha256 -ne $m.permit.cli_sha256){throw 'INFERENCE_CLI_REFERENCE_HOLD'}
 $packageRoot=Split-Path (Split-Path $a.service_codex.path -Parent) -Parent
 if(@(Get-ChildItem -LiteralPath $packageRoot -File -Recurse).Count -ne 45){throw 'INFERENCE_OFFICIAL_PACKAGE_COUNT_HOLD'}
 foreach($p in $m.official_package_sha256.PSObject.Properties){if((Sha (Join-Path $packageRoot $p.Name)) -ne $p.Value){throw 'INFERENCE_OFFICIAL_PACKAGE_SHA_HOLD'}}
 $oldNames=@($a.runtime_sha256.PSObject.Properties.Name);$newNames=@($m.permit.runtime_sha256.PSObject.Properties.Name)
 $changed=@('collab/usage_bridge.py','collab/service_ai.py','collab/service_batch_once.py')
 if($oldNames.Count -ne 43 -or $newNames.Count -ne 44 -or @($newNames|Where-Object {$_ -notin $oldNames}).Count -ne 1 -or 'collab/usage_diagnostics.py' -notin $newNames -or @($oldNames|Where-Object {$_ -notin $newNames}).Count){throw 'INFERENCE_RUNTIME_SET_HOLD'}
 foreach($k in $oldNames){if($k -notin $changed -and $m.permit.runtime_sha256.$k -ne $a.runtime_sha256.$k){throw 'INFERENCE_UNCHANGED40_HOLD'}}
 if((Test-Path -LiteralPath $permitPath) -or (Test-Path -LiteralPath 'C:/ProgramData/LimitlessRelay/usage-only-approval.json')){throw 'INFERENCE_ACTIVE_PERMIT_HOLD'}
 foreach($pair in @(@('InferenceJob90.cs',$m.job_sha256),@('inference-package.zip',$m.archive_sha256))){
  $source=if($pair[0] -eq 'InferenceJob90.cs'){Join-Path $PSScriptRoot $pair[0]}else{Join-Path $prepared $pair[0]}
  if((Sha $source) -ne $pair[1]){throw 'INFERENCE_SOURCE_PIN_HOLD'}
 }
 No-Link 'C:/ProgramData/LimitlessRelay'
 if(Test-Path -LiteralPath $trialBase){No-Link $trialBase}else{$null=New-Item -ItemType Directory -Path $trialBase}
 $folder=Join-Path $trialBase $nonce
 if(Test-Path -LiteralPath $folder){throw 'INFERENCE_TRIAL_ALREADY_EXISTS_HOLD'};$null=New-Item -ItemType Directory -Path $folder
 Set-TrialAcl $folder $a.service_sid 'ReadAndExecute'
 $childScript=Join-Path $folder 'run-approved-inference-authfix-once90.ps1';Copy-Item -LiteralPath $PSCommandPath -Destination $childScript
 Copy-Item -LiteralPath $manifestPath -Destination (Join-Path $folder 'Manifest.json')
 Copy-Item -LiteralPath $helper -Destination (Join-Path $folder 'ProtectionHelpers.ps1')
 Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'InferenceJob90.cs') -Destination (Join-Path $folder 'InferenceJob90.cs')
 $metadata=Join-Path $a.tool_root 'BatchReadOnlyMetadata.psm1'
 if((Sha $metadata) -ne $m.metadata_sha256){throw 'INFERENCE_METADATA_SOURCE_HOLD'}
 Copy-Item -LiteralPath $metadata -Destination (Join-Path $folder 'BatchReadOnlyMetadata.psm1')
 $archive=Join-Path $folder 'package.zip';Copy-Item -LiteralPath (Join-Path $prepared 'inference-package.zip') -Destination $archive
 if((Sha $archive) -ne $m.archive_sha256){throw 'INFERENCE_ARCHIVE_COPY_HOLD'}
 $allowed=[Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
 foreach($p in $m.files_sha256.PSObject.Properties){$null=$allowed.Add($p.Name)}
 $zip=[IO.Compression.ZipFile]::OpenRead($archive)
 try{
  $seen=[Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
  foreach($e in $zip.Entries){if(-not $allowed.Contains($e.FullName) -or -not $seen.Add($e.FullName) -or $e.FullName.Contains('..') -or $e.FullName.Contains('\') -or $e.FullName.StartsWith('/')){throw 'INFERENCE_ARCHIVE_SCOPE_HOLD'}}
  if(-not $seen.SetEquals($allowed) -or $allowed.Count -ne 47){throw 'INFERENCE_ARCHIVE_SET_HOLD'}
 }finally{$zip.Dispose()}
 $package=Join-Path $folder 'Package';[IO.Compression.ZipFile]::ExtractToDirectory($archive,$package,$false)
 foreach($p in $m.files_sha256.PSObject.Properties){if((Sha (Join-Path $package $p.Name)) -ne $p.Value){throw 'INFERENCE_DEPLOYED_SHA_HOLD'}}
 $null=New-Item -ItemType Directory -Path (Join-Path $folder 'EmptyWorkspace')
 $out=Join-Path $folder 'Output';$null=New-Item -ItemType Directory -Path $out;Set-TrialAcl $out $a.service_sid 'Modify'
 $r.deployment='PASS_44_EXACT_40_PRESERVED';$r.stage='LOCAL_SECURE_INPUT'
 $credential=Get-Credential -UserName 'PAUL-PC\LIMITLESSRelaySvc' -Message '공식 usage App Server 최대1개 후 읽기전용 추론 exec 최대1개/전체90초입니다. Windows 암호는 로컬 보안 입력에만 입력하세요.'
 if(-not $credential -or $credential.UserName -ine 'PAUL-PC\LIMITLESSRelaySvc'){throw 'INFERENCE_LOCAL_CREDENTIAL_HOLD'}
 $executionWatch=[Diagnostics.Stopwatch]::StartNew()
 $taskArguments='-NoProfile -NonInteractive -WindowStyle Hidden -File "'+$childScript+'" -Child -RequestId '+$nonce+' -ScriptSha '+(Sha $childScript)+' -ManifestSha '+(Sha (Join-Path $folder 'Manifest.json'))
 $action=New-ScheduledTaskAction -Execute (Get-Command pwsh).Source -Argument $taskArguments -WorkingDirectory $folder
 $settings=New-ScheduledTaskSettingsSet -ExecutionTimeLimit (New-TimeSpan -Seconds 90) -MultipleInstances IgnoreNew
 $plain=$credential.GetNetworkCredential().Password
 Register-ScheduledTask -TaskName $name -TaskPath '\' -Description $marker -Action $action -Settings $settings -User $credential.UserName -Password $plain -RunLevel Limited|Out-Null
 $registered=$true;$plain=$null;$task=Get-ScheduledTask -TaskName $name -TaskPath '\'
 if($task.Principal.LogonType.ToString() -ne 'Password' -or $task.Principal.RunLevel.ToString() -ne 'Limited' -or [Xml.XmlConvert]::ToTimeSpan($task.Settings.ExecutionTimeLimit).TotalSeconds -ne 90 -or $task.Settings.RestartCount -ne 0 -or -not $task.Settings.AllowHardTerminate){throw 'INFERENCE_TASK_POLICY_HOLD'}
 $permit=$m.permit;$permit.approved=$true;$permit.expires_at=[DateTimeOffset]::UtcNow.ToUnixTimeSeconds()+300;$permit.trial_tool_root=Join-Path $package 'Tools/DiscordAICollab'
 New-FileJson $permitPath $permit;$permitCreated=$true;$permitSha=Sha $permitPath
 New-FileJson $fuse @{request_id=$nonce;kind='OWNER_APPROVED_INFERENCE90_KEYRING_VIEWIMAGE_FIX';created=[DateTimeOffset]::UtcNow.ToUnixTimeSeconds()}
 $r.stage='START_APPROVED_INFERENCE_ONCE90';$r.trial_task_launches=1
 Start-ScheduledTask -TaskName $name -TaskPath '\'
 while($executionWatch.ElapsedMilliseconds -lt 80000){Start-Sleep -Seconds 1;$task=Get-ScheduledTask -TaskName $name -TaskPath '\';$info=Get-ScheduledTaskInfo -TaskName $name -TaskPath '\';if($task.State.ToString() -ne 'Running' -and $info.LastRunTime.Year -gt 2000){break}}
 if($null -eq $info){throw 'INFERENCE_REGISTRATION_DEADLINE_HOLD'}
 $r.task_exit_code=$info.LastTaskResult
 $childSummaryPath=Join-Path $out 'child-summary.json'
 if(Test-Path -LiteralPath $childSummaryPath){
  $c=Get-Content -LiteralPath $childSummaryPath -Raw|ConvertFrom-Json
  if($c.request_id -ne $nonce){throw 'INFERENCE_RESULT_ID_HOLD'}
  $r.child=$c;$r.inference_exec_launches=$c.inference_exec_launches;$r.usage_app_server_launches=$c.usage_app_server_launches
  if($c.status -eq 'VERIFIED' -and $r.task_exit_code -eq 0){$r.status='VERIFIED'}
 }
}catch{$r.reason=if($_.Exception.Message -match '^INFERENCE_[A-Z0-9_]+$'){$_.Exception.Message}else{'INFERENCE_PARENT_HOLD'};$r.error_chain=Safe-Error $_}
finally{
 $plain=$null;if($credential){$credential.Password.Dispose()};$credential=$null
 try{
  if($permitCreated){if((Sha $permitPath) -ne $permitSha){throw 'INFERENCE_PERMIT_CHANGED_HOLD'};$used=Get-Content -LiteralPath $permitPath -Raw|ConvertFrom-Json;if($used.request_id -ne $nonce){throw 'INFERENCE_PERMIT_ID_HOLD'};Move-Item -LiteralPath $permitPath -Destination (Join-Path $folder 'permit.consumed.disabled.json');$r.permit_deactivated=$true}else{$r.permit_deactivated= -not (Test-Path -LiteralPath $permitPath)}
 }catch{$r.status='NOT_VERIFIED';$r.permit_cleanup='NOT_VERIFIED';$r.permit_error=Safe-Error $_}
 try{
  if($registered){$task=Get-ScheduledTask -TaskName $name -TaskPath '\';if($task.Description -ne $marker -or -not $task.Actions[0].Arguments.Contains($nonce)){throw 'INFERENCE_TASK_ID_HOLD'};if($task.State.ToString() -eq 'Running'){Stop-ScheduledTask -TaskName $name -TaskPath '\';Start-Sleep -Seconds 2;$task=Get-ScheduledTask -TaskName $name -TaskPath '\';$r.status='NOT_VERIFIED';$r.forced_task_stop=$true};if($task.State.ToString() -eq 'Running'){throw 'INFERENCE_TASK_RUNNING_HOLD'};Unregister-ScheduledTask -TaskName $name -TaskPath '\' -Confirm:$false;$r.cleanup='REMOVED'}else{$r.cleanup='NOT_REGISTERED'}
  $r.approval_unchanged=((Sha $approvalPath) -eq $approvalSha);if(-not $r.approval_unchanged){throw 'INFERENCE_APPROVAL_CHANGED_HOLD'}
  $r.previous_markers_preserved=$true;foreach($p in $m.previous_markers_sha256.PSObject.Properties){if((Sha $p.Name) -ne $p.Value){$r.previous_markers_preserved=$false;throw 'INFERENCE_PREVIOUS_MARKER_HOLD'}}
  if($before){$after=Boot-Snapshot;$same=$true;foreach($n in $before.Keys){if($before[$n] -ne $after[$n]){$same=$false}};$r.boot_tasks_unchanged=$same;if(-not $same){throw 'INFERENCE_BOOT_CHANGED_HOLD'}}
  if($ledgerBefore){$r.ledger_unchanged=((Ledger-Snapshot $a.tool_root) -ceq $ledgerBefore);if(-not $r.ledger_unchanged){throw 'INFERENCE_LEDGER_CHANGED_HOLD'}}
 }catch{$r.status='NOT_VERIFIED';$r.cleanup_or_protection='NOT_VERIFIED';$r.cleanup_error=Safe-Error $_}
 if($executionWatch){$r.local_execution_elapsed_ms=$executionWatch.ElapsedMilliseconds;if($executionWatch.ElapsedMilliseconds -ge 90000){$r.status='NOT_VERIFIED';$r.reason='INFERENCE_TOTAL_DEADLINE_NOT_VERIFIED'}}
 if($out){$r|ConvertTo-Json -Depth 24|Set-Content -LiteralPath (Join-Path $out 'summary.json') -Encoding utf8}
}
$r|ConvertTo-Json -Depth 24
if($r.status -eq 'VERIFIED'){exit 0}else{exit 2}

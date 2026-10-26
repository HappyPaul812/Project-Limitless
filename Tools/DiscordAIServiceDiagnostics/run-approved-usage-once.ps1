param([switch]$Child,[ValidatePattern('^[a-f0-9]{32}$')][string]$RequestId,[ValidatePattern('^[a-f0-9]{64}$')][string]$ScriptSha)
# 대표의 기존 단발 승인 전용입니다. 자동 재시도/모델/Discord/운영 배포는 없습니다.
$ErrorActionPreference='Stop'
$approvalPath='C:/ProgramData/LimitlessRelay/boot-approval.json'
$approvalSha='3e032ebcce3d91bd762a8dec81048d04607ddde761afd62489c0fa2b8e49140f'
$permitPath='C:/ProgramData/LimitlessRelay/usage-only-approval.json'
$fuse='C:/ProgramData/LimitlessRelay/UsageOnly20261026.bindingfix-e10d02cb.launch-consumed.json'
$previousFuse='C:/ProgramData/LimitlessRelay/UsageOnly20261026.launch-consumed.json'
$previousFuseSha='62a867f542d6800a68f9a626d1c792ace7143081a7ff8e15ce1638b412cab97b'
$trialBase='C:/ProgramData/LimitlessRelay/UsageTrials'
$helperSha='f0b3589cadf63a2c47d363b614453ecf4a72bc565769436ac8ebb9f5b4eca7a0'
$jobSha='4be51b7fdbabb681bf822c5d22cd624f64bcf73f483fd017575a6e81759fd155'
$metadataSha='09458bfc9a2b5516784efa7e9bda7013506dc0c21673b8aad7aec84240b60faf'
$archiveSha='55241c88d0f43c518faac6f66d7b892fc3128e4bffa320bfc899f613e5b466a9'
$draftSha='1f561edc3f68e1c7fd599b1608c4b43613e80e9a30705d89e27673bb6f9fae79'
$envResult='C:/ProgramData/LimitlessRelay/UsageEnvironmentChecks/c332610ebdfa43f1b4b13b3dea5120d6/Output/summary.json'
$envResultSha='a2ec822433f2116e690e673bddac95a6b5cd1036775fc22d6e43b2999870bf44'
$helper=Join-Path $PSScriptRoot $(if($Child){'ProtectionHelpers.ps1'}else{'restore-service-environment.ps1'})
if((Get-FileHash -LiteralPath $helper -Algorithm SHA256).Hash.ToLower() -ne $helperSha){throw 'USAGE_PROTECTION_HELPER_SHA_HOLD'}
# 검증된 기존 스크립트의 순수 보호 함수만 읽습니다. param/등록/실행 본문은 실행하지 않습니다.
$tokens=$null;$errors=$null;$ast=[Management.Automation.Language.Parser]::ParseFile($helper,[ref]$tokens,[ref]$errors)
if($errors.Count){throw 'USAGE_HELPER_PARSE_HOLD'}
$names=@('Sha','No-Link','Read-Approval','Safe-Error','Boot-Snapshot','Ledger-Snapshot')
$defs=@($ast.FindAll({param($n) $n -is [Management.Automation.Language.FunctionDefinitionAst] -and $n.Name -in $names},$true))
if($defs.Count -ne 6){throw 'USAGE_HELPER_SET_HOLD'}
foreach($definition in $defs){. ([scriptblock]::Create($definition.Extent.Text))}
function New-FileJson([string]$path,$value){
 $bytes=[Text.UTF8Encoding]::new($false).GetBytes(($value|ConvertTo-Json -Depth 20))
 $stream=[IO.File]::Open($path,[IO.FileMode]::CreateNew,[IO.FileAccess]::Write,[IO.FileShare]::Read)
 try{$stream.Write($bytes,0,$bytes.Length)}finally{$stream.Dispose()}
}
function Report-Number($number){
 if($null -eq $number -or $number -is [bool] -or $number -is [string] -or $number -isnot [ValueType]){throw 'USAGE_REPORT_NUMBER_HOLD'}
 $converted=[double]$number
 if(-not [double]::IsFinite($converted)){throw 'USAGE_REPORT_FINITE_HOLD'}
 return $converted
}
function Select-OfficialUsageResult($usage){
 $keys=@('source','observed','ordinary_allowed','windows')
 if(@(Compare-Object (@($usage.PSObject.Properties.Name)|Sort-Object) ($keys|Sort-Object)).Count -or $usage.source -ne 'CODEX_APP_SERVER_OFFICIAL' -or $usage.ordinary_allowed -isnot [bool] -or @($usage.windows).Count -ne 2){throw 'USAGE_REPORT_SCHEMA_HOLD'}
 $observedAt=Report-Number $usage.observed
 if($observedAt -le 0){throw 'USAGE_REPORT_TIME_HOLD'}
 $cleanWindows=@();$minutes=@(300,10080)
 for($windowIndex=0;$windowIndex -lt 2;$windowIndex++){
  $rawWindow=$usage.windows[$windowIndex]
  if(@(Compare-Object (@($rawWindow.PSObject.Properties.Name)|Sort-Object) (@('minutes','used_percent','resets_at')|Sort-Object)).Count){throw 'USAGE_REPORT_WINDOW_KEYS_HOLD'}
  $windowPercent=Report-Number $rawWindow.used_percent;$windowReset=Report-Number $rawWindow.resets_at
  if((Report-Number $rawWindow.minutes) -ne $minutes[$windowIndex] -or $windowPercent -lt 0 -or $windowPercent -gt 100 -or $windowReset -le $observedAt){throw 'USAGE_REPORT_WINDOW_HOLD'}
  $cleanWindows+=,@{minutes=$minutes[$windowIndex];used_percent=$windowPercent;resets_at=$windowReset}
 }
 return @{source='CODEX_APP_SERVER_OFFICIAL';observed=$observedAt;ordinary_allowed=$usage.ordinary_allowed;windows=$cleanWindows}
}
if($Child){
 $watch=[Diagnostics.Stopwatch]::StartNew();$folder=Join-Path $trialBase $RequestId;$out=Join-Path $folder 'Output'
 $r=@{status='HOLD';stage='SERVICE_IDENTITY';request_id=$RequestId;model_calls=0;discord_posts=0;sqlite_operations=0;automatic_retries=0;usage_read_attempts=$null}
 $private=$null
 try{
  if(-not $RequestId -or (Sha $PSCommandPath) -ne $ScriptSha){throw 'USAGE_CHILD_SHA_HOLD'}
  $a=Read-Approval;$id=[Security.Principal.WindowsIdentity]::GetCurrent()
  if($id.User.Value -ne $a.service_sid -or $id.Name -ne 'PAUL-PC\LIMITLESSRelaySvc' -or ([Security.Principal.WindowsPrincipal]::new($id)).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)){throw 'USAGE_SERVICE_IDENTITY_HOLD'}
  $metadata=Join-Path $folder 'BatchReadOnlyMetadata.psm1'
  if((Sha $metadata) -ne $metadataSha){throw 'USAGE_METADATA_SHA_HOLD'}
  Import-Module $metadata -Force;$logon=[LimitlessBatchMetadataV1]::CurrentLogon()
  if($logon[0] -ne 4 -or $logon[1] -ne 0 -or [Environment]::UserInteractive){throw 'USAGE_SESSION0_LOGON4_HOLD'}
  $r.account=$id.Name;$r.sid_matches=$true;$r.session_id=$logon[1];$r.logon_type=$logon[0]
  $homeBefore=$env:CODEX_HOME;$serviceProfile=[Environment]::GetFolderPath([Environment+SpecialFolder]::UserProfile)
  if($serviceProfile -ine 'C:\Users\LIMITLESSRelaySvc' -or $env:USERPROFILE -ine $serviceProfile){throw 'USAGE_PROFILE_HOLD'}
  $serviceCodexHome=Join-Path $serviceProfile '.codex';No-Link $serviceCodexHome
  if($homeBefore -and $homeBefore -ine $serviceCodexHome){throw 'USAGE_HOME_HOLD'}
  $privateRoot=Join-Path $serviceCodexHome 'limitless-usage-trials'
  if(Test-Path -LiteralPath $privateRoot){No-Link $privateRoot}else{$null=New-Item -ItemType Directory -Path $privateRoot}
  $private=Join-Path $privateRoot $RequestId
  if(Test-Path -LiteralPath $private){throw 'USAGE_PRIVATE_REQUEST_ALREADY_EXISTS_HOLD'}
  $null=New-Item -ItemType Directory -Path $private
  $r.private_diagnostics=$private
  New-FileJson (Join-Path $private 'context.json') @{account=$id.Name;sid=$id.User.Value;session_id=0;logon_type=4;request_id=$RequestId;stage='BEFORE_USAGE'}
  $permit=Get-Content -LiteralPath $permitPath -Raw|ConvertFrom-Json
  if(-not $permit.approved -or $permit.request_id -ne $RequestId -or $permit.kind -ne 'USAGE_ONLY_ONCE'){throw 'USAGE_PERMIT_HOLD'}
  $launch=Get-Content -LiteralPath $fuse -Raw|ConvertFrom-Json
  if($launch.request_id -ne $RequestId){throw 'USAGE_LAUNCH_MARKER_HOLD'}
  $tool=Join-Path $folder 'Package/Tools/DiscordAICollab';$entry=Join-Path $folder 'Package/Tools/DiscordAIServiceDiagnostics/usage-only-once.py'
  if($permit.trial_tool_root -ine $tool -or (Sha $entry) -ne $permit.entry_sha256){throw 'USAGE_ENTRY_ROOT_HOLD'}
  $jobSource=Join-Path $folder 'UsageOnlyJob.cs'
  if((Sha $jobSource) -ne $jobSha){throw 'USAGE_JOB_SOURCE_HOLD'}
  Add-Type -Path $jobSource
  New-FileJson (Join-Path $out 'child-run-consumed.json') @{request_id=$RequestId;kind='USAGE_ONLY_NO_RETRY'}
  $diagnostics=Join-Path $serviceCodexHome 'limitless-usage-diagnostics'
  $beforeNames=@();if(Test-Path -LiteralPath $diagnostics){$beforeNames=@(Get-ChildItem -LiteralPath $diagnostics -File -Filter 'usage-*.json'|ForEach-Object Name)}
  $budget=[Math]::Min(17000,[Math]::Floor(18000-$watch.Elapsed.TotalMilliseconds))
  if($budget -le 0){throw 'USAGE_SUPERVISOR_DEADLINE_HOLD'}
  $r.stage='JOB_SUPERVISED_USAGE';$r.supervisor_budget_ms=$budget
  $python=Join-Path $a.tool_root '.venv/Scripts/python.exe'
  # 정지 상태의 Python을 새 Job에 넣은 뒤 시작합니다. CLI는 기존 고정 참조를 사용합니다.
  $j=[LimitlessUsageJobV1]::Run($python,@('-I','-B','-X','utf8',$entry,'--request-id',$RequestId),$tool,[int]$budget)
  $r.job=@{timed_out=$j.TimedOut;exit_code=$j.ExitCode;active_processes=$j.ActiveProcesses;cleanup_verified=$j.CleanupVerified;stdout_truncated=$j.StdoutTruncated;stderr_truncated=$j.StderrTruncated}
  # 원시 stderr는 메모리에서만 처리하고 어휘 제한 요약만 자기 프로필에 저장합니다.
  $entryResult=$null
  try{
   $value=$j.Stdout|ConvertFrom-Json
   if($value.status -notin @('HOLD','USAGE_ONLY_OBSERVED') -or $value.reason -notmatch '^[A-Z_]{1,100}$' -or $value.usage_read_attempts -notin @(0,1) -or $value.model_calls -ne 0 -or $value.discord_posts -ne 0 -or $value.sqlite_operations -ne 0 -or $value.automatic_retries -ne 0 -or $value.mode_changed -ne $false){throw 'USAGE_OUTPUT_SCHEMA_HOLD'}
   $entryResult=@{status=$value.status;reason=$value.reason;usage_read_attempts=$value.usage_read_attempts;model_calls=0;discord_posts=0;sqlite_operations=0;automatic_retries=0;mode_changed=$false}
   if($null -ne $value.official_usage){$entryResult.official_usage=Select-OfficialUsageResult $value.official_usage}
   if($value.status -eq 'USAGE_ONLY_OBSERVED' -and -not $entryResult.official_usage){throw 'USAGE_REPORT_MISSING_HOLD'}
   $r.entry_result=$entryResult;$r.usage_read_attempts=$value.usage_read_attempts
  }catch{$r.output_status='USAGE_OUTPUT_NOT_VERIFIED'}
  $rpcRecords=@()
  if(Test-Path -LiteralPath $diagnostics){
   foreach($f in @(Get-ChildItem -LiteralPath $diagnostics -File -Filter 'usage-*.json'|Where-Object {$_.Name -notin $beforeNames})){
    if($f.Length -gt 24576){continue}
    try{$v=Get-Content -LiteralPath $f.FullName -Raw|ConvertFrom-Json;if($v.rpc_method -in @('initialize','account/read','account/rateLimits/read')){$rpcRecords+=,@{file=$f.Name;record=$v}}}catch{}
   }
  }
  if($rpcRecords.Count -eq 1){
   $v=$rpcRecords[0].record
   # 상세 메시지/stderr는 자기 프로필에만 둡니다. 공용 결과는 method/id/숫자코드만 포함합니다.
   $r.rpc=@{method=$v.rpc_method;request_id=$v.request_id;error_code=$v.error.code;process_exit_code=$v.process_exit_code;status=$v.status}
  }
  New-FileJson (Join-Path $private 'execution.json') @{request_id=$RequestId;entry=$entryResult;job=$r.job;stderr_summary=$j.StderrSummary;rpc_records=$rpcRecords;stage=$r.stage}
  $r.private_diagnostics=$private;$r.codex_home_unchanged=($homeBefore -ceq $env:CODEX_HOME)
  if(-not $j.TimedOut -and $j.CleanupVerified -and $j.ActiveProcesses -eq 0 -and $j.ExitCode -eq 0 -and $entryResult.status -eq 'USAGE_ONLY_OBSERVED' -and $r.codex_home_unchanged){$r.status='USAGE_ONLY_OBSERVED'}
  else{$r.reason='USAGE_HOLD_NO_RETRY'}
 }catch{$r.reason=if($_.Exception.Message -match '^USAGE_[A-Z0-9_]+$'){$_.Exception.Message}else{'USAGE_CHILD_HOLD'};$r.error_chain=Safe-Error $_
  if($private){try{New-FileJson (Join-Path $private 'failure.json') $r}catch{}}
 }
 $r|ConvertTo-Json -Depth 8|Set-Content -LiteralPath (Join-Path $out 'child-summary.json') -Encoding utf8
 if($r.status -eq 'USAGE_ONLY_OBSERVED'){exit 0}else{exit 2}
}
$id=[Security.Principal.WindowsIdentity]::GetCurrent()
if(-not ([Security.Principal.WindowsPrincipal]::new($id)).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)){Write-Output 'HOLD_ADMIN_POWERSHELL_REQUIRED_NO_USAGE';exit 2}
if(Test-Path -LiteralPath $fuse){Write-Output 'HOLD_SINGLE_APPROVAL_ALREADY_LAUNCHED_NO_RETRY';exit 2}
$repo=Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
 $archive=Join-Path (Split-Path $repo -Parent) 'UsageTrialLaunchBindingFix20261026/usage-package.zip'
$draftPath=Join-Path $repo '문서/00_프로젝트/Evidence/UsageTrialBindingFix20261026/usage-only-approval.DRAFT_NOT_AUTHORIZED.json'
$nonce=[guid]::NewGuid().ToString('N');$name='LIMITLESS-UsageOnly-'+$nonce;$marker='USAGE_ONLY_ONE_APPROVED_'+$nonce
$credential=$null;$plain=$null;$registered=$false;$permitCreated=$false;$out=$null;$before=$null;$ledgerBefore=$null;$folder=$null;$permitSha=$null
$r=@{status='HOLD';stage='DEPLOYMENT_PREFLIGHT';request_id=$nonce;model_calls=0;discord_posts=0;automatic_retries=0;usage_read_attempts=$null;usage_launch_attempts=0;parent_is_admin=$true;parent_session_id=(Get-Process -Id $PID).SessionId}
try{
 $a=Read-Approval;$before=Boot-Snapshot;$ledgerBefore=Ledger-Snapshot $a.tool_root
 if((Sha $previousFuse) -ne $previousFuseSha){throw 'USAGE_PREVIOUS_MARKER_CHANGED_HOLD'}
 if((Sha $envResult) -ne $envResultSha){throw 'USAGE_ENVIRONMENT_PROOF_HOLD'}
 $envProof=Get-Content -LiteralPath $envResult -Raw|ConvertFrom-Json
 if($envProof.status -ne 'ENVIRONMENT_PASS_USAGE_NOT_ATTEMPTED' -or $envProof.child.session_id -ne 0 -or $envProof.child.logon_type -ne 4 -or -not $envProof.child.sid_matches -or $envProof.cleanup -ne 'REMOVED' -or -not $envProof.boot_tasks_unchanged -or -not $envProof.ledger_unchanged){throw 'USAGE_ENVIRONMENT_NOT_PASS_HOLD'}
 if(Test-Path -LiteralPath $permitPath){throw 'USAGE_EXISTING_ACTIVE_PERMIT_HOLD'}
 if((Sha $archive) -ne $archiveSha -or (Sha $draftPath) -ne $draftSha -or (Sha (Join-Path $PSScriptRoot 'UsageOnlyJob.cs')) -ne $jobSha){throw 'USAGE_SOURCE_PIN_HOLD'}
 $draft=Get-Content -LiteralPath $draftPath -Raw|ConvertFrom-Json
 $oldNames=@($a.runtime_sha256.PSObject.Properties.Name);$newNames=@($draft.runtime_sha256.PSObject.Properties.Name)
 $changed=@('collab/usage_bridge.py','collab/service_ai.py','collab/service_batch_once.py');$newModule='collab/usage_diagnostics.py'
 if($oldNames.Count -ne 43 -or $newNames.Count -ne 44 -or @($newNames|Where-Object {$_ -notin $oldNames}).Count -ne 1 -or $newModule -notin $newNames -or @($oldNames|Where-Object {$_ -notin $newNames}).Count){throw 'USAGE_MANIFEST_SET_HOLD'}
 foreach($k in $oldNames){if($k -notin $changed -and $draft.runtime_sha256.$k -ne $a.runtime_sha256.$k){throw 'USAGE_UNCHANGED_40_HOLD'}}
 No-Link 'C:/ProgramData/LimitlessRelay'
 if(Test-Path -LiteralPath $trialBase){No-Link $trialBase}else{$null=New-Item -ItemType Directory -Path $trialBase}
 $folder=Join-Path $trialBase $nonce;if(Test-Path -LiteralPath $folder){throw 'USAGE_REQUEST_FOLDER_EXISTS_HOLD'}
 $null=New-Item -ItemType Directory -Path $folder
 $childScript=Join-Path $folder 'run-approved-usage-once.ps1';Copy-Item -LiteralPath $PSCommandPath -Destination $childScript
 Copy-Item -LiteralPath $helper -Destination (Join-Path $folder 'ProtectionHelpers.ps1')
 Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'UsageOnlyJob.cs') -Destination (Join-Path $folder 'UsageOnlyJob.cs')
 $metadata=Join-Path $a.tool_root 'BatchReadOnlyMetadata.psm1'
 if((Sha $metadata) -ne $metadataSha){throw 'USAGE_METADATA_PIN_HOLD'}
 Copy-Item -LiteralPath $metadata -Destination (Join-Path $folder 'BatchReadOnlyMetadata.psm1')
 $copy=Join-Path $folder 'package.zip';Copy-Item -LiteralPath $archive -Destination $copy
 if((Sha $copy) -ne $archiveSha -or (Sha $childScript) -ne (Sha $PSCommandPath)){throw 'USAGE_COPY_HOLD'}
 $allowed=[Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
 foreach($k in $newNames){$null=$allowed.Add('Tools/DiscordAICollab/'+$k)}
 $null=$allowed.Add('Tools/DiscordAICollab/roles.json');$null=$allowed.Add('Tools/DiscordAIServiceDiagnostics/usage-only-once.py')
 $zip=[IO.Compression.ZipFile]::OpenRead($copy)
 try{$seen=[Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal);foreach($e in $zip.Entries){if(-not $allowed.Contains($e.FullName) -or -not $seen.Add($e.FullName)){throw 'USAGE_ARCHIVE_SCOPE_HOLD'}};if(-not $seen.SetEquals($allowed)){throw 'USAGE_ARCHIVE_SET_HOLD'}}finally{$zip.Dispose()}
 $package=Join-Path $folder 'Package';[IO.Compression.ZipFile]::ExtractToDirectory($copy,$package,$false)
 $trialTool=Join-Path $package 'Tools/DiscordAICollab';$entry=Join-Path $package 'Tools/DiscordAIServiceDiagnostics/usage-only-once.py'
 # 이 검사는 기존 QA 재실행이 아니라 새 보호 배포 경로의 무결성 gate입니다. 생략하지 않습니다.
 foreach($p in $draft.runtime_sha256.PSObject.Properties){if((Sha (Join-Path $trialTool $p.Name)) -ne $p.Value){throw 'USAGE_DEPLOYED_SHA_HOLD'}}
 if((Sha $entry) -ne $draft.entry_sha256 -or (Sha (Join-Path $trialTool 'roles.json')) -ne $draft.roles_sha256 -or (Sha $a.service_codex.path) -ne $draft.cli_sha256){throw 'USAGE_DEPLOYED_ENTRY_CLI_HOLD'}
 $r.deployment='PASS_44_EXACT_40_PRESERVED';$r.stage='LOCAL_SECURE_INPUT'
 $out=Join-Path $folder 'Output';$null=New-Item -ItemType Directory -Path $out
 $acl=[Security.AccessControl.DirectorySecurity]::new();$acl.SetAccessRuleProtection($true,$false);$acl.SetOwner($id.User)
 foreach($sid in @($id.User.Value,'S-1-5-18','S-1-5-32-544')){$acl.AddAccessRule([Security.AccessControl.FileSystemAccessRule]::new([Security.Principal.SecurityIdentifier]::new($sid),'FullControl','ContainerInherit,ObjectInherit','None','Allow'))}
 $acl.AddAccessRule([Security.AccessControl.FileSystemAccessRule]::new([Security.Principal.SecurityIdentifier]::new($a.service_sid),'Modify','ContainerInherit,ObjectInherit','None','Allow'));Set-Acl -LiteralPath $out -AclObject $acl
 $credential=Get-Credential -UserName 'PAUL-PC\LIMITLESSRelaySvc' -Message '기존 승인 usage-only 최대1회/20초입니다. 서비스 Windows 암호는 로컬 입력에만 입력하세요. 모델/Discord 호출은 없습니다.'
 if(-not $credential -or $credential.UserName -ine 'PAUL-PC\LIMITLESSRelaySvc'){throw 'USAGE_LOCAL_CREDENTIAL_HOLD'}
 $taskArguments='-NoProfile -NonInteractive -WindowStyle Hidden -File "'+$childScript+'" -Child -RequestId '+$nonce+' -ScriptSha '+(Sha $childScript)
 $action=New-ScheduledTaskAction -Execute (Get-Command pwsh).Source -Argument $taskArguments -WorkingDirectory $folder
 $settings=New-ScheduledTaskSettingsSet -ExecutionTimeLimit (New-TimeSpan -Seconds 20) -MultipleInstances IgnoreNew
 $plain=$credential.GetNetworkCredential().Password
 Register-ScheduledTask -TaskName $name -TaskPath '\' -Description $marker -Action $action -Settings $settings -User $credential.UserName -Password $plain -RunLevel Limited|Out-Null
 $registered=$true;$plain=$null;$t=Get-ScheduledTask -TaskName $name -TaskPath '\'
 if($t.Principal.LogonType.ToString() -ne 'Password' -or $t.Principal.RunLevel.ToString() -ne 'Limited' -or [Xml.XmlConvert]::ToTimeSpan($t.Settings.ExecutionTimeLimit).TotalSeconds -ne 20 -or $t.Settings.RestartCount -ne 0 -or -not $t.Settings.AllowHardTerminate){throw 'USAGE_TASK_POLICY_HOLD'}
 $draft.approved=$true;$draft.request_id=$nonce;$draft.expires_at=[DateTimeOffset]::UtcNow.ToUnixTimeSeconds()+300;$draft.trial_tool_root=$trialTool
 New-FileJson $permitPath $draft;$permitCreated=$true;$permitSha=Sha $permitPath
 $r.stage='START_APPROVED_USAGE_ONCE'
 # 날짜별 전체 승인 마커도 먼저 소비합니다. 재실행해 새 request를 만들어 호출하지 못합니다.
 New-FileJson $fuse @{request_id=$nonce;kind='OWNER_APPROVED_BINDINGFIX_E10D02CB_USAGE_ONCE';previous_request='987a8f4268534a0496a9e600e7af5dd7';created=[DateTimeOffset]::UtcNow.ToUnixTimeSeconds()}
 $r.usage_launch_attempts=1
 Start-ScheduledTask -TaskName $name -TaskPath '\'
 for($i=0;$i -lt 35;$i++){Start-Sleep -Seconds 1;$t=Get-ScheduledTask -TaskName $name -TaskPath '\';$info=Get-ScheduledTaskInfo -TaskName $name -TaskPath '\';if($t.State.ToString() -ne 'Running' -and $info.LastRunTime.Year -gt 2000){break}}
 $r.task_exit_code=$info.LastTaskResult
 $childSummaryPath=Join-Path $out 'child-summary.json'
 if(Test-Path -LiteralPath $childSummaryPath){$c=Get-Content -LiteralPath $childSummaryPath -Raw|ConvertFrom-Json;$r.child=$c;$r.usage_read_attempts=$c.usage_read_attempts;if($c.status -eq 'USAGE_ONLY_OBSERVED' -and $r.task_exit_code -eq 0){$r.status='USAGE_ONLY_OBSERVED'}}
}catch{$r.reason=if($_.Exception.Message -match '^USAGE_[A-Z0-9_]+$'){$_.Exception.Message}else{'USAGE_PARENT_HOLD'};$r.error_chain=Safe-Error $_}
finally{
 $plain=$null;if($credential){$credential.Password.Dispose()};$credential=$null
 try{
  # 먼저 신규 접수를 막습니다. 다른 주체의 permit은 덮어쓰거나 이동하지 않습니다.
  if($permitCreated){if((Sha $permitPath) -ne $permitSha){throw 'USAGE_PERMIT_CHANGED_HOLD'};$used=Get-Content -LiteralPath $permitPath -Raw|ConvertFrom-Json;if($used.request_id -ne $nonce){throw 'USAGE_PERMIT_ID_HOLD'};Move-Item -LiteralPath $permitPath -Destination (Join-Path $folder 'permit.consumed.disabled.json');$r.permit_deactivated=$true}else{$r.permit_deactivated= -not (Test-Path -LiteralPath $permitPath)}
 }catch{$r.status='HOLD';$r.permit_cleanup='NOT_VERIFIED';$r.permit_error=Safe-Error $_}
 try{
  if($registered){$t=Get-ScheduledTask -TaskName $name -TaskPath '\';if($t.Description -ne $marker -or -not $t.Actions[0].Arguments.Contains($nonce)){throw 'USAGE_TASK_ID_HOLD'};if($t.State.ToString() -eq 'Running'){Stop-ScheduledTask -TaskName $name -TaskPath '\';Start-Sleep -Seconds 2;$t=Get-ScheduledTask -TaskName $name -TaskPath '\';$r.status='HOLD';$r.supervisor_task_stopped=$true};if($t.State.ToString() -eq 'Running'){throw 'USAGE_TASK_RUNNING_HOLD'};Unregister-ScheduledTask -TaskName $name -TaskPath '\' -Confirm:$false;$r.cleanup='REMOVED'}else{$r.cleanup='NOT_REGISTERED'}
  $r.approval_unchanged=((Sha $approvalPath) -eq $approvalSha);if(-not $r.approval_unchanged){throw 'USAGE_APPROVAL_CHANGED_HOLD'}
  $r.previous_launch_marker_preserved=((Sha $previousFuse) -eq $previousFuseSha);if(-not $r.previous_launch_marker_preserved){throw 'USAGE_PREVIOUS_MARKER_CHANGED_HOLD'}
  if($before){$after=Boot-Snapshot;$same=$true;foreach($n in $before.Keys){if($before[$n] -ne $after[$n]){$same=$false}};$r.boot_tasks_unchanged=$same;if(-not $same){throw 'USAGE_BOOT_CHANGED_HOLD'}}
  if($ledgerBefore){$r.ledger_unchanged=((Ledger-Snapshot $a.tool_root) -ceq $ledgerBefore);if(-not $r.ledger_unchanged){throw 'USAGE_LEDGER_CHANGED_HOLD'}}
 }catch{$r.status='HOLD';$r.cleanup_or_protection='NOT_VERIFIED';$r.cleanup_error=Safe-Error $_}
 if($out){$r|ConvertTo-Json -Depth 12|Set-Content -LiteralPath (Join-Path $out 'summary.json') -Encoding utf8}
}
$r|ConvertTo-Json -Depth 12
if($r.status -eq 'USAGE_ONLY_OBSERVED'){exit 0}else{exit 2}

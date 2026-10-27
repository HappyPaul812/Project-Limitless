# 순수 설치 순서입니다. 실제 Windows adapter와 가짜 adapter가 같은 함수를 사용합니다.
function Get-V2Hash([byte[]]$Bytes){if($null -eq $Bytes){throw 'V2_NULL_BYTES_HOLD'};[Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($Bytes)).ToLowerInvariant()}
function Assert-V2WorkerState($State){
 if($State.known -ne $true -or $State.foreign_processes -ne 0 -or $State.other_launchable_tasks -ne 0){throw 'V2_SHARED_WORKER_NOT_EXCLUSIVE_HOLD'}
}
function Invoke-V2InstallCore {
 param($Api,[byte[]]$ManifestBytes,[hashtable]$Payloads,[string]$ApprovalRaw,[string]$ZipSha)
 $stage='VALIDATE_PACKAGE';$lease=$null;$before=$null;$consumed=$false
 $result=[ordered]@{schema=2;status='HOLD';stage=$stage;consumed=$false;accounts_created=0;tasks_registered=0;ipc_calls=0;worker_calls=0;automatic_retries=0;v1_reference_status='REFERENCE_VERIFICATION_HOLD'}
 try{
  $raw=[Text.Encoding]::UTF8.GetString($ManifestBytes);[LimitlessV2Contract]::Manifest($raw);$m=$raw|ConvertFrom-Json -AsHashtable
  if($Payloads.Count -ne $m.files_sha256.Count){throw 'V2_EXACT_PAYLOAD_HOLD'}
  foreach($name in $m.files_sha256.Keys){if(-not $Payloads.ContainsKey($name) -or (Get-V2Hash $Payloads[$name]) -cne $m.files_sha256[$name]){throw 'V2_PAYLOAD_SHA_HOLD'}}
  $manifestSha=Get-V2Hash $ManifestBytes;$installerSha=$m.files_sha256['Deployment/install-session0-v2.ps1']
  $stage='READ_BOUNDARY';$context=$Api.Context()
  if(-not $context.admin -or -not $context.worker_sid -or $context.worker_sid -eq $context.admin_sid -or $context.worker_batch_present -ne $true -or $context.worker_groups_users_only -ne $true){throw 'V2_ADMIN_WORKER_CONTEXT_HOLD'}
  [LimitlessV2Contract]::Approval($ApprovalRaw,$manifestSha,$installerSha,$ZipSha,$m.approval_id,$m.epoch,$context.worker_sid,$context.admin_sid,[long]$context.now)
  $stage='PRESERVE_V1_AND_FRESH_V2';$before=$Api.Snapshot()
  if(-not $before.known -or -not $before.old_bot_disabled -or -not $before.old_code_verified -or -not $before.operational_preserved){throw 'V2_PRESERVATION_UNKNOWN_HOLD'}
  $Api.Fresh($m);Assert-V2WorkerState ($Api.WorkerState())
  $stage='CONSUME_V2_APPROVAL';$Api.Consume($m,$manifestSha);$consumed=$true;$result.consumed=$true
  $stage='ACQUIRE_SHARED_WORKER';$lease=$Api.AcquireLease($context.worker_sid);Assert-V2WorkerState ($Api.WorkerState())
  $stage='CREATE_DISABLED_ACCOUNT';$result.account_creation_attempts=1;$result.accounts_created=$null;$created=$Api.CreateAccount($m.bot_account,$m.package_id)
  $result.accounts_created=1
  [LimitlessV2Contract]::Distinct($created.sid,$context.worker_sid,$context.admin_sid)
  if($created.name -cne $m.bot_account -or -not $created.disabled -or -not $created.users_only -or -not $created.fresh){throw 'V2_NEW_ACCOUNT_READBACK_HOLD'}
  $stage='GRANT_NEW_SID_BATCH';$Api.GrantBatch($created.sid,$context.worker_sid,$context.admin_sid)
  $stage='PUBLISH_PROTECTED_CODE';$Api.Publish($m,$ManifestBytes,$Payloads,$created.sid,$context.worker_sid)
  $stage='BIND_OS_SIDS';$binding=[ordered]@{schema=2;kind='LIMITLESS_V2_INSTALLED_BINDING';package_id=$m.package_id;epoch=$m.epoch;approval_id=$m.approval_id;manifest_sha256=$manifestSha;bot_sid=$created.sid;worker_sid=$context.worker_sid;admin_sid=$context.admin_sid;execution_mode='FAKE_ONLY';auto_start=$false}
  $bindingBytes=[Text.UTF8Encoding]::new($false).GetBytes(($binding|ConvertTo-Json -Depth 8 -Compress));$bindingSha=Get-V2Hash $bindingBytes
  $Api.WriteBinding($bindingBytes,$created.sid,$context.worker_sid)
  $stage='REGISTER_CREATE_ONLY_DISABLED_TASK';Assert-V2WorkerState ($Api.WorkerState())
  $sddl=[LimitlessV2Contract]::TaskSddl($context.worker_sid)
  # 응답이 불명확해도 소비를 되돌리거나 UPDATE/재등록하지 않습니다.
  $result.task_registration_attempts=1;$result.tasks_registered=$null;$actual=$Api.RegisterTask($m,$context.worker_sid,$manifestSha,$bindingSha,$sddl,50);$result.tasks_registered=1
  [LimitlessV2Contract]::ExactSddl($actual.sddl,$sddl)
  if($actual.name -cne $m.task_name -or -not $actual.disabled -or $actual.triggers -ne 0 -or $actual.run_level -ne 0 -or $actual.retries -ne 0 -or $actual.principal -cne $context.worker_sid -or -not $actual.action_pinned -or $actual.time_limit_seconds -ne 90 -or $actual.multiple_instances -ne 2 -or $actual.allow_hard_terminate -ne $true){throw 'V2_TASK_READBACK_HOLD'}
  $stage='POST_INSTALL_READBACK';$Api.Verify($m,$binding,$Payloads,$bindingSha)
  $after=$Api.Snapshot();if(-not $after.known -or $before.fingerprint -cne $after.fingerprint){throw 'V2_V1_OR_OPERATIONAL_CHANGED_HOLD'}
  Assert-V2WorkerState ($Api.WorkerState())
  $result.status='V2_INSTALLED_DISABLED_FAKE_ONLY_NO_EXECUTION';$result.binding_sha256=$bindingSha;$result.bot_sid=$created.sid;$result.manifest_sha256=$manifestSha
 }catch{
  $result.status='HOLD';$result.errors=[LimitlessV2Contract]::ErrorCodes($_.Exception)
  if($Api.PSObject.Properties['createdSid'] -and $Api.createdSid){$result.partial_new_sid=$Api.createdSid}
  if($Api.PSObject.Properties['lastWorkerState'] -and $Api.lastWorkerState){$result.worker_query_errors=$Api.lastWorkerState.errors}
 }finally{
  $result.stage=$stage
  if($before){try{$after=$Api.Snapshot();$result.v1_preserved=($after.known -and $before.fingerprint -ceq $after.fingerprint);if(-not $result.v1_preserved){$result.status='HOLD'}}catch{$result.status='HOLD';$result.v1_preserved=$false}}
  if($lease){try{$lease.Dispose()}catch{$result.status='HOLD';$result.lease_release_error=[LimitlessV2Contract]::ErrorCodes($_.Exception)}}
  if($consumed){try{$Api.Record($result)}catch{$result.status='HOLD';$result.record_error=[LimitlessV2Contract]::ErrorCodes($_.Exception)}}
 }
 return $result
}

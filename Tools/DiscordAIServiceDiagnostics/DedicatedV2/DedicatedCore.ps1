# fake/native adapter가 동일한 설치 순서를 사용합니다. 기존 계정·프로세스 수정 경로는 없습니다.
function D3Hash([byte[]]$Bytes){if($null -eq $Bytes){throw 'D3_NULL_BYTES'};[Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($Bytes)).ToLowerInvariant()}
function Assert-D3NewUser($User,$Role,$Context,$Seen){
 if(-not $User.created_new -or -not $User.disabled -or -not $User.users_only -or $User.name -cne $Role -or -not [DedicatedContract]::Sid($User.sid) -or $User.sid -in @($Context.preexisting_sids)+@($Seen)+@($Context.admin_sid,[DedicatedContract]::OldWorker,[DedicatedContract]::OldBot)){throw 'D3_OS_NEW_SID_REQUIRED'}
}
function Invoke-D3Install($Api,[byte[]]$ManifestBytes,[hashtable]$Payloads,[string]$ApprovalRaw,[string]$ZipSha){
 $stage='PACKAGE';$before=$null;$lease=$null;$consumed=$false;$result=@{schema=3;status='HOLD';stage=$stage;created_accounts=@();account_attempts=0;task_attempts=0;automatic_retries=0;ipc_calls=0;model_calls=0;auth_imports=0;v1_reference_status='REFERENCE_VERIFICATION_HOLD'}
 try{
  $raw=[Text.Encoding]::UTF8.GetString($ManifestBytes);[DedicatedContract]::Manifest($raw);$m=$raw|ConvertFrom-Json -AsHashtable
  if($Payloads.Count -ne $m.files_sha256.Count){throw 'D3_EXACT_PAYLOADS'}
  foreach($n in $m.files_sha256.Keys){if(-not $Payloads.ContainsKey($n) -or (D3Hash $Payloads[$n]) -cne $m.files_sha256[$n]){throw 'D3_PAYLOAD_SHA'}}
  $ms=D3Hash $ManifestBytes;$stage='ADMIN_APPROVAL';$context=$Api.Context()
  if(-not $context.admin){throw 'D3_ADMIN_REQUIRED'}
  [DedicatedContract]::InstallApproval($ApprovalRaw,$ms,$m.files_sha256['Deployment/install-session0-v2.ps1'],$ZipSha,$m.approval_id,$m.epoch,$context.admin_sid,[long]$context.now)
  $stage='PRESERVE_EXISTING_FRESH_NAMESPACE';$before=$Api.Snapshot()
  if(-not $before.known -or -not $before.preserved){throw 'D3_PRESERVATION_UNKNOWN'}
  $Api.Fresh($m);$stage='CONSUME_NEW_INSTALL_APPROVAL';$Api.Consume($m,$ms);$consumed=$true
  $stage='INSTALL_LEASE';$lease=$Api.InstallLease($m)
  $stage='CREATE_DISABLED_BOT';$result.account_attempts++;$bot=$Api.CreateUser($m.bot_account,$m.package_id,'bot');Assert-D3NewUser $bot $m.bot_account $context @();$result.created_accounts+=@{role='bot';sid=$bot.sid}
  $stage='CREATE_DISABLED_DEDICATED_WORKER';$result.account_attempts++;$worker=$Api.CreateUser($m.worker_account,$m.package_id,'worker');Assert-D3NewUser $worker $m.worker_account $context @($bot.sid);$result.created_accounts+=@{role='worker';sid=$worker.sid}
  [DedicatedContract]::Distinct($bot.sid,$worker.sid,$context.admin_sid)
  $stage='NEW_SIDS_ONLY_RIGHTS';$Api.Rights($bot.sid,$worker.sid,$context.admin_sid)
  $stage='PUBLISH_CODE_STATE_EMPTY_AUTH';$Api.Publish($m,$ManifestBytes,$Payloads,$bot.sid,$worker.sid)
  $bound=@{schema=3;kind='DEDICATED_V2_OS_BINDING';package_id=$m.package_id;epoch=$m.epoch;approval_id=$m.approval_id;manifest_sha256=$ms;bot_sid=$bot.sid;worker_sid=$worker.sid;admin_sid=$context.admin_sid;execution_mode='FAKE_ONLY';auto_start=$false}
  $binding=[Text.UTF8Encoding]::new($false).GetBytes(($bound|ConvertTo-Json -Compress));$bs=D3Hash $binding
  $Api.Binding($binding,$bot.sid,$worker.sid)
  $stage='ENABLE_ONLY_NEW_BATCH_WORKER';$Api.EnableWorker($worker.sid);$result.worker_install_state='ENABLED_BATCH_ONLY'
  $stage='CREATE_ONLY_DISABLED_TASK';$result.task_attempts=1;$task=$Api.Task($m,$worker.sid,$ms,$bs)
  [DedicatedContract]::ExactSddl($task.sddl,[DedicatedContract]::TaskSddl($worker.sid))
  if(-not $task.disabled -or $task.triggers -ne 0 -or $task.retries -ne 0 -or $task.principal -cne $worker.sid -or $task.run_level -ne 0 -or $task.logon_type -ne 1 -or $task.seconds -ne 90 -or $task.instances -ne 2 -or -not $task.hard_terminate -or -not $task.action_pinned){throw 'D3_TASK_READBACK'}
  $stage='FINAL_READBACK';$Api.Verify($m,$bound,$Payloads,$bs)
  $after=$Api.Snapshot();if(-not $after.known -or $after.fingerprint -cne $before.fingerprint){throw 'D3_EXISTING_CHANGED'}
  $result.status='DEDICATED_INSTALLED_BOT_DISABLED_WORKER_BATCH_ONLY_TASK_DISABLED';$result.binding_sha256=$bs;$result.manifest_sha256=$ms
 }catch{$result.status='HOLD';$result.errors=[DedicatedContract]::ErrorCodes($_.Exception)}
 finally{
  $result.stage=$stage;$result.consumed=$consumed
  if($lease){try{$lease.Dispose()}catch{$result.status='HOLD';$result.lease_error=[DedicatedContract]::ErrorCodes($_.Exception)}}
  if($before){try{$last=$Api.Snapshot();$result.existing_preserved=$last.known -and $last.fingerprint -ceq $before.fingerprint;if(-not $result.existing_preserved){$result.status='HOLD'}}catch{$result.status='HOLD';$result.preservation_error=[DedicatedContract]::ErrorCodes($_.Exception)}}
  if($consumed){try{$Api.Record($result)}catch{$result.status='HOLD';$result.record_error=[DedicatedContract]::ErrorCodes($_.Exception)}}
 }
 $result
}
function Assert-D3RuntimeAdmission($Manifest,$Binding,$Facts){
 [DedicatedContract]::Distinct($Binding.bot_sid,$Binding.worker_sid,$Binding.admin_sid)
 if($Manifest.execution_mode -cne 'FAKE_ONLY' -or $Manifest.real_ai_enabled -ne $false -or $Facts.worker_sid -cne $Binding.worker_sid -or $Facts.security_logon_type -ne 4 -or $Facts.session -ne 0 -or $Facts.admin -or $Facts.elevated -or -not $Facts.users_only -or -not $Facts.rights_exact -or -not $Facts.credential_scope_unshared -or -not $Facts.launcher_control_verified -or -not $Facts.process_generation_verified -or -not $Facts.no_foreign_dedicated_processes -or -not $Facts.no_other_dedicated_tasks -or $Facts.codex_home -cne $Manifest.codex_home -or -not $Facts.auth_empty -or $Facts.other_auth_imported){throw 'D3_RUNTIME_BOUNDARY_OR_EXCLUSIVITY_HOLD'}
 # PIN/대화형 로그인은 batch Session0의 전제조건으로 쓰지 않습니다. 실제 부팅 동작은 별도 검증입니다.
 $true
}

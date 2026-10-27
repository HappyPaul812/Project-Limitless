# r2 전용 단발 순서입니다. 실제 생성은 명시 승인/콘솔/미사용 namespace 검증 뒤에만 시작합니다.
function Invoke-R2InstallOnce($Api,$ManifestBytes,$Payloads,$Pins){
 $r=@{schema=3;kind='R2_INSTALL_ONLY';status='HOLD';phase='READONLY_GATES';approval_creation_attempted=$false;approval_created=$false;install_calls=0;automatic_retries=0;ipc_calls=0;model_calls=0;runtime_status='HOLD_NEW_TOKEN_GROUP_EXCLUSIVITY_NOT_VERIFIED';partial_assets_preserve_no_retry=$true}
 try{
  $facts=$Api.Gates()
  foreach($key in @('admin','console','stage','native_constructor','fresh_namespace','r2_records_absent','old_preserved')){if($facts[$key] -isnot [bool] -or -not $facts[$key]){throw 'R2_INSTALL_GATE_HOLD'}}
  $r.phase='LOCAL_SECURE_INPUT';$Api.Passwords()
  $r.phase='REVALIDATE_BEFORE_APPROVAL';$context=$Api.Revalidate()
  $a=@{schema=3;kind='DEDICATED_INSTALL_ONCE';approved=$true;approval_id=$Pins.id;epoch=$Pins.epoch;manifest_sha256=$Pins.ms;installer_sha256=$Pins.ins;zip_sha256=$Pins.zip;admin_sid=$context.admin_sid;expires_unix=([long]$context.now+900);preserve_existing=$true;create_bot='LIMITLESSBotV2D';create_worker='LIMITLESSWorkV2D';worker_install_state='ENABLED_BATCH_ONLY'}
  $raw=$a|ConvertTo-Json -Compress
  [DedicatedContract]::InstallApproval($raw,$Pins.ms,$Pins.ins,$Pins.zip,$Pins.id,$Pins.epoch,$context.admin_sid,[long]$context.now)
  $r.phase='R2_APPROVAL_CREATE_NEW';$r.approval_creation_attempted=$true;$r.approval_created=$null;$Api.Approve($raw);$r.approval_created=$true
  $r.phase='R2_NATIVE_INSTALL_ONCE';$r.install_calls=1;$r.native_result=$Api.Install($ManifestBytes,$Payloads,$raw,$Pins.zip)
  if($r.native_result.status -cne 'DEDICATED_INSTALLED_BOT_DISABLED_WORKER_BATCH_ONLY_TASK_DISABLED'){throw 'R2_NATIVE_HOLD_NO_RETRY'}
  $r.phase='POSTINSTALL_READONLY_AUDIT';$r.audit=$Api.Audit($r.native_result)
  if($r.audit.verified -isnot [bool] -or -not $r.audit.verified){throw 'R2_POSTINSTALL_AUDIT_HOLD'}
  $r.status='R2_INSTALLED_VERIFIED_DISABLED_NO_START';$r.phase='STOP_NO_IPC'
 }catch{$r.status='HOLD';$r.errors=[DedicatedContract]::ErrorCodes($_.Exception)}
 finally{
  if($r.approval_creation_attempted){try{$Api.Record($r)}catch{$r.status='HOLD';$r.record_errors=[DedicatedContract]::ErrorCodes($_.Exception)}}
  try{$Api.ClearPasswords()}catch{$r.status='HOLD';$r.clear_errors=[DedicatedContract]::ErrorCodes($_.Exception)}
 }
 $r
}

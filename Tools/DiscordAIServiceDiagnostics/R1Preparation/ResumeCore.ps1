# 한 번의 재개도 별도 marker로 고정합니다. fake/native가 같은 승인 보존 순서를 사용합니다.
function Invoke-R1ConsoleResume($Api,$Pins){
 $r=@{schema=3;kind='R1_CONSOLE_RESUME';status='HOLD';phase='READ_ONLY_GATES';reserved=$false;old_preserved=$false;fresh_approval=$false;installer_launches=0;automatic_retries=0;ipc_calls=0;model_calls=0;runtime_status='HOLD_NEW_TOKEN_GROUP_EXCLUSIVITY_NOT_VERIFIED'};$lease=$null
 try{
  $f=$Api.Facts()
  foreach($key in @('admin','console_input','console_output','fresh_namespace','private_approval_verified','stage_verified','unused','expired','same_admin')){if($f[$key] -isnot [bool] -or -not $f[$key]){throw 'R1_RESUME_GATE_HOLD'}}
  if($f.old_sha -cne $Pins.old_sha){throw 'R1_OLD_APPROVAL_PIN_HOLD'}
  $old=$f.old;if($old.expires_unix -gt $f.now){throw 'R1_EXPECTED_EXPIRED_ONLY'}
  # 만료된 파일을 유효한 승인으로 사용하지 않습니다. 대표의 재개 요청에 따른 새 승인 바이트입니다.
  $fresh=$old.Clone();$fresh.expires_unix=[long]$f.now+900;$raw=$fresh|ConvertTo-Json -Compress
  [DedicatedContract]::InstallApproval($raw,$Pins.ms,$Pins.ins,$Pins.zip,$Pins.id,$Pins.epoch,$f.admin_sid,[long]$f.now)
  $r.phase='RESERVE_RESUME_ONCE';$lease=$Api.Reserve();$r.reserved=$true
  $r.phase='PRESERVE_EXPIRED_APPROVAL';$Api.Preserve();$r.old_preserved=$true
  $r.phase='PUBLISH_NEW_15MIN_APPROVAL';$Api.Publish([Text.UTF8Encoding]::new($false).GetBytes($raw));$r.fresh_approval=$true
  $r.phase='CONSOLE_INHERITED_INSTALLER_ONCE';$r.installer_launches=1;$code=$Api.Launch()
  if($code -ne 0){throw 'R1_CHILD_EXIT_HOLD'}
  $Api.Recheck();$r.status='CHILD_RETURNED_REVIEW_INSTALLER_JSON_NOT_SUCCESS_PROOF';$r.phase='AWAIT_NATIVE_RESULT_REVIEW'
 }catch{$r.status='HOLD';$r.errors=[DedicatedContract]::ErrorCodes($_.Exception)}
 finally{if($r.reserved){try{$Api.Record($r)}catch{$r.status='HOLD';$r.record_errors=[DedicatedContract]::ErrorCodes($_.Exception)}};if($lease){try{$lease.Dispose()}catch{$r.status='HOLD';$r.close_errors=[DedicatedContract]::ErrorCodes($_.Exception)}}}
 $r
}

function Invoke-R2TaskSecurityReadback($Backend,$Evidence){
 $result=@{schema=1;kind='R2_TASK_SECURITY_TARGETED_READONLY';status='HOLD';stage='PREPARE';changes=0;install_calls=0;ipc_calls=0;model_calls=0;automatic_retries=0;effective_worker_access='UNKNOWN_TOKEN_NOT_CHECKED';runtime_admitted=$false;descriptor_readback_status='UNKNOWN';read_attempts=0}
 try{
  $before=$Backend.Prepare();if($before.verified -isnot [bool] -or -not $before.verified){throw 'READBACK_SOURCE_HOLD'}
  $result.stage='TASK_COM_CONNECT';$Backend.Connect()
  $result.stage='TASK_GET_AND_SD';$result.read_attempts=1;$first=$Backend.Read()
  foreach($key in @('path_match','principal_sid_match','disabled','trigger0','state_disabled','policy_settings_match','action_match','xml_baseline_match')){if($first[$key] -isnot [bool] -or -not $first[$key]){throw ('READBACK_'+$key.ToUpperInvariant()+'_HOLD')}}
  $result.stage='TASK_DESCRIPTOR_POLICY_COMPARE'
  $result.candidate_verdict=[Limitless.TaskSecurityCandidate.DedicatedTaskReadContract]::VerifyOffline($first.sddl,$Evidence.ObjectKind,[Limitless.TaskSecurityCandidate.DedicatedTaskReadContract]::TaskPath,[Limitless.TaskSecurityCandidate.DedicatedTaskReadContract]::Worker,$Evidence)
  $result.stage='READONLY_END_RECHECK';$result.read_attempts=2;$last=$Backend.Read();$after=$Backend.Prepare()
  if($before.fingerprint -cne $after.fingerprint -or $after.verified -isnot [bool] -or -not $after.verified -or $first.fingerprint -cne $last.fingerprint){throw 'READBACK_CHANGED_DURING_QUERY_HOLD'}
  $result.task=$first;$result.source_checks=$before;$result.descriptor_readback_status='PASS';$result.status='TASK_DESCRIPTOR_READONLY_PASS_EFFECTIVE_ACCESS_HOLD';$result.stage='STOP_NO_IPC'
 }catch{
  $result.status='HOLD';$result.errors=@();for($e=$_.Exception;$null -ne $e;$e=$e.InnerException){$r=@{type=$e.GetType().FullName;hresult=$e.HResult};if($e -is [ComponentModel.Win32Exception]){$r.win32=$e.NativeErrorCode};$result.errors+=$r}
 }
 $result
}

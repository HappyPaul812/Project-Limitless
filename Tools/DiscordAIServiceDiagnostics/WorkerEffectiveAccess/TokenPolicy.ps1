function Test-R2ExistingTokenPolicy($Facts){
 $r=@{status='HOLD';accesscheck_status='UNKNOWN';session0_batch_status='UNKNOWN';job_exclusivity='UNKNOWN';old_worker_conflict='UNKNOWN';future_worker_execution='UNVERIFIED';runtime_admitted=$false}
 if($Facts.token_accesscheck_known -isnot [bool] -or -not $Facts.token_accesscheck_known -or $Facts.process_token_stable -isnot [bool] -or -not $Facts.process_token_stable -or $Facts.token_elevated -isnot [bool] -or $Facts.token_elevated -or $Facts.token_user_sid -cne 'S-1-5-21-2858504392-1958168942-1662906075-1010'){return $r}
 $read=@($Facts.checks|Where-Object requested -EQ 0x120089);$max=@($Facts.checks|Where-Object requested -EQ 0x2000000)
 if($read.Count -ne 1 -or $max.Count -ne 1 -or -not $read[0].allowed -or -not $max[0].allowed -or $max[0].granted -ne 0x120089){return $r}
 foreach($mask in @(0x20,2,4,0x10,0x100,0x10000,0x40000,0x80000)){$check=@($Facts.checks|Where-Object requested -EQ $mask);if($check.Count -ne 1 -or $check[0].allowed){return $r}}
 if(@($Facts.checks|Where-Object {$_.privileges_used -ne 0}).Count){return $r}
 $r.accesscheck_status='EXISTING_PROCESS_TOKEN_DACL_MODEL_PASS'
 if($Facts.logon_query_status -ceq 'READ_OK' -and $Facts.security_logon_type -eq 4 -and $Facts.session_id -eq 0){$r.session0_batch_status='OBSERVED_EXISTING_TOKEN_ONLY'}
 $r.status='TOKEN_DACL_CHECK_COMPLETE_RUNTIME_EXCLUSIVITY_HOLD';return $r
}

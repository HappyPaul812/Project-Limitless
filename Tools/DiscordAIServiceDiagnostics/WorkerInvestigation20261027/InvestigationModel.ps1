# 이 모델은 관측 역할과 충돌 위험을 분리하며 r11의 foreign_processes=0 정책을 변경하지 않습니다.
function Get-InvestigationError($Exception,[string]$Stage){
 $codes=@();$e=$Exception;$n=0
 while($e -and $n++ -lt 8){$c=@{type=$e.GetType().FullName;hresult=$e.HResult};if($e -is [ComponentModel.Win32Exception]){$c.win32=$e.NativeErrorCode};if($e.PSObject.Properties['Operation']){$c.operation=$e.Operation};$codes+=$c;$e=$e.InnerException}
 @{stage=$Stage;codes=$codes}
}
function Get-TaskLogonLabel([int]$Value){switch($Value){0{'NONE'}1{'PASSWORD'}2{'S4U'}3{'INTERACTIVE_TOKEN'}4{'GROUP'}5{'SERVICE_ACCOUNT'}6{'INTERACTIVE_TOKEN_OR_PASSWORD'}default{'UNKNOWN'}}}
function Get-SecurityLogonLabel([int]$Value){switch($Value){2{'INTERACTIVE'}3{'NETWORK'}4{'BATCH'}5{'SERVICE'}7{'UNLOCK'}8{'NETWORK_CLEARTEXT'}9{'NEW_CREDENTIALS'}10{'REMOTE_INTERACTIVE'}11{'CACHED_INTERACTIVE'}12{'CACHED_REMOTE_INTERACTIVE'}13{'CACHED_UNLOCK'}default{'UNKNOWN'}}}
function Select-InvestigationFields($Object,[string[]]$Names){$safe=@{};foreach($key in $Names){if($null -ne $Object.$key){$safe[$key]=$Object.$key}};$safe}
function Get-InvestigationCreatedUtc($Value){
 if($Value -is [datetime]){return $Value.ToUniversalTime().ToString('o')}
 if($Value -is [datetimeoffset]){return $Value.UtcDateTime.ToString('o')}
 if($Value -isnot [string]){throw 'CREATION_TYPE_UNKNOWN'}
 [datetimeoffset]::Parse($Value,[Globalization.CultureInfo]::InvariantCulture).UtcDateTime.ToString('o')
}
function Get-InvestigationRole($Process,[string]$WorkerSid){
 $r=@{role='UNKNOWN';ai_activity='NOT_VERIFIED';safe_to_exclude=$false;risk='UNKNOWN'}
 if($Process.identity -ne 'MATCH'){$r.role='BASELINE_IDENTITY_'+$Process.identity;return $r}
 if($Process.exited_during_query){$r.role='EXITED_DURING_QUERY';return $r}
 if(-not $Process.owner_sid){$r.role='OWNER_UNKNOWN';return $r}
 if($Process.owner_sid -cne $WorkerSid){$r.role='OTHER_ACCOUNT_ASSOCIATION';return $r}
 if(-not $Process.image){$r.role='IMAGE_UNKNOWN';return $r}
 $name=([string]$Process.image.name).ToLowerInvariant()
 if($name -match '^codex(?:-[a-z0-9_-]+)?\.exe$'){$r.role='CODEX_NAMED_IMAGE_INDICATOR';$r.risk='POTENTIAL_AI_OR_CLI_COLLISION';return $r}
 if($name -in @('node.exe','python.exe','pythonw.exe','py.exe','pwsh.exe','powershell.exe','cmd.exe','dotnet.exe','bun.exe','uv.exe')){$r.role='GENERAL_RUNTIME_OR_LAUNCHER_UNKNOWN';return $r}
 if($name -in @('conhost.exe','fontdrvhost.exe','dwm.exe','sihost.exe','csrss.exe','winlogon.exe','userinit.exe','taskhostw.exe','runtimebroker.exe','explorer.exe')){
  if($Process.image.location -ceq 'WINDOWS' -and $Process.image.signature_status -ceq 'VALID_AT_CACHE_POLICY'){$r.role='SIGNED_WINDOWS_RESIDENT_CANDIDATE'}else{$r.role='WINDOWS_NAME_ONLY_NOT_VERIFIED'}
 }
 $r
}
function Resolve-InvestigationPrincipal($Principal,[scriptblock]$Resolver,$Processes,[string]$WorkerSid){
 $u=$Principal.UserId;$g=$Principal.GroupId;$type=[int]$Principal.LogonType
 $r=@{task_scheduler_logon_type=$type;task_scheduler_logon_label=(Get-TaskLogonLabel $type);status='UNKNOWN';execution_confirmed=$false;future_token_membership='UNKNOWN';worker_activation='UNKNOWN'}
 try{
  foreach($v in @($u,$g)){if($null -ne $v -and ($v -isnot [string] -or $v.Length -gt 512)){throw 'PRINCIPAL_TYPE_OR_BOUND'}}
  $hasUser=-not [string]::IsNullOrWhiteSpace($u);$hasGroup=-not [string]::IsNullOrWhiteSpace($g)
  if($type -eq 4){
   if($hasUser -or -not $hasGroup){throw 'GROUP_PRINCIPAL_SHAPE'}
   $r.group_id=$g;$resolved=& $Resolver $g
   if(-not $resolved.resolved -or $resolved.sid_name_use -notin @(2,4,5) -or $resolved.sid -cnotmatch '^S-1-'){throw 'GROUP_OS_RESOLUTION_UNKNOWN'}
   $r.group_sid=$resolved.sid;$r.group_category=$resolved.category;$r.group_name=$resolved.name;$r.group_domain=$resolved.domain
   $tokens=@($Processes|Where-Object{$_.identity -ceq 'MATCH' -and -not $_.exited_during_query -and $_.owner_sid -ceq $WorkerSid -and $_.token_query_known})
   $matches=@();$deny=@();$restricted=$false
   foreach($p in $tokens){
    foreach($entry in @($p.token_groups|Where-Object sid -CEQ $resolved.sid)){
     if($null -eq $entry.attributes -or [long]$entry.attributes -lt 0 -or [long]$entry.attributes -gt [uint32]::MaxValue){throw 'TOKEN_GROUP_ATTRIBUTES_UNKNOWN'}
     $attributes=[uint32]$entry.attributes
     if(($attributes -band 4) -ne 0 -and ($attributes -band 16) -eq 0){$matches+=@{pid=$p.pid;created_utc=$p.actual_created_utc;authentication_luid=$p.authentication_luid;security_logon_type=$p.security_logon_type;session_id=$p.session_id};if(@($p.restricted_sids).Count){$restricted=$true}}
     elseif(($attributes -band 16) -ne 0){$deny+=$p.pid}
    }
   }
   $r.enabled_token_evidence=$matches;$r.deny_only_pids=$deny
   if($restricted){$r.membership='ENABLED_GROUP_RESTRICTED_TOKEN_CONTEXT_UNKNOWN'}
   elseif($matches.Count){$r.membership='ENABLED_IN_OBSERVED_WORKER_PRIMARY_TOKEN';$r.worker_activation='POTENTIAL_GROUP_ACTIVATION_SESSION_CONDITIONS_NOT_PROVEN'}
   elseif($deny.Count){$r.membership='DENY_ONLY_IN_OBSERVED_TOKEN'}
   elseif($tokens.Count){$r.membership='NOT_PRESENT_IN_OBSERVED_TOKENS_NOT_FUTURE_EXCLUSION'}
   else{$r.membership='UNKNOWN_NO_VERIFIED_WORKER_TOKEN'}
   $r.status='OS_RESOLVED_TOKEN_SNAPSHOT_ONLY'
  }else{
   if(-not $hasUser -or $hasGroup){throw 'USER_PRINCIPAL_SHAPE'}
   $resolved=& $Resolver $u;if(-not $resolved.resolved -or $resolved.sid -cnotmatch '^S-1-'){throw 'USER_OS_RESOLUTION_UNKNOWN'}
   $r.user_sid=$resolved.sid;$r.worker_configured_user=($resolved.sid -ceq $WorkerSid);$r.status='OS_RESOLVED_USER'
   if($r.worker_configured_user){$r.worker_activation='POTENTIAL_CONFIGURED_WORKER_ACCOUNT'}
  }
 }catch{$r.errors=Get-InvestigationError $_.Exception 'PRINCIPAL_OS_RESOLUTION_OR_TOKEN_MEMBERSHIP'}
 $r
}
function Read-InvestigationProcess($Expected,[string]$WorkerSid,$Provider,[string]$Scope='BASELINE'){
 $r=@{pid=[int]$Expected.pid;expected_created_utc=$Expected.created_utc;scope=$Scope;identity='UNKNOWN';errors=@();cim_consistent=$false}
 try{
  # PS7 JSON parser가 DateTime으로 변환했어도 기본 문자열 변환으로 소수초를 잃지 않습니다.
  $expectedUtc=Get-InvestigationCreatedUtc $Expected.created_utc;$r.expected_created_utc=$expectedUtc
  $native=& $Provider.Process $r.pid $expectedUtc
  $safe=Select-InvestigationFields $native @('pid','expected_created_utc','creation_comparison_precision','actual_created_utc','identity','exited_during_query','owner_sid','authentication_luid','authentication_id_decimal','session_id','token_query_known','in_any_job','job_identity','token_close_win32','process_close_win32')
  foreach($key in $safe.Keys){$r[$key]=$safe[$key]}
  if($native.owner_account){$r.owner_account=Select-InvestigationFields $native.owner_account @('sid','name','domain','sid_name_use','category','resolved')}
  foreach($key in @('token_groups','restricted_sids')){if($null -ne $native.$key){$r[$key]=@($native.$key|ForEach-Object{Select-InvestigationFields $_ @('sid','attributes','enabled','deny_only','logon_sid')})}}
  foreach($key in @('owner_lookup_error','token_error','image_error','job_error')){if($native.$key){$r[$key]=Select-InvestigationFields $native.$key @('type','hresult','win32','operation')}}
  if($r.identity -ceq 'MATCH' -and -not $r.exited_during_query){
   try{
    $cim=@(& $Provider.Cim $r.pid)
    if($cim.Count -ne 1 -or -not [WorkerReadOnlyNative]::CreationMatches($r.actual_created_utc,([datetime]$cim[0].CreationDate).ToUniversalTime().ToString('o'))){throw 'CIM_GENERATION_RACE'}
    $r.parent_pid=[int]$cim[0].ParentProcessId;$r.cim_session_id=[int]$cim[0].SessionId;$r.cim_consistent=($null -eq $r.session_id -or $r.session_id -eq $r.cim_session_id)
   }catch{$r.errors+=Get-InvestigationError $_.Exception 'SCOPED_CIM_PARENT_GENERATION'}
   if($native.image_query_path -and ($Scope -ceq 'BASELINE' -or $r.owner_sid -ceq $WorkerSid)){
    try{$image=& $Provider.Image $native.image_query_path;$r.image=Select-InvestigationFields $image @('name','name_sha256','location','sha256','file_id','image_kind','signature_policy','signature_code','signature_status','catalog_enumeration_performed')}catch{$r.errors+=Get-InvestigationError $_.Exception 'IMAGE_HANDLE_SHA_SIGNATURE'}
   }
   if($r.authentication_id_decimal -match '^\d+$'){
    try{$session=@(& $Provider.Logon $r.authentication_id_decimal);if($session.Count -ne 1 -or [string]$session[0].LogonId -cne $r.authentication_id_decimal){throw 'LOGON_LUID_UNKNOWN'};$r.security_logon_type=[int]$session[0].LogonType;$r.security_logon_label=Get-SecurityLogonLabel $r.security_logon_type}
    catch{$r.errors+=Get-InvestigationError $_.Exception 'SECURITY_LOGON_LUID_ONLY'}
   }
  }
 }catch{
  $failure=Get-InvestigationError $_.Exception 'PROCESS_LIMITED_INFORMATION';$r.errors+=$failure
  $denied=@($failure.codes|Where-Object win32 -eq 5).Count -gt 0;$notFound=@($failure.codes|Where-Object win32 -In @(87,1168)).Count -gt 0
  if($denied){$r.identity='ACCESS_DENIED'}elseif($notFound){try{if(@(& $Provider.Cim $r.pid).Count -eq 0){$r.identity='PROCESS_EXITED_AT_SNAPSHOT'}}catch{$r.errors+=Get-InvestigationError $_.Exception 'PROCESS_ABSENCE_CHECK'}}
 }
 $r.assessment=Get-InvestigationRole $r $WorkerSid;$r
}
function Invoke-WorkerInvestigation($Baseline,[string]$WorkerSid,$Provider){
 $result=@{schema=1;status='HOLD';baseline_process_count=@($Baseline).Count;targets=@();children=@();parents=@();tasks=@();errors=@();r11_foreign_zero_guard_unchanged=$true;actual_ai_request_verified=$false;global_exclusive_execution_verified=$false;lease_consume_contents_read=$false}
 foreach($expected in $Baseline){$result.targets+=Read-InvestigationProcess $expected $WorkerSid $Provider}
 $parents=@{};$targetIds=@($Baseline|ForEach-Object{[int]$_.pid})
 foreach($p in $result.targets|Where-Object{$_.identity -ceq 'MATCH' -and $_.cim_consistent -and $_.parent_pid -gt 0}){
  $inSet=@($result.targets|Where-Object pid -eq $p.parent_pid)
  if($inSet.Count -eq 1 -and $inSet[0].identity -ceq 'MATCH'){$parent=@{pid=$p.parent_pid;created_utc=$inSet[0].actual_created_utc;source='VERIFIED_TARGET_SET'}}
  elseif(-not $parents.ContainsKey($p.parent_pid)){
   try{$parents[$p.parent_pid]=& $Provider.Parent $p.parent_pid}catch{$parents[$p.parent_pid]=@{pid=$p.parent_pid;errors=(Get-InvestigationError $_.Exception 'ADJACENT_PARENT_LIMITED_QUERY')}}
   $parent=$parents[$p.parent_pid]
  }else{$parent=$parents[$p.parent_pid]}
  $edge=@{child_pid=$p.pid;parent_pid=$p.parent_pid;relation='UNKNOWN'}
  if($parent.created_utc){$edge.relation=if([datetimeoffset]::Parse($parent.created_utc) -gt [datetimeoffset]::Parse($p.actual_created_utc)){'PARENT_PID_REUSED'}else{'GENERATION_ORDER_COMPATIBLE_NOT_LAUNCH_PROOF'}}
  $result.parents+=$edge
 }
 $seenChildren=@{};$childBound=64
 foreach($p in $result.targets|Where-Object{$_.identity -ceq 'MATCH' -and $_.cim_consistent -and -not $_.exited_during_query}){
  if($childBound -le 0){$result.errors+=@{stage='DIRECT_CHILD_SCOPE';reason='CHILD_BOUND_HOLD'};break}
  try{foreach($child in @(& $Provider.Children $p.pid)){
   if($childBound-- -le 0){throw 'CHILD_QUERY_BOUND_HOLD'}
   if([int]$child.ParentProcessId -ne $p.pid -or $null -eq $child.CreationDate -or ([datetime]$child.CreationDate).ToUniversalTime() -lt [datetime]::Parse($p.actual_created_utc).ToUniversalTime()){throw 'CHILD_PARENT_GENERATION_UNKNOWN'}
   $childPid=[int]$child.ProcessId;if($childPid -in $targetIds -or $seenChildren.ContainsKey($childPid)){continue};$seenChildren[$childPid]=$true
   $result.children+=Read-InvestigationProcess @{pid=$childPid;created_utc=([datetime]$child.CreationDate).ToUniversalTime().ToString('o')} $WorkerSid $Provider 'DIRECT_CHILD'
  }}catch{$result.errors+=Get-InvestigationError $_.Exception 'DIRECT_CHILD_SCOPE'}
 }
 $all=@($result.targets)+@($result.children)
 try{foreach($task in @(& $Provider.Tasks)){
  $principal=Resolve-InvestigationPrincipal $task.Principal $Provider.Resolve $all $WorkerSid
  $row=@{task_id_hash=$task.task_id_hash;known_task_label=$task.known_task_label;enabled=$task.enabled;state=$task.state;principal=$principal}
  $relevant=($task.Principal.LogonType -eq 4 -or $principal.worker_configured_user -or $task.known_task_label -or $principal.status -eq 'UNKNOWN')
  if($relevant){
   try{$detail=& $Provider.TaskDetail $task;$row.action_images=$detail.action_images;$row.running_instances=$detail.running_instances;$row.engine_pid_relation='ENGINE_PID_IS_NOT_ACTION_PID_OR_JOB_PROOF'}catch{$row.errors=Get-InvestigationError $_.Exception 'RELEVANT_TASK_READONLY_DETAIL'}
   $result.tasks+=$row
  }
 }}catch{$result.errors+=Get-InvestigationError $_.Exception 'TASK_PRINCIPAL_METADATA_ONLY'}
 try{$result.lease_metadata=& $Provider.Leases}catch{$result.errors+=Get-InvestigationError $_.Exception 'FIXED_LEASE_METADATA_ONLY'}
 $result.ai_or_cli_image_indicators=@($all|Where-Object{$_.assessment.risk -ceq 'POTENTIAL_AI_OR_CLI_COLLISION'}|ForEach-Object{$_.pid})
 $result.collision_assessment=if($result.ai_or_cli_image_indicators.Count){'POTENTIAL_AI_OR_CLI_COLLISION_NOT_MODEL_ACTIVITY_PROOF'}else{'UNKNOWN_NO_EXCLUSIVITY_PROOF_FROM_SNAPSHOT'}
 $result.policy_note='Role labels never exclude processes, rewrite the original count, or authorize staging/install/IPC.'
 $result
}

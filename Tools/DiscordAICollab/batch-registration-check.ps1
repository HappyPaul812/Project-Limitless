param([ValidateSet('Diagnose')][string]$Action='Diagnose')
# 1번 작업 전용. 안전한 CIM 등록은 1회만 시도하고 인증 오류면 반복/권한 변경 없이 중단합니다.
$ErrorActionPreference='Stop';$root=$PSScriptRoot
if(-not ([Security.Principal.WindowsPrincipal]::new([Security.Principal.WindowsIdentity]::GetCurrent())).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)){Write-Output 'HOLD_ADMIN_LOCAL_EXECUTION_REQUIRED';exit 2}
$approvalPath='C:\ProgramData\LimitlessRelay\batch-environment-check.json'
$approval=Get-Content -LiteralPath $approvalPath -Raw | ConvertFrom-Json
if($approval.kind -ne 'SAFE_BATCH_ENVIRONMENT_ONLY' -or $approval.tool_root -ne $root){throw 'BATCH_SCOPE_MISMATCH'}
foreach($entry in $approval.scripts.PSObject.Properties){if((Get-FileHash -LiteralPath (Join-Path $root $entry.Name) -Algorithm SHA256).Hash.ToLower() -ne $entry.Value){throw 'BATCH_SCRIPT_SHA_MISMATCH'}}
Import-Module (Join-Path $root 'BatchReadOnlyMetadata.psm1') -Force
$out=Join-Path $root 'data/batch-check';New-Item -ItemType Directory -Force -Path $out | Out-Null
$nonce=[guid]::NewGuid().ToString('N');$name='LIMITLESS-BatchCheck-'+$nonce
$account=$env:COMPUTERNAME+'\LIMITLESSRelaySvc';$marker='LIMITLESS_SAFE_BATCH_'+$nonce
$script:plainPassword=$null;$registered=$false;$result=@{status='HOLD';action=$Action;request_id=$nonce;task_name=$name;actual_ai_calls=0;codex_calls=0;mode_changed=$false;original_tasks_changed=$false;stage='PRECHECK'}
function Get-BootSnapshot {
 $values=@{}
 foreach($taskName in @('LIMITLESS-Bot-OnBoot','LIMITLESS-Art-OnBoot')){
  $task=Get-ScheduledTask -TaskPath '\' -TaskName $taskName
  $xml=Export-ScheduledTask -TaskPath '\' -TaskName $taskName
  $bytes=[Text.Encoding]::Unicode.GetBytes($xml)
  $hash=[Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($bytes)).ToLower()
  $values[$taskName]=@{xml_sha256=$hash;user=$task.Principal.UserId;logon=$task.Principal.LogonType.ToString();run_level=$task.Principal.RunLevel.ToString();execute=$task.Actions[0].Execute;arguments=$task.Actions[0].Arguments;enabled=$task.Settings.Enabled}
 }
 return $values
}
function Sanitize([object]$Value){
 $text=[string]$Value
 if($script:plainPassword){
  foreach($secret in @($script:plainPassword,[Security.SecurityElement]::Escape($script:plainPassword),[uri]::EscapeDataString($script:plainPassword))){if($secret){$text=$text.Replace($secret,'[REDACTED]')}}
 }
 if($text.Length -gt 3000){$text=$text.Substring(0,3000)}
 return $text
}
function Read-Error([Management.Automation.ErrorRecord]$Record){
 $chain=@();$errorObject=$Record.Exception
 for($depth=0;$errorObject -and $depth -lt 5;$depth++){
  $item=@{type=$errorObject.GetType().FullName;hresult=$errorObject.HResult;message=(Sanitize $errorObject.Message)}
  if($errorObject.PSObject.Properties['NativeErrorCode']){$item.native_error_code=[string]$errorObject.NativeErrorCode}
  if($errorObject -is [Microsoft.Management.Infrastructure.CimException] -and $errorObject.ErrorData){
   $fields=@{}
   foreach($property in $errorObject.ErrorData.CimInstanceProperties){
    if($property.Name -in @('Message','CIMStatusCode','CIMStatusCodeDescription','ErrorSource','ErrorType','MessageID','ProbableCause','ProbableCauseDescription','RecommendedActions')){$fields[$property.Name]=Sanitize $property.Value}
   }
   $item.cim_error_data=$fields
  }
  $chain+=,$item;$errorObject=$errorObject.InnerException
 }
 return @{error_id=(Sanitize $Record.FullyQualifiedErrorId);category=$Record.CategoryInfo.Category.ToString();chain=$chain}
}
try{
 $result.before=Get-BootSnapshot
 $user=Get-LocalUser -Name LIMITLESSRelaySvc
 if(-not $user.Enabled -or $user.SID.Value -ne $approval.service_sid){throw 'SERVICE_ACCOUNT_ENABLED_SID_REQUIRED'}
 $adminMembers=@(Get-LocalGroupMember -SID 'S-1-5-32-544')
 if(@($adminMembers | Where-Object {$_.SID.Value -eq $user.SID.Value}).Count){throw 'STANDARD_SERVICE_ACCOUNT_REQUIRED'}
 $rights=@([LimitlessBatchMetadataV1]::Rights($user.SID.Value))
 $implicitDeny=@()
 foreach($sid in @('S-1-1-0','S-1-5-11','S-1-5-32-545')){if('SeDenyBatchLogonRight' -in @([LimitlessBatchMetadataV1]::Rights($sid))){$implicitDeny+=$sid}}
 $result.service=@{name=$account;sid=$user.SID.Value;enabled=$user.Enabled;is_admin=$false;batch_right_direct=('SeBatchLogonRight' -in $rights);deny_batch_direct=('SeDenyBatchLogonRight' -in $rights);implicit_deny_sids=$implicitDeny}
 $result.batch_rights_before=$rights
 if('SeBatchLogonRight' -notin $rights -or 'SeDenyBatchLogonRight' -in $rights -or $implicitDeny.Count){throw 'BATCH_LOGON_POLICY_HOLD_NO_POLICY_CHANGE'}
 $pwsh=(Get-Command pwsh).Source
 $arguments='-NoProfile -NonInteractive -WindowStyle Hidden -File "'+(Join-Path $root 'batch-account-check.ps1')+'" -RequestId '+$nonce
 $result.execution=@{execute=$pwsh;arguments=$arguments;working_directory=$root}
 if(Get-ScheduledTask -TaskPath '\' -TaskName $name -ErrorAction SilentlyContinue){throw 'UNEXPECTED_TEST_TASK_EXISTS'}
 $credential=Get-Credential -UserName $account -Message '1번 배치 환경 검증 전용입니다. 서비스 계정 Windows 암호를 로컬 입력창에만 입력하세요.'
 if(-not $credential -or $credential.UserName -ine $account){throw 'SERVICE_CREDENTIAL_REQUIRED'}
 $script:plainPassword=$credential.GetNetworkCredential().Password
 $actionSpec=New-ScheduledTaskAction -Execute $pwsh -Argument $arguments -WorkingDirectory $root
 $settings=New-ScheduledTaskSettingsSet -ExecutionTimeLimit (New-TimeSpan -Minutes 2) -MultipleInstances IgnoreNew -DisallowHardTerminate
 $result.stage='REGISTER_SAFE_BATCH'
 Register-ScheduledTask -TaskPath '\' -TaskName $name -Description $marker -Action $actionSpec -Settings $settings -User $account -Password $script:plainPassword -RunLevel Limited | Out-Null
 $registered=$true
 $task=Get-ScheduledTask -TaskPath '\' -TaskName $name
 $result.registered=@{logon=$task.Principal.LogonType.ToString();run_level=$task.Principal.RunLevel.ToString();user=$task.Principal.UserId}
 # 비대화형 Password가 아니면 실행하지 않습니다. 등록 기본값의 문제도 여기서 분리합니다.
 if($task.Principal.LogonType.ToString() -ne 'Password' -or $task.Principal.RunLevel.ToString() -ne 'Limited'){throw 'UNEXPECTED_REGISTERED_LOGON_POLICY'}
 $result.stage='RUN_SAFE_BATCH'
 Start-ScheduledTask -TaskPath '\' -TaskName $name
 $resultPath=Join-Path $out ('account-'+$nonce+'.json')
 for($attempt=0;$attempt -lt 11;$attempt++){
  Start-Sleep -Seconds 5
  $state=(Get-ScheduledTask -TaskPath '\' -TaskName $name).State.ToString()
  if((Test-Path -LiteralPath $resultPath) -and $state -ne 'Running'){break}
 }
 if(-not (Test-Path -LiteralPath $resultPath)){throw 'SAFE_BATCH_RESULT_NOT_VERIFIED'}
 $child=Get-Content -LiteralPath $resultPath -Raw | ConvertFrom-Json
 $info=Get-ScheduledTaskInfo -TaskPath '\' -TaskName $name
 $result.child=$child;$result.task_exit_code=$info.LastTaskResult
 if($child.request_id -ne $nonce -or $child.status -ne 'PASS' -or $info.LastTaskResult -ne 0){throw 'SAFE_BATCH_EXECUTION_FAILED'}
 $result.status='PASS';$result.stage='SAFE_BATCH_VERIFIED'
} catch {
 $result.status='HOLD';$result.error=Read-Error $_
} finally {
 # 이 실행에서 생성한 nonce/설명/안전 명령이 모두 맞는 임시 작업만 정리합니다.
 try{
  $test=Get-ScheduledTask -TaskPath '\' -TaskName $name -ErrorAction SilentlyContinue
  if($test){
   if($test.Description -ne $marker -or -not $test.Actions[0].Arguments.Contains($nonce) -or -not $test.Actions[0].Arguments.Contains('batch-account-check.ps1')){throw 'TEST_CLEANUP_IDENTITY_MISMATCH'}
   if($test.State.ToString() -eq 'Running'){$result.cleanup='RUNNING_NOT_FORCIBLY_STOPPED';$result.status='HOLD'}
   else {Unregister-ScheduledTask -TaskPath '\' -TaskName $name -Confirm:$false;$result.cleanup='REMOVED'}
  } else {$result.cleanup='NO_TEST_TASK_REGISTERED'}
 } catch {$result.cleanup='NOT_VERIFIED';$result.status='HOLD'}
 try{
  $result.after=Get-BootSnapshot
  $same=$true
  foreach($taskName in @('LIMITLESS-Bot-OnBoot','LIMITLESS-Art-OnBoot')){if($result.before[$taskName].xml_sha256 -ne $result.after[$taskName].xml_sha256){$same=$false}}
  $result.original_tasks_changed=-not $same
  $result.batch_rights_after=@([LimitlessBatchMetadataV1]::Rights($approval.service_sid))
  $result.rights_unchanged=(($result.batch_rights_before | Sort-Object | ConvertTo-Json -Compress) -eq ($result.batch_rights_after | Sort-Object | ConvertTo-Json -Compress))
  if(-not $same -or -not $result.rights_unchanged){$result.status='HOLD'}
 } catch {$result.protection_check='NOT_VERIFIED';$result.status='HOLD'}
 $script:plainPassword=$null;$credential=$null
}
$destination=Join-Path $out ('run-'+$nonce+'.json')
[IO.File]::WriteAllText($destination,($result | ConvertTo-Json -Depth 12),[Text.UTF8Encoding]::new($false))
$result | ConvertTo-Json -Depth 12

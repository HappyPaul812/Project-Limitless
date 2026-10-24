param([switch]$Apply)
# 대표가 승인한 네 SHA의 내용 등록만 합니다. 실행·작업 등록·모드 변경은 하지 않습니다.
$ErrorActionPreference='Stop'
$toolRoot=Join-Path (Split-Path $PSScriptRoot) 'DiscordAICollab'
$proposal=Get-Content (Join-Path $PSScriptRoot 'boot-diagnostics-registration-proposal.json') -Raw|ConvertFrom-Json
$approvalPath='C:\ProgramData\LimitlessRelay\boot-approval.json'
$registryPath='C:\ProgramData\LimitlessRelay\user-worker-integrity.json'
$result=@{status='HOLD';stage='VALIDATE';actual_ai_calls=0;execution_permission=$false;auto_start=$false;boot_tasks_changed=$false}
$stream=$null;$oldBytes=$null;$changed=$false
function Digest([byte[]]$Bytes){[Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($Bytes)).ToLower()}
try{
 $entries=@($proposal.conditional_integrity_registration)
 if($entries.Count -ne 4){throw 'EXACT_FOUR_REVIEWED_FILES_REQUIRED'}
 foreach($e in $entries){
  $expected=Join-Path $toolRoot $e.relative_path
  if([IO.Path]::GetFullPath($expected) -ine [IO.Path]::GetFullPath($e.absolute_path)){throw 'REVIEWED_PATH_MISMATCH'}
  $bytes=[IO.File]::ReadAllBytes($e.absolute_path)
  # 소스를 실행/import하지 않고 실제 바이트와 검토 SHA를 대조합니다.
  if((Digest $bytes) -ne $e.sha256 -or $bytes.Length -ne $e.bytes){throw 'REVIEWED_CONTENT_CHANGED_HOLD'}
 }
 $oldBytes=[IO.File]::ReadAllBytes($approvalPath)
 if((Digest $oldBytes) -ne $proposal.approval_file_sha256_before){throw 'APPROVAL_BASE_CHANGED_HOLD'}
 $old=[Text.Encoding]::UTF8.GetString($oldBytes)|ConvertFrom-Json
 $oldRuntime=@{};foreach($p in $old.runtime_sha256.PSObject.Properties){$oldRuntime[$p.Name]=$p.Value}
 $matched=0
 foreach($f in Get-ChildItem (Join-Path $toolRoot 'collab') -Filter '*.py' -File){
  $key='collab/'+$f.Name
  if($oldRuntime.ContainsKey($key)){
   if((Get-FileHash -LiteralPath $f.FullName).Hash.ToLower() -ne $oldRuntime[$key]){throw 'ORIGINAL_RUNTIME_CHANGED_HOLD'}
   $matched++
  }elseif($key -notin @($entries.relative_path)){throw 'UNREVIEWED_RUNTIME_FILE_HOLD'}
 }
 if($matched -ne 38){throw 'ORIGINAL_38_REQUIRED'}
 $result.files_verified=4;$result.original_38_preserved=$true;$result.approval_before_sha256=Digest $oldBytes
 if(-not $Apply){$result.status='VALIDATED_NOT_APPLIED';$result|ConvertTo-Json;exit 0}
 $identity=[Security.Principal.WindowsIdentity]::GetCurrent()
 $beforeAcl=Get-Acl $approvalPath
 $beforeSecurity=[Convert]::ToBase64String($beforeAcl.GetSecurityDescriptorBinaryForm())
 if(Test-Path -LiteralPath $registryPath){throw 'EXISTING_WORKER_REGISTRY_REVIEW_REQUIRED'}
 # 별도 허용 목록은 실행 허가가 아닌 내용 고정 목록입니다. 기존 부팅 검사에는 호환 SHA만 추가합니다.
 $registry=@{version=1;kind='USER_WORKER_INTEGRITY_ONLY';execution_permission=$false;auto_start=$false;requires_interactive_session=$true;files=@($entries|Select-Object absolute_path,relative_path,sha256);approved_scope='2026-10-24 대표 SHA 등록 및 오프라인 연결 승인'}
 $module=Join-Path $toolRoot 'ServiceProfileDacl.psm1'
 if((Get-FileHash $module).Hash.ToLower() -ne $old.service_auth_scripts.'ServiceProfileDacl.psm1'){throw 'DACL_HELPER_PIN_CHANGED'}
 Import-Module $module -Force
 [IO.File]::WriteAllText($registryPath,($registry|ConvertTo-Json -Depth 8),[Text.UTF8Encoding]::new($false))
 if([LimitlessProfileDaclV1]::SetProtectedDacl($registryPath,$identity.User.Value) -ne 0){throw 'NEW_REGISTRY_PROTECTION_FAILED'}
 $newAcl=Get-Acl $registryPath
 if(-not $newAcl.AreAccessRulesProtected -or $newAcl.GetOwner([Security.Principal.SecurityIdentifier]).Value -ne $identity.User.Value){throw 'NEW_REGISTRY_ACL_NOT_VERIFIED'}
 $result.worker_registry=$registryPath
 $result.stage='REGISTER_BOOT_COMPATIBILITY_HASHES'
 $stream=[IO.File]::Open($approvalPath,[IO.FileMode]::Open,[IO.FileAccess]::ReadWrite,[IO.FileShare]::None)
 $read=[byte[]]::new([int]$stream.Length);$offset=0
 while($offset -lt $read.Length){$count=$stream.Read($read,$offset,$read.Length-$offset);if($count -eq 0){throw 'APPROVAL_READ_INCOMPLETE'};$offset+=$count}
 if((Digest $read) -ne (Digest $oldBytes)){throw 'APPROVAL_CHANGED_DURING_REGISTRATION'}
 foreach($e in $entries){if((Get-FileHash $e.absolute_path).Hash.ToLower() -ne $e.sha256){throw 'SOURCE_CHANGED_DURING_REGISTRATION'}}
 foreach($e in $entries){$old.runtime_sha256|Add-Member -NotePropertyName $e.relative_path -NotePropertyValue $e.sha256}
 foreach($key in $oldRuntime.Keys){if($old.runtime_sha256.PSObject.Properties[$key].Value -ne $oldRuntime[$key]){throw 'ORIGINAL_APPROVAL_VALUE_CHANGED'}}
 $backup=Join-Path (Split-Path $approvalPath) ('boot-approval.before-diagnostics-'+[guid]::NewGuid().ToString('N')+'.json')
 [IO.File]::WriteAllBytes($backup,$oldBytes)
 if([LimitlessProfileDaclV1]::SetProtectedDacl($backup,$identity.User.Value) -ne 0){throw 'BACKUP_PROTECTION_FAILED'}
 $newBytes=[Text.UTF8Encoding]::new($false).GetBytes(($old|ConvertTo-Json -Depth 50))
 $changed=$true;$stream.Position=0;$stream.SetLength(0);$stream.Write($newBytes,0,$newBytes.Length);$stream.Flush($true)
 $stream.Dispose();$stream=$null
 if([Convert]::ToBase64String((Get-Acl $approvalPath).GetSecurityDescriptorBinaryForm()) -ne $beforeSecurity){throw 'ORIGINAL_APPROVAL_SECURITY_CHANGED'}
 $after=Get-Content $approvalPath -Raw|ConvertFrom-Json
 foreach($e in $entries){if($after.runtime_sha256.PSObject.Properties[$e.relative_path].Value -ne $e.sha256){throw 'NEW_HASH_POSTCHECK_FAILED'}}
 $result.status='REGISTERED_INTEGRITY_ONLY';$result.stage='COMPLETE';$result.approval_after_sha256=(Get-FileHash $approvalPath).Hash.ToLower();$result.backup=$backup;$result.approval_acl_owner_preserved=$true
}catch{
 if($stream){$stream.Dispose();$stream=$null}
 if($changed -and $oldBytes){[IO.File]::WriteAllBytes($approvalPath,$oldBytes);$result.rollback='ORIGINAL_BYTES_RESTORED'}
 $message=$_.Exception.Message
 $result.reason=if($message -match '^[A-Z0-9_]+$'){$message}else{'REGISTRATION_ACCESS_OR_IO_HOLD'}
 $result.exception_type=$_.Exception.GetType().Name
}finally{
 if($stream){$stream.Dispose()}
 $output=Join-Path $toolRoot 'data/user-worker-registration.json'
 [IO.File]::WriteAllText($output,($result|ConvertTo-Json -Depth 8),[Text.UTF8Encoding]::new($false))
 $result|ConvertTo-Json -Depth 8
}

param([switch]$SelfTest)
# 대표 승인: 운영 CLI 참조 두 값만 전환하고 서비스 bootstrap 실패 시 복원합니다. 추론은 없습니다.
$ErrorActionPreference='Stop'
$repo=Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
$tool=Join-Path $repo 'Tools/DiscordAICollab'
$approvalPath='C:/ProgramData/LimitlessRelay/boot-approval.json'
$oldPath=Join-Path $tool 'data/runtime/codex/c6144bc19b2705f2/codex.exe'
$oldSha='c6144bc19b2705f25a40d8488b3d57d17a223c5df3c5f65e3c58d72601956c1e'
$newPath=Join-Path $tool 'data/runtime/codex/official-0.164.0-alpha.11.1-x64/bin/codex.exe'
$newSha='4fd2f057bad603bf53dc202457ffd0c3591de62e867a8b01e73816e3fbce5a73'
$verify=Join-Path $PSScriptRoot 'verify-isolated-batch.ps1'
$evidence=Join-Path $repo '문서/00_프로젝트/Evidence/ServiceCLI_OfficialPackage_20261024.json'
function Sha([string]$p){(Get-FileHash -LiteralPath $p -Algorithm SHA256).Hash.ToLower()}
function Canon($obj){$obj|ConvertTo-Json -Depth 100 -Compress}
function New-ReferenceDocument([string]$json,[string]$path,[string]$hash){
 $obj=$json|ConvertFrom-Json
 $obj.service_codex.path=$path;$obj.service_codex.sha256=$hash
 return ($obj|ConvertTo-Json -Depth 100)
}
function Write-Atomic([string]$path,[byte[]]$bytes){
 $acl=Get-Acl -LiteralPath $path
 $tmp=$path+'.limitless-'+[guid]::NewGuid().ToString('N')+'.tmp'
 try {
  [IO.File]::WriteAllBytes($tmp,$bytes);Set-Acl -LiteralPath $tmp -AclObject $acl
  [IO.File]::Replace($tmp,$path,[System.Management.Automation.Language.NullString]::Value)
 } finally {if(Test-Path -LiteralPath $tmp){Remove-Item -LiteralPath $tmp -Force}}
}
function Assert-ServiceRx([string]$path){
 $rules=(Get-Acl -LiteralPath $path).Access
 $service=@($rules|Where-Object{$_.IdentityReference.Translate([Security.Principal.SecurityIdentifier]).Value -eq 'S-1-5-21-2858504392-1958168942-1662906075-1007' -and $_.AccessControlType -eq 'Allow'})
 if(@($service|Where-Object{($_.FileSystemRights -band [Security.AccessControl.FileSystemRights]::ReadAndExecute) -eq [Security.AccessControl.FileSystemRights]::ReadAndExecute}).Count -eq 0){throw 'SERVICE_RX_ACE_HOLD'}
 if(@($service|Where-Object{($_.FileSystemRights -band [Security.AccessControl.FileSystemRights]::Write) -ne 0}).Count){throw 'SERVICE_WRITE_NOT_EXPECTED'}
}
function Verify-Package {
 if((Sha $evidence) -ne '04cecc241628c7a17a46ef9fc3befbe32e287792f938ecc4f854f20a40eb9833'){throw 'MANIFEST_PIN_HOLD'}
 $manifest=Get-Content -LiteralPath $evidence -Raw|ConvertFrom-Json
 $package=Split-Path (Split-Path $newPath -Parent) -Parent
 if($manifest.files.Count -ne 45 -or @(Get-ChildItem -LiteralPath $package -File -Recurse).Count -ne 45){throw 'FILE_COUNT_HOLD'}
 foreach($f in $manifest.files){$p=Join-Path $package $f.path;if((Sha $p) -ne $f.sha256){throw 'PACKAGE_SHA_HOLD'};Assert-ServiceRx $p}
 foreach($rel in @('bin/codex.exe','bin/codex-code-mode-host.exe','codex-resources/codex-command-runner.exe','codex-resources/codex-windows-sandbox-setup.exe')){
  $p=Join-Path $package $rel;$s=Get-AuthenticodeSignature -LiteralPath $p
  if($s.Status -ne 'Valid' -or $s.SignerCertificate.Subject -notmatch 'OpenAI OpCo'){throw 'SIGNATURE_HOLD'}
 }
}
if($SelfTest){
 # 운영 설정/자격 증명/예약 작업을 사용하지 않는 실제 파일 교체·복원 시험입니다.
 $dir=Join-Path $tool ('data/service-reference-selftest/'+[guid]::NewGuid().ToString('N'))
 New-Item -ItemType Directory -Path $dir -Force|Out-Null
 $p=Join-Path $dir 'synthetic.json';$json='{"service_codex":{"path":"old","sha256":"oldsha","version":"same"},"runtime_sha256":{"keep":"same"},"mode":"MANUAL"}'
 $bytes=[Text.UTF8Encoding]::new($false).GetBytes($json);[IO.File]::WriteAllBytes($p,$bytes);$testAcl=(Get-Acl -LiteralPath $p).Sddl
 $changed=New-ReferenceDocument $json 'new' 'newsha';$obj=$changed|ConvertFrom-Json
 if($obj.runtime_sha256.keep -ne 'same' -or $obj.mode -ne 'MANUAL' -or $obj.service_codex.version -ne 'same'){throw 'SELFTEST_UNRELATED_FIELDS'}
 try {
  Write-Atomic $p ([Text.UTF8Encoding]::new($false).GetBytes($changed))
  if((Get-Content $p -Raw|ConvertFrom-Json).service_codex.path -ne 'new'){throw 'SELFTEST_WRITE'}
  throw 'SIMULATED_POSTCHECK_FAILURE'
 } catch {
  if($_.Exception.Message -ne 'SIMULATED_POSTCHECK_FAILURE'){throw}
  Write-Atomic $p $bytes
 }
 if([Convert]::ToBase64String([IO.File]::ReadAllBytes($p)) -ne [Convert]::ToBase64String($bytes)){throw 'SELFTEST_ROLLBACK'}
 if((Get-Acl -LiteralPath $p).Sddl -ne $testAcl){throw 'SELFTEST_ACL'}
 @{status='PASS';tests=4;mode='SYNTHETIC_ONLY';actual_ai_calls=0;production_changed=$false}|ConvertTo-Json -Compress
 exit 0
}
$identity=[Security.Principal.WindowsIdentity]::GetCurrent()
if(-not ([Security.Principal.WindowsPrincipal]::new($identity)).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)){
 Write-Output 'HOLD_ADMIN_LOCAL_CREDENTIAL_REQUIRED_REFERENCE_UNCHANGED';exit 2
}
$nonce=[guid]::NewGuid().ToString('N');$switched=$false;$backup=$null;$afterSha=$null
$result=@{status='HOLD';request_id=$nonce;actual_ai_calls=0;reference_switched=$false;automatic_retries=0}
try {
 Verify-Package
 if((Sha $verify) -ne '1d81fb9e5cd70bd8d3cbb5b79cb6d347a38be62881f89f5f452e2008ca864a9c'){throw 'BATCH_CHECK_SCRIPT_PIN_HOLD'}
 $beforeBytes=[IO.File]::ReadAllBytes($approvalPath);$beforeSha=Sha $approvalPath;$beforeAcl=(Get-Acl -LiteralPath $approvalPath).Sddl
 $beforeText=[Text.Encoding]::UTF8.GetString($beforeBytes).TrimStart([char]0xfeff);$before=$beforeText|ConvertFrom-Json
 if($beforeSha -ne '0cd2ce55f7e23b44190e7f4786c2e9f68e2e291b1888e5768ea606f391c8d2d6' -or $before.service_codex.path -ine $oldPath -or $before.service_codex.sha256 -ne $oldSha){throw 'BEFORE_REFERENCE_CHANGED_HOLD'}
 if($before.service_codex.version -ne '0.164.0-alpha.11.1' -or (Sha $oldPath) -ne $oldSha){throw 'OLD_CLI_HOLD'}
 # 기존 42개 파일 승인은 읽기만 합니다.
 $pythonFiles=@(Get-ChildItem -LiteralPath (Join-Path $tool 'collab') -Filter '*.py' -File)
 if($pythonFiles.Count -ne 42){throw 'EXISTING_42_FILES_REQUIRED'}
 foreach($p in $pythonFiles){
  $key='collab/'+$p.Name
  if($before.runtime_sha256.$key -ne (Sha $p.FullName)){throw 'EXISTING_PYTHON_SHA_HOLD'}
 }
 $backup=Join-Path (Split-Path $approvalPath -Parent) ('before-cli-reference-'+$nonce+'.json')
 [IO.File]::WriteAllBytes($backup,$beforeBytes);Set-Acl -LiteralPath $backup -AclObject (Get-Acl -LiteralPath $approvalPath)
 if((Sha $backup) -ne $beforeSha){throw 'BACKUP_SHA_HOLD'}
 $result.backup_path=$backup;$result.before_sha256=$beforeSha
 $result.before_reference=@{path=$oldPath;sha256=$oldSha};$result.after_reference=@{path=$newPath;sha256=$newSha}
 $expected=New-ReferenceDocument $beforeText $newPath $newSha
 if((Sha $approvalPath) -ne $beforeSha){throw 'CONCURRENT_CHANGE_HOLD'}
 $afterBytes=[Text.UTF8Encoding]::new($false).GetBytes($expected)
 $afterSha=[Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($afterBytes)).ToLower()
 $switched=$true;Write-Atomic $approvalPath $afterBytes
 if((Sha $approvalPath) -ne $afterSha){throw 'POSTWRITE_SHA_HOLD'}
 if((Canon (Get-Content -LiteralPath $approvalPath -Raw|ConvertFrom-Json)) -cne (Canon ($expected|ConvertFrom-Json))){throw 'ONLY_REFERENCE_CHANGE_HOLD'}
 if((Get-Acl -LiteralPath $approvalPath).Sddl -ne $beforeAcl){throw 'APPROVAL_ACL_HOLD'}
 $out=Join-Path $tool 'data/service-isolated-batch-check'
 $oldRuns=@(Get-ChildItem -LiteralPath $out -Filter 'run-*.json' -File -ErrorAction SilentlyContinue|ForEach-Object Name)
 # 별도 pwsh에서 기존 검증기를 실행해 로컬 Get-Credential 입력과 exit를 격리합니다.
 & (Get-Command pwsh).Source -NoProfile -File $verify
 $batchExit=$LASTEXITCODE
 $newRuns=@(Get-ChildItem -LiteralPath $out -Filter 'run-*.json' -File|Where-Object{$_.Name -notin $oldRuns})
 if($batchExit -ne 0 -or $newRuns.Count -ne 1){throw 'SERVICE_BOOTSTRAP_HOLD'}
 $check=Get-Content -LiteralPath $newRuns[0].FullName -Raw|ConvertFrom-Json
 if($check.status -ne 'PASS' -or $check.task_exit_code -ne 0 -or $check.cleanup -ne 'REMOVED' -or -not $check.boot_tasks_unchanged -or -not $check.approval_unchanged){throw 'BATCH_PROTECTION_HOLD'}
 if($check.child.account -ne 'PAUL-PC\LIMITLESSRelaySvc' -or $check.child.session_id -ne 0 -or $check.child.logon_type -ne 4 -or $check.child.cli_path -ine $newPath -or $check.child.cli_sha256 -ne $newSha -or $check.child.version_exit -ne 0 -or $check.child.help_exit -ne 0 -or -not $check.child.codex_home_unchanged){throw 'SERVICE_RESULT_HOLD'}
 Verify-Package
 if((Sha $approvalPath) -ne $afterSha){throw 'POSTCHECK_CONCURRENT_CHANGE_HOLD'}
 $result.service_check=$check;$result.approval_after_sha256=$afterSha;$result.status='PASS';$result.reference_switched=$true;$result.rollback='NOT_NEEDED_BACKUP_VERIFIED'
} catch {
 $result.error='TRANSITION_OR_POSTCHECK_FAILED_NO_RAW_ERROR_LOGGED'
 if($switched){
  try {
   # 동시 변경이 있으면 다른 설정을 덮어쓰지 않고 HOLD합니다.
   $currentSha=Sha $approvalPath
   if($currentSha -ne $afterSha -and $currentSha -ne $beforeSha){throw 'CONCURRENT_CHANGE_NO_CLOBBER'}
   if($currentSha -ne $beforeSha){Write-Atomic $approvalPath ([IO.File]::ReadAllBytes($backup))}
   if((Sha $approvalPath) -ne $beforeSha -or (Get-Acl -LiteralPath $approvalPath).Sddl -ne $beforeAcl){throw 'RESTORE_NOT_VERIFIED'}
   $result.rollback='RESTORED_EXACT_BYTES_AND_ACL'
  } catch {$result.rollback='NOT_VERIFIED_ADMIN_REVIEW_REQUIRED'}
 } else {$result.rollback='NO_REFERENCE_CHANGE'}
}
$destination=Join-Path $tool 'data/service-reference-switch';New-Item -ItemType Directory -Path $destination -Force|Out-Null
$result|ConvertTo-Json -Depth 10|Set-Content -LiteralPath (Join-Path $destination ('switch-'+$nonce+'.json')) -Encoding utf8
$result|ConvertTo-Json -Depth 10
if($result.status -eq 'PASS'){exit 0}else{exit 2}

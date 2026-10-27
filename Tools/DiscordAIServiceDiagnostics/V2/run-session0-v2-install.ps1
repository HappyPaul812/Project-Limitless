param([ValidateSet('Inspect','Install')][string]$Action='Inspect',[Parameter(Mandatory)][string]$Bundle,[string]$ApprovalPath,[switch]$ConfirmSystemChanges)
$ErrorActionPreference='Stop'
$stage='PACKAGE_READ';$result=@{schema=2;status='HOLD';system_changes=0;automatic_retries=0}
try{
 $root=[IO.Path]::GetFullPath($Bundle);$zip=$root+'.zip'
 $manifestBytes=[IO.File]::ReadAllBytes((Join-Path $root 'HostManifest.json'));$raw=[Text.Encoding]::UTF8.GetString($manifestBytes);$m=$raw|ConvertFrom-Json -AsHashtable
 $doc=[System.Text.Json.JsonDocument]::Parse($raw)
 try{
  $keys=@($doc.RootElement.EnumerateObject()|ForEach-Object Name)
  $expected=@('schema','kind','package_id','epoch','approval_id','approved','auto_start','execution_mode','max_seconds','bot_account','code_root','state_root','task_name','pipe_name','boot_approval_sha256','files_sha256')
  if(($keys.Count -ne $expected.Count) -or (($keys|Sort-Object -CaseSensitive) -join ',') -cne (($expected|Sort-Object -CaseSensitive) -join ',') -or $doc.RootElement.GetProperty('schema').GetInt32() -ne 2 -or $doc.RootElement.GetProperty('max_seconds').GetInt32() -ne 90 -or $doc.RootElement.GetProperty('approved').ValueKind -ne [System.Text.Json.JsonValueKind]::False -or $doc.RootElement.GetProperty('auto_start').ValueKind -ne [System.Text.Json.JsonValueKind]::False){throw 'V2_WRAPPER_SCHEMA_HOLD'}
  $names=@($doc.RootElement.GetProperty('files_sha256').EnumerateObject()|ForEach-Object Name)
  if($names.Count -ne @($names|ForEach-Object{$_.ToLowerInvariant()}|Sort-Object -Unique).Count){throw 'V2_WRAPPER_DUPLICATE_PATH_HOLD'}
 }finally{$doc.Dispose()}
 function Wrapper-V2Hash([byte[]]$Bytes){[Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($Bytes)).ToLowerInvariant()}
 $ms=Wrapper-V2Hash $manifestBytes;$zipBytes=[IO.File]::ReadAllBytes($zip);$zipSha=Wrapper-V2Hash $zipBytes
 $installerBytes=[IO.File]::ReadAllBytes((Join-Path $root 'Deployment/install-session0-v2.ps1'));$ins=Wrapper-V2Hash $installerBytes
 if($Action -eq 'Install'){
  $stage='EXPLICIT_APPROVAL_BEFORE_COMPILATION'
  if(-not $ConfirmSystemChanges -or $m.approval_id -cnotmatch '^v2-install-[a-f0-9]{32}$'){throw 'V2_INSTALL_NOT_APPROVED'}
  $expected='C:\ProgramData\LimitlessRelayV2Approvals\'+$m.approval_id+'.json'
  if([IO.Path]::GetFullPath($ApprovalPath) -cne $expected){throw 'V2_APPROVAL_PATH_HOLD'}
  $acl=Get-Acl -LiteralPath $expected;$sd=[Security.AccessControl.RawSecurityDescriptor]::new($acl.Sddl)
  if($sd.Owner.Value -cne 'S-1-5-32-544' -or $sd.Group.Value -cne 'S-1-5-18' -or -not ($sd.ControlFlags -band [Security.AccessControl.ControlFlags]::DiscretionaryAclProtected) -or $sd.DiscretionaryAcl.Count -ne 2){throw 'V2_APPROVAL_ACL_HOLD'}
  foreach($ace in $sd.DiscretionaryAcl){if($ace -isnot [Security.AccessControl.CommonAce] -or $ace.AceQualifier -ne [Security.AccessControl.AceQualifier]::AccessAllowed -or $ace.AceFlags -ne 0 -or $ace.AccessMask -ne 2032127 -or $ace.SecurityIdentifier.Value -notin @('S-1-5-18','S-1-5-32-544')){throw 'V2_APPROVAL_ACE_HOLD'}}
  $a=Get-Content -Raw -LiteralPath $expected|ConvertFrom-Json -AsHashtable
  if($a.approved -ne $true -or $a.kind -cne 'INSTALL_V2' -or $a.manifest_sha256 -cne $ms -or $a.installer_sha256 -cne $ins -or $a.zip_sha256 -cne $zipSha){throw 'V2_APPROVED_BYTES_HOLD'}
 }
 $stage='VERIFY_ALL_PACKAGE_BYTES'
 $verifiedPayloads=@{}
 if($m.files_sha256['Deployment/install-session0-v2.ps1'] -cne $ins){throw 'V2_INSTALLER_SHA_HOLD'}
 $payloadFiles=@(Get-ChildItem -LiteralPath $root -Recurse -File -Force)
 if($payloadFiles.Count -ne $m.files_sha256.Count+1){throw 'V2_EXACT_FILESET_HOLD'}
 foreach($n in $m.files_sha256.Keys){
  if($n -cnotmatch '^(Code|Runtime|Deployment)/' -or $n.Contains('..') -or $n.Contains(':') -or $n.Contains('\')){throw 'V2_WRAPPER_PATH_HOLD'}
  $p=[IO.Path]::GetFullPath((Join-Path $root $n));if(-not $p.StartsWith($root+'\',[StringComparison]::OrdinalIgnoreCase)){throw 'V2_WRAPPER_ROOT_HOLD'}
  $item=Get-Item -Force -LiteralPath $p;$bytes=[IO.File]::ReadAllBytes($p);if($item.Attributes -band [IO.FileAttributes]::ReparsePoint -or (Wrapper-V2Hash $bytes) -cne $m.files_sha256[$n]){throw 'V2_WRAPPER_SHA_HOLD'};$verifiedPayloads[$n]=$bytes
 }
 if($m.schema -ne 2 -or $m.kind -cne 'LIMITLESS_SESSION0_V2_CANDIDATE' -or $m.package_id -cnotmatch '^[a-f0-9]{32}$' -or $m.epoch -cne ('20261027-v2-'+$m.package_id) -or $m.approval_id -cne ('v2-install-'+$m.package_id) -or $m.pipe_name -cne ('LimitlessPlanningV2-'+$m.package_id) -or $m.approved -ne $false -or $m.auto_start -ne $false -or $m.execution_mode -cne 'FAKE_ONLY' -or $m.max_seconds -ne 90 -or $m.code_root -cne 'C:\Program Files\LimitlessRelayTrialV2_20261027' -or $m.state_root -cne 'C:\ProgramData\LimitlessRelayTrialV2_20261027' -or $m.bot_account -cne 'LIMITLESSBotTrialV2' -or $m.task_name -cne 'LIMITLESS-Trial-V2-20261027-S0Host'){throw 'V2_WRAPPER_NAMESPACE_HOLD'}
 # Inspect에서는 패키지 C#도 실행하지 않습니다. Install은 보호 승인 SHA 확인 뒤 설치기가 한 assembly로 컴파일합니다.
 # ZIP entry 집합과 bytes를 payload와 대조합니다. 기존 패키지를 설치/실행하지 않습니다.
 $zipStream=[IO.MemoryStream]::new($zipBytes,$false);$archive=[IO.Compression.ZipArchive]::new($zipStream,[IO.Compression.ZipArchiveMode]::Read)
 try{
  if($archive.Entries.Count -ne $payloadFiles.Count){throw 'V2_ZIP_COUNT_HOLD'};$seen=@{}
  foreach($entry in $archive.Entries){if($seen.ContainsKey($entry.FullName)){throw 'V2_ZIP_DUPLICATE_HOLD'};$seen[$entry.FullName]=$true;$stream=$entry.Open();$mem=[IO.MemoryStream]::new();try{$stream.CopyTo($mem);$sha=Wrapper-V2Hash $mem.ToArray()}finally{$stream.Dispose();$mem.Dispose()};$expected=if($entry.FullName -ceq 'HostManifest.json'){$ms}else{$m.files_sha256[$entry.FullName]};if($sha -cne $expected){throw 'V2_ZIP_SHA_HOLD'}}
 }finally{$archive.Dispose();$zipStream.Dispose()}
 $stage='CANDIDATE_VERIFIED'
 if($Action -eq 'Inspect'){$result=@{schema=2;status='V2_PACKAGE_INSPECT_PASS_NOT_INSTALLED';manifest_sha256=$ms;installer_sha256=$ins;zip_sha256=$zipSha;approved=$false;system_changes=0;automatic_retries=0}}
 else{
  $stage='APPROVED_INSTALL_ONCE'
  $sealedInstaller=[scriptblock]::Create([Text.Encoding]::UTF8.GetString($verifiedPayloads['Deployment/install-session0-v2.ps1']))
  $result=& $sealedInstaller -Action Install -Bundle $root -ApprovalPath $ApprovalPath -ZipSha $zipSha -ConfirmSystemChanges -VerifiedManifestBytes $manifestBytes -VerifiedPayloads $verifiedPayloads
  # installer가 내보낸 정제 JSON만 전달합니다. Host/IPC를 시작하는 경로는 없습니다.
  $result=($result -join "`n")|ConvertFrom-Json -AsHashtable
 }
}catch{$result=@{schema=2;status='HOLD';stage=$stage;error_types=@($_.Exception.GetType().FullName);hresult=$_.Exception.HResult;automatic_retries=0;reason='V2_WRAPPER_HOLD_NO_EXECUTION_RETRY'}}
$result|ConvertTo-Json -Depth 14
# exit 없이 논리적 성공/실패만 JSON으로 반환합니다.

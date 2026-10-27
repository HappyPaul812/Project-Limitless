param([Parameter(Mandatory)][string]$Candidate,[Parameter(Mandatory)][string]$Output)
$ErrorActionPreference='Stop'
$package=Get-Content -Raw -LiteralPath ($Candidate+'.package.json')|ConvertFrom-Json
$stage=& ($Candidate+'\Deployment\stage-v2-package.ps1') -Action InspectArchive -SourceZip ($Candidate+'.zip') -ManifestSha $package.manifest_sha256 -InstallerSha $package.installer_sha256 -ZipSha $package.zip_sha256 -PayloadCount $package.payload_count
$stageResult=$stage|ConvertFrom-Json
if($stageResult.status -cne 'STAGING_ARCHIVE_OFFLINE_PASS_NOT_CREATED'){throw 'NEW_STAGE_ARCHIVE_FAILED'}
$wrapped=& ($Candidate+'\Deployment\run-session0-v2-install.ps1') -Action Inspect -Bundle $Candidate;$w=$wrapped|ConvertFrom-Json
if($w.status -cne 'V2_PACKAGE_INSPECT_PASS_NOT_INSTALLED'){throw 'NEW_WRAPPER_INSPECT_FAILED'}
$mb=[IO.File]::ReadAllBytes(($Candidate+'\HostManifest.json'));$m=[Text.Encoding]::UTF8.GetString($mb)|ConvertFrom-Json -AsHashtable
$payloads=@{};foreach($n in $m.files_sha256.Keys){$payloads[$n]=[IO.File]::ReadAllBytes((Join-Path $Candidate $n))}
$installer=[scriptblock]::Create([Text.Encoding]::UTF8.GetString($payloads['Deployment/install-session0-v2.ps1']))
& $installer -DefinitionsOnly -VerifiedManifestBytes $mb -VerifiedPayloads $payloads
$sealed=& $installer -Action Inspect -Bundle 'F:\SYNTHETIC_NO_MUTABLE_PATH' -VerifiedManifestBytes $mb -VerifiedPayloads $payloads;$sealedResult=$sealed|ConvertFrom-Json
if($sealedResult.status -cne 'V2_CANDIDATE_INSPECT_ONLY'){throw 'NEW_MEMORY_INSPECT_FAILED'}
$noCreate=& ($Candidate+'\Deployment\stage-v2-package.ps1') -Action Create -SourceZip 'F:\SYNTHETIC_MUST_NOT_READ.zip' -ManifestSha $package.manifest_sha256 -InstallerSha $package.installer_sha256 -ZipSha $package.zip_sha256 -PayloadCount $package.payload_count
$noCreateResult=$noCreate|ConvertFrom-Json
if($noCreateResult.status -cne 'HOLD' -or $noCreateResult.stage -cne 'PIN_ARCHIVE'){throw 'NEW_CREATE_GUARD_FAILED'}
$sourceMismatches=@();$sourceRoot=$PSScriptRoot
foreach($n in $m.files_sha256.Keys){
 if($n.StartsWith('Code/V2/') -and $n -ne 'Code/V2/InstallAssembly.cs'){
  $name=Split-Path $n -Leaf;$p=Join-Path $sourceRoot $name
  if($name -in @('TrialAccountPolicy.cs','BrokerJob90.cs','service-token-metadata.py')){$p=Join-Path (Split-Path $sourceRoot) $name}
 }elseif($n.StartsWith('Deployment/') -and $n.EndsWith('.ps1')){$p=Join-Path $sourceRoot (Split-Path $n -Leaf)}else{continue}
 if((Get-FileHash -LiteralPath $p).Hash.ToLowerInvariant() -cne $m.files_sha256[$n]){$sourceMismatches+=$n}
}
if($sourceMismatches.Count){throw 'NEW_SOURCE_PAYLOAD_MISMATCH'}
$old='F:\study\codex\Project-Limitless\Temp\Session0V2Candidate20261027-r9\HostManifest.json'
if((Get-FileHash -LiteralPath $old).Hash.ToLowerInvariant() -cne '250bab2d46010841353a319fbcf34581784366ac3042e6bc0fdc97aadd17c09e'){throw 'R9_COMPARISON_MANIFEST_PIN'}
$r9=Get-Content -LiteralPath $old -Raw|ConvertFrom-Json -AsHashtable
$added=@($m.files_sha256.Keys|Where-Object{-not $r9.files_sha256.ContainsKey($_)}|Sort-Object)
$changed=@($m.files_sha256.Keys|Where-Object{$r9.files_sha256.ContainsKey($_) -and $m.files_sha256[$_] -cne $r9.files_sha256[$_]}|Sort-Object)
$removed=@($r9.files_sha256.Keys|Where-Object{-not $m.files_sha256.ContainsKey($_)}|Sort-Object)
$runtimeUnchanged=@($r9.files_sha256.Keys|Where-Object{$_.StartsWith('Runtime/') -and $m.files_sha256[$_] -ceq $r9.files_sha256[$_]})
if($added.Count -ne 3 -or $changed.Count -ne 3 -or $removed.Count -ne 0 -or $runtimeUnchanged.Count -ne 741){throw 'NEW_COMPARISON_UNEXPECTED'}
$worker='S-1-5-21-2858504392-1958168942-1662906075-1007';$admin='S-1-5-21-2858504392-1958168942-1662906075-1001';$now=[DateTimeOffset]::UtcNow.ToUnixTimeSeconds()
# 메모리 합성 승인 계약만 검사합니다. 승인 파일을 만들거나 설치기로 전달하지 않습니다.
$approval=@{schema=2;kind='INSTALL_V2';approved=$true;preserve_v1_hold=$true;approval_id=$m.approval_id;epoch=$m.epoch;manifest_sha256=$package.manifest_sha256;installer_sha256=$package.installer_sha256;zip_sha256=$package.zip_sha256;worker_sid=$worker;admin_sid=$admin;expires_unix=$now+100}
[LimitlessV2Contract]::Approval(($approval|ConvertTo-Json -Compress),$package.manifest_sha256,$package.installer_sha256,$package.zip_sha256,$m.approval_id,$m.epoch,$worker,$admin,$now)
$approval.approval_id=$r9.approval_id;$approval.epoch=$r9.epoch;$rejected=$false
try{[LimitlessV2Contract]::Approval(($approval|ConvertTo-Json -Compress),$package.manifest_sha256,$package.installer_sha256,$package.zip_sha256,$m.approval_id,$m.epoch,$worker,$admin,$now)}catch{$rejected=$true}
if(-not $rejected){throw 'NEW_R9_APPROVAL_MIX_NOT_REJECTED'}
$summary=@{passed=7;failed=0;candidate=$Candidate;archive=$stageResult;wrapper=$w;sealed_inspect=$sealedResult;unapproved_create_gate=$noCreateResult;source_mismatches=$sourceMismatches;r9_approval_mix_rejected=$true;approval_files_created=0;r9_diff=@{added=$added;changed=$changed;removed=$removed;runtime_unchanged=$runtimeUnchanged.Count;manifest_changed=$true};native_calls=0;actual_staging_created=$false;actual_installs=0}
$summary|ConvertTo-Json -Depth 12|Set-Content -LiteralPath $Output -Encoding utf8
@{passed=7;failed=0;candidate=$Candidate;payload=$payloads.Count;entries=$payloads.Count+1;diff=$summary.r9_diff}|ConvertTo-Json -Depth 5

param([ValidateSet('Inspect','Install')][string]$Action='Inspect',[Parameter(Mandatory)][string]$Bundle,[Parameter(Mandatory)][string]$ManifestSha,[Parameter(Mandatory)][string]$InstallerSha,[Parameter(Mandatory)][string]$ZipSha,[string]$ApprovalPath,[switch]$ConfirmSystemChanges)
$ErrorActionPreference='Stop';$stageSession=$null;$stage='SEALED_PACKAGE';$result=@{schema=3;status='HOLD';automatic_retries=0;account_calls=0;task_calls=0;ipc_calls=0}
try{
 function H3([byte[]]$b){[Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($b)).ToLowerInvariant()}
 foreach($pin in @($ManifestSha,$InstallerSha,$ZipSha)){if($pin -cnotmatch '^[a-f0-9]{64}$'){throw 'D3_PIN_FORMAT'}}
 $root=[IO.Path]::GetFullPath($Bundle);$mb=[IO.File]::ReadAllBytes((Join-Path $root 'HostManifest.json'))
 if((H3 $mb) -cne $ManifestSha -or (H3 ([IO.File]::ReadAllBytes((Join-Path $root 'Deployment/install-session0-v2.ps1')))) -cne $InstallerSha){throw 'D3_INSTALLER_MANIFEST_PIN'}
 $m=[Text.Encoding]::UTF8.GetString($mb)|ConvertFrom-Json -AsHashtable
 if($Action -eq 'Install'){
  $stage='PROTECTED_APPROVAL_BEFORE_COMPILATION';$expected='C:\ProgramData\LimitlessRelayDedicatedV2Approvals\'+$m.approval_id+'.json'
  if(-not $ConfirmSystemChanges -or $m.approval_id -cnotmatch '^dedicated-install-[a-f0-9]{32}$' -or [IO.Path]::GetFullPath($ApprovalPath) -cne $expected){throw 'D3_EXPLICIT_INSTALL_APPROVAL_REQUIRED'}
  $acl=Get-Acl -LiteralPath $expected;$sd=[Security.AccessControl.RawSecurityDescriptor]::new($acl.Sddl)
  if($sd.Owner.Value -cne 'S-1-5-32-544' -or $sd.Group.Value -cne 'S-1-5-18' -or -not ($sd.ControlFlags -band [Security.AccessControl.ControlFlags]::DiscretionaryAclProtected) -or $sd.DiscretionaryAcl.Count -ne 2){throw 'D3_APPROVAL_SD'}
  foreach($ace in $sd.DiscretionaryAcl){if($ace -isnot [Security.AccessControl.CommonAce] -or $ace.AccessMask -ne 2032127 -or $ace.AceFlags -ne 0 -or $ace.AceQualifier -ne 'AccessAllowed' -or $ace.SecurityIdentifier.Value -notin @('S-1-5-18','S-1-5-32-544')){throw 'D3_APPROVAL_ACE'}}
  $approvalRaw=[IO.File]::ReadAllText($expected);$a=$approvalRaw|ConvertFrom-Json -AsHashtable
  if($a.approved -isnot [bool] -or -not $a.approved -or $a.schema -ne 3 -or $a.kind -cne 'DEDICATED_INSTALL_ONCE' -or $a.manifest_sha256 -cne $ManifestSha -or $a.installer_sha256 -cne $InstallerSha -or $a.zip_sha256 -cne $ZipSha){throw 'D3_APPROVED_PIN_REQUIRED'}
 }
 $payload=@{};foreach($n in $m.files_sha256.Keys){if($n -cnotmatch '^(Code|Runtime|Deployment)/' -or $n.Contains('..') -or $n.Contains(':') -or $n.Contains('\')){throw 'D3_BOOTSTRAP_PATH'};$b=[IO.File]::ReadAllBytes((Join-Path $root $n));if((H3 $b) -cne $m.files_sha256[$n]){throw 'D3_SEALED_SHA'};$payload[$n]=$b}
 if(@(Get-ChildItem -LiteralPath $root -Recurse -File -Force).Count -ne $payload.Count+1){throw 'D3_EXACT_SET'}
 if('DedicatedContract' -as [type]){throw 'D3_FRESH_PROCESS_REQUIRED'}
 Add-Type -TypeDefinition ([Text.Encoding]::UTF8.GetString($payload['Code/V2/InstallAssembly.cs']))
 $id='';$all=[LimitlessV2Staging]::Archive([IO.File]::ReadAllBytes($root+'.zip'),$ManifestSha,$InstallerSha,$ZipSha,$payload.Count,[ref]$id)
 [DedicatedContract]::Manifest([Text.Encoding]::UTF8.GetString($mb))
 if($Action -eq 'Inspect'){$result=@{schema=3;status='DEDICATED_PACKAGE_OFFLINE_PASS_NOT_INSTALLED';payload_count=$payload.Count;manifest_sha256=$ManifestSha;installer_sha256=$InstallerSha;zip_sha256=$ZipSha;approved=$false;auto_start=$false;model_calls=0;actual_installs=0}}
 else{
  $stage='NATIVE_PROTECTED_DEDICATED_STAGING';$stageSession=[LimitlessV2Staging]::Verify([V2NativeStageFs]::new(),$root,$ManifestSha,$InstallerSha,$ZipSha,$payload.Count)
  foreach($n in $payload.Keys){if((H3 $stageSession.Bytes[$n]) -cne (H3 $payload[$n])){throw 'D3_STAGE_SEALED_MISMATCH'}}
  [LimitlessV2Publish]::Verify($ApprovalPath,[DedicatedContract]::FsSddl('private',$false,'',''))
  . ([scriptblock]::Create([Text.Encoding]::UTF8.GetString($payload['Code/V2/DedicatedCore.ps1'])))
  . ([scriptblock]::Create([Text.Encoding]::UTF8.GetString($payload['Code/V2/DedicatedWindows.ps1'])))
  $bp=Read-Host '새 Dedicated Bot 암호 (로컬 보안 입력)' -AsSecureString;$wp=Read-Host '새 Dedicated Worker 암호 (로컬 보안 입력)' -AsSecureString
  try{$api=New-D3WindowsApi $m $payload $bp $wp;$result=Invoke-D3Install $api $mb $payload $approvalRaw $ZipSha;$stageSession.Recheck()}finally{$bp=$null;$wp=$null}
 }
}catch{$result=@{schema=3;status='HOLD';stage=$stage;errors=if('DedicatedContract' -as [type]){[DedicatedContract]::ErrorCodes($_.Exception)}else{@{type=$_.Exception.GetType().FullName;hresult=$_.Exception.HResult}};automatic_retries=0;partial_assets_preserve=$true}}
finally{if($stageSession){try{$stageSession.Dispose()}catch{$result.status='HOLD';$result.stage_close=@{type=$_.Exception.GetType().FullName;hresult=$_.Exception.HResult}}}}
$result|ConvertTo-Json -Depth 14

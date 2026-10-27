param([ValidateSet('Inspect','Stage')][string]$Action='Inspect',[switch]$ConfirmCreateR2StageOnce)
# 계정/Task/설치승인 분기는 없습니다. 기본 Inspect는 archive 메모리 검사뿐입니다.
$ErrorActionPreference='Stop';Set-StrictMode -Version Latest
$source='F:\study\codex\Project-Limitless\Temp\Session0DedicatedV2Candidate20261027-r2.zip'
$ms='7c8410145ed84f06fc9901e93f78e18d0cc103ad89757172e08c84f91d8a3976';$ins='83c97d781fb2d9aed797b41ea6bc799b83e939ad3ffa6f7af6a2d26f1080d68f';$zs='4efa0aa8a38b32fa40fa5a2dfb4042e028eeb07b1ab8d73c713a1d80efe028d0';$package='3effaac30bf44760986fee9e6019b283'
$phase='PIN_R2_ARCHIVE';$session=$null;$rootHandle=$null;$r1Handle=$null;$r1Stamp=$null;$allow=[Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
$result=@{schema=3;kind='R2_STAGE_ONLY';action=$Action;status='HOLD';creation_attempted=$false;automatic_retries=0;installed=$false;account_calls=0;task_calls=0;approval_writes=0;ipc_calls=0;model_calls=0}
function Get-R2StageSha256([byte[]]$bytes){[Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($bytes)).ToLowerInvariant()}
function Get-R2StageError($e){$rows=@();for($n=0;$e -and $n -lt 8;$n++){$row=@{type=$e.GetType().FullName;hresult=$e.HResult};if($e -is [ComponentModel.Win32Exception]){$row.win32=$e.NativeErrorCode};if($allow.Contains($e.Message)){$row.guard=$e.Message};$rows+=$row;$e=$e.InnerException};$rows}
try{
 if($PSVersionTable.PSVersion.Major -ne 7 -or -not [Environment]::Is64BitProcess){throw 'R2_PS7_X64_REQUIRED'}
 if('DedicatedContract' -as [type]){throw 'R2_FRESH_PROCESS_REQUIRED'}
 if($Action -eq 'Stage' -and -not $ConfirmCreateR2StageOnce){throw 'R2_EXPLICIT_STAGE_ONLY_REQUIRED'}
 $f=[IO.File]::Open($source,[IO.FileMode]::Open,[IO.FileAccess]::Read,[IO.FileShare]::Read)
 try{if($f.Length -gt 128MB){throw 'R2_ZIP_BOUND'};$mem=[IO.MemoryStream]::new();try{$f.CopyTo($mem);$zb=$mem.ToArray()}finally{$mem.Dispose()}}finally{$f.Dispose()}
 if((Get-R2StageSha256 $zb) -cne $zs){throw 'R2_ZIP_PIN'}
 $mem=[IO.MemoryStream]::new($zb,$false);$zip=[IO.Compression.ZipArchive]::new($mem,[IO.Compression.ZipArchiveMode]::Read)
 try{
  function Read-R2StageEntry([string]$name){$a=@($zip.Entries|Where-Object FullName -CEQ $name);if($a.Count -ne 1 -or $a[0].Length -gt 64MB){throw 'R2_ENTRY'};$s=$a[0].Open();$o=[IO.MemoryStream]::new();try{$s.CopyTo($o);return ,$o.ToArray()}finally{$s.Dispose();$o.Dispose()}}
  $mb=Read-R2StageEntry 'HostManifest.json';if((Get-R2StageSha256 $mb) -cne $ms){throw 'R2_MANIFEST_PIN'};$m=[Text.Encoding]::UTF8.GetString($mb)|ConvertFrom-Json -AsHashtable
  $assembly=Read-R2StageEntry 'Code/V2/InstallAssembly.cs';if((Get-R2StageSha256 $assembly) -cne $m.files_sha256['Code/V2/InstallAssembly.cs']){throw 'R2_ASSEMBLY_PIN'}
 }finally{$zip.Dispose();$mem.Dispose()}
 $shim=[IO.File]::ReadAllBytes('F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\Tools\DiscordAIServiceDiagnostics\R2Staging\R2StageBoundary.cs')
 if((Get-R2StageSha256 $shim) -cne '7203eec5d26fe02b18fb9379e1ad12cb36fb8fea7169a58c6987e012f4cf3664'){throw 'R2_SHIM_PIN'}
 $cs=[Text.Encoding]::UTF8.GetString($assembly)+"`n"+[Text.Encoding]::UTF8.GetString($shim)
 foreach($match in [regex]::Matches($cs,'"((?:STAGE_|D3_|V2_|R2_)[A-Z0-9_]+)"')){$null=$allow.Add($match.Groups[1].Value)}
 Add-Type -TypeDefinition $cs
 $id='';$all=[LimitlessV2Staging]::Archive($zb,$ms,$ins,$zs,766,[ref]$id)
 if($id -cne $package -or $m.package_id -cne $package -or $m.approval_id -cne ('dedicated-install-'+$package) -or $m.epoch -cne ('20261027-dedicated-'+$package) -or $m.pipe_name -cne ('LimitlessDedicatedV2-'+$package) -or $m.staging_root -cne [DedicatedContract]::StagingRoot){throw 'R2_IDENTITY_BINDING'}
 $result.package_id=$package;$result.payload_count=766;$result.bundle_file_count=767;$result.manifest_sha256=$ms;$result.installer_sha256=$ins;$result.zip_sha256=$zs;$result.bundle=[IO.Path]::Combine([R2StageBoundary]::Target,'Bundle')
 if($Action -eq 'Inspect'){$result.status='R2_ARCHIVE_PASS_NOT_CREATED'}
 else{
  $phase='ADMIN_PRIVATE_PARENT_R1_READONLY'
  $me=[Security.Principal.WindowsIdentity]::GetCurrent();if(-not ([Security.Principal.WindowsPrincipal]::new($me)).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)){throw 'R2_ADMIN_REQUIRED'}
  $native=[V2NativeStageFs]::new();$rootHandle=$native.Open([DedicatedContract]::StagingRoot);[LimitlessV2Staging]::Protected($rootHandle.Info(),[DedicatedContract]::StagingRoot,$true)
  $r1Handle=$native.Open([R2StageBoundary]::R1);[LimitlessV2Staging]::Protected($r1Handle.Info(),[R2StageBoundary]::R1,$true);$r1Stamp=[R2StageBoundary]::Stamp($r1Handle.Info())
  if($native.Exists([R2StageBoundary]::Target)){throw 'R2_EXISTING_TARGET_HOLD'}
  $bounded=[R2StageBoundary]::new($native);$phase='CREATE_R2_PRIVATE_ONCE';$result.creation_attempted=$true
  $session=[LimitlessV2Staging]::Create($bounded,$source,$ms,$ins,$zs,766)
  $phase='EXACT_BYTES_OWNER_DACL_IDS';[R2StageBoundary]::ExactCopies($bounded,$session,$zb)
  $h=$bounded.Open($session.Bundle);try{$i=$h.Info();[LimitlessV2Staging]::Protected($i,$session.Bundle,$true);$result.directory_sddl=$i.Sddl;$result.directory_file_id=$i.Id;$result.volume=$i.Volume;$sd=[Security.AccessControl.RawSecurityDescriptor]::new($i.Sddl);$result.owner_sid=$sd.Owner.Value;$result.group_sid=$sd.Group.Value}finally{$h.Dispose()}
  $h=$bounded.Open((Join-Path $session.Bundle 'HostManifest.json'));try{$i=$h.Info();[LimitlessV2Staging]::Protected($i,$i.Path,$false);$result.manifest_file_sddl=$i.Sddl}finally{$h.Dispose()}
  if([R2StageBoundary]::Stamp($r1Handle.Info()) -cne $r1Stamp){throw 'R2_R1_DIRECTORY_METADATA_CHANGED'}
  $session.Recheck();$result.r1_directory_metadata_preserved=$true;$result.writes_restricted_to_r2=$true;$result.exact_bytes_verified=$true;$result.all_file_metadata_sha_verified=$true;$result.bundle_zip=$session.Bundle+'.zip';$result.file_creation_attempts=$bounded.FileCreationAttempts;$result.status='R2_STAGING_CREATED_VERIFIED_NOT_INSTALLED'
 }
}catch{$result.status='HOLD';$result.phase=$phase;$result.errors=@(Get-R2StageError $_.Exception);$result.partial_assets_preserve_no_retry=$true}
finally{foreach($h in @($session,$r1Handle,$rootHandle)){if($h){try{$h.Dispose()}catch{$result.status='HOLD';$result.close_errors=@(Get-R2StageError $_.Exception)}}}}
$result|ConvertTo-Json -Depth 12

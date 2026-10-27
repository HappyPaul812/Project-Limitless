param([ValidateSet('InspectArchive','Verify','Create')][string]$Action='InspectArchive',[Parameter(Mandatory)][string]$SourceZip,[Parameter(Mandatory)][string]$ManifestSha,[Parameter(Mandatory)][string]$InstallerSha,[Parameter(Mandatory)][string]$ZipSha,[Parameter(Mandatory)][int]$PayloadCount,[string]$Bundle,[switch]$ConfirmCreateProtectedStaging)
$ErrorActionPreference='Stop';$session=$null;$stage='PIN_ARCHIVE';$result=@{schema=2;status='HOLD';automatic_retries=0;installed=$false;created=$false}
try{
 if($Action -eq 'Create' -and -not $ConfirmCreateProtectedStaging){throw 'STAGING_SEPARATE_CREATE_APPROVAL_REQUIRED'}
 foreach($pin in @($ManifestSha,$InstallerSha,$ZipSha)){if($pin -cnotmatch '^[a-f0-9]{64}$'){throw 'STAGE_SHA_FORMAT'}}
 function StageSha([byte[]]$bytes){[Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($bytes)).ToLowerInvariant()}
 $source=[IO.File]::Open($SourceZip,[IO.FileMode]::Open,[IO.FileAccess]::Read,[IO.FileShare]::Read)
 try{if($source.Length -gt 128MB){throw 'STAGE_BOOTSTRAP_ZIP_BOUND'};$memory=[IO.MemoryStream]::new();try{$source.CopyTo($memory);if($memory.Length -gt 128MB){throw 'STAGE_BOOTSTRAP_ZIP_BOUND'};$zipBytes=$memory.ToArray()}finally{$memory.Dispose()}}finally{$source.Dispose()}
 if((StageSha $zipBytes) -cne $ZipSha){throw 'STAGE_BOOTSTRAP_ZIP_PIN'}
 $ms=[IO.MemoryStream]::new($zipBytes,$false);$zip=[IO.Compression.ZipArchive]::new($ms,[IO.Compression.ZipArchiveMode]::Read)
 try{
  $entries=@($zip.Entries|Where-Object FullName -CEQ 'HostManifest.json');if($entries.Count -ne 1){throw 'STAGE_BOOTSTRAP_MANIFEST_COUNT'}
  function StageRead($entry){$s=$entry.Open();$m=[IO.MemoryStream]::new();try{if($entry.Length -gt 64MB){throw 'STAGE_BOOTSTRAP_BOUND'};$s.CopyTo($m);return ,$m.ToArray()}finally{$s.Dispose();$m.Dispose()}}
  $raw=StageRead $entries[0];if((StageSha $raw) -cne $ManifestSha){throw 'STAGE_BOOTSTRAP_MANIFEST_PIN'}
  $manifest=[Text.Encoding]::UTF8.GetString($raw)|ConvertFrom-Json -AsHashtable
  $entry=@($zip.Entries|Where-Object FullName -CEQ 'Code/V2/InstallAssembly.cs');if($entry.Count -ne 1){throw 'STAGE_BOOTSTRAP_ASSEMBLY_COUNT'}
  $assembly=StageRead $entry[0];if((StageSha $assembly) -cne $manifest.files_sha256['Code/V2/InstallAssembly.cs']){throw 'STAGE_BOOTSTRAP_ASSEMBLY_PIN'}
 }finally{$zip.Dispose();$ms.Dispose()}
 if('LimitlessV2Staging' -as [type]){throw 'STAGE_FRESH_NO_PROFILE_POWERSHELL_REQUIRED'}
 Add-Type -TypeDefinition ([Text.Encoding]::UTF8.GetString($assembly))
 $id='';$all=[LimitlessV2Staging]::Archive($zipBytes,$ManifestSha,$InstallerSha,$ZipSha,$PayloadCount,[ref]$id)
 $stage='ARCHIVE_VERIFIED';$result.package_id=$id;$result.payload_count=$PayloadCount;$result.zip_entries=$all.Count
 if($Action -eq 'InspectArchive'){$result.status='STAGING_ARCHIVE_OFFLINE_PASS_NOT_CREATED'}
 else{
  $fs=[V2NativeStageFs]::new();$stage='NATIVE_'+$Action.ToUpperInvariant()
  if($Action -eq 'Create'){$result.creation_attempted=$true;$result.created=$null;$session=[LimitlessV2Staging]::Create($fs,[IO.Path]::GetFullPath($SourceZip),$ManifestSha,$InstallerSha,$ZipSha,$PayloadCount);$result.created=$true;$result.status='STAGING_CREATED_VERIFIED_NOT_INSTALLED'}
  else{$session=[LimitlessV2Staging]::Verify($fs,$Bundle,$ManifestSha,$InstallerSha,$ZipSha,$PayloadCount);$result.status='STAGING_VERIFIED_NOT_INSTALL_APPROVAL'}
  $session.Recheck();$result.bundle=$session.Bundle
 }
}catch{
 $result.status='HOLD';$result.stage=$stage;$result.errors=@();$e=$_.Exception
 while($e){$row=@{type=$e.GetType().FullName;hresult=$e.HResult};if($e -is [ComponentModel.Win32Exception]){$row.win32=$e.NativeErrorCode};if($e.Data.Contains('stage_close_hresult')){$row.close_hresult=$e.Data['stage_close_hresult']};$result.errors+=$row;$e=$e.InnerException}
 $result.partial_assets_preserve_no_retry=($Action -eq 'Create')
}finally{if($session){try{$session.Dispose()}catch{$result.status='HOLD';$result.close_error=@{type=$_.Exception.GetType().FullName;hresult=$_.Exception.HResult}}}}
$result|ConvertTo-Json -Depth 12

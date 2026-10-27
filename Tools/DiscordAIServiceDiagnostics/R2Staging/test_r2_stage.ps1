param([Parameter(Mandatory)][string]$OutputPath)
$ErrorActionPreference='Stop';Set-StrictMode -Version Latest
$candidate='F:\study\codex\Project-Limitless\Temp\Session0DedicatedV2Candidate20261027-r2'
$pins=Get-Content ($candidate+'.package.json') -Raw|ConvertFrom-Json -AsHashtable
$assembly=[IO.File]::ReadAllText($candidate+'\Code\V2\InstallAssembly.cs')
$fake=[IO.File]::ReadAllText((Join-Path $PSScriptRoot '../V2/test_V2StagingFake.cs')) -replace '(?m)^using [A-Za-z0-9_.]+;',''
$boundary=[IO.File]::ReadAllText((Join-Path $PSScriptRoot 'R2StageBoundary.cs'))
Add-Type -TypeDefinition ($assembly+"`n"+$fake+"`n"+$boundary)
$zip=[IO.File]::ReadAllBytes($candidate+'.zip');$rows=@()
function New-R2Fake {
 $fs=[V2FakeStageFs]::new($script:zip);$fs.Add([DedicatedContract]::StagingRoot,$true,$null);$fs.Add([R2StageBoundary]::R1,$true,$null)
 $fs.Add(([R2StageBoundary]::R1+'\Bundle'),$true,$null);$fs.Add(([R2StageBoundary]::R1+'\Bundle\r1-kept.bin'),$false,[byte[]]@(8,9,10));$fs
}
function R1Fingerprint($fs){$items=@();foreach($p in @($fs.Nodes.Keys|Where-Object {$_.StartsWith([R2StageBoundary]::R1,[StringComparison]::Ordinal)}|Sort-Object)){$n=$fs.Nodes[$p];$items+=($n.Path+'|'+$n.Id+'|'+$n.Sd+'|'+[Convert]::ToHexString($n.Bytes))};$items -join "`n"}
function Check($n,$body){try{if(-not (& $body)){throw 'ASSERT'};$script:rows+=@{name=$n;status='PASS'}}catch{$script:rows+=@{name=$n;status='FAIL';type=$_.Exception.GetType().FullName}}}
Check 'R2_CREATE_IN_MEMORY_EXACT_BYTES_AND_R1_PRESERVED' {
 $fs=New-R2Fake;$before=R1Fingerprint $fs;$b=[R2StageBoundary]::new($fs)
 $s=[LimitlessV2Staging]::Create($b,[V2FakeStageFs]::Source,$pins.manifest_sha256,$pins.installer_sha256,$pins.zip_sha256,766)
 try{[R2StageBoundary]::ExactCopies($b,$s,$zip);$s.PayloadCount -eq 766 -and $s.Bytes.Count -eq 767 -and $s.PackageId -ceq '3effaac30bf44760986fee9e6019b283' -and $s.Bundle -ceq ([R2StageBoundary]::Target+'\Bundle') -and $b.FileCreationAttempts -eq 768 -and (R1Fingerprint $fs) -ceq $before -and @($fs.CreationDescriptors|Where-Object {$_ -cnotin @([LimitlessV2Staging]::DirSd,[LimitlessV2Staging]::FileSd)}).Count -eq 0}finally{$s.Dispose()}
}
foreach($path in @([DedicatedContract]::StagingRoot,[R2StageBoundary]::R1,([R2StageBoundary]::R1+'\tamper'),[DedicatedContract]::ApprovalRoot,([R2StageBoundary]::Target+'-other\x'),([R2StageBoundary]::Target+'\..\r1'),([R2StageBoundary]::Target+'\x:ads'),'C:\outside','\\server\share\x')){
 Check ('REJECT_WRITE_SCOPE_'+$path) {$fs=New-R2Fake;$b=[R2StageBoundary]::new($fs);try{$b.Directory($path,[LimitlessV2Staging]::DirSd);$false}catch{$fs.MutationCalls -eq 0}}
}
Check 'EXISTING_R2_PACKAGE_HOLD_NO_OVERWRITE' {$fs=New-R2Fake;$fs.Add([R2StageBoundary]::Target,$true,$null);$b=[R2StageBoundary]::new($fs);try{$s=[LimitlessV2Staging]::Create($b,[V2FakeStageFs]::Source,$pins.manifest_sha256,$pins.installer_sha256,$pins.zip_sha256,766);$s.Dispose();$false}catch{$fs.MutationCalls -eq 0}}
foreach($fault in @('CORRUPT_COPY','TARGET_HARDLINK','TARGET_ADS','TARGET_REPARSE','TARGET_WRITER','TARGET_GROUP','EXTRA_ENTRY','MUTATION_FAIL')){
 Check ('FAULT_HOLD_R1_PRESERVED_'+$fault) {$fs=New-R2Fake;$before=R1Fingerprint $fs;$fs.Fault=$fault;$b=[R2StageBoundary]::new($fs);try{$s=[LimitlessV2Staging]::Create($b,[V2FakeStageFs]::Source,$pins.manifest_sha256,$pins.installer_sha256,$pins.zip_sha256,766);$s.Dispose();$false}catch{(R1Fingerprint $fs) -ceq $before}}
}
Check 'WRONG_ZIP_SHA_HOLD_NO_MUTATION' {$fs=New-R2Fake;$b=[R2StageBoundary]::new($fs);try{$s=[LimitlessV2Staging]::Create($b,[V2FakeStageFs]::Source,$pins.manifest_sha256,$pins.installer_sha256,('0'*64),766);$s.Dispose();$false}catch{$fs.MutationCalls -eq 0}}
Check 'WRONG_MANIFEST_SHA_HOLD_NO_MUTATION' {$fs=New-R2Fake;$b=[R2StageBoundary]::new($fs);try{$s=[LimitlessV2Staging]::Create($b,[V2FakeStageFs]::Source,('0'*64),$pins.installer_sha256,$pins.zip_sha256,766);$s.Dispose();$false}catch{$fs.MutationCalls -eq 0}}
Check 'R1_ARCHIVE_REJECTED_WITH_R2_PINS' {$old=[IO.File]::ReadAllBytes('F:\study\codex\Project-Limitless\Temp\Session0DedicatedV2Candidate20261027-r1.zip');$id='';try{$null=[LimitlessV2Staging]::Archive($old,$pins.manifest_sha256,$pins.installer_sha256,$pins.zip_sha256,766,[ref]$id);$false}catch{$true}}
Check 'EXACT_BYTE_VERIFICATION_REJECTS_POSTCOPY_CHANGE' {
 $fs=New-R2Fake;$b=[R2StageBoundary]::new($fs);$s=[LimitlessV2Staging]::Create($b,[V2FakeStageFs]::Source,$pins.manifest_sha256,$pins.installer_sha256,$pins.zip_sha256,766)
 try{$fs.Nodes[$s.Bundle+'\HostManifest.json'].Bytes[0]=0;try{[R2StageBoundary]::ExactCopies($b,$s,$zip);$false}catch{$true}}finally{$s.Dispose()}
}
Check 'STAGE_TOOL_ONLY_INSPECT_STAGE_NO_INSTALL_BRANCH' {$s=Get-Content (Join-Path $PSScriptRoot 'stage-r2-only.ps1') -Raw;$s.Contains("[ValidateSet('Inspect','Stage')]") -and $s.Contains("[string]`$Action='Inspect'") -and $s -notmatch 'Invoke-D3Install|New-D3WindowsApi|New-LocalUser|RegisterTask|Read-Host|Remove-Item|Set-Acl'}
Check 'PS_PARSE_FULL_ENTRY' {$t=$null;$e=$null;$null=[Management.Automation.Language.Parser]::ParseFile((Join-Path $PSScriptRoot 'stage-r2-only.ps1'),[ref]$t,[ref]$e);$e.Count -eq 0}
$r=@{kind='R2_STAGE_NEW_BOUNDARY_MEMORY_TESTS';pass=@($rows|Where-Object status -eq PASS).Count;fail=@($rows|Where-Object status -eq FAIL).Count;tests=$rows;real_staging_creations=0;account_calls=0;approval_writes=0;install_calls=0}
$r|ConvertTo-Json -Depth 8|Set-Content -LiteralPath $OutputPath -Encoding utf8NoBOM;$r|Select-Object pass,fail|ConvertTo-Json;if($r.fail){throw 'R2_STAGE_TEST_HOLD'}

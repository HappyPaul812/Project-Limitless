param([ValidateSet('Inspect','Cleanup')][string]$Action='Inspect',[switch]$ConfirmApprovedCleanup)
# 이 도구는 SID1008/옛 Code root 정리만 제공합니다. 설치/Task 등록/실행/exit 기능은 없습니다.
$ErrorActionPreference='Stop'
$repo='F:/study/codex/Project-Limitless/Temp/DiscordUsageDiagnostics20261026/Worktree'
$root='C:\Program Files\LimitlessRelayTrial'
$state='C:\ProgramData\LimitlessRelayTrial'
$backup='C:\ProgramData\LimitlessRelayTrialCleanup20261027'
$botSid='S-1-5-21-2858504392-1958168942-1662906075-1008'
$adminSid='S-1-5-21-2858504392-1958168942-1662906075-1001'
$workerSid='S-1-5-21-2858504392-1958168942-1662906075-1007'
$taskName='LIMITLESS-Trial-S0Host'
function Need-Cleanup($Value,[string]$Code){if(-not $Value){throw $Code}}
function Hash-Cleanup([byte[]]$Bytes){[Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($Bytes)).ToLower()}
function Read-CleanupManifest([string]$Base,[string]$Expected){
 $bytes=[IO.File]::ReadAllBytes((Join-Path $Base 'HostManifest.json'))
 Need-Cleanup ((Hash-Cleanup $bytes) -ceq $Expected) 'PRESERVED_SHA_CHANGED_HOLD'
 return [Text.Encoding]::UTF8.GetString($bytes)|ConvertFrom-Json
}
function Convert-CleanupCanonical($Value){
 if($null -eq $Value){return 'null'}
 if($Value -is [Collections.IDictionary]){
  [string[]]$keys=@($Value.Keys);[Array]::Sort($keys,[StringComparer]::Ordinal)
  $parts=@(foreach($key in $keys){($key|ConvertTo-Json -Compress)+':'+(Convert-CleanupCanonical $Value[$key])})
  return '{'+($parts -join ',')+'}'
 }
 if($Value -is [Collections.IEnumerable] -and $Value -isnot [string]){return '['+(@(foreach($v in $Value){Convert-CleanupCanonical $v}) -join ',')+']'}
 return ConvertTo-Json -InputObject $Value -Depth 10 -Compress
}
function Safe-CleanupError($Record){
 $nodes=@();$ex=$Record.Exception
 for($i=0;$null -ne $ex -and $i -lt 6;$i++){$n=@{type=$ex.GetType().FullName;hresult=$ex.HResult};if($ex -is [ComponentModel.Win32Exception]){$n.win32=$ex.NativeErrorCode};$nodes+=$n;$ex=$ex.InnerException}
 $known=@('FIXED_ADMIN_REQUIRED','EXPLICIT_APPROVED_CLEANUP_SWITCH_REQUIRED','CLEANUP_ALREADY_ATTEMPTED_OR_BACKUP_EXISTS_HOLD','CLEANUP_REFERENCES_OR_LAYOUT_HOLD','CLEANUP_ACCOUNT_IDENTITY_HOLD','CLEANUP_ACCOUNT_POLICY_CHANGED_HOLD','PRESERVED_SHA_CHANGED_HOLD','ONBOOT_CHANGED_HOLD','WORKER_IDENTITY_CHANGED_HOLD','CLEANUP_STATE_CHANGED_AFTER_BACKUP_HOLD','CLEANUP_BACKUP_NOT_VERIFIED_HOLD','CLEANUP_NATIVE_SHA_HOLD','TRIAL_TASK_PRESENT_OR_UNKNOWN_HOLD','UNEXPECTED_FILE_OR_DIRECTORY_HOLD','UNEXPECTED_STREAM_HOLD','UNEXPECTED_ATTRIBUTE_HOLD','CLEANUP_OWNER_DACL_HOLD','CLEANUP_ACE_HOLD','CLEANUP_ACE_FLAGS_HOLD','CLEANUP_DACL_PROTECTION_HOLD','CODE_ROOT_FINAL_UNKNOWN_HOLD','ACCOUNT_FINAL_UNKNOWN_HOLD','ACCOUNT_LAST_CHECK_HOLD','REFERENCES_CHANGED_BEFORE_ACCOUNT_DELETE_HOLD','ACCOUNT_CHANGED_BEFORE_DELETE_HOLD','BOT_NAME_CHANGED_HOLD','BOT_CIM_IDENTITY_UNKNOWN_HOLD','BOT_LOGON_OR_EXPIRY_UNKNOWN_HOLD','STATE_ROOT_UNEXPECTED_HOLD','EXACT_ROOT_REQUIRED','EXACT_NATIVE_ROOT_REQUIRED','EXPECTED_855_HOLD','UNKNOWN_DIRECTORY_HOLD','UNKNOWN_FILE_HOLD','BACKUP_SHA_HOLD','BACKUP_EVIDENCE_HOLD','CLEANUP_PROTECTION_HOLD','CLEANUP_PATH_SCOPE_HOLD')
 $code='CLEANUP_HOLD_DETAILS_REDACTED';if($Record.Exception.Message -cin $known){$code=$Record.Exception.Message}
 return @{code=$code;exceptions=$nodes}
}
function Assert-CleanupPath([string]$Path){
 $full=[IO.Path]::GetFullPath($Path)
 Need-Cleanup ($full.Equals($root,[StringComparison]::OrdinalIgnoreCase) -or $full.StartsWith($root+'\',[StringComparison]::OrdinalIgnoreCase)) 'CLEANUP_PATH_SCOPE_HOLD'
 return $full
}
function Assert-CleanupAcl([string]$Sddl,[bool]$Directory,[bool]$RootEntry,[bool]$PrivateBackup=$false){
 $d=[Security.AccessControl.RawSecurityDescriptor]::new($Sddl)
 Need-Cleanup ($d.Owner.Value -ceq 'S-1-5-32-544' -and $d.Group.Value -ceq 'S-1-5-18' -and $null -ne $d.DiscretionaryAcl -and $d.DiscretionaryAcl.Count -eq 2) 'CLEANUP_OWNER_DACL_HOLD'
 if($RootEntry -or $PrivateBackup){Need-Cleanup ([bool]($d.ControlFlags -band [Security.AccessControl.ControlFlags]::DiscretionaryAclProtected)) 'CLEANUP_DACL_PROTECTION_HOLD'}
 $seen=@{}
 foreach($a in $d.DiscretionaryAcl){
  Need-Cleanup ($a -is [Security.AccessControl.CommonAce] -and -not $a.IsCallback -and $a.AceQualifier -eq [Security.AccessControl.AceQualifier]::AccessAllowed -and $a.SecurityIdentifier.Value -in @('S-1-5-18','S-1-5-32-544') -and -not $seen.ContainsKey($a.SecurityIdentifier.Value) -and $a.AccessMask -eq 2032127) 'CLEANUP_ACE_HOLD'
  $seen[$a.SecurityIdentifier.Value]=$true
  $allowed=if($Directory){19}else{16} # OI/CI/Inherited만 허용, 감사/전파 예외를 임의 수용하지 않습니다.
  Need-Cleanup (([int]$a.AceFlags -band (-bnot $allowed)) -eq 0) 'CLEANUP_ACE_FLAGS_HOLD'
 }
}
function Assert-CleanupSnapshot($Snapshot){
 Need-Cleanup ($Snapshot.admin_sid -ceq $adminSid -and $Snapshot.worker_sid -ceq $workerSid -and $Snapshot.bot.sid -ceq $botSid -and $Snapshot.bot.name -ceq 'LIMITLESSBotTrial' -and $Snapshot.bot.enabled -eq $false -and $Snapshot.bot.description -ceq 'LIMITLESS_TRIAL_BOUNDARY_V1') 'CLEANUP_ACCOUNT_IDENTITY_HOLD'
 Need-Cleanup ($Snapshot.bot.group_sids.Count -eq 1 -and $Snapshot.bot.group_sids[0] -ceq 'S-1-5-32-545' -and $Snapshot.bot.direct_rights.Count -eq 0 -and $Snapshot.bot.expiry_utc -ceq '2026-10-27T13:41:13.2064794Z') 'CLEANUP_ACCOUNT_POLICY_CHANGED_HOLD'
 Need-Cleanup ($Snapshot.task_absent -eq $true -and $Snapshot.state_absent -eq $true -and $Snapshot.file_count -eq 855 -and $Snapshot.unexpected -eq 0 -and $Snapshot.refs.hits -eq 0 -and $Snapshot.refs.unknown -eq 0 -and $Snapshot.bot.profile_count -eq 0 -and $Snapshot.bot.logon_count -eq 0 -and $Snapshot.bot.last_logon_null -eq $true) 'CLEANUP_REFERENCES_OR_LAYOUT_HOLD'
 Need-Cleanup ($Snapshot.preserved -eq $true -and $Snapshot.root -ceq $root -and $Snapshot.fingerprint.Length -eq 64) 'CLEANUP_PROTECTION_HOLD'
}
function Invoke-CleanupCore($Snapshot,[hashtable]$Effects){
 Assert-CleanupSnapshot $Snapshot
 $proof=& $Effects.Backup $Snapshot
 Need-Cleanup ($proof.verified -eq $true) 'CLEANUP_BACKUP_NOT_VERIFIED_HOLD'
 $again=& $Effects.ReadAgain
 Assert-CleanupSnapshot $again
 Need-Cleanup ($again.fingerprint -ceq $Snapshot.fingerprint) 'CLEANUP_STATE_CHANGED_AFTER_BACKUP_HOLD'
 & $Effects.DeleteCode $again
 & $Effects.CheckAccount
 & $Effects.DeleteAccount
 & $Effects.Final
}
function Has-CleanupReference([string]$Value,[string[]]$Aliases){
 if([string]::IsNullOrEmpty($Value)){return $false}
 foreach($alias in $Aliases){if($Value.IndexOf($alias,[StringComparison]::OrdinalIgnoreCase) -ge 0){return $true}}
 return $false
}
function Read-CleanupTaskScan($Folder,[string[]]$Aliases){
 $hits=0;$count=0;$unknown=0
 foreach($t in $Folder.GetTasks(1)){$count++;$xml=[string]$t.Xml;if([string]::IsNullOrEmpty($xml)){$unknown++};if(Has-CleanupReference $xml $Aliases){$hits++}}
 foreach($f in $Folder.GetFolders(0)){$sub=Read-CleanupTaskScan $f $Aliases;$hits+=$sub.hits;$count+=$sub.count;$unknown+=$sub.unknown}
 return @{hits=$hits;count=$count;unknown=$unknown}
}
function Assert-TrialTaskAbsent {
 $s=New-Object -ComObject 'Schedule.Service';$s.Connect();$folder=$s.GetFolder('\')
 $missing=$false
 try{$null=$folder.GetTask($taskName)}catch{$e=$_.Exception;while($e.InnerException){$e=$e.InnerException};if($e.HResult -eq -2147024894){$missing=$true}else{throw}}
 Need-Cleanup ($missing -and $taskName -notin @($folder.GetTasks(1)|ForEach-Object{$_.Name})) 'TRIAL_TASK_PRESENT_OR_UNKNOWN_HOLD'
}
function Read-CleanupRefs([string[]]$Aliases){
 $s=New-Object -ComObject 'Schedule.Service';$s.Connect()
 $tasks=Read-CleanupTaskScan ($s.GetFolder('\')) $Aliases
 $serviceHits=0;$serviceCount=0;$serviceUnknown=0
 foreach($class in @('Win32_Service','Win32_SystemDriver')){
  foreach($service in Get-CimInstance $class -ErrorAction Stop){$serviceCount++;if([string]::IsNullOrEmpty($service.PathName)){$serviceUnknown++};if((Has-CleanupReference ([string]$service.PathName) $Aliases) -or (Has-CleanupReference ([string]$service.StartName) $Aliases)){$serviceHits++}}
 }
 $processHits=0;$unknown=@();$processCount=0
 foreach($p in Get-CimInstance Win32_Process -ErrorAction Stop){
  $processCount++
  # PID0/4의 OS 커널 pseudo-process 외의 조회 불가는 안전하다고 추측하지 않습니다.
  $kernel=($p.ProcessId -eq 0 -and $p.Name -eq 'System Idle Process') -or ($p.ProcessId -eq 4 -and $p.Name -eq 'System')
  if(-not $kernel -and ([string]::IsNullOrEmpty($p.CommandLine) -or [string]::IsNullOrEmpty($p.ExecutablePath))){$unknown+=[int]$p.ProcessId}
  if((Has-CleanupReference ([string]$p.CommandLine) $Aliases) -or (Has-CleanupReference ([string]$p.ExecutablePath) $Aliases)){$processHits++}
 }
 return @{hits=($tasks.hits+$serviceHits+$processHits);unknown=($unknown.Count+$serviceUnknown+$tasks.unknown);unknown_processes=$unknown.Count;unknown_services=$serviceUnknown;unknown_tasks=$tasks.unknown;unknown_pids=@($unknown|Select-Object -First 16);task_count=$tasks.count;service_count=$serviceCount;process_count=$processCount;task_hits=$tasks.hits;service_hits=$serviceHits;process_hits=$processHits}
}
function Read-CleanupBot {
 $u=Get-LocalUser -SID ([Security.Principal.SecurityIdentifier]::new($botSid)) -ErrorAction Stop
 Need-Cleanup ($u.Name -ceq 'LIMITLESSBotTrial') 'BOT_NAME_CHANGED_HOLD'
 Need-Cleanup ($null -ne $u.PSObject.Properties['LastLogon'] -and $null -ne $u.AccountExpires) 'BOT_LOGON_OR_EXPIRY_UNKNOWN_HOLD'
 $groups=@();foreach($g in Get-LocalGroup -ErrorAction Stop){if(@(Get-LocalGroupMember -SID $g.SID -ErrorAction Stop|Where-Object{$_.SID.Value -ceq $botSid}).Count){$groups+=$g.SID.Value}}
 $account=@(Get-CimInstance Win32_UserAccount -Filter ("SID='"+$botSid+"'") -ErrorAction Stop)
 Need-Cleanup ($account.Count -eq 1) 'BOT_CIM_IDENTITY_UNKNOWN_HOLD'
 $profiles=@(Get-CimInstance Win32_UserProfile -Filter ("SID='"+$botSid+"'") -ErrorAction Stop)
 $logons=@(Get-CimAssociatedInstance -InputObject $account[0] -Association Win32_LoggedOnUser -ResultClassName Win32_LogonSession -ErrorAction Stop)
 return @{name=$u.Name;sid=$u.SID.Value;enabled=$u.Enabled;description=$u.Description;expiry_utc=$u.AccountExpires.ToUniversalTime().ToString('o');group_sids=$groups;direct_rights=@([LimitlessTrialCleanupV1]::DirectRights($botSid));profile_count=$profiles.Count;logon_count=$logons.Count;last_logon_null=($null -eq $u.LastLogon)}
}
function Read-CleanupPreservation {
 $checks=@(
  @('F:/study/codex/Project-Limitless/Temp/Session0TaskSecurityAtomicCandidate20261026/HostManifest.json','6efbd7839bd4e452d9fbca8a465290533472e87cde71845a73bd37bdb21493ab'),
  @('F:/study/codex/Project-Limitless/Temp/Session0TaskSecurityAtomicCandidate20261026/Deployment/install-session0-trial.ps1','117065bbfca686474f9104bc0dff20fa6528dfb9c204e5a2f2720d91994958e9'),
  @('F:/study/codex/Project-Limitless/Temp/Session0TaskSecurityAtomicCandidate20261026.zip','4b2c56edf5bb8edc6ee7a40a10504f537223bc77874b43a36e61be735c318f83'),
  @('F:/study/codex/Project-Limitless/Temp/Session0LsaRepairCandidate20261026/HostManifest.json','33ed9a8318c039cd70cf8260338f56e81025ce891c0156ac18556c2aec36d165'),
  @('F:/study/codex/Project-Limitless/Temp/Session0LsaRepairCandidate20261026/Deployment/install-session0-trial.ps1','79194ad63dfaf500158d7afa8aadf59e26e0a7487ee595ae9c685e35003c75f8'),
  @('F:/study/codex/Project-Limitless/Temp/Session0LsaRepairCandidate20261026.zip','d678e760c1ccd41d05a4683a776cf2e877026c1de9bc5fd0a61aeccb4231daf3'),
  @((Join-Path $repo '문서/00_프로젝트/팀운영/Evidence/Session0AtomicInstall20261026/0f8e15c951a144efa79f5ccdbeed9c55.json'),'80523d2b2320cabce31789d7322a3a7c480ff8f225d42c1f0120a3bdcbea5d6b'),
  @((Join-Path $repo '문서/00_프로젝트/팀운영/Evidence/Session0PartialReadOnly20261026/a11561f51e114b398c4b58d7686b9a57.json'),'f6f8c9e3589299a3dd918a4bb1395164d6797812bbfe85c832fda0cb234fb9be'),
  @('C:/ProgramData/LimitlessRelay/boot-approval.json','3e032ebcce3d91bd762a8dec81048d04607ddde761afd62489c0fa2b8e49140f'))
 foreach($pair in $checks){Need-Cleanup ((Get-FileHash -LiteralPath $pair[0] -Algorithm SHA256 -ErrorAction Stop).Hash.ToLower() -ceq $pair[1]) 'PRESERVED_SHA_CHANGED_HOLD'}
 foreach($base in @('F:/study/codex/Project-Limitless/Temp/Session0TaskSecurityAtomicCandidate20261026','F:/study/codex/Project-Limitless/Temp/Session0LsaRepairCandidate20261026')){
  $expectedManifest=if($base.Contains('LsaRepair')){'33ed9a8318c039cd70cf8260338f56e81025ce891c0156ac18556c2aec36d165'}else{'6efbd7839bd4e452d9fbca8a465290533472e87cde71845a73bd37bdb21493ab'}
  $m=Read-CleanupManifest $base $expectedManifest
  Need-Cleanup (@($m.files_sha256.PSObject.Properties).Count -eq 855 -and @(Get-ChildItem -LiteralPath $base -Recurse -File -Force -ErrorAction Stop).Count -eq 856) 'PRESERVED_SHA_CHANGED_HOLD'
  foreach($entry in $m.files_sha256.PSObject.Properties){
   $p=Join-Path $base $entry.Name;[LimitlessTrialCleanupV1]::NoReparseAncestors($p)
   Need-Cleanup ((Get-FileHash -LiteralPath $p -Algorithm SHA256 -ErrorAction Stop).Hash.ToLower() -ceq $entry.Value) 'PRESERVED_SHA_CHANGED_HOLD'
  }
 }
 $expected=@{'LIMITLESS-Bot-OnBoot'='e0c74168b935aa5d3fa418d65d66e03e49abeb1179975586c0ead4afd5b4df99';'LIMITLESS-Art-OnBoot'='bb5ccabf40f7148fca44fdf538bd30cca2590e08122338cc0ae1c0d1836ab579'}
 foreach($name in $expected.Keys){$xml=Export-ScheduledTask -TaskName $name -TaskPath '\' -ErrorAction Stop;Need-Cleanup ((Hash-Cleanup ([Text.Encoding]::UTF8.GetBytes($xml))) -ceq $expected[$name]) 'ONBOOT_CHANGED_HOLD'}
 Need-Cleanup ((Get-LocalUser -Name 'LIMITLESSRelaySvc' -ErrorAction Stop).SID.Value -ceq $workerSid) 'WORKER_IDENTITY_CHANGED_HOLD'
 return $true
}
function Read-CleanupSnapshot($Manifest,$Report){
 $Report.stage='READ_ACCOUNT';$bot=Read-CleanupBot;$Report.account=$bot
 $Report.stage='VERIFY_PRESERVATION';$preserved=Read-CleanupPreservation
 Assert-TrialTaskAbsent
 $stateNames=@(Get-ChildItem -LiteralPath 'C:\ProgramData' -Name -Force -ErrorAction Stop)
 Need-Cleanup ('LimitlessRelayTrial' -notin $stateNames) 'STATE_ROOT_UNEXPECTED_HOLD'
 $Report.stage='VERIFY_CODE_LAYOUT'
 Need-Cleanup ([IO.Path]::GetFullPath($root) -ceq 'C:\Program Files\LimitlessRelayTrial') 'EXACT_ROOT_REQUIRED'
 [LimitlessTrialCleanupV1]::NoReparseAncestors($root)
 $all=@(Get-ChildItem -LiteralPath $root -Recurse -Force -ErrorAction Stop)
 $expectedFiles=@{};foreach($p in $Manifest.files_sha256.PSObject.Properties){$expectedFiles[$p.Name]=$p.Value}
 Need-Cleanup ($expectedFiles.Count -eq 855) 'EXPECTED_855_HOLD'
 $expectedDirs=@{'EmptyWorkspace'=$true}
 foreach($name in $expectedFiles.Keys){$p=$name;while($p.Contains('/')){$p=$p.Substring(0,$p.LastIndexOf('/'));$expectedDirs[$p]=$true}}
 $files=@($all|Where-Object{-not $_.PSIsContainer});$dirs=@($all|Where-Object{$_.PSIsContainer})
 Need-Cleanup ($files.Count -eq 855 -and $dirs.Count -eq $expectedDirs.Count) 'UNEXPECTED_FILE_OR_DIRECTORY_HOLD'
 $inventory=@();$paths=@(Get-Item -LiteralPath $root -Force)+$all
 foreach($item in $paths){
  $path=Assert-CleanupPath $item.FullName
  Need-Cleanup (-not ($item.Attributes -band ([IO.FileAttributes]::ReparsePoint -bor [IO.FileAttributes]::Hidden -bor [IO.FileAttributes]::System -bor [IO.FileAttributes]::ReadOnly))) 'UNEXPECTED_ATTRIBUTE_HOLD'
  $relative=if($path -ceq $root){''}else{$path.Substring($root.Length+1).Replace('\','/')}
  $acl=Get-Acl -LiteralPath $path -ErrorAction Stop
  Assert-CleanupAcl $acl.Sddl $item.PSIsContainer ($relative -ceq '')
  $streams=@(Get-Item -LiteralPath $path -Stream '*' -ErrorAction Stop)
  Need-Cleanup (@($streams|Where-Object{$_.Stream -ne ':$DATA'}).Count -eq 0) 'UNEXPECTED_STREAM_HOLD'
  if($item.PSIsContainer){Need-Cleanup ($relative -ceq '' -or $expectedDirs.ContainsKey($relative)) 'UNKNOWN_DIRECTORY_HOLD';[LimitlessTrialCleanupV1]::ProbeDirectory($path)}else{
   Need-Cleanup ($expectedFiles.ContainsKey($relative)) 'UNKNOWN_FILE_HOLD'
   $data=[LimitlessTrialCleanupV1]::ReadPinned($path,$expectedFiles[$relative]);$data=$null
  }
  $inventory+=@{path=$relative;directory=$item.PSIsContainer;sddl=$acl.Sddl;sha256=if($item.PSIsContainer){''}else{$expectedFiles[$relative]};attributes=[int]$item.Attributes;creation_utc=$item.CreationTimeUtc.ToString('o');write_utc=$item.LastWriteTimeUtc.ToString('o');length=if($item.PSIsContainer){0}else{$item.Length}}
 }
 $Report.verified_code_files=855
 $Report.stage='CHECK_REFERENCES'
 $aliases=@('LimitlessRelayTrial','LIMITLESSBotTrial',$botSid,$taskName,[LimitlessTrialCleanupV1]::ShortRoot())
 $refs=Read-CleanupRefs $aliases;$Report.references=$refs
 $canonical=[ordered]@{bot=$bot;inventory=@($inventory|Sort-Object path);root=$root;admin=$adminSid;worker=$workerSid}
 $fingerprint=Hash-Cleanup ([Text.Encoding]::UTF8.GetBytes((Convert-CleanupCanonical $canonical)))
 return @{admin_sid=$adminSid;worker_sid=$workerSid;bot=$bot;root=$root;file_count=855;unexpected=0;task_absent=$true;state_absent=$true;refs=$refs;preserved=$preserved;fingerprint=$fingerprint;inventory=@($inventory|Sort-Object path);aliases=$aliases}
}
function Assert-BackupPath([string]$Path,[bool]$Directory){Assert-CleanupAcl (Get-Acl -LiteralPath $Path -ErrorAction Stop).Sddl $Directory $true $true}
function Make-CleanupBackup($Snapshot,$Report){
 $Report.stage='CREATE_PROTECTED_BACKUP_ONCE'
 [LimitlessTrialCleanupV1]::NewBackupDirectory($backup);$Report.cleanup_consumed=$true
 Assert-BackupPath $backup $true
 foreach($dir in @('CodeSnapshot','Evidence')){[LimitlessTrialCleanupV1]::NewBackupDirectory((Join-Path $backup $dir));Assert-BackupPath (Join-Path $backup $dir) $true}
 foreach($entry in @($Snapshot.inventory|Where-Object{$_.directory -and $_.path}|Sort-Object {$_.path.Length})){
  $p=Join-Path (Join-Path $backup 'CodeSnapshot') $entry.path
  [LimitlessTrialCleanupV1]::NewBackupDirectory($p);Assert-BackupPath $p $true
 }
 foreach($entry in $Snapshot.inventory|Where-Object{-not $_.directory}){
  $data=[LimitlessTrialCleanupV1]::ReadPinned((Join-Path $root $entry.path),$entry.sha256)
  $p=Join-Path (Join-Path $backup 'CodeSnapshot') $entry.path
  [LimitlessTrialCleanupV1]::NewBackupFile($p,$data);Assert-BackupPath $p $false
  Need-Cleanup ((Get-FileHash -LiteralPath $p -Algorithm SHA256).Hash.ToLower() -ceq $entry.sha256) 'BACKUP_SHA_HOLD'
 }
 $copies=@(
  @('OriginalApprovedHostManifest.json','F:/study/codex/Project-Limitless/Temp/Session0TaskSecurityAtomicCandidate20261026/HostManifest.json','6efbd7839bd4e452d9fbca8a465290533472e87cde71845a73bd37bdb21493ab'),
  @('Install1.json',(Join-Path $repo '문서/00_프로젝트/팀운영/Evidence/Session0AtomicInstall20261026/0f8e15c951a144efa79f5ccdbeed9c55.json'),'80523d2b2320cabce31789d7322a3a7c480ff8f225d42c1f0120a3bdcbea5d6b'),
  @('AdminReadOnly.json',(Join-Path $repo '문서/00_프로젝트/팀운영/Evidence/Session0PartialReadOnly20261026/a11561f51e114b398c4b58d7686b9a57.json'),'f6f8c9e3589299a3dd918a4bb1395164d6797812bbfe85c832fda0cb234fb9be'))
 foreach($pair in $copies){$data=[IO.File]::ReadAllBytes($pair[1]);Need-Cleanup ((Hash-Cleanup $data) -ceq $pair[2]) 'BACKUP_EVIDENCE_HOLD';$p=Join-Path (Join-Path $backup 'Evidence') $pair[0];[LimitlessTrialCleanupV1]::NewBackupFile($p,$data);Assert-BackupPath $p $false;Need-Cleanup ((Get-FileHash -LiteralPath $p -Algorithm SHA256).Hash.ToLower() -ceq $pair[2]) 'BACKUP_EVIDENCE_HOLD'}
 $p=Join-Path (Join-Path $backup 'Evidence') 'VerifiedInventory.json'
 $indexBytes=[Text.UTF8Encoding]::new($false).GetBytes(($Snapshot|ConvertTo-Json -Depth 12))
 [LimitlessTrialCleanupV1]::NewBackupFile($p,$indexBytes);Assert-BackupPath $p $false
 Need-Cleanup ((Get-FileHash -LiteralPath $p -Algorithm SHA256).Hash.ToLower() -ceq (Hash-Cleanup $indexBytes)) 'BACKUP_EVIDENCE_HOLD'
 $Report.backup_path=$backup;$Report.backup_verified_files=855
 return @{verified=$true;path=$backup}
}
$cleanupResult=[ordered]@{status='HOLD';stage='VERIFY_ADMIN';action=$Action;request_id=([guid]::NewGuid().ToString('N'));cleanup_consumed=$false;code_files_deleted=0;directories_deleted=0;account_deleted=$false;task_deletion_calls=0;install_calls=0;new_account_calls=0;host_ipc_fake_calls=0;model_calls=0;usage_reads=0;discord_calls=0;automatic_retries=0}
try{
 $id=[Security.Principal.WindowsIdentity]::GetCurrent();$admin=([Security.Principal.WindowsPrincipal]::new($id)).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
 $cleanupResult.reader=@{account=$id.Name;sid=$id.User.Value;is_admin=$admin;session_id=(Get-Process -Id $PID).SessionId}
 Need-Cleanup ($admin -and $id.User.Value -ceq $adminSid) 'FIXED_ADMIN_REQUIRED'
 if($Action -eq 'Cleanup'){Need-Cleanup ([bool]$ConfirmApprovedCleanup) 'EXPLICIT_APPROVED_CLEANUP_SWITCH_REQUIRED'}
 Need-Cleanup (-not (Test-Path -LiteralPath $backup)) 'CLEANUP_ALREADY_ATTEMPTED_OR_BACKUP_EXISTS_HOLD'
 $native=Join-Path $repo 'Tools/DiscordAIServiceDiagnostics/TrialCleanupNative.cs'
 # 별도 도구의 source도 고정 hash를 확인한 bytes만 컴파일합니다.
 $nativeBytes=[IO.File]::ReadAllBytes($native)
 Need-Cleanup ((Hash-Cleanup $nativeBytes) -ceq '1c8abecf66cacf45e4dcc1958f1ea44f4243472012f273c46e2255722d9b50f9') 'CLEANUP_NATIVE_SHA_HOLD'
 Add-Type -TypeDefinition ([Text.Encoding]::UTF8.GetString($nativeBytes))
 Need-Cleanup ([LimitlessTrialCleanupV1]::Root -ceq $root -and [LimitlessTrialCleanupV1]::Backup -ceq $backup) 'EXACT_NATIVE_ROOT_REQUIRED'
 $null=Read-CleanupPreservation
 $manifest=Read-CleanupManifest 'F:/study/codex/Project-Limitless/Temp/Session0TaskSecurityAtomicCandidate20261026' '6efbd7839bd4e452d9fbca8a465290533472e87cde71845a73bd37bdb21493ab'
 $snapshot=Read-CleanupSnapshot $manifest $cleanupResult
 Assert-CleanupSnapshot $snapshot
 $cleanupResult.preflight_fingerprint=$snapshot.fingerprint
 if($Action -eq 'Inspect'){$cleanupResult.status='CLEANUP_PREFLIGHT_PASS_NO_CHANGES'}else{
  $effects=@{
   Backup={param($s)Make-CleanupBackup $s $cleanupResult}
   ReadAgain={Read-CleanupSnapshot $manifest $cleanupResult}
   DeleteCode={param($s)
    $cleanupResult.stage='DELETE_VERIFIED_CODE_FILES'
    foreach($e in $s.inventory|Where-Object{-not $_.directory}){
     $p=Assert-CleanupPath (Join-Path $root $e.path);Assert-CleanupAcl (Get-Acl -LiteralPath $p).Sddl $false $false
     [LimitlessTrialCleanupV1]::DeletePinned($p,$e.sha256);$cleanupResult.code_files_deleted++
    }
    foreach($e in @($s.inventory|Where-Object{$_.directory}|Sort-Object {$_.path.Length} -Descending)){
     $p=if($e.path){Join-Path $root $e.path}else{$root};$p=Assert-CleanupPath $p
     Assert-CleanupAcl (Get-Acl -LiteralPath $p).Sddl $true ($e.path -eq '')
     [LimitlessTrialCleanupV1]::DeleteEmptyDirectory($p);$cleanupResult.directories_deleted++
    }
   }
   CheckAccount={
    $cleanupResult.stage='RECHECK_ACCOUNT_BEFORE_SID_DELETE'
    $current=Read-CleanupBot
    Need-Cleanup ((Convert-CleanupCanonical $current) -ceq (Convert-CleanupCanonical $snapshot.bot)) 'ACCOUNT_CHANGED_BEFORE_DELETE_HOLD'
    Assert-TrialTaskAbsent
    $r=Read-CleanupRefs $snapshot.aliases;Need-Cleanup ($r.hits -eq 0 -and $r.unknown -eq 0) 'REFERENCES_CHANGED_BEFORE_ACCOUNT_DELETE_HOLD'
    $null=Read-CleanupPreservation
   }
   DeleteAccount={
    $cleanupResult.stage='DELETE_EXACT_DISABLED_BOT_SID'
    $u=Get-LocalUser -SID ([Security.Principal.SecurityIdentifier]::new($botSid)) -ErrorAction Stop
    Need-Cleanup ($u.SID.Value -ceq $botSid -and $u.Name -ceq 'LIMITLESSBotTrial' -and $u.Enabled -eq $false) 'ACCOUNT_LAST_CHECK_HOLD'
    Remove-LocalUser -SID ([Security.Principal.SecurityIdentifier]::new($botSid)) -ErrorAction Stop
    $cleanupResult.account_deleted=$true
   }
   Final={
    $cleanupResult.stage='FINAL_READ_ONLY_VERIFY'
    Need-Cleanup (@(Get-LocalUser -ErrorAction Stop|Where-Object{$_.SID.Value -ceq $botSid -or $_.Name -ceq 'LIMITLESSBotTrial'}).Count -eq 0) 'ACCOUNT_FINAL_UNKNOWN_HOLD'
    Need-Cleanup ('LimitlessRelayTrial' -notin @(Get-ChildItem -LiteralPath 'C:\Program Files' -Name -Force -ErrorAction Stop)) 'CODE_ROOT_FINAL_UNKNOWN_HOLD'
    Assert-TrialTaskAbsent;$null=Read-CleanupPreservation
    $cleanupResult.final=@{bot_sid_absent=$true;code_root_absent=$true;trial_task_absent=$true;onboot_and_approvals_unchanged=$true;new_package_installed=$false}
   }
  }
  Invoke-CleanupCore $snapshot $effects
  $cleanupResult.status='CLEANUP_COMPLETE_NO_INSTALL';$cleanupResult.stage='COMPLETE'
 }
}catch{$cleanupResult.status='HOLD';$cleanupResult.error=Safe-CleanupError $_}
finally{
 $cleanupResult.logical_exit_code=if($cleanupResult.status -eq 'HOLD'){2}else{0}
 if($cleanupResult.cleanup_consumed){try{[LimitlessTrialCleanupV1]::NewBackupFile((Join-Path $backup ('Result-'+$cleanupResult.request_id+'.json')),[Text.UTF8Encoding]::new($false).GetBytes(($cleanupResult|ConvertTo-Json -Depth 12)))}catch{$cleanupResult.status='HOLD';$cleanupResult.stage='PROTECTED_RESULT_WRITE_HOLD';$cleanupResult.logical_exit_code=2;$cleanupResult.protected_result_write='FAILED_RECORD_IN_WORKSPACE'}}
 $folder=Join-Path $repo '문서/00_프로젝트/팀운영/Evidence/Session0TrialCleanup20261027'
 try{$null=New-Item -ItemType Directory -Path $folder -Force;$p=Join-Path $folder ($cleanupResult.request_id+'.json');$cleanupResult.receipt_path=$p;$json=$cleanupResult|ConvertTo-Json -Depth 12;[IO.File]::WriteAllText($p,$json,[Text.UTF8Encoding]::new($false));$json}catch{$cleanupResult.status='HOLD';$cleanupResult.stage='WORKSPACE_RESULT_WRITE_HOLD';$cleanupResult.logical_exit_code=2;$cleanupResult.output_error=Safe-CleanupError $_;$cleanupResult|ConvertTo-Json -Depth 12}
 $global:LASTEXITCODE=[int]$cleanupResult.logical_exit_code
}
return

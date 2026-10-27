param([ValidateSet('Inspect','Resume')][string]$Action='Inspect',[switch]$ConfirmPreserveExpiredApprovalAndInstallOnce)
# 고정 후보는 수정하지 않습니다. 암호는 기존 설치기의 상속 콘솔에서만 받습니다.
$ErrorActionPreference='Stop';Set-StrictMode -Version Latest
$session=$null;$approvalParent=$null;$phase='READ_ONLY_BOOTSTRAP'
$root='C:\ProgramData\LimitlessRelayDedicatedV2Approvals';$package='a5af3087b78b45d1baae46067d5b7ac9';$id='dedicated-install-'+$package
$bundle='C:\Program Files\LimitlessRelayDedicatedV2Staging20261027\'+$package+'\Bundle'
$pins=@{old_sha='2ee977688b2cd0d11590aff0232318f8bf936f2f1f25e3a9dace64e2669e9cdf';ms='43f6afcc91eae196e015393670cd9cd4ff37194099b313c2b901177226cd32e3';ins='83c97d781fb2d9aed797b41ea6bc799b83e939ad3ffa6f7af6a2d26f1080d68f';zip='be58f8cdbe0befdcc64cd67287865c6cd6fc2611b4a706d26cda022a8014ca25';id=$id;epoch='20261027-dedicated-'+$package}
function H([byte[]]$b){[Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($b)).ToLowerInvariant()}
try{
 $me=[Security.Principal.WindowsIdentity]::GetCurrent()
 if($PSVersionTable.PSVersion.Major -ne 7 -or -not [Environment]::Is64BitProcess -or -not ([Security.Principal.WindowsPrincipal]::new($me)).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)){throw 'R1_ADMIN_PS7_X64'}
 if('DedicatedContract' -as [type]){throw 'R1_FRESH_PROCESS_REQUIRED'}
 # Bootstrap 코드는 원본ZIP 전체 SHA와 Manifest pin으로 한정합니다.
 $zbytes=[IO.File]::ReadAllBytes('F:\study\codex\Project-Limitless\Temp\Session0DedicatedV2Candidate20261027-r1.zip');if((H $zbytes) -cne $pins.zip){throw 'R1_ZIP_PIN'}
 $mem=[IO.MemoryStream]::new($zbytes,$false);$zip=[IO.Compression.ZipArchive]::new($mem,[IO.Compression.ZipArchiveMode]::Read)
 try{
  function ReadZip([string]$name){$e=@($zip.Entries|Where-Object FullName -CEQ $name);if($e.Count -ne 1 -or $e[0].Length -gt 64MB){throw 'R1_ENTRY'};$s=$e[0].Open();$o=[IO.MemoryStream]::new();try{$s.CopyTo($o);return ,$o.ToArray()}finally{$s.Dispose();$o.Dispose()}}
  $mb=ReadZip 'HostManifest.json';if((H $mb) -cne $pins.ms){throw 'R1_MANIFEST_PIN'};$m=[Text.Encoding]::UTF8.GetString($mb)|ConvertFrom-Json -AsHashtable
  $assembly=ReadZip 'Code/V2/InstallAssembly.cs';if((H $assembly) -cne $m.files_sha256['Code/V2/InstallAssembly.cs']){throw 'R1_ASSEMBLY_PIN'}
 }finally{$zip.Dispose();$mem.Dispose()}
 Add-Type -TypeDefinition ([Text.Encoding]::UTF8.GetString($assembly))
 $phase='VERIFY_EXISTING_PROTECTED_STAGE';$fs=[V2NativeStageFs]::new();$session=[LimitlessV2Staging]::Verify($fs,$bundle,$pins.ms,$pins.ins,$pins.zip,766)
 $approvalParent=$fs.Open($root);[LimitlessV2Staging]::Protected($approvalParent.Info(),$root,$true)
 $approval=Join-Path $root ($id+'.json');$marker=Join-Path $root ('Resume-'+$pins.old_sha.Substring(0,16))
 $api=[pscustomobject]@{fs=$fs;session=$session;root=$root;approval=$approval;marker=$marker;pins=$pins;me=$me;bundle=$bundle;oldBytes=$null}
 $api|Add-Member ScriptMethod Facts {
  $h=$this.fs.Open($this.approval);try{[LimitlessV2Staging]::Protected($h.Info(),$this.approval,$false);$bytes=$h.Read();if($bytes.Length -gt 65536){throw 'R1_APPROVAL_BOUNDS'};$this.oldBytes=$bytes}finally{$h.Dispose()}
  $old=[Text.Encoding]::UTF8.GetString($bytes)|ConvertFrom-Json -AsHashtable
  $unused=(-not $this.fs.Exists((Join-Path $this.root $this.pins.id)));$fresh=$true
  foreach($p in @('C:\Program Files\LimitlessRelayDedicatedV2_20261027','C:\ProgramData\LimitlessRelayDedicatedV2_20261027',$this.marker)){if($this.fs.Exists($p)){$fresh=$false}}
  if(@(Get-LocalUser -ErrorAction Stop|Where-Object Name -In @('LIMITLESSBotV2D','LIMITLESSWorkV2D')).Count){$fresh=$false}
  $s=New-Object -ComObject Schedule.Service;$s.Connect();if(@($s.GetFolder('\').GetTasks(1)|Where-Object Name -IEQ 'LIMITLESS-DedicatedV2-20261027-Host').Count){$fresh=$false}
  $now=[datetimeoffset]::UtcNow.ToUnixTimeSeconds()
  @{admin=$true;console_input=(-not [Console]::IsInputRedirected);console_output=(-not [Console]::IsOutputRedirected);fresh_namespace=$fresh;private_approval_verified=$true;stage_verified=$true;unused=$unused;expired=([long]$old.expires_unix -le $now);same_admin=($old.admin_sid -ceq $this.me.User.Value);old=$old;old_sha=(H $bytes);admin_sid=$this.me.User.Value;now=$now}
 }
 $api|Add-Member ScriptMethod Reserve {
  [LimitlessV2Publish]::NewDirectory($this.marker,[DedicatedContract]::FsSddl('private',$true,'',''))
  $p=Join-Path $this.marker 'resume.lease';[LimitlessV2Publish]::NewFile($p,[byte[]]::new(0),[DedicatedContract]::FsSddl('private',$false,'',''))
  [IO.File]::Open($p,[IO.FileMode]::Open,[IO.FileAccess]::ReadWrite,[IO.FileShare]::None)
 }
 $api|Add-Member ScriptMethod Preserve {
  $h=$this.fs.Open($this.approval);try{[LimitlessV2Staging]::Protected($h.Info(),$this.approval,$false);if((H $h.Read()) -cne $this.pins.old_sha){throw 'R1_OLD_CHANGED'}}finally{$h.Dispose()}
  $archive=Join-Path $this.marker 'ExpiredApproval.json';[IO.File]::Move($this.approval,$archive,$false)
  [LimitlessV2Publish]::Verify($archive,[DedicatedContract]::FsSddl('private',$false,'',''));if((H ([IO.File]::ReadAllBytes($archive))) -cne $this.pins.old_sha){throw 'R1_ARCHIVE_SHA'}
 }
 $api|Add-Member ScriptMethod Publish {param($bytes)
  [LimitlessV2Publish]::NewFile($this.approval,$bytes,[DedicatedContract]::FsSddl('private',$false,'',''))
 }
 $api|Add-Member ScriptMethod Launch {
  if([Console]::IsInputRedirected -or [Console]::IsOutputRedirected){throw 'R1_INTERACTIVE_CONSOLE_REQUIRED'}
  $this.session.Recheck()
  $args='-NoLogo -NoProfile -File "'+(Join-Path $this.bundle 'Deployment/install-session0-v2.ps1')+'" -Action Install -Bundle "'+$this.bundle+'" -ManifestSha '+$this.pins.ms+' -InstallerSha '+$this.pins.ins+' -ZipSha '+$this.pins.zip+' -ApprovalPath "'+$this.approval+'" -ConfirmSystemChanges'
  # stdout/stdin을 파이프로 수집하지 않습니다. 로컬 보안 암호 입력은 이 상속 콘솔에만 남습니다.
  $child=Start-Process -FilePath 'C:\Program Files\PowerShell\7\pwsh.exe' -ArgumentList $args -NoNewWindow -Wait -PassThru
  $child.ExitCode
 }
 $api|Add-Member ScriptMethod Recheck {$this.session.Recheck()}
 $api|Add-Member ScriptMethod Record {param($result)
  [LimitlessV2Publish]::NewFile((Join-Path $this.marker 'ResumeOutcome.json'),[Text.UTF8Encoding]::new($false).GetBytes(($result|ConvertTo-Json -Depth 12)),[DedicatedContract]::FsSddl('private',$false,'',''))
 }
 # ScriptBlock 메모리 실행에서도 PSScriptRoot에 의존하지 않도록 고정 절대 소스 경로를 사용합니다.
 $corePath='F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\Tools\DiscordAIServiceDiagnostics\R1Preparation\ResumeCore.ps1'
 $core=[IO.File]::ReadAllBytes($corePath);if((H $core) -cne '01fa6f5f51f5a3a269d149be74ccb540ab0422a18ad8b1089490a6af6bf57785'){throw 'R1_RESUME_CORE_PIN'}
 . ([scriptblock]::Create([Text.Encoding]::UTF8.GetString($core)))
 if($Action -eq 'Inspect'){$f=$api.Facts();$view=@{};foreach($key in @('admin','console_input','console_output','fresh_namespace','unused','expired','same_admin','old_sha')){$view[$key]=$f[$key]};@{schema=3;status='READONLY_RECOVERY_INSPECTION';facts=$view;changes=0}|ConvertTo-Json -Depth 5}
 else{
  if(-not $ConfirmPreserveExpiredApprovalAndInstallOnce){throw 'R1_EXPLICIT_MANUAL_RESUME_REQUIRED'}
  $phase='EXPLICIT_ONE_RESUME';Invoke-R1ConsoleResume $api $pins|ConvertTo-Json -Depth 12
 }
}catch{@{schema=3;kind='R1_CONSOLE_RESUME_BOOTSTRAP';status='HOLD';phase=$phase;errors=@{type=$_.Exception.GetType().FullName;hresult=$_.Exception.HResult};automatic_retries=0;partial_assets_preserve=$true}|ConvertTo-Json -Depth 8}
finally{foreach($handle in @($approvalParent,$session)){if($handle){try{$handle.Dispose()}catch{@{schema=3;kind='R1_RESUME_CLOSE';status='HOLD';errors=@{type=$_.Exception.GetType().FullName;hresult=$_.Exception.HResult};automatic_retries=0}|ConvertTo-Json -Depth 5}}}}

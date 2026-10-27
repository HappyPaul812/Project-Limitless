param([switch]$ConfirmApprovedReadOnly)
$ErrorActionPreference='Stop';Set-StrictMode -Version Latest
if(-not $ConfirmApprovedReadOnly){throw 'SEPARATE_ADMIN_READONLY_APPROVAL_REQUIRED'}
if($PSVersionTable.PSVersion.Major -ne 7 -or -not [Environment]::Is64BitProcess){throw 'PS7_X64_REQUIRED'}
$identity=[Security.Principal.WindowsIdentity]::GetCurrent();if(-not ([Security.Principal.WindowsPrincipal]::new($identity)).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)){throw 'ADMIN_READONLY_REQUIRED'}
$root='F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree'
function Get-R2TaskSha([byte[]]$Bytes){[Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($Bytes)).ToLowerInvariant()}
function Read-R2TaskPinned([string]$Path,[string]$Sha){$bytes=[IO.File]::ReadAllBytes($Path);if((Get-R2TaskSha $bytes) -cne $Sha){throw 'READONLY_SOURCE_SHA_HOLD'};return ,$bytes}
$cs=Read-R2TaskPinned (Join-Path $root 'Tools/DiscordAIServiceDiagnostics/TaskSecurityCandidate/TaskReadSecurity.cs') '5cca69ee5991e6f20d58d3325c67eb102e94a9b92464884d0d3b615df696b103'
if('Limitless.TaskSecurityCandidate.DedicatedTaskReadContract' -as [type]){throw 'FRESH_PROCESS_REQUIRED'}
Add-Type -TypeDefinition ([Text.Encoding]::UTF8.GetString($cs))
$snapshot=Read-R2TaskPinned (Join-Path $root '문서/00_프로젝트/팀운영/Evidence/DedicatedV2R2Readonly20261027/Admin-33a67728035f4fe88db7f539e5b648dc.json') '4de232b3d5af7aa6662ce4e4ec6a4d28ddc529b5c0d50656a9114f71e679140e'
$evidence=[Limitless.TaskSecurityCandidate.DedicatedTaskReadContract]::FromPinnedSnapshot($snapshot)
$core=Read-R2TaskPinned (Join-Path $root 'Tools/DiscordAIServiceDiagnostics/TaskSecurityReadback/ReadbackCore.ps1') '3f29d411c2aa2453c02c5a9ef5c77aeb9b52b29c31c60bd3e09f627b5a0e0b0f'
. ([scriptblock]::Create([Text.Encoding]::UTF8.GetString($core)))
$old=[Text.Encoding]::UTF8.GetString($snapshot)|ConvertFrom-Json -AsHashtable
$backend=[pscustomobject]@{scheduler=$null;folder=$null;old=$old;code='C:\Program Files\LimitlessRelayDedicatedV2_20261027';worker_sid='S-1-5-21-2858504392-1958168942-1662906075-1010'}
$backend|Add-Member ScriptMethod File {param($Path,$Sha)
 $p=$Path;while($p){$i=Get-Item -LiteralPath $p -Force -ErrorAction Stop;if($i.Attributes -band [IO.FileAttributes]::ReparsePoint){throw 'READBACK_REPARSE_HOLD'};$parent=Split-Path $p -Parent;if($parent -ceq $p){break};$p=$parent}
 $bytes=Read-R2TaskPinned $Path $Sha;$acl=(Get-Acl -LiteralPath $Path -ErrorAction Stop).Sddl
 $stage='FILESYSTEM_'+$Path
 if($Path -ceq (Join-Path $this.code 'HostManifest.json')){$stage='INSTALLED_MANIFEST'}
 if($Path -ceq (Join-Path $this.code 'Code/V2/start-host-v2.ps1')){$stage='TASK_ACTION_PIN_COMPARE_NO_ARGUMENT_OUTPUT'}
 $matches=@($this.old.queries|Where-Object {$_.stage -ceq $stage})
 if($matches.Count -ne 1 -or $matches[0].status -cne 'READ_OK'){throw 'READBACK_FILE_BASELINE_MISSING_OR_UNKNOWN'}
 $expected=if($stage -ceq 'TASK_ACTION_PIN_COMPARE_NO_ARGUMENT_OUTPUT'){$matches[0].value.host_metadata}else{$matches[0].value}
 if(-not $expected){throw 'READBACK_FILE_BASELINE_MISSING'}
 $sd=[Security.AccessControl.RawSecurityDescriptor]::new($acl);if($sd.GetSddlForm([Security.AccessControl.AccessControlSections]14) -cne $expected.sddl){throw 'READBACK_FILE_ACL_CHANGED'}
 @{sha256=$Sha;sddl=$sd.GetSddlForm([Security.AccessControl.AccessControlSections]14);bytes=$bytes}
}
$backend|Add-Member ScriptMethod Prepare {
 $binding=$this.File((Join-Path $this.code 'InstalledBinding.json'),'d698ae90f0909a9363b04dea67c186f95bfa5f7f2f5d937adf3ae7a23aa011c9')
 $manifest=$this.File((Join-Path $this.code 'HostManifest.json'),'7c8410145ed84f06fc9901e93f78e18d0cc103ad89757172e08c84f91d8a3976')
 $hostFile=$this.File((Join-Path $this.code 'Code/V2/start-host-v2.ps1'),'063c063223f8989605b1031ea0978d890b43ee95d4787e8ad578b923ba361f46')
 $b=[Text.Encoding]::UTF8.GetString($binding.bytes)|ConvertFrom-Json -AsHashtable
 if($b.package_id -cne '3effaac30bf44760986fee9e6019b283' -or $b.worker_sid -cne $this.worker_sid -or $b.bot_sid -cne 'S-1-5-21-2858504392-1958168942-1662906075-1009' -or $b.manifest_sha256 -cne $manifest.sha256){throw 'READBACK_BINDING_HOLD'}
 $u=Get-LocalUser -Name LIMITLESSWorkV2D -ErrorAction Stop;if($u.SID.Value -cne $this.worker_sid -or -not $u.Enabled -or $u.AccountExpires -le (Get-Date)){throw 'READBACK_WORKER_ID_OR_EXPIRY_HOLD'}
 $parts=@($binding.sha256,$binding.sddl,$manifest.sha256,$manifest.sddl,$hostFile.sha256,$hostFile.sddl,$u.SID.Value,$u.Enabled)
 @{verified=$true;fingerprint=(Get-R2TaskSha ([Text.Encoding]::UTF8.GetBytes(($parts -join '|'))));binding_sha256=$binding.sha256;manifest_sha256=$manifest.sha256;host_sha256=$hostFile.sha256;worker_sid=$u.SID.Value;worker_enabled=$u.Enabled}
}
$backend|Add-Member ScriptMethod Connect {$this.scheduler=New-Object -ComObject Schedule.Service;$this.scheduler.Connect();$this.folder=$this.scheduler.GetFolder('\')}
$backend|Add-Member ScriptMethod Read {
 $t=$this.folder.GetTask('LIMITLESS-DedicatedV2-20261027-Host');$sd=$t.GetSecurityDescriptor(7);$d=$t.Definition
 if($d.Principal.UserId -like 'S-1-*'){$sid=$d.Principal.UserId}else{$sid=([Security.Principal.NTAccount]::new($d.Principal.UserId)).Translate([Security.Principal.SecurityIdentifier]).Value}
 $actionMatch=$false;if($d.Actions.Count -eq 1){$a=$d.Actions.Item(1);$expected='-NoProfile -NonInteractive -File "'+(Join-Path $this.code 'Code/V2/start-host-v2.ps1')+'" -ManifestSha 7c8410145ed84f06fc9901e93f78e18d0cc103ad89757172e08c84f91d8a3976 -BindingSha d698ae90f0909a9363b04dea67c186f95bfa5f7f2f5d937adf3ae7a23aa011c9';$actionMatch=$a.Path -ceq 'C:\Program Files\PowerShell\7\pwsh.exe' -and $a.WorkingDirectory -ceq $this.code -and $a.Arguments -ceq $expected}
 $xmlSha=Get-R2TaskSha ([Text.Encoding]::UTF8.GetBytes([string]$t.Xml))
 $facts=@{path_match=($t.Path -ceq '\LIMITLESS-DedicatedV2-20261027-Host');principal_sid_match=($sid -ceq $this.worker_sid -and $d.Principal.LogonType -eq 1 -and $d.Principal.RunLevel -eq 0 -and [string]::IsNullOrEmpty($d.Principal.GroupId));disabled=(-not $t.Enabled);trigger0=($d.Triggers.Count -eq 0);state_disabled=($t.State -eq 1);policy_settings_match=($d.Settings.RestartCount -eq 0 -and $d.Settings.MultipleInstances -eq 2 -and $d.Settings.AllowHardTerminate -and [Xml.XmlConvert]::ToTimeSpan($d.Settings.ExecutionTimeLimit).TotalSeconds -eq 90);action_match=[bool]$actionMatch;xml_baseline_match=($xmlSha -ceq '4ef912d925510838b18765f6b18204c7da83de4b5d0b182381eaf36dde89a47b');sddl=$sd;task_path=$t.Path;principal_sid=$sid;xml_sha256=$xmlSha}
 $stable=@($facts.Keys|Sort-Object|ForEach-Object {$_+'='+[string]$facts[$_]}) -join '|'
 $facts.fingerprint=Get-R2TaskSha ([Text.Encoding]::UTF8.GetBytes($stable));$facts
}
$result=Invoke-R2TaskSecurityReadback $backend $evidence
$result.collected_utc=[datetime]::UtcNow.ToString('o');$result.v1_status='REFERENCE_VERIFICATION_HOLD';$result.runtime_status='HOLD_NEW_TOKEN_GROUP_EXCLUSIVITY_NOT_VERIFIED'
$output=Join-Path $root ('문서/00_프로젝트/팀운영/Evidence/TaskSecurityReadback20261027/Admin-'+[guid]::NewGuid().ToString('N')+'.json')
$result.result_path=$output;$bytes=[Text.UTF8Encoding]::new($false).GetBytes(($result|ConvertTo-Json -Depth 16));$f=[IO.File]::Open($output,[IO.FileMode]::CreateNew,[IO.FileAccess]::Write,[IO.FileShare]::None);try{$f.Write($bytes)}finally{$f.Dispose()}
@{status=$result.status;descriptor_readback_status=$result.descriptor_readback_status;effective_worker_access=$result.effective_worker_access;result_path=$output;changes=0;install_calls=0;ipc_calls=0;model_calls=0}|ConvertTo-Json

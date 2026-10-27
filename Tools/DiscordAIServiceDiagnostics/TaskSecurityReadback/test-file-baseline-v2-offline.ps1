param()
$ErrorActionPreference='Stop';Set-StrictMode -Version Latest
$root='F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree'
$dir=Join-Path $root 'Tools/DiscordAIServiceDiagnostics/TaskSecurityReadback'
$snapshot=[IO.File]::ReadAllBytes((Join-Path $root '문서/00_프로젝트/팀운영/Evidence/DedicatedV2R2Readonly20261027/Admin-33a67728035f4fe88db7f539e5b648dc.json'))
$sha=[Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($snapshot)).ToLowerInvariant()
if($sha -cne '4de232b3d5af7aa6662ce4e4ec6a4d28ddc529b5c0d50656a9114f71e679140e'){throw 'SNAPSHOT_SHA_HOLD'}
$old=[Text.Encoding]::UTF8.GetString($snapshot)|ConvertFrom-Json -AsHashtable
$original=[IO.File]::ReadAllText((Join-Path $dir 'probe-task-security-readonly.ps1'))
$updated=[IO.File]::ReadAllText((Join-Path $dir 'probe-task-security-readonly-v2.ps1'))
function New-FileAdapter([string]$Source){$begin=$Source.IndexOf('$backend=[pscustomobject]');$end=$Source.IndexOf('$result=Invoke-R2TaskSecurityReadback');$block=$Source.Substring($begin,$end-$begin);. ([scriptblock]::Create($block));return $backend}
# 실제 File ScriptMethod는 그대로 실행하며 파일시스템/바이트 읽기 함수만 가짜로 제공합니다.
function Get-Item {param($LiteralPath,[switch]$Force,$ErrorAction)[pscustomobject]@{Attributes=[IO.FileAttributes]::Normal}}
function Get-Acl {param($LiteralPath,$ErrorAction)$stage='FILESYSTEM_'+$LiteralPath;if($LiteralPath.EndsWith('HostManifest.json')){$stage='INSTALLED_MANIFEST'};if($LiteralPath.EndsWith('start-host-v2.ps1')){$stage='TASK_ACTION_PIN_COMPARE_NO_ARGUMENT_OUTPUT'};$row=@($old.queries|Where-Object stage -CEQ $stage)[0];$value=if($stage -eq 'TASK_ACTION_PIN_COMPARE_NO_ARGUMENT_OUTPUT'){$row.value.host_metadata}else{$row.value};[pscustomobject]@{Sddl=$value.sddl}}
function Read-R2TaskPinned {param($Path,$Sha)return ,[byte[]]@(1,2,3)}
$legacy=New-FileAdapter $original;$fixed=New-FileAdapter $updated
$script:rows=@()
function Check-FileBaseline($Name,[scriptblock]$Test){try{if(-not (& $Test)){throw 'ASSERT'};$script:rows+=@{name=$Name;status='PASS'}}catch{$script:rows+=@{name=$Name;status='FAIL';type=$_.Exception.GetType().FullName;hresult=$_.Exception.HResult}}}
Check-FileBaseline 'LEGACY_MANIFEST_PROPERTY_EXCEPTION_REPRODUCED' {try{$legacy.File((Join-Path $legacy.code 'HostManifest.json'),'FAKE')|Out-Null;$false}catch{$e=$_.Exception;while($e.InnerException){$e=$e.InnerException};$e -is [Management.Automation.PropertyNotFoundException]}}
Check-FileBaseline 'LEGACY_HOST_PROPERTY_EXCEPTION_REPRODUCED' {try{$legacy.File((Join-Path $legacy.code 'Code/V2/start-host-v2.ps1'),'FAKE')|Out-Null;$false}catch{$e=$_.Exception;while($e.InnerException){$e=$e.InnerException};$e -is [Management.Automation.PropertyNotFoundException]}}
foreach($fileName in @('InstalledBinding.json','HostManifest.json','Code/V2/start-host-v2.ps1')){Check-FileBaseline ('ACTUAL_FIXED_FILE_METHOD_'+$fileName) {$value=$fixed.File((Join-Path $fixed.code $fileName),'FAKE');$value.sha256 -ceq 'FAKE' -and $value.bytes.Length -eq 3 -and $value.sddl.StartsWith('O:BAG:SYD:P')}}
Check-FileBaseline 'MISSING_BASELINE_REJECTED' {$saved=$fixed.old;$fixed.old=@{queries=@()};try{$fixed.File((Join-Path $fixed.code 'HostManifest.json'),'FAKE')|Out-Null;$false}catch{$true}finally{$fixed.old=$saved}}
Check-FileBaseline 'DUPLICATE_BASELINE_REJECTED' {$saved=$fixed.old;$row=@($saved.queries|Where-Object stage -CEQ INSTALLED_MANIFEST)[0];$fixed.old=@{queries=@($row,$row)};try{$fixed.File((Join-Path $fixed.code 'HostManifest.json'),'FAKE')|Out-Null;$false}catch{$true}finally{$fixed.old=$saved}}
Check-FileBaseline 'UNKNOWN_BASELINE_REJECTED' {$saved=$fixed.old;$fixed.old=@{queries=@(@{stage='INSTALLED_MANIFEST';status='UNKNOWN'})};try{$fixed.File((Join-Path $fixed.code 'HostManifest.json'),'FAKE')|Out-Null;$false}catch{$true}finally{$fixed.old=$saved}}
Check-FileBaseline 'ENTRY_SYNTAX_VALID' {$t=$null;$e=$null;[void][Management.Automation.Language.Parser]::ParseInput($updated,[ref]$t,[ref]$e);$e.Count -eq 0}
@{kind='TASK_READBACK_FILE_BASELINE_V2_REGRESSION';pass=@($script:rows|Where-Object status -EQ PASS).Count;fail=@($script:rows|Where-Object status -EQ FAIL).Count;tests=$script:rows;admin_queries=0;changes=0;install_calls=0;ipc_calls=0;existing_48_repeated=$false;existing_22_repeated=$false}|ConvertTo-Json -Depth 8

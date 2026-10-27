param([Parameter(Mandatory)][string]$Candidate,[Parameter(Mandatory)][string]$Output)
$ErrorActionPreference='Stop'
$raw=[IO.File]::ReadAllBytes((Join-Path $Candidate 'HostManifest.json'));$m=[Text.Encoding]::UTF8.GetString($raw)|ConvertFrom-Json -AsHashtable
$payloads=@{};foreach($n in $m.files_sha256.Keys){$b=[IO.File]::ReadAllBytes((Join-Path $Candidate $n));$hash=[Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($b)).ToLowerInvariant();if($hash -cne $m.files_sha256[$n]){throw 'V2_PACKAGE_BYTES_HOLD'};$payloads[$n]=$b}
$inspect=& (Join-Path $Candidate 'Deployment/run-session0-v2-install.ps1') -Action Inspect -Bundle $Candidate
if((($inspect -join "`n")|ConvertFrom-Json).status -cne 'V2_PACKAGE_INSPECT_PASS_NOT_INSTALLED'){throw 'V2_PACKAGE_INSPECT_HOLD'}
# 실제 설치기 bytes를 DefinitionsOnly로만 로드합니다. 계정·파일·Task·권한 API는 호출하지 않습니다.
$installer=[scriptblock]::Create([Text.Encoding]::UTF8.GetString($payloads['Deployment/install-session0-v2.ps1']))
& $installer -DefinitionsOnly -VerifiedManifestBytes $raw -VerifiedPayloads $payloads
[LimitlessV2Contract]::Manifest([Text.Encoding]::UTF8.GetString($raw))
$sealedInspect=& $installer -Action Inspect -Bundle 'F:\SYNTHETIC_MUST_NOT_READ_MUTABLE_SOURCE' -VerifiedManifestBytes $raw -VerifiedPayloads $payloads
if((($sealedInspect -join "`n")|ConvertFrom-Json).status -cne 'V2_CANDIDATE_INSPECT_ONLY'){throw 'V2_MEMORY_PAYLOAD_RELOAD_HOLD'}
$cases=@(@{name='valid';field=$null;value=$null;expected=$true},@{name='schema1';field='schema';value=1;expected=$false},@{name='v1root';field='code_root';value='C:\Program Files\LimitlessRelayTrial';expected=$false},@{name='v1pipe';field='pipe_name';value=('LimitlessPlanning-'+$m.package_id);expected=$false},@{name='approved';field='approved';value=$true;expected=$false},@{name='auto_start';field='auto_start';value=$true;expected=$false},@{name='over90';field='max_seconds';value=91;expected=$false},@{name='forged_sid';field='bot_sid';value='FORGED';expected=$false},@{name='duplicate_key';field=$null;value=$null;expected=$false})
$results=@()
foreach($case in $cases){
 $copy=([Text.Encoding]::UTF8.GetString($raw)|ConvertFrom-Json -AsHashtable)
 if($case.field){$copy[$case.field]=$case.value};$value=$copy|ConvertTo-Json -Depth 12 -Compress
 if($case.name -eq 'duplicate_key'){$value=$value.TrimEnd('}')+',"schema":2}'}
 $accepted=$true;try{[LimitlessV2Contract]::Manifest($value)}catch{$accepted=$false}
 if($accepted -ne $case.expected){throw 'V2_CROSS_CONTRACT_HOLD'}
 $results+=@{name=$case.name;accepted=$accepted;expected=$case.expected;pass=$true}
}
$summary=@{status='PACKAGE_OFFLINE_PASS_NOT_INSTALLED';payload_files=$payloads.Count;zip_entries=$payloads.Count+1;manifest_sha256=([Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($raw)).ToLowerInvariant());verified_memory_bootstrap=$true;mutable_source_not_reloaded=$true;cross_contract_cases=$results;actual_windows_calls=0;actual_ipc_calls=0;actual_workers=0;actual_installs=0}
$summary|ConvertTo-Json -Depth 8|Set-Content -LiteralPath $Output -Encoding utf8
'PASS package bytes/ZIP/Inspect/verified-memory bootstrap; cross-contract9; Windows calls0'

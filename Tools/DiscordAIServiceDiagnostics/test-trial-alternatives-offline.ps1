#requires -Version 7.0
param()
$ErrorActionPreference='Stop'
# 855개 실제 파일을 다시 스캔하지 않고 이미 보존한 승인 Manifest의 항목을 가짜 상태로 사용합니다.
$baseline=Join-Path $PSScriptRoot '../../문서/00_프로젝트/팀운영/Evidence/Session0LsaRepair20261026/BaselineInventory.json'
if((Get-FileHash -LiteralPath $baseline -Algorithm SHA256).Hash -ne 'C2D398BE42D7A12FB4574BFF5E2A8820271EC82E355993F24E017A6E706F16D3'){throw 'BASELINE_INVENTORY_SHA_HOLD'}
$inventory=Get-Content -LiteralPath $baseline -Raw|ConvertFrom-Json -AsHashtable
Add-Type -Path @((Join-Path $PSScriptRoot 'TrialAlternativesOfflineModel.cs'),(Join-Path $PSScriptRoot 'TrialAlternativesOfflineTests.cs'))
$files=[Collections.Generic.List[LimitlessTrialAlternativesOffline.FileEvidence]]::new()
foreach($name in @($inventory.Keys|Sort-Object)){
 if($name -eq 'HostManifest.json'){continue}
 $f=[LimitlessTrialAlternativesOffline.FileEvidence]::new()
 $f.Relative=$name;$f.ExpectedSha=$inventory[$name];$f.ObservedSha=$inventory[$name];$files.Add($f)
}
$lines=[LimitlessTrialAlternativesOffline.AlternativeTests]::Run($files.ToArray())
$lines
$failed=@($lines|Where-Object{$_ -like 'FAIL *'}).Count
@{passed=@($lines|Where-Object{$_ -like 'PASS *'}).Count;failed=$failed;baseline_entries=$files.Count;actual_rm_calls=0;actual_scheduler_calls=0;actual_worker_calls=0;actual_account_calls=0;system_changes=0;model_only=$true}|ConvertTo-Json -Compress
if($failed){throw 'ALTERNATIVES_OFFLINE_TEST_HOLD'}

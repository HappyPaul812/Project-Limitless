param([ValidateSet('Inspect','Install')][string]$Action='Inspect',[string]$Bundle,[string]$ApprovalPath,[string]$ZipSha,[switch]$ConfirmSystemChanges,[switch]$DefinitionsOnly,[byte[]]$VerifiedManifestBytes,[hashtable]$VerifiedPayloads)
$ErrorActionPreference='Stop';$protectedStage=$null
if($Action -eq 'Install' -and (-not $VerifiedManifestBytes -or -not $VerifiedPayloads)){throw 'V2_VERIFIED_MEMORY_ENTRY_ONLY'}
$support=if($VerifiedPayloads){''}elseif(Test-Path -LiteralPath (Join-Path $PSScriptRoot 'V2Contract.cs')){$PSScriptRoot}else{Join-Path $PSScriptRoot '../Code/V2'}
if($VerifiedPayloads){
 $policyHash=[Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($VerifiedPayloads['Code/V2/TrialAccountPolicy.cs']))
 if($policyHash -cne 'D9A0BC50137119BC28C17C5496B5FC20B4DA81925B806BC472A538C4415F850B'){throw 'V2_UTF16_POLICY_PIN_HOLD'}
 if(-not ('LimitlessV2Contract' -as [type])){Add-Type -TypeDefinition ([Text.Encoding]::UTF8.GetString($VerifiedPayloads['Code/V2/InstallAssembly.cs']))}
}else{
 $policy=if(Test-Path -LiteralPath (Join-Path $support 'TrialAccountPolicy.cs')){Join-Path $support 'TrialAccountPolicy.cs'}else{Join-Path $support '../TrialAccountPolicy.cs'}
 if((Get-FileHash -LiteralPath $policy).Hash -cne 'D9A0BC50137119BC28C17C5496B5FC20B4DA81925B806BC472A538C4415F850B'){throw 'V2_UTF16_POLICY_PIN_HOLD'}
 if(-not ('LimitlessV2Contract' -as [type])){Add-Type -Path @((Join-Path $support 'V2Contract.cs'),(Join-Path $support 'V2Publish.cs'),(Join-Path $support 'V2PolicyReadback.cs'),(Join-Path $support 'V2Staging.cs'),$policy)}
}
if(-not ('LimitlessV2Publish' -as [type])){throw 'V2_FRESH_POWERSHELL_REQUIRED'}
if($VerifiedPayloads){. ([scriptblock]::Create([Text.Encoding]::UTF8.GetString($VerifiedPayloads['Code/V2/V2InstallCore.ps1'])))}else{. (Join-Path $support 'V2InstallCore.ps1')}
if($DefinitionsOnly){return}
if(-not $Bundle){throw 'V2_BUNDLE_REQUIRED'}
[byte[]]$manifestBytes=if($VerifiedManifestBytes){$VerifiedManifestBytes}else{[IO.File]::ReadAllBytes((Join-Path $Bundle 'HostManifest.json'))}
$raw=[Text.Encoding]::UTF8.GetString($manifestBytes);[LimitlessV2Contract]::Manifest($raw);$m=$raw|ConvertFrom-Json -AsHashtable
$payloads=@{}
foreach($n in $m.files_sha256.Keys){
 [LimitlessV2Contract]::Relative($n)
 # if의 pipeline 반환으로 빈 byte[]가 null로 바뀌지 않게 직접 대입합니다.
 if($VerifiedPayloads){$payloads[$n]=$VerifiedPayloads[$n]}else{$payloads[$n]=[IO.File]::ReadAllBytes((Join-Path $Bundle $n))}
 if((Get-V2Hash $payloads[$n]) -cne $m.files_sha256[$n]){throw 'V2_PACKAGE_FILE_SHA_HOLD'}
}
if($Action -eq 'Inspect'){@{status='V2_CANDIDATE_INSPECT_ONLY';approved=$false;files=$payloads.Count;manifest_sha256=(Get-V2Hash $manifestBytes);system_changes=0}|ConvertTo-Json;return}
if(-not $ConfirmSystemChanges){throw 'V2_SEPARATE_INSTALL_APPROVAL_REQUIRED'}
$expectedApproval=Join-Path ([LimitlessV2Contract]::ApprovalRoot) ($m.approval_id+'.json')
if([IO.Path]::GetFullPath($ApprovalPath) -cne $expectedApproval){throw 'V2_FIXED_APPROVAL_PATH_HOLD'}
[LimitlessV2Publish]::Verify($ApprovalPath,[LimitlessV2Contract]::FsSddl('private',$false,'',''))
$approval=[IO.File]::ReadAllText($ApprovalPath)
# 암호는 별도 실설치 승인 이후 로컬 보안 입력만 받습니다. 비밀을 JSON/예외에 넣지 않습니다.
try{
 # 승인 bytes와 보호 staging의 native readback을 연결하고 handle을 설치 종료까지 유지합니다.
 $protectedStage=[LimitlessV2Staging]::Verify([V2NativeStageFs]::new(),$Bundle,(Get-V2Hash $manifestBytes),$m.files_sha256['Deployment/install-session0-v2.ps1'],$ZipSha,$payloads.Count)
 foreach($n in $payloads.Keys){if((Get-V2Hash $protectedStage.Bytes[$n]) -cne (Get-V2Hash $payloads[$n])){throw 'V2_STAGING_MEMORY_MISMATCH'}}
 $protectedStage.Recheck()
 $password=Read-Host '새 V2 시험 계정 암호 (로컬 입력만)' -AsSecureString
 $credential=Get-Credential -UserName 'PAUL-PC\LIMITLESSRelaySvc' -Message '기존 Worker 인증은 복사하지 않으며 로컬 입력만 사용합니다.'
 . ([scriptblock]::Create([Text.Encoding]::UTF8.GetString($VerifiedPayloads['Code/V2/V2WorkerQuery.ps1'])))
 . ([scriptblock]::Create([Text.Encoding]::UTF8.GetString($VerifiedPayloads['Code/V2/V2WindowsAdapter.ps1'])))
 $api=New-V2WindowsAdapter $Bundle $password $credential $VerifiedPayloads
 $coreResult=Invoke-V2InstallCore $api $manifestBytes $payloads $approval $ZipSha
 $protectedStage.Recheck()
 $coreResult|ConvertTo-Json -Depth 12
}finally{$password=$null;$credential=$null;$payloads=$null;if($protectedStage){$protectedStage.Dispose()}}
# exit를 호출하지 않아 부모 대화형 PowerShell 창을 종료하지 않습니다.

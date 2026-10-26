param([ValidateSet('Preflight')][string]$Action='Preflight')
# 읽기 전용입니다. 작업 생성 시점 DACL 검증이 충족되기 전에는 설치 기능을 제공하지 않습니다.
$ErrorActionPreference='Stop'
$bundle='F:/study/codex/Project-Limitless/Temp/Session0HostBundle20261026Ready'
$receipt=@{status='HOLD';stage='CANDIDATE_PREFLIGHT';model_calls=0;official_usage_reads=0;discord_operations=0;fake_worker_calls=0;system_changes=0;new_install_attempted=$false;real_ipc_verified=$false}
try {
 $m=Get-FileHash -LiteralPath (Join-Path $bundle 'HostManifest.json') -Algorithm SHA256
 $i=Get-FileHash -LiteralPath (Join-Path $bundle 'Deployment/install-session0-trial.ps1') -Algorithm SHA256
 if($m.Hash.ToLower() -cne '9aa4a60d6dd57c320f2d268ef96dcf86694e53ca9dfdcbb6a3e97d1f33dfe7ec' -or $i.Hash.ToLower() -cne '337ad613ef6471ae6f4058043965bb3b812a0e6b31adc05ce1efb61ce0158829'){throw 'APPROVED_SHA_HOLD'}
 $config=Get-Content -LiteralPath (Join-Path $bundle 'HostManifest.json') -Raw|ConvertFrom-Json
 $entries=@($config.files_sha256.PSObject.Properties)
 if($entries.Count -ne 855 -or @(Get-ChildItem -LiteralPath $bundle -File -Recurse -Force).Count -ne 856){throw 'EXACT855_HOLD'}
 foreach($entry in $entries){
  if($entry.Name -notmatch '^(Code|Runtime|Legacy|Deployment)/' -or $entry.Name.Contains('..') -or $entry.Name.Contains(':') -or $entry.Name.Contains([char]92)){throw 'FILE_SCOPE_HOLD'}
  if((Get-FileHash -LiteralPath (Join-Path $bundle $entry.Name) -Algorithm SHA256).Hash.ToLower() -cne $entry.Value){throw 'FILE_SHA_HOLD'}
 }
 $source=[IO.File]::ReadAllText((Join-Path $bundle 'Deployment/install-session0-trial.ps1'))
 $receipt.manifest_sha_match=$true;$receipt.installer_sha_match=$true;$receipt.exact_files_verified=855
 $receipt.post_create_dacl=($source.IndexOf('$null=Register-ScheduledTask') -ge 0 -and $source.IndexOf('$registered.SetSecurityDescriptor') -gt $source.IndexOf('$null=Register-ScheduledTask'))
 $receipt.task_security_descriptor_readback=$source.Contains('.GetSecurityDescriptor(')
 $receipt.status='HOLD_TASK_DACL_AT_REGISTRATION_NOT_VERIFIED';$receipt.stage='SECURITY_PREFLIGHT';$receipt.installation_blocked=$true
} catch {$receipt.error_type=$_.Exception.GetType().Name;$receipt.hresult=$_.Exception.HResult}
$receipt|ConvertTo-Json -Depth 5

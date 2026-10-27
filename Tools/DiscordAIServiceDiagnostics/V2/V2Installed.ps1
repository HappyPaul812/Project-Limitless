function Read-V2Installed([string]$ManifestSha,[string]$BindingSha,[switch]$AllRuntime){
 if($PSScriptRoot -cne (Join-Path ([LimitlessV2Contract]::CodeRoot) 'Code/V2')){throw 'V2_FIXED_INSTALLED_PATH_HOLD'}
 $root=[LimitlessV2Contract]::CodeRoot
 $manifestBytes=[IO.File]::ReadAllBytes((Join-Path $root 'HostManifest.json'));$bindingBytes=[IO.File]::ReadAllBytes((Join-Path $root 'InstalledBinding.json'))
 if((Get-V2Hash $manifestBytes) -cne $ManifestSha -or (Get-V2Hash $bindingBytes) -cne $BindingSha){throw 'V2_START_HASH_HOLD'}
 $raw=[Text.Encoding]::UTF8.GetString($manifestBytes);$br=[Text.Encoding]::UTF8.GetString($bindingBytes)
 [LimitlessV2Contract]::Binding($br,$raw,$ManifestSha);$m=$raw|ConvertFrom-Json -AsHashtable;$b=$br|ConvertFrom-Json -AsHashtable
 [LimitlessSession0PipeV2]::VerifyProtectedPath($root,$false,$b.worker_sid)
 foreach($n in $m.files_sha256.Keys){
  if(-not $AllRuntime -and $n.StartsWith('Runtime/')){continue}
  $p=Join-Path $root $n;[LimitlessSession0PipeV2]::VerifyProtectedPath($p,$false,$b.worker_sid)
  if((Get-FileHash -LiteralPath $p).Hash.ToLowerInvariant() -cne $m.files_sha256[$n]){throw 'V2_INSTALLED_PAYLOAD_HOLD'}
 }
 [LimitlessV2Publish]::Verify((Join-Path $root 'InstalledBinding.json'),[LimitlessV2Contract]::FsSddl('code',$false,$b.worker_sid,$b.bot_sid))
 return @{manifest=$m;binding=$b}
}

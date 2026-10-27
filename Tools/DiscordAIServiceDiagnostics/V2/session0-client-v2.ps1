param([Parameter(Mandatory)][ValidatePattern('^LimitlessPlanningV2-[a-f0-9]{32}-(arm|work|control)$')][string]$PipeName,[Parameter(Mandatory)][ValidatePattern('^[a-f0-9]{64}$')][string]$ManifestSha,[Parameter(Mandatory)][ValidatePattern('^[a-f0-9]{64}$')][string]$BindingSha)
$ErrorActionPreference='Stop';$pipe=$null;$cancel=$null
try{
 # C# 컴파일 전에 bootstrap 자체와 읽을 코드의 소유자/쓰기권한/핀을 확인합니다.
 $fixed='C:\Program Files\LimitlessRelayTrialV2_20261027'
 if($PSScriptRoot -cne ($fixed+'\Code\V2')){throw 'V2_BOOTSTRAP_LOCATION_HOLD'}
 $mr=[IO.File]::ReadAllBytes((Join-Path $fixed 'HostManifest.json'));$br=[IO.File]::ReadAllBytes((Join-Path $fixed 'InstalledBinding.json'))
 function Bootstrap-V2Hash([byte[]]$v){[Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($v)).ToLowerInvariant()}
 if((Bootstrap-V2Hash $mr) -cne $ManifestSha -or (Bootstrap-V2Hash $br) -cne $BindingSha){throw 'V2_BOOTSTRAP_PIN_HOLD'}
 $pins=([Text.Encoding]::UTF8.GetString($mr)|ConvertFrom-Json -AsHashtable).files_sha256
 $targets=@($fixed,(Join-Path $fixed 'Code'),$PSScriptRoot,(Join-Path $fixed 'HostManifest.json'),(Join-Path $fixed 'InstalledBinding.json'))
 foreach($n in $pins.Keys){if($n.StartsWith('Code/')){$p=Join-Path $fixed $n;if((Bootstrap-V2Hash ([IO.File]::ReadAllBytes($p))) -cne $pins[$n]){throw 'V2_BOOTSTRAP_CODE_SHA_HOLD'};$targets+=$p}}
 foreach($p in $targets){
  if((Get-Item -Force -LiteralPath $p).Attributes -band [IO.FileAttributes]::ReparsePoint){throw 'V2_BOOTSTRAP_REPARSE_HOLD'}
  $acl=Get-Acl -LiteralPath $p
  if($acl.GetOwner([Security.Principal.SecurityIdentifier]).Value -cne 'S-1-5-32-544'){throw 'V2_BOOTSTRAP_OWNER_HOLD'}
  foreach($rule in $acl.GetAccessRules($true,$true,[Security.Principal.SecurityIdentifier])){if($rule.PropagationFlags -band [Security.AccessControl.PropagationFlags]::InheritOnly){continue};if($rule.AccessControlType -eq 'Allow' -and $rule.IdentityReference.Value -notin @('S-1-5-18','S-1-5-32-544') -and ([int]$rule.FileSystemRights -band 0xD0156)){throw 'V2_BOOTSTRAP_WRITER_HOLD'}}
 }

 Add-Type -Path @((Join-Path $PSScriptRoot 'V2Contract.cs'),(Join-Path $PSScriptRoot 'V2Publish.cs'),(Join-Path $PSScriptRoot 'Session0PipeBoundary.cs'))
 . (Join-Path $PSScriptRoot 'V2InstallCore.ps1');. (Join-Path $PSScriptRoot 'V2Installed.ps1')
 $c=Read-V2Installed $ManifestSha $BindingSha;$m=$c.manifest;$b=$c.binding
 if($PipeName -notin @(@('arm','work','control')|ForEach-Object{$m.pipe_name+'-'+$_})){throw 'V2_CLIENT_PIPE_PIN_HOLD'}
 $inputStream=[Console]::OpenStandardInput();$data=[Collections.Generic.List[byte]]::new()
 while(($n=$inputStream.ReadByte()) -ne -1){if($data.Count -ge 16384){throw 'V2_CLIENT_FRAME_HOLD'};$data.Add([byte]$n)}
 $cancel=[Threading.CancellationTokenSource]::new(85000)
 $pipe=[LimitlessSession0PipeV2]::Connect($PipeName,$b.worker_sid)
 [LimitlessSession0PipeV2]::WriteFrame($pipe,$data.ToArray(),$cancel.Token).GetAwaiter().GetResult()
 $answer=[LimitlessSession0PipeV2]::ReadFrame($pipe,$cancel.Token).GetAwaiter().GetResult();[Console]::Out.Write([Text.Encoding]::UTF8.GetString($answer))
}catch{[Console]::Out.Write('{"schema":2,"status":"HOLD","reason":"V2_PIPE_AUTH_OR_PROTOCOL_HOLD"}')}
finally{if($pipe){$pipe.Dispose()};if($cancel){$cancel.Dispose()}}

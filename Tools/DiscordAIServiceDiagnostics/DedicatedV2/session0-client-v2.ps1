param([Parameter(Mandatory)][string]$PipeName,[Parameter(Mandatory)][string]$ManifestSha,[Parameter(Mandatory)][string]$BindingSha)
$ErrorActionPreference='Stop';$pipe=$null;$cancel=$null
try{
 if($PSScriptRoot -cne 'C:\Program Files\LimitlessRelayDedicatedV2_20261027\Code\V2'){throw 'D3_CLIENT_LOCATION'}
 . (Join-Path $PSScriptRoot 'DedicatedBootstrap.ps1');$c=Read-D3Installed $ManifestSha $BindingSha;$m=$c.manifest;$b=$c.binding
 if($PipeName -notin @('arm','work','control'|ForEach-Object{$m.pipe_name+'-'+$_})){throw 'D3_CLIENT_PIPE'}
 $inputStream=[Console]::OpenStandardInput();$data=[Collections.Generic.List[byte]]::new();while(($n=$inputStream.ReadByte()) -ne -1){if($data.Count -ge 16384){throw 'D3_CLIENT_BOUND'};$data.Add([byte]$n)}
 $cancel=[Threading.CancellationTokenSource]::new(85000);$pipe=[LimitlessSession0PipeV2]::Connect($PipeName,$b.worker_sid)
 [LimitlessSession0PipeV2]::WriteFrame($pipe,$data.ToArray(),$cancel.Token).GetAwaiter().GetResult();$answer=[LimitlessSession0PipeV2]::ReadFrame($pipe,$cancel.Token).GetAwaiter().GetResult();[Console]::Out.Write([Text.Encoding]::UTF8.GetString($answer))
}catch{[Console]::Out.Write('{"schema":2,"status":"HOLD","reason":"DEDICATED_PIPE_IDENTITY_NO_RETRY"}')}
finally{if($pipe){$pipe.Dispose()};if($cancel){$cancel.Dispose()}}

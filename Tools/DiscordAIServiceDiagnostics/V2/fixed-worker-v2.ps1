param([Parameter(Mandatory)][ValidatePattern('^[a-f0-9]{64}$')][string]$ManifestSha,[Parameter(Mandatory)][ValidatePattern('^[a-f0-9]{64}$')][string]$BindingSha)
$ErrorActionPreference='Stop';$cancel=$null
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

 Add-Type -Path @((Join-Path $PSScriptRoot 'V2Contract.cs'),(Join-Path $PSScriptRoot 'V2Publish.cs'),(Join-Path $PSScriptRoot 'Session0PipeBoundary.cs'),(Join-Path $PSScriptRoot 'BrokerJob90.cs'))
 . (Join-Path $PSScriptRoot 'V2InstallCore.ps1');. (Join-Path $PSScriptRoot 'V2Installed.ps1')
 $c=Read-V2Installed $ManifestSha $BindingSha;$m=$c.manifest;$b=$c.binding;$homeBefore=$env:CODEX_HOME
 $me=[LimitlessSession0PipeV2]::Current();$logon=[LimitlessSession0PipeV2]::CurrentLogon()
 if($me.Sid -cne $b.worker_sid -or $me.Administrator -or $me.Elevated -or $logon[0] -ne 4 -or $logon[1] -ne 0){throw 'V2_WORKER_TOKEN_HOLD'}
 $inputPipe=[Console]::OpenStandardInput()
 function Read-V2Exact([int]$size){$bytes=[byte[]]::new($size);$i=0;while($i -lt $size){$n=$inputPipe.Read($bytes,$i,$size-$i);if($n -eq 0){throw 'V2_WORKER_EOF_HOLD'};$i+=$n};return ,$bytes}
 $header=Read-V2Exact 4;$size=[BitConverter]::ToInt32($header,0);if($size -lt 2 -or $size -gt 16384){throw 'V2_WORKER_SIZE_HOLD'}
 $bytes=Read-V2Exact $size;$p=[Text.Encoding]::UTF8.GetString($bytes)|ConvertFrom-Json -AsHashtable
 if($p.epoch -cne $m.epoch -or $p.request_id -cnotmatch '^[a-f0-9]{32}$' -or $p.budget_ms -lt 1000 -or $p.budget_ms -gt 80000){throw 'V2_WORKER_INPUT_HOLD'}
 $cancel=[Threading.CancellationTokenSource]::new();$null=[LimitlessBrokerJob90V1]::WatchInputClose($inputPipe,$cancel)
 $env:PATH='C:\Program Files\PowerShell\7;C:\Windows\System32;C:\Windows'
 $job=[LimitlessBrokerJob90V1]::RunInput((Join-Path $m.code_root 'Runtime/python.exe'),@('-I','-B','-X','utf8',(Join-Path $PSScriptRoot 'entry-v2.py'),'--action','fake_worker','--manifest-sha',$ManifestSha,'--binding-sha',$BindingSha),$m.code_root,[int]$p.budget_ms,$cancel.Token,$bytes)
 if($job.ExitCode -ne 0 -or $job.TimedOut -or $job.Cancelled -or -not $job.CleanupVerified -or $job.ActiveProcesses -ne 0 -or $job.StdoutTruncated -or $job.StderrTruncated -or $homeBefore -cne $env:CODEX_HOME){throw 'V2_WORKER_JOB_HOLD'}
 $value=$job.Stdout|ConvertFrom-Json -AsHashtable
 if($value.status -cne 'FAKE_ONLY_COMPLETE' -or $value.request_id -cne $p.request_id -or $value.epoch -cne $m.epoch){throw 'V2_FAKE_RESULT_HOLD'}
 $value.job=@{exit_code=0;active_processes=0;cleanup_verified=$true;timed_out=$false};$value|ConvertTo-Json -Depth 10 -Compress
}catch{'{"schema":2,"status":"HOLD","reason":"V2_FIXED_WORKER_HOLD_NO_RETRY"}'}
finally{if($cancel){$cancel.Cancel();$cancel.Dispose()}}

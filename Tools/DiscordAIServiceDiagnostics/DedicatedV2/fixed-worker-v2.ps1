param([Parameter(Mandatory)][string]$ManifestSha,[Parameter(Mandatory)][string]$BindingSha)
$ErrorActionPreference='Stop';$cancel=$null
try{
 if($PSScriptRoot -cne 'C:\Program Files\LimitlessRelayDedicatedV2_20261027\Code\V2'){throw 'D3_CHILD_LOCATION'}
 . (Join-Path $PSScriptRoot 'DedicatedBootstrap.ps1');$c=Read-D3Installed $ManifestSha $BindingSha -WorkerContext;$m=$c.manifest;$b=$c.binding
 $me=[LimitlessSession0PipeV2]::Current();$logon=[LimitlessSession0PipeV2]::CurrentLogon();if($me.Sid -cne $b.worker_sid -or $me.Administrator -or $me.Elevated -or $logon[0] -ne 4 -or $logon[1] -ne 0){throw 'D3_CHILD_IDENTITY'}
 Set-D3WorkerEnvironment $m
 $inputPipe=[Console]::OpenStandardInput();function Read-D3Exact([int]$count){$v=[byte[]]::new($count);$offset=0;while($offset -lt $count){$n=$inputPipe.Read($v,$offset,$count-$offset);if($n -eq 0){throw 'D3_CHILD_EOF'};$offset+=$n};return ,$v}
 $size=[BitConverter]::ToInt32((Read-D3Exact 4),0);if($size -lt 2 -or $size -gt 16384){throw 'D3_CHILD_FRAME'};$raw=Read-D3Exact $size;$p=[Text.Encoding]::UTF8.GetString($raw)|ConvertFrom-Json -AsHashtable
 if($p.epoch -cne $m.epoch -or $p.request_id -cnotmatch '^[a-f0-9]{32}$' -or $p.budget_ms -lt 1000 -or $p.budget_ms -gt 80000){throw 'D3_CHILD_REQUEST'}
 $cancel=[Threading.CancellationTokenSource]::new();$null=[LimitlessBrokerJob90V1]::WatchInputClose($inputPipe,$cancel)
 $job=[LimitlessBrokerJob90V1]::RunInput((Join-Path $m.code_root 'Runtime/python.exe'),@('-I','-B','-X','utf8',(Join-Path $PSScriptRoot 'entry-v2.py'),'--action','fake_worker','--manifest-sha',$ManifestSha,'--binding-sha',$BindingSha),$m.code_root,[int]$p.budget_ms,$cancel.Token,$raw)
 if($job.ExitCode -ne 0 -or $job.TimedOut -or $job.Cancelled -or -not $job.CleanupVerified -or $job.ActiveProcesses -ne 0 -or $job.StdoutTruncated -or $job.StderrTruncated){throw 'D3_CHILD_JOB_HOLD'}
 $value=$job.Stdout|ConvertFrom-Json -AsHashtable;if($value.status -cne 'FAKE_ONLY_COMPLETE' -or $value.epoch -cne $m.epoch -or $value.request_id -cne $p.request_id){throw 'D3_CHILD_RESULT'}
 $value.job=@{exit_code=0;active_processes=0;cleanup_verified=$true;timed_out=$false};$value|ConvertTo-Json -Depth 10 -Compress
}catch{'{"schema":2,"status":"HOLD","reason":"DEDICATED_FAKE_JOB_NO_RETRY"}'}finally{if($cancel){$cancel.Cancel();$cancel.Dispose()}}

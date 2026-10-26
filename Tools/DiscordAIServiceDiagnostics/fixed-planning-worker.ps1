# 보호 설치에서만 실행되는 고정 Worker wrapper입니다. 임의 argv/prompt를 받지 않습니다.
$ErrorActionPreference='Stop'
[Console]::OutputEncoding=[Text.UTF8Encoding]::new($false)
$install=Split-Path (Split-Path (Split-Path $PSScriptRoot -Parent) -Parent) -Parent
$config=Get-Content -LiteralPath (Join-Path $install 'HostManifest.json') -Raw|ConvertFrom-Json
Add-Type -Path (Join-Path $PSScriptRoot 'Session0PipeBoundary.cs')
Add-Type -Path (Join-Path $PSScriptRoot 'BrokerJob90.cs')
$watch=[Diagnostics.Stopwatch]::StartNew();$cancel=[Threading.CancellationTokenSource]::new()
$homeBefore=$env:CODEX_HOME
$answer=@{status='NOT_VERIFIED';reason='FIXED_WRAPPER_HOLD'}
try {
 [LimitlessSession0PipeV1]::VerifyProtectedPath($install,$false,$config.worker_sid)
 [LimitlessSession0PipeV1]::VerifyProtectedPath($config.state_root,$true,$config.worker_sid)
 $identity=[LimitlessSession0PipeV1]::Current();$logon=[LimitlessSession0PipeV1]::CurrentLogon()
 if($identity.Sid -ne $config.worker_sid -or $identity.Administrator -or $identity.Elevated -or $logon[0] -ne 4 -or $logon[1] -ne 0){throw 'FIXED_OS_CONTEXT_HOLD'}
 if($config.execution_mode -eq 'OWNER_APPROVED_READONLY_ONCE'){
  foreach($entry in $config.external_cli_files.PSObject.Properties){[LimitlessSession0PipeV1]::VerifyExternalReadonly((Join-Path $config.external_cli_root $entry.Name),$config.admin_sid,$entry.Value)}
 }
 $inputPipe=[Console]::OpenStandardInput()
 function Read-Exact([int]$size){
  $buffer=[byte[]]::new($size);$offset=0
  while($offset -lt $size){$n=$inputPipe.Read($buffer,$offset,$size-$offset);if($n -eq 0){throw 'FIXED_EOF_HOLD'};$offset+=$n};return ,$buffer
 }
 $header=Read-Exact 4;$size=[BitConverter]::ToInt32($header,0)
 if($size -lt 2 -or $size -gt 16384){throw 'FIXED_SIZE_HOLD'}
 $bytes=Read-Exact $size;$payload=[Text.Encoding]::UTF8.GetString($bytes)|ConvertFrom-Json
 $null=[LimitlessBrokerJob90V1]::WatchInputClose($inputPipe,$cancel)
 $remaining=[Math]::Min([int]$payload.budget_ms,[int](($payload.ticket.deadline-[DateTimeOffset]::UtcNow.ToUnixTimeMilliseconds()/1000)*1000))-$watch.ElapsedMilliseconds-1500
 if($remaining -lt 1000){throw 'FIXED_TIME_HOLD'}
 $env:LIMITLESS_BATCH_LOGON_TYPE='4';$env:LIMITLESS_BATCH_SESSION_ID='0'
 $env:PATH='C:\Program Files\PowerShell\7;C:\Windows\System32;C:\Windows'
 $python=Join-Path $install 'Runtime/python.exe';$entry=Join-Path $PSScriptRoot 'fixed-planning-entry.py'
 $job=[LimitlessBrokerJob90V1]::RunInput($python,@('-I','-B','-X','utf8',$entry),$install,[int]$remaining,$cancel.Token,$bytes)
 $value=$job.Stdout|ConvertFrom-Json
 $answer=@{request_id=$payload.ticket.request_id;account=[Security.Principal.WindowsIdentity]::GetCurrent().Name;sid_matches=$true;session_id=0;logon_type=4
  status='NOT_VERIFIED';codex_home_unchanged=($homeBefore -ceq $env:CODEX_HOME);entry_result=$value
  job=@{exit_code=$job.ExitCode;timed_out=$job.TimedOut;cleanup_verified=$job.CleanupVerified;active_processes=$job.ActiveProcesses}}
 if($job.ExitCode -eq 0 -and -not $job.Cancelled -and -not $job.TimedOut -and $job.CleanupVerified -and $job.ActiveProcesses -eq 0 -and -not $job.StdoutTruncated -and -not $job.StderrTruncated -and $value.status -eq 'VERIFIED'){$answer.status='VERIFIED'}
} catch {$answer=@{status='NOT_VERIFIED';reason='FIXED_WRAPPER_HOLD_NO_RETRY'}}
finally {$cancel.Cancel();$cancel.Dispose()}
$answer|ConvertTo-Json -Depth 24 -Compress
# 가짜/검증 실패도 중개기가 HOLD 결과로 기록할 수 있도록 구조화 응답을 반환합니다.
exit 0

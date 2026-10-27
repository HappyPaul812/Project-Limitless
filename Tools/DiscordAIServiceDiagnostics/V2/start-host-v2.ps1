param([Parameter(Mandatory)][ValidatePattern('^[a-f0-9]{64}$')][string]$ManifestSha,[Parameter(Mandatory)][ValidatePattern('^[a-f0-9]{64}$')][string]$BindingSha)
$ErrorActionPreference='Stop';$lease=$null;$hostInstance=$null;$stage='BOOTSTRAP';$c=$null;$workerState=$null
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

 Add-Type -Path @((Join-Path $PSScriptRoot 'V2Contract.cs'),(Join-Path $PSScriptRoot 'V2Publish.cs'),(Join-Path $PSScriptRoot 'Session0PipeBoundary.cs'),(Join-Path $PSScriptRoot 'BrokerJob90.cs'),(Join-Path $PSScriptRoot 'V2WorkerFence.cs'),(Join-Path $PSScriptRoot 'V2Host.cs'))
 . (Join-Path $PSScriptRoot 'V2InstallCore.ps1');. (Join-Path $PSScriptRoot 'V2Installed.ps1');. (Join-Path $PSScriptRoot 'V2WindowsAdapter.ps1')
 $stage='VALIDATE_INSTALLED_PACKAGE';$c=Read-V2Installed $ManifestSha $BindingSha -AllRuntime;$m=$c.manifest;$b=$c.binding
 $identity=[LimitlessSession0PipeV2]::Current();$logon=[LimitlessSession0PipeV2]::CurrentLogon()
 if($identity.Sid -cne $b.worker_sid -or $identity.Administrator -or $identity.Elevated -or $logon[0] -ne 4 -or $logon[1] -ne 0){throw 'V2_HOST_REAL_TOKEN_HOLD'}
 $boot='C:\ProgramData\LimitlessRelay\boot-approval.json';if((Get-FileHash -LiteralPath $boot).Hash.ToLowerInvariant() -cne [LimitlessV2Contract]::BootSha){throw 'V2_HOST_BOOT_HOLD'}
 $stage='IPC_APPROVAL';$aPath=Join-Path $m.code_root 'IPCApproval.json'
 [LimitlessV2Publish]::Verify($aPath,[LimitlessV2Contract]::FsSddl('code',$false,$b.worker_sid,$b.bot_sid))
 $approvalRaw=Get-Content -Raw -LiteralPath $aPath
 [LimitlessV2Contract]::IpcApproval($approvalRaw,$m.epoch,$ManifestSha,$BindingSha,[DateTimeOffset]::UtcNow.ToUnixTimeSeconds());$a=$approvalRaw|ConvertFrom-Json -AsHashtable
 $stage='SHARED_WORKER_FENCE';$workerState=Get-V2WorkerSnapshot $b.worker_sid @($PID);Assert-V2WorkerState $workerState
 $leasePath=Join-Path ([LimitlessV2Contract]::ApprovalRoot) ('Worker-'+$b.worker_sid+'.lease')
 [LimitlessV2Publish]::Verify($leasePath,[LimitlessV2Contract]::FsSddl('state',$false,$b.worker_sid,''))
 $lease=[IO.File]::Open($leasePath,[IO.FileMode]::Open,[IO.FileAccess]::ReadWrite,[IO.FileShare]::None)
 $workerState=Get-V2WorkerSnapshot $b.worker_sid @($PID);Assert-V2WorkerState $workerState
 [LimitlessSession0PipeV2]::VerifyProtectedPath($m.state_root,$true,$b.worker_sid)
 $task=(Get-ScheduledTask -TaskName $m.task_name -TaskPath '\' -ErrorAction Stop)
 $args='-NoProfile -NonInteractive -File "'+$PSCommandPath+'" -ManifestSha '+$ManifestSha+' -BindingSha '+$BindingSha
 $sid=if($task.Principal.UserId -like 'S-1-*'){$task.Principal.UserId}else{([Security.Principal.NTAccount]::new($task.Principal.UserId)).Translate([Security.Principal.SecurityIdentifier]).Value}
 if($sid -cne $b.worker_sid -or $task.Principal.LogonType -ne 'Password' -or $task.Principal.RunLevel -ne 'Limited' -or $task.Triggers.Count -ne 0 -or $task.Settings.RestartCount -ne 0 -or $task.Settings.MultipleInstances -ne 'IgnoreNew' -or [Xml.XmlConvert]::ToTimeSpan($task.Settings.ExecutionTimeLimit).TotalSeconds -ne 90 -or -not $task.Settings.AllowHardTerminate -or $task.Actions.Count -ne 1 -or $task.Actions[0].Arguments -cne $args -or $task.Actions[0].WorkingDirectory -ine $m.code_root -or $task.Actions[0].Execute -ine 'C:\Program Files\PowerShell\7\pwsh.exe'){throw 'V2_FIXED_TASK_HOLD'}
 $s=New-Object -ComObject 'Schedule.Service';$s.Connect();[LimitlessV2Contract]::ExactSddl($s.GetFolder('\').GetTask($m.task_name).GetSecurityDescriptor(7),[LimitlessV2Contract]::TaskSddl($b.worker_sid))
 $env:PATH='C:\Program Files\PowerShell\7;C:\Windows\System32;C:\Windows'
 foreach($n in @('PYTHONPATH','PYTHONHOME','DOTNET_STARTUP_HOOKS','DOTNET_ADDITIONAL_DEPS')){[Environment]::SetEnvironmentVariable($n,$null,'Process')}
 $stage='IPC_ONE_SHOT_CONSUME';$consumedPath=Join-Path $m.state_root 'ipc-host-consumed.json'
 $consume=[IO.File]::Open($consumedPath,[IO.FileMode]::CreateNew,[IO.FileAccess]::Write,[IO.FileShare]::None)
 try{$bytes=[Text.UTF8Encoding]::new($false).GetBytes(($m.epoch));$consume.Write($bytes,0,$bytes.Length);$consume.Flush($true)}finally{$consume.Dispose()}
 $lifetime=[int][Math]::Min(90000,($a.expires_unix-[DateTimeOffset]::UtcNow.ToUnixTimeSeconds())*1000)
 $stage='RUN_FAKE_ONLY';$hostInstance=[LimitlessV2Host]::new($b.bot_sid,$b.admin_sid,$b.worker_sid,$m.pipe_name,$ManifestSha,$BindingSha);$hostInstance.Run($lifetime)
}catch{
 $failure=@{schema=2;status='HOLD';stage=$stage;reason='V2_START_HOLD_NO_RETRY';error_type=$_.Exception.GetType().FullName;hresult=$_.Exception.HResult}
 if('LimitlessV2Contract' -as [type]){$failure.errors=[LimitlessV2Contract]::ErrorCodes($_.Exception)}
 if('LimitlessV2WorkerFence' -as [type]){$failure.worker_query_errors=[LimitlessV2WorkerFence]::FailureCodes}
 if($workerState -and $workerState.errors){$failure.worker_query_errors=$workerState.errors}
 # State의 보호를 확인할 수 있을 때만 제한된 상속 ACL로 새 오류 파일을 남깁니다.
 if($c){try{
  [LimitlessSession0PipeV2]::VerifyProtectedPath($c.manifest.state_root,$true,$c.binding.worker_sid)
  $log=Join-Path $c.manifest.state_root ('HostError-'+[guid]::NewGuid().ToString('N')+'.json')
  $stream=[IO.File]::Open($log,[IO.FileMode]::CreateNew,[IO.FileAccess]::Write,[IO.FileShare]::None)
  try{$bytes=[Text.UTF8Encoding]::new($false).GetBytes(($failure|ConvertTo-Json -Depth 12));$stream.Write($bytes,0,$bytes.Length);$stream.Flush($true)}finally{$stream.Dispose()}
  [LimitlessSession0PipeV2]::VerifyProtectedPath($log,$true,$c.binding.worker_sid);$failure.protected_log='VERIFIED'
 }catch{$failure.protected_log='UNAVAILABLE_OR_NOT_VERIFIED'}}
 $failure|ConvertTo-Json -Depth 12 -Compress
}
finally{if($hostInstance){$hostInstance.Stop()};if($lease){$lease.Dispose()}}

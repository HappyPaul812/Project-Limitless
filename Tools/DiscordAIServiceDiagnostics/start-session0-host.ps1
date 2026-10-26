param([Parameter(Mandatory)][ValidatePattern('^[a-f0-9]{64}$')][string]$ManifestSha)
# 고정 task가 서비스 배치 토큰으로 시작합니다. 계정/서비스/task를 만들지 않습니다.
$ErrorActionPreference='Stop'
$install=Join-Path $env:ProgramFiles 'LimitlessRelayTrial'
if($PSScriptRoot -ine (Join-Path $install 'Code/Tools/DiscordAIServiceDiagnostics')){throw 'HOST_FIXED_INSTALL_HOLD'}
$manifest=Join-Path $install 'HostManifest.json'
if((Get-FileHash -LiteralPath $manifest -Algorithm SHA256).Hash.ToLower() -ne $ManifestSha){throw 'HOST_MANIFEST_PIN_HOLD'}
$config=Get-Content -LiteralPath $manifest -Raw|ConvertFrom-Json
if($config.schema -ne 1 -or @($config.bot_sid,$config.admin_sid,$config.worker_sid|Select-Object -Unique).Count -ne 3){throw 'HOST_DISTINCT_SID_HOLD'}
# 보호된 설치의 코드만 compile합니다. bootstrap도 owner와 모든 allow-write rule을 검사합니다.
function Test-InstallAcl([string]$path){
 if((Get-Item -LiteralPath $path -Force).Attributes -band [IO.FileAttributes]::ReparsePoint){throw 'HOST_REPARSE_HOLD'}
 $acl=Get-Acl -LiteralPath $path
 $trusted=@('S-1-5-18','S-1-5-32-544')
 if($acl.GetOwner([Security.Principal.SecurityIdentifier]).Value -notin $trusted){throw 'HOST_OWNER_HOLD'}
 foreach($rule in $acl.GetAccessRules($true,$true,[Security.Principal.SecurityIdentifier])){
  if($rule.PropagationFlags -band [Security.AccessControl.PropagationFlags]::InheritOnly){continue}
  if($rule.AccessControlType -eq 'Allow' -and $rule.IdentityReference.Value -notin $trusted -and ([int]$rule.FileSystemRights -band 0xD0156)) {throw 'HOST_WRITER_HOLD'}
 }
}
Test-InstallAcl $install;Test-InstallAcl $manifest
foreach($entry in $config.files_sha256.PSObject.Properties){
 $path=[IO.Path]::GetFullPath((Join-Path $install $entry.Name))
 if(-not $path.StartsWith($install+[IO.Path]::DirectorySeparatorChar,[StringComparison]::OrdinalIgnoreCase)){throw 'HOST_FILE_SCOPE_HOLD'}
 Test-InstallAcl $path
 if((Get-FileHash -LiteralPath $path -Algorithm SHA256).Hash.ToLower() -ne $entry.Value){throw 'HOST_FILE_SHA_HOLD'}
}
$files=@('BrokerJob90.cs','Session0PipeBoundary.cs','Session0Host.cs')|ForEach-Object{Join-Path $PSScriptRoot $_}
Add-Type -Path $files
if($config.execution_mode -eq 'OWNER_APPROVED_READONLY_ONCE'){
 foreach($entry in $config.external_cli_files.PSObject.Properties){[LimitlessSession0PipeV1]::VerifyExternalReadonly((Join-Path $config.external_cli_root $entry.Name),$config.admin_sid,$entry.Value)}
}
[LimitlessSession0PipeV1]::VerifyProtectedPath($config.state_root,$true,$config.worker_sid)
$peer=[LimitlessSession0PipeV1]::Current();$logon=[LimitlessSession0PipeV1]::CurrentLogon()
if($peer.Sid -ne $config.worker_sid -or $peer.Elevated -or $peer.Administrator -or $logon[0] -ne 4 -or $logon[1] -ne 0){throw 'HOST_BATCH_CONTEXT_HOLD'}
$task=Get-ScheduledTask -TaskName $config.task_name
$taskSid=if($task.Principal.UserId -match '^S-1-'){$task.Principal.UserId}else{([Security.Principal.NTAccount]::new($task.Principal.UserId)).Translate([Security.Principal.SecurityIdentifier]).Value}
$expected='-NoProfile -NonInteractive -File "'+$PSCommandPath+'" -ManifestSha '+$ManifestSha
if($task.Actions.Count -ne 1 -or $task.Actions[0].Execute -ine 'C:/Program Files/PowerShell/7/pwsh.exe' -or $task.Actions[0].Arguments -cne $expected -or $taskSid -ne $config.worker_sid -or $task.Principal.LogonType -ne 'Password' -or $task.Principal.RunLevel -ne 'Limited' -or $task.Settings.MultipleInstances -ne 'IgnoreNew' -or $task.Actions[0].WorkingDirectory -ine $install){throw 'HOST_FIXED_TASK_HOLD'}
$env:PATH='C:\Program Files\PowerShell\7;C:\Windows\System32;C:\Windows'
foreach($name in @('PYTHONPATH','PYTHONHOME','DOTNET_STARTUP_HOOKS','DOTNET_ADDITIONAL_DEPS')){[Environment]::SetEnvironmentVariable($name,$null,'Process')}
$hostInstance=[LimitlessSession0HostV1]::new($install,$config.bot_sid,$config.admin_sid,$config.worker_sid)
try{$hostInstance.Run($config.pipe_name,900)}finally{$hostInstance.Stop()}

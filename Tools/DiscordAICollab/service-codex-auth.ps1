param([ValidateSet('Inspect','Login','Verify')][string]$Action='Inspect',[string]$RequestId='local')
# 사용자 직접 실행 전용 로그인입니다. 기기 코드는 로컬 콘솔에만 표시하고 기록/전송하지 않습니다.
$ErrorActionPreference='Stop'
$root=$PSScriptRoot
if($RequestId -ne 'local' -and $RequestId -notmatch '^[a-f0-9]{32}$') {throw 'INVALID_REQUEST_ID'}
$result=@{status='HOLD';reason='AUTH_INSPECTION_NOT_COMPLETED';request_id=$RequestId;actual_ai_calls=0;api_fallback=$false;mode_changed=$false;action=$Action}
$oldEnvironment=@{}
function Read-Cli([string[]]$CliArguments) {
 $start=[Diagnostics.ProcessStartInfo]::new($script:binary)
 $start.UseShellExecute=$false;$start.CreateNoWindow=$true;$start.RedirectStandardOutput=$true;$start.RedirectStandardError=$true
 foreach($arg in $CliArguments) {$start.ArgumentList.Add($arg)}
 $process=[Diagnostics.Process]::new();$process.StartInfo=$start
 try {
  if(-not $process.Start()) {throw 'CLI_START_FAILED'}
  $stdout=$process.StandardOutput.ReadToEndAsync();$stderr=$process.StandardError.ReadToEndAsync()
  if(-not $process.WaitForExit(15000)) {$process.Kill();throw 'CLI_READ_TIMEOUT'}
  return @{code=$process.ExitCode;text=($stdout.GetAwaiter().GetResult()+$stderr.GetAwaiter().GetResult())}
 } finally {$process.Dispose()}
}
try {
 $approval=Get-Content 'C:\ProgramData\LimitlessRelay\boot-approval.json' -Raw | ConvertFrom-Json
 $identity=[Security.Principal.WindowsIdentity]::GetCurrent()
 $isAdmin=([Security.Principal.WindowsPrincipal]::new($identity)).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
 if($identity.User.Value -ne $approval.service_sid -or $identity.Name.Split('\')[-1] -ine 'LIMITLESSRelaySvc' -or $isAdmin) {throw 'SERVICE_ACCOUNT_CONTEXT_REQUIRED'}
 if($approval.tool_root -ne $root) {throw 'SERVICE_DEPLOYMENT_ROOT_MISMATCH'}
 if($approval.service_auth_scripts.'service-codex-auth.ps1' -ne (Get-FileHash -LiteralPath $PSCommandPath -Algorithm SHA256).Hash.ToLower()) {throw 'AUTH_SCRIPT_SHA_NOT_VERIFIED'}
 $profile=[Environment]::GetFolderPath([Environment+SpecialFolder]::UserProfile)
 if(-not [IO.Path]::IsPathRooted($profile) -or $profile -ine $env:USERPROFILE -or (Split-Path $profile -Leaf) -notlike 'LIMITLESSRelaySvc*') {throw 'SERVICE_OWN_PROFILE_REQUIRED'}
 $homePath=Join-Path $profile '.codex'
 if((Test-Path -LiteralPath $homePath) -and ((Get-Item -LiteralPath $homePath -Force).Attributes -band [IO.FileAttributes]::ReparsePoint)) {throw 'SERVICE_HOME_LINK_REJECTED'}
 $result.context_verified=$true;$result.profile=$profile;$result.codex_home=$homePath;$result.home_existed_before=Test-Path -LiteralPath $homePath
 $binary=$approval.service_codex.path
 if(-not $binary -or -not (Test-Path -LiteralPath $binary -PathType Leaf) -or (Get-FileHash -LiteralPath $binary -Algorithm SHA256).Hash.ToLower() -ne $approval.service_codex.sha256) {throw 'PINNED_CLI_ACCESS_OR_SHA_NOT_VERIFIED'}
 $result.cli_path=$binary;$result.cli_sha256=$approval.service_codex.sha256
 # 현재 서비스 계정의 환경만 허용하고 다른 사용자 CODEX_HOME/API·Discord 비밀은 제거합니다.
 $allowed=@('SYSTEMROOT','WINDIR','USERPROFILE','APPDATA','LOCALAPPDATA','TEMP','TMP','PATH','PATHEXT','COMSPEC','HOME','CODEX_HOME','LANG','LC_ALL')
 foreach($entry in [Environment]::GetEnvironmentVariables('Process').GetEnumerator()) {
  $oldEnvironment[$entry.Key]=$entry.Value
  if($entry.Key -notin $allowed) {[Environment]::SetEnvironmentVariable($entry.Key,$null,'Process')}
 }
 foreach($managed in @('USERPROFILE','HOME','CODEX_HOME')) {if(-not $oldEnvironment.ContainsKey($managed)) {$oldEnvironment[$managed]=$null}}
 $env:USERPROFILE=$profile;$env:HOME=$profile;$env:CODEX_HOME=$homePath
 $version=Read-Cli -CliArguments @('--version');$result.cli_version=[regex]::Match($version.text,'codex-cli\s+([0-9A-Za-z.\-]+)').Groups[1].Value
 $help=Read-Cli -CliArguments @('login','--help');$result.device_auth_supported=($help.code -eq 0 -and $help.text.Contains('--device-auth'))
 if($version.code -ne 0 -or -not $result.cli_version -or -not $result.device_auth_supported) {throw 'CLI_VERSION_OR_DEVICE_AUTH_NOT_SUPPORTED'}
 if($Action -eq 'Login') {
  # 자동 점검/리다이렉트된 출력에서 로그인 코드를 만들지 않습니다. 대표가 연 서비스 계정 콘솔에서만 실행합니다.
  if(-not [Environment]::UserInteractive -or [Console]::IsOutputRedirected -or [Console]::IsInputRedirected) {throw 'OWNER_LOCAL_INTERACTIVE_CONSOLE_REQUIRED'}
  New-Item -ItemType Directory -Force -Path $homePath | Out-Null
  # 서비스 계정 자신의 신규 프로필만 보호합니다. 상위 폴더나 기존 개인 계정 ACL은 건드리지 않습니다.
  $acl=[Security.AccessControl.DirectorySecurity]::new();$acl.SetAccessRuleProtection($true,$false)
  $acl.SetOwner($identity.User)
  foreach($sidValue in @($identity.User.Value,'S-1-5-18','S-1-5-32-544')) {
   $rule=[Security.AccessControl.FileSystemAccessRule]::new([Security.Principal.SecurityIdentifier]::new($sidValue),[Security.AccessControl.FileSystemRights]::FullControl,[Security.AccessControl.InheritanceFlags]'ContainerInherit,ObjectInherit',[Security.AccessControl.PropagationFlags]::None,[Security.AccessControl.AccessControlType]::Allow)
   $acl.AddAccessRule($rule)
  }
  Set-Acl -LiteralPath $homePath -AclObject $acl
  $configPath=Join-Path $homePath 'config.toml'
  $configText='cli_auth_credentials_store = "keyring"'+[Environment]::NewLine+'forced_login_method = "chatgpt"'+[Environment]::NewLine
  if(Test-Path -LiteralPath $configPath) {
   if([IO.File]::ReadAllText($configPath) -ne $configText) {throw 'EXISTING_SERVICE_CONFIG_REVIEW_REQUIRED'}
  } else {[IO.File]::WriteAllText($configPath,$configText,[Text.UTF8Encoding]::new($false))}
  $result.config_sha256_before_login=(Get-FileHash -LiteralPath $configPath -Algorithm SHA256).Hash.ToLower()
  Write-Host '이 서비스 계정 로컬 창의 안내로 바울 대표가 직접 인증하세요. 코드/토큰은 채팅·Discord·GitHub에 복사하지 마세요.'
  # 이 호출의 stdout/stderr를 변수/파일/봇으로 수집하지 않습니다. 실제 추론/exec 호출은 없습니다.
  & $binary login --device-auth
  if($LASTEXITCODE -ne 0) {throw 'DEVICE_AUTH_NOT_COMPLETED'}
 }
 if(Test-Path -LiteralPath $homePath) {
  $status=Read-Cli -CliArguments @('login','status')
  $result.subscription_authenticated=($status.code -eq 0 -and $status.text.Contains('using ChatGPT'))
 } else {$result.subscription_authenticated=$false}
 if($Action -eq 'Inspect') {$result.status='INSPECTED';$result.reason=if($result.subscription_authenticated){'CHATGPT_STATUS_CONFIRMED_USAGE_VERIFY_NEXT'}else{'OWNER_DEVICE_LOGIN_REQUIRED'}}
 else {
  if(-not $result.subscription_authenticated) {throw 'CHATGPT_SUBSCRIPTION_NOT_AUTHENTICATED'}
  $after=[Security.Principal.WindowsIdentity]::GetCurrent()
  if($after.User.Value -ne $identity.User.Value -or $env:CODEX_HOME -ne $homePath -or (Get-FileHash -LiteralPath $binary -Algorithm SHA256).Hash.ToLower() -ne $result.cli_sha256) {throw 'LOGIN_CONTEXT_CHANGED'}
  if([Environment]::GetFolderPath([Environment+SpecialFolder]::UserProfile) -ine $profile) {throw 'LOGIN_CONTEXT_CHANGED'}
  if($Action -eq 'Login' -and (Get-FileHash -LiteralPath $configPath -Algorithm SHA256).Hash.ToLower() -ne $result.config_sha256_before_login) {throw 'LOGIN_CONFIG_CHANGED'}
  $result.login_context_unchanged=$true
  $result.config_sha256_after_login=if(Test-Path -LiteralPath (Join-Path $homePath 'config.toml')){(Get-FileHash -LiteralPath (Join-Path $homePath 'config.toml') -Algorithm SHA256).Hash.ToLower()}else{$null}
  Set-Location -LiteralPath $root
  $usageText=& (Join-Path $root '.venv/Scripts/python.exe') -m collab.service_ai --request-id $RequestId
  $usage=$usageText | ConvertFrom-Json
  $result.official_usage=$usage
  $result.status=if($usage.status -eq 'READONLY_LINK_READY'){'AUTH_AND_USAGE_VERIFIED'}else{'HOLD'}
  $result.reason=if($result.status -eq 'AUTH_AND_USAGE_VERIFIED'){'MANUAL_KEEP_NO_AI_INFERENCE'}else{$usage.reason}
 }
} catch {
 $reason=$_.Exception.Message
 $result.status='HOLD';$result.reason=if($reason -match '^[A-Z_]+$'){$reason}else{'SERVICE_AUTH_INSPECTION_OR_LOGIN_NOT_VERIFIED'}
} finally {
 foreach($key in $oldEnvironment.Keys) {[Environment]::SetEnvironmentVariable($key,$oldEnvironment[$key],'Process')}
}
# 코드/토큰/메일·원문 CLI 출력을 저장하지 않습니다. 여기에는 분류와 공개 환경 메타데이터만 있습니다.
$output=Join-Path $root ('data/boot/service-codex-auth-'+$RequestId+'.json')
$temporary=$output+'.tmp'
[IO.File]::WriteAllText($temporary,($result | ConvertTo-Json -Depth 8),[Text.UTF8Encoding]::new($false))
Move-Item -LiteralPath $temporary -Destination $output -Force
$result | ConvertTo-Json -Depth 8

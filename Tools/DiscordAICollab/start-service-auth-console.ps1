param([ValidateSet('Inspect','Login','Verify')][string]$Action='Inspect')
# 바울 대표가 직접 여는 서비스 계정 콘솔입니다. runas 네이티브 인수 전달을 사용하지 않습니다.
$ErrorActionPreference='Stop'
$root=$PSScriptRoot
$approval=Get-Content 'C:\ProgramData\LimitlessRelay\boot-approval.json' -Raw | ConvertFrom-Json
if($approval.service_auth_scripts.'start-service-auth-console.ps1' -ne (Get-FileHash -LiteralPath $PSCommandPath -Algorithm SHA256).Hash.ToLower()) {throw 'AUTH_LAUNCHER_SHA_NOT_VERIFIED'}
$account=$env:COMPUTERNAME+'\LIMITLESSRelaySvc'
$profileKey='HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion\ProfileList\'+$approval.service_sid
$serviceProfile=[Environment]::ExpandEnvironmentVariables((Get-ItemProperty -LiteralPath $profileKey -Name ProfileImagePath).ProfileImagePath)
if(-not [IO.Path]::IsPathRooted($serviceProfile) -or (Split-Path $serviceProfile -Leaf) -notlike 'LIMITLESSRelaySvc*') {throw 'SERVICE_PROFILE_PATH_NOT_VERIFIED'}
$pwsh=(Get-Command pwsh).Source
$credential=Get-Credential -UserName $account -Message '서비스 계정 콘솔 실행용 Windows 암호를 로컬 입력창에만 입력하세요.'
if($null -eq $credential -or $credential.UserName -ine $account) {Write-Output 'HOLD_SERVICE_CREDENTIAL_REQUIRED';exit 2}
$original=@{}
try {
 foreach($entry in [Environment]::GetEnvironmentVariables('Process').GetEnumerator()) {$original[$entry.Key]=$entry.Value}
 $allowed=@('SYSTEMROOT','WINDIR','PATH','PATHEXT','COMSPEC','TEMP','TMP','LANG','LC_ALL')
 foreach($key in $original.Keys) {if($key -notin $allowed) {[Environment]::SetEnvironmentVariable($key,$null,'Process')}}
 # 부모 사용자의 프로필/비밀을 상속하지 않습니다. 레지스트리의 승인 SID 프로필만 지정합니다.
 $env:USERPROFILE=$serviceProfile;$env:HOME=$serviceProfile;$env:CODEX_HOME=Join-Path $serviceProfile '.codex'
 $env:APPDATA=Join-Path $serviceProfile 'AppData/Roaming';$env:LOCALAPPDATA=Join-Path $serviceProfile 'AppData/Local'
 $env:TEMP=Join-Path $serviceProfile 'AppData/Local/Temp';$env:TMP=$env:TEMP
 $arguments=@('-NoProfile','-NoExit','-File',(Join-Path $root 'service-codex-auth.ps1'),'-Action',$Action)
 # 사용자가 직접 요청한 인증/점검 콘솔이므로 표시합니다. Bot/Art와 작업 스케줄러는 건드리지 않습니다.
 $process=Start-Process -FilePath $pwsh -ArgumentList $arguments -Credential $credential -LoadUserProfile -WorkingDirectory $root -WindowStyle Normal -PassThru
 @{status='SERVICE_CONSOLE_STARTED';action=$Action;process_id=$process.Id;actual_ai_calls=0;mode_changed=$false} | ConvertTo-Json
} catch {
 $native=$_.Exception
 while($native.InnerException) {$native=$native.InnerException}
 $code=if($native -is [ComponentModel.Win32Exception]){$native.NativeErrorCode}else{$null}
 @{status='HOLD';reason='WINDOWS_SERVICE_CONSOLE_LOGON_FAILED';native_error=$code;exception_type=$native.GetType().Name;actual_ai_calls=0;mode_changed=$false} | ConvertTo-Json
} finally {
 foreach($key in @('USERPROFILE','HOME','CODEX_HOME','APPDATA','LOCALAPPDATA')) {if(-not $original.ContainsKey($key)) {[Environment]::SetEnvironmentVariable($key,$null,'Process')}}
 foreach($key in $original.Keys) {[Environment]::SetEnvironmentVariable($key,$original[$key],'Process')}
 $credential=$null
}

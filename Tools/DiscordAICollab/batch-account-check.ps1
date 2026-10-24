param([Parameter(Mandatory)][ValidatePattern('^[a-f0-9]{32}$')][string]$RequestId)
# AI/Discord/Codex/네트워크를 호출하지 않고 자기 실행 계정·환경만 기록합니다.
$ErrorActionPreference='Stop';$root=$PSScriptRoot
$result=@{request_id=$RequestId;status='FAIL';actual_ai_calls=0;codex_calls=0;network_calls=0;at=[datetime]::UtcNow.ToString('o')}
try {
 $approval=Get-Content 'C:\ProgramData\LimitlessRelay\boot-approval.json' -Raw | ConvertFrom-Json
 $identity=[Security.Principal.WindowsIdentity]::GetCurrent()
 $admin=([Security.Principal.WindowsPrincipal]::new($identity)).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
 Import-Module (Join-Path $root 'BatchReadOnlyMetadata.psm1') -Force
 $logon=[LimitlessBatchMetadataV1]::CurrentLogon()
 $profile=[Environment]::GetFolderPath([Environment+SpecialFolder]::UserProfile)
 $result.account=$identity.Name;$result.sid_matches=($identity.User.Value -eq $approval.service_sid);$result.is_admin=$admin
 $result.logon_type=$logon[0];$result.session_id=$logon[1];$result.user_interactive=[Environment]::UserInteractive
 $result.profile=$profile;$result.userprofile=$env:USERPROFILE;$result.codex_home_environment=$env:CODEX_HOME
 $result.working_directory=(Get-Location).Path;$result.script_root=$root
 $result.process_path=(Get-Process -Id $PID).Path
 $result.status=if($result.sid_matches -and -not $admin -and $logon[0] -eq 4 -and $logon[1] -eq 0 -and -not $result.user_interactive -and $profile -ieq $env:USERPROFILE -and (Get-Location).Path -eq $root){'PASS'}else{'FAIL'}
} catch {
 $result.status='FAIL';$result.error_type=$_.Exception.GetType().Name;$result.hresult=$_.Exception.HResult
}
$result.exit_code=if($result.status -eq 'PASS'){0}else{2}
$output=Join-Path $root ('data/batch-check/account-'+$RequestId+'.json')
[IO.File]::WriteAllText(($output+'.tmp'),($result | ConvertTo-Json -Depth 5),[Text.UTF8Encoding]::new($false))
Move-Item -LiteralPath ($output+'.tmp') -Destination $output -Force
exit $result.exit_code

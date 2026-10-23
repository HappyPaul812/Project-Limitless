param([Parameter(Mandatory)][string]$ToolRoot)
$ErrorActionPreference='Stop'
$stage='SERVICE_IDENTITY';$token=$null;$saved=$null;$pointer=[IntPtr]::Zero;$client=$null;$plainToken=$null
$resultPath=Join-Path $ToolRoot 'data/boot/service-setup-result.json'
try {
 if([Security.Principal.WindowsIdentity]::GetCurrent().Name.Split('\')[-1] -ne 'LIMITLESSRelaySvc') {throw 'SERVICE_IDENTITY_REQUIRED'}
 Set-Location -LiteralPath $ToolRoot
 Import-Module (Join-Path $ToolRoot 'DiscordSecretStore.psm1') -Force
 $public=Get-Content 'data/boot/setup-public.json' -Raw | ConvertFrom-Json
 Write-Host 'LIMITLESS 서비스 계정 보안 설정. 기존 Windows PIN/암호가 아닌 Discord Bot Token을 입력합니다.'
 $stage='NEW_ACCOUNT_DPAPI_TOKEN'
 $token=Read-Host 'Bot Token (숨김 입력, 채팅에 보내지 마세요)' -AsSecureString
 Save-LimitlessDiscordCredential -Root (Join-Path $ToolRoot 'data/private/boot-discord') -Token $token -Owners $public.owners -Guild $public.guild -Channels $public.channels -Replace | Out-Null
 $saved=Read-LimitlessDiscordCredential (Join-Path $ToolRoot 'data/private/boot-discord')
 # 새 계정 자신의 토큰으로 고정 Discord REST 인증만 확인합니다. Gateway/AI/메시지는 실행하지 않습니다.
 $stage='DISCORD_REST_AUTH'
 $pointer=[Runtime.InteropServices.Marshal]::SecureStringToBSTR($saved.Token)
 $plainToken=[Runtime.InteropServices.Marshal]::PtrToStringBSTR($pointer)
 $handler=[Net.Http.HttpClientHandler]::new();$handler.AllowAutoRedirect=$false
 $client=[Net.Http.HttpClient]::new($handler);$client.Timeout=[timespan]::FromSeconds(20)
 $client.DefaultRequestHeaders.Authorization=[Net.Http.Headers.AuthenticationHeaderValue]::new('Bot',$plainToken)
 $response=$client.GetAsync('https://discord.com/api/v10/users/@me').GetAwaiter().GetResult()
 if([int]$response.StatusCode -ne 200) {throw 'DISCORD_TOKEN_OR_CONNECTION_NOT_VERIFIED'}
 $response.Dispose();$client.Dispose();$client=$null;$plainToken=$null
 $stage='SERVICE_PYTHON_QUEUE_ACCESS'
 & (Join-Path $ToolRoot '.venv/Scripts/python.exe') -c "import sqlite3,json,pathlib; p=pathlib.Path('data/phase2/relay.sqlite'); c=sqlite3.connect(p.as_uri()+'?mode=ro',uri=True) if p.is_absolute() else sqlite3.connect('file:'+str(p)+'?mode=ro',uri=True); m=c.execute('SELECT mode FROM operation_mode').fetchone()[0]; assert m=='MANUAL'; print('SERVICE_RUNTIME_AND_MANUAL_PASS')"
 if($LASTEXITCODE -ne 0) {throw 'SERVICE_RUNTIME_OR_QUEUE_ACCESS_FAILED'}
 $stage='DELIVERY_INBOX_ACCESS'
 $config=Get-Content 'data/phase3/config.json' -Raw | ConvertFrom-Json
 if(-not (Test-Path -LiteralPath $config.inbox)) {throw 'DELIVERY_INBOX_UNAVAILABLE'}
 @{status='PASS';same_service_identity=$true;dpapi_current_user=$true;discord_rest_auth=$true;queue_read=$true;mode='MANUAL';ai='HOLD';prelogin_boot='NOT_VERIFIED'} | ConvertTo-Json | Set-Content $resultPath -Encoding utf8
 Write-Host '서비스 계정 토큰·REST·큐 접근 확인 PASS. 실제 봇과 AI는 시작하지 않았습니다.'
 exit 0
} catch {
 @{status='HOLD';reason=$stage;secret_logged=$false} | ConvertTo-Json | Set-Content $resultPath -Encoding utf8
 Write-Host '설정 실패: 암호/토큰 원문은 기록하지 않았습니다. 등록은 보류됩니다.'
 exit 2
} finally {
 if($null -ne $client) {$client.Dispose()}
 if($pointer -ne [IntPtr]::Zero) {[Runtime.InteropServices.Marshal]::ZeroFreeBSTR($pointer)}
 if($null -ne $saved) {$saved.Token.Dispose()}
 if($null -ne $token) {$token.Dispose()}
 $plainToken=$null
}

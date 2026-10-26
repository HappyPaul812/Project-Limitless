param([switch]$ConfirmLocalTokenInput)
# 새 봇 계정 자기 프로필에만 새 입력을 저장합니다. 기존 token 저장소를 열지 않습니다.
$ErrorActionPreference='Stop'
if(-not $ConfirmLocalTokenInput){'NO_TOKEN_INPUT_NO_STORE_CHANGE';return}
$install=Join-Path $env:ProgramFiles 'LimitlessRelayTrial'
$config=Get-Content -LiteralPath (Join-Path $install 'HostManifest.json') -Raw|ConvertFrom-Json
$identity=[Security.Principal.WindowsIdentity]::GetCurrent()
if($identity.User.Value -ne $config.bot_sid -or ([Security.Principal.WindowsPrincipal]::new($identity)).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)){throw 'NEW_LIMITED_BOT_SID_REQUIRED'}
Import-Module (Join-Path $install 'Code/Tools/DiscordAICollab/DiscordSecretStore.psm1') -Force
$store=Join-Path $env:LOCALAPPDATA 'LimitlessBotTrial/private'
if(Test-Path -LiteralPath $store){throw 'NEW_OWN_STORE_ONLY_NO_REPLACE'}
$token=Read-Host 'Limitless-test 전용 새 토큰을 로컬에서만 입력하세요. 기존 운영/시험 저장소를 복사하지 마세요' -AsSecureString
try{
 $null=Save-LimitlessDiscordCredential -Root $store -Token $token -Owners '740791801987727412' -Guild '1563112744436244541' -Channels '1564163234338967552'
 $status=Get-LimitlessDiscordCredentialStatus -Root $store
 if($status.status -ne 'ENCRYPTED_STORE_READY'){throw 'OWN_STORE_READBACK_HOLD'}
 @{status='OWN_DPAPI_READY_APP_ID_NOT_NETWORK_VERIFIED';token_displayed=$false;discord_requests=0}|ConvertTo-Json
} finally {$token.Dispose();$token=$null}

param([ValidateSet('Inspect','Setup')][string]$Action='Setup')
Set-StrictMode -Version Latest
$ErrorActionPreference='Stop'

function Test-MobileTrialApplication {
    param($Application,[string]$ExpectedOwner)
    if ([string]$Application.id -ne '1564156403571105906') {return $false}
    $owner=$null;$teamOwner=$null
    if ($Application.PSObject.Properties['owner']) {$owner=[string]$Application.owner.id}
    if ($Application.PSObject.Properties['team'] -and $null -ne $Application.team) {
        if ($Application.team.PSObject.Properties['owner_user_id']) {$teamOwner=[string]$Application.team.owner_user_id}
    }
    return ($owner -eq $ExpectedOwner -or $teamOwner -eq $ExpectedOwner)
}

function Test-MobileTrialPair {
    param([string]$Trial,[string]$Production)
    return ($Trial -eq '1564156403571105906' -and $Production -eq '1563102982386749440' -and $Trial -ne $Production)
}

# 오프라인 테스트는 함수만 가져오며 token/네트워크/디렉터리를 생성하지 않습니다.
if ($MyInvocation.InvocationName -eq '.') {return}

$stage='PIN_CHECK';$token=$null;$saved=$null;$pointer=[IntPtr]::Zero
$client=$null;$handler=$null;$response=$null;$stream=$null;$plainToken=$null
$result=@{status='HOLD';mode='MANUAL';trial_application_id='1564156403571105906';production_application_id='1563102982386749440';token_displayed=$false;model_calls=0;official_usage_reads=0;discord_application_reads=0;command_creates=0;host_launches=0}
try {
    if (-not (Test-MobileTrialPair $result.trial_application_id $result.production_application_id)) {throw 'APP_PAIR_HOLD'}
    $approvalPath='C:/ProgramData/LimitlessRelay/boot-approval.json'
    if ((Get-FileHash -LiteralPath $approvalPath -Algorithm SHA256).Hash.ToLowerInvariant() -ne '3e032ebcce3d91bd762a8dec81048d04607ddde761afd62489c0fa2b8e49140f') {throw 'BOOT_APPROVAL_SHA_HOLD'}
    $approval=Get-Content -LiteralPath $approvalPath -Raw|ConvertFrom-Json
    $publicPath=Join-Path $approval.tool_root 'data/boot/setup-public.json'
    $public=Get-Content -LiteralPath $publicPath -Raw|ConvertFrom-Json
    if ([string]$public.owners -ne '740791801987727412' -or [string]$public.guild -ne '1563112744436244541') {throw 'OWNER_OR_GUILD_HOLD'}
    $prepared='F:/study/codex/Project-Limitless/Temp/DiscordUsageDiagnostics20261026/MobileDiscordPhase3Prepared20261026'
    $manifestPath=Join-Path $prepared 'Manifest.UNAPPROVED.json'
    if ((Get-FileHash -LiteralPath $manifestPath -Algorithm SHA256).Hash.ToLowerInvariant() -ne 'f69049c61737476e6707bcb9559957e88115fbc19da0a56a92c8ae339348e5a7') {throw 'MANIFEST_SHA_HOLD'}
    $manifest=Get-Content -LiteralPath $manifestPath -Raw|ConvertFrom-Json
    $module=Join-Path $prepared 'Package/Tools/DiscordAICollab/DiscordSecretStore.psm1'
    if ((Get-FileHash -LiteralPath $module -Algorithm SHA256).Hash.ToLowerInvariant() -ne $manifest.packaged_files.'DiscordSecretStore.psm1'.sha256) {throw 'SECRET_MODULE_SHA_HOLD'}
    $root='F:/study/codex/Project-Limitless/Temp/MobileTrialCredentials20261026/LimitlessTest'
    $secretRoot=Join-Path $root 'private'
    if (Test-Path -LiteralPath $root) {throw 'TRIAL_ROOT_ALREADY_EXISTS_NO_RESET'}
    $result.owner_approved_id_matches=$true;$result.application_ids_distinct=$true
    $result.current_windows_identity=[Security.Principal.WindowsIdentity]::GetCurrent().Name
    $result.stage='TOKEN_NOT_ENTERED'
    if ($Action -eq 'Inspect') {
        $result.status='LOCAL_PUBLIC_PREFLIGHT_PASS';$result.token_store_created=$false
        $result|ConvertTo-Json -Compress
        exit 0
    }
    Import-Module -Name $module -Force
    $stage='LOCAL_SECURE_INPUT'
    Write-Host 'Limitless-test 전용 Bot Token만 입력하세요. 기존 운영 Token은 사용하지 않습니다.'
    $token=Read-Host '시험 Token (숨김 입력; 채팅에 보내지 마세요)' -AsSecureString
    if ($token.Length -lt 1 -or $token.Length -gt 4096) {throw 'TOKEN_LENGTH_HOLD'}
    $pointer=[Runtime.InteropServices.Marshal]::SecureStringToBSTR($token)
    $plainToken=[Runtime.InteropServices.Marshal]::PtrToStringBSTR($pointer)
    $stage='TRIAL_APP_AND_OWNER_CHECK'
    $handler=[Net.Http.HttpClientHandler]::new();$handler.AllowAutoRedirect=$false
    $client=[Net.Http.HttpClient]::new($handler);$client.Timeout=[timespan]::FromSeconds(15)
    $client.DefaultRequestHeaders.Authorization=[Net.Http.Headers.AuthenticationHeaderValue]::new('Bot',$plainToken)
    $result.discord_application_reads=1
    # 고정 endpoint 단발 GET. 자동 재시도/redirect/메시지/명령 등록이 없습니다.
    $response=$client.GetAsync('https://discord.com/api/v10/oauth2/applications/@me',[Net.Http.HttpCompletionOption]::ResponseHeadersRead).GetAwaiter().GetResult()
    $result.http_status=[int]$response.StatusCode
    if ($result.http_status -ne 200) {throw 'TRIAL_APP_HTTP_HOLD'}
    $stream=$response.Content.ReadAsStreamAsync().GetAwaiter().GetResult()
    $buffer=[byte[]]::new(65537);$count=0
    $readDeadline=[Diagnostics.Stopwatch]::StartNew()
    while ($count -lt $buffer.Length) {
        $cts=[Threading.CancellationTokenSource]::new([timespan]::FromSeconds([Math]::Max(0.1,15-$readDeadline.Elapsed.TotalSeconds)))
        try {$n=$stream.ReadAsync($buffer,$count,$buffer.Length-$count,$cts.Token).GetAwaiter().GetResult()} finally {$cts.Dispose()}
        if ($n -eq 0) {break};$count+=$n
        if ($readDeadline.Elapsed.TotalSeconds -ge 15) {throw 'APP_RESPONSE_TIMEOUT_HOLD'}
    }
    if ($count -gt 65536) {throw 'APP_RESPONSE_SIZE_HOLD'}
    $app=[Text.Encoding]::UTF8.GetString($buffer,0,$count)|ConvertFrom-Json
    [Array]::Clear($buffer,0,$buffer.Length)
    if (-not (Test-MobileTrialApplication $app ([string]$public.owners))) {throw 'APP_OR_OWNERSHIP_HOLD'}
    $app=$null
    $result.api_trial_app_matches=$true;$result.api_owner_matches=$true
    $stage='SAVE_NEW_TRIAL_DPAPI_STORE'
    if (Test-Path -LiteralPath $root) {throw 'TRIAL_ROOT_CREATED_DURING_CHECK_HOLD'}
    Save-LimitlessDiscordCredential -Root $secretRoot -Token $token -Owners ([string]$public.owners) -Guild '1563112744436244541' -Channels '1564163234338967552'|Out-Null
    $saved=Read-LimitlessDiscordCredential -Root $secretRoot
    $result.same_windows_account_readback=$true;$result.dpapi_current_user=$true
    $result.status='TRIAL_TOKEN_READY_HOST_NOT_STARTED';$result.stage='COMPLETE'
    $result.token_store_created=$true;$result.secret_root=$secretRoot
    $result|ConvertTo-Json|Set-Content -LiteralPath (Join-Path $root 'setup-result.json') -Encoding utf8
    $result|ConvertTo-Json -Compress
} catch {
    $result.stage=$stage;$result.error_type=$_.Exception.GetType().Name
    $result.status='HOLD';$result|ConvertTo-Json -Compress
    exit 2
} finally {
    $plainToken=$null
    if ($pointer -ne [IntPtr]::Zero) {[Runtime.InteropServices.Marshal]::ZeroFreeBSTR($pointer)}
    if ($null -ne $stream) {$stream.Dispose()}
    if ($null -ne $response) {$response.Dispose()}
    if ($null -ne $client) {$client.Dispose()}
    if ($null -ne $handler) {$handler.Dispose()}
    if ($null -ne $saved) {$saved.Token.Dispose()}
    if ($null -ne $token) {$token.Dispose()}
}

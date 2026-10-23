$ErrorActionPreference='Stop'
Import-Module (Join-Path $PSScriptRoot '../DiscordSecretStore.psm1') -Force
$base=[IO.Path]::GetFullPath((Join-Path $PSScriptRoot '../data/tmp'))
[IO.Directory]::CreateDirectory($base) | Out-Null
$fixture=Join-Path $base ('SecretStore_'+[Guid]::NewGuid().ToString('N'))
$root=Join-Path $fixture 'private'
$results=[Collections.Generic.List[object]]::new()
$fake='synthetic-test-value-not-a-discord-token'
$secure=ConvertTo-SecureString $fake -AsPlainText -Force
function Check([string]$Name,[scriptblock]$Body) {
    try { & $Body; $results.Add(@{test=$Name;status='PASS'}) }
    catch { $results.Add(@{test=$Name;status='FAIL';failure_kind=$_.Exception.GetType().Name}) }
}
function Require([bool]$Condition) { if (-not $Condition) { throw 'ASSERTION_FAILED' } }
try {
    Check 'missing-store-waits' { Require ((Get-LimitlessDiscordCredentialStatus $root).status -eq 'WAITING_FOR_USER_SETUP') }
    Check 'invalid-numeric-ids-rejected' {
        $denied=$false
        try { $null=Save-LimitlessDiscordCredential $root $secure 'name' '2' '3' } catch { $denied=$true }
        Require $denied
    }
    Check 'protected-path-rejected' {
        $denied=$false
        try { $null=Save-LimitlessDiscordCredential (Join-Path $fixture 'Unity') $secure '1' '2' '3' } catch { $denied=$true }
        Require $denied
    }
    Check 'save-returns-only-safe-status' {
        $answer=Save-LimitlessDiscordCredential $root $secure '1' '2' '3'
        Require ($answer -eq 'ENCRYPTED_STORE_SAVED')
    }
    Check 'stored-file-has-no-plaintext' {
        $text=[IO.File]::ReadAllText((Join-Path $root 'connection.json'))
        Require (-not $text.Contains($fake))
        $record=$text | ConvertFrom-Json
        Require ($record.protection -eq 'DPAPI_CURRENT_USER')
        Require ($record.tokenCipher -match '^01000000')
    }
    Check 'directory-acl-only-current-user' {
        $sid=[Security.Principal.WindowsIdentity]::GetCurrent().User.Value
        $acl=Get-Acl -LiteralPath $root
        Require $acl.AreAccessRulesProtected
        foreach ($rule in $acl.GetAccessRules($true,$true,[Security.Principal.SecurityIdentifier])) { Require ($rule.IdentityReference.Value -eq $sid) }
    }
    Check 'same-user-dpapi-roundtrip' {
        $got=Read-LimitlessDiscordCredential $root
        $ptr=[IntPtr]::Zero
        try {
            $ptr=[Runtime.InteropServices.Marshal]::SecureStringToBSTR($got.Token)
            Require ([Runtime.InteropServices.Marshal]::PtrToStringBSTR($ptr) -eq $fake)
        } finally {
            if ($ptr -ne [IntPtr]::Zero) { [Runtime.InteropServices.Marshal]::ZeroFreeBSTR($ptr) }
            $got.Token.Dispose()
        }
    }
    Check 'status-never-includes-secret-or-ids' {
        $status=Get-LimitlessDiscordCredentialStatus $root
        Require ($status.status -eq 'ENCRYPTED_STORE_READY')
        $json=$status | ConvertTo-Json -Compress
        Require (-not $json.Contains($fake))
        Require (-not $json.Contains('owners'))
    }
    Check 'overwrite-requires-explicit-replace' {
        $denied=$false
        try { $null=Save-LimitlessDiscordCredential $root $secure '1' '2' '3' } catch { $denied=$true }
        Require $denied
    }
    Check 'corrupted-encrypted-store-fails-closed' {
        [IO.File]::WriteAllText((Join-Path $root 'connection.json'),'{}')
        Require ((Get-LimitlessDiscordCredentialStatus $root).status -eq 'LOCAL_STORE_UNAVAILABLE')
    }
    Check 'explicit-credential-replacement-recovers' {
        $null=Save-LimitlessDiscordCredential $root $secure '1' '2' '3' -Replace
        Require ((Get-LimitlessDiscordCredentialStatus $root).status -eq 'ENCRYPTED_STORE_READY')
    }
} finally {
    $secure.Dispose()
    # 이 테스트가 만든 고유 디렉터리만 삭제하며 실제 등록 폴더는 건드리지 않습니다.
    if (Test-Path -LiteralPath $fixture) {
        $resolved=(Resolve-Path -LiteralPath $fixture).Path
        if ([IO.Path]::GetDirectoryName($resolved) -ne $base -or [IO.Path]::GetFileName($resolved) -notlike 'SecretStore_*') { throw 'CLEANUP_PATH_REJECTED' }
        Remove-Item -LiteralPath $resolved -Recurse -Force
    }
}
$failed=@($results | Where-Object status -ne 'PASS').Count
$report=@{mode='REAL_WINDOWS_DPAPI_SYNTHETIC_SECRET_ONLY';run=$results.Count;failed=$failed;tests=$results;network_calls=0;actual_bot_token_used=$false}
$outputPath=Join-Path $PSScriptRoot '../data/phase2/secret-store-tests.json'
[IO.File]::WriteAllText($outputPath,($report | ConvertTo-Json -Depth 5),[Text.UTF8Encoding]::new($false))
$report | ConvertTo-Json -Depth 5
if ($failed -gt 0) { exit 1 }

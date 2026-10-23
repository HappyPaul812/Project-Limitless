# Windows 사용자 DPAPI와 전용 ACL로 암호화 설정만 보관합니다. 평문 출력 함수는 없습니다.
Set-StrictMode -Version Latest

function Get-LimitlessStorePath {
    param([Parameter(Mandatory)][string]$Root)
    if ([Environment]::OSVersion.Platform -ne [PlatformID]::Win32NT) { throw 'WINDOWS_REQUIRED' }
    $full = [IO.Path]::GetFullPath($Root)
    if (($full -split '[\\/]') | Where-Object { $_ -in @('Unity','Assets','UserData','Saves','ThirdParty','.git') }) { throw 'PROTECTED_PATH' }
    $part = $full
    while ($part) {
        if (Test-Path -LiteralPath $part) {
            if ((Get-Item -LiteralPath $part -Force).Attributes -band [IO.FileAttributes]::ReparsePoint) { throw 'LINK_PATH_REJECTED' }
        }
        $parent = [IO.Directory]::GetParent($part)
        if ($null -eq $parent) { break }
        $part = $parent.FullName
    }
    return (Join-Path $full 'connection.json')
}

function Assert-LimitlessIds {
    param([string]$Owners,[string]$Guild,[string]$Channels)
    foreach ($text in @($Owners,$Guild,$Channels)) {
        if ([string]::IsNullOrWhiteSpace($text)) { throw 'NUMERIC_IDS_REQUIRED' }
        foreach ($entry in ($text -split ',')) {
            [UInt64]$numeric = 0
            if ($entry -notmatch '^\d+$' -or -not [UInt64]::TryParse($entry,[ref]$numeric) -or $numeric -eq 0) { throw 'NUMERIC_IDS_REQUIRED' }
        }
    }
    if ($Guild.Contains(',')) { throw 'ONE_GUILD_REQUIRED' }
}

function Assert-LimitlessPrivateAcl {
    param([string]$Path)
    $sid = [Security.Principal.WindowsIdentity]::GetCurrent().User
    $acl = Get-Acl -LiteralPath $Path
    if (-not $acl.AreAccessRulesProtected -or $acl.GetOwner([Security.Principal.SecurityIdentifier]).Value -ne $sid.Value) { throw 'PRIVATE_ACL_REQUIRED' }
    $rules = @($acl.GetAccessRules($true,$true,[Security.Principal.SecurityIdentifier]))
    if ($rules.Count -eq 0) { throw 'PRIVATE_ACL_REQUIRED' }
    foreach ($rule in $rules) {
        if ($rule.IdentityReference.Value -ne $sid.Value -or $rule.AccessControlType -ne 'Allow') { throw 'PRIVATE_ACL_REQUIRED' }
    }
}

function Save-LimitlessDiscordCredential {
    param([string]$Root,[Security.SecureString]$Token,[string]$Owners,[string]$Guild,[string]$Channels,[switch]$Replace)
    $path = Get-LimitlessStorePath $Root
    Assert-LimitlessIds $Owners $Guild $Channels
    if ($null -eq $Token -or $Token.Length -lt 1 -or $Token.Length -gt 4096) { throw 'TOKEN_REQUIRED' }
    if ((Test-Path -LiteralPath $path) -and -not $Replace) { throw 'STORE_EXISTS_REQUIRES_REPLACE' }
    if ((Test-Path -LiteralPath $path) -and ((Get-Item -LiteralPath $path -Force).Attributes -band [IO.FileAttributes]::ReparsePoint)) { throw 'LINK_PATH_REJECTED' }
    $folder = [IO.Path]::GetDirectoryName($path)
    [IO.Directory]::CreateDirectory($folder) | Out-Null
    $alreadyPrivate = $false
    try { Assert-LimitlessPrivateAcl $folder; $alreadyPrivate = $true } catch { }
    if (-not $alreadyPrivate) {
        $sid = [Security.Principal.WindowsIdentity]::GetCurrent().User
        # 기존 보안 기술자에서 DACL만 바꾸며, 이미 전용 ACL이면 불필요하게 다시 쓰지 않습니다.
        $acl = Get-Acl -LiteralPath $folder -ErrorAction Stop
        if ($acl.GetOwner([Security.Principal.SecurityIdentifier]).Value -ne $sid.Value) { $acl.SetOwner($sid) }
        $acl.SetAccessRuleProtection($true,$false)
        foreach ($existingRule in @($acl.GetAccessRules($true,$false,[Security.Principal.SecurityIdentifier]))) { $acl.RemoveAccessRuleSpecific($existingRule) }
        $rule = [Security.AccessControl.FileSystemAccessRule]::new($sid,'FullControl','ContainerInherit,ObjectInherit','None','Allow')
        $acl.AddAccessRule($rule)
        Set-Acl -LiteralPath $folder -AclObject $acl -ErrorAction Stop
    }
    Assert-LimitlessPrivateAcl $folder
    # -Key/-AsPlainText를 사용하지 않아 Windows CurrentUser DPAPI가 적용됩니다.
    $cipher = ConvertFrom-SecureString -SecureString $Token
    $record = @{ version=1; protection='DPAPI_CURRENT_USER'; owners=$Owners; guild=$Guild; channels=$Channels; tokenCipher=$cipher }
    $temporary = Join-Path $folder ([Guid]::NewGuid().ToString('N')+'.tmp')
    try {
        [IO.File]::WriteAllText($temporary,($record | ConvertTo-Json -Compress),[Text.UTF8Encoding]::new($false))
        Move-Item -LiteralPath $temporary -Destination $path -Force:$Replace -ErrorAction Stop
    } finally {
        if (Test-Path -LiteralPath $temporary) { Remove-Item -LiteralPath $temporary }
    }
    return 'ENCRYPTED_STORE_SAVED'
}

function Read-LimitlessDiscordCredential {
    param([string]$Root)
    $path = Get-LimitlessStorePath $Root
    if (-not (Test-Path -LiteralPath $path -PathType Leaf)) { throw 'WAITING_FOR_USER_SETUP' }
    if ((Get-Item -LiteralPath $path -Force).Attributes -band [IO.FileAttributes]::ReparsePoint) { throw 'LINK_PATH_REJECTED' }
    Assert-LimitlessPrivateAcl ([IO.Path]::GetDirectoryName($path))
    $fileAcl = Get-Acl -LiteralPath $path
    $sid = [Security.Principal.WindowsIdentity]::GetCurrent().User.Value
    foreach ($rule in $fileAcl.GetAccessRules($true,$true,[Security.Principal.SecurityIdentifier])) {
        if ($rule.IdentityReference.Value -ne $sid -or $rule.AccessControlType -ne 'Allow') { throw 'PRIVATE_ACL_REQUIRED' }
    }
    try {
        $record = [IO.File]::ReadAllText($path) | ConvertFrom-Json
        if ($record.version -ne 1 -or $record.protection -ne 'DPAPI_CURRENT_USER' -or $record.tokenCipher.Length -gt 32768) { throw 'INVALID' }
        Assert-LimitlessIds $record.owners $record.guild $record.channels
        $secure = ConvertTo-SecureString -String $record.tokenCipher
        if ($secure.Length -eq 0) { $secure.Dispose(); throw 'INVALID' }
        return [PSCustomObject]@{ Token=$secure; Owners=$record.owners; Guild=$record.guild; Channels=$record.channels }
    } catch { throw 'LOCAL_STORE_UNREADABLE_OR_INVALID' }
}

function Get-LimitlessDiscordCredentialStatus {
    param([string]$Root)
    $secureRecord = $null
    try {
        $secureRecord = Read-LimitlessDiscordCredential $Root
        return @{ status='ENCRYPTED_STORE_READY'; protection='DPAPI_CURRENT_USER'; secret_displayed=$false }
    } catch {
        if ($_.Exception.Message -eq 'WAITING_FOR_USER_SETUP') { return @{ status='WAITING_FOR_USER_SETUP'; secret_displayed=$false } }
        return @{ status='LOCAL_STORE_UNAVAILABLE'; secret_displayed=$false }
    } finally { if ($null -ne $secureRecord) { $secureRecord.Token.Dispose() } }
}

Export-ModuleMember -Function Save-LimitlessDiscordCredential,Read-LimitlessDiscordCredential,Get-LimitlessDiscordCredentialStatus

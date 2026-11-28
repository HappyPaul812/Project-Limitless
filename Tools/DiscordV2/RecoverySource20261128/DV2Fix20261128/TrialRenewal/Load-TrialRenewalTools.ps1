# 비공개 Worker 값은 관리자 보호 저장소에서만 읽습니다.
$trialRenewalPolicyPath='F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\Tools\DiscordV2\TrialRenewal\TrialRenewalPolicy.ps1'
$trialRenewalNativePath='F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\Tools\DiscordV2\TrialRenewal\TrialRenewalStore.cs'
$trialRenewalStoreBase=Join-Path $PSScriptRoot 'StoreBase.cs'
$trialRenewalBindingReader=Join-Path $PSScriptRoot '../TaskPrep/Read-TaskWorkerBinding.ps1'
$trialRenewalThumb='437423567AD4B703578F231E34A06592B38DB4EB'
$trialRenewalDer='7dba5310d6ded0035595e5e7ab8b8b73422f2925b49c633426995c322940672c'
$trialRenewalSpki='2a6d0a7d31e7642df209b6cbe12fa628500663f540eb6bd557cac2aca2d31f7c'
function Assert-RenewalToolPin([string]$path,[string]$expected) {
    if ((Get-FileHash -LiteralPath $path).Hash -cne $expected) {throw 'HOLD_RENEWAL_TOOL_PIN'}
}
Assert-RenewalToolPin $trialRenewalPolicyPath '26EC288B2DE613EF09764BD2B48F6B4DCDD624A3D179257F7AC02CEE977FEC24'
Assert-RenewalToolPin $trialRenewalNativePath 'FE35FADFE42F549D992C94053D017811EC8D1461AB550889D642135755B68C76'
Assert-RenewalToolPin $trialRenewalStoreBase 'DED6E1E60BDF77628C5529AF0761DE6E38DB51EAA12ECC86DC6222BCCBD9C497'
Assert-RenewalToolPin $trialRenewalBindingReader 'DECB4BFF132174D1F956F69BCE59BF0031EC0959CCCD560BF8EA2BCEB0F0B23C'
. $trialRenewalPolicyPath
if (!('LimitlessTrialRenewal20261128+Store' -as [type])) {
    Add-Type -Path $trialRenewalStoreBase,$trialRenewalNativePath -CompilerOptions '/define:INSTALLER_SIGNING' -ErrorAction Stop
}
function Open-ApprovedTrialRenewalStore {
    Import-Module -Name (Join-Path $env:windir 'System32/WindowsPowerShell/v1.0/Modules/Microsoft.PowerShell.LocalAccounts/1.0.0.0/Microsoft.PowerShell.LocalAccounts.psd1') -ErrorAction Stop
    $actor=[Security.Principal.WindowsIdentity]::GetCurrent()
    try {
        $expected=[Security.Principal.NTAccount]::new([Environment]::MachineName,'windows').Translate([Security.Principal.SecurityIdentifier])
        if (!$actor.User.Equals($expected) -or !([Security.Principal.WindowsPrincipal]::new($actor)).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)) {throw 'HOLD_RENEWAL_ADMIN'}
        $snapshot=@(& $trialRenewalBindingReader)
        if ($snapshot[0] -cne 'PRODUCT_NATIVE_BINDING_LOCAL_VALUES_VERIFIED_ORIGINAL_RECEIPT_UNVERIFIED') {throw 'HOLD_RENEWAL_BINDING'}
        $binding=$snapshot[-1]|ConvertFrom-Json
        [pscustomobject]@{Binding=$binding;Store=[LimitlessTrialRenewal20261128+Store]::new($actor.User.Value,$binding.Worker)}
    } finally {$actor.Dispose()}
}

$ErrorActionPreference = 'Stop'
$actor = [Security.Principal.WindowsIdentity]::GetCurrent()
if (-not ([Security.Principal.WindowsPrincipal]::new($actor)).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)) {
    throw 'HOLD_ADMIN_REQUIRED'
}
$approvedOperator = [Security.Principal.NTAccount]::new([Environment]::MachineName,'windows').Translate([Security.Principal.SecurityIdentifier])
if (-not $actor.User.Equals($approvedOperator)) { throw 'HOLD_OPERATOR_TOKEN' }
$dll = Join-Path $PSScriptRoot '../../WorkerBindingPreparation-00f284ae/WorkerBindingAdapter.dll'
$adapterBytes = [IO.File]::ReadAllBytes($dll)
if ([Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($adapterBytes)) -cne 'F4EE251B70C7A3E42F5D028B99FE05FACD02F317C1A82A7B5E30061FBF024E86') { throw 'HOLD_ADAPTER_SHA' }
$auditPath = Join-Path $PSScriptRoot '../../Invoke-V2WorkerSidReadOnly.ps1'
if ((Get-FileHash -LiteralPath $auditPath).Hash -cne '610F3B921FB393D94279769AFD92CBE37B5981D75F1441F287972002234F6CC6') { throw 'HOLD_AUDIT_SHA' }
$assembly = [Reflection.Assembly]::Load($adapterBytes)
$type = $assembly.GetType('LimitlessVerifierStorePreparation').GetNestedType('InstallApprovalStore')
$now = [DateTimeOffset]::UtcNow
$store = [Activator]::CreateInstance($type,[object[]]@($actor.User.Value,$now,$now.AddMinutes(5)))
try {
    $name = 'WorkerBinding.v1.json'
    $stream = $store.Open($name)
    try {
        $receipt = $store.Receipt($stream,$name)
        $baselinePath = Join-Path ([Environment]::GetFolderPath('LocalApplicationData')) 'Limitless-DiscordV2-WorkerBinding-Baseline-20261128.json'
        $baselineItem = Get-Item -LiteralPath $baselinePath -Force
        if ($baselineItem.Attributes -band [IO.FileAttributes]::ReparsePoint) { throw 'HOLD_BASELINE_REPARSE' }
        $baselineAcl = Get-Acl -LiteralPath $baselinePath
        $trusted = @($actor.User.Value,'S-1-5-18','S-1-5-32-544')
        if ($baselineAcl.GetOwner([Security.Principal.SecurityIdentifier]).Value -notin $trusted) { throw 'HOLD_BASELINE_OWNER' }
        $write = [Security.AccessControl.FileSystemRights]'WriteData,AppendData,WriteAttributes,WriteExtendedAttributes,Delete,ChangePermissions,TakeOwnership'
        foreach ($ace in $baselineAcl.GetAccessRules($true,$true,[Security.Principal.SecurityIdentifier])) {
            if ($ace.AccessControlType -eq 'Allow' -and ($ace.FileSystemRights -band $write) -and $ace.IdentityReference.Value -notin $trusted) {
                throw 'HOLD_BASELINE_WRITABLE_BY_OTHER'
            }
        }
        $baselineStream = [IO.File]::Open($baselinePath,[IO.FileMode]::Open,[IO.FileAccess]::Read,[IO.FileShare]::Read)
        try {
            if ($baselineStream.Length -le 0 -or $baselineStream.Length -gt 16384) { throw 'HOLD_BASELINE_LENGTH' }
            $baselineBytes = [byte[]]::new($baselineStream.Length)
            $baselineStream.ReadExactly($baselineBytes)
        } finally { $baselineStream.Dispose() }
        $baseline = [Text.Encoding]::UTF8.GetString($baselineBytes) | ConvertFrom-Json
        $expected = $baseline.WorkerBindingSha256
        if ($baseline.Schema -cne 'LIMITLESS_V2_WORKER_BINDING_LOCAL_BASELINE_V1' -or
            $baseline.OriginalReceiptStatus -cne 'UNVERIFIED' -or $baseline.WorkerDisabled -ne $true -or
            $baseline.BindingFile -cne $name -or $baseline.HandleReceipt -cne $receipt -or
            $expected -notmatch '\A[0-9a-f]{64}\z') { throw 'HOLD_BASELINE_NOT_MATCHED' }
        if ($stream.Length -le 0 -or $stream.Length -gt 4096) { throw 'HOLD_BINDING_LENGTH' }
        $bytes = [byte[]]::new($stream.Length)
        $stream.ReadExactly($bytes)
        if ([Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($bytes)) -ine $expected) { throw 'HOLD_BINDING_SHA' }
        $binding = [Text.Encoding]::UTF8.GetString($bytes) | ConvertFrom-Json
        if ($binding.Schema -cne 'LIMITLESS_V2_WORKER_BINDING_V1' -or $binding.Scope -cne 'BINDING_ONLY' -or
            $binding.TaskPath -cne '\LIMITLESS-V2-FAKE-ONLY' -or
            $binding.CodeRoot -cne 'C:\ProgramData\Limitless\DiscordV2\Code' -or
            $binding.EvidenceRoot -cne 'C:\ProgramData\Limitless\DiscordV2\Evidence') { throw 'HOLD_BINDING_SCOPE' }
        $boundSid = [Security.Principal.SecurityIdentifier]::new($binding.WorkerSid)
        $user = Get-LocalUser -SID $boundSid
        if ($user.Name -cne 'LimitlessV2Worker') { throw 'HOLD_WORKER_STATE' }

        if ($receipt -cne $store.Receipt($stream,$name)) { throw 'HOLD_BINDING_CHANGED' }
        'PRODUCT_NATIVE_BINDING_LOCAL_VALUES_VERIFIED_ORIGINAL_RECEIPT_UNVERIFIED'
        'LOCAL ONLY. Share the status line, not the values below.'
        [ordered]@{
            Worker = $boundSid.Value
            TaskPath = $binding.TaskPath
            CodeRoot = $binding.CodeRoot
            EvidenceRoot = $binding.EvidenceRoot
            BaselineMatch = $true
            WorkerDisabled = !$user.Enabled
            OriginalReceiptStatus = 'UNVERIFIED'
        } | ConvertTo-Json
    } finally { $stream.Dispose() }
} finally { $store.Dispose(); $actor.Dispose() }

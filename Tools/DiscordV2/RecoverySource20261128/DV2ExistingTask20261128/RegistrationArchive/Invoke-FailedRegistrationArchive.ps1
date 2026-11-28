#Requires -Version 7.5
$ErrorActionPreference='Stop'
$taskPrep='F:\study\codex\Project-Limitless\Temp\DV2Fix20261128\TaskPrep'
$runtime='C:\ProgramData\Limitless\DiscordV2'
$code=Join-Path $runtime 'Code'
$evidence=Join-Path $runtime 'Evidence'
$control=Join-Path $runtime 'Control'
$pending=Join-Path $control 'Approvals\PendingOperations'
$ledger=Join-Path $control 'DeploymentLedger'
$localAccounts=Join-Path $env:windir 'System32/WindowsPowerShell/v1.0/Modules/Microsoft.PowerShell.LocalAccounts/1.0.0.0/Microsoft.PowerShell.LocalAccounts.psd1'
Import-Module -Name $localAccounts -ErrorAction Stop
function Assert-ExistingTaskFingerprint {
    $actual=@(& (Join-Path $PSScriptRoot '../Inspection/Read-ExistingTaskProductFingerprint.ps1'))
    if($actual -cnotcontains 'TASK_FINGERPRINT_SHA256=38808125a5b7df12165cb5565999c03ea6ca117d48cadccd47502b9c9cd84cc1'){throw 'HOLD_EXISTING_TASK_FINGERPRINT_CHANGED'}
}
Assert-ExistingTaskFingerprint
$state=(& (Join-Path $taskPrep 'Read-V2TaskRegistrationFailure.ps1'))|ConvertFrom-Json
if(!$state.TaskPresent -or !$state.WorkerDisabled -or !$state.RegisterIntentPresent -or $state.RegisteredReceiptPresent -or
    $state.HoldRecords -ne 1 -or !$state.RegisterApprovalExpired -or $state.RemainingProductProcesses -ne 0 -or
    $state.UnresolvedProcessPaths -ne 0){throw 'HOLD_REGISTER_RECOVERY_UNEXPECTED_STATE'}
. 'F:\study\codex\Project-Limitless\Temp\DV2Fix20261128\TrialRenewal\Load-TrialRenewalTools.ps1'
$session=Open-ApprovedTrialRenewalStore
try {
    $cert=Get-Item -LiteralPath "Cert:\LocalMachine\Root\$trialRenewalThumb"
    try {
        $rsa=[Security.Cryptography.X509Certificates.RSACertificateExtensions]::GetRSAPublicKey($cert)
        try {
            if(!$rsa -or (Get-TrialRenewalSha $cert.RawData) -cne $trialRenewalDer){throw 'HOLD_REGISTRATION_CERT_PIN'}
            $head=Resolve-ProtectedTrialHead $session.Store $session.Binding $rsa $trialRenewalSpki ([DateTimeOffset]::UtcNow)
            $prefix=$head.Identity.TrialId
            $names=@("$prefix.register-only.approval.json",'FakeOnlyExecution.v2.register-only.json','FakeOnlyExecution.v2.register-only.sig')
            function Read-ScopedFile([string]$p,[int]$limit) {
                $item=Get-Item -LiteralPath $p -Force
                if ($item.PSIsContainer -or ($item.Attributes -band [IO.FileAttributes]::ReparsePoint)){
                    throw 'HOLD_REGISTER_RECOVERY_FILE_TYPE'
                }
                $file=[IO.File]::Open($p,'Open','Read','Read')
                Read-TrialRenewalStream $file $limit
            }
            $files=@{}
            foreach($name in $names){
                $raw=Read-ScopedFile (Join-Path $code $name) 4096
                $original=Read-ScopedFile (Join-Path $pending $name) 4096
                if((Get-TrialRenewalSha $raw) -cne (Get-TrialRenewalSha $original)){throw 'HOLD_REGISTER_RECOVERY_SIDECAR_PAIR'}
                $files[$name]=$raw
            }
            $op=Read-TrialRenewalJson $files[$names[0]]
            $envelope=Read-TrialRenewalJson $files[$names[1]]
            if(!$rsa.VerifyData($files[$names[1]],$files[$names[2]],[Security.Cryptography.HashAlgorithmName]::SHA256,
                [Security.Cryptography.RSASignaturePadding]::Pss) -or $op.TrialId -cne $prefix -or $op.ContractSha -cne $head.TrialSha -or
                $op.Operation -cne '--register-only' -or $op.ApprovalId -cnotmatch '^(?!0{32}$)[0-9a-f]{32}$' -or
                $envelope.TrialContractSha256 -cne $head.TrialSha -or $envelope.OperationApprovalSha256 -cne (Get-TrialRenewalSha $files[$names[0]]) -or
                [DateTimeOffset]::UtcNow -lt (Read-TrialRenewalUtc $op.ExpiresUtc)){throw 'HOLD_REGISTER_RECOVERY_OPERATION_BINDING'}
            $intentBytes=Read-ScopedFile (Join-Path $evidence "$prefix.admin.register-intent") 4096
            $intent=Read-TrialRenewalJson $intentBytes
            $holds=@(Get-ChildItem -LiteralPath $evidence -Force -File |
                Where-Object Name -Match ('^'+[regex]::Escape($prefix)+'\.admin\.hold-[0-9a-f]{32}$'))
            if($holds.Count -ne 1){throw 'HOLD_REGISTER_RECOVERY_HOLD_COUNT'}
            $failedBytes=Read-ScopedFile $holds[0].FullName 4096
            $failed=Read-TrialRenewalJson $failedBytes
            foreach($r in $intent,$failed){
                if($r.TrialId -cne $prefix -or $r.ContractSha -cne $head.TrialSha -or $r.ApprovalId -cne $op.ApprovalId){
                    throw 'HOLD_REGISTER_RECOVERY_RECORD_BINDING'
                }
            }
            if($intent.Status -cne 'REGISTER_INTENT' -or $failed.Status -cne 'HOLD'){throw 'HOLD_REGISTER_RECOVERY_STATUS'}
            $consumed=Read-ScopedFile (Join-Path $ledger ($op.ApprovalId+'.deploy-consumed')) 512
            $nl=[string][char]10
            if([Text.Encoding]::UTF8.GetString($consumed) -cne ($op.ApprovalId+$nl+(Get-TrialRenewalSha $files[$names[1]])+$nl)){
                throw 'HOLD_REGISTER_RECOVERY_CONSUMPTION'
            }
            $identity=[Security.Principal.WindowsIdentity]::GetCurrent()
            try { $actorSid=$identity.User.Value } finally {$identity.Dispose()}
            $storeSource=Join-Path $PSScriptRoot 'Store.cs'
            $archiveSource=Join-Path $PSScriptRoot 'Archive.cs'
            if((Get-FileHash -LiteralPath $storeSource).Hash -cne '341BC053754973DFA88D915B87BF12A8CCD1518574A3FAAC911C75DB2807CEA9' -or
                (Get-FileHash -LiteralPath $archiveSource).Hash -cne '49AB5C53CF3CDE9B4C16221F62F4AEDEDC84C02E5F4938BA3C0BD8280BC700D6'){throw 'HOLD_REG_ARCHIVE_SOURCE_PINS'}
            Add-Type -Path $storeSource,$archiveSource -CompilerOptions '/define:INSTALLER_SIGNING'
            $archive=[LimitlessFailedRegistrationArchive20261128+Archive]::new($actorSid,$session.Binding.Worker)
            try {
                $ledgerFile=$archive.OpenLedger($op.ApprovalId)
                try {
                    $ledgerSha=Get-TrialRenewalSha $consumed
                    function Assert-LedgerReadback {
                        $ledgerFile.Position=0
                        $current=[byte[]]::new([int]$ledgerFile.Length)
                        $ledgerFile.ReadExactly($current)
                        if((Get-TrialRenewalSha $current) -cne $ledgerSha){throw 'HOLD_REG_ARCHIVE_LEDGER_CHANGED'}
                    }
                    Assert-LedgerReadback
                    $plan=@(
                        foreach($area in 'Code','Pending'){
                            foreach($name in $names){
                                $sha=Get-TrialRenewalSha $files[$name]
                                [ordered]@{Area=$area;Name=$name;Sha256=$sha;Identity=$archive.Snapshot($area,$name,$sha)}
                            }
                        }
                        $intentHash=Get-TrialRenewalSha $intentBytes
                        $holdHash=Get-TrialRenewalSha $failedBytes
                        [ordered]@{Area='Evidence';Name="$prefix.admin.register-intent";Sha256=$intentHash
                            Identity=$archive.Snapshot('Evidence',"$prefix.admin.register-intent",$intentHash)}
                        [ordered]@{Area='Evidence';Name=$holds[0].Name;Sha256=$holdHash
                            Identity=$archive.Snapshot('Evidence',$holds[0].Name,$holdHash)}
                    )
                    'FAILED_REGISTRATION_ARCHIVE_SCOPE_AND_LEDGER_VERIFIED'
                    if((Read-Host 'B 승인 확인 후 ARCHIVE-EXISTING-TASK-FAILED-INPUTS 입력') -cne 'ARCHIVE-EXISTING-TASK-FAILED-INPUTS'){
                        throw 'HOLD_REG_ARCHIVE_APPROVAL'
                    }
                    $after=(& (Join-Path $taskPrep 'Read-V2TaskRegistrationFailure.ps1'))|ConvertFrom-Json
                    if(!$after.TaskPresent -or !$after.WorkerDisabled -or $after.RemainingProductProcesses -ne 0 -or
                        $after.UnresolvedProcessPaths -ne 0 -or $after.RegisteredReceiptPresent -or $after.HoldRecords -ne 1){
                        throw 'HOLD_REG_ARCHIVE_STATE_CHANGED'
                    }
                    $record=[ordered]@{
                        Profile='DISCORD_V2_FAILED_REGISTRATION_ARCHIVE_V1';ObservedUtc=[DateTimeOffset]::UtcNow.ToString('o')
                        TrialSha256=$head.TrialSha;ApprovalId=$op.ApprovalId;InstalledApprovalSha256=(Get-TrialRenewalSha $files[$names[0]])
                        LedgerSha256=$ledgerSha;Files=$plan
                    }
                    Assert-ExistingTaskFingerprint
                    $archive.Begin([Text.UTF8Encoding]::new($false).GetBytes(($record|ConvertTo-Json -Depth 8)))
                    foreach($step in $plan){$archive.Move($step.Area,$step.Name,$step.Sha256,$step.Identity)}
                    Assert-LedgerReadback
                    $final=(& (Join-Path $taskPrep 'Invoke-V2TaskPrepared.ps1') -Mode Inspect)|ConvertFrom-Json
                    if(!$final.TaskPresent -or !$final.WorkerDisabled -or $final.RemainingProductProcesses -ne 0 -or
                        $final.UnresolvedProcessPaths -ne 0){throw 'HOLD_REG_ARCHIVE_FINAL_STATE'}
                    Assert-ExistingTaskFingerprint
                    'FAILED_REGISTRATION_EVIDENCE_AND_SIDECARS_ARCHIVED_LEDGER_UNCHANGED'
                } finally {$ledgerFile.Dispose()}
            } finally {$archive.Dispose()}
        } finally {if($rsa){$rsa.Dispose()}}
    } finally {$cert.Dispose()}
} finally {$session.Store.Dispose()}

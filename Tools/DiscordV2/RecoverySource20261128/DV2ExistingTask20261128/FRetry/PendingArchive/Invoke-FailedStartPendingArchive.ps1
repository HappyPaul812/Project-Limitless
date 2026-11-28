#Requires -Version 7.5
param([switch]$ArchiveAfterExplicitApproval)
$ErrorActionPreference='Stop'
$root='C:\ProgramData\Limitless\DiscordV2'
$prep=Join-Path $PSScriptRoot '../../../DV2Fix20261128/TaskPrep'
. (Join-Path $PSScriptRoot '../Renewal/Load-TrialRenewalTools.ps1')
$status=& (Join-Path $prep 'Invoke-V2TaskPrepared.ps1') -Mode Inspect|ConvertFrom-Json
if(!$status.WorkerDisabled -or !$status.TaskPresent -or !$status.Task.Disabled -or $status.Task.Instances -ne 0 -or
    $status.RemainingProductProcesses -ne 0 -or $status.UnresolvedProcessPaths -ne 0 -or
    @($status.Evidence.PSObject.Properties|Where-Object Value).Count -ne 0){throw 'HOLD_FAILED_START_NOT_IDLE'}
function Assert-Absent([string]$path){
    try{$null=[IO.File]::GetAttributes($path)}catch [IO.FileNotFoundException]{return}
    throw 'HOLD_FAILED_START_TARGET_PRESENT'
}
$session=Open-ApprovedTrialRenewalStore
try {
    $cert=Get-Item -LiteralPath "Cert:\LocalMachine\Root\$trialRenewalThumb"
    try {
        $rsa=[Security.Cryptography.X509Certificates.RSACertificateExtensions]::GetRSAPublicKey($cert)
        try {
            if(!$rsa -or (Get-TrialRenewalSha $cert.RawData) -cne $trialRenewalDer){throw 'HOLD_FAILED_START_CERT'}
            $head=Resolve-ProtectedTrialHead $session.Store $session.Binding $rsa $trialRenewalSpki ([DateTimeOffset]::UtcNow)
            $names=@(($head.Identity.TrialId+'.start-once.approval.json'),'FakeOnlyExecution.v2.json','FakeOnlyExecution.v2.sig')
            $pending=Join-Path $root 'Control/Approvals/PendingOperations'
            $raw=@{}
            foreach($name in $names){
                Assert-Absent (Join-Path $session.Binding.CodeRoot $name)
                $raw[$name]=Read-TrialRenewalStream ([IO.File]::Open((Join-Path $pending $name),'Open','Read','Read')) 4096
            }
            $operation=Read-TrialRenewalJson $raw[$names[0]]
            $contract=Read-TrialRenewalJson $raw[$names[1]]
            $expiry=[DateTimeOffset]::ParseExact($contract.ExpiresUtc,'o',[Globalization.CultureInfo]::InvariantCulture)
            if(!$rsa.VerifyData($raw[$names[1]],$raw[$names[2]],[Security.Cryptography.HashAlgorithmName]::SHA256,[Security.Cryptography.RSASignaturePadding]::Pss) -or
                $contract.TrialContractSha256 -cne $head.TrialSha -or $operation.ContractSha -cne $head.TrialSha -or
                $contract.OperationApprovalSha256 -cne (Get-TrialRenewalSha $raw[$names[0]]) -or
                $operation.TrialId -cne $head.Identity.TrialId -or $operation.Operation -cne '--start-once' -or
                $operation.ApprovalId -cnotmatch '^(?!0{32}$)[a-f0-9]{32}$' -or
                $expiry.Offset -ne [TimeSpan]::FromHours(9) -or $expiry -gt [DateTimeOffset]::UtcNow){throw 'HOLD_FAILED_START_APPROVAL_BINDING'}
            $ledger=Join-Path $root ('Control/DeploymentLedger/'+$operation.ApprovalId+'.deploy-consumed')
            Assert-Absent $ledger
            $source=Join-Path $PSScriptRoot 'Store.cs'
            $archiveSource=Join-Path $PSScriptRoot 'Archive.cs'
            if((Get-FileHash -LiteralPath $source).Hash -cne '341BC053754973DFA88D915B87BF12A8CCD1518574A3FAAC911C75DB2807CEA9' -or (Get-FileHash -LiteralPath $archiveSource).Hash -cne 'D09ECE12C367A0BF9E642820D408F92081439837815EA9A6742CBBC6B7376E9E'){
                throw 'HOLD_FAILED_START_SOURCE_PIN'
            }
            Add-Type -Path $source,$archiveSource -CompilerOptions '/define:INSTALLER_SIGNING'
            $actor=[Security.Principal.WindowsIdentity]::GetCurrent()
            try{$sid=$actor.User.Value}finally{$actor.Dispose()}
            $archive=[LimitlessFailedRegistrationArchive20261128+Archive]::new($sid,$session.Binding.Worker)
            try {
                $files=@(foreach($name in $names){
                    $sha=Get-TrialRenewalSha $raw[$name]
                    [ordered]@{Name=$name;Sha256=$sha;Identity=$archive.Snapshot('Pending',$name,$sha)}
                })
                'FAILED_START_PENDING_ARCHIVE_READINESS_VERIFIED_NO_WRITES'
                if(!$ArchiveAfterExplicitApproval){return}
                if((Read-Host '별도 보관 승인 후 ARCHIVE-FAILED-START-PENDING 입력') -cne 'ARCHIVE-FAILED-START-PENDING'){throw 'HOLD_FAILED_START_ARCHIVE_APPROVAL'}
                Assert-Absent $ledger
                foreach($name in $names){Assert-Absent (Join-Path $session.Binding.CodeRoot $name)}
                $record=[ordered]@{Profile='DISCORD_V2_FAILED_START_PENDING_ARCHIVE_V1';TrialSha256=$head.TrialSha;ApprovalId=$operation.ApprovalId;Files=$files}
                $archive.Begin([Text.UTF8Encoding]::new($false).GetBytes(($record|ConvertTo-Json -Depth 8)))
                foreach($file in $files){$archive.Move('Pending',$file.Name,$file.Sha256,$file.Identity)}
                Assert-Absent $ledger
                'FAILED_START_PENDING_FILES_ARCHIVED_NO_CONSUMPTION_NO_EXECUTION'
            }finally{$archive.Dispose()}
        }finally{if($rsa){$rsa.Dispose()}}
    }finally{$cert.Dispose()}
}finally{$session.Store.Dispose()}

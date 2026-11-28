#Requires -Version 7.0
$ErrorActionPreference='Stop'
Import-Module -Name (Join-Path $env:windir 'System32/WindowsPowerShell/v1.0/Modules/Microsoft.PowerShell.LocalAccounts/1.0.0.0/Microsoft.PowerShell.LocalAccounts.psd1') -ErrorAction Stop
$bindingOutput=@(& (Join-Path $PSScriptRoot 'Read-TaskWorkerBinding.ps1'))
if($bindingOutput[0] -cne 'PRODUCT_NATIVE_BINDING_LOCAL_VALUES_VERIFIED_ORIGINAL_RECEIPT_UNVERIFIED'){throw 'HOLD_ARCHIVE_BINDING'}
$binding=$bindingOutput[-1]|ConvertFrom-Json
$archive='C:\ProgramData\Limitless\DiscordV2\Control\FailedProductDeploymentArchive20261128'
$evidence=Join-Path $archive 'Evidence'
foreach($path in $archive,$evidence) {
    $item=Get-Item -LiteralPath $path -Force
    if(!$item.PSIsContainer -or ($item.Attributes -band [IO.FileAttributes]::ReparsePoint)){throw 'HOLD_ARCHIVE_TYPE'}
    $acl=Get-Acl -LiteralPath $path
    $sd=[Security.AccessControl.RawSecurityDescriptor]::new($acl.GetSecurityDescriptorBinaryForm(),0)
    if(!$acl.AreAccessRulesProtected -or $sd.Owner.Value -notin 'S-1-5-32-544','S-1-5-18' -or $null -eq $sd.DiscretionaryAcl){
        throw 'HOLD_ARCHIVE_DACL'
    }
    foreach($ace in $sd.DiscretionaryAcl){
        if($ace -isnot [Security.AccessControl.CommonAce] -or $ace.IsCallback){throw 'HOLD_ARCHIVE_COMPLEX_ACE'}
        if($ace.AceQualifier -ne 'AccessAllowed' -or ($ace.AceFlags -band [Security.AccessControl.AceFlags]::InheritOnly)){continue}
        $trustee=$ace.SecurityIdentifier.Value
        if($trustee -notin 'S-1-5-32-544','S-1-5-18' -and ($ace.AccessMask -band 0x500D0156)) {
            if($trustee -ceq $binding.Worker){throw 'HOLD_ARCHIVED_EVIDENCE_WORKER_WRITE_ACE'}
            throw 'HOLD_ARCHIVED_EVIDENCE_UNTRUSTED_WRITE_ACE'
        }
    }
}
'ARCHIVED_EVIDENCE_NO_UNTRUSTED_WRITE_ACE_ACTUAL_TOKEN_NOT_TESTED'

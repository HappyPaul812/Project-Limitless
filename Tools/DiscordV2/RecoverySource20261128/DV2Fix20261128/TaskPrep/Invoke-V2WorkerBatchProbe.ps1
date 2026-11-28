#Requires -Version 7.0
param([switch]$ProbeAfterExplicitApproval,[switch]$RestoreDisabledAfterExplicitApproval)
$ErrorActionPreference='Stop'
Import-Module -Name (Join-Path $env:windir 'System32/WindowsPowerShell/v1.0/Modules/Microsoft.PowerShell.LocalAccounts/1.0.0.0/Microsoft.PowerShell.LocalAccounts.psd1') -ErrorAction Stop
$state=@(& (Join-Path $PSScriptRoot 'Read-TaskWorkerBinding.ps1'))
if($state[0] -cne 'PRODUCT_NATIVE_BINDING_LOCAL_VALUES_VERIFIED_ORIGINAL_RECEIPT_UNVERIFIED'){throw 'HOLD_WORKER_BINDING'}
$binding=$state[-1]|ConvertFrom-Json
$sid=[Security.Principal.SecurityIdentifier]::new($binding.Worker)
$worker=Get-LocalUser -SID $sid
if($RestoreDisabledAfterExplicitApproval){
    if ($ProbeAfterExplicitApproval){throw 'HOLD_MULTIPLE_OPERATION_MODES'}
    if ((Read-Host '별도 원복 승인 후 RESTORE-V2-WORKER-DISABLED 입력') -cne 'RESTORE-V2-WORKER-DISABLED'){throw 'HOLD_RESTORE_APPROVAL'}
    if($worker.Enabled){Disable-LocalUser -SID $sid}
    if((Get-LocalUser -SID $sid).Enabled){throw 'HOLD_WORKER_DISABLE_READBACK'}
    'WORKER_DISABLED_READBACK_NO_TASK_OR_PROCESS_CHANGE'
    return
}
if(!$ProbeAfterExplicitApproval){
    "WORKER_DISABLED=$(!$worker.Enabled)"
    'ACTUAL_BATCH_TOKEN_NOT_TESTED'
    return
}
if($worker.Enabled){throw 'HOLD_NEEDS_DISABLED_WORKER'}
& (Join-Path $PSScriptRoot 'Test-V2ArchivedEvidenceWriteBoundary.ps1')
if((Read-Host '별도 활성화·배치 토큰 확인·원복 승인 후 PROBE-V2-BATCH-AND-DISABLE 입력') -cne 'PROBE-V2-BATCH-AND-DISABLE'){throw 'HOLD_BATCH_PROBE_APPROVAL'}
Add-Type -TypeDefinition @"
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;
using Microsoft.Win32.SafeHandles;
public static class LimitlessV2ActualBatchTokenProbe
{
    [DllImport("advapi32.dll",EntryPoint="LogonUserW",CharSet=CharSet.Unicode,SetLastError=true)]
    [return:MarshalAs(UnmanagedType.Bool)]
    private static extern bool Logon(string user,string computer,IntPtr password,int kind,int provider,out SafeAccessTokenHandle token);
    public static void Check(string name,SecureString password,string worker)
    {
        IntPtr secret=Marshal.SecureStringToGlobalAllocUnicode(password);
        try
        {
            if(!Logon(name,Environment.MachineName,secret,4,0,out var token))
                throw new Win32Exception(Marshal.GetLastWin32Error());
            using(token)
            using(var identity=new WindowsIdentity(token.DangerousGetHandle()))
            {
                var principal=new WindowsPrincipal(identity);
                if(identity.User?.Value!=worker||principal.IsInRole(WindowsBuiltInRole.Administrator)||
                    identity.Groups==null||!identity.Groups.Contains(new SecurityIdentifier("S-1-5-3")))
                    throw new InvalidOperationException("HOLD_BATCH_TOKEN_SID_OR_ROLE");
            }
        }
        finally {Marshal.ZeroFreeGlobalAllocUnicode(secret);}
    }
}
"@
$password=Read-Host '전용 Worker 암호 입력. 화면에 표시되지 않습니다' -AsSecureString
if(!$password -or $password.Length -eq 0){throw 'HOLD_EMPTY_PASSWORD'}
try {
    Enable-LocalUser -SID $sid
    [LimitlessV2ActualBatchTokenProbe]::Check($worker.Name,$password,$binding.Worker)
    'ACTUAL_BATCH_TOKEN_SID_AND_NONADMIN_BATCH_GROUP_VERIFIED'
} finally {
    $password.Dispose()
    Disable-LocalUser -SID $sid
    if((Get-LocalUser -SID $sid).Enabled){throw 'HOLD_WORKER_DISABLE_READBACK'}
    'WORKER_DISABLED_READBACK_NO_TASK_OR_PROCESS_CHANGE'
}

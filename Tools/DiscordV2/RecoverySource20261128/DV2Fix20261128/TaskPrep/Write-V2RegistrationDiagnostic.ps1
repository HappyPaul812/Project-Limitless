#Requires -Version 7.0
param([string]$Supervisor,[string]$TrialPath,[string]$TrialSha,[string]$Control,[Parameter(Mandatory)][DateTimeOffset]$Deadline)
$ErrorActionPreference='Stop'
$parent=Get-Item -LiteralPath $Control -Force
if(!$parent.PSIsContainer -or ($parent.Attributes -band [IO.FileAttributes]::ReparsePoint)){
    throw 'HOLD_REGISTRATION_DIAGNOSTIC_DIRECTORY'
}
$acl=Get-Acl -LiteralPath $Control
$admins=[Security.Principal.SecurityIdentifier]::new('S-1-5-32-544')
$system=[Security.Principal.SecurityIdentifier]::new('S-1-5-18')
$trusted=@($admins.Value,$system.Value)
if(!$acl.AreAccessRulesProtected -or $acl.GetOwner([Security.Principal.SecurityIdentifier]).Value -notin $trusted){
    throw 'HOLD_REGISTRATION_DIAGNOSTIC_DIRECTORY_ACL'
}
foreach($rule in $acl.GetAccessRules($true,$true,[Security.Principal.SecurityIdentifier])){
    if($rule.AccessControlType -eq 'Allow' -and $rule.IdentityReference.Value -notin $trusted){
        throw 'HOLD_REGISTRATION_DIAGNOSTIC_DIRECTORY_ACL'
    }
}
$security=[Security.AccessControl.FileSecurity]::new()
$security.SetOwner($admins)
$security.SetAccessRuleProtection($true,$false)
foreach($sid in $admins,$system){
    $security.AddAccessRule([Security.AccessControl.FileSystemAccessRule]::new($sid,'FullControl','Allow'))
}
$path=Join-Path $Control ('RegisterDiagnostic-'+[guid]::NewGuid().ToString('N')+'.jsonl')
$stream=[IO.FileSystemAclExtensions]::Create([IO.FileInfo]::new($path),[IO.FileMode]::CreateNew,
    [Security.AccessControl.FileSystemRights]::FullControl,[IO.FileShare]::Read,4096,[IO.FileOptions]::WriteThrough,$security)
$writer=[IO.StreamWriter]::new($stream)
$process=[Diagnostics.Process]::new()
function Write-Diagnostic([string]$Channel,[string]$Status,[Nullable[int]]$ExitCode){
    $safe=[ordered]@{Utc=[DateTimeOffset]::UtcNow.ToString('o');Channel=$Channel;Status=$Status}
    if($null -ne $ExitCode){$safe.ExitCode=$ExitCode}
    $writer.WriteLine(($safe | ConvertTo-Json -Compress))
    $writer.Flush()
    $stream.Flush($true)
}
$started=$false
try {
    $writer.AutoFlush=$true
    Write-Host "등록 진단 경로: $path"
    Write-Diagnostic 'process' 'ATTEMPT'
    $start=$process.StartInfo
    $start.FileName=$Supervisor
    $start.UseShellExecute=$false
    # stdin은 콘솔에 남겨 비표시 암호 입력을 유지합니다.
    $start.RedirectStandardOutput=$true
    $start.RedirectStandardError=$true
    foreach($argument in '--register-only','--contract',$TrialPath,'--contract-sha',$TrialSha,'--password-console'){
        $start.ArgumentList.Add($argument)
    }
    if([DateTimeOffset]::UtcNow -ge $Deadline){throw 'HOLD_REGISTER_DIAGNOSTIC_TIMEOUT'}
    $started=$process.Start()
    if(!$started){throw 'HOLD_REGISTRATION_DIAGNOSTIC_PROCESS_START'}
    $readers=@{stdout=$process.StandardOutput;stderr=$process.StandardError}
    $pending=@{stdout=$readers.stdout.ReadLineAsync();stderr=$readers.stderr.ReadLineAsync()}
    $prompt='Worker 암호를 입력하고 Enter를 누르세요. 입력은 표시되지 않습니다. Esc로 취소할 수 있습니다.'
    $maskedPrompt=$prompt.Replace('입력은 표시되지 않습니다.','입력은 -로 표시됩니다.')
    $maskLength=0
    while($pending.Count){
        $remaining=($Deadline-[DateTimeOffset]::UtcNow).TotalMilliseconds
        if($remaining -le 0 -or [Threading.Tasks.Task]::WaitAny([Threading.Tasks.Task[]]@($pending.Values),[int][Math]::Ceiling($remaining)) -lt 0){
            Write-Diagnostic 'process' 'HOLD_REGISTER_DIAGNOSTIC_TIMEOUT'
            throw 'HOLD_REGISTER_DIAGNOSTIC_TIMEOUT'
        }
        foreach($channel in @($pending.Keys)){
            $read=$pending[$channel]
            if(!$read.IsCompleted){continue}
            $line=$read.GetAwaiter().GetResult()
            if($null -eq $line){$pending.Remove($channel);continue}
            if($channel -ceq 'stdout' -and $line -ceq 'PASSWORD_MASK_ADD'){
                Write-Host '-' -NoNewline
                $maskLength++
            } elseif($channel -ceq 'stdout' -and $line -ceq 'PASSWORD_MASK_ERASE'){
                if($maskLength -gt 0){
                    Write-Host "`b `b" -NoNewline
                    $maskLength--
                }
            } elseif($channel -ceq 'stdout' -and ($line -ceq $prompt -or $line -ceq $maskedPrompt)){
                Write-Host $line
                Write-Diagnostic $channel 'PASSWORD_PROMPT_SHOWN'
            } elseif($channel -ceq 'stderr' -and $line -cmatch '^HOLD_REGISTER_(?:ADMISSION|CONSOLE|PASSWORD|FINALADMISSION|SCHEDULERNEWTASK|SCHEDULERXML|SCHEDULERREGISTER|READBACK)_(?:CANCELLED|APPROVAL_EXPIRED|CONSOLE_REQUIRED|EMPTY_PASSWORD|PASSWORD_LENGTH|REJECTED|COM_HRESULT_0x[0-9A-F]{8})$'){
                if($maskLength -gt 0){Write-Host; $maskLength=0}
                Write-Host $line
                Write-Diagnostic $channel $line
            } else {
                Write-Diagnostic $channel 'UNRECOGNIZED_OUTPUT_REDACTED'
            }
            $pending[$channel]=$readers[$channel].ReadLineAsync()
        }
    }
    if($maskLength -gt 0){Write-Host}
    $remaining=[Math]::Max(0,($Deadline-[DateTimeOffset]::UtcNow).TotalMilliseconds)
    if(!$process.WaitForExit([int][Math]::Ceiling($remaining))){
        Write-Diagnostic 'process' 'HOLD_REGISTER_DIAGNOSTIC_TIMEOUT'
        throw 'HOLD_REGISTER_DIAGNOSTIC_TIMEOUT'
    }
    Write-Diagnostic 'process' 'EXIT' $process.ExitCode
    $process.ExitCode
} finally {
    try {
        if($started -and !$process.HasExited){$process.Kill()}
    } finally {
        $process.Dispose()
        $writer.Dispose()
    }
}

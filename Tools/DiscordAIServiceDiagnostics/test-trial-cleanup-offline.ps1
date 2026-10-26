$ErrorActionPreference='Stop'
$source=Join-Path $PSScriptRoot 'cleanup-partial-trial.ps1'
$native=Join-Path $PSScriptRoot 'TrialCleanupNative.cs'
$t=$null;$e=$null;$ast=[Management.Automation.Language.Parser]::ParseFile($source,[ref]$t,[ref]$e)
if($e.Count){throw 'CLEANUP_PARSE_HOLD'}
foreach($name in @('Need-Cleanup','Hash-Cleanup','Convert-CleanupCanonical','Safe-CleanupError','Assert-CleanupPath','Assert-CleanupAcl','Assert-CleanupSnapshot','Invoke-CleanupCore','Has-CleanupReference','Read-CleanupTaskScan')){
 $n=$ast.Find({param($n)$n -is [Management.Automation.Language.FunctionDefinitionAst] -and $n.Name -eq $name},$true)
 . ([scriptblock]::Create($n.Extent.Text))
}
$code=[IO.File]::ReadAllText($native);Add-Type -TypeDefinition $code
$root='C:\Program Files\LimitlessRelayTrial'
$adminSid='S-1-5-21-2858504392-1958168942-1662906075-1001'
$workerSid='S-1-5-21-2858504392-1958168942-1662906075-1007'
$botSid='S-1-5-21-2858504392-1958168942-1662906075-1008'
$script:pass=0;$script:fail=0
function Need($v){if(-not $v){throw 'ASSERT_FAIL'}}
function Reject([scriptblock]$Body){$hit=$false;try{& $Body|Out-Null}catch{$hit=$true};Need $hit}
function Test($Name,[scriptblock]$Body){try{& $Body;$script:pass++;"PASS $Name"}catch{$script:fail++;"FAIL $Name : $($_.Exception.GetType().Name)"}}
function Snapshot {
 @{admin_sid=$adminSid;worker_sid=$workerSid;root=$root;file_count=855;unexpected=0;task_absent=$true;state_absent=$true;refs=@{hits=0;unknown=0};preserved=$true;fingerprint=('a'*64);bot=@{sid=$botSid;name='LIMITLESSBotTrial';enabled=$false;description='LIMITLESS_TRIAL_BOUNDARY_V1';expiry_utc='2026-10-27T13:41:13.2064794Z';group_sids=@('S-1-5-32-545');direct_rights=@();profile_count=0;logon_count=0;last_logon_null=$true}}
}
Test 'cleanup-source-and-native-compile' {Need ($null -ne ('LimitlessTrialCleanupV1' -as [type]))}
Test 'no-exit-statement' {Need ($null -eq $ast.Find({param($n)$n -is [Management.Automation.Language.ExitStatementAst]},$true))}
Test 'no-installer-or-task-or-process-mutation-command' {
 $names=@($ast.FindAll({param($n)$n -is [Management.Automation.Language.CommandAst]},$true)|ForEach-Object{$_.GetCommandName()})
 Need (@($names|Where-Object{$_ -match '^(Register-|Unregister-|Enable-|Disable-|Set-Acl|New-LocalUser|New-Service|Remove-Service|Start-|Stop-|Restart-|Get-Credential|Read-Host|Invoke-Expression|Remove-Item)'}).Count -eq 0)
}
Test 'account-delete-is-one-sid-only-command' {
 $nodes=@($ast.FindAll({param($n)$n -is [Management.Automation.Language.CommandAst] -and $n.GetCommandName() -eq 'Remove-LocalUser'},$true))
 Need ($nodes.Count -eq 1 -and $nodes[0].Extent.Text.Contains('-SID') -and -not $nodes[0].Extent.Text.Contains('-Name'))
}
Test 'native-has-no-policy-grant-or-task-or-kill-api' {Need ($code -notmatch 'LsaAddAccountRights|LsaRemoveAccountRights|TerminateProcess|RmShutdown|RmRestart|RegisterTaskDefinition|DeleteTask')}
Test 'native-delete-is-handle-pinned-and-directories-nonrecursive' {Need ($code.Contains('SetFileInformationByHandle(h,4') -and $code.Contains('SHA256.HashData(stream)') -and $code.Contains('Directory.Delete(path,false)') -and $code.Contains('Scoped(Root,path)'))}
Test 'backup-created-private-and-create-new' {Need ($code.Contains('O:BAG:SYD:P') -and $code.Contains('ref sa,1,0x80') -and $code.Contains('Scoped(Backup,path)'))}
Test 'exact-root-and-child-path-accepted' {Need ((Assert-CleanupPath $root) -ceq $root);Need ([LimitlessTrialCleanupV1]::Scoped($root,$root+'\Code\a.cs') -ceq ($root+'\Code\a.cs'))}
foreach($path in @('C:\Program Files\Other','C:\Program Files\LimitlessRelayTrial2','C:\Program Files\LimitlessRelayTrial\..\Other','F:\study\codex\Project-Limitless')){
 Test 'out-of-root-path-blocked' {Reject {Assert-CleanupPath $path};Reject {[LimitlessTrialCleanupV1]::Scoped($root,$path)}}
}
$sddl='O:BAG:SYD:P(A;OICI;FA;;;SY)(A;OICI;FA;;;BA)'
Test 'root-admin-system-protected-acl' {Assert-CleanupAcl $sddl $true $true}
Test 'child-file-inherited-admin-system-acl' {Assert-CleanupAcl 'O:BAG:SYD:(A;ID;FA;;;SY)(A;ID;FA;;;BA)' $false $false}
foreach($bad in @($sddl.Replace('O:BA','O:SY'),$sddl.Replace('D:P','D:'),($sddl+'(A;;GR;;;BU)'),$sddl.Replace('(A;OICI;FA;;;BA)','(A;OICI;GR;;;BA)'))){Test 'unexpected-owner-or-dacl-blocked' {Reject {Assert-CleanupAcl $bad $true $true}}}
Test 'canonical-json-independent-of-key-insertion-order' {
 $a=[ordered]@{b=@{z=2;a=1};a=@(3,4)};$b=[ordered]@{a=@(3,4);b=@{a=1;z=2}}
 Need ((Convert-CleanupCanonical $a) -ceq (Convert-CleanupCanonical $b))
}
Test 'known-long-short-and-account-references-detected' {
 $aliases=@('LimitlessRelayTrial','LIMITLESSBotTrial',$botSid,'C:\PROGRA~1\LIMITL~1')
 foreach($x in @('C:/Program Files/LimitlessRelayTrial/Code/x.py','paul-pc\limitlessbottrial','C:\PROGRA~1\LIMITL~1\Code',$botSid)){Need (Has-CleanupReference $x $aliases)}
 Need (-not (Has-CleanupReference 'C:\ProgramData\LimitlessRelay\boot.py' $aliases))
}
function Fake-TaskFolder($Tasks,$Children,[bool]$Denied=$false){
 $f=[pscustomobject]@{Tasks=$Tasks;Children=$Children;Denied=$Denied}
 $f|Add-Member ScriptMethod GetTasks {param($flags)if($this.Denied -or $flags -ne 1){throw 'SYNTHETIC_TASK_READ_DENIED'};return $this.Tasks}
 $f|Add-Member ScriptMethod GetFolders {param($flags)return $this.Children}
 return $f
}
Test 'hidden-task-tree-reference-and-unknown-xml-are-not-ignored' {
 $child=Fake-TaskFolder @([pscustomobject]@{Xml='C:\Program Files\LimitlessRelayTrial\Code\x.py'}) @()
 $parent=Fake-TaskFolder @([pscustomobject]@{Xml=''},[pscustomobject]@{Xml='ordinary'}) @($child)
 $r=Read-CleanupTaskScan $parent @('LimitlessRelayTrial');Need ($r.count -eq 3 -and $r.hits -eq 1 -and $r.unknown -eq 1)
}
Test 'unreadable-task-tree-aborts-instead-of-assuming-no-reference' {Reject {Read-CleanupTaskScan (Fake-TaskFolder @() @() $true) @('LimitlessRelayTrial')}}
Test 'service-driver-coverage-and-no-restart-manager-registry-writes' {
 $body=[IO.File]::ReadAllText($source)
 Need ($body.Contains('Win32_SystemDriver') -and $body.Contains('unknown_services') -and $code -notmatch 'RmRegisterResources|RmShutdown|RmRestart')
}
Test 'error-summary-does-not-export-secret-message' {
 $r=Safe-CleanupError ([pscustomobject]@{Exception=[Exception]::new('SYNTHETIC_PASSWORD_TOKEN',[ComponentModel.Win32Exception]::new(5))})
 $j=$r|ConvertTo-Json -Depth 6;Need ($j -notmatch 'SYNTHETIC_PASSWORD_TOKEN' -and $r.exceptions[1].win32 -eq 5)
}
Test 'valid-snapshot-accepted' {Assert-CleanupSnapshot (Snapshot)}
foreach($kind in @('sid','enabled','group','rights','expiry','task','file','unexpected','refs','unknown','profile','logon','preserved','root')){
 Test ('preflight-blocks-'+$kind) {
  $s=Snapshot
  switch($kind){sid{$s.bot.sid=$workerSid}enabled{$s.bot.enabled=$true}group{$s.bot.group_sids+=@('S-1-5-32-544')}rights{$s.bot.direct_rights=@('SeBatchLogonRight')}expiry{$s.bot.expiry_utc='changed'}task{$s.task_absent=$false}file{$s.file_count=856}unexpected{$s.unexpected=1}refs{$s.refs.hits=1}unknown{$s.refs.unknown=1}profile{$s.bot.profile_count=1}logon{$s.bot.logon_count=1}preserved{$s.preserved=$false}root{$s.root='C:\Windows'}}
  $events=[Collections.Generic.List[string]]::new()
  $effects=@{Backup={param($s)$events.Add('backup');@{verified=$true}};ReadAgain={Snapshot};DeleteCode={$events.Add('code')};CheckAccount={};DeleteAccount={$events.Add('account')};Final={}}
  Reject {Invoke-CleanupCore $s $effects};Need ($events.Count -eq 0)
 }
}
Test 'backup-recheck-deletion-final-order' {
 $events=[Collections.Generic.List[string]]::new()
 $effects=@{Backup={param($s)$events.Add('backup');@{verified=$true}};ReadAgain={$events.Add('recheck');Snapshot};DeleteCode={param($s)$events.Add('code')};CheckAccount={$events.Add('account-recheck')};DeleteAccount={$events.Add('account')};Final={$events.Add('final')}}
 Invoke-CleanupCore (Snapshot) $effects;Need (($events -join ',') -ceq 'backup,recheck,code,account-recheck,account,final')
}
Test 'backup-failure-prevents-all-deletion' {
 $events=[Collections.Generic.List[string]]::new();$effects=@{Backup={param($s)@{verified=$false}};ReadAgain={Snapshot};DeleteCode={$events.Add('code')};CheckAccount={};DeleteAccount={$events.Add('account')};Final={}}
 Reject {Invoke-CleanupCore (Snapshot) $effects};Need ($events.Count -eq 0)
}
Test 'changed-state-after-backup-prevents-deletion' {
 $events=[Collections.Generic.List[string]]::new();$effects=@{Backup={param($s)@{verified=$true}};ReadAgain={$s=Snapshot;$s.fingerprint=('b'*64);$s};DeleteCode={$events.Add('code')};CheckAccount={};DeleteAccount={$events.Add('account')};Final={}}
 Reject {Invoke-CleanupCore (Snapshot) $effects};Need ($events.Count -eq 0)
}
Test 'file-delete-failure-leaves-account-disabled-no-auto-retry' {
 $events=[Collections.Generic.List[string]]::new();$effects=@{Backup={param($s)@{verified=$true}};ReadAgain={Snapshot};DeleteCode={$events.Add('code');throw 'SYNTHETIC_DELETE_FAILURE'};CheckAccount={};DeleteAccount={$events.Add('account')};Final={}}
 Reject {Invoke-CleanupCore (Snapshot) $effects};Need (($events -join ',') -ceq 'code')
}
@{passed=$script:pass;failed=$script:fail;actual_delete_calls=0;actual_account_changes=0;actual_backup_creation=0;actual_lsa_calls=0;actual_scheduler_calls=0}|ConvertTo-Json -Compress
if($script:fail){throw 'CLEANUP_OFFLINE_HOLD'}

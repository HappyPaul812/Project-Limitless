param([Parameter(Mandatory)][string]$ManifestSha,[Parameter(Mandatory)][string]$BindingSha)
$ErrorActionPreference='Stop';$started=[Diagnostics.Process]::GetCurrentProcess().StartTime.ToUniversalTime();$lease=$null;$hostObject=$null
try{
 $fixed='C:\Program Files\LimitlessRelayDedicatedV2_20261027';if($PSScriptRoot -cne ($fixed+'\Code\V2')){throw 'D3_HOST_LOCATION'}
 # 코드는 BA 보호 설치 경로에서만 로드합니다. 공통 bootstrap은 실행 전 전체 SHA·writer를 검사합니다.
 . (Join-Path $PSScriptRoot 'DedicatedBootstrap.ps1');$c=Read-D3Installed $ManifestSha $BindingSha -WorkerContext;$m=$c.manifest;$b=$c.binding
 . (Join-Path $PSScriptRoot 'DedicatedCore.ps1')
 $me=[LimitlessSession0PipeV2]::Current();$logon=[LimitlessSession0PipeV2]::CurrentLogon()
 $aPath=Join-Path $fixed 'DedicatedIpcApproval.json';[LimitlessV2Publish]::Verify($aPath,[DedicatedContract]::FsSddl('code',$false,$b.worker_sid,$b.bot_sid));$ar=[IO.File]::ReadAllText($aPath)
 [DedicatedContract]::IpcApproval($ar,$m.epoch,$ManifestSha,$BindingSha,[datetimeoffset]::UtcNow.ToUnixTimeSeconds());$a=$ar|ConvertFrom-Json -AsHashtable
 $rights=@([DedicatedAccountPolicy]::Read($b.worker_sid)|Sort-Object);$groupList=@(Get-LocalGroup|ForEach-Object{$g=$_;if(@(Get-LocalGroupMember -SID $g.SID -ErrorAction Stop|Where-Object {$_.SID.Value -ceq $b.worker_sid}).Count){$g.SID.Value}})
 $scheduler=New-Object -ComObject Schedule.Service;$scheduler.Connect()
 function Read-D3OtherTasks($folder){foreach($t in $folder.GetTasks(1)){if($t.Path -ceq ('\'+$m.task_name)){continue};$p=$t.Definition.Principal;if([int]$p.LogonType -eq 4){if($t.Enabled -or [int]$t.State -eq 4){throw 'D3_GROUP_EFFECTIVE_TOKEN_UNKNOWN'};continue};$u=$p.UserId;if([string]::IsNullOrWhiteSpace($u)){throw 'D3_TASK_PRINCIPAL_UNKNOWN'};$sid=if($u -like 'S-1-*'){$u}else{([Security.Principal.NTAccount]::new($u)).Translate([Security.Principal.SecurityIdentifier]).Value};if($sid -ceq $b.worker_sid -and ($t.Enabled -or [int]$t.State -eq 4)){throw 'D3_OTHER_DEDICATED_LAUNCHER'}};foreach($f in $folder.GetFolders(0)){Read-D3OtherTasks $f}}
 $rightsExact=(($rights -join ',') -ceq (@([DedicatedContract]::Rights|Sort-Object)-join ','))
 # GROUP4와 보안 BATCH4를 구분합니다. 실행 가능한 GROUP의 유효 토큰을 증명하지 못하면 HOLD합니다.
 if(-not $rightsExact){throw 'D3_RIGHTS_UNKNOWN'};Read-D3OtherTasks ($scheduler.GetFolder('\'))
 $leasePath=Join-Path $m.state_root 'Control\worker.lease';[LimitlessV2Publish]::Verify($leasePath,[DedicatedContract]::FsSddl('state',$false,$b.worker_sid,''));$lease=[IO.File]::Open($leasePath,[IO.FileMode]::Open,[IO.FileAccess]::ReadWrite,[IO.FileShare]::None)
 $facts=@{worker_sid=$me.Sid;security_logon_type=$logon[0];session=$logon[1];admin=$me.Administrator;elevated=$me.Elevated;users_only=($groupList.Count -eq 1 -and $groupList[0] -ceq 'S-1-5-32-545');rights_exact=$rightsExact;credential_scope_unshared=$a.credential_scope_unshared;launcher_control_verified=$a.noncooperative_launcher_control_verified;process_generation_verified=$true;no_foreign_dedicated_processes=[LimitlessV2WorkerFence]::Clear($b.worker_sid,[Diagnostics.Process]::GetCurrentProcess().Id);no_other_dedicated_tasks=$true;codex_home=$m.codex_home;auth_empty=(@(Get-ChildItem -LiteralPath $m.codex_home -Force -ErrorAction Stop).Count -eq 0);other_auth_imported=$false}
 $null=Assert-D3RuntimeAdmission $m $b $facts
 $consume=Join-Path $m.state_root ('Control\Ledger\ipc-'+$m.package_id+'.consumed');$f=[IO.File]::Open($consume,[IO.FileMode]::CreateNew,[IO.FileAccess]::Write,[IO.FileShare]::None);try{$f.WriteByte(1);$f.Flush($true)}finally{$f.Dispose()}
 Set-D3WorkerEnvironment $m
 $remaining=[Math]::Min(90000-([datetime]::UtcNow-$started).TotalMilliseconds,([datetimeoffset]::FromUnixTimeSeconds([long]$a.expires_unix).UtcDateTime-[datetime]::UtcNow).TotalMilliseconds)
 if($remaining -le 1000){throw 'D3_BOOTSTRAP_DEADLINE'}
 $hostObject=[LimitlessV2Host]::new($b.bot_sid,$b.admin_sid,$b.worker_sid,$m.pipe_name,$ManifestSha,$BindingSha);$hostObject.Run([int]$remaining)
}catch{@{schema=3;status='HOLD';reason='DEDICATED_HOST_BOUNDARY_NO_RETRY'}|ConvertTo-Json -Compress}
finally{if($hostObject){$hostObject.Stop()};if($lease){$lease.Dispose()}}

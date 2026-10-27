# Worker 조회만 분리합니다. 생성/등록/종료/권한 변경/lease 획득 기능은 없습니다.
function Resolve-V2QueryPrincipal($Principal,[scriptblock]$Translate){
 $u=$Principal.UserId;$g=$Principal.GroupId
 $shape=@{user_type=if($null -eq $u){'null'}else{$u.GetType().FullName};user_length=([string]$u).Length;group_type=if($null -eq $g){'null'}else{$g.GetType().FullName};group_length=([string]$g).Length;logon_type=[int]$Principal.LogonType}
 if($u -isnot [string] -and $null -ne $u){throw 'USER_TYPE_HOLD'}
 if($g -isnot [string] -and $null -ne $g){throw 'GROUP_TYPE_HOLD'}
 $hasUser=-not [string]::IsNullOrWhiteSpace($u);$hasGroup=-not [string]::IsNullOrWhiteSpace($g)
 if($hasUser -eq $hasGroup){throw 'PRINCIPAL_AMBIGUOUS_HOLD'}
 $value=if($hasUser){$u}else{$g}
 if($hasGroup -and [int]$Principal.LogonType -ne 4){throw 'GROUP_LOGON_HOLD'}
 $sid=if($value -cmatch '^S-1-'){([Security.Principal.SecurityIdentifier]::new($value)).Value}else{& $Translate $value}
 if($sid -isnot [string] -or $sid -cnotmatch '^S-1-'){throw 'SID_RESULT_HOLD'}
 @{sid=$sid;kind=if($hasUser){'user'}else{'group'};shape=$shape}
}
function Invoke-V2WorkerQuery([string]$WorkerSid,$Provider){
 $r=@{known=$false;process_query_known=$false;task_query_known=$false;lease_query_known=$false;foreign_processes=$null;other_launchable_tasks=$null;lease_status='UNKNOWN';processes=@();errors=@();stage='SID'}
 $stage='SID';$shape=$null
 try{
  $sid=[Security.Principal.SecurityIdentifier]::new($WorkerSid)
  if($sid.Value -cne 'S-1-5-21-2858504392-1958168942-1662906075-1007'){throw 'WORKER_SID_HOLD'}
  $stage='ACCOUNT_LOOKUP';$accounts=@(& $Provider.Account $WorkerSid)
  if($accounts.Count -ne 1 -or $accounts[0].SID -cne $WorkerSid){throw 'ACCOUNT_IDENTITY_HOLD'}
  $stage='SESSION_ASSOCIATION';$sessions=@(& $Provider.Sessions $accounts[0]);$seen=@{}
  foreach($session in $sessions){$stage='PROCESS_ASSOCIATION';foreach($p in @(& $Provider.Processes $session)){
   $pidValue=[int]$p.ProcessId
   if($pidValue -le 0 -or $null -eq $p.CreationDate){throw 'PROCESS_IDENTITY_UNKNOWN'}
   $created=([datetime]$p.CreationDate).ToUniversalTime().ToString('o')
   if($seen.ContainsKey($pidValue) -and $seen[$pidValue] -cne $created){throw 'PID_REUSE_HOLD'}
   $seen[$pidValue]=$created
  }}
  $r.processes=@($seen.Keys|Sort-Object|ForEach-Object{@{pid=$_;created_utc=$seen[$_]}})
  $r.foreign_processes=$seen.Count;$r.process_query_known=$true
  $stage='TASK_ENUMERATION';$tasks=@(& $Provider.Tasks);$count=0
  foreach($t in $tasks){
   $stage='TASK_PRINCIPAL_SHAPE';$u=$t.Principal.UserId;$g=$t.Principal.GroupId
   $shape=@{user_type=if($null -eq $u){'null'}else{$u.GetType().FullName};user_length=([string]$u).Length;group_type=if($null -eq $g){'null'}else{$g.GetType().FullName};group_length=([string]$g).Length;logon_type=[int]$t.Principal.LogonType}
   $stage='TASK_PRINCIPAL_RESOLUTION';$principal=Resolve-V2QueryPrincipal $t.Principal $Provider.Translate
   $matches=if($principal.kind -ceq 'user'){$principal.sid -ceq $WorkerSid}else{& $Provider.GroupMember $principal.sid $WorkerSid}
   if($matches -isnot [bool]){throw 'GROUP_MEMBERSHIP_UNKNOWN'}
   if($matches -and ($t.Enabled -or [int]$t.State -eq 4)){$count++}
  }
  $r.other_launchable_tasks=$count;$r.task_query_known=$true;$shape=$null
 }catch{
  $e=$_.Exception;$codes=@();$function=$stage;while($e){$row=@{type=$e.GetType().FullName;hresult=$e.HResult};if($e.Data.Contains('V2QueryFunction')){$function=[string]$e.Data['V2QueryFunction']};if($e -is [ComponentModel.Win32Exception]){$row.win32=$e.NativeErrorCode};$codes+=$row;$e=$e.InnerException}
  $r.errors+=@{stage=$stage;function=$function;argument_shape=$shape;codes=$codes}
 }
 # Task 조회 실패와 독립적으로 lease 메타데이터만 확인합니다. 열거나 잠그지 않습니다.
 try{if($WorkerSid -cne 'S-1-5-21-2858504392-1958168942-1662906075-1007'){throw 'SID'};$r.lease_status=& $Provider.Lease $WorkerSid;if($r.lease_status -notin @('ABSENT','PRESENT_NOT_PROBED')){throw 'LEASE_UNKNOWN'};$r.lease_query_known=($r.lease_status -ceq 'ABSENT')}
 catch{$r.errors+=@{stage='LEASE_READONLY';codes=@{type=$_.Exception.GetType().FullName;hresult=$_.Exception.HResult}}}
 $r.known=$r.process_query_known -and $r.task_query_known -and $r.lease_query_known
 $r.stage=$stage;$r.status=if($r.known -and $r.foreign_processes -eq 0 -and $r.other_launchable_tasks -eq 0){'PASS'}else{'HOLD'};$r
}
function New-V2WorkerReadOnlyProvider {
 @{
  Account={param($sid)Get-CimInstance Win32_UserAccount -Filter "SID='$sid'" -ErrorAction Stop}
  Sessions={param($a)Get-CimAssociatedInstance -InputObject $a -Association Win32_LoggedOnUser -ResultClassName Win32_LogonSession -ErrorAction Stop}
  Processes={param($s)Get-CimAssociatedInstance -InputObject $s -Association Win32_SessionProcess -ResultClassName Win32_Process -ErrorAction Stop}
  Tasks={
   $scheduler=New-Object -ComObject Schedule.Service;$scheduler.Connect()
   function Visit-V2QueryFolder($folder){foreach($t in $folder.GetTasks(1)){[pscustomobject]@{Principal=$t.Definition.Principal;Enabled=[bool]$t.Enabled;State=[int]$t.State}};foreach($f in $folder.GetFolders(0)){Visit-V2QueryFolder $f}}
   Visit-V2QueryFolder ($scheduler.GetFolder('\'))
  }
  Translate={param($value)
   try{$account=[Security.Principal.NTAccount]::new($value)}catch{$_.Exception.Data['V2QueryFunction']='NTAccount.ctor(string)';throw}
   try{$account.Translate([Security.Principal.SecurityIdentifier]).Value}catch{$_.Exception.Data['V2QueryFunction']='NTAccount.Translate(Type)';throw}
  }
  GroupMember={param($group,$sid)
   if($group -in @('S-1-1-0','S-1-5-11')){$true}else{
    # 재귀/도메인 그룹은 멤버십을 추정하지 않습니다.
    $members=@(Get-LocalGroupMember -SID $group -ErrorAction Stop)
    if(@($members|Where-Object ObjectClass -ne 'User').Count){throw 'NESTED_GROUP_UNKNOWN'}
    @($members|Where-Object {$_.SID.Value -ceq $sid}).Count -gt 0
   }
  }
  Lease={param($sid)
   $parent='C:\ProgramData\LimitlessRelayV2Approvals'
   # Test-Path는 거부를 부재로 오인할 수 있으므로 부모 목록으로 확인합니다.
   $base=@(Get-ChildItem -LiteralPath 'C:\ProgramData' -Directory -Force -ErrorAction Stop|Where-Object Name -IEQ 'LimitlessRelayV2Approvals')
   if($base.Count -eq 0){'ABSENT'}else{
    if($base.Count -ne 1 -or $base[0].Attributes -band [IO.FileAttributes]::ReparsePoint){throw 'LEASE_PARENT_UNKNOWN'}
    $items=@(Get-ChildItem -LiteralPath $parent -Force -ErrorAction Stop|Where-Object Name -IEQ ('Worker-'+$sid+'.lease'))
    if($items.Count -eq 0){'ABSENT'}else{'PRESENT_NOT_PROBED'}
   }
  }
 }
}

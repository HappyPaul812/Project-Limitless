# 읽기 전용 부분 설치 진단. 설치기 호출·암호 입력·exit·시스템 변경은 제공하지 않습니다.
$ErrorActionPreference='Stop'
function Safe-Error($Record){
 $chain=@();$e=$Record.Exception
 for($i=0;$null -ne $e -and $i -lt 6;$i++){
  $item=[ordered]@{type=$e.GetType().Name;hresult=$e.HResult}
  if($e -is [ComponentModel.Win32Exception]){$item.native_error_code=$e.NativeErrorCode}
  $chain+=$item;$e=$e.InnerException
 }
 return @{status='UNKNOWN_READ_FAILED';errors=$chain}
}
function Read-Probe([scriptblock]$Read){try{& $Read}catch{Safe-Error $_}}
function File-Hash([string]$Path){(Get-FileHash -LiteralPath $Path -Algorithm SHA256 -ErrorAction Stop).Hash.ToLower()}
function Presence([string]$Path){
 try{$item=Get-Item -LiteralPath $Path -Force -ErrorAction Stop;return @{status='PRESENT';directory=$item.PSIsContainer;reparse=[bool]($item.Attributes -band [IO.FileAttributes]::ReparsePoint)}}catch{
  $failure=Safe-Error $_
  try{
   $parent=Split-Path $Path -Parent;$name=Split-Path $Path -Leaf
   $names=@(Get-ChildItem -LiteralPath $parent -Force -Name -ErrorAction Stop)
   if($name -notin $names){return @{status='ABSENT_FROM_READABLE_PARENT'}}
  }catch{}
  return $failure
 }
}
function Read-Path([string]$Path){
 $p=Presence $Path
 if($p.status -ne 'PRESENT'){return $p}
 $p.acl=Read-Probe {$a=Get-Acl -LiteralPath $Path -ErrorAction Stop;@{status='READ';owner=$a.Owner;sddl=$a.Sddl}}
 if(-not $p.directory){$p.sha256=Read-Probe {@{status='READ';value=(File-Hash $Path)}}}
 return $p
}
function Read-TrialTask($Folder,[bool]$IsAdmin){
 try{
  $t=$Folder.GetTask('LIMITLESS-Trial-S0Host');$d=$t.Definition
  # 전체 arguments/XML은 내보내지 않습니다. 고정 경로와 인수 hash만 보존합니다.
  $actions=@();for($i=1;$i -le $d.Actions.Count;$i++){
   $a=$d.Actions.Item($i);$actions+=@{type=$a.Type;path=$a.Path;working_directory=$a.WorkingDirectory;arguments_sha256=([Convert]::ToHexString([Security.Cryptography.SHA256]::HashData([Text.Encoding]::UTF8.GetBytes([string]$a.Arguments))).ToLower())}
  }
  return @{status='PRESENT';path=$t.Path;enabled=$t.Enabled;principal=$d.Principal.UserId;logon_type=$d.Principal.LogonType;run_level=$d.Principal.RunLevel;triggers=$d.Triggers.Count;sddl=$t.GetSecurityDescriptor(7);actions=$actions}
 }catch{
  $failure=Safe-Error $_;$ex=$_.Exception;$hr=$ex.HResult
  while($null -ne $ex.InnerException){$ex=$ex.InnerException;$hr=$ex.HResult}
  if($hr -eq -2147024894 -and $IsAdmin){
   try{$names=@($Folder.GetTasks(1)|ForEach-Object{$_.Name});if('LIMITLESS-Trial-S0Host' -notin $names){return @{status='ABSENT_ADMIN_ENUM_AND_NOT_FOUND';get_task_hresult=$hr}}}catch{}
  }
  return $failure
 }
}
function Read-BatchRights([string]$Sid){
 if(-not ('LimitlessPartialReadOnlyV1' -as [type])){
 Add-Type -TypeDefinition @'
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Principal;
public static class LimitlessPartialReadOnlyV1 {
 [StructLayout(LayoutKind.Sequential)]struct Text {public ushort Length,Max;public IntPtr Buffer;}
 [StructLayout(LayoutKind.Sequential)]struct Attr {public int Length;public IntPtr Root,Object;public uint Flags;public IntPtr Descriptor,Quality;}
 [DllImport("advapi32.dll")]static extern uint LsaOpenPolicy(IntPtr name,ref Attr attr,uint access,out IntPtr handle);
 [DllImport("advapi32.dll")]static extern uint LsaEnumerateAccountRights(IntPtr handle,byte[] sid,out IntPtr rights,out uint count);
 [DllImport("advapi32.dll")]static extern uint LsaFreeMemory(IntPtr value);
 [DllImport("advapi32.dll")]static extern uint LsaClose(IntPtr handle);
 [DllImport("advapi32.dll")]static extern uint LsaNtStatusToWinError(uint status);
 public static string[] Read(string sid){
  var id=new SecurityIdentifier(sid);var bytes=new byte[id.BinaryLength];id.GetBinaryForm(bytes,0);
  IntPtr handle=IntPtr.Zero,rights=IntPtr.Zero;
  try{
   var attr=new Attr{Length=Marshal.SizeOf<Attr>()};uint status=LsaOpenPolicy(IntPtr.Zero,ref attr,0x800,out handle);
   if(status!=0)throw new Win32Exception((int)LsaNtStatusToWinError(status));
   uint count;status=LsaEnumerateAccountRights(handle,bytes,out rights,out count);
   if(status!=0)throw new Win32Exception((int)LsaNtStatusToWinError(status));
   var found=new List<string>();int size=Marshal.SizeOf<Text>();
   for(int i=0;i<count;i++){var text=Marshal.PtrToStructure<Text>(IntPtr.Add(rights,checked(i*size)));found.Add(Marshal.PtrToStringUni(text.Buffer,text.Length/2));}
   return found.ToArray();
  }finally{if(rights!=IntPtr.Zero)LsaFreeMemory(rights);if(handle!=IntPtr.Zero)LsaClose(handle);}
 }
}
'@
 }
 return @{status='READ';direct_rights=@([LimitlessPartialReadOnlyV1]::Read($Sid));inherited_group_rights_included=$false}
}
function Event-Projection([string]$Text){
 # 한정된 오류 토큰만 추출합니다. 원문/사용자 문자열/명령/인수는 반환하지 않습니다.
 if($Text.Length -gt 32768){$Text=$Text.Substring(0,32768)}
 if($Text -notmatch 'install-session0-trial|run-atomic-trial-install|LimitlessRelayTrial|LIMITLESSBotTrial|LimitlessTrialAccountPolicy'){return $null}
 $codes=@([regex]::Matches($Text,'\bCS[0-9]{4}\b|\b0x[89a-fA-F][0-9a-fA-F]{7}\b')|ForEach-Object{$_.Value}|Select-Object -Unique)
 $functions=@('Add-LocalGroupMember','New-LocalUser','GrantNewBotBatch','LsaOpenPolicy','LsaAddAccountRights','NewFile','Set-Acl','RegisterTaskDefinition','GetSecurityDescriptor')|Where-Object{$Text.Contains($_)}
 $categories=@();if($Text -match '(?i)access.*denied|unauthorized'){$categories+='ACCESS_DENIED_TEXT'}
 if($Text -match '(?i)already.*member|member.*already'){$categories+='ALREADY_MEMBER_TEXT'}
 return @{error_tokens=$codes;known_functions=@($functions);categories=$categories;raw_message_saved=$false}
}
function Read-EventLog([string]$Log,[datetime]$Start,[datetime]$End){
 $info=Get-WinEvent -ListLog $Log -ErrorAction Stop
 $events=@()
 try{$records=@(Get-WinEvent -FilterHashtable @{LogName=$Log;StartTime=$Start;EndTime=$End;Level=@(1,2,3)} -MaxEvents 30 -ErrorAction Stop)}catch{
  if($_.FullyQualifiedErrorId -like 'NoMatchingEventsFound*'){return @{status='NO_MATCHING_ERROR_EVENTS';enabled=$info.IsEnabled}}
  throw
 }
 foreach($e in $records){
  $projection=Event-Projection ([string]$e.Message)
  if($null -ne $projection){$events+=@{id=$e.Id;record_id=$e.RecordId;time_utc=$e.TimeCreated.ToUniversalTime().ToString('o');projection=$projection}}
 }
 return @{status='READ_BOUNDED';enabled=$info.IsEnabled;examined=$records.Count;relevant=$events;max_events=30;raw_messages_saved=$false}
}

$repo='F:/study/codex/Project-Limitless/Temp/DiscordUsageDiagnostics20261026/Worktree'
$receipt=Join-Path $repo '문서/00_프로젝트/팀운영/Evidence/Session0AtomicInstall20261026/0f8e15c951a144efa79f5ccdbeed9c55.json'
$sid='S-1-5-21-2858504392-1958168942-1662906075-1008'
$root='C:/Program Files/LimitlessRelayTrial';$state='C:/ProgramData/LimitlessRelayTrial'
$result=[ordered]@{status='NOT_VERIFIED_HOLD';diagnostic_id=([guid]::NewGuid().ToString('N'));source_request_id='0f8e15c951a144efa79f5ccdbeed9c55';install_attempts_added=0;system_changes=0;host_ipc_fake_calls=0;model_calls=0;official_usage_reads=0;discord_calls=0;credential_reads=0}
try{
 $id=[Security.Principal.WindowsIdentity]::GetCurrent();$admin=([Security.Principal.WindowsPrincipal]::new($id)).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
 $result.reader=@{account=$id.Name;sid=$id.User.Value;session_id=(Get-Process -Id $PID).SessionId;is_admin=$admin}
 $previous=Get-Content -LiteralPath $receipt -Raw|ConvertFrom-Json
 $result.source_receipt_sha256=File-Hash $receipt
 $result.previous=@{status=$previous.status;stage=$previous.stage;install_attempts=$previous.install_attempts;error_type=$previous.error_type;hresult=$previous.hresult;safe_error_code=$previous.safe_error_code;boot_tasks_unchanged=$previous.boot_tasks_unchanged;approval_unchanged=$previous.approval_unchanged}
 $result.account=Read-Probe {
  $u=Get-LocalUser -SID ([Security.Principal.SecurityIdentifier]::new($sid)) -ErrorAction Stop
  @{status='READ';name=$u.Name;sid=$u.SID.Value;enabled=$u.Enabled;expires=$u.AccountExpires}
 }
 $result.worker=Read-Probe {$u=Get-LocalUser -Name 'LIMITLESSRelaySvc' -ErrorAction Stop;@{status='READ';name=$u.Name;sid=$u.SID.Value}}
 $result.groups=Read-Probe {
  $found=@();$failures=@();foreach($g in Get-LocalGroup -ErrorAction Stop){
   try{if(@(Get-LocalGroupMember -SID $g.SID -ErrorAction Stop|Where-Object{$_.SID.Value -ceq $sid}).Count){$found+=@{name=$g.Name;sid=$g.SID.Value}}}catch{$failures+=Safe-Error $_}
  }
  @{status=if($failures.Count){'PARTIAL_UNKNOWN'}else{'READ'};memberships=$found;failed_groups=$failures.Count}
 }
 $result.batch_rights=Read-Probe {Read-BatchRights $sid}
 $result.task=Read-Probe {$s=New-Object -ComObject 'Schedule.Service';$s.Connect();Read-TrialTask ($s.GetFolder('\')) $admin}
 $result.paths=@{};foreach($p in @($root,$state,(Join-Path $root 'Code'),(Join-Path $root 'HostManifest.json'),(Join-Path $root 'DeploymentRecord.json'),(Join-Path $root 'Code/Tools/DiscordAIServiceDiagnostics/TrialAccountPolicy.cs'))){$result.paths[$p]=Read-Path $p}
 $bundle='F:/study/codex/Project-Limitless/Temp/Session0TaskSecurityAtomicCandidate20261026'
 $result.approved_sha=Read-Probe {
  $m=File-Hash (Join-Path $bundle 'HostManifest.json');$i=File-Hash (Join-Path $bundle 'Deployment/install-session0-trial.ps1');$z=File-Hash ($bundle+'.zip')
  @{status='READ';manifest=$m;installer=$i;zip=$z;all_match=($m -ceq '6efbd7839bd4e452d9fbca8a465290533472e87cde71845a73bd37bdb21493ab' -and $i -ceq '117065bbfca686474f9104bc0dff20fa6528dfb9c204e5a2f2720d91994958e9' -and $z -ceq '4b2c56edf5bb8edc6ee7a40a10504f537223bc77874b43a36e61be735c318f83')}
 }
 $result.installed_payload=Read-Probe {
  if(-not $result.approved_sha.all_match){throw 'APPROVED_SHA_HOLD'}
  $null=Get-ChildItem -LiteralPath $root -Force -ErrorAction Stop
  $c=Get-Content -LiteralPath (Join-Path $bundle 'HostManifest.json') -Raw|ConvertFrom-Json
  $different=@();$read_errors=0;$checked=0
  foreach($entry in $c.files_sha256.PSObject.Properties){try{if((File-Hash (Join-Path $root $entry.Name)) -cne $entry.Value){$different+=$entry.Name};$checked++}catch{$read_errors++}}
  @{status=if($read_errors){'PARTIAL_UNKNOWN'}else{'READ'};expected=855;checked=$checked;read_errors=$read_errors;mismatches=$different}
 }
 $result.boot_tasks=Read-Probe {
  $hashes=@{};$same=$true
  foreach($name in @('LIMITLESS-Bot-OnBoot','LIMITLESS-Art-OnBoot')){
   $xml=Export-ScheduledTask -TaskName $name -TaskPath '\' -ErrorAction Stop
   $h=[Convert]::ToHexString([Security.Cryptography.SHA256]::HashData([Text.Encoding]::UTF8.GetBytes($xml))).ToLower();$hashes[$name]=$h
   if($h -cne $previous.boot_tasks_before.$name){$same=$false}
  }
  @{status='READ';hashes=$hashes;unchanged_from_install=$same}
 }
 $result.boot_approval=Read-Probe {$h=File-Hash 'C:/ProgramData/LimitlessRelay/boot-approval.json';@{status='READ';sha256=$h;matches=($h -ceq '3e032ebcce3d91bd762a8dec81048d04607ddde761afd62489c0fa2b8e49140f')}}
 $end=(Get-Item -LiteralPath $receipt).LastWriteTime.AddMinutes(2);$start=$end.AddMinutes(-12)
 $result.events=@{};foreach($log in @('PowerShellCore/Operational','Microsoft-Windows-PowerShell/Operational','Windows PowerShell','Microsoft-Windows-TaskScheduler/Operational')){$result.events[$log]=Read-Probe {Read-EventLog $log $start $end}}
 $result.original_exception_recovery='NOT_VERIFIED_FIXED_THROW_RECEIPT_NO_INNER_EXCEPTION'
}catch{$result.fatal_read_error=Safe-Error $_}
# JSON 증거만 작업 브랜치에 새로 씁니다. 기존 기록 및 보호 폴더는 변경하지 않습니다.
try{
 $out=Join-Path $repo '문서/00_프로젝트/팀운영/Evidence/Session0PartialReadOnly20261026'
 $null=New-Item -ItemType Directory -Path $out -Force
 $path=Join-Path $out ($result.diagnostic_id+'.json');$result.receipt_path=$path
 $json=$result|ConvertTo-Json -Depth 16;[IO.File]::WriteAllText($path,$json,[Text.UTF8Encoding]::new($false))
 $json
}catch{[ordered]@{status='NOT_VERIFIED_HOLD';output_error=(Safe-Error $_)}|ConvertTo-Json -Depth 8}

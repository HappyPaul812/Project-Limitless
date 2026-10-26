#requires -Version 7.0
param([switch]$OfflineTest,[string]$ScriptDirectory=$PSScriptRoot)
$ErrorActionPreference='Stop'
# 과거 19건 중 보존된 PID16개만 조회합니다. 명령행은 메모리에서 일치 여부만 판독합니다.
# 설치/정리 도구를 불러오지 않고 Windows 조회 API와 새 진단 JSON 쓰기만 사용합니다.
function Match-Trial([string]$Value,[string[]]$Aliases) {
 foreach($alias in $Aliases){if($alias -and $Value.IndexOf($alias,[StringComparison]::OrdinalIgnoreCase) -ge 0){return $true}}
 return $false
}
function Read-Code($ErrorRecord){
 $list=@();$ex=$ErrorRecord.Exception
 while($ex){
  $entry=@{type=$ex.GetType().FullName;hresult=$ex.HResult}
  if($ex -is [Microsoft.Management.Infrastructure.CimException]){$entry.native_error_code=[int]$ex.NativeErrorCode;$entry.status_code=[int]$ex.StatusCode}
  if($ex -is [ComponentModel.Win32Exception]){$entry.win32_code=$ex.NativeErrorCode}
  $list+=$entry;$ex=$ex.InnerException
 }
 return ,$list
}
function Classify-Current([bool]$Hit,[bool]$Denied,[bool]$Absent){
 if($Hit){return 'REFERENCE_CONFIRMED'}
 if($Absent){return 'PROCESS_EXITED'}
 if($Denied){return 'ACCESS_DENIED'}
 # 비일치는 경로 별칭/간접 파일 접근 및 과거 동일성을 증명하지 않습니다.
 return 'UNKNOWN'
}
if($OfflineTest){
 $n=0
 foreach($case in @(
  @('C:\PROGRA~1\LIMITL~1\worker.exe',@('C:\PROGRA~1\LIMITL~1'),$true),
  @('secret=DO_NOT_PERSIST LimitlessRelayTrial',@('LimitlessRelayTrial'),$true),
  @('normal.exe',@('LimitlessRelayTrial'),$false),
  @('x',@(''),$false))) {
  if((Match-Trial $case[0] $case[1]) -ne $case[2]){throw 'MATCH_TEST_FAILED'};$n++
 }
 foreach($case in @(@($true,$true,$false,'REFERENCE_CONFIRMED'),@($false,$true,$false,'ACCESS_DENIED'),@($false,$false,$true,'PROCESS_EXITED'),@($false,$false,$false,'UNKNOWN'))){
  if((Classify-Current $case[0] $case[1] $case[2]) -ne $case[3]){throw 'CLASSIFY_TEST_FAILED'};$n++
 }
 try{throw 'SECRET_DO_NOT_PERSIST'}catch{$safe=Read-Code $_|ConvertTo-Json -Depth 5;if($safe.Contains('SECRET_DO_NOT_PERSIST')){throw 'REDACTION_TEST_FAILED'};$n++}
 "Offline $n PASS / 0 FAIL; native/CIM calls 0"
 return
}
$source=Join-Path $ScriptDirectory '../../문서/00_프로젝트/팀운영/Evidence/Session0TrialCleanup20261027/39d34e6461e94a97b0ccac01b611ef8f.json'
if((Get-FileHash -LiteralPath $source -Algorithm SHA256).Hash -ne '2379842226CB58AA83C01132915C0A7D1675B8F9243DCE2D29532D4A751BDC9F'){throw 'SOURCE_SHA_HOLD'}
$old=Get-Content -LiteralPath $source -Raw|ConvertFrom-Json
Add-Type -TypeDefinition @'
using System;
using System.Text;
using System.Runtime.InteropServices;
public static class LimitlessProcessReadOnlyV1 {
 [DllImport("kernel32.dll",SetLastError=true)] public static extern IntPtr OpenProcess(uint access,bool inherit,uint pid);
 [DllImport("kernel32.dll",SetLastError=true)] public static extern bool CloseHandle(IntPtr handle);
 [DllImport("kernel32.dll",SetLastError=true,CharSet=CharSet.Unicode)] public static extern bool QueryFullProcessImageName(IntPtr h,uint flags,StringBuilder path,ref uint size);
 [DllImport("kernel32.dll",SetLastError=true)] public static extern bool GetProcessTimes(IntPtr h,out long created,out long exited,out long kernel,out long user);
 [DllImport("kernel32.dll",SetLastError=true)] public static extern bool GetProcessInformation(IntPtr h,int kind,out uint value,uint size);
 [DllImport("kernel32.dll",SetLastError=true,CharSet=CharSet.Unicode)] public static extern uint GetShortPathName(string path,StringBuilder result,uint size);
}
'@
$aliases=@('LimitlessRelayTrial','LIMITLESSBotTrial','S-1-5-21-2858504392-1958168942-1662906075-1008','LIMITLESS-Trial-S0Host')
$short=[Text.StringBuilder]::new(32768)
$shortLength=[LimitlessProcessReadOnlyV1]::GetShortPathName('C:\Program Files\LimitlessRelayTrial',$short,32768)
$aliasError=if($shortLength -eq 0){[Runtime.InteropServices.Marshal]::GetLastWin32Error()}else{$null}
if($shortLength -gt 0 -and $shortLength -lt 32768){$aliases+=$short.ToString()}
$rows=@()
foreach($processId in $old.references.unknown_pids){
 $r=[ordered]@{historical_pid=$processId;historical_creation_utc=$null;historical_error='NOT_RECORDED';historical_status='UNKNOWN';identity='UNPROVEN_MISSING_HISTORICAL_CREATION';current_status='UNKNOWN';name=$null;creation_utc=$null;session_id=$null;owner_account=$null;owner_sid=$null;image_path=$null;command_line_available=$false;command_line_match=$null;image_match=$null;owner_match=$null;protected_level=$null;protected_windows_system='UNKNOWN';native=@{};cim=@{};errors=@();reference_scope='image/command_line/owner literal aliases only; indirect references not verified'}
 $hit=$false;$denied=$false;$absent=$false;$image=$null
 # 최소 조회 권한만 요청합니다. 디버그 권한 활성화나 메모리 읽기는 하지 않습니다.
 $handle=[LimitlessProcessReadOnlyV1]::OpenProcess(0x1000,$false,[uint32]$processId)
 if($handle -eq [IntPtr]::Zero){$code=[Runtime.InteropServices.Marshal]::GetLastWin32Error();$r.native.open_error=$code;$denied=($code -eq 5)}
 else{
  try{
   [long]$created=0;[long]$exited=0;[long]$kernel=0;[long]$user=0
   if([LimitlessProcessReadOnlyV1]::GetProcessTimes($handle,[ref]$created,[ref]$exited,[ref]$kernel,[ref]$user)){$r.creation_utc=[DateTime]::FromFileTimeUtc($created).ToString('o')}else{$r.native.times_error=[Runtime.InteropServices.Marshal]::GetLastWin32Error()}
   [uint32]$size=32768;$buf=[Text.StringBuilder]::new(32768)
   if([LimitlessProcessReadOnlyV1]::QueryFullProcessImageName($handle,0,$buf,[ref]$size)){$image=$buf.ToString()}else{$r.native.image_error=[Runtime.InteropServices.Marshal]::GetLastWin32Error()}
   [uint32]$level=0
   if([LimitlessProcessReadOnlyV1]::GetProcessInformation($handle,7,[ref]$level,4)){$r.protected_level=$level}else{$r.native.protection_error=[Runtime.InteropServices.Marshal]::GetLastWin32Error()}
   try{
    $p=Get-CimInstance Win32_Process -Filter "ProcessId=$([int]$processId)" -Property ProcessId,Name,CreationDate,SessionId,ExecutablePath,CommandLine -ErrorAction Stop
    if($null -ne $p){
     $r.name=$p.Name;$r.session_id=$p.SessionId
     $cimTime=if($p.CreationDate){$p.CreationDate.ToUniversalTime().ToString('o')}else{$null}
     $r.cim.creation_utc=$cimTime
     # 핸들 생성시각과 CIM 시각이 다르면 중간 PID 재사용 가능성으로 모든 교차정보를 보류합니다.
     $r.cim.same_current_instance=($r.creation_utc -and $cimTime -and [Math]::Abs(([DateTime]$r.creation_utc-[DateTime]$cimTime).TotalMilliseconds) -lt 1)
     if($r.cim.same_current_instance){
      $r.command_line_available=-not [string]::IsNullOrEmpty($p.CommandLine)
      if($r.command_line_available){$r.command_line_match=Match-Trial $p.CommandLine $aliases;$hit=$hit -or $r.command_line_match}
      $r.cim.image_available=-not [string]::IsNullOrEmpty($p.ExecutablePath)
      if(-not $image){$image=$p.ExecutablePath}
      foreach($method in @('GetOwner','GetOwnerSid')){
       try{$o=Invoke-CimMethod -InputObject $p -MethodName $method -ErrorAction Stop;$r.cim[$method]=[uint32]$o.ReturnValue
        if($o.ReturnValue -in @(2,3)){$denied=$true}
        if($o.ReturnValue -eq 0){if($method -eq 'GetOwnerSid'){$r.owner_sid=$o.Sid}else{$r.owner_account=$o.Domain+'\'+$o.User}}
       }catch{$r.errors+=@{operation=$method;codes=(Read-Code $_)}}
      }
      $r.owner_match=(Match-Trial ([string]$r.owner_sid) $aliases) -or (Match-Trial ([string]$r.owner_account) $aliases);$hit=$hit -or $r.owner_match
     }
     $p=$null
    }else{$r.cim.state='NO_INSTANCE_DURING_QUERY'}
   }catch{$r.errors+=@{operation='CIM';codes=(Read-Code $_)}}
   if($exited -gt 0){$r.native.exit_utc=[DateTime]::FromFileTimeUtc($exited).ToString('o')}
  }finally{$null=[LimitlessProcessReadOnlyV1]::CloseHandle($handle)}
 }
 if($handle -eq [IntPtr]::Zero){
  # 부재는 별도의 PID 필터 조회 성공과 OpenProcess 87이 함께 있을 때만 표시합니다.
  try{$p=Get-CimInstance Win32_Process -Filter "ProcessId=$([int]$processId)" -Property ProcessId,Name,CreationDate,SessionId,ExecutablePath,CommandLine -ErrorAction Stop
   if($null -eq $p -and $r.native.open_error -eq 87){$absent=$true;$r.identity='HISTORICAL_PID_CURRENTLY_ABSENT'}
   elseif($p){
    $r.name=$p.Name;$r.session_id=$p.SessionId;$r.cim.creation_utc=if($p.CreationDate){$p.CreationDate.ToUniversalTime().ToString('o')}else{$null};$r.cim.state='INSTANCE_PRESENT_NATIVE_DENIED'
    $image=$p.ExecutablePath;$r.command_line_available=-not [string]::IsNullOrEmpty($p.CommandLine)
    if($r.command_line_available){$r.command_line_match=Match-Trial $p.CommandLine $aliases;$hit=$hit -or $r.command_line_match}
    foreach($method in @('GetOwner','GetOwnerSid')){
     try{$o=Invoke-CimMethod -InputObject $p -MethodName $method -ErrorAction Stop;$r.cim[$method]=[uint32]$o.ReturnValue
      if($o.ReturnValue -eq 0){if($method -eq 'GetOwnerSid'){$r.owner_sid=$o.Sid}else{$r.owner_account=$o.Domain+'\'+$o.User}}
     }catch{$r.errors+=@{operation=$method;codes=(Read-Code $_)}}
    }
    $r.owner_match=(Match-Trial ([string]$r.owner_sid) $aliases) -or (Match-Trial ([string]$r.owner_account) $aliases);$hit=$hit -or $r.owner_match
    $r.cim.identity_caveat='NO_NATIVE_HANDLE; per-provider observations may race; no negative verdict'
   }
   $p=$null
  }catch{$r.errors+=@{operation='CIM_EXISTENCE';codes=(Read-Code $_)}}
 }
 if($image){
  $r.image_match=Match-Trial $image $aliases;$hit=$hit -or $r.image_match
  # 개인 경로는 공유 JSON에 저장하지 않습니다. Windows 고정 경로만 노출합니다.
  if($image -match '^C:\\Windows\\(System32|SysWOW64)\\[A-Za-z0-9_.-]+\.exe$'){$r.image_path=$image}else{$r.image_path='REDACTED_NON_SYSTEM_PATH'}
 }
 $r.current_status=Classify-Current $hit $denied $absent
 if($absent){$r.historical_status='PROCESS_EXITED'}
 # 보호 수준을 읽어도 Windows 시스템 구성요소라는 인증은 별도입니다. 이름만으로 확정하지 않습니다.
 $rows+=[pscustomobject]$r
}
for($i=$rows.Count;$i -lt [int]$old.references.unknown_processes;$i++){
 $rows+=[pscustomobject]@{historical_pid=$null;historical_status='UNKNOWN';identity='SOURCE_PID_TRUNCATED';current_status='UNKNOWN';historical_error='NOT_RECORDED';protected_windows_system='UNKNOWN'}
}
$id=[Security.Principal.WindowsIdentity]::GetCurrent()
$result=[ordered]@{schema=1;observed_utc=[DateTime]::UtcNow.ToString('o');status='REFERENCE_VERIFICATION_HOLD';source_sha256='2379842226cb58aa83c01132915c0a7d1675b8f9243dce2d29532d4a751bdc9f';source_creation_missing=$true;source_pid_missing=3;is_admin=([Security.Principal.WindowsPrincipal]::new($id)).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator);short_alias_error=$aliasError;rows=$rows;scope='Only preserved16 PIDs; no repeat of tasks/services/all processes; no handles/modules/environment scan';cleanup_or_install_calls=0}
$outDir=Join-Path $ScriptDirectory '../../문서/00_프로젝트/팀운영/Evidence/UnknownProcesses20261027'
$null=New-Item -ItemType Directory -Path $outDir -Force
$out=Join-Path $outDir (([guid]::NewGuid().ToString('N'))+'.json')
$json=$result|ConvertTo-Json -Depth 12
$stream=[IO.File]::Open($out,[IO.FileMode]::CreateNew,[IO.FileAccess]::Write,[IO.FileShare]::None)
try{$bytes=[Text.Encoding]::UTF8.GetBytes($json);$stream.Write($bytes,0,$bytes.Length)}finally{$stream.Dispose()}
Write-Output "REFERENCE_VERIFICATION_HOLD; sanitized JSON: $([IO.Path]::GetFullPath($out))"

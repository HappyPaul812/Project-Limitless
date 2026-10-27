# 실패한 r1의 승인/소비/신규자산만 읽습니다. 암호 입력·설치·수정·삭제 함수가 없습니다.
$ErrorActionPreference='Stop';Set-StrictMode -Version Latest
$base='C:\ProgramData\LimitlessRelayDedicatedV2Approvals';$id='dedicated-install-a5af3087b78b45d1baae46067d5b7ac9'
$rows=[Collections.Generic.List[object]]::new()
function SafeError($e){$a=@();for($n=0;$e -and $n -lt 6;$n++){$a+=@{type=$e.GetType().FullName;hresult=$e.HResult};$e=$e.InnerException};$a}
function ProbePath([string]$tag,[string]$path,[string]$kind='Metadata'){
 $r=@{name=$tag;status='UNKNOWN'}
 if($path.StartsWith($base+'\',[StringComparison]::OrdinalIgnoreCase) -and $rows[0].status -ne 'PRESENT'){$r.reason='PROTECTED_PARENT_NOT_VERIFIED';$rows.Add($r);return}
 try{
  $item=Get-Item -LiteralPath $path -Force -ErrorAction Stop
  if($item.Attributes -band [IO.FileAttributes]::ReparsePoint){throw 'R1_REPARSE_HOLD'}
  $acl=Get-Acl -LiteralPath $path -ErrorAction Stop;$r.owner_sid=$acl.GetOwner([Security.Principal.SecurityIdentifier]).Value;$r.sddl=$acl.Sddl;$r.status='PRESENT'
  if($kind -ne 'Metadata'){
   if($item.PSIsContainer -or $item.Length -gt 1MB){throw 'R1_JSON_KIND_SIZE_HOLD'}
   $bytes=[IO.File]::ReadAllBytes($path);$r.sha256=[Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($bytes)).ToLowerInvariant()
   $j=[Text.Encoding]::UTF8.GetString($bytes)|ConvertFrom-Json -AsHashtable
   if($kind -eq 'Approval'){
    foreach($key in @('schema','kind','approved','approval_id','epoch','manifest_sha256','installer_sha256','zip_sha256','admin_sid','expires_unix','preserve_existing','create_bot','create_worker','worker_install_state')){if(-not $j.ContainsKey($key)){throw 'R1_APPROVAL_SHAPE_HOLD'}}
    $r.fixed_r1_binding=($j.schema -eq 3 -and $j.kind -ceq 'DEDICATED_INSTALL_ONCE' -and $j.approval_id -ceq $id -and $j.epoch -ceq '20261027-dedicated-a5af3087b78b45d1baae46067d5b7ac9' -and $j.manifest_sha256 -ceq '43f6afcc91eae196e015393670cd9cd4ff37194099b313c2b901177226cd32e3' -and $j.installer_sha256 -ceq '83c97d781fb2d9aed797b41ea6bc799b83e939ad3ffa6f7af6a2d26f1080d68f' -and $j.zip_sha256 -ceq 'be58f8cdbe0befdcc64cd67287865c6cd6fc2611b4a706d26cda022a8014ca25' -and $j.create_bot -ceq 'LIMITLESSBotV2D' -and $j.create_worker -ceq 'LIMITLESSWorkV2D' -and $j.worker_install_state -ceq 'ENABLED_BATCH_ONLY')
    $r.expired=([long]$j.expires_unix -le [datetimeoffset]::UtcNow.ToUnixTimeSeconds())
    $r.same_admin=($j.admin_sid -ceq [Security.Principal.WindowsIdentity]::GetCurrent().User.Value)
   }else{
    $r.schema_is_3=($j.schema -eq 3);$r.has_consumed_true=($j.ContainsKey('consumed') -and $j.consumed -eq $true)
   }
  }
 }catch [Management.Automation.ItemNotFoundException]{$r.status='ABSENT'}catch{$r.status='UNKNOWN';$r.errors=@(SafeError $_.Exception)}
 $rows.Add($r)
}
ProbePath 'APPROVAL_ROOT' $base
ProbePath 'INSTALL_APPROVAL_JSON' ($base+'\'+$id+'.json') 'Approval'
ProbePath 'CONSUMPTION_DIRECTORY' ($base+'\'+$id)
ProbePath 'CONSUMED_MARKER' ($base+'\'+$id+'\consumed.json') 'Consumption'
ProbePath 'INSTALL_LEASE' ($base+'\'+$id+'\install.lease')
ProbePath 'DEDICATED_CODE_ROOT' 'C:\Program Files\LimitlessRelayDedicatedV2_20261027'
ProbePath 'DEDICATED_STATE_ROOT' 'C:\ProgramData\LimitlessRelayDedicatedV2_20261027'
foreach($name in @('LIMITLESSBotV2D','LIMITLESSWorkV2D')){
 $r=@{name=$name;status='UNKNOWN'}
 try{$u=Get-LocalUser -Name $name -ErrorAction Stop;$r.status='PRESENT';$r.sid=$u.SID.Value;$r.enabled=$u.Enabled}
 catch{if($_.Exception.GetType().FullName -ceq 'Microsoft.PowerShell.Commands.UserNotFoundException'){$r.status='ABSENT'}else{$r.errors=@(SafeError $_.Exception)}}
 $rows.Add($r)
}
$r=@{name='DEDICATED_TASK';status='UNKNOWN'}
try{
 $s=New-Object -ComObject Schedule.Service;$s.Connect();$t=$s.GetFolder('\').GetTask('LIMITLESS-DedicatedV2-20261027-Host')
 $r.status='PRESENT';$r.enabled=$t.Enabled;$r.triggers=$t.Definition.Triggers.Count;$r.principal_user=$t.Definition.Principal.UserId;$r.logon_type=$t.Definition.Principal.LogonType;$r.sddl=$t.GetSecurityDescriptor(7)
}catch{if($_.Exception.HResult -eq -2147024894){$r.status='ABSENT'}else{$r.errors=@(SafeError $_.Exception)}}
$rows.Add($r)
$r=@{name='PROTECTED_RESULTS';status='UNKNOWN';count=$null}
try{
 if($rows[0].status -ne 'PRESENT'){throw 'R1_PROTECTED_PARENT_NOT_VERIFIED'}
 $dir=Get-Item -LiteralPath ($base+'\'+$id) -Force -ErrorAction Stop
 if($dir.Attributes -band [IO.FileAttributes]::ReparsePoint){throw 'R1_REPARSE_HOLD'}
 $files=@(Get-ChildItem -LiteralPath $dir.FullName -Filter 'Result-*.json' -File -Force -ErrorAction Stop)
 $r.status='KNOWN';$r.count=$files.Count
 # 결과 본문/명령줄/인증은 읽거나 출력하지 않습니다. 존재 여부만으로 재시도를 허용하지 않습니다.
}catch [Management.Automation.ItemNotFoundException]{$r.status='DIRECTORY_ABSENT';$r.count=0}catch{$r.errors=@(SafeError $_.Exception)}
$rows.Add($r)
@{schema=3;kind='R1_INSTALL_HOLD_READONLY';admin=([Security.Principal.WindowsPrincipal]::new([Security.Principal.WindowsIdentity]::GetCurrent())).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator);status='HOLD_REQUIRES_RECOVERY_REVIEW';retry_authorized_by_probe=$false;checks=$rows;password_reads=0;install_calls=0;changes=0;ipc_calls=0;model_calls=0}|ConvertTo-Json -Depth 10

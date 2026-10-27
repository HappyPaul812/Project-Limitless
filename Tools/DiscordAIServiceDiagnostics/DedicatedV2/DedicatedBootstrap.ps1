function Read-D3Installed([string]$ManifestSha,[string]$BindingSha,[switch]$WorkerContext){
 $fixed='C:\Program Files\LimitlessRelayDedicatedV2_20261027'
 function D3Hash([byte[]]$b){[Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($b)).ToLowerInvariant()}
 $mr=[IO.File]::ReadAllBytes((Join-Path $fixed 'HostManifest.json'));$br=[IO.File]::ReadAllBytes((Join-Path $fixed 'InstalledBinding.json'))
 if((D3Hash $mr) -cne $ManifestSha -or (D3Hash $br) -cne $BindingSha){throw 'D3_BOOTSTRAP_PINS'}
 $m=[Text.Encoding]::UTF8.GetString($mr)|ConvertFrom-Json -AsHashtable;$b=[Text.Encoding]::UTF8.GetString($br)|ConvertFrom-Json -AsHashtable
 $paths=@($fixed,(Join-Path $fixed 'Code'),(Join-Path $fixed 'Code/V2'),(Join-Path $fixed 'HostManifest.json'),(Join-Path $fixed 'InstalledBinding.json'))
 foreach($n in $m.files_sha256.Keys){$p=Join-Path $fixed $n;if((D3Hash ([IO.File]::ReadAllBytes($p))) -cne $m.files_sha256[$n]){throw 'D3_BOOTSTRAP_FILE_SHA'};$paths+=$p}
 foreach($p in $paths){$i=Get-Item -LiteralPath $p -Force;if($i.Attributes -band [IO.FileAttributes]::ReparsePoint){throw 'D3_CODE_REPARSE'};$acl=Get-Acl -LiteralPath $p;if($acl.GetOwner([Security.Principal.SecurityIdentifier]).Value -cne 'S-1-5-32-544'){throw 'D3_CODE_OWNER'};foreach($r in $acl.GetAccessRules($true,$true,[Security.Principal.SecurityIdentifier])){if($r.PropagationFlags -band [Security.AccessControl.PropagationFlags]::InheritOnly){continue};if($r.AccessControlType -eq 'Allow' -and $r.IdentityReference.Value -notin @('S-1-5-18','S-1-5-32-544') -and ([int]$r.FileSystemRights -band 0xD0156)){throw 'D3_CODE_WRITER'}}}
 if('DedicatedContract' -as [type]){throw 'D3_FRESH_PROCESS_REQUIRED'}
 Add-Type -TypeDefinition ([IO.File]::ReadAllText((Join-Path $fixed 'Code/V2/InstallAssembly.cs')))
 [DedicatedContract]::Binding([Text.Encoding]::UTF8.GetString($br),[Text.Encoding]::UTF8.GetString($mr),$ManifestSha)
 if($WorkerContext){[LimitlessV2Publish]::Verify($m.codex_home,[DedicatedContract]::FsSddl('auth',$true,$b.worker_sid,''));[LimitlessV2Publish]::Verify((Join-Path $m.state_root 'Data'),[DedicatedContract]::FsSddl('state',$true,$b.worker_sid,''))}
 @{manifest=$m;binding=$b}
}
function Set-D3WorkerEnvironment($m){
 # 새 Host/자식 프로세스에서만 환경을 최소 목록으로 만듭니다. 기존 환경의 값은 읽거나 기록하지 않습니다.
 $names=@([Environment]::GetEnvironmentVariables('Process').Keys)
 foreach($name in $names){[Environment]::SetEnvironmentVariable([string]$name,$null,'Process')}
 foreach($pair in @{SystemRoot='C:\Windows';WINDIR='C:\Windows';PATH='C:\Program Files\PowerShell\7;C:\Windows\System32;C:\Windows';TEMP=($m.state_root+'\Temp');TMP=($m.state_root+'\Temp');CODEX_HOME=$m.codex_home}.GetEnumerator()){[Environment]::SetEnvironmentVariable($pair.Key,$pair.Value,'Process')}
}

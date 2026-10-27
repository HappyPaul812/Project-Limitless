param()
$ErrorActionPreference='Stop'
$base='F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\Tools\DiscordAIServiceDiagnostics\WorkerInvestigation20261027'
$source='F:\study\codex\Project-Limitless\Temp\V2Hold2Worker20261027\76eec5af0bca409fbbbe3930e8eae3db.json'
$out='F:\study\codex\Project-Limitless\Temp\WorkerProcessGroupInvestigation20261027'
$result=@{schema=1;status='HOLD';utc=[DateTimeOffset]::UtcNow.ToString('o');system_changes=0;actual_ai_calls=0;privileges_enabled=0;expected_target_count=11;stage='SOURCE_AND_TOOL_PINS'}
function Local-Error($e,$stage){$codes=@();$n=0;while($e -and $n++ -lt 8){$c=@{type=$e.GetType().FullName;hresult=$e.HResult};if($e -is [ComponentModel.Win32Exception]){$c.win32=$e.NativeErrorCode};if($e.PSObject.Properties['Operation']){$c.operation=$e.Operation};$codes+=$c;$e=$e.InnerException};@{stage=$stage;codes=$codes}}
function Local-Sha([byte[]]$bytes){[Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($bytes)).ToLowerInvariant()}
try{
 $identity=[Security.Principal.WindowsIdentity]::GetCurrent();$result.admin=([Security.Principal.WindowsPrincipal]::new($identity)).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
 $raw=[IO.File]::ReadAllBytes($source)
 if((Local-Sha $raw) -cne '8223799133dea9736f9dbe341d4498c0a020ec1880a2cb1797fea1cb7e86ccbe'){throw 'BASELINE_SHA_HOLD'}
 $baseline=[Text.Encoding]::UTF8.GetString($raw)|ConvertFrom-Json -AsHashtable
 if($baseline.worker.processes.Count -ne 11 -or -not $baseline.worker.process_query_known){throw 'BASELINE_SCOPE_HOLD'}
 $code=[IO.File]::ReadAllBytes((Join-Path $base 'WorkerReadOnlyNative.cs'));$model=[IO.File]::ReadAllBytes((Join-Path $base 'InvestigationModel.ps1'))
 if((Local-Sha $code) -cne '7919d951410e515d19834616c4f74a8f8efade78a7cc31a9d5d561304516ddf9' -or (Local-Sha $model) -cne 'e96fa9aab959f857bc5aca1b15f6e16966e3bddedd271e30d4425b4f757ac6b2'){throw 'INVESTIGATION_CODE_SHA_HOLD'}
 if('WorkerReadOnlyNative' -as [type]){throw 'FRESH_POWERSHELL_REQUIRED'}
 Add-Type -TypeDefinition ([Text.Encoding]::UTF8.GetString($code))
 . ([scriptblock]::Create([Text.Encoding]::UTF8.GetString($model)))
 $workerSid='S-1-5-21-2858504392-1958168942-1662906075-1007'
 $result.stage='LOCAL_WORKER_IDENTITY_BINDING'
 $worker=[WorkerReadOnlyNative]::Resolve(([Environment]::MachineName+'\LIMITLESSRelaySvc'))
 $result.worker_binding=$worker
 if($worker.sid -cne $workerSid -or $worker.sid_name_use -ne 1){throw 'WORKER_SID_BINDING_HOLD'}
 $provider=@{
  Process={param($id,$created)[WorkerReadOnlyNative]::Read([int]$id,[string]$created)}
  Cim={param($id)Get-CimInstance Win32_Process -Filter ('ProcessId='+[int]$id) -Property ProcessId,ParentProcessId,CreationDate,SessionId -ErrorAction Stop}
  Parent={param($id)[WorkerReadOnlyNative]::Parent([int]$id)}
  Children={param($id)Get-CimInstance Win32_Process -Filter ('ParentProcessId='+[int]$id) -Property ProcessId,ParentProcessId,CreationDate,SessionId -ErrorAction Stop|Select-Object -First 65}
  Logon={param($id)if([string]$id -cnotmatch '^\d+$'){throw 'LUID_FORMAT_HOLD'};Get-CimInstance Win32_LogonSession -Filter ("LogonId='"+$id+"'") -Property LogonId,LogonType -ErrorAction Stop}
  Resolve={param($value)[WorkerReadOnlyNative]::Resolve([string]$value)}
  Image={param($path)
   $image=[WorkerReadOnlyNative]::Image($path);$name=[string]$image.name
   $known=@('codex.exe','codex-command-runner.exe','codex-runner.exe','codex-exec.exe','node.exe','python.exe','pythonw.exe','py.exe','pwsh.exe','powershell.exe','cmd.exe','dotnet.exe','bun.exe','uv.exe','conhost.exe','fontdrvhost.exe','dwm.exe','sihost.exe','csrss.exe','winlogon.exe','userinit.exe','taskhostw.exe','runtimebroker.exe','explorer.exe')
   if($name.ToLowerInvariant() -notin $known){$image['name_sha256']=Local-Sha ([Text.Encoding]::UTF8.GetBytes($name));$image['name']='OTHER_EXECUTABLE_NAME_REDACTED'}
   $image
  }
  Tasks={
   $scheduler=New-Object -ComObject Schedule.Service;$scheduler.Connect()
   function Read-PrincipalFolder($folder){
    foreach($task in $folder.GetTasks(1)){
     $p=$task.Definition.Principal;$label=if($task.Path -in @('\LIMITLESS-Bot-OnBoot','\LIMITLESS-Art-OnBoot')){$task.Path}else{$null}
     [pscustomobject]@{Principal=@{UserId=$p.UserId;GroupId=$p.GroupId;LogonType=[int]$p.LogonType};enabled=[bool]$task.Enabled;state=[int]$task.State;task_id_hash=(Local-Sha ([Text.Encoding]::UTF8.GetBytes([string]$task.Path)));known_task_label=$label;native_task=$task}
    }
    foreach($f in $folder.GetFolders(0)){Read-PrincipalFolder $f}
   }
   Read-PrincipalFolder ($scheduler.GetFolder('\'))
  }
  TaskDetail={param($task)
   $images=@();foreach($action in $task.native_task.Definition.Actions){
    if([int]$action.Type -eq 0){$name=[IO.Path]::GetFileName([string]$action.Path).ToLowerInvariant();$images+=if($name -in @('codex.exe','node.exe','python.exe','pwsh.exe','powershell.exe','cmd.exe','dotnet.exe')){$name}else{'OTHER_EXECUTABLE_NAME_REDACTED'}}else{$images+='NON_EXEC_ACTION_UNKNOWN'}
   }
   $instances=@();foreach($instance in $task.native_task.GetInstances(0)){
    if($instances.Count -ge 32){throw 'TASK_INSTANCE_BOUND'}
    $instances+=@{engine_pid=[int]$instance.EnginePID;state=[int]$instance.State;instance_id=[string]$instance.InstanceGuid}
   };@{action_images=$images;running_instances=$instances}
  }
  Leases={
   $root='C:\ProgramData\LimitlessRelayV2Approvals';$paths=@{worker_lease=($root+'\Worker-S-1-5-21-2858504392-1958168942-1662906075-1007.lease');r9_consume=($root+'\v2-install-01f8a36c9bfe4241b24b7eaa6caaddc2\consumed.json');r11_consume=($root+'\v2-install-13f746811f8a4b7da4c1bc3402c67f41\consumed.json');r11_approval=($root+'\v2-install-13f746811f8a4b7da4c1bc3402c67f41.json')};$rows=@{}
   foreach($key in $paths.Keys){try{$rows[$key]=[WorkerReadOnlyNative]::PathMetadata($paths[$key])}catch{$rows[$key]=Get-InvestigationError $_.Exception 'FIXED_PATH_METADATA'}}
   @{coverage='KNOWN_V2_PATHS_ONLY_NOT_GLOBAL';entries=$rows;contents_read=$false;locks_acquired=0;ancestor_chain_verified=$false}
  }
 }
 $result.stage='SCOPED_PROCESS_AND_GROUP_INVESTIGATION'
 $result.investigation=Invoke-WorkerInvestigation $baseline.worker.processes $workerSid $provider
}catch{$result.errors=Local-Error $_.Exception $result.stage}
try{
 [IO.Directory]::CreateDirectory($out)|Out-Null;$path=Join-Path $out ([guid]::NewGuid().ToString('N')+'.json')
 $bytes=[Text.UTF8Encoding]::new($false).GetBytes(($result|ConvertTo-Json -Depth 28));$file=[IO.File]::Open($path,[IO.FileMode]::CreateNew,[IO.FileAccess]::Write,[IO.FileShare]::None);try{$file.Write($bytes)}finally{$file.Dispose()}
 @{status='HOLD';admin=$result.admin;result_path=$path;expected_target_count=11;collected_target_count=if($result.investigation){@($result.investigation.targets).Count}else{0};task_count=if($result.investigation){@($result.investigation.tasks).Count}else{0};errors=$result.errors}|ConvertTo-Json -Depth 8
}catch{@{status='HOLD';result_path=$null;errors=(Local-Error $_.Exception 'RESULT_WRITE')}|ConvertTo-Json -Depth 8}

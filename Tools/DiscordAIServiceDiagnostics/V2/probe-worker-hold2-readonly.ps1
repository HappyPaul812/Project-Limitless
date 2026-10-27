$ErrorActionPreference='Stop'
$r=@{schema=2;status='HOLD';system_changes=0;staging_created=$false;utc=[DateTimeOffset]::UtcNow.ToString('o')}
try{
 $identity=[Security.Principal.WindowsIdentity]::GetCurrent()
 if(-not ([Security.Principal.WindowsPrincipal]::new($identity)).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)){throw 'ADMIN_READONLY_REQUIRED'}
 $helper='F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\Tools\DiscordAIServiceDiagnostics\V2\V2WorkerQuery.ps1'
 $b=[IO.File]::ReadAllBytes($helper)
 if([Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($b)).ToLowerInvariant() -cne 'd6471b2b2427da3b94b9724c45e367408a1846d1ec230a4957c45c0e01265a18'){throw 'HELPER_SHA_HOLD'}
 . ([scriptblock]::Create([Text.Encoding]::UTF8.GetString($b)))
 $r.worker=Invoke-V2WorkerQuery 'S-1-5-21-2858504392-1958168942-1662906075-1007' (New-V2WorkerReadOnlyProvider)
 $r.status=$r.worker.status
}catch{$r.errors=@{type=$_.Exception.GetType().FullName;hresult=$_.Exception.HResult}}
$out='F:\study\codex\Project-Limitless\Temp\V2Hold2Worker20261027'
[IO.Directory]::CreateDirectory($out)|Out-Null
$path=Join-Path $out ([guid]::NewGuid().ToString('N')+'.json')
$b=[Text.UTF8Encoding]::new($false).GetBytes(($r|ConvertTo-Json -Depth 14));$f=[IO.File]::Open($path,[IO.FileMode]::CreateNew,[IO.FileAccess]::Write,[IO.FileShare]::None);try{$f.Write($b)}finally{$f.Dispose()}
@{status=$r.status;result_path=$path;worker=$r.worker;errors=$r.errors}|ConvertTo-Json -Depth 14

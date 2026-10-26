# 새90초 supervisor만 가짜 Python 프로세스로 검사합니다. 공식 CLI는 호출하지 않습니다.
$ErrorActionPreference='Stop'
Add-Type -Path (Join-Path $PSScriptRoot 'InferenceJob90.cs')
$python=(Get-Command python).Source
$scratch=Join-Path ([IO.Path]::GetTempPath()) ('LimitlessInferenceJobOffline-'+[guid]::NewGuid().ToString('N'))
$null=New-Item -ItemType Directory -Path $scratch
$fake=Join-Path $scratch 'fake.py'
@'
import os,subprocess,sys,time
from pathlib import Path
mode=sys.argv[1]
if mode=='normal':print('SYNTHETIC_OK')
elif mode=='flood':
 os.write(1,b'a'*200000);os.write(2,b'b'*100000)
elif mode=='tree':
 child=subprocess.Popen([sys.executable,'-c','import time;time.sleep(30)'])
 Path(sys.argv[2]).write_text(str(child.pid))
 time.sleep(30)
elif mode=='exit_tree':
 child=subprocess.Popen([sys.executable,'-c','import time;time.sleep(30)'])
 Path(sys.argv[2]).write_text(str(child.pid))
'@ | Set-Content -LiteralPath $fake -Encoding utf8
$normal=[LimitlessInferenceJob90V1]::Run($python,@($fake,'normal'),$scratch,85000)
if($normal.ExitCode -ne 0 -or $normal.Stdout.Trim() -ne 'SYNTHETIC_OK' -or -not $normal.CleanupVerified -or $normal.ActiveProcesses -ne 0){throw 'NORMAL_FAIL'}
'NATIVE90_NORMAL_MAX_BUDGET_PASS'
$flood=[LimitlessInferenceJob90V1]::Run($python,@($fake,'flood'),$scratch,5000)
if(-not $flood.StdoutTruncated -or -not $flood.StderrTruncated -or -not $flood.CleanupVerified -or $flood.Stdout.Length -gt 65536){throw 'FLOOD_FAIL'}
'NATIVE90_BOUNDED_CONCURRENT_DRAIN_PASS'
$pidFile=Join-Path $scratch 'descendant.pid'
$watch=[Diagnostics.Stopwatch]::StartNew()
$timeout=[LimitlessInferenceJob90V1]::Run($python,@($fake,'tree',$pidFile),$scratch,2200)
if(-not $timeout.TimedOut -or -not $timeout.CleanupVerified -or $timeout.ActiveProcesses -ne 0 -or $watch.ElapsedMilliseconds -gt 5000){throw 'TIMEOUT_FAIL'}
$descendantId=[int](Get-Content -LiteralPath $pidFile)
if(Get-Process -Id $descendantId -ErrorAction SilentlyContinue){throw 'DESCENDANT_SURVIVED'}
'NATIVE90_TIMEOUT_TREE_CLEANUP_PASS'
$exitPidFile=Join-Path $scratch 'exit-descendant.pid'
$exitResult=[LimitlessInferenceJob90V1]::Run($python,@($fake,'exit_tree',$exitPidFile),$scratch,5000)
if(-not $exitResult.CleanupVerified -or $exitResult.ActiveProcesses -ne 0){throw 'ROOT_EXIT_CLEANUP_FAIL'}
$exitDescendantId=[int](Get-Content -LiteralPath $exitPidFile)
if(Get-Process -Id $exitDescendantId -ErrorAction SilentlyContinue){throw 'EXIT_DESCENDANT_SURVIVED'}
'NATIVE90_ROOT_EXIT_TREE_CLEANUP_PASS'
$parentScript=Join-Path $scratch 'parent.ps1';$parentPidFile=Join-Path $scratch 'parent-descendant.pid'
@'
param($JobSource,$PythonExe,$Fake,$Scratch,$PidFile)
Add-Type -Path $JobSource
$null=[LimitlessInferenceJob90V1]::Run($PythonExe,@($Fake,'tree',$PidFile),$Scratch,85000)
'@ | Set-Content -LiteralPath $parentScript -Encoding utf8
$argLine='-NoProfile -NonInteractive -File "'+$parentScript+'" "'+(Join-Path $PSScriptRoot 'InferenceJob90.cs')+'" "'+$python+'" "'+$fake+'" "'+$scratch+'" "'+$parentPidFile+'"'
$parentProcess=Start-Process -FilePath (Get-Command pwsh).Source -ArgumentList $argLine -WindowStyle Hidden -PassThru
try{
 for($probe=0;$probe -lt 80 -and -not (Test-Path -LiteralPath $parentPidFile);$probe++){Start-Sleep -Milliseconds 100}
 if(-not (Test-Path -LiteralPath $parentPidFile)){throw 'PARENT_FIXTURE_NOT_READY'}
 $parentDescendantId=[int](Get-Content -LiteralPath $parentPidFile)
 $parentProcess.Kill();$null=$parentProcess.WaitForExit(1000)
 for($probe=0;$probe -lt 20 -and (Get-Process -Id $parentDescendantId -ErrorAction SilentlyContinue);$probe++){Start-Sleep -Milliseconds 100}
 if(Get-Process -Id $parentDescendantId -ErrorAction SilentlyContinue){throw 'PARENT_EXIT_DESCENDANT_SURVIVED'}
}finally{if(-not $parentProcess.HasExited){$parentProcess.Kill()};$parentProcess.Dispose()}
'NATIVE90_PARENT_DEATH_KILL_ON_CLOSE_PASS'
$guard=$false
try{$null=[LimitlessInferenceJob90V1]::Run($python,@($fake,'normal'),$scratch,90000)}catch{$guard=$true}
if(-not $guard){throw 'BUDGET_GUARD_FAIL'}
'NATIVE90_RESERVES_OUTER_CLEANUP_BUDGET_PASS'
$parseTokens=$null;$parseErrors=$null
$tree=[Management.Automation.Language.Parser]::ParseFile((Join-Path $PSScriptRoot 'run-approved-inference-once90.ps1'),[ref]$parseTokens,[ref]$parseErrors)
if($parseErrors.Count){throw 'PS_PARSE_FAIL'}
$params=@($tree.ParamBlock.Parameters|ForEach-Object {$_.Name.VariablePath.UserPath.ToLowerInvariant()})
$reserved=@('home','pid','psscriptroot','pscommandpath','error','pshome','myinvocation','executioncontext','psboundparameters')
$bad=@($tree.FindAll({param($n) $n -is [Management.Automation.Language.AssignmentStatementAst] -and $n.Left -is [Management.Automation.Language.VariableExpressionAst]},$true)|ForEach-Object {$_.Left.VariablePath.UserPath.ToLowerInvariant()}|Where-Object {$_ -in $params -or $_ -in $reserved})
if($bad.Count){throw 'PS_VARIABLE_COLLISION_FAIL'}
'NATIVE90_LAUNCHER_PARSE_BINDINGS_PASS'
'INFERENCE90_NATIVE_OFFLINE_7_PASS_REAL_USAGE_0_REAL_AI_0'

# 관리자 스크립트 실행 아님: 파일 AST의 순수 함수 두 개만 추출해 Fake에 연결합니다.
Set-StrictMode -Version Latest
$ErrorActionPreference='Stop'
$path=Join-Path $PSScriptRoot 'Prepare-Staging.ps1'
$tokens=$null;$errors=$null
$ast=[Management.Automation.Language.Parser]::ParseFile($path,[ref]$tokens,[ref]$errors)
if($errors.Count){throw 'PARSE_FAIL'}
$functions=$ast.FindAll({param($n) $n -is [Management.Automation.Language.FunctionDefinitionAst]},$false)
if($functions.Count -ne 2){throw 'FUNCTION_SCOPE_FAIL'}
foreach($f in $functions){. ([scriptblock]::Create($f.Extent.Text))}
$script:passed=0
function Check([bool]$ok){if(-not $ok){throw 'ASSERT'}}
function Test([string]$name,[scriptblock]$action){& $action;$script:passed++;Write-Output "PASS $name"}
function Fixture {
    $m=[pscustomobject]@{Time=[DateTimeOffset]::Parse('2026-10-31T20:00:00+09:00');Admin=$true;Elevated=$true;Impersonating=$false;Sid='SYNTHETIC_ACTOR';Present=$false;Ntfs=$true;Parents=$true;Image='2ff5669350a6761a7eb979f6133a4b0a85b44f5440f96a05644a57601026349e';Script=('a'*64);Calls=0;Readback=$true;FailCreate=$false;Before=$null;Attempts=[Collections.Generic.HashSet[string]]::new()}
    $m|Add-Member ScriptMethod Now {return $this.Time}
    $m|Add-Member ScriptMethod Actor {return @{Admin=$this.Admin;Elevated=$this.Elevated;Impersonating=$this.Impersonating;Sid=$this.Sid}}
    $m|Add-Member ScriptMethod FixedNtfs {return $this.Ntfs}
    $m|Add-Member ScriptMethod VerifiedScriptSha {return $this.Script}
    $m|Add-Member ScriptMethod ValidatePinnedParents {param($b) if(-not $this.Parents -or $b -cne 'SYNTHETIC_PIN'){throw 'PARENTS'}}
    $m|Add-Member ScriptMethod Exists {param($p) return $this.Present}
    $m|Add-Member ScriptMethod PinnedImageSha {return $this.Image}
    $m|Add-Member ScriptMethod AttemptOnce {param($id) if(-not $this.Attempts.Add($id)){throw 'USED'}}
    $m|Add-Member ScriptMethod CreateNewWithInitialSecurity {param($p,$s,$guard) if($this.Before){& $this.Before $this}; & $guard; $this.Calls++;if($this.FailCreate){throw 'CREATE'};$this.Present=$true}
    $m|Add-Member ScriptMethod VerifyCreatedHandle {param($p,$s) if(-not $this.Readback){throw 'READBACK'}}
    $a=[pscustomobject]@{Profile='DISCORD_V2_INITIAL_STAGING_V1';Stage='C:\ProgramData\LimitlessDiscordV2BootstrapStaging';ApprovalId='11111111111111111111111111111111';ExpiresUtc=[DateTimeOffset]::Parse('2026-10-31T22:00:00+09:00');AdministratorSid='SYNTHETIC_ACTOR';ScriptSha=('a'*64);ParentBindings='SYNTHETIC_PIN'}
    return @{M=$m;A=$a}
}
function Run($f){Invoke-StagingDraftModel $f.M $f.A ([Threading.CancellationToken]::None)}
function Deny($f,[int]$calls=0){$denied=$false;try{Run $f|Out-Null}catch{$denied=$true};Check $denied;Check ($f.M.Calls -eq $calls)}
Test 'valid-model-initial-SD-readback' {$f=Fixture;Check ((Run $f) -ceq 'MODEL_STAGE_CREATED_AND_READBACK');Check ($f.M.Calls -eq 1)}
foreach($field in @('Admin','Elevated','Ntfs','Parents')){Test "deny-$field" {$f=Fixture;$f.M.$field=$false;Deny $f}}
Test 'deny-impersonation' {$f=Fixture;$f.M.Impersonating=$true;Deny $f}
Test 'deny-actor-mismatch' {$f=Fixture;$f.A.AdministratorSid='OTHER';Deny $f}
Test 'deny-existing-stage' {$f=Fixture;$f.M.Present=$true;Deny $f}
Test 'deny-image-tamper' {$f=Fixture;$f.M.Image='0'*64;Deny $f}
Test 'deny-script-binding' {$f=Fixture;$f.A.ScriptSha='b'*64;Deny $f}
Test 'deny-profile' {$f=Fixture;$f.A.Profile='OTHER';Deny $f}
Test 'deny-path-override' {$f=Fixture;$f.A.Stage='C:\OTHER';Deny $f}
Test 'deny-empty-id' {$f=Fixture;$f.A.ApprovalId='0'*32;Deny $f}
Test 'deny-expired' {$f=Fixture;$f.A.ExpiresUtc=$f.M.Time;Deny $f}
Test 'deny-cutoff' {$f=Fixture;$f.M.Time=[DateTimeOffset]::Parse('2026-11-02T00:00:00+09:00');Deny $f}
Test 'deny-cancelled' {$f=Fixture;$c=[Threading.CancellationToken]::new($true);$denied=$false;try{Invoke-StagingDraftModel $f.M $f.A $c}catch{$denied=$true};Check $denied;Check ($f.M.Calls -eq 0)}
Test 'boundary-expiry-before-native' {$f=Fixture;$f.M.Before={param($m)$m.Time=$m.Time.AddDays(2)};Deny $f}
Test 'boundary-parent-change' {$f=Fixture;$f.M.Before={param($m)$m.Parents=$false};Deny $f}
Test 'boundary-name-race' {$f=Fixture;$f.M.Before={param($m)$m.Present=$true};Deny $f}
Test 'readback-failure-preserve' {$f=Fixture;$f.M.Readback=$false;Deny $f 1;Check $f.M.Present;Deny $f 1}
Test 'failed-create-no-retry-model' {$f=Fixture;$f.M.FailCreate=$true;Deny $f 1;Deny $f 1}
Test 'same-memory-source' {$b=[Text.Encoding]::UTF8.GetBytes('original');$sha=[Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($b)).ToLowerInvariant();$text=ConvertTo-VerifiedDraftText $b $sha;$b[0]=0;Check ($text -ceq 'original')}
Test 'source-tamper-rejected' {$b=[Text.Encoding]::UTF8.GetBytes('changed');$denied=$false;try{ConvertTo-VerifiedDraftText $b ('0'*64)}catch{$denied=$true};Check $denied}
Test 'strict-UTF8' {$b=[byte[]]@(255);$sha=[Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($b)).ToLowerInvariant();$denied=$false;try{ConvertTo-VerifiedDraftText $b $sha}catch{$denied=$true};Check $denied}
Test 'live-entry-hard-HOLD' {Check ($ast.EndBlock.Statements[-1] -is [Management.Automation.Language.ThrowStatementAst])}
Write-Output "SUMMARY PASS=$script:passed FAIL=0 WINDOWS_CALLS=0"

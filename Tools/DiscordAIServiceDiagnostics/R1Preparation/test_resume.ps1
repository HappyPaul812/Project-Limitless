param([Parameter(Mandatory)][string]$OutputPath)
$ErrorActionPreference='Stop';Set-StrictMode -Version Latest
Add-Type -TypeDefinition ([IO.File]::ReadAllText('F:\study\codex\Project-Limitless\Temp\Session0DedicatedV2Candidate20261027-r1\Code\V2\InstallAssembly.cs'))
. (Join-Path $PSScriptRoot 'ResumeCore.ps1')
$pins=@{ms='43f6afcc91eae196e015393670cd9cd4ff37194099b313c2b901177226cd32e3';ins='83c97d781fb2d9aed797b41ea6bc799b83e939ad3ffa6f7af6a2d26f1080d68f';zip='be58f8cdbe0befdcc64cd67287865c6cd6fc2611b4a706d26cda022a8014ca25';id='dedicated-install-a5af3087b78b45d1baae46067d5b7ac9';epoch='20261027-dedicated-a5af3087b78b45d1baae46067d5b7ac9';old_sha=('a'*64)}
$old=@{schema=3;kind='DEDICATED_INSTALL_ONCE';approved=$true;approval_id=$pins.id;epoch=$pins.epoch;manifest_sha256=$pins.ms;installer_sha256=$pins.ins;zip_sha256=$pins.zip;admin_sid='S-1-5-21-1-2-3-1100';expires_unix=999;preserve_existing=$true;create_bot='LIMITLESSBotV2D';create_worker='LIMITLESSWorkV2D';worker_install_state='ENABLED_BATCH_ONLY'}
function NewFake($fault=''){
 $a=[pscustomobject]@{fault=$fault;calls=[Collections.Generic.List[string]]::new();used=$false;fresh=$null}
 $a|Add-Member ScriptMethod Facts {
  $f=@{admin=$true;console_input=$true;console_output=$true;fresh_namespace=(-not $this.used);private_approval_verified=$true;stage_verified=$true;unused=$true;expired=$true;same_admin=$true;old=$script:old.Clone();old_sha=$script:pins.old_sha;admin_sid=$script:old.admin_sid;now=1000}
  if($this.fault -in $f.Keys){$f[$this.fault]=$false}
  if($this.fault -eq 'WRONG_SHA'){$f.old_sha='b'*64}
  if($this.fault -eq 'BAD_ROLE'){$f.old.create_worker='LIMITLESSRelaySvc'}
  if($this.fault -eq 'OLD_EPOCH'){$f.old.epoch='old'}
  if($this.fault -eq 'BOOL_STRING'){$f.console_input='true'}
  $f
 }
 $a|Add-Member ScriptMethod Reserve {if($this.used){throw 'DUPLICATE'};$this.used=$true;$this.calls.Add('Reserve');if($this.fault -eq 'RESERVE'){throw 'SECRET'};[IO.MemoryStream]::new()}
 $a|Add-Member ScriptMethod Preserve {$this.calls.Add('Preserve');if($this.fault -eq 'PRESERVE'){throw 'SECRET'}}
 $a|Add-Member ScriptMethod Publish {param($raw)$this.calls.Add('Publish');$this.fresh=[Text.Encoding]::UTF8.GetString($raw)|ConvertFrom-Json -AsHashtable;if($this.fault -eq 'PUBLISH'){throw 'SECRET'}}
 $a|Add-Member ScriptMethod Launch {$this.calls.Add('Launch');if($this.fault -eq 'LAUNCH'){throw 'SECRET'};if($this.fault -eq 'EXIT'){return 1};0}
 $a|Add-Member ScriptMethod Recheck {$this.calls.Add('Recheck');if($this.fault -eq 'RECHECK'){throw 'SECRET'}}
 $a|Add-Member ScriptMethod Record {param($r)$this.calls.Add('Record');if($this.fault -eq 'RECORD'){throw 'SECRET'}}
 $a
}
$rows=@()
function Check($n,$fn){try{if(-not (& $fn)){throw 'ASSERT'};$script:rows+=@{name=$n;status='PASS'}}catch{$script:rows+=@{name=$n;status='FAIL';type=$_.Exception.GetType().FullName}}}
Check 'PRESERVE_BEFORE_FRESH_APPROVAL_BEFORE_SINGLE_LAUNCH' {$a=NewFake;$r=Invoke-R1ConsoleResume $a $pins;$r.status -eq 'CHILD_RETURNED_REVIEW_INSTALLER_JSON_NOT_SUCCESS_PROOF' -and ($a.calls -join ',') -eq 'Reserve,Preserve,Publish,Launch,Recheck,Record' -and $a.fresh.expires_unix -eq 1900 -and $old.expires_unix -eq 999}
foreach($f in @('admin','console_input','console_output','fresh_namespace','private_approval_verified','stage_verified','unused','expired','same_admin','WRONG_SHA','BAD_ROLE','OLD_EPOCH','BOOL_STRING')){Check ('GATE_NO_MUTATION_'+$f) {$a=NewFake $f;$r=Invoke-R1ConsoleResume $a $pins;$r.status -eq 'HOLD' -and $a.calls.Count -eq 0}}
foreach($f in @('RESERVE','PRESERVE','PUBLISH','LAUNCH','EXIT','RECHECK','RECORD')){Check ('FAULT_HOLD_NO_RETRY_'+$f) {$a=NewFake $f;$r=Invoke-R1ConsoleResume $a $pins;$r.status -eq 'HOLD' -and $r.automatic_retries -eq 0 -and @($a.calls|Where-Object {$_ -eq 'Launch'}).Count -le 1 -and ($r|ConvertTo-Json -Depth 12) -notmatch 'SECRET'}}
Check 'DUPLICATE_RESUME_BLOCKED' {$a=NewFake;$null=Invoke-R1ConsoleResume $a $pins;$r=Invoke-R1ConsoleResume $a $pins;$r.status -eq 'HOLD' -and @($a.calls|Where-Object {$_ -eq 'Launch'}).Count -eq 1}
Check 'NEW_CORE_AND_WRAPPER_PARSE' {foreach($name in @('ResumeCore.ps1','resume-r1-console.ps1')){$t=$null;$e=$null;$null=[Management.Automation.Language.Parser]::ParseFile((Join-Path $PSScriptRoot $name),[ref]$t,[ref]$e);if($e.Count){return $false}};$true}
Check 'NATIVE_CONSOLE_NO_REDIRECT_OR_PASSWORD_ARGUMENT' {$s=Get-Content (Join-Path $PSScriptRoot 'resume-r1-console.ps1') -Raw;$line=@($s -split "`n"|Where-Object {$_ -match '^  \$child=Start-Process'});$line.Count -eq 1 -and $line[0].Contains('-NoNewWindow -Wait -PassThru') -and $line[0] -notmatch 'Redirect|Credential|Password' -and $s -notmatch '\$json\s*=\s*&|Remove-Item|Set-Acl|Clear-Content'}
$out=@{kind='NEW_RESUME_FAKE_TESTS_NOT_151';pass=@($rows|Where-Object status -eq PASS).Count;fail=@($rows|Where-Object status -eq FAIL).Count;tests=$rows;actual_approval_changes=0;actual_installer_launches=0;actual_password_reads=0}
$out|ConvertTo-Json -Depth 8|Set-Content -LiteralPath $OutputPath -Encoding utf8NoBOM;$out|Select-Object pass,fail|ConvertTo-Json
if($out.fail){throw 'RESUME_FAKE_FAILURE'}

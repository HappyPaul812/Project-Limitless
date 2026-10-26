# 같은 응답의 숫자 보고 경계만 검사합니다. 서비스/CLI/사용량은 실행하지 않습니다.
$ErrorActionPreference='Stop'
$tokens=$null;$errors=$null;$path=Join-Path $PSScriptRoot 'run-approved-usage-once.ps1'
$ast=[Management.Automation.Language.Parser]::ParseFile($path,[ref]$tokens,[ref]$errors)
if($errors.Count){throw 'REPORT_PARSER_FAILED'}
$names=@('Report-Number','Select-OfficialUsageResult')
$defs=@($ast.FindAll({param($n) $n -is [Management.Automation.Language.FunctionDefinitionAst] -and $n.Name -in $names},$true))
if($defs.Count -ne 2){throw 'REPORT_FUNCTION_SET_FAILED'}
foreach($definition in $defs){. ([scriptblock]::Create($definition.Extent.Text))}
$valid='{"source":"CODEX_APP_SERVER_OFFICIAL","observed":1000,"ordinary_allowed":true,"windows":[{"minutes":300,"used_percent":25,"resets_at":2000},{"minutes":10080,"used_percent":70,"resets_at":3000}]}'|ConvertFrom-Json
$safe=Select-OfficialUsageResult $valid
if($safe.source -ne 'CODEX_APP_SERVER_OFFICIAL' -or $safe.windows[0].used_percent -ne 25 -or $safe.windows[1].used_percent -ne 70){throw 'REPORT_NUMBERS_CHANGED'}
'NORMALIZED_NUMERIC_RESULT_COPY_PASS'
$bad=($valid|ConvertTo-Json -Depth 5)|ConvertFrom-Json
$bad|Add-Member -NotePropertyName email -NotePropertyValue 'synthetic-private@example.test'
$rejected=$false
try{$null=Select-OfficialUsageResult $bad}catch{$rejected=$_.Exception.Message -eq 'USAGE_REPORT_SCHEMA_HOLD'}
if(-not $rejected){throw 'REPORT_EXTRA_FIELD_NOT_REJECTED'}
'REPORT_EXTRA_PRIVATE_FIELD_REJECTED_PASS'
$valid.windows[0].used_percent='NaN'
$rejected=$false
try{$null=Select-OfficialUsageResult $valid}catch{$rejected=$_.Exception.Message -eq 'USAGE_REPORT_NUMBER_HOLD'}
if(-not $rejected){throw 'REPORT_INVALID_NUMBER_NOT_REJECTED'}
'REPORT_INVALID_NUMBER_REJECTED_PASS_REAL_USAGE_0'

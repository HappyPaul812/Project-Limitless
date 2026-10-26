# 변수 충돌만 검사하는 오프라인 회귀입니다. 실행기/작업/CLI/사용량은 시작하지 않습니다.
$ErrorActionPreference='Stop'
function Read-Ast([string]$path){
 $tokens=$null;$errors=$null;$ast=[Management.Automation.Language.Parser]::ParseFile($path,[ref]$tokens,[ref]$errors)
 if($errors.Count){throw 'PARSER_FAILED'};return $ast
}
function Find-Collisions($ast){
 $reserved=@('home','profile','pid','host','args','input','psscriptroot','pscommandpath','pshome','error','psboundparameters','executioncontext','myinvocation')
 $parameters=@($ast.ParamBlock.Parameters|ForEach-Object {$_.Name.VariablePath.UserPath.ToLowerInvariant()})
 $assignments=@($ast.FindAll({param($n) $n -is [Management.Automation.Language.AssignmentStatementAst] -and $n.Left -is [Management.Automation.Language.VariableExpressionAst]},$true))
 return @($assignments|ForEach-Object {$_.Left.VariablePath.UserPath.ToLowerInvariant()}|Where-Object {$_ -in $reserved -or $_ -in $parameters}|Sort-Object -Unique)
}
$deployed='C:/ProgramData/LimitlessRelay/UsageTrials/987a8f4268534a0496a9e600e7af5dd7/run-approved-usage-once.ps1'
$old=@(Find-Collisions (Read-Ast $deployed));if($old.Count -ne 2 -or 'home' -notin $old -or 'child' -notin $old){throw 'OLD_TWO_COLLISIONS_NOT_REPRODUCED'}
$new=@(Find-Collisions (Read-Ast (Join-Path $PSScriptRoot 'run-approved-usage-once.ps1')))
if($new.Count){throw 'FIXED_SOURCE_COLLISIONS_REMAIN'}
'AST_OLD_TWO_NEW_ZERO_PASS'
if((Get-Variable HOME).Options.ToString() -notmatch 'ReadOnly'){throw 'HOME_METADATA_MISMATCH'}
$typeError=& {
 param([switch]$Child)
 try{$child='synthetic-child-summary.json';throw 'EXPECTED_TYPE_ERROR'}
 catch{return @{outer=$_.Exception.GetType().Name;inner=$_.Exception.InnerException.GetType().Name}}
}
# 바깥 예외 포장은 파일/함수 문맥에 따라 달라도 내부 switch 변환 오류는 같아야 합니다.
if($typeError.outer -notin @('RuntimeException','ArgumentTransformationMetadataException') -or $typeError.inner -ne 'PSInvalidCastException'){throw 'SWITCH_BINDER_REPRO_FAILED'}
'HOME_METADATA_AND_SWITCH_BINDER_PASS'
$ok=& {
 param([switch]$Child)
 $before=(Get-Variable HOME).Value
 $childSummaryPath='synthetic-child-summary.json';$serviceCodexHome='synthetic-own-profile'
 return ($Child.IsPresent -eq $false -and $childSummaryPath -eq 'synthetic-child-summary.json' -and $serviceCodexHome -eq 'synthetic-own-profile' -and (Get-Variable HOME).Value -ceq $before)
}
if(-not $ok){throw 'NEW_BINDINGS_FAILED'}
'RENAMED_LOCALS_PRESERVE_AUTOMATIC_AND_SWITCH_VALUES_PASS'
'NEW_BINDING_REGRESSION_3_PASS_REAL_USAGE_0'

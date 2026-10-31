# 기존 문서 로더의 검증 전용 파일화입니다. 관리자 권한으로 실행하지 마십시오.
# 입력을 데이터로만 읽으며 스크립트 실행, 동적 컴파일, Windows 변경을 하지 않습니다.
#Requires -Version 7.3
param([Parameter(Mandatory)][string]$SourcePath)
Set-StrictMode -Version Latest
$ErrorActionPreference='Stop'
$expectedSha='cd3d1393612d9aadb50435e563f9525bfd70e1234eec0ca9e1a443de8ecf5d09'
if(-not [IO.Path]::IsPathFullyQualified($SourcePath) -or $SourcePath.StartsWith('\\')){throw 'HOLD_SOURCE_PATH'}
$sourceStream=[IO.FileStream]::new($SourcePath,[IO.FileMode]::Open,[IO.FileAccess]::Read,[IO.FileShare]::Read)
try {
    if($sourceStream.Length -le 0 -or $sourceStream.Length -gt 1048576){throw 'HOLD_SOURCE_SIZE'}
    $sourceBytes=[byte[]]::new([int]$sourceStream.Length)
    $sourceStream.ReadExactly($sourceBytes)
    $actual=[Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($sourceBytes)).ToLowerInvariant()
    if($actual -cne $expectedSha){throw 'HOLD_SOURCE_SHA'}
    $verifiedText=[Text.UTF8Encoding]::new($false,$true).GetString($sourceBytes)
    # 동일 메모리 텍스트도 실행하지 않습니다. 신뢰 승인과 실제 backend가 미완성입니다.
    Write-Output "SOURCE_BYTES_MATCH_ONLY_NOT_EXECUTION_APPROVAL=$actual"
} finally { $sourceStream.Dispose() }
throw 'HOLD_NO_BACKEND_NO_AUTHORIZATION_DO_NOT_EXECUTE_SOURCE'

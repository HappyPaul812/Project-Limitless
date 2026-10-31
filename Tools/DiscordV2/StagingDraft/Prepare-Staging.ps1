# 관리자 실행 금지 초안. 모의 QA는 AST에서 두 함수만 추출하며 이 파일을 실행하지 않습니다.
# 최초 신뢰는 관리자 권한이 아니라 별도 신뢰 채널의 승인 SHA와 동일 메모리 바이트 검증에 둡니다.
#Requires -Version 7.0
Set-StrictMode -Version Latest

function ConvertTo-VerifiedDraftText {
    param([byte[]]$Bytes,[string]$ExpectedSha)
    if($ExpectedSha -cnotmatch '^[0-9a-f]{64}$' -or $Bytes.Length -eq 0 -or $Bytes.Length -gt 1048576){throw 'HOLD_SOURCE_INPUT'}
    $actual=[Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($Bytes)).ToLowerInvariant()
    if($actual -cne $ExpectedSha){throw 'HOLD_SOURCE_SHA'}
    # 검증한 동일 바이트를 엄격한 UTF8로 해석합니다. 경로를 다시 열어 실행하지 않습니다.
    return [Text.UTF8Encoding]::new($false,$true).GetString($Bytes)
}

function Invoke-StagingDraftModel {
    param($HostModel,$Approval,[Threading.CancellationToken]$Cancellation)
    $stage='C:\ProgramData\LimitlessDiscordV2BootstrapStaging'
    $image='2ff5669350a6761a7eb979f6133a4b0a85b44f5440f96a05644a57601026349e'
    $sddl='O:BAG:SYD:P(A;;FA;;;SY)(A;;FA;;;BA)'
    $cutoff=[DateTimeOffset]::Parse('2026-11-01T23:59:59+09:00')
    # HostModel 계약: 관찰은 부작용 없음, AttemptOnce는 같은 승인 재사용 거부,
    # CreateNewWithInitialSecurity는 실제 호출 직전 경계를 다시 실행해야 합니다.
    $guard={
        if($Cancellation.IsCancellationRequested){throw 'HOLD_CANCELLED'}
        $now=$HostModel.Now()
        if($now -gt $cutoff -or $Approval.ExpiresUtc -le $now -or $Approval.ExpiresUtc -gt $cutoff){throw 'HOLD_EXPIRED'}
        if($Approval.Profile -cne 'DISCORD_V2_INITIAL_STAGING_V1' -or $Approval.Stage -cne $stage){throw 'HOLD_SCOPE'}
        $id=[guid]::Empty
        if(-not [guid]::TryParseExact($Approval.ApprovalId,'N',[ref]$id) -or $id -eq [guid]::Empty -or $id.ToString('N') -cne $Approval.ApprovalId){throw 'HOLD_APPROVAL_ID'}
        if($Approval.ScriptSha -cnotmatch '^[0-9a-f]{64}$' -or $Approval.ScriptSha -cne $HostModel.VerifiedScriptSha()){throw 'HOLD_SCRIPT_BINDING'}
        $actor=$HostModel.Actor()
        if(-not $actor.Elevated -or -not $actor.Admin -or $actor.Impersonating -or $actor.Sid -cne $Approval.AdministratorSid){throw 'HOLD_ACTOR'}
        if(-not $HostModel.FixedNtfs()){throw 'HOLD_VOLUME'}
        $HostModel.ValidatePinnedParents($Approval.ParentBindings)
        if($HostModel.Exists($stage)){throw 'HOLD_EXISTS'}
        if($HostModel.PinnedImageSha() -cne $image){throw 'HOLD_IMAGE_SHA'}
    }.GetNewClosure()
    & $guard
    # 원장 기반의 최초 승인 소비를 구현했다고 주장하지 않습니다. 실제 admission은 아직 없습니다.
    $HostModel.AttemptOnce($Approval.ApprovalId)
    try {
        $HostModel.CreateNewWithInitialSecurity($stage,$sddl,$guard)
        $HostModel.VerifyCreatedHandle($stage,$sddl)
        return 'MODEL_STAGE_CREATED_AND_READBACK'
    } catch { throw 'HOLD_PRESERVE_PARTIAL_NO_RETRY' }
}

# 실제 Windows backend/운영 승인 소비를 연결하지 않았습니다. bool/환경변수/옵션으로 열리지 않습니다.
# 모의 PASS만으로 특권 스크립트를 배포하지 않기 위한 명시적 정지점입니다.
throw 'HOLD_DRAFT_ONLY_NO_WINDOWS_BACKEND_OR_EXECUTION_ADMISSION'

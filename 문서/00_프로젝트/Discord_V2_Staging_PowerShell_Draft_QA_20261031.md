# 최초 staging 관리자 PowerShell 초안 / 모의 25 PASS / 실제 적용 HOLD

기준 `0d0c15a304bd76711c2e80cd88cfa63fa6121bef`. 브랜치 `codex/discord-v2-staging-powershell-draft-20261031`. GitHub #66·최신 로컬 인계 확인 후 새 EXE 없이 초안/표적 QA만 추가했습니다.

## 완료와 한계

- `Tools/DiscordV2/StagingDraft/Prepare-Staging.ps1`: 동일 메모리 바이트 SHA/엄격 UTF8 검증 함수, 고정 외부 staging 모형 제어 함수. 직접 실행은 마지막 무조건 HOLD로 중단합니다. bool·환경변수·옵션으로 열 수 없습니다.
- `StageSecurityApi.cs`: CreateDirectoryW/SECURITY_ATTRIBUTES 선언 초안. 관리형 인메모리 컴파일 PASS. 실제 P/Invoke 호출부는 없습니다. 기존 WindowsBootstrapHost의 초기 SD 생성 방식 재사용 지점을 명시했습니다.
- `Test-StagingModel.ps1`: 관리자 스크립트 전체를 실행하거나 dot-source하지 않습니다. AST에서 지정된 함수 두 개만 추출하여 합성 Fake에 연결합니다. **25 PASS / 0 FAIL / 실제 Windows 호출 0회**, 첫 실행 통과. 기존 59 PASS 및 완료 시험 재실행 0회.
- 실제 backend, 실제 부모 ACL/토큰/HANDLE/소유자/readback, 실제 이미지 복사·봉인, 프로세스 간 영구 승인 재사용 차단은 **NOT_VERIFIED/미구현**입니다. 모형 결과를 실제 보안 PASS로 승격하지 않습니다. 단일 실사용 관리자 절차 완성은 HOLD입니다.

## 정확한 신규 검증

정상 모형 1회, 관리자/상승/NTFS/부모 조건 각 거부, 위장·다른 계정·기존 대상·이미지 변조·스크립트 결합 불일치·다른 프로필·경로 override·빈 승인 ID·만료·cutoff·취소 거부. Native 경계 직전 만료/부모 변경/이름 선점 모의 시 생성 호출 0회. readback 실패 시 모형의 부분 결과 보존, 생성 실패 후 같은 모형 승인 재시도 거부. 동일 메모리 텍스트 고정, 잘못된 원문 SHA/UTF8 거부, 실제 진입 HOLD 확인. 총 25개입니다. 모형의 Attempts HashSet은 **인메모리**이며 영구 소비 원장이 아닙니다. 다른 프로세스의 재사용을 막았다고 주장하지 않습니다.

## 출처·무결성·실행 중 교체 경계

관리자 권한 자체는 신뢰 근거가 아닙니다. 로컬 Git commit은 코드의 식별 근거이며 서명·독립 승인 증명이 아닙니다. 이 보고/콘솔/원문 코드의 검토와 대표가 별도 신뢰 채널에서 확인한 정확한 원문 SHA가 필요합니다. 같은 수정 가능한 디렉터리의 .sha256 파일을 신뢰 루트로 사용하지 않습니다.

현재 작업 파일의 raw SHA256은 `CD3D1393612D9AADB50435E563F9525BFD70E1234EEC0CA9E1A443DE8ECF5D09`입니다. 줄바꿈/인코딩 변경도 다른 SHA로 거부해야 합니다. 이 SHA의 기록은 **관리자 실행 승인 발급이 아닙니다**.

실행 중 교체는 경로를 검사하고 다시 실행하는 방식 대신, 한 번 읽은 원문 bytes를 승인 SHA와 대조하고 그 **동일 메모리 bytes에서 만든 텍스트**만 사용하는 구조로 줄입니다. 모의 QA는 검증 후 입력 버퍼가 바뀌어도 이미 고정한 텍스트가 유지됨을 확인했습니다. 이는 호스트/로더 자체의 출처나 부모 HANDLE 경합 안전성을 증명하지 않습니다.

관리자 콘솔의 최초 로더는 별도로 직접 검토해야 합니다. 프로필/모듈/동명 함수·기존 Add-Type 타입 오염, 시스템 호스트의 출처, 실제 관리자 계정, 스크립트 원문 승인, 컴파일 입력과 native 호출 경계를 확정하지 못했으므로 여기서 더 많은 도구를 만들지 않고 중지합니다.

## 대표 GUI와 관리자 명령 준비

대표 GUI 확인: 신뢰된 Windows PowerShell/PowerShell 호스트의 설치 출처와 실제 계정, UAC에 표시된 프로그램/계정, 새 이미지에 대한 V3 판정. 비밀 암호를 보고서·채팅에 공유하지 않습니다. 보안 경고 우회·실행정책 Bypass·V3 예외는 사용하지 않습니다. 이번에는 GUI/UAC 조작을 하지 않았습니다.

다음은 **미래 별도 승인 후의 검토용 로더 초안**입니다. 지금 실행하지 마십시오. Windows 변경이나 스크립트 실행이 아니라, 같은 메모리 원문 SHA 확인과 텍스트 고정까지만 표현합니다. 고정 원문 SHA는 별도 채널에서 대표가 확인해야 합니다.

```powershell
# 별도 승인 후 신뢰된 관리자 콘솔에서만. 절대경로는 검토 완료한 실제 경로로 확정해야 합니다.
$seedFile = '<검토한 Prepare-Staging.ps1 절대경로>'
$expectedSeedSha = 'cd3d1393612d9aadb50435e563f9525bfd70e1234eec0ca9e1a443de8ecf5d09'
$seedStream = [IO.FileStream]::new($seedFile,[IO.FileMode]::Open,[IO.FileAccess]::Read,[IO.FileShare]::Read)
try {
    if ($seedStream.Length -le 0 -or $seedStream.Length -gt 1048576) { throw 'HOLD_SOURCE_SIZE' }
    $seedBytes = [byte[]]::new([int]$seedStream.Length)
    $seedStream.ReadExactly($seedBytes)
    $seedActual = [Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($seedBytes)).ToLowerInvariant()
    if ($seedActual -cne $expectedSeedSha) { throw 'HOLD_SOURCE_SHA' }
    $verifiedSeedText = [Text.UTF8Encoding]::new($false,$true).GetString($seedBytes)
} finally { $seedStream.Dispose() }
# 실행/Invoke-Expression/ScriptBlock 호출은 추가하지 않습니다. 현재 초안에는 실제 Windows backend가 없습니다.
```

위 로더도 이번에 실행하지 않았습니다. 실제 생성 명령을 완성한 것으로 취급하지 않습니다. PowerShell/.NET 최소 버전과 로더 자체의 검증도 실제 적용 선행 조건입니다. 초안 본문에 Invoke-Expression을 추가하거나 마지막 HOLD를 삭제하는 것은 새로운 코드 변경이며 별도 검토 대상입니다.

## 실제 적용 승인 범위 및 예상 변경

예상 최초 생성 대상은 **오직 `C:\ProgramData\LimitlessDiscordV2BootstrapStaging` 신규 폴더 1개**입니다. 기존 `C:\ProgramData\Limitless\DiscordV2`와 분리됩니다. 여섯 Bootstrap 대상 전체 부재 계약 유지, 기존 C:\/ProgramData ACL 변경 금지. 초기 예상 SD는 `O:BAG:SYD:P(A;;FA;;;SY)(A;;FA;;;BA)`이며 사후 ACL 수정 경로를 두지 않습니다. leaf는 실제 생성 전 존재하면 거부, 부모 owner/DACL/reparse/최종 경로/FileKey/HANDLE을 pin 및 변경 직전 재검증해야 합니다. 현재 실제 모형은 이를 HostModel 계약으로만 나타냅니다.

별도 승인에는 검증된 최종 스크립트/로더 SHA와 출처·실제 관리자 SID·정확한 신규 일회성 승인 식별/시간창·부모 바인딩·정확한 Windows 변경 목록·영구 재사용 차단 및 실패 기록 위치를 포함해야 합니다. 현재 합성 QA 값은 실제 SID/승인 자료가 아닙니다. 승인 JSON이나 실운영 binding을 발급하지 않았습니다.

실패 시 자동 삭제·ACL 수선·기존 자료 덮어쓰기·자동 재시도 없이 보존/HOLD합니다. 실제 cross-process 재사용 차단이 증명되기 전에는 실행하지 않습니다. staging 폴더 생성은 Bootstrap 이미지 복사·봉인 또는 Bootstrap 실행 승인과 같지 않습니다. 이후 이미지 복사/승인 자료 봉인/Bootstrap 실행은 정확한 변경 범위와 승인 조건을 별도로 확정해야 합니다.

기존 Bootstrap SHA `2FF5669350A6761A7EB979F6133A4B0A85B44F5440F96A05644A57601026349E`·2,951,168바이트, Probe `0BABC05A2E0B24B1E6563D049B8FAABD7375F737B2ACD5AEDEAF840FC5E59322`·1,892,352, Supervisor `6DF94E47C301C20EAB537EB6AD49C680129B80787536358E6BDC78ED50A842E7`·7,559,680은 기존 근거 재사용이며 이번 재측정/변경/실행 0회입니다. 기존 제품 소스·Native binding diff0.

## 중지 및 11월7일 인계

남은 HOLD는 최초 로더 출처 승인, 실 Windows backend/원본 봉인/영구 재사용 차단 검증, 실제 관리자·부모 HANDLE 검증, 새 이미지 V3 공식 판정 NOT_VERIFIED입니다. 추가 프로그램/NativeAOT를 제작하지 않았습니다. 실Windows변경·폴더 생성·ACL 변경·제품 EXE 실행·계정/Task/OnBoot/V3/Unity/Save변경0, Push/main병합0.

NO_GO / MANUAL / EXECUTION_HOLD. 기존 cutoff **2026-11-01 23:59:59 KST** 자동 연장 금지. 11월2일 평일 게임 개발 복귀, 11월7일 Discord 재개 시 새 지시/시간창·계정 신뢰와 영향 코드/이미지 승인부터 검토합니다. 기존 이미지나 옛 승인 자동 재사용 금지. 오늘은 초안/모의 QA 결과로 인계하고 중지합니다.

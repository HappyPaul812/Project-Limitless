# 최초 staging 최종 정적 판정 — HOLD

기준 `142e67ad01726ef1c415f3315c159f58bcc2afa5`. 2026-10-31. GitHub #66 및 CODEX_LATEST의 신규 25 PASS / 0 FAIL 인계를 확인했습니다. 기존 시험·빌드·컴파일을 반복하지 않았습니다. 이번에는 기존 문서 로더를 검증 전용 파일로 옮기고 AST 구문만 확인했습니다. 관리자 스크립트와 로더 모두 실행하지 않았습니다.

## 최종 식별값

이 값은 현재 **초안의 원문 바이트 식별값**입니다. 실사용 완성본·서명·실행 승인이라는 뜻이 아닙니다.

|파일|SHA256|
|---|---|
|Tools/DiscordV2/StagingDraft/Prepare-Staging.ps1|CD3D1393612D9AADB50435E563F9525BFD70E1234EEC0CA9E1A443DE8ECF5D09|
|Tools/DiscordV2/StagingDraft/Verify-StagingSource.Loader.ps1|504BD435029E0D9754C82170502FB43B5481E11C4A86F15D3DB5D53D97DE3A32|
|Tools/DiscordV2/StagingDraft/StageSecurityApi.cs|5A568CFA5784D84DC859BA35F63A5EB532FDEE15DFCD8AEE661200BFF902E439|

줄바꿈·인코딩 변경도 다른 바이트이며 SHA 불일치 시 중지해야 합니다. Git checkout의 CRLF 변환도 예외가 아닙니다. 임의 정규화 후 같다고 통과시키지 않습니다. 기존 모의 QA 파일 및 위 두 기존 초안 파일은 변경하지 않았습니다.

## READY가 아닌 구체적 이유

1. Prepare-Staging.ps1의 생성/검증은 HostModel 인터페이스에 대한 모형입니다. Windows backend가 없습니다. 마지막 진입은 무조건 HOLD입니다.
2. StageSecurityApi.cs는 SECURITY_ATTRIBUTES/CreateDirectoryW 선언뿐이며 호출부가 없습니다. 실제 관리자 토큰, 부모 HANDLE pin, owner/DACL/최종 경로/FileKey readback을 수행하지 않습니다. 기존 API 선언 컴파일 PASS는 구현 완성의 근거가 아닙니다.
3. AttemptOnce 모형은 인메모리 집합이며 프로세스 간 영구 재사용을 차단하지 않습니다. 신뢰할 기록 위치와 실패 후 재사용 거부 절차가 미확정입니다.
4. 로더의 원문 SHA 일치는 파일 식별만 제공합니다. 로더 자체의 승인된 출처·독립 신뢰 채널, 실제 관리자 계정과 호스트 신뢰를 확정하지 않았습니다. 자기 SHA/같은 폴더의 checksum 파일/관리자 권한 자체로 이를 증명하지 않습니다.
5. 기존 Bootstrap 원본의 안전한 HANDLE 읽기·봉인 복제 및 승인 자료 발급도 이번 범위에서 완성되지 않았습니다. 미확정 SID/ID/Approved=true 또는 신뢰 판정을 생성하지 않았습니다.

따라서 최종 판정은 **HOLD**, READY_FOR_USER_APPROVAL이 아닙니다. 마지막 throw를 삭제하거나 모형 backend를 실제 쓰기에 연결하는 변경은 단순 사용법 보완이 아닙니다. 이 미완성 부분을 검증하지 않고 대표가 실행할 생성 명령을 발급할 수 없습니다. 별도 새 EXE나 추가 보안 프로그램을 제작하지 않았습니다.

## 로더의 정확한 동작과 명령

로더는 SourcePath를 데이터로 한 번 열고 쓰기/삭제 공유를 허용하지 않은 FileStream에서 제한된 크기의 bytes를 읽어 SHA 및 엄격 UTF8을 확인합니다. 그 동일 메모리 텍스트도 실행하지 않습니다. Add-Type/Invoke-Expression/ScriptBlock 호출이나 Windows 변경은 없습니다. 마지막은 HOLD 예외입니다. 구문 오류 0개·마지막 throw AST 확인만 수행했고 로더 실행/새 모의 시험은 하지 않았습니다.

아래는 **향후 검토용 비관리자 데이터 검증 명령**입니다. 이번에는 실행하지 않았습니다. 관리자 실행·생성 명령이 아니며 성공하더라도 최종 HOLD로 끝납니다.

```powershell
& 'F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\Tools\DiscordV2\StagingDraft\Verify-StagingSource.Loader.ps1' -SourcePath 'F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\Tools\DiscordV2\StagingDraft\Prepare-Staging.ps1'
```

신뢰하지 않은 로더를 검증 없이 실행하라는 지시로 사용하면 안 됩니다. 대표가 먼저 정확한 원문/별도 채널 SHA와 출처를 확인해야 합니다. 위 F 경로는 개인 작업트리이지 보호된 관리자 실행 배포 경로가 아닙니다. 이 파일들을 -Verb RunAs/-ExecutionPolicy Bypass로 실행하지 않습니다. 실제 생성 명령은 **미발급**입니다.

## 향후 승인 대상 계약과 GUI

최초 생성 대상은 `C:\ProgramData\LimitlessDiscordV2BootstrapStaging` 신규 폴더 하나입니다. 기존 Bootstrap의 여섯 생성 대상 밖이며 전체 부재 계약을 유지합니다. 생성 시 초기 예상 SDDL은 `O:BAG:SYD:P(A;;FA;;;SY)(A;;FA;;;BA)`. C:\ 및 C:\ProgramData ACL은 변경하지 않습니다. 기존 경로·reparse·경합·검증 불명은 거부하고 실패 시 기존 자료/부분 결과를 보존하며 자동 삭제·수선·재시도하지 않습니다. 이 조건들은 실backend 검증 전의 계약이며 실제 적용 PASS가 아닙니다.

실제 승인 제출에는 완성·검증된 스크립트/로더의 새 SHA, 승인된 출처와 실제 관리자 계정, 정확한 일회성 승인/시간창·부모 바인딩·영구 소비 기록·실패 처리 범위를 포함해야 합니다. 현재 파일의 SHA 승인만으로 미구현 backend가 완성되거나 실행이 허용되지 않습니다. Bootstrap 이미지 복사/승인 자료 봉인/Bootstrap 실행/Task·FAKE_ONLY는 분리된 승인 대상입니다.

비밀이 아닌 자료: 코드 SHA·대상/작업 범위·유효기간·검증 결과·승인 식별/서명 정책. 실제 계정 SID·경로 binding·승인 원문은 로컬 비공개로 관리하며 미확정값을 채우지 않습니다. 암호/토큰/자격증명은 수집·공유·Git 기록하지 않습니다.

대표 GUI 확인은 의도한 관리자 계정 및 신뢰된 호스트/원문의 출처, 향후 UAC에 표시되는 실제 프로그램/계정, V3의 새 이미지 판정입니다. UAC 동의는 코드 신뢰/범위 승인 대체물이 아닙니다. 현재는 UAC를 띄우거나 관리자 콘솔에서 본 파일을 실행하지 마십시오. 미확인 보안 경고는 우회하지 않습니다.

## 보존과 인계

기존 25 PASS / 0 FAIL은 모형 검증 기록으로 그대로 보존했습니다. 기존 59 PASS 및 다른 시험 반복 0회. 기존 Probe/Supervisor/Bootstrap 소스·이미지·binding diff0, 실제 Windows 생성·ACL/계정TaskOnBootV3/UnitySave 변경·EXE실행·NativeAOT빌드0. Push/main병합0.

브랜치 `codex/discord-v2-staging-final-readiness-hold-20261031`, 코드 기준 `142e67ad01726ef1c415f3315c159f58bcc2afa5`. 신규 추가는 기존 로더의 검증 전용 파일화 및 인계 문서뿐입니다. 전체 실사용 절차는 미완료 상태로 11월7일 인계합니다. 다음 작업은 새 도구 제작이 아니라 기존 관리자 스크립트의 실backend/최초 신뢰·영구 재사용 차단 계약을 하나의 검증 가능한 범위로 확정하는 것입니다.

NO_GO / MANUAL / EXECUTION_HOLD. 기존 cutoff **2026-11-01 23:59:59 KST** 자동 연장·옛 승인 재사용 금지. 11월2일부터 평일 게임 개발 복귀, 11월7일 Discord 재개 시 새 지시/시간창·영향 코드/이미지 검토가 선행돼야 합니다.

# Supervisor 핵심·관리자 CLI 구현 결과 — 2026-10-28

기준 `c97ef5542bbe40e9388d01125a6d67de73dfd34b` / Issue66 댓글6461020709. 기존 두 프로젝트를 재사용했습니다. **소스 구현·단위/모의 시험 완료 범위와 실제 Windows 검증을 구분하며 NO_GO / MANUAL / HOLD를 유지합니다.**

## 실제 구현

- `Tools/DiscordV2/NativeBatch/Supervisor/AdmissionPolicy.cs`, `ReleaseAdmission.cs`, `OperationApproval.cs`: strict 승인 Scope, 동작별 승인과 동일 계약 SHA 결합, 보호된 관리자 등록/시작/instance 기록 확인, 영구 단발 소비와 중복 차단. 등록 승인만으로 시작할 수 없습니다. 기존 소비 기록 초기화·재사용0.
- `AdmissionWindows.cs`, `TokenPolicy.cs`, `NativeInterop.cs`, `HandleCensus.cs`: AccessCheck·그룹/위험 특권, 경로 pinning/reparse·파일 교체 방지, kernel HANDLE 보호·비상속, 외부 HANDLE 및 SID 독점 검사. 실제 선언은 공통 소스로 옮겼으며 보호할 SID/Task/경로 상수만 로컬 비공개 binding에 남겼습니다. Probe도 같은 공통 코드에 연결했습니다.
- `AdminController.cs`, `AdminWindows.cs`, `AccountNative.cs`, `TaskPolicy.cs`, `TaskSchedulerNative.cs`: 계정·Batch 권리·이미지 SHA 사전 검사, create-only 등록/읽기검증, 단발 시작·instance 기록·finally Disable, 정확 instance만 중지, Disabled·instance0·fingerprint·정리 근거 조건부 삭제. NativeAOT의 내장 COM에 의존하지 않는 명시적 vtable 어댑터입니다. 실Windows 어댑터는 컴파일만 했고 호출하지 않았습니다.
- `JobMonitor.cs`: 기존 Job 감독 루프에 취소·오류 시 자기 Job 종료/정리를 연결했습니다. 단조 시간 기준10초 강제 종료,85초 cutoff,90초 drain과 실행 중 독점 재검사를 production과 시험이 공유합니다. OS 정지·sleep·API block에 대한 절대 wall-clock 보장으로 표현하지 않습니다.

발견·수정한 결함은 위험 HANDLE 권리 누락, 조회/권리0 외부 Job HANDLE의 수명 연장, 느슨했던 소비 기록 ACL/owner 판정, primary token 역할 확인의 불필요한 duplicate 권리 의존, 동작별 승인/시작 intent 결합 누락, 미확정 Batch 문맥 SID의 부재 가정, Task 상태와 instance 수의 불일치, 취소·응답 유실 경로입니다. DLL 검색은 System32로 제한했습니다. 실행 허용 flag를 true로 바꾸지 않았습니다.

## 신규 검증

| 항목 | 결과 / 범위 |
|---|---|
| 신규 실제 정책·소비·관리자 흐름·Job 루프 시험 | **52 PASS / 0 FAIL** |
| AccessCheck | 현재 호출 token + 메모리 SD 읽기/쓰기 판정. 대상 계정·파일 ACL 변경0 |
| 원자 소비 | 새 시험 전용 파일에서 CREATE_NEW 경합·flush 실패·재소비 거부 |
| x64 ABI | 관리 size/offset 시험 및 설치된 Microsoft SDK 헤더 `/Zs /utf-8` 구조체·COM vtable static_assert 통과 |
| Supervisor / Probe 일반 컴파일 | 각각 오류0·경고0, 실행0 |
| NativeAOT | **시도0 / 생성0 / SHA 없음** |

최초 AccessCheck 양성 fixture는 AppContainer에서 Everyone 허용만으로 충분하지 않아 실패했습니다. 현재 사용자/AC와 제한 SID(있을 때)에 읽기만 허용한 메모리 fixture로 수정했고, production ACL 정책은 완화하지 않았습니다. 현재 시험 token은 AppContainer1/restricting SID0으로 관측됐습니다. Probe의 첫 no-restore 일반 컴파일은 이전 실패 obj의 NU1100을 재표시했으며, 원본을 보존하고 새 격리 obj에서 일반 C# 컴파일을 완료했습니다. NativeAOT 실패 재시도가 아닙니다.

SDK10.0.403의 인증서 자동생성·tools PATH 추가·telemetry·workload 알림을 자식 환경에서 차단하고 격리 CLI home/sources-clear config를 사용했습니다. 이번 새 인증서 설치 메시지는 없었습니다. 현재 도구 문맥의 CurrentUser/My 조회에서 ASP.NET 개발 인증서 OID 일치0이며 이전 전체 사용자 저장소 변화는 baseline 부재로 여전히 UNKNOWN입니다. 신뢰/삭제/설정 변경 명령0.

## 남은 HOLD와 3단계 판정

**필수 보안 실검증이 끝나지 않아 사용자 지시에 따라 NativeAOT 빌드를 하지 않았습니다.**

1. 외부 HANDLE의 NtQuerySystemInformation class64는 공개 안정 ABI가 아니며 SDK 구조체 시험으로 실제 kernel table 지원성을 증명할 수 없습니다. redaction·접근 거부·layout 불명은 HOLD입니다.
2. 제한 SID의 시스템/PPL process 조회가 거부되면 전체 SID 독점은 UNKNOWN입니다. Scheduler의 외부 관리 HANDLE도 임의로 예외 처리하지 않았습니다. Job HANDLE은 조회 전용/권리0도 외부 소유를 허용하지 않습니다.
3. 실제 kernel DACL/OWNER RIGHTS·file sharing pinning·COM 등록/readback·실패 정리를 실제 경로에서 검증하지 않았습니다. 현 승인에 없는 ACL·Task·Session0 실행으로 이를 채우지 않았습니다. Scheduler가 추가하는 미확인 XML 기본값도 HOLD입니다.

실제 시험 전에는 이 보안 관측·운영 경계를 검증할 승인 범위를 별도로 확정해야 하며, 그 결과 후에만 NativeAOT 각1회·이미지SHA 봉인→정확 신규 자산/권한 변경 승인→단발 시험 승인 순서로 진행할 수 있습니다. 현재 실행 gate=false, 승인 계약/동작별 승인 템플릿도 모두 미승인입니다. 명령 발급 또는 실제 시험 준비 완료라고 보고하지 않습니다. 추가 설치 경로를 만들지 않았습니다.

NuGet7 PASS·Catalog/HTTP/hash 실패 자료·이전 소스 archive와 기존 승인 소비 기록 보존. E 공식 패키지 재다운로드/재검증0이며 NativeAOT 단계가 보류되어 해당 feed 복원/발행도 수행하지 않았습니다. 실제 Task 등록/시작/중지/삭제·계정·권리·파일 ACL·OnBoot·Job/Session0/Probe·AI/업무IPC·Unity/Save·운영 Discord 변경0. 도구 내부 컴파일/현재 token의 메모리 AccessCheck와 대상 시험 실행을 혼동하지 않습니다. Push/main 병합0.

소스·binding·기존 프로젝트·미승인 템플릿·실패 이력·시험 로그는 새 Git 제외 checkpoint로 보존했고 기존 archive를 덮어쓰지 않았습니다. 정확 경로/명령·보호 정보는 로컬 상세 인계에만 기록합니다. 테스트 프로젝트와 fixture는 공개 가능한 source이며 기존 시스템 PASS를 반복하는 별도 검증기를 만들지 않았습니다.

브랜치 `codex/discord-usage-diagnostics-20261026`, 소스/보고 로컬 커밋 / `REMOTE_NOT_PUSHED`. 최종 SHA는 완료 보고와 Issue66 댓글에 기록합니다. 2026-11-01 마감 유지, 완료 보고 후 중지합니다.

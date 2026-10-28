# NativeBatch Supervisor 소스와 시험

기존 Probe/Supervisor 두 프로젝트가 이 폴더의 공통 코드를 링크합니다. 새로운 실행 아키텍처나 운영 진입점은 아닙니다. 실제 배포 SID·Task/경로 상수와 프로젝트 준비본은 Git 제외 로컬 자료에 두며, 공개 시험은 존재하지 않는 `Native.TestBinding.cs`만 사용합니다.

`Supervisor.Core.projitems`는 실제 Supervisor 구현 입력입니다. `NativeInterop.cs`는 두 프로그램이 공유하는 Windows ABI, `TokenPolicy.cs`는 TOKEN_QUERY만으로 위험 그룹을 거부하는 공통 코드입니다. `AdminController`는 실제 Windows 어댑터와 모의 backend가 공유하는 등록/시작/정리 순서입니다. Windows 어댑터에는 기존 Task 갱신·계정/권리 부여·ACL staging 수정 코드가 없습니다. 신규 Task 생성과 자기 runtime kernel object 보호 코드는 실제 실행 승인이 있어야만 사용할 수 있습니다.

시험은 `Tests/Supervisor.Tests.csproj`의 별도 managed 진입점입니다. 파일 단발 소비 경합은 새 시험 전용 폴더에서만 시험하고 기존 소비 기록은 만지지 않습니다. Windows AccessCheck는 메모리 SD와 현재 호출 token에만 적용하며 파일/계정 ACL을 바꾸지 않습니다. AppContainer의 양성 fixture는 현재 사용자/AC와 restricting SID(있는 경우)에 읽기만 허용합니다. 이는 production ACL 정책 변경이 아닙니다. NativeAbi.cpp는 설치된 Microsoft SDK 헤더에 대한 `/Zs /utf-8` 구문 검사만 하며 실행 이미지를 만들지 않습니다.

모든 dotnet 호출에는 자식 환경으로 `DOTNET_GENERATE_ASPNET_CERTIFICATE=false`, `DOTNET_ADD_GLOBAL_TOOLS_TO_PATH=false`, `DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1`, `DOTNET_CLI_TELEMETRY_OPTOUT=1`, `DOTNET_CLI_WORKLOAD_UPDATE_NOTIFY_DISABLE=true` 및 격리 `DOTNET_CLI_HOME`을 지정합니다. 테스트의 `LIMITLESS_TEST_ROOT`도 새 로컬 시험 전용 폴더로 고정해야 합니다. 설치된 SDK reference만 사용하는 sources-clear NuGet.Config로 일반 컴파일합니다. 전역 설정을 바꾸거나 추가 패키지를 받지 않습니다.

## 승인 경계

TrialContract의 strict Scope는 SID/Task·코드/증거 경로·FAKE_ONLY/noIPC/noAI·Supervisor1/Probe1·90초·허용 동작을 고정합니다. 실제 각 동작 승인에는 별도 protected OperationApproval이 필요하며 같은 trial/contract SHA/동작/만료에 묶입니다. 이로써 등록 승인만으로 시작할 수 없고 이후 시작 승인 때 기존 Task Action이나 기본 계약 SHA를 변경할 필요가 없습니다. 승인 파일을 자동 발급하지 않습니다. 관리자 정리도 별도 Cleanup 범위와 동작별 승인이 필요합니다.

등록은 create-only/초기Disabled/Trigger0/Password/Limited 및 원래 계획의 자동 principal ACE 추가 금지 조건을 사용합니다. 시작은 영구 intent를 먼저 쓰고 Enable→Run→instance 보존→finally Disable 순서이며 응답 유실도 재실행 사유가 아닙니다. 중지는 저장한 정확 instance GUID만 사용하고 Task 전체 Stop은 호출하지 않습니다. 삭제는 Disabled·instance0·동일 fingerprint 및 시작 이력이 있으면 정리 근거까지 필요합니다. 파일·소비 기록을 삭제하거나 초기화하지 않습니다.

Supervisor도 관리자 소유의 등록/시작 intent와 해당 승인 ID를 검증하고, Probe 재개 전에 반환된 instance 기록까지 요구합니다. 이 기록은 Worker에게 읽기만 허용하며 Worker 소유 소비 파일과 구분합니다. 기록이 아직 없거나 응답이 유실됐으면 재개하지 않고 HOLD합니다.

## 아직 해소되지 않은 실행·빌드 HOLD

`ReleaseAdmissionImplemented=false`는 유지합니다. 시험 성공은 실제 Windows Task/Session0/Job 실행 또는 배포 승인 PASS가 아닙니다. 특히 아래를 해결하지 않고 NativeAOT 빌드·실행 준비 완료로 판정하지 않습니다.

- 외부 HANDLE 관측의 NtQuerySystemInformation class64는 공개 안정 ABI가 아닙니다. SDK 구조체 대조나 합성 buffer 시험으로 실제 kernel table 지원성을 인증할 수 없습니다. 주소 redaction·접근 거부·알 수 없는 layout은 HOLD입니다.
- 제한 SID가 시스템/PPL process의 SID를 조회하지 못하면 전체 SID 독점 관측은 UNKNOWN입니다. SYSTEM/관리자 PID라는 추측으로 제외하지 않습니다. Scheduler의 정상 외부 handle도 위험 권리를 가지면 HOLD입니다.
- 보안 descriptor 생성/실제 kernel object DACL·OWNER RIGHTS·rename/share 잠금·COM 등록 readback 및 실패 시 정리는 아직 실제 경로에서 검증하지 않았습니다. 실제 ACL/Task 변경 금지 범위를 시험 fixture로 우회하지 않습니다.
- Task Scheduler가 주입하는 추가 XML 기본값은 모르는 항목이면 등록 후 HOLD로 남깁니다. 승인 없이 원격 조회/등록/삭제로 알아내지 않습니다.
- 90초는 단조 deadline과 기존 Scheduler 한도의 계약입니다. OS 정지·sleep·API block에 대한 절대 wall-clock 보장이 아닙니다. 감독 중에도 주기적/종료 직전 독점 검사를 실패하면 자기 Job 종료·HOLD로 처리합니다.

NativeAOT 생성물과 Task/계정/ACL·실제 단발 실행 승인은 이 소스·시험과 별개입니다. 기존 NuGet PASS·Catalog 실패 원본과 승인 소비 기록은 보존합니다.

## 승인된 FAKE_ONLY 전용 독점성 확장

기본 계약의 전역 동일SID 검사는 유지합니다. 선택적 `FakeOnlyIsolation`의 `FAKE_ONLY_EXECUTION_V1`만 전용 시험 범위를 적용하며 `GlobalSidAbsence=UNKNOWN`으로 남깁니다. 관리자 소유의 신뢰 진술·검토 원문/정확 SHA·trial/이미지/자산·유효 시각이 없으면 Supervisor와 관리자 CLI는 HOLD합니다. 실제 계정 사용에 대한 신뢰 근거를 합성 시험이나 위험 수용 승인으로 대체하지 않습니다. Job/HANDLE/단발 소비·lease/정리·noIPC/noAI/실행 게이트는 유지합니다. `--fake-only-contract`는 기존52개를 재실행하지 않는17개 정책/모의 시험입니다. 실제 Windows7개와 구분하며 이번 NativeAOT 발행은 신뢰 근거 부재로0회입니다.
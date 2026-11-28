# Discord V2 재개 기록 · 2026-11-28

- [11월 21일 마감](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-7060405453)과 백업 인계를 대조했습니다. 시작 worktree clean, 기준 HEAD 일치. 기존 75/14 PASS를 재실행하지 않았습니다.
- Supervisor는 보호 Code에서 열어 둔 이미지 HANDLE과 고정 인증서·공개키 핀으로 두 이미지의 WinVerifyTrust 결과를 확인합니다. 검증된 실제 서명자 공개키로 Code의 FakeOnlyProduct.v1.json 및 FakeOnlyProduct.v1.sig를 검증하고, 기존 승인·소비 및 Probe 재개 경계에서도 제품 승인 만료를 확인합니다. 파일명 두 개를 이번 변경에서 정했습니다. 검증기는 기존 Staging WinTrust 구현을 Supervisor에 맞게 옮겼습니다.
- 관리형 빌드 경고/오류 0. 격리 관리형 DLL을 대상으로 신규 무서명 거부 1 PASS. 운영 서명 성공, 서명된 실행 계약 조합, NativeAOT 및 실제 Worker 토큰은 미검증입니다.
- 기존 배포 도구·Supervisor의 11월 1일 Deadline을 유지합니다. 재연결은 기존 승인 재사용 없이 서명·보호 원장 기반 배포 승인, 제품 계약(최대 5분), 기존 등록/시작 승인과 계정 만료를 별도 권한과 기간으로 다뤄야 합니다. 배포는 승인·요청·계획 기간의 교집합을 매 변경 직전 확인하고, 실행은 제품 승인까지 교집합에 포함합니다. 승인 구조를 검토한 뒤 기존 절대 cutoff를 그 프로필의 검증된 기간으로 대체하고 만료 경계 및 cleanup 예외를 표적 검증해야 합니다.

## 묶음 검토 요청

1. **Worker 직접 배치 권리**. 보호 WorkerBinding.v1.json의 원본 SHA·HANDLE·보안 descriptor를 기존 로컬 영수증과 대조하고 같은 비활성 로컬 SID를 확인합니다. 적용 정책에서 직접 허용·직접/그룹 거부, 로컬/관리자 그룹, 기존 Task principal, 보호 자산과의 권한 분리를 확인한 뒤 LsaAddAccountRights로 해당 SID의 SeBatchLogonRight 한 항목만 부여하는 범위입니다. 같은 SID로 직접 권리를 다시 읽고 추가 권리 1개를 확인합니다. 별도 실제 토큰 승인 전에는 유효 권한이나 배치 로그온 성공을 UNKNOWN으로 기록합니다.
2. **격리 NativeAOT 및 서명**. 기존 일반 Windows PowerShell 7 절차, SDK 10.0.403 및 기존 검증한 offline feed/runtime 10.0.10을 사용합니다. 제품 프로필 구현·비공개 binding 원본 대조 후 Supervisor 후보 1회 발행, AMD64/CLR 없음 확인, 승인된 인증서로 후보 서명 후 최종 SHA를 계산합니다. Probe는 기존 이미지의 서명과 SHA가 제품 계약을 만족할 때 재사용합니다. [직전 발행 근거](Discord_V2_Supervisor_FixedBuild_20261031.md)
3. **보호 제품 배치**. 서명 후 해시에서 배포 매니페스트를 생성하고 배포 도구의 기간 계약을 먼저 연결합니다. 독립 배포 승인과 보호 원장 소비를 사용하고, 두 경로의 보호 descriptor 및 Worker 유효 접근(코드 RX, 증거 제한 생성, Control/운영 쓰기 거부)을 별도 검증합니다. 부정 권한이 UNKNOWN이면 배치를 HOLD합니다.
4. **비활성 Task 등록**. 배치와 권한 확인이 통과하고 별도의 등록 승인이 유효할 때 기존 binding의 이름·SID로 disabled, 무트리거, LeastPrivilege, 90초 Task 한 건을 등록하고 readback합니다. 계정 활성화, Task 시작, 단발 실행은 이번 묶음 범위 밖의 별도 승인입니다.

현행 제품의 만료 연결 미완료 및 보호 바인딩 로컬 값 미대조로 관리자 변경용 확정 명령은 HOLD입니다. 승인 유효기간은 해당 단계 준비 완료 후 발급합니다. 기존 Worker와 보호 Staging/배포 도구는 재사용하며 운영 변경 0건입니다.

**NO_GO / MANUAL / EXECUTION_HOLD**

소스 커밋 `b6b5c466b1181c679a77409d21b7a8a2ce483b56`. LOCAL_COMMITTED / REMOTE_NOT_PUSHED.
[Issue #66 재개 보고](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-7231970616)

## 11월 28일 통합 후속

- Supervisor 한 경로에서 서명된 UTC 최대 5분 실행 창으로 고정 날짜를 대체했습니다. V2는 제품 이미지와 기존 보호 TrialContract 및 시작 승인 각각의 SHA를 요구합니다. 이름은 FakeOnlyExecution.v2.json 및 FakeOnlyExecution.v2.sig입니다. 기존 시작 스코프·영수증·소비·계정 검사 유지. 관리형 합성 신규 7 PASS, Supervisor와 DeploymentAdmin 소스의 관리형 컴파일 경고·오류 0. 기존 75/14/1 미재시험.
- DeploymentAdmin은 기존 고정 이미지/크기, 보호 manifest 허용 목록, 제품 검증 후 운영 HOLD가 남았습니다. Task 관리자 경계도 기존 cutoff와 활성 계정을 요구합니다. Probe는 그대로 호출되며 실제 서명 및 시작 인자/ABI와 보호 이미지 조합 런타임은 미검증입니다. 전체 호환 PASS 조건 미충족으로 NativeAOT·후보·SHA 없음.
- WorkerBinding은 일반 세션 ACCESS_DENIED. 로컬 관리자용 Review-WorkerBatchRight.ps1 준비. 이전 생성 영수증 SHA를 현지 입력하고 보호 어댑터가 동일 HANDLE로 다시 읽습니다. 기존 SID/비활성/정책 대조에 실패하면 중단합니다. 배치 거부 정책 항목이 있으면 보수적으로 HOLD합니다. 기본 모드는 조회, 별도 승인 뒤에만 -Grant. PowerShell 구문 및 임베디드 LSA 컴파일만 검증했고 실제 실행 0, 실제 토큰은 UNKNOWN입니다.
- 다음 구현 순서는 DeploymentAdmin 서명된 배포 승인·보호 원장·이미지 신뢰 경계와 고정 날짜 교체, 등록 승인과 비활성 Worker 상태의 별도 계약, 이후 동일 binding으로 오프라인 빌드 및 서명 전후 SHA, 보호 staging 복사, Code RX/Evidence 제한 생성 및 Control 쓰기 거부, 별도 비활성·무트리거·LeastPrivilege·90초 Task 등록 readback입니다. 실행 V2는 배포 승인과 별개로 실행 직전에 준비합니다. 기존 PowerShell 7 오프라인 입력 기준은 위 직전 발행 근거를 사용합니다.
- [Issue #66 통합 후속 보고](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-7232068574). 소스 635442e80838414a96345cd1abfed8f7c0968652. LOCAL_COMMITTED / REMOTE_NOT_PUSHED. NO_GO / MANUAL / EXECUTION_HOLD.

## Worker 조건부 승인 및 관리자 경계 후속

- 승인된 새 로컬 기준과 보호 바인딩의 동일 HANDLE 영수증·SHA, 기존 비활성 Worker SID 및 정책을 대조하는 실행본을 준비했습니다. PowerShell 구문과 LSA 어댑터 컴파일 통과. 실제 관리자 실행 결과 대기. 11월 21일 원본 영수증 미확인 유지.
- Task 시작 관리자는 동일 서명 V2를 진입, 승인 소비, Enable/Run 직전에 검사하며 다른 승인 바이트를 거부합니다. 등록/정리 cutoff 유지. DeploymentAdmin은 운영 HOLD 앞에 서명된 요청과 제품 승인 기간 교집합을 검사합니다. 기존 배포 cutoff 및 보호 매니페스트/이미지 제한 유지. 신규 격리 6 PASS, 기존 시험 재실행 없음.
- Probe의 무인자·고정 경로·Job 조건이 Supervisor 호출과 정적으로 일치하고 NativeInterop/TokenPolicy 두 공유 입력은 기존 Probe 빌드 입력과 SHA 일치합니다. 실제 서명, 제품 전용 binding, 런타임 호환은 미검증입니다. 배포 운영 경계와 별도 등록 계약 완료 후 격리 NativeAOT를 진행할 수 있습니다.
- [Issue #66 보고](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-7232411940). 소스 8f6e199e45c2b37bdb97eb0eaa0b0ad8d419b0ab. LOCAL_COMMITTED / REMOTE_NOT_PUSHED. NO_GO / MANUAL / EXECUTION_HOLD.

## 제품 통합 빌드 선행 조건

- DeploymentAdmin의 --review는 서명 요청에 결합된 제품 계약, 엄격한 매니페스트 5개 파일명, Trial 해시, 승인/제품/요청/계획 만료, 원장·관리자·도구 신원, 두 후보의 길이·SHA·AMD64 Native PE·보호 HANDLE Authenticode를 확인합니다. 기존 고정 이미지 해시·크기와 날짜를 사용하지 않는 검토 경로입니다. --apply는 명시 HOLD이며 보호 설치와 원장 소비는 연결 전입니다.
- Task 관리자는 등록·시작·정리 각각의 승인 SHA를 서명 V2에 결합합니다. 보호 Trial과 개별 5분 이하 동작 승인이 모두 살아 있어야 하므로 작업 사이에 Trial 해시를 바꿀 필요가 없고, 만료된 입력은 새 서명만으로 되살릴 수 없습니다. 신규 등록은 비활성 Worker와 확인된 배치 권리, 기존 비활성/무트리거/LeastPrivilege/90초 조건을 요구합니다. 실제 Scheduler의 Password 등록 동작은 미확인입니다.
- 신규 격리 10 PASS. 관리형 컴파일 경고·오류 0. 기존 75/14/1/7/6 시험 재실행 0. Probe 무인자 ABI 계약과 공유 TokenPolicy/NativeInterop는 유지됩니다. 운영 이미지 서명 및 실제 동작 미시험.
- 실제 NativeBinding.cs 제작 전 관리자 로컬 기준의 보호 HANDLE 영수증·SHA와 비활성 전용 계정 SID를 재대조하고 Worker/TaskPath/CodeRoot/EvidenceRoot 네 값을 같은 비공개 입력으로 생성해야 합니다. Supervisor·Probe·DeploymentAdmin의 출처와 전체 빌드 입력 해시를 로컬에 기록합니다. 현재 세션에서는 보호 바인딩과 로컬 영수증에 접근하지 못했습니다. 테스트 바인딩을 후보 발행에 사용하지 않습니다.
- NativeAOT는 위 기존 SDK 10.0.403·runtime 10.0.10·offline feed와 일반 Windows PowerShell 7 방법을 사용합니다. 별도 격리 출력에 3개 프로젝트 각각 발행, AMD64/CLR 부재 확인, 후보별 서명 전 SHA를 기록한 뒤 별도 승인으로 보호 staging에 새 파일 복사·서명·동일 HANDLE readback과 서명 후 SHA 기록 순서입니다. 서명 후 해시로 보호 manifest를 작성합니다. 실제 배포 경계에는 최종 만료 재검사·원장 단발 소비·CREATE_NEW 및 Code/Evidence 권한 readback이 필요합니다. 작업별 시한부 서명은 보호 Code에 승인별로 새 파일만 놓는 별도 경계가 필요하며, 제품 배포 시 실행 승인과 함께 복사하지 않습니다.
- Worker는 DIRECT_LOCAL_LSA_CONFIRMED / MERGED_POLICY_MISMATCH / EFFECTIVE_BATCH_UNKNOWN. 11월 21일 원본 미확인 유지. 현재 발행 없음. [Issue #66](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-7232555407). 소스 afb371d7e3210f3952cdcda0463c0082c7422ad2. LOCAL_COMMITTED / REMOTE_NOT_PUSHED. NO_GO / MANUAL / EXECUTION_HOLD.

## 보호 적용과 동작 승인 설치

- DeploymentAdmin의 실제 진입점은 기존 적용 컨트롤러를 호출합니다. 서명 요청·제품 승인은 보호된 핸들에서 한 번 검증하며, 모든 쓰기 단계에서 기간·계획·계정/도구 신원·원장을 다시 검사합니다. 현대 서명 계약의 만료를 사용하고, 레거시 경로는 기존 고정 날짜를 유지합니다. 실제 운영 실행은 하지 않았습니다.
- 동작별 --install-operation은 보호 Control/Approvals/PendingOperations 원본의 기존 operation JSON·서명 V2 JSON·SIG만 대상으로 합니다. 현재 보호 Trial과 두 이미지의 SHA 및 Authenticode, 동작·만료, 단발 원장, CREATE_NEW/DACL/경로·SHA readback을 요구합니다. 부분 실패는 자동 재시도/정리 없이 HOLD합니다. Windows 실기록 및 실제 서명 미시험.
- 격리 신규 6 PASS. 실제 제품 소스 관리형 컴파일 경고·오류 0. 합성 테스트에서만 미사용 fixture 필드 경고. 새 로컬 관리자 읽기 명령 Read-ProductNativeBinding.ps1과 후보 확인 명령 Read-ProductCandidateEvidence.ps1 구문 PASS. 실제 보호 바인딩 값을 격리 빌드에 사용할 수 있도록 로컬 비공개 입력으로 확정하는 절차는 관리자 확인 후 진행합니다. 승인된 이전 PowerShell 7·SDK 10.0.403·10.0.10 오프라인 feed 사용, Supervisor/Probe/DeploymentAdmin 순후보 발행, Task 기능은 Supervisor에 포함합니다. 별도 관리자 바이너리는 없습니다.
- 사용자 관리자 실행 결과 대기이며 NativeAOT·서명·운영 배포·동작 파일 설치 0. Worker 직접 LSA 권리 상태/병합 정책 불일치/토큰 미확인 유지. [Issue #66](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-7232643530). 소스 579cbe43c799d336adc0f164187dfb8240fea90e. LOCAL_COMMITTED / REMOTE_NOT_PUSHED. NO_GO / MANUAL / EXECUTION_HOLD.

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

## 제품 NativeAOT 후보 발행

- 대표 관리자 제품 바인딩 대조 통과. 기존 비활성 Worker와 실제 보호 바인딩을 비공개 단일 입력으로 사용했습니다. 11월 21일 원본 영수증 미확인 유지.
- PowerShell 7 일반 Windows 세션, SDK 10.0.403, 기존 10.0.10 오프라인 feed 사용. 샌드박스 Supervisor MSB4216/MSB4027은 일반 세션에서 해결했습니다. DeploymentAdmin의 오프라인 apphost NU1101은 해당 발행에만 UseAppHost=false를 적용해 해결했습니다. 소스/프로젝트 변경 없음.
- 총 세 최종 발행 exit 0, 경고 0, AMD64 PE32+·CLR 없음, 격리 빌드 입력 33개 SHA 재대조 일치. 후보만 발행했으며 어떤 후보도 실행하지 않았습니다.
- 로컬 증거 Temp/DV2P20261128/CandidateEvidence.json 및 artifacts. 아래 SHA-256은 서명 전입니다.
- Supervisor.exe · 7855104 bytes · f51c4782093423eb1d9d049e048cc7f4da22747296f413feb651604a369a430b
- Probe.exe · 1892352 bytes · 1b99d49a3426e9ffa199359e9b4fd9a96011106c5e99cc0a30ff8d403cabc70b
- Limitless.DeploymentAdmin.exe · 3685376 bytes · efac1f951f57db45571670496c305f0fcbf8dda1b6af064212fb3ec6bd0266ac
- 다음 승인 범위는 격리 후보의 보호 복사와 코드 서명, 서명 직후 동일 파일의 인증서 핀·SHA-256 및 출처 재검증, 서명 후 SHA를 사용한 배포 승인 서류 준비입니다. 운영 배치 및 도구 교체, Task 등록·실행·AI는 별도 승인입니다. Worker는 비활성 상태이며 권한 재부여하지 않았습니다.
- [Issue #66 발행 결과](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-7232698193). 소스 579cbe43c799d336adc0f164187dfb8240fea90e, 체크포인트 25bf04d0. LOCAL_COMMITTED / REMOTE_NOT_PUSHED. NO_GO / MANUAL / EXECUTION_HOLD.

## 제품 세 후보 보호 서명

- 승인 근거는 Issue #66 후보 발행 댓글과 대표의 세 후보 1회 보호 복사·코드서명 승인입니다. Issue 댓글의 3개 SHA/길이를 기계 추출해 로컬 CandidateEvidence.json 및 세 실제 파일과 대조했습니다. 기존 SignTool 파일 SHA와 공개 인증서 유효기간 확인, 기존 보호 workspace 어댑터의 후보별 마커 변형 컴파일과 PowerShell 구문 통과.
- 격리 로컬 SigningBatch/Invoke-ProductSigningBatch.ps1 준비. 기존 windows 상승 관리자와 인증서 핀·시간·EKU·개인키 존재를 확인합니다. ProgramData의 별도 제품 서명 작업 디렉터리에 최초 생성 보안 descriptor로 Tool·후보를 복사하며, 후보별 CREATE_NEW 마커 후 SignTool 1회, SignTool/Authenticode/Thumbprint/DER/SPKI/기간/서명 후 SHA 검사와 보호 통합 Manifest를 기록합니다. 실패 파일은 자동 재시도하지 않습니다.
- 작업 환경은 비상승 상태여서 실제 보호 복사/서명/검증은 대표 관리자 PowerShell 7 실행 결과 대기입니다. 서명 작업 위치, 서명 후 Manifest, 제품/배포/Trial·위험 근거 연결과 임시 승인 발급 요건 및 검토·적용 명령은 격리 SigningBatch/ProtectedDeploymentPrerequisites.md에 준비했습니다. 배포·도구 교체와 Task, Worker 활성화 및 AI는 별도 승인입니다.
- 후보 소스 579cbe43, 발행 기록 8b3ddde1. 기존 빌드/시험 반복 0. Worker 비활성, 11월 21일 원본 영수증 미확인, 실제 배치 토큰 미시험. LOCAL_COMMITTED / REMOTE_NOT_PUSHED. NO_GO / MANUAL / EXECUTION_HOLD.

## 제품 보호 서명 결과

- 대표 관리자 PowerShell 7 출력에서 보호 복사·후보별 1회 코드서명·SignTool 검증 및 PRODUCT_SIGNING_BATCH_VERIFIED를 확인했습니다. 배치 성공 상태는 Authenticode·인증서 지문·DER·SPKI·EKU·유효기간·서명 후 SHA와 보호 manifest readback 통과를 포함합니다. 타임스탬프 없음. 일반 세션에서는 보호 manifest를 독립적으로 읽지 못했습니다.
- 보호 매니페스트 C:/ProgramData/LimitlessDiscordV2ProductSigning20261128/ProductSigningManifest.json. 다음 값은 대표 관리자 실행 JSON의 서명 후 SHA-256입니다.
- Supervisor.exe · 5d906ac10caa5dd6c102091090ac623377d8bebe2775383d715749a87dcc4b7b
- Probe.exe · 333d6840bd25f251019c3fca0b88f3ed7903910ea768f6576b0214ce789a7a5e
- Limitless.DeploymentAdmin.exe · 78e0cf1e84e11ecca987d0fceb3409e58f891f00f809e680b39c1b9b69d9c985
- 다음 별도 승인 범위는 서명된 배포 도구 보호 반영, 서명 후 SHA와 보호 manifest에 결합한 제품·배포 승인의 발급, 검토(읽기 전용) 후 실제 적용입니다. 실제 승인·도구 교체·배포·Task·AI 실행 없음.
- 후보 소스 579cbe43, 서명 준비 662573e9. Worker 비활성, 11월 21일 원본 영수증 미확인, 실제 배치 토큰 미검증. NO_GO / MANUAL / EXECUTION_HOLD.

## 제품 보호 배포 통합 준비

- Issue #66 서명 결과 JSON에서 후보 3개 SHA를 자동 추출했습니다. 관리자 조회 전용 실행본은 보호 기준 WorkerBinding 및 보호 서명 매니페스트와 Issue 해시, 후보 파일·Authenticode·인증서 핀을 대조하고 Tool/Code/Evidence 존재 여부 및 Control/Approvals/DeploymentLedger 관리자 전용 DACL을 확인합니다. 현재 일반 세션의 도구 미발견은 권한 있는 확인을 대신하지 않습니다.
- 별도 도구 반영 모드는 기본값에서 꺼져 있습니다. 대상 부재와 명시 승인 후 기존 보호 어댑터를 사용해 CREATE_NEW, 같은 HANDLE 경로·파일 ID/SD·전체 SHA와 flush를 검증합니다. 기존 항목은 HOLD, 이동·교체·덮어쓰기 없음. 어댑터 컴파일 및 PowerShell 구문 검사 통과, 관리자 실행 대기.
- 격리 DeploymentPrep의 ProtectedDeploymentRunbook.md에 ① 신규 도구 반영 ② 제품/배포 승인 발급·RSA-PSS 분리 서명 ③ 무변경 review와 별도 승인 apply의 경계, ④ Task 비활성 등록 명령을 기록했습니다. Code RX/Evidence 제한 생성/관리자 전용 Control ACL과 서명 이후 같은 SHA를 확인하는 조회 전용 readback 스크립트도 준비했습니다.
- 보호 도구 반영, 시한부 승인 발급, 예고한 제품 배포, Task 등록 모두 0. 기존 시험·빌드·서명 반복 0, Worker 비활성 및 직접 권리/병합 불일치/실제 토큰 미검증, 11월 21일 원본 영수증 미확인 유지. 소스 579cbe43. NO_GO / MANUAL / EXECUTION_HOLD.

## 독립 배포 도구 보호 반영 결과

- 대표의 한 파일 승인 후 관리자 PowerShell 7에서 SIGNED_DEPLOYMENT_TOOL_MANIFEST_SHA_AND_CERTIFICATE_MATCH, TOOL_ABSENT;CODE_ABSENT;EVIDENCE_ABSENT 및 DEPLOYMENT_TOOL_CREATED_HANDLE_VERIFIED_NO_EXECUTION을 확인했습니다.
- C:/ProgramData/Limitless/DiscordV2/Control/DeploymentTool/Limitless.DeploymentAdmin.exe 한 파일 CREATE_NEW, 동일 HANDLE 경로·파일 ID·소유자·DACL 및 flush 후 SHA-256 readback. 기본값 보호 관리자 전용 Control/Approvals/DeploymentLedger descriptor 일치. 관리자 실행 출력 근거이며 정상 파일 교체 또는 도구 실행은 없었습니다.
- 다음 승인 발급 전 실제 신뢰 근거·위험 검토·Trial 및 보호 배포 매니페스트를 확정합니다. 격리 DeploymentPrep/ProtectedDeploymentRunbook.md 및 Invoke-ProductDeploymentReviewed.ps1에 승인 입력과 review/apply 명령 준비. 시한부 서명 발급, 실제 제품 적용, Task 등록은 각각 별도 승인입니다. Worker 비활성, 실제 토큰 및 11월 21일 원본 영수증 미확인. NO_GO / MANUAL / EXECUTION_HOLD.

## 제품 계약 발급 및 실행 준비

- 격리 DeploymentPrep/Issue-ProductDeploymentApprovals.ps1 준비. 기본 실행은 관리자 읽기 전용 readiness, 별도 발급 승인 모드만 보호 CREATE_NEW로 원시 JSON·RSA-PSS 분리 서명 등을 씁니다. 기존 보호 WorkerBinding 및 서명 manifest·Issue 해시·실제 설치 도구 SHA를 확인합니다. 해시 수기 입력 없음.
- Trial은 기존 승인 대상 ID·실제 Worker/Task·서명 Supervisor/Probe를 연결하고 FAKE_ONLY·NoAi·NoIpc·90초·각 이미지 1개를 고정합니다. 자격 증명 미공유, 대상 SID의 미신뢰 코드 및 다른 발사 경로 미관측, 관리자 잔여 위험 수용은 대표의 검토 근거 입력과 별도 승인 전까지 미확정입니다. 실제 Worker 토큰과 전역 SID 부재는 미확인.
- 배포·제품 승인은 검토 승인 확인 후 5분, 보호 Trial/신뢰 근거는 15분입니다. 현재 Trial은 보호 Code에 고정되므로 15분 뒤 별도 등록이 필요하다면 승인된 갱신/정리 경계 없이는 진행하지 않아야 합니다. 임의 연장/덮어쓰기 없음.
- review는 같은 세션에서 요청 SHA를 자동 캡처하며 apply는 별도 승인과 그 해시를 요구합니다. post-apply 읽기 검증, 별도 Task 등록 및 sidecar 명령을 ProtectedDeploymentRunbook.md에 정리. 신규 보호 어댑터 컴파일·PS 구문 통과, 합성 문서 결합 및 PSS 2종 검증 통과. 기존 시험 반복 0. 실제 발급·review·apply·Task·AI 0. Worker 비활성, 11월 21일 원본 영수증 미확인. NO_GO / MANUAL / EXECUTION_HOLD.

## 제품 적용 부분 실패와 경로 비교 수정

- 관리자 승인 발급 및 review 무변경 PASS 뒤 apply 1회 HOLD. 승인 원장 소비 12:42:27 KST, 승인 만료 12:43:50 KST. Code/Evidence 존재, 순서상 첫 Supervisor 파일 0바이트, 생성 12:42:27.074 KST 및 마지막 쓰기 시각 동일, 보호 DACL/소유자/그룹 읽기 확인. Probe 및 나머지 계약 파일 세 개 부재.
- 서명된 도구의 CREATE_NEW 직후 경로 검사에서 잘못된 Windows 경로 접두어를 확인했습니다. 해당 검사는 첫 쓰기 전에 호출되며 0바이트 현상과 일치합니다. 기존 정상 HANDLE 경로 검증을 신규 복사에도 공유하도록 수정했습니다. 실제 격리 exclusive CREATE_NEW 핸들의 일치 경로 통과 및 다른 경로 거부 2 PASS. 소스 ccdb33ea7a1deabf9da4d757360eecff8ba428bd.
- 원장/승인 기록/부분 파일과 폴더 보존, 수정 도구 빌드·서명·교체·정리·재실행·Task·AI 0. 만료와 원장 소비 상태이므로 기존 요청 재사용 불가. 수정 도구 발행과 보호 상태 복구는 별도 승인 필요. Unity 확인 대상 없음. NO_GO / MANUAL / EXECUTION_HOLD.

## 보호 배포 수정 도구 발행과 복구 준비

- ccdb33ea7a1deabf9da4d757360eecff8ba428bd의 DeploymentAdmin만 일반 Windows PowerShell 7·SDK 10.0.403·승인된 10.0.10 오프라인 입력으로 NativeAOT 발행했습니다. exit 0, 경고·오류 0, AMD64 PE32+ CLR 없음, 비공개 포함 입력 해시 30개 동일. 기존 Supervisor/Probe 재빌드·서명 및 운영 후보 접근 없음.
- 실제 Windows CREATE_NEW 독점 HANDLE에서 프로덕션 복사 루프를 발췌해 실제 부분 파일의 원본 크기와 같은 합성 7,856,960바이트 파일을 썼습니다. 쓰기/flush 후 동일 HANDLE SHA, 다시 연 파일 SHA, 기존 파일 CREATE_NEW 거부 3 PASS. 운영 경로·원장·ACL·관리자 back end 및 실승인은 사용하지 않았습니다. 기존 시험 반복 0.
- 후보는 Temp/DV2Fix20261128/artifacts 및 CandidateEvidence.json에 있습니다. 서명 준비 스크립트와 보호 어댑터 컴파일/구문, 복구용 발급기 및 동일 review/apply SHA 래퍼 구문 통과. 새 최종 도구 SHA는 보호 서명 Manifest에서 자동 연결합니다.
- 복구 절차 Temp/DV2Fix20261128/ProtectedDeploymentRecovery.md. A 수정 후보 1회 서명, B 기존 도구 보호 보관 및 교체, C 부분 Code/Evidence 및 해당 만료 승인 파일의 보호 보관, D 새 승인 및 배포를 독립 승인으로 둡니다. 원장은 보존하고 고정 경로를 안전하게 비우는 방식입니다. B/C 운영 명령은 아직 실행하지 않았으며 서명 후 식별값/상태 확인이 선행됩니다.
- 실제 서명·도구 교체·부분 상태 변경·발급·배포·Task·AI 0. Worker 비활성. NO_GO / MANUAL / EXECUTION_HOLD.
- 수정 DeploymentAdmin 서명 전 SHA-256 b31a88c607e8c9ef59a033b95df4e913132d85e5e6ce6706e80cf2731dc307b3 · 3684864 bytes.

## 수정 도구 A 승인 및 보호 서명 결과

- 대표 승인 A 이후 관리자 PowerShell 7에서 수정 DeploymentAdmin 한 파일의 SignTool sign/verify 및 FIXED_DEPLOYMENT_TOOL_SIGNED_VERIFIED를 확인했습니다. 기존 승인된 인증서 핀/DER/SPKI/EKU/유효기간과 Authenticode 판정 및 보호 manifest readback은 스크립트 PASS에 포함됩니다. 타임스탬프 없음. 이 기록의 근거는 대표 관리자 출력입니다.
- 서명 후 SHA-256 f07e9b8f3c45faa123fba6482bc239fc497a287046a72cad3befcdb95a68d4b9. Manifest C:/ProgramData/LimitlessDiscordV2DeploymentAdminFixSigning20261128/ProductSigningManifest.json.
- B 기존 도구 보관/교체, C 부분 상태/만료 승인 보호 보관, D 발급 및 apply는 별도 승인 대기. 운영 파일/원장/부분 파일 변경 및 재실행 0. Worker 비활성, Task·AI 실행 0. NO_GO / MANUAL / EXECUTION_HOLD.

## 수정 DeploymentAdmin 보호 교체 B 완료

- 대표 승인 B 후 관리자 출력 OLD_DEPLOYMENT_TOOL_ARCHIVED_AND_FIXED_TOOL_CREATED_HANDLE_VERIFIED_NO_EXECUTION 확인. 기존 도구를 동일 NTFS의 Control/DeploymentToolArchive20261128 보호 경로에 이동, 동일 HANDLE 경로·ID·SD·SHA 일치. 변경 대상 고정 경로에 수정 서명 도구만 CREATE_NEW, 동일 HANDLE 검증 및 flush 후 SHA 일치.
- 수정 도구 소스 ccdb33ea 및 A 서명 기록 28400eeb. 근거는 관리자 실행 출력. 운영 도구 시작, 부분 Code/Evidence 및 소비 원장 변경 0. C 부분 상태 보호 복구와 D 새 발급/배포 별도 승인 대기. 수정 전후 합성 Rename 검증은 실제 파일에 대한 동일 HANDLE 새 경로, 덮어쓰기 차단, 데이터 보존을 확인했습니다. NO_GO / MANUAL / EXECUTION_HOLD.

## C 부분 배포 증거 보호 보관 완료

- 대표 승인 C 후 관리자 출력 PARTIAL_CODE_EVIDENCE_AND_EXPIRED_APPROVALS_ARCHIVED_LEDGER_UNCHANGED 확인. 최초 실패 요청의 정확한 서명·해시 연결, 비활성 Worker, 보호 DACL/파일 ID 및 원장 본문을 대조한 뒤 해당 0바이트 Supervisor가 남은 Code, 빈 Evidence, 만료 승인 파일 10개만 동일 NTFS의 Control/FailedProductDeploymentArchive20261128 보관소로 이동했습니다.
- 기존 경로 부재 및 보관물 동일 HANDLE ID/SD/SHA, 소비 원장 SHA 불변 검증이 성공 상태에 포함됩니다. 관리자 출력 근거. 실제 이동 전 격리 디렉터리 HANDLE 이름 변경 및 0바이트 자식 보존 검증 PASS. 파일 삭제/덮어쓰기 없음.
- D의 첫 단계는 수정 도구 보호 서명 매니페스트를 사용하는 Issue-RecoveryProductDeploymentApprovals.ps1 기본 읽기 전용 사전 확인입니다. 별도 승인 전 발급 및 apply 0, Worker 비활성, Task·AI 0. NO_GO / MANUAL / EXECUTION_HOLD.

## 수정 도구 제품 보호 재배포 완료

- 대표 승인 D-1 후 새 제품/배포 승인 발급 및 SIGNED_PRODUCT_INPUTS_REVIEWED_NO_CHANGES 확인. 발급 유효기간 13:40:31 KST, Trial 만료 13:50:31 KST. 새 요청 검토 SHA로 별도 D-2 승인, 수정 도구 apply 1회 DEPLOYMENT_COMPLETED_NO_EXECUTION.
- 대표 관리자 독립 읽기 POST_DEPLOYMENT_FILES_SIGNATURES_DACL_AND_BOTH_LEDGER_RECORDS_VERIFIED_TOKEN_NOT_TESTED 확인. 제품 파일 5개 길이/SHA, Supervisor/Probe Authenticode 및 Code/Evidence/Control/Approvals/Ledger DACL 검증. 새 소비 기록은 요청과 일치, 이전 실패 소비 기록은 보호 보관 증거 SHA와 동일. 기존 실패 증거 보존.
- 운영 제품 경로 보호 배치 완료. Worker 비활성, 실제 배치 토큰 미시험, 직접 LSA 권리/병합 정책 불일치 및 11월 21일 원본 영수증 미확인 유지. Task 등록, Worker 활성화, 제품 실행·AI 없음. 현재 Trial 만료 뒤 등록은 별도 보호 갱신/정리 계약과 승인 전 HOLD. Unity 확인 대상 없음. 도구 소스 ccdb33ea. NO_GO / MANUAL / EXECUTION_HOLD.

## 전용 Task 및 FAKE_ONLY 시험 준비

- Windows UTC 04:47:52 기준 기록상 Trial은 04:50:31 UTC(13:50:31 KST) 만료입니다. 실제 Code의 SHA/신뢰 시각/전용 Task/Worker/Evidence를 출력하는 관리자 읽기 전용 Inspect 준비. 일반 세션은 보호 Trial에 접근할 수 없으므로 보호 파일 조회 결과 대기.
- TaskPrep의 TaskAndFakeOnlyRunbook.md 및 관리자 명령을 준비했습니다. 비활성 전용 등록, 실제 로컬 배치 토큰만 확인하고 Worker 비활성 원복, 별도 승인으로 시작 sidecar 및 단발 FAKE_ONLY 시작/Cleanup, Supervisor 0·Probe 125·Job drain·Evidence·잔류 프로세스 및 종료 상태 검증 범위를 구분했습니다. PowerShell 구문과 보호 sidecar 작성 및 Windows 배치 토큰 어댑터 컴파일 PASS. 운영 실행/발급 0, 기존 PASS 미반복.
- 만료 시 보호 Code의 불변 Trial/신뢰 3파일과 기존 승인 증거만 보관하고, 동일 보호 이미지에 새 Trial 및 별도 서명 갱신 영수증을 결합해야 합니다. 현재 발급기는 배포 요청의 기존 SHA를 요구하므로 갱신 수용/승격 코드는 별도 구현 대상이며 운영 갱신 미실행입니다. 현재 보관된 실패 Evidence의 명시 ACE가 비활성 Worker에게 남아 있어, 실제 활성화 전에 보관본 유효 쓰기 권리를 확인해야 합니다.
- Task 등록/활성화/토큰 검증/실행/AI 0. Worker 비활성·직접 LSA 권리와 병합 정책 차이·실제 토큰 및 11월 21일 원본 영수증 미확인 유지. Unity 확인 대상 없음. NO_GO / MANUAL / EXECUTION_HOLD.

## 전용 Task 등록 전 관리자 조회

- 대표 관리자 Windows UTC 04:52:02(13:52:02 KST), Trial 및 신뢰 기간 만료 04:50:31 UTC(13:50:31 KST), TrialAndTrustLive=false. Worker 비활성, 전용 Task 없음, 등록/시작/instance/감독·완료 증거 모두 없음, 제품 경로 잔류 및 경로 미확인 프로세스 0.
- TaskPrep의 Worker 바인딩 조회 상대경로 오류 교정, 기존 DLL 및 조회 스크립트 SHA 핀 대조 후 관리자 재조회 성공. 실제 배치 토큰 미시험, Task 등록 및 실행 없음.
- 만료된 보호 Trial·신뢰 파일과 이전 원장을 보존합니다. 기존 제품/서명/배치 경로를 재사용하려면 서명된 갱신 영수증과 새 Trial SHA를 승인하는 보호 경계를 먼저 구현해야 합니다. 현재 등록 및 FAKE_ONLY 실행 HOLD. NO_GO / MANUAL / EXECUTION_HOLD.

## 보호 Trial 갱신 구현

- 별도 PowerShell/C# 보호 관리 경계 구현. 원래 서명 배포 요청·manifest 및 소비 원장을 읽어 기준을 확정하고, 이전/신규 Trial·신뢰·근거 SHA와 동일 Worker/Task/이미지를 RSA-PSS 영수증에 연결합니다. 승격 최대 5분, 신규 Trial/신뢰 최대 15분. 이전 살아 있는 Trial, 변조·다른 바인딩·만료·중복/부분 경로를 HOLD합니다.
- 이전 SHA별 CREATE_NEW 보호 위치, 기존 3개 계약의 동일 HANDLE 경로·ID·SD·SHA 보관, 새 3개 제한 경로 CREATE_NEW 및 전체 SHA, 마지막 승격 완료 영수증. 중간 실패는 증거를 보존하며 새 head를 인정하지 않습니다. 원래 EXE, 서명, 배포/실패 원장을 그대로 보존하는 별도 스크립트이며 이번 제품 빌드/서명/배포 없음.
- 합성 격리 6개 PASS (만료/살아 있는 이전 계약 거부, 변조/다른 head 재생 거부, 점유/부분 경로 불승격과 증거 보존, 정상 서명 승격, 이미지 및 원장 불변, 새 보호 입력 변조 거부). 보호 어댑터 Add-Type 및 연동 스크립트 구문 통과. 실제 운영 갱신은 미검증/미실행.
- 로컬 TrialRenewal/TrialRenewalAndRunbook.md에 T 보호 갱신, R 등록, P 배치 토큰 확인, F 단발 실행·원복·사후 판정을 별도 승인으로 준비했습니다. 등록/시작 sidecar 발급은 새 서명 head의 확인을 요구합니다. 기존 보관 Evidence에서 Worker/미신뢰 쓰기 ACE가 확인되면 활성화와 배치 토큰/시험을 HOLD합니다. 읽기 전용 관리자 확인 명령 별도. Worker 비활성, 실제 토큰 및 11월 21일 원본 미확인, Task·AI·API 실행 0. Unity 확인 사항 없음. NO_GO / MANUAL / EXECUTION_HOLD.
- 갱신 소스 4d2d4bb142f444b2eca20ace08fe9347111bdd34.

## 보관 Evidence Worker 쓰기 경계 처리

- 대표 관리자 읽기에서 Trial 갱신 사전 확인 PASS, 보관된 실패 Evidence의 기존 Worker 쓰기 ACE 확인. 별도 승인 후 ARCHIVED_EVIDENCE_ORIGINAL_DACL_PRESERVED_WORKER_ACE_REMOVED_HANDLE_VERIFIED 성공 상태를 확인했습니다.
- 보호 보관소 내 Evidence 한 폴더만 대상으로 원래 보안 설명자를 보호 CREATE_NEW로 보존하고, 같은 HANDLE에서 정확한 Worker ACE 한 항목 제거 및 보호 DACL/파일 ID 재검증. 관리자 출력 근거. 운영 Evidence, 배포 원장, Worker 계정은 수정하지 않았습니다.
- 다음은 실제 시험 진행자가 확보된 시점에 T 보호 Trial 갱신 별도 승인입니다. 갱신에서 기존 제품/원장 보존 및 운영 작업 미발생 유지. 실제 배치 토큰 미검증, Worker 비활성, 등록/실행/AI 0. Unity 확인 대상 없음. 갱신 소스 4d2d4bb1. NO_GO / MANUAL / EXECUTION_HOLD.

## 보호 Trial 갱신 T 완료

- 대표 별도 승인 T 후 관리자 출력 TRIAL_RENEWAL_PROMOTED_SIGNED_CHAIN_VERIFIED_NO_TASK_EXECUTION 확인. 이전 보호 Trial·신뢰·근거의 보관과 새 SHA 결합 RSA-PSS 갱신 영수증/완료 기록 및 기존 이미지 SHA 재대조 성공 상태입니다.
- 갱신 Trial 만료 2026-11-28 05:27:07 UTC(14:27:07 KST). 실제 운영 명령의 결과는 대표 관리자 출력에서 확인했습니다. 기존 제품·서명·배포 원장 및 실패 증거는 보존했습니다.
- 다음 R 등록 전용 서명 파일 발급 및 비활성 Task 등록 별도 승인. Worker 비활성, P 실제 배치 토큰·F 단발 실행·AI/API 미실행, 11월 21일 원본 영수증 미확인. Unity 확인 사항 없음. 갱신 소스 4d2d4bb1. NO_GO / MANUAL / EXECUTION_HOLD.

## R 등록 HOLD 및 암호 대기 소스 수정

- 대표 승인 R 후 sidecar 이름 조합의 PowerShell 우선순위 오류를 교정하고 등록·시작 이름 배열 각각 검증. 등록 전용 승인 발급 및 설치 PASS 뒤 Supervisor 등록 호출 HOLD. 관리자 두 차례 동일 상태 조회에서 Worker 비활성, Task 없음, 등록 의도 있음, 등록 완료 영수증 없음, HOLD 1개, 설치 승인 14:20:43 KST 만료, 당시 Trial/신뢰 유효, 잔류/경로 미확인 제품 프로세스 0. 실제 토큰 미시험.
- 운영 도구의 일반 실패 출력만으로 만료와 Scheduler 거부를 구분할 수 없습니다. 기존 소스의 무안내 암호 대기/입력 중 만료 미확인 결함을 수정했습니다. 입력 전에 안내하고 서명된 승인 만료 시 입력을 종료하며 이전 COM 직전 전체 검증은 유지합니다. 신규 격리 3 PASS (만료 직전/정확 만료/서명 기간 초과). 소스 0c432d51585959958299ce05079a4f440b33d613. 수정 바이너리 발행/서명/교체 없음.
- 기존 등록 의도 및 승인 소비는 보존합니다. 재시도·재발급·정리 0. 승인 기록 및 Code/PendingOperations 서명 입력을 보존하고, Task 부재와 결합한 별도 보호 복구가 선행됩니다. 제품 경계, 해시 또는 기존 원장 임의 변경 금지. Worker 활성화·P/F/AI/API 0. NO_GO / MANUAL / EXECUTION_HOLD.

## 등록 진단 Supervisor 수정 및 복구 준비

- 0c432d51 기반 등록 오류 UNKNOWN 유지. Supervisor의 암호 콘솔/빈 입력·취소·승인 만료, 단계별 Scheduler COM HRESULT, 등록 readback 진단을 구분했습니다. 임의 예외 메시지/입력/경로를 출력하지 않습니다. 신규 격리 4 PASS, 기존 성공 시험 반복 0. 소스 95da64f8ceaabd1b6884472ab444779ea56572f7.
- 변경 Supervisor 한 종만 일반 PowerShell 7 및 기존 SDK 10.0.403/10.0.10 로컬 오프라인 feed로 NativeAOT 발행했습니다. exit 0, 경고·오류 0, AMD64 PE32+ CLR 없음. 빌드 입력 25 해시 불변, Probe·DeploymentAdmin 발행/서명/운영 변경 없음.
- 격리 DV2R20261128에 Supervisor 단발 보호 서명 실행본과 실패 등록 범위 읽기 전용 명령 준비. 기존 등록 의도/HOLD, Code/PendingOperations 사본 및 단발 소비 원장을 승인 ID와 SHA에 결합해 복구 대상을 확인합니다. 보호 이동 작업 자체 및 운영 교체 실행은 보류. 사본·기록 보호 보관과 원래 소비 원장 불변 절차, 서명된 이미지 전환/갱신 계약 요구를 RegistrationRecoveryRunbook.md에 정리했습니다.
- 기존 Trial 갱신은 동일 Supervisor SHA를 요구합니다. 서명된 이전/신규 Supervisor 전환을 수용하는 코드와 운영 교체 실행본은 미완료이며 단순 파일 교체 금지. 암호 입력 준비 완료 뒤 발급, 별도 등록/배치 토큰/단발 FAKE_ONLY/비활성 원복 명령을 정리했습니다. 실제 서명·보호 이동·운영 교체·발급·등록·활성화·시험·AI 0. Worker 비활성, 실제 토큰 및 11월 21일 원본 영수증 미확인. Unity 확인 대상 없음. NO_GO / MANUAL / EXECUTION_HOLD.
- 서명 전 후보 1aaff5b1bc17a04268e76c95265d142de852f894dcdbf82dfe7db2c91d3dff2a / 7862784 bytes.

## 등록 진단 Supervisor A 보호 서명 PASS

- 대표 관리자 출력 FAILED_REGISTRATION_EVIDENCE_AND_EXPIRED_SIDECARS_BOUND_LEDGER_READ_ONLY로 대상 결합 사전 확인 PASS. 별도 A 승인 및 등록 진단 Supervisor 한 파일 단발 SignTool sign/verify, REGISTRATION_SUPERVISOR_SIGNED_VERIFIED 확인. 해당 스크립트의 Authenticode, 지문/DER/SPKI/EKU/유효기간 및 보호 manifest 확인 포함. 타임스탬프 없음. 근거는 대표 관리자 출력입니다.
- 서명 후 SHA-256 149043ab6da4f3891c60cc7230db77ec39eaad7291af6dea86135effeec620dc. 보호 서명 Manifest는 C:/ProgramData/LimitlessDiscordV2SupervisorRegistrationSigning20261128/ProductSigningManifest.json.
- 운영 Supervisor, Trial, 등록 실패 기록 및 원장 변경 없음. B 해당 실패 기록 보호 보관, C 제품 교체/서명 해시 전환, D/P/F 별도 승인 대기. Worker 비활성 및 실제 배치 토큰 미시험, Task/실행/AI/API 0. Unity 확인 사항 없음. 후보 소스 95da64f8. NO_GO / MANUAL / EXECUTION_HOLD.

## 실패 등록 B 보호 보관 및 Supervisor 전환 준비

- 대표 승인 B 후 FAILED_REGISTRATION_EVIDENCE_AND_SIDECARS_ARCHIVED_LEDGER_UNCHANGED 확인. 해당 등록 승인 Code/PendingOperations 각 3개와 Evidence 등록 의도·HOLD 2개만 보호 보관. 동일 HANDLE ID·DACL·SHA 및 기존 소비 원장 불변 확인. Worker/Task/제품·경로 상태는 변경하지 않았습니다.
- RSA-PSS Supervisor 전환 영수증과 보호 보관/승격 경계를 구현했습니다. 기존/신규 Supervisor SHA, 새 Trial·Trust·Basis, 보호 새 서명 Manifest와 등록 실패 보관 영수증을 결합합니다. 기존 Supervisor만 보호 보관, 해당 EXE와 3 계약만 CREATE_NEW, Probe·DeploymentAdmin·기존 제품 서명/원장 보존, 마지막 commit 없으면 새 상태 거부. 기존 등록/시작 발급기가 새 서명 head를 검증합니다.
- 신규 합성 격리 4 PASS (부분 전환 불승격, 정상 Supervisor와 Trial 결합, 보관 이미지/실패 증거 변조 거부, 같은 서명으로도 Probe 변경 금지). 관리 보호 어댑터 컴파일 및 PowerShell 구문 통과. 신규 제품 빌드·운영 교체·서명·발급 0.
- 격리 DV2R20261128/Migration/Invoke-SupervisorTrialMigration.ps1 기본 실행은 관리자 읽기 전용. 실제 C 교체 및 갱신은 별도 승인 및 관리자 readiness 결과 필요. 승인 직후 5분 쓰기 창, 새 Trial 15분. D/P/F 실제 작업 미실행, Worker 비활성 유지. Unity 확인 사항 없음. 소스 779acc40f50c961581ada6ba3f9b2b93b555df5a. NO_GO / MANUAL / EXECUTION_HOLD.

- 등록 화면에서 암호 입력마다 `-` 표시, Backspace 입력 시 표시 제거를 적용했습니다. 로컬 진단 래퍼는 표시 이벤트를 중계하고 로그에는 저장하지 않습니다. 격리 오프라인 Supervisor NativeAOT 발행 성공. 운영 콘솔 입력 동작 및 실제 등록 미검증, 서명·교체·승인 발급 미실행. 이전 등록 실패는 빈 입력 가능성이 있으나 세부 진단이 없어 UNKNOWN. NO_GO / MANUAL / EXECUTION_HOLD.

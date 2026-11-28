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

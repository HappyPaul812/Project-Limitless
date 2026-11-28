# Discord V2 등록 출력과 격리 COM 진단

운영 재등록 금지 상태입니다. 이 문서는 준비 절차입니다.

## 출력 보존

기존 래퍼는 Supervisor의 stdout/stderr를 콘솔에 전달하고 종료 코드는 일반 HOLD로 변환했습니다. 제품 진단을 파일에 쓰지 않았으므로 화면 밖에서는 복구할 수 없습니다. stderr가 당시 화면에 실제 표시됐는지는 UNKNOWN입니다.

수정 래퍼는 stdin을 콘솔에 남기고 stdout/stderr를 별도로 읽습니다. 알려진 암호 안내는 화면에 전달하고 로그에는 표식으로 기록합니다. 허용 목록에 일치하는 제품 상태 및 HRESULT, UTC, 종료 코드를 관리자와 SYSTEM만 접근하는 Control의 CREATE_NEW 파일에 씁니다. 기타 출력은 내용 없이 제외 표식으로 기록합니다. 기존 실패 파일과 원장을 수정하지 않습니다.

## 격리 검증 순서

별도 승인 후 운영 네트워크, 운영 계정/암호, WorkerBinding, 보호 경로 및 원장을 갖지 않는 일회용 Windows VM에서 수행합니다. 같은 Windows 빌드와 PowerShell x64를 사용합니다. 검증 대상은 COM 호출과 콘솔 전달이며 기존 제품 실행을 재현하지 않습니다. VM에 전용 비관리자 테스트 계정 및 고유한 테스트 Task 이름을 쓰고 스냅샷으로 각 조건을 분리합니다.

1. 입력을 콘솔에 남긴 상태에서 합성 프로그램으로 stdout 안내, stderr 고정 HRESULT, 종료 코드와 로그 ACL을 확인합니다. 미승인 문자열이 로그에 남지 않아야 합니다.
2. 스케줄러 Operational 로그를 VM에서만 활성화합니다. 동일 VM 전용 XML을 검증 모드(TASK_VALIDATE_ONLY)로 검사하고 잘못된 XML 대조군과 HRESULT를 비교합니다. 이 단계로 계정 조건을 판정하지 않습니다.
3. VM 전용 작업은 무트리거, 비활성, LeastPrivilege, PT90S 및 VM 전용 무해한 동작으로 구성합니다. 운영 COM과 동일하게 ITaskService.NewTask → ITaskDefinition.XmlText → ITaskFolder.RegisterTaskDefinition(TASK_CREATE | TASK_DONT_ADD_PRINCIPAL_ACE, TASK_LOGON_PASSWORD)를 사용합니다. 단계·HRESULT(성공 경고 포함)·작업 존재/비활성 readback만 기록합니다. 시작하지 않습니다.
4. 활성 VM 계정·정확한 암호·명시적 배치 허용을 기준으로 한 번씩 비교합니다. (a) 계정 비활성만 변경 (b) 암호만 불일치 (c) 배치 허용만 제거 또는 명시적 배치 거부 (별도 행) (d) XML만 변경. 각 행 시작 전 스냅샷을 복원하고 실제 계정 상태와 정책을 읽기 전용 확인합니다. 암호는 VM 메모리만 사용합니다. 배치 권한 자동 부여 여부를 등록 뒤 확인합니다.
5. 실패 단계 및 HRESULT를 VM 이벤트와 대조합니다. VM 결과는 동일 조건 재현 근거이며 과거 운영 실패를 소급 확정하는 근거는 아닙니다. 테스트 작업은 미실행 상태로 스냅샷 복원합니다.
6. 격리 결과에 따른 최소 수정: 콘솔 오류면 실행 호스트/입력 경계, XML 오류면 해당 정의, 계정·암호 오류면 해당 계정 검증 단계, 배치 정책 오류면 승인된 정책 경계만 검토합니다. 운영 재등록은 원인 확인, 새 Trial·단발 승인, 보호 보관 및 별도 승인 전까지 HOLD입니다.

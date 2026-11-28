# Discord V2 Task 등록과 단발 FAKE_ONLY 준비

현행 제품과 승인 원장·보호 경로는 재사용합니다. Worker 비활성, 실제 배치 토큰 미시험, 11월 21일 원본 영수증 미확인. 현재 관찰된 Trial 만료는 2026-11-28 04:50:31.0088781 UTC / 13:50:31 KST입니다. 날짜를 바꾸거나 오래된 sidecar를 재사용하지 않습니다.

## 읽기 전용

~~~powershell
& 'F:\study\codex\Project-Limitless\Temp\DV2Fix20261128\TaskPrep\Invoke-V2TaskPrepared.ps1' -Mode Inspect
~~~

Windows UTC/Trial 및 신뢰 자료의 기간, 기존 Worker 상태, 전용 Task 존재 및 설정, 실행 증거, Scheduler 결과, 제품 경로의 잔류 프로세스 또는 경로 조회 UNKNOWN을 확인합니다.

## 만료 후 보호 갱신 절차 · 별도 승인 필요

현재 모든 Task/감독 실행은 Code/TrialContract.json의 전체 SHA와 그 Trial에 결합된 최신 신뢰 근거를 확인합니다. 만료된 Trial에 새 operation 서명만 발급해 사용할 수 없습니다.

1. Windows UTC, 전용 Task/실행 인스턴스 부재 또는 정확한 비활성 상태, 관련 프로세스 0, Worker 비활성. 기존 제품/서명과 두 소비 원장, 보관 자료 및 보호 SD를 다시 확인합니다.
2. 아직 전용 Task가 없는 경우 승인된 별도 갱신 작업을 마련해, Code의 기존 Trial·신뢰 JSON·근거 3개 및 대응하는 만료 승인 입력을 기존 파일 ID·SD·SHA와 함께 보호 보관합니다. 제품 EXE·Code/Evidence 디렉터리 또는 소비 원장을 바꾸지 않습니다. Task가 존재하면 고정 인자의 이전 Trial SHA 문제 때문에 현재 절차는 HOLD입니다.
3. 신규 검토 시각·근거와 같은 Worker/Task·경로·이미지 SHA로 새 3개 입력을 보호 CREATE_NEW하고, 이전 배포와 새 입력 모두에 결합한 별도의 관리자 서명 갱신 영수증을 보호 원장에 기록해야 합니다. 기존 배포 승인 파일이나 소비 기록을 수정하지 않습니다.
4. 동작 승인 발급기는 현재 서명된 배포 요청의 Trial SHA를 확인하므로, 갱신 완료 전에는 새 SHA와 서명된 갱신 영수증을 확인하도록 경계를 추가해야 합니다. 보호 갱신 실행본 및 실제 변경은 이번 준비 범위에서 미구현이며 만료 상태는 HOLD입니다.

## 등록 R · 서명 발급과 등록 승인

Trial과 신뢰 시각이 모두 유효하고 전용 Task가 부재하며 Worker 비활성일 때만 실행합니다. 사용자 별도 R 승인으로 등록 전용 sidecar 3개를 발급합니다.

~~~powershell
& 'C:\Program Files\PowerShell\7\pwsh.exe' -NoProfile -File 'F:\study\codex\Project-Limitless\Temp\DV2Fix20261128\TaskPrep\Issue-V2TaskOperationSidecars.ps1' -Operation Register -IssueAfterExplicitApproval
~~~

~~~text
ISSUE-V2-OPERATION-SIDECARS
~~~

유효기간 내 별도 Task 등록 승인 후 아래를 실행합니다. 암호는 제품의 비표시 콘솔에만 입력합니다.

~~~powershell
& 'F:\study\codex\Project-Limitless\Temp\DV2Fix20261128\TaskPrep\Invoke-V2TaskPrepared.ps1' -Mode Register
~~~

~~~text
REGISTER-DISABLED-V2-TASK
~~~

등록 후 Inspect로 비활성/무트리거/LeastPrivilege/90초/정확 Worker SID와 액션/인스턴스 0/등록 영수증을 확인합니다. HRESULT·등록 readback 불명 시 재시도하지 않습니다.

## 배치 토큰 P · 기존 Worker 잠깐 활성화와 원복에 대한 별도 승인

Task 비활성/실행 0 상태에서만 아래를 실행합니다. Windows 배치 로그온 토큰의 기존 SID·배치 그룹·비관리자 여부를 확인합니다. 토큰을 닫고 승인된 계정만 비활성으로 돌려놓습니다. 실제 FAKE_ONLY 실행 없이 확인하며 그룹·권한 값이나 암호를 저장하지 않습니다. 실제 Token PASS 전에는 실행으로 넘어가지 않습니다.

~~~powershell
& 'C:\Program Files\PowerShell\7\pwsh.exe' -NoProfile -File 'F:\study\codex\Project-Limitless\Temp\DV2Fix20261128\TaskPrep\Invoke-V2WorkerBatchProbe.ps1' -ProbeAfterExplicitApproval
~~~

~~~text
PROBE-V2-BATCH-AND-DISABLE
~~~

## 실행 F · 시작 sidecar 승인, Worker 활성화·단발 실행·비활성 원복의 별도 승인

배치 토큰 PASS와 Task 등록 readback PASS, 활성화 준비 상태를 확인합니다. 보호 보관본 Evidence에는 원래 Worker ACE가 증거로 남아 있어, 활성화 전에 보관본에 대한 Worker 쓰기 차단 여부를 확인하는 별도 조건이 남습니다. 기존 복구 증거는 수정하지 않습니다. 시작용 sidecar의 별도 발급을 먼저 승인합니다.

~~~powershell
& 'C:\Program Files\PowerShell\7\pwsh.exe' -NoProfile -File 'F:\study\codex\Project-Limitless\Temp\DV2Fix20261128\TaskPrep\Issue-V2TaskOperationSidecars.ps1' -Operation Start -IssueAfterExplicitApproval
~~~

~~~text
ISSUE-V2-OPERATION-SIDECARS
~~~

~~~powershell
& 'F:\study\codex\Project-Limitless\Temp\DV2Fix20261128\TaskPrep\Invoke-V2TaskPrepared.ps1' -Mode Run
~~~

~~~text
START-ONE-FAKE-ONLY
~~~

이 래퍼는 Worker 활성화 후 1회 native start를 호출합니다. native 컨트롤러는 자기 Task를 원복하고, 래퍼는 최대 92초 읽기 대기 후 Worker를 비활성으로 되돌립니다. CTRL+C/종료/권리 오류 뒤 반드시 Inspect로 상태를 재확인합니다. Worker 상태만 원복하고 Task/프로세스에 손대지 않는 별도 명령은 Invoke-V2WorkerBatchProbe.ps1 -RestoreDisabledAfterExplicitApproval 입니다.

~~~text
RESTORE-V2-WORKER-DISABLED
~~~

## 성공 판정

관리 명령 종료 0만으로 제품 실행 성공으로 표시하지 않습니다. Scheduler LastTaskResult=0, 동일 Trial 등록/시작 영수증과 instance, created-once/generation/final, 최종 TOKEN_PROBE_ONLY_COMPLETE를 대조합니다. Supervisor는 Probe 강제 종료 코드 125 및 Job 내부 활성 0·총 1을 확인한 경우에만 해당 최종 문구를 냅니다. 마지막으로 전용 Task 비활성/인스턴스 0, Worker 비활성, 제품 프로세스 잔류 0 및 경로 불명 0. HOLD나 측정 누락 시 증거 보존, 재시도/다른 Task 중지 없음. NoAi/NoIpc/FAKE_ONLY 유지.

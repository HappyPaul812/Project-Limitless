# Discord V2 Trial 보호 갱신과 시험 승인

원래 보호 요청의 RSA-PSS 서명과 매니페스트에서 출발합니다. 현재 Trial·신뢰 JSON·근거의 SHA를 서명된 갱신 영수증으로 연결합니다. 기존 제품 EXE·서명·배포 원장·실패 증거는 그대로 보존합니다. 기존 SHA별 보호 폴더를 한 번만 생성하고, 이전 파일을 동일 핸들·ID·SD·SHA로 보관합니다. 새 3개 파일은 CREATE_NEW와 같은 핸들 readback을 요구하며 기록은 마지막에 커밋합니다. 부분 상태가 생기면 커밋이 없는 예약으로 HOLD하며 기존 증거를 살립니다.

## 지금 가능한 읽기 전용 확인

~~~powershell
& 'C:\Program Files\PowerShell\7\pwsh.exe' -NoProfile -File 'F:\study\codex\Project-Limitless\Temp\DV2Fix20261128\TrialRenewal\Invoke-ProtectedTrialRenewal.ps1'
~~~

기대 결과 TRIAL_RENEWAL_READINESS_VERIFIED_NO_SIGNING_NO_WRITES. 별도로 보관된 실패 Evidence의 DACL을 확인합니다.

~~~powershell
& 'F:\study\codex\Project-Limitless\Temp\DV2Fix20261128\TaskPrep\Test-V2ArchivedEvidenceWriteBoundary.ps1'
~~~

HOLD_ARCHIVED_EVIDENCE_WORKER_WRITE_ACE가 나오면 활성화/토큰/실행을 보류합니다. 보호 보관본에 기존 Worker 쓰기 ACE가 남은 상태에서 상위 보관소의 DACL만으로 실제 유효 쓰기 차단을 가정하지 않습니다. 보관본 권한 변경은 승인 범위가 따로 필요합니다. 이 조회는 ACL을 수정하거나 토큰을 발급하지 않습니다.

## 갱신 승인 T

등록부터 실행까지 실제 승인 담당자와 필요한 시간이 확보된 직후, 기존 Trial/신뢰 만료 상태에서만 진행합니다. 기존 Task 및 제품 프로세스/실행 증거/이전 동작 승인 입력이 없어야 합니다. 새 확인 근거를 로컬 프롬프트에 입력합니다. 비밀값은 입력하지 않습니다.

~~~powershell
& 'C:\Program Files\PowerShell\7\pwsh.exe' -NoProfile -File 'F:\study\codex\Project-Limitless\Temp\DV2Fix20261128\TrialRenewal\Invoke-ProtectedTrialRenewal.ps1' -PromoteAfterExplicitApproval
~~~

~~~text
PROMOTE-SIGNED-V2-TRIAL
~~~

갱신 승격 창 최대 5분, 새 Trial/신뢰 최대 15분. 성공 상태 TRIAL_RENEWAL_PROMOTED_SIGNED_CHAIN_VERIFIED_NO_TASK_EXECUTION. HOLD나 중단이 있으면 재시도, 이전 승인 재사용, 자동 정리 없음. 여러 차례 필요한 경우 결합된 이전 영수증을 검증하고 직전 Trial이 아직 살아 있으면 새 갱신은 거부됩니다. 이미 Task가 있는 경우 현재 등록 액션 SHA와 결합돼 있어 이 갱신 경로는 HOLD입니다.

## 등록 R · 별도 승인 및 유효기간 필요

~~~powershell
& 'C:\Program Files\PowerShell\7\pwsh.exe' -NoProfile -File 'F:\study\codex\Project-Limitless\Temp\DV2Fix20261128\TaskPrep\Issue-V2TaskOperationSidecars.ps1' -Operation Register -IssueAfterExplicitApproval
~~~

~~~text
ISSUE-V2-OPERATION-SIDECARS
~~~

~~~powershell
& 'F:\study\codex\Project-Limitless\Temp\DV2Fix20261128\TaskPrep\Invoke-V2TaskPrepared.ps1' -Mode Register
~~~

~~~text
REGISTER-DISABLED-V2-TASK
~~~

비활성·무트리거·LeastPrivilege·90초 및 정확한 액션/Worker SID, 작업 인스턴스 0을 Inspect로 읽습니다. 운영 Task 등록은 미검증입니다.

## 배치 토큰 P · 한 계정 활성화·로그온·비활성 원복 승인

등록 검증 및 보관본 쓰기 차단의 확인이 선행됩니다. 비표시 비밀번호 입력, 제품 프로세스 생성 없이 실제 로컬 배치 토큰의 SID/비관리자·Batch 그룹을 확인하고 즉시 닫습니다. 이후 비활성 readback 필수.

~~~powershell
& 'C:\Program Files\PowerShell\7\pwsh.exe' -NoProfile -File 'F:\study\codex\Project-Limitless\Temp\DV2Fix20261128\TaskPrep\Invoke-V2WorkerBatchProbe.ps1' -ProbeAfterExplicitApproval
~~~

~~~text
PROBE-V2-BATCH-AND-DISABLE
~~~

## 실행 F · 시작 파일 발급, 활성화·단발 실행·비활성 원복 별도 승인

실제 토큰 PASS와 Task 비활성/인스턴스 0을 먼저 확인합니다. 발급 창 확인 후 시작합니다.

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

~~~powershell
& 'F:\study\codex\Project-Limitless\Temp\DV2Fix20261128\TaskPrep\Invoke-V2TaskPrepared.ps1' -Mode Inspect
~~~

사후 판정은 Supervisor 0, Probe 125와 Job drain을 증명하는 최종 Evidence, 전용 Task 비활성/인스턴스 0, Worker 비활성, 잔류/경로 미확인 프로세스 0입니다. HOLD는 원인 확정 전 정책 복귀 여부만 조회합니다. 다른 Task 또는 프로세스 중지 없음. AI·외부 API 없음.

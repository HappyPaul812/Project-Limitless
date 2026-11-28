# 기존 Task 보호 복구 및 FAKE_ONLY 시험

관리자 PowerShell 7에서 실행합니다. A만 승인됐습니다. B·P·C·F는 각 해당 승인 후 실행합니다. P는 C 전에도 실행할 수 있어 5분 복구 창을 소모하지 않습니다. HOLD 후에는 재실행 전에 증거를 보존하고 상태를 조회합니다.

## A · 승인된 후보 1회 보호 서명

~~~powershell
& 'C:\Program Files\PowerShell\7\pwsh.exe' -NoProfile -File 'F:\study\codex\Project-Limitless\Temp\DV2ExistingTask20261128\Signing\Invoke-RecoverySupervisorSigning.ps1'
~~~
~~~text
SIGN-RECOVERY-SUPERVISOR
~~~

성공 표식은 `RECOVERY_SUPERVISOR_SIGNED_VERIFIED`입니다.

## B · 기존 실패 증거와 만료 입력 보호 보관 · 별도 승인

기존 Task와 소비 원장은 유지합니다. 등록 의도, HOLD, 만료 등록 승인 파일을 기존 SHA와 파일 ID로 대조해 보호 보관합니다. 보호 진단 로그는 제자리에 남습니다.

~~~powershell
& 'C:\Program Files\PowerShell\7\pwsh.exe' -NoProfile -File 'F:\study\codex\Project-Limitless\Temp\DV2ExistingTask20261128\RegistrationArchive\Invoke-FailedRegistrationArchive.ps1'
~~~
~~~text
ARCHIVE-EXISTING-TASK-FAILED-INPUTS
~~~

## P · Worker 배치 토큰 확인과 비활성 복귀 · 별도 승인

전용 Task가 비활성·무트리거·비실행이고 Worker 비활성인 상태에서 진행합니다. 기존 암호는 보안 콘솔 입력에서만 사용합니다.

~~~powershell
$status = & 'F:\study\codex\Project-Limitless\Temp\DV2Fix20261128\TaskPrep\Invoke-V2TaskPrepared.ps1' -Mode Inspect | ConvertFrom-Json
if (!$status.WorkerDisabled -or !$status.TaskPresent -or !$status.Task.Disabled -or $status.Task.TriggerCount -ne 0 -or $status.Task.Instances -ne 0 -or $status.RemainingProductProcesses -ne 0 -or $status.UnresolvedProcessPaths -ne 0) { throw 'HOLD_P_TASK_STATE' }
& 'C:\Program Files\PowerShell\7\pwsh.exe' -NoProfile -File 'F:\study\codex\Project-Limitless\Temp\DV2Fix20261128\TaskPrep\Invoke-V2WorkerBatchProbe.ps1' -ProbeAfterExplicitApproval
~~~
~~~text
PROBE-V2-BATCH-AND-DISABLE
~~~

`ACTUAL_BATCH_TOKEN_SID_AND_NONADMIN_BATCH_GROUP_VERIFIED`와 `WORKER_DISABLED_READBACK_NO_TASK_OR_PROCESS_CHANGE`를 확인합니다.

## C · 기존 Task 유지 보호 전환 및 새 서명 복구 영수증 · 별도 승인

A·B 후 읽기 전용 사전 확인을 수행합니다.

~~~powershell
& 'C:\Program Files\PowerShell\7\pwsh.exe' -NoProfile -File 'F:\study\codex\Project-Limitless\Temp\DV2ExistingTask20261128\Migration\Invoke-SupervisorTrialMigration.ps1'
~~~

`EXISTING_TASK_MIGRATION_READINESS_VERIFIED_NO_SIGNING_NO_WRITES` PASS, P PASS 및 실제 F 준비가 끝나면 C를 별도 승인합니다.

~~~powershell
& 'C:\Program Files\PowerShell\7\pwsh.exe' -NoProfile -File 'F:\study\codex\Project-Limitless\Temp\DV2ExistingTask20261128\Migration\Invoke-SupervisorTrialMigration.ps1' -MigrateAfterExplicitApproval
~~~

실제 신뢰 근거를 입력한 뒤 붙여넣습니다.

~~~text
RECOVER-EXISTING-TASK-AND-PROMOTE-TRIAL
~~~

성공 표식은 `EXISTING_TASK_RECOVERY_PROMOTED_NO_TASK_EXECUTION`입니다. 복구 영수증은 최대 5분, 신규 Trial은 15분입니다. 기존 Task 등록을 변경하지 않으며 기존 등록 성공 파일을 생성하지 않습니다.

## F · 새 Trial에 대한 시작 승인 및 단발 시험 · 별도 승인

~~~powershell
& 'C:\Program Files\PowerShell\7\pwsh.exe' -NoProfile -File 'F:\study\codex\Project-Limitless\Temp\DV2Fix20261128\TaskPrep\Issue-V2TaskOperationSidecars.ps1' -Operation Start -UseExistingTaskRecovery -IssueAfterExplicitApproval
~~~
~~~text
ISSUE-V2-OPERATION-SIDECARS
~~~
~~~powershell
& 'C:\Program Files\PowerShell\7\pwsh.exe' -NoProfile -File 'F:\study\codex\Project-Limitless\Temp\DV2Fix20261128\TaskPrep\Invoke-V2TaskPrepared.ps1' -Mode Run -UseExistingTaskRecovery
~~~
~~~text
START-ONE-FAKE-ONLY
~~~

종료 후 읽기 전용 상태를 확인합니다. 복구 창 만료 후에도 기본 Inspect를 사용할 수 있습니다. 기본 Inspect의 ProtectedActionMatch는 복구 이전 SHA와 신규 Trial SHA가 다르면 false로 표시되므로 상태/증거 조회에 사용합니다.

~~~powershell
& 'C:\Program Files\PowerShell\7\pwsh.exe' -NoProfile -File 'F:\study\codex\Project-Limitless\Temp\DV2Fix20261128\TaskPrep\Invoke-V2TaskPrepared.ps1' -Mode Inspect
~~~

Supervisor 0, Probe 125, Job drain, 최종 Evidence, Worker·Task 비활성 및 잔류·조회 불명 프로세스 0을 확인합니다.

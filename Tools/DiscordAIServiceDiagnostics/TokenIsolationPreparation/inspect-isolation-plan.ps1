param([ValidateSet('Inspect','Run')][string]$Action='Inspect')
$ErrorActionPreference='Stop';Set-StrictMode -Version Latest
# 이 준비 도구는 관리자 여부와 무관하게 로그온/프로세스/Job/실토큰 API를 호출하지 않습니다.
$r=@{schema=1;kind='SID1010_FAKE_ONLY_ISOLATION_PREPARATION';status='HOLD_SESSION0_LAUNCH_PATH_NOT_APPROVED';action=$Action;ready_for_actual_trial=$false;worker_sid='S-1-5-21-2858504392-1958168942-1662906075-1010';existing_task_changes=0;new_logons=0;worker_processes_created=0;job_objects_created=0;token_queries=0;ipc_calls=0;model_calls=0;automatic_retries=0;max_seconds=90;active_process_limit=1;runtime_admitted=$false;missing=@('APPROVED_SESSION0_LAUNCHER','PRIVILEGE_BASELINE_WITHOUT_GRANT','SEALED_NATIVE_DIAGNOSTIC_IMAGE','ATOMIC_JOB_CHILD_POLICY_NATIVE_VALIDATION','LIVE_CLEANUP_AND_EVIDENCE_VALIDATION')}
if($Action -ceq 'Run'){$r.status='HOLD_ACTUAL_EXECUTION_NOT_IMPLEMENTED_OR_AUTHORIZED'}
$r|ConvertTo-Json -Depth 6

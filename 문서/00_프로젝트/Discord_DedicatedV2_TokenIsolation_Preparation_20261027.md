# SID1010 토큰 관측 전용 FAKE_ONLY 격리시험 사전 준비 — 2026-10-27

상태 OFFLINE_DESIGN_PREPARED / HOLD_SESSION0_LAUNCH_PATH_NOT_APPROVED.
브랜치 codex/discord-usage-diagnostics-20261026, 기준813d37ead4fa605021b98026fee98e5d8671050a.
승인은 사전 준비만이며 실제 Windows 실행 승인이 아닙니다. 기존 Task descriptor PASS, 31/48/22 등 성공 결과는 재사용했고 반복하지 않았습니다.

## 1. 실행 경로의 중요한 제약

LogonUserW(LOGON32_LOGON_BATCH=4, 로컬 LIMITLESSWorkV2D, 로컬 SecureString 입력) → 실제 TokenUser1010/AuthLUID/LSA Batch4/Session0 검증 → CreateProcessAsUserW의 최소 진단 PE1개 생성이라는 절차를 조건부 설계했습니다.
**일반 관리자 PowerShell에서 이 경로가 바로 Session0으로 실행된다는 보장은 없습니다.**
CreateProcessAsUserW는 보통 SeIncreaseQuotaPrivilege 및 경우에 따라 SeAssignPrimaryTokenPrivilege가 필요하고, 토큰의 Session에서 실행됩니다. 해당 API가 비활성 privilege를 일시 활성화할 수 있으므로 필요한 privilege가 이미 승인된 실행 주체에 존재하고 활성화됐다는 근거가 없으면 호출 전 HOLD합니다.
SetTokenInformation(TokenSessionId=0)에는 Act As Part Of OS(SeTcbPrivilege) 조건이 필요합니다. 이번 범위에서 특권을 부여/활성화하거나 Session을 조작하지 않습니다. Interactive fallback(CreateProcessWithLogonW), 기존1007 인증/토큰 재사용, 기존 Disabled Task 실행으로 우회하지 않습니다.
현재 증거에는 적법한 Session0 생성 주체 및 그 권한 baseline이 없습니다. 따라서 실제 launcher를 연결하지 않았고 신규 시스템 Task/서비스/LSA 패키지를 설계 결정하거나 만들지 않았습니다. 필요한 경우 별도 범위 결정을 먼저 받아야 합니다.
실행 중에는 SID1010 Worker1개만 Job에 넣습니다. 기존 관리자 콘솔은 감독 주체이며, 별도 SYSTEM supervisor 프로세스가 필요해진다면 그것도 현재의 단일 대상 계획에 몰래 추가하지 않고 별도 검토/승인해야 합니다.

## 2. 단계별 생성·관측 계약 (실제 호출 미구현)

A. 비밀 입력 이전: 원본 설치/Stage/계정/Task/승인소비/Auth/OnBoot 불변 근거, 현재 descriptor PASS/파일SHA, 승인된 Session0 생성 경로·권한, 진단 이미지SHA, 생성 시 원자적 Job/child policy 지원, 외부 privileged handle 부재, 감독의 Job 상속불가를 모두 확인합니다. 하나라도 UNKNOWN이면 새로그온0/프로세스0 HOLD.
B. 완전히 별도 고유 시험 ID로 관리자/SYSTEM 보호 진단 evidence 위치에 CREATE_NEW 의도 기록을 durable flush합니다. 기존 승인/소비 epoch와 설치 Entry/Result/Lease는 수정하지 않습니다. 이번에 실제 승인파일이나 evidence staging을 생성하지 않습니다.
C. 무명 private Job, 생성 시 보호 SECURITY_ATTRIBUTES 및 non-inheritable handle, KILL_ON_JOB_CLOSE, ACTIVE_PROCESS_LIMIT1, BREAKAWAY/SILENT_BREAKAWAY 금지를 설정하고 readback합니다. Job handle은 감독만 보유하고 Worker에 상속/복사하지 않습니다.
D. 로컬 보안 입력으로 받은 시험 계정 암호만 메모리에서 사용해 Batch 로그온을 수행하고, 성공 직후 SID1010/primary token/AuthLUID/Batch4/Session0/비관리자·비상승·token groups/deny-only/restricted/privileges를 검증합니다. 암호를 인수/환경/JSON에 넣지 않으며 사용 버퍼를 zero/free하고 token handle을 닫습니다. 프로필·기존 인증·CODEX_HOME을 로드/복사하지 않습니다.
E. 고정 절대 lpApplicationName의 standalone native 진단 이미지1개만 CREATE_SUSPENDED|EXTENDED_STARTUPINFO_PRESENT|CREATE_UNICODE_ENVIRONMENT으로 생성하도록 설계합니다. PROC_THREAD_ATTRIBUTE_JOB_LIST로 생성 시 Job 편입, CHILD_PROCESS_RESTRICTED로 자식 생성을 제한합니다. 앱 인자/검색PATH/스크립트/셸/플러그인/CLR/다른실행기/IPC/network API는 배제합니다. 환경은 SystemRoot 등 최소 allowlist로 새로 구성하고 부모 환경·인증을 상속하지 않습니다.
F. 반환 handle로 SID/PID/생성UTC/AuthLUID/Batch4/Session0/이미지SHA/Job 정확 membership·active1·child restriction을 재확인한 뒤에만 resume합니다. 원자 Job 편입이 지원되지 않으면 '만든 뒤 Assign'으로 낮추지 않고 생성 전에 HOLD합니다.
G. 감독이 기존 AccessCheck reader의 core를 같은 process handle에 결합하는 방향으로 준비합니다. 그룹목록 추정 대신 실제 토큰/제한SID/거부권한/privileges·읽기와 실행쓰기삭제WDACWOWNER·MAX를 평가합니다. 새 구현에서는 PID로 재탐색하는 틈을 줄이고 handle/생성UTC/Auth/ModifiedID를 대조해야 합니다.
H. Worker 자체는 수동 대기·자기 신원 최소 검증·유한 정상 종료만 수행하는 소형 프로그램이어야 합니다. Discord/AI/IPC/CodexCLI/업무·사용량/인증 호출0입니다. 실행 가능한 진단 PE는 아직 제작·서명/해시 봉인하지 않았습니다. 소스/모델 SHA를 PE SHA로 부르지 않습니다.

## 3. 90초·단일 프로세스·자식 및 handle 정책

Job의 per-job/process user-time은 CPU 시간이며 90초 wall-clock 보장이 아닙니다. 전체 단조 시계 budget90초를 별도 감독하며, 85초까지 Worker 종료 명령을 내리고 마지막5초를 drain/active0·handle 확인에 예약하는 계약입니다.
지원하지 않는 attribute/중첩Job/시스템 sleep·감독 정지/API 블록 등으로 native deadline 증명이 불가능하면 HOLD합니다. 모델 통과가 Windows 실시간 deadline 보장을 뜻하지 않습니다. 특히 LogonUser 같은 비취소형 호출의 hang 처리와 credential 수명까지 native 실행 설계에서 추가 확인해야 합니다.
CHILD_PROCESS_RESTRICTED는 privileged foreign process handle을 악용하면 우회될 수 있습니다. 따라서 이 flag 하나로 sandbox를 증명하지 않습니다. active1/no-breakaway/최소 token/고정 코드·환경·권한·외부 위험 handle 부재 검증을 병행하고 입증 부족 시 HOLD합니다.
Job active1은 한 순간의 동시 프로세스 한도입니다. Worker가 자기 종료 후 다른 프로세스를 교체하는 방식까지 이 값 하나로 배제했다고 주장하지 않습니다. 단일 진단 이미지와 child policy 및 native readback이 모두 필요합니다.
Worker가 Job handle을 받지 않으며 관리자 프로세스만 마지막 handle을 소유해야 합니다. 숨은 duplicate handle이 있으면 창 종료 kill-on-close가 보장되지 않으므로 실행 불가입니다.

## 4. 종료·실패·중단·창 종료 증거 처리

| 상황 | 새 시험 대상의 정리 | 증거와 상태 |
|---|---|---|
| 정상 종료 | 정상 process signaled/exit·Job active0 확인, thread/process/token/job/attribute/env/secret buffer 해제 | 성공·PIDgeneration/Auth·권한·시간·정리 결과를 별도 최종 journal에 flush, token관측 범위 PASS만 가능 |
| 부분 실패/잘못된SID/Session/권한 | resume 전 정지, 새 시험 소유 Job만 종료·drain·handle 해제 | 원시 Type/HRESULT/Win32/NTSTATUS/단계·생성자산 보존, HOLD·재시도0 |
| 85초 timeout | 소유 Job handle에만 TerminateJobObject, 90초 안 active0/handle 확인 | 초과/정리불명은 HOLD; CPU limit으로 wall budget을 대체하지 않음 |
| 취소/Ctrl+C | 소유 Job만 종료하고 finally의 SafeHandle 해제 시도 | 취소와 cleanup을 별도 기록, 기존Worker/Task를 종료하지 않음 |
| 관리자 창 종료/프로세스 강제종료 | 사용자 finally를 가정하지 않음. OS가 감독의 마지막 Job handle을 닫아 KILL_ON_JOB_CLOSE가 적용되는 설계 | 사전 durable intent만 남을 수 있음. 최종 JSON 작성/원본 token조회/정리 완료를 주장하지 않고 사후 읽기 검토 HOLD |
| cleanup/native Close 실패 | globalPID kill/운영 프로세스 종료/자동삭제로 확대하지 않음 | UNKNOWN 및 원시 코드 보존, 다음 복구는 별도 승인 |

이번 모의 모델은 창 종료를 사용자 finally와 다른 kernel-close 경로로 표현했습니다. 실제 kernel handle lifetime/정리·로그 flush는 native 시험 전 미검증입니다.

## 5. 기존 운영 보호·충돌 조건

기존 V2 r2 Task는 Disabled/Trigger0 그대로, 계정1009/1010/1007·권리·ACL·Code/State/Auth·r1/r2Stage·설치승인consumed/EntryResultLease·OnBoot2/기존11프로세스는 변경하지 않습니다.
시험의 Job/handle/token/proc 경계는 고유 시험에서 생성한 객체에만 적용합니다. SID1007 소유 PID/토큰/Job은 열어 대체하거나 종료하지 않습니다.
기존 봇/아트/인증/CODEX_HOME/SQLite/게임/Unity/Save를 열지 않는 고정 이미지·빈업무/무IPC로 자원/인증 경합을 줄입니다. CPU·구독·비협조 실행기의 전역 독점성까지 입증한 것은 아니므로 runtime/운영모드 HOLD 유지입니다.
새 staging/evidence/실행 주체를 실제로 만드는 범위는 현재 승인되지 않았습니다.

## 6. 산출물·모의 검증과 관리자 명령

TokenIsolationPreparation/IsolationModel.cs는 Windows API 없는 가상 상태·시계·handle 모델입니다.
inspect-isolation-plan.ps1은 Inspect만 준비 상태를 출력하며 Run 요청도 HOLD로 차단합니다. 실제 launcher가 아니고 암호/계정/Task/Job/토큰 API를 호출하지 않습니다.
신규28 PASS /0 FAIL: 필수 gate9개 중 하나라도 불명일 때 로그온 전 거부, journal/job/logon/Session/SID/privilege/create/PID/token/권한/cancel/timeout/child 오류, 창종료 kernel model, CPU-vs-wall budget, cleanup불명·최종증거실패를 구분했습니다.
실제로그온/시험프로세스/Job/토큰0, 기존성공테스트반복0. 결과 Evidence/TokenIsolationPreparation20261027/OfflineTests.json, Readiness.json, SourceSHA256.json.

SHA256:
- inspect-isolation-plan.ps1 4f59204005a9e92fab5904875e1b37f196fcd8b512214a3d2b323faba3dd913c
- IsolationModel.cs 9c6d4953f36540a709f3f64fce9fe2721d57c9a065da2d481b0e42ecb5078486
- test-isolation-model.ps1 a36ce72c201996a829c150bb5da53887b76a404f4e6fbaaf1883422a05afe8c9

관리자 PowerShell7용 **준비 상태 확인 명령**입니다. 실제 시험 명령이 아닙니다. 이번에는 모델시험 외 수동 Inspect 실행도 하지 않았습니다.
`powershell
$r2PlanCmd='$r2PlanBytes=[IO.File]::ReadAllBytes("F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\Tools\DiscordAIServiceDiagnostics\TokenIsolationPreparation\inspect-isolation-plan.ps1");if([Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($r2PlanBytes)).ToLowerInvariant() -cne "4f59204005a9e92fab5904875e1b37f196fcd8b512214a3d2b323faba3dd913c"){throw "R2_ISOLATION_PLAN_SHA_HOLD"};& ([scriptblock]::Create([Text.Encoding]::UTF8.GetString($r2PlanBytes))) -Action Inspect';Start-Process -FilePath 'C:\Program Files\PowerShell\7\pwsh.exe' -ArgumentList @('-NoLogo','-NoProfile','-EncodedCommand',[Convert]::ToBase64String([Text.Encoding]::Unicode.GetBytes($r2PlanCmd))) -NoNewWindow -Wait
`

예상 status=HOLD_SESSION0_LAUNCH_PATH_NOT_APPROVED, ready_for_actual_trial=false, 모든 실제 생성/조회 카운터0.
Run 분기는 HOLD_ACTUAL_EXECUTION_NOT_IMPLEMENTED_OR_AUTHORIZED입니다. 실제 로그온/생성 명령을 준비 완료했다고 주장하거나 대표에게 실행 요청하지 않습니다.

## 7. 실제 시험 승인 전 필요한 조건

1. 기존 Task/계정권한 변경 없이 사용할 승인된 Session0 생성 경로 및 이미 존재·활성인 필요 privilege를 입증하거나, 별도 운영 변경 범위를 검토받아야 합니다. 현재는 근거 없음으로 HOLD입니다.
2. standalone 진단 PE·native 감독기/adapter의 코드·바이트 SHA·보호 staging·입력/환경과 handle 상속 정책을 봉인하고, 원자 Job/child/90초/중단/창 종료·credential cleanup의 native 경로를 검토해야 합니다.
3. 실제 로그온1회/진단process1개/새 Job·별도 증거 경로·정리 범위·로컬 보안 암호입력만 포함하는 구체적 단발 실행 명령을 완성한 뒤, 바울 대표의 별도 실행 승인을 받아야 합니다.
지금은 실행 경로·native image·권한/cleanup이 미확정이므로 실전 승인 요청의 마지막 단계에 도달하지 않았습니다. 작업이 어렵다는 이유로 안전 조건을 완화하지 않습니다.
V1/MANUAL/A/유효토큰/Session0Batch/Job독점/runtime HOLD 유지. 사전 준비·최소 모의검증 완료 후 중지, LOCAL_COMMITTED / REMOTE_NOT_PUSHED, 최종커밋 Issue후속.

공식 근거:
https://learn.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-createprocessasuserw
https://learn.microsoft.com/en-us/windows/win32/api/winnt/ne-winnt-token_information_class
https://learn.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-updateprocthreadattribute
https://learn.microsoft.com/en-us/windows/win32/procthread/job-objects
https://learn.microsoft.com/en-us/windows/win32/api/winnt/ns-winnt-jobobject_basic_limit_information

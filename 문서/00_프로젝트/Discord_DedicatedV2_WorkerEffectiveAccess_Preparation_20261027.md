# r2 Worker 유효 접근·독점성 후속 준비 — 2026-10-27

상태 OFFLINE_PREPARED / ACTUAL_TOKEN_UNVERIFIED / EXCLUSIVITY_HOLD.
브랜치 codex/discord-usage-diagnostics-20261026, 기준9c3582c7dcd62658c9ef694f491dbc190b736493. Issue66 최신6441249450/6441257930, 기존 Task descriptor PASS 및48PASS/22PASS를 재사용했습니다. 성공 검사·기존11프로세스/334프로세스/45query 전수조사 반복0.
실제 Windows/관리자 토큰 조회0, Task실행·새로그온·IPC·AI·계정TaskACL·승인소비·인증·OnBoot·게임 원본 변경0.

## 최소 실제 토큰 도구

WorkerEffectiveAccess/ExistingTokenRead.cs, TokenPolicy.ps1, probe-existing-token-readonly.ps1, test-token-policy-offline.ps1.
기존 SID1010 프로세스의 **확인된 PID와 생성UTC**를 입력으로 받습니다. 현재 보존 자료에는 그 기존 대상이 없습니다. 이는 토큰 부재 확정이 아니라 입력·증거 부족입니다.
PID/생성시각이 없으면 HOLD_EXISTING_TOKEN_TARGET_REQUIRED, 프로세스/토큰 조회0. 새로운 Task나 로그온을 만들어 대상을 확보하지 않습니다. 확인된 대상 확보 자체가 필요하면 후속 최소 읽기 범위를 별도 합의해야 합니다. 임의 PID/추측 시각/기존 SID1007 PID를 사용하지 않습니다.

OpenProcess(QUERY_LIMITED_INFORMATION) 및 GetProcessTimes로 세대를 고정합니다. CIM 시각 정밀도는 microsecond이며 보유한 같은 프로세스 handle에서 종료·생성시각을 다시 확인합니다.
OpenProcessToken(QUERY)에서 실제 TokenUser=SID1010인지 확인한 뒤 TokenGroups·group attributes·restricted SID·token session/elevation·TOKEN_STATISTICS의 Auth/Token/Modified ID를 읽습니다.
AccessCheck에는 impersonation token 형식이 필요하므로 같은 기존 token을 QUERY|DUPLICATE로 열고 **메모리상의 평가용 사본만** DuplicateToken(SecurityImpersonation)으로 만듭니다. 원본 token과 AuthID를 대조하며, 실제 impersonation/새 LSA 로그온/프로세스 생성/권한 부여는 하지 않고 모든 handle을 닫습니다. TOKEN_DUPLICATE 거부도 HOLD이며 SeDebugPrivilege 활성화·우회는 없습니다.
이 API가 만드는 토큰 사본과 새 로그온은 다릅니다. LogonUser/CreateProcess*/Impersonate/SetThreadToken/AdjustTokenPrivileges API는 선언하지 않습니다.

대상 Task의 GetTask/GetSecurityDescriptor(7)는 실제 AccessCheck에 필요한 현재 SD만 읽고 기존 PASS의 SDDL과 동일한지 확인합니다. descriptor 후보48검증·Principal/Action/전체45query를 다시 실행하지 않습니다. 앞뒤 SD가 달라도 HOLD입니다.
Windows AccessCheck는 실제 토큰의 Enabled/deny-only 그룹·restricted SID·권한/거부 ACE를 함께 평가합니다. Groups 목록을 직접 합산해 실제 유효권한으로 대체하지 않습니다.
Task-file generic mapping을 사용하고 DesiredAccess는 미리 specific으로 고정합니다: 읽기0x120089, 실행0x20, write-data2/append4/write-EA0x10/write-attributes0x100/DELETE0x10000/WRITE_DAC0x40000/WRITE_OWNER0x80000 및 MAXIMUM_ALLOWED.
읽기 허용, 위험권리 각각 거부, MAX 결과 정확읽기집합, 사용특권0, 비상승·원본token 안정성 확인이 모두 필요합니다. API실패/unknown/권한추가/원본token교체/ModifiedID변화/프로세스종료는 HOLD. 원시 Type/HRESULT/Win32/operation 및 LSA NTSTATUS를 보존합니다.

제한: AccessCheck는 **그 시점 프로세스 기본 토큰과 Task DACL의 모델 평가**입니다. Task RPC 서비스의 별도 검사, thread impersonation token, 무결성 label/SACL에 따른 실제 동작, 다른 개체와 임의 미래 실행기까지 증명하지 않습니다. 정책 결과가 EXISTING_PROCESS_TOKEN_DACL_MODEL_PASS여도 미래 Worker 실행권한 및 전체 runtime PASS가 아닙니다.

## 별도 검증 항목과 독립 시험 계획

| 항목 | 이번 준비/수집 설계 | 계속 미검증인 범위 |
|---|---|---|
| 유효 토큰 | 실제 기본token의 AccessCheck + 그룹속성/restricted/deny 판독 | 기존대상없음·새실행토큰·threadtoken·실서비스접근 |
| Session0 | TokenSessionId와 LSA logon session의 session 대조 | PIN입력전 실제부팅 성공 |
| Batch | 실제 Auth LUID에 LsaGetLogonSessionData의 security LogonType4 확인 | Task Scheduler GROUP enum4와 혼동금지, 신규 Task 실행 미검증 |
| Job | IsProcessInJob로 any-job 여부만 읽음 | Job식별·전용성·자식포함·제한·정리/남은handle은 UNKNOWN |
| SID1007 충돌 | 신규1010과1007 SID·파일·Auth·Task·IPC 분리 원칙 재사용 | 기존11프로세스를 종료/무시하지 않음, 공용 구독·CPU/CLI·다른실행기 경합은 UNKNOWN |
| Lease/승인 | 기존 기록 보존, 단발소비·epoch·Job/PIDgeneration 연결 설계 | 빈/부재 lease만으로 독점 선언 금지 |
| FAKE_ONLY IPC | 아래 별도 계획만 작성 | 실제 실행·모델·Discord 0 |

FAKE_ONLY 독립 시험은 별도 승인으로만 진행합니다:
1. 검증된 별도 시험 범위·SHA·고유 승인/소비 epoch·Task/Worker/SID/PIDgeneration 및 runtime 경계 고정. 기존 consumed 승인을 재사용하지 않습니다. 이번에 새 승인을 발행하지 않습니다.
2. 실제 생성 시점부터 Job 및 자식 프로세스 포함, 최대1작업·90초·자동재시도0·고유IPC·원자적lease 경쟁을 검증할 계획을 준비합니다. 다른 실행기가 lease를 우회할 수 있으면 HOLD.
3. 먼저 가짜 Scheduler/Worker/Job/시계로 동시요청·중복소비·PID재사용·timeout·Job자식잔존·오류결과의 HOLD를 검증하고, 각 생성/종료 권한 범위도 후속 승인에 명시합니다.
4. 실제 실행 승인을 받은 때만 제한된 비AI FAKE_ONLY 단발 Session0 시험을 검토합니다. 종료 시 Job/자식/lease의 처리와 증거 보존을 검증하고, 실패 시 운영 Worker나 기존11프로세스에 영향을 주지 않습니다. 이번에는 Task/Worker/Job/IPC를 생성·실행하지 않습니다.
5. 다른 실행기/같은PC·구독 한도의 충돌이 통제되지 않으면 runtime HOLD 유지. SID분리·any-job·lease 부재만으로 해소하지 않습니다.

## 신규 모의 검증

31 PASS / 0 FAIL. 기존48/22 반복false.
C#컴파일·ScriptBlock실행구문, 실제API호출전PID0차단, 실제token없는group정보거부, fake AccessCheck read/overgrant/unknownMax/privilege/elevated/세대변경, fake DACL의 Users간접allow·deny-only allow거부/deny적용·disabledgroup·restricted2차검사, Batch/Session/LSA불명, any-job·lease부재·Session0Batch만으로 독점/실행허용불가를 확인했습니다.
가짜DACL은 시험용 한정 모델이며 실제 native AccessCheck 대체물이 아닙니다. 실제 OS token/AccessCheck/LSA 조회0, 새로운 로그온0. native 함수 본체의 Windows 동작은 아직 미검증입니다.
증거 Evidence/WorkerEffectiveAccess20261027/OfflineTests.json, SourceSHA256.json, Preparation.json.

SHA256:
- probe-existing-token-readonly.ps1 581d2c91ee4ce4037e251169dc1a99793b71b6700bfc7ed3bfc8645901c7eb7d
- ExistingTokenRead.cs a00de5e9a49c550a1aa2c261a5c735d37938d897a5670eaaa238469a3fecc365
- TokenPolicy.ps1 567f01ea08b1eb6c3fbe7ef47c8f7626e9cab00d1889ce894cd126a1fad3eb10
- test-token-policy-offline.ps1 d392bbf368da2341e049891c3fb22baf904147ff0617adb14428c960739a6322

## 별도 승인 후 관리자 PowerShell7 한 줄

이번에는 실행하지 않습니다. 다음 명령은 암호가 아닌 **확인된 기존 SID1010 PID/생성UTC(Z)**만 로컬 입력받습니다. 값이 없으면0/빈시각으로 HOLD합니다. 대상 확보를 위해 Task를 시작하거나 새 로그온하지 않습니다.
`powershell
$r2TokenCmd='$r2TokenBytes=[IO.File]::ReadAllBytes("F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\Tools\DiscordAIServiceDiagnostics\WorkerEffectiveAccess\probe-existing-token-readonly.ps1");if([Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($r2TokenBytes)).ToLowerInvariant() -cne "581d2c91ee4ce4037e251169dc1a99793b71b6700bfc7ed3bfc8645901c7eb7d"){throw "R2_EXISTING_TOKEN_TOOL_SHA_HOLD"};& ([scriptblock]::Create([Text.Encoding]::UTF8.GetString($r2TokenBytes))) -ConfirmApprovedReadOnly -TargetProcessId ([int](Read-Host "Known existing SID1010 PID; 0 if unknown")) -ExpectedCreatedUtc (Read-Host "Known creation UTC with Z; empty if unknown")';Start-Process -FilePath 'C:\Program Files\PowerShell\7\pwsh.exe' -ArgumentList @('-NoLogo','-NoProfile','-EncodedCommand',[Convert]::ToBase64String([Text.Encoding]::Unicode.GetBytes($r2TokenCmd))) -NoNewWindow -Wait
`

예상 결과: 대상정보없음=HOLD_EXISTING_TOKEN_TARGET_REQUIRED; 접근거부/PID재사용/잘못된SID/API불명/overgrant=HOLD; 샘플token DACL 평가완료=TOKEN_DACL_CHECK_COMPLETE_RUNTIME_EXCLUSIVITY_HOLD.
어떤 경우에도 runtime_admitted=false, job_exclusivity/old_worker_conflict=UNKNOWN. LSA조회가 불가하면 Batch/Session 판정도 UNKNOWN입니다.
결과: F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\문서\00_프로젝트\팀운영\Evidence\WorkerEffectiveAccess20261027\Admin-<GUID>.json. 정제JSON1개만 CreateNew합니다. 명령줄/환경/암호/인증 토큰은 읽거나 저장하지 않습니다.
자동재시도/삭제/권한완화 금지, PowerShell exit 없음. 기존 descriptor PASS·V1/MANUAL/A/runtime HOLD 유지. 계정·Task·ACL·승인소비·인증·OnBoot·게임/Unity/Save 변경0, Push0. 준비 완료 후 중지합니다.
최종 로컬SHA Issue후속 / LOCAL_COMMITTED / REMOTE_NOT_PUSHED.

공식 참고:
https://learn.microsoft.com/en-us/windows/win32/api/securitybaseapi/nf-securitybaseapi-accesscheck
https://learn.microsoft.com/en-us/windows/win32/api/securitybaseapi/nf-securitybaseapi-duplicatetoken
https://learn.microsoft.com/en-us/windows/win32/api/ntsecapi/nf-ntsecapi-lsagetlogonsessiondata

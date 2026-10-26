# Issue #66 미확인 프로세스 읽기 전용 조사 — 2026-10-27

상태 **REFERENCE_VERIFICATION_HOLD / WAIT_ADMIN_READONLY / NOT_CLEANED**.
기준 a8ec860e922d088a5f4699157ac957aa441204e0, 브랜치 codex/discord-usage-diagnostics-20261026.
기존 Task282·서비스/드라이버792·프로세스334 성공 검사는 재실행하지 않았습니다.

## 확인 결과

정체 확인 **0/19**, 실제 참조 발견 **0건(무참조 확정 아님)**. 현재 조회 시도16개 ACCESS_DENIED, 원본 PID 누락3개 UNKNOWN. 과거 동일성은 모두 UNKNOWN입니다. 보존된16개의 현재 존재·종료 여부도 이번 토큰으로 확정하지 못했습니다.

| 과거 PID | 현재 조회 상태 | 근거 |
|---|---|---|
| 288 | ACCESS_DENIED | OpenProcess Win32=5; CIM native_error_code=2, HRESULT=-2146233088, StatusCode=0 |
| 328 | ACCESS_DENIED | OpenProcess Win32=5; CIM native_error_code=2, HRESULT=-2146233088, StatusCode=0 |
| 940 | ACCESS_DENIED | OpenProcess Win32=5; CIM native_error_code=2, HRESULT=-2146233088, StatusCode=0 |
| 1316 | ACCESS_DENIED | OpenProcess Win32=5; CIM native_error_code=2, HRESULT=-2146233088, StatusCode=0 |
| 1452 | ACCESS_DENIED | OpenProcess Win32=5; CIM native_error_code=2, HRESULT=-2146233088, StatusCode=0 |
| 1532 | ACCESS_DENIED | OpenProcess Win32=5; CIM native_error_code=2, HRESULT=-2146233088, StatusCode=0 |
| 1552 | ACCESS_DENIED | OpenProcess Win32=5; CIM native_error_code=2, HRESULT=-2146233088, StatusCode=0 |
| 1588 | ACCESS_DENIED | OpenProcess Win32=5; CIM native_error_code=2, HRESULT=-2146233088, StatusCode=0 |
| 3360 | ACCESS_DENIED | OpenProcess Win32=5; CIM native_error_code=2, HRESULT=-2146233088, StatusCode=0 |
| 5000 | ACCESS_DENIED | OpenProcess Win32=5; CIM native_error_code=2, HRESULT=-2146233088, StatusCode=0 |
| 5128 | ACCESS_DENIED | OpenProcess Win32=5; CIM native_error_code=2, HRESULT=-2146233088, StatusCode=0 |
| 10376 | ACCESS_DENIED | OpenProcess Win32=5; CIM native_error_code=2, HRESULT=-2146233088, StatusCode=0 |
| 19404 | ACCESS_DENIED | OpenProcess Win32=5; CIM native_error_code=2, HRESULT=-2146233088, StatusCode=0 |
| 4912 | ACCESS_DENIED | OpenProcess Win32=5; CIM native_error_code=2, HRESULT=-2146233088, StatusCode=0 |
| 28124 | ACCESS_DENIED | OpenProcess Win32=5; CIM native_error_code=2, HRESULT=-2146233088, StatusCode=0 |
| 32660 | ACCESS_DENIED | OpenProcess Win32=5; CIM native_error_code=2, HRESULT=-2146233088, StatusCode=0 |
| 누락1 | UNKNOWN | 원본에 PID 없음 |
| 누락2 | UNKNOWN | 원본에 PID 없음 |
| 누락3 | UNKNOWN | 원본에 PID 없음 |

16개 모두 이름·소유 계정/SID·Session·실행 경로·명령줄 확보·보호 수준 확인 불가입니다. 보호된 Windows 시스템 프로세스라고 판정하지 않습니다. 현재 거부 코드는 **현재 검사**의 근거이며 과거 실패 원인으로 소급하지 않습니다.

원본 SHA256 2379842226cb58aa83c01132915c0a7d1675b8f9243dce2d29532d4a751bdc9f. 원본 references.unknown_pids는16개만 존재합니다. 기존 cleanup-partial-trial.ps1 Read-CleanupRefs의 Select-Object -First 16 때문에 3개가 잘렸습니다. unknown은 비커널 프로세스의 ExecutablePath 또는 CommandLine 공백에서 증가하며, **어느 필드인지/개별 실패 코드/이름/생성시각은 저장하지 않았습니다**. 전체 HOLD 예외 HRESULT는 개별 PID 오류가 아닙니다. 원본 파일은 수정하지 않았습니다.

## 전용 진단기

[스크립트](../../Tools/DiscordAIServiceDiagnostics/probe-unknown-processes-readonly.ps1).
SHA256 **33bc59120213a49f1e27a9e96f25cb872db566783d8eed46c3595d168d28aaf1**.

Windows OpenProcess(PROCESS_QUERY_LIMITED_INFORMATION=0x1000), GetProcessTimes, QueryFullProcessImageName, GetProcessInformation(ProcessProtectionLevelInfo), GetShortPathName 및 PID 필터 Win32_Process/GetOwner/GetOwnerSid만 조회합니다. Windows 제공 오류와 숫자 반환값을 구분해 기록합니다. 원시 예외 메시지/명령행/환경변수는 출력·저장하지 않습니다. 비시스템 이미지 경로는 공유 결과에 REDACTED_NON_SYSTEM_PATH로 기록합니다. 시스템 경로도 보호/신뢰 판정 근거로 사용하지 않습니다.

명령행·이미지·소유자에 시험 root의 긴 이름/실제8.3별칭, 계정, SID, Task 이름이 나타나는지만 메모리에서 검사합니다. 환경변수 확장, junction/다른 경로 별칭, 간접 파일 열기, 모듈/핸들·현재 디렉터리까지 완전 확인하는 도구가 아닙니다. 따라서 **문자열 불일치로 NO_REFERENCE_VERIFIED를 만들지 않습니다**. 보호 수준 확인도 무참조 근거가 아닙니다. 보호정보 실패/불명이면 UNKNOWN을 보존합니다.

과거 생성시각 부재 때문에 현재 조회가 성공해도 SAME으로 승격하지 않습니다. 현재 PID 부재는 OpenProcess87과 CIM 부재의 동시 근거에서만 PROCESS_EXITED로 기록하며 과거 실행의 이미지/참조는 복구하지 않습니다. 핸들과 CIM 생성시각 비교로 검사 중 PID 교체도 구분합니다.

검증: 새로운 합성 **9 PASS/0 FAIL**(별칭/대소문자/빈별칭/참조우선/거부/부재/증거부족/오류비밀제거). PowerShell 파싱 오류0, AST 호출목록 및 native 선언 수동검토, git diff --check PASS. 실제 일반토큰16개 조회 완료. 관리자 성공 분기·보호 수준 조회 성공 분기는 현 환경에서 미검증입니다. 기존 설치/정리 테스트 전체 반복0.

현재 결과: [93caaadf8461456db97236a77a689885.json](팀운영/Evidence/UnknownProcesses20261027/93caaadf8461456db97236a77a689885.json). 초기 결과 ec85802a…도 보존했습니다. 초기 결과의 CIM 숫자코드 보존을 보완한 뒤 위 최종결과를 생성했습니다. 재검사는 미확인16개에만 한정했습니다.

## 바울 대표 직접 실행 — 관리자 PowerShell 7

관리자 PowerShell 7에서 아래 한 줄을 실행합니다. 별도 NoProfile 자식 프로세스이며 대화형 창을 종료하는 exit가 없습니다. 기존 설치·정리 스크립트는 호출하지 않습니다. SeDebugPrivilege 활성화·ACL변경·보호 우회·계정/서비스/Task 변경·프로세스 종료/재시작은 없습니다.

```powershell
& 'C:\Program Files\PowerShell\7\pwsh.exe' -NoProfile -NonInteractive -Command '& { $probeBytes=[IO.File]::ReadAllBytes("F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\Tools\DiscordAIServiceDiagnostics\probe-unknown-processes-readonly.ps1"); if([Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($probeBytes)).ToLowerInvariant() -ne "33bc59120213a49f1e27a9e96f25cb872db566783d8eed46c3595d168d28aaf1"){throw "PROBE_SHA_HOLD"}; & ([scriptblock]::Create([Text.Encoding]::UTF8.GetString($probeBytes))) -ScriptDirectory "F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\Tools\DiscordAIServiceDiagnostics" }'
```

비밀 없는 결과 저장 경로:
F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\문서\00_프로젝트\팀운영\Evidence\UnknownProcesses20261027\<새GUID>.json

관리자도 Windows가 제공하지 않는 정보는 UNKNOWN/ACCESS_DENIED로 남깁니다. 오류 원문이나 명령행을 별도로 수집해 공유하지 마십시오.

## HOLD 해제와 안전한 다음 단계

현재 해제 불가입니다. 관리자 결과를 판독하고도 누락3개와 과거 생성시각·실패 필드 부족은 남을 수 있습니다. 이미 존재하는 비밀 없는 당시 프로세스 ID/생성시각 증거가 있다면 연결 검증하는 방법이 우선입니다. 현재 조회만으로 과거19개를 복원하거나 이미 종료됐다고 추정하지 않습니다.

당시 증거가 없으면 과거19개 완전복구 불가를 명시해야 합니다. 그 경우 별도 승인된 **현재 미확인 대상만 보존하는 새 읽기 전용 진단 범위** 또는 검증된855개 자산의 사용 여부에 대한 별도 검증 계획을 합의해야 합니다. 이번에는 전체 프로세스 재검사/파일 핸들 검사/다른 진단 서비스 시작을 실행하지 않았습니다. 경로 별칭·간접 참조까지 해소할 안전한 방법이 없으면 HOLD를 계속 유지합니다.

부분 정리 재개 조건은 모든 관련 참조·계정/자산 상태가 신뢰할 수 있는 증거로 해소되고, 기존 증거·원본 보호 및 변경 범위가 검토되며, **바울 대표가 후속 정리를 명시 승인하는 것**입니다. 이전 Cleanup은 자동 재실행하지 않습니다. 새 LSA 설치도 별도 승인 대상입니다.

파일/계정/Task 삭제0, 설치/IPC/FakeWorker/AI/usage/Discord/A모드/기존봇·아트·OnBoot/Unity·Save/Push/main/종료·재부팅 변경0.
LOCAL_COMMITTED / REMOTE_NOT_PUSHED. 실제 커밋은 CODEX_LATEST 및 Issue66 후속 댓글에 기록합니다.

## 공식 조회 근거

- [Win32_Process 및 소유자 조회](https://learn.microsoft.com/en-us/windows/win32/cimwin32prov/win32-process)
- [OpenProcess 권한·오류](https://learn.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-openprocess)
- [GetProcessInformation](https://learn.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-getprocessinformation)
- [보호 수준 구조](https://learn.microsoft.com/en-us/windows/win32/api/processthreadsapi/ns-processthreadsapi-process_protection_level_information)


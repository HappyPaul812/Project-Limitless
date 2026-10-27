# Issue66 Worker11·GroupPrincipal 전용 읽기 조사

**신규 오프라인56 PASS / 0 FAIL. 실제 조사 HOLD_ACCESS_DENIED_11 / GROUP_UNKNOWN / EXCLUSIVITY_UNKNOWN.** r11의 기존 차단 정책을 변경하지 않았고 프로세스를 제외하거나 0개로 만들지 않았습니다. 실제 AI 실행·동시 충돌의 존재와 부재 모두 확정하지 못했습니다.

브랜치 `codex/discord-usage-diagnostics-20261026`, 지정 판독 기준 `8b1371618153b8494bf0f67456c08241a3f66ac6`. 기존 관리자76eec5af JSON SHA `8223799133dea9736f9dbe341d4498c0a020ec1880a2cb1797fea1cb7e86ccbe`의 PID·생성시각11개를 고정 재사용했습니다. 최신 기획 Issue66 댓글6433320343도 확인했습니다. 전체 preflight/이전 Worker probe/855전수/기존 성공suite 반복0.

## 1. 실제 11개 역할과 근거

OS LookupAccountNameW/LookupAccountSidW로 `PAUL-PC\LIMITLESSRelaySvc`가 로컬 사용자·SID `S-1-5-21-2858504392-1958168942-1662906075-1007`인 것은 확인했습니다. 이것이 개별11개 프로세스의 현재 소유 SID 확인을 대신하지는 않습니다.

새 도구의 현재 비관리자 실행에서 **11개 모두 OpenProcess(PROCESS_QUERY_LIMITED_INFORMATION=0x1000) Win32 5**. 내부 WorkerQueryFailure HRESULT -2147467259, PowerShell MethodInvocationException -2146233087. 실제 생성시각·개별 owner/token·이미지·서명·부모/자식·Job은 미확인입니다. 기존 SID1007 로그온 연관과 현재 소유자를 혼동하지 않습니다.

| PID | 원본 생성시각 UTC (2026-10-23) | 이번 판정 | 정제된 역할 |
|---|---|---|---|
| 6356 | 17:40:25.1963220 | ACCESS_DENIED/Win32 5 | UNKNOWN |
| 7432 | 17:40:23.2275860 | ACCESS_DENIED/Win32 5 | UNKNOWN |
| 7936 | 17:40:23.2334890 | ACCESS_DENIED/Win32 5 | UNKNOWN |
| 18796 | 17:40:25.2045200 | ACCESS_DENIED/Win32 5 | UNKNOWN |
| 23564 | 17:40:23.9557730 | ACCESS_DENIED/Win32 5 | UNKNOWN |
| 28788 | 17:40:24.4783450 | ACCESS_DENIED/Win32 5 | UNKNOWN |
| 31460 | 17:40:23.8230130 | ACCESS_DENIED/Win32 5 | UNKNOWN |
| 34416 | 17:40:23.8143850 | ACCESS_DENIED/Win32 5 | UNKNOWN |
| 36008 | 17:40:24.4866550 | ACCESS_DENIED/Win32 5 | UNKNOWN |
| 41880 | 17:40:24.6165970 | ACCESS_DENIED/Win32 5 | UNKNOWN |
| 47064 | 17:40:24.6228470 | ACCESS_DENIED/Win32 5 | UNKNOWN |

최종 도구 실행 결과 `F:\study\codex\Project-Limitless\Temp\WorkerProcessGroupInvestigation20261027\48106246eb244358bcc2bec4bc6a42bf.json`, SHA `c82fd16d72130e0cffd0bab04abefc6d64431057d4946f5968aa861cb368c03a`. Evidence/WorkerInvestigation20261027/에 원본 bytes와 ProcessMatrix를 보존했습니다. 수정 중 첫 비관리자 결과04b19e43…는 Temp에 남겼으며, 소수초 변환 보완 후의 최종 판독 정본은48106246…입니다. 옛 Worker probe를 반복한 것이 아니라 새 좁은 도구만 실행했습니다.

Task Scheduler도 UnauthorizedAccessException/HRESULT -2147024891. 따라서 현재 GroupId 실제 값/SID/범주/Worker 멤버십·그룹 작업 실행 가능성은 **UNKNOWN**입니다. 이전 문자열 길이11로 특정 그룹을 추측하지 않았습니다. 빈 tasks/children 배열은 수집 불가/범위 제한의 결과일 수 있어 '존재0'이 아닙니다.

고정된 V2 lease·r9/r11 consume marker·r11 approval 경로의 메타데이터는 ABSENT였습니다. 내용·락은 읽거나 획득하지 않았고, 부모 chain은 검증하지 않았습니다. 기존 운영DB·전역 lease·모든 승인/소비 기록의 부재 증명이 아닙니다.

## 2. 새 도구의 범위와 실제 충돌 판별 한계

소스는 기존 V2 폴더 밖 `Tools/DiscordAIServiceDiagnostics/WorkerInvestigation20261027/`. r11에 배포하거나 기존 조회 함수를 교체하지 않았습니다.

- 원본 SHA와11개 scope, 현재 로컬 Worker SID를 먼저 확인합니다. 각 PID는 native GetProcessTimes의 생성시각을 원본 WMI의 마이크로초 정밀도로 비교합니다. 일치하지 않으면 PID_REUSED/세대 불일치로 HOLD하고 새 PID 객체의 상세를 과거 객체에 연결하지 않습니다. 1초 같은 임의 허용 오차는 없습니다. JSON parser가 DateTime으로 바꿔도 UTC roundtrip으로 소수초를 보존하며 암묵 문자열 변환을 사용하지 않습니다. 공급자 정밀도 불일치도 안전 판정으로 승격하지 않습니다.
- OpenProcessToken(TOKEN_QUERY=8), GetTokenInformation의 User/Groups/Statistics/RestrictedSids/SessionId로 실제 소유 SID·계정, authentication LUID·세션 ID·기본 토큰의 활성/deny-only 그룹을 읽습니다. 원시 토큰·privilege 목록·비밀은 저장하지 않습니다. Win32_LogonSession은 해당 LUID의 LogonId/LogonType만 조회하고 인증 패키지·암호를 읽지 않습니다. 스레드 impersonation 토큰 전체를 조사한 것은 아닙니다.
- QueryFullProcessImageName의 임시 경로는 파일 조사에만 쓰고 결과 JSON에는 내보내지 않습니다. 동일 읽기 file handle에서 SHA와 WinVerifyTrust를 연결하고 파일 ID/형식·위치 범주를 기록합니다. 알려진 실행파일명 외에는 이름도 해시로 정제합니다. 외부 서명 네트워크 조회·UI는 금지하고 cache-only 정책을 사용합니다. catalog 전체 검색/최신 온라인 폐기 검증은 하지 않아 signature 미검증을 악성/무서명으로 단정하지 않습니다. [WinTrust 정책](https://learn.microsoft.com/en-us/windows/win32/api/wintrust/ns-wintrust-wintrust_data).
- Codex 이름의 이미지가 있으면 CLI/AI 관련 **가능성 단서**이지 실제 모델 추론·요청의 증거가 아닙니다. Python/Node/PowerShell은 역할 불명 런타임으로 분류합니다. Windows 경로+서명이 맞는 알려진 이름도 정상 상주 **후보**일 뿐 제외 허가가 아닙니다. 모든 역할의 safe_to_exclude=false를 유지합니다.
- 부모는 인접 한 단계의 생성시각, 자식은 검증된 대상의 직접 자식만 최대64개까지 조회합니다. 부모가 자식보다 나중에 생성됐으면 부모 PID 재사용으로 판단하고 연결하지 않습니다. 그 외에도 세대 순서 호환일 뿐 실행 주체의 완전한 증명은 아닙니다. 전역 프로세스 스캔·보호 메모리·명령줄·환경 조회는 없습니다.
- IsProcessInJob은 어떤 Job에 속했는지만 알려줍니다. 타 Job handle을 열거나 job 한계를 변경하지 않아 특정 V2/기존 Job과의 동일성은 UNKNOWN입니다. Task EnginePID는 task engine PID이며 action PID/Job의 증명으로 바꾸지 않습니다. [공식 EnginePID](https://learn.microsoft.com/en-us/windows/win32/api/taskschd/nf-taskschd-irunningtask-get_enginepid).
- 작업 principal 메타데이터를 읽고 GROUP/Worker user/OnBoot2/조회불명 작업에만 action 실행파일 **종류** 및 GetInstances 메타데이터를 읽습니다. XML/Arguments/명령줄은 읽거나 기록하지 않습니다. 일반 Task 경로도 SHA로 식별하며 알려진 OnBoot 이름만 표기합니다. Run/Register/Terminate 호출0.

## 3. GroupPrincipal 조회 개선과 로그온 enum

기존 Get-LocalGroupMember는 로컬 SAM 그룹에 한정돼 비-SAM·도메인·잘 알려진 SID를 처리하지 못할 수 있습니다. 새 도구는 실제 COM GroupId를 먼저 기록하고 이름이면 [LookupAccountNameW](https://learn.microsoft.com/en-us/windows/win32/api/winbase/nf-winbase-lookupaccountnamew), SID이면 LookupAccountSidW로 OS 매핑을 확인합니다. SID_NAME_USE의 Group/Alias/WellKnownGroup과 authority를 구분합니다. 조회거부/없음/도메인 미연결은 실제 숫자 코드와 UNKNOWN/HOLD로 보존하고 다른 범주로 추정하지 않습니다.

| 문맥 | 값4의 의미 | 사용 목적 |
|---|---|---|
| Task Scheduler TASK_LOGON_TYPE | GROUP | GroupId 기반 activation |
| Windows SECURITY_LOGON_TYPE / Win32_LogonSession | BATCH | 해당 인증 LUID의 batch 로그온 |

[Task Scheduler enum](https://learn.microsoft.com/en-us/windows/win32/api/taskschd/ne-taskschd-task_logon_type), [Windows Security enum](https://learn.microsoft.com/en-us/windows/win32/api/ntsecapi/ne-ntsecapi-security_logon_type). 두 숫자를 서로 변환하거나 동일 의미로 쓰지 않습니다. UserId=null/GroupId 존재/Task type4는 이름 생성자에 null을 넘기지 않고 GROUP 경로로 처리합니다.

실제 소유 SID1007·PID/생성시각이 검증된 기본 토큰에서 SID의 SE_GROUP_ENABLED/USE_FOR_DENY_ONLY 속성으로 관측 소속을 판단합니다. restricted SID가 있으면 제한 문맥을 UNKNOWN으로 남깁니다. 중첩·도메인 소속도 토큰에 반영된 SID 관측을 근거로 삼고 SAM 이름만으로 멤버라고 하지 않습니다. [그룹 SID 속성](https://learn.microsoft.com/en-us/windows/win32/secauthz/sid-attributes-in-an-access-token).

관측 토큰에 그룹이 있어도 세션·로그온 유형·Task 조건까지 검증한 것이 아니므로 '실행 가능성'만 기록합니다. 토큰에 없거나 deny-only라고 미래 로그온 토큰에서도 절대 불가능하다고 결론내리지 않습니다. 실제 Task를 실행해 확인하는 것은 이번 금지 범위입니다.

## 4. 공유 Worker 정책 분석과 대안

기존 WMI 로그온 연관 개수는 실제 소유 SID/역할의 증명이 아니므로 정상 상주·다른 소유자 연관까지 과도하게 막을 **가능성**은 있습니다. 이번 실제 owner가 모두 거부돼 그런 사례가 있는지는 미확정입니다. 실제 owner·세대·이미지·서명·부모·토큰을 확보하면 진단 정확도는 개선할 수 있지만 r11의 zero gate를 이 도구가 변경하지는 않습니다.

순간적인 'AI 이미지 없음'·lease ABSENT·정상 서명은 비협조적인 기존 직접 실행기/예약 작업의 다음 실행을 막지 못합니다. 이름 allowlist로 상주 프로세스를 빼거나 개수만0으로 만들면 TOCTOU 경합이 남습니다. 서명된 런타임도 AI 자식을 시작할 수 있고, 같은 SID이면 권한·인증 범위를 공유하므로 원본 승인/소비/lease 일관성을 전역에서 강제하지 못합니다.

검토 대안(이번 미구현·미설치):

1. 모든 기존·신규 launcher가 하나의 신뢰된 broker/lease/승인 epoch·CREATE_NEW consume/Job·PID 생성시각 검증을 준수하는 구조. 기존 직접실행 경로까지 차단·조정하지 않으면 보장이 아닙니다. OnBoot/봇 변경 및 OS 실행통제는 이번 금지이므로 공유 구조의 강한 독점 보장은 미해결입니다.
2. **V2 전용 Worker SID/빈 독립 profile의 FAKE_ONLY 구조를 우선 설계 권고.** 기존 SID1007과 인증/CODEX_HOME은 보존하고 복사하지 않으며 fake 시험에 실제 AI 인증을 가져오지 않습니다. 실제 SID는 향후 OS가 발급해야 합니다. 현 r11은1007 계약이므로 새 계정으로 임의 실행할 수 없고 모든 구성·Manifest·승인·권한의 별도 후보 설계가 필요합니다. 계정·작업 생성은 이번0.
3. 별도 VM에서 네트워크/인증 없는 Session0 FAKE_ONLY 검증. 호스트와 분리는 강화되나 환경 준비와 별도 승인·검증 부담이 더 큽니다. SID 분리만으로 VM 수준 격리라고 주장하지 않습니다.

현재 결론은 **실제 AI 충돌 UNKNOWN / 독점 실행 보장 불가 / HOLD 유지**입니다.

## 5. 신규 검증·산출물 SHA

신규 오프라인56 PASS/0 FAIL. 가짜 프로세스·Task·그룹 SID·토큰으로 PID 재사용/소수초·타임존/소유자 불명·다른 SID/CLI 단서/정상 후보/서명 불명, GROUP/BATCH 분리, null user, 로컬·내장·well-known·도메인 그룹, 거부/미매핑/RPC 오류, deny-only·restricted, Job/EnginePID/lease 오판, 64개 자식 제한, 11개 보존, 비밀 sentinel 비출력을 확인했습니다. Native 권한마스크/금지API·PS AST도 검사했습니다. fake suite는 실제 프로세스/토큰/Task/서명 API를 호출하지 않았습니다. 실제 native 서명·토큰 그룹·GroupId 매핑 성공은 아직 미검증입니다.

증거 `문서/00_프로젝트/팀운영/Evidence/WorkerInvestigation20261027/`: OfflineTests, SourceSHA256, FinalProof, ProcessMatrix, 최종 실제 결과48106246… JSON. 원본 생성시각/개별 실패 코드를 보존하고 상상으로 역할을 채우지 않았습니다.

| 변경 파일 (신규4개) | SHA256 |
|---|---|
| WorkerReadOnlyNative.cs | 7919d951410e515d19834616c4f74a8f8efade78a7cc31a9d5d561304516ddf9 |
| InvestigationModel.ps1 | e96fa9aab959f857bc5aca1b15f6e16966e3bddedd271e30d4425b4f757ac6b2 |
| investigate-worker-readonly.ps1 | 955a1d717ba42da7a9439946da8970d7976bc0adeb395eb91fc6b717c655fd38 |
| test_investigation_offline.ps1 | 8d8e623f26085c575544b464e652c3b31b6a0dd0ac0413a3e13632b964bfcfe5 |

## 6. 관리자 추가 검사 한 줄

새 관리자 PowerShell7에서 아래 **새 전용 도구**만 실행합니다. 기존 preflight/Worker probe가 아닙니다. 현재 창을 종료하는 exit, SeDebug 활성화, 네트워크 AI 호출, 계정/Task/ACL 변경이 없습니다. 실패/불명은 HOLD이며 자동 재시도하지 않습니다.

```powershell
$b=[IO.File]::ReadAllBytes('F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\Tools\DiscordAIServiceDiagnostics\WorkerInvestigation20261027\investigate-worker-readonly.ps1'); if([Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($b)).ToLowerInvariant() -cne '955a1d717ba42da7a9439946da8970d7976bc0adeb395eb91fc6b717c655fd38'){throw 'INVESTIGATION_SHA_HOLD'}; & ([scriptblock]::Create([Text.Encoding]::UTF8.GetString($b)))
```

스크립트가 원본 JSON/Native/Model SHA도 확인한 뒤 memory load합니다. 결과는 `F:\study\codex\Project-Limitless\Temp\WorkerProcessGroupInvestigation20261027\<GUID>.json`에 CREATE_NEW로 저장합니다. 기본 최상위 상태는 진단용 HOLD이며 실제 데이터의 각 필드로 판독합니다. 이 도구는 성공한 조회가 있어도 설치·독점 실행 승인을 발급하지 않습니다. 관리자도 정보가 거부되면 우회하지 않고 UNKNOWN 보존입니다.

## 7. 원본 보호·중지

r11 Installer `eb321fc5ee0426cdd7e1c8f989f05a5e2ec55e6d1887789f0e16490598311bc2`, Manifest `e8d6ed623934a577f88a5bb99ebafc78a2a469c10485a0c648c40dd457356dc0`, ZIP `0250f736b44798d3ba73b5d9674fc154fa233bc74e3b972f85931c2aa59a41d0` 실제 SHA 불변 확인. 기존 V2 코드도 수정0. V1 참조HOLD/MANUAL/A·Worker/인증/CODEX_HOME/OnBoot2/봇아트/운영SQLite·Unity·게임·Save 보존. 프로세스 종료/재시작·계정/Task/ACL/LSA·staging/설치·IPC/FakeWorker/AI/usage/Discord·Push/main병합/재부팅0.

LOCAL_COMMITTED / REMOTE_NOT_PUSHED. 조사 코드와 오프라인 검증 완료, 관리자 추가 자료 전 HOLD로 중지합니다.

마감: 구현·증거 로컬 커밋07ef386bd150af8b2f81ae77edd35722e3870dc1. 실제 Issue66 보고 https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6433810712 . 신규56PASS/0FAIL, 실제11개ACCESS_DENIED/역할UNKNOWN·GroupPrincipal UNKNOWN·독점실행HOLD. r11불변/운영변경0, LOCAL_COMMITTED / REMOTE_NOT_PUSHED.

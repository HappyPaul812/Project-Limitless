# Issue66 환경 PASS 대조·승인된 사용량 단발 명령 — 2026-10-26

**ENVIRONMENT_PASS_CONFIRMED / LIVE_USAGE_COMMAND_READY / OWNER_LOCAL_EXECUTION_PENDING**.

바울 대표가 전달한 request `c332610ebdfa43f1b4b13b3dea5120d6` 결과를 실제 ProgramData summary.json과 대조했습니다. 관리자 parent Session9와 서비스 child `PAUL-PC\LIMITLESSRelaySvc` Session0/logon4를 구분했으며, task종료0·SID/CLI SHA 일치·CODEX_HOME보존·보호경로I/O·20초설정/재시작0·임시작업REMOVED·부팅/승인/장부 보존 모두 PASS입니다. 공식 사용량0·모델0이므로 기존 단발 승인은 아직 미소진입니다. 이 환경 PASS를 실제 AI 추론 성공으로 표현하지 않습니다.

## 최종 관리자 명령

이미 열린 **관리자 PowerShell**에서 다음 명령을 직접 실행합니다. 이는 환경확인이 아니라 **기존 승인 범위의 실제 사용량 최대1회**를 수행하는 명령입니다. 새 승인을 요구하는 단계가 아닙니다. 이전 보안 입력은 저장하지 않았으므로 서비스 계정 Windows 암호를 로컬 Get-Credential에 한 번 더 입력해야 합니다. 채팅/Discord로 암호를 전달하지 않습니다.

```powershell
pwsh -NoProfile -File "F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\Tools\DiscordAIServiceDiagnostics\run-approved-usage-once.ps1"
```

기존 일반 Codex 도구의 sandbox escalation은 관리자 토큰을 주지 않으므로, Codex가 현재 비관리자 프로세스에서 대신 실행하지 않습니다. 장치 정책을 Bypass하거나 PIN/자동로그인을 바꾸지 않습니다.

## 정확한 범위와 단발 차단

- 검토된46파일 묶음(기존40승인 보존·변경3+신규1·roles·고정usage-only entry)을 새 ProgramData/LimitlessRelay/UsageTrials/<nonce>/Package에만 배치합니다. 기존 운영경로/boot-approval/CLI·CODEX_HOME/봇·아트·OnBoot는 바꾸지 않습니다.
- bundle/manifest/helper/entry/기존CLI SHA를 고정 확인합니다. **새 배포 경로의 필수 SHA gate와 실제 entry의 보안 검사는 생략하지 않습니다.** 이것은 기존44 QA/22+12 전수시험을 다시 실행하는 것이 아닙니다.
- 실제 환경 PASS의 정제summary hash까지 확인하고, 현재 OnBoot/승인·readonly장부/MANUAL 및 단발marker/활성permit 부재를 확인합니다. 배포 gate가 실패하면 사용량0으로 HOLD합니다.
- 관리자 전용의 새 permit을 정확한 trial root·44허용항목·새request·5분 만료·1회20초·모델0·Discord0·SQLite업무0으로 만듭니다. 기존 승인 목록을 자동 확대하지 않습니다.
- `UsageOnly20261026.launch-consumed.json`을 CreateNew로 **Start 직전에 소비**합니다. 명령 재실행 시 새request를 발급하여 반복 조회하지 못합니다. 임시작업은 Password/Limited·IgnoreNew·RestartCount0·20초·AllowHardTerminate입니다. 자식도 동일request 실행marker를 CreateNew로 만듭니다. 실패 시 이 marker들을 삭제하지 않습니다.
- entry는 기존 `usage-only-once.py` SHA `f176f64145009894659daecdeaed5085e633d624eebfcb0db3a651e0aea48fd7` 그대로입니다. 기존 모델 runner를 배포·호출하지 않습니다.

## 20초와 프로세스 트리 정리

신규 `UsageOnlyJob.cs`는 CreateProcess(CREATE_SUSPENDED) → 새 Job 할당 → Resume 순서입니다. Assign 실패 시 Python을 실행하지 않고 종료합니다. KILL_ON_JOB_CLOSE를 설정하여 Task Scheduler가 부모 PowerShell을 종료하더라도 Python/app-server 자식은 같은 Job에서 정리됩니다. breakaway나 약한 Sandbox를 사용하지 않습니다.

worker의 실제 서비스 SID/Session0/logon4를 다시 확인하고, worker 경과시간을 뺀 잔여한도 중 최대17초만 Python에 주며 종료/로그용 여유를 둡니다. Scheduler는 worker 전체20초 한도입니다. helper는 timeout에 Job 전체를 종료하고 ActiveProcesses0 확인·bounded pipe drain을 합니다. stdout64KiB/stderr8KiB 메모리상한 뒤에도 계속 읽어 버려 교착을 막으며, 원시 stderr는 저장/출력하지 않고 제한된 진단 어휘로 정제한4096자 요약만 자기프로필에 저장합니다. 이는 OS hard realtime 보증이 아니라 native Job+Scheduler의 실제 시간제한/정리 수단입니다.

## 로그·실패·정리

서비스 자기 `.codex/limitless-usage-trials/<nonce>/`의 context/execution/failure 기록과 기존 `.codex/limitless-usage-diagnostics/`의 실제 RPC 기록을 보존합니다. RPC method/id/error.code/정제message/data·stderr·종료코드는 기존 보호로거가 담당합니다. 공용 summary에는 고정 상태/횟수·서비스 계정/세션·정리 결과·RPC method/id/숫자코드만 담고, 상세 메시지/stderr/원문SID/인증자료는 넣지 않습니다.

실행 후 성공/실패와 관계없이 활성permit이 해당nonce/작성SHA와 맞는지 확인해 trial의 `permit.consumed.disabled.json`으로 옮겨 고정 활성경로를 비웁니다. 소비marker는 유지합니다. 설명/nonce/명령이 맞는 이번 임시작업만 제거합니다. 실행중이면 해당 작업만 정지하여 Job handle 종료로 트리를 정리하며 HOLD합니다. 기존 OnBoot/봇/아트는 정지하지 않습니다. 다른 주체의permit·작업을 덮어쓰거나 지우지 않습니다.

새 ProgramData `UsageTrials/<nonce>/Output/summary.json`에 결과가 남습니다. 실제 API 요청 근거를 복구하지 못한 강제종료의 경우 호출횟수를0으로 추측하지 않고 **UNKNOWN_MAX1**로 취급하며 재시도하지 않습니다. entry의 usage_read_attempts는 provider.read 그룹 시도 수이며, 초기화/account/read에서 거부되면 실제 rateLimits RPC에 도달하지 않았을 수 있습니다. 실제 모델/Discord/SQLite업무는 이 경로에 없습니다.

## 이번 준비에서 수행한 새 검증

기존44/22+12 전체검증을 반복하지 않았습니다. 새 native Job만 가짜 Python으로 다음4건 PASS: 정상종료, 대량stdout/stderr 상한·drain, timeout의부모/자식트리 정리, **가짜 PowerShell 부모만 종료했을 때 Job handle close로 자식 둘도 종료**. 실제 Codex/네트워크/인증/사용량/모델 호출0입니다. helper는 가짜프로세스만 종료했으며 운영 프로세스는 건드리지 않았습니다.

새 launcher 구문 및 비관리자 무변경가드 PASS. 기존 보호함수는 실제 환경PASS에 사용한 restore-service-environment.ps1의 고정 SHA를 검사한 뒤 AST에서 정확한6개 함수 정의만 재사용합니다. 그 스크립트의 main/등록/실행 코드는 실행하지 않습니다. archive의 정확한46개 경로 집합·활성permit/전체단발marker 부재 확인 PASS. 지금까지 실제 trial 배포/사용량은0입니다.

### 소스/산출물 SHA256

| 대상 | SHA256 |
| --- | --- |
| run-approved-usage-once.ps1 | ea7a24d93315baadfab9a62ef37b5174a00695dfff1405fe595e00b8a43afe5a |
| UsageOnlyJob.cs | 4be51b7fdbabb681bf822c5d22cd624f64bcf73f483fd017575a6e81759fd155 |
| 환경 PASS summary | a2ec822433f2116e690e673bddac95a6b5cd1036775fc22d6e43b2999870bf44 |
| 46파일 ZIP | 395264ca39ff3f64afd97ef8c848a142bdc51a0c83a9b8b2fb3328170e8ee482 |

ZIP은 `Temp/DiscordUsageDiagnostics20261026/UsageTrialLaunch20261026/usage-package.zip` 로컬 준비물입니다. 인증·암호·토큰·Save를 포함하지 않습니다. 아직 ProgramData에 배포하지 않았습니다.

브랜치 codex/discord-usage-diagnostics-20261026. 환경PASS근거와새보호기준비만확정, **실조회결과는대표직접실행후확인**합니다. commit은Issue66/최종보고참조. LOCAL_COMMITTED / REMOTE_NOT_PUSHED. 실행결과 후에는 성공/실패무관 중지하며 실제AI/A는 별도승인입니다.

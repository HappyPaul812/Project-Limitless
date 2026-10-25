# Issue #66 공식 사용량 게이트 Denied 오프라인 진단 — 2026-10-26

상태: **DIAGNOSIS_COMPLETE / UNDERLYING_RPC_CAUSE_NOT_VERIFIED / AI_EXECUTION_HOLD / MANUAL**.

바울 대표 승인 범위에 따라 코드·보존된 로그만 조사했습니다. 실제 AI 호출0, 사용량 실조회0, 운영 설정/인증/서비스 변경0입니다. 이번 기록은 수정·실제 재시험 승인이 아닙니다.

## 확인된 직접 실패와 확인할 수 없는 원인

마지막 시험 결과 `Tools/DiscordAICollab/data/service-final-once-20261024/result.json`은 stage=`OFFICIAL_USAGE_AND_APPROVAL`, reason=`OFFICIAL_READ_REJECTED`, error_type=`Denied`, actual_ai_calls=0, exit_code=null, usage={}입니다. 모델 프로세스 종료코드가 아닙니다. `batch-summary.json`의 task_exit_code=2는 이 시험 실행기가 실패로 반환한 코드이며, 원시 HTTP/JSON-RPC 오류 코드가 아닙니다.

정확한 호출 경로:

1. `Tools/DiscordAIServiceDiagnostics/service-final-once.py:129~130`: stage 설정 → `asyncio.run(provider.read())`.
2. `Tools/DiscordAICollab/collab/usage_bridge.py:42~64`: `OfficialUsage.read()` → 내부 `rpc()` → 응답 id 일치 및 `error` 키 발견 → `Denied('OFFICIAL_READ_REJECTED')`.
3. runner `service-final-once.py:196~198`: 예외 문자열과 타입만 결과에 기록.

이로써 **공식 CLI app-server의 요청 id와 맞는 JSON 응답에 error 키가 있었음**은 확인됩니다. 이것은 자체 승인 거부 문구이며, 서버가 문자 그대로 Denied를 반환했다는 증거는 아닙니다. `error` 값·code/message/data와 method/id를 버리므로 아래 셋 중 어느 RPC에서 거부됐는지조차 현재 결과 파일로는 식별할 수 없습니다.

| RPC | 코드 위치 | 가능한 실패 |
| --- | --- | --- |
| initialize, id1 | usage_bridge.py:69 | 같은 OFFICIAL_READ_REJECTED |
| account/read, id2, refreshToken=false | :71 | 같은 OFFICIAL_READ_REJECTED |
| account/rateLimits/read, id3 | :75 | 같은 OFFICIAL_READ_REJECTED |

원시 오류 코드는 **NOT_RETAINED_IN_AVAILABLE_ATTEMPT_EVIDENCE**입니다. 401/403/429/Windows 권한/구독 소진/Session0 인증 실패 중 하나로 단정하지 않습니다. 앞 단계 `cli_info()`의 subscription_available=true는 `login status` 출력에 기반한 당시 로컬 인증 확인이지, usage RPC나 모델 실행 성공 증거가 아닙니다.

## 원시 로그가 없는 이유와 조사 한계

- `usage_bridge.py:48`에서 app-server stderr를 `DEVNULL`로 폐기합니다. stdout에서 읽은 event도 저장하지 않고 오류를 새 예외로 바꿉니다. app-server의 종료코드도 결과에 기록하지 않습니다.
- runner는 :95에서 private archive 폴더를 준비하지만, `save_execution_log()`는 모델 subprocess 실행 :154 이후 :157 또는 모델 timeout 분기 :190에서만 호출합니다. usage 단계 일반 예외에는 이 함수가 호출되지 않습니다.
- 따라서 마지막 시험의 `result.json`·`batch-summary.json`은 동일한 축약 정보이며, 해당 폴더에는 추론용 JSONL/stderr/last-message/preflight 로그가 없습니다. PowerShell child는 Python stdout을 `Out-Null`로 보냅니다(`run-service-final-once.ps1:57~58`).
- 서비스 `.codex/config.toml`, `.codex/log`, 해당 request의 `.codex/limitless-diagnostics/`는 현재 계정에서 `PermissionError / WinError5`로 접근 거부됐습니다. **이는 현재 조사자의 파일 접근 실패이며 원래 RPC 오류코드가 아닙니다.** ACL 변경/소유권 취득/인증 복사/DPAPI 복호화/서비스 계정 재실행을 하지 않았습니다. 보호된 기존 로그의 내용과 존재 여부를 확정하지 않습니다.
- 원문을 보존하지 않은 과거 `turn.failed`는 별도 모델 실행 장애입니다. 이번 사건은 모델 실행 전 차단이므로 같은 원인으로 묶지 않습니다.

## 성공 당시와 마지막 실패의 차이

| 항목 | 과거 조회 성공 기록 | 마지막 실패 / 현재 오프라인 실측 |
| --- | --- | --- |
| 근거 | data/boot/service-codex-auth-local.json 및 service-codex-auth-46bee54f62714b42838b595c731da1b2.json, 대응 service-ai-probe JSON | data/service-final-once-20261024/result.json, batch-context/summary |
| CLI 버전 문자열 | 0.164.0-alpha.11.1 | 동일 |
| CLI SHA256 | c6144bc19b2705f25a40d8488b3d57d17a223c5df3c5f65e3c58d72601956c1e | 4fd2f057bad603bf53dc202457ffd0c3591de62e867a8b01e73816e3fbce5a73 |
| 배포 | runtime/codex/c6144bc19b2705f2/codex.exe 단독 파일 | runtime/codex/official-0.164.0-alpha.11.1-x64/bin/codex.exe 공식 전체 패키지 |
| 서비스 CODEX_HOME | 서비스 자기 프로필 .codex | 같은 프로필 경로 기록 |
| 실행 문맥 | 서비스 인증/조회 성공. 모든 성공 기록의 Session0/logon4 메타데이터가 있는 것은 아님 | batch-context에서 Session0/logon4, 비대화형 배치 실행 확인 |
| 인증 | AUTH_AND_USAGE_VERIFIED, 구독 인증 및 정규화된 usage 기록 | 당시 login status의 subscription_available=true. 현재 실조회/재로그인0 |
| 사용량 RPC 결과 | READONLY_LINK_READY / CODEX_APP_SERVER_OFFICIAL | 원시 error 정보 유실 |

공식 패키지 전환 기록 `data/service-reference-switch/switch-984adccf569441ee9248ff0934af56e5.json`은 PASS입니다. 현재 `C:/ProgramData/LimitlessRelay/boot-approval.json`의 service_codex.path/SHA와 실제 exe SHA가 마지막 실패 값과 일치합니다. 기존 45파일/서명/--version/--help VERIFIED 검사를 반복하지 않았습니다.

환경 구성 코드의 차이도 확인했습니다. 일반 사용자 실행은 `phase2.py:41~47`의 허용 환경을 상속하고, 서비스는 `service_ai.py:32~50`에서 실행 토큰·고정 배포 위치를 확인한 뒤 USERPROFILE/HOME/CODEX_HOME을 서비스 자신의 프로필로 고정합니다. API/Discord 키와 proxy 변수는 CLI child 환경에 전달하지 않습니다. 과거 서비스 config는 keyring+forced chatgpt 두 줄 템플릿이었으나 **현재 config 내용·OS keyring 접근·실제 HTTP/TLS 원인**은 확인하지 못했습니다. 바이너리/세션/환경 차이는 조사 단서이며 원인 확정이 아닙니다. 임의 이전 exe 복귀나 인증 저장방식 전환을 권하지 않습니다.

## 신규 오프라인 분석

운영 모듈을 import/수정하지 않고 `OfficialUsage` 클래스 AST만 메모리에 로드했습니다. subprocess와 비동기 I/O·timeout을 모두 메모리 모형으로 바꾸고 synthetic error를 id1/2/3에 각각 제공했습니다. **3/3에서 원시 code/message가 보존되지 않고 동일 예외가 발생함**을 확인했습니다. 합성 오류의 -32603은 실제 사건 코드가 아닙니다. 실행 가능한 네트워크/CLI 프로세스는 생성하지 않았습니다.

분석 재현물은 로컬 `Temp/discord_denied_replay_20261026.py` 및 `Temp/discord_denied_offline_20261026.py`, 공개 요약은 `Evidence/DiscordDeniedDiagnosis20261026/OfflineDiagnosis_20261026.json`입니다. 최초 실제 asyncio 이벤트 루프를 사용한 진단 하네스가 이 실행환경에서 대기하여 해당 진단 세션만 종료하고, 이벤트 루프도 메모리 모형으로 치환했습니다. 운영 봇·서비스·기존 CLI/VERIFIED 테스트는 실행·중지하지 않았습니다. 이 하네스 현상을 원래 서비스 장애 원인으로 취급하지 않습니다.

## 보호 결과

- 운영 Python42개·단발 진입점/검증기·boot wrapper/approval·기존 OnBoot XML 사본·승인 CLI **52파일 SHA 동일**. 운영 코드/경로/설정 변경0. 실제 인증파일은 읽거나 복사하지 않았습니다.
- SQLite는 `mode=ro&immutable=1`로 읽었으며 WAL 부재를 확인했습니다. 파일 전체 bytes는 조사 중 변했으므로 불변이라고 주장하지 않습니다. tasks/approvals/operation_mode/settings/control_settings/operation_scopes/operation_checkpoints/user_worker_jobs **8개 핵심 테이블의 행 수·내용 digest 전후 동일**, MANUAL 유지. 이 진단은 DB 쓰기/Store 초기화를 하지 않았습니다.
- 부팅 작업2개를 실행·등록·수정하지 않았습니다. 현재 등록 상태 읽기는 schtasks의 network address 오류와 Get-ScheduledTask/Export-ScheduledTask의 CimJobException(HResult -2146233087)으로 **LIVE_NOT_VERIFIED**입니다. 과거 batch-summary의 boot_tasks_unchanged=true/approval_unchanged=true/temporary_task_removed=true와 기존 XML 증거는 보존했습니다. 그 과거 결과를 현재 작업 등록 성공으로 대체하지 않습니다.
- 기존 PIN 없는 봇·아트 실사용 VERIFIED는 유지하며 재시험하지 않았습니다. 이번 봇의 실제 Discord 온라인 상태까지 새로 검증했다는 뜻은 아닙니다.
- 원본 main/porcelain 작업상태는 조사 전후 동일. Unity·게임·Save·아트 변경0, Push/merge/PC 종료·재부팅0.

## 최소 복구안과 필요한 승인 — 제안만, 수정하지 않음

1. **먼저 로컬 진단 보존만 수정하는 별도 승인**: usage bridge의 실패 RPC method/id, error.code 및 제한·마스킹된 message/data, app-server stderr/종료코드를 동일 서비스 프로필의 제한된 파일에 보존하도록 설계합니다. 기존 Denied/HOLD·20초 timeout·RPC allowlist·인증·Sandbox·사용량/승인 게이트는 그대로 둡니다. stderr를 PIPE로 바꾸면 동시에 drain하여 교착을 방지하고 크기 제한·비밀 마스킹·로컬 ACL을 적용해야 합니다. 원시 auth/token/account dump를 공개하지 않습니다. 운영 배포 전 모의응답으로 진단보존만 검증합니다.
2. 접근 가능한 기존 서비스 로그가 필요하면, 별도 승인된 기존 관리자/서비스 계정의 **읽기만 하는 로컬 검사**로 비밀 제거된 관련 오류만 확보합니다. 현재 계정의 ACL을 넓히거나 인증을 복사하지 않습니다.
3. 근거가 계속 부족하면 **사용량 읽기 전용 단발 재시험을 별도 승인**받아 동일 CLI/프로필/문맥으로 최대1회·20초·응용 재시도0을 계획합니다. initialize/account/read/rateLimits만 허용, 모델/Discord 게시/모드 변경0, 기존 소비 슬롯 재사용0입니다. 이번에는 실행하지 않았습니다.
4. 그 오류를 해석해 최소 원인 수정안을 다시 검토합니다. 이후 실제 AI 최대1회/90초 및 Discord 결과 게시가 필요하면 **또 다른 명시적 승인**이 필요합니다. usage 실패를 건너뛰는 추론·A활성화는 금지합니다.

**추가 비용:** 이번 진단이 새 모델/API 사용을 발생시키지 않았습니다. 새 유료 API 전환0. 후속 사용량-only 시험은 모델 추론을 포함하지 않는 계획이며 실제 요금 결과를 미리 보증하지 않습니다. 후속 AI 시험은 기존 구독 한도를 사용할 수 있으므로 별도 확인·승인이 필요합니다.

**PIN 없는 무인 AI 가능성:** 부팅 봇/아트와 Session0 CLI bootstrap·과거 서비스 usage 성공은 가능성의 근거지만 Session0 실제 추론 완결은 여전히 NOT_VERIFIED입니다. PIN 제거·자동로그인 없이 달성 가능한지 이번 증거만으로 확정하지 않습니다. 로그인 후 Worker 예비안은 바울 대표의 미로그인 운영 목표를 충족하는 해결책으로 간주하지 않습니다.

공식 프로토콜 대조: [Codex App Server](https://learn.chatgpt.com/docs/app-server)는 응답 id와 result/error(code/message) 및 account/rateLimits/read를 설명합니다. 현재 공식 문서는 고정된 구버전 exe의 실제 실패 원인을 입증하지 않습니다.

담당 브랜치 `codex/discord-ai-collaboration`, 조사 시작 HEAD `9477b24d`에서 문서만 로컬 커밋합니다. **LOCAL_COMMITTED / REMOTE_NOT_PUSHED**. 최종 보고와 Issue #66에 전체 커밋 SHA를 기록합니다. 원인 확인을 위한 수정·실조회·실제 모델 재시험은 승인 전 진행하지 않고 여기서 중지합니다.

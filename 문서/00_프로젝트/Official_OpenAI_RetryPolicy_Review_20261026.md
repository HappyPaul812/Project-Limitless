# Issue66 공식 OpenAI 공급자 재시도 정책 검토 (2026-10-26)

**REVIEW_COMPLETE / PROPOSAL_ONLY / MANUAL / AI_EXECUTION_HOLD.**

기본 공식 OpenAI/ChatGPT 구독 경로를 유지한 채 내부 HTTP·스트림 재시도를 0으로 설정하는 공식 지원 방법은 확인하지 못했습니다. 승인 CLI 0.164.0-alpha.11.1의 기존 설정 거부 기록과 현재 공식 문서가 일치합니다. 사용자 정의 공급자·URL/인증 전환으로 우회하지 않습니다. 이번에는 문서·정적 검토만 완료했으며 실제 추론은 시작하지 않았습니다.

## 공식 지원 범위와 실패 위치

| 항목 | 확인 결과 |
| --- | --- |
| `model_provider="openai"` | 기본 공식 공급자 선택. 유지할 설정입니다. |
| `forced_login_method="chatgpt"` | ChatGPT 인증 방식 제한. 기존 서비스 CODEX_HOME의 공식 구독 인증을 유지합니다. 이번에 인증 조회·재설정은 하지 않았습니다. |
| `model_providers.<id>` | 사용자 정의 공급자 정의. 기본 ID `openai`는 예약되어 덮어쓸 수 없습니다. |
| `model_providers.openai.request_max_retries=0` | 해당 고정 CLI에서 bootstrap 거부. 공식 builtin 제어법으로 사용할 수 없습니다. |
| `model_providers.openai.stream_max_retries=0` | 위와 동일. 사용자 config·프로필·`-c` 위치만 옮겨 해결된다고 볼 근거가 없습니다. |
| 다른 이름의 custom provider | 일반 문서에는 정의 방법이 있으나 이번 지시에서 금지. 생성·제안 경로에서 제외합니다. |
| `openai_base_url`, `chatgpt_base_url` | URL 제어이며 retry0 설정이 아닙니다. 변경하지 않습니다. |
| 임의 환경변수·미문서화 top-level retry 키 | 고정 공식 builtin에 유효하다는 근거 없음. 무시되는 설정을 성공으로 취급하지 않습니다. |

근거: [공식 Configuration Reference](https://learn.chatgpt.com/docs/config-file/config-reference)의 `model_provider`, `model_providers.<id>`, `forced_login_method`, [Advanced Configuration](https://learn.chatgpt.com/docs/config-file/config-advanced)의 Custom model providers 및 per-provider tuning, [Sample Configuration](https://learn.chatgpt.com/docs/config-file/config-sample)의 provider 예시입니다. 문서의 HTTP retry 기본4·stream retry 기본5는 일반 provider 설정의 참고값입니다. **승인된 바이너리의 ChatGPT 경로에서 실제 적용되는 전체 요청 상한을 4·5만으로 확정하지 않습니다.** 별도의 builtin retry0 공식 키/옵션/환경변수는 이 자료에서 확인하지 못했습니다. 미래 버전 전체에 불가능하다고 단정하지 않습니다.

실패한 실제 적용 위치는 원본 저장소 `Temp/inference_config_probe_20261026.py`의 `controls` 목록입니다. 이 코드가 `features list`에 두 `-c` 값을 전달했습니다. 새 빈 Temp CODEX_HOME을 사용한 **설정 전용 이전 점검**이었으며 운영 `.codex/config.toml`·서비스 프로필·boot-approval·phase2.py를 수정하지 않았습니다. `features list` 실행은 설정 부트스트랩에서 종료코드1, `reserved built-in provider IDs: openai` 오류로 막혔습니다. 최신 실행기의 고칠 두 줄이 운영에 이미 배포되어 있는 상황이 아닙니다.

기존 증거: `Evidence/InferenceRetryPreflightHOLD20261026/Preflight.json`, 기반 commit `b5cd8d9a17de973f6faedfe3cd946926e4d7b6b6`. 이번에는 CLI 설정 검사조차 재실행하지 않고 기록을 재사용했습니다. 이전 임시 probe의 잘못된 두 줄도 증거 보존을 위해 그대로 두었습니다. 향후 실행 명령 구성에는 이 두 줄을 포함하지 않는 것이 제안입니다.

## 네 가지 정책은 별개입니다

| 구분 | 통제 대상과 제안 | 보장하지 않는 것 |
| --- | --- | --- |
| LIMITLESS 자동 재실행 | 새 격리 실행기에서 모델 CLI 프로세스 최대1회, app retry0·Scheduler RestartCount0. 실패·timeout·검증거부 후 새 subprocess/새request/같은 permit 재사용 금지. CreateNew 소비 마커 유지. | 단일 CLI 내부 네트워크 재시도0은 아님. |
| CLI HTTP 재시도 | 공식 builtin 기본 동작을 그대로 허용하는 대안. 대표의 별도 정책 승인 전에는 허용하지 않음. | 원격으로 전달되는 물리 HTTP 요청이 정확히1건이라는 보장 없음. |
| CLI 스트림 재연결 | 응답 전송 중 단절 시 CLI 내부 복구가 가능함을 별도 승인 대상으로 표시. | 단순 수신 재개인지 재요청/서버 작업 재개인지 이벤트만으로 확정 못할 수 있음. |
| 전체90초·강제 종료 | 새 작업의 단조 시계 절대 deadline에 사전검사·usage·CLI·정리 시간을 함께 배정. 하위 트리를 Windows Job에 묶고 timeout/부모종료 시 종료. | 서버 접수 후 원격 추론 취소, 원격 사용량0, 종료 직후 회계 확정은 보장 못함. |

CLI 프로세스1개, 완료된 JSONL turn1개, HTTP 요청1개, 서버 모델 실행1회는 서로 다른 관측값입니다. 정책 승인 후에도 `model_process_launches`, `completed_turns`, `application_retries`, 관측된 reconnect/retry 증거를 분리해 기록해야 합니다. transport 전체를 보지 못하면 `internal_retry_count=UNKNOWN`으로 남깁니다. 증거가 없다는 이유로 0이라고 쓰지 않습니다. 외부 출력에서 금지된 도구 항목이나 오류가 나오면 기존처럼 NOT_VERIFIED입니다.

## 기본 내부 재시도를 허용할 때의 위험과 사용량

아래는 문서와 로컬 구조에 따른 **위험 평가**이며 실제 모델 시험 결과가 아닙니다.

- 일시적 네트워크/서버 오류가 발생하면 같은 CLI 프로세스에서 추가 HTTP 요청이나 스트림 재연결이 생길 수 있습니다. 중단된 요청을 서버가 이미 처리했는지 클라이언트가 모를 수 있어, 로컬 프로세스1회만으로 원격 중복처리 가능성을 제거할 수 없습니다.
- 더 긴 처리·추가 요청은 ChatGPT 구독 한도를 더 소비할 가능성이 있습니다. 사용량/금액을 0 또는 고정치로 보장할 수 없습니다. API 키·유료 API로 전환하지 않는 정책을 유지하지만 **구독 한도 소비가 없다는 의미는 아닙니다.** 이번 검토에서 추가 모델 사용량은 발생시키지 않았습니다.
- 문서의 request4와 stream5를 더해 총9회 또는 곱해 고정 전체 상한이라고 계산하지 않습니다. 중첩·연결 방식·인증 복구·서버 처리의 실제 조합은 별도이며 고정 CLI의 실행 증거도 아직 없습니다.
- 90초가 끝나 로컬 트리를 종료해도 이미 전송된 요청을 회수할 수 없습니다. 원격 처리/사용량 반영이 나중에 끝날 수 있으므로 로컬 종료는 비용/원격 작업의 hard cap이 아닙니다. 단발 후 결과 확인을 이유로 모델을 다시 호출하지 않습니다.
- JSONL에 turn.failed/error가 없고 종료0·정확한 최종 응답·완료턴1을 확인해야 로컬 시험 성공입니다. 이것으로 모든 물리 요청 수까지 증명했다고 쓰지 않습니다. 최신 usage가 필요하면 이후 승인된 시험의 사전1회만 수행하고 사후 자동조회는 추가하지 않는 안을 검토합니다.

## 가장 안전한 최소 변경안 — 아직 미구현·미배포

엄격한 내부retry0을 계속 요구하면 **HOLD 유지**가 현재 가능한 안전한 선택입니다. 공식 builtin에서 지원되는 제어법이 확인되기 전까지 실행하지 않습니다.

단발 연결 검증을 진행하려면 대표가 다음 정책 차이를 먼저 승인하는 안을 제안합니다: **“LIMITLESS CLI 프로세스 자동 재실행0·완료턴 최대1·전체90초, 공식 builtin 내부 HTTP/stream 재시도는 기본값 허용; 원격 요청/사용량의 정확한1회 보장은 하지 않음.”** 이는 기존 승인 문구를 자동 확대하는 것이 아니며 아직 승인되지 않았습니다.

그 승인 후 필요한 최소 구현 범위는 다음과 같습니다.

1. 기존 `probe_command`의 기본 OpenAI/ChatGPT·read-only·approval never·도구/웹/MCP 차단을 재사용하고, 실패한 `model_providers.openai.*` 두 override를 새 단발 명령에 추가하지 않습니다. 기존 서비스 인증·CODEX_HOME·CLI 패키지·운영 승인 SHA를 바꾸지 않습니다. 단순히 기존 config를 수정할 필요가 없습니다.
2. CLI 내부retry 허용 정책과 app retry0을 **새 단발 permit/메타데이터**에서 별도로 명시하고, 과거 문서예약 request `e0d2e6fb9a6e48708b35052c02ad133c`의 미소비 상태를 유지합니다. 대표의 정책 승인만으로 시험을 자동 시작하거나 기존 소비 마커를 지우지 않습니다.
3. 현재 usage-only `UsageOnlyJob.cs`는 입력 시간을 **최대20000ms**로 거부하므로 그대로90초에 재사용할 수 없습니다. 새로운 격리 variant에 90초 이내 절대예산·kill-on-close·제한 stdout/stderr·종료 확인을 적용하고 변경분만 오프라인 검증하는 별도 단계가 필요합니다. 기존20초 helper·운영 프로그램은 수정하지 않습니다.
4. 새 임시 작업은 Password/Limited, Session0/logon4, Scheduler90초·RestartCount0·hard termination 허용을 검증해야 합니다. Python/CLI는 남은 시간에서 종료 여유를 뺀 예산만 사용합니다. 시간이 부족하면 CLI 시작 전 HOLD. cleanup에는 유한한 여유시간이 필요하므로 **90초 실행예산과 최종 정리/보고 지연**은 따로 기록하며, 현재 수단으로 벽시계90.000초 내 모든 정리 완료를 보장한다고 쓰지 않습니다.
5. 옛 `run-service-final-once.ps1`은 task6분/DisallowHardTerminate이고 Python `subprocess.run(timeout=90)`만 사용하며 Discord/SQLite 경로도 있습니다. 그대로 실행하지 않습니다. 새 격리 entry에서는 고정 사용자 prompt만 전달하고 Discord/SQLite 업무경로를 포함하지 않으며, 기존 JSONL·최종응답 대조·보호 진단·permit 비활성·임시작업 제거·프로세스0 확인을 유지해야 합니다. 실패는 NOT_VERIFIED·재시도0입니다.

위 항목은 검토안입니다. 이번에 runner/Job/permit 코드를 작성하거나 배포하지 않았습니다. 기존 generic provider 정의를 고치거나 공식 CLI를 다른 버전으로 교체하는 안도 포함하지 않습니다.

## 이번 검증과 보호

- 기존 문서/설정오류/정적 코드/Job4·22+12 PASS 기록 재사용. 이전 테스트·CLI probe 재실행0. 이번 검토를 새 추론·서비스 환경 PASS로 세지 않습니다.
- 모델 추론0, 공식 사용량 실조회0, CLI 실행0, 신규 승인 슬롯 실행·소비0, permit 발급0, ProgramData 배포0.
- runtime/운영 config/CLI/인증/ACL/봇·아트/OnBoot/SQLite/Unity/게임/Save 변경0. MANUAL 및 AI_EXECUTION_HOLD 유지. 최신 프로세스 건강/운영 DB 상태를 새로 실측한 것으로 주장하지 않습니다.
- 코드 무수정·문서/인계/검토근거만 로컬 commit. Push/main병합/PC종료·재부팅0. `Review.json`은 근거파일 SHA와 변경0 카운터를 기록합니다.

브랜치 `codex/discord-usage-diagnostics-20261026`, 기반 `b5cd8d9a17de973f6faedfe3cd946926e4d7b6b6`. 최종 docs-only 로컬 commit은 Issue66/최종 보고 참조. **LOCAL_COMMITTED / REMOTE_NOT_PUSHED / POLICY_APPROVAL_PENDING. 검토 후 중지합니다.**

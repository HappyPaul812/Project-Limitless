# Issue66 Session0 실제 AI 단발 승인 — 추론 전 HOLD (2026-10-26)

**NOT_VERIFIED / AUTHORIZED_NOT_STARTED / RETRY_POLICY_HOLD / MANUAL 유지.**

바울 대표의 이번 승인(고정 한국어 요청·모델 최대1회·90초·자동재시도0)을 확인했습니다. 이번 실제 추론은 실행하지 않았습니다. 최신 공식 사용량 조회도 하지 않았으며 승인된 모델 슬롯은 소비되지 않았습니다.

## 직접 확인한 차단 조건

기존 `collab/phase2.py`의 `verify_cli_controls`는 `BUILTIN_BOUNDED_DEFAULT; NO_APPLICATION_RETRY`를 명시합니다. 따라서 응용 실행 재시도0과 CLI 내부 HTTP/stream 재시도0을 같은 것으로 보고하지 않았습니다.

승인된 공식 CLI를 **인증 없는 새 Temp CODEX_HOME**에서 `features list` 설정 전용 명령으로 점검했습니다. `forced_login_method="chatgpt"`, `model_provider="openai"`, 기존 도구 차단 및 아래 두 설정을 전달했습니다.

```toml
model_providers.openai.request_max_retries = 0
model_providers.openai.stream_max_retries = 0
```

결과: **exit1**, `model_providers contains reserved built-in provider IDs: openai. Built-in providers cannot be overridden.` bootstrap 설정 단계에서 거부됐습니다. 앞선 help/option 점검에서 features list의 `--strict-config`는 exit2로 거부되어 제거했으며, 수정한 설정 전용 점검에서 위 별도 원인을 확인했습니다. 두 점검 모두 모델/사용량 호출0입니다.

공식 [Configuration Reference](https://learn.chatgpt.com/docs/config-file/config-reference)는 provider별 request/stream 재시도 키를 설명합니다. **해당 버전의 기본 openai provider에 적용 가능하다는 증거는 아니며**, 실제 고정 CLI가 그 override를 거부했습니다. 지원되는 다른 기본-provider 재시도0 제어법을 확인하지 못했습니다. 다른 provider 생성·URL/인증 변경·보안 우회·기본 재시도 허용으로 조건을 임의 변경하지 않았습니다. CLI가 절대로 재시도0을 지원하지 않는다고 단정한 것은 아닙니다.

## 실행·검증 결과

- 이번 서비스 실행 **없음**. 실제 agent 비관리자 Session9; 목표 PAUL-PC\LIMITLESSRelaySvc / Session0 / logon4는 이전 usage-only 성공 기록이며 이번에 새로 검증한 것으로 세지 않습니다.
- 신규 요청 ID **`e0d2e6fb9a6e48708b35052c02ad133c`**는 격리 문서에서만 예약했습니다. ProgramData trial/permit/launch marker/임시 작업 등록0. 이전 소비 마커2개는 그대로입니다.
- 실제 모델0 / 공식 사용량 실조회0 / Discord 게시0 / SQLite 업무처리0 / 자동재시도0. JSONL/완료턴/최종답변/추론토큰은 **실행 전 차단으로 NOT_RUN**입니다.
- 모델요청은 사용자 원문 그대로 위 증거 JSON에 기록했으며 prompt SHA `6f25b6b4de8fce640b6b8d3a6567cc90120b20640428946b400b202657e1937d`. 성공 응답을 모의 생성하거나 실제 성공으로 보고하지 않았습니다.
- CLI SHA **`4fd2f057bad603bf53dc202457ffd0c3591de62e867a8b01e73816e3fbce5a73`**, 승인 참조일치. 기존 공식 전체 패키지45파일 SHA 불일치0. 운영 runtime43항목 SHA 불일치0; 승인목록 확대0.
- boot-approval SHA **`3e032ebcce3d91bd762a8dec81048d04607ddde761afd62489c0fa2b8e49140f`** 보존, 기존 활성 usage permit 없음, 과거 마커 SHA는 `Preflight.json` 참조.
- 봇/아트/OnBoot/장부/MANUAL/운영 코드·설정·CLI/CODEX_HOME/ACL/인증/Unity/게임/Save를 변경하거나 재시작하지 않았습니다. **이번 새 실행이 없으므로 부팅 작업·DB·프로세스 건강 상태를 새로 실측해 PASS했다고 주장하지 않습니다.** 비밀번호·토큰·인증파일을 열거나 복사하지 않았습니다.
- 기존22/12/44/Job4/서비스 환경 PASS 전체 재실행0. 이번 무결성 확인은 고정 실행 파일·기존 승인 runtime과 package만 읽었습니다.

## 다음 진행 조건

우선 승인된 공식 CLI의 기본 ChatGPT 경로에서 내부 전송 재시도0을 지원하는지 확인하거나, 바울 대표가 요구하신 자동재시도 범위를 명확히 결정해야 합니다. 사용자 조건을 임의로 완화하지 않습니다. 현재는 **실행 명령을 제공하거나 구 runner를 실행하면 안 됩니다.** 구 `run-service-final-once.ps1`은 Discord/SQLite 경로가 있어 이번 시험에 그대로 재사용할 수도 없습니다.

이 제약을 해소한 다음에만 기존 성공한 Password/Limited·Session0/logon4 절차를 새 격리 진입점·90초 Job 감독·동일 고정 prompt·도구 차단·JSONL 검증·보호 로그·새 permit/소비 마커에 연결할 수 있습니다. 관리자 권한과 로컬 암호 입력은 바울 대표가 직접 수행하며 다시 승인받은 것으로 가정하지 않습니다. 현재 요청은 미시작/미소비입니다. 실제 추론·A/모바일 자동연결은 미검증이며 A는 여전히 미승인입니다.

## 기록

브랜치 `codex/discord-usage-diagnostics-20261026`, 기반 `862b860090add1bca365100da4c1e0aee5bf0440`. 이번 변경은 docs/evidence만이며 로컬 commit SHA는 Issue66/최종보고에 기록합니다. `LOCAL_COMMITTED / REMOTE_NOT_PUSHED`. 증거 `Evidence/InferenceRetryPreflightHOLD20261026/Preflight.json`.

**추론 실패가 아니라 추론 전 선행조건 HOLD입니다. 실제 AI 단발을 시작하지 않고 중지합니다.**

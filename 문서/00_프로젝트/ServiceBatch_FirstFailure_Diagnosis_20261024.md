# 서비스 배치 첫 추론 실패 — 원인 미확정 / 추가 AI 0회

## 판정

**NOT_VERIFIED 유지.** 실행 request `06f42ed466a8466f89633255c81abeeb`, 2026-10-24 14:41 KST. CLI 종료1, 배치 작업 종료2, 실행 시도1, 소요15.812초, 완료 턴0. 토큰 사용량은 `{}`로 미확인이지 0토큰 확정이 아니다. Discord 상태는 `NOT_SENT`: 검증 실패 후 게시 코드에 진입하지 않았으므로 Discord 인증/네트워크 실패라는 근거는 없다.

정본 근거: `Tools/DiscordAICollab/data/service-batch-once-20261024/batch-summary.json`, `result.json`, `batch-context.json`, `schema.json`, `attempt-consumed`. 해당 폴더와 empty 하위에 원본 JSONL/stderr/launch manifest는 없었다. 별도 data/ai-ready replay3개는 과거 합성 fixture이며 이번 실행 원본이 아니다.

## 첫 오류와 10개 error 이벤트

첫 실제 오류 원문·코드: **복구하지 못함**. 10개 각각의 원인 분류도 **UNKNOWN 10개**다. `EXIT_NONZERO`는 검증기의 종료코드 판정이며 첫 서버 오류 코드가 아니다. `turn.failed`1개의 직접 메시지도 저장되지 않아 미확정이다. error10을 모델 호출10회나 도구 요청10회로 해석하지 않는다.

원인: 첫 실행기 구현293db5d6은 `capture_output=True`로 받은 stdout/stderr를 메모리에서만 검사하고 집계만 result에 기록했다. stderr를 파일로 저장하지 않았고 최종 응답 파일도 finally에서 삭제했다. `--ephemeral`도 사용했다. 원문 보존 없이 집계만 남긴 구현은 진단 근거 손실이라는 **확인된 결함**이다. 이 결함을 CLI 실패 자체의 원인으로 오인하지 않는다.

서비스 `.codex` 목록 조회는 일반 실행과 sandbox 밖 실행 모두 Windows UnauthorizedAccessException이었다. sandbox 밖 실행은 Windows 관리자 승격을 뜻하지 않는다. ACL·계정 권한·인증 저장소를 변경하거나 다른 사용자 비밀을 열지 않았다. 기존 서비스 로그가 실제 존재하는지는 현재 계정에서 확인 불가다.

## FORBIDDEN_ITEM의 정확한 의미

`output_parser.py`의 허용 item type은 `reasoning`, `agent_message` 두 종류다. `item.started/updated/completed` 안의 item type이 이 밖이면 모두 `FORBIDDEN_ITEM`으로 거부하고 기존 집계에는 `forbidden`으로만 기록했다. 따라서 이번 값2는 **허용 목록 밖 item 이벤트2개**이며 실제 셸/웹/MCP 호출2회라는 뜻이 아니다. 상위 event type이 `item.completed`2개라는 사실만 남아 있고 내부 item type과 원문, 요청한 도구·권한은 미확인이다. 경고·오류 같은 항목도 같은 분류에 들어갈 수 있지만 이번 두 개가 그러했다는 증거는 없다. 허용 목록과 거부 판정은 변경하지 않았다.

## 계층별 확인 범위

| 계층 | 확인한 근거 | 판정 |
| --- | --- | --- |
| Windows 계정/배치 | PAUL-PC\\LIMITLESSRelaySvc, Batch 로그온4, Session0, 비대화형 | PASS |
| Codex 구독 인증 | 직전 login status subscription_available=true, CLI0.164.0-alpha.11.1 | PASS, 추론 요청 인증 성공까지 뜻하지 않음 |
| 공식 사용량 | App Server source, 5시간10%/주간17%, ordinary_allowed=true | 조회 PASS, 추론 endpoint 성공은 미확인 |
| GitHub | docs/planning d73bdbf9, 단일 PLANNING.md SHA ad23c0ca… | 문서 fetch PASS |
| 모델 | 명시적 --model 없음, CLI 기본값, 실제 해석된 모델 미기록 | 사용 가능 여부 UNKNOWN |
| 네트워크 | GitHub·App Server 조회 성공, inference HTTP/TLS 오류 미저장 | 추론 통신 UNKNOWN |
| 파일 접근 | 서비스가 schema/result/marker를 생성, 현재 schema 소유자 서비스·서비스 ReadAndExecute ACL | 서비스 상위 프로세스 쓰기 근거, CLI/Sandbox 내부 읽기 성공은 별도 미확인 |
| 검증기 | 종료1·error10·forbidden2·완료0·최종응답없음을 거부 | 거부 적정, CLI 실패를 검증기 오탐이라고 확정 불가 |

서비스 CODEX_HOME은 `C:\Users\LIMITLESSRelaySvc\.codex`. 부모 작업 폴더는 Tools/DiscordAICollab, CLI `-C`는 `data/service-batch-once-20261024/empty`. 원본 argv는 저장되지 않았으나 같은 commit의 생성 함수로 `exec --json --ephemeral --ignore-user-config --sandbox read-only --skip-git-repo-check`와 `approval_policy=never`, ChatGPT/OpenAI 강제, shell/apps/browser/computer/multi_agent 비활성, MCP/plugins/hooks 비우기, web_search disabled, project_doc_max_bytes0을 재구성할 수 있다. 최초 실행 argv를 직접 보존한 증거와는 구분한다.

로그인/사용량은 기존 사용자 설정을 사용하는 반면 exec는 `--ignore-user-config`를 사용한다. 인증 저장방식·모델 기본값에 미치는 실제 차이는 이번 원문이 없으므로 미확정이다. 이를 근거로 설정을 임의 변경하지 않았다. 서비스 config/keyring 내용을 읽거나 복사하지 않았다.

## 최소 수정 및 추가 호출 없는 검증

- 단발 실패 경로에 `launch-manifest.json`과 `failure-diagnostics.json`, `failure-projection.jsonl` 기록을 추가했다. manifest는 내부 고정 argv/환경변수 이름만, 진단은 알려진 오류 코드·고정 원인 분류·item type만 보존한다. 인증·명령 인수·도구 응답·추론·원문 메시지는 저장하지 않는다. 실패 projection은 내용이 제거된 파생물이며 원본 JSONL이 아니다.
- Sandbox/금지 도구/JSONL/최종응답 검증·90초·1회 제한·인증·provider 설정은 변경0. 소비 마커와 DB CONSUMED 슬롯 유지. 자동재시도·새 승인 발급·실제 호출·사용량 재조회·Discord 게시0.
- unittest `tests.test_failure_evidence tests.test_output_parser`: **30 PASS**. 10개 합성 오류 분류, 자유문구 제거, 알려진 item type 기록, projection의 거부 이유 동일성, 금지 도구·중복/손상 JSON·최종응답 검증을 확인했다. 이번 원본 replay는 **UNAVAILABLE**이며 통과했다고 주장하지 않는다.
- 기존 보관 replay3개: single_final PASS, commentary_then_final PASS, forbidden_tool NOT_VERIFIED/FORBIDDEN_ITEM. 과거 합성 fixture 재생이다.
- 새 오프라인 로그 검사 `python -m collab.offline_batch_diagnosis`: 현재 계정에서 SERVICE_LOG_ACCESS_DENIED_NO_ACL_CHANGE. 원본 실패 시각 UTC05:40:50~05:41:46의 기존 `.codex/log/*.log`만 검사하며 고정 분류만 로컬 기록한다. subprocess/CLI/네트워크/자격 증명/예약 작업 호출이 없다.

## 보호 및 다음 단계

첫 배치 summary의 OnBoot2 SHA 전후 동일·임시 작업 제거=true를 확인했다. 이번 세션에서 현재 예약 작업 XML 읽기는 권한상 불가하므로 새 XML 대조 PASS로 승격하지 않는다. Bot/Art 상태 파일은 모두 RUNNING이고 조사 중 heartbeat 증가, ai_worker_enabled=false/ai_calls0 유지. 관련 Bot/Art/OnBoot 실행 파일은 수정하지 않았다. 운영DB MANUAL, 승인 슬롯 CONSUMED 유지.

**다음 실제 재시험은 아직 준비 완료가 아니다.** 우선 관리자 또는 기존 서비스 계정에서 위 오프라인 검사만 실행해 남아 있는 동시간대 로그를 확인해야 한다. 원문이 남지 않았다면 첫 실패 직접 원인은 끝내 UNKNOWN이며, 새 진단이 붙은 별도 1회 시험은 바울 대표의 새로운 승인 이후 별도 슬롯/폴더로 준비해야 한다. 기존 marker/슬롯을 삭제하거나 이번 승인으로 재호출하지 않는다. 추가 진단만으로 실제 추론 성공을 증명하지 않는다.

브랜치 codex/discord-ai-collaboration. 선행 구현293db5d6. 이번 진단 수정 commit은 최종 보고 SHA를 따른다. GitHub Push/main 병합·A 활성화·게임/Unity/아트·PC 종료/재부팅0.

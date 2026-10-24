# PlanningAgent 사용자 Worker 실전1회 준비 — 승인 대기 / 실제 AI0

## 사전 점검

2026-10-24 16:19:44 KST 조회 기준, 실제 일반 사용자 PAUL-PC\windows / Session8 / 자기 프로필 C:\Users\windows / CODEX_HOME C:\Users\windows\.codex에서 공식 CLI 구독 인증 PASS·App Server 공식 사용량 조회 PASS. CLI0.164.0-alpha.11.1, 5시간68%/주간26% 사용, ordinary_allowed=true. 버전/login status·initialize/initialized/account/read(refreshToken=false)/account/rateLimits/read만 사용했고 exec·모델 turn·로그인 시작·API fallback은 없었다. 원문 로그인 출력/토큰/메일은 기록하지 않았다.

기존 등록 내용51개 모두 SHA 동일. 기존 봇·아트는 RUNNING/heartbeat 증가이고 bot-service.lock 및 service.lock이 실제 잠금 유지(LOCK_HELD) 상태다. 중복 봇·감시·Worker 프로세스를 시작하지 않았다. 운영 SQLite 연결·MANUAL·Worker claim0 확인. 조회 사용량은 실행 시점까지 보장하지 않으므로 실제 실행 직전에 다시 조회해야 한다.

## 신규 슬롯1건

- ID: USER_WORKER_PLANNING_READONLY_20261024_01
- 역할: PlanningAgent
- 상태: WAITING_FOR_OWNER_APPROVAL, expires0, 실행 승인false
- 계획 SHA: 42b0861a9b9dc5920d6afcc4654c2d28c13d00dad89d797446eaa095b7e5cb10
- 계약 SHA(정규화 JSON): 8fc083d9f1a02d71af9805fdefa143165b4fe3bac81f939accf132dc71af7813
- contract.json 실제 파일 SHA: a77ef622854a60525e537a83e8b513961e913d3ed42bcf4247afc0b2c8a06500

계획/계약의 정규화 SHA는 기존 real_roles.digest 방식(sort_keys·UTF-8·ensure_ascii=false)의 값이다. 보기 좋게 들여쓴 계약 파일의 raw SHA와 구분한다. 검토 정본은 Tools/DiscordAIUserWorker/trial-review.json, 실제 계획/계약/사전점검은 Tools/DiscordAICollab/data/user-worker-trial의 plan.json/contract.json/preflight.json이다.

settings에 새 WAITING 항목1개, user_worker_jobs에 같은 계획의 WAITING 업무1개, 전용 체크포인트1개를 등록했다. 기존 CONSUMED12개는 모두 그대로이며 재사용/삭제/승인/자동 갱신하지 않았다. 준비 중 계약 보강은 동일 미승인 슬롯 안에서만 반영했으며 최종 계획은 위 SHA1개다. 기존 tasks 업무를 추가하지 않았고 운영 outbox는 기존10건 그대로다.

## 문서 정본과 시험 계약

GitHub docs/planning 최신 commit d73bdbf9a3162132b959f4672e88c3975b361c3c의 `문서/00_프로젝트/팀운영/담당/PLANNING.md`1개만 입력한다. 역할 문서 원본 바이트 SHA: ad23c0caeb18122a0053a24fe3ad540f694e20b69092c82948f79ff80e138469. 직전 점검의 역할 정본과 같았다. main ref도 기준 메타데이터로 고정하며 추가 문서를 읽지는 않는다. 정확한 두 ref는 plan.json의 source_refs에 있다.

한국어 존댓말로 담당 역할을1~5문장 요약한다. 문장마다 한 줄·마침표1개, summary600자 이내, 기존 JSON 필드 role/summary/next_review만 사용하고 next_review는 바울 대표로 고정한다. 파일 변경/도구 호출/추가 업무/후속 AI/미술 승인/유료 API fallback을 금지한다. 공식 ChatGPT 구독·1회·90초·자동재시도0. 기본 VERIFIED JSONL/최종응답 검증을 통과하고 위 계약도 통과한 경우에만 기존 봇의 outbox에 REAL AI VERIFIED를 게시한다. 실패 시 VERIFIED 게시0·기존 HOLD 보고만 허용한다.

## 승인 전 공개하는 코드 차이 — 두 후보 모두 미배포

기존12슬롯만 허용하는 현재 실행기를 바꾸지 않고 후보 파일/diff를 별도 보관했다. 현재 부팅 승인 목록·별도 Worker 실행 허가·승인된 실행 소스51개는 변경하지 않았다.

| 대상 | 현재 SHA | 후보 SHA |
| --- | --- | --- |
| collab/real_roles.py | 8bf4738d98883c2beedb476eba1faa591de621caf5f021ae451bdbd7493982ad | bd133ff0792a0932006e1a87b66499b1efe788d4447b06905232a2b34dda6ee3 |
| DiscordAIUserWorker/durable_queue.py | e41a0d881f1c4e33c742421af832634721089be958f4f1dd5b9bd10c8c4b4ccc | 1614f510abcdad909b25b69780c99f9536b3ff49bf24d40cdeb64661877cb8b5 |

후보와 정확한 unified diff는 Tools/DiscordAIUserWorker/candidates/real_roles.py·real_roles.diff, durable_queue.py·durable_queue.diff에 있다.

real_roles 후보 변경은4곳이다: 새 Planning 단발 상수/허용목록1개 추가, 정확한 WAITING 계획만 별도 승인하는 함수, 기존 검증 PASS 후5문장 계약 검사, 해당 슬롯의 자동 인계 제외. 기존12개 슬롯 이름/소비 기록·기존 JSONL 검증기·Sandbox·도구 금지는 유지한다. 승인 함수는 준비 단계에서 운영 DB에 호출하지 않았다.

Worker 후보 변경은2곳이다: 계획의 auto_handoff=false이면 추가 인계하지 않음, publish_verified=true로 승인될 계획의 PASS 결과만 기존 outbox에 넣음. 직접 Discord 연결·새 봇 실행·후속 AI 승인 생성은 없다. 해당 계획에는 auto_handoff=false/publish_verified=true가 SHA로 고정돼 있다. 현재 배포된 Worker에는 이 후보가 반영되지 않았으므로 지금 실행 가능하다고 보고하지 않는다.

## 검증과 보호

미배포 후보를 격리 SQLite/모의 CLI에만 연결한 신규6개 테스트 PASS. 승인 전 CLI 호출0, 새 슬롯만1회 소비/12기존소비 보존,6문장 응답 거부, 잘못된 승인자/변경된 계획 거부,추가 인계0/PASS만 게시 대기,실패VERIFIED차단을 확인했다. 테스트의 모의 REAL_CLI_READONLY 이벤트는 실제 AI VERIFIED가 아니며 운영 DB/Discord에 보내지 않았다.

실제 AI0·현재 신규 승인0·기존 소비 재사용0·Worker 자동시작0·A 활성화0. 기존 부팅 작업/승인 목록/실행 허가/게임/아트/Save/Unity/Push/merge/PC종료·재부팅 변경0. 봇·아트 잠금과 heartbeat 보존, MANUAL 유지. 실제 모델 실행에는 별도의 실행 승인과 그 직전 공식 사용량 재검증이 필요하다.

## 남은 승인 조건

준비 상태는 **READY_FOR_OWNER_REVIEW_NOT_EXECUTABLE**이다. 바울 대표가 위 신규 슬롯·정확한 계획SHA·계약SHA·두 후보 코드SHA를 검토하고 명시적으로 승인해야 한다. 이후에만 정확한 후보를 반영하고 관련 무결성 SHA를 갱신하며, 이 시험에 한정된 실행 허가와 유효한 미소비 승인으로 전환한다. 단순한 SHA 내용 등록이나 이번 준비 요청을 실행 승인으로 해석하지 않는다.

승인 전 정본/계약/후보 코드/CLI SHA가 달라지면 차이를 보고하고 HOLD한다. 실제 호출 직전 동일 사용자 SID/자기 프로필·원본 로그·공식 인증·5시간/주간usage·정본·예약·중복claim·승인 만료를 다시 확인한다. 승인된 경우에도1회/90초/재시도0으로 종료하며 MANUAL 유지·추가 업무0이다.

이번 단계는 슬롯 준비와 읽기 점검에서 중지한다. 브랜치 codex/discord-ai-collaboration, 선행 commit cd1b3327. 이번 commit SHA는 최종 보고를 따른다.

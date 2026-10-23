# Discord 실제 AI 연결 준비 — WAITING_FOR_OWNER_APPROVAL

## 실제 확인과 미검증

기존 도구와 모의 worker를 유지하고 별도 `collab/real_roles.py` 실행기를 추가했습니다. 이번 실제 추론 호출0·신규 실제 업무0·승인 슬롯 미등록입니다. `collab/ai_ready.py`의 CLI는 계획 준비/Discord 보고만 제공하며 추론 실행 명령이나 자동 승인 명령을 제공하지 않습니다.

실제 Discord 응답 영수증: status2·pending1·handoff1. 기존 사용자 화면 근거와 일치하므로 세 명령 모두 실제 PASS입니다. 등록·응답 버그는 재현되지 않아 기존9명령/처리를 불필요하게 변경하지 않았습니다. 실제 채널에 준비 보고 메시지1563136599376527392 게시 성공. 기존 모의 업무8점검 PASS는 보존하고 전수 반복하지 않았습니다.

새 실행기 변경분12 테스트 PASS/0 FAIL. 모두 가짜 subprocess이며 실제 모델 연결 PASS가 아닙니다. Python 컴파일·PowerShell 구문 오류0. 테스트는 승인 없음→프로세스0, 소유자 제한, 계획SHA변조/만료 차단, API인증 거부, 4역할 공통1회 슬롯, 실패/timeout 재호출 차단, 단일turn/역할/도구 출력 검증, 후속 큐만 생성, MOCK/REAL 표시 분리를 확인합니다. 최초12와 수정후12는 같은 테스트이며24로 합산하지 않습니다.

## 공식 실행 방식과 구독 근거

설치 CLI0.163.0-alpha.11.2의 `login status`를 내부에서 판별한 결과 ChatGPT 구독 인증 가능=True. 인증 출력/계정 식별자/키는 기록하지 않습니다. `features list` 설정 검사 PASS, 모델 추론0. `codex exec --json --ephemeral --ignore-user-config --sandbox read-only`를 재사용하며 `--output-schema`로 최종 응답만 구조화합니다. 기존 built-in openai provider의 내부 설정은 덮어쓰지 않습니다. API 키/endpoint/provider 우회·로그인 변경 없음.

[공식 인증](https://learn.chatgpt.com/docs/auth)은 ChatGPT 로그인은 구독 접근, API 키는 별도 사용량 과금임을 구분합니다. [공식 비대화형 실행](https://learn.chatgpt.com/docs/non-interactive-mode)은 저장 인증 재사용, JSONL 이벤트, output-schema를 설명합니다. 문서상 구독 경로와 로컬 인증 준비는 확인했지만 **실제 요청 수락·구독 차감·해당 모델 사용 가능 여부는 아직 NOT_VERIFIED**입니다. 인증만으로 청구 내역을 입증하지 않습니다.

조회 시점 계정 전체 5시간95% 사용/5% 잔여, 주간72% 잔여. 유료 크레딧 없음. 5시간 리셋 예정2026-10-23 19:30:39 KST(조회 스냅샷). 리셋권 사용/한도 변경/구매/유료API 활성화0. 이번 준비가 계정 전체 사용률 변화 전부를 발생시켰다고 귀속하지 않습니다. 새 추론0이므로 이 준비단계의 별도 AI 호출 비용0. 후속 실제 호출 사용량은 CLI turn.completed 숫자와 인증 종류를 기록하되 계정 공유 사용률을 정확한 호출별 비용으로 계산하지 않습니다.

## 4역할 문서 준비

GitHub connector로 읽기만 수행했고 main과 각 담당 브랜치를 조회 시점 커밋으로 고정했습니다.

|참조|커밋|
|---|---|
|main|47ad2eecb40135b2ee28e4947d642ebdbf0bf68a|
|docs/planning|d73bdbf9a3162132b959f4672e88c3975b361c3c|
|docs/art-new|d9d14c0f6b7f8c0652c13b769e205dd5de426c7f|
|docs/art-renewal|1d171f79ea95ba12dc6ffcb288f3466ebc3dfb31|

AGENTS·PROJECT_CONTEXT·CURRENT_STATUS 및 PLANNING/CODEX/SPRITE_ART/WORLD_ART_RENEWAL 역할 문서, 역할별 최신 인계 발췌11파일을 준비했습니다. 각 입력에는 자기 역할 문서와 관련 인계/기획 최신 발췌만 선택합니다. 큰 CURRENT_STATUS는 발췌임을 표시하며 전체 읽기나 모든 연결 문서 검토를 주장하지 않습니다. planning CURRENT_STATUS의 오래된 첫 항목과10월23일 최신 인계 차이를 발견했으며 모델에 충돌/오래된 기록을 보고하도록 지시합니다. 실제 업무 확정 전에는 승인 대상의 연결 정본도 추가 조회해 계획을 새로 고정해야 합니다.

4개의 기존 ChatGPT 대화에 로그인하거나 제어하는 기능은 아닙니다. 구독 CLI의 독립된 ephemeral 역할 입력4개를 준비한 것이며, 아트 역할도 이번에는 문서 기반 텍스트 검토만 가능합니다. 이미지 제작/Unity 검수/최종 미술 승인 권한은 없습니다.

## 승인 전에 검토할 구체적 호출 계획

첫 검증은 CodexAgent 문서 기반 읽기 전용 요약1회를 권고합니다. 입력은 다음 경로의 `CodexAgent.plan.json`에 고정되어 있습니다. 4역할 전체가 아니라 **이 중 한 역할만 합계1회**입니다.

|역할|입력bytes|계획SHA256|
|---|---:|---|
|기획|25454|6997865bc8c9f29e648026be694f8c755fa85bef48ffc61f846e6cf4b569bb55|
|Codex|25255|d572d0ea8154abbf3af159b3c02c39c5d419d84404f7c24d541aab3a837e4988|
|메인 아트|30891|15ab21e2135db268d1ea6e78c36a9dc0a0cd8d7f3d6bbaeccbf7e9d0c28bac48|
|리뉴얼 아트|30964|c8a0c13c4a9d62a9c0bf60893262ff86179d775eab4da1b5ce37ffe764b13e51|

계획SHA는 sha256 필드를 제외한 JSON 정렬 직렬화의 해시입니다. 파일바이트 해시와 혼동하지 않습니다. 입력은 약25~31KB로 실제 tokenizer 미측정입니다. 보수적 사전 계획은 입력 최대 약31,000토큰에 프로토콜 오버헤드, 출력은 한국어600자 요청으로 잡되 정확한 토큰 사용량/응답 길이/요금 상한을 보장하지 않습니다. 추론 강도low, 모델은 설치 CLI의 기본 선택이며 실제 제공 모델명/추론 성공은 첫 호출에서 확인해야 합니다. 모델이 접근 불가하면 모델/API 자동전환 없이 종료합니다.

실행을 위한 별도 승인에는 **역할·해당 계획SHA·읽기전용1회·90초·구독 사용·결과 Discord 보고 범위**가 포함되어야 합니다. 코드가 준비되었다는 사실이나 Discord /approve(미술 승인)는 추론 승인으로 해석하지 않습니다. 승인 후 운영자가 `grant_once`를 같은 phase2/relay.sqlite에 기록하고 `run_once`를 한 번 호출합니다. 승인 유효10분·정본계획24시간, 변경 시 재검토합니다. 승인1회는4역할 공통SLOT이며 이전 실패 승인ID/장부를 재활용하거나 새DB로 우회하지 않습니다.

필요 권한: 기존 CLI ChatGPT 로그인 재사용, 승인된 텍스트를 OpenAI로 전송, 빈 격리 폴더의 schema 작성/CLI 실행, 협업 DB·결과 기록, 허용된 Discord 채널 보고. 모델에는 shell/apps/browser/computer/multi_agent/MCP/웹/플러그인/훅을 비활성화하고 stdin 문서만 제공합니다. 자식환경에는 API/Discord/GitHub 토큰을 전달하지 않습니다. 공식 CLI의 유한 transport retry 기본값은 유지하고 애플리케이션 재시도0.90초초과·오류·부정형식·도구시도는 NOT_VERIFIED로 끝나며 재호출하지 않습니다.

## 인계와 승인 정책

검증된 실제 최종 응답만 `REAL_CLI_READONLY`로 저장하고 `[REAL AI / 구독 CLI / 읽기 전용1회]`로 표시합니다. MOCK는 `[MOCK / 실제 AI 호출 없음]`으로 표시합니다. 임의 파일의 MOCK 결과를 실제 결과로 복사하지 않습니다. `AIResultReport`는 실제 실행 SLOT에 연결된 장부 근거가 없으면 게시하지 않습니다.

유효한 실제 응답은 기존 Store.finish/auto_handoff로 후속 큐 또는 대표 승인대기를 만들 수 있습니다. **다음 AI 호출은 자동 시작하지 않습니다.** 실제 AI→AI 연속 추론은 이번1회 범위 밖이며 NOT_VERIFIED입니다. 테스트는 가짜 CLI 응답을 큐로 전달하는 기능만 검증했습니다. 실제 응답의 내용을 셸/기획 변경/승인 지시로 실행하지 않습니다.

아트 제작→Codex 검수→바울 대표 최종 미술 승인→승인 범위 정식 적용을 유지합니다. 텍스트 검토 TECH_PASS는 Unity QA나 미술 승인 아닙니다. 정식 적용 이후에도 Push/main병합은 별도 명시적 승인입니다. 기존 게임/Save/UserData/승인자산/SFX 변경0, 기존 QA 반복0. 원본7356개 SHA변경/누락0·Git상태동일. 원격Push/merge0.

[근거 폴더](../Evidence/DiscordAIReady20261023/)에 준비계획·문서스냅샷·비밀 제거 사용량·슬래시 영수증·보호 결과를 기록합니다. 브랜치 codex/discord-ai-collaboration. 실제 호출 승인은 이번 작업에서 받지 않았으며 완료 보고 후 별도 단계입니다.

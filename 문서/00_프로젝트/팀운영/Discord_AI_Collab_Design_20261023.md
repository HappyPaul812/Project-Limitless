# Discord AI 협업 로컬 파일럿 착수 계약

바울 대표 첨부 통합 구축 지시(2026-10-23)를 이번 작업의 범위로 삼습니다. 기준 main은 47ad2eecb40135b2ee28e4947d642ebdbf0bf68a, 별도 브랜치 codex/discord-ai-collaboration입니다. 제품 C# 규칙은 유지하며, 요청된 Python 운영 도구만 Tools/DiscordAICollab에 추가합니다.

## 범위와 결정

- 0단계 조사, 1단계 로컬 릴레이, 2단계 #63/#64/#65 **복제 이벤트**를 구현·검증합니다. 기존 실제 QA 및 제작은 재실행하지 않습니다.
- 3단계 파일 검역은 로컬 PNG/ZIP/Manifest 검사까지 구현합니다. 실제 Unity 실행, 새 미술 제작, TTS/BGM/영상 생성은 NOT_VERIFIED입니다.
- Discord SDK 어댑터는 준비하되 실연결·채널 생성·외부 메시지 전송·유료 호출·배포는 하지 않습니다. GitHub는 조회만 합니다.
- 기술 검수, 미술 승인, 게임 적용, Push, merge는 각각 별도입니다. 이 파일럿에는 제품 적용/Push/merge 실행기가 없습니다.
- SQLite 트랜잭션, 이벤트 키, 작업 임대 토큰으로 중복을 막습니다. 중단된 RUNNING은 자동 재호출하지 않고 NOT_VERIFIED로 돌려 결과불명 비용을 보존합니다. 동시에 최대 2개입니다.
- 역할별 자동 인계는 고정된 모의 보고 단계만 허용합니다. 일반 텍스트·첨부·AI 출력은 명령 권한을 갖지 않습니다. 인계 깊이를 제한합니다.
- 승인자는 설정된 Discord 숫자 ID로만 인증합니다. 승인에는 작업 ID·SHA256·범위를 결합하고 변경 시 무효화합니다.
- 모든 비용 한도 기본 0, AI 실행 기본 꺼짐. 독립 venv를 사용하며 기존 conda py3_12를 변경하지 않습니다. 일요일(KST)에는 새 생산/QA를 시작하지 않습니다.

## 저장과 경계

SQLite: tasks/events/audit/settings/approvals/usage. inbox와 quarantine은 전용 data 폴더에만 생성합니다. ZIP은 전체 사전 검증 후 새 검역 폴더에만 풉니다. 실행 파일·경로 탈출·symlink·과대 압축·중복 경로는 거부합니다. PNG는 Pillow로 실제 decode하고 RGBA/alpha/dimension/frame 계약을 대조합니다. 출처·상용 권리 주장은 증거 경로로 남기며 법적 권리를 자동 승인하지 않습니다.

네트워크 URL 가져오기는 파일럿에서 비활성화합니다. Discord 첨부는 인증된 slash command의 SDK attachment 객체를 사용하며, 임의 URL을 서버가 따라가지 않습니다. 공유 inbox는 실제 파일을 복사해 검역합니다. 기존 ChatGPT 대화나 비공개 생성파일에 접근하지 않습니다.

## 조사와 문서 차이

AGENTS, PROJECT_CONTEXT, CURRENT_STATUS, CODEX_WORKFLOW, START_HERE, COMMON_RULES, 브랜치 규칙, 8개 담당 문서, 존재하는 6개 LATEST 인계를 확인했습니다. SFX/영상 최신 인계는 없으며 완료로 만들지 않습니다. TTS의 PT 일자/프로젝트·모델별 quota 정책은 TTS 전용이며 협업 서버의 KST 일요일 정책과 혼합하지 않습니다. SFX는 이번 최신 지시가 우선하여 PAUSED입니다.

루트 상태 문서 e2801286는 당시 스냅샷이며 실제 HEAD 47ad2eec와 다릅니다. 최신 Main23 및 아트 로컬 결과는 별도 worktree에 있습니다. 이 도구가 과거 인계를 근거로 Main23 전수 QA나 재제작을 자동 발주하지 않습니다. GitHub #63/#64/#65와 PR54를 읽기 전용 조회했습니다.

## 수용 기준

모의 Discord 접수→역할 인계→기술 보고→미술 승인 대기, 재시작 복구, 동시성/중복/루프 제한, 비용 초과, 비관리자 거부, SHA 변경, 게임/Push/merge 차단, 악성 ZIP, 원본 보호를 테스트합니다. 실제 연결·AI 추론·Unity는 실행하지 않았으면 NOT_VERIFIED로 보고합니다. 후속 운영 승인은 실행할 대상·비용·권한을 정한 뒤 별도로 받습니다.

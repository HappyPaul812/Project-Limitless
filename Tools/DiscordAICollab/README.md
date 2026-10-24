# 운영 자동화 실연동 (2026-10-24)

[완료 보고](../../문서/00_프로젝트/팀운영/Operations_Automation_Live_Verified_20261024.md)와 [운영 안내](OPERATIONS_LIVE_KO.md)를 확인하세요. 새6종 Slash 실제응답·공식 App Server 사용량 자동조회 연결 완료. 승인된 단발 재개만 가능하며 현재대기0·추가추론0. Windows 자동시작은 비활성XML만 준비·미등록, 실제PC종료/Push/main병합 비활성. 아래 이전 단계의 새 명령/자동사용량 미연결 표기는 당시 이력입니다.

# Phase3 대표 일반 첨부 VERIFIED (2026-10-23)

[실제 검증 보고](../../문서/00_프로젝트/팀운영/Phase3_Owner_Attachment_Verified_20261023.md)와 [자동 시작·중지 설계](../../문서/00_프로젝트/팀운영/Phase3_Autostart_Stop_Design_20261023.md)를 추가했습니다. 기존 감시기가 자동 등록한 대표 일반 PNG1건을 원문/SHA로 확인하고 TEST_ONLY·승인불가로 구분했습니다. 새로운 업로드나 수동 업무 생성은 없었습니다. 기존3건 보존·AI/Unity/자동시작등록/Push0. 아래 과거 일반첨부 미검증 표기는 당시 이력입니다.

# 예약·사용량·안전 종료 모의 기능 (2026-10-23)

[운영 안내](SCHEDULE_OPERATIONS_KO.md)와 [검증 보고](../../문서/00_프로젝트/팀운영/Scheduling_Usage_Shutdown_Simulation_20261023.md)를 추가했습니다. 예약/마감/사용량 차단·종료 조건은 로컬 모의 검증을 마쳤으며 실제 종료·통합은 비활성입니다. 새 6개 Slash 명령의 서버 등록과 서비스 재시작은 이번에 하지 않았습니다. 아래 기존 Phase3·AI 단계 이력은 보존합니다.

# Phase3 현재 운영 상태

아트-납품 Discord 첨부 및 Windows 납품함 감시·격리 검사·Codex 대기 업무·Discord 보고를 연결했습니다. [운영 안내](PHASE3_ART_DELIVERY_KO.md)와 [검증 보고](../../문서/00_프로젝트/팀운영/Phase3_Art_Delivery_Verified_20261023.md)를 확인하세요. 수령 감시만 실행 중이며 AI·Unity worker는 실행하지 않습니다. 대표 계정 신규 첨부 수령은 아직 직접 관찰하지 않았습니다. 아래 내용은 이전 단계 이력이며 watcher 미연결 등 당시 상태를 보존합니다.

# LIMITLESS Discord AI 협업 — 로컬 파일럿

## 2026-10-23 최신 검증 — 네 역할 단발 자동 인계 성공

Planning→NewArt/Renewal→Codex→원래Planning기록 연결 파일럿1건을 실제CLI4회로 완료했습니다. [실제 보고](../../문서/00_프로젝트/팀운영/Four_Role_Real_Handoff_Verified_20261023.md)와 [납품 자동화 조사](../../문서/00_프로젝트/팀운영/Art_Delivery_Automation_Feasibility_20261023.md)를 현행 기준으로 확인하세요. 전용 슬롯은 소비됐으며 `FourRolePilotOnce` 재실행·추가AI는 새 지시 전 금지입니다. 실제 아트/Unity QA worker·폴더 watcher는 연결되지 않았습니다. 기존 ChatGPT 대화 파일 자동 접근도 없습니다. 아래 초기 단계 안내는 당시 이력으로 보존하며, 현재의 개별 AI VERIFIED/Discord 실연결 여부는 최신 보고를 따릅니다.


## 실제 AI 준비 — 별도 승인 대기

최신 첫 시험은 **PlanningAgent 정본 요약1회**로 제한합니다. 다른 역할 grant/run은 차단하며 성공 후 확장은 별도 승인입니다. status/pending 한국어 표시와 MOCK 미술승인 제외 안내를 실제 Discord에서 확인했습니다. 최신 승인계획은 [PlanningAgent 준비 보고](../../문서/00_프로젝트/팀운영/Discord_Planning_First_Approval_Pending_20261023.md)를 따릅니다.

`collab.real_roles`는 기존 MOCK worker와 별도의 구독 CLI 실행기입니다. `python -m collab.ai_ready prepare`로4역할 SHA 고정 입력을 만들고, 바울 대표의 별도 승인 후에만 같은 협업DB의 공통1회 슬롯을 grant/run합니다. 승인 전 실제 모델 호출0. Discord9명령에는 추론 실행/승인 명령을 추가하지 않았습니다. `register-discord.ps1 -Action AIReadyReport`는 준비 상태만 게시하며 `AIResultReport`는 승인 실행 장부의 실제 결과만 게시합니다. [범위·계획 SHA·예상 사용량](../../문서/00_프로젝트/팀운영/Discord_AI_Real_Connection_Readiness_20261023.md)을 먼저 확인하세요.

## 암호화 등록 및 실연결 후속

[안전한 로컬 설정 안내](SECURE_DISCORD_SETUP_KO.md)를 따릅니다. `register-discord.ps1 -Action Setup`은 숨김 입력·현재 Windows 사용자 DPAPI 저장·최대45초 연결 검사를 수행합니다. 실제 연결/명령9등록/메시지 전송 PASS는 확인했으며 실제 Slash 실행은 별도 영수증으로 구분합니다. `-Action Run`은 봇을 유지합니다. 기존 connect-discord.ps1은 메모리 입력용으로 보존합니다.

`HandoffPrepare`는 고정 모의 업무1건을 REVIEW_PENDING으로 준비하고 지정 채널에 안내를 전송합니다. 바울 대표가 실제 `/limitless handoff`를 실행한 뒤 `HandoffFinish`가 Codex→기획→승인대기까지 모의 결과만 전달합니다. 모의 TECH_PASS는 실제 Unity 검수/미술 승인 의미가 아닙니다. 신규 아트는 Codex 검수 후 바울 대표 최종 승인, 적용 이후에도 Push/merge는 별도 승인입니다. 실제 AI runner는 비활성입니다.

## Phase2 후속 상태 (2026-10-23)

[대표 설정 안내](PHASE2_SETUP_KO.md)를 추가했습니다. Discord 설정은 WAITING_FOR_USER_SETUP이며 숨김 입력 connect-discord.ps1로 대표가 직접 연결합니다. 기존9명령·큐를 재사용하고 응답 성공/권한 거부 영수증만 추가합니다. 고정 Codex 실행1회는 bootstrap provider override 거부로 완료turn0이며 자동 재실행0입니다. 수정 준비안은 추론 없는 설정 검사만 PASS, 새 추론은 별도 승인 필요입니다. 이전 단계의57 PASS와 새 변경분 테스트는 구분합니다. 아래 최초 파일럿 이력은 보존합니다.

바울 대표의 2026-10-23 통합 구축 지시를 위한 독립 Python 도구입니다. 실제 ChatGPT 담당 세션을 제어하는 완성 서비스는 아닙니다. 기존 세션·로그인·비공개 생성물에 접근하지 않습니다.

## 현재 실행 가능한 범위

- 영속 SQLite 작업 큐, 숫자 ID 인증, 이벤트 중복 제거, 임대·2개 동시 실행 제한, 일요일 KST에도 사전 승인 업무 허용(모드·승인·한도 유지).
- 역할 8개 설정. 기획/구현/신규·리뉴얼 아트는 이벤트 방식, TTS/BGM/영상은 요청 시, SFX는 PAUSED.
- 모의 역할 보고와 자동 인계. #63→Codex→기획→바울 대표 승인대기를 **복제 이벤트**로 실행합니다.
- Discord 정식 SDK의 `/limitless` 9개 slash command, 인증된 ZIP 첨부 검역. 실연결은 아직 하지 않았습니다.
- GitHub 고정 저장소 GET 어댑터. 쓰기, push, merge 없음.
- ZIP SHA256/CRC/Manifest, RGBA PNG decode·해상도·프레임 계약, 권리 검토 대기, 기존 파일명 충돌 보고.
- SHA·범위 결합 승인과 파일 변경 시 무효 판정. 기술 PASS는 미술 승인이 아닙니다. 모의 QA는 실제 미술 승인에 사용할 수 없습니다.

실제 AI 역할 실행·유료 API·Unity 검사·제품 적용·자동 채널 게시·GitHub 쓰기는 구현/운영 승인 전 단계입니다. Discord bot은 접수·상태·승인 게이트만 제공하며 실제 QA worker를 실행하지 않습니다. `CodexRunner`는 공식 CLI 명령 미리보기만 제공하고 `run()`은 항상 거부합니다. `LIMITLESS_ALLOW_AI=1`만으로 이 제한을 풀 수 없습니다.

## 설치와 로컬 실행 (PowerShell)

```powershell
Set-Location 'F:/study/codex/Project-Limitless/Temp/DiscordAICollab20261023/Worktree/Tools/DiscordAICollab'
./setup.ps1
$env:PYTHONIOENCODING='utf-8'
./.venv/Scripts/python.exe -m collab demo --data data/demo
./.venv/Scripts/python.exe -m collab status --data data/demo
./.venv/Scripts/python.exe run_tests.py
```

setup은 이 폴더의 `.venv`에만 설치합니다. 기존 `conda activate py3_12`의 Python은 3.12.14이고 이를 읽기 전용 확인했습니다. discord.py 2.7.1·Pillow 12.3.0과 전이 의존성은 `requirements.lock.txt`로 기록했습니다. 설치에는 패키지 다운로드 네트워크가 필요합니다. 테스트의 asyncio 로컬 소켓이 Windows 샌드박스에 막히면 오프라인 테스트 프로세스만 허용해야 합니다. 봇을 켤 필요는 없습니다.

`demo`는 3개 원본 모의 이벤트에서 6개 역할 작업을 만들고 3개 승인대기에 도달합니다. 같은 DB에서 다시 실행해도 6개를 유지합니다. 일요일에도 사전 승인 업무를 처리할 수 있으며 수동 정지·승인·사용량·동시 실행 제한은 유지합니다. 모든 기술 결과 근거는 MOCK이고 실제 Unity는 NOT_VERIFIED입니다.

`serve`는 로컬 stdin JSONL 릴레이입니다. 인터넷 리스닝 포트가 없습니다. 별도 터미널에서 다음 한 줄을 입력할 수 있습니다.

```powershell
./.venv/Scripts/python.exe -m collab serve --data data/local
# 이후 stdin에 입력
{"mode":"MOCK","event_id":"status-001","user_id":1,"action":"status"}
```

stdio의 user_id=1은 로컬 테스트 전용이며 Discord 인증과 다른 DB를 씁니다. 외부에 stdin 제어권을 공개하지 않습니다. `tick` 모의 이벤트는 한 작업 보고와 인계를 진행합니다. 임의 셸·Python·모델 출력 실행은 없습니다.

## 작업 계약

`python -m collab`의 `sample()`와 tests를 예제로 사용하세요. 필수 필드:

| 필드 | 의미 |
|---|---|
| title / role | 작업명, 역할 설정의 정확한 키 |
| issue / base_sha | 관련 이슈(또는 PR를 별도 추가), 기준 Git SHA40 |
| scope / protected_assets | 승인 가능한 정확한 범위와 보호 목록 |
| inputs / verification | 요청 입력·검증 기준 목록 |
| approval_required | 승인 필요 여부. 파일럿은 false여도 자동 적용하지 않음 |
| artifact / artifact_sha | 실제 검역 ZIP 경로와 SHA256; 모의는 명시적 가짜 경로 |

상태: NEW→TRIAGED→RUNNING→REVIEW_PENDING→APPROVAL_PENDING→APPROVED. 역할 보고를 인계한 작업은 DONE이며 **인계 완료** 의미입니다. 실제 미술·게임 적용 완료 의미가 아닙니다. FAILED/BLOCKED/NOT_VERIFIED를 별도 보존합니다. CANCELLED는 예약 상태이며 취소 명령은 후속 단계입니다. 최대 인계 깊이4, 설정된 next 역할로만 인계합니다. 모델이 임의 역할·권한을 지정할 수 없습니다.

## 첨부·공유 inbox

```powershell
./.venv/Scripts/python.exe -m collab intake --zip '실제 ZIP 절대경로' --sha '제작본 SHA256' --data data/review
```

새 검역 폴더에만 풀며 원본 ZIP·Unity·Save에 쓰지 않습니다. 정식 파일명 충돌은 API의 official_names 목록으로 보고하며 자동 교체하지 않습니다. 실제 제품 목록의 수집·연결은 후속 Unity QA 단계입니다. 과거 아트 ZIP의 Manifest 형식은 제각각이므로 이 파일럿의 엄격 계약과 다르면 거부합니다. 기존 ZIP을 수정하지 말고 후속 승인된 변환기에서 원본 SHA를 참조하도록 설계해야 합니다.

ZIP 루트 `Manifest.json` 계약:

```json
{
  "source":"제작자·출처",
  "commercial_rights":"상업 사용권에 대한 주장; 사람의 증거 검토 필요",
  "license_evidence":"License.txt",
  "files":[
    {"path":"candidate.png","sha256":"실제 64자리 SHA", "dimensions":[512,512],"frames":1},
    {"path":"License.txt","sha256":"실제 64자리 SHA"}
  ]
}
```

Manifest 자신을 제외한 모든 파일을 등재합니다. 시트는 `frame_size:[폭,높이]`와 실제 frames를 지정합니다. 최대 ZIP32MiB·확장128MiB·256항목·항목32MiB·압축비200·PNG16777216픽셀입니다. 경로 탈출, 실행·위장 실행, symlink, ADS, Windows 예약명, 대소문자 충돌, CRC/SHA 불일치는 거부합니다. source/commercial_rights가 있어도 RIGHTS_HUMAN_REVIEW_PENDING입니다. 실제 법적 권리·미술·배경 품질/실게임 크기·Unity 접근성은 자동 승인하지 않습니다. 임의 URL 다운로드는 SSRF 방지를 위해 비활성입니다. 공유 파일 또는 인증된 Discord Attachment만 사용합니다.

## Discord 실연결 준비 — 현재 실행하지 않음

바울 대표가 실연결을 승인한 뒤 Discord Developer Portal에서 bot/application을 준비합니다. 일반 사용자 계정 토큰이나 브라우저 세션은 사용하지 않습니다. bot·applications.commands 범위로 테스트 guild에 초대하고 필요한 채널 보기/명령 응답 권한만 부여합니다. Message Content·Administrator 권한은 요구하지 않습니다.

채널안: 대표-지시 / 기획-회의 / codex-검수 / 아트-협업 / tts-오디오 / 승인-대기 / 업무-완료 / 시스템-로그. 자동 채널 생성은 하지 않습니다. 현재는 허용 채널의 slash 응답만 ephemeral로 보내고 역할별 채널 자동 게시·알림 outbox는 후속 단계입니다.

`.env.example`은 설명서이며 자동 로드하지 않습니다. 실제 값은 작업 프로세스 환경변수에만 넣습니다. `LIMITLESS_OWNER_IDS`(쉼표로 숫자 ID), `LIMITLESS_GUILD_ID`, `LIMITLESS_CHANNEL_IDS`, `DISCORD_BOT_TOKEN`, 명시적 실연결 승인 뒤 `LIMITLESS_ALLOW_DISCORD=1`이 필요합니다. 서버의 관리자/표시명만으로는 소유자 권한을 주지 않습니다. 모든 slash command를 다시 Store에서 인증합니다.

```powershell
# 위 실연결 승인을 받은 뒤에만 실행
./.venv/Scripts/python.exe -m collab discord --data data/live
```

첫 연결에서 **테스트 guild에 명령을 등록하는 외부 쓰기**가 발생합니다. SDK가 Discord rate limit을 처리합니다. 명령은 먼저 defer하고 검역을 백그라운드 thread로 넘깁니다. Gateway의 인증된 interaction 외에 임의 HTTP webhook은 받지 않습니다. ACK 후 재시작 때문에 응답이 유실될 수 있으므로 같은 interaction 키의 장부를 확인합니다. 자동 메시지 재발송은 하지 않습니다.

## 승인·중단·복구

- `/limitless status`, `pending`: 장부 상태 조회.
- `task`: JSON 계약과 선택 ZIP 접수만. 외부 자연어를 명령으로 실행하지 않음.
- `handoff`: REVIEW_PENDING 보고만 고정 다음 역할로 인계.
- `approve/reject`: task_id+sha+scope. 미술 승인만 기록하며 본편 적용/Push/merge 권한을 주지 않음.
- `pause/resume`: 전체 또는 정확한 역할 키. 정지 시 RUNNING은 NOT_VERIFIED로 바꾸어 늦게 도착한 결과를 거부. 실제 외부 프로세스 종료는 실행기가 없는 이번 단계의 범위 밖.
- `budget`: task/day/month **micro-USD** 한도. 1000000=USD1. 기본0이므로 호출 선점 자체가 거부됨. 예산 설정만으로 AI 실행은 활성화되지 않음.

실행 임대는300초입니다. 만료 후 NOT_VERIFIED로 보존하며 자동 재실행하지 않습니다. 재시작 시 recover()가 만료 작업을 회수합니다. 아직 유효한 임대는 유효기간까지 기다립니다. 사람이 원래 호출/결과를 확인한 뒤 새 event_id로 별도 검토 작업을 접수하세요. API 결과불명은 선점 비용을 보존하며 QUOTA_EXCEEDED는 전체 신규 시작을 멈춥니다. 자동 유료 fallback·다른 키로 무조건 재시도 없음. TTS는 기존 PT/실 프로젝트·모델 quota 계약이 별도로 필요하며 이 KST 장부가 무료 한도를 증명하지 않습니다.

DB 백업은 relay 프로세스를 정상 종료한 뒤 data 폴더 전체를 새 경로에 복사합니다. 운영 중 복사는 SQLite backup API가 필요합니다. 원래 DB를 덮어쓰지 말고 별도 data 경로로 복구한 뒤 status와 NOT_VERIFIED/RESERVED를 확인합니다. SQLite 파일과 inbox는 같은 OS 사용자의 비공개 자료이므로 서버에 공개하지 않습니다. 토큰·원문을 audit에 기록하지 않지만 작업 계약 payload 자체는 저장하므로 비밀을 계약에 붙이지 마세요. data/.env/.venv는 Git 제외입니다.

## 공식 연동과 비용 조사 (2026-10-23)

설치 CLI: 0.163.0-alpha.11.2. `login status`는 ChatGPT 로그인, `exec --help`는 --json/--ephemeral/--ignore-user-config/--sandbox read-only 지원을 확인했습니다. 실제 모델 추론은0회이며 선택 모델의 계정 권한·잔여량은 미실증입니다. 도움말 조회에 샌드박스 arg0 임시 경로 경고가 있었지만 인증/도움말은 종료0이었습니다.

[공식 Codex 인증](https://learn.chatgpt.com/docs/auth)은 ChatGPT 구독 로그인과 API key 사용량 과금을 구분합니다. [공식 가격/한도](https://learn.chatgpt.com/docs/pricing)에 Plus의 Codex 포함, 작업/모델에 따라 다른 공유 사용량과 주간 한도 가능성이 안내되어 있습니다. Plus 표의 GPT-6.1 Sol/GPT-6 Luna는 **이 계정에서 추론 성공을 확인했다는 뜻이 아닙니다**. 기존 구독 비용 외 파일럿 AI 추가 호출 비용은0입니다. 추후 API를 선택하면 [공식 API 가격](https://platform.openai.com/docs/pricing)과 당시 모델 권한을 별도 확인하고 금액을 승인받아야 합니다. 가격을 코드에 고정하지 않았습니다.

공식 최소 연동안은 [비대화형 codex exec](https://learn.chatgpt.com/docs/non-interactive-mode)의 JSONL 결과입니다. 장기 UI/승인이 필요하면 [공식 app-server](https://developers.openai.com/siwc/token-sharing-open-source/codex-app-server) 경로를 후속 검토합니다. 새 OAuth 앱의 Sign in with ChatGPT와 이미 로그인된 CLI 사용은 다른 설치 경로입니다. 공식 동의가 기존 ChatGPT 대화 접근권을 주지는 않습니다. 앱 전용 OAuth 토큰 추출·기존 대화 resume·브라우저 자동 로그인은 구현하지 않습니다.

[Discord 공식 명령](https://docs.discord.com/developers/interactions/application-commands), [응답](https://docs.discord.com/developers/interactions/receiving-and-responding), [rate limits](https://docs.discord.com/developers/topics/rate-limits), [discord.py 공식 SDK 문서](https://discordpy.readthedocs.io/en/stable/interactions/api.html)를 기준으로 Group/CommandTree/Attachment를 사용했습니다.

## 다음 승인과 미완료

1. 테스트 guild·bot·소유자/채널 ID와 **실연결·slash 등록** 승인. 토큰은 대화/Git에 제출하지 않습니다.
2. 제한된 읽기 전용 Codex 단일 작업의 실제 추론 승인·모델/구독잔여량 확인·credential/MCP 격리·외부 권한 차단. 이 검증 전 자동 AI worker는 연결하지 않습니다.
3. 실제 아트 Manifest 변환 계약, Unity 검수 실행기와 증거 수집기, 승인한 SHA의 게임 적용기, 역할별 알림 outbox, 월간 영상 scheduler를 각각 후속 구현합니다. 영상 자동 게시0·SFX PAUSED 유지.
4. GitHub 쓰기/배포/실제 제작·API 비용/원본 적용/Push/merge는 각각 별도 승인입니다.

서비스 계정 A 실행기 연결 및 인증 HOLD/단발 점검은 [SERVICE_AI_BRIDGE_KO.md](SERVICE_AI_BRIDGE_KO.md)를 따릅니다. MANUAL은 대표 명시적 활성화 전까지 유지합니다.

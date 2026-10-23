# 운영 자동화 실연동 VERIFIED

시작 2026-10-23 / 완료 2026-10-24 KST / 브랜치 `codex/discord-ai-collaboration`

## 결과와 실제 범위

기존9+새6 Slash 등록 완료. **바울 대표 계정에서 새6종 모두 실제 실행·한국어 비공개 응답을 확인**했고 Gateway interaction/response 영수증을 저장했습니다. 공식 Codex App Server 사용량 자동 조회, 소진/실패 보류·회복 재확인 및 이미 승인된 단발 작업의 재개 경로를 연결했습니다. 현재 제어는 LIVE_CONTROL, 실제 PC 종료는 비활성입니다.

신규 로컬 모의 **27 PASS / 0 FAIL**. 기존 네 AI VERIFIED·자동 인계·아트 입고 전체 QA는 재실행하지 않았습니다. 실제 AI 재개 큐0건으로 추가 외부 AI 추론0회입니다. 실제 한도 소진을 유도하거나 실제 모델 재개를 실행하지 않았으며 해당 분기는 모의 runner로만 확인했습니다.

## 실제 Discord 명령

| 명령 | 실제 응답 ID | 판정 |
|---|---|---|
| usage | 1563203648329220176 | PASS·공식 조회 결과 |
| schedule | 1563204118061785158 | PASS·SCH-79e9740a0440 생성 |
| schedule-status | 1563204498308997210 | PASS·등록된 예약 조회 |
| shutdown | 1563205218022465689 | PASS·PC 지문/진행중 업무 확인·종료 HOLD |
| finish | 1563205626971295794 | PASS·마감 장부 요약·통합 HOLD |
| cancel | 1563205979166744796 | PASS·release_intake:true로 시험 예약/접수 보류 해제 |

모두 owner_verified=1, ephemeral=1, gateway=1입니다. 응답 후 SDK가 반환한 실제 response ID를 저장했으며 봇 자체 모의 호출을 실제 사람의 명령으로 계산하지 않았습니다. [영수증](../Evidence/OperationsLive20261024/actual-command-receipts.json).

시험 마감은 2026-10-24 02:00 KST, 정리10분이었으나 취소 완료했습니다. 현재 활성 예약 없음. 수동 pause/역할 정지/미술 보류는 취소로 해제되지 않습니다. 특정 예약의 접수 보류 해제는 대표의 release_intake:true를 요구하고 RUNNING이 있으면 거부합니다. [예약 기록](../Evidence/OperationsLive20261024/runtime-schedules.json).

관리 명령은 기존 숫자ID 설정을 사용하되 실제 봇은 owner1명만 허용합니다. 비대표 요청이 사용량 provider에 도달하지 않는 분기는 신규 모의 검증 PASS입니다. 별도 비대표 Discord 계정의 실제 실행은 시험하지 않았습니다.

Discord 공개 보고: shutdown HOLD `1563205236997230653`, finish HOLD `1563205639868522673`, 최종 운영 보고 `1563209891034693767` — 모두 SENT. [등록/운영 상태](../Evidence/OperationsLive20261024/live-status.json).

## 공식 사용량 연결

[공식 App Server 문서](https://learn.chatgpt.com/docs/app-server)에 명시된 stdio JSONL을 사용했습니다. 설치 CLI `0.163.0-alpha.11.2`에서 실제 성공했습니다. CLI 자체는 alpha 버전이므로 버전/응답 변경을 추측으로 보정하지 않고 불명확하면 HOLD합니다.

RPC는 **initialize → initialized → account/read(refreshToken:false) → account/rateLimits/read**만 허용합니다. chatgpt 인증만 수용합니다. thread/turn·로그인 변경·유료 API·크레딧 구매·재설정권 소비·메일 전송 요청을 하지 않습니다. 원문 계정ID/이메일/토큰/크레딧 정보/CLI stdout·stderr는 저장하지 않고 안전한 사용량 필드만 정규화합니다.

2026-10-24 00:27:38 KST 관측은 5시간 **60% 사용 / 2026-10-24 03:23:53 KST 회복**, 주간 **9% 사용 / 2026-10-30 22:23:53 KST 회복**입니다. 개발 대화/다른 세션이 같은 구독을 사용하므로 이후 값은 변합니다. 봇의 자동 관측은 실제 여러 차례 성공했으며 최신 값은 `/usage`로 갱신합니다. [최신 관측 근거](../Evidence/OperationsLive20261024/official-usage-latest.json), [최초 공식 연결](../Evidence/OperationsLive20261024/official-usage-initial.json).

- 정상120초 주기, 대표 `/usage`는 즉시 재확인합니다.
- 소진된 창의 가장 늦은 공식 회복 시각까지 대기하고 이후 새 관측으로 확인합니다.
- 실패 즉시 이전 AVAILABLE도 무효화하여 UNKNOWN/HOLD. 자동 실패3회면 회로를 열고 중지합니다.
- 관측5분 만료·필수 창/허용 여부/회복 시각 불명확·주일·마감 여유 부족은 실행 차단입니다.

재개는 기존 정확한 plan SHA·role·slot 승인이 APPROVED이고 미소비·미만료일 때만 큐에 들어갑니다. 빈 격리 폴더/실행파일 SHA/동시2개/주일/예약/역할 정지를 확인하고 기존 readonly90초 실행기·출력 검증기를 최대1회 재사용합니다. 새 승인·승인기한 연장·MOCK/TEST_ONLY의 실제 업무 전환은 없습니다. 기존 승인10분이 회복 전에 만료되면 재승인이 필요합니다. 실패·소비·재시작 중 RUNNING 결과불명은 자동 재시도하지 않습니다.

현재 실제 대기0·외부 역할 추론0, 기존 모델 usage13행 동일입니다. 이 개발 대화 자체가 사용하는 구독 사용량까지0이라고 주장하지 않습니다. 추가 유료 API 호출/크레딧·재설정권 소비는0입니다.

## Windows 자동 시작 준비 — 등록 제외

| 설정 | 준비 내용 |
|---|---|
| 작업 | LIMITLESS-Art-OnLogon / LIMITLESS-Bot-OnLogon |
| 트리거 | 동일 DPAPI 사용자 로그인 후30초 |
| 실행 | 고정 operations-service.ps1 StartIfEnabled, Art/Bot enum만 |
| 권한 | InteractiveToken·LeastPrivilege |
| 중복 | IgnoreNew + 각 서비스 file lock |
| 실패 재시작 | 최대3회·간격1분, wrapper가 자식 종료까지 대기 |
| 강제 종료 | 금지, 서비스 실행시간 제한0 |
| 현재 | XML Disabled·작업 등록0·승인파일 없음·시작 게이트 비활성 |

공식 Windows cmdlet으로 실제 미등록 XML 두 개를 내보내고 설정을 검사했습니다. 미승인 StartIfEnabled는 실제로 HOLD하며 새 프로세스를 시작하지 않았습니다. [XML 검사](../Evidence/OperationsLive20261024/autostart-xml-checks.json), [Art 설정안](../Evidence/OperationsLive20261024/Art-autostart-plan.json), [Bot 설정안](../Evidence/OperationsLive20261024/Bot-autostart-plan.json), [등록0 확인](../Evidence/OperationsLive20261024/task-registration-check.json).

실제 XML은 도구의 `data/operations/LIMITLESS-Art-OnLogon.xml`, `LIMITLESS-Bot-OnLogon.xml`입니다. 등록/활성화는 별도 명시적 승인 전 수행하지 않습니다. 토큰은 기존 CurrentUser DPAPI/ACL로 유지하고 XML/인수에 넣지 않습니다. 영속 disable 표식·고정 코드 오류 journal·시각별 로컬 stdout/stderr 보존을 준비했습니다. [조작/안전 중지 안내](../../../Tools/DiscordAICollab/OPERATIONS_LIVE_KO.md).

PC 완전 종료 중에는 자동 재개할 수 없습니다. 이 로그인 방식은 로그아웃 중 지속 운영도 보장하지 않습니다. 재부팅/로그아웃 실험·SYSTEM 우회·암호 저장·자동 로그인은 하지 않았습니다.

## 종료 안전성과 운영 중지

종료 요청은 현재 PC 지문과 실제 SQLite RUNNING 수를 읽습니다. 미저장 사용자 작업·파일복사·Unity 안전 상태·명시적 통합 승인·원격/로컬 SHA 동기화는 미확인이므로 HOLD입니다. 실제 종료 API/명령 실행기는 연결하지 않았습니다. `/cancel` 취소 및 향후120초 유예/재검증 계약을 유지합니다. [실제 preflight/마감 결과](../Evidence/OperationsLive20261024/pc-shutdown-and-finish-checks.json).

봇 정상 종료에서 timer 취소가 SDK close까지 취소하던 충돌을 발견해 수정했습니다. 단일 신규 모의 시험과 실제 정상 종료 exit0·STOPPED·재접속을 확인했습니다. 중복 봇 실행도 실제 file lock으로 거부됐습니다. 최초 구형 봇 교체는 백업/RUNNING0 확인 후 정확한 봇 PID만 종료했습니다. 나머지는 정상 종료 표식을 사용했습니다.

아트 수령 서비스는 예약 게이트를 읽도록 기존 커서/지문을 보존해 정상 재접속했습니다. 재접속 후 기존 TEST_ONLY/입고 이벤트/검수 업무가 재등록되지 않았음을 대조했습니다. 실제 AI·Unity Worker를 시작하지 않았습니다. 현재 봇과 아트 수령 서비스는 실행 중입니다.

## 검증·보호·Git

신규 모의27건 = 최초23 + 소진/백엔드거부1 + 운영모드/손상계획2 + 정상종료충돌1. 관련2건 재확인은 중복 가산하지 않았습니다. Python compileall·PowerShell 문법·git diff 검사 PASS. 기존48/45/8 및 네 AI 전수 재실행 없음. 실제 계정의 소진/실제 AI 재개는 유도하지 않았습니다.

[원본 보호](../Evidence/OperationsLive20261024/protection.json): **7,356파일 SHA변경0/누락0**, 원본 git 상태 동일. 기존 업무21→21·모델 usage13→13·승인/설정/아트 수령/원래 이벤트/아트 outbox 내용 동일, 격리 원본 SHA 모두 일치. TEST_ONLY 시험은 그대로이며 원본게임/Save/승인아트 변경0·Unity0·실제 PC종료0·자동 시작등록0·Push/main 병합0.

아트 제작 → Codex 검수 → 바울 대표 미술 승인 → 승인 범위 내 게임 적용 순서를 유지합니다. 예약·운영 실연결·미술 승인으로 Push/main 병합 권한을 확장하지 않습니다.

구현 커밋 `7ef5495312ab6d986a646f8ee32d3656e823c2bd`. 보고/근거는 후속 로컬 문서 커밋입니다. 원격 Push 및 main 병합 없음. 다음 필요한 승인은 두 Windows 자동 시작 작업의 등록/활성화 범위이며 이번에는 진행하지 않았습니다.

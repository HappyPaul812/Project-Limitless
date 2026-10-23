# Discord 협업 Phase2 준비·제한 검증 보고 — PARTIAL

바울 대표의 후속 지시에 따라 de6198480f4021f4aecaae51cbc88c57234b3d77 위에서 기존 구현을 유지하고 실연결 준비·단발 실행 게이트만 보완했습니다. **Discord는 WAITING_FOR_USER_SETUP, 실제 Codex 단발 검증은 bootstrap 실패**입니다. 연결·추론 성공으로 보고하지 않습니다.

## 1. Discord 준비와 대표 설정

기존9명령/SQLite/승인/검역을 그대로 사용합니다. 숨김 토큰 입력 `connect-discord.ps1`, 비밀을 출력하지 않는 preflight, Gateway·명령 등록 영수증, 실제 응답 성공/권한 거부 audit를 추가했습니다. SDK 오류 원문은 노출하지 않고 무한 재연결하지 않습니다.

**대표가 직접 할 일:** Developer Portal 앱/bot 생성→Guild Install의 bot·applications.commands 범위→테스트 채널 View Channels/Send Messages만→관리 중인 테스트 서버 초대→개발자 모드로 대표/서버/채널 숫자 ID 준비→로컬 스크립트에서 토큰 숨김 입력. Administrator·privileged intents·일반 채팅 열람은 필요 없습니다. [한국어 단계별 안내](../../../Tools/DiscordAICollab/PHASE2_SETUP_KO.md).

현재 프로세스에는 토큰·대표/서버/채널 ID가 없어 **외부 Discord 접속0·실제 메시지 전송0**입니다. `/limitless status/task/handoff/pending/approve/reject/pause/resume/budget` 9종과 비소유자 거부는 실제 SDK 콜백을 이용한 **로컬 모의 확인**만 통과했습니다. 실제 서버 Gateway·명령 등록·송수신·권한 차단은 모두 NOT_VERIFIED입니다. 임의 인증정보 생성·브라우저 세션 우회·계정 연결은 하지 않았습니다.

## 2. Codex 실제 단발 실행

설치 CLI **0.163.0-alpha.11.2**, 공식 ChatGPT 구독 로그인 방식의 사용 가능 여부만 로컬에서 확인했습니다. 계정 식별자·인증 원문·토큰 파일은 보고에 저장하지 않았습니다. [공식 인증](https://learn.chatgpt.com/docs/auth)과 [비대화형 실행](https://learn.chatgpt.com/docs/non-interactive-mode)에 따른 경로입니다.

승인된 실행은 빈 별도 workspace/read-only/고정 완료표식 입력으로 **CLI exec1회**, 종료코드1, **완료 turn0 / 응답 미수신 / 토큰 사용량 이벤트 미수신**입니다. API 키 환경변수 제거·ChatGPT 방식 강제, 제품 입력0, 자동 재실행0입니다.

**실패 원인:** 제가 지정한 `model_providers.openai.request_max_retries=0` 및 stream_max_retries=0이 설치버전의 **reserved built-in provider override 금지**에 걸렸습니다. 동일 설정의 추론 없는 bootstrap 검사에서도 같은 거부를 확인했습니다. 따라서 성공한 모델 추론은 없으며, 안전한 결과 표식도 수신하지 못했습니다.

**준비안 수정:** 두 override를 제거하고 built-in provider를 유지했습니다. API endpoint/custom provider로 우회하지 않았습니다. 이후 **추론 없는 features list만** 실행하여 설정 로딩과 shell/apps/browser/computer/multi_agent 비활성 PASS를 확인했습니다. 현재 실행기는 실제 모델 앞에서 이 검사를 먼저 요구합니다. 새 추론은 실행하지 않았습니다.

주의: 수정 준비안에서는 built-in의 유한 transport retry 기본값이 남습니다. 애플리케이션은 재시도하지 않으며 CLI1회/90초 한도를 유지합니다. 정확한 provider 요청 횟수를1이라고 보장하지 않습니다. 최초 승인 장부는 소비된 채 보존하므로, DB삭제/초기화로 재시도하지 않고 **새 단발 승인ID를 부여하는 별도 후속 승인**이 필요합니다.

## 3. 큐 연결·사용량·비용

실제 전용 SQLite 큐 `Tools/DiscordAICollab/data/phase2/relay.sqlite`에 다음이 기록됐습니다. 이 DB는 대표가 준비된 bot을 실행할 때 동일하게 사용합니다. 로컬 검증 신원은 실제 Discord 신원으로 전송하지 않습니다.

- task `L-bd113b185adf4a60`, CodexAgent / **NOT_VERIFIED / USER_REVIEW_PENDING**.
- 단발 승인 예약→실행 실패→결과불명 사용량→종료 audit, 중복 재실행 차단.
- 입력/출력 원문 대신 상태·숫자 사용량·고정 표식 여부만 전달. **실제 성공 추론→Discord 왕복은 NOT_VERIFIED**입니다.
- 사용량 장부의 input/output=0은 미수신 시 자리값, status=UNKNOWN입니다. 이를 실제0토큰 실측으로 해석하지 않습니다. 추가 유료 API 호출0·추가 API 비용USD0, 키 fallback·결제·크레딧 구매0.
- 계정 전체 사용률 조회 스냅샷: 5시간61%→69%, 주간23%→24%. 이 구현 대화/다른 공유 작업을 포함하는 값으로 단발 실패의 사용량·비용으로 배분할 수 없습니다. 계정 식별자는 제거했습니다. 기존 구독 자체의 사용을0이라고 주장하지 않습니다.

## 4. PASS / FAIL / NOT_VERIFIED

|검증|결과|근거|
|---|---|---|
|이번 변경분 로컬/모의 테스트|**22 PASS / 0 FAIL / 0 ERROR / 0 SKIP**, 40.427초|9종 owner 응답·9종 non-owner 거부, 설정 누락, 비밀 미전달, API 인증 거부, 단발/재시작, 중복·시간초과·quota, 정지/동시2/일요일, 미술 승인 분리|
|PowerShell/Python|PASS|실행 스크립트 구문 오류0, compileall, Git diff 공백 검사|
|실제 CLI 실행 검증|**FAIL1 / 모델 추론 NOT_VERIFIED**|exec1·완료0·bootstrap provider override 거부|
|수정된 CLI 설정 준비안|**CONFIG_ONLY_PASS**|모델 호출 없이 설정 로딩·필수 도구 비활성 확인|
|실제 Discord·추론 성공 응답 전달|**NOT_VERIFIED**|WAITING_FOR_USER_SETUP, 모델 결과 미수신|

기존57 PASS는 역사적 결과로 보존하며 이번22와 합산하지 않습니다. 최초19 변경분 검사를22에 중복 합산하지 않습니다. 기존 Unity/Main23/펫 전수QA 재실행0·Unity 실행0입니다. 모의 Gateway ready 영수증은 실제 서버 연결 근거로 사용하지 않습니다.

## 5. 보호·정책

이번 시작 시 원본의 Git 추적/비무시 미추적 및 Save/UserData 파일을 포함한 **7,356파일 SHA 변경0**, 원본 Git 상태 동일, 원본 HEAD47ad2eec 불변입니다. 이전7,343은 이전 파일럿의 당시 집계이며 이번 스냅샷과 합산하지 않습니다. 게임 제품/Scene/승인 원본/Save 변경0·포커스 조작0·원격 Push/main병합0입니다.

아트 제작→Codex 검수→바울 대표 최종 승인→승인범위 내 적용, 그 뒤에도 Push/merge는 별도 승인입니다. 기술 PASS와 미술 승인 분리·AI 자체 승인 금지를 유지하고, 추론 연결시험 evidence도 미술 승인으로 사용할 수 없게 차단했습니다. SFXAgent PAUSED 유지. 일반 CodexRunner/Discord 자동 AI 실행·아트 제작/게임 적용기는 계속 꺼져 있습니다.

## 6. 파일·브랜치·다음 단계

증거: [모의22](Evidence/DiscordAICollabPhase2_20261023/delta-tests.json), [단발 실제 결과](Evidence/DiscordAICollabPhase2_20261023/probe-result.json), [오류 진단](Evidence/DiscordAICollabPhase2_20261023/probe-diagnosis.json), [설정만 검사](Evidence/DiscordAICollabPhase2_20261023/config-only-check.json), [큐](Evidence/DiscordAICollabPhase2_20261023/queue-receipt.json), [Discord 대기](Evidence/DiscordAICollabPhase2_20261023/discord-preflight.json), [사용률](Evidence/DiscordAICollabPhase2_20261023/account-usage-summary.json), 보호 결과·SHA 목록은 같은 폴더에 보존합니다. SQLite/비밀설정·CLI 원문은 Git 제외입니다.

- 브랜치 **codex/discord-ai-collaboration**, 동일 격리 worktree `F:/study/codex/Project-Limitless/Temp/DiscordAICollab20261023/Worktree`.
- 계약 c7102c5f, 운영도구/테스트 **1390086ad84615daf34916e9ca4830a3fa06e739**, 최종 보고·인계는 이 브랜치 후속 문서 HEAD.
- **LOCAL_COMMITTED / REMOTE_NOT_PUSHED**. GitHub 새 이슈/댓글/PR/배포/본편적용0.
- 다음: 대표가 Portal/로컬 설정 후 bot을 직접 실행→실제 서버9명령·비소유자 거부 영수증 확인. 이 연결 자체에 API 과금 활성화가 필요하지 않습니다.
- 수정 준비안의 **새 단발 추론1회 승인과 구독 잔여량 확인**이 별도로 필요합니다. 기존 실패 장부를 우회하거나 자동 재호출하지 않습니다. 이후 자동 AI worker·일반 제작·제품 적용·Push/merge는 각각 별도 단계입니다.

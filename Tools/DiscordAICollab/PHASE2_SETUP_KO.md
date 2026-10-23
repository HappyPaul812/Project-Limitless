# 바울 대표의 Discord 설정 안내

현재 **WAITING_FOR_USER_SETUP**입니다. 실제 연결은 바울 대표가 아래 설정을 마친 뒤 로컬에서 직접 실행합니다. 토큰을 Codex 대화·GitHub·스크린샷·명령줄에 붙이지 마세요.

## Developer Portal에서 한 번 설정

1. [Discord Developer Portal](https://discord.com/developers/applications) → **New Application** → LIMITLESS 테스트용 앱을 만듭니다. Bot 페이지에서 bot을 확인/생성하고 **Reset Token**으로 토큰을 발급합니다. 일반 Discord 계정 토큰을 사용하지 않습니다.
2. **Installation → Guild Install**을 선택하고 기본 설치 범위에 **bot, applications.commands**를 지정합니다. 권한은 테스트 텍스트 채널 **View Channels, Send Messages**만 부여합니다. Administrator·Manage Server·Manage Roles·Manage Webhooks·Mention Everyone·Read Message History는 필요하지 않습니다. Presence/Members/Message Content 등 privileged intents도 끕니다.
3. 설치 링크로 **대표가 관리하는 테스트 서버**에 초대합니다. 다른 서버가 아닌지 확인합니다. Portal에 Installation 경로가 다르게 보이면 OAuth2 URL Generator에서 같은 두 scope와 두 권한으로 초대 링크를 만듭니다.
4. Discord 사용자 설정 → 고급 → **개발자 모드**를 켭니다. 대표 계정·테스트 서버·허용 텍스트 채널의 메뉴에서 **ID 복사**를 사용해 숫자 ID를 로컬에 준비합니다. 이름/닉네임/서버 관리자 권한만으로 bot 승인 권한을 주지 않습니다.

## 로컬 실행

```powershell
Set-Location 'F:/study/codex/Project-Limitless/Temp/DiscordAICollab20261023/Worktree/Tools/DiscordAICollab'
./connect-discord.ps1
```

이 스크립트는 대표/서버/채널 ID와 **숨김 토큰**을 물어봅니다. 토큰은 이 실행의 환경변수에만 두고 종료 시 원래 환경을 복구합니다. .env 파일 생성·영구 환경변수 설정은 하지 않습니다. PowerShell transcript/화면 녹화를 켜고 비밀을 붙여넣지 마세요. 토큰이 노출되었다면 Portal에서 재발급하세요.

이미 환경변수를 직접 준비했다면 아래 **비밀을 출력하지 않는** 확인 명령만 사용할 수 있습니다.

```powershell
./.venv/Scripts/python.exe -m collab.phase2 preflight
# LOCAL_CONFIG_READY일 때 바울 대표가 직접 실행
./.venv/Scripts/python.exe -m collab discord --data data/phase2
```

필수 설정이 없으면 WAITING_FOR_USER_SETUP입니다. 조건이 맞으면 LOCAL_CONFIG_READY이지만 이는 실제 연결 PASS가 아닙니다. 실행 스크립트가 첫 연결에서 **해당 테스트 서버에 /limitless 명령9종을 등록**합니다. Interactions Endpoint URL/공개 서버/ngrok는 필요하지 않습니다. 이 bot은 Gateway SDK를 사용합니다.

접속 후 로컬 `data/phase2/discord-connection.json`에 Gateway 연결·명령 등록 결과가 남습니다. task·권한·메시지 원문이나 토큰은 여기에 기록하지 않습니다. 실제 명령 응답이 성공한 경우 SQLite audit에 DISCORD_REPLY_SENT_*가 남고, 권한 거부 응답은 DISCORD_DENIAL_REPLY_SENT로 구분합니다. 테스트 콜백에서 만든 모의 receipt는 실제 서버 결과로 사용하지 않습니다.

## 서버에서 확인할 9개 명령

일반 채팅 내용은 읽지 않습니다. 여기서 송수신 확인은 **Slash Command 입력→bot의 비공개 응답**입니다. 명령이 보이지 않으면 bot의 guild 초대, applications.commands 범위, 사용자 **Use Application Commands**, 채널 권한과 숫자 ID를 확인합니다.

|명령|대표 계정으로 확인할 결과|
|---|---|
|/limitless status|업무 상태별 개수. 단발 Codex 검증 결과는 현재 NOT_VERIFIED1로 표시될 수 있음|
|/limitless task|아래 테스트 계약 접수. 실제 AI/아트 제작은 시작하지 않음|
|/limitless pending|승인/검토/결과불명 대기 조회|
|/limitless handoff|접수 직후 NEW 작업은 상태 부족으로 거부하는 것이 정상. 검수 완료 없이 인계 금지|
|/limitless approve|가짜 task/SHA는 거부. 연결 확인을 위해 실제 미술을 승인하지 않음|
|/limitless reject|가짜 task/SHA는 거부. 실제 후보를 임의 반려하지 않음|
|/limitless pause|role=CodexAgent로 신규 시작 정지|
|/limitless resume|같은 CodexAgent만 재개. SFXAgent는 PAUSED 유지|
|/limitless budget|task_limit/day_limit/month_limit 모두0. API/결제 활성화 안 함|

task의 contract 입력 예시:

```json
{"title":"Phase2 Discord 접수 확인","role":"PlanningAgent","issue":null,"base_sha":"de6198480f4021f4aecaae51cbc88c57234b3d77","scope":"DISCORD_CONNECTIVITY_ONLY","protected_assets":["Unity","Save","approved_art"],"inputs":["synthetic test"],"verification":["receive and reply only"],"approval_required":true,"artifact":"NO_ART_ASSET"}
```

별도 테스트 계정도 채널에서 명령을 사용할 수 있게 한 뒤 **대표 ID 목록에는 넣지 않고**, status/approve/pause를 호출하면 모두 거부되어야 합니다. 표시명을 대표와 같게 해도 거부되어야 합니다. 실제 타인에게 토큰·대표 계정 접근권을 넘기지 않습니다. 로컬 모의 테스트는9종 모두의 비소유자 거부를 검사했습니다. 서버 실증은 아직 NOT_VERIFIED입니다.

연결 실패 시 재연결을 무한 반복하지 않고 종료합니다. 서버 초대·토큰·숫자 ID·채널 권한을 대표가 로컬에서 수정한 뒤 다시 실행합니다. SDK 예외 원문은 토큰 노출 방지를 위해 출력하지 않습니다. 상세 원문 로그를 GitHub에 올리지 마세요.

## Codex 단발 검증 현재 상태와 다음 승인

현재 CLI는 설치/구독 인증 방식 확인을 마쳤습니다. 인증 원문/계정 식별자는 보존하지 않습니다. 승인된 CLI 실행1회는 **bootstrap 설정 오류로 종료1, 완료turn0**입니다. 설치버전은 built-in openai provider를 retry=0 설정으로 덮어쓸 수 없습니다. API endpoint/provider를 바꾸는 우회는 하지 않았습니다.

잘못된 provider override는 제거했고, **추론 없는 설정 검사만 PASS**입니다. shell/apps/browser/computer/multi_agent=false를 확인했습니다. 애플리케이션의 단발 장부와90초 상한은 유지하나, 후속 실행에서는 built-in의 유한 transport retry 기본값이 적용될 수 있습니다. 원래 승인 장부를 삭제하거나 DB를 바꾸어 재시도하지 마세요. **새 추론1회에 대한 바울 대표의 별도 승인**과 구독 잔여량 확인이 있어야 새 승인ID를 갖는 후속 작업을 준비합니다. 유료 크레딧 구입/자동 활성화도 하지 않습니다. 일반 CodexRunner와 Discord 자동 AI 실행은 여전히 꺼져 있습니다.

결과는 원문 대신 숫자 사용량·고정 완료표식·오류 분류만 로컬 큐에 저장됩니다. 실제 Discord 업무 큐가 같은 `data/phase2/relay.sqlite`를 사용하므로 서버 연결 뒤 status에서 볼 수 있지만, 지금 Discord까지 전송됐다고 주장하지 않습니다. 단발 추론 결과를 미술 승인에 사용하는 것도 차단합니다.

아트 제작→Codex 검수→바울 대표 최종 승인→승인 범위 내 적용 순서를 유지합니다. 제품 적용·Push/main병합·실제 아트/음성 생성은 이번 연결 확인 명령으로 허가되지 않습니다.

공식 근거: [Discord 첫 bot](https://docs.discord.com/developers/quick-start/getting-started), [권한](https://docs.discord.com/developers/topics/permissions), [OpenAI 인증](https://learn.chatgpt.com/docs/auth), [비대화형 실행](https://learn.chatgpt.com/docs/non-interactive-mode), [CLI 설정](https://learn.chatgpt.com/docs/config-file/config-reference).

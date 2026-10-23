# 아트 파일 수령→격리 검수→대표 승인 요청 자동화 조사 (2026-10-23)

네 역할 실제 인계 파일럿 성공 후 **코드·문서·도구 목록을 읽기 전용으로 조사**했습니다. 아래 경로는 구현 가능성/현재 차이이며 새 watcher/Unity worker/파일수령·이미지제작/미술승인은 실행하지 않았습니다. 실제 파일 전송부터 Unity까지의 end-to-end는 **NOT_VERIFIED**입니다.

## 현재 구현과 부족한 연결

|기능|현재 근거|판정·필요 추가 작업|
|---|---|---|
|Discord ZIP 수령|collab/bot.py:82~105 / 인증된 `/limitless task`의 attachment.read→data/phase2/inbox→inspect_zip→quarantine|코드 존재. 실제 첨부 수령 재시험은 이번 미실행. 독립PNG 접수는 현재 거부, 일반 채널 파일 자동감지도 없음|
|ZIP/PNG 검역|intake.py / SHA256·CRC·경로·링크·압축폭탄·Manifest 파일별SHA·RGBA decode/규격·프레임 계약|재사용 가능. 현재 ZIP32MiB/전체확장128MiB/256entries 및 루트Manifest.json 필수. 기존 납품물의 manifest 형식 호환 확인 필요|
|공유 폴더 감시|inbox 디렉터리 사용은 있으나 watcher/polling 서비스 없음|미구현. 명시적 납품함만 감시하고 완성파일·SHA·중복 이벤트 관리 필요|
|실제 Codex/Unity QA 요청|core.py 큐·인계는 존재, adapters.CodexRunner.run은 거부, MockWorker는 MOCK|일회성 REAL 텍스트 중계와 실제 아트QA worker는 별개. 검역PASS→고정계약QA업무 생성→승인된격리worker 연결 필요|
|화면/애니메이션 비교자료|기존 MinimalDelta65 격리 ArtPilotsDeltaQA.cs의 Camera/Canvas RenderTexture 캡처 및 Bee Idle/Attack 캡처 코드|재사용 기법 존재. 특정 격리경로/대상/사례에 묶였으므로 범용 서비스로 즉시 실행 불가. 실제 새GameView·애니메이션 QA 미실행|
|대표 승인 요청|Discord 텍스트 게시는 이번 실증, `/approve`는 owner ID·SHA·scope·TECH_PASS·실물 재해시 검사|이미지 비교자료·납품SHA·QA근거를 붙이는 자동승인요청은 미구현. 현재 MOCK/REAL_CLI_READONLY 결과는 미술승인 불가. 실제 아트검수 evidence를 갖춘 별도 업무 필요|

봇은 Intents.none이며 on_message 수집기가 없습니다. 모든 채널 내용을 읽는 방식으로 바꾸지 않고 명시적 Slash 첨부 접수부터 확장할 수 있습니다. [Discord 공식 application-command 문서](https://github.com/discord/discord-api-docs/blob/main/developers/interactions/application-commands.mdx)는 ATTACHMENT 옵션을 지원합니다. 이는 현재 코드 경로의 API 지원 근거이며 이번에 파일 접수 end-to-end가 검증됐다는 뜻은 아닙니다.

프로그램의32MiB 한도는 Discord 계정/서버 업로드 한도와 별개입니다. 예를 들어 기존 비교ZIP47,639,471bytes는 현 intake 한도를 넘습니다. 한도를 임의 확대하지 않고 별도 공유폴더 경로나 승인된 분할 납품 계약을 검토해야 합니다. 현재 bot 호출은 official_names를 전달하지 않아 기존 승인파일명 충돌표 연동도 없습니다. 덮어쓰기는 검역폴더로 차단하지만 승인자산 카탈로그와 GUID/meta 보호 검사는 실제 QA 단계에 추가해야 합니다.

## 실제 연결 가능한 공유 경로와 세션 경계

1. **현재 ChatGPT 아트 대화 → 파일 내보내기**: 대표 또는 담당자가 실제 PNG/ZIP을 다운로드한 뒤 지정된 로컬/공유 납품함에 놓거나 인증된 Discord Slash 첨부로 올립니다. 이 첫 내보내기를 자동 수행하는 connector는 현재 구현에 없습니다. 외부 Codex CLI와 기존 ChatGPT 아트 대화창은 같은 세션이 아니며 그 대화의 로그인·파일·생성내역에 접근 가능하다고 가정하지 않습니다.
2. **납품함**: 새로 활성화하지 않은 제안 경로 `F:/study/codex/Project-Limitless/Temp/ArtDeliveryInbox/`. 이 PC에서 실제 파일이 생성된 후에만 relay가 읽을 수 있습니다. 외부 담당자에게 이 Windows 경로를 적어 보내는 것만으로 파일 공유가 되지 않습니다. 원격 제작자는 별도 승인된 공유 서비스/마운트/업로더가 필요하고 현재는 미연결입니다.
3. **Discord 경로**: 허용 owner/guild/channel의 Slash attachment만 SDK로 즉시 받아 원본 바이트/SHA를 로컬 검역에 보존합니다. URL 텍스트 임의 다운로드는 intake.fetch_url에서 차단된 상태를 유지합니다. [Discord 공식 첨부 문서](https://github.com/discord/discord-api-docs/blob/main/developers/reference.mdx#signed-attachment-cdn-urls)에 따라 CDN 첨부 URL은 만료될 수 있으므로 URL만 장기 보관하지 않고 메시지/첨부ID·파일SHA·로컬 실물을 기준으로 관리하는 설계를 권고합니다.

구독 CLI에 앱·도구가 비활성인 현재 텍스트 검토 방식만으로 이미지 제작이나 Unity 검수를 수행할 수 없습니다. 실제 파일 worker가 받아야 할 아티팩트와 실행 권한은 별도 계약으로 분리합니다. 계정 쿠키/대화창 스크래핑·비공개 파일주소 추측·유료API 자동전환은 사용하지 않습니다.

## 다음 구현 순서 제안 — 이번에는 미활성

명시적 납품함/Slash 접수 → 완성된 PNG/ZIP+Manifest 검역 → `delivery:<sha>:<scope>` 고유 업무로 중복 차단 → Codex 격리 QA 요청 → 비교자료/QA결과/SHA 고정 → 바울 대표 승인 요청 → 대표의 명시적 SHA·범위 승인 기록 순서입니다. 독립PNG에는 별도 sidecar 계약과 decode/해시 검사를 적용해야 하며 현재 ZIP-only 경로를 우회하지 않습니다.

감시는 `.part`/임시파일을 건너뛰고 완료 rename 또는 안정된 크기/해시 확인 뒤 처리해야 합니다. 재시작 시 장부에서 중복을 차단하고 실패 작업을 자동 재추론하지 않습니다. 현재 방식의 실사용은 watcher를 켠다고 자동 유료/AI 호출이나 Unity 실행으로 이어지지 않도록 각 worker의 호출·시간·정지·승인 한도를 먼저 계약해야 합니다.

격리 Unity worker는 원본 아닌 새 검수 checkout/임시 Assets와 테스트 전용 Save 경로만 사용해야 합니다. 현재 도구 목록에는 Unity MCP가 노출되지 않아 연결 상태를 검증하지 않았고, Editor/Play는 호출하지 않았습니다. 과거 검수 도구는 아래 경로에서 정적 확인했습니다.

`F:/study/codex/Project-Limitless/Temp/MinimalDelta65/Worktree/Unity/Client/Assets/_Project/Scripts/Editor/ArtPilotsDeltaQA.cs`

SHA256 `0d21a7d6076fccfd20a137ff8c09cbfc0a839de75586139e820c237a808f6d57`. 40행은 특정 Temp/MinimalDelta65/Worktree 및 BatchMode만 허용하고,224~225행은 카메라/Canvas를 RenderTexture로 촬영한 뒤 상태를 복원합니다. 캡처/애니메이션 비교 자동화의 기법은 확인되지만 이 기존 worktree를 변경하거나 실행하지 않았습니다. 새 캡처는 배율·방향·프레임·40/128표시·세화면비·alpha 배경 계약을 고정하고 실제 PNG SHA와 원본프레임 맵을 기록해야 합니다. 기존 QA를 전수 반복할 이유는 없습니다.

대표 승인 요청에는 후보/기존 비교 이미지의 실제 첨부 또는 접근 가능한 공유 실물, 업무ID·원본SHA·기준main·판정·HOLD·미결권리·적용범위를 포함해야 합니다. 로컬경로만 게시하고 원격 열람 가능하다고 주장하지 않습니다. 대표 승인 후에도 본편 적용·Push/main merge는 별도 명시 승인 대상입니다.

현재 판정은 **구현 가능하나 파일 공급자 연결·폴더 감시·실제 Unity QA worker·비교자료 전송 연결이 필요**합니다. 이번에는 문서 조사만 완료했으며 새 파일제작/납품자동수집/Unity실행/승인자동처리0입니다.

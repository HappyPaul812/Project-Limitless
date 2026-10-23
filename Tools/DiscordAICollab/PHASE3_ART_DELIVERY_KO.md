# Phase3 아트 자동 납품 사용 안내

현재 **파일 수령·격리 검사·Codex 검수 요청 등록·Discord 보고**만 실행합니다. 실제 AI/Unity worker와 미술 승인·게임 적용·Push/merge는 실행하지 않습니다. 파일 수령/형식 TECH_PASS는 Codex 검수 PASS나 바울 대표 미술 승인이 아닙니다.

## 납품 위치

- Discord: Limitless AI Team의 **#아트-납품** (`1563179817715638403`). 대표가 Message Content Intent 활성화 및 채널 생성을 완료했고 API에서 확인했습니다. 허용된 대표 숫자 ID의 첨부만 수령합니다. 일반 봇 메시지는 무시합니다.
- Windows 납품함: **`F:\study\codex\Project-Limitless\Temp\ArtDeliveryInbox`**. 이 로컬 폴더는 네트워크 SMB 공유를 자동 설정한 것이 아닙니다. 원격 제작자는 실제 다운로드/복사/허용된 공유 업로더를 통해 전달해야 합니다. 기존 ChatGPT 아트 대화창의 파일을 직접 가져오는 기능은 없습니다.
- 격리 보관함: `F:\study\codex\Project-Limitless\Temp\DiscordAICollab20261023\Worktree\Tools\DiscordAICollab\data\phase3\quarantine\<SHA256>\source.png` 또는 `source.zip`.
- 업무·수령·보고 장부: 같은 도구의 `data\phase2\relay.sqlite`. 검사 상세는 `art_deliveries.report`, 보고 상태는 `art_delivery_outbox`, 서비스 상태는 `data\phase3\service-status.json`입니다.

## PNG와 ZIP 계약

PNG는 `캐릭터.png`와 **`캐릭터.png.manifest.json`**을 함께 전달합니다. Discord에서는 같은 메시지에 두 파일을 첨부합니다. PNG 원본 바이트를 수정할 필요는 없습니다.

```json
{
  "artist_role": "RenewalArtAgent",
  "issue": 63,
  "base_sha": "47ad2eecb40135b2ee28e4947d642ebdbf0bf68a",
  "approval_status": "USER_REVIEW_PENDING",
  "source": "제작자·원본 출처 및 권리 근거 설명",
  "commercial_rights": false,
  "files": [{
    "path": "캐릭터.png",
    "sha256": "실제 PNG의 64자리 SHA256",
    "dimensions": [512, 512],
    "frames": 1,
    "frame_size": [512, 512]
  }]
}
```

예시는 단일 포즈입니다. 실제 시트의 프레임 수·셀 크기로 명시해야 하며, 한 포즈를20프레임 애니메이션이라고 등록하지 않습니다. 신규 아트는 NewArtAgent/#59, 리뉴얼은 RenewalArtAgent/#63처럼 담당·Issue를 적습니다. 후속 Issue도 양의 정수로 전달할 수 있습니다. 기준 SHA가 다르거나 담당/Issue가 모순이면 HOLD입니다.

ZIP은 루트 **Manifest.json**과 PNG/문서/권리 증거 파일을 포함합니다. Manifest.files에 각 파일의 path/sha256을 선언하고 PNG에 dimensions/frames/frame_size를 추가합니다. `source`, `commercial_rights: true`, `license_evidence: "LICENSE.txt"` 및 해시로 검증된 해당 파일이 있더라도 **RIGHTS_HUMAN_REVIEW_PENDING**이며 상업권·미술 승인을 자동 확정하지 않습니다. ZIP의 선택적 `파일.zip.manifest.json`은 담당/Issue/기준SHA 맥락만 보충하고 내부 파일 계약을 덮어쓰지 않습니다.

기존 #63 ZIP은 `file` 키 기반 구 Manifest를 읽을 수 있지만, 누락된 Sprite 규격·프레임·권리 및 미등재파일은 **HOLD**입니다. 기존 원본을 고쳐 맞추지 않습니다. 누락 Manifest도 안전한 파일이면 HOLD 검수 요청을 만들며, SHA 불일치·손상·위험 ZIP은 REJECTED로 차단합니다.

한도는 파일32MiB, ZIP확장합128MiB/256entries/압축비200배, PNG당16,777,216픽셀·전체67,108,864픽셀입니다. Discord 업로드 한도와 프로그램 한도는 별개입니다. 중첩 ZIP·실행 파일·GUID/meta·Assets/Unity/Save 등 보호 경로·링크·경로 이탈·숨긴 후행 PNG payload는 거부합니다. 정적 검사이며 악성코드 완전 무해 인증을 제공하는 것은 아닙니다. 어떤 업로드 파일도 실행하지 않습니다.

## 자동 처리와 상태

3초 간격으로 지정 채널과 납품함을 확인합니다. 폴더 파일·sidecar가 최소3초 안정되고 여러 관측에서 동일해야 하며, Windows 쓰기 핸들이 남아 있으면 수령하지 않습니다. `.part` 파일은 무시합니다. 파일 완료 rename 또는 쓰기 종료 후 처리합니다.

SHA가 이미 있으면 **DUPLICATE**, 새 업무를 만들지 않습니다. 동일 이벤트도 다시 다운로드/게시하지 않습니다. 재시작 후에도 SQLite 장부로 중복을 막습니다. 같은 바이너리에 바뀐 메타데이터만 보내는 경우에도 자동 재승인/덮어쓰기를 하지 않습니다. 보완 내용은 기존 검수 업무에 별도로 연결하는 후속 절차가 필요합니다.

안전한 파일은 **HOLD**, CodexAgent **BLOCKED / NOT_VERIFIED / USER_REVIEW_PENDING** 업무를 만듭니다. 요청에 담당·Issue·격리 파일 경로/SHA·검사 결과·보호 대상·미술 상태·다음 확인 항목을 넣습니다. BLOCKED는 오류가 아니라 이번 단계가 ‘검수 요청 생성까지만’ 허용되기 때문입니다. 기존 MockWorker나 실제 AI 실행기가 자동으로 가져가지 못합니다. `/approve`로 수령 결과를 바로 미술 승인할 수 없습니다.

위험 파일은 REJECTED/업무 등록0이며 격리 원본만 남깁니다. 메시지 전송 성공은 SENT, 결과 불명은 UNCERTAIN으로 남기고 자동 중복 게시하지 않습니다. 소유자·서버·채널이 다른 업로드는 다운로드하지 않습니다.

## 실행·정지

서비스는 현재 백그라운드 실행 중입니다. PC/세션 종료 후 재시작이 필요하며 Windows 자동 로그인 시작 작업은 만들지 않았습니다. 별도 PowerShell에서 다음 명령을 사용할 수 있습니다. 토큰은 기존 DPAPI 암호화 저장소에서만 읽고 채팅/GitHub에 입력하지 않습니다.

```powershell
pwsh -NoProfile -File "F:\study\codex\Project-Limitless\Temp\DiscordAICollab20261023\Worktree\Tools\DiscordAICollab\register-discord.ps1" -Action Phase3Run
```

정지는 다른 PowerShell에서 `-Action Phase3Stop`을 실행합니다. Phase3만 멈추며 기존 Discord 봇·4역할 AI 인계에는 영향이 없습니다. 단일 프로세스 잠금으로 중복 감시를 막습니다. `Phase3Setup`은 설정 점검용이며 기존 비밀을 바꾸지 않습니다. `Phase3LiveTest`는 이미 완료된 전송 시험이므로 일상 운영에서 반복하지 않습니다.

실제 Discord 파일 전송 시험은 **봇 자체 업로드→서버 재수신**으로 표시했습니다. 대표 본인 계정에서 신규 파일을 올리는 경로는 권한·코드·fake SDK 테스트를 확인했으나 아직 직접 관측하지 않았습니다. Windows 감시는 별도 프로세스가 쓰기 완료 후 스스로 수령·등록·보고한 것까지 실제 검증했습니다.

**아트 제작→Codex 검수→바울 대표 미술 승인→승인 범위 정식 적용**을 유지합니다. 다음 실제 AI 호출·Unity worker·정식 적용·Push/main 병합은 각각 별도 승인/지시 대상입니다.

# Phase 3 아트 자동 납품 검증 보고 — 2026-10-23

## 결과

지정 Discord 채널 `아트-납품`(1563179817715638403)과 Windows 납품함을 연결했습니다. 파일 수령·격리 검사·SHA 중복 제거·Codex 검수 대기 업무 등록·Discord 보고까지만 자동화했습니다. 실제 AI 추가 호출 0회, Unity 실행 0회이며 미술 승인·제품 적용·Push·main 병합은 수행하지 않았습니다. 기존 네 역할 VERIFIED 및 자동 인계 코드는 보존했습니다.

신규 로컬 테스트 **45 PASS / 0 FAIL**, 실제 파일 파이프라인 기준 **8 PASS / 0 FAIL**입니다. 선택 재확인 3건은 45건에 중복 가산하지 않았습니다. 기존 AI·Unity 전수 QA는 재실행하지 않았습니다.

## 구현과 보관

- Discord: 대표 사용자·서버·채널 인증 후 첨부만 수령합니다. Message Content Intent 활성과 채널 권한을 확인했습니다. 기존 Slash 봇은 보존하고 별도 REST 감시 서비스를 사용합니다.
- Windows: `F:\study\codex\Project-Limitless\Temp\ArtDeliveryInbox`. 쓰기 핸들 종료와 3초 안정 상태를 확인한 뒤 처리합니다. 같은 SHA와 이벤트를 재처리하지 않습니다.
- 격리 보관: `Tools/DiscordAICollab/data/phase3/` 아래 설정·서비스 상태 및 격리 파일을 관리합니다. 정확한 개별 artifact 경로와 SHA는 [납품 장부](../Evidence/Phase3ArtDelivery20261023/deliveries.json)에 있습니다. 업무는 기존 `data/phase2/relay.sqlite`에 추가하며 기존 업무를 변경하지 않습니다.
- PNG CRC·실제 디코딩·규격·알파·프레임, ZIP CRC·경로 이탈·중복 경로·심볼릭 링크·실행 파일·압축 폭탄을 검사합니다. 압축을 풀거나 파일을 실행하지 않습니다. 정적 검사는 전체 악성코드 검사 인증을 뜻하지 않습니다.
- Manifest·출처·상업 이용 근거·승인·보호 자산 충돌을 기록합니다. 권리나 승인 불명확은 HOLD, 위험한 파일은 REJECTED입니다. 기술적 수령 성공으로 미술 승인을 부여하지 않습니다.
- 검수 요청은 담당·Issue·기준 SHA·파일 SHA·검사·보호 범위를 포함합니다. **BLOCKED / NOT_VERIFIED / USER_REVIEW_PENDING**으로 등록하여 기존 worker의 실행 대상이 되지 않습니다.

## 실제 파일 검증

원본 `F:/Downloads/Limitless_Chapter1_VenomBee_OriginalKeyArt_Pilot_v1_20261023.zip`의 격리 사본을 사용했습니다. 원본 SHA256은 `2e65662d68c77cfb90d7e39d9d40c2cd2816ee33882debe8a8e93ace782bd478`이며 변경하지 않았습니다.

| 실제 경로 | 결과 | Codex 검수 업무 |
|---|---|---|
| Discord 512px PNG 첨부 전송·재수신 | HOLD | L-bf8ebb36f38240c9 |
| Windows 기존 ZIP 사본 | HOLD | L-169ba798cf6646cc |
| 같은 PNG의 다른 이름 사본 | DUPLICATE | 추가 생성 없음 |
| 무해한 경로 이탈 시험 ZIP | REJECTED | 생성 없음·압축 해제 없음 |
| Windows 128px PNG, 쓰기 핸들 7초 유지 후 종료 | HOLD | L-79a30df7eb18488c |

마지막 항목은 시험 코드가 watcher/ingest를 직접 호출하지 않고 실행 중인 백그라운드 서비스가 처리했습니다. 쓰기 중에는 업무가 없었고 종료 후 등록됐습니다. 서비스 정지·재시작 후 중복 업무 없음도 확인했습니다.

Discord 실제 전송·수신은 **봇 자신의 첨부를 명시적으로 SELF_TRANSPORT_TEST로 지정**하여 검증했습니다. 대표 계정이 새로 올린 첨부의 일반 자동 수령은 **NOT_VERIFIED**입니다. 운영 수령은 대표 계정만 허용하며, 임의 봇 첨부를 허용하지 않습니다. 이 제한은 실제 시험을 대표 업로드로 오인하지 않기 위해 기록합니다.

Discord 결과 보고 5건 SENT: `1563180797370826778`, `1563180799954526231`, `1563180802630754326`, `1563180804606267395`, `1563181682582884413`. [영수증](../Evidence/Phase3ArtDelivery20261023/discord-receipts.json), [실제 파일 시험](../Evidence/Phase3ArtDelivery20261023/live-file-test.json), [쓰기 종료 시험](../Evidence/Phase3ArtDelivery20261023/background-writer-close-test.json).

기존 ZIP은 구형 Manifest의 규격·프레임 정보 부족, 불투명 참고 보드, 미기재 부속 파일 및 권리 확인 필요로 HOLD입니다. 파일을 수정하거나 신규 제작하지 않았습니다. 기존 CC BY 및 대표 승인 조건은 해결됐다고 선언하지 않습니다.

## 보호·비용·운영 한계

[보호 근거](../Evidence/Phase3ArtDelivery20261023/protection.json): 원본 **7,356파일 SHA 변경 0·누락 0**, 원본 git 상태 동일, 기존 17개 업무 동일, 승인 장부 동일, usage 추가 0행. AI·Unity 0회, 추가 유료 API 호출·자동 전환·추가 결제 0입니다. AI 토큰 사용은 이번 단계 0입니다.

현재 수령 감시 서비스 RUNNING, AI·Unity worker 비활성입니다. Windows 로그인 자동 시작은 설치하지 않았으므로 PC/세션 종료 후 안내에 따라 재실행해야 합니다. ChatGPT 아트 대화창을 외부 세션과 공유하거나 파일을 직접 가져오지 않습니다. 공유 납품함은 로컬 디렉터리이며 SMB 외부 공개나 네트워크 공유 권한은 자동 설정하지 않았습니다.

승인 순서: **아트 제작 → Codex 검수 → 바울 대표 미술 승인 → 승인 범위 내 정식 적용**. 후속 실제 AI·격리 Unity 실행은 별도 승인/지시가 필요하며, 정식 적용과 Push·main 병합은 각각 별도 명시적 승인이 필요합니다.

## 운영·후속 설계·Git

[한국어 운영 안내](../../../Tools/DiscordAICollab/PHASE3_ART_DELIVERY_KO.md), [격리 Unity Worker 설계](Phase3_Unity_Worker_Design_20261023.md), [신규 로컬 테스트](../Evidence/Phase3ArtDelivery20261023/local-tests.json), [실제 8기준](../Evidence/Phase3ArtDelivery20261023/verification.json).

브랜치 `codex/discord-ai-collaboration`. 구현 커밋 `101435127ccdb50c342f9438a603425cf29521d5`, `82c35d14435d6a8124dda46c1ed8f3d878a6bdd9`, 최종 코드 `be8441e4bdb4769b67b9e925a981bcc4ccc9a283`. 문서·근거는 이 보고서가 포함된 후속 로컬 커밋으로 기록합니다. 원격 Push 및 main 병합 없음.

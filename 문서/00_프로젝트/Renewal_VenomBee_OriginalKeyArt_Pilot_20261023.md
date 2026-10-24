# LIMITLESS — Chapter1 Field01 독침벌 원화 파일럿 v1 (2026-10-23)

Issue #63. 기존 54행 감사, NPC OGA03/09/20 시안, Spider HOLD, 공식 Wolf·Bear·Fox, Chapter2 공식 몬스터/보스, Scene, Spawn, AI, 독, 드롭, 기존 Sprite/GUID/meta, Save/UserData 유지. **main 직접 변경 및 Unity Import 0.**

## 기존 동작 계약(실제 main MonsterDefinition 확인)
- `Unity/Client/Assets/_Project/Resources/MonsterDefinitions/02_VenomBee.asset` monsterId `venom_bee`; Idle GUID `24000000000000000000000000000004`, frame238×215 / columns8 / count10; Attack GUID `24000000000000000000000000000005`, frame315×253 / columns10 / count10, 10FPS, sourceFacesRight1, visualScale0.85.
- Field01 Spawn 3개는 Issue #63 근거. 실제 GUID→PNG 원본 물리 경로 및 원본 license는 아직 읽기전용 역추적하지 못하여 **RIGHTS_SOURCE_PENDING**. PilotBee validation README의 미연결 서술만으로 현행 Sprite 소스를 확정할 수 없음. OGA CC BY3.0 대 ZIP CC BY4.0 충돌. 기존 원본 재배포/복제 없음. **GameView NOT_VERIFIED**.

## 신규 디자인 시안
- 독침벌 고유 1포즈, 검정-황금 갑각·명확한 침·막질 날개·붉은 겹눈; 귀여운 마스코트 스타일 회피. 새로 생성한 콘셉트 일러스트에서 몬스터를 분리해 자동 배경 제거·512×512 RGBA 구성. `VenomBee_OriginalKeyArt_Pilot_v1_512.png`, `VenomBee_Preview_128.png`, `VenomBee_Preview_40.png`, `VenomBee_DesignBoard_Source.png` 납품. 생성 포즈의 크기를 맞춘 **512 캔버스 후보**이며 네이티브 512px 수작업 페인팅 아님. 기존 현행 PNG 실물에 의한 비교나 Ch2 공식 원화에 의한 엄밀한 비교는 **NOT_VERIFIED**. Alpha 자동 분리 경계/실게임 작은 크기 추가 QA 필요.
- ZIP `Limitless_Chapter1_VenomBee_OriginalKeyArt_Pilot_v1_20261023.zip` 2,291,579 bytes / SHA256 `2e65662d68c77cfb90d7e39d9d40c2cd2816ee33882debe8a8e93ace782bd478` / 8파일 / zipfile.testzip CRC PASS. 포함 Manifest.json / SHA256SUMS.txt / SourceAndRuntimeMapping.md / QA.md. ZIP은 이 채팅 다운로드로 제공, GitHub 바이너리 업로드 없음.
- 범위: 1포즈만. Idle10/Attack10 전체20프레임 생성0. 사용자 미술 승인 전 Unity 적용 금지. 다음 게이트: Codex read-only GUID↔실제 PNG meta/license 확인 후 실사 비교, 바울 대표 디자인 승인, 이후 별도 모션 파일럿.

**결과:** `ART_CANDIDATE / USER_REVIEW_PENDING / NOT_UNITY_APPLIED / RIGHTS_SOURCE_PENDING`.

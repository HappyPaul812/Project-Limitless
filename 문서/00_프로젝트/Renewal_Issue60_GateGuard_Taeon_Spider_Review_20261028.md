# LIMITLESS / Issue #60 후속 — 남문 경비 구별성·숲거미 원본 조사 (2026-10-28)

브랜치: docs/art-renewal. 본 결과는 기존 자산의 읽기 전용 확인·참고 비교판 제작이며 사용자 최종 미술 승인 또는 Unity/GameView PASS가 아니다.

## 1. OGA20 남문 경비 × 태온 참고 128px·40px 비교

- OGA20 기존 후보 Eldiran_GateGuard_Concept_512.png, 512×512 RGBA, SHA256 6e52a6f1c9a7e66e915e20652ee6171b7e6b43f2da5beda61bb5568116b2b9a4. 기존 128/40 미리보기·원본 바이트 보존. StableID starter-village-gate-guard / 기존 OGA20 GUID a1200000000000000000000000000008 / PPU28 및 NPC 기능 유지.
- 게임 공식 태온 Portrait 경로 Unity/Client/Assets/_Project/Art/Portraits/Portrait_Taeon.png (2,210,929 bytes, GitHub blob SHA1 46e52ebd13012ffba6255862e1a2054afebfd274). Resources/DialoguePortraitDefinitions/Taeon.asset speakerId=companion_taeon, portrait GUID 0a625cbb0ffbddb41b069ef08ec82a9e. 이번 환경에서 공식 PNG 바이너리 직접 읽기 불가.
- 이번 GateGuard_Taeon_ReferenceScale_Comparison_20261028.png SHA256 a54e405875912119c380c61903e6d01ad4774dccfbfb3cf569df464c65b58656는 OGA20 기존 원본과 과거 Library의 '태온 수호자 픽셀 전투 스프라이트 시트.png' 대기 포즈 참고 컷을 동일 128×128 / 40×40 캔버스에 배열한 잠정 비교판. Library 참고 시트 SHA256 7153993579bb9d5415833cedaef3c1e44f16f12de1cf9b6d3f0b535aa21761c7. 이 시트를 공식 게임 태온 초상화로 확정하거나 실제 Unity PPU 동배율 PASS로 주장하지 않는다.
- 공통점: 짧은 갈색 머리, 은회색 판금, 짙은 파랑 천/망토, 젊은 남성 체형. 차이: OGA20 긴 창·직립 외곽·긴 제복, 태온 참고는 검·방패·유동적인 망토. 40px에서 머리·방어구·색상의 유사성 위험이 높으므로 STYLE_SIMILARITY_REVIEW_HOLD / USER_ART_APPROVAL_PENDING. 전면 재제작0. 공식 Taeon 원본과 실제 동배율·배경에서 추가 검증 필요.

## 2. forest_spider 실제 Sprite·Animation·Spawn 출처

- Unity/Client/Assets/_Project/Resources/MonsterDefinitions/03_ForestSpider.asset 직접 확인: monsterId=forest_spider, animationFramesPerSecond=10, visualScale=0.7, sourceFacesRight=1. Idle/Walk/Attack/Shoot의 ResourcePath는 MonsterAnimations/Spider/{Idle,Walk,Attack,Shoot}. 현행 Idle/Attack SpriteSheet/fileID 및 fieldSprite/AnimatorController는 0이므로 별도 Resources 프레임 파일을 기준으로 추적. 현재 Unity 실재생은 NOT_VERIFIED.
- 실제 GitHub main 파일 목록: Idle 8장(첫 이미지 336×226, 첫 .meta GUID cb000000000000000000000000000100); Walk 8장(359×229, cb000000000000000000000000000108); Attack 8장(325×228, cb000000000000000000000000000116); Shoot 8장(357×236, cb000000000000000000000000000124). 총 32 PNG. 이름 suffix 000,008,016,024,032,040,048,056. 크기는 각 폴더 첫 PNG 헤더 직접 확인; 나머지28개 치수는 미검증. 첫 PNG 4개 .meta PPU256/Pivot(0.5,0.5)/Point Filter 확인. 열거한 GUID는 Sprite meta GUID, Git Blob SHA1이나 SHA256으로 혼동 금지.
- Field02_ForestSpider_01~04 Spawn asset 4개 확인; 01은 Field_02 및 monster GUID b33f06912b2a48d0afcd626d1d88fc03. Field03_EntranceSpider.asset 역시 같은 GUID 연결. 정적 Spawn 데이터 연결은 사실이나 실제 전 장면 노출/플레이는 별도 QA.
- 원본 저장소 Unity/Client/Assets/ThirdParty/ethanf30games/2DSpider/README.md: 작가 ethanf30games, 작품 2D Spider, 원본 Spider Idle/Walk/Attack/Shoot.zip 각 62프레임 기록. 공식 OpenGameArt https://opengameart.org/content/2d-spider 라이선스 CC0 확인. 이전 README의 'Scene/전투 미연결'은 최신 몬스터 정의/Spawn 참조와 충돌하는 오래된 설명. 원본62와 현행8의 바이트·픽셀 동일, 런타임 로딩, 공식 사용자 미술 승인 기록은 아직 미확인: SOURCE_CC0_IDENTIFIED / ACTIVE_SPAWN_DATA / ART_APPROVAL_UNKNOWN / NEW_SPIDER_ART_HOLD.
- 향후 별도 승인 시 기본 호환 목표: Idle/Walk/Attack/Shoot 각각8프레임, 10FPS, 우향, 현재 각 포즈 캔버스/PPU256/중앙 Pivot/알파/루프를 최대한 보존하고 40·128px 식별성 및 실루엣을 QA. 원본32 PNG 전수 규격·원본 ZIP·런타임 frame loader·기존 공식 승인 여부를 Codex 읽기전용 감사한 후 확정. 신규 PNG/애니메이션 제작0.

## 3. VenomBee·기존 공식 자산 보호

- VenomBee 기존 좌향 1포즈 TECH PASS, 현행 우향 Idle10+Attack10/10FPS 교체 ART_HOLD. 신규 키포즈·20프레임0. 2DPIXX PilotBee OGA CC BY3.0 대 원본 LICENSE CC BY4.0 버전 충돌 미해결. 이전 v1 ZIP·rights/motion 계약 그대로 보존.
- Wolf/Bear/Fox, Chapter2 공식 몬스터/보스/환경, OGA03·OGA09·OGA20 기존 시안, 모든 NPC ID·E키/Quest·Collider·Sorting, MonsterDefinition·독/드롭·AI·Spawn, Save/UserData/GUID/meta/Unity Editor/원본 프로젝트/main 변경0.

## 4. 파일 납품·SHA 및 후속 게이트

- 결과 ZIP Limitless_Issue60_GateGuard_Spider_Review_20261028.zip, 263760 bytes, SHA256 064c1ecee4a01c7c8826c2b8763d2cb1765b334ca7cb1376631395c2f6c03905, 6 entries, ZIP CRC PASS. 포함: 참고 비교 PNG, ForestSpider_And_GateGuard_Audit_20261028.md, ForestSpider_Frames_20261028.csv, QA_README.md, Manifest.json, SHA256SUMS.txt. ZIP/PNG는 채팅 결과 파일이며 GitHub 바이너리 업로드0, 사용자 Windows F: 자동 복사0.
- 다음 Codex 안전 조사: 공식 Portrait_Taeon.png 실제 바이트와 OGA20을 같은 GameView·PPU·Pivot·128/40px 크기로 비교; 원본 Sprite SHA/시인성 자료 전달. Spider의 전체32 PNG 및 원본 CC0 ZIP 4개와 런타임 FrameLoader/Scene Spawn/기존 승인 여부를 읽기전용으로 확인. 자동 전수 QA·게임 적용·원본 변조 금지.
- 정확 상태: REFERENCE_BOARD_DELIVERED / OFFICIAL_TAEON_COMPARISON_NOT_VERIFIED / SPIDER_CC0_SOURCE_VERIFIED / SPIDER_STATIC_USAGE_MAPPED / SPIDER_ART_APPROVAL_UNKNOWN / NEW_ART_ZERO / USER_ART_APPROVAL_PENDING / MAIN_UNCHANGED.
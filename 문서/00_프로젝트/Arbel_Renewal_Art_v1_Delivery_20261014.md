# Arbel 마을 오리지널 리뉴얼 아트 v1 납품 — 2026-10-14

**담당:** Chapter 1·2 기존 아트 리뉴얼 / **작업 브랜치:** `docs/art-renewal`.
**상태:** `ART_DELIVERED / USER_ART_REVIEW_PENDING / UNITY_NOT_APPLIED / RUNTIME_NOT_VERIFIED`.
**기준 main:** `bddca540d2fdd9e93f642f8d0f387b34730bd2d8` (PR #10 병합 확인).

## 최신 정본·원본 보호

- [최신 Sprite 복구 QA](Sprite_Reference_Recovery_Final_QA_20261014.md), [이전 EOD 통합 이력](EOD_Integration_Final_QA_20261013.md), [Chapter2 오리지널화 로드맵](LIMITLESS_Chapter2_Original_Art_Roadmap_20261012.md), 팀운영 공통·역할/인계를 확인.
- 실제 `Unity/Client/Assets/_Project/Scenes/Arbel.unity`의 **직렬화 SpriteRenderer 284개**를 분석: Grass247 / Path16 / 건물8 (Roof2·Wall4·Door2) / 울타리4·나무3·수풀2·바위1·화톳불1·통1·상자1 등 총21. 격리 QA 문서의 **Runtime Renderer 295**는 동적 오브젝트를 포함하므로 별도 관리.
- 복구 자산 `_Project/Art/Environment/SpriteReferenceRecovery20261014`의 **원본 PNG11/Sprite123**은 다섯 Scene의 복구 자산. Arbel만 123종이라는 뜻 아님. 이번 제작은 이 원본 PNG/신규 GUID/meta/Scene을 **수정하거나 삭제하지 않음**.
- 실제 Arbel 원본 scene Git blob SHA `a5a765b3122fc37650a974f3aa832c18bc423ba5`. 현재 저장소와 로컬 Editor/정식 적용은 구분.

## 납품 파일 / 실사용 자산

- ZIP: `Limitless_Arbel_Renewal_Art_v1.zip`. ChatGPT 다운로드에서 사용자가 `F:\Downloads`로 저장.
- ZIP SHA256: `ec707452169aa99ec297a7ac23a14928d4a0123b4b7b051d17ddcb282953d626`.
- 파일 크기: **3,145,099 bytes**; 총 **47파일 / PNG40**.
- 신규 Unity용 환경 Sprite **34 PNG**: **기존 Renderer 교체 후보 23종** (`Ground/`6, `Road/`2, `Buildings/`6, `Props/`9), **배치 선택용 장식 11종** (`Optional_Service/`). 나머지 PNG6은 Preview5 / Seam QA1.
- 추가 내용: `Manifest.json` (PNG별 SHA256·크기·PPU128·center·alpha bbox·용도), `README.md`, **`Renderer_Mapping_284.csv`**, `Optional_Service_Placement.csv`, `Codex_Integration_Handoff.md`, `Rights_and_Provenance.md`, `QA/Offline_Validation_v1.json`.
- Preview는 **기존 씬 오브젝트의 좌표를 기준으로 배치한 독립 합성 이미지**이며 **실제 Unity RenderTexture가 아님**. 구(舊) 모습 비교 패널은 원본 게임 화면이 아닌 **기존 구조 모식도**. 새 복구 PNG의 실제 Runtime 미술 비교는 승인 후 Codex 현장 촬영이 필요함.
- 독립 제작 procedural pixel-art 선언 포함. 외부 에셋 픽셀 사용0이나 독립적인 최종 상용권리 감사 **NOT_VERIFIED**.

## 시각·기능 대응

- Field04 v2의 부드러운 올리브/황갈색 대지 톤을 계승하되 나무·석재·따뜻한 기와로 안전한 서부 보급 마을 구현.
- 실제 Grass 247개 모두 19×13 정본 격자의 Sprite-only 제안. Loam/건조한 흙/중앙 광장 돌바닥 6변형은 동일 외곽 6px 접합 기준으로 생성. 기존 PathLeft8/PathRight8은 남북형 마을 중앙길의 기존 `x=±0.5,y=-6..1`을 유지.
- 기존 **두 건물** Roof2/Wall4/Door2와 Fence4/Tree3/Bush2/rock/camp/barrel/chest 실제 역할에 정확히 대응. 제안 Roof는 원래 128px에서 384×192px로 커져 가림·정렬 및 실제 화면 검증 필수. 나무/바위/수풀은 원래 recovered Sprite.rect와 후보 native footprint 차이가 있어 별도 접지 검증 필수.
- 실시간 기능 고정: 레온(0,3), 상점(4,2), 치유사(2,-2), 파티(-3,-2), 펫(5,-3)/(7,-3), 서쪽 주민(-5,2), Portal 관리(-7,-3), PortalAnchor(-7,-4), Main14 옛 우물(-3,3), SafeZone 리스폰(0,-1.5), Main13 입구(7,0), Main21 원조 지점(-2.2,-1). NPC들은 직렬화 환경 Renderer가 아니라 주로 `Chapter2IntroFlow` 등 런타임으로 생성하므로 아트/기능 경계 보호.
- 실제 외곽 연결: 동쪽 Field04 Spawn(8,0)/Exit(10.7,0), 서쪽 Field05 Spawn(-8,0)/Exit(-10.7,0), 북서쪽 Field06 Spawn(-6,5)/Exit(-6,7.7). Portal은 현재 준비 중으로 실제 Fast Travel 개방 금지.
- **Portal 장식은 배치 보류:** PortalAnchor(-7,-4)와 기존 TreeSouthWest(-7,-4.2) 공간이 겹침. `Optional_Service` 아트 PNG 자체는 납품하되 합성 Preview에도 일부러 놓지 않았으며 실제 배치/나무 재배치를 승인 없이 지시하지 않음.
- 기존 Sprite 복구·Field04 공식42·기존 캐릭터/NPC/Quest/Collider/Save/TTS/BGM/Scene/Prefab/GUI/meta/GUID **모두 무변경**.

## 검증·사용자 승인 / 다음 작업

- 로컬 제작 이미지40개, 개별 SHA·Manifest, 압축 CRC, `Renderer_Mapping_284.csv` 284행/각 Sprite 이름+좌표 중복0, 모든 6종 Ground 외곽 일치의 **오프라인 제작 QA PASS**.
- 실제 Unity Import, Renderer Anchor/Sorting, 확대접지, 휠체어 통로, Portal/마차 연출, 23 BuildScene Sprite누락, SafeZone/Quest/UI/Save/Continue 회귀는 이 제작 단계에서 **NOT_VERIFIED**.
- **지금 사용자 미술 승인 필요**: 신규 교체 아트23종 승인 여부, 추가장식11종의 실제 사용과 개별 배치 여부, 건물 지붕 크기, 기존 마을의 따뜻함·가독성. 승인 전 Unity 원본 적용 금지.
- 승인 후 Codex가 최신 main을 별도 worktree에서 확인하여 새 자산만 프로젝트 전용 경로에 Import하고, 기존 복구 원본과 기능을 보호하면서 개별 Scene Sprite만 교체 후 격리 Runtime QA 및 실제 RenderTexture 생성.
- **Git 운영:** 이 문서는 `docs/art-renewal` 브랜치 전용. main 직접 Commit/Push/PR 병합/force Push 실행0. ZIP은 ChatGPT 다운로드로 전달되며 Git에 대용량 아트 파일을 등록하지 않았음.

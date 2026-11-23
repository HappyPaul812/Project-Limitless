# LIMITLESS / 리뉴얼 아트 / Issue #124 후속 — 길 특성5 GUID 대조·시각 1종 파일럿 납품 (2026-11-23)

## 1. 작업 범위·실제 파일
- 바울 대표 후속 지시: 기존 길 공식 Path Symbol5와 Game-icons.net trait5, 전리품 아이콘4의 참조를 먼저 대조. 신규 파일럿은 **시각 trait 1종만**. 승인된 NPC/몬스터/펫/길 문장과 Unity 프로젝트 보호.
- 후보 이미지(신규 ChatGPT 생성 시안 `비전_타깃_아이_아이콘.png` 원본 1254×1254 RGBA → 중심 crop → LANCZOS 정규화): **`VisionTrait_Original_Pilot_512_RGBA.png`** 512×512 투명 RGBA, SHA256 **`9a903a53dacc8190e35c2ae77cd034f4c92fef00c93a9e933639fc81cf024232`**. **`VisionTrait_Original_Pilot_128_RGBA.png`** 128×128 투명 RGBA, SHA256 **`080cb11af9ccd93325a1aaed9c9e63df4bcc64d2ce93dace3434143f873e283a`**. **`VisionTrait_Preview40_RGBA.png`** 40×40 SHA256 `1b98ab21059f4b5c97901f24c30267919c22960e6c400f1a44552264849d4eee`. 원화 **정확히1개**, 추가4개 원화 제작0.
- 실물 미리보기 `Review/Vision_512_128_40_ActualPixel_Review.png` SHA256 **`715a44b476286a7881785b6dd85110fabb4f95f2cb4f97ba6bc6a860f5e31c1f`**. 후보 512·128·40의 실제 픽셀만 전시. 40px는 첫 육안 파일럿이며 게임 UI 검증 아님.
- 길 특성5 대조 보드 `Review/Path5_GUID_Comparison_Board.png` SHA256 **`cdea9d133d29e6e2c80b40f62a00a1f4dd21f73845e5980547df5cbe65724c43`**. 기존 `PathDefinition`의 **실제 연결 GUID·파일명·저작자/라이선스**와 신규 시각 파일럿 표시. **중요: 원격 GitHub의 기존 공식 Symbol5·외부 trait5 이미지 원본 바이너리 실물은 이번 작업환경에서 가져오지 못하였으므로 기존 원화의 픽셀 썸네일을 보드에 실었다고 주장하지 않음.** 검증된 구조 대조판이지 실제 기존 이미지 픽셀 비교 PASS가 아니다. Codex 로컬 원본 512/128/40 비교는 후속 판정 필요.
- 별도 검수·교체 계획: ZIP 내 `Docs/Path_Trait5_Replacement_Plan.md`, `Docs/SourceDeclaration.txt`, `Docs/QA.json`, `Manifest.json`, `SHA256SUMS.txt` 제공. 기존 승인 파일이나 생성된 시각 파일럿 이외 PNG/그래픽을 사용하지 않음.

## 2. 현재 길 5종 계약/기존 자체 제작 문장 보존 (11/19 복구 ref `f64082914e114096b6b9393263e7a36f7d829a55`)

| 길 | 기존 Game-icons.net 외부 trait | trait GUID | 기존 자체 제작 `pathSymbol` GUID | 이번 대상 |
|---|---|---|---|---|
| 시각 | `Path/White/eye-target.png`, Delapouite | `c94e7ffd3ad84d729b36c90c4ca0f80e` | `b7f20eef45a243ee98b8fb12d89672f0` | **시각 고유 trait 파일럿 1종** |
| 청각 | `Path/White/sound-waves.png`, Skoll | `e54a40b325884dfe94b051148ef04d14` | `a6e19ddf349142dd87a7ea01c78561f0` | 미제작 |
| 지적 | `Path/White/mesh-network.png`, Delapouite | `b5ddb75c1df7465fb0494dfd5c76a6e7` | `e88d6e7ce8a84846a334279fd09eecea` | 미제작 |
| 지체 | `Path/White/cog.png`, Lorc | `765c9dee6e494a2cbc30f76d637b5066` | `c8031ff056b344ffa9c90c23e9a783f0` | 미제작 |
| 마음의 상처 | `Path/White/heart-shield.png`, Delapouite | `ecc84d2dd627400cb76d063c617fff46` | `f4eabf7b484442698c961bbd8fe7d252` | 미제작 |

- `Unity/Client/Assets/_Project/Resources/PathDefinitions/01..05*.asset`에서 **`pathSymbol`≠`traitIcon` 직접 확인**. 원래 공식 문장 5개는 고유 자산이므로 그대로 **재사용/KEEP**, trait 5개만 추후 고유화 후보. `UI/PathSelectionController.cs`는 공식 문장·특성 아이콘을 별도 Sprite로 렌더. 기존 공식 Path Symbol을 임의로 재작화 또는 trait 역할에 강제 재사용하는 방식 금지.
- 기존 Game-icons.net의 `Path/White/` 다섯 파일은 CC BY 3.0. 실제 게임 교체 전까지 원본/크레딧(Delapouite·Skoll·Lorc) 보호. 기존 Path/Black5는 이 지정 슬롯과는 다르며 전체 미사용 판단 금지.
- 기존 자체 제작 trait 전용 승인 대체본을 11/19 복구본의 5 `traitIcon` 참조에서 찾지 못했음. **로컬/모든 브랜치/원화 라이브러리 전체의 부재 증명은 아님**. 기존 이미지 실물 비교도 미완료.

## 3. 전리품 4종 중복 제작 보호
- `_Project/Resources/ItemDefinitions/GrassSlimeGel.asset` → `GameIconsNet/Material/White/dripping-goo.png` GUID `40b87c1443b1efa4f82e58cbfd197337`.
- `VenomBeeStinger.asset` → `wasp-sting.png` GUID `73b52f1f2998f6646a226eed86602ed4`.
- `ForestSpiderSilk.asset` → `spider-web.png` GUID `fa18969478fc18e4c9f6dc15069b6999`.
- `VenomSnakeScale.asset` → `dorsal-scales.png` GUID `aeb1ca89347241c4289e2bbb53e91c8a`.
- 외부 자산 사용/ItemDefinition 매핑 조사만 실시. 추가 전리품 PNG 0, `ItemDefinition` 변경0. Chapter2 자체 승인 Loot·#96 HP/MP·기존 물약 원화 재제작0.

## 4. 교체 승인 단계 (지금은 적용 금지)
1. **실물 디자인 검증:** Codex가 로컬 공식 Symbol5·Game-icons trait5·공식 전리품/자체 승인 후보를 읽기 전용으로 확보해 동일 실제 512/128/40 크기와 실제 UI 30/40px에서 비교 후 디자인 중복 여부/톤·명암·가독성을 판정. 본 보드의 `GUID_COMPARE_ONLY`를 실물 아트 PASS로 오판 금지.
2. **대표 미술 승인** 후 시각 후보의 색상/대비/테두리 픽셀 최소 보완 여부 결정. 이후 다른 4종의 기승인 자체 원화 유무 재확인하고 필요한 것만 별도 1종씩 제작.
3. **제품 교체는 별도 Codex 승인**: 시각 후보 전용 신규 Sprite/.meta 생성 후 `PathDefinitions/01_VisionPath.asset:traitIcon` 참조 **1곳만 최소 교체**; 기존 `pathSymbol`, GUID/meta 원본, 퀘스트·길 역할·캐릭터·저장 데이터 불변. 초기 이미지 크기/피벗·UI 배치·Tint·키보드 및 마우스 입력/세이브 회귀 DeltaQA 필수.
4. 전리품 4종은 길 특성 5종 뒤 별도 승인 발주. 사용 중인 Game-icons 크레딧은 모든 참조 교체 확인 전 삭제 금지.

## 5. ZIP 검증/결론
- **파일럿 전체 ZIP:** `/mnt/data/LIMITLESS_Issue124_Trait5_VisionPilot_20261123.zip`; **512,911 bytes**, SHA256 **`f8d0d09986131cf68d19d72a2b6546fe4324583758777e06a2c80b53d11e3a9c`**, **10 entries**, ZIP CRC `testzip() None` PASS, Manifest payload8/8 SHA256+bytes PASS. SourceDeclaration 포함. 단축 직접 PNG `/mnt/data/VisionTrait_Pilot_128.png`와 `/mnt/data/VisionTrait_Pilot_512.png`은 ZIP 내부 후보와 바이트 동일.
- **상태:** `ART_PILOT_1_READY / GUID_REFERENCE_COMPARISON_COMPLETE / EXISTING_PNG_PIXEL_COMPARISON_PENDING / GAMEVIEW_NOT_VERIFIED / USER_ART_REVIEW_PENDING / NOT_UNITY_APPLIED`. 신규 아트 후보 **1종**, 공식 Symbol5·외부 Icon5 및 전리품4의 실제 Unity 원본 PNG/GUID/meta/PathDefinition/ItemDefinition/씬/코드/Runtime **변경 0**.
- Unity **6000.6.5f1** 고정, Issue **#85 HOLD**, Save/UserData, Discord, 기존 승인 NPC/몬스터/펫/불씨망령 및 모든 공식 원화 보존. 문서 커밋만 `docs/art-renewal`, `main` 직접 커밋/Push/Merge 없음. 사용자 게임 PC 로컬 동기화·Unity Play 실행/게임 적용0.

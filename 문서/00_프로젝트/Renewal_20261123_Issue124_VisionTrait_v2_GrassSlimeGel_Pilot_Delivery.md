# LIMITLESS / Issue #124 후속 / 시각 Trait v2 국소 보완 + 풀슬라임 젤 1종 파일럿 (2026-11-23)

## 근거 · 기존 승인 자산 보호

- 담당 `docs/art-renewal`, [Issue #124](https://github.com/HappyPaul812/Project-Limitless/issues/124) **최신 2026-11-23 기획 후속 지시 및 Codex 실물 비교 접수**.
- 읽은 결과: [Codex 실제 PNG 동배율 비교 댓글](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7088308591), [기획 v2 권고](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7088337969), [당일 작업 배정](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7089569833), `WORLD_ART_RENEWAL_LATEST.md`.
- Codex 실제 로컬 원본 비교: 공식 Symbol5 + 외부 Trait5 GUID 대조 PASS, **기존 v1 40px PASS, 30px 부분 가독성, 24/26px 동심원/표식이 눈보다 우선**. 실제 파일 `Vision_Trait_Actual_Pixels_OFFLINE.png`은 Codex 미Push 로컬 브랜치에만 있고 이번 작업환경에 실물 없음. **Codex 분석 결과 재사용**, 새 원본 로컬 비교를 한 것으로 주장하지 않음.
- 실제 v1 소스 **바이트 검증**: `/mnt/data/VisionTrait_Pilot_512.png` 512 RGBA SHA256 `9a903a53dacc8190e35c2ae77cd034f4c92fef00c93a9e933639fc81cf024232`; `VisionTrait_Pilot_128.png` SHA256 `080cb11af9ccd93325a1aaed9c9e63df4bcc64d2ce93dace3434143f873e283a`. 기존 v1 ZIP/PNG **BYTE_KEEP**.
- `Unity/Client/Assets/_Project/Resources/PathDefinitions/01_VisionPath.asset:traitIcon`은 Game-icons.net `eye-target.png` GUID `c94e7ffd3ad84d729b36c90c4ca0f80e`; 공식 자체 `pathSymbol` GUID `b7f20eef45a243ee98b8fb12d89672f0`는 전혀 다른 슬롯. **공식 Path Symbol 5종 및 나머지 Trait4는 모두 원본 유지**.

## A. 시각 Trait v2: v1 원형 보존·작은 크기 가독성 보완

- **실제 후보 PNG**
  - `PNG_Originals/VisionTrait_v2_Master512_RGBA.png`: 512×512 투명 RGBA, SHA256 **`c86d5ab634d2a9bab855222335f2583275975ab070454a568cb53104915a4072`**.
  - `PNG_Originals/VisionTrait_v2_128_RGBA.png`: **128×128 투명 RGBA 실사용 후보**, SHA256 **`6b4fcdc9897d712b2490dc14dbe8ea59abc30876738aa2f7443e5effc55aacb1`**.
  - `Review/Vision_v2_{40,30,26,24}px.png`: 512 마스터로부터 LANCZOS 실제 크기 축소. `Review/Vision_v1_{30,26,24}px.png`도 같은 v1 마스터에서 생성.
- **v1 기반 타겟 수정**: 외곽 원형 장식의 일부 청색 아크만 불투명도 낮춤, 중앙 눈 내부 어두운 영역 대비/청색 하이라이트 소폭 강화, 기존 흰 동공 중심 약 38px 반경의 **작은 국소 영역을 짙은 네이비 동공+작은 하이라이트로 전환**. 눈의 전체 방향·타원 실루엣, 기존 4방 문양, 전체 냉청색 색감과 v1 바이트 원본은 보존. 512의 **20.451% RGBA 픽셀 값 변경**(대부분 외곽 반투명 조정·상대적으로 작은 색상 변화 포함), 전면 원화 재생성/교체 없음. 새 이미지 생성으로 전체 v1을 바꾸는 시안은 사용하지 않음.
- `Review/Vision_v1_v2_512_128_40_30_26_24_Board.png`: v1/v2를 512·128·40·30·26·24 기준 표시. **512는 판에 맞춘 74% 표시로 명시**, 128 이하 슬롯은 1:1 픽셀 크기. 실제512 원본은 별도 파일. `Review/Vision_v1_to_v2_Delta_Overlay.png`: v1/v2/512 변경 좌표 강도. 보드의 실물 v1/v2는 원본에서 직접 렌더.
- **판정**: `V1_40PX_CODEX_PASS_REFERENCE / V2_OFFLINE_SIZE_PREVIEW_CREATED / 30_26_24_TARGETED_CONTRAST_CANDIDATE / LIVE_UI_READABILITY_NOT_VERIFIED / USER_ART_APPROVAL_PENDING`. 24/26px에서 눈이 표식보다 먼저 보이는지 대표 미술 판단 필요. *실제 Unity 30px 슬롯*에서 PASS 주장 금지.

## B. Chapter1 GrassSlimeGel 128px 오리지널 파일럿

- 원격 복구 참조 `f64082914e114096b6b9393263e7a36f7d829a55`의 `Unity/Client/Assets/_Project/Resources/ItemDefinitions/GrassSlimeGel.asset` **실물 YAML** 확인:
  - `itemId: material_grass_slime_gel`, 표시명 슬라임 점액, `icon: {fileID: 21300000, guid: 40b87c1443b1efa4f82e58cbfd197337, type: 3}` = **`Assets/ThirdParty/GameIconsNet/Material/White/dripping-goo.png.meta`**와 직접 GUID 일치.
  - `iconTint: {r: 0.55,g:1,b:0.38,a:1}` **변경 금지**. `_Project/Resources/MonsterDefinitions/01_GrassSlime.asset` lootID가 `material_grass_slime_gel`이며, `InventoryPresenter.BuildItemRows`가 `item.Icon, item.IconTint` 사용. 따라서 단순 폴더 보관 아님.
- **실제 후보 PNG**
  - `PNG_Originals/GrassSlimeGel_Original_128_RGBA.png`: **128×128 투명 RGBA**, SHA256 **`ca06a5384883e0c848c4a2e893f5f524ee0d81ef22a0420425f3c0c58a902a55`**.
  - `PNG_Originals/GrassSlimeGel_Original_Master512_RGBA.png`: 512×512 편집/보존 원화 SHA256 **`7d838c7005837e60178bbcdcaf5265fa309b67c88c78d6a25bb75dc6ce3e4816`**.
  - `Docs/GrassSlimeGel_Original_VectorSource.svg`: 독립 Bézier 외형·음영·젤 광택·작은 점액 방울을 **처음부터 직접 제작**한 생성원천, 외부 Game-icons 픽셀·슬라임 몬스터 원화·기존 승인 Ch2/3 잡템 PNG 복사0.
- 반투명 녹색 **비인격성 점액 덩어리**(눈/입 없는 원료), 큰 윤곽과 강한 광택, 어두운 녹색 외곽선으로 재료로 보이도록 디자인. `Review/GrassSlimeGel_128_40_30_Inventory_Readability.png`: 실제 128·40·30 축소를 어두운 인벤토리 **모의 슬롯**에 배치; 위 행 원본, 아래 행 **기존 iconTint(0.55,1,0.38) 시뮬레이션**. 모의 화면일 뿐 Unity 실제 GameView 아님.
- **중요한 비교 제한:** 기존 `dripping-goo.png`는 .meta/GUID 및 로컬 Codex 조사 결과로 실사용 확인되지만, 기존 PNG 바이너리 실물은 이번 환경에서 취득하지 못해 직접 화면 픽셀 대조 **NOT_VERIFIED**. Ch2/Ch3 공식 아이콘도 이번 환경에 원본 PNG가 없으므로 화풍 실물 Side-by-Side **NOT_VERIFIED**. 밝은 반투명 아이콘/어두운 윤곽선으로 화풍 통일 의도는 반영하되 **최종 스타일 PASS 주장은 하지 않음**. 다음 Codex의 읽기 전용 동배율 비교로 판정.
- 나머지 전리품3 (`VenomBeeStinger`, `ForestSpiderSilk`, `VenomSnakeScale`) 및 다른 길 Trait4 **신규 제작0**, 기존 Game-icons.net 원본 CC BY3 저작자 표시 유지.

## 납품·검증

- 전체 ZIP **`/mnt/data/LIMITLESS_Issue124_VisionTrait_v2_GrassSlimeGel_Pilot_20261123.zip`**, **893,138 bytes**, SHA256 **`9940b54a3fa4828d745470a580be4a952c5e0b3ab5431ddc4f322fa3a5560bff`**, **23 entries**, ZIP CRC `testzip() == None` PASS, **Manifest 21/21 SHA256 & bytes PASS**. `SHA256SUMS.txt`, `SourceDeclaration.md`, `README_DELIVERY.md`, `QA.json`, 원화 PNG4·축소 프리뷰·비교판·Slime SVG 원천 포함. Manifest는 순환 검증을 피하도록 본인/검증 SUMS 제외.
- 게임용 PNG 두 장만 ZIP **`/mnt/data/LIMITLESS_Issue124_2PNG_GameCandidates_20261123.zip`**, SHA256 **`8af188413faa53418ffe1bf073fda2d0e2891d6db0efd3482367eb9868a255d8`**, 2 PNG, CRC PASS.
- **파일 기술 PASS**: 두 128 PNG가 RGBA 투명 배경, 치수 128², 투명 영역 존재, 코드 기준 아이템 슬롯 및 40/30 축소 시각 검토 보드 생성. GameView/실제 구동/감산 색상·배율·실전 행동 및 제품 UI 클릭 QA 미실시.
- **승인 게이트**: `VISION_V2_ART_REVIEW_PENDING / GRASSSLIMEGEL_PILOT_ART_REVIEW_PENDING / NO_UNITY_APPLY / CURRENT_GUID_META_ASSET_UNCHANGED`. 사용자 미술 승인 후 Codex가 기존 Sprite/Source GUID/meta 덮어쓰기 없이 신규 Sprite 별도 등록하고, 정확히 `PathDefinitions/01_VisionPath.asset:traitIcon`과 `ItemDefinitions/GrassSlimeGel.asset:icon`의 참조만 최소 변경하고 인벤토리 tint·가독성·안전거점 UI 입력/Save 회귀 검사. **이번에는 구현 승인 없음**.

## 보호

- Unity **6000.6.5f1** 고정, Issue **#85 HOLD**, 원격 `main` 직접 변경0.
- 기존 `PathSymbol` 5종/길 특성 나머지4종/기존 Game-icons.net 원화/GUID/meta/PathDefinition/ItemDefinition, Ch2·3 공식 전리품/물약, NPC/몬스터/펫/환경, 기존 Scene/Prefab/Resources/Runtime/Save/UserData/Discord 모두 **변경0**.
- `docs/art-renewal`의 문서 보고·인계·Issue 댓글만 갱신, 신규 바이너리 원화는 ChatGPT 작업환경/ZIP으로 납품, Windows `F:\Downloads` 로컬 자동 복사/Unity 적용 없음.

# LIMITLESS Arbel 환경 리뉴얼 v2 — v1 최소 보완 납품 (2026-10-14)

**담당 브랜치:** `docs/art-renewal` / **기준 main:** `bddca540d2fdd9e93f642f8d0f387b34730bd2d8`.
**상태:** `ART_DELIVERED / OFFLINE_QA_PASS / USER_ART_REVIEW_PENDING / UNITY_NOT_APPLIED / RUNTIME_NOT_VERIFIED`.
기획 담당 [NEEDS_MINOR_FIX 인계](https://github.com/HappyPaul812/Project-Limitless/blob/docs/planning/문서/00_프로젝트/Arbel_Renewal_v1_MinorFix_QA_Handoff_20261014.md)를 확인하고 세 항목만 보완했습니다.

## 납품물

- 파일명: `Limitless_Arbel_Renewal_Art_v2.zip` (ChatGPT 다운로드 제공, 사용자가 F:\Downloads에 저장).
- **v2 ZIP SHA256:** `17625d35c93599bdd74a4b8280c0416240c402be279815c8966199410bca97b7`.
- **v2 ZIP 크기:** 1,509,138 bytes / 전체 55파일 / PNG43개 (실사용34 + Preview8 + QA 반복도1).
- 보호한 원본 **v1 ZIP SHA256:** `ec707452169aa99ec297a7ac23a14928d4a0123b4b7b051d17ddcb282953d626` (변경0).
- **신규 사용 아트** 34종(23 direct replacement, 11 optional). 이전 34종 중 **변경 PNG 3종** (RoadLeft, RoadRight, RoofRespite); **31종은 v1 PNG byte/SHA256 완전 동일**. 동일 이미지의 새 파일명을 무조건 v2로 바꾸지 않고 원래 v1 이름을 보존했습니다.
- `Manifest.json`, `README.md`, `Renderer_Mapping_284.csv`, `Codex_Integration_Handoff.md`, `Rights_and_Provenance.md`, `Optional_Service_Placement.csv`, `QA/Changed_PNG_SHA256_v1_v2.csv`, `QA/Road_Seam_v2.json`, `QA/Props_9_Grounding_Audit_v2.csv`, `QA/Facility_Sign_v2.json`, `QA/Offline_Validation_v2.json` 포함. 합성 Preview 8장 및 기존 Ground 반복 QA 1장 포함.
- 모든 PNG decoding/ZIP CRC/Manifest 34파일 SHA/사이즈 및 284행 target 대응 자체 검증 PASS.

## 1) 도로 2 PNG의 가장자리 픽셀 수리

- v1 `Road/LL_C2_Arbel_Road_Left_128_v1.png` → `Road/LL_C2_Arbel_Road_Left_128_v2.png` (총 변경 픽셀 10).
- v1 `Road/LL_C2_Arbel_Road_Right_128_v1.png` → `Road/LL_C2_Arbel_Road_Right_128_v2.png` (총 변경 픽셀 4).
- Left→Right RGB/RGBA 수평 연결 불일치 9행(0-based y=16,54,55,66,73,82,87,108,122), Left 상하 1열(x=14), Right 상하 4열(x=114,115,116,117) 수정. 중복 없는 최소 대상 RGBA 14픽셀만 변경.
- 재검사: **Left→Right RGBA edge 0, Left 상하 반복 RGBA edge 0, Right 상하 반복 RGBA edge 0**. PNG 128×128 / PPU128 / Center Pivot, 기존 Vertical Road x±0.5 y=-6..1 및 이름/좌표 보호. Left-to-Left 수평 연결은 실제 배치 계약이 아니므로 억지로 일치시키지 않음.
- 전후 2열×4행 실제 이미지 반복도 `Preview/LL_C2_Arbel_Road_Seam_BeforeAfter_v2.png`는 오프라인 합성, Unity RenderTexture 아님.

## 2) 소품 9종 원본 크기·접지 수치 정정

- 소품은 **9종 PNG, 기존 Renderer 13개 참조**(Fence4, Tree3, Bush2, Rock1, Camp1, Barrel1, Chest1). 9개 원본 `SpriteReferenceRecovery20261014` .meta의 실제 rect 크기와 새 PNG Canvas·visible Alpha bbox/foot를 감사표로 산출.
- 원본 Sprite rect: Tree Medium 125×146, Big 205×247, Rock102×62, Camp88×75, Barrel64×82, Chest84×73, Bush Olive100×84 / Dry99×76, Fence128×128 (PPU128). 후보 PNG 크기 및 Alpha bbox와 접지 월드 좌표를 `QA/Props_9_Grounding_Audit_v2.csv`에 저장.
- v1 대응표의 `See recovered meta/actual slice` 미기록 상태를 상세 비교표로 대체하되 **실사용 소품 PNG 9종 모두 동일 바이트**, Sprite Transform/Pivot/Collider 변경 0.
- 중요한 검증 한계: 원본 .meta Sprite rect 하단값은 실제 원본 PNG Alpha foot와 동일하다고 단정할 수 없습니다. 새 PNG의 visible Alpha bbox/foot은 측정했지만 원본 PNG의 실제 Alpha foot 및 Unity Import/실물 접지 비교는 **NOT_VERIFIED**. 이 단계에서 PNG를 9개 임의 리사이즈하거나 이동시키지 않는 것이 최소 변경이며, Codex 현장 감사에서 차이가 입증된 항목만 후속 이미지 수정 승인 대상으로 분류합니다.
- `Preview/LL_C2_Arbel_Props9_Grounding_Metadata_v2.png`는 후보 9종의 접지선과 원본 크기 수치 비교 가이드. 픽셀 전후가 바뀌었다는 의미가 아님.

## 3) 북서 건물 의료 십자 표시 혼동 제거

- 실제 북서 건물 지붕 Sprite anchor **(-4.5,4.5)**, 실제 healer NPC **(2,-2)**, 잡화 상인 **(4,2)** 확인. 북서 지붕은 현행 치유시설 좌표가 아니므로 치유 십자 표시가 기능 혼동을 초래함.
- `Buildings/LL_C2_Arbel_Roof_Respite_384x192_v1.png` → `Buildings/LL_C2_Arbel_Roof_Respite_384x192_v2.png`. 십자 크림색 **205픽셀만 기존 원형 메달의 동일한 배경색으로 변경**, 원형 테두리·기와·문·건물 좌표·지붕 384×192/PPU128/Pivot 그대로.
- 동쪽 건물 `Roof_Provision`, 기존 문/벽/NPC/Exit/Quest 자산은 바이트·좌표 불변.
- `Preview/LL_C2_Arbel_Northwest_Roof_Sign_BeforeAfter_v2.png` 변경부 시각 비교 (합성 확대).
- 별도 **384×192 대형 지붕 가림/카메라·NPC 렌더링 QA HOLD 유지**, 미술 승인과 Runtime 승인 별도.

## 보호 및 다음 게이트

- `Renderer_Mapping_284.csv` **284/284** 기존 GameObject 이름/XY/source GUID+fileID/PPU/Pivot/Sorting 그대로; 수정 표의 target PNG 경로만 Road 8+8·NW Roof1 = 17행 업데이트. **교체후보23/선택장식11** 구분 유지.
- PortalAnchor (-7,-4)와 TreeSouthWest (-7,-4.2)의 기존 시각 충돌 때문에 Portal 장식은 **HOLD / 배치0**, 마차 주변 장식도 승인 전 미배치.
- **Unity Scene/Prefab/기존 Recovery PNG11·Sprite123/GUID/meta/Collider/NPC/Quest/Save/TTS/BGM/C# 변경 0.** 이 세션에서는 Unity Import/Play/실제 RenderTexture 실행0.
- 다음: (1) 바울 대표 도로 반복·북서 지붕·소품 기록 미술 확인 (2) Codex가 v1/v2 SHA·Scene 284대응 및 원본 Alpha·접지를 읽기 전용 확인 (3) 승인 후 별도 worktree에서 선정 자산만 Unity Import/Runtime·접근성·Save/Quest 회귀 QA.
- **Git:** 문서 및 인계만 `docs/art-renewal`에 commit; main 직접 Commit/Push/병합0, 강제 Push0. ZIP은 Git에 업로드하지 않고 ChatGPT 다운로드 파일로 제공.

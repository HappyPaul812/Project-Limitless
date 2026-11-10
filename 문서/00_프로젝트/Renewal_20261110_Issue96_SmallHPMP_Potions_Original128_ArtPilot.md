# LIMITLESS / 리뉴얼 아트 / Issue #96 — 시작 마을 HP·MP 소형 물약 아이콘 2종 오리지널 후보 납품

**작성:** 2026-11-10 (KST) · **담당 브랜치:** `docs/art-renewal`  
**판정:** `HP_MP_ORIGINAL_ART2_DELIVERED / PNG_HASH_CRC_TECH_PASS / USER_ART_REVIEW_PENDING / NOT_UNITY_APPLIED`

## 1. 기존 조사·계약과 범위

- [Issue #96](https://github.com/HappyPaul812/Project-Limitless/issues/96) 및 [Codex 내부 원본 재탐색 결과](https://github.com/HappyPaul812/Project-Limitless/issues/96#issuecomment-6758023477) 확인: 과거 132 ZIP·2499 PNG 등 범위 확인 결과 기존 자체 제작 HP/MP 그림 `NOT_FOUND / ART2_NEEDED`. 기존 활성 Kenney `HealingPotionSmall.png`·`ManaPotionSmall.png`는 외부 자산이고 신규 자체 오리지널 대체 필요.
- 총괄 [새 아트 2종 발주](https://github.com/HappyPaul812/Project-Limitless/issues/96#issuecomment-6759409066) 및 최신 사용자 지시에 따라 **정확히 HP·MP 작은 회복약 2종 각 1장만 신규 제작**. Chapter2 잡템·상태이상 해제약 오용 및 Kenney 픽셀 트레이싱/리컬러 없음.
- 게임 기존 규격은 상점 `ShopPresenter.BuildItemRows` **64×64 Image/preserveAspect**, 기존 Item importer `Single/PPU100/Pivot(0.5,0.5)/Bilinear/Uncompressed`; 실 사용자 게임 화면 재생 및 importer 동작은 이번 작업에서 미검증. 따라서 64px 화면 예시는 **OFFLINE MOCKUP**, Unity 결과가 아님.
- 게임 수치 및 가격 **HP +30 / 상점 가격 20**, **MP +12 / 상점 가격 30** 절대 변경하지 않았음.

## 2. 신규 PNG 2장 및 무결성

| 신규 아이콘 | 사양 | SHA256 | 구별 |
|---|---|---|---|
| `Candidates/Small_HP_Potion_Original_128_RGBA.png` | **128×128 RGBA 투명** | `675a62927ee6a1321ad552674e931a8f53f88680f8d14de7a0acde7c17497f64` | 둥근 붉은 유리약병, 코르크 마개, 적색 리본과 십자 메달 |
| `Candidates/Small_MP_Potion_Original_128_RGBA.png` | **128×128 RGBA 투명** | `39b90dfc0dac057c61ef39adb23d215236bb9ef7abe721c6bce0e713cdfdb64a` | 날씬한 푸른 물약병, 마나 수정 마개, 청색 리본과 달·별 메달 |

두 후보는 *형태와 색 모두* 구분하도록 별도 디자인. 원형 캔버스 1254×1254 RGBA 이미지 생성 출처로부터 비율을 보존해 128×128(투명)으로 축소. 알파가 1/255 이하인 비가시 잡픽셀만 지우고 보이는 RGB 작품은 유지. 출력 alpha>8 픽셀은 캔버스 최외곽 4변에 **0**. 후보의 보이는 bbox: HP `(19,7)-(109,123)`, MP `(29,7)-(98,123)`. 128 PNG 직접 상점 64px 참고 확대 대비 실제 UI GameView 미검증.

**제작 소스(파일 그대로 ZIP에 포함):**
- `OriginalSources/HP_Generated1254_SOURCE.png` SHA256 `6d0b72616f0cb8c6e4c38e59d83ff2a1304477a24011a4c1bf523b875338ee37`
- `OriginalSources/MP_Generated1254_SOURCE.png` SHA256 `7aa9af9d850d23931102c4d7b163f29ce59aefb12e7a789bc4e0cbfe3536f063`

## 3. 상점 미리보기와 QA 범위

- `Review/HP_MP_128_64_40_32_RealScale_OFFLINE.png`: **128/64/40/32px 실 축소**, 두 병의 색/체형/장식 구별. 64px 이하에서 장식 세부는 작아지나 붉은 둥근 병·푸른 긴 병 구분은 유지.
- `Review/HP_MP_64px_LightDark_ShopUI_OFFLINE.png`: 기존 계약에 맞춘 64px 이미지로 밝음·어두움 배경과 설명(기존 HP+30/20, MP+12/30)을 배치한 **수작업 오프라인 UI 모형**, 원본 상점 실제 캡처가 아님.
- `Review/{HP,MP}_Potion_64px_ShopIcon_OFFLINE.png`: 독립 64px 표시 예시.
- `Evidence/SourceDeclaration.md`: 생성 이미지와 Kenney 비파생·아직 미승인 출처 관계 명시.
- `Evidence/QA_Results.json`: 신규 파일 2종만, RGBA/외곽 알파 검사/게임 미적용, 활성 아이템 효과·가격 변경0 기록.

## 4. 실제 ZIP·전달

- **파일** `LIMITLESS_ISSUE96_HP_MP_ORIGINAL_POTIONS_20261110.zip`
- **2,286,596 bytes**, ZIP SHA256 **`d1ce0e1cbbf64b588dd2b905a50cc4ecbc2e9bf2bd06378b58ab1d2cb62d7ead`**
- **13 entries / CRC PASS / Manifest 11/11 bytes+SHA256 PASS**. `SHA256SUMS.txt` 추가 포함.
- **ChatGPT 라이브러리:** `/LIMITLESS_Art_Delivery/Issue96_HP_MP_Potions_20261110/`에 **통합 ZIP+PNG2+실축소 비교판+상점 밝음/어두움 모형+각64px PNG2 = 7개 업로드 성공**. 사용자 Windows F: 자동 복사 또는 Github 원격 PNG/ZIP Push 없음.
- 아래 두 파일만 신규 게임 아이콘 후보이며, `OriginalSources` 2장은 같은 두 원화의 1254px 작업용 대형 소스(추가 아이템 종류가 아님).

## 5. 보호·승인·후속

- 기존 Kenney 파일 `HealingPotionSmall.png`, `ManaPotionSmall.png`, 기존 .meta/GUID, ItemDefinition, `ShopPresenter`/상점 가격·구매/판매/수량/툴팁/아이템 효과, Scene·Prefab·Save/UserData, Unity **6000.6.5f1**, 승인 아트·MonsterDefinition, `main` **변경0**.
- VenomSnake #97 Attack01v2 KEEP·Attack03v2 KEEP 권고 유지, 신규 애니메이션 재작업0. #95 NPC 두 신규 후보 보존·재작업0.
- **USER_ART_REVIEW_PENDING:** 바울 대표가 두 물약의 128/64 크기 미술 승인 이후, Codex가 시작 마을 **거래창 64px 한정** 별도 격리 표시 적용·실게임 가독성 및 구매·판매·아이템 수치·Save0 DeltaQA를 수행해야 함. 공용 ItemDefinition·GUID 직접 갈아끼우기 금지.
- `docs/art-renewal` 문서 커밋 외 게임 원본 수정0, `main` 직접 commit/Push/merge 금지.

**최종 상태:** `SMALL_HP_MP_TWO_ORIGINAL_ART_CANDIDATES_READY / USER_ART_REVIEW_PENDING / SHOP_RUNTIME_REPLACEMENT_HOLD / UNITY_NOT_APPLIED`.

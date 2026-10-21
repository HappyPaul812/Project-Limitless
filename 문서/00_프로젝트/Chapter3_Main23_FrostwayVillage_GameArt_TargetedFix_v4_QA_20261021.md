# LIMITLESS Chapter3 Main23 서릿길 마을 — GameArt v4 2종 표적 수정 납품 및 QA (2026-10-21)

> **ART_TARGETED_FIX_V4_DELIVERED / 41_PNG_BYTES_PRESERVED / OFFLINE_FILE_QA_PASS / TILE_PATTERN_ART_HOLD / CODEX_V4_DELTA_QA_PENDING / USER_FINAL_ART_APPROVAL_PENDING / NO_UNITY_INTEGRATION**

- 담당: Chapter3 신규 아트 총괄, 브랜치 `docs/art-new`, [Issue #45](https://github.com/HappyPaul812/Project-Limitless/issues/45).
- 기획 정본: `docs/planning/문서/00_프로젝트/20261021_Frostway_v3_WolfRun_v4_ArtFail_TargetedRetouch_Orders.md` (planning commit `12f8dd9e`).
- Codex v3 격리 검사: ZIP/CRC/Manifest/43 PNG 기술 PASS, 세 화면비 Unity 조립/통행 QA 완료(앞선 실행 결과), **미술 FAIL** 두 PNG의 원화 잔여 조각 + **Tiles 4종 반복무늬 시각 HOLD**. 이 v3 Unity PASS를 v4에 대한 신규 Unity 검사로 오인하지 않음.

## 실제 납품 파일

- ZIP **`Limitless_Chapter3_FrostwayVillage_Environment_GameArt_TargetedFix_v4.zip`**
- **완성 ZIP SHA256: `ee25baef91ec740b85dd3bb62b32583292d703f9261863bd534d0eb8241d1896`**
- ZIP 크기 **19,767,770 bytes**, **70 entries**, `ZipFile.testzip()==None` (CRC PASS), SHA256SUMS 전체 엔트리 **69/69 대조 PASS**.
- 기존 v3 원본 ZIP **`Limitless_Chapter3_FrostwayVillage_Environment_GameArt_v3.zip`** SHA256 **`2a74677d5f62d7e13e64ea85ba62e12175be0be0e0d19864c23233f1bd84e79a`**. 이 원본은 읽기 참조만 했고 수정하지 않음.
- 실물 v4 ZIP은 이 ChatGPT 대화에서 클릭 다운로드로 제공했고, PNG/ZIP 바이너리를 GitHub에 업로드한 것이 아님. GitHub 브랜치에는 이번 문서와 인계만 기록.

## 수정 2종 — 불필요한 독립 컴포넌트만 제거

| 게임 PNG | v3 SHA256 | v4 SHA256 | 제거된 독립 조각 |
|---|---|---|---|
| `Buildings/Frostway_SkillShop_Building.png` (511×418 RGBA) | `7e3562c22f00a07a85f0ab0349b8119dff924f3ef4dccf199cccdc9ec367be89` | `ef34b49de4f807debf4a69e1947eb9b8e02a543ee117ca2867fe3aa06a5efb17` | **2,496 픽셀**; 정확한 별도 alpha connected component bbox x12..28/y196..376 (포함) |
| `Buildings/Modular/Frostway_Roof_Gable.png` (175×219 RGBA) | `4971eea0e473ec7c7c1579001a454be52417fd9f3f38b9e223b00e0deaaf71ec` | `37ea6a69844b662c40583cea3d457ff0c47344aef7054ff8e32b4d09870dceb0` | **712 픽셀**; 정확한 별도 component bbox x100..144/y183..206 (포함) |

- 분석 방법: RGBA alpha >0 기준 8-connected component 분할. 두 PNG 모두 기존 원본에 정확히 두 개의 연결 요소(본체1, 누출 조각1)가 있고, 조각의 좌표·픽셀 수가 Codex 지적 범위와 맞음을 증명. **조각 mask에 속하는 픽셀만 alpha=0 및 RGB=0**. 지정 사각형을 무조건 통째로 지우지 않음.
- **두 정상 본체는 RGBA 픽셀 하나도 변경 없음**. 수정 후 각각 컴포넌트1만 남음.
- `QA/BeforeAfter/SkillShop_FullScale_1x.png`, `SkillShop_Defect_Zoom_2x.png`, `SkillShop_Display128_Compare.png`, Roof_Gable 동일 3개 파일에 **수정 전후·2배 확대·128px 동일 표시 높이** 비교.
- 이 비교는 오프라인 이미지. **Unity GameView 캡처가 아니고, 128px은 미리보기 축소 높이**임.

## 나머지 41 PNG — 바이트·SHA 동일

- 원본 v3의 43개 게임용 자산 경로 구조를 그대로 유지. **지정한 2개를 제외한 게임용 PNG 41/41 모두 v3 ZIP entry 바이트가 완전히 동일**. 확장·재인코딩·신규 디자인 변경 없음.
- **Tiles 4종을 포함**하여 보존함. 기존 2×2/3×3 반복검사 이미지 8개, Offline Layout Preview 16:9·4:3·21:9 3개 및 Asset Overview PNG1도 참고 자료로 바이트 보존.
- **중요:** `Preview/`의 4개 이미지는 v3에서 만든 오프라인 배치 보존본이며, v4 재합성이나 Unity v4 GameView로 오인하면 안 됨.
- v4 Manifest.json/Manifest.md 및 QA/Offline_QA.json, SHA256SUMS.txt 갱신; 자산별 최신 SHA, 변경 마스크 bbox/픽셀 수 및 41 KEEP 증명 기록.

## Tiles 4종 — 시각 반복 HOLD만 따로 기록(타일 내용 미변경)

3×3 동일 타일을 9번 배치하여 육안상 반복되는 **대표 문양/좌표 예시**를 `QA/PatternHOLD/TilePattern_3x3_Overview.png` 및 4개 `..._3x3_Annotated.png`에 표시했다. 아래는 **각 원본 256×256 타일 내부 XY 좌표(좌상단 원점, 양끝 포함)**이며 반복 횟수 9; 예: (x+256,y), (x+512,y), (x,y+256) 등에 동일 문양이 재등장.

| 보존 파일 | 예시 반복 좌표 원본 타일 | 시각 상태 |
|---|---|---|
| `Tiles/Frostway_Ground_Earth_01.png` | x9..94, y120..252 | 아래쪽 풀·회색 돌 무리 반복 / HOLD |
| `Tiles/Frostway_Ground_Gravel_01.png` | x23..130, y26..120 | 비교적 밝은 자갈·돌 집중부 반복 / HOLD |
| `Tiles/Frostway_Ground_ThinFrost_01.png` | x9..94, y120..252 | 흙 바탕의 동일한 풀·돌 조합 반복 / HOLD |
| `Tiles/Frostway_Path_Stone_01.png` | x60..215, y94..240 | 판석의 특정 배열·이음 패턴 반복 / HOLD |

- 모든 4종 256×256 RGBA Base의 상하/좌우 맨자리 픽셀 일치 **기술 PASS**이나 단일 소스 타일 반복에 따른 문양 시각 HOLD는 그대로 남아 있음.
- 이번에는 바울 대표 승인 전 Tile/Transition/스폰/Collision/Renderer를 임의로 개선·생성하지 않음. 후속 미술 결정에서 필요한 타일/영역만 좁혀 별도 v5 보완 여부 결정.

## 단계 분리 및 인계

- **OFFLINE FORMAT PASS:** PNG43 decode, SHA41 보존, 수정2 정상 본체 RGBA 불변/잔여조각0, ZIP 70 members CRC/해시 PASS, Tiles4 edge PASS.
- **ART HOLD:** Tiles4 단일 소스 반복 문양 체감, 대표 최종 미술 검토 전. 두 누출 조각은 오프라인 기준 제거됨.
- **CODEX UNITY v4 DeltaQA PENDING:** 변경2 PNG 교체 후 실제 16:9/4:3/21:9 격리 GameView·건물 모듈 접합·도보/휠체어 경로 등 **변경분만** 검증. 기존 전체 합격 전수 재검사 금지.
- F1 승인6, v2 기존 콘셉트 원본11, Chapter1·2 리뉴얼, Wolf Run6, 몬스터/인물 원본, Main23 Scene/Quest/NPC/Save/UserData/GUID/Unity Editor 미수정. **main 직접 커밋·Push·병합 금지**.

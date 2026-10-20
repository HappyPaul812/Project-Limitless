# LIMITLESS — Field09 Issue #37 Cracked Overlay 단일 보완 v2 납품 (2026-10-20)

## 요청 / 정확한 상태

- 기획 정본: `docs/planning/문서/00_프로젝트/Field09_Redesign_Sample3_UnityQA_OverlayOnly_Fix_20261020.md`, GitHub Issue #37.
- Codex 격리 Unity QA 결과 **Base Ground 시각 PASS, Obsidian Slab 시각 PASS**, 두 자산 **절대 수정하지 않음**.
- 유일한 시각 FAIL: Cracked Overlay **World x≈-1.5**에서 밝은 균열이 Tiled 오버레이 오른쪽 렌더 경계에서 수직으로 잘림.
- 상태 **`ISSUE37_OVERLAY_V2_PNG_ACTUAL_DELIVERED / OFFLINE_QA_PASS / BASE_SLAB_SHA_UNCHANGED / CODEX_SINGLE_CASE_UNITY_QA_PENDING / USER_ART_APPROVAL_PENDING / OFFICIAL_SCENE_UNCHANGED`**.

## 실제 제작 PNG 1종

- 파일 **`LL_Field09_Redesign_Ground_Cracked_Overlay_v2.png`** (대화 Sandbox `/mnt/data/Limitless_Field09_Redesign_Sample3_OverlayFix_v2/Assets/`).
- 512×512 RGBA, PPU128, 중앙 Pivot, Tiled 9×15, Position (-6,0), 오른쪽 경계 x≈-1.5 유지.
- **원본 v1 SHA256** `56d8e0ea9bf3ca2167227362b87bb219a21d4470f317456f36eb8aba99caedeb`
- **새 v2 SHA256** `0147e6aff58e5304fd71e7474a9c018958a4234ec4c2dc2cf7382a905a7be9b7`
- **새 v2 픽셀 RGBA byte-array SHA256** `4dd0589c36a07d90b238f3402a7dd7336f8f3becdef30117dd7cf3eeacd13d4d`
- 수정 픽셀: **3705**, x=100..155 / y=30..389. RGB 3채널 원본 **100% 동일**, Alpha만 국소적으로 변경. 전체 디자인/색/타일 가장자리 RGBA 보존. 기존 v1의 딱딱한 3개 균열 횡단부(타일 x128, y≈43·273·362)의 끝을 곡선형·분기형 알파 감소로 자연스럽게 약화.
- 경계 x=128에서 **강광원 교차 픽셀 (R>170 & Alpha>60): 이전 8 → 신규 0**. 이는 오프라인 픽셀 지표이지 실제 Unity 시각 PASS의 증거는 아님.

## Base 및 Slab 보호 체크

| 자산 | v1 SHA256 유지 | 판정 |
|---|---|---|
| `LL_Field09_Redesign_Ground_Base_v1.png` | `1bd8a614988ee182640a71b9771646c3009545140e0522a3290ee3b0e3138203` | **VISUAL PASS · KEEP · 실제 파일 수정0** |
| `LL_Field09_Redesign_Obsidian_Slab_v1.png` | `dfa3abb2c6c86abc3f94baa213428038a1c15b4c963e76fef69491a6deb09692` | **VISUAL PASS · KEEP · 실제 파일 수정0** |

새 ZIP에는 **오버레이 PNG 한 개만** 포함하며 합격 PNG2는 원본 v1 ZIP에서 재사용합니다.

## 실제 ZIP / QA

- `Limitless_Field09_Redesign_Sample3_OverlayFix_v2.zip` — **3,338,697 bytes / 11 entries**.
- **ZIP SHA256** `ec831166a753eef179da8fc283faf1fe81d1880941bce10c8d93225269cd418c`
- Python 표준 `zipfile.ZIP_DEFLATED` 신규 ZIP 작성, `testzip()==None` **CRC PASS**, 새 PNG 엔트리 실제 원화 바이트 동등 및 11 entry 비어있는 파일0 검사.
- `Manifest.json`: 정확한 입력·출력 SHA, PPU/Pivot/World position, Base/Slab SHA. `QA/Changed_Pixels.csv`: 3705 픽셀 (x,y,구/신 Alpha). `QA/Offline_Validation.json`: RGB 보존·경계 교차 픽셀 검사.
- `Preview/Overlay_v1_vs_v2_x_minus1_5_Detail_NotUnity.png`: 정확한 World x=-1.5 경계 확대 비교 (기존/새 오버레이 PNG를 실제 Base 타일 위에 합성한 **오프라인 시뮬레이션**, Unity 화면 아님).
- `Preview/Before_v1_21x15Base_9x15Overlay_NotUnity.png`, `Preview/After_v2_21x15Base_9x15Overlay_NotUnity.png`, 3×3 합성 확대 및 단일 타일 원본 비교.
- `tools/rebuild_overlay_v2.py`: 단일 원본 Overlay v1에서 수정 PNG를 재생성하는 재현 코드(입력 원본 SHA 확인).
- **산출물 공개 방식:** PNG와 ZIP은 이 대화의 **실제 sandbox 파일**로 납품됨. 사용자 F:·원본 Unity로 자동 복사하거나 GitHub에 바이너리 원화를 업로드했다고 주장하지 않음. 대신 Codex 로컬에서 동일 출력 PNG를 생성하도록 실행 가능한 코드를 **담당 GitHub 브랜치에 커밋**:
  - `Art_Tools/Field09_Issue37_OverlayFix_v2.py` (Commit `b9dbd5e66524eb249a86e4d7cc421c12656b60dc`).
  - Codex가 보유한 원래 `F:\Downloads\Limitless_Field09_Redesign_Sample3_v1.zip`의 **Cracked Overlay v1 한 파일만 읽기전용 추출**, Python3 Pillow+numpy로 스크립트 실행해 v2 후보 생성 가능. 대표에게 F: 자료 재업로드를 요구하지 않음.

## Codex에게 전달할 유일한 다음 작업

1. GitHub `docs/art-renewal/Art_Tools/Field09_Issue37_OverlayFix_v2.py`와 이 문서를 사용. 원본 v1 ZIP의 Overlay PNG만 안전한 별도 worktree/임시 폴더로 복사, 입력 SHA 대조 후 새 PNG 생성. **Base/Slab v1 그대로, 원본 Scene 변경0**.
2. 신규 `CrackedOverlay_v2`만 격리 Import (새 GUID). x=-1.5, 3개 GameView 해상도, `Tiled 9×15`과 `Base 21×15` 이종 접합에서 밝은 균열 직선절단·어두운 수직 결함 추가 여부 **단일 시각 Delta QA**. 이전 전체 31PASS 및 Main18 관련 QA 재실행 금지.
3. FAIL이면 SpriteRenderer Tiled clamp/Shader/Material/Sorting 원인과 픽셀 문제를 분리 보고. 이번 오프라인 파일 검사만으로 Runtime 0FAIL 주장 금지.
4. 바울 대표 최종 미술승인 및 별도 공식 적용 승인 전에는 추가 Field09 자산 제작, 원본 Unity/사용자 Save/meta/GUID/main 수정·Push·병합 금지.

**본 문서는 GitHub 작업 기록이며 새 바이너리 원화/ZIP 다운로드 링크는 해당 ChatGPT 대화 메시지에서 제공.**

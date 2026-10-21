# Wolf v4 · NorthF1 환경6 v2 신규 납품 검수 — 2026-10-21

**Wolf: 기술 PASS / 미술 HOLD. F1: 파일 기술6/6 PASS / 개별 미술3PASS·2HOLD·1FAIL / 전체 묶음 HOLD.**

실물 ZIP 두 개와 직전 검수 결론을 대조한 비파괴 오프라인 검수입니다. **모든 전후·확대·반복 이미지는 실제 PNG를 사용한 오프라인 합성이며 Unity GameView가 아닙니다.** 새 아트 제작·PNG 수정·Wolf Run/Bear/Fox·Main22·Scene/Quest·Unity Import·정식 적용은 하지 않았습니다.

## 정본과 검수 브랜치

- 최신 기획 [표적 제작 지시](https://github.com/HappyPaul812/Project-Limitless/blob/docs/planning/문서/00_프로젝트/20261021_ArtReview_Result_SnakeWolfAndNorthF1_TargetedProduction.md), blob `a2c37a33ae447ef2537c51655f765f5789c1bdd1`.
- [Issue43 Wolf v4 납품](https://github.com/HappyPaul812/Project-Limitless/issues/43#issuecomment-6306433679), art-renewal 보고 blob `59a9cd59b65d8f7052023d84b1ec7b77da570ab4`.
- [Issue30 F1 환경6 v2 납품](https://github.com/HappyPaul812/Project-Limitless/issues/30#issuecomment-6306178569), art-new 보고 blob `f80f660d85ee26318b2db537f84949d64fb9f89e`.
- 직전 검수 `5df5f74d7e9070544bb88aa92fbfe991972498cf`에서 **`codex/art-review-wolfv4-northf1v2-20261021`**을 분리했습니다. GitHub main 재조회는 `e4e9891f6f01ea95cada9e63b1f09946d94d6041`입니다.

## A. Wolf 원화1포즈 알파 보정 v4

입력 `F:/Downloads/Limitless_Wolf_KeyArt_v3_TargetedAlphaFix_v4.zip`.

| 기술 항목 | 결과 |
|---|---|
| ZIP | **PASS**, SHA256 `e241535f1c505bf50f4de09317e74dbc3780be722139ff0baa02c908223765c6`, 2,466,565bytes /14파일 /전체CRC 정상 |
| PNG·Manifest | **PASS**, 전체9PNG 디코딩, Assets2개의 실제 용량/SHA가 Manifest와 일치 |
| 수정 후보 | `Wolf_KeyArt_v3_AlphaFix_v4.png`, 952×1306 RGBA, 1,020,181bytes |
| 후보 PNG SHA256 | **`62f72e97525f0266cbb6132da8b21571594f7c0deb3b1e1749cce749c9bf1c0a`** |
| 원본 v3 | ZIP의 `Wolf_KeyArt_v3_ORIGINAL_READ_ONLY.png`가 직전 검수에서 보존한 v3 실파일과 **바이트 완전 동일** |
| 원본 PNG SHA256 | `30b2d464fe2c456c0fa7d82f1fad51f64065b6e8379ea174548deec7d1b91b4e` |
| 변경 범위 | **RGB 변경0 / 알파 변경12,088픽셀 / 알파 증가0 / 캔버스 크기 변경0** |
| 변경 CSV | 12,088행의 x/y/before_alpha/after_alpha를 실제 전 픽셀 차이 집합과 대조하여 누락·추가0 |

변경 bbox는 좌상단 기준 **[165,821,684,1216)**, 납품의 inclusive 표기 `[165,821,683,1215]`와 같은 범위입니다. 전체 캔버스의 약 **0.97224%**만 RGBA가 달라졌고 bbox 바깥은 모두 동일합니다. 전체 alpha bbox도 전후 `[48,48,904,1258]`로 같습니다. 머리/시선·몸통·털 RGB·발 색·전체 배율을 다시 그린 것이 아닌 **알파만 국소적으로 줄인 표적 수정**으로 판정합니다. RGB 동일만으로 수정 경계의 털 알파까지 모두 보존됐다고 주장하지는 않습니다.

### 직전 결함 수정 여부 — 부분 해결, 미술 HOLD

| 부위 | 전후 확인 | 현재 판정 |
|---|---|---|
| 가슴 밑 넓은 회색 매트 | 넓게 늘어진 하단 회색 덩어리는 상당 부분 투명화됨 | **부분 해결**. 위쪽 털과 앞다리 사이의 삼각형 회색 잔여가 남음 |
| 뒷다리 뒤 큰 배경 블록 | 종아리 옆 긴 수직 블록은 제거됨 | **주요 결함 개선**. 다리 가장자리의 작은 검은 점/돌출 잔여는 최종 국소 정리 필요 |
| 발·발밑 | 발 색·형태는 유지됨 | 오른쪽 앞발 아래 바닥 조각은 수정 bbox 밖이어서 그대로 남아 있음. 새로 생긴 결함은 아니지만 직전 비교에서 확인한 잔여 |
| 128px 인상 | 전체 캔버스를 동일128×128 칸, 별도로 Battle Image136×112 칸에 preserveAspect 방식으로 축소 | 서 있는 늑대 인상·크기·색은 유지. 작은 미리보기만으로 잔여 매트 제거 완료를 판정하지 않음 |

**재검수할 정확한 원본좌표(좌상단 x/y, 우·하단 exclusive)**

1. `[630,785,700,870)` — 가슴 윗부분/앞다리 안쪽의 회색 삼각 조각. 실제 털은 보호하고 배경 면만 추가 마스킹.
2. `[150,1125,195,1175)` — 뒷다리 가장자리의 점형 검은 돌출. 원래 털인지 배경 잔여인지 경계를 확인한 뒤 해당 몇 픽셀만 정리.
3. `[750,1200,905,1260)` — 오른쪽 앞발 밑 바닥 조각. 발톱·발 모양을 바꾸지 않고 바깥 지면 잔여만 제거. 이 영역은 현 변경 bbox와 대부분 겹치지 않습니다.

원래 늑대의 형태·색을 다시 제작할 필요는 없습니다. **과도한 재드로잉 아님 / 주요 매트 감소 확인 / 완전 수리 PASS는 보류**가 정확한 결론입니다.

[128px·전투 슬롯 전후](Evidence/WolfV4F1V2_20261021/Wolf_128px_BattleSlot_BeforeAfter.png) · [가슴2배 확대](Evidence/WolfV4F1V2_20261021/Wolf_Chest_BeforeAfter.png) · [뒷다리2배 확대](Evidence/WolfV4F1V2_20261021/Wolf_HindLeg_BeforeAfter.png) · [발 전후](Evidence/WolfV4F1V2_20261021/Wolf_Paws_BeforeAfter.png) · [잔여 위치](Evidence/WolfV4F1V2_20261021/Wolf_RemainingArtifacts_Coordinates.png)

## B. NorthF1 Environment6 ArtImprovement v2

입력 `F:/Downloads/Limitless_Chapter3_NorthF1_Environment_6_ArtImprovement_v2.zip`.

**ZIP 기술 PASS**: SHA256 **`8158f501763b525022b8b9fc1645fe2b0a5c0bc8bf9f73aabcb02024c3a3235c`**, 3,897,421bytes /23파일 /전체CRC 정상. 구성은 신규PNG6·원본PNG6·Preview4·반복QA4·Manifest/SHA/QA JSON3이며, 전체20PNG 디코딩 정상입니다. SHA256SUMS22항목, QA JSON의 후보6 SHA/크기/모드와 originalProtected6 SHA 모두 실제 파일과 일치합니다. 직전의 서로 충돌하던 원본 해시 표 문제는 이번 패키지에서는 재현되지 않았습니다.

기존 원본6은 직전 검수의 `Original_F1` 실파일과 **각각 바이트 동일**입니다. 과거 샘플3종도 수정하지 않았습니다. 신규6은 모두256×256 RGBA, 별도 `_v2` 파일입니다.

| PNG (`Art/`) | 기술 | 미술 | 전후 확인·현재 보류 사유 |
|---|---|---|---|
| `NorthF1_Ground_Earth_Base_v2.png` | **PASS** | **PASS** | 기존 큰 올리브 다각형 얼룩 대신 토양·자갈·풀·완만한 명암으로 바뀌어 평면감 완화. 3×3의 단일 타일 반복은 남지만 직전의 큰 도식적 얼룩 결함은 해소되어 정적 미술 후보로 통과 |
| `NorthF1_Ground_ThinFrost_Base_v2.png` | **PASS** | **HOLD** | 흙 우세·서리 가장자리 소재감은 개선. 3×3에서 같은 밝은 사선/고리형 서리 무늬가256px 간격으로 뚜렷하게 반복됨. 가장 강한 밝은 문양만 분산·완화할 필요 |
| `NorthF1_EarthToFrost_Transition_Overlay_v2.png` | **PASS — 단방향 파일 규격** | **FAIL** | 우측 가중 구조 유지. 그러나 Earth+전환부 오른쪽의 진한 흰 얼음이 다음 ThinFrost 타일에서 바로 갈색/옅은 서리로 바뀌어 **직선 경계에서 밀도·명도가 갑자기 끊김**. 실제 제공 타일 조합의 연결 목적 미달 |
| `NorthF1_Path_WornTrail_Overlay_v2.png` | **PASS** | **PASS — 시각 경로만** | 패인 중앙 흙면과 자갈 가장자리로 기반 지면보다 보행 띠가 잘 읽힘. Y반복 가장자리RGBA 동일, 연결 단절 없음. 물리적 휠체어 폭·Collider PASS는 아님 |
| `NorthF1_Prop_FrostShrub_01_v2.png` | **PASS** | **PASS** | 가지 분기·서리 림·몸통 명암·밑돌 접지로 직선 도식 인상 완화. 원화/128px에서 관목으로 구분되고 캔버스 잘림 없음 |
| `NorthF1_Prop_Waymarker_01_v2.png` | **PASS** | **HOLD** | 목재결·눈·기둥/밑돌 깊이는 개선. 다만 양끝이 모두 뾰족하고 판 안에 명확한 방향 표시가 없어128px에서 어느 방향을 지시하는지 모호함. 방향 안내 용도로 통과시키기 전 단일 지시 방향을 명확히 할 필요 |
| **F1 전체 묶음** | **파일 기술 PASS** | **HOLD** | **ThinFrost·Transition·Waymarker 3PNG가 보류 원인**. 통과한 Earth·Path·Shrub를 전면 재제작할 필요 없음 |

### 새 PNG SHA256

| PNG | SHA256 |
|---|---|
| Earth | `ccf2c22e90bee292ecf2d0d030dad53e0032457272ebb94fccdf6491fabcf8b9` |
| ThinFrost | `5fcb2da32042a83beb036797d3e9eeb9a5e3cf0016062f88dd66d012fc969b85` |
| Transition | `7d11443186fcad691a5830a4a959f3a3d9076b613669b8db26c4fe56a5551aa1` |
| Path | `e78b41d48707dfce6a79dd4e826f864c5c2608b45a0f457af414ee9768a20175` |
| Shrub | `6c83c88881e7f89245ed580d50424b2671629ad9491da93f247aab99bfe4746e` |
| Waymarker | `40036738e7da0c25df5c0b69aacba37c3d6e4a681d566fa3a5d30d7caea7431d` |

### 변경 범위·반복·연결의 구분

원본6과 신규6의 RGBA 변경 픽셀은 Earth65,536(100%), ThinFrost65,536(100%), Transition27,797(42.41%), Path25,207(38.46%), Shrub18,933(28.89%), Waymarker18,144(27.69%)입니다. 투명 영역의 RGB도 비교한 수치이며 체감 미술 면적 비율은 아닙니다. Wolf의 알파 표적 수정과 달리 **F1은 승인된 발주 범위 내6종 새 개선안**이며 원본 덮어쓰기가 아닙니다.

- **Earth/ThinFrost**: 각 상하·좌우 끝RGBA100% 일치,2×2/3×3 자체 반복에 수치상 이음선 단절 없음. 다만 같은 밝은 무늬 반복감은 별도 미술 판정입니다.
- **Path**: 상하RGBA동일·위아래alpha>0 각각97픽셀로 연속. 길 시각 판독은 개선됐으나256px을 임의의 월드폭으로 환산하지 않았습니다.
- **Transition**: alpha0~253, 좌측edge alpha>0 0픽셀·우측204픽셀, 우측 지향 유지. 상하끝은 둘 다 투명해서 동일하나4방향 오토타일이라는 뜻이 아닙니다.
- **다른 소재 연결**: Earth오른쪽과 ThinFrost왼쪽은 완전히 같지 않으며 RGB차 평균은3.82/255입니다. 이 작은 수치 차이만으로 FAIL을 준 것이 아닙니다. FAIL 주근거는 **전환 오버레이의 높은 흰 서리 밀도가 다음 타일과 잇지 못하는 실제 오프라인 합성**입니다.
- 전환부의 문제 위치: 후보원본 **x210~255의 고밀도 흰 부분**이 다음 ThinFrost의 왼쪽x0~45로 넘어가는 접합. 비교판에서는 상단 조합x512 부근(패널여백 제외)의 수직 끊김으로 확인됩니다. 단방향이라는 사실 자체는 허용 규격이므로 FAIL 원인과 분리했습니다.
- 표지판은 원본 기준 **[28,26,227,244)** 안의 판 부분(대략y70~125), 특히 좌우 양끝 형상·판 내부 방향 표식 부재를 확인하십시오. 월드의 새 방향을 임의로 지정한 것이 아니라 현재 안내 판독성의 문제를 기록한 것입니다.

### 최소 수정 포인트

1. **ThinFrost**: 흙 우세는 유지하고 반복에서 가장 두드러지는 밝은 사선 서리 문양만 분산/약화. 전체 지면 재디자인 요구 아님.
2. **Transition**: 우측 끝의 흰 서리량·색·입자 크기를 현재 ThinFrost 왼쪽과 이어지게 국소 보정. 단방향 유지, 새4방향 세트 제작 요구 아님.
3. **Waymarker**: 현재 목재·기둥·접지는 유지하고 작은 표시에서도 지시 방향이 한 번에 읽히게 판의 방향 단서만 정리.
4. **Wolf**: 위 세 국소 영역의 배경 잔여/점만 마감. 털색·몸·발·머리 재드로잉 불필요.

[지면1:1·128px 전후](Evidence/WolfV4F1V2_20261021/F1_Grounds_BeforeAfter.png) · [길·소품1:1·128px 전후](Evidence/WolfV4F1V2_20261021/F1_PathProps_BeforeAfter.png) · [Earth2×2](Evidence/WolfV4F1V2_20261021/Ground_Earth_Base_Repeat2x2.png) · [Earth3×3](Evidence/WolfV4F1V2_20261021/Ground_Earth_Base_Repeat3x3.png) · [ThinFrost2×2](Evidence/WolfV4F1V2_20261021/Ground_ThinFrost_Base_Repeat2x2.png) · [ThinFrost3×3](Evidence/WolfV4F1V2_20261021/Ground_ThinFrost_Base_Repeat3x3.png) · [전환/길 연결 실패 위치](Evidence/WolfV4F1V2_20261021/F1_Transition_Path_Delta.png) · [방향 판독·소품](Evidence/WolfV4F1V2_20261021/F1_Props_OnEarth_256px.png) · [실제Ch2 PNG 소재 비교](Evidence/WolfV4F1V2_20261021/F1_Ch2_Material_Comparison_Offline.png)

## 검수 한계·원본 보호·Git

F1 PPU/TileGrid/Collider·실제 통행 폭이 미확정이므로 게임 Scene·플레이어/휠체어 물리 통행 PASS를 주장하지 않습니다. Chapter2 비교는 실제 로컬 DeepCore 원본512px을 비교용256px로 표시한 **오프라인 소재·명암 비교**이며 바이옴색/월드배율 강제 일치 기준이 아닙니다. Wolf도128px/136×112슬롯 오프라인 비교이며 새로운 PPU를 정하지 않았습니다.

원본 Assets/UserData/Settings/Packages **4,351파일 SHA 변경0**, 원본 Git status 바이트/HEAD 동일. 사용자 두 ZIP은 검수 시작/종료SHA동일. 원본 v3와 F1원본6을 그대로 보존했습니다. 원본 Editor 전후 Bootstrap / PlayOFF / PausedOFF / compiling false / 비포커스 유지, 읽기 전용 상태 조회 외 조작0입니다.

브랜치에는 이 보고서·실측JSON/CSV·오프라인 비교·재현 스크립트만 기록합니다. 원본 Unity·Save·UserData·승인 아트·기존 코드 수정0, 신규 아트 제작0, 공식 적용0, Wolf Run/Bear/Fox0, Main22/Scene/Quest 연결0, **main 직접commit·Push·main병합 모두 미실행**입니다. 대표 최종 승인으로 기록하지 않았고, 제작 담당에게 추가 발주 메시지도 보내지 않았습니다.

[전체 비교](Evidence/WolfV4F1V2_20261021/index.html) · [기술 검사·전체SHA](Evidence/WolfV4F1V2_20261021/Validation.json) · [원본 보호](Evidence/WolfV4F1V2_20261021/Protection.json)

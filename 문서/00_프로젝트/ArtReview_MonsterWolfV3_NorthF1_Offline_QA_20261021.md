# 신규 아트 2패키지 비파괴 검수 — 2026-10-21

**오프라인 검수 완료. 맹독뱀 정적1포즈 미술 PASS / 늑대 FAIL / F1 후보3종 각각 HOLD / F1 전체 HOLD.** 두 ZIP/PNG 파일 기술 형식·과거 정본 해시 일치는 PASS이지만, 기획/납품 QA의 해시·수치 기록에는 정정이 필요합니다. **USER_ART_APPROVAL_PENDING / NOT_UNITY_INTEGRATED.**

이번 이미지 전부는 실제 로컬 PNG로 만든 **오프라인 비교·진단 합성**입니다. Unity GameView를 촬영하거나 Import·Play·Scene·콜라이더를 검증한 결과가 아닙니다. 원본 게임과 서리들쥐 직전 승인 적용은 수정하지 않았습니다.

## 확인 정본

- [기획 검수 게이트](https://github.com/HappyPaul812/Project-Limitless/blob/docs/planning/문서/00_프로젝트/ArtReview_20261021_MonsterWolfV3_And_NorthF1_Sample_ValidationGates.md), blob `85ca090715eedd64f59508d1e4cf9300e57c1825`.
- [Issue43 최신 검수 지시](https://github.com/HappyPaul812/Project-Limitless/issues/43#issuecomment-6305398057), docs/art-renewal 납품 보고 blob `151fd94be9c43f1e16f7390f75fe546b6cbf0d53`.
- [Issue30 최신 검수 지시](https://github.com/HappyPaul812/Project-Limitless/issues/30#issuecomment-6305398736), docs/art-new 후보 보고 blob `11800fcedf8717406f58a0d1fb317cc1123fd468` 및 최초F1 납품 보고 blob `6aa5d016aa3a67735602e83f9157e3b063a0190d`.
- 실제 GitHub main 재조회 SHA `e4e9891f6f01ea95cada9e63b1f09946d94d6041`. 검수 브랜치는 직전 서리들쥐 적용 완료 `4d8f0af3e9c2abb99f9eec6f357c9cd1616cb5d8`에서 분리했습니다. 이번 결과와 그 이전 적용 기능을 혼동하지 않습니다.

## 패키지1 — 맹독뱀·늑대 v3

입력: `F:/Downloads/Limitless_MonsterWolf_KeyArt_v3_Review_20261021.zip`.

| 기술 항목 | 결과 |
|---|---|
| ZIP | SHA `a5189bdb542988da69581135f137b39c98b3c5434272cb5020d13310a98fdae8`, 3,468,949bytes·13항목·CRC PASS |
| PNG | 9개 전체 디코딩 PASS, 원화2개는 각각1254×1254/952×1306 RGBA |
| 원화 동일성 | ZIP 실물2개 SHA가 Manifest의 source hash 및 독립된 10/19 v3 납품 정본과2/2 일치 |
| Manifest | 원화 파일명·크기·용량·SHA PASS. 알파군집 수치 정의는 아래 정정 필요 |
| 게임 준비 상태 | PPU·Pivot·모션·정식 Unity 비교 미확정, 게임용 Sprite Sheet 아님 |

**정확 SHA256**

- Snake: `7001d199b279cb8e541fc603bd62351ab460d3b44da91bc91a2d65dc099f8fd0`.
- Wolf: `30b2d464fe2c456c0fa7d82f1fad51f64065b6e8379ea174548deec7d1b91b4e`.

최신 기획/Issue/10/21 납품 보고의 Snake 문자열 `7001d199b279cb8e541fc603bd62351ab460d3b44da91a2d65dc099f8fd0`는 **60자리로 `bc91`이 빠져 있습니다**. 실제 파일·패키지 Manifest·10/19 보고의64자리 SHA가 일치하므로 파일 변조로 판단하지 않습니다. 원격 문서는 이번에 수정하지 않았습니다. 과거 원화 별도 실파일을 추가로 확보해 직접두파일 비교한 것이 아니라, 보존된 과거 정본 해시와 실물의 암호학적 동일성을 확인한 것입니다.

### 실제 Chapter2 공식 PNG와 같은 전투 표시 슬롯 비교

공식5종의 원본 PNG를 로컬 Unity에서 직접 읽었습니다. 첫 Idle셀314×314 / meta PPU314 / 셀1WU를 확인하고, 기존 실제 전투 Image136×112 / preserveAspect / scale1에 대응하는 크기로 **전체 셀·전체 후보 캔버스**를 비율 유지 Point 축소했습니다. 후보만 bbox를 잘라 크게 맞추지 않았습니다.

- 공식5종의 가시 폭은 약111.64px, 높이91.31~110.22px입니다.
- Snake는 정사각 캔버스112×112 안에서 alpha>16 본체 약105.93×105.75px.
- Wolf는 세로 캔버스 특성상81.64×112 안에서 본체 약73.41×103.77px.
- 이는 기존 Battle Image 슬롯과 같은 **오프라인 투영 비교**이며, 신규 KeyArt의 PPU/월드 체급이 승인됐거나 동일 Camera GameView에서 실측했다는 의미가 아닙니다.

[공식5종·신규2종 동일 슬롯 비교](Evidence/ArtReviewMonsterWolfF1_20261021/Monster_ActualBattleSlot_Offline.png)

### 개별 미술 판정과 문제 위치

| 대상 | 미술 판정 | 근거·후속 |
|---|---|---|
| 맹독뱀 | **PASS — 정적1포즈에 한정** | 112px에서 감긴 몸·들린 머리·입/독니가 읽히며 공식5종과 함께 둬도 정보량·명암이 크게 무너지지 않음. 본체 잘림 재현 없음. 저알파 잔여는 정리 권고이며 사용자 승인·Import 준비와 분리 |
| 늑대 | **FAIL** | 늑대 얼굴·서 있는 실루엣은 읽히지만 털이 실사에 가까워 공식5종의 강조된 덩어리감과 차이. 더 직접적인 반려 사유는 가슴 아래 회색 배경 잔여와 앞발 밑 지면 조각. 어두운 미리보기만으로 놓칠 수 있어 흰색/녹색 합성에서 확인 |

**Snake 알파 수치와 잘림 판단**

- alpha>0 bbox `[0,23,1236,1224]`. 왼쪽 x=0 접촉25픽셀은 **모두 alpha=1/255**: y869~873,992~997,1005~1018. 투명도0.39% 잔여라 본체가 잘린 증거로 확대하지 않습니다.
- alpha>16 bbox `[39,33,1225,1217]`, alpha>128 bbox `[39,34,1225,1217]`. 본체는 캔버스 네 변에 닿지 않습니다.
- 4방향 연결: alpha>0 전체552군집, **16픽셀 미만547개**. 납품의548은 엄격한 `<16` 재계산과 다릅니다. 8방향 연결은184군집·16미만182개로, 대각선 연결 여부에 따라 수가 달라집니다.
- alpha>16에서는8방향2군집이며 작은 것은 **(449,411)의 alpha18 단일픽셀**입니다. alpha>128에서는 본체1군집뿐입니다. 고립 잔여가 실제로 있으나 112px에서 본체 파손으로 보이지 않아 정적 포즈 미술 FAIL로 삼지 않았습니다. 임의 알파 삭제·이미지 수정은 하지 않았습니다.

**Wolf 정확 진단 범위(원본 좌상단 x/y)**

- alpha>0 bbox `[48,48,904,1258]`, 캔버스 경계 접촉0.
- 가슴 아래 두 앞다리 사이 **(540,780)–(760,990)** 확대영역: 불규칙한 회색 매트가 몸 밖 배경 영역에 남아 있음. 안티앨리어싱 가장자리만의 문제가 아닙니다.
- 앞쪽 발·발밑 **(650,1080)–(930,1270)**: 발 아래 바닥 조각/밝은 띠가 남아 지면과 함께 잘라온 인상. 뒤쪽 발·털끝 **(40,1020)–(300,1265)**도 경계 정리 확인 대상.
- 재계산상 alpha>0/16/128 모두8방향1군집, 작은16미만 군집0. 납품 JSON의 Wolf 작은군집1은 맞지 않습니다. **한 덩어리 알파라는 사실만으로 매트가 없다는 뜻은 아닙니다.**

[늑대 결함 위치 확대](Evidence/ArtReviewMonsterWolfF1_20261021/Wolf_Matte_Locations.png) · [늑대 흰/녹색 배경](Evidence/ArtReviewMonsterWolfF1_20261021/Wolf_AlphaBackgrounds.png) · [맹독뱀 흰/녹색 배경](Evidence/ArtReviewMonsterWolfF1_20261021/VenomSnake_AlphaBackgrounds.png)

## 패키지2 — Chapter3 F1 후보3종

입력: `F:/Downloads/Limitless_Chapter3_NorthF1_ArtReview_Samples_v1.zip`.

| 기술 항목 | 결과 |
|---|---|
| ZIP | SHA `dd9963d2a2ce442ad12ecccc8c3ae74edbfb7421eb838003d21ae2df767bd086`, 1,149,681bytes·18항목·CRC PASS |
| PNG·해시 목록 | 전체15PNG 디코딩, 게임후보/기존9PNG 모두256×256 RGBA. SHA256SUMS 17항목 모두 일치 |
| 기존6 보존 | 실제 해시가 10/20 최초 납품 정본 및 QA JSON의 `originals`와6/6 일치 |
| QA 기록 | **HOLD/정정 필요**: 같은 JSON의 `original_preserved_sha256` 표는 실물과0/6 일치. 파일이 변한 것이 아니라 내부 두 표가 충돌 |
| 반복 경계 | 새 Earth/ThinFrost 각각 상하·좌우 가장자리 RGBA 완전 동일, 불투명 Alpha255 PASS |
| 전환부 | Alpha0~106, 좌측 평균Alpha0·우측0.875, 상하 경계 일치·좌우 불일치. 우측 가중 단방향 오버레이이며4방향 Seamless 타일로 승인하지 않음 |

신규 후보 SHA256은 기획·패키지 기록과 모두 일치합니다.

- Earth `573a11195bbc71135441688fca2792fb0fa2de0a7322e400f08275dfcf8c9a18`.
- ThinFrost `6158bdb1962f9f8934a673c767258f1bd2912b22e2ac8250c631fc32e027a803`.
- Transition `6785a24c0332d3f9b5fee13b317576eada7a3fc4e1f0f2839d4c625c6a6844a9`.

잘못된 `original_preserved_sha256` 표가 raw RGBA 픽셀 해시인지도 대조했으나6종 모두 다릅니다. 실제 값·옛 정본·서로 충돌하는 납품 JSON은 증거 폴더에 보존했습니다. 원본 PNG 재저장이나 해시표 임의수정은 하지 않았습니다.

### 후보3종과 전체 승인 상태를 분리한 판정

| 대상 | 미술 판정 | 문제 위치·개선/남은 일 |
|---|---|---|
| Earth 후보 | **HOLD** | 작은 자갈/풀 점은 추가됐지만 기존 올리브색 다각형 얼룩과 큰 음영 형태가 그대로 반복. 256셀 전체의 큰 무늬, 2×2에서 x/y256 간격의 동일 패턴 반복. Ch2 공식에 비해 토양·돌·풀의 덩어리 명암 분리와 깊이가 부족 |
| ThinFrost 후보 | **HOLD** | 회색 전면 도포가 줄어 흙 우세 방향은 개선. RGB 차이>=8 픽셀6.7169%는 재현됐으나 적설면적 지표는 아님. 큰 회백 자갈 같은 서리 조각과 원본의 큰 얼룩 패턴이 반복돼 이른 서리의 소재감/완성도 검토 필요 |
| Transition 후보 | **HOLD** | 단방향이라는 사실 자체는 결함 아님. 다만 셀 우측의 짧은 선/얼음 조각 분포가 지면 재질을 잇기보다 별도 노이즈처럼 읽힘. Earth+overlay→ThinFrost 연결에서 서리 밀도/형태 이행을 다듬을 필요. 좌우 반복/회전4방향 승인 아님 |
| 기존 흙길 | **HOLD 유지** | 기반 흙과 중앙 보행면의 명도/잔무늬가 가까움. 중심부 세로 길띠와 양옆 경계가 새로운 지면의 잔무늬에 묻힘. 점선 같은 가장자리만으로 통행로를 읽게 하지 않도록 후속 보완 필요 |
| 기존 관목 | **HOLD 유지** | 중앙 세로가지·직선 실루엣, 일정한 명암/받침 그림자가 도식적. 새 지면3종이 이 문제를 해결한 것은 아님 |
| 기존 표지판 | **HOLD 유지** | 화살표는 읽히나 판·기둥의 정면 도형/테두리와 재질 명암이 단순. 방향 안내 기능의 판독성과 공식 환경 스타일 완성도를 분리 |
| **F1 전체** | **HOLD / 최종 승인 아님** | 새3종만으로 기존6종·도로·소품·F1 Scene/게임 준비 완료로 확대하지 않음 |

### 같은 타일 반복과 다른 소재 연결 검증

Earth/ThinFrost 각각2×2 반복에는 픽셀 단절선이 없습니다. Earth 오른쪽 끝과 ThinFrost 왼쪽 끝도 RGBA가 정확히 같습니다. 따라서 경계선 기술 검사를 실패했다고 보고하지 않습니다. 반면 **같은 큰 모양의 반복은 여전히 눈에 띄고**, 전환 오버레이의 서리량·표현은 별도 미술 문제입니다. 'seam 수치PASS'와 '자연스러운 최종 지면PASS'는 다릅니다.

공식 Chapter2 `DeepCore_Ground_Base.png`·`DeepCore_Cliff_Boundary.png`의 **실제 원본512PNG**를 로컬에서 읽고 표면 입체감·명암 덩어리·소재 분리를 비교했습니다. 다른 바이옴이므로 검은 화산색을 F1에 요구하지 않았습니다. F1 원본256과 Ch2 원본512를 원래 픽셀 수로 보여주는 판과 F1 128px 축소판을 구분했습니다. F1 PPU/TileGrid/Camera 계약이 없으므로 같은 world unit·실제 표시 배율의 환경 검증으로 주장하지 않습니다.

길의 시각적 경계를 검토했지만 **실제 휠체어 통행폭·Collider·다양한 화면비의 카메라 통행 안전성은 NOT_VERIFIED**입니다. F1 Scene이 없는 상태에서 소스픽셀만으로 안전 통행 PASS를 만들지 않았습니다.

[원본/후보1:1·128px](Evidence/ArtReviewMonsterWolfF1_20261021/F1_Original_Candidate_1to1_Offline.png) · [Earth2×2](Evidence/ArtReviewMonsterWolfF1_20261021/Ground_Earth_Base_Repeat2x2_1to1.png) · [ThinFrost2×2](Evidence/ArtReviewMonsterWolfF1_20261021/Ground_ThinFrost_Base_Repeat2x2_1to1.png) · [전환·길 합성](Evidence/ArtReviewMonsterWolfF1_20261021/F1_Transition_Path_Offline.png) · [기존소품](Evidence/ArtReviewMonsterWolfF1_20261021/F1_Unchanged_Props_Offline.png) · [공식Ch2 원본512](Evidence/ArtReviewMonsterWolfF1_20261021/Ch2_Official_Environment_NativePixels.png)

## 보호·Git·후속

원본 Unity Assets/UserData/Settings/Packages **4,351파일 SHA 변경0**, 원본 Git status 바이트·HEAD 동일. 사용자 두 ZIP 전후 SHA 동일. 원본 Editor는 전후 Bootstrap / PlayOFF / PausedOFF / compiling false / 비포커스 유지. 실제 Unity 도구는 읽기 전용 상태 조회만 했고 Import·Refresh·Play·포커스 변경0입니다.

브랜치 `codex/art-review-monsterwolf-f1-20261021`. 두 패키지의 문서·진단 JSON·비교 이미지·재현 스크립트만 이 검수 브랜치에 커밋합니다. 원본 Unity·기존 승인 아트·Save 변경0, 새 아트 제작·모션 제작·정식 적용0, main 직접commit·Push·병합0입니다. 같은 서리들쥐 적용을 반복하지 않았습니다.

바울 대표께서 맹독뱀 정적1포즈의 최종 미술을 판단하실 수 있습니다. 늑대는 표시한 가슴/발 매트 정리, F1은 지면 큰 무늬·서리 재질·길 가독성/소품 깊이에 대한 표적 보완이 우선입니다. 이번에 아트 담당에게 메시지·새 제작 발주는 보내지 않았습니다. 최종 상태 **USER_ART_APPROVAL_PENDING / WOLF_FIX_REQUIRED / F1_ART_HOLD**.

[전체 비교 페이지](Evidence/ArtReviewMonsterWolfF1_20261021/index.html) · [PNG/알파/해시 검사 원문](Evidence/ArtReviewMonsterWolfF1_20261021/Validation.json) · [원본 보호](Evidence/ArtReviewMonsterWolfF1_20261021/Protection.json)

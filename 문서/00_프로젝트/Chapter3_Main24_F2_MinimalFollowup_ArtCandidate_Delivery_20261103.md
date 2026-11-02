# LIMITLESS / Main24 F2 길 찾기 가독성 — 기존 후보 국소보완 + 새 대안 레퍼런스 / 2026-11-03

**담당:** 메인 신규 아트 / `docs/art-new`  
**이슈:** [#75](https://github.com/HappyPaul812/Project-Limitless/issues/75) / 관련 [#74](https://github.com/HappyPaul812/Project-Limitless/issues/74)  
**상태:** `ART_CANDIDATE / OFFLINE_FILE_QA_PASS / USER_ART_REVIEW_PENDING / F2_ACCESSIBILITY_UNITY_NOT_VERIFIED / NOT_UNITY_APPLIED`.

## 결과와 납품 링크의 차이

- **실물 ZIP**: `Limitless_F2_MinimalFollowup_20261103.zip`; **SHA256 `310d1755b14ab67f744eea420dd1f8e6c41e397fb8689ea2f07b4d7981323a28`**; **1,604,092 bytes, 14 files, ZIP CRC PASS, SHA256SUMS 13/13 PASS**.
- 사용자에게 **ChatGPT 파일 라이브러리** `/LIMITLESS/Main24_F2_Minimal_20261103/Limitless_F2_MinimalFollowup_20261103.zip` 등록 완료. 기존 대화의 sandbox 다운로드 오류가 반복되어 Library 백업 전달로 전환. **GitHub에 PNG·ZIP 바이너리 미업로드**, 사용자 Windows F: 복사·해시 직접 검증 미실시.
- 이 회차는 크게 **서로 다른 2종의 산출물**을 구분한다.
  1. `Candidate/` — 기존 후보의 픽셀·알파를 **국소적으로만 수정한** v3 투명 RGBA 3개. 실제 교체 여부는 미승인.
  2. `Reference_Alternative_Concept/` — 바울 대표가 긍정 평가한 **새로 생성한 3장의 전체 재디자인 콘셉트**를 원본 비율로 3개 셀에 맞춰 정리. 이것은 기존 v2→v3 국소 수정본이 **아니며**, 승인된 게임용 교체 PNG·실제 Main24 맵·Unity 화면으로 취급하면 안 된다.
- `Preview/F2_Minimal_OnePage_BeforeAfter_OFFLINE.png`는 위 두 성격을 **왼쪽 원본 v2 / 가운데 국소 수정 v3 / 오른쪽 새 대안 콘셉트**로 구분해 표시. `Preview/{Sign,Stump,Fence}_128_40_ActualPNG_OFFLINE.png`는 실제 파일로부터 LANCZOS/NEAREST 축소·확대한 **오프라인 가독성 검토**. `Preview/FrostOwl_v3_512_128_40_Readability_ONLY.png`는 승인 정적 원화의 읽힘만 비교함.

## 소스 보존·변경 수치 (전체 SHA 실제 입력/출력 검증)

| 대상 | SOURCE v2 SHA256 | 국소 v3 SHA256 | 바뀐 픽셀 | 변경 bbox 좌·상·우·하 | 알파 변경 |
|---|---|---|---:|---|---:|
| `Candidate/F2_FirstSnowTrailSign_v3_CANDIDATE.png` (288×384 RGBA) | `987b2d8e597d8d77feeb5c75910c5d1b0a0a18d08cbddbc760bc24187fba39ae` | `70b8ed868d61b1abc37d8f775116613ea45352b80fd53fa0cacf7d29a61ce37c` | 1,512 | (97,155,210,195) | **0** |
| `Candidate/F2_SnowStumpPerch_v3_CANDIDATE.png` (320×384 RGBA) | `cef5ca388730c7e86242997401616afbc925fd42d270158bc2ce6b878400a71d` | `eb32bf200da3f08023f0f83b32df9a2f75b9357fa8ffc1f2647b116a5f5b60b8` | 1,795 | (192,157,272,204) | 883 |
| `Candidate/F2_PathEdge_LowFence_v3_CANDIDATE.png` (256×192 RGBA) | `5a79c4ab41e5700dcaac1a68a0929323f10adf0643b7042bf0d3021ee26788ff` | `3aa0a8081ff1a58a4ab7157a974a550fc0b6c57a8263e1a81774b0027edadd6e` | 13,146 | (10,104,245,175) | 13,146 |

**국소 수정 내용과 제한:**
- **Sign:** 화살표·기둥·눈캡·외곽 알파 **그대로 유지**, 과거 가상 글자 제거로 생긴 목재 색면/청색 흔적의 일부를 국소 인페인팅·잔결로 자연스럽게 완화. 1,512 RGB 픽셀만 차이. 목적지 이름·가짜 문자 신규 생성0. 미술 결과 `REVIEW_PENDING`: 더 정교한 수작업 픽셀 마감이 필요할 수 있다.
- **Stump/Perch:** 큰 눈 그루터기, 바위·식생·바닥 KEEP. 오른쪽 원판 상부 가로 (192..271), 세로(157..203) 내에서만 불규칙 외곽 미세 워핑과 나이테 가벼운 명암을 추가. **원판/기둥 큰 구조를 완전히 없앤 작업이 아님**; 눈에 띄는 개선은 작아 미술 최종결정 `ART_REVIEW_PENDING`.
- **LowFence:** 상단 레일·말뚝 원본 바이트 픽셀 유지, 하단 눈판/접지 약한 밝은 영역에만 완만한 알파 감쇠 적용. RGB 변경0. **실제 바위·관목 Collider·도보/휠체어 폭은 본 담당 미검증**, `PATH_VISUAL_CANDIDATE / ACCESSIBILITY_NOT_PASSED`.

## 별도 새 그림 3장(참고/교체 불가)

- `Reference_Alternative_Concept/F2_FirstSnowTrailSign_FULLREDRAW_REFERENCE.png` 288×384 RGBA — SHA `864e964ee4cc3868bd315af2249fe92d04e888937ae66f963232636cda8e4488`.
- `Reference_Alternative_Concept/F2_SnowStumpPerch_FULLREDRAW_REFERENCE.png` 320×384 RGBA — SHA `251fad7f5c6271f4f22dab6eeb4389e75864e4ec11380888b267b8892a435af3`.
- `Reference_Alternative_Concept/F2_PathEdge_LowFence_FULLREDRAW_REFERENCE.png` 256×192 RGBA — SHA `49c2969b8607d91b6bf15cb0e4485bcc3f25379669064b025d63b099c9c62e8c`.
- 위 생성 시안은 색감/외형 구상에만 참고, 원본 3종과 픽셀 시차를 유지하는 국소 수정 결과가 아니며 Unity PPU/Pivot/Scene/실 통행 검증0. 공식 승격·교체0.

## 보호·신규 작업 금지사항

- `F2_FallenSnowBranch_v1` 나뭇가지 `KEEP`, `F2_GentleFirstSnow_Overlay_v1` 약한 눈발 `KEEP`, F2 P1 GroundOverlay/SnowPine/FrostShrub `KEEP_CANDIDATE` 및 LowFence v1/v2 **원본 보존**.
- 승인 Frost Owl v3 정적 원화 **SHA256 `81a31cf37cb834799ab5c9428da612ab13a5a496025805685daefed7b5d3c467`** 바이트 불변/신규 애니메이션 0; Attack 모션 `ART_HOLD` 그대로.
- Main23 승인 환경43, 지면타일9, F1 공식 환경, Orna alpha80px, Residence·Inn KEEP, 신규 문/지붕/간판 HOLD, Monster/Pet·리뉴얼 Spider/Bee, Quest·Save/UserData/BuildSettings·Unity Scene/Prefab/.meta 본편 **수정0**. `main` 직접 commit/Push/merge0.
- **파일 QA와 미술/접근성 QA를 엄격히 분리**: PNG3 decode/RGBA/해상도, ZIP CRC 및 SUMS13/13 PASS; `ART_STYLE_PASS`, `WAYFINDING_ACCESSIBILITY_PASS`, `UNITY_TECH_PASS`는 모두 `NOT_TESTED / USER_REVIEW_PENDING`.
- 후속 게이트: 바울 대표 원페이지에서 **국소 수정 v3 vs 신규 대안 레퍼런스** 방향 결정 → 수용된 후보만 Codex 격리 Unity에서 실제 F2 도보/휠체어 길 시야/PPU/Pivot/Sorting·Collider·16:9/4:3/21:9 DeltaQA → 대표 별도 정식 본편 적용 승인.

**승인여부**: `USER_ART_APPROVED = false`. **현재 납품은 미술 검토 후보이며 공식 자산이 아님**.

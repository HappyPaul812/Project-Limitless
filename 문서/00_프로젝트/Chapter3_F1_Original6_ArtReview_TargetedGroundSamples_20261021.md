# LIMITLESS Chapter3 F1 북풍의 들머리 — 기존 6종 미술 비교와 지면 3종 표적 수정 샘플 (2026-10-21)

> **F1_ART_HOLD / ORIGINAL6_PRESERVED / GROUND_FROST_TRANSITION_SAMPLES_DELIVERED / USER_ART_APPROVAL_PENDING / NOT_UNITY_INTEGRATED**

- 담당 **신규 아트 총괄** `docs/art-new`, Issue [#30](https://github.com/HappyPaul812/Project-Limitless/issues/30).
- 정본: `docs/planning/문서/00_프로젝트/Chapter3_20261021_F1_Main22_FirstPlayable_Priority_And_BossDefer.md`. 우선순위: Main22 첫 플레이(서리들쥐 → F1 → Main22), 설빙의 폭군 보스 애니메이션·Main23/24 후순위.
- 기존 실물 `Limitless_Chapter3_ArtNew_F1_Frostway_Pilot_v1.zip`에서 `F1/`의 6 PNG를 직접 추출·영상 확인. **기존 파일 바이트 원본 ZIP과 SHA-256 재대조 6/6 일치**. 새 샘플은 별도 파일명으로 생성하여 원본을 변경하지 않음.
- Chapter2 비교: `Limitless_Main20_DeepCore_VeinfireColossus_Art_Pack_v3.zip` 내부 **공식** `Environment/DeepCore_Ground_Base.png` 및 `DeepCore_Cliff_Boundary.png` (512×512) 실물 참조. 화염/동토 다른 환경이므로 팔레트나 타일 PPU를 복사하지 않고 소재 밀도·음영·표면 디테일만 비교. 브랜치 문서에 Chapter2 승인 PNG 편집·복제 없음.

## A. 대상 기존 6종 미술 판독

| 기존 F1 PNG 이름 | 크기 | 현재 시각 QA / 사유 | 이번 변경 |
| --- | --- | --- | --- |
| `NorthF1_Ground_Earth_Base.png` | 256×256 RGBA | **HOLD**: 흙·초록 얼룩이 평면적이고 암석/토양 음영 깊이 부족 | 별도 질감 샘플 제시 |
| `NorthF1_Ground_ThinFrost_Base.png` | 256×256 RGBA | **HOLD**: 원본의 넓은 회색 표면이 초기 북풍보다 이미 얼어붙은 지역 인상 | 별도 제한적 서리 샘플 제시 |
| `NorthF1_EarthToFrost_Transition_Overlay.png` | 256×256 RGBA | **HOLD**: 무작위 흰 점 중심, 흙↔서리의 소재 연결감 약함 | 별도 전환부 샘플 제시 |
| `NorthF1_Path_WornTrail_Overlay.png` | 256×256 RGBA | **HOLD**: 흙길과 기반 지면 명도·패턴 차이 적어 좁은 통로 식별에 불리 | 원본 보호, 미술승인 후 후속 최소 보정 |
| `NorthF1_Prop_FrostShrub_01.png` | 256×256 RGBA | **HOLD**: 실루엣은 보이나 단순 직선 가지/표면 명암 깊이 미흡 | 원본 보호 |
| `NorthF1_Prop_Waymarker_01.png` | 256×256 RGBA | **HOLD/PARTIAL**: 방향 표지는 읽히나 Chapter2 공식 장식보다 도식적 | 원본 보호 |

**F1 전체 6종 공식 미술 승인 0.** 원본 PNG 자체 기술 형식이 정상인 것과 화면 미술 완성도는 별개.

## B. 실제 아트 납품

**ChatGPT 대화 다운로드 ZIP:** `Limitless_Chapter3_NorthF1_ArtReview_Samples_v1.zip`  
**ZIP SHA-256:** `dd9963d2a2ce442ad12ecccc8c3ae74edbfb7421eb838003d21ae2df767bd086`  
**파일 구성 18개:** `Original_F1/` 기존 F1 PNG6(원본 비트 동일), `Targeted_Samples/` 신규 후보 PNG3, `Preview/` 4종(원본6 접촉판/전후 128px/오프라인 F1 길 연결/공식 Ch2 비교), `QA/` 반복검수 PNG2+QA JSON1, `Manifest.md`, `SHA256SUMS.txt`.

신규 3 PNG(모두 256×256 RGBA):
- `NorthF1_Ground_Earth_Base_ArtSample_v2.png` SHA256 `573a11195bbc71135441688fca2792fb0fa2de0a7322e400f08275dfcf8c9a18` — 기존 흙·초록 비중을 유지하면서 작은 자갈·흙층·풀 디테일의 실험적 추가.
- `NorthF1_Ground_ThinFrost_Base_ArtSample_v2.png` SHA256 `6158bdb1962f9f8934a673c767258f1bd2912b22e2ac8250c631fc32e027a803` — 흙 지면이 압도적 주색, 작은 서리 흔적만 추가. 새 Earth 후보와의 RGB ≥8/채널 차이가 나는 픽셀 비율 약 **6.7%** (시각적 적설면적 또는 시스템 기후 수치 아님).
- `NorthF1_EarthToFrost_Transition_Overlay_ArtSample_v2.png` SHA256 `6785a24c0332d3f9b5fee13b317576eada7a3fc4e1f0f2839d4c625c6a6844a9` — 투명 우측 가중 서리 분포, 4방향 자율전환 타일 아님.

기존 6 PNG는 **원본 바이트 그대로 재포장**. 이번에 새 PNG를 '게임 공식'으로 교체하지 않음. 아트/미리보기/Manifest는 ChatGPT ZIP으로 제공했으며, **GitHub에는 문서/해시만 기록하고 바이너리는 업로드하지 않았음**.

## C. 오프라인 QA / 불확실성

1. 실제 ZIP `testzip()` PASS, 내부 18개 전부 PNG 디코딩/CRC PASS. 원본 F1 SHA 원본 ZIP과 전부 일치.
2. 신규 Ground 후보 2개 각각 원형 256×256 RGBA/불투명 알파, 좌우 첫·끝 및 상하 첫·끝 픽셀 배열 **완전 일치**. 2×2 Repeat 이미지는 QA 참조. 전환부는 단방향 투명 오버레이이며 Seamless 4방향 판정 아님.
3. `Preview/F1_EarthFrost_Path_Composite_Offline.png`는 **Pillow 합성 예시**, Unity GameView/실제 F1 Scene 촬영이 아님. 휠체어 통로 길·콜라이더·플레이어 Sprite/PPU는 실제 검사하지 않았음.
4. `Preview/Chapter2_Official_Environment_Visual_Reference.png`는 두 ZIP의 이미지 실물과 비슷한 **200px 시각 패널** 비교. Ch2는 다른 바이옴, 원본 512px→200px 미리보기로, 정확한 Unity 실사용 PPU/월드 길이·발 높이 같은 배율을 주장하지 않는다. 128px 미리보기도 그림 축소이지 GameView 실제 크기 아님.
5. 아트 한계: 신규 3샘플은 잔무늬의 미세 보완과 서리 과다를 개선하는 **1차 선택용 수정 샘플**이며, Chapter2 공식 원화의 최종 미술 깊이·캐릭터/휠체어가 구분되는 길·모서리 변형을 합격으로 선언하지 않는다. 지면·길/오버레이 연결의 가시적 완성도는 사용자 미술검수 필요.
6. PPU/TileGrid/Pivot/지형 Wall/Collider/Sorting 등은 최신 **Codex F1 실제 기술 계약 전까지 TBD**, 현재 256px은 PNG 소스 해상도이고 'Unity tile cell'이라 단정하지 않음.

## D. 후속 게이트

- 바울 대표가 흙 우세+절제된 얇은 서리·전환부 방향에 대해 **아트 의견/승인** → 경로 가독성 강화/관목·표지판 깊이의 최소 수정 범위 확정 → 실제 Codex F1 기술/Scene 계약 확인.
- F1 실사용 자산/Unity Import·Main22 Scene·Quest·전투/Save/복귀 검증은 **사용자 미술 승인과 Codex 기술확정 후** 진행.
- 이전 승인 서리들쥐 A Idle/Attack/Hit/Defeat·설빙의 폭군 B·F1 마을 PNG4·Chapter1/2 리뉴얼/Field09·Unity/Save/UserData/Editor 원본 모두 변경 없음.
- 보스 전투 시트·마을 신규 NPC·F2~F5 일괄 제작 미진행. `docs/art-new`만 문서 커밋, **main 직접 커밋·Push·병합 없음**.

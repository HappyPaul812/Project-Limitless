# LIMITLESS / Chapter3 전투 배경 / Issue #91 — v2 지정 영역 표적 보완 실물 납품 (2026-11-04)

**브랜치**: `docs/art-new`. **상태**: `BATTLE_BG_V2_TARGETED_CANDIDATE / OFFLINE_FILE_QA_PASS / USER_VISUAL_REVIEW_PENDING / UNITY_NOT_TESTED / NOT_UNITY_APPLIED`.

## 1. 최신 근거 및 승인 경계

- [Issue #91 총괄 기획 최신 v2 발주](https://github.com/HappyPaul812/Project-Limitless/issues/91#issuecomment-6623743094), [Codex Chapter1 공식 BG3 + Chapter3 2개 동일 1672×941 원본 1:1 비교/수정 영역](https://github.com/HappyPaul812/Project-Limitless/issues/91#issuecomment-6623246887) 확인.
- Codex 공식 Ch1 실제 원화 3장 1:1 비교에서는 재질별 작은 불규칙 색면, 세밀한 명암/수피/원경 표현이 풍부했고 C3 신규 v1은 큰 평면·타원 수관·균일한 설원 조각 위주라는 차이를 지적함. **Ch1 PNG 화소 자체는 본 아트 세션의 작업 컨테이너로 가져오지 못했으므로** 이번 ZIP에 Ch1 공식 원본 이미지를 넣거나 우리 검증에서 원본 Ch1 픽셀 직접 읽기 PASS라고 주장하지 않는다. Codex 원본비교 파일 `F:\Downloads\LIMITLESS_Issue91_Ch1_Ch3_1672x941_1to1_Comparison_20261104.png` SHA `e10aa647089ee9b665058f3428e589b50dbfbdd97b508bf87ca7f361b42ceee9`은 **외부 Codex 보고 근거**임.
- 기존 C3 **v1 원본2 불변**. 승인된 Ch1 6, Ch2 기본색 전장/Field11 DeepCore 지면 재사용, Owl/Spider/ColdVFX/F2·Frostway 승인 환경, Unity/Scene/Prefab/BattleUI/Save/UserData/meta/BuildSettings/Packages, main 변경0. Chapter2 신규 BG 제작0.

## 2. 실제 신규 게임 배경 후보 2 PNG / 원본 해시 보호

**ZIP 실물:** `Limitless_Chapter3_BattleBG_Issue91_v2_TARGETED_20261104.zip`; **7,880,511 bytes**, **24 entries**, SHA256 **`cccb5b92c6fb319ffb7a2c1da213634db05f99c6150277106732738a20b37181`**, ZIP CRC `testzip PASS`, `SHA256SUMS.txt` **23/23 PASS**.
ChatGPT 파일 라이브러리 **`/LIMITLESS/Chapter3_Issue91_BattleBG_v2_20261104/`**에 실물 ZIP 등록 성공. 별도 다운로드 파일은 현재 대화 `/mnt/data/Limitless_Chapter3_BattleBG_Issue91_v2_TARGETED_20261104.zip`. 사용자 Windows `F:\Downloads`에 자동 복사/PC SHA 확인되지 않음. GitHub에는 **문서만 커밋, PNG·ZIP 업로드 없음**.

| 지역 | 기존 v1 PNG 정확한 SHA256 (KEEP) | 신규 v2 PNG 정확한 SHA256 |
|---|---|---|
| Frostway Snowroad | `a5f3bb960b5c5e6fe4140cb2de64e726f4bc7fabb8674f895789fed128adcd25` | `Candidate/C3_BattleBG_FrostwaySnowroad_v2_TARGETED_CANDIDATE_1672x941.png` — `339beeb80d313c589cf50842fc9f6964049b0752e783d83ac8a7097c768205ab` |
| First Snow Forest F2 | `93388ee52fd75da4a82df0b68e03b2083167a25f339d97b41dc71c807125282b` | `Candidate/C3_BattleBG_FirstSnowForest_F2_v2_TARGETED_CANDIDATE_1672x941.png` — `4d57949a1a9d2ae5600d4ba1b95989153fed6a0d8f47f8fc52e6f670a013535a` |

모두 실제 **1672×941 RGB 정적 불투명 PNG**. 승인 원본 v1 별도로 보존, 본 ZIP에는 변경된 v2 후보만 `Candidate/` 수록.

## 3. 표적 작업 영역과 실제 파일 QA

새로운 픽셀아트 v2 구상 두 장을 별도로 만든 뒤, **전체 재생성 이미지를 게임용으로 사용하지 않고 기존 v1의 원본 픽셀에 지정 사각 영역 내부에서만 세부 질감·국소 명암 변화**를 적용했다. 큰 구도·중앙 길/산/수목 위치·장면의 기본 색체계 유지. Chapter1 공식 PNG를 직접 트레이싱·복사하지 않았음. 이미지 출처/보정은 ZIP의 `Docs/SourceDeclaration.md`에 구분.

- **Snowroad S1** 원경 `(650,20)-(1320,475)`: 기존 산/구름 형태는 보호하면서 대칭적 평탄 면에 작은 불규칙 색면·원경 재질 질감을 한정.
- **Snowroad S2** 도로·눈 `(700,535)-(1180,941)`: 기존 중앙 전투 여백/도로 외곽 연결 보존, 재질 입자·경계 세밀함만 보완.
- **F2 F1-L/R** 수관 `(0,0)-(480,330)`, `(1240,0)-(1672,340)`: 기존 눈/잎 수관 덩어리에 재질 명암과 입자만 국소 보완.
- **F2 F2** 오른쪽 수간/밑동 `(1310,240)-(1640,925)`: 바울 대표의 정식 아군 후열 오른쪽 전투 배치가 배경과 경쟁하지 않도록 큰 실루엣 유지, 수피/접지 명암·세부 질감만 보완.

**정량 QA(원본 픽셀 1:1 diff)**:
- Snowroad 수정 픽셀 **455,966 / 1,573,352 (28.98%)**, 수정한 **지정 ROI 밖 변경 픽셀0**, 변경 bbox `(651,22)-(1318,939)`.
- Forest 수정 픽셀 **470,950 / 1,573,352 (29.93%)**, 수정한 **지정 ROI 밖 변경 픽셀0**, 변경 bbox `(1,1)-(1671,924)`.
- 사각 표적 영역이 서로 여러 곳이므로 변경 bbox가 넓어도 **그 바깥 공백 포함 전체 장면 재작업은 없음**. v1의 직접 변경·원본 PNG SHA 변조0. 수정 픽셀 수가 작다는 주장은 하지 않음.
- 파일 decode·RGB/1672×941·ZIP CRC·SHA256 SUMS23/23 PASS = **오프라인 파일 기술 QA**. 정확한 동일 Ch1화풍 미술 PASS·게임실행 PASS가 아님.

## 4. 전후 미술자료·정식 슬롯 가독성 기준

- `Preview/Issue91_v1_v2_TwoScenes_Overview_OFFLINE.png`: 기존 Snowroad/F2 v1과 변경 v2 실제 파일을 동일 배율 축소한 2지역 전후 한눈 비교판.
- `Preview/Issue91_Snowroad_ROI_BeforeAfter_1to1_OFFLINE.png`, `Preview/Issue91_Forest_ROI_BeforeAfter_1to1_OFFLINE.png`: **실제 지정 ROI에서 나온 동일 크기의 원본 비축소 1:1 RGB 크롭**. `Preview/{Snowroad,Forest}_{S1,S2,F1L,F1R,F2}_v1/v2_1to1_CROP.png` 각각10장 수록. 게임용 원본 두 파일 이외의 그림은 비교판.
- `Preview/Issue91_OfficialSlot_2Scenes_OFFLINE.png`: **정식 배치 적 왼쪽(x=.32/.14), 아군 오른쪽(x=.68/.86)**, 아군 3명·적 자리 및 아래 명령/HPMana 위치를 **도식적 가짜 실루엣**으로 표시한 시각 검토 참고. **실제 Unity 기존 공식 UI/실제 3인 Sprite를 캡처한 사진 아님**. 기존 반대 배치 목업은 사용하지 않았다.
- `Manifest.json`, `QA/DeltaQA.json`, `Docs/DesignNotes.md`, `Docs/SourceDeclaration.md`, `SHA256SUMS.txt`, `README_KR.md` 포함.
- **미확인:** 실제 Ch1 원본 PNG 파일과 신규 v2 1:1 실물 동일판(이 ZIP에는 실제 Ch1 원본 수록X); 공식 Battle UI·실제 캐릭터/적 Sprite·960/1280/1680·SafeArea·정식 VFX/이동/접근성 Unity CameraRT. 기존 Codex가 확인한 Ch1 대비 원경/질감 차이를 참고해 보완했지만 **미술 최종 PASS/Unity TECH PASS 선언 금지**.

## 5. 판정 및 다음 승인

`BATTLE_BG_V2_TARGETED_CANDIDATE / OFFLINE_FILE_QA_PASS / USER_VISUAL_REVIEW_PENDING / UNITY_NOT_TESTED / NOT_UNITY_APPLIED`.

바울 대표가 두 지역 v1/v2 미술 시안 및 지정 ROI 1:1 비교·정식 아군 우측 가독성을 승인하면, **그때만 Codex Unity6000.6.5f1 격리**로 실제 UI·아군3·적·960/1280/1680 화면 DeltaQA 검토. 별도 사용자 승인 전 Unity 본편 적용·BattleCatalog 수정·main 직접 commit/Push/merge 금지.

# LIMITLESS / 메인 신규 아트 / Issue #91 — Chapter3 전투 배경 v2 최종 승인용 비교 자료 (2026-11-11)

**브랜치:** `docs/art-new`  
**상태:** `BATTLE_BG_V2_KEEP_2 / FINAL_ART_REVIEW_DELIVERED / USER_VISUAL_REVIEW_PENDING / OFFLINE_ONLY / NOT_UNITY_APPLIED`.

## 1. 발주 및 이전 Codex 자료 재사용

- 근거: [Issue #91 최신 기획 병렬 지시(11월 11일)](https://github.com/HappyPaul812/Project-Limitless/issues/91#issuecomment-6781032360), [Codex 실제 Ch1 3종↔Ch3 v2 2종 1672×941 5패널 원본 1:1 비교 완료](https://github.com/HappyPaul812/Project-Limitless/issues/91#issuecomment-6624969736), [기존 v2 표적 아트 납품](https://github.com/HappyPaul812/Project-Limitless/issues/91#issuecomment-6624133353).
- **기존 Codex 5패널 1:1 자료 재제작0**. 사용자 로컬 경로: `F:/study/codex/Project-Limitless/Temp/Issue91Ch1V2Final20261104/LIMITLESS_Issue91_Ch1_Ch3v2_1672x941_1to1_FinalComparison_20261104.png`, Codex 보고된 SHA256 **`ae44f57176aaf9aac52d169e6e63ef8e2af2a9ff4b3fa8e6833626b77a1b64f3`**. 실물 5144×4400 / 5패널 RGB 화소 5/5 동일·입력5/5 SHA 변경없음은 **Codex 감사 결과**. 본 아트 세션에는 해당 로컬 F:파일의 바이트가 없으므로 **직접 열어 재검증하거나 새 ZIP에 동봉했다는 주장을 하지 않음**.
- Codex 감사 원본 Ch1 3개 SHA: Forest `820f1542ae55c97254ad38df7830d36226b89a7751a6e627dbe715ad75bf7720`, Grassland `ee46adad2ed360f73ddbdc012aed29e90f25b0c9590485985df3a3eaa20ccb98`, SilentCatacombs `92301abcaacb58337edbd1b7506d747565ead795530d24f763ba35072babc767`. Ch1 공식 원본 파일은 이번 ZIP에 넣지 않음.

## 2. 기존 Ch3 v2 2종 원본 보존 (실제 바이트 재검증)

| 대상 | 실제 원본 경로·규격 | 로컬 실제 SHA256 재확인 | 판정 |
|---|---|---|---|
| **Frostway Snowroad v2** | `C3_BattleBG_FrostwaySnowroad_v2_TARGETED_CANDIDATE_1672x941.png`, 1672×941 RGB | `339beeb80d313c589cf50842fc9f6964049b0752e783d83ac8a7097c768205ab` | **KEEP** |
| **FirstSnowForest F2 v2** | `C3_BattleBG_FirstSnowForest_F2_v2_TARGETED_CANDIDATE_1672x941.png`, 1672×941 RGB | `4d57949a1a9d2ae5600d4ba1b95989153fed6a0d8f47f8fc52e6f670a013535a` | **KEEP** |

기존 v2 게임용 배경 PNG 신규 제작·재인코딩·덮어쓰기·v3 제작 **0**. 이번 제출물은 원본 두 장을 그대로 사용해 **별도 OFFLINE 검토판만 2장** 만든 것임.

## 3. 실제 납품 파일·검증

- **ZIP:** `LIMITLESS_Issue91_BattleBG_v2_FinalReview_20261111.zip` (**1,729,054 bytes / 6 entries**). **ZIP SHA256 `4d0ab744ce4e069c62b7ab720870179c0ccc5918ad0ef70024afe57bafafcd69`**, CRC `testzip PASS`, `SHA256SUMS.txt` **5/5 PASS**.
- **요약판:** `Preview/Issue91_C3_BattleBG_v2_FinalApprovalSummary_OFFLINE.png` **1660×1150 RGB**, SHA256 **`850dfb1ff7ed886bc9b8a7cb67e6d6b3b6e4c32cbbb6054897c0f0bb13b1fa52`**. 실제 Snowroad/F2 v2 PNG를 동일 비율·동일 배율로 각각 표시하고 장점/표적 검토/Ch1 대비 근거를 함께 기재. 화풍 검토 문구는 실제 이전 Codex 5패널 조사 결과 **인용 요약**이며 Ch1 원본이 이 이미지에 들어있지 않음.
- **후열 위험 표시판:** `Preview/Issue91_F2_RightRearRisk_OriginalCoordinates_OFFLINE.png` **1770×1240 RGB**, SHA256 **`bda9ba101c899797c5db7c2db007a968db7dad7544810e852c7c7e15614b19c3`**. 실제 F2 v2 배경을 기준으로 좌상단 원본 픽셀 좌표 **오른쪽 수간/밑동 `(1310,240)-(1640,925)`**, 정식 배치 후열 중심 근사 **`(1391,433)`**, 전열 참고 **`(1113,433)/(1113,521)`**를 일관 변환해 표시.
- **정직한 이미지 레벨:** 두 판 모두 **OFFLINE**, Unity GameView/실제 정식 3인 Sprite/HUD/상태이상·VFX 합성/3해상도 QA **아님**. 위험판에 보이는 후열 점/HP 모양은 단순 위치 **도식**, 실제 UI·캐릭터 가림 확정 증거가 아님.
- **문서:** `Docs/StyleReview.md`, `Manifest.json`, `README_KR.md`, `SHA256SUMS.txt` 수록.
- **실제 다운로드:** ChatGPT 파일 라이브러리 `/LIMITLESS/Issue91_BattleBG_v2_FinalReview_20261111/`에 ZIP 및 두 판을 각기 실물 업로드 성공. ZIP과 별개 개별 PNG를 내려받을 수 있게 구성. GitHub 바이너리 업로드0, 사용자 Windows F:\Downloads 직접 저장·PC SHA 미검증.

## 4. 화풍 비교/표적 검토 판정

Codex Ch1 대비: **Forest**는 수피·이끼의 재질/미세 색면, **Grassland**는 풀·흙·바위의 불규칙 질감, **Catacombs**는 석재 균열·건축 디테일 등 고밀도 국소 명암과 거리별 세부 감소가 확인됨. C3 **v2**는 v1 대비 원경·눈길·수피 질감이 늘었으나 **큰 타원 수관·설산 큰 면·직선 길의 평면 구조**는 남음. 따라서 `CH1_STYLE_PARITY_FULL_PASS` 선언하지 않음.

- **Snowroad KEEP:** S1 원경 `(650,20)-(1320,475)` 구름·설산 큰 도형/새 질감 연결, S2 눈길 `(700,535)-(1180,941)` 경계/가까운 눈 조각 반복이 **최종 시각 확인 포인트**. 넓은 중심 전투 공간 보존. 실제 판정된 가독성 결함 없으므로 신규 수정 발주0.
- **F2 KEEP:** 우측 수피·가지 `(1310,240)-(1640,660)`, 밑동/눈·바위 `(1310,660)-(1640,925)`가 오른쪽 후열과 **배경 명도 경쟁 가능성**. 배경이 Sprite보다 앞에 그려져 기하적으로 덮는다는 뜻 아님. 원본 나무·밑동 삭제·재배치0. 실제 캐릭터 실루엣/HP·상태·VFX 위치와 부딪힌다는 증거가 나온 경우만 `TARGETED_FIX_RECOMMENDED`(우측 전경 배경 국소 대비·질감 경감) 검토.
- **공식 기본 좌우 배치:** 적 왼쪽 `x=.32/.14`, 아군 오른쪽 `x=.68/.86`, 기존 Codex UI 근거 사용. 3인/적 실제 스프라이트/피격/행동/VFX 및 UI 기능/960·1280·1680 실현 가독성은 **NOT_VERIFIED**.

## 5. 보존·승인 경계

`BATTLE_BG_V2_KEEP_2 / OFFLINE_ART_REVIEW_READY / USER_VISUAL_REVIEW_PENDING / UNITY_NOT_TESTED / NOT_UNITY_APPLIED`. 바울 대표가 두 원본 배경의 최종 미술 승인/수정 보류 여부를 결정하면 이후 별도 Codex 격리 Unity6000.6.5f1에서 실제 BattleUI/3인·적/화면비 DeltaQA를 검토할 수 있음. **이번에 추가 시각 자료 2장만 제작.** 기존 Ch1/2/3 배경, Owl/Spider/VFX/환경, Unity 원본/Scene/Prefab/Save/UserData/meta/GUID/BuildSettings/Packages/BattleBackgroundCatalog/main 변경0. main 직접 commit/Push/merge0.

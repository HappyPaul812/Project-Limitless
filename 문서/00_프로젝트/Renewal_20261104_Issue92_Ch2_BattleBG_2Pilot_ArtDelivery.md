# LIMITLESS / Chapter 1·2 리뉴얼 / Issue #92 — Chapter 2 Battle Background 독립 파일럿 2종 납품

**작성일:** 2026-11-04 · **담당 브랜치:** `docs/art-renewal` · **상태:** `CH2_BATTLE_BG_2_PNG_TECH_PASS / OFFLINE_STYLE_COMPARISON_PARTIAL / USER_VISUAL_ART_REVIEW_PENDING / NOT_UNITY_APPLIED`.

## 1. 요청 정본과 작업 경계

- [Issue #92 최신 발주](https://github.com/HappyPaul812/Project-Limitless/issues/92)와 [Issue #91의 Ch1·Ch3 실제 1:1 화풍 관찰](https://github.com/HappyPaul812/Project-Limitless/issues/91#issuecomment-6623246887), [최종 Ch1 원화 비교 자료 설명](https://github.com/HappyPaul812/Project-Limitless/issues/91#issuecomment-6624969736), 리뉴얼 역할 문서 `WORLD_ART_RENEWAL.md`, `WORLD_ART_RENEWAL_LATEST.md` 확인.
- 기존 Chapter2 Field04~10은 전용 BattleBG PNG 연결이 없는 기본색 패널 상태이며, Main20/Field11은 `DeepCore_Ground_Base` 지면 재사용. **이번 파일럿은 Field05~08 / Field09~10 공용 후보 2종만 제작**. Field11 신규0.
- 정식 배치 방향 **적 왼쪽(전열/후열 x≈.32/.14), 아군 세 명 오른쪽(전열/후열 x≈.68/.86)**. 휠체어, HP·상태이상, 6스킬 버튼, 턴 UI가 겹치지 않는 가운데 낮은 대비 지면 및 가장자리 오브젝트 배치. 실제 Unity GameView/카탈로그 연결은 **검증0**.

## 2. 실제 신규 이미지와 파일 SHA256

| 지역 후보 | 실제 PNG 파일 | 규격 | SHA256 |
|---|---|---|---|
| 잿빛 화염 황야 Field05~08 | `Candidate/C2_BattleBG_AshenWasteland_Field05_08_v1_CANDIDATE_1672x941.png` | **1672×941 RGB**, 불투명 | **`575832c793fa37734f4ec68c6fb7ddc4076a30c787abdd984635da55dbfc1af3`** |
| 흑요석 균열 지대 Field09~10 | `Candidate/C2_BattleBG_ObsidianRift_Field09_10_v1_CANDIDATE_1672x941.png` | **1672×941 RGB**, 불투명 | **`62d3c4e7bca73a9958f11ac165f196971a162f203bc7686e17b6fee9c5a661f9`** |

**실물 ZIP:** `LIMITLESS_ISSUE92_CH2_BATTLEBG_2PILOT_20261104.zip`, **10,175,488 bytes, SHA256 `e439cd32bda173063ee05fd3cb3fd8a099dc4f3a6a67b805150b2777ba6b7ff0`**, 11 entries, ZIP CRC PASS, `Manifest.json` **9/9 페이로드 bytes+SHA PASS**, `SHA256SUMS.txt` **10/10 항목 PASS** (Manifest 포함, SUMS 자기 자신 제외). 실제 PNG 2장 모두 RGB 디코드 PASS.

**출처·제작 방식** `Docs/SourceDeclaration.md` 기록: 두 장 모두 이번 ChatGPT ImageGen에서 생성한 원화. 기존 Ch1 BattleBG·승인 Ch2 환경 타일/외부 에셋의 픽셀 합성·트레이스·리컬러 아님. Ashen 생성 소스는 원래 1672×941로 새 PNG 재저장만. Obsidian 소스는 1672×940으로 생성되어 **맨 아래 한 행(1px)만 복사해 1672×941로 확장**, 원근 전체를 리스케일하지 않음. 생성원화 SHA는 동봉 출처선언에 기록. 출처 선언은 법적 비침해 보증은 아님.

## 3. 지역별 디자인·UI 가독성

- **Field05~08 / 잿빛 화염 황야:** 중앙 넓은 화산재·돌 지면, 바깥쪽 암석/메마른 나무, 원경 불길과 먼지. 불빛은 캐릭터 슬롯 정면보다 원경에 배치. 중앙 배치영역 BT.709 산술 평균 명도 **83.9/255** (이미지 수치이며 실제 캐릭터 대비 PASS 근거 아님).
- **Field09~10 / 흑요석 균열 지대:** 검은 유리질 층상 절벽, 낮은 발광의 길고 불규칙한 균열, 분할 암반. 재빛 황야와는 지형 구조·실루엣으로 구분됨. 중앙 배치영역 평균 명도 **48.1/255** (어두운 캐릭터·상태아이콘 실게임 대비는 추가 검증 필요). 발광 과다·번쩍임 연출 없음. 배경 자체에 적·아군·휠체어·HUD·글자 없음.
- `Review/C2_BattleBG_EnemyLeft_AlliesRight_3Party_OFFLINE.png`: **1280×720를 참고한 적1 왼쪽·아군3 오른쪽(휠체어 모형 1 포함)·6스킬 HUD 모조실루엣** 두 장면의 비교. 실제 정식 Sprite/Unity HUD 스크린샷 아님. 위치와 대비의 사전 검토용일 뿐 최종 UI/접근성 PASS로 사용 금지.
- `Review/C2_BattleBG_2PNG_Overview_OFFLINE.png`: 두 신규 배경만 나란히 보는 오프라인 원화 미리보기.

## 4. Chapter 1 화풍 비교의 검증 경계 (중요)

- 실제 공식 Ch1 Forest/Grassland/SilentCatacombs PNG는 GitHub main 아래 `Unity/Client/Assets/_Project/Art/Battle/Backgrounds/Chapter1/`에 존재하며 규격 1672×941. [Issue #91 Codex의 실제 1:1 검수](https://github.com/HappyPaul812/Project-Limitless/issues/91#issuecomment-6623246887) 기준 SHA: Forest `820f1542ae55c97254ad38df7830d36226b89a7751a6e627dbe715ad75bf7720`, Grassland `ee46adad2ed360f73ddbdc012aed29e90f25b0c9590485985df3a3eaa20ccb98`, SilentCatacombs `92301abcaacb58337edbd1b7506d747565ead795530d24f763ba35072babc767`.
- **이번 실행 환경에서는 GitHub 공식 PNG 실제 바이트를 가져오지 못했음**. `Review/Ch1_Style_Reference_Criteria_vs_Ch2_OFFLINE.png`에는 이번 신규 2이미지의 실제 픽셀과 **기존 Codex 실물 감사에서 관찰한** Ch1 화풍 기준(수피·이끼/풀·흙·석재의 불규칙 작은 색면과 거리별 명암)을 **문장으로** 표시. **Ch1 원본 PNG 픽셀은 비교판에 포함되지 않았고 직접 1:1 화풍 동등 검증은 미완료**. 다른 임의 이미지를 Ch1 원본으로 오인시키지 않음.
- 대표의 Ch1·Ch2 동일 화풍 최종 승인은 **Ch1 실물 원본을 실제 확보한 뒤 1:1 비교**와 별도 미술 판단 필요. 새 배경 2종을 전면 재제작하라는 결론은 아님.

## 5. 납품 경로·보호 및 다음 단계

- ChatGPT 라이브러리 **`/LIMITLESS_Art_Delivery/Chapter2_Issue92_BattleBG_20261104/`**에 실제 ZIP + 1672 PNG2 + 비교판3 = **총6개 파일 업로드/재목록 확인**. 사용자의 Windows `F:\Downloads`에 자동 저장한 적 없고 GitHub에는 실제 PNG·ZIP 바이너리를 Push하지 않음.
- 기존 Ch1 배경6/Chapter2 공식 환경·몬스터·보스·펫·Spider Attack8·Bee v3·Ch3 BG, Unity 6000.6.5f1/Scene/Prefab/BattleBackgroundCatalog/GUID/meta/Save/UserData/Field11 DeepCore·main 모두 **변경0**. 전체 Field별 전투 배경 양산0. `docs/art-renewal` **문서만 커밋**.
- **판정:** `2_BATTLEBG_PNG_FILE_TECH_PASS / ZIP_HASH_CRC_PASS / OFFLINE_BATTLE_PLACEMENT_MOCKUP / CH1_ACTUAL_PIXEL_COMPARISON_NOT_VERIFIED / 960_1280_1680_REAL_HUD_QA_NOT_TESTED / USER_ART_APPROVAL_REQUIRED / NOT_UNITY_APPLIED`.
- 다음: (a) 대표의 2장 미술 판단, 필요 시 대비/지역감만 최소 표적 보완 (b) Ch1 실제 PNG를 포함한 1:1 화풍 비교 (c) 승인 후 Codex가 Unity **6000.6.5f1 격리 환경**에서 실제 적왼쪽·아군3 오른쪽/휠체어/상태·스킬 960·1280·1680 DeltaQA (d) 본편 카탈로그/씬 적용은 별도 승인.

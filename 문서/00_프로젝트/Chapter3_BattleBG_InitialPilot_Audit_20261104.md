# LIMITLESS / Chapter3 전투 화면 배경 초기 미술 파일럿·Ch1/2 현황 감사 (2026-11-04)

**담당**: 메인 신규 아트 (`docs/art-new`)  
**현재 상태**: `BATTLE_BG_CANDIDATE / OFFLINE_PNG_QA_PASS / USER_VISUAL_REVIEW_PENDING / UNITY_NOT_VERIFIED / NOT_UNITY_APPLIED`.

## 1. 우선 조사: Chapter 1·2 원격 GitHub 실물 / 카탈로그 (READ-ONLY)

2026-11-04 원격 `main` Git tree **7,600개 항목**, [실제 배경 폴더](https://github.com/HappyPaul812/Project-Limitless/tree/main/Unity/Client/Assets/_Project/Art/Battle/Backgrounds/Chapter1), [Chapter1 카탈로그](https://github.com/HappyPaul812/Project-Limitless/blob/main/Unity/Client/Assets/_Project/Resources/BattleBackgrounds/Chapter1.asset), [BattleBackgroundCatalog.cs](https://github.com/HappyPaul812/Project-Limitless/blob/main/Unity/Client/Assets/_Project/Scripts/Battle/BattleBackgroundCatalog.cs), [BattleSceneController.cs](https://github.com/HappyPaul812/Project-Limitless/blob/main/Unity/Client/Assets/_Project/Scripts/Battle/BattleSceneController.cs)와 PNG 메타를 조회. 파일 직접 변경 없음.

| 챕터 / 자산 | 실물 범위 | 적용/참조 근거 | 정확한 판정 |
|---|---|---|---|
| Ch1 독립 PNG **6개** | `BattleBG_Grassland.png`, `Forest`, `Toxic_Swamp`, `Ancient_Underground_Sanctuary`, `Silent_Catacombs`, `Silent_Catacombs_Boss` | `Chapter1.asset`는 Grassland→Field01, Forest→Field02/03, Silent_Catacombs→Dungeon01/B2, Boss→Silent Warden을 연결 | **6개 PNG 보유, 4개 원본이 카탈로그 참조**, ToxicSwamp/AncientSanctuary는 PNG 보유하지만 해당 카탈로그 연결 확인되지 않음. 전체 적용 완료 주장 금지 |
| Ch2 독립 BattleBG | 원격 `main`의 `Art/Battle/Backgrounds/Chapter2` 및 `BattleBackgrounds/Chapter2.asset` **미발견** | `BattleBackgroundCatalog.Find()`는 Main20 전투를 위해 `Resources/Main20/Environment/DeepCore_Ground_Base` 기존 지면 Sprite 재사용; 그 외 미매칭 시 BattleSceneController가 기존 Sky/Distance/Ground/GoldenHorizon 요소를 그림 | **Main20 배경 재사용 확인**. Ch2 다른 지역의 로컬 PC 보관/별도 Scene 실제 전용 BG 존재 여부는 `NOT_VERIFIED_LOCALLY`; 신규 Ch2 제작 금지 |
| Ch3 BG | 원격 `main`에서 `Art/Battle/Backgrounds/Chapter3` **미발견** | Chapter3 전용 카탈로그/Scene 매핑 미확인 | 아래 **두 후보 신규 제작**, 공식 Sprite 미적용 |

**자산 규격 근거:** Ch1 `BattleBG_Forest.png.meta`와 `Grassland.png.meta`에서 실제 스프라이트 영역 `1672×941px`, `spritePixelsToUnits 100`, 중앙 Pivot `(0.5,0.5)`을 확인. **C3 신규 PNG는 동일 1672×941 해상도, RGB 불투명 배경**으로 제작; PPU100/중앙Pivot은 *참고용*이며 Ch3 Unity Import 실제 결정은 `SPEC_PENDING`. `.meta` 생성/런타임 검증 없음.

## 2. 신규 Chapter3 전투 배경 PNG 정확히 2개

- 실제 납품 ZIP: **`Limitless_Chapter3_BattleBackground_2Pilot_20261104.zip`** (**1,600,528 bytes / 12 entries**).
- **ZIP SHA256: `a8c02e9ab6189a6feb074d3f83c8cd6a8bce61ff1b8de110e7c8216934468eb9`**, ZIP CRC `testzip()` PASS, `SHA256SUMS.txt` **11/11 내용 바이트 대조 PASS**. `Manifest.json` 포함. 신규 파일 PNG decode/치수 2/2 PASS.
- **ChatGPT 파일 라이브러리 `/LIMITLESS/Chapter3_BattleBackground_20261104/Limitless_Chapter3_BattleBackground_2Pilot_20261104.zip` 실제 업로드 및 재조회 성공**. 별도 비교판 개별 라이브러리 업로드는 `container_session_expired`로 실패했으나 **ZIP 내부 `Preview/`에는 비교판과 개별 UI 목업이 포함되어 있고 ZIP 무결성 PASS**. GitHub에는 **문서만 커밋**, PNG/ZIP 바이너리 업로드 없음. 대표 PC `F:\Downloads` 복사·동일 SHA 확인 미실시.

| 새 후보 파일 | 해상도·모드 | 실제 SHA256 | 용도 |
|---|---|---|---|
| `Candidate/C3_BattleBG_FrostwaySnowroad_v1_CANDIDATE_1672x941.png` | **1672×941 RGB** | `a5f3bb960b5c5e6fe4140cb2de64e726f4bc7fabb8674f895789fed128adcd25` | 서릿길 마을·북부 설원 일반 전투 배경 |
| `Candidate/C3_BattleBG_FirstSnowForest_F2_v1_CANDIDATE_1672x941.png` | **1672×941 RGB** | `93388ee52fd75da4a82df0b68e03b2083167a25f339d97b41dc71c807125282b` | F2 첫눈의 숲 전투 배경 |

**제작 성격:** 두 파일은 원본 Ch1/2 PNG의 재색칠/이미지 복사가 아닌 **독립 신규 프로시저럴 픽셀 배경**, 기존 승인 Ch3 환경의 설원·청회색 색감과 동선 방향을 참고한 첫 구성 시안. **기존 실제 Ch1·2 이미지 전체의 동배율 화풍 픽셀 비교 및 실제 Ch3 GameView는 하지 못했으므로 화풍 완전 정합 또는 ART PASS 주장 금지**. 본체 후보 PNG에 텍스트/전투 캐릭터/적/HUD/효과/상태 아이콘은 그리지 않음.

## 3. UI·캐릭터 실사용 가독성 검토 자료

- `Preview/Chapter3_BattleBG_2Scene_UI_Comparison_OFFLINE.png`: 두 새 실제 PNG를 동일 960×540에 축소하고, **3명 아군+적의 독립 임시 실루엣, 비공식 공격메뉴·HP·MP UI 목업**을 똑같이 얹은 A/B 보드. 캐릭터 및 기존 Limitless 공식 Sprite·Owl·UI 자산은 **사용·편집하지 않음**. 실제 Unity CameraRT/GameView가 아니라 **OFFLINE** 참고판.
- `Preview/UI_Mock_Snowroad.png`, `Preview/UI_Mock_Forest.png`: 배경별 UI 목업 개별 PNG. 전투 UI 미적용.
- `Docs/DesignNotes.md`: 중앙/하단 미술 밀도 제한, 배경 명도와 적·아군 대비, 16:9 원본 비율, F1→F2 설원 분위기 연계, 향후 빙하 협곡은 **후속 제안일 뿐 미제작** 명시.
- `Docs/Chapter1_2_BattleBG_Audit.md`, `Docs/Chapter1_2_BattleBG_Status.csv`: Ch1 6개/연결현황과 Ch2 Main20 재사용·미확인 현황 한눈 표.
- `Manifest.json`, `SHA256SUMS.txt`, `QA_Result.json`, `README_KR.md` 포함. **ZIP 내 검증 대상 11파일 해시**와 스펙·출처 기록.
- 후보 2개 모두 **게임 공식 규격이라고 인증된 배경이 아닌 미술 1차 파일럿**. Chapter1 기존 1672×941과 크기를 맞췄지만 실제 UI의 960/1280/1680·4:3·21:9에서 표시/크롭/배치, 전투 슬롯·Sorting·PPU·Pivot·명도/접근성은 `UNITY_NOT_TESTED`.

## 4. 원본 보호·승인 상태

- Chapter1 BattleBG **기존 여섯 PNG**, Chapter2 Main20 기존 지면, Ch3 Frostway 승인 지면9/환경43, Owl v3 Idle8/Attack4, Spider 및 Chapter1·2 Renewal, 냉기 VFX v1/v2, F2 후보/승인 원본, 코드·Save/UserData·BuildSettings·Packages·원본 Unity Scene/Prefab/GUID/meta **변경0**. `main` 직접 Commit/Push/Merge0.
- 새 애니메이션/VFX/UI 제작0, 실제 BattleBackgroundCatalog/전투기능 연결0.
- **제작·파일 QA:** `BATTLE_BG_CANDIDATE / OFFLINE_PNG_QA_PASS`.
- **시각 승인:** `USER_VISUAL_REVIEW_PENDING` (대표 판단 전).
- **Unity 기술검증 및 제품 적용:** `UNITY_NOT_VERIFIED / NOT_UNITY_APPLIED`.

**다음 게이트:** 바울 대표가 두 배경의 화풍·명도·적/동료 슬롯 가독성을 먼저 미술 판단 → 승인된 후보만 Codex Unity 6000.6.5f1 **격리 Scene**에서 실제 Battle UI/캐릭터·몬스터, 원본 비율, 화면비별 크롭/기본 구도 DeltaQA → 별도 제품 반영 승인 전 Scene/코드·main 변경 금지. Ch2 로컬 배경 인벤토리는 Codex의 로컬 read-only 조사가 별도로 필요.

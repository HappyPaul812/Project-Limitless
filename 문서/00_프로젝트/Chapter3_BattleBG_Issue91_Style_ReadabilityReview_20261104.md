# LIMITLESS / 메인 신규 아트 / Issue #91 — Chapter3 전투 배경 화풍·가독성 비교 검토 (2026-11-04)

**담당 브랜치:** `docs/art-new`  
**대상:** Chapter3 서릿길 설원 / F2 첫눈의 숲 전투 배경 2종 **원본 KEEP_CANDIDATE**.  
**상태:** `BATTLE_BG_CANDIDATE / KEEP_CANDIDATE_2 / TARGETED_STYLE_REVIEW_ONLY / CH1_STYLE_PARITY_NOT_VERIFIED / OFFLINE_MOCKUP_ONLY / USER_VISUAL_REVIEW_PENDING / UNITY_NOT_TESTED / NOT_UNITY_APPLIED`.

## 1. 기준 문서·실제 검수

- [Issue #91 기획 기준 및 승인 게이트](https://github.com/HappyPaul812/Project-Limitless/issues/91) · [기획 원본 2종 검수 댓글](https://github.com/HappyPaul812/Project-Limitless/issues/91#issuecomment-6622460670) · [Codex Ch1/2 로컬 감사](https://github.com/HappyPaul812/Project-Limitless/issues/91#issuecomment-6622600289) 확인.
- [Chapter3 전투 배경 최초 실물 납품·Ch1/2 사전감사](./Chapter3_BattleBG_InitialPilot_Audit_20261104.md), `docs/art-new` commit `580e55d9ebad719a56bbc15da0215b3224b4cea2`, 현재 `SPRITE_ART_LATEST.md` 확인.
- Codex는 사용자 Windows F: ZIP을 직접 읽고 SHA `a8c02e9ab6189a6feb074d3f83c8cd6a8bce61ff1b8de110e7c8216934468eb9` **F_DRIVE_ZIP_SHA_MATCH** 기록. 로컬 브랜치 `codex/chapter2-battle-background-audit-20261104`, commit `064448a8b52d92f1c506dd2b37c713814be4eb9d` (**LOCAL_COMMITTED/REMOTE_NOT_PUSHED**). 런타임 플레이 검증이 아닌 코드·GUID·Scene 정적 감사임.
- 본 아트 검토에서는 기존 **Chapter3 신규 PNG2와 오프라인 UI 비교판**의 실제 바이트/이미지·SHA를 읽었고, **Chapter1 공식 PNG6의 메타와 카탈로그 결선 자료는 확인하였으나 Ch1 원본 PNG 화소의 1:1 시각 판독은 자료 수급 실패로 수행하지 못함**. 따라서 'Chapter1 공식 배경과 동일 배율 화풍 비교 완료', 'Chapter1보다 도트가 덜 거칠다'는 확정 판정은 **하지 않는다**. 아래에는 Chapter3 자체에서 확인된 보완 후보 부분과 Ch1 대조 시 볼 검사 지점을 엄격히 구분함.

## 2. Ch1·Ch2 감사 최신 정정 (중복 제작 금지)

| 범위 | 확인 결과 | 분류 |
|---|---|---|
| Chapter1 | `Art/Battle/Backgrounds/Chapter1` 공식 PNG 6개, 실제 Catalog 참조 **고유 4개**(Grassland/Forest/Silent Catacombs/Boss). Toxic Swamp·Ancient Sanctuary PNG 보유, 활성 참조0 | `CH1_PNG_6 / ACTIVE_CATALOG_4` |
| Chapter2 Field04~10 | Catalog 매핑 없음 → `NightSky/DistantField/BattleGround/GoldenHorizon` 기본 전장 색상 패널 경로. 특정 신규 전투 배경 이미지가 있는 것처럼 표시 금지 | `DEFAULT_BATTLE_PANEL` |
| Chapter2 Field11 / Main20 | `DeepCore_Ground_Base` **512², PPU128** 기존 월드 지면 텍스처 재사용. 전용 전투 일러스트 세트 아님 | `GROUND_TEXTURE_REUSE` |
| Chapter2 Field10 `BurningPulse_DistantFlameSilhouette` | **1024×512 월드 원경** 장식이며 Battle Catalog 참조 아님 | `WORLD_BG_ONLY` |
| Chapter3 | 신규 독립 배경 PNG2는 현재 `CANDIDATE`, 실제 Catalog/Scene/Unity 화면 적용0 | `KEEP_CANDIDATE` |

Ch1 공식 배경 크기는 `1672×941`이며 Forest/Grassland `.meta`의 `PPU100/중앙Pivot` 확인. 동일 해상도만으로 **화풍·도트 입자 크기 정합을 증명하지 않음**.

## 3. 신규 배경 두 종 원본 보호·오프라인 픽셀 검토

| 후보 원본(모두 1672×941 RGB) | SHA256 정확 값 | 현재 판정 |
|---|---|---|
| `Candidate/C3_BattleBG_FrostwaySnowroad_v1_CANDIDATE_1672x941.png` | `a5f3bb960b5c5e6fe4140cb2de64e726f4bc7fabb8674f895789fed128adcd25` | `KEEP_CANDIDATE / TARGETED_STYLE_REVIEW_ONLY` |
| `Candidate/C3_BattleBG_FirstSnowForest_F2_v1_CANDIDATE_1672x941.png` | `93388ee52fd75da4a82df0b68e03b2083167a25f339d97b41dc71c807125282b` | `KEEP_CANDIDATE / TARGETED_STYLE_REVIEW_ONLY` |

- **이미지 관찰(Snowroad):** 중앙 아군3·적 슬롯용 개방된 눈길, 원경 설산/침엽수 분리. 새 PNG 내 하늘 구름이 큰 타원형 평면/다단 수평띠로 나타나고, 산의 눈 경계가 큰 삼각 실루엣이며, 설원 도로 경계가 직선적인 부분이 보인다. **Ch1 원화보다 부드럽다는 비교 판단은 미검증**. 후보 보완 위치는 원경 구름·산 표면의 입자 구성, 지면 눈의 질감 반복, 중앙 길의 날카로운 경계. 바울 대표의 실제 화풍 비교 전 **수정 파일 0**.
- **이미지 관찰(F2):** 중앙 길 확보와 좌우 전경 커다란 둥근 수관/기둥 프레이밍이 숲을 구분. 좌측 큰 나무가 왼쪽 아군 실루엣과 근접해 **OFFLINE 모의 실루엣 대비 간섭 위험**이 있음. 전경 수관이 큰 타원 덩어리이고, 몇몇 나무 밑동이 넓게 평평한 형태라 접지감 확인 대상. **Chapter1 Forest 원화와 동일 배율 도트 형태 차이의 확정 판단은 미검증**. 후보 수정 위치는 좌측 전경의 점유량·수관 도트 결·나무 접지.
- **재현 가능한 오프라인 수치:** 원본 PNG를 직접 열어 BT.709 가중치(`0.2126R + 0.7152G + 0.0722B`)로 평가한 이미지 전체 평균 명도: Snowroad **166.2/255**, F2 **128.5/255**. 같은 방식 상반부/하반부 Snowroad **155.6/176.7**, F2 **111.3/145.6**. 이는 **신규2종 간 상대 명암**만 비교하며, Ch1 공식 이미지 평균/실제 전투 HUD의 명도 대비 또는 3화면비 가독성 PASS가 아님.
- **기존 실물 ZIP** `Limitless_Chapter3_BattleBackground_2Pilot_20261104.zip`, SHA `a8c02e9ab6189a6feb074d3f83c8cd6a8bce61ff1b8de110e7c8216934468eb9`, 기존 `Preview/Chapter3_BattleBG_2Scene_UI_Comparison_OFFLINE.png` SHA `9f056830c1466c56afb17d13fc639780510e6a688dbc8de9e93e6c2423c0c9cb`. 이번 회차 PNG·ZIP 신규 버전/재제작/교체 **0개**.

## 4. 전투 UI / 아군3·적1 가독성 — 검증 레벨 구분

- 기존 `Preview/UI_Mock_Snowroad.png` 및 `UI_Mock_Forest.png`, `Chapter3_BattleBG_2Scene_UI_Comparison_OFFLINE.png`은 **정식 게임의 플레이어3·서리올빼미·명령창/HP·MP 패널이 아닌 비공식 모의 실루엣과 UI**.
- **Snowroad(오프라인 구도):** 개방된 중앙과 시원한 원근으로 화면 속 아군/적 모형이 구분됨. 설원 바닥 높은 명도가 실제 밝은 캐릭터나 HP 수치의 가독성을 보장하지는 않음. **실 UI 직접 비교/Runtime PASS 아님**.
- **F2(오프라인 구도):** 아군3·적1 모형의 위치는 중앙 경로를 기준으로 읽히나 좌측 커다란 나무·그림자·실루엣과 시각 충돌 가능. 실제 40px 몬스터 표정, HP/MP 글씨, 960/1280/1680×720·4:3/21:9 Safe Area는 미검증.
- 화면에 표시된 명령창/체력 패널의 위치·기능은 게임 정본 레이아웃과 동일하다고 주장하지 않음.
- 필요 후속: **Ch1 공식 Forest·Grassland·Catacombs 원본 PNG 실물 3개**와 신규2를 실제 **1672×941 1:1 동일 크기/동일 샤프닝 및 색공간**으로 비교한 시각판 → 사용자 화풍 판단 → 승인 후보에 한해 Unity6000.6.5f1 격리 실제 아군3·적·UI/3해상도 Delta QA. Ch1 원본 이미지 수급 불가 시 검증을 `NOT_VERIFIED`로 유지.

## 5. 사용자의 보고 문구에 대한 정식 분류

**확인 가능한 확정:** 신규2 PNG·SHA와 구조/구도/오프라인 실루엣 판독, Ch1 고유4 카탈로그 연결·Ch2 기본 패널/DeepCore 지면 재사용, C3 신규2 `KEEP_CANDIDATE`, 신규 아트 제작0.  
**잠정/가능성:** Snowroad 구름의 매끈한 면·지면 평탄성·길 경계, F2 수관 단순성·좌측 아군 가독성 경쟁·일부 나무 접지는 **표적 미술 검토 후보**일 뿐 실제 Ch1 화풍 불일치 확정 근거가 아님.  
**미검증:** Ch1↔C3 공식 원본1:1 도트질감 비교, 원본 실제 BattleUI/실 스프라이트/Unity CameraRT/게임 적정 명도·화면비.  
**금지:** C3 두 PNG 전면 재작업, 공식 Sprite 교체, 원본 Ch1/Ch2 이미지 재제작, Unity/Scene/Prefab/Save/UserData/BattleUI/BuildSettings/Packages/meta 변경 및 `main` 직접 commit·Push·merge.

**완료 상태:** `ISSUE91_STYLE_TRIAGE_DOCUMENTED / BATTLE_BG_CANDIDATE / KEEP_CANDIDATE_2 / OFFLINE_MOCKUP_ONLY / CH1_STYLE_PARITY_NOT_VERIFIED / USER_VISUAL_REVIEW_PENDING / UNITY_NOT_TESTED / NOT_UNITY_APPLIED`.

**후속 승인 게이트:** 실제 Ch1 PNG 실물을 확보해 1:1 원화 비교를 완성하기 전에는 스타일 PASS나 수정 지시를 확정하지 않는다. 바울 대표 미술 승인 후에만 Codex 격리 Unity DeltaQA; 본편 적용은 다시 별도 승인.

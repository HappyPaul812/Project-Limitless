# LIMITLESS — Chapter1·2 비고유 자산 실사용/보관 구분 1차 정적 감사 및 NPC 2종 선행 시안 (2026-10-23)

## 감사의 범위와 정본

- [Issue #57](https://github.com/HappyPaul812/Project-Limitless/issues/57), [2026-10-23 기획 정본](https://github.com/HappyPaul812/Project-Limitless/blob/docs/planning/문서/00_프로젝트/LIMITLESS_20261023_ArtRenewal_NonOriginal_Inventory_And_Main23_FirstPlayable_Kickoff.md), `WORLD_ART_RENEWAL.md`, `Chapter1_2_Monsters_And_Beasts_Original_Art_Renewal_Assignment_20261019.md` 확인.
- **분석 기준 GitHub `main` commit `47ad2eecb40135b2ee28e4947d642ebdbf0bf68a`.**
- 읽기 전용 소스 C#·YAML `.asset`·Sprite `.meta`·ThirdParty LICENSE/README 및 7291개 main Git tree 분석. 이번 조사에서는 사용자 로컬 Unity Editor/GameView 및 실제 실행을 하지 않음. **코드 참조의 존재와 실제 렌더 확인을 혼동하지 말 것.**
- **완료 보호:** 2026-10-22 사용자 승인/통합 Wolf·Bear·Fox Run6, Chapter2 공식 몬스터 11~18 및 보스, 승인 NPC 초상화, Field09 승인 5+B 및 앞선 리뉴얼 환경. 본 작업은 **Scene, Sprite GUID/meta, Prefab, Save/UserData, MonsterDefinition, 런타임 코드, 원본 main 변경0**.

## 1. 확인된 비고유 자산 — ACTIVE 대 STORED 분리

| 범주 | 실제 확인된 내용 | 판정·다음 행동 |
|---|---|---|
| **시작마을 Eldiran 일반 NPC 12** | `StarterVillageHubInstaller`가 정확히 **12 stable ID**를 만들고 `VillageNpcAppearanceCatalog.Apply`가 `Resources.Load<Sprite>(VillageNpcSprites/Eldiran/OGA*.png)` 로드. 각 12 `.meta` 실제 GUID 및 PPU28 확인. 같은 외형 일부가 묘지 입구/Arbel에서도 재사용됨 | **ACTIVE_RUNTIME_CODE_MAPPED 12/12.** 실제 GameView 동시 표시 12/12 및 Collider/Sorting 확인은 **Codex 읽기전용 QA 대기**. NPC 중 최초2만 시안 |
| **MonsterDefinition 18** | `Resources/MonsterDefinitions/01..18` YAML/GUID 실물 확인. `FieldMonsterInstaller`는 sceneName 맞는 `MonsterSpawns`에서 Runtime 생성. Field01 독침벌, Field02 숲거미·맹독뱀 **실제 Spawn.asset이 MonsterDefinition GUID를 참조**, Field03에도 거미·뱀 Spawn 있음. | **ACTIVE_SPAWN_DATA 01..04**. 실제 체감 비고유 확정은 보류. 02 `venom_bee` Sprite GUID가 240000…04/05 형태이므로 실제 매칭 Sprite 현물 필요. 03 `forest_spider`는 `MonsterAnimations/Spider/{Idle,Walk,Attack,Shoot}` 실존, 저작권 확인 필요. 04 `venom_snake`는 별도 32×32 Snake sheet 실행 참조, 승인 권고 고해상도 KeyArt v3와 구별. 05..10은 자체 Art/MonsterSheets/정식 승인 근거 감사 보류. 11..18 **KEEP** |
| **PVFX Foundry 효과** | 원본 `_Project/Art/Effects/ThirdParty/PVFX_Foundry_0.3.0`은 원본 보존용. 별도 `_Project/Resources/BattleSkillEffects` 내 `SpectralBloom`, `RadiantHeal`, `SolarShrapnelCharge`, `WarmExplosion`, `ElectricImpact`, `ArcaneParry`, `EarthRupture`, `FrostNova` 실제 존재. `BattleFireballVisuals`, `BattleCleanseVisuals`, `BattleUiIconCatalog`, `PROJECT_USAGE.md`가 앞선 7개 효과의 실전 연동을 설명. | 효과군별 **ACTIVE_CODE_OR_USAGE_DOC 7그룹**, FrostNova **RESOURCE_ONLY_CALL_UNVERIFIED**. 마커·발사·명중·상태이상 로직 보호 |
| **Kenney UI BattleIcons** | `ThirdParty/Kenney/BoardGameIcons/Resources/KenneyBattleIcons` PNG14, `ThirdParty/Kenney/GameIcons/Resources/KenneyBattleIcons` PNG7. `BattleUiIconCatalog` 코드에서 직접 `KenneyBattleIcons/...` 로드를 사용 | **ACTIVE_RUNTIME_CODE_MAPPED(21개 경로 후보)**. 모든 21개가 화면에서 보이는지는 별도 확인. CC0 |
| **Game-icons.net** | `ThirdParty/GameIconsNet/Path/{Black,White}` 10 PNG, `Material/White` 4 PNG 실제 존재 | **STORED_LIBRARY / UI ACTUAL_USAGE_UNVERIFIED**. Path UI/인벤토리 동적 참조 GUID 검사 전 실사용14개라고 단정 금지. CC BY 3.0 및 개별 CC0 예외 |
| **2DPIXX PilotBee** | `ThirdParty/2DPIXX/PilotBee/README.md`에서 필드·전투 미연결 명시. 사용 후보용 원본 PNG+ZIP+라이선스 보존 | **STORED_ONLY_DOCUMENTED**, `venom_bee`의 실제 Sprite 자산이라고 임의 단정 금지. 원본 LICENSE는 CC BY4.0, OGA 페이지 기록 CC BY3.0 버전 충돌 → 출처 재확인 |
| **기타 외부 팩** | Kenney RPGBase 수백개, Schwarnhild BasicHandDrawn, Xariami EssentialRPG 원본, ScratchIO Animal 원본이 `ThirdParty`에 존재 | **STORED_LIBRARY, 일부 Scene GUID 사용 미검증**. Xariami license 근거 누락 `RIGHTS_UNKNOWN_HOLD`. Schwarnhild는 크레딧+팩 재배포 금지, Kenney는 CC0. 원본 삭제 금지 |
| **환경 보류** | Field09 HeatVent·Obsidian Spire (Issue #38) 미술 QA 잔여, Field10 기존 13종 대표 최종 미술 검토 대기 | **HOLD_QUALITY_ONLY**, 승인 원본/맵 전체 재제작 금지 |
| **펫3 / 공식 몬스터·보스** | 사용자 확인: Wolf·Bear·Fox Run6 최종 승인·main 통합. Chapter2 주요 5종/후반 몬스터 11~18/보스 공식 | **OFFICIAL_KEEP / CHANGE0** |

**수량의 엄밀한 의미:** 현재 **12 NPC가 런타임 로딩 경로에 연결되어 있음**은 코드 직접 확인, **마을 12 NPC의 실제 Play/GameView 표시 성공**과 **프로젝트 전체에 남은 비고유 Sprite의 최종 개수**는 미확정. ThirdParty 폴더 내 파일 수를 '게임 미사용 개수'로 계산하지 않는다.

## 2. 시작마을 12 NPC GUID 및 역할 (기존 자산 유지)

| 역할·stable ID | 현행 Eldiran 스프라이트 | PNG GUID |
|---|---|---|
| 훈련장 `starter-village-training-guide` | OGA02_TrainingGuide | a1020000000000000000000000000001 |
| **잡화상점 `starter-village-general-shop`** | OGA03_GeneralShop | a1030000000000000000000000000002 |
| 치유 `starter-village-healer` | OGA06_Healer | a1060000000000000000000000000003 |
| 주민1 `starter-village-resident-01` | OGA07_Resident01 | a1070000000000000000000000000009 |
| **대표 `starter-village-main-guide`** | OGA09_VillageRepresentative | a1090000000000000000000000000004 |
| 파티 `starter-village-party-manager` | OGA10_PartyManager | a1100000000000000000000000000005 |
| 주민2 `starter-village-resident-02` | OGA11_Resident02 | a1110000000000000000000000000010 |
| 주민3 `starter-village-resident-03` | OGA13_Resident03 | a1130000000000000000000000000011 |
| 은행 `starter-village-bank` | OGA16_Bank | a1160000000000000000000000000006 |
| 장비 `starter-village-equipment-shop` | OGA17_EquipmentShop | a1170000000000000000000000000007 |
| 주민4 `starter-village-resident-04` | OGA19_Resident04 | a1190000000000000000000000000012 |
| 남문 `starter-village-gate-guard` | OGA20_GateGuard | a1200000000000000000000000000008 |

Scene에 Eldiran PNG GUID 직접 직렬화가 없어도 동적 Resources 로드가 사용하므로 'Scene GUID 없음 = 미사용' 판정 금지. `VillageNpcAppearanceCatalog`은 Sorting5, PPU28 원본, scale1, 물리 BoxCollider size(0.5,0.34)/offset(0,0.17), 역할 NPC 이름표. `VillageNpcRole`이 Quest·샵·치유·파티 등 실제 행동을 처리하므로 2D 외형만 격리 교체할 것.

## 3. NPC 첫 두 종만 독립 오리지널 외형 후보 실제 제작 (미술 승인 전)

- **상인 후보** `Eldiran_GeneralShop_Concept_512.png`: 실용적 앞치마·물품 파우치·청록 의상·붉은 머릿수건, 안전한 시장/생활감. stable ID `starter-village-general-shop` 그대로.
- **마을 주민 대표 후보** `Eldiran_VillageRepresentative_Concept_512.png`: 환영하는 인상·진청 외투·목 장식·방향 안내 지팡이. stable ID `starter-village-main-guide` 그대로.
- **실제 후보 실물**: 각 **512×512 투명 PNG**, 원본 SVG, 128px 및 40px 소형 미리보기, 이중 비교 보드, 각 SHA256 Manifest 및 QA. 두 독립 새 디자인만 제작, 12종 전체 양산/게임 적용 없음.
- 두 시안은 **디자인 방향 시안이며 사용자가 공식 원화/성별·목소리/PPU/Pivot/게임 실화면 호환까지 승인한 최종 Sprite가 아니다.** 기존 Eldiran 32×32/PPU28를 임의 덮어쓰면 캐릭터 크기·Collider 문제 발생. 사용자 미술 승인 뒤 Codex가 새 독립 GUID로 `codex/*` 격리 적용/스케일 QA만 실시.
- 파일은 **이 채팅 실물 ZIP** `Limitless_Eldiran_NPC_Pilot2_Concepts_20261023.zip` 및 통합 감사 ZIP `Limitless_Chapter12_NonOriginal_Audit_NPCPilot2_20261023.zip`로 납품; GitHub 브랜치에 그림 바이너리 업로드 또는 바울 대표 PC F: 자동 복사 **없음**.

## 4. 권리/출처

- Eldiran `ThirdParty/Eldiran/RPGCharacters32/LICENSE.txt`: CC0 1.0, 원본 sprite sheet 마젠타 키 제거 파생 Sprite.
- Kenney`ThirdParty/Kenney/.../License.txt`: CC0.
- PVFX `.../PVFX_Foundry_0.3.0/LICENSE.txt` 및 `PROJECT_USAGE.md`: CC0, 출처 참고 추천.
- `ThirdParty/GameIconsNet/license.txt`: CC BY 3.0 기본/일부 작가별 CC0; 제작자 Attribution 목록 관리 필요.
- `ThirdParty/Schwarnhild/BasicHandDrawn/Info_and_License.txt`: 게임 상업 사용·수정 가능, 저작자 크레딧 요청, 자산 자체 재배포/재판매 금지.
- Xariami EssentialRPG: 저장소 내 라이선스 근거 미확인 `RIGHTS_UNKNOWN_HOLD`.
- `ThirdParty/2DPIXX/PilotBee`: 원본 license CC BY4.0와 OpenGameArt페이지 README CC BY3.0 버전 불일치, **현재 미연결**. 권리 확정 전 신규 제품 도입 금지.
- ScratchIO `AnimatedWildAnimals`: CC0, 신규 승인 Wolf/Bear/Fox 완성본과 구분.

## 5. 후속 일감 우선순위 / READ ONLY Codex 계약

1. **Codex 읽기전용 실사용 감사**: `codex/art-renewal-nonoriginal-audit-20261023` 작업 맥락으로 `World_StarterVillage`·Prefab·Scene·Animator·Sprite GUID/fileID+동적 Resource 경로, `MonsterSpawns`와 정의18, `BattleSkillEffects`, `BattleUiIconCatalog`, `PathVisualDefinitions`, Inventory, Field09/10 대상 Renderer 조사. CSV 분류 `ACTIVE_CODE_MAPPED`, `STORED_ONLY_DOC`, `GUID_ACTIVE_RENDER_CONFIRMED`, `UNKNOWN`를 실제 GameView/증거로 업데이트. 로컬 원본 Unity Editor/Play 사용자 작업 방해·수정 금지.
2. 바울 대표 **상인·마을 대표 시안 2종 스타일 승인** → 승인 스타일로 1포즈 게임용 Sprite 실물 제안, 기능/남녀/StableID/Quest/E키/Sorting/Collider 유지. 승인 전 나머지10 일괄 제작 금지.
3. `venom_bee`, `forest_spider`, `venom_snake` 중 실제 현행 비고유·권리 확인된 최우선 1종만 차기 파일럿. 18종 일괄 생산 금지.
4. PVFX 개별 실사용 기능·VFX 마커, Kenney·Game-icons.net UI 중 실제 쓰는 것부터 미술 교체, Field09 HeatVent/Spire·Field10는 기존 승인 맵 보존한 채 개별 QA.
5. 모든 미술 승인 후 **별도 Codex Unity 실제 GameView QA와 대표 승인**을 거쳐야 Runtime 교체 가능.

## 납품·무결성

- 통합 감사 CSV **54행**(NPC12/몬스터18/VFX8/아이콘그룹4/ThirdParty원본6/환경잔여3/보호펫3), SHA256 `5f4b85dbad613d1174a07a5fbc746c734fda8abc6b7bef9dba1c44861f8fe4e5`.
- 한글 보고서 SHA256 `feec267281006ea46b020a1f38ef61fa5c35a02268e280c6e21c63c3056f7e3e`.
- 통합 ZIP `Limitless_Chapter12_NonOriginal_Audit_NPCPilot2_20261023.zip`: **304,209 bytes** / SHA256 `4b9e08538f8669cc00c2a8a9f73db5141f8377fe5a94cf69a9d137e38847b4dd`, **zipfile CRC PASS**. 내부 한국어 보고·CSV54행·두 NPC PNG/SVG/미리보기·Manifest/SHA256SUMS 포함.
- 독립 NPC ZIP `Limitless_Eldiran_NPC_Pilot2_Concepts_20261023.zip`: 291,863 bytes / SHA256 `7162600a51fddffc8a51087c8d1bbb1f8b14c928250ade120c197d23677fb0b3` / CRC PASS.
- `docs/art-renewal`에 이 보고서만 작성. **ZIP/PNG 바이너리 본 repo 업로드0**. `main` Commit/Push/Merge0.

**상태:** `STATIC_RUNTIME_CODE_AUDIT_COMPLETE / LOCAL_UNITY_GAMEVIEW_GUID_CROSSCHECK_PENDING / NPC2_ORIGINAL_DESIGN_CONCEPTS_DELIVERED / USER_ART_APPROVAL_PENDING / NO_RUNTIME_OR_MAIN_CHANGES`.

# StarterVillage 리뉴얼 사전 조사 기준 — 2026-10-09

## 1. 조사 범위와 기준

- LOCAL HEAD: `fbbc7e976382d9960c3e7128af54a5ae2483f34f`. 최근 5개 commit: fbbc7e9 / e75e7b7 / cad75de / 14327fd / 7a78aa8.
- 시작 시 tracked 수정 75개, 최상위 미추적 25항목(전체 status 562행). 현재 Scene·NPC/Player Prefab·무료 환경 meta가 사용자 수정 상태이므로 LOCAL 파일을 기준으로 조사했다.
- AGENTS → PROJECT_CONTEXT → CURRENT_STATUS → CODEX_WORKFLOW → 관련 시스템/코드/Scene/meta 순으로 확인. Editor는 Bootstrap, 비활성 포커스, Play/컴파일 중 아님. Scene 전환·재생성·Import·Play·Asset 저장을 하지 않았다.
- Scene YAML의 GameObject/Transform/SpriteRenderer/Collider와 Runtime 설치 코드를 대조했다. Unity MCP의 메모리 내 읽기 전용 AssetDatabase 조회로 실제 사용 타일 Sprite ID·Rect·PPU·Pivot을 교차 확인했다. 캐시가 LOCAL meta보다 최신이라고 가정하지 않았다.
- 상태: **BASELINE_DOCUMENTED / RENEWAL_NOT_IMPLEMENTED**. 런타임 화면·실제 통행·Continue 재실행은 미검증. 사용자 Save 내용은 열람/변경하지 않고 보호 해시 대상으로만 취급했다.
- 이번 지시의 “해당 문서만 Commit”에 따라 CURRENT_STATUS는 변경하지 않는다.

## 2. 크기·카메라·저장 구조

- 현재 Scene에는 환경 SpriteRenderer **284개**, 환경 BoxCollider2D **11개**, Player/NPC PrefabInstance 각 1개, Main Camera와 DialogueSystem이 있다. 런타임 설치 전 마을 파일 자체에 남문/WorldBounds/12명 전체 NPC가 저장되어 있지는 않다.
- 지면: 정수 X=-9…9, Y=-6…6의 247셀. 128px / PPU128 = 셀 1×1 world unit, Scale(1,1), 회전 0. 시각 외곽 및 Runtime WorldBounds: 중심(0,0), **19×13**, X[-9.5,9.5], Y[-6.5,6.5].
- 남북 길: X=-0.5,+0.5, Y=-6…1, 16셀. 길 면적 X[-1,1], Y[-6.5,1.5]. 지면과 길은 Collider가 없는 시각층이다.
- LOCAL 카메라: Orthographic 4.75, 위치(0,0,-10), viewport 전체. CameraFollow followSpeed=8, Player 대상. 화면 비율에 따라 `min(4.75,6.5,9.5/aspect)`로 높이를 줄이고 viewport 전체를 Bounds 안으로 제한한다. 16:9 예: 화면 16.8889×9.5, 카메라 중심 X±1.0556/Y±1.75. 고정 해상도 계약은 없으므로 실제 지원 비율마다 최종 미술 검토가 필요하다.
- 신규 시작 Scene Player Prefab override 위치 **(0,-5)**. NPC template 저장 위치(1.5,-1.5)는 Runtime main-guide **(0,1.5)**로 이동한다. template 위치를 실제 주민 대표 위치로 전달하면 안 된다.
- SafeZone: `safezone_starter_village`, Scene `World_StarterVillage`, 귀환 `Spawn_From_Field01` (0,-4.65).
- WorldPositionSaveController: 5초 주기 및 pause/quit 저장, 유효 slot만 저장. 이어하기는 같은 Scene의 유한 좌표가 Bounds 안이면 실제 X/Y 복원(속도 0, PendingSpawn 제거), 잘못된 좌표는 Spawn 경로로 대체한다. **Bounds 내부 Collider 점유는 검사하지 않으므로 구조/충돌 이동은 기존 Save 좌표를 막을 수 있다.**
- SceneTransitionService는 이전 좌표를 비우고 목적 Scene/Spawn ID를 전달한다. SceneSpawnPoint는 최대 10프레임 Player 검색 후 배치 성공 시 저장한다. Quest/Inventory/Party 상태나 Save 스키마는 이번 조사에서 변경하지 않았다.

## 3. 환경 자산별 규격 매핑

**교체 단위: 환경 고유 Sprite 15종 / 원본 PNG 11개.** Scene 배치 284개 + 남문 Runtime 활성 울타리 16개 = 300개 환경 렌더러를 15종으로 교체 가능하다. 남문 Prefab에는 18개가 있으나 X±10의 2개는 Runtime에서 비활성화된다. NPC 외형 12종은 별도 교체 후보로, 합산 시 **27종**이다. Main12 기록 지점은 기존 그림이 없는 별도 신규 표현 검토 1곳이며 위 27종에 포함하지 않는다. Player/동료/퀘스트 마커/HUD는 보호·유지 대상이다.

환경 전부 Sorting Layer ID=0(Default), Sprite draw mode Simple, PPU128, 정규화 Pivot(0.5,0.5), Scale(1,1). Point / 무압축 / Mipmap Off를 사용한다. 원본 시트 전체 크기와 실제 사용 Rect 크기를 구분했다. 아래 Rect는 PNG **좌하단** 원점이다.

|역할 / 현재 Sprite|원본 PNG / 해상도|사용 Rect(x,y,w,h)|배치 수(Scene+남문 활성)|Sorting Order|
|---|---|---|---|---|
|tiles_grass_4_0|`ThirdParty/Schwarnhild/BasicHandDrawn/tiles/tiles_grass.png` / 1280×768|512,0,128,128|247|-10|
|tiles_grass_5_4|`ThirdParty/Schwarnhild/BasicHandDrawn/tiles/tiles_grass.png` / 1280×768|640,512,128,128|8|-8|
|tiles_grass_4_4|`ThirdParty/Schwarnhild/BasicHandDrawn/tiles/tiles_grass.png` / 1280×768|512,512,128,128|8|-8|
|house_tiles_new_4_4|`ThirdParty/Schwarnhild/BasicHandDrawn/tiles/house_tiles_new.png` / 768×640|512,512,128,128|2|2|
|house_tiles_new_1_1|`ThirdParty/Schwarnhild/BasicHandDrawn/tiles/house_tiles_new.png` / 768×640|128,128,128,128|2|2|
|fence_tiles_2_2|`ThirdParty/Schwarnhild/BasicHandDrawn/tiles/fence_tiles.png` / 640×640|256,256,128,128|20|2|
|house_tiles_new_1_3|`ThirdParty/Schwarnhild/BasicHandDrawn/tiles/house_tiles_new.png` / 768×640|128,384,128,128|4|2|
|tree_medium|`ThirdParty/Schwarnhild/BasicHandDrawn/assets/tree_medium.png` / 128×156|0,0,128,156|2|4|
|Campfire_Type_A|`ThirdParty/Xariami/EssentialRPG/Props_and_Loot/Campfire_Type_A.png` / 128×128|0,0,128,128|1|2|
|rock_01|`ThirdParty/Schwarnhild/BasicHandDrawn/assets/rock_01.png` / 128×128|0,0,128,128|1|1|
|Wooden_Barrel_Type_A|`ThirdParty/Xariami/EssentialRPG/Village_and_Camp/Wooden_Barrel_Type_A.png` / 128×128|0,0,128,128|1|3|
|bush_01|`ThirdParty/Schwarnhild/BasicHandDrawn/assets/bush_01.png` / 128×128|0,0,128,128|1|1|
|tree_big|`ThirdParty/Schwarnhild/BasicHandDrawn/assets/tree_big.png` / 255×256|0,0,255,256|1|4|
|bush_02|`ThirdParty/Schwarnhild/BasicHandDrawn/assets/bush_02.png` / 128×128|0,0,128,128|1|1|
|Wooden_Chest_Type_A|`ThirdParty/Xariami/EssentialRPG/Village_and_Camp/Wooden_Chest_Type_A.png` / 128×128|0,0,128,128|1|3|

환경 PNG·meta는 원본을 덮어쓰지 않고 향후 `_Project/Art/Environment/StarterVillage/` 등의 별도 자산으로 납품/연결하는 방식이 적합하다(경로 권장안, 이번 작업에서 생성하지 않음). 타일 7종(지면1·길2·건물3·울타리1)은 128×128 사용셀, 나무/덤불/바위/소품 8종은 위 PNG 캔버스를 유지하는 것이 기존 세계 크기 보존에 가장 단순하다.

## 4. LOCAL 배치 및 충돌 매핑

Collider의 center = Transform 위치 + offset, size는 world unit. 아래 모두 Scale(1,1), 비Trigger. 건물 두 채의 Roof/Wall에는 Collider가 없고 Door의 얕은 Collider만 있다. 그림 전체를 건물 Collider로 취급하지 않는다.

|오브젝트|Sprite / 위치|Order|Collider size / offset|
|---|---|---|---|
|TreeSouthWest|tree_medium / (-7, -4.2)|4|(0.4, 0.25) / (0, -0.5)|
|Chest|Wooden_Chest_Type_A / (-2.3, -1.4)|3|(0.45, 0.3) / (0, -0.3)|
|Barrel|Wooden_Barrel_Type_A / (2.5, -1.3)|3|(0.35, 0.3) / (0, -0.3)|
|BushWest|bush_01 / (-5.5, -1.2)|1|없음|
|BushEast|bush_02 / (5.6, -1)|1|없음|
|Campfire|Campfire_Type_A / (0, 1.1)|2|없음|
|TreeEast|tree_medium / (7.5, 1.5)|4|(0.4, 0.25) / (0, -0.5)|
|TreeWest|tree_big / (-7.5, 1.8)|4|(0.45, 0.3) / (0, -0.75)|
|RockNorth|rock_01 / (-1.7, 2.1)|1|없음|
|WallLeft|house_tiles_new_1_3 / (-5, 3.5)|2|없음|
|Door|house_tiles_new_1_1 / (-4.5, 3.5)|2|(1.5, 0.3) / (0, -0.35)|
|WallRight|house_tiles_new_1_3 / (-4, 3.5)|2|없음|
|WallLeft|house_tiles_new_1_3 / (4, 3.5)|2|없음|
|Door|house_tiles_new_1_1 / (4.5, 3.5)|2|(1.5, 0.3) / (0, -0.35)|
|WallRight|house_tiles_new_1_3 / (5, 3.5)|2|없음|
|Roof|house_tiles_new_4_4 / (-4.5, 4.5)|2|없음|
|Roof|house_tiles_new_4_4 / (4.5, 4.5)|2|없음|
|Fence_-7.5_5.3|fence_tiles_2_2 / (-7.5, 5.3)|2|(0.9, 0.2) / (0, -0.35)|
|Fence_-6.5_5.3|fence_tiles_2_2 / (-6.5, 5.3)|2|(0.9, 0.2) / (0, -0.35)|
|Fence_6.5_5.3|fence_tiles_2_2 / (6.5, 5.3)|2|(0.9, 0.2) / (0, -0.35)|
|Fence_7.5_5.3|fence_tiles_2_2 / (7.5, 5.3)|2|(0.9, 0.2) / (0, -0.35)|

- Grass는 모든 정수 좌표 조합 X=-9…9 / Y=-6…6(247). Path는 X=±0.5 / Y=-6…1(16). 위 규칙으로 모든 반복 자산의 좌표를 복원할 수 있다.
- 남문 울타리: X=-9…-2 및 2…9, Y=-5.7(16). Order2, 각 Collider size(0.9,0.2), offset(0,-0.35) → 중심 Y=-6.05. X±10 2개는 유지하되 비활성. 중앙 fence 충돌 간격은 X[-1.55,1.55].
- Runtime 외곽 Collider 두께0.5: Top center(0,6.5),size(19,0.5); Left/Right center(±9.5,0),size(0.5,13); Bottom 좌우 center(±5.5,-6.5),size(8,0.5). 남쪽 실제 Bounds 개방폭3: X[-1.5,1.5].
- 남문 가이드: center(±1.7,-6.9),size(0.4,1.6); SafetyStop center(0,-7.65),size(3.4,0.5). 경계 밖 출구 접근을 제한하는 구조를 시각적 바닥 확장과 혼동하지 않는다.
- 이 좌표는 장애물 제외 전 원시 통로 폭이다. Player Collider 및 NPC body가 차지하는 공간을 포함한 실제 최소 통행 폭과 대각선 끼임은 Runtime 미검증이다. 식생/장식 전체에 충돌이 있는 것은 아니다.

## 5. NPC Stable ID·규격·상호작용

StarterVillageHubInstaller가 template 1명을 주민 대표로 설정하고 11명을 복제한다. 외형 catalog는 Stable ID로 Sprite를 선택하므로 Scene template Sprite만 교체해도 Runtime 외형 12종은 교체되지 않는다.

|Stable ID|실제 Runtime 좌표|기존 PNG|해상도 / PPU / Pivot|Order|
|---|---|---|---|---|
|`starter-village-general-shop`|(-5.8, 1.2)|OGA03_GeneralShop|32×32 / 28 / {x: 0.5, y: 0}|5|
|`starter-village-equipment-shop`|(5.8, 1.2)|OGA17_EquipmentShop|32×32 / 28 / {x: 0.5, y: 0}|5|
|`starter-village-healer`|(-4.4, 2)|OGA06_Healer|32×32 / 28 / {x: 0.5, y: 0}|5|
|`starter-village-bank`|(4.4, 2)|OGA16_Bank|32×32 / 28 / {x: 0.5, y: 0}|5|
|`starter-village-party-manager`|(-6.4, -2.1)|OGA10_PartyManager|32×32 / 28 / {x: 0.5, y: 0}|5|
|`starter-village-training-guide`|(6.4, -2.1)|OGA02_TrainingGuide|32×32 / 28 / {x: 0.5, y: 0}|5|
|`starter-village-resident-01`|(-3.2, -1.2)|OGA07_Resident01|32×32 / 28 / {x: 0.5, y: 0}|5|
|`starter-village-resident-02`|(3.2, -2.6)|OGA11_Resident02|32×32 / 28 / {x: 0.5, y: 0}|5|
|`starter-village-resident-03`|(-7.2, 0)|OGA13_Resident03|32×32 / 28 / {x: 0.5, y: 0}|5|
|`starter-village-resident-04`|(7.2, 0)|OGA19_Resident04|32×32 / 28 / {x: 0.5, y: 0}|5|
|`starter-village-gate-guard`|(2.4, -5.1)|OGA20_GateGuard|32×32 / 28 / {x: 0.5, y: 0}|5|
|`starter-village-main-guide`|(0, 1.5)|OGA09_VillageRepresentative|32×32 / 28 / {x: 0.5, y: 0}|5|

- 모든 NPC: 발 Pivot(0.5,0), PPU28, 32×32 → canvas world 1.142857×1.142857, Scale1. RGBA transparent / Point. Runtime body Box size(0.5,0.34),offset(0,0.17), 비Trigger. template CircleCollider radius0.5/offset0/Trigger는 유지된다.
- 이름 표시 local(0,1.02),Order7(그림자6), Quest marker local(0,1.92),CanvasOrder21. 일반 주민 이름은 비표시. 머리 위 표시를 그림으로 가리지 않는다.
- InteractionSystem의 LOCAL Player 설정은 radius2, 대화 종료 거리는 코드 기본3. 거리로 가장 가까운 NPC를 선택하고 E/F·Gamepad South로 조작한다. Circle 크기가 대화 선택 거리는 아니다. Stable ID·VillageNpcRole·NpcController·NpcInteractionPrompt·NpcQuestMarkerPresenter를 보호한다.
- 같은 Eldiran Resources가 CatacombEntranceSafeZone/Arbel 등에도 사용된다. 시작 마을 리뉴얼을 위해 공유 PNG를 덮어쓰면 안 된다. 마을 전용 Sprite와 ID별 mapping 변경은 별도 구현 작업이다.

## 6. 남문과 Field_01 왕복 계약

|Scene / 역할|ID / 좌표|Trigger / 연결|
|---|---|---|
|StarterVillageSouthGate / Exit_To_Field01|targetScene: Field_01; targetSpawnPointId: Spawn_From_StarterVillage / (0, -6.5)|Trigger (2.5, 0.9)|
|StarterVillageSouthGate / Spawn_From_Field01|spawnPointId: Spawn_From_Field01 / (0, -4.65)|Spawn|
|Field_01 / Spawn_From_StarterVillage|spawnPointId: Spawn_From_StarterVillage / (0, 4.7)|Spawn|
|Field_01 / Entrance_To_StarterVillage|targetScene: World_StarterVillage; targetSpawnPointId: Spawn_From_Field01 / (0, 6.65)|Trigger (2.5, 0.9)|

마을 Spawn(0,-4.65)와 출구(0,-6.5)의 중심거리1.85, Trigger 상단Y=-6.05로 겹치지 않는다. Scene명·Spawn ID·Trigger·경계 개방은 아트 교체 시에도 유지한다. 남문 전용 문 PNG는 없으며 같은 Fence Sprite의 열과 중앙 공백으로 표현한다. 문 아치나 큰 간판은 통로 식별·Order·폭·카메라 경계를 별도 검토해야 한다.

## 7. Main01〜12 동선 / Chapter1 서브퀘스트5

Main01 주민 대표→남문 경비→Field01. Main05는 마을 Scene 로드 도착을 `starter_village_main05_return`로 통지하고 경비/대표에게 보고한다(별도 위치 Trigger 없음). Main12는 잡화상과 `village_main12_records` **(-3,2.5)** 조사 지점을 사용한다. 조사 지점은 Runtime 라벨/Navigation target만 있고 고유 Sprite·Collider가 없다. 신규 기록 책상/게시판 그림을 원하면 추가 아트 계약이 필요하며 기존 조사 좌표와 조작 공간을 유지한다.

아래는 현재 Definition의 목표 TargetId 순서다. Field에서 수행하는 전투/조사와 마을 NPC 접점을 구분하며, Main02~04/06~11의 모든 목표를 마을 구조로 옮기지 않는다.

|Definition|Quest ID|목표 TargetId 순서|
|---|---|---|
|Main01_CallReaches|`main_01_call_reaches`|`starter-village-main-guide` → `starter-village-gate-guard`|
|Main02_GrasslandAnomaly|`main_02_grassland_anomaly`|`field01_main02_investigation_area` → `field01_main02_investigation_encounter` → `field01_main02_tracks`|
|Main03_UnfamiliarCompanion|`main_03_unfamiliar_companion`|`field01_main03_taeon_meeting` → `companion_taeon` → `field01_main03_taeon_encounter` → `companion_taeon` → `field01_main03_next_clue`|
|Main04_ThreePeople|`main_04_three_people`|`field01_main04_miel_meeting` → `companion_miel` → `field01_main04_three_people_encounter` → `companion_miel`|
|Main05_ReturnOfThree|`main_05_return_of_three`|`starter_village_main05_return` → `starter-village-gate-guard` → `starter-village-main-guide`|
|Main06_IntoTheForest|`main_06_into_the_forest`|`field02_main06_entry` → `field02_main06_anomaly_trace`|
|Main07_DeepTracks|`main_07_deep_tracks`|`field02_main07_wheel_tracks` → `field02_main07_wounded_traveler` → `field02_main07_paul_trail` → `companion_paul` → `field02_main07_paul_encounter` → `field02_main07_return_to_miel` → `field02_main07_miel_meeting` → `field02_main07_paul_farewell`|
|Main08_WhatTheyAvoid|`main_08_what_they_avoid`|`field02_main08_avoid_trace_01` → `field02_main08_avoid_trace_02` → `field02_main08_avoid_trace_03` → `field02_main08_paul_wheel_tracks` → `field03_main08_entry` → `field03_main08_investigation_trace` → `field03_main08_deep_zone`|
|Main09_ReunionInSilence|`main_09_reunion_in_silence`|`field03_main09_blue_trace` → `field03_main09_paul_tracks` → `companion_paul` → `companion_paul` → `field03_main09_structural_trace` → `companion_paul`|
|Main10_CenterOfSilence|`main_10_center_of_silence`|`field03_main09_structural_trace` → `field03_main10_stonework` → `field03_main10_silence_direction` → `field03_main10_underground_descent` → `field03_main10_catacomb_entrance` → `field03_main10_catacomb_entrance` → `field03_main10_catacomb_entrance`|
|Main11_SilentCatacomb|`main_11_silent_catacomb`|`dungeon01_main11_b1_central` → `dungeon01_main11_west_ossuary` → `dungeon01_main11_east_chamber` → `dungeon01_main11_lower_gate` → `dungeon01_main11_b2_entry` → `dungeon01_main11_b2_entrance` → `dungeon01_main11_sealed_tombs` → `dungeon01_main11_unknown_room` → `dungeon01_main11_blue_flow` → `dungeon01_b2_boss` → `dungeon01_main11_sealed_chamber` → `dungeon01_main11_broken_tablet` → `safezone_catacomb_entrance`|
|Main12_RemainingRecord|`main_12_remaining_record`|`field03_main12_tablet` → `starter-village-general-shop` → `village_main12_records` → `field03_main12_party_decision` → `field04_west_entry`|

|서브퀘스트|시작/반납 NPC|선행 Quest|수집 Item / 수량|
|---|---|---|---|
|`side_c1_fence_resin`|starter-village-gate-guard / starter-village-gate-guard|main_01_call_reaches|material_grass_slime_gel × 2, material_venom_bee_stinger × 1|
|`side_c1_forest_first_aid`|starter-village-general-shop / starter-village-general-shop|main_06_into_the_forest|material_forest_spider_silk × 1, material_venom_snake_scale × 1|
|`side_c1_grave_fragment`|starter-village-main-guide / starter-village-main-guide|main_11_silent_catacomb|material_grave_wight_fragment × 1|
|`side_c1_night_wing_report`|starter-village-main-guide / starter-village-main-guide|main_08_what_they_avoid|material_shade_bat_wing × 1|
|`side_c1_reinforce_gate`|starter-village-gate-guard / starter-village-gate-guard|main_07_deep_tracks|material_moss_beetle_shell × 2|

소재 수집·반납은 기존 Quest/Inventory 시스템을 재사용한다. 담당 NPC 위치·ID, 도착 판정, 퀘스트 마커 접근 공간을 보존하며 리뉴얼 과정에서 Item/Monster Drop/보상/Save 데이터 변경은 필요하지 않다.

## 8. 무료 에셋 의존성과 교체 경계

|계열|현재 직접 사용|로컬 근거 / 제약|
|---|---|---|
|Schwarnhild BasicHandDrawn|환경 PNG8개, Sprite12종(지면/길/건물/울타리/식생/바위)|Info_and_License.txt: 상업·비상업 사용/수정 가능, 재배포·재판매 금지, 크레딧 요청. 원본 보호|
|Xariami EssentialRPG|PNG3개/Sprite3종(Chest/Barrel/Campfire)|외부에셋.md와 실제 폴더: 로컬 라이선스 문서 없음. 라이선스 확인 미결; 합법성 자동 승인하지 않음|
|Eldiran RPGCharacters32|NPC 파생 PNG12개|로컬 LICENSE 및 외부에셋.md: CC0 기록. 원본 및 다른 거점 공유 파생본 보호|
|Kenney RPGBase|현재 마을 환경284개 및 남문에는 직접 Sprite 참조 없음|레거시 생성기/다른 Scene 사용은 남을 수 있으므로 폴더 삭제 대상 아님|

신규 원화는 기존 무료 파일을 재배포한 묶음이 아니라 프로젝트 전용 독립 납품물로 관리한다. 위 라이선스 내용은 **로컬 동봉 기록 조사**이며 외부 라이선스 최신 확인을 수행한 것은 아니다.

## 9. 구조 유지 교체 / 추가 검토

|분류|대상|조건 / 미결|
|---|---|---|
|구조 유지 가능|환경15종(반복배치300개)|새 Sprite 참조만 연결, PPU/Pivot/Scale/Sorting/좌표/Collider 동일. 건물3부품×2채와 길 좌우 연결 seam 보존|
|연결 코드 검토 필요|NPC12종|Runtime catalog Stable ID mapping 및 공유 Resources 분리. 발 Pivot와 Collider/머리 위 표시 유지|
|신규 표현 설계 검토|Main12 기록 지점1곳|현재 고유 Sprite 없음. 책상/건물/게시판 여부 및 크기 미확정. 새 Collider/Quest 구조는 임의 추가하지 않음|
|구조·Save 회귀 검토|큰 건물 통합, 출입 가능한 실내, 큰 나무, 새 문 아치, 길 확장/맵 확장|현 충돌/통행/카메라/Save 좌표 유지 가능 여부 별도 확인. 기존 저장 좌표 이전 정책을 임의 확정하지 않음|
|보호|Player와길별외형, 태온·미엘, 퀘스트/상점/은행/편성/회복, HUD, Main/Side 진행|이번 교체 목록에 넣지 않음|

## 10. 아트 담당 전달 및 후속 검증

1. 환경15종 + NPC12종으로 기존 대체 자산 목록을 전달한다. 원본 시트 합본 여부/파일명/변형 개수는 아직 정식 Handoff 계약이 아니며 향후 확정한다.
2. 환경은 표의 canvas/cell·PPU128·중앙Pivot, NPC는32×32·PPU28·발Pivot을 기준으로 한다. 고해상도 납품은 같은 world 크기가 되도록 PPU를 비례 조정하는 검토안이며 임의 확정하지 않는다.
3. 건물 조립부 seam, 길 좌우 연결 및 잔디 반복경계, 나무의 바닥 접점, 울타리 통과 불가/중앙개방 구분, NPC 직업의 색상 외 형태 구분을 확인한다. 기존 collider 발자국을 배치표와 함께 제공한다.
4. Main12 기록 지점의 신규 외형, NPC 리뉴얼 포함 여부, 스타일/해상도 확대, 문 아치/건물 합본은 사용자 결정이 남아 있다. 기존15종 환경 대체만으로 구조를 유지하는 최소 리뉴얼은 준비 가능하다.
5. 향후 구현 순서: 정식 Art Handoff/원화 승인 → 별도 프로젝트 PNG Import → Sprite 참조 및 village 전용 catalog 연결 → 통행/화면비/Quest Main01·05·12·Side5/남문 왕복/신규·구버전 Continue 회귀. 재생성 메뉴를 실행하면 LOCAL Scene/meta 변경을 덮을 수 있으므로 교체 수단으로 자동 채택하지 않는다.
6. 이번에는 정적 참조·좌표·규격 조사만 검증했다. Unity 실제 렌더링/상호작용/대화/마커 가림/왕복 이동/저장·불러오기 성공은 **미검증**이며 PASS로 기록하지 않는다.

## 11. 좌표 기반 전체 배치도

비대화형으로 LOCAL 환경 위치 + Runtime 남문/NPC/조사 지점을 투영했다. 실제 Unity screenshot이나 최종 미술 결과가 아니다. 지면247셀은 영역으로 압축 표시한다. 빨강=충돌, 노랑=NPC, 청록=Spawn/출구, 파랑=Main12 지점. 외부 미리보기가 없어도 4~7절 표만으로 좌표를 복원할 수 있다.

![StarterVillage 좌표 배치도](C:/Users/windows/.codex/visualizations/2026/10/09/01a11ec3-5c81-7c13-aed1-db24a0e98dff/village/layout.png)

```text
북 y=+6.5  ┌────────────────── 19 units ──────────────────┐
           │ 북울타리  집NW(-4.5,3.5)   집NE(4.5,3.5)   │
           │       기록(-3,2.5)       은행(4.4,2)        │
           │ 치유(-4.4,2)  대표(0,1.5)                  │
           │ 잡화(-5.8,1.2)      장비(5.8,1.2)          │
           │ 주민03(-7.2,0)      주민04(7.2,0)          │
           │ 주민01(-3.2,-1.2) 주민02(3.2,-2.6)        │
           │ 파티(-6.4,-2.1)   훈련(6.4,-2.1)          │
           │         귀환Spawn(0,-4.65)                │
           │                  경비(2.4,-5.1)          │
           │ 남울타리 y=-5.7   중앙개방 x±1.5           │
남 y=-6.5  └──────────────── Exit(0,-6.5) ──────────────┘
```

## 12. 조사 원본 식별 및 보호

- `Unity/Client/Assets/_Project/Scenes/World_StarterVillage.unity` SHA256 `c86036a1c4dd3472ebf2efc810e9b9886f411e6feee0ff7a3c313910b6540105`
- `Unity/Client/Assets/_Project/Resources/World/StarterVillageSouthGate.prefab` SHA256 `04d5ba49c05b52d757e5620a58aa03857cfb86379e0f1c7cb40ae5523511e8de`
- `Unity/Client/Assets/_Project/Scenes/Field_01.unity` SHA256 `fe0434fac5fba396ca24b12dbf6bd6686face743cdd58ff08b98a094f005ee55`
- `Unity/Client/Assets/_Project/Prefabs/VillageNpcPlaceholder.prefab` SHA256 `de60c373ac05a0942999b0670ae5b343673dfd8b43ac01bf19d7cd7773b5d8d8`

해시 비교 대상은 Git tracked/untracked 및 사용자 UserData/TTS 출력 파일이다. 조사 종료 시 **기존 4,305개 파일 변경 0개**, status 차이는 이 조사 문서 1개뿐임을 확인했다. 환경247셀·길16셀의 전체 좌표 조합과 환경 Sprite의 Scale/Sorting Layer/Simple 모드/PPU/Pivot/Point/무압축/Mipmap Off를 자동 대조했다. Editor는 종료 시에도 Bootstrap·Play Off·포커스 비활성 상태였다.

사용자 수정 타일 meta의 이름/ID 기록을 현재 sprites 목록과 함께 해석했으며, Scene이 사용하는 타일7종은 읽기 전용 AssetDatabase에서도 같은 ID·Rect·Pivot·PPU로 확인됐다. import cache와 LOCAL 파일을 강제로 동기화하거나 meta를 수정하지 않았다.

문서만 staged/commit한다. 작업 보조 JSON/배치도/스크립트는 게임 Asset 바깥 또는 ignored Temp에 두고 commit에 포함하지 않는다. GitHub Push는 실행하지 않는다.

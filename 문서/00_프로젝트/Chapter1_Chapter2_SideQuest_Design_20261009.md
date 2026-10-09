# Chapter1·2 서브 퀘스트 정식 설계 — 2026-10-09

## 계약
QuestType.Side/ActiveSideQuests와 SaveVersion1·슬롯5 유지. 기존 메인·아이템·드롭·Voice ID 보존. Chapter1 EXP155/Talent62, Chapter2 EXP285/Talent111, 합계EXP440/Talent173. 만렙50·경험치 곡선 변경0.

## 퀘스트9
### 끈끈한 울타리 / `side_c1_fence_resin`
- 선행: `main_01_call_reaches` 완료. NPC `starter-village-gate-guard` (남문 경비병), 지역 `World_StarterVillage`. 수락/납품 같은 NPC.
- 보상 EXP20/Talent8. 재료: `material_grass_slime_gel`×2, `material_venom_bee_stinger`×1
- ObjectiveId: `collect_material_grass_slime_gel`, `collect_material_venom_bee_stinger`; CollectItem, TargetId=ItemId. 모든 수집 목표는 병렬 현재보유 수량이다.
- 수락/이유: 울타리 틈을 메울 접착제가 필요합니다. 점액과 독침으로 고정 핀을 만들 수 있겠어요.
- 부족: 재료가 아직 부족합니다. 필요한 수량을 확인하고 천천히 모아 오세요.
- 납품/감사: 덕분에 울타리를 단단히 고정할 수 있겠군요. 고맙습니다.
### 숲길의 응급 묶음 / `side_c1_forest_first_aid`
- 선행: `main_06_into_the_forest` 완료. NPC `starter-village-general-shop` (잡화 상인), 지역 `World_StarterVillage`. 수락/납품 같은 NPC.
- 보상 EXP25/Talent10. 재료: `material_forest_spider_silk`×1, `material_venom_snake_scale`×1
- ObjectiveId: `collect_material_forest_spider_silk`, `collect_material_venom_snake_scale`; CollectItem, TargetId=ItemId. 모든 수집 목표는 병렬 현재보유 수량이다.
- 수락/이유: 숲길을 오가는 이들을 위해 응급 묶음을 준비하려 합니다. 거미줄로 묶고 비늘로 덮개를 만들려고요.
- 부족: 재료가 아직 부족합니다. 필요한 수량을 확인하고 천천히 모아 오세요.
- 납품/감사: 이 재료로 튼튼한 응급 묶음을 만들겠습니다. 잘 가져다주셨어요.
### 버텨 줄 방책 / `side_c1_reinforce_gate`
- 선행: `main_07_deep_tracks` 완료. NPC `starter-village-gate-guard` (남문 경비병), 지역 `World_StarterVillage`. 수락/납품 같은 NPC.
- 보상 EXP30/Talent12. 재료: `material_moss_beetle_shell`×2
- ObjectiveId: `collect_material_moss_beetle_shell`; CollectItem, TargetId=ItemId. 모든 수집 목표는 병렬 현재보유 수량이다.
- 수락/이유: 방책의 닳은 부분을 보강해야 합니다. 단단한 이끼갑충 등껍질 두 개를 부탁드립니다.
- 부족: 재료가 아직 부족합니다. 필요한 수량을 확인하고 천천히 모아 오세요.
- 납품/감사: 이 정도면 방책이 한동안 버텨 주겠군요. 수고하셨습니다.
### 밤을 나는 그림자 / `side_c1_night_wing_report`
- 선행: `main_08_what_they_avoid` 완료. NPC `starter-village-main-guide` (주민 대표), 지역 `World_StarterVillage`. 수락/납품 같은 NPC.
- 보상 EXP35/Talent14. 재료: `material_shade_bat_wing`×1
- ObjectiveId: `collect_material_shade_bat_wing`; CollectItem, TargetId=ItemId. 모든 수집 목표는 병렬 현재보유 수량이다.
- 수락/이유: 밤에 날아다니는 위협을 기록하고 있습니다. 그늘박쥐의 날개막을 살펴보면 방비에 도움이 되겠어요.
- 부족: 재료가 아직 부족합니다. 필요한 수량을 확인하고 천천히 모아 오세요.
- 납품/감사: 직접 확인할 자료가 생겼군요. 주민들에게 주의할 점을 알리겠습니다.
### 무덤에서 돌아온 조각 / `side_c1_grave_fragment`
- 선행: `main_11_silent_catacomb` 완료. NPC `starter-village-main-guide` (주민 대표), 지역 `World_StarterVillage`. 수락/납품 같은 NPC.
- 보상 EXP45/Talent18. 재료: `material_grave_wight_fragment`×1
- ObjectiveId: `collect_material_grave_wight_fragment`; CollectItem, TargetId=ItemId. 모든 수집 목표는 병렬 현재보유 수량이다.
- 수락/이유: 지하묘지에서 돌아온 흔적을 따로 보관하려 합니다. 망자의 파편 한 개를 가져다주시겠어요? 의미를 단정하지 않고 기록하겠습니다.
- 부족: 재료가 아직 부족합니다. 필요한 수량을 확인하고 천천히 모아 오세요.
- 납품/감사: 조각은 안전하게 보관하겠습니다. 확인한 사실부터 차근차근 남기지요.
### 재를 막는 덧댐 / `side_c2_ash_barrier`
- 선행: `main_13_drying_land` 완료. NPC `arbel-leon` (레온), 지역 `Arbel`. 수락/납품 같은 NPC.
- 보상 EXP55/Talent20. 재료: `material_soot_hound_fang`×2, `material_heatwind_hawk_feather`×1
- ObjectiveId: `collect_material_soot_hound_fang`, `collect_material_heatwind_hawk_feather`; CollectItem, TargetId=ItemId. 모든 수집 목표는 병렬 현재보유 수량이다.
- 수락/이유: 문틈으로 날아드는 재를 막을 덧댐이 필요합니다. 송곳니는 고정 핀으로, 깃털은 틈을 채우는 데 쓰려 합니다.
- 부족: 재료가 아직 부족합니다. 필요한 수량을 확인하고 천천히 모아 오세요.
- 납품/감사: 주민들이 재를 덜 맞게 되겠군요. 필요한 곳부터 덧대겠습니다.
### 갈라진 길의 보수 / `side_c2_fissure_repairs`
- 선행: `main_15_burning_traces` 완료. NPC `arbel-leon` (레온), 지역 `Arbel`. 수락/납품 같은 NPC.
- 보상 EXP65/Talent25. 재료: `material_fissure_lizard_scale`×1, `material_ember_beetle_carapace`×1
- ObjectiveId: `collect_material_fissure_lizard_scale`, `collect_material_ember_beetle_carapace`; CollectItem, TargetId=ItemId. 모든 수집 목표는 병렬 현재보유 수량이다.
- 수락/이유: 갈라진 길 가장자리를 임시로 덮으려 합니다. 비늘과 갑각으로 보강판을 만들면 사람들이 지나기 쉬워질 겁니다.
- 부족: 재료가 아직 부족합니다. 필요한 수량을 확인하고 천천히 모아 오세요.
- 납품/감사: 이제 급한 구간부터 보수할 수 있겠습니다. 고맙습니다.
### 사라지지 않는 불씨 / `side_c2_ember_residue`
- 선행: `main_16_shape_in_the_ash` 완료. NPC `arbel-leon` (레온), 지역 `Arbel`. 수락/납품 같은 NPC.
- 보상 EXP75/Talent30. 재료: `material_ember_wraith_ember_shard`×2
- ObjectiveId: `collect_material_ember_wraith_ember_shard`; CollectItem, TargetId=ItemId. 모든 수집 목표는 병렬 현재보유 수량이다.
- 수락/이유: 남은 불씨가 얼마나 오래 열을 품는지 살펴보려 합니다. 잔불 파편 두 개를 안전하게 보관해 비교하겠습니다.
- 부족: 재료가 아직 부족합니다. 필요한 수량을 확인하고 천천히 모아 오세요.
- 납품/감사: 함부로 결론 내리지 않고 열이 식는 과정을 기록하겠습니다. 수고하셨습니다.
### 흑요석 잔해의 쓰임 / `side_c2_obsidian_shell`
- 선행: `main_18_black_heat` 완료. NPC `arbel-leon` (레온), 지역 `Arbel`. 수락/납품 같은 NPC.
- 보상 EXP90/Talent36. 재료: `material_obsidian_beetle_shell`×1
- ObjectiveId: `collect_material_obsidian_beetle_shell`; CollectItem, TargetId=ItemId. 모든 수집 목표는 병렬 현재보유 수량이다.
- 수락/이유: 뜨거운 잔해를 다룰 때 받칠 단단한 조각이 필요합니다. 흑요석 갑각 한 개를 부탁드립니다.
- 부족: 재료가 아직 부족합니다. 필요한 수량을 확인하고 천천히 모아 오세요.
- 납품/감사: 단단한 받침이 생겼군요. 주민들의 작업에 잘 쓰겠습니다.

## 시스템·UI
CollectItem enum을 기존 값 뒤에 추가한다. 현재보유 수량은 매 조회 InventoryService에서 읽고, 판매/획득 이벤트로 HUD/QuestLog/Marker를 갱신한다. 수락 전 보유분 인정·판매 시 감소. 전체 재료/보상 공간/통화 overflow를 소비 전 검증하고 소비→지급→완료기록을 동기 처리, 처리 중 재진입 차단. 실패는 소비하지 않는다. 완료ID가 납품/보상 지급 기록이며 별도 중복 플래그나 SaveVersion 추가 없음.
NPC 선택창에서 퀘스트별 수락/납품 및 기존 대화·업무를 명시 선택한다. 수락/거절·납품 확인·수량·보상·감사 텍스트 제공. 기존 WorldModalState와 입력 차단 사용. 메인 추적을 수락 시 덮어쓰지 않고 QuestLog에서 다른 추적 선택. TTS 제작0/VoiceCatalog 변경0.
Scene/Prefab을 덮어쓰지 않고 기존 stable NPC 역할에 공통 진입점을 연결한다. 신규 UI는 프로젝트 기존 legacy uGUI 텍스트 스타일을 따른다.
Save/Continue는 기존 QuestProgress/Inventory/Currency/Level 필드를 사용한다. 수집 카운트 저장값은 복원 후 현재 Inventory로 재계산한다.
밸런스는 실제 Quest/MonsterDefinition과 ExperienceProgression을 사용하는 대표 경로 시뮬레이션으로 QA에 기록한다. Main20는 설계 보상만 별도 구분하며 종료Lv16~17은 도달 보장값이 아니다.


## EXP 대표 경로 시뮬레이션

기존 MaxLevel50 및 EXP 곡선 유지. Lv1/EXP0 시작, Main01~19의 퀘스트 보상 총705, 서브 전부440 추가. 강제 스토리 전투 EXP는 이 표에 포함하지 않아 0전투 행은 실제 메인 플레이가 아닌 퀘스트 보상만의 하한이다. 2/5전투 행은 각 Main 뒤 일반 적3마리 전투를 해당 횟수만큼 수행하는 가정이며 실제 동선 측정값이 아니다. Main 번호-3을1~14로 제한한 목표 레벨에 가장 가까운 기존 MonsterDefinition을 사용하고 매 전투 현재 레벨 차 배율을 적용한다. 서브는 선행 Main 완료 직후 납품하는 가정이다.

| 서브 전부 | Main당 일반 전투 | Main11 Lv / 잔여EXP | Main19 Lv / 잔여EXP | 누적EXP Main19 |
|---|---:|---|---|---:|
| 없음 |0|3 /25|5 /85|705|
| 전부 |0|4 /10|6 /245|1145|
| 없음 |2|7 /193|12 /850|5250|
| 전부 |2|7 /348|13 /247|5627|
| 없음 |5|10 /405|17 /1110|11910|
| 전부 |5|10 /560|17 /1550|12350|

EXP 필요량100+25×(Lv-1)+5×(Lv-1)². 몬스터가 플레이어보다7레벨 이상 낮으면0%,4~6 낮으면50%, ±3이면100%,4~6 높으면125%,7 이상 높으면150%. 반올림은 기존 구현을 따른다. 총 획득 EXP 차이는 서브 자체440 외에 레벨 상승으로 바뀐 몬스터 배율도 반영한다.

목표 Chapter2 종료 Lv16~17은 전투 빈도에 민감하다. 이 표의 Main19 5전투 가정에서는Lv17이지만 Main20 및 강제 전투를 포함한 실제 Chapter2 완주 레벨은 미검증이다. 심각한 과도 보상으로 확정할 증거는 없고, 실제 동선·전투 분포 수집 후 조정 여부를 별도 결정한다. 이번 작업은 EXP 곡선·몬스터 EXP·기존 보상을 변경하지 않았다.

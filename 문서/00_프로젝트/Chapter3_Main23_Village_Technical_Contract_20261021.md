# Main23 서릿길 마을 기술 계약·변경 파일 계획

2026-10-21 / **TECH_CONTRACT_READY / IMPLEMENTATION_NOT_STARTED / ART_APPROVAL_PENDING**

브랜치 `codex/chapter3-main23-village-preflight-20261021`. 기준 `2631e8750df8d98fb298754f3c82c5ca651c2fda`(Main22 적용준비), 구현 기준 `be7f64a3ff48a8848471166d4aefc4969080bd8b`. 본 브랜치는 기존 구현을 읽기 위해 이를 상속했으며 **이번 변경분만 문서·정적 조사 자료**입니다. 통째 main 병합용 브랜치가 아닙니다. 원본 local main `e4e9891f6f01ea95cada9e63b1f09946d94d6041` 유지.

## 1. 정본과 확정의 범위

[Issue #45](https://github.com/HappyPaul812/Project-Limitless/issues/45) 본문과 댓글0, `docs/planning`의 [10/21 착수 정본](https://github.com/HappyPaul812/Project-Limitless/blob/docs/planning/문서/00_프로젝트/20261021_Chapter3_Main23_Frostway_Village_Art_And_Tech_Parallel_Kickoff.md)을 직접 읽었습니다. 착수 문서 blob `0c0b99a3e90720e8be9128b3456ba1f4698ee695`, Issue가 보고한 기획 commit `1dd71d4c`는 short SHA 그대로 기록합니다. 원격의 세부 정본3개도 직접 확인했습니다.

- [마스터플랜](../03_스토리/Chapter3_서릿길마을_상세마스터플랜_v1_20261019.md): 원격 blob SHA는 아래 Sources의 실제 반환 값을 따릅니다.
- [스토리 시퀀스](../03_스토리/Chapter3_Main23_24_서릿길마을_스토리시퀀스_v1_20261019.md), [NPC·서비스](../03_스토리/Chapter3_서릿길마을_NPC_서비스_서브퀘스트_v1_20261019.md).
- [스킬 정본](../06_스킬/Chapter3_스킬상점_강화_냉기_동결괴사_20261012.md), 기존 Main22 적용준비 문서·플레이 안내·실제 C#/Asset/Scene/Settings를 대조했습니다.
- 원격 응답 원문·URL·blob SHA: [GitHubSources.json](Evidence/Main23VillagePreflight20261021/GitHubSources.json).

**확정 기술 규칙**은 아래 상태 전환·데이터 경계·재사용 인터페이스·비활성 보장입니다. 이름/가격/보상/정식 콘텐츠 ID/Transform까지 기획 확정으로 승격하지 않습니다. 식별자는 충돌 없는 **기술 예약안**, 치수는 물리 QA를 위한 예산으로 구분합니다.

앞선 A 완료 `2631e875`, B 완료 `c47d7ba1`(Wolf 기술PASS/미술FAIL)은 별도 작업으로 유지합니다. Main22 원본 적용·원격 main 통합은 아직 없습니다. 기존 Main22 **136PASS/0FAIL**, F1 **64PASS**, 서리들쥐 **40PASS**를 재사용하며 전수 실행하지 않았습니다.

## 2. Main22→Main23→Main24 상태 계약

`QuestService.GetState`는 prerequisite 완료 여부로 Available을 계산하고, 존재하지 않는 QuestDefinition은 Locked입니다. `TryStart`는 다른 Active Main이 있으면 거절하며 Main Quest 시작 자체를 자동 저장하지 않습니다.

1. Main22의 ID·5목표·완료 쉼터·Save·서리들쥐 승리 경로를 보존합니다. Main22 완료를 Main23 prerequisite로 둡니다. Main22 Asset의 nextMainQuestId를 이용한 강제 연쇄 시작은 하지 않습니다.
2. Main22 완료 후 Main23 Available. F1 쉼터의 명시적인 다음 조사 선택 등 **플레이어 입력**에서 TryStart 성공→Save를 호출합니다. 완료 순간 강제 대화·전송 없음. 이미 Main22 완료한 Save에도 같은 조건으로 노출합니다.
3. F1 심부는 기존 `World_NorthF1` 안에서 연결합니다. 별도 F1 복제 Scene/새 몬스터/지정전투를 만들지 않습니다. 기존 남부 좌표와 Main22의 y8 쉼터·Spawn을 유지하고 북쪽 구간의 조사·출구만 추가합니다.
4. 기존 F1은 Bounds32×24WU, x[-16,16]/y[-12,12], 북벽 닫힘입니다. Main23 요소가 실제 안전 여유와 함께 들어가는지 배치 단계에 확인합니다. 부족하면 **북쪽만** 지면·Bounds·Collider를 일관되게 확장하고 기존 남부 오브젝트를 전체 이동시키지 않습니다. 지금 길이·좌표를 임의 확정하지 않습니다.
5. 마을 최초 진입은 Main22 완료 조건에서 허용합니다. 흔적을 덜 본 상태로 먼저 도착해도 뒤 목표를 소급 완료하지 않으며 F1로 돌아갈 수 있어야 합니다. 두 흔적을 본 뒤의 재입장/현재 Scene 상태 확인으로 정상 목표를 인정합니다. 순서 보장을 위해 새 이동 제한을 기획에 몰래 추가하지 않습니다.
6. Main23 완료 후 자유 이동/정비를 회복합니다. 향후 Main24 Definition의 prerequisite=Main23로 Available이 되되 자동 시작·F2 자동 이동 없음. **현재 Main24 Definition은 없어 Available을 실제 구현했다고 표시하지 않습니다.** 임시 빈 Quest Asset이나 가짜 F2 목적지를 만들지 않습니다.

## 3. 후보 5목표와 재개·중복 방지

|순서|Objective ID 예약안|종류/대상|완료 조건|
|---|---|---|---|
|1|`reach_deep_north_f1`|ReachLocation / `northf1_main23_deep`|Main23 Active 상태에서 플레이어가 검증된 심부 영역에 실제 도착|
|2|`inspect_two_southbound_traces`|Interact / `northf1_main23_traces`, RequiredCount2|동물 흔적과 장작·짐수레 흔적을 각각1회 조사; Main22 A/B와 다른 대상|
|3|`arrive_frostway_south_gate`|ReachLocation / `frostway_main23_arrival`|목적 Scene의 유효 Player 배치 및 PendingSpawn 해제 뒤 도착 인정|
|4|`hear_frostway_testimony`|TalkToNpc / `frostway-guide`|주민 현상 증언·태온/하렌 장면을 끝낸 콜백. 클릭·창 열기만으로 인정하지 않음|
|5|`register_frostway_checkpoint`|Interact / `frostway_main23_checkpoint`|중앙 거점에서 명시 등록, 안전 Spawn 검증→거점 활성화→목표 통지→완료 저장|

5목표를 유지하면서 서로 다른 흔적만 세기 위해 `Main23ClueFlags` 선택 int(2비트)를 QuestProgressSaveData에 추가하는 최소 확장안을 채택합니다. Main22ClueFlags를 공유하지 않습니다. 미래 Main23Progress는 `Observe` 신규 비트일 때만 NotifyInteraction하며 Capture/Restore/Reset을 모두 연결하고 복원은 `flags & 3`. 구버전 JSON 누락=0. ObjectiveCount와 비트 기록을 함께 저장하고 불일치/손상 Save를 어떻게 일관화할지 구현 QA에서 확인합니다. 임의 Quest 전체 구조 개편은 필요 없습니다.

완료/해금/보상은 QuestService 기존 완료 경로 한 곳만 사용합니다. 보상값은 TBD, 무보상 프로토타입은 별도 구현 지시에서 명시될 때만 사용하며 이번에 영구0으로 확정하지 않습니다. 서브퀘스트·상점구매·스킬구매·펫분양·족제비전투는 메인 필수 조건이 아닙니다.

## 4. Scene·Spawn·문·안전지역

|구간|기술 계약|
|---|---|
|F1 북문→마을 남문|`SceneTransitionService.TryLoad(World_FrostwayVillage, Spawn_FrostwayVillage_FromNorthF1)`; Main22 완료 조건/로드 가능 여부를 검증하는 전용 gate 호출부 사용|
|마을 남문→F1 북쪽|`TryLoad(World_NorthF1, Spawn_NorthF1_FromFrostwayVillage)`; 과거 초보마을행 F1남문과 별개|
|마을 체크포인트|`safezone_frostway_village`→World_FrostwayVillage / Spawn_FrostwayVillage_Checkpoint. 입구 Spawn과 분리|
|북문→F2|현재 물리벽과 미개통 안내. 활성 Trigger/목적 Scene/TargetSpawn 등록0. Main24·F2 승인 이후에만 연결|
|동쪽|내부 장식·공간 예약. 포탈/Scene 출구 아님|

기존 SceneTransitionTrigger는 prerequisite/모달/Quest를 검사하지 않습니다. 이를 그대로 붙여 잠긴 문을 열었다고 간주하지 않고, 조건부 호출 컴포넌트 또는 기존 방식의 명시 선택 gate에서 검사합니다. Trigger를 사용하는 실제 Exit만 WorldBoundaryOpening을 만들며 잠긴 북문과 동쪽은 경계 구멍을 내지 않습니다. 명시 선택 gate를 쓸 때는 Main22처럼 물리 stop을 남겨 맵 밖으로 걸어나가지 못하게 합니다. 이 선택의 시각/조작 방식은 기존 Main22와 맞추되 아트 배치 시 결정합니다.

모든 도착 Spawn은 대응 출발 Trigger·상호작용·경계벽/정적 Collider와 겹치지 않아야 합니다. `player collider 반경 + trigger 반경/깊이 + 양의 clearance`를 기준으로 도착 포켓을 분리하고 단순한 중심점 거리만으로 통과시키지 않습니다. SceneTransitionService 중복 로드 방지·실패 시 PendingSpawn/저장좌표 복구를 재사용합니다. 기존 SceneSpawnPoint의 10프레임 Player 대기 및 overlap 보정은 안전망이지 잘못된 배치 허용 근거가 아닙니다.

마을 전체는 몬스터/조우 Spawn0인 안전 Scene입니다. 일반 난로는 장식/기존 휴식이고 보스 온기의 화로·특수 상태 해제·전투 명령 기능을 복제하지 않습니다.

## 5. Save/Continue·패배 복귀

- GameSaveService Version1의 CurrentSceneId/SpawnPointId/실제 좌표와 LastSafeZone3필드를 재사용합니다. 별도 Main23 Save 파일·슬롯·UserData 이관 없음.
- `WorldPositionSaveController`는 유효 Scene/Bounds 안의 유한 좌표를 우선 복원하며 실패 시 기존 PendingSpawn fallback,5초 주기 저장을 사용합니다. **Bounds 안이라도 건물 Collider 내부인지 검사하는 복원은 현재 없습니다.** 새 고정 건물 안에 기존 저장 위치를 배치하지 않고, 배치가 바뀌는 경우 목표 Spawn의 실제 비충돌 복원까지 별도 검증해야 합니다.
- Save.Validate는 Application.CanStreamedLevelBeLoaded(CurrentSceneId)를 검사합니다. 새 마을 BuildSettings 활성 등록이 선행되어야 하며 파일 이름만 추가한다고 Continue가 완성되지 않습니다. 기존 설정의 configObjects(app-ui 포함)·Scene순서는 보존합니다.
- 입장 시에는 도착 위치 저장만 수행합니다. 주민 증언 전에 최종 거점 목표를 자동 완료하지 않습니다. 체크포인트 실제 등록 시 안전 Scene/Spawn 확인→ActivateSafeZone→마지막 목표 Notify 순으로 호출해야 QuestService.Complete의 자동 Save에 새 거점이 포함됩니다.
- GameSessionData.ActivateSafeZone는 빈값만 막고 Scene/Spawn 실재성을 검사하지 않습니다. BattleSceneFlow 패배복귀는 LastSafeZone Scene/Spawn으로 이동하므로 등록 호출부/QA에서 실재성과 로드 성공을 보장합니다.
- `SaveCurrentSession`은 bool을 반환하지만 QuestService.Complete는 현재 실패를 롤백하지 않습니다. 등록·완료 후 최종 저장 성공 확인, 실패 안내/재시도 시 무보상중복·메모리완료/디스크미완료 복원을 검사하는 항목을 둡니다. 기존 코드를 원자적 디스크 트랜잭션이라고 과장하지 않습니다.
- Main23 단계별 Continue/흔적1개 Save/마을도착-증언중단/등록전후/다른 슬롯 Reset과 구버전Main22 Save를 격리 복사본으로 확인할 계획입니다. 새 Scene 저장을 옛 실행본에서 읽는 역방향 호환은 보장하지 않습니다.

## 6. 서비스 재사용과 실제 미구현

|역할 가칭|재사용/추가 연결|한계·보호|
|---|---|---|
|오르나/안내|NpcController+VillageNpcRole+Main23 전용 대화 콜백|이름/TTS/Dialogue ID TBD. 증언 완료 뒤 목표 통지, Quest 로그에 핵심 정보 유지|
|도아/여관회복|VillageNpcRoleType.Healer→PartyResourceService.HealPartyFully→Save|현재 확인창·파티HP/MP회복·저장 경로 그대로. 숙박료/시간/특수버프 신설 없음|
|베른/파티|PartyManager→PartyManagementPresenter.OpenAt|기존 Main05 완료 조건 재사용. 플레이어+동료2명,하렌 자동4인 편성0. 조작 잠금/취소 재사용|
|펫분양/관리|SafeZonePetServiceRollout Sites에 새 안전 거점1개를 등록; 기존 두 역할/Presenter/BeastCompanionService 사용|현재 Sites는 초보마을/Arbel/Field03만 있어 자동 지원 아님. Main21 완료 gate·사수/세린 정책·발견/소유/장착/100탈렌트 기존규칙 유지. 수동NPC와 자동생성 중복 금지|
|마레/보급|새 ShopDefinition→ShopPresenter.OpenCatalog→ShopService/Inventory/Economy|GeneralShop 기본분기는 새 NPC를 StarterShop으로 보내므로 명시 새 catalog 연결 필요. 품목·가격 TBD. 잔고부족/최대stack/overflow/취소의 기존 원자적 거래 재사용|
|루덴/스킬판매|미래 전용 스킬판매 데이터/서비스/UI→승인된 정의 등록+공용Economy|**기존 ShopService는 Item ID/Inventory 전용**, 스킬 판매로 즉시 재사용 불가. 아래 별도 의존성 필요|

`VillageNpcRoleType`에는 SkillShop이 없습니다. 새 값이 필요하면 enum 끝에 추가하여 기존 직렬화 숫자 유지. 기존 코드 주석의 “치유소만 실제 행동”은 현 동작과 다릅니다. 구현 때 관련 주석도 현재 실제 상점/파티/펫 분기에 맞추되 이번에 C#은 수정하지 않습니다.

SkillLoadoutService는6슬롯·전투스냅샷·전투외 무료교체와 RegisterApprovedDefinition(신규 자동장착X/강화 슬롯승계)을 지원하지만 **세션 메모리만** 보관하며 Load/Reset 때 사라집니다. 결제→습득 영속성·장착 저장·중복구매 차단·직업/강화선행/판매잠금 데이터와 Save migration은 별도 선행 구현입니다. 구매 기록은 거점별이 아니라 캐릭터/직업에 연결된 기존 슬롯 Save 안에서 공유해야 하고, 결제/승인정의/소유권 갱신·저장 실패의 부분적용 방지를 함께 계약합니다. 현 일반 Shop UI를 스킬상점 완성으로 표시하지 않습니다.

마을 미술 승인만으로 스킬경제가 자동 완성되지 않습니다. 상품/가격·스킬효과·소유권저장이 미확정이면 해당 판매기능은 미완료로 명시하며, 가짜 구매창이나 클릭만으로 돈을 차감하는 임시 기능을 내놓지 않습니다. Main23 진행/거점 등록은 이 구매에 의존하지 않으므로 별도로 검증 가능합니다.

ItemEffectType에는 RecoverHp/Mp,RemovePoison/Burn/Shock/Silence/Overheat까지만 있습니다. 어묵탕/한기 해제를 이름만 다른 기존 약으로 대체할 수 없습니다. Main24의 최소 대응 수단 보장·보충/소진 규칙과 실제 해제효과·ID·아이콘·가격은 별도 계약입니다. 정화Ⅱ/온기의 불꽃 무료지급0,Chapter3 완료 전 전국 스킬판매 일괄 오픈0.

## 7. PPU·Grid·Bounds·휠체어·입력 계약

**확인값:** F1 승인256×256 타일 PPU128→2×2WU,Grid.cellSize(2,2,1),Bounds32×24WU,Camera orthographicSize5. 기존 PlayerPlaceholder는 루트 scale1/CircleCollider2D radius0.5,Variant는 자식 Visual 교체 구조입니다. 실제 런타임 최대 Collider/스케일이 항상 이 값인지는 이번에 실행 확인하지 않았습니다.

|항목|계약|
|---|---|
|마을 타일 배율|F1과 맞추는 기본 제안은256px/PPU128/셀2WU. 다른 해상도는 `pixels/PPU=동일 WU` 관계로 맞춤. 승인 PNG의 실물크기·Grounding과 함께 결정|
|31×21타일 후보|1타일=1WU로 환산 금지. 셀2라면62×42WU. 이는 공간예산 환산이며 Scene 크기를62×42로 확정/생성한 것이 아님|
|주동선·동쪽 진입로|유효 폭3WU 이상 예산. 기본 collider지름1WU보다 크지만 runtime 최대 footprint+여유 및 코너 swept 충돌을 실제 측정해 최종 판정|
|중앙/서비스 포켓|중앙7×5WU,서비스 앞3×2WU는 기획 후보예산. NPC·간판·문·처마 Collider가 들어간 뒤 실제 남는 폭으로 재계산|
|카메라|WorldBounds2D+CameraFollow 재사용. h=min(default,Height/2,Width/(2aspect)),가로반경=h×aspect를 제외해 중심 Clamp. 16:9/4:3/21:9에서 문·서비스·동쪽 끝을 이동하며 검은 외부0/길·이름표 가림 확인|
|충돌|바닥Tile collider없음,건물 발밑/벽 물리와 지붕 Sorting 분리. 실제 출구만 경계 Opening; 북문 미개통벽 유지|

NPC InteractionSystem은 가장 가까운 반경2WU(이탈3WU) 대상이며 같은 거리 tie가 안정적인 ID 순서가 아닙니다. 현재 Main22Site는 별도 Update에서 E/F/A를 받고 Main22MouseTravel은 Main22Site만 찾습니다. 따라서 **새 마을에 복사만 해서 클릭 NPC까지 지원된다고 간주하지 않습니다**.

Main23 입력 계약: 열린 Modal 우선→현재 Quest 조사/증언의 명시 타깃→가장 가까운 서비스; 한 입력을 한 번만 소비합니다. 표시 prompt와 실행 대상은 같아야 합니다. 클릭은 고른 오브젝트 ID를 유지하며 이동 도착 후 거리·현재단계·모달을 다시 검사; 다른 NPC로 자동 대체하지 않습니다. E/F/게임패드A는 같은 Interact 함수,Esc/B는 취소·선택해제·잠금해제. 수동이동은 안내이동을 취소합니다. 서비스 포켓은 가능한 상호작용 반경이 겹치지 않게 배치하고, 실제 겹침에는 명시 타깃/메뉴 선택으로 해결합니다. Main22 완료 후 F1의 기존 HUD/클릭 루프와 새Main23 루프가 동시에 움직임을 쓰지 않게 소유권을 넘기는 최소 변경이 필요합니다. 기존 Main21/22 NPC·기존 맵 입력에는 영향이 없도록 Scene/Quest 범위로 제한합니다.

## 8. 동쪽 미래 포탈 OFF 계약

약7×7WU는 동쪽 비점유 후보구역이며 **좌표 미확정**입니다. Pad3×3/Landing3×3/Queue/Clearance를 서로 겹치지 않게 예약합니다. 7×7 안에 이 예산과 주변 여유가 실제로 들어가는지 검사하며 안 들어가면 부지를 조정합니다. 포탈을 통과하지 않고 남문/북문/스킬상점에 갈 수 있어야 합니다.

현재 허용: 비충돌 바닥·비작동 기단 아트와 설계용 범위 표시. 금지: PortalAnchor/Teleport/SceneTransitionTrigger/FieldConnectionDefinition/활성SceneSpawnPoint/FastTravel 대상·버튼/퀘스트마커/VFX/NPC/동작 콜백. Landing은 **설계영역 이름일 뿐 Spawn 등록 아님**. Main23 완료·SaveLoad·서비스 rollout로 자동 활성화하지 않습니다. 나무·주민·지붕·장작·간판의 물리/렌더윤곽도 예약영역 침범0. 아르벨 기존 나무-포탈 겹침을 재현하지 않도록 별도 점검합니다.

## 9. ID 예약안·충돌 조사

정식 콘텐츠 ID는 원격 기획에서 TBD입니다. 아래는 이번 기술계약에서 중복 없이 쓸 **예약안**이며 실Asset/Scene/저장데이터에는 생성하지 않았습니다. 표시명 변경과 ID 변경은 별개입니다.

|종류|예약안|
|---|---|
|Scene|World_FrostwayVillage|
|Quest|main_23_southbound_traces|
|Spawn|Spawn_FrostwayVillage_FromNorthF1 / Spawn_NorthF1_FromFrostwayVillage / Spawn_FrostwayVillage_Checkpoint|
|SafeZone|safezone_frostway_village|
|NPC|frostway-guide / frostway-skill-shop / frostway-supply / frostway-inn / frostway-party|
|펫NPC|safezone_frostway_village-pet-adoption / safezone_frostway_village-pet-management (기존 rollout 생성규칙)|
|Navigation·목표|northf1_main23_deep / northf1_main23_traces / northf1_main23_animal_trace / northf1_main23_cart_trace / frostway_main23_arrival / frostway_main23_checkpoint|
|Shop|shop_frostway_supply|

원본 디스크+상속된Main22 기준본의 Assets/ProjectSettings 텍스트에서 위20개 예약 문자열 사용0. [IdAudit.json](Evidence/Main23VillagePreflight20261021/IdAudit.json)에 범위/전체 QuestID·중복/Scene 결과를 남겼습니다. 범용 문자열 검색이므로 미래 외부 브랜치 전부의 유일성을 보증하지 않으며 구현 시작 때 최신 대상 브랜치에서 재검사합니다. Objective ID는 Quest 내부 범위로 관리합니다. GUID는 지금 발급하지 않습니다.

원본 Assets 아래 기존 백업 폴더의 Bootstrap/World_StarterVillage Scene명 중복은 발견됐으나 새ID 충돌은 아닙니다. 백업을 삭제하거나 BuildSettings에 추가하지 않았습니다. 기존 GoldenArrow/HealerProjectile GUID 이슈는 Main22 적용준비 기록과 동일하게 별도이며 이번에 수정하지 않습니다.

## 10. 후속 구현 변경 파일 계획 — 이번 변경 아님

경로는 `Unity/Client/` 기준입니다. 신규 파일은 대응 meta를 포함하고 기존 meta/GUID를 재생성하지 않습니다. 아래는 역할별 계획이며 신규 스킬경제 범위는 승인 전 제품수량으로 확정하지 않습니다.

|대상|예정 변경·이유|
|---|---|
|Assets/_Project/Scenes/World_NorthF1.unity|북쪽 조사2·심부 target·gate·마을귀환 Spawn/필요 시 북쪽 지면·Bounds 확장. 기존Main22 좌표/아트 바이트 유지|
|Assets/_Project/Scenes/World_FrostwayVillage.unity +meta|새 안전Scene/Player/Camera/Bounds/남문/체크포인트/북문닫힘/예약부지/서비스 배치|
|Assets/_Project/Resources/QuestDefinitions/Main23_SouthboundTraces.asset +meta|5목표/prerequisiteMain22/승인보상|
|Assets/_Project/Scripts/World/Chapter3Main23Flow.cs +meta|F1·마을 설치/단계/대화/조건부gate/안전거점등록. 구체 보조클래스는 여기 역할 범위 안에서 최소화|
|Assets/_Project/Scripts/Core/Main23Progress.cs +meta, Scripts/Core/QuestService.cs|독립2흔적 플래그와Export/Import/Reset. 기존Main22 필드보존|
|Assets/_Project/Scripts/World/Chapter3Main22Flow.cs, Main22GuidanceHud.cs|완료후Main23 안내와입력 소유권 인계만. Main22목표·전투/완료Save 중복구현0|
|Assets/_Project/Scripts/NPC/InteractionSystem.cs, VillageNpcRole.cs|Main23범위 우선순위/대화·상점catalog 연결; 기존맵분기 보존. 스킬판매 역할은 실제 구현 승인 시 enum끝에 추가|
|Assets/_Project/Scripts/World/SafeZonePetServiceRollout.cs|Sites1곳/좌표 등록. 수동설치와중복0|
|Assets/_Project/Resources/ShopDefinitions/FrostwaySupply.asset +meta|승인 품목만. 어묵탕/신규효과 선행이 없으면 기존상품과 미구현을 명시|
|Assets/_Project/Art/Environment/Chapter3/FrostwayVillage/ +meta|승인VillageArt만. 기존F1/사용자PNG/ThirdParty 수정0|
|Assets/_Project/Scripts/Editor/Main23ContentBuilder.cs, Main23ContractQA.cs +meta|향후 별도격리경로 제한/자동실행0. Main22Builder는 격리경로 고정이라 여기서 재실행하지 않음|
|ProjectSettings/EditorBuildSettings.asset|새마을Scene1항목만 의미병합. 기존app-ui/Scene순서 보존|
|문서/00_프로젝트/CURRENT_STATUS.md 및 Main23 QA/시작안내|구현·검증·미검증·적용구분 갱신|

별도 의존 작업: 스킬판매Definition/거래Service/전용Presenter,SkillLoadoutService 영속 연계,GameSaveService 선택 SkillOwnership/Loadout SaveDTO·검증·복원; 승인상품/강화효과 및 Main24해제 아이템·기능. **일반상점 파일 하나로 대체할 수 없는 범위**이며 이번 Main23문서작업에 구현하지 않습니다. Main24 Quest/Scene·F2·한기는 후속 별도 작업. 기존 CameraFollow/WorldBounds/SceneTransition/Spawn/기본PlayerPrefab/전투·SaveDirectory·ProjectVersion/Packages는 수정필요가 입증되지 않으면 그대로 재사용합니다.

## 11. 후속 최소 QA·승인 게이트

1. VillageArtPilot v2 실물·Manifest·Sprite역할매핑·발기준·지붕Sorting·PPU/타일seam과16:9/4:3/21:9 비교→바울 대표 미술승인. 기존Village4의 포맷PASS를 미술승인으로 재사용하지 않음.
2. 별도 구현 지시 후 격리 Scene/5목표 구현. 신규 변경만 컴파일/ID·GUID·로드·문·Spawn/도보·휠체어/서비스취소·입력중복/카메라 검증. F1북벽 수정이 영향을 주는 구간은 표적 회귀하되 기존64/40/136 전수 반복0.
3. Save1흔적·동일흔적반복·역순조사·중간Scene진입·주민중단·등록전후·패배복귀·다른슬롯/구버전Continue/저장실패 확인. Main22완료·남문귀환·기존펫/파티 데이터 보호.
4. 동쪽예약지 component/목적지/UI/Save로드에서 작동0,북문F2 로드0,추가강제전투0/필수구매0. 아직 없는 스킬판매와한기해제를PASS로 적지 않음.
5. Main23격리 직접플레이 확인 이후 원본 적용/PR통합은 별도지시. 원본Editor/Save를 QA에 사용하지 않음.

## 12. 현재 완료·미결

완료: 8개 검토항목의 기술 경계·재사용/미구현·5목표·예약ID·변경파일계획 확정. 이번 실측은 정적조사이며 신규 Unity/Play/컴파일PASS가 아닙니다. Unity MCP 연결도구가 없어 Editor 상태 조회는 하지 않았고 포커스/선택/Play/Import/Refresh/생성0입니다.

미결: 마을 실물아트/미술승인,공식 표시명·NPC/Dialogue/TTS/Item/Skill IDs,정확 Bounds·북쪽확장길이·PPU/좌표/문폭실측,판매목록·가격·보상,스킬소유권/강화/저장거래 세부경제,Main24 무자금 한기대책/북문F2 연결. 정본6000.5.7f1 대 원본/기존QA6000.6.5f1 차이는 기존OPEN이며 새 호환성 검증을 했다고 주장하지 않습니다.

원본 보호: [ProtectionResult.json](Evidence/Main23VillagePreflight20261021/ProtectionResult.json). 초기스냅샷은 읽기조사와별도worktree 생성 후 기록했고 그 전 원본파일쓰기는 없었습니다. Assets/UserData/Packages/Settings SHA 및 rootHEAD/status 전후 비교. 이번 commit은 문서·조사증거만, main 직접commit/Push/병합/원본적용0.

# 2차 QA 후속 잔여 상태 정리 — 2026-10-06

## 검증 전 범위

기준은 로컬 QA commit `feeaa08`, 전체817체크(PASS812/FAIL0/NV3/USER2)다. 기존812 PASS는 재실행하지 않는다. 남은 NV3의 구현/검증 단계를 코드·Scene·설정·빌드 산출물로 좁혀 확인한 뒤 분류를 갱신한다. 장비 구현, 전체 Build, 물리 입력/foreground 전환, Voice219/Main04재생성5/PNG/WAV 수정은 하지 않는다.

최소 확인: 장착 상태와 Inventory·Beast를 구분해 Save 계약 조사; Dungeon Collider/Layer/Tag/충돌 callback/Scene 전환/복귀 grace 정적 확인; 기존 Standalone 산출물과 Scene List/현재 컴파일 상태 확인. 물리 접촉 미실행은 PASS로 바꾸지 않는다. 입력 연결·Focus·Navigation PASS는 이력 재사용하고 실물 Keyboard/Gamepad만 남긴다.

## 결과

총817 유지: PASS812 / FAIL0 / NOT_VERIFIED0 / DEFERRED_FEATURE1 / DEFERRED_RELEASE_VALIDATION1 / USER_INPUT_REQUIRED3 / FOREGROUND_REQUIRED0. 마지막 세 분류는 판정 근거·후속 책임을 명확하게 한 것이며 신규 PASS가 아니다. Voice LISTENING_REQUIRED219는 별도 분모, Main04 재생성5는 작업 제외 그대로다.

|기존 category/id|구현 상태·분류 A/B/C/D|최종 분류|자동 확인 범위 / 사용자 필요 / 다음 조치|
|---|---|---|---|
|Defeat/equipment|B: 일반 Equipment 장착 자체 미구현|DEFERRED_FEATURE|ItemCategory.Equipment 분류는 있으나 InventoryEntry는 ItemId/Count뿐. GameSaveData에 장비 슬롯·장착ID 없음. 장착 상태를 만들지 않았으므로 저장 QA만 빠진 경우가 아니다. 자동 장착/복원 검사 대상 없음·현재 사용자 확인 불필요. 향후 별도 장비 구현 요청 때 Save 계약 포함, 이번에 구현하지 않음.|
|Dungeon/physical-contact|A: 구현됨, 실제 물리 접촉 미실행. 남은 검증 D|USER_INPUT_REQUIRED|Collider/물리 설정/콜백/전환·복귀 유예 정적 확인. 기존 서비스→실제 Battle·승리812 PASS 재사용. 실제 이동 접촉→Battle와 도망 후 즉시 재조우 방지 확인 필요. 현재 사용자의 입력/foreground가 필요하므로 실행하지 않음.|
|Save/standalone-os-restart|C: Release/Standalone 검증. 저장소 범위 게임 Build 미생성|DEFERRED_RELEASE_VALIDATION|Editor Save/Bootstrap Continue는 구현·검증됨. 현재 게임 실행파일/게임 BuildReport가 로컬 저장소 내 없음. 외부 디스크 산출물 존재까지 단정하지 않는다. Build 뒤 실행 자체도 이번 증거 없음. 개발 단계 전체 Build 불필요, Release 후보를 만들 때 프로세스 종료→재실행→동일 슬롯 Continue 검사.|

### 좁힌 근거

- 일반 장비: `Scripts/Core/ItemDefinition.cs`의 Equipment enum은 아이템 분류다. `InventoryService.cs` Export/Import는 ItemId·Count 배열. `GameSaveService.cs`의 GameSaveData는 Inventory·PartyResources·CompanionRoster·BeastCompanions 등을 저장하지만 일반 Equipment 슬롯/장착ID는 없다. 일반 장비 관리/장착 서비스도 없다. BeastCompanion의 장착·저장은 이미 구현됐으며 이 DEFERRED_FEATURE에 포함하지 않는다.
- Dungeon B1/B2는 Builder에서 PlayerPlaceholder Prefab을 배치한 Scene이다. Player Prefab은 Layer0/Default·Untagged·Dynamic Rigidbody2D(m_BodyType0)/Simulated1·일반 Collider(isTrigger0). 몬스터는 `FieldMonsterInstaller.CreateMonster`에서 Rigidbody2D+CircleCollider2D(radius .4)+MonsterFieldController를 런타임 생성하며 Layer/Tag/Trigger를 별도 변경하지 않아 Default/Untagged/비Trigger 기본설정이다. 두 Dungeon Player Prefab 참조와 물리 속성 override를 추가 확인했다. Physics2DSettings의 LayerCollisionMatrix는 전부 ff로 Default 간 충돌 차단 없음. 이는 실제 실행 중 물리 충돌 발생 근거를 대신하지 않는다.
- `MonsterFieldController.Awake`는 중력0/회전고정/Continuous; `OnCollisionEnter2D`는 Tag 대신 충돌 Collider의 `GetComponent<PlayerController>()`로 판단하고 BeginEncounter→MonsterEncounterService.TryRaise를 호출한다. `BattleSceneFlow.Initialize`는 EncounterStarted→EnterBattle을 구독하고 Player·Spawn 정보를 기록해 Battle LoadSceneAsync로 연결한다.
- 승리/도망 공통 ReturnToField는 SuppressForSeconds(2f), 승리한 일반 Spawn만 MarkDefeated, PrepareFieldReturn 뒤 원 Scene 복귀. BattleReturnSafety는 원접촉 위치에서 1.2 떨어진 위치로 옮겨 World 위치를 저장한다. 유예 중 접촉은 TryRaise에서 거절한다. 실제 충돌 재진입/조작 감각은 검사하지 않았고 grace 동작 전체를 새 Runtime PASS로 선언하지 않는다.
- Build: 로컬 저장소 파일 조사에서 Library/PackageCache의 도구 exe는 게임 산출물에서 제외했다. 게임 exe/게임 BuildReport/Build 폴더 없음. EditorBuildSettings의 Scene19개 전부 존재·경로 중복0, Bootstrap 첫 Scene·OpeningIntro/Battle/B1/B2/Field07 포함. 현재 Unity6000.5.7f1은 컴파일 중 아님·Console Error0·Bootstrap clean EditMode·is_focused=false. 새 Build/Play Mode/Scene 변경 없음. 설정 준비 상태만 확인했으며 Build 성공 보장은 아니다.

### Input 및 Foreground

Keyboard: 기존 UI Action 연결·Focus·Navigation·방향 Event PASS를 재사용한다. 실물 키 눌림/기기·OS 포커스/Tab·Shift+Tab·Enter·Esc 조작은 USER_INPUT_REQUIRED 유지. Gamepad: 기존 UI Action 연결 PASS 재사용, 실제 연결된 기기의 D-pad/스틱·A/B·연결 상태와 사용감은 USER_INPUT_REQUIRED 유지. 가상 입력을 물리 검증으로 처리하지 않았다.

사용자 검증3항목(던전 접촉/Keyboard/Gamepad)은 별도 새 foreground 허락 뒤에만 에이전트가 직접 수행한다. [AGENTS.md §14](../../AGENTS.md)의 “사용자의 명시적 사전 허락을 받은 경우에만 foreground 검증을 실행한다”를 적용해 이번에는 실행하지 않았다. 검증할 것은 게임 창 입력 수신과 실물 기기의 조작 결과이며, background 이벤트 호출은 OS 포커스와 실제 기기 입력을 확인할 수 없다. 예상 조작은 게임 실행 화면을 활성화하고 이동키·UI 탐색/확정/취소, Gamepad 연결 후 방향/A/B, Dungeon 일반 몬스터 접촉 및 도망 후 복귀 이동이다. 사용자가 직접 수행하는 것도 가능하다. Dungeon 물리엔진 검증만은 별도 background QA를 추가할 대안이 있지만 이번 최소 정리에서 새 harness/Play 실행을 만들지 않았으며 foreground가 기술적으로 반드시 필요한 물리엔진 시험이라고 단정하지 않는다.

### 다음 개발 우선순위 — 세 단계

A. **다음 개발 진행 가능.** 이번 범위에서 알려진 미해결 구현 Blocker0, 기존812 PASS 유지. 잔여 상태는 새 결함 발견이 아닌 구현/출시/입력 단계 구분이다.

B. **반드시 먼저 고칠 확인된 Blocker0.** 일반 장비는 별도 DEFERRED_FEATURE이며 구현하지 않은 기능을 기존 검증 실패로 취급하지 않는다. 향후 사용자 요구가 장비를 전제로 하면 먼저 장비 구현 범위를 확정해야 한다. 새 기능 설계는 여기서 하지 않는다.

C. **Release 직전으로 유예 가능:** Standalone Build→실제 실행→프로세스 종료→Continue·저장 위치/권한·슬롯 보존, 실물 Keyboard/Gamepad와 Dungeon 접촉/복귀 grace 검증. Release 확정 전에 필수로 완료한다. 별도Voice 청취219/Main045 상태도 그대로 후속 관리하며 이번 개발 진행 판단을 음성 품질 Release 승인으로 확대하지 않는다.

### 변경·재개

문서/CSV/Runtime JSON 분류만 변경. 게임C#/EditorQA/Scene/Asset/사용자Save/Settings/PNG/WAV 변경0, PASS812 재실행0·전체Build0·포커스전환0·Push0. 다음 세션 정확한 시작점은 이 목록의 USER3 또는 Release 준비/별도 Equipment 요청이다. 기존 QA launcher는 과거 실행용이며 전체 Launch를 상태 정리 목적으로 다시 실행하지 않는다. 선행 관련 commit `feeaa08`; 이번 문서commit은 최종보고 참조.

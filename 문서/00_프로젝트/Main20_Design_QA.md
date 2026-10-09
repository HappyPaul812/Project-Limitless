# Main20 정식 설계 QA — 문서 / 정적 구현 가능성

2026-10-09. 시작 HEAD `6a4ccc372915fa9af26689ddf25e714f8b7f866f`. LOCAL과 저장된 origin/main은 동일（ahead0/behind0）이며 사용자 제공 Push 기준6a4ccc3과 일치한다. fetch/원격 조회/Push0. 기존 tracked 변경75·미추적25를 보존한다.

필수 프로젝트/Story/Main18·19/몬스터/전투/스킬/Balance/Art/TTS/BGM 문서를 확인한 뒤 실제 LOCAL 코드와 데이터 구조를 읽었다. 문서화→정적 검증 순서이며 Unity6000.5.7f1의 Scene/Play/Console 상태를 변경하거나 실행하지 않았다. 아래 SUPPORTED는 기존 기반 API를 뜻하며 Main20이 이미 구현됐다는 뜻이 아니다.

## 정적 판정

전체: **NEEDS_IMPLEMENTATION_EXTENSION**. 공용 직접 피해·Burn·Overheat·Taunt·보상·Story 복귀 기반은 재사용할 수 있다. Main20 Phase/행동/HUD/Scene/Quest/Encounter/BGM 매핑은 별도 구현이 필요하다. 기존 시스템 전체 재설계는 필요하지 않다.

소스 경로는 `Unity/Client/Assets/_Project/Scripts/` 기준이다.

| 항목 | 판정 | LOCAL 근거와 후속 작업 |
| --- | --- | --- |
| 2 Phase | NEEDS_IMPLEMENTATION_EXTENSION | Battle/BattleDungeonMonsterRuntime에 파수꾼 phaseTwo 선례. Main20 상태는 없음. BattleChapter2MonsterRuntime의 종별 행동에 Main20 전용 상태/분기를 작은 범위로 추가하거나 별도 Boss Runtime으로 분리 |
| HP50% 첫 전환1회 | NEEDS_IMPLEMENTATION_EXTENSION | 기존 ShouldEnterPhaseTwo는60%·Boss 자기 행동 시 검사. Main20은 피해 직후 HP 경계 관측+한 번 flag가 필요; 1200→600 경계, 다중 타격·DoT·KO 경계 검사 |
| HP 회복 없는 전환 | SUPPORTED_WITH_SMALL_EXTENSION | Combatant HP를 유지하고 flag/표현만 변경 가능. TurnOrderQueue.InitializeBattle/Build 재호출·Party/상태 Clear를 전환에서 호출하지 않아야 함 |
| deterministic action cycle | SUPPORTED_WITH_SMALL_EXTENSION | BattleChapter2MonsterRuntime.BeginActorAction/TakeAction 개체별 자기 행동 카운터 선례. Main20 4/5 행동 순환·Phase간 cursor/예고 이어가기 추가 |
| Telegraph | NEEDS_IMPLEMENTATION_EXTENSION | Chapter2 Prepared 예고 및 Controller의 bossTelegraphText 선례. 현재 HUD는 파수꾼 충격/장막 전용. Main20 Core Wave/Core Eruption 다음 행동·전체과열+1 텍스트와 flag 추가, 짧은 messageText만으로 대체 불가 |
| AoE 직접 피해 | SUPPORTED_WITH_SMALL_EXTENSION | BattleSceneController.PlayChapter2Scatter / PlayMain18HeatAction: allies.LivingMembers 스냅샷·pathTraits.ApplyDirectDamage(areaAttack:true).80/60/55% 및 생존 후 과열을 Main20 분기에서 매핑 |
| Overheat+1 | SUPPORTED | Battle/BattleSkillSystem.cs의 BattleStatusEffectRuntime.ApplyOverheat/GetOverheatStacks/ClearOverheat. 3도달 ceil8%·방어 없음·KO정리. 직접 피해 생존 확인 뒤 호출 |
| Burn | SUPPORTED | 동일 파일 ApplyOrRefreshBurn, Controller.PlayBasicAttack(inflictBurn:true). Attack30% 저장·2회·비중첩·생존 확인·기존 해제/야수 첫 Burn 방어 |
| 최고 Overheat Target | SUPPORTED_WITH_SMALL_EXTENSION | PlayMain18HeatAction: Magic 공용 대상→최고Stack 필터→ChooseEnemyTarget. Main2090% 행동으로 연결 필요 |
| Taunt 우선 | SUPPORTED | Battle/BattleCore.cs TargetResolver.ResolveHostileTargets는 단일ForcedTarget 우선. 광역LivingMembers는 강제대상 경로를 사용하지 않음 |
| IsBoss Flee 차단 | SUPPORTED_WITH_SMALL_EXTENSION | Controller.Flee는 enemies.Any(IsBoss) 차단. IsBoss는 MonsterDefinition 필드가 아니라 BattleParticipantSetup→Combatant 값. Factory에서 새 Boss 참가자에true를 명시해야 함 |
| Boss defeat objective | SUPPORTED_WITH_SMALL_EXTENSION | Core/QuestService.NotifyEncounterWon 및 Controller.EndBattle의 StableEncounterId 통보·battleEnded 중복 가드. 새 Quest9/Story Encounter/반환 데이터 등록 필요 |
| Retry | SUPPORTED_WITH_SMALL_EXTENSION | World/Chapter2Main19Flow의 현재DefeatEncounter에서 EnterStoryBattle 재호출·BattleSceneFlow.ReturnToField는 Story Spawn 제거 안 함. Main20 실패 목표6 유지·재도전 Site 등록 필요 |
| Save/Continue | SUPPORTED_WITH_SMALL_EXTENSION | Core/GameSaveService Version1, QuestProgress 완료/진행·Party/Inventory/좌표 저장. Battle Scene 저장 불가. Field11 등록 및 목표별 표현 복원 필요. 전투 중 Phase/상태 저장·중간 Boss전 이어하기를 지원한다고 주장하지 않음 |
| BGM Encounter override | SUPPORTED_WITH_SMALL_EXTENSION | Audio/BgmSceneCatalog.FindBattle에 파수꾼 전용 ID 선례, 다른Boss는null. Main20 ID→Crowns를isBoss null보다 먼저 추가·Field11→Paths 등록 필요 |
| Main19→Main20 Scene | NEEDS_IMPLEMENTATION_EXTENSION | Field10 서쪽 폐쇄·Chapter2Main19Flow.Site(12) 안내 고정, Field11/Connection 없음. Main19 완료 Gate·왕복Exit·Spawn·Bounds/Build등록 필요 |
| Boss Sprite4×4 | SUPPORTED | Monster/MonsterSpriteSheetAnimation ExplicitIdle/Attack/Hit/Defeat, Editor/Main18ContentBuilder314px slicing 선례. Skill/Hit는3행 공유·KO마지막 유지 |
| Phase2 Sprite / VFX7 | SUPPORTED_WITH_SMALL_EXTENSION | 현 Configure는Definition 배열을 고정 로드하며 Phase별 교체 API 없음. 작은 Phase표현 매핑·7VFX Presenter 연결 필요; 공용 Monster Runtime 재설계 불필요 |
| Boss/Quest 보상 분리 | SUPPORTED_WITH_SMALL_EXTENSION | MonsterDefinition.BaseExperience/CurrencyReward/LootEntries와 Quest RewardBundle 분리. Attack30은 기준10×300%, Lv15/HP1200/Agility10/EXP140/Talent40·Loot 빈 배열 등록 필요 |

## 구현 전 미결·충돌 경계

- 기존 파수꾼60% 전환은 다음Boss 행동을 소환에 소비하고 장막/소환을 사용한다. Main20의 즉시50%·회복/소환/상태초기화 없음과 다르므로 기존 임계값만50으로 바꾸거나 파수꾼 Runtime을 공용 변경하지 않는다. Controller는IsBoss 모두를dungeonMonsters.SetBoss에 넣지만 행동은monster_silent_warden ID에서만 분기한다. Main20 연결 시 파수꾼 상태/HUD가 섞이지 않게 확인한다.
- Phase 경계의 **미실행 예고 보존 방식 / Phase2 첫 행동 cursor**는 사용자 정본에 구체 규칙이 없다. 예고 취소·파동을 분출로 교체·예고 없는 광역·Phase전환 턴 추가·Queue 리셋을 임의 확정하지 않는다. 구현 전 이 경우의 명시 계약을 정하고 검증한다. 이번 설계의 고정 패턴과 다음행동 예고 의미는 유지한다.
- 용융 강타의 근접/마법/원거리 사거리 분류는 미확정. 단일 Taunt 우선과 기존 TargetResolver는 유지하며 임의 후열 관통·공격속성·방어무시를 추가하지 않는다.
- 전투 EXP140은 기존 BaseExperience140으로 대응되나 현재 실제 EXP에는 레벨 차이 배율이 적용된다. 모든 레벨에서 최종140 고정 지급이 의도라면 기존 시스템과 **DESIGN_CONFLICT** 경계가 생긴다. 이번에는 공용 보상을 변경하거나 고정지급을 구현하지 않고 기본/최종 지급을 구분한다. Quest EXP100/Talent80은별도이며 중복가산 금지.
- Field11 Geometry/실제 Boss 크기/Phase2 Sheet 또는overlay 최종 선택은 Art·구현 전 기술 검토에서 확정한다. 사용자가 지정한 수치를 난이도 체감 추측으로 조정하지 않는다.

## 문서 작성 후 검증 결과

- Quest명/ID/Scene/선행·Boss명/ID/수치·보상 분리·Phase1 4행동/Phase2 5행동·50%1회·Overheat/Burn 계약을 요청과 대조했다. Objective9개의 고유ID/허용type/target, Story 전후 본문을 확인했다.
- TTS Manifest8 고유ID: 세린5 TTS_PENDING / Player2+지문1 NOT_EXPECTED. Story표와 본문·줄바꿈·ID 일치, 세린companion_serin/Schedar 유지. 생성/API/Unity연결0.
- Art는Field6종（Rift2파일）·기본Boss4×4/16프레임·Phase2 대응안·VFX7·KO계약을 명시했다. Main19 실루엣 원본 확대/복사 금지. 납품/미술승인/실제화면QA는미완료.
- Crowns 실재·4,273,649bytes·SHA256·177.815458초·MP3/44100/stereo/192kbps·전체decode exit0, Unity동일byte0 확인. [감사 및 적용 계약](Main20_Audio_Handoff.md). 음악 청취는USER_LISTENING_REQUIRED.
- Main18 원본15/checkpoint/이전 보고서/Manifest는 이전 문서화 작업의 보호 SHA256과 일치한다. 프로젝트 전용 Asset/코드·Packages/ProjectSettings·Main18 출력·기존 Main18/19 Manifest·TTS 정책·Crowns 원본2556파일은 문서 작성 후 commit 전후의 경로/SHA256 집계로 추가 보호 검증한다. 이번 승인 변경은 아래 문서9개뿐이다. Unity 실행/컴파일/Play QA는 수행0이며 구현 PASS로 승격하지 않는다.
- Main18 성공15·잔여7·Unity 적용0 / Main19 TTS_PENDING13·생성0을 CURRENT_STATUS에 보존한다. 게임 코드0·Unity Asset 생성/변경0·Audio Import0·MP3 복사0·TTS API0·PNG/WAV 생성0·SaveVersion 변경0·Secret 기록0. Markdown 링크·CSV 정합·git diff --check로 문서 검증한다.

## 변경 문서와 다음 단계

신규: Chapter2_Main20_심부의_거신.md / Main20_Art_Handoff.md / Main20_Audio_Handoff.md / Main20_TTS_Manifest.csv / Main20_Design_QA.md.

갱신: Chapter2_서부_방향.md / Chapter2_BGM_정본.md / CURRENT_STATUS.md / Chapter2_몬스터_1차_설계.md（정본 링크만 추가）. 과거 TBD는 이력으로 보존하고 최신 링크를 상단에 명시한다.

다음은 Art Handoff를 아트 담당 세션에 전달해 Boss/Field11/VFX를 제작하는 것이다. 그 후 Phase 미결 계약·사거리/보상 경계 확인→별도 승인된 Main20 구현→백그라운드 Compile/9목표/보스 패턴/50%/예고/방어/과열/Burn/Retry/보상/Continue/Bounds/BGM QA→사용자 미술·음악·난이도 검토 순서다. TTS는 기존 quota 정책과 Main18→19 제작 우선순위를 보호한다. GitHub Push0.

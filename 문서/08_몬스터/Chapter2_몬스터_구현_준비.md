# Chapter 2 몬스터 구현 준비

> 2026-09-29 **LOCAL 코드·Asset 정적 조사 결과**. 이 문서는 구현 전 파이프라인 조사 기록이다. 이후 사용자 제공 5종 PNG와 1차 수치·행동 계약이 도착했으나, **PNG 5개 모두 1254×1254로 동일 크기 4×4 분할 조건을 충족하지 않아** Import/Runtime은 진행하지 않았다. 최신 기획·차단 사유는 [Chapter 2 몬스터 1차 설계](Chapter2_몬스터_1차_설계.md)를 따른다. 필수 항목은 [몬스터 설계 규칙](몬스터_설계_규칙과_템플릿.md), 전투·분양 정책은 [전투시스템](../10_전투/전투시스템.md)과 [BeastCompanion 정본](../10_전투/사수_BeastCompanion_확장_설계.md)을 따른다.

## 현재 구현 경계

아래 경로는 모두 `Unity/Client/Assets/_Project/` 기준이다. `Resources/MonsterDefinitions/`의 정의는 `Scripts/Monster/MonsterDefinition.cs`가 읽는다. `MonsterId`는 승리·펫 종 기록 키이며 Level/HP/Base EXP/Talent/전리품/지원 몬스터/필드 이동·외형/전투 공격 배율·민첩/프레임 자료와 일부 기본 공격 독·광역 피해 설정을 담는다. **일반 몬스터 Skill 목록 또는 범용 AI 정책 필드는 없다.** 전투 Attack은 `Scripts/Battle/BattlePrototypeEncounter.cs`의 Slime 기준 10 × `BattleAttackPercent`를 올림해 만든다. `BattleAgility`는 `UseDefinitionBattleAgility`가 참일 때 쓰고, 아니면 편성 호출값을 쓴다. Formation과 `IsBoss`는 Definition이 아니라 해당 전투의 참가자 구성에서 지정한다. `CreateMonster`가 만든 모든 적의 현재 기본 `TargetRangeType`은 **MeleePhysical**이다. 후열에 놓였다는 사실만으로 원거리 공격이 되지 않는다.

| 종 / `Resources/MonsterDefinitions` | Stable ID | Lv / HP / Attack / 민첩 / EXP / Talent | 현재 편성·행동·외형 경로 |
|---|---|---|---|
| 초원 슬라임 `01_GrassSlime.asset` | `grass_slime` | 1 / 60 / 10 / 편성값 / 8 / 3 | 일반 전열 지원·후열 주몹 가능; 기본 단일 공격. `Art/Monsters/01_GrassSlime`, `Animations/Monsters/01_GrassSlime`의 Field Animator와 Definition의 프레임 자료 |
| 독침벌 `02_VenomBee.asset` | `venom_bee` | 2 / 70 / 12 / 편성값 / 12 / 4 | 일반 후열 주몹, Story 편성도 후열; 기본 공격 시 일반 독·부여 대기. Definition의 시트 프레임 경로 |
| 숲거미 `03_ForestSpider.asset` | `forest_spider` | 3 / 80 / 14 / 편성값 / 16 / 5 | 일반 후열 주몹, Story 편성은 전열도 사용; `HasDirectAreaAttack`이면 독액 분사 20 우선. `Resources/MonsterAnimations/Spider` 프레임 경로 |
| 맹독뱀 `04_VenomSnake.asset` | `venom_snake` | 4 / 95 / 16 / 편성값 / 20 / 6 | 일반 후열 주몹, 혼합·Story 편성은 전열/후열; 기본 공격 맹독·부여 대기. `Resources/MonsterAnimations/Snake/snake_move_sheet.png` |
| 이끼갑충 `05_MossBeetle.asset` | `monster_moss_beetle` | 5 / 115 / 17 / 7 / 24 / 7 | Field03 전열, 기본 단일 공격. `Art/Monsters/05_MossBeetle`의 명시적 Idle/Walk/Attack/Hit/Defeat 프레임 |
| 그늘박쥐 `06_ShadeBat.asset` | `monster_shade_bat` | 6 / 90 / 19 / 15 / 28 / 8 | Field03·Dungeon 후열, 기본 단일 공격. `Art/Monsters/06_ShadeBat`의 명시적 5종 프레임 |
| 묘지 망자 `07_GraveWight.asset` | `monster_grave_wight` | 7 / 125 / 20 / 8 / 32 / 9 | Dungeon 전열, 기본 단일 공격. `Art/Monsters/07_GraveWight`의 명시적 5종 프레임 |
| 침묵의 잔영 `08_SilentEcho.asset` | `monster_silent_echo` | 8 / 90 / 18 / 14 / 34 / 10 | Dungeon B2 후열, 침묵의 속삭임 우선(개체별 대기), 이후 기본 공격. Definition 명시적 프레임 |
| 봉인 수호체 `09_SealGuardian.asset` | `monster_seal_guardian` | 9 / 165 / 19 / 7 / 40 / 12 | Dungeon B2 전열·보스 소환, 동료 보호 장막 우선, 대상 없으면 기본 공격. Definition 명시적 프레임 |
| 침묵의 파수꾼 `10_SilentWarden.asset` | `monster_silent_warden` | 10 / 620 / 24 / 6 / 120 / 35 | Dungeon B2 전열 Boss; 표식·예고→다음 자기 행동 충격·2단계 소환. Definition 명시적 프레임 |

Attack 표의 수치는 현 `BattlePrototypeEncounterFactory`의 기준 공격 10과 Definition 배율로 계산한 현재 전투 참가자 값이다. 첫 네 종은 `UseDefinitionBattleAgility`가 꺼져 있어 편성별 민첩이 달라질 수 있다. 현재 Field 스폰은 `Resources/MonsterSpawns/*.asset`의 `FieldMonsterSpawnDefinition.cs`가 SceneName·SpawnId·종·위치·활동 반경·리스폰을 지정하고, `FieldMonsterInstaller.cs`가 Scene 로드 시 생성한다. Dungeon 전용 배치와 비리스폰 Boss도 같은 타입을 쓴다. 조우 구성은 `BattlePrototypeEncounter.cs`가 일반 3인전·Field03 특례·Dungeon B1/B2·Story별로 결정한다. 일반전의 지원 몬스터는 Definition의 `EncounterSupportMonster`에서 얻지만 전열/후열·개체 수는 편성 코드에서 정한다. 실제 Victory에서는 `BattleSceneController.cs`가 보상·Save와 `BeastCompanionService.RecordVictory`를 처리한다. Flee/Defeat는 승리 종을 기록하지 않는다.

현재 Spawn Asset 연결은 Slime `Field01_GrassSlime_01~05`, 독침벌 `Field01_VenomBee_01~03`, 숲거미 `Field02_ForestSpider_01~04`·`Field03_EntranceSpider`, 맹독뱀 `Field02_VenomSnake_01~02`·`Field03_EntranceSnake`, 이끼갑충 `Field03_EntranceBeetle`·`Field03_MiddleBeetle`, 그늘박쥐 `Field03_MiddleBat`·`Field03_DeepBat`·`Dungeon01_B2_02`, 묘지 망자 `Dungeon01_B1_01~04`·`Dungeon01_B2_01`, 봉인 수호체 `Dungeon01_B2_03~04`, 침묵의 파수꾼 `Dungeon01_B2_Boss`다. 침묵의 잔영은 별도 Field Spawn Asset 없이 B2 Encounter에 포함된다. Dungeon B1의 그늘박쥐와 보스의 소환 수호체도 Encounter 생성 경로에서 추가된다.

## Sprite·Animation 파이프라인

- 최종 원본 PNG는 예처럼 `Art/Monsters/<종>/`에 보존한다. 기존 Spider/Snake는 `Resources/MonsterAnimations/<종>/`의 개별 프레임·시트도 사용한다. **단일 강제 Import 규격은 없다.** Slime Field 시트는 Multiple/314 PPU/Point/Alpha on, Snake 시트는 Single/32 PPU/Point/Alpha on, Spider 개별 프레임은 Single/256 PPU/Point/Alpha on이다. 각 원본의 실제 크기·Alpha·프레임 경계를 확인하고 해당 용도·기존 유사종과 비교해 Import Mode, PPU, Filter, Compression, Pivot을 확정한다. 원본 이미지는 수정하지 않는다.
- `MonsterDefinition.cs`는 시트+셀 크기/열·프레임 수, `Resources` 프레임 경로, **명시적 Sprite 배열**을 지원한다. `MonsterSpriteSheetAnimation.cs`는 Field `SpriteRenderer`와 Battle `Image`에서 같은 Definition의 프레임을 재생한다. 명시적 프레임이 있으면 이를 우선 사용한다. 실사용 동작은 **Idle, Walk, Attack, Hit, Defeat**, Spider처럼 원거리 연출이 있으면 **Shoot**이다. Defeat는 한 번 재생 후 마지막 프레임 유지. 원본에 없는 프레임을 임의 생성하지 않는다.
- Slime Field는 별도 Animator Controller·방향별 Idle/Walk 클립 경로도 있다. 반면 최근 Moss Beetle/Shade Bat/Grave Wight와 B2 종은 Definition의 명시적 프레임 경로가 중심이며 `fieldAnimatorController`가 비어 있다. 새 종 모두에 Animator·클립을 강제할 이유는 없다. 최종 시트 구조에 맞춰 기존 명시적 프레임 경로를 먼저 검토하고, 필요한 경우에만 Animator를 둔다.
- 실제 연결 뒤 5종 프레임 순서·Pivot 흔들림·Field 이동과 Battle Attack/Hit/Defeat, Missing Sprite/Meta/GUID, 원본 해시, 각 해상도에서의 표시를 검증한다. `BeastCompanionCatalog.GetMonsterFrames`는 Walk → Resource Walk → Idle → 시트 → Field Sprite 순으로 습격 프레임을 찾는다.

## 행동·상태·Targeting 재사용 지점

- `BattleSceneController.EnemyAction`은 B2 종별 분기 → Definition 광역 직접 공격 → 기본 단일 공격 순서다. `BattleMonsterAbilityRuntime.cs`는 독 부여 대기, `BattleDungeonMonsterRuntime.cs`는 침묵의 잔영 대기·수호체 장막·파수꾼 예고/단계를 **전투 중에만** 보관한다. 일반 단일·광역·예고는 기존 실행 예시가 있지만, **일반 몬스터용 범용 행동 목록, Burn 부여 AI, 자기 방어·다음 행동 강화 정책은 없다.** 따라서 Chapter 2 스킬을 Definition만 채워서 실행할 수는 없다.
- `BattleCore.cs`의 `TargetResolver`는 MeleePhysical(동열 전열 생존 시 후열 보호), RangedPhysical(살아 있는 후열 우선), Magic(전후열 자유)과 도발 강제 대상을 처리한다. 현재 모든 몬스터 기본 공격은 MeleePhysical로 만들어진다. 그을음들개·균열도마뱀·화열딱정벌레는 전열 근접 성격, 열풍매는 후열 원거리 성격, 불씨망령은 후열 마법·광역 성격이라는 **설계 예상**만 있다. 실제 원거리/마법 행동과 도발 처리는 구현 때 기존 타깃 API를 명시해 연결해야 한다.
- 공용 Burn은 `Scripts/Battle/BattleSkillSystem.cs`의 `BattleStatusEffectRuntime.ApplyOrRefreshBurn(target, rawDamagePerTick, ticks)`가 대상별 `RemainingTicks`와 `RawDamagePerTick`을 저장한다. 재적용은 중첩 없이 둘 다 교체한다. 파이어 볼은 **명중 시 공격력의 30% 원시 피해, 2틱**을 전달하는 현재 예시일 뿐 신규 몬스터/펫의 확정 수치가 아니다. 대상 행동 종료의 `ApplyBurnTickAtActionEnd`는 `DamageOverTime` 경로로 피해를 주고 횟수를 줄인다. `RemoveHarmfulStatus(Burn)`과 `RemoveAllHarmfulStatuses`는 각각 화상 연고(`BattleItemUseService.cs`)·정화에서 사용한다. 죽은 참가자의 상태는 `RemoveInvalidCombatants`에서 정리하고 전투 종료에는 `BattleSceneController`의 전투별 Runtime이 폐기된다. 신규 Burn 종류를 만들지 않으며 Chapter 2의 피해·틱 수는 기획 확정 전 `TBD`다.
- 파수꾼의 **예고→자신의 다음 행동에 광역 충격**은 `BattleDungeonMonsterRuntime.PrepareShock/CompleteShock`와 `BattleSceneController.TryPlayDungeonMonsterAction/PlayWardenShock`에 실례가 있다. 다만 보스 전용 ID 분기다. 균열도마뱀의 지면 울림·불씨망령의 응축은 유사한 전투 중 상태와 다음 행동 소비 패턴을 참고할 수 있으나 현 일반 AI로 자동 실행되지는 않는다. 자기 방어도 수호체의 **타인 보호 장막**과 다르므로 그대로 동일시하지 않는다.

## 몬스터 펫 연결

`Scripts/Core/BeastCompanionService.cs`의 `MonsterIds`는 현재 Chapter 1 다섯 종만 열거한다. `RecordVictory`는 실제 종 ID를 기록하지만 **목록 노출·구매·장착 가능 여부는 이 등록 목록에 달려 있다.** `GetAdoptionCandidates` → `TryAdopt`(100 Talent) → Save → 사수별 `TryEquip` 흐름이다. `Scripts/Battle/BeastCompanionSystem.cs`의 `BeastCompanionCatalog.ResolveMonster`는 등록 ID와 `Resources/MonsterDefinitions`를 통해 Monster 기반 Definition을 만든다. `BattleActionPresenter.cs`는 해당 종 프레임을 재생한다. 전투당 첫 독·침묵 방어의 소비 상태는 `BattleStatusEffectRuntime`의 `spentPoisonGuards`·`spentSilenceGuards`에 Combatant별로 있다. 직접 피해 완화는 `ApplyIncomingDamage`에서 **DirectCombatAction**일 때만 적용한다. `Scripts/Battle/BattleSkillSystem.cs`의 `ExecuteBeastCompanionAssault`에는 독침벌의 생존 대상 일반 독 추가 예시가 있다.

Chapter 2 펫 구현 때는 네 종의 MonsterId 등록, 분양 UI 설명·효과 표시, MonsterDefinition과 공식 Walk/Idle 프레임, 습격 효과·패시브, 캐릭터별 장착 Save/Continue·구버전 fallback·Victory/Flee/Defeat·구매 실패 회귀를 함께 점검한다. 불씨망령은 등록하지 않는다.

| 종 | 실제 구현 체크리스트와 현재 제한 |
|---|---|
| **그을음들개** | 최종 시트의 Idle/Walk/Attack/Hit/Defeat → Definition의 ID·성장/보상·전열 편성·불씨 물기/사나운 돌진 AI·공용 Burn 연결. 펫은 습격 **180%/CD3 유지**, 적중 후 생존 대상에 기존 Burn 적용. 현재 습격 독침벌 분기와 Burn API를 조합하되 Burn 수치·틱 수 `TBD`. |
| **열풍매** | 공식 프레임(원거리 Shoot 필요 여부 원본 확인) → Definition·후열 편성·불꽃 깃털/잿바람 급습 타깃·Burn. **기존 원거리 직접 피해 -5% Passive 안은 사용자 최신 계약으로 폐기**했다. 새 펫 효과는 습격 시 대상의 `FormationRow.Rear`를 판정해 최종 직접 피해 ×1.10이며 원거리 피해 분류 확장은 필요 없다. |
| **균열도마뱀** | 최종 5종 프레임 → Definition·전열 편성·지열 긁기/균열 돌진/지면 울림 AI. 펫 첫 직접 피해 -20%는 기존 DirectCombatAction 분기와 `spentPoisonGuards` 같은 **Combatant별 전투 중 1회 소비 Set** 패턴을 재사용 가능. DoT는 origin으로 제외. 소비 시점·0 피해·방어와 계산 순서 등은 구현 전 검증. 지면 울림 후 강화는 보스 예고 패턴을 참고하되 일반 AI 연결은 새로 필요. |
| **화열딱정벌레** | 최종 5종 프레임 → Definition·전열 편성·발화 갑각/뜨거운 분비액/갑각 수축 AI·Burn. 펫 첫 유효 Burn 무효는 `ApplyOrRefreshPoison`의 유효성 검사 후 소비와 Silence 소비 Set 패턴을 참고할 수 있다. **현재 `ApplyOrRefreshBurn`은 void이며 저항 분기가 없다.** 유효 Burn 검사 뒤 1회 소비 경계를 연결하고 적용 결과 UI를 검증해야 한다. |
| **불씨망령** | 최종 5종 프레임 → Definition·후열 편성·잿불 비산 광역 Burn/불씨 응축/열핵 분출 AI. 광역 직접 피해와 Burn 부여는 현재 서로 다른 경로라 조합·타깃별 적용 검증 필요. 펫 목록·Definition·패시브는 **없음**. |

## 최종 Asset·수치 확정 후 권장 구현 순서

1. 사용자 확정 PNG와 프레임 경계·Alpha·방향·해시 확인 → 기존 유사종 기준 Import·Slice·프레임 연결. Placeholder로 대체하지 않는다.
2. 종별 Stable ID·Level/HP/Attack 배율/민첩/EXP/Talent/전리품·지원 몬스터를 확정한 뒤 MonsterDefinition 생성. Field 위치와 Encounter 진형·보스 여부는 별도 결정한다.
3. 기본·원거리·광역·Burn·예고/강화/방어 행동의 타깃과 AI 실행 경계를 종별로 연결하고 공용 상태·DoT·정화 회귀.
4. 펫 가능 네 종의 승리 목록·구매·장착·습격/패시브·공식 프레임 연결. 열풍매 피해 분류가 해결되지 않으면 해당 패시브는 보류한다.
5. 확정된 Chapter 2 Field/안전지대/World Bounds와 SpawnDefinition·조우 편성을 연결한다. 실제 Scene/Quest 제작 승인 범위와 맞춘다.
6. Edit Mode 참조·컴파일·Console 확인 후 격리 Play Mode에서 Field 이동, 전열/후열·도발, 전투 승패/도주, Burn/저항·전투 초기화, 보상·저장·펫 경제를 검증한다.

**남은 입력:** 4×4 동일 크기로 분할 가능한 5종 공식 수정 시트와 실제 프레임 경계, 종별 Import 규격, Encounter/Spawn과 Chapter 2 Field 위치. Level·HP·Attack·민첩·EXP·Talent와 1차 스킬 배율·Burn 규칙은 최신 [설계 문서](Chapter2_몬스터_1차_설계.md)에 확정됐으며 Loot Item은 이번 구현에서 추가하지 않는다. Overheat는 별도 설계 후보로 남는다.

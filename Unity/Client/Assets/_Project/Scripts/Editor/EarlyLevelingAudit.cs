using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using ProjectLimitless.Battle;
using ProjectLimitless.Core;
using ProjectLimitless.Monster;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjectLimitless.EditorTools
{
    /// <summary>격리 사본의 테스트 슬롯으로 성장·공용 독·실제 Field/Battle과 저장 연결을 검증합니다.</summary>
    [InitializeOnLoad]
    public static class EarlyLevelingAudit
    {
        private const string Key = "ProjectLimitless.EarlyLevelingAudit";
        private static int stage;
        private static double readyAt;
        private static FieldMonsterSpawnDefinition snakeSpawn;
        static EarlyLevelingAudit() { EditorApplication.playModeStateChanged += OnPlay; }

        public static void RunBatch()
        {
            if (!Application.isBatchMode) throw new InvalidOperationException("배치 검증 전용입니다.");
            try
            {
                Check(GameSaveService.CurrentSlotIndex == 0, "사용자 슬롯 미선택");
                Check(Application.dataPath.Replace('\\', '/').EndsWith("/Project-Limitless/Temp/Lv/Assets"), "격리 프로젝트에서만 저장 검증");
                AuditRules();
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
                SessionState.SetBool(Key, true);
                EditorApplication.EnterPlaymode();
            }
            catch (Exception e) { Fail(e); }
        }

        private static void OnPlay(PlayModeStateChange state)
        {
            if (!SessionState.GetBool(Key, false) || state != PlayModeStateChange.EnteredPlayMode) return;
            try
            {
                AuditRules();
                PrepareTestSaves();
                GameSaveService.RestoreSession(3, GameSaveService.InspectSlot(3).Data);
                GameSessionData.SelectJob("mage");
                GameSessionData.ConfigureProgress(3, 160);
                stage = 1;
                readyAt = EditorApplication.timeSinceStartup + 3;
                EditorApplication.update += Tick;
                SceneManager.LoadSceneAsync("Field_02");
            }
            catch (Exception e) { Fail(e); }
        }

        private static void Tick()
        {
            if (EditorApplication.timeSinceStartup < readyAt) return;
            try
            {
                if (stage == 1)
                {
                    Check(SceneManager.GetActiveScene().name == "Field_02", "Field_02 로드");
                    var monsters = UnityEngine.Object.FindObjectsByType<MonsterFieldController>();
                    Check(monsters.Length == 6, "거미 4 + 뱀 2 실제 생성");
                    var snakes = monsters.Where(m => m.SpawnDefinition.Monster.MonsterId == "venom_snake").ToArray();
                    Check(snakes.Length == 2, "뱀 2개");
                    snakeSpawn = snakes[0].SpawnDefinition;
                    foreach (var snake in snakes)
                    {
                        var animation = snake.GetComponentInChildren<MonsterSpriteSheetAnimation>();
                        Check(animation != null && snake.GetComponentInChildren<SpriteRenderer>().sprite != null, "뱀 실제 Animation/Sprite");
                        foreach (var zone in Resources.LoadAll<FieldEntranceSafetyZoneDefinition>("FieldEntranceSafetyZones").Where(z => z.SceneName == "Field_02"))
                            Check(Vector2.Distance(snake.transform.position, zone.Center) > zone.Radius, "뱀 안전지대 밖");
                    }
                    BattleEncounterContext.Set(snakeSpawn.Monster, snakeSpawn, null, null,
                        new Vector2(0, 0), snakeSpawn.Position);
                    SceneManager.LoadSceneAsync("Battle");
                    stage = 2;
                    readyAt = EditorApplication.timeSinceStartup + 4;
                }
                else if (stage == 2)
                {
                    var controller = UnityEngine.Object.FindAnyObjectByType<BattleSceneController>();
                    Check(controller != null, "실제 Battle 생성");
                    var snakeAnimation = UnityEngine.Object.FindObjectsByType<MonsterSpriteSheetAnimation>()
                        .Single(a => ((MonsterDefinition)Field(a, "definition")).MonsterId == "venom_snake");
                    Check(((Sprite[])Field(snakeAnimation, "idleFrames")).Length == 4 &&
                        ((Sprite[])Field(snakeAnimation, "attackFrames")).Length == 4 && snakeAnimation.transform.localScale.x < 0,
                        "맹독뱀 Idle/Attack 4프레임과 우향 반전");
                    snakeAnimation.PlayAttack();
                    Check((bool)Field(snakeAnimation, "attacking"), "맹독뱀 공격 전환");
                    snakeAnimation.StopAttackAndReturnToIdle();
                    Check(!(bool)Field(snakeAnimation, "attacking"), "맹독뱀 Idle 복귀");
                    var formation = (Formation)Field(controller, "enemies");
                    Check(formation.Members.Count() == 3, "실제 3마리 조우");
                    foreach (var enemy in formation.Members) enemy.TakeDamage(int.MaxValue, false);
                    // 입력/피해 연출 타이밍과 무관하게 확정 승리 경계만 호출해 중복 보상을 검증합니다.
                    controller.StopAllCoroutines();
                    Call(controller, "EndBattle", "승리!", true);
                    Check(GameSessionData.Level == 4 && GameSessionData.CurrentExperience == 42, "실제 승리 52 EXP와 이월");
                    GameSaveData saved = GameSaveService.InspectSlot(3).Data;
                    Check(saved != null && saved.Level == 4 && saved.CurrentExperience == 42 && saved.CurrentSceneId == "Field_02", "승리 직후 현재 슬롯/안전 Scene 저장");
                    for (int slot = 1; slot <= GameSaveService.DefaultSaveSlotCount; slot++)
                        if (slot != 3) Check(File.ReadAllText(GameSaveService.GetSaveFilePath(slot)) == SessionState.GetString(Key + slot, ""), "다른 슬롯 불변");
                    Call(controller, "EndBattle", "승리!", true);
                    Check(GameSessionData.CurrentExperience == 42, "중복 Victory 미지급");
                    var result = GameObject.Find("VictoryResult/Result")?.GetComponent<Text>();
                    if (result == null) result = UnityEngine.Object.FindObjectsByType<Text>().FirstOrDefault(t => t.name == "Result");
                    Check(result != null && result.text.Contains("획득 EXP: 52") && result.text.Contains("Lv.3 → Lv.4"), "Victory 결과 UI");
                    Canvas.ForceUpdateCanvases();
                    Check(result.preferredHeight <= result.rectTransform.rect.height, "Victory 결과 텍스트 높이");
                    UnityEngine.Object.FindObjectsByType<Button>().Single(b => b.name == "VictoryReturn").onClick.Invoke();
                    stage = 3;
                    readyAt = EditorApplication.timeSinceStartup + 3;
                }
                else if (stage == 3)
                {
                    Check(SceneManager.GetActiveScene().name == "Field_02", "승리 Field 복귀");
                    Check(!UnityEngine.Object.FindObjectsByType<MonsterFieldController>().Any(m => m.SpawnDefinition == snakeSpawn), "처치한 스폰만 제거");
                    stage = 4;
                    readyAt = EditorApplication.timeSinceStartup + 31;
                }
                else
                {
                    Check(UnityEngine.Object.FindObjectsByType<MonsterFieldController>().Any(m => m.SpawnDefinition == snakeSpawn), "30초 뱀 리스폰");
                    GameSessionData.Reset();
                    Check(GameSaveService.TryLoadSlot(3, out GameSaveData restored), "슬롯 재로드");
                    GameSaveService.RestoreSession(3, restored);
                    Check(GameSessionData.Level == 4 && GameSessionData.CurrentExperience == 42 && GameSessionData.HasSavedWorldPosition, "진행/위치 복원");
                    EditorApplication.update -= Tick;
                    SessionState.EraseBool(Key);
                    Debug.Log("EARLY_LEVELING_AUDIT ALL PASS (Edit + Play + Field/Battle/Respawn)");
                    EditorApplication.Exit(0);
                }
            }
            catch (Exception e) { Fail(e); }
        }

        private static void AuditRules()
        {
            int[] required = { 100, 130, 170, 220, 280 };
            for (int i = 0; i < required.Length; i++) Check(ExperienceProgression.RequiredExp(i + 1) == required[i], "EXP 곡선");
            var gain = ExperienceProgression.Add(4, 200, 50);
            Check(gain.Level == 5 && gain.CurrentExperience == 30, "초과 EXP");
            gain = ExperienceProgression.Add(1, 0, 400);
            Check(gain.Level == 4 && gain.CurrentExperience == 0, "연속 레벨업");
            gain = ExperienceProgression.Add(49, 0, int.MaxValue);
            Check(gain.Level == CharacterGrowthCalculator.MaxLevel && gain.CurrentExperience == 0
                && ExperienceProgression.RequiredExp(gain.Level) == 0, "만렙/큰 EXP 안전");
            int[] levels = { 3, 4, 6, 7, 13, 14, 16, 17 };
            int[] rewards = { 0, 10, 10, 20, 20, 25, 25, 30 };
            for (int i = 0; i < levels.Length; i++)
            {
                Check(ExperienceProgression.MonsterExperience(20, 10, levels[i]) == rewards[i], "EXP 경계값");
                Check(ExperienceProgression.GetNameColor(ExperienceProgression.GetCategory(10, levels[i])) != Color.white, "몬스터 흰색 제외");
            }
            Check(ExperienceProgression.MonsterExperience(1, 10, 4) == 1, ".5 반올림");
            var before = CharacterGrowthCalculator.Calculate("mage", 3);
            var after = CharacterGrowthCalculator.Calculate("mage", 4);
            Check(CharacterGrowthCalculator.CalculateAttack("mage", after) > CharacterGrowthCalculator.CalculateAttack("mage", before)
                && CharacterGrowthCalculator.CalculateMaxHp("mage", after) > CharacterGrowthCalculator.CalculateMaxHp("mage", before), "기존 성장 공식 재계산");
            Check(CharacterGrowthCalculator.CalculateMaxMp("healer", CharacterGrowthCalculator.Calculate("healer", 4))
                == CharacterGrowthCalculator.CalculateMaxMp("healer", CharacterGrowthCalculator.Calculate("healer", 3)) + 1, "치유사 레벨 MP 성장");
            var definitions = Resources.LoadAll<MonsterDefinition>("MonsterDefinitions");
            string[] ids = { "grass_slime", "venom_bee", "forest_spider", "venom_snake" };
            int[] hp = { 60, 70, 80, 95 }, exp = { 8, 12, 16, 20 }, sums = { 24, 28, 40, 52 };
            for (int i = 0; i < ids.Length; i++)
            {
                var definition = definitions.Single(d => d.MonsterId == ids[i]);
                Check(definition.MonsterLevel == i + 1 && definition.BaseExperience == exp[i] && definition.MaxHp == hp[i], "몬스터 데이터 " + ids[i]);
                var encounter = BattlePrototypeEncounterFactory.CreateThreeVsThree("검증", "mage", 100, 36, 10,
                    definitions[0], definitions[1], definition);
                var setups = new Dictionary<Combatant, BattleParticipantSetup>(CombatantReferenceComparer.Instance);
                foreach (var setup in encounter.Enemies)
                {
                    var enemy = Actor(setup.Id, setup.MaxHp, BattleSide.Enemies);
                    enemy.TakeDamage(int.MaxValue, false);
                    setups.Add(enemy, setup);
                    Check(setup.MaxHp == setup.MonsterDefinition.MaxHp && setup.Attack == 8 + setup.MonsterDefinition.MonsterLevel * 2, "HP/Attack 실제 생성");
                }
                Check(BattleExperienceReward.Calculate(true, 3, setups.Keys.Concat(setups.Keys), setups) == sums[i], "실제 개체 합산/참조 중복 제거");
                Check(BattleExperienceReward.Calculate(false, 3, setups.Keys, setups) == 0, "승리 이외 보상 없음");
            }
            var spawnData = Resources.LoadAll<FieldMonsterSpawnDefinition>("MonsterSpawns").Where(s => s.SceneName == "Field_02").ToArray();
            Check(spawnData.Length == 6, "Field02 스폰 데이터 6개");
            foreach (var spawn in spawnData.Where(s => s.Monster.MonsterId == "venom_snake"))
            {
                Check(spawn.RespawnSeconds == 30 && Mathf.Abs(spawn.Position.x) + spawn.ActivityRadius < 10.5f
                    && Mathf.Abs(spawn.Position.y) + spawn.ActivityRadius < 7.5f, "뱀 Bounds/리스폰");
                foreach (var other in spawnData.Where(s => s != spawn))
                    Check(Vector2.Distance(spawn.Position, other.Position) > spawn.ActivityRadius + other.ActivityRadius, "활동 반경 비중첩");
                foreach (var zone in Resources.LoadAll<FieldEntranceSafetyZoneDefinition>("FieldEntranceSafetyZones").Where(z => z.SceneName == "Field_02"))
                    Check(Vector2.Distance(spawn.Position, zone.Center) > spawn.ActivityRadius + zone.Radius, "활동 반경 전체 안전");
            }
            Combatant target = Actor("same", 100), another = Actor("same", 100), bee = Actor("bee", 70), snakeActor = Actor("bee", 95);
            var status = new BattleStatusEffectRuntime();
            PoisonDefinition normal = new PoisonDefinition(), venom = new PoisonDefinition("venom", "맹독", 7, 7);
            Check(status.ApplyOrRefreshPoison(target, 3, normal, bee), "일반 독 부여");
            target.Defend(); status.ApplyGaiaWall(target, 2); status.ApplyIronWall(target, 2); status.ApplyGuardianCover(another);
            Check(status.ApplyPoisonTickAtActionEnd(target, out int ticks) == 5 && ticks == 2, "독 직접 피해 감소 무시");
            Check(status.ApplyOrRefreshPoison(target, 3, venom, snakeActor) && status.GetPoisonRemaining(target) == 3, "강한 독 교체");
            Check(status.ApplyPoisonTickAtActionEnd(target, out ticks) == 7 && ticks == 2, "맹독 7%, 중첩 12% 없음");
            Check(!status.ApplyOrRefreshPoison(target, 3, normal, bee) && status.GetPoisonRemaining(target) == 2, "약한 독 거부/수명 유지");
            Check(status.ApplyOrRefreshPoison(target, 3, venom, snakeActor) && status.GetPoisonRemaining(target) == 3, "같은 강도 갱신");
            Check(status.GetPoisonSource(target) == snakeActor && !status.HasActivePoison(another), "Source/개체 참조 독립");
            Check(status.RemoveAllHarmfulStatuses(target) == 1 && !status.HasActivePoison(target), "맹독 정화");
            status.ApplyOrRefreshPoison(target, 3, normal, bee);
            Check(status.RemoveAllHarmfulStatuses(target) == 1, "일반 독 정화");
            var fragile = Actor("fragile", 1);
            status.ApplyOrRefreshPoison(fragile, 3, venom);
            Check(status.ApplyPoisonTickAtActionEnd(fragile, out _) == 1 && !fragile.IsAlive && !status.HasActivePoison(fragile), "최소 피해/전투불능 제거");
            var cooldowns = new BattleMonsterAbilityRuntime();
            cooldowns.StartPoisonInflictionCooldown(bee, 2);
            cooldowns.CompleteActorAction(bee);
            Check(cooldowns.GetPoisonInflictionCooldown(bee) == 2 && cooldowns.CanInflictPoison(snakeActor), "첫 행동 제외/개체 독립");
            cooldowns.CompleteActorAction(snakeActor);
            cooldowns.CompleteActorAction(bee);
            Check(cooldowns.GetPoisonInflictionCooldown(bee) == 1, "대기 2→1");
            cooldowns.CompleteActorAction(bee);
            Check(cooldowns.CanInflictPoison(bee), "대기 1→0");
            for (int slot = 1; slot <= GameSaveService.DefaultSaveSlotCount; slot++)
            {
                var saved = JsonUtility.FromJson<GameSaveData>(JsonUtility.ToJson(new GameSaveData {
                    Level = slot, CurrentExperience = slot * 10, HasSavedWorldPosition = true, SavedPositionX = slot }));
                Check(saved.Level == slot && saved.CurrentExperience == slot * 10 && saved.SavedPositionX == slot, "진행/위치 JSON 왕복");
            }
            Debug.Log("EARLY_LEVELING_AUDIT rules PASS");
            typeof(BattleSkillButtonIconAudit).GetMethod("Audit", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, null);
        }

        private static void PrepareTestSaves()
        {
            // 실행 경로 가드를 통과한 격리 사본의 테스트 파일만 씁니다. 실제 프로젝트 UserData는 복사하지 않습니다.
            for (int slot = 1; slot <= GameSaveService.DefaultSaveSlotCount; slot++)
            {
                GameSaveService.SelectSlot(slot);
                GameSessionData.ConfigurePlayer(ProjectLimitless.Player.PlayerVisualType.Male, "성장 검증 " + slot);
                GameSessionData.SelectPlayerPath(Resources.LoadAll<PlayerPathDefinition>("PathDefinitions").First().Id);
                GameSessionData.SelectJob("mage");
                GameSessionData.ConfigureProgress(slot, slot * 10);
                GameSessionData.RecordWorldPosition(0f, 0f);
                Check(GameSaveService.SaveCurrentSession("Field_02", ""), "테스트 슬롯 저장");
                Check(GameSaveService.TryLoadSlot(slot, out GameSaveData data) && data.Level == slot && data.CurrentExperience == slot * 10, "5슬롯 독립 저장/로드");
                SessionState.SetString(Key + slot, File.ReadAllText(GameSaveService.GetSaveFilePath(slot)));
            }
        }

        private static Combatant Actor(string id, int hp, BattleSide side = BattleSide.Allies) =>
            new Combatant(id, id, side, new FormationSlot(FormationRow.Front, 0), hp, 10, 10, 0, TargetRangeType.MeleePhysical, true);
        private static object Field(object target, string name) => target.GetType().GetField(name, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(target);
        private static void Call(object target, string name, params object[] args) => target.GetType().GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic).Invoke(target, args);
        private static void Check(bool value, string message) { if (!value) throw new Exception("EARLY_LEVELING_AUDIT FAIL: " + message); }
        private static void Fail(Exception error) { EditorApplication.update -= Tick; SessionState.EraseBool(Key); Debug.LogException(error); EditorApplication.Exit(1); }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ProjectLimitless.Core;
using ProjectLimitless.NPC;
using ProjectLimitless.Player;
using ProjectLimitless.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace ProjectLimitless.EditorTools
{
    /// <summary>격리 복사본의 Batch Mode에서만 의뢰·실제 NPC/UI·저장/Continue를 검증합니다.</summary>
    [InitializeOnLoad]
    public static class SideQuestAudit
    {
        private const string Key = "Limitless.SideQuestAudit";
        private const BindingFlags Flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;
        private static string Root => Path.GetFullPath(Path.Combine(Application.dataPath, "../SideQuestAuditResults"));
        private static IEnumerator routine;
        private static readonly List<string> results = new List<string>();
        private static double deadline;
        private static int errors;
        private static QuestDefinition[] Sides => QuestCatalog.All.Where(x => x.IsItemDelivery).OrderBy(x => x.QuestId).ToArray();
        static SideQuestAudit()
        {
            EditorApplication.playModeStateChanged += state =>
            {
                if (!SessionState.GetBool(Key, false)) return;
                if (state == PlayModeStateChange.EnteredPlayMode)
                {
                    GameSaveService.AuditSaveDirectory = Path.Combine(Root, "Saves");
                    UserSettingsService.BeginAudit(Path.Combine(Root, "Settings"));
                    UserSettingsService.SetMuteAll(true);
                    Application.runInBackground = true;
                    Application.logMessageReceived += Log;
                    results.Clear();
                    // Application.CanStreamedLevelBeLoaded는 배치 Edit 단계의 Scene 목록 준비 전에는 false입니다.
                    // 실제 저장 검증은 Play 상태에서 수행하고 원본 Save 검증 규칙은 완화하지 않습니다.
                    routine = Flatten(Runtime()); deadline = EditorApplication.timeSinceStartup + 240;
                    EditorApplication.update += Tick;
                }
            };
        }

        public static void RunBatch()
        {
            if (!Application.isBatchMode || !Application.dataPath.Replace('\\', '/').Contains("/Temp/SideQuestQA/Client/Assets"))
                throw new InvalidOperationException("격리 SideQuestQA 프로젝트 Batch Mode에서만 실행합니다.");
            Directory.CreateDirectory(Root);
            try
            {
                Check(Sides.Length == 9, "EditMode.QuestAssets9");
                GameSaveService.FinishAudit();
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
                SessionState.SetBool(Key, true);
                EditorApplication.EnterPlaymode();
            }
            catch (Exception e) { Fail(e); }
        }

        private static void Check(bool condition, string id)
        {
            results.Add((condition ? "PASS|" : "FAIL|") + id);
            File.WriteAllLines(Path.Combine(Root, "results.txt"), results);
            if (!condition) throw new InvalidOperationException(id);
        }
        private static void Seed()
        {
            GameSessionData.Reset(); GameSaveService.SelectSlot(1);
            GameSessionData.ConfigurePlayer(PlayerVisualType.Male, "격리 의뢰 테스트");
            GameSessionData.SelectJob("fighter"); GameSessionData.SelectPlayerPath("path.hearing");
            GameSessionData.RecordLocation("World_StarterVillage", "Spawn_From_Field01");
            QuestService.ImportSaveData(new QuestProgressSaveData
            { CompletedQuestIds = QuestCatalog.All.Where(x => x.QuestType == QuestType.Main).Select(x => x.QuestId).ToArray() });
        }
        private static void AddNeeds(QuestDefinition quest, int extra = 0)
        { foreach (var objective in quest.Objectives) Check(InventoryService.TryAddItem(objective.TargetId, objective.RequiredCount + extra), "Fixture.Item." + objective.TargetId); }

        private static void EditChecks()
        {
            Check(Sides.Length == 9 && Sides.Take(5).Sum(x => x.Reward.Experience) == 155
                && Sides.Skip(5).Sum(x => x.Reward.Experience) == 285 && Sides.Sum(x => x.Reward.Currency) == 173, "Data.Count9.Reward440.173");
            Check(QuestCatalog.All.Select(x => x.QuestId).Distinct().Count() == QuestCatalog.All.Count, "Data.UniqueIds");
            foreach (var quest in Sides)
            {
                Seed(); QuestService.Reset();
                Check(QuestService.GetState(quest.QuestId) == QuestState.Locked && !QuestService.TryStart(quest.QuestId), "Prerequisite.Locked." + quest.QuestId);
                QuestService.ImportSaveData(new QuestProgressSaveData { CompletedQuestIds = quest.PrerequisiteQuestIds.ToArray() });
                Check(QuestService.GetState(quest.QuestId) == QuestState.Available, "Prerequisite.Available." + quest.QuestId);
                AddNeeds(quest); Check(QuestService.TryStart(quest.QuestId), "Accept." + quest.QuestId);
                var state = QuestService.ActiveSideQuests.Single();
                Check(state.CurrentObjective == null && QuestService.CanDeliverItems(quest)
                    && QuestService.BuildItemProgress(state).Contains("필요한 재료를 모두"), "Preowned.Progress." + quest.QuestId);
                var target = quest.Objectives[0]; int changeEvents = 0; Action changed = () => changeEvents++;
                QuestService.Changed += changed;
                Check(ShopService.TrySell(target.TargetId, 1) == ShopTransactionResult.Success
                    && state.CurrentObjective != null && changeEvents > 0, "Sell.ProgressDecrease." + quest.QuestId);
                QuestService.Changed -= changed;
                Check(GameSaveService.SaveCurrentSession() && GameSaveService.TryLoadSlot(1, out var partial), "Save.Partial." + quest.QuestId + "." + GameSaveService.InspectSlot(1).Error);
                GameSaveService.TryLoadSlot(1, out partial); GameSaveService.RestoreSession(1, partial);
                state = QuestService.ActiveSideQuests.Single();
                Check(state.CurrentObjective != null && !QuestService.CanDeliverItems(quest), "Save.Partial.RestoredCounts." + quest.QuestId);
                string before = JsonUtility.ToJson(new Snapshot { Inventory = InventoryService.ExportSaveData() });
                Check(!QuestService.TryDeliverSideQuest(quest.QuestId, quest.TurnInNpcId)
                    && before == JsonUtility.ToJson(new Snapshot { Inventory = InventoryService.ExportSaveData() }), "Insufficient.Atomic." + quest.QuestId);
                Check(InventoryService.TryAddItem(target.TargetId, 1), "RestoreItem." + quest.QuestId);
                Check(!QuestService.TryDeliverSideQuest(quest.QuestId, "wrong-npc"), "WrongNpc." + quest.QuestId);
                EconomyService.Import(int.MaxValue); before = JsonUtility.ToJson(new Snapshot { Inventory = InventoryService.ExportSaveData() });
                Check(!QuestService.TryDeliverSideQuest(quest.QuestId, quest.TurnInNpcId)
                    && before == JsonUtility.ToJson(new Snapshot { Inventory = InventoryService.ExportSaveData() }), "RewardOverflow.NoConsumption." + quest.QuestId);
                EconomyService.Reset(); InventoryService.TryAddItem("material_scorching_watcher_core_fragment", 3);
                Check(QuestService.TryDeliverSideQuest(quest.QuestId, quest.TurnInNpcId)
                    && quest.Objectives.All(x => InventoryService.GetItemCount(x.TargetId) == 0)
                    && InventoryService.GetItemCount("material_scorching_watcher_core_fragment") == 3
                    && EconomyService.GetCurrency() == quest.Reward.Currency, "Delivery.Exact.OtherItemProtected." + quest.QuestId);
                int currency = EconomyService.GetCurrency();
                Check(!QuestService.TryDeliverSideQuest(quest.QuestId, quest.TurnInNpcId) && !QuestService.TryStart(quest.QuestId)
                    && EconomyService.GetCurrency() == currency, "Completed.Once." + quest.QuestId);
                Check(GameSaveService.TryLoadSlot(1, out var saved), "Save.Completed.Read." + quest.QuestId);
                GameSaveService.RestoreSession(1, saved); GameSaveService.RestoreSession(1, saved);
                Check(QuestService.GetState(quest.QuestId) == QuestState.Completed && !QuestService.TryDeliverSideQuest(quest.QuestId, quest.TurnInNpcId)
                    && EconomyService.GetCurrency() == currency, "Load.Repeat.NoDuplicate." + quest.QuestId);
            }
            Seed(); foreach (var quest in Sides) Check(QuestService.TryStart(quest.QuestId), "Concurrent." + quest.QuestId);
            Check(QuestService.ActiveSideQuests.Count() == 9, "Concurrent9");
            QuestService.Reset(); var main = QuestCatalog.All.Single(x => x.QuestId == "main_19_burning_pulse");
            QuestService.ImportSaveData(new QuestProgressSaveData { CompletedQuestIds = main.PrerequisiteQuestIds.ToArray() });
            Check(QuestService.TryStart(main.QuestId), "Main.TrackFixture");
            QuestService.ImportSaveData(new QuestProgressSaveData { ActiveQuests = QuestService.ExportSaveData().ActiveQuests,
                CompletedQuestIds = QuestCatalog.All.Where(x => x.QuestType == QuestType.Main && x != main).Select(x => x.QuestId).ToArray(), TrackedQuestId = main.QuestId });
            Check(QuestService.TryStart(Sides[0].QuestId) && QuestService.TrackedQuestId == main.QuestId, "Main.TrackPreserved");
            Check(QuestService.TryTrack(Sides[0].QuestId), "Side.TrackSwitch");
            Check(GameSaveService.TryLoadSlot(1, out var tracked), "Track.Save"); GameSaveService.RestoreSession(1, tracked);
            Check(QuestService.TrackedQuestId == Sides[0].QuestId, "Track.Restored");

            // 메인19개를 실제 순차 사건 API로 진행합니다. 필드 이동/전투 연출 전체 회귀와는 구분합니다.
            foreach (var quest in QuestCatalog.All.Where(x => x.QuestType == QuestType.Main).OrderBy(x => x.QuestId))
            {
                Seed(); QuestService.ImportSaveData(new QuestProgressSaveData { CompletedQuestIds = quest.PrerequisiteQuestIds.ToArray() });
                Check(QuestService.TryStart(quest.QuestId), "Main01to19.Start." + quest.QuestId);
                foreach (var step in quest.Objectives)
                {
                    for (int i = 0; i < step.RequiredCount; i++) Notify(step);
                    int index = QuestService.ActiveMainQuest?.CurrentObjectiveIndex ?? -1;
                    var next = QuestService.ActiveMainQuest?.CurrentObjective;
                    if (next != null && next.ObjectiveType == step.ObjectiveType && next.TargetId == step.TargetId)
                    {
                        // Main09의 연속 폴 대화는 같은 NPC에서 서로 다른 대화 종료를 기다립니다.
                        // 일반 사건 API에는 대화 실행ID가 없으므로 두 번째 호출을 중복으로 가정하지 않습니다.
                        results.Add("CONTRACT|AdjacentSameTarget.CallerDialogueBoundary." + quest.QuestId + "." + step.ObjectiveId);
                    }
                    else
                    {
                        Notify(step); Check(index == (QuestService.ActiveMainQuest?.CurrentObjectiveIndex ?? -1), "Main.DuplicateObjective." + quest.QuestId + "." + step.ObjectiveId);
                    }
                }
                if (!string.IsNullOrEmpty(quest.TurnInNpcId)) QuestService.NotifyNpcTalked(quest.TurnInNpcId);
                Check(QuestService.GetState(quest.QuestId) == QuestState.Completed, "Main01to19.Complete." + quest.QuestId);
            }
            Seed(); InventoryService.Reset();
            // 기존 Main18 규칙 검증은 격리 복사본에서만 실행하며 그 결과 파일도 복사본 밖으로 나가지 않습니다.
            Directory.CreateDirectory(Path.GetFullPath(Path.Combine(Application.dataPath, "../../../문서/00_프로젝트")));
            typeof(Main18RuntimeAudit).GetMethod("Rules", Flags).Invoke(null, null);
            Check(Main18RuntimeAudit.Results.All(x => x.StartsWith("PASS|")), "Regression.Main18.Overheat.Cooling.AI.Rules");
            File.WriteAllLines(Path.Combine(Root, "main18_rules.txt"), Main18RuntimeAudit.Results);
            GameSaveData legacy = JsonUtility.FromJson<GameSaveData>("{\"Version\":1,\"Level\":4}");
            QuestService.ImportSaveData(legacy.QuestProgress); InventoryService.ImportSaveData(legacy.Inventory);
            Check(!QuestService.ActiveQuests.Any() && !InventoryService.ExportSaveData().Any(), "Legacy.MissingFields");
            for (int slot = 1; slot <= 5; slot++) { Seed(); GameSaveService.SelectSlot(slot); Check(GameSaveService.SaveCurrentSession() && GameSaveService.TryLoadSlot(slot, out _), "FiveSlots." + slot); }
            Seed(); ExtraAtomicChecks();
        }

        private static void ExtraAtomicChecks()
        {
            var template = Sides[0]; var second = ScriptableObject.CreateInstance<QuestDefinition>();
            second.ConfigureForAudit("audit_shared_material", "재료 공유 검증", QuestType.Side, template.Objectives.ToArray());
            second.ConfigureNpcFlow(template.StartNpcId, template.TurnInNpcId); QuestCatalog.RegisterForAudit(second);
            QuestService.TryStart(template.QuestId); QuestService.TryStart(second.QuestId); AddNeeds(template);
            Check(QuestService.TryDeliverSideQuest(template.QuestId, template.TurnInNpcId)
                && !QuestService.TryDeliverSideQuest(second.QuestId, second.TurnInNpcId), "SharedMaterial.NoDoubleSpend");
            QuestService.Reset(); QuestCatalog.ReloadForAudit(); Object.DestroyImmediate(second);
            Seed(); var blocked = ScriptableObject.CreateInstance<QuestDefinition>();
            blocked.ConfigureForAudit("audit_reward_full", "보상 수용 검증", QuestType.Side, template.Objectives.ToArray(),
                new RewardBundle { Currency = 5, Items = new[] { new ItemReward { ItemId = "material_soot_hound_fang", Count = 1 } } });
            blocked.ConfigureNpcFlow(template.StartNpcId, template.TurnInNpcId); QuestCatalog.RegisterForAudit(blocked);
            QuestService.TryStart(blocked.QuestId); AddNeeds(blocked); InventoryService.TryAddItem("material_soot_hound_fang", 99);
            Check(!QuestService.TryDeliverSideQuest(blocked.QuestId, blocked.TurnInNpcId)
                && template.Objectives.All(x => InventoryService.GetItemCount(x.TargetId) == x.RequiredCount)
                && EconomyService.GetCurrency() == 0, "RewardFull.NoConsumption.NoCurrency");
            QuestService.NotifyNpcTalked(blocked.TurnInNpcId);
            Check(QuestService.GetState(blocked.QuestId) == QuestState.Active, "TalkCannotAutoDeliver");
            QuestService.Reset(); QuestCatalog.ReloadForAudit(); Object.DestroyImmediate(blocked);
        }
        private static void Notify(QuestObjectiveDefinition step)
        {
            switch (step.ObjectiveType)
            {
                case QuestObjectiveType.TalkToNpc: QuestService.NotifyNpcTalked(step.TargetId); break;
                case QuestObjectiveType.ReachLocation: QuestService.NotifyLocationReached(step.TargetId); break;
                case QuestObjectiveType.DefeatEncounter: QuestService.NotifyEncounterWon(step.TargetId); break;
                case QuestObjectiveType.Interact: QuestService.NotifyInteraction(step.TargetId); break;
                case QuestObjectiveType.GenericSignal: QuestService.NotifySignal(step.TargetId); break;
            }
        }

        private static void Balance()
        {
            var mains = QuestCatalog.All.Where(x => x.QuestType == QuestType.Main).OrderBy(x => x.QuestId).ToArray();
            var monsters = Resources.LoadAll<ProjectLimitless.Monster.MonsterDefinition>("MonsterDefinitions").OrderBy(x => x.MonsterLevel).ToArray();
            var lines = new List<string>();
            foreach (bool sides in new[] { false, true }) foreach (int fights in new[] { 0, 2, 5 })
            {
                int level = 1, exp = 0, total = 0; var done = new HashSet<string>();
                foreach (var main in mains)
                {
                    var gain = ExperienceProgression.Add(level, exp, main.Reward.Experience); level = gain.Level; exp = gain.CurrentExperience; total += main.Reward.Experience; done.Add(main.QuestId);
                    if (sides) foreach (var side in Sides.Where(x => x.PrerequisiteQuestIds.Contains(main.QuestId)))
                    { gain = ExperienceProgression.Add(level, exp, side.Reward.Experience); level = gain.Level; exp = gain.CurrentExperience; total += side.Reward.Experience; }
                    // 가정: 각 Main 뒤 해당 지역 레벨의 일반 적 세 마리와 2/5전투를 진행합니다.
                    int chapterLevel = Math.Min(14, Math.Max(1, int.Parse(main.QuestId.Substring(5, 2)) - 3));
                    var monster = monsters.OrderBy(x => Math.Abs(x.MonsterLevel - chapterLevel)).First();
                    for (int n = 0; n < fights; n++)
                    { int reward = 3 * ExperienceProgression.MonsterExperience(monster.BaseExperience, level, monster.MonsterLevel); total += reward; gain = ExperienceProgression.Add(level, exp, reward); level = gain.Level; exp = gain.CurrentExperience; }
                    if (main.QuestId.StartsWith("main_11") || main.QuestId.StartsWith("main_19")) lines.Add($"sides={sides}, 3-enemy-fights-per-main={fights}, after={main.QuestId}, Level={level}, remainingEXP={exp}, accumulatedEXP={total}");
                }
            }
            File.WriteAllLines(Path.Combine(Root, "balance.txt"), lines);
            Check(CharacterGrowthCalculator.MaxLevel == 50 && ExperienceProgression.MonsterExperience(100, 20, 13) == 0
                && ExperienceProgression.MonsterExperience(100, 20, 16) == 50, "Balance.Level50.ExpMultipliers");
        }

        private static IEnumerator Runtime()
        {
            EditChecks(); Balance();
            File.WriteAllLines(Path.Combine(Root, "service_results.txt"), results);
            Seed(); SceneManager.LoadSceneAsync("World_StarterVillage"); yield return Scene("World_StarterVillage");
            foreach (var quest in Sides)
            {
                string scene = quest.QuestId.StartsWith("side_c1") ? "World_StarterVillage" : "Arbel";
                if (SceneManager.GetActiveScene().name != scene) { SceneManager.LoadSceneAsync(scene); yield return Scene(scene); }
                var role = Object.FindObjectsByType<VillageNpcRole>(FindObjectsSortMode.None).Single(x => x.NpcId == quest.StartNpcId);
                var npc = role.GetComponent<NpcController>();
                // 거절 후 재수락: 실제 선택창 Button과 공용 대화창의 Cancel/Confirm callback을 실행합니다.
                role.Interact(npc); yield return null;
                Check(SideQuestNpcPresenter.Instance.IsOpen && SideQuestNpcPresenter.Instance.QuestChoiceCount > 0, "Runtime.NpcChoices." + quest.QuestId);
                FindChoice(quest.DisplayName).onClick.Invoke(); yield return null;
                DialogueButton("cancelButton").onClick.Invoke();
                Check(QuestService.GetState(quest.QuestId) == QuestState.Available, "Runtime.Decline." + quest.QuestId);
                AddNeeds(quest, 1); yield return null;
                role.Interact(npc); yield return null; FindChoice(quest.DisplayName).onClick.Invoke(); yield return null;
                DialogueButton("confirmButton").onClick.Invoke(); yield return null;
                Check(QuestService.GetState(quest.QuestId) == QuestState.Active && QuestService.CanDeliverItems(quest), "Runtime.Accept.Preowned." + quest.QuestId);
                DialoguePresenter.Instance.Hide();
                Check(GameSaveService.SaveCurrentSession(), "Runtime.Accept.Save." + quest.QuestId);
                yield return Continue("Active." + quest.QuestId);
                role = Object.FindObjectsByType<VillageNpcRole>(FindObjectsSortMode.None).Single(x => x.NpcId == quest.TurnInNpcId); npc = role.GetComponent<NpcController>();
                var log = QuestLogPresenter.Instance; log.Open();
                Check(log.Details.Contains("/"), "Runtime.QuestLog.Counts." + quest.QuestId); log.Close();
                role.Interact(npc); yield return null; FindChoice(quest.DisplayName).onClick.Invoke(); yield return null;
                int beforeCurrency = EconomyService.GetCurrency(); DialogueButton("confirmButton").onClick.Invoke(); yield return null;
                Check(QuestService.GetState(quest.QuestId) == QuestState.Completed && EconomyService.GetCurrency() == beforeCurrency + quest.Reward.Currency
                    && quest.Objectives.All(x => InventoryService.GetItemCount(x.TargetId) == 1), "Runtime.Deliver.Reward.Once." + quest.QuestId);
                DialoguePresenter.Instance.Hide(); yield return Continue("Completed." + quest.QuestId);
                Check(!QuestService.TryDeliverSideQuest(quest.QuestId, quest.TurnInNpcId), "Runtime.Continue.NoDuplicate." + quest.QuestId);
            }
            Check(Sides.All(x => QuestService.GetState(x.QuestId) == QuestState.Completed), "Runtime.Completed9");
            Check(errors == 0, "Runtime.ConsoleErrors0");
            File.WriteAllText(Path.Combine(Root, "status.txt"), "PASS");
        }
        private static UnityEngine.UI.Button DialogueButton(string field) => (UnityEngine.UI.Button)typeof(DialoguePresenter).GetField(field, Flags).GetValue(DialoguePresenter.Instance);
        private static UnityEngine.UI.Button FindChoice(string title) => SideQuestNpcPresenter.Instance.GetComponentsInChildren<UnityEngine.UI.Button>()
            .Single(x => x.GetComponentInChildren<UnityEngine.UI.Text>().text.Contains(title));
        private static IEnumerator Continue(string label)
        {
            string scene = SceneManager.GetActiveScene().name;
            var quest = JsonUtility.ToJson(QuestService.ExportSaveData());
            var items = JsonUtility.ToJson(new Snapshot { Inventory = InventoryService.ExportSaveData() });
            int money = EconomyService.GetCurrency(), level = GameSessionData.Level, exp = GameSessionData.CurrentExperience;
            Check(GameSaveService.SaveCurrentSession(scene), "Runtime.Continue.Write." + label);
            SceneManager.LoadSceneAsync("Bootstrap"); yield return Scene("Bootstrap"); GameSessionData.Reset();
            var button = GameObject.Find("StartMenuCanvas/Slot01/Action")?.GetComponent<UnityEngine.UI.Button>();
            Check(button != null, "Runtime.Continue.RealBootstrapButton." + label); button.onClick.Invoke(); yield return Scene(scene);
            Check(quest == JsonUtility.ToJson(QuestService.ExportSaveData()) && items == JsonUtility.ToJson(new Snapshot { Inventory = InventoryService.ExportSaveData() })
                && money == EconomyService.GetCurrency() && level == GameSessionData.Level && exp == GameSessionData.CurrentExperience, "Runtime.Continue.StateExact." + label);
        }
        private static IEnumerator Scene(string name)
        {
            double until = EditorApplication.timeSinceStartup + 30;
            while (SceneManager.GetActiveScene().name != name && EditorApplication.timeSinceStartup < until) yield return null;
            Check(SceneManager.GetActiveScene().name == name, "Runtime.Scene." + name);
            double ready = EditorApplication.timeSinceStartup + 1;
            while (EditorApplication.timeSinceStartup < ready) yield return null;
            ProjectLimitless.Monster.MonsterEncounterService.SuppressForSeconds(3600);
            Check(SceneManager.GetActiveScene().GetRootGameObjects().Sum(x => x.GetComponentsInChildren<Transform>(true)
                .Sum(t => GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(t.gameObject))) == 0, "Runtime.MissingScript0." + name);
        }
        private static IEnumerator Flatten(IEnumerator root)
        {
            var stack = new Stack<IEnumerator>(); stack.Push(root);
            while (stack.Count > 0) { var next = stack.Peek(); if (!next.MoveNext()) { stack.Pop(); continue; }
                if (next.Current is IEnumerator nested) stack.Push(nested); else yield return null; }
        }
        private static void Log(string message, string stack, LogType type)
        { if (type == LogType.Error || type == LogType.Exception || type == LogType.Assert) { errors++; results.Add("ERROR|" + message); } }
        private static void Tick()
        {
            try
            {
                if (EditorApplication.timeSinceStartup > deadline) throw new TimeoutException("Runtime timeout");
                if (routine.MoveNext()) return;
                EditorApplication.update -= Tick; SessionState.SetBool(Key, false); File.WriteAllLines(Path.Combine(Root, "results.txt"), results);
                EditorApplication.Exit(0);
            }
            catch (Exception e) { Fail(e); }
        }
        private static void Fail(Exception e)
        {
            results.Add("FAIL|" + e); Directory.CreateDirectory(Root);
            File.WriteAllLines(Path.Combine(Root, "results.txt"), results); File.WriteAllText(Path.Combine(Root, "status.txt"), "FAIL");
            Debug.LogException(e); EditorApplication.Exit(1);
        }
        [Serializable] private sealed class Snapshot { public InventoryEntry[] Inventory; }
    }
}

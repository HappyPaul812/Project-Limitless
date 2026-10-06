using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ProjectLimitless.Battle;
using ProjectLimitless.Core;
using ProjectLimitless.Monster;
using ProjectLimitless.World;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ProjectLimitless.Player;
using ProjectLimitless.UI;
using ProjectLimitless.Audio;

namespace ProjectLimitless.EditorTools
{
    /// <summary>생산 Battle Controller의 실제 행동·상태·승리 경로로 Main17 대표 밸런스를 측정합니다. 사용자 Save는 격리 실행기가 보호합니다.</summary>
    [InitializeOnLoad]
    public static class Main17ClosureAudit
    {
        const BindingFlags Hidden = BindingFlags.Instance | BindingFlags.NonPublic;
        const BindingFlags StaticHidden = BindingFlags.Static | BindingFlags.NonPublic;
        const string Armed = "Limitless.Main17Closure";
        static readonly List<string> rows = new List<string>();
        public static string Status = "Idle";
        static string Root => Path.GetFullPath(Path.Combine(Application.dataPath, "../../../문서/00_프로젝트"));
        static Main17ClosureAudit()
        {
            var initialize = Partial9FixedSpriteAudit.Status;
            EditorApplication.playModeStateChanged += state =>
            {
                if (!SessionState.GetBool(Armed, false)) return;
                if (state == PlayModeStateChange.EnteredPlayMode)
                    typeof(Partial9FixedSpriteAudit).GetField("routine", StaticHidden).SetValue(null, SessionState.GetBool(Armed + ".Finish", false) ? RunFinish() : Run());
                if (state == PlayModeStateChange.EnteredEditMode)
                {
                    SessionState.SetBool(Armed, false);
                    // 다음 실행에 이번 표본 종류가 남아 검증 ID나 전투 정책을 바꾸지 않도록 정리합니다.
                    SessionState.SetBool(Armed + ".Boundary", false);
                    SessionState.SetBool(Armed + ".Healing", false);
                    Status = Partial9FixedSpriteAudit.Status;
                }
            };
        }
        public static string Launch()
        {
            SessionState.SetBool(Armed + ".Healing", false);
            SessionState.SetBool(Armed + ".Low", false); SessionState.SetBool(Armed + ".Finish", false);
            rows.Clear(); rows.Add("Job,Level,Encounter,Win,AllyActions,EnemyActions,DamageTaken,HealingReceived,Potions,AllyKO,Seconds");
            Status = "Running"; SessionState.SetBool(Armed, true); return Partial9FixedSpriteAudit.Launch();
        }
        /// <summary>Quest 보상만으로 도달하는 Lv4 하한을 추가합니다. 기존30개 표본을 다시 실행하지 않습니다.</summary>
        public static string LaunchLow()
        {
            SessionState.SetBool(Armed + ".Healing", false);
            rows.Clear(); rows.AddRange(File.ReadAllLines(Path.Combine(Root, "Main17_Balance_Matrix.csv")));
            SessionState.SetBool(Armed + ".Low", true); SessionState.SetBool(Armed + ".Finish", false);
            SessionState.SetBool(Armed, true); Status = "Running"; return Partial9FixedSpriteAudit.Launch();
        }
        public static string LaunchFinish()
        {
            SessionState.SetBool(Armed + ".Boundary", false);
            SessionState.SetBool(Armed + ".Finish", true); SessionState.SetBool(Armed, true);
            Status = "Running"; return Partial9FixedSpriteAudit.Launch();
        }
        public static string LaunchRewardBoundary()
        {
            SessionState.SetBool(Armed + ".Boundary", true); SessionState.SetBool(Armed + ".Finish", true); SessionState.SetBool(Armed, true);
            Status = "Running"; return Partial9FixedSpriteAudit.Launch();
        }
        /// <summary>치유사를 기본공격만 쓰는 정책과 구분하여 실제 치유 스킬의 MP·회복 비용을 측정합니다.</summary>
        public static string LaunchHealing()
        {
            rows.Clear(); rows.Add("Job,Level,Encounter,Win,AllyActions,EnemyActions,DamageTaken,HealingReceived,Potions,AllyKO,Seconds");
            SessionState.SetBool(Armed + ".Healing", true); SessionState.SetBool(Armed + ".Low", false); SessionState.SetBool(Armed + ".Finish", false);
            SessionState.SetBool(Armed, true); Status = "Running"; return Partial9FixedSpriteAudit.Launch();
        }
        static T Get<T>(object c, string field) => (T)c.GetType().GetField(field, Hidden).GetValue(c);
        static void Check(bool condition, string id)
        {
            if (SessionState.GetBool(Armed + ".Boundary", false)) id = "LevelUp." + id;
            File.AppendAllText(Path.Combine(Root, "Main17_Closure_Runtime.txt"), (condition ? "PASS|" : "FAIL|") + id + "\n");
            if (!condition) throw new InvalidOperationException(id);
        }
        static void Hit(BattleSceneController c, Combatant target) => typeof(SecondRegressionAudit).GetMethod("Hit", StaticHidden).Invoke(null, new object[] { c, target });
        /// <summary>보상 변경에 영향받는 마지막 실제 대화·완료·저장만 검증합니다. 기존5지점 저장은 반복하지 않습니다.</summary>
        static IEnumerator RunFinish()
        {
            typeof(SecondRegressionAudit).GetMethod("Seed", StaticHidden).Invoke(null, new object[] { "fighter", 12 });
            if (SessionState.GetBool(Armed + ".Boundary", false)) GameSessionData.ConfigureProgress(12, ExperienceProgression.RequiredExp(12) - 30);
            CompanionRosterService.UnlockPaul("fighter"); CompanionRosterService.UnlockSerin();
            QuestDefinition quest; QuestCatalog.TryGet(Chapter2Main17Flow.QuestId, out quest);
            QuestService.ImportSaveData(new QuestProgressSaveData
            {
                CompletedQuestIds = QuestCatalog.All.Where(x => x.QuestId.StartsWith("main_") && string.CompareOrdinal(x.QuestId, "main_17") < 0).Select(x => x.QuestId).ToArray(),
                ActiveQuests = new[] { new ActiveQuestSaveData { QuestId = quest.QuestId, Objectives = quest.Objectives.Select((x, i) => new QuestObjectiveProgressData { ObjectiveId = x.ObjectiveId, CurrentCount = i < 11 ? 1 : 0 }).ToArray() } }
            });
            SceneManager.LoadSceneAsync(Chapter2Main17Flow.Field);
            for (int i = 0; i < 20; i++) yield return null;
            var player = UnityEngine.Object.FindAnyObjectByType<PlayerController>();
            Check(player != null && Chapter2Main17Flow.IsCurrent(11), "Finish.ExactLastObjective");
            Check(GameObject.Find("RedRiftTitle").GetComponent<TextMesh>().text == "붉은 균열 협곡", "Finish.RegionName");
            Check(UnityEngine.Object.FindAnyObjectByType<QuestHudPresenter>() != null, "Finish.QuestHudPresent");
            player.transform.position = Chapter2Main17Flow.Positions[11]; Physics2D.SyncTransforms();
            for (int i = 0; i < 3; i++) yield return null;
            var site = GameObject.Find("field08_main17_withdraw").GetComponent<Main17Site>();
            Check(site.transform.Find("SiteLabel").gameObject.activeInHierarchy, "Finish.InspectPrompt");
            string party = JsonUtility.ToJson(CompanionRosterService.ExportSaveData()), beast = JsonUtility.ToJson(BeastCompanionService.ExportSaveData());
            int level = GameSessionData.Level, experience = GameSessionData.CurrentExperience, currency = EconomyService.GetCurrency();
            var expected = ExperienceProgression.Add(level, experience, 60);
            site.TryInteract(); Check(DialoguePresenter.Instance.IsOpen, "Finish.DialogueOpened");
            while (DialoguePresenter.Instance.IsOpen)
            {
                var p = DialoguePresenter.Instance; var line = Get<DialogueLine[]>(p, "sequenceLines")[Get<int>(p, "sequencePageIndex")];
                Check(Get<VoicePlaybackSource>(p, "voicePlayback").Clip == null, "Finish." + line.DialogueId + ".NoVoice");
                Check(Get<GameObject>(p, "portraitRoot").activeSelf == (line.SpeakerId != "" && line.SpeakerId != "player"), "Finish." + line.DialogueId + ".Portrait");
                p.Advance(); yield return null;
            }
            Check(QuestService.GetState(quest.QuestId) == QuestState.Completed, "Finish.Completed");
            Check(GameSessionData.Level == expected.Level && GameSessionData.CurrentExperience == expected.CurrentExperience && EconomyService.GetCurrency() == currency + 50, "Reward.Actual60Exp50Talent");
            QuestService.NotifyInteraction("field08_main17_withdraw"); site.TryInteract();
            Check(GameSessionData.CurrentExperience == expected.CurrentExperience && EconomyService.GetCurrency() == currency + 50 && !DialoguePresenter.Instance.IsOpen, "Reward.NoDuplicateGrant");
            Check(JsonUtility.ToJson(CompanionRosterService.ExportSaveData()) == party && JsonUtility.ToJson(BeastCompanionService.ExportSaveData()) == beast, "Reward.PartyBeastPreserved");
            Check(GameSaveService.SaveCurrentWorldPosition(player.transform.position, Chapter2Main17Flow.Field), "Reward.SavedCompletion");
            SceneManager.LoadSceneAsync("Bootstrap"); for (int i = 0; i < 15; i++) yield return null;
            GameSessionData.Reset(); GameObject.Find("StartMenuCanvas/Slot01/Action").GetComponent<Button>().onClick.Invoke();
            for (int i = 0; i < 20; i++) yield return null;
            Check(SceneManager.GetActiveScene().name == Chapter2Main17Flow.Field && QuestService.GetState(quest.QuestId) == QuestState.Completed, "Reward.ContinueCompleted");
            Check(GameSessionData.Level == expected.Level && GameSessionData.CurrentExperience == expected.CurrentExperience && EconomyService.GetCurrency() == currency + 50, "Reward.ContinueNoGrantAgain");
            Check(JsonUtility.ToJson(CompanionRosterService.ExportSaveData()) == party && JsonUtility.ToJson(BeastCompanionService.ExportSaveData()) == beast, "Reward.ContinuePartyBeast");
            Check(BgmPlaybackService.Instance.CurrentClip == Resources.Load<BgmSceneCatalog>("Audio/Music/BgmSceneCatalog").Find(Chapter2Main17Flow.Field), "Reward.ContinueField08Bgm");
            SceneManager.LoadSceneAsync("Bootstrap"); for (int i = 0; i < 10; i++) yield return null;
        }
        static IEnumerator Run()
        {
            bool healingPolicy = SessionState.GetBool(Armed + ".Healing", false);
            if (!SessionState.GetBool(Armed + ".Low", false) && !healingPolicy) File.WriteAllText(Path.Combine(Root, "Main17_Closure_Runtime.txt"), "");
            float oldScale = Time.timeScale;
            try
            {
                // 현재 적 Lv12와 경험치 색상 경계의 안쪽·동일 레벨·녹색 경계를 대표 표본으로 사용합니다.
                foreach (string job in healingPolicy ? new[] { "healer" } : new[] { "guardian", "healer", "sharpshooter", "fighter", "mage" })
                foreach (int level in healingPolicy ? new[] { 4, 12 } : SessionState.GetBool(Armed + ".Low", false) ? new[] { 4 } : new[] { 9, 12, 16 })
                foreach (bool story in new[] { true, false })
                {
                    Time.timeScale = 6;
                    typeof(SecondRegressionAudit).GetMethod("Seed", StaticHidden).Invoke(null, new object[] { job, level });
                    CompanionRosterService.UnlockPaul(job); CompanionRosterService.UnlockSerin();
                    Check(CompanionRosterService.TrySetComposition(new[] { CompanionRosterService.SerinId, CompanionRosterService.MielId },
                        new Dictionary<string, FormationRow> { { "player", FormationRow.Front }, { CompanionRosterService.SerinId, FormationRow.Rear }, { CompanionRosterService.MielId, FormationRow.Rear } }), job + level + ".PartyFixture");
                    QuestService.ImportSaveData(new QuestProgressSaveData { CompletedQuestIds = QuestCatalog.All.Where(x => x.QuestId.StartsWith("main_") && string.CompareOrdinal(x.QuestId, "main_17") < 0).Select(x => x.QuestId).ToArray() });
                    InventoryService.TryAddItem("item_healing_potion_small", 3);
                    var spawn = Resources.Load<FieldMonsterSpawnDefinition>(story ? "StoryEncounterReturns/Main17_ThreatReturn" : "MonsterSpawns/Field08_Beetle01");
                    BattleEncounterContext.Set(spawn.Monster, spawn, null, null, Vector2.zero, spawn.Position);
                    // 실제 출발 Scene과 Spawn을 기존 Encounter Context에 기록합니다.
                    GameSessionData.RecordLocation(Chapter2Main17Flow.Field, "Spawn_From_Field07");

                    if (story) BattleEncounterContext.SetStoryEncounter(Chapter2Main17Flow.EncounterId, spawn.Monster, spawn, null, null, Vector2.zero, spawn.Position);
                    SceneManager.LoadSceneAsync("Battle");
                    double ready = EditorApplication.timeSinceStartup + 15;
                    while (SceneManager.GetActiveScene().name != "Battle" && EditorApplication.timeSinceStartup < ready) yield return null;
                    for (int i = 0; i < 8; i++) yield return null;
                    var c = UnityEngine.Object.FindAnyObjectByType<BattleSceneController>();
                    Check(c != null, job + level + "." + story + ".Controller");
                    var allies = Get<Formation>(c, "allies"); var enemies = Get<Formation>(c, "enemies");
                    Check(allies.Members.Count() == 3 && enemies.Members.Count() == 1 && enemies.Members.Single().MaxHp == spawn.Monster.MaxHp, job + level + "." + story + ".ActualFactory");
                    var last = allies.Members.ToDictionary(x => x, x => x.CurrentHp);
                    int damage = 0, healing = 0, allyActions = 0, enemyActions = 0, potions = 0;
                    Combatant observed = null; double began = EditorApplication.timeSinceStartup;
                    while (!Get<bool>(c, "battleEnded") && EditorApplication.timeSinceStartup - began < 70)
                    {
                        var actor = Get<Combatant>(c, "currentActor");
                        if (actor != null && actor != observed) { if (actor.Side == BattleSide.Allies) allyActions++; else enemyActions++; observed = actor; }
                        foreach (var a in allies.Members) { int change = a.CurrentHp - last[a]; if (change < 0) damage -= change; else healing += change; last[a] = a.CurrentHp; }
                        var button = Get<Button>(c, "attackButton");
                        if (!Get<bool>(c, "actionPlaying") && button.IsActive() && button.interactable)
                        {
                            var weak = allies.LivingMembers.OrderBy(x => (float)x.CurrentHp / x.MaxHp).First();
                            bool healer = Get<Dictionary<Combatant, JobDefinition>>(c, "combatantJobs")[actor].JobId == "healer";
                            if (healingPolicy && healer && weak.CurrentHp * 100 < weak.MaxHp * 65 && actor.CanSpendMp(6))
                            {
                                Get<Button>(c, "skillButton").onClick.Invoke();
                                Get<List<Button>>(c, "skillMenuButtons").Single(x => x.name == "Skill_healer_healing_light").onClick.Invoke();
                                Hit(c, weak);
                            }
                            else if (weak.CurrentHp * 100 < weak.MaxHp * 35 && InventoryService.GetItemCount("item_healing_potion_small") > 0)
                            {
                                Get<Button>(c, "itemButton").onClick.Invoke();
                                var potion = Get<List<Button>>(c, "itemMenuButtons").Single(x => x.name == "Item_item_healing_potion_small");
                                potion.onClick.Invoke(); Hit(c, weak); potions++;
                            }
                            else
                            {
                                button.onClick.Invoke();
                                if (Get<bool>(c, "choosingTarget")) Hit(c, Get<IReadOnlyList<Combatant>>(c, "selectableTargets").First());
                            }
                        }
                        yield return null;
                    }
                    foreach (var a in allies.Members) { int change = a.CurrentHp - last[a]; if (change < 0) damage -= change; else healing += change; }
                    bool win = enemies.IsDefeated;
                    rows.Add(string.Join(",", job, level, story ? "Story" : "General", win, allyActions, enemyActions, damage, healing, potions, allies.Members.Count(x => !x.IsAlive), (EditorApplication.timeSinceStartup - began).ToString("F2", System.Globalization.CultureInfo.InvariantCulture)));
                    File.WriteAllLines(Path.Combine(Root, healingPolicy ? "Main17_Healing_Matrix.csv" : "Main17_Balance_Matrix.csv"), rows);
                    Check(win && allyActions > 1 && enemyActions > 0, job + level + "." + story + ".VictoryNotInstant");
                    // 정상 승리 UI로 복귀한 뒤 다음 표본을 새 Combatant와 초기 자원으로 시작합니다.
                    UnityEngine.Object.FindObjectsByType<Button>().Single(x => x.name == "VictoryReturn").onClick.Invoke();
                    ready = EditorApplication.timeSinceStartup + 15;
                    while (SceneManager.GetActiveScene().name != Chapter2Main17Flow.Field && EditorApplication.timeSinceStartup < ready) yield return null;
                    for (int i = 0; i < 6; i++) yield return null;
                }
                SceneManager.LoadSceneAsync("Bootstrap");
                for (int i = 0; i < 10; i++) yield return null;
            }
            finally { Time.timeScale = oldScale; }
        }
    }
}

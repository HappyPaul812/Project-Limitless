using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ProjectLimitless.Core;
using ProjectLimitless.Monster;
using ProjectLimitless.Player;
using ProjectLimitless.World;
using ProjectLimitless.UI;
using ProjectLimitless.Battle;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectLimitless.EditorTools
{
    /// <summary>Main17의 영향만 기존 백그라운드 격리 실행기에 연결합니다. 이전 QA 결과 파일은 쓰지 않습니다.</summary>
    [InitializeOnLoad]
    public static class Main17RuntimeAudit
    {
        private const BindingFlags Hidden = BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;
        private const string Armed = "Limitless.Main17.Audit";
        public static readonly List<string> Results = new List<string>();
        private static bool story;
        private static bool battle;
        private static bool paths;
        private static bool saves;
        private static string Output => Path.GetFullPath(Path.Combine(Application.dataPath, "../../../문서/00_프로젝트/Main17_Runtime_Results.txt"));
        static Main17RuntimeAudit()
        {
            var initialized = Partial9FixedSpriteAudit.Status;
            EditorApplication.playModeStateChanged += state =>
            {
                if (!SessionState.GetBool(Armed, false)) return;
                if (state == PlayModeStateChange.EnteredPlayMode)
                    typeof(Partial9FixedSpriteAudit).GetField("routine", Hidden).SetValue(null, Flatten(paths ? RunPaths() : RunConnections()));
                if (state == PlayModeStateChange.EnteredEditMode) { SessionState.SetBool(Armed, false); Save(); }
            };
        }
        public static string LaunchConnections() { Results.Clear(); saves = false; paths = false; story = false; battle = false; SessionState.SetBool(Armed, true); return Partial9FixedSpriteAudit.Launch(); }
        public static string LaunchStory() { Results.Clear(); saves = false; paths = false; story = true; battle = false; SessionState.SetBool(Armed, true); return Partial9FixedSpriteAudit.Launch(); }
        public static string LaunchBattle() { Results.Clear(); saves = false; paths = false; story = true; battle = true; SessionState.SetBool(Armed, true); return Partial9FixedSpriteAudit.Launch(); }
        public static string LaunchPaths() { Results.Clear(); saves = false; paths = true; SessionState.SetBool(Armed, true); return Partial9FixedSpriteAudit.Launch(); }
        public static string LaunchSaves() { Results.Clear(); saves = true; paths = false; story = true; battle = true; SessionState.SetBool(Armed, true); return Partial9FixedSpriteAudit.Launch(); }
        private static T Value<T>(object owner, string name) => (T)owner.GetType().GetField(name, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(owner);
        private static void Save() => File.WriteAllLines(Output, Results);
        private static void Check(bool condition, string id)
        {
            Results.Add((condition ? "PASS|" : "FAIL|") + id); Save();
            if (!condition) throw new InvalidOperationException(id);
        }
        private static IEnumerator Flatten(IEnumerator first)
        {
            var stack = new Stack<IEnumerator>(); stack.Push(first);
            while (stack.Count > 0)
            {
                var top = stack.Peek();
                if (!top.MoveNext()) { stack.Pop(); continue; }
                if (top.Current is IEnumerator nested) stack.Push(nested); else yield return top.Current;
            }
        }
        private static IEnumerator WaitScene(string name)
        {
            double until = EditorApplication.timeSinceStartup + 30;
            while (SceneManager.GetActiveScene().name != name && EditorApplication.timeSinceStartup < until) yield return null;
            Check(SceneManager.GetActiveScene().name == name, "Scene." + name);
            yield return Wait(.7);
            MonsterEncounterService.SuppressForSeconds(3600);
        }
        private static IEnumerator Wait(double seconds)
        {
            double until = EditorApplication.timeSinceStartup + seconds;
            while (EditorApplication.timeSinceStartup < until) yield return null;
        }
        private static IEnumerator RunConnections()
        {
            // 기존 QA의 초기화만 재사용합니다. 이전817건의 결과를 쓰는 메서드는 호출하지 않습니다.
            // 기존 Main16 정상 전투 감사와 같은 Fighter20을 사용합니다. 게임의 성장 수치는 변경하지 않습니다.
            typeof(SecondRegressionAudit).GetMethod("Seed", Hidden).Invoke(null, new object[] { "fighter", 20 });
            if (saves) GameSessionData.SelectPlayerPath("path.hearing");
            GameSessionData.RecordLocation(Chapter2Main17Flow.PreviousField, "Spawn_From_Field06");
            SceneManager.LoadSceneAsync(Chapter2Main17Flow.PreviousField);
            yield return WaitScene(Chapter2Main17Flow.PreviousField);
            var gate = UnityEngine.Object.FindAnyObjectByType<Main17AccessGate>();
            Check(gate != null && gate.GetComponent<BoxCollider2D>().enabled, "Gate.LockedBeforeMain16");
            var quests = QuestCatalog.All.Where(value => value.QuestId.StartsWith("main_") &&
                string.CompareOrdinal(value.QuestId, "main_17") < 0).Select(value => value.QuestId).ToArray();
            QuestService.ImportSaveData(new QuestProgressSaveData { CompletedQuestIds = quests });
            // 완료 기록만 주입하면 이전 임시 동행 조건이 남습니다. 실제 Main15 완료의 정식 해금도 함께 준비합니다.
            CompanionRosterService.UnlockPaul("fighter");
            CompanionRosterService.UnlockSerin();
            if (saves) Check(CompanionRosterService.TrySetComposition(new[] { CompanionRosterService.SerinId, CompanionRosterService.MielId }, new Dictionary<string, FormationRow> { { "player", FormationRow.Front }, { CompanionRosterService.SerinId, FormationRow.Rear }, { CompanionRosterService.MielId, FormationRow.Rear } }), "Save.SerinSelectedFixture");
            yield return Wait(.2);
            Check(!gate.GetComponent<BoxCollider2D>().enabled, "Gate.OpenAfterMain16");
            Check(QuestService.GetState(Chapter2Main17Flow.QuestId) == QuestState.Available, "Main17.Available");
            var spawn07 = UnityEngine.Object.FindObjectsByType<SceneSpawnPoint>().FirstOrDefault(value => value.name == "Spawn_west_to_field08");
            Check(spawn07 != null && Vector2.Distance(spawn07.transform.position, new Vector2(-10.25f, 0)) > 2, "Field07.SpawnSeparated");
            Check(UnityEngine.Object.FindObjectsByType<SceneTransitionTrigger>().Count() == 2, "Field07.TwoTransitions");
            SceneTransitionService.Load(Chapter2Main17Flow.Field, "Spawn_From_Field07");
            yield return WaitScene(Chapter2Main17Flow.Field);
            var player = UnityEngine.Object.FindAnyObjectByType<PlayerController>();
            Check(player != null && Vector2.Distance(player.transform.position, new Vector2(7, 0)) < .2f, "Field08.SpawnRestored");
            var bounds = UnityEngine.Object.FindAnyObjectByType<WorldBounds2D>();
            Check(bounds != null && bounds.Bounds.size.x == 21 && bounds.Bounds.size.y == 15, "Field08.Bounds21x15");
            Check(UnityEngine.Object.FindObjectsByType<SceneTransitionTrigger>().Count() == 1, "Field08.EastOnlyTransition");
            Check(UnityEngine.Object.FindObjectsByType<BoxCollider2D>().Any(value => value.name.StartsWith("Boundary_Left")), "Field08.DeepWestClosed");
            Check(Camera.main != null, "Field08.Camera");
            Check(SceneManager.GetActiveScene().name == Chapter2Main17Flow.Field, "Field08.NoImmediateReturn");
            if (saves) yield return Continue("entry");
            if (story)
            {
                string roster = JsonUtility.ToJson(CompanionRosterService.ExportSaveData());
                Check(QuestService.ActiveMainQuest?.Definition.QuestId == Chapter2Main17Flow.QuestId && Chapter2Main17Flow.IsCurrent(1), "Story.StartAndEntry");
                var actor = GameObject.Find("Serin_Story_Main17");
                Check(actor != null && actor.GetComponent<Animator>()?.runtimeAnimatorController == Resources.Load<RuntimeAnimatorController>("Chapter2/Serin"), "Story.SerinOfficialAnimator");
                for (int index = 1; index <= 6; index++)
                {
                    player = UnityEngine.Object.FindAnyObjectByType<PlayerController>();
                    player.transform.position = Chapter2Main17Flow.Positions[index]; Physics2D.SyncTransforms();
                    var site = GameObject.Find("field08_main17_" + Chapter2Main17Flow.Sites[index]).GetComponent<Main17Site>();
                    site.TryInteract();
                    Check(DialoguePresenter.Instance.IsOpen, "Story.Open." + index);
                    Check(Chapter2Main17Flow.IsCurrent(index), "Story.NoEarlyProgress." + index);
                    int guard = 0;
                    while (DialoguePresenter.Instance.IsOpen && guard++ < 60) { DialoguePresenter.Instance.Advance(); yield return Wait(.03); }
                    Check(Chapter2Main17Flow.IsCurrent(index + 1), "Story.Complete." + index);
                    site.TryInteract(); Check(Chapter2Main17Flow.IsCurrent(index + 1), "Story.NoDuplicate." + index);
                }
                Check(JsonUtility.ToJson(CompanionRosterService.ExportSaveData()) == roster, "Story.RosterFormationPreserved");
                Check(Chapter2Main17Flow.IsCurrent(7), "Story.WitnessReady");
                if (saves) yield return Continue("before-witness");
                if (battle)
                {
                    yield return Inspect(7);
                    yield return WaitScene("Battle");
                    if (saves)
                    {
                        // 도망의 공용 복귀 경로만 제어합니다. 실제 버튼 확률/물리 입력 테스트라고 주장하지 않습니다.
                        BattleSceneFlow.ReturnToField(false); yield return WaitScene(Chapter2Main17Flow.Field);
                        Check(Chapter2Main17Flow.IsCurrent(8), "Save.NoWinKeptObjective");
                        yield return Continue("retry-before-victory");
                        Check(SceneManager.GetActiveScene().name == Chapter2Main17Flow.Field, "Save.NoAutomaticRebattle");
                        var retryPlayer = UnityEngine.Object.FindAnyObjectByType<PlayerController>();
                        retryPlayer.transform.position = Chapter2Main17Flow.Positions[7]; Physics2D.SyncTransforms();
                        GameObject.Find("field08_main17_witness").GetComponent<Main17Site>().TryInteract(); yield return WaitScene("Battle");
                    }
                    Check(BattleEncounterContext.StableEncounterId == Chapter2Main17Flow.EncounterId, "Battle.StableStoryId");
                    Check(Chapter2Main17Flow.IsCurrent(8), "Battle.WitnessSeparateFromVictory");
                    var controller = UnityEngine.Object.FindAnyObjectByType<BattleSceneController>();
                    Check(Value<Formation>(controller, "enemies").Members.Count() == 1, "Battle.ExistingSingleLizard");
                    Check(Value<Formation>(controller, "allies").Members.Count() == 1 + CompanionRosterService.ActivePartyCharacterIds.Count && Value<Formation>(controller, "allies").Members.Count() <= 3, "Battle.ExactSavedParty");
                    int attacks = 0; double until = EditorApplication.timeSinceStartup + 180;
                    while (!Value<bool>(controller, "battleEnded") && EditorApplication.timeSinceStartup < until)
                    {
                        var attack = Value<Button>(controller, "attackButton");
                        if (!Value<bool>(controller, "actionPlaying") && attack.IsActive() && attack.interactable)
                        {
                            attack.onClick.Invoke();
                            var targets = Value<IReadOnlyList<Combatant>>(controller, "selectableTargets");
                            if (Value<bool>(controller, "choosingTarget") && targets.Count > 0)
                            { typeof(SecondRegressionAudit).GetMethod("Hit", Hidden).Invoke(null, new object[] { controller, targets[0] }); attacks++; }
                        }
                        yield return null;
                    }
                    Results.Add("INFO|Battle.Attacks=" + attacks + "; enemyHP=" + string.Join(",", Value<Formation>(controller, "enemies").Members.Select(value => value.CurrentHp)) + "; allyHP=" + string.Join(",", Value<Formation>(controller, "allies").Members.Select(value => value.CurrentHp))); Save();
                    Check(Value<Formation>(controller, "enemies").IsDefeated && attacks > 0, "Battle.NormalAttackVictory");
                    Check(Chapter2Main17Flow.IsCurrent(9), "Battle.OnlyVictoryAdvanced");
                    UnityEngine.Object.FindObjectsByType<Button>().First(value => value.name == "VictoryReturn").onClick.Invoke();
                    yield return WaitScene(Chapter2Main17Flow.Field);
                    Check(JsonUtility.ToJson(CompanionRosterService.ExportSaveData()) == roster, "Battle.PartyFormationPreserved");
                    if (saves) yield return Continue("after-victory");
                    for (int index = 9; index <= 11; index++) yield return Inspect(index);
                    Check(QuestService.GetState(Chapter2Main17Flow.QuestId) == QuestState.Completed, "Story.Main17Completed");
                    if (saves) yield return Continue("completed");
                }
            }
            SceneTransitionService.Load(Chapter2Main17Flow.PreviousField, "Spawn_From_Field08");
            yield return WaitScene(Chapter2Main17Flow.PreviousField);
            player = UnityEngine.Object.FindAnyObjectByType<PlayerController>();
            Check(player != null && Vector2.Distance(player.transform.position, new Vector2(-7, 0)) < .2f, "Field07.ReturnSpawnRestored");
            yield return Wait(1);
            Check(SceneManager.GetActiveScene().name == Chapter2Main17Flow.PreviousField, "Field07.NoImmediateReturn");
        }
        private static IEnumerator Inspect(int index)
        {
            var player = UnityEngine.Object.FindAnyObjectByType<PlayerController>();
            player.transform.position = Chapter2Main17Flow.Positions[index]; Physics2D.SyncTransforms();
            var site = GameObject.Find("field08_main17_" + Chapter2Main17Flow.Sites[index]).GetComponent<Main17Site>();
            site.TryInteract(); Check(DialoguePresenter.Instance.IsOpen, "Story.Open." + index);
            int guard = 0;
            while (DialoguePresenter.Instance != null && DialoguePresenter.Instance.IsOpen && guard++ < 60)
            { DialoguePresenter.Instance.Advance(); yield return Wait(.03); }
            Check(index == 11 ? QuestService.GetState(Chapter2Main17Flow.QuestId) == QuestState.Completed : Chapter2Main17Flow.IsCurrent(index + 1), "Story.Complete." + index);
        }

        private static IEnumerator RunPaths()
        {
            foreach (string path in new[] { "path.hearing", "path.vision", "path.intellectual", "path.mobility", "path.emotional-scar" })
            {
                typeof(SecondRegressionAudit).GetMethod("Seed", Hidden).Invoke(null, new object[] { "fighter", 20 });
                GameSessionData.SelectPlayerPath(path);
                QuestService.ImportSaveData(new QuestProgressSaveData { CompletedQuestIds = QuestCatalog.All.Where(value => value.QuestId.StartsWith("main_") && string.CompareOrdinal(value.QuestId, "main_17") < 0).Select(value => value.QuestId).ToArray() });
                CompanionRosterService.UnlockPaul("fighter"); CompanionRosterService.UnlockSerin();
                string roster = JsonUtility.ToJson(CompanionRosterService.ExportSaveData());
                GameSessionData.RecordLocation(Chapter2Main17Flow.Field, "Spawn_From_Field07");
                SceneManager.LoadSceneAsync(Chapter2Main17Flow.Field); yield return WaitScene(Chapter2Main17Flow.Field);
                for (int index = 1; index <= 6; index++)
                {
                    var player = UnityEngine.Object.FindAnyObjectByType<PlayerController>();
                    player.transform.position = Chapter2Main17Flow.Positions[index]; Physics2D.SyncTransforms();
                    var site = GameObject.Find("field08_main17_" + Chapter2Main17Flow.Sites[index]).GetComponent<Main17Site>();
                    site.TryInteract(); Check(DialoguePresenter.Instance.IsOpen, path + ".Open." + index);
                    var lines = Value<DialogueLine[]>(DialoguePresenter.Instance, "sequenceLines");
                    Check(lines.Select(value => value.DialogueId).SequenceEqual(Main17DialogueCatalog.Get(index).Select(value => value.DialogueId)), path + ".ExactIDs." + index);
                    if (index == 2 || index == 6)
                        Check(lines.First(value => value.SpeakerId != "").SpeakerId == (path == "path.hearing" ? "player" : CompanionRosterService.SerinId), path + ".PlayerPriority." + index);
                    if (index == 2)
                    {
                        DialoguePresenter.Instance.Hide(); yield return Wait(.1);
                        Check(Chapter2Main17Flow.IsCurrent(index), path + ".CancelNoProgress");
                        site.TryInteract();
                    }
                    int guard = 0;
                    while (DialoguePresenter.Instance.IsOpen && guard++ < 60) { DialoguePresenter.Instance.Advance(); yield return Wait(.03); }
                    Check(Chapter2Main17Flow.IsCurrent(index + 1), path + ".SameResult." + index);
                }
                Check(GameObject.FindObjectsByType<Chapter2Main17Flow>().Count() == 1 && GameObject.Find("Serin_Story_Main17") != null, path + ".SingleActorFlow");
                Check(JsonUtility.ToJson(CompanionRosterService.ExportSaveData()) == roster, path + ".PartyPreserved");
            }
        }
        private static IEnumerator Continue(string label)
        {
            string scene = SceneManager.GetActiveScene().name;
            var player = UnityEngine.Object.FindAnyObjectByType<PlayerController>(); Vector2 at = player.transform.position;
            string quest = JsonUtility.ToJson(QuestService.ExportSaveData()), party = JsonUtility.ToJson(CompanionRosterService.ExportSaveData()), beast = JsonUtility.ToJson(BeastCompanionService.ExportSaveData());
            string path = GameSessionData.SelectedPlayerPathId;
            Check(GameSaveService.SaveCurrentWorldPosition(at, scene), "Save." + label + ".WriteIsolated");
            SceneManager.LoadSceneAsync("Bootstrap"); yield return WaitScene("Bootstrap"); GameSessionData.Reset();
            var action = GameObject.Find("StartMenuCanvas/Slot01/Action")?.GetComponent<Button>();
            Check(action != null, "Save." + label + ".ContinueButton"); action.onClick.Invoke(); yield return WaitScene(scene);
            player = UnityEngine.Object.FindAnyObjectByType<PlayerController>();
            Check(player != null && Vector2.Distance(player.transform.position, at) < .05f, "Save." + label + ".ScenePosition");
            Check(JsonUtility.ToJson(QuestService.ExportSaveData()) == quest, "Save." + label + ".QuestExact");
            Check(JsonUtility.ToJson(CompanionRosterService.ExportSaveData()) == party, "Save." + label + ".PartyFormationExact");
            Check(JsonUtility.ToJson(BeastCompanionService.ExportSaveData()) == beast, "Save." + label + ".BeastExact");
            Check(GameSessionData.SelectedPlayerPathId == path && CompanionRosterService.IsUnlocked(CompanionRosterService.SerinId), "Save." + label + ".PathSerinPermanent");
            Check(UnityEngine.Object.FindObjectsByType<Chapter2Main17Flow>().Count() == 1 && UnityEngine.Object.FindObjectsByType<Main17Site>().Count() == 11, "Save." + label + ".DerivedSitesOnce");
        }
    }
}

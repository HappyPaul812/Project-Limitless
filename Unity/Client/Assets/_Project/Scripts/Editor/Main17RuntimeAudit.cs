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
        private static string Output => Path.GetFullPath(Path.Combine(Application.dataPath, "../../../문서/00_프로젝트/Main17_Runtime_Results.txt"));
        static Main17RuntimeAudit()
        {
            var initialized = Partial9FixedSpriteAudit.Status;
            EditorApplication.playModeStateChanged += state =>
            {
                if (!SessionState.GetBool(Armed, false)) return;
                if (state == PlayModeStateChange.EnteredPlayMode)
                    typeof(Partial9FixedSpriteAudit).GetField("routine", Hidden).SetValue(null, Flatten(RunConnections()));
                if (state == PlayModeStateChange.EnteredEditMode) { SessionState.SetBool(Armed, false); Save(); }
            };
        }
        public static string LaunchConnections() { Results.Clear(); story = false; SessionState.SetBool(Armed, true); return Partial9FixedSpriteAudit.Launch(); }
        public static string LaunchStory() { Results.Clear(); story = true; SessionState.SetBool(Armed, true); return Partial9FixedSpriteAudit.Launch(); }
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
            typeof(SecondRegressionAudit).GetMethod("Seed", Hidden).Invoke(null, new object[] { "guardian", 15 });
            GameSessionData.RecordLocation(Chapter2Main17Flow.PreviousField, "Spawn_From_Field06");
            SceneManager.LoadSceneAsync(Chapter2Main17Flow.PreviousField);
            yield return WaitScene(Chapter2Main17Flow.PreviousField);
            var gate = UnityEngine.Object.FindAnyObjectByType<Main17AccessGate>();
            Check(gate != null && gate.GetComponent<BoxCollider2D>().enabled, "Gate.LockedBeforeMain16");
            var quests = QuestCatalog.All.Where(value => value.QuestId.StartsWith("main_") &&
                string.CompareOrdinal(value.QuestId, "main_17") < 0).Select(value => value.QuestId).ToArray();
            QuestService.ImportSaveData(new QuestProgressSaveData { CompletedQuestIds = quests });
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
            if (story)
            {
                string roster = JsonUtility.ToJson(CompanionRosterService.ExportSaveData());
                Check(QuestService.ActiveMainQuest?.Definition.QuestId == Chapter2Main17Flow.QuestId && Chapter2Main17Flow.IsCurrent(1), "Story.StartAndEntry");
                var actor = GameObject.Find("Serin_Story_Main17");
                Check(actor != null && actor.GetComponent<Animator>()?.runtimeAnimatorController == Resources.Load<RuntimeAnimatorController>("Chapter2/Serin"), "Story.SerinOfficialAnimator");
                for (int index = 1; index <= 6; index++)
                {
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
            }
            SceneTransitionService.Load(Chapter2Main17Flow.PreviousField, "Spawn_From_Field08");
            yield return WaitScene(Chapter2Main17Flow.PreviousField);
            player = UnityEngine.Object.FindAnyObjectByType<PlayerController>();
            Check(player != null && Vector2.Distance(player.transform.position, new Vector2(-7, 0)) < .2f, "Field07.ReturnSpawnRestored");
            yield return Wait(1);
            Check(SceneManager.GetActiveScene().name == Chapter2Main17Flow.PreviousField, "Field07.NoImmediateReturn");
        }
    }
}

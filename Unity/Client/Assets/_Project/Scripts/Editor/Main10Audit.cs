#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProjectLimitless.Battle;
using ProjectLimitless.Core;
using ProjectLimitless.Monster;
using ProjectLimitless.Player;
using ProjectLimitless.UI;
using ProjectLimitless.World;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace ProjectLimitless.Editor
{
    /// <summary>사용자 저장 슬롯과 분리해 Main09 완료부터 Main10 입구 발견까지 실제 Scene으로 검사합니다.</summary>
    [InitializeOnLoad]
    public static class Main10Audit
    {
        public static readonly List<string> Results = new List<string>();
        public static string Status { get; private set; } = "Idle";
        private static IEnumerator routine;
        private static double nextTick;

        static Main10Audit()
        {
            EditorApplication.update += Tick;
            EditorApplication.playModeStateChanged += state =>
            {
                if (state == PlayModeStateChange.EnteredEditMode) GameSaveService.FinishAudit();
            };
        }

        public static void Start()
        {
            if (!EditorApplication.isPlaying) throw new InvalidOperationException("Play Mode에서 실행해주세요.");
            Results.Clear(); Status = "Running";
            Application.runInBackground = true;
            GameSaveService.AuditSaveDirectory = System.IO.Path.Combine(Application.dataPath, "../UserData/Main10Audit", Guid.NewGuid().ToString("N"));
            GameSaveService.SelectSlot(1);
            routine = Run();
        }

        private static void Tick()
        {
            if (routine == null || EditorApplication.timeSinceStartup < nextTick) return;
            nextTick = EditorApplication.timeSinceStartup + .1;
            try { if (!routine.MoveNext()) { routine = null; Status = "PASS"; } }
            catch (Exception error) { Results.Add("FAIL " + error); routine = null; Status = "FAIL"; }
        }

        private static void Check(bool condition, string label)
        { if (!condition) throw new InvalidOperationException(label); Results.Add("PASS " + label); }

        private static IEnumerator WaitScene(string name)
        {
            int ticks = 0;
            while (SceneManager.GetActiveScene().name != name && ticks++ < 250) yield return null;
            if (SceneManager.GetActiveScene().name != name) throw new TimeoutException(name);
            for (int i = 0; i < 8; i++) yield return null;
        }

        private static void Seed()
        {
            GameSessionData.Reset();
            GameSessionData.ConfigurePlayer(PlayerVisualType.Male, "감사 플레이어");
            GameSessionData.SelectPlayerPath(PathCombatTraitRuntime.MobilityPathId);
            GameSessionData.SelectJob("guardian");
            GameSessionData.ConfigureProgress(12, 0);
            CompanionRosterService.Reset(); CompanionRosterService.UnlockIntroCompanions(); CompanionRosterService.UnlockPaul("guardian");
            QuestService.ImportSaveData(new QuestProgressSaveData
            { CompletedQuestIds = QuestCatalog.All.Where(x => x.QuestId != MainQuest10FieldFlow.QuestId).Select(x => x.QuestId).ToArray() });
            GameSessionData.RecordLocation("Field_03", "Spawn_From_Field02");
            GameSessionData.SetPendingSpawnPoint("Spawn_From_Field02");
        }

        private static IEnumerator RoundTrip(string label)
        {
            string quest = JsonUtility.ToJson(QuestService.ExportSaveData());
            string roster = JsonUtility.ToJson(CompanionRosterService.ExportSaveData());
            int currency = EconomyService.GetCurrency(), exp = GameSessionData.CurrentExperience;
            var player = Object.FindAnyObjectByType<PlayerController>();
            Check(GameSaveService.SaveCurrentWorldPosition(player.transform.position, "Field_03"), label + " Save");
            SceneManager.LoadSceneAsync("Bootstrap"); var wait = WaitScene("Bootstrap"); while (wait.MoveNext()) yield return null;
            var action = GameObject.Find("StartMenuCanvas/Slot01/Action")?.GetComponent<Button>();
            Check(action != null, label + " Bootstrap");
            action.onClick.Invoke();
            wait = WaitScene("Field_03"); while (wait.MoveNext()) yield return null;
            Check(JsonUtility.ToJson(QuestService.ExportSaveData()) == quest, label + " Quest/Object 복원");
            Check(JsonUtility.ToJson(CompanionRosterService.ExportSaveData()) == roster
                && EconomyService.GetCurrency() == currency && GameSessionData.CurrentExperience == exp,
                label + " 동료·보상 복원");
            bool active = QuestService.ActiveMainQuest?.Definition.QuestId == MainQuest10FieldFlow.QuestId;
            Check(Object.FindAnyObjectByType<QuestNavigationPresenter>().HasTarget == active, label + " Navigation 복원");
        }

        private static IEnumerator Run()
        {
            Seed(); MonsterEncounterService.SuppressForSeconds(3600);
            SceneManager.LoadSceneAsync("Field_03"); var wait = WaitScene("Field_03"); while (wait.MoveNext()) yield return null;
            Check(QuestService.GetState(MainQuest09FieldFlow.QuestId) == QuestState.Completed
                && QuestService.ActiveMainQuest?.Definition.QuestId == MainQuest10FieldFlow.QuestId, "Main09 완료→Main10 시작");
            var structure = GameObject.Find("MainQuest09FieldFlow/" + MainQuest09FieldFlow.StructureId);
            Check(structure != null && structure.activeInHierarchy && structure.GetComponents<MainQuest10Interactable>().Length == 1,
                "Main09 석재 동일 오브젝트 재사용");
            Check(Object.FindObjectsByType<MainQuest09Interactable>().Count(x => x.gameObject.activeInHierarchy) == 1,
                "Main09의 다른 대상 비활성");
            var flow = Object.FindAnyObjectByType<MainQuest10FieldFlow>();
            Check(flow != null && !flow.transform.Find(MainQuest10FieldFlow.EntranceId).gameObject.activeSelf,
                "Main10 전 입구 숨김");
            QuestLogPresenter.Instance.Open();
            Check(QuestLogPresenter.Instance.IsOpen, "Quest Log Main10 열림");
            QuestLogPresenter.Instance.Close();
            wait = RoundTrip("A 시작 직후"); while (wait.MoveNext()) yield return null;
            int currency = EconomyService.GetCurrency(), exp = GameSessionData.CurrentExperience;
            string[] ids = { MainQuest09FieldFlow.StructureId, MainQuest10FieldFlow.StoneworkId,
                MainQuest10FieldFlow.SilenceId, MainQuest10FieldFlow.DescentId,
                MainQuest10FieldFlow.EntranceId, MainQuest10FieldFlow.EntranceId, MainQuest10FieldFlow.EntranceId };
            for (int step = 0; step < ids.Length; step++)
            {
                Check(QuestService.ActiveMainQuest.CurrentObjectiveIndex == step, "Main10 순차 목표 " + step);
                string targetId = QuestService.ActiveMainQuest.CurrentObjective.TargetId;
                Check(targetId == ids[step] && QuestNavigationTargetRegistry.TryGet(targetId, out _), "목표와 Navigation 위치 " + step);
                var actor = Object.FindObjectsByType<MainQuest10Interactable>().First(x => x.TargetId == targetId && x.gameObject.activeInHierarchy);
                var player = Object.FindAnyObjectByType<PlayerController>();
                player.transform.position = actor.transform.position + Vector3.down * .65f;
                var body = player.GetComponent<Rigidbody2D>();
                if (body != null) { body.position = player.transform.position; body.linearVelocity = Vector2.zero; }
                yield return null;
                Check(actor.TryInteract() && DialoguePresenter.Instance.IsOpen, "실제 거리·대화 " + step);
                if (step == 0)
                {
                    DialoguePresenter.Instance.Hide();
                    Check(QuestService.ActiveMainQuest.CurrentObjectiveIndex == 0, "대화 취소 진행 없음");
                    Check(actor.TryInteract(), "취소 후 재대화");
                }
                for (int page = 0; page < MainQuest10Dialogue.Lines(step).Length; page++)
                { DialoguePresenter.Instance.Advance(); yield return null; }
                Check(!DialoguePresenter.Instance.IsOpen, "대화 완료 " + step);
                if (step == 1) { wait = RoundTrip("B 가공된 돌 후"); while (wait.MoveNext()) yield return null; }
                if (step == 4)
                {
                    Check(GameObject.Find("MainQuest10FieldFlow/" + MainQuest10FieldFlow.EntranceId) != null,
                        "입구 공개");
                    wait = RoundTrip("C 입구 발견 후"); while (wait.MoveNext()) yield return null;
                }
            }
            Check(QuestService.GetState(MainQuest10FieldFlow.QuestId) == QuestState.Completed, "Main10 완료");
            Check(EconomyService.GetCurrency() == currency + 40 && GameSessionData.CurrentExperience == exp + 40,
                "EXP40·탈렌트40 1회 지급");
            QuestService.NotifyInteraction(MainQuest10FieldFlow.EntranceId);
            Check(EconomyService.GetCurrency() == currency + 40 && GameSessionData.CurrentExperience == exp + 40,
                "중복 신호 보상 없음");
            var entrance = Object.FindObjectsByType<MainQuest10Interactable>().First(x => x.TargetId == MainQuest10FieldFlow.EntranceId);
            Check(entrance.gameObject.activeInHierarchy && entrance.TryInteract() && DialoguePresenter.Instance.IsOpen,
                "미구현 Dungeon 진입 안전 Hook");
            DialoguePresenter.Instance.Hide();
            wait = RoundTrip("D 완료 후"); while (wait.MoveNext()) yield return null;
            Check(QuestService.GetState(MainQuest10FieldFlow.QuestId) == QuestState.Completed
                && GameObject.Find("MainQuest10FieldFlow/" + MainQuest10FieldFlow.EntranceId) != null,
                "Continue 뒤 입구 유지");
            Results.Add("SAVE_DIRECTORY " + GameSaveService.AuditSaveDirectory);
        }
    }
}
#endif

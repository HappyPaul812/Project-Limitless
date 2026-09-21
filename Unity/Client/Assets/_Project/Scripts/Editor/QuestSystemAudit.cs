using System;
using System.Linq;
using ProjectLimitless.Core;
using ProjectLimitless.Player;
using ProjectLimitless.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectLimitless.Editor
{
    /// <summary>실제 스토리 데이터를 만들지 않고 개발용 메모리 데이터로 퀘스트 기반을 검증합니다.</summary>
    [InitializeOnLoad]
    public static class QuestSystemAudit
    {
        private const string BatchKey = "ProjectLimitless.QuestSystemAudit";

        static QuestSystemAudit() => EditorApplication.playModeStateChanged += OnPlayModeChanged;

        private static void Audit()
        {
            QuestDefinition main1 = MakeQuest("dev_main_01", "개발 메인 1", QuestType.Main, null, "dev_main_02", 7,
                Step("talk", "테스트 NPC와 대화", QuestObjectiveType.TalkToNpc, "dev_npc"),
                Step("reach", "테스트 장소 도착", QuestObjectiveType.ReachLocation, "dev_location"),
                Step("win", "테스트 전투 승리", QuestObjectiveType.DefeatEncounter, "dev_encounter"));
            QuestDefinition main2 = MakeQuest("dev_main_02", "개발 메인 2", QuestType.Main, new[] { "dev_main_01" }, "", 0,
                Step("signal", "테스트 신호", QuestObjectiveType.GenericSignal, "dev_finish"));
            QuestDefinition side1 = MakeQuest("dev_side_01", "개발 서브 1", QuestType.Side, null, "", 0,
                Step("interact", "테스트 상호작용", QuestObjectiveType.Interact, "dev_interaction"));
            QuestDefinition side2 = MakeQuest("dev_side_02", "개발 서브 2", QuestType.Side, null, "", 0,
                Step("signal", "테스트 신호", QuestObjectiveType.GenericSignal, "dev_side_signal"));
            Register(main1, main2, side1, side2);

            GameSessionData.Reset();
            Check(QuestService.GetState("dev_main_01") == QuestState.Available, "첫 메인 Available");
            Check(QuestService.GetState("dev_main_02") == QuestState.Locked, "다음 메인 Locked");
            Check(QuestService.TryStart("dev_main_01"), "메인 시작");
            Check(!QuestService.TryStart("dev_main_02"), "선행 미완료 메인 시작 방지");
            Check(QuestService.GetTrackedQuest()?.CurrentObjective?.ObjectiveId == "talk", "첫 Objective 표시");
            QuestService.NotifyNpcTalked("wrong_id");
            Check(QuestService.GetTrackedQuest()?.CurrentObjective?.ObjectiveId == "talk", "잘못된 ID 무시");
            QuestService.NotifyNpcTalked("dev_npc");
            Check(QuestService.GetTrackedQuest()?.CurrentObjective?.ObjectiveId == "reach", "두 번째 Objective 이동");

            Check(QuestService.TryStart("dev_side_01") && QuestService.TryStart("dev_side_02")
                && QuestService.ActiveSideQuests.Count() == 2, "서브 두 개 동시 Active");
            QuestProgressSaveData midProgress = QuestService.ExportSaveData();
            string json = JsonUtility.ToJson(midProgress);
            QuestService.Reset();
            QuestService.ImportSaveData(JsonUtility.FromJson<QuestProgressSaveData>(json));
            Check(QuestService.ActiveMainQuest?.CurrentObjective?.ObjectiveId == "reach"
                && QuestService.ActiveSideQuests.Count() == 2, "Active/Objectives 저장 복원");

            EconomyService.Reset(); GameSessionData.ConfigureProgress(1, 0);
            QuestService.NotifyLocationReached("dev_location");
            QuestService.NotifyEncounterWon("dev_encounter");
            Check(QuestService.GetState("dev_main_01") == QuestState.Completed && EconomyService.GetCurrency() == 7,
                "완료와 RewardBundle 1회 지급");
            QuestService.NotifyEncounterWon("dev_encounter");
            Check(EconomyService.GetCurrency() == 7, "완료 Signal 반복 보상 방지");
            Check(QuestService.GetState("dev_main_02") == QuestState.Available && QuestService.TryStart("dev_main_02"),
                "다음 메인 해금과 시작");

            QuestProgressSaveData completedProgress = QuestService.ExportSaveData();
            QuestService.Reset(); QuestService.ImportSaveData(completedProgress);
            Check(QuestService.GetState("dev_main_01") == QuestState.Completed
                && QuestService.ActiveMainQuest?.Definition.QuestId == "dev_main_02", "Completed/Main 복원");
            GameSaveData legacy = JsonUtility.FromJson<GameSaveData>("{\"Version\":1,\"Level\":4}");
            QuestService.ImportSaveData(legacy.QuestProgress);
            Check(!QuestService.ActiveQuests.Any() && !QuestService.CompletedQuestIds.Any(), "구버전 Quest 필드 없음 호환");

            QuestService.ImportSaveData(midProgress);
            GameObject canvasObject = new GameObject("QuestAuditCanvas", typeof(RectTransform), typeof(Canvas));
            QuestHudPresenter hud = QuestHudPresenter.EnsureOn(canvasObject);
            hud.Refresh();
            Text text = canvasObject.transform.Find("QuestHud/QuestText").GetComponent<Text>();
            Check(!hud.IsToastActive && canvasObject.transform.Find("QuestHud").GetComponent<CanvasGroup>().alpha == 0f,
                "월드 HUD 상시 Quest 표시 제거");

            GameObject questLogObject = new GameObject("QuestLogAudit");
            QuestLogPresenter questLog = questLogObject.AddComponent<QuestLogPresenter>();
            questLog.Open();
            Check(questLog.IsOpen && questLog.VisibleQuestCount == 3, "Quest Log Main 1/Side 2 동적 목록");
            Check(questLog.SelectedQuestId == "dev_main_01" && questLog.Details.Contains("[메인 퀘스트]")
                && questLog.Details.Contains("테스트 장소 도착"), "Active Main 기본 선택과 상세 목표");
            Text[] logTexts = questLogObject.GetComponentsInChildren<Text>(true);
            Check(logTexts.Any(x => x.text == "[메인 퀘스트]") && logTexts.Any(x => x.text == "[서브 퀘스트]"), "Main/Side Section 표시");
            Check(PlayerController.IsMovementLocked && WorldModalState.IsOwnedBy(questLog), "Quest Log Modal과 이동 잠금");
            object modalOwner = new object(); WorldModalState.TryAcquire(modalOwner);
            Check(!WorldModalState.IsOwnedBy(modalOwner), "다른 Modal 동시 획득 방지");
            questLog.Close();
            Check(!questLog.IsOpen && !PlayerController.IsMovementLocked && !WorldModalState.IsOpen, "Quest Log 닫기 상태 복구");

            QuestService.Reset();
            questLog.Open();
            Check(questLog.VisibleQuestCount == 0 && questLog.Details.Contains("진행 중인 퀘스트가 없습니다."), "Quest 0개 안전 표시");
            questLog.Close();

            QuestDefinition toastQuest = MakeQuest("dev_toast", "알림 검증", QuestType.Side, null, "", 0,
                Step("toast_a", "첫 알림 목표", QuestObjectiveType.GenericSignal, "toast_a"),
                Step("toast_b", "두 번째 알림 목표", QuestObjectiveType.GenericSignal, "toast_b"));
            QuestCatalog.RegisterForAudit(toastQuest);
            Check(QuestService.TryStart(toastQuest.QuestId) && hud.IsToastActive
                && text.text.Contains("[서브 퀘스트 시작]"), "Quest 시작 Toast");
            QuestService.NotifySignal("toast_a");
            Check(text.text.Contains("[목표 갱신]") && text.text.Contains("두 번째 알림 목표"), "Objective 변경 Toast");
            QuestService.NotifySignal("toast_b");
            Check(text.text.Contains("[서브 퀘스트 완료]") && text.text.Contains("알림 검증"), "Quest 완료 Toast");
            typeof(QuestHudPresenter).GetField("hideAtRealtime", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                ?.SetValue(hud, 0d);
            hud.SendMessage("Update");
            Check(!hud.IsToastActive && hud.GetComponent<CanvasGroup>().alpha == 0f, "Quest Toast 자동 숨김");

            UnityEngine.Object.DestroyImmediate(questLogObject);
            UnityEngine.Object.DestroyImmediate(canvasObject);

            QuestService.Reset();
            foreach (QuestDefinition quest in new[] { main1, main2, side1, side2 }) UnityEngine.Object.DestroyImmediate(quest);
            UnityEngine.Object.DestroyImmediate(toastQuest);
            QuestCatalog.ReloadForAudit(); EconomyService.Reset(); InventoryService.Reset();
        }

        [MenuItem("Project Limitless/Test/Quest System Audit")]
        public static void RunBatch()
        {
            try
            {
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
                SessionState.SetBool(BatchKey, true);
                EditorApplication.EnterPlaymode();
            }
            catch (Exception exception) { Debug.LogException(exception); EditorApplication.Exit(1); }
        }

        private static void OnPlayModeChanged(PlayModeStateChange state)
        {
            if (!SessionState.GetBool(BatchKey, false)) return;
            try
            {
                if (state == PlayModeStateChange.EnteredPlayMode)
                {
                    Audit();
                    Debug.Log("QUEST_SYSTEM_AUDIT Play Mode PASS");
                    EditorApplication.ExitPlaymode();
                }
                else if (state == PlayModeStateChange.EnteredEditMode)
                {
                    SessionState.EraseBool(BatchKey);
                    Debug.Log("QUEST_SYSTEM_AUDIT ALL PASS");
                    if (Application.isBatchMode) EditorApplication.Exit(0);
                }
            }
            catch (Exception exception)
            {
                SessionState.EraseBool(BatchKey);
                Debug.LogException(exception);
                if (Application.isBatchMode) EditorApplication.Exit(1);
            }
        }

        private static QuestObjectiveDefinition Step(string id, string description, QuestObjectiveType type, string target)
        { var step = new QuestObjectiveDefinition(); step.ConfigureForAudit(id, description, type, target); return step; }

        private static QuestDefinition MakeQuest(string id, string title, QuestType type, string[] prerequisites,
            string nextId, int currency, params QuestObjectiveDefinition[] objectives)
        {
            QuestDefinition quest = ScriptableObject.CreateInstance<QuestDefinition>();
            quest.ConfigureForAudit(id, title, type, objectives, new RewardBundle { Currency = currency }, prerequisites, nextId);
            return quest;
        }

        private static void Register(params QuestDefinition[] quests)
        { QuestCatalog.ReloadForAudit(); foreach (QuestDefinition quest in quests) QuestCatalog.RegisterForAudit(quest); }

        private static void Check(bool condition, string message)
        { if (!condition) throw new InvalidOperationException("QUEST_SYSTEM_AUDIT FAIL: " + message); }
    }
}

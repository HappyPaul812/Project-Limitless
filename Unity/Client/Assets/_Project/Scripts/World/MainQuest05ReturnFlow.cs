using ProjectLimitless.Core;
using ProjectLimitless.NPC;
using ProjectLimitless.Player;
using ProjectLimitless.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectLimitless.World
{
    /// <summary>Main 04의 임시 동행을 마을 보고 뒤 정식 동료 해금으로 전환하는 Main 05 흐름입니다.</summary>
    public static class MainQuest05ReturnFlow
    {
        public const string QuestId = "main_05_return_of_three";
        public const string StarterVillageLocationId = "starter_village_main05_return";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;
            QuestService.Changed -= OnQuestChanged;
            QuestService.Changed += OnQuestChanged;
        }

        private static void OnQuestChanged()
        {
            Scene scene = SceneManager.GetActiveScene();
            if (scene.name == "Field_01" || scene.name == "World_StarterVillage") EnsureStartedAndAdvance(scene.name);
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode) => EnsureStartedAndAdvance(scene.name);

        private static void EnsureStartedAndAdvance(string sceneName)
        {
            bool shouldSave = false;
            if (QuestService.GetState(QuestId) == QuestState.Available)
                shouldSave = QuestService.TryStart(QuestId);

            QuestRuntimeState active = QuestService.ActiveMainQuest;
            if (sceneName == "World_StarterVillage" && active?.Definition.QuestId == QuestId
                && active.CurrentObjective?.TargetId == StarterVillageLocationId)
            {
                QuestService.NotifyLocationReached(StarterVillageLocationId);
                shouldSave = true;
            }

            if (shouldSave && GameSaveService.CurrentSlotIndex > 0) GameSaveService.SaveCurrentSession();
        }

        /// <summary>기존 마을 NPC 상호작용에서 stable ID로 Main 05 보고 대화를 선택합니다.</summary>
        public static bool TryHandleNpc(string npcId, NpcController npc)
        {
            QuestRuntimeState quest = QuestService.ActiveMainQuest;
            if (quest?.Definition.QuestId != QuestId || quest.CurrentObjective?.TargetId != npcId) return false;

            DialogueLine[] lines = npcId == MainQuest01NpcFlow.GuardId ? GuardReport() : RepresentativeReport();
            DialoguePresenter.Instance?.ShowSequence(lines, () => CompleteNpcReport(npcId));
            return true;
        }

        private static void CompleteNpcReport(string npcId)
        {
            QuestService.NotifyNpcTalked(npcId);
            if (npcId == MainQuest01NpcFlow.RepresentativeId && QuestService.GetState(QuestId) == QuestState.Completed)
            {
                // Main 03/04의 Story 참가 기록과 달리 이 시점부터 저장되는 정식 해금입니다.
                bool newlyUnlocked = CompanionRosterService.UnlockIntroCompanions();
                if (newlyUnlocked)
                    QuestHudPresenter.Notify("새로운 동료가 합류했습니다.\n태온\n미엘");
            }
            if (GameSaveService.CurrentSlotIndex > 0) GameSaveService.SaveCurrentSession();
        }

        private static string PlayerName => string.IsNullOrWhiteSpace(GameSessionData.PlayerName) ? "플레이어" : GameSessionData.PlayerName;
        private static DialogueLine Line(string id, string name, string message) => new DialogueLine(id, name, message);

        private static DialogueLine[] GuardReport() => new[]
        {
            Line(MainQuest01NpcFlow.GuardId, "남문 경비병", "돌아오셨군요.\n두 분은…?"),
            Line(string.Empty, PlayerName, "초원에서 만났습니다.\n함께 상황을 조사했습니다."),
            Line(CompanionRosterService.TaeonId, "태온", "몬스터들이 마을 쪽으로 움직이는 건 맞습니다.\n하지만 마을을 노리고 있는 것 같지는 않습니다."),
            Line(CompanionRosterService.MielId, "미엘", "무언가를 피해서 내려오고 있는 것 같아요."),
            Line(MainQuest01NpcFlow.GuardId, "남문 경비병", "피해서… 말입니까?"),
            Line(string.Empty, PlayerName, "아직 원인은 모릅니다.")
        };

        private static DialogueLine[] RepresentativeReport() => new[]
        {
            Line(MainQuest01NpcFlow.RepresentativeId, "주민 대표", "그렇다면 단순히 몬스터를 쫓아내는 것만으로\n해결될 일은 아니겠군요."),
            Line(MainQuest01NpcFlow.RepresentativeId, "주민 대표", "초원 너머 숲에서도\n비슷한 일이 있다는 이야기가 있었습니다."),
            Line(MainQuest01NpcFlow.RepresentativeId, "주민 대표", "세 분 덕분에 적어도\n어디부터 살펴봐야 할지는 알게 됐습니다."),
            Line(CompanionRosterService.TaeonId, "태온", "저도 이 움직임이\n어디서 시작됐는지 확인하고 싶습니다."),
            Line(CompanionRosterService.TaeonId, "태온", "괜찮으시다면…\n조금 더 함께 가겠습니다."),
            Line(CompanionRosterService.MielId, "미엘", "이대로 두면\n다치는 사람이 더 생길 겁니다."),
            Line(CompanionRosterService.MielId, "미엘", "저도 같이 가겠습니다.\n제가 할 수 있는 일이 있을 거예요."),
            Line(string.Empty, PlayerName, "그럼 앞으로도 잘 부탁드립니다.")
        };
    }
}

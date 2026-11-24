using System.Linq;
using ProjectLimitless.Battle;
using ProjectLimitless.Core;
using ProjectLimitless.Monster;
using ProjectLimitless.Player;
using ProjectLimitless.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectLimitless.World
{
    /// <summary>고갯길의 입장, 두 조사, 지정 승리와 정리 대화를 기존 순차 퀘스트에 연결합니다.</summary>
    public static class Chapter3Main25Flow
    {
        public const string SceneId = "World_NorthF3";
        public const string EntrySpawn = "Spawn_NorthF3_FromF2";
        public const string ReturnSpawn = "Spawn_NorthF2_FromF3";
        public const string QuestId = "main_25_broken_warmth_flow";
        public const string EncounterId = "main25_first_ice_beetle";
        public static int Step => QuestService.ActiveMainQuest?.Definition.QuestId == QuestId
            ? QuestService.ActiveMainQuest.CurrentObjectiveIndex : -1;
        public static bool Completed => QuestService.GetState(QuestId) == QuestState.Completed;

        public static Main23Site CurrentSite()
        {
            var kind = Step == 1 ? Main23Site.Kind.F3MarkSouth : Step == 2 ? Main23Site.Kind.F3MarkNorth
                : Step == 3 ? Main23Site.Kind.F3Beetle : Main23Site.Kind.F3North;
            return Object.FindObjectsByType<Main23Site>(FindObjectsSortMode.None).FirstOrDefault(site => site.SiteKind == kind);
        }

        internal static void UpdateArrival(PlayerController player)
        {
            if (player == null || WorldModalState.IsOpen || SceneManager.GetActiveScene().name != SceneId
                || !string.IsNullOrEmpty(GameSessionData.PendingSpawnPointId)) return;
            if (QuestService.GetState(QuestId) == QuestState.Available) QuestService.TryStart(QuestId);
            if (Step != 0) return;
            QuestService.NotifyLocationReached("north_f3_entry");
            Chapter3Main23Flow.Save();
        }

        internal static bool TryInteract(Main23Site site)
        {
            switch (site.SiteKind)
            {
                case Main23Site.Kind.F2North:
                    if (Chapter3Main24Flow.Completed) Travel("북쪽 눈 덮인 고갯길", SceneId, EntrySpawn);
                    else Explain("서리올빼미를 물리치고 동료들과 이야기한 뒤 북쪽으로 오세요.");
                    return true;
                case Main23Site.Kind.F3South:
                    Travel("남쪽 첫눈의 숲", Main24F2Traversal.SceneId, ReturnSpawn);
                    return true;
                case Main23Site.Kind.F3MarkSouth:
                    if (Step == 1)
                        Chapter3Main24Flow.ShowStory(() => Inspect(1, "main25_warmth_mark_south"),
                            new DialogueLine("companion_serin", "세린", "이쪽만 서리 모양이 다르네요. 바람 방향과도 맞지 않아요.", "main25_serin_001"),
                            new DialogueLine("companion_paul", "폴", "마력 표식입니다. 이곳에서 흐름이 끊긴 모양이에요.", "main25_paul_001"));
                    else Explain("서리가 덮인 오래된 마력 표식입니다.");
                    return true;
                case Main23Site.Kind.F3MarkNorth:
                    if (Step == 2)
                        Chapter3Main24Flow.ShowStory(() => Inspect(2, "main25_warmth_mark_north"),
                            new DialogueLine("companion_paul", "폴", "원래 이쪽으로 흘렀다면… 지금은 반대로 끌려가는군요.", "main25_paul_002"),
                            new DialogueLine("companion_taeon", "태온", "돌아갈 수 있게 길부터 표시하자.", "main25_taeon_001"),
                            new DialogueLine("story_haren", "하렌", "…그래. 어디로 가는 게 좋겠어?", "main25_haren_001"));
                    else Explain("남쪽 표식부터 조사하면 흐름을 비교할 수 있습니다.");
                    return true;
                case Main23Site.Kind.F3Beetle:
                    if (Step == 3)
                        Main21ChoicePanel.Show("얼음갑충과 싸우시겠습니까?", new[] { "전투 시작", "머무른다" }, choice =>
                        {
                            if (choice != 0 || Step != 3) return;
                            Chapter3Main24Flow.ShowStory(EnterBeetle,
                                new DialogueLine("companion_serin", "세린", "갑각을 닫으면 공격이 잘 통하지 않을 것 같아요.", "main25_serin_002"),
                                new DialogueLine("companion_taeon", "태온", "몸을 움츠릴 땐 다른 행동으로 대비하죠.", "main25_taeon_002"));
                        });
                    return true;
                case Main23Site.Kind.F3North:
                    if (Step == 4)
                        Chapter3Main24Flow.ShowStory(() => Inspect(4, "main25_broken_flow_debrief"),
                            new DialogueLine("companion_miel", "미엘", "북쪽에 쉬던 사람들이 있을지도 몰라요.", "main25_miel_001"),
                            new DialogueLine("companion_paul", "폴", "이 흔적을 따라가면 피난처에 닿을 겁니다.", "main25_paul_003"),
                            new DialogueLine("", "플레이어", "그쪽부터 확인합시다.", "main25_player_debrief"));
                    else Explain(Completed ? "북쪽은 백색 침엽림 방향입니다. 다음 구간은 준비 중입니다."
                        : "고갯길의 마력 흔적을 먼저 확인하세요.");
                    return true;
                default:
                    return false;
            }
        }

        private static void Inspect(int expected, string target)
        {
            if (Step != expected) return;
            QuestService.NotifyInteraction(target);
            Chapter3Main23Flow.Save();
        }

        private static void EnterBeetle()
        {
            if (Step != 3 || BattleSceneFlow.IsTransitioning) return;
            var spawn = Resources.Load<FieldMonsterSpawnDefinition>("Main25IceBeetle/Encounter");
            if (spawn == null || !MonsterEncounterService.TryRaise(spawn.Monster, spawn))
                QuestHudPresenter.Notify("얼음갑충 조우를 열 수 없습니다. 잠시 뒤 다시 선택하세요.");
        }

        private static void Travel(string label, string scene, string spawn)
        {
            Main21ChoicePanel.Show(label, new[] { "이동", "머무른다" }, choice =>
            {
                if (choice == 0 && !SceneTransitionService.TryLoad(scene, spawn))
                    QuestHudPresenter.Notify("이동 경로를 확인할 수 없습니다.");
            });
        }

        private static void Explain(string text)
        {
            DialoguePresenter.Instance.ShowSequence("눈 덮인 고갯길", new[] { text }, null);
        }
    }
}

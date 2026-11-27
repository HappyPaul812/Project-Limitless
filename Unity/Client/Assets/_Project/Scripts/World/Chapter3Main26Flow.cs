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
    /// <summary>F3 북문 보급부터 F4 주민 철수와 쉼터 대화까지 기존 퀘스트 진행으로 연결합니다.</summary>
    public static class Chapter3Main26Flow
    {
        public const string SceneId = "World_NorthF4";
        public const string EntrySpawn = "Spawn_NorthF4_FromF3";
        public const string ReturnSpawn = "Spawn_NorthF3_FromF4";
        public const string QuestId = "main_26_vanishing_shelter";
        public const string EncounterId = "main26_first_blizzard_wisp";
        public const string Ointment = "thaw_ointment";
        public static int Step => QuestService.ActiveMainQuest?.Definition.QuestId == QuestId
            ? QuestService.ActiveMainQuest.CurrentObjectiveIndex : -1;
        public static bool Completed => QuestService.GetState(QuestId) == QuestState.Completed;

        internal static Main23Site CurrentSite()
        {
            var kind = Step == 2 ? Main23Site.Kind.F4Shelter : Step == 3 ? Main23Site.Kind.F4Wisp
                : Step == 4 ? Main23Site.Kind.F4Evacuation : Step == 5 ? Main23Site.Kind.F4Shelter : Main23Site.Kind.F4North;
            return Object.FindObjectsByType<Main23Site>(FindObjectsSortMode.None).FirstOrDefault(site => site.SiteKind == kind);
        }

        internal static void AddEnvironment(Transform field)
        {
            // 검증된 넓은 통로의 바깥쪽에만 승인 침엽수·눈더미를 놓습니다.
            var tree = Resources.Load<Sprite>("Main26F4/Conifer");
            var drift = Resources.Load<Sprite>("Main26F4/Snowdrift");
            for (int i = 0; i < 5; i++)
            {
                Place(tree, new Vector2(-5.1f, -10 + 5 * i), 1, 2);
                Place(tree, new Vector2(5.2f, -8 + 5 * i), .85f, 2);
                Place(drift, new Vector2(i % 2 == 0 ? 3.8f : -3.8f, -11 + 5 * i), .85f, 1);
            }
            void Place(Sprite sprite, Vector2 at, float scale, int order)
            {
                if (sprite == null) return;
                var render = new GameObject(sprite.name, typeof(SpriteRenderer)).GetComponent<SpriteRenderer>();
                render.transform.SetParent(field, false);
                render.transform.position = at;
                render.transform.localScale = Vector3.one * scale;
                render.sprite = sprite;
                render.sortingOrder = order;
            }
        }

        internal static void UpdateArrival(PlayerController player)
        {
            if (player == null || Step != 1 || SceneManager.GetActiveScene().name != SceneId
                || !string.IsNullOrEmpty(GameSessionData.PendingSpawnPointId) || WorldModalState.IsOpen) return;
            var checkpoint = Object.FindObjectsByType<SceneSpawnPoint>(FindObjectsSortMode.None)
                .FirstOrDefault(spawn => spawn.SpawnPointId == EntrySpawn);
            if (checkpoint == null || Physics2D.OverlapCircleAll(checkpoint.transform.position, .55f)
                .Any(collider => !collider.isTrigger && collider.GetComponentInParent<PlayerController>() == null)) return;
            GameSessionData.ActivateSafeZone("safezone_north_f4_entry", SceneId, EntrySpawn);
            QuestService.NotifyLocationReached("north_f4_entry");
            Chapter3Main23Flow.Save();
        }

        internal static bool TryInteract(Main23Site site)
        {
            switch (site.SiteKind)
            {
                case Main23Site.Kind.F3North when Chapter3Main25Flow.Completed:
                    if (QuestService.GetState(QuestId) == QuestState.Available) QuestService.TryStart(QuestId);
                    if (Step == 0)
                    {
                        Chapter3Main24Flow.ShowStory(() =>
                        {
                            if (Step != 0) return;
                            if (!InventoryService.TryAddItem(Ointment, 2))
                            {
                                QuestHudPresenter.Notify("가방에 해빙 연고 두 개를 받을 공간이 필요합니다.");
                                return;
                            }
                            Advance(0, "main26_supply");
                            QuestHudPresenter.Notify("해빙 연고 2개를 받았습니다.");
                        }, Line("mare", "마레", "이번에는 동상에 대비하세요. 해빙 연고 두 개를 준비했습니다. 한기와 동상은 다른 상태예요. 연고는 동상에 써 주세요."));
                    }
                    else if (Step >= 1 || Completed) Travel("백색 침엽림으로 이동", SceneId, EntrySpawn);
                    return true;
                case Main23Site.Kind.F4South:
                    Travel("눈 덮인 고갯길로 이동", Chapter3Main25Flow.SceneId, ReturnSpawn);
                    return true;
                case Main23Site.Kind.F4Shelter:
                    if (Step == 2)
                    {
                        Chapter3Main24Flow.ShowStory(() => Advance(2, "main26_shelter"),
                            Line("", "피난 주민", "원래 이곳은 잠시 몸을 녹일 수 있었어요. 이제는 길마저 얼어서 움직이기 어렵네요."),
                            Line("companion_miel", "미엘", "먼저 상태를 확인할게요. 서둘러 움직이지 마세요.", "main26_miel_001"),
                            Line("companion_serin", "세린", "눈바람이 한곳에 모이고 있어요.", "main26_serin_001"));
                    }
                    else if (Step == 5) ShowTea();
                    else QuestHudPresenter.Notify("얼어붙은 원정 쉼터입니다.");
                    return true;
                case Main23Site.Kind.F4Wisp when Step == 3:
                    Main21ChoicePanel.Show("눈보라 정령과 싸우시겠습니까?", new[] { "전투 시작", "머무른다" }, choice =>
                    {
                        if (choice != 0 || Step != 3) return;
                        Chapter3Main24Flow.ShowStory(EnterWisp,
                            Line("companion_paul", "폴", "마력이 응축되고 있습니다. 저 공격은 한기보다 위험할 수 있어요.", "main26_paul_001"),
                            Line("companion_miel", "미엘", "동상은 행동하지 못하는 동안 체력도 줄어듭니다. 연고를 준비해 주세요.", "main26_miel_002"));
                    });
                    return true;
                case Main23Site.Kind.F4Evacuation when Step == 4:
                    Chapter3Main24Flow.ShowStory(() => Advance(4, "main26_evacuation"),
                        Line("story_haren", "하렌", "위험하니까 넌 안에 있어. 내가 확인하고 올게.", "main26_haren_001"),
                        Line("companion_taeon", "태온", "형, 나 혼자 하겠다는 게 아니야. 우리가 어디로 갈지 같이 정하자는 거야.", "main26_taeon_001"),
                        Line("story_haren", "하렌", "네가 다치면 어쩌려고!", "main26_haren_002"),
                        Line("companion_taeon", "태온", "나도 형이 다치는 건 싫어. 그래서 혼자 가게 두기 싫은 거야.", "main26_taeon_002"),
                        Line("", "플레이어", "함께 길을 확인합시다."),
                        Line("companion_serin", "세린", "저는 바람이 덜 부는 쪽을 볼게요.", "main26_serin_002"),
                        Line("companion_paul", "폴", "바퀴와 짐수레가 지나갈 길을 확인하겠습니다.", "main26_paul_002"),
                        Line("companion_miel", "미엘", "저는 주민들의 상태를 살필게요.", "main26_miel_003"),
                        Line("story_haren", "하렌", "…알았어. 같이 가자.", "main26_haren_003"),
                        Line("", "피난 주민", "이제 돌아갈 길을 알겠어요. 정말 고맙습니다."));
                    return true;
                case Main23Site.Kind.F4North:
                    QuestHudPresenter.Notify(Completed ? "북쪽 동토의 균열 방향입니다. 다음 구간은 준비 중입니다."
                        : "피난 주민의 안전을 먼저 확인하세요.");
                    return true;
                default:
                    return false;
            }
        }

        private static void ShowTea()
        {
            Chapter3Main24Flow.ShowStory(() =>
            {
                if (GameSessionData.SelectedPlayerVisual == PlayerVisualType.Female)
                    Main21ChoicePanel.Show("함께 차를 마시겠습니까?", new[] { "한 잔 마신다", "듣고 있는다" }, choice =>
                    {
                        if (choice == 0) Chapter3Main24Flow.ShowStory(FinishStory,
                            Line("", "플레이어", "두 분 차가 식기 전에 저도 한 잔 할래요."));
                        else FinishStory();
                    });
                else FinishStory();
            },
            Line("companion_serin", "세린", "미엘 씨, 폴 씨가 요즘 미엘 씨 몫의 차부터 챙기던데요?", "main26_serin_003"),
            Line("companion_miel", "미엘", "어머, 그런 것까지 보고 계셨어요?", "main26_miel_004"),
            Line("companion_serin", "세린", "하하, 제가 사수잖아요. 놓치는 게 별로 없죠.", "main26_serin_004"),
            Line("companion_miel", "미엘", "그럼 저도 조심해야겠네요.", "main26_miel_005"),
            Line("companion_serin", "세린", "그런데 미엘 씨는 다른 분들 치료부터 하시면서, 정작 자기 손은 왜 늘 나중이에요?", "main26_serin_005"),
            Line("companion_miel", "미엘", "……예전에도 저한테 먼저 치료받으라고 하던 사람이 있었어요.", "main26_miel_006"),
            Line("companion_serin", "세린", "그럼 이번엔 제 말도 들어 주세요. 차 식기 전에 마셔요.", "main26_serin_006"),
            Line("companion_miel", "미엘", "네. 이번엔 그럴게요.", "main26_miel_007"));
        }

        private static void FinishStory() => Chapter3Main24Flow.ShowStory(() => Advance(5, "main26_debrief"),
            Line("companion_taeon", "태온", "이제 더 북쪽을 확인해 보죠.", "main26_taeon_003"), Line("story_haren", "하렌", "그래.", "main26_haren_004"));

        private static DialogueLine Line(string speakerId, string speaker, string text, string dialogueId = null) =>
            new DialogueLine(speakerId, speaker, text, dialogueId);

        private static void EnterWisp()
        {
            var spawn = Resources.Load<FieldMonsterSpawnDefinition>("Main26F4/Encounter");
            if (Step == 3 && spawn != null && BattleSceneFlow.EnterStoryBattle(EncounterId, spawn.Monster, spawn, spawn.Position)) return;
            QuestHudPresenter.Notify("눈보라 정령 전투를 시작할 수 없습니다.");
        }

        private static void Advance(int expected, string target)
        {
            if (Step != expected) return;
            QuestService.NotifyInteraction(target);
            Chapter3Main23Flow.Save();
        }

        private static void Travel(string title, string scene, string spawn) =>
            Main21ChoicePanel.Show(title, new[] { "이동", "머무른다" }, choice =>
            {
                if (choice == 0 && !SceneTransitionService.TryLoad(scene, spawn))
                    QuestHudPresenter.Notify("이동 경로를 확인할 수 없습니다.");
            });
    }
}

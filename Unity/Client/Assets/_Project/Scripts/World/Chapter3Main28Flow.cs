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
    /// <summary>승리만 지정 조우 목표를 채우고, 현장 복귀 후 확인 대사가 Main28을 완료합니다.</summary>
    public static class Chapter3Main28Flow
    {
        public const string QuestId = "main_28_frost_tyrant";
        public const string PrerequisiteId = "main_27_light_beyond_ice";
        public const string EncounterId = "main28_frost_tyrant";
        public const string ArrivalId = "main28_last_warmth_arrival";
        public const string DebriefId = "main28_last_warmth_debrief";
        public static int Step => QuestService.ActiveMainQuest?.Definition.QuestId == QuestId
            ? QuestService.ActiveMainQuest.CurrentObjectiveIndex : -1;

        // 현장 씬 소유자가 검증된 복귀 위치와 체크포인트를 전달합니다. 이전 필드의 연결은 건드리지 않습니다.
        public static bool TryInteract(FieldMonsterSpawnDefinition encounter, string checkpointSpawnId)
        {
            if (encounter == null || encounter.SpawnId != EncounterId || encounter.Monster?.MonsterId != "frost_tyrant"
                || encounter.SceneName != SceneManager.GetActiveScene().name || BattleSceneFlow.IsTransitioning
                || string.IsNullOrWhiteSpace(checkpointSpawnId) || DialoguePresenter.Instance == null
                || WorldModalState.IsOpen) return false;
            if (Step == 0 || QuestService.GetState(QuestId) == QuestState.Available)
            {
                var checkpoint = Object.FindObjectsByType<SceneSpawnPoint>(FindObjectsSortMode.None)
                    .FirstOrDefault(spawn => spawn.gameObject.scene.name == encounter.SceneName
                        && spawn.SpawnPointId == checkpointSpawnId);
                if (checkpoint == null || Physics2D.OverlapCircleAll(checkpoint.transform.position, .55f)
                    .Any(collider => !collider.isTrigger && collider.GetComponentInParent<PlayerController>() == null))
                    return false;
            }
            if (QuestService.GetState(QuestId) == QuestState.Available) QuestService.TryStart(QuestId);
            if (Step == 0)
            {
                GameSessionData.ActivateSafeZone("safezone_last_warmth", encounter.SceneName, checkpointSpawnId);
                QuestService.NotifyLocationReached(ArrivalId);
                Chapter3Main23Flow.Save();
            }
            if (Step == 1)
            {
                Chapter3Main24Flow.ShowStory(() =>
                {
                    if (Step != 1) return;
                    Main21ChoicePanel.Show("설빙의 폭군", new[] { "전투 시작", "머무른다" }, choice =>
                    {
                        if (choice != 0 || Step != 1) return;
                        if (CompanionRosterService.ActivePartyCharacterIds.Take(2).Count() != 2)
                        {
                            QuestHudPresenter.Notify("동료 두 명을 편성한 뒤 전투를 시작하세요.");
                            return;
                        }
                        if (!BattleSceneFlow.EnterStoryBattle(EncounterId,
                            encounter.Monster, encounter, encounter.Position))
                            QuestHudPresenter.Notify("설빙의 폭군 전투를 시작할 수 없습니다.");
                    });
                },
                new DialogueLine("companion_serin", "세린", "화로에서 나오는 빛을 저 괴물의 냉기가 가로막고 있어요."),
                new DialogueLine("companion_paul", "폴", "화로의 마력은 아직 살아 있습니다. 저 힘이라면 놈의 냉기를 걷어낼 수 있을 겁니다."),
                new DialogueLine("companion_miel", "미엘", "한 사람에게 동결괴사가 세 번 쌓이면 전투불능이 될 수 있어요. 두 번이라도 위험합니다."),
                new DialogueLine("story_haren", "하렌", "태온, 어떻게 할까?"),
                new DialogueLine("companion_taeon", "태온", "저 화로를 이용하자. 누구든 위험해지면 서로 도와주면 돼."),
                new DialogueLine("", "플레이어", "준비되면 들어갑시다."));
                return true;
            }
            if (Step != 2) return false;
            Chapter3Main24Flow.ShowStory(() =>
            {
                if (Step != 2) return;
                QuestService.NotifyInteraction(DebriefId);
                Chapter3Main23Flow.Save();
            },
            new DialogueLine("companion_serin", "세린", "눈발이… 잦아들고 있어요."),
            new DialogueLine("companion_paul", "폴", "온기가 다시 흐르기 시작했습니다."),
            new DialogueLine("companion_miel", "미엘", "다들 괜찮으세요? 우선 안전한 곳으로 가요."));
            return true;
        }
    }
}

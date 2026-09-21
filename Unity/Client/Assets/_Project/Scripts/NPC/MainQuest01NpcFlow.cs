using ProjectLimitless.Core;
using ProjectLimitless.World;

namespace ProjectLimitless.NPC
{
    /// <summary>Main 01의 고유 대사만 보관하며, 범용 마커와 퀘스트 상태 판단에는 관여하지 않습니다.</summary>
    public static class MainQuest01NpcFlow
    {
        public const string QuestId = "main_01_call_reaches";
        public const string RepresentativeId = "starter-village-main-guide";
        public const string GuardId = "starter-village-gate-guard";

        private static readonly string[] RepresentativeDialogue =
        {
            "처음 보는 얼굴이군요. 여행자이십니까?",
            "마침 잘 오셨다고 해야 할지 모르겠군요.\n요 며칠 초원의 몬스터들이 마을 가까이까지 내려오고 있습니다.",
            "아직 큰 피해는 없지만, 평소와는 분명히 달라요.",
            "남문을 지키는 경비가 상황을 가장 잘 알고 있을 겁니다.\n시간이 괜찮으시다면 한번 이야기를 들어주시겠습니까?",
            "무슨 일이 있었는지 들어보겠습니다."
        };

        private static readonly string[] GuardDialogue =
        {
            "주민 대표에게 이야기를 들으셨군요.",
            "평소라면 초원 안쪽에서나 보이던 녀석들입니다.\n그런데 며칠 전부터 길 근처까지 내려오기 시작했습니다.",
            "쫓아내도 다시 나타나고요.",
            "이상한 건, 마을을 노리고 오는 것 같지는 않다는 겁니다.",
            "놈들이 어디서부터 내려오는 건지 확인할 수 있다면 도움이 될 텐데요."
        };

        public static bool TryHandle(string npcId, NpcController npc)
        {
            QuestState state = QuestService.GetState(QuestId);
            if (npcId == RepresentativeId && state == QuestState.Available)
                QuestService.TryStart(QuestId);

            QuestRuntimeState active = QuestService.ActiveMainQuest;
            string targetId = active?.CurrentObjective?.TargetId;
            if (active?.Definition.QuestId != QuestId || targetId != npcId) return false;

            string[] pages = npcId == RepresentativeId ? RepresentativeDialogue : GuardDialogue;
            ProjectLimitless.UI.DialoguePresenter.Instance?.ShowSequence(
                npcId, npc.DisplayName, pages, () =>
                {
                    QuestService.NotifyNpcTalked(npcId);
                    // Main 01의 마지막 대화가 끝난 직후 다음 조사 목표를 HUD에 자연스럽게 이어 줍니다.
                    if (npcId == GuardId && QuestService.GetState(MainQuest02FieldFlow.QuestId) == QuestState.Available)
                        QuestService.TryStart(MainQuest02FieldFlow.QuestId);
                });
            return true;
        }
    }
}

using ProjectLimitless.Core;
using ProjectLimitless.World;

namespace ProjectLimitless.Battle
{
    /// <summary>Main 04의 지정 3인 Story Encounter 승리만 퀘스트 진행 신호로 바꿉니다.</summary>
    public static class MainQuest04EncounterBridge
    {
        public const string EncounterId = MainQuest04FieldFlow.EncounterId;

        public static void NotifyVictory(string storyEncounterId)
        {
            // 일반 전투와 도주·패배에는 이 ID가 없으므로 미엘 대화 단계로 잘못 넘어가지 않습니다.
            if (storyEncounterId == EncounterId) QuestService.NotifyEncounterWon(EncounterId);
        }
    }
}

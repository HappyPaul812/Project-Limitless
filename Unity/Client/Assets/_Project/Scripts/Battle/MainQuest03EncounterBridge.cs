using ProjectLimitless.Core;
using ProjectLimitless.World;

namespace ProjectLimitless.Battle
{
    /// <summary>Main 03의 지정 Story Encounter 승리만 퀘스트 진행 신호로 바꿉니다.</summary>
    public static class MainQuest03EncounterBridge
    {
        public const string EncounterId = MainQuest03FieldFlow.EncounterId;

        public static void NotifyVictory(string storyEncounterId)
        {
            // 일반 조우 승리, 도망, 패배는 이 stable ID를 전달하지 않으므로 Main 03을 진행시키지 않습니다.
            if (storyEncounterId == EncounterId) QuestService.NotifyEncounterWon(EncounterId);
        }
    }
}

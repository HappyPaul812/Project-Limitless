using ProjectLimitless.Core;
using ProjectLimitless.World;

namespace ProjectLimitless.Battle
{
    /// <summary>Main 07 지정 전투의 승리만 다음 목표로 전달하며 도주·패배는 진행하지 않습니다.</summary>
    public static class MainQuest07EncounterBridge
    {
        public const string EncounterId = MainQuest07FieldFlow.EncounterId;
        public static void NotifyVictory(string storyEncounterId)
        { if (storyEncounterId == EncounterId) QuestService.NotifyEncounterWon(EncounterId); }
    }
}

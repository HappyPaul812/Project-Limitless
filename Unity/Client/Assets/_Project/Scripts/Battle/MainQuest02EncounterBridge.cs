using ProjectLimitless.Core;
using ProjectLimitless.Monster;
using ProjectLimitless.World;

namespace ProjectLimitless.Battle
{
    /// <summary>필드 Spawn 승리를 퀘스트용 stable Encounter ID로 바꾸는 작은 연결 지점입니다.</summary>
    public static class MainQuest02EncounterBridge
    {
        public static void NotifyVictory(FieldMonsterSpawnDefinition spawn)
        {
            // 일반 몬스터 처치 수가 아니라 조사 구역에 지정된 한 조우만 인정합니다.
            // SpawnDefinition과 리스폰 책임은 그대로 두므로 퀘스트 상태가 일반 필드 재생성을 바꾸지 않습니다.
            if (spawn == null || spawn.SceneName != "Field_01" || spawn.SpawnId != MainQuest02FieldFlow.QuestSpawnId) return;
            QuestService.NotifyEncounterWon(MainQuest02FieldFlow.EncounterId);
        }
    }
}

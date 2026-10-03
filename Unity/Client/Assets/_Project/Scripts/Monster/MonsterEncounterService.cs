using System;
using System.Collections.Generic;
using ProjectLimitless.World;
using UnityEngine;

namespace ProjectLimitless.Monster
{
    /// <summary>
    /// 필드 몬스터와 플레이어의 접촉을 향후 턴제 전투 시스템에 전달하는 연결 지점입니다.
    /// 현재는 조우 Event와 로그만 제공하며 Scene 이동, HP, 공격 또는 전투 시작 규칙은 처리하지 않습니다.
    /// </summary>
    public static class MonsterEncounterService
    {
        /// <summary>전투 시스템이 구현되면 구독하여 접촉한 몬스터 데이터를 받을 수 있는 Event입니다.</summary>
        public static event Action<MonsterDefinition, FieldMonsterSpawnDefinition> EncounterStarted;
        private static readonly Dictionary<string, float> defeatedUntilBySpawn = new Dictionary<string, float>();
        private static float suppressedUntil;

        // Play Mode를 다시 시작할 때 이전 실행에서 등록된 Event 구독이 남지 않게 초기화합니다.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Reset()
        {
            EncounterStarted = null;
            defeatedUntilBySpawn.Clear();
            suppressedUntil = 0f;
        }

        /// <summary>전투 복귀 직후 지정한 시간 동안 필드 조우를 받지 않습니다.</summary>
        public static void SuppressForSeconds(float seconds) => suppressedUntil = Mathf.Max(suppressedUntil, Time.unscaledTime + Mathf.Max(0f, seconds));

        /// <summary>접촉 조우를 받을 수 있으면 몬스터 종류와 정확한 스폰 데이터를 Event로 보내고 true를 반환합니다.</summary>
        public static bool TryRaise(MonsterDefinition monster, FieldMonsterSpawnDefinition spawn)
        {
            if (monster == null || spawn == null || Time.unscaledTime < suppressedUntil) return false;
            if (spawn.SceneName == Chapter2Main16Flow.Field && monster.MonsterId == "ember_wraith" &&
                !Chapter2Main16Flow.EmberGeneralSpawnUnlocked) return false;
            if (spawn.SpawnId == MainQuest11DungeonFlow.BossId && !MainQuest11DungeonFlow.IsBossAvailable) return false;

            Debug.Log($"몬스터 조우: {monster.DisplayName} ({spawn.SpawnId})");
            EncounterStarted?.Invoke(monster, spawn);
            return true;
        }

        /// <summary>승리한 스폰만 데이터에 지정된 시간 동안 비활성 상태로 기록합니다.</summary>
        public static void MarkDefeated(FieldMonsterSpawnDefinition spawn)
        {
            if (spawn == null) return;
            defeatedUntilBySpawn[GetSpawnKey(spawn)] = spawn.NonRespawningBoss ? float.PositiveInfinity :
                Time.realtimeSinceStartup + Mathf.Max(0f, spawn.RespawnSeconds);
        }

        /// <summary>해당 스폰이 지금 필드에 생성될 수 있는지 확인합니다.</summary>
        public static bool IsSpawnAvailable(FieldMonsterSpawnDefinition spawn)
        {
            if (spawn == null) return false;
            // 첫 지정 Story 승리 전에는 일반 불씨망령이 등장하지 않습니다. Save count가 Unlock을 복원합니다.
            if (spawn.SceneName == Chapter2Main16Flow.Field && spawn.Monster != null &&
                spawn.Monster.MonsterId == "ember_wraith" && !Chapter2Main16Flow.EmberGeneralSpawnUnlocked) return false;
            // Save/Continue 후 런타임 비재생성 Dictionary가 비어도 Quest 목표 기록이 보스 완료를 복원합니다.
            // 조사가 끝나기 전에는 보스 조우를 막아 먼저 처치하고 목표가 꼬이는 일도 방지합니다.
            if (spawn.SpawnId == MainQuest11DungeonFlow.BossId && !MainQuest11DungeonFlow.IsBossAvailable) return false;
            string key = GetSpawnKey(spawn);
            if (!defeatedUntilBySpawn.TryGetValue(key, out float defeatedUntil)) return true;
            if (Time.realtimeSinceStartup < defeatedUntil) return false;
            defeatedUntilBySpawn.Remove(key);
            return true;
        }

        private static string GetSpawnKey(FieldMonsterSpawnDefinition spawn) => $"{spawn.SceneName}:{spawn.SpawnId}";
    }
}

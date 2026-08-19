using System;
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
        public static event Action<MonsterDefinition> EncounterStarted;

        // Play Mode를 다시 시작할 때 이전 실행에서 등록된 Event 구독이 남지 않게 초기화합니다.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Reset() => EncounterStarted = null;

        /// <summary>MonsterFieldController가 플레이어 접촉을 처음 감지했을 때 한 번 호출합니다.</summary>
        public static void Raise(MonsterDefinition monster)
        {
            if (monster == null) return;

            Debug.Log($"몬스터 조우: {monster.DisplayName}");
            EncounterStarted?.Invoke(monster);
        }
    }
}

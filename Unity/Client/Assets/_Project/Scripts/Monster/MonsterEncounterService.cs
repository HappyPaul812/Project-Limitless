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
        private static float suppressedUntil;

        // Play Mode를 다시 시작할 때 이전 실행에서 등록된 Event 구독이 남지 않게 초기화합니다.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Reset()
        {
            EncounterStarted = null;
            suppressedUntil = 0f;
        }

        /// <summary>전투 복귀 직후 지정한 시간 동안 필드 조우를 받지 않습니다.</summary>
        public static void SuppressForSeconds(float seconds) => suppressedUntil = Mathf.Max(suppressedUntil, Time.unscaledTime + Mathf.Max(0f, seconds));

        /// <summary>접촉 조우를 받을 수 있으면 Event를 보내고 true를 반환합니다.</summary>
        public static bool TryRaise(MonsterDefinition monster)
        {
            if (monster == null || Time.unscaledTime < suppressedUntil) return false;

            Debug.Log($"몬스터 조우: {monster.DisplayName}");
            EncounterStarted?.Invoke(monster);
            return true;
        }

        /// <summary>기존 호출부 호환을 위한 조우 알림입니다.</summary>
        public static void Raise(MonsterDefinition monster) => TryRaise(monster);
    }
}

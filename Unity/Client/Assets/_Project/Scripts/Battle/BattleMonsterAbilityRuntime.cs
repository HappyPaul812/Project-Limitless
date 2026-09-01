using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectLimitless.Battle
{
    /// <summary>
    /// 몬스터 고유 기본 공격 효과의 재사용 대기시간을 전투 중에 보관합니다.
    ///
    /// 대상에게 걸린 "독 3/2/1"은 그 대상이 앞으로 몇 번 행동할 때 피해를 받을지 뜻합니다. 반면
    /// 독침벌의 "독침 대기 2/1/0"은 그 벌이 앞으로 몇 번 행동을 마쳐야 독을 다시 부여할 수 있는지
    /// 뜻합니다. 주인과 감소 기준이 완전히 다른 두 값이므로 같은 PoisonState에 넣지 않습니다.
    /// </summary>
    public sealed class BattleMonsterAbilityRuntime
    {
        private sealed class PoisonInflictionCooldownState
        {
            public int RemainingActions;
            public bool IgnoreCurrentActionCompletion;
        }

        // Combatant를 키로 사용하므로 향후 독침벌이 여러 마리 등장해도 각 벌의 2→1→0이 서로 섞이지 않습니다.
        private readonly Dictionary<Combatant, PoisonInflictionCooldownState> poisonInflictionCooldowns =
            new Dictionary<Combatant, PoisonInflictionCooldownState>(CombatantReferenceComparer.Instance);

        public int GetPoisonInflictionCooldown(Combatant actor)
        {
            return actor != null && actor.IsAlive &&
                poisonInflictionCooldowns.TryGetValue(actor, out PoisonInflictionCooldownState state)
                ? state.RemainingActions : 0;
        }

        public bool CanInflictPoison(Combatant actor) => actor != null && actor.IsAlive &&
            GetPoisonInflictionCooldown(actor) <= 0;

        /// <summary>
        /// 독을 정상 부여한 순간 대기시간을 시작합니다. 방금 독을 건 현재 행동은 대기시간을 줄이는 행동이
        /// 아니므로 첫 행동 완료 알림을 한 번 무시합니다. 그 결과 부여 직후 2, 다음 벌 행동 뒤 1,
        /// 그다음 벌 행동 뒤 0이라는 확정 흐름이 유지됩니다.
        /// </summary>
        public void StartPoisonInflictionCooldown(Combatant actor, int cooldownActions)
        {
            if (actor == null || !actor.IsAlive || cooldownActions <= 0) return;
            poisonInflictionCooldowns[actor] = new PoisonInflictionCooldownState
            {
                RemainingActions = cooldownActions,
                IgnoreCurrentActionCompletion = true
            };
        }

        /// <summary>행동을 마친 바로 그 Combatant의 독침 대기시간만 한 단계 줄입니다.</summary>
        public void CompleteActorAction(Combatant actor)
        {
            if (actor == null || !poisonInflictionCooldowns.TryGetValue(actor, out PoisonInflictionCooldownState state))
                return;
            if (state.IgnoreCurrentActionCompletion)
            {
                state.IgnoreCurrentActionCompletion = false;
                return;
            }

            state.RemainingActions = Math.Max(0, state.RemainingActions - 1);
            if (state.RemainingActions == 0) poisonInflictionCooldowns.Remove(actor);
        }

        /// <summary>전투불능 몬스터는 다시 행동하지 않으므로 남은 내부 대기 상태도 정리합니다.</summary>
        public void RemoveInvalidCombatants(IEnumerable<Combatant> combatants)
        {
            if (combatants == null) return;
            foreach (Combatant actor in combatants.Where(item => item != null && !item.IsAlive).ToArray())
                poisonInflictionCooldowns.Remove(actor);
        }
    }
}

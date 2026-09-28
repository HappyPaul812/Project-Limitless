using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectLimitless.Battle
{
    /// <summary>
    /// B2 몬스터의 재사용 대기와 장막, 보스의 예고·단계를 이번 전투에만 보관합니다.
    /// 저장 파일에는 넣지 않으므로 승리·도망·전멸 뒤 새 전투에 임시 상태가 넘어가지 않습니다.
    /// </summary>
    public sealed class BattleDungeonMonsterRuntime
    {
        private sealed class EchoCooldown
        {
            public int Remaining;
            public bool IgnoreCastingAction;
        }

        private readonly Dictionary<Combatant, EchoCooldown> echoCooldowns =
            new Dictionary<Combatant, EchoCooldown>(CombatantReferenceComparer.Instance);
        private readonly Dictionary<Combatant, Combatant> sealProtectors =
            new Dictionary<Combatant, Combatant>(CombatantReferenceComparer.Instance);
        private Combatant boss;
        private Combatant[] summonedGuardians = Array.Empty<Combatant>();
        private int bossActionIndex;
        private bool shockPrepared;
        private bool phaseTwo;

        public bool IsShockPrepared => shockPrepared;
        public bool IsPhaseTwo => phaseTwo;
        public bool CanEchoWhisper(Combatant actor) => actor != null && actor.IsAlive &&
            (!echoCooldowns.TryGetValue(actor, out EchoCooldown state) || state.Remaining == 0);

        /// <summary>시전한 현재 행동은 제외하고 다음 자기 행동 두 번을 지난 뒤에만 재사용합니다.</summary>
        public void StartEchoWhisperCooldown(Combatant actor)
        {
            if (actor == null || !actor.IsAlive) return;
            echoCooldowns[actor] = new EchoCooldown { Remaining = 2, IgnoreCastingAction = true };
        }

        public void CompleteActorAction(Combatant actor)
        {
            if (actor == null || !echoCooldowns.TryGetValue(actor, out EchoCooldown state)) return;
            if (state.IgnoreCastingAction) { state.IgnoreCastingAction = false; return; }
            state.Remaining = Math.Max(0, state.Remaining - 1);
            if (state.Remaining == 0) echoCooldowns.Remove(actor);
        }

        /// <summary>보호 대상과 실제 수호체 참조를 묶어 수호체 KO 직후 자동 해제합니다.</summary>
        public bool TryProtect(Combatant guardian, Combatant target)
        {
            if (guardian == null || target == null || guardian == target || !guardian.IsAlive || !target.IsAlive ||
                guardian.Side != target.Side || sealProtectors.ContainsKey(target)) return false;
            sealProtectors[target] = guardian;
            return true;
        }

        public bool IsProtected(Combatant target) => target != null && sealProtectors.TryGetValue(target, out Combatant guardian)
            && guardian != null && guardian.IsAlive && target.IsAlive;

        public void SetBoss(Combatant combatant) => boss = combatant;
        public bool ShouldEnterPhaseTwo => boss != null && boss.IsAlive && !phaseTwo &&
            (long)boss.CurrentHp * 100 <= (long)boss.MaxHp * 60;

        /// <summary>HP 경계는 한 번만 통과하고, 소환 자체가 그 행동을 차지합니다.</summary>
        public void EnterPhaseTwo(IReadOnlyList<Combatant> guardians)
        {
            phaseTwo = true;
            shockPrepared = false;
            summonedGuardians = guardians == null ? Array.Empty<Combatant>() : guardians.Where(item => item != null).ToArray();
        }

        public bool HasWardenBarrier => phaseTwo && boss != null && boss.IsAlive &&
            summonedGuardians.Any(item => item.IsAlive);

        public void PrepareShock() => shockPrepared = true;
        public void CompleteShock() => shockPrepared = false;

        /// <summary>보스의 기본 패턴을 한 행동씩 순환시킵니다. 침묵 표식 간격은 네 행동 이상입니다.</summary>
        public int TakeBossPattern() => bossActionIndex++ % 3;

        /// <summary>
        /// 기존 개인 방어 계산 전에 장막 한 종류만 적용합니다. DoT는 그대로 통과시키고 보스에게는
        /// 파수의 장막만 허용해 수호체 장막과 30% 감소가 중첩되지 않게 합니다.
        /// </summary>
        public int ModifyIncomingDamage(Combatant target, int rawDamage, BattleDamageOrigin origin)
        {
            if (target == null || origin != BattleDamageOrigin.DirectCombatAction) return rawDamage;
            bool barrier = target == boss ? HasWardenBarrier : IsProtected(target);
            return barrier ? (int)Math.Max(1L, ((long)Math.Max(1, rawDamage) * 70L + 99L) / 100L) : rawDamage;
        }

        public void RemoveInvalidCombatants(IEnumerable<Combatant> combatants)
        {
            if (combatants == null) return;
            foreach (Combatant dead in combatants.Where(item => item != null && !item.IsAlive).ToArray())
            {
                echoCooldowns.Remove(dead);
                sealProtectors.Remove(dead);
            }
            foreach (Combatant target in sealProtectors.Where(pair => !pair.Value.IsAlive).Select(pair => pair.Key).ToArray())
                sealProtectors.Remove(target);
            if (boss != null && !boss.IsAlive) shockPrepared = false;
        }

        public void Clear()
        {
            echoCooldowns.Clear();
            sealProtectors.Clear();
            summonedGuardians = Array.Empty<Combatant>();
            boss = null;
            shockPrepared = false;
            phaseTwo = false;
            bossActionIndex = 0;
        }
    }
}

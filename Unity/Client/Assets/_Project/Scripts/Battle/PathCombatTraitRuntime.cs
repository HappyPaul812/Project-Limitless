using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectLimitless.Battle
{
    /// <summary>PathId가 명시된 참가자들의 길 효과와 전투별 임시 상태를 함께 관리합니다.</summary>
    public sealed class PathCombatTraitRuntime
    {
        public const string VisionPathId = "path.vision";
        public const string HearingPathId = "path.hearing";
        public const string IntellectualPathId = "path.intellectual";
        public const string MobilityPathId = "path.mobility";
        public const string EmotionalScarPathId = "path.emotional-scar";

        private sealed class TraitState
        {
            public Combatant Owner;
            public string PathId;
            public bool ResilienceTriggered;
            public int ResilienceActionsRemaining;
            public readonly HashSet<Combatant> Echoes = new HashSet<Combatant>(CombatantReferenceComparer.Instance);
            public readonly HashSet<Combatant> EnemiesThatAttacked = new HashSet<Combatant>(CombatantReferenceComparer.Instance);
            public Combatant FocusTarget;
            public int FocusStacks;
            public readonly Dictionary<Combatant, Combatant> LastTargetByEnemy = new Dictionary<Combatant, Combatant>(CombatantReferenceComparer.Instance);
            public readonly Dictionary<Combatant, Combatant> PatternEnemyByAlly = new Dictionary<Combatant, Combatant>(CombatantReferenceComparer.Instance);
        }

        private readonly Dictionary<Combatant, TraitState> states = new Dictionary<Combatant, TraitState>(CombatantReferenceComparer.Instance);
        private readonly Combatant[] combatants;

        /// <summary>
        /// 이름으로 태온·미엘을 판정하면 번역이나 개명 때 기능이 깨집니다. 참가자 데이터의 PathId와 실제
        /// Combatant 참조를 묶어 두면 플레이어와 NPC가 같은 규칙을 쓰면서도 각 상태가 서로 섞이지 않습니다.
        /// </summary>
        public PathCombatTraitRuntime(IEnumerable<KeyValuePair<Combatant, string>> bindings, IEnumerable<Combatant> combatants)
        {
            this.combatants = combatants?.Where(item => item != null).ToArray() ?? Array.Empty<Combatant>();
            if (bindings == null) return;
            foreach (KeyValuePair<Combatant, string> binding in bindings)
                if (binding.Key != null && !string.IsNullOrWhiteSpace(binding.Value))
                    states[binding.Key] = new TraitState { Owner = binding.Key, PathId = binding.Value };
        }

        public event Action<string> FeedbackOccurred;
        public string GetPathId(Combatant owner) => Find(owner)?.PathId ?? string.Empty;
        public int GetResilienceActionsRemaining(Combatant owner) => Find(owner)?.ResilienceActionsRemaining ?? 0;
        public Combatant GetFocusTarget(Combatant owner)
        {
            TraitState state = Find(owner);
            return state?.FocusTarget != null && state.FocusTarget.IsAlive ? state.FocusTarget : null;
        }
        public int GetFocusStacks(Combatant owner) => GetFocusTarget(owner) == null ? 0 : Find(owner).FocusStacks;
        public bool HasEcho(Combatant target) => target != null && states.Values.Any(state => state.Echoes.Contains(target));
        public Combatant GetPatternEnemy(Combatant owner, Combatant ally)
        {
            TraitState state = Find(owner);
            return state != null && ally != null && state.PatternEnemyByAlly.TryGetValue(ally, out Combatant enemy) && enemy.IsAlive ? enemy : null;
        }
        public Combatant GetAnyPatternEnemy(Combatant ally) => states.Values.Select(state =>
            state.PatternEnemyByAlly.TryGetValue(ally, out Combatant enemy) && enemy.IsAlive ? enemy : null).FirstOrDefault(enemy => enemy != null);

        public void BeginActorAction(Combatant actor)
        {
            if (actor == null) return;
            foreach (TraitState state in states.Values.Where(item => item.PathId == HearingPathId && item.Echoes.Remove(actor)))
                FeedbackOccurred?.Invoke($"{actor.DisplayName}의 잔향이 사라졌습니다.");
            ClearInvalidState();
        }

        public void CompleteActorAction(Combatant actor)
        {
            if (actor == null) return;
            TraitState ownerState = Find(actor);
            if (ownerState?.ResilienceActionsRemaining > 0) ownerState.ResilienceActionsRemaining--;
            foreach (TraitState state in states.Values.Where(item => item.PathId == HearingPathId && actor.IsAlive && item.EnemiesThatAttacked.Remove(actor)))
            {
                state.Echoes.Add(actor);
                FeedbackOccurred?.Invoke($"{state.Owner.DisplayName}이(가) {actor.DisplayName}의 잔향을 포착했습니다.");
            }
            ClearInvalidState();
        }

        /// <summary>길 배율을 먼저 한 번 계산한 뒤 기존 방어·상태 경로에 실제 피해를 한 번만 전달합니다.</summary>
        public int ApplyDirectDamage(BattleStatusEffectRuntime statusEffects, Combatant source, Combatant target, int rawDamage, bool areaAttack = false)
        {
            if (statusEffects == null) throw new ArgumentNullException(nameof(statusEffects));
            if (source == null || target == null) return 0;
            int modified = rawDamage;
            TraitState sourceState = Find(source);
            bool consumeEcho = false;
            if (sourceState != null)
            {
                if (sourceState.PathId == EmotionalScarPathId && sourceState.ResilienceActionsRemaining > 0) modified = Increase(modified, 10);
                else if (sourceState.PathId == HearingPathId && sourceState.Echoes.Contains(target)) { modified = Increase(modified, 10); consumeEcho = true; }
                else if (sourceState.PathId == VisionPathId && ReferenceEquals(target, GetFocusTarget(source))) modified = Increase(modified, sourceState.FocusStacks * 3);
                else if (sourceState.PathId == MobilityPathId && source.Slot.Row == FormationRow.Rear) modified = Increase(modified, 5);
            }
            TraitState targetState = Find(target);
            if (targetState?.PathId == MobilityPathId && target.Slot.Row == FormationRow.Front) modified = Reduce(modified, 5);

            List<TraitState> consumedPatterns = states.Values.Where(state => state.PathId == IntellectualPathId &&
                state.PatternEnemyByAlly.TryGetValue(target, out Combatant enemy) && ReferenceEquals(enemy, source)).ToList();
            if (consumedPatterns.Count > 0)
            {
                modified = Reduce(modified, 10);
                foreach (TraitState state in consumedPatterns) state.PatternEnemyByAlly.Remove(target);
                FeedbackOccurred?.Invoke($"{target.DisplayName}이(가) 익힌 {source.DisplayName}의 패턴으로 피해를 줄였습니다.");
            }

            Dictionary<Combatant, int> hpBefore = combatants.ToDictionary(item => item, item => item.CurrentHp, CombatantReferenceComparer.Instance);
            int applied = statusEffects.ApplyIncomingDamage(target, modified, BattleDamageOrigin.DirectCombatAction);
            foreach (TraitState state in states.Values.Where(item => item.PathId == EmotionalScarPathId))
                foreach (KeyValuePair<Combatant, int> pair in hpBefore.Where(pair => pair.Key.Side == state.Owner.Side))
                    ObserveThresholdCrossing(state, pair.Key, pair.Value);

            if (applied > 0)
            {
                foreach (TraitState state in states.Values.Where(item => source.Side != item.Owner.Side && target.Side == item.Owner.Side))
                {
                    state.EnemiesThatAttacked.Add(source);
                    if (state.PathId != IntellectualPathId) continue;
                    bool sameTarget = state.LastTargetByEnemy.TryGetValue(source, out Combatant previous) && ReferenceEquals(previous, target);
                    state.LastTargetByEnemy[source] = target;
                    if (sameTarget && !consumedPatterns.Contains(state) && target.IsAlive)
                    {
                        state.PatternEnemyByAlly[target] = source;
                        FeedbackOccurred?.Invoke($"{state.Owner.DisplayName}이(가) {source.DisplayName}의 공격 패턴을 익혔습니다.");
                    }
                }
                if (sourceState != null)
                {
                    if (consumeEcho) { sourceState.Echoes.Remove(target); FeedbackOccurred?.Invoke($"{target.DisplayName}의 잔향을 이용했습니다."); }
                    // 광역은 여러 대상을 동시에 맞히므로 기존 집중 대상만 강화하고 대상을 선택하거나 중첩시키지 않습니다.
                    if (sourceState.PathId == VisionPathId && !areaAttack)
                    {
                        if (ReferenceEquals(sourceState.FocusTarget, target)) sourceState.FocusStacks = Math.Min(3, sourceState.FocusStacks + 1);
                        else { sourceState.FocusTarget = target; sourceState.FocusStacks = 1; }
                    }
                }
            }
            ClearInvalidState();
            return applied;
        }

        public int ModifyDirectHealing(Combatant source, Combatant target, int rawHealing)
        {
            TraitState state = Find(source);
            if (state == null || target == null) return rawHealing;
            if (state.PathId == EmotionalScarPathId && state.ResilienceActionsRemaining > 0) return Increase(rawHealing, 10);
            if (state.PathId == MobilityPathId && source.Slot.Row == FormationRow.Rear) return Increase(rawHealing, 5);
            if (state.PathId == IntellectualPathId && GetPatternEnemy(source, target) != null) return Increase(rawHealing, 10);
            return rawHealing;
        }

        /// <summary>화상·독에는 길 배율을 적용하지 않고 회복탄력의 실제 HP 경계 통과만 관찰합니다.</summary>
        public int ObserveHealthChange(Combatant target, Func<int> applyChange)
        {
            if (applyChange == null) return 0;
            int beforeHp = target?.CurrentHp ?? 0;
            int result = applyChange();
            foreach (TraitState state in states.Values.Where(item => item.PathId == EmotionalScarPathId && target?.Side == item.Owner.Side))
                ObserveThresholdCrossing(state, target, beforeHp);
            return result;
        }

        private TraitState Find(Combatant owner) => owner != null && states.TryGetValue(owner, out TraitState state) ? state : null;
        private void ObserveThresholdCrossing(TraitState state, Combatant target, int beforeHp)
        {
            if (state.ResilienceTriggered || target == null || beforeHp <= 0 || beforeHp * 2 <= target.MaxHp || target.CurrentHp * 2 > target.MaxHp) return;
            state.ResilienceTriggered = true;
            state.ResilienceActionsRemaining = 2;
            FeedbackOccurred?.Invoke($"{state.Owner.DisplayName}의 회복탄력이 발동했습니다. 다음 2회 행동이 강화됩니다.");
        }

        private void ClearInvalidState()
        {
            foreach (TraitState state in states.Values)
            {
                state.Echoes.RemoveWhere(item => item == null || !item.IsAlive);
                state.EnemiesThatAttacked.RemoveWhere(item => item == null || !item.IsAlive);
                if (state.FocusTarget != null && !state.FocusTarget.IsAlive) { state.FocusTarget = null; state.FocusStacks = 0; }
                foreach (Combatant ally in state.PatternEnemyByAlly.Where(pair => pair.Key == null || !pair.Key.IsAlive || pair.Value == null || !pair.Value.IsAlive).Select(pair => pair.Key).ToArray())
                    state.PatternEnemyByAlly.Remove(ally);
            }
        }

        private static int Increase(int value, int percent) => Math.Max(1, (int)Math.Ceiling(value * (100 + percent) / 100f));
        private static int Reduce(int value, int percent) => Math.Max(1, (int)Math.Ceiling(value * (100 - percent) / 100f));
    }
}

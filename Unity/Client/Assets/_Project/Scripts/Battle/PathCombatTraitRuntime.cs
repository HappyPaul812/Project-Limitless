using System;
using System.Collections.Generic;

namespace ProjectLimitless.Battle
{
    /// <summary>저장된 플레이어 길을 전투 한 판 동안만 해석하는 공통 런타임입니다.</summary>
    public sealed class PathCombatTraitRuntime
    {
        public const string VisionPathId = "path.vision";
        public const string HearingPathId = "path.hearing";
        public const string IntellectualPathId = "path.intellectual";
        public const string MobilityPathId = "path.mobility";
        public const string EmotionalScarPathId = "path.emotional-scar";

        // 길은 직업 ID를 전혀 보지 않습니다. 추천 직업은 사용법을 안내하는 시너지일 뿐 기능 잠금이 아닙니다.
        private readonly Combatant owner;
        private readonly Combatant[] ownerAllies;
        private readonly string pathId;
        // 같은 정의의 몬스터 A/B도 별개 상태를 가져야 하므로 ID나 이름 대신 실제 Combatant 참조를 키로 씁니다.
        private readonly HashSet<Combatant> echoes = new HashSet<Combatant>(CombatantReferenceComparer.Instance);
        private readonly HashSet<Combatant> enemiesThatAttacked = new HashSet<Combatant>(CombatantReferenceComparer.Instance);
        private readonly Dictionary<Combatant, Combatant> lastTargetByEnemy = new Dictionary<Combatant, Combatant>(CombatantReferenceComparer.Instance);
        private readonly Dictionary<Combatant, Combatant> patternEnemyByAlly = new Dictionary<Combatant, Combatant>(CombatantReferenceComparer.Instance);
        private bool resilienceTriggered;
        private int resilienceActionsRemaining;
        private Combatant focusTarget;
        private int focusStacks;

        public PathCombatTraitRuntime(Combatant owner, string pathId, IEnumerable<Combatant> ownerAllies = null)
        {
            this.owner = owner;
            this.pathId = pathId ?? string.Empty;
            this.ownerAllies = ownerAllies == null ? Array.Empty<Combatant>() : new List<Combatant>(ownerAllies).ToArray();
        }

        public event Action<string> FeedbackOccurred;
        public Combatant Owner => owner;
        public string PathId => pathId;
        public int ResilienceActionsRemaining => resilienceActionsRemaining;
        public Combatant FocusTarget => focusTarget != null && focusTarget.IsAlive ? focusTarget : null;
        public int FocusStacks => FocusTarget == null ? 0 : focusStacks;
        public bool HasEcho(Combatant combatant) => combatant != null && echoes.Contains(combatant);
        public Combatant GetPatternEnemy(Combatant ally) => ally != null && patternEnemyByAlly.TryGetValue(ally, out Combatant enemy) && enemy.IsAlive ? enemy : null;

        public void BeginActorAction(Combatant actor)
        {
            if (actor == null) return;
            if (actor.Side != owner?.Side && echoes.Remove(actor))
                FeedbackOccurred?.Invoke($"{actor.DisplayName}의 잔향이 사라졌습니다.");
            ClearInvalidState();
        }

        public void CompleteActorAction(Combatant actor)
        {
            if (actor == null) return;
            if (ReferenceEquals(actor, owner) && resilienceActionsRemaining > 0)
                resilienceActionsRemaining--;
            if (actor.Side != owner?.Side && actor.IsAlive && enemiesThatAttacked.Remove(actor) && pathId == HearingPathId)
            {
                echoes.Add(actor);
                FeedbackOccurred?.Invoke($"{actor.DisplayName}의 움직임에서 잔향을 포착했습니다.");
            }
            ClearInvalidState();
        }

        public int ApplyDirectDamage(BattleStatusEffectRuntime statusEffects, Combatant source, Combatant target,
            int rawDamage, bool areaAttack = false)
        {
            if (statusEffects == null) throw new ArgumentNullException(nameof(statusEffects));
            if (source == null || target == null) return 0;

            bool consumeEcho = false;
            int modified = rawDamage;
            if (ReferenceEquals(source, owner))
            {
                if (pathId == EmotionalScarPathId && resilienceActionsRemaining > 0) modified = Increase(modified, 10);
                else if (pathId == HearingPathId && echoes.Contains(target)) { modified = Increase(modified, 10); consumeEcho = true; }
                else if (pathId == VisionPathId && ReferenceEquals(target, FocusTarget)) modified = Increase(modified, focusStacks * 3);
                else if (pathId == MobilityPathId && owner.Slot.Row == FormationRow.Rear) modified = Increase(modified, 5);
            }

            bool consumedPattern = false;
            if (ReferenceEquals(target, owner) && pathId == MobilityPathId && owner.Slot.Row == FormationRow.Front)
                modified = Reduce(modified, 5);
            if (pathId == IntellectualPathId && patternEnemyByAlly.TryGetValue(target, out Combatant patternEnemy) && ReferenceEquals(source, patternEnemy))
            {
                modified = Reduce(modified, 10);
                patternEnemyByAlly.Remove(target);
                consumedPattern = true;
                FeedbackOccurred?.Invoke($"{target.DisplayName}이(가) 익힌 {source.DisplayName}의 패턴으로 피해를 줄였습니다.");
            }

            // 수호의 맹세가 다른 아군에게 피해를 이전할 수 있어 플레이어 편 전체의 실제 HP 변화를 관찰합니다.
            Dictionary<Combatant, int> allyHpBefore = new Dictionary<Combatant, int>(CombatantReferenceComparer.Instance);
            foreach (Combatant ally in ownerAllies)
                if (ally != null) allyHpBefore[ally] = ally.CurrentHp;
            int applied = statusEffects.ApplyIncomingDamage(target, modified, BattleDamageOrigin.DirectCombatAction);
            foreach (KeyValuePair<Combatant, int> pair in allyHpBefore) ObserveThresholdCrossing(pair.Key, pair.Value);

            if (source.Side != owner?.Side && target.Side == owner?.Side && applied > 0)
            {
                enemiesThatAttacked.Add(source);
                if (pathId == IntellectualPathId)
                {
                    bool sameTarget = lastTargetByEnemy.TryGetValue(source, out Combatant previous) && ReferenceEquals(previous, target);
                    lastTargetByEnemy[source] = target;
                    if (sameTarget && !consumedPattern && target.IsAlive)
                    {
                        patternEnemyByAlly[target] = source;
                        FeedbackOccurred?.Invoke($"{target.DisplayName}이(가) {source.DisplayName}의 공격 패턴을 익혔습니다.");
                    }
                }
            }

            if (ReferenceEquals(source, owner) && applied > 0)
            {
                if (consumeEcho)
                {
                    echoes.Remove(target);
                    FeedbackOccurred?.Invoke($"{target.DisplayName}의 잔향을 이용해 피해를 높였습니다.");
                }
                if (pathId == VisionPathId && !areaAttack)
                {
                    // 광역 공격은 여러 적을 동시에 맞히므로 집중 대상을 새로 정하거나 중첩을 쌓지 않습니다.
                    if (ReferenceEquals(focusTarget, target)) focusStacks = Math.Min(3, focusStacks + 1);
                    else { focusTarget = target; focusStacks = 1; }
                }
            }
            ClearInvalidState();
            return applied;
        }

        public int ModifyDirectHealing(Combatant source, Combatant target, int rawHealing)
        {
            if (!ReferenceEquals(source, owner) || target == null) return rawHealing;
            if (pathId == EmotionalScarPathId && resilienceActionsRemaining > 0) return Increase(rawHealing, 10);
            if (pathId == MobilityPathId && owner.Slot.Row == FormationRow.Rear) return Increase(rawHealing, 5);
            if (pathId == IntellectualPathId && GetPatternEnemy(target) != null) return Increase(rawHealing, 10);
            return rawHealing;
        }

        /// <summary>화상·독에는 길 배율을 섞지 않고, HP 50% 통과라는 실제 사건만 관찰합니다.</summary>
        public int ObserveHealthChange(Combatant target, Func<int> applyChange)
        {
            if (applyChange == null) return 0;
            int beforeHp = target?.CurrentHp ?? 0;
            int result = applyChange();
            ObserveThresholdCrossing(target, beforeHp);
            return result;
        }

        private void ObserveThresholdCrossing(Combatant target, int beforeHp)
        {
            if (pathId != EmotionalScarPathId || resilienceTriggered || owner == null || target == null ||
                target.Side != owner.Side || beforeHp <= 0 || beforeHp * 2 <= target.MaxHp || target.CurrentHp * 2 > target.MaxHp) return;
            resilienceTriggered = true;
            // 이 값은 SaveData가 아닌 전투 런타임에만 있어 새 전투마다 '전투당 1회'가 자연스럽게 초기화됩니다.
            resilienceActionsRemaining = 2;
            FeedbackOccurred?.Invoke($"{owner.DisplayName}의 회복탄력이 발동했습니다. 다음 2회 행동이 강화됩니다.");
        }

        private void ClearInvalidState()
        {
            echoes.RemoveWhere(item => item == null || !item.IsAlive);
            enemiesThatAttacked.RemoveWhere(item => item == null || !item.IsAlive);
            if (focusTarget != null && !focusTarget.IsAlive) { focusTarget = null; focusStacks = 0; }
            List<Combatant> invalidAllies = new List<Combatant>();
            foreach (KeyValuePair<Combatant, Combatant> pair in patternEnemyByAlly)
                if (pair.Key == null || !pair.Key.IsAlive || pair.Value == null || !pair.Value.IsAlive) invalidAllies.Add(pair.Key);
            foreach (Combatant ally in invalidAllies) patternEnemyByAlly.Remove(ally);
        }

        private static int Increase(int value, int percent) => Math.Max(1, (int)Math.Ceiling(value * (100 + percent) / 100f));
        private static int Reduce(int value, int percent) => Math.Max(1, (int)Math.Ceiling(value * (100 - percent) / 100f));
    }
}

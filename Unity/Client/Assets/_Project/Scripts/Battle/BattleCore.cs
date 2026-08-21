using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectLimitless.Battle
{
    /// <summary>전열과 후열을 구분합니다. 각 행은 왼쪽·가운데·오른쪽의 세 슬롯을 가집니다.</summary>
    public enum FormationRow { Front, Rear }

    /// <summary>기본 공격과 향후 스킬이 사용하는 공통 사거리 종류입니다.</summary>
    public enum TargetRangeType { MeleePhysical, RangedPhysical, Magic }

    /// <summary>전투 참가자가 아군인지 적인지 구분합니다.</summary>
    public enum BattleSide { Allies, Enemies }

    /// <summary>2×3 진형 안의 한 위치를 나타냅니다. Column은 왼쪽부터 0, 1, 2입니다.</summary>
    public readonly struct FormationSlot : IEquatable<FormationSlot>
    {
        public FormationSlot(FormationRow row, int column)
        {
            if (column < 0 || column > 2) throw new ArgumentOutOfRangeException(nameof(column));
            Row = row;
            Column = column;
        }

        public FormationRow Row { get; }
        public int Column { get; }
        public bool Equals(FormationSlot other) => Row == other.Row && Column == other.Column;
        public override bool Equals(object obj) => obj is FormationSlot other && Equals(other);
        public override int GetHashCode() => ((int)Row * 397) ^ Column;
        public override string ToString() => $"{Row}-{Column}";
    }

    /// <summary>
    /// 플레이어, 동료, 몬스터를 같은 턴 규칙으로 다루는 런타임 전투 데이터입니다.
    /// MonoBehaviour와 분리되어 향후 NPC나 여러 몬스터도 같은 구조를 재사용할 수 있습니다.
    /// </summary>
    public sealed class Combatant
    {
        public Combatant(string id, string displayName, BattleSide side, FormationSlot slot, int maxHp, int attack, int agility, int actionPriority, TargetRangeType basicRange, bool playerControlled, bool boss = false)
        {
            Id = id;
            DisplayName = displayName;
            Side = side;
            Slot = slot;
            MaxHp = Math.Max(1, maxHp);
            CurrentHp = MaxHp;
            Attack = Math.Max(1, attack);
            Agility = agility;
            ActionPriority = actionPriority;
            BasicRange = basicRange;
            IsPlayerControlled = playerControlled;
            IsBoss = boss;
        }

        public string Id { get; }
        public string DisplayName { get; }
        public BattleSide Side { get; }
        public FormationSlot Slot { get; }
        public int MaxHp { get; }
        public int CurrentHp { get; private set; }
        public int Attack { get; }
        public int Agility { get; }
        public int ActionPriority { get; }
        public TargetRangeType BasicRange { get; }
        public bool IsPlayerControlled { get; }
        public bool IsBoss { get; }
        public bool IsDefending { get; private set; }
        public Combatant ForcedTarget { get; private set; }
        public int ForcedTargetActionsRemaining { get; private set; }
        public bool IsAlive => CurrentHp > 0;

        /// <summary>방어는 선택한 시점부터 이 참가자의 다음 행동 차례가 시작될 때까지 유지됩니다.</summary>
        public void Defend() => IsDefending = true;

        /// <summary>자신의 새 행동 차례가 시작되면 이전 차례에 사용한 방어를 종료합니다.</summary>
        public void BeginTurn() => IsDefending = false;

        /// <summary>피해를 적용합니다. 방어 중이면 확정 규칙에 따라 절반으로 줄입니다.</summary>
        public int TakeDamage(int rawDamage)
        {
            int damage = Math.Max(1, rawDamage);
            if (IsDefending) damage = Math.Max(1, (int)Math.Ceiling(damage * .5f));
            CurrentHp = Math.Max(0, CurrentHp - damage);
            return damage;
        }

        /// <summary>단일 적대 행동을 지정 대상에게 강제하는 도발 상태를 기록합니다.</summary>
        public void ApplyTaunt(Combatant forcedTarget, int affectedActions = 2)
        {
            ForcedTarget = forcedTarget;
            ForcedTargetActionsRemaining = Math.Max(0, affectedActions);
        }

        /// <summary>도발에 걸린 참가자가 행동을 마칠 때 남은 행동 횟수를 줄입니다.</summary>
        public void CompleteAction()
        {
            if (ForcedTargetActionsRemaining <= 0) return;
            ForcedTargetActionsRemaining--;
            if (ForcedTargetActionsRemaining == 0) ForcedTarget = null;
        }
    }

    /// <summary>한 진영의 전열 3칸과 후열 3칸을 관리합니다.</summary>
    public sealed class Formation
    {
        private readonly Dictionary<FormationSlot, Combatant> slots = new Dictionary<FormationSlot, Combatant>();
        public Formation(BattleSide side) => Side = side;
        public BattleSide Side { get; }
        public IEnumerable<Combatant> Members => slots.Values;
        public IEnumerable<Combatant> LivingMembers => slots.Values.Where(item => item.IsAlive);
        public bool IsDefeated => !LivingMembers.Any();

        public void Place(Combatant combatant)
        {
            if (combatant == null) throw new ArgumentNullException(nameof(combatant));
            if (combatant.Side != Side) throw new InvalidOperationException("다른 진영의 전투 참가자를 배치할 수 없습니다.");
            if (slots.ContainsKey(combatant.Slot)) throw new InvalidOperationException($"이미 사용 중인 진형 슬롯입니다: {combatant.Slot}");
            slots.Add(combatant.Slot, combatant);
        }

        public Combatant Get(FormationRow row, int column)
        {
            slots.TryGetValue(new FormationSlot(row, column), out Combatant combatant);
            return combatant;
        }

        public bool Contains(Combatant combatant) => combatant != null && slots.Values.Contains(combatant);
    }

    /// <summary>도발과 근거리·원거리·마법 규칙을 한 곳에서 판정합니다.</summary>
    public static class TargetResolver
    {
        public static IReadOnlyList<Combatant> ResolveHostileTargets(Combatant actor, Formation opponents, TargetRangeType range, bool areaAttack = false)
        {
            if (actor == null || opponents == null || !actor.IsAlive) return Array.Empty<Combatant>();

            // 광역이 아닌 행동은 도발 대상을 가장 먼저 확인하며 일반 사거리보다 우선합니다.
            if (!areaAttack && actor.ForcedTargetActionsRemaining > 0 && actor.ForcedTarget != null && actor.ForcedTarget.IsAlive && opponents.Contains(actor.ForcedTarget))
                return new[] { actor.ForcedTarget };

            List<Combatant> front = Enumerable.Range(0, 3).Select(column => opponents.Get(FormationRow.Front, column)).Where(item => item != null && item.IsAlive).ToList();
            List<Combatant> rear = Enumerable.Range(0, 3).Select(column => opponents.Get(FormationRow.Rear, column)).Where(item => item != null && item.IsAlive).ToList();

            if (range == TargetRangeType.Magic) return front.Concat(rear).ToArray();
            if (range == TargetRangeType.RangedPhysical) return rear.Count > 0 ? rear : front;

            List<Combatant> melee = new List<Combatant>(front);
            for (int column = 0; column < 3; column++)
            {
                Combatant rearTarget = opponents.Get(FormationRow.Rear, column);
                Combatant protector = opponents.Get(FormationRow.Front, column);
                if (rearTarget != null && rearTarget.IsAlive && (protector == null || !protector.IsAlive)) melee.Add(rearTarget);
            }
            return melee;
        }
    }

    /// <summary>민첩과 행동 우선도로 한 라운드의 순서를 만들고 UI 타임라인에 남은 순서를 제공합니다.</summary>
    public sealed class TurnOrderQueue
    {
        private readonly List<Combatant> order = new List<Combatant>();
        private int nextIndex;
        public IReadOnlyList<Combatant> Upcoming => order.Skip(nextIndex).Where(item => item.IsAlive).ToArray();

        public void Build(IEnumerable<Combatant> combatants)
        {
            order.Clear();
            order.AddRange(combatants.Where(item => item.IsAlive)
                .OrderByDescending(item => item.ActionPriority)
                .ThenByDescending(item => item.Agility)
                .ThenBy(item => item.Id, StringComparer.Ordinal));
            nextIndex = 0;
        }

        public Combatant TakeNext(IEnumerable<Combatant> allCombatants)
        {
            while (true)
            {
                while (nextIndex < order.Count)
                {
                    Combatant candidate = order[nextIndex++];
                    if (candidate.IsAlive) return candidate;
                }
                Build(allCombatants);
                if (order.Count == 0) return null;
            }
        }
    }
}
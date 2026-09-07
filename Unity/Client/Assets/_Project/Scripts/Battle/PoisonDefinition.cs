using System;
using UnityEngine;

namespace ProjectLimitless.Battle
{
    // AttackScaling은 향후 확장 지점이며 이번에는 계산/플레이어 스킬을 구현하지 않습니다.
    public enum PoisonDamageMode { MaxHpPercent, AttackScaling }

    /// <summary>몬스터와 향후 독칼·독화살이 함께 사용할 독 데이터입니다. 이름이 아닌 Strength로 교체를 판단합니다.</summary>
    [Serializable]
    public sealed class PoisonDefinition
    {
        [SerializeField] private string id = "poison";
        [SerializeField] private string displayName = "독";
        [SerializeField] private PoisonDamageMode damageMode = PoisonDamageMode.MaxHpPercent;
        [SerializeField, Min(1)] private int power = 5;
        [SerializeField, Min(1)] private int strength = 5;

        public PoisonDefinition() { }
        public PoisonDefinition(string id, string displayName, int power, int strength)
        { this.id = id; this.displayName = displayName; this.power = power; this.strength = strength; }
        public string Id => id;
        public string DisplayName => displayName;
        public PoisonDamageMode DamageMode => damageMode;
        public int Power => Math.Max(1, power);
        public int Strength => Math.Max(1, strength);
    }
}

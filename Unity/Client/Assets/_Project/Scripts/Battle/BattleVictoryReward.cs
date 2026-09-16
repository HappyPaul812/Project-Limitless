using System;
using System.Collections.Generic;
using System.Linq;
using ProjectLimitless.Core;
using ProjectLimitless.Monster;
using UnityEngine;

namespace ProjectLimitless.Battle
{
    /// <summary>확률 판정을 교체할 수 있게 하여 실제 게임은 Unity 난수를, Audit은 고정 난수를 사용합니다.</summary>
    public interface IBattleRewardRandom
    {
        float NextValue();
        int RangeInclusive(int minimum, int maximum);
    }

    public sealed class UnityBattleRewardRandom : IBattleRewardRandom
    {
        public float NextValue() => UnityEngine.Random.value;
        public int RangeInclusive(int minimum, int maximum) => UnityEngine.Random.Range(minimum, maximum + 1);
    }

    /// <summary>
    /// 승리한 한 전투의 EXP·탈렌트·전리품을 한 번만 계산합니다.
    /// UI와 저장이 몬스터 목록을 다시 순회하지 않게 해야 화면과 실제 보유 값이 항상 같습니다.
    /// </summary>
    public static class BattleVictoryReward
    {
        public static RewardBundle Calculate(bool victory, int playerLevel, IEnumerable<Combatant> enemies,
            IReadOnlyDictionary<Combatant, BattleParticipantSetup> setups, IBattleRewardRandom random = null)
        {
            var result = new RewardBundle { Items = Array.Empty<ItemReward>() };
            if (!victory || enemies == null || setups == null) return result;

            random ??= new UnityBattleRewardRandom();
            var defeated = new List<Combatant>();
            var seen = new HashSet<Combatant>(CombatantReferenceComparer.Instance);
            foreach (Combatant enemy in enemies)
                if (enemy != null && !enemy.IsAlive && enemy.Side == BattleSide.Enemies && seen.Add(enemy)
                    && setups.TryGetValue(enemy, out BattleParticipantSetup setup) && setup.MonsterDefinition != null)
                    defeated.Add(enemy);

            // 기존 EXP 계산기를 그대로 사용해 레벨 차이 배율·반올림·만렙 규칙을 보존합니다.
            result.Experience = BattleExperienceReward.Calculate(true, playerLevel, defeated, setups);
            long currency = 0;
            var itemCounts = new Dictionary<string, int>(StringComparer.Ordinal);
            foreach (Combatant enemy in defeated)
            {
                MonsterDefinition definition = setups[enemy].MonsterDefinition;
                // EXP 배율은 성장 속도만 조절합니다. 탈렌트와 전리품은 회색 몬스터도 데이터 그대로 지급합니다.
                currency = Math.Min(int.MaxValue, currency + definition.CurrencyReward);
                foreach (MonsterLootEntry loot in definition.LootEntries)
                {
                    if (loot == null || string.IsNullOrWhiteSpace(loot.ItemId) || loot.DropChance <= 0f
                        || random.NextValue() >= loot.DropChance) continue;
                    int count = random.RangeInclusive(loot.MinCount, loot.MaxCount);
                    if (count <= 0) continue;
                    itemCounts.TryGetValue(loot.ItemId, out int current);
                    itemCounts[loot.ItemId] = (int)Math.Min(int.MaxValue, (long)current + count);
                }
            }

            result.Currency = (int)currency;
            // 같은 ItemId를 여기서 합산해 Inventory와 Victory UI가 한 줄씩 같은 수량을 사용합니다.
            result.Items = itemCounts.OrderBy(pair => pair.Key, StringComparer.Ordinal)
                .Select(pair => new ItemReward { ItemId = pair.Key, Count = pair.Value }).ToArray();
            return result;
        }
    }
}

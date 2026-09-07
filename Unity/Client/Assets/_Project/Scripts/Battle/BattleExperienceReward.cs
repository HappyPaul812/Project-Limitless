using System;
using System.Collections.Generic;
using ProjectLimitless.Core;

namespace ProjectLimitless.Battle
{
    /// <summary>전투 고정 보상 대신 처치한 실제 몬스터마다 보상을 계산합니다. 동일 정의의 두 벌도 각각 지급합니다.</summary>
    public static class BattleExperienceReward
    {
        public static int Calculate(bool victory, int playerLevel,
            IEnumerable<Combatant> enemies, IReadOnlyDictionary<Combatant, BattleParticipantSetup> setups)
        {
            if (!victory || enemies == null || setups == null) return 0;
            long total = 0;
            var seen = new HashSet<Combatant>(CombatantReferenceComparer.Instance);
            foreach (Combatant enemy in enemies)
            {
                // 같은 개체가 목록에 중복되면 한 번만, 같은 Asset을 공유하는 별개 개체는 각각 집계합니다.
                if (enemy == null || enemy.IsAlive || enemy.Side != BattleSide.Enemies || !seen.Add(enemy)
                    || !setups.TryGetValue(enemy, out BattleParticipantSetup setup) || setup.MonsterDefinition == null) continue;
                total += ExperienceProgression.MonsterExperience(setup.MonsterDefinition.BaseExperience,
                    playerLevel, setup.MonsterDefinition.MonsterLevel);
            }
            return (int)Math.Min(int.MaxValue, total);
        }
    }
}

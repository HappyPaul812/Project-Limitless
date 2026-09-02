using System;

namespace ProjectLimitless.Core
{
    /// <summary>
    /// 레벨과 직업으로부터 기본 능력치와 전투용 파생 능력치를 다시 계산합니다.
    /// 저장 파일에는 Level과 JobId만 남기므로 슬롯을 바꾸거나 다시 불러와도 같은 결과가 나옵니다.
    /// 길(Path)은 향후 별도 패시브를 담당하므로 이 계산에는 넣지 않습니다.
    /// </summary>
    public static class CharacterGrowthCalculator
    {
        public const int MaxLevel = 50;
        public const int BaseStatValue = 10;
        public const int VitalityHpCoefficient = 2;
        public const int MainStatAttackCoefficient = 2;
        public const float GuardianWillAttackCoefficient = .5f;
        public const float GuardianWillDefenseCoefficient = 1.5f;
        public const float HealerWillAttackCoefficient = .5f;
        public const float HealerWillHealingCoefficient = 1.5f;

        // 아래 MP 수치는 기능 검증용 임시 밸런스입니다. 확정 전에는 이 한 곳만 조정합니다.
        public const int TemporaryHealerBaseMp = 20;
        public const int TemporaryHealerMpPerIntelligence = 2;
        public const int TemporaryHealerBaseMpRecovery = 2;
        public const int TemporaryHealerIntelligencePerRecovery = 10;
        public const int TemporaryHealingLightMpCost = 6;
        public const int TemporaryHealingWaveMpCost = 10;
        public const int TemporaryCleanseMpCost = 5;

        public static CharacterGrowthStats Calculate(string jobId, int requestedLevel)
        {
            int level = Math.Max(1, Math.Min(MaxLevel, requestedLevel));
            int gainedLevels = level - 1;
            CharacterGrowthStats result = new CharacterGrowthStats(level, BaseStatValue);
            ApplyStartingBonus(jobId, ref result);

            // 모든 능력치는 레벨마다 1씩 자랍니다. 딜러 주 스탯은 1,2,1,2의 결정적 추가 패턴으로
            // 평균 +1.5가 되며 float 저장이나 무작위가 필요 없습니다.
            CharacterStatType? mainStat = GetDealerMainStat(jobId);
            // 이번에 레벨 성장 수치가 확정된 직업은 딜러 3종뿐입니다. 수호자·치유사의 6능력치
            // 레벨 성장은 임의로 만들지 않고, 확정된 직업 시작 보너스와 HP 성장만 적용합니다.
            if (mainStat.HasValue)
            {
                result.AddToAll(gainedLevels);
                result.Add(mainStat.Value, gainedLevels / 2);
            }
            return result;
        }

        public static int CalculateAttack(string jobId, CharacterGrowthStats stats, int baseAttack = 12)
        {
            float bonus = 0f;
            CharacterStatType? mainStat = GetDealerMainStat(jobId);
            if (mainStat.HasValue) bonus = stats.Get(mainStat.Value) * MainStatAttackCoefficient;
            else if (jobId == "guardian") bonus = stats.Willpower * GuardianWillAttackCoefficient;
            else if (jobId == "healer") bonus = stats.Willpower * HealerWillAttackCoefficient;
            return Math.Max(1, RoundCombatValue(baseAttack + bonus));
        }

        public static int CalculateDefense(string jobId, CharacterGrowthStats stats, int baseDefense = 0) =>
            Math.Max(0, RoundCombatValue(baseDefense + (jobId == "guardian" ? stats.Willpower * GuardianWillDefenseCoefficient : 0f)));

        public static int CalculateHealingPower(string jobId, CharacterGrowthStats stats) =>
            jobId == "healer" ? Math.Max(0, RoundCombatValue(stats.Willpower * HealerWillHealingCoefficient)) : 0;

        public static int CalculateMaxHp(string jobId, CharacterGrowthStats stats, int baseHp = 80)
        {
            int growth = GetHpGrowthPerLevel(jobId);
            return Math.Max(1, baseHp + (stats.Level - 1) * growth + stats.Health * VitalityHpCoefficient);
        }

        public static bool UsesMp(string jobId) => jobId == "healer";
        public static int CalculateMaxMp(string jobId, CharacterGrowthStats stats) => UsesMp(jobId)
            ? TemporaryHealerBaseMp + stats.Intelligence * TemporaryHealerMpPerIntelligence : 0;
        public static int CalculateMpRecovery(string jobId, CharacterGrowthStats stats) => UsesMp(jobId)
            ? TemporaryHealerBaseMpRecovery + stats.Intelligence / TemporaryHealerIntelligencePerRecovery : 0;

        public static int GetHpGrowthPerLevel(string jobId) => jobId == "guardian" ? 5 : jobId == "fighter" ? 4 :
            (jobId == "healer" || jobId == "sharpshooter") ? 3 : jobId == "mage" ? 2 : 0;

        // MidpointAwayFromZero를 공통 정책으로 사용해 .5가 직업마다 위·아래로 흔들리지 않게 합니다.
        public static int RoundCombatValue(float value) => (int)Math.Round(value, MidpointRounding.AwayFromZero);

        private static CharacterStatType? GetDealerMainStat(string jobId)
        {
            if (jobId == "fighter") return CharacterStatType.Strength;
            if (jobId == "sharpshooter") return CharacterStatType.Sense;
            if (jobId == "mage") return CharacterStatType.Intelligence;
            return null;
        }

        private static void ApplyStartingBonus(string jobId, ref CharacterGrowthStats stats)
        {
            if (jobId == "guardian") { stats.Health += 2; stats.Willpower += 2; }
            else if (jobId == "healer") { stats.Willpower += 2; stats.Intelligence += 2; }
            else if (jobId == "sharpshooter") { stats.Sense += 2; stats.Agility += 2; }
            else if (jobId == "fighter") { stats.Strength += 2; stats.Agility += 2; }
            else if (jobId == "mage") { stats.Intelligence += 2; stats.Sense += 2; }
        }
    }

    public struct CharacterGrowthStats
    {
        public CharacterGrowthStats(int level, int baseValue)
        { Level = level; Strength = Health = Agility = Intelligence = Willpower = Sense = baseValue; }
        public int Level;
        public int Strength;
        public int Health;
        public int Agility;
        public int Intelligence;
        public int Willpower;
        public int Sense;
        public int Get(CharacterStatType stat) => stat == CharacterStatType.Strength ? Strength :
            stat == CharacterStatType.Health ? Health : stat == CharacterStatType.Agility ? Agility :
            stat == CharacterStatType.Intelligence ? Intelligence : stat == CharacterStatType.Willpower ? Willpower : Sense;
        public void Add(CharacterStatType stat, int amount)
        {
            if (stat == CharacterStatType.Strength) Strength += amount; else if (stat == CharacterStatType.Health) Health += amount;
            else if (stat == CharacterStatType.Agility) Agility += amount; else if (stat == CharacterStatType.Intelligence) Intelligence += amount;
            else if (stat == CharacterStatType.Willpower) Willpower += amount; else Sense += amount;
        }
        public void AddToAll(int amount)
        { Strength += amount; Health += amount; Agility += amount; Intelligence += amount; Willpower += amount; Sense += amount; }
    }
}

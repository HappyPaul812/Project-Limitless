using System;
using UnityEngine;

namespace ProjectLimitless.Core
{
    /// <summary>이름색과 보상이 같은 레벨 차이 경계를 사용하도록 공유하는 판정입니다.</summary>
    public enum LevelDifferenceCategory { Gray, Green, Yellow, Orange, Red }

    /// <summary>현재 레벨 안의 경험치 진행과 보상 규칙을 중앙 관리합니다. 능력치 성장 공식은 변경하지 않습니다.</summary>
    public static class ExperienceProgression
    {
        public static int RequiredExp(int level)
        {
            if (level >= CharacterGrowthCalculator.MaxLevel) return 0;
            int gained = Math.Max(1, level) - 1;
            return 100 + 25 * gained + 5 * gained * gained;
        }

        public static LevelDifferenceCategory GetCategory(int playerLevel, int monsterLevel)
        {
            long difference = (long)monsterLevel - playerLevel;
            return difference <= -7 ? LevelDifferenceCategory.Gray : difference <= -4 ? LevelDifferenceCategory.Green
                : difference <= 3 ? LevelDifferenceCategory.Yellow : difference <= 6 ? LevelDifferenceCategory.Orange
                : LevelDifferenceCategory.Red;
        }

        public static int GetExperiencePercent(LevelDifferenceCategory category)
        {
            // 충분히 낮은 몬스터 반복 사냥만으로 성장하지 않도록 회색 보상은 0입니다.
            switch (category)
            {
                case LevelDifferenceCategory.Gray: return 0;
                case LevelDifferenceCategory.Green: return 50;
                case LevelDifferenceCategory.Orange: return 125;
                case LevelDifferenceCategory.Red: return 150;
                default: return 100;
            }
        }

        public static Color GetNameColor(LevelDifferenceCategory category)
        {
            switch (category)
            {
                case LevelDifferenceCategory.Gray: return new Color(.6f, .6f, .6f, 1f);
                case LevelDifferenceCategory.Green: return new Color(.3f, .85f, .35f, 1f);
                case LevelDifferenceCategory.Orange: return new Color(1f, .55f, .1f, 1f);
                case LevelDifferenceCategory.Red: return new Color(1f, .25f, .25f, 1f);
                default: return new Color(1f, .9f, .2f, 1f);
            }
        }

        public static int MonsterExperience(int baseExperience, int playerLevel, int monsterLevel)
        {
            long scaled = (long)Math.Max(0, baseExperience) * GetExperiencePercent(GetCategory(playerLevel, monsterLevel));
            // 몬스터 한 마리마다 소수 첫째 자리에서 반올림(.5는 위로)한 뒤 합산합니다.
            // 정수 long 연산으로 큰 입력의 곱셈 오버플로와 float 오차를 피합니다.
            return (int)Math.Min(int.MaxValue, (scaled + 50L) / 100L);
        }

        public static ExperienceGain Add(int level, int currentExperience, int reward)
        {
            int before = Math.Max(1, Math.Min(CharacterGrowthCalculator.MaxLevel, level));
            level = before;
            // CurrentExperience는 평생 누적 EXP가 아닙니다. 레벨업 비용을 빼고 나머지를 다음 레벨로
            // 넘겨 한 번에 큰 보상을 받아도 손해가 없고 여러 레벨을 연속으로 올릴 수 있습니다.
            long remaining = (long)Math.Max(0, currentExperience) + Math.Max(0, reward);
            while (level < CharacterGrowthCalculator.MaxLevel && remaining >= RequiredExp(level))
            {
                remaining -= RequiredExp(level);
                level++;
            }
            // 만렙에는 다음 구간이 없으므로 진행 EXP는 0으로 정규화합니다.
            return new ExperienceGain(before, level, level == CharacterGrowthCalculator.MaxLevel ? 0 : (int)remaining);
        }
    }

    /// <summary>순수 계산 결과입니다. 저장과 UI는 이 결과를 읽으며 HP/MP 회복 규칙을 추가하지 않습니다.</summary>
    public readonly struct ExperienceGain
    {
        public ExperienceGain(int previousLevel, int level, int currentExperience)
        { PreviousLevel = previousLevel; Level = level; CurrentExperience = currentExperience; }
        public int PreviousLevel { get; }
        public int Level { get; }
        public int CurrentExperience { get; }
        public bool LeveledUp => Level > PreviousLevel;
    }
}

namespace ProjectLimitless.Core
{
    /// <summary>캐릭터 생성 화면들이 동일한 최종 능력치 미리보기 계산을 사용하게 합니다.</summary>
    public static class CharacterCreationStatsCalculator
    {
        public const int BaseStatValue = 10;

        public static int GetFinalStat(PlayerPathDefinition path, JobDefinition job, CharacterStatType stat)
        {
            int value = path == null ? BaseStatValue : path.GetPreviewStat(stat, BaseStatValue);
            return job == null ? value : job.ApplyStatBonus(stat, value);
        }
    }
}

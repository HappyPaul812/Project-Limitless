using System;

namespace ProjectLimitless.Core
{
    [Serializable] public sealed class ItemReward { public string ItemId = string.Empty; public int Count; }
    [Serializable] public sealed class RewardBundle
    {
        public int Experience;
        public int Currency;
        public ItemReward[] Items;

        /// <summary>
        /// EXP와 탈렌트를 먼저 확정하고, 전리품은 들어가는 수량만 안전하게 지급합니다.
        /// 향후 가방 용량이 생겨 일부 전리품을 못 받아도 이미 얻은 성장과 화폐까지 사라지지 않습니다.
        /// 반환 Bundle은 실제 지급분이므로 Victory UI와 Save가 같은 결과를 사용합니다.
        /// </summary>
        public RewardApplicationResult ApplyBestEffort()
        {
            ExperienceGain experienceGain = ExperienceProgression.Add(
                GameSessionData.Level, GameSessionData.CurrentExperience, Math.Max(0, Experience));
            GameSessionData.ConfigureProgress(experienceGain.Level, experienceGain.CurrentExperience);

            int appliedCurrency = EconomyService.AddCurrency(Math.Max(0, Currency)) ? Math.Max(0, Currency) : 0;
            var appliedItems = new System.Collections.Generic.List<ItemReward>();
            if (Items != null)
            {
                foreach (ItemReward item in Items)
                {
                    if (item == null || item.Count <= 0) continue;
                    int addable = Math.Min(item.Count, InventoryService.GetAddableAmount(item.ItemId));
                    if (addable > 0 && InventoryService.TryAddItem(item.ItemId, addable))
                        appliedItems.Add(new ItemReward { ItemId = item.ItemId, Count = addable });
                }
            }

            return new RewardApplicationResult(experienceGain, new RewardBundle
            {
                Experience = Math.Max(0, Experience),
                Currency = appliedCurrency,
                Items = appliedItems.ToArray()
            });
        }

        /// <summary>퀘스트와 몬스터가 같은 사전 검증을 거쳐 일부 보상만 지급되는 상태를 막습니다.</summary>
        public bool TryApply()
        {
            if (Experience < 0 || Currency < 0 || EconomyService.GetCurrency() > int.MaxValue - Currency) return false;
            if (Items != null)
                foreach (ItemReward item in Items)
                    if (item == null || !InventoryService.CanAddItem(item.ItemId, item.Count)) return false;
            EconomyService.AddCurrency(Currency);
            if (Items != null) foreach (ItemReward item in Items) InventoryService.TryAddItem(item.ItemId, item.Count);
            if (Experience > 0)
            {
                ExperienceGain result = ExperienceProgression.Add(GameSessionData.Level, GameSessionData.CurrentExperience, Experience);
                GameSessionData.ConfigureProgress(result.Level, result.CurrentExperience);
            }
            return true;
        }
    }

    public sealed class RewardApplicationResult
    {
        public RewardApplicationResult(ExperienceGain experienceGain, RewardBundle appliedReward)
        { ExperienceGain = experienceGain; AppliedReward = appliedReward; }

        public ExperienceGain ExperienceGain { get; }
        public RewardBundle AppliedReward { get; }
    }
}

using System;

namespace ProjectLimitless.Core
{
    [Serializable] public sealed class ItemReward { public string ItemId = string.Empty; public int Count; }
    [Serializable] public sealed class RewardBundle
    {
        public int Experience;
        public int Currency;
        public ItemReward[] Items;

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
}

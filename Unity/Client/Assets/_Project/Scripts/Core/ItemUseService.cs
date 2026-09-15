namespace ProjectLimitless.Core
{
    public enum ItemUseResult
    {
        Success,
        ItemNotFound,
        ItemNotOwned,
        NotUsableInWorld,
        InvalidEffect,
        InvalidTarget,
        TargetKnockedOut,
        TargetDoesNotUseMp,
        AlreadyFull
    }

    public readonly struct ItemUseOutcome
    {
        public ItemUseOutcome(ItemUseResult result, int actualAmount = 0)
        { Result = result; ActualAmount = actualAmount; }
        public ItemUseResult Result { get; }
        public int ActualAmount { get; }
        public bool Succeeded => Result == ItemUseResult.Success;
    }

    /// <summary>월드 UI와 향후 다른 진입점이 같은 검증·효과·소비 순서를 공유합니다.</summary>
    public static class ItemUseService
    {
        public static ItemUseOutcome CanUse(string itemId, string targetCharacterId)
        {
            if (!ItemCatalog.TryGet(itemId, out ItemDefinition item)) return new ItemUseOutcome(ItemUseResult.ItemNotFound);
            if (!InventoryService.HasItem(itemId, 1)) return new ItemUseOutcome(ItemUseResult.ItemNotOwned);
            if (item.Category != ItemCategory.Consumable
                || (item.UseType != ItemUseType.World && item.UseType != ItemUseType.WorldAndBattle))
                return new ItemUseOutcome(ItemUseResult.NotUsableInWorld);
            if (item.EffectType == ItemEffectType.None || item.EffectAmount <= 0)
                return new ItemUseOutcome(ItemUseResult.InvalidEffect);
            if (!PartyResourceService.TryGet(targetCharacterId, out PartyMemberResourceSnapshot target))
                return new ItemUseOutcome(ItemUseResult.InvalidTarget);
            if (target.CurrentHp <= 0) return new ItemUseOutcome(ItemUseResult.TargetKnockedOut);

            if (item.EffectType == ItemEffectType.RecoverHp)
                return new ItemUseOutcome(target.CurrentHp >= target.MaxHp ? ItemUseResult.AlreadyFull : ItemUseResult.Success);
            if (item.EffectType == ItemEffectType.RecoverMp)
            {
                if (target.MaxMp <= 0) return new ItemUseOutcome(ItemUseResult.TargetDoesNotUseMp);
                return new ItemUseOutcome(target.CurrentMp >= target.MaxMp ? ItemUseResult.AlreadyFull : ItemUseResult.Success);
            }
            return new ItemUseOutcome(ItemUseResult.InvalidEffect);
        }

        public static ItemUseOutcome TryUse(string itemId, string targetCharacterId)
        {
            ItemUseOutcome validation = CanUse(itemId, targetCharacterId);
            if (!validation.Succeeded) return validation;
            ItemCatalog.TryGet(itemId, out ItemDefinition item);
            if (!PartyResourceService.TryRecover(targetCharacterId, item.EffectType, item.EffectAmount, out int actualAmount))
                return new ItemUseOutcome(ItemUseResult.InvalidTarget);

            // CanUse 이후 같은 프레임에서 보유 수량이 유지되는 단일 메인 스레드 흐름입니다.
            // 방어적으로 소비가 실패하면 예외를 던져 조용한 자원 복제를 허용하지 않습니다.
            if (!InventoryService.TryRemoveItem(itemId, 1))
                throw new System.InvalidOperationException("아이템 효과 적용 뒤 소비에 실패했습니다.");
            return new ItemUseOutcome(ItemUseResult.Success, actualAmount);
        }
    }
}

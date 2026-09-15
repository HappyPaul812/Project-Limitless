namespace ProjectLimitless.Core
{
    public enum ShopTransactionResult { Success, InvalidItem, InsufficientCurrency, InventoryFull, NoItem, CurrencyOverflow }

    /// <summary>
    /// 구매·판매 규칙을 UI 밖에 둔 공용 거래 서비스입니다. 먼저 모든 조건을 검사해야
    /// 돈만 빠지거나 아이템만 사라지는 중간 상태를 만들지 않습니다.
    /// </summary>
    public static class ShopService
    {
        public static ShopTransactionResult TryBuy(string itemId)
        {
            if (!ItemCatalog.TryGet(itemId, out ItemDefinition item)) return ShopTransactionResult.InvalidItem;
            // 가격은 상점 코드가 아니라 ItemDefinition에서 읽어 밸런스 조정을 데이터 변경으로 끝냅니다.
            if (!EconomyService.CanAfford(item.BuyPrice)) return ShopTransactionResult.InsufficientCurrency;
            if (!InventoryService.CanAddItem(itemId, 1)) return ShopTransactionResult.InventoryFull;

            // 위 검증 뒤에는 두 작업이 모두 성공할 수 있습니다. 예상 밖 실패에는 즉시 원상복구합니다.
            if (!EconomyService.TrySpendCurrency(item.BuyPrice)) return ShopTransactionResult.InsufficientCurrency;
            if (InventoryService.TryAddItem(itemId, 1)) return ShopTransactionResult.Success;
            EconomyService.AddCurrency(item.BuyPrice);
            return ShopTransactionResult.InventoryFull;
        }

        public static ShopTransactionResult TrySell(string itemId)
        {
            if (!ItemCatalog.TryGet(itemId, out ItemDefinition item)) return ShopTransactionResult.InvalidItem;
            if (!InventoryService.HasItem(itemId, 1)) return ShopTransactionResult.NoItem;
            if (EconomyService.GetCurrency() > int.MaxValue - item.SellPrice) return ShopTransactionResult.CurrencyOverflow;

            if (!InventoryService.TryRemoveItem(itemId, 1)) return ShopTransactionResult.NoItem;
            if (EconomyService.AddCurrency(item.SellPrice)) return ShopTransactionResult.Success;
            InventoryService.TryAddItem(itemId, 1);
            return ShopTransactionResult.CurrencyOverflow;
        }
    }
}

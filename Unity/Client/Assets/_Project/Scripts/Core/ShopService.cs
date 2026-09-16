namespace ProjectLimitless.Core
{
    public enum ShopTransactionResult { Success, InvalidItem, InsufficientCurrency, InventoryFull, NoItem, CurrencyOverflow }

    /// <summary>
    /// 구매·판매 규칙을 UI 밖에 둔 공용 거래 서비스입니다. 먼저 모든 조건을 검사해야
    /// 돈만 빠지거나 아이템만 사라지는 중간 상태를 만들지 않습니다.
    /// </summary>
    public static class ShopService
    {
        public static int GetMaximumBuyQuantity(ShopDefinition shop, string itemId)
        {
            if (!IsSoldBy(shop, itemId) || !ItemCatalog.TryGet(itemId, out ItemDefinition item)) return 0;
            int affordable = item.BuyPrice == 0 ? int.MaxValue : EconomyService.GetCurrency() / item.BuyPrice;
            return System.Math.Min(affordable, InventoryService.GetAddableAmount(itemId));
        }

        public static int GetMaximumSellQuantity(string itemId) =>
            ItemCatalog.TryGet(itemId, out ItemDefinition item) && IsSellable(item)
                ? InventoryService.GetItemCount(itemId)
                : 0;

        /// <summary>판매 목록과 실제 거래가 같은 규칙을 사용하도록 공용 판정을 제공합니다.</summary>
        public static bool IsSellable(ItemDefinition item) => item != null
            && item.SellPrice > 0
            && item.Category != ItemCategory.Quest
            && item.Category != ItemCategory.KeyItem;

        public static ShopTransactionResult TryBuy(ShopDefinition shop, string itemId, int quantity)
        {
            if (quantity <= 0 || !IsSoldBy(shop, itemId) || !ItemCatalog.TryGet(itemId, out ItemDefinition item))
                return ShopTransactionResult.InvalidItem;
            // 총 가격을 거래 전에 long으로 계산해 int 곱셈 overflow와 부분 적용을 함께 막습니다.
            if (!TryCalculateTotal(item.BuyPrice, quantity, out int total)) return ShopTransactionResult.CurrencyOverflow;
            if (!EconomyService.CanAfford(total)) return ShopTransactionResult.InsufficientCurrency;
            if (!InventoryService.CanAddItem(itemId, quantity)) return ShopTransactionResult.InventoryFull;

            // 수량별 거래를 공용 서비스에서 처리해야 모든 Shop UI가 같은 원자성 규칙을 공유합니다.
            if (!EconomyService.TrySpendCurrency(total)) return ShopTransactionResult.InsufficientCurrency;
            if (InventoryService.TryAddItem(itemId, quantity)) return ShopTransactionResult.Success;
            EconomyService.AddCurrency(total);
            return ShopTransactionResult.InventoryFull;
        }

        public static ShopTransactionResult TrySell(string itemId, int quantity)
        {
            if (quantity <= 0 || !ItemCatalog.TryGet(itemId, out ItemDefinition item) || !IsSellable(item))
                return ShopTransactionResult.InvalidItem;
            if (!InventoryService.HasItem(itemId, quantity)) return ShopTransactionResult.NoItem;
            if (!TryCalculateTotal(item.SellPrice, quantity, out int total)
                || EconomyService.GetCurrency() > int.MaxValue - total) return ShopTransactionResult.CurrencyOverflow;

            if (!InventoryService.TryRemoveItem(itemId, quantity)) return ShopTransactionResult.NoItem;
            if (EconomyService.AddCurrency(total)) return ShopTransactionResult.Success;
            InventoryService.TryAddItem(itemId, quantity);
            return ShopTransactionResult.CurrencyOverflow;
        }

        public static bool TryCalculateTotal(int unitPrice, int quantity, out int total)
        {
            long calculated = (long)unitPrice * quantity;
            total = calculated >= 0 && calculated <= int.MaxValue ? (int)calculated : 0;
            return quantity > 0 && unitPrice >= 0 && calculated <= int.MaxValue;
        }

        private static bool IsSoldBy(ShopDefinition shop, string itemId)
        {
            if (shop == null || string.IsNullOrEmpty(itemId)) return false;
            for (int i = 0; i < shop.ItemIds.Count; i++) if (shop.ItemIds[i] == itemId) return true;
            return false;
        }
    }
}

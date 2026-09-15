using System;
using System.Collections.Generic;
using System.Linq;
using ProjectLimitless.Core;
using UnityEditor;
using UnityEngine;

namespace ProjectLimitless.Editor
{
    public static class EconomyInventoryAudit
    {
        [MenuItem("Project Limitless/Test/Economy Inventory Audit")]
        private static void Audit()
        {
            Check(CurrencyPresentation.DisplayName == "탈렌트", "화폐 표시명");
            Check(CurrencyPresentation.Icon != null, "공용 화폐 아이콘");
            ItemDefinition[] assets = Resources.LoadAll<ItemDefinition>("ItemDefinitions");
            Check(assets.All(x => x != null && !string.IsNullOrWhiteSpace(x.ItemId)), "빈 ItemId 없음");
            Check(assets.Select(x => x.ItemId).Distinct(StringComparer.Ordinal).Count() == assets.Length, "중복 ItemId 없음");
            Check(assets.All(x => x.MaxStack > 0 && x.BuyPrice >= 0 && x.SellPrice >= 0), "가격/MaxStack 유효");
            Check(assets.All(x => x.SellPrice <= x.BuyPrice), "판매가가 구매가 이하");
            Check(assets.Where(x => x.Category == ItemCategory.Consumable).All(x => x.UseType != ItemUseType.None), "Consumable UseType 지정");
            Check(assets.Where(x => x.EffectType == ItemEffectType.RecoverHp || x.EffectType == ItemEffectType.RecoverMp)
                .All(x => x.EffectAmount > 0), "회복 효과량 양수");

            ShopDefinition[] shops = Resources.LoadAll<ShopDefinition>("ShopDefinitions");
            Check(shops.Any(x => x.ShopId == "shop_general_starter_village"), "시작 마을 잡화상 데이터");
            foreach (ShopDefinition shop in shops)
            {
                Check(!string.IsNullOrWhiteSpace(shop.ShopId), "빈 ShopId 없음");
                Check(shop.ItemIds.Distinct(StringComparer.Ordinal).Count() == shop.ItemIds.Count, "중복 Shop Item 없음");
                Check(shop.ItemIds.All(id => ItemCatalog.TryGet(id, out _)), "Shop ItemId가 Catalog에 존재");
            }

            ItemDefinition testItem = ScriptableObject.CreateInstance<ItemDefinition>();
            testItem.ConfigureForAudit("audit_item", 99);
            ItemCatalog.RegisterForAudit(testItem);
            EconomyService.Reset(); InventoryService.Reset();
            Check(EconomyService.GetCurrency() == 0 && EconomyService.AddCurrency(100) && EconomyService.AddCurrency(25), "화폐 0→125");
            Check(!EconomyService.TrySpendCurrency(200) && EconomyService.GetCurrency() == 125, "부족 지출 원자성");
            Check(EconomyService.TrySpendCurrency(20) && EconomyService.GetCurrency() == 105, "화폐 20 지출");
            Check(InventoryService.GetItemCount("audit_item") == 0 && InventoryService.TryAddItem("audit_item", 3), "아이템 0→3");
            Check(InventoryService.TryRemoveItem("audit_item", 1) && InventoryService.GetItemCount("audit_item") == 2, "아이템 3→2");
            Check(!InventoryService.TryRemoveItem("audit_item", 3) && InventoryService.GetItemCount("audit_item") == 2, "초과 제거 원자성");

            var save = new GameSaveData { Currency = 105, Inventory = InventoryService.ExportSaveData() };
            GameSaveData roundTrip = JsonUtility.FromJson<GameSaveData>(JsonUtility.ToJson(save, true));
            EconomyService.Reset(); InventoryService.Reset();
            EconomyService.Import(roundTrip.Currency); InventoryService.ImportSaveData(roundTrip.Inventory);
            Check(EconomyService.GetCurrency() == 105 && InventoryService.GetItemCount("audit_item") == 2, "Save JSON 왕복");
            GameSaveData legacy = JsonUtility.FromJson<GameSaveData>("{\"Version\":1,\"Level\":4,\"CurrentExperience\":42}");
            EconomyService.Import(legacy.Currency); InventoryService.ImportSaveData(legacy.Inventory);
            Check(EconomyService.GetCurrency() == 0 && InventoryService.ExportSaveData().Length == 0
                && legacy.Level == 4 && legacy.CurrentExperience == 42, "구버전 누락 필드 호환");
            Check(InventoryService.ExportSaveData().All(x => x.Count > 0), "음수 Count 없음");
            UnityEngine.Object.DestroyImmediate(testItem);
            ItemCatalog.ReloadForAudit();
            EconomyService.Reset(); InventoryService.Reset();

            EconomyService.AddCurrency(100);
            ShopDefinition starterShop = shops.First(x => x.ShopId == "shop_general_starter_village");
            EconomyService.Reset(); InventoryService.Reset(); EconomyService.AddCurrency(200);
            Check(ShopService.TryBuy(starterShop, "item_healing_potion_small", 3) == ShopTransactionResult.Success
                && EconomyService.GetCurrency() == 140 && InventoryService.GetItemCount("item_healing_potion_small") == 3,
                "상점 회복약 3개 구매 200→140, 0→3");
            Check(ShopService.TryBuy(starterShop, "item_mana_potion_small", 4) == ShopTransactionResult.Success
                && EconomyService.GetCurrency() == 20 && InventoryService.GetItemCount("item_mana_potion_small") == 4,
                "상점 마력 회복약 4개 구매 140→20, 0→4");
            Check(ShopService.GetMaximumBuyQuantity(starterShop, "item_mana_potion_small") == 0, "20 탈렌트에서 마력 회복약 최대 0");
            int currencyBeforeFailure = EconomyService.GetCurrency();
            int manaBeforeFailure = InventoryService.GetItemCount("item_mana_potion_small");
            Check(ShopService.TryBuy(starterShop, "item_mana_potion_small", 1) == ShopTransactionResult.InsufficientCurrency
                && EconomyService.GetCurrency() == currencyBeforeFailure
                && InventoryService.GetItemCount("item_mana_potion_small") == manaBeforeFailure, "돈 부족 구매 원자성");
            Check(ShopService.TrySell("item_healing_potion_small", 2) == ShopTransactionResult.Success
                && EconomyService.GetCurrency() == 40 && InventoryService.GetItemCount("item_healing_potion_small") == 1,
                "상점 회복약 2개 판매 20→40, 3→1");
            Check(ShopService.GetMaximumSellQuantity("item_healing_potion_small") == 1
                && ShopService.TrySell("item_healing_potion_small", 2) == ShopTransactionResult.NoItem
                && InventoryService.GetItemCount("item_healing_potion_small") == 1, "보유 수량 초과 판매 방어");
            Check(!ShopService.TryCalculateTotal(int.MaxValue, 2, out _), "가격×수량 overflow 방어");
            InventoryService.Reset();
            PartyResourceService.Reset();
            PartyResourceService.ResolveForBattle("audit_hp", 104, 0);
            PartyResourceService.RecordBattleResult("audit_hp", 50, 0, 104, 0);
            InventoryService.TryAddItem("item_healing_potion_small", 3);
            ItemUseOutcome hpUse = ItemUseService.TryUse("item_healing_potion_small", "audit_hp");
            Check(hpUse.Succeeded && hpUse.ActualAmount == 30
                && PartyResourceService.TryGet("audit_hp", out PartyMemberResourceSnapshot hpAfter) && hpAfter.CurrentHp == 80
                && InventoryService.GetItemCount("item_healing_potion_small") == 2, "회복약 50/104→80/104, 3→2");
            PartyResourceService.RecordBattleResult("audit_hp", 90, 0, 104, 0);
            hpUse = ItemUseService.TryUse("item_healing_potion_small", "audit_hp");
            Check(hpUse.Succeeded && hpUse.ActualAmount == 14
                && PartyResourceService.TryGet("audit_hp", out hpAfter) && hpAfter.CurrentHp == 104, "회복약 초과 회복 Clamp");
            int hpPotionBefore = InventoryService.GetItemCount("item_healing_potion_small");
            Check(ItemUseService.TryUse("item_healing_potion_small", "audit_hp").Result == ItemUseResult.AlreadyFull
                && InventoryService.GetItemCount("item_healing_potion_small") == hpPotionBefore, "풀 HP 소비 방지");

            PartyResourceService.ResolveForBattle("audit_mp", 104, 44);
            PartyResourceService.RecordBattleResult("audit_mp", 104, 20, 104, 44);
            InventoryService.TryAddItem("item_mana_potion_small", 3);
            ItemUseOutcome mpUse = ItemUseService.TryUse("item_mana_potion_small", "audit_mp");
            Check(mpUse.Succeeded && mpUse.ActualAmount == 12
                && PartyResourceService.TryGet("audit_mp", out PartyMemberResourceSnapshot mpAfter) && mpAfter.CurrentMp == 32
                && InventoryService.GetItemCount("item_mana_potion_small") == 2, "마력 회복약 20/44→32/44, 3→2");
            PartyResourceService.RecordBattleResult("audit_mp", 104, 40, 104, 44);
            mpUse = ItemUseService.TryUse("item_mana_potion_small", "audit_mp");
            Check(mpUse.Succeeded && mpUse.ActualAmount == 4
                && PartyResourceService.TryGet("audit_mp", out mpAfter) && mpAfter.CurrentMp == 44, "마력 회복약 초과 회복 Clamp");
            int manaPotionBefore = InventoryService.GetItemCount("item_mana_potion_small");
            Check(ItemUseService.TryUse("item_mana_potion_small", "audit_hp").Result == ItemUseResult.TargetDoesNotUseMp
                && InventoryService.GetItemCount("item_mana_potion_small") == manaPotionBefore, "MP 미사용 대상 소비 방지");
            PartyResourceService.ConfigureForAudit("audit_ko", 0, 0, 104, 0);
            InventoryService.TryAddItem("item_healing_potion_small", 1);
            hpPotionBefore = InventoryService.GetItemCount("item_healing_potion_small");
            Check(ItemUseService.TryUse("item_healing_potion_small", "audit_ko").Result == ItemUseResult.TargetKnockedOut
                && InventoryService.GetItemCount("item_healing_potion_small") == hpPotionBefore, "HP 0 대상 소비 방지");

            InventoryEntry[] inventoryAfterUse = InventoryService.ExportSaveData();
            PartyMemberResourceSaveData[] resourcesAfterUse = PartyResourceService.ExportSaveData();
            InventoryService.Reset(); PartyResourceService.Reset();
            InventoryService.ImportSaveData(inventoryAfterUse); PartyResourceService.ImportSaveData(resourcesAfterUse);
            Check(InventoryService.GetItemCount("item_mana_potion_small") == manaPotionBefore
                && PartyResourceService.TryGet("audit_mp", out PartyMemberResourceSnapshot restoredMp)
                && restoredMp.CurrentMp == 44, "아이템 사용 뒤 Inventory/PartyResources Save Restore");
            InventoryService.Reset();
            ItemDefinition stackItem = ScriptableObject.CreateInstance<ItemDefinition>();
            stackItem.ConfigureForAudit("audit_stack_item", 99);
            ItemCatalog.RegisterForAudit(stackItem);
            Check(InventoryService.TryAddItem("audit_stack_item", 99) && !InventoryService.TryAddItem("audit_stack_item", 1), "Inventory MaxStack 강제");
            UnityEngine.Object.DestroyImmediate(stackItem);
            ItemCatalog.ReloadForAudit();
            EconomyService.Reset(); InventoryService.Reset();
            Debug.Log("ECONOMY_INVENTORY_AUDIT ALL PASS");
        }

        private static void Check(bool condition, string message)
        {
            if (!condition) throw new InvalidOperationException("ECONOMY_INVENTORY_AUDIT FAIL: " + message);
        }
    }
}

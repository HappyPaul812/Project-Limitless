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
            Debug.Log("ECONOMY_INVENTORY_AUDIT ALL PASS");
        }

        private static void Check(bool condition, string message)
        {
            if (!condition) throw new InvalidOperationException("ECONOMY_INVENTORY_AUDIT FAIL: " + message);
        }
    }
}

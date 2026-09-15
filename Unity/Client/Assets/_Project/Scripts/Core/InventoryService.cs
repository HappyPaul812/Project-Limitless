using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectLimitless.Core
{
    [Serializable] public sealed class InventoryEntry
    {
        public string ItemId = string.Empty;
        public int Count;
        public InventoryEntry() { }
        public InventoryEntry(string itemId, int count) { ItemId = itemId; Count = count; }
    }

    /// <summary>ItemDefinition 전체 대신 stable ItemId와 수량만 저장해 이름·설명 변경과 JSON을 분리합니다.</summary>
    public static class InventoryService
    {
        private static readonly Dictionary<string, int> Counts = new Dictionary<string, int>(StringComparer.Ordinal);
        public static int GetItemCount(string itemId) => itemId != null && Counts.TryGetValue(itemId, out int count) ? count : 0;
        public static bool HasItem(string itemId, int amount) => amount >= 0 && GetItemCount(itemId) >= amount;
        public static bool CanAddItem(string itemId, int amount)
        {
            if (amount <= 0 || !ItemCatalog.TryGet(itemId, out ItemDefinition item)) return false;
            int current = GetItemCount(itemId);
            // UI와 공용 Inventory API가 같은 MaxStack을 지켜야 상점 밖의 보상도 Stack 제한을 우회하지 못합니다.
            return current <= int.MaxValue - amount && current + amount <= item.MaxStack;
        }
        public static int GetAddableAmount(string itemId)
        {
            return ItemCatalog.TryGet(itemId, out ItemDefinition item)
                ? Math.Max(0, item.MaxStack - GetItemCount(itemId))
                : 0;
        }
        public static bool TryAddItem(string itemId, int amount)
        {
            if (!CanAddItem(itemId, amount)) return false;
            Counts[itemId] = GetItemCount(itemId) + amount;
            return true;
        }
        public static bool TryRemoveItem(string itemId, int amount)
        {
            if (amount <= 0 || !HasItem(itemId, amount)) return false;
            int remaining = Counts[itemId] - amount;
            if (remaining == 0) Counts.Remove(itemId); else Counts[itemId] = remaining;
            return true;
        }
        public static InventoryEntry[] ExportSaveData() => Counts.OrderBy(x => x.Key).Select(x => new InventoryEntry(x.Key, x.Value)).ToArray();
        public static void ImportSaveData(InventoryEntry[] entries)
        {
            Counts.Clear();
            if (entries == null) return;
            foreach (InventoryEntry entry in entries)
                if (entry != null && entry.Count > 0 && ItemCatalog.TryGet(entry.ItemId, out ItemDefinition item)
                    && entry.Count <= item.MaxStack)
                    Counts[entry.ItemId] = entry.Count;
        }
        public static void Reset() => Counts.Clear();
    }
}

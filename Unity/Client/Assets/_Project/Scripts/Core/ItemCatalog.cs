using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectLimitless.Core
{
    /// <summary>퀘스트·몬스터·상점·은행이 같은 ItemId 해석 규칙을 공유하는 단일 조회 지점입니다.</summary>
    public static class ItemCatalog
    {
        private static Dictionary<string, ItemDefinition> definitions;
        public static IReadOnlyCollection<ItemDefinition> All { get { EnsureLoaded(); return definitions.Values; } }
        public static bool TryGet(string itemId, out ItemDefinition definition)
        {
            EnsureLoaded();
            return definitions.TryGetValue(itemId ?? string.Empty, out definition);
        }
        private static void EnsureLoaded()
        {
            if (definitions != null) return;
            definitions = new Dictionary<string, ItemDefinition>(StringComparer.Ordinal);
            foreach (ItemDefinition item in Resources.LoadAll<ItemDefinition>("ItemDefinitions"))
                if (item != null && !string.IsNullOrWhiteSpace(item.ItemId) && !definitions.ContainsKey(item.ItemId))
                    definitions.Add(item.ItemId, item);
        }
#if UNITY_EDITOR
        public static void RegisterForAudit(ItemDefinition item) { EnsureLoaded(); definitions[item.ItemId] = item; }
        public static void ReloadForAudit() => definitions = null;
#endif
    }
}

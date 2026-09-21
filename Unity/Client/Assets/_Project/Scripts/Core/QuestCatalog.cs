using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectLimitless.Core
{
    /// <summary>Resources의 퀘스트를 안정적인 QuestId로 찾는 단일 조회 지점입니다.</summary>
    public static class QuestCatalog
    {
        private static Dictionary<string, QuestDefinition> definitions;
        public static IReadOnlyCollection<QuestDefinition> All { get { EnsureLoaded(); return definitions.Values; } }

        public static bool TryGet(string questId, out QuestDefinition definition)
        { EnsureLoaded(); return definitions.TryGetValue(questId ?? string.Empty, out definition); }

        private static void EnsureLoaded()
        {
            if (definitions != null) return;
            definitions = new Dictionary<string, QuestDefinition>(StringComparer.Ordinal);
            foreach (QuestDefinition quest in Resources.LoadAll<QuestDefinition>("QuestDefinitions"))
                if (quest != null && !string.IsNullOrWhiteSpace(quest.QuestId) && !definitions.ContainsKey(quest.QuestId))
                    definitions.Add(quest.QuestId, quest);
        }

#if UNITY_EDITOR
        public static void RegisterForAudit(QuestDefinition quest) { EnsureLoaded(); definitions[quest.QuestId] = quest; }
        public static void ReloadForAudit() => definitions = null;
#endif
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectLimitless.UI
{
    /// <summary>Resources의 초상화 정의를 stable ID로 조회합니다. 정의나 Sprite가 없어도 대화를 계속 표시합니다.</summary>
    public static class DialoguePortraitCatalog
    {
        public const string ResourceFolder = "DialoguePortraitDefinitions";
        public const string TaeonId = "companion_taeon";
        public const string MielId = "companion_miel";
        public const string PaulId = "companion_paul";

        private static Dictionary<string, DialoguePortraitDefinition> definitions;

        public static bool TryGetDefinition(string speakerId, out DialoguePortraitDefinition definition)
        {
            EnsureLoaded();
            if (string.IsNullOrWhiteSpace(speakerId))
            {
                definition = null;
                return false;
            }

            return definitions.TryGetValue(speakerId, out definition);
        }

        public static Sprite GetPortrait(string speakerId)
            => TryGetDefinition(speakerId, out DialoguePortraitDefinition definition) ? definition.Portrait : null;

        private static void EnsureLoaded()
        {
            if (definitions != null) return;

            definitions = new Dictionary<string, DialoguePortraitDefinition>(StringComparer.Ordinal);
            foreach (DialoguePortraitDefinition definition in Resources.LoadAll<DialoguePortraitDefinition>(ResourceFolder))
            {
                if (definition == null || string.IsNullOrWhiteSpace(definition.SpeakerId)) continue;
                if (!definitions.TryAdd(definition.SpeakerId, definition))
                    Debug.LogWarning($"[DialoguePortrait] 중복 stable ID를 무시합니다: {definition.SpeakerId}");
            }
        }

#if UNITY_EDITOR
        /// <summary>Editor에서 정의를 생성한 직후 다음 조회가 새 콘텐츠를 다시 읽게 합니다.</summary>
        public static void ReloadInEditor() => definitions = null;
#endif
    }
}

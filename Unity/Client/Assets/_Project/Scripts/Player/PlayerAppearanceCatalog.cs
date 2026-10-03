using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectLimitless.Player
{
    /// <summary>외부 제작 50종의 원본과 고정 ID를 보관합니다. 제작 테마는 길·직업 선택을 제한하지 않습니다.</summary>
    [CreateAssetMenu(menuName = "Project Limitless/Player/Appearance Catalog")]
    public sealed class PlayerAppearanceCatalog : ScriptableObject
    {
        [Serializable]
        public sealed class Entry
        {
            public string appearanceId, theme, jobTheme, gender, assetPath;
            public int width, height;
            public bool hasAlpha;
            public int expectedFrameCount;
            // 검수 미완료 외형은 선택 목록에 넣지 않습니다. 원본의 Alpha·방향·Pivot 문제를 숨기지 않습니다.
            public bool readyForSelection;
            public string reviewReason;
            public Texture2D sheet;
            public Sprite[] frames;
        }
        [Serializable]
        public sealed class Inventory { public Entry[] entries; }
        [SerializeField] private Entry[] entries = Array.Empty<Entry>();
        public IReadOnlyList<Entry> Entries => entries;

        /// <summary>알 수 없는 고정 ID는 null을 반환합니다. 미래 선택/Save 호출자는 기존 외형을 유지해야 합니다.</summary>
        public Entry Find(string appearanceId)
        {
            if (string.IsNullOrEmpty(appearanceId)) return null;
            foreach (Entry entry in entries)
                if (entry.appearanceId == appearanceId) return entry;
            return null;
        }
#if UNITY_EDITOR
        // 편입 도구만 원본 Texture 참조를 채웁니다. 실제 선택/Save는 검수 뒤 별도 연결합니다.
        public void SetImportedEntries(Entry[] importedEntries) => entries = importedEntries;
#endif
    }
}

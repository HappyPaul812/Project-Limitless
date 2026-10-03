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
            // Down/Left/Right/Up마다 Idle, Walk 순서입니다. 공용 상태 머신에는 이 Clip만 덮어씁니다.
            public AnimationClip[] clips;
            public bool IsUsable => readyForSelection && frames != null && frames.Length == 16 &&
                Array.TrueForAll(frames, sprite => sprite != null) && clips != null && clips.Length == 8 &&
                Array.TrueForAll(clips, clip => clip != null);
        }
        [Serializable]
        public sealed class Inventory { public Entry[] entries; }
        [SerializeField] private Entry[] entries = Array.Empty<Entry>();
        public IReadOnlyList<Entry> Entries => entries;
        public static PlayerAppearanceCatalog Load() => Resources.Load<PlayerAppearanceCatalog>("PlayerAppearances/External50");

        /// <summary>불명·미검수·참조 손상은 null로 돌려 기존 성별/길 외형을 보호합니다.</summary>
        public static Entry Resolve(string id)
        {
            Entry entry = Load()?.Find(id);
            return entry != null && entry.IsUsable ? entry : null;
        }

        public List<Entry> Filter(PlayerVisualType gender, string theme)
        {
            var result = new List<Entry>();
            foreach (Entry entry in entries)
                if (entry.IsUsable && entry.gender == gender.ToString() &&
                    (string.IsNullOrEmpty(theme) || entry.theme == theme)) result.Add(entry);
            return result;
        }

        public static string ThemeName(string theme)
        {
            switch (theme)
            {
                case "Vision": return "시각";
                case "Hearing": return "청각";
                case "Intellectual": return "지적";
                case "Mobility": return "지체";
                case "EmotionalScar": return "마음의 상처";
                default: return "전체";
            }
        }

        public static string DisplayName(Entry entry)
        {
            if (entry == null) return "기존 기본 외형";
            string job = entry.jobTheme;
            switch (job)
            {
                case "Fighter": job = "투사"; break;
                case "Guardian": job = "수호자"; break;
                case "Healer": job = "치유사"; break;
                case "Mage": job = "마도사"; break;
                case "Marksman": job = "사수"; break;
            }
            return ThemeName(entry.theme) + " · " + job + " 테마";
        }

        /// <summary>원본 Controller의 이름 끝 상태를 찾아 Clip만 교체합니다. 불완전하면 적용하지 않습니다.</summary>
        public static AnimatorOverrideController CreateOverride(Entry entry, RuntimeAnimatorController shared)
        {
            if (entry == null || !entry.IsUsable || shared == null) return null;
            var controller = new AnimatorOverrideController(shared);
            var pairs = new List<KeyValuePair<AnimationClip, AnimationClip>>();
            controller.GetOverrides(pairs);
            string[] directions = { "Down", "Left", "Right", "Up" };
            int replaced = 0;
            for (int i = 0; i < pairs.Count; i++)
                for (int direction = 0; direction < directions.Length; direction++)
                    for (int motion = 0; motion < 2; motion++)
                        if (pairs[i].Key.name.EndsWith((motion == 0 ? "Idle_" : "Walk_") + directions[direction], StringComparison.Ordinal))
                        {
                            pairs[i] = new KeyValuePair<AnimationClip, AnimationClip>(pairs[i].Key, entry.clips[direction * 2 + motion]);
                            replaced++;
                        }
            if (replaced != 8) { UnityEngine.Object.Destroy(controller); return null; }
            controller.ApplyOverrides(pairs);
            return controller;
        }

        /// <summary>알 수 없는 고정 ID는 null을 반환합니다. Runtime은 Resolve로 검수 상태도 확인합니다.</summary>
        public Entry Find(string appearanceId)
        {
            if (string.IsNullOrEmpty(appearanceId)) return null;
            foreach (Entry entry in entries)
                if (entry.appearanceId == appearanceId) return entry;
            return null;
        }
#if UNITY_EDITOR
        // 편입 도구가 원본 Texture 참조를 채우며, 선택 Build가 검수 정책과 Clip을 연결합니다.
        public void SetImportedEntries(Entry[] importedEntries) => entries = importedEntries;
#endif
    }
}

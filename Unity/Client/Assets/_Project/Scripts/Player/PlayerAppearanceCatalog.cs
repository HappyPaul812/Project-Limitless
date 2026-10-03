using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectLimitless.Player
{
    /// <summary>외부 제작 50종의 원본과 고정 ID를 보관합니다. 성별·길·직업 조합으로 대응 정의를 조회합니다.</summary>
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
            // 직렬화 호환 필드입니다. 수동 선택 권한이 아니라 미술 검수 통과 여부를 뜻합니다.
            public bool readyForSelection;
            public bool RuntimeReady => IsUsable;
            public string GenderStableId => gender;
            public string PathStableId => PathIdForTheme(theme);
            public string JobStableId => jobTheme == "Marksman" ? "sharpshooter" : jobTheme?.ToLowerInvariant();
            public string ValidationStatus => readyForSelection ? "PASS" :
                (string.IsNullOrEmpty(reviewReason) ? "BLOCKED_UNREVIEWED" : reviewReason.Split(':')[0]);
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

        /// <summary>입력 메타데이터의 명명 차이만 대응합니다. 그림이나 배열 순서로 추론하지 않습니다.</summary>
        public static string PathIdForTheme(string theme)
        {
            switch (theme)
            {
                case "Vision": return "path.vision";
                case "Hearing": return "path.hearing";
                case "Intellectual": return "path.intellectual";
                case "Mobility": return "path.mobility";
                case "EmotionalScar": return "path.emotional-scar";
                default: return string.Empty;
            }
        }

        /// <summary>Blocked도 정확한 정의를 반환합니다. 누락·중복 조합은 다른 Sprite로 채우지 않습니다.</summary>
        public Entry FindCombination(string genderId, string pathId, string jobId)
        {
            if (string.IsNullOrEmpty(genderId) || string.IsNullOrEmpty(pathId) || string.IsNullOrEmpty(jobId)) return null;
            Entry found = null;
            foreach (Entry entry in entries)
                if (entry.GenderStableId == genderId && entry.PathStableId == pathId && entry.JobStableId == jobId)
                {
                    if (found != null) return null;
                    found = entry;
                }
            return found;
        }

        public static Entry ForCombination(PlayerVisualType gender, string pathId, string jobId) =>
            Load()?.FindCombination(gender.ToString(), pathId, jobId);

        /// <summary>불명·미검수·참조 손상은 null로 돌려 기본 성별 Sprite의 임시 fallback을 사용합니다.</summary>
        public static Entry Resolve(string id)
        {
            Entry entry = Load()?.Find(id);
            return entry != null && entry.IsUsable ? entry : null;
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
            return ThemeName(entry.theme) + " · " + job;
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
        // 편입 도구가 원본 Texture 참조를 채우며, 검수 Build가 미술 정책과 Clip을 연결합니다.
        public void SetImportedEntries(Entry[] importedEntries) => entries = importedEntries;
#endif
    }
}

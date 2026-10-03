using System;
using UnityEngine;

namespace ProjectLimitless.Audio
{
    /// <summary>확정 음악과 실제 Scene 역할을 보관합니다. 후보 음악과 미확정 Scene은 자동 배정하지 않습니다.</summary>
    [CreateAssetMenu(menuName = "Project Limitless/Audio/BGM Scene Catalog")]
    public sealed class BgmSceneCatalog : ScriptableObject
    {
        [Serializable]
        private struct Entry
        {
            public string sceneName;
            public AudioClip clip;
        }
        [SerializeField] private Entry[] scenes = Array.Empty<Entry>();
        [SerializeField] private AudioClip safeZoneClip;
        public AudioClip SafeZoneClip => safeZoneClip;
        [SerializeField] private AudioClip chapter1BattleClip;
        [SerializeField] private AudioClip silentWardenClip;
        [SerializeField] private AudioClip westernIntroductionClip;
        [SerializeField] private AudioClip deepWestClip;
        public AudioClip WesternIntroductionClip => westernIntroductionClip;
        // 최초 심부 Scene은 미정이므로 자동 Scene 배정과 분리해 보관합니다.
        public AudioClip DeepWestClip => deepWestClip;

        /// <summary>파수꾼 ID가 최우선입니다. 다른 Boss·Chapter2 일반전은 미정 상태를 유지합니다.</summary>
        public AudioClip FindBattle(string originScene, string encounterId, bool isBoss)
        {
            if (encounterId == ProjectLimitless.World.MainQuest11DungeonFlow.BossId) return silentWardenClip;
            if (isBoss) return null;
            switch (originScene)
            {
                case "": // 직접 Battle Scene을 실행한 기존 기본 전투도 기본곡을 사용합니다.
                case "Field_01":
                case "Field_02":
                case "Field_03":
                case "Dungeon_01":
                case "Dungeon_01_B2": return chapter1BattleClip;
                default: return null;
            }
        }

        public AudioClip Find(string sceneName)
        {
            foreach (Entry entry in scenes)
                if (string.Equals(entry.sceneName, sceneName, StringComparison.Ordinal)) return entry.clip;
            return null;
        }
    }
}

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
        [SerializeField] private AudioClip chapter2BattleClip;
        [SerializeField] private AudioClip silentWardenClip;
        [SerializeField] private AudioClip veinfireColossusClip;
        [SerializeField] private AudioClip westernIntroductionClip;
        [SerializeField] private AudioClip deepWestClip;
        public AudioClip WesternIntroductionClip => westernIntroductionClip;
        // 최초 심부 Scene은 미정이므로 자동 Scene 배정과 분리해 보관합니다.
        public AudioClip DeepWestClip => deepWestClip;

        /// <summary>전용 파수꾼 곡을 먼저 판정하고, 일반 서부 전투에만 Chapter2 공통곡을 배정합니다.</summary>
        public AudioClip FindBattle(string originScene, string encounterId, bool isBoss)
        {
            if (encounterId == ProjectLimitless.World.MainQuest11DungeonFlow.BossId) return silentWardenClip;
            if (encounterId == ProjectLimitless.World.Chapter2Main20Flow.EncounterId) return veinfireColossusClip;
            if (isBoss) return null;
            switch (originScene)
            {
                case "Field_04_WesternBorder":
                case "Field_05_WesternOutskirts":
                case "Field_06_ScorchedTrail":
                case "Field_07_AshenReach":
                case "Field_08_RedRift":
                case "Field_09_ObsidianScar":
                case "Field_10_BurningPulse": return chapter2BattleClip;
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

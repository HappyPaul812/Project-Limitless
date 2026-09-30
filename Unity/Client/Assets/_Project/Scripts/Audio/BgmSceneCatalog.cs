using System;
using UnityEngine;

namespace ProjectLimitless.Audio
{
    /// <summary>확정된 세 곡과 실제 Scene 역할을 보관합니다. 미등록 Scene에는 음악을 추측해서 배정하지 않습니다.</summary>
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

        public AudioClip Find(string sceneName)
        {
            foreach (Entry entry in scenes)
                if (string.Equals(entry.sceneName, sceneName, StringComparison.Ordinal)) return entry.clip;
            return null;
        }
    }
}

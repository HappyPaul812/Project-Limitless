using System;
using UnityEngine;

namespace ProjectLimitless.Audio
{
    /// <summary>
    /// 안정적인 대사 ID와 실제 음성 파일을 연결합니다. Opening뿐 아니라 향후 NPC 음성도 같은
    /// 방식으로 별도 Catalog를 만들어 사용할 수 있습니다.
    /// </summary>
    [CreateAssetMenu(menuName = "Project Limitless/Audio/Voice Clip Catalog", fileName = "VoiceClipCatalog")]
    public sealed class VoiceClipCatalog : ScriptableObject
    {
        [Serializable]
        private struct Entry
        {
            [SerializeField] private string id;
            [SerializeField] private AudioClip clip;

            public string Id => id;
            public AudioClip Clip => clip;
        }

        [SerializeField] private Entry[] entries = Array.Empty<Entry>();

        public AudioClip Find(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;
            for (int i = 0; i < entries.Length; i++)
            {
                if (string.Equals(entries[i].Id, id, StringComparison.Ordinal)) return entries[i].Clip;
            }
            return null;
        }
    }
}

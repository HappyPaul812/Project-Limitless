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
            [SerializeField] private string speakerId;

            public string Id => id;
            public AudioClip Clip => clip;
            public string SpeakerId => speakerId;
        }

        [SerializeField] private Entry[] entries = Array.Empty<Entry>();

        public AudioClip Find(string id, string speakerId = null)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;
            for (int i = 0; i < entries.Length; i++)
            {
                if (!string.Equals(entries[i].Id, id, StringComparison.Ordinal)) continue;
                // 기존 Intro는 화자 제한이 없습니다. Story는 등록된 화자와 다른 ID가 오면 음성을 재생하지 않습니다.
                if (!string.IsNullOrEmpty(entries[i].SpeakerId)
                    && !string.Equals(entries[i].SpeakerId, speakerId, StringComparison.Ordinal)) return null;
                return entries[i].Clip;
            }
            return null;
        }
    }
}

using UnityEngine;

namespace ProjectLimitless.UI
{
    /// <summary>표시 이름과 분리된 stable ID로 대화 초상화를 연결하는 콘텐츠 정의입니다.</summary>
    [CreateAssetMenu(fileName = "DialoguePortrait", menuName = "Project Limitless/UI/Dialogue Portrait")]
    public sealed class DialoguePortraitDefinition : ScriptableObject
    {
        [SerializeField] private string speakerId = string.Empty;
        [SerializeField] private string displayName = string.Empty;
        [SerializeField] private Sprite portrait;

        public string SpeakerId => speakerId;
        public string DisplayName => displayName;
        public Sprite Portrait => portrait;

        /// <summary>Editor 콘텐츠 생성기가 기존 정의를 안전하게 갱신할 때 사용합니다.</summary>
        public void ConfigureContent(string stableSpeakerId, string speakerDisplayName, Sprite portraitSprite)
        {
            speakerId = stableSpeakerId ?? string.Empty;
            displayName = speakerDisplayName ?? string.Empty;
            portrait = portraitSprite;
        }
    }
}

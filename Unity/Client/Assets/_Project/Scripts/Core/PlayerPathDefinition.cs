using System;
using UnityEngine;

namespace ProjectLimitless.Core
{
    /// <summary>
    /// 아직 세계관의 정식 길 이름이 확정되지 않았으므로 사용하는 개발용 식별자입니다.
    /// Path01~Path04는 UI Prototype 슬롯이며 실제 길의 수나 순서를 확정하지 않습니다.
    /// </summary>
    public enum PlayerPathType
    {
        None,
        Path01,
        Path02,
        Path03,
        Path04,
    }

    /// <summary>
    /// 한 Path 카드에 표시할 데이터를 UI 동작과 분리해 보관합니다.
    /// 향후 이름·설명·아이콘·특징·관련 직업이 확정되어도 선택 코드를 고치지 않고 데이터만 교체할 수 있습니다.
    /// </summary>
    [Serializable]
    public sealed class PlayerPathDefinition
    {
        [SerializeField] private PlayerPathType pathType;
        [SerializeField] private string displayName;
        [SerializeField] private Sprite icon;
        [SerializeField] private string shortDescription;
        [SerializeField, TextArea] private string detailedDescription;
        [SerializeField] private string features;
        [SerializeField] private string relatedJobInformation;

        public PlayerPathType PathType => pathType;
        public string DisplayName => displayName;
        public Sprite Icon => icon;
        public string ShortDescription => shortDescription;
        public string DetailedDescription => detailedDescription;
        public string Features => features;
        public string RelatedJobInformation => relatedJobInformation;

        /// <summary>현재 Prototype의 임시 카드 데이터를 만듭니다. 정식 설정 확정 후 교체할 값입니다.</summary>
        public PlayerPathDefinition(PlayerPathType type, string temporaryName)
        {
            pathType = type;
            displayName = temporaryName;
            shortDescription = "설명 준비 중";
            detailedDescription = string.Empty;
            features = string.Empty;
            relatedJobInformation = string.Empty;
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectLimitless.Core
{
    /// <summary>캐릭터 생성 미리보기에서 사용하는 여섯 가지 기본 능력치입니다.</summary>
    public enum CharacterStatType
    {
        Health,
        Strength,
        Agility,
        Sense,
        Intelligence,
        Willpower,
    }

    [Serializable]
    public struct StatBonus
    {
        [SerializeField] private CharacterStatType stat;
        [SerializeField] private int amount;

        public CharacterStatType Stat => stat;
        public int Amount => amount;
    }

    /// <summary>
    /// 아직 JobDefinition이 없는 단계에서 추천 직업의 안정적인 ID와 화면 이름을 함께 보관합니다.
    /// 향후 JobDefinition이 생기면 ID로 연결하므로 PathSelection과 JobSelection이 같은 추천 데이터를 재사용할 수 있습니다.
    /// </summary>
    [Serializable]
    public struct RecommendedJob
    {
        [SerializeField] private string id;
        [SerializeField] private string displayName;

        public string Id => id;
        public string DisplayName => displayName;
    }

    /// <summary>
    /// 길의 표시 정보와 전투 패시브를 UI 코드에서 분리해 보관하는 데이터 에셋입니다.
    /// 길은 기본 성향과 패시브를, 직업은 전투 방식과 액티브 스킬을 담당하므로 서로 독립적으로 조합됩니다.
    /// 새 길은 이 에셋을 Resources/PathDefinitions에 추가하면 선택 화면에 자동으로 나타납니다.
    /// </summary>
    [CreateAssetMenu(fileName = "PathDefinition", menuName = "Project Limitless/Path Definition")]
    public sealed class PlayerPathDefinition : ScriptableObject
    {
        [SerializeField] private string id;
        [SerializeField] private string displayName;
        [SerializeField] private string motifDescription;
        [SerializeField, TextArea] private string shortDescription;
        [SerializeField, TextArea] private string detailedDescription;
        [SerializeField] private StatBonus[] statBonuses;
        [SerializeField] private string passiveName;
        [SerializeField, TextArea] private string passiveDescription;
        [SerializeField] private string[] keywords;
        [SerializeField] private RecommendedJob[] recommendedJobs;
        // PathSymbol은 길 자체의 정체성을 나타내는 프로젝트 자체 제작 문장입니다. 캐릭터 외형과
        // 별개로 보관하므로 어떤 외형을 사용하더라도 길 이름과 같은 공식 상징을 계속 쓸 수 있습니다.
        [SerializeField] private Sprite pathSymbol;
        // TraitIcon은 전투 중 패시브의 기능과 상태를 빠르게 알아보게 하는 아이콘입니다. 공식 문장과
        // 한 Icon 필드를 공유하면 화면마다 뜻이 뒤바뀌므로 두 Sprite를 명시적으로 분리합니다.
        [SerializeField] private Sprite traitIcon;
        [SerializeField] private string[] traitStatusMarkerIds;
        [SerializeField, TextArea] private string relatedJobInformation;

        public string Id => id;
        public string DisplayName => displayName;
        public string MotifDescription => motifDescription;
        public string ShortDescription => shortDescription;
        public string DetailedDescription => detailedDescription;
        public IReadOnlyList<StatBonus> StatBonuses => statBonuses;
        public string PassiveName => passiveName;
        public string PassiveDescription => passiveDescription;
        public IReadOnlyList<string> Keywords => keywords;
        public IReadOnlyList<RecommendedJob> RecommendedJobs => recommendedJobs ?? Array.Empty<RecommendedJob>();
        public Sprite PathSymbol => pathSymbol;
        public Sprite TraitIcon => traitIcon;
        public IReadOnlyList<string> TraitStatusMarkerIds => traitStatusMarkerIds ?? Array.Empty<string>();
        public string RelatedJobInformation => relatedJobInformation;

        /// <summary>길은 직접 능력치 보너스를 주지 않으므로 기본값을 그대로 돌려줍니다.</summary>
        public int GetPreviewStat(CharacterStatType stat, int baseValue = 10)
        {
            return baseValue;
        }
    }
}

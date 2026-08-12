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
    /// 길의 표시 정보와 1차 프로토타입 수치를 UI 코드에서 분리해 보관하는 데이터 에셋입니다.
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
        [SerializeField] private Sprite icon;
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
        public Sprite Icon => icon;
        public string RelatedJobInformation => relatedJobInformation;

        /// <summary>모든 기본값 10에 이 길의 보너스만 더합니다. 현재 길은 능력치를 감소시키지 않습니다.</summary>
        public int GetPreviewStat(CharacterStatType stat, int baseValue = 10)
        {
            int result = baseValue;
            if (statBonuses == null) return result;
            foreach (StatBonus bonus in statBonuses)
            {
                if (bonus.Stat == stat) result += bonus.Amount;
            }
            return result;
        }
    }
}

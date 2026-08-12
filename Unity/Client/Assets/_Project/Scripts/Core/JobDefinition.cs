using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectLimitless.Core
{
    /// <summary>실제 전투 SkillDefinition을 만들기 전, 캐릭터 생성 화면에 보여 줄 시작 스킬 요약입니다.</summary>
    [Serializable]
    public struct SkillPreviewData
    {
        [SerializeField] private string skillId;
        [SerializeField] private string skillName;
        [SerializeField] private string skillType;
        [SerializeField, TextArea] private string skillDescription;

        /// <summary>표시 이름이 바뀌어도 Save/Combat 시스템에서 같은 스킬을 찾기 위한 안정적인 ID입니다.</summary>
        public string SkillId => skillId;
        public string SkillName => skillName;
        public string SkillType => skillType;
        public string SkillDescription => skillDescription;
    }

    /// <summary>전투 실행 코드와 결합하지 않고 패시브의 1차 설계 내용을 보관합니다.</summary>
    [Serializable]
    public struct PassivePreviewData
    {
        [SerializeField] private string passiveId;
        [SerializeField] private string passiveName;
        [SerializeField, TextArea] private string passiveDescription;

        public string PassiveId => passiveId;
        public string PassiveName => passiveName;
        public string PassiveDescription => passiveDescription;
    }

    /// <summary>
    /// 직업의 전투 역할과 표시 정보를 UI 코드 밖에 보관하는 데이터 에셋입니다.
    /// 길은 캐릭터의 성향과 고유 패시브를, 직업은 전투 역할과 향후 스킬·성장을 담당하므로 별도로 선택합니다.
    /// </summary>
    [CreateAssetMenu(fileName = "JobDefinition", menuName = "Project Limitless/Job Definition")]
    public sealed class JobDefinition : ScriptableObject
    {
        [SerializeField] private string jobId;
        [SerializeField] private string displayName;
        [SerializeField] private string roleName;
        [SerializeField, TextArea] private string shortDescription;
        [SerializeField, TextArea] private string detailedDescription;
        [SerializeField] private CharacterStatType[] primaryStats;
        [SerializeField] private string[] keywords;
        [SerializeField] private Sprite icon;
        // 프리뷰 수치와 실제 전투 계산을 분리하여 전투 시스템이 확정될 때 안전하게 연결하거나 마이그레이션합니다.
        [SerializeField] private StatBonus[] statBonuses;
        [SerializeField] private PassivePreviewData passive;
        [SerializeField] private SkillPreviewData[] startingSkills;

        /// <summary>표시 이름이 바뀌어도 저장 데이터와 추천 연결을 유지하는 안정적인 ID입니다.</summary>
        public string JobId => jobId;
        public string DisplayName => displayName;
        public string RoleName => roleName;
        public string ShortDescription => shortDescription;
        public string DetailedDescription => detailedDescription;
        public IReadOnlyList<CharacterStatType> PrimaryStats => primaryStats ?? Array.Empty<CharacterStatType>();
        public IReadOnlyList<string> Keywords => keywords ?? Array.Empty<string>();
        public Sprite Icon => icon;
        public IReadOnlyList<StatBonus> StatBonuses => statBonuses ?? Array.Empty<StatBonus>();
        public PassivePreviewData Passive => passive;
        public IReadOnlyList<SkillPreviewData> StartingSkills => startingSkills ?? Array.Empty<SkillPreviewData>();

        /// <summary>기본값과 길 보너스가 적용된 값에 이 직업의 +4 보너스를 더합니다.</summary>
        public int ApplyStatBonus(CharacterStatType stat, int currentValue)
        {
            foreach (StatBonus bonus in StatBonuses)
            {
                if (bonus.Stat == stat) currentValue += bonus.Amount;
            }
            return currentValue;
        }
    }
}

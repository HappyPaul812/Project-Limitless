using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectLimitless.Core
{
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
        [SerializeField] private StatBonus[] futureStatBonuses;
        [SerializeField] private string futurePassive;
        [SerializeField] private string[] futureStartingSkills;

        /// <summary>표시 이름이 바뀌어도 저장 데이터와 추천 연결을 유지하는 안정적인 ID입니다.</summary>
        public string JobId => jobId;
        public string DisplayName => displayName;
        public string RoleName => roleName;
        public string ShortDescription => shortDescription;
        public string DetailedDescription => detailedDescription;
        public IReadOnlyList<CharacterStatType> PrimaryStats => primaryStats ?? Array.Empty<CharacterStatType>();
        public IReadOnlyList<string> Keywords => keywords ?? Array.Empty<string>();
        public Sprite Icon => icon;
    }
}

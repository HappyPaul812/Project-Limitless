using System.Collections.Generic;
using System.Linq;
using ProjectLimitless.Battle;
using UnityEngine;

namespace ProjectLimitless.Core
{
    /// <summary>기존 CharacterId의 전투·표시 정보를 연결합니다. Sprite와 애니메이션은 기존 원본을 재사용합니다.</summary>
    [CreateAssetMenu(menuName = "Project Limitless/Companion Definition")]
    public sealed class CompanionDefinition : ScriptableObject
    {
        public string CharacterId;
        public string DisplayName;
        public string JobId;
        public string JobName;
        public string PathId;
        public string PathName;
        public FormationRow DefaultRow;
        public TargetRangeType BasicRange;
        public int MaxHp;
        public int Attack;
        public int Agility;

        /// <summary>레벨은 저장하지 않고 참가자 생성 시마다 현재 Player 레벨을 읽습니다.</summary>
        public int EffectiveLevel => GameSessionData.Level;

        public BattleParticipantSetup CreateParticipant(FormationSlot slot)
        {
            CharacterGrowthStats growth = CharacterGrowthCalculator.Calculate(JobId, EffectiveLevel);
            CharacterGrowthStats first = CharacterGrowthCalculator.Calculate(JobId, 1);
            // 수호자·치유사 Player 성장은 미확정이므로 바꾸지 않습니다. 승인된 동료 정책만 여기서
            // 공통 +1/레벨을 적용하고, 직업 공격식·HP식은 기존 계산기를 그대로 재사용합니다.
            if (JobId == "guardian" || JobId == "healer") growth.AddToAll(growth.Level - 1);
            // MaxHp와 Agility는 완성된 Lv1 고유값이므로 증가분만 더해 시작 보너스 이중 적용을 막습니다.
            // Attack은 고유 기본 공격력이며 주 능력치 보너스를 한 번 더하는 Player와 같은 공격식입니다.
            int hp = System.Math.Max(1, MaxHp + CharacterGrowthCalculator.CalculateMaxHp(JobId, growth)
                - CharacterGrowthCalculator.CalculateMaxHp(JobId, first));
            int attack = CharacterGrowthCalculator.CalculateAttack(JobId, growth, Attack);
            int agility = System.Math.Max(1, Agility + growth.Agility - first.Agility);
            return new BattleParticipantSetup(CharacterId, DisplayName, JobId, BattleSide.Allies, slot, hp, attack, agility, 0,
                BasicRange, true, BattleParticipantVisualType.PrototypeCompanion, DisplayName, pathId: PathId);
        }
    }

    /// <summary>명단 UI와 일반 전투가 같은 Resources 데이터를 읽으며 새 동료 수를 제한하지 않습니다.</summary>
    public static class CompanionCatalog
    {
        public static IReadOnlyList<CompanionDefinition> All => Resources.LoadAll<CompanionDefinition>("CompanionDefinitions")
            .OrderBy(x => x.name).ToArray();
        public static CompanionDefinition Find(string id) => All.FirstOrDefault(x => x.CharacterId == id);
    }
}

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

        public BattleParticipantSetup CreateParticipant(FormationSlot slot) => new BattleParticipantSetup(
            CharacterId, DisplayName, JobId, BattleSide.Allies, slot, MaxHp, Attack, Agility, 0,
            BasicRange, true, BattleParticipantVisualType.PrototypeCompanion, DisplayName, pathId: PathId);
    }

    /// <summary>명단 UI와 일반 전투가 같은 Resources 데이터를 읽으며 새 동료 수를 제한하지 않습니다.</summary>
    public static class CompanionCatalog
    {
        public static IReadOnlyList<CompanionDefinition> All => Resources.LoadAll<CompanionDefinition>("CompanionDefinitions")
            .OrderBy(x => x.name).ToArray();
        public static CompanionDefinition Find(string id) => All.FirstOrDefault(x => x.CharacterId == id);
    }
}

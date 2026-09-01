using System;
using System.Collections.Generic;
using ProjectLimitless.Monster;

namespace ProjectLimitless.Battle
{
    /// <summary>전투 화면이 참가자의 출처와 무관하게 표시 방법을 선택할 수 있도록 하는 임시 Visual 구분입니다.</summary>
    public enum BattleParticipantVisualType
    {
        Player,
        PrototypeCompanion,
        EncounterMonster
    }

    /// <summary>
    /// 한 전투 참가자를 생성하는 데 필요한 값만 보관합니다.
    /// 향후 CompanionDefinition이나 필드 Encounter 데이터가 생기면 이 형식으로 변환해 같은 전투 생성 흐름을 재사용합니다.
    /// </summary>
    public sealed class BattleParticipantSetup
    {
        public BattleParticipantSetup(string id, string displayName, string jobId, BattleSide side, FormationSlot slot,
            int maxHp, int attack, int agility, int actionPriority, TargetRangeType basicRange,
            bool playerControlled, BattleParticipantVisualType visualType, string placeholderLabel = "",
            MonsterDefinition monsterDefinition = null)
        {
            Id = id;
            DisplayName = displayName;
            JobId = jobId ?? string.Empty;
            Side = side;
            Slot = slot;
            MaxHp = maxHp;
            Attack = attack;
            Agility = agility;
            ActionPriority = actionPriority;
            BasicRange = basicRange;
            IsPlayerControlled = playerControlled;
            VisualType = visualType;
            PlaceholderLabel = placeholderLabel ?? string.Empty;
            MonsterDefinition = monsterDefinition;
        }

        public string Id { get; }
        public string DisplayName { get; }
        public string JobId { get; }
        public BattleSide Side { get; }
        public FormationSlot Slot { get; }
        public int MaxHp { get; }
        public int Attack { get; }
        public int Agility { get; }
        public int ActionPriority { get; }
        public TargetRangeType BasicRange { get; }
        public bool IsPlayerControlled { get; }
        public BattleParticipantVisualType VisualType { get; }
        public string PlaceholderLabel { get; }
        public MonsterDefinition MonsterDefinition { get; }
    }

    /// <summary>아군과 적 참가자 목록을 함께 전달하는 Encounter 단위 데이터입니다.</summary>
    public sealed class BattleEncounterSetup
    {
        public BattleEncounterSetup(IReadOnlyList<BattleParticipantSetup> allies, IReadOnlyList<BattleParticipantSetup> enemies)
        {
            Allies = allies ?? Array.Empty<BattleParticipantSetup>();
            Enemies = enemies ?? Array.Empty<BattleParticipantSetup>();
        }

        public IReadOnlyList<BattleParticipantSetup> Allies { get; }
        public IReadOnlyList<BattleParticipantSetup> Enemies { get; }
    }

    /// <summary>
    /// 정식 동료·파티 편성·다중 필드 조우 데이터가 생기기 전 3대3 전투를 검증하기 위한 구성입니다.
    /// 전투 컨트롤러와 분리되어 있어 정식 데이터가 준비되면 이 Factory만 교체할 수 있습니다.
    /// </summary>
    public static class BattlePrototypeEncounterFactory
    {
        private const int SlimeBaseAttack = 10;

        public static BattleEncounterSetup CreateThreeVsThree(string playerName, string playerJobId,
            int playerMaxHp, int playerAttack, int playerAgility, MonsterDefinition slime, MonsterDefinition venomBee,
            MonsterDefinition encounteredMonster)
        {
            BattleParticipantSetup[] allies =
            {
                new BattleParticipantSetup("companion_taeon", "태온", "guardian", BattleSide.Allies,
                    new FormationSlot(FormationRow.Front, 0), 132, 10, 9, 0,
                    TargetRangeType.MeleePhysical, true, BattleParticipantVisualType.PrototypeCompanion, "태"),
                new BattleParticipantSetup("player", playerName, playerJobId, BattleSide.Allies,
                    new FormationSlot(FormationRow.Front, 1), playerMaxHp, playerAttack, playerAgility, 0,
                    ResolveBasicRange(playerJobId), true, BattleParticipantVisualType.Player),
                new BattleParticipantSetup("companion_miel", "미엘", "healer", BattleSide.Allies,
                    new FormationSlot(FormationRow.Rear, 0), 104, 8, 12, 0,
                    TargetRangeType.Magic, true, BattleParticipantVisualType.PrototypeCompanion, "미")
            };

            bool spiderEncounter = encounteredMonster != null && encounteredMonster.MonsterId == "forest_spider";
            BattleParticipantSetup[] enemies =
            {
                CreateMonster(slime, "grass_slime_a", "초원 슬라임", FormationRow.Front, 0, 11),
                CreateMonster(slime, "grass_slime_b", "초원 슬라임", FormationRow.Front, 1, 10),
                CreateMonster(spiderEncounter ? encounteredMonster : venomBee,
                    spiderEncounter ? "forest_spider_1" : "venom_bee_1",
                    spiderEncounter ? "숲거미" : "독침벌", FormationRow.Rear, 0, 12)
            };
            return new BattleEncounterSetup(allies, enemies);
        }

        private static BattleParticipantSetup CreateMonster(MonsterDefinition monster, string fallbackId,
            string fallbackName, FormationRow row, int column, int agility)
        {
            // Field의 스폰은 위치·리스폰을 나타내고 Battle 참가자는 이번 전투의 Formation 칸을 나타냅니다.
            // 둘은 서로 다른 개념이지만 같은 MonsterDefinition을 참조하므로 이름과 외형은 한 데이터에서 공유합니다.
            string name = monster == null || string.IsNullOrWhiteSpace(monster.DisplayName) ? fallbackName : monster.DisplayName;
            if (fallbackId.EndsWith("_a", StringComparison.Ordinal)) name += " A";
            else if (fallbackId.EndsWith("_b", StringComparison.Ordinal)) name += " B";
            else if (fallbackId.EndsWith("_1", StringComparison.Ordinal)) name += " 1";
            int attackPercent = monster == null ? 100 : monster.BattleAttackPercent;
            int attack = (int)Math.Max(1L, ((long)SlimeBaseAttack * attackPercent + 99L) / 100L);
            return new BattleParticipantSetup(fallbackId, name, string.Empty, BattleSide.Enemies,
                new FormationSlot(row, column), 55, attack, agility, 0,
                TargetRangeType.MeleePhysical, false, BattleParticipantVisualType.EncounterMonster,
                monsterDefinition: monster);
        }

        private static TargetRangeType ResolveBasicRange(string jobId)
        {
            switch (jobId)
            {
                case "sharpshooter": return TargetRangeType.RangedPhysical;
                case "mage":
                case "healer": return TargetRangeType.Magic;
                default: return TargetRangeType.MeleePhysical;
            }
        }
    }
}

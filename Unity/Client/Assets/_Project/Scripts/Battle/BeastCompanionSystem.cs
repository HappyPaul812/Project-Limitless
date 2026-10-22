using System;
using System.Collections.Generic;
using ProjectLimitless.Core;
using ProjectLimitless.Monster;
using UnityEngine;
using System.Linq;

namespace ProjectLimitless.Battle
{
    /// <summary>
    /// 사수 전용 야수의 전투 연출 데이터입니다. 야수는 파티 동료가 아니라 스킬 연출에 참여하는 존재이므로
    /// Combatant를 상속하지 않으며 HP, 독립 턴, Formation 슬롯도 갖지 않습니다.
    /// </summary>
    public sealed class BeastCompanionDefinition
    {
        public BeastCompanionDefinition(string id, string displayName, string runResourcePath,
            int frameWidth, int iconFrameIndex, float framesPerSecond, float displayScale, float travelSpeed,
            string iconResourcePath = null, int iconFrameWidth = 0)
        {
            Id = id ?? string.Empty;
            DisplayName = displayName ?? string.Empty;
            RunResourcePath = runResourcePath ?? string.Empty;
            FrameWidth = Math.Max(1, frameWidth);
            IconResourcePath = iconResourcePath ?? RunResourcePath;
            IconFrameWidth = iconFrameWidth > 0 ? iconFrameWidth : FrameWidth;
            IconFrameIndex = Math.Max(0, iconFrameIndex);
            FramesPerSecond = Math.Max(1f, framesPerSecond);
            DisplayScale = Math.Max(.1f, displayScale);
            TravelSpeed = Math.Max(1f, travelSpeed);
        }

        public string Id { get; }
        public string DisplayName { get; }
        public string RunResourcePath { get; }
        public int FrameWidth { get; }
        // 승인 Run 교체가 기존 스킬/시설 아이콘을 바꾸지 않도록 아이콘 원본 규격을 별도로 보존합니다.
        public string IconResourcePath { get; }
        public int IconFrameWidth { get; }
        /// <summary>
        /// Run 시트 중 작은 버튼에서도 몸통·머리·꼬리·다리가 잘 구분되는 프레임 번호입니다.
        /// 야수별 실루엣이 다르므로 UI 코드에 Wolf 전용 숫자를 넣지 않고 야수 데이터가 직접 제공합니다.
        /// </summary>
        public int IconFrameIndex { get; }
        public float FramesPerSecond { get; }
        public float DisplayScale { get; }
        public float TravelSpeed { get; }
        public MonsterDefinition Monster { get; private set; }

        public static BeastCompanionDefinition FromMonster(MonsterDefinition monster)
        {
            if (monster == null) return null;
            var definition = new BeastCompanionDefinition(monster.MonsterId, monster.DisplayName,
                string.Empty, 64, 0, monster.AnimationFramesPerSecond, 1.5f, 570f);
            definition.Monster = monster;
            return definition;
        }
    }

    /// <summary>
    /// 야수 ID와 연출 자료를 한곳에서 관리합니다. 전투 화면은 현재 사수의 장착 ID만 조회합니다.
    /// </summary>
    public static class BeastCompanionCatalog
    {
        private static readonly BeastCompanionDefinition DefaultWolf = new BeastCompanionDefinition(
            // Run 재생은 기존 12 FPS를 유지하고, 실제 Transform 이동만 최초 구현 속도의 3/4로 사용합니다.
            // 프레임 속도와 이동 속도가 분리되어 있으므로 발 동작은 유지하면서 돌진 거리만 천천히 이동합니다.
            // 0부터 세는 4번 프레임(실제 다섯 번째)은 몸통과 네 다리의 간격이 비교적 분명해
            // 40px 안팎의 작은 스킬 버튼에서도 달리는 늑대 형태를 알아보기 쉽습니다.
            // 승인 512×384 셀은 0.25배로 128×96 UI 영역에 표시합니다. 기존 아이콘 64px 원본은 유지합니다.
            "wolf", "Wolf", "CompanionAssaultValidation/Wolf_Run6_v5", 512, 4, 12f, .25f, 760f * 3f / 4f,
            "CompanionAssaultValidation/Wolf_Run", 64);
        private static readonly BeastCompanionDefinition Bear = new BeastCompanionDefinition(
            // 최종 승인 v4 달리기만 128×96으로 연결하고, 시설/스킬 아이콘은 기존 64px 원본을 보존합니다.
            "bear", "Bear", "CompanionAssaultValidation/Bear_Run6_v4", 512, 0, 12f, .25f, 570f,
            "CompanionAssaultValidation/Bear_Run", 64);
        private static readonly BeastCompanionDefinition Fox = new BeastCompanionDefinition(
            // 승인 v7 달리기는 512px 셀을 0.25배로 표시하며, 기존 아이콘 원본과 저장 ID는 유지합니다.
            "fox", "Fox", "CompanionAssaultValidation/Fox_Run6_v7", 512, 0, 12f, .25f, 570f,
            "CompanionAssaultValidation/Fox_Run", 64);
        private static readonly Dictionary<string, BeastCompanionDefinition> Definitions =
            new Dictionary<string, BeastCompanionDefinition>(StringComparer.Ordinal)
            { { DefaultWolf.Id, DefaultWolf }, { Bear.Id, Bear }, { Fox.Id, Fox } };

        /// <summary>
        /// 현재 기본 야수를 UI 아이콘 같은 비전투 표시에도 제공합니다. 호출자는 Wolf 경로나 프레임 번호를
        /// 직접 알 필요가 없으므로, 향후 기본 또는 장착 야수가 Bear/Fox로 바뀌면 이 경계만 교체하면 됩니다.
        /// </summary>
        public static BeastCompanionDefinition GetDefaultDefinition()
        {
            return DefaultWolf;
        }

        public static BeastCompanionDefinition GetEquippedOrDefault(Combatant owner)
        {
            if (owner == null || owner.Side != BattleSide.Allies) return DefaultWolf;
            return Get(BeastCompanionService.GetEquippedId(owner.Id));
        }

        public static BeastCompanionDefinition Get(string id) =>
            id != null && Definitions.TryGetValue(id, out BeastCompanionDefinition definition)
                ? definition : ResolveMonster(id);

        private static BeastCompanionDefinition ResolveMonster(string id)
        {
            if (!BeastCompanionService.IsMonsterPet(id)) return DefaultWolf;
            MonsterDefinition monster = Resources.LoadAll<MonsterDefinition>("MonsterDefinitions")
                .FirstOrDefault(item => item.MonsterId == id);
            return BeastCompanionDefinition.FromMonster(monster) ?? DefaultWolf;
        }

        /// <summary>몬스터 원본의 걷기/대기 Sprite를 순서대로 재사용합니다. 시트만 있을 때만 임시 프레임을 만듭니다.</summary>
        public static Sprite[] GetMonsterFrames(MonsterDefinition monster, out bool generated)
        {
            generated = false;
            if (monster == null) return Array.Empty<Sprite>();
            if (monster.ExplicitWalkFrames.Length > 0) return monster.ExplicitWalkFrames;
            string resourcePath = !string.IsNullOrWhiteSpace(monster.WalkFrameResourcePath)
                ? monster.WalkFrameResourcePath : monster.IdleFrameResourcePath;
            if (!string.IsNullOrWhiteSpace(resourcePath))
            {
                Sprite[] loaded = Resources.LoadAll<Sprite>(resourcePath);
                Array.Sort(loaded, (left, right) => string.CompareOrdinal(left.name, right.name));
                if (loaded.Length > 0) return loaded;
            }
            if (monster.ExplicitIdleFrames.Length > 0) return monster.ExplicitIdleFrames;
            Texture2D sheet = monster.IdleSpriteSheet;
            Vector2Int size = monster.IdleFrameSize;
            if (sheet != null && size.x > 0 && size.y > 0 && monster.IdleFrameCount > 0)
            {
                var frames = new Sprite[monster.IdleFrameCount];
                for (int index = 0; index < frames.Length; index++)
                {
                    int column = index % monster.IdleColumns;
                    int row = index / monster.IdleColumns;
                    frames[index] = Sprite.Create(sheet,
                        new Rect(column * size.x, sheet.height - (row + 1) * size.y, size.x, size.y),
                        new Vector2(.5f, .5f), size.x);
                }
                generated = true;
                return frames;
            }
            return monster.FieldSprite == null ? Array.Empty<Sprite>() : new[] { monster.FieldSprite };
        }
    }
}

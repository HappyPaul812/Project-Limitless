using System.Collections.Generic;
using UnityEngine;

namespace ProjectLimitless.Battle
{
    /// <summary>
    /// 전투 UI에서 사용하는 아이콘 이름과 실제 Kenney Sprite 경로를 한곳에서 연결합니다.
    ///
    /// BattleSceneController가 PNG 파일명을 직접 알게 만들면 아이콘을 교체할 때 화면 구성 코드까지
    /// 찾아 고쳐야 합니다. 이 카탈로그를 사이에 두면 버튼과 HUD는 "공격", "방어" 같은 역할만 요청하고,
    /// 어떤 원본 Sprite를 보여 줄지는 이 파일에서 바꿀 수 있습니다. 향후 BattleSkillDefinition에 아이콘
    /// 식별자를 추가할 때도 같은 Load 흐름을 재사용할 수 있습니다.
    /// </summary>
    public static class BattleUiIconCatalog
    {
        public const string Attack = "command.attack";
        public const string Skill = "command.skill";
        public const string Defend = "command.defend";
        public const string Flee = "command.flee";
        public const string Cancel = "command.cancel";
        public const string Acting = "status.acting";
        public const string Taunt = "status.taunt";
        public const string CooldownStart = "status.cooldown.start";
        public const string CooldownProgress = "status.cooldown.progress";
        public const string CooldownEnd = "status.cooldown.end";
        public const string GuardianTauntSkill = "skill.guardian.taunt";
        public const string HealerHealingLightSkill = "skill.healer.healing_light";
        public const string SharpshooterAimSkill = "skill.sharpshooter.aim";
        public const string FighterEdgeSkill = "skill.fighter.edge";

        // Resources 폴더 아래의 상대 경로만 보관합니다. 원본 압축과 라이선스는 ThirdParty에 그대로
        // 보존하고, 런타임에 필요한 PNG만 Resources에서 Sprite로 불러오는 구조입니다.
        private static readonly Dictionary<string, string> ResourcePaths = new Dictionary<string, string>
        {
            { Attack, "KenneyBattleIcons/sword" },
            { Skill, "KenneyBattleIcons/star" },
            { Defend, "KenneyBattleIcons/shield" },
            { Flee, "KenneyBattleIcons/exitRight" },
            // 취소는 X보다 "한 단계 뒤로 이동" 의미가 분명한 왼쪽 화살표를 사용합니다.
            // cross.png는 투사 '난도'의 준비 동작과 누적되는 전투 감각을 나타내는 스킬·상태 아이콘으로 씁니다.
            { Cancel, "KenneyBattleIcons/arrowLeft" },
            { Acting, "KenneyBattleIcons/arrowRight" },
            // 도발은 조준점보다 "한 참가자가 특정 방향을 향하게 됨"을 보여 주는 pawn_right를 사용합니다.
            // 실제 강제 대상 판정은 Combatant가 담당하고, 이 경로는 그 결과를 읽어 보여 주기만 합니다.
            { Taunt, "KenneyBattleIcons/pawn_right" },
            { CooldownStart, "KenneyBattleIcons/hourglass_top" },
            { CooldownProgress, "KenneyBattleIcons/hourglass" },
            { CooldownEnd, "KenneyBattleIcons/hourglass_bottom" },
            // 아래 세 항목은 스킬 버튼 전용입니다. 같은 도발이라도 적에게 남은 상태는 pawn_right,
            // 수호자가 누르는 스킬은 pawn_left를 사용해 서로 다른 화면 역할을 구분합니다.
            { GuardianTauntSkill, "KenneyBattleIcons/pawn_left" },
            { HealerHealingLightSkill, "KenneyBattleIcons/suit_hearts" },
            { SharpshooterAimSkill, "KenneyBattleIcons/target" },
            { FighterEdgeSkill, "KenneyBattleIcons/cross" }
        };

        private static readonly Dictionary<string, Sprite> Cache = new Dictionary<string, Sprite>();

        /// <summary>
        /// 역할 식별자에 맞는 Sprite를 한 번만 읽고 재사용합니다.
        /// 에셋이 빠졌거나 Import가 아직 끝나지 않았으면 null을 반환합니다. 호출 쪽은 유니코드 기호를
        /// 대신 만들지 않고 한글 텍스트만 남기므로, 아이콘 문제 때문에 명령 자체가 사라지지 않습니다.
        /// </summary>
        public static Sprite Load(string iconId)
        {
            if (string.IsNullOrEmpty(iconId) || !ResourcePaths.TryGetValue(iconId, out string resourcePath))
                return null;
            if (Cache.TryGetValue(iconId, out Sprite cached)) return cached;

            Sprite sprite = Resources.Load<Sprite>(resourcePath);
            Cache[iconId] = sprite;
            return sprite;
        }

        /// <summary>
        /// 화면에 표시되는 남은 턴 값만 보고 모래시계 단계를 고릅니다.
        /// 예를 들어 총 3턴이면 3은 막 시작한 top, 2는 진행 중인 기본 모양, 1은 다음 감소 때
        /// 사라지는 bottom입니다. 여기서는 쿨타임을 감소시키지 않으므로 전투 계산과 UI 표현이 분리됩니다.
        /// </summary>
        public static string GetCooldownIconId(int remainingTurns, int totalTurns)
        {
            if (remainingTurns <= 1) return CooldownEnd;
            if (totalTurns > 0 && remainingTurns >= totalTurns) return CooldownStart;
            return CooldownProgress;
        }
    }
}

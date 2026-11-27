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
        public const string Item = "command.item";
        public const string Defend = "command.defend";
        public const string Flee = "command.flee";
        public const string Cancel = "command.cancel";
        public const string Acting = "status.acting";
        public const string Taunt = "status.taunt";
        public const string CooldownStart = "status.cooldown.start";
        public const string CooldownProgress = "status.cooldown.progress";
        public const string CooldownEnd = "status.cooldown.end";
        public const string GuardianTauntSkill = "skill.guardian.taunt";
        public const string GuardianIronWallSkill = "skill.guardian.iron_wall";
        public const string IronWall = "status.guardian.iron_wall";
        public const string GuardianCoverAlliesSkill = "skill.guardian.cover_allies";
        public const string GuardianCover = "status.guardian.cover_allies";
        public const string HealerHealingLightSkill = "skill.healer.healing_light";
        public const string HealerHealingWaveSkill = "skill.healer.healing_wave";
        public const string HealerCleanseSkill = "skill.healer.cleanse";
        public const string SharpshooterAimSkill = "skill.sharpshooter.aim";
        public const string SharpshooterArrowRainSkill = "skill.sharpshooter.arrow_rain";
        public const string SharpshooterCompanionAssaultSkill = "skill.sharpshooter.companion_assault";
        public const string FighterNandoSkill = "skill.fighter.nando";
        public const string FighterMomentum = "status.fighter.momentum";
        public const string FighterCriticalStrikeSkill = "skill.fighter.critical_strike";
        public const string FighterWhirlwindSkill = "skill.fighter.whirlwind";
        public const string MageFireballSkill = "skill.mage.fireball";
        public const string Burn = "status.burn";
        public const string MageThunderboltSkill = "skill.mage.thunderbolt";
        public const string Shock = "status.shock";
        public const string MageGaiaWallSkill = "skill.mage.gaia_wall";
        public const string GaiaWall = "status.gaia_wall";
        public const string Poison = "status.poison";

        // Resources 폴더 아래의 상대 경로만 보관합니다. 원본 압축과 라이선스는 ThirdParty에 그대로
        // 보존하고, 런타임에 필요한 PNG만 Resources에서 Sprite로 불러오는 구조입니다.
        private const string CriticalStrikeIconPath = "BattleItemIcons/Fighter_CriticalStrike_v2_128_RGBA";

        private const string FireballIconPath = "BattleItemIcons/Mage_FireBall_v1_KEEP_128_RGBA";
        private const string GaiaWallIconPath = "BattleItemIcons/Mage_GaiaWell_v3_128_RGBA";

        private static readonly Dictionary<string, string> ResourcePaths = new Dictionary<string, string>
        {
            { Attack, "BattleItemIcons/AttackButton_Original_128_RGBA" },
            { Skill, "BattleItemIcons/SkillButton_Original_128_RGBA" },
            { Item, "BattleItemIcons/Battle_Item_Icons_128x128_Sheet" },
            { Defend, "BattleItemIcons/DefendButton_Original_128_RGBA" },
            { Flee, "BattleItemIcons/FleeButton_v2_128_RGBA" },
            // 취소는 X보다 "한 단계 뒤로 이동" 의미가 분명한 왼쪽 화살표를 사용합니다.
            // cross.png는 투사의 공격 스킬 '난도'에만 사용하고, 전투 자원 기세와 회심의 일격은 skull로 구분합니다.
            { Cancel, "BattleItemIcons/CancelButton_Original_128_RGBA" },
            { Acting, "KenneyBattleIcons/arrowRight" },
            // 도발은 조준점보다 "한 참가자가 특정 방향을 향하게 됨"을 보여 주는 pawn_right를 사용합니다.
            // 실제 강제 대상 판정은 Combatant가 담당하고, 이 경로는 그 결과를 읽어 보여 주기만 합니다.
            { Taunt, "KenneyBattleIcons/pawn_right" },
            { CooldownStart, "KenneyBattleIcons/hourglass_top" },
            { CooldownProgress, "KenneyBattleIcons/hourglass" },
            { CooldownEnd, "KenneyBattleIcons/hourglass_bottom" },
            // 도발 스킬은 전용 원화를 사용하고, 적에게 남는 상태는 pawn_right를 사용합니다.
            { GuardianTauntSkill, "BattleItemIcons/Guardian_Taunt_v1_KEEP_128_RGBA" },
            // 철벽 버튼과 상태 HUD는 같은 실제 성벽 아이콘을 사용해 두 화면의 의미를 일치시킵니다.
            // 성벽 실루엣으로 기본 방어 및 가이아 웰과 구분합니다.
            { GuardianIronWallSkill, "BattleItemIcons/Guardian_IronWall_v2_128_RGBA" },
            { IronWall, "BattleItemIcons/Guardian_IronWall_v2_128_RGBA" },
            // 수호의 맹세는 보호막 원화를 스킬과 상태 HUD에서 공유합니다.
            { GuardianCoverAlliesSkill, "BattleItemIcons/Guardian_CoverAllies_v2_128_RGBA" },
            { GuardianCover, "BattleItemIcons/Guardian_CoverAllies_v2_128_RGBA" },
            { HealerHealingLightSkill, "BattleItemIcons/Healer_HealingLight_v2_128_RGBA" },
            { HealerHealingWaveSkill, "BattleItemIcons/Healer_HealingWave_v1_KEEP_128_RGBA" },
            { HealerCleanseSkill, "BattleItemIcons/Healer_Cleanse_v2_128_RGBA" },
            { SharpshooterAimSkill, "BattleItemIcons/Sharpshooter_AimedShot_Original_128_RGBA" },
            { SharpshooterArrowRainSkill, "BattleItemIcons/Sharpshooter_ArrowRain_Original_128_RGBA" },
            // 동료의 습격은 장착 야수와 무관한 공통 버튼 그림입니다. 야수별 Run은 전투 연출에서 사용합니다.
            { SharpshooterCompanionAssaultSkill, "BattleItemIcons/Sharpshooter_CompanionAssault_v2_128_RGBA" },
            { FighterNandoSkill, "BattleItemIcons/Fighter_Nando_v2_128_RGBA" },
            // 회심의 일격과 기세 HUD는 같은 Sprite를 참조해 기세의 사용처를 보여 줍니다.
            { FighterMomentum, CriticalStrikeIconPath },
            { FighterCriticalStrikeSkill, CriticalStrikeIconPath },
            { FighterWhirlwindSkill, "BattleItemIcons/Fighter_Whirlwind_v1_KEEP_128_RGBA" },
            // 파이어 볼과 화상, 가이아 웰과 방어 상태는 각각 같은 승인 Sprite를 공유합니다.
            { MageFireballSkill, FireballIconPath },
            { Burn, FireballIconPath },
            { MageThunderboltSkill, "BattleItemIcons/Mage_Thunderbolt_v2_128_RGBA" },
            { Shock, "KenneyBattleIcons/power" },
            { MageGaiaWallSkill, GaiaWallIconPath },
            { GaiaWall, GaiaWallIconPath },
            { Poison, string.Empty }
        };

        private static readonly Dictionary<string, Sprite> Cache = new Dictionary<string, Sprite>();

        /// <summary>스킬 버튼은 상세·상태와 같은 로더를 사용하며, Play 전환 뒤 무효화된 캐시만 복구합니다.</summary>
        public static Sprite LoadSkillButtonIcon(string iconId)
        {
            if (!string.IsNullOrEmpty(iconId) && Cache.TryGetValue(iconId, out Sprite existing) &&
                (existing == null || existing.texture == null)) Cache.Remove(iconId);
            return Load(iconId);
        }

        /// <summary>
        /// 역할 식별자에 맞는 Sprite를 한 번만 읽고 재사용합니다.
        /// 에셋이 빠졌거나 Import가 아직 끝나지 않았으면 null을 반환합니다. 호출 쪽은 유니코드 기호를
        /// 대신 만들지 않고 한글 텍스트만 남기므로, 아이콘 문제 때문에 명령 자체가 사라지지 않습니다.
        /// </summary>
        public static Sprite Load(string iconId)
        {
            if (iconId == Item)
            {
                Sprite[] itemIcons = Resources.LoadAll<Sprite>("BattleItemIcons/Battle_Item_Icons_128x128_Sheet");
                for (int index = 0; index < itemIcons.Length; index++)
                    if (itemIcons[index].name == "Battle_Item_Icon") return itemIcons[index];
                return null;
            }
            if (string.IsNullOrEmpty(iconId) || !ResourcePaths.TryGetValue(iconId, out string resourcePath))
                return null;
            if (Cache.TryGetValue(iconId, out Sprite cached)) return cached;

            Sprite sprite;
            if (iconId == Poison)
            {
                // Venom Ward 16장 중 녹색 고리가 가장 조밀한 index 6을 고정 아이콘으로 사용합니다.
                // 독 틱의 Acid Splash 재생과 분리해 HUD가 애니메이션처럼 깜박이지 않게 합니다.
                sprite = BattlePoisonVisuals.LoadStatusIcon();
            }
            else
            {
                sprite = Resources.Load<Sprite>(resourcePath);
            }
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

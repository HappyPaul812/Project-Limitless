using System;
using System.Collections.Generic;
using System.Linq;
using ProjectLimitless.Core;

namespace ProjectLimitless.Battle
{
    /// <summary>실제 전투에서 실행할 스킬 효과 종류입니다. 구현된 효과만 이 열거형에 추가합니다.</summary>
    public enum BattleSkillEffectType
    {
        None,
        Taunt,
        SingleAllyHeal,
        AreaAllyHeal,
        SingleRangedPhysicalAttack,
        SingleMeleePhysicalAttackWithMomentumGain,
        SingleMeleePhysicalAttackConsumingMomentum,
        AreaMeleePhysicalAttackWithMomentumGain,
        AreaRangedPhysicalAttack,
        SingleBeastPhysicalAttack,
        SingleMagicAttackWithBurn,
        AreaMagicAttackWithShock,
        SelfDamageReduction,
        GuardianIronWall,
        GuardianCoverAllies,
        RemoveAllHarmfulStatuses
    }

    /// <summary>
    /// 광역 스킬이 어느 적 행을 공격하는지 나타내는 데이터입니다. 기본 공격의 TargetRangeType은
    /// "전열이 비면 노출된 후열을 근거리로 공격" 같은 접근 규칙을 담당하지만, 스킬 범위는 회오리 베기처럼
    /// 전열에 고정될 수 있습니다. 두 의미를 분리해야 기본 공격 규칙을 바꾸지 않고 화살비·썬더볼트도
    /// 각각 EnemyRearRowAll·EnemyAll 데이터만 지정해 같은 해석기를 재사용할 수 있습니다.
    /// </summary>
    public enum BattleSkillTargetRange
    {
        None,
        EnemyFrontRowAll,
        EnemyRearRowAll,
        EnemyAll
    }

    /// <summary>JobDefinition의 프리뷰와 전투 실행 정보를 연결하는 읽기 전용 런타임 스킬 데이터입니다.</summary>
    public sealed class BattleSkillDefinition
    {
        public BattleSkillDefinition(string id, string displayName, string description, bool implemented,
            BattleSkillEffectType effectType, int cooldownTurns, int effectDuration, float maxHpHealRatio = 0f,
            int attackDamagePercent = 0, string iconId = null, string targetDescription = null,
            string effectDescription = null, string typeDescription = null, string durationDescription = null,
            IReadOnlyList<int> momentumDamagePercents = null, BattleSkillTargetRange targetRange = BattleSkillTargetRange.None,
            int burnDamagePercent = 0)
        {
            Id = id ?? string.Empty;
            DisplayName = displayName ?? string.Empty;
            Description = description ?? string.Empty;
            IsImplemented = implemented;
            EffectType = effectType;
            CooldownTurns = Math.Max(0, cooldownTurns);
            EffectDuration = Math.Max(0, effectDuration);
            MaxHpHealRatio = Math.Max(0f, maxHpHealRatio);
            AttackDamagePercent = Math.Max(0, attackDamagePercent);
            IconId = iconId ?? string.Empty;
            TargetDescription = targetDescription ?? string.Empty;
            EffectDescription = effectDescription ?? string.Empty;
            TypeDescription = typeDescription ?? string.Empty;
            DurationDescription = durationDescription ?? string.Empty;
            MomentumDamagePercents = momentumDamagePercents ?? Array.Empty<int>();
            TargetRange = targetRange;
            BurnDamagePercent = Math.Max(0, burnDamagePercent);
        }

        public string Id { get; }
        public string DisplayName { get; }
        public string Description { get; }
        public bool IsImplemented { get; }
        public BattleSkillEffectType EffectType { get; }
        public int CooldownTurns { get; }
        public int EffectDuration { get; }
        /// <summary>최대 HP 중 몇 %를 회복할지 나타내는 데이터입니다. 0.35는 최대 HP의 35%입니다.</summary>
        public float MaxHpHealRatio { get; }
        /// <summary>기본 공격력에 적용할 정수 퍼센트입니다. 160은 기본 공격 피해의 160%입니다.</summary>
        public int AttackDamagePercent { get; }
        /// <summary>
        /// 스킬 버튼에 표시할 그림의 역할 식별자입니다. PNG 파일명 자체를 넣지 않고 카탈로그의 ID를
        /// 저장하므로, 화면 코드는 직업명이나 스킬명을 비교하지 않아도 알맞은 Sprite를 찾을 수 있습니다.
        /// 나중에 그림을 교체할 때는 스킬 데이터나 아이콘 카탈로그만 바꾸면 됩니다.
        /// </summary>
        public string IconId { get; }
        /// <summary>
        /// 아래 문자열은 버튼이 아니라 설명 팝업에서 사용하는 표시 데이터입니다. 전투 화면은 스킬 이름을
        /// 비교하지 않고 이 값을 순서대로 보여 주므로, 향후 난도나 파이어 볼도 같은 팝업을 재사용할 수 있습니다.
        /// 실제 대상 판정과 수치 계산은 기존 Executor와 TargetResolver가 계속 담당합니다.
        /// </summary>
        public string TargetDescription { get; }
        public string EffectDescription { get; }
        public string TypeDescription { get; }
        public string DurationDescription { get; }
        /// <summary>
        /// 기세 0부터 차례대로 적용할 피해 배율입니다. 회심의 일격 데이터만 네 값을 가지며, UI와 Executor가
        /// 같은 목록을 읽기 때문에 설명의 수치와 실제 피해가 따로 어긋나지 않습니다.
        /// </summary>
        public IReadOnlyList<int> MomentumDamagePercents { get; }
        /// <summary>광역 스킬의 행 범위입니다. UI 문구가 아니라 전투 대상 해석기가 읽는 실행 데이터입니다.</summary>
        public BattleSkillTargetRange TargetRange { get; }
        /// <summary>화상 한 번이 명중 당시 공격력의 몇 %인지 나타냅니다. 30이면 당시 Attack의 30%입니다.</summary>
        public int BurnDamagePercent { get; }
    }

    /// <summary>
    /// 직업의 시작 스킬 프리뷰를 실제 전투 메뉴용 데이터로 변환합니다.
    /// 아직 구현되지 않은 스킬도 목록에는 남기되 실행 가능 상태로 만들지 않습니다.
    /// </summary>
    public static class BattleSkillCatalog
    {
        public const string GuardianTauntId = "guardian_taunt";
        public const string GuardianIronWallId = "guardian_iron_defense";
        public const string GuardianCoverAlliesId = "guardian_intercept";
        public const string HealerHealingLightId = "healer_healing_light";
        public const string HealerHealingWaveId = "healer_healing_wave";
        public const string HealerCleanseId = "healer_cleanse";
        public const string SharpshooterAimId = "sharpshooter_aim";
        public const string SharpshooterArrowRainId = "sharpshooter_arrow_rain";
        public const string SharpshooterCompanionAssaultId = "sharpshooter_companion_attack";
        public const string FighterNandoId = "fighter_slash_stack";
        public const string FighterCriticalStrikeId = "fighter_finishing_strike";
        public const string FighterWhirlwindId = "fighter_whirlwind";
        public const string MageFireballId = "mage_fireball";
        public const string MageThunderboltId = "mage_thunderbolt";
        public const string MageGaiaWallId = "mage_gaia_wall";

        // 광역 회복 수치는 단일 회복의 기준값에서 파생합니다. 치유의 빛이 조정되면 회복의 파동도
        // 같은 비율로 따라가므로 두 스킬의 밸런스가 서로 다른 숫자로 흩어지지 않습니다.
        public const float HealingLightMaxHpHealRatio = .35f;
        public const float HealingWaveOfHealingLightRatio = .6f;

        public static IReadOnlyList<BattleSkillDefinition> GetSkills(JobDefinition job)
        {
            if (job == null) return Array.Empty<BattleSkillDefinition>();

            return job.StartingSkills.Select(preview =>
            {
                if (preview.SkillId == GuardianTauntId)
                    return new BattleSkillDefinition(preview.SkillId, preview.SkillName,
                        "적 전체를 도발합니다.\n도발에 걸린 각 적의 다음 2회 행동 동안 단일 적대 행동의 대상이 수호자로 강제됩니다.\n광역 공격에는 적용되지 않습니다.",
                        true, BattleSkillEffectType.Taunt, 3, 2,
                        iconId: BattleUiIconCatalog.GuardianTauntSkill,
                        targetDescription: "대상: 적 전체",
                        effectDescription: "효과: 단일 적대 행동의 대상 강제",
                        durationDescription: "지속: 각 적의 다음 2회 행동");
                if (preview.SkillId == GuardianIronWallId)
                    return new BattleSkillDefinition(preview.SkillId, "철벽",
                        "굳건한 방벽으로 몸을 지켜 받는 피해를 크게 줄입니다.", true,
                        BattleSkillEffectType.GuardianIronWall, 4, 2,
                        iconId: BattleUiIconCatalog.GuardianIronWallSkill,
                        targetDescription: "대상: 자신",
                        effectDescription: "효과: 받는 피해 70% 감소",
                        typeDescription: "유형: 자기 보호",
                        durationDescription: "지속: 자신의 다음 2회 행동\n재사용 대기시간: 4턴\n공용 방어와 중첩 불가");
                if (preview.SkillId == GuardianCoverAlliesId)
                    return new BattleSkillDefinition(preview.SkillId, "대신 막기",
                        "동료들의 피해를 대신 감당합니다.\n수호자의 다음 행동 전까지 살아 있는 아군 전체가 직접 공격과 공격 스킬로 받는 피해를 최대 50% 줄이고, 줄인 피해의 절반을 수호자가 대신 받습니다.", true,
                        BattleSkillEffectType.GuardianCoverAllies, 4, 0,
                        iconId: BattleUiIconCatalog.GuardianCoverAlliesSkill,
                        targetDescription: "대상: 자신을 제외한 살아 있는 아군 전체",
                        effectDescription: "효과: 직접 피해 최대 50% 감소\n이전 피해: 감소량의 50%\n이전 상한: 수호자 최대 HP의 40%\nDoT 보호 불가",
                        typeDescription: "유형: 광역 보호",
                        durationDescription: "지속: 수호자의 다음 행동 전까지\n재사용 대기시간: 4턴");
                if (preview.SkillId == HealerHealingLightId)
                    // 1차 밸런스 값 35%는 화면 코드가 아니라 스킬 정의에 둡니다. 나중에 수치를 조정해도
                    // 대상 선택이나 VFX 코드를 다시 고칠 필요가 없습니다. 별도 쿨타임은 현재 기획에 없어 0입니다.
                    return new BattleSkillDefinition(preview.SkillId, preview.SkillName,
                        "살아 있는 아군 1명의 HP를 대상 최대 HP의 35%만큼 회복합니다.\n전투불능 상태의 아군은 대상으로 선택할 수 없습니다.", true,
                        BattleSkillEffectType.SingleAllyHeal, 0, 0, HealingLightMaxHpHealRatio,
                        iconId: BattleUiIconCatalog.HealerHealingLightSkill,
                        targetDescription: "대상: 살아 있는 아군 1명",
                        effectDescription: "회복량: 최대 HP의 35%");
                if (preview.SkillId == HealerHealingWaveId)
                    return new BattleSkillDefinition(preview.SkillId, preview.SkillName,
                        "치유사를 중심으로 회복의 파동을 일으켜 살아 있는 아군 전체의 HP를 회복합니다.\n전투불능 아군은 회복하거나 부활시키지 않습니다.", true,
                        BattleSkillEffectType.AreaAllyHeal, 3, 0,
                        HealingLightMaxHpHealRatio * HealingWaveOfHealingLightRatio,
                        iconId: BattleUiIconCatalog.HealerHealingWaveSkill,
                        targetDescription: "대상: 살아 있는 아군 전체(자신 포함)",
                        effectDescription: "효과: 치유의 빛 기본 회복량의 60%",
                        typeDescription: "유형: 광역 회복",
                        durationDescription: "재사용 대기시간: 3턴");
                if (preview.SkillId == HealerCleanseId)
                    return new BattleSkillDefinition(preview.SkillId, preview.SkillName,
                        "빛으로 아군을 정화하여 해로운 상태이상을 모두 제거합니다.", true,
                        BattleSkillEffectType.RemoveAllHarmfulStatuses, 2, 0,
                        iconId: BattleUiIconCatalog.HealerCleanseSkill,
                        targetDescription: "대상: 살아 있는 아군 1명",
                        effectDescription: "효과: 해로운 상태이상 모두 제거\n현재 제거 가능: 독 / 화상 / 감전",
                        typeDescription: "유형: 상태이상 해제",
                        durationDescription: "재사용 대기시간: 2턴");
                if (preview.SkillId == SharpshooterAimId)
                    // 기본 공격력과 스킬 배율을 분리하면 캐릭터 성장으로 Attack이 달라져도 정조준은 항상
                    // 그 시점 기본 공격의 160%를 사용합니다. 성공 직후 쿨타임 2를 저장하고 사수의 다음 행동
                    // 시작에 1, 그다음 시작에 0이 되므로 HUD와 실행기가 같은 2턴 흐름을 공유합니다.
                    return new BattleSkillDefinition(preview.SkillId, preview.SkillName,
                        "정확히 조준해 강력한 화살을 발사합니다.\n후열 적이 살아 있으면 후열만 공격할 수 있고, 후열이 전멸하면 전열을 공격할 수 있습니다.", true,
                        BattleSkillEffectType.SingleRangedPhysicalAttack, 2, 0, 0f, 160,
                        BattleUiIconCatalog.SharpshooterAimSkill,
                        targetDescription: "대상: 적 1명",
                        effectDescription: "피해: 기본 공격의 160%",
                        typeDescription: "유형: 원거리 물리");
                if (preview.SkillId == SharpshooterArrowRainId)
                    return new BattleSkillDefinition(preview.SkillId, preview.SkillName,
                        "화살을 하늘로 쏘아 올려 적 후열에 비처럼 쏟아냅니다.\n살아 있는 적 후열 전체에 일반 공격의 120% 피해를 줍니다.", true,
                        // 대상당 120%라는 공격 성능은 유지하고, 강한 후열 광역기를 반복하는 빈도만
                        // 사수 행동 기준 2턴으로 제한합니다. 수치와 사용 빈도를 데이터에서 따로 조절하면
                        // 향후 밸런스 테스트에서도 피해 계산이나 Projectile 연출을 다시 고칠 필요가 없습니다.
                        BattleSkillEffectType.AreaRangedPhysicalAttack, 2, 0, 0f, 120,
                        iconId: BattleUiIconCatalog.SharpshooterArrowRainSkill,
                        targetDescription: "대상: 적 후열 전체",
                        effectDescription: "피해: 일반 공격의 120%",
                        typeDescription: "유형: 원거리 물리",
                        durationDescription: "재사용 대기시간: 2턴",
                        targetRange: BattleSkillTargetRange.EnemyRearRowAll);
                if (preview.SkillId == SharpshooterCompanionAssaultId)
                    // 야수의 종류는 이 스킬 정의나 BattleSceneController에 넣지 않습니다. 현재 장착 야수를
                    // BeastCompanionCatalog에서 받도록 분리해 두면 Bear/Fox가 추가되어도 대상·피해 코드는 같습니다.
                    return new BattleSkillDefinition(preview.SkillId, preview.SkillName,
                        "야수 동료에게 명령해 적 하나를 습격하게 합니다.\n야수가 직접 전장으로 달려들어 일반 공격의 180% 피해를 줍니다.", true,
                        BattleSkillEffectType.SingleBeastPhysicalAttack, 3, 0, 0f, 180,
                        iconId: BattleUiIconCatalog.SharpshooterCompanionAssaultSkill,
                        targetDescription: "대상: 적 1명",
                        effectDescription: "범위: 전열/후열 자유\n피해: 일반 공격의 180%",
                        typeDescription: "유형: 야수 / 단일 물리",
                        durationDescription: "재사용 대기시간: 3턴");
                if (preview.SkillId == FighterNandoId)
                    return new BattleSkillDefinition(preview.SkillId, preview.SkillName,
                        "적 1명에게 일반 공격의 150% 피해를 주고 기세를 1 얻습니다.\n기세는 최대 3까지 쌓이며 회심의 일격을 강화합니다.\n난도 스킬을 세 번째 직접 사용하면 2턴 동안 재사용할 수 없습니다.", true,
                        BattleSkillEffectType.SingleMeleePhysicalAttackWithMomentumGain, 2, 0, 0f, 150,
                        iconId: BattleUiIconCatalog.FighterNandoSkill,
                        targetDescription: "대상: 적 1명",
                        effectDescription: "피해: 일반 공격의 150%\n효과: 기세 +1\n기세 최대: 3",
                        typeDescription: "유형: 근거리 물리",
                        durationDescription: "세 번째 직접 사용 후 재사용: 2턴");
                if (preview.SkillId == FighterCriticalStrikeId)
                    return new BattleSkillDefinition(preview.SkillId, preview.SkillName,
                        "보유한 기세를 모두 소모하여 강력한 일격을 가합니다.\n기세가 높을수록 피해가 증가합니다.", true,
                        BattleSkillEffectType.SingleMeleePhysicalAttackConsumingMomentum, 0, 0,
                        iconId: BattleUiIconCatalog.FighterCriticalStrikeSkill,
                        targetDescription: "대상: 적 1명",
                        effectDescription: "기세 0: 일반 공격의 100%\n기세 1: 일반 공격의 130%\n기세 2: 일반 공격의 160%\n기세 3: 일반 공격의 190%\n효과: 공격 적중 후 기세 전부 소모",
                        typeDescription: "유형: 근거리 물리",
                        momentumDamagePercents: new[] { 100, 130, 160, 190 });
                if (preview.SkillId == FighterWhirlwindId)
                    return new BattleSkillDefinition(preview.SkillId, preview.SkillName,
                        "회전하며 적 전열 전체를 베어 각 대상에게 일반 공격의 80% 피해를 줍니다.\n실제로 적중한 적 수만큼 기세를 얻으며, 기세는 최대 3까지 쌓입니다.\n회오리 베기로 기세 3이 되어도 난도의 재사용 대기시간은 발생하지 않습니다.", true,
                        BattleSkillEffectType.AreaMeleePhysicalAttackWithMomentumGain, 0, 0, 0f, 80,
                        iconId: BattleUiIconCatalog.FighterWhirlwindSkill,
                        targetDescription: "대상: 적 전열 전체",
                        effectDescription: "피해: 적마다 일반 공격의 80%\n효과: 맞힌 적 1명당 기세 +1\n기세 최대: 3",
                        typeDescription: "유형: 광역 근거리 물리",
                        durationDescription: "재사용 대기시간: 없음",
                        targetRange: BattleSkillTargetRange.EnemyFrontRowAll);
                if (preview.SkillId == MageFireballId)
                    return new BattleSkillDefinition(preview.SkillId, preview.SkillName,
                        "불의 마력을 충전해 큰 화염탄을 발사합니다.\n적 하나에게 일반 공격의 170% 피해를 주고 화상을 2회 부여합니다.\n화상은 대상 행동 종료 시 명중 당시 공격력의 30% 피해를 주며, 다시 맞으면 중첩하지 않고 2회로 갱신됩니다.", true,
                        BattleSkillEffectType.SingleMagicAttackWithBurn, 3, 2, 0f, 170,
                        iconId: BattleUiIconCatalog.MageFireballSkill,
                        targetDescription: "대상: 적 1명",
                        effectDescription: "범위: 전열/후열 자유\n즉발 피해: 일반 공격의 170%\n화상: 대상 행동 종료 시 30% 피해 × 2회",
                        typeDescription: "유형: 단일 마법",
                        durationDescription: "재사용 대기시간: 3턴",
                        burnDamagePercent: 30);
                if (preview.SkillId == MageThunderboltId)
                    // 적 전열과 후열을 모두 덮는 기술은 한 번에 3~6명을 맞힐 수 있으므로 대상당 피해를
                    // 단일 공격보다 낮은 90%로 둡니다. 범위의 이득과 대상별 피해를 분리해 조정하는 값입니다.
                    return new BattleSkillDefinition(preview.SkillId, preview.SkillName,
                        "청백색 번개를 적 진영 전체에 떨어뜨립니다.\n살아 있는 모든 적에게 일반 공격의 90% 피해를 주고 감전 1을 부여합니다.", true,
                        BattleSkillEffectType.AreaMagicAttackWithShock, 3, 1, 0f, 90,
                        iconId: BattleUiIconCatalog.MageThunderboltSkill,
                        targetDescription: "대상: 적 전체",
                        effectDescription: "피해: 일반 공격의 90%\n효과: 감전 1\n감전: 다음 행동에서 주는 피해 15% 감소",
                        typeDescription: "유형: 광역 마법",
                        durationDescription: "재사용 대기시간: 3턴",
                        targetRange: BattleSkillTargetRange.EnemyAll);
                if (preview.SkillId == MageGaiaWallId)
                    return new BattleSkillDefinition(preview.SkillId, preview.SkillName,
                        "대지의 힘으로 자신을 보호합니다.\n다음 2회 행동 동안 받는 피해가 60% 감소합니다.", true,
                        BattleSkillEffectType.SelfDamageReduction, 4, 2,
                        iconId: BattleUiIconCatalog.MageGaiaWallSkill,
                        targetDescription: "대상: 자신",
                        effectDescription: "효과: 받는 피해 60% 감소",
                        typeDescription: "유형: 자기 보호",
                        durationDescription: "지속: 자신의 다음 2회 행동\n재사용 대기시간: 4턴");
                return new BattleSkillDefinition(preview.SkillId, preview.SkillName, preview.SkillDescription, false,
                    BattleSkillEffectType.None, 0, 0);
            }).ToArray();
        }
    }

    /// <summary>BattleSkillDefinition의 광역 범위 데이터를 실제 Formation 참가자 목록으로 변환합니다.</summary>
    public static class BattleSkillTargetResolver
    {
        public static IReadOnlyList<Combatant> ResolveHostileAreaTargets(BattleSkillDefinition skill, Formation opponents)
        {
            if (skill == null || opponents == null) return Array.Empty<Combatant>();

            // Formation 자체에 새 규칙을 넣지 않고 공개된 슬롯 조회만 사용합니다. 회오리 베기는 전열이
            // 비어도 후열로 범위를 넓히지 않습니다. 이는 "접근 가능한 적"을 찾는 기본 근거리 공격과 달리,
            // 스킬 데이터가 지정한 공간인 전열만 베는 광역 공격이기 때문입니다.
            IEnumerable<FormationRow> rows;
            switch (skill.TargetRange)
            {
                case BattleSkillTargetRange.EnemyFrontRowAll:
                    rows = new[] { FormationRow.Front };
                    break;
                case BattleSkillTargetRange.EnemyRearRowAll:
                    rows = new[] { FormationRow.Rear };
                    break;
                case BattleSkillTargetRange.EnemyAll:
                    rows = new[] { FormationRow.Front, FormationRow.Rear };
                    break;
                default:
                    return Array.Empty<Combatant>();
            }

            return rows.SelectMany(row => Enumerable.Range(0, 3)
                    .Select(column => opponents.Get(row, column)))
                .Where(target => target != null && target.IsAlive)
                .ToArray();
        }
    }

    /// <summary>
    /// 투사마다 자기 기세와 난도 스킬 직접 사용 횟수를 보관하는 전투 자원 저장소입니다.
    /// "난도"는 공격 스킬 이름이고 "기세"는 투사 본인의 다음 기술을 강화하는 자원이므로 Combatant를
    /// 키로 사용합니다. 이렇게 해야 같은 전투에 투사가 여러 명 있어도 각자의 중첩이 섞이지 않습니다.
    /// </summary>
    public sealed class BattleFighterResourceRuntime
    {
        public const int MaxMomentum = 3;
        private readonly Dictionary<Combatant, int> momentumByActor = new Dictionary<Combatant, int>();
        private readonly Dictionary<Combatant, int> directNandoUsesByActor = new Dictionary<Combatant, int>();

        public int GetMomentum(Combatant actor) => actor != null && momentumByActor.TryGetValue(actor, out int momentum) ? momentum : 0;

        /// <summary>
        /// 회오리 베기처럼 다른 기술이 적중 수만큼 기세를 줄 때 재사용할 공용 진입점입니다. Math.Min으로
        /// 최대 3을 지키되 난도 직접 사용 횟수는 건드리지 않으므로, 이 경로로 3이 되어도 난도 쿨타임은
        /// 생기지 않습니다. 반환값은 UI나 연출이 실제 증가량을 알 수 있게 합니다.
        /// </summary>
        public int AddMomentum(Combatant actor, int requestedMomentum)
        {
            if (actor == null || requestedMomentum <= 0) return 0;
            int previous = GetMomentum(actor);
            int next = Math.Min(MaxMomentum, previous + requestedMomentum);
            momentumByActor[actor] = next;
            return next - previous;
        }

        /// <summary>난도 스킬의 성공한 직접 사용만 기록하며, 세 번째인지 호출자에게 알려 줍니다.</summary>
        public bool RecordDirectNandoUse(Combatant actor)
        {
            if (actor == null) return false;
            int uses = directNandoUsesByActor.TryGetValue(actor, out int current) ? current + 1 : 1;
            bool thirdUse = uses >= 3;
            directNandoUsesByActor[actor] = thirdUse ? 0 : uses;
            return thirdUse;
        }

        /// <summary>
        /// 회심의 일격이 피해 배율을 정한 뒤 기세를 전부 소비할 때 사용합니다. 이 메서드는 난도 쿨타임이나
        /// 직접 사용 횟수를 건드리지 않으므로, 기세를 0으로 만들어도 이미 시작된 난도 쿨타임은 유지됩니다.
        /// </summary>
        public int ConsumeAllMomentum(Combatant actor)
        {
            int consumed = GetMomentum(actor);
            if (actor != null) momentumByActor[actor] = 0;
            return consumed;
        }
    }

    /// <summary>참가자 자신의 행동 차례를 기준으로 스킬별 남은 재사용 대기 턴을 관리합니다.</summary>
    public sealed class BattleSkillCooldowns
    {
        private readonly Dictionary<Combatant, Dictionary<string, int>> remainingByActor =
            new Dictionary<Combatant, Dictionary<string, int>>();

        public int GetRemaining(Combatant actor, string skillId)
        {
            if (actor == null || string.IsNullOrEmpty(skillId)) return 0;
            return remainingByActor.TryGetValue(actor, out Dictionary<string, int> skills) &&
                   skills.TryGetValue(skillId, out int remaining) ? remaining : 0;
        }

        public void Start(Combatant actor, string skillId, int turns)
        {
            if (actor == null || string.IsNullOrEmpty(skillId)) return;
            if (!remainingByActor.TryGetValue(actor, out Dictionary<string, int> skills))
            {
                skills = new Dictionary<string, int>(StringComparer.Ordinal);
                remainingByActor.Add(actor, skills);
            }
            skills[skillId] = Math.Max(0, turns);
        }

        /// <summary>해당 참가자의 새 행동 차례가 시작될 때 그 참가자의 쿨타임만 한 칸 줄입니다.</summary>
        public void BeginActorTurn(Combatant actor)
        {
            if (actor == null || !remainingByActor.TryGetValue(actor, out Dictionary<string, int> skills)) return;
            foreach (string skillId in skills.Keys.ToArray())
            {
                skills[skillId] = Math.Max(0, skills[skillId] - 1);
            }
        }
    }

    /// <summary>
    /// 정화가 제거할 수 있는 해로운 상태의 공통 이름입니다. 독·화상·감전은 저장 방식과 작동 방식이
    /// 서로 다르지만, 정화 입장에서는 모두 제거 대상이라는 한 가지 공통점이 있습니다. 향후 출혈·저주·마비를
    /// 구현할 때 이 목록과 아래 공통 조회·제거 메서드에 연결하면 정화 스킬 코드는 그대로 재사용할 수 있습니다.
    /// </summary>
    public enum HarmfulStatusType
    {
        Poison,
        Burn,
        Shock
    }

    /// <summary>
    /// HP를 줄이는 원인을 구분합니다. 대신 막기는 적이 선택한 직접 전투 행동만 보호해야 하므로
    /// 화상·독 같은 지속 피해를 같은 TakeDamage 호출이라는 이유만으로 가로채지 않습니다. 향후 출혈,
    /// 반사 피해, 환경 피해도 새 원인을 추가해 보호 여부를 명시할 수 있습니다.
    /// </summary>
    public enum BattleDamageOrigin
    {
        DirectCombatAction,
        DamageOverTime,
        Reflected,
        Environmental,
        GuardianTransfer
    }

    /// <summary>한 번의 대신 막기 계산 결과를 연출 계층에 전달하는 읽기 전용 자료입니다.</summary>
    public readonly struct GuardianInterceptionResult
    {
        public GuardianInterceptionResult(Combatant protectedAlly, Combatant guardian, int reducedDamage,
            int scheduledTransfer, int appliedGuardianDamage)
        {
            ProtectedAlly = protectedAlly;
            Guardian = guardian;
            ReducedDamage = Math.Max(0, reducedDamage);
            ScheduledTransfer = Math.Max(0, scheduledTransfer);
            AppliedGuardianDamage = Math.Max(0, appliedGuardianDamage);
        }

        public Combatant ProtectedAlly { get; }
        public Combatant Guardian { get; }
        public int ReducedDamage { get; }
        public int ScheduledTransfer { get; }
        public int AppliedGuardianDamage { get; }
    }

    /// <summary>도발처럼 전투 참가자에게 남는 상태의 적용과 무효 상태 정리를 담당합니다.</summary>
    public sealed class BattleStatusEffectRuntime
    {
        private sealed class BurnState
        {
            public int RemainingTicks;
            public int RawDamagePerTick;
        }

        private sealed class GaiaWallState
        {
            public int RemainingActions;
            public bool IgnoreCurrentActionCompletion;
        }

        private sealed class IronWallState
        {
            public int RemainingActions;
            public bool IgnoreCurrentActionCompletion;
        }

        private sealed class PoisonState
        {
            public int RemainingActions;
        }

        private sealed class GuardianCoverState
        {
            public Combatant Guardian;
            public int MaximumTransferBudget;
            public int RemainingTransferBudget;
        }

        private readonly Dictionary<Combatant, BurnState> burns = new Dictionary<Combatant, BurnState>();
        private readonly HashSet<Combatant> shockedTargets = new HashSet<Combatant>();
        private readonly Dictionary<Combatant, GaiaWallState> gaiaWalls = new Dictionary<Combatant, GaiaWallState>();
        private readonly Dictionary<Combatant, IronWallState> ironWalls = new Dictionary<Combatant, IronWallState>();
        private readonly Dictionary<Combatant, PoisonState> poisons = new Dictionary<Combatant, PoisonState>();
        private readonly Dictionary<BattleSide, GuardianCoverState> guardianCovers =
            new Dictionary<BattleSide, GuardianCoverState>();

        /// <summary>계산 결과만 화면에 알려 주어 피해 규칙이 UI 오브젝트를 직접 참조하지 않게 합니다.</summary>
        public event Action<GuardianInterceptionResult> GuardianInterceptionOccurred;

        public int ApplyTauntToAll(Combatant source, Formation opponents, int affectedActions)
        {
            if (source == null || !source.IsAlive || opponents == null) return 0;
            Combatant[] targets = opponents.LivingMembers.ToArray();
            foreach (Combatant target in targets) target.ApplyTaunt(source, affectedActions);
            return targets.Length;
        }

        /// <summary>도발 시전자가 전투불능이면 즉시 모든 강제 대상 상태를 해제합니다.</summary>
        public void RemoveInvalidTaunts(IEnumerable<Combatant> combatants)
        {
            if (combatants == null) return;
            foreach (Combatant combatant in combatants)
            {
                if (combatant.ForcedTarget != null && !combatant.ForcedTarget.IsAlive)
                    combatant.ApplyTaunt(null, 0);
            }
        }

        /// <summary>
        /// 감전은 지속 피해가 아니라 다음 행동의 "주는 피해"를 약하게 만드는 제어 디버프입니다.
        /// 대상별 Combatant를 Set에 따로 저장하므로 여러 적의 감전이 서로 섞이지 않습니다. 이미 들어 있는
        /// 대상을 다시 Add해도 복제되지 않으므로 중첩 대신 감전 1로 자연스럽게 갱신됩니다.
        /// </summary>
        public void ApplyOrRefreshShock(Combatant target)
        {
            if (target != null && target.IsAlive) shockedTargets.Add(target);
        }

        public bool HasShock(Combatant target) => target != null && target.IsAlive && shockedTargets.Contains(target);

        /// <summary>
        /// 가이아 웰은 다른 아군을 대신 막거나 적의 대상을 바꾸는 기술이 아니라 마도사 자신에게만 붙는
        /// 피해 감소 상태입니다. Combatant를 키로 저장하므로 다른 참가자의 행동에는 남은 횟수가 줄지 않습니다.
        /// </summary>
        public void ApplyGaiaWall(Combatant target, int protectedActions)
        {
            if (target == null || !target.IsAlive || protectedActions <= 0) return;
            gaiaWalls[target] = new GaiaWallState
            {
                RemainingActions = protectedActions,
                // 상태를 건 현재 행동은 "다음 2회"에 포함되지 않으므로 첫 완료 알림만 건너뜁니다.
                IgnoreCurrentActionCompletion = true
            };
        }

        public int GetGaiaWallRemaining(Combatant target) => target != null && target.IsAlive &&
            gaiaWalls.TryGetValue(target, out GaiaWallState state) ? state.RemainingActions : 0;

        /// <summary>
        /// 철벽은 수호자 자신에게만 붙는 전문 방어 상태입니다. 공용 방어보다 강한 이유는 아군의 공격을
        /// 받아내는 탱커가 위험한 순간을 버티는 직업 고유 선택지이기 때문이며, 대신 4턴 재사용 제한을 둡니다.
        /// 사용한 현재 행동은 "다음 2회 행동"에 포함하지 않아 처음부터 철벽 2를 온전히 보장합니다.
        /// </summary>
        public void ApplyIronWall(Combatant target, int protectedActions)
        {
            if (target == null || !target.IsAlive || protectedActions <= 0) return;
            ironWalls[target] = new IronWallState
            {
                RemainingActions = protectedActions,
                IgnoreCurrentActionCompletion = true
            };
        }

        public int GetIronWallRemaining(Combatant target) => target != null && target.IsAlive &&
            ironWalls.TryGetValue(target, out IronWallState state) ? state.RemainingActions : 0;

        public bool HasStrongerSelfDefense(Combatant target) =>
            GetGaiaWallRemaining(target) > 0 || GetIronWallRemaining(target) > 0;

        /// <summary>
        /// 수호자와 이전 예산만 저장합니다. 보호 대상 배열이나 세 자리 슬롯을 저장하지 않으므로 실제 피격
        /// 순간 같은 진영·생존·수호자 자신 제외 조건을 판정하며 6명 이상 파티도 같은 코드로 처리합니다.
        /// </summary>
        public bool ApplyGuardianCover(Combatant guardian)
        {
            if (guardian == null || !guardian.IsAlive) return false;
            int budget = (int)Math.Max(1L, ((long)Math.Max(1, guardian.MaxHp) * 40L + 99L) / 100L);
            guardianCovers[guardian.Side] = new GuardianCoverState
            {
                Guardian = guardian,
                MaximumTransferBudget = budget,
                RemainingTransferBudget = budget
            };
            return true;
        }

        public bool HasGuardianCover(Combatant guardian) => guardian != null && guardian.IsAlive &&
            guardianCovers.TryGetValue(guardian.Side, out GuardianCoverState state) && state.Guardian == guardian &&
            state.RemainingTransferBudget > 0;

        public int GetGuardianCoverMaximumBudget(Combatant guardian) => HasGuardianCover(guardian) &&
            guardianCovers.TryGetValue(guardian.Side, out GuardianCoverState state) ? state.MaximumTransferBudget : 0;

        public int GetGuardianCoverRemainingBudget(Combatant guardian) => HasGuardianCover(guardian) &&
            guardianCovers.TryGetValue(guardian.Side, out GuardianCoverState state) ? state.RemainingTransferBudget : 0;

        /// <summary>수호자의 다음 행동이 시작되는 경계에서 보호를 끝내 지속시간이 한 행동 늘지 않게 합니다.</summary>
        public void BeginActorAction(Combatant actor)
        {
            if (actor == null) return;
            if (guardianCovers.TryGetValue(actor.Side, out GuardianCoverState state) && state.Guardian == actor)
                guardianCovers.Remove(actor.Side);
        }

        /// <summary>
        /// 가이아 웰은 공용 방어 50%보다 강한 마도사 전용 생존기이므로 원시 피해의 40%만 받습니다.
        /// 활성 중에는 방어를 함께 적용하지 않는 확정 규칙에 따라 TakeDamage의 방어 단계를 건너뜁니다.
        /// UI 차단 외에도 계산 경계에서 중첩을 막아 외부 호출이 있어도 60% 감소만 적용되게 합니다.
        /// </summary>
        public int ApplyIncomingDamage(Combatant target, int rawDamage,
            BattleDamageOrigin origin = BattleDamageOrigin.DirectCombatAction)
        {
            if (target == null) return 0;
            int damageAfterCover = Math.Max(1, rawDamage);
            GuardianInterceptionResult? interception = null;
            if (origin == BattleDamageOrigin.DirectCombatAction &&
                guardianCovers.TryGetValue(target.Side, out GuardianCoverState cover) &&
                cover.Guardian != null && cover.Guardian.IsAlive && cover.Guardian != target &&
                cover.RemainingTransferBudget > 0)
            {
                int desiredReduction = damageAfterCover / 2;
                int desiredTransfer = (desiredReduction + 1) / 2;
                int scheduledTransfer = Math.Min(cover.RemainingTransferBudget, desiredTransfer);
                int actualReduction = Math.Min(desiredReduction, scheduledTransfer * 2);
                if (scheduledTransfer > 0 && actualReduction > 0)
                {
                    damageAfterCover -= actualReduction;
                    // 예산은 철벽 적용 뒤 HP 피해가 아니라 적용 전 이전 예정량으로 소비합니다. 철벽이 실제
                    // 피해를 줄였다는 이유로 예산이 되돌아오면 최대 HP 40%라는 보호 위험 상한이 커집니다.
                    cover.RemainingTransferBudget -= scheduledTransfer;
                    int guardianDamage = ApplyPersonalDefense(cover.Guardian, scheduledTransfer);
                    interception = new GuardianInterceptionResult(target, cover.Guardian, actualReduction,
                        scheduledTransfer, guardianDamage);
                    if (cover.RemainingTransferBudget <= 0 || !cover.Guardian.IsAlive)
                        guardianCovers.Remove(target.Side);
                }
            }

            int appliedDamage = ApplyPersonalDefense(target, damageAfterCover);
            if (interception.HasValue) GuardianInterceptionOccurred?.Invoke(interception.Value);
            return appliedDamage;
        }

        /// <summary>철벽·가이아 웰·공용 방어의 기존 우선순위를 한곳에서 유지합니다.</summary>
        private int ApplyPersonalDefense(Combatant target, int rawDamage)
        {
            if (target == null) return 0;
            if (GetIronWallRemaining(target) > 0)
            {
                // 철벽 70%와 공용 방어 50%를 곱하면 의도보다 지나치게 강해지고 UI 설명과 실제 결과도
                // 달라집니다. 따라서 원시 피해의 30%만 기존 올림·최소 1 규칙으로 계산한 뒤,
                // TakeDamage에는 공용 방어를 적용하지 말라고 알려 언제나 10→3 한 번만 감소시킵니다.
                int ironWallDamage = (int)Math.Max(1L, ((long)Math.Max(1, rawDamage) * 30L + 99L) / 100L);
                return target.TakeDamage(ironWallDamage, applyDefending: false);
            }
            if (GetGaiaWallRemaining(target) <= 0) return target.TakeDamage(rawDamage);
            int reducedDamage = (int)Math.Max(1L, ((long)Math.Max(1, rawDamage) * 40L + 99L) / 100L);
            return target.TakeDamage(reducedDamage, applyDefending: false);
        }

        /// <summary>감전된 행동자의 모든 공격 피해를 85%로 만든 뒤 기존 TakeDamage로 넘길 값입니다.</summary>
        public int ModifyOutgoingDamage(Combatant source, int rawDamage)
        {
            if (rawDamage <= 0 || !HasShock(source)) return rawDamage;
            return (int)Math.Max(1L, ((long)rawDamage * 85L + 99L) / 100L);
        }

        /// <summary>
        /// 공격·방어·회복처럼 무엇을 했는지와 관계없이 그 참가자의 행동이 끝나면 감전 1을 소비합니다.
        /// 행동 도중에는 계속 남겨 두어 여러 대상을 때리는 광역 공격도 모든 피해에 같은 15% 감소를 받습니다.
        /// </summary>
        public void CompleteActorAction(Combatant actor)
        {
            if (actor == null) return;
            shockedTargets.Remove(actor);

            if (gaiaWalls.TryGetValue(actor, out GaiaWallState gaia))
            {
                if (gaia.IgnoreCurrentActionCompletion)
                    gaia.IgnoreCurrentActionCompletion = false;
                else
                {
                    // 공격·스킬 등 무엇을 선택했든 마도사 자신의 행동 기회 하나를 마쳤을 때만 2→1→제거합니다.
                    gaia.RemainingActions = Math.Max(0, gaia.RemainingActions - 1);
                    if (gaia.RemainingActions == 0) gaiaWalls.Remove(actor);
                }
            }
            CompleteIronWallAction(actor);
        }

        private void CompleteIronWallAction(Combatant actor)
        {
            if (!ironWalls.TryGetValue(actor, out IronWallState ironWall)) return;
            if (ironWall.IgnoreCurrentActionCompletion)
            {
                ironWall.IgnoreCurrentActionCompletion = false;
                return;
            }

            // 전체 라운드가 아니라 수호자 자신의 행동 횟수를 기준으로 해야 파티·적 수가 늘어나도 실제로
            // 두 번의 선택 기회를 같은 보호 아래 사용할 수 있습니다. 다른 참가자의 행동은 이 키를 건드리지 않습니다.
            ironWall.RemainingActions = Math.Max(0, ironWall.RemainingActions - 1);
            if (ironWall.RemainingActions == 0) ironWalls.Remove(actor);
        }

        /// <summary>전투불능 참가자는 다음 행동이 없으므로 화면과 저장소 양쪽에서 상태를 즉시 제거합니다.</summary>
        public void RemoveInvalidPersistentEffects(IEnumerable<Combatant> combatants)
        {
            if (combatants == null) return;
            Combatant[] members = combatants.Where(combatant => combatant != null).ToArray();
            RemoveInvalidTaunts(members);
            foreach (Combatant dead in members.Where(combatant => !combatant.IsAlive))
            {
                burns.Remove(dead);
                shockedTargets.Remove(dead);
                gaiaWalls.Remove(dead);
                ironWalls.Remove(dead);
                poisons.Remove(dead);
                if (guardianCovers.TryGetValue(dead.Side, out GuardianCoverState cover) && cover.Guardian == dead)
                    guardianCovers.Remove(dead.Side);
            }
        }

        /// <summary>
        /// 화상은 같은 대상에게 여러 묶음을 쌓지 않습니다. 이미 화상 중이라도 새 파이어 볼이 맞으면
        /// 남은 횟수를 2로 되돌리고, 새 명중 당시 마도사의 공격력으로 계산한 피해를 덮어씁니다.
        /// 이렇게 저장해야 이후 마도사의 능력치가 바뀌어도 이미 붙은 화상 피해가 소급 변경되지 않습니다.
        /// </summary>
        public void ApplyOrRefreshBurn(Combatant target, int rawDamagePerTick, int ticks)
        {
            if (target == null || !target.IsAlive || rawDamagePerTick <= 0 || ticks <= 0) return;
            burns[target] = new BurnState
            {
                RemainingTicks = ticks,
                RawDamagePerTick = rawDamagePerTick
            };
        }

        public int GetBurnRemaining(Combatant target) => target != null && burns.TryGetValue(target, out BurnState burn)
            ? burn.RemainingTicks : 0;

        public bool HasActiveBurn(Combatant target) => target != null && target.IsAlive && GetBurnRemaining(target) > 0;

        /// <summary>
        /// 대상 행동이 끝난 정확한 시점에 한 번만 호출합니다. 저장된 원시 피해를 기존 TakeDamage로 전달해
        /// HP 감소 규칙을 재사용하고, 적용 뒤 남은 횟수를 줄여 0이면 상태를 제거합니다.
        /// </summary>
        public int ApplyBurnTickAtActionEnd(Combatant target, out int remainingTicks)
        {
            remainingTicks = 0;
            if (!HasActiveBurn(target) || !burns.TryGetValue(target, out BurnState burn)) return 0;
            // 화상은 직접 공격이 끝난 뒤 상태가 만드는 DoT이므로 대신 막기 보호 계산을 명시적으로 건너뜁니다.
            int damage = ApplyIncomingDamage(target, burn.RawDamagePerTick, BattleDamageOrigin.DamageOverTime);
            burn.RemainingTicks = Math.Max(0, burn.RemainingTicks - 1);
            remainingTicks = burn.RemainingTicks;
            if (burn.RemainingTicks == 0 || !target.IsAlive) burns.Remove(target);
            return damage;
        }

        /// <summary>
        /// 독은 여러 묶음을 더하지 않습니다. 이미 독 1/2/3인 대상도 새 독침벌 공격에 맞으면
        /// Dictionary의 같은 대상 값을 3으로 덮어써 독 4 이상으로 올라가지 않게 합니다.
        /// 향후 정화는 이 저장소에서 해당 대상의 PoisonState만 제거하면 됩니다.
        /// </summary>
        public void ApplyOrRefreshPoison(Combatant target, int affectedActions)
        {
            if (target == null || !target.IsAlive || affectedActions <= 0) return;
            poisons[target] = new PoisonState { RemainingActions = affectedActions };
        }

        public int GetPoisonRemaining(Combatant target) => target != null && target.IsAlive &&
            poisons.TryGetValue(target, out PoisonState poison) ? poison.RemainingActions : 0;

        public bool HasActivePoison(Combatant target) => GetPoisonRemaining(target) > 0;

        /// <summary>
        /// 대상에게 현재 걸린 해로운 상태만 공통 목록으로 돌려줍니다. 기존 Dictionary와 HashSet을 하나의
        /// 거대한 새 저장소로 옮기지 않는 이유는, 이미 검증된 독 틱·화상 저장 피해·감전 행동 종료 규칙을
        /// 그대로 보호하기 위해서입니다. 이 메서드는 저장 방식을 바꾸지 않고 정화가 읽을 공통 창구만 제공합니다.
        /// </summary>
        public IReadOnlyList<HarmfulStatusType> GetHarmfulStatuses(Combatant target)
        {
            if (target == null || !target.IsAlive) return Array.Empty<HarmfulStatusType>();
            List<HarmfulStatusType> results = new List<HarmfulStatusType>(3);
            if (HasActivePoison(target)) results.Add(HarmfulStatusType.Poison);
            if (HasActiveBurn(target)) results.Add(HarmfulStatusType.Burn);
            if (HasShock(target)) results.Add(HarmfulStatusType.Shock);
            return results;
        }

        /// <summary>
        /// 지정한 해로운 상태 하나만 기존 저장소에서 제거합니다. 방어·가이아 웰·도발·기세·쿨타임은
        /// 전투 전략을 이루는 이로운 상태나 별도 자원이므로 이 분류에 들어오지 않으며 정화로 지워지지 않습니다.
        /// </summary>
        public bool RemoveHarmfulStatus(Combatant target, HarmfulStatusType statusType)
        {
            if (target == null) return false;
            switch (statusType)
            {
                case HarmfulStatusType.Poison:
                    return poisons.Remove(target);
                case HarmfulStatusType.Burn:
                    return burns.Remove(target);
                case HarmfulStatusType.Shock:
                    return shockedTargets.Remove(target);
                default:
                    return false;
            }
        }

        /// <summary>현재 등록된 해로운 상태를 모두 제거하고 실제로 없앤 종류 수를 반환합니다.</summary>
        public int RemoveAllHarmfulStatuses(Combatant target)
        {
            HarmfulStatusType[] active = GetHarmfulStatuses(target).ToArray();
            int removed = 0;
            foreach (HarmfulStatusType statusType in active)
                if (RemoveHarmfulStatus(target, statusType)) removed++;
            return removed;
        }

        /// <summary>
        /// 독 피해는 대상의 현재 HP가 아니라 최대 HP의 5%를 올림 계산합니다. long으로 먼저 곱해 큰 HP에서도
        /// 정수 범위를 넘는 중간 계산을 피하고 최소 1을 보장합니다. 상태 고유 피해이므로 방어·가이아 웰을
        /// 거치지 않고 TakeDamage가 최종 HP를 0 아래로 내리지 않도록 맡깁니다.
        /// </summary>
        public int ApplyPoisonTickAtActionEnd(Combatant target, out int remainingActions)
        {
            remainingActions = 0;
            if (!HasActivePoison(target) || !poisons.TryGetValue(target, out PoisonState poison)) return 0;
            int rawDamage = (int)Math.Max(1L, ((long)Math.Max(1, target.MaxHp) * 5L + 99L) / 100L);
            int damage = target.TakeDamage(rawDamage, applyDefending: false);
            poison.RemainingActions = Math.Max(0, poison.RemainingActions - 1);
            remainingActions = poison.RemainingActions;
            if (poison.RemainingActions == 0 || !target.IsAlive) poisons.Remove(target);
            return damage;
        }
    }

    /// <summary>스킬 사용 가능 판정과 실제 효과 실행을 UI 코드 밖에서 처리합니다.</summary>
    public sealed class BattleSkillExecutor
    {
        private readonly BattleSkillCooldowns cooldowns;
        private readonly BattleStatusEffectRuntime statusEffects;
        private readonly BattleFighterResourceRuntime fighterResources;

        public BattleSkillExecutor(BattleSkillCooldowns cooldowns, BattleStatusEffectRuntime statusEffects,
            BattleFighterResourceRuntime fighterResources)
        {
            this.cooldowns = cooldowns ?? throw new ArgumentNullException(nameof(cooldowns));
            this.statusEffects = statusEffects ?? throw new ArgumentNullException(nameof(statusEffects));
            this.fighterResources = fighterResources ?? throw new ArgumentNullException(nameof(fighterResources));
        }

        public bool CanUse(Combatant actor, BattleSkillDefinition skill, out string reason)
        {
            if (actor == null || !actor.IsAlive)
            {
                reason = "행동할 수 없는 상태입니다.";
                return false;
            }
            if (skill == null || !skill.IsImplemented)
            {
                reason = "아직 사용할 수 없습니다.";
                return false;
            }

            int remaining = cooldowns.GetRemaining(actor, skill.Id);
            if (remaining > 0)
            {
                reason = $"{skill.DisplayName}은(는) {remaining}턴 뒤 다시 사용할 수 있습니다.";
                return false;
            }
            if (skill.EffectType == BattleSkillEffectType.SingleMeleePhysicalAttackWithMomentumGain &&
                fighterResources.GetMomentum(actor) >= BattleFighterResourceRuntime.MaxMomentum)
            {
                reason = "기세가 이미 최대입니다.";
                return false;
            }

            reason = string.Empty;
            return true;
        }

        /// <summary>
        /// 피해뿐 아니라 모든 피격 연출까지 끝나 행동 성공이 확정된 스킬의 쿨타임을 등록합니다.
        /// 화살비는 유효 후열이 없으면 연출 자체를 시작하지 않으므로 이 메서드도 호출되지 않습니다.
        /// 스킬 버튼을 누른 즉시 시작하면 거절된 행동에도 쿨타임이 생길 수 있어 완료 경계를 분리합니다.
        ///
        /// BattleSkillCooldowns는 Combatant와 Skill ID를 함께 키로 사용합니다. 따라서 같은 사수의
        /// 정조준과 화살비도 서로 다른 남은 턴을 저장하고, 다른 캐릭터 행동에는 BeginActorTurn이 그
        /// 사수를 받지 않으므로 화살비 쿨타임도 감소하지 않습니다.
        /// </summary>
        public void RegisterCooldownAfterSuccessfulUse(Combatant actor, BattleSkillDefinition skill)
        {
            if (actor == null || skill == null || skill.CooldownTurns <= 0) return;
            cooldowns.Start(actor, skill.Id, skill.CooldownTurns);
        }

        private int CalculateOutgoingAttackDamage(Combatant actor, int damagePercent)
        {
            long scaledDamage = (long)actor.Attack * damagePercent;
            int rawDamage = (int)Math.Max(1L, (scaledDamage + 99L) / 100L);
            return statusEffects.ModifyOutgoingDamage(actor, rawDamage);
        }

        private int ApplyDamage(Combatant target, int rawDamage)
        {
            return statusEffects.ApplyIncomingDamage(target, rawDamage);
        }

        public bool Execute(Combatant actor, BattleSkillDefinition skill, Formation opponents, out string message)
        {
            if (!CanUse(actor, skill, out message)) return false;

            switch (skill.EffectType)
            {
                case BattleSkillEffectType.Taunt:
                    int affected = statusEffects.ApplyTauntToAll(actor, opponents, skill.EffectDuration);
                    if (affected <= 0)
                    {
                        message = "도발을 적용할 대상이 없습니다.";
                        return false;
                    }
                    cooldowns.Start(actor, skill.Id, skill.CooldownTurns);
                    message = $"{actor.DisplayName}의 도발! 적 {affected}명은 각자 다음 {skill.EffectDuration}회 행동 동안 단일 적대 행동의 대상을 수호자로 지정합니다.";
                    return true;
                default:
                    message = "아직 사용할 수 없습니다.";
                    return false;
            }
        }

        /// <summary>
        /// 단일 아군 회복을 실행합니다. 공격용 TargetResolver는 적대 사거리와 도발을 판단하므로 사용하지 않고,
        /// 같은 진영·생존 여부를 직접 확인합니다. 최대 HP 대상은 선택까지 허용하지만 여기서 거절하여 행동과
        /// 쿨타임을 소비하지 않습니다.
        /// </summary>
        public bool ExecuteSingleAllyHeal(Combatant actor, Combatant target, BattleSkillDefinition skill,
            out int recoveredHp, out string message)
        {
            recoveredHp = 0;
            if (!CanUse(actor, skill, out message)) return false;
            if (skill.EffectType != BattleSkillEffectType.SingleAllyHeal || target == null || target.Side != actor.Side)
            {
                message = "살아 있는 아군만 치유할 수 있습니다.";
                return false;
            }
            if (!target.IsAlive)
            {
                message = "전투불능 아군은 치유의 빛으로 부활시킬 수 없습니다.";
                return false;
            }
            if (target.CurrentHp >= target.MaxHp)
            {
                message = $"{target.DisplayName}은(는) 이미 HP가 가득 찼습니다.";
                return false;
            }

            // Ceiling은 소수점이 생겼을 때 항상 올림합니다. 예를 들어 최대 HP 101의 35%인 35.35는
            // 36으로 안정적으로 정수화하며, Combatant.RecoverHp가 남은 빈 HP보다 많이 채워지지 않게 막습니다.
            int requestedHp = Math.Max(1, (int)Math.Ceiling(target.MaxHp * skill.MaxHpHealRatio));
            recoveredHp = target.RecoverHp(requestedHp);
            if (recoveredHp <= 0)
            {
                message = "회복할 HP가 없습니다.";
                return false;
            }

            if (skill.CooldownTurns > 0) cooldowns.Start(actor, skill.Id, skill.CooldownTurns);
            message = $"{actor.DisplayName}의 {skill.DisplayName}! {target.DisplayName}의 HP가 {recoveredHp} 회복되었습니다.";
            return true;
        }

        /// <summary>
        /// 살아 있는 같은 편 한 명의 해로운 상태를 모두 제거합니다. 상태가 없을 때 행동을 소비하지 않는 것은
        /// 아무 변화도 만들지 못한 실수 입력에 턴과 쿨타임이라는 비용을 부과하지 않기 위함입니다. 실제 쿨타임은
        /// VFX까지 정상 완료된 뒤 Controller가 등록하므로, 이 메서드는 상태 제거 성공 여부만 반환합니다.
        /// </summary>
        public bool ExecuteSingleAllyCleanse(Combatant actor, Combatant target, BattleSkillDefinition skill,
            out int removedCount, out string message)
        {
            removedCount = 0;
            if (!CanUse(actor, skill, out message)) return false;
            if (skill.EffectType != BattleSkillEffectType.RemoveAllHarmfulStatuses || target == null ||
                target.Side != actor.Side || !target.IsAlive)
            {
                message = "살아 있는 아군만 정화할 수 있습니다.";
                return false;
            }
            if (statusEffects.GetHarmfulStatuses(target).Count == 0)
            {
                message = "정화할 해로운 상태가 없습니다.";
                return false;
            }

            removedCount = statusEffects.RemoveAllHarmfulStatuses(target);
            if (removedCount <= 0)
            {
                message = "정화할 해로운 상태가 없습니다.";
                return false;
            }
            message = $"{actor.DisplayName}의 {skill.DisplayName}! {target.DisplayName}의 해로운 상태이상 {removedCount}개를 제거했습니다.";
            return true;
        }

        /// <summary>
        /// Formation에서 확정한 살아 있는 같은 편 전체를 한 번에 회복합니다. 화면에 보이는 인원 수를
        /// 전제로 하지 않고 전달받은 목록을 순회하므로 현재 3명뿐 아니라 6명 이상으로 확장해도 같은
        /// 계산을 사용합니다. HP가 가득 찬 아군은 0 회복으로 남기고, 적어도 한 명의 HP가 실제로
        /// 증가할 때만 성공으로 처리하여 실패한 사용이 행동이나 쿨타임을 소비하지 않게 합니다.
        /// </summary>
        public bool ExecuteAreaAllyHeal(Combatant actor, IReadOnlyList<Combatant> targets,
            BattleSkillDefinition skill, out IReadOnlyList<int> recoveredAmounts, out string message)
        {
            recoveredAmounts = Array.Empty<int>();
            if (!CanUse(actor, skill, out message)) return false;
            if (skill.EffectType != BattleSkillEffectType.AreaAllyHeal || targets == null)
            {
                message = "회복할 수 있는 살아 있는 아군이 없습니다.";
                return false;
            }

            Combatant[] livingAllies = targets
                .Where(target => target != null && target.IsAlive && target.Side == actor.Side)
                .ToArray();
            if (!livingAllies.Any(target => target.CurrentHp < target.MaxHp))
            {
                message = "HP를 회복할 수 있는 살아 있는 아군이 없습니다.";
                return false;
            }

            int[] results = new int[livingAllies.Length];
            for (int index = 0; index < livingAllies.Length; index++)
            {
                Combatant target = livingAllies[index];
                // 치유의 빛과 같은 올림 정책입니다. MaxHpHealRatio 자체가 35% × 60%에서 파생되므로
                // 최대 HP 101이라면 21.21을 올린 22를 요청하고 RecoverHp가 최대 HP를 넘지 않게 막습니다.
                int requestedHp = Math.Max(1, (int)Math.Ceiling(target.MaxHp * skill.MaxHpHealRatio));
                results[index] = target.RecoverHp(requestedHp);
            }

            recoveredAmounts = results;
            int healedCount = results.Count(amount => amount > 0);
            int totalRecovered = results.Sum();
            message = $"{actor.DisplayName}의 {skill.DisplayName}! 아군 {healedCount}명의 HP가 총 {totalRecovered} 회복되었습니다.";
            return healedCount > 0;
        }

        /// <summary>
        /// Projectile이 대상에 도착한 순간 호출하는 정조준 피해 처리입니다. 대상 후보는 앞 단계에서
        /// TargetResolver가 정하지만, 도착 시점에도 적·생존 여부를 다시 확인해 무효 대상에 피해를 주지 않습니다.
        /// UI나 Projectile은 피해 공식을 모르고 이 메서드의 결과만 표시하므로 계산과 연출이 분리됩니다.
        /// </summary>
        public bool ExecuteSingleRangedPhysicalAttack(Combatant actor, Combatant target, BattleSkillDefinition skill,
            out int damage, out string message)
        {
            damage = 0;
            if (!CanUse(actor, skill, out message)) return false;
            if (skill.EffectType != BattleSkillEffectType.SingleRangedPhysicalAttack || target == null ||
                target.Side == actor.Side || !target.IsAlive)
            {
                message = "공격할 수 있는 살아 있는 적이 아닙니다.";
                return false;
            }

            // (공격력×160 + 99) / 100은 정수만으로 160%를 계산하면서 나머지가 있으면 올림하는 식입니다.
            // 예: 12×160=1920 → (1920+99)/100=20, 15×160=2400 → 24입니다. float 오차로 24가 25가 되는
            // 일을 피하며, 이후 TakeDamage가 기존 방어 50%를 그대로 적용해 방어 무시 효과도 생기지 않습니다.
            int rawDamage = CalculateOutgoingAttackDamage(actor, skill.AttackDamagePercent);
            damage = ApplyDamage(target, rawDamage);
            cooldowns.Start(actor, skill.Id, skill.CooldownTurns);
            message = $"{actor.DisplayName}의 {skill.DisplayName}! {target.DisplayName}에게 {damage} 피해.";
            return true;
        }

        /// <summary>
        /// Wolf가 대상에게 접촉한 프레임에만 호출되는 동료의 습격 피해 처리입니다. Wolf는 사수의 명령을
        /// 화면으로 보여 주는 BeastCompanion이지 Combatant가 아니므로 HP·턴·Formation을 만들지 않습니다.
        /// 피해의 주체와 쿨타임 기준은 사수이며, TakeDamage를 거쳐 기존 방어의 50% 감소도 그대로 유지합니다.
        /// </summary>
        public bool ExecuteSingleBeastPhysicalAttack(Combatant actor, Combatant target, BattleSkillDefinition skill,
            out int damage, out string message)
        {
            damage = 0;
            if (!CanUse(actor, skill, out message)) return false;
            if (skill.EffectType != BattleSkillEffectType.SingleBeastPhysicalAttack || target == null ||
                target.Side == actor.Side || !target.IsAlive)
            {
                message = "공격할 수 있는 살아 있는 적이 아닙니다.";
                return false;
            }

            // 일반 공격력의 180%를 소수점 없이 올림합니다. 실제 감소는 기존 Combatant.TakeDamage가 담당합니다.
            int rawDamage = CalculateOutgoingAttackDamage(actor, skill.AttackDamagePercent);
            damage = ApplyDamage(target, rawDamage);

            // 이 값은 다른 참가자의 행동에는 줄지 않고, 사수 자신의 행동 시작에만 3→2→1→0으로 감소합니다.
            cooldowns.Start(actor, skill.Id, skill.CooldownTurns);
            message = $"{actor.DisplayName}의 {skill.DisplayName}! {target.DisplayName}에게 {damage} 피해.";
            return true;
        }

        /// <summary>
        /// Warm Explosion이 가장 커지는 index 4에서 호출되는 파이어 볼 계산입니다. 즉발 170%와 화상 30%는
        /// 모두 현재 마도사의 Attack으로 정수 올림 계산하지만, 화상 쪽은 계산 결과 자체를 상태 저장소에 넣습니다.
        /// 재적중하면 상태 저장소가 기존 묶음을 더하지 않고 새 피해·2회로 교체합니다.
        /// </summary>
        public bool ExecuteSingleMagicAttackWithBurn(Combatant actor, Combatant target, BattleSkillDefinition skill,
            out int damage, out string message)
        {
            damage = 0;
            if (!CanUse(actor, skill, out message)) return false;
            if (skill.EffectType != BattleSkillEffectType.SingleMagicAttackWithBurn || target == null ||
                target.Side == actor.Side || !target.IsAlive)
            {
                message = "공격할 수 있는 살아 있는 적이 아닙니다.";
                return false;
            }

            int rawDamage = CalculateOutgoingAttackDamage(actor, skill.AttackDamagePercent);
            damage = ApplyDamage(target, rawDamage);

            // 즉발 피해로 쓰러진 대상에게는 이후 행동도 없으므로 화상을 남기지 않습니다.
            if (target.IsAlive)
            {
                int storedBurnDamage = CalculateOutgoingAttackDamage(actor, skill.BurnDamagePercent);
                statusEffects.ApplyOrRefreshBurn(target, storedBurnDamage, skill.EffectDuration);
            }

            cooldowns.Start(actor, skill.Id, skill.CooldownTurns);
            message = $"{actor.DisplayName}의 {skill.DisplayName}! {target.DisplayName}에게 {damage} 피해와 화상 {skill.EffectDuration}회.";
            return true;
        }

        /// <summary>
        /// 난도의 칼이 실제로 닿는 순간 피해와 자원 획득을 함께 확정합니다. 150%는 고정 피해 15가 아니라
        /// 현재 투사의 Attack에 곱하므로 성장한 일반 공격 피해를 그대로 따라갑니다. `(Attack×150+99)/100`은
        /// 소수점이 생기면 올림하는 정수 계산이며, 마지막 TakeDamage가 기존 방어 50%를 그대로 적용합니다.
        /// </summary>
        public bool ExecuteSingleMeleePhysicalAttackWithMomentumGain(Combatant actor, Combatant target,
            BattleSkillDefinition skill, out int damage, out string message)
        {
            damage = 0;
            if (!CanUse(actor, skill, out message)) return false;
            if (skill.EffectType != BattleSkillEffectType.SingleMeleePhysicalAttackWithMomentumGain || target == null ||
                target.Side == actor.Side || !target.IsAlive)
            {
                message = "공격할 수 있는 살아 있는 적이 아닙니다.";
                return false;
            }

            int rawDamage = CalculateOutgoingAttackDamage(actor, skill.AttackDamagePercent);
            damage = ApplyDamage(target, rawDamage);

            // 피해가 적용된 뒤에만 기세를 올립니다. AddMomentum은 회오리 베기도 재사용할 수 있지만,
            // 직접 사용 기록은 이 난도 스킬 경로에서만 남겨 두 효과가 같은 기세 3을 만들더라도 구분됩니다.
            fighterResources.AddMomentum(actor, 1);
            bool thirdDirectUse = fighterResources.RecordDirectNandoUse(actor);
            int currentMomentum = fighterResources.GetMomentum(actor);
            // 쿨타임 조건은 현재 기세가 아니라 난도 스킬의 세 번째 직접 사용입니다. 회심의 일격으로 기세를
            // 먼저 소비했거나 회오리 베기로 기세 3이 되어도 이 직접 사용 기록과 쿨타임 규칙은 흔들리지 않습니다.
            if (thirdDirectUse) cooldowns.Start(actor, skill.Id, skill.CooldownTurns);

            message = $"{actor.DisplayName}의 {skill.DisplayName}! {target.DisplayName}에게 {damage} 피해. 현재 기세 {currentMomentum}.";
            return true;
        }

        /// <summary>
        /// 회심의 일격은 타격 순간의 기세를 먼저 읽어 0/1/2/3을 100/130/160/190%로 변환합니다.
        /// 피해 계산 전에 기세를 없애면 항상 100%가 되므로, 배율과 피해를 확정한 뒤 성공한 타격에서만
        /// ConsumeAllMomentum을 호출합니다. 기세 저장소는 쿨타임 저장소와 달라 난도의 남은 턴은 변하지 않습니다.
        /// </summary>
        public bool ExecuteSingleMeleePhysicalAttackConsumingMomentum(Combatant actor, Combatant target,
            BattleSkillDefinition skill, out int damage, out int consumedMomentum, out string message)
        {
            damage = 0;
            consumedMomentum = 0;
            if (!CanUse(actor, skill, out message)) return false;
            if (skill.EffectType != BattleSkillEffectType.SingleMeleePhysicalAttackConsumingMomentum || target == null ||
                target.Side == actor.Side || !target.IsAlive ||
                skill.MomentumDamagePercents.Count != BattleFighterResourceRuntime.MaxMomentum + 1)
            {
                message = "공격할 수 있는 살아 있는 적이 아닙니다.";
                return false;
            }

            int momentum = Math.Min(BattleFighterResourceRuntime.MaxMomentum, fighterResources.GetMomentum(actor));
            int damagePercent = skill.MomentumDamagePercents[momentum];
            int rawDamage = CalculateOutgoingAttackDamage(actor, damagePercent);
            damage = ApplyDamage(target, rawDamage);
            consumedMomentum = fighterResources.ConsumeAllMomentum(actor);
            message = $"{actor.DisplayName}의 {skill.DisplayName}! 기세 {momentum}으로 {target.DisplayName}에게 {damage} 피해.";
            return true;
        }

        /// <summary>
        /// 회오리 베기의 타격 순간 범위 해석기가 넘긴 살아 있는 전열 적 모두에게 같은 80% 물리 피해를 적용합니다. 정수식
        /// `(Attack×80+99)/100`은 소수점을 올림하므로 Attack 12라면 9.6이 10이 되고, 각 대상의
        /// TakeDamage가 방어 중 50% 감소를 기존 규칙 그대로 처리합니다.
        ///
        /// 기세는 실제 피해 처리를 통과한 전열 대상 수만큼 한 번에 AddMomentum으로 올립니다. 후열은 대상
        /// 배열에 들어오지 않으므로 피해뿐 아니라 기세 계산에도 포함되지 않습니다. 이 공용 자원
        /// 진입점은 최대 3을 보장하지만 RecordDirectNandoUse를 부르지 않습니다. 그래서 회오리 베기로
        /// 기세가 3이 되어도 "난도를 직접 세 번 사용"한 기록이나 난도 쿨타임은 생기지 않습니다.
        /// 향후 다른 다중 타격 기술도 같은 AddMomentum 경로를 재사용할 수 있습니다.
        /// </summary>
        public bool ExecuteAreaMeleePhysicalAttackWithMomentumGain(Combatant actor,
            IReadOnlyList<Combatant> targets, BattleSkillDefinition skill, out IReadOnlyList<int> damages,
            out int gainedMomentum, out string message)
        {
            damages = Array.Empty<int>();
            gainedMomentum = 0;
            if (!CanUse(actor, skill, out message)) return false;
            if (skill.EffectType != BattleSkillEffectType.AreaMeleePhysicalAttackWithMomentumGain || targets == null)
            {
                message = "공격할 수 있는 살아 있는 적이 없습니다.";
                return false;
            }

            Combatant[] livingEnemies = targets.Where(target => target != null && target.IsAlive && target.Side != actor.Side).ToArray();
            if (livingEnemies.Length == 0)
            {
                message = "공격할 수 있는 살아 있는 적이 없습니다.";
                return false;
            }

            int rawDamage = CalculateOutgoingAttackDamage(actor, skill.AttackDamagePercent);
            int[] appliedDamages = new int[livingEnemies.Length];
            for (int index = 0; index < livingEnemies.Length; index++)
                appliedDamages[index] = ApplyDamage(livingEnemies[index], rawDamage);

            // 자원은 적에게 붙는 상태가 아니라 공격한 투사 Combatant를 키로 저장합니다. 같은 편에 투사가
            // 여러 명 있어도 서로의 기세가 섞이지 않고, 회심의 일격도 자기 기세만 읽고 소비할 수 있습니다.
            gainedMomentum = fighterResources.AddMomentum(actor, livingEnemies.Length);
            damages = appliedDamages;
            message = $"{actor.DisplayName}의 {skill.DisplayName}! 적 {livingEnemies.Length}명에게 피해. 현재 기세 {fighterResources.GetMomentum(actor)}.";
            return true;
        }

        /// <summary>
        /// 화살비의 마지막 낙하 시점에 후열 대상마다 120% 피해를 한 번만 적용합니다. 화면에는 대상당
        /// 여러 화살이 보이지만 그것은 밀도를 만드는 연출입니다. Projectile 하나마다 TakeDamage를 호출하면
        /// 화살 수가 2개인 대상과 4개인 대상의 실제 피해가 달라지므로, 스킬 데이터가 약속한 "대상당 120%"
        /// 규칙을 지킬 수 없습니다. 따라서 Controller가 확정한 후열 대상 목록을 한 번 순회합니다.
        /// </summary>
        public bool ExecuteAreaRangedPhysicalAttack(Combatant actor, IReadOnlyList<Combatant> targets,
            BattleSkillDefinition skill, out IReadOnlyList<int> damages, out string message)
        {
            damages = Array.Empty<int>();
            if (!CanUse(actor, skill, out message)) return false;
            if (skill.EffectType != BattleSkillEffectType.AreaRangedPhysicalAttack || targets == null)
            {
                message = "화살비로 공격할 후열 적이 없습니다.";
                return false;
            }

            Combatant[] livingEnemies = targets.Where(target => target != null && target.IsAlive && target.Side != actor.Side).ToArray();
            if (livingEnemies.Length == 0)
            {
                message = "화살비로 공격할 후열 적이 없습니다.";
                return false;
            }

            // 다른 퍼센트 공격과 같은 올림 정책입니다. 예를 들어 Attack 12는
            // (12×120+99)/100 = 15가 되며, TakeDamage가 방어 중 50% 감소를 그대로 담당합니다.
            int rawDamage = CalculateOutgoingAttackDamage(actor, skill.AttackDamagePercent);
            int[] appliedDamages = new int[livingEnemies.Length];
            for (int index = 0; index < livingEnemies.Length; index++)
                appliedDamages[index] = ApplyDamage(livingEnemies[index], rawDamage);

            damages = appliedDamages;
            message = $"{actor.DisplayName}의 {skill.DisplayName}! 적 후열 {livingEnemies.Length}명에게 피해.";
            return true;
        }

        /// <summary>
        /// 썬더볼트의 Peak 한 번에 살아 있는 전열·후열 적 모두를 처리합니다. 대상당 90%는 적 전체를
        /// 동시에 맞히는 범위 이득을 고려한 수치이며, 각 대상의 TakeDamage가 방어 감소를 그대로 담당합니다.
        /// 광역 목록은 TargetResolver의 단일 강제 대상 경로를 통과하지 않으므로 도발도 적용되지 않습니다.
        /// </summary>
        public bool ExecuteAreaMagicAttackWithShock(Combatant actor, IReadOnlyList<Combatant> targets,
            BattleSkillDefinition skill, out IReadOnlyList<int> damages, out string message)
        {
            damages = Array.Empty<int>();
            if (!CanUse(actor, skill, out message)) return false;
            if (skill.EffectType != BattleSkillEffectType.AreaMagicAttackWithShock || targets == null)
            {
                message = "썬더볼트로 공격할 적이 없습니다.";
                return false;
            }

            Combatant[] livingEnemies = targets.Where(target => target != null && target.IsAlive && target.Side != actor.Side).ToArray();
            if (livingEnemies.Length == 0)
            {
                message = "썬더볼트로 공격할 살아 있는 적이 없습니다.";
                return false;
            }

            int rawDamage = CalculateOutgoingAttackDamage(actor, skill.AttackDamagePercent);
            int[] appliedDamages = new int[livingEnemies.Length];
            for (int index = 0; index < livingEnemies.Length; index++)
            {
                Combatant target = livingEnemies[index];
                appliedDamages[index] = ApplyDamage(target, rawDamage);
                // 쓰러진 적은 다음 행동이 없으므로 감전을 남기지 않습니다. 살아남은 대상별 Set 항목만
                // 갱신하여 여러 번 맞아도 감전 2가 되지 않고 각자 감전 1을 유지합니다.
                if (target.IsAlive) statusEffects.ApplyOrRefreshShock(target);
            }

            damages = appliedDamages;
            message = $"{actor.DisplayName}의 {skill.DisplayName}! 적 {livingEnemies.Length}명에게 피해와 감전 1.";
            return true;
        }

        /// <summary>
        /// VFX의 보호막이 완성되는 시점에 자기 자신에게만 상태를 적용합니다. Presenter는 그림만 재생하고
        /// 실제 2회 지속·60% 계산은 상태 저장소가 담당하므로, 연출 속도를 바꿔도 전투 규칙은 변하지 않습니다.
        /// </summary>
        public bool ExecuteSelfDamageReduction(Combatant actor, BattleSkillDefinition skill, out string message)
        {
            if (!CanUse(actor, skill, out message)) return false;
            if (skill.EffectType != BattleSkillEffectType.SelfDamageReduction)
            {
                message = "자신에게 보호 효과를 적용할 수 없습니다.";
                return false;
            }

            statusEffects.ApplyGaiaWall(actor, skill.EffectDuration);
            message = $"{actor.DisplayName}의 {skill.DisplayName}! 다음 {skill.EffectDuration}회 행동 동안 받는 피해 60% 감소.";
            return true;
        }

        /// <summary>Earth Rupture의 방벽이 솟는 시점에 철벽 상태만 적용합니다. 도발은 별도 저장소이므로 유지됩니다.</summary>
        public bool ExecuteGuardianIronWall(Combatant actor, BattleSkillDefinition skill, out string message)
        {
            if (!CanUse(actor, skill, out message)) return false;
            if (skill.EffectType != BattleSkillEffectType.GuardianIronWall)
            {
                message = "철벽을 자신에게 적용할 수 없습니다.";
                return false;
            }

            statusEffects.ApplyIronWall(actor, skill.EffectDuration);
            message = $"{actor.DisplayName}의 {skill.DisplayName}! 다음 {skill.EffectDuration}회 행동 동안 받는 피해 70% 감소.";
            return true;
        }

        /// <summary>대상 목록을 고정하지 않고 수호자 쪽에 광역 보호 상태와 최대 HP 40% 예산을 시작합니다.</summary>
        public bool ExecuteGuardianCover(Combatant actor, BattleSkillDefinition skill, out string message)
        {
            if (!CanUse(actor, skill, out message)) return false;
            if (skill.EffectType != BattleSkillEffectType.GuardianCoverAllies || !statusEffects.ApplyGuardianCover(actor))
            {
                message = "대신 막기를 적용할 수 없습니다.";
                return false;
            }

            int budget = statusEffects.GetGuardianCoverMaximumBudget(actor);
            message = $"{actor.DisplayName}의 {skill.DisplayName}! 다음 행동 전까지 동료들을 보호합니다. 이전 예산 {budget}.";
            return true;
        }
    }
}

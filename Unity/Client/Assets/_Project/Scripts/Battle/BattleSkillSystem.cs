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
        SingleRangedPhysicalAttack,
        SingleMeleePhysicalAttackWithMomentumGain,
        SingleMeleePhysicalAttackConsumingMomentum,
        AreaMeleePhysicalAttackWithMomentumGain,
        AreaRangedPhysicalAttack
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
            IReadOnlyList<int> momentumDamagePercents = null, BattleSkillTargetRange targetRange = BattleSkillTargetRange.None)
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
    }

    /// <summary>
    /// 직업의 시작 스킬 프리뷰를 실제 전투 메뉴용 데이터로 변환합니다.
    /// 아직 구현되지 않은 스킬도 목록에는 남기되 실행 가능 상태로 만들지 않습니다.
    /// </summary>
    public static class BattleSkillCatalog
    {
        public const string GuardianTauntId = "guardian_taunt";
        public const string HealerHealingLightId = "healer_healing_light";
        public const string SharpshooterAimId = "sharpshooter_aim";
        public const string SharpshooterArrowRainId = "sharpshooter_arrow_rain";
        public const string FighterNandoId = "fighter_slash_stack";
        public const string FighterCriticalStrikeId = "fighter_finishing_strike";
        public const string FighterWhirlwindId = "fighter_whirlwind";

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
                if (preview.SkillId == HealerHealingLightId)
                    // 1차 밸런스 값 35%는 화면 코드가 아니라 스킬 정의에 둡니다. 나중에 수치를 조정해도
                    // 대상 선택이나 VFX 코드를 다시 고칠 필요가 없습니다. 별도 쿨타임은 현재 기획에 없어 0입니다.
                    return new BattleSkillDefinition(preview.SkillId, preview.SkillName,
                        "살아 있는 아군 1명의 HP를 대상 최대 HP의 35%만큼 회복합니다.\n전투불능 상태의 아군은 대상으로 선택할 수 없습니다.", true,
                        BattleSkillEffectType.SingleAllyHeal, 0, 0, .35f,
                        iconId: BattleUiIconCatalog.HealerHealingLightSkill,
                        targetDescription: "대상: 살아 있는 아군 1명",
                        effectDescription: "회복량: 최대 HP의 35%");
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

    /// <summary>도발처럼 전투 참가자에게 남는 상태의 적용과 무효 상태 정리를 담당합니다.</summary>
    public sealed class BattleStatusEffectRuntime
    {
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
            long scaledDamage = (long)actor.Attack * skill.AttackDamagePercent;
            int rawDamage = (int)Math.Max(1L, (scaledDamage + 99L) / 100L);
            damage = target.TakeDamage(rawDamage);
            cooldowns.Start(actor, skill.Id, skill.CooldownTurns);
            message = $"{actor.DisplayName}의 {skill.DisplayName}! {target.DisplayName}에게 {damage} 피해.";
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

            long scaledDamage = (long)actor.Attack * skill.AttackDamagePercent;
            int rawDamage = (int)Math.Max(1L, (scaledDamage + 99L) / 100L);
            damage = target.TakeDamage(rawDamage);

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
            long scaledDamage = (long)actor.Attack * damagePercent;
            int rawDamage = (int)Math.Max(1L, (scaledDamage + 99L) / 100L);
            damage = target.TakeDamage(rawDamage);
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

            long scaledDamage = (long)actor.Attack * skill.AttackDamagePercent;
            int rawDamage = (int)Math.Max(1L, (scaledDamage + 99L) / 100L);
            int[] appliedDamages = new int[livingEnemies.Length];
            for (int index = 0; index < livingEnemies.Length; index++)
                appliedDamages[index] = livingEnemies[index].TakeDamage(rawDamage);

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
            long scaledDamage = (long)actor.Attack * skill.AttackDamagePercent;
            int rawDamage = (int)Math.Max(1L, (scaledDamage + 99L) / 100L);
            int[] appliedDamages = new int[livingEnemies.Length];
            for (int index = 0; index < livingEnemies.Length; index++)
                appliedDamages[index] = livingEnemies[index].TakeDamage(rawDamage);

            damages = appliedDamages;
            message = $"{actor.DisplayName}의 {skill.DisplayName}! 적 후열 {livingEnemies.Length}명에게 피해.";
            return true;
        }
    }
}

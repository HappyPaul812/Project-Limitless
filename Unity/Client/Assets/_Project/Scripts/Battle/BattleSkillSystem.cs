using System;
using System.Collections.Generic;
using System.Linq;
using ProjectLimitless.Core;

namespace ProjectLimitless.Battle
{
    /// <summary>실제 전투에서 실행할 스킬 효과 종류입니다. 구현된 효과만 이 열거형에 추가합니다.</summary>
    public enum BattleSkillEffectType { None, Taunt, SingleAllyHeal, SingleRangedPhysicalAttack, SingleMeleePhysicalAttackWithFighterEdge }

    /// <summary>JobDefinition의 프리뷰와 전투 실행 정보를 연결하는 읽기 전용 런타임 스킬 데이터입니다.</summary>
    public sealed class BattleSkillDefinition
    {
        public BattleSkillDefinition(string id, string displayName, string description, bool implemented,
            BattleSkillEffectType effectType, int cooldownTurns, int effectDuration, float maxHpHealRatio = 0f,
            int attackDamagePercent = 0, string iconId = null, string targetDescription = null,
            string effectDescription = null, string typeDescription = null, string durationDescription = null)
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
        public const string FighterEdgeId = "fighter_slash_stack";

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
                if (preview.SkillId == FighterEdgeId)
                    return new BattleSkillDefinition(preview.SkillId, preview.SkillName,
                        "적 1명을 베어 일반 공격 피해의 150%를 주고, 적중 후 자신이 난도 1중첩을 얻습니다.\n난도는 최대 3중첩이며 향후 회심의 일격을 강화합니다.\n난도 스킬을 직접 세 번째 사용해 3중첩이 되면 재사용 대기시간이 발생합니다.", true,
                        BattleSkillEffectType.SingleMeleePhysicalAttackWithFighterEdge, 2, 0, 0f, 150,
                        iconId: BattleUiIconCatalog.FighterEdgeSkill,
                        targetDescription: "대상: 적 1명",
                        effectDescription: "피해: 일반 공격의 150%\n적중 후: 자신에게 난도 +1\n최대 중첩: 3",
                        typeDescription: "유형: 근거리 물리",
                        durationDescription: "세 번째 사용 후 재사용: 2턴");
                return new BattleSkillDefinition(preview.SkillId, preview.SkillName, preview.SkillDescription, false,
                    BattleSkillEffectType.None, 0, 0);
            }).ToArray();
        }
    }

    /// <summary>
    /// 투사마다 자기 난도와 난도 스킬 직접 사용 횟수를 보관하는 전투 자원 저장소입니다.
    /// 난도는 특정 적에게 붙는 약화 효과가 아니라 투사 본인의 다음 기술을 강화하는 자원이므로 Combatant를
    /// 키로 사용합니다. 이렇게 해야 같은 전투에 투사가 여러 명 있어도 각자의 중첩이 섞이지 않습니다.
    /// </summary>
    public sealed class BattleFighterResourceRuntime
    {
        public const int MaxEdgeStacks = 3;
        private readonly Dictionary<Combatant, int> edgeStacksByActor = new Dictionary<Combatant, int>();
        private readonly Dictionary<Combatant, int> directUsesByActor = new Dictionary<Combatant, int>();

        public int GetEdgeStacks(Combatant actor) => actor != null && edgeStacksByActor.TryGetValue(actor, out int stacks) ? stacks : 0;

        /// <summary>
        /// 회오리 베기처럼 다른 기술이 적중 수만큼 난도를 줄 때 재사용할 공용 진입점입니다. Math.Min으로
        /// 3중첩 상한을 지키되 직접 사용 횟수는 건드리지 않으므로, 이 경로로 3이 되어도 난도 스킬 쿨타임은
        /// 생기지 않습니다. 반환값은 UI나 연출이 실제 증가량을 알 수 있게 합니다.
        /// </summary>
        public int AddEdgeStacks(Combatant actor, int requestedStacks)
        {
            if (actor == null || requestedStacks <= 0) return 0;
            int previous = GetEdgeStacks(actor);
            int next = Math.Min(MaxEdgeStacks, previous + requestedStacks);
            edgeStacksByActor[actor] = next;
            return next - previous;
        }

        /// <summary>난도 스킬의 성공한 직접 사용만 기록하며, 세 번째인지 호출자에게 알려 줍니다.</summary>
        public bool RecordDirectEdgeUse(Combatant actor)
        {
            if (actor == null) return false;
            int uses = directUsesByActor.TryGetValue(actor, out int current) ? current + 1 : 1;
            bool thirdUse = uses >= 3;
            directUsesByActor[actor] = thirdUse ? 0 : uses;
            return thirdUse;
        }

        /// <summary>
        /// 향후 회심의 일격이 배율 계산 전에 현재 난도를 읽고, 성공 후 전부 소비할 때 사용하는 API입니다.
        /// 자원 소비는 쿨타임이나 직접 사용 횟수를 바꾸지 않아 두 규칙이 서로 독립적으로 유지됩니다.
        /// </summary>
        public int ConsumeAllEdgeStacks(Combatant actor)
        {
            int consumed = GetEdgeStacks(actor);
            if (actor != null) edgeStacksByActor[actor] = 0;
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
            if (skill.EffectType == BattleSkillEffectType.SingleMeleePhysicalAttackWithFighterEdge &&
                fighterResources.GetEdgeStacks(actor) >= BattleFighterResourceRuntime.MaxEdgeStacks)
            {
                reason = "난도가 이미 최대입니다.";
                return false;
            }

            reason = string.Empty;
            return true;
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
        public bool ExecuteSingleMeleePhysicalAttackWithFighterEdge(Combatant actor, Combatant target,
            BattleSkillDefinition skill, out int damage, out string message)
        {
            damage = 0;
            if (!CanUse(actor, skill, out message)) return false;
            if (skill.EffectType != BattleSkillEffectType.SingleMeleePhysicalAttackWithFighterEdge || target == null ||
                target.Side == actor.Side || !target.IsAlive)
            {
                message = "공격할 수 있는 살아 있는 적이 아닙니다.";
                return false;
            }

            long scaledDamage = (long)actor.Attack * skill.AttackDamagePercent;
            int rawDamage = (int)Math.Max(1L, (scaledDamage + 99L) / 100L);
            damage = target.TakeDamage(rawDamage);

            // 피해가 적용된 뒤에만 난도를 올립니다. 자원 증가 API는 회오리 베기도 재사용할 수 있지만,
            // 직접 사용 기록은 이 난도 스킬 경로에서만 남겨 두 효과가 같은 3중첩을 만들더라도 구분됩니다.
            fighterResources.AddEdgeStacks(actor, 1);
            bool thirdDirectUse = fighterResources.RecordDirectEdgeUse(actor);
            int currentStacks = fighterResources.GetEdgeStacks(actor);
            if (thirdDirectUse && currentStacks >= BattleFighterResourceRuntime.MaxEdgeStacks)
                cooldowns.Start(actor, skill.Id, skill.CooldownTurns);

            message = $"{actor.DisplayName}의 {skill.DisplayName}! {target.DisplayName}에게 {damage} 피해. 현재 난도 {currentStacks}중첩.";
            return true;
        }
    }
}

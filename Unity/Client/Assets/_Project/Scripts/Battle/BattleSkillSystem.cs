using System;
using System.Collections.Generic;
using System.Linq;
using ProjectLimitless.Core;

namespace ProjectLimitless.Battle
{
    /// <summary>실제 전투에서 실행할 스킬 효과 종류입니다. 구현된 효과만 이 열거형에 추가합니다.</summary>
    public enum BattleSkillEffectType { None, Taunt }

    /// <summary>JobDefinition의 프리뷰와 전투 실행 정보를 연결하는 읽기 전용 런타임 스킬 데이터입니다.</summary>
    public sealed class BattleSkillDefinition
    {
        public BattleSkillDefinition(string id, string displayName, string description, bool implemented,
            BattleSkillEffectType effectType, int cooldownTurns, int effectDuration)
        {
            Id = id ?? string.Empty;
            DisplayName = displayName ?? string.Empty;
            Description = description ?? string.Empty;
            IsImplemented = implemented;
            EffectType = effectType;
            CooldownTurns = Math.Max(0, cooldownTurns);
            EffectDuration = Math.Max(0, effectDuration);
        }

        public string Id { get; }
        public string DisplayName { get; }
        public string Description { get; }
        public bool IsImplemented { get; }
        public BattleSkillEffectType EffectType { get; }
        public int CooldownTurns { get; }
        public int EffectDuration { get; }
    }

    /// <summary>
    /// 직업의 시작 스킬 프리뷰를 실제 전투 메뉴용 데이터로 변환합니다.
    /// 아직 구현되지 않은 스킬도 목록에는 남기되 실행 가능 상태로 만들지 않습니다.
    /// </summary>
    public static class BattleSkillCatalog
    {
        public const string GuardianTauntId = "guardian_taunt";

        public static IReadOnlyList<BattleSkillDefinition> GetSkills(JobDefinition job)
        {
            if (job == null) return Array.Empty<BattleSkillDefinition>();

            return job.StartingSkills.Select(preview => preview.SkillId == GuardianTauntId
                ? new BattleSkillDefinition(preview.SkillId, preview.SkillName, preview.SkillDescription, true,
                    BattleSkillEffectType.Taunt, 3, 2)
                : new BattleSkillDefinition(preview.SkillId, preview.SkillName, preview.SkillDescription, false,
                    BattleSkillEffectType.None, 0, 0)).ToArray();
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

        public BattleSkillExecutor(BattleSkillCooldowns cooldowns, BattleStatusEffectRuntime statusEffects)
        {
            this.cooldowns = cooldowns ?? throw new ArgumentNullException(nameof(cooldowns));
            this.statusEffects = statusEffects ?? throw new ArgumentNullException(nameof(statusEffects));
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
    }
}

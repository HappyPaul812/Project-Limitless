using System;
using System.Collections.Generic;
using System.Linq;
using ProjectLimitless.Core;

namespace ProjectLimitless.Battle
{
    /// <summary>
    /// 캐릭터 주변의 즉시 상태 표시와 상세 팝업이 함께 사용하는 상태 표식입니다.
    /// 현재는 한글 텍스트로 표시하지만 Id와 Count를 분리해 향후 아이콘 Asset으로 쉽게 교체할 수 있습니다.
    /// </summary>
    public readonly struct BattleStatusMarker
    {
        public BattleStatusMarker(string id, string label, int count = 0)
        {
            Id = id ?? string.Empty;
            Label = label ?? string.Empty;
            Count = Math.Max(0, count);
        }

        public string Id { get; }
        public string Label { get; }
        public int Count { get; }
        public string DisplayText => Count > 0 ? $"{Label} {Count}" : Label;
    }

    /// <summary>
    /// 쿨타임 계산 결과를 UI에 전달하는 읽기 전용 묶음입니다. 남은 턴과 원래 총 턴을 함께 보관하면
    /// HP HUD는 전투 값을 다시 계산하지 않고도 시작·진행·마지막 아이콘을 고를 수 있습니다.
    /// </summary>
    public readonly struct BattleCooldownStatus
    {
        public BattleCooldownStatus(string skillName, int remainingTurns, int totalTurns)
        {
            SkillName = skillName ?? string.Empty;
            RemainingTurns = Math.Max(0, remainingTurns);
            TotalTurns = Math.Max(0, totalTurns);
        }

        public string SkillName { get; }
        public int RemainingTurns { get; }
        public int TotalTurns { get; }
        public string DisplayText => $"{SkillName} 재사용 {RemainingTurns}턴";
    }

    /// <summary>
    /// 현재 구현된 전투 정보만 모아 UI에 전달하는 읽기 전용 표시 모델입니다.
    /// BattleCore의 값을 복사해 보여 주기만 하므로 고정 HP 목록이나 향후 보스 HP Bar를 추가해도
    /// 피해·방어·도발 계산 코드와 UI 코드가 서로 얽히지 않습니다.
    /// </summary>
    public sealed class BattleCombatantStatusViewModel
    {
        public BattleCombatantStatusViewModel(string title, string category, int currentHp, int maxHp,
            IReadOnlyList<BattleStatusMarker> markers, IReadOnlyList<BattleCooldownStatus> cooldowns,
            IReadOnlyList<string> resourceDetails = null)
        {
            Title = title ?? string.Empty;
            Category = category ?? string.Empty;
            CurrentHp = currentHp;
            MaxHp = maxHp;
            Markers = markers ?? Array.Empty<BattleStatusMarker>();
            Cooldowns = cooldowns ?? Array.Empty<BattleCooldownStatus>();
            ResourceDetails = resourceDetails ?? Array.Empty<string>();
        }

        public string Title { get; }
        public string Category { get; }
        public int CurrentHp { get; }
        public int MaxHp { get; }
        public IReadOnlyList<BattleStatusMarker> Markers { get; }
        public IReadOnlyList<BattleCooldownStatus> Cooldowns { get; }
        public IReadOnlyList<string> ResourceDetails { get; }

        public string CompactStatus => string.Join("  ", Markers.Select(marker => marker.DisplayText));

        public string DetailText
        {
            get
            {
                List<string> lines = new List<string> { $"{Title} · {Category}", $"HP {CurrentHp} / {MaxHp}" };
                // 기세는 상세 전용 `현재/최대` 줄이 있으므로 요약용 `기세 n`을 중복해서 넣지 않습니다.
                // 다른 상태는 기존처럼 같은 표식을 재사용해 HUD와 상세 정보가 어긋나지 않게 합니다.
                lines.AddRange(Markers.Where(marker => marker.Id != "fighter.momentum").Select(marker => marker.DisplayText));
                lines.AddRange(ResourceDetails);
                lines.AddRange(Cooldowns.Select(cooldown => cooldown.DisplayText));
                return string.Join("\n", lines);
            }
        }
    }

    /// <summary>Combatant의 계산값을 변경하지 않고 상세 상태 표시용 모델로 읽어 옵니다.</summary>
    public static class BattleCombatantStatusViewModelFactory
    {
        public static BattleCombatantStatusViewModel Create(Combatant combatant, JobDefinition job,
            BattleParticipantSetup setup, BattleSkillCooldowns cooldowns, BattleFighterResourceRuntime fighterResources,
            BattleStatusEffectRuntime statusEffects)
        {
            if (combatant == null) throw new ArgumentNullException(nameof(combatant));

            string category = job != null ? job.DisplayName
                : setup != null && setup.VisualType == BattleParticipantVisualType.EncounterMonster ? "몬스터" : "전투 참가자";

            // 전투불능 참가자에게 도발·방어·재사용 표시가 남으면 실제 전투 상태와 화면 정보가 달라집니다.
            // HP와 분류는 상세 팝업에서 계속 확인할 수 있게 두되, 행동에 의미가 있는 상태 목록은 즉시 비웁니다.
            // Combatant 내부 값을 억지로 바꾸지 않고 표시 모델의 경계에서 정리하므로 전투 계산 규칙에는 영향이 없습니다.
            if (!combatant.IsAlive)
                return new BattleCombatantStatusViewModel(combatant.DisplayName, category,
                    combatant.CurrentHp, combatant.MaxHp, Array.Empty<BattleStatusMarker>(), Array.Empty<BattleCooldownStatus>());

            List<BattleStatusMarker> markers = new List<BattleStatusMarker>();
            if (combatant.IsDefending) markers.Add(new BattleStatusMarker("defend", "방어"));
            if (combatant.ForcedTargetActionsRemaining > 0 && combatant.ForcedTarget != null && combatant.ForcedTarget.IsAlive)
                markers.Add(new BattleStatusMarker("taunt", "도발", combatant.ForcedTargetActionsRemaining));
            // UI는 실제 자원을 바꾸지 않고 참가자별 런타임 값을 읽어 표시만 합니다. 계산과 표시를 나누면
            // HUD를 고쳐도 회오리 베기·회심의 일격의 중첩 판정에는 영향을 주지 않습니다.
            int momentum = fighterResources?.GetMomentum(combatant) ?? 0;
            if (momentum > 0) markers.Add(new BattleStatusMarker("fighter.momentum", "기세", momentum));
            // 화상 숫자는 "몇 턴"이 아니라 앞으로 대상 행동 종료 시 피해가 발생할 남은 횟수입니다.
            // 계산은 상태 저장소가 담당하고 UI는 읽기만 하므로 HUD 표시가 화상 횟수를 소모하지 않습니다.
            int burnRemaining = statusEffects?.GetBurnRemaining(combatant) ?? 0;
            if (burnRemaining > 0) markers.Add(new BattleStatusMarker("burn", "화상", burnRemaining));
            int poisonRemaining = statusEffects?.GetPoisonRemaining(combatant) ?? 0;
            if (poisonRemaining > 0) markers.Add(new BattleStatusMarker("poison", "독", poisonRemaining));
            // 감전은 대상마다 독립적으로 감전 1만 유지합니다. HUD와 상세 팝업이 같은 Marker를 읽으므로
            // 어느 한쪽만 남는 일이 없고, 행동 종료 시 런타임에서 제거되면 두 화면에서도 함께 사라집니다.
            if (statusEffects?.HasShock(combatant) == true)
                markers.Add(new BattleStatusMarker("shock", "감전", 1));
            // 남은 수치는 전체 라운드가 아니라 이 마도사가 앞으로 마칠 행동 횟수입니다. 다른 참가자의
            // 차례에는 값이 변하지 않으며 HUD와 상세 팝업은 같은 Marker를 사용합니다.
            int gaiaRemaining = statusEffects?.GetGaiaWallRemaining(combatant) ?? 0;
            if (gaiaRemaining > 0) markers.Add(new BattleStatusMarker("gaia", "가이아", gaiaRemaining));
            // 상단 요약은 공간을 아끼기 위해 1중첩부터 표시하지만, 투사의 상세 팝업은 자원이 0일 때도
            // 현재값과 상한을 함께 보여 줍니다. UI 문구는 읽기만 하며 실제 전투 자원은 변경하지 않습니다.
            List<string> resourceDetails = new List<string>();
            if (poisonRemaining > 0) resourceDetails.Add("독: 행동 종료 시 최대 HP 5% 피해");
            if (job != null && job.JobId == "fighter")
                resourceDetails.Add($"기세 {momentum}/{BattleFighterResourceRuntime.MaxMomentum}");

            List<BattleCooldownStatus> cooldownLines = new List<BattleCooldownStatus>();
            foreach (BattleSkillDefinition skill in BattleSkillCatalog.GetSkills(job).Where(skill => skill.IsImplemented))
            {
                int remaining = cooldowns?.GetRemaining(combatant, skill.Id) ?? 0;
                if (remaining > 0)
                    cooldownLines.Add(new BattleCooldownStatus(skill.DisplayName, remaining, skill.CooldownTurns));
            }

            return new BattleCombatantStatusViewModel(combatant.DisplayName, category,
                combatant.CurrentHp, combatant.MaxHp, markers, cooldownLines, resourceDetails);
        }
    }
}

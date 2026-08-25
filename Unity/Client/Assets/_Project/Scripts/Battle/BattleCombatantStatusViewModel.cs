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
    /// 현재 구현된 전투 정보만 모아 UI에 전달하는 읽기 전용 표시 모델입니다.
    /// BattleCore의 값을 복사해 보여 주기만 하므로 고정 HP 목록이나 향후 보스 HP Bar를 추가해도
    /// 피해·방어·도발 계산 코드와 UI 코드가 서로 얽히지 않습니다.
    /// </summary>
    public sealed class BattleCombatantStatusViewModel
    {
        public BattleCombatantStatusViewModel(string title, string category, int currentHp, int maxHp,
            IReadOnlyList<BattleStatusMarker> markers, IReadOnlyList<string> cooldowns)
        {
            Title = title ?? string.Empty;
            Category = category ?? string.Empty;
            CurrentHp = currentHp;
            MaxHp = maxHp;
            Markers = markers ?? Array.Empty<BattleStatusMarker>();
            Cooldowns = cooldowns ?? Array.Empty<string>();
        }

        public string Title { get; }
        public string Category { get; }
        public int CurrentHp { get; }
        public int MaxHp { get; }
        public IReadOnlyList<BattleStatusMarker> Markers { get; }
        public IReadOnlyList<string> Cooldowns { get; }

        public string CompactStatus => string.Join("  ", Markers.Select(marker => marker.DisplayText));

        public string DetailText
        {
            get
            {
                List<string> lines = new List<string> { $"{Title} · {Category}", $"HP {CurrentHp} / {MaxHp}" };
                lines.AddRange(Markers.Select(marker => marker.DisplayText));
                lines.AddRange(Cooldowns);
                return string.Join("\n", lines);
            }
        }
    }

    /// <summary>Combatant의 계산값을 변경하지 않고 상세 상태 표시용 모델로 읽어 옵니다.</summary>
    public static class BattleCombatantStatusViewModelFactory
    {
        public static BattleCombatantStatusViewModel Create(Combatant combatant, JobDefinition job,
            BattleParticipantSetup setup, BattleSkillCooldowns cooldowns)
        {
            if (combatant == null) throw new ArgumentNullException(nameof(combatant));

            string category = job != null ? job.DisplayName
                : setup != null && setup.VisualType == BattleParticipantVisualType.EncounterMonster ? "몬스터" : "전투 참가자";
            List<BattleStatusMarker> markers = new List<BattleStatusMarker>();
            if (combatant.IsDefending) markers.Add(new BattleStatusMarker("defend", "방어"));
            if (combatant.ForcedTargetActionsRemaining > 0 && combatant.ForcedTarget != null && combatant.ForcedTarget.IsAlive)
                markers.Add(new BattleStatusMarker("taunt", "도발", combatant.ForcedTargetActionsRemaining));

            List<string> cooldownLines = new List<string>();
            foreach (BattleSkillDefinition skill in BattleSkillCatalog.GetSkills(job).Where(skill => skill.IsImplemented))
            {
                int remaining = cooldowns?.GetRemaining(combatant, skill.Id) ?? 0;
                if (remaining > 0) cooldownLines.Add($"{skill.DisplayName} 재사용 {remaining}턴");
            }

            return new BattleCombatantStatusViewModel(combatant.DisplayName, category,
                combatant.CurrentHp, combatant.MaxHp, markers, cooldownLines);
        }
    }
}

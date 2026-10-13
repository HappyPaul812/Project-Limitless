using System;
using System.Collections.Generic;
using System.Linq;

using ProjectLimitless.Core;
using UnityEngine.SceneManagement;

namespace ProjectLimitless.Battle
{
    /// <summary>저장 계약 미확정으로 파일에 쓰지 않고 동료 ID·직업별 장착을 이번 실행 동안만 유지합니다.</summary>
    public static class SkillLoadoutService
    {
        public const int SlotCount = 6;
        private sealed class State
        {
            public readonly List<BattleSkillDefinition> Library = new List<BattleSkillDefinition>();
            public readonly string[] Slots = new string[SlotCount];
        }
        private static readonly Dictionary<string, State> states = new Dictionary<string, State>(StringComparer.Ordinal);
        private static State Resolve(string characterId, JobDefinition job)
        {
            if (job == null || string.IsNullOrEmpty(characterId)) return null;
            string key = characterId + ":" + job.JobId;
            if (states.TryGetValue(key, out State state)) return state;
            state = new State();
            state.Library.AddRange(BattleSkillCatalog.GetSkills(job));
            for (int i = 0; i < Math.Min(SlotCount, state.Library.Count); i++) state.Slots[i] = state.Library[i].Id;
            states.Add(key, state);
            return state;
        }
        public static IReadOnlyList<BattleSkillDefinition> GetLibrary(string characterId, JobDefinition job) =>
            Resolve(characterId, job)?.Library.ToArray() ?? Array.Empty<BattleSkillDefinition>();
        public static IReadOnlyList<string> GetSlots(string characterId, JobDefinition job) =>
            Resolve(characterId, job)?.Slots.ToArray() ?? new string[SlotCount];
        // 전투 시작 시 복사본을 고정합니다. 메뉴 재개나 대상 취소로 장착 상태를 다시 읽지 않습니다.
        public static IReadOnlyList<BattleSkillDefinition> Snapshot(string characterId, JobDefinition job)
        {
            State state = Resolve(characterId, job);
            return state == null ? Array.Empty<BattleSkillDefinition>() : state.Slots.Where(id => !string.IsNullOrEmpty(id))
                .Select(id => state.Library.FirstOrDefault(skill => skill.Id == id)).Where(skill => skill != null).ToArray();
        }
        // UI 소유자만 자신의 잠금을 통과할 수 있고 전투 Scene에서는 어떤 소유자도 변경할 수 없습니다.
        public static bool CanManage(object modalOwner = null) =>
            !SceneManager.GetActiveScene().name.StartsWith("Battle", StringComparison.Ordinal)
            && (!WorldModalState.IsOpen || WorldModalState.IsOwnedBy(modalOwner))
            && (!ProjectLimitless.Player.PlayerController.IsMovementLocked || WorldModalState.IsOwnedBy(modalOwner));
        public static bool TrySetSlot(string characterId, JobDefinition job, int slot, string skillId, object modalOwner = null)
        {
            if (!CanManage(modalOwner) || slot < 0 || slot >= SlotCount) return false;
            State state = Resolve(characterId, job);
            if (state == null || (!string.IsNullOrEmpty(skillId) && !state.Library.Any(skill => skill.Id == skillId))) return false;
            // 같은 스킬을 다른 칸에 다시 장착하면 이전 칸을 비웁니다. 액티브 중복 표시를 막습니다.
            for (int i = 0; i < SlotCount; i++) if (state.Slots[i] == skillId && !string.IsNullOrEmpty(skillId)) state.Slots[i] = null;
            state.Slots[slot] = string.IsNullOrEmpty(skillId) ? null : skillId;
            return true;
        }
        /// <summary>미래 구매 계층이 직업/가격/영속 거래를 확정한 뒤 사용할 내부 계약입니다. 자동 장착은 하지 않습니다.</summary>
        internal static bool RegisterApprovedDefinition(string characterId, JobDefinition job, BattleSkillDefinition definition, string replacesId = null)
        {
            if (!CanManage() || definition == null || !definition.IsImplemented || string.IsNullOrEmpty(definition.Id)) return false;
            State state = Resolve(characterId, job);
            if (state == null || state.Library.Any(skill => skill.Id == definition.Id)) return false;
            if (!string.IsNullOrEmpty(replacesId))
            {
                int old = state.Library.FindIndex(skill => skill.Id == replacesId);
                if (old < 0) return false;
                state.Library[old] = definition;
                for (int i = 0; i < SlotCount; i++) if (state.Slots[i] == replacesId) state.Slots[i] = definition.Id;
            }
            else state.Library.Add(definition);
            return true;
        }
        /// <summary>새 게임/다른 Save 로드 때 호출하여 저장되지 않은 임시 편성이 다른 캐릭터로 유출되지 않게 합니다.</summary>
        public static void Reset() => states.Clear();
    }
}

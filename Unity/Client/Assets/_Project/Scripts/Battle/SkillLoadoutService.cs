using System;
using System.Collections.Generic;
using System.Linq;

using ProjectLimitless.Core;
using UnityEngine.SceneManagement;

namespace ProjectLimitless.Battle
{
    [Serializable]
    public sealed class SkillLoadoutSaveData
    {
        public int SchemaVersion = 1;
        public CharacterSkillLoadoutSaveData[] Characters;
    }
    [Serializable]
    public sealed class CharacterSkillLoadoutSaveData
    {
        public string CharacterId;
        public string JobId;
        public string[] OwnedSkillIds;
        public string[] EquippedSlots;
    }

    /// <summary>캐릭터·직업별 보관함과 6칸을 관리하고 안정적인 ID만 Save에 전달합니다.</summary>
    public static class SkillLoadoutService
    {
        public const int SlotCount = 6;
        private sealed class State
        {
            public string CharacterId;
            public string JobId;
            public readonly List<BattleSkillDefinition> Library = new List<BattleSkillDefinition>();
            public readonly string[] Slots = new string[SlotCount];
        }
        private static readonly Dictionary<string, State> states = new Dictionary<string, State>(StringComparer.Ordinal);
        private static State Resolve(string characterId, JobDefinition job)
        {
            if (job == null || string.IsNullOrEmpty(characterId)) return null;
            string key = characterId + ":" + job.JobId;
            if (states.TryGetValue(key, out State state)) return state;
            state = new State { CharacterId = characterId, JobId = job.JobId };
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
            string[] before = (string[])state.Slots.Clone();
            // 같은 스킬을 다른 칸에 다시 장착하면 이전 칸을 비웁니다. 액티브 중복 표시를 막습니다.
            for (int i = 0; i < SlotCount; i++) if (state.Slots[i] == skillId && !string.IsNullOrEmpty(skillId)) state.Slots[i] = null;
            state.Slots[slot] = string.IsNullOrEmpty(skillId) ? null : skillId;
            // 실제 슬롯을 선택한 게임에서는 변경 직후 저장합니다. 실패하면 화면/전투도 이전 구성입니다.
            // Export는 상태를 읽기만 하므로 이 저장이 장착 변경을 다시 호출하지 않습니다.
            if (GameSaveService.CurrentSlotIndex > 0 && !GameSaveService.SaveCurrentSession())
            {
                Array.Copy(before, state.Slots, SlotCount);
                return false;
            }
            return true;
        }
        /// <summary>비활성 동료/직업도 보존합니다. 키 문자열을 다시 분해하거나 Unity Object를 저장하지 않습니다.</summary>
        public static SkillLoadoutSaveData ExportSaveData() => new SkillLoadoutSaveData
        {
            Characters = states.Values.OrderBy(s => s.CharacterId, StringComparer.Ordinal).ThenBy(s => s.JobId, StringComparer.Ordinal)
                .Select(s => new CharacterSkillLoadoutSaveData { CharacterId = s.CharacterId, JobId = s.JobId,
                    OwnedSkillIds = s.Library.Select(skill => skill.Id).Distinct(StringComparer.Ordinal).ToArray(),
                    EquippedSlots = (string[])s.Slots.Clone() }).ToArray()
        };
        /// <summary>구버전 누락은 기본 3개, 명시적인 빈 칸은 해제 상태입니다. 손상은 해당 기록에만 제한합니다.</summary>
        public static void ImportSaveData(SkillLoadoutSaveData data)
        {
            Reset();
            if (data == null || data.SchemaVersion != 1 || data.Characters == null) return;
            var jobs = UnityEngine.Resources.LoadAll<JobDefinition>("JobDefinitions");
            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var row in data.Characters)
            {
                if (row == null || string.IsNullOrEmpty(row.CharacterId) || string.IsNullOrEmpty(row.JobId)) continue;
                JobDefinition job = jobs.FirstOrDefault(j => j.JobId == row.JobId);
                var companion = CompanionCatalog.Find(row.CharacterId);
                if (job == null || (row.CharacterId != PartyResourceService.PlayerCharacterId &&
                    (companion == null || companion.JobId != row.JobId))) continue;
                if (!seen.Add(row.CharacterId + ":" + row.JobId)) continue;
                State state = Resolve(row.CharacterId, job);
                // 현재 정본 카탈로그는 무료 기본 스킬입니다. 저장 ID를 새 정의/구매/강화 승인으로 사용하지 않습니다.
                // 미등록 추가·강화판은 복원하지 않으며 기본 보유권은 항상 유지합니다. 상품/수치 확정은 별도입니다.
                var owned = new HashSet<string>(state.Library.Where(s => s.IsImplemented).Select(s => s.Id), StringComparer.Ordinal);
                if (row.EquippedSlots == null) continue;
                Array.Clear(state.Slots, 0, SlotCount);
                var equipped = new HashSet<string>(StringComparer.Ordinal);
                for (int i = 0; i < Math.Min(SlotCount, row.EquippedSlots.Length); i++)
                {
                    string id = row.EquippedSlots[i];
                    if (!string.IsNullOrEmpty(id) && owned.Contains(id) && equipped.Add(id)) state.Slots[i] = id;
                }
            }
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
        /// <summary>새 게임/다른 슬롯 로드 전에 호출하여 이전 캐릭터의 장착이 섞이지 않게 합니다.</summary>
        public static void Reset() => states.Clear();
    }
}

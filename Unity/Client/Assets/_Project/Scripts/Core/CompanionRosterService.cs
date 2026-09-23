using System;
using System.Collections.Generic;
using System.Linq;
using ProjectLimitless.Battle;

namespace ProjectLimitless.Core
{
    /// <summary>정식 해금 동료와 현재 기본 파티의 stable CharacterId만 저장합니다.</summary>
    [Serializable]
    public sealed class CompanionRosterSaveData
    {
        public string[] UnlockedCharacterIds = Array.Empty<string>();
        public string[] ActivePartyCharacterIds = Array.Empty<string>();
        public PartyFormationSaveData[] Formation;
        public bool HasManualComposition;
        public bool PaulDefaultApplied;
    }

    /// <summary>기존 FormationSlot을 JSON 값으로 저장합니다. Column은 행별로 중복 없이 재구성합니다.</summary>
    [Serializable]
    public sealed class PartyFormationSaveData
    {
        public string CharacterId;
        public FormationRow Row;
    }

    /// <summary>
    /// Story Encounter에 잠시 참가하는 인물과 저장되는 정식 동료를 구분합니다.
    /// Main 05 전에는 태온·미엘이 이 목록에 없고, 완료 뒤부터 일반 전투와 저장에서 정식 파티로 취급합니다.
    /// </summary>
    public static class CompanionRosterService
    {
        public const string TaeonId = "companion_taeon";
        public const string MielId = "companion_miel";
        public const string PaulId = "companion_paul";
        private static readonly HashSet<string> Unlocked = new HashSet<string>(StringComparer.Ordinal);
        private static readonly List<string> ActiveParty = new List<string>();
        private static readonly Dictionary<string, FormationRow> Rows = new Dictionary<string, FormationRow>();
        public static bool HasManualComposition { get; private set; }
        public static bool PaulDefaultApplied { get; private set; }
        public const int SoloCompanionLimit = 2;

        public static IReadOnlyCollection<string> UnlockedCharacterIds => Unlocked;
        public static IReadOnlyList<string> ActivePartyCharacterIds => ActiveParty;
        public static bool IsUnlocked(string characterId) => Unlocked.Contains(characterId ?? string.Empty);
        public static bool IsActivePartyMember(string characterId) => ActiveParty.Contains(characterId ?? string.Empty);

        /// <summary>Main 05 완료 시 한 번 호출하며, 반복 호출해도 중복 ID가 생기지 않습니다.</summary>
        public static bool UnlockIntroCompanions()
        {
            bool changed = Unlocked.Add(TaeonId) | Unlocked.Add(MielId);
            if (changed && !HasManualComposition) SetDefaultIntroParty();
            return changed;
        }

        /// <summary>최초 합류 때만 직업 기본 편성을 적용하며 사용자가 확정한 편성은 보존합니다.</summary>
        public static bool UnlockPaul(string playerJobId)
        {
            bool added = Unlocked.Add(PaulId);
            if (!PaulDefaultApplied)
            {
                PaulDefaultApplied = true;
                if (!HasManualComposition)
                {
                    ActiveParty.Clear();
                    AddIfUnlocked(playerJobId == "guardian" ? MielId : TaeonId);
                    AddIfUnlocked(playerJobId == "guardian" || playerJobId == "healer" ? PaulId : MielId);
                }
            }
            return added;
        }

        public static FormationRow GetRow(string id) => Rows.TryGetValue(id, out FormationRow row)
            ? row : (CompanionCatalog.Find(id)?.DefaultRow ?? FormationRow.Front);

        public static FormationSlot GetSlot(string id)
        {
            var members = new[] { PartyResourceService.PlayerCharacterId }.Concat(ActiveParty).ToArray();
            FormationRow row = GetRow(id);
            int column = members.TakeWhile(x => x != id).Count(x => GetRow(x) == row);
            return new FormationSlot(row, column);
        }

        public static bool HasOffensiveRole(string playerJobId, IEnumerable<string> ids) =>
            IsOffensiveJob(playerJobId) || ids.Any(id => IsOffensiveJob(CompanionCatalog.Find(id)?.JobId));
        private static bool IsOffensiveJob(string job) => job == "sharpshooter" || job == "fighter" || job == "mage";

        /// <summary>안전지역 확인은 UI 진입점이 담당하고 서비스는 명단·중복·행을 원자적으로 검증합니다.</summary>
        public static bool TrySetComposition(IEnumerable<string> ids, IReadOnlyDictionary<string, FormationRow> rows)
        {
            string[] selected = ids?.ToArray();
            if (selected == null || selected.Length > SoloCompanionLimit || selected.Distinct().Count() != selected.Length
                || selected.Any(id => !IsUnlocked(id) || CompanionCatalog.Find(id) == null) || rows == null) return false;
            string[] members = new[] { PartyResourceService.PlayerCharacterId }.Concat(selected).ToArray();
            if (members.Any(id => !rows.ContainsKey(id) || !Enum.IsDefined(typeof(FormationRow), rows[id]))) return false;
            ActiveParty.Clear(); ActiveParty.AddRange(selected);
            foreach (string id in members) Rows[id] = rows[id];
            HasManualComposition = true;
            return true;
        }

        /// <summary>Player는 항상 암묵적으로 포함되므로 저장 배열에는 교체 가능한 동료만 기록합니다.</summary>
        public static void SetDefaultIntroParty()
        {
            ActiveParty.Clear();
            AddIfUnlocked(TaeonId);
            AddIfUnlocked(MielId);
        }

        public static CompanionRosterSaveData ExportSaveData() => new CompanionRosterSaveData
        {
            UnlockedCharacterIds = Unlocked.OrderBy(id => id, StringComparer.Ordinal).ToArray(),
            ActivePartyCharacterIds = ActiveParty.ToArray(),
            Formation = Rows.Select(x => new PartyFormationSaveData { CharacterId = x.Key, Row = x.Value }).ToArray(),
            HasManualComposition = HasManualComposition,
            PaulDefaultApplied = PaulDefaultApplied
        };

        public static void ImportSaveData(CompanionRosterSaveData data)
        {
            // 저장된 ID를 현재 동료 카탈로그와 대조하면서 중복·미해금·인원 초과를 걸러냅니다.
            // null은 구버전 Save에 이 필드가 없었다는 뜻으로 받아 빈 편성에서 다시 시작합니다.
            Reset();
            if (data == null) return; // 구버전 Version 1 Save는 정식 동료가 없는 상태로 호환합니다.
            if (data.UnlockedCharacterIds != null)
                foreach (string id in data.UnlockedCharacterIds)
                    if (!string.IsNullOrWhiteSpace(id)) Unlocked.Add(id);
            if (data.ActivePartyCharacterIds != null)
                foreach (string id in data.ActivePartyCharacterIds)
                    if (!string.IsNullOrWhiteSpace(id) && Unlocked.Contains(id) && !ActiveParty.Contains(id) && ActiveParty.Count < 2)
                        ActiveParty.Add(id);
            HasManualComposition = data.HasManualComposition;
            PaulDefaultApplied = data.PaulDefaultApplied || Unlocked.Contains(PaulId);
            foreach (PartyFormationSaveData entry in data.Formation ?? Array.Empty<PartyFormationSaveData>())
                if (entry != null && (entry.CharacterId == PartyResourceService.PlayerCharacterId || IsUnlocked(entry.CharacterId))
                    && Enum.IsDefined(typeof(FormationRow), entry.Row)) Rows[entry.CharacterId] = entry.Row;
            if (data.ActivePartyCharacterIds == null && !HasManualComposition) SetDefaultIntroParty();
        }

        public static void Reset()
        {
            Unlocked.Clear();
            ActiveParty.Clear();
            Rows.Clear(); HasManualComposition = false; PaulDefaultApplied = false;
        }

        private static void AddIfUnlocked(string id)
        {
            if (Unlocked.Contains(id) && !ActiveParty.Contains(id) && ActiveParty.Count < 2) ActiveParty.Add(id);
        }
    }
}

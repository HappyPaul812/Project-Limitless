using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectLimitless.Core
{
    /// <summary>정식 해금 동료와 현재 기본 파티의 stable CharacterId만 저장합니다.</summary>
    [Serializable]
    public sealed class CompanionRosterSaveData
    {
        public string[] UnlockedCharacterIds = Array.Empty<string>();
        public string[] ActivePartyCharacterIds = Array.Empty<string>();
    }

    /// <summary>
    /// Story Encounter에 잠시 참가하는 인물과 저장되는 정식 동료를 구분합니다.
    /// Main 05 전에는 태온·미엘이 이 목록에 없고, 완료 뒤부터 일반 전투와 저장에서 정식 파티로 취급합니다.
    /// </summary>
    public static class CompanionRosterService
    {
        public const string TaeonId = "companion_taeon";
        public const string MielId = "companion_miel";
        private static readonly HashSet<string> Unlocked = new HashSet<string>(StringComparer.Ordinal);
        private static readonly List<string> ActiveParty = new List<string>();

        public static IReadOnlyCollection<string> UnlockedCharacterIds => Unlocked;
        public static IReadOnlyList<string> ActivePartyCharacterIds => ActiveParty;
        public static bool IsUnlocked(string characterId) => Unlocked.Contains(characterId ?? string.Empty);
        public static bool IsActivePartyMember(string characterId) => ActiveParty.Contains(characterId ?? string.Empty);

        /// <summary>Main 05 완료 시 한 번 호출하며, 반복 호출해도 중복 ID가 생기지 않습니다.</summary>
        public static bool UnlockIntroCompanions()
        {
            bool changed = Unlocked.Add(TaeonId) | Unlocked.Add(MielId);
            SetDefaultIntroParty();
            return changed;
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
            ActivePartyCharacterIds = ActiveParty.ToArray()
        };

        public static void ImportSaveData(CompanionRosterSaveData data)
        {
            Reset();
            if (data == null) return; // 구버전 Version 1 Save는 정식 동료가 없는 상태로 호환합니다.
            if (data.UnlockedCharacterIds != null)
                foreach (string id in data.UnlockedCharacterIds)
                    if (!string.IsNullOrWhiteSpace(id)) Unlocked.Add(id);
            if (data.ActivePartyCharacterIds != null)
                foreach (string id in data.ActivePartyCharacterIds)
                    if (!string.IsNullOrWhiteSpace(id) && Unlocked.Contains(id) && !ActiveParty.Contains(id) && ActiveParty.Count < 2)
                        ActiveParty.Add(id);
        }

        public static void Reset()
        {
            Unlocked.Clear();
            ActiveParty.Clear();
        }

        private static void AddIfUnlocked(string id)
        {
            if (Unlocked.Contains(id) && !ActiveParty.Contains(id) && ActiveParty.Count < 2) ActiveParty.Add(id);
        }
    }
}

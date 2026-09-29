using System;
using System.Collections.Generic;
using System.Linq;
using ProjectLimitless.Battle;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectLimitless.Core
{
    [Serializable]
    public sealed class EquippedBeastSaveData
    {
        public string CharacterId;
        public string BeastId;
    }

    [Serializable]
    public sealed class BeastCompanionSaveData
    {
        public string[] DefeatedMonsterIds;
        public string[] UnlockedMonsterPetIds;
        public EquippedBeastSaveData[] EquippedBeasts;
    }

    /// <summary>
    /// 전투 승리 이력, 종 해금, 사수별 장착을 분리합니다. 승리만으로는 장착할 수 없으며
    /// 기본 세 종은 구버전 저장에서도 항상 무료로 사용됩니다.
    /// </summary>
    public static class BeastCompanionService
    {
        public const int MonsterPetPrice = 100;
        private static readonly string[] BaseIds = { "wolf", "bear", "fox" };
        private static readonly string[] MonsterIds =
        {
            "venom_bee", "forest_spider", "venom_snake",
            "monster_moss_beetle", "monster_shade_bat",
            "soot_hound", "heatwind_hawk", "fissure_lizard", "ember_beetle"
        };
        private static readonly HashSet<string> Defeated = new HashSet<string>(StringComparer.Ordinal);
        private static readonly HashSet<string> Unlocked = new HashSet<string>(StringComparer.Ordinal);
        private static readonly Dictionary<string, string> Equipped =
            new Dictionary<string, string>(StringComparer.Ordinal);

        public static IReadOnlyCollection<string> DefeatedMonsterIds => Defeated;
        public static IReadOnlyCollection<string> UnlockedMonsterPetIds => Unlocked;
        public static bool IsMonsterPet(string id) => Array.IndexOf(MonsterIds, id) >= 0;
        public static bool IsUnlocked(string id) => Array.IndexOf(BaseIds, id) >= 0 || Unlocked.Contains(id ?? string.Empty);
        public static string GetEquippedId(string characterId) =>
            characterId != null && Equipped.TryGetValue(characterId, out string id) && IsUnlocked(id) ? id :
                characterId == ProjectLimitless.World.Chapter2IntroFlow.SerinId ? "fox" : "wolf";

        /// <summary>실제 전체 승리 분기에서 전달된 MonsterDefinition 종만 기록합니다.</summary>
        public static void RecordVictory(IEnumerable<string> monsterIds)
        {
            if (monsterIds == null) return;
            foreach (string id in monsterIds)
                if (!string.IsNullOrWhiteSpace(id)) Defeated.Add(id);
        }

        public static string[] GetAdoptionCandidates() =>
            MonsterIds.Where(id => Defeated.Contains(id) && !Unlocked.Contains(id)).ToArray();
        public static string[] GetDiscoveredMonsterPets() => MonsterIds.Where(Defeated.Contains).ToArray();
        public static string GetPassiveDescription(string id)
        {
            switch (id)
            {
                case "wolf": return "직접 피해 +5% (지속 피해 제외)";
                case "bear": return "최대 HP +10%";
                case "fox": return "민첩 +2";
                case "venom_bee": return "동료의 습격 후 생존 대상에게 일반 독";
                case "venom_snake": return "전투마다 첫 유효 독 1회 무효";
                case "forest_spider": return "적대 광역 직접 피해 -10%";
                case "monster_moss_beetle": return "직접 피해 -5%";
                case "monster_shade_bat": return "전투마다 첫 유효 침묵 1회 무효";
                case "soot_hound": return "동료의 습격 후 생존 대상에게 화상 2회";
                case "heatwind_hawk": return "동료의 습격 후열 대상 직접 피해 +10%";
                case "fissure_lizard": return "전투마다 첫 유효 직접 피해 1회 -20%";
                case "ember_beetle": return "전투마다 첫 유효 화상 1회 무효";
                default: return string.Empty;
            }
        }

        /// <summary>자격과 잔액을 모두 확인한 뒤 한 번만 차감하므로 실패한 구매는 화폐를 잃지 않습니다.</summary>
        public static bool TryAdopt(string id)
        {
            if (!IsMonsterPet(id) || !Defeated.Contains(id) || Unlocked.Contains(id)
                || !EconomyService.CanAfford(MonsterPetPrice)) return false;
            if (!EconomyService.TrySpendCurrency(MonsterPetPrice)) return false;
            Unlocked.Add(id);
            if (GameSaveService.SaveCurrentSession()) return true;
            // 슬롯 쓰기에 실패했다면 같은 실행 중에도 미완료 구매로 되돌려 화폐와 해금을 함께 보존합니다.
            Unlocked.Remove(id);
            EconomyService.AddCurrency(MonsterPetPrice);
            return false;
        }

        public static string[] GetAvailableBeasts() => BaseIds.Concat(MonsterIds.Where(Unlocked.Contains)).ToArray();
        public static int GetEffectiveMaxHp(int baseMaxHp, string beastId) =>
            beastId == "bear" ? (int)Math.Min(int.MaxValue, ((long)Math.Max(1, baseMaxHp) * 110L + 99L) / 100L)
                : Math.Max(1, baseMaxHp);
        public static int GetEffectiveAgility(int baseAgility, string beastId) =>
            beastId == "fox" ? baseAgility + 2 : baseAgility;

        /// <summary>관리 UI가 호출하는 장착 경계입니다. Battle Scene에서는 교체할 수 없습니다.</summary>
        public static bool TryEquip(string characterId, string beastId)
        {
            if (SceneManager.GetActiveScene().name == BattleSceneFlow.BattleSceneName
                || !IsSharpshooter(characterId) || !IsUnlocked(beastId)) return false;
            string previous = GetEquippedId(characterId);
            int baseMaxHp = characterId == PartyResourceService.PlayerCharacterId
                ? CharacterGrowthCalculator.CalculateMaxHp(GameSessionData.SelectedJobId,
                    CharacterGrowthCalculator.Calculate(GameSessionData.SelectedJobId, GameSessionData.Level))
                : CompanionCatalog.Find(characterId).MaxHp;
            int oldMaxHp = GetEffectiveMaxHp(baseMaxHp, previous);
            int newMaxHp = GetEffectiveMaxHp(baseMaxHp, beastId);
            bool hadResources = PartyResourceService.TryGet(characterId, out PartyMemberResourceSnapshot before);
            PartyResourceService.RescaleMaxHp(characterId, oldMaxHp, newMaxHp);
            Equipped[characterId] = beastId;
            if (GameSaveService.SaveCurrentSession()) return true;
            if (hadResources)
                PartyResourceService.RecordBattleResult(characterId, before.CurrentHp, before.CurrentMp,
                    oldMaxHp, before.MaxMp);
            Equipped[characterId] = previous;
            return false;
        }

        public static string[] GetSharpshooterCharacterIds()
        {
            var ids = new List<string>();
            if (GameSessionData.SelectedJobId == "sharpshooter") ids.Add(PartyResourceService.PlayerCharacterId);
            foreach (string id in CompanionRosterService.UnlockedCharacterIds)
                if (CompanionCatalog.Find(id)?.JobId == "sharpshooter") ids.Add(id);
            return ids.ToArray();
        }

        public static bool IsSharpshooter(string characterId) =>
            characterId == PartyResourceService.PlayerCharacterId
                ? GameSessionData.SelectedJobId == "sharpshooter"
                : characterId == ProjectLimitless.World.Chapter2IntroFlow.SerinId
                    ? ProjectLimitless.World.Chapter2IntroFlow.SerinTemporarilyPresent
                    : CompanionRosterService.IsUnlocked(characterId)
                      && CompanionCatalog.Find(characterId)?.JobId == "sharpshooter";

        public static BeastCompanionSaveData ExportSaveData() => new BeastCompanionSaveData
        {
            DefeatedMonsterIds = Defeated.OrderBy(id => id, StringComparer.Ordinal).ToArray(),
            UnlockedMonsterPetIds = Unlocked.OrderBy(id => id, StringComparer.Ordinal).ToArray(),
            EquippedBeasts = Equipped.Select(pair => new EquippedBeastSaveData
                { CharacterId = pair.Key, BeastId = pair.Value }).ToArray()
        };

        /// <summary>옛 Version 1 JSON에는 필드가 없으므로 빈 이력과 Wolf 기본 장착으로 복원합니다.</summary>
        public static void ImportSaveData(BeastCompanionSaveData data)
        {
            Reset();
            if (data == null) return;
            foreach (string id in data.DefeatedMonsterIds ?? Array.Empty<string>())
                if (!string.IsNullOrWhiteSpace(id)) Defeated.Add(id);
            foreach (string id in data.UnlockedMonsterPetIds ?? Array.Empty<string>())
                if (IsMonsterPet(id)) Unlocked.Add(id);
            foreach (EquippedBeastSaveData item in data.EquippedBeasts ?? Array.Empty<EquippedBeastSaveData>())
                if (item != null && IsSharpshooter(item.CharacterId) && IsUnlocked(item.BeastId))
                    Equipped[item.CharacterId] = item.BeastId;
        }

        public static void Reset()
        {
            Defeated.Clear();
            Unlocked.Clear();
            Equipped.Clear();
        }
    }
}

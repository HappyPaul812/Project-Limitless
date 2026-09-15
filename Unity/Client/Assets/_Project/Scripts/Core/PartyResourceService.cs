using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectLimitless.Core
{
    /// <summary>저장 파일에 기록하는 파티원 한 명의 전투 밖 현재 HP/MP입니다.</summary>
    [Serializable]
    public sealed class PartyMemberResourceSaveData
    {
        public string CharacterId = string.Empty;
        public int CurrentHp;
        public int CurrentMp;
    }

    /// <summary>Battle과 향후 World UI가 같은 현재 자원을 읽을 때 사용하는 읽기 전용 값입니다.</summary>
    public readonly struct PartyMemberResourceSnapshot
    {
        public PartyMemberResourceSnapshot(string characterId, int currentHp, int currentMp, int maxHp = 0, int maxMp = 0)
        { CharacterId = characterId; CurrentHp = currentHp; CurrentMp = currentMp; MaxHp = maxHp; MaxMp = maxMp; }

        public string CharacterId { get; }
        public int CurrentHp { get; }
        public int CurrentMp { get; }
        public int MaxHp { get; }
        public int MaxMp { get; }
    }

    /// <summary>
    /// 전투가 끝나도 남아야 하는 HP/MP만 stable character ID별로 관리합니다. 독·화상·도발·쿨타임 같은
    /// 전투 상태는 Combatant와 각 Runtime에만 있으므로 이 저장소나 SaveData에 들어오지 않습니다.
    /// </summary>
    public static class PartyResourceService
    {
        public const string PlayerCharacterId = "player";

        private sealed class RuntimeState
        {
            public int CurrentHp;
            public int CurrentMp;
            public int MaxHp;
            public int MaxMp;
        }

        private static readonly Dictionary<string, RuntimeState> States =
            new Dictionary<string, RuntimeState>(StringComparer.Ordinal);

        /// <summary>
        /// Battle 생성 시 현재 최대치에 맞춰 저장값을 안전하게 읽습니다. 값이 없는 신규 캐릭터나 구버전
        /// Save는 최대치로 시작하고, 오래된 수치가 새 최대치를 넘으면 여기서 Clamp합니다.
        /// </summary>
        public static PartyMemberResourceSnapshot ResolveForBattle(string characterId, int maxHp, int maxMp)
        {
            ValidateCharacterId(characterId);
            maxHp = Math.Max(1, maxHp);
            maxMp = Math.Max(0, maxMp);
            if (!States.TryGetValue(characterId, out RuntimeState state))
            {
                state = new RuntimeState { CurrentHp = maxHp, CurrentMp = maxMp };
                States.Add(characterId, state);
            }

            state.MaxHp = maxHp;
            state.MaxMp = maxMp;
            state.CurrentHp = Math.Max(1, Math.Min(maxHp, state.CurrentHp));
            state.CurrentMp = Math.Max(0, Math.Min(maxMp, state.CurrentMp));
            return CreateSnapshot(characterId, state);
        }

        /// <summary>
        /// Battle Runtime에서 변한 값을 Session 쪽에 다시 씁니다. Combatant는 Scene과 함께 폐기되므로
        /// 이 경계를 거치지 않으면 다음 전투가 이전 피해와 MP 소비를 알 수 없습니다.
        /// </summary>
        public static void RecordBattleResult(string characterId, int currentHp, int currentMp, int maxHp, int maxMp)
        {
            ValidateCharacterId(characterId);
            maxHp = Math.Max(1, maxHp);
            maxMp = Math.Max(0, maxMp);
            States[characterId] = new RuntimeState
            {
                // 전투불능은 필드 상태로 지속하지 않으며 다음 전투가 0 HP로 시작하지 않도록 1을 보장합니다.
                CurrentHp = Math.Max(1, Math.Min(maxHp, currentHp)),
                CurrentMp = Math.Max(0, Math.Min(maxMp, currentMp)),
                MaxHp = maxHp,
                MaxMp = maxMp
            };
        }

        /// <summary>실제로 레벨이 오른 한 캐릭터만 최종 레벨의 새 최대치까지 완전히 회복합니다.</summary>
        public static void HealFully(string characterId, int newMaxHp, int newMaxMp)
        {
            ValidateCharacterId(characterId);
            newMaxHp = Math.Max(1, newMaxHp);
            newMaxMp = Math.Max(0, newMaxMp);
            States[characterId] = new RuntimeState
            { CurrentHp = newMaxHp, CurrentMp = newMaxMp, MaxHp = newMaxHp, MaxMp = newMaxMp };
        }

        /// <summary>
        /// 패배 복귀와 향후 마을 치유소가 사용할 파티 공통 완전 회복 API입니다. Battle 진입 때 등록된
        /// 각 파티원의 최대치를 사용하므로 치유소가 GameSessionData 내부 값을 직접 반복 수정할 필요가 없습니다.
        /// </summary>
        public static void HealPartyFully()
        {
            foreach (RuntimeState state in States.Values)
            {
                if (state.MaxHp <= 0) continue;
                state.CurrentHp = state.MaxHp;
                state.CurrentMp = state.MaxMp;
            }
        }

        public static bool TryGet(string characterId, out PartyMemberResourceSnapshot snapshot)
        {
            if (!string.IsNullOrWhiteSpace(characterId) && States.TryGetValue(characterId, out RuntimeState state))
            {
                snapshot = CreateSnapshot(characterId, state);
                return true;
            }
            snapshot = default;
            return false;
        }

        /// <summary>Sprite와 달리 현재 HP/MP는 플레이 결과이므로 게임 재실행 뒤에도 남도록 값으로 내보냅니다.</summary>
        public static PartyMemberResourceSaveData[] ExportSaveData() => States
            .OrderBy(pair => pair.Key, StringComparer.Ordinal)
            .Select(pair => new PartyMemberResourceSaveData
            { CharacterId = pair.Key, CurrentHp = pair.Value.CurrentHp, CurrentMp = pair.Value.CurrentMp }).ToArray();

        /// <summary>
        /// 구버전 JSON에는 배열이 없어 null로 역직렬화됩니다. 빈 상태로 받아 두면 각 파티원이 첫 Battle에서
        /// 현재 최대치로 초기화되므로 기존 슬롯을 삭제하거나 별도 Sprite 같은 Unity Object를 저장할 필요가 없습니다.
        /// </summary>
        public static void ImportSaveData(IEnumerable<PartyMemberResourceSaveData> savedStates)
        {
            States.Clear();
            if (savedStates == null) return;
            foreach (PartyMemberResourceSaveData saved in savedStates)
            {
                if (saved == null || string.IsNullOrWhiteSpace(saved.CharacterId) || States.ContainsKey(saved.CharacterId)) continue;
                States.Add(saved.CharacterId, new RuntimeState
                { CurrentHp = Math.Max(1, saved.CurrentHp), CurrentMp = Math.Max(0, saved.CurrentMp) });
            }
        }

        public static void Reset() => States.Clear();

        /// <summary>필드 소비 아이템이 Battle과 같은 지속 자원을 안전하게 회복합니다.</summary>
        public static bool TryRecover(string characterId, ItemEffectType effectType, int requestedAmount, out int actualAmount)
        {
            actualAmount = 0;
            if (requestedAmount <= 0 || !States.TryGetValue(characterId ?? string.Empty, out RuntimeState state)
                || state.CurrentHp <= 0 || state.MaxHp <= 0)
                return false;

            if (effectType == ItemEffectType.RecoverHp)
            {
                actualAmount = Math.Min(requestedAmount, state.MaxHp - state.CurrentHp);
                if (actualAmount <= 0) return false;
                state.CurrentHp += actualAmount;
                return true;
            }

            if (effectType == ItemEffectType.RecoverMp)
            {
                if (state.MaxMp <= 0) return false;
                actualAmount = Math.Min(requestedAmount, state.MaxMp - state.CurrentMp);
                if (actualAmount <= 0) return false;
                state.CurrentMp += actualAmount;
                return true;
            }
            return false;
        }

        private static PartyMemberResourceSnapshot CreateSnapshot(string characterId, RuntimeState state) =>
            new PartyMemberResourceSnapshot(characterId, state.CurrentHp, state.CurrentMp, state.MaxHp, state.MaxMp);

        private static void ValidateCharacterId(string characterId)
        {
            if (string.IsNullOrWhiteSpace(characterId)) throw new ArgumentException("파티원 stable ID가 필요합니다.", nameof(characterId));
        }
    }
}

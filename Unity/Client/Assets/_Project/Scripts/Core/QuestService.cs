using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectLimitless.Core
{
    public enum QuestState { Locked, Available, Active, Completed }
    public enum NpcQuestMarkerState { None, Available, ActiveObjective, ReadyToTurnIn }

    // 저장 파일에는 Scene 오브젝트 대신 안정적인 목표 ID와 진행 횟수만 남깁니다.
    // 불러올 때 QuestDefinition의 현재 목표 목록에 다시 맞춰 오래된 저장도 읽을 수 있습니다.
    [Serializable]
    public sealed class QuestObjectiveProgressData
    {
        public string ObjectiveId = string.Empty;
        public int CurrentCount;
    }

    // 진행 중인 퀘스트 한 개의 목표별 스냅샷입니다. 완료한 퀘스트는 아래 CompletedQuestIds로 옮깁니다.
    [Serializable]
    public sealed class ActiveQuestSaveData
    {
        public string QuestId = string.Empty;
        public QuestObjectiveProgressData[] Objectives = Array.Empty<QuestObjectiveProgressData>();
    }

    // 메인·서브 퀘스트의 진행 상태와 화면에서 추적할 퀘스트 ID를 함께 저장합니다.
    [Serializable]
    public sealed class QuestProgressSaveData
    {
        public ActiveQuestSaveData[] ActiveQuests = Array.Empty<ActiveQuestSaveData>();
        public string[] CompletedQuestIds = Array.Empty<string>();
        public string TrackedQuestId = string.Empty;
    }

    /// <summary>실행 중인 한 퀘스트의 목표별 횟수를 보관합니다.</summary>
    public sealed class QuestRuntimeState
    {
        private readonly int[] objectiveCounts;
        public QuestRuntimeState(QuestDefinition definition, int[] counts = null)
        {
            Definition = definition;
            objectiveCounts = new int[definition.Objectives.Count];
            if (counts != null) Array.Copy(counts, objectiveCounts, Math.Min(counts.Length, objectiveCounts.Length));
        }
        public QuestDefinition Definition { get; }
        // 보유 재료는 누적 횟수가 아닙니다. 판매·납품·로드 후에도 현재 인벤토리가 정본입니다.
        public IReadOnlyList<int> ObjectiveCounts => Definition.IsItemDelivery
            ? Definition.Objectives.Select((x, i) => CountAt(i)).ToArray() : objectiveCounts;
        private int CountAt(int index)
        {
            var objective = Definition.Objectives[index];
            return objective.ObjectiveType == QuestObjectiveType.CollectItem
                ? Math.Min(objective.RequiredCount, InventoryService.GetItemCount(objective.TargetId)) : objectiveCounts[index];
        }
        public int CurrentObjectiveIndex
        {
            get
            {
                for (int i = 0; i < objectiveCounts.Length; i++)
                    if (CountAt(i) < Definition.Objectives[i].RequiredCount) return i;
                return objectiveCounts.Length;
            }
        }
        public QuestObjectiveDefinition CurrentObjective => CurrentObjectiveIndex < Definition.Objectives.Count
            ? Definition.Objectives[CurrentObjectiveIndex] : null;
        internal bool Notify(QuestObjectiveType type, string targetId)
        {
            // 이전 목표를 다시 조사해도 진행되지 않도록 현재 순서의 목표 하나만 비교합니다.
            // 대화가 끝나거나 전투 승리가 확정된 호출자가 사건을 알릴 때만 횟수를 올립니다.
            int index = CurrentObjectiveIndex;
            if (index >= Definition.Objectives.Count) return false;
            QuestObjectiveDefinition objective = Definition.Objectives[index];
            if (objective.ObjectiveType != type || !string.Equals(objective.TargetId, targetId, StringComparison.Ordinal)) return false;
            objectiveCounts[index] = Math.Min(objective.RequiredCount, objectiveCounts[index] + 1);
            return true;
        }
    }

    /// <summary>
    /// 퀘스트 상태와 사건 알림만 담당합니다. Scene을 검색하지 않으며 NPC·장소·전투 시스템이
    /// stable ID를 명시적으로 알려 줄 때만 현재 순차 목표를 진행합니다.
    /// </summary>
    public static class QuestService
    {
        private static readonly Dictionary<string, QuestRuntimeState> active = new Dictionary<string, QuestRuntimeState>(StringComparer.Ordinal);
        private static readonly HashSet<string> completed = new HashSet<string>(StringComparer.Ordinal);
        private static string trackedQuestId = string.Empty;
        public static event Action Changed;
        private static bool delivering;
        static QuestService() => InventoryService.Changed += () =>
        {
            // 납품 중간 상태는 숨기고 전체 확정 후 UI에 알립니다.
            if (!delivering && ActiveSideQuests.Any(x => x.Definition.IsItemDelivery)) Changed?.Invoke();
        };

        public static IReadOnlyCollection<QuestRuntimeState> ActiveQuests => active.Values;
        public static IEnumerable<QuestRuntimeState> ActiveSideQuests => active.Values.Where(x => x.Definition.QuestType == QuestType.Side);
        public static IEnumerable<string> CompletedQuestIds => completed;
        public static QuestRuntimeState ActiveMainQuest => active.Values.FirstOrDefault(x => x.Definition.QuestType == QuestType.Main);
        public static string TrackedQuestId => trackedQuestId;

        public static QuestState GetState(string questId)
        {
            if (completed.Contains(questId ?? string.Empty)) return QuestState.Completed;
            if (active.ContainsKey(questId ?? string.Empty)) return QuestState.Active;
            if (!QuestCatalog.TryGet(questId, out QuestDefinition definition)) return QuestState.Locked;
            return definition.PrerequisiteQuestIds.All(completed.Contains) ? QuestState.Available : QuestState.Locked;
        }

        public static bool TryStart(string questId)
        {
            if (GetState(questId) != QuestState.Available || !QuestCatalog.TryGet(questId, out QuestDefinition definition)
                || definition.Objectives.Count == 0) return false;
            if (definition.QuestType == QuestType.Main && ActiveMainQuest != null) return false;
            active.Add(definition.QuestId, new QuestRuntimeState(definition));
            if (string.IsNullOrEmpty(trackedQuestId) || definition.QuestType == QuestType.Main) trackedQuestId = definition.QuestId;
            if (definition.QuestType == QuestType.Side && GameSaveService.CurrentSlotIndex > 0) GameSaveService.SaveCurrentSession();
            Changed?.Invoke();
            return true;
        }

        public static bool TryTrack(string questId)
        {
            if (!active.ContainsKey(questId ?? string.Empty)) return false;
            trackedQuestId = questId;
            if (GameSaveService.CurrentSlotIndex > 0) GameSaveService.SaveCurrentSession();
            Changed?.Invoke();
            return true;
        }

        public static QuestRuntimeState GetTrackedQuest()
        {
            if (active.TryGetValue(trackedQuestId, out QuestRuntimeState selected)) return selected;
            QuestRuntimeState main = ActiveMainQuest;
            return main ?? active.Values.FirstOrDefault();
        }

        /// <summary>
        /// NPC 이름이나 GameObject 이름을 보지 않고 stable ID와 퀘스트 데이터만 비교합니다.
        /// Presenter가 상태 변경 이벤트 때만 호출하므로 매 프레임 Catalog 전체를 훑지 않습니다.
        /// </summary>
        public static NpcQuestMarkerState GetNpcMarkerState(string npcId)
        {
            if (string.IsNullOrWhiteSpace(npcId)) return NpcQuestMarkerState.None;
            if (active.Values.Any(x => (x.Definition.IsItemDelivery ? CanDeliverItems(x.Definition) : x.CurrentObjective == null)
                && string.Equals(x.Definition.TurnInNpcId, npcId, StringComparison.Ordinal)))
                return NpcQuestMarkerState.ReadyToTurnIn;
            if (active.Values.Any(x => x.CurrentObjective?.ObjectiveType == QuestObjectiveType.TalkToNpc
                && string.Equals(x.CurrentObjective.TargetId, npcId, StringComparison.Ordinal)))
                return NpcQuestMarkerState.ActiveObjective;
            if (active.Values.Any(x => x.Definition.IsItemDelivery && x.Definition.TurnInNpcId == npcId))
                return NpcQuestMarkerState.ActiveObjective;
            if (QuestCatalog.All.Any(x => GetState(x.QuestId) == QuestState.Available
                && string.Equals(x.StartNpcId, npcId, StringComparison.Ordinal)))
                return NpcQuestMarkerState.Available;
            return NpcQuestMarkerState.None;
        }

        public static void NotifyNpcTalked(string npcId) => Notify(QuestObjectiveType.TalkToNpc, npcId);
        public static void NotifyLocationReached(string locationId) => Notify(QuestObjectiveType.ReachLocation, locationId);
        public static void NotifyEncounterWon(string encounterId) => Notify(QuestObjectiveType.DefeatEncounter, encounterId);
        public static void NotifyInteraction(string interactionId) => Notify(QuestObjectiveType.Interact, interactionId);
        public static void NotifySignal(string signalId) => Notify(QuestObjectiveType.GenericSignal, signalId);

        private static void Notify(QuestObjectiveType type, string targetId)
        {
            if (string.IsNullOrWhiteSpace(targetId)) return;
            foreach (QuestRuntimeState state in active.Values.ToArray())
            {
                // 재료 의뢰는 일반 대화/전투 알림으로 완료하지 않고 명시적인 납품만 허용합니다.
                if (state.Definition.IsItemDelivery) continue;
                if (state.CurrentObjective == null
                    && type == QuestObjectiveType.TalkToNpc
                    && string.Equals(state.Definition.TurnInNpcId, targetId, StringComparison.Ordinal))
                {
                    Complete(state);
                    continue;
                }
                if (!state.Notify(type, targetId)) continue;
                if (state.CurrentObjective == null && string.IsNullOrEmpty(state.Definition.TurnInNpcId)) Complete(state);
                else Changed?.Invoke();
            }
        }

        /// <summary>동일 아이템을 요구하는 목표도 합산해 서로 같은 재료를 두 번 인정하지 않습니다.</summary>
        private static Dictionary<string, int> DeliveryRequirements(QuestDefinition definition)
        {
            var result = new Dictionary<string, int>(StringComparer.Ordinal);
            foreach (var objective in definition.Objectives)
            {
                result.TryGetValue(objective.TargetId, out int count);
                result[objective.TargetId] = checked(count + objective.RequiredCount);
            }
            return result;
        }

        public static bool CanDeliverItems(QuestDefinition definition) => definition != null && definition.IsItemDelivery
            && DeliveryRequirements(definition).All(x => ItemCatalog.TryGet(x.Key, out _) && InventoryService.HasItem(x.Key, x.Value));

        /// <summary>
        /// 납품 한 번의 재료 소비·보상·완료 기록을 동기 확정합니다. 먼저 전체 조건을 검사하고
        /// 처리 중 재진입을 막아 같은 클릭/대화 알림이 재료와 보상을 중복 처리하지 못하게 합니다.
        /// 메인 퀘스트의 Complete 경로는 그대로 유지합니다.
        /// </summary>
        public static bool TryDeliverSideQuest(string questId, string npcId)
        {
            if (delivering || !active.TryGetValue(questId ?? string.Empty, out QuestRuntimeState state)
                || !state.Definition.IsItemDelivery || state.Definition.TurnInNpcId != npcId
                || !CanDeliverItems(state.Definition)) return false;
            var reward = state.Definition.Reward;
            if (reward == null || reward.Experience < 0 || reward.Currency < 0
                || EconomyService.GetCurrency() > int.MaxValue - reward.Currency) return false;
            var requirements = DeliveryRequirements(state.Definition);
            // TryApply는 동일 보상ID를 각각 검사합니다. 서브 납품은 먼저 합산하여 부분 지급 가능성도 차단합니다.
            var rewardCounts = new Dictionary<string, long>(StringComparer.Ordinal);
            foreach (var item in reward.Items ?? Array.Empty<ItemReward>())
            {
                if (item == null || item.Count <= 0 || !ItemCatalog.TryGet(item.ItemId, out _)) return false;
                rewardCounts.TryGetValue(item.ItemId, out long count); rewardCounts[item.ItemId] = count + item.Count;
            }
            foreach (var pair in rewardCounts)
            {
                ItemCatalog.TryGet(pair.Key, out ItemDefinition item);
                requirements.TryGetValue(pair.Key, out int removed);
                if ((long)InventoryService.GetItemCount(pair.Key) - removed + pair.Value > item.MaxStack) return false;
            }
            InventoryEntry[] before = InventoryService.ExportSaveData();
            delivering = true;
            try
            {
                foreach (var pair in requirements)
                    if (!InventoryService.TryRemoveItem(pair.Key, pair.Value)) { InventoryService.ImportSaveData(before); return false; }
                int previousLevel = GameSessionData.Level;
                if (!reward.TryApply()) { InventoryService.ImportSaveData(before); return false; }
                active.Remove(questId); completed.Add(questId);
                if (trackedQuestId == questId) trackedQuestId = ActiveMainQuest?.Definition.QuestId ?? string.Empty;
                if (GameSessionData.Level > previousLevel)
                {
                    CharacterGrowthStats growth = CharacterGrowthCalculator.Calculate(GameSessionData.SelectedJobId, GameSessionData.Level);
                    int hp = CharacterGrowthCalculator.CalculateMaxHp(GameSessionData.SelectedJobId, growth);
                    if (GameSessionData.SelectedJobId == "sharpshooter") hp = BeastCompanionService.GetEffectiveMaxHp(hp,
                        BeastCompanionService.GetEquippedId(PartyResourceService.PlayerCharacterId));
                    PartyResourceService.HealFully(PartyResourceService.PlayerCharacterId, hp,
                        CharacterGrowthCalculator.CalculateMaxMp(GameSessionData.SelectedJobId, growth));
                }
                if (GameSaveService.CurrentSlotIndex > 0) GameSaveService.SaveCurrentSession();
            }
            finally { delivering = false; }
            Changed?.Invoke();
            return true;
        }

        /// <summary>NPC/QuestLog/HUD가 동일한 수량과 납품 안내 문구를 사용합니다.</summary>
        public static string BuildItemProgress(QuestRuntimeState state)
        {
            if (state == null || !state.Definition.IsItemDelivery) return string.Empty;
            var lines = state.Definition.Objectives.Select(x =>
            {
                ItemCatalog.TryGet(x.TargetId, out ItemDefinition item);
                return $"{item?.DisplayName ?? x.TargetId} {Math.Min(x.RequiredCount, InventoryService.GetItemCount(x.TargetId))}/{x.RequiredCount}";
            });
            return string.Join("\n", lines) + (CanDeliverItems(state.Definition)
                ? "\n필요한 재료를 모두 모았습니다. 의뢰한 NPC에게 전달하세요." : "\n재료를 모아 의뢰한 NPC에게 전달하세요.");
        }

        private static void Complete(QuestRuntimeState state)
        {
            // 보상을 먼저 원자적으로 지급해야 인벤토리 부족 시 완료 기록만 남는 손실을 피할 수 있습니다.
            int previousLevel = GameSessionData.Level;
            if (!state.Definition.Reward.TryApply()) return;
            if (GameSessionData.Level > previousLevel)
            {
                CharacterGrowthStats growth = CharacterGrowthCalculator.Calculate(GameSessionData.SelectedJobId, GameSessionData.Level);
                int maxHp = CharacterGrowthCalculator.CalculateMaxHp(GameSessionData.SelectedJobId, growth);
                if (GameSessionData.SelectedJobId == "sharpshooter")
                    maxHp = BeastCompanionService.GetEffectiveMaxHp(maxHp,
                        BeastCompanionService.GetEquippedId(PartyResourceService.PlayerCharacterId));
                PartyResourceService.HealFully(PartyResourceService.PlayerCharacterId,
                    maxHp,
                    CharacterGrowthCalculator.CalculateMaxMp(GameSessionData.SelectedJobId, growth));
            }
            active.Remove(state.Definition.QuestId);
            completed.Add(state.Definition.QuestId);
            // 완료 기록·영구 해금·자동 편성을 같은 저장 경계 안에서 확정합니다.
            if (state.Definition.QuestId == "main_09_reunion_in_silence")
                CompanionRosterService.UnlockPaul(GameSessionData.SelectedJobId);
            if (state.Definition.QuestId == "main_15_burning_traces")
                CompanionRosterService.UnlockSerin();
            if (trackedQuestId == state.Definition.QuestId) trackedQuestId = string.Empty;
            if (GameSaveService.CurrentSlotIndex > 0) GameSaveService.SaveCurrentSession();
            Changed?.Invoke();
        }

        public static QuestProgressSaveData ExportSaveData()
        {
            return new QuestProgressSaveData
            {
                ActiveQuests = active.Values.Select(state => new ActiveQuestSaveData
                {
                    QuestId = state.Definition.QuestId,
                    Objectives = state.Definition.Objectives.Select((objective, index) => new QuestObjectiveProgressData
                    { ObjectiveId = objective.ObjectiveId, CurrentCount = state.ObjectiveCounts[index] }).ToArray()
                }).ToArray(),
                CompletedQuestIds = completed.ToArray(),
                TrackedQuestId = trackedQuestId
            };
        }

        public static void ImportSaveData(QuestProgressSaveData data)
        {
            active.Clear(); completed.Clear(); trackedQuestId = string.Empty;
            if (data == null) { Changed?.Invoke(); return; }
            if (data.CompletedQuestIds != null)
                foreach (string questId in data.CompletedQuestIds)
                    if (!string.IsNullOrWhiteSpace(questId)) completed.Add(questId);
            if (data.ActiveQuests != null)
            {
                foreach (ActiveQuestSaveData saved in data.ActiveQuests)
                {
                    if (saved == null || completed.Contains(saved.QuestId) || !QuestCatalog.TryGet(saved.QuestId, out QuestDefinition definition)) continue;
                    int[] counts = new int[definition.Objectives.Count];
                    for (int i = 0; i < definition.Objectives.Count; i++)
                    {
                        QuestObjectiveProgressData progress = saved.Objectives?.FirstOrDefault(x => x != null
                            && string.Equals(x.ObjectiveId, definition.Objectives[i].ObjectiveId, StringComparison.Ordinal));
                        counts[i] = Math.Min(definition.Objectives[i].RequiredCount, Math.Max(0, progress?.CurrentCount ?? 0));
                    }
                    if (definition.QuestType == QuestType.Main && active.Values.Any(x => x.Definition.QuestType == QuestType.Main)) continue;
                    active[definition.QuestId] = new QuestRuntimeState(definition, counts);
                }
            }
            if (!string.IsNullOrEmpty(data.TrackedQuestId) && active.ContainsKey(data.TrackedQuestId)) trackedQuestId = data.TrackedQuestId;
            Changed?.Invoke();
        }

        public static void Reset()
        { active.Clear(); completed.Clear(); trackedQuestId = string.Empty; Changed?.Invoke(); }
    }
}

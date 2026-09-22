using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectLimitless.Core
{
    public enum QuestState { Locked, Available, Active, Completed }
    public enum NpcQuestMarkerState { None, Available, ActiveObjective, ReadyToTurnIn }

    [Serializable]
    public sealed class QuestObjectiveProgressData
    {
        public string ObjectiveId = string.Empty;
        public int CurrentCount;
    }

    [Serializable]
    public sealed class ActiveQuestSaveData
    {
        public string QuestId = string.Empty;
        public QuestObjectiveProgressData[] Objectives = Array.Empty<QuestObjectiveProgressData>();
    }

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
        public IReadOnlyList<int> ObjectiveCounts => objectiveCounts;
        public int CurrentObjectiveIndex
        {
            get
            {
                for (int i = 0; i < objectiveCounts.Length; i++)
                    if (objectiveCounts[i] < Definition.Objectives[i].RequiredCount) return i;
                return objectiveCounts.Length;
            }
        }
        public QuestObjectiveDefinition CurrentObjective => CurrentObjectiveIndex < Definition.Objectives.Count
            ? Definition.Objectives[CurrentObjectiveIndex] : null;
        internal bool Notify(QuestObjectiveType type, string targetId)
        {
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
            if (active.Values.Any(x => x.CurrentObjective == null
                && string.Equals(x.Definition.TurnInNpcId, npcId, StringComparison.Ordinal)))
                return NpcQuestMarkerState.ReadyToTurnIn;
            if (active.Values.Any(x => x.CurrentObjective?.ObjectiveType == QuestObjectiveType.TalkToNpc
                && string.Equals(x.CurrentObjective.TargetId, npcId, StringComparison.Ordinal)))
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

        private static void Complete(QuestRuntimeState state)
        {
            // 보상을 먼저 원자적으로 지급해야 인벤토리 부족 시 완료 기록만 남는 손실을 피할 수 있습니다.
            int previousLevel = GameSessionData.Level;
            if (!state.Definition.Reward.TryApply()) return;
            if (GameSessionData.Level > previousLevel)
            {
                CharacterGrowthStats growth = CharacterGrowthCalculator.Calculate(GameSessionData.SelectedJobId, GameSessionData.Level);
                PartyResourceService.HealFully(PartyResourceService.PlayerCharacterId,
                    CharacterGrowthCalculator.CalculateMaxHp(GameSessionData.SelectedJobId, growth),
                    CharacterGrowthCalculator.CalculateMaxMp(GameSessionData.SelectedJobId, growth));
            }
            active.Remove(state.Definition.QuestId);
            completed.Add(state.Definition.QuestId);
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

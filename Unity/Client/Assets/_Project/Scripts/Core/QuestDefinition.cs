using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectLimitless.Core
{
    public enum QuestType { Main, Side }
    public enum QuestObjectiveType { TalkToNpc, ReachLocation, DefeatEncounter, Interact, GenericSignal }

    /// <summary>한 목표가 기다리는 사건과 화면에 보여 줄 문구를 함께 담는 데이터입니다.</summary>
    [Serializable]
    public sealed class QuestObjectiveDefinition
    {
        [SerializeField] private string objectiveId = string.Empty;
        [SerializeField] private string description = string.Empty;
        [SerializeField] private QuestObjectiveType objectiveType;
        [SerializeField] private string targetId = string.Empty;
        [Min(1), SerializeField] private int requiredCount = 1;

        public string ObjectiveId => objectiveId;
        public string Description => description;
        public QuestObjectiveType ObjectiveType => objectiveType;
        public string TargetId => targetId;
        public int RequiredCount => Math.Max(1, requiredCount);

#if UNITY_EDITOR
        public void ConfigureForAudit(string id, string text, QuestObjectiveType type, string target, int count = 1)
        { objectiveId = id; description = text; objectiveType = type; targetId = target; requiredCount = Math.Max(1, count); }
#endif
    }

    /// <summary>
    /// 실제 대사나 Scene 오브젝트와 분리된 퀘스트 원본 데이터입니다. 저장에는 이 Asset이 아니라
    /// 바뀌지 않는 QuestId와 ObjectiveId만 기록해 표시 이름을 고쳐도 진행을 유지합니다.
    /// </summary>
    [CreateAssetMenu(menuName = "Project Limitless/Quest Definition")]
    public sealed class QuestDefinition : ScriptableObject
    {
        [SerializeField] private string questId = string.Empty;
        [SerializeField] private string displayName = string.Empty;
        [TextArea, SerializeField] private string description = string.Empty;
        [SerializeField] private QuestType questType;
        [SerializeField] private QuestObjectiveDefinition[] objectives = Array.Empty<QuestObjectiveDefinition>();
        [SerializeField] private RewardBundle reward = new RewardBundle();
        [SerializeField] private string[] prerequisiteQuestIds = Array.Empty<string>();
        [SerializeField] private string nextMainQuestId = string.Empty;
        [SerializeField] private string startNpcId = string.Empty;
        [SerializeField] private string turnInNpcId = string.Empty;

        public string QuestId => questId;
        public string DisplayName => displayName;
        public string Description => description;
        public QuestType QuestType => questType;
        public IReadOnlyList<QuestObjectiveDefinition> Objectives => objectives;
        public RewardBundle Reward => reward;
        public IReadOnlyList<string> PrerequisiteQuestIds => prerequisiteQuestIds;
        public string NextMainQuestId => nextMainQuestId;
        public string StartNpcId => startNpcId;
        public string TurnInNpcId => turnInNpcId;

#if UNITY_EDITOR
        public void ConfigureForAudit(string id, string title, QuestType type, QuestObjectiveDefinition[] steps,
            RewardBundle completionReward = null, string[] prerequisites = null, string nextQuestId = "")
        {
            questId = id; displayName = title; questType = type;
            objectives = steps ?? Array.Empty<QuestObjectiveDefinition>();
            reward = completionReward ?? new RewardBundle();
            prerequisiteQuestIds = prerequisites ?? Array.Empty<string>();
            nextMainQuestId = nextQuestId ?? string.Empty;
        }

        public void ConfigureNpcFlow(string questStartNpcId, string questTurnInNpcId = "")
        { startNpcId = questStartNpcId ?? string.Empty; turnInNpcId = questTurnInNpcId ?? string.Empty; }

        public void ConfigureDescription(string questDescription) => description = questDescription ?? string.Empty;
#endif
    }
}

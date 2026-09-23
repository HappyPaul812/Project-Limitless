#if UNITY_EDITOR
using ProjectLimitless.Core;
using ProjectLimitless.Battle;
using ProjectLimitless.World;
using UnityEditor;
using UnityEngine;

namespace ProjectLimitless.Editor
{
    /// <summary>Main09와 동료 연결 데이터만 생성하며 기존 Scene·Sprite·몬스터는 건드리지 않습니다.</summary>
    public static class MainQuest09ContentEditor
    {
        public static void Build()
        {
            const string folder = "Assets/_Project/Resources/CompanionDefinitions";
            if (!AssetDatabase.IsValidFolder(folder)) AssetDatabase.CreateFolder("Assets/_Project/Resources", "CompanionDefinitions");
            Companion("01_Taeon", CompanionRosterService.TaeonId, "태온", "guardian", "수호자", PathCombatTraitRuntime.IntellectualPathId, "지적의 길", FormationRow.Front, 132, 10, 9);
            Companion("02_Miel", CompanionRosterService.MielId, "미엘", "healer", "치유사", PathCombatTraitRuntime.EmotionalScarPathId, "마음의 상처", FormationRow.Rear, 104, 8, 12);
            Companion("03_Paul", CompanionRosterService.PaulId, "폴", "mage", "마도사", PathCombatTraitRuntime.MobilityPathId, "지체의 길", FormationRow.Rear, 96, 14, 10);
            const string path = "Assets/_Project/Resources/QuestDefinitions/Main09_ReunionInSilence.asset";
            var quest = AssetDatabase.LoadAssetAtPath<QuestDefinition>(path);
            if (quest == null) { quest = ScriptableObject.CreateInstance<QuestDefinition>(); AssetDatabase.CreateAsset(quest, path); }
            quest.ConfigureForAudit(MainQuest09FieldFlow.QuestId, "침묵 속의 재회", QuestType.Main, new[]
            {
                Step("follow_blue_trace", "푸른 빛이 사라진 방향을 따라가세요.", MainQuest09FieldFlow.BlueTraceId),
                Step("inspect_paul_tracks", "숲길에 남은 바퀴 자국을 조사하세요.", MainQuest09FieldFlow.TracksId),
                Step("reunite_with_paul", "앞쪽에서 들려오는 목소리를 확인하세요.", CompanionRosterService.PaulId, QuestObjectiveType.TalkToNpc),
                Step("exchange_information", "폴과 서로 확인한 정보를 정리하세요.", CompanionRosterService.PaulId, QuestObjectiveType.TalkToNpc),
                Step("inspect_structural_trace", "폴이 발견한 비정상적인 구조물을 조사하세요.", MainQuest09FieldFlow.StructureId),
                Step("decide_to_continue_together", "폴과 다음 조사에 대해 이야기하세요.", CompanionRosterService.PaulId, QuestObjectiveType.TalkToNpc)
            }, new RewardBundle { Experience = 35, Currency = 35, Items = System.Array.Empty<ItemReward>() },
                new[] { MainQuest08FieldFlow.QuestId });
            quest.ConfigureDescription("푸른 빛의 흔적을 따라 폴과 재회하고, 각자 확인한 사실을 모아 숲 안쪽의 가공된 석재 흔적을 조사한다.");
            quest.ConfigureNpcFlow(""); EditorUtility.SetDirty(quest);
            AssetDatabase.SaveAssets(); QuestCatalog.ReloadForAudit();
        }

        private static QuestObjectiveDefinition Step(string id, string description, string target, QuestObjectiveType type = QuestObjectiveType.Interact)
        { var step = new QuestObjectiveDefinition(); step.ConfigureForAudit(id, description, type, target); return step; }

        private static void Companion(string asset, string id, string name, string job, string jobName, string path, string pathName, FormationRow row, int hp, int attack, int agility)
        {
            string assetPath = "Assets/_Project/Resources/CompanionDefinitions/" + asset + ".asset";
            var value = AssetDatabase.LoadAssetAtPath<CompanionDefinition>(assetPath);
            if (value == null) { value = ScriptableObject.CreateInstance<CompanionDefinition>(); AssetDatabase.CreateAsset(value, assetPath); }
            value.CharacterId = id; value.DisplayName = name; value.JobId = job; value.JobName = jobName;
            value.PathId = path; value.PathName = pathName; value.DefaultRow = row;
            value.MaxHp = hp; value.Attack = attack; value.Agility = agility;
            value.BasicRange = job == "guardian" ? TargetRangeType.MeleePhysical : TargetRangeType.Magic;
            EditorUtility.SetDirty(value);
        }
    }
}
#endif

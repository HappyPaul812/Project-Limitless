#if UNITY_EDITOR
using ProjectLimitless.Core;
using ProjectLimitless.NPC;
using ProjectLimitless.World;
using UnityEditor;
using UnityEngine;

namespace ProjectLimitless.Editor
{
    public static class MainQuestContentEditor
    {
        private const string Folder = "Assets/_Project/Resources/QuestDefinitions";

        [MenuItem("Project Limitless/Quest/Create Main Quest 01 Content")]
        public static void Create()
        {
            EnsureFolder("Assets/_Project/Resources");
            EnsureFolder(Folder);

            QuestObjectiveDefinition representative = Objective("talk_representative", "주민 대표와 대화하세요.", MainQuest01NpcFlow.RepresentativeId);
            QuestObjectiveDefinition guard = Objective("talk_south_gate_guard", "남문 경비병과 대화하세요.", MainQuest01NpcFlow.GuardId);
            QuestDefinition main01 = LoadOrCreate("Main01_CallReaches");
            main01.ConfigureForAudit(MainQuest01NpcFlow.QuestId, "부름이 닿은 곳", QuestType.Main,
                new[] { representative, guard }, new RewardBundle { Experience = 0, Currency = 0, Items = System.Array.Empty<ItemReward>() },
                System.Array.Empty<string>(), "main_02_grassland_anomaly");
            main01.ConfigureNpcFlow(MainQuest01NpcFlow.RepresentativeId);
            main01.ConfigureDescription("시작 마을의 주민 대표와 남문 경비병에게 최근 초원의 몬스터들이 마을 가까이 나타나는 상황을 확인한다.");

            QuestDefinition main02 = LoadOrCreate("Main02_GrasslandAnomaly");
            main02.ConfigureForAudit("main_02_grassland_anomaly", "초원의 이상", QuestType.Main,
                new[]
                {
                    Objective("reach_investigation_area", "초원 안쪽의 조사 지점으로 이동하세요.", QuestObjectiveType.ReachLocation, MainQuest02FieldFlow.InvestigationAreaId),
                    Objective("win_investigation_encounter", "조사 지점 주변의 몬스터 무리를 물리치세요.", QuestObjectiveType.DefeatEncounter, MainQuest02FieldFlow.EncounterId),
                    Objective("inspect_tracks", "전투 지점 너머의 흔적을 조사하세요.", QuestObjectiveType.Interact, MainQuest02FieldFlow.TracksId)
                }, new RewardBundle { Experience = 0, Currency = 0, Items = System.Array.Empty<ItemReward>() },
                new[] { MainQuest01NpcFlow.QuestId }, "main_03_unfamiliar_companion");
            main02.ConfigureNpcFlow(string.Empty);
            main02.ConfigureDescription("최근 초원의 몬스터들이 마을 가까이까지 내려오고 있다. 남문 경비병에게 들은 상황을 바탕으로 초원 안쪽에서 원인을 조사한다.");

            QuestDefinition main03 = LoadOrCreate("Main03_UnfamiliarCompanion");
            main03.ConfigureForAudit("main_03_unfamiliar_companion", "낯선 동행", QuestType.Main,
                System.Array.Empty<QuestObjectiveDefinition>(), new RewardBundle(), new[] { MainQuest02FieldFlow.QuestId });
            main03.ConfigureNpcFlow(string.Empty);

            EditorUtility.SetDirty(main01);
            EditorUtility.SetDirty(main02);
            EditorUtility.SetDirty(main03);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Main 01·02 퀘스트 데이터와 Quest Log용 설명·목표 문구가 준비되었습니다.");
        }

        private static QuestObjectiveDefinition Objective(string id, string text, string target)
            => Objective(id, text, QuestObjectiveType.TalkToNpc, target);

        private static QuestObjectiveDefinition Objective(string id, string text, QuestObjectiveType type, string target)
        { var objective = new QuestObjectiveDefinition(); objective.ConfigureForAudit(id, text, type, target); return objective; }

        private static QuestDefinition LoadOrCreate(string assetName)
        {
            string path = $"{Folder}/{assetName}.asset";
            QuestDefinition definition = AssetDatabase.LoadAssetAtPath<QuestDefinition>(path);
            if (definition != null) return definition;
            definition = ScriptableObject.CreateInstance<QuestDefinition>();
            AssetDatabase.CreateAsset(definition, path);
            return definition;
        }

        private static void EnsureFolder(string path)
        {
            if (AssetDatabase.IsValidFolder(path)) return;
            int split = path.LastIndexOf('/');
            AssetDatabase.CreateFolder(path.Substring(0, split), path.Substring(split + 1));
        }
    }
}
#endif

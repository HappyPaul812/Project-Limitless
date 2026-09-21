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
                new[]
                {
                    Objective("reach_taeon_meeting", "초원 안쪽의 새로운 흔적을 따라가세요.", QuestObjectiveType.ReachLocation, MainQuest03FieldFlow.MeetingLocationId),
                    Objective("talk_to_taeon_first", "태온과 대화하세요.", QuestObjectiveType.TalkToNpc, MainQuest03FieldFlow.TaeonId),
                    Objective("win_taeon_encounter", "태온과 함께 다가오는 몬스터 무리를 물리치세요.", QuestObjectiveType.DefeatEncounter, MainQuest03FieldFlow.EncounterId),
                    Objective("talk_to_taeon_after_battle", "전투가 끝난 뒤 태온과 대화하세요.", QuestObjectiveType.TalkToNpc, MainQuest03FieldFlow.TaeonId),
                    Objective("reach_next_clue", "태온과 함께 초원 안쪽으로 이동하세요.", QuestObjectiveType.ReachLocation, MainQuest03FieldFlow.NextClueLocationId)
                }, new RewardBundle { Experience = 0, Currency = 0, Items = System.Array.Empty<ItemReward>() },
                new[] { MainQuest02FieldFlow.QuestId }, MainQuest03FieldFlow.NextQuestId);
            main03.ConfigureNpcFlow(string.Empty);
            main03.ConfigureDescription("초원 안쪽에서 몬스터의 움직임을 관찰하던 태온과 만나 서로의 단서를 합치고, 함께 다음 조사 지점으로 이동한다.");

            QuestDefinition main04 = LoadOrCreate("Main04_ThreePeople");
            main04.ConfigureForAudit(MainQuest03FieldFlow.NextQuestId, "세 사람", QuestType.Main,
                new[]
                {
                    Objective("reach_miel_meeting", "앞서간 사람의 흔적을 따라가세요.", QuestObjectiveType.ReachLocation, MainQuest04FieldFlow.MeetingLocationId),
                    Objective("talk_to_miel_first", "부상자를 돌보고 있는 미엘과 대화하세요.", QuestObjectiveType.TalkToNpc, MainQuest04FieldFlow.MielId),
                    Objective("win_three_people_encounter", "태온, 미엘과 함께 다가오는 몬스터 무리를 물리치세요.", QuestObjectiveType.DefeatEncounter, MainQuest04FieldFlow.EncounterId),
                    Objective("talk_to_miel_after_battle", "전투가 끝난 뒤 미엘과 대화하세요.", QuestObjectiveType.TalkToNpc, MainQuest04FieldFlow.MielId)
                }, new RewardBundle { Experience = 0, Currency = 0, Items = System.Array.Empty<ItemReward>() },
                new[] { MainQuest03FieldFlow.QuestId }, MainQuest04FieldFlow.NextQuestId);
            main04.ConfigureNpcFlow(string.Empty);
            main04.ConfigureDescription("태온과 함께 앞서간 사람의 흔적을 따라가 미엘을 만나고, 세 사람이 가진 현장 정보를 합쳐 마을에 보고하기로 결정한다.");

            QuestDefinition main05 = LoadOrCreate("Main05_ReturnOfThree");
            main05.ConfigureForAudit(MainQuest04FieldFlow.NextQuestId, "돌아온 세 사람", QuestType.Main,
                new[]
                {
                    Objective("return_to_starter_village", "태온, 미엘과 함께 시작 마을로 돌아가세요.", QuestObjectiveType.ReachLocation, MainQuest05ReturnFlow.StarterVillageLocationId),
                    Objective("report_to_south_gate_guard", "남문 경비병에게 조사 결과를 알려주세요.", MainQuest01NpcFlow.GuardId),
                    Objective("report_to_village_representative", "주민 대표에게 조사 결과를 보고하세요.", MainQuest01NpcFlow.RepresentativeId)
                }, new RewardBundle
                {
                    Experience = 40,
                    Currency = 40,
                    Items = new[] { new ItemReward { ItemId = "item_healing_potion_small", Count = 1 } }
                }, new[] { MainQuest04FieldFlow.QuestId });
            main05.ConfigureNpcFlow(string.Empty);
            main05.ConfigureDescription("태온, 미엘과 함께 시작 마을로 돌아가 남문 경비병과 주민 대표에게 조사 결과를 보고하고, 초원 너머 숲의 다음 이상을 확인한다.");

            EditorUtility.SetDirty(main01);
            EditorUtility.SetDirty(main02);
            EditorUtility.SetDirty(main03);
            EditorUtility.SetDirty(main04);
            EditorUtility.SetDirty(main05);
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

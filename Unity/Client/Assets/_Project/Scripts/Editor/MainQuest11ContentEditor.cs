#if UNITY_EDITOR
using System;
using ProjectLimitless.Core;
using ProjectLimitless.World;
using UnityEditor;
using UnityEngine;

namespace ProjectLimitless.Editor
{
    /// <summary>Main11 데이터 두 개만 생성·갱신합니다. 기존 Dungeon Scene과 몬스터 Asset은 재생성하지 않습니다.</summary>
    public static class MainQuest11ContentEditor
    {
        public static void Build()
        {
            const string questPath = "Assets/_Project/Resources/QuestDefinitions/Main11_SilentCatacomb.asset";
            var quest = AssetDatabase.LoadAssetAtPath<QuestDefinition>(questPath);
            if (quest == null)
            {
                quest = ScriptableObject.CreateInstance<QuestDefinition>();
                AssetDatabase.CreateAsset(quest, questPath);
            }
            quest.ConfigureForAudit(MainQuest11DungeonFlow.QuestId, "침묵의 지하묘지", QuestType.Main, new[]
            {
                Step("explore_b1", "지하묘지 B1 내부를 살펴보세요.", QuestObjectiveType.ReachLocation, "dungeon01_main11_b1_central"),
                Step("inspect_west_ossuary", "서쪽 안치실을 조사하세요.", QuestObjectiveType.Interact, "dungeon01_main11_west_ossuary"),
                Step("inspect_collapsed_chamber", "동쪽 무너진 묘실을 조사하세요.", QuestObjectiveType.Interact, "dungeon01_main11_east_chamber"),
                Step("open_lower_passage", "북쪽 봉인된 하강로를 여세요.", QuestObjectiveType.Interact, "dungeon01_main11_lower_gate"),
                Step("descend_to_b2", "열린 하강로로 B2에 내려가세요.", QuestObjectiveType.ReachLocation, "dungeon01_main11_b2_entry"),
                Step("explore_b2", "B2의 안치 공간을 확인하세요.", QuestObjectiveType.Interact, "dungeon01_main11_b2_entrance"),
                Step("inspect_sealed_tombs", "서쪽 폐쇄 묘실을 조사하세요.", QuestObjectiveType.Interact, "dungeon01_main11_sealed_tombs"),
                Step("inspect_unknown_chamber", "동쪽 용도불명실을 조사하세요.", QuestObjectiveType.Interact, "dungeon01_main11_unknown_room"),
                Step("trace_blue_flow", "중앙 석실의 푸른 흐름을 살펴보세요.", QuestObjectiveType.Interact, "dungeon01_main11_blue_flow"),
                Step("defeat_silent_warden", "침묵의 파수꾼을 쓰러뜨리세요.", QuestObjectiveType.DefeatEncounter, MainQuest11DungeonFlow.BossId),
                Step("inspect_sealed_chamber", "파수꾼 뒤 봉인실을 조사하세요.", QuestObjectiveType.Interact, "dungeon01_main11_sealed_chamber"),
                Step("recover_broken_tablet", "부서진 석판 조각을 확보하세요.", QuestObjectiveType.Interact, "dungeon01_main11_broken_tablet"),
                Step("leave_silent_catacomb", "단서를 가지고 지하묘지 입구로 돌아가세요.", QuestObjectiveType.ReachLocation, CatacombEntranceSafeZone.SafeZoneId)
            }, new RewardBundle { Experience = 60, Currency = 50, Items = Array.Empty<ItemReward>() },
                new[] { MainQuest10FieldFlow.QuestId });
            quest.ConfigureDescription("B1과 B2의 흔적을 따라 침묵의 파수꾼 뒤 봉인실을 조사하고, 석판 조각을 가지고 입구로 돌아온다.");
            quest.ConfigureNpcFlow("");
            EditorUtility.SetDirty(quest);

            const string itemPath = "Assets/_Project/Resources/ItemDefinitions/BrokenCatacombTablet.asset";
            var item = AssetDatabase.LoadAssetAtPath<ItemDefinition>(itemPath);
            if (item == null)
            {
                item = ScriptableObject.CreateInstance<ItemDefinition>();
                AssetDatabase.CreateAsset(item, itemPath);
            }
            item.ConfigureContent(MainQuest11DungeonFlow.TabletItemId, "부서진 석판 조각",
                "대부분 부서져 읽을 수 없지만 지하묘지에서 반복해서 본 문양이 새겨져 있습니다.",
                ItemCategory.Quest, 1, 0, 0, null, Color.white, ItemUseType.None, "스토리 단서");
            item.ConfigureEffect(ItemEffectType.None, 0);
            EditorUtility.SetDirty(item);
            AssetDatabase.SaveAssets();
            QuestCatalog.ReloadForAudit();
            ItemCatalog.ReloadForAudit();
        }

        private static QuestObjectiveDefinition Step(string id, string description, QuestObjectiveType type, string target)
        {
            var value = new QuestObjectiveDefinition();
            value.ConfigureForAudit(id, description, type, target);
            return value;
        }
    }
}
#endif

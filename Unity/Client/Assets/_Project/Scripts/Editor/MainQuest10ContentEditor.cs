#if UNITY_EDITOR
using ProjectLimitless.Core;
using ProjectLimitless.World;
using UnityEditor;
using UnityEngine;

namespace ProjectLimitless.Editor
{
    /// <summary>Main10의 퀘스트 데이터만 만들며 기존 Scene과 사용자 아트는 다시 생성하지 않습니다.</summary>
    public static class MainQuest10ContentEditor
    {
        public static void Build()
        {
            const string path = "Assets/_Project/Resources/QuestDefinitions/Main10_CenterOfSilence.asset";
            var quest = AssetDatabase.LoadAssetAtPath<QuestDefinition>(path);
            if (quest == null)
            {
                quest = ScriptableObject.CreateInstance<QuestDefinition>();
                AssetDatabase.CreateAsset(quest, path);
            }
            quest.ConfigureForAudit(MainQuest10FieldFlow.QuestId, "침묵의 중심", QuestType.Main, new[]
            {
                Step("return_to_structural_trace", "폴이 발견한 구조물을 다시 조사하세요.", MainQuest09FieldFlow.StructureId),
                Step("examine_stonework", "가공된 돌의 흔적을 조사하세요.", MainQuest10FieldFlow.StoneworkId),
                Step("follow_the_silence", "소리가 사라지는 방향을 따라가세요.", MainQuest10FieldFlow.SilenceId),
                Step("find_underground_descent", "지하로 이어지는 흔적을 찾으세요.", MainQuest10FieldFlow.DescentId),
                Step("reveal_catacomb_entrance", "가려진 구조물의 입구를 확인하세요.", MainQuest10FieldFlow.EntranceId),
                Step("inspect_catacomb_entrance", "지하묘지 입구를 조사하세요.", MainQuest10FieldFlow.EntranceId),
                Step("prepare_to_enter_catacomb", "침묵의 지하묘지 입구를 확인하세요.", MainQuest10FieldFlow.EntranceId)
            }, new RewardBundle { Experience = 40, Currency = 40, Items = System.Array.Empty<ItemReward>() },
                new[] { MainQuest09FieldFlow.QuestId });
            quest.ConfigureDescription("Field_03 깊은 곳에서 가공된 석재의 연결과 고요해지는 방향을 따라가 침묵의 지하묘지 입구를 확인한다.");
            quest.ConfigureNpcFlow("");
            EditorUtility.SetDirty(quest);
            AssetDatabase.SaveAssets();
            QuestCatalog.ReloadForAudit();
        }

        private static QuestObjectiveDefinition Step(string id, string text, string target)
        {
            var objective = new QuestObjectiveDefinition();
            objective.ConfigureForAudit(id, text, QuestObjectiveType.Interact, target);
            return objective;
        }
    }
}
#endif

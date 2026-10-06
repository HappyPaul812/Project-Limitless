using System.Linq;
using ProjectLimitless.Core;
using UnityEditor;
using UnityEngine;

namespace ProjectLimitless.Editor
{
    /// <summary>Main17의 새 데이터만 생성합니다. 이전 Quest나 사용자의 Scene을 다시 생성하지 않습니다.</summary>
    public static class Main17ContentBuilder
    {
        public const string QuestId = "main_17_red_rift";
        public const string Field = "Field_08_RedRift";
        public const string QuestPath = "Assets/_Project/Resources/QuestDefinitions/Main17_RedRift.asset";
        public static readonly string[] ObjectiveIds = { "enter_red_rift", "inspect_first_crack", "follow_vibration",
            "inspect_animal_tracks", "meet_serin", "compare_findings", "investigate_deep_resonance", "witness_new_threat",
            "defeat_new_threat", "inspect_after_battle", "locate_deeper_route", "decide_to_withdraw" };
        public static readonly string[] Targets = { "entry", "crack", "vibration", "tracks", "serin", "compare",
            "resonance", "witness", "threat", "after", "route", "withdraw" };

        /// <summary>설계 순서와 안정적인 ID를 Asset에 저장합니다. 미확정 보상 수치는 추가하지 않습니다.</summary>
        public static string BuildQuest()
        {
            string[] labels = { "붉은 균열 협곡 진입", "첫 붉은 균열 조사", "반복되는 진동 추적", "갈라진 동물 흔적 조사",
                "협곡의 세린과 합류", "황야와 협곡 관찰 비교", "깊은 균열의 반복 신호 조사", "균열 주변 위협 목격",
                "지정 협곡 조우 격파", "전투 뒤에도 남은 진동 조사", "깊은 곳으로 향하는 경로 확인", "안전한 준비를 위해 철수 결정" };
            var steps = ObjectiveIds.Select((id, i) =>
            {
                var step = new QuestObjectiveDefinition();
                var kind = i == 0 || i == 7 ? QuestObjectiveType.ReachLocation : i == 8 ? QuestObjectiveType.DefeatEncounter : QuestObjectiveType.Interact;
                step.ConfigureForAudit(id, labels[i], kind, "field08_main17_" + Targets[i]);
                return step;
            }).ToArray();
            QuestDefinition quest = AssetDatabase.LoadAssetAtPath<QuestDefinition>(QuestPath);
            if (quest == null)
            {
                quest = ScriptableObject.CreateInstance<QuestDefinition>();
                AssetDatabase.CreateAsset(quest, QuestPath);
            }
            quest.ConfigureForAudit(QuestId, "붉은 균열", QuestType.Main, steps, new RewardBundle(), new[] { "main_16_shape_in_the_ash" });
            quest.ConfigureDescription("붉은 균열 협곡에서 황야의 진동과 열기 흔적을 비교하고, 깊은 곳으로 향하는 경로의 안전을 판단한다.");
            EditorUtility.SetDirty(quest);
            AssetDatabase.SaveAssetIfDirty(quest);
            QuestCatalog.ReloadForAudit();
            return QuestId + ": 12 sequential objectives / prerequisite Main16 / no new reward values";
        }
    }
}

using System.Linq;
using ProjectLimitless.Core;
using ProjectLimitless.Monster;
using ProjectLimitless.World;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace ProjectLimitless.Editor
{
    /// <summary>Main17의 새 데이터만 생성합니다. 이전 Quest나 사용자의 Scene을 다시 생성하지 않습니다.</summary>
    public static class Main17ContentBuilder
    {
        public const string QuestId = "main_17_red_rift";
        public const string Field = "Field_08_RedRift";
        public const string QuestPath = "Assets/_Project/Resources/QuestDefinitions/Main17_RedRift.asset";
        public const string ScenePath = "Assets/_Project/Scenes/Field_08_RedRift.unity";
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

        /// <summary>새 Field08 복사본만 Additive로 편집합니다. 출입구는 기존 공용 연결 데이터로 설치합니다.</summary>
        public static string BuildField()
        {
            Connection("Field07_WestToField08", "Field_07_AshenReach", "west_to_field08", -1, Field, "Spawn_From_Field07", false);
            Connection("Field08_EastToField07", Field, "east_to_field07", 1, "Field_07_AshenReach", "Spawn_From_Field08", true);
            var zonePath = "Assets/_Project/Resources/FieldEntranceSafetyZones/Field08_EastEntry.asset";
            var zone = AssetDatabase.LoadAssetAtPath<FieldEntranceSafetyZoneDefinition>(zonePath);
            if (zone == null) { zone = ScriptableObject.CreateInstance<FieldEntranceSafetyZoneDefinition>(); AssetDatabase.CreateAsset(zone, zonePath); }
            var serialized = new SerializedObject(zone);
            serialized.FindProperty("sceneName").stringValue = Field;
            serialized.FindProperty("zoneId").stringValue = "field08_east_entry";
            serialized.FindProperty("center").vector2Value = new Vector2(7, 0);
            serialized.FindProperty("radius").floatValue = 2.2f;
            serialized.ApplyModifiedPropertiesWithoutUndo(); EditorUtility.SetDirty(zone); AssetDatabase.SaveAssetIfDirty(zone);
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(ScenePath) == null)
            {
                if (!AssetDatabase.CopyAsset("Assets/_Project/Scenes/Field_07_AshenReach.unity", ScenePath))
                    throw new System.InvalidOperationException("Field08 Scene 복사 실패");
                var active = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
                var selected = Selection.objects;
                var scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Additive);
                try
                {
                    var roots = scene.GetRootGameObjects();
                    var environment = roots.Single(root => root.name == "FieldEnvironment").transform;
                    foreach (var renderer in environment.GetComponentsInChildren<SpriteRenderer>())
                    {
                        if (renderer.name.StartsWith("AshSoil_") || renderer.name.StartsWith("Road")) renderer.color = new Color(.48f, .24f, .20f);
                        else if (renderer.name.StartsWith("Rock") || renderer.name.StartsWith("CanyonLip")) renderer.color = new Color(.22f, .19f, .20f);
                        else if (renderer.name.StartsWith("AshCrack")) renderer.color = new Color(.58f, .18f, .12f);
                    }
                    var title = environment.Find("AshenReachTitle");
                    if (title != null) { title.name = "RedRiftTitle"; title.GetComponent<TextMesh>().text = "붉은 균열 협곡"; }
                    // 심부는 아직 닫힙니다. 서쪽 경계를 유지하고 북/남 조사 지점에서 중앙으로 돌아올 수 있습니다.
                    var bounds = roots.Select(root => root.GetComponent<WorldBounds2D>()).First(value => value != null);
                    foreach (Transform child in bounds.transform.Cast<Transform>().Where(value => value.name.StartsWith("Boundary_")).ToArray())
                        Object.DestroyImmediate(child.gameObject);
                    WorldBounds2D.CreateBoundaryColliders(bounds.transform, bounds.Bounds, .3f, new WorldBoundaryOpening(WorldBoundarySide.Right, 0, 3));
                    if (!EditorSceneManager.SaveScene(scene, ScenePath)) throw new System.InvalidOperationException("Field08 저장 실패");
                }
                finally { EditorSceneManager.CloseScene(scene, true); UnityEngine.SceneManagement.SceneManager.SetActiveScene(active); Selection.objects = selected; }
            }
            if (!EditorBuildSettings.scenes.Any(value => value.path == ScenePath))
                EditorBuildSettings.scenes = EditorBuildSettings.scenes.Concat(new[] { new EditorBuildSettingsScene(ScenePath, true) }).ToArray();
            return "Field08 / two connections / entry safety / build scene ready";
        }

        private static void Connection(string name, string source, string id, int side, string target, string targetSpawn, bool replace)
        {
            string path = "Assets/_Project/Resources/FieldConnections/" + name + ".asset";
            var asset = AssetDatabase.LoadAssetAtPath<FieldConnectionDefinition>(path);
            if (asset == null) { asset = ScriptableObject.CreateInstance<FieldConnectionDefinition>(); AssetDatabase.CreateAsset(asset, path); }
            string ownSpawn = source == Field ? "Spawn_From_Field07" : "Spawn_From_Field08";
            asset.Configure(source, id, ownSpawn, new Vector2(side * 7, 0), new Vector2(side * 10.25f, 0), new Vector2(1.1f, 3), target, targetSpawn, replace);
            EditorUtility.SetDirty(asset); AssetDatabase.SaveAssetIfDirty(asset);
        }
    }
}

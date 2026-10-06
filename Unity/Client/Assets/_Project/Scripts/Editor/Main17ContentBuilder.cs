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

        /// <summary>설계 순서와 안정적인 ID와 사용자가 확정한 Chapter2 완료 보상을 Asset에 저장합니다.</summary>
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
            quest.ConfigureForAudit(QuestId, "붉은 균열", QuestType.Main, steps,
                new RewardBundle { Experience = 60, Currency = 50 }, new[] { "main_16_shape_in_the_ash" });
            quest.ConfigureDescription("붉은 균열 협곡에서 황야의 진동과 열기 흔적을 비교하고, 깊은 곳으로 향하는 경로의 안전을 판단한다.");
            EditorUtility.SetDirty(quest);
            AssetDatabase.SaveAssetIfDirty(quest);
            QuestCatalog.ReloadForAudit();
            return QuestId + ": 12 sequential objectives / prerequisite Main16 / approved reward EXP60 Talent50";
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

        /// <summary>필수 단독 균열도마뱀과 우회 가능한 일반 갑충을 구분합니다. 원본 몬스터 수치는 변경하지 않습니다.</summary>
        public static string BuildEncounter()
        {
            string path = "Assets/_Project/Resources/StoryEncounterReturns/Main17_ThreatReturn.asset";
            var spawn = AssetDatabase.LoadAssetAtPath<FieldMonsterSpawnDefinition>(path);
            if (spawn == null) { spawn = ScriptableObject.CreateInstance<FieldMonsterSpawnDefinition>(); AssetDatabase.CreateAsset(spawn, path); }
            var monster = Resources.LoadAll<MonsterDefinition>("MonsterDefinitions").Single(value => value.MonsterId == "fissure_lizard");
            spawn.Configure(Field, Chapter2Main17Flow.EncounterId, monster, new Vector2(-4, 0), .8f, 35);
            spawn.SetNonRespawningBoss(false); EditorUtility.SetDirty(spawn); AssetDatabase.SaveAssetIfDirty(spawn);
            string generalPath = "Assets/_Project/Resources/MonsterSpawns/Field08_Beetle01.asset";
            var general = AssetDatabase.LoadAssetAtPath<FieldMonsterSpawnDefinition>(generalPath);
            if (general == null) { general = ScriptableObject.CreateInstance<FieldMonsterSpawnDefinition>(); AssetDatabase.CreateAsset(general, generalPath); }
            var beetle = Resources.LoadAll<MonsterDefinition>("MonsterDefinitions").Single(value => value.MonsterId == "ember_beetle");
            general.Configure(Field, "field08_beetle_01", beetle, new Vector2(4, -4.5f), .8f, 35);
            general.SetNonRespawningBoss(false); EditorUtility.SetDirty(general); AssetDatabase.SaveAssetIfDirty(general);
            return "Main17 Story single lizard / optional general beetle / unchanged stats";
        }

        /// <summary>복사된 타일의 녹색 경계를 없애고 기본 도형으로 협곡 지형을 표현합니다. PNG 원본이나 이전 Scene은 수정하지 않습니다.</summary>
        public static string PolishField()
        {
            // 정식 원본이 도착한 뒤에는 생성기가 완성 Art를 다시 단색 Placeholder로 덮지 않습니다.
            if (AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/_Project/Art/Environment/Chapter2/RedRift/RedRift_Ground_Base.png") != null)
                return ProjectLimitless.EditorTools.Main17EnvironmentArt.Apply();
            var active = UnityEngine.SceneManagement.SceneManager.GetActiveScene(); var selected = Selection.objects;
            var scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Additive);
            try
            {
                var environment = scene.GetRootGameObjects().Single(value => value.name == "FieldEnvironment").transform;
                foreach (Transform child in environment.Cast<Transform>().Where(value => value.name.StartsWith("RedRiftGeometry_")).ToArray()) Object.DestroyImmediate(child.gameObject);
                foreach (var renderer in environment.GetComponentsInChildren<SpriteRenderer>())
                    if (renderer.name.StartsWith("AshSoil_") || renderer.name.StartsWith("Road") || renderer.name.StartsWith("Tree") || renderer.name.StartsWith("BrokenTrack_")) renderer.gameObject.SetActive(false);
                Material ground = Material("RedRift_Ground", new Color(.42f, .20f, .15f));
                Material dark = Material("RedRift_Crack", new Color(.12f, .09f, .10f));
                Material glow = Material("RedRift_Glow", new Color(.52f, .13f, .08f));
                Material ash = Material("RedRift_Ash", new Color(.43f, .35f, .31f));
                Plane(environment, "Ground", Vector2.zero, new Vector2(21, 15), ground, -20);
                // 깊은 틈은 중앙 통로를 막지만 북쪽/남쪽으로 돌아갈 수 있습니다. 서쪽 조사 좌표는 틈과 겹치지 않습니다.
                var crack = Plane(environment, "DeepCrack", new Vector2(-5, .5f), new Vector2(.7f, 7), dark, -10);
                crack.AddComponent<BoxCollider2D>();
                Plane(environment, "InnerGlow", new Vector2(-4.9f, .5f), new Vector2(.10f, 6.5f), glow, -9);
                for (int i = 0; i < 6; i++)
                {
                    var line = Plane(environment, "Branch" + i, new Vector2(-4.3f + i * 1.3f, -1.6f + (i % 3) * 1.5f), new Vector2(1.3f, .08f), dark, -8);
                    line.transform.localRotation = Quaternion.Euler(0, 0, i % 2 == 0 ? 25 : -30);
                }
                for (int i = 0; i < 24; i++)
                {
                    var mark = Plane(environment, "Ash" + i, new Vector2(-8 + (i * 7 % 19) * .85f, -5 + (i * 3 % 11)), new Vector2(.15f, .07f), ash, -7);
                    // 재 표시는 Scene에 기본 Mesh만 저장합니다. 별도 파일이 없는 런타임 전용 Behaviour는 직렬화하지 않습니다.
                }
                for (int i = 0; i < 8; i++) Plane(environment, "Track" + i, new Vector2(3 - i * .4f, i < 4 ? 3 : 3 + (i % 2 == 0 ? 1 : -1) * (i - 3) * .2f), new Vector2(.10f, .16f), ash, -6);
                for (int i = 0; i < 3; i++)
                {
                    var rock = Plane(environment, "Obsidian" + i, new Vector2(-7 + i * .6f, 4.8f), new Vector2(.7f, 1.5f), dark, -6);
                    rock.transform.localRotation = Quaternion.Euler(0, 0, 25 + i * 20);
                }
                if (!EditorSceneManager.SaveScene(scene, ScenePath)) throw new System.InvalidOperationException("Field08 지형 저장 실패");
                return "Field08 red ground / deep crack with north-south detours / ash / dark rock; no PNG generated";
            }
            finally { EditorSceneManager.CloseScene(scene, true); UnityEngine.SceneManagement.SceneManager.SetActiveScene(active); Selection.objects = selected; }
        }
        private static Material Material(string name, Color color)
        {
            string path = "Assets/_Project/Resources/Chapter2/" + name + ".mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material == null) { material = new Material(Shader.Find("Sprites/Default")); AssetDatabase.CreateAsset(material, path); }
            material.color = color; EditorUtility.SetDirty(material); AssetDatabase.SaveAssetIfDirty(material); return material;
        }
        private static GameObject Plane(Transform parent, string name, Vector2 at, Vector2 size, Material material, int order)
        {
            var value = GameObject.CreatePrimitive(PrimitiveType.Quad); value.name = "RedRiftGeometry_" + name;
            value.transform.SetParent(parent, false); value.transform.localPosition = at; value.transform.localScale = new Vector3(size.x, size.y, 1);
            Object.DestroyImmediate(value.GetComponent<Collider>());
            var renderer = value.GetComponent<MeshRenderer>(); renderer.sharedMaterial = material; renderer.sortingOrder = order; return value;
        }
    }
}

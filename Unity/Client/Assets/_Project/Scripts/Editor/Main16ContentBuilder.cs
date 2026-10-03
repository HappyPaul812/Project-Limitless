using System;
using System.Linq;
using ProjectLimitless.Core;
using ProjectLimitless.Monster;
using ProjectLimitless.World;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace ProjectLimitless.Editor
{
    /// <summary>기존 Scene과 원본 이미지는 보존하고 Main16 전용 Scene·데이터만 생성합니다.</summary>
    public static class Main16ContentBuilder
    {
        private const string Root = "Assets/_Project/";
        private const string ScenePath = Root + "Scenes/Field_07_AshenReach.unity";

        public static string Build()
        {
            string[] ids = { "talk_to_leon_about_ash", "head_deeper_west", "enter_ashen_reach", "inspect_moving_ash",
                "trace_ground_vibration", "inspect_broken_tracks", "witness_ember_wraith", "defeat_ember_wraith",
                "inspect_afterimage", "find_western_route", "return_to_arbel", "report_the_anomaly" };
            string[] targets = { "arbel-leon", "field06_main16_west_exit", "field07_main16_entry", "field07_main16_ash",
                "field07_main16_vibration", "field07_main16_tracks", Chapter2Main16Flow.WitnessId, Chapter2Main16Flow.StoryEncounterId,
                "field07_main16_afterimage", "field07_main16_canyon", "arbel_main16_return", "arbel-leon" };
            string[] labels = { "레온에게 재 이상 현상 듣기", "뜨거운 길의 서쪽 출구로 이동", "재바람 황야에 진입",
                "바람과 다르게 움직이는 재 조사", "재와 지면 진동의 간격 추적", "흩어진 동물 흔적 조사", "재 속의 형상 목격",
                "지정 불씨망령 격파", "전투 후 남은 균열과 열기 조사", "붉은 균열 협곡 입구 확인", "아르벨로 귀환", "레온에게 사실과 추정 보고" };
            QuestObjectiveType[] kinds = { QuestObjectiveType.TalkToNpc, QuestObjectiveType.ReachLocation, QuestObjectiveType.ReachLocation,
                QuestObjectiveType.Interact, QuestObjectiveType.Interact, QuestObjectiveType.Interact, QuestObjectiveType.ReachLocation,
                QuestObjectiveType.DefeatEncounter, QuestObjectiveType.Interact, QuestObjectiveType.ReachLocation, QuestObjectiveType.ReachLocation, QuestObjectiveType.TalkToNpc };
            var steps = ids.Select((id, i) => { var step = new QuestObjectiveDefinition(); step.ConfigureForAudit(id, labels[i], kinds[i], targets[i]); return step; }).ToArray();
            QuestDefinition quest = Asset<QuestDefinition>("QuestDefinitions/Main16_ShapeInTheAsh.asset");
            quest.ConfigureForAudit(Chapter2Main16Flow.QuestId, "재 속의 형상", QuestType.Main, steps, new RewardBundle(), new[] { "main_15_burning_traces" });
            quest.ConfigureNpcFlow("arbel-leon");
            quest.ConfigureDescription("재바람 황야에서 재와 지면의 이상을 조사하고, 관찰한 사실과 추정을 나누어 레온에게 보고한다.");
            Save(quest);
            MonsterDefinition[] monsters = Resources.LoadAll<MonsterDefinition>("MonsterDefinitions");
            Spawn("StoryEncounterReturns/Main16_EmberWraithReturn.asset", Chapter2Main16Flow.StoryEncounterId, "ember_wraith", new Vector2(-3, 0), monsters);
            Spawn("MonsterSpawns/Field07_Beasts01.asset", "field07_beasts_01", "soot_hound", new Vector2(-1, 4), monsters);
            Spawn("MonsterSpawns/Field07_Beasts02.asset", "field07_beasts_02", "fissure_lizard", new Vector2(0, -4), monsters);
            Spawn("MonsterSpawns/Field07_EmberWraith01.asset", "field07_ember_wraith_01", "ember_wraith", new Vector2(-6, 4), monsters);
            var zone = Asset<FieldEntranceSafetyZoneDefinition>("FieldEntranceSafetyZones/Field07_EastEntry.asset");
            var serialized = new SerializedObject(zone);
            serialized.FindProperty("sceneName").stringValue = Chapter2Main16Flow.Field;
            serialized.FindProperty("zoneId").stringValue = "field07_east_entry";
            serialized.FindProperty("center").vector2Value = new Vector2(7, 0);
            serialized.FindProperty("radius").floatValue = 2.2f;
            serialized.ApplyModifiedPropertiesWithoutUndo(); Save(zone);
            var west = Asset<FieldConnectionDefinition>("FieldConnections/Field06_WestToField07.asset");
            west.Configure(Chapter2Main16Flow.PreviousField, "west_to_field07", "Spawn_From_Field07", new Vector2(-7, 0),
                new Vector2(-10.25f, 0), new Vector2(1.1f, 3), Chapter2Main16Flow.Field, "Spawn_From_Field06", false); Save(west);
            var east = Asset<FieldConnectionDefinition>("FieldConnections/Field07_EastToField06.asset");
            east.Configure(Chapter2Main16Flow.Field, "east_to_field06", "Spawn_From_Field06", new Vector2(7, 0),
                new Vector2(10.25f, 0), new Vector2(1.1f, 3), Chapter2Main16Flow.PreviousField, "Spawn_From_Field07", true); Save(east);
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(ScenePath) == null) BuildScene();
            if (!EditorBuildSettings.scenes.Any(scene => scene.path == ScenePath))
                EditorBuildSettings.scenes = EditorBuildSettings.scenes.Concat(new[] { new EditorBuildSettingsScene(ScenePath, true) }).ToArray();
            QuestCatalog.ReloadForAudit();
            return "Main16: 12 objectives, 8 data assets, Field07 and BuildSettings ready.";
        }

        private static T Asset<T>(string relative) where T : ScriptableObject
        {
            string path = Root + "Resources/" + relative;
            string folder = path.Substring(0, path.LastIndexOf('/'));
            EnsureFolder(folder);
            T asset = AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset != null) return asset;
            asset = ScriptableObject.CreateInstance<T>(); AssetDatabase.CreateAsset(asset, path); return asset;
        }
        private static void EnsureFolder(string path)
        {
            if (AssetDatabase.IsValidFolder(path)) return;
            int split = path.LastIndexOf('/'); EnsureFolder(path.Substring(0, split));
            AssetDatabase.CreateFolder(path.Substring(0, split), path.Substring(split + 1));
        }
        private static void Save(UnityEngine.Object asset) { EditorUtility.SetDirty(asset); AssetDatabase.SaveAssetIfDirty(asset); }
        private static void Spawn(string path, string id, string monsterId, Vector2 at, MonsterDefinition[] monsters)
        {
            var spawn = Asset<FieldMonsterSpawnDefinition>(path);
            spawn.Configure(Chapter2Main16Flow.Field, id, monsters.Single(monster => monster.MonsterId == monsterId), at, .8f, 35);
            spawn.SetNonRespawningBoss(false); Save(spawn);
        }

        public static void BuildScene()
        {
            // 별도 Scene을 Additive로 열어 저장하고 닫습니다. 기존 활성 Scene·선택·창 포커스를 유지합니다.
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(ScenePath) == null &&
                !AssetDatabase.CopyAsset(Root + "Scenes/Field_06_ScorchedTrail.unity", ScenePath)) throw new InvalidOperationException("Field07 Scene 복사 실패");
            var active = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            var selection = Selection.objects;
            var scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Additive);
            try
            {
                var roots = scene.GetRootGameObjects();
                Transform environment = roots.Single(root => root.name == "FieldEnvironment").transform;
                foreach (Transform generated in environment.Cast<Transform>().Where(child => child.name.StartsWith("AshCrack_") || child.name == "AshenReachTitle" || child.name.StartsWith("CanyonLip_") || child.name.StartsWith("BrokenTrack_")).ToArray())
                    UnityEngine.Object.DestroyImmediate(generated.gameObject);
                Sprite soil = environment.Find("RoadLeft_0").GetComponent<SpriteRenderer>().sprite;
                foreach (Transform child in environment.Cast<Transform>().ToArray())
                {
                    SpriteRenderer renderer = child.GetComponent<SpriteRenderer>();
                    if (child.name.StartsWith("Grass_"))
                    {
                        renderer.sprite = soil; renderer.color = new Color(.67f, .58f, .48f);
                        child.name = "AshSoil_" + child.name.Substring(6);
                    }
                    else if (child.name.StartsWith("Road")) renderer.color = new Color(.58f, .50f, .43f);
                    else if (child.name.StartsWith("Bush") || child.name.StartsWith("Fence")) UnityEngine.Object.DestroyImmediate(child.gameObject);
                    else if (child.name.StartsWith("Tree")) renderer.color = new Color(.50f, .36f, .27f);
                    else if (child.name.StartsWith("Rock")) renderer.color = new Color(.31f, .29f, .27f);
                }
                foreach (GameObject root in roots)
                    if (root.name.StartsWith("Boundary") || root.name.StartsWith("Spawn_") || root.name.StartsWith("Entrance_") || root.name.StartsWith("Future"))
                        UnityEngine.Object.DestroyImmediate(root);
                WorldBounds2D bounds = roots.First(root => root != null && root.GetComponent<WorldBounds2D>() != null).GetComponent<WorldBounds2D>();
                foreach (Transform boundary in bounds.transform.Cast<Transform>().Where(child => child.name.StartsWith("Boundary_")).ToArray())
                    UnityEngine.Object.DestroyImmediate(boundary.gameObject);
                // 복사된 Bounds의 직렬화 값과 Camera 연결은 그대로 재사용합니다. 실제 통로는 동쪽 하나뿐입니다.
                WorldBounds2D.CreateBoundaryColliders(bounds.transform, bounds.Bounds, .3f, new WorldBoundaryOpening(WorldBoundarySide.Right, 0, 3));
                var player = roots.First(root => root != null && root.GetComponent<ProjectLimitless.Player.PlayerController>() != null);
                player.transform.position = new Vector3(7, 0, 0);
                for (int i = 0; i < 8; i++)
                {
                    var crack = new GameObject("AshCrack_" + i, typeof(SpriteRenderer)); crack.transform.SetParent(environment, false);
                    crack.transform.localPosition = new Vector3(-4.5f + i * .55f, -1.1f + (i % 3) * .28f, 0);
                    crack.transform.localScale = new Vector3(.7f, .08f, 1); crack.transform.localRotation = Quaternion.Euler(0, 0, (i % 2 == 0 ? 15 : -20));
                    var renderer = crack.GetComponent<SpriteRenderer>(); renderer.sprite = soil; renderer.color = i % 3 == 0 ? new Color(.47f, .23f, .17f) : new Color(.27f, .20f, .17f); renderer.sortingOrder = 1;
                }
                var title = new GameObject("AshenReachTitle", typeof(TextMesh)); title.transform.SetParent(environment, false);
                // 발자국은 원본 바닥 Sprite의 작은 배치로 표현합니다. 서쪽으로 이어지다 위·아래로 흩어지는 흔적입니다.
                for (int i = 0; i < 10; i++)
                {
                    var track = new GameObject("BrokenTrack_" + i, typeof(SpriteRenderer)); track.transform.SetParent(environment, false);
                    track.transform.localPosition = new Vector3(i < 6 ? 1.5f - i * .45f : -1.3f - (i - 6) * .18f,
                        i < 6 ? 1 + (i % 2) * .18f : 1 + (i % 2 == 0 ? 1 : -1) * (i - 5) * .2f, 0);
                    track.transform.localScale = new Vector3(.12f, .18f, 1);
                    var renderer = track.GetComponent<SpriteRenderer>(); renderer.sprite = soil; renderer.color = new Color(.35f, .31f, .28f); renderer.sortingOrder = 1;
                }
                title.transform.localPosition = new Vector3(6, 5.8f, 0);
                var text = title.GetComponent<TextMesh>(); text.text = "재바람 황야"; text.characterSize = .18f; text.anchor = TextAnchor.MiddleCenter;
                text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"); text.fontSize = 32; text.GetComponent<MeshRenderer>().sharedMaterial = text.font.material;
                var rock = environment.Find("RockWest");
                for (int i = 0; i < 2; i++)
                {
                    var lip = UnityEngine.Object.Instantiate(rock.gameObject, environment); lip.name = "CanyonLip_" + i;
                    lip.transform.localPosition = new Vector3(-9.1f, i == 0 ? 1.9f : -1.9f, 0);
                    lip.GetComponent<SpriteRenderer>().color = new Color(.65f, .30f, .22f);
                }
                if (!EditorSceneManager.SaveScene(scene, ScenePath)) throw new InvalidOperationException("Field07 Scene 저장 실패");
            }
            finally { EditorSceneManager.CloseScene(scene, true); UnityEngine.SceneManagement.SceneManager.SetActiveScene(active); Selection.objects = selection; }
        }
    }
}

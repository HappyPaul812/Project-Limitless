#if UNITY_EDITOR
using System;
using System.Linq;
using ProjectLimitless.CameraSystem;
using ProjectLimitless.Monster;
using ProjectLimitless.World;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectLimitless.Editor
{
    /// <summary>
    /// B1과 같은 Player·Camera·World Bounds·석재 Sprite를 써서 B2의 독립 Scene을 만듭니다.
    /// B1의 봉인 벽은 건드리지 않아 Main11 이전의 실제 플레이 경로가 열리지 않습니다.
    /// </summary>
    public static class Dungeon01B2Builder
    {
        private const string ScenePath = "Assets/_Project/Scenes/Dungeon_01_B2.unity";
        private const string SpawnRoot = "Assets/_Project/Resources/MonsterSpawns/";
        private const string MonsterRoot = "Assets/_Project/Resources/MonsterDefinitions/";
        private static Transform environment;
        private static Sprite stoneSprite;

        [MenuItem("Project Limitless/Content/Build Dungeon 01 B2")]
        public static void Build()
        {
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(ScenePath) != null)
                throw new InvalidOperationException("B2 Scene이 이미 있으므로 자동으로 덮어쓰지 않습니다.");
            GameObject playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(
                "Assets/_Project/Prefabs/PlayerPlaceholder.prefab");
            stoneSprite = AssetDatabase.LoadAssetAtPath<Sprite>(
                "Assets/ThirdParty/Kenney/RPGBase/PNG/rpgTile152.png");
            MonsterDefinition wight = LoadMonster("07_GraveWight");
            MonsterDefinition bat = LoadMonster("06_ShadeBat");
            MonsterDefinition echo = LoadMonster("08_SilentEcho");
            MonsterDefinition guardian = LoadMonster("09_SealGuardian");
            MonsterDefinition warden = LoadMonster("10_SilentWarden");
            if (playerPrefab == null || stoneSprite == null || wight == null || bat == null ||
                echo == null || guardian == null || warden == null)
                throw new InvalidOperationException("B2에 필요한 Player·석재·MonsterDefinition을 모두 준비해야 합니다.");

            Scene previous = SceneManager.GetActiveScene();
            Scene dungeon = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);
            try
            {
                SceneManager.SetActiveScene(dungeon);
                environment = new GameObject("Dungeon01B2_Environment").transform;
                Rect("StoneBase", 0, 0, 33, 25, new Color(.3f, .34f, .37f), -20);
                Floor("B2Entrance", 0, -9, 9, 5);
                Floor("B2SouthPassage", 0, -6, 5, 3);
                Floor("WestClosedOssuary", -10, -1, 8, 8);
                Floor("EastUnknownRoom", 10, -1, 8, 8);
                Floor("CentralStoneRoom", 0, -1, 12, 8);
                Floor("SealGallery", 0, 4.5f, 6, 6);
                Floor("BossAntechamber", 0, 9.5f, 11, 5);
                BuildWallsAndMarks();

                var player = (GameObject)PrefabUtility.InstantiatePrefab(playerPrefab, dungeon);
                player.name = "Player";
                player.transform.position = new Vector3(0, -8.5f, 0);
                var cameraObject = new GameObject("Main Camera", typeof(Camera), typeof(AudioListener), typeof(CameraFollow));
                cameraObject.tag = "MainCamera";
                cameraObject.transform.position = new Vector3(0, -8.5f, -10);
                Camera camera = cameraObject.GetComponent<Camera>();
                camera.orthographic = true;
                camera.orthographicSize = 5;
                camera.clearFlags = CameraClearFlags.SolidColor;
                camera.backgroundColor = new Color(.07f, .1f, .14f);
                cameraObject.GetComponent<CameraFollow>().SetTarget(player.transform);
                WorldBoundaryGeneratorUtility.Create("WorldBounds", Vector2.zero, new Vector2(33, 25), 1);
                var spawn = new GameObject("Spawn_From_B1_Test", typeof(SceneSpawnPoint));
                spawn.transform.position = new Vector3(0, -8.5f, 0);
                spawn.GetComponent<SceneSpawnPoint>().Configure("Spawn_From_B1_Test");
                // 현재 B1에서 B2로 향하는 Trigger는 만들지 않습니다. 이 출구는 Editor/Test 진입의 귀환용입니다.
                var exit = new GameObject("Exit_To_B1_Test", typeof(BoxCollider2D), typeof(SceneTransitionTrigger));
                exit.transform.position = new Vector3(0, -11.9f, 0);
                exit.GetComponent<BoxCollider2D>().size = new Vector2(3, .8f);
                exit.GetComponent<BoxCollider2D>().isTrigger = true;
                exit.GetComponent<SceneTransitionTrigger>().Configure("Dungeon_01", "Spawn_From_Field03");
                EditorSceneManager.SaveScene(dungeon, ScenePath);
            }
            finally
            {
                SceneManager.SetActiveScene(previous);
                EditorSceneManager.CloseScene(dungeon, true);
            }

            CreateSpawn("Dungeon01_B2_01", "dungeon01_b2_01", wight, new Vector2(0, -4), .55f, false);
            CreateSpawn("Dungeon01_B2_02", "dungeon01_b2_02", bat, new Vector2(-10, -1), .55f, false);
            CreateSpawn("Dungeon01_B2_03", "dungeon01_b2_03", guardian, new Vector2(10, -1), .55f, false);
            CreateSpawn("Dungeon01_B2_04", "dungeon01_b2_04", guardian, new Vector2(0, 4), .55f, false);
            CreateSpawn("Dungeon01_B2_Boss", "dungeon01_b2_boss", warden, new Vector2(0, 9.5f), .55f, true);
            var scenes = EditorBuildSettings.scenes.ToList();
            if (scenes.All(item => item.path != ScenePath))
            {
                scenes.Add(new EditorBuildSettingsScene(ScenePath, true));
                EditorBuildSettings.scenes = scenes.ToArray();
            }
            AssetDatabase.SaveAssets();
            Debug.Log("Dungeon_01 B2 독립 Scene과 일반 조우 4개·보스 테스트 조우를 설치했습니다.");
        }

        private static MonsterDefinition LoadMonster(string fileName) =>
            AssetDatabase.LoadAssetAtPath<MonsterDefinition>(MonsterRoot + fileName + ".asset");

        private static void CreateSpawn(string fileName, string id, MonsterDefinition monster, Vector2 position,
            float radius, bool boss)
        {
            if (AssetDatabase.LoadAssetAtPath<FieldMonsterSpawnDefinition>(SpawnRoot + fileName + ".asset") != null)
                throw new InvalidOperationException("기존 B2 Spawn Asset을 덮어쓰지 않습니다: " + fileName);
            var spawn = ScriptableObject.CreateInstance<FieldMonsterSpawnDefinition>();
            spawn.Configure("Dungeon_01_B2", id, monster, position, radius, 35);
            spawn.SetNonRespawningBoss(boss);
            AssetDatabase.CreateAsset(spawn, SpawnRoot + fileName + ".asset");
        }

        private static void Floor(string name, float x, float y, float width, float height)
        {
            Rect(name, x, y, width, height, new Color(.58f, .61f, .64f), -10);
            for (float line = x - width / 2 + 1; line < x + width / 2; line += 2)
                Rect(name + "_Joint", line, y, .035f, height, new Color(.35f, .38f, .42f), -9);
        }

        private static void BuildWallsAndMarks()
        {
            Wall("EntryWest", -4.5f, -9, .35f, 5);
            Wall("EntryEast", 4.5f, -9, .35f, 5);
            Wall("WestOuter", -14, -1, .4f, 8);
            Wall("EastOuter", 14, -1, .4f, 8);
            Wall("WestTop", -10, 3, 8, .4f);
            Wall("EastTop", 10, 3, 8, .4f);
            Wall("SealGalleryWest", -3.2f, 5, .4f, 5);
            Wall("SealGalleryEast", 3.2f, 5, .4f, 5);
            Wall("BossRoomWest", -5.5f, 9.5f, .4f, 5);
            Wall("BossRoomEast", 5.5f, 9.5f, .4f, 5);
            for (int index = 0; index < 4; index++)
            {
                Rect("UnknownStoneMark", -3 + index * 2, 1.6f, .35f, .35f,
                    new Color(.2f, .35f, .47f), 1);
                Rect("BlueFlow", -1.5f + index, 5.2f, .12f, 1.4f,
                    new Color(.22f, .48f, .62f), 0);
            }
            var clueAnchor = new GameObject("dungeon01_b2_after_boss_anchor");
            clueAnchor.transform.SetParent(environment, false);
            clueAnchor.transform.position = new Vector3(0, 11.6f, 0);
        }

        private static void Wall(string name, float x, float y, float width, float height)
        {
            GameObject wall = Rect(name, x, y, width, height, new Color(.72f, .75f, .77f), 2);
            wall.AddComponent<BoxCollider2D>().size = stoneSprite.bounds.size;
        }

        private static GameObject Rect(string name, float x, float y, float width, float height, Color color, int order)
        {
            var tile = new GameObject(name, typeof(SpriteRenderer));
            tile.transform.SetParent(environment, false);
            tile.transform.position = new Vector3(x, y, 0);
            tile.transform.localScale = new Vector3(width / stoneSprite.bounds.size.x,
                height / stoneSprite.bounds.size.y, 1);
            var renderer = tile.GetComponent<SpriteRenderer>();
            renderer.sprite = stoneSprite;
            renderer.color = color;
            renderer.sortingOrder = order;
            return tile;
        }
    }
}
#endif

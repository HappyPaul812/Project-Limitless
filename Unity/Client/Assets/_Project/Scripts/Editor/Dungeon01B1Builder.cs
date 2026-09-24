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
    /// 기존 월드의 Player·Camera·전환·몬스터 데이터 방식으로 B1 Scene을 설치합니다.
    /// 열린 Scene은 Additive로 유지하고 Dungeon_01만 새로 저장합니다.
    /// </summary>
    public static class Dungeon01B1Builder
    {
        const string ScenePath = "Assets/_Project/Scenes/Dungeon_01.unity";
        const string Root = "Assets/_Project/Resources/";
        const string PlayerPath = "Assets/_Project/Prefabs/PlayerPlaceholder.prefab";
        static Transform environment;
        static Sprite stoneSprite;

        [MenuItem("Project Limitless/Content/Build Dungeon 01 B1")]
        public static void Build()
        {
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(ScenePath) != null)
                throw new InvalidOperationException("Dungeon_01이 이미 있으므로 자동으로 덮어쓰지 않습니다.");
            var playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(PlayerPath);
            var wight = AssetDatabase.LoadAssetAtPath<MonsterDefinition>(Root + "MonsterDefinitions/07_GraveWight.asset");
            var bat = AssetDatabase.LoadAssetAtPath<MonsterDefinition>(Root + "MonsterDefinitions/06_ShadeBat.asset");
            if (playerPrefab == null || wight == null || bat == null) throw new InvalidOperationException("Player 또는 기존 두 몬스터 Asset이 없습니다.");

            Scene previous = SceneManager.GetActiveScene();
            Scene dungeon = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);
            try
            {
                SceneManager.SetActiveScene(dungeon);
                environment = new GameObject("Dungeon01_Environment").transform;
                stoneSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/ThirdParty/Kenney/RPGBase/PNG/rpgTile152.png");
                if (stoneSprite == null) throw new InvalidOperationException("기존 밝은 석재 기본 Sprite를 찾지 못했습니다.");
                // 기존 Main10 입구처럼 단순한 석재 형태를 쌓습니다. 새 이미지나 외부 Asset은 만들지 않습니다.
                Rect("StoneBase", 0, 0, 33, 25, new Color(.40f, .42f, .42f), -20);
                Floor("EntranceHall", 0, -9, 9, 5);
                Floor("EntrancePassage", 0, -6, 4, 2);
                Floor("CentralGallery", 0, -3, 11, 5);
                Floor("WestPassage", -6, -2, 5, 3);
                Floor("WestOssuary", -10, 0, 8, 8);
                Floor("EastPassage", 6, -2, 5, 3);
                Floor("EastCollapsedChamber", 10, 0, 8, 8);
                Floor("NorthGallery", 0, 3.2f, 5, 8);
                Floor("NorthSealedRoom", 0, 9.5f, 9, 4.5f);
                BuildWallsAndProps();

                var player = (GameObject)PrefabUtility.InstantiatePrefab(playerPrefab, dungeon);
                player.name = "Player";
                player.transform.position = new Vector3(0, -8.5f, 0);
                var cameraObject = new GameObject("Main Camera", typeof(Camera), typeof(AudioListener), typeof(CameraFollow));
                cameraObject.tag = "MainCamera";
                cameraObject.transform.position = new Vector3(0, -8.5f, -10);
                var camera = cameraObject.GetComponent<Camera>();
                camera.orthographic = true;
                camera.orthographicSize = 5;
                camera.clearFlags = CameraClearFlags.SolidColor;
                camera.backgroundColor = new Color(.10f, .11f, .11f);
                cameraObject.GetComponent<CameraFollow>().SetTarget(player.transform);
                WorldBoundaryGeneratorUtility.Create("WorldBounds", Vector2.zero, new Vector2(33, 25), 1);

                // Spawn과 Trigger 사이 3.4유닛을 확보해 Field03에서 내려온 즉시 되돌아가지 않습니다.
                var spawn = new GameObject("Spawn_From_Field03", typeof(SceneSpawnPoint));
                spawn.transform.position = new Vector3(0, -8.5f, 0);
                spawn.GetComponent<SceneSpawnPoint>().Configure("Spawn_From_Field03");
                var exit = new GameObject("Exit_To_Field03", typeof(BoxCollider2D), typeof(SceneTransitionTrigger));
                exit.transform.position = new Vector3(0, -11.9f, 0);
                exit.GetComponent<BoxCollider2D>().size = new Vector2(3, .8f);
                exit.GetComponent<BoxCollider2D>().isTrigger = true;
                exit.GetComponent<SceneTransitionTrigger>().Configure("Field_03", "Spawn_From_Dungeon01");

                EditorSceneManager.SaveScene(dungeon, ScenePath);
            }
            finally
            {
                SceneManager.SetActiveScene(previous);
                EditorSceneManager.CloseScene(dungeon, true);
            }

            CreateSpawn("Dungeon01_B1_01", "dungeon01_b1_01", wight, new Vector2(0, -3), .55f);
            CreateSpawn("Dungeon01_B1_02", "dungeon01_b1_02", wight, new Vector2(-10, .2f), .55f);
            CreateSpawn("Dungeon01_B1_03", "dungeon01_b1_03", wight, new Vector2(10, .2f), .55f);
            CreateSpawn("Dungeon01_B1_04", "dungeon01_b1_04", wight, new Vector2(0, 5.3f), .55f);
            // Field03의 기존 연결 설치기는 Scene Spawn을 교체합니다. 같은 데이터 흐름으로 복귀 지점만 추가합니다.
            var returnSpawn = ScriptableObject.CreateInstance<FieldConnectionDefinition>();
            returnSpawn.ConfigureSpawnOnly("Field_03", "dungeon01_return", "Spawn_From_Dungeon01", new Vector2(.55f, -2.65f));
            AssetDatabase.CreateAsset(returnSpawn, Root + "FieldConnections/Field03_Dungeon01Return.asset");
            var scenes = EditorBuildSettings.scenes.ToList();
            if (scenes.All(item => item.path != ScenePath))
            {
                scenes.Add(new EditorBuildSettingsScene(ScenePath, true));
                EditorBuildSettings.scenes = scenes.ToArray();
            }
            AssetDatabase.SaveAssets();
            Debug.Log("Dungeon_01 B1 Scene, 4개 일반 조우, Field03 복귀 Spawn을 설치했습니다.");
        }

        static void CreateSpawn(string assetName, string id, MonsterDefinition monster, Vector2 position, float radius)
        {
            var spawn = ScriptableObject.CreateInstance<FieldMonsterSpawnDefinition>();
            spawn.Configure("Dungeon_01", id, monster, position, radius, 35);
            AssetDatabase.CreateAsset(spawn, Root + "MonsterSpawns/" + assetName + ".asset");
        }

        static void Floor(string name, float x, float y, float width, float height)
        {
            Rect(name, x, y, width, height, new Color(.70f, .68f, .63f), -10);
            // 반복된 줄눈이 색에만 의존하지 않고 돌바닥의 방향을 보여줍니다.
            for (float line = x - width / 2 + 1; line < x + width / 2; line += 2)
                Rect(name + "_Joint", line, y, .035f, height, new Color(.42f, .42f, .40f), -9);
        }

        static void BuildWallsAndProps()
        {
            // 방의 입구는 최소 3유닛 이상 열어 둡니다. 정밀한 한 칸 이동을 요구하지 않습니다.
            Wall("EntryWest", -4.5f, -9, .35f, 5);
            Wall("EntryEast", 4.5f, -9, .35f, 5);
            Wall("EntryNorthWest", -3.3f, -6.5f, 2.4f, .35f);
            Wall("EntryNorthEast", 3.3f, -6.5f, 2.4f, .35f);
            Wall("WestOuter", -14, .1f, .4f, 8);
            Wall("WestTop", -10, 4.1f, 8, .4f);
            Wall("EastOuter", 14, .1f, .4f, 8);
            Wall("EastTop", 10, 4.1f, 8, .4f);
            Wall("NorthWest", -2.7f, 5.6f, .4f, 5);
            Wall("NorthEast", 2.7f, 5.6f, .4f, 5);
            Wall("SealedDescent", 0, 11.2f, 4.5f, .7f);
            Rect("SealedDescent_Step", 0, 10.4f, 4, .6f, new Color(.15f, .15f, .15f), 1);
            // 나중 Quest 대상은 이 이름을 stable 위치로 사용할 수 있습니다. 지금은 상호작용을 붙이지 않습니다.
            var descent = new GameObject("dungeon01_b1_lower_descent");
            descent.transform.SetParent(environment, false);
            descent.transform.position = new Vector3(0, 10.5f, 0);
            for (int i = 0; i < 4; i++)
            {
                Rect("WestNiche", -13.1f, -2.5f + i * 1.6f, 1.25f, .7f, new Color(.42f, .4f, .36f), 0);
                Rect("WestNicheLid", -13.1f, -2.5f + i * 1.6f, 1.35f, .12f, new Color(.52f, .49f, .43f), 1);
            }
            Wall("EastRubbleA", 12.5f, 2.6f, 1.5f, .7f);
            Wall("EastRubbleB", 13.1f, 1.8f, .8f, .6f);
            Rect("TorchBracketWest", -2, -8.5f, .2f, .6f, new Color(.43f, .36f, .27f), 1);
            Rect("TorchBracketEast", 2, -8.5f, .2f, .6f, new Color(.43f, .36f, .27f), 1);
        }

        static void Wall(string name, float x, float y, float width, float height)
        {
            var wall = Rect(name, x, y, width, height, new Color(.83f, .79f, .72f), 2);
            // Sprite 원래 크기에 Transform 배율이 적용되어 화면에 보이는 벽과 충돌 영역이 일치합니다.
            wall.AddComponent<BoxCollider2D>().size = stoneSprite.bounds.size;
        }

        static GameObject Rect(string name, float x, float y, float width, float height, Color color, int order)
        {
            var tile = new GameObject(name, typeof(SpriteRenderer));
            tile.transform.SetParent(environment, false);
            tile.transform.position = new Vector3(x, y, 0);
            tile.transform.localScale = new Vector3(width / stoneSprite.bounds.size.x, height / stoneSprite.bounds.size.y, 1);
            var renderer = tile.GetComponent<SpriteRenderer>();
            renderer.sprite = stoneSprite;
            renderer.color = color;
            renderer.sortingOrder = order;
            return tile;
        }
    }
}
#endif

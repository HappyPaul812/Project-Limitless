using System;
using System.Collections.Generic;
using System.Linq;
using ProjectLimitless.CameraSystem;
using ProjectLimitless.Player;
using ProjectLimitless.World;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectLimitless.Editor
{
    /// <summary>Milestone 02의 첫 야외 필드만 생성하며 기존 Starter Village Scene은 수정하지 않습니다.</summary>
    public static class Field01SceneGenerator
    {
        private const string FieldScenePath = "Assets/_Project/Scenes/Field_01.unity";
        private const string VillageScenePath = "Assets/_Project/Scenes/World_StarterVillage.unity";
        private const string PlayerPrefabPath = "Assets/_Project/Prefabs/PlayerPlaceholder.prefab";
        private const string BasicRoot = "Assets/ThirdParty/Schwarnhild/BasicHandDrawn/";
        private const string GrassTilesPath = BasicRoot + "tiles/tiles_grass.png";
        private const string FenceTilesPath = BasicRoot + "tiles/fence_tiles.png";

        [MenuItem("Project-Limitless/Milestone 02/Generate Field 01")]
        public static void GenerateField01()
        {
            GameObject playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(PlayerPrefabPath);
            if (playerPrefab == null) throw new InvalidOperationException($"Player Prefab을 찾지 못했습니다: {PlayerPrefabPath}");

            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            CreateFieldEnvironment();
            GameObject player = (GameObject)PrefabUtility.InstantiatePrefab(playerPrefab, scene);
            player.name = "Player";
            player.transform.position = new Vector3(0f, -4.7f, 0f);
            CreateCamera(player.transform);
            CreateSpawnAndTransitions();
            CreateBoundaries();

            EditorSceneManager.SaveScene(scene, FieldScenePath);
            AddFieldToBuildSettings();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Milestone 02 Field_01 생성 및 Build Settings 등록을 완료했습니다. Starter Village Scene은 변경하지 않았습니다.");
        }

        public static void ValidateField01()
        {
            Scene scene = EditorSceneManager.OpenScene(FieldScenePath, OpenSceneMode.Single);
            GameObject[] roots = scene.GetRootGameObjects();
            T Find<T>() where T : Component => roots.SelectMany(root => root.GetComponentsInChildren<T>(true)).FirstOrDefault();
            GameObject FindObject(string name) => roots.SelectMany(root => root.GetComponentsInChildren<Transform>(true)).Select(item => item.gameObject).FirstOrDefault(item => item.name == name);

            PlayerController player = Find<PlayerController>();
            if (player == null || player.GetComponent<PlayerVisualController>() == null || player.GetComponent<PlayerNameplate>() == null)
                throw new InvalidOperationException("Field_01 Player에 이동, 외형 또는 Nameplate Component가 누락됐습니다.");
            if (Find<CameraFollow>() == null || Camera.main == null)
                throw new InvalidOperationException("Field_01 Main Camera 또는 CameraFollow가 누락됐습니다.");
            if (FindObject("Spawn_From_StarterVillage")?.GetComponent<SceneSpawnPoint>() == null)
                throw new InvalidOperationException("Field_01 마을 진입 Spawn Point가 누락됐습니다.");
            if (FindObject("Entrance_To_StarterVillage")?.GetComponent<SceneTransitionTrigger>() == null)
                throw new InvalidOperationException("Field_01 마을 복귀 Trigger가 누락됐습니다.");
            if (roots.SelectMany(root => root.GetComponentsInChildren<Collider2D>(true)).Count() < 10)
                throw new InvalidOperationException("Field_01 장애물 Collider가 예상보다 적습니다.");

            int missingScripts = roots.Sum(GameObjectUtility.GetMonoBehavioursWithMissingScriptCount);
            if (missingScripts != 0) throw new InvalidOperationException($"Field_01 Missing Script 수: {missingScripts}");

            string[] buildPaths = EditorBuildSettings.scenes.Where(item => item.enabled).Select(item => item.path).ToArray();
            int villageIndex = Array.IndexOf(buildPaths, VillageScenePath);
            int fieldIndex = Array.IndexOf(buildPaths, FieldScenePath);
            if (villageIndex < 0 || fieldIndex != villageIndex + 1)
                throw new InvalidOperationException("Build Settings에서 Field_01이 Starter Village 바로 뒤에 있지 않습니다.");

            Debug.Log($"Field_01 검증 완료: Player/Visual/Nameplate/Camera/Spawn/Transition 정상, Collider {roots.SelectMany(root => root.GetComponentsInChildren<Collider2D>(true)).Count()}개, Missing Script 0개.");
        }

        private static void CreateFieldEnvironment()
        {
            GameObject root = new GameObject("FieldEnvironment");
            CreateGround(root.transform);
            CreateRoad(root.transform);

            CreateBasic(root.transform, "TreeNorthWest", "assets/tree_big.png", new Vector2(-8f, 5f), 4, new Vector2(.5f, .3f), new Vector2(0f, -.75f));
            CreateBasic(root.transform, "TreeNorthEast", "assets/tree_medium.png", new Vector2(7.5f, 5.2f), 4, new Vector2(.45f, .28f), new Vector2(0f, -.55f));
            CreateBasic(root.transform, "TreeWest", "assets/tree_medium.png", new Vector2(-8.5f, 0f), 4, new Vector2(.45f, .28f), new Vector2(0f, -.55f));
            CreateBasic(root.transform, "TreeEast", "assets/tree_big.png", new Vector2(8.5f, -.5f), 4, new Vector2(.5f, .3f), new Vector2(0f, -.75f));
            CreateBasic(root.transform, "TreeSouthWest", "assets/tree_medium.png", new Vector2(-7.5f, -5f), 4, new Vector2(.45f, .28f), new Vector2(0f, -.55f));
            CreateBasic(root.transform, "BushWest", "assets/bush_01.png", new Vector2(-5.5f, 2.2f), 2);
            CreateBasic(root.transform, "BushEast", "assets/bush_02.png", new Vector2(5.7f, 2.8f), 2);
            CreateBasic(root.transform, "BushSouth", "assets/bush_01.png", new Vector2(4.8f, -4.4f), 2);
            CreateBasic(root.transform, "RockWest", "assets/rock_01.png", new Vector2(-5.8f, -2.2f), 3, new Vector2(.45f, .25f), new Vector2(0f, -.25f));
            CreateBasic(root.transform, "RockNorth", "assets/rock_02.png", new Vector2(4.5f, 4.4f), 3, new Vector2(.4f, .22f), new Vector2(0f, -.22f));

            foreach (float x in new[] { -4f, -3f, 3f, 4f }) CreateFence(root.transform, new Vector2(x, -5.7f));
            foreach (float x in new[] { -6f, -5f, 5f, 6f }) CreateFence(root.transform, new Vector2(x, 5.8f));
        }

        private static void CreateGround(Transform parent)
        {
            string[] grass = { "tiles_grass_4_0", "tiles_grass_5_0", "tiles_grass_6_0" };
            for (int x = -10; x <= 10; x++)
            for (int y = -7; y <= 7; y++)
            {
                int variant = Math.Abs((x * 7) + (y * 11)) % grass.Length;
                GameObject tile = CreateGrid(parent, $"Grass_{x}_{y}", GrassTilesPath, grass[variant], new Vector2(x, y), -10);
                tile.GetComponent<SpriteRenderer>().flipX = ((x + y) & 1) == 0;
            }
        }

        private static void CreateRoad(Transform parent)
        {
            int[] roadCenter = { 0, 0, -1, -1, 0, 0, 1, 1, 1, 0, 0, -1, -1, 0, 0 };
            for (int index = 0; index < roadCenter.Length; index++)
            {
                int y = index - 7;
                int x = roadCenter[index];
                CreateGrid(parent, $"RoadLeft_{y}", GrassTilesPath, "tiles_grass_4_4", new Vector2(x - .5f, y), -8);
                CreateGrid(parent, $"RoadRight_{y}", GrassTilesPath, "tiles_grass_5_4", new Vector2(x + .5f, y), -8);
            }
        }

        private static void CreateSpawnAndTransitions()
        {
            GameObject spawn = new GameObject("Spawn_From_StarterVillage");
            spawn.transform.position = new Vector3(0f, -4.7f, 0f);
            spawn.AddComponent<SceneSpawnPoint>().Configure("Spawn_From_StarterVillage");

            GameObject returnTrigger = new GameObject("Entrance_To_StarterVillage", typeof(BoxCollider2D));
            returnTrigger.transform.position = new Vector3(0f, -6.65f, 0f);
            BoxCollider2D trigger = returnTrigger.GetComponent<BoxCollider2D>();
            trigger.size = new Vector2(2.5f, .9f);
            trigger.isTrigger = true;
            returnTrigger.AddComponent<SceneTransitionTrigger>().Configure("World_StarterVillage", "Spawn_From_Field01");

            GameObject futureExit = new GameObject("FutureNorthExit_TODO");
            futureExit.transform.position = new Vector3(0f, 6.6f, 0f);
        }

        private static void CreateCamera(Transform target)
        {
            GameObject cameraObject = new GameObject("Main Camera", typeof(Camera), typeof(AudioListener), typeof(CameraFollow));
            cameraObject.tag = "MainCamera";
            cameraObject.transform.position = new Vector3(target.position.x, target.position.y, -10f);
            Camera camera = cameraObject.GetComponent<Camera>();
            camera.orthographic = true;
            camera.orthographicSize = 5f;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(.14f, .25f, .15f, 1f);
            cameraObject.GetComponent<CameraFollow>().SetTarget(target);
        }

        private static void CreateBoundaries()
        {
            CreateBoundary("BoundaryLeft", new Vector2(-10.7f, 0f), new Vector2(1f, 16f));
            CreateBoundary("BoundaryRight", new Vector2(10.7f, 0f), new Vector2(1f, 16f));
            CreateBoundary("BoundaryTop", new Vector2(0f, 7.7f), new Vector2(22f, 1f));
            CreateBoundary("BoundaryBottomLeft", new Vector2(-6.5f, -7.7f), new Vector2(9f, 1f));
            CreateBoundary("BoundaryBottomRight", new Vector2(6.5f, -7.7f), new Vector2(9f, 1f));
        }

        private static void CreateBoundary(string name, Vector2 position, Vector2 size)
        {
            GameObject boundary = new GameObject(name, typeof(BoxCollider2D));
            boundary.transform.position = position;
            boundary.GetComponent<BoxCollider2D>().size = size;
        }

        private static GameObject CreateBasic(Transform parent, string name, string relativePath, Vector2 position, int order, Vector2? colliderSize = null, Vector2? colliderOffset = null)
            => CreateSprite(parent, name, AssetDatabase.LoadAssetAtPath<Sprite>(BasicRoot + relativePath), position, order, colliderSize, colliderOffset);

        private static GameObject CreateFence(Transform parent, Vector2 position)
            => CreateGrid(parent, $"Fence_{position.x}_{position.y}", FenceTilesPath, "fence_tiles_2_2", position, 2, new Vector2(.9f, .2f), new Vector2(0f, -.35f));

        private static GameObject CreateGrid(Transform parent, string name, string assetPath, string spriteName, Vector2 position, int order, Vector2? colliderSize = null, Vector2? colliderOffset = null)
        {
            Sprite sprite = AssetDatabase.LoadAllAssetsAtPath(assetPath).OfType<Sprite>().FirstOrDefault(item => item.name == spriteName);
            return CreateSprite(parent, name, sprite, position, order, colliderSize, colliderOffset);
        }

        private static GameObject CreateSprite(Transform parent, string name, Sprite sprite, Vector2 position, int order, Vector2? colliderSize = null, Vector2? colliderOffset = null)
        {
            if (sprite == null) throw new InvalidOperationException($"Field Sprite를 찾지 못했습니다: {name}");
            GameObject result = new GameObject(name, typeof(SpriteRenderer));
            result.transform.SetParent(parent);
            result.transform.position = position;
            SpriteRenderer renderer = result.GetComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sortingOrder = order;
            if (colliderSize.HasValue)
            {
                BoxCollider2D collider = result.AddComponent<BoxCollider2D>();
                collider.size = colliderSize.Value;
                collider.offset = colliderOffset ?? Vector2.zero;
            }
            return result;
        }

        private static void AddFieldToBuildSettings()
        {
            List<EditorBuildSettingsScene> scenes = EditorBuildSettings.scenes.ToList();
            scenes.RemoveAll(item => item.path == FieldScenePath);
            int villageIndex = scenes.FindIndex(item => item.path == VillageScenePath);
            int insertIndex = villageIndex >= 0 ? villageIndex + 1 : scenes.Count;
            scenes.Insert(insertIndex, new EditorBuildSettingsScene(FieldScenePath, true));
            EditorBuildSettings.scenes = scenes.ToArray();
        }
    }
}

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
    /// <summary>
    /// Unity Editor 메뉴에서 Milestone 02의 첫 야외 필드 Scene과 마을 남문 Prefab을 자동 생성합니다.
    /// 게임 실행 중에는 사용되지 않으며, 기존 StarterVillage Scene Asset은 직접 수정하지 않습니다.
    /// </summary>
    public static class Field01SceneGenerator
    {
        // 생성·검증·Build Settings 등록에 사용하는 프로젝트 전용 Asset 경로입니다.
        private const string FieldScenePath = "Assets/_Project/Scenes/Field_01.unity";
        private const string VillageScenePath = "Assets/_Project/Scenes/World_StarterVillage.unity";
        private const string PlayerPrefabPath = "Assets/_Project/Prefabs/PlayerPlaceholder.prefab";
        private const string VillageGatePrefabPath = "Assets/_Project/Resources/World/StarterVillageSouthGate.prefab";
        // Field 환경을 그릴 ThirdParty Sprite 원본 위치입니다. 원본 Asset 자체는 수정하지 않습니다.
        private const string BasicRoot = "Assets/ThirdParty/Schwarnhild/BasicHandDrawn/";
        private const string GrassTilesPath = BasicRoot + "tiles/tiles_grass.png";
        private const string FenceTilesPath = BasicRoot + "tiles/fence_tiles.png";

        [MenuItem("Project-Limitless/Milestone 02/Generate Field 01")]
        /// <summary>
        /// Unity Editor 메뉴를 선택할 때 호출되어 Field_01의 환경, Player, Camera, 출입구와 경계를 생성합니다.
        /// 완성된 Scene을 저장하고 Build Settings에서 StarterVillage 바로 다음 순서로 등록합니다.
        /// </summary>
        public static void GenerateField01()
        {
            GameObject playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(PlayerPrefabPath);
            if (playerPrefab == null) throw new InvalidOperationException($"Player Prefab을 찾지 못했습니다: {PlayerPrefabPath}");

            CreateStarterVillageSouthGatePrefab();

            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            CreateFieldEnvironment();
            GameObject player = (GameObject)PrefabUtility.InstantiatePrefab(playerPrefab, scene);
            player.name = "Player";
            player.transform.position = new Vector3(0f, 4.7f, 0f);
            CreateCamera(player.transform);
            CreateSpawnAndTransitions();
            CreateBoundaries();

            EditorSceneManager.SaveScene(scene, FieldScenePath);
            AddFieldToBuildSettings();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Milestone 02 Field_01 생성 및 Build Settings 등록을 완료했습니다. Starter Village Scene은 변경하지 않았습니다.");
        }

        /// <summary>
        /// 생성된 Field_01을 열어 Player 외형, Camera, Bounds, 출입구, Collider와 Missing Script를 검사합니다.
        /// 필수 구성이 빠졌으면 예외를 발생시켜 잘못된 Scene이 정상 결과처럼 사용되지 않게 합니다.
        /// </summary>
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
            WorldBounds2D worldBounds = Find<WorldBounds2D>();
            if (worldBounds == null || worldBounds.Bounds.size != new Vector3(21f, 15f, 0f))
                throw new InvalidOperationException("Field_01 WorldBounds2D가 누락됐거나 크기가 올바르지 않습니다.");
            if (FindObject("Spawn_From_StarterVillage")?.GetComponent<SceneSpawnPoint>() == null)
                throw new InvalidOperationException("Field_01 마을 진입 Spawn Point가 누락됐습니다.");
            if (FindObject("Entrance_To_StarterVillage")?.GetComponent<SceneTransitionTrigger>() == null)
                throw new InvalidOperationException("Field_01 마을 복귀 Trigger가 누락됐습니다.");
            if (FindObject("Spawn_From_StarterVillage").transform.position.y <= 0f || FindObject("Entrance_To_StarterVillage").transform.position.y <= 0f)
                throw new InvalidOperationException("Field_01 마을 Spawn과 복귀 Trigger가 북쪽에 있지 않습니다.");
            if (roots.SelectMany(root => root.GetComponentsInChildren<Collider2D>(true)).Count() < 10)
                throw new InvalidOperationException("Field_01 장애물 Collider가 예상보다 적습니다.");

            int missingScripts = roots.Sum(GameObjectUtility.GetMonoBehavioursWithMissingScriptCount);
            if (missingScripts != 0) throw new InvalidOperationException($"Field_01 Missing Script 수: {missingScripts}");

            GameObject gatePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(VillageGatePrefabPath);
            if (gatePrefab == null || gatePrefab.GetComponentInChildren<SceneSpawnPoint>(true) == null || gatePrefab.GetComponentInChildren<SceneTransitionTrigger>(true) == null)
                throw new InvalidOperationException("Starter Village 남문 Prefab 또는 Spawn/Transition이 누락됐습니다.");
            if (gatePrefab.GetComponentInChildren<TextMesh>(true) != null)
                throw new InvalidOperationException("Starter Village 남문에 글씨 기반 출구 표식이 남아 있습니다.");
            if (gatePrefab.GetComponentsInChildren<Collider2D>(true).Length < 10 || GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(gatePrefab) != 0)
                throw new InvalidOperationException("Starter Village 남문 Collider 또는 Script 구성이 올바르지 않습니다.");

            string[] buildPaths = EditorBuildSettings.scenes.Where(item => item.enabled).Select(item => item.path).ToArray();
            int villageIndex = Array.IndexOf(buildPaths, VillageScenePath);
            int fieldIndex = Array.IndexOf(buildPaths, FieldScenePath);
            if (villageIndex < 0 || fieldIndex != villageIndex + 1)
                throw new InvalidOperationException("Build Settings에서 Field_01이 Starter Village 바로 뒤에 있지 않습니다.");

            Debug.Log($"Field_01 검증 완료: Player/Visual/Nameplate/Camera/Spawn/Transition 정상, Collider {roots.SelectMany(root => root.GetComponentsInChildren<Collider2D>(true)).Count()}개, Missing Script 0개.");
        }

        /// <summary>필드의 바닥, 길, 나무와 소품을 FieldEnvironment 아래에 배치합니다.</summary>
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

            foreach (float x in new[] { -4f, -3f, 3f, 4f }) CreateFence(root.transform, new Vector2(x, 5.7f));
            foreach (float x in new[] { -6f, -5f, 5f, 6f }) CreateFence(root.transform, new Vector2(x, -5.8f));
        }

        /// <summary>플레이 영역 전체를 덮는 잔디 Tile을 좌표별로 생성합니다.</summary>
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

        /// <summary>북쪽 마을 입구에서 남쪽까지 이어지는 굽은 길을 Tile로 배치합니다.</summary>
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

        /// <summary>
        /// 마을에서 들어올 때의 Spawn Point와 북쪽 마을 복귀 Trigger를 서로 떨어뜨려 배치합니다.
        /// 이렇게 해야 Scene 진입 직후 같은 Trigger에 닿아 이전 Scene으로 되돌아가는 일을 막을 수 있습니다.
        /// </summary>
        private static void CreateSpawnAndTransitions()
        {
            GameObject spawn = new GameObject("Spawn_From_StarterVillage");
            spawn.transform.position = new Vector3(0f, 4.7f, 0f);
            spawn.AddComponent<SceneSpawnPoint>().Configure("Spawn_From_StarterVillage");

            GameObject returnTrigger = new GameObject("Entrance_To_StarterVillage", typeof(BoxCollider2D));
            returnTrigger.transform.position = new Vector3(0f, 6.65f, 0f);
            BoxCollider2D trigger = returnTrigger.GetComponent<BoxCollider2D>();
            trigger.size = new Vector2(2.5f, .9f);
            trigger.isTrigger = true;
            returnTrigger.AddComponent<SceneTransitionTrigger>().Configure("World_StarterVillage", "Spawn_From_Field01");

            GameObject futureExit = new GameObject("FutureSouthExit_TODO");
            futureExit.transform.position = new Vector3(0f, -6.6f, 0f);
        }

        /// <summary>Player를 따라가는 Orthographic Main Camera를 생성하고 추적 대상을 연결합니다.</summary>
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

        /// <summary>Field Tile 크기의 WorldBounds와 북쪽 중앙 입구를 제외한 네 면 Collider를 생성합니다.</summary>
        private static void CreateBoundaries()
        {
            WorldBoundaryGeneratorUtility.Create(
                "WorldBounds",
                Vector2.zero,
                new Vector2(21f, 15f),
                1f,
                new WorldBoundaryOpening(WorldBoundarySide.Top, 0f, 4f));
        }

        /// <summary>
        /// StarterVillage에서 런타임에 설치할 남문 Prefab을 생성합니다.
        /// 중앙 통로만 비운 울타리, Field 복귀 Spawn, Field_01 전환 Trigger를 함께 저장합니다.
        /// </summary>
        private static void CreateStarterVillageSouthGatePrefab()
        {
            EnsureAssetFolder("Assets/_Project/Resources/World");
            GameObject root = new GameObject("StarterVillageSouthGate");

            for (int x = -9; x <= 9; x++)
            {
                if (x >= -1 && x <= 1) continue;
                CreateFence(root.transform, new Vector2(x, -5.7f));
            }

            GameObject spawn = new GameObject("Spawn_From_Field01");
            spawn.transform.SetParent(root.transform);
            spawn.transform.position = new Vector3(0f, -4.65f, 0f);
            spawn.AddComponent<SceneSpawnPoint>().Configure("Spawn_From_Field01");

            GameObject exit = new GameObject("Exit_To_Field01", typeof(BoxCollider2D));
            exit.transform.SetParent(root.transform);
            exit.transform.position = new Vector3(0f, -6.5f, 0f);
            BoxCollider2D collider = exit.GetComponent<BoxCollider2D>();
            collider.size = new Vector2(2.5f, .9f);
            collider.isTrigger = true;
            exit.AddComponent<SceneTransitionTrigger>().Configure("Field_01", "Spawn_From_StarterVillage");

            PrefabUtility.SaveAsPrefabAsset(root, VillageGatePrefabPath);
            UnityEngine.Object.DestroyImmediate(root);
        }

        /// <summary>Prefab 저장 경로의 폴더가 없을 때 상위 폴더부터 순서대로 만듭니다.</summary>
        private static void EnsureAssetFolder(string path)
        {
            string current = "Assets";
            foreach (string part in path.Split('/').Skip(1))
            {
                string next = current + "/" + part;
                if (!AssetDatabase.IsValidFolder(next)) AssetDatabase.CreateFolder(current, part);
                current = next;
            }
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

        /// <summary>Field_01을 중복 없이 StarterVillage 바로 다음 Build 순서에 등록합니다.</summary>
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

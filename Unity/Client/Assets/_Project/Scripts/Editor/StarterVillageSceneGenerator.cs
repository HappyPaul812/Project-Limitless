#if UNITY_EDITOR
using System.Collections.Generic;
using ProjectLimitless.CameraSystem;
using ProjectLimitless.Core;
using ProjectLimitless.NPC;
using ProjectLimitless.Player;
using ProjectLimitless.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectLimitless.EditorTools
{
    /// <summary>Unity Editor에서 Milestone 01의 실제 Scene Asset을 생성한다.</summary>
    public static class StarterVillageSceneGenerator
    {
        private const string BootstrapScenePath = "Assets/_Project/Scenes/Bootstrap.unity";
        private const string WorldScenePath = "Assets/_Project/Scenes/World_StarterVillage.unity";
        private const string PlayerPrefabPath = "Assets/_Project/Prefabs/PlayerPlaceholder.prefab";
        private const string NpcPrefabPath = "Assets/_Project/Prefabs/VillageNpcPlaceholder.prefab";
        private const string KenneyRoot = "Assets/ThirdParty/Kenney/RPGBase/PNG/";

        [MenuItem("Project-Limitless/Milestone 01/Generate Scenes")]
        private static void GenerateScenes()
        {
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(BootstrapScenePath) != null ||
                AssetDatabase.LoadAssetAtPath<SceneAsset>(WorldScenePath) != null ||
                AssetDatabase.LoadAssetAtPath<GameObject>(PlayerPrefabPath) != null ||
                AssetDatabase.LoadAssetAtPath<GameObject>(NpcPrefabPath) != null)
            {
                Debug.LogError("Milestone 01 Scene 또는 Prefab이 이미 있습니다. 기존 Asset을 덮어쓰지 않았습니다.");
                return;
            }

            ConfigureKenneyImportSettings();
            CreatePlaceholderPrefabs(out GameObject playerPrefab, out GameObject npcPrefab);
            CreateBootstrapScene();
            CreateStarterVillageScene(playerPrefab, npcPrefab);
            ConfigureBuildSettings();
            AssetDatabase.SaveAssets();
            Debug.Log("Milestone 01 Scene 생성 완료: Bootstrap, World_StarterVillage");
        }

        private static void CreateBootstrapScene()
        {
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            new GameObject("Bootstrap").AddComponent<BootstrapLoader>();
            EditorSceneManager.SaveScene(scene, BootstrapScenePath);
        }

        private static void CreateStarterVillageScene(GameObject playerPrefab, GameObject npcPrefab)
        {
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            CreateCamera(out CameraFollow cameraFollow);
            CreateKenneyStarterVillage();

            GameObject player = (GameObject)PrefabUtility.InstantiatePrefab(playerPrefab);
            player.name = "Player";
            player.transform.position = new Vector2(0f, -5f);

            GameObject npc = (GameObject)PrefabUtility.InstantiatePrefab(npcPrefab);
            npc.name = "VillageNpc";
            npc.transform.position = new Vector2(1.5f, -1.5f);

            new GameObject("DialogueSystem").AddComponent<DialoguePresenter>();
            cameraFollow.SetTarget(player.transform);
            EditorSceneManager.SaveScene(scene, WorldScenePath);
        }

        [MenuItem("Project-Limitless/Milestone 01/Apply Kenney Village Environment")]
        private static void ApplyKenneyVillageEnvironment()
        {
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(WorldScenePath) == null)
            {
                Debug.LogError("World_StarterVillage Scene을 먼저 생성해야 합니다.");
                return;
            }

            ConfigureKenneyImportSettings();
            Scene scene = EditorSceneManager.OpenScene(WorldScenePath, OpenSceneMode.Single);
            GameObject existingEnvironment = GameObject.Find("KenneyStarterVillage");
            if (existingEnvironment != null)
            {
                Object.DestroyImmediate(existingEnvironment);
            }

            string[] legacyNames = { "VillageFloor", "BoundaryTop", "BoundaryBottom", "BoundaryLeft", "BoundaryRight", "BuildingNorthWest", "BuildingNorthEast", "BuildingSouthEast", "Well", "Crate" };
            foreach (string legacyName in legacyNames)
            {
                GameObject legacyObject = GameObject.Find(legacyName);
                if (legacyObject != null)
                {
                    Object.DestroyImmediate(legacyObject);
                }
            }

            CreateKenneyStarterVillage();
            EditorSceneManager.SaveScene(scene);
            AssetDatabase.SaveAssets();
            Debug.Log("Kenney RPG Base 환경을 World_StarterVillage에 적용했습니다.");
        }

        private static void CreatePlaceholderPrefabs(out GameObject playerPrefab, out GameObject npcPrefab)
        {
            GameObject playerSource = CreatePlaceholder("PlayerPlaceholder", Vector2.zero, new Vector2(0.7f, 0.9f), new Color(0.2f, 0.65f, 1f), "플레이어", 5, false);
            Rigidbody2D body = playerSource.AddComponent<Rigidbody2D>();
            body.gravityScale = 0f;
            body.freezeRotation = true;
            playerSource.AddComponent<CircleCollider2D>().radius = 0.5f;
            playerSource.AddComponent<PlayerController>();
            playerSource.AddComponent<InteractionSystem>();
            playerPrefab = PrefabUtility.SaveAsPrefabAsset(playerSource, PlayerPrefabPath);
            Object.DestroyImmediate(playerSource);

            GameObject npcSource = CreatePlaceholder("VillageNpcPlaceholder", Vector2.zero, new Vector2(0.7f, 0.9f), new Color(1f, 0.75f, 0.2f), "마을 주민", 5, false);
            npcSource.AddComponent<CircleCollider2D>().isTrigger = true;
            npcSource.AddComponent<NpcController>().Configure("마을 주민", "어서 오세요. 여기는 우리의 첫 번째 마을입니다.");
            npcSource.AddComponent<NpcInteractionPrompt>();
            npcPrefab = PrefabUtility.SaveAsPrefabAsset(npcSource, NpcPrefabPath);
            Object.DestroyImmediate(npcSource);
        }

        private static void CreateCamera(out CameraFollow cameraFollow)
        {
            GameObject cameraObject = new GameObject("Main Camera");
            cameraObject.tag = "MainCamera";
            cameraObject.transform.position = new Vector3(0f, 0f, -10f);
            Camera camera = cameraObject.AddComponent<Camera>();
            camera.orthographic = true;
            camera.orthographicSize = 6f;
            camera.backgroundColor = new Color(0.08f, 0.12f, 0.18f);
            cameraObject.AddComponent<AudioListener>();
            cameraFollow = cameraObject.AddComponent<CameraFollow>();
        }

        private static void CreateKenneyStarterVillage()
        {
            GameObject root = new GameObject("KenneyStarterVillage");
            CreateGround(root.transform);
            CreatePath(root.transform);
            CreateHouse(root.transform, "HouseNorthWest", new Vector2(-7f, 4f));
            CreateHouse(root.transform, "HouseNorthEast", new Vector2(7f, 4f));
            CreateKenneySprite(root.transform, "Well", "rpgTile184.png", new Vector2(0f, 1.75f), 4, new Vector2(0.55f, 0.3f), new Vector2(0f, -0.2f));
            CreateKenneySprite(root.transform, "Crate", "rpgTile163.png", new Vector2(-3f, -1.5f), 4, new Vector2(0.6f, 0.5f), new Vector2(0f, -0.2f));
            CreateTree(root.transform, "TreeWest", "rpgTile195.png", new Vector2(-10.5f, 0.5f));
            CreateTree(root.transform, "TreeEast", "rpgTile197.png", new Vector2(10.5f, 0.5f));
            CreateTree(root.transform, "TreeSouthWest", "rpgTile200.png", new Vector2(-10f, -5.5f));
            CreateFence(root.transform, new Vector2(-12.5f, 7.5f), "rpgTile181.png");
            CreateFence(root.transform, new Vector2(-11.5f, 7.5f), "rpgTile182.png");
            CreateFence(root.transform, new Vector2(11.5f, 7.5f), "rpgTile215.png");
            CreateFence(root.transform, new Vector2(12.5f, 7.5f), "rpgTile216.png");
        }

        private static void CreateGround(Transform parent)
        {
            for (int x = -13; x <= 13; x++)
            {
                for (int y = -8; y <= 8; y++)
                {
                    CreateKenneySprite(parent, $"Grass_{x}_{y}", "rpgTile003.png", new Vector2(x, y), -10);
                }
            }
        }

        private static void CreatePath(Transform parent)
        {
            for (int y = -8; y <= 1; y++)
            {
                CreateKenneySprite(parent, $"PathLeft_{y}", "rpgTile008.png", new Vector2(-0.5f, y), -8);
                CreateKenneySprite(parent, $"PathRight_{y}", "rpgTile008.png", new Vector2(0.5f, y), -8);
            }
        }

        private static void CreateHouse(Transform parent, string houseName, Vector2 position)
        {
            GameObject house = new GameObject(houseName);
            house.transform.SetParent(parent);
            CreateKenneySprite(house.transform, "RoofLeft", "rpgTile101.png", position + new Vector2(-1f, 1f), 2);
            CreateKenneySprite(house.transform, "RoofCenter", "rpgTile103.png", position + new Vector2(0f, 1f), 2);
            CreateKenneySprite(house.transform, "RoofRight", "rpgTile105.png", position + new Vector2(1f, 1f), 2);
            CreateKenneySprite(house.transform, "WallLeft", "rpgTile120.png", position + new Vector2(-1f, 0f), 2);
            CreateKenneySprite(house.transform, "WallCenter", "rpgTile122.png", position, 2, new Vector2(2.5f, 0.35f), new Vector2(0f, -0.25f));
            CreateKenneySprite(house.transform, "WallRight", "rpgTile124.png", position + new Vector2(1f, 0f), 2);
        }

        private static void CreateTree(Transform parent, string objectName, string fileName, Vector2 position)
        {
            CreateKenneySprite(parent, objectName, fileName, position, 3, new Vector2(0.3f, 0.25f), new Vector2(0f, -0.35f));
        }

        private static void CreateFence(Transform parent, Vector2 position, string fileName)
        {
            CreateKenneySprite(parent, $"Fence_{position.x}_{position.y}", fileName, position, 2, new Vector2(0.9f, 0.2f), new Vector2(0f, -0.2f));
        }

        private static GameObject CreateKenneySprite(Transform parent, string objectName, string fileName, Vector2 position, int sortingOrder, Vector2? colliderSize = null, Vector2? colliderOffset = null)
        {
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(KenneyRoot + fileName);
            if (sprite == null)
            {
                throw new System.InvalidOperationException($"Kenney Sprite를 찾지 못했습니다: {fileName}");
            }

            GameObject gameObject = new GameObject(objectName, typeof(SpriteRenderer));
            gameObject.transform.SetParent(parent);
            gameObject.transform.position = position;
            SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sortingOrder = sortingOrder;

            if (colliderSize.HasValue)
            {
                BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
                collider.size = colliderSize.Value;
                collider.offset = colliderOffset ?? Vector2.zero;
            }

            return gameObject;
        }

        private static void ConfigureKenneyImportSettings()
        {
            string[] guids = AssetDatabase.FindAssets("t:Texture2D", new[] { KenneyRoot.TrimEnd('/') });
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
                if (importer == null)
                {
                    continue;
                }

                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                importer.spritePixelsPerUnit = 64;
                importer.filterMode = FilterMode.Point;
                importer.textureCompression = TextureImporterCompression.Uncompressed;
                importer.mipmapEnabled = false;
                importer.SaveAndReimport();
            }
        }

        private static GameObject CreatePlaceholder(string objectName, Vector2 position, Vector2 size, Color color, string label, int order, bool addCollider)
        {
            GameObject gameObject = new GameObject(objectName);
            gameObject.transform.position = position;
            gameObject.AddComponent<SpriteRenderer>();
            gameObject.AddComponent<PlaceholderVisual>().Configure(color, size, label, order);

            if (addCollider)
            {
                BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
                collider.size = Vector2.one;
            }

            return gameObject;
        }

        private static void ConfigureBuildSettings()
        {
            List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>();
            scenes.Add(new EditorBuildSettingsScene(BootstrapScenePath, true));
            scenes.Add(new EditorBuildSettingsScene(WorldScenePath, true));

            foreach (EditorBuildSettingsScene existingScene in EditorBuildSettings.scenes)
            {
                if (existingScene.path != BootstrapScenePath && existingScene.path != WorldScenePath)
                {
                    scenes.Add(existingScene);
                }
            }

            EditorBuildSettings.scenes = scenes.ToArray();
        }
    }
}
#endif

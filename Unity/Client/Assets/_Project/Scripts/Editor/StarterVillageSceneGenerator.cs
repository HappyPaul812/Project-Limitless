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
            CreatePlaceholder("VillageFloor", Vector2.zero, new Vector2(28f, 18f), new Color(0.28f, 0.55f, 0.31f), "", -10, false);
            CreateVillageBoundary();
            CreateBuildingsAndObstacles();

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

        private static void CreateVillageBoundary()
        {
            Color wallColor = new Color(0.2f, 0.23f, 0.3f);
            CreatePlaceholder("BoundaryTop", new Vector2(0f, 9f), new Vector2(29f, 0.75f), wallColor, "마을 경계", 1, true);
            CreatePlaceholder("BoundaryBottom", new Vector2(0f, -9f), new Vector2(29f, 0.75f), wallColor, "", 1, true);
            CreatePlaceholder("BoundaryLeft", new Vector2(-14f, 0f), new Vector2(0.75f, 18f), wallColor, "", 1, true);
            CreatePlaceholder("BoundaryRight", new Vector2(14f, 0f), new Vector2(0.75f, 18f), wallColor, "", 1, true);
        }

        private static void CreateBuildingsAndObstacles()
        {
            Color buildingColor = new Color(0.65f, 0.34f, 0.2f);
            Color obstacleColor = new Color(0.38f, 0.26f, 0.15f);
            CreatePlaceholder("BuildingNorthWest", new Vector2(-7f, 4f), new Vector2(5f, 3f), buildingColor, "건물", 2, true);
            CreatePlaceholder("BuildingNorthEast", new Vector2(7f, 4.5f), new Vector2(4f, 4f), buildingColor, "건물", 2, true);
            CreatePlaceholder("BuildingSouthEast", new Vector2(8f, -4f), new Vector2(3.5f, 2.5f), buildingColor, "건물", 2, true);
            CreatePlaceholder("Well", new Vector2(-2f, 2f), new Vector2(1.5f, 1.5f), obstacleColor, "우물", 3, true);
            CreatePlaceholder("Crate", new Vector2(-5f, -3f), new Vector2(1f, 1f), obstacleColor, "상자", 3, true);
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

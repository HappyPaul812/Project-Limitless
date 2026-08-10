#if UNITY_EDITOR
using System.Collections.Generic;
using System;
using ProjectLimitless.CameraSystem;
using ProjectLimitless.Core;
using ProjectLimitless.NPC;
using ProjectLimitless.Player;
using ProjectLimitless.UI;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityObject = UnityEngine.Object;

namespace ProjectLimitless.EditorTools
{
    /// <summary>Unity Editor에서 Milestone 01의 실제 Scene Asset을 생성한다.</summary>
    public static class StarterVillageSceneGenerator
    {
        private const string BootstrapScenePath = "Assets/_Project/Scenes/Bootstrap.unity";
        private const string WorldScenePath = "Assets/_Project/Scenes/World_StarterVillage.unity";
        private const string PlayerPrefabPath = "Assets/_Project/Prefabs/PlayerPlaceholder.prefab";
        private const string NpcPrefabPath = "Assets/_Project/Prefabs/VillageNpcPlaceholder.prefab";
        private const string PlayerSpritePath = "Assets/_Project/Art/Characters/Player/Player_Male_Base_Walk.png";
        private const string PlayerAnimationFolder = "Assets/_Project/Animations/Player";
        private const string PlayerAnimatorPath = PlayerAnimationFolder + "/Player.controller";
        private const string KenneyRoot = "Assets/ThirdParty/Kenney/RPGBase/PNG/";
        private const string BackupRoot = "Assets/_Project/Backup/Milestone01";
        private static readonly string[] ManagedAssetPaths =
        {
            BootstrapScenePath,
            WorldScenePath,
            PlayerPrefabPath,
            NpcPrefabPath,
        };

        [MenuItem("Project-Limitless/Milestone 01/Generate Scenes")]
        private static void GenerateScenes()
        {
            string backupPath = null;
            try
            {
                EnsureNoUnsavedMilestoneScenes();
                backupPath = BackupExistingMilestoneAssets();
                DeleteExistingMilestoneAssets();

                ConfigureKenneyImportSettings();
                CreatePlaceholderPrefabs(out GameObject playerPrefab, out GameObject npcPrefab);
                CreateBootstrapScene();
                CreateStarterVillageScene(playerPrefab, npcPrefab);
                ConfigureBuildSettings();
                AssetDatabase.SaveAssets();
                Debug.Log($"Milestone 01 Scene 생성 완료: Bootstrap, World_StarterVillage. 백업: {backupPath ?? "없음"}");
            }
            catch (Exception exception)
            {
                Debug.LogError($"Milestone 01 재생성 실패: {exception.Message}\n백업 위치: {backupPath ?? "생성 전 또는 기존 Asset 없음"}");
                Debug.LogException(exception);
            }
        }

        private static void EnsureNoUnsavedMilestoneScenes()
        {
            foreach (string scenePath in new[] { BootstrapScenePath, WorldScenePath })
            {
                Scene scene = SceneManager.GetSceneByPath(scenePath);
                if (scene.IsValid() && scene.isDirty)
                {
                    throw new InvalidOperationException($"저장되지 않은 Scene이 열려 있습니다. 먼저 저장하세요: {scenePath}");
                }
            }
        }

        private static string BackupExistingMilestoneAssets()
        {
            List<string> existingPaths = new List<string>();
            foreach (string path in ManagedAssetPaths)
            {
                if (AssetDatabase.LoadMainAssetAtPath(path) != null)
                {
                    existingPaths.Add(path);
                }
            }

            if (existingPaths.Count == 0)
            {
                return null;
            }

            EnsureFolder("Assets/_Project/Backup");
            EnsureFolder(BackupRoot);
            string backupPath = $"{BackupRoot}/{DateTime.Now:yyyyMMdd_HHmmss}";
            EnsureFolder(backupPath);

            foreach (string sourcePath in existingPaths)
            {
                string destinationPath = $"{backupPath}/{System.IO.Path.GetFileName(sourcePath)}";
                if (!AssetDatabase.CopyAsset(sourcePath, destinationPath) || AssetDatabase.LoadMainAssetAtPath(destinationPath) == null)
                {
                    throw new InvalidOperationException($"Asset 백업에 실패했습니다: {sourcePath}");
                }
            }

            AssetDatabase.SaveAssets();
            return backupPath;
        }

        private static void DeleteExistingMilestoneAssets()
        {
            foreach (string path in ManagedAssetPaths)
            {
                if (AssetDatabase.LoadMainAssetAtPath(path) != null && !AssetDatabase.DeleteAsset(path))
                {
                    throw new InvalidOperationException($"백업 후 기존 Asset 제거에 실패했습니다: {path}");
                }
            }
        }

        private static void EnsureFolder(string path)
        {
            if (AssetDatabase.IsValidFolder(path))
            {
                return;
            }

            string parent = System.IO.Path.GetDirectoryName(path)?.Replace('\\', '/');
            string folderName = System.IO.Path.GetFileName(path);
            if (string.IsNullOrEmpty(parent) || !AssetDatabase.IsValidFolder(parent))
            {
                throw new InvalidOperationException($"백업 폴더의 상위 경로를 찾지 못했습니다: {path}");
            }

            AssetDatabase.CreateFolder(parent, folderName);
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
                UnityObject.DestroyImmediate(existingEnvironment);
            }

            string[] legacyNames = { "VillageFloor", "BoundaryTop", "BoundaryBottom", "BoundaryLeft", "BoundaryRight", "BuildingNorthWest", "BuildingNorthEast", "BuildingSouthEast", "Well", "Crate" };
            foreach (string legacyName in legacyNames)
            {
                GameObject legacyObject = GameObject.Find(legacyName);
                if (legacyObject != null)
                {
                    UnityObject.DestroyImmediate(legacyObject);
                }
            }

            CreateKenneyStarterVillage();
            EditorSceneManager.SaveScene(scene);
            AssetDatabase.SaveAssets();
            Debug.Log("Kenney RPG Base 환경을 World_StarterVillage에 적용했습니다.");
        }

        private static void CreatePlaceholderPrefabs(out GameObject playerPrefab, out GameObject npcPrefab)
        {
            Sprite[][] directionalSprites = PreparePlayerSpriteAssets();
            GameObject playerSource = new GameObject("PlayerPlaceholder", typeof(SpriteRenderer));
            SpriteRenderer playerRenderer = playerSource.GetComponent<SpriteRenderer>();
            playerRenderer.sprite = directionalSprites[0][0];
            playerRenderer.sortingOrder = 5;
            Animator animator = playerSource.AddComponent<Animator>();
            animator.runtimeAnimatorController = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(PlayerAnimatorPath);
            Rigidbody2D body = playerSource.AddComponent<Rigidbody2D>();
            body.gravityScale = 0f;
            body.freezeRotation = true;
            playerSource.AddComponent<CircleCollider2D>().radius = 0.5f;
            playerSource.AddComponent<PlayerController>();
            playerSource.AddComponent<PlayerSpriteAnimator>();
            playerSource.AddComponent<InteractionSystem>();
            playerPrefab = PrefabUtility.SaveAsPrefabAsset(playerSource, PlayerPrefabPath);
            UnityObject.DestroyImmediate(playerSource);

            GameObject npcSource = CreatePlaceholder("VillageNpcPlaceholder", Vector2.zero, new Vector2(0.7f, 0.9f), new Color(1f, 0.75f, 0.2f), "마을 주민", 5, false);
            npcSource.AddComponent<CircleCollider2D>().isTrigger = true;
            npcSource.AddComponent<NpcController>().Configure("마을 주민", "어서 오세요. 여기는 우리의 첫 번째 마을입니다.");
            npcSource.AddComponent<NpcInteractionPrompt>();
            npcPrefab = PrefabUtility.SaveAsPrefabAsset(npcSource, NpcPrefabPath);
            UnityObject.DestroyImmediate(npcSource);
        }

        private static Sprite[][] PreparePlayerSpriteAssets()
        {
            EnsureFolder("Assets/_Project/Animations");
            EnsureFolder(PlayerAnimationFolder);

            TextureImporter importer = AssetImporter.GetAtPath(PlayerSpritePath) as TextureImporter;
            if (importer == null || importer.spriteImportMode != SpriteImportMode.Multiple || importer.spritePixelsPerUnit != 48)
            {
                throw new InvalidOperationException("Player_Male_Base_Walk.png는 Multiple Sprite, PPU 48로 Import되어야 합니다.");
            }

            string[] directions = { "Down", "Left", "Right", "Up" };
            UnityObject[] allAssets = AssetDatabase.LoadAllAssetsAtPath(PlayerSpritePath);
            Sprite[] importedSprites = Array.ConvertAll(
                Array.FindAll(allAssets, asset => asset is Sprite),
                asset => (Sprite)asset);
            Array.Sort(importedSprites, (left, right) =>
            {
                int row = right.rect.y.CompareTo(left.rect.y);
                return row != 0 ? row : left.rect.x.CompareTo(right.rect.x);
            });
            if (importedSprites.Length != 16)
            {
                throw new InvalidOperationException($"Player_Male_Base_Walk.png 재Import 후 Sprite 수가 16개가 아닙니다: {importedSprites.Length}");
            }

            for (int index = 0; index < importedSprites.Length; index++)
            {
                Rect rect = importedSprites[index].rect;
                float expectedX = (index % 4) * 48f;
                float expectedY = (3 - index / 4) * 48f;
                if (rect.width != 48f || rect.height != 48f || rect.x != expectedX || rect.y != expectedY)
                {
                    throw new InvalidOperationException($"Player_Male_Base_Walk.png의 {index}번 Sprite Grid가 올바르지 않습니다. 48x48 Grid 16개로 다시 Slice하세요.");
                }
            }

            Sprite[][] result = new Sprite[4][];
            for (int row = 0; row < directions.Length; row++)
            {
                result[row] = new Sprite[4];
                Array.Copy(importedSprites, row * 4, result[row], 0, 4);
            }

            CreatePlayerAnimationClips(directions, result);
            CreatePlayerAnimatorController(directions);
            return result;
        }

        private static void CreatePlayerAnimationClips(string[] directions, Sprite[][] directionalSprites)
        {
            for (int directionIndex = 0; directionIndex < directions.Length; directionIndex++)
            {
                CreatePlayerClip($"Idle_{directions[directionIndex]}", new[] { directionalSprites[directionIndex][0] }, false);
                CreatePlayerClip($"Walk_{directions[directionIndex]}", directionalSprites[directionIndex], true);
            }
        }

        private static void CreatePlayerClip(string clipName, Sprite[] frames, bool loop)
        {
            string path = $"{PlayerAnimationFolder}/{clipName}.anim";
            AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
            if (clip == null)
            {
                clip = new AnimationClip { name = clipName, frameRate = 8f };
                AssetDatabase.CreateAsset(clip, path);
            }

            ObjectReferenceKeyframe[] keys = new ObjectReferenceKeyframe[frames.Length];
            for (int i = 0; i < frames.Length; i++)
            {
                keys[i] = new ObjectReferenceKeyframe { time = i / 8f, value = frames[i] };
            }

            EditorCurveBinding binding = new EditorCurveBinding
            {
                type = typeof(SpriteRenderer),
                path = string.Empty,
                propertyName = "m_Sprite",
            };
            AnimationUtility.SetObjectReferenceCurve(clip, binding, keys);
            AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(clip);
            settings.loopTime = loop;
            AnimationUtility.SetAnimationClipSettings(clip, settings);
            EditorUtility.SetDirty(clip);
        }

        private static void CreatePlayerAnimatorController(string[] directions)
        {
            AnimatorController controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(PlayerAnimatorPath);
            if (controller == null)
            {
                controller = AnimatorController.CreateAnimatorControllerAtPath(PlayerAnimatorPath);
            }

            controller.parameters = new[]
            {
                new AnimatorControllerParameter { name = "Speed", type = AnimatorControllerParameterType.Float },
                new AnimatorControllerParameter { name = "MoveX", type = AnimatorControllerParameterType.Float },
                new AnimatorControllerParameter { name = "MoveY", type = AnimatorControllerParameterType.Float },
            };
            AnimatorStateMachine stateMachine = controller.layers[0].stateMachine;
            foreach (ChildAnimatorState state in stateMachine.states)
            {
                stateMachine.RemoveState(state.state);
            }

            AnimatorState defaultState = null;
            foreach (string direction in directions)
            {
                foreach (string action in new[] { "Idle", "Walk" })
                {
                    AnimatorState state = stateMachine.AddState($"{action}_{direction}");
                    state.motion = AssetDatabase.LoadAssetAtPath<AnimationClip>($"{PlayerAnimationFolder}/{action}_{direction}.anim");
                    if (action == "Idle" && direction == "Down")
                    {
                        defaultState = state;
                    }
                }
            }

            stateMachine.defaultState = defaultState;
            EditorUtility.SetDirty(controller);
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

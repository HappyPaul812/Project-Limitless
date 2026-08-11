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
using UnityEditor.U2D.Sprites;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityObject = UnityEngine.Object;

namespace ProjectLimitless.EditorTools
{
    /// <summary>
    /// Unity Editor 메뉴에서 Milestone 01의 Scene, Prefab, Sprite Animation Asset을 자동으로 만듭니다.
    /// AssetDatabase는 프로젝트 파일을 읽고 저장하는 Editor 전용 기능이므로 빌드된 실제 게임에서는 실행되지 않습니다.
    /// 기존 결과물은 먼저 시간별 폴더에 백업한 뒤 다시 만들어 수동 작업을 잃을 위험을 줄입니다.
    /// </summary>
    public static class StarterVillageSceneGenerator
    {
        // 자동 생성하거나 참조할 프로젝트 Asset 경로를 한곳에서 관리합니다.
        private const string BootstrapScenePath = "Assets/_Project/Scenes/Bootstrap.unity";
        private const string CharacterCreationScenePath = "Assets/_Project/Scenes/CharacterCreation.unity";
        private const string WorldScenePath = "Assets/_Project/Scenes/World_StarterVillage.unity";
        private const string PlayerPrefabPath = "Assets/_Project/Prefabs/PlayerPlaceholder.prefab";
        private const string NpcPrefabPath = "Assets/_Project/Prefabs/VillageNpcPlaceholder.prefab";
        private const string MalePlayerSpritePath = "Assets/_Project/Art/Characters/Player/Player_Male_Base_Walk_128.png";
        private const string FemalePlayerSpritePath = "Assets/_Project/Art/Characters/Player/Player_Female_Base_Walk_128.png";
        private const string PlayerAnimationFolder = "Assets/_Project/Animations/Player";
        private const string MalePlayerAnimatorPath = PlayerAnimationFolder + "/Player_Male.controller";
        private const string FemalePlayerAnimatorPath = PlayerAnimationFolder + "/Player_Female.controller";
        private const string BasicHandDrawnRoot = "Assets/ThirdParty/Schwarnhild/BasicHandDrawn/";
        private const string EssentialRpgRoot = "Assets/ThirdParty/Xariami/EssentialRPG/";
        private const string GrassTilesPath = BasicHandDrawnRoot + "tiles/tiles_grass.png";
        private const string HouseTilesPath = BasicHandDrawnRoot + "tiles/house_tiles_new.png";
        private const string FenceTilesPath = BasicHandDrawnRoot + "tiles/fence_tiles.png";
        private const string BackupRoot = "Assets/_Project/Backup/Milestone01";
        // Scene 전체 재생성 전에 백업하고 교체하는 Asset 목록입니다.
        private static readonly string[] ManagedAssetPaths =
        {
            BootstrapScenePath,
            CharacterCreationScenePath,
            WorldScenePath,
            PlayerPrefabPath,
            NpcPrefabPath,
        };

        /// <summary>Editor 메뉴에서 백업, 가져오기 설정, Prefab, Scene, Build Settings를 순서대로 생성합니다.</summary>
        [MenuItem("Project-Limitless/Milestone 01/Generate Scenes")]
        private static void GenerateScenes()
        {
            string backupPath = null;
            try
            {
                // 저장하지 않은 Scene 작업을 덮어쓰지 않도록 확인한 뒤 기존 결과물을 백업합니다.
                EnsureNoUnsavedMilestoneScenes();
                backupPath = BackupExistingMilestoneAssets();
                DeleteExistingMilestoneAssets();

                ConfigureStarterVillageImportSettings();
                CreatePlaceholderPrefabs(out GameObject playerPrefab, out GameObject npcPrefab);
                CreateBootstrapScene();
                CreateCharacterCreationScene();
                CreateStarterVillageScene(playerPrefab, npcPrefab);
                ConfigureBuildSettings();
                AssetDatabase.SaveAssets();
                Debug.Log($"Milestone 01 Scene 생성 완료: Bootstrap, CharacterCreation, World_StarterVillage. 백업: {backupPath ?? "없음"}");
            }
            catch (Exception exception)
            {
                Debug.LogError($"Milestone 01 재생성 실패: {exception.Message}\n백업 위치: {backupPath ?? "생성 전 또는 기존 Asset 없음"}");
                Debug.LogException(exception);
            }
        }

        /// <summary>기존 플레이어의 게임 기능은 유지하고 Male/Female Visual 자식과 전용 Animator를 연결합니다.</summary>
        [MenuItem("Project-Limitless/Milestone 01/Apply Male-Female Player Visuals")]
        public static void ApplyMaleFemalePlayerVisuals()
        {
            // Prefab 내용을 임시 편집 공간에 열고 finally에서 반드시 닫습니다.
            GameObject playerPrefab = PrefabUtility.LoadPrefabContents(PlayerPrefabPath);
            try
            {
                PlayerVisualAssets visualAssets = PrepareAllPlayerVisualAssets();
                if (playerPrefab.GetComponent<PlayerController>() == null || playerPrefab.GetComponent<Rigidbody2D>() == null ||
                    playerPrefab.GetComponent<CircleCollider2D>() == null ||
                    playerPrefab.GetComponent<InteractionSystem>() == null)
                {
                    throw new InvalidOperationException("Player Prefab의 기존 이동 또는 상호작용 Component를 찾지 못했습니다.");
                }

                ConfigurePlayerVisualHierarchy(playerPrefab, visualAssets);
                PrefabUtility.SaveAsPrefabAsset(playerPrefab, PlayerPrefabPath);
                AssetDatabase.SaveAssets();
                Debug.Log("Male/Female 128x128 Visual과 전용 Animator를 Player Prefab에 적용했습니다. 기본값은 Male입니다.");
            }
            finally
            {
                PrefabUtility.UnloadPrefabContents(playerPrefab);
            }
        }

        /// <summary>
        /// 기존 World Scene은 건드리지 않고 Character Creation Scene, Bootstrap 시작 대상, Build Settings만 갱신합니다.
        /// </summary>
        [MenuItem("Project-Limitless/Milestone 01/Generate Character Creation Scene")]
        public static void GenerateCharacterCreationSceneAssets()
        {
            try
            {
                EnsureNoUnsavedScenes(BootstrapScenePath, CharacterCreationScenePath);
                CreateCharacterCreationScene();
                UpdateBootstrapStartScene();
                ConfigureBuildSettings();
                AssetDatabase.SaveAssets();
                Debug.Log("CharacterCreation Scene 생성과 Bootstrap 연결을 완료했습니다. 기존 World Scene은 변경하지 않았습니다.");
            }
            catch (Exception exception)
            {
                Debug.LogError($"CharacterCreation Scene 생성 실패: {exception.Message}");
                Debug.LogException(exception);
            }
        }

        /// <summary>자동 생성 대상 Scene에 저장되지 않은 변경이 있으면 덮어쓰기 전에 작업을 중단합니다.</summary>
        private static void EnsureNoUnsavedMilestoneScenes()
        {
            EnsureNoUnsavedScenes(BootstrapScenePath, CharacterCreationScenePath, WorldScenePath);
        }

        /// <summary>지정한 자동 생성 대상 중 열려 있고 저장하지 않은 Scene이 있는지 확인합니다.</summary>
        private static void EnsureNoUnsavedScenes(params string[] scenePaths)
        {
            foreach (string scenePath in scenePaths)
            {
                Scene scene = SceneManager.GetSceneByPath(scenePath);
                if (scene.IsValid() && scene.isDirty)
                {
                    throw new InvalidOperationException($"저장되지 않은 Scene이 열려 있습니다. 먼저 저장하세요: {scenePath}");
                }
            }
        }

        /// <summary>기존 Scene과 Prefab을 시간별 백업 폴더에 복사하고 그 폴더 경로를 반환합니다.</summary>
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
            // 실행 시각을 폴더명에 넣어 여러 차례 생성한 백업이 서로 덮어쓰지 않게 합니다.
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

        /// <summary>백업이 끝난 자동 관리 Asset을 삭제하여 같은 경로에 새 결과물을 만들 수 있게 합니다.</summary>
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

        /// <summary>AssetDatabase에 폴더가 없으면 바로 위의 기존 폴더 아래에 새로 만듭니다.</summary>
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

        /// <summary>게임 진입점 역할을 하는 빈 Scene과 BootstrapLoader GameObject를 만들어 저장합니다.</summary>
        private static void CreateBootstrapScene()
        {
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            BootstrapLoader loader = new GameObject("Bootstrap").AddComponent<BootstrapLoader>();
            loader.ConfigureStartScene("CharacterCreation");
            EditorSceneManager.SaveScene(scene, BootstrapScenePath);
        }

        /// <summary>Male/Female Down Idle 미리보기가 연결된 최소 Character Creation Scene을 만듭니다.</summary>
        private static void CreateCharacterCreationScene()
        {
            PlayerVisualAssets visualAssets = PrepareAllPlayerVisualAssets();
            Scene previousActiveScene = SceneManager.GetActiveScene();
            Scene existingScene = SceneManager.GetSceneByPath(CharacterCreationScenePath);
            if (existingScene.IsValid())
            {
                EditorSceneManager.CloseScene(existingScene, true);
            }

            bool hasEmptyUntitledScene = previousActiveScene.IsValid() &&
                string.IsNullOrEmpty(previousActiveScene.path) &&
                !previousActiveScene.isDirty &&
                previousActiveScene.rootCount == 0;
            // 저장된 작업 Scene은 Additive로 보존합니다. Unity가 처음 연 빈 Scene만 안전하게 교체합니다.
            NewSceneMode creationMode = hasEmptyUntitledScene ? NewSceneMode.Single : NewSceneMode.Additive;
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, creationMode);
            SceneManager.SetActiveScene(scene);
            CharacterCreationController controller = new GameObject("CharacterCreationSystem").AddComponent<CharacterCreationController>();
            controller.Configure(visualAssets.MaleDefaultSprite, visualAssets.FemaleDefaultSprite, "World_StarterVillage");
            EditorSceneManager.SaveScene(scene, CharacterCreationScenePath);
            if (!hasEmptyUntitledScene)
            {
                EditorSceneManager.CloseScene(scene, true);
                if (previousActiveScene.IsValid() && previousActiveScene.isLoaded)
                {
                    SceneManager.SetActiveScene(previousActiveScene);
                }
            }
        }

        /// <summary>기존 Bootstrap Scene의 다른 내용은 유지하고 시작 Scene 이름만 CharacterCreation으로 바꿉니다.</summary>
        private static void UpdateBootstrapStartScene()
        {
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(BootstrapScenePath) == null)
            {
                CreateBootstrapScene();
                return;
            }

            Scene scene = SceneManager.GetSceneByPath(BootstrapScenePath);
            bool wasAlreadyLoaded = scene.IsValid();
            if (!wasAlreadyLoaded)
            {
                scene = EditorSceneManager.OpenScene(BootstrapScenePath, OpenSceneMode.Additive);
            }

            BootstrapLoader loader = UnityObject.FindFirstObjectByType<BootstrapLoader>();
            if (loader == null)
            {
                throw new InvalidOperationException("Bootstrap Scene에서 BootstrapLoader를 찾지 못했습니다.");
            }

            loader.ConfigureStartScene("CharacterCreation");
            EditorUtility.SetDirty(loader);
            EditorSceneManager.SaveScene(scene);
            if (!wasAlreadyLoaded)
            {
                EditorSceneManager.CloseScene(scene, true);
            }
        }

        /// <summary>카메라, 환경, 플레이어, NPC, 대화 UI를 배치한 Starter Village Scene을 저장합니다.</summary>
        private static void CreateStarterVillageScene(GameObject playerPrefab, GameObject npcPrefab)
        {
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            CreateCamera(out CameraFollow cameraFollow);
            CreateHandDrawnStarterVillage();

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

        /// <summary>기존 월드의 플레이어와 NPC는 유지하면서 손그림 마을 환경 부분만 다시 만듭니다.</summary>
        [MenuItem("Project-Limitless/Milestone 01/Apply Hand-Drawn Village Environment")]
        private static void ApplyHandDrawnVillageEnvironment()
        {
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(WorldScenePath) == null)
            {
                Debug.LogError("World_StarterVillage Scene을 먼저 생성해야 합니다.");
                return;
            }

            ConfigureStarterVillageImportSettings();
            Scene scene = EditorSceneManager.OpenScene(WorldScenePath, OpenSceneMode.Single);
            GameObject existingEnvironment = GameObject.Find("HandDrawnStarterVillage");
            if (existingEnvironment != null)
            {
                UnityObject.DestroyImmediate(existingEnvironment);
            }

            // 예전 생성 방식의 오브젝트가 겹쳐 보이지 않도록 이름으로 찾아 정리합니다.
            string[] legacyNames = { "VillageFloor", "BoundaryTop", "BoundaryBottom", "BoundaryLeft", "BoundaryRight", "BuildingNorthWest", "BuildingNorthEast", "BuildingSouthEast", "Well", "Crate" };
            foreach (string legacyName in legacyNames)
            {
                GameObject legacyObject = GameObject.Find(legacyName);
                if (legacyObject != null)
                {
                    UnityObject.DestroyImmediate(legacyObject);
                }
            }

            CreateHandDrawnStarterVillage();
            EditorSceneManager.SaveScene(scene);
            AssetDatabase.SaveAssets();
            Debug.Log("Schwarnhild/Xariami 환경을 World_StarterVillage에 적용했습니다.");
        }

        /// <summary>필요한 컴포넌트를 조합해 플레이어와 NPC의 재사용 가능한 Prefab을 만듭니다.</summary>
        private static void CreatePlaceholderPrefabs(out GameObject playerPrefab, out GameObject npcPrefab)
        {
            PlayerVisualAssets visualAssets = PrepareAllPlayerVisualAssets();
            GameObject playerSource = new GameObject("PlayerPlaceholder");
            // Rigidbody2D와 Collider2D는 플레이어의 2D 물리 이동과 벽 충돌 범위를 담당합니다.
            Rigidbody2D body = playerSource.AddComponent<Rigidbody2D>();
            body.gravityScale = 0f;
            body.freezeRotation = true;
            playerSource.AddComponent<CircleCollider2D>().radius = 0.5f;
            playerSource.AddComponent<PlayerController>();
            playerSource.AddComponent<InteractionSystem>().Configure(2f);
            ConfigurePlayerVisualHierarchy(playerSource, visualAssets);
            playerPrefab = PrefabUtility.SaveAsPrefabAsset(playerSource, PlayerPrefabPath);
            UnityObject.DestroyImmediate(playerSource);

            GameObject npcSource = CreatePlaceholder("VillageNpcPlaceholder", Vector2.zero, new Vector2(0.7f, 0.9f), new Color(1f, 0.75f, 0.2f), "마을 주민", 5, false);
            // Trigger Collider는 물리적으로 밀어내지 않고 겹침을 감지하는 충돌 범위입니다.
            CircleCollider2D npcCollider = npcSource.AddComponent<CircleCollider2D>();
            npcCollider.radius = 0.5f;
            npcCollider.isTrigger = true;
            npcSource.AddComponent<NpcController>().Configure("마을 주민", "어서 오세요. 여기는 우리의 첫 번째 마을입니다.");
            npcSource.AddComponent<NpcInteractionPrompt>();
            npcPrefab = PrefabUtility.SaveAsPrefabAsset(npcSource, NpcPrefabPath);
            UnityObject.DestroyImmediate(npcSource);
        }

        /// <summary>
        /// 4×4 Sprite 시트가 올바르게 잘렸는지 검사하고 방향별 배열, Animation Clip, Animator를 준비합니다.
        /// Sprite Slice는 한 장의 큰 이미지에서 각 애니메이션 프레임의 사각형을 나누는 작업입니다.
        /// </summary>
        private static PlayerVisualAssets PrepareAllPlayerVisualAssets()
        {
            EnsureFolder("Assets/_Project/Animations");
            EnsureFolder(PlayerAnimationFolder);

            Sprite[][] maleSprites = PreparePlayerSpriteAssets(MalePlayerSpritePath, "Male");
            Sprite[][] femaleSprites = PreparePlayerSpriteAssets(FemalePlayerSpritePath, "Female");
            return new PlayerVisualAssets(
                maleSprites[0][0],
                femaleSprites[0][0],
                AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(MalePlayerAnimatorPath),
                AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(FemalePlayerAnimatorPath));
        }

        /// <summary>한 외형의 4×4 Sprite 시트를 검증하고 전용 Clip과 Animator Controller를 준비합니다.</summary>
        private static Sprite[][] PreparePlayerSpriteAssets(string spritePath, string visualName)
        {

            TextureImporter importer = AssetImporter.GetAtPath(spritePath) as TextureImporter;
            if (importer == null || importer.spriteImportMode != SpriteImportMode.Multiple || importer.spritePixelsPerUnit != 128)
            {
                throw new InvalidOperationException($"{spritePath}는 Multiple Sprite, PPU 128로 Import되어야 합니다.");
            }

            string[] directions = { "Down", "Left", "Right", "Up" };
            UnityObject[] allAssets = AssetDatabase.LoadAllAssetsAtPath(spritePath);
            Sprite[] importedSprites = Array.ConvertAll(
                Array.FindAll(allAssets, asset => asset is Sprite),
                asset => (Sprite)asset);
            // Unity가 반환하는 순서와 관계없이 위쪽 행부터, 같은 행에서는 왼쪽부터 정렬합니다.
            Array.Sort(importedSprites, (left, right) =>
            {
                int row = right.rect.y.CompareTo(left.rect.y);
                return row != 0 ? row : left.rect.x.CompareTo(right.rect.x);
            });
            if (importedSprites.Length != 16)
            {
                throw new InvalidOperationException($"{spritePath}의 Sprite 수가 16개가 아닙니다: {importedSprites.Length}");
            }

            for (int index = 0; index < importedSprites.Length; index++)
            {
                Rect rect = importedSprites[index].rect;
                float expectedX = (index % 4) * 128f;
                float expectedY = (3 - index / 4) * 128f;
                // 각 조각의 크기·위치와 발밑 중심 Pivot이 예상과 다르면 잘못된 애니메이션을 만들기 전에 중단합니다.
                if (rect.width != 128f || rect.height != 128f || rect.x != expectedX || rect.y != expectedY ||
                    importedSprites[index].pivot != new Vector2(64f, 0f))
                {
                    throw new InvalidOperationException($"{spritePath}의 {index}번 Sprite 설정이 올바르지 않습니다. 128x128 Grid 16개와 Bottom Center Pivot을 확인하세요.");
                }
            }

            Sprite[][] result = new Sprite[4][];
            for (int row = 0; row < directions.Length; row++)
            {
                result[row] = new Sprite[4];
                Array.Copy(importedSprites, row * 4, result[row], 0, 4);
            }

            CreatePlayerAnimationClips(visualName, directions, result);
            CreatePlayerAnimatorController(visualName, directions);
            return result;
        }

        /// <summary>각 방향에 정지 1프레임과 반복 걷기 4프레임 Animation Clip을 만듭니다.</summary>
        private static void CreatePlayerAnimationClips(string visualName, string[] directions, Sprite[][] directionalSprites)
        {
            for (int directionIndex = 0; directionIndex < directions.Length; directionIndex++)
            {
                CreatePlayerClip(GetPlayer128ClipName(visualName, "Idle", directions[directionIndex]), new[] { directionalSprites[directionIndex][0] }, false);
                CreatePlayerClip(GetPlayer128ClipName(visualName, "Walk", directions[directionIndex]), directionalSprites[directionIndex], true);
            }
        }

        /// <summary>Sprite 프레임들을 시간 순서로 배치한 Animation Clip Asset을 만들거나 갱신합니다.</summary>
        private static void CreatePlayerClip(string clipName, Sprite[] frames, bool loop)
        {
            string path = $"{PlayerAnimationFolder}/{clipName}.anim";
            AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
            if (clip == null)
            {
                clip = new AnimationClip { name = clipName, frameRate = 6f };
                AssetDatabase.CreateAsset(clip, path);
            }

            clip.frameRate = 6f;

            // Keyframe은 특정 시각에 SpriteRenderer가 보여 줄 Sprite를 기록합니다.
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

        /// <summary>Animator Controller에 이동 매개변수와 4방향 대기·걷기 상태를 다시 구성합니다.</summary>
        private static void CreatePlayerAnimatorController(string visualName, string[] directions)
        {
            string animatorPath = visualName == "Female" ? FemalePlayerAnimatorPath : MalePlayerAnimatorPath;
            AnimatorController controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(animatorPath);
            if (controller == null)
            {
                controller = AnimatorController.CreateAnimatorControllerAtPath(animatorPath);
            }

            controller.parameters = new[]
            {
                new AnimatorControllerParameter { name = "Speed", type = AnimatorControllerParameterType.Float },
                new AnimatorControllerParameter { name = "MoveX", type = AnimatorControllerParameterType.Float },
                new AnimatorControllerParameter { name = "MoveY", type = AnimatorControllerParameterType.Float },
            };
            AnimatorStateMachine stateMachine = controller.layers[0].stateMachine;
            // 자동 관리 Controller이므로 기존 상태를 비우고 정해진 상태를 일관되게 다시 만듭니다.
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
                    state.motion = AssetDatabase.LoadAssetAtPath<AnimationClip>($"{PlayerAnimationFolder}/{GetPlayer128ClipName(visualName, action, direction)}.anim");
                    if (action == "Idle" && direction == "Down")
                    {
                        defaultState = state;
                    }
                }
            }

            stateMachine.defaultState = defaultState;
            EditorUtility.SetDirty(controller);
        }

        /// <summary>동작과 방향을 프로젝트의 Animation Clip 파일명 규칙으로 조합합니다.</summary>
        private static string GetPlayer128ClipName(string visualName, string action, string direction)
        {
            return $"Player_{visualName}_128_{action}_{direction}";
        }

        /// <summary>
        /// 물리·입력·상호작용 Component는 Player 부모에 보존하고, 그림과 Animator만 Visual 자식으로 옮깁니다.
        /// 이미 적용된 Prefab에 다시 실행해도 같은 Visual 자식을 재사용하므로 중복 구조를 만들지 않습니다.
        /// </summary>
        private static void ConfigurePlayerVisualHierarchy(GameObject player, PlayerVisualAssets assets)
        {
            PlayerSpriteAnimator rootSpriteAnimator = player.GetComponent<PlayerSpriteAnimator>();
            if (rootSpriteAnimator != null)
            {
                UnityObject.DestroyImmediate(rootSpriteAnimator);
            }

            Animator rootAnimator = player.GetComponent<Animator>();
            if (rootAnimator != null)
            {
                UnityObject.DestroyImmediate(rootAnimator);
            }

            SpriteRenderer rootRenderer = player.GetComponent<SpriteRenderer>();
            if (rootRenderer != null)
            {
                UnityObject.DestroyImmediate(rootRenderer);
            }

            PlaceholderVisual placeholder = player.GetComponent<PlaceholderVisual>();
            if (placeholder != null)
            {
                UnityObject.DestroyImmediate(placeholder);
            }

            Transform visualTransform = player.transform.Find("Visual");
            GameObject visual = visualTransform != null ? visualTransform.gameObject : new GameObject("Visual");
            visual.transform.SetParent(player.transform, false);
            visual.transform.localPosition = Vector3.zero;
            visual.transform.localRotation = Quaternion.identity;
            visual.transform.localScale = Vector3.one;

            SpriteRenderer renderer = visual.GetComponent<SpriteRenderer>();
            if (renderer == null)
            {
                renderer = visual.AddComponent<SpriteRenderer>();
            }

            renderer.sortingOrder = 5;
            Animator animator = visual.GetComponent<Animator>();
            if (animator == null)
            {
                animator = visual.AddComponent<Animator>();
            }

            PlayerSpriteAnimator spriteAnimator = visual.GetComponent<PlayerSpriteAnimator>();
            if (spriteAnimator == null)
            {
                visual.AddComponent<PlayerSpriteAnimator>();
            }

            PlayerVisualController visualController = player.GetComponent<PlayerVisualController>();
            if (visualController == null)
            {
                visualController = player.AddComponent<PlayerVisualController>();
            }

            visualController.Configure(
                renderer,
                animator,
                assets.MaleAnimatorController,
                assets.FemaleAnimatorController,
                assets.MaleDefaultSprite,
                assets.FemaleDefaultSprite);
        }

        /// <summary>두 외형의 기본 Sprite와 Animator Controller를 생성 단계 사이에서 묶어 전달합니다.</summary>
        private readonly struct PlayerVisualAssets
        {
            public PlayerVisualAssets(
                Sprite maleDefaultSprite,
                Sprite femaleDefaultSprite,
                RuntimeAnimatorController maleAnimatorController,
                RuntimeAnimatorController femaleAnimatorController)
            {
                MaleDefaultSprite = maleDefaultSprite;
                FemaleDefaultSprite = femaleDefaultSprite;
                MaleAnimatorController = maleAnimatorController;
                FemaleAnimatorController = femaleAnimatorController;
            }

            public Sprite MaleDefaultSprite { get; }
            public Sprite FemaleDefaultSprite { get; }
            public RuntimeAnimatorController MaleAnimatorController { get; }
            public RuntimeAnimatorController FemaleAnimatorController { get; }
        }

        /// <summary>2D Orthographic 카메라와 소리 수신기, 플레이어 추적 컴포넌트를 만듭니다.</summary>
        private static void CreateCamera(out CameraFollow cameraFollow)
        {
            GameObject cameraObject = new GameObject("Main Camera");
            cameraObject.tag = "MainCamera";
            cameraObject.transform.position = new Vector3(0f, 0f, -10f);
            Camera camera = cameraObject.AddComponent<Camera>();
            // Orthographic은 원근에 따라 크기가 달라지지 않는 2D용 카메라 방식입니다.
            camera.orthographic = true;
            camera.orthographicSize = 4.75f;
            camera.backgroundColor = new Color(0.08f, 0.12f, 0.18f);
            cameraObject.AddComponent<AudioListener>();
            cameraFollow = cameraObject.AddComponent<CameraFollow>();
        }

        /// <summary>바닥, 길, 집, 자연물, 소품, 울타리를 하나의 마을 환경 부모 아래에 배치합니다.</summary>
        private static void CreateHandDrawnStarterVillage()
        {
            GameObject root = new GameObject("HandDrawnStarterVillage");
            CreateGround(root.transform);
            CreatePath(root.transform);
            CreateHouse(root.transform, "HouseNorthWest", new Vector2(-4.5f, 3.5f));
            CreateHouse(root.transform, "HouseNorthEast", new Vector2(4.5f, 3.5f));
            CreateBasicSprite(root.transform, "TreeWest", "assets/tree_big.png", new Vector2(-7.5f, 1.8f), 4, new Vector2(0.45f, 0.3f), new Vector2(0f, -0.75f));
            CreateBasicSprite(root.transform, "TreeEast", "assets/tree_medium.png", new Vector2(7.5f, 1.5f), 4, new Vector2(0.4f, 0.25f), new Vector2(0f, -0.5f));
            CreateBasicSprite(root.transform, "TreeSouthWest", "assets/tree_medium.png", new Vector2(-7f, -4.2f), 4, new Vector2(0.4f, 0.25f), new Vector2(0f, -0.5f));
            CreateBasicSprite(root.transform, "BushWest", "assets/bush_01.png", new Vector2(-5.5f, -1.2f), 1);
            CreateBasicSprite(root.transform, "BushEast", "assets/bush_02.png", new Vector2(5.6f, -1f), 1);
            CreateBasicSprite(root.transform, "RockNorth", "assets/rock_01.png", new Vector2(-1.7f, 2.1f), 1);
            CreateEssentialSprite(root.transform, "Chest", "Village_and_Camp/Wooden_Chest_Type_A.png", new Vector2(-2.3f, -1.4f), 3, new Vector2(0.45f, 0.3f), new Vector2(0f, -0.3f));
            CreateEssentialSprite(root.transform, "Barrel", "Village_and_Camp/Wooden_Barrel_Type_A.png", new Vector2(2.5f, -1.3f), 3, new Vector2(0.35f, 0.3f), new Vector2(0f, -0.3f));
            CreateEssentialSprite(root.transform, "Campfire", "Props_and_Loot/Campfire_Type_A.png", new Vector2(0f, 1.1f), 2);
            CreateFence(root.transform, new Vector2(-7.5f, 5.3f));
            CreateFence(root.transform, new Vector2(-6.5f, 5.3f));
            CreateFence(root.transform, new Vector2(6.5f, 5.3f));
            CreateFence(root.transform, new Vector2(7.5f, 5.3f));
        }

        /// <summary>128×128 잔디 타일을 격자로 반복 배치해 마을 바닥을 채웁니다.</summary>
        private static void CreateGround(Transform parent)
        {
            for (int x = -9; x <= 9; x++)
            {
                for (int y = -6; y <= 6; y++)
                {
                    CreateGridSprite(parent, $"Grass_{x}_{y}", GrassTilesPath, "tiles_grass_4_0", new Vector2(x, y), -10);
                }
            }
        }

        /// <summary>두 타일 너비의 세로 흙길을 마을 중앙에 배치합니다.</summary>
        private static void CreatePath(Transform parent)
        {
            for (int y = -6; y <= 1; y++)
            {
                CreateGridSprite(parent, $"PathLeft_{y}", GrassTilesPath, "tiles_grass_4_4", new Vector2(-0.5f, y), -8);
                CreateGridSprite(parent, $"PathRight_{y}", GrassTilesPath, "tiles_grass_5_4", new Vector2(0.5f, y), -8);
            }
        }

        /// <summary>여러 조각 Sprite를 조합하고 문 앞에 Collider2D를 둔 집 하나를 만듭니다.</summary>
        private static void CreateHouse(Transform parent, string houseName, Vector2 position)
        {
            GameObject house = new GameObject(houseName);
            house.transform.SetParent(parent);
            CreateGridSprite(house.transform, "Roof", HouseTilesPath, "house_tiles_new_4_4", position + new Vector2(0f, 1f), 2);
            CreateGridSprite(house.transform, "WallLeft", HouseTilesPath, "house_tiles_new_1_3", position + new Vector2(-0.5f, 0f), 2);
            CreateGridSprite(house.transform, "WallRight", HouseTilesPath, "house_tiles_new_1_3", position + new Vector2(0.5f, 0f), 2);
            CreateGridSprite(house.transform, "Door", HouseTilesPath, "house_tiles_new_1_1", position + new Vector2(0f, 0f), 2, new Vector2(1.5f, 0.3f), new Vector2(0f, -0.35f));
        }

        /// <summary>지정 위치에 충돌 범위가 있는 울타리 Sprite를 만듭니다.</summary>
        private static void CreateFence(Transform parent, Vector2 position)
        {
            CreateGridSprite(parent, $"Fence_{position.x}_{position.y}", FenceTilesPath, "fence_tiles_2_2", position, 2, new Vector2(0.9f, 0.2f), new Vector2(0f, -0.35f));
        }

        /// <summary>Basic Hand-Drawn 폴더의 단일 Sprite를 읽어 마을 오브젝트로 만듭니다.</summary>
        private static GameObject CreateBasicSprite(Transform parent, string objectName, string relativePath, Vector2 position, int sortingOrder, Vector2? colliderSize = null, Vector2? colliderOffset = null)
        {
            return CreateSprite(parent, objectName, AssetDatabase.LoadAssetAtPath<Sprite>(BasicHandDrawnRoot + relativePath), position, sortingOrder, colliderSize, colliderOffset);
        }

        /// <summary>Essential RPG 폴더의 단일 Sprite를 읽어 마을 오브젝트로 만듭니다.</summary>
        private static GameObject CreateEssentialSprite(Transform parent, string objectName, string relativePath, Vector2 position, int sortingOrder, Vector2? colliderSize = null, Vector2? colliderOffset = null)
        {
            return CreateSprite(parent, objectName, AssetDatabase.LoadAssetAtPath<Sprite>(EssentialRpgRoot + relativePath), position, sortingOrder, colliderSize, colliderOffset);
        }

        /// <summary>한 이미지에서 잘라낸 여러 Sprite 중 이름이 맞는 조각을 찾아 오브젝트로 만듭니다.</summary>
        private static GameObject CreateGridSprite(Transform parent, string objectName, string assetPath, string spriteName, Vector2 position, int sortingOrder, Vector2? colliderSize = null, Vector2? colliderOffset = null)
        {
            Sprite sprite = Array.Find(AssetDatabase.LoadAllAssetsAtPath(assetPath), asset => asset is Sprite && asset.name == spriteName) as Sprite;
            return CreateSprite(parent, objectName, sprite, position, sortingOrder, colliderSize, colliderOffset);
        }

        /// <summary>SpriteRenderer를 만들고, 필요하면 벽이나 장애물 역할의 BoxCollider2D도 추가합니다.</summary>
        private static GameObject CreateSprite(Transform parent, string objectName, Sprite sprite, Vector2 position, int sortingOrder, Vector2? colliderSize = null, Vector2? colliderOffset = null)
        {
            if (sprite == null)
            {
                throw new InvalidOperationException($"Starter Village Sprite를 찾지 못했습니다: {objectName}");
            }

            GameObject gameObject = new GameObject(objectName, typeof(SpriteRenderer));
            gameObject.transform.SetParent(parent);
            gameObject.transform.position = position;
            SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sortingOrder = sortingOrder;

            // 충돌 크기를 전달한 장식물만 플레이어가 통과하지 못하는 장애물이 됩니다.
            if (colliderSize.HasValue)
            {
                BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
                collider.size = colliderSize.Value;
                collider.offset = colliderOffset ?? Vector2.zero;
            }

            return gameObject;
        }

        /// <summary>마을에 사용하는 격자 이미지와 단일 이미지의 Unity Import 설정을 일괄 적용합니다.</summary>
        private static void ConfigureStarterVillageImportSettings()
        {
            ConfigureGridImport(GrassTilesPath, 10, 6);
            ConfigureGridImport(HouseTilesPath, 6, 5);
            ConfigureGridImport(FenceTilesPath, 5, 5);
            string[] individualPaths =
            {
                BasicHandDrawnRoot + "assets/tree_big.png", BasicHandDrawnRoot + "assets/tree_medium.png",
                BasicHandDrawnRoot + "assets/bush_01.png", BasicHandDrawnRoot + "assets/bush_02.png", BasicHandDrawnRoot + "assets/rock_01.png",
                EssentialRpgRoot + "Village_and_Camp/Wooden_Chest_Type_A.png", EssentialRpgRoot + "Village_and_Camp/Wooden_Barrel_Type_A.png",
                EssentialRpgRoot + "Props_and_Loot/Campfire_Type_A.png",
            };
            foreach (string path in individualPaths)
            {
                ConfigureSingleSpriteImport(path);
            }
        }

        /// <summary>이미지 한 장 전체를 128 PPU의 픽셀 선명한 단일 Sprite로 가져옵니다.</summary>
        private static void ConfigureSingleSpriteImport(string path)
        {
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer == null)
            {
                throw new InvalidOperationException($"Texture Importer를 찾지 못했습니다: {path}");
            }

            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.spritePixelsPerUnit = 128;
            importer.filterMode = FilterMode.Point;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.mipmapEnabled = false;
            importer.SaveAndReimport();
        }

        /// <summary>격자 이미지를 128×128 크기의 여러 Sprite로 자동 Slice하여 다시 가져옵니다.</summary>
        private static void ConfigureGridImport(string path, int columns, int rows)
        {
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            if (importer == null || texture == null || texture.width != columns * 128 || texture.height != rows * 128)
            {
                throw new InvalidOperationException($"128x128 Grid Texture 규격이 올바르지 않습니다: {path}");
            }

            // 각 칸의 이름, 픽셀 사각형, 중심점을 Sprite Editor 데이터로 구성합니다.
            SpriteRect[] sprites = new SpriteRect[columns * rows];
            string textureName = System.IO.Path.GetFileNameWithoutExtension(path);
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    int index = y * columns + x;
                    sprites[index] = new SpriteRect
                    {
                        name = $"{textureName}_{x}_{y}",
                        rect = new Rect(x * 128, y * 128, 128, 128),
                        alignment = (int)SpriteAlignment.Center,
                        pivot = new Vector2(0.5f, 0.5f),
                        spriteID = GUID.Generate(),
                    };
                }
            }

            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Multiple;
            importer.spritePixelsPerUnit = 128;
            importer.filterMode = FilterMode.Point;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.mipmapEnabled = false;
            // Sprite Editor 데이터 제공자를 통해 계산한 Slice 정보를 TextureImporter에 저장합니다.
            SpriteDataProviderFactories factories = new SpriteDataProviderFactories();
            factories.Init();
            var dataProvider = factories.GetSpriteEditorDataProviderFromObject(importer);
            dataProvider.InitSpriteEditorDataProvider();
            dataProvider.SetSpriteRects(sprites);
            dataProvider.Apply();
            importer.SaveAndReimport();
        }

        /// <summary>실제 그림이 준비되지 않은 오브젝트에 단색 Sprite, 이름표, 선택적 충돌 범위를 만듭니다.</summary>
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

        /// <summary>게임 흐름 순서대로 Bootstrap, Character Creation, 월드 Scene을 빌드 목록 앞에 둡니다.</summary>
        private static void ConfigureBuildSettings()
        {
            List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>();
            scenes.Add(new EditorBuildSettingsScene(BootstrapScenePath, true));
            scenes.Add(new EditorBuildSettingsScene(CharacterCreationScenePath, true));
            scenes.Add(new EditorBuildSettingsScene(WorldScenePath, true));

            foreach (EditorBuildSettingsScene existingScene in EditorBuildSettings.scenes)
            {
                if (existingScene.path != BootstrapScenePath && existingScene.path != CharacterCreationScenePath && existingScene.path != WorldScenePath)
                {
                    scenes.Add(existingScene);
                }
            }

            EditorBuildSettings.scenes = scenes.ToArray();
        }
    }
}
#endif

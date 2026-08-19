using System;
using System.Collections.Generic;
using System.Linq;
using ProjectLimitless.Monster;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace ProjectLimitless.Editor
{
    /// <summary>
    /// 초원 슬라임의 4×4 Sprite 시트를 자동으로 자르고 방향별 Idle/Walk Animation을 만듭니다.
    /// Unity Editor가 프로젝트를 열거나 원본 PNG를 다시 Import할 때 실행되며, 완성된 Visual을 MonsterDefinition에 연결합니다.
    /// </summary>
    public static class GrassSlimeAnimationBuilder
    {
        public const string SpritePath = "Assets/_Project/Art/Monsters/01_GrassSlime/01_GrassSlime_Walk.png";
        private const string AnimationFolder = "Assets/_Project/Animations/Monsters/01_GrassSlime";
        private const string ControllerPath = AnimationFolder + "/GrassSlime.controller";
        private const string DefinitionPath = "Assets/_Project/Resources/MonsterDefinitions/01_GrassSlime.asset";
        private const string VisualName = "GrassSlime";
        private static readonly string[] Directions = { "Down", "Left", "Right", "Up" };

        public static bool IsBuilding { get; private set; }

        /// <summary>
        /// Script가 다시 컴파일된 뒤 한 번 호출됩니다. 원본이 존재하지만 연결 결과가 없을 때만 자동 생성을 예약합니다.
        /// Import 처리 도중 즉시 Asset을 다시 Import하면 충돌할 수 있어 다음 Editor 갱신 시점까지 기다립니다.
        /// </summary>
        [InitializeOnLoadMethod]
        private static void ScheduleAutomaticBuild()
        {
            EditorApplication.delayCall += () =>
            {
                if (AssetDatabase.LoadAssetAtPath<Texture2D>(SpritePath) != null && !IsDefinitionConnected())
                    ImportAndBuild();
            };
        }

        /// <summary>개발자가 필요할 때 동일한 자동 생성 과정을 Unity 메뉴에서 다시 실행할 수 있게 합니다.</summary>
        [MenuItem("Project-Limitless/Monsters/Import And Build Grass Slime")]
        public static void ImportAndBuild()
        {
            if (IsBuilding) return;

            IsBuilding = true;
            try
            {
                Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(SpritePath);
                ValidateSource(texture);
                EnsureFolder(AnimationFolder);
                ConfigureAndSlice(texture.width, texture.height);

                Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(SpritePath)
                    .OfType<Sprite>()
                    .OrderBy(GetDirectionAndFrameOrder)
                    .ToArray();
                if (sprites.Length != 16)
                    throw new InvalidOperationException($"{SpritePath}: 4×4 Sprite 16개를 불러오지 못했습니다.");

                Dictionary<string, AnimationClip> clips = new Dictionary<string, AnimationClip>();
                for (int direction = 0; direction < Directions.Length; direction++)
                {
                    Sprite[] frames = sprites.Skip(direction * 4).Take(4).ToArray();
                    string directionName = Directions[direction];
                    clips[$"Idle_{directionName}"] = CreateClip("Idle", directionName, new[] { frames[0] }, false);
                    clips[$"Walk_{directionName}"] = CreateClip("Walk", directionName, frames, true);
                }

                AnimatorController controller = CreateController(clips);
                ConnectDefinition(controller, sprites[0]);
                AssetDatabase.SaveAssets();
                Debug.Log("초원 슬라임 Sprite Import, 4×4 Slice, Animation 8개와 Animator 연결을 완료했습니다.");
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
            finally
            {
                IsBuilding = false;
            }
        }

        /// <summary>4×4로 정확히 나뉘는 정사각형 Cell인지 검사하여 잘못된 프레임 생성 전에 중단합니다.</summary>
        private static void ValidateSource(Texture2D texture)
        {
            if (texture == null) throw new InvalidOperationException($"초원 슬라임 PNG가 없습니다: {SpritePath}");
            if (texture.width % 4 != 0 || texture.height % 4 != 0)
                throw new InvalidOperationException($"초원 슬라임 PNG는 4×4로 나뉘어야 합니다: {texture.width}×{texture.height}");
            if (texture.width / 4 != texture.height / 4)
                throw new InvalidOperationException("초원 슬라임 Sprite의 각 Cell은 정사각형이어야 합니다.");
        }

        /// <summary>
        /// 위에서 아래로 Down/Left/Right/Up인 원본 행 순서를 Unity의 아래쪽 원점 좌표로 변환해 16개 Sprite를 만듭니다.
        /// 발이 닿는 위치가 움직일 때 흔들리지 않도록 각 Sprite의 Pivot은 아래쪽 중앙으로 통일합니다.
        /// </summary>
        private static void ConfigureAndSlice(int width, int height)
        {
            TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath(SpritePath);
            int cellSize = width / 4;
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Multiple;
            importer.mipmapEnabled = false;
            importer.filterMode = FilterMode.Point;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.alphaIsTransparency = true;
            importer.spritePixelsPerUnit = cellSize;

            TextureImporterSettings settings = new TextureImporterSettings();
            importer.ReadTextureSettings(settings);
            settings.spriteMeshType = SpriteMeshType.FullRect;
            importer.SetTextureSettings(settings);

            SpriteRect[] spriteRects = new SpriteRect[16];
            for (int row = 0; row < 4; row++)
            for (int frame = 0; frame < 4; frame++)
            {
                int index = row * 4 + frame;
                spriteRects[index] = new SpriteRect
                {
                    name = $"{VisualName}_{Directions[row]}_{frame}",
                    rect = new Rect(frame * cellSize, height - ((row + 1) * cellSize), cellSize, cellSize),
                    alignment = SpriteAlignment.BottomCenter,
                    pivot = new Vector2(.5f, 0f),
                    spriteID = GUID.Generate(),
                };
            }

            SpriteDataProviderFactories factories = new SpriteDataProviderFactories();
            factories.Init();
            ISpriteEditorDataProvider provider = factories.GetSpriteEditorDataProviderFromObject(importer);
            provider.InitSpriteEditorDataProvider();
            provider.SetSpriteRects(spriteRects);
            provider.Apply();
            importer.SaveAndReimport();
        }

        /// <summary>Idle은 첫 프레임을 유지하고, Walk는 네 프레임을 초당 약 6장 속도로 반복하는 Clip을 만듭니다.</summary>
        private static AnimationClip CreateClip(string motion, string direction, Sprite[] frames, bool loop)
        {
            string stateName = $"{motion}_{direction}";
            string path = $"{AnimationFolder}/{VisualName}_{stateName}.anim";
            AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
            if (clip == null)
            {
                clip = new AnimationClip();
                AssetDatabase.CreateAsset(clip, path);
            }

            clip.name = $"{VisualName}_{stateName}";
            clip.frameRate = 6f;
            EditorCurveBinding binding = new EditorCurveBinding
            {
                path = string.Empty,
                type = typeof(SpriteRenderer),
                propertyName = "m_Sprite",
            };
            ObjectReferenceKeyframe[] keys = frames.Select((frame, index) => new ObjectReferenceKeyframe
            {
                time = index / 6f,
                value = frame,
            }).ToArray();
            AnimationUtility.SetObjectReferenceCurve(clip, binding, keys);
            AnimationClipSettings clipSettings = AnimationUtility.GetAnimationClipSettings(clip);
            clipSettings.loopTime = loop;
            AnimationUtility.SetAnimationClipSettings(clip, clipSettings);
            EditorUtility.SetDirty(clip);
            return clip;
        }

        /// <summary>Controller를 재생성 가능한 8개 상태로 정리하며 기본 상태는 아래쪽 Idle로 둡니다.</summary>
        private static AnimatorController CreateController(IReadOnlyDictionary<string, AnimationClip> clips)
        {
            AnimatorController controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(ControllerPath);
            if (controller == null) controller = AnimatorController.CreateAnimatorControllerAtPath(ControllerPath);

            AnimatorStateMachine stateMachine = controller.layers[0].stateMachine;
            foreach (ChildAnimatorState child in stateMachine.states) stateMachine.RemoveState(child.state);
            foreach (string stateName in Directions.Select(direction => "Idle_" + direction)
                         .Concat(Directions.Select(direction => "Walk_" + direction)))
                stateMachine.AddState(stateName).motion = clips[stateName];
            stateMachine.defaultState = stateMachine.states.First(child => child.state.name == "Idle_Down").state;
            EditorUtility.SetDirty(controller);
            return controller;
        }

        /// <summary>런타임 설치기가 실제 Sprite와 Animator를 사용하도록 초원 슬라임 데이터 Asset에 생성 결과를 기록합니다.</summary>
        private static void ConnectDefinition(AnimatorController controller, Sprite defaultSprite)
        {
            MonsterDefinition definition = AssetDatabase.LoadAssetAtPath<MonsterDefinition>(DefinitionPath);
            if (definition == null) throw new InvalidOperationException($"MonsterDefinition이 없습니다: {DefinitionPath}");

            SerializedObject serialized = new SerializedObject(definition);
            serialized.FindProperty("fieldSprite").objectReferenceValue = defaultSprite;
            serialized.FindProperty("fieldAnimatorController").objectReferenceValue = controller;
            serialized.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(definition);
        }

        private static bool IsDefinitionConnected()
        {
            MonsterDefinition definition = AssetDatabase.LoadAssetAtPath<MonsterDefinition>(DefinitionPath);
            return definition != null && definition.FieldSprite != null && definition.FieldAnimatorController != null;
        }

        private static int GetDirectionAndFrameOrder(Sprite sprite)
        {
            for (int direction = 0; direction < Directions.Length; direction++)
            for (int frame = 0; frame < 4; frame++)
                if (sprite.name.EndsWith($"_{Directions[direction]}_{frame}", StringComparison.Ordinal))
                    return direction * 4 + frame;
            return int.MaxValue;
        }

        private static void EnsureFolder(string path)
        {
            string current = "Assets";
            foreach (string part in path.Split('/').Skip(1))
            {
                string next = current + "/" + part;
                if (!AssetDatabase.IsValidFolder(next)) AssetDatabase.CreateFolder(current, part);
                current = next;
            }
        }
    }

    /// <summary>초원 슬라임 PNG가 추가되거나 바뀌면 자동 생성기를 다시 실행하도록 Unity Import 완료 시점을 감시합니다.</summary>
    public sealed class GrassSlimeSpritePostprocessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            if (GrassSlimeAnimationBuilder.IsBuilding || !importedAssets.Contains(GrassSlimeAnimationBuilder.SpritePath)) return;
            EditorApplication.delayCall += GrassSlimeAnimationBuilder.ImportAndBuild;
        }
    }
}

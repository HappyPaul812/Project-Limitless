using System;
using System.Collections.Generic;
using System.Linq;
using ProjectLimitless.Core;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace ProjectLimitless.Editor
{
    /// <summary>4×4 전체 캐릭터 시트를 Slice하고 Path 전용 Animation/Animator/Definition을 갱신합니다.</summary>
    public static class PathVisualCharacterVariantBuilder
    {
        private const string AnimationRoot = "Assets/_Project/Animations/Player/PathVariants";
        private static readonly string[] Directions = { "Down", "Left", "Right", "Up" };

        private sealed class VariantSpec
        {
            public string PathId;
            public string VariantName;
            public string SpritePath;
            public bool Female;
            public string Folder;
        }

        private static readonly VariantSpec[] Specs =
        {
            Spec("path.hearing", "Player_Male_Hearing", "Hearing/Male/Player_Male_Hearing_Walk.png", false, "Hearing"),
            Spec("path.hearing", "Player_Female_Hearing", "Hearing/Female/Player_Female_Hearing_Walk.png", true, "Hearing"),
            Spec("path.vision", "Player_Male_Vision", "Vision/Male/Player_Male_Vision_Walk.png", false, "Vision"),
            Spec("path.vision", "Player_Female_Vision", "Vision/Female/Player_Female_Vision_Walk.png", true, "Vision"),
            Spec("path.mobility", "Player_Male_Physical_Wheelchair", "Physical/Male/Player_Male_Physical_Wheelchair_Walk.png", false, "Physical"),
            Spec("path.mobility", "Player_Female_Physical_Wheelchair", "Physical/Female/Player_Female_Physical_Wheelchair_Walk.png", true, "Physical"),
        };

        [InitializeOnLoadMethod]
        private static void ScheduleAutomaticBuild()
        {
            EditorApplication.delayCall += () =>
            {
                if (ValidateSources().Count == 0 && !AreAllVariantsConnected())
                    ImportAndBuildCharacterVariants();
            };
        }

        [MenuItem("Project-Limitless/Path Visuals/Import And Build Character Variants")]
        public static void ImportAndBuildCharacterVariants()
        {
            List<string> errors = ValidateSources();
            if (errors.Count > 0)
            {
                Debug.LogError("Path Visual Variant 생성을 중단했습니다.\n" + string.Join("\n", errors));
                return;
            }

            EnsureFolder(AnimationRoot);
            foreach (VariantSpec spec in Specs) Build(spec);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Path Visual 캐릭터 Variant 6개의 Import, Slice, Animation, Animator, Definition 연결을 완료했습니다.");
        }

        private static List<string> ValidateSources()
        {
            List<string> errors = new List<string>();
            foreach (VariantSpec spec in Specs)
            {
                Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(spec.SpritePath);
                if (texture == null) { errors.Add($"누락: {spec.SpritePath}"); continue; }
                if (texture.width % 4 != 0 || texture.height % 4 != 0)
                { errors.Add($"4×4 분할 불가: {spec.SpritePath} ({texture.width}×{texture.height})"); continue; }
                int cellWidth = texture.width / 4;
                int cellHeight = texture.height / 4;
                if (cellWidth != cellHeight) errors.Add($"Cell이 정사각형이 아님: {spec.SpritePath} ({cellWidth}×{cellHeight})");
            }
            return errors;
        }

        private static void Build(VariantSpec spec)
        {
            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(spec.SpritePath);
            int cellSize = texture.width / 4;
            ConfigureAndSlice(spec, texture.width, texture.height, cellSize);
            Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(spec.SpritePath).OfType<Sprite>().OrderBy(GetDirectionAndFrameOrder).ToArray();
            if (sprites.Length != 16 || sprites.Distinct().Count() != 16) throw new InvalidOperationException($"{spec.SpritePath}: 서로 다른 Sprite 16개가 필요합니다.");

            string folder = $"{AnimationRoot}/{spec.Folder}";
            EnsureFolder(folder);
            Dictionary<string, AnimationClip> clips = new Dictionary<string, AnimationClip>();
            for (int direction = 0; direction < 4; direction++)
            {
                Sprite[] frames = sprites.Skip(direction * 4).Take(4).ToArray();
                clips[$"Idle_{Directions[direction]}"] = CreateClip(folder, spec.VariantName, "Idle", Directions[direction], new[] { frames[0] }, false);
                clips[$"Walk_{Directions[direction]}"] = CreateClip(folder, spec.VariantName, "Walk", Directions[direction], frames, true);
            }

            AnimatorController controller = CreateController(folder, spec.VariantName, clips);
            ConnectDefinition(spec, controller, sprites[0]);
        }

        private static void ConfigureAndSlice(VariantSpec spec, int width, int height, int cellSize)
        {
            TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath(spec.SpritePath);
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Multiple;
            importer.mipmapEnabled = false;
            importer.filterMode = FilterMode.Point;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.alphaIsTransparency = true;
            importer.spritePixelsPerUnit = cellSize;
            TextureImporterSettings textureSettings = new TextureImporterSettings();
            importer.ReadTextureSettings(textureSettings);
            textureSettings.spriteMeshType = SpriteMeshType.FullRect;
            importer.SetTextureSettings(textureSettings);

            SpriteRect[] metadata = new SpriteRect[16];
            for (int row = 0; row < 4; row++)
            for (int frame = 0; frame < 4; frame++)
            {
                int index = row * 4 + frame;
                metadata[index] = new SpriteRect
                {
                    name = $"{spec.VariantName}_{Directions[row]}_{frame}",
                    rect = new Rect(frame * cellSize, height - ((row + 1) * cellSize), cellSize, cellSize),
                    alignment = SpriteAlignment.BottomCenter,
                    pivot = new Vector2(.5f, 0f),
                    spriteID = GUID.Generate(),
                };
            }
            SpriteDataProviderFactories factories = new SpriteDataProviderFactories();
            factories.Init();
            ISpriteEditorDataProvider dataProvider = factories.GetSpriteEditorDataProviderFromObject(importer);
            dataProvider.InitSpriteEditorDataProvider();
            dataProvider.SetSpriteRects(metadata);
            dataProvider.Apply();
            importer.SaveAndReimport();
        }

        private static AnimationClip CreateClip(string folder, string variant, string motion, string direction, Sprite[] frames, bool loop)
        {
            string path = $"{folder}/{variant}_{motion}_{direction}.anim";
            AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
            if (clip == null) { clip = new AnimationClip(); AssetDatabase.CreateAsset(clip, path); }
            clip.name = $"{variant}_{motion}_{direction}";
            clip.frameRate = 6f;
            EditorCurveBinding binding = new EditorCurveBinding { path = string.Empty, type = typeof(SpriteRenderer), propertyName = "m_Sprite" };
            ObjectReferenceKeyframe[] keys = frames.Select((frame, index) => new ObjectReferenceKeyframe { time = index / 6f, value = frame }).ToArray();
            AnimationUtility.SetObjectReferenceCurve(clip, binding, keys);
            AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(clip);
            settings.loopTime = loop;
            AnimationUtility.SetAnimationClipSettings(clip, settings);
            EditorUtility.SetDirty(clip);
            return clip;
        }

        private static AnimatorController CreateController(string folder, string variant, IReadOnlyDictionary<string, AnimationClip> clips)
        {
            string path = $"{folder}/{variant}.controller";
            AnimatorController controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(path);
            if (controller == null) controller = AnimatorController.CreateAnimatorControllerAtPath(path);
            EnsureParameter(controller, "Speed", AnimatorControllerParameterType.Float);
            EnsureParameter(controller, "MoveX", AnimatorControllerParameterType.Float);
            EnsureParameter(controller, "MoveY", AnimatorControllerParameterType.Float);
            AnimatorStateMachine stateMachine = controller.layers[0].stateMachine;
            foreach (ChildAnimatorState child in stateMachine.states) stateMachine.RemoveState(child.state);
            foreach (string stateName in Directions.Select(d => "Idle_" + d).Concat(Directions.Select(d => "Walk_" + d)))
                stateMachine.AddState(stateName).motion = clips[stateName];
            stateMachine.defaultState = stateMachine.states.First(item => item.state.name == "Idle_Down").state;
            EditorUtility.SetDirty(controller);
            return controller;
        }

        private static void ConnectDefinition(VariantSpec spec, AnimatorController controller, Sprite downIdle)
        {
            PlayerPathVisualDefinition definition = AssetDatabase.FindAssets("t:PlayerPathVisualDefinition")
                .Select(AssetDatabase.GUIDToAssetPath).Select(AssetDatabase.LoadAssetAtPath<PlayerPathVisualDefinition>)
                .FirstOrDefault(item => item != null && item.PathId == spec.PathId);
            if (definition == null) throw new InvalidOperationException($"PathVisualDefinition 누락: {spec.PathId}");
            SerializedObject serialized = new SerializedObject(definition);
            SerializedProperty variant = serialized.FindProperty(spec.Female ? "femaleVariant" : "maleVariant");
            variant.FindPropertyRelative("animatorController").objectReferenceValue = controller;
            variant.FindPropertyRelative("defaultDownSprite").objectReferenceValue = downIdle;
            serialized.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(definition);
        }

        private static bool AreAllVariantsConnected()
        {
            foreach (VariantSpec spec in Specs)
            {
                PlayerPathVisualDefinition definition = FindDefinition(spec.PathId);
                if (definition == null) return false;
                SerializedObject serialized = new SerializedObject(definition);
                SerializedProperty variant = serialized.FindProperty(spec.Female ? "femaleVariant" : "maleVariant");
                if (variant.FindPropertyRelative("animatorController").objectReferenceValue == null ||
                    variant.FindPropertyRelative("defaultDownSprite").objectReferenceValue == null) return false;
            }
            return true;
        }

        private static PlayerPathVisualDefinition FindDefinition(string pathId) => AssetDatabase.FindAssets("t:PlayerPathVisualDefinition")
            .Select(AssetDatabase.GUIDToAssetPath).Select(AssetDatabase.LoadAssetAtPath<PlayerPathVisualDefinition>)
            .FirstOrDefault(item => item != null && item.PathId == pathId);

        private static int GetDirectionAndFrameOrder(Sprite sprite)
        {
            for (int direction = 0; direction < Directions.Length; direction++)
            for (int frame = 0; frame < 4; frame++)
                if (sprite.name.EndsWith($"_{Directions[direction]}_{frame}", StringComparison.Ordinal)) return direction * 4 + frame;
            return int.MaxValue;
        }

        private static void EnsureParameter(AnimatorController controller, string name, AnimatorControllerParameterType type)
        { if (!controller.parameters.Any(parameter => parameter.name == name)) controller.AddParameter(name, type); }

        private static void EnsureFolder(string path)
        {
            string current = "Assets";
            foreach (string part in path.Split('/').Skip(1))
            { string next = current + "/" + part; if (!AssetDatabase.IsValidFolder(next)) AssetDatabase.CreateFolder(current, part); current = next; }
        }

        private static VariantSpec Spec(string pathId, string variantName, string relativeSpritePath, bool female, string folder) => new VariantSpec
        { PathId = pathId, VariantName = variantName, SpritePath = "Assets/_Project/Art/Characters/PathVisuals/" + relativeSpritePath, Female = female, Folder = folder };
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using ProjectLimitless.Battle;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.U2D.Sprites;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectLimitless.EditorTools
{
    /// <summary>태온 최종 전투 시트를 명시적 209px Grid와 발 기준 Pivot으로 재생성합니다.</summary>
    public static class TaeonBattleAnimationBuilder
    {
        private const string SheetPath = "Assets/_Project/Resources/BattleCharacters/Taeon/Taeon_Battle_Final.png";
        private const string AnimationFolder = "Assets/_Project/Resources/BattleCharacters/Taeon/Animations";
        private const string ControllerPath = AnimationFolder + "/Taeon_Battle.controller";
        private const int CellSize = 209;
        private const float FramesPerSecond = 10f;

        private static readonly (string Name, int Row, int Count, bool Loop)[] Motions =
        {
            ("Idle", 0, 4, true),
            ("Attack", 1, 6, false),
            ("Guard", 2, 4, false),
            ("Skill", 3, 6, false),
            ("Hit", 4, 4, false),
            ("Defeat", 5, 6, false)
        };

        [MenuItem("Project Limitless/Content/Build Taeon Battle Animation")]
        public static void Build()
        {
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            ConfigureImporterAndSlices();
            EnsureFolder(AnimationFolder);
            Dictionary<string, AnimationClip> clips = CreateClips();
            CreateController(clips);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            Debug.Log("[Taeon Battle] 209x209 Grid, 30 Sprites, 6 Clips, Left 방향 Controller 생성 완료");
        }

        private static void ConfigureImporterAndSlices()
        {
            TextureImporter importer = AssetImporter.GetAtPath(SheetPath) as TextureImporter;
            if (importer == null) throw new InvalidOperationException($"TextureImporter를 찾을 수 없습니다: {SheetPath}");
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Multiple;
            importer.alphaSource = TextureImporterAlphaSource.FromInput;
            importer.alphaIsTransparency = true;
            importer.sRGBTexture = true;
            importer.mipmapEnabled = false;
            importer.filterMode = FilterMode.Point;
            importer.wrapMode = TextureWrapMode.Clamp;
            importer.npotScale = TextureImporterNPOTScale.None;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.maxTextureSize = 2048;
            importer.spritePixelsPerUnit = 100f;
            importer.SaveAndReimport();

            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(SheetPath);
            SpriteDataProviderFactories factories = new SpriteDataProviderFactories();
            factories.Init();
            ISpriteEditorDataProvider provider = factories.GetSpriteEditorDataProviderFromObject(importer);
            provider.InitSpriteEditorDataProvider();
            List<SpriteRect> spriteRects = new List<SpriteRect>();
            foreach ((string motion, int row, int count, bool _) in Motions)
            {
                for (int frame = 0; frame < count; frame++)
                {
                    spriteRects.Add(new SpriteRect
                    {
                        name = $"Taeon_{motion}_{frame:00}",
                        rect = new Rect(frame * CellSize, texture.height - ((row + 1) * CellSize), CellSize, CellSize),
                        alignment = SpriteAlignment.Custom,
                        pivot = new Vector2(.5f, 0f),
                        spriteID = GUID.Generate()
                    });
                }
            }
            provider.SetSpriteRects(spriteRects.ToArray());
            provider.Apply();
            importer.SaveAndReimport();
        }

        private static Dictionary<string, AnimationClip> CreateClips()
        {
            Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(SheetPath).OfType<Sprite>().ToArray();
            Dictionary<string, AnimationClip> result = new Dictionary<string, AnimationClip>(StringComparer.Ordinal);
            foreach ((string motion, int _, int count, bool loop) in Motions)
            {
                string path = $"{AnimationFolder}/Taeon_{motion}.anim";
                AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
                if (clip == null)
                {
                    clip = new AnimationClip { name = $"Taeon_{motion}" };
                    AssetDatabase.CreateAsset(clip, path);
                }
                clip.frameRate = FramesPerSecond;
                Sprite[] frames = Enumerable.Range(0, count)
                    .Select(index => sprites.First(sprite => sprite.name == $"Taeon_{motion}_{index:00}"))
                    .ToArray();
                ObjectReferenceKeyframe[] keys = frames.Select((sprite, index) => new ObjectReferenceKeyframe
                {
                    time = index / FramesPerSecond,
                    value = sprite
                }).ToArray();
                EditorCurveBinding binding = new EditorCurveBinding
                {
                    path = string.Empty,
                    type = typeof(Image),
                    propertyName = "m_Sprite"
                };
                AnimationUtility.SetObjectReferenceCurve(clip, binding, keys);
                AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(clip);
                settings.loopTime = loop;
                settings.stopTime = count / FramesPerSecond;
                AnimationUtility.SetAnimationClipSettings(clip, settings);
                EditorUtility.SetDirty(clip);
                result[motion] = clip;
            }
            return result;
        }

        private static void CreateController(IReadOnlyDictionary<string, AnimationClip> clips)
        {
            AnimatorController controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(ControllerPath);
            if (controller == null) controller = AnimatorController.CreateAnimatorControllerAtPath(ControllerPath);
            foreach (AnimatorControllerParameter parameter in controller.parameters)
                controller.RemoveParameter(parameter);
            foreach (string trigger in new[] { "Attack", "Guard", "Skill", "Hit", "Defeat" })
                controller.AddParameter(trigger, AnimatorControllerParameterType.Trigger);

            AnimatorStateMachine machine = controller.layers[0].stateMachine;
            foreach (AnimatorState state in machine.states.Select(item => item.state).ToArray()) machine.RemoveState(state);
            foreach (AnimatorStateTransition transition in machine.anyStateTransitions.ToArray()) machine.RemoveAnyStateTransition(transition);

            Dictionary<string, AnimatorState> states = new Dictionary<string, AnimatorState>(StringComparer.Ordinal);
            foreach ((string motion, int _, int _, bool loop) in Motions)
            {
                AnimatorState state = machine.AddState(motion);
                state.motion = clips[motion];
                states[motion] = state;
                if (!loop && motion != "Defeat")
                {
                    AnimatorStateTransition back = state.AddTransition(states.TryGetValue("Idle", out AnimatorState idle) ? idle : null);
                    back.hasExitTime = true;
                    back.exitTime = 1f;
                    back.duration = 0f;
                }
            }
            machine.defaultState = states["Idle"];

            // Idle이 먼저 생성되므로 위 루프의 단발 상태 전이를 실제 Idle로 다시 확정합니다.
            foreach (string motion in new[] { "Attack", "Guard", "Skill", "Hit" })
            {
                foreach (AnimatorStateTransition transition in states[motion].transitions.ToArray()) states[motion].RemoveTransition(transition);
                AnimatorStateTransition back = states[motion].AddTransition(states["Idle"]);
                back.hasExitTime = true;
                back.exitTime = 1f;
                back.duration = 0f;
            }
            foreach (string trigger in new[] { "Attack", "Guard", "Skill", "Hit", "Defeat" })
            {
                AnimatorStateTransition transition = machine.AddAnyStateTransition(states[trigger]);
                transition.AddCondition(AnimatorConditionMode.If, 0f, trigger);
                transition.hasExitTime = false;
                transition.duration = 0f;
                transition.canTransitionToSelf = false;
            }
            EditorUtility.SetDirty(controller);
        }

        private static void EnsureFolder(string path)
        {
            string current = "Assets";
            foreach (string part in path.Substring("Assets/".Length).Split('/'))
            {
                string next = current + "/" + part;
                if (!AssetDatabase.IsValidFolder(next)) AssetDatabase.CreateFolder(current, part);
                current = next;
            }
        }
    }
}

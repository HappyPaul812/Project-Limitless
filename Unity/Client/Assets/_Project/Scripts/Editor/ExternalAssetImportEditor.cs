using System;
using System.IO;
using System.Linq;
using ProjectLimitless.Player;
using ProjectLimitless.Battle;
using UnityEditor;
using UnityEngine;

namespace ProjectLimitless.EditorTools
{
    /// <summary>이번에 추가한 파일만 Import하고 Catalog를 채웁니다. Scene·Prefab·기존 Player Animator는 건드리지 않습니다.</summary>
    public static class ExternalAssetImportEditor
    {
        private const string PlayerRoot = "Assets/_Project/Art/Characters/Player/External50/";
        private const string BackgroundRoot = "Assets/_Project/Art/Battle/Backgrounds/Chapter1/";

        public static string Import()
        {
            if (EditorApplication.isPlaying || EditorApplication.isCompiling)
                throw new InvalidOperationException("편입은 컴파일 완료 후 Edit Mode에서만 실행합니다.");
            var inventory = JsonUtility.FromJson<PlayerAppearanceCatalog.Inventory>(
                File.ReadAllText("Assets/_Project/Resources/PlayerAppearances/External50.json"));
            if (inventory.entries.Length != 50 || inventory.entries.Select(e => e.appearanceId).Distinct().Count() != 50)
                throw new InvalidOperationException("외형 50종과 고정 ID 유일성을 먼저 확인해야 합니다.");
            // 실제 기존 기본 플레이어의 현재 PPU를 따릅니다. 원본 픽셀 크기를 바꾸는 Resize는 하지 않습니다.
            var existing = AssetImporter.GetAtPath("Assets/_Project/Art/Characters/Player/Player_Male_Base_Walk_v1.png") as TextureImporter;
            float ppu = existing != null ? existing.spritePixelsPerUnit : 128f;
            foreach (var entry in inventory.entries)
            {
                if (!entry.assetPath.StartsWith(PlayerRoot, StringComparison.Ordinal))
                    throw new InvalidOperationException("외형 Import 범위를 벗어난 경로입니다.");
                var importer = (TextureImporter)AssetImporter.GetAtPath(entry.assetPath);
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Multiple;
                importer.spritePixelsPerUnit = ppu;
                importer.spritePivot = new Vector2(.5f, .5f);
                importer.filterMode = FilterMode.Point;
                importer.textureCompression = TextureImporterCompression.Uncompressed;
                importer.mipmapEnabled = false;
                importer.npotScale = TextureImporterNPOTScale.None;
                importer.maxTextureSize = 2048;
                importer.alphaIsTransparency = entry.hasAlpha;
                // Vision만 README와 실제 크기가 128px Grid로 일치합니다. 번호는 위치일 뿐 방향명으로 추측하지 않습니다.
                if (entry.theme == "Vision" && entry.width == 512 && entry.height == 512)
                {
                    var slices = new SpriteMetaData[16];
                    for (int i = 0; i < slices.Length; i++) slices[i] = new SpriteMetaData
                    {
                        name = Path.GetFileNameWithoutExtension(entry.assetPath) + "_Frame_" + i.ToString("D2"),
                        rect = new Rect(i % 4 * 128, (3 - i / 4) * 128, 128, 128),
                        alignment = (int)SpriteAlignment.Center, pivot = new Vector2(.5f, .5f)
                    };
                    importer.spritesheet = slices;
                }
                else
                {
                    // Unity의 최초 Multiple 전환이 그림의 빈 공간으로 자동 Slice를 만들 수 있습니다.
                    // 방향/Cell이 미정인 40종은 그 추정 Rect를 명시적으로 비웁니다.
                    importer.spritesheet = Array.Empty<SpriteMetaData>();
                }
                importer.SaveAndReimport();
                entry.sheet = AssetDatabase.LoadAssetAtPath<Texture2D>(entry.assetPath);
                entry.frames = AssetDatabase.LoadAllAssetsAtPath(entry.assetPath).OfType<Sprite>().OrderBy(s => s.name).ToArray();
                entry.readyForSelection = false;
            }
            var appearances = LoadOrCreate<PlayerAppearanceCatalog>("Assets/_Project/Resources/PlayerAppearances/External50.asset");
            appearances.SetImportedEntries(inventory.entries);
            EditorUtility.SetDirty(appearances);

            foreach (string file in Directory.GetFiles(BackgroundRoot, "*.png"))
            {
                var importer = (TextureImporter)AssetImporter.GetAtPath(file.Replace('\\', '/'));
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                importer.filterMode = FilterMode.Bilinear;
                importer.textureCompression = TextureImporterCompression.Uncompressed;
                importer.mipmapEnabled = false;
                importer.npotScale = TextureImporterNPOTScale.None;
                importer.maxTextureSize = 2048;
                importer.SaveAndReimport();
            }
            var backgrounds = LoadOrCreate<BattleBackgroundCatalog>("Assets/_Project/Resources/BattleBackgrounds/Chapter1.asset");
            var data = new SerializedObject(backgrounds);
            var scenes = data.FindProperty("scenes");
            string[] sceneNames = { "Field_01", "Field_02", "Field_03", "Dungeon_01", "Dungeon_01_B2" };
            string[] artNames = { "Grassland", "Forest", "Forest", "Silent_Catacombs", "Silent_Catacombs" };
            scenes.arraySize = sceneNames.Length;
            for (int i = 0; i < sceneNames.Length; i++)
            {
                scenes.GetArrayElementAtIndex(i).FindPropertyRelative("sceneName").stringValue = sceneNames[i];
                scenes.GetArrayElementAtIndex(i).FindPropertyRelative("sprite").objectReferenceValue =
                    AssetDatabase.LoadAssetAtPath<Sprite>(BackgroundRoot + "BattleBG_" + artNames[i] + ".png");
            }
            data.FindProperty("silentWarden").objectReferenceValue = AssetDatabase.LoadAssetAtPath<Sprite>(
                BackgroundRoot + "BattleBG_Silent_Catacombs_Boss.png");
            data.ApplyModifiedPropertiesWithoutUndo();
            // 다른 사용자 dirty Asset까지 SaveAssets로 저장하지 않고 이번 Catalog 둘만 저장합니다.
            AssetDatabase.SaveAssetIfDirty(appearances);
            AssetDatabase.SaveAssetIfDirty(backgrounds);
            return "BGM 7곡, Background 6종, Player 50종 편입; Vision 160 Frame; 나머지 40종 Slice 보류; Scene 비변경";
        }

        private static T LoadOrCreate<T>(string path) where T : ScriptableObject
        {
            T asset = AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset != null) return asset;
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, path);
            return asset;
        }
    }
}

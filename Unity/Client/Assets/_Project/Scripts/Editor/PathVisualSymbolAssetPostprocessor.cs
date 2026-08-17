using ProjectLimitless.Core;
using UnityEditor;
using UnityEngine;

namespace ProjectLimitless.Editor
{
    /// <summary>길 Symbol PNG의 Import 설정과 Definition 참조를 경로 기준으로 자동 유지합니다.</summary>
    public sealed class PathVisualSymbolAssetPostprocessor : AssetPostprocessor
    {
        private const string IntellectualSpritePath = "Assets/_Project/Art/Characters/PathVisuals/Intellectual/Path_Intellectual_Companion_Emblem.png";
        private const string EmotionalScarSpritePath = "Assets/_Project/Art/Characters/PathVisuals/EmotionalScar/Path_EmotionalScar_Heart.png";
        private const string IntellectualDefinitionPath = "Assets/_Project/Resources/PathVisualDefinitions/03_IntellectualVisual.asset";
        private const string EmotionalScarDefinitionPath = "Assets/_Project/Resources/PathVisualDefinitions/05_EmotionalScarVisual.asset";

        private void OnPreprocessTexture()
        {
            if (!IsManagedSymbol(assetPath)) return;
            TextureImporter importer = (TextureImporter)assetImporter;
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.mipmapEnabled = false;
            importer.filterMode = FilterMode.Point;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.alphaIsTransparency = true;
        }

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (string imported in importedAssets)
            {
                if (!IsManagedSymbol(imported)) continue;
                EditorApplication.delayCall -= ConnectAllSymbols;
                EditorApplication.delayCall += ConnectAllSymbols;
                break;
            }
        }

        [MenuItem("Project-Limitless/Path Visuals/Import And Connect Symbols")]
        public static void ImportAndConnectSymbols()
        {
            AssetDatabase.ImportAsset(IntellectualSpritePath, ImportAssetOptions.ForceUpdate);
            AssetDatabase.ImportAsset(EmotionalScarSpritePath, ImportAssetOptions.ForceUpdate);
            ConnectAllSymbols();
        }

        private static void ConnectAllSymbols()
        {
            ConnectSymbol(IntellectualSpritePath, IntellectualDefinitionPath);
            ConnectSymbol(EmotionalScarSpritePath, EmotionalScarDefinitionPath);
            AssetDatabase.SaveAssets();
        }

        private static void ConnectSymbol(string spritePath, string definitionPath)
        {
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
            PlayerPathVisualDefinition definition = AssetDatabase.LoadAssetAtPath<PlayerPathVisualDefinition>(definitionPath);
            if (sprite == null || definition == null) return;

            SerializedObject serializedDefinition = new SerializedObject(definition);
            SerializedProperty symbolProperty = serializedDefinition.FindProperty("symbolSprite");
            if (symbolProperty.objectReferenceValue == sprite) return;
            symbolProperty.objectReferenceValue = sprite;
            serializedDefinition.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(definition);
        }

        private static bool IsManagedSymbol(string path) => path == IntellectualSpritePath || path == EmotionalScarSpritePath;
    }
}

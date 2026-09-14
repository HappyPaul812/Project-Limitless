#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace ProjectLimitless.EditorTools
{
    /// <summary>승인된 Eldiran 파생 NPC Sprite의 비파괴 Import 설정을 재현합니다.</summary>
    public static class EldiranNpcImportSettings
    {
        private const string DerivedFolder = "Assets/_Project/Resources/VillageNpcSprites/Eldiran";

        [MenuItem("LIMITLESS/Assets/Apply Eldiran NPC Import Settings")]
        public static void Apply()
        {
            string[] guids = AssetDatabase.FindAssets("t:Texture2D", new[] { DerivedFolder });
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if (AssetImporter.GetAtPath(path) is not TextureImporter importer) continue;

                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                importer.spritePixelsPerUnit = 28f;
                importer.filterMode = FilterMode.Point;
                importer.mipmapEnabled = false;
                importer.alphaIsTransparency = true;
                importer.textureCompression = TextureImporterCompression.Uncompressed;
                TextureImporterSettings settings = new TextureImporterSettings();
                importer.ReadTextureSettings(settings);
                settings.spriteAlignment = (int)SpriteAlignment.Custom;
                settings.spritePivot = new Vector2(0.5f, 0f);
                importer.SetTextureSettings(settings);
                importer.SaveAndReimport();
            }

            AssetDatabase.SaveAssets();
            Debug.Log($"Eldiran NPC 파생 Sprite Import 설정 완료: {guids.Length}개");
        }
    }
}
#endif

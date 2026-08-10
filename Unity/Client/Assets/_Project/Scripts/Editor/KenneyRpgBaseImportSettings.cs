#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace ProjectLimitless.EditorTools
{
    /// <summary>Kenney RPG Base PNG를 2D 픽셀 아트 Sprite로 일관되게 가져온다.</summary>
    public sealed class KenneyRpgBaseImportSettings : AssetPostprocessor
    {
        public const string PngRoot = "Assets/ThirdParty/Kenney/RPGBase/PNG/";

        private void OnPreprocessTexture()
        {
            if (!assetPath.StartsWith(PngRoot) || !assetPath.EndsWith(".png"))
            {
                return;
            }

            TextureImporter importer = (TextureImporter)assetImporter;
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.spritePixelsPerUnit = 64;
            importer.filterMode = FilterMode.Point;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.mipmapEnabled = false;
            importer.alphaIsTransparency = true;
        }
    }
}
#endif

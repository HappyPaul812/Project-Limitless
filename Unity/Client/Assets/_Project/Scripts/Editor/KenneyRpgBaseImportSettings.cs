#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace ProjectLimitless.EditorTools
{
    /// <summary>
    /// Kenney RPG Base PNG를 Unity 프로젝트에 가져올 때 픽셀 아트용 설정을 자동 적용합니다.
    /// AssetPostprocessor는 Unity Editor의 가져오기 도구이며, 빌드된 실제 게임에서는 실행되지 않습니다.
    /// </summary>
    public sealed class KenneyRpgBaseImportSettings : AssetPostprocessor
    {
        // 이 폴더 아래의 PNG만 프로젝트 규칙에 맞게 자동 설정합니다.
        public const string PngRoot = "Assets/ThirdParty/Kenney/RPGBase/PNG/";

        /// <summary>대상 PNG를 가져오기 직전에 Sprite 크기·필터·압축 설정을 통일합니다.</summary>
        private void OnPreprocessTexture()
        {
            if (!assetPath.StartsWith(PngRoot) || !assetPath.EndsWith(".png"))
            {
                return;
            }

            // TextureImporter는 이미지가 Unity 안에서 어떤 방식으로 쓰일지를 정하는 Editor 전용 도구입니다.
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

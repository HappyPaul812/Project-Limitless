#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ProjectLimitless.EditorTools
{
    /// <summary>초보 마을 전용 오리지널 환경15종의 등록 경로를 고정합니다. 다른 지역의 무료 원본은 건드리지 않습니다.</summary>
    public static class StarterVillageRenewalArt
    {
        private const string Root = "Assets/_Project/Art/Environment/StarterVillage/V4/";
        private static readonly Dictionary<string, string> Paths = new Dictionary<string, string>
        {
            { "tiles_grass_4_0", "Ground/LL_C1_SV_Ground_Grass_128_v1.png" },
            { "tiles_grass_5_4", "Ground/LL_C1_SV_Path_Right_128_v1.png" },
            { "tiles_grass_4_4", "Ground/LL_C1_SV_Path_Left_128_v1.png" },
            { "house_tiles_new_4_4", "Buildings/LL_C1_SV_House_Roof_Center_128_v2.png" },
            { "house_tiles_new_1_3", "Buildings/LL_C1_SV_House_Wall_Window_128_v2.png" },
            { "house_tiles_new_1_1", "Buildings/LL_C1_SV_House_Door_128_v4.png" },
            { "fence_tiles_2_2", "Props/LL_C1_SV_Fence_Wood_128_v1.png" },
            { "tree_medium", "Props/LL_C1_SV_Tree_Medium_128x156_v1.png" },
            { "tree_big", "Props/LL_C1_SV_Tree_Big_255x256_v1.png" },
            { "bush_01", "Props/LL_C1_SV_Bush_A_128_v1.png" },
            { "bush_02", "Props/LL_C1_SV_Bush_B_128_v1.png" },
            { "rock_01", "Props/LL_C1_SV_Rock_128_v1.png" },
            { "Wooden_Barrel_Type_A", "Props/LL_C1_SV_Barrel_128_v1.png" },
            { "Wooden_Chest_Type_A", "Props/LL_C1_SV_Chest_128_v1.png" },
            { "Campfire_Type_A", "Props/LL_C1_SV_Campfire_128_v1.png" },
        };

        /// <summary>기존 역할 이름을 신규 Sprite로 대응합니다. 누락 시 무료 에셋으로 조용히 되돌리지 않고 생성 작업을 중단합니다.</summary>
        public static Sprite Load(string legacyRole)
        {
            if (!Paths.TryGetValue(legacyRole, out string relativePath))
                throw new InvalidOperationException("마을 환경 역할이 등록되지 않았습니다: " + legacyRole);
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(Root + relativePath);
            if (sprite == null)
                throw new InvalidOperationException("마을 오리지널 Sprite가 없습니다: " + Root + relativePath);
            return sprite;
        }

        /// <summary>마을 재생성 전 전용15종의 Import 계약만 확인·보완합니다. ThirdParty Importer와 GUID를 변경하지 않습니다.</summary>
        public static void ConfigureImports()
        {
            foreach (string relativePath in Paths.Values)
            {
                string path = Root + relativePath;
                TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
                if (importer == null) throw new InvalidOperationException("마을 Importer가 없습니다: " + path);
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                importer.spritePixelsPerUnit = 128;
                TextureImporterSettings settings = new TextureImporterSettings();
                importer.ReadTextureSettings(settings);
                settings.spriteAlignment = (int)SpriteAlignment.Center;
                settings.spritePivot = new Vector2(0.5f, 0.5f);
                settings.spriteMeshType = SpriteMeshType.FullRect;
                importer.SetTextureSettings(settings);
                importer.filterMode = FilterMode.Point;
                importer.wrapMode = TextureWrapMode.Clamp;
                importer.textureCompression = TextureImporterCompression.Uncompressed;
                importer.mipmapEnabled = false;
                importer.npotScale = TextureImporterNPOTScale.None;
                importer.SaveAndReimport();
            }
        }
    }
}
#endif

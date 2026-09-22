#if UNITY_EDITOR
using System.Linq;
using ProjectLimitless.Core;
using ProjectLimitless.Monster;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace ProjectLimitless.Editor
{
    /// <summary>사용자 최종 원본을 변경하지 않고 묘지 망자 Sprite와 데이터 Asset을 구성합니다.</summary>
    public static class GraveWightContentBuilder
    {
        private const string SheetPath = "Assets/_Project/Art/Monsters/07_GraveWight/Grave_Wight_Battle_Final.png";
        private const string MonsterPath = "Assets/_Project/Resources/MonsterDefinitions/07_GraveWight.asset";
        private const string ItemPath = "Assets/_Project/Resources/ItemDefinitions/GraveWightFragment.asset";
        private const int Columns = 6;
        private const int CellWidth = 209;

        [MenuItem("Project Limitless/Content/Build Grave Wight")]
        public static void Build()
        {
            SliceSheet();
            ItemDefinition fragment = LoadOrCreate<ItemDefinition>(ItemPath);
            fragment.ConfigureContent("material_grave_wight_fragment", "망자의 파편",
                "오래된 지하묘지의 망자에게서 떨어져 나온 마력의 파편.",
                ItemCategory.Material, 99, 0, 8, null, Color.white, ItemUseType.None, "판매 재료");
            fragment.ConfigureEffect(ItemEffectType.None, 0);
            EditorUtility.SetDirty(fragment);

            Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(SheetPath).OfType<Sprite>().OrderBy(sprite => sprite.name).ToArray();
            Sprite[] Row(int row) => sprites.Where(sprite => sprite.name.Contains($"_R{row}_")).Take(6).ToArray();
            MonsterLootEntry loot = new MonsterLootEntry();
            loot.Configure("material_grave_wight_fragment", .35f);
            MonsterDefinition shadeBat = Resources.LoadAll<MonsterDefinition>("MonsterDefinitions")
                .FirstOrDefault(definition => definition.MonsterId == "monster_shade_bat");
            MonsterDefinition graveWight = LoadOrCreate<MonsterDefinition>(MonsterPath);
            graveWight.ConfigureContent("monster_grave_wight", "묘지 망자", 7, 32, 9, 125, 200, 8,
                .42f, new Vector2(.9f, 1.08f), .95f, Row(0)[0], Row(0), Row(1), Row(2), Row(3), Row(4),
                shadeBat, new[] { loot });
            EditorUtility.SetDirty(graveWight);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("묘지 망자 Sprite 30프레임과 Monster/Item Definition 생성을 완료했습니다.");
        }

        /// <summary>실제 Alpha 영역을 측정한 5개 행 위치로 잘라 행 경계에 걸친 Hit·Defeat도 보존합니다.</summary>
        private static void SliceSheet()
        {
            AssetDatabase.ImportAsset(SheetPath, ImportAssetOptions.ForceUpdate);
            TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath(SheetPath);
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Multiple;
            importer.spritePixelsPerUnit = CellWidth;
            importer.filterMode = FilterMode.Point;
            importer.wrapMode = TextureWrapMode.Clamp;
            importer.mipmapEnabled = false;
            importer.alphaIsTransparency = true;
            importer.sRGBTexture = true;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.maxTextureSize = 2048;
            importer.SaveAndReimport();

            int[] rowTop = { 20, 245, 455, 680, 890 };
            const int cellHeight = 220;
            SpriteRect[] rects = new SpriteRect[30];
            for (int row = 0; row < 5; row++)
            {
                int y = 1254 - rowTop[row] - cellHeight;
                for (int column = 0; column < Columns; column++)
                {
                    int index = row * Columns + column;
                    rects[index] = new SpriteRect
                    {
                        name = $"GraveWight_R{row}_F{column:00}",
                        rect = new Rect(column * CellWidth, y, CellWidth, cellHeight),
                        alignment = SpriteAlignment.BottomCenter,
                        pivot = new Vector2(.5f, 0f),
                        spriteID = GUID.Generate()
                    };
                }
            }

            SpriteDataProviderFactories factories = new SpriteDataProviderFactories();
            factories.Init();
            ISpriteEditorDataProvider provider = factories.GetSpriteEditorDataProviderFromObject(importer);
            provider.InitSpriteEditorDataProvider();
            provider.SetSpriteRects(rects);
            provider.Apply();
            importer.SaveAndReimport();
        }

        private static T LoadOrCreate<T>(string path) where T : ScriptableObject
        {
            T asset = AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset != null) return asset;
            asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, path);
            return asset;
        }
    }
}
#endif

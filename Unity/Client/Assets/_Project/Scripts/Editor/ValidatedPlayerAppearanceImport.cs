using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;
using ProjectLimitless.Player;

namespace ProjectLimitless.EditorTools
{
    /// <summary>검증본 50종만 편입합니다. 원본 PNG와 기존 Player/Scene/Save는 변경하지 않습니다.</summary>
    public static class ValidatedPlayerAppearanceImport
    {
        private const string Root = "Assets/_Project/Art/Characters/Player/Validated50/";
        private const string InventoryPath = "Assets/_Project/Resources/PlayerAppearances/Validated50.json";
        private const string CatalogPath = "Assets/_Project/Resources/PlayerAppearances/External50.asset";

        public static string Import()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isCompiling)
                throw new InvalidOperationException("Edit Mode 컴파일 완료 후 실행해야 합니다.");
            var inventory = JsonUtility.FromJson<PlayerAppearanceCatalog.Inventory>(File.ReadAllText(InventoryPath));
            var catalog = AssetDatabase.LoadAssetAtPath<PlayerAppearanceCatalog>(CatalogPath);
            // 기존 ID와 Catalog GUID를 유지해야 미래 Save 연결에서 외형 식별이 바뀌지 않습니다.
            if (catalog == null || inventory?.entries == null || inventory.entries.Length != 50 ||
                inventory.entries.Select(e => e.appearanceId).Distinct().Count() != 50 ||
                !inventory.entries.Select(e => e.appearanceId).OrderBy(x => x)
                    .SequenceEqual(catalog.Entries.Select(e => e.appearanceId).OrderBy(x => x)))
                throw new InvalidOperationException("50개 기존 고정 ID와 입력 목록이 일치하지 않습니다.");
            foreach (var entry in inventory.entries)
                if (!entry.assetPath.StartsWith(Root, StringComparison.Ordinal) ||
                    entry.assetPath.Contains("..") || !File.Exists(entry.assetPath) ||
                    entry.width != 512 || entry.height != 512 || !entry.hasAlpha || entry.expectedFrameCount != 16)
                    throw new InvalidOperationException("검증본 경로 또는 규격이 올바르지 않습니다.");

            var factory = new SpriteDataProviderFactories();
            factory.Init();
            foreach (var entry in inventory.entries)
            {
                var importer = AssetImporter.GetAtPath(entry.assetPath) as TextureImporter;
                if (importer == null) throw new InvalidOperationException("Texture Importer 누락: " + entry.assetPath);
                var provider = factory.GetSpriteEditorDataProviderFromObject(importer);
                if (provider == null) throw new InvalidOperationException("Sprite Data Provider 누락");
                provider.InitSpriteEditorDataProvider();
                var edit = provider.GetDataProvider<ISpriteFrameEditCapability>();
                var required = new[] { EEditCapability.CreateAndDeleteSprite, EEditCapability.EditSpriteRect,
                    EEditCapability.EditSpriteName, EEditCapability.EditPivot };
                if (edit == null || required.Any(c => !edit.GetEditCapability().HasCapability(c)))
                    throw new InvalidOperationException("Sprite 편집 권한 부족: 편입 중단");
                var names = provider.GetDataProvider<ISpriteNameFileIdDataProvider>();
                if (names == null) throw new InvalidOperationException("Sprite 이름/ID Provider 누락");

                // 실제 정식 128px Player와 같은 크기·필터·Pivot을 사용합니다.
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Multiple;
                importer.spritePixelsPerUnit = 128;
                importer.filterMode = FilterMode.Point;
                importer.textureCompression = TextureImporterCompression.Uncompressed;
                importer.alphaSource = TextureImporterAlphaSource.FromInput;
                importer.alphaIsTransparency = true;
                importer.mipmapEnabled = false;
                importer.maxTextureSize = 512;
                var old = provider.GetSpriteRects();
                var rects = new SpriteRect[16];
                string basename = Path.GetFileNameWithoutExtension(entry.assetPath);
                for (int i = 0; i < rects.Length; i++)
                {
                    string name = basename + "_Frame_" + i.ToString("00");
                    var previous = old.FirstOrDefault(r => r.name == name);
                    // 재편입 때 Sprite ID를 보존하여 Catalog 및 미래 Clip 참조가 끊어지지 않습니다.
                    rects[i] = new SpriteRect { name = name,
                        spriteID = previous != null ? previous.spriteID : GUID.Generate(),
                        rect = new Rect(i % 4 * 128, (3 - i / 4) * 128, 128, 128),
                        alignment = SpriteAlignment.BottomCenter, pivot = new Vector2(0.5f, 0) };
                }
                provider.SetSpriteRects(rects);
                names.SetNameFileIdPairs(rects.Select(r => new SpriteNameFileIdPair(r.name, r.spriteID)));
                provider.Apply();
                importer.SaveAndReimport();
                entry.sheet = AssetDatabase.LoadAssetAtPath<Texture2D>(entry.assetPath);
                entry.frames = AssetDatabase.LoadAllAssetsAtPath(entry.assetPath).OfType<Sprite>()
                    .OrderBy(s => s.name, StringComparer.Ordinal).ToArray();
                if (entry.sheet == null || entry.frames.Length != 16)
                    throw new InvalidOperationException("시트 또는 16개 프레임 참조 누락");
                // 방향 행 계약과 발 위치 육안 검수 전에는 기존 Male/Female+Path fallback을 유지합니다.
                entry.readyForSelection = false;
            }
            // 전체 편입 성공 후 이 Catalog만 저장합니다. 다른 사용자 dirty Asset은 저장하지 않습니다.
            catalog.SetImportedEntries(inventory.entries);
            EditorUtility.SetDirty(catalog);
            AssetDatabase.SaveAssetIfDirty(catalog);
            return "Registered 50 appearances / 800 sprites; selection remains pending visual review.";
        }
    }
}

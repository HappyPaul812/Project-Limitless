#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace ProjectLimitless.Editor
{
    /// <summary>
    /// 사용자 원본 PNG를 바꾸지 않고 Unity Importer의 Sprite Rect만 등록합니다.
    /// Editor 코드로 실행하며 이미 같은 이름·개수로 Slice된 파일은 다시 건드리지 않습니다.
    /// </summary>
    public static class Dungeon01B2SpriteSlicer
    {
        public static void SliceAll()
        {
            SliceMonster("Assets/_Project/Resources/MonsterSheets/Silent_Echo_128x128_Sprite_Sheet.png", "SilentEcho");
            SliceMonster("Assets/_Project/Resources/MonsterSheets/Seal_Guardian_128x128_Sprite_Sheet.png", "SealGuardian");
            SliceMonster("Assets/_Project/Resources/MonsterSheets/Silent_Warden_128x128_Sprite_Sheet.png", "SilentWarden");
            SliceIcons("Assets/_Project/Resources/BattleItemIcons/Battle_Item_Icons_128x128_Sheet.png");
        }

        private static void SliceMonster(string path, string prefix)
        {
            var names = new List<string>();
            var rects = new List<Rect>();
            string[] actions = { "Idle", "Move", "Attack", "Hit", "Death" };
            for (int row = 0; row < 5; row++)
            {
                int count = row == 3 ? 2 : 4;
                for (int column = 0; column < count; column++)
                {
                    names.Add(prefix + "_" + actions[row] + "_" + column.ToString("00"));
                    rects.Add(new Rect(column * 128, (4 - row) * 128, 128, 128));
                }
            }
            Slice(path, 512, 640, names, rects);
        }

        private static void SliceIcons(string path)
        {
            string[] names = { "Battle_Item_Icon", "Antidote_Icon", "Burn_Ointment_Icon",
                "Insulation_Potion_Icon", "Silence_Cure_Potion_Icon" };
            var rects = new List<Rect>();
            for (int column = 0; column < names.Length; column++)
                rects.Add(new Rect(column * 128, 0, 128, 128));
            Slice(path, 640, 128, names, rects);
        }

        private static void Slice(string path, int width, int height, IReadOnlyList<string> names,
            IReadOnlyList<Rect> rects)
        {
            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            if (texture == null || texture.width != width || texture.height != height)
                throw new InvalidOperationException("사용자 최종 시트 규격이 다릅니다: " + path);
            var importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer == null) throw new InvalidOperationException("TextureImporter를 찾지 못했습니다: " + path);
            if (importer.spriteImportMode != SpriteImportMode.Multiple || importer.textureType != TextureImporterType.Sprite ||
                importer.spritePixelsPerUnit != 128 || importer.filterMode != FilterMode.Point ||
                importer.mipmapEnabled || importer.textureCompression != TextureImporterCompression.Uncompressed ||
                !importer.alphaIsTransparency || importer.wrapMode != TextureWrapMode.Clamp)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Multiple;
                importer.spritePixelsPerUnit = 128;
                importer.filterMode = FilterMode.Point;
                importer.mipmapEnabled = false;
                importer.textureCompression = TextureImporterCompression.Uncompressed;
                importer.alphaIsTransparency = true;
                importer.sRGBTexture = true;
                importer.wrapMode = TextureWrapMode.Clamp;
                importer.SaveAndReimport();
            }

            var factory = new SpriteDataProviderFactories();
            factory.Init();
            ISpriteEditorDataProvider provider = factory.GetSpriteEditorDataProviderFromObject(importer);
            if (provider == null) throw new InvalidOperationException("Sprite Data Provider가 없습니다: " + path);
            provider.InitSpriteEditorDataProvider();
            ISpriteFrameEditCapability edit = provider.GetDataProvider<ISpriteFrameEditCapability>();
            if (edit == null || !edit.GetEditCapability().HasCapability(EEditCapability.CreateAndDeleteSprite))
                throw new InvalidOperationException("Sprite Slice 기능을 지원하지 않는 Importer입니다: " + path);
            SpriteRect[] current = provider.GetSpriteRects();
            if (current.Length == names.Count && names.All(name => current.Any(item => item.name == name))) return;
            var sprites = new SpriteRect[names.Count];
            for (int index = 0; index < names.Count; index++)
            {
                sprites[index] = new SpriteRect
                {
                    name = names[index], rect = rects[index], alignment = SpriteAlignment.Center,
                    spriteID = GUID.Generate()
                };
            }
            provider.SetSpriteRects(sprites);
            provider.Apply();
            importer.SaveAndReimport();
        }
    }
}
#endif

#if UNITY_EDITOR
using System;
using System.Linq;
using ProjectLimitless.Core;
using ProjectLimitless.Monster;
using UnityEditor;
using UnityEngine;

namespace ProjectLimitless.Editor
{
    /// <summary>
    /// 사용자가 제공한 최종 Slice Sprite를 정의·아이템에 연결합니다. 원본 PNG는 수정하지 않고
    /// 프레임 수와 이름을 확인한 뒤에만 Asset을 생성해 잘못된 시트가 게임에 들어가지 않게 합니다.
    /// </summary>
    public static class Dungeon01B2MonsterBuilder
    {
        private const string MonsterRoot = "Assets/_Project/Resources/MonsterDefinitions/";
        private const string SheetRoot = "Assets/_Project/Resources/MonsterSheets/";
        private const string ItemIconPath = "Assets/_Project/Resources/BattleItemIcons/Battle_Item_Icons_128x128_Sheet.png";

        [MenuItem("Project Limitless/Content/Connect Dungeon 01 B2 Final Sprites")]
        public static void Connect()
        {
            ConnectMonster("08_SilentEcho", "Silent_Echo_128x128_Sprite_Sheet", "SilentEcho",
                "monster_silent_echo", "침묵의 잔영", 8, 90, 180, 14, 34, 10, .95f);
            ConnectMonster("09_SealGuardian", "Seal_Guardian_128x128_Sprite_Sheet", "SealGuardian",
                "monster_seal_guardian", "봉인 수호체", 9, 165, 190, 7, 40, 12, 1.05f);
            ConnectMonster("10_SilentWarden", "Silent_Warden_128x128_Sprite_Sheet", "SilentWarden",
                "monster_silent_warden", "침묵의 파수꾼", 10, 620, 240, 6, 120, 35, 1.35f);
            ConnectItemIcons();
            AssetDatabase.SaveAssets();
            Debug.Log("Dungeon_01 B2 최종 Sprite와 아이템 아이콘 연결을 완료했습니다.");
        }

        private static void ConnectMonster(string assetName, string sheetFile, string prefix, string id,
            string displayName, int level, int hp, int attackPercent, int agility, int exp, int talent, float scale)
        {
            string sheetPath = SheetRoot + sheetFile + ".png";
            Sprite[] sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(sheetPath).OfType<Sprite>().ToArray();
            Sprite[] idle = ReadFrames(sprites, prefix + "_Idle_", 4);
            Sprite[] walk = ReadFrames(sprites, prefix + "_Move_", 4);
            Sprite[] attack = ReadFrames(sprites, prefix + "_Attack_", 4);
            Sprite[] hit = ReadFrames(sprites, prefix + "_Hit_", 2);
            Sprite[] defeat = ReadFrames(sprites, prefix + "_Death_", 4);
            string assetPath = MonsterRoot + assetName + ".asset";
            MonsterDefinition definition = AssetDatabase.LoadAssetAtPath<MonsterDefinition>(assetPath);
            if (definition == null)
            {
                definition = ScriptableObject.CreateInstance<MonsterDefinition>();
                AssetDatabase.CreateAsset(definition, assetPath);
            }
            // 프로젝트의 기준 공격력 10에 대한 비율이므로 180/190/240은 실제 Attack 18/19/24입니다.
            definition.ConfigureContent(id, displayName, level, exp, talent, hp, attackPercent, agility,
                .42f, new Vector2(.9f, 1.08f), scale, idle[0], idle, walk, attack, hit, defeat,
                null, Array.Empty<MonsterLootEntry>());
            EditorUtility.SetDirty(definition);
        }

        private static Sprite[] ReadFrames(Sprite[] sprites, string namePrefix, int expected)
        {
            Sprite[] frames = sprites.Where(item => item.name.StartsWith(namePrefix, StringComparison.Ordinal))
                .OrderBy(item => item.name, StringComparer.Ordinal).ToArray();
            if (frames.Length != expected)
                throw new InvalidOperationException($"{namePrefix} 프레임이 {expected}개여야 합니다. 현재 {frames.Length}개입니다.");
            return frames;
        }

        private static void ConnectItemIcons()
        {
            Sprite[] icons = AssetDatabase.LoadAllAssetRepresentationsAtPath(ItemIconPath).OfType<Sprite>().ToArray();
            string[] names = { "Battle_Item_Icon", "Antidote_Icon", "Burn_Ointment_Icon",
                "Insulation_Potion_Icon", "Silence_Cure_Potion_Icon" };
            foreach (string name in names)
                if (!icons.Any(item => item.name == name))
                    throw new InvalidOperationException("아이템 시트의 Slice가 없습니다: " + name);
            string[] ids = { "item_poison_antidote", "item_burn_ointment",
                "item_shock_insulation_potion", "item_silence_remedy_potion" };
            for (int index = 0; index < ids.Length; index++)
            {
                ItemDefinition item = Resources.LoadAll<ItemDefinition>("ItemDefinitions")
                    .FirstOrDefault(entry => entry.ItemId == ids[index]);
                if (item == null) throw new InvalidOperationException("아이템 정의가 없습니다: " + ids[index]);
                SerializedObject serialized = new SerializedObject(item);
                serialized.FindProperty("icon").objectReferenceValue = icons.First(entry => entry.name == names[index + 1]);
                serialized.ApplyModifiedPropertiesWithoutUndo();
                EditorUtility.SetDirty(item);
            }
        }
    }
}
#endif

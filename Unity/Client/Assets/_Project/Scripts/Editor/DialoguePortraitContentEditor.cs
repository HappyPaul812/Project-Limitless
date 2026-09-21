#if UNITY_EDITOR
using ProjectLimitless.NPC;
using ProjectLimitless.UI;
using UnityEditor;
using UnityEngine;

namespace ProjectLimitless.Editor
{
    /// <summary>핵심 NPC 초상화 슬롯을 생성합니다. 기존에 연결한 Sprite는 다시 실행해도 보존합니다.</summary>
    public static class DialoguePortraitContentEditor
    {
        private const string ResourceRoot = "Assets/_Project/Resources";
        private const string Folder = ResourceRoot + "/DialoguePortraitDefinitions";

        [MenuItem("Project Limitless/UI/Create Core Dialogue Portrait Slots")]
        public static void CreateCoreSlots()
        {
            EnsureFolder(ResourceRoot);
            EnsureFolder(Folder);
            CreateOrUpdate("VillageRepresentative", MainQuest01NpcFlow.RepresentativeId, "주민 대표");
            CreateOrUpdate("SouthGateGuard", MainQuest01NpcFlow.GuardId, "남문 경비병");
            CreateOrUpdate("Taeon", DialoguePortraitCatalog.TaeonId, "태온");
            CreateOrUpdate("Miel", DialoguePortraitCatalog.MielId, "미엘");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            DialoguePortraitCatalog.ReloadInEditor();
            Debug.Log("[DialoguePortrait] 핵심 NPC 4명의 stable ID 초상화 슬롯을 준비했습니다.");
        }

        private static void CreateOrUpdate(string assetName, string speakerId, string displayName)
        {
            string path = $"{Folder}/{assetName}.asset";
            DialoguePortraitDefinition definition = AssetDatabase.LoadAssetAtPath<DialoguePortraitDefinition>(path);
            if (definition == null)
            {
                definition = ScriptableObject.CreateInstance<DialoguePortraitDefinition>();
                AssetDatabase.CreateAsset(definition, path);
            }

            definition.ConfigureContent(speakerId, displayName, definition.Portrait);
            EditorUtility.SetDirty(definition);
        }

        private static void EnsureFolder(string path)
        {
            if (AssetDatabase.IsValidFolder(path)) return;
            int split = path.LastIndexOf('/');
            AssetDatabase.CreateFolder(path.Substring(0, split), path.Substring(split + 1));
        }
    }
}
#endif

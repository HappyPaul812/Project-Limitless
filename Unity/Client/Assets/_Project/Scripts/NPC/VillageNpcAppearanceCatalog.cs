using System.Collections.Generic;
using ProjectLimitless.Core;
using UnityEngine;

namespace ProjectLimitless.NPC
{
    /// <summary>시작 마을 stable NPC ID와 승인된 외형 리소스를 연결합니다.</summary>
    public static class VillageNpcAppearanceCatalog
    {
        private const int NpcSortingOrder = 5;
        private const int NameplateSortingOrder = 7;
        private const int NameplateFontSize = 40;
        private const float NameplateCharacterSize = 0.075f;
        private const float LongNameplateCharacterSize = 0.065f;
        private static readonly Vector3 NameplateOffset = new Vector3(0f, 1.02f, 0f);
        private static readonly Vector3 NameplateShadowOffset = new Vector3(0.018f, -0.018f, 0f);

        // 표시 이름은 번역이나 기획 과정에서 바뀔 수 있으므로 외형 선택에 사용하지 않습니다.
        // 기능에서 사용하는 stable ID와 ThirdParty 파생 Sprite 경로를 연결해야 외형 변경이 대사에 영향을 주지 않습니다.
        private static readonly IReadOnlyDictionary<string, string> ResourcePaths =
            new Dictionary<string, string>
            {
                ["starter-village-general-shop"] = "VillageNpcSprites/Eldiran/OGA03_GeneralShop",
                ["starter-village-equipment-shop"] = "VillageNpcSprites/Eldiran/OGA17_EquipmentShop",
                ["starter-village-healer"] = "VillageNpcSprites/Eldiran/OGA06_Healer",
                ["starter-village-bank"] = "VillageNpcSprites/Eldiran/OGA16_Bank",
                ["starter-village-party-manager"] = "VillageNpcSprites/Eldiran/OGA10_PartyManager",
                ["starter-village-training-guide"] = "VillageNpcSprites/Eldiran/OGA02_TrainingGuide",
                ["starter-village-resident-01"] = "VillageNpcSprites/Eldiran/OGA07_Resident01",
                ["starter-village-resident-02"] = "VillageNpcSprites/Eldiran/OGA11_Resident02",
                ["starter-village-resident-03"] = "VillageNpcSprites/Eldiran/OGA13_Resident03",
                ["starter-village-resident-04"] = "VillageNpcSprites/Eldiran/OGA19_Resident04",
                ["starter-village-gate-guard"] = "VillageNpcSprites/Eldiran/OGA20_GateGuard",
                ["starter-village-main-guide"] = "VillageNpcSprites/Eldiran/OGA09_VillageRepresentative",
            };

        public static bool Apply(
            GameObject npc,
            string stableNpcId,
            string displayName,
            VillageNpcRoleType role)
        {
            ConfigureNameplate(npc, displayName, role);

            if (!ResourcePaths.TryGetValue(stableNpcId, out string resourcePath)) return false;

            Sprite sprite = Resources.Load<Sprite>(resourcePath);
            if (sprite == null)
            {
                Debug.LogError($"시작 마을 NPC 외형을 불러오지 못했습니다: {stableNpcId} -> {resourcePath}");
                return false;
            }

            SpriteRenderer renderer = npc.GetComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.color = Color.white;
            renderer.sortingOrder = NpcSortingOrder;

            // 원본 픽셀은 확대 저장하지 않습니다. PPU로 체격을 정하고 Transform은 1로 유지해 Collider 크기를 보호합니다.
            npc.transform.localScale = Vector3.one;

            PlaceholderVisual placeholder = npc.GetComponent<PlaceholderVisual>();
            if (placeholder != null) Object.Destroy(placeholder);

            // 기존 원형 Trigger는 E 상호작용 감지용으로 두고, 작은 물리 Collider만 별도로 둡니다.
            // Scene의 기존 Collider가 다른 초기화 코드에서 Destroy된 경우 Unity 객체는
            // C# null은 아니지만 Unity null로 판정되므로 ?? 대신 명시적으로 확인합니다.
            BoxCollider2D body = npc.GetComponent<BoxCollider2D>();
            if (body == null) body = npc.AddComponent<BoxCollider2D>();
            body.isTrigger = false;
            body.size = new Vector2(0.5f, 0.34f);
            body.offset = new Vector2(0f, 0.17f);
            return true;
        }

        /// <summary>
        /// 시작 마을 NPC의 기존 TextMesh 이름표를 한 줄·중앙 정렬로 통일하고,
        /// 스프라이트와 겹치지 않는 머리 위 위치와 얇은 그림자를 적용합니다.
        /// </summary>
        private static void ConfigureNameplate(
            GameObject npc,
            string displayName,
            VillageNpcRoleType role)
        {
            Transform labelTransform = npc.transform.Find("Label");
            TextMesh label = labelTransform != null ? labelTransform.GetComponent<TextMesh>() : null;
            if (label == null) return;

            // 활성 Prefab을 복제할 때 PlaceholderVisual.Awake가 만들었던 구형 Label이
            // 함께 복제된 경우에는 한 개만 남겨 역할명과 공통 주민명이 겹치지 않게 합니다.
            for (int index = npc.transform.childCount - 1; index >= 0; index--)
            {
                Transform child = npc.transform.GetChild(index);
                if (child == labelTransform || child.name != "Label") continue;
                child.gameObject.SetActive(false);
                Object.Destroy(child.gameObject);
            }

            // 일반 배경 주민은 상시 이름표를 숨기고, 기능 NPC와 주민 대표는
            // 역할명 또는 향후 확정될 정식 이름 한 줄만 표시합니다.
            label.text = displayName;
            bool showNameplate = role != VillageNpcRoleType.Resident;
            labelTransform.gameObject.SetActive(showNameplate);
            if (!showNameplate) return;

            labelTransform.localPosition = NameplateOffset;
            labelTransform.localScale = Vector3.one;
            label.anchor = TextAnchor.MiddleCenter;
            label.alignment = TextAlignment.Center;
            label.fontSize = NameplateFontSize;
            label.characterSize = displayName.Length >= 7
                ? LongNameplateCharacterSize
                : NameplateCharacterSize;
            label.color = Color.white;
            label.richText = false;
            label.GetComponent<MeshRenderer>().sortingOrder = NameplateSortingOrder;

            Transform shadowTransform = labelTransform.Find("NameShadow");
            if (shadowTransform == null)
            {
                GameObject shadowObject = new GameObject("NameShadow", typeof(TextMesh));
                shadowTransform = shadowObject.transform;
                shadowTransform.SetParent(labelTransform, false);
            }

            shadowTransform.localPosition = NameplateShadowOffset;
            shadowTransform.localScale = Vector3.one;
            TextMesh shadow = shadowTransform.GetComponent<TextMesh>();
            shadow.text = label.text;
            shadow.anchor = label.anchor;
            shadow.alignment = label.alignment;
            shadow.font = label.font;
            shadow.fontSize = label.fontSize;
            shadow.characterSize = label.characterSize;
            shadow.color = new Color(0f, 0f, 0f, 0.9f);
            shadow.richText = false;
            shadow.GetComponent<MeshRenderer>().sortingOrder = NameplateSortingOrder - 1;
        }
    }
}

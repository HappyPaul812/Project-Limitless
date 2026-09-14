using System.Collections.Generic;
using ProjectLimitless.Core;
using UnityEngine;

namespace ProjectLimitless.NPC
{
    /// <summary>시작 마을 stable NPC ID와 승인된 외형 리소스를 연결합니다.</summary>
    public static class VillageNpcAppearanceCatalog
    {
        private const int NpcSortingOrder = 5;

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
                ["starter-village-gate-guard"] = "VillageNpcSprites/Eldiran/OGA20_GateGuard",
                ["starter-village-main-guide"] = "VillageNpcSprites/Eldiran/OGA09_VillageRepresentative",
            };

        public static bool Apply(GameObject npc, string stableNpcId, string displayName)
        {
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

            TextMesh label = npc.GetComponentInChildren<TextMesh>(true);
            if (label != null) label.text = displayName;

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
    }
}

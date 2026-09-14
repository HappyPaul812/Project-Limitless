using ProjectLimitless.NPC;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectLimitless.World
{
    /// <summary>
    /// 사용자가 다듬은 시작 마을 Scene과 원본 Sprite를 다시 생성하지 않고 1장 허브 역할만 런타임에 설치합니다.
    /// 기존 VillageNpc를 원형으로 복제하므로 새 역할도 현재 프로젝트의 NPC 외형과 상호작용 범위를 그대로 따릅니다.
    /// </summary>
    public static class StarterVillageHubInstaller
    {
        private const string SceneName = "World_StarterVillage";
        private const string RootName = "Chapter01_StarterVillageHub";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            SceneManager.sceneLoaded -= Install;
            SceneManager.sceneLoaded += Install;
        }

        private static void Install(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != SceneName || GameObject.Find(RootName) != null) return;

            NpcController template = Object.FindFirstObjectByType<NpcController>();
            if (template == null)
            {
                Debug.LogError("시작 마을 허브의 기준 NPC를 찾지 못했습니다.");
                return;
            }

            GameObject root = new GameObject(RootName);
            SceneManager.MoveGameObjectToScene(root, scene);

            Transform plaza = CreateZone(root.transform, "Zone_CentralPlaza");
            ConfigureNpc(template.gameObject, "Npc_VillageRepresentative", "starter-village-main-guide",
                "주민 대표", "마을에 오신 것을 환영합니다. 준비를 마치면 남문 경비병에게 길을 물어보세요.",
                VillageNpcRoleType.VillageRepresentative, new Vector2(0f, 1.5f), plaza);

            Transform market = CreateZone(root.transform, "Zone_Market");
            CreateNpc(template, market, "Npc_GeneralShop", "starter-village-general-shop", "잡화 상인",
                "여행에 필요한 물품을 준비하고 있습니다. 정식 거래는 다음 단계에서 열립니다.",
                VillageNpcRoleType.GeneralShop, new Vector2(-5.8f, 1.2f));
            CreateNpc(template, market, "Npc_EquipmentShop", "starter-village-equipment-shop", "장비 상인",
                "장비를 손질하고 있습니다. 정식 거래는 다음 단계에서 열립니다.",
                VillageNpcRoleType.EquipmentShop, new Vector2(5.8f, 1.2f));

            Transform rest = CreateZone(root.transform, "Zone_HealerInn");
            CreateNpc(template, rest, "Npc_Healer", "starter-village-healer", "치유사",
                "다친 곳이 있다면 편하게 말씀하세요.", VillageNpcRoleType.Healer, new Vector2(-4.4f, 2f));

            Transform services = CreateZone(root.transform, "Zone_SafeServices");
            CreateNpc(template, services, "Npc_Bank", "starter-village-bank", "은행 안내인",
                "소지품을 안전하게 맡길 수 있는 공간입니다. 보관 업무는 아직 준비 중입니다.",
                VillageNpcRoleType.Bank, new Vector2(4.4f, 2f));
            CreateNpc(template, services, "Npc_PartyManager", "starter-village-party-manager", "파티 안내인",
                "동료와 작전을 정비하는 안전한 공간입니다. 파티 편성은 아직 준비 중입니다.",
                VillageNpcRoleType.PartyManager, new Vector2(-6.4f, -2.1f));

            Transform training = CreateZone(root.transform, "Zone_TrainingPlaceholder");
            CreateNpc(template, training, "Npc_TrainingGuide", "starter-village-training-guide", "훈련장 관리인",
                "기초 훈련을 위한 자리를 정리하고 있습니다.", VillageNpcRoleType.TrainingGuide, new Vector2(6.4f, -2.1f));

            Transform residents = CreateZone(root.transform, "Zone_Residents");
            CreateNpc(template, residents, "Npc_Resident01", "starter-village-resident-01", "주민",
                "광장은 누구나 쉬어 갈 수 있는 곳이에요.", VillageNpcRoleType.Resident, new Vector2(-3.2f, -1.2f));
            CreateNpc(template, residents, "Npc_Resident02", "starter-village-resident-02", "주민",
                "남문 밖은 초원이에요. 준비를 확인하고 나가세요.", VillageNpcRoleType.Resident, new Vector2(3.2f, -2.6f));
            CreateNpc(template, residents, "Npc_Resident03", "starter-village-resident-03", "주민",
                "마을 길은 넓으니 천천히 둘러보셔도 됩니다.", VillageNpcRoleType.Resident, new Vector2(-7.2f, 0f));
            CreateNpc(template, residents, "Npc_Resident04", "starter-village-resident-04", "주민",
                "상점과 치유소가 광장 주변에 모여 있어요.", VillageNpcRoleType.Resident, new Vector2(7.2f, 0f));

            Transform gate = CreateZone(root.transform, "Zone_SouthGate");
            CreateNpc(template, gate, "Npc_GateGuard", "starter-village-gate-guard", "남문 경비병",
                "이 길은 초원으로 이어집니다. 마을로 돌아올 때도 같은 문을 이용하세요.",
                VillageNpcRoleType.GateGuard, new Vector2(2.4f, -5.1f));
        }

        private static Transform CreateZone(Transform parent, string name)
        {
            GameObject zone = new GameObject(name);
            zone.transform.SetParent(parent, false);
            return zone.transform;
        }

        private static void CreateNpc(NpcController template, Transform parent, string objectName, string npcId,
            string displayName, string dialogue, VillageNpcRoleType role, Vector2 position)
        {
            GameObject npc = Object.Instantiate(template.gameObject, position, Quaternion.identity, parent);
            ConfigureNpc(npc, objectName, npcId, displayName, dialogue, role, position, parent);
        }

        private static void ConfigureNpc(GameObject npc, string objectName, string npcId, string displayName,
            string dialogue, VillageNpcRoleType role, Vector2 position, Transform parent)
        {
            npc.name = objectName;
            npc.transform.SetParent(parent, true);
            npc.transform.position = position;
            npc.GetComponent<NpcController>().Configure(displayName, dialogue);
            VillageNpcRole roleData = npc.GetComponent<VillageNpcRole>() ?? npc.AddComponent<VillageNpcRole>();
            roleData.Configure(npcId, role, true);
            VillageNpcAppearanceCatalog.Apply(npc, npcId, displayName);
        }
    }
}

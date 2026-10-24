using ProjectLimitless.NPC;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectLimitless.World
{
    /// <summary>
    /// Main21의 기존 완료 기록으로 펫 서비스의 이용 장소만 확대합니다.
    /// 구매·장착·발견 데이터는 거점마다 만들지 않고 기존 BeastCompanionService를 공유합니다.
    /// Scene 원본을 재생성하지 않으며 이미 있는 같은 역할의 NPC는 그대로 사용합니다.
    /// </summary>
    public sealed class SafeZonePetServiceRollout : MonoBehaviour
    {
        private const string RootName = "Chapter2_PetServiceRollout";

        // 실제 안전지역 설치 코드에 대응하는 목록입니다. Field03 전체가 안전지역인 것은 아닙니다.
        // 향후 정식 거점이 생기면 해당 안전 영역 안에서 검증한 좌표를 이 목록에 등록합니다.
        private static readonly ServiceSite[] Sites =
        {
            new ServiceSite("safezone_frostway_village", "World_FrostwayVillage", new Vector2(-6, 5), new Vector2(6, 5)),
            new ServiceSite("safezone_starter_village", "World_StarterVillage",
                new Vector2(-5.8f, -3.6f), new Vector2(5.8f, -3.6f)),
            new ServiceSite("safezone_arbel", "Arbel", new Vector2(5, -3), new Vector2(7, -3)),
            new ServiceSite(CatacombEntranceSafeZone.SafeZoneId, "Field_03",
                new Vector2(-2.05f, -3.55f), new Vector2(2.85f, -3.55f)),
        };

        private sealed class ServiceSite
        {
            public readonly string Id, Scene;
            public readonly Vector2 Adoption, Management;
            public ServiceSite(string id, string scene, Vector2 adoption, Vector2 management)
            { Id = id; Scene = scene; Adoption = adoption; Management = management; }
        }

        private ServiceSite site;
        private bool? lastCompleted;
        private GameObject adoptionNpc, managementNpc;
        private bool dirty = true;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Register()
        {
            SceneManager.sceneLoaded -= Install;
            SceneManager.sceneLoaded += Install;
        }

        private static void Install(Scene scene, LoadSceneMode mode)
        {
            foreach (ServiceSite candidate in Sites)
            {
                if (scene.name != candidate.Scene) continue;
                foreach (GameObject root in scene.GetRootGameObjects())
                    if (root.GetComponent<SafeZonePetServiceRollout>() != null) return;
                GameObject instance = new GameObject(RootName);
                SceneManager.MoveGameObjectToScene(instance, scene);
                instance.AddComponent<SafeZonePetServiceRollout>().site = candidate;
                return;
            }
        }

        private void OnEnable() => ProjectLimitless.Core.QuestService.Changed += MarkDirty;
        private void OnDisable() => ProjectLimitless.Core.QuestService.Changed -= MarkDirty;
        private void MarkDirty() => dirty = true;

        private void LateUpdate()
        {
            if (site == null) return;
            bool completed = Chapter2Main21Flow.Completed;
            if (!dirty && lastCompleted == completed) return;
            dirty = false;
            lastCompleted = completed;
            // LateUpdate까지 기다려 기존 Arbel의 Start가 설치한 두 NPC를 먼저 찾습니다.
            // Continue나 다른 슬롯 로드로 미완료에 돌아가면 추가한 NPC만 닫습니다.
            if (!completed)
            {
                RemoveGenerated(ref adoptionNpc);
                RemoveGenerated(ref managementNpc);
                return;
            }
            EnsureNpc(ref adoptionNpc, VillageNpcRoleType.PetAdoption, site.Adoption,
                "pet-adoption", "펫 분양 담당");
            EnsureNpc(ref managementNpc, VillageNpcRoleType.PetManagement, site.Management,
                "pet-management", "펫 관리 담당");
        }

        private void EnsureNpc(ref GameObject created, VillageNpcRoleType role, Vector2 position,
            string suffix, string displayName)
        {
            foreach (GameObject root in gameObject.scene.GetRootGameObjects())
                foreach (VillageNpcRole existing in root.GetComponentsInChildren<VillageNpcRole>(false))
                    if (existing.Role == role) return;
            string id = site.Id + "-" + suffix;
            if(site.Scene=="World_FrostwayVillage") displayName += " / 임시 외형";
            created = new GameObject(id, typeof(SpriteRenderer), typeof(NpcController), typeof(VillageNpcRole));
            created.transform.SetParent(transform, false);
            created.transform.position = position;
            created.GetComponent<NpcController>().Configure(displayName,
                "함께할 야수 동료를 살펴보세요.");
            created.GetComponent<VillageNpcRole>().Configure(id, role);
            new GameObject("Label", typeof(TextMesh)).transform.SetParent(created.transform, false);
            VillageNpcAppearanceCatalog.Apply(created, id, displayName, role);
        }

        private static void RemoveGenerated(ref GameObject npc)
        {
            if (npc == null) return;
            npc.SetActive(false);
            Destroy(npc);
            npc = null;
        }
    }
}

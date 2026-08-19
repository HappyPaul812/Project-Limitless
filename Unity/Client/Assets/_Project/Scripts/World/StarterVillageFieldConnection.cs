using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectLimitless.World
{
    /// <summary>사용자 수정 마을 Scene을 덮어쓰지 않고 남쪽 Field 출구와 복귀 Spawn을 설치합니다.</summary>
    public static class StarterVillageFieldConnection
    {
        private const string VillageScene = "World_StarterVillage";
        private const string RootName = "Milestone02_FieldConnection";
        private const string SouthGateResourcePath = "World/StarterVillageSouthGate";
        private static readonly Vector2 VillageSize = new Vector2(19f, 13f);

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            SceneManager.sceneLoaded -= Install;
            SceneManager.sceneLoaded += Install;
        }

        private static void Install(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != VillageScene || GameObject.Find(RootName) != null) return;

            GameObject prefab = Resources.Load<GameObject>(SouthGateResourcePath);
            if (prefab == null)
            {
                Debug.LogError($"마을 남문 Prefab을 찾지 못했습니다: Resources/{SouthGateResourcePath}");
                return;
            }

            GameObject root = Object.Instantiate(prefab);
            root.name = RootName;
            HideFenceOutsideVillage(root.transform, "Fence_-10_-5.7");
            HideFenceOutsideVillage(root.transform, "Fence_10_-5.7");
            CreateVillageBounds(root.transform);
            SceneManager.MoveGameObjectToScene(root, scene);
        }

        private static void HideFenceOutsideVillage(Transform parent, string fenceName)
        {
            Transform fence = parent.Find(fenceName);
            if (fence != null) fence.gameObject.SetActive(false);
        }

        /// <summary>
        /// 마을 바닥(-9..9, -6..6)의 외곽을 막고 남쪽 중앙만 남문 통로로 남깁니다.
        /// 남문 통로는 긴 전환 Trigger 양옆의 가이드와 끝의 안전벽으로 감싸므로
        /// 플레이어가 Trigger를 비껴 검은 영역으로 나갈 수 없습니다.
        /// </summary>
        private static void CreateVillageBounds(Transform parent)
        {
            GameObject boundsObject = new GameObject("WorldBounds", typeof(WorldBounds2D));
            boundsObject.transform.SetParent(parent, false);
            WorldBounds2D worldBounds = boundsObject.GetComponent<WorldBounds2D>();
            worldBounds.Configure(Vector2.zero, VillageSize);
            WorldBounds2D.CreateBoundaryColliders(
                boundsObject.transform,
                worldBounds.Bounds,
                .5f,
                new WorldBoundaryOpening(WorldBoundarySide.Bottom, 0f, 3f));

            CreateBoundary(parent, "SouthGate_GuideLeft", new Vector2(-1.7f, -6.9f), new Vector2(.4f, 1.6f));
            CreateBoundary(parent, "SouthGate_GuideRight", new Vector2(1.7f, -6.9f), new Vector2(.4f, 1.6f));
            CreateBoundary(parent, "SouthGate_SafetyStop", new Vector2(0f, -7.65f), new Vector2(3.4f, .5f));
        }

        private static void CreateBoundary(Transform parent, string name, Vector2 position, Vector2 size)
        {
            GameObject boundary = new GameObject(name, typeof(BoxCollider2D));
            boundary.transform.SetParent(parent, false);
            boundary.transform.localPosition = position;
            boundary.GetComponent<BoxCollider2D>().size = size;
        }
    }
}

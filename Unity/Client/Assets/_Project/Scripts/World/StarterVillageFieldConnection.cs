using ProjectLimitless.CameraSystem;
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
        private static readonly Bounds VillageBounds = new Bounds(Vector3.zero, new Vector3(19f, 13f, 0f));

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
            CreateVillageBoundaries(root.transform);
            SceneManager.MoveGameObjectToScene(root, scene);

            CameraFollow cameraFollow = Object.FindFirstObjectByType<CameraFollow>();
            if (cameraFollow == null)
            {
                Debug.LogError("World_StarterVillage CameraFollow를 찾지 못해 카메라 경계를 설정할 수 없습니다.");
                return;
            }

            cameraFollow.SetMovementBounds(VillageBounds);
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
        private static void CreateVillageBoundaries(Transform parent)
        {
            CreateBoundary(parent, "Boundary_Top", new Vector2(0f, 6.65f), new Vector2(20f, .5f));
            CreateBoundary(parent, "Boundary_Left", new Vector2(-9.65f, 0f), new Vector2(.5f, 13.8f));
            CreateBoundary(parent, "Boundary_Right", new Vector2(9.65f, 0f), new Vector2(.5f, 13.8f));

            CreateBoundary(parent, "Boundary_SouthLeft", new Vector2(-5.5f, -6.35f), new Vector2(8f, .5f));
            CreateBoundary(parent, "Boundary_SouthRight", new Vector2(5.5f, -6.35f), new Vector2(8f, .5f));

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

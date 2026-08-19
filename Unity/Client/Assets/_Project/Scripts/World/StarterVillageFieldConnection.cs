using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectLimitless.World
{
    /// <summary>
    /// World_StarterVillage가 열릴 때 남문, Field 복귀 지점, 월드 경계를 런타임에 설치합니다.
    /// 마을 Scene Asset을 다시 저장하지 않고도 Field_01 연결 구조를 항상 같은 상태로 유지하기 위한 초기화 코드입니다.
    /// </summary>
    public static class StarterVillageFieldConnection
    {
        // 이 연결 구조를 설치해야 하는 대상 Scene 이름입니다.
        private const string VillageScene = "World_StarterVillage";
        // 중복 설치를 막기 위해 런타임에 생성한 최상위 GameObject에 붙이는 고정 이름입니다.
        private const string RootName = "Milestone02_FieldConnection";
        // Resources 폴더에서 불러올 남문 Prefab의 확장자 없는 경로입니다.
        private const string SouthGateResourcePath = "World/StarterVillageSouthGate";
        // 마을의 실제 배경 Tile 전체 크기이며 카메라와 외곽 Collider가 함께 사용합니다.
        private static readonly Vector2 VillageSize = new Vector2(19f, 13f);

        // 게임 실행 시 Scene 로드 이벤트를 한 번 정리해 등록하여 재생이나 재초기화 때 중복 호출되지 않게 합니다.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            SceneManager.sceneLoaded -= Install;
            SceneManager.sceneLoaded += Install;
        }

        /// <summary>
        /// Scene 로드가 끝날 때 호출되며, StarterVillage인 경우에만 남문 Prefab과 공통 경계를 설치합니다.
        /// 이미 설치된 루트가 있으면 아무것도 만들지 않아 Trigger와 Collider가 중복되지 않습니다.
        /// </summary>
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

        /// <summary>이전 Prefab에 남아 있을 수 있는 Tile 영역 밖 끝 울타리를 화면에서 숨깁니다.</summary>
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

        /// <summary>남문 통로를 보조하는 개별 사각 Collider GameObject를 런타임에 만듭니다.</summary>
        private static void CreateBoundary(Transform parent, string name, Vector2 position, Vector2 size)
        {
            GameObject boundary = new GameObject(name, typeof(BoxCollider2D));
            boundary.transform.SetParent(parent, false);
            boundary.transform.localPosition = position;
            boundary.GetComponent<BoxCollider2D>().size = size;
        }
    }
}

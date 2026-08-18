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
            SceneManager.MoveGameObjectToScene(root, scene);
        }
    }
}

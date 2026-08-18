using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectLimitless.World
{
    /// <summary>사용자 수정 마을 Scene을 덮어쓰지 않고 남쪽 Field 출구와 복귀 Spawn을 설치합니다.</summary>
    public static class StarterVillageFieldConnection
    {
        private const string VillageScene = "World_StarterVillage";
        private const string FieldScene = "Field_01";
        private const string RootName = "Milestone02_FieldConnection";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            SceneManager.sceneLoaded -= Install;
            SceneManager.sceneLoaded += Install;
        }

        private static void Install(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != VillageScene || GameObject.Find(RootName) != null) return;

            GameObject root = new GameObject(RootName);
            SceneManager.MoveGameObjectToScene(root, scene);

            GameObject spawn = new GameObject("Spawn_From_Field01");
            spawn.transform.SetParent(root.transform);
            spawn.transform.position = new Vector3(0f, -4.75f, 0f);
            spawn.AddComponent<SceneSpawnPoint>().Configure("Spawn_From_Field01");

            GameObject exit = new GameObject("Exit_To_Field01", typeof(BoxCollider2D));
            exit.transform.SetParent(root.transform);
            exit.transform.position = new Vector3(0f, -6.65f, 0f);
            BoxCollider2D collider = exit.GetComponent<BoxCollider2D>();
            collider.size = new Vector2(2.5f, 0.9f);
            collider.isTrigger = true;
            exit.AddComponent<SceneTransitionTrigger>().Configure(FieldScene, "Spawn_From_StarterVillage");

            GameObject marker = new GameObject("FieldExitMarker", typeof(TextMesh));
            marker.transform.SetParent(root.transform);
            marker.transform.position = new Vector3(0f, -5.75f, 0f);
            TextMesh label = marker.GetComponent<TextMesh>();
            label.text = "FIELD";
            label.anchor = TextAnchor.MiddleCenter;
            label.alignment = TextAlignment.Center;
            label.characterSize = 0.08f;
            label.fontSize = 48;
            label.color = new Color(0.32f, 0.2f, 0.08f, 1f);
            marker.GetComponent<MeshRenderer>().sortingOrder = 3;
        }
    }
}

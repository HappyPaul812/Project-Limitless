using System.Linq;
using ProjectLimitless.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectLimitless.World
{
    /// <summary>Main16 이후 협곡 연결을 기존 Scene/Quest 서비스에 연결합니다. 기존 Scene 원본은 보존합니다.</summary>
    public sealed class Chapter2Main17Flow : MonoBehaviour
    {
        public const string QuestId = "main_17_red_rift";
        public const string Field = "Field_08_RedRift";
        public const string PreviousField = "Field_07_AshenReach";
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Register() { SceneManager.sceneLoaded -= Install; SceneManager.sceneLoaded += Install; }
        private static void Install(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != Field && scene.name != PreviousField) return;
            if (scene.GetRootGameObjects().Any(value => value.GetComponent<Chapter2Main17Flow>() != null)) return;
            var root = new GameObject("Chapter2Main17Flow", typeof(Chapter2Main17Flow));
            SceneManager.MoveGameObjectToScene(root, scene);
        }
        private void Awake()
        {
            if (gameObject.scene.name != PreviousField) return;
            var bounds = gameObject.scene.GetRootGameObjects().Select(value => value.GetComponent<WorldBounds2D>()).FirstOrDefault(value => value != null);
            if (bounds == null) { Debug.LogError("Main17: Field07 WorldBounds가 없습니다."); return; }
            // 같은 Bounds로 경계를 재구성하여 기존 동쪽과 새 서쪽 출구 외에는 통과할 수 없게 합니다.
            foreach (Transform child in bounds.transform.Cast<Transform>().Where(value => value.name.StartsWith("Boundary_")).ToArray()) Destroy(child.gameObject);
            WorldBounds2D.CreateBoundaryColliders(transform, bounds.Bounds, .3f,
                new WorldBoundaryOpening(WorldBoundarySide.Right, 0, 3), new WorldBoundaryOpening(WorldBoundarySide.Left, 0, 3));
            var gate = new GameObject("Main17AccessBarrier", typeof(BoxCollider2D), typeof(Main17AccessGate));
            gate.transform.SetParent(transform, false); gate.transform.position = new Vector2(-9.45f, 0);
            gate.GetComponent<BoxCollider2D>().size = new Vector2(.35f, 3);
        }
    }

    /// <summary>Main16 보고 완료까지 서쪽 출구를 막습니다. 저장된 완료 기록에서 매번 잠금을 복원합니다.</summary>
    public sealed class Main17AccessGate : MonoBehaviour
    {
        private void Update() => GetComponent<BoxCollider2D>().enabled = QuestService.GetState(Chapter2Main16Flow.QuestId) != QuestState.Completed;
    }
}

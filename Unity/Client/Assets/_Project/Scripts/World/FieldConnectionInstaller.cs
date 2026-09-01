using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectLimitless.World
{
    /// <summary>Resources의 연결 데이터를 읽어 Scene마다 필요한 출입구를 설치합니다.</summary>
    public static class FieldConnectionInstaller
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            SceneManager.sceneLoaded -= Install;
            SceneManager.sceneLoaded += Install;
        }

        private static void Install(Scene scene, LoadSceneMode mode)
        {
            FieldConnectionDefinition[] definitions = Resources.LoadAll<FieldConnectionDefinition>("FieldConnections")
                .Where(item => item.SceneName == scene.name).OrderBy(item => item.ConnectionId).ToArray();
            if (definitions.Any(item => item.ReplaceExistingSceneConnections))
            {
                foreach (SceneTransitionTrigger trigger in Object.FindObjectsByType<SceneTransitionTrigger>())
                    if (trigger.gameObject.scene == scene) Object.Destroy(trigger.gameObject);
                foreach (SceneSpawnPoint spawnPoint in Object.FindObjectsByType<SceneSpawnPoint>())
                    if (spawnPoint.gameObject.scene == scene) Object.Destroy(spawnPoint.gameObject);
            }
            foreach (FieldConnectionDefinition definition in definitions)
            {
                // 출발 Trigger는 다음 Scene의 SpawnPoint ID만 전달합니다. 목적지 Scene이 그 ID를 찾아
                // 플레이어를 Trigger와 떨어진 위치에 놓으므로 Field 간 왕복 직후 재전환되지 않습니다.
                GameObject spawn = new GameObject($"Spawn_{definition.ConnectionId}", typeof(SceneSpawnPoint));
                spawn.transform.position = definition.SpawnPosition;
                spawn.GetComponent<SceneSpawnPoint>().Configure(definition.SpawnPointId);
                SceneManager.MoveGameObjectToScene(spawn, scene);

                GameObject transition = new GameObject($"Transition_{definition.ConnectionId}",
                    typeof(BoxCollider2D), typeof(SceneTransitionTrigger));
                transition.transform.position = definition.TransitionPosition;
                BoxCollider2D collider = transition.GetComponent<BoxCollider2D>();
                collider.isTrigger = true;
                collider.size = definition.TransitionSize;
                transition.GetComponent<SceneTransitionTrigger>().Configure(
                    definition.TargetSceneName, definition.TargetSpawnPointId);
                SceneManager.MoveGameObjectToScene(transition, scene);
            }
        }
    }
}

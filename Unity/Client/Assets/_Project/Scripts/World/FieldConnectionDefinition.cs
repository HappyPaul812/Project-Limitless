using UnityEngine;

namespace ProjectLimitless.World
{
    /// <summary>Field 사이 출입구 한 쌍 중 현재 Scene 쪽 SpawnPoint와 Transition을 보관합니다.</summary>
    [CreateAssetMenu(fileName = "FieldConnection", menuName = "Project Limitless/Field Connection")]
    public sealed class FieldConnectionDefinition : ScriptableObject
    {
        [SerializeField] private string sceneName;
        [SerializeField] private string connectionId;
        [SerializeField] private string spawnPointId;
        [SerializeField] private Vector2 spawnPosition;
        [SerializeField] private Vector2 transitionPosition;
        [SerializeField] private Vector2 transitionSize = new Vector2(3f, 1f);
        [SerializeField] private string targetSceneName;
        [SerializeField] private string targetSpawnPointId;
        [SerializeField] private bool replaceExistingSceneConnections;

        public string SceneName => sceneName;
        public string ConnectionId => connectionId;
        public string SpawnPointId => spawnPointId;
        public Vector2 SpawnPosition => spawnPosition;
        public Vector2 TransitionPosition => transitionPosition;
        public Vector2 TransitionSize => transitionSize;
        public string TargetSceneName => targetSceneName;
        public string TargetSpawnPointId => targetSpawnPointId;
        public bool ReplaceExistingSceneConnections => replaceExistingSceneConnections;
    }
}

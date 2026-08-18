using ProjectLimitless.Player;
using UnityEngine;

namespace ProjectLimitless.World
{
    /// <summary>플레이어가 넓은 Trigger 영역에 들어오면 지정 Scene과 Spawn Point로 이동합니다.</summary>
    [RequireComponent(typeof(Collider2D))]
    public sealed class SceneTransitionTrigger : MonoBehaviour
    {
        [SerializeField] private string targetScene;
        [SerializeField] private string targetSpawnPointId;

        public void Configure(string sceneName, string spawnPointId)
        {
            targetScene = sceneName;
            targetSpawnPointId = spawnPointId;
            Collider2D trigger = GetComponent<Collider2D>();
            if (trigger != null) trigger.isTrigger = true;
        }

        private void Reset() => GetComponent<Collider2D>().isTrigger = true;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<PlayerController>() == null) return;
            SceneTransitionService.Load(targetScene, targetSpawnPointId);
        }
    }
}

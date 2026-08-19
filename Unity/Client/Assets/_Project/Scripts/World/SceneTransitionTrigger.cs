using ProjectLimitless.Player;
using UnityEngine;

namespace ProjectLimitless.World
{
    /// <summary>
    /// 월드 출입구의 Trigger Collider에 붙어 플레이어 진입을 감지합니다.
    /// 플레이어가 들어오면 SceneTransitionService에 목적지 Scene과 Spawn Point를 전달합니다.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public sealed class SceneTransitionTrigger : MonoBehaviour
    {
        // 이동할 Scene의 Build Settings 이름입니다.
        [SerializeField] private string targetScene;
        // 다음 Scene에서 플레이어를 배치할 SceneSpawnPoint의 고유 ID입니다.
        [SerializeField] private string targetSpawnPointId;

        /// <summary>Scene Generator가 목적지 정보와 Trigger Collider 설정을 함께 지정할 때 호출합니다.</summary>
        public void Configure(string sceneName, string spawnPointId)
        {
            targetScene = sceneName;
            targetSpawnPointId = spawnPointId;
            Collider2D trigger = GetComponent<Collider2D>();
            if (trigger != null) trigger.isTrigger = true;
        }

        // Inspector에서 Component를 처음 추가하거나 Reset할 때 물리 벽이 아닌 감지 영역으로 맞춥니다.
        private void Reset() => GetComponent<Collider2D>().isTrigger = true;

        /// <summary>Collider가 들어온 순간 호출되며, PlayerController인 경우에만 Scene 전환을 요청합니다.</summary>
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<PlayerController>() == null) return;
            SceneTransitionService.Load(targetScene, targetSpawnPointId);
        }
    }
}

using ProjectLimitless.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectLimitless.World
{
    /// <summary>
    /// 출구 Trigger에서 받은 Scene 전환 요청을 실행하고 다음 Scene의 Spawn Point ID를 전달합니다.
    /// 모든 월드 이동이 이 서비스를 거치므로 중복 로드와 Spawn 정보 전달을 한 곳에서 관리합니다.
    /// </summary>
    public static class SceneTransitionService
    {
        // 비동기 Scene 로드가 진행 중인지 나타내며, Trigger가 여러 번 겹쳐도 전환 요청이 중복되지 않게 합니다.
        private static bool isLoading;

        // 게임 시작 전에 로드 상태를 초기화하고, 새 Scene이 준비되면 다시 전환할 수 있도록 이벤트를 연결합니다.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            isLoading = false;
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        /// <summary>
        /// SceneTransitionTrigger가 플레이어 진입을 감지했을 때 호출합니다.
        /// 목적지 Spawn ID를 세션에 먼저 저장한 뒤 대상 Scene을 비동기로 불러옵니다.
        /// </summary>
        public static void Load(string targetScene, string targetSpawnPointId)
        {
            if (isLoading || string.IsNullOrWhiteSpace(targetScene)) return;
            isLoading = true;
            GameSessionData.SetPendingSpawnPoint(targetSpawnPointId);
            SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Single);
        }

        // 새 Scene 로드가 끝나면 다음 출구 Trigger를 받을 수 있도록 중복 방지 상태를 해제합니다.
        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode) => isLoading = false;
    }
}

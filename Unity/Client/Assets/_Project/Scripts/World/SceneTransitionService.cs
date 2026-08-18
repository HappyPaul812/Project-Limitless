using ProjectLimitless.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectLimitless.World
{
    /// <summary>Scene 전환 요청과 다음 Spawn Point 전달을 한 곳에서 처리합니다.</summary>
    public static class SceneTransitionService
    {
        private static bool isLoading;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            isLoading = false;
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public static void Load(string targetScene, string targetSpawnPointId)
        {
            if (isLoading || string.IsNullOrWhiteSpace(targetScene)) return;
            isLoading = true;
            GameSessionData.SetPendingSpawnPoint(targetSpawnPointId);
            SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Single);
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode) => isLoading = false;
    }
}

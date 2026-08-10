using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectLimitless.Core
{
    /// <summary>게임 시작 Scene에서 첫 번째 월드를 로드한다.</summary>
    public sealed class BootstrapLoader : MonoBehaviour
    {
        [SerializeField] private string worldSceneName = "World_StarterVillage";

        private void Start()
        {
            SceneManager.LoadSceneAsync(worldSceneName, LoadSceneMode.Single);
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectLimitless.Core
{
    /// <summary>
    /// 게임 시작용 Bootstrap GameObject에 붙어 첫 번째 월드 Scene을 불러옵니다.
    /// 시작 절차와 실제 월드를 분리하여, 게임이 항상 같은 진입점에서 시작되게 합니다.
    /// </summary>
    public sealed class BootstrapLoader : MonoBehaviour
    {
        // 시작 직후 불러올 Scene 이름입니다. private이지만 SerializeField 덕분에 Inspector에서 바꿀 수 있습니다.
        [SerializeField] private string worldSceneName = "World_StarterVillage";

        /// <summary>GameObject가 처음 활성화된 뒤 월드 Scene을 비동기로 불러옵니다.</summary>
        private void Start()
        {
            // Single 모드는 현재 Bootstrap Scene을 내리고 지정한 월드 하나만 남깁니다.
            SceneManager.LoadSceneAsync(worldSceneName, LoadSceneMode.Single);
        }
    }
}

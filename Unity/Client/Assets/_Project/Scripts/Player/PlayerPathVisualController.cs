using ProjectLimitless.Core;
using UnityEngine;

namespace ProjectLimitless.Player
{
    [RequireComponent(typeof(PlayerVisualController))]
    public sealed class PlayerPathVisualController : MonoBehaviour
    {
        private PlayerVisualController baseVisual;
        private string currentPathId;
        // 외형 Definition의 심볼 슬롯은 이전 구조와의 호환을 위해 남아 있지만, 공식 문장은 모든 길이
        // 공통으로 사용하는 PlayerPathDefinition에서 찾습니다. 이로써 캐릭터 외형과 Path 정체성이 분리됩니다.
        public Sprite SymbolSprite => PathPresentationResolver.Find(currentPathId)?.PathSymbol;

        private void Awake()
        {
            baseVisual = GetComponent<PlayerVisualController>();
            Apply(GameSessionData.SelectedPlayerPathId);
        }

        public void Apply(string pathId)
        {
            if (baseVisual == null) baseVisual = GetComponent<PlayerVisualController>();
            baseVisual.ResetPathVariant();
            currentPathId = pathId;
            // Ready 조합만 적용합니다. Blocked·미선택은 기본 성별의 임시 fallback을 유지합니다.
            // 다른 길의 Variant로 대체하면 50조합의 미술 문제를 숨기므로 그 경로는 사용하지 않습니다.
            baseVisual.ApplySelectedAppearance();
        }
    }
}

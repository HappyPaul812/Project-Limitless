using ProjectLimitless.Core;
using UnityEngine;

namespace ProjectLimitless.Player
{
    [RequireComponent(typeof(PlayerVisualController))]
    public sealed class PlayerPathVisualController : MonoBehaviour
    {
        private PlayerVisualController baseVisual;
        private PlayerPathVisualDefinition definition;
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
            baseVisual.ResetPathVariant();
            currentPathId = pathId;
            definition = PathVisualCatalog.Find(pathId);
            if (definition != null && (definition.VisualMode == PathVisualMode.CharacterVariant || definition.VisualMode == PathVisualMode.WheelchairVariant))
            {
                CharacterVariantSet variant = definition.GetVariant(baseVisual.VisualType);
                if (variant.IsReady) baseVisual.ApplyPathVariant(variant.AnimatorController, variant.DefaultDownSprite);
            }
            if (definition == null && !string.IsNullOrWhiteSpace(pathId)) Debug.LogWarning($"Path Visual: '{pathId}' 정의를 찾지 못해 기본 외형을 유지합니다.", this);
        }
    }
}

using ProjectLimitless.Core;
using UnityEngine;

namespace ProjectLimitless.Player
{
    [RequireComponent(typeof(PlayerVisualController))]
    public sealed class PlayerPathVisualController : MonoBehaviour
    {
        private PlayerVisualController baseVisual;
        private PlayerPathVisualDefinition definition;
        public Sprite SymbolSprite => definition?.SymbolSprite;

        private void Awake()
        {
            baseVisual = GetComponent<PlayerVisualController>();
            Apply(GameSessionData.SelectedPlayerPathId);
        }

        public void Apply(string pathId)
        {
            baseVisual.ResetPathVariant();
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

using ProjectLimitless.Core;
using UnityEngine;

namespace ProjectLimitless.Player
{
    [RequireComponent(typeof(PlayerVisualController))]
    public sealed class PlayerPathVisualController : MonoBehaviour
    {
        private PlayerVisualController baseVisual;
        private PlayerSpriteAnimator spriteAnimator;
        private SpriteRenderer baseRenderer;
        private SpriteRenderer overlayRenderer;
        private PlayerPathVisualDefinition definition;
        private FacingDirection lastDirection = (FacingDirection)(-1);
        public Sprite SymbolSprite => definition?.SymbolSprite;

        private void Awake()
        {
            baseVisual = GetComponent<PlayerVisualController>();
            spriteAnimator = GetComponentInChildren<PlayerSpriteAnimator>(true);
            baseRenderer = spriteAnimator != null ? spriteAnimator.GetComponent<SpriteRenderer>() : GetComponentInChildren<SpriteRenderer>(true);
            EnsureOverlay();
            Apply(GameSessionData.SelectedPlayerPathId);
        }

        private void LateUpdate()
        {
            FacingDirection direction = spriteAnimator != null ? spriteAnimator.CurrentFacingDirection : FacingDirection.Down;
            if (direction == lastDirection) return;
            lastDirection = direction;
            RefreshSprite();
        }

        public void Apply(string pathId)
        {
            definition = PathVisualCatalog.Find(pathId);
            lastDirection = (FacingDirection)(-1);
            RefreshSprite();
            if (definition == null && !string.IsNullOrWhiteSpace(pathId)) Debug.LogWarning($"Path Visual: '{pathId}' 정의를 찾지 못해 기본 외형을 유지합니다.", this);
        }

        private void EnsureOverlay()
        {
            if (baseRenderer == null) return;
            Transform existing = baseRenderer.transform.Find("PathVisual");
            GameObject overlay = existing != null ? existing.gameObject : new GameObject("PathVisual");
            overlay.transform.SetParent(baseRenderer.transform, false);
            overlayRenderer = overlay.GetComponent<SpriteRenderer>() ?? overlay.AddComponent<SpriteRenderer>();
            overlayRenderer.sortingLayerID = baseRenderer.sortingLayerID;
            overlayRenderer.sortingOrder = baseRenderer.sortingOrder + 1;
        }

        private void RefreshSprite()
        {
            if (overlayRenderer == null) return;
            if (definition == null) { overlayRenderer.sprite = null; if (baseRenderer != null) baseRenderer.enabled = true; return; }
            DirectionalSpriteSet sprites = definition.GetSprites(baseVisual.VisualType);
            overlayRenderer.sprite = sprites.Get(spriteAnimator != null ? spriteAnimator.CurrentFacingDirection : FacingDirection.Down);
            bool wheelchairReady = definition.VisualMode == PathVisualMode.Wheelchair && sprites.HasAny;
            if (baseRenderer != null) baseRenderer.enabled = !wheelchairReady;
        }
    }
}

using ProjectLimitless.Core;
using ProjectLimitless.Player;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectLimitless.UI
{
    public static class PathVisualPreview
    {
        public static void Apply(Image character, Image overlay, Image symbol, string pathId, PlayerVisualType visualType)
        {
            PlayerPathVisualDefinition definition = PathVisualCatalog.Find(pathId);
            Sprite visual = definition?.GetSprites(visualType).Get(FacingDirection.Down);
            if (overlay != null) { overlay.sprite = visual; overlay.color = visual != null ? Color.white : Color.clear; overlay.preserveAspect = true; }
            if (symbol != null) { symbol.sprite = definition?.SymbolSprite; symbol.color = symbol.sprite != null ? Color.white : Color.clear; symbol.preserveAspect = true; }
            if (character != null) character.color = definition != null && definition.VisualMode == PathVisualMode.Wheelchair && visual != null ? Color.clear : Color.white;
        }

        public static string GetDisplayName(string pathId) => PathVisualCatalog.Find(pathId)?.VisualDisplayName ?? "외형 Asset 준비 중";
    }
}

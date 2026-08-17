using ProjectLimitless.Core;
using ProjectLimitless.Player;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectLimitless.UI
{
    public static class PathVisualPreview
    {
        public static void Apply(Image character, Image overlay, Image symbol, Sprite baseSprite, string pathId, PlayerVisualType visualType)
        {
            PlayerPathVisualDefinition definition = PathVisualCatalog.Find(pathId);
            CharacterVariantSet variant = definition != null ? definition.GetVariant(visualType) : default;
            if (character != null) character.sprite = baseSprite;
            if (character != null && definition != null && (definition.VisualMode == PathVisualMode.CharacterVariant || definition.VisualMode == PathVisualMode.WheelchairVariant) && variant.DefaultDownSprite != null)
                character.sprite = variant.DefaultDownSprite;
            if (overlay != null) { overlay.sprite = null; overlay.color = Color.clear; }
            if (symbol != null) { symbol.sprite = definition?.SymbolSprite; symbol.color = symbol.sprite != null ? Color.white : Color.clear; symbol.preserveAspect = true; }
            if (character != null) character.color = Color.white;
        }

        public static string GetDisplayName(string pathId) => PathVisualCatalog.Find(pathId)?.VisualDisplayName ?? "외형 Asset 준비 중";
    }
}

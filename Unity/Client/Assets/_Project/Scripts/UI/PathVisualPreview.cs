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
            // 캐릭터 Variant는 외형이고 PathSymbol은 길의 공식 문장입니다. 두 자료를 별도 Definition에서
            // 읽어 외형만으로 길을 판단하게 만들지 않으며, 항상 길 이름과 함께 표시할 수 있게 합니다.
            PlayerPathDefinition presentation = PathPresentationResolver.Find(pathId);
            if (symbol != null) { symbol.sprite = presentation?.PathSymbol; symbol.color = symbol.sprite != null ? Color.white : Color.clear; symbol.preserveAspect = true; }
            if (character != null) character.color = Color.white;
        }

        public static string GetDisplayName(string pathId) => PathVisualCatalog.Find(pathId)?.VisualDisplayName ?? "외형 Asset 준비 중";
    }
}

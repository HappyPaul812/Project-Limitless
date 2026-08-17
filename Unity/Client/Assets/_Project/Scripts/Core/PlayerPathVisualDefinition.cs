using System;
using UnityEngine;

namespace ProjectLimitless.Core
{
    public enum PathVisualMode { None, CharacterVariant, WheelchairVariant, BodyEmblem }

    [Serializable]
    public struct CharacterVariantSet
    {
        [SerializeField] private RuntimeAnimatorController animatorController;
        [SerializeField] private Sprite defaultDownSprite;
        public RuntimeAnimatorController AnimatorController => animatorController;
        public Sprite DefaultDownSprite => defaultDownSprite;
        public bool IsReady => animatorController != null && defaultDownSprite != null;
    }

    [CreateAssetMenu(fileName = "PathVisualDefinition", menuName = "Project Limitless/Path Visual Definition")]
    public sealed class PlayerPathVisualDefinition : ScriptableObject
    {
        [SerializeField] private string pathId;
        [SerializeField] private PathVisualMode visualMode;
        [SerializeField] private string visualDisplayName;
        [SerializeField] private CharacterVariantSet maleVariant;
        [SerializeField] private CharacterVariantSet femaleVariant;
        [SerializeField] private Sprite symbolSprite;
        public string PathId => pathId;
        public PathVisualMode VisualMode => visualMode;
        public string VisualDisplayName => visualDisplayName;
        public Sprite SymbolSprite => symbolSprite;
        public CharacterVariantSet GetVariant(ProjectLimitless.Player.PlayerVisualType visualType) => visualType == ProjectLimitless.Player.PlayerVisualType.Female ? femaleVariant : maleVariant;
    }
}

using System;
using UnityEngine;

namespace ProjectLimitless.Core
{
    public enum PathVisualMode { None, HeadAccessory, EyeAccessory, BodyEmblem, Wheelchair }
    public enum FacingDirection { Down, Left, Right, Up }

    [Serializable]
    public struct DirectionalSpriteSet
    {
        [SerializeField] private Sprite down;
        [SerializeField] private Sprite left;
        [SerializeField] private Sprite right;
        [SerializeField] private Sprite up;

        public Sprite Get(FacingDirection direction)
        {
            switch (direction)
            {
                case FacingDirection.Left: return left;
                case FacingDirection.Right: return right;
                case FacingDirection.Up: return up;
                default: return down;
            }
        }

        public bool HasAny => down != null || left != null || right != null || up != null;
    }

    [CreateAssetMenu(fileName = "PathVisualDefinition", menuName = "Project Limitless/Path Visual Definition")]
    public sealed class PlayerPathVisualDefinition : ScriptableObject
    {
        [SerializeField] private string pathId;
        [SerializeField] private PathVisualMode visualMode;
        [SerializeField] private string visualDisplayName;
        [SerializeField] private DirectionalSpriteSet maleSprites;
        [SerializeField] private DirectionalSpriteSet femaleSprites;
        [SerializeField] private Sprite symbolSprite;
        public string PathId => pathId;
        public PathVisualMode VisualMode => visualMode;
        public string VisualDisplayName => visualDisplayName;
        public Sprite SymbolSprite => symbolSprite;
        public DirectionalSpriteSet GetSprites(ProjectLimitless.Player.PlayerVisualType visualType) => visualType == ProjectLimitless.Player.PlayerVisualType.Female ? femaleSprites : maleSprites;
    }
}

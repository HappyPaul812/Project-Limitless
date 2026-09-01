using UnityEngine;

namespace ProjectLimitless.World
{
    /// <summary>기존 무료 환경 Sprite를 재사용해 Field마다 색감과 숲 밀도를 바꾸는 데이터입니다.</summary>
    [CreateAssetMenu(fileName = "FieldTheme", menuName = "Project Limitless/Field Theme")]
    public sealed class FieldThemeDefinition : ScriptableObject
    {
        [SerializeField] private string sceneName;
        [SerializeField] private Color groundTint = Color.white;
        [SerializeField] private Color cameraColor = new Color(.08f, .12f, .09f, 1f);
        [SerializeField] private string treeTemplateName;
        [SerializeField] private string bushTemplateName;
        [SerializeField] private Vector2[] extraTreePositions;
        [SerializeField] private Vector2[] extraBushPositions;

        public string SceneName => sceneName;
        public Color GroundTint => groundTint;
        public Color CameraColor => cameraColor;
        public string TreeTemplateName => treeTemplateName;
        public string BushTemplateName => bushTemplateName;
        public Vector2[] ExtraTreePositions => extraTreePositions ?? new Vector2[0];
        public Vector2[] ExtraBushPositions => extraBushPositions ?? new Vector2[0];
    }
}

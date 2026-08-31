using UnityEngine;

namespace ProjectLimitless.Monster
{
    /// <summary>
    /// 몬스터 한 종류가 필드에서 어떻게 보이고 움직이는지를 보관하는 데이터 Asset입니다.
    /// 향후 전투 능력치는 별도 전투 데이터로 연결하며, 이 클래스에는 아직 HP나 공격 수치를 넣지 않습니다.
    /// </summary>
    [CreateAssetMenu(fileName = "MonsterDefinition", menuName = "Project Limitless/Monster Definition")]
    public sealed class MonsterDefinition : ScriptableObject
    {
        // 표시 이름이 바뀌어도 저장·전투 시스템에서 같은 몬스터를 찾기 위한 고유 ID입니다.
        [SerializeField] private string monsterId;
        // 필드 이름표와 향후 전투 UI에 보여 줄 이름입니다.
        [SerializeField] private string displayName;
        // 필드에서 1초 동안 이동하는 거리입니다.
        [SerializeField, Min(0.1f)] private float fieldMoveSpeed = 0.8f;
        // 목적지에 도착한 뒤 다음 이동을 시작하기 전 최소·최대 대기 시간입니다.
        [SerializeField, Min(0f)] private float minimumIdleTime = 0.8f;
        [SerializeField, Min(0f)] private float maximumIdleTime = 2f;
        // 필드에서 처음 표시할 기본 Sprite와 방향별 동작을 재생할 Animator Controller입니다.
        // 둘 다 연결된 몬스터는 Placeholder 대신 실제 Visual과 Animation을 사용합니다.
        [SerializeField] private Sprite fieldSprite;
        [SerializeField] private RuntimeAnimatorController fieldAnimatorController;
        [SerializeField] private Color placeholderColor = new Color(.35f, .8f, .35f, 1f);
        [SerializeField] private Vector2 visualSize = new Vector2(.8f, .65f);
        [Header("공용 SpriteSheet Animation")]
        // 같은 시트 정보를 Field와 Battle이 함께 읽습니다. 한 곳에서 프레임 구조를 고치면 두 화면이
        // 함께 바뀌므로 Scene마다 경로나 프레임 수를 다시 적다가 서로 달라지는 문제를 막습니다.
        [SerializeField] private Texture2D idleSpriteSheet;
        [SerializeField] private Vector2Int idleFrameSize;
        [SerializeField, Min(1)] private int idleColumns = 1;
        [SerializeField, Min(1)] private int idleFrameCount = 1;
        [SerializeField] private Texture2D attackSpriteSheet;
        [SerializeField] private Vector2Int attackFrameSize;
        [SerializeField, Min(1)] private int attackColumns = 1;
        [SerializeField, Min(1)] private int attackFrameCount = 1;
        [SerializeField, Min(1f)] private float animationFramesPerSecond = 10f;
        [SerializeField, Min(.1f)] private float visualScale = 1f;
        [SerializeField] private bool sourceFacesRight = true;

        public string MonsterId => monsterId;
        public string DisplayName => displayName;
        public float FieldMoveSpeed => fieldMoveSpeed;
        public float MinimumIdleTime => minimumIdleTime;
        public float MaximumIdleTime => Mathf.Max(minimumIdleTime, maximumIdleTime);
        public Sprite FieldSprite => fieldSprite;
        public RuntimeAnimatorController FieldAnimatorController => fieldAnimatorController;
        public Color PlaceholderColor => placeholderColor;
        public Vector2 VisualSize => visualSize;
        public Texture2D IdleSpriteSheet => idleSpriteSheet;
        public Vector2Int IdleFrameSize => idleFrameSize;
        public int IdleColumns => Mathf.Max(1, idleColumns);
        public int IdleFrameCount => Mathf.Max(1, idleFrameCount);
        public Texture2D AttackSpriteSheet => attackSpriteSheet;
        public Vector2Int AttackFrameSize => attackFrameSize;
        public int AttackColumns => Mathf.Max(1, attackColumns);
        public int AttackFrameCount => Mathf.Max(1, attackFrameCount);
        public float AnimationFramesPerSecond => Mathf.Max(1f, animationFramesPerSecond);
        public float VisualScale => Mathf.Max(.1f, visualScale);
        public bool SourceFacesRight => sourceFacesRight;
        public bool UsesSpriteSheetAnimation => idleSpriteSheet != null && idleFrameSize.x > 0 && idleFrameSize.y > 0;
    }
}

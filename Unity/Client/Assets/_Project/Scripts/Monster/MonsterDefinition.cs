using UnityEngine;

namespace ProjectLimitless.Monster
{
    /// <summary>
    /// 몬스터 한 종류가 필드와 전투에서 공유하는 외형·이동·기본 공격 차이를 보관하는 데이터 Asset입니다.
    /// HP 같은 Encounter별 수치는 전투 구성에 남기고, 독침벌처럼 종류 자체의 특징인 공격 배율과 독 부여
    /// 규칙만 이곳에 두어 Field 개체와 Battle 참가자가 같은 몬스터 정의를 재사용하게 합니다.
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
        [Header("개별 프레임 Animation (Resources 경로)")]
        [SerializeField] private string idleFrameResourcePath;
        [SerializeField] private string walkFrameResourcePath;
        [SerializeField] private string attackFrameResourcePath;
        [SerializeField] private string shootFrameResourcePath;
        [Header("전투 기본 공격")]
        // 슬라임의 기준 공격력을 100으로 보며 몬스터 종류별 차이만 데이터에 둡니다.
        // 독침벌은 120으로 설정해 전투 생성 코드에 이름별 피해 숫자를 흩어 놓지 않습니다.
        [SerializeField, Min(1)] private int battleAttackPercent = 100;
        // 0이면 독을 걸지 않고, 양수이면 정상 기본 공격 적중 시 그 횟수로 독을 부여·갱신합니다.
        [SerializeField, Min(0)] private int basicAttackPoisonActions;
        // 독을 정상 부여한 뒤 이 몬스터 자신의 행동 몇 회 동안 다시 부여하지 못하는지 나타냅니다.
        // 독 대상의 지속시간과 별도 데이터라서 정화나 독 자연 종료가 이 값을 바꾸지 않습니다.
        [SerializeField, Min(0)] private int basicAttackPoisonCooldownActions;
        [Header("광역 직접 공격")]
        [SerializeField] private string directAreaAttackName;
        [SerializeField, Min(0)] private int directAreaAttackDamage;

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
        public string IdleFrameResourcePath => idleFrameResourcePath;
        public string WalkFrameResourcePath => walkFrameResourcePath;
        public string AttackFrameResourcePath => attackFrameResourcePath;
        public string ShootFrameResourcePath => shootFrameResourcePath;
        public bool UsesSpriteSheetAnimation => idleSpriteSheet != null && idleFrameSize.x > 0 && idleFrameSize.y > 0
            || !string.IsNullOrWhiteSpace(idleFrameResourcePath);
        public int BattleAttackPercent => Mathf.Max(1, battleAttackPercent);
        public int BasicAttackPoisonActions => Mathf.Max(0, basicAttackPoisonActions);
        public int BasicAttackPoisonCooldownActions => Mathf.Max(0, basicAttackPoisonCooldownActions);
        public string DirectAreaAttackName => directAreaAttackName;
        public int DirectAreaAttackDamage => Mathf.Max(0, directAreaAttackDamage);
        public bool HasDirectAreaAttack => DirectAreaAttackDamage > 0 && !string.IsNullOrWhiteSpace(directAreaAttackName);
    }
}

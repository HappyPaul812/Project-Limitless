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
        // 실제 몬스터 그림이 준비되면 연결합니다. 비어 있으면 명시적인 개발용 Placeholder를 사용합니다.
        [SerializeField] private Sprite fieldSprite;
        [SerializeField] private Color placeholderColor = new Color(.35f, .8f, .35f, 1f);
        [SerializeField] private Vector2 visualSize = new Vector2(.8f, .65f);

        public string MonsterId => monsterId;
        public string DisplayName => displayName;
        public float FieldMoveSpeed => fieldMoveSpeed;
        public float MinimumIdleTime => minimumIdleTime;
        public float MaximumIdleTime => Mathf.Max(minimumIdleTime, maximumIdleTime);
        public Sprite FieldSprite => fieldSprite;
        public Color PlaceholderColor => placeholderColor;
        public Vector2 VisualSize => visualSize;
    }
}

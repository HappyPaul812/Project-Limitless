using UnityEngine;

namespace ProjectLimitless.Monster
{
    /// <summary>
    /// 특정 World/Field Scene에 몬스터 한 마리를 어디에 배치할지 정의하는 데이터 Asset입니다.
    /// 같은 MonsterDefinition을 여러 배치 데이터에서 재사용하여 몬스터 종류와 Scene 배치를 분리합니다.
    /// </summary>
    [CreateAssetMenu(fileName = "FieldMonsterSpawn", menuName = "Project Limitless/Field Monster Spawn")]
    public sealed class FieldMonsterSpawnDefinition : ScriptableObject
    {
        // 이 배치가 적용될 Scene 이름입니다.
        [SerializeField] private string sceneName;
        // Scene 안에서 배치 한 곳을 구분하는 고유 ID입니다.
        [SerializeField] private string spawnId;
        // 이 위치에 생성할 몬스터 종류 데이터입니다.
        [SerializeField] private MonsterDefinition monster;
        // 몬스터가 처음 나타나는 위치와 그 위치를 중심으로 허용할 배회 반경입니다.
        [SerializeField] private Vector2 position;
        [SerializeField, Min(.5f)] private float activityRadius = 2.5f;

        public string SceneName => sceneName;
        public string SpawnId => spawnId;
        public MonsterDefinition Monster => monster;
        public Vector2 Position => position;
        public float ActivityRadius => activityRadius;
    }
}

using UnityEngine;

namespace ProjectLimitless.Monster
{
    /// <summary>
    /// 한 Field 출입구 주변에서 몬스터가 생성되거나 배회하면 안 되는 원형 구역입니다.
    /// 출입 직후 잠깐 충돌을 무시하는 재조우 유예와 달리, 이 데이터는 필드가 열려 있는 내내
    /// 출입구 자체를 안전하게 유지합니다. Scene 이름과 좌표를 공용 코드에 넣지 않아 던전에도 재사용할 수 있습니다.
    /// </summary>
    [CreateAssetMenu(fileName = "FieldEntranceSafetyZone", menuName = "Project Limitless/Field Entrance Safety Zone")]
    public sealed class FieldEntranceSafetyZoneDefinition : ScriptableObject
    {
        [SerializeField] private string sceneName;
        [SerializeField] private string zoneId;
        [SerializeField] private Vector2 center;
        [SerializeField, Min(.5f)] private float radius = 2.2f;

        public string SceneName => sceneName;
        public string ZoneId => zoneId;
        public Vector2 Center => center;
        public float Radius => Mathf.Max(.5f, radius);
    }
}

using ProjectLimitless.Player;
using UnityEngine;

namespace ProjectLimitless.Monster
{
    /// <summary>
    /// Top-Down 필드의 몬스터가 시작 위치 주변을 천천히 배회하도록 움직입니다.
    /// Rigidbody2D를 사용하므로 나무·바위·울타리 같은 Collider와 충돌하며, 플레이어 접촉 시 멈추고 조우 Event를 보냅니다.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public sealed class MonsterFieldController : MonoBehaviour
    {
        private MonsterDefinition definition;
        private Rigidbody2D body;
        private Vector2 activityCenter;
        private Vector2 destination;
        private float activityRadius;
        private float waitUntil;
        private float nextObstacleRedirectTime;
        private bool encountered;

        public MonsterDefinition Definition => definition;
        public bool HasEncounteredPlayer => encountered;

        /// <summary>GameObject가 준비될 때 중력 없는 Top-Down 물리 이동 설정을 적용합니다.</summary>
        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            body.gravityScale = 0f;
            body.freezeRotation = true;
            body.interpolation = RigidbodyInterpolation2D.Interpolate;
            body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }

        /// <summary>Scene 설치기가 몬스터 종류, 활동 중심과 반경을 지정한 직후 호출합니다.</summary>
        public void Configure(MonsterDefinition monster, Vector2 center, float radius)
        {
            definition = monster;
            activityCenter = center;
            activityRadius = Mathf.Max(.5f, radius);
            destination = center;
            waitUntil = Time.time + Random.Range(definition.MinimumIdleTime, definition.MaximumIdleTime);
        }

        /// <summary>일정한 물리 갱신 간격마다 현재 목적지를 향해 이동합니다.</summary>
        private void FixedUpdate()
        {
            if (definition == null || encountered || Time.time < waitUntil) return;

            // 다른 물체에 밀려 활동 범위 밖으로 나가면 새 임의 목적지보다 중심 복귀를 우선합니다.
            if ((body.position - activityCenter).sqrMagnitude > activityRadius * activityRadius)
            {
                destination = activityCenter;
            }

            if (Vector2.Distance(body.position, destination) <= .08f)
            {
                ChooseNextDestination();
                return;
            }

            Vector2 nextPosition = Vector2.MoveTowards(
                body.position,
                destination,
                definition.FieldMoveSpeed * Time.fixedDeltaTime);
            body.MovePosition(nextPosition);
        }

        /// <summary>활동 중심을 기준으로 원 안의 새 목적지를 선택하고 잠시 쉬었다가 이동합니다.</summary>
        private void ChooseNextDestination(float minimumDelay = 0f)
        {
            destination = activityCenter + (Random.insideUnitCircle * activityRadius);
            float idleTime = Random.Range(definition.MinimumIdleTime, definition.MaximumIdleTime);
            waitUntil = Time.time + Mathf.Max(minimumDelay, idleTime);
        }

        /// <summary>플레이어나 장애물과 부딪힌 첫 물리 프레임에 호출됩니다.</summary>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.GetComponent<PlayerController>() != null)
            {
                BeginEncounter();
                return;
            }

            // 장애물 안쪽을 계속 목적지로 삼아 밀어붙이지 않도록 다른 방향을 고릅니다.
            ChooseNextDestination(.2f);
            nextObstacleRedirectTime = Time.time + .5f;
        }

        /// <summary>장애물과 계속 맞닿아 이동이 막힌 경우 일정 간격으로 목적지를 다시 선택합니다.</summary>
        private void OnCollisionStay2D(Collision2D collision)
        {
            if (encountered || collision.collider.GetComponent<PlayerController>() != null) return;
            if (Time.time < nextObstacleRedirectTime) return;

            ChooseNextDestination(.2f);
            nextObstacleRedirectTime = Time.time + .5f;
        }

        /// <summary>첫 플레이어 접촉에서 이동을 멈추고 전투 연결용 조우 Event를 한 번만 발생시킵니다.</summary>
        private void BeginEncounter()
        {
            if (encountered) return;

            encountered = true;
            body.linearVelocity = Vector2.zero;
            body.constraints = RigidbodyConstraints2D.FreezeAll;
            MonsterEncounterService.Raise(definition);
        }
    }
}

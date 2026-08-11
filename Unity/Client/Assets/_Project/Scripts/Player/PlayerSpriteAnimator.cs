using UnityEngine;

namespace ProjectLimitless.Player
{
    /// <summary>
    /// 플레이어 GameObject에서 이동 방향을 읽어 4방향 대기·걷기 애니메이션을 재생합니다.
    /// PlayerController의 8방향 입력을 위·아래·왼쪽·오른쪽 중 더 뚜렷한 한 방향으로 바꿔 Animator에 전달합니다.
    /// </summary>
    [RequireComponent(typeof(PlayerController), typeof(Animator))]
    public sealed class PlayerSpriteAnimator : MonoBehaviour
    {
        // 문자열을 매 프레임 비교하지 않도록 Animator 매개변수 이름을 빠른 숫자 값으로 미리 바꿉니다.
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int MoveX = Animator.StringToHash("MoveX");
        private static readonly int MoveY = Animator.StringToHash("MoveY");

        private PlayerController playerController;
        private Animator animator;
        private Vector2 facing = Vector2.down;
        private string currentState;

        /// <summary>같은 GameObject의 이동 처리와 Animator 컴포넌트를 찾아 보관합니다.</summary>
        private void Awake()
        {
            playerController = GetComponent<PlayerController>();
            animator = GetComponent<Animator>();
        }

        /// <summary>매 프레임 이동 여부와 마지막으로 바라본 방향에 맞는 애니메이션을 선택합니다.</summary>
        private void Update()
        {
            Vector2 movement = playerController.Movement;
            bool isMoving = movement.sqrMagnitude > 0.0001f;
            if (isMoving)
            {
                // 대각선 입력은 절댓값이 더 큰 축을 사용해 네 방향 Sprite 중 하나를 고릅니다.
                facing = Mathf.Abs(movement.x) > Mathf.Abs(movement.y)
                    ? new Vector2(Mathf.Sign(movement.x), 0f)
                    : new Vector2(0f, Mathf.Sign(movement.y));
            }

            animator.SetFloat(Speed, isMoving ? 1f : 0f);
            animator.SetFloat(MoveX, facing.x);
            animator.SetFloat(MoveY, facing.y);

            // 바라보는 방향과 이동 여부를 Animator 상태 이름(예: Walk_Left)으로 조합합니다.
            string direction = facing.y < 0f ? "Down" : facing.y > 0f ? "Up" : facing.x < 0f ? "Left" : "Right";
            string nextState = (isMoving ? "Walk_" : "Idle_") + direction;
            if (nextState == currentState)
            {
                return;
            }

            animator.Play(nextState, 0, 0f);
            currentState = nextState;
        }
    }
}

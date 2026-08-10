using UnityEngine;

namespace ProjectLimitless.Player
{
    /// <summary>PlayerController의 이동 입력을 4방향 Animator 상태에 연결한다.</summary>
    [RequireComponent(typeof(PlayerController), typeof(Animator))]
    public sealed class PlayerSpriteAnimator : MonoBehaviour
    {
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int MoveX = Animator.StringToHash("MoveX");
        private static readonly int MoveY = Animator.StringToHash("MoveY");

        private PlayerController playerController;
        private Animator animator;
        private Vector2 facing = Vector2.down;
        private string currentState;

        private void Awake()
        {
            playerController = GetComponent<PlayerController>();
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            Vector2 movement = playerController.Movement;
            bool isMoving = movement.sqrMagnitude > 0.0001f;
            if (isMoving)
            {
                facing = Mathf.Abs(movement.x) > Mathf.Abs(movement.y)
                    ? new Vector2(Mathf.Sign(movement.x), 0f)
                    : new Vector2(0f, Mathf.Sign(movement.y));
            }

            animator.SetFloat(Speed, isMoving ? 1f : 0f);
            animator.SetFloat(MoveX, facing.x);
            animator.SetFloat(MoveY, facing.y);

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

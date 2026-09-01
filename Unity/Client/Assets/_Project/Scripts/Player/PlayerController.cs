using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectLimitless.Player
{
    /// <summary>
    /// 플레이어 GameObject에서 키보드와 게임패드의 8방향 이동 입력을 받아 실제 위치를 움직입니다.
    /// Unity Input System으로 입력을 읽고, Rigidbody2D(2D 물리 이동 컴포넌트)로 충돌을 고려해 이동합니다.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public sealed class PlayerController : MonoBehaviour
    {
        // 플레이어가 1초 동안 이동할 거리입니다. Inspector에서 0.1 이상의 값으로 조절할 수 있습니다.
        [SerializeField, Min(0.1f)] private float moveSpeed = 4f;

        private InputAction moveAction;
        private Rigidbody2D body;
        private Vector2 movement;

        /// <summary>현재 입력된 이동 방향입니다. PlayerSpriteAnimator가 걷는 방향을 정할 때 읽습니다.</summary>
        public Vector2 Movement => movement;

        /// <summary>필요한 2D 물리 컴포넌트를 준비하고 이동 키를 Input System에 등록합니다.</summary>
        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            body.gravityScale = 0f;
            body.freezeRotation = true;
            body.interpolation = RigidbodyInterpolation2D.Interpolate;

            // 월드 Player에만 주기 저장기를 붙입니다. Battle에는 이 PlayerController가 없으므로 전투 좌표나
            // 적 HP·턴 상태가 월드 저장을 덮어쓰지 않습니다.
            if (GetComponent<ProjectLimitless.World.WorldPositionSaveController>() == null)
                gameObject.AddComponent<ProjectLimitless.World.WorldPositionSaveController>();

            moveAction = new InputAction("Move", InputActionType.Value);
            AddKeyboardBindings("<Keyboard>/w", "<Keyboard>/s", "<Keyboard>/a", "<Keyboard>/d");
            AddKeyboardBindings("<Keyboard>/upArrow", "<Keyboard>/downArrow", "<Keyboard>/leftArrow", "<Keyboard>/rightArrow");
            moveAction.AddBinding("<Gamepad>/leftStick");
        }

        /// <summary>이 컴포넌트가 활성화되면 이동 입력을 받기 시작합니다.</summary>
        private void OnEnable() => moveAction?.Enable();

        /// <summary>비활성화된 동안에는 불필요한 이동 입력을 받지 않습니다.</summary>
        private void OnDisable() => moveAction?.Disable();

        /// <summary>매 화면 프레임마다 현재 키보드 또는 게임패드 방향을 읽습니다.</summary>
        private void Update()
        {
            movement = moveAction.ReadValue<Vector2>();
            // 가로와 세로를 동시에 누를 때 대각선 속도가 더 빨라지지 않도록 방향 벡터의 길이를 1로 맞춥니다.
            if (movement.sqrMagnitude > 1f)
            {
                movement.Normalize();
            }
        }

        /// <summary>일정한 물리 갱신 간격마다 읽어 둔 방향으로 플레이어를 이동합니다.</summary>
        private void FixedUpdate()
        {
            // fixedDeltaTime을 곱하면 컴퓨터의 화면 갱신 속도와 관계없이 같은 속도로 이동합니다.
            body.MovePosition(body.position + movement * moveSpeed * Time.fixedDeltaTime);
        }

        /// <summary>GameObject가 제거될 때 직접 만든 InputAction의 메모리를 정리합니다.</summary>
        private void OnDestroy() => moveAction?.Dispose();

        /// <summary>위·아래·왼쪽·오른쪽 키 네 개를 하나의 2D 방향 입력으로 묶습니다.</summary>
        private void AddKeyboardBindings(string up, string down, string left, string right)
        {
            InputActionSetupExtensions.CompositeSyntax composite = moveAction.AddCompositeBinding("2DVector");
            composite.With("Up", up);
            composite.With("Down", down);
            composite.With("Left", left);
            composite.With("Right", right);
        }
    }
}

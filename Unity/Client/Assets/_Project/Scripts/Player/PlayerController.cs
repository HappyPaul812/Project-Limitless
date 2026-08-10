using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectLimitless.Player
{
    /// <summary>Input System으로 2D 플레이어의 8방향 물리 이동을 처리한다.</summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public sealed class PlayerController : MonoBehaviour
    {
        [SerializeField, Min(0.1f)] private float moveSpeed = 4f;

        private InputAction moveAction;
        private Rigidbody2D body;
        private Vector2 movement;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            body.gravityScale = 0f;
            body.freezeRotation = true;
            body.interpolation = RigidbodyInterpolation2D.Interpolate;

            moveAction = new InputAction("Move", InputActionType.Value);
            AddKeyboardBindings("<Keyboard>/w", "<Keyboard>/s", "<Keyboard>/a", "<Keyboard>/d");
            AddKeyboardBindings("<Keyboard>/upArrow", "<Keyboard>/downArrow", "<Keyboard>/leftArrow", "<Keyboard>/rightArrow");
            moveAction.AddBinding("<Gamepad>/leftStick");
        }

        private void OnEnable() => moveAction?.Enable();

        private void OnDisable() => moveAction?.Disable();

        private void Update()
        {
            movement = moveAction.ReadValue<Vector2>();
            if (movement.sqrMagnitude > 1f)
            {
                movement.Normalize();
            }
        }

        private void FixedUpdate()
        {
            body.MovePosition(body.position + movement * moveSpeed * Time.fixedDeltaTime);
        }

        private void OnDestroy() => moveAction?.Dispose();

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

using ProjectLimitless.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectLimitless.NPC
{
    /// <summary>주변의 가장 가까운 NPC와 시간 제한 없이 상호작용한다.</summary>
    public sealed class InteractionSystem : MonoBehaviour
    {
        [SerializeField, Min(0.1f)] private float interactionRadius = 2f;

        private InputAction interactAction;
        private InputAction cancelAction;
        private NpcController currentTarget;

        public NpcController CurrentTarget => currentTarget;
        public float InteractionRadius => interactionRadius;

        public void Configure(float radius)
        {
            interactionRadius = Mathf.Max(0.1f, radius);
        }

        private void Awake()
        {
            interactAction = new InputAction("Interact", InputActionType.Button);
            interactAction.AddBinding("<Keyboard>/e");
            interactAction.AddBinding("<Keyboard>/f");
            interactAction.AddBinding("<Gamepad>/buttonSouth");
            interactAction.performed += OnInteract;

            cancelAction = new InputAction("CloseDialogue", InputActionType.Button);
            cancelAction.AddBinding("<Keyboard>/escape");
            cancelAction.AddBinding("<Gamepad>/buttonEast");
            cancelAction.performed += _ => DialoguePresenter.Instance?.Hide();
        }

        private void OnEnable()
        {
            interactAction?.Enable();
            cancelAction?.Enable();
            RefreshCurrentTarget();
        }

        private void OnDisable()
        {
            interactAction?.Disable();
            cancelAction?.Disable();
        }

        private void OnDestroy()
        {
            if (interactAction != null)
            {
                interactAction.performed -= OnInteract;
                interactAction.Dispose();
            }

            cancelAction?.Dispose();
        }

        private void Update()
        {
            RefreshCurrentTarget();
        }

        private void OnInteract(InputAction.CallbackContext _)
        {
            // Scene 재생성 또는 활성화 순서와 무관하게 입력 순간의 실제 대상을 사용한다.
            RefreshCurrentTarget();
            if (currentTarget != null)
            {
                DialoguePresenter.Instance?.Show(currentTarget.DisplayName, currentTarget.Dialogue);
            }
        }

        private void RefreshCurrentTarget()
        {
            SetCurrentTarget(FindNearestNpc());
        }

        private void SetCurrentTarget(NpcController newTarget)
        {
            if (currentTarget == newTarget)
            {
                return;
            }

            if (currentTarget != null)
            {
                NpcInteractionPrompt.GetOrAdd(currentTarget).SetVisible(false);
            }

            currentTarget = newTarget;
            if (currentTarget != null)
            {
                NpcInteractionPrompt.GetOrAdd(currentTarget).SetVisible(true);
            }
        }

        private NpcController FindNearestNpc()
        {
            NpcController[] npcs = FindObjectsByType<NpcController>(FindObjectsSortMode.None);
            NpcController nearestNpc = null;
            float closestDistance = interactionRadius * interactionRadius;

            foreach (NpcController npc in npcs)
            {
                float distance = (npc.transform.position - transform.position).sqrMagnitude;
                if (distance <= closestDistance)
                {
                    closestDistance = distance;
                    nearestNpc = npc;
                }
            }

            return nearestNpc;
        }
    }
}

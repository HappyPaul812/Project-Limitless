using ProjectLimitless.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectLimitless.NPC
{
    /// <summary>주변의 가장 가까운 NPC와 시간 제한 없이 상호작용한다.</summary>
    public sealed class InteractionSystem : MonoBehaviour
    {
        [SerializeField, Min(0.1f)] private float interactionRange = 1.5f;

        private InputAction interactAction;
        private InputAction cancelAction;

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

        private void OnInteract(InputAction.CallbackContext _)
        {
            NpcController nearestNpc = FindNearestNpc();
            if (nearestNpc != null)
            {
                DialoguePresenter.Instance?.Show(nearestNpc.DisplayName, nearestNpc.Dialogue);
            }
        }

        private NpcController FindNearestNpc()
        {
            NpcController[] npcs = FindObjectsByType<NpcController>(FindObjectsSortMode.None);
            NpcController nearestNpc = null;
            float closestDistance = interactionRange * interactionRange;

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

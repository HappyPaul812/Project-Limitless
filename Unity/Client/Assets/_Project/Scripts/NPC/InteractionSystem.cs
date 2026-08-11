using ProjectLimitless.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectLimitless.NPC
{
    /// <summary>
    /// 플레이어 GameObject에 붙어 주변에서 가장 가까운 NPC를 찾고 대화를 시작합니다.
    /// 정확한 방향을 맞추거나 제한 시간 안에 누를 필요 없이 일정 거리 안에서 상호작용할 수 있어 조작 부담을 줄입니다.
    /// </summary>
    public sealed class InteractionSystem : MonoBehaviour
    {
        // 이 거리 안에 들어온 NPC만 대화 대상으로 봅니다. Inspector에서 0.1 이상으로 조절할 수 있습니다.
        [SerializeField, Min(0.1f)] private float interactionRadius = 2f;

        private InputAction interactAction;
        private InputAction cancelAction;
        private NpcController currentTarget;

        public NpcController CurrentTarget => currentTarget;
        public float InteractionRadius => interactionRadius;

        /// <summary>Scene 생성 도구가 상호작용 가능 거리를 설정할 때 사용합니다.</summary>
        public void Configure(float radius)
        {
            interactionRadius = Mathf.Max(0.1f, radius);
        }

        /// <summary>대화 시작·닫기 키와 게임패드 버튼을 Input System에 등록합니다.</summary>
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

        /// <summary>활성화될 때 입력을 켜고 현재 가장 가까운 NPC를 즉시 확인합니다.</summary>
        private void OnEnable()
        {
            interactAction?.Enable();
            cancelAction?.Enable();
            RefreshCurrentTarget();
        }

        /// <summary>비활성화될 때 상호작용 입력을 받지 않도록 합니다.</summary>
        private void OnDisable()
        {
            interactAction?.Disable();
            cancelAction?.Disable();
        }

        /// <summary>등록한 입력 이벤트를 해제하고 InputAction의 메모리를 정리합니다.</summary>
        private void OnDestroy()
        {
            if (interactAction != null)
            {
                interactAction.performed -= OnInteract;
                interactAction.Dispose();
            }

            cancelAction?.Dispose();
        }

        /// <summary>플레이어 또는 NPC가 움직일 수 있으므로 매 프레임 가장 가까운 대상을 다시 확인합니다.</summary>
        private void Update()
        {
            RefreshCurrentTarget();
        }

        /// <summary>상호작용 버튼을 누른 순간의 대상 이름과 대사를 대화 UI에 표시합니다.</summary>
        private void OnInteract(InputAction.CallbackContext _)
        {
            // Scene 재생성 또는 활성화 순서와 무관하게 입력 순간의 실제 대상을 사용한다.
            RefreshCurrentTarget();
            if (currentTarget != null)
            {
                DialoguePresenter.Instance?.Show(currentTarget.DisplayName, currentTarget.Dialogue);
            }
        }

        /// <summary>현재 위치에서 가장 가까운 NPC를 찾아 현재 대상으로 갱신합니다.</summary>
        private void RefreshCurrentTarget()
        {
            SetCurrentTarget(FindNearestNpc());
        }

        /// <summary>대상이 바뀔 때 이전 안내는 숨기고 새 대상의 안내만 표시합니다.</summary>
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

        /// <summary>Scene의 모든 NPC 중 상호작용 반경 안에서 가장 가까운 한 명을 찾습니다.</summary>
        private NpcController FindNearestNpc()
        {
            NpcController[] npcs = FindObjectsByType<NpcController>(FindObjectsSortMode.None);
            NpcController nearestNpc = null;
            // 제곱 거리를 비교하면 실제 거리 계산에 필요한 제곱근 연산을 피하면서 같은 결과를 얻습니다.
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

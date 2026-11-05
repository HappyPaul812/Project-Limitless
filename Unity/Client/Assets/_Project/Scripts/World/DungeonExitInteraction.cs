using ProjectLimitless.Core;
using ProjectLimitless.Player;
using ProjectLimitless.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace ProjectLimitless.World
{
    /// <summary>던전 입구의 명시적 귀환 안내입니다. 퀘스트·몬스터·인벤토리는 건드리지 않고 기존 Scene 전환만 요청합니다.</summary>
    public sealed class DungeonExitInteraction : MonoBehaviour
    {
        private InputAction interact;
        private InputAction confirmKeys;
        private Transform player;
        private GameObject marker;
        private string targetScene, targetSpawn, label;
        private const float Range = 1.4f;

        public void Configure(bool lowerFloor)
        {
            targetScene = lowerFloor ? "Dungeon_01" : "Field_03";
            targetSpawn = lowerFloor ? "Spawn_From_B2" : "Spawn_From_Dungeon01";
            label = lowerFloor ? "B1으로 돌아가기" : "밖으로 나가기";
            marker = CreateMarker();
        }

        private void Awake()
        {
            interact = new InputAction("DungeonExit", InputActionType.Button);
            interact.AddBinding("<Keyboard>/e"); interact.AddBinding("<Keyboard>/f");
            interact.AddBinding("<Gamepad>/buttonSouth");
            interact.performed += _ => TryInteract();
            // E/F/A는 현재 선택 버튼을 실행합니다. UI 모듈이 같은 A를 다시 Submit해도
            // Presenter의 프레임 차단으로 확인·취소가 두 번 실행되지 않습니다.
            confirmKeys = new InputAction("DungeonExitConfirm", InputActionType.Button);
            confirmKeys.AddBinding("<Keyboard>/e"); confirmKeys.AddBinding("<Keyboard>/f");
            confirmKeys.AddBinding("<Gamepad>/buttonSouth");
            confirmKeys.performed += _ => {
                var d = DialoguePresenter.Instance;
                if (d != null && WorldModalState.IsOwnedBy(d) && d.CanBeginInteractionThisFrame && confirmationOpen)
                {
                    var selected = EventSystem.current?.currentSelectedGameObject?.GetComponent<Button>();
                    if (selected != null) selected.onClick.Invoke();
                    else Confirm();
                }
            };
        }
        private bool confirmationOpen;
        private void OnEnable() { interact?.Enable(); confirmKeys?.Enable(); }
        private void OnDisable() { interact?.Disable(); confirmKeys?.Disable(); }
        private void OnDestroy() { interact?.Dispose(); confirmKeys?.Dispose(); }
        private bool Near()
        {
            if (player == null) player = FindAnyObjectByType<PlayerController>()?.transform;
            return player != null && Vector2.Distance(player.position, transform.position) <= Range;
        }
        private void Update()
        {
            if (confirmationOpen && !WorldModalState.IsOpen) confirmationOpen = false;
            if (marker != null) { Near(); marker.SetActive(!WorldModalState.IsOpen && player != null && Vector2.Distance(player.position, transform.position) <= 5f); }
        }
        public bool TryInteract()
        {
            var d = DialoguePresenter.Instance;
            if (!Near() || d == null || WorldModalState.IsOpen || !d.CanBeginInteractionThisFrame) return false;
            confirmationOpen = true;
            d.ShowConfirmation("", label + "?\n조사 진행은 유지되며 나중에 다시 들어올 수 있습니다.", label, "계속 탐험", Confirm);
            return true;
        }
        private void Confirm()
        {
            var d = DialoguePresenter.Instance;
            if (!confirmationOpen || d == null || !d.CanBeginInteractionThisFrame) return;
            confirmationOpen = false;
            d.Hide();
            // 이동 실패 시에도 모달을 정리하고 현재 위치에서 재시도할 수 있습니다.
            if (!SceneTransitionService.TryLoad(targetScene, targetSpawn))
                d.Show("", "이동할 수 없습니다. 잠시 후 다시 시도해 주세요.");
        }
        private GameObject CreateMarker()
        {
            var root = new GameObject("DungeonExitMarker", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            root.transform.SetParent(transform, false);
            root.transform.localPosition = new Vector3(0, .55f, 0);
            root.transform.localScale = Vector3.one * .01f;
            var canvas = root.GetComponent<Canvas>(); canvas.renderMode = RenderMode.WorldSpace; canvas.sortingOrder = 24;
            root.GetComponent<RectTransform>().sizeDelta = new Vector2(390, 80);
            var button = new GameObject("ExitButton", typeof(Image), typeof(Button));
            button.transform.SetParent(root.transform, false);
            var rect = button.GetComponent<RectTransform>(); rect.anchorMin = Vector2.zero; rect.anchorMax = Vector2.one;
            rect.offsetMin = rect.offsetMax = Vector2.zero;
            button.GetComponent<Image>().color = new Color(.08f, .13f, .2f, .96f);
            button.GetComponent<Button>().onClick.AddListener(() => TryInteract());
            var text = new GameObject("Label", typeof(Text)).GetComponent<Text>();
            text.transform.SetParent(button.transform, false);
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"); text.fontSize = 22;
            text.alignment = TextAnchor.MiddleCenter; text.color = Color.white; text.raycastTarget = false;
            text.text = label + "\n[E/F · 게임패드 A · 클릭]";
            text.rectTransform.anchorMin = Vector2.zero; text.rectTransform.anchorMax = Vector2.one;
            text.rectTransform.offsetMin = text.rectTransform.offsetMax = Vector2.zero;
            return root;
        }
    }
}

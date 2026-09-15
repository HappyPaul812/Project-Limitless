using System;
using ProjectLimitless.Core;
using ProjectLimitless.Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace ProjectLimitless.UI
{
    /// <summary>
    /// DialogueSystem GameObject에 붙어 화자 이름과 대사를 화면 아래쪽 패널에 표시합니다.
    /// Scene 어디서든 하나의 Instance를 통해 같은 대화 UI를 사용하도록 관리합니다.
    /// </summary>
    public sealed class DialoguePresenter : MonoBehaviour
    {
        public static DialoguePresenter Instance { get; private set; }

        // 대화에 사용할 글꼴입니다. 비어 있으면 Unity 기본 글꼴을 사용합니다.
        [SerializeField] private Font dialogueFont;
        private GameObject panel;
        private Text dialogueText;
        private GameObject choiceRow;
        private Button confirmButton;
        private Button cancelButton;
        private Transform dialogueViewer;
        private Transform dialogueOwner;
        private float dialogueBreakDistance;
        private bool isDistanceTracked;

        /// <summary>중복 대화 UI를 제거하고, 이 객체를 공용 Instance로 등록한 뒤 패널을 만듭니다.</summary>
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            CreatePanel();
        }

        /// <summary>현재 공용 Instance가 제거되는 객체라면 참조를 비웁니다.</summary>
        private void OnDestroy()
        {
            WorldExperienceHud.SetInteractionUiOpen(this, false);
            WorldModalState.Release(this);
            if (Instance == this)
            {
                Instance = null;
            }
        }

        /// <summary>화자와 대사를 넣고 대화 패널을 화면에 표시합니다.</summary>
        public void Show(string speaker, string message)
        {
            if (!WorldModalState.TryAcquire(this)) return;
            dialogueText.text = $"{speaker}\n{message}\n\n[Esc 또는 게임패드 B: 닫기]";
            choiceRow.SetActive(false);
            panel.SetActive(true);
            panel.transform.SetAsLastSibling();
            WorldExperienceHud.SetInteractionUiOpen(this, true);
        }

        /// <summary>시간 제한 없이 마우스·키보드·게임패드로 고를 수 있는 두 선택지를 표시합니다.</summary>
        public void ShowConfirmation(
            string speaker,
            string message,
            string confirmText,
            string cancelText,
            Action onConfirmed)
        {
            if (!WorldModalState.TryAcquire(this)) return;
            dialogueText.text = $"{speaker}\n{message}";
            choiceRow.SetActive(true);
            ConfigureButton(confirmButton, confirmText, () =>
            {
                choiceRow.SetActive(false);
                onConfirmed?.Invoke();
            });
            ConfigureButton(cancelButton, cancelText, Hide);
            panel.SetActive(true);
            panel.transform.SetAsLastSibling();
            WorldExperienceHud.SetInteractionUiOpen(this, true);
            confirmButton.Select();
        }

        /// <summary>대화 내용을 유지한 채 패널을 화면에서 숨깁니다.</summary>
        public void Hide()
        {
            panel.SetActive(false);
            ClearDistanceTracking();
            WorldExperienceHud.SetInteractionUiOpen(this, false);
            WorldModalState.Release(this);
        }

        /// <summary>현재 대화의 실제 참여자 참조와 공통 종료 거리를 등록합니다.</summary>
        public void TrackDistance(Transform viewer, Transform owner, float breakDistance)
        {
            dialogueViewer = viewer;
            dialogueOwner = owner;
            dialogueBreakDistance = Mathf.Max(0.1f, breakDistance);
            isDistanceTracked = true;
        }

        /// <summary>대화 중 상대가 사라지거나 종료 거리를 벗어나면 공통 종료 경로로 닫습니다.</summary>
        private void Update()
        {
            if (panel == null || !panel.activeSelf || !isDistanceTracked)
            {
                return;
            }

            if (dialogueOwner == null || dialogueViewer == null)
            {
                Hide();
                return;
            }

            if (!dialogueOwner.gameObject.activeInHierarchy
                || !dialogueViewer.gameObject.activeInHierarchy
                || (dialogueOwner.position - dialogueViewer.position).sqrMagnitude
                    > dialogueBreakDistance * dialogueBreakDistance)
            {
                Hide();
            }
        }

        private void ClearDistanceTracking()
        {
            dialogueViewer = null;
            dialogueOwner = null;
            dialogueBreakDistance = 0f;
            isDistanceTracked = false;
        }

        /// <summary>Scene 전환이나 객체 비활성화 중에도 HUD 억제 상태가 남지 않게 해제합니다.</summary>
        private void OnDisable()
        {
            WorldExperienceHud.SetInteractionUiOpen(this, false);
            WorldModalState.Release(this);
        }

        /// <summary>해상도에 맞춰 크기가 조절되는 Canvas와 대화 배경·글자를 코드로 구성합니다.</summary>
        private void CreatePanel()
        {
            if (EventSystem.current == null)
            {
                InputSystemUIInputModule inputModule = new GameObject("EventSystem", typeof(EventSystem))
                    .AddComponent<InputSystemUIInputModule>();
                inputModule.AssignDefaultActions();
            }

            GameObject canvasObject = new GameObject("DialogueCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasObject.transform.SetParent(transform, false);
            // Screen Space Overlay는 카메라 위치와 관계없이 UI를 화면 위에 고정해 표시합니다.
            Canvas dialogueCanvas = canvasObject.GetComponent<Canvas>();
            dialogueCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            // World HUD(5)보다 높게 두어 표시 억제가 실패해도 상호작용 UI가 앞에 남습니다.
            dialogueCanvas.sortingOrder = 10;
            canvasObject.GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasObject.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1280, 720);

            panel = new GameObject("DialoguePanel", typeof(Image));
            panel.transform.SetParent(canvasObject.transform, false);
            Image panelImage = panel.GetComponent<Image>();
            panelImage.color = new Color(0.04f, 0.06f, 0.1f, 0.92f);
            RectTransform panelRect = panel.GetComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0.08f, 0.05f);
            panelRect.anchorMax = new Vector2(0.92f, 0.28f);
            panelRect.offsetMin = Vector2.zero;
            panelRect.offsetMax = Vector2.zero;

            GameObject textObject = new GameObject("DialogueText", typeof(Text));
            textObject.transform.SetParent(panel.transform, false);
            dialogueText = textObject.GetComponent<Text>();
            dialogueText.font = dialogueFont != null
                ? dialogueFont
                : Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            dialogueText.fontSize = 28;
            dialogueText.color = Color.white;
            dialogueText.alignment = TextAnchor.MiddleLeft;
            dialogueText.horizontalOverflow = HorizontalWrapMode.Wrap;
            dialogueText.verticalOverflow = VerticalWrapMode.Overflow;
            RectTransform textRect = textObject.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = new Vector2(28f, 20f);
            textRect.offsetMax = new Vector2(-28f, -62f);

            choiceRow = new GameObject("ChoiceRow", typeof(RectTransform));
            choiceRow.transform.SetParent(panel.transform, false);
            RectTransform choiceRect = choiceRow.GetComponent<RectTransform>();
            choiceRect.anchorMin = new Vector2(0.48f, 0f);
            choiceRect.anchorMax = new Vector2(0.98f, 0f);
            choiceRect.pivot = new Vector2(1f, 0f);
            choiceRect.anchoredPosition = new Vector2(0f, 14f);
            choiceRect.sizeDelta = new Vector2(520f, 52f);

            confirmButton = CreateChoiceButton(choiceRow.transform, "ConfirmButton", new Vector2(-270f, 0f));
            cancelButton = CreateChoiceButton(choiceRow.transform, "CancelButton", Vector2.zero);

            panel.SetActive(false);
        }

        /// <summary>선택지 버튼의 공통 크기와 읽기 쉬운 글자 스타일을 구성합니다.</summary>
        private Button CreateChoiceButton(Transform parent, string objectName, Vector2 position)
        {
            GameObject buttonObject = new GameObject(objectName, typeof(Image), typeof(Button));
            buttonObject.transform.SetParent(parent, false);
            RectTransform rect = buttonObject.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(1f, 0f);
            rect.anchorMax = new Vector2(1f, 0f);
            rect.pivot = new Vector2(1f, 0f);
            rect.anchoredPosition = position;
            rect.sizeDelta = new Vector2(250f, 48f);
            buttonObject.GetComponent<Image>().color = new Color(0.18f, 0.24f, 0.34f, 1f);

            GameObject labelObject = new GameObject("Label", typeof(Text));
            labelObject.transform.SetParent(buttonObject.transform, false);
            Text label = labelObject.GetComponent<Text>();
            label.font = dialogueFont != null ? dialogueFont : Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            label.fontSize = 22;
            label.color = Color.white;
            label.alignment = TextAnchor.MiddleCenter;
            RectTransform labelRect = labelObject.GetComponent<RectTransform>();
            labelRect.anchorMin = Vector2.zero;
            labelRect.anchorMax = Vector2.one;
            labelRect.offsetMin = Vector2.zero;
            labelRect.offsetMax = Vector2.zero;
            return buttonObject.GetComponent<Button>();
        }

        /// <summary>이전 상호작용의 Listener가 다음 선택에서 다시 실행되지 않도록 교체합니다.</summary>
        private static void ConfigureButton(Button button, string label, UnityEngine.Events.UnityAction action)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(action);
            button.GetComponentInChildren<Text>().text = label;
        }
    }
}

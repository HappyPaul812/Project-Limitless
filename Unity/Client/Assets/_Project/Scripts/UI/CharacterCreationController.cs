using ProjectLimitless.Core;
using ProjectLimitless.Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjectLimitless.UI
{
    /// <summary>
    /// CharacterCreation Scene에서 Male/Female 외형을 고르고 Starter Village로 이동하는 최소 화면을 만듭니다.
    /// 선택값은 GameSessionData에 기록되어 Scene이 바뀐 뒤 PlayerVisualController가 읽을 수 있습니다.
    /// 실제 Player Prefab을 복제하지 않고 각 외형의 Down Idle Sprite 한 장만 미리보기로 사용합니다.
    /// </summary>
    public sealed class CharacterCreationController : MonoBehaviour
    {
        private const string DefaultWorldSceneName = "World_StarterVillage";

        // Editor 생성 도구가 실제 Male/Female Down Idle Sprite를 연결합니다.
        [SerializeField] private Sprite malePreviewSprite;
        [SerializeField] private Sprite femalePreviewSprite;
        [SerializeField] private string worldSceneName = DefaultWorldSceneName;

        private readonly Color normalButtonColor = new Color(0.14f, 0.18f, 0.27f, 1f);
        private readonly Color selectedButtonColor = new Color(0.22f, 0.34f, 0.52f, 1f);
        private readonly Color accentColor = new Color(0.42f, 0.78f, 1f, 1f);

        private Button maleButton;
        private Button femaleButton;
        private Button startButton;
        private Text maleLabel;
        private Text femaleLabel;
        private Text selectionLabel;
        private Image maleBackground;
        private Image femaleBackground;
        private Outline maleOutline;
        private Outline femaleOutline;
        private PlayerVisualType selectedVisual = PlayerVisualType.Male;
        private bool isStarting;

        /// <summary>Editor 생성 도구가 미리보기 Sprite와 입장할 월드 Scene을 연결합니다.</summary>
        public void Configure(Sprite maleSprite, Sprite femaleSprite, string targetWorldSceneName)
        {
            malePreviewSprite = maleSprite;
            femalePreviewSprite = femaleSprite;
            worldSceneName = targetWorldSceneName;
        }

        /// <summary>Canvas, 선택 버튼, 미리보기, 상태 문구, 시작 버튼을 코드로 구성합니다.</summary>
        private void Awake()
        {
            GameSessionData.Reset();
            CreateEventSystem();
            CreateInterface();
            SelectVisual(PlayerVisualType.Male);
            EventSystem.current.SetSelectedGameObject(maleButton.gameObject);
        }

        /// <summary>Tab과 Space도 사용할 수 있게 하여 마우스 없이 모든 항목을 조작할 수 있게 합니다.</summary>
        private void Update()
        {
            if (Keyboard.current == null)
            {
                return;
            }

            if (Keyboard.current.tabKey.wasPressedThisFrame)
            {
                MoveToNextControl(Keyboard.current.shiftKey.isPressed ? -1 : 1);
            }

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                InvokeSelectedControl();
            }
        }

        /// <summary>Male 또는 Female 선택을 세션에 즉시 기록하고 글자와 테두리로 현재 상태를 알립니다.</summary>
        private void SelectVisual(PlayerVisualType visualType)
        {
            selectedVisual = visualType;
            GameSessionData.SelectPlayerVisual(visualType);
            bool isMale = visualType == PlayerVisualType.Male;

            maleLabel.text = isMale ? "남성\n✓ 선택됨" : "남성";
            femaleLabel.text = isMale ? "여성" : "여성\n✓ 선택됨";
            selectionLabel.text = $"현재 선택: {(isMale ? "남성" : "여성")}";
            SetChoiceAppearance(maleBackground, maleOutline, isMale);
            SetChoiceAppearance(femaleBackground, femaleOutline, !isMale);
        }

        /// <summary>선택한 외형을 확정한 뒤 Starter Village를 한 번만 불러옵니다.</summary>
        private void StartGame()
        {
            if (isStarting)
            {
                return;
            }

            isStarting = true;
            GameSessionData.SelectPlayerVisual(selectedVisual);
            startButton.interactable = false;
            SceneManager.LoadSceneAsync(worldSceneName, LoadSceneMode.Single);
        }

        /// <summary>선택된 버튼은 색상뿐 아니라 굵은 테두리로도 구분합니다.</summary>
        private void SetChoiceAppearance(Image background, Outline outline, bool isSelected)
        {
            background.color = isSelected ? selectedButtonColor : normalButtonColor;
            outline.effectColor = isSelected ? accentColor : new Color(0.38f, 0.42f, 0.5f, 1f);
            outline.effectDistance = isSelected ? new Vector2(5f, -5f) : new Vector2(2f, -2f);
        }

        /// <summary>Tab 또는 Shift+Tab으로 Male, Female, 게임 시작 버튼을 순환합니다.</summary>
        private void MoveToNextControl(int direction)
        {
            Button[] controls = { maleButton, femaleButton, startButton };
            GameObject current = EventSystem.current.currentSelectedGameObject;
            int index = System.Array.FindIndex(controls, button => button.gameObject == current);
            int nextIndex = (index + direction + controls.Length) % controls.Length;
            EventSystem.current.SetSelectedGameObject(controls[nextIndex].gameObject);
        }

        /// <summary>Space를 누르면 현재 키보드 포커스가 있는 버튼을 클릭한 것처럼 실행합니다.</summary>
        private void InvokeSelectedControl()
        {
            GameObject current = EventSystem.current.currentSelectedGameObject;
            current?.GetComponent<Button>()?.onClick.Invoke();
        }

        /// <summary>방향키·Enter 입력을 처리할 Input System 기반 EventSystem을 만듭니다.</summary>
        private static void CreateEventSystem()
        {
            if (EventSystem.current != null)
            {
                return;
            }

            GameObject eventSystemObject = new GameObject("EventSystem", typeof(EventSystem));
            InputSystemUIInputModule inputModule = eventSystemObject.AddComponent<InputSystemUIInputModule>();
            inputModule.AssignDefaultActions();
        }

        /// <summary>큰 버튼과 명확한 한글 문구를 사용하는 Character Creation 화면을 만듭니다.</summary>
        private void CreateInterface()
        {
            Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            GameObject canvasObject = new GameObject("CharacterCreationCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasObject.transform.SetParent(transform, false);
            Canvas canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1280f, 720f);

            Image background = CreateImage(canvasObject.transform, "Background", new Color(0.045f, 0.065f, 0.11f, 1f));
            Stretch(background.rectTransform);
            CreateText(canvasObject.transform, "Title", "Project-Limitless", font, 46, new Vector2(0.5f, 0.88f), new Vector2(720f, 70f));
            CreateText(canvasObject.transform, "Subtitle", "캐릭터 선택", font, 34, new Vector2(0.5f, 0.78f), new Vector2(520f, 58f));

            maleButton = CreateChoiceButton(canvasObject.transform, "MaleButton", "남성", malePreviewSprite, font, new Vector2(0.35f, 0.49f), out maleLabel, out maleBackground, out maleOutline);
            femaleButton = CreateChoiceButton(canvasObject.transform, "FemaleButton", "여성", femalePreviewSprite, font, new Vector2(0.65f, 0.49f), out femaleLabel, out femaleBackground, out femaleOutline);
            maleButton.onClick.AddListener(() => SelectVisual(PlayerVisualType.Male));
            femaleButton.onClick.AddListener(() => SelectVisual(PlayerVisualType.Female));

            selectionLabel = CreateText(canvasObject.transform, "SelectionLabel", "현재 선택: 남성", font, 28, new Vector2(0.5f, 0.24f), new Vector2(500f, 52f));
            startButton = CreateTextButton(canvasObject.transform, "StartButton", "게임 시작", font, new Vector2(0.5f, 0.13f), new Vector2(340f, 76f));
            startButton.onClick.AddListener(StartGame);
            CreateText(canvasObject.transform, "KeyboardHelp", "Tab/방향키: 이동   Enter/Space: 선택", font, 20, new Vector2(0.5f, 0.045f), new Vector2(700f, 38f));

            ConfigureNavigation();
        }

        /// <summary>방향키를 누를 때 두 외형과 시작 버튼 사이를 예측 가능한 순서로 이동하게 합니다.</summary>
        private void ConfigureNavigation()
        {
            Navigation maleNavigation = new Navigation { mode = Navigation.Mode.Explicit, selectOnRight = femaleButton, selectOnDown = startButton };
            Navigation femaleNavigation = new Navigation { mode = Navigation.Mode.Explicit, selectOnLeft = maleButton, selectOnDown = startButton };
            Navigation startNavigation = new Navigation { mode = Navigation.Mode.Explicit, selectOnUp = maleButton };
            maleButton.navigation = maleNavigation;
            femaleButton.navigation = femaleNavigation;
            startButton.navigation = startNavigation;
        }

        private static Button CreateChoiceButton(Transform parent, string name, string label, Sprite previewSprite, Font font, Vector2 anchor, out Text labelText, out Image background, out Outline outline)
        {
            GameObject buttonObject = new GameObject(name, typeof(Image), typeof(Button), typeof(Outline));
            buttonObject.transform.SetParent(parent, false);
            SetRect(buttonObject.GetComponent<RectTransform>(), anchor, new Vector2(300f, 280f));
            background = buttonObject.GetComponent<Image>();
            Button button = buttonObject.GetComponent<Button>();
            button.targetGraphic = background;
            outline = buttonObject.GetComponent<Outline>();

            Image preview = CreateImage(buttonObject.transform, "Preview", Color.white);
            preview.sprite = previewSprite;
            preview.preserveAspect = true;
            SetRect(preview.rectTransform, new Vector2(0.5f, 0.62f), new Vector2(190f, 190f));
            labelText = CreateText(buttonObject.transform, "Label", label, font, 28, new Vector2(0.5f, 0.13f), new Vector2(260f, 70f));
            return button;
        }

        private static Button CreateTextButton(Transform parent, string name, string label, Font font, Vector2 anchor, Vector2 size)
        {
            GameObject buttonObject = new GameObject(name, typeof(Image), typeof(Button), typeof(Outline));
            buttonObject.transform.SetParent(parent, false);
            SetRect(buttonObject.GetComponent<RectTransform>(), anchor, size);
            Image image = buttonObject.GetComponent<Image>();
            image.color = new Color(0.16f, 0.42f, 0.65f, 1f);
            Button button = buttonObject.GetComponent<Button>();
            button.targetGraphic = image;
            Outline outline = buttonObject.GetComponent<Outline>();
            outline.effectColor = Color.white;
            outline.effectDistance = new Vector2(2f, -2f);
            CreateText(buttonObject.transform, "Label", label, font, 30, Vector2.one * 0.5f, size - new Vector2(20f, 12f));
            return button;
        }

        private static Text CreateText(Transform parent, string name, string value, Font font, int fontSize, Vector2 anchor, Vector2 size)
        {
            GameObject textObject = new GameObject(name, typeof(Text));
            textObject.transform.SetParent(parent, false);
            Text text = textObject.GetComponent<Text>();
            text.font = font;
            text.fontSize = fontSize;
            text.color = Color.white;
            text.alignment = TextAnchor.MiddleCenter;
            text.text = value;
            SetRect(text.rectTransform, anchor, size);
            return text;
        }

        private static Image CreateImage(Transform parent, string name, Color color)
        {
            GameObject imageObject = new GameObject(name, typeof(Image));
            imageObject.transform.SetParent(parent, false);
            Image image = imageObject.GetComponent<Image>();
            image.color = color;
            return image;
        }

        private static void SetRect(RectTransform rectTransform, Vector2 anchor, Vector2 size)
        {
            rectTransform.anchorMin = anchor;
            rectTransform.anchorMax = anchor;
            rectTransform.pivot = Vector2.one * 0.5f;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = size;
        }

        private static void Stretch(RectTransform rectTransform)
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
        }
    }
}

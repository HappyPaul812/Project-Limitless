using ProjectLimitless.Core;
using ProjectLimitless.Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text.RegularExpressions;

namespace ProjectLimitless.UI
{
    /// <summary>
    /// CharacterCreation Scene에서 Male/Female 외형과 이름을 정한 뒤 다음 PathSelection 단계로 이동합니다.
    /// 선택값은 GameSessionData에 기록되어 여러 캐릭터 생성 Scene 사이에서 유지됩니다.
    /// 실제 Player Prefab을 복제하지 않고 각 외형의 Down Idle Sprite 한 장만 미리보기로 사용합니다.
    /// </summary>
    public sealed class CharacterCreationController : MonoBehaviour
    {
        private const string DefaultNextSceneName = "PathSelection";
        private const int MaximumPlayerNameLength = 12;

        // Editor 생성 도구가 실제 Male/Female Down Idle Sprite를 연결합니다.
        [SerializeField] private Sprite malePreviewSprite;
        [SerializeField] private Sprite femalePreviewSprite;
        [SerializeField] private string nextSceneName = DefaultNextSceneName;

        // 화면 전체에서 반복해서 사용하는 판타지 RPG 색상입니다. 충분한 명암 차이로 글자를 쉽게 읽을 수 있게 합니다.
        private readonly Color normalButtonColor = new Color(0.075f, 0.105f, 0.17f, 0.97f);
        private readonly Color selectedButtonColor = new Color(0.13f, 0.2f, 0.3f, 1f);
        private readonly Color accentColor = new Color(0.88f, 0.7f, 0.32f, 1f);
        private readonly Color mutedBorderColor = new Color(0.32f, 0.4f, 0.52f, 1f);
        private readonly Color focusColor = new Color(1f, 0.86f, 0.48f, 1f);

        private Button maleButton;
        private Button femaleButton;
        private Button startButton;
        private Text maleLabel;
        private Text femaleLabel;
        private Text selectionLabel;
        private InputField nameInputField;
        private Text nameErrorLabel;
        private Image maleBackground;
        private Image femaleBackground;
        private Outline maleOutline;
        private Outline femaleOutline;
        private Outline nameInputOutline;
        private Outline startButtonOutline;
        private PlayerVisualType selectedVisual = PlayerVisualType.Male;
        private bool isStarting;

        /// <summary>Editor 생성 도구가 미리보기 Sprite와 입장할 월드 Scene을 연결합니다.</summary>
        public void Configure(Sprite maleSprite, Sprite femaleSprite, string targetNextSceneName)
        {
            malePreviewSprite = maleSprite;
            femalePreviewSprite = femaleSprite;
            nextSceneName = targetNextSceneName;
        }

        /// <summary>Canvas, 선택 버튼, 미리보기, 상태 문구, 시작 버튼을 코드로 구성합니다.</summary>
        private void Awake()
        {
            CreateEventSystem();
            CreateInterface();
            // 이전 단계에서 돌아온 경우 세션의 Female/Male과 이름을 그대로 UI에 복원합니다.
            selectedVisual = GameSessionData.SelectedPlayerVisual;
            nameInputField.text = GameSessionData.PlayerName;
            SelectVisual(selectedVisual);
            EventSystem.current.SetSelectedGameObject(maleButton.gameObject);
        }

        /// <summary>Tab, Space, 이름 입력 중 Enter를 처리하여 마우스 없이도 화면을 완료할 수 있게 합니다.</summary>
        private void Update()
        {
            if (Keyboard.current == null)
            {
                return;
            }

            bool isEditingName = nameInputField != null && nameInputField.isFocused;
            if (Keyboard.current.tabKey.wasPressedThisFrame)
            {
                MoveToNextControl(Keyboard.current.shiftKey.isPressed ? -1 : 1);
                return;
            }

            if (isEditingName)
            {
                // 이름 입력 중에는 문자와 방향키가 외형 선택 단축키로 전달되지 않게 합니다.
                if (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.numpadEnterKey.wasPressedThisFrame)
                {
                    StartGame();
                }

                return;
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

        /// <summary>선택한 외형과 이름을 확정한 뒤 다음 Path Selection Scene을 한 번만 불러옵니다.</summary>
        private void StartGame()
        {
            if (isStarting)
            {
                return;
            }

            if (!TryValidatePlayerName(nameInputField.text, out string normalizedName, out string errorMessage))
            {
                nameErrorLabel.text = errorMessage;
                EventSystem.current.SetSelectedGameObject(nameInputField.gameObject);
                nameInputField.ActivateInputField();
                return;
            }

            isStarting = true;
            nameInputField.text = normalizedName;
            nameErrorLabel.text = string.Empty;
            GameSessionData.ConfigurePlayer(selectedVisual, normalizedName);
            startButton.interactable = false;
            SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Single);
        }

        /// <summary>
        /// 앞뒤 공백을 제거한 이름이 1~12자의 한글, 영문, 숫자로만 이루어졌는지 확인합니다.
        /// 검증 로직을 별도 메서드로 두어 UI 밖에서도 같은 규칙을 시험할 수 있습니다.
        /// </summary>
        public static bool TryValidatePlayerName(string input, out string normalizedName, out string errorMessage)
        {
            normalizedName = input?.Trim() ?? string.Empty;
            if (normalizedName.Length == 0)
            {
                errorMessage = "캐릭터 이름을 입력해주세요.";
                return false;
            }

            if (normalizedName.Length > MaximumPlayerNameLength)
            {
                errorMessage = "캐릭터 이름은 12자 이하로 입력해주세요.";
                return false;
            }

            if (!Regex.IsMatch(normalizedName, "^[가-힣A-Za-z0-9]+$"))
            {
                errorMessage = "이름은 한글, 영문, 숫자만 사용할 수 있습니다.";
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }

        /// <summary>선택된 버튼은 색상뿐 아니라 굵은 테두리로도 구분합니다.</summary>
        private void SetChoiceAppearance(Image background, Outline outline, bool isSelected)
        {
            background.color = isSelected ? selectedButtonColor : normalButtonColor;
            outline.effectColor = isSelected ? accentColor : mutedBorderColor;
            outline.effectDistance = isSelected ? new Vector2(4f, -4f) : new Vector2(2f, -2f);
        }

        /// <summary>Tab 또는 Shift+Tab으로 Male, Female, 이름 입력, 게임 시작 순서로 이동합니다.</summary>
        private void MoveToNextControl(int direction)
        {
            Selectable[] controls = { maleButton, femaleButton, nameInputField, startButton };
            GameObject current = EventSystem.current.currentSelectedGameObject;
            int index = System.Array.FindIndex(controls, control => control.gameObject == current);
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

        /// <summary>외부 이미지 없이 uGUI 패널을 겹쳐 판타지 RPG 분위기의 Character Creation 화면을 만듭니다.</summary>
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
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;

            CreateLayeredBackground(canvasObject.transform);
            Image contentPanel = CreateImage(canvasObject.transform, "ContentPanel", new Color(0.025f, 0.045f, 0.08f, 0.78f));
            SetRect(contentPanel.rectTransform, new Vector2(0.5f, 0.49f), new Vector2(900f, 650f));
            Outline contentOutline = contentPanel.gameObject.AddComponent<Outline>();
            contentOutline.effectColor = new Color(0.25f, 0.35f, 0.5f, 0.55f);
            contentOutline.effectDistance = new Vector2(2f, -2f);

            Text title = CreateText(canvasObject.transform, "Title", "PROJECT LIMITLESS", font, 44, new Vector2(0.5f, 0.915f), new Vector2(760f, 58f));
            title.color = new Color(0.95f, 0.82f, 0.5f, 1f);
            title.fontStyle = FontStyle.Bold;
            Text subtitle = CreateText(canvasObject.transform, "Subtitle", "캐릭터 생성", font, 25, new Vector2(0.5f, 0.845f), new Vector2(520f, 42f));
            subtitle.color = new Color(0.82f, 0.88f, 0.96f, 1f);
            CreateDivider(canvasObject.transform, new Vector2(0.5f, 0.805f), 460f);

            maleButton = CreateChoiceButton(canvasObject.transform, "MaleButton", "남성", malePreviewSprite, font, new Vector2(0.35f, 0.615f), out maleLabel, out maleBackground, out maleOutline);
            femaleButton = CreateChoiceButton(canvasObject.transform, "FemaleButton", "여성", femalePreviewSprite, font, new Vector2(0.65f, 0.615f), out femaleLabel, out femaleBackground, out femaleOutline);
            maleButton.onClick.AddListener(() => SelectVisual(PlayerVisualType.Male));
            femaleButton.onClick.AddListener(() => SelectVisual(PlayerVisualType.Female));

            selectionLabel = CreateText(canvasObject.transform, "SelectionLabel", "현재 선택: 남성", font, 21, new Vector2(0.5f, 0.405f), new Vector2(500f, 34f));
            selectionLabel.color = new Color(0.76f, 0.84f, 0.94f, 1f);

            Image namePanel = CreateImage(canvasObject.transform, "NamePanel", new Color(0.055f, 0.08f, 0.13f, 0.96f));
            SetRect(namePanel.rectTransform, new Vector2(0.5f, 0.285f), new Vector2(560f, 126f));
            Outline namePanelOutline = namePanel.gameObject.AddComponent<Outline>();
            namePanelOutline.effectColor = new Color(0.28f, 0.38f, 0.52f, 0.9f);
            namePanelOutline.effectDistance = new Vector2(2f, -2f);
            Text nameLabel = CreateText(canvasObject.transform, "NameLabel", "캐릭터 이름", font, 22, new Vector2(0.5f, 0.335f), new Vector2(460f, 34f));
            nameLabel.fontStyle = FontStyle.Bold;
            nameLabel.color = new Color(0.94f, 0.83f, 0.57f, 1f);
            nameInputField = CreateNameInputField(canvasObject.transform, font, new Vector2(0.5f, 0.275f), out nameInputOutline);
            nameErrorLabel = CreateText(canvasObject.transform, "NameError", string.Empty, font, 18, new Vector2(0.5f, 0.225f), new Vector2(700f, 28f));
            nameErrorLabel.color = new Color(1f, 0.65f, 0.62f, 1f);
            nameInputField.onValueChanged.AddListener(_ => nameErrorLabel.text = string.Empty);
            startButton = CreateTextButton(canvasObject.transform, "StartButton", "다음", font, new Vector2(0.5f, 0.125f), new Vector2(380f, 68f), out startButtonOutline);
            startButton.onClick.AddListener(StartGame);
            Text keyboardHelp = CreateText(canvasObject.transform, "KeyboardHelp", "Tab / 방향키 : 이동     Enter / Space : 선택", font, 18, new Vector2(0.5f, 0.04f), new Vector2(760f, 30f));
            keyboardHelp.color = new Color(0.7f, 0.77f, 0.86f, 1f);

            CreateStepIndicator(canvasObject.transform, font, 1);

            ConfigureNavigation();
            ConfigureFocusFeedback();
        }

        /// <summary>전체 4단계 중 현재 화면이 1단계임을 텍스트와 테두리로 함께 표시합니다.</summary>
        private static void CreateStepIndicator(Transform parent, Font font, int currentStep)
        {
            string[] steps = { "1 기본 정보", "2 길", "3 직업", "4 확인" };
            for (int index = 0; index < steps.Length; index++)
            {
                bool isCurrent = index + 1 == currentStep;
                Image stepPanel = CreateImage(parent, $"Step{index + 1}", isCurrent
                    ? new Color(0.18f, 0.25f, 0.34f, 0.98f)
                    : new Color(0.04f, 0.065f, 0.11f, 0.88f));
                SetRect(stepPanel.rectTransform, new Vector2(0.35f + index * 0.1f, 0.975f), new Vector2(122f, 28f));
                Outline outline = stepPanel.gameObject.AddComponent<Outline>();
                outline.effectColor = isCurrent ? new Color(0.95f, 0.76f, 0.36f, 1f) : new Color(0.25f, 0.32f, 0.42f, 1f);
                outline.effectDistance = isCurrent ? new Vector2(2f, -2f) : Vector2.one;
                Text label = CreateText(stepPanel.transform, "Label", isCurrent ? $"{steps[index]} · 현재" : steps[index], font, 15, Vector2.one * 0.5f, new Vector2(118f, 26f));
                label.color = isCurrent ? new Color(1f, 0.88f, 0.58f, 1f) : new Color(0.63f, 0.7f, 0.79f, 1f);
            }
        }

        /// <summary>방향키를 누를 때 두 외형과 시작 버튼 사이를 예측 가능한 순서로 이동하게 합니다.</summary>
        private void ConfigureNavigation()
        {
            Navigation maleNavigation = new Navigation { mode = Navigation.Mode.Explicit, selectOnRight = femaleButton, selectOnDown = nameInputField };
            Navigation femaleNavigation = new Navigation { mode = Navigation.Mode.Explicit, selectOnLeft = maleButton, selectOnDown = nameInputField };
            Navigation nameNavigation = new Navigation { mode = Navigation.Mode.Explicit, selectOnUp = maleButton, selectOnDown = startButton };
            Navigation startNavigation = new Navigation { mode = Navigation.Mode.Explicit, selectOnUp = nameInputField };
            maleButton.navigation = maleNavigation;
            femaleButton.navigation = femaleNavigation;
            nameInputField.navigation = nameNavigation;
            startButton.navigation = startNavigation;
        }

        /// <summary>키보드 Focus를 선택 상태와 별도로 밝은 금색 테두리로 보여 줍니다.</summary>
        private void ConfigureFocusFeedback()
        {
            AddFocusFeedback(maleButton.gameObject, maleOutline, () => SetChoiceAppearance(maleBackground, maleOutline, selectedVisual == PlayerVisualType.Male));
            AddFocusFeedback(femaleButton.gameObject, femaleOutline, () => SetChoiceAppearance(femaleBackground, femaleOutline, selectedVisual == PlayerVisualType.Female));
            AddFocusFeedback(nameInputField.gameObject, nameInputOutline, () =>
            {
                nameInputOutline.effectColor = new Color(0.36f, 0.52f, 0.7f, 1f);
                nameInputOutline.effectDistance = new Vector2(2f, -2f);
            });
            AddFocusFeedback(startButton.gameObject, startButtonOutline, () =>
            {
                startButtonOutline.effectColor = accentColor;
                startButtonOutline.effectDistance = new Vector2(2f, -2f);
            });
        }

        /// <summary>Selectable이 Focus를 얻고 잃을 때 기존 클릭 동작과 무관한 테두리 변화만 추가합니다.</summary>
        private void AddFocusFeedback(GameObject target, Outline outline, System.Action restoreAppearance)
        {
            EventTrigger trigger = target.GetComponent<EventTrigger>() ?? target.AddComponent<EventTrigger>();
            AddEventTrigger(trigger, EventTriggerType.Select, _ =>
            {
                outline.effectColor = focusColor;
                outline.effectDistance = new Vector2(4f, -4f);
            });
            AddEventTrigger(trigger, EventTriggerType.Deselect, _ => restoreAppearance());
        }

        /// <summary>EventTrigger에 한 종류의 UI 이벤트와 실행할 동작을 연결합니다.</summary>
        private static void AddEventTrigger(EventTrigger trigger, EventTriggerType eventType, System.Action<BaseEventData> callback)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry { eventID = eventType };
            entry.callback.AddListener(eventData => callback(eventData));
            trigger.triggers.Add(entry);
        }

        private static Button CreateChoiceButton(Transform parent, string name, string label, Sprite previewSprite, Font font, Vector2 anchor, out Text labelText, out Image background, out Outline outline)
        {
            GameObject buttonObject = new GameObject(name, typeof(Image), typeof(Button), typeof(Outline));
            buttonObject.transform.SetParent(parent, false);
            SetRect(buttonObject.GetComponent<RectTransform>(), anchor, new Vector2(300f, 270f));
            background = buttonObject.GetComponent<Image>();
            Button button = buttonObject.GetComponent<Button>();
            button.targetGraphic = background;
            button.colors = CreateSelectableColors(new Color(0.11f, 0.16f, 0.24f, 1f), new Color(0.18f, 0.25f, 0.35f, 1f));
            outline = buttonObject.GetComponent<Outline>();

            Image previewBackground = CreateImage(buttonObject.transform, "PreviewBackground", new Color(0.035f, 0.06f, 0.1f, 0.85f));
            SetRect(previewBackground.rectTransform, new Vector2(0.5f, 0.61f), new Vector2(212f, 202f));
            Outline previewOutline = previewBackground.gameObject.AddComponent<Outline>();
            previewOutline.effectColor = new Color(0.26f, 0.34f, 0.46f, 0.75f);
            previewOutline.effectDistance = new Vector2(1f, -1f);
            Image preview = CreateImage(buttonObject.transform, "Preview", Color.white);
            preview.sprite = previewSprite;
            preview.preserveAspect = true;
            SetRect(preview.rectTransform, new Vector2(0.5f, 0.62f), new Vector2(194f, 194f));
            Image divider = CreateImage(buttonObject.transform, "CardDivider", new Color(0.72f, 0.57f, 0.27f, 0.8f));
            SetRect(divider.rectTransform, new Vector2(0.5f, 0.245f), new Vector2(220f, 2f));
            labelText = CreateText(buttonObject.transform, "Label", label, font, 25, new Vector2(0.5f, 0.115f), new Vector2(270f, 58f));
            labelText.fontStyle = FontStyle.Bold;
            return button;
        }

        /// <summary>LegacyRuntime 글꼴과 한글 IME를 사용하는 한 줄 이름 입력란을 만듭니다.</summary>
        private static InputField CreateNameInputField(Transform parent, Font font, Vector2 anchor, out Outline outline)
        {
            GameObject inputObject = new GameObject("NameInputField", typeof(Image), typeof(InputField), typeof(Outline));
            inputObject.transform.SetParent(parent, false);
            SetRect(inputObject.GetComponent<RectTransform>(), anchor, new Vector2(500f, 54f));
            Image background = inputObject.GetComponent<Image>();
            background.color = new Color(0.025f, 0.045f, 0.08f, 1f);
            outline = inputObject.GetComponent<Outline>();
            outline.effectColor = new Color(0.36f, 0.52f, 0.7f, 1f);
            outline.effectDistance = new Vector2(2f, -2f);

            Text inputText = CreateText(inputObject.transform, "Text", string.Empty, font, 23, Vector2.one * 0.5f, new Vector2(454f, 44f));
            inputText.alignment = TextAnchor.MiddleLeft;
            Text placeholder = CreateText(inputObject.transform, "Placeholder", "이름을 입력하세요", font, 22, Vector2.one * 0.5f, new Vector2(454f, 44f));
            placeholder.alignment = TextAnchor.MiddleLeft;
            placeholder.color = new Color(0.62f, 0.69f, 0.78f, 1f);

            InputField inputField = inputObject.GetComponent<InputField>();
            inputField.targetGraphic = background;
            inputField.colors = CreateSelectableColors(new Color(0.05f, 0.08f, 0.13f, 1f), new Color(0.1f, 0.15f, 0.22f, 1f));
            inputField.textComponent = inputText;
            inputField.placeholder = placeholder;
            inputField.characterLimit = MaximumPlayerNameLength;
            inputField.lineType = InputField.LineType.SingleLine;
            inputField.contentType = InputField.ContentType.Standard;
            return inputField;
        }

        private Button CreateTextButton(Transform parent, string name, string label, Font font, Vector2 anchor, Vector2 size, out Outline outline)
        {
            GameObject buttonObject = new GameObject(name, typeof(Image), typeof(Button), typeof(Outline));
            buttonObject.transform.SetParent(parent, false);
            SetRect(buttonObject.GetComponent<RectTransform>(), anchor, size);
            Image image = buttonObject.GetComponent<Image>();
            image.color = new Color(0.12f, 0.32f, 0.5f, 1f);
            Button button = buttonObject.GetComponent<Button>();
            button.targetGraphic = image;
            button.colors = CreateSelectableColors(new Color(0.16f, 0.42f, 0.62f, 1f), new Color(0.22f, 0.5f, 0.7f, 1f));
            outline = buttonObject.GetComponent<Outline>();
            outline.effectColor = accentColor;
            outline.effectDistance = new Vector2(2f, -2f);
            Text buttonLabel = CreateText(buttonObject.transform, "Label", label, font, 29, Vector2.one * 0.5f, size - new Vector2(20f, 12f));
            buttonLabel.fontStyle = FontStyle.Bold;
            return button;
        }

        /// <summary>마우스 Hover와 키보드 Focus가 기본 상태와 충분히 다르게 보이는 공통 색 변화를 만듭니다.</summary>
        private static ColorBlock CreateSelectableColors(Color highlighted, Color selected)
        {
            ColorBlock colors = ColorBlock.defaultColorBlock;
            colors.normalColor = Color.white;
            colors.highlightedColor = highlighted;
            colors.selectedColor = selected;
            colors.pressedColor = new Color(0.72f, 0.72f, 0.72f, 1f);
            colors.disabledColor = new Color(0.45f, 0.45f, 0.45f, 0.7f);
            colors.colorMultiplier = 1f;
            colors.fadeDuration = 0.12f;
            return colors;
        }

        /// <summary>
        /// 단색 Image 여러 장을 위아래로 겹쳐 외부 이미지 없이 짙은 네이비 그라데이션과 약한 비네트 느낌을 냅니다.
        /// 가장자리 장식은 반투명이라 중앙 UI와 글자의 대비를 해치지 않습니다.
        /// </summary>
        private static void CreateLayeredBackground(Transform parent)
        {
            Image baseLayer = CreateImage(parent, "BackgroundBase", new Color(0.018f, 0.03f, 0.06f, 1f));
            Stretch(baseLayer.rectTransform);

            Image upperBlue = CreateImage(parent, "BackgroundUpperBlue", new Color(0.035f, 0.075f, 0.13f, 0.9f));
            SetArea(upperBlue.rectTransform, new Vector2(0f, 0.54f), Vector2.one);
            Image middleBlue = CreateImage(parent, "BackgroundMiddleBlue", new Color(0.025f, 0.055f, 0.1f, 0.72f));
            SetArea(middleBlue.rectTransform, new Vector2(0f, 0.28f), new Vector2(1f, 0.68f));
            Image lowerShade = CreateImage(parent, "BackgroundLowerShade", new Color(0.008f, 0.018f, 0.04f, 0.78f));
            SetArea(lowerShade.rectTransform, Vector2.zero, new Vector2(1f, 0.34f));

            Image topVignette = CreateImage(parent, "VignetteTop", new Color(0f, 0.005f, 0.02f, 0.42f));
            SetArea(topVignette.rectTransform, new Vector2(0f, 0.9f), Vector2.one);
            Image bottomVignette = CreateImage(parent, "VignetteBottom", new Color(0f, 0f, 0.01f, 0.58f));
            SetArea(bottomVignette.rectTransform, Vector2.zero, new Vector2(1f, 0.1f));
            Image leftVignette = CreateImage(parent, "VignetteLeft", new Color(0f, 0.005f, 0.02f, 0.32f));
            SetArea(leftVignette.rectTransform, Vector2.zero, new Vector2(0.08f, 1f));
            Image rightVignette = CreateImage(parent, "VignetteRight", new Color(0f, 0.005f, 0.02f, 0.32f));
            SetArea(rightVignette.rectTransform, new Vector2(0.92f, 0f), Vector2.one);
        }

        /// <summary>제목 아래에 금색 중앙선과 양쪽 푸른 보조선을 배치합니다.</summary>
        private static void CreateDivider(Transform parent, Vector2 anchor, float width)
        {
            Image line = CreateImage(parent, "TitleDivider", new Color(0.76f, 0.61f, 0.3f, 0.9f));
            SetRect(line.rectTransform, anchor, new Vector2(width, 2f));
            Image centerMark = CreateImage(parent, "TitleDividerMark", new Color(0.98f, 0.83f, 0.48f, 1f));
            SetRect(centerMark.rectTransform, anchor, new Vector2(54f, 4f));
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

        /// <summary>부모 화면의 정규화된 두 지점 사이를 Image가 채우게 합니다.</summary>
        private static void SetArea(RectTransform rectTransform, Vector2 anchorMin, Vector2 anchorMax)
        {
            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
        }
    }
}

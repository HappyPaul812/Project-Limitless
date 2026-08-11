using System.Collections.Generic;
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
    /// 캐릭터 생성을 여러 Scene으로 나눈 두 번째 단계입니다.
    /// 한 화면에 너무 많은 결정을 몰아넣지 않고, GameSessionData를 통해 이전 단계의 외형·이름과 현재 Path를 유지합니다.
    /// </summary>
    public sealed class PathSelectionController : MonoBehaviour
    {
        private const string PreviousSceneName = "CharacterCreation";
        private const string FutureJobSceneName = "JobSelection";

        [SerializeField] private Sprite malePreviewSprite;
        [SerializeField] private Sprite femalePreviewSprite;
        [SerializeField] private string previousSceneName = PreviousSceneName;
        [SerializeField] private string nextSceneName = FutureJobSceneName;
        [SerializeField] private PlayerPathDefinition[] pathDefinitions;

        private readonly List<Button> pathButtons = new List<Button>();
        private readonly List<Text> pathLabels = new List<Text>();
        private readonly List<Image> pathBackgrounds = new List<Image>();
        private readonly List<Outline> pathOutlines = new List<Outline>();
        private readonly List<Selectable> tabControls = new List<Selectable>();

        private readonly Color normalCardColor = new Color(0.075f, 0.105f, 0.17f, 0.97f);
        private readonly Color selectedCardColor = new Color(0.13f, 0.2f, 0.3f, 1f);
        private readonly Color accentColor = new Color(0.88f, 0.7f, 0.32f, 1f);
        private readonly Color focusColor = new Color(1f, 0.86f, 0.48f, 1f);
        private readonly Color mutedBorderColor = new Color(0.32f, 0.4f, 0.52f, 1f);

        private Button previousButton;
        private Button nextButton;
        private Text developmentInfoLabel;
        private PlayerPathType selectedPath = PlayerPathType.None;

        /// <summary>Editor 생성 도구가 캐릭터 요약용 Sprite와 앞뒤 Scene 이름을 연결합니다.</summary>
        public void Configure(Sprite maleSprite, Sprite femaleSprite, string previousScene, string futureNextScene)
        {
            malePreviewSprite = maleSprite;
            femalePreviewSprite = femaleSprite;
            previousSceneName = previousScene;
            nextSceneName = futureNextScene;
        }

        private void Awake()
        {
            EnsureDefaultPathDefinitions();
            CreateEventSystem();
            CreateInterface();
            selectedPath = GameSessionData.SelectedPlayerPath;
            RefreshPathCards();
            EventSystem.current.SetSelectedGameObject(GetInitialFocusButton().gameObject);
        }

        /// <summary>Tab, Shift+Tab, Space를 기존 CharacterCreation과 같은 방식으로 지원합니다.</summary>
        private void Update()
        {
            if (Keyboard.current == null)
            {
                return;
            }

            if (Keyboard.current.tabKey.wasPressedThisFrame)
            {
                MoveToNextControl(Keyboard.current.shiftKey.isPressed ? -1 : 1);
                return;
            }

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                EventSystem.current.currentSelectedGameObject?.GetComponent<Button>()?.onClick.Invoke();
            }
        }

        /// <summary>Prototype 카드가 비어 있으면 임시 Path01~Path04 데이터를 준비합니다.</summary>
        private void EnsureDefaultPathDefinitions()
        {
            if (pathDefinitions != null && pathDefinitions.Length > 0)
            {
                return;
            }

            pathDefinitions = new[]
            {
                new PlayerPathDefinition(PlayerPathType.Path01, "Path 01"),
                new PlayerPathDefinition(PlayerPathType.Path02, "Path 02"),
                new PlayerPathDefinition(PlayerPathType.Path03, "Path 03"),
                new PlayerPathDefinition(PlayerPathType.Path04, "Path 04"),
            };
        }

        /// <summary>카드를 선택하면 개발용 식별자만 세션에 저장하고 모든 카드의 선택 표시를 갱신합니다.</summary>
        private void SelectPath(PlayerPathType pathType)
        {
            selectedPath = pathType;
            GameSessionData.SelectPlayerPath(pathType);
            developmentInfoLabel.text = string.Empty;
            RefreshPathCards();
        }

        private void RefreshPathCards()
        {
            for (int index = 0; index < pathDefinitions.Length; index++)
            {
                bool isSelected = pathDefinitions[index].PathType == selectedPath;
                pathBackgrounds[index].color = isSelected ? selectedCardColor : normalCardColor;
                pathOutlines[index].effectColor = isSelected ? accentColor : mutedBorderColor;
                pathOutlines[index].effectDistance = isSelected ? new Vector2(4f, -4f) : new Vector2(2f, -2f);
                pathLabels[index].text = isSelected
                    ? $"{pathDefinitions[index].DisplayName}\n✓ 선택됨\n{pathDefinitions[index].ShortDescription}"
                    : $"{pathDefinitions[index].DisplayName}\n{pathDefinitions[index].ShortDescription}";
            }
        }

        /// <summary>이전 단계로만 이동하며 GameSessionData는 초기화하지 않아 외형과 이름이 복원됩니다.</summary>
        private void ReturnToCharacterCreation()
        {
            SceneManager.LoadSceneAsync(previousSceneName, LoadSceneMode.Single);
        }

        /// <summary>JobSelection은 아직 없으므로 Scene을 열지 않고 다음 구현 단계 안내만 보여 줍니다.</summary>
        private void ShowFutureStepNotice()
        {
            developmentInfoLabel.text = selectedPath == PlayerPathType.None
                ? "먼저 길 카드를 선택해주세요."
                : "직업 선택 화면은 다음 단계에서 구현됩니다.";
        }

        private void CreateInterface()
        {
            Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            GameObject canvasObject = new GameObject("PathSelectionCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasObject.transform.SetParent(transform, false);
            Canvas canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1280f, 720f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;

            CreateLayeredBackground(canvasObject.transform);
            Image contentPanel = CreateImage(canvasObject.transform, "ContentPanel", new Color(0.025f, 0.045f, 0.08f, 0.8f));
            SetRect(contentPanel.rectTransform, new Vector2(0.5f, 0.49f), new Vector2(1080f, 650f));
            AddOutline(contentPanel.gameObject, new Color(0.25f, 0.35f, 0.5f, 0.55f), 2f);

            Text title = CreateText(canvasObject.transform, "Title", "PROJECT LIMITLESS", font, 42, new Vector2(0.5f, 0.915f), new Vector2(720f, 56f));
            title.color = new Color(0.95f, 0.82f, 0.5f, 1f);
            title.fontStyle = FontStyle.Bold;
            Text subtitle = CreateText(canvasObject.transform, "Subtitle", "당신의 길을 선택하세요", font, 24, new Vector2(0.5f, 0.845f), new Vector2(560f, 40f));
            subtitle.color = new Color(0.82f, 0.88f, 0.96f, 1f);
            CreateStepIndicator(canvasObject.transform, font);

            CreateCharacterSummary(canvasObject.transform, font);
            CreatePathCards(canvasObject.transform, font);
            CreateBottomControls(canvasObject.transform, font);
            ConfigureNavigation();
        }

        /// <summary>이전 단계에서 선택한 이름, 외형 문구, Down Idle Sprite를 요약해 보여 줍니다.</summary>
        private void CreateCharacterSummary(Transform parent, Font font)
        {
            Image panel = CreateImage(parent, "CharacterSummary", new Color(0.055f, 0.08f, 0.13f, 0.97f));
            SetRect(panel.rectTransform, new Vector2(0.19f, 0.5f), new Vector2(250f, 400f));
            AddOutline(panel.gameObject, new Color(0.38f, 0.48f, 0.62f, 0.9f), 2f);
            Text heading = CreateText(panel.transform, "Heading", "캐릭터", font, 22, new Vector2(0.5f, 0.91f), new Vector2(210f, 36f));
            heading.color = new Color(0.94f, 0.83f, 0.57f, 1f);
            heading.fontStyle = FontStyle.Bold;

            Image previewFrame = CreateImage(panel.transform, "PreviewFrame", new Color(0.025f, 0.045f, 0.08f, 1f));
            SetRect(previewFrame.rectTransform, new Vector2(0.5f, 0.64f), new Vector2(190f, 190f));
            AddOutline(previewFrame.gameObject, new Color(0.28f, 0.38f, 0.5f, 0.85f), 1f);
            Image preview = CreateImage(panel.transform, "Preview", Color.white);
            preview.sprite = GameSessionData.SelectedPlayerVisual == PlayerVisualType.Female ? femalePreviewSprite : malePreviewSprite;
            preview.preserveAspect = true;
            SetRect(preview.rectTransform, new Vector2(0.5f, 0.64f), new Vector2(178f, 178f));

            string playerName = string.IsNullOrWhiteSpace(GameSessionData.PlayerName) ? "이름 미설정" : GameSessionData.PlayerName;
            string visualName = GameSessionData.SelectedPlayerVisual == PlayerVisualType.Female ? "여성" : "남성";
            Text summary = CreateText(panel.transform, "SummaryText", $"{playerName}\n{visualName}", font, 23, new Vector2(0.5f, 0.22f), new Vector2(210f, 100f));
            summary.fontStyle = FontStyle.Bold;
        }

        /// <summary>정의 배열을 순회해 2×2 Prototype 카드를 만듭니다. 배열 항목을 바꾸면 카드 수도 함께 바뀝니다.</summary>
        private void CreatePathCards(Transform parent, Font font)
        {
            for (int index = 0; index < pathDefinitions.Length; index++)
            {
                int capturedIndex = index;
                int column = index % 2;
                int row = index / 2;
                // 배열 항목 수가 바뀌어도 두 열 Grid로 자동 배치되므로 카드 생성 코드를 다시 작성할 필요가 없습니다.
                Vector2 anchor = new Vector2(0.48f + column * 0.25f, 0.64f - row * 0.25f);
                GameObject cardObject = new GameObject($"PathCard{index + 1}", typeof(Image), typeof(Button), typeof(Outline));
                cardObject.transform.SetParent(parent, false);
                SetRect(cardObject.GetComponent<RectTransform>(), anchor, new Vector2(270f, 150f));
                Image background = cardObject.GetComponent<Image>();
                Button button = cardObject.GetComponent<Button>();
                button.targetGraphic = background;
                button.colors = CreateSelectableColors();
                Outline outline = cardObject.GetComponent<Outline>();
                Text label = CreateText(cardObject.transform, "Label", string.Empty, font, 21, Vector2.one * 0.5f, new Vector2(240f, 120f));
                label.fontStyle = FontStyle.Bold;
                button.onClick.AddListener(() => SelectPath(pathDefinitions[capturedIndex].PathType));
                AddFocusFeedback(cardObject, outline, () => RefreshPathCards());

                pathButtons.Add(button);
                pathLabels.Add(label);
                pathBackgrounds.Add(background);
                pathOutlines.Add(outline);
                tabControls.Add(button);
            }
        }

        private void CreateBottomControls(Transform parent, Font font)
        {
            previousButton = CreateTextButton(parent, "PreviousButton", "이전", font, new Vector2(0.43f, 0.14f));
            nextButton = CreateTextButton(parent, "NextButton", "다음", font, new Vector2(0.69f, 0.14f));
            previousButton.onClick.AddListener(ReturnToCharacterCreation);
            nextButton.onClick.AddListener(ShowFutureStepNotice);
            AddFocusFeedback(previousButton.gameObject, previousButton.GetComponent<Outline>(), () => RestoreBottomButtonOutline(previousButton));
            AddFocusFeedback(nextButton.gameObject, nextButton.GetComponent<Outline>(), () => RestoreBottomButtonOutline(nextButton));
            tabControls.Add(previousButton);
            tabControls.Add(nextButton);

            developmentInfoLabel = CreateText(parent, "DevelopmentInfo", "", font, 18, new Vector2(0.56f, 0.065f), new Vector2(700f, 30f));
            developmentInfoLabel.color = new Color(1f, 0.77f, 0.5f, 1f);
            Text help = CreateText(parent, "KeyboardHelp", "Tab / 방향키 : 이동     Enter / Space : 선택", font, 17, new Vector2(0.56f, 0.025f), new Vector2(720f, 26f));
            help.color = new Color(0.7f, 0.77f, 0.86f, 1f);
        }

        private void RestoreBottomButtonOutline(Button button)
        {
            Outline outline = button.GetComponent<Outline>();
            outline.effectColor = accentColor;
            outline.effectDistance = new Vector2(2f, -2f);
        }

        private void ConfigureNavigation()
        {
            for (int index = 0; index < pathButtons.Count; index++)
            {
                bool isLeftColumn = index % 2 == 0;
                Selectable left = !isLeftColumn ? pathButtons[index - 1] : null;
                Selectable right = isLeftColumn && index + 1 < pathButtons.Count ? pathButtons[index + 1] : null;
                Selectable up = index >= 2 ? pathButtons[index - 2] : null;
                Selectable down = index + 2 < pathButtons.Count
                    ? pathButtons[index + 2]
                    : (isLeftColumn ? previousButton : nextButton);
                pathButtons[index].navigation = ExplicitNavigation(left, right, up, down);
            }

            int lastLeftIndex = pathButtons.Count % 2 == 0 ? pathButtons.Count - 2 : pathButtons.Count - 1;
            int lastRightIndex = pathButtons.Count - 1;
            previousButton.navigation = ExplicitNavigation(null, nextButton, pathButtons[lastLeftIndex], null);
            nextButton.navigation = ExplicitNavigation(previousButton, null, pathButtons[lastRightIndex], null);
        }

        private static Navigation ExplicitNavigation(Selectable left, Selectable right, Selectable up, Selectable down)
        {
            return new Navigation { mode = Navigation.Mode.Explicit, selectOnLeft = left, selectOnRight = right, selectOnUp = up, selectOnDown = down };
        }

        private Button GetInitialFocusButton()
        {
            int selectedIndex = System.Array.FindIndex(pathDefinitions, definition => definition.PathType == selectedPath);
            return selectedIndex >= 0 ? pathButtons[selectedIndex] : pathButtons[0];
        }

        private void MoveToNextControl(int direction)
        {
            GameObject current = EventSystem.current.currentSelectedGameObject;
            int index = tabControls.FindIndex(control => control.gameObject == current);
            int nextIndex = (index + direction + tabControls.Count) % tabControls.Count;
            EventSystem.current.SetSelectedGameObject(tabControls[nextIndex].gameObject);
        }

        private void AddFocusFeedback(GameObject target, Outline outline, System.Action restoreAppearance)
        {
            EventTrigger trigger = target.AddComponent<EventTrigger>();
            AddEventTrigger(trigger, EventTriggerType.Select, _ =>
            {
                outline.effectColor = focusColor;
                outline.effectDistance = new Vector2(4f, -4f);
            });
            AddEventTrigger(trigger, EventTriggerType.Deselect, _ => restoreAppearance());
        }

        private static void AddEventTrigger(EventTrigger trigger, EventTriggerType type, System.Action<BaseEventData> callback)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry { eventID = type };
            entry.callback.AddListener(eventData => callback(eventData));
            trigger.triggers.Add(entry);
        }

        private static void CreateEventSystem()
        {
            if (EventSystem.current != null)
            {
                return;
            }

            GameObject eventSystemObject = new GameObject("EventSystem", typeof(EventSystem));
            InputSystemUIInputModule module = eventSystemObject.AddComponent<InputSystemUIInputModule>();
            module.AssignDefaultActions();
        }

        private static Button CreateTextButton(Transform parent, string name, string label, Font font, Vector2 anchor)
        {
            GameObject buttonObject = new GameObject(name, typeof(Image), typeof(Button), typeof(Outline));
            buttonObject.transform.SetParent(parent, false);
            SetRect(buttonObject.GetComponent<RectTransform>(), anchor, new Vector2(250f, 58f));
            Image image = buttonObject.GetComponent<Image>();
            image.color = new Color(0.1f, 0.28f, 0.44f, 1f);
            Button button = buttonObject.GetComponent<Button>();
            button.targetGraphic = image;
            button.colors = CreateSelectableColors();
            Outline outline = buttonObject.GetComponent<Outline>();
            outline.effectColor = new Color(0.88f, 0.7f, 0.32f, 1f);
            outline.effectDistance = new Vector2(2f, -2f);
            Text text = CreateText(buttonObject.transform, "Label", label, font, 25, Vector2.one * 0.5f, new Vector2(220f, 48f));
            text.fontStyle = FontStyle.Bold;
            return button;
        }

        private static ColorBlock CreateSelectableColors()
        {
            ColorBlock colors = ColorBlock.defaultColorBlock;
            colors.normalColor = Color.white;
            colors.highlightedColor = new Color(0.82f, 0.88f, 0.98f, 1f);
            colors.selectedColor = new Color(0.92f, 0.82f, 0.58f, 1f);
            colors.pressedColor = new Color(0.68f, 0.68f, 0.68f, 1f);
            colors.fadeDuration = 0.12f;
            return colors;
        }

        private static void CreateStepIndicator(Transform parent, Font font)
        {
            string[] steps = { "1 기본 정보", "2 길", "3 직업", "4 확인" };
            for (int index = 0; index < steps.Length; index++)
            {
                bool isCurrent = index == 1;
                Image panel = CreateImage(parent, $"Step{index + 1}", isCurrent ? new Color(0.18f, 0.25f, 0.34f, 0.98f) : new Color(0.04f, 0.065f, 0.11f, 0.88f));
                SetRect(panel.rectTransform, new Vector2(0.35f + index * 0.1f, 0.975f), new Vector2(122f, 28f));
                AddOutline(panel.gameObject, isCurrent ? new Color(0.95f, 0.76f, 0.36f, 1f) : new Color(0.25f, 0.32f, 0.42f, 1f), isCurrent ? 2f : 1f);
                Text text = CreateText(panel.transform, "Label", isCurrent ? $"{steps[index]} · 현재" : steps[index], font, 15, Vector2.one * 0.5f, new Vector2(118f, 26f));
                text.color = isCurrent ? new Color(1f, 0.88f, 0.58f, 1f) : new Color(0.63f, 0.7f, 0.79f, 1f);
            }
        }

        private static void CreateLayeredBackground(Transform parent)
        {
            Image baseLayer = CreateImage(parent, "BackgroundBase", new Color(0.018f, 0.03f, 0.06f, 1f));
            Stretch(baseLayer.rectTransform);
            Image upperLayer = CreateImage(parent, "BackgroundUpperBlue", new Color(0.035f, 0.075f, 0.13f, 0.9f));
            SetArea(upperLayer.rectTransform, new Vector2(0f, 0.52f), Vector2.one);
            Image lowerLayer = CreateImage(parent, "BackgroundLowerShade", new Color(0.008f, 0.018f, 0.04f, 0.82f));
            SetArea(lowerLayer.rectTransform, Vector2.zero, new Vector2(1f, 0.36f));
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
            text.raycastTarget = false;
            SetRect(text.rectTransform, anchor, size);
            return text;
        }

        private static Image CreateImage(Transform parent, string name, Color color)
        {
            GameObject imageObject = new GameObject(name, typeof(Image));
            imageObject.transform.SetParent(parent, false);
            Image image = imageObject.GetComponent<Image>();
            image.color = color;
            image.raycastTarget = false;
            return image;
        }

        private static Outline AddOutline(GameObject target, Color color, float distance)
        {
            Outline outline = target.AddComponent<Outline>();
            outline.effectColor = color;
            outline.effectDistance = new Vector2(distance, -distance);
            return outline;
        }

        private static void SetRect(RectTransform rect, Vector2 anchor, Vector2 size)
        {
            rect.anchorMin = anchor;
            rect.anchorMax = anchor;
            rect.pivot = Vector2.one * 0.5f;
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = size;
        }

        private static void Stretch(RectTransform rect)
        {
            SetArea(rect, Vector2.zero, Vector2.one);
        }

        private static void SetArea(RectTransform rect, Vector2 min, Vector2 max)
        {
            rect.anchorMin = min;
            rect.anchorMax = max;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }
    }
}

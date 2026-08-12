using System;
using System.Collections.Generic;
using System.Linq;
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
    /// <summary>캐릭터 생성 2단계에서 데이터 에셋을 읽어 길 목록과 상세 정보를 만듭니다.</summary>
    public sealed class PathSelectionController : MonoBehaviour
    {
        private const string ResourcePath = "PathDefinitions";
        [SerializeField] private Sprite malePreviewSprite;
        [SerializeField] private Sprite femalePreviewSprite;
        [SerializeField] private string previousSceneName = "CharacterCreation";
        [SerializeField] private string nextSceneName = "JobSelection";
        [SerializeField] private PlayerPathDefinition[] pathDefinitions;

        private readonly List<Button> pathButtons = new List<Button>();
        private readonly List<Text> pathLabels = new List<Text>();
        private readonly List<Image> pathBackgrounds = new List<Image>();
        private readonly List<Outline> pathOutlines = new List<Outline>();
        private readonly List<Selectable> tabControls = new List<Selectable>();
        private readonly Color normalColor = new Color(0.075f, 0.105f, 0.17f, 0.97f);
        private readonly Color selectedColor = new Color(0.13f, 0.2f, 0.3f, 1f);
        private readonly Color accentColor = new Color(0.88f, 0.7f, 0.32f, 1f);
        private readonly Color focusColor = new Color(1f, 0.86f, 0.48f, 1f);
        private readonly Color mutedColor = new Color(0.32f, 0.4f, 0.52f, 1f);

        private Button previousButton;
        private Button nextButton;
        private Text detailName;
        private Text detailDescription;
        private Text detailStats;
        private Text detailPassive;
        private Text detailKeywords;
        private Text noticeLabel;
        private string selectedPathId = string.Empty;

        public void Configure(Sprite maleSprite, Sprite femaleSprite, string previousScene, string futureNextScene)
        {
            malePreviewSprite = maleSprite;
            femalePreviewSprite = femaleSprite;
            previousSceneName = previousScene;
            nextSceneName = futureNextScene;
        }

        private void Awake()
        {
            LoadDefinitions();
            CreateEventSystem();
            CreateInterface();
            selectedPathId = GameSessionData.SelectedPlayerPathId;
            if (!pathDefinitions.Any(definition => definition.Id == selectedPathId)) selectedPathId = string.Empty;
            RefreshSelection();
            if (pathButtons.Count > 0) EventSystem.current.SetSelectedGameObject(GetInitialFocusButton().gameObject);
        }

        private void Update()
        {
            if (Keyboard.current == null || tabControls.Count == 0) return;
            if (Keyboard.current.tabKey.wasPressedThisFrame)
            {
                MoveToNextControl(Keyboard.current.shiftKey.isPressed ? -1 : 1);
                return;
            }
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
                EventSystem.current.currentSelectedGameObject?.GetComponent<Button>()?.onClick.Invoke();
        }

        /// <summary>Resources 폴더에 에셋 하나를 추가하는 것만으로 목록이 확장되며 ID 순서로 안정적으로 표시됩니다.</summary>
        private void LoadDefinitions()
        {
            if (pathDefinitions == null || pathDefinitions.Length == 0)
                pathDefinitions = Resources.LoadAll<PlayerPathDefinition>(ResourcePath);
            pathDefinitions = pathDefinitions.Where(item => item != null && !string.IsNullOrWhiteSpace(item.Id)).OrderBy(item => item.Id).ToArray();
            if (pathDefinitions.Length == 0) Debug.LogError($"길 데이터가 없습니다. Resources/{ResourcePath}를 확인해주세요.");
        }

        private void SelectPath(PlayerPathDefinition definition)
        {
            selectedPathId = definition.Id;
            GameSessionData.SelectPlayerPath(selectedPathId);
            noticeLabel.text = string.Empty;
            RefreshSelection();
        }

        private void RefreshSelection()
        {
            for (int i = 0; i < pathDefinitions.Length; i++)
            {
                bool selected = pathDefinitions[i].Id == selectedPathId;
                pathBackgrounds[i].color = selected ? selectedColor : normalColor;
                pathOutlines[i].effectColor = selected ? accentColor : mutedColor;
                pathOutlines[i].effectDistance = selected ? new Vector2(4f, -4f) : new Vector2(2f, -2f);
                pathLabels[i].text = selected ? $"{pathDefinitions[i].DisplayName}   ✓ 선택됨\n{FormatBonuses(pathDefinitions[i])}" : $"{pathDefinitions[i].DisplayName}\n{FormatBonuses(pathDefinitions[i])}";
            }

            PlayerPathDefinition definition = pathDefinitions.FirstOrDefault(item => item.Id == selectedPathId) ?? pathDefinitions.FirstOrDefault();
            if (definition == null) return;
            detailName.text = definition.DisplayName;
            detailDescription.text = definition.ShortDescription;
            detailStats.text = "능력치 미리보기\n" + FormatStatPreview(definition);
            detailPassive.text = $"고유 능력 · {definition.PassiveName}\n{definition.PassiveDescription}";
            detailKeywords.text = "키워드  " + string.Join(" / ", definition.Keywords);
        }

        private static string FormatBonuses(PlayerPathDefinition definition) => string.Join("   ", definition.StatBonuses.Select(b => $"{StatName(b.Stat)} +{b.Amount}"));
        private static string FormatStatPreview(PlayerPathDefinition d) => $"체력 {d.GetPreviewStat(CharacterStatType.Health)}   힘 {d.GetPreviewStat(CharacterStatType.Strength)}   민첩 {d.GetPreviewStat(CharacterStatType.Agility)}\n감각 {d.GetPreviewStat(CharacterStatType.Sense)}   지능 {d.GetPreviewStat(CharacterStatType.Intelligence)}   의지 {d.GetPreviewStat(CharacterStatType.Willpower)}";
        private static string StatName(CharacterStatType stat) => new[] { "체력", "힘", "민첩", "감각", "지능", "의지" }[(int)stat];

        private void CreateInterface()
        {
            Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            GameObject canvasObject = new GameObject("PathSelectionCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasObject.transform.SetParent(transform, false);
            Canvas canvas = canvasObject.GetComponent<Canvas>(); canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>(); scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize; scaler.referenceResolution = new Vector2(1280, 720); scaler.matchWidthOrHeight = 0.5f;
            Image background = Image(canvasObject.transform, "Background", new Color(0.018f, 0.03f, 0.06f, 1)); Stretch(background.rectTransform);
            Text title = Text(canvasObject.transform, "Title", "PROJECT LIMITLESS", font, 36, new Vector2(.5f, .92f), new Vector2(650, 48)); title.color = accentColor; title.fontStyle = FontStyle.Bold;
            Text(canvasObject.transform, "Subtitle", "당신의 길을 선택하세요", font, 22, new Vector2(.5f, .86f), new Vector2(500, 34));
            CreateSteps(canvasObject.transform, font);
            CreateCharacterSummary(canvasObject.transform, font);
            CreatePathCards(canvasObject.transform, font);
            CreateDetailPanel(canvasObject.transform, font);
            CreateBottomControls(canvasObject.transform, font);
            ConfigureNavigation();
        }

        private void CreateCharacterSummary(Transform parent, Font font)
        {
            Image panel = Image(parent, "CharacterSummary", new Color(.055f, .08f, .13f, .97f)); SetRect(panel.rectTransform, new Vector2(.13f, .48f), new Vector2(190, 390)); AddOutline(panel.gameObject, mutedColor, 2);
            Text heading = Text(panel.transform, "Heading", "캐릭터", font, 20, new Vector2(.5f, .91f), new Vector2(160, 32)); heading.color = accentColor;
            Image preview = Image(panel.transform, "Preview", Color.white); preview.sprite = GameSessionData.SelectedPlayerVisual == PlayerVisualType.Female ? femalePreviewSprite : malePreviewSprite; preview.preserveAspect = true; SetRect(preview.rectTransform, new Vector2(.5f, .62f), new Vector2(150, 170));
            string name = string.IsNullOrWhiteSpace(GameSessionData.PlayerName) ? "이름 미설정" : GameSessionData.PlayerName;
            string visual = GameSessionData.SelectedPlayerVisual == PlayerVisualType.Female ? "여성" : "남성";
            Text summary = Text(panel.transform, "Summary", $"{name}\n{visual}", font, 20, new Vector2(.5f, .22f), new Vector2(165, 80)); summary.fontStyle = FontStyle.Bold;
        }

        private void CreatePathCards(Transform parent, Font font)
        {
            for (int i = 0; i < pathDefinitions.Length; i++)
            {
                int captured = i;
                GameObject card = new GameObject($"PathCard{i + 1}", typeof(Image), typeof(Button), typeof(Outline)); card.transform.SetParent(parent, false);
                SetRect(card.GetComponent<RectTransform>(), new Vector2(.355f, .70f - i * .085f), new Vector2(300, 56));
                Image image = card.GetComponent<Image>(); Button button = card.GetComponent<Button>(); button.targetGraphic = image; button.colors = SelectableColors();
                Outline outline = card.GetComponent<Outline>(); Text label = Text(card.transform, "Label", "", font, 17, Vector2.one * .5f, new Vector2(275, 50)); label.fontStyle = FontStyle.Bold;
                button.onClick.AddListener(() => SelectPath(pathDefinitions[captured])); AddFocusFeedback(card, outline, RefreshSelection);
                pathButtons.Add(button); pathLabels.Add(label); pathBackgrounds.Add(image); pathOutlines.Add(outline); tabControls.Add(button);
            }
        }

        private void CreateDetailPanel(Transform parent, Font font)
        {
            Image panel = Image(parent, "DetailPanel", new Color(.055f, .08f, .13f, .97f)); SetRect(panel.rectTransform, new Vector2(.72f, .48f), new Vector2(500, 390)); AddOutline(panel.gameObject, mutedColor, 2);
            detailName = Text(panel.transform, "PathName", "", font, 27, new Vector2(.5f, .89f), new Vector2(450, 42)); detailName.color = accentColor; detailName.fontStyle = FontStyle.Bold;
            detailDescription = Text(panel.transform, "Description", "", font, 18, new Vector2(.5f, .73f), new Vector2(440, 70));
            detailStats = Text(panel.transform, "Stats", "", font, 18, new Vector2(.5f, .50f), new Vector2(440, 92));
            detailPassive = Text(panel.transform, "Passive", "", font, 18, new Vector2(.5f, .27f), new Vector2(440, 92));
            detailKeywords = Text(panel.transform, "Keywords", "", font, 16, new Vector2(.5f, .08f), new Vector2(450, 45)); detailKeywords.color = new Color(.76f, .83f, .92f, 1);
        }

        private void CreateBottomControls(Transform parent, Font font)
        {
            previousButton = TextButton(parent, "PreviousButton", "이전", font, new Vector2(.39f, .105f)); nextButton = TextButton(parent, "NextButton", "다음", font, new Vector2(.66f, .105f));
            previousButton.onClick.AddListener(() => SceneManager.LoadSceneAsync(previousSceneName, LoadSceneMode.Single));
            nextButton.onClick.AddListener(() => noticeLabel.text = string.IsNullOrEmpty(selectedPathId) ? "먼저 길을 선택해주세요." : "직업 선택 화면은 다음 단계에서 구현됩니다.");
            AddFocusFeedback(previousButton.gameObject, previousButton.GetComponent<Outline>(), () => RestoreButton(previousButton)); AddFocusFeedback(nextButton.gameObject, nextButton.GetComponent<Outline>(), () => RestoreButton(nextButton));
            tabControls.Add(previousButton); tabControls.Add(nextButton);
            noticeLabel = Text(parent, "Notice", "", font, 17, new Vector2(.53f, .045f), new Vector2(700, 28)); noticeLabel.color = new Color(1, .77f, .5f, 1);
            Text help = Text(parent, "Help", "Tab / Shift+Tab / 방향키: 이동   Enter / Space: 선택", font, 15, new Vector2(.53f, .015f), new Vector2(760, 24)); help.color = new Color(.7f, .77f, .86f, 1);
        }

        private void ConfigureNavigation()
        {
            for (int i = 0; i < pathButtons.Count; i++) pathButtons[i].navigation = Nav(null, null, i > 0 ? pathButtons[i - 1] : null, i + 1 < pathButtons.Count ? pathButtons[i + 1] : previousButton);
            Selectable last = pathButtons.Count > 0 ? pathButtons[pathButtons.Count - 1] : null;
            previousButton.navigation = Nav(null, nextButton, last, null); nextButton.navigation = Nav(previousButton, null, last, null);
        }

        private Button GetInitialFocusButton() { int i = Array.FindIndex(pathDefinitions, d => d.Id == selectedPathId); return pathButtons[i >= 0 ? i : 0]; }
        private void MoveToNextControl(int direction) { int i = tabControls.FindIndex(c => c.gameObject == EventSystem.current.currentSelectedGameObject); EventSystem.current.SetSelectedGameObject(tabControls[(i + direction + tabControls.Count) % tabControls.Count].gameObject); }
        private void AddFocusFeedback(GameObject target, Outline outline, Action restore) { EventTrigger trigger = target.AddComponent<EventTrigger>(); AddTrigger(trigger, EventTriggerType.Select, _ => { outline.effectColor = focusColor; outline.effectDistance = new Vector2(5, -5); }); AddTrigger(trigger, EventTriggerType.Deselect, _ => restore()); }
        private static void AddTrigger(EventTrigger trigger, EventTriggerType type, Action<BaseEventData> action) { EventTrigger.Entry entry = new EventTrigger.Entry { eventID = type }; entry.callback.AddListener(data => action(data)); trigger.triggers.Add(entry); }
        private void RestoreButton(Button button) { Outline o = button.GetComponent<Outline>(); o.effectColor = accentColor; o.effectDistance = new Vector2(2, -2); }
        private static Navigation Nav(Selectable left, Selectable right, Selectable up, Selectable down) => new Navigation { mode = Navigation.Mode.Explicit, selectOnLeft = left, selectOnRight = right, selectOnUp = up, selectOnDown = down };
        private static void CreateEventSystem() { if (EventSystem.current != null) return; InputSystemUIInputModule module = new GameObject("EventSystem", typeof(EventSystem)).AddComponent<InputSystemUIInputModule>(); module.AssignDefaultActions(); }
        private static Button TextButton(Transform parent, string name, string label, Font font, Vector2 anchor) { GameObject obj = new GameObject(name, typeof(Image), typeof(Button), typeof(Outline)); obj.transform.SetParent(parent, false); SetRect(obj.GetComponent<RectTransform>(), anchor, new Vector2(230, 52)); Button button = obj.GetComponent<Button>(); button.targetGraphic = obj.GetComponent<Image>(); button.colors = SelectableColors(); Outline o = obj.GetComponent<Outline>(); o.effectColor = new Color(.88f, .7f, .32f, 1); o.effectDistance = new Vector2(2, -2); Text(obj.transform, "Label", label, font, 22, Vector2.one * .5f, new Vector2(210, 44)).fontStyle = FontStyle.Bold; return button; }
        private static ColorBlock SelectableColors() { ColorBlock c = ColorBlock.defaultColorBlock; c.normalColor = Color.white; c.highlightedColor = new Color(.82f, .88f, .98f, 1); c.selectedColor = new Color(.92f, .82f, .58f, 1); c.pressedColor = new Color(.68f, .68f, .68f, 1); return c; }
        private static void CreateSteps(Transform parent, Font font) { string[] steps = { "1 기본 정보", "2 길 · 현재", "3 직업", "4 확인" }; for (int i = 0; i < steps.Length; i++) { Image p = Image(parent, $"Step{i + 1}", i == 1 ? new Color(.18f, .25f, .34f, 1) : new Color(.04f, .065f, .11f, .88f)); SetRect(p.rectTransform, new Vector2(.35f + i * .1f, .975f), new Vector2(122, 28)); AddOutline(p.gameObject, i == 1 ? new Color(.95f, .76f, .36f, 1) : new Color(.25f, .32f, .42f, 1), i == 1 ? 2 : 1); Text(p.transform, "Label", steps[i], font, 15, Vector2.one * .5f, new Vector2(118, 26)); } }
        private static Text Text(Transform parent, string name, string value, Font font, int size, Vector2 anchor, Vector2 dimensions) { GameObject obj = new GameObject(name, typeof(Text)); obj.transform.SetParent(parent, false); Text text = obj.GetComponent<Text>(); text.font = font; text.fontSize = size; text.color = Color.white; text.alignment = TextAnchor.MiddleCenter; text.text = value; text.raycastTarget = false; SetRect(text.rectTransform, anchor, dimensions); return text; }
        private static Image Image(Transform parent, string name, Color color) { GameObject obj = new GameObject(name, typeof(Image)); obj.transform.SetParent(parent, false); Image image = obj.GetComponent<Image>(); image.color = color; image.raycastTarget = false; return image; }
        private static Outline AddOutline(GameObject target, Color color, float distance) { Outline o = target.AddComponent<Outline>(); o.effectColor = color; o.effectDistance = new Vector2(distance, -distance); return o; }
        private static void SetRect(RectTransform rect, Vector2 anchor, Vector2 size) { rect.anchorMin = anchor; rect.anchorMax = anchor; rect.pivot = Vector2.one * .5f; rect.anchoredPosition = Vector2.zero; rect.sizeDelta = size; }
        private static void Stretch(RectTransform rect) { rect.anchorMin = Vector2.zero; rect.anchorMax = Vector2.one; rect.offsetMin = Vector2.zero; rect.offsetMax = Vector2.zero; }
    }
}

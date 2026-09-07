using System;
using System.Collections.Generic;
using System.Linq;
using ProjectLimitless.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjectLimitless.UI
{
    /// <summary>공식 아이콘·이름·특성을 함께 보여 주는 캐릭터 생성 2단계입니다.</summary>
    public sealed class PathSelectionController : MonoBehaviour
    {
        private static readonly string[] DisplayOrder =
        {
            "path.emotional-scar", "path.hearing", "path.vision", "path.mobility", "path.intellectual"
        };
        [SerializeField] private Sprite malePreviewSprite;
        [SerializeField] private Sprite femalePreviewSprite;
        [SerializeField] private string previousSceneName = "CharacterCreation";
        [SerializeField] private string nextSceneName = "JobSelection";
        [SerializeField] private PlayerPathDefinition[] pathDefinitions;

        private readonly List<Button> cards = new List<Button>();
        private readonly List<Outline> cardOutlines = new List<Outline>();
        private readonly List<Text> cardChecks = new List<Text>();
        private readonly List<Selectable> controls = new List<Selectable>();
        private readonly Color gold = new Color(.88f, .7f, .32f, 1f);
        private readonly Color focusGold = new Color(1f, .86f, .48f, 1f);
        private readonly Color normal = new Color(.06f, .09f, .15f, .98f);
        private readonly Color selected = new Color(.13f, .2f, .3f, 1f);
        private Text detailName;
        private Text detailTrait;
        private Text detailDescription;
        private Text detailRecommended;
        private Text notice;
        private Image detailIcon;
        private Button previousButton;
        private Button chooseButton;
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
            if (pathDefinitions == null || pathDefinitions.Length == 0)
                pathDefinitions = PathPresentationResolver.All.ToArray();
            pathDefinitions = pathDefinitions.Where(item => item != null)
                .OrderBy(item => Array.IndexOf(DisplayOrder, item.Id) is int index && index >= 0 ? index : DisplayOrder.Length)
                .ThenBy(item => item.Id).ToArray();
            CreateEventSystem();
            CreateInterface();
            selectedPathId = GameSessionData.SelectedPlayerPathId;
            if (!pathDefinitions.Any(item => item.Id == selectedPathId)) selectedPathId = string.Empty;
            Refresh();
            if (cards.Count > 0) EventSystem.current.SetSelectedGameObject(cards[Math.Max(0, Array.FindIndex(pathDefinitions, item => item.Id == selectedPathId))].gameObject);
        }

        private void Update()
        {
            if (Keyboard.current == null || controls.Count == 0 || !Keyboard.current.tabKey.wasPressedThisFrame) return;
            int direction = Keyboard.current.shiftKey.isPressed ? -1 : 1;
            int index = controls.FindIndex(item => item.gameObject == EventSystem.current.currentSelectedGameObject);
            EventSystem.current.SetSelectedGameObject(controls[(index + direction + controls.Count) % controls.Count].gameObject);
        }

        private void SelectPath(int index)
        {
            selectedPathId = pathDefinitions[index].Id;
            GameSessionData.SelectPlayerPath(selectedPathId);
            notice.text = string.Empty;
            Refresh();
        }

        private void Refresh()
        {
            for (int i = 0; i < cards.Count; i++)
            {
                bool active = pathDefinitions[i].Id == selectedPathId;
                cards[i].GetComponent<Image>().color = active ? selected : normal;
                cardOutlines[i].effectColor = active ? focusGold : new Color(.28f, .35f, .45f, 1f);
                cardOutlines[i].effectDistance = active ? new Vector2(4, -4) : new Vector2(2, -2);
                cards[i].transform.localScale = active ? Vector3.one * 1.03f : Vector3.one;
                cardChecks[i].text = active ? "✓ 선택됨" : string.Empty;
            }
            PlayerPathDefinition path = PathPresentationResolver.Find(selectedPathId) ?? pathDefinitions.FirstOrDefault();
            if (path == null) return;
            detailName.text = path.DisplayName;
            detailTrait.text = path.PassiveName;
            detailDescription.text = path.PassiveDescription;
            detailRecommended.text = $"[추천: {string.Join(" · ", path.RecommendedJobs.Select(item => item.DisplayName))}]";
            detailIcon.sprite = path.Icon;
            detailIcon.color = detailIcon.sprite == null ? Color.clear : Color.white;
        }

        private void CreateInterface()
        {
            Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            GameObject canvasObject = new GameObject("PathSelectionCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            Canvas canvas = canvasObject.GetComponent<Canvas>(); canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>(); scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize; scaler.referenceResolution = new Vector2(1280, 720); scaler.matchWidthOrHeight = .5f;
            Image background = MakeImage(canvas.transform, "Background", new Color(.018f, .03f, .06f, 1f)); Stretch(background.rectTransform);
            Text title = MakeText(canvas.transform, "Title", "길 선택", font, 34, new Vector2(.5f, .94f), new Vector2(600, 44)); title.color = gold; title.fontStyle = FontStyle.Bold;
            MakeText(canvas.transform, "Guide", "당신이 걸어갈 길을 선택하세요.\n길은 전투 특성에 영향을 주지만, 직업 선택을 제한하지 않습니다.", font, 17, new Vector2(.5f, .865f), new Vector2(900, 58));
            CreateCards(canvas.transform, font);
            CreateDetail(canvas.transform, font);
            previousButton = MakeButton(canvas.transform, "PreviousButton", "[ 이전 ]", font, new Vector2(.35f, .055f), () => SceneManager.LoadSceneAsync(previousSceneName, LoadSceneMode.Single));
            chooseButton = MakeButton(canvas.transform, "ChooseButton", "[ 이 길을 선택 ]", font, new Vector2(.65f, .055f), Confirm);
            controls.Add(previousButton); controls.Add(chooseButton);
            notice = MakeText(canvas.transform, "Notice", string.Empty, font, 15, new Vector2(.5f, .012f), new Vector2(700, 22)); notice.color = new Color(1f, .7f, .45f, 1f);
            ConfigureNavigation();
        }

        private void CreateCards(Transform parent, Font font)
        {
            for (int i = 0; i < pathDefinitions.Length; i++)
            {
                int captured = i;
                PlayerPathDefinition path = pathDefinitions[i];
                GameObject cardObject = new GameObject($"PathCard{i + 1}", typeof(Image), typeof(Button), typeof(Outline));
                cardObject.transform.SetParent(parent, false);
                SetRect(cardObject.GetComponent<RectTransform>(), new Vector2(.15f + i * .175f, .64f), new Vector2(190, 190));
                Button card = cardObject.GetComponent<Button>(); card.targetGraphic = cardObject.GetComponent<Image>(); card.onClick.AddListener(() => SelectPath(captured));
                Outline outline = cardObject.GetComponent<Outline>();
                Image icon = MakeImage(cardObject.transform, "OfficialIcon", Color.clear); icon.sprite = path.Icon; icon.color = icon.sprite == null ? Color.clear : Color.white; icon.preserveAspect = true; SetRect(icon.rectTransform, new Vector2(.5f, .73f), new Vector2(66, 66));
                if (path.Icon == null) { Text todo = MakeText(cardObject.transform, "IconTodo", "아이콘 준비 중", font, 12, new Vector2(.5f, .73f), new Vector2(130, 24)); todo.color = new Color(.65f, .7f, .78f, 1f); }
                Text name = MakeText(cardObject.transform, "PathName", path.DisplayName, font, 20, new Vector2(.5f, .45f), new Vector2(170, 28)); name.fontStyle = FontStyle.Bold;
                Text trait = MakeText(cardObject.transform, "Trait", path.PassiveName, font, 16, new Vector2(.5f, .29f), new Vector2(170, 24)); trait.color = new Color(.82f, .88f, .96f, 1f);
                Text recommended = MakeText(cardObject.transform, "Recommended", $"[추천: {string.Join(" · ", path.RecommendedJobs.Select(item => item.DisplayName))}]", font, 13, new Vector2(.5f, .13f), new Vector2(180, 24)); recommended.color = new Color(1f, .84f, .46f, 1f);
                Text check = MakeText(cardObject.transform, "Selected", string.Empty, font, 13, new Vector2(.5f, .025f), new Vector2(160, 20)); check.fontStyle = FontStyle.Bold;
                cards.Add(card); cardOutlines.Add(outline); cardChecks.Add(check); controls.Add(card);
            }
        }

        private void CreateDetail(Transform parent, Font font)
        {
            Image panel = MakeImage(parent, "PathDetailPanel", new Color(.05f, .075f, .12f, .98f)); SetRect(panel.rectTransform, new Vector2(.5f, .285f), new Vector2(1040, 210)); AddOutline(panel.gameObject, gold, 2);
            detailIcon = MakeImage(panel.transform, "OfficialIcon", Color.clear); detailIcon.preserveAspect = true; SetRect(detailIcon.rectTransform, new Vector2(.1f, .55f), new Vector2(110, 110));
            detailName = MakeText(panel.transform, "PathName", string.Empty, font, 27, new Vector2(.27f, .72f), new Vector2(260, 38)); detailName.color = gold; detailName.fontStyle = FontStyle.Bold;
            detailTrait = MakeText(panel.transform, "TraitName", string.Empty, font, 21, new Vector2(.27f, .48f), new Vector2(260, 32)); detailTrait.fontStyle = FontStyle.Bold;
            detailRecommended = MakeText(panel.transform, "Recommended", string.Empty, font, 16, new Vector2(.27f, .24f), new Vector2(290, 28)); detailRecommended.color = new Color(1f, .84f, .46f, 1f);
            detailDescription = MakeText(panel.transform, "Description", string.Empty, font, 17, new Vector2(.68f, .61f), new Vector2(560, 90)); detailDescription.alignment = TextAnchor.MiddleLeft;
            Text guide = MakeText(panel.transform, "Guide", "추천은 시너지 안내이며 모든 직업을 자유롭게 선택할 수 있습니다.", font, 14, new Vector2(.68f, .23f), new Vector2(560, 28)); guide.color = new Color(.72f, .8f, .9f, 1f);
        }

        private void Confirm()
        {
            if (string.IsNullOrEmpty(selectedPathId)) { notice.text = "먼저 길 카드를 선택해주세요."; return; }
            SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Single);
        }

        private void ConfigureNavigation()
        {
            for (int i = 0; i < cards.Count; i++) cards[i].navigation = Nav(i > 0 ? cards[i - 1] : cards[cards.Count - 1], i + 1 < cards.Count ? cards[i + 1] : cards[0], chooseButton, chooseButton);
            previousButton.navigation = Nav(chooseButton, chooseButton, cards[0], cards[0]);
            chooseButton.navigation = Nav(previousButton, previousButton, cards[cards.Count - 1], cards[cards.Count - 1]);
        }

        private static Button MakeButton(Transform parent, string name, string label, Font font, Vector2 anchor, Action action)
        {
            GameObject obj = new GameObject(name, typeof(Image), typeof(Button), typeof(Outline)); obj.transform.SetParent(parent, false); SetRect(obj.GetComponent<RectTransform>(), anchor, new Vector2(250, 48));
            obj.GetComponent<Image>().color = new Color(.12f, .32f, .5f, 1f); AddOutline(obj, new Color(.88f, .7f, .32f, 1f), 2); Button button = obj.GetComponent<Button>(); button.onClick.AddListener(() => action()); MakeText(obj.transform, "Label", label, font, 20, Vector2.one * .5f, new Vector2(230, 40)).fontStyle = FontStyle.Bold; return button;
        }
        private static Navigation Nav(Selectable left, Selectable right, Selectable up, Selectable down) => new Navigation { mode = Navigation.Mode.Explicit, selectOnLeft = left, selectOnRight = right, selectOnUp = up, selectOnDown = down };
        private static void CreateEventSystem() { if (EventSystem.current != null) return; InputSystemUIInputModule module = new GameObject("EventSystem", typeof(EventSystem)).AddComponent<InputSystemUIInputModule>(); module.AssignDefaultActions(); }
        private static Text MakeText(Transform parent, string name, string value, Font font, int size, Vector2 anchor, Vector2 dimensions) { GameObject obj = new GameObject(name, typeof(Text)); obj.transform.SetParent(parent, false); Text text = obj.GetComponent<Text>(); text.font = font; text.fontSize = size; text.color = Color.white; text.alignment = TextAnchor.MiddleCenter; text.text = value; text.raycastTarget = false; SetRect(text.rectTransform, anchor, dimensions); return text; }
        private static Image MakeImage(Transform parent, string name, Color color) { GameObject obj = new GameObject(name, typeof(Image)); obj.transform.SetParent(parent, false); Image image = obj.GetComponent<Image>(); image.color = color; image.raycastTarget = false; return image; }
        private static Outline AddOutline(GameObject target, Color color, float distance) { Outline outline = target.GetComponent<Outline>() ?? target.AddComponent<Outline>(); outline.effectColor = color; outline.effectDistance = new Vector2(distance, -distance); return outline; }
        private static void SetRect(RectTransform rect, Vector2 anchor, Vector2 size) { rect.anchorMin = anchor; rect.anchorMax = anchor; rect.pivot = Vector2.one * .5f; rect.anchoredPosition = Vector2.zero; rect.sizeDelta = size; }
        private static void Stretch(RectTransform rect) { rect.anchorMin = Vector2.zero; rect.anchorMax = Vector2.one; rect.offsetMin = Vector2.zero; rect.offsetMax = Vector2.zero; }
    }
}

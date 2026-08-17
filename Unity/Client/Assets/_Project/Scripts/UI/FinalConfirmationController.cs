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
    /// <summary>세션에 저장된 캐릭터 생성 결과를 데이터 에셋으로 조회해 최종 확인 화면을 구성합니다.</summary>
    public sealed class FinalConfirmationController : MonoBehaviour
    {
        [SerializeField] private Sprite malePreviewSprite;
        [SerializeField] private Sprite femalePreviewSprite;
        [SerializeField] private string previousSceneName = "JobSelection";
        [SerializeField] private string worldSceneName = "World_StarterVillage";

        private readonly List<Selectable> controls = new List<Selectable>();
        private readonly Color gold = new Color(.88f, .7f, .32f, 1f);
        private readonly Color focusGold = new Color(1f, .86f, .48f, 1f);
        private PlayerPathDefinition path;
        private JobDefinition job;

        public void Configure(Sprite male, Sprite female, string previousScene, string worldScene)
        { malePreviewSprite = male; femalePreviewSprite = female; previousSceneName = previousScene; worldSceneName = worldScene; }

        private void Awake()
        {
            path = Resources.LoadAll<PlayerPathDefinition>("PathDefinitions").FirstOrDefault(item => item.Id == GameSessionData.SelectedPlayerPathId);
            job = Resources.LoadAll<JobDefinition>("JobDefinitions").FirstOrDefault(item => item.JobId == GameSessionData.SelectedJobId);
            ValidateSession(); CreateEventSystem(); CreateInterface();
            Selectable initialFocus = controls.Last().interactable ? controls.Last() : controls.First();
            EventSystem.current.SetSelectedGameObject(initialFocus.gameObject);
        }

        private void Update()
        {
            if (Keyboard.current == null || controls.Count == 0) return;
            if (Keyboard.current.tabKey.wasPressedThisFrame) MoveFocus(Keyboard.current.shiftKey.isPressed ? -1 : 1);
            else if (Keyboard.current.spaceKey.wasPressedThisFrame) EventSystem.current.currentSelectedGameObject?.GetComponent<Button>()?.onClick.Invoke();
        }

        private void ValidateSession()
        {
            if (string.IsNullOrWhiteSpace(GameSessionData.PlayerName)) Debug.LogWarning("FinalConfirmation: 플레이어 이름이 비어 있습니다.");
            if (path == null) Debug.LogError($"FinalConfirmation: 길 ID '{GameSessionData.SelectedPlayerPathId}'에 해당하는 PathDefinition을 찾지 못했습니다.");
            if (job == null) Debug.LogError($"FinalConfirmation: 직업 ID '{GameSessionData.SelectedJobId}'에 해당하는 JobDefinition을 찾지 못했습니다.");
        }

        private void CreateInterface()
        {
            CreateCameraIfMissing();
            Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            GameObject canvasObject = new GameObject("FinalConfirmationCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster)); canvasObject.transform.SetParent(transform, false);
            canvasObject.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>(); scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize; scaler.referenceResolution = new Vector2(1280, 720); scaler.matchWidthOrHeight = .5f;
            Image background = MakeImage(canvasObject.transform, "Background", new Color(.018f, .03f, .06f, 1)); Stretch(background.rectTransform);
            Text title = MakeText(canvasObject.transform, "Title", "PROJECT LIMITLESS", font, 36, new Vector2(.5f, .92f), new Vector2(650, 48)); title.color = gold; title.fontStyle = FontStyle.Bold;
            MakeText(canvasObject.transform, "Subtitle", "캐릭터 최종 확인", font, 22, new Vector2(.5f, .86f), new Vector2(500, 34));
            CreateSteps(canvasObject.transform, font); CreateCharacter(canvasObject.transform, font); CreateSelection(canvasObject.transform, font); CreateStats(canvasObject.transform, font); CreateButtons(canvasObject.transform, font);
        }

        private void CreateCharacter(Transform parent, Font font)
        {
            Image panel = Panel(parent, "CharacterPanel", new Vector2(.13f, .48f), new Vector2(245, 420));
            Heading(panel.transform, font, "캐릭터", .92f);
            Image previewFrame = MakeImage(panel.transform, "PreviewFrame", new Color(.025f, .045f, .075f, 1)); SetRect(previewFrame.rectTransform, new Vector2(.5f, .66f), new Vector2(175, 190)); AddOutline(previewFrame.gameObject, new Color(.32f, .4f, .52f, 1), 2);
            Sprite selectedSprite = GameSessionData.SelectedPlayerVisual == PlayerVisualType.Female ? femalePreviewSprite : malePreviewSprite;
            Image characterImage = MakeImage(previewFrame.transform, "CharacterImage", Color.white); characterImage.sprite = selectedSprite; characterImage.preserveAspect = true; SetRect(characterImage.rectTransform, Vector2.one * .5f, new Vector2(155, 170));
            Image pathVisual = MakeImage(previewFrame.transform, "PathVisualPreview", Color.clear); SetRect(pathVisual.rectTransform, Vector2.one * .5f, new Vector2(155, 170));
            Image pathSymbol = MakeImage(panel.transform, "PathSymbol", Color.clear); SetRect(pathSymbol.rectTransform, new Vector2(.78f, .42f), new Vector2(42, 42));
            PathVisualPreview.Apply(characterImage, pathVisual, pathSymbol, selectedSprite, GameSessionData.SelectedPlayerPathId, GameSessionData.SelectedPlayerVisual);
            if (selectedSprite == null)
            {
                // Sprite 참조가 끊겨도 불투명한 흰 사각형이 캐릭터처럼 보이지 않게 하고 원인을 Console에 남깁니다.
                characterImage.color = Color.clear;
                Debug.LogError($"FinalConfirmation: {GameSessionData.SelectedPlayerVisual} Down Idle Sprite가 연결되지 않았습니다.");
            }
            string playerName = string.IsNullOrWhiteSpace(GameSessionData.PlayerName) ? "이름 미설정" : GameSessionData.PlayerName;
            string visual = GameSessionData.SelectedPlayerVisual == PlayerVisualType.Female ? "여성" : "남성";
            Text summary = MakeText(panel.transform, "Summary", $"{playerName}\n\n성별  {visual}", font, 20, new Vector2(.5f, .25f), new Vector2(210, 110)); summary.fontStyle = FontStyle.Bold;
        }

        private void CreateSelection(Transform parent, Font font)
        {
            Image panel = Panel(parent, "SelectionPanel", new Vector2(.49f, .49f), new Vector2(535, 440));
            string pathBonuses = path == null ? "데이터 없음" : string.Join(" · ", path.StatBonuses.Select(b => $"{StatName(b.Stat)} +{b.Amount}"));
            string jobBonuses = job == null ? "데이터 없음" : string.Join(" · ", job.StatBonuses.Select(b => $"{StatName(b.Stat)} +{b.Amount}"));
            string skills = job == null ? "• 데이터 없음" : string.Join("\n", job.StartingSkills.Select(s => $"• {s.SkillName}"));
            bool recommended = path != null && job != null && path.RecommendedJobs.Any(item => item.Id == job.JobId);
            Text content = MakeText(panel.transform, "SelectionSummary",
                $"선택한 길\n{path?.DisplayName ?? "길 미선택"}\n외형  {PathVisualPreview.GetDisplayName(GameSessionData.SelectedPlayerPathId)}\n능력치  {pathBonuses}\n패시브  {path?.PassiveName ?? "-"}\n\n선택한 직업{(recommended ? "   <color=#FFDB8A>[추천 직업]</color>" : "")}\n{job?.DisplayName ?? "직업 미선택"}\n역할  {job?.RoleName ?? "-"}\n능력치  {jobBonuses}\n패시브  {(job == null ? "-" : job.Passive.PassiveName)}\n\n시작 스킬\n{skills}",
                font, 18, new Vector2(.5f, .5f), new Vector2(480, 400));
            content.supportRichText = true; content.alignment = TextAnchor.MiddleLeft; content.fontStyle = FontStyle.Bold;
        }

        private void CreateStats(Transform parent, Font font)
        {
            Image panel = Panel(parent, "StatsPanel", new Vector2(.82f, .49f), new Vector2(300, 400)); Heading(panel.transform, font, "최종 능력치", .9f);
            Text note = MakeText(panel.transform, "Note", "기본 10 + 길 + 직업", font, 15, new Vector2(.5f, .8f), new Vector2(260, 28)); note.color = new Color(.72f, .78f, .88f, 1);
            int V(CharacterStatType stat) => CharacterCreationStatsCalculator.GetFinalStat(path, job, stat);
            Text stats = MakeText(panel.transform, "Stats", $"체력  {V(CharacterStatType.Health)}      힘  {V(CharacterStatType.Strength)}\n\n민첩  {V(CharacterStatType.Agility)}      감각  {V(CharacterStatType.Sense)}\n\n지능  {V(CharacterStatType.Intelligence)}      의지  {V(CharacterStatType.Willpower)}", font, 21, new Vector2(.5f, .49f), new Vector2(270, 210)); stats.fontStyle = FontStyle.Bold;
        }

        private void CreateButtons(Transform parent, Font font)
        {
            Button previous = MakeButton(parent, "PreviousButton", "이전", font, new Vector2(.39f, .095f)); Button start = MakeButton(parent, "StartButton", "게임 시작", font, new Vector2(.66f, .095f));
            previous.onClick.AddListener(() => SceneManager.LoadSceneAsync(previousSceneName, LoadSceneMode.Single));
            start.interactable = path != null && job != null && !string.IsNullOrWhiteSpace(GameSessionData.PlayerName);
            start.onClick.AddListener(() => SceneManager.LoadSceneAsync(worldSceneName, LoadSceneMode.Single)); controls.Add(previous); controls.Add(start);
            previous.navigation = Nav(start, start); start.navigation = Nav(previous, previous);
            MakeText(parent, "Help", "Tab / Shift+Tab / 방향키: 이동   Enter / Space: 선택", font, 15, new Vector2(.53f, .02f), new Vector2(760, 24)).color = new Color(.7f, .77f, .86f, 1);
        }

        private void MoveFocus(int direction) { int index = controls.FindIndex(item => item.gameObject == EventSystem.current.currentSelectedGameObject); EventSystem.current.SetSelectedGameObject(controls[(index + direction + controls.Count) % controls.Count].gameObject); }
        private Image Panel(Transform parent, string name, Vector2 anchor, Vector2 size) { Image panel = MakeImage(parent, name, new Color(.055f, .08f, .13f, .97f)); SetRect(panel.rectTransform, anchor, size); AddOutline(panel.gameObject, new Color(.32f, .4f, .52f, 1), 2); return panel; }
        private void Heading(Transform parent, Font font, string label, float y) { Text text = MakeText(parent, "Heading", label, font, 22, new Vector2(.5f, y), new Vector2(260, 34)); text.color = gold; text.fontStyle = FontStyle.Bold; }
        private Button MakeButton(Transform parent, string name, string label, Font font, Vector2 anchor) { GameObject obj = new GameObject(name, typeof(Image), typeof(Button), typeof(Outline)); obj.transform.SetParent(parent, false); SetRect(obj.GetComponent<RectTransform>(), anchor, new Vector2(240, 56)); Image image = obj.GetComponent<Image>(); image.color = new Color(.12f, .32f, .5f, 1); Button button = obj.GetComponent<Button>(); button.targetGraphic = image; button.colors = CtaColors(); Outline outline = obj.GetComponent<Outline>(); outline.effectColor = gold; outline.effectDistance = new Vector2(2, -2); MakeText(obj.transform, "Label", $"[ {label} ]", font, 22, Vector2.one * .5f, new Vector2(220, 48)).fontStyle = FontStyle.Bold; EventTrigger trigger = obj.AddComponent<EventTrigger>(); AddTrigger(trigger, EventTriggerType.Select, _ => { outline.effectColor = focusGold; outline.effectDistance = new Vector2(5, -5); }); AddTrigger(trigger, EventTriggerType.Deselect, _ => { outline.effectColor = gold; outline.effectDistance = new Vector2(2, -2); }); return button; }
        private static Navigation Nav(Selectable left, Selectable right) => new Navigation { mode = Navigation.Mode.Explicit, selectOnLeft = left, selectOnRight = right, selectOnUp = left, selectOnDown = right };
        private static ColorBlock CtaColors() { ColorBlock c = ColorBlock.defaultColorBlock; c.normalColor = Color.white; c.highlightedColor = new Color(1.25f, 1.18f, 1.08f, 1); c.selectedColor = new Color(1.18f, 1.12f, 1.02f, 1); c.pressedColor = new Color(.62f, .72f, .82f, 1); c.disabledColor = new Color(.48f, .52f, .58f, .72f); return c; }
        private static void CreateSteps(Transform parent, Font font) { string[] steps = { "1 기본 정보", "2 길", "3 직업", "4 확인 · 현재" }; for (int i = 0; i < steps.Length; i++) { bool current = i == 3; Image panel = MakeImage(parent, $"Step{i + 1}", current ? new Color(.18f, .25f, .34f, 1) : new Color(.04f, .065f, .11f, .88f)); SetRect(panel.rectTransform, new Vector2(.35f + i * .1f, .975f), new Vector2(122, 28)); AddOutline(panel.gameObject, current ? new Color(.95f, .76f, .36f, 1) : new Color(.25f, .32f, .42f, 1), current ? 2 : 1); MakeText(panel.transform, "Label", steps[i], font, 15, Vector2.one * .5f, new Vector2(118, 26)); } }
        private static string StatName(CharacterStatType stat) => new[] { "체력", "힘", "민첩", "감각", "지능", "의지" }[(int)stat];
        private static void CreateEventSystem() { if (EventSystem.current != null) return; InputSystemUIInputModule module = new GameObject("EventSystem", typeof(EventSystem)).AddComponent<InputSystemUIInputModule>(); module.AssignDefaultActions(); }
        private static void CreateCameraIfMissing() { if (Camera.main != null) return; GameObject obj = new GameObject("Main Camera"); obj.tag = "MainCamera"; obj.transform.position = new Vector3(0, 0, -10); Camera camera = obj.AddComponent<Camera>(); camera.clearFlags = CameraClearFlags.SolidColor; camera.backgroundColor = new Color(.018f, .03f, .06f, 1); camera.orthographic = true; }
        private static void AddTrigger(EventTrigger trigger, EventTriggerType type, Action<BaseEventData> action) { EventTrigger.Entry entry = new EventTrigger.Entry { eventID = type }; entry.callback.AddListener(data => action(data)); trigger.triggers.Add(entry); }
        private static Text MakeText(Transform parent, string name, string value, Font font, int size, Vector2 anchor, Vector2 dimensions) { GameObject obj = new GameObject(name, typeof(Text)); obj.transform.SetParent(parent, false); Text text = obj.GetComponent<Text>(); text.font = font; text.fontSize = size; text.color = Color.white; text.alignment = TextAnchor.MiddleCenter; text.text = value; text.raycastTarget = false; SetRect(text.rectTransform, anchor, dimensions); return text; }
        private static Image MakeImage(Transform parent, string name, Color color) { GameObject obj = new GameObject(name, typeof(Image)); obj.transform.SetParent(parent, false); Image image = obj.GetComponent<Image>(); image.color = color; image.raycastTarget = false; return image; }
        private static Outline AddOutline(GameObject target, Color color, float distance) { Outline outline = target.AddComponent<Outline>(); outline.effectColor = color; outline.effectDistance = new Vector2(distance, -distance); return outline; }
        private static void SetRect(RectTransform rect, Vector2 anchor, Vector2 size) { rect.anchorMin = anchor; rect.anchorMax = anchor; rect.pivot = Vector2.one * .5f; rect.anchoredPosition = Vector2.zero; rect.sizeDelta = size; }
        private static void Stretch(RectTransform rect) { rect.anchorMin = Vector2.zero; rect.anchorMax = Vector2.one; rect.offsetMin = Vector2.zero; rect.offsetMax = Vector2.zero; }
    }
}

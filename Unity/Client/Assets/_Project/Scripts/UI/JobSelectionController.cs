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
    /// <summary>데이터 에셋에서 직업을 읽어 캐릭터 생성 3단계 선택 화면을 구성합니다.</summary>
    public sealed class JobSelectionController : MonoBehaviour
    {
        [SerializeField] private Sprite malePreviewSprite;
        [SerializeField] private Sprite femalePreviewSprite;
        [SerializeField] private string previousSceneName = "PathSelection";
        [SerializeField] private string nextSceneName = "FinalConfirmation";
        [SerializeField] private JobDefinition[] jobDefinitions;

        private readonly List<Button> jobButtons = new List<Button>();
        private readonly List<Text> jobLabels = new List<Text>();
        private readonly List<Image> jobBackgrounds = new List<Image>();
        private readonly List<Outline> jobOutlines = new List<Outline>();
        private readonly List<Selectable> tabControls = new List<Selectable>();
        private readonly Color normalColor = new Color(.075f, .105f, .17f, .97f);
        private readonly Color selectedColor = new Color(.13f, .2f, .3f, 1f);
        private readonly Color accentColor = new Color(.88f, .7f, .32f, 1f);
        private readonly Color focusColor = new Color(1f, .86f, .48f, 1f);
        private readonly Color mutedColor = new Color(.32f, .4f, .52f, 1f);
        private HashSet<string> recommendedJobIds = new HashSet<string>();
        private string selectedJobId = string.Empty;
        private Button previousButton;
        private Button nextButton;
        private Text detailName;
        private Text detailRole;
        private Text detailDescription;
        private Text detailBonuses;
        private Text detailPassive;
        private Text detailSkills;
        private Text detailFinalStats;
        private Text detailRecommendation;
        private Text noticeLabel;
        private PlayerPathDefinition selectedPath;

        public void Configure(Sprite male, Sprite female, string previousScene, string futureScene)
        {
            malePreviewSprite = male; femalePreviewSprite = female; previousSceneName = previousScene; nextSceneName = futureScene;
        }

        private void Awake()
        {
            LoadData(); CreateEventSystem(); CreateInterface();
            selectedJobId = jobDefinitions.Any(job => job.JobId == GameSessionData.SelectedJobId) ? GameSessionData.SelectedJobId : string.Empty;
            RefreshSelection();
            if (jobButtons.Count > 0) EventSystem.current.SetSelectedGameObject(GetInitialFocus().gameObject);
        }

        private void Update()
        {
            if (Keyboard.current == null || tabControls.Count == 0) return;
            if (Keyboard.current.tabKey.wasPressedThisFrame) { MoveTab(Keyboard.current.shiftKey.isPressed ? -1 : 1); return; }
            if (Keyboard.current.spaceKey.wasPressedThisFrame) EventSystem.current.currentSelectedGameObject?.GetComponent<Button>()?.onClick.Invoke();
        }

        private void LoadData()
        {
            if (jobDefinitions == null || jobDefinitions.Length == 0) jobDefinitions = Resources.LoadAll<JobDefinition>("JobDefinitions");
            jobDefinitions = jobDefinitions.Where(job => job != null && !string.IsNullOrWhiteSpace(job.JobId)).OrderBy(job => job.name).ToArray();
            selectedPath = Resources.LoadAll<PlayerPathDefinition>("PathDefinitions").FirstOrDefault(item => item.Id == GameSessionData.SelectedPlayerPathId);
            // 추천은 배지 표시용일 뿐입니다. 이 집합에 없어도 모든 직업 버튼은 활성화되고 선택할 수 있습니다.
            recommendedJobIds = selectedPath == null ? new HashSet<string>() : selectedPath.RecommendedJobs.Select(job => job.Id).ToHashSet();
            if (jobDefinitions.Length == 0) Debug.LogError("Resources/JobDefinitions에 직업 데이터가 없습니다.");
        }

        private void SelectJob(JobDefinition job)
        {
            selectedJobId = job.JobId;
            // ID만 세션에 저장하므로 Scene을 왕복하거나 향후 저장 파일을 만들 때도 선택을 복원할 수 있습니다.
            GameSessionData.SelectJob(selectedJobId);
            noticeLabel.text = string.Empty;
            RefreshSelection();
        }

        private void RefreshSelection()
        {
            for (int i = 0; i < jobDefinitions.Length; i++)
            {
                JobDefinition job = jobDefinitions[i]; bool selected = job.JobId == selectedJobId; bool recommended = recommendedJobIds.Contains(job.JobId);
                jobBackgrounds[i].color = selected ? selectedColor : normalColor;
                jobOutlines[i].effectColor = selected ? accentColor : mutedColor;
                jobOutlines[i].effectDistance = selected ? new Vector2(4, -4) : new Vector2(2, -2);
                string recommendationBadge = recommended ? " <color=#FFDB8A>[추천 직업]</color>" : string.Empty;
                jobLabels[i].text = $"{job.DisplayName}{recommendationBadge}\n{job.RoleName}{(selected ? "   ✓ 선택됨" : "")}";
            }
            JobDefinition detail = jobDefinitions.FirstOrDefault(job => job.JobId == selectedJobId) ?? jobDefinitions.FirstOrDefault();
            if (detail == null) return;
            detailName.text = detail.DisplayName;
            detailRole.text = "역할\n" + detail.RoleName;
            detailDescription.text = detail.ShortDescription;
            detailBonuses.text = "능력치 보너스\n" + string.Join(" · ", detail.StatBonuses.Select(bonus => $"{StatName(bonus.Stat)} +{bonus.Amount}"));
            detailPassive.text = $"고유 패시브\n{detail.Passive.PassiveName}\n{GetUiSummary(detail.Passive.PassiveDescription)}";
            detailSkills.text = "시작 스킬\n" + string.Join("\n", detail.StartingSkills.Select(skill => $"• {skill.SkillName}"));
            detailFinalStats.text = "길 + 직업 최종 능력치\n" + FormatFinalStats(detail);
            detailRecommendation.text = recommendedJobIds.Contains(detail.JobId) ? "[ 현재 길 추천 ]" : string.Empty;
        }

        /// <summary>전투 스탯을 만들지 않고 기본 10 + 길 + 직업 보너스만 캐릭터 생성용으로 계산합니다.</summary>
        private string FormatFinalStats(JobDefinition job)
        {
            int Value(CharacterStatType stat)
            {
                return CharacterCreationStatsCalculator.GetFinalStat(selectedPath, job, stat);
            }
            return $"체력 {Value(CharacterStatType.Health)}        힘 {Value(CharacterStatType.Strength)}\n민첩 {Value(CharacterStatType.Agility)}        감각 {Value(CharacterStatType.Sense)}\n지능 {Value(CharacterStatType.Intelligence)}        의지 {Value(CharacterStatType.Willpower)}";
        }

        /// <summary>원본 데이터는 보존하고 직업 선택 화면에는 첫 문장만 보여 가독성을 유지합니다.</summary>
        private static string GetUiSummary(string description)
        {
            if (string.IsNullOrWhiteSpace(description)) return string.Empty;
            int sentenceEnd = description.IndexOf('.');
            return sentenceEnd >= 0 ? description.Substring(0, sentenceEnd + 1) : description;
        }

        private void CreateInterface()
        {
            Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            GameObject canvasObject = new GameObject("JobSelectionCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster)); canvasObject.transform.SetParent(transform, false);
            canvasObject.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>(); scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize; scaler.referenceResolution = new Vector2(1280, 720); scaler.matchWidthOrHeight = .5f;
            Image background = Image(canvasObject.transform, "Background", new Color(.018f, .03f, .06f, 1)); Stretch(background.rectTransform);
            Text title = Text(canvasObject.transform, "Title", "PROJECT LIMITLESS", font, 36, new Vector2(.5f, .92f), new Vector2(650, 48)); title.color = accentColor; title.fontStyle = FontStyle.Bold;
            Text(canvasObject.transform, "Subtitle", "당신의 직업을 선택하세요", font, 22, new Vector2(.5f, .86f), new Vector2(500, 34));
            CreateSteps(canvasObject.transform, font); CreateSummary(canvasObject.transform, font); CreateCards(canvasObject.transform, font); CreateDetail(canvasObject.transform, font); CreateBottom(canvasObject.transform, font); ConfigureNavigation();
        }

        private void CreateSummary(Transform parent, Font font)
        {
            Image panel = Image(parent, "CharacterSummary", new Color(.055f, .08f, .13f, .97f)); SetRect(panel.rectTransform, new Vector2(.13f, .48f), new Vector2(190, 390)); AddOutline(panel.gameObject, mutedColor, 2);
            Text heading = Text(panel.transform, "Heading", "캐릭터", font, 20, new Vector2(.5f, .92f), new Vector2(160, 30)); heading.color = accentColor;
            Image preview = Image(panel.transform, "Preview", Color.white); preview.sprite = GameSessionData.SelectedPlayerVisual == PlayerVisualType.Female ? femalePreviewSprite : malePreviewSprite; preview.preserveAspect = true; SetRect(preview.rectTransform, new Vector2(.5f, .67f), new Vector2(145, 155));
            Image pathVisual = Image(panel.transform, "PathVisualPreview", Color.clear); SetRect(pathVisual.rectTransform, new Vector2(.5f, .67f), new Vector2(145, 155));
            Image pathSymbol = Image(panel.transform, "PathSymbol", Color.clear); SetRect(pathSymbol.rectTransform, new Vector2(.78f, .45f), new Vector2(38, 38));
            PathVisualPreview.Apply(preview, pathVisual, pathSymbol, preview.sprite, GameSessionData.SelectedPlayerPathId, GameSessionData.SelectedPlayerVisual);
            string name = string.IsNullOrWhiteSpace(GameSessionData.PlayerName) ? "이름 미설정" : GameSessionData.PlayerName;
            string visual = GameSessionData.SelectedPlayerVisual == PlayerVisualType.Female ? "여성" : "남성";
            string path = Resources.LoadAll<PlayerPathDefinition>("PathDefinitions").FirstOrDefault(item => item.Id == GameSessionData.SelectedPlayerPathId)?.DisplayName ?? "길 미선택";
            Text summary = Text(panel.transform, "Summary", $"{name}\n{visual}\n\n선택한 길\n{path}", font, 18, new Vector2(.5f, .24f), new Vector2(170, 150)); summary.fontStyle = FontStyle.Bold;
        }

        private void CreateCards(Transform parent, Font font)
        {
            for (int i = 0; i < jobDefinitions.Length; i++)
            {
                int captured = i; GameObject card = new GameObject($"JobCard{i + 1}", typeof(Image), typeof(Button), typeof(Outline)); card.transform.SetParent(parent, false); SetRect(card.GetComponent<RectTransform>(), new Vector2(.355f, .70f - i * .085f), new Vector2(300, 56));
                Image image = card.GetComponent<Image>(); Button button = card.GetComponent<Button>(); button.targetGraphic = image; button.colors = SelectableColors(); Outline outline = card.GetComponent<Outline>();
                Text label = Text(card.transform, "Label", "", font, 17, Vector2.one * .5f, new Vector2(275, 50)); label.supportRichText = true; label.fontStyle = FontStyle.Bold; button.onClick.AddListener(() => SelectJob(jobDefinitions[captured])); AddFocus(card, outline, RefreshSelection);
                jobButtons.Add(button); jobLabels.Add(label); jobBackgrounds.Add(image); jobOutlines.Add(outline); tabControls.Add(button);
            }
        }

        private void CreateDetail(Transform parent, Font font)
        {
            Image panel = Image(parent, "DetailPanel", new Color(.055f, .08f, .13f, .97f)); SetRect(panel.rectTransform, new Vector2(.735f, .505f), new Vector2(535, 490)); AddOutline(panel.gameObject, mutedColor, 2);
            detailName = Text(panel.transform, "Name", "", font, 28, new Vector2(.5f, .925f), new Vector2(490, 38)); detailName.color = accentColor; detailName.fontStyle = FontStyle.Bold;
            detailRecommendation = Text(panel.transform, "Recommendation", "", font, 14, new Vector2(.5f, .855f), new Vector2(240, 24)); detailRecommendation.color = new Color(1, .86f, .54f, 1); detailRecommendation.fontStyle = FontStyle.Bold;
            detailRole = Text(panel.transform, "Role", "", font, 17, new Vector2(.5f, .775f), new Vector2(480, 42)); detailRole.fontStyle = FontStyle.Bold;
            detailDescription = Text(panel.transform, "Description", "", font, 17, new Vector2(.5f, .685f), new Vector2(485, 40));
            detailBonuses = Text(panel.transform, "Bonuses", "", font, 18, new Vector2(.5f, .58f), new Vector2(480, 52)); detailBonuses.color = new Color(1, .86f, .54f, 1); detailBonuses.fontStyle = FontStyle.Bold;
            detailPassive = Text(panel.transform, "Passive", "", font, 16, new Vector2(.5f, .445f), new Vector2(490, 76));
            detailSkills = Text(panel.transform, "StartingSkills", "", font, 17, new Vector2(.5f, .275f), new Vector2(470, 84)); detailSkills.alignment = TextAnchor.MiddleLeft; detailSkills.fontStyle = FontStyle.Bold;
            detailFinalStats = Text(panel.transform, "FinalStats", "", font, 17, new Vector2(.5f, .105f), new Vector2(390, 78)); detailFinalStats.fontStyle = FontStyle.Bold;
        }

        private void CreateBottom(Transform parent, Font font)
        {
            previousButton = TextButton(parent, "PreviousButton", "이전", font, new Vector2(.39f, .105f)); nextButton = TextButton(parent, "NextButton", "다음", font, new Vector2(.66f, .105f));
            previousButton.onClick.AddListener(() => SceneManager.LoadSceneAsync(previousSceneName, LoadSceneMode.Single)); nextButton.onClick.AddListener(OpenFinalConfirmation);
            AddFocus(previousButton.gameObject, previousButton.GetComponent<Outline>(), () => RestoreButton(previousButton)); AddFocus(nextButton.gameObject, nextButton.GetComponent<Outline>(), () => RestoreButton(nextButton)); tabControls.Add(previousButton); tabControls.Add(nextButton);
            noticeLabel = Text(parent, "Notice", "", font, 17, new Vector2(.53f, .045f), new Vector2(700, 28)); noticeLabel.color = new Color(1, .77f, .5f, 1);
            Text help = Text(parent, "Help", "Tab / Shift+Tab / 방향키: 이동   Enter / Space: 선택", font, 15, new Vector2(.53f, .015f), new Vector2(760, 24)); help.color = new Color(.7f, .77f, .86f, 1);
        }

        private void OpenFinalConfirmation()
        {
            if (string.IsNullOrEmpty(selectedJobId)) { noticeLabel.text = "직업을 선택해주세요."; return; }
            SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Single);
        }
        private void ConfigureNavigation() { for (int i = 0; i < jobButtons.Count; i++) jobButtons[i].navigation = Nav(null, null, i > 0 ? jobButtons[i - 1] : null, i + 1 < jobButtons.Count ? jobButtons[i + 1] : previousButton); Selectable last = jobButtons.Count > 0 ? jobButtons[jobButtons.Count - 1] : null; previousButton.navigation = Nav(null, nextButton, last, null); nextButton.navigation = Nav(previousButton, null, last, null); }
        private Button GetInitialFocus() { int i = Array.FindIndex(jobDefinitions, job => job.JobId == selectedJobId); return jobButtons[i >= 0 ? i : 0]; }
        private void MoveTab(int direction) { int i = tabControls.FindIndex(c => c.gameObject == EventSystem.current.currentSelectedGameObject); EventSystem.current.SetSelectedGameObject(tabControls[(i + direction + tabControls.Count) % tabControls.Count].gameObject); }
        private void AddFocus(GameObject target, Outline outline, Action restore) { EventTrigger trigger = target.AddComponent<EventTrigger>(); AddTrigger(trigger, EventTriggerType.Select, _ => { outline.effectColor = focusColor; outline.effectDistance = new Vector2(5, -5); }); AddTrigger(trigger, EventTriggerType.Deselect, _ => restore()); }
        private static void AddTrigger(EventTrigger trigger, EventTriggerType type, Action<BaseEventData> action) { EventTrigger.Entry entry = new EventTrigger.Entry { eventID = type }; entry.callback.AddListener(data => action(data)); trigger.triggers.Add(entry); }
        private void RestoreButton(Button button) { Outline o = button.GetComponent<Outline>(); o.effectColor = accentColor; o.effectDistance = new Vector2(2, -2); }
        private static Navigation Nav(Selectable left, Selectable right, Selectable up, Selectable down) => new Navigation { mode = Navigation.Mode.Explicit, selectOnLeft = left, selectOnRight = right, selectOnUp = up, selectOnDown = down };
        private static string StatName(CharacterStatType stat) => new[] { "체력", "힘", "민첩", "감각", "지능", "의지" }[(int)stat];
        private static void CreateEventSystem() { if (EventSystem.current != null) return; InputSystemUIInputModule module = new GameObject("EventSystem", typeof(EventSystem)).AddComponent<InputSystemUIInputModule>(); module.AssignDefaultActions(); }
        private static Button TextButton(Transform parent, string name, string label, Font font, Vector2 anchor) { GameObject obj = new GameObject(name, typeof(Image), typeof(Button), typeof(Outline)); obj.transform.SetParent(parent, false); SetRect(obj.GetComponent<RectTransform>(), anchor, new Vector2(230, 52)); Image image = obj.GetComponent<Image>(); image.color = new Color(.12f, .32f, .5f, 1); Button button = obj.GetComponent<Button>(); button.targetGraphic = image; button.colors = CtaColors(); Outline outline = obj.GetComponent<Outline>(); outline.effectColor = new Color(.88f, .7f, .32f, 1); outline.effectDistance = new Vector2(2, -2); Text text = Text(obj.transform, "Label", $"[ {label} ]", font, 22, Vector2.one * .5f, new Vector2(210, 44)); text.color = new Color(.98f, .94f, .82f, 1); text.fontStyle = FontStyle.Bold; return button; }
        private static ColorBlock CtaColors() { ColorBlock c = ColorBlock.defaultColorBlock; c.normalColor = Color.white; c.highlightedColor = new Color(1.25f, 1.18f, 1.08f, 1); c.selectedColor = new Color(1.18f, 1.12f, 1.02f, 1); c.pressedColor = new Color(.62f, .72f, .82f, 1); c.disabledColor = new Color(.48f, .52f, .58f, .72f); return c; }
        private static ColorBlock SelectableColors() { ColorBlock c = ColorBlock.defaultColorBlock; c.normalColor = Color.white; c.highlightedColor = new Color(.82f, .88f, .98f, 1); c.selectedColor = new Color(.92f, .82f, .58f, 1); c.pressedColor = new Color(.68f, .68f, .68f, 1); return c; }
        private static void CreateSteps(Transform parent, Font font) { string[] steps = { "1 기본 정보", "2 길", "3 직업 · 현재", "4 확인" }; for (int i = 0; i < steps.Length; i++) { bool current = i == 2; Image panel = Image(parent, $"Step{i + 1}", current ? new Color(.18f, .25f, .34f, 1) : new Color(.04f, .065f, .11f, .88f)); SetRect(panel.rectTransform, new Vector2(.35f + i * .1f, .975f), new Vector2(122, 28)); AddOutline(panel.gameObject, current ? new Color(.95f, .76f, .36f, 1) : new Color(.25f, .32f, .42f, 1), current ? 2 : 1); Text(panel.transform, "Label", steps[i], font, 15, Vector2.one * .5f, new Vector2(118, 26)); } }
        private static Text Text(Transform parent, string name, string value, Font font, int size, Vector2 anchor, Vector2 dimensions) { GameObject obj = new GameObject(name, typeof(Text)); obj.transform.SetParent(parent, false); Text text = obj.GetComponent<Text>(); text.font = font; text.fontSize = size; text.color = Color.white; text.alignment = TextAnchor.MiddleCenter; text.text = value; text.raycastTarget = false; SetRect(text.rectTransform, anchor, dimensions); return text; }
        private static Image Image(Transform parent, string name, Color color) { GameObject obj = new GameObject(name, typeof(Image)); obj.transform.SetParent(parent, false); Image image = obj.GetComponent<Image>(); image.color = color; image.raycastTarget = false; return image; }
        private static Outline AddOutline(GameObject target, Color color, float distance) { Outline outline = target.AddComponent<Outline>(); outline.effectColor = color; outline.effectDistance = new Vector2(distance, -distance); return outline; }
        private static void SetRect(RectTransform rect, Vector2 anchor, Vector2 size) { rect.anchorMin = anchor; rect.anchorMax = anchor; rect.pivot = Vector2.one * .5f; rect.anchoredPosition = Vector2.zero; rect.sizeDelta = size; }
        private static void Stretch(RectTransform rect) { rect.anchorMin = Vector2.zero; rect.anchorMax = Vector2.one; rect.offsetMin = Vector2.zero; rect.offsetMax = Vector2.zero; }
    }
}

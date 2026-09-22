using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// 월드에서 Q 또는 게임패드 View/Back 버튼으로 여는 퀘스트 목록입니다.
    /// 왼쪽에는 진행 중인 퀘스트 제목만, 오른쪽에는 선택한 데이터의 상세 내용을 표시합니다.
    /// </summary>
    public sealed class QuestLogPresenter : MonoBehaviour
    {
        private static readonly Color PanelColor = new Color(0.055f, 0.065f, 0.09f, 0.98f);
        private static readonly Color RowColor = new Color(0.13f, 0.17f, 0.24f, 1f);
        private static readonly Color SelectedColor = new Color(0.34f, 0.27f, 0.12f, 1f);
        private static readonly Color Gold = new Color(0.82f, 0.66f, 0.25f, 1f);

        public static QuestLogPresenter Instance { get; private set; }

        private readonly List<Button> questButtons = new List<Button>();
        private readonly List<QuestRuntimeState> displayedQuests = new List<QuestRuntimeState>();
        private readonly List<Selectable> focusOrder = new List<Selectable>();
        private GameObject panel;
        private ScrollRect listScroll;
        private RectTransform listContent;
        private Text detailsText;
        private Text emptyText;
        private Button closeButton;
        private Button trackButton;
        private QuestRuntimeState selectedQuest;
        private InputAction toggleAction;
        private InputAction cancelAction;
        private InputAction navigateAction;
        private InputAction trackAction;

        public bool IsOpen => panel != null && panel.activeSelf;
        public int VisibleQuestCount => displayedQuests.Count;
        public string SelectedQuestId => selectedQuest?.Definition.QuestId ?? string.Empty;
        public string Details => detailsText?.text ?? string.Empty;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Register()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (!scene.name.StartsWith("World_", StringComparison.Ordinal)
                && !scene.name.StartsWith("Field_", StringComparison.Ordinal)) return;
            if (Instance == null) new GameObject("QuestLogSystem").AddComponent<QuestLogPresenter>();
        }

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            CreateUi();

            toggleAction = new InputAction("ToggleQuestLog", InputActionType.Button);
            toggleAction.AddBinding("<Keyboard>/q");
            // 현재 Input System Gamepad 레이아웃에서 View/Back 계열의 실제 control path는 select입니다.
            toggleAction.AddBinding("<Gamepad>/select");
            toggleAction.performed += _ => { if (IsOpen) Close(); else Open(); };

            cancelAction = new InputAction("CloseQuestLog", InputActionType.Button);
            cancelAction.AddBinding("<Keyboard>/escape");
            cancelAction.AddBinding("<Gamepad>/buttonEast");
            cancelAction.performed += _ => { if (IsOpen) Close(); };

            navigateAction = new InputAction("NavigateQuestList", InputActionType.Value);
            InputActionSetupExtensions.CompositeSyntax keys = navigateAction.AddCompositeBinding("2DVector");
            keys.With("Up", "<Keyboard>/upArrow"); keys.With("Down", "<Keyboard>/downArrow");
            navigateAction.AddBinding("<Gamepad>/dpad");
            navigateAction.AddBinding("<Gamepad>/leftStick");
            navigateAction.performed += context =>
            {
                if (!IsOpen) return;
                float vertical = context.ReadValue<Vector2>().y;
                if (Mathf.Abs(vertical) > 0.5f) MoveQuestSelection(vertical > 0f ? -1 : 1);
            };

            trackAction = new InputAction("TrackSelectedQuest", InputActionType.Button);
            trackAction.AddBinding("<Keyboard>/t");
            trackAction.AddBinding("<Gamepad>/buttonNorth");
            trackAction.performed += _ => { if (IsOpen) TrackSelectedQuest(); };
        }

        private void OnEnable()
        {
            // Editor 감사 뒤 Domain Reload가 생겨도 Scene에 남은 정상 인스턴스를 다시 공용 진입점으로 연결합니다.
            if (Instance == null) Instance = this;
            toggleAction?.Enable();
            cancelAction?.Enable();
            navigateAction?.Enable();
            trackAction?.Enable();
            QuestService.Changed += RefreshWhileOpen;
        }

        private void OnDisable()
        {
            toggleAction?.Disable();
            cancelAction?.Disable();
            navigateAction?.Disable();
            trackAction?.Disable();
            QuestService.Changed -= RefreshWhileOpen;
            ReleaseModal();
        }

        private void OnDestroy()
        {
            ReleaseModal();
            toggleAction?.Dispose();
            cancelAction?.Dispose();
            navigateAction?.Dispose();
            trackAction?.Dispose();
            if (Instance == this) Instance = null;
        }

        private void Update()
        {
            if (!IsOpen) return;
            Keyboard keyboard = Keyboard.current;
            if (keyboard != null && keyboard.tabKey.wasPressedThisFrame)
                MoveFocus(keyboard.leftShiftKey.isPressed || keyboard.rightShiftKey.isPressed ? -1 : 1);
            KeepSelectedRowVisible();
        }

        /// <summary>다른 월드 Modal이 없을 때만 열고 플레이어 이동과 월드 HUD를 잠급니다.</summary>
        public void Open()
        {
            if (!WorldModalState.TryAcquire(this)) return;
            RefreshList();
            panel.SetActive(true);
            panel.transform.parent.SetAsLastSibling();
            WorldExperienceHud.SetInteractionUiOpen(this, true);
            PlayerController.SetMovementLocked(this, true);
            SelectDefaultQuest();
        }

        public void Close()
        {
            if (EventSystem.current?.currentSelectedGameObject != null
                && EventSystem.current.currentSelectedGameObject.transform.IsChildOf(transform))
                EventSystem.current.SetSelectedGameObject(null);
            panel?.SetActive(false);
            selectedQuest = null;
            ReleaseModal();
        }

        private void ReleaseModal()
        {
            WorldExperienceHud.SetInteractionUiOpen(this, false);
            PlayerController.SetMovementLocked(this, false);
            WorldModalState.Release(this);
        }

        private void RefreshWhileOpen()
        {
            if (!IsOpen) return;
            string previousId = SelectedQuestId;
            RefreshList();
            int previousIndex = displayedQuests.FindIndex(x => x.Definition.QuestId == previousId);
            if (previousIndex >= 0) SelectQuest(previousIndex);
            else SelectDefaultQuest();
        }

        /// <summary>실행 중인 수만큼 행을 다시 만들며 Main과 Side를 텍스트 Section으로 구분합니다.</summary>
        public void RefreshList()
        {
            foreach (Transform child in listContent) Destroy(child.gameObject);
            questButtons.Clear();
            displayedQuests.Clear();
            focusOrder.Clear();

            QuestRuntimeState main = QuestService.ActiveMainQuest;
            QuestRuntimeState[] sides = QuestService.ActiveSideQuests.OrderBy(x => x.Definition.DisplayName).ToArray();
            if (main != null)
            {
                CreateSection("[메인 퀘스트]");
                CreateQuestRow(main);
            }
            if (sides.Length > 0)
            {
                CreateSection("[서브 퀘스트]");
                foreach (QuestRuntimeState side in sides) CreateQuestRow(side);
            }

            focusOrder.Add(closeButton);
            focusOrder.Add(trackButton);
            emptyText.gameObject.SetActive(displayedQuests.Count == 0);
            detailsText.text = displayedQuests.Count == 0 ? "진행 중인 퀘스트가 없습니다." : string.Empty;
            Canvas.ForceUpdateCanvases();
            listScroll.verticalNormalizedPosition = 1f;
        }

        private void SelectDefaultQuest()
        {
            if (displayedQuests.Count == 0)
            {
                EventSystem.current?.SetSelectedGameObject(closeButton.gameObject);
                return;
            }

            SelectQuest(0);
        }

        private void CreateSection(string label)
        {
            GameObject section = new GameObject(label, typeof(RectTransform), typeof(CanvasRenderer), typeof(Text), typeof(LayoutElement));
            section.transform.SetParent(listContent, false);
            Text text = section.GetComponent<Text>();
            ConfigureText(text, 24, Gold, TextAnchor.MiddleLeft);
            text.fontStyle = FontStyle.Bold;
            text.text = label;
            text.raycastTarget = false;
            LayoutElement layout = section.GetComponent<LayoutElement>();
            layout.minHeight = 42f; layout.preferredHeight = 42f;
        }

        private void CreateQuestRow(QuestRuntimeState state)
        {
            int index = displayedQuests.Count;
            displayedQuests.Add(state);
            GameObject rowObject = new GameObject("Quest_" + state.Definition.QuestId,
                typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button), typeof(LayoutElement));
            rowObject.transform.SetParent(listContent, false);
            Image image = rowObject.GetComponent<Image>(); image.color = RowColor;
            Button button = rowObject.GetComponent<Button>();
            button.targetGraphic = image;
            ColorBlock colors = button.colors;
            colors.highlightedColor = new Color(0.22f, 0.28f, 0.38f, 1f);
            colors.selectedColor = SelectedColor;
            colors.pressedColor = new Color(0.42f, 0.34f, 0.14f, 1f);
            button.colors = colors;
            button.navigation = new Navigation { mode = Navigation.Mode.None };
            LayoutElement layout = rowObject.GetComponent<LayoutElement>();
            layout.minHeight = 54f; layout.preferredHeight = 54f;

            GameObject labelObject = new GameObject("Label", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
            labelObject.transform.SetParent(rowObject.transform, false);
            Text label = labelObject.GetComponent<Text>();
            ConfigureText(label, 25, Color.white, TextAnchor.MiddleLeft);
            label.text = (IsTracked(state) ? "◆ " : "  ") + state.Definition.DisplayName;
            label.raycastTarget = false;
            Stretch(label.rectTransform, 16f, 10f, -16f, -10f);
            button.onClick.AddListener(() => SelectQuest(index));
            EventTrigger trigger = rowObject.AddComponent<EventTrigger>();
            var entry = new EventTrigger.Entry { eventID = EventTriggerType.Select };
            entry.callback.AddListener(_ => SelectQuest(index, false));
            trigger.triggers.Add(entry);
            questButtons.Add(button);
            focusOrder.Add(button);
        }

        private void SelectQuest(int index) => SelectQuest(index, true);

        private void SelectQuest(int index, bool moveFocus)
        {
            if (index < 0 || index >= displayedQuests.Count) return;
            selectedQuest = displayedQuests[index];
            for (int i = 0; i < questButtons.Count; i++)
            {
                questButtons[i].GetComponent<Image>().color = i == index ? SelectedColor : RowColor;
                string tracked = IsTracked(displayedQuests[i]) ? "◆ " : "";
                questButtons[i].GetComponentInChildren<Text>().text = (i == index ? "▶ " : "  ") + tracked
                    + displayedQuests[i].Definition.DisplayName;
            }
            detailsText.text = BuildDetails(selectedQuest);
            if (moveFocus) EventSystem.current?.SetSelectedGameObject(questButtons[index].gameObject);
            KeepSelectedRowVisible();
        }

        private static string BuildDetails(QuestRuntimeState state)
        {
            QuestDefinition definition = state.Definition;
            var text = new StringBuilder();
            text.AppendLine(definition.DisplayName).AppendLine();
            text.AppendLine(definition.QuestType == QuestType.Main ? "[메인 퀘스트]" : "[서브 퀘스트]").AppendLine();
            text.AppendLine("설명");
            text.AppendLine(string.IsNullOrWhiteSpace(definition.Description) ? "등록된 설명이 없습니다." : definition.Description);
            text.AppendLine().AppendLine("현재 목표");
            text.AppendLine(state.CurrentObjective?.Description ?? "완료 보고를 기다리고 있습니다.");
            text.AppendLine(IsTracked(state) ? "◆ 현재 위치 안내 중" : "[T / 게임패드 Y] 이 퀘스트 추적");
            if (definition.Objectives.Count > 1)
                text.AppendLine($"진행 단계  {Mathf.Min(state.CurrentObjectiveIndex + 1, definition.Objectives.Count)} / {definition.Objectives.Count}");
            text.AppendLine().AppendLine("보상");
            text.Append(BuildReward(definition.Reward));
            return text.ToString();
        }

        private static string BuildReward(RewardBundle reward)
        {
            if (reward == null) return "없음";
            var parts = new List<string>();
            if (reward.Experience > 0) parts.Add($"EXP {reward.Experience}");
            if (reward.Currency > 0) parts.Add(CurrencyPresentation.FormatAmount(reward.Currency));
            if (reward.Items != null)
            {
                foreach (ItemReward item in reward.Items)
                {
                    if (item == null || item.Count <= 0) continue;
                    string name = ItemCatalog.TryGet(item.ItemId, out ItemDefinition definition)
                        ? definition.DisplayName : item.ItemId;
                    parts.Add($"{name} × {item.Count}");
                }
            }
            return parts.Count == 0 ? "없음" : string.Join("\n", parts);
        }

        private static bool IsTracked(QuestRuntimeState state)
            => state != null && QuestService.GetTrackedQuest()?.Definition.QuestId == state.Definition.QuestId;

        private void MoveFocus(int direction)
        {
            if (focusOrder.Count == 0) return;
            int index = focusOrder.FindIndex(x => x.gameObject == EventSystem.current?.currentSelectedGameObject);
            index = (index + direction + focusOrder.Count) % focusOrder.Count;
            EventSystem.current?.SetSelectedGameObject(focusOrder[index].gameObject);
        }

        private void MoveQuestSelection(int direction)
        {
            if (questButtons.Count == 0) return;
            int index = questButtons.FindIndex(x => x.gameObject == EventSystem.current?.currentSelectedGameObject);
            if (index < 0) index = Mathf.Max(0, displayedQuests.IndexOf(selectedQuest));
            SelectQuest((index + direction + questButtons.Count) % questButtons.Count);
        }

        private void KeepSelectedRowVisible()
        {
            if (displayedQuests.Count <= 1 || EventSystem.current?.currentSelectedGameObject == null) return;
            int index = questButtons.FindIndex(x => x.gameObject == EventSystem.current.currentSelectedGameObject);
            if (index < 0) return;
            listScroll.verticalNormalizedPosition = 1f - (float)index / (displayedQuests.Count - 1);
        }

        private void CreateUi()
        {
            EnsureEventSystem();
            GameObject canvasObject = new GameObject("QuestLogCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasObject.transform.SetParent(transform, false);
            Canvas canvas = canvasObject.GetComponent<Canvas>(); canvas.renderMode = RenderMode.ScreenSpaceOverlay; canvas.sortingOrder = 20;
            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize; scaler.referenceResolution = new Vector2(1280f, 720f); scaler.matchWidthOrHeight = 0.5f;

            panel = new GameObject("QuestLogPanel", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Outline));
            panel.transform.SetParent(canvasObject.transform, false);
            RectTransform panelRect = panel.GetComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0.08f, 0.08f); panelRect.anchorMax = new Vector2(0.92f, 0.92f);
            panelRect.offsetMin = Vector2.zero; panelRect.offsetMax = Vector2.zero;
            panel.GetComponent<Image>().color = PanelColor;
            Outline outline = panel.GetComponent<Outline>(); outline.effectColor = Gold; outline.effectDistance = new Vector2(2f, -2f);

            Text title = CreateText(panel.transform, "Title", "퀘스트", 34, Color.white, TextAnchor.MiddleCenter);
            SetRect(title.rectTransform, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -38f), new Vector2(-48f, 62f));

            CreateList(panel.transform);
            CreateDetails(panel.transform);
            closeButton = CreateButton(panel.transform, "CloseButton", "닫기  [Q / Esc / 게임패드 B]", new Vector2(320f, 48f));
            closeButton.navigation = new Navigation { mode = Navigation.Mode.None };
            closeButton.onClick.AddListener(Close);
            SetRect(closeButton.GetComponent<RectTransform>(), new Vector2(0.18f, 0f), new Vector2(0.18f, 0f),
                new Vector2(0.5f, 0f), new Vector2(0f, 26f), new Vector2(320f, 48f));
            trackButton = CreateButton(panel.transform, "TrackButton", "추적  [T / 게임패드 Y]", new Vector2(300f, 48f));
            trackButton.navigation = new Navigation { mode = Navigation.Mode.None };
            trackButton.onClick.AddListener(TrackSelectedQuest);
            SetRect(trackButton.GetComponent<RectTransform>(), new Vector2(0.82f, 0f), new Vector2(0.82f, 0f),
                new Vector2(0.5f, 0f), new Vector2(0f, 26f), new Vector2(300f, 48f));
            panel.SetActive(false);
        }

        private void TrackSelectedQuest()
        {
            if (selectedQuest == null || !QuestService.TryTrack(selectedQuest.Definition.QuestId)) return;
            RefreshWhileOpen();
        }

        private void CreateList(Transform parent)
        {
            GameObject listPanel = new GameObject("QuestList", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            listPanel.transform.SetParent(parent, false);
            RectTransform listRect = listPanel.GetComponent<RectTransform>();
            listRect.anchorMin = new Vector2(0f, 0f); listRect.anchorMax = new Vector2(0.36f, 1f);
            listRect.offsetMin = new Vector2(24f, 86f); listRect.offsetMax = new Vector2(-8f, -82f);
            listPanel.GetComponent<Image>().color = new Color(0.075f, 0.09f, 0.13f, 1f);

            GameObject scrollObject = new GameObject("ScrollView", typeof(RectTransform), typeof(ScrollRect));
            scrollObject.transform.SetParent(listPanel.transform, false); Stretch(scrollObject.GetComponent<RectTransform>(), 12f, 12f, -12f, -12f);
            GameObject viewport = new GameObject("Viewport", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Mask));
            viewport.transform.SetParent(scrollObject.transform, false); Stretch(viewport.GetComponent<RectTransform>(), 0f, 0f, 0f, 0f);
            viewport.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.01f); viewport.GetComponent<Mask>().showMaskGraphic = false;
            GameObject content = new GameObject("Content", typeof(RectTransform), typeof(VerticalLayoutGroup), typeof(ContentSizeFitter));
            content.transform.SetParent(viewport.transform, false);
            listContent = content.GetComponent<RectTransform>();
            listContent.anchorMin = new Vector2(0f, 1f); listContent.anchorMax = new Vector2(1f, 1f); listContent.pivot = new Vector2(0.5f, 1f);
            listContent.offsetMin = Vector2.zero; listContent.offsetMax = Vector2.zero;
            VerticalLayoutGroup layout = content.GetComponent<VerticalLayoutGroup>();
            layout.spacing = 8f; layout.padding = new RectOffset(4, 4, 4, 4); layout.childControlHeight = true; layout.childControlWidth = true; layout.childForceExpandHeight = false;
            content.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            listScroll = scrollObject.GetComponent<ScrollRect>();
            listScroll.viewport = viewport.GetComponent<RectTransform>(); listScroll.content = listContent; listScroll.horizontal = false; listScroll.vertical = true; listScroll.scrollSensitivity = 34f;

            emptyText = CreateText(viewport.transform, "EmptyMessage", "진행 중인 퀘스트가 없습니다.", 24, Color.white, TextAnchor.MiddleCenter);
            Stretch(emptyText.rectTransform, 20f, 20f, -20f, -20f);
        }

        private void CreateDetails(Transform parent)
        {
            GameObject detailPanel = new GameObject("QuestDetails", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            detailPanel.transform.SetParent(parent, false);
            RectTransform rect = detailPanel.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.36f, 0f); rect.anchorMax = Vector2.one;
            rect.offsetMin = new Vector2(8f, 24f); rect.offsetMax = new Vector2(-24f, -82f);
            detailPanel.GetComponent<Image>().color = new Color(0.075f, 0.09f, 0.13f, 1f);
            detailsText = CreateText(detailPanel.transform, "DetailsText", string.Empty, 25, Color.white, TextAnchor.UpperLeft);
            detailsText.horizontalOverflow = HorizontalWrapMode.Wrap; detailsText.verticalOverflow = VerticalWrapMode.Overflow;
            detailsText.lineSpacing = 1.15f; Stretch(detailsText.rectTransform, 28f, 24f, -28f, -24f);
        }

        private static Button CreateButton(Transform parent, string name, string label, Vector2 size)
        {
            GameObject buttonObject = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
            buttonObject.transform.SetParent(parent, false);
            if (size != Vector2.zero) buttonObject.GetComponent<RectTransform>().sizeDelta = size;
            buttonObject.GetComponent<Image>().color = RowColor;
            Text text = CreateText(buttonObject.transform, "Label", label, 22, Color.white, TextAnchor.MiddleCenter);
            Stretch(text.rectTransform, 8f, 4f, -8f, -4f); text.raycastTarget = false;
            return buttonObject.GetComponent<Button>();
        }

        private static Text CreateText(Transform parent, string name, string value, int size, Color color, TextAnchor anchor)
        {
            GameObject textObject = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
            textObject.transform.SetParent(parent, false);
            Text text = textObject.GetComponent<Text>(); ConfigureText(text, size, color, anchor); text.text = value; text.raycastTarget = false;
            return text;
        }

        private static void ConfigureText(Text text, int size, Color color, TextAnchor anchor)
        {
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"); text.fontSize = size; text.color = color; text.alignment = anchor;
        }

        private static void EnsureEventSystem()
        {
            if (EventSystem.current != null) return;
            InputSystemUIInputModule module = new GameObject("EventSystem", typeof(EventSystem)).AddComponent<InputSystemUIInputModule>();
            module.AssignDefaultActions();
        }

        private static void Stretch(RectTransform rect, float left, float bottom, float right, float top)
        {
            rect.anchorMin = Vector2.zero; rect.anchorMax = Vector2.one; rect.offsetMin = new Vector2(left, bottom); rect.offsetMax = new Vector2(right, top);
        }

        private static void SetRect(RectTransform rect, Vector2 min, Vector2 max, Vector2 pivot, Vector2 position, Vector2 size)
        {
            rect.anchorMin = min; rect.anchorMax = max; rect.pivot = pivot; rect.anchoredPosition = position; rect.sizeDelta = size;
        }
    }
}

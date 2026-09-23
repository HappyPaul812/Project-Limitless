using System.Collections.Generic;
using System.Linq;
using ProjectLimitless.Battle;
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
    /// <summary>안전지역 NPC에서만 여는 편성 초안 화면입니다. 취소 시 저장된 명단·진형은 바뀌지 않습니다.</summary>
    public sealed class PartyManagementPresenter : MonoBehaviour
    {
        public static PartyManagementPresenter Instance { get; private set; }
        private readonly List<string> draft = new List<string>();
        private readonly Dictionary<string, FormationRow> rows = new Dictionary<string, FormationRow>();
        private readonly List<Button> focus = new List<Button>();
        private readonly Dictionary<string, Text> memberLabels = new Dictionary<string, Text>();
        private GameObject panel;
        private RectTransform content;
        private ScrollRect scroll;
        private Text summary, message;
        private Button confirm, cancel;
        private InputAction closeAction;
        private Transform source;
        private bool warning;
        private GameObject previousFocus;
        public bool IsOpen => panel != null && panel.activeSelf;
        public bool IsWarningVisible => IsOpen && warning;
        public IReadOnlyList<string> DraftIds => draft;
        public string Summary => summary == null ? "" : summary.text;

        public static bool OpenAt(Transform safeAreaSource)
        {
            // 향후 캠프는 같은 진입점의 안전지역 정책을 확장합니다. 필드 단축키는 제공하지 않습니다.
            if (safeAreaSource == null || safeAreaSource.gameObject.scene.name != "World_StarterVillage"
                || SceneManager.GetActiveScene().name != "World_StarterVillage") return false;
            if (Instance == null) new GameObject("PartyManagementSystem").AddComponent<PartyManagementPresenter>();
            return Instance.Open(safeAreaSource);
        }

        private void Awake()
        {
            Instance = this; CreateUi();
            closeAction = new InputAction("ClosePartyManagement", InputActionType.Button);
            closeAction.AddBinding("<Keyboard>/escape"); closeAction.AddBinding("<Gamepad>/buttonEast");
            closeAction.performed += _ => { if (IsOpen) Cancel(); };
        }
        private void OnEnable() => closeAction?.Enable();
        private void OnDisable() { closeAction?.Disable(); Close(); }
        private void OnDestroy() { Close(); closeAction?.Dispose(); if (Instance == this) Instance = null; }

        private bool Open(Transform safeAreaSource)
        {
            if (!WorldModalState.TryAcquire(this)) return false;
            source = safeAreaSource; warning = false;
            draft.Clear(); draft.AddRange(CompanionRosterService.ActivePartyCharacterIds);
            rows.Clear(); rows["player"] = CompanionRosterService.GetRow("player");
            foreach (CompanionDefinition member in CompanionCatalog.All.Where(x => CompanionRosterService.IsUnlocked(x.CharacterId)))
                rows[member.CharacterId] = CompanionRosterService.GetRow(member.CharacterId);
            BuildRows(); panel.SetActive(true); Refresh();
            PlayerController.SetMovementLocked(this, true); WorldExperienceHud.SetInteractionUiOpen(this, true);
            EventSystem.current?.SetSelectedGameObject(focus[0].gameObject);
            return true;
        }

        public void Close()
        {
            if (EventSystem.current?.currentSelectedGameObject != null
                && EventSystem.current.currentSelectedGameObject.transform.IsChildOf(transform)) EventSystem.current.SetSelectedGameObject(null);
            panel?.SetActive(false); warning = false; source = null;
            WorldModalState.Release(this); PlayerController.SetMovementLocked(this, false);
            WorldExperienceHud.SetInteractionUiOpen(this, false);
        }

        private void Update()
        {
            if (!IsOpen) return;
            if (source == null || SceneManager.GetActiveScene().name != "World_StarterVillage") { Close(); return; }
            if (Keyboard.current?.tabKey.wasPressedThisFrame == true)
            {
                var usable = focus.Where(x => x.interactable).ToArray();
                int index = System.Array.FindIndex(usable, x => x.gameObject == EventSystem.current?.currentSelectedGameObject);
                int direction = Keyboard.current.shiftKey.isPressed ? -1 : 1;
                EventSystem.current?.SetSelectedGameObject(usable[(index + direction + usable.Length) % usable.Length].gameObject);
            }
            var selected = EventSystem.current?.currentSelectedGameObject;
            if (selected != null && selected != previousFocus && selected.transform.IsChildOf(content))
            {
                int index = selected.transform.GetSiblingIndex();
                if (content.childCount > 1) scroll.verticalNormalizedPosition = 1f - (float)index / (content.childCount - 1);
            }
            previousFocus = selected;
        }

        public void ToggleCompanion(string id)
        {
            if (!IsOpen || warning || !rows.ContainsKey(id) || id == "player") return;
            if (draft.Contains(id)) draft.Remove(id);
            else if (draft.Count < CompanionRosterService.SoloCompanionLimit) draft.Add(id);
            else { message.text = "동료는 최대 2명입니다. 먼저 선택한 동료를 해제해주세요."; return; }
            Refresh();
        }

        public void ToggleRow(string id)
        {
            if (!IsOpen || warning || !rows.ContainsKey(id)) return;
            rows[id] = rows[id] == FormationRow.Front ? FormationRow.Rear : FormationRow.Front;
            Refresh();
        }

        public void Confirm()
        {
            if (!IsOpen || source == null || SceneManager.GetActiveScene().name != "World_StarterVillage") return;
            if (!warning && !CompanionRosterService.HasOffensiveRole(GameSessionData.SelectedJobId, draft))
            {
                warning = true; message.text = "공격 역할의 동료가 없습니다.\n전투 시간이 길어질 수 있습니다.";
                foreach (Button button in focus) button.interactable = button == confirm || button == cancel;
                confirm.GetComponentInChildren<Text>().text = "그래도 편성";
                cancel.GetComponentInChildren<Text>().text = "돌아가기";
                EventSystem.current?.SetSelectedGameObject(cancel.gameObject); return;
            }
            if (!CompanionRosterService.TrySetComposition(draft, rows)) { message.text = "편성 정보를 다시 확인해주세요."; return; }
            if (GameSaveService.CurrentSlotIndex > 0 && !GameSaveService.SaveCurrentSession())
            { message.text = "현재 편성은 적용되었지만 저장에 실패했습니다. 다시 확정해 저장을 시도해주세요."; return; }
            Close(); QuestHudPresenter.Notify("파티 편성을 적용했습니다.");
        }

        public void Cancel()
        {
            if (warning) { warning = false; Refresh(); EventSystem.current?.SetSelectedGameObject(confirm.gameObject); }
            else Close();
        }

        private void Refresh()
        {
            summary.text = "현재 편성 · Player 고정 + 동료 " + draft.Count + "/2\n"
                + GameSessionData.PlayerName + " (Player) · " + RowLabel(rows["player"]) + "\n"
                + string.Join("\n", draft.Select(id => CompanionCatalog.Find(id).DisplayName + " · " + RowLabel(rows[id])));
            foreach (var pair in memberLabels)
            {
                var definition = CompanionCatalog.Find(pair.Key);
                bool player = pair.Key == "player";
                pair.Value.text = player ? "[고정] " + GameSessionData.PlayerName + " · Player · " + RowLabel(rows[pair.Key])
                    : (draft.Contains(pair.Key) ? "[✓ 선택] " : "[미선택] ") + definition.DisplayName + " · "
                        + definition.JobName + " · " + definition.PathName + " · " + RowLabel(rows[pair.Key]);
            }
            foreach (Button button in focus) button.interactable = true;
            confirm.GetComponentInChildren<Text>().text = "편성 확정"; cancel.GetComponentInChildren<Text>().text = "취소";
            message.text = "동료 이름: 선택/해제    배치 버튼: 전열/후열\n확정 전 변경은 저장되지 않습니다. Tab·방향키 / 게임패드 이동·확인·취소";
        }
        private static string RowLabel(FormationRow row) => row == FormationRow.Front ? "전열" : "후열";

        private void BuildRows()
        {
            foreach (Transform child in content) { child.gameObject.SetActive(false); Destroy(child.gameObject); }
            focus.Clear(); memberLabels.Clear();
            AddMember("player");
            foreach (CompanionDefinition definition in CompanionCatalog.All.Where(x => CompanionRosterService.IsUnlocked(x.CharacterId))) AddMember(definition.CharacterId);
            focus.Add(confirm); focus.Add(cancel);
        }

        private void AddMember(string id)
        {
            Button select = MakeButton(content, id, "", () => { if (id == "player") ToggleRow(id); else ToggleCompanion(id); });
            select.gameObject.AddComponent<LayoutElement>().preferredHeight = 60;
            memberLabels[id] = select.GetComponentInChildren<Text>(); focus.Add(select);
            Sprite portrait = DialoguePortraitCatalog.GetPortrait(id);
            if (portrait != null)
            {
                var imageObject = new GameObject("Portrait", typeof(RectTransform), typeof(Image));
                imageObject.transform.SetParent(select.transform, false);
                Image image = imageObject.GetComponent<Image>(); image.sprite = portrait; image.preserveAspect = true; image.raycastTarget = false;
                Rect(image.rectTransform,0,0,.12f,1); Rect(memberLabels[id].rectTransform,.13f,0,.99f,1);
            }
            Button row = MakeButton(content, id + "_Row", "↔ 전열 / 후열 변경", () => ToggleRow(id));
            row.gameObject.AddComponent<LayoutElement>().preferredHeight = 40; focus.Add(row);
        }

        private void CreateUi()
        {
            if (EventSystem.current == null)
                new GameObject("EventSystem", typeof(EventSystem)).AddComponent<InputSystemUIInputModule>().AssignDefaultActions();
            var canvasObject = new GameObject("PartyCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasObject.transform.SetParent(transform, false);
            canvasObject.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObject.GetComponent<Canvas>().sortingOrder = 22;
            var scaler = canvasObject.GetComponent<CanvasScaler>(); scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1280,720); scaler.matchWidthOrHeight = .5f;
            panel = new GameObject("PartyPanel", typeof(RectTransform), typeof(Image)); panel.transform.SetParent(canvasObject.transform, false);
            panel.GetComponent<Image>().color = new Color(.055f,.065f,.09f,.98f);
            Rect(panel.GetComponent<RectTransform>(), .07f,.07f,.93f,.93f);
            Text title = MakeText(panel.transform, "파티 편성", 32); Rect(title.rectTransform,.03f,.87f,.97f,.98f);
            summary = MakeText(panel.transform, "", 24); Rect(summary.rectTransform,.03f,.38f,.36f,.85f);
            message = MakeText(panel.transform, "", 20); Rect(message.rectTransform,.03f,.12f,.97f,.3f);
            var root = new GameObject("RosterScroll", typeof(RectTransform), typeof(ScrollRect)); root.transform.SetParent(panel.transform,false);
            Rect(root.GetComponent<RectTransform>(),.38f,.32f,.97f,.85f); scroll = root.GetComponent<ScrollRect>();
            var viewport = new GameObject("Viewport", typeof(RectTransform),typeof(Image),typeof(Mask)); viewport.transform.SetParent(root.transform,false);
            Rect(viewport.GetComponent<RectTransform>(),0,0,1,1); viewport.GetComponent<Image>().color = new Color(.1f,.12f,.16f);
            viewport.GetComponent<Mask>().showMaskGraphic = true;
            var list = new GameObject("Content",typeof(RectTransform),typeof(VerticalLayoutGroup),typeof(ContentSizeFitter)); list.transform.SetParent(viewport.transform,false);
            content = list.GetComponent<RectTransform>(); content.anchorMin = new Vector2(0,1); content.anchorMax = Vector2.one;
            content.pivot = new Vector2(.5f,1); content.sizeDelta = Vector2.zero;
            var layout = list.GetComponent<VerticalLayoutGroup>(); layout.spacing = 6; layout.padding = new RectOffset(8,8,8,8);
            layout.childControlHeight = true; layout.childForceExpandHeight = false; layout.childControlWidth = true;
            list.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            scroll.viewport = viewport.GetComponent<RectTransform>(); scroll.content = content; scroll.horizontal = false; scroll.scrollSensitivity = 28;
            confirm = MakeButton(panel.transform,"Confirm","편성 확정",Confirm); Rect(confirm.GetComponent<RectTransform>(),.53f,.025f,.74f,.11f);
            cancel = MakeButton(panel.transform,"Cancel","취소",Cancel); Rect(cancel.GetComponent<RectTransform>(),.76f,.025f,.97f,.11f);
            panel.SetActive(false);
        }

        private static Text MakeText(Transform parent, string value, int size)
        {
            var go = new GameObject("Label",typeof(RectTransform),typeof(Text)); go.transform.SetParent(parent,false);
            var text = go.GetComponent<Text>(); text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = size; text.color = Color.white; text.text = value; text.raycastTarget = false;
            return text;
        }
        private static Button MakeButton(Transform parent, string name, string value, UnityEngine.Events.UnityAction action)
        {
            var go = new GameObject(name,typeof(RectTransform),typeof(Image),typeof(Button)); go.transform.SetParent(parent,false);
            go.GetComponent<Image>().color = new Color(.18f,.22f,.29f);
            Text text = MakeText(go.transform,value,22); text.alignment = TextAnchor.MiddleCenter; Rect(text.rectTransform,.01f,0,.99f,1);
            Button button = go.GetComponent<Button>(); button.onClick.AddListener(action);
            ColorBlock colors = button.colors; colors.selectedColor = new Color(1f,.8f,.35f); colors.highlightedColor = colors.selectedColor; button.colors = colors;
            return button;
        }
        private static void Rect(RectTransform rect, float left,float bottom,float right,float top)
        { rect.anchorMin = new Vector2(left,bottom); rect.anchorMax = new Vector2(right,top); rect.offsetMin = rect.offsetMax = Vector2.zero; }
    }
}

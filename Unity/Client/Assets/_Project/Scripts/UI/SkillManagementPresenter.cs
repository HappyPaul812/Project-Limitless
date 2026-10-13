using System;
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
    /// <summary>전투 밖의 무료 스킬 장착 화면입니다. 6칸과 보관함을 분리하고 저장 미지원 범위를 직접 알립니다.</summary>
    public sealed class SkillManagementPresenter : MonoBehaviour
    {
        public static SkillManagementPresenter Instance { get; private set; }
        private GameObject panel;
        private RectTransform libraryContent;
        private ScrollRect libraryScroll;
        private Text detail;
        private Button openButton;
        private readonly List<Button> focus = new List<Button>();
        private readonly List<Button> slotButtons = new List<Button>();
        private readonly List<Button> libraryButtons = new List<Button>();
        private readonly List<Button> characterButtons = new List<Button>();
        private InputAction toggle;
        private string characterId;
        private JobDefinition job;
        private string selectedSkillId;
        private int selectedSlot;
        private Text slotHeading;
        public bool IsOpen => panel != null && panel.activeSelf;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Register()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (!IsWorldScene(scene.name)) return;
            if (Instance == null) new GameObject("SkillManagementSystem").AddComponent<SkillManagementPresenter>();
        }
        private static bool IsWorldScene(string scene) => scene.StartsWith("World_", StringComparison.Ordinal)
            || scene.StartsWith("Field_", StringComparison.Ordinal) || scene.StartsWith("Dungeon_", StringComparison.Ordinal) || scene == "Arbel";
        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            CreateUi();
            toggle = new InputAction("ToggleSkillManagement", InputActionType.Button, "<Keyboard>/k");
            // 패드 South는 NPC 상호작용과 공용이므로 열기에는 아직 사용되지 않는 Menu/Start를 별도로 씁니다.
            toggle.AddBinding("<Gamepad>/start");
            toggle.performed += _ => { if (IsOpen) Close(); else Open(); };
        }
        private void OnEnable() => toggle?.Enable();
        private void OnDisable() { toggle?.Disable(); Release(); }
        private void OnDestroy() { toggle?.Dispose(); Release(); if (Instance == this) Instance = null; }
        private void Update()
        {
            openButton.interactable = !PlayerController.IsMovementLocked && !WorldModalState.IsOpen;
            if (!IsOpen) return;
            if ((Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
                || (Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame)) { Close(); return; }
            if (Keyboard.current != null && Keyboard.current.tabKey.wasPressedThisFrame)
            {
                int current = focus.FindIndex(button => button.gameObject == EventSystem.current?.currentSelectedGameObject);
                int direction = Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed ? -1 : 1;
                if (focus.Count > 0) focus[(current + direction + focus.Count) % focus.Count].Select();
            }
            // 게임패드로 화면 밖 보관함 행을 선택하면 자동 스크롤하여 포커스를 보이게 유지합니다.
            int libraryIndex = libraryButtons.FindIndex(button => button.gameObject == EventSystem.current?.currentSelectedGameObject);
            if (libraryIndex >= 0 && libraryContent.rect.height > libraryScroll.viewport.rect.height)
            {
                float y = libraryIndex * 58f;
                float visible = libraryScroll.viewport.rect.height;
                float top = libraryContent.anchoredPosition.y;
                if (y < top) libraryContent.anchoredPosition = new Vector2(0, y);
                else if (y + 54f > top + visible) libraryContent.anchoredPosition = new Vector2(0, y + 54f - visible);
            }
        }
        public void Open()
        {
            if (!IsWorldScene(SceneManager.GetActiveScene().name) || PlayerController.IsMovementLocked || !WorldModalState.TryAcquire(this)) return;
            characterId = PartyResourceService.PlayerCharacterId;
            job = FindJob(GameSessionData.SelectedJobId);
            selectedSlot = 0; selectedSkillId = null;
            panel.SetActive(true);
            PlayerController.SetMovementLocked(this, true);
            WorldExperienceHud.SetInteractionUiOpen(this, true);
            BuildCharacters();
            Refresh();
            slotButtons[0].Select();
        }
        public void Close()
        {
            if (EventSystem.current?.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.transform.IsChildOf(transform))
                EventSystem.current.SetSelectedGameObject(null);
            panel?.SetActive(false); Release();
            // 월드로 돌아갈 때 UI 선택을 남기지 않아 패드 상호작용(South)이 메뉴를 다시 열지 않게 합니다.
        }
        private void Release()
        {
            PlayerController.SetMovementLocked(this, false);
            WorldExperienceHud.SetInteractionUiOpen(this, false);
            WorldModalState.Release(this);
        }
        private static JobDefinition FindJob(string id) => Resources.LoadAll<JobDefinition>("JobDefinitions").FirstOrDefault(value => value.JobId == id);
        private void ChooseCharacter(string id, string jobId)
        {
            characterId = id; job = FindJob(jobId); selectedSkillId = null; selectedSlot = 0; Refresh(); slotButtons[0].Select();
        }
        // 화면 생성 이후 같은 지도에서 새 동료가 합류할 수 있어 열 때마다 최신 해금 명단을 다시 읽습니다.
        private void BuildCharacters()
        {
            foreach (Button old in characterButtons) { focus.Remove(old); old.gameObject.SetActive(false); Destroy(old.gameObject); }
            characterButtons.Clear();
            Button player = ButtonAt(panel.transform, "Player", "주인공", new Vector2(24, -100), new Vector2(145, 38));
            player.onClick.AddListener(() => ChooseCharacter(PartyResourceService.PlayerCharacterId, GameSessionData.SelectedJobId)); characterButtons.Add(player);
            int memberIndex = 1;
            foreach (CompanionDefinition member in CompanionCatalog.All.Where(value => CompanionRosterService.IsUnlocked(value.CharacterId)))
            {
                Button memberButton = ButtonAt(panel.transform, member.CharacterId, member.DisplayName, new Vector2(24 + memberIndex++ * 154, -100), new Vector2(145, 38));
                string id = member.CharacterId, jobId = member.JobId; memberButton.onClick.AddListener(() => ChooseCharacter(id, jobId)); characterButtons.Add(memberButton);
            }
            focus.InsertRange(0, characterButtons);
        }
        private void Refresh()
        {
            IReadOnlyList<BattleSkillDefinition> skills = SkillLoadoutService.GetLibrary(characterId, job);
            IReadOnlyList<string> slots = SkillLoadoutService.GetSlots(characterId, job);
            slotHeading.text = $"장착 슬롯 1~6 (선택: {selectedSlot + 1})";
            for (int i = 0; i < slotButtons.Count; i++)
            {
                var skill = skills.FirstOrDefault(value => value.Id == slots[i]);
                slotButtons[i].GetComponentInChildren<Text>().text = $"{i + 1}. {skill?.DisplayName ?? "비어 있음"}";
            }
            // 이전 행은 즉시 비활성화하고 파괴 예약합니다. 같은 프레임의 레이아웃 중복을 방지합니다.
            foreach (Transform child in libraryContent) { child.gameObject.SetActive(false); Destroy(child.gameObject); }
            foreach (Button old in libraryButtons) focus.Remove(old);
            libraryButtons.Clear();
            foreach (BattleSkillDefinition skill in skills)
            {
                string status = slots.Contains(skill.Id) ? "장착 중" : "보관함";
                Button row = ButtonAt(libraryContent, "Library_" + skill.Id, skill.DisplayName + " · " + status, Vector2.zero, new Vector2(500, 54));
                LayoutElement layout = row.gameObject.AddComponent<LayoutElement>(); layout.preferredHeight = 54; layout.minHeight = 54;
                string captured = skill.Id;
                row.onClick.AddListener(() => { selectedSkillId = captured; ShowDetail(); });
                EventTrigger trigger = row.gameObject.AddComponent<EventTrigger>();
                EventTrigger.Entry entry = new EventTrigger.Entry { eventID = EventTriggerType.Select };
                entry.callback.AddListener(_ => { selectedSkillId = captured; ShowDetail(); }); trigger.triggers.Add(entry);
                libraryButtons.Add(row); focus.Add(row);
            }
            libraryScroll.verticalNormalizedPosition = 1f;
            ShowDetail();
        }
        private void ShowDetail()
        {
            BattleSkillDefinition skill = SkillLoadoutService.GetLibrary(characterId, job).FirstOrDefault(value => value.Id == selectedSkillId);
            detail.text = skill == null ? "왼쪽 슬롯을 선택한 뒤 보관함 스킬을 골라 장착하세요.\n해제는 선택한 슬롯을 비웁니다."
                : $"{skill.DisplayName}\n{skill.Description}\nMP {skill.MpCost} · 재사용 {skill.CooldownTurns}턴\n{skill.TargetDescription}";
        }
        private void Equip()
        {
            if (string.IsNullOrEmpty(selectedSkillId)) return;
            if (SkillLoadoutService.TrySetSlot(characterId, job, selectedSlot, selectedSkillId, this)) { Refresh(); slotButtons[selectedSlot].Select(); }
        }
        private void Unequip()
        {
            if (SkillLoadoutService.TrySetSlot(characterId, job, selectedSlot, null, this)) { Refresh(); slotButtons[selectedSlot].Select(); }
        }
        private void CreateUi()
        {
            if (EventSystem.current == null) new GameObject("EventSystem", typeof(EventSystem)).AddComponent<InputSystemUIInputModule>().AssignDefaultActions();
            GameObject canvasObject = new GameObject("SkillManagementCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasObject.transform.SetParent(transform, false);
            Canvas canvas = canvasObject.GetComponent<Canvas>(); canvas.renderMode = RenderMode.ScreenSpaceOverlay; canvas.sortingOrder = 22;
            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>(); scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1280, 720); scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
            openButton = ButtonAt(canvasObject.transform, "OpenSkillManagement", "스킬 관리 (K)", new Vector2(1080, -18), new Vector2(182, 42));
            openButton.GetComponent<RectTransform>().anchorMin = openButton.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
            openButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-200, -18);
            openButton.onClick.AddListener(Open);
            panel = new GameObject("SkillManagementPanel", typeof(Image)); panel.transform.SetParent(canvasObject.transform, false);
            panel.GetComponent<Image>().color = new Color(.035f, .055f, .09f, .98f);
            RectTransform rect = panel.GetComponent<RectTransform>(); rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(.5f, .5f);
            rect.sizeDelta = new Vector2(1040, 650);
            Label(panel.transform, "Title", "스킬 관리 · 무료 (K / 패드 메뉴)", new Vector2(24, -16), new Vector2(550, 36), 26);
            Label(panel.transform, "SessionNotice", "임시 장착: 지도·전투 이동 동안 유지 / 새 게임·Save 로드·재시작 시 시작 3개로 복원", new Vector2(24, -54), new Vector2(990, 36), 17);
            slotHeading = Label(panel.transform, "SlotsTitle", "장착 슬롯 1~6", new Vector2(24, -148), new Vector2(445, 32), 21);
            for (int i = 0; i < 6; i++)
            {
                int captured = i;
                Button slot = ButtonAt(panel.transform, "Slot_" + (i + 1), "비어 있음", new Vector2(24 + (i % 2) * 224, -192 - (i / 2) * 64), new Vector2(215, 54));
                slot.onClick.AddListener(() => { selectedSlot = captured; slotHeading.text = $"장착 슬롯 1~6 (선택: {captured + 1})"; });
                slotButtons.Add(slot); focus.Add(slot);
            }
            Label(panel.transform, "LibraryTitle", "습득 스킬 보관함 (상하 스크롤)", new Vector2(494, -148), new Vector2(522, 32), 21);
            GameObject scrollObject = new GameObject("LibraryScroll", typeof(Image), typeof(ScrollRect)); scrollObject.transform.SetParent(panel.transform, false);
            SetRect(scrollObject.GetComponent<RectTransform>(), new Vector2(494, -192), new Vector2(522, 340));
            scrollObject.GetComponent<Image>().color = new Color(.06f, .1f, .15f);
            GameObject viewport = new GameObject("Viewport", typeof(Image), typeof(Mask)); viewport.transform.SetParent(scrollObject.transform, false);
            Stretch(viewport.GetComponent<RectTransform>()); viewport.GetComponent<Mask>().showMaskGraphic = false;
            GameObject content = new GameObject("Content", typeof(RectTransform), typeof(VerticalLayoutGroup), typeof(ContentSizeFitter)); content.transform.SetParent(viewport.transform, false);
            libraryContent = content.GetComponent<RectTransform>(); libraryContent.anchorMin = new Vector2(0, 1); libraryContent.anchorMax = Vector2.one; libraryContent.pivot = new Vector2(.5f, 1);
            libraryContent.sizeDelta = Vector2.zero;
            VerticalLayoutGroup group = content.GetComponent<VerticalLayoutGroup>(); group.spacing = 4; group.childControlHeight = true; group.childControlWidth = true;
            group.childForceExpandHeight = false; group.childForceExpandWidth = true; group.padding = new RectOffset(8, 8, 4, 4);
            content.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            libraryScroll = scrollObject.GetComponent<ScrollRect>(); libraryScroll.viewport = viewport.GetComponent<RectTransform>(); libraryScroll.content = libraryContent;
            libraryScroll.horizontal = false; libraryScroll.movementType = ScrollRect.MovementType.Clamped;
            detail = Label(panel.transform, "Details", "", new Vector2(24, -388), new Vector2(440, 148), 18);
            Button equip = ButtonAt(panel.transform, "Equip", "선택 슬롯에 장착", new Vector2(24, -556), new Vector2(280, 54)); equip.onClick.AddListener(Equip); focus.Add(equip);
            Button remove = ButtonAt(panel.transform, "Unequip", "선택 슬롯 해제", new Vector2(326, -556), new Vector2(280, 54)); remove.onClick.AddListener(Unequip); focus.Add(remove);
            Button close = ButtonAt(panel.transform, "Close", "닫기 / Esc · B", new Vector2(718, -556), new Vector2(298, 54)); close.onClick.AddListener(Close); focus.Add(close);
            panel.SetActive(false);
        }
        private static void SetRect(RectTransform rect, Vector2 position, Vector2 size)
        { rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(0, 1); rect.anchoredPosition = position; rect.sizeDelta = size; }
        private static void Stretch(RectTransform rect) { rect.anchorMin = Vector2.zero; rect.anchorMax = Vector2.one; rect.offsetMin = rect.offsetMax = Vector2.zero; }
        private static Text Label(Transform parent, string name, string text, Vector2 position, Vector2 size, int fontSize)
        {
            GameObject obj = new GameObject(name, typeof(Text)); obj.transform.SetParent(parent, false); Text value = obj.GetComponent<Text>();
            value.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"); value.fontSize = fontSize; value.text = text; value.color = Color.white;
            value.alignment = TextAnchor.UpperLeft; SetRect(value.rectTransform, position, size); return value;
        }
        private static Button ButtonAt(Transform parent, string name, string label, Vector2 position, Vector2 size)
        {
            GameObject obj = new GameObject(name, typeof(Image), typeof(Button)); obj.transform.SetParent(parent, false);
            obj.GetComponent<Image>().color = new Color(.12f, .25f, .38f); SetRect(obj.GetComponent<RectTransform>(), position, size);
            Text text = Label(obj.transform, "Label", label, Vector2.zero, size, 19); text.alignment = TextAnchor.MiddleCenter; Stretch(text.rectTransform);
            return obj.GetComponent<Button>();
        }
    }
}

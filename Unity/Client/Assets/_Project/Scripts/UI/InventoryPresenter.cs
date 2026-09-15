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
    /// <summary>월드 소지품 표시와 대상 선택만 담당하며 실제 효과·소비는 ItemUseService에 위임합니다.</summary>
    public sealed class InventoryPresenter : MonoBehaviour
    {
        public static InventoryPresenter Instance { get; private set; }
        private readonly List<Button> itemButtons = new List<Button>();
        private readonly List<Button> targetButtons = new List<Button>();
        private readonly List<Selectable> focusOrder = new List<Selectable>();
        private GameObject panel;
        private Transform itemList;
        private Transform targetList;
        private Text detailsLabel;
        private Text messageLabel;
        private Text currencyLabel;
        private Image currencyIcon;
        private Button useButton;
        private Button closeButton;
        private ItemDefinition selectedItem;
        private BattleParticipantSetup selectedTarget;
        private InputAction toggleAction;
        public bool IsOpen => panel != null && panel.activeSelf;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Register()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (!scene.name.StartsWith("World_", System.StringComparison.Ordinal)
                && !scene.name.StartsWith("Field_", System.StringComparison.Ordinal)) return;
            if (Instance == null) new GameObject("InventorySystem").AddComponent<InventoryPresenter>();
        }

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            CreateUi();
            toggleAction = new InputAction("ToggleInventory", InputActionType.Button, "<Keyboard>/i");
            toggleAction.performed += _ => { if (IsOpen) Close(); else Open(); };
        }

        private void OnEnable() => toggleAction?.Enable();
        private void OnDisable() { toggleAction?.Disable(); ReleaseModal(); }
        private void OnDestroy()
        {
            ReleaseModal();
            toggleAction?.Dispose();
            if (Instance == this) Instance = null;
        }

        private void Update()
        {
            if (!IsOpen) return;
            Keyboard keyboard = Keyboard.current;
            if (keyboard != null && keyboard.tabKey.wasPressedThisFrame)
                MoveFocus(keyboard.leftShiftKey.isPressed || keyboard.rightShiftKey.isPressed ? -1 : 1);
            if ((keyboard != null && keyboard.escapeKey.wasPressedThisFrame)
                || (Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame)) Close();
        }

        public void Open()
        {
            if (!WorldModalState.TryAcquire(this)) return;
            BuildItemRows();
            BuildTargetRows();
            currencyIcon.sprite = CurrencyPresentation.Icon;
            currencyIcon.color = CurrencyPresentation.IconTint;
            currencyLabel.text = CurrencyPresentation.FormatAmount(EconomyService.GetCurrency());
            panel.SetActive(true);
            WorldExperienceHud.SetInteractionUiOpen(this, true);
            PlayerController.SetMovementLocked(this, true);
            if (itemButtons.Count > 0) SelectItem(0); else RefreshEmptyState();
        }

        public void Close()
        {
            if (EventSystem.current?.currentSelectedGameObject != null
                && EventSystem.current.currentSelectedGameObject.transform.IsChildOf(transform))
                EventSystem.current.SetSelectedGameObject(null);
            panel?.SetActive(false);
            selectedItem = null; selectedTarget = null;
            ReleaseModal();
        }

        private void ReleaseModal()
        {
            WorldExperienceHud.SetInteractionUiOpen(this, false);
            PlayerController.SetMovementLocked(this, false);
            WorldModalState.Release(this);
        }

        private void BuildItemRows()
        {
            foreach (Transform child in itemList) Destroy(child.gameObject);
            itemButtons.Clear(); focusOrder.Clear();
            ItemDefinition[] owned = ItemCatalog.All.Where(x => InventoryService.GetItemCount(x.ItemId) > 0)
                .OrderBy(x => x.DisplayName).ToArray();
            for (int i = 0; i < owned.Length; i++)
            {
                ItemDefinition item = owned[i]; int index = i;
                Button row = CreateButton(itemList, "Item_" + item.ItemId,
                    $"{item.DisplayName}  × {InventoryService.GetItemCount(item.ItemId)}", new Vector2(0f, -i * 72f), new Vector2(380f, 64f));
                Image icon = CreateImage(row.transform, "Icon", item.Icon, item.IconTint, new Vector2(10f, -7f), new Vector2(50f, 50f));
                icon.raycastTarget = false;
                Text label = row.GetComponentInChildren<Text>(); label.alignment = TextAnchor.MiddleLeft;
                label.rectTransform.offsetMin = new Vector2(72f, 0f);
                row.onClick.AddListener(() => SelectItem(index));
                itemButtons.Add(row); focusOrder.Add(row);
            }
        }

        private BattleParticipantSetup[] GetActiveParty()
        {
            CharacterGrowthStats growth = CharacterGrowthCalculator.Calculate(GameSessionData.SelectedJobId, GameSessionData.Level);
            return BattlePrototypeEncounterFactory.CreateThreeVsThree(GameSessionData.PlayerName, GameSessionData.SelectedJobId,
                GameSessionData.SelectedPlayerPathId, CharacterGrowthCalculator.CalculateMaxHp(GameSessionData.SelectedJobId, growth),
                CharacterGrowthCalculator.CalculateAttack(GameSessionData.SelectedJobId, growth), growth.Agility, null, null, null).Allies.ToArray();
        }

        private void BuildTargetRows()
        {
            foreach (Transform child in targetList) Destroy(child.gameObject);
            targetButtons.Clear();
            foreach (BattleParticipantSetup member in GetActiveParty())
            {
                CharacterGrowthStats growth = CharacterGrowthCalculator.Calculate(member.JobId, member.Id == PartyResourceService.PlayerCharacterId ? GameSessionData.Level : 1);
                int maxMp = CharacterGrowthCalculator.CalculateMaxMp(member.JobId, growth);
                PartyResourceService.ResolveForBattle(member.Id, member.MaxHp, maxMp);
                Button row = CreateButton(targetList, "Target_" + member.Id, FormatTarget(member),
                    new Vector2(0f, -targetButtons.Count * 82f), new Vector2(400f, 74f));
                row.GetComponentInChildren<Text>().alignment = TextAnchor.MiddleLeft;
                BattleParticipantSetup captured = member;
                row.onClick.AddListener(() => SelectTarget(captured));
                targetButtons.Add(row); focusOrder.Add(row);
            }
            focusOrder.Add(useButton); focusOrder.Add(closeButton);
            if (selectedTarget == null && GetActiveParty().Length > 0) selectedTarget = GetActiveParty()[0];
        }

        private static string FormatTarget(BattleParticipantSetup member)
        {
            PartyResourceService.TryGet(member.Id, out PartyMemberResourceSnapshot value);
            string mp = value.MaxMp > 0 ? $"\nMP {value.CurrentMp} / {value.MaxMp}" : string.Empty;
            return $"{member.DisplayName}\nHP {value.CurrentHp} / {value.MaxHp}{mp}";
        }

        private void SelectItem(int index)
        {
            ItemDefinition[] owned = ItemCatalog.All.Where(x => InventoryService.GetItemCount(x.ItemId) > 0).OrderBy(x => x.DisplayName).ToArray();
            if (index < 0 || index >= owned.Length) return;
            selectedItem = owned[index]; messageLabel.text = string.Empty; Refresh();
            itemButtons[index].Select();
        }

        private void SelectTarget(BattleParticipantSetup target)
        { selectedTarget = target; messageLabel.text = string.Empty; Refresh(); }

        private void UseSelected()
        {
            if (selectedItem == null || selectedTarget == null) return;
            ItemUseOutcome result = ItemUseService.TryUse(selectedItem.ItemId, selectedTarget.Id);
            messageLabel.text = ResultMessage(result, selectedItem);
            if (result.Succeeded) GameSaveService.SaveCurrentSession();
            BuildItemRows(); BuildTargetRows();
            if (InventoryService.GetItemCount(selectedItem.ItemId) > 0) Refresh(); else RefreshEmptyState();
        }

        private void Refresh()
        {
            if (selectedItem == null) { RefreshEmptyState(); return; }
            detailsLabel.text = $"{selectedItem.DisplayName}\n{selectedItem.Description}\n\n효과: {selectedItem.EffectPreview}\n보유: {InventoryService.GetItemCount(selectedItem.ItemId)}";
            ItemUseOutcome state = selectedTarget == null ? new ItemUseOutcome(ItemUseResult.InvalidTarget)
                : ItemUseService.CanUse(selectedItem.ItemId, selectedTarget.Id);
            useButton.interactable = state.Succeeded;
        }

        private void RefreshEmptyState()
        {
            selectedItem = null; detailsLabel.text = "보유한 아이템이 없습니다."; useButton.interactable = false;
        }

        private static string ResultMessage(ItemUseOutcome result, ItemDefinition item)
        {
            if (result.Succeeded)
            {
                string resource = item != null && item.EffectType == ItemEffectType.RecoverMp ? "MP" : "HP";
                return $"{resource}가 {result.ActualAmount} 회복되었습니다.";
            }
            if (result.Result == ItemUseResult.AlreadyFull) return "이미 최대치입니다.";
            if (result.Result == ItemUseResult.TargetKnockedOut) return "전투불능 대상에게 사용할 수 없습니다.";
            if (result.Result == ItemUseResult.TargetDoesNotUseMp) return "MP를 사용하지 않는 대상입니다.";
            return "이 아이템을 사용할 수 없습니다.";
        }

        private void MoveFocus(int direction)
        {
            if (focusOrder.Count == 0 || EventSystem.current == null) return;
            int current = focusOrder.FindIndex(x => x != null && x.gameObject == EventSystem.current.currentSelectedGameObject);
            for (int step = 1; step <= focusOrder.Count; step++)
            {
                int next = (current + direction * step + focusOrder.Count) % focusOrder.Count;
                if (focusOrder[next] != null && focusOrder[next].IsInteractable()) { focusOrder[next].Select(); return; }
            }
        }

        private void CreateUi()
        {
            EnsureEventSystem(); Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            GameObject canvasObject = new GameObject("InventoryCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasObject.transform.SetParent(transform, false);
            Canvas canvas = canvasObject.GetComponent<Canvas>(); canvas.renderMode = RenderMode.ScreenSpaceOverlay; canvas.sortingOrder = 20;
            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>(); scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize; scaler.referenceResolution = new Vector2(1280, 720);
            panel = CreatePanel(canvasObject.transform, "InventoryPanel", new Vector2(.12f, .08f), new Vector2(.88f, .92f));
            CreateText(panel.transform, "Title", font, 32, TextAnchor.MiddleLeft, new Vector2(30f, -32f), new Vector2(350f, 52f)).text = "소지품";
            currencyIcon = CreateImage(panel.transform, "CurrencyIcon", null, Color.white, new Vector2(690f, -35f), new Vector2(42f, 42f));
            currencyLabel = CreateText(panel.transform, "Currency", font, 24, TextAnchor.MiddleLeft, new Vector2(742f, -32f), new Vector2(180f, 48f));
            itemList = CreateContainer(panel.transform, "ItemList", new Vector2(30f, -100f), new Vector2(380f, 390f));
            detailsLabel = CreateText(panel.transform, "Details", font, 22, TextAnchor.UpperLeft, new Vector2(435f, -105f), new Vector2(280f, 250f));
            targetList = CreateContainer(panel.transform, "TargetList", new Vector2(730f, -105f), new Vector2(400f, 260f));
            messageLabel = CreateText(panel.transform, "Message", font, 22, TextAnchor.MiddleCenter, new Vector2(430f, -385f), new Vector2(700f, 70f)); messageLabel.color = new Color(1f, .85f, .35f);
            useButton = CreateButton(panel.transform, "UseButton", "사용", new Vector2(630f, -495f), new Vector2(190f, 58f)); useButton.onClick.AddListener(UseSelected);
            closeButton = CreateButton(panel.transform, "CloseButton", "닫기", new Vector2(850f, -495f), new Vector2(190f, 58f)); closeButton.onClick.AddListener(Close);
            panel.SetActive(false);
        }

        private static void EnsureEventSystem()
        { if (EventSystem.current == null) new GameObject("EventSystem", typeof(EventSystem)).AddComponent<InputSystemUIInputModule>().AssignDefaultActions(); }
        private static Transform CreateContainer(Transform parent, string name, Vector2 position, Vector2 size)
        { GameObject obj = new GameObject(name, typeof(RectTransform)); obj.transform.SetParent(parent, false); RectTransform r = (RectTransform)obj.transform; SetTopLeft(r, position, size); return obj.transform; }
        private static GameObject CreatePanel(Transform parent, string name, Vector2 min, Vector2 max)
        { GameObject obj = new GameObject(name, typeof(Image)); obj.transform.SetParent(parent, false); obj.GetComponent<Image>().color = new Color(.035f, .055f, .09f, .98f); RectTransform r = obj.GetComponent<RectTransform>(); r.anchorMin = min; r.anchorMax = max; r.offsetMin = r.offsetMax = Vector2.zero; return obj; }
        private static Text CreateText(Transform parent, string name, Font font, int size, TextAnchor alignment, Vector2 position, Vector2 dimensions)
        { GameObject obj = new GameObject(name, typeof(Text)); obj.transform.SetParent(parent, false); Text t = obj.GetComponent<Text>(); t.font = font; t.fontSize = size; t.color = Color.white; t.alignment = alignment; t.horizontalOverflow = HorizontalWrapMode.Wrap; t.verticalOverflow = VerticalWrapMode.Overflow; SetTopLeft(t.rectTransform, position, dimensions); return t; }
        private static Button CreateButton(Transform parent, string name, string text, Vector2 position, Vector2 dimensions)
        { GameObject obj = new GameObject(name, typeof(Image), typeof(Button), typeof(Outline)); obj.transform.SetParent(parent, false); obj.GetComponent<Image>().color = new Color(.15f, .23f, .34f); Outline o = obj.GetComponent<Outline>(); o.effectColor = new Color(.85f, .68f, .28f, .8f); o.effectDistance = new Vector2(2f, -2f); SetTopLeft(obj.GetComponent<RectTransform>(), position, dimensions); Text label = CreateText(obj.transform, "Label", Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"), 21, TextAnchor.MiddleCenter, Vector2.zero, dimensions); label.text = text; label.rectTransform.anchorMin = Vector2.zero; label.rectTransform.anchorMax = Vector2.one; label.rectTransform.pivot = new Vector2(.5f, .5f); label.rectTransform.offsetMin = label.rectTransform.offsetMax = Vector2.zero; return obj.GetComponent<Button>(); }
        private static Image CreateImage(Transform parent, string name, Sprite sprite, Color color, Vector2 position, Vector2 dimensions)
        { GameObject obj = new GameObject(name, typeof(Image)); obj.transform.SetParent(parent, false); Image image = obj.GetComponent<Image>(); image.sprite = sprite; image.color = color; image.preserveAspect = true; SetTopLeft(image.rectTransform, position, dimensions); return image; }
        private static void SetTopLeft(RectTransform r, Vector2 position, Vector2 size)
        { r.anchorMin = r.anchorMax = r.pivot = new Vector2(0f, 1f); r.anchoredPosition = position; r.sizeDelta = size; }
    }
}

using System.Collections.Generic;
using ProjectLimitless.Core;
using ProjectLimitless.Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace ProjectLimitless.UI
{
    /// <summary>
    /// ShopDefinition의 품목을 공통 화면으로 보여주는 Modal UI입니다. 판매 데이터와 화면을 분리해
    /// 이후 다른 마을 상점은 새 ShopDefinition만 연결해 같은 구매·판매 흐름을 재사용합니다.
    /// </summary>
    public sealed class ShopPresenter : MonoBehaviour
    {
        private const string StarterShopPath = "ShopDefinitions/StarterVillageGeneralShop";
        public static ShopPresenter Instance { get; private set; }

        private readonly List<Button> itemButtons = new List<Button>();
        private readonly List<Selectable> focusOrder = new List<Selectable>();
        private GameObject panel;
        private Text titleLabel;
        private Text currencyLabel;
        private Image currencyIcon;
        private Text detailsLabel;
        private Text messageLabel;
        private Transform itemList;
        private Button buyButton;
        private Button sellButton;
        private Button closeButton;
        private ShopDefinition shop;
        private ItemDefinition selectedItem;
        private Transform owner;

        public bool IsOpen => panel != null && panel.activeSelf;

        /// <summary>Scene에 상점 UI가 없으면 한 번만 만들고 시작 마을 잡화상 데이터를 엽니다.</summary>
        public static void OpenStarterGeneralShop(Transform shopOwner)
        {
            ShopDefinition definition = Resources.Load<ShopDefinition>(StarterShopPath);
            if (definition == null) { Debug.LogError("시작 마을 잡화상 데이터를 찾지 못했습니다."); return; }
            if (Instance == null) new GameObject("ShopSystem").AddComponent<ShopPresenter>();
            Instance.Open(definition, shopOwner);
        }

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            CreateUi();
        }

        private void Update()
        {
            if (!IsOpen) return;
            if (owner == null || !owner.gameObject.activeInHierarchy) { Close(); return; }

            Keyboard keyboard = Keyboard.current;
            if (keyboard != null && keyboard.tabKey.wasPressedThisFrame)
                MoveFocus(keyboard.leftShiftKey.isPressed || keyboard.rightShiftKey.isPressed ? -1 : 1);
            if ((keyboard != null && keyboard.escapeKey.wasPressedThisFrame)
                || (Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame)) Close();
        }

        private void OnDisable() => ReleaseModalState();
        private void OnDestroy()
        {
            ReleaseModalState();
            if (Instance == this) Instance = null;
        }

        public void Open(ShopDefinition definition, Transform shopOwner)
        {
            if (definition == null || shopOwner == null) return;
            DialoguePresenter.Instance?.Hide();
            shop = definition;
            owner = shopOwner;
            titleLabel.text = definition.DisplayName;
            BuildItemRows();
            panel.SetActive(true);
            panel.transform.SetAsLastSibling();
            // CurrencyPresentation을 공용으로 써 화면마다 화폐명과 아이콘이 달라지는 일을 막습니다.
            currencyIcon.sprite = CurrencyPresentation.Icon;
            currencyIcon.color = CurrencyPresentation.IconTint;
            WorldExperienceHud.SetInteractionUiOpen(this, true);
            // 메뉴 방향키가 캐릭터 이동까지 일으키지 않도록 상점이 소유한 잠금을 겁니다.
            PlayerController.SetMovementLocked(this, true);
            SelectItem(0);
        }

        public void Close()
        {
            if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null
                && EventSystem.current.currentSelectedGameObject.transform.IsChildOf(transform))
                EventSystem.current.SetSelectedGameObject(null);
            if (panel != null) panel.SetActive(false);
            shop = null; selectedItem = null; owner = null;
            ReleaseModalState();
        }

        private void ReleaseModalState()
        {
            WorldExperienceHud.SetInteractionUiOpen(this, false);
            PlayerController.SetMovementLocked(this, false);
        }

        private void BuildItemRows()
        {
            foreach (Transform child in itemList) Destroy(child.gameObject);
            itemButtons.Clear();
            focusOrder.Clear();
            for (int i = 0; i < shop.ItemIds.Count; i++)
            {
                if (!ItemCatalog.TryGet(shop.ItemIds[i], out ItemDefinition item)) continue;
                int index = itemButtons.Count;
                Button row = CreateButton(itemList, "Item_" + item.ItemId, item.DisplayName, new Vector2(0f, -index * 92f), new Vector2(430f, 82f));
                Image icon = CreateImage(row.transform, "Icon", item.Icon, item.IconTint, new Vector2(18f, 9f), new Vector2(64f, 64f));
                icon.raycastTarget = false;
                Text label = row.GetComponentInChildren<Text>();
                RectTransform labelRect = label.rectTransform;
                labelRect.anchorMin = Vector2.zero;
                labelRect.anchorMax = Vector2.one;
                labelRect.offsetMin = new Vector2(96f, 8f);
                labelRect.offsetMax = new Vector2(-12f, -8f);
                label.alignment = TextAnchor.MiddleLeft;
                int captured = i;
                row.onClick.AddListener(() => SelectItem(captured));
                itemButtons.Add(row);
                focusOrder.Add(row);
            }
            focusOrder.Add(buyButton); focusOrder.Add(sellButton); focusOrder.Add(closeButton);
        }

        private void SelectItem(int index)
        {
            if (shop == null || index < 0 || index >= shop.ItemIds.Count || !ItemCatalog.TryGet(shop.ItemIds[index], out selectedItem)) return;
            messageLabel.text = string.Empty;
            Refresh();
            if (index < itemButtons.Count) itemButtons[index].Select();
        }

        private void Refresh()
        {
            currencyLabel.text = CurrencyPresentation.FormatAmount(EconomyService.GetCurrency());
            if (selectedItem == null) return;
            int count = InventoryService.GetItemCount(selectedItem.ItemId);
            string effect = string.IsNullOrWhiteSpace(selectedItem.EffectPreview) ? string.Empty : $"\n예정 효과: {selectedItem.EffectPreview}";
            bool affordable = EconomyService.CanAfford(selectedItem.BuyPrice) && InventoryService.CanAddItem(selectedItem.ItemId, 1);
            detailsLabel.text = $"{selectedItem.DisplayName}\n{selectedItem.Description}{effect}\n\n보유: {count}\n구매: {CurrencyPresentation.FormatAmount(selectedItem.BuyPrice)} ({(affordable ? "가능" : "불가")})\n판매: {CurrencyPresentation.FormatAmount(selectedItem.SellPrice)} ({(count > 0 ? "가능" : "불가")})";
            // 불가 사유를 색상에만 맡기지 않고 텍스트로 알리며, 버튼을 누르면 짧은 원인 메시지도 제공합니다.
            buyButton.interactable = selectedItem != null;
            sellButton.interactable = selectedItem != null;
        }

        private void Buy()
        {
            ShopTransactionResult result = selectedItem == null ? ShopTransactionResult.InvalidItem : ShopService.TryBuy(selectedItem.ItemId);
            messageLabel.text = ResultMessage(result, true);
            if (result == ShopTransactionResult.Success) GameSaveService.SaveCurrentSession();
            Refresh();
        }

        private void Sell()
        {
            ShopTransactionResult result = selectedItem == null ? ShopTransactionResult.InvalidItem : ShopService.TrySell(selectedItem.ItemId);
            messageLabel.text = ResultMessage(result, false);
            if (result == ShopTransactionResult.Success) GameSaveService.SaveCurrentSession();
            Refresh();
        }

        private static string ResultMessage(ShopTransactionResult result, bool buying)
        {
            if (result == ShopTransactionResult.Success) return buying ? "1개를 구매했습니다." : "1개를 판매했습니다.";
            if (result == ShopTransactionResult.InsufficientCurrency) return "탈렌트가 부족합니다.";
            if (result == ShopTransactionResult.NoItem) return "판매할 아이템이 없습니다.";
            if (result == ShopTransactionResult.InventoryFull) return "아이템을 더 보유할 수 없습니다.";
            return "거래를 완료할 수 없습니다.";
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
            EnsureEventSystem();
            Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            GameObject canvasObject = new GameObject("ShopCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasObject.transform.SetParent(transform, false);
            Canvas canvas = canvasObject.GetComponent<Canvas>(); canvas.renderMode = RenderMode.ScreenSpaceOverlay; canvas.sortingOrder = 20;
            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>(); scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize; scaler.referenceResolution = new Vector2(1280, 720);
            panel = CreatePanel(canvasObject.transform, "ShopPanel", new Vector2(0.15f, 0.1f), new Vector2(0.85f, 0.9f));
            titleLabel = CreateText(panel.transform, "Title", font, 32, TextAnchor.MiddleLeft, new Vector2(30f, -72f), new Vector2(430f, 58f));
            currencyIcon = CreateImage(panel.transform, "CurrencyIcon", null, Color.white, new Vector2(585f, -62f), new Vector2(42f, 42f));
            currencyLabel = CreateText(panel.transform, "Currency", font, 25, TextAnchor.MiddleLeft, new Vector2(638f, -68f), new Vector2(220f, 52f));
            itemList = new GameObject("ItemList", typeof(RectTransform)).transform; itemList.SetParent(panel.transform, false);
            RectTransform listRect = (RectTransform)itemList; listRect.anchorMin = new Vector2(0f, 1f); listRect.anchorMax = new Vector2(0f, 1f); listRect.pivot = new Vector2(0f, 1f); listRect.anchoredPosition = new Vector2(30f, -115f); listRect.sizeDelta = new Vector2(430f, 330f);
            detailsLabel = CreateText(panel.transform, "Details", font, 23, TextAnchor.UpperLeft, new Vector2(490f, -125f), new Vector2(370f, 300f));
            messageLabel = CreateText(panel.transform, "Message", font, 22, TextAnchor.MiddleCenter, new Vector2(30f, -460f), new Vector2(830f, 45f)); messageLabel.color = new Color(1f, .85f, .35f);
            buyButton = CreateButton(panel.transform, "BuyButton", "구매 1개", new Vector2(220f, -520f), new Vector2(180f, 58f)); buyButton.onClick.AddListener(Buy);
            sellButton = CreateButton(panel.transform, "SellButton", "판매 1개", new Vector2(420f, -520f), new Vector2(180f, 58f)); sellButton.onClick.AddListener(Sell);
            closeButton = CreateButton(panel.transform, "CloseButton", "닫기", new Vector2(620f, -520f), new Vector2(180f, 58f)); closeButton.onClick.AddListener(Close);
            panel.SetActive(false);
        }

        private static void EnsureEventSystem()
        {
            if (EventSystem.current != null) return;
            InputSystemUIInputModule module = new GameObject("EventSystem", typeof(EventSystem)).AddComponent<InputSystemUIInputModule>(); module.AssignDefaultActions();
        }

        private static GameObject CreatePanel(Transform parent, string name, Vector2 min, Vector2 max)
        {
            GameObject obj = new GameObject(name, typeof(Image)); obj.transform.SetParent(parent, false); obj.GetComponent<Image>().color = new Color(.035f, .055f, .09f, .98f);
            RectTransform rect = obj.GetComponent<RectTransform>(); rect.anchorMin = min; rect.anchorMax = max; rect.offsetMin = Vector2.zero; rect.offsetMax = Vector2.zero; return obj;
        }

        private static Text CreateText(Transform parent, string name, Font font, int size, TextAnchor alignment, Vector2 position, Vector2 dimensions)
        {
            GameObject obj = new GameObject(name, typeof(Text)); obj.transform.SetParent(parent, false); Text text = obj.GetComponent<Text>(); text.font = font; text.fontSize = size; text.color = Color.white; text.alignment = alignment; text.horizontalOverflow = HorizontalWrapMode.Wrap; text.verticalOverflow = VerticalWrapMode.Overflow;
            RectTransform rect = text.rectTransform; rect.anchorMin = new Vector2(0f, 1f); rect.anchorMax = new Vector2(0f, 1f); rect.pivot = new Vector2(0f, 1f); rect.anchoredPosition = position; rect.sizeDelta = dimensions; return text;
        }

        private static Button CreateButton(Transform parent, string name, string labelText, Vector2 position, Vector2 dimensions)
        {
            GameObject obj = new GameObject(name, typeof(Image), typeof(Button), typeof(Outline)); obj.transform.SetParent(parent, false); obj.GetComponent<Image>().color = new Color(.15f, .23f, .34f, 1f); Outline outline = obj.GetComponent<Outline>(); outline.effectColor = new Color(.85f, .68f, .28f, .8f); outline.effectDistance = new Vector2(2f, -2f);
            RectTransform rect = obj.GetComponent<RectTransform>(); rect.anchorMin = new Vector2(0f, 1f); rect.anchorMax = new Vector2(0f, 1f); rect.pivot = new Vector2(0f, 1f); rect.anchoredPosition = position; rect.sizeDelta = dimensions;
            Text label = CreateText(obj.transform, "Label", Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"), 22, TextAnchor.MiddleCenter, Vector2.zero, dimensions); label.text = labelText; RectTransform lr = label.rectTransform; lr.anchorMin = Vector2.zero; lr.anchorMax = Vector2.one; lr.pivot = new Vector2(.5f, .5f); lr.offsetMin = Vector2.zero; lr.offsetMax = Vector2.zero; return obj.GetComponent<Button>();
        }

        private static Image CreateImage(Transform parent, string name, Sprite sprite, Color color, Vector2 position, Vector2 dimensions)
        {
            GameObject obj = new GameObject(name, typeof(Image)); obj.transform.SetParent(parent, false); Image image = obj.GetComponent<Image>(); image.sprite = sprite; image.color = color; image.preserveAspect = true;
            RectTransform rect = image.rectTransform; rect.anchorMin = new Vector2(0f, 1f); rect.anchorMax = new Vector2(0f, 1f); rect.pivot = new Vector2(0f, 1f); rect.anchoredPosition = position; rect.sizeDelta = dimensions; return image;
        }
    }
}

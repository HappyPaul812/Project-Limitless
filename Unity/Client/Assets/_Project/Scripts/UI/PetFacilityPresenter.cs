using System;
using System.Collections.Generic;
using ProjectLimitless.Battle;
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
    /// 향후 안전한 관리 시설 NPC가 호출할 공용 화면입니다. Scene에 자동 설치하거나
    /// 필드 단축키를 등록하지 않으며, 분양과 장착 모두 같은 서비스의 규칙을 사용합니다.
    /// </summary>
    public sealed class PetFacilityPresenter : MonoBehaviour
    {
        private static PetFacilityPresenter instance;
        private readonly Dictionary<string, Sprite> createdIcons = new Dictionary<string, Sprite>();
        private GameObject panel;
        private Transform owner;
        private Transform rows;
        private Text title;
        private Text details;
        private Text balance;
        private Text message;
        private Image portrait;
        private Button actionButton;
        private Font font;
        private bool adoption;
        private string selectedCharacterId;
        private string selectedBeastId;

        public static void OpenAdoption(Transform npc) => Open(npc, true);
        public static void OpenManagement(Transform npc) => Open(npc, false);

        private static void Open(Transform npc, bool isAdoption)
        {
            if (npc == null) return;
            if (instance == null) new GameObject("PetFacilitySystem").AddComponent<PetFacilityPresenter>();
            instance.Show(npc, isAdoption);
        }

        private void Awake()
        {
            if (instance != null && instance != this) { Destroy(gameObject); return; }
            instance = this;
            BuildUi();
        }

        private void Update()
        {
            if (panel == null || !panel.activeSelf) return;
            if (owner == null || !owner.gameObject.activeInHierarchy ||
                Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame ||
                Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame) Close();
        }

        private void OnDestroy()
        {
            Release();
            foreach (Sprite icon in createdIcons.Values) if (icon != null) Destroy(icon);
            if (instance == this) instance = null;
        }

        private void Show(Transform npc, bool isAdoption)
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == BattleSceneFlow.BattleSceneName
                || !WorldModalState.TryAcquire(this)) return;
            owner = npc;
            adoption = isAdoption;
            selectedCharacterId = null;
            selectedBeastId = null;
            panel.SetActive(true);
            title.text = adoption ? "펫 분양" : "펫 관리";
            PlayerController.SetMovementLocked(this, true);
            WorldExperienceHud.SetInteractionUiOpen(this, true);
            RebuildRows();
        }

        public void Close()
        {
            if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null
                && EventSystem.current.currentSelectedGameObject.transform.IsChildOf(transform))
                EventSystem.current.SetSelectedGameObject(null);
            if (panel != null) panel.SetActive(false);
            owner = null;
            Release();
        }

        private void OnDisable() => Release();

        private void Release()
        {
            PlayerController.SetMovementLocked(this, false);
            WorldExperienceHud.SetInteractionUiOpen(this, false);
            WorldModalState.Release(this);
        }

        private void RebuildRows()
        {
            // Destroy는 프레임 끝에 실행됩니다. 즉시 비활성화해 모드 전환 직후 옛 버튼을 누르지 못하게 합니다.
            foreach (Transform row in rows) { row.gameObject.SetActive(false); Destroy(row.gameObject); }
            balance.text = $"보유 탈렌트: {EconomyService.GetCurrency()}";
            message.text = string.Empty;
            Button first = null;
            int index = 0;
            if (!adoption && selectedCharacterId == null)
            {
                foreach (string characterId in BeastCompanionService.GetSharpshooterCharacterIds())
                {
                    string captured = characterId;
                    string label = captured == PartyResourceService.PlayerCharacterId
                        ? GameSessionData.PlayerName : CompanionCatalog.Find(captured)?.DisplayName ?? captured;
                    Button row = AddRow(index++, label, () => { selectedCharacterId = captured; RebuildRows(); });
                    if (first == null) first = row;
                }
                details.text = "장착을 변경할 사수 캐릭터를 선택하세요.";
                actionButton.gameObject.SetActive(false);
                if (index == 0) message.text = "현재 관리할 사수가 없습니다.";
            }
            else
            {
                if (!adoption)
                {
                    Button back = AddRow(index++, "← 사수 다시 선택", () =>
                    { selectedCharacterId = null; selectedBeastId = null; RebuildRows(); });
                    first = back;
                }
                string[] ids = adoption ? BeastCompanionService.GetDiscoveredMonsterPets()
                    : BeastCompanionService.GetAvailableBeasts();
                foreach (string id in ids)
                {
                    string captured = id;
                    BeastCompanionDefinition beast = BeastCompanionCatalog.Get(captured);
                    string label = adoption
                        ? $"{beast.DisplayName} · {(BeastCompanionService.IsUnlocked(captured) ? "보유 중" : "100 탈렌트")}"
                        : $"{beast.DisplayName} {(BeastCompanionService.GetEquippedId(selectedCharacterId) == captured ? "· 장착 중" : "")}";
                    Button row = AddRow(index++, label, () => { selectedBeastId = captured; ShowDetails(); });
                    if (first == null) first = row;
                }
                if (index == 0) message.text = "승리 기록이 있는 분양 대상이 없습니다.";
                if (selectedBeastId == null || Array.IndexOf(ids, selectedBeastId) < 0)
                    selectedBeastId = ids.Length > 0 ? ids[0] : null;
                ShowDetails();
            }
            if (first != null) first.Select();
        }

        private void ShowDetails()
        {
            if (selectedBeastId == null)
            { details.text = "목록에서 펫을 선택하세요."; portrait.sprite = null; actionButton.gameObject.SetActive(false); return; }
            BeastCompanionDefinition beast = BeastCompanionCatalog.Get(selectedBeastId);
            portrait.sprite = GetIcon(beast);
            string effect = BeastCompanionService.GetPassiveDescription(selectedBeastId);
            if (adoption)
            {
                bool owned = BeastCompanionService.IsUnlocked(selectedBeastId);
                details.text = $"{beast.DisplayName}\n{effect}\n가격: {BeastCompanionService.MonsterPetPrice} 탈렌트\n"
                    + (owned ? "보유 중" : EconomyService.CanAfford(BeastCompanionService.MonsterPetPrice) ? "구매 가능" : "탈렌트 부족");
                actionButton.gameObject.SetActive(!owned);
                actionButton.interactable = !owned && EconomyService.CanAfford(BeastCompanionService.MonsterPetPrice);
                actionButton.GetComponentInChildren<Text>().text = "분양";
            }
            else
            {
                string currentId = BeastCompanionService.GetEquippedId(selectedCharacterId);
                details.text = $"{beast.DisplayName}\n{effect}\n현재: {BeastCompanionCatalog.Get(currentId).DisplayName}"
                    + $" · {BeastCompanionService.GetPassiveDescription(currentId)}";
                actionButton.gameObject.SetActive(true);
                actionButton.interactable = currentId != selectedBeastId;
                actionButton.GetComponentInChildren<Text>().text = currentId == selectedBeastId ? "장착 중" : "장착";
            }
        }

        private void ApplySelection()
        {
            bool success = adoption ? BeastCompanionService.TryAdopt(selectedBeastId)
                : BeastCompanionService.TryEquip(selectedCharacterId, selectedBeastId);
            RebuildRows();
            message.text = success ? adoption ? "분양을 완료했습니다." : "장착을 변경했습니다."
                : "조건을 확인한 뒤 다시 시도하세요.";
        }

        private Sprite GetIcon(BeastCompanionDefinition beast)
        {
            if (createdIcons.TryGetValue(beast.Id, out Sprite icon)) return icon;
            if (beast.Monster != null)
            {
                if (beast.Monster.FieldSprite != null) return beast.Monster.FieldSprite;
                Sprite[] frames = BeastCompanionCatalog.GetMonsterFrames(beast.Monster, out bool generated);
                if (frames.Length == 0) return null;
                if (generated)
                {
                    for (int index = 1; index < frames.Length; index++) Destroy(frames[index]);
                    createdIcons[beast.Id] = frames[0];
                }
                return frames[0];
            }
            Texture2D sheet = Resources.Load<Texture2D>(beast.RunResourcePath);
            if (sheet == null) return null;
            int frame = Mathf.Clamp(beast.IconFrameIndex, 0, sheet.width / beast.FrameWidth - 1);
            icon = Sprite.Create(sheet, new Rect(frame * beast.FrameWidth, 0, beast.FrameWidth, sheet.height),
                new Vector2(.5f, .5f), 1f);
            createdIcons[beast.Id] = icon;
            return icon;
        }

        private void BuildUi()
        {
            if (EventSystem.current == null)
                new GameObject("EventSystem", typeof(EventSystem)).AddComponent<InputSystemUIInputModule>().AssignDefaultActions();
            font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            GameObject canvasObject = new GameObject("PetFacilityCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasObject.transform.SetParent(transform, false);
            Canvas canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay; canvas.sortingOrder = 21;
            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1280, 720);
            panel = new GameObject("PetFacilityPanel", typeof(Image));
            panel.transform.SetParent(canvasObject.transform, false);
            panel.GetComponent<Image>().color = new Color(.035f, .055f, .09f, .98f);
            RectTransform rect = (RectTransform)panel.transform;
            rect.anchorMin = new Vector2(.15f, .1f); rect.anchorMax = new Vector2(.85f, .9f);
            rect.offsetMin = rect.offsetMax = Vector2.zero;
            title = AddText("Title", panel.transform, new Vector2(30, -30), new Vector2(500, 50), 30);
            balance = AddText("Balance", panel.transform, new Vector2(550, -30), new Vector2(310, 50), 22);
            rows = new GameObject("PetRows", typeof(RectTransform)).transform;
            rows.SetParent(panel.transform, false);
            RectTransform list = (RectTransform)rows;
            list.anchorMin = list.anchorMax = new Vector2(0, 1); list.pivot = new Vector2(0, 1);
            list.anchoredPosition = new Vector2(25, -100); list.sizeDelta = new Vector2(425, 340);
            portrait = new GameObject("PetPortrait", typeof(Image)).GetComponent<Image>();
            portrait.transform.SetParent(panel.transform, false);
            portrait.preserveAspect = true; portrait.raycastTarget = false;
            SetTopLeft(portrait.rectTransform, new Vector2(510, -110), new Vector2(110, 110));
            details = AddText("Details", panel.transform, new Vector2(510, -235), new Vector2(350, 170), 21);
            actionButton = AddButton("Action", panel.transform, new Vector2(525, -430), new Vector2(150, 52), "선택", ApplySelection);
            AddButton("Close", panel.transform, new Vector2(710, -430), new Vector2(140, 52), "닫기", Close);
            message = AddText("Message", panel.transform, new Vector2(25, -455), new Vector2(480, 58), 20);
            panel.SetActive(false);
        }

        private Button AddRow(int index, string label, Action onClick) =>
            AddButton("PetRow_" + index, rows, new Vector2(0, -index * 53), new Vector2(420, 48), label, onClick);

        private Button AddButton(string name, Transform parent, Vector2 position, Vector2 size, string label, Action onClick)
        {
            GameObject obj = new GameObject(name, typeof(Image), typeof(Button));
            obj.transform.SetParent(parent, false);
            obj.GetComponent<Image>().color = new Color(.16f, .26f, .34f);
            SetTopLeft((RectTransform)obj.transform, position, size);
            Button button = obj.GetComponent<Button>(); button.onClick.AddListener(() => onClick());
            Text text = AddText("Label", obj.transform, Vector2.zero, size, 20);
            text.alignment = TextAnchor.MiddleCenter; text.text = label;
            return button;
        }

        private Text AddText(string name, Transform parent, Vector2 position, Vector2 size, int fontSize)
        {
            Text text = new GameObject(name, typeof(Text)).GetComponent<Text>();
            text.transform.SetParent(parent, false);
            SetTopLeft(text.rectTransform, position, size);
            text.font = font; text.fontSize = fontSize; text.color = Color.white;
            text.alignment = TextAnchor.UpperLeft; text.horizontalOverflow = HorizontalWrapMode.Wrap;
            return text;
        }

        private static void SetTopLeft(RectTransform rect, Vector2 position, Vector2 size)
        {
            rect.anchorMin = rect.anchorMax = new Vector2(0, 1); rect.pivot = new Vector2(0, 1);
            rect.anchoredPosition = position; rect.sizeDelta = size;
        }
    }
}

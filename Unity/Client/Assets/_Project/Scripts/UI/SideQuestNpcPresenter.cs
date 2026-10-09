using System;
using System.Linq;
using ProjectLimitless.Core;
using ProjectLimitless.NPC;
using ProjectLimitless.Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace ProjectLimitless.UI
{
    /// <summary>같은 NPC의 여러 의뢰와 기존 업무를 시간제한 없이 명시적으로 선택하는 공통 창입니다.</summary>
    public sealed class SideQuestNpcPresenter : MonoBehaviour
    {
        private GameObject panel;
        private int openedFrame;
        public static SideQuestNpcPresenter Instance { get; private set; }
        public bool IsOpen => panel != null && panel.activeSelf;
        public int QuestChoiceCount { get; private set; }

        public static bool TryOpen(string npcId, NpcController npc, Action usualInteraction)
        {
            QuestDefinition[] quests = QuestCatalog.All.Where(x => x.IsItemDelivery
                && (x.StartNpcId == npcId || x.TurnInNpcId == npcId)
                && (QuestService.GetState(x.QuestId) == QuestState.Available || QuestService.GetState(x.QuestId) == QuestState.Active))
                .OrderBy(x => x.QuestId, StringComparer.Ordinal).ToArray();
            if (quests.Length == 0 || npc == null) return false;
            if (Instance == null) new GameObject("SideQuestNpcSystem").AddComponent<SideQuestNpcPresenter>();
            return Instance.Open(npcId, npc, quests, usualInteraction);
        }

        private void Awake() { Instance = this; }
        private void OnDisable() => Release();
        private void OnDestroy() { Release(); if (Instance == this) Instance = null; }
        private void Update()
        {
            if (IsOpen && Time.frameCount > openedFrame && ((Keyboard.current?.escapeKey.wasPressedThisFrame ?? false)
                || (Gamepad.current?.buttonEast.wasPressedThisFrame ?? false))) Close();
        }

        private bool Open(string npcId, NpcController npc, QuestDefinition[] quests, Action usual)
        {
            if (!WorldModalState.TryAcquire(this)) return false;
            if (panel != null) Destroy(panel);
            panel = new GameObject("SideQuestChoices", typeof(RectTransform), typeof(Canvas),
                typeof(UnityEngine.UI.CanvasScaler), typeof(UnityEngine.UI.GraphicRaycaster));
            panel.transform.SetParent(transform, false);
            panel.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            panel.GetComponent<Canvas>().sortingOrder = 140;
            var scaler = panel.GetComponent<UnityEngine.UI.CanvasScaler>();
            scaler.uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1280, 720); scaler.matchWidthOrHeight = .5f;
            var box = new GameObject("Choices", typeof(RectTransform), typeof(UnityEngine.UI.Image));
            box.transform.SetParent(panel.transform, false);
            var rect = box.GetComponent<RectTransform>(); rect.anchorMin = rect.anchorMax = new Vector2(.5f, .5f);
            rect.sizeDelta = new Vector2(760, 160 + 58 * (quests.Length + 2));
            box.GetComponent<UnityEngine.UI.Image>().color = new Color(.04f, .055f, .08f, .98f);
            MakeText(box.transform, npc.DisplayName + " — 의뢰 선택", new Vector2(0, -24), new Vector2(710, 44), 28);
            MakeText(box.transform, "↑↓ / 패드: 선택   Enter / A: 확인   Esc / B: 닫기", new Vector2(0, -70), new Vector2(710, 36), 20);
            var buttons = new System.Collections.Generic.List<UnityEngine.UI.Button>();
            for (int i = 0; i < quests.Length; i++)
            {
                QuestDefinition quest = quests[i]; bool active = QuestService.GetState(quest.QuestId) == QuestState.Active;
                string status = !active ? "수락" : QuestService.CanDeliverItems(quest) ? "납품 가능" : "재료 확인";
                buttons.Add(MakeButton(box.transform, $"[{status}] {quest.DisplayName}", i,
                    () => { Close(); ShowQuest(npcId, npc, quest); }));
            }
            buttons.Add(MakeButton(box.transform, "기존 이야기 / 상점·업무", quests.Length, () => { Close(); usual?.Invoke(); }));
            buttons.Add(MakeButton(box.transform, "닫기", quests.Length + 1, Close));
            for (int i = 0; i < buttons.Count; i++) buttons[i].navigation = new UnityEngine.UI.Navigation
            {
                mode = UnityEngine.UI.Navigation.Mode.Explicit,
                selectOnUp = buttons[(i + buttons.Count - 1) % buttons.Count], selectOnDown = buttons[(i + 1) % buttons.Count]
            };
            if (EventSystem.current == null)
                new GameObject("EventSystem", typeof(EventSystem)).AddComponent<InputSystemUIInputModule>().AssignDefaultActions();
            openedFrame = Time.frameCount; QuestChoiceCount = quests.Length;
            PlayerController.SetMovementLocked(this, true); WorldExperienceHud.SetInteractionUiOpen(this, true);
            buttons[0].Select(); return true;
        }

        /// <summary>확인 순간 최신 수량을 다시 검사해 창을 연 뒤 판매하거나 중복 클릭해도 잘못 납품하지 않습니다.</summary>
        public static void ShowQuest(string npcId, NpcController npc, QuestDefinition quest)
        {
            if (DialoguePresenter.Instance == null) return;
            if (QuestService.GetState(quest.QuestId) == QuestState.Available)
            {
                string needs = string.Join("\n", quest.Objectives.Select(x =>
                { ItemCatalog.TryGet(x.TargetId, out ItemDefinition item); return $"{item?.DisplayName ?? x.TargetId} ×{x.RequiredCount}"; }));
                DialoguePresenter.Instance.ShowConfirmation(npcId, npc.DisplayName,
                    quest.AcceptanceDialogue + "\n\n" + needs + $"\n보상 EXP {quest.Reward.Experience} / 탈렌트 {quest.Reward.Currency}",
                    "수락한다", "거절한다", () =>
                    {
                        bool accepted = QuestService.TryStart(quest.QuestId);
                        DialoguePresenter.Instance.Hide();
                        if (accepted) DialoguePresenter.Instance.Show(npcId, npc.DisplayName, "부탁드립니다. 재료가 모이면 다시 말씀해 주세요.");
                    });
                return;
            }
            if (QuestService.GetState(quest.QuestId) != QuestState.Active) return;
            QuestRuntimeState state = QuestService.ActiveSideQuests.First(x => x.Definition.QuestId == quest.QuestId);
            string progress = QuestService.BuildItemProgress(state);
            if (!QuestService.CanDeliverItems(quest))
            { DialoguePresenter.Instance.Show(npcId, npc.DisplayName, quest.InsufficientDialogue + "\n\n" + progress); return; }
            DialoguePresenter.Instance.ShowConfirmation(npcId, npc.DisplayName,
                progress + $"\n\n재료를 납품하시겠습니까?\n보상 EXP {quest.Reward.Experience} / 탈렌트 {quest.Reward.Currency}",
                "납품한다", "아직 보관한다", () =>
                {
                    bool delivered = QuestService.TryDeliverSideQuest(quest.QuestId, npcId);
                    DialoguePresenter.Instance.Hide();
                    DialoguePresenter.Instance.Show(npcId, npc.DisplayName, delivered
                        ? quest.CompletionDialogue + $"\n[의뢰 완료] EXP +{quest.Reward.Experience} / 탈렌트 +{quest.Reward.Currency}"
                        : "납품을 완료하지 못했습니다. 재료 수량과 보상 수용 상태를 다시 확인해 주세요.");
                });
        }

        public void Close()
        {
            if (panel != null) panel.SetActive(false);
            EventSystem.current?.SetSelectedGameObject(null);
            DialoguePresenter.Instance?.BlockInteractionThisFrame(); Release();
        }
        private void Release()
        { WorldModalState.Release(this); PlayerController.SetMovementLocked(this, false); WorldExperienceHud.SetInteractionUiOpen(this, false); }

        private UnityEngine.UI.Button MakeButton(Transform parent, string label, int index, Action action)
        {
            var obj = new GameObject("Choice_" + index, typeof(RectTransform), typeof(UnityEngine.UI.Image), typeof(UnityEngine.UI.Button));
            obj.transform.SetParent(parent, false); var rect = obj.GetComponent<RectTransform>();
            rect.anchorMin = rect.anchorMax = new Vector2(.5f, 1); rect.pivot = new Vector2(.5f, 1);
            rect.anchoredPosition = new Vector2(0, -124 - 58 * index); rect.sizeDelta = new Vector2(710, 50);
            var image = obj.GetComponent<UnityEngine.UI.Image>(); image.color = new Color(.16f, .2f, .29f, 1);
            var button = obj.GetComponent<UnityEngine.UI.Button>(); button.targetGraphic = image;
            MakeText(obj.transform, label, Vector2.zero, new Vector2(690, 48), 24, true);
            button.onClick.AddListener(() => { if (Time.frameCount > openedFrame && IsOpen) action(); }); return button;
        }
        private static void MakeText(Transform parent, string message, Vector2 position, Vector2 size, int fontSize, bool centered = false)
        {
            var text = new GameObject("Label", typeof(RectTransform), typeof(UnityEngine.UI.Text)).GetComponent<UnityEngine.UI.Text>();
            text.transform.SetParent(parent, false); text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = fontSize; text.color = Color.white; text.text = message; text.raycastTarget = false;
            text.alignment = TextAnchor.MiddleCenter; var rect = text.rectTransform;
            rect.anchorMin = rect.anchorMax = new Vector2(.5f, centered ? .5f : 1); rect.pivot = new Vector2(.5f, centered ? .5f : 1);
            rect.anchoredPosition = position; rect.sizeDelta = size;
        }
    }
}

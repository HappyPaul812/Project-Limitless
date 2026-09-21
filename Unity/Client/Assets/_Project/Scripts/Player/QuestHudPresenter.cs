using ProjectLimitless.Core;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectLimitless.Player
{
    /// <summary>월드 화면에 현재 추적 중인 퀘스트와 순차 목표 하나만 간결하게 표시합니다.</summary>
    public sealed class QuestHudPresenter : MonoBehaviour
    {
        private const string ObjectName = "QuestHud";
        private CanvasGroup canvasGroup;
        private Text questText;

        public static QuestHudPresenter EnsureOn(GameObject overlayCanvasObject)
        {
            Transform existing = overlayCanvasObject.transform.Find(ObjectName);
            if (existing != null) return existing.GetComponent<QuestHudPresenter>() ?? existing.gameObject.AddComponent<QuestHudPresenter>();
            GameObject hud = new GameObject(ObjectName, typeof(RectTransform), typeof(CanvasGroup), typeof(CanvasRenderer),
                typeof(UnityEngine.UI.Image), typeof(Outline), typeof(QuestHudPresenter));
            hud.transform.SetParent(overlayCanvasObject.transform, false);
            return hud.GetComponent<QuestHudPresenter>();
        }

        private void Awake() { BuildIfNeeded(); Refresh(); }
        private void OnEnable() { QuestService.Changed += Refresh; Refresh(); }
        private void OnDisable() => QuestService.Changed -= Refresh;

        private void Update() => RefreshVisibility();

        /// <summary>대화·상점·인벤토리의 공용 Modal 상태를 즉시 화면에 반영합니다.</summary>
        public void RefreshVisibility()
        {
            // 기존 대화·상점·인벤토리가 공유하는 Modal 상태를 그대로 따라 별도 UI 규칙을 만들지 않습니다.
            canvasGroup.alpha = WorldModalState.IsOpen || QuestService.GetTrackedQuest() == null ? 0f : 1f;
        }

        public void Refresh()
        {
            BuildIfNeeded();
            QuestRuntimeState state = QuestService.GetTrackedQuest();
            if (state == null) { questText.text = string.Empty; canvasGroup.alpha = 0f; return; }
            string type = state.Definition.QuestType == QuestType.Main ? "메인" : "서브";
            QuestObjectiveDefinition objective = state.CurrentObjective;
            questText.text = objective == null ? $"[{type}] {state.Definition.DisplayName}"
                : $"[{type}] {state.Definition.DisplayName}\n{objective.Description}";
            canvasGroup.alpha = WorldModalState.IsOpen ? 0f : 1f;
        }

        private void BuildIfNeeded()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.interactable = false; canvasGroup.blocksRaycasts = false;
            if (questText != null) return;

            RectTransform root = GetComponent<RectTransform>();
            root.anchorMin = new Vector2(0f, 1f); root.anchorMax = new Vector2(0f, 1f);
            root.pivot = new Vector2(0f, 1f); root.anchoredPosition = new Vector2(22f, -22f);
            root.sizeDelta = new Vector2(390f, 82f);
            UnityEngine.UI.Image panel = GetComponent<UnityEngine.UI.Image>();
            panel.color = new Color(0.055f, 0.065f, 0.09f, 0.94f); panel.raycastTarget = false;
            Outline border = GetComponent<Outline>(); border.effectColor = new Color(0.82f, 0.66f, 0.25f, 1f);
            border.effectDistance = new Vector2(2f, -2f);

            GameObject textObject = new GameObject("QuestText", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text), typeof(Outline));
            textObject.transform.SetParent(transform, false);
            RectTransform rect = textObject.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero; rect.anchorMax = Vector2.one;
            rect.offsetMin = new Vector2(14f, 10f); rect.offsetMax = new Vector2(-14f, -10f);
            questText = textObject.GetComponent<Text>();
            questText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            questText.fontSize = 18; questText.color = Color.white; questText.alignment = TextAnchor.MiddleLeft;
            questText.raycastTarget = false; questText.horizontalOverflow = HorizontalWrapMode.Wrap;
            questText.verticalOverflow = VerticalWrapMode.Truncate;
            Outline textOutline = textObject.GetComponent<Outline>();
            textOutline.effectColor = new Color(0f, 0f, 0f, 0.9f); textOutline.effectDistance = new Vector2(1f, -1f);
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using ProjectLimitless.Core;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectLimitless.Player
{
    /// <summary>퀘스트 목록을 상시 표시하지 않고 시작·목표 갱신·완료만 잠깐 알리는 월드 Toast입니다.</summary>
    public sealed class QuestHudPresenter : MonoBehaviour
    {
        private const string ObjectName = "QuestHud";
        private const float ToastSeconds = 4f;
        private readonly Dictionary<string, int> knownObjectives = new Dictionary<string, int>();
        private readonly HashSet<string> knownActive = new HashSet<string>();
        private readonly Queue<string> pendingNotifications = new Queue<string>();
        private CanvasGroup canvasGroup;
        private Text questText;
        private double hideAtRealtime;
        private bool toastActive;

        public bool IsToastActive => toastActive;

        public static QuestHudPresenter EnsureOn(GameObject overlayCanvasObject)
        {
            Transform existing = overlayCanvasObject.transform.Find(ObjectName);
            if (existing != null) return existing.GetComponent<QuestHudPresenter>() ?? existing.gameObject.AddComponent<QuestHudPresenter>();
            GameObject hud = new GameObject(ObjectName, typeof(RectTransform), typeof(CanvasGroup), typeof(CanvasRenderer),
                typeof(UnityEngine.UI.Image), typeof(Outline), typeof(QuestHudPresenter));
            hud.transform.SetParent(overlayCanvasObject.transform, false);
            return hud.GetComponent<QuestHudPresenter>();
        }

        private void Awake() { BuildIfNeeded(); CaptureSnapshot(); HideImmediately(); }

        private void OnEnable()
        {
            QuestService.Changed += OnQuestChanged;
            CaptureSnapshot();
            HideImmediately();
        }

        private void OnDisable()
        {
            QuestService.Changed -= OnQuestChanged;
            toastActive = false;
            pendingNotifications.Clear();
        }

        private void Update()
        {
            if (toastActive && Time.realtimeSinceStartupAsDouble >= hideAtRealtime)
            {
                HideImmediately();
                if (pendingNotifications.Count > 0) ShowToast(pendingNotifications.Dequeue());
            }
            RefreshVisibility();
        }

        /// <summary>Modal이 열린 동안 Toast가 입력이나 화면을 가리지 않도록 숨깁니다.</summary>
        public void RefreshVisibility()
        {
            UpdateToastPosition();
            if (canvasGroup != null) canvasGroup.alpha = toastActive && !WorldModalState.IsOpen ? 1f : 0f;
        }

        private void UpdateToastPosition()
        {
            RectTransform root = GetComponent<RectTransform>();
            RectTransform canvasRect = transform.parent as RectTransform;
            if (canvasRect == null) return;

            // 넓은 화면에서는 중앙을 유지합니다. 좁아지면 Toast만 오른쪽으로 옮겨
            // 좌상단 EXP 패널과 가로 영역을 나누고, 아래로 밀어 월드 화면을 더 가리지 않습니다.
            float canvasWidth = canvasRect.rect.width;
            float toastWidth = Mathf.Min(560f, Mathf.Max(200f, canvasWidth - 384f));
            float centeredLeft = (canvasWidth - toastWidth) * 0.5f;
            float toastLeft = Mathf.Max(centeredLeft, 360f);
            root.sizeDelta = new Vector2(toastWidth, 88f);
            root.anchoredPosition = new Vector2(toastLeft - centeredLeft, -24f);
        }

        /// <summary>기존 감사 코드와 호환되는 명시적 갱신 진입점입니다.</summary>
        public void Refresh() => RefreshVisibility();

        private void OnQuestChanged()
        {
            QuestRuntimeState[] current = QuestService.ActiveQuests.ToArray();
            HashSet<string> activeIds = current.Select(x => x.Definition.QuestId).ToHashSet();
            HashSet<string> completedIds = QuestService.CompletedQuestIds.ToHashSet();
            string completedId = knownActive.FirstOrDefault(id => !activeIds.Contains(id) && completedIds.Contains(id));

            if (!string.IsNullOrEmpty(completedId) && QuestCatalog.TryGet(completedId, out QuestDefinition completed))
                ShowToast($"[{TypeLabel(completed)} 퀘스트 완료]\n{completed.DisplayName}");
            else
            {
                QuestRuntimeState started = current.FirstOrDefault(x => !knownActive.Contains(x.Definition.QuestId));
                if (started != null) ShowToast($"[{TypeLabel(started.Definition)} 퀘스트 시작]\n{started.Definition.DisplayName}");
                else
                {
                    QuestRuntimeState changed = current.FirstOrDefault(x => knownObjectives.TryGetValue(x.Definition.QuestId, out int index)
                        && index != x.CurrentObjectiveIndex);
                    if (changed?.CurrentObjective != null) ShowToast($"[목표 갱신]\n{changed.CurrentObjective.Description}");
                }
            }

            CaptureSnapshot();
        }

        private static string TypeLabel(QuestDefinition definition) => definition.QuestType == QuestType.Main ? "메인" : "서브";

        private void CaptureSnapshot()
        {
            knownActive.Clear(); knownObjectives.Clear();
            foreach (QuestRuntimeState state in QuestService.ActiveQuests)
            {
                knownActive.Add(state.Definition.QuestId);
                knownObjectives[state.Definition.QuestId] = state.CurrentObjectiveIndex;
            }
        }

        private void ShowToast(string message)
        {
            BuildIfNeeded();
            questText.text = message;
            toastActive = true;
            hideAtRealtime = Time.realtimeSinceStartupAsDouble + ToastSeconds;
            RefreshVisibility();
        }

        /// <summary>퀘스트 완료 Toast가 표시 중이면 접근 가능한 텍스트 알림을 다음 순서로 예약합니다.</summary>
        public static void Notify(string message)
        {
            QuestHudPresenter presenter = FindAnyObjectByType<QuestHudPresenter>();
            if (presenter == null || string.IsNullOrWhiteSpace(message)) return;
            if (presenter.toastActive) presenter.pendingNotifications.Enqueue(message);
            else presenter.ShowToast(message);
        }

        private void HideImmediately()
        {
            toastActive = false;
            if (canvasGroup != null) canvasGroup.alpha = 0f;
        }

        private void BuildIfNeeded()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.interactable = false; canvasGroup.blocksRaycasts = false;
            if (questText != null) return;

            RectTransform root = GetComponent<RectTransform>();
            root.anchorMin = new Vector2(0.5f, 1f); root.anchorMax = new Vector2(0.5f, 1f);
            root.pivot = new Vector2(0.5f, 1f); root.anchoredPosition = new Vector2(0f, -24f); root.sizeDelta = new Vector2(560f, 88f);
            UpdateToastPosition();
            UnityEngine.UI.Image panel = GetComponent<UnityEngine.UI.Image>();
            panel.color = new Color(0.055f, 0.065f, 0.09f, 0.96f); panel.raycastTarget = false;
            Outline border = GetComponent<Outline>(); border.effectColor = new Color(0.82f, 0.66f, 0.25f, 1f); border.effectDistance = new Vector2(2f, -2f);

            GameObject textObject = new GameObject("QuestText", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text), typeof(Outline));
            textObject.transform.SetParent(transform, false);
            questText = textObject.GetComponent<Text>();
            questText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"); questText.fontSize = 25;
            questText.color = Color.white; questText.alignment = TextAnchor.MiddleCenter; questText.raycastTarget = false;
            RectTransform rect = textObject.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero; rect.anchorMax = Vector2.one; rect.offsetMin = new Vector2(18f, 10f); rect.offsetMax = new Vector2(-18f, -10f);
            Outline textOutline = textObject.GetComponent<Outline>(); textOutline.effectColor = Color.black; textOutline.effectDistance = new Vector2(1f, -1f);
        }
    }
}

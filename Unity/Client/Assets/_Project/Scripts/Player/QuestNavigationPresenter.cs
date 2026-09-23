using ProjectLimitless.Core;
using ProjectLimitless.World;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectLimitless.Player
{
    /// <summary>추적 Objective의 화면 안 Marker와 화면 밖 방향·거리를 하나의 UI로 표시합니다.</summary>
    public sealed class QuestNavigationPresenter : MonoBehaviour
    {
        private const float EdgePadding = 16f;
        private CanvasGroup group;
        private RectTransform marker;
        private Text markerText;
        private Text arrowText;
        private QuestNavigationTargetRegistry.Target target;
        private bool hasTarget;

        public bool HasTarget => hasTarget;
        public string TargetLabel => hasTarget ? target.Label : string.Empty;

        public static QuestNavigationPresenter EnsureOn(GameObject overlayCanvasObject)
        {
            Transform existing = overlayCanvasObject.transform.Find("QuestNavigation");
            if (existing != null) return existing.GetComponent<QuestNavigationPresenter>() ?? existing.gameObject.AddComponent<QuestNavigationPresenter>();
            GameObject root = new GameObject("QuestNavigation", typeof(RectTransform), typeof(CanvasGroup), typeof(QuestNavigationPresenter));
            root.transform.SetParent(overlayCanvasObject.transform, false);
            return root.GetComponent<QuestNavigationPresenter>();
        }

        private void Awake() { Build(); RefreshTarget(); }
        private void OnEnable()
        {
            QuestService.Changed += RefreshTarget;
            QuestNavigationTargetRegistry.Changed += RefreshTarget;
            RefreshTarget();
        }
        private void OnDisable()
        {
            QuestService.Changed -= RefreshTarget;
            QuestNavigationTargetRegistry.Changed -= RefreshTarget;
        }

        private void RefreshTarget()
        {
            // 저장된 추적 Quest의 현재 목표 ID를 Scene의 실제 위치 등록부에 연결합니다.
            // Scene 밖 목표처럼 위치가 없으면 임의 좌표를 가리키지 않고 안내를 숨깁니다.
            QuestRuntimeState tracked = QuestService.GetTrackedQuest();
            string id = tracked?.CurrentObjective?.TargetId;
            hasTarget = QuestNavigationTargetRegistry.TryGet(id, out target);
            if (group != null) group.alpha = hasTarget && !WorldModalState.IsOpen ? 1f : 0f;
        }

        private void LateUpdate()
        {
            // 카메라가 움직여도 대상의 화면 좌표를 매 프레임 다시 계산합니다. 화면 밖에서는
            // 가장자리 방향 안내로 바꾸고, 대화 등 Modal 중에는 월드 안내를 숨깁니다.
            if (!hasTarget || target.Transform == null) { RefreshTarget(); return; }
            Camera camera = Camera.main;
            if (camera == null) { group.alpha = 0f; return; }
            if (WorldModalState.IsOpen) { group.alpha = 0f; return; }

            Vector3 screen = camera.WorldToScreenPoint(target.Transform.position);
            bool behind = screen.z < 0f;
            Vector2 center = new Vector2(Screen.width * .5f, Screen.height * .5f);
            Vector2 direction = (Vector2)screen - center;
            if (behind) direction = -direction;
            Vector2 halfSize = marker.rect.size * .5f + Vector2.one * EdgePadding;
            bool onScreen = !behind && screen.x >= halfSize.x && screen.x <= Screen.width - halfSize.x
                && screen.y >= halfSize.y && screen.y <= Screen.height - halfSize.y;
            Vector2 position = onScreen ? (Vector2)screen : ClampToEdge(center, direction, halfSize);
            marker.position = position;
            float distance = Vector2.Distance(camera.transform.position, target.Transform.position);
            markerText.text = onScreen ? $"◆ 현재 목표\n{target.Label}" : $"◆ 현재 목표  {distance:0}m";
            arrowText.gameObject.SetActive(!onScreen);
            if (!onScreen) arrowText.rectTransform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f);
            group.alpha = 1f;
        }

        private static Vector2 ClampToEdge(Vector2 center, Vector2 direction, Vector2 halfSize)
        {
            if (direction.sqrMagnitude < .001f) direction = Vector2.up;
            float horizontal = (center.x - halfSize.x) / Mathf.Max(.001f, Mathf.Abs(direction.x));
            float vertical = (center.y - halfSize.y) / Mathf.Max(.001f, Mathf.Abs(direction.y));
            return center + direction * Mathf.Min(horizontal, vertical);
        }

        private void Build()
        {
            group = GetComponent<CanvasGroup>(); group.interactable = false; group.blocksRaycasts = false;
            marker = GetComponent<RectTransform>(); marker.sizeDelta = new Vector2(270f, 76f);
            GameObject panel = new GameObject("Background", typeof(RectTransform), typeof(CanvasRenderer), typeof(UnityEngine.UI.Image), typeof(Outline));
            panel.transform.SetParent(transform, false); Stretch(panel.GetComponent<RectTransform>());
            panel.GetComponent<UnityEngine.UI.Image>().color = new Color(.025f, .04f, .07f, .92f);
            Outline border = panel.GetComponent<Outline>(); border.effectColor = new Color(.3f, .78f, 1f, 1f); border.effectDistance = new Vector2(2f, -2f);
            markerText = MakeText(panel.transform, "MarkerText", 24, TextAnchor.MiddleCenter); Stretch(markerText.rectTransform, 34f, 5f, -8f, -5f);
            arrowText = MakeText(panel.transform, "DirectionArrow", 30, TextAnchor.MiddleCenter); arrowText.text = "▲";
            RectTransform arrow = arrowText.rectTransform; arrow.anchorMin = arrow.anchorMax = new Vector2(0f, .5f); arrow.pivot = new Vector2(.5f, .5f); arrow.anchoredPosition = new Vector2(18f, 0f); arrow.sizeDelta = new Vector2(36f, 36f);
        }

        private static Text MakeText(Transform parent, string name, int size, TextAnchor alignment)
        {
            Text text = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Text), typeof(Outline)).GetComponent<Text>();
            text.transform.SetParent(parent, false); text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"); text.fontSize = size;
            text.fontStyle = FontStyle.Bold; text.color = Color.white; text.alignment = alignment; text.raycastTarget = false;
            Outline outline = text.GetComponent<Outline>(); outline.effectColor = Color.black; outline.effectDistance = new Vector2(1f, -1f);
            return text;
        }
        private static void Stretch(RectTransform rect, float left = 0f, float bottom = 0f, float right = 0f, float top = 0f)
        { rect.anchorMin = Vector2.zero; rect.anchorMax = Vector2.one; rect.offsetMin = new Vector2(left, bottom); rect.offsetMax = new Vector2(right, top); }
    }
}

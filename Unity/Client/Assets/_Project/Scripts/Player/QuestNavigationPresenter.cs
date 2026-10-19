using ProjectLimitless.Core;
using ProjectLimitless.World;
using ProjectLimitless.NPC;
using ProjectLimitless.Monster;
using System.Collections.Generic;
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
        private RectTransform experienceHud;
        private readonly Vector3[] experienceHudCorners = new Vector3[4];
        private QuestNavigationTargetRegistry.Target target;
        private bool hasTarget;
        private Transform player;
        private static bool tutorialShown;
        private QuestRuntimeState tracked;
        public float PlayerDistance { get; private set; }
        public bool IsTargetOnScreen { get; private set; }

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
            tracked = QuestService.GetTrackedQuest();
            string id = tracked?.Definition.IsItemDelivery == true && QuestService.CanDeliverItems(tracked.Definition)
                ? tracked.Definition.TurnInNpcId : tracked?.CurrentObjective?.TargetId;
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

            bool main09 = Main09QuestGuidance.Applies(tracked);
            marker.sizeDelta = main09 ? new Vector2(380f, 68f) : new Vector2(270f, 76f);
            markerText.fontSize = main09 ? 18 : 24;
            if (player == null) player = FindAnyObjectByType<PlayerController>()?.transform;
            if (player == null) { group.alpha = 0f; return; }
            PlayerDistance = Vector2.Distance(player.position, target.Transform.position);
            Vector3 screen = camera.WorldToScreenPoint(target.Transform.position);
            bool behind = screen.z < 0f;
            Vector2 center = new Vector2(Screen.width * .5f, Screen.height * .5f);
            Vector2 direction = (Vector2)screen - center;
            if (behind) direction = -direction;
            Vector2 halfSize = marker.rect.size * .5f + Vector2.one * EdgePadding;
            bool onScreen = !behind && screen.x >= halfSize.x && screen.x <= Screen.width - halfSize.x
                && screen.y >= halfSize.y && screen.y <= Screen.height - halfSize.y;
            IsTargetOnScreen = onScreen;
            // 발밑 링은 목적지 중심을 보존하고 두 줄 안내는 아래로 띄워 대상을 가리지 않습니다.
            Vector2 position = onScreen ? (Vector2)screen + (main09 ? Vector2.down * 88f : Vector2.zero) : ClampToEdge(center, direction, halfSize);
            if (main09 && onScreen) position = PlaceMain09Label(camera, screen, halfSize);
            position = AvoidExperienceHud(position, halfSize);
            marker.position = position;
            float distance = PlayerDistance;
            if (main09)
            {
                bool near = distance <= Main09QuestGuidance.InteractionRadius;
                markerText.text = $"{tracked.CurrentObjectiveIndex + 1}/6 · {Main09QuestGuidance.Title(tracked.CurrentObjectiveIndex)}\n"
                    + (near ? $"{Main09QuestGuidance.Controls}  {Main09QuestGuidance.Action(tracked.CurrentObjectiveIndex)}" : $"표시까지 이동 · {Main09QuestGuidance.Controls}") + $" · {distance:0.0}m";
                if (!tutorialShown)
                {
                    tutorialShown = true;
                    QuestHudPresenter.Notify("흰 바닥 링으로 이동하세요.\n가까이서 E/F · 게임패드 A로 조사·대화합니다.");
                }
            }
            else markerText.text = onScreen ? $"◆ 현재 목표\n{target.Label}" : $"◆ 현재 목표  {distance:0}m";
            arrowText.gameObject.SetActive(!onScreen);
            if (!onScreen) arrowText.rectTransform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f);
            group.alpha = 1f;
        }

        /// <summary>표시 패널이 조사점 주변의 플레이어·폴·기존 NPC·몬스터를 덮지 않는 빈 공간을 고릅니다.</summary>
        private Vector2 PlaceMain09Label(Camera camera, Vector2 point, Vector2 halfSize)
        {
            var occupied = new List<Rect>();
            foreach (SpriteRenderer sprite in FindObjectsByType<SpriteRenderer>(FindObjectsSortMode.None))
            {
                if (!sprite.enabled || sprite.sprite == null) continue;
                if (sprite.GetComponentInParent<PlayerController>() == null
                    && sprite.GetComponentInParent<NpcController>() == null
                    && sprite.GetComponentInParent<MonsterFieldController>() == null
                    && sprite.GetComponent<MainQuest09Interactable>() == null) continue;
                Vector2 min = camera.WorldToScreenPoint(sprite.bounds.min), max = camera.WorldToScreenPoint(sprite.bounds.max);
                occupied.Add(Rect.MinMaxRect(min.x - 8, min.y - 8, max.x + 8, max.y + 24));
            }
            Vector2 best = point; float bestArea = float.MaxValue;
            foreach (Vector2 offset in new[] { new Vector2(0, -94), new Vector2(-halfSize.x - 72, -94),
                new Vector2(halfSize.x + 72, -94), new Vector2(0, 132), new Vector2(-halfSize.x - 72, 100),
                new Vector2(halfSize.x + 72, 100), new Vector2(-halfSize.x - 72, 0), new Vector2(halfSize.x + 72, 0) })
            {
                Vector2 candidate = point + offset;
                candidate.x = Mathf.Clamp(candidate.x, halfSize.x, Screen.width - halfSize.x);
                candidate.y = Mathf.Clamp(candidate.y, halfSize.y, Screen.height - halfSize.y);
                candidate = AvoidExperienceHud(candidate, halfSize);
                Rect area = new Rect(candidate - halfSize, halfSize * 2); float overlap = 0;
                foreach (Rect actor in occupied)
                    overlap += Mathf.Max(0, Mathf.Min(area.xMax, actor.xMax) - Mathf.Max(area.xMin, actor.xMin))
                        * Mathf.Max(0, Mathf.Min(area.yMax, actor.yMax) - Mathf.Max(area.yMin, actor.yMin));
                if (overlap >= bestArea) continue;
                bestArea = overlap; best = candidate;
                if (overlap == 0) break;
            }
            return best;
        }

        private Vector2 AvoidExperienceHud(Vector2 position, Vector2 halfSize)
        {
            if (experienceHud == null) experienceHud = transform.parent.Find("WorldExperienceHud") as RectTransform;
            if (experienceHud == null || experienceHud.GetComponent<CanvasGroup>().alpha <= 0f) return position;

            // 목표가 화면 좌상단에 있어도 방향 마커가 경험치 정보를 덮지 않도록
            // 겹치는 경우에만 패널 아래로 옮깁니다. HUD 위치 프리셋이 바뀌어도 실제 모서리를 읽습니다.
            experienceHud.GetWorldCorners(experienceHudCorners);
            Rect hudArea = Rect.MinMaxRect(experienceHudCorners[0].x, experienceHudCorners[0].y,
                experienceHudCorners[2].x, experienceHudCorners[2].y);
            Rect markerArea = Rect.MinMaxRect(position.x - halfSize.x, position.y - halfSize.y,
                position.x + halfSize.x, position.y + halfSize.y);
            if (hudArea.Overlaps(markerArea))
                position.y = Mathf.Max(halfSize.y, hudArea.yMin - EdgePadding - halfSize.y);
            return position;
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

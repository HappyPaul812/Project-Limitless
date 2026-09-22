using ProjectLimitless.Core;
using ProjectLimitless.Player;
using ProjectLimitless.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjectLimitless.World
{
    /// <summary>Field_01에 Main 02 조사 구역과 흔적 상호작용을 설치합니다.</summary>
    public static class MainQuest02FieldFlow
    {
        public const string QuestId = "main_02_grassland_anomaly";
        public const string InvestigationAreaId = "field01_main02_investigation_area";
        public const string EncounterId = "field01_main02_investigation_encounter";
        public const string TracksId = "field01_main02_tracks";
        public const string QuestSpawnId = "grass_slime_01";
        private const string SceneName = "Field_01";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            SceneManager.sceneLoaded -= Install;
            SceneManager.sceneLoaded += Install;
        }

        private static void Install(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != SceneName || GameObject.Find("MainQuest02FieldFlow") != null) return;
            // 이전 버전 저장에서 Main 01만 끝난 경우에도 Field 진입 시 Main 02를 안전하게 이어 줍니다.
            if (QuestService.GetState(QuestId) == QuestState.Available) QuestService.TryStart(QuestId);

            GameObject root = new GameObject("MainQuest02FieldFlow");
            SceneManager.MoveGameObjectToScene(root, scene);

            GameObject area = new GameObject("Main02InvestigationArea", typeof(BoxCollider2D), typeof(QuestLocationTrigger));
            area.transform.SetParent(root.transform, false);
            area.transform.position = new Vector3(.4f, 1f, 0f);
            BoxCollider2D areaCollider = area.GetComponent<BoxCollider2D>();
            areaCollider.isTrigger = true;
            areaCollider.size = new Vector2(2.4f, 2.4f);
            area.GetComponent<QuestLocationTrigger>().Configure(QuestId, InvestigationAreaId);
            QuestNavigationTarget.Attach(area, InvestigationAreaId, "조사 지점");

            GameObject tracks = new GameObject("Main02Tracks", typeof(CircleCollider2D), typeof(MainQuest02Tracks));
            tracks.transform.SetParent(root.transform, false);
            tracks.transform.position = new Vector3(5.1f, -1.2f, 0f);
            CircleCollider2D tracksCollider = tracks.GetComponent<CircleCollider2D>();
            tracksCollider.isTrigger = true;
            tracksCollider.radius = 1.35f;
            QuestNavigationTarget.Attach(tracks, TracksId, "흩어진 흔적", new Vector3(0f, 1.35f, 0f));
        }
    }

    /// <summary>플레이어가 넓은 조사 구역에 들어왔을 때 stable Location ID를 한 번 알립니다.</summary>
    public sealed class QuestLocationTrigger : MonoBehaviour
    {
        private string questId;
        private string locationId;
        public void Configure(string targetQuestId, string stableLocationId)
        { questId = targetQuestId; locationId = stableLocationId; }

        private void OnTriggerEnter2D(Collider2D other)
            => TryNotify(other);

        private void OnTriggerStay2D(Collider2D other)
            => TryNotify(other);

        private void FixedUpdate()
        {
            QuestRuntimeState active = QuestService.ActiveMainQuest;
            if (active?.Definition.QuestId != questId || active.CurrentObjective?.TargetId != locationId) return;
            // 저장 복원 직후 Trigger 안에서 시작해 Enter 사건이 생기지 않는 경우를 작은 영역 검사로 보완합니다.
            BoxCollider2D area = GetComponent<BoxCollider2D>();
            foreach (Collider2D candidate in Physics2D.OverlapBoxAll(transform.position, area.size, 0f))
                if (candidate.GetComponent<PlayerController>() != null) { QuestService.NotifyLocationReached(locationId); return; }
        }

        private void TryNotify(Collider2D other)
        {
            if (other.GetComponent<PlayerController>() == null) return;
            QuestRuntimeState active = QuestService.ActiveMainQuest;
            if (active?.Definition.QuestId == questId && active.CurrentObjective?.TargetId == locationId)
                QuestService.NotifyLocationReached(locationId);
        }
    }

    /// <summary>전투 뒤에만 보이는 흔적과 기존 DialoguePresenter를 이용한 조사 입력을 담당합니다.</summary>
    public sealed class MainQuest02Tracks : MonoBehaviour
    {
        private const string CommonConclusion = "이 흔적을 보면, 몬스터들이 마을을 향해 몰려온 것이라기보다 무언가를 피해 이쪽으로 밀려온 것 같다.";
        private InputAction interactAction;
        private GameObject marker;
        private GameObject prompt;
        private PlayerController nearbyPlayer;

        private void Awake()
        {
            marker = CreateWorldLabel("TracksMarker", new Vector3(0f, 1.4f, 0f), "◇ 흩어진 흔적", new Color(.82f, .72f, .42f));
            prompt = CreateWorldLabel("TracksPrompt", new Vector3(0f, 2f, 0f), "[E/F] 조사", new Color(.3f, .78f, 1f));
            interactAction = new InputAction("Investigate", InputActionType.Button);
            interactAction.AddBinding("<Keyboard>/e");
            interactAction.AddBinding("<Keyboard>/f");
            interactAction.AddBinding("<Gamepad>/buttonSouth");
            interactAction.performed += OnInteract;
        }

        private void OnEnable()
        {
            QuestService.Changed += Refresh;
            interactAction?.Enable();
            Refresh();
        }

        private void OnDisable()
        {
            QuestService.Changed -= Refresh;
            interactAction?.Disable();
        }

        private void OnDestroy() => interactAction?.Dispose();

        private void OnTriggerEnter2D(Collider2D other)
        { nearbyPlayer = other.GetComponent<PlayerController>() ?? nearbyPlayer; Refresh(); }

        private void OnTriggerStay2D(Collider2D other)
        { nearbyPlayer = other.GetComponent<PlayerController>() ?? nearbyPlayer; Refresh(); }

        private void OnTriggerExit2D(Collider2D other)
        { if (other.GetComponent<PlayerController>() == nearbyPlayer) nearbyPlayer = null; Refresh(); }

        private void FixedUpdate()
        {
            if (!IsCurrentObjective()) return;
            PlayerController found = null;
            foreach (Collider2D candidate in Physics2D.OverlapCircleAll(transform.position, 1.35f))
            {
                found = candidate.GetComponent<PlayerController>();
                if (found != null) break;
            }
            if (found == nearbyPlayer) return;
            nearbyPlayer = found;
            Refresh();
        }

        private void OnInteract(InputAction.CallbackContext context)
        {
            if (nearbyPlayer == null || !IsCurrentObjective() || WorldModalState.IsOpen) return;
            // 길별 문장은 정보량이나 보상을 바꾸지 않고 같은 결론을 받아들이는 표현만 달리합니다.
            DialoguePresenter.Instance?.ShowSequence("조사", new[] { GetPathReaction(GameSessionData.SelectedPlayerPathId), CommonConclusion },
                () => QuestService.NotifyInteraction(MainQuest02FieldFlow.TracksId));
            DialoguePresenter.Instance?.TrackDistance(nearbyPlayer.transform, transform, 3f);
        }

        private void Refresh()
        {
            bool visible = IsCurrentObjective();
            if (marker != null) marker.SetActive(visible);
            if (prompt != null) prompt.SetActive(visible && nearbyPlayer != null && !WorldModalState.IsOpen);
        }

        private static string GetPathReaction(string pathId)
        {
            switch (pathId)
            {
                case "path.emotional-scar": return "위험은 지나간 듯하지만, 이곳에는 아직 불안하게 흩어진 흔적이 남아 있다.";
                case "path.hearing": return "주변은 다시 조용해졌지만, 움직임이 한쪽으로 급히 쏠렸던 분위기가 남아 있다.";
                case "path.vision": return "흩어진 흔적을 차례로 정리해 보니, 움직임이 한 방향으로 이어져 있다.";
                case "path.mobility": return "눌린 풀과 비켜 난 자리를 따라가 보니, 여러 몬스터가 열린 쪽으로 급히 움직인 듯하다.";
                case "path.intellectual": return "여러 흔적의 순서를 하나씩 맞춰 보니, 같은 방향으로 급히 움직인 흐름이 보인다.";
                default: return "흩어진 흔적을 천천히 살펴보니, 여러 몬스터가 같은 방향으로 급히 움직인 듯하다.";
            }
        }

        private bool IsCurrentObjective()
        {
            QuestRuntimeState active = QuestService.ActiveMainQuest;
            return active?.Definition.QuestId == MainQuest02FieldFlow.QuestId
                && active.CurrentObjective?.TargetId == MainQuest02FieldFlow.TracksId;
        }

        private GameObject CreateWorldLabel(string name, Vector3 localPosition, string text, Color color)
        {
            GameObject canvasObject = new GameObject(name, typeof(Canvas), typeof(CanvasScaler));
            canvasObject.transform.SetParent(transform, false);
            canvasObject.transform.localPosition = localPosition;
            canvasObject.transform.localScale = Vector3.one * .01f;
            Canvas canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.sortingOrder = 22;
            canvasObject.GetComponent<RectTransform>().sizeDelta = new Vector2(250f, 48f);
            GameObject backgroundObject = new GameObject("Background", typeof(Image));
            backgroundObject.transform.SetParent(canvasObject.transform, false);
            Image background = backgroundObject.GetComponent<Image>();
            background.color = new Color(.025f, .04f, .07f, .9f);
            Stretch(background.rectTransform);
            GameObject textObject = new GameObject("Text", typeof(Text));
            textObject.transform.SetParent(backgroundObject.transform, false);
            Text label = textObject.GetComponent<Text>();
            label.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            label.fontSize = 27;
            label.fontStyle = FontStyle.Bold;
            label.alignment = TextAnchor.MiddleCenter;
            label.color = color;
            label.text = text;
            Stretch(label.rectTransform);
            return canvasObject;
        }

        private static void Stretch(RectTransform rect)
        { rect.anchorMin = Vector2.zero; rect.anchorMax = Vector2.one; rect.offsetMin = new Vector2(6f, 3f); rect.offsetMax = new Vector2(-6f, -3f); }
    }
}

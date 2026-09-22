using ProjectLimitless.Core;
using ProjectLimitless.Player;
using ProjectLimitless.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjectLimitless.World
{
    /// <summary>Main 06을 시작하고 Field_02 진입·이상 흔적 조사를 기존 퀘스트 시스템에 연결합니다.</summary>
    public static class MainQuest06ForestFlow
    {
        public const string QuestId = "main_06_into_the_forest";
        public const string ForestLocationId = "field02_main06_entry";
        public const string AnomalyTraceId = "field02_main06_anomaly_trace";
        private const string Field01Scene = "Field_01";
        private const string Field02Scene = "Field_02";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            bool shouldSave = false;
            // Main 05 직후 마을에서는 Available을 유지하고, 숲으로 향해 Field_01에 나설 때 자연스럽게 시작합니다.
            if ((scene.name == Field01Scene || scene.name == Field02Scene)
                && QuestService.GetState(QuestId) == QuestState.Available)
                shouldSave = QuestService.TryStart(QuestId);

            if (scene.name != Field02Scene)
            {
                if (shouldSave && GameSaveService.CurrentSlotIndex > 0) GameSaveService.SaveCurrentSession();
                return;
            }

            QuestRuntimeState active = QuestService.ActiveMainQuest;
            if (active?.Definition.QuestId == QuestId && active.CurrentObjective?.TargetId == ForestLocationId)
            {
                QuestService.NotifyLocationReached(ForestLocationId);
                shouldSave = true;
            }

            InstallTrace(scene);
            if (shouldSave && GameSaveService.CurrentSlotIndex > 0) GameSaveService.SaveCurrentSession();
        }

        private static void InstallTrace(Scene scene)
        {
            if (GameObject.Find("MainQuest06ForestFlow") != null) return;
            // Field_02를 저장에서 바로 이어도 대화 UI가 반드시 준비되도록 기존 Main 03/04 방식을 따릅니다.
            if (DialoguePresenter.Instance == null) new GameObject("DialogueSystem").AddComponent<DialoguePresenter>();
            GameObject root = new GameObject("MainQuest06ForestFlow");
            SceneManager.MoveGameObjectToScene(root, scene);

            GameObject trace = new GameObject("Main06AnomalyTrace", typeof(CircleCollider2D), typeof(MainQuest06AnomalyTrace));
            trace.transform.SetParent(root.transform, false);
            trace.transform.position = new Vector3(.2f, -1.6f, 0f);
            CircleCollider2D collider = trace.GetComponent<CircleCollider2D>();
            collider.isTrigger = true;
            collider.radius = 1.35f;
        }
    }

    /// <summary>숲길 바닥의 한 방향 이동 흔적을 보여 주고, 조사 대화가 끝난 뒤에만 Main 06을 완료합니다.</summary>
    public sealed class MainQuest06AnomalyTrace : MonoBehaviour
    {
        private InputAction interactAction;
        private GameObject visualRoot;
        private GameObject marker;
        private GameObject prompt;
        private PlayerController nearbyPlayer;

        private void Awake()
        {
            visualRoot = CreateTrailMarks();
            marker = CreateWorldLabel("TraceMarker", new Vector3(0f, 1.45f, 0f), "◇ 이상한 이동 흔적", new Color(.86f, .72f, .4f));
            prompt = CreateWorldLabel("TracePrompt", new Vector3(0f, 2.05f, 0f), "[E/F] 조사", new Color(.3f, .78f, 1f));
            interactAction = new InputAction("InvestigateMain06Trace", InputActionType.Button);
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
        private void OnTriggerEnter2D(Collider2D other) { nearbyPlayer = other.GetComponent<PlayerController>() ?? nearbyPlayer; Refresh(); }
        private void OnTriggerStay2D(Collider2D other) { nearbyPlayer = other.GetComponent<PlayerController>() ?? nearbyPlayer; Refresh(); }
        private void OnTriggerExit2D(Collider2D other) { if (other.GetComponent<PlayerController>() == nearbyPlayer) nearbyPlayer = null; Refresh(); }

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
            DialogueLine[] lines =
            {
                new DialogueLine(CompanionRosterService.TaeonId, "태온", "초원에서 봤던 움직임과 비슷합니다."),
                new DialogueLine(CompanionRosterService.TaeonId, "태온", "하지만 여기서는 더 넓게 퍼져 있어요."),
                new DialogueLine(CompanionRosterService.MielId, "미엘", "그럼 초원의 문제만은 아니었던 거네요."),
                new DialogueLine(string.Empty, PlayerName, "숲 안쪽을 더 확인해봐야겠습니다.")
            };
            DialoguePresenter.Instance?.ShowSequence(lines, CompleteInvestigation);
            DialoguePresenter.Instance?.TrackDistance(nearbyPlayer.transform, transform, 3f);
        }

        private static string PlayerName => string.IsNullOrWhiteSpace(GameSessionData.PlayerName) ? "플레이어" : GameSessionData.PlayerName;

        private void CompleteInvestigation()
        {
            QuestService.NotifyInteraction(MainQuest06ForestFlow.AnomalyTraceId);
            if (GameSaveService.CurrentSlotIndex > 0) GameSaveService.SaveCurrentSession();
        }

        private bool IsCurrentObjective()
        {
            QuestRuntimeState active = QuestService.ActiveMainQuest;
            return active?.Definition.QuestId == MainQuest06ForestFlow.QuestId
                && active.CurrentObjective?.TargetId == MainQuest06ForestFlow.AnomalyTraceId;
        }

        private void Refresh()
        {
            bool visible = IsCurrentObjective();
            if (visualRoot != null) visualRoot.SetActive(visible);
            if (marker != null) marker.SetActive(visible);
            if (prompt != null) prompt.SetActive(visible && nearbyPlayer != null && !WorldModalState.IsOpen);
        }

        private GameObject CreateTrailMarks()
        {
            GameObject root = new GameObject("OneWayTrailVisual");
            root.transform.SetParent(transform, false);
            // 작은 눌린 자국들이 한 방향으로 넓어지는 배치로, 마법 효과 없이 이동 흐름만 환경적으로 전달합니다.
            Vector3[] positions = { new Vector3(-.75f, -.45f), new Vector3(-.35f, -.2f), new Vector3(.05f, .05f), new Vector3(.5f, .28f), new Vector3(.92f, .48f) };
            for (int i = 0; i < positions.Length; i++)
            {
                GameObject mark = GameObject.CreatePrimitive(PrimitiveType.Quad);
                mark.name = $"PressedGround_{i + 1:00}";
                mark.transform.SetParent(root.transform, false);
                mark.transform.localPosition = positions[i];
                mark.transform.localRotation = Quaternion.Euler(0f, 0f, 28f);
                mark.transform.localScale = new Vector3(.32f + i * .035f, .12f, 1f);
                Object.Destroy(mark.GetComponent<MeshCollider>());
                Renderer renderer = mark.GetComponent<Renderer>();
                renderer.material = new Material(Shader.Find("Sprites/Default"));
                renderer.material.color = new Color(.16f, .12f, .07f, .58f);
                renderer.sortingOrder = 4;
            }
            return root;
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
            canvasObject.GetComponent<RectTransform>().sizeDelta = new Vector2(280f, 48f);
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

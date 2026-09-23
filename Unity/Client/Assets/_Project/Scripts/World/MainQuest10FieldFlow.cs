using System.Linq;
using ProjectLimitless.Core;
using ProjectLimitless.Player;
using ProjectLimitless.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjectLimitless.World
{
    /// <summary>Main09의 석재를 재사용하고, Field03 깊은 곳의 Main10 조사 대상만 단계별로 보여줍니다.</summary>
    public sealed class MainQuest10FieldFlow : MonoBehaviour
    {
        public const string QuestId = "main_10_center_of_silence";
        public const string StoneworkId = "field03_main10_stonework";
        public const string SilenceId = "field03_main10_silence_direction";
        public const string DescentId = "field03_main10_underground_descent";
        public const string EntranceId = "field03_main10_catacomb_entrance";
        private GameObject stonework, silence, descent, entrance;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Register()
        { SceneManager.sceneLoaded -= Install; SceneManager.sceneLoaded += Install; }

        private static void Install(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != "Field_03") return;
            if (DialoguePresenter.Instance == null) new GameObject("DialogueSystem").AddComponent<DialoguePresenter>();
            var root = new GameObject("MainQuest10FieldFlow");
            SceneManager.MoveGameObjectToScene(root, scene);
            root.AddComponent<MainQuest10FieldFlow>();
        }

        private void Awake()
        {
            stonework = Site(StoneworkId, "가공된 돌", new Vector2(-1.15f, -3.7f));
            silence = Site(SilenceId, "고요해지는 숲길", new Vector2(-1.85f, -4.2f));
            descent = Site(DescentId, "땅속으로 이어지는 흔적", new Vector2(-0.7f, -4.55f));
            entrance = Site(EntranceId, "침묵의 지하묘지 입구", new Vector2(0.55f, -4.85f));
            Mark(stonework.transform, new Vector2(-.16f, 0), new Vector2(.55f, .21f), new Color(.43f, .46f, .42f));
            Mark(stonework.transform, new Vector2(.31f, -.1f), new Vector2(.33f, .18f), new Color(.35f, .39f, .36f));
            Mark(descent.transform, new Vector2(-.28f, .06f), new Vector2(.65f, .2f), new Color(.4f, .43f, .39f));
            Mark(descent.transform, new Vector2(.12f, -.21f), new Vector2(.72f, .17f), new Color(.3f, .34f, .31f));
            // 완전한 입구는 발견 단계 전에는 비활성화됩니다. 문양은 특정 세력을 암시하지 않는 마모된 선입니다.
            Mark(entrance.transform, Vector2.zero, new Vector2(1.65f, .83f), new Color(.045f, .065f, .065f));
            Mark(entrance.transform, new Vector2(-.97f, .22f), new Vector2(.29f, 1.29f), new Color(.45f, .48f, .43f));
            Mark(entrance.transform, new Vector2(.97f, .22f), new Vector2(.29f, 1.29f), new Color(.45f, .48f, .43f));
            Mark(entrance.transform, new Vector2(0, .89f), new Vector2(2.14f, .28f), new Color(.4f, .44f, .4f));
            Mark(entrance.transform, new Vector2(0, -.47f), new Vector2(1.42f, .16f), new Color(.33f, .36f, .33f));
        }

        private void Start()
        {
            // sceneLoaded 콜백이 모두 끝난 뒤 Main09가 만든 실제 석재 오브젝트에 조사 동작을 연결합니다.
            var earlier = FindObjectsByType<MainQuest09FieldFlow>().FirstOrDefault();
            var structure = earlier?.transform.Find(MainQuest09FieldFlow.StructureId);
            if (structure != null)
            {
                var interactable = structure.gameObject.AddComponent<MainQuest10Interactable>();
                interactable.Configure(MainQuest09FieldFlow.StructureId, "가공된 석재");
            }
            Refresh();
        }

        private void OnEnable() { QuestService.Changed += Refresh; Refresh(); }
        private void OnDisable() => QuestService.Changed -= Refresh;

        private void Refresh()
        {
            if (QuestService.GetState(QuestId) == QuestState.Available) QuestService.TryStart(QuestId);
            var active = QuestService.ActiveMainQuest;
            int step = active?.Definition.QuestId == QuestId ? active.CurrentObjectiveIndex : -1;
            stonework.SetActive(step == 1);
            silence.SetActive(step == 2);
            descent.SetActive(step == 3);
            entrance.SetActive(step >= 4 || QuestService.GetState(QuestId) == QuestState.Completed);
        }

        private GameObject Site(string id, string label, Vector2 position)
        {
            var site = new GameObject(id);
            site.transform.SetParent(transform, false);
            site.transform.position = position;
            site.AddComponent<MainQuest10Interactable>().Configure(id, label);
            QuestNavigationTarget.Attach(site, id, label, new Vector3(0, 1.4f, 0));
            return site;
        }

        private static void Mark(Transform parent, Vector2 offset, Vector2 size, Color color)
        {
            var mark = GameObject.CreatePrimitive(PrimitiveType.Quad);
            mark.name = "OldStone";
            Destroy(mark.GetComponent<Collider>());
            mark.transform.SetParent(parent, false);
            mark.transform.localPosition = offset;
            mark.transform.localScale = size;
            var material = new Material(Shader.Find("Sprites/Default"));
            material.color = color;
            var renderer = mark.GetComponent<MeshRenderer>();
            renderer.sharedMaterial = material;
            renderer.sortingOrder = 5;
            mark.AddComponent<MainQuest09MarkerMaterial>().OwnedMaterial = material;
        }
    }

    /// <summary>현재 목표와 가까울 때만 기존 대화를 열며, 완료 뒤에는 안전한 던전 진입 Hook을 제공합니다.</summary>
    public sealed class MainQuest10Interactable : MonoBehaviour
    {
        public string TargetId { get; private set; }
        private string label;
        private InputAction interact;
        private GameObject marker;
        private static int completedFrame = -1;
        private bool IsCurrent => QuestService.ActiveMainQuest?.Definition.QuestId == MainQuest10FieldFlow.QuestId
            && QuestService.ActiveMainQuest.CurrentObjective?.TargetId == TargetId;

        public void Configure(string id, string displayName) { TargetId = id; label = displayName; }

        private void Awake()
        {
            interact = new InputAction("Main10Interact", InputActionType.Button);
            interact.AddBinding("<Keyboard>/e");
            interact.AddBinding("<Keyboard>/f");
            interact.AddBinding("<Gamepad>/buttonSouth");
            interact.performed += _ => TryInteract();
        }
        private void Start() => marker = CreateMarker();
        private void Update()
        {
            if (marker != null) marker.SetActive((IsCurrent || IsCompletedEntrance) && !WorldModalState.IsOpen);
        }
        private bool IsCompletedEntrance => TargetId == MainQuest10FieldFlow.EntranceId
            && QuestService.GetState(MainQuest10FieldFlow.QuestId) == QuestState.Completed;
        private void OnEnable() => interact.Enable();
        private void OnDisable() => interact.Disable();
        private void OnDestroy() => interact.Dispose();

        public bool TryInteract()
        {
            if ((!IsCurrent && !IsCompletedEntrance) || WorldModalState.IsOpen || completedFrame == Time.frameCount) return false;
            var player = FindAnyObjectByType<PlayerController>()?.transform;
            if (player == null || Vector2.Distance(player.position, transform.position) > 1.6f) return false;
            if (IsCompletedEntrance)
            {
                if (Application.CanStreamedLevelBeLoaded("Dungeon_01"))
                    SceneTransitionService.Load("Dungeon_01", "Spawn_From_Field03");
                else
                {
                    DialoguePresenter.Instance.ShowSequence(new[]
                    { new DialogueLine("", GameSessionData.PlayerName, "입구를 확인했다. 내려가기 전 준비를 마치고 다시 돌아오자.") }, null);
                    DialoguePresenter.Instance.TrackDistance(player, transform, 3f);
                }
                return true;
            }
            int step = QuestService.ActiveMainQuest.CurrentObjectiveIndex;
            DialoguePresenter.Instance.ShowSequence(MainQuest10Dialogue.Lines(step), () => Complete(step));
            DialoguePresenter.Instance.TrackDistance(player, transform, 3f);
            return true;
        }

        private void Complete(int step)
        {
            if (!IsCurrent || QuestService.ActiveMainQuest.CurrentObjectiveIndex != step) return;
            completedFrame = Time.frameCount;
            QuestService.NotifyInteraction(TargetId);
            if (GameSaveService.CurrentSlotIndex > 0) GameSaveService.SaveCurrentSession();
        }

        private GameObject CreateMarker()
        {
            var root = new GameObject("QuestMarker", typeof(Canvas), typeof(CanvasScaler));
            root.transform.SetParent(transform, false);
            root.transform.localPosition = new Vector3(0, 1.5f, 0);
            root.transform.localScale = Vector3.one * .01f;
            var canvas = root.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace; canvas.sortingOrder = 22;
            root.GetComponent<RectTransform>().sizeDelta = new Vector2(390, 70);
            var textObject = new GameObject("Text", typeof(Text));
            textObject.transform.SetParent(root.transform, false);
            var text = textObject.GetComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = 22; text.alignment = TextAnchor.MiddleCenter;
            text.color = new Color(.65f, .87f, .92f);
            text.text = (label ?? "조사 지점") + "\n[E/F · 게임패드 A] 확인";
            text.rectTransform.anchorMin = Vector2.zero; text.rectTransform.anchorMax = Vector2.one;
            text.rectTransform.offsetMin = text.rectTransform.offsetMax = Vector2.zero;
            return root;
        }
    }

    /// <summary>네 사람이 확인한 사실만 말하며 원인·제작자·푸른 빛의 정체는 답으로 제시하지 않습니다.</summary>
    public static class MainQuest10Dialogue
    {
        private static DialogueLine P(string text) => new DialogueLine(CompanionRosterService.PaulId, "폴", text);
        private static DialogueLine T(string text) => new DialogueLine(CompanionRosterService.TaeonId, "태온", text);
        private static DialogueLine M(string text) => new DialogueLine(CompanionRosterService.MielId, "미엘", text);
        private static DialogueLine U(string text) => new DialogueLine("", GameSessionData.PlayerName, text);

        public static DialogueLine[] Lines(int step)
        {
            switch (step)
            {
                case 0: return new[] { T("폴이 가리킨 석재가 여기입니다. 주변 흙과 모서리의 방향이 다릅니다."),
                    P("네. 전에 본 자리를 다시 확인해보죠. 기억보다 돌이 도망가지는 않았을 겁니다."), U("이어지는 부분을 살펴보겠습니다.") };
                case 1: return new[] { T("모서리 간격이 반복됩니다. 자연 암반과는 형태가 다릅니다."),
                    P("표면에 손댄 흔적이 있습니다. 오래된 구조의 일부일 가능성이 높지만, 누가 언제 만들었는지는 모르겠습니다."),
                    M("흙 아래쪽으로도 이어지는 것 같아요."), U("보이는 범위만 기록하고 안쪽으로 가보죠.") };
                case 2: return new[] { M("이쪽으로 올수록 벌레와 새 소리가 거의 들리지 않아요."),
                    T("몬스터의 흔적도 안쪽을 피해 돌아갑니다. 먼저 안전한 길을 확인하죠."),
                    P("귀를 쉬게 할 계획은 없었는데, 조용해지는 방향이 분명하군요.") };
                case 3: return new[] { U("무너진 돌 아래로 경사가 이어집니다."),
                    T("발 디딜 곳부터 살펴야 합니다. 흙이 밀린 자리는 피하세요."),
                    P("단단한 가장자리를 표시해두겠습니다. 안쪽 구조는 아직 보이지 않습니다.") };
                case 4: return new[] { M("덩굴 아래에 빈 공간이 있어요."),
                    P("오래된 석재와 안치 공간처럼 보이는 자리가 드러났습니다. 정확한 용도는 더 확인해야 합니다."),
                    U("침묵의 지하묘지로 이어지는 입구 같습니다. 지금 보이는 부분을 조사하죠.") };
                case 5: return new[] { T("몬스터들이 피하던 방향과 일치합니다."),
                    P("마력의 흐름도 아래쪽으로 향합니다. 관련은 있어 보이지만 이것이 원인인지는 아직 모릅니다."),
                    M("주변 생물 소리도 거의 들리지 않아요."), U("서두르지 말고 내려갈 경로를 확인하죠.") };
                default: return new[] { P("돌아갈 길과 단단한 가장자리는 표시했습니다. 아래쪽은 준비해서 확인하는 편이 좋겠습니다."),
                    T("무너진 돌을 밟지 않도록 입구 위치를 기억해두겠습니다."),
                    M("모두 상태를 확인하고 움직여요."), U("입구를 확인했습니다. 준비를 마친 뒤 안쪽을 조사하겠습니다.") };
            }
        }
    }
}

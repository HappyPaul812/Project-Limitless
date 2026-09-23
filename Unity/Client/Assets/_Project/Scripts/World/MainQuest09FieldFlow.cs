using System.Linq;
using ProjectLimitless.Core;
using ProjectLimitless.UI;
using ProjectLimitless.Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjectLimitless.World
{
    /// <summary>기존 Field03 위에 현재 퀘스트 단계의 조사 지점만 설치합니다. Scene 원본은 변경하지 않습니다.</summary>
    public sealed class MainQuest09FieldFlow : MonoBehaviour
    {
        public const string QuestId = "main_09_reunion_in_silence";
        public const string BlueTraceId = "field03_main09_blue_trace";
        public const string TracksId = "field03_main09_paul_tracks";
        public const string StructureId = "field03_main09_structural_trace";
        private GameObject blue, tracks, paul, structure;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Register()
        { SceneManager.sceneLoaded -= Install; SceneManager.sceneLoaded += Install; }

        private static void Install(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != "Field_03") return;
            if (DialoguePresenter.Instance == null) new GameObject("DialogueSystem").AddComponent<DialoguePresenter>();
            var root = new GameObject("MainQuest09FieldFlow");
            SceneManager.MoveGameObjectToScene(root, scene);
            root.AddComponent<MainQuest09FieldFlow>();
        }

        private void Awake()
        {
            blue = Site(BlueTraceId, "푸른 빛의 흔적", new Vector2(2.4f, -4.1f));
            tracks = Site(TracksId, "평행한 바퀴 자국", new Vector2(3.5f, -3.1f));
            paul = Site(CompanionRosterService.PaulId, "폴", new Vector2(1.8f, -1.4f));
            structure = Site(StructureId, "가공된 석재", new Vector2(-0.3f, -3.3f));
            var sprite = paul.AddComponent<SpriteRenderer>();
            sprite.sprite = Resources.LoadAll<Sprite>("BattleCharacters/Paul/Paul_Battle_Final").FirstOrDefault(x => x.name == "Paul_Idle_00");
            sprite.sortingOrder = 8; paul.transform.localScale = Vector3.one * .72f;
            Mark(blue.transform, Vector2.zero, new Vector2(.35f,.1f), new Color(.2f,.5f,.6f));
            Mark(tracks.transform, new Vector2(-.22f,0), new Vector2(.045f,.8f), new Color(.3f,.27f,.18f));
            Mark(tracks.transform, new Vector2(.22f,0), new Vector2(.045f,.8f), new Color(.3f,.27f,.18f));
            Mark(structure.transform, Vector2.zero, new Vector2(.75f,.28f), new Color(.48f,.52f,.48f));
        }

        private void OnEnable() { QuestService.Changed += Refresh; Refresh(); }
        private void OnDisable() { QuestService.Changed -= Refresh; }
        private void Refresh()
        {
            if (QuestService.GetState(QuestId) == QuestState.Available) QuestService.TryStart(QuestId);
            QuestRuntimeState quest = QuestService.ActiveMainQuest;
            int index = quest?.Definition.QuestId == QuestId ? quest.CurrentObjectiveIndex : -1;
            blue.SetActive(index == 0); tracks.SetActive(index == 1);
            paul.SetActive(index >= 2 && index <= 5);
            // Main10의 첫 조사에서 같은 석재를 다시 사용합니다. 복제 오브젝트는 만들지 않습니다.
            bool revisiting = QuestService.ActiveMainQuest?.Definition.QuestId == MainQuest10FieldFlow.QuestId
                && QuestService.ActiveMainQuest.CurrentObjectiveIndex == 0;
            structure.SetActive(index == 4 || revisiting);
        }

        private GameObject Site(string id, string label, Vector2 position)
        {
            var site = new GameObject(id); site.transform.SetParent(transform); site.transform.position = position;
            var interactable = site.AddComponent<MainQuest09Interactable>(); interactable.TargetId = id; interactable.Label = label;
            QuestNavigationTarget.Attach(site, id, label, new Vector3(0,1.45f,0));
            return site;
        }

        private static void Mark(Transform parent, Vector2 position, Vector2 scale, Color color)
        {
            var marker = GameObject.CreatePrimitive(PrimitiveType.Quad); marker.name = "InvestigationMark";
            Destroy(marker.GetComponent<Collider>()); marker.transform.SetParent(parent, false);
            marker.transform.localPosition = position; marker.transform.localScale = scale;
            var material = new Material(Shader.Find("Sprites/Default")); material.color = color;
            marker.GetComponent<MeshRenderer>().sharedMaterial = material;
            marker.GetComponent<MeshRenderer>().sortingOrder = 4;
            marker.AddComponent<MainQuest09MarkerMaterial>().OwnedMaterial = material;
        }
    }

    /// <summary>런타임 생성 표시용 Material의 수명을 Scene에 맞춥니다.</summary>
    public sealed class MainQuest09MarkerMaterial : MonoBehaviour
    {
        public Material OwnedMaterial;
        private void OnDestroy() { if (OwnedMaterial != null) Destroy(OwnedMaterial); }
    }

    /// <summary>대화가 끝나야 현재 목표 하나만 진행하며 취소·거리 이탈은 진행시키지 않습니다.</summary>
    public sealed class MainQuest09Interactable : MonoBehaviour
    {
        public string TargetId;
        public string Label;
        private InputAction interact;
        private Transform player;
        private GameObject marker;
        private static int completedFrame = -1;
        private bool IsCurrent => QuestService.ActiveMainQuest?.Definition.QuestId == MainQuest09FieldFlow.QuestId
            && QuestService.ActiveMainQuest.CurrentObjective?.TargetId == TargetId;
        private void Awake()
        {
            interact = new InputAction("Main09Interact", InputActionType.Button);
            interact.AddBinding("<Keyboard>/e"); interact.AddBinding("<Keyboard>/f"); interact.AddBinding("<Gamepad>/buttonSouth");
            interact.performed += _ => TryInteract();
        }
        private void Start() => marker = CreateMarker();
        private void Update()
        {
            if (marker != null) marker.SetActive(IsCurrent && !WorldModalState.IsOpen);
        }
        private void OnEnable() => interact.Enable();
        private void OnDisable() => interact.Disable();
        private void OnDestroy() => interact.Dispose();

        public bool TryInteract()
        {
            if (!IsCurrent || WorldModalState.IsOpen || completedFrame == Time.frameCount) return false;
            if (player == null) player = FindAnyObjectByType<PlayerController>()?.transform;
            if (player == null || Vector2.Distance(player.position, transform.position) > 1.6f) return false;
            int step = QuestService.ActiveMainQuest.CurrentObjectiveIndex;
            DialoguePresenter.Instance.ShowSequence(MainQuest09Dialogue.Lines(step), () => Complete(step));
            DialoguePresenter.Instance.TrackDistance(player, transform, 3f);
            return true;
        }

        private void Complete(int step)
        {
            if (!IsCurrent || QuestService.ActiveMainQuest.CurrentObjectiveIndex != step) return;
            completedFrame = Time.frameCount;
            if (TargetId == CompanionRosterService.PaulId) QuestService.NotifyNpcTalked(TargetId);
            else QuestService.NotifyInteraction(TargetId);
            if (QuestService.GetState(MainQuest09FieldFlow.QuestId) == QuestState.Completed)
                QuestHudPresenter.Notify("새로운 동료가 합류했습니다.\n폴 · 마도사 · 지체의 길\n안전지역의 파티 안내인에게 편성을 변경할 수 있습니다.");
            if (GameSaveService.CurrentSlotIndex > 0) GameSaveService.SaveCurrentSession();
        }

        private GameObject CreateMarker()
        {
            var root = new GameObject("QuestMarker", typeof(Canvas), typeof(CanvasScaler));
            root.transform.SetParent(transform, false); root.transform.localPosition = new Vector3(0,1.5f,0);
            root.transform.localScale = Vector3.one * .01f;
            var canvas = root.GetComponent<Canvas>(); canvas.renderMode = RenderMode.WorldSpace; canvas.sortingOrder = 22;
            root.GetComponent<RectTransform>().sizeDelta = new Vector2(360,70);
            var textObject = new GameObject("Text", typeof(Text)); textObject.transform.SetParent(root.transform,false);
            var label = textObject.GetComponent<Text>(); label.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            label.fontSize = 23; label.alignment = TextAnchor.MiddleCenter; label.color = new Color(.4f,.82f,1f);
            label.text = Label + "\n[E/F · 게임패드 A] 확인";
            label.rectTransform.anchorMin = Vector2.zero; label.rectTransform.anchorMax = Vector2.one;
            label.rectTransform.offsetMin = label.rectTransform.offsetMax = Vector2.zero;
            return root;
        }
    }

    /// <summary>확인한 사실과 추측을 구분하고 폴의 독립적인 조사 판단으로 합류를 연결합니다.</summary>
    public static class MainQuest09Dialogue
    {
        private static DialogueLine P(string text) => new DialogueLine(CompanionRosterService.PaulId, "폴", text);
        private static DialogueLine T(string text) => new DialogueLine(CompanionRosterService.TaeonId, "태온", text);
        private static DialogueLine M(string text) => new DialogueLine(CompanionRosterService.MielId, "미엘", text);
        private static DialogueLine U(string text) => new DialogueLine("", GameSessionData.PlayerName, text);
        public static DialogueLine[] Lines(int step)
        {
            switch (step)
            {
                case 0: return new[] { M("빛은 이쪽에서 보였어요. 지금은 사라졌네요."), T("빛의 정체는 아직 모릅니다. 남아 있는 흔적부터 보죠."), U("발밑을 확인하면서 따라가겠습니다.") };
                case 1: return new[] { T("두 줄의 폭이 일정합니다. 이번에는 깊게 빠진 자국이 아니에요."), M("단단한 지면을 따라 이어져 있네요. 폴 씨도 이쪽으로 가셨을까요?"), P("그 길은 괜찮습니다! 오른쪽 가장자리만 피해주세요.") };
                case 2: return new[] { P("또 뵙네요. 이번에는 제가 길을 안내해드릴 차례인가 봅니다."), M("혼자 여기까지 오신 거예요? 다치신 곳은요?"), P("괜찮습니다. 오늘 계획표에는 진흙에 빠지는 일정도 빼뒀고요."), T("지나오신 길을 확인하고 계셨습니까?"), P("네. 단단한 땅과 돌아갈 길을 먼저 표시해뒀습니다. 여러분은 괜찮으세요?"), U("저희도 무사합니다. 서로 확인한 것을 이야기해보죠.") };
                case 3: return new[] { U("몬스터들이 깊은 곳을 둥글게 피하고 있습니다. 안쪽으로 갈수록 수도 줄었고요."), T("이동 흔적이 방향을 바꾸는 지점이 있었습니다."), M("조금 전에는 짧게 푸른 빛도 봤어요."), P("빛은 저도 봤습니다. 제 마법인지부터 묻고 싶으시겠지만, 저도 답을 찾는 중입니다."), P("제가 지나온 길에서는 마력 흐름이 한 방향으로 치우쳐 있었습니다. 평소 숲의 지형과 다른 석재도 발견했고요."), P("제가 확인한 건 여기까지입니다. 서로 관련이 있는지, 원인이 무엇인지는 아직 모릅니다."), T("사실과 추측을 나눠두는 게 좋겠습니다."), P("동의합니다. 석재는 저쪽 낮은 지점에 있습니다. 가장자리 지반은 밟지 마세요.") };
                case 4: return new[] { T("모서리의 간격이 반복됩니다. 자연 암반과는 다릅니다."), P("표면에 가공한 자국도 있습니다. 오래된 구조물 일부로 보이지만, 용도는 모르겠습니다."), M("흙 아래로 더 이어지는 것 같아요."), U("지금 보이는 부분만 기록하죠. 아래가 안전한지는 아직 모르니까요."), P("좋습니다. 무너진 부분과 돌아갈 길을 표시해두겠습니다.") };
                default: return new[] { P("각자 본 것만으로는 놓치는 게 있네요. 현상도 여기서 끝난 것 같지 않고요."), P("같은 방향을 조사하고 있으니, 계속 정보를 나누는 편이 합리적이겠습니다. 저도 함께 가도 될까요?"), U("함께 가죠. 확인한 것을 서로 알려주면 좋겠습니다."), T("안전한 경로부터 같이 확인하겠습니다."), M("쉬어야 할 때도 말씀해주세요. 그건 모두에게 하는 말이에요."), P("그럼 쉬는 시간도 계획에 넣겠습니다. 빈칸을 남겨둔 보람이 있네요."), P("우선 여기서 정리하고, 더 깊은 곳은 준비해서 확인하죠.") };
            }
        }
    }
}

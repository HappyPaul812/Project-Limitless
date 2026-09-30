using System;
using System.Linq;
using ProjectLimitless.Core;
using ProjectLimitless.NPC;
using ProjectLimitless.Player;
using ProjectLimitless.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace ProjectLimitless.World
{
    /// <summary>Main15의 Arbel 대화와 서쪽 조사 지점을 기존 Quest·Scene 위에 설치합니다.</summary>
    public sealed class Chapter2Main15Flow : MonoBehaviour
    {
        public const string QuestId = "main_15_burning_traces";
        private const string Field = "Field_06_ScorchedTrail";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Register()
        {
            SceneManager.sceneLoaded -= Install;
            SceneManager.sceneLoaded += Install;
        }

        private static void Install(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != "Arbel" && scene.name != Field) return;
            var root = new GameObject("Chapter2Main15Flow", typeof(Chapter2Main15Flow));
            SceneManager.MoveGameObjectToScene(root, scene);
        }

        private void Start()
        {
            if (DialoguePresenter.Instance == null) new GameObject("DialogueSystem").AddComponent<DialoguePresenter>();
            if (gameObject.scene.name == "Arbel")
            {
                GameObject gate = GameObject.Find("Transition_northwest_to_field06");
                if (gate != null)
                {
                    gate.AddComponent<Main15AccessGate>();
                    QuestNavigationTarget.Attach(gate, "field06_east_entry", "서쪽 황토 길");
                }
                Site("arbel_main15_return", "아르벨 귀환", new Vector2(-6, 5), true);
                var serin = new GameObject("arbel-serin", typeof(SpriteRenderer), typeof(NpcController), typeof(VillageNpcRole));
                serin.transform.SetParent(transform, false);
                serin.transform.position = new Vector2(-4, 4);
                serin.GetComponent<NpcController>().Configure("세린", "서쪽을 더 확인하겠습니다.");
                serin.GetComponent<VillageNpcRole>().Configure("arbel-serin", VillageNpcRoleType.Resident);
                var controller = Resources.Load<RuntimeAnimatorController>("Chapter2/Serin");
                if (controller != null) serin.AddComponent<Animator>().runtimeAnimatorController = controller;
                QuestNavigationTarget.Attach(serin, "arbel-serin", "세린", new Vector3(0, 1.8f));
            }
            else
            {
                OpenEastBoundary();
                Site("field06_east_entry", "서쪽 황토 길", new Vector2(7, 0), true);
                Site("field06_main15_soot", "그을린 흔적", new Vector2(5, -2));
                Site("field06_main15_tracks", "동쪽으로 향한 발자국", new Vector2(2, 2));
                Site("field06_main15_pulse", "짧아진 지면 울림", new Vector2(-1, -2));
                Site("field06_main15_crack", "갈라진 지면", new Vector2(-4, 2));
                Site("field06_main15_heat", "검게 변한 바위", new Vector2(-7, -2));
                var serin = new GameObject("field06-serin", typeof(SpriteRenderer), typeof(NpcController), typeof(VillageNpcRole));
                serin.transform.SetParent(transform, false);
                serin.transform.position = new Vector2(-6, 1);
                serin.GetComponent<NpcController>().Configure("세린", "서쪽의 울림을 더 살펴보죠.");
                serin.GetComponent<VillageNpcRole>().Configure("field06-serin", VillageNpcRoleType.Resident);
                var controller = Resources.Load<RuntimeAnimatorController>("Chapter2/Serin");
                if (controller != null) serin.AddComponent<Animator>().runtimeAnimatorController = controller;
                QuestNavigationTarget.Attach(serin, "field06-serin", "세린", new Vector3(0, 1.8f));
                BuildEnvironment();
            }
        }

        private void Site(string id, string label, Vector2 position, bool arrival = false)
        {
            var site = new GameObject(id, typeof(Main15Site));
            site.transform.SetParent(transform, false);
            site.transform.position = position;
            site.GetComponent<Main15Site>().Configure(id, label, arrival);
            QuestNavigationTarget.Attach(site, id, label, new Vector3(0, 1.3f));
        }

        private void OpenEastBoundary()
        {
            // 복제한 Field의 동쪽 벽은 실제 출입구 중앙만 비워 두고 위아래 충돌을 보존합니다.
            Transform old = gameObject.scene.GetRootGameObjects().SelectMany(root => root.GetComponentsInChildren<Transform>(true))
                .FirstOrDefault(child => child.name == "BoundaryRight");
            if (old != null) Destroy(old.gameObject);
            for (int i = 0; i < 2; i++)
            {
                var wall = new GameObject("Main15BoundaryRight" + i, typeof(BoxCollider2D));
                wall.transform.SetParent(transform, false);
                wall.transform.position = new Vector2(10.5f, i == 0 ? -4.5f : 4.5f);
                wall.GetComponent<BoxCollider2D>().size = new Vector2(.3f, 6);
            }
            // 복제 원본의 북쪽 중앙에는 기존 출구 틈이 남아 있습니다. Field06은 동쪽만 실제
            // 출구이므로 이 틈을 막아 플레이어가 전환 없이 맵 밖으로 빠지지 않게 합니다.
            var northWall = new GameObject("Main15BoundaryTop", typeof(BoxCollider2D));
            northWall.transform.SetParent(transform, false);
            northWall.transform.position = new Vector2(0, 7.7f);
            northWall.GetComponent<BoxCollider2D>().size = new Vector2(4, 1);
        }

        private void BuildEnvironment()
        {
            // 기존 숲 Field를 복제한 바탕 위에 서쪽으로 갈수록 적은 수의 마른 흙·균열·그을음 표식을 더합니다.
            // 모두 비충돌 표식이며 Scene의 사용자 원본 Sprite는 바꾸지 않습니다.
            foreach (SpriteRenderer plant in gameObject.scene.GetRootGameObjects()
                         .SelectMany(root => root.GetComponentsInChildren<SpriteRenderer>(true)))
            {
                float x = plant.transform.position.x;
                if (plant.name.StartsWith("Grass_", StringComparison.Ordinal) && x < 2f)
                {
                    if (x < -3f && Mathf.Abs(Mathf.RoundToInt(plant.transform.position.y)) % 2 == 0)
                        plant.gameObject.SetActive(false);
                    else plant.color = new Color(.72f, .55f, .3f, 1f);
                }
                else if (plant.name.StartsWith("Tree", StringComparison.Ordinal) && x < 0f)
                    plant.color = new Color(.55f, .39f, .29f, 1f);
                else if (plant.name.StartsWith("Rock", StringComparison.Ordinal) && x < 0f)
                    plant.color = new Color(.38f, .31f, .28f, 1f);
            }
            Patch("DrySoilEast", new Vector2(4, -3), new Vector2(3, 1), new Color(.55f, .36f, .23f));
            Patch("OchreGround", new Vector2(0, 3), new Vector2(4, 1.3f), new Color(.58f, .32f, .18f));
            Patch("CrackedGround", new Vector2(-4, 2), new Vector2(3, .18f), new Color(.22f, .14f, .12f));
            Patch("CrackBranch", new Vector2(-3.6f, 2.35f), new Vector2(.16f, 1.1f), new Color(.22f, .14f, .12f));
            Patch("ScorchedRock", new Vector2(-7, -2), new Vector2(1.3f, .75f), new Color(.19f, .17f, .16f));
            Patch("DryGrass", new Vector2(5, 2), new Vector2(.7f, .4f), new Color(.57f, .43f, .2f));
        }

        private void Patch(string name, Vector2 position, Vector2 size, Color color)
        {
            var mark = GameObject.CreatePrimitive(PrimitiveType.Quad);
            mark.name = name;
            Destroy(mark.GetComponent<Collider>());
            mark.transform.SetParent(transform, false);
            mark.transform.localPosition = position;
            mark.transform.localScale = size;
            var renderer = mark.GetComponent<MeshRenderer>();
            renderer.sharedMaterial = new Material(Shader.Find("Sprites/Default")) { color = color };
            renderer.sortingOrder = 2;
        }

        public static bool TryHandleNpc(string id, NpcController npc)
        {
            bool first = id == "arbel-leon" && QuestService.GetState(QuestId) == QuestState.Available;
            QuestRuntimeState quest = QuestService.ActiveMainQuest;
            bool current = quest?.Definition.QuestId == QuestId && quest.CurrentObjective?.TargetId == id;
            if (!first && !current) return false;
            DialogueLine[] lines;
            if (first) lines = new[] { new DialogueLine(id, "레온", "서쪽 길을 다녀온 사람들이 땅이 뜨겁다고 합니다. 작은 불 흔적은 있지만 큰 불길을 본 사람은 없어요."),
                new DialogueLine(CompanionRosterService.SerinId, "세린", "저도 계속 서쪽을 확인할 생각이에요. 같이 살펴보죠.") };
            else if (id == "field06-serin") lines = new[]
            {
                new DialogueLine(CompanionRosterService.SerinId, "세린", "또 시작됐어요. 지난번보다 간격이 조금 짧아요. 서쪽에서 오지만 위치까지는 모르겠습니다."),
                new DialogueLine("companion_paul", "폴", "이 지면은 제 휠에도 별로 반갑지 않네요."),
                new DialogueLine(CompanionRosterService.SerinId, "세린", "땅이 뜨거워서요?"),
                new DialogueLine("companion_paul", "폴", "타이어도 오늘은 기분이 나쁜 모양입니다.")
            };
            else if (id == "arbel-leon") lines = new[]
            { new DialogueLine(id, "레온", "변한 동물과 균열, 더 강한 열기와 진동이 있었군요. 원인을 단정하지 않고 주민들에게 대비를 알리겠습니다.") };
            else lines = new[]
            {
                new DialogueLine(CompanionRosterService.SerinId, "세린", "저도 계속 서쪽을 확인할 생각이에요. 혼자보다 함께 움직이는 편이 낫겠습니다."),
                new DialogueLine("companion_taeon", "태온", "그렇다면 앞으로도 함께하시겠습니까?"),
                new DialogueLine(CompanionRosterService.SerinId, "세린", "네. 당분간이 아니어도 괜찮다면요."),
                new DialogueLine("companion_miel", "미엘", "물론이에요."),
                new DialogueLine("companion_paul", "폴", "제 일이 줄어든다면 언제나 환영입니다.")
            };
            DialoguePresenter.Instance?.ShowSequence(lines, () =>
            {
                if (first && !QuestService.TryStart(QuestId)) return;
                if (QuestService.ActiveMainQuest?.Definition.QuestId != QuestId) return;
                QuestService.NotifyNpcTalked(id);
                if (GameSaveService.CurrentSlotIndex > 0) GameSaveService.SaveCurrentSession();
            });
            return true;
        }
    }

    /// <summary>레온의 의뢰를 듣기 전에는 새 북서쪽 통행을 열지 않습니다.</summary>
    public sealed class Main15AccessGate : MonoBehaviour
    {
        private BoxCollider2D entrance;
        private void Awake() => entrance = GetComponent<BoxCollider2D>();
        private void Update()
        {
            QuestState state = QuestService.GetState(Chapter2Main15Flow.QuestId);
            if (entrance != null) entrance.enabled = state == QuestState.Active || state == QuestState.Completed;
        }
    }

    /// <summary>조사 입력은 현재 순차 목표와 플레이어 거리·모달 상태를 모두 만족할 때만 전달합니다.</summary>
    public sealed class Main15Site : MonoBehaviour
    {
        private string targetId, label;
        private bool arrival;
        private InputAction action;
        private TextMesh marker;
        public void Configure(string id, string display, bool isArrival)
        { targetId = id; label = display; arrival = isArrival; }
        private bool Current => QuestService.ActiveMainQuest?.Definition.QuestId == Chapter2Main15Flow.QuestId &&
            QuestService.ActiveMainQuest.CurrentObjective?.TargetId == targetId;
        private void Awake()
        {
            action = new InputAction("Main15Inspect", InputActionType.Button);
            action.AddBinding("<Keyboard>/e"); action.AddBinding("<Keyboard>/f");
            action.AddBinding("<Gamepad>/buttonSouth");
            action.performed += _ => Interact();
        }
        private void Start()
        {
            marker = new GameObject("SiteLabel", typeof(TextMesh)).GetComponent<TextMesh>();
            marker.transform.SetParent(transform, false);
            marker.transform.localPosition = new Vector3(0, 1, 0);
            marker.text = label + (arrival ? "" : " [E/F]");
            marker.characterSize = .12f; marker.anchor = TextAnchor.MiddleCenter;
        }
        private void OnEnable() => action?.Enable();
        private void OnDisable() => action?.Disable();
        private void OnDestroy() => action?.Dispose();
        private void Update()
        {
            if (marker != null) marker.gameObject.SetActive(Current && !WorldModalState.IsOpen);
            if (arrival && Current && !WorldModalState.IsOpen && IsPlayerNear())
            {
                QuestService.NotifyLocationReached(targetId);
                if (GameSaveService.CurrentSlotIndex > 0) GameSaveService.SaveCurrentSession();
            }
        }
        private bool IsPlayerNear()
        {
            PlayerController player = FindAnyObjectByType<PlayerController>();
            return player != null && Vector2.Distance(player.transform.position, transform.position) < 1.7f;
        }
        private void Interact()
        {
            if (arrival || !Current || WorldModalState.IsOpen || !IsPlayerNear()) return;
            DialogueLine[] lines = Lines(targetId);
            DialoguePresenter.Instance?.ShowSequence(lines, () =>
            {
                if (!Current) return;
                QuestService.NotifyInteraction(targetId);
                if (GameSaveService.CurrentSlotIndex > 0) GameSaveService.SaveCurrentSession();
            });
        }
        private static DialogueLine[] Lines(string id)
        {
            switch (id)
            {
                case "field06_main15_soot": return new[]
                {
                    new DialogueLine("companion_miel", "미엘", "몸에 불이 붙은 건가요?"),
                    new DialogueLine("companion_paul", "폴", "불이 붙었다기보다 몸 자체가 변한 것 같습니다."),
                    new DialogueLine("companion_taeon", "태온", "가까이 가기 전에 상태를 확인하죠."),
                    new DialogueLine(CompanionRosterService.SerinId, "세린", "움직임은 보통 야생동물과 비슷해요.")
                };
                case "field06_main15_tracks": return new[] { new DialogueLine("companion_taeon", "태온", "발자국이 동쪽으로 향합니다. 이곳에서 도망치는 듯합니다.") };
                case "field06_main15_pulse": return new[] { new DialogueLine(CompanionRosterService.SerinId, "세린", "또 시작됐어요. 간격은 조금 짧고 강해졌지만 완전히 일정하지는 않아요.") };
                case "field06_main15_crack": return new[]
                {
                    new DialogueLine("companion_paul", "폴", "이 아래에 열원이 있는 건 맞는 것 같습니다."),
                    new DialogueLine("companion_taeon", "태온", "그렇다고 여기가 시작점이라고 단정할 수는 없습니다."),
                    new DialogueLine(CompanionRosterService.SerinId, "세린", "진동은 더 서쪽에서 옵니다.")
                };
                default: return new[] { new DialogueLine("companion_paul", "폴", "바위는 일부 검고 식물은 말랐습니다. 큰 산불 흔적 없이 지면의 열이 오른 것 같군요.") };
            }
        }
    }
}

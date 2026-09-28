using ProjectLimitless.Core;
using ProjectLimitless.Player;
using ProjectLimitless.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjectLimitless.World
{
    /// <summary>
    /// 기존 B1/B2 Scene 위에 Main11 조사 지점만 설치합니다. Scene 원본과 몬스터 배치를 다시 만들지 않으며,
    /// 저장된 Quest 목표 번호를 기준으로 하강로·봉인실·석판의 상태를 매번 복원합니다.
    /// </summary>
    public sealed class MainQuest11DungeonFlow : MonoBehaviour
    {
        public const string QuestId = "main_11_silent_catacomb";
        public const string TabletItemId = "quest_broken_catacomb_tablet";
        public const string BossId = "dungeon01_b2_boss";
        private const string B1 = "Dungeon_01";
        private const string B2 = "Dungeon_01_B2";
        private bool bossAfterDialogueShown;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Register()
        {
            SceneManager.sceneLoaded -= Install;
            SceneManager.sceneLoaded += Install;
        }

        private static void Install(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != B1 && scene.name != B2) return;
            if (DialoguePresenter.Instance == null) new GameObject("DialogueSystem").AddComponent<DialoguePresenter>();
            var root = new GameObject("MainQuest11DungeonFlow", typeof(MainQuest11DungeonFlow));
            SceneManager.MoveGameObjectToScene(root, scene);
        }

        public static int CurrentStep => QuestService.ActiveMainQuest?.Definition.QuestId == QuestId
            ? QuestService.ActiveMainQuest.CurrentObjectiveIndex : -1;
        public static bool BossDefeated => QuestService.GetState(QuestId) == QuestState.Completed || CurrentStep >= 10;
        public static bool IsBossAvailable => QuestService.GetState(QuestId) != QuestState.Completed &&
            (CurrentStep < 0 || CurrentStep == 9);

        private void Awake()
        {
            if (gameObject.scene.name == B1)
            {
                // Main10의 입구와 파티·보급·안전지대는 그대로 두고 실제 던전 진입에서 시작합니다.
                if (QuestService.GetState(QuestId) == QuestState.Available) QuestService.TryStart(QuestId);
                Site("dungeon01_main11_b1_central", "B1 중앙 회랑", new Vector2(0, -5.1f), true);
                Site("dungeon01_main11_west_ossuary", "서쪽 안치실", new Vector2(-10, 0), false);
                Site("dungeon01_main11_east_chamber", "동쪽 무너진 묘실", new Vector2(10, 0), false);
                Site("dungeon01_main11_lower_gate", "봉인된 하강로", new Vector2(0, 8.2f), false);
                var spawn = new GameObject("Spawn_From_B2", typeof(SceneSpawnPoint));
                spawn.transform.SetParent(transform, false);
                spawn.transform.position = new Vector3(0, 6.8f, 0);
                spawn.GetComponent<SceneSpawnPoint>().Configure("Spawn_From_B2");
            }
            else
            {
                Site("dungeon01_main11_b2_entry", "B2 입구", new Vector2(0, -8.5f), true);
                Site("dungeon01_main11_b2_entrance", "사용되지 않은 안치 공간", new Vector2(0, -7.3f), false);
                Site("dungeon01_main11_sealed_tombs", "서쪽 폐쇄 묘실", new Vector2(-10, -1), false);
                Site("dungeon01_main11_unknown_room", "동쪽 용도불명실", new Vector2(10, -1), false);
                Site("dungeon01_main11_blue_flow", "중앙 석실의 푸른 흐름", new Vector2(0, 0), false);
                Site("dungeon01_main11_sealed_chamber", "파수꾼 뒤 봉인실", new Vector2(0, 11.2f), false);
                Site("dungeon01_main11_broken_tablet", "부서진 석판 조각", new Vector2(1.25f, 11.35f), false);
                BuildSealedChamber();
            }
        }

        private void Start()
        {
            if (gameObject.scene.name != B2 || CurrentStep != 10 || bossAfterDialogueShown) return;
            bossAfterDialogueShown = true;
            DialoguePresenter.Instance.ShowSequence(MainQuest11Dialogue.AfterBoss, null);
        }

        private void Site(string id, string label, Vector2 position, bool arrival)
        {
            var site = new GameObject(id, typeof(MainQuest11Site));
            site.transform.SetParent(transform, false);
            site.transform.position = position;
            site.GetComponent<MainQuest11Site>().Configure(id, label, arrival);
            QuestNavigationTarget.Attach(site, id, label, new Vector3(0, 1.4f, 0));
        }

        private void BuildSealedChamber()
        {
            // 기존 B2의 보스 뒤 Anchor와 석재 계열을 따라 작은 단서 공간만 표시합니다.
            // 아래에서 올라온 빛이 파손된 돌 사이로 지나가므로 구조물이 빛의 원천처럼 보이지 않습니다.
            Stone("DeepCrack", new Vector2(0, 11.5f), new Vector2(2.3f, .22f), new Color(.045f, .075f, .1f), 2);
            Stone("BlueFromBelow", new Vector2(0, 11.62f), new Vector2(.35f, .85f), new Color(.22f, .47f, .68f, .75f), 3);
            Stone("BrokenStoneLeft", new Vector2(-.72f, 11.45f), new Vector2(.8f, .55f), new Color(.54f, .58f, .6f), 4);
            Stone("BrokenStoneRight", new Vector2(.75f, 11.7f), new Vector2(.7f, .48f), new Color(.51f, .55f, .57f), 4);
            for (int i = 0; i < 3; i++)
                Stone("RepeatedUnknownMark" + i, new Vector2(-.9f + i * .9f, 11.98f), new Vector2(.13f, .13f),
                    new Color(.18f, .37f, .51f), 5);
            var barrier = new GameObject("BossChamberBarrier", typeof(BoxCollider2D), typeof(MainQuest11BossBarrier));
            barrier.transform.SetParent(transform, false);
            barrier.transform.position = new Vector3(0, 10.5f, 0);
            barrier.GetComponent<BoxCollider2D>().size = new Vector2(11f, .28f);
        }

        private void Stone(string name, Vector2 position, Vector2 size, Color color, int order)
        {
            var stone = new GameObject(name, typeof(SpriteRenderer));
            stone.transform.SetParent(transform, false);
            stone.transform.position = position;
            // B2 바닥이 이미 사용하는 석재 Sprite를 참조해 새 이미지나 임시 텍스처를 만들지 않습니다.
            var existingStone = GameObject.Find("Dungeon01B2_Environment")?.GetComponentInChildren<SpriteRenderer>();
            var sprite = existingStone != null ? existingStone.sprite : null;
            if (sprite == null) { Destroy(stone); return; }
            var renderer = stone.GetComponent<SpriteRenderer>();
            renderer.sprite = sprite; renderer.color = color; renderer.sortingOrder = order;
            stone.transform.localScale = new Vector3(size.x / sprite.bounds.size.x, size.y / sprite.bounds.size.y, 1);
        }
    }

    /// <summary>보스 목표 완료 전에는 뒤쪽 작은 봉인실로 지나가지 못하게 하는 물리 경계입니다.</summary>
    public sealed class MainQuest11BossBarrier : MonoBehaviour
    {
        private void Update() => GetComponent<Collider2D>().enabled = !MainQuest11DungeonFlow.BossDefeated;
    }

    /// <summary>각 조사 지점은 현재 순차 목표일 때만 입력을 받고, 대사를 끝까지 본 뒤에만 진행합니다.</summary>
    public sealed class MainQuest11Site : MonoBehaviour
    {
        public string TargetId { get; private set; }
        private string label;
        private bool arrival;
        private InputAction interact;
        private GameObject marker;
        private Transform player;
        private static int completedFrame = -1;

        public void Configure(string id, string displayName, bool isArrival)
        { TargetId = id; label = displayName; arrival = isArrival; }

        private void Awake()
        {
            interact = new InputAction("Main11Interact", InputActionType.Button);
            interact.AddBinding("<Keyboard>/e"); interact.AddBinding("<Keyboard>/f");
            interact.AddBinding("<Gamepad>/buttonSouth");
            interact.performed += _ => TryInteract();
        }

        private void Start() => marker = CreateMarker();
        private void OnEnable() => interact?.Enable();
        private void OnDisable() => interact?.Disable();
        private void OnDestroy() => interact?.Dispose();

        private bool IsCurrent => QuestService.ActiveMainQuest?.Definition.QuestId == MainQuest11DungeonFlow.QuestId
            && QuestService.ActiveMainQuest.CurrentObjective?.TargetId == TargetId;
        private bool IsReturnChoice => TargetId == "dungeon01_main11_broken_tablet" && MainQuest11DungeonFlow.CurrentStep == 12;
        private bool IsOpenGate => TargetId == "dungeon01_main11_lower_gate" &&
            (MainQuest11DungeonFlow.CurrentStep >= 4 || QuestService.GetState(MainQuest11DungeonFlow.QuestId) == QuestState.Completed);

        private void Update()
        {
            if (player == null) player = FindAnyObjectByType<PlayerController>()?.transform;
            if (marker != null) marker.SetActive((IsCurrent || IsReturnChoice || IsOpenGate) && !WorldModalState.IsOpen);
            if (arrival && IsCurrent && player != null && Vector2.Distance(player.position, transform.position) < 2.2f)
            {
                QuestService.NotifyLocationReached(TargetId);
                if (GameSaveService.CurrentSlotIndex > 0) GameSaveService.SaveCurrentSession();
            }
        }

        public bool TryInteract()
        {
            if (arrival || (!IsCurrent && !IsReturnChoice && !IsOpenGate) || WorldModalState.IsOpen || completedFrame == Time.frameCount)
                return false;
            if (player == null) player = FindAnyObjectByType<PlayerController>()?.transform;
            if (player == null || Vector2.Distance(player.position, transform.position) > 1.8f) return false;
            if (IsReturnChoice) { ShowReturnChoice(); return true; }
            if (IsOpenGate)
            {
                SceneTransitionService.Load("Dungeon_01_B2", "Spawn_From_B1_Test");
                return true;
            }
            int step = MainQuest11DungeonFlow.CurrentStep;
            DialoguePresenter.Instance.ShowSequence(MainQuest11Dialogue.Lines(step), () => Complete(step));
            DialoguePresenter.Instance.TrackDistance(player, transform, 3f);
            return true;
        }

        private void Complete(int step)
        {
            if (!IsCurrent || MainQuest11DungeonFlow.CurrentStep != step) return;
            if (step == 11 && InventoryService.GetItemCount(MainQuest11DungeonFlow.TabletItemId) == 0 &&
                !InventoryService.TryAddItem(MainQuest11DungeonFlow.TabletItemId, 1)) return;
            // 석판 획득과 목표 진행을 한 저장 경계에 묶고, 중복 입력에서는 한 번만 지급합니다.
            completedFrame = Time.frameCount;
            QuestService.NotifyInteraction(TargetId);
            if (GameSaveService.CurrentSlotIndex > 0) GameSaveService.SaveCurrentSession();
            if (step == 11) ShowReturnChoice();
        }

        private void ShowReturnChoice()
        {
            DialoguePresenter.Instance.ShowConfirmation("", "지하묘지 입구로 돌아가시겠습니까?",
                "돌아간다", "아직 조사한다", () =>
                {
                    // 귀환 선택은 목적지로 이동만 합니다. 최종 완료는 실제 안전지대 도착이 확인한 뒤입니다.
                    SceneTransitionService.Load("Field_03", "Spawn_From_Dungeon01");
                });
        }

        private GameObject CreateMarker()
        {
            var root = new GameObject("QuestMarker", typeof(Canvas), typeof(CanvasScaler));
            root.transform.SetParent(transform, false);
            root.transform.localPosition = new Vector3(0, 1.3f, 0);
            root.transform.localScale = Vector3.one * .01f;
            var canvas = root.GetComponent<Canvas>(); canvas.renderMode = RenderMode.WorldSpace; canvas.sortingOrder = 22;
            root.GetComponent<RectTransform>().sizeDelta = new Vector2(390, 70);
            var text = new GameObject("Text", typeof(Text)).GetComponent<Text>();
            text.transform.SetParent(root.transform, false);
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = 22; text.alignment = TextAnchor.MiddleCenter;
            text.color = new Color(.82f, .9f, 1f);
            text.text = label + (arrival ? "\n이동하여 확인" : "\n[E/F · 게임패드 A] 확인");
            text.rectTransform.anchorMin = Vector2.zero; text.rectTransform.anchorMax = Vector2.one;
            text.rectTransform.offsetMin = text.rectTransform.offsetMax = Vector2.zero;
            return root;
        }
    }

    /// <summary>확인한 흔적과 추측을 나눠 말하고, 세 동료의 기존 말투를 유지합니다.</summary>
    public static class MainQuest11Dialogue
    {
        private static DialogueLine P(string text) => new DialogueLine(CompanionRosterService.PaulId, "폴", text);
        private static DialogueLine T(string text) => new DialogueLine(CompanionRosterService.TaeonId, "태온", text);
        private static DialogueLine M(string text) => new DialogueLine(CompanionRosterService.MielId, "미엘", text);
        private static DialogueLine U(string text) => new DialogueLine("", GameSessionData.PlayerName, text);

        public static DialogueLine[] AfterBoss => new[]
        { P("멈췄는데도 주변의 빛은 그대로군요."), T("이 존재가 빛의 근원은 아니었던 것 같습니다."),
          M("뒤쪽을 보세요. 길이 열린 것 같아요.") };

        public static DialogueLine[] Lines(int step)
        {
            switch (step)
            {
                case 1: return new[] { T("석관과 묘표가 남아 있습니다. 실제 안치 공간으로 쓰였던 곳입니다."),
                    M("오래된 흔적이지만 누군가 머물렀던 자리였겠네요."), U("서쪽 공간을 기록하겠습니다.") };
                case 2: return new[] { T("무너진 돌 사이의 긁힌 자국은 다른 흔적보다 새롭습니다."),
                    P("비슷한 선이 반복되네요. 뜻은 아직 알 수 없습니다."), U("북쪽 장치도 확인해보죠.") };
                case 3: return new[] { T("양쪽 구조를 확인했습니다. 이제 이 장치를 움직일 수 있겠습니다."),
                    P("아래로 이어지는 길입니다. 단단한 가장자리를 따라 내려가죠.") };
                case 5: return new[] { M("쓰이지 않은 안치 공간이 많아요. 위층과는 다르네요."),
                    T("돌을 다듬은 방식도 더 정교합니다. 안쪽부터 확인하죠."),
                    P("바닥은 지나갈 수 있습니다. 거친 부분은 표시해두겠습니다.") };
                case 6: return new[] { P("닫힌 묘실인데 안치 흔적은 거의 없습니다. 다른 용도였을 수도 있겠네요."),
                    M("폴, 이쪽 바닥은 괜찮아요?"), P("네. 조금 거칠지만 지나갈 수 있습니다.") };
                case 7: return new[] { T("석재의 홈과 문양이 반복됩니다. 의미는 아직 모르겠습니다."),
                    P("용도를 정하기에는 단서가 모자랍니다. 본 것만 기록하죠.") };
                case 8: return new[] { P("푸른 흐름은 이 방에서 시작되는 것 같지 않습니다. 더 안쪽에서 들어오는 듯하군요."),
                    T("방향은 확인했습니다. 근원은 아직 모릅니다."), M("무리하지 말고 앞쪽을 살펴봐요.") };
                case 10: return new[] { P("빛이 이 구조물에서 만들어지는 건 아닌 것 같습니다. 아래에서 올라와 틈을 지나갑니다."),
                    T("그렇다면 지금 보이는 균열로 내려갈 수 있겠습니까?"),
                    M("아래쪽은 많이 무너져 있어요."),
                    T("지금 억지로 들어가는 건 위험합니다."),
                    P("동의합니다. 무너진 지하에서 길을 개척하는 취미는 없거든요.") };
                case 11: return new[] { P("석판은 대부분 부서졌습니다. 글자를 읽기는 어렵겠네요."),
                    M("밖에서 알아볼 수 있는 사람이 있을지도 몰라요."),
                    T("지하묘지에서 본 것과 같은 문양입니다. 가져가는 편이 좋겠습니다.") };
                default: return new[] { U("주변을 더 살펴보겠습니다.") };
            }
        }
    }
}

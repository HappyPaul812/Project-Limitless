using System.Linq;
using ProjectLimitless.Core;
using ProjectLimitless.NPC;
using ProjectLimitless.Player;
using ProjectLimitless.UI;
using ProjectLimitless.Battle;
using ProjectLimitless.Monster;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace ProjectLimitless.World
{
    /// <summary>Main17 완료 기록을 이어받아 준비·심부 조사·두 지정 전투를 설치합니다.
    /// 모든 영구 진행은 기존 Quest count와 Inventory에만 남기며 저장 파티를 바꾸지 않습니다.</summary>
    public sealed class Chapter2Main18Flow : MonoBehaviour
    {
        public const string QuestId = "main_18_black_heat";
        public const string Field = "Field_09_ObsidianScar";
        public const string PreviousField = "Field_08_RedRift";
        public const string CoolingItem = "item_cooling_remedy";
        public const string BeetleEncounter = "field09_main18_beetle_tutorial";
        public const string WatcherEncounter = "field09_main18_watcher";
        public static readonly string[] Targets = { "arbel_main18_return", "arbel-leon", "field08_main18_deep_route",
            "field09_main18_entry", "field09_main18_ground", "field09_main18_heat_vent", BeetleEncounter,
            "field09_main18_overheat", "field09_main18_vibration", "field09_main18_watcher_witness", WatcherEncounter,
            "field09_main18_after_watcher", "field09_main18_deeper_route" };
        public static readonly Vector2[] Positions = { new Vector2(-6,5), new Vector2(0,3), new Vector2(-7.6f,0),
            new Vector2(7.2f,0), new Vector2(5.2f,1), new Vector2(3,-2), new Vector2(1.2f,-.6f),
            new Vector2(0,-1.2f), new Vector2(-2.2f,2), new Vector2(-4.4f,.5f), new Vector2(-4.4f,.5f),
            new Vector2(-5.2f,-1.2f), new Vector2(-7.4f,0) };
        private static int blockedFrame = -1;
        internal static bool CanInteract => Time.frameCount > blockedFrame;
        internal static void BlockFrame() => blockedFrame = Time.frameCount;
        public static bool IsCurrent(int index) => QuestService.ActiveMainQuest?.Definition.QuestId == QuestId
            && QuestService.ActiveMainQuest.CurrentObjective?.TargetId == Targets[index];
        public static bool Prepared => QuestService.GetState(QuestId) == QuestState.Completed ||
            QuestService.ActiveMainQuest?.Definition.QuestId == QuestId && QuestService.ActiveMainQuest.ObjectiveCounts[1] > 0;
        public static bool CoolingShopUnlocked => QuestService.GetState(Chapter2Main17Flow.QuestId) == QuestState.Completed
            || QuestService.GetState(QuestId) == QuestState.Active || QuestService.GetState(QuestId) == QuestState.Completed;
        internal static void Save() { if (GameSaveService.CurrentSlotIndex > 0) GameSaveService.SaveCurrentSession(); }
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Register()
        {
            blockedFrame = -1;
            SceneManager.sceneLoaded -= Install;
            SceneManager.sceneLoaded += Install;
        }
        private static void Install(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != Field && scene.name != PreviousField && scene.name != "Arbel"
                && scene.name != Chapter2Main16Flow.Field && scene.name != Chapter2Main16Flow.PreviousField) return;
            if (scene.GetRootGameObjects().Any(x => x.GetComponent<Chapter2Main18Flow>() != null)) return;
            var root = new GameObject("Chapter2Main18Flow", typeof(Chapter2Main18Flow));
            SceneManager.MoveGameObjectToScene(root, scene);
        }
        private void Start()
        {
            QuestService.Changed += BeginIfAvailable;
            BeginIfAvailable();
            if (DialoguePresenter.Instance == null) new GameObject("DialogueSystem").AddComponent<DialoguePresenter>();
            string scene = gameObject.scene.name;
            if (scene == PreviousField)
            {
                var bounds = gameObject.scene.GetRootGameObjects().Select(x => x.GetComponent<WorldBounds2D>()).First(x => x != null);
                // Field08의 원본은 그대로 두고 실행 중 실제 서쪽 Exit만 엽니다. 준비 전에는 물리 문이 출입을 막습니다.
                foreach (Transform child in bounds.transform.Cast<Transform>().Where(x => x.name.StartsWith("Boundary_")).ToArray()) Destroy(child.gameObject);
                WorldBounds2D.CreateBoundaryColliders(transform, bounds.Bounds, .3f,
                    new WorldBoundaryOpening(WorldBoundarySide.Right, 0, 3), new WorldBoundaryOpening(WorldBoundarySide.Left, 0, 3));
                var gate = new GameObject("Main18AccessGate", typeof(BoxCollider2D), typeof(Main18AccessGate));
                gate.transform.SetParent(transform, false); gate.transform.position = new Vector2(-9.45f, 0);
                gate.GetComponent<BoxCollider2D>().size = new Vector2(.35f, 3);
                Site(2);
            }
            if (scene == "Arbel") Site(0);
            if (scene == Field)
            {
                for (int i = 3; i < Targets.Length; i++) if (i != 10) Site(i);
                var serin = new GameObject("Serin_Story_Main18", typeof(SpriteRenderer));
                serin.transform.SetParent(transform, false); serin.transform.position = new Vector2(4.4f, 1.2f);
                var controller = Resources.Load<RuntimeAnimatorController>("Chapter2/Serin");
                if (controller != null) serin.AddComponent<Animator>().runtimeAnimatorController = controller;
                Site(13); // 서쪽에는 전환 없이 세계관 안의 통행 불가 안내만 둡니다.
            }
            AttachRoutes();
        }
        private void OnDestroy() => QuestService.Changed -= BeginIfAvailable;
        public static void BeginIfAvailable()
        {
            if (QuestService.GetState(QuestId) == QuestState.Available && QuestService.TryStart(QuestId)) Save();
        }
        private void Site(int index)
        {
            var value = new GameObject(index == 13 ? "Main18ClosedDeepRoute" : Targets[index], typeof(Main18Site));
            value.transform.SetParent(transform, false); value.transform.position = index == 13 ? new Vector2(-8.8f,0) : Positions[index];
            value.GetComponent<Main18Site>().Configure(index);
            if (index < 13) QuestNavigationTarget.Attach(value, Targets[index], index == 0 ? "아르벨 귀환" : "흑요석 상흔 조사");
            if (index == 9) QuestNavigationTarget.Attach(value, WatcherEncounter, "작열 감시자 재도전");
        }
        private void AttachRoutes()
        {
            var exits = gameObject.scene.GetRootGameObjects().SelectMany(x => x.GetComponentsInChildren<SceneTransitionTrigger>(true));
            foreach (var exit in exits)
            {
                bool west = exit.name.Contains("west");
                if (!west && gameObject.scene.name != "Arbel")
                { QuestNavigationTarget.Attach(exit.gameObject, Targets[0], "아르벨로 귀환"); QuestNavigationTarget.Attach(exit.gameObject, Targets[1], "아르벨의 레온"); }
                if (west) foreach (string id in Targets.Skip(2)) QuestNavigationTarget.Attach(exit.gameObject, id, "흑요석 상흔으로");
            }
        }
        /// <summary>보고 완료 상태와 수량을 한 저장 경계에서 확정합니다. 꽉 찬 Stack이면 진행하지 않아 무료 물품을 잃지 않습니다.</summary>
        public static bool CompleteReport()
        {
            if (!IsCurrent(1) || !InventoryService.CanAddItem(CoolingItem, 3)) return false;
            if (!InventoryService.TryAddItem(CoolingItem, 3)) return false;
            QuestService.NotifyNpcTalked("arbel-leon"); Save(); return true;
        }
        public static bool TryHandleNpc(string id, NpcController npc)
        {
            if (id != "arbel-leon" || !IsCurrent(1)) return false;
            if (WorldModalState.IsOpen || DialoguePresenter.Instance == null) return true;
            DialoguePresenter.Instance.ShowSequence(Main18DialogueCatalog.Get(1), () =>
            {
                BlockFrame();
                if (!CompleteReport() && IsCurrent(1)) DialoguePresenter.Instance.Show(id, "레온", "냉각약을 받을 공간을 비운 뒤 다시 말씀해 주세요.");
            });
            return true;
        }
    }
    /// <summary>보고 완료 전 물리 문과 안내를 유지하고, 저장된 Quest 목표에서 출입 가능 여부를 복원합니다.</summary>
    public sealed class Main18AccessGate : MonoBehaviour
    {
        private void Update() => GetComponent<BoxCollider2D>().enabled = !Chapter2Main18Flow.Prepared;
    }
    /// <summary>조사 대화는 끝까지 읽었을 때만 진행합니다. 두 지정 전투는 승리 전까지 같은 지점에서 재도전할 수 있습니다.</summary>
    public sealed class Main18Site : MonoBehaviour
    {
        private int index;
        private bool busy;
        private InputAction input;
        private TextMesh marker;
        private GameObject storyVisual;
        public void Configure(int value) => index = value;
        private bool Current => index == 13 || Chapter2Main18Flow.IsCurrent(index) || index == 9 && Chapter2Main18Flow.IsCurrent(10)
            || index == 2 && !Chapter2Main18Flow.Prepared;
        private void Awake()
        {
            input = new InputAction("Main18Inspect", InputActionType.Button);
            input.AddBinding("<Keyboard>/e"); input.AddBinding("<Keyboard>/f"); input.AddBinding("<Gamepad>/buttonSouth");
            input.performed += _ => TryInteract();
        }
        private void OnEnable() => input?.Enable();
        private void OnDisable() => input?.Disable();
        private void OnDestroy() => input?.Dispose();
        private void Start()
        {
            if (index == 6 || index == 9)
            {
                // 지정 조우는 일반 Respawn과 분리하고, 저장된 목표가 아직 남은 동안 원본 Idle 프레임을 표시합니다.
                var monster = Resources.Load<MonsterDefinition>("MonsterDefinitions/" + (index == 6 ? "16_ObsidianBeetle" : "17_ScorchingWatcher"));
                if (monster != null)
                {
                    storyVisual = new GameObject("StoryMonsterVisual", typeof(SpriteRenderer), typeof(MonsterSpriteSheetAnimation));
                    storyVisual.transform.SetParent(transform, false);
                    storyVisual.transform.localScale = Vector3.one * (index == 6 ? .8f : 1.3f);
                    var renderer = storyVisual.GetComponent<SpriteRenderer>(); renderer.sortingOrder = 5;
                    storyVisual.GetComponent<MonsterSpriteSheetAnimation>().Configure(renderer, monster);
                }
            }
            marker = new GameObject("SiteLabel", typeof(TextMesh)).GetComponent<TextMesh>(); marker.transform.SetParent(transform, false);
            marker.transform.localPosition = Vector3.up; marker.text = index == 13 ? "더 깊은 길 [E/F / A]" : "조사 [E/F / A]";
            marker.characterSize = .12f; marker.anchor = TextAnchor.MiddleCenter;
            marker.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"); marker.fontSize = 32;
            marker.GetComponent<MeshRenderer>().sharedMaterial = marker.font.material;
        }
        private bool Near()
        {
            var player = FindAnyObjectByType<PlayerController>();
            return player != null && Vector2.Distance(player.transform.position, transform.position) < 1.7f;
        }
        private void Update()
        {
            if (storyVisual != null) storyVisual.SetActive(Current);
            if (busy && (DialoguePresenter.Instance == null || !DialoguePresenter.Instance.IsOpen)) busy = false;
            if (marker != null) marker.gameObject.SetActive(Current && !WorldModalState.IsOpen);
            if ((index == 0 || index == 3 || index == 2 && Chapter2Main18Flow.Prepared) && Chapter2Main18Flow.IsCurrent(index) && Near() && !WorldModalState.IsOpen)
            { QuestService.NotifyLocationReached(Chapter2Main18Flow.Targets[index]); Chapter2Main18Flow.Save(); }
        }
        public void TryInteract()
        {
            if (!Current || busy || WorldModalState.IsOpen || !Chapter2Main18Flow.CanInteract || !Near() || DialoguePresenter.Instance == null) return;
            if (index == 0 || index == 3) return;
            if (index == 2 || index == 13)
            {
                DialoguePresenter.Instance.ShowSequence(Main18DialogueCatalog.Get(index), null); return;
            }
            if (index == 6 || index == 9 && Chapter2Main18Flow.IsCurrent(10)) { EnterBattle(index == 6); return; }
            busy = true;
            DialoguePresenter.Instance.ShowSequence(Main18DialogueCatalog.Get(index), () =>
            {
                busy = false; Chapter2Main18Flow.BlockFrame();
                if (!Chapter2Main18Flow.IsCurrent(index)) return;
                if (index == 9) QuestService.NotifyLocationReached(Chapter2Main18Flow.Targets[index]);
                else QuestService.NotifyInteraction(Chapter2Main18Flow.Targets[index]);
                Chapter2Main18Flow.Save();
                if (index == 9) EnterBattle(false);
            });
        }
        private void EnterBattle(bool beetle)
        {
            if (!Chapter2Main18Flow.IsCurrent(beetle ? 6 : 10)) return;
            var spawn = Resources.Load<FieldMonsterSpawnDefinition>("StoryEncounterReturns/Main18_" + (beetle ? "BeetleReturn" : "WatcherReturn"));
            if (spawn == null || spawn.Monster == null) { Debug.LogError("Main18 지정 전투 복귀 데이터가 없습니다."); return; }
            BattleSceneFlow.EnterStoryBattle(beetle ? Chapter2Main18Flow.BeetleEncounter : Chapter2Main18Flow.WatcherEncounter,
                spawn.Monster, spawn, transform.position);
        }
    }
}

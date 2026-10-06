using System.Linq;
using ProjectLimitless.Core;
using ProjectLimitless.UI;
using ProjectLimitless.Player;
using ProjectLimitless.Battle;
using ProjectLimitless.Monster;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace ProjectLimitless.World
{
    /// <summary>Main16 이후 협곡 연결을 기존 Scene/Quest 서비스에 연결합니다. 기존 Scene 원본은 보존합니다.</summary>
    public sealed class Chapter2Main17Flow : MonoBehaviour
    {
        public const string QuestId = "main_17_red_rift";
        public const string Field = "Field_08_RedRift";
        public const string PreviousField = "Field_07_AshenReach";
        public const string EncounterId = "field08_main17_threat";
        public static readonly string[] Sites = { "entry", "crack", "vibration", "tracks", "serin", "compare", "resonance", "witness", "threat", "after", "route", "withdraw" };
        public static readonly Vector2[] Positions = { new Vector2(7,0), new Vector2(4,0), new Vector2(2,-2), new Vector2(1,3), new Vector2(0,1), new Vector2(-1,1), new Vector2(-3,-2), new Vector2(-4,0), new Vector2(-4,0), new Vector2(-4,-1), new Vector2(-7,0), new Vector2(-7,1) };
        public static bool IsCurrent(int index) => QuestService.ActiveMainQuest?.Definition.QuestId == QuestId && QuestService.ActiveMainQuest.CurrentObjective?.TargetId == "field08_main17_" + Sites[index];
        internal static void Save() { if (GameSaveService.CurrentSlotIndex > 0) GameSaveService.SaveCurrentSession(); }
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Register() { SceneManager.sceneLoaded -= Install; SceneManager.sceneLoaded += Install; }
        private static void Install(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != Field && scene.name != PreviousField) return;
            if (scene.GetRootGameObjects().Any(value => value.GetComponent<Chapter2Main17Flow>() != null)) return;
            var root = new GameObject("Chapter2Main17Flow", typeof(Chapter2Main17Flow));
            SceneManager.MoveGameObjectToScene(root, scene);
        }
        private void Awake()
        {
            if (gameObject.scene.name != PreviousField) return;
            var bounds = gameObject.scene.GetRootGameObjects().Select(value => value.GetComponent<WorldBounds2D>()).FirstOrDefault(value => value != null);
            if (bounds == null) { Debug.LogError("Main17: Field07 WorldBounds가 없습니다."); return; }
            // 같은 Bounds로 경계를 재구성하여 기존 동쪽과 새 서쪽 출구 외에는 통과할 수 없게 합니다.
            foreach (Transform child in bounds.transform.Cast<Transform>().Where(value => value.name.StartsWith("Boundary_")).ToArray()) Destroy(child.gameObject);
            WorldBounds2D.CreateBoundaryColliders(transform, bounds.Bounds, .3f,
                new WorldBoundaryOpening(WorldBoundarySide.Right, 0, 3), new WorldBoundaryOpening(WorldBoundarySide.Left, 0, 3));
            var gate = new GameObject("Main17AccessBarrier", typeof(BoxCollider2D), typeof(Main17AccessGate));
            gate.transform.SetParent(transform, false); gate.transform.position = new Vector2(-9.45f, 0);
            gate.GetComponent<BoxCollider2D>().size = new Vector2(.35f, 3);
        }
        private void Start()
        {
            if (gameObject.scene.name == PreviousField)
            {
                var exit = gameObject.scene.GetRootGameObjects().FirstOrDefault(value => value.name == "Transition_west_to_field08");
                if (exit != null) foreach (string id in Sites) QuestNavigationTarget.Attach(exit, "field08_main17_" + id, "붉은 균열 협곡으로");
                return;
            }
            BeginIfAvailable();
            if (DialoguePresenter.Instance == null) new GameObject("DialogueSystem").AddComponent<DialoguePresenter>();
            for (int i = 0; i < Sites.Length; i++)
            {
                if (i == 8) continue; // 목격 지점이 같은 좌표의 재도전도 담당합니다.
                var site = new GameObject("field08_main17_" + Sites[i], typeof(Main17Site));
                site.transform.SetParent(transform, false); site.transform.position = Positions[i]; site.GetComponent<Main17Site>().Configure(i);
                QuestNavigationTarget.Attach(site, site.name, i == 0 ? "협곡 진입" : "협곡 조사");
                if (i == 7) QuestNavigationTarget.Attach(site, EncounterId, "협곡 조우 재도전");
            }
            var serin = new GameObject("Serin_Story_Main17", typeof(SpriteRenderer));
            serin.transform.SetParent(transform, false); serin.transform.position = Positions[4] + Vector2.down * .5f;
            var controller = Resources.Load<RuntimeAnimatorController>("Chapter2/Serin");
            if (controller != null) serin.AddComponent<Animator>().runtimeAnimatorController = controller;
            // 현장 인물만 만들며 명단/선택/Formation/Beast 장착 API는 호출하지 않습니다.
        }
        private void Update()
        {
            if (gameObject.scene.name != PreviousField) return;
            var player = FindAnyObjectByType<PlayerController>();
            if (player != null && player.transform.position.x < -7.5f && !WorldModalState.IsOpen) BeginIfAvailable();
        }
        private static void BeginIfAvailable()
        {
            if (QuestService.GetState(QuestId) == QuestState.Available && QuestService.TryStart(QuestId)) Save();
        }
    }

    /// <summary>Main16 보고 완료까지 서쪽 출구를 막습니다. 저장된 완료 기록에서 매번 잠금을 복원합니다.</summary>
    public sealed class Main17AccessGate : MonoBehaviour
    {
        private void Update() => GetComponent<BoxCollider2D>().enabled = QuestService.GetState(Chapter2Main16Flow.QuestId) != QuestState.Completed;
    }

    /// <summary>대화 완료만 목표를 진행합니다. 취소/Scene 종료/무관한 장소 입력은 저장 count를 변경하지 않습니다.</summary>
    public sealed class Main17Site : MonoBehaviour
    {
        private int index;
        private bool busy;
        private InputAction action;
        private TextMesh marker;
        public void Configure(int value) => index = value;
        private bool Current => Chapter2Main17Flow.IsCurrent(index) || index == 7 && Chapter2Main17Flow.IsCurrent(8);
        private void Awake()
        {
            action = new InputAction("Main17Inspect", InputActionType.Button);
            action.AddBinding("<Keyboard>/e"); action.AddBinding("<Keyboard>/f"); action.AddBinding("<Gamepad>/buttonSouth");
            action.performed += _ => TryInteract();
        }
        private void OnEnable() => action?.Enable();
        private void OnDisable() => action?.Disable();
        private void OnDestroy() => action?.Dispose();
        private void Start()
        {
            marker = new GameObject("SiteLabel", typeof(TextMesh)).GetComponent<TextMesh>();
            marker.transform.SetParent(transform, false); marker.transform.localPosition = Vector3.up;
            marker.text = index == 0 ? "협곡 진입" : "조사 [E/F / A]"; marker.characterSize = .12f; marker.anchor = TextAnchor.MiddleCenter;
            marker.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"); marker.fontSize = 32; marker.GetComponent<MeshRenderer>().sharedMaterial = marker.font.material;
        }
        private bool Near()
        {
            var player = FindAnyObjectByType<PlayerController>();
            return player != null && Vector2.Distance(player.transform.position, transform.position) < 1.7f;
        }
        private void Update()
        {
            if (busy && (DialoguePresenter.Instance == null || !DialoguePresenter.Instance.IsOpen)) busy = false;
            if (marker != null) marker.gameObject.SetActive(Current && !WorldModalState.IsOpen);
            if (index == 0 && Current && Near() && !WorldModalState.IsOpen)
            { QuestService.NotifyLocationReached("field08_main17_entry"); Chapter2Main17Flow.Save(); }
        }
        public void TryInteract()
        {
            if (index == 0 || !Current || busy || WorldModalState.IsOpen || !Near() || DialoguePresenter.Instance == null) return;
            if (Chapter2Main17Flow.IsCurrent(8)) { EnterBattle(); return; }
            busy = true;
            DialoguePresenter.Instance.ShowSequence(Main17DialogueCatalog.Get(index), () =>
            {
                busy = false;
                if (!Chapter2Main17Flow.IsCurrent(index)) return;
                string id = "field08_main17_" + Chapter2Main17Flow.Sites[index];
                if (index == 7) QuestService.NotifyLocationReached(id); else QuestService.NotifyInteraction(id);
                Chapter2Main17Flow.Save();
                if (index == 7) EnterBattle();
            });
        }
        private void EnterBattle()
        {
            if (!Chapter2Main17Flow.IsCurrent(8)) return;
            var spawn = Resources.Load<FieldMonsterSpawnDefinition>("StoryEncounterReturns/Main17_ThreatReturn");
            if (spawn == null || spawn.Monster == null) { Debug.LogError("Main17 지정 전투 복귀 데이터가 없습니다."); return; }
            // 승리 알림은 기존 전투 Controller만 전달합니다. 도망/패배에서는 목표 count를 유지합니다.
            if (!BattleSceneFlow.EnterStoryBattle(Chapter2Main17Flow.EncounterId, spawn.Monster, spawn, transform.position))
                Debug.LogWarning("Main17 전투에 진입하지 못했습니다. 이 조사 지점에서 다시 시도할 수 있습니다.");
        }
    }
}

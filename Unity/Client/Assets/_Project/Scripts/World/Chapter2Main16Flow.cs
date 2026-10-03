using System;
using System.Linq;
using ProjectLimitless.Battle;
using ProjectLimitless.Core;
using ProjectLimitless.Monster;
using ProjectLimitless.NPC;
using ProjectLimitless.Player;
using ProjectLimitless.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace ProjectLimitless.World
{
    /// <summary>Main16 조사·현장 인물·서쪽 연결을 기존 World 위에 설치합니다. Save나 Party를 별도로 만들지 않습니다.</summary>
    public sealed class Chapter2Main16Flow : MonoBehaviour
    {
        public const string QuestId = "main_16_shape_in_the_ash";
        public const string Field = "Field_07_AshenReach";
        public const string PreviousField = "Field_06_ScorchedTrail";
        public const string StoryEncounterId = "field07_main16_ember_wraith";
        public const string WitnessId = "field07_main16_witness";
        public static bool IsHearingPlayer => GameSessionData.SelectedPlayerPathId == "path.hearing";

        public static bool IsCurrent(string targetId) => QuestService.ActiveMainQuest?.Definition.QuestId == QuestId &&
            QuestService.ActiveMainQuest.CurrentObjective?.TargetId == targetId;

        /// <summary>지정 승리와 협곡 발견은 기존 목표 count에서 복원합니다. Save에 같은 의미의 Flag를 중복 저장하지 않습니다.</summary>
        public static bool HasFinishedObjective(string objectiveId)
        {
            if (QuestService.GetState(QuestId) == QuestState.Completed) return true;
            QuestRuntimeState quest = QuestService.ActiveMainQuest;
            if (quest?.Definition.QuestId != QuestId) return false;
            for (int i = 0; i < quest.Definition.Objectives.Count; i++)
                if (quest.Definition.Objectives[i].ObjectiveId == objectiveId)
                    return quest.ObjectiveCounts[i] >= quest.Definition.Objectives[i].RequiredCount;
            return false;
        }
        public static bool EmberGeneralSpawnUnlocked => HasFinishedObjective("defeat_ember_wraith");

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Register()
        {
            SceneManager.sceneLoaded -= Install;
            SceneManager.sceneLoaded += Install;
        }
        private static void Install(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != "Arbel" && scene.name != Field && scene.name != PreviousField) return;
            if (scene.GetRootGameObjects().Any(root => root.name == "Chapter2Main16Flow")) return;
            var root = new GameObject("Chapter2Main16Flow", typeof(Chapter2Main16Flow));
            SceneManager.MoveGameObjectToScene(root, scene);
        }

        private void Awake()
        {
            // Spawn 복원은 Start에서 수행됩니다. 서쪽 경계는 그보다 먼저 준비하고 출입구는 기존 연결 데이터가 설치합니다.
            if (gameObject.scene.name == PreviousField) OpenWestExit();
        }
        private void Start()
        {
            if (DialoguePresenter.Instance == null) new GameObject("DialogueSystem").AddComponent<DialoguePresenter>();
            if (gameObject.scene.name == "Arbel")
            {
                Site("arbel_main16_return", "아르벨 귀환", new Vector2(-6, 5), true);
                Transform gate = FindSceneObject("Transition_northwest_to_field06");
                if (gate != null) AttachInvestigationRoutes(gate.gameObject);
            }
            else if (gameObject.scene.name == PreviousField)
            {
                Site("field06_main16_west_exit", "재바람 황야로 가는 서쪽 길", new Vector2(-8, 0), true);
                Transform west = FindSceneObject("Transition_west_to_field07");
                if (west != null) AttachInvestigationRoutes(west.gameObject);
                Transform east = FindSceneObject("Transition_east_to_arbel");
                if (east != null) AttachReturnRoutes(east.gameObject);
            }
            else
            {
                Site("field07_main16_entry", "재바람 황야", new Vector2(7, 0), true);
                Site("field07_main16_ash", "움직이는 재", new Vector2(4, 0), false, "ash");
                Site("field07_main16_vibration", "지면 진동", new Vector2(1, -2), false, "vibration");
                Site("field07_main16_tracks", "흩어진 동물 흔적", new Vector2(-1, 1), false, "tracks");
                Site(WitnessId, "재가 모이는 지면", new Vector2(-3, 0), true, "witness");
                Site("field07_main16_afterimage", "남은 균열과 열기", new Vector2(-3, -1), false, "afterimage");
                Site("field07_main16_canyon", "붉은 균열 협곡 입구", new Vector2(-8, 0), false, "canyon");
                Transform east = FindSceneObject("Transition_east_to_field06");
                if (east != null) AttachReturnRoutes(east.gameObject);
                CreateStorySerin();
                CreateAshMarks();
            }
        }

        private Transform FindSceneObject(string name) => gameObject.scene.GetRootGameObjects()
            .SelectMany(root => root.GetComponentsInChildren<Transform>(true)).FirstOrDefault(t => t.name == name);

        private void Site(string id, string label, Vector2 at, bool arrival, string dialogue = "")
        {
            var site = new GameObject(id, typeof(Main16Site));
            site.transform.SetParent(transform, false); site.transform.position = at;
            site.GetComponent<Main16Site>().Configure(id, label, arrival, dialogue);
            QuestNavigationTarget.Attach(site, id, label, new Vector3(0, 1.3f));
            if (id == WitnessId) QuestNavigationTarget.Attach(site, StoryEncounterId, "불씨망령 재도전", new Vector3(0, 1.3f));
        }
        private static void AttachInvestigationRoutes(GameObject exit)
        {
            foreach (string id in new[] { "field06_main16_west_exit", "field07_main16_entry", "field07_main16_ash",
                "field07_main16_vibration", "field07_main16_tracks", WitnessId, StoryEncounterId,
                "field07_main16_afterimage", "field07_main16_canyon" })
                QuestNavigationTarget.Attach(exit, id, "서쪽의 재바람 황야");
        }
        private static void AttachReturnRoutes(GameObject exit)
        {
            QuestNavigationTarget.Attach(exit, "arbel_main16_return", "아르벨로 귀환");
            QuestNavigationTarget.Attach(exit, "arbel-leon", "아르벨의 레온");
        }
        private void CreateStorySerin()
        {
            // 현장 Story NPC는 전투 Party와 별개입니다. 미편성 세린도 정보를 공유하지만 편성/Formation API는 호출하지 않습니다.
            var npc = new GameObject("Serin_Story_Main16", typeof(SpriteRenderer), typeof(NpcController));
            npc.transform.SetParent(transform, false); npc.transform.position = new Vector2(4, -2);
            npc.GetComponent<NpcController>().Configure("세린", "기록한 흔적을 함께 확인하죠.");
            var controller = Resources.Load<RuntimeAnimatorController>("Chapter2/Serin");
            if (controller != null) npc.AddComponent<Animator>().runtimeAnimatorController = controller;
        }
        private void CreateAshMarks()
        {
            Sprite ground = gameObject.scene.GetRootGameObjects().SelectMany(root => root.GetComponentsInChildren<SpriteRenderer>())
                .FirstOrDefault(renderer => renderer.name.StartsWith("AshSoil_"))?.sprite;
            if (ground == null) return;
            for (int i = 0; i < 7; i++)
            {
                var ash = new GameObject("WindlessAsh_" + i, typeof(SpriteRenderer), typeof(Main16AshPulse));
                ash.transform.SetParent(transform, false); ash.transform.position = new Vector2(3.5f + i * .17f, (i % 3 - 1) * .2f);
                ash.transform.localScale = new Vector3(.15f, .09f, 1);
                var renderer = ash.GetComponent<SpriteRenderer>(); renderer.sprite = ground; renderer.color = new Color(.80f, .77f, .72f); renderer.sortingOrder = 1;
            }
        }
        private void OpenWestExit()
        {
            Transform wall = FindSceneObject("BoundaryLeft");
            if (wall != null) Destroy(wall.gameObject);
            for (int i = 0; i < 2; i++)
            {
                var part = new GameObject("Main16BoundaryLeft" + i, typeof(BoxCollider2D));
                part.transform.SetParent(transform, false); part.transform.position = new Vector2(-10.5f, i == 0 ? -4.5f : 4.5f);
                part.GetComponent<BoxCollider2D>().size = new Vector2(.3f, 6);
            }
            var locked = new GameObject("Main16AccessBarrier", typeof(BoxCollider2D), typeof(Main16AccessGate));
            locked.transform.SetParent(transform, false); locked.transform.position = new Vector2(-9.45f, 0);
            locked.GetComponent<BoxCollider2D>().size = new Vector2(.35f, 3);
        }

        /// <summary>NPC 시작/보고 대화가 끝난 경우에만 목표를 전달합니다. 대화 중 닫거나 Scene을 떠나면 진행하지 않습니다.</summary>
        public static bool TryHandleNpc(string id, NpcController npc)
        {
            if (id != "arbel-leon") return false;
            bool first = QuestService.GetState(QuestId) == QuestState.Available;
            if (!first && !IsCurrent(id)) return false;
            if (DialoguePresenter.Instance == null || WorldModalState.IsOpen) return true;
            bool introduction = first || QuestService.ActiveMainQuest?.CurrentObjective?.ObjectiveId == "talk_to_leon_about_ash";
            DialoguePresenter.Instance.ShowSequence(Main16DialogueCatalog.Get(introduction ? "start" : "report", IsHearingPlayer), () =>
            {
                if (first && !QuestService.TryStart(QuestId)) return;
                if (!IsCurrent(id)) return;
                QuestService.NotifyNpcTalked(id);
                Save();
            });
            return true;
        }
        internal static void Save() { if (GameSaveService.CurrentSlotIndex > 0) GameSaveService.SaveCurrentSession(); }
    }

    /// <summary>의뢰를 듣기 전에는 실제 서쪽 Opening도 통과하지 못하게 합니다. 진입 Trigger와 물리 잠금 벽은 분리합니다.</summary>
    public sealed class Main16AccessGate : MonoBehaviour
    {
        private void Update()
        {
            QuestState state = QuestService.GetState(Chapter2Main16Flow.QuestId);
            GetComponent<BoxCollider2D>().enabled = state != QuestState.Active && state != QuestState.Completed;
        }
    }

    /// <summary>순차 조사·목격·재도전을 기존 입력과 수동 대화로 연결합니다. 진행은 완료 콜백에서만 한 번 일어납니다.</summary>
    public sealed class Main16Site : MonoBehaviour
    {
        private string targetId, label, dialogue;
        private bool arrival, busy, witnessAttempted;
        private InputAction action;
        private TextMesh marker;
        private GameObject apparition;
        public void Configure(string id, string display, bool autoArrival, string scene)
        { targetId = id; label = display; arrival = autoArrival; dialogue = scene; }
        private bool Current => Chapter2Main16Flow.IsCurrent(targetId) ||
            targetId == Chapter2Main16Flow.WitnessId && Chapter2Main16Flow.IsCurrent(Chapter2Main16Flow.StoryEncounterId);
        private void Awake()
        {
            action = new InputAction("Main16Inspect", InputActionType.Button);
            action.AddBinding("<Keyboard>/e"); action.AddBinding("<Keyboard>/f"); action.AddBinding("<Gamepad>/buttonSouth");
            action.performed += _ => TryInteract();
        }
        private void Start()
        {
            marker = new GameObject("SiteLabel", typeof(TextMesh)).GetComponent<TextMesh>();
            marker.transform.SetParent(transform, false); marker.transform.localPosition = new Vector3(0, 1, 0);
            marker.text = label + (arrival && dialogue != "witness" ? "" : " [E/F]");
            marker.characterSize = .12f; marker.anchor = TextAnchor.MiddleCenter;
            marker.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"); marker.fontSize = 32;
            marker.GetComponent<MeshRenderer>().sharedMaterial = marker.font.material;
        }
        private void OnEnable() => action?.Enable();
        private void OnDisable() => action?.Disable();
        private void OnDestroy() => action?.Dispose();
        private void Update()
        {
            if (busy && (DialoguePresenter.Instance == null || !DialoguePresenter.Instance.IsOpen))
            {
                busy = false;
                if (apparition != null) apparition.SetActive(false);
            }
            if (marker != null) marker.gameObject.SetActive(Current && !WorldModalState.IsOpen);
            // 목격은 처음에는 도착 연출입니다. 도망/패배 뒤에는 E/F로 재도전해 즉시 전투에 되돌아가는 루프를 막습니다.
            if (arrival && dialogue != "witness" && Current && !WorldModalState.IsOpen && NearPlayer()) CompleteArrival();
            else if (dialogue == "witness" && !witnessAttempted && Chapter2Main16Flow.IsCurrent(targetId) && !WorldModalState.IsOpen && NearPlayer()) TryInteract();
        }
        private bool NearPlayer()
        {
            PlayerController player = FindAnyObjectByType<PlayerController>();
            return player != null && Vector2.Distance(player.transform.position, transform.position) < 1.7f;
        }
        private void CompleteArrival()
        {
            QuestService.NotifyLocationReached(targetId);
            Chapter2Main16Flow.Save();
        }
        public void TryInteract()
        {
            if (!Current || busy || WorldModalState.IsOpen || !NearPlayer() || DialoguePresenter.Instance == null) return;
            if (dialogue == "witness") { Witness(); return; }
            if (arrival) return;
            busy = true;
            DialoguePresenter.Instance.ShowSequence(Main16DialogueCatalog.Get(dialogue, Chapter2Main16Flow.IsHearingPlayer), () =>
            {
                busy = false;
                if (!Current) return;
                // 협곡 발견은 정식 설계의 ReachLocation입니다. 경계 도착만으로 넘어가지 않고 현장 확인 대화를 마친 뒤 알립니다.
                if (targetId == "field07_main16_canyon") QuestService.NotifyLocationReached(targetId);
                else QuestService.NotifyInteraction(targetId);
                Chapter2Main16Flow.Save();
            });
        }
        private void Witness()
        {
            FieldMonsterSpawnDefinition spawn = Resources.Load<FieldMonsterSpawnDefinition>("StoryEncounterReturns/Main16_EmberWraithReturn");
            if (spawn == null || spawn.Monster == null)
            { Debug.LogError("Main16 불씨망령 Story 조우 데이터가 없습니다."); return; }
            busy = true;
            // 첫 도착 연출을 닫은 사용자에게 매 프레임 다시 열지 않습니다. 같은 Scene에서는 E/F로 다시 확인합니다.
            witnessAttempted = true;
            if (Chapter2Main16Flow.IsCurrent(Chapter2Main16Flow.StoryEncounterId))
            {
                DialoguePresenter.Instance.ShowSequence(Main16DialogueCatalog.Get("retry", false), () => EnterBattle(spawn));
                return;
            }
            DialoguePresenter.Instance.ShowSequence(Main16DialogueCatalog.Get("witness_ground", false), () =>
            {
                if (!Current) { busy = false; return; }
                // 접촉 가능한 FieldMonster를 만들지 않습니다. 재가 모이는 서술이 끝난 뒤 공식 외형만 활성화합니다.
                if (apparition == null)
                {
                    apparition = new GameObject("EmberWraithStoryApparition", typeof(SpriteRenderer));
                    apparition.transform.SetParent(transform, false);
                    var renderer = apparition.GetComponent<SpriteRenderer>();
                    renderer.sprite = spawn.Monster.ExplicitIdleFrames.FirstOrDefault() ?? spawn.Monster.FieldSprite;
                    renderer.sortingOrder = 4;
                    apparition.transform.localScale = Vector3.one * spawn.Monster.VisualScale;
                }
                apparition.SetActive(true);
                DialoguePresenter.Instance.ShowSequence(Main16DialogueCatalog.Get("witness_emerge", false), () =>
                {
                    if (!Chapter2Main16Flow.IsCurrent(targetId)) { busy = false; return; }
                    CompleteArrival(); // 목격과 지정 승리의 저장 경계를 분리합니다. Continue는 8번에서 재도전합니다.
                    EnterBattle(spawn);
                });
            });
        }
        private void EnterBattle(FieldMonsterSpawnDefinition spawn)
        {
            busy = false;
            if (apparition != null) apparition.SetActive(false);
            if (!Chapter2Main16Flow.IsCurrent(Chapter2Main16Flow.StoryEncounterId)) return;
            // 이 API는 저장 Party를 바꾸지 않습니다. 전투 참가자는 현재 사용자 편성을 그대로 읽습니다.
            if (!BattleSceneFlow.EnterStoryBattle(Chapter2Main16Flow.StoryEncounterId, spawn.Monster, spawn, transform.position))
                Debug.LogWarning("Main16 Story Battle에 진입하지 못했습니다. 조사 지점에서 다시 시도할 수 있습니다.");
        }
    }

    /// <summary>재는 같은 간격으로 조금씩 움직입니다. 조사 입력에 제한 시간을 주거나 카메라를 흔들지 않습니다.</summary>
    public sealed class Main16AshPulse : MonoBehaviour
    {
        private Vector3 origin;
        private void Start() => origin = transform.localPosition;
        private void Update() => transform.localPosition = origin + Vector3.right * (.10f * Mathf.Sin(Time.time * 1.7f));
    }
}

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
    /// <summary>Main12~14의 조사 지점과 거점 인물을 기존 Scene 위에 설치합니다. 진행 상태는 QuestService에서 복원합니다.</summary>
    public sealed class Chapter2IntroFlow : MonoBehaviour
    {
        public const string Main12 = "main_12_remaining_record";
        public const string Main13 = "main_13_drying_land";
        public const string Main14 = "main_14_ground_pulse";
        public const string SerinId = "companion_serin";
        public const string Main13Battle = "field04_main13_serin_encounter";
        public const string Main14Battle = "field05_main14_fleeing_beasts";
        // Quest 단계로만 임시 동행을 복원하므로 기존 Save의 영구 명단과 섞이지 않습니다.
        public static bool SerinTemporarilyPresent
        {
            get
            {
                if (CompanionRosterService.IsUnlocked(SerinId)) return false;
                QuestRuntimeState main = QuestService.ActiveMainQuest;
                return main?.Definition.QuestId == Main13 && main.CurrentObjectiveIndex >= 3 && main.CurrentObjectiveIndex < 5 ||
                    main?.Definition.QuestId == Main14 && main.CurrentObjectiveIndex >= 4 ||
                    QuestService.GetState(Main14) == QuestState.Completed;
            }
        }
        private string sceneId;
        private bool safeZoneActivated;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Register()
        {
            SceneManager.sceneLoaded -= Install;
            SceneManager.sceneLoaded += Install;
        }

        private static void Install(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != "Field_03" && scene.name != "World_StarterVillage" &&
                scene.name != "Field_04_WesternBorder" && scene.name != "Arbel" &&
                scene.name != "Field_05_WesternOutskirts") return;
            var root = new GameObject("Chapter2IntroFlow", typeof(Chapter2IntroFlow));
            SceneManager.MoveGameObjectToScene(root, scene);
        }

        private void Start()
        {
            sceneId = gameObject.scene.name;
            if (DialoguePresenter.Instance == null) new GameObject("DialogueSystem").AddComponent<DialoguePresenter>();
            if (sceneId == "Field_03")
            {
                Site("field03_main12_tablet", "부서진 석판", new Vector2(4, -3), Main12);
                Site("field03_main12_party_decision", "서쪽으로 갈 준비", new Vector2(-8, 0), Main12);
                WestGate();
                OpenSide("BoundaryLeft", -10.5f);
            }
            else if (sceneId == "World_StarterVillage")
                Site("village_main12_records", "오래된 문양 기록", new Vector2(-3, 2.5f), Main12);
            else if (sceneId == "Field_04_WesternBorder")
            {
                OpenSide("BoundaryLeft", -10.5f);
                OpenSide("BoundaryRight", 10.5f);
                Site("field04_west_entry", "서쪽 숲 경계", new Vector2(7, 0), Main12, true);
                Site("field04_main13_dry_soil", "마른 토양", new Vector2(5, -2), Main13);
                Site("field04_main13_shallow_stream", "줄어든 물길", new Vector2(2, 2), Main13);
                Site(Main13Battle, "도망친 야수", new Vector2(-3, 0), Main13);
                Npc("field04-serin", "세린", "서쪽의 지면을 확인하고 있어요.", VillageNpcRoleType.Resident, new Vector2(-1, 0), true);
            }
            else if (sceneId == "Arbel")
            {
                var bounds = new GameObject("WorldBounds", typeof(WorldBounds2D));
                bounds.transform.SetParent(transform, false);
                bounds.GetComponent<WorldBounds2D>().Configure(Vector2.zero, new Vector2(21, 15));
                WorldBounds2D.CreateBoundaryColliders(bounds.transform, bounds.GetComponent<WorldBounds2D>().Bounds,
                    .3f, new WorldBoundaryOpening(WorldBoundarySide.Left, 0, 3),
                    new WorldBoundaryOpening(WorldBoundarySide.Right, 0, 3),
                    new WorldBoundaryOpening(WorldBoundarySide.Top, -6, 3));
                var respawn = new GameObject("Spawn_Arbel_Center", typeof(SceneSpawnPoint));
                respawn.transform.SetParent(transform, false);
                respawn.transform.position = new Vector2(0, -1.5f);
                respawn.GetComponent<SceneSpawnPoint>().Configure("Spawn_Arbel_Center");
                Site("arbel_main13_arrival", "아르벨 입구", new Vector2(7, 0), Main13, true);
                Site("arbel_main14_old_well", "오래된 서쪽 우물", new Vector2(-3, 3), Main14);
                Npc("arbel-leon", "레온", "주민의 생활을 지키는 일이 먼저입니다.", VillageNpcRoleType.Resident, new Vector2(0, 3));
                Npc("arbel-shop", "잡화 상인", "길을 나서기 전에 보급하세요.", VillageNpcRoleType.GeneralShop, new Vector2(4, 2));
                Npc("arbel-healer", "치유사", "여기서 쉬어 가세요.", VillageNpcRoleType.Healer, new Vector2(2, -2));
                Npc("arbel-party-guide", "파티 안내인", "동료와 전열을 점검하세요.", VillageNpcRoleType.PartyManager, new Vector2(-3, -2));
                Npc("arbel-pet-adoption", "펫 분양 담당", "직접 상대해 본 종의 길들여진 개체를 맡깁니다.", VillageNpcRoleType.PetAdoption, new Vector2(5, -3));
                Npc("arbel-pet-management", "펫 관리 담당", "함께할 동료를 살펴보세요.", VillageNpcRoleType.PetManagement, new Vector2(7, -3));
                Npc("arbel-resident-west", "서쪽 주민", "예전보다 마르는 속도가 빨라졌어요.", VillageNpcRoleType.Resident, new Vector2(-5, 2));
                Npc("arbel-portal-keeper", "Portal 관리인", "연결 기능은 아직 준비 중입니다.", VillageNpcRoleType.Resident, new Vector2(-7, -3));
                Mark(transform, "PortalAnchor", new Vector2(-7, -4), new Vector2(2, .7f), new Color(.23f, .29f, .38f));
            }
            else
            {
                OpenSide("BoundaryRight", 10.5f);
                Site("field05_main14_irrigation", "마른 관개수로", new Vector2(5, 2), Main14);
                Site("field05_main14_watchpost", "서쪽 감시초소", new Vector2(-2, 2), Main14);
                Site(Main14Battle, "도망친 야수의 흔적", new Vector2(-4, 0), Main14);
                Site("field05_main14_strong_pulse", "강한 지면 진동", new Vector2(-6, -2), Main14, true);
                Npc("field05-serin", "세린", "이쪽의 진동을 따로 확인하고 있어요.", VillageNpcRoleType.Resident, new Vector2(1, 0), true);
            }
            TryStartQuest();
        }

        private void Update()
        {
            TryStartQuest();
            if (sceneId != "Arbel" || safeZoneActivated || GameSessionData.PendingSpawnPointId != string.Empty) return;
            if (FindAnyObjectByType<PlayerController>() == null) return;
            safeZoneActivated = true;
            // 첫 정상 진입과 이어하기 모두 같은 기존 안전지대 저장 경계를 사용합니다.
            GameSessionData.ActivateSafeZone("safezone_arbel", "Arbel", "Spawn_Arbel_Center");
            if (GameSaveService.CurrentSlotIndex > 0) GameSaveService.SaveCurrentSession();
        }

        private static void TryStartQuest()
        {
            foreach (string id in new[] { Main12, Main13, Main14 })
                if (QuestService.GetState(id) == QuestState.Available && QuestService.ActiveMainQuest == null &&
                    (id != Main12 || InventoryService.GetItemCount(MainQuest11DungeonFlow.TabletItemId) > 0))
                {
                    if (QuestService.TryStart(id) && GameSaveService.CurrentSlotIndex > 0)
                        GameSaveService.SaveCurrentSession();
                    break;
                }
        }

        private void Site(string id, string label, Vector2 position, string questId, bool arrival = false)
        {
            var site = new GameObject(id, typeof(Chapter2ObjectiveSite));
            site.transform.SetParent(transform, false);
            site.transform.position = position;
            site.GetComponent<Chapter2ObjectiveSite>().Configure(id, label, questId, arrival);
            QuestNavigationTarget.Attach(site, id, label, new Vector3(0, 1.3f, 0));
            if (id.Contains("soil") || id.Contains("well") || id.Contains("irrigation") || id.Contains("watchpost"))
                Mark(site.transform, "GroundClue", Vector2.zero,
                    id.Contains("irrigation") ? new Vector2(2.4f, .35f) : new Vector2(1.3f, .75f),
                    id.Contains("well") ? new Color(.39f, .43f, .42f) : new Color(.46f, .34f, .22f));
        }

        private static void Mark(Transform parent, string name, Vector2 localPosition, Vector2 size, Color color)
        {
            var mark = GameObject.CreatePrimitive(PrimitiveType.Quad);
            mark.name = name;
            Destroy(mark.GetComponent<Collider>());
            mark.transform.SetParent(parent, false);
            mark.transform.localPosition = localPosition;
            mark.transform.localScale = size;
            var renderer = mark.GetComponent<MeshRenderer>();
            renderer.sharedMaterial = new Material(Shader.Find("Sprites/Default")) { color = color };
            renderer.sortingOrder = 4;
        }

        private void Npc(string id, string name, string line, VillageNpcRoleType role, Vector2 position, bool serin = false)
        {
            var npc = new GameObject(id, typeof(SpriteRenderer), typeof(NpcController), typeof(VillageNpcRole));
            npc.transform.SetParent(transform, false);
            npc.transform.position = position;
            npc.GetComponent<NpcController>().Configure(name, line);
            var label = new GameObject("Label", typeof(TextMesh)).GetComponent<TextMesh>();
            label.transform.SetParent(npc.transform, false);
            label.text = name;
            label.characterSize = .075f;
            label.anchor = TextAnchor.MiddleCenter;
            label.transform.localPosition = new Vector3(0, 1.02f, 0);
            var roleData = npc.GetComponent<VillageNpcRole>();
            roleData.Configure(id, role);
            if (!serin) VillageNpcAppearanceCatalog.Apply(npc, id, name, role);
            else
            {
                // 공식 Animator를 Resources 참조로 연결합니다. Sprite는 Controller의 첫 Idle 프레임이 갱신합니다.
                var controller = Resources.Load<RuntimeAnimatorController>("Chapter2/Serin");
                if (controller != null) npc.AddComponent<Animator>().runtimeAnimatorController = controller;
            }
            NpcQuestMarkerPresenter.GetOrAdd(npc.GetComponent<NpcController>(), roleData);
        }

        private void WestGate()
        {
            var gate = new GameObject("Transition_WestToField04", typeof(BoxCollider2D), typeof(Chapter2WestGate));
            gate.transform.SetParent(transform, false);
            gate.transform.position = new Vector2(-10.7f, 0);
            var collider = gate.GetComponent<BoxCollider2D>();
            collider.isTrigger = true; collider.size = new Vector2(1, 3);
            var spawn = new GameObject("Spawn_From_Field04", typeof(SceneSpawnPoint));
            spawn.transform.SetParent(transform, false);
            spawn.transform.position = new Vector2(-8, 0);
            spawn.GetComponent<SceneSpawnPoint>().Configure("Spawn_From_Field04");
            QuestNavigationTarget.Attach(gate, "field04_west_entry", "서쪽 숲 경계");
            var lockWall = new GameObject("LockedWestPassage", typeof(BoxCollider2D), typeof(Chapter2WestLock));
            lockWall.transform.SetParent(transform, false);
            lockWall.transform.position = new Vector2(-10.5f, 0);
            lockWall.GetComponent<BoxCollider2D>().size = new Vector2(.3f, 3);
        }

        private void OpenSide(string oldName, float x)
        {
            Transform old = gameObject.scene.GetRootGameObjects().SelectMany(root => root.GetComponentsInChildren<Transform>(true))
                .FirstOrDefault(child => child.name == oldName);
            if (old != null) Destroy(old.gameObject);
            // 기존 Field 외벽 중 실제 출구의 중앙 3칸만 비우고 위아래 물리 경계를 다시 만듭니다.
            for (int i = 0; i < 2; i++)
            {
                var wall = new GameObject("Chapter2Boundary_" + oldName + i, typeof(BoxCollider2D));
                wall.transform.SetParent(transform, false);
                wall.transform.position = new Vector2(x, i == 0 ? -4.5f : 4.5f);
                wall.GetComponent<BoxCollider2D>().size = new Vector2(.3f, 6);
            }
        }

        public static bool TryHandleNpc(string id, NpcController npc)
        {
            string quest = id == "starter-village-general-shop" ? Main12 :
                id == "field04-serin" || id == "arbel-leon" && QuestService.GetState(Main13) == QuestState.Active ? Main13 : Main14;
            bool current = QuestService.ActiveMainQuest?.Definition.QuestId == quest &&
                QuestService.ActiveMainQuest.CurrentObjective?.TargetId == id;
            if (!current && id != "field04-serin" && id != "field05-serin" && id != "arbel-leon") return false;
            DialogueLine[] lines;
            if (id == "starter-village-general-shop") lines = new[]
            { new DialogueLine(id, "잡화 상인", "예전에 서쪽에서 온 상인이 비슷한 문양이 새겨진 돌을 가져온 적이 있어요. 뜻까지는 모릅니다.") };
            else if (id == "field04-serin" && QuestService.ActiveMainQuest?.Definition.QuestId == Main13 &&
                     QuestService.ActiveMainQuest.CurrentObjectiveIndex >= 4) lines = new[]
            { new DialogueLine(SerinId, "세린", "저도 아르벨로 갑니다. 그곳까지 함께 움직이죠.") };
            else if (id == "field04-serin") lines = new[]
            {
                new DialogueLine("companion_miel", "미엘", "괜찮으세요?"),
                new DialogueLine(SerinId, "세린", "잠시만요. 바람이 강하면 말소리가 흐려져서요."),
                new DialogueLine(SerinId, "세린", "이 장치가 땅의 진동을 제가 알아볼 수 있는 신호로 바꿔 줘요. 중요한지는 제가 판단하고요."),
                new DialogueLine(SerinId, "세린", "잠깐만요. 앞에 둘, 오른쪽 뒤에 하나 더 있어요. 발밑으로 울립니다.")
            };
            else if (id == "field05-serin") lines = new[]
            {
                new DialogueLine(SerinId, "세린", "마을 안에서는 약한데 서쪽으로 갈수록 진동이 커져요. 일정한 간격으로 반복되고요."),
                new DialogueLine("companion_taeon", "태온", "열기는 계속 이어집니다. 같은 원인인지는 더 확인해야겠습니다.")
            };
            else if (id == "arbel-leon" && quest == Main13) lines = new[]
            { new DialogueLine(id, "레온", "아르벨에 오신 걸 환영합니다. 세린은 잠시 자기 조사를 마치고 오겠다고 했습니다.") };
            else if (id == "arbel-leon" && current && QuestService.ActiveMainQuest.CurrentObjectiveIndex == 0) lines = new[]
            { new DialogueLine(id, "레온", "서쪽 땅이 마른 건 오래됐지만 최근 속도가 빠릅니다. 주민들이 쓸 우물부터 확인해 주시겠습니까?") };
            else if (id == "arbel-leon" && current) lines = new[]
            { new DialogueLine(id, "레온", "건조, 열기, 주기적인 진동, 도망친 야수까지 확인됐군요. 원인을 단정하지 않고 주민을 대비시키겠습니다.") };
            else lines = new[] { new DialogueLine(id, npc.DisplayName, npc.Dialogue) };
            DialoguePresenter.Instance?.ShowSequence(lines, () =>
            {
                if (!current) return;
                QuestService.NotifyNpcTalked(id);
                if (GameSaveService.CurrentSlotIndex > 0) GameSaveService.SaveCurrentSession();
            });
            return true;
        }
    }

    /// <summary>서쪽 경계의 통행은 Main12 마지막 목표부터 허용합니다.</summary>
    public sealed class Chapter2WestGate : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<PlayerController>() == null) return;
            QuestRuntimeState quest = QuestService.ActiveMainQuest;
            if (quest?.Definition.QuestId == Chapter2IntroFlow.Main12 && quest.CurrentObjectiveIndex == 4 ||
                QuestService.GetState(Chapter2IntroFlow.Main12) == QuestState.Completed)
                SceneTransitionService.Load("Field_04_WesternBorder", "Spawn_From_Field03");
        }
    }

    /// <summary>Main12가 서쪽 이동을 허용하기 전에는 열린 외벽 틈을 물리적으로 막습니다.</summary>
    public sealed class Chapter2WestLock : MonoBehaviour
    {
        private void Update()
        {
            QuestRuntimeState quest = QuestService.ActiveMainQuest;
            GetComponent<BoxCollider2D>().enabled = !(
                quest?.Definition.QuestId == Chapter2IntroFlow.Main12 && quest.CurrentObjectiveIndex == 4 ||
                QuestService.GetState(Chapter2IntroFlow.Main12) == QuestState.Completed);
        }
    }

    /// <summary>순차 목표일 때만 조사 입력을 받고, 대사 종료 또는 전투 승리 뒤에 진행합니다.</summary>
    public sealed class Chapter2ObjectiveSite : MonoBehaviour
    {
        private string targetId, label, questId;
        private bool arrival;
        private InputAction action;
        private TextMesh marker;
        private bool showingArrival;
        public void Configure(string id, string name, string quest, bool isArrival)
        { targetId = id; label = name; questId = quest; arrival = isArrival; }
        private bool Current => QuestService.ActiveMainQuest?.Definition.QuestId == questId &&
            QuestService.ActiveMainQuest.CurrentObjective?.TargetId == targetId;
        private void Awake()
        {
            action = new InputAction("Chapter2Inspect", InputActionType.Button);
            action.AddBinding("<Keyboard>/e"); action.AddBinding("<Keyboard>/f");
            action.AddBinding("<Gamepad>/buttonSouth");
            action.performed += _ => TryInteract();
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
            if (showingArrival && !WorldModalState.IsOpen) showingArrival = false;
            if (marker != null) marker.gameObject.SetActive(Current && !WorldModalState.IsOpen);
            if (!arrival || !Current) return;
            PlayerController player = FindAnyObjectByType<PlayerController>();
            if (player != null && Vector2.Distance(player.transform.position, transform.position) < 1.7f)
            {
                if (targetId == "field05_main14_strong_pulse")
                {
                    if (showingArrival || WorldModalState.IsOpen) return;
                    showingArrival = true;
                    DialoguePresenter.Instance?.ShowSequence(new[]
                    {
                        new DialogueLine(Chapter2IntroFlow.SerinId, "세린", "그 야수는 이쪽으로 오던 게 아니에요. 서쪽에서 도망치고 있었어요."),
                        new DialogueLine("companion_miel", "미엘", "그럼 저쪽에 무언가 있나요?"),
                        new DialogueLine(Chapter2IntroFlow.SerinId, "세린", "이번 건 큽니다."),
                        new DialogueLine("companion_taeon", "태온", "이번에는 저도 느꼈습니다. 아직 원인은 알 수 없습니다.")
                    }, () =>
                    {
                        showingArrival = false;
                        if (!Current) return;
                        QuestService.NotifyLocationReached(targetId);
                        if (GameSaveService.CurrentSlotIndex > 0) GameSaveService.SaveCurrentSession();
                    });
                    return;
                }
                QuestService.NotifyLocationReached(targetId);
                if (GameSaveService.CurrentSlotIndex > 0) GameSaveService.SaveCurrentSession();
            }
        }
        private void TryInteract()
        {
            if (arrival || !Current || WorldModalState.IsOpen) return;
            PlayerController player = FindAnyObjectByType<PlayerController>();
            if (player == null || Vector2.Distance(player.transform.position, transform.position) > 1.7f) return;
            if (targetId == Chapter2IntroFlow.Main13Battle || targetId == Chapter2IntroFlow.Main14Battle)
            {
                string asset = targetId == Chapter2IntroFlow.Main13Battle ? "StoryEncounterReturns/Main13_SerinReturn" :
                    "StoryEncounterReturns/Main14_BeastReturn";
                FieldMonsterSpawnDefinition returnSpawn = Resources.Load<FieldMonsterSpawnDefinition>(asset);
                if (returnSpawn != null) BattleSceneFlow.EnterStoryBattle(targetId, returnSpawn.Monster, returnSpawn, transform.position);
                return;
            }
            string speaker = targetId == "field03_main12_party_decision" ? "태온" : "폴";
            string text = Line(targetId);
            DialoguePresenter.Instance?.ShowSequence(new[] { new DialogueLine("", speaker, text) }, () =>
            {
                if (!Current) return;
                QuestService.NotifyInteraction(targetId);
                if (GameSaveService.CurrentSlotIndex > 0) GameSaveService.SaveCurrentSession();
            });
            DialoguePresenter.Instance?.TrackDistance(player.transform, transform, 3f);
        }
        private static string Line(string id)
        {
            switch (id)
            {
                case "field03_main12_tablet": return "지하묘지와 같은 반복 문양입니다. 아주 오래된 기록이지만 의미는 아직 알 수 없습니다.";
                case "village_main12_records": return "같은 문양은 맞습니다. 다만 같은 원인이라는 뜻인지는 아직 모르겠군요.";
                case "field03_main12_party_decision": return "직접 확인하는 편이 좋겠습니다. 미엘도 서쪽으로 가볼 이유는 충분하다고 합니다.";
                case "field04_main13_dry_soil": return "서쪽으로 갈수록 흙이 더 말라 있습니다. 계절 탓인지는 아직 판단할 수 없겠어요.";
                case "field04_main13_shallow_stream": return "물길이 눈에 띄게 줄었습니다. 주변의 살아 있는 나무와 비교해 기록해 두죠.";
                case "arbel_main14_old_well": return "서쪽 우물의 수위가 더 낮습니다. 지하 흐름 차이는 가능성일 뿐입니다.";
                case "field05_main14_irrigation": return "수로는 거의 말랐고 그늘의 바닥도 미세하게 따뜻합니다.";
                case "field05_main14_watchpost": return "타버린 풀과 갈라진 돌입니다. 열이 잠깐 발생했거나 무언가 지나갔을 수 있습니다.";
                default: return "이번에는 모두가 약하게 느꼈습니다. 진동이 더 강해졌지만 원인은 아직 모릅니다.";
            }
        }
    }
}

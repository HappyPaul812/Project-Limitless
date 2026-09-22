using System;
using System.Linq;
using ProjectLimitless.Battle;
using ProjectLimitless.Core;
using ProjectLimitless.Monster;
using ProjectLimitless.NPC;
using ProjectLimitless.Player;
using ProjectLimitless.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjectLimitless.World
{
    /// <summary>Field_01에 태온의 첫 등장, 지정 전투, 단서 결합과 마지막 조사 지점을 설치합니다.</summary>
    public static class MainQuest03FieldFlow
    {
        public const string QuestId = "main_03_unfamiliar_companion";
        public const string TaeonId = "companion_taeon";
        public const string MeetingLocationId = "field01_main03_taeon_meeting";
        public const string EncounterId = "field01_main03_taeon_encounter";
        public const string NextClueLocationId = "field01_main03_next_clue";
        public const string NextQuestId = "main_04_three_people";
        public static readonly Vector2 TaeonMeetingPosition = new Vector2(7.5f, -.8f);
        public static readonly Vector2 NextCluePosition = new Vector2(8.2f, 4.6f);

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            SceneManager.sceneLoaded -= Install;
            SceneManager.sceneLoaded += Install;
        }

        private static void Install(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != "Field_01" || GameObject.Find("MainQuest03FieldFlow") != null) return;
            // Field Scene을 직접 열거나 Single 전환한 경우에도 기존 공용 대화 정책을 그대로 사용할 수 있게 합니다.
            if (DialoguePresenter.Instance == null) new GameObject("DialogueSystem").AddComponent<DialoguePresenter>();
            GameObject root = new GameObject("MainQuest03FieldFlow");
            SceneManager.MoveGameObjectToScene(root, scene);

            CreateLocation(root.transform, "Main03MeetingArea", new Vector2(6.6f, -.8f), new Vector2(2.4f, 2.4f),
                MeetingLocationId, false);
            CreateLocation(root.transform, "Main03NextClue", NextCluePosition, new Vector2(2.2f, 2.2f),
                NextClueLocationId, true);

            GameObject taeon = new GameObject("Main03_Taeon", typeof(NpcController), typeof(VillageNpcRole),
                typeof(MainQuest03TaeonActor));
            taeon.transform.SetParent(root.transform, false);
            taeon.transform.position = TaeonMeetingPosition;
            taeon.GetComponent<NpcController>().Configure("태온", "몬스터의 움직임을 관찰하고 있습니다.");
            VillageNpcRole role = taeon.GetComponent<VillageNpcRole>();
            role.Configure(TaeonId, VillageNpcRoleType.Resident, false);
            QuestNavigationTarget.Attach(taeon, EncounterId, "태온 주변 몬스터", new Vector3(0f, 1.9f, 0f));
            SpriteRenderer renderer = new GameObject("OfficialTaeonVisual", typeof(SpriteRenderer)).GetComponent<SpriteRenderer>();
            renderer.transform.SetParent(taeon.transform, false);
            renderer.sprite = Resources.LoadAll<Sprite>("BattleCharacters/Taeon/Taeon_Battle_Final")
                .FirstOrDefault(sprite => sprite.name == "Taeon_Idle_00");
            renderer.sortingOrder = 8;
            NpcQuestMarkerPresenter.GetOrAdd(taeon.GetComponent<NpcController>(), role);
            // 모든 자식을 만든 뒤 Coordinator를 켜야 첫 Refresh에서 Actor와 Location을 빠뜨리지 않습니다.
            root.AddComponent<MainQuest03Coordinator>();
        }

        private static void CreateLocation(Transform parent, string name, Vector2 position, Vector2 size,
            string locationId, bool finalClue)
        {
            GameObject location = new GameObject(name, typeof(BoxCollider2D), typeof(MainQuest03Location));
            location.transform.SetParent(parent, false);
            location.transform.position = position;
            BoxCollider2D collider = location.GetComponent<BoxCollider2D>();
            collider.isTrigger = true;
            collider.size = size;
            location.GetComponent<MainQuest03Location>().Configure(locationId, finalClue);
            QuestNavigationTarget.Attach(location, locationId, finalClue ? "마지막 단서" : "태온이 있는 곳");
        }
    }

    /// <summary>Quest Available 상태를 시작하고 저장 복원 뒤 Actor와 목적지 표시를 현재 Objective에 맞춥니다.</summary>
    public sealed class MainQuest03Coordinator : MonoBehaviour
    {
        private void OnEnable()
        {
            QuestService.Changed += Refresh;
            EnsureStarted();
            Refresh();
        }

        private void OnDisable() => QuestService.Changed -= Refresh;

        private void EnsureStarted()
        {
            if (QuestService.GetState(MainQuest03FieldFlow.QuestId) != QuestState.Available) return;
            if (QuestService.TryStart(MainQuest03FieldFlow.QuestId) && GameSaveService.CurrentSlotIndex > 0)
                GameSaveService.SaveCurrentSession();
        }

        private void Refresh()
        {
            EnsureStarted();
            MainQuest03TaeonActor actor = GetComponentInChildren<MainQuest03TaeonActor>(true);
            actor?.RefreshForQuest();
            foreach (MainQuest03Location location in GetComponentsInChildren<MainQuest03Location>(true))
                location.RefreshForQuest();
        }
    }

    /// <summary>첫 만남 지점과 마지막 사람의 흔적 지점을 stable Location ID로 진행합니다.</summary>
    public sealed class MainQuest03Location : MonoBehaviour
    {
        private string locationId;
        private bool finalClue;
        private GameObject label;

        public void Configure(string stableLocationId, bool isFinalClue)
        {
            locationId = stableLocationId;
            finalClue = isFinalClue;
            if (finalClue) label = CreateLabel("◇ 누군가 지나간 흔적");
        }

        private void OnTriggerEnter2D(Collider2D other) => TryReach(other.GetComponent<PlayerController>());
        private void OnTriggerStay2D(Collider2D other) => TryReach(other.GetComponent<PlayerController>());

        private void TryReach(PlayerController player)
        {
            if (player == null || !IsCurrentObjective()) return;
            if (!finalClue)
            {
                QuestService.NotifyLocationReached(locationId);
                return;
            }
            if (WorldModalState.IsOpen) return;
            DialoguePresenter.Instance?.ShowSequence(MainQuest03FieldFlow.TaeonId, "태온", new[]
            {
                "바닥에 간단히 치료한 흔적과 떨어진 붕대가 남아 있습니다.",
                "누군가 먼저 이곳을 지나간 것 같습니다."
            }, () =>
            {
                QuestService.NotifyLocationReached(locationId);
            });
            DialoguePresenter.Instance?.TrackDistance(player.transform, transform, 3f);
        }

        public void RefreshForQuest()
        {
            bool visible = finalClue && IsCurrentObjective();
            if (label != null) label.SetActive(visible);
        }

        private bool IsCurrentObjective()
        {
            QuestRuntimeState quest = QuestService.ActiveMainQuest;
            return quest?.Definition.QuestId == MainQuest03FieldFlow.QuestId
                && quest.CurrentObjective?.TargetId == locationId;
        }

        private GameObject CreateLabel(string text)
        {
            GameObject canvasObject = new GameObject("StoryClueMarker", typeof(Canvas), typeof(CanvasScaler));
            canvasObject.transform.SetParent(transform, false);
            canvasObject.transform.localPosition = new Vector3(0f, 1.25f, 0f);
            canvasObject.transform.localScale = Vector3.one * .01f;
            Canvas canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.sortingOrder = 22;
            canvasObject.GetComponent<RectTransform>().sizeDelta = new Vector2(310f, 48f);
            Text labelText = new GameObject("Text", typeof(Text)).GetComponent<Text>();
            labelText.transform.SetParent(canvasObject.transform, false);
            labelText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            labelText.fontSize = 26;
            labelText.fontStyle = FontStyle.Bold;
            labelText.alignment = TextAnchor.MiddleCenter;
            labelText.color = new Color(.82f, .72f, .42f);
            labelText.text = text;
            RectTransform rect = labelText.rectTransform;
            rect.anchorMin = Vector2.zero; rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero; rect.offsetMax = Vector2.zero;
            return canvasObject;
        }
    }

    /// <summary>태온의 두 대화와 재도전 가능한 Story Encounter 진입을 현재 Objective에 맞춰 처리합니다.</summary>
    public sealed class MainQuest03TaeonActor : MonoBehaviour
    {
        public void RefreshForQuest()
        {
            QuestRuntimeState quest = QuestService.ActiveMainQuest;
            bool active = quest?.Definition.QuestId == MainQuest03FieldFlow.QuestId;
            gameObject.SetActive(active);
            if (!active) return;
            transform.position = quest.CurrentObjective?.TargetId == MainQuest03FieldFlow.NextClueLocationId
                ? (Vector3)(MainQuest03FieldFlow.NextCluePosition + new Vector2(-.55f, -.25f))
                : (Vector3)MainQuest03FieldFlow.TaeonMeetingPosition;
        }

        public bool TryInteract(Transform player, float breakDistance)
        {
            QuestRuntimeState quest = QuestService.ActiveMainQuest;
            if (quest?.Definition.QuestId != MainQuest03FieldFlow.QuestId) return false;
            string target = quest.CurrentObjective?.TargetId;
            if (target == MainQuest03FieldFlow.TaeonId)
            {
                bool firstConversation = quest.CurrentObjective.ObjectiveId == "talk_to_taeon_first";
                string[] pages = firstConversation ? FirstConversation() : AfterBattleConversation();
                DialoguePresenter.Instance?.ShowSequence(MainQuest03FieldFlow.TaeonId, "대화", pages, () =>
                {
                    QuestService.NotifyNpcTalked(MainQuest03FieldFlow.TaeonId);
                    if (GameSaveService.CurrentSlotIndex > 0) GameSaveService.SaveCurrentSession();
                });
                DialoguePresenter.Instance?.TrackDistance(player, transform, breakDistance);
                return true;
            }
            if (target == MainQuest03FieldFlow.EncounterId)
            {
                DialoguePresenter.Instance?.ShowSequence(MainQuest03FieldFlow.TaeonId, "태온", new[]
                {
                    "옵니다.",
                    "제가 앞을 막겠습니다.\n뒤를 부탁드리겠습니다."
                }, StartStoryEncounter);
                DialoguePresenter.Instance?.TrackDistance(player, transform, breakDistance);
                return true;
            }
            return true;
        }

        private void StartStoryEncounter()
        {
            MonsterDefinition bee = Resources.LoadAll<MonsterDefinition>("MonsterDefinitions")
                .FirstOrDefault(item => item.MonsterId == "venom_bee");
            FieldMonsterSpawnDefinition returnSpawn = Resources.LoadAll<FieldMonsterSpawnDefinition>("MonsterSpawns")
                .FirstOrDefault(item => item.SceneName == "Field_01" && item.SpawnId == "venom_bee_01");
            BattleSceneFlow.EnterStoryBattle(MainQuest03FieldFlow.EncounterId, bee, returnSpawn, transform.position);
        }

        private static string[] FirstConversation() => new[]
        {
            "태온\n잠깐만요. 더 가까이 가지 않는 게 좋겠습니다.",
            "플레이어\n무슨 일이 있습니까?",
            "태온\n저 몬스터들 말입니다.\n그냥 돌아다니는 것 같지만…\n계속 같은 쪽을 피하고 있어요.",
            "플레이어\n저도 조금 전에 이상한 흔적을 발견했습니다.\n마을 쪽으로 몰려온 흔적이었습니다.",
            "태온\n그렇군요.\n그러면 제가 보고 있던 움직임하고\n이어질지도 모르겠습니다."
        };

        // 플레이어의 흔적과 태온의 반복 행동 관찰을 함께 놓아야 다음 방향을 추론할 수 있습니다.
        // 태온 한 사람이 초능력처럼 정답을 알아내는 장면으로 만들지 않습니다.
        private static string[] AfterBattleConversation() => new[]
        {
            "태온\n역시 이상합니다.",
            "플레이어\n방금 몬스터들도 같은 방향을 피했습니까?",
            "태온\n네.\n싸우는 동안에도 몇 번이나\n그쪽으로 움직이지 않으려고 했어요.",
            "플레이어\n제가 본 흔적도\n그 반대쪽에서 시작됐습니다.",
            "태온\n그렇다면 우연은 아닌 것 같습니다.",
            "태온\n저도 저쪽을 확인하려던 참이었습니다.",
            "태온\n목적이 같다면 잠시 함께 가시죠.\n혼자 움직이는 것보다는 안전할 겁니다."
        };
    }
}

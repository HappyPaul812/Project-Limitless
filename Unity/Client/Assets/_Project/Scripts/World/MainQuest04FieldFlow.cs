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
    /// <summary>Field_01에 미엘의 첫 등장, 부상자 치료 장면과 최초 3인 Story Encounter를 설치합니다.</summary>
    public static class MainQuest04FieldFlow
    {
        public const string QuestId = "main_04_three_people";
        public const string MielId = "companion_miel";
        public const string TaeonId = "companion_taeon";
        public const string MeetingLocationId = "field01_main04_miel_meeting";
        public const string EncounterId = "field01_main04_three_people_encounter";
        public const string NextQuestId = "main_05_return_of_three";
        public static readonly Vector2 MeetingPosition = new Vector2(7.2f, 5.5f);

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            SceneManager.sceneLoaded -= Install;
            SceneManager.sceneLoaded += Install;
        }

        private static void Install(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != "Field_01" || GameObject.Find("MainQuest04FieldFlow") != null) return;
            if (DialoguePresenter.Instance == null) new GameObject("DialogueSystem").AddComponent<DialoguePresenter>();
            GameObject root = new GameObject("MainQuest04FieldFlow");
            SceneManager.MoveGameObjectToScene(root, scene);

            GameObject meeting = new GameObject("Main04MielMeetingArea", typeof(BoxCollider2D), typeof(MainQuest04MeetingLocation));
            meeting.transform.SetParent(root.transform, false);
            meeting.transform.position = MeetingPosition;
            BoxCollider2D area = meeting.GetComponent<BoxCollider2D>();
            area.isTrigger = true;
            area.size = new Vector2(2.6f, 2.4f);

            CreateStoryActor(root.transform, "Main04_Taeon", "태온", TaeonId,
                "BattleCharacters/Taeon/Taeon_Battle_Final", "Taeon_Idle_00", MeetingPosition + new Vector2(-1.25f, .1f), false);
            GameObject miel = CreateStoryActor(root.transform, "Main04_Miel", "미엘", MielId,
                "BattleCharacters/Miel/Miel_Battle_Final", "Miel_Idle_00", MeetingPosition + new Vector2(.35f, .1f), true);
            miel.AddComponent<MainQuest04MielActor>();
            CreateWoundedTraveler(root.transform);
            root.AddComponent<MainQuest04Coordinator>();
        }

        private static GameObject CreateStoryActor(Transform parent, string objectName, string displayName, string stableId,
            string spritePath, string spriteName, Vector2 position, bool addMarker)
        {
            GameObject actor = new GameObject(objectName, typeof(NpcController), typeof(VillageNpcRole));
            actor.transform.SetParent(parent, false);
            actor.transform.position = position;
            actor.GetComponent<NpcController>().Configure(displayName, "상황을 살피고 있습니다.");
            VillageNpcRole role = actor.GetComponent<VillageNpcRole>();
            role.Configure(stableId, VillageNpcRoleType.Resident, false);
            SpriteRenderer renderer = new GameObject("Official" + displayName + "Visual", typeof(SpriteRenderer)).GetComponent<SpriteRenderer>();
            renderer.transform.SetParent(actor.transform, false);
            renderer.sprite = Resources.LoadAll<Sprite>(spritePath).FirstOrDefault(sprite => sprite.name == spriteName);
            renderer.sortingOrder = 8;
            if (addMarker) NpcQuestMarkerPresenter.GetOrAdd(actor.GetComponent<NpcController>(), role);
            return actor;
        }

        private static void CreateWoundedTraveler(Transform parent)
        {
            // 미엘은 구조를 기다리는 대상이 아니라, 도착 전부터 다른 사람을 치료하며 자기 역할을 수행합니다.
            GameObject traveler = new GameObject("Main04_WoundedTraveler");
            traveler.transform.SetParent(parent, false);
            traveler.transform.position = MeetingPosition + new Vector2(.95f, -.35f);
            SpriteRenderer renderer = traveler.AddComponent<SpriteRenderer>();
            renderer.sprite = Resources.Load<Sprite>("VillageNpcSprites/Eldiran/OGA07_Resident01");
            renderer.sortingOrder = 7;
            renderer.color = new Color(.82f, .82f, .82f, 1f);
            CreateLabel(traveler.transform, "부상당한 여행자");
        }

        private static void CreateLabel(Transform parent, string text)
        {
            GameObject canvasObject = new GameObject("StoryLabel", typeof(Canvas), typeof(CanvasScaler));
            canvasObject.transform.SetParent(parent, false);
            canvasObject.transform.localPosition = new Vector3(0f, 1.15f, 0f);
            canvasObject.transform.localScale = Vector3.one * .01f;
            Canvas canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.sortingOrder = 22;
            canvasObject.GetComponent<RectTransform>().sizeDelta = new Vector2(280f, 42f);
            Text label = new GameObject("Text", typeof(Text)).GetComponent<Text>();
            label.transform.SetParent(canvasObject.transform, false);
            label.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            label.fontSize = 24;
            label.alignment = TextAnchor.MiddleCenter;
            label.color = Color.white;
            label.text = text;
            label.rectTransform.anchorMin = Vector2.zero;
            label.rectTransform.anchorMax = Vector2.one;
            label.rectTransform.offsetMin = Vector2.zero;
            label.rectTransform.offsetMax = Vector2.zero;
        }
    }

    public sealed class MainQuest04Coordinator : MonoBehaviour
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
            if (QuestService.GetState(MainQuest04FieldFlow.QuestId) != QuestState.Available) return;
            if (QuestService.TryStart(MainQuest04FieldFlow.QuestId) && GameSaveService.CurrentSlotIndex > 0)
                GameSaveService.SaveCurrentSession();
        }

        private void Refresh()
        {
            EnsureStarted();
            bool active = QuestService.ActiveMainQuest?.Definition.QuestId == MainQuest04FieldFlow.QuestId;
            foreach (Transform child in transform)
                if (child.name.StartsWith("Main04_", StringComparison.Ordinal)) child.gameObject.SetActive(active);
            MainQuest04MeetingLocation location = GetComponentInChildren<MainQuest04MeetingLocation>(true);
            location?.RefreshForQuest();
        }
    }

    public sealed class MainQuest04MeetingLocation : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other) => TryReach(other.GetComponent<PlayerController>());
        private void OnTriggerStay2D(Collider2D other) => TryReach(other.GetComponent<PlayerController>());

        private void TryReach(PlayerController player)
        {
            if (player == null || !IsCurrentObjective()) return;
            QuestService.NotifyLocationReached(MainQuest04FieldFlow.MeetingLocationId);
        }

        public void RefreshForQuest() => gameObject.SetActive(QuestService.ActiveMainQuest?.Definition.QuestId == MainQuest04FieldFlow.QuestId);

        private static bool IsCurrentObjective()
        {
            QuestRuntimeState quest = QuestService.ActiveMainQuest;
            return quest?.Definition.QuestId == MainQuest04FieldFlow.QuestId
                && quest.CurrentObjective?.TargetId == MainQuest04FieldFlow.MeetingLocationId;
        }
    }

    /// <summary>미엘의 두 대화와 재도전 가능한 Main 04 Story Encounter 진입을 처리합니다.</summary>
    public sealed class MainQuest04MielActor : MonoBehaviour
    {
        public bool TryInteract(Transform player, float breakDistance)
        {
            QuestRuntimeState quest = QuestService.ActiveMainQuest;
            if (quest?.Definition.QuestId != MainQuest04FieldFlow.QuestId) return false;
            string target = quest.CurrentObjective?.TargetId;
            if (target == MainQuest04FieldFlow.MielId)
            {
                DialogueLine[] lines = quest.CurrentObjective.ObjectiveId == "talk_to_miel_first"
                    ? FirstConversation() : AfterBattleConversation();
                DialoguePresenter.Instance?.ShowSequence(lines, () =>
                {
                    QuestService.NotifyNpcTalked(MainQuest04FieldFlow.MielId);
                    if (GameSaveService.CurrentSlotIndex > 0) GameSaveService.SaveCurrentSession();
                });
                DialoguePresenter.Instance?.TrackDistance(player, transform, breakDistance);
                return true;
            }
            if (target == MainQuest04FieldFlow.EncounterId)
            {
                DialoguePresenter.Instance?.ShowSequence(EncounterConversation(), StartStoryEncounter);
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
            BattleSceneFlow.EnterStoryBattle(MainQuest04FieldFlow.EncounterId, bee, returnSpawn, transform.position);
        }

        private static DialogueLine Line(string id, string name, string text) => new DialogueLine(id, name, text);
        private static string PlayerName => string.IsNullOrWhiteSpace(GameSessionData.PlayerName) ? "플레이어" : GameSessionData.PlayerName;

        private static DialogueLine[] FirstConversation() => new[]
        {
            Line(MainQuest04FieldFlow.MielId, "미엘", "조금만 참으세요.\n출혈은 멎었습니다."),
            Line(MainQuest04FieldFlow.TaeonId, "태온", "괜찮으십니까?"),
            Line(MainQuest04FieldFlow.MielId, "미엘", "저보다 이분이 먼저예요."),
            Line(MainQuest04FieldFlow.TaeonId, "태온", "당신도 다친 것 같은데요."),
            Line(MainQuest04FieldFlow.MielId, "미엘", "알아요.\n그래도 아직 움직일 수 있어요."),
            Line(string.Empty, PlayerName, "여기서 무슨 일이 있었습니까?"),
            Line(MainQuest04FieldFlow.MielId, "미엘", "몬스터들에게 습격받았습니다.\n그런데 조금 이상했어요."),
            Line(MainQuest04FieldFlow.MielId, "미엘", "처음부터 사람을 노리고\n온 것 같지는 않았습니다."),
            Line(MainQuest04FieldFlow.TaeonId, "태온", "무언가를 피하고 있었습니까?"),
            Line(MainQuest04FieldFlow.MielId, "미엘", "네.\n갑자기 길을 가로막게 되자\n공격한 것처럼 보였어요.")
        };

        private static DialogueLine[] EncounterConversation() => new[]
        {
            Line(MainQuest04FieldFlow.TaeonId, "태온", "또 옵니다.\n제가 앞을 맡겠습니다."),
            Line(MainQuest04FieldFlow.MielId, "미엘", "다친 곳은 제가 보겠습니다."),
            Line(string.Empty, PlayerName, "갑시다.")
        };

        private static DialogueLine[] AfterBattleConversation() => new[]
        {
            // 미엘은 다른 사람을 돌보면서도 자기 상태를 외면하지 않습니다. 상처를 자기희생 성격으로 고정하지 않습니다.
            Line(MainQuest04FieldFlow.MielId, "미엘", "괜찮으세요?\n다친 곳부터 확인할게요."),
            Line(MainQuest04FieldFlow.TaeonId, "태온", "본인부터 보셔야 하는 것 아닙니까?"),
            Line(MainQuest04FieldFlow.MielId, "미엘", "저도 볼 겁니다.\n이번에는 순서대로요."),
            // 세 사람의 현장 정보가 서로 보완될 뿐, 어느 한 사람도 사건의 정답을 독점하지 않습니다.
            Line(MainQuest04FieldFlow.TaeonId, "태온", "제가 본 움직임과\n이곳에서 있었던 일까지 합치면…"),
            Line(string.Empty, PlayerName, "몬스터들이 마을을 노리고\n내려오는 건 아닌 것 같습니다."),
            Line(MainQuest04FieldFlow.MielId, "미엘", "초원보다 더 안쪽에서\n무언가가 벌어지고 있는 것 같아요."),
            Line(MainQuest04FieldFlow.TaeonId, "태온", "여기서 더 들어가는 건\n지금은 위험할 것 같습니다."),
            Line(MainQuest04FieldFlow.MielId, "미엘", "마을에도 이 상황을 알려야 해요."),
            Line(string.Empty, PlayerName, "돌아가죠.")
        };
    }
}

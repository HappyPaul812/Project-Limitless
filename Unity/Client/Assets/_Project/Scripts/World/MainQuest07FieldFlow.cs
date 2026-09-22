using System.Linq;
using ProjectLimitless.Battle;
using ProjectLimitless.Core;
using ProjectLimitless.Monster;
using ProjectLimitless.Player;
using ProjectLimitless.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjectLimitless.World
{
    /// <summary>Field_02의 바퀴 자국부터 Paul 첫 만남·Story Battle·작별까지 Main 07을 설치합니다.</summary>
    public static class MainQuest07FieldFlow
    {
        public const string QuestId = "main_07_deep_tracks";
        public const string WheelTracksId = "field02_main07_wheel_tracks";
        public const string WoundedTravelerId = "field02_main07_wounded_traveler";
        public const string PaulTrailId = "field02_main07_paul_trail";
        public const string PaulId = "companion_paul";
        public const string EncounterId = "field02_main07_paul_encounter";
        public const string ReturnToMielId = "field02_main07_return_to_miel";
        public const string MielMeetingId = "field02_main07_miel_meeting";
        public const string PaulFarewellId = "field02_main07_paul_farewell";
        public static readonly Vector2 TracksPosition = new Vector2(2.7f, -1.2f);
        public static readonly Vector2 WoundedPosition = new Vector2(4.4f, .2f);
        public static readonly Vector2 PaulPosition = new Vector2(6.5f, -2.35f);

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize() { SceneManager.sceneLoaded -= Install; SceneManager.sceneLoaded += Install; }

        private static void Install(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != "Field_02" || GameObject.Find("MainQuest07FieldFlow") != null) return;
            if (QuestService.GetState(QuestId) == QuestState.Available) QuestService.TryStart(QuestId);
            if (DialoguePresenter.Instance == null) new GameObject("DialogueSystem").AddComponent<DialoguePresenter>();
            GameObject root = new GameObject("MainQuest07FieldFlow"); SceneManager.MoveGameObjectToScene(root, scene);
            CreateSite(root.transform, "Main07WheelTracks", WheelTracksId, TracksPosition, "◇ 평행한 두 줄의 깊은 흔적");
            CreateWheelTracksVisual(root.transform);
            CreateSite(root.transform, "Main07WoundedTraveler", WoundedTravelerId, WoundedPosition, "◆ 부상당한 여행자");
            CreateLocation(root.transform, "Main07PaulTrail", PaulTrailId, PaulPosition);
            CreateSite(root.transform, "Main07Paul", PaulId, PaulPosition, "◆ 폴");
            Transform paulSite = root.transform.Find("Main07Paul");
            if (paulSite != null) QuestNavigationTarget.Attach(paulSite.gameObject, EncounterId, "폴 주변 몬스터", new Vector3(0f, 1.5f, 0f));
            CreateLocation(root.transform, "Main07ReturnToMiel", ReturnToMielId, WoundedPosition);
            CreateSite(root.transform, "Main07Miel", MielMeetingId, WoundedPosition + new Vector2(-.8f, 0f), "◆ 미엘");
            CreateSite(root.transform, "Main07PaulFarewell", PaulFarewellId, WoundedPosition + new Vector2(.8f, 0f), "◆ 폴");
            CreateStoryVisual(root.transform, "WoundedTravelerVisual", "VillageNpcSprites/Eldiran/OGA07_Resident01", null, WoundedPosition);
            CreateStoryVisual(root.transform, "PaulOfficialFieldVisual", "BattleCharacters/Paul/Paul_Battle_Final", "Paul_Idle_00", PaulPosition);
            root.AddComponent<MainQuest07Coordinator>();
        }

        private static void CreateWheelTracksVisual(Transform parent)
        {
            GameObject root = new GameObject("Main07WheelTracksVisual"); root.transform.SetParent(parent, false); root.transform.position = TracksPosition;
            foreach (float offset in new[] { -.32f, .32f })
            {
                GameObject line = GameObject.CreatePrimitive(PrimitiveType.Quad); line.transform.SetParent(root.transform, false);
                line.transform.localPosition = new Vector3(offset, 0f, 0f); line.transform.localRotation = Quaternion.Euler(0f, 0f, 18f); line.transform.localScale = new Vector3(.16f, 1.65f, 1f);
                Object.Destroy(line.GetComponent<MeshCollider>()); Renderer renderer = line.GetComponent<Renderer>(); renderer.material = new Material(Shader.Find("Sprites/Default")); renderer.material.color = new Color(.13f,.09f,.05f,.62f); renderer.sortingOrder = 4;
            }
        }

        private static void CreateSite(Transform parent, string name, string targetId, Vector2 position, string label)
        {
            GameObject site = new GameObject(name, typeof(CircleCollider2D), typeof(MainQuest07Interactable));
            site.transform.SetParent(parent, false); site.transform.position = position;
            site.GetComponent<CircleCollider2D>().isTrigger = true; site.GetComponent<CircleCollider2D>().radius = 1.25f;
            site.GetComponent<MainQuest07Interactable>().Configure(targetId, label);
            QuestNavigationTarget.Attach(site, targetId, label.TrimStart('◆', '◇', ' '), new Vector3(0f, 1.45f, 0f));
        }

        private static void CreateLocation(Transform parent, string name, string targetId, Vector2 position)
        {
            GameObject site = new GameObject(name, typeof(CircleCollider2D), typeof(MainQuest07Location));
            site.transform.SetParent(parent, false); site.transform.position = position;
            site.GetComponent<CircleCollider2D>().isTrigger = true; site.GetComponent<CircleCollider2D>().radius = 1.4f;
            site.GetComponent<MainQuest07Location>().Configure(targetId);
            QuestNavigationTarget.Attach(site, targetId, targetId == PaulTrailId ? "바퀴 자국" : "미엘에게 돌아가기");
        }

        private static void CreateStoryVisual(Transform parent, string name, string path, string spriteName, Vector2 position)
        {
            GameObject obj = new GameObject(name, typeof(SpriteRenderer)); obj.transform.SetParent(parent, false); obj.transform.position = position;
            SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
            renderer.sprite = string.IsNullOrEmpty(spriteName) ? Resources.Load<Sprite>(path)
                : Resources.LoadAll<Sprite>(path).FirstOrDefault(x => x.name == spriteName);
            renderer.sortingOrder = 8; renderer.transform.localScale = string.IsNullOrEmpty(spriteName) ? Vector3.one : Vector3.one * .72f;
        }
    }

    public sealed class MainQuest07Coordinator : MonoBehaviour
    {
        private void OnEnable() { QuestService.Changed += Refresh; Refresh(); }
        private void OnDisable() => QuestService.Changed -= Refresh;
        private void Refresh()
        {
            bool active = QuestService.ActiveMainQuest?.Definition.QuestId == MainQuest07FieldFlow.QuestId;
            foreach (Transform child in transform) child.gameObject.SetActive(active);
            if (!active) return;
            int index = QuestService.ActiveMainQuest.CurrentObjectiveIndex;
            Transform wounded = transform.Find("WoundedTravelerVisual"); if (wounded != null) wounded.gameObject.SetActive(index >= 1 && index < 8);
            Transform paul = transform.Find("PaulOfficialFieldVisual");
            if (paul != null)
            {
                paul.gameObject.SetActive(index >= 2 && index < 8);
                paul.position = index >= 5 ? MainQuest07FieldFlow.WoundedPosition + new Vector2(.8f, 0f) : MainQuest07FieldFlow.PaulPosition;
            }
        }
    }

    public sealed class MainQuest07Location : MonoBehaviour
    {
        private string targetId;
        public void Configure(string value) => targetId = value;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<PlayerController>() == null) return;
            QuestRuntimeState quest = QuestService.ActiveMainQuest;
            if (quest?.Definition.QuestId == MainQuest07FieldFlow.QuestId && quest.CurrentObjective?.TargetId == targetId)
                QuestService.NotifyLocationReached(targetId);
        }
    }

    /// <summary>현재 목표에 해당하는 지점만 Marker와 입력을 제공하고, 대화 완료 뒤 stable ID를 알립니다.</summary>
    public sealed class MainQuest07Interactable : MonoBehaviour
    {
        private string targetId;
        private string labelText;
        private PlayerController nearby;
        private InputAction action;
        private GameObject marker;
        public void Configure(string id, string label) { targetId = id; labelText = label; }

        private void Start()
        {
            marker = CreateLabel(labelText + "\n[E/F] 확인");
            action = new InputAction("Main07Interact", InputActionType.Button);
            action.AddBinding("<Keyboard>/e"); action.AddBinding("<Keyboard>/f"); action.AddBinding("<Gamepad>/buttonSouth");
            action.performed += _ => Interact(); action.Enable(); QuestService.Changed += Refresh; Refresh();
        }
        private void OnDestroy() { QuestService.Changed -= Refresh; action?.Dispose(); }
        private void OnTriggerEnter2D(Collider2D other) { nearby = other.GetComponent<PlayerController>() ?? nearby; Refresh(); }
        private void OnTriggerExit2D(Collider2D other) { if (other.GetComponent<PlayerController>() == nearby) nearby = null; Refresh(); }
        private bool Current() => QuestService.ActiveMainQuest?.Definition.QuestId == MainQuest07FieldFlow.QuestId
            && QuestService.ActiveMainQuest.CurrentObjective?.TargetId == targetId;
        private void Refresh() { if (marker != null) marker.SetActive(Current()); }

        private void Interact()
        {
            if (nearby == null || !Current() || WorldModalState.IsOpen) return;
            DialoguePresenter.Instance.ShowSequence(GetLines(), Complete);
            DialoguePresenter.Instance.TrackDistance(nearby.transform, transform, 3f);
        }

        private void Complete()
        {
            QuestObjectiveType type = QuestService.ActiveMainQuest?.CurrentObjective?.ObjectiveType ?? QuestObjectiveType.Interact;
            if (type == QuestObjectiveType.TalkToNpc) QuestService.NotifyNpcTalked(targetId); else QuestService.NotifyInteraction(targetId);
            if (targetId == MainQuest07FieldFlow.PaulId) StartEncounter();
            if (GameSaveService.CurrentSlotIndex > 0) GameSaveService.SaveCurrentSession();
        }

        private void StartEncounter()
        {
            MonsterDefinition snake = Resources.LoadAll<MonsterDefinition>("MonsterDefinitions").FirstOrDefault(x => x.MonsterId == "venom_snake");
            FieldMonsterSpawnDefinition spawn = Resources.LoadAll<FieldMonsterSpawnDefinition>("MonsterSpawns")
                .FirstOrDefault(x => x.SceneName == "Field_02" && x.SpawnId == "venom_snake_02");
            BattleSceneFlow.EnterStoryBattle(MainQuest07FieldFlow.EncounterId, snake, spawn, transform.position);
        }

        private DialogueLine[] GetLines()
        {
            string player = string.IsNullOrWhiteSpace(GameSessionData.PlayerName) ? "플레이어" : GameSessionData.PlayerName;
            DialogueLine L(string id, string name, string text) => new DialogueLine(id, name, text);
            if (targetId == MainQuest07FieldFlow.WheelTracksId) return new[] { L(CompanionRosterService.TaeonId,"태온","수레가 지나간 흔적은 아닌 것 같습니다."), L(CompanionRosterService.TaeonId,"태온","폭이 일정하고… 한쪽이 계속 더 깊게 눌려 있어요."), L(CompanionRosterService.MielId,"미엘","누군가 이쪽으로 지나간 것 같네요."), L("",player,"따라가 보죠.") };
            if (targetId == MainQuest07FieldFlow.WoundedTravelerId) return new[] { L(CompanionRosterService.MielId,"미엘","이분을 그냥 두고 갈 수는 없어요. 제가 상태를 볼게요."), L(CompanionRosterService.TaeonId,"태온","혼자 괜찮겠습니까?"), L(CompanionRosterService.MielId,"미엘","네. 두 분은 흔적을 확인해주세요."), L(CompanionRosterService.MielId,"미엘","상황이 안 좋으면 바로 돌아오시고요.") };
            if (targetId == MainQuest07FieldFlow.PaulId) return PaulFirst();
            if (targetId == MainQuest07FieldFlow.MielMeetingId) return MielMeeting();
            return new[] { L(MainQuest07FieldFlow.PaulId,"폴","아무래도 저희가 보고 있는 게 같은 현상 같기는 하네요."), L(MainQuest07FieldFlow.PaulId,"폴","그런데 저는 확인해볼 곳이 하나 더 있습니다."), L("",player,"혼자 가시려고요?"), L(MainQuest07FieldFlow.PaulId,"폴","이번에는 진흙 없는 길로요."), L(MainQuest07FieldFlow.PaulId,"폴","아까 충분히 배웠습니다. 헤헤."), L(MainQuest07FieldFlow.PaulId,"폴","다시 만나게 되면 그때 정보부터 맞춰보죠.") };
        }

        private static DialogueLine[] PaulFirst() => new[]
        {
            new DialogueLine(MainQuest07FieldFlow.PaulId,"폴","아, 사람이다. 반갑습니다. 진짜로요."), new DialogueLine(MainQuest07FieldFlow.PaulId,"폴","원래 계획은 저 진흙을 피해 가는 거였는데요."), new DialogueLine(MainQuest07FieldFlow.PaulId,"폴","보시다시피 계획이 아주 성공적입니다."), new DialogueLine(CompanionRosterService.TaeonId,"태온","바퀴가 빠지셨는데요."), new DialogueLine(MainQuest07FieldFlow.PaulId,"폴","네, 그러게요. 그것도 아주 깊~게요...;;"), new DialogueLine(MainQuest07FieldFlow.PaulId,"폴","하여튼 바로 잡아당기시면 안 됩니다. 그러면 앞바퀴까지 빠질 거예요."), new DialogueLine(MainQuest07FieldFlow.PaulId,"폴","저쪽에 넓은 돌 보이시죠? 그걸 오른쪽 바퀴 뒤에 받쳐주시고요."), new DialogueLine(MainQuest07FieldFlow.PaulId,"폴","제가 바퀴를 돌릴 때 같이 밀면 될 것 같습니다."), new DialogueLine(CompanionRosterService.TaeonId,"태온","옵니다."), new DialogueLine(MainQuest07FieldFlow.PaulId,"폴","아… 저건 계획에 없었는데요."), new DialogueLine(MainQuest07FieldFlow.PaulId,"폴","뭐, 바퀴가 안 움직이는 거지 마법까지 안 나가는 건 아니니까요.")
        };

        private static DialogueLine[] MielMeeting() => new[]
        {
            new DialogueLine(MainQuest07FieldFlow.PaulId,"폴","후우… 됐네요."), new DialogueLine(MainQuest07FieldFlow.PaulId,"폴","다음에는 저 길로 안 갑니다. 경험을 했으면 계획을 수정해야죠."), new DialogueLine(MainQuest07FieldFlow.PaulId,"폴","아, 도와주셔서 감사합니다. 저는 폴이라고 합니다."), new DialogueLine(MainQuest07FieldFlow.PaulId,"폴","저도 이 숲을 좀 조사하고 있었습니다."), new DialogueLine(MainQuest07FieldFlow.PaulId,"폴","며칠 전부터 숲 안쪽의 마력 흐름이 평소랑 달라졌거든요."), new DialogueLine(MainQuest07FieldFlow.PaulId,"폴","처음에는 별일 아닌 줄 알았는데… 몬스터들 움직임까지 달라지는 걸 보고 생각이 바뀌었습니다."), new DialogueLine(CompanionRosterService.TaeonId,"태온","저희도 같은 현상을 따라 여기까지 왔습니다."), new DialogueLine(MainQuest07FieldFlow.PaulId,"폴","그러면 제가 진흙에 빠진 건 몰라도, 방향은 제대로 잡은 것 같네요."),
            new DialogueLine(CompanionRosterService.MielId,"미엘","돌아오셨네요. 그리고… 처음 보는 분도 계시네요."), new DialogueLine(MainQuest07FieldFlow.PaulId,"폴","폴이라고 합니다."), new DialogueLine(MainQuest07FieldFlow.PaulId,"폴","원래는 조금 더 멀쩡한 모습으로 처음 뵐 예정이었는데요. 그늘숲이 협조를 안 해주더라고요."), new DialogueLine(CompanionRosterService.MielId,"미엘","혹시 다치신 곳은 없으세요?"), new DialogueLine(MainQuest07FieldFlow.PaulId,"폴","저는 괜찮습니다. 저분부터 봐주세요."), new DialogueLine(CompanionRosterService.MielId,"미엘","저분은 이제 괜찮으세요. 그러니까 폴 씨도 확인해야죠."), new DialogueLine(MainQuest07FieldFlow.PaulId,"폴","…아, 순서가 벌써 제 차례인가요?")
        };

        private GameObject CreateLabel(string text)
        {
            GameObject root = new GameObject("QuestMarker", typeof(Canvas), typeof(CanvasScaler)); root.transform.SetParent(transform, false); root.transform.localPosition = new Vector3(0,1.5f,0); root.transform.localScale = Vector3.one*.01f;
            Canvas canvas=root.GetComponent<Canvas>(); canvas.renderMode=RenderMode.WorldSpace; canvas.sortingOrder=22; root.GetComponent<RectTransform>().sizeDelta=new Vector2(320,70);
            Text t=new GameObject("Text",typeof(Text)).GetComponent<Text>(); t.transform.SetParent(root.transform,false); t.font=Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"); t.fontSize=24; t.alignment=TextAnchor.MiddleCenter; t.color=new Color(.3f,.78f,1f); t.text=text; t.rectTransform.anchorMin=Vector2.zero; t.rectTransform.anchorMax=Vector2.one; t.rectTransform.offsetMin=Vector2.zero; t.rectTransform.offsetMax=Vector2.zero; return root;
        }
    }
}

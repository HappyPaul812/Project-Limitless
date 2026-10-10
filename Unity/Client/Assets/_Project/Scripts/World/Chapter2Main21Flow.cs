using System.Collections;
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
    /// <summary>Main20 완료 후에만 시작하며 기존 Scene를 변경하지 않고13개의 순차 목표를 설치합니다.</summary>
    public sealed class Chapter2Main21Flow : MonoBehaviour
    {
        public const string QuestId = "main_21_returning_warmth", Field = "Field_10_BurningPulse";
        public const string EncounterA = "field10_main21_encounter_a", EncounterB = "field10_main21_encounter_b";
        public const string ArrivalSpawn = "Spawn_Main21_CarriageArrival";
        public static readonly string[] Targets = {
            "field10_main21_heat_change", "field10_main21_pulse_trace", EncounterA,
            "field10_main21_passage_briefing", "field10_main21_passage_decision", EncounterB,
            "field10_main21_passage_verified", "arbel_main21_report_leon", "arbel_main21_aid_point",
            "starter_village_main21_carriage_arrival", "starter_village_main21_north_trace",
            "starter_village_main21_haren_meeting", "starter_village_main21_departure_briefing" };
        public static readonly Vector2[] Positions = {
            new Vector2(6.3f,-2.6f),new Vector2(3.8f,1),new Vector2(1,-2.6f),new Vector2(-1,2.7f),
            Vector2.zero,new Vector2(-4.8f,-2.5f),new Vector2(-6,2.2f),new Vector2(0,3),
            new Vector2(2,-1),new Vector2(0,-4.65f),new Vector2(0,4.4f),new Vector2(2.8f,2),new Vector2(0,2.5f) };
        public static readonly Vector2[] CluePositions = { new Vector2(-.8f,-2.6f),new Vector2(2,2.6f),new Vector2(-3,2.6f) };
        internal static int BlockedFrame = -1;
        public static bool Current(int index) => QuestService.ActiveMainQuest?.Definition.QuestId == QuestId
            && QuestService.ActiveMainQuest.CurrentObjective?.TargetId == Targets[index];
        public static bool Completed => QuestService.GetState(QuestId) == QuestState.Completed;
        internal static void Save() { if(GameSaveService.CurrentSlotIndex > 0) GameSaveService.SaveCurrentSession(); }
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Register() { BlockedFrame=-1;SceneManager.sceneLoaded-=Install;SceneManager.sceneLoaded+=Install; }
        private static void Install(Scene scene, LoadSceneMode mode)
        {
            // 중간 Field의 구버전 Save도 귀로를 찾도록 기존 Scene에는 경로 안내만 설치합니다.
            bool relevant=scene.name==Field||scene.name=="Arbel"||scene.name=="World_StarterVillage"
                ||scene.name.StartsWith("Field_0")||scene.name==Chapter2Main20Flow.Field;
            if(!relevant||scene.GetRootGameObjects().Any(x=>x.GetComponent<Chapter2Main21Flow>()!=null))return;
            SceneManager.MoveGameObjectToScene(new GameObject("Chapter2Main21Flow",typeof(Chapter2Main21Flow)),scene);
        }
        private IEnumerator Start()
        {
            QuestService.Changed+=Begin;
            // 기존 Runtime Bounds와 NPC 설치 이후 연결하며 기존 구조에 Collider를 추가하지 않습니다.
            yield return null;
            Begin();
            if(DialoguePresenter.Instance==null)new GameObject("DialogueSystem").AddComponent<DialoguePresenter>();
            string scene=gameObject.scene.name;
            if(scene==Field)
            {
                for(int i=0;i<7;i++)Site(i,Positions[i]);
                for(int i=0;i<3;i++)Site(13+i,CluePositions[i]);
            }
            else if(scene=="Arbel")
            {
                var leon=FindObjectsByType<VillageNpcRole>().FirstOrDefault(x=>x.NpcId=="arbel-leon");
                if(leon!=null)QuestNavigationTarget.Attach(leon.gameObject,Targets[7],"레온에게 보고");
                Site(8,Positions[8]);Site(9,new Vector2(-2.2f,-1));
            }
            else if(scene=="World_StarterVillage")
            {
                var spawn=new GameObject(ArrivalSpawn,typeof(SceneSpawnPoint));spawn.transform.SetParent(transform,false);
                spawn.transform.position=Positions[9];spawn.GetComponent<SceneSpawnPoint>().Configure(ArrivalSpawn);
                Site(9,Positions[9]);for(int i=10;i<13;i++)Site(i,Positions[i]);
                yield return RecoverArrival();
            }
            AttachRoutes();
        }
        private void Begin()
        {
            // Main20 완료를 반드시 요구합니다. 미완료 Save나 Main22를 임의 진행하지 않습니다.
            if(gameObject.scene.name==Field&&QuestService.GetState(Chapter2Main20Flow.QuestId)==QuestState.Completed
                &&QuestService.GetState(QuestId)==QuestState.Available&&QuestService.TryStart(QuestId))Save();
        }
        private void Site(int index,Vector2 position)
        {
            var go=new GameObject(index<13?Targets[index]:"main21_route_clue_"+(index-13),typeof(Main21Site));
            go.transform.SetParent(transform,false);go.transform.position=position;go.GetComponent<Main21Site>().Configure(index);
            if(index<13)QuestNavigationTarget.Attach(go,Targets[index],Main21Site.Title(index));
        }
        private void AttachRoutes()
        {
            foreach(var exit in gameObject.scene.GetRootGameObjects().SelectMany(x=>x.GetComponentsInChildren<SceneTransitionTrigger>(true)))
            {
                // 기존 Scene의 방향 이름에 따라 귀환 Quest Navigation을 유지합니다.
                string direction=exit.name.ToLowerInvariant();
                bool east=direction.Contains("east")||direction.Contains("return");
                if(east&&gameObject.scene.name!= "Arbel"&&gameObject.scene.name!="World_StarterVillage")
                    foreach(string target in Targets.Skip(7).Take(3))QuestNavigationTarget.Attach(exit.gameObject,target,"아르벨로 돌아가기");
            }
        }
        public static bool TryHandleNpc(string id,NpcController npc)
        {
            if(id!="arbel-leon"||!Current(7))return false;
            if(!WorldModalState.IsOpen&&DialoguePresenter.Instance!=null)
                DialoguePresenter.Instance.ShowSequence(Main21DialogueCatalog.Get(7),()=>Advance(7));
            return true;
        }
        public static void Advance(int index)
        {
            if(!Current(index)||index==2||index==5||index==9)return;
            BlockedFrame=Time.frameCount;QuestService.NotifyInteraction(Targets[index]);Save();
        }
        public static IEnumerator RecoverArrival()
        {
            if(!Current(9)||!Main21Progress.ReturnAccepted||GameSessionData.LastSpawnPointId!=ArrivalSpawn
                &&GameSessionData.PendingSpawnPointId!=ArrivalSpawn)yield break;
            float deadline=Time.realtimeSinceStartup+10;
            while(Time.realtimeSinceStartup<deadline)
            {
                var player=FindAnyObjectByType<PlayerController>();
                // Spawn 대기 지연과 배치 전 종료 Save 모두 전용 도착 기록·현재 목표·안전 지면을 확인한 뒤 배치를 복구합니다.
                if(player!=null&&(GameSessionData.PendingSpawnPointId==ArrivalSpawn
                    ||GameSessionData.LastSpawnPointId==ArrivalSpawn&&!ArrivalSafe(player)))
                {
                    var bounds=FindAnyObjectByType<WorldBounds2D>();
                    bool clear=Physics2D.OverlapCircleAll(Positions[9],.25f).All(x=>x.transform.IsChildOf(player.transform)
                        ||x.isTrigger&&x.GetComponent<SceneTransitionTrigger>()==null);
                    if(bounds!=null&&bounds.Bounds.Contains(Positions[9])&&clear)
                    {
                        var body=player.GetComponent<Rigidbody2D>();
                        if(body!=null){body.position=Positions[9];body.linearVelocity=Vector2.zero;}else player.transform.position=Positions[9];
                        GameSessionData.ClearPendingSpawnPoint();GameSessionData.RecordLocation("World_StarterVillage",ArrivalSpawn);
                    }
                }
                if(player!=null&&GameSessionData.PendingSpawnPointId==string.Empty&&ArrivalSafe(player))
                {
                    // 위치 저장 실패면 목표를 먼저 진행하지 않고 전용 Spawn 기록에서 재시도합니다.
                    if(GameSaveService.CurrentSlotIndex>0&&!GameSaveService.SaveCurrentWorldPosition(player.transform.position,"World_StarterVillage",ArrivalSpawn))yield break;
                    QuestService.NotifyLocationReached(Targets[9]);Save();yield break;
                }
                yield return null;
            }
            Debug.LogWarning("Main21 도착 배치 확인 지연: 목표를 유지합니다. 안전 위치에서 재시도하세요.");
        }
        public static bool ArrivalSafe(PlayerController player)
        {
            var bounds=FindAnyObjectByType<WorldBounds2D>();
            if(player.gameObject.scene.name!="World_StarterVillage"||bounds==null
                ||Vector2.Distance(player.transform.position,Positions[9])>.3f||!bounds.Bounds.Contains(player.transform.position))return false;
            foreach(var collider in Physics2D.OverlapCircleAll(player.transform.position,.25f))
                if(!collider.transform.IsChildOf(player.transform)&&(!collider.isTrigger||collider.GetComponent<SceneTransitionTrigger>()!=null))return false;
            return true;
        }
        private void OnDestroy()=>QuestService.Changed-=Begin;
    }

    /// <summary>거리·현재 목표·모달 경계를 확인하고 자유 순서 단서는 별도 저장합니다.</summary>
    public sealed class Main21Site : MonoBehaviour
    {
        public int Index { get; private set; }
        private InputAction input;private bool busy,aidInspected;private TextMesh label;
        public void Configure(int index)=>Index=index;
        public static string Title(int i)
        {
            string[] titles={"달라진 열기","잔류 진동","꺼지지 않은 불씨","통행로 조사","안전 통로 선택","마지막 통로의 위협",
                "통행 안전 확인","레온에게 보고","주민 상황 확인 / 보급품 전달","접근 가능한 귀환 마차","북풍·서리 조사",
                "하렌 · ART_PENDING","북쪽 탐험 준비","A 중앙 균열길","B 암반 지름길","C 옛 우회로"};return titles[i];
        }
        private void Awake()
        {
            input=new InputAction("Main21Inspect",InputActionType.Button);input.AddBinding("<Keyboard>/e");input.AddBinding("<Keyboard>/f");
            input.AddBinding("<Gamepad>/buttonSouth");input.performed+=_=>TryInteract();
        }
        private void OnEnable()=>input?.Enable();private void OnDisable()=>input?.Disable();private void OnDestroy()=>input?.Dispose();
        private void Start()
        {
            label=new GameObject("Main21Label",typeof(TextMesh)).GetComponent<TextMesh>();label.transform.SetParent(transform,false);
            label.transform.localPosition=Vector3.up*.45f;label.text=Title(Index)+" [E/F / A]";label.characterSize=.10f;
            label.anchor=TextAnchor.MiddleCenter;label.font=Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");label.fontSize=32;
            label.GetComponent<MeshRenderer>().sharedMaterial=label.font.material;
        }
        private bool Available=>Index>=13?Chapter2Main21Flow.Current(4):Chapter2Main21Flow.Current(Index);
        private void Update()
        {
            if(busy&&(DialoguePresenter.Instance==null||!DialoguePresenter.Instance.IsOpen)&&!WorldModalState.IsOpen)busy=false;
            if(label!=null){label.gameObject.SetActive(Available&&!WorldModalState.IsOpen);
                if(Index==4&&QuestService.ActiveMainQuest?.Definition.QuestId==Chapter2Main21Flow.QuestId
                    &&QuestService.ActiveMainQuest.CurrentObjectiveIndex>4){label.gameObject.SetActive(true);label.text="옛 우회로 · 안전 표시";}}
        }
        public bool TryInteract()
        {
            var player=FindAnyObjectByType<PlayerController>();var d=DialoguePresenter.Instance;
            if(!Available||busy||WorldModalState.IsOpen||player==null||Vector2.Distance(player.transform.position,transform.position)>.8f
                ||Time.frameCount<=Chapter2Main21Flow.BlockedFrame||d==null||!d.CanBeginInteractionThisFrame)return false;
            Chapter2Main21Flow.BlockedFrame=Time.frameCount;
            if(Index==2||Index==5)
            {
                var spawn=Resources.Load<FieldMonsterSpawnDefinition>("StoryEncounterReturns/Main21_Encounter"+(Index==2?"A":"B"));
                return spawn!=null&&BattleSceneFlow.EnterStoryBattle(Chapter2Main21Flow.Targets[Index],spawn.Monster,spawn,transform.position);
            }
            if(Index>=13)
            {
                int clue=Index-13;string[] facts={"표면 온도는 내려갔지만 일정 간격의 지면 진동이 남아 있다.","거리는 짧지만 바닥에 높은 잔열이 남아 있다.","지면이 안정됐고 잔열이 낮다. 통행 폭도 확보돼 있다."};
                busy=true;d.ShowSequence(new[]{new DialogueLine("","","<지문> "+Title(Index)+"\n"+facts[clue])},()=>{busy=false;Main21Progress.Observe(clue);Chapter2Main21Flow.Save();});return true;
            }
            if(Index==4)
            {
                if(!Main21Progress.AllClues){d.Show("조사","세 지점의 단서를 모두 확인한 뒤 비교하세요. 조사 순서는 자유입니다.");return true;}
                Main21ChoicePanel.Show("안전하게 통행할 길을 선택하세요.",new[]{"A 중앙 균열길","B 암반 지름길","C 옛 우회로","다시 조사한다"},ChoosePassage);return true;
            }
            if(Index==9&&gameObject.scene.name=="World_StarterVillage")
            {
                StartCoroutine(Chapter2Main21Flow.RecoverArrival());return true;
            }
            if(Index==9)
            {
                d.ShowSequence(Main21DialogueCatalog.Get(9),()=>Main21ChoicePanel.Show("휠체어2대와 동료 좌석을 갖춘 같은 마차로 귀환합니다.",
                    new[]{"마차로 돌아간다","아직 준비가 안 됐다"},choice=>{if(choice==0)Main21CarriageReturnController.Begin();}));return true;
            }
            // 주민 지원은 현장 확인과 배치·전달의 두 단계입니다. 소모품이나 무료 회복을 추가하지 않습니다.
            if(Index==8&&!aidInspected)
            {
                busy=true;d.ShowSequence(new[]{new DialogueLine("","","<지문> 주민들이 기다리는 장소와 보급 상자의 수량을 확인합니다. 붕대를 전달할 공간이 비어 있습니다.")},()=>{busy=false;aidInspected=true;label.text="보급 상자 배치·전달 [E/F / A]";});return true;
            }
            busy=true;d.ShowSequence(Main21DialogueCatalog.Get(Index),()=>{busy=false;Chapter2Main21Flow.Advance(Index);});return true;
        }
        public static void ChoosePassage(int choice)
        {
            if(!Chapter2Main21Flow.Current(4)||!Main21Progress.AllClues||choice==3)return;
            var d=DialoguePresenter.Instance;if(d==null)return;
            if(choice!=2){d.Show("통행로 확인",choice==0?"중앙 균열길은 지면 진동이 남아 있어 안전하지 않습니다. 다시 조사할 수 있습니다.":"암반 지름길은 높은 잔열이 남아 있어 안전하지 않습니다. 다시 조사할 수 있습니다.");return;}
            d.ShowSequence(Main21DialogueCatalog.Get(4),()=>Chapter2Main21Flow.Advance(4));
        }
    }
}

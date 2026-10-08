using System.Linq;
using ProjectLimitless.Core;
using ProjectLimitless.Player;
using ProjectLimitless.UI;
using ProjectLimitless.Monster;
using ProjectLimitless.Battle;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace ProjectLimitless.World
{
    /// <summary>Main18 완료 기록을 이어받는 Main19 조사 흐름입니다. 기존 Scene·Party·상태 규칙을 재생성하지 않습니다.</summary>
    public sealed class Chapter2Main19Flow:MonoBehaviour
    {
        public const string QuestId="main_19_burning_pulse",Field="Field_10_BurningPulse";
        public const string PatrolA="field10_main19_patrol_a",PatrolB="field10_main19_patrol_b";
        public static readonly string[] Targets={"field09_main19_gate","field10_main19_entry","field10_main19_track_1","field10_main19_cliff",PatrolA,
            "field10_main19_pulse_1","field10_main19_corridor",PatrolB,"field10_main19_track_2","field10_main19_ridge","field10_main19_witness","field10_main19_withdraw"};
        public static readonly Vector2[] Positions={new Vector2(-7.6f,0),new Vector2(7.8f,0),new Vector2(5.5f,1),new Vector2(3.4f,-2),new Vector2(1.2f,-.8f),
            new Vector2(-.6f,1.8f),new Vector2(-2.5f,.5f),new Vector2(-4,-1.3f),new Vector2(-5.3f,1),new Vector2(-7,.8f),new Vector2(-7.6f,.2f),new Vector2(-6.2f,-1.2f)};
        static int blockedFrame=-1;
        internal static bool CanInteract=>Time.frameCount>blockedFrame;
        internal static void BlockFrame()=>blockedFrame=Time.frameCount;
        public static bool Unlocked=>QuestService.GetState(Chapter2Main18Flow.QuestId)==QuestState.Completed;
        public static bool IsCurrent(int i)=>QuestService.ActiveMainQuest?.Definition.QuestId==QuestId&&QuestService.ActiveMainQuest.CurrentObjective?.TargetId==Targets[i];
        internal static void Save(){if(GameSaveService.CurrentSlotIndex>0)GameSaveService.SaveCurrentSession();}
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Register(){blockedFrame=-1;SceneManager.sceneLoaded-=Install;SceneManager.sceneLoaded+=Install;}
        static void Install(Scene scene,LoadSceneMode mode)
        {
            if(scene.name!=Field&&scene.name!=Chapter2Main18Flow.Field)return;
            if(scene.GetRootGameObjects().Any(x=>x.GetComponent<Chapter2Main19Flow>()!=null))return;
            SceneManager.MoveGameObjectToScene(new GameObject("Chapter2Main19Flow",typeof(Chapter2Main19Flow)),scene);
        }
        void Start()
        {
            QuestService.Changed+=Begin;Begin();
            if(DialoguePresenter.Instance==null)new GameObject("DialogueSystem").AddComponent<DialoguePresenter>();
            if(gameObject.scene.name==Chapter2Main18Flow.Field)
            {
                // Field09 실행 중 Bounds만 새 Exit에 맞춥니다. 완료 전에는 물리 Gate가 기존 폐쇄를 유지합니다.
                var bounds=gameObject.scene.GetRootGameObjects().Select(x=>x.GetComponent<WorldBounds2D>()).First(x=>x!=null);
                foreach(Transform child in bounds.transform.Cast<Transform>().Where(x=>x.name.StartsWith("Boundary_")).ToArray())Destroy(child.gameObject);
                WorldBounds2D.CreateBoundaryColliders(transform,bounds.Bounds,.3f,new WorldBoundaryOpening(WorldBoundarySide.Left,0,3),new WorldBoundaryOpening(WorldBoundarySide.Right,0,3));
                var gate=new GameObject("Main19AccessGate",typeof(BoxCollider2D),typeof(Main19AccessGate));gate.transform.SetParent(transform,false);
                gate.transform.position=new Vector2(-9.45f,0);gate.GetComponent<BoxCollider2D>().size=new Vector2(.35f,3);
                Site(0);
            }
            else
            {
                for(int i=1;i<12;i++)Site(i);Site(12);
                var serin=new GameObject("Serin_Story_Main19",typeof(Animator));serin.transform.SetParent(transform,false);serin.transform.position=new Vector2(4.7f,1.8f);
                serin.AddComponent<SpriteRenderer>().sortingOrder=8;serin.GetComponent<Animator>().runtimeAnimatorController=Resources.Load<RuntimeAnimatorController>("Chapter2/Serin");
            }
            foreach(var exit in gameObject.scene.GetRootGameObjects().SelectMany(x=>x.GetComponentsInChildren<SceneTransitionTrigger>(true)))
                if(exit.name.Contains("west_to_field10"))foreach(string target in Targets)QuestNavigationTarget.Attach(exit.gameObject,target,"맥동의 열맥으로");
        }
        void OnDestroy()=>QuestService.Changed-=Begin;
        static void Begin(){if(QuestService.GetState(QuestId)==QuestState.Available&&QuestService.TryStart(QuestId))Save();}
        void Site(int index)
        {
            var go=new GameObject(index==12?"Main19ClosedDeepRoute":Targets[index],typeof(Main19Site));go.transform.SetParent(transform,false);
            go.transform.position=index==12?new Vector2(-8.8f,-2.8f):Positions[index];go.GetComponent<Main19Site>().Configure(index);
            if(index<12)QuestNavigationTarget.Attach(go,Targets[index],index==4||index==7?"혼합 조우 재도전":"맥동 조사");
        }
    }
    /// <summary>Main18 완료 전에는 출입을 막고 완료 후에는 과거 막힌길 안내만 숨깁니다. Scene 파일은 쓰지 않습니다.</summary>
    public sealed class Main19AccessGate:MonoBehaviour
    {
        void Update()
        {
            bool unlocked=Chapter2Main19Flow.Unlocked;GetComponent<BoxCollider2D>().enabled=!unlocked;
            if(unlocked){var old=GameObject.Find("Main18ClosedDeepRoute");if(old!=null)old.SetActive(false);}
        }
    }
    /// <summary>순서대로 Location 또는 명시 대화를 완료하며 지정전투는 승리 전까지 같은 위치에서 재도전합니다.</summary>
    public sealed class Main19Site:MonoBehaviour
    {
        int index;bool busy;InputAction action;TextMesh marker;
        public void Configure(int value)=>index=value;
        bool Current=>index==12||Chapter2Main19Flow.IsCurrent(index);
        bool Near(){var p=FindAnyObjectByType<PlayerController>();return p!=null&&Vector2.Distance(p.transform.position,transform.position)<.55f;}
        void Awake(){action=new InputAction("Main19Inspect",InputActionType.Button);action.AddBinding("<Keyboard>/e");action.AddBinding("<Keyboard>/f");action.AddBinding("<Gamepad>/buttonSouth");action.performed+=_=>TryInteract();}
        void OnEnable()=>action?.Enable();void OnDisable()=>action?.Disable();void OnDestroy()=>action?.Dispose();
        void Start()
        {
            marker=new GameObject("SiteLabel",typeof(TextMesh)).GetComponent<TextMesh>();marker.transform.SetParent(transform,false);marker.transform.localPosition=Vector3.up;
            marker.text=index==12?"심부 통행 불가 [E/F / A]":index==4||index==7?"혼합 무리 [E/F / A]":"맥동 조사 [E/F / A]";
            marker.characterSize=.12f;marker.anchor=TextAnchor.MiddleCenter;marker.font=Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");marker.fontSize=32;marker.GetComponent<MeshRenderer>().sharedMaterial=marker.font.material;
        }
        void Update()
        {
            if(busy&&(DialoguePresenter.Instance==null||!DialoguePresenter.Instance.IsOpen))busy=false;
            if(marker!=null)marker.gameObject.SetActive(Current&&!WorldModalState.IsOpen);
            if((index==0||index==1||index==6||index==9)&&Chapter2Main19Flow.IsCurrent(index)&&Near()&&!WorldModalState.IsOpen)
            {QuestService.NotifyLocationReached(Chapter2Main19Flow.Targets[index]);Chapter2Main19Flow.Save();}
        }
        public void TryInteract()
        {
            var d=DialoguePresenter.Instance;
            if(!Current||busy||WorldModalState.IsOpen||!Chapter2Main19Flow.CanInteract||!Near()||d==null||!d.CanBeginInteractionThisFrame)return;
            if(index==0||index==1||index==6||index==9)return;
            if(index==4||index==7)
            {
                var spawn=Resources.Load<FieldMonsterSpawnDefinition>("StoryEncounterReturns/Main19_"+(index==4?"PatrolA":"PatrolB"));
                if(spawn==null||spawn.Monster==null){Debug.LogError("Main19 지정전투 복귀 데이터가 없습니다.");return;}
                Chapter2Main19Flow.BlockFrame();BattleSceneFlow.EnterStoryBattle(Chapter2Main19Flow.Targets[index],spawn.Monster,spawn,transform.position);return;
            }
            busy=true;d.ShowSequence(Main19DialogueCatalog.Get(index),()=>
            {
                busy=false;Chapter2Main19Flow.BlockFrame();if(index==12||!Chapter2Main19Flow.IsCurrent(index))return;
                QuestService.NotifyInteraction(Chapter2Main19Flow.Targets[index]);Chapter2Main19Flow.Save();
            });
        }
    }
}

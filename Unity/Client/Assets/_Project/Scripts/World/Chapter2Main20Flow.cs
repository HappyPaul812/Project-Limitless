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
    /// <summary>Main19 완료 후 심부 진입부터 Boss 승리·잔해·냉각·귀환까지 이어갑니다. 진행은 기존 Quest Save만 사용합니다.</summary>
    public sealed class Chapter2Main20Flow : MonoBehaviour
    {
        public const string QuestId="main_20_colossus_of_the_depths", Field="Field_11_DeepCore";
        public const string EncounterId="field11_main20_veinfire_colossus";
        public static readonly string[] Targets={"field11_main20_entry","field11_main20_core_rift","field11_main20_trace",
            "field11_main20_arena","field11_main20_confront",EncounterId,"field11_main20_collapsed_core","field11_main20_heat_recession","field11_main20_return"};
        public static readonly Vector2[] Positions={new Vector2(7.8f,0),new Vector2(5,1.1f),new Vector2(2.8f,.2f),new Vector2(0,0),
            new Vector2(-1.7f,0),new Vector2(-1.7f,0),new Vector2(-2,1),new Vector2(1,-1.2f),new Vector2(7.8f,0)};
        public static bool Unlocked=>QuestService.GetState(Chapter2Main19Flow.QuestId)==QuestState.Completed;
        public static bool Current(int index)=>QuestService.ActiveMainQuest?.Definition.QuestId==QuestId&&QuestService.ActiveMainQuest.CurrentObjective?.TargetId==Targets[index];
        public static bool Won=>QuestService.GetState(QuestId)==QuestState.Completed||QuestService.ActiveMainQuest?.Definition.QuestId==QuestId&&QuestService.ActiveMainQuest.CurrentObjectiveIndex>=6;
        internal static int BlockedFrame=-1;
        internal static void Save(){if(GameSaveService.CurrentSlotIndex>0)GameSaveService.SaveCurrentSession();}
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Register(){BlockedFrame=-1;SceneManager.sceneLoaded-=Install;SceneManager.sceneLoaded+=Install;}
        private static void Install(Scene scene,LoadSceneMode mode)
        {
            if(scene.name!=Field&&scene.name!=Chapter2Main19Flow.Field)return;
            if(scene.GetRootGameObjects().Any(x=>x.GetComponent<Chapter2Main20Flow>()!=null))return;
            SceneManager.MoveGameObjectToScene(new GameObject("Chapter2Main20Flow",typeof(Chapter2Main20Flow)),scene);
        }
        private void Start()
        {
            QuestService.Changed+=Begin;Begin();
            if(gameObject.scene.name!=Field)
            {
                // 기존 Field10 Scene과 동쪽 이동은 그대로 두고 Runtime 서쪽 경계만 개방합니다.
                var bounds=gameObject.scene.GetRootGameObjects().Select(x=>x.GetComponent<WorldBounds2D>()).First(x=>x!=null);
                foreach(Transform child in bounds.transform.Cast<Transform>().Where(x=>x.name.StartsWith("Boundary_")).ToArray())Destroy(child.gameObject);
                WorldBounds2D.CreateBoundaryColliders(transform,bounds.Bounds,.3f,new WorldBoundaryOpening(WorldBoundarySide.Left,0,3),new WorldBoundaryOpening(WorldBoundarySide.Right,0,3));
                var gate=new GameObject("Main20AccessGate",typeof(BoxCollider2D),typeof(Main20AccessGate));gate.transform.SetParent(transform,false);
                gate.transform.position=new Vector2(-9.45f,0);gate.GetComponent<BoxCollider2D>().size=new Vector2(.35f,3);
                return;
            }
            if(DialoguePresenter.Instance==null)new GameObject("DialogueSystem").AddComponent<DialoguePresenter>();
            for(int i=0;i<Targets.Length;i++)
            {
                var site=new GameObject(Targets[i],typeof(Main20Site));site.transform.SetParent(transform,false);site.transform.position=Positions[i];
                site.GetComponent<Main20Site>().Configure(i);QuestNavigationTarget.Attach(site,Targets[i],i==5?"열맥 거신 재도전":"심부 조사");
            }
            gameObject.AddComponent<Main20FieldPresentation>();
        }
        private static void Begin(){if(Unlocked&&QuestService.GetState(QuestId)==QuestState.Available&&QuestService.TryStart(QuestId))Save();}
        private void OnDestroy()=>QuestService.Changed-=Begin;
    }
    /// <summary>Main19 선행 완료가 실제 물리 출입구와 안내를 함께 해제합니다.</summary>
    public sealed class Main20AccessGate : MonoBehaviour
    {
        private void Update()
        {
            bool open=Chapter2Main20Flow.Unlocked;GetComponent<BoxCollider2D>().enabled=!open;
            var old=GameObject.Find("Main19ClosedDeepRoute");if(old!=null)old.SetActive(!open);
        }
    }
    /// <summary>현재 목표만 가까운 거리에서 진행합니다. 대화 완료 경계/한 프레임 차단으로 입력 중복과 보상 반복을 막습니다.</summary>
    public sealed class Main20Site : MonoBehaviour
    {
        private int index;private bool busy;private InputAction action;private TextMesh label;
        public void Configure(int value)=>index=value;
        private bool Near(){var p=FindAnyObjectByType<PlayerController>();return p!=null&&Vector2.Distance(p.transform.position,transform.position)<.75f;}
        private void Awake(){action=new InputAction("Main20Inspect",InputActionType.Button);action.AddBinding("<Keyboard>/e");action.AddBinding("<Keyboard>/f");action.AddBinding("<Gamepad>/buttonSouth");action.performed+=_=>TryInteract();}
        private void OnEnable()=>action?.Enable();private void OnDisable()=>action?.Disable();private void OnDestroy()=>action?.Dispose();
        private void Start()
        {
            label=new GameObject("SiteLabel",typeof(TextMesh)).GetComponent<TextMesh>();label.transform.SetParent(transform,false);label.transform.localPosition=Vector3.up;
            label.text=index==5?"열맥 거신 재도전 [E/F / A]":"심부 조사 [E/F / A]";label.characterSize=.11f;label.anchor=TextAnchor.MiddleCenter;
            label.font=Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");label.fontSize=32;label.GetComponent<MeshRenderer>().sharedMaterial=label.font.material;
        }
        private void Update()
        {
            bool current=Chapter2Main20Flow.Current(index);if(label!=null)label.gameObject.SetActive(current&&!WorldModalState.IsOpen);
            if(current&&(index==0||index==2||index==3||index==8)&&Near()&&!WorldModalState.IsOpen&&Time.frameCount>Chapter2Main20Flow.BlockedFrame)
            {QuestService.NotifyLocationReached(Chapter2Main20Flow.Targets[index]);Chapter2Main20Flow.BlockedFrame=Time.frameCount;Chapter2Main20Flow.Save();}
        }
        public void TryInteract()
        {
            var d=DialoguePresenter.Instance;
            if(!Chapter2Main20Flow.Current(index)||busy||WorldModalState.IsOpen||!Near()||d==null||!d.CanBeginInteractionThisFrame||Time.frameCount<=Chapter2Main20Flow.BlockedFrame)return;
            if(index==0||index==2||index==3||index==8)return;
            if(index==5){Battle();return;}
            busy=true;
            d.ShowSequence(Main20DialogueCatalog.Get(index),()=>{
                busy=false;Chapter2Main20Flow.BlockedFrame=Time.frameCount;
                if(!Chapter2Main20Flow.Current(index))return;
                QuestService.NotifyInteraction(Chapter2Main20Flow.Targets[index]);Chapter2Main20Flow.Save();
                if(index==4&&Chapter2Main20Flow.Current(5))Battle();
            });
        }
        private void Battle()
        {
            var spawn=Resources.Load<FieldMonsterSpawnDefinition>("StoryEncounterReturns/Main20_ColossusReturn");
            if(spawn==null||spawn.Monster==null){Debug.LogError("Main20 Boss 반환 데이터 누락");return;}
            Chapter2Main20Flow.BlockedFrame=Time.frameCount;
            BattleSceneFlow.EnterStoryBattle(Chapter2Main20Flow.EncounterId,spawn.Monster,spawn,transform.position);
        }
    }
}

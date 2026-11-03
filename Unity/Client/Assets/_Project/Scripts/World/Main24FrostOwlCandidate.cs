using ProjectLimitless.Battle;
using ProjectLimitless.Core;
using ProjectLimitless.Monster;
using ProjectLimitless.Player;
using ProjectLimitless.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace ProjectLimitless.World
{
    /// <summary>
    /// 기존 F2에 선택형 검증 조우 하나만 설치합니다. 정식 몬스터/보상/퀘스트 ID를 확정하지 않습니다.
    /// 수치는 기존 슬라임을 차용한 후보이며, 한기·유료상품·Main24 완료는 여기서 처리하지 않습니다.
    /// </summary>
    public sealed class Main24FrostOwlCandidate : MonoBehaviour
    {
        public const string ResourceRoot = "Main24FrostOwlCandidate/";
        public const string MonsterId = "CANDIDATE_frost_owl_basic_qa";
        public static readonly Vector2 EncounterPosition = new Vector2(5.7f,-2f);
        public static bool IsEncounter => IsDefinition(BattleEncounterContext.Monster)
            && BattleEncounterContext.FieldSceneName == Main24F2Traversal.SceneId;
        public static bool IsDefinition(MonsterDefinition value) => value != null && value.MonsterId == MonsterId;
        FieldMonsterSpawnDefinition spawn;
        bool choosing;
        InputAction interact;

        void Awake()
        {
            // 기존 InteractionSystem처럼 이벤트를 받아 Fixed/Dynamic 입력 갱신 사이의 누락을 피합니다.
            interact=new InputAction("F2OwlCandidateInteract",InputActionType.Button);
            interact.AddBinding("<Keyboard>/e");interact.AddBinding("<Keyboard>/f");
            interact.AddBinding("<Gamepad>/buttonSouth");interact.AddBinding("<Mouse>/leftButton");
            interact.performed+=context=>{
                if(context.control.device is Mouse&&(Camera.main==null||Vector2.Distance(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()),transform.position)>.9f))return;
                Choose();
            };
        }
        void OnEnable()=>interact?.Enable();
        void OnDisable()=>interact?.Disable();
        void OnDestroy()=>interact?.Dispose();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Register() { SceneManager.sceneLoaded -= Install; SceneManager.sceneLoaded += Install; }
        static void Install(Scene scene,LoadSceneMode mode)
        {
            if(scene.name!=Main24F2Traversal.SceneId||Object.FindAnyObjectByType<Main24FrostOwlCandidate>()!=null)return;
            var definition=Resources.Load<FieldMonsterSpawnDefinition>(ResourceRoot+"Encounter");
            if(definition==null)return; // 이 후보 자산을 넣지 않은 제품에는 아무것도 설치하지 않습니다.
            var go=new GameObject("CANDIDATE_FrostOwl_OptionalEncounter");SceneManager.MoveGameObjectToScene(go,scene);
            go.transform.position=EncounterPosition;go.AddComponent<Main24FrostOwlCandidate>().spawn=definition;
        }
        void Start()
        {
            var sprite=new GameObject("ApprovedIdle",typeof(SpriteRenderer),typeof(MonsterSpriteSheetAnimation));sprite.transform.SetParent(transform,false);
            var view=sprite.GetComponent<SpriteRenderer>();view.sortingOrder=3;
            sprite.GetComponent<MonsterSpriteSheetAnimation>().Configure(view,spawn.Monster);
            // Collider를 추가하지 않아 도보/휠체어 길을 막지 않습니다. 접근한 뒤 명시 선택만 받습니다.
            var go=new GameObject("CandidateLabel",typeof(RectTransform),typeof(Canvas),typeof(UnityEngine.UI.Text));go.transform.SetParent(transform,false);go.transform.localPosition=Vector3.up*1.4f;go.transform.localScale=Vector3.one*.01f;
            var canvas=go.GetComponent<Canvas>();canvas.renderMode=RenderMode.WorldSpace;canvas.overrideSorting=true;canvas.sortingOrder=6000;
            go.GetComponent<RectTransform>().sizeDelta=new Vector2(420,80);
            var text=go.GetComponent<UnityEngine.UI.Text>();text.font=Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");text.fontSize=20;text.color=Color.white;text.alignment=TextAnchor.MiddleCenter;text.raycastTarget=false;text.text="서리올빼미 · 격리 전투 후보\nE/F/클릭 · 한기 HOLD / 보상 없음";
            var outline=go.AddComponent<UnityEngine.UI.Outline>();outline.effectColor=Color.black;outline.effectDistance=new Vector2(2,-2);
        }
        void Update()
        {
            if(choosing&&!WorldModalState.IsOpen)choosing=false;
        }
        public bool Choose()
        {
            var player=Object.FindAnyObjectByType<PlayerController>();
            if(choosing||WorldModalState.IsOpen||DialoguePresenter.Instance?.IsOpen==true
                ||DialoguePresenter.Instance!=null&&!DialoguePresenter.Instance.CanBeginInteractionThisFrame
                ||player==null||Vector2.Distance(player.transform.position,transform.position)>1.8f)return false;
            choosing=true;
            Main21ChoicePanel.Show("서리올빼미 기본공격 검증 · 수치 임시 / 한기 미적용 / 보상 없음",new[]{"전투 시작","머무른다"},index=>{choosing=false;if(index==0)TryEnter();});return true;
        }
        public bool TryEnter()
        {
            if(WorldModalState.IsOpen||BattleSceneFlow.IsTransitioning||spawn==null||!Application.CanStreamedLevelBeLoaded(BattleSceneFlow.BattleSceneName))return false;
            var player=Object.FindAnyObjectByType<PlayerController>();if(player==null||Vector2.Distance(player.transform.position,transform.position)>1.8f)return false;
            // 기존 전투 전환의 단일 로드 잠금·복귀 Context·재조우 억제를 그대로 사용합니다.
            if(!MonsterEncounterService.TryRaise(spawn.Monster,spawn))return false;
            return BattleSceneFlow.IsTransitioning&&BattleEncounterContext.Spawn==spawn;
        }
    }
}

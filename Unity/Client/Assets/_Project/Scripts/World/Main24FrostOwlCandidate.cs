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
    /// 기존 F2의 선택 조우 표시를 재사용합니다. 조사 단계가 끝났을 때만 지정 전투를 열고,
    /// 승리 단계로 넘어가면 같은 조우를 다시 열지 않습니다.
    /// </summary>
    public sealed class Main24FrostOwlCandidate : MonoBehaviour
    {
        public const string ResourceRoot = "Main24FrostOwlCandidate/";
        public const string MonsterId = "frost_owl";
        public const string EncounterId = "main24_first_frost_owl";
        public static readonly Vector2 EncounterPosition = new Vector2(5.7f,-2f);
        public static bool IsEncounter => IsDefinition(BattleEncounterContext.Monster)
            && BattleEncounterContext.FieldSceneName == Main24F2Traversal.SceneId;
        public static bool IsDefinition(MonsterDefinition value) => value != null && value.MonsterId == MonsterId;
        FieldMonsterSpawnDefinition spawn;
        bool choosing;
        InputAction interact;
        RectTransform label;
        Canvas labelCanvas;
        RectTransform guidancePanel;
        UnityEngine.UI.Button[] hudButtons;
        readonly Vector3[] labelCorners = new Vector3[4];
        readonly System.Collections.Generic.List<Rect> labelObstacles = new System.Collections.Generic.List<Rect>();
        const float LabelScreenPadding = 8f;

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
        void OnDestroy(){interact?.Dispose();Canvas.willRenderCanvases-=KeepLabelInView;}

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Register() { SceneManager.sceneLoaded -= Install; SceneManager.sceneLoaded += Install; }
        static void Install(Scene scene,LoadSceneMode mode)
        {
            if(scene.name!=Main24F2Traversal.SceneId||Object.FindAnyObjectByType<Main24FrostOwlCandidate>()!=null)return;
            var definition=Resources.Load<FieldMonsterSpawnDefinition>(ResourceRoot+"Main24_FirstFrostOwl");
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
            var text=go.GetComponent<UnityEngine.UI.Text>();text.font=Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");text.fontSize=20;text.color=Color.white;text.alignment=TextAnchor.MiddleCenter;text.raycastTarget=false;text.text="서리올빼미\nE/F/클릭 · 한기 공격 1회 / 첫 한기 전투";
            var outline=go.AddComponent<UnityEngine.UI.Outline>();outline.effectColor=Color.black;outline.effectDistance=new Vector2(2,-2);
            label=go.GetComponent<RectTransform>();labelCanvas=canvas;
            // CameraFollow의 LateUpdate 뒤 실제 화면을 기준으로 이 안내 글만 보정합니다.
            Canvas.willRenderCanvases+=KeepLabelInView;
        }

        /// <summary>
        /// 월드 Canvas 자체에는 여유가 있어도 Camera 밖으로 투영되면 글이 잘립니다.
        /// 조우/이동/Camera는 건드리지 않고, 화면에 걸친 표식의 사각형만 최소 거리로 옮깁니다.
        /// 완전히 화면 밖인 표식은 기존처럼 숨겨 먼 곳의 안내가 HUD처럼 따라붙지 않게 합니다.
        /// </summary>
        void KeepLabelInView()
        {
            var camera=Camera.main;if(label==null||camera==null)return;
            label.localPosition=Vector3.up*1.4f;
            Rect original=ScreenBounds(label,camera),viewport=camera.pixelRect;
            labelCanvas.enabled=camera.WorldToScreenPoint(label.position).z>0&&viewport.Overlaps(original);
            if(!labelCanvas.enabled)return;
            Rect safe=Rect.MinMaxRect(viewport.xMin+LabelScreenPadding,viewport.yMin+LabelScreenPadding,viewport.xMax-LabelScreenPadding,viewport.yMax-LabelScreenPadding);
            Rect clamped=new Rect(Mathf.Clamp(original.xMin,safe.xMin,safe.xMax-original.width),Mathf.Clamp(original.yMin,safe.yMin,safe.yMax-original.height),original.width,original.height);
            if(hudButtons==null)hudButtons=Object.FindObjectsByType<UnityEngine.UI.Button>(FindObjectsSortMode.None);
            if(guidancePanel==null)guidancePanel=Object.FindAnyObjectByType<Main23GuidanceHud>()?.transform.Find("Main23Guidance/Panel") as RectTransform;
            labelObstacles.Clear();
            if(guidancePanel!=null&&guidancePanel.gameObject.activeInHierarchy)labelObstacles.Add(ScreenBounds(guidancePanel,camera));
            foreach(var button in hudButtons)if(button!=null&&button.isActiveAndEnabled)labelObstacles.Add(ScreenBounds(button.transform as RectTransform,camera));
            // 화면 안의 원래 위치가 HUD와 겹치지 않으면 그대로 유지합니다.
            Rect best=clamped;float distance=float.PositiveInfinity;
            ConsiderLabelRect(clamped,original,safe,ref best,ref distance);
            foreach(var obstacle in labelObstacles)
            {
                ConsiderLabelRect(new Rect(obstacle.xMax+LabelScreenPadding,clamped.y,clamped.width,clamped.height),original,safe,ref best,ref distance);
                ConsiderLabelRect(new Rect(obstacle.xMin-LabelScreenPadding-clamped.width,clamped.y,clamped.width,clamped.height),original,safe,ref best,ref distance);
                ConsiderLabelRect(new Rect(clamped.x,obstacle.yMax+LabelScreenPadding,clamped.width,clamped.height),original,safe,ref best,ref distance);
                ConsiderLabelRect(new Rect(clamped.x,obstacle.yMin-LabelScreenPadding-clamped.height,clamped.width,clamped.height),original,safe,ref best,ref distance);
            }
            var projected=camera.WorldToScreenPoint(label.position);var shift=best.position-original.position;
            label.position=camera.ScreenToWorldPoint(projected+new Vector3(shift.x,shift.y,0));
        }

        void ConsiderLabelRect(Rect candidate,Rect original,Rect safe,ref Rect best,ref float distance)
        {
            if(candidate.xMin<safe.xMin||candidate.yMin<safe.yMin||candidate.xMax>safe.xMax||candidate.yMax>safe.yMax)return;
            foreach(var obstacle in labelObstacles)if(candidate.Overlaps(obstacle))return;
            float next=(candidate.position-original.position).sqrMagnitude;
            if(next<distance){best=candidate;distance=next;}
        }

        Rect ScreenBounds(RectTransform rect,Camera camera)
        {
            rect.GetWorldCorners(labelCorners);var canvas=rect.GetComponentInParent<Canvas>();
            Camera projection=canvas!=null&&canvas.renderMode==RenderMode.ScreenSpaceOverlay?null:camera;
            Vector2 min=Vector2.positiveInfinity,max=Vector2.negativeInfinity;
            foreach(var corner in labelCorners){var point=RectTransformUtility.WorldToScreenPoint(projection,corner);min=Vector2.Min(min,point);max=Vector2.Max(max,point);}
            return Rect.MinMaxRect(min.x,min.y,max.x,max.y);
        }
        void Update()
        {
            if(choosing&&!WorldModalState.IsOpen)choosing=false;
            if (Chapter3Main24Flow.Step > 5 || QuestService.GetState(Chapter3Main24Flow.QuestId) == QuestState.Completed)
                Destroy(gameObject);
        }
        public bool Choose()
        {
            var player=Object.FindAnyObjectByType<PlayerController>();
            if(Chapter3Main24Flow.Step != 5 || choosing||WorldModalState.IsOpen||DialoguePresenter.Instance?.IsOpen==true
                ||DialoguePresenter.Instance!=null&&!DialoguePresenter.Instance.CanBeginInteractionThisFrame
                ||player==null||Vector2.Distance(player.transform.position,transform.position)>1.8f)return false;
            choosing=true;
            Main21ChoicePanel.Show("서리올빼미와 전투합니다.\n한기 공격은 전투당 한 번만 시도합니다. 어묵탕은 행동 가능한 동료가 사용합니다.",new[]{"전투 시작","머무른다"},index=>{choosing=false;if(index==0)TryEnter();});return true;
        }
        public bool TryEnter()
        {
            if(Chapter3Main24Flow.Step != 5 || WorldModalState.IsOpen||BattleSceneFlow.IsTransitioning||spawn==null||!Application.CanStreamedLevelBeLoaded(BattleSceneFlow.BattleSceneName))return false;
            var player=Object.FindAnyObjectByType<PlayerController>();if(player==null||Vector2.Distance(player.transform.position,transform.position)>1.8f)return false;
            // 기존 전투 전환의 단일 로드 잠금·복귀 Context·재조우 억제를 그대로 사용합니다.
            if(!MonsterEncounterService.TryRaise(spawn.Monster,spawn))return false;
            return BattleSceneFlow.IsTransitioning&&BattleEncounterContext.Spawn==spawn;
        }
    }
}

using System.Collections;
using ProjectLimitless.Core;
using ProjectLimitless.Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
namespace ProjectLimitless.World
{
    /// <summary>마차 주행은 Arbel의 표시 UI에서만 실행합니다. 안전 체크포인트와 실제 도착을 구분하며 Skip도 동일한 완료 경로를 사용합니다.</summary>
    public sealed class Main21CarriageReturnController : MonoBehaviour
    {
        private static Main21CarriageReturnController active;
        private bool finishing;private InputAction skip;private RectTransform background;private int openedFrame;
        private UnityEngine.UI.Text status;private UnityEngine.UI.Image wagon;private Main21CarriageArtDefinition art;private float elapsed;private Vector2 departure;private string departureSpawn;
        public bool IsFinishing=>finishing;
        public static bool Begin()
        {
            if(active!=null||!Chapter2Main21Flow.Current(9)||SceneManager.GetActiveScene().name!="Arbel")return false;
            var player=FindAnyObjectByType<PlayerController>();var art=Resources.Load<Main21CarriageArtDefinition>("Main21_CarriageArt");
            if(player==null||art==null||art.RampOpen==null||art.RampClosed==null||!Application.CanStreamedLevelBeLoaded("World_StarterVillage"))return false;
            int previous=Main21Progress.Flags;Main21Progress.AcceptReturn();
            // 출발 수락만 기록합니다. 저장 실패면 원래 플래그를 복원하고 이동하지 않습니다.
            if(GameSaveService.CurrentSlotIndex<=0||!GameSaveService.SaveCurrentWorldPosition(player.transform.position,"Arbel",GameSessionData.LastSpawnPointId))
            {Main21Progress.Restore(previous);DialoguePresenterMessage("출발 체크포인트를 저장하지 못했습니다. 저장 슬롯을 확인하고 다시 선택하세요.");return false;}
            var controller=new GameObject("Main21CarriageReturn",typeof(Main21CarriageReturnController)).GetComponent<Main21CarriageReturnController>();
            if(!WorldModalState.TryAcquire(controller)){Destroy(controller.gameObject);return false;}
            active=controller;controller.departure=player.transform.position;controller.departureSpawn=GameSessionData.LastSpawnPointId;
            DontDestroyOnLoad(controller.gameObject);PlayerController.SetMovementLocked(controller,true);controller.openedFrame=Time.frameCount;controller.Build(art);
            return true;
        }
        private static void DialoguePresenterMessage(string text)=>ProjectLimitless.UI.DialoguePresenter.Instance?.Show("귀환 마차",text);
        private void Build(Main21CarriageArtDefinition art)
        {
            this.art=art;
            var canvas=gameObject.AddComponent<Canvas>();canvas.renderMode=RenderMode.ScreenSpaceOverlay;canvas.sortingOrder=400;
            gameObject.AddComponent<UnityEngine.UI.GraphicRaycaster>();var scaler=gameObject.AddComponent<UnityEngine.UI.CanvasScaler>();
            scaler.uiScaleMode=UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;scaler.referenceResolution=new Vector2(1280,720);
            var backdrop=Main21ChoicePanel.Rect(transform,"Backdrop",new Vector2(2560,1440),Vector2.zero);backdrop.gameObject.AddComponent<UnityEngine.UI.Image>().color=new Color(.05f,.10f,.08f);
            background=Main21ChoicePanel.Rect(transform,"SlowBackground",new Vector2(2200,900),Vector2.zero);
            var bg=background.gameObject.AddComponent<UnityEngine.UI.Image>();bg.sprite=art.Background;bg.color=new Color(.55f,.65f,.55f);bg.raycastTarget=false;
            var wagon=Main21ChoicePanel.Rect(transform,"AccessibleWagon",new Vector2(800,430),new Vector2(0,-10));
            var image=wagon.gameObject.AddComponent<UnityEngine.UI.Image>();this.wagon=image;image.sprite=art.RampOpen;image.preserveAspect=true;image.raycastTarget=false;
            status=Main21ChoicePanel.Text(transform,"Caption","아르벨 → 초보 마을\n휠체어2대와 동료 좌석3을 갖춘 귀환 마차\n내부 승객 가림·말 애니메이션 ART_PENDING",new Vector2(1100,160),new Vector2(0,245));
            var button=Main21ChoicePanel.Rect(transform,"Skip",new Vector2(540,66),new Vector2(0,-285));button.gameObject.AddComponent<UnityEngine.UI.Image>().color=new Color(.1f,.18f,.26f);
            var b=button.gameObject.AddComponent<UnityEngine.UI.Button>();Main21ChoicePanel.Text(button,"Text","연출 건너뛰기 [Esc / B]",new Vector2(520,60),Vector2.zero);b.onClick.AddListener(Skip);b.Select();
            skip=new InputAction("Main21CarriageSkip",InputActionType.Button);skip.AddBinding("<Keyboard>/escape");skip.AddBinding("<Gamepad>/buttonEast");skip.performed+=_=>Skip();skip.Enable();
        }
        private void Update()
        {
            if(finishing)return;elapsed+=Time.unscaledDeltaTime;
            // 정지 탑승 상태에서 주행용 닫힌 상태로만 바꿉니다. 미검증 말/승객 애니메이션은 사용하지 않습니다.
            if(elapsed>=1&&wagon!=null)wagon.sprite=art.RampClosed;if(background!=null)background.anchoredPosition=new Vector2(-elapsed*15,0);
            if(elapsed>=12)CompleteReturnOnce();
        }
        public void Skip(){if(Time.frameCount>openedFrame)CompleteReturnOnce();}
        public void CompleteReturnOnce(){if(finishing)return;finishing=true;StartCoroutine(Arrive());}
        private IEnumerator Arrive()
        {
            status.text="초보 마을로 이동 중입니다. 실제 도착 위치와 저장을 확인합니다.";
            if(!SceneTransitionService.TryLoad("World_StarterVillage",Chapter2Main21Flow.ArrivalSpawn))
            {Fail("마을 로드를 시작하지 못했습니다. 출발은 보류됐으며 다시 선택할 수 있습니다.");yield break;}
            float deadline=Time.realtimeSinceStartup+20;
            while(Time.realtimeSinceStartup<deadline)
            {
                if(SceneManager.GetActiveScene().name=="World_StarterVillage")
                {
                    yield return Chapter2Main21Flow.RecoverArrival();
                    if(!Chapter2Main21Flow.Current(9)){Destroy(gameObject);yield break;}
                    Fail("도착 위치 또는 저장 확인이 지연됐습니다. 도착 목표를 유지합니다. 마을의 귀환 확인에서 재시도하세요.");yield break;
                }
                yield return null;
            }
            Fail("마을 로드가 지연됐습니다. 저장된 아르벨 체크포인트에서 귀환을 다시 선택할 수 있습니다.");
        }
        private void Fail(string text)
        {
            // 실패를 성공으로 처리하지 않으며, 자기 입력 잠금만 먼저 풀어 안내 대화를 열 수 있게 합니다.
            if(SceneManager.GetActiveScene().name=="Arbel")
            {GameSessionData.ClearPendingSpawnPoint();GameSessionData.RecordLocation("Arbel",departureSpawn);GameSessionData.RecordWorldPosition(departure.x,departure.y);}
            WorldModalState.Release(this);PlayerController.SetMovementLocked(this,false);gameObject.SetActive(false);Destroy(gameObject);DialoguePresenterMessage(text);
        }
        private void OnDestroy(){skip?.Dispose();WorldModalState.Release(this);PlayerController.SetMovementLocked(this,false);if(active==this)active=null;}
    }
}

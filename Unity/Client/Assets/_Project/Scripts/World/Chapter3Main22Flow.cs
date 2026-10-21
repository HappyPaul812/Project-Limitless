using System.Collections;
using System.Linq;
using ProjectLimitless.Core;
using ProjectLimitless.Player;
using ProjectLimitless.UI;
using ProjectLimitless.Battle;
using ProjectLimitless.Monster;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
namespace ProjectLimitless.World
{
/// <summary>Main21 원본을 바꾸지 않고 북문 선택과 북부 첫 탐험5목표를 연결합니다. Scene마다 하나만 설치합니다.</summary>
public sealed class Chapter3Main22Flow : MonoBehaviour
{
 public const string QuestId="main_22_northwind_signs", Field="World_NorthF1", Village="World_StarterVillage";
 public const string EncounterId="northf1_main22_mouse_encounter", ArrivalSpawn="Spawn_NorthF1_FromStarterVillage", ReturnSpawn="Spawn_StarterVillage_FromNorthF1", ShelterSpawn="Spawn_NorthF1_Shelter";
 public const string EntryTarget="northf1_main22_entry", FrostTarget="northf1_main22_thin_frost", ClueTarget="northf1_main22_southbound_traces", ShelterTarget="northf1_main22_shelter";
 public static int Step => QuestService.ActiveMainQuest?.Definition.QuestId==QuestId ? QuestService.ActiveMainQuest.CurrentObjectiveIndex : -1;
 public static bool Completed=>QuestService.GetState(QuestId)==QuestState.Completed;
 public static bool Unlocked=>Chapter2Main21Flow.Completed;
 public static string Hint=>SceneManager.GetActiveScene().name==Village ? (Completed?"Main22 완료 · 마을에서 자유롭게 정비할 수 있습니다.":Step>=0?"북문을 직접 선택해 F1 조사를 이어가세요.":"마을에서 자유롭게 정비한 뒤 북문 출발을 선택하세요.") : Step==1?"얇은 서리 표시를 조사하세요.":Step==2?$"남하 흔적 {ClueCount}/2 · 서로 다른 두 곳을 조사하세요.":Step==3?"서리들쥐 한 기와 지정전투 · 승리 후 쉼터로 이동":Step==4?"안전 쉼터에서 조사 결과를 정리하세요.":Completed?"Main22 완료 · 남쪽 출구로 마을에 돌아갈 수 있습니다.":"마을 북쪽 출발점에서 북부로 가기를 직접 선택하세요.";
 public static int ClueCount => (Main22Progress.HasClue(0)?1:0)+(Main22Progress.HasClue(1)?1:0);
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)] static void Register(){SceneManager.sceneLoaded-=Install;SceneManager.sceneLoaded+=Install;}
 static void Install(Scene scene,LoadSceneMode mode){if(scene.name!=Village&&scene.name!=Field)return;if(scene.GetRootGameObjects().Any(x=>x.GetComponent<Chapter3Main22Flow>()!=null))return;var root=new GameObject("Chapter3_Main22",typeof(Chapter3Main22Flow));SceneManager.MoveGameObjectToScene(root,scene);}
 IEnumerator Start(){yield return null;
  if(DialoguePresenter.Instance==null)new GameObject("DialogueSystem").AddComponent<DialoguePresenter>();
  if(gameObject.scene.name==Village){InstallVillageGate();CreateSite(Main22Site.Kind.NorthGate,new Vector2(0,5.4f),"북부 출발 · 직접 선택");}
  else{
   CreateSite(Main22Site.Kind.SouthGate,new Vector2(0,-10),"초보 마을로 복귀");
   CreateSite(Main22Site.Kind.Frost,new Vector2(4.6f,-6),"얇은 서리 조사");
   CreateSite(Main22Site.Kind.TraceA,new Vector2(0,-2),"남하 흔적 A · 작은 발자국");
   CreateSite(Main22Site.Kind.TraceB,new Vector2(0,1),"남하 흔적 B · 먹이 흔적");
   CreateSite(Main22Site.Kind.Mouse,new Vector2(0,4),"서리들쥐 · 지정전투");
   CreateSite(Main22Site.Kind.Shelter,new Vector2(0,8),"안전 쉼터 · 결과 정리");
   // 실제 Spawn 배치와 도착 저장 이후에만 첫 목표를 완료합니다. Continue로 들어와도 중복되지 않습니다.
   for(int i=0;i<30&&FindAnyObjectByType<PlayerController>()==null;i++)yield return null;
   if(Step==0&&FindAnyObjectByType<PlayerController>()!=null&&GameSessionData.PendingSpawnPointId==string.Empty){QuestService.NotifyLocationReached(EntryTarget);Save();}
  }
  gameObject.AddComponent<Main22MouseTravel>();gameObject.AddComponent<Main22GuidanceHud>();
 }
 void CreateSite(Main22Site.Kind kind,Vector2 at,string title){var g=new GameObject("Main22_"+kind,typeof(Main22Site));g.transform.SetParent(transform,false);g.transform.position=at;g.GetComponent<Main22Site>().Configure(kind,title);}
 bool northOpened;
 void InstallVillageGate(){
  // 마을 Scene/기존 남문·NPC를 재생성하지 않습니다. 북쪽 기존 벽 하나만 출구 양옆으로 나눕니다.
  if(transform.Find(ReturnSpawn)==null){var spawn=new GameObject(ReturnSpawn,typeof(SceneSpawnPoint));spawn.transform.SetParent(transform,false);spawn.transform.position=new Vector2(0,4.8f);spawn.GetComponent<SceneSpawnPoint>().Configure(ReturnSpawn);}
  if(!Unlocked)return;
  northOpened=true;
  var top=FindObjectsByType<BoxCollider2D>(FindObjectsSortMode.None).FirstOrDefault(x=>x.name=="Boundary_Top_0"&&x.bounds.size.x>18);
  if(top!=null){var center=top.bounds.center;var height=top.size.y;top.enabled=false;Box("NorthBoundaryLeft",new Vector2(-5.5f,center.y),new Vector2(8,height));Box("NorthBoundaryRight",new Vector2(5.5f,center.y),new Vector2(8,height));}
  Box("NorthGateGuideLeft",new Vector2(-1.7f,6.4f),new Vector2(.4f,1));Box("NorthGateGuideRight",new Vector2(1.7f,6.4f),new Vector2(.4f,1));Box("NorthGateManualStop",new Vector2(0,6.9f),new Vector2(3.4f,.4f));
 }
 void Box(string name,Vector2 pos,Vector2 size){var g=new GameObject(name,typeof(BoxCollider2D));g.transform.SetParent(transform,false);g.transform.position=pos;g.GetComponent<BoxCollider2D>().size=size;}
 void Update(){if(gameObject.scene.name==Village&&Unlocked&&!northOpened){InstallVillageGate();}if(!Unlocked||WorldModalState.IsOpen)return;bool pressed=Keyboard.current?.eKey.wasPressedThisFrame==true||Keyboard.current?.fKey.wasPressedThisFrame==true||Gamepad.current?.buttonSouth.wasPressedThisFrame==true;if(!pressed)return;NearestInRange()?.TryInteract();}
 public static Main22Site CurrentSite(){var sites=FindObjectsByType<Main22Site>(FindObjectsSortMode.None);if(SceneManager.GetActiveScene().name==Village)return sites.FirstOrDefault(x=>x.SiteKind==Main22Site.Kind.NorthGate);var kind=Completed?Main22Site.Kind.SouthGate:Step==1?Main22Site.Kind.Frost:Step==2?(!Main22Progress.HasClue(0)?Main22Site.Kind.TraceA:Main22Site.Kind.TraceB):Step==3?Main22Site.Kind.Mouse:Step==4?Main22Site.Kind.Shelter:Main22Site.Kind.SouthGate;return sites.FirstOrDefault(x=>x.SiteKind==kind);}
 public static Main22Site NearestInRange(){var p=FindAnyObjectByType<PlayerController>();return p==null?null:FindObjectsByType<Main22Site>(FindObjectsSortMode.None).Where(x=>x.Available&&Vector2.Distance(p.transform.position,x.transform.position)<=Main22Site.Range).OrderBy(x=>Vector2.Distance(p.transform.position,x.transform.position)).FirstOrDefault();}
 public static void Save(){if(GameSaveService.CurrentSlotIndex>0)GameSaveService.SaveCurrentSession();}
}

/// <summary>현재 목표/거리/모달 조건이 맞을 때만 조사·선택을 실행합니다. 마우스와 키보드가 같은 경로를 사용합니다.</summary>
public sealed class Main22Site : MonoBehaviour
{
 public enum Kind{NorthGate,SouthGate,Frost,TraceA,TraceB,Mouse,Shelter}
 public Kind SiteKind{get;private set;}public string Title{get;private set;}public const float Range=1.25f;
 TextMesh label;LineRenderer ring;GameObject mouseVisual;bool busy;Material ringMaterial;
 public bool Available=>Chapter3Main22Flow.Unlocked&&(SiteKind==Kind.NorthGate||SiteKind==Kind.SouthGate||SiteKind==Kind.Frost&&Chapter3Main22Flow.Step==1||SiteKind==Kind.TraceA&&Chapter3Main22Flow.Step==2&&!Main22Progress.HasClue(0)||SiteKind==Kind.TraceB&&Chapter3Main22Flow.Step==2&&!Main22Progress.HasClue(1)||SiteKind==Kind.Mouse&&Chapter3Main22Flow.Step==3||SiteKind==Kind.Shelter&&Chapter3Main22Flow.Step==4);
 public void Configure(Kind kind,string title){SiteKind=kind;Title=title;}
 void Start(){
  label=new GameObject("Label",typeof(TextMesh)).GetComponent<TextMesh>();label.transform.SetParent(transform,false);label.transform.localPosition=Vector3.up*.85f;label.characterSize=.085f;label.fontSize=36;label.anchor=TextAnchor.MiddleCenter;label.font=Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");label.GetComponent<MeshRenderer>().sharedMaterial=label.font.material;label.GetComponent<MeshRenderer>().sortingOrder=20;
  ring=new GameObject("InteractionRing",typeof(LineRenderer)).GetComponent<LineRenderer>();ring.transform.SetParent(transform,false);ring.useWorldSpace=false;ring.loop=true;ring.positionCount=32;ring.startWidth=ring.endWidth=.035f;ring.sortingOrder=2;ringMaterial=new Material(Shader.Find("Sprites/Default"));ring.sharedMaterial=ringMaterial;
  for(int i=0;i<32;i++){float angle=i*Mathf.PI*2/32;ring.SetPosition(i,new Vector3(Mathf.Cos(angle)*.65f,Mathf.Sin(angle)*.38f,0));}
  if(SiteKind==Kind.Mouse){mouseVisual=new GameObject("ApprovedFrostFieldMouse",typeof(SpriteRenderer),typeof(MonsterSpriteSheetAnimation));mouseVisual.transform.SetParent(transform,false);var def=Resources.Load<FieldMonsterSpawnDefinition>("StoryEncounterReturns/Main22_Encounter").Monster;var sr=mouseVisual.GetComponent<SpriteRenderer>();sr.sprite=def.FieldSprite;sr.sortingOrder=3;mouseVisual.GetComponent<MonsterSpriteSheetAnimation>().Configure(sr,def);}
 }
 void Update(){if(busy&&!WorldModalState.IsOpen)busy=false;bool visible=Available&&!WorldModalState.IsOpen;label.gameObject.SetActive(visible);ring.gameObject.SetActive(visible);label.text=Title+"\n[클릭 / E·F / A]";if(mouseVisual!=null)mouseVisual.SetActive(Chapter3Main22Flow.Unlocked&&Chapter3Main22Flow.Step>=0&&Chapter3Main22Flow.Step<=3&&!Chapter3Main22Flow.Completed);}
 public bool TryInteract(){var p=FindAnyObjectByType<PlayerController>();var d=DialoguePresenter.Instance;if(!Available||busy||WorldModalState.IsOpen||p==null||Vector2.Distance(p.transform.position,transform.position)>Range||d==null||!d.CanBeginInteractionThisFrame)return false;
  if(SiteKind==Kind.NorthGate||SiteKind==Kind.SouthGate){bool north=SiteKind==Kind.NorthGate;Main21ChoicePanel.Show(north?"북부로 출발하시겠습니까?":"초보 마을로 돌아가시겠습니까?",new[]{north?"북부 F1로 출발":"초보 마을로 복귀","아직 머무른다"},choice=>{if(choice!=0)return;if(north&&QuestService.GetState(Chapter3Main22Flow.QuestId)==QuestState.Available)QuestService.TryStart(Chapter3Main22Flow.QuestId);Chapter3Main22Flow.Save();SceneTransitionService.TryLoad(north?Chapter3Main22Flow.Field:Chapter3Main22Flow.Village,north?Chapter3Main22Flow.ArrivalSpawn:Chapter3Main22Flow.ReturnSpawn);});return true;}
  if(SiteKind==Kind.Mouse){var spawn=Resources.Load<FieldMonsterSpawnDefinition>("StoryEncounterReturns/Main22_Encounter");return spawn!=null&&BattleSceneFlow.EnterStoryBattle(Chapter3Main22Flow.EncounterId,spawn.Monster,spawn,transform.position);}
  busy=true;string text=SiteKind==Kind.Frost?"풀과 흙 사이에 얇은 서리가 남아 있습니다. 지면 전체가 얼어붙은 상태는 아닙니다.":SiteKind==Kind.TraceA?"작은 발자국들이 북쪽에서 남쪽으로 이어집니다.":SiteKind==Kind.TraceB?"먹이를 남긴 채 남쪽으로 이동한 흔적입니다. 두 단서가 같은 방향을 가리킵니다.":"동물들이 북쪽을 피해 남하하고 있습니다. 귀환 길을 확보했고, 안쪽 상황은 아직 확인되지 않았습니다.";
  d.ShowSequence("조사 · 임시 대사",new[]{text},()=>{busy=false;if(SiteKind==Kind.Frost)QuestService.NotifyInteraction(Chapter3Main22Flow.FrostTarget);else if(SiteKind==Kind.TraceA||SiteKind==Kind.TraceB){if(Main22Progress.Observe(SiteKind==Kind.TraceA?0:1))QuestService.NotifyInteraction(Chapter3Main22Flow.ClueTarget);}else{QuestService.NotifyInteraction(Chapter3Main22Flow.ShelterTarget);GameSessionData.ActivateSafeZone("safezone_northf1_shelter",Chapter3Main22Flow.Field,Chapter3Main22Flow.ShelterSpawn);}Chapter3Main22Flow.Save();});return true;
 }
 void OnDestroy(){if(ringMaterial!=null)Destroy(ringMaterial);}
}
}

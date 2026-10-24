using System.Collections;
using System.Linq;
using ProjectLimitless.Core;
using ProjectLimitless.Player;
using ProjectLimitless.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace ProjectLimitless.World
{
/// <summary>Main22의 남부 탐험을 보존하면서 심부 조사와 서릿길 안전거점을 연결합니다. 대사는 임시 기획 텍스트입니다.</summary>
public sealed class Chapter3Main23Flow : MonoBehaviour
{
 Material placeholderMaterial;
 public const string QuestId="main_23_southbound_traces", Village="World_FrostwayVillage", Field="World_NorthF1";
 public const string ArrivalSpawn="Spawn_FrostwayVillage_FromNorthF1", ReturnSpawn="Spawn_NorthF1_FromFrostwayVillage", SafeSpawn="Spawn_FrostwayVillage_Checkpoint";
 public const string Deep="northf1_main23_deep", Traces="northf1_main23_traces", Arrival="frostway_main23_arrival", Guide="frostway-guide", Checkpoint="frostway_main23_checkpoint";
 public static int Step=>QuestService.ActiveMainQuest?.Definition.QuestId==QuestId?QuestService.ActiveMainQuest.CurrentObjectiveIndex:-1;
 public static bool Completed=>QuestService.GetState(QuestId)==QuestState.Completed;
 public static string Hint=>Step==0?"심부의 길을 따라 북쪽으로 이동하세요.":Step==1?$"동물·짐수레 흔적 {ClueCount}/2 · 서로 다른 두 곳을 조사하세요.":Step==2?"북쪽 출구를 직접 선택해 서릿길 마을로 이동하세요.":Step==3?"주민 안내 표식에서 증언을 끝까지 들으세요.":Step==4?"중앙 안전거점을 직접 등록하세요.":Completed?"Main23 완료 · 북문 F2 미개통 · 남문 F1 왕복 가능":"쉼터에서 Main23 시작을 직접 선택하세요.";
 public static int ClueCount=>(Main23Progress.HasClue(0)?1:0)+(Main23Progress.HasClue(1)?1:0);
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)] static void Register(){SceneManager.sceneLoaded-=Install;SceneManager.sceneLoaded+=Install;}
 static void Install(Scene scene,LoadSceneMode mode){if(scene.name!=Field&&scene.name!=Village)return;var root=new GameObject("Chapter3_Main23",typeof(Chapter3Main23Flow));SceneManager.MoveGameObjectToScene(root,scene);}
 IEnumerator Start(){yield return null;
  if(DialoguePresenter.Instance==null)new GameObject("DialogueSystem").AddComponent<DialoguePresenter>();
  if(gameObject.scene.name==Field){Site(Main23Site.Kind.Start,new Vector2(0,8),"Main23 · 조사 시작");Site(Main23Site.Kind.TraceA,new Vector2(-2,15),"동물의 남하 흔적");Site(Main23Site.Kind.TraceB,new Vector2(2,18),"장작·짐수레 흔적");Site(Main23Site.Kind.NorthGate,new Vector2(0,22),"서릿길 마을 남문으로");Site(Main23Site.Kind.SouthGate,new Vector2(0,-10),"초보 마을로 복귀");}
  else{Site(Main23Site.Kind.SouthGate,new Vector2(0,-10),"남문 · F1로 복귀");Site(Main23Site.Kind.Guide,new Vector2(-3,-4),"주민 안내 · 증언 / 임시 외형");Site(Main23Site.Kind.Checkpoint,Vector2.zero,"중앙 안전거점 등록");Site(Main23Site.Kind.Rest,new Vector2(-6,2),"여관 · 휴식 / 임시 표식");Site(Main23Site.Kind.Party,new Vector2(6,2),"파티 관리 / 임시 표식");Site(Main23Site.Kind.LockedNorth,new Vector2(0,9),"북문 · F2 미개통");Site(Main23Site.Kind.Skill,new Vector2(6,-4),"스킬 판매 예정 · 구매 불가");}
  // 미승인 건물의 충돌 영역을 선과 글자로 함께 표시합니다. 보이지 않는 장애물로 남기지 않습니다.
  if(gameObject.scene.name==Village){placeholderMaterial=new Material(Shader.Find("Sprites/Default"));foreach(var box in FindObjectsByType<BoxCollider2D>(FindObjectsSortMode.None).Where(x=>x.name.StartsWith("VISUAL_PLACEHOLDER"))){var line=box.gameObject.AddComponent<LineRenderer>();line.useWorldSpace=false;line.loop=true;line.positionCount=4;line.startWidth=line.endWidth=.08f;line.sharedMaterial=placeholderMaterial;line.startColor=line.endColor=Color.yellow;line.sortingOrder=1;var h=box.size*.5f;line.SetPositions(new[]{new Vector3(-h.x,-h.y),new Vector3(-h.x,h.y),new Vector3(h.x,h.y),new Vector3(h.x,-h.y)});}}
  for(int i=0;i<40&&FindAnyObjectByType<PlayerController>()==null;i++)yield return null;
  if(Chapter3Main22Flow.Completed&&GetComponent<Main23MouseTravel>()==null){gameObject.AddComponent<Main23MouseTravel>();gameObject.AddComponent<Main23GuidanceHud>();}
 }
 void Site(Main23Site.Kind kind,Vector2 at,string title){var g=new GameObject("Main23_"+kind,typeof(Main23Site));g.transform.SetParent(transform,false);g.transform.position=at;g.GetComponent<Main23Site>().Configure(kind,title);}
 void Update(){if(!Chapter3Main22Flow.Completed)return;if(GetComponent<Main23MouseTravel>()==null){gameObject.AddComponent<Main23MouseTravel>();gameObject.AddComponent<Main23GuidanceHud>();}var p=FindAnyObjectByType<PlayerController>();if(p==null||WorldModalState.IsOpen)return;
  // 도착 목표는 실제 Player와 Spawn 처리가 끝난 뒤만 인정합니다. 역순 방문에서 뒤 단계를 미리 채우지 않습니다.
  if(GameSessionData.PendingSpawnPointId==string.Empty){if(Step==0&&gameObject.scene.name==Field&&p.transform.position.y>=13){QuestService.NotifyLocationReached(Deep);Save();}else if(Step==2&&gameObject.scene.name==Village){QuestService.NotifyLocationReached(Arrival);Save();}}
  if(Keyboard.current?.eKey.wasPressedThisFrame==true||Keyboard.current?.fKey.wasPressedThisFrame==true||Gamepad.current?.buttonSouth.wasPressedThisFrame==true)NearestInRange()?.TryInteract();
 }
 public static Main23Site CurrentSite(){var kind=SceneManager.GetActiveScene().name==Village?(Step==3?Main23Site.Kind.Guide:Step==4?Main23Site.Kind.Checkpoint:Main23Site.Kind.SouthGate):Step==0?Main23Site.Kind.TraceA:Step==1?(!Main23Progress.HasClue(0)?Main23Site.Kind.TraceA:Main23Site.Kind.TraceB):Step>=2||Completed?Main23Site.Kind.NorthGate:Main23Site.Kind.Start;return FindObjectsByType<Main23Site>(FindObjectsSortMode.None).FirstOrDefault(s=>s.SiteKind==kind);}
 public static Main23Site NearestInRange(){var p=FindAnyObjectByType<PlayerController>();return p==null?null:FindObjectsByType<Main23Site>(FindObjectsSortMode.None).Where(s=>s.Available&&Vector2.Distance(p.transform.position,s.transform.position)<=Main23Site.Range).OrderBy(s=>Vector2.Distance(p.transform.position,s.transform.position)).FirstOrDefault();}
 public static bool Save()=>GameSaveService.CurrentSlotIndex>0&&GameSaveService.SaveCurrentSession();
 void OnDestroy(){if(placeholderMaterial!=null)Destroy(placeholderMaterial);}
}
/// <summary>선택한 표식 하나만 실행하며 대화를 끝낸 콜백에서 목표를 통지합니다. 취소는 Quest를 바꾸지 않습니다.</summary>
public sealed class Main23Site:MonoBehaviour
{
 public enum Kind{Start,TraceA,TraceB,NorthGate,SouthGate,Guide,Checkpoint,Rest,Party,LockedNorth,Skill}
 public Kind SiteKind{get;private set;}public string Title{get;private set;}public const float Range=1.25f;
 UnityEngine.UI.Text label; Canvas labelCanvas; bool busy;
 public bool Available=>Chapter3Main22Flow.Completed&&(SiteKind!=Kind.Start||!Chapter3Main23Flow.Completed)&&(SiteKind!=Kind.TraceA&&SiteKind!=Kind.TraceB||Chapter3Main23Flow.Step==1&&!Main23Progress.HasClue(SiteKind==Kind.TraceA?0:1));
 public void Configure(Kind kind,string title){SiteKind=kind;Title=title;}
 // 어두운 바탕의 월드 안내판을 발 위치 위에 둡니다. 이동/Quest 판정 위치는 그대로 유지합니다.
 void Start(){
  var panel=new GameObject("Main23_ReadableSite",typeof(RectTransform),typeof(Canvas),typeof(UnityEngine.UI.Image));
  panel.transform.SetParent(transform,false);panel.transform.localPosition=Vector3.up*2.2f;panel.transform.localScale=Vector3.one*.01f;
  var rect=panel.GetComponent<RectTransform>();rect.sizeDelta=new Vector2(310,58);
  labelCanvas=panel.GetComponent<Canvas>();labelCanvas.renderMode=RenderMode.WorldSpace;labelCanvas.overrideSorting=true;labelCanvas.sortingOrder=6000;
  panel.GetComponent<UnityEngine.UI.Image>().color=new Color(.025f,.04f,.06f,.94f);panel.GetComponent<UnityEngine.UI.Image>().raycastTarget=false;
  label=new GameObject("Text",typeof(RectTransform),typeof(UnityEngine.UI.Text)).GetComponent<UnityEngine.UI.Text>();label.transform.SetParent(panel.transform,false);
  var textRect=label.GetComponent<RectTransform>();textRect.anchorMin=Vector2.zero;textRect.anchorMax=Vector2.one;textRect.offsetMin=new Vector2(8,2);textRect.offsetMax=new Vector2(-8,-2);
  label.font=Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");label.fontSize=19;label.alignment=TextAnchor.MiddleCenter;label.color=Color.white;label.raycastTarget=false;
 }
 public string DisplayTitle=>SiteKind==Kind.TraceA&&Chapter3Main23Flow.Step==0?"① F1 심부로 이동":SiteKind==Kind.TraceA?"② 동물 흔적 조사":SiteKind==Kind.TraceB?"② 짐수레·장작 조사":SiteKind==Kind.Guide?"④ 주민 증언 · 임시 외형":SiteKind==Kind.Checkpoint?(Chapter3Main23Flow.Completed?"안전거점 · 저장":"⑤ 중앙 안전거점 등록"):SiteKind==Kind.NorthGate?"③ 서릿길 마을로":SiteKind==Kind.Start?"Main23 · 조사 시작":SiteKind==Kind.Rest?"여관 · 휴식 / 임시 표식":SiteKind==Kind.Party?"파티 관리 / 임시 표식":SiteKind==Kind.Skill?"스킬 판매 예정 · 구매 불가":Title;
 // 글자나 안내판을 클릭해도 같은 표식 소유자 하나에만 이동·상호작용을 전달합니다.
 public bool ContainsWorldPoint(Vector2 point){if(Vector2.Distance(transform.position,point)<.8f)return true;if(labelCanvas==null||!labelCanvas.gameObject.activeInHierarchy)return false;var rect=labelCanvas.GetComponent<RectTransform>();return rect.rect.Contains(rect.InverseTransformPoint(new Vector3(point.x,point.y,transform.position.z)));}
  void Update(){if(busy&&!WorldModalState.IsOpen)busy=false;var player=FindAnyObjectByType<PlayerController>();bool target=Chapter3Main23Flow.CurrentSite()==this;bool near=player!=null&&Vector2.Distance(player.transform.position,transform.position)<4;labelCanvas.gameObject.SetActive(Available&&!WorldModalState.IsOpen&&(target||near));label.text=(target?"▶ ":"")+DisplayTitle+"\n클릭 / E·F · 취소: Esc";}
 public bool TryInteract(){var p=FindAnyObjectByType<PlayerController>();var d=DialoguePresenter.Instance;if(!Available||busy||WorldModalState.IsOpen||p==null||d==null||!d.CanBeginInteractionThisFrame||Vector2.Distance(p.transform.position,transform.position)>Range)return false;
  if(SiteKind==Kind.Start){Main21ChoicePanel.Show("Main23 · 따뜻한 땅을 떠난 자들",new[]{"심부 조사 시작 / 이어가기","아직 머무른다"},choice=>{if(choice==0&&QuestService.GetState(Chapter3Main23Flow.QuestId)==QuestState.Available){QuestService.TryStart(Chapter3Main23Flow.QuestId);Chapter3Main23Flow.Save();}});return true;}
  if(SiteKind==Kind.NorthGate||SiteKind==Kind.SouthGate){bool toVillage=SiteKind==Kind.NorthGate;bool starter=gameObject.scene.name==Chapter3Main23Flow.Field&&!toVillage;Main21ChoicePanel.Show("이동하시겠습니까?",new[]{toVillage?"서릿길 마을로":starter?"초보 마을로":"F1로 돌아가기","머무른다"},choice=>{if(choice!=0)return;Chapter3Main23Flow.Save();SceneTransitionService.TryLoad(toVillage?Chapter3Main23Flow.Village:starter?Chapter3Main22Flow.Village:Chapter3Main23Flow.Field,toVillage?Chapter3Main23Flow.ArrivalSpawn:starter?Chapter3Main22Flow.ReturnSpawn:Chapter3Main23Flow.ReturnSpawn);});return true;}
  if(SiteKind==Kind.Checkpoint){if(Chapter3Main23Flow.Step!=4&&!Chapter3Main23Flow.Completed){d.ShowSequence("안전거점",new[]{"주민의 증언을 먼저 확인한 뒤 등록하세요."},null);return true;}Main21ChoicePanel.Show("안전거점을 등록하고 저장하시겠습니까?",new[]{"등록하고 저장","취소"},choice=>{if(choice!=0)return;
   // 실제 Scene 안의 안전 Spawn이 비충돌인 경우에만 패배 복귀 위치를 등록합니다.
   var spawn=FindObjectsByType<SceneSpawnPoint>(FindObjectsSortMode.None).FirstOrDefault(x=>x.name==Chapter3Main23Flow.SafeSpawn);if(spawn==null||Physics2D.OverlapCircleAll(spawn.transform.position,.55f).Any(c=>!c.isTrigger&&!c.transform.IsChildOf(p.transform))){d.ShowSequence("등록 실패",new[]{"안전한 복귀 지점을 확인할 수 없습니다."},null);return;}
   GameSessionData.ActivateSafeZone("safezone_frostway_village",Chapter3Main23Flow.Village,Chapter3Main23Flow.SafeSpawn);QuestService.NotifyInteraction(Chapter3Main23Flow.Checkpoint);bool saved=Chapter3Main23Flow.Save();d.ShowSequence("안전거점",new[]{saved?"안전거점 등록과 저장을 완료했습니다.":"저장에 실패했습니다. 거점에서 다시 등록해 저장을 재시도하세요."},null);
  });return true;}
  if(SiteKind==Kind.Party){PartyManagementPresenter.OpenAt(transform);return true;}
  if(SiteKind==Kind.Rest){Main21ChoicePanel.Show("파티 HP/MP를 회복하시겠습니까?",new[]{"휴식","취소"},choice=>{if(choice==0){PartyResourceService.HealPartyFully();Chapter3Main23Flow.Save();}});return true;}
  string[] lines=SiteKind==Kind.Guide?new[]{"[임시 텍스트 · 주민] 요 며칠 남쪽으로 가는 짐승이 늘었어요. 지붕의 서리가 낮에도 잘 안 녹아요.","세린: 오는 길의 발자국도 같은 방향이었어요.","주민: 어느 길이 안전했죠? 하렌: 그건 내가…", "태온: 형. 내가 본 거니까 내가 얘기할게. 길목보다 왼쪽 둔덕이 덜 얼었어.","주민: 그 길을 마을 사람들에게도 먼저 알려야겠네요.","폴: 이쪽 흙은 단단하네요. 모두 지나갈 수 있겠습니다. 미엘: 잠깐 쉬며 손도 확인해요."}:SiteKind==Kind.TraceA?new[]{"세린: 먹이까지 남겨 두고 남쪽으로 내려갔어요.","태온: 돌아올 길도 먼저 확인했어, 형."}:SiteKind==Kind.TraceB?new[]{"사람이 옮긴 장작과 짐수레 바퀴 자국이 남쪽으로 이어집니다.","폴: 단단한 길을 따라가면 모두 지나갈 수 있겠습니다."}:new[]{SiteKind==Kind.LockedNorth?"북문 F2는 아직 열리지 않았습니다.":"스킬 판매는 준비 중입니다. 구매 기능은 없습니다."};
  busy=true;d.ShowSequence("Main23 · 임시 대사",lines,()=>{busy=false;if(SiteKind==Kind.Guide)QuestService.NotifyNpcTalked(Chapter3Main23Flow.Guide);else if(SiteKind==Kind.TraceA||SiteKind==Kind.TraceB){if(Chapter3Main23Flow.Step==1&&Main23Progress.Observe(SiteKind==Kind.TraceA?0:1))QuestService.NotifyInteraction(Chapter3Main23Flow.Traces);}Chapter3Main23Flow.Save();});return true;
 }
}
}

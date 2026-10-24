using System.Collections.Generic;
using System.Linq;
using ProjectLimitless.Core;
using ProjectLimitless.Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
namespace ProjectLimitless.World
{
/// <summary>Main23의 마우스 단일 클릭 이동입니다. 실제 Collider로 작은 격자 경로를 찾고 기존 PlayerController에 방향만 전달합니다.</summary>
public sealed class Main23MouseTravel : MonoBehaviour
{
 readonly Queue<Vector2> route=new Queue<Vector2>();PlayerController player;Main23Site inspectAfterArrival;float lastDistance;double stuckSince;
 const float Cell=.5f;
 public bool IsTravelling=>route.Count>0;
 void Start(){player=FindAnyObjectByType<PlayerController>();}
 public bool GoTo(Main23Site site){if(site==null)return false;bool found=GoToPoint(site.transform.position);inspectAfterArrival=null;return found;}
 public bool GoToPoint(Vector2 goal){Cancel();if(!Chapter3Main22Flow.Completed||player==null||WorldModalState.IsOpen)return false;var bounds=FindAnyObjectByType<WorldBounds2D>();if(bounds==null||!bounds.Bounds.Contains(goal))return false;
  var start=Vector2Int.RoundToInt((Vector2)player.transform.position/Cell);var open=new Queue<Vector2Int>();var came=new Dictionary<Vector2Int,Vector2Int>();open.Enqueue(start);came[start]=start;Vector2Int end=start;bool reached=false;
  while(open.Count>0&&came.Count<7000){var at=open.Dequeue();if(Vector2.Distance((Vector2)at*Cell,goal)<.8f){end=at;reached=true;break;}
   foreach(var delta in new[]{Vector2Int.up,Vector2Int.down,Vector2Int.left,Vector2Int.right}){var next=at+delta;Vector2 world=(Vector2)next*Cell;if(came.ContainsKey(next)||!bounds.Bounds.Contains(world)||!Clear(world))continue;came[next]=at;open.Enqueue(next);}}
  if(!reached)return false;var reverse=new List<Vector2>();while(end!=start){reverse.Add((Vector2)end*Cell);end=came[end];}reverse.Reverse();foreach(var p in reverse)route.Enqueue(p);lastDistance=float.PositiveInfinity;stuckSince=Time.realtimeSinceStartupAsDouble;return true;
 }
 bool Clear(Vector2 at)=>Physics2D.OverlapCircleAll(at,.52f).All(c=>c.isTrigger||c.transform.IsChildOf(player.transform)||c.attachedRigidbody!=null&&c.attachedRigidbody.bodyType!=RigidbodyType2D.Static);
 public void Cancel(){route.Clear();inspectAfterArrival=null;if(player!=null)player.SetGuidedMovement(Vector2.zero);}
 void Update(){
  if(player==null)player=FindAnyObjectByType<PlayerController>();if(player==null)return;
  if(!Chapter3Main22Flow.Completed){Cancel();return;}
  if(WorldModalState.IsOpen||player.HasManualMovement||Keyboard.current?.escapeKey.wasPressedThisFrame==true||Mouse.current?.rightButton.wasPressedThisFrame==true){Cancel();return;}
  if(Mouse.current?.leftButton.wasPressedThisFrame==true&&Camera.main!=null&&!PointerOverUi()){
   Vector2 at=Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());var site=FindObjectsByType<Main23Site>(FindObjectsSortMode.None).Where(s=>s.Available&&s.ContainsWorldPoint(at)).OrderBy(s=>Vector2.Distance(s.transform.position,at)).FirstOrDefault();
   if(site!=null){if(!site.TryInteract()&&GoToPoint(site.transform.position))inspectAfterArrival=site;}else GoToPoint(at);
  }
  if(route.Count==0){player.SetGuidedMovement(Vector2.zero);return;}
  Vector2 diff=route.Peek()-(Vector2)player.transform.position;if(diff.magnitude<.12f){route.Dequeue();lastDistance=float.PositiveInfinity;stuckSince=Time.realtimeSinceStartupAsDouble;if(route.Count==0){player.SetGuidedMovement(Vector2.zero);var target=inspectAfterArrival;inspectAfterArrival=null;target?.TryInteract();}return;}
  if(diff.magnitude<lastDistance-.01f){lastDistance=diff.magnitude;stuckSince=Time.realtimeSinceStartupAsDouble;}else if(Time.realtimeSinceStartupAsDouble-stuckSince>1.5){Cancel();return;}
  player.SetGuidedMovement(diff.normalized);
 }
 bool PointerOverUi(){if(EventSystem.current==null)return false;var result=new List<RaycastResult>();EventSystem.current.RaycastAll(new PointerEventData(EventSystem.current){position=Mouse.current.position.ReadValue()},result);return result.Count>0;}
 void OnDisable()=>Cancel();
}

/// <summary>현재 목표·두 단서 카운트·방향과 거리·마우스 조작을 상시 제공합니다. 기존 Toast나 퀘스트창은 교체하지 않습니다.</summary>
public sealed class Main23GuidanceHud : MonoBehaviour
{
 UnityEngine.UI.Text text,actionText;UnityEngine.UI.Button action;CanvasGroup group;Main23MouseTravel travel;
 public string CurrentText=>text!=null?text.text:string.Empty;
 void Start(){
  travel=GetComponent<Main23MouseTravel>();var root=new GameObject("Main23Guidance",typeof(RectTransform),typeof(Canvas),typeof(UnityEngine.UI.CanvasScaler),typeof(UnityEngine.UI.GraphicRaycaster),typeof(CanvasGroup));root.transform.SetParent(transform,false);var canvas=root.GetComponent<Canvas>();canvas.renderMode=RenderMode.ScreenSpaceOverlay;canvas.sortingOrder=80;group=root.GetComponent<CanvasGroup>();var scaler=root.GetComponent<UnityEngine.UI.CanvasScaler>();scaler.uiScaleMode=UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;scaler.referenceResolution=new Vector2(1280,720);scaler.matchWidthOrHeight=1;
  if(EventSystem.current==null){var es=new GameObject("Main23EventSystem",typeof(EventSystem),typeof(InputSystemUIInputModule));es.GetComponent<InputSystemUIInputModule>().AssignDefaultActions();}
  var panel=Rect(root.transform,"Panel",new Vector2(460,248),new Vector2(18,18));panel.anchorMin=panel.anchorMax=Vector2.zero;panel.pivot=Vector2.zero;panel.gameObject.AddComponent<UnityEngine.UI.Image>().color=new Color(.035f,.055f,.08f,.96f);
  text=Text(panel,"CurrentGoal",new Vector2(428,144),new Vector2(16,98),17);text.alignment=TextAnchor.UpperLeft;
  Button(panel,"Navigate","목표로 이동",new Vector2(205,42),new Vector2(16,50),()=>travel.GoTo(Chapter3Main23Flow.CurrentSite()));
  action=Button(panel,"Interact","조사 / 선택",new Vector2(205,42),new Vector2(238,50),()=>Chapter3Main23Flow.NearestInRange()?.TryInteract());actionText=action.GetComponentInChildren<UnityEngine.UI.Text>();
  Button(panel,"CancelTravel","이동 취소",new Vector2(205,34),new Vector2(238,8),()=>travel.Cancel());
  Button(panel,"ReturnRoute","마을 출구로 이동",new Vector2(205,34),new Vector2(16,8),()=>travel.GoTo(FindObjectsByType<Main23Site>(FindObjectsSortMode.None).FirstOrDefault(s=>s.SiteKind==Main23Site.Kind.SouthGate||s.SiteKind==Main23Site.Kind.Start)));
 }
 void Update(){if(group==null)return;bool show=Chapter3Main22Flow.Completed&&!WorldModalState.IsOpen;group.alpha=show?1:0;group.blocksRaycasts=group.interactable=show;if(!show)return;var target=Chapter3Main23Flow.CurrentSite();var p=FindAnyObjectByType<PlayerController>();string direction="";if(target!=null&&p!=null){var diff=target.transform.position-p.transform.position;direction=$"{(diff.y>.5f?"북":diff.y<-.5f?"남":"")}{(diff.x>.5f?"동":diff.x<-.5f?"서":"")}쪽 · {diff.magnitude:0.0}m → {target.DisplayTitle}";}
  text.text=$"Main23 · 따뜻한 땅을 떠난 자들 {(Chapter3Main23Flow.Completed?"완료":$"{Mathf.Max(0,Chapter3Main23Flow.Step+1)}/5")}\n{Chapter3Main23Flow.Hint}\n{direction}\n표식 클릭: 이동·선택 / E·F: 상호작용\nEsc: 이동·창 취소 / 우클릭: 이동 취소";
  action.interactable=Chapter3Main23Flow.NearestInRange()!=null;actionText.text=action.interactable?"가까운 표시 조사 / 선택":"표시 가까이 이동하세요";
 }
 static RectTransform Rect(Transform parent,string name,Vector2 size,Vector2 pos){var t=new GameObject(name,typeof(RectTransform)).GetComponent<RectTransform>();t.SetParent(parent,false);t.anchorMin=t.anchorMax=Vector2.zero;t.pivot=Vector2.zero;t.sizeDelta=size;t.anchoredPosition=pos;return t;}
 static UnityEngine.UI.Text Text(Transform parent,string name,Vector2 size,Vector2 pos,int font){var t=Rect(parent,name,size,pos).gameObject.AddComponent<UnityEngine.UI.Text>();t.font=Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");t.fontSize=font;t.color=Color.white;t.raycastTarget=false;t.alignment=TextAnchor.MiddleCenter;return t;}
 static UnityEngine.UI.Button Button(Transform parent,string name,string label,Vector2 size,Vector2 pos,UnityEngine.Events.UnityAction click){var t=Rect(parent,name,size,pos);t.gameObject.AddComponent<UnityEngine.UI.Image>().color=new Color(.15f,.25f,.35f);var b=t.gameObject.AddComponent<UnityEngine.UI.Button>();Text(t,"Text",size,Vector2.zero,16).text=label;b.onClick.AddListener(click);return b;}
}
}

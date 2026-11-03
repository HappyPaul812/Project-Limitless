using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ProjectLimitless.Core;
using ProjectLimitless.Player;
using ProjectLimitless.World;
using ProjectLimitless.Battle;
using ProjectLimitless.NPC;
using Object=UnityEngine.Object;
namespace ProjectLimitless.EditorTools
{
/// <summary>Main24 변경 구간만 합성 저장과 가상 입력으로 검증합니다. 기존 전수 QA와 개인 저장은 사용하지 않습니다.</summary>
[InitializeOnLoad] public static class Main24F2TraversalQA
{
 const string Key="Main24F2TraversalQA20261103";
 static int captureWidth=1280;
 static string Root=>Path.Combine(Directory.GetParent(Application.dataPath).FullName,"F2TraversalQA");


 static readonly List<string> checks=new List<string>(),errors=new List<string>();static IEnumerator routine;static double deadline;
 static string Output=>Path.Combine(Root,"Playthrough");
 static Main24F2TraversalQA(){EditorApplication.playModeStateChanged+=s=>{if(s!=PlayModeStateChange.EnteredPlayMode||!SessionState.GetBool(Key,false))return;Directory.CreateDirectory(Output);Application.runInBackground=true;UnityEngine.InputSystem.InputSystem.settings.backgroundBehavior=UnityEngine.InputSystem.InputSettings.BackgroundBehavior.IgnoreFocus;UnityEngine.InputSystem.InputSystem.settings.editorInputBehaviorInPlayMode=UnityEngine.InputSystem.InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;foreach(var device in UnityEngine.InputSystem.InputSystem.devices.ToArray())if(device is UnityEngine.InputSystem.Keyboard||device is UnityEngine.InputSystem.Mouse)UnityEngine.InputSystem.InputSystem.DisableDevice(device);UnityEngine.InputSystem.InputSystem.AddDevice<UnityEngine.InputSystem.Keyboard>("Main24F2VirtualKeyboard");UnityEngine.InputSystem.InputSystem.AddDevice<UnityEngine.InputSystem.Mouse>("Main24F2VirtualMouse");AudioListener.volume=0;Application.targetFrameRate=60;QualitySettings.vSyncCount=0;GameSaveService.AuditSaveDirectory=Path.Combine(Root,"Saves");UserSettingsService.BeginAudit(Path.Combine(Root,"Settings"));UserSettingsService.SetMuteAll(true);Application.logMessageReceived+=(t,st,k)=>{if(k==LogType.Error||k==LogType.Exception||k==LogType.Assert){if(st.Contains("UnityEditor.Search"))File.AppendAllText(Path.Combine(Output,"editor_search_errors.txt"),t+"\n"+st+"\n");else errors.Add(t+"\n"+st);}};routine=Flatten(Run());deadline=EditorApplication.timeSinceStartup+600;EditorApplication.update+=Tick;};}
 public static void Start(){if(!Application.isBatchMode||!Application.dataPath.Replace('\\','/').Contains("/Temp/Main24F2FirstTraversal20261103/QA/Client/Assets"))throw new Exception("격리 Main24 경로만 허용합니다.");Directory.CreateDirectory(Output);EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);SessionState.SetBool(Key,true);EditorApplication.EnterPlaymode();}
 static void Check(bool ok,string name){checks.Add((ok?"PASS|":"FAIL|")+name);File.WriteAllLines(Path.Combine(Output,"checks.txt"),checks);if(!ok)throw new Exception(name);}
 static PlayerController Player=>Object.FindAnyObjectByType<PlayerController>();
 static IEnumerator Wait(float t){float end=Time.realtimeSinceStartup+t;while(Time.realtimeSinceStartup<end)yield return null;}
 static IEnumerator Load(string scene){var op=SceneManager.LoadSceneAsync(scene);while(!op.isDone)yield return null;yield return Wait(.6f);}
 static IEnumerator Until(Func<bool> condition,string label,float timeout=12){float end=Time.realtimeSinceStartup+timeout;while(!condition()&&Time.realtimeSinceStartup<end)yield return null;Check(condition(),label);}
 static Main23Site Site(Main23Site.Kind kind)=>Object.FindObjectsByType<Main23Site>(FindObjectsSortMode.None).Single(x=>x.SiteKind==kind);
 static void At(Vector2 pos){Player.transform.position=pos;var rb=Player.GetComponent<Rigidbody2D>();rb.position=pos;rb.linearVelocity=Vector2.zero;Physics2D.SyncTransforms();}
 static IEnumerator Walk(Main23Site site,string label){var travel=Object.FindAnyObjectByType<Main23MouseTravel>();Check(travel.GoTo(site),label+".RouteFound");yield return Until(()=>!travel.IsTravelling,label+".Arrived",16);Check(Vector2.Distance(Player.transform.position,site.transform.position)<Main23Site.Range,label+".WithinInteractionRadius position="+Player.transform.position+" target="+site.transform.position);}
 static IEnumerator Close(){var d=ProjectLimitless.UI.DialoguePresenter.Instance;for(int i=0;i<40&&d!=null&&d.IsOpen;i++){d.Advance();yield return Wait(.04f);}yield return Wait(.08f);Check(d!=null&&!d.IsOpen,"Dialogue.Closed");}
 static IEnumerator Choose(Main23Site site,int choice,string label){Check(site.TryInteract(),label+".OpenedChoice");yield return Wait(.08f);var panel=Object.FindAnyObjectByType<Main21ChoicePanel>();Check(panel!=null,label+".ActualChoicePanel");panel.Select(choice);yield return Wait(.8f);}
 static void Checkpoint(string n){Check(GameSaveService.SaveCurrentWorldPosition(Player.transform.position,SceneManager.GetActiveScene().name,GameSessionData.LastSpawnPointId),"Save."+n);File.Copy(GameSaveService.GetSaveFilePath(1),Path.Combine(Root,"Checkpoint_"+n+".json"),true);}
 static IEnumerator Continue(string checkpoint){Check(GameSaveService.TryLoadSlot(1,out var data),"Continue.Read."+checkpoint);GameSaveService.RestoreSession(1,data);yield return Load(data.CurrentSceneId);}
 static void Seed(bool completed){GameSessionData.Reset();GameSessionData.ConfigurePlayer(PlayerVisualType.Male,"Main24 합성 QA");GameSessionData.SelectPlayerPath("path.mobility");GameSessionData.SelectJob("guardian");GameSessionData.ConfigureProgress(14,0);EconomyService.AddCurrency(77);if(completed){QuestService.ImportSaveData(new QuestProgressSaveData{CompletedQuestIds=new[]{Chapter3Main22Flow.QuestId,Chapter2Main21Flow.QuestId,"main_05_return_of_three","main_09_reunion_in_silence","main_15_burning_traces"}});CompanionRosterService.UnlockIntroCompanions();CompanionRosterService.UnlockPaul("guardian");CompanionRosterService.UnlockSerin();}GameSaveService.SelectSlot(1);GameSessionData.RecordLocation(Chapter3Main23Flow.Field,Chapter3Main22Flow.ShelterSpawn);GameSessionData.ActivateSafeZone("safezone_starter_village",Chapter3Main23Flow.Field,Chapter3Main22Flow.ShelterSpawn);}
 static IEnumerator Press(UnityEngine.InputSystem.Key key){var keyboard=UnityEngine.InputSystem.Keyboard.current??UnityEngine.InputSystem.InputSystem.AddDevice<UnityEngine.InputSystem.Keyboard>();UnityEngine.InputSystem.InputSystem.QueueStateEvent(keyboard,new UnityEngine.InputSystem.LowLevel.KeyboardState(key));yield return Wait(.08f);UnityEngine.InputSystem.InputSystem.QueueStateEvent(keyboard,new UnityEngine.InputSystem.LowLevel.KeyboardState());yield return Wait(.1f);}
 static IEnumerator Click(Main23Site site){yield return Wait(.4f);var mouse=UnityEngine.InputSystem.Mouse.current??UnityEngine.InputSystem.InputSystem.AddDevice<UnityEngine.InputSystem.Mouse>();Vector2 point=Camera.main.WorldToScreenPoint(site.transform.position);UnityEngine.InputSystem.InputSystem.QueueStateEvent(mouse,new UnityEngine.InputSystem.LowLevel.MouseState{position=point,buttons=1});yield return Wait(.08f);UnityEngine.InputSystem.InputSystem.QueueStateEvent(mouse,new UnityEngine.InputSystem.LowLevel.MouseState{position=point});yield return Wait(.12f);}
 static UnityEngine.UI.Button Button(string name)=>Object.FindObjectsByType<UnityEngine.UI.Button>(FindObjectsSortMode.None).Single(x=>x.name==name);
 static IEnumerator Run(){
  Seed(true);
  QuestService.ImportSaveData(new QuestProgressSaveData{CompletedQuestIds=new[]{Chapter3Main23Flow.QuestId,Chapter3Main22Flow.QuestId,Chapter2Main21Flow.QuestId,"main_05_return_of_three","main_09_reunion_in_silence","main_15_burning_traces"}});
  GameSessionData.ActivateSafeZone("safezone_frostway_village",Chapter3Main23Flow.Village,Chapter3Main23Flow.SafeSpawn);
  Check(QuestService.TryStart(Chapter3Main24Flow.QuestId),"Fixture.Main24Start");
  var progress=QuestService.ExportSaveData();var saved=progress.ActiveQuests.Single(x=>x.QuestId==Chapter3Main24Flow.QuestId);
  // 이전 #80 저장에는 신규 목표가 없었습니다. 기존 4개 ObjectiveId만으로 진행을 복원합니다.
  saved.Objectives=saved.Objectives.Where(x=>!x.ObjectiveId.StartsWith("main24_candidate_")).ToArray();
  foreach(var objective in saved.Objectives)if(objective.ObjectiveId!="await_f2_opening")objective.CurrentCount=1;
  QuestService.ImportSaveData(progress);
  Check(Chapter3Main24Flow.Step==3,"LegacyMain24.StableObjectiveIdsPreserved");
  GameSessionData.ClearWorldPosition();GameSessionData.SetPendingSpawnPoint(Chapter3Main23Flow.SafeSpawn);
  yield return Load(Chapter3Main23Flow.Village);
  int currency=EconomyService.GetCurrency();var job=Resources.LoadAll<JobDefinition>("JobDefinitions").Single(x=>x.JobId=="guardian");string id=PartyResourceService.PlayerCharacterId;
  Check(SkillLoadoutService.TrySetSlot(id,job,5,SkillLoadoutService.GetLibrary(id,job)[0].Id),"Fixture.SlotSixNonEmpty");
  string slots=string.Join("|",SkillLoadoutService.GetSlots(id,job));string owned=string.Join("|",SkillLoadoutService.GetLibrary(id,job).Select(x=>x.Id));
  Check(SkillLoadoutService.GetSlots(id,job).Count==6,"Fixture.ExistingSixSlots");
  yield return Walk(Site(Main23Site.Kind.LockedNorth),"Wheelchair.VillageNorthGate");
  yield return Press(UnityEngine.InputSystem.Key.E);Check(Object.FindAnyObjectByType<Main21ChoicePanel>()!=null,"NorthGate.KeyboardE");
  yield return Press(UnityEngine.InputSystem.Key.Escape);Check(SceneManager.GetActiveScene().name==Chapter3Main23Flow.Village&&Chapter3Main24Flow.Step==3&&!WorldModalState.IsOpen,"NorthGate.CancelNoProgress");
  yield return Choose(Site(Main23Site.Kind.LockedNorth),0,"NorthGate.EnterF2");yield return Wait(.5f);
  Check(SceneManager.GetActiveScene().name==Main24F2Traversal.SceneId&&Chapter3Main24Flow.Step==4,"F2.RealSceneArrivalAdvances");
  Check(Vector2.Distance(Player.transform.position,new Vector2(0,-8))<.15f&&string.IsNullOrEmpty(GameSessionData.PendingSpawnPointId),"F2.SafeSpawnSeparatedFromExit");
  Check(!Physics2D.OverlapCircleAll(Player.transform.position,.52f).Any(c=>!c.isTrigger&&!c.transform.IsChildOf(Player.transform)),"F2.SpawnNoStaticOverlap");
  Check(Object.FindObjectsByType<PlayerController>(FindObjectsSortMode.None).Length==1,"F2.SingleActualPlayer");
  Check(Object.FindAnyObjectByType<WorldBounds2D>()!=null&&Player.GetComponent<Collider2D>().enabled,"F2.ActualPlayerColliderAndBounds");
  var stump=Object.FindAnyObjectByType<Main24F2StumpDepth>();var view=stump.GetComponent<SpriteRenderer>();
  Check(Mathf.Abs(view.sprite.pixelsPerUnit-128)<.01f&&Vector2.Distance(view.sprite.pivot,new Vector2(160,11))<.01f&&stump.transform.localScale==Vector3.one,"Stump.PPU128PivotAndScale");
  Check(stump.GetComponent<BoxCollider2D>().enabled,"Stump.ActualTrunkCollider");
  Check(Mathf.Abs(view.bounds.min.y-(1f-11f/128))<.001f,"Stump.CanvasPivotGroundLine");
  yield return Walk(Site(Main23Site.Kind.F2Observe),"Wheelchair.F2ActualColliderRoute");
  Check(Object.FindAnyObjectByType<Main23GuidanceHud>().CurrentText.Contains("관찰"),"F2.QuestDirectionVisible");
  yield return Press(UnityEngine.InputSystem.Key.F);Check(ProjectLimitless.UI.DialoguePresenter.Instance.IsOpen,"F2.KeyboardF.Observe");
  yield return Press(UnityEngine.InputSystem.Key.Escape);Check(Chapter3Main24Flow.Step==4&&!WorldModalState.IsOpen,"F2.ObserveCancelNoProgress");
  yield return Wait(.3f);yield return Click(Site(Main23Site.Kind.F2Observe));Check(ProjectLimitless.UI.DialoguePresenter.Instance.IsOpen,"F2.Mouse.ReenterObservation");yield return Close();
  Check(Chapter3Main24Flow.Step==5,"F2.ObservationOnlyTemporaryProgress");
  Checkpoint("F2Observed");Vector2 saveAt=Player.transform.position;yield return Continue("F2Observed");yield return Wait(.3f);
  Check(SceneManager.GetActiveScene().name==Main24F2Traversal.SceneId&&Vector2.Distance(Player.transform.position,saveAt)<.15f&&Chapter3Main24Flow.Step==5,"Continue.F2ScenePositionAndProgress");
  Check(GameSessionData.LastSafeZoneSceneId==Chapter3Main23Flow.Village,"Continue.VillageCheckpointPreserved");
  // 실제 카메라와 실제 Player/그루터기를 세 비율로 렌더합니다. OS GameView 캡처로 표시하지 않습니다.
  foreach(int width in new[]{1280,960,1680}){
   captureWidth=width;Camera.main.aspect=width/720f;At(new Vector2(2,1));yield return Wait(.5f);
   var b=Object.FindAnyObjectByType<WorldBounds2D>().Bounds;var c=Camera.main;float h=c.orthographicSize,w=h*c.aspect;
   Check(c.transform.position.x-w>=b.min.x-.01f&&c.transform.position.x+w<=b.max.x+.01f&&c.transform.position.y-h>=b.min.y-.01f&&c.transform.position.y+h<=b.max.y+.01f,"Camera.ViewportWithinF2."+width);
   CaptureCameraUI("F2_ActualPlayMode_Wheelchair_"+width);
   Check(Object.FindAnyObjectByType<Main23GuidanceHud>().CurrentText.Contains("남쪽"),"Hud.ReturnDirection."+width);
  }
  captureWidth=1280;Camera.main.aspect=1280f/720;
  At(new Vector2(-.3f,2));yield return Wait(.4f);stump=Object.FindAnyObjectByType<Main24F2StumpDepth>();view=stump.GetComponent<SpriteRenderer>();var actor=Player.GetComponentInChildren<SpriteRenderer>();
  Check(view.sortingOrder>actor.sortingOrder,"Stump.ActorBehindDepth");CaptureCameraUI("F2_ActualPlayer_BehindStump");
  At(new Vector2(-.3f,.2f));yield return Wait(.4f);Check(view.sortingOrder<actor.sortingOrder,"Stump.ActorInFrontDepth");CaptureCameraUI("F2_ActualPlayer_InFrontOfStump");
  var before=Player.transform.position;
  UnityEngine.InputSystem.InputSystem.QueueStateEvent(UnityEngine.InputSystem.Keyboard.current,new UnityEngine.InputSystem.LowLevel.KeyboardState(UnityEngine.InputSystem.Key.W));yield return Wait(.6f);
  UnityEngine.InputSystem.InputSystem.QueueStateEvent(UnityEngine.InputSystem.Keyboard.current,new UnityEngine.InputSystem.LowLevel.KeyboardState());yield return Wait(.2f);
  Check(Player.transform.position.y<1f&&Player.transform.position.y>=before.y-.05f,"Wheelchair.PhysicsBlockedByStumpNotPassingThrough");
  Check(Object.FindAnyObjectByType<Main23MouseTravel>().GoToPoint(new Vector2(3,1)),"Wheelchair.BypassStump.RouteFound");yield return Until(()=>!Object.FindAnyObjectByType<Main23MouseTravel>().IsTravelling,"Wheelchair.BypassStump.Arrived");
  Check(Vector2.Distance(Player.transform.position,new Vector2(3,1))<.85f,"Wheelchair.BypassStump.ActualPosition");
  yield return Walk(Site(Main23Site.Kind.F2Return),"Wheelchair.F2ReturnRoute");yield return Choose(Site(Main23Site.Kind.F2Return),1,"F2.ReturnCancel");
  Check(SceneManager.GetActiveScene().name==Main24F2Traversal.SceneId&&Chapter3Main24Flow.Step==5,"F2.CancelReturnNoProgress");
  yield return Choose(Site(Main23Site.Kind.F2Return),0,"F2.ReturnVillage");yield return Wait(.5f);
  Check(SceneManager.GetActiveScene().name==Chapter3Main23Flow.Village&&Vector2.Distance(Player.transform.position,new Vector2(0,7))<.15f,"Village.NorthReturnSpawn");
  yield return Walk(Site(Main23Site.Kind.LockedNorth),"Wheelchair.NorthReturnArrival");Check(Chapter3Main24Flow.Step==6,"Village.RoundTripProgressOnly");
  Checkpoint("ReturnedVillage");yield return Continue("ReturnedVillage");Check(Chapter3Main24Flow.Step==6,"Continue.CompletedTraversalNotCompletedQuest");
  GameSessionData.SelectPlayerPath("path.vision");Player.GetComponent<PlayerPathVisualController>().Apply("path.vision");
  yield return Choose(Site(Main23Site.Kind.LockedNorth),0,"Walking.ReenterF2");yield return Wait(.4f);
  Check(Chapter3Main24Flow.Step==6,"RepeatedPortal.NoDuplicateProgress");
  yield return Walk(Site(Main23Site.Kind.F2Observe),"Walking.F2ActualColliderRoute");
  Site(Main23Site.Kind.F2Observe).TryInteract();yield return Close();Check(Chapter3Main24Flow.Step==6,"RepeatedObservation.NoCompletionOrReward");
  captureWidth=1280;At(new Vector2(3,1));yield return Wait(.5f);CaptureCameraUI("F2_ActualPlayMode_Walking_1280");
  var start=Player.transform.position;UnityEngine.InputSystem.InputSystem.QueueStateEvent(UnityEngine.InputSystem.Keyboard.current,new UnityEngine.InputSystem.LowLevel.KeyboardState(UnityEngine.InputSystem.Key.W));yield return Wait(.4f);UnityEngine.InputSystem.InputSystem.QueueStateEvent(UnityEngine.InputSystem.Keyboard.current,new UnityEngine.InputSystem.LowLevel.KeyboardState());yield return Wait(.15f);Check(Player.transform.position.y>start.y+.5f,"Walking.RealKeyboardMovement");
  yield return Walk(Site(Main23Site.Kind.F2Return),"Walking.F2ReturnRoute");yield return Choose(Site(Main23Site.Kind.F2Return),0,"Walking.ReturnVillage");
  // F2 북문 연결이 기존 남문/F1 및 거점을 가로채지 않는지 연결부만 확인합니다.
  yield return Walk(Site(Main23Site.Kind.SouthGate),"Delta.VillageSouthRoute");yield return Choose(Site(Main23Site.Kind.SouthGate),0,"Delta.F1Exit");
  Check(SceneManager.GetActiveScene().name==Chapter3Main23Flow.Field&&Chapter3Main24Flow.Step==6,"Delta.F1LinkPreserved");
  yield return Walk(Site(Main23Site.Kind.NorthGate),"Delta.F1ReentryRoute");yield return Choose(Site(Main23Site.Kind.NorthGate),0,"Delta.VillageReentry");
  Check(SceneManager.GetActiveScene().name==Chapter3Main23Flow.Village,"Delta.VillageLinkPreserved");
  Check(EconomyService.GetCurrency()==currency&&string.Join("|",SkillLoadoutService.GetSlots(id,job))==slots&&string.Join("|",SkillLoadoutService.GetLibrary(id,job).Select(x=>x.Id))==owned,"Final.CurrencyOwnershipSixSlotsUnchanged");
  Check(Chapter3Main23Flow.Completed&&QuestService.GetState(Chapter3Main24Flow.QuestId)==QuestState.Active&&Chapter3Main24Flow.Step==6,"Final.Main23PreservedMain24NoCompletion");
  Check(GameSessionData.LastSafeZoneSceneId==Chapter3Main23Flow.Village,"Final.SafeZonePreserved");
  Check(errors.Count==0,"Console.NoRuntimeErrors");File.WriteAllText(Path.Combine(Output,"status.txt"),"PASS");
 }
 // Camera RT는 실제 Unity UI/Scene 렌더이며 GameView 내부 타깃 캡처와 파일명을 분리합니다.
 static void CaptureCameraUI(string name){var c=Camera.main;var old=c.targetTexture;var active=RenderTexture.active;var rt=RenderTexture.GetTemporary(captureWidth,720,24);var canvases=Object.FindObjectsByType<Canvas>(FindObjectsSortMode.None).Where(x=>x.renderMode==RenderMode.ScreenSpaceOverlay).ToArray();foreach(var cv in canvases){cv.renderMode=RenderMode.ScreenSpaceCamera;cv.worldCamera=c;cv.planeDistance=1;}
  c.targetTexture=rt;Canvas.ForceUpdateCanvases();c.Render();RenderTexture.active=rt;var image=new Texture2D(captureWidth,720,TextureFormat.RGBA32,false);image.ReadPixels(new Rect(0,0,captureWidth,720),0,0);image.Apply();File.WriteAllBytes(Path.Combine(Output,name+".png"),image.EncodeToPNG());Object.Destroy(image);foreach(var cv in canvases){cv.renderMode=RenderMode.ScreenSpaceOverlay;cv.worldCamera=null;}c.targetTexture=old;RenderTexture.active=active;RenderTexture.ReleaseTemporary(rt);}
 static IEnumerator Flatten(IEnumerator first){var stack=new Stack<IEnumerator>();stack.Push(first);while(stack.Count>0){var top=stack.Peek();if(!top.MoveNext()){stack.Pop();continue;}if(top.Current is IEnumerator nested){stack.Push(nested);continue;}yield return top.Current;}}
 static void Tick(){try{if(Player!=null&&SceneManager.GetActiveScene().name==Main24F2Traversal.SceneId&&Time.frameCount%6==0)File.AppendAllText(Path.Combine(Output,"physics_trace.csv"),$"{Time.frameCount},{GameSessionData.SelectedPlayerPathId},{Player.GetComponent<Rigidbody2D>().position.x:F3},{Player.GetComponent<Rigidbody2D>().position.y:F3},{Chapter3Main24Flow.Step}\n");if(EditorApplication.timeSinceStartup>deadline)throw new TimeoutException("Main24 QA timeout");if(routine.MoveNext())return;SessionState.SetBool(Key,false);EditorApplication.update-=Tick;EditorApplication.Exit(0);}catch(Exception ex){File.WriteAllText(Path.Combine(Output,"failure.txt"),ex.ToString());File.WriteAllLines(Path.Combine(Output,"console_errors.txt"),errors);File.WriteAllText(Path.Combine(Output,"status.txt"),"FAIL");EditorApplication.Exit(1);}}
}
}

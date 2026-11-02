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
[InitializeOnLoad] public static class Main24FirstPlayableQA
{
 const string Key="Main24FirstPlayableQA20261103";
 static int captureWidth=1280;
 static string Root=>Path.Combine(Directory.GetParent(Application.dataPath).FullName,"FirstPlayableQA");


 static readonly List<string> checks=new List<string>(),errors=new List<string>();static IEnumerator routine;static double deadline;
 static string Output=>Path.Combine(Root,"Playthrough");
 static Main24FirstPlayableQA(){EditorApplication.playModeStateChanged+=s=>{if(s!=PlayModeStateChange.EnteredPlayMode||!SessionState.GetBool(Key,false))return;Directory.CreateDirectory(Output);Application.runInBackground=true;UnityEngine.InputSystem.InputSystem.settings.backgroundBehavior=UnityEngine.InputSystem.InputSettings.BackgroundBehavior.IgnoreFocus;UnityEngine.InputSystem.InputSystem.settings.editorInputBehaviorInPlayMode=UnityEngine.InputSystem.InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;foreach(var device in UnityEngine.InputSystem.InputSystem.devices.ToArray())if(device is UnityEngine.InputSystem.Keyboard||device is UnityEngine.InputSystem.Mouse)UnityEngine.InputSystem.InputSystem.DisableDevice(device);UnityEngine.InputSystem.InputSystem.AddDevice<UnityEngine.InputSystem.Keyboard>("Main24VirtualKeyboard");UnityEngine.InputSystem.InputSystem.AddDevice<UnityEngine.InputSystem.Mouse>("Main24VirtualMouse");AudioListener.volume=0;Application.targetFrameRate=60;QualitySettings.vSyncCount=0;GameSaveService.AuditSaveDirectory=Path.Combine(Root,"Saves");UserSettingsService.BeginAudit(Path.Combine(Root,"Settings"));UserSettingsService.SetMuteAll(true);Application.logMessageReceived+=(t,st,k)=>{if(k==LogType.Error||k==LogType.Exception||k==LogType.Assert){if(st.Contains("UnityEditor.Search"))File.AppendAllText(Path.Combine(Output,"editor_search_errors.txt"),t+"\n"+st+"\n");else errors.Add(t+"\n"+st);}};routine=Flatten(Run());deadline=EditorApplication.timeSinceStartup+600;EditorApplication.update+=Tick;};}
 public static void Start(){if(!Application.isBatchMode||!Application.dataPath.Replace('\\','/').Contains("/Temp/Main24FirstPlayable20261103/QA/Client/Assets"))throw new Exception("격리 Main24 경로만 허용합니다.");Directory.CreateDirectory(Output);EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);SessionState.SetBool(Key,true);EditorApplication.EnterPlaymode();}
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
  Seed(true);Check(QuestService.GetState(Chapter3Main24Flow.QuestId)==QuestState.Locked,"Main24.RequiresMain23");
  QuestService.TryStart(Chapter3Main23Flow.QuestId);
  var progress=QuestService.ExportSaveData();var q=progress.ActiveQuests.Single(x=>x.QuestId==Chapter3Main23Flow.QuestId);
  for(int i=0;i<q.Objectives.Length-1;i++)q.Objectives[i].CurrentCount=i==1?2:1;
  progress.Main23ClueFlags=3;QuestService.ImportSaveData(progress);
  yield return Load(Chapter3Main23Flow.Village);At(Vector2.zero);yield return Wait(.3f);
  Check(Chapter3Main23Flow.Step==4,"Main23.CheckpointReadyFixture");
  yield return Choose(Site(Main23Site.Kind.Checkpoint),0,"Main23.Checkpoint");yield return Close();
  Check(Chapter3Main23Flow.Completed&&QuestService.GetState(Chapter3Main24Flow.QuestId)==QuestState.Available,"Main23ToMain24.AvailableNoAutoDialogue");
  Check(Chapter3Main24Flow.Step==-1&&!WorldModalState.IsOpen,"Main24.NoForcedStart");
  int currency=EconomyService.GetCurrency();var job=Resources.LoadAll<JobDefinition>("JobDefinitions").Single(x=>x.JobId=="guardian");string id=PartyResourceService.PlayerCharacterId;
  var basic=BattleSkillCatalog.GetSkills(job).Select(x=>x.Id).ToArray();
  Check(SkillLoadoutService.GetLibrary(id,job).Count==3&&SkillLoadoutService.GetSlots(id,job).Count==6,"Skills.FreeThreeSixSlots");
  At(Site(Main23Site.Kind.Supply).transform.position);yield return Wait(.2f);Site(Main23Site.Kind.Supply).TryInteract();yield return Close();
  Check(Chapter3Main24Flow.Step==-1,"OutOfOrder.SupplyDoesNotStart");
  yield return Walk(Site(Main23Site.Kind.Skill),"Wheelchair.SkillRoute");
  yield return Press(UnityEngine.InputSystem.Key.E);Check(Object.FindAnyObjectByType<Main21ChoicePanel>()!=null,"Keyboard.E.OpensShop");
  yield return Press(UnityEngine.InputSystem.Key.Escape);Check(!WorldModalState.IsOpen&&Chapter3Main24Flow.Step==-1,"Shop.CancelNoStart");
  yield return Choose(Site(Main23Site.Kind.Skill),0,"Skill.Guidance");
  Check(ProjectLimitless.UI.DialoguePresenter.Instance.IsOpen,"Skill.ActualDialogue");
  yield return Press(UnityEngine.InputSystem.Key.Escape);Check(Chapter3Main24Flow.Step==-1&&!WorldModalState.IsOpen,"Dialogue.CancelNoProgress");
  yield return Choose(Site(Main23Site.Kind.Skill),0,"Skill.Reenter");yield return Close();
  Check(Chapter3Main24Flow.Step==1,"Skill.CompleteGuidanceAdvances");
  yield return Choose(Site(Main23Site.Kind.Skill),0,"Skill.Repeat");yield return Close();Check(Chapter3Main24Flow.Step==1,"Skill.RepeatNoDuplicate");
  yield return Choose(Site(Main23Site.Kind.Skill),1,"Skill.FreeManagement");
  Check(ProjectLimitless.UI.SkillManagementPresenter.Instance.IsOpen,"Skills.ExistingUiConnected");
  var buttons=(List<UnityEngine.UI.Button>)typeof(ProjectLimitless.UI.SkillManagementPresenter).GetField("slotButtons",System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.NonPublic).GetValue(ProjectLimitless.UI.SkillManagementPresenter.Instance);
  Check(buttons.Count==6&&buttons.All(x=>x.gameObject.activeInHierarchy),"Skills.SixActualVisibleCells");
  buttons[5].onClick.Invoke();Button("Library_"+basic[0]).onClick.Invoke();Button("Equip").onClick.Invoke();yield return Wait(.15f);
  Check(SkillLoadoutService.GetSlots(id,job)[5]==basic[0],"Skills.UiEquipSlotSix");
  Button("Unequip").onClick.Invoke();yield return Wait(.15f);Check(string.IsNullOrEmpty(SkillLoadoutService.GetSlots(id,job)[5]),"Skills.UiUnequip");
  Button("Library_"+basic[0]).onClick.Invoke();Button("Equip").onClick.Invoke();yield return Wait(.15f);
  CaptureCameraUI("Main24_SixSlots_1280");yield return Press(UnityEngine.InputSystem.Key.Escape);
  Check(!WorldModalState.IsOpen,"Skills.EscapeReturnsWorld");
  string owned=string.Join(",",SkillLoadoutService.GetLibrary(id,job).Select(x=>x.Id));
  Check(EconomyService.GetCurrency()==currency&&owned==string.Join(",",basic),"Shop.NoCurrencyOrOwnershipChange");
  yield return Walk(Site(Main23Site.Kind.Supply),"Wheelchair.SupplyRoute");
  yield return Press(UnityEngine.InputSystem.Key.F);Check(ProjectLimitless.UI.DialoguePresenter.Instance.IsOpen,"Keyboard.F.SupplyDialogue");
  yield return Press(UnityEngine.InputSystem.Key.Escape);Check(Chapter3Main24Flow.Step==1,"Supply.CancelNoProgress");
  yield return Wait(.2f);yield return Click(Site(Main23Site.Kind.Supply));Check(ProjectLimitless.UI.DialoguePresenter.Instance.IsOpen,"Mouse.SupplyDialogue");yield return Close();
  Check(Chapter3Main24Flow.Step==2,"Supply.PurchaseFreeAdvance");Checkpoint("Prepared");yield return Continue("Prepared");yield return Wait(.3f);
  Check(Chapter3Main24Flow.Step==2&&SkillLoadoutService.GetSlots(id,job)[5]==basic[0],"Continue.QuestAndSlotSix");
  Check(GameSessionData.LastSafeZoneSceneId==Chapter3Main23Flow.Village,"Continue.CheckpointPreserved");
  yield return Walk(Site(Main23Site.Kind.LockedNorth),"Wheelchair.NorthRoute");
  Check(Chapter3Main24Flow.Step==3,"North.PhysicalArrival");
  Site(Main23Site.Kind.LockedNorth).TryInteract();yield return Close();Site(Main23Site.Kind.LockedNorth).TryInteract();yield return Close();
  Check(Chapter3Main24Flow.Step==3&&QuestService.GetState(Chapter3Main24Flow.QuestId)==QuestState.Active,"F2.HoldNoQuestCompletion");
  Check(SceneManager.GetActiveScene().name==Chapter3Main23Flow.Village&&EconomyService.GetCurrency()==currency,"F2.NoSceneLoadNoReward");
  Check(Object.FindObjectsByType<Text>(FindObjectsSortMode.None).Any(t=>t.text.Contains("F2 진입/전투 준비 중")),"Hud.VisibleF2Hold");
  Checkpoint("North");yield return Continue("North");yield return Wait(.3f);Check(Chapter3Main24Flow.Step==3,"Continue.NorthHold");
  foreach(int width in new[]{1280,960,1680}){captureWidth=width;Camera.main.aspect=width/720f;At(Vector2.zero);yield return Wait(.4f);CaptureCameraUI("Main24_Hud_"+width);Check(Object.FindAnyObjectByType<Main23GuidanceHud>().CurrentText.Contains("Main24"),"Hud.Aspect."+width);}
  GameSessionData.SelectPlayerPath("path.vision");Player.GetComponent<PlayerVisualController>().ApplySelectedAppearance();At(new Vector2(0,-2));yield return Wait(.2f);
  var start=Player.transform.position;var keyboard=UnityEngine.InputSystem.Keyboard.current;UnityEngine.InputSystem.InputSystem.QueueStateEvent(keyboard,new UnityEngine.InputSystem.LowLevel.KeyboardState(UnityEngine.InputSystem.Key.W));yield return Wait(.4f);UnityEngine.InputSystem.InputSystem.QueueStateEvent(keyboard,new UnityEngine.InputSystem.LowLevel.KeyboardState());yield return Wait(.2f);Check(Player.transform.position.y>start.y+.5f,"Walking.KeyboardMovement");
  yield return Walk(Site(Main23Site.Kind.Skill),"Walking.SkillRoute");
  yield return Walk(Site(Main23Site.Kind.SouthGate),"Walking.SouthGate");yield return Choose(Site(Main23Site.Kind.SouthGate),0,"Village.ReturnF1");yield return Wait(.5f);Check(SceneManager.GetActiveScene().name==Chapter3Main23Flow.Field&&Chapter3Main24Flow.Step==3,"F1.QuestPreserved");
  yield return Walk(Site(Main23Site.Kind.NorthGate),"Walking.VillageReturn");yield return Choose(Site(Main23Site.Kind.NorthGate),0,"F1.ReenterVillage");yield return Wait(.6f);
  Check(SceneManager.GetActiveScene().name==Chapter3Main23Flow.Village&&Chapter3Main24Flow.Step==3,"Village.RoundTripPreserved");
  Check(EconomyService.GetCurrency()==currency&&string.Join(",",SkillLoadoutService.GetLibrary(id,job).Select(x=>x.Id))==owned,"Final.CurrencyOwnershipUnchanged");
  Check(Chapter3Main23Flow.Completed&&!QuestService.CompletedQuestIds.Contains(Chapter3Main24Flow.QuestId),"Final.Main23PreservedMain24NotCompleted");
  Check(errors.Count==0,"Console.NoRuntimeErrors");File.WriteAllText(Path.Combine(Output,"status.txt"),"PASS");
 }
 // Camera RT는 실제 Unity UI/Scene 렌더이며 GameView 내부 타깃 캡처와 파일명을 분리합니다.
 static void CaptureCameraUI(string name){var c=Camera.main;var old=c.targetTexture;var active=RenderTexture.active;var rt=RenderTexture.GetTemporary(captureWidth,720,24);var canvases=Object.FindObjectsByType<Canvas>(FindObjectsSortMode.None).Where(x=>x.renderMode==RenderMode.ScreenSpaceOverlay).ToArray();foreach(var cv in canvases){cv.renderMode=RenderMode.ScreenSpaceCamera;cv.worldCamera=c;cv.planeDistance=1;}
  c.targetTexture=rt;Canvas.ForceUpdateCanvases();c.Render();RenderTexture.active=rt;var image=new Texture2D(captureWidth,720,TextureFormat.RGBA32,false);image.ReadPixels(new Rect(0,0,captureWidth,720),0,0);image.Apply();File.WriteAllBytes(Path.Combine(Output,name+".png"),image.EncodeToPNG());Object.Destroy(image);foreach(var cv in canvases){cv.renderMode=RenderMode.ScreenSpaceOverlay;cv.worldCamera=null;}c.targetTexture=old;RenderTexture.active=active;RenderTexture.ReleaseTemporary(rt);}
 static IEnumerator Flatten(IEnumerator first){var stack=new Stack<IEnumerator>();stack.Push(first);while(stack.Count>0){var top=stack.Peek();if(!top.MoveNext()){stack.Pop();continue;}if(top.Current is IEnumerator nested){stack.Push(nested);continue;}yield return top.Current;}}
 static void Tick(){try{if(EditorApplication.timeSinceStartup>deadline)throw new TimeoutException("Main24 QA timeout");if(routine.MoveNext())return;SessionState.SetBool(Key,false);EditorApplication.update-=Tick;EditorApplication.Exit(0);}catch(Exception ex){File.WriteAllText(Path.Combine(Output,"failure.txt"),ex.ToString());File.WriteAllLines(Path.Combine(Output,"console_errors.txt"),errors);File.WriteAllText(Path.Combine(Output,"status.txt"),"FAIL");EditorApplication.Exit(1);}}
}
}

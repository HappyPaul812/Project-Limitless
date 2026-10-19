using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ProjectLimitless.Core;
using ProjectLimitless.Player;
using ProjectLimitless.World;
using ProjectLimitless.UI;
using ProjectLimitless.Monster;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using Object=UnityEngine.Object;
namespace ProjectLimitless.EditorTools
{
    /// <summary>원본 Editor/저장을 사용하지 않고 Main09 안내·입력·6단계·Continue를 실제 월드에서 검증합니다.</summary>
    [InitializeOnLoad] public static class Main09ClarityAudit
    {
        const string Armed="Main09Clarity20261019";
        static string Mode=>SessionState.GetString(Armed+"Mode","Before");
        static string Root=>Path.GetFullPath(Path.Combine(Application.dataPath,"../../../Main09Clarity20261019/Evidence",Mode));
        static readonly BindingFlags Flags=BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
        static readonly List<string> Checks=new List<string>(),Errors=new List<string>();
        static IEnumerator routine;static double deadline;static Keyboard keyboard;static Gamepad gamepad;
        static PlayerController Player=>Object.FindAnyObjectByType<PlayerController>();
        static QuestNavigationPresenter Nav=>Object.FindAnyObjectByType<QuestNavigationPresenter>();
        static T Get<T>(object o,string n)=>(T)o.GetType().GetField(n,Flags).GetValue(o);
        static Main09ClarityAudit(){EditorApplication.playModeStateChanged+=s=>{if(s!=PlayModeStateChange.EnteredPlayMode||!SessionState.GetBool(Armed,false))return;
            GameSaveService.AuditSaveDirectory=Path.Combine(Root,"Saves");UserSettingsService.BeginAudit(Path.Combine(Root,"Settings"));
            Application.runInBackground=true;AudioListener.volume=0;InputSystem.settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;
            keyboard=InputSystem.AddDevice<Keyboard>();gamepad=InputSystem.AddDevice<Gamepad>();Application.logMessageReceived+=(t,st,k)=>{if(k==LogType.Error||k==LogType.Exception){Errors.Add(t+"\n"+st);File.WriteAllLines(Path.Combine(Root,"errors.txt"),Errors);}};
            routine=Flatten(Run());deadline=EditorApplication.timeSinceStartup+1800;EditorApplication.update+=Tick;};}
        public static void RunBefore(){SessionState.SetString(Armed+"Mode","Before");Start();}
        public static void RunAfter(){SessionState.SetString(Armed+"Mode","After");Start();}
        static void Start(){if(!Application.isBatchMode||!Application.dataPath.Replace('\\','/').Contains("/Temp/Field05Review20261019/Client/Assets"))throw new Exception("격리 Batch만 허용");Directory.CreateDirectory(Root);File.WriteAllText(Path.Combine(Root,"failure.txt"),"");File.WriteAllText(Path.Combine(Root,"status.txt"),"RUNNING");GameSaveService.AuditSaveDirectory=Path.Combine(Root,"Saves");UserSettingsService.BeginAudit(Path.Combine(Root,"Settings"));EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);SessionState.SetBool(Armed,true);EditorApplication.EnterPlaymode();}
        static void Check(bool ok,string label){Checks.Add((ok?"PASS|":"FAIL|")+label);File.WriteAllLines(Path.Combine(Root,"checks.txt"),Checks);if(!ok)throw new Exception(label);}
        static IEnumerator Wait(float s=.15f){float end=Time.realtimeSinceStartup+s;while(Time.realtimeSinceStartup<end)yield return null;}
        static IEnumerator Load(string scene){if(scene!="Bootstrap"&&scene!="Battle")GameSessionData.RecordLocation(scene,scene=="Field_03"?"Spawn_From_Field02":"");SceneManager.LoadSceneAsync(scene);float end=Time.realtimeSinceStartup+40;while(SceneManager.GetActiveScene().name!=scene&&Time.realtimeSinceStartup<end)yield return null;Check(SceneManager.GetActiveScene().name==scene,"Scene."+scene);MonsterEncounterService.SuppressForSeconds(3600);yield return Wait(.4f);}
        static void Seed(string path){GameSessionData.Reset();GameSaveService.SelectSlot(1);GameSessionData.ConfigurePlayer(PlayerVisualType.Male,"Main09 안내 QA");GameSessionData.SelectPlayerPath(path);GameSessionData.SelectJob("mage");GameSessionData.ConfigureProgress(12,0);CompanionRosterService.UnlockIntroCompanions();QuestService.ImportSaveData(new QuestProgressSaveData{CompletedQuestIds=QuestCatalog.All.Where(x=>x.QuestId.StartsWith("main_")&&string.CompareOrdinal(x.QuestId.Substring(0,7),"main_09")<0).Select(x=>x.QuestId).ToArray()});GameSessionData.SetPendingSpawnPoint("Spawn_From_Field02");}
        static void At(Vector2 p){var b=Player.GetComponent<Rigidbody2D>();b.linearVelocity=Vector2.zero;b.position=p;Player.transform.position=p;Physics2D.SyncTransforms();}
        static IEnumerator Press(Key key){InputSystem.QueueStateEvent(keyboard,new KeyboardState(key));yield return Wait(.09f);InputSystem.QueueStateEvent(keyboard,new KeyboardState());yield return Wait(.09f);}
        static IEnumerator A(){InputSystem.QueueStateEvent(gamepad,new GamepadState().WithButton(GamepadButton.South));yield return Wait(.09f);InputSystem.QueueStateEvent(gamepad,new GamepadState());yield return Wait(.09f);}
        static IEnumerator Shot(string name,int width){PlayModeWindow.SetCustomRenderingResolution((uint)width,720,"Main09 background QA");float resizeEnd=Time.realtimeSinceStartup+5;while((Screen.width!=width||Screen.height!=720)&&Time.realtimeSinceStartup<resizeEnd){PlayModeWindow.SetCustomRenderingResolution((uint)width,720,"Main09 background QA");yield return Wait(.25f);}yield return Wait(.5f);Canvas.ForceUpdateCanvases();Check(Screen.width==width&&Screen.height==720,"Capture.Resolution."+name+".Actual"+Screen.width+"x"+Screen.height);ScreenCapture.CaptureScreenshot(Path.Combine(Root,name+".png"));yield return Wait(.35f);}
        static IEnumerator Views(string prefix){foreach(int width in new[]{1280,960,1680}){yield return Shot(prefix+"_world_"+width,width);yield return Press(Key.Q);Check(QuestLogPresenter.Instance.IsOpen,"Q.LogOpen."+prefix+width);yield return Shot(prefix+"_log_"+width,width);yield return Press(Key.Escape);Check(!QuestLogPresenter.Instance.IsOpen,"Esc.LogClosed."+prefix+width);}}
        static IEnumerator Continue(string label){string quest=JsonUtility.ToJson(QuestService.ExportSaveData()),party=JsonUtility.ToJson(CompanionRosterService.ExportSaveData());var at=Player.transform.position;Check(GameSaveService.SaveCurrentWorldPosition(at,"Field_03"),"Save."+label);yield return Load("Bootstrap");GameSessionData.Reset();GameObject.Find("StartMenuCanvas/Slot01/Action").GetComponent<Button>().onClick.Invoke();float end=Time.realtimeSinceStartup+40;while(SceneManager.GetActiveScene().name!="Field_03"&&Time.realtimeSinceStartup<end)yield return null;yield return Wait(.4f);Check(SceneManager.GetActiveScene().name=="Field_03"&&quest==JsonUtility.ToJson(QuestService.ExportSaveData())&&party==JsonUtility.ToJson(CompanionRosterService.ExportSaveData())&&Vector2.Distance(at,Player.transform.position)<.05f,"Continue.Exact."+label);}
        static bool Clear(Vector2 p,float radius)=>!Physics2D.OverlapCircleAll(p,radius).Any(x=>!x.isTrigger&&x.GetComponentInParent<PlayerController>()==null);
        static IEnumerator Walk(Vector2 target,bool pad){
            const float step=.25f;float radius=Player.GetComponent<CircleCollider2D>().radius*Player.transform.lossyScale.x+.03f;var start=new Vector2Int(Mathf.RoundToInt(Player.transform.position.x/step),Mathf.RoundToInt(Player.transform.position.y/step));var goal=new Vector2Int(Mathf.RoundToInt(target.x/step),Mathf.RoundToInt(target.y/step));
            var q=new Queue<Vector2Int>();var prev=new Dictionary<Vector2Int,Vector2Int>();q.Enqueue(start);prev[start]=start;
            bool found=false;while(q.Count>0){var n=q.Dequeue();if(Vector2.Distance((Vector2)n*step,target-Vector2.up*.7f)<=1.15f&&Clear((Vector2)n*step,radius)){goal=n;found=true;break;}foreach(var dir in new[]{Vector2Int.up,Vector2Int.down,Vector2Int.left,Vector2Int.right}){var next=n+dir;var point=(Vector2)next*step;if(Mathf.Abs(point.x)>12||Mathf.Abs(point.y)>8||prev.ContainsKey(next)||!Clear(point,radius))continue;prev[next]=n;q.Enqueue(next);}}
            Check(found,"Route.Exists."+target+".Pad"+pad);var points=new List<Vector2>();var node=goal;while(node!=start){points.Add((Vector2)node*step);node=prev[node];}points.Reverse();
            foreach(var point in points){float end=Time.realtimeSinceStartup+3;while(Vector2.Distance(Player.transform.position,point)>.11f&&Time.realtimeSinceStartup<end){Vector2 diff=point-(Vector2)Player.transform.position;Vector2 axis=Mathf.Abs(diff.x)>Mathf.Abs(diff.y)?new Vector2(Mathf.Sign(diff.x),0):new Vector2(0,Mathf.Sign(diff.y));if(pad)InputSystem.QueueStateEvent(gamepad,new GamepadState{leftStick=axis});else InputSystem.QueueStateEvent(keyboard,new KeyboardState(axis.x>0?Key.D:axis.x<0?Key.A:axis.y>0?Key.W:Key.S));yield return Wait(.02f);}InputSystem.QueueStateEvent(keyboard,new KeyboardState());InputSystem.QueueStateEvent(gamepad,new GamepadState());yield return Wait(.03f);Check(Vector2.Distance(Player.transform.position,point)<.23f,"Route.PhysicsInput."+point+".Pad"+pad);}
        }
        static IEnumerator Run(){yield return Load("Bootstrap");Seed("path.mobility");yield return Load("Field_03");At(new Vector2(2.1876f,1.6177f));yield return Wait(.4f);Check(QuestService.ActiveMainQuest?.Definition.QuestId==MainQuest09FieldFlow.QuestId,"Main09.Current");yield return Views("Step1");if(Mode=="Before"){File.WriteAllText(Path.Combine(Root,"status.txt"),"BEFORE_CAPTURED");yield break;}
            var camera=Camera.main;var cameraAt=camera.transform.position;var actor0=Object.FindObjectsByType<MainQuest09Interactable>().Single(x=>x.TargetId==MainQuest09FieldFlow.BlueTraceId);
            camera.transform.position+=Vector3.left*30;typeof(QuestNavigationPresenter).GetMethod("LateUpdate",Flags).Invoke(Nav,null);
            float actual=(float)typeof(QuestNavigationPresenter).GetProperty("PlayerDistance").GetValue(Nav);Check(Mathf.Abs(actual-Vector2.Distance(Player.transform.position,actor0.transform.position))<.001f,"Distance.PlayerXYNotCamera");Check(Get<Text>(Nav,"arrowText").gameObject.activeSelf,"Arrow.OffscreenVisible");
            camera.transform.position=cameraAt;yield return Wait(.2f);
            foreach(bool pad in new[]{false,true}){
                Seed(pad?"path.mobility":"path.vision");yield return Load("Field_03");At(new Vector2(2.1876f,1.6177f));yield return Wait(.4f);int currency=EconomyService.GetCurrency(),exp=GameSessionData.CurrentExperience;
                for(int step=0;step<6;step++){
                    var quest=QuestService.ActiveMainQuest;Check(quest.Definition.QuestId==MainQuest09FieldFlow.QuestId&&quest.CurrentObjectiveIndex==step,"Step.Sequential."+step+".Pad"+pad);
                    string id=quest.CurrentObjective.TargetId;var actor=Object.FindObjectsByType<MainQuest09Interactable>().Single(x=>x.TargetId==id);Check(QuestNavigationTargetRegistry.TryGet(id,out var target)&&target.Transform.position==actor.transform.position,"Anchor.ExactInteractionCenter."+step);Check(actor.transform.Find("Main09GroundMarker").gameObject.activeSelf,"Floor.Visible."+step);
                    var navText=Get<Text>(Nav,"markerText");Check(navText.text.StartsWith((step+1)+"/6")&&navText.text.Contains("E/F")&&navText.text.Contains("게임패드 A"),"HUD.StepControls."+step);if(step==3)Check(navText.text.Contains("폴에게 다시 말을 거세요"),"Paul.AgainExplicit");
                    yield return Press(Key.Q);Check(QuestLogPresenter.Instance.Details.Contains(quest.CurrentObjective.Description)&&QuestLogPresenter.Instance.Details.Contains("게임패드 A"),"Log.ObjectiveSynced."+step);int unchanged=quest.CurrentObjectiveIndex;Check(!actor.TryInteract(),"Modal.BlocksInteraction."+step);yield return Press(Key.Escape);Check(quest.CurrentObjectiveIndex==unchanged,"Modal.NoProgress."+step);
                    yield return Walk((Vector2)actor.transform.position+Vector2.up*.7f,pad);yield return Wait(.2f);Check(Vector2.Distance(Player.transform.position,actor.transform.position)<=1.6f,"Approach.WithinOriginalRadius."+step);Check(navText.text.Contains(step==2||step==3||step==5?"대화":"조사"),"Approach.ActionHint."+step);
                    if(!pad)yield return Views("Stage"+(step+1));else yield return Shot("Wheelchair_Stage"+(step+1),1280);
                    if(pad)yield return A();else yield return Press(step%2==0?Key.E:Key.F);Check(DialoguePresenter.Instance.IsOpen,"Input.ActualInteract."+step+".Pad"+pad);
                    if(step==0){DialoguePresenter.Instance.Hide();yield return Wait(.1f);Check(QuestService.ActiveMainQuest.CurrentObjectiveIndex==0,"Cancel.NoProgress");yield return Press(Key.E);Check(DialoguePresenter.Instance.IsOpen,"Cancel.Restart");At(actor.transform.position+Vector3.up*4);yield return Wait(.3f);Check(!DialoguePresenter.Instance.IsOpen&&QuestService.ActiveMainQuest.CurrentObjectiveIndex==0,"DistanceCancel.NoProgress");At(actor.transform.position+Vector3.up*.7f);if(pad)yield return A();else yield return Press(Key.F);}
                    for(int page=0;page<MainQuest09Dialogue.Lines(step).Length;page++){if(pad)yield return A();else yield return Press(Key.Enter);}Check(!DialoguePresenter.Instance.IsOpen,"Dialogue.Completed."+step+".Pad"+pad);yield return Wait(.15f);if(step==2||step==4)yield return Continue("Step"+step+".Pad"+pad);
                }
                Check(QuestService.GetState(MainQuest09FieldFlow.QuestId)==QuestState.Completed,"Quest.Complete.Pad"+pad);Check(EconomyService.GetCurrency()==currency+35&&GameSessionData.CurrentExperience==exp+35,"Reward.Exact35Once.Pad"+pad);Check(CompanionRosterService.IsUnlocked(CompanionRosterService.PaulId),"Paul.Joined.Pad"+pad);QuestService.NotifyNpcTalked(CompanionRosterService.PaulId);Check(EconomyService.GetCurrency()==currency+35,"Reward.NoDuplicate.Pad"+pad);yield return Continue("Completed.Pad"+pad);
            }
            Check(Errors.All(x=>x.Contains("UnityEditor.Search")||x.Contains("Host type is not matching")),"Console.NewErrors0");File.WriteAllText(Path.Combine(Root,"status.txt"),"PASS_KNOWN_ENV_PARTIAL");
        }
        static IEnumerator Flatten(IEnumerator r){var s=new Stack<IEnumerator>();s.Push(r);while(s.Count>0){var t=s.Peek();if(!t.MoveNext()){s.Pop();continue;}if(t.Current is IEnumerator c){s.Push(c);continue;}yield return t.Current;}}
        static void Tick(){try{if(EditorApplication.timeSinceStartup>deadline)throw new TimeoutException("Main09 QA timeout");if(routine.MoveNext())return;EditorApplication.update-=Tick;SessionState.SetBool(Armed,false);EditorApplication.Exit(0);}catch(Exception e){File.WriteAllText(Path.Combine(Root,"status.txt"),"FAIL");File.WriteAllText(Path.Combine(Root,"failure.txt"),e.ToString());EditorApplication.Exit(1);}}
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ProjectLimitless.Battle;
using ProjectLimitless.Core;
using ProjectLimitless.Player;
using ProjectLimitless.UI;
using ProjectLimitless.World;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object=UnityEngine.Object;

/// <summary>F2 Scene 등록으로 연결되는 로드·귀환·합성 Continue만 검증합니다. 기존44/81/90 QA를 호출하지 않습니다.</summary>
[DefaultExecutionOrder(-10000)]
public sealed class F2RegistrationProbe:MonoBehaviour
{
    public static string Output=>Path.GetFullPath(Path.Combine(Application.dataPath,"../../Evidence"));
    public bool Done,Failed;IEnumerator routine;readonly List<string> checks=new List<string>(),errors=new List<string>();
    static PlayerController Player=>Object.FindAnyObjectByType<PlayerController>();
    static BattleSceneController Battle=>Object.FindAnyObjectByType<BattleSceneController>();
    static T Read<T>(object o,string f)=>(T)o.GetType().GetField(f,BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance).GetValue(o);
    static bool Ended=>Read<bool>(Battle,"battleEnded");
    static bool Ready=>Battle!=null&&!Ended&&!Read<bool>(Battle,"actionPlaying")&&Read<Combatant>(Battle,"currentActor")?.IsPlayerControlled==true;
    static Formation Enemies=>Read<Formation>(Battle,"enemies");
    static Button Button(string name)=>Object.FindObjectsByType<Button>(FindObjectsSortMode.None).Single(x=>x.name==name);
    void Awake(){DontDestroyOnLoad(gameObject);Directory.CreateDirectory(Output);GameSaveService.AuditSaveDirectory=Path.GetFullPath(Path.Combine(Application.dataPath,"../../SyntheticSaves"));UserSettingsService.BeginAudit(Path.GetFullPath(Path.Combine(Application.dataPath,"../../SyntheticSettings")));UserSettingsService.SetMuteAll(true);AudioListener.volume=0;Application.runInBackground=true;QualitySettings.vSyncCount=0;Application.targetFrameRate=60;Application.logMessageReceived+=(text,stack,type)=>{if(type==LogType.Error||type==LogType.Exception||type==LogType.Assert){if(stack.Contains("UnityEditor.Search.SearchDatabase"))File.AppendAllText(Path.Combine(Output,"EnvironmentErrors.txt"),text+"\n"+stack);else errors.Add(text+"\n"+stack);}};}
    void Start(){routine=Flatten(Run());}
    void Update(){if(Done||routine==null)return;try{if(!routine.MoveNext())Finish(false);}catch(Exception e){errors.Add(e.ToString());Finish(true);}}
    void Finish(bool failed){Done=true;Failed=failed||errors.Count>0;File.WriteAllLines(Path.Combine(Output,"checks.txt"),checks);File.WriteAllLines(Path.Combine(Output,"Errors.txt"),errors);File.WriteAllText(Path.Combine(Output,"status.txt"),Failed?"FAIL":"PASS");}
    void Check(bool ok,string label){checks.Add((ok?"PASS ":"FAIL ")+label);File.WriteAllLines(Path.Combine(Output,"checks.txt"),checks);if(!ok)throw new Exception(label);}
    static IEnumerator Flatten(IEnumerator e){var stack=new Stack<IEnumerator>();stack.Push(e);while(stack.Count>0){var top=stack.Peek();if(!top.MoveNext()){stack.Pop();continue;}if(top.Current is IEnumerator nested)stack.Push(nested);else yield return top.Current;}}
    static IEnumerator Wait(float s){float end=Time.realtimeSinceStartup+s;while(Time.realtimeSinceStartup<end)yield return null;}
    IEnumerator Until(Func<bool> condition,string label,float seconds=16){float end=Time.realtimeSinceStartup+seconds;while(!condition()&&Time.realtimeSinceStartup<end)yield return null;Check(condition(),label);}
    static void At(Vector2 p){Player.transform.position=p;var rb=Player.GetComponent<Rigidbody2D>();rb.position=p;rb.linearVelocity=Vector2.zero;Physics2D.SyncTransforms();}
    static IEnumerator Load(string scene){var op=SceneManager.LoadSceneAsync(scene);while(!op.isDone)yield return null;yield return Wait(.6f);}
    IEnumerator Travel(Main23Site.Kind kind,string destination,string label){var site=Object.FindObjectsByType<Main23Site>(FindObjectsSortMode.None).Single(x=>x.SiteKind==kind);At((Vector2)site.transform.position+Vector2.up*.5f);yield return Wait(.2f);Check(site.TryInteract(),label+".ActualSiteInteraction");yield return Wait(.1f);Object.FindAnyObjectByType<Main21ChoicePanel>().Select(0);yield return Until(()=>SceneManager.GetActiveScene().name==destination&&Player!=null,label+".SceneLoaded");yield return Wait(.6f);}
    IEnumerator EnterBattle(){At(new Vector2(4.2f,-2));yield return Wait(.3f);Check(Object.FindAnyObjectByType<Main24FrostOwlCandidate>().TryEnter(),"Owl.ExistingEntryService");yield return Until(()=>Battle!=null&&SceneManager.GetActiveScene().name=="Battle","Owl.BattleLoaded");yield return Wait(.2f);}
    void Capture(string name){var camera=Camera.main;if(camera==null)return;var canvases=Object.FindObjectsByType<Canvas>(FindObjectsSortMode.None).Where(x=>x.renderMode==RenderMode.ScreenSpaceOverlay).ToArray();var prior=camera.targetTexture;var active=RenderTexture.active;var rt=RenderTexture.GetTemporary(1280,720,24);camera.targetTexture=rt;foreach(var c in canvases){c.renderMode=RenderMode.ScreenSpaceCamera;c.worldCamera=camera;c.planeDistance=1;}Canvas.ForceUpdateCanvases();camera.Render();RenderTexture.active=rt;var t=new Texture2D(1280,720,TextureFormat.RGBA32,false);t.ReadPixels(new Rect(0,0,1280,720),0,0);t.Apply();File.WriteAllBytes(Path.Combine(Output,name+".png"),t.EncodeToPNG());Destroy(t);foreach(var c in canvases){c.renderMode=RenderMode.ScreenSpaceOverlay;c.worldCamera=null;}camera.targetTexture=prior;RenderTexture.active=active;RenderTexture.ReleaseTemporary(rt);}
    IEnumerator Run(){
        GameSessionData.Reset();GameSessionData.ConfigurePlayer(PlayerVisualType.Male,"F2 등록 합성");GameSessionData.SelectPlayerPath("path.mobility");GameSessionData.SelectJob("guardian");GameSessionData.ConfigureProgress(14,0);GameSaveService.SelectSlot(1);EconomyService.AddCurrency(77);
        QuestService.ImportSaveData(new QuestProgressSaveData{CompletedQuestIds=new[]{Chapter3Main23Flow.QuestId,Chapter3Main22Flow.QuestId,Chapter2Main21Flow.QuestId}});QuestService.TryStart(Chapter3Main24Flow.QuestId);var seed=QuestService.ExportSaveData();var q=seed.ActiveQuests.Single(x=>x.QuestId==Chapter3Main24Flow.QuestId);for(int i=0;i<3;i++)q.Objectives[i].CurrentCount=1;QuestService.ImportSaveData(seed);
        GameSessionData.ActivateSafeZone("safezone_frostway_village",Chapter3Main23Flow.Village,Chapter3Main23Flow.SafeSpawn);GameSessionData.SetPendingSpawnPoint(Chapter3Main23Flow.SafeSpawn);yield return Load(Chapter3Main23Flow.Village);
        Check(Application.CanStreamedLevelBeLoaded(Main24F2Traversal.SceneId),"RegisteredF2IsLoadable");yield return Travel(Main23Site.Kind.LockedNorth,Main24F2Traversal.SceneId,"NorthGateToF2");Check(Object.FindAnyObjectByType<Main24FrostOwlCandidate>()!=null,"F2ExistingEncounterPresent");Capture("F2_RegisteredEntry");
        Check(GameSaveService.SaveCurrentWorldPosition(Player.transform.position,Main24F2Traversal.SceneId,GameSessionData.LastSpawnPointId),"SyntheticF2Save");Check(GameSaveService.TryLoadSlot(1,out var saved),"SyntheticF2Read");GameSaveService.RestoreSession(1,saved);yield return Load(saved.CurrentSceneId);Check(SceneManager.GetActiveScene().name==Main24F2Traversal.SceneId&&Player!=null,"ContinueLoadsRegisteredF2");
        yield return Travel(Main23Site.Kind.F2Return,Chapter3Main23Flow.Village,"F2ToVillage");yield return Travel(Main23Site.Kind.LockedNorth,Main24F2Traversal.SceneId,"RepeatNorthGateToF2");yield return EnterBattle();
        int attacks=0;while(!Ended&&attacks++<8){yield return Until(()=>Ready||Ended,"VictoryNextTurn");if(Ended)break;Button("AttackButton").onClick.Invoke();yield return Wait(.08f);var views=(IDictionary)Read<object>(Battle,"combatantViews");Read<Button>(views[Enemies.Members.Single()],"HitArea").onClick.Invoke();yield return Wait(.8f);}
        Check(Ended,"VictoryReached");Button("VictoryReturn").onClick.Invoke();yield return Until(()=>SceneManager.GetActiveScene().name==Main24F2Traversal.SceneId&&Player!=null,"VictoryReturnsRegisteredF2");yield return Wait(.6f);Capture("F2_AfterVictory");
        yield return Wait(2.2f);var growth=CharacterGrowthCalculator.Calculate("guardian",14);int hp=CharacterGrowthCalculator.CalculateMaxHp("guardian",growth),mp=CharacterGrowthCalculator.CalculateMaxMp("guardian",growth);PartyResourceService.ConfigureForAudit("player",1,mp,hp,mp);yield return EnterBattle();yield return Until(()=>Ready||Ended,"DefeatTurnReady");if(!Ended)Button("DefendButton").onClick.Invoke();yield return Until(()=>Ended,"DefeatReached");yield return Until(()=>SceneManager.GetActiveScene().name==Chapter3Main23Flow.Village&&Player!=null,"DefeatReturnsVillage");yield return Wait(.4f);Capture("Village_AfterDefeat");
        Check(EconomyService.GetCurrency()==77&&QuestService.GetState(Chapter3Main24Flow.QuestId)!=QuestState.Completed,"NoRewardOrMain24Completion");Check(errors.Count==0,"NoNewRuntimeErrors");
    }
}

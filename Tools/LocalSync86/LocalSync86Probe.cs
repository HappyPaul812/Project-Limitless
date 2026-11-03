using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ProjectLimitless.Battle;
using ProjectLimitless.Core;
using ProjectLimitless.Monster;
using ProjectLimitless.NPC;
using ProjectLimitless.Player;
using ProjectLimitless.UI;
using ProjectLimitless.World;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object=UnityEngine.Object;

/// <summary>기존 전수QA 대신 새로 합쳐진 경계만 확인합니다. 저장은 새 합성 슬롯으로만 격리합니다.</summary>
[DefaultExecutionOrder(-10000)]
public sealed class LocalSync86Probe:MonoBehaviour
{
    public static string Output=>Path.GetFullPath(Path.Combine(Application.dataPath,"../../Evidence"));
    public bool Done,Failed;IEnumerator routine;readonly List<string> checks=new List<string>(),errors=new List<string>();
    static PlayerController Player=>Object.FindAnyObjectByType<PlayerController>();
    static BattleSceneController Battle=>Object.FindAnyObjectByType<BattleSceneController>();
    static T Read<T>(object o,string field)=>(T)o.GetType().GetField(field,BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance).GetValue(o);
    static Formation Allies=>Read<Formation>(Battle,"allies");
    static Formation Enemies=>Read<Formation>(Battle,"enemies");
    static Combatant Hero=>Allies.Members.Single(x=>x.Id=="player");
    static bool Ended=>Read<bool>(Battle,"battleEnded");
    static bool Ready=>Battle!=null&&!Ended&&!Read<bool>(Battle,"actionPlaying")&&Read<Combatant>(Battle,"currentActor")?.IsPlayerControlled==true;
    static Button Button(string name)=>Object.FindObjectsByType<Button>(FindObjectsSortMode.None).Single(x=>x.name==name);
    static Main23Site Site(Main23Site.Kind kind)=>Object.FindObjectsByType<Main23Site>(FindObjectsSortMode.None).Single(x=>x.SiteKind==kind);
    void Awake(){DontDestroyOnLoad(gameObject);Directory.CreateDirectory(Output);GameSaveService.AuditSaveDirectory=Path.GetFullPath(Path.Combine(Application.dataPath,"../../SyntheticSaves"));UserSettingsService.BeginAudit(Path.GetFullPath(Path.Combine(Application.dataPath,"../../SyntheticSettings")));UserSettingsService.SetMuteAll(true);AudioListener.volume=0;Application.runInBackground=true;QualitySettings.vSyncCount=0;Application.targetFrameRate=60;InputSystem.settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;InputSystem.settings.editorInputBehaviorInPlayMode=InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;Application.logMessageReceived+=(text,stack,type)=>{if(type==LogType.Error||type==LogType.Exception||type==LogType.Assert){if(stack.Contains("UnityEditor.Search.SearchDatabase"))File.AppendAllText(Path.Combine(Output,"EnvironmentErrors.txt"),text+"\n"+stack);else errors.Add(text+"\n"+stack);}};}
    void Start(){routine=Flatten(Run());}
    void Update(){if(Done||routine==null)return;try{if(!routine.MoveNext())Finish(false);}catch(Exception e){errors.Add(e.ToString());Finish(true);}}
    void Finish(bool failed){Done=true;Failed=failed||errors.Count>0;File.WriteAllLines(Path.Combine(Output,"checks.txt"),checks);File.WriteAllLines(Path.Combine(Output,"Errors.txt"),errors);File.WriteAllText(Path.Combine(Output,"status.txt"),Failed?"FAIL":"PASS");}
    void Check(bool ok,string name){checks.Add((ok?"PASS ":"FAIL ")+name);File.WriteAllLines(Path.Combine(Output,"checks.txt"),checks);if(!ok)throw new Exception(name);}
    static IEnumerator Flatten(IEnumerator e){var stack=new Stack<IEnumerator>();stack.Push(e);while(stack.Count>0){var top=stack.Peek();if(!top.MoveNext()){stack.Pop();continue;}if(top.Current is IEnumerator nested)stack.Push(nested);else yield return top.Current;}}
    static IEnumerator Wait(float seconds){float end=Time.realtimeSinceStartup+seconds;while(Time.realtimeSinceStartup<end)yield return null;}
    IEnumerator Until(Func<bool> ok,string name,float seconds=16){float end=Time.realtimeSinceStartup+seconds;while(!ok()&&Time.realtimeSinceStartup<end)yield return null;Check(ok(),name);}
    static void At(Vector2 p){Player.transform.position=p;var rb=Player.GetComponent<Rigidbody2D>();rb.position=p;rb.linearVelocity=Vector2.zero;Physics2D.SyncTransforms();}
    static IEnumerator Load(string scene){var op=SceneManager.LoadSceneAsync(scene);while(!op.isDone)yield return null;yield return Wait(.6f);}
    static IEnumerator CloseDialogue(){var d=DialoguePresenter.Instance;for(int i=0;i<40&&d!=null&&d.IsOpen;i++){d.Advance();yield return Wait(.04f);}yield return Wait(.15f);}
    IEnumerator Choose(Main23Site.Kind kind,int choice){var site=Site(kind);At((Vector2)site.transform.position+Vector2.down*.5f);yield return Wait(.2f);Check(site.TryInteract(),"Choice."+kind);yield return Wait(.1f);var panel=Object.FindAnyObjectByType<Main21ChoicePanel>();Check(panel!=null,"ChoicePanel."+kind);panel.Select(choice);yield return Wait(.6f);}
    IEnumerator Travel(bool enter){Main24F2Traversal.ChooseTravel(enter);yield return Wait(.1f);Object.FindAnyObjectByType<Main21ChoicePanel>().Select(0);yield return Until(()=>SceneManager.GetActiveScene().name==(enter?Main24F2Traversal.SceneId:Chapter3Main23Flow.Village)&&Player!=null,enter?"Portal.VillageToF2":"Portal.F2ToVillage");yield return Wait(.5f);}
    void Capture(string name){var cam=Camera.main;if(cam==null)return;var canvases=Object.FindObjectsByType<Canvas>(FindObjectsSortMode.None).Where(c=>c.renderMode==RenderMode.ScreenSpaceOverlay).ToArray();var old=cam.targetTexture;var active=RenderTexture.active;var rt=RenderTexture.GetTemporary(1280,720,24);cam.targetTexture=rt;foreach(var c in canvases){c.renderMode=RenderMode.ScreenSpaceCamera;c.worldCamera=cam;c.planeDistance=1;}Canvas.ForceUpdateCanvases();cam.Render();RenderTexture.active=rt;var t=new Texture2D(1280,720,TextureFormat.RGBA32,false);t.ReadPixels(new Rect(0,0,1280,720),0,0);t.Apply();File.WriteAllBytes(Path.Combine(Output,name+".png"),t.EncodeToPNG());Destroy(t);foreach(var c in canvases){c.renderMode=RenderMode.ScreenSpaceOverlay;c.worldCamera=null;}cam.targetTexture=old;RenderTexture.active=active;RenderTexture.ReleaseTemporary(rt);}
    IEnumerator EnterBattle(){At(new Vector2(4.2f,-2));yield return Wait(.3f);Check(Object.FindAnyObjectByType<Main24FrostOwlCandidate>().TryEnter(),"Owl.EntryExistingService");yield return Until(()=>Battle!=null&&SceneManager.GetActiveScene().name=="Battle","Owl.ActualBattleScene");yield return Wait(.3f);}
    IEnumerator Run()
    {
        GameSessionData.Reset();GameSessionData.ConfigurePlayer(PlayerVisualType.Male,"Sync86 합성");GameSessionData.SelectPlayerPath("path.mobility");GameSessionData.SelectJob("guardian");GameSessionData.ConfigureProgress(14,0);EconomyService.AddCurrency(77);GameSaveService.SelectSlot(1);
        QuestService.ImportSaveData(new QuestProgressSaveData{CompletedQuestIds=new[]{Chapter3Main22Flow.QuestId,Chapter2Main21Flow.QuestId}});Check(QuestService.GetState(Chapter3Main24Flow.QuestId)==QuestState.Locked,"Main24.RequiresMain23");QuestService.TryStart(Chapter3Main23Flow.QuestId);
        var seed=QuestService.ExportSaveData();var main23=seed.ActiveQuests.Single(x=>x.QuestId==Chapter3Main23Flow.QuestId);for(int i=0;i<main23.Objectives.Length-1;i++)main23.Objectives[i].CurrentCount=i==1?2:1;seed.Main23ClueFlags=3;QuestService.ImportSaveData(seed);
        GameSessionData.SetPendingSpawnPoint(Chapter3Main23Flow.SafeSpawn);yield return Load(Chapter3Main23Flow.Village);yield return Choose(Main23Site.Kind.Checkpoint,0);yield return CloseDialogue();
        Check(Chapter3Main23Flow.Completed&&QuestService.GetState(Chapter3Main24Flow.QuestId)==QuestState.Available,"Main23CheckpointUnlocksMain24");
        yield return Choose(Main23Site.Kind.Skill,0);yield return CloseDialogue();Check(Chapter3Main24Flow.Step==1,"Main24SkillDialogueAfterMain23");
        var job=Resources.LoadAll<JobDefinition>("JobDefinitions").Single(x=>x.JobId=="guardian");string playerId=PartyResourceService.PlayerCharacterId;var basic=SkillLoadoutService.GetLibrary(playerId,job).First();SkillManagementPresenter.Instance.Open();yield return Wait(.2f);Check(Read<List<Button>>(SkillManagementPresenter.Instance,"slotButtons").Count==6,"ActualSixSlotUI");Capture("Integrated_SixSlots");SkillManagementPresenter.Instance.Close();yield return Wait(.2f);Check(SkillLoadoutService.TrySetSlot(playerId,job,5,basic.Id),"SlotSixSavedUsingExistingService");
        // 표시 문구만 새 합성 의뢰로 확인합니다. 실제 기존 퀘스트 보상/20여개 의뢰를 반복 검증하지 않습니다.
        var item=ItemCatalog.All.First();var objective=new QuestObjectiveDefinition();objective.ConfigureForAudit("sync86_ui_item","합성",QuestObjectiveType.CollectItem,item.ItemId);var side=ScriptableObject.CreateInstance<QuestDefinition>();side.ConfigureForAudit("sync86_ui_only","합성 UI",QuestType.Side,new[]{objective});side.ConfigureNpcFlow("sync86_npc");QuestCatalog.RegisterForAudit(side);QuestService.TryStart(side.QuestId);InventoryService.TryAddItem(item.ItemId,1);var npc=new GameObject("SyntheticQuestNpc").AddComponent<NpcController>();npc.Configure("합성 의뢰","합성");SideQuestNpcPresenter.ShowQuest("sync86_npc",npc,side);yield return Wait(.15f);Check(Object.FindObjectsByType<Text>(FindObjectsSortMode.None).Any(x=>x.text=="완료"),"QuestCompletionActualButtonLabel");Capture("Integrated_QuestCompleteLabel");DialoguePresenter.Instance.Hide();InventoryService.TryRemoveItem(item.ItemId,1);var clean=QuestService.ExportSaveData();clean.ActiveQuests=clean.ActiveQuests.Where(x=>x.QuestId!=side.QuestId).ToArray();QuestService.ImportSaveData(clean);Destroy(npc.gameObject);yield return Wait(.2f);
        var supply=Site(Main23Site.Kind.Supply);At(supply.transform.position);yield return Wait(.2f);Check(supply.TryInteract(),"Supply.GuidanceNoPurchase");yield return CloseDialogue();Check(Chapter3Main24Flow.Step==2,"Supply.AdvancesMain24WithoutPurchase");At(new Vector2(0,8.45f));yield return Wait(.4f);Check(Chapter3Main24Flow.Step==3,"NorthGateReady");yield return Travel(true);Check(Chapter3Main24Flow.Step==4,"F2ArrivalObjectiveRegistered");
        var travel=Object.FindAnyObjectByType<Main23MouseTravel>();Check(travel.GoTo(Site(Main23Site.Kind.F2Observe)),"IntegratedF2ExistingColliderRoute");yield return Until(()=>!travel.IsTravelling,"IntegratedF2RouteArrived",20);Check(Site(Main23Site.Kind.F2Observe).TryInteract(),"F2ObserveExistingInteraction");yield return CloseDialogue();Check(Chapter3Main24Flow.Step==5,"F2ObservationObjective");Capture("Integrated_F2");yield return Travel(false);At(new Vector2(0,8.45f));yield return Wait(.3f);Check(Chapter3Main24Flow.Step>=6,"VillageReturnObjective");
        string quest=JsonUtility.ToJson(QuestService.ExportSaveData());yield return Travel(true);yield return EnterBattle();var monster=BattleEncounterContext.Monster;Check(monster.ExplicitIdleFrames.Length==8&&monster.ExplicitAttackFrames.Length==4&&monster.AnimationFramesPerSecond==8,"ApprovedOwlFramesLoadedInBattleUI");
        yield return Until(()=>Ready,"Battle.FirstPlayerTurn");int drops=0,previous=Hero.CurrentHp;var observed=Hero;observed.HpChanged+=()=>{if(observed.CurrentHp<previous)drops++;previous=observed.CurrentHp;};Button("DefendButton").onClick.Invoke();yield return Until(()=>drops==1,"IntegratedOwlDamageOnce");yield return Until(()=>Ready,"IntegratedTurnReturns");Check(drops==1,"NoDuplicateDamageAfterTurn");
        int attacks=0;while(!Ended&&attacks++<8){yield return Until(()=>Ready||Ended,"IntegratedNextTurn");if(Ended)break;Button("AttackButton").onClick.Invoke();yield return Wait(.08f);var views=(IDictionary)Read<object>(Battle,"combatantViews");Read<Button>(views[Enemies.Members.Single()],"HitArea").onClick.Invoke();yield return Wait(.8f);}
        Check(Ended&&Hero.IsAlive,"IntegratedVictory");Capture("Integrated_OwlVictory");Button("VictoryReturn").onClick.Invoke();yield return Until(()=>SceneManager.GetActiveScene().name==Main24F2Traversal.SceneId&&Player!=null,"VictoryReturnsToF2");yield return Wait(.6f);
        Check(GameSaveService.SaveCurrentWorldPosition(Player.transform.position,Main24F2Traversal.SceneId,GameSessionData.LastSpawnPointId)&&GameSaveService.TryLoadSlot(1,out var saved),"SyntheticSaveReadable");GameSaveService.TryLoadSlot(1,out saved);GameSaveService.RestoreSession(1,saved);yield return Load(saved.CurrentSceneId);Check(SkillLoadoutService.GetSlots(playerId,job)[5]==basic.Id&&JsonUtility.ToJson(QuestService.ExportSaveData())==quest,"ContinuePreservesSlotSixAndMain24");
        yield return Wait(2.2f);var growth=CharacterGrowthCalculator.Calculate("guardian",14);int hp=CharacterGrowthCalculator.CalculateMaxHp("guardian",growth),mp=CharacterGrowthCalculator.CalculateMaxMp("guardian",growth);PartyResourceService.ConfigureForAudit("player",1,mp,hp,mp);yield return EnterBattle();yield return Until(()=>Ready||Ended,"LossExistingTurnRule");if(!Ended)Button("DefendButton").onClick.Invoke();yield return Until(()=>Ended,"IntegratedDefeat");Capture("Integrated_OwlDefeat");yield return Until(()=>SceneManager.GetActiveScene().name==Chapter3Main23Flow.Village&&Player!=null,"DefeatReturnsToSafeVillage");
        Check(EconomyService.GetCurrency()==77&&QuestService.GetState(Chapter3Main24Flow.QuestId)!=QuestState.Completed,"NoRewardNoPurchaseNoMain25Unlock");Check(errors.Count==0,"NoNewRuntimeErrors");
    }
}

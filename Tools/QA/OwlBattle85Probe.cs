using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ProjectLimitless.Battle;
using ProjectLimitless.Core;
using ProjectLimitless.Monster;
using ProjectLimitless.Player;
using ProjectLimitless.UI;
using ProjectLimitless.World;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object=UnityEngine.Object;

// 실제 Scene/Battle UI와 기존 Save 서비스를 사용하되 데이터·입력은 전부 이 fixture에서 새로 만듭니다.
[DefaultExecutionOrder(-10000)]
public sealed class OwlBattle85Probe:MonoBehaviour
{
    [Serializable] public class Frame { public float t; public string scene,animation,actor;public int playerHp,owlHp;public bool action; }
    [Serializable] public class Records { public List<Frame> frames=new List<Frame>();public List<string> events=new List<string>(); }
    public static string Output=>Path.GetFullPath(Path.Combine(Application.dataPath,"../../Evidence"));
    public bool Done,Failed;
    readonly List<string> checks=new List<string>(),errors=new List<string>();
    readonly HashSet<string> idleSeen=new HashSet<string>(),attackSeen=new HashSet<string>();
    readonly Records records=new Records();
    IEnumerator routine;
    string movie;
    float lastCapture;
    int movieFrame,playerDrops,owlDrops;
    int previousPlayerHp,previousOwlHp;
    string questSnapshot,beastSnapshot,inventorySnapshot;
    bool observedPlayerHit;
    Keyboard testKeyboard;
    Mouse testMouse;
    static T Read<T>(object obj,string field)=>(T)obj.GetType().GetField(field,BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance).GetValue(obj);
    static BattleSceneController Battle=>Object.FindAnyObjectByType<BattleSceneController>();
    static PlayerController Player=>Object.FindAnyObjectByType<PlayerController>();
    static Main24FrostOwlCandidate Site=>Object.FindAnyObjectByType<Main24FrostOwlCandidate>();
    static bool IsBattle=>SceneManager.GetActiveScene().name==BattleSceneFlow.BattleSceneName&&Battle!=null;
    static Formation Allies=>Read<Formation>(Battle,"allies");
    static Formation Enemies=>Read<Formation>(Battle,"enemies");
    static Combatant Hero=>Allies.Members.Single(x=>x.Id=="player");
    static Combatant Owl=>Enemies.Members.Single();
    static bool Busy=>Read<bool>(Battle,"actionPlaying");
    static bool Ended=>Read<bool>(Battle,"battleEnded");
    static Combatant Actor=>Read<Combatant>(Battle,"currentActor");
    static bool PlayerReady=>IsBattle&&!Ended&&!Busy&&Actor!=null&&Actor.IsPlayerControlled&&Read<Button>(Battle,"attackButton").interactable;
    static object View(Combatant value)=>((IDictionary)Read<object>(Battle,"combatantViews"))[value];
    static Image ImageOf(Combatant value)=>Read<Image>(View(value),"SpriteImage");
    static MonsterSpriteSheetAnimation OwlAnimation=>Read<MonsterSpriteSheetAnimation>(View(Owl),"MonsterAnimation");
    static Button Button(string name)=>Object.FindObjectsByType<Button>(FindObjectsSortMode.None).Single(x=>x.name==name);

    void Awake()
    {
        DontDestroyOnLoad(gameObject);Directory.CreateDirectory(Output);
        GameSaveService.AuditSaveDirectory=Path.GetFullPath(Path.Combine(Application.dataPath,"../../SyntheticSaves"));
        UserSettingsService.BeginAudit(Path.GetFullPath(Path.Combine(Application.dataPath,"../../SyntheticSettings")));UserSettingsService.SetMuteAll(true);
        Application.runInBackground=true;
        InputSystem.settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;
        InputSystem.settings.editorInputBehaviorInPlayMode=InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
        foreach(var device in InputSystem.devices.ToArray())if(device is Keyboard||device is Mouse)InputSystem.DisableDevice(device);
        testKeyboard=InputSystem.AddDevice<Keyboard>("Owl85VirtualKeyboard");testMouse=InputSystem.AddDevice<Mouse>("Owl85VirtualMouse");
        AudioListener.volume=0;Application.targetFrameRate=60;QualitySettings.vSyncCount=0;
        Application.logMessageReceived+=(text,stack,type)=>{
            if(type!=LogType.Exception&&type!=LogType.Error&&type!=LogType.Assert)return;
            if(text.Contains("ArgumentOutOfRangeException")&&stack.Contains("UnityEditor.Search.SearchDatabase"))File.AppendAllText(Path.Combine(Output,"EnvironmentErrors.txt"),text+"\n"+stack+"\n");
            else errors.Add(text+"\n"+stack);
        };
    }
    void Start(){routine=Flatten(Run());}
    void Update()
    {
        if(Done||routine==null)return;
        try{if(!routine.MoveNext())Finish(false);}
        catch(Exception exception){errors.Add(exception.ToString());Finish(true);}
    }
    void Finish(bool failed)
    {
        Failed=failed;Done=true;movie=null;
        File.WriteAllLines(Path.Combine(Output,"checks.txt"),checks);
        File.WriteAllLines(Path.Combine(Output,"Errors.txt"),errors);
        File.WriteAllText(Path.Combine(Output,"Trace.json"),JsonUtility.ToJson(records,true));
        File.WriteAllText(Path.Combine(Output,"status.txt"),failed?"FAIL":"PASS");
    }
    void Check(bool condition,string label)
    {checks.Add((condition?"PASS ":"FAIL ")+label);File.WriteAllLines(Path.Combine(Output,"checks.txt"),checks);if(!condition)throw new Exception(label);}
    static IEnumerator Flatten(IEnumerator first)
    {var stack=new Stack<IEnumerator>();stack.Push(first);while(stack.Count>0){var top=stack.Peek();if(!top.MoveNext()){stack.Pop();continue;}if(top.Current is IEnumerator nested){stack.Push(nested);continue;}yield return top.Current;}}
    static IEnumerator Wait(float seconds){float end=Time.realtimeSinceStartup+seconds;while(Time.realtimeSinceStartup<end)yield return null;}
    IEnumerator Until(Func<bool> condition,string label,float seconds=15)
    {float end=Time.realtimeSinceStartup+seconds;while(!condition()&&Time.realtimeSinceStartup<end)yield return null;Check(condition(),label);}
    IEnumerator Load(string scene){var op=SceneManager.LoadSceneAsync(scene);while(!op.isDone)yield return null;yield return Wait(.5f);}
    static void At(Vector2 position){Player.transform.position=position;Player.GetComponent<Rigidbody2D>().position=position;Player.GetComponent<Rigidbody2D>().linearVelocity=Vector2.zero;Physics2D.SyncTransforms();}
    IEnumerator WalkTo(Vector2 target,string label)
    {
        float end=Time.realtimeSinceStartup+8;
        while(Vector2.Distance(Player.transform.position,target)>.12f&&Time.realtimeSinceStartup<end){Player.SetGuidedMovement((target-(Vector2)Player.transform.position).normalized);yield return null;}
        Player.SetGuidedMovement(Vector2.zero);yield return Wait(.1f);Check(Vector2.Distance(Player.transform.position,target)<.2f,label);
    }
    IEnumerator Press(Key key){InputSystem.QueueStateEvent(testKeyboard,new KeyboardState(key));yield return Wait(.08f);InputSystem.QueueStateEvent(testKeyboard,new KeyboardState());yield return Wait(.12f);}
    IEnumerator ClickSite(){Vector2 point=Camera.main.WorldToScreenPoint(Site.transform.position);InputSystem.QueueStateEvent(testMouse,new MouseState{position=point,buttons=1});yield return Wait(.08f);InputSystem.QueueStateEvent(testMouse,new MouseState{position=point});yield return Wait(.12f);}
    IEnumerator ChooseNorth()
    {Main24F2Traversal.ChooseTravel(true);yield return Wait(.1f);Object.FindAnyObjectByType<Main21ChoicePanel>().Select(0);yield return Until(()=>SceneManager.GetActiveScene().name==Main24F2Traversal.SceneId&&Player!=null&&Site!=null,"F2.ExistingSceneLoaded");yield return Wait(.5f);}
    IEnumerator Approach(string mode)
    {At(new Vector2(0,-7));yield return WalkTo(new Vector2(0,-3),mode+".SouthTrail");yield return WalkTo(new Vector2(3,-1),mode+".Bend");yield return WalkTo(new Vector2(4.2f,-2),mode+".OwlApproach");Check(!Physics2D.OverlapCircleAll(Player.transform.position,.25f).Any(c=>!c.isTrigger&&!c.transform.IsChildOf(Player.transform)),mode+".NoStaticOverlap");}
    void SubscribeDamage()
    {
        previousPlayerHp=Hero.CurrentHp;previousOwlHp=Owl.CurrentHp;playerDrops=owlDrops=0;
        Hero.HpChanged+=()=>{int now=Hero.CurrentHp;if(now<previousPlayerHp)playerDrops++;records.events.Add("PlayerHP "+previousPlayerHp+"->"+now+" @"+Time.time);previousPlayerHp=now;};
        Owl.HpChanged+=()=>{int now=Owl.CurrentHp;if(now<previousOwlHp)owlDrops++;records.events.Add("OwlHP "+previousOwlHp+"->"+now+" @"+Time.time);previousOwlHp=now;};
    }
    bool HpBarMatches(Combatant c)
    {var row=((IDictionary)Read<object>(Battle,"combatantHpRows"))[c];var fill=Read<Image>(row,"HpFill");return Mathf.Abs(fill.rectTransform.anchorMax.x-(float)c.CurrentHp/c.MaxHp)<.001f;}
    void Capture(string name,int width=1280)
    {
        var camera=Camera.main;if(camera==null)return;
        var canvases=Object.FindObjectsByType<Canvas>(FindObjectsSortMode.None).Where(x=>x.renderMode==RenderMode.ScreenSpaceOverlay).ToArray();
        var prior=camera.targetTexture;var active=RenderTexture.active;var rt=RenderTexture.GetTemporary(width,720,24);
        camera.targetTexture=rt;foreach(var cv in canvases){cv.renderMode=RenderMode.ScreenSpaceCamera;cv.worldCamera=camera;cv.planeDistance=1;}
        Canvas.ForceUpdateCanvases();camera.Render();RenderTexture.active=rt;
        bool fieldFits=true;
        if(name.StartsWith("Battle_ActualUI_"))
        {
            var field=GameObject.Find("SideViewBattlefield").GetComponent<RectTransform>();var corners=new Vector3[4];field.GetWorldCorners(corners);var left=RectTransformUtility.WorldToScreenPoint(camera,corners[0]);var right=RectTransformUtility.WorldToScreenPoint(camera,corners[2]);fieldFits=left.x>=-1&&right.x<=width+1;
        }
        var png=new Texture2D(width,720,TextureFormat.RGBA32,false);png.ReadPixels(new Rect(0,0,width,720),0,0);png.Apply();string file=Path.Combine(Output,name+".png");Directory.CreateDirectory(Path.GetDirectoryName(file));File.WriteAllBytes(file,png.EncodeToPNG());Destroy(png);
        foreach(var cv in canvases){cv.renderMode=RenderMode.ScreenSpaceOverlay;cv.worldCamera=null;}camera.targetTexture=prior;RenderTexture.active=active;RenderTexture.ReleaseTemporary(rt);
        if(name.StartsWith("Battle_ActualUI_"))Check(fieldFits,"Battle.ViewportFieldWithinFrame."+width);
    }
    void LateUpdate()
    {
        if(!IsBattle)return;
        var animation=OwlAnimation;string sprite=animation.CurrentFrame?.name??"none";
        if(sprite.Contains("Idle"))idleSeen.Add(sprite);if(sprite.Contains("Attack"))attackSeen.Add(sprite);
        if(ImageOf(Hero).color.a<.9f)observedPlayerHit=true;
        records.frames.Add(new Frame{t=Time.time,scene=SceneManager.GetActiveScene().name,animation=sprite,actor=Actor?.Id,playerHp=Hero.CurrentHp,owlHp=Owl.CurrentHp,action=Busy});
        if(movie!=null&&Time.unscaledTime-lastCapture>=.05f){lastCapture=Time.unscaledTime;Capture(movie+"/"+(movieFrame++).ToString("0000"));}
    }
    void StartMovie(string name){movie=name;movieFrame=0;lastCapture=-1;}

    IEnumerator Run()
    {
        yield return Wait(.5f);
        GameSessionData.Reset();GameSessionData.ConfigurePlayer(PlayerVisualType.Male,"Owl85 합성 플레이");GameSessionData.SelectPlayerPath("path.mobility");GameSessionData.SelectJob("guardian");GameSessionData.ConfigureProgress(14,0);EconomyService.AddCurrency(77);GameSaveService.SelectSlot(1);
        QuestService.ImportSaveData(new QuestProgressSaveData{CompletedQuestIds=new[]{Chapter3Main23Flow.QuestId,Chapter3Main22Flow.QuestId,Chapter2Main21Flow.QuestId}});
        Check(QuestService.TryStart(Chapter3Main24Flow.QuestId),"Seed.Main24Active");var quests=QuestService.ExportSaveData();foreach(var item in quests.ActiveQuests.Single(x=>x.QuestId==Chapter3Main24Flow.QuestId).Objectives)if(item.ObjectiveId!="await_f2_opening")item.CurrentCount=1;QuestService.ImportSaveData(quests);
        GameSessionData.ActivateSafeZone("safezone_frostway_village",Chapter3Main23Flow.Village,Chapter3Main23Flow.SafeSpawn);GameSessionData.SetPendingSpawnPoint(Chapter3Main23Flow.SafeSpawn);
        yield return Load(Chapter3Main23Flow.Village);yield return ChooseNorth();
        Check(Object.FindObjectsByType<Main24FrostOwlCandidate>(FindObjectsSortMode.None).Length==1,"F2.SingleOptionalEncounter");
        Check(Site.GetComponentsInChildren<Collider2D>().Length==0,"F2.OwlDoesNotBlockPath");Check(Object.FindAnyObjectByType<Main24F2StumpDepth>()!=null,"F2.ApprovedStumpPreserved");
        yield return Approach("Wheelchair");Capture("F2_Wheelchair_Approach");
        questSnapshot=JsonUtility.ToJson(QuestService.ExportSaveData());beastSnapshot=JsonUtility.ToJson(BeastCompanionService.ExportSaveData());inventorySnapshot=JsonUtility.ToJson(new InventorySaveProxy{items=InventoryService.ExportSaveData()});
        yield return Press(Key.E);
        File.WriteAllText(Path.Combine(Output,"InputDiagnostics.txt"),"mode="+InputSystem.settings.updateMode+" keyboard="+testKeyboard.name+" enabled="+testKeyboard.enabled+" pressed="+testKeyboard.eKey.isPressed+" current="+Keyboard.current.name+" site="+Site.transform.position+" player="+Player.transform.position+" modal="+WorldModalState.IsOpen+" choosing="+Read<bool>(Site,"choosing")+" dialogue="+DialoguePresenter.Instance?.IsOpen+" can="+DialoguePresenter.Instance?.CanBeginInteractionThisFrame+" action="+Read<InputAction>(Site,"interact").enabled);
        Check(Object.FindAnyObjectByType<Main21ChoicePanel>()!=null,"F2.KeyboardE.Choice");yield return Press(Key.Escape);Check(!WorldModalState.IsOpen&&SceneManager.GetActiveScene().name==Main24F2Traversal.SceneId,"F2.CancelNoBattle");
        yield return ClickSite();Check(Object.FindAnyObjectByType<Main21ChoicePanel>()!=null,"F2.Mouse.Choice");Object.FindAnyObjectByType<Main21ChoicePanel>().Select(0);Check(!Site.TryEnter(),"F2.DoubleEntryBlocked");
        yield return Until(()=>IsBattle,"Battle.RealControllerCreated");yield return Wait(.3f);SubscribeDamage();
        Check(Enemies.Members.Count()==1&&Allies.Members.Any(),"Battle.ActualFormation");Check(Owl.MaxHp==Resources.Load<MonsterDefinition>("MonsterDefinitions/01_GrassSlime").MaxHp,"Battle.BorrowedNotOfficialStats");
        Check(OwlAnimation!=null&&ImageOf(Owl).GetComponent<Animator>()==null,"Battle.RealImageFramePlayer");
        Check(ImageOf(Owl).rectTransform.sizeDelta==new Vector2(136,112),"Battle.CommonDisplayScale1Preserved");Check(Mathf.Abs(ImageOf(Owl).sprite.pixelsPerUnit-512)<.01f,"Battle.PPU512");
        Check(BattleEncounterContext.Monster.AnimationFramesPerSecond==8&&BattleEncounterContext.Monster.ExplicitIdleFrames.Length==8&&BattleEncounterContext.Monster.ExplicitAttackFrames.Length==4,"Battle.Approved8And4Frames");
        yield return Until(()=>PlayerReady,"Battle.PlayerTurn");yield return Wait(1.1f);Check(idleSeen.Count==8,"Battle.Idle8ActuallyDisplayed");
        foreach(int width in new[]{1280,960,1680})Capture("Battle_ActualUI_"+width,width);
        var normalSize=ImageOf(Owl).rectTransform.sizeDelta;ImageOf(Owl).rectTransform.sizeDelta=new Vector2(40,40);Capture("Battle_ActualImage_40px");Check(ImageOf(Owl).rectTransform.sizeDelta==new Vector2(40,40),"Battle.SameImageActual40pxProbe");ImageOf(Owl).rectTransform.sizeDelta=normalSize;
        Check(!Chapter3Main24Flow.SiteTitle(Main23Site.Kind.F2Observe).Contains("전투 없음"),"F2.GuidanceMatchesOptionalBattle");
        int dropsBeforeDefend=playerDrops,hpBeforeDefend=Hero.CurrentHp;observedPlayerHit=false;attackSeen.Clear();
        StartMovie("VictoryBattle");Button("DefendButton").onClick.Invoke();
        yield return Until(()=>playerDrops==dropsBeforeDefend+1,"Battle.OwlDamageOnce");yield return Until(()=>PlayerReady,"Battle.OwlAttackCompleted");
        Check(playerDrops==dropsBeforeDefend+1&&Hero.CurrentHp<hpBeforeDefend,"Battle.PlayerHPChangedOnce");Check(HpBarMatches(Hero)&&observedPlayerHit,"Battle.GenericPlayerHitAndHpBar");Check(attackSeen.Count==4,"Battle.Attack4ActuallyDisplayed");
        int swings=0;
        while(!Ended&&swings<12)
        {
            yield return Until(()=>Ended||PlayerReady,"Battle.NextTurnReady");if(Ended)break;
            int before=owlDrops;Button("AttackButton").onClick.Invoke();yield return Wait(.08f);Read<Button>(View(Owl),"HitArea").onClick.Invoke();swings++;
            yield return Until(()=>owlDrops==before+1,"Battle.PlayerImpactOnce."+swings);yield return Wait(.8f);Check(owlDrops==before+1,"Battle.NoDuplicateImpact."+swings);Check(HpBarMatches(Owl),"Battle.OwlHpBar."+swings);
        }
        yield return Until(()=>Ended&&Object.FindObjectsByType<Button>(FindObjectsSortMode.None).Any(x=>x.name=="VictoryReturn"),"Battle.VictoryResult");
        Check(!Owl.IsAlive&&ImageOf(Owl).color.a<1,"Battle.GenericDefeatDim");Check(EconomyService.GetCurrency()==77&&GameSessionData.CurrentExperience==0,"Battle.NoUnapprovedReward");
        Check(JsonUtility.ToJson(QuestService.ExportSaveData())==questSnapshot&&JsonUtility.ToJson(BeastCompanionService.ExportSaveData())==beastSnapshot,"Battle.NoQuestOrPetUnlock");
        typeof(BattleSceneController).GetMethod("EndBattle",BindingFlags.Instance|BindingFlags.NonPublic).Invoke(Battle,new object[]{"duplicate",true});Check(EconomyService.GetCurrency()==77,"Battle.DuplicateVictoryGuard");
        Capture("Battle_VictoryResult");movie=null;Button("VictoryReturn").onClick.Invoke();
        yield return Until(()=>SceneManager.GetActiveScene().name==Main24F2Traversal.SceneId&&Player!=null,"Victory.ReturnF2");yield return Wait(.7f);
        Check(!Physics2D.OverlapCircleAll(Player.transform.position,.25f).Any(c=>!c.isTrigger&&!c.transform.IsChildOf(Player.transform)),"Victory.SafeReturnCollider");Check(Object.FindObjectsByType<Main24FrostOwlCandidate>(FindObjectsSortMode.None).Length==1,"Victory.SingleEncounterAfterReturn");
        At(new Vector2(4.2f,-2));Check(!Site.TryEnter(),"Victory.ReentryGracePeriod");yield return Wait(2.1f);
        Check(PartyResourceService.TryGet("player",out var beforeSave),"Continue.VictoryResourcesExist");
        Check(GameSaveService.SaveCurrentWorldPosition(Player.transform.position,Main24F2Traversal.SceneId,GameSessionData.LastSpawnPointId),"Continue.SaveF2");Check(GameSaveService.TryLoadSlot(1,out var saved),"Continue.ReadSyntheticSlot");GameSaveService.RestoreSession(1,saved);yield return Load(saved.CurrentSceneId);yield return Wait(.4f);
        Check(PartyResourceService.TryGet("player",out var afterContinue)&&afterContinue.CurrentHp==beforeSave.CurrentHp,"Continue.VictoryHPUnchanged");
        Check(Site!=null&&SceneManager.GetActiveScene().name==Main24F2Traversal.SceneId,"Continue.F2AndEncounter");Check(JsonUtility.ToJson(QuestService.ExportSaveData())==questSnapshot,"Continue.Main24Unchanged");Check(GameSessionData.LastSafeZoneSceneId==Chapter3Main23Flow.Village,"Continue.SafeCheckpoint");
        GameSessionData.SelectPlayerPath("path.vision");Player.GetComponent<PlayerPathVisualController>().Apply("path.vision");yield return Approach("Walking");Capture("F2_Walking_Approach");
        var growth=CharacterGrowthCalculator.Calculate("guardian",14);int maxHp=CharacterGrowthCalculator.CalculateMaxHp("guardian",growth),maxMp=CharacterGrowthCalculator.CalculateMaxMp("guardian",growth);PartyResourceService.ConfigureForAudit("player",1,maxMp,maxHp,maxMp);
        yield return Press(Key.F);Check(Object.FindAnyObjectByType<Main21ChoicePanel>()!=null,"F2.KeyboardF.Choice");Object.FindAnyObjectByType<Main21ChoicePanel>().Select(0);yield return Until(()=>IsBattle,"Repeat.BattleEntry");yield return Wait(.2f);SubscribeDamage();StartMovie("DefeatBattle");
        yield return Until(()=>Ended||PlayerReady,"Defeat.ExistingTurnOrder");
        if(!Ended){Check(Hero.CurrentHp==1,"Defeat.SyntheticHP1");Button("DefendButton").onClick.Invoke();}
        yield return Until(()=>Ended,"Defeat.ExistingRuleTriggered");Check(playerDrops==1&&!Hero.IsAlive,"Defeat.OneLethalImpact");Capture("Battle_Defeat");
        yield return Until(()=>SceneManager.GetActiveScene().name==Chapter3Main23Flow.Village&&Player!=null,"Defeat.ReturnExistingSafeVillage");movie=null;yield return Wait(.5f);
        Check(PartyResourceService.TryGet("player",out var healed)&&healed.CurrentHp==healed.MaxHp,"Defeat.ExistingFullHeal");Check(EconomyService.GetCurrency()==77&&JsonUtility.ToJson(QuestService.ExportSaveData())==questSnapshot,"Defeat.NoRewardOrQuestChange");
        yield return ChooseNorth();Check(Site!=null&&Chapter3Main24Flow.Step>=6&&QuestService.GetState(Chapter3Main24Flow.QuestId)!=QuestState.Completed,"Repeat.F2ExplorationReadyMain24NotComplete");
        Check(JsonUtility.ToJson(new InventorySaveProxy{items=InventoryService.ExportSaveData()})==inventorySnapshot,"Inventory.NoItemGrantOrPurchase");Capture("F2_AfterVictoryDefeat");
        Check(errors.Count==0,"NoNewCompileRuntimeErrors");
    }
    [Serializable] class InventorySaveProxy{public InventoryEntry[] items;}
}

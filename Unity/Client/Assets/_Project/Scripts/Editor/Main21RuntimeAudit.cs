using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ProjectLimitless.Core;
using ProjectLimitless.World;
using ProjectLimitless.Player;
using ProjectLimitless.NPC;
using ProjectLimitless.Monster;
using ProjectLimitless.Battle;
using ProjectLimitless.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object=UnityEngine.Object;
namespace ProjectLimitless.EditorTools
{
    /// <summary>원본 Editor와 Save를 사용하지 않는 Main21 전용 Batch QA입니다. 실제 대화/전투/Continue/귀환을 실행합니다.</summary>
    [InitializeOnLoad]
    public static class Main21RuntimeAudit
    {
        private const string Key="Limitless.Main21Audit";
        private static readonly BindingFlags Flags=BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance|BindingFlags.Static;
        private static string Root=>Path.Combine(Directory.GetCurrentDirectory(),"Main21Results");
        private static IEnumerator routine;private static double deadline;private static readonly List<string> results=new List<string>();
        private static readonly List<string> errors=new List<string>();private static bool expectSaveFailure;
        static Main21RuntimeAudit()
        {
            EditorApplication.playModeStateChanged+=state=>{
                if(!SessionState.GetBool(Key,false)||state!=PlayModeStateChange.EnteredPlayMode)return;
                GameSaveService.AuditSaveDirectory=Path.Combine(Root,"Saves");UserSettingsService.BeginAudit(Path.Combine(Root,"Settings"));UserSettingsService.SetMuteAll(true);
                Application.logMessageReceived+=Log;Application.runInBackground=true;routine=Flatten(Run());deadline=EditorApplication.timeSinceStartup+480;EditorApplication.update+=Tick;
            };
        }
        public static void RunBatch()
        {
            if(!Application.isBatchMode||!Application.dataPath.Replace('\\','/').Contains("/Temp/Main21QA/Client/Assets"))throw new InvalidOperationException("격리 Main21QA만 허용");
            Directory.CreateDirectory(Root);EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);SessionState.SetBool(Key,true);EditorApplication.EnterPlaymode();
        }
        private static void Check(bool value,string id)
        {results.Add((value?"PASS|":"FAIL|")+id);File.WriteAllLines(Path.Combine(Root,"results.txt"),results);if(!value)throw new InvalidOperationException(id);}
        private static T Get<T>(object obj,string name)=>(T)obj.GetType().GetField(name,Flags).GetValue(obj);
        private static object Call(object obj,string name,params object[] args)=>obj.GetType().GetMethod(name,Flags).Invoke(obj,args);
        private static void Set(object obj,string name,object value)=>obj.GetType().GetField(name,Flags).SetValue(obj,value);
        private static IEnumerator Wait(float seconds){float end=Time.realtimeSinceStartup+seconds;while(Time.realtimeSinceStartup<end)yield return null;}
        private static IEnumerator Scene(string name)
        {
            float end=Time.realtimeSinceStartup+35;while(SceneManager.GetActiveScene().name!=name&&Time.realtimeSinceStartup<end)yield return null;
            Check(SceneManager.GetActiveScene().name==name,"Scene."+name);yield return Wait(.5f);MonsterEncounterService.SuppressForSeconds(600);
            Check(SceneManager.GetActiveScene().GetRootGameObjects().Sum(g=>g.GetComponentsInChildren<Transform>(true).Sum(t=>GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(t.gameObject)))==0,"MissingScript0."+name);
        }
        private static void At(Vector2 position)
        {var p=Object.FindAnyObjectByType<PlayerController>();p.GetComponent<Rigidbody2D>().position=position;p.transform.position=position;}
        private static IEnumerator CloseDialogue()
        {
            int count=0;while(DialoguePresenter.Instance!=null&&DialoguePresenter.Instance.IsOpen&&count++<25)
            {DialoguePresenter.Instance.AdvanceFromInput();yield return Wait(.08f);}
            Check(DialoguePresenter.Instance==null||!DialoguePresenter.Instance.IsOpen,"Dialogue.Completed");yield return Wait(.05f);
        }
        private static void Seed(bool main20=true)
        {
            GameSessionData.Reset();GameSaveService.SelectSlot(1);GameSessionData.ConfigurePlayer(PlayerVisualType.Male,"격리 Main21 QA");
            GameSessionData.SelectJob("fighter");GameSessionData.SelectPlayerPath("path.hearing");GameSessionData.ConfigureProgress(15,0);
            GameSessionData.RecordLocation(Chapter2Main21Flow.Field,"Spawn_From_Field09");
            CompanionRosterService.UnlockIntroCompanions();CompanionRosterService.UnlockPaul("fighter");CompanionRosterService.UnlockSerin();
            Check(CompanionRosterService.TrySetComposition(new[]{CompanionRosterService.PaulId,CompanionRosterService.TaeonId},
                new Dictionary<string,FormationRow>{{"player",FormationRow.Front},{CompanionRosterService.PaulId,FormationRow.Rear},{CompanionRosterService.TaeonId,FormationRow.Front}}),"Party.FreeChoice.PaulTaeon");
            QuestService.ImportSaveData(new QuestProgressSaveData{CompletedQuestIds=QuestCatalog.All.Where(x=>x.QuestType==QuestType.Main
                &&x.QuestId!=Chapter2Main21Flow.QuestId&&(main20||x.QuestId!=Chapter2Main20Flow.QuestId)).Select(x=>x.QuestId).ToArray()});
        }
        private static void Index(int index)
        {
            QuestCatalog.TryGet(Chapter2Main21Flow.QuestId,out var quest);
            QuestService.ImportSaveData(new QuestProgressSaveData {Main21Flags=Main21Progress.Flags,
                CompletedQuestIds=QuestCatalog.All.Where(x=>x.QuestType==QuestType.Main&&x.QuestId!=Chapter2Main21Flow.QuestId).Select(x=>x.QuestId).ToArray(),
                ActiveQuests=new[]{new ActiveQuestSaveData{QuestId=quest.QuestId,Objectives=quest.Objectives.Select((x,i)=>new QuestObjectiveProgressData{ObjectiveId=x.ObjectiveId,CurrentCount=i<index?1:0}).ToArray()}}});
        }
        private static IEnumerator Interact(int index)
        {
            At(Chapter2Main21Flow.Positions[index]);yield return Wait(.12f);
            Check(GameObject.Find(Chapter2Main21Flow.Targets[index]).GetComponent<Main21Site>().TryInteract(),"Site."+index);
            yield return CloseDialogue();
        }
        private static IEnumerator Continue(string label)
        {
            string scene=SceneManager.GetActiveScene().name;var player=Object.FindAnyObjectByType<PlayerController>();
            string expected=JsonUtility.ToJson(QuestService.ExportSaveData());Vector2 pos=player.transform.position;
            Check(GameSaveService.SaveCurrentWorldPosition(pos,scene),"Continue.Write."+label);
            SceneManager.LoadSceneAsync("Bootstrap");yield return Scene("Bootstrap");GameSessionData.Reset();
            var button=GameObject.Find("StartMenuCanvas/Slot01/Action")?.GetComponent<UnityEngine.UI.Button>();Check(button!=null,"Continue.RealButton."+label);
            button.onClick.Invoke();yield return Scene(scene);
            if(label!="LoadedBeforeSpawn")Check(JsonUtility.ToJson(QuestService.ExportSaveData())==expected&&Vector2.Distance(Object.FindAnyObjectByType<PlayerController>().transform.position,pos)<.1f,"Continue.Exact."+label);
        }
        private static IEnumerator Battle(int index)
        {
            for(int attempt=0;attempt<3;attempt++)
            {
                At(Chapter2Main21Flow.Positions[index]);yield return Wait(.15f);
                Check(GameObject.Find(Chapter2Main21Flow.Targets[index]).GetComponent<Main21Site>().TryInteract(),"Battle.Enter."+index+"."+attempt);
                yield return Scene("Battle");var controller=Object.FindAnyObjectByType<BattleSceneController>();controller.StopAllCoroutines();Set(controller,"actionPlaying",false);
                var foes=Get<Formation>(controller,"enemies").Members.ToArray();var allies=Get<Formation>(controller,"allies").Members.ToArray();
                var mapping=Get<Dictionary<Combatant,BattleParticipantSetup>>(controller,"participantSetups");
                Check(allies.Length==3&&foes.Length==2,"Battle.ThreeVsTwo."+index);
                Check(foes.All(x=>mapping[x].MonsterDefinition!=null&&x.MaxHp==mapping[x].MonsterDefinition.MaxHp),"Battle.OriginalMonsterStats."+index);
                Check(index==2?foes[0].Slot.Row==FormationRow.Front&&foes[1].Slot.Row==FormationRow.Rear:foes.All(x=>x.Slot.Row==FormationRow.Front),"Battle.Formation."+index);
                int currency=EconomyService.GetCurrency();
                if(attempt==0)
                {
                    Call(controller,"Flee");yield return Scene(Chapter2Main21Flow.Field);
                    Check(Chapter2Main21Flow.Current(index)&&currency==EconomyService.GetCurrency(),"Battle.Flee.NoQuestReward."+index);
                }
                else if(attempt==1)
                {
                    foreach(var ally in allies)ally.TakeDamage(ally.MaxHp*10);
                    Call(controller,"EndBattle","격리 패배",false);yield return Scene(GameSessionData.LastSafeZoneSceneId);
                    Check(Chapter2Main21Flow.Current(index)&&currency==EconomyService.GetCurrency(),"Battle.Defeat.NoQuestReward."+index);
                    SceneManager.LoadSceneAsync(Chapter2Main21Flow.Field);yield return Scene(Chapter2Main21Flow.Field);
                }
                else
                {
                    foreach(var foe in foes)foe.TakeDamage(foe.MaxHp*10);
                    var expected=BattleVictoryReward.Calculate(true,GameSessionData.Level,foes,mapping);
                    Call(controller,"EndBattle","격리 승리",true);
                    Check(Chapter2Main21Flow.Current(index+1)&&EconomyService.GetCurrency()==currency+expected.Currency,"Battle.Victory.ObjectiveAndCurrency."+index);
                    int after=EconomyService.GetCurrency();Call(controller,"EndBattle","중복 승리",true);Check(EconomyService.GetCurrency()==after,"Battle.DuplicateReward0."+index);
                    Object.FindObjectsByType<UnityEngine.UI.Button>().Single(x=>x.name=="VictoryReturn").onClick.Invoke();yield return Scene(Chapter2Main21Flow.Field);
                    yield return Continue("Battle"+index);
                }
            }
        }
        private static int TotalExperience()=>Enumerable.Range(1,GameSessionData.Level-1).Sum(ExperienceProgression.RequiredExp)+GameSessionData.CurrentExperience;
        private static IEnumerator Run()
        {
            Seed(false);Check(!QuestService.TryStart(Chapter2Main21Flow.QuestId),"Main20Incomplete.Locked");
            Seed();SceneManager.LoadSceneAsync(Chapter2Main21Flow.Field);yield return Scene(Chapter2Main21Flow.Field);
            Check(Chapter2Main21Flow.Current(0),"Main20Complete.Field10.StartsMain21");
            // 실제 LOCAL 지형의 Collider를 읽어 조사 표식이 벽 내부나 출입 Trigger에 놓이지 않았는지 확인합니다.
            var fieldBounds=Object.FindAnyObjectByType<WorldBounds2D>();
            int point=0;
            foreach(var position in Chapter2Main21Flow.Positions.Take(7).Concat(Chapter2Main21Flow.CluePositions))
            {
                bool clear=Physics2D.OverlapCircleAll(position,.25f).All(x=>x.GetComponent<PlayerController>()!=null
                    ||x.isTrigger&&x.GetComponent<SceneTransitionTrigger>()==null);
                Check(fieldBounds!=null&&fieldBounds.Bounds.Contains(position)&&clear,"Field10.Geometry.SafeSite."+point++);
            }
            QuestCatalog.TryGet(Chapter2Main21Flow.QuestId,out var quest);Check(quest.Objectives.Count==13&&quest.Reward.Experience==100&&quest.Reward.Currency==60&&quest.Reward.Items.Length==0,"Quest.Contract13.Reward100_60_NoItem");
            // 실제 Path Catalog의 다섯 ID로 정보 접근과 최초 관찰자를 확인합니다.
            foreach(string path in new[]{"path.vision","path.hearing","path.intellectual","path.mobility","path.emotional-scar"})
            {
                GameSessionData.SelectPlayerPath(path);Check(Main21DialogueCatalog.Get(1).Length>=2&&Main21DialogueCatalog.Get(3).Length>=3,"Path.AllInformation."+path);
                if(path=="path.hearing")Check(Main21DialogueCatalog.Get(1)[0].IsPlayer,"Path.Hearing.PlayerFirst");
                if(path=="path.mobility")Check(Main21DialogueCatalog.Get(3)[0].IsPlayer,"Path.Mobility.PlayerFirst");
            }
            GameSessionData.SelectPlayerPath("path.hearing");
            for(int slot=2;slot<=5;slot++){GameSaveService.SelectSlot(slot);Check(GameSaveService.SaveCurrentSession(),"Slot5.Write."+slot);}GameSaveService.SelectSlot(1);
            var slots=Enumerable.Range(2,4).ToDictionary(x=>x,x=>File.ReadAllText(GameSaveService.GetSaveFilePath(x)));
            yield return Interact(0);Check(Chapter2Main21Flow.Current(1),"Sequential.1");yield return Interact(1);Check(Chapter2Main21Flow.Current(2),"Sequential.2");
            yield return Battle(2);yield return Interact(3);Check(Chapter2Main21Flow.Current(4),"Sequential.4");
            At(Chapter2Main21Flow.Positions[4]);yield return Wait(.1f);GameObject.Find(Chapter2Main21Flow.Targets[4]).GetComponent<Main21Site>().TryInteract();
            Check(!Main21Progress.AllClues&&Chapter2Main21Flow.Current(4),"Puzzle.MissingClues.NoProgress");DialoguePresenter.Instance.Hide();
            foreach(int clue in new[]{2,0,1})
            {At(Chapter2Main21Flow.CluePositions[clue]);yield return Wait(.1f);Check(GameObject.Find("main21_route_clue_"+clue).GetComponent<Main21Site>().TryInteract(),"Puzzle.Clue."+clue);yield return CloseDialogue();if(clue==0)yield return Continue("PartialClues");}
            Check(Main21Progress.AllClues,"Puzzle.Clues.AllRestored");string inventory=JsonUtility.ToJson(new InventorySnapshot{Entries=InventoryService.ExportSaveData()});int money=EconomyService.GetCurrency();
            for(int wrong=0;wrong<2;wrong++){Main21Site.ChoosePassage(wrong);Check(Chapter2Main21Flow.Current(4)&&EconomyService.GetCurrency()==money&&inventory==JsonUtility.ToJson(new InventorySnapshot{Entries=InventoryService.ExportSaveData()}),"Puzzle.Wrong.NoLoss."+wrong);DialoguePresenter.Instance.Hide();yield return Wait(.1f);}
            Main21Site.ChoosePassage(2);yield return CloseDialogue();Check(Chapter2Main21Flow.Current(5),"Puzzle.Correct.Once");Check(CompanionRosterService.TrySetComposition(new[]{CompanionRosterService.SerinId,CompanionRosterService.MielId},
                new Dictionary<string,FormationRow>{{"player",FormationRow.Front},{CompanionRosterService.SerinId,FormationRow.Rear},{CompanionRosterService.MielId,FormationRow.Rear}}),"Party.FreeChoice.SerinMiel");
            yield return Battle(5);yield return Interact(6);
            SceneManager.LoadSceneAsync("Arbel");yield return Scene("Arbel");var leon=Object.FindObjectsByType<VillageNpcRole>().Single(x=>x.NpcId=="arbel-leon");
            At((Vector2)leon.transform.position+Vector2.down*.7f);yield return Wait(.1f);
            // 실제 NPC가 서브 의뢰 선택창을 먼저 열면 기존 이야기/업무를 선택해 메인 보고로 들어갑니다.
            leon.Interact(leon.GetComponent<NpcController>());
            yield return Wait(.1f); // 창을 연 입력으로 선택까지 처리하지 않는 기존 프레임 보호를 지킵니다.
            if(SideQuestNpcPresenter.Instance!=null&&SideQuestNpcPresenter.Instance.IsOpen)
            {
                Object.FindObjectsByType<UnityEngine.UI.Button>().Single(x=>x.GetComponentsInChildren<UnityEngine.UI.Text>()
                    .Any(t=>t.text=="기존 이야기 / 상점·업무")).onClick.Invoke();
            }
            Check(DialoguePresenter.Instance.IsOpen,"Leon.ExistingNpc.Report");yield return CloseDialogue();Check(Chapter2Main21Flow.Current(8),"Sequential.8");
            inventory=JsonUtility.ToJson(new InventorySnapshot{Entries=InventoryService.ExportSaveData()});yield return Interact(8);Check(Chapter2Main21Flow.Current(8),"Aid.InspectThenDeliver");yield return Interact(8);
            Check(Chapter2Main21Flow.Current(9)&&inventory==JsonUtility.ToJson(new InventorySnapshot{Entries=InventoryService.ExportSaveData()}),"Aid.Deliver.NoInventoryConsumption");
            At(new Vector2(-2.2f,-1));yield return Wait(.1f);GameObject.Find(Chapter2Main21Flow.Targets[9]).GetComponent<Main21Site>().TryInteract();yield return CloseDialogue();
            Object.FindAnyObjectByType<Main21ChoicePanel>().Select(1);yield return Wait(.1f);Check(!Main21Progress.ReturnAccepted&&Chapter2Main21Flow.Current(9),"Carriage.No.Reoffer");
            Check(Main21CarriageReturnController.Begin()&&!Main21CarriageReturnController.Begin(),"Carriage.Yes.DuplicateAcceptanceBlocked");
            Check(Main21Progress.ReturnAccepted&&PlayerController.IsMovementLocked,"Carriage.CheckpointAndInputLock");
            var ride=Object.FindAnyObjectByType<Main21CarriageReturnController>();yield return Wait(.2f);ride.Skip();ride.Skip();yield return Scene("World_StarterVillage");yield return Wait(.5f);
            Check(Chapter2Main21Flow.Current(10)&&!PlayerController.IsMovementLocked&&!WorldModalState.IsOpen,"Carriage.Skip.Arrival.ObjectiveAndInputRestore");
            Check(GameSessionData.LastSpawnPointId==Chapter2Main21Flow.ArrivalSpawn&&Vector2.Distance(Object.FindAnyObjectByType<PlayerController>().transform.position,Chapter2Main21Flow.Positions[9])<.1f,"Carriage.Spawn.ActualVerified");
            Check(Object.FindObjectsByType<VillageNpcRole>().Count(x=>x.NpcId!="")>=12,"Village.Npc12.Preserved");yield return Continue("CarriageArrived");Main21VillageAuditChecks.Validate();
            // 도착 요청이 없는 수동 마을 진입은 완료로 오인하지 않습니다.
            Index(9);GameSessionData.RecordLocation("World_StarterVillage","Spawn_From_Field01");GameSessionData.ClearPendingSpawnPoint();
            yield return Chapter2Main21Flow.RecoverArrival();Check(Chapter2Main21Flow.Current(9),"Carriage.ManualVillageEntry.NoCompletion");
            // Player가 늦게 나타나는 실제 coroutine 경계를 검사합니다.
            var latePlayer=Object.FindAnyObjectByType<PlayerController>();latePlayer.gameObject.SetActive(false);
            GameSessionData.SetPendingSpawnPoint(Chapter2Main21Flow.ArrivalSpawn);
            Object.FindAnyObjectByType<Chapter2Main21Flow>().StartCoroutine(Chapter2Main21Flow.RecoverArrival());
            yield return Wait(.4f);Check(Chapter2Main21Flow.Current(9),"Carriage.DelayedSpawn.NoEarlyCompletion");
            latePlayer.gameObject.SetActive(true);yield return Wait(.5f);Check(Chapter2Main21Flow.Current(10),"Carriage.DelayedSpawn.Recovered");
            Index(9);GameSessionData.RecordLocation("World_StarterVillage",Chapter2Main21Flow.ArrivalSpawn);GameSessionData.ClearPendingSpawnPoint();At(new Vector2(0,-5));
            yield return Continue("LoadedBeforeSpawn");Check(Chapter2Main21Flow.Current(10)&&Vector2.Distance(Object.FindAnyObjectByType<PlayerController>().transform.position,Chapter2Main21Flow.Positions[9])<.1f,"Carriage.LoadedBeforeSpawn.Recovered");
            Index(9);SceneManager.LoadSceneAsync("Arbel");yield return Scene("Arbel");At(new Vector2(-2.2f,-1));yield return Wait(.1f);
            int oldFlags=Main21Progress.Flags;string directory=GameSaveService.AuditSaveDirectory;
            string blocked=Path.Combine(Root,"BlockedSaveDirectory");File.WriteAllText(blocked,"QA fault injection");
            GameSaveService.AuditSaveDirectory=blocked;expectSaveFailure=true;
            bool rejected=!Main21CarriageReturnController.Begin();expectSaveFailure=false;GameSaveService.AuditSaveDirectory=directory;
            Check(rejected&&Main21Progress.Flags==oldFlags,"Carriage.SaveFailure.NoDeparture");
            DialoguePresenter.Instance.Hide();GameSaveService.SelectSlot(1);
            Check(Main21CarriageReturnController.Begin(),"Carriage.LoadFailure.Begin");
            // 이미 로드 중인 공용 전환에 의해 실제 귀환 요청이 거절되는 실패 분기를 주입합니다.
            typeof(SceneTransitionService).GetField("isLoading",Flags).SetValue(null,true);
            Object.FindAnyObjectByType<Main21CarriageReturnController>().CompleteReturnOnce();
            typeof(SceneTransitionService).GetField("isLoading",Flags).SetValue(null,false);
            yield return Wait(.1f);DialoguePresenter.Instance.Hide();
            Check(Chapter2Main21Flow.Current(9)&&SceneManager.GetActiveScene().name=="Arbel"
                &&!WorldModalState.IsOpen&&!PlayerController.IsMovementLocked,"Carriage.LoadFailure.NoProgress.InputRestored");
            Check(Main21CarriageReturnController.Begin(),"Carriage.AcceptBeforeContinue");
            Object.Destroy(Object.FindAnyObjectByType<Main21CarriageReturnController>().gameObject);yield return Wait(.1f);yield return Continue("AcceptedAtArbel");
            Check(Main21Progress.ReturnAccepted&&Chapter2Main21Flow.Current(9)&&Object.FindAnyObjectByType<Main21CarriageReturnController>()==null,"Carriage.Continue.NoAutoDeparture");
            float beginTime=Time.realtimeSinceStartup;Check(Main21CarriageReturnController.Begin(),"Carriage.NormalBegin");yield return Wait(1.5f);
            CaptureCarriage();yield return Wait(.3f);
            yield return Scene("World_StarterVillage");yield return Wait(.2f);
            Check(Chapter2Main21Flow.Current(10)&&Time.realtimeSinceStartup-beginTime>=12&&Time.realtimeSinceStartup-beginTime<20,"Carriage.Normal12Seconds.SameArrival");
            Check(!PlayerController.IsMovementLocked&&!WorldModalState.IsOpen,"Carriage.Normal.InputRestored");
            foreach(var site in Object.FindObjectsByType<Main21Site>())
                Check(Physics2D.OverlapCircleAll(site.transform.position,.25f).All(x=>x.isTrigger||x.GetComponentInParent<PlayerController>()!=null),"Village.Site.Safe."+site.Index);
            yield return Interact(10);string rosterBefore=JsonUtility.ToJson(CompanionRosterService.ExportSaveData());yield return Interact(11);
            Check(Chapter2Main21Flow.Current(12)&&rosterBefore==JsonUtility.ToJson(CompanionRosterService.ExportSaveData()),"Haren.StoryOnly.NoPartyChange");
            int currencyBefore=EconomyService.GetCurrency(),experienceBefore=TotalExperience();yield return Interact(12);
            Check(Chapter2Main21Flow.Completed&&EconomyService.GetCurrency()==currencyBefore+60&&TotalExperience()==experienceBefore+100,"Chapter2.Complete.RewardExactlyOnce");
            QuestService.NotifyInteraction(Chapter2Main21Flow.Targets[12]);Check(EconomyService.GetCurrency()==currencyBefore+60,"Quest.DuplicateCompletion0");
            Check(QuestService.ActiveMainQuest==null&&SceneManager.GetActiveScene().name=="World_StarterVillage","NoMain22.NoSceneTransition");yield return Continue("Completed");
            Check(slots.All(x=>File.ReadAllText(GameSaveService.GetSaveFilePath(x.Key))==x.Value),"OtherSlots2to5.Unchanged");
            Check(!SceneTransitionService.TryLoad("main21_missing_scene","missing"),"LoadFailure.Preflight.NoLock");
            File.WriteAllLines(Path.Combine(Root,"console_errors.txt"),errors);results.Add(errors.Count==0?"PASS|Console.Errors0":"FAIL|Console.Errors."+errors.Count);
            File.WriteAllLines(Path.Combine(Root,"results.txt"),results);File.WriteAllText(Path.Combine(Root,"status.txt"),errors.Count==0?"PASS":"PARTIAL_CONSOLE_ERRORS");
        }
        private static void CaptureCarriage()
        {
            // Overlay UI를 격리 카메라의 RenderTexture에만 잠시 연결하고 모든 설정을 즉시 복원합니다.
            var canvas=Object.FindAnyObjectByType<Main21CarriageReturnController>().GetComponent<Canvas>();
            var camera=Camera.main;var mode=canvas.renderMode;var worldCamera=canvas.worldCamera;float distance=canvas.planeDistance;
            var previousTarget=camera.targetTexture;float aspect=camera.aspect;var active=RenderTexture.active;
            var texture=RenderTexture.GetTemporary(1280,720,24,RenderTextureFormat.ARGB32);
            try
            {
                canvas.renderMode=RenderMode.ScreenSpaceCamera;canvas.worldCamera=camera;canvas.planeDistance=1;
                camera.aspect=1280f/720;camera.targetTexture=texture;Canvas.ForceUpdateCanvases();camera.Render();RenderTexture.active=texture;
                var image=new Texture2D(1280,720,TextureFormat.RGBA32,false);image.ReadPixels(new Rect(0,0,1280,720),0,0);image.Apply();
                File.WriteAllBytes(Path.Combine(Root,"carriage_runtime.png"),image.EncodeToPNG());Object.Destroy(image);
            }
            finally{canvas.renderMode=mode;canvas.worldCamera=worldCamera;canvas.planeDistance=distance;camera.targetTexture=previousTarget;camera.aspect=aspect;RenderTexture.active=active;RenderTexture.ReleaseTemporary(texture);}
            Check(File.Exists(Path.Combine(Root,"carriage_runtime.png")),"Carriage.RenderTexture.Captured");
        }
        [Serializable]private sealed class InventorySnapshot{public InventoryEntry[] Entries;}
        private static void Log(string message,string stack,LogType type)
        {
            if(expectSaveFailure&&type==LogType.Error&&message.StartsWith("슬롯 1을 쓰지 못했습니다.",StringComparison.Ordinal)){results.Add("EXPECTED_ERROR|SaveFaultInjection|"+message);return;}
            if(type==LogType.Error||type==LogType.Exception||type==LogType.Assert)errors.Add(message+"\n"+stack);
        }
        private static IEnumerator Flatten(IEnumerator root)
        {var stack=new Stack<IEnumerator>();stack.Push(root);while(stack.Count>0){var it=stack.Peek();if(!it.MoveNext()){stack.Pop();continue;}if(it.Current is IEnumerator nested)stack.Push(nested);else yield return null;}}
        private static void Tick()
        {try{if(EditorApplication.timeSinceStartup>deadline)throw new TimeoutException("Main21 QA timeout");if(routine.MoveNext())return;EditorApplication.update-=Tick;SessionState.SetBool(Key,false);EditorApplication.Exit(0);}catch(Exception e){results.Add("FAIL|"+e);File.WriteAllLines(Path.Combine(Root,"results.txt"),results);File.WriteAllText(Path.Combine(Root,"status.txt"),"FAIL");Debug.LogException(e);EditorApplication.Exit(1);}}
    }
}

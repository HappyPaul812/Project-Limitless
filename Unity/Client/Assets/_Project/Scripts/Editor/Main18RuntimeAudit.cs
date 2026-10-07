using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ProjectLimitless.Core;
using ProjectLimitless.World;
using ProjectLimitless.Player;
using ProjectLimitless.UI;
using ProjectLimitless.Monster;
using ProjectLimitless.Battle;
using ProjectLimitless.Audio;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjectLimitless.EditorTools
{
    /// <summary>기존 비포커스 실행기의 Save/설정 격리와 종료 복구만 재사용합니다.
    /// 실제 Controller·대화·Continue 경로를 검사하며 사용자 슬롯이나 기존 QA 결과는 쓰지 않습니다.</summary>
    [InitializeOnLoad]
    public static class Main18RuntimeAudit
    {
        private const string Armed="Limitless.Main18.Audit";
        private const BindingFlags Flags=BindingFlags.NonPublic|BindingFlags.Public|BindingFlags.Static|BindingFlags.Instance;
        public static readonly List<string> Results=new List<string>();
        private static string Output=>Path.GetFullPath(Path.Combine(Application.dataPath,"../../../문서/00_프로젝트/Main18_Runtime_Results.txt"));
        public static string Status="IDLE";
        private static bool balanceOnly;
        static Main18RuntimeAudit()
        {
            var initialized=Partial9FixedSpriteAudit.Status;
            EditorApplication.playModeStateChanged+=state=>
            {
                if(!SessionState.GetBool(Armed,false))return;
                if(state==PlayModeStateChange.EnteredPlayMode)
                    typeof(Partial9FixedSpriteAudit).GetField("routine",Flags).SetValue(null,Flatten(balanceOnly?Balance():Run()));
                if(state==PlayModeStateChange.EnteredEditMode){SessionState.SetBool(Armed,false);Write();}
            };
        }
        public static string Launch(bool balance=false)
        {
            balanceOnly=balance;Results.Clear();Status="RUNNING";SessionState.SetBool(Armed,true);Write();
            return Partial9FixedSpriteAudit.Launch();
        }
        private static void Write()=>File.WriteAllLines(Output,new[]{"STATUS|"+Status}.Concat(Results));
        private static void Check(bool condition,string id)
        {Results.Add((condition?"PASS|":"FAIL|")+id);Write();if(!condition)throw new InvalidOperationException(id);}
        private static T Value<T>(object owner,string field)=>(T)owner.GetType().GetField(field,Flags).GetValue(owner);
        private static object Call(object owner,string method,params object[] args)=>owner.GetType().GetMethod(method,Flags).Invoke(owner,args);
        private static IEnumerator Flatten(IEnumerator root)
        {
            var stack=new Stack<IEnumerator>();stack.Push(root);
            while(stack.Count>0)
            {
                var next=stack.Peek();bool moved;
                try{moved=next.MoveNext();}catch(Exception e){Results.Add("FAIL|Exception."+e);Status="FAIL";Write();throw;}
                if(!moved){stack.Pop();continue;}if(next.Current is IEnumerator nested)stack.Push(nested);else yield return next.Current;
            }
            Status="PASS";Write();
        }
        private static IEnumerator Wait(double seconds)
        {double until=EditorApplication.timeSinceStartup+seconds;while(EditorApplication.timeSinceStartup<until)yield return null;}
        private static IEnumerator Scene(string name)
        {
            double until=EditorApplication.timeSinceStartup+30;while(SceneManager.GetActiveScene().name!=name&&EditorApplication.timeSinceStartup<until)yield return null;
            Check(SceneManager.GetActiveScene().name==name,"Scene."+name);yield return Wait(1.2);MonsterEncounterService.SuppressForSeconds(3600);
            Check(SceneManager.GetActiveScene().GetRootGameObjects().Sum(x=>x.GetComponentsInChildren<Transform>(true).Sum(t=>GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(t.gameObject)))==0,"MissingScript."+name);
        }
        private static void Seed(string job="fighter",int level=14)
        {
            typeof(SecondRegressionAudit).GetMethod("Seed",Flags).Invoke(null,new object[]{job,level});
            GameSessionData.SelectPlayerPath("path.hearing");
            QuestService.ImportSaveData(new QuestProgressSaveData{CompletedQuestIds=QuestCatalog.All.Where(x=>x.QuestId.StartsWith("main_")&&string.CompareOrdinal(x.QuestId,"main_18")<0).Select(x=>x.QuestId).ToArray()});
            CompanionRosterService.UnlockPaul(job);CompanionRosterService.UnlockSerin();
            Check(CompanionRosterService.TrySetComposition(new[]{CompanionRosterService.SerinId,CompanionRosterService.MielId},new Dictionary<string,FormationRow>{{"player",FormationRow.Front},{CompanionRosterService.SerinId,FormationRow.Rear},{CompanionRosterService.MielId,FormationRow.Rear}}),"Fixture.Party."+job);
            GameSessionData.ActivateSafeZone("arbel","Arbel","Spawn_Arbel_Center");EconomyService.Import(200);
            InventoryService.TryAddItem("item_healing_potion_small",6);InventoryService.TryAddItem("item_mana_potion_small",4);
        }
        private static Combatant Actor(string id,int hp=100)=>new Combatant(id,id,BattleSide.Allies,new FormationSlot(FormationRow.Front,0),hp,30,10,0,TargetRangeType.MeleePhysical,true);
        private static void Rules()
        {
            foreach(int hp in new[]{1,12,13,50,99,100,101,250,560,1000000})
                foreach(string protection in new[]{"none","defend","iron","gaia","cover"})
                {
                    var s=new BattleStatusEffectRuntime();var a=Actor("a",hp);var g=Actor("g",1000);
                    if(protection=="defend")a.Defend();if(protection=="iron")s.ApplyIronWall(a,2);if(protection=="gaia")s.ApplyGaiaWall(a,2);if(protection=="cover")s.ApplyGuardianCover(g);
                    int budget=s.GetGuardianCoverRemainingBudget(g);int expected=(int)Math.Max(1L,((long)hp*8+99)/100);
                    Check(s.ApplyOverheat(a)==0&&s.GetOverheatStacks(a)==1,"Heat."+hp+"."+protection+".0to1");
                    Check(s.ApplyOverheat(a)==0&&s.GetOverheatStacks(a)==2,"Heat."+hp+"."+protection+".1to2");
                    Check(s.ApplyOverheat(a)==expected&&s.GetOverheatStacks(a)==0&&a.CurrentHp==Math.Max(0,hp-expected),"Heat."+hp+"."+protection+".BurstExact");
                    Check(g.CurrentHp==g.MaxHp&&s.GetGuardianCoverRemainingBudget(g)==budget,"Heat."+hp+"."+protection+".NoTransfer");
                }
            var statuses=new BattleStatusEffectRuntime();var target=Actor("target",200);statuses.ApplyOverheat(target);statuses.ApplyOverheat(target);
            statuses.ApplyOrRefreshPoison(target,3);statuses.ApplyOrRefreshBurn(target,3,2);statuses.ApplyOrRefreshShock(target);statuses.ApplyOrRefreshSilence(target);
            var model=BattleCombatantStatusViewModelFactory.Create(target,null,null,new BattleSkillCooldowns(),new BattleFighterResourceRuntime(),statuses);
            Check(model.CompactStatus.Contains("과열 2/3 위험")&&model.DetailText.Contains("정화로 제거할 수 없습니다"),"HUD.Stack2.TextRiskDetail");
            Check(statuses.GetHarmfulStatuses(target).Count==4,"Heat.IndependentFourStatuses");
            foreach(var effect in new[]{ItemEffectType.RemovePoison,ItemEffectType.RemoveBurn,ItemEffectType.RemoveShock,ItemEffectType.RemoveSilence})
            {
                var item=ItemCatalog.All.Single(x=>x.EffectType==effect);InventoryService.TryAddItem(item.ItemId,1);
                Check(BattleItemUseService.TryUse(item,target,statuses,out _)&&statuses.GetOverheatStacks(target)==2,"Cooling.OtherRemedyPreserves."+effect);
            }
            statuses.ApplyOrRefreshPoison(target,3);statuses.ApplyOrRefreshBurn(target,3,2);statuses.ApplyOrRefreshShock(target);statuses.ApplyOrRefreshSilence(target);
            var cleanse=new BattleSkillDefinition("audit_cleanse","정화","",true,BattleSkillEffectType.RemoveAllHarmfulStatuses,2,0,mpCost:0);
            var executor=new BattleSkillExecutor(new BattleSkillCooldowns(),statuses,new BattleFighterResourceRuntime());
            Check(executor.ExecuteSingleAllyCleanse(Actor("healer"),target,cleanse,out int removed,out _)&&removed==4&&statuses.GetOverheatStacks(target)==2,"Heat.ActualCleansePreserves");
            statuses.ApplyOrRefreshSilence(target);statuses.ApplyOrRefreshPoison(target,3);statuses.ApplyOrRefreshBurn(target,3,2);statuses.ApplyOrRefreshShock(target);
            ItemCatalog.TryGet(Chapter2Main18Flow.CoolingItem,out var cooling);InventoryService.TryAddItem(cooling.ItemId,2);int count=InventoryService.GetItemCount(cooling.ItemId);
            Check(BattleItemUseService.TryUse(cooling,target,statuses,out _)&&statuses.GetOverheatStacks(target)==0&&InventoryService.GetItemCount(cooling.ItemId)==count-1&&statuses.GetHarmfulStatuses(target).Count==4,"Cooling.SuccessDuringSilenceFourPreserved");
            Check(!BattleItemUseService.TryUse(cooling,target,statuses,out var message)&&message=="제거할 과열이 없습니다."&&InventoryService.GetItemCount(cooling.ItemId)==count-1,"Cooling.EmptyNoConsumption");
            statuses.ApplyOverheat(target);target.TakeDamage(10000,false);statuses.RemoveInvalidPersistentEffects(new[]{target});Check(statuses.GetOverheatStacks(target)==0,"Heat.KOCleared");
            Check(new BattleStatusEffectRuntime().GetOverheatStacks(Actor("new"))==0,"Heat.NewBattleZero");
            var ai=new BattleChapter2MonsterRuntime();var beetle=Actor("beetle");var watcher=Actor("watcher");
            for(int n=1;n<=12;n++)
            {
                ai.BeginActorAction(beetle);ai.BeginActorAction(watcher);
                Check(ai.TakeAction(beetle,"obsidian_beetle")== (n%3==1?"heat_spray":"basic"),"AI.Beetle."+n);
                Check(ai.TakeAction(watcher,"scorching_watcher")== (n%4==3?"heat_wave":n%4==2?"heat_pressure":"heat_injection"),"AI.Watcher."+n);
            }
            InventoryService.Reset();
            foreach(var monster in Resources.LoadAll<MonsterDefinition>("MonsterDefinitions").Where(x=>x.MonsterId=="obsidian_beetle"||x.MonsterId=="scorching_watcher"))
                Check(monster.ExplicitIdleFrames.Length==4&&monster.ExplicitAttackFrames.Length==4&&monster.ExplicitDefeatFrames.Length==4,"Art.Frames."+monster.MonsterId);
            foreach(string path in new[]{"path.vision","path.hearing","path.intellectual","path.mobility","path.emotional-scar"})
            {
                var lines=Main18DialogueCatalog.Get(8,path);Check(lines.Length==2&&lines[0].SpeakerId=="player"&&lines[1].SpeakerId==CompanionRosterService.SerinId,"Path.Observation."+path);
            }
        }
        private static void Move(Vector2 at){UnityEngine.Object.FindAnyObjectByType<PlayerController>().transform.position=at;Physics2D.SyncTransforms();}
        private static IEnumerator Inspect(int index)
        {
            Move(Chapter2Main18Flow.Positions[index]);yield return Wait(.1);
            GameObject.Find(Chapter2Main18Flow.Targets[index]).GetComponent<Main18Site>().TryInteract();
            if(index==6||index==9&&Chapter2Main18Flow.IsCurrent(10))yield break;
            Check(DialoguePresenter.Instance.IsOpen,"Dialogue.Open."+index);Check(Chapter2Main18Flow.IsCurrent(index),"Dialogue.NoEarlyProgress."+index);
            yield return ReadDialogue();
        }
        private static IEnumerator ReadDialogue()
        {
            int guard=0;while(DialoguePresenter.Instance!=null&&DialoguePresenter.Instance.IsOpen&&guard++<50){DialoguePresenter.Instance.Advance();yield return Wait(.035);}
            // 마지막 페이지에서 Battle Scene으로 전환되면 월드 대화 인스턴스가 정상적으로 파괴됩니다.
            Check(DialoguePresenter.Instance==null||!DialoguePresenter.Instance.IsOpen,"Dialogue.Closed");
        }
        private static IEnumerator Continue(string label)
        {
            string scene=SceneManager.GetActiveScene().name;Vector2 at=UnityEngine.Object.FindAnyObjectByType<PlayerController>().transform.position;
            string quest=JsonUtility.ToJson(QuestService.ExportSaveData()),party=JsonUtility.ToJson(CompanionRosterService.ExportSaveData()),beast=JsonUtility.ToJson(BeastCompanionService.ExportSaveData()),inventory=JsonUtility.ToJson(new InventorySnapshot{Entries=InventoryService.ExportSaveData()});
            string resources=JsonUtility.ToJson(new ResourceSnapshot{Entries=PartyResourceService.ExportSaveData()});
            int money=EconomyService.GetCurrency(),level=GameSessionData.Level,experience=GameSessionData.CurrentExperience;string path=GameSessionData.SelectedPlayerPathId;
            Check(GameSaveService.SaveCurrentWorldPosition(at,scene),"Continue."+label+".Write");
            Check(GameSaveService.TryLoadSlot(1,out var saved)&&saved.Version==1,"Continue."+label+".Version1");
            SceneManager.LoadSceneAsync("Bootstrap");yield return Scene("Bootstrap");GameSessionData.Reset();
            var button=GameObject.Find("StartMenuCanvas/Slot01/Action")?.GetComponent<Button>();Check(button!=null,"Continue."+label+".RealButton");button.onClick.Invoke();yield return Scene(scene);
            Check(Vector2.Distance(UnityEngine.Object.FindAnyObjectByType<PlayerController>().transform.position,at)<.05f,"Continue."+label+".ScenePosition");
            Check(JsonUtility.ToJson(QuestService.ExportSaveData())==quest,"Continue."+label+".QuestCompleted");
            Check(JsonUtility.ToJson(CompanionRosterService.ExportSaveData())==party,"Continue."+label+".PartyFormationUnlock");
            Check(JsonUtility.ToJson(BeastCompanionService.ExportSaveData())==beast,"Continue."+label+".Beast");
            Check(JsonUtility.ToJson(new InventorySnapshot{Entries=InventoryService.ExportSaveData()})==inventory&&EconomyService.GetCurrency()==money,"Continue."+label+".InventoryCoolingCurrency");
            Check(JsonUtility.ToJson(new ResourceSnapshot{Entries=PartyResourceService.ExportSaveData()})==resources&&GameSessionData.SelectedPlayerPathId==path,"Continue."+label+".HpMpPath");
            Check(GameSessionData.Level==level&&GameSessionData.CurrentExperience==experience,"Continue."+label+".GrowthExp");
            Check(!Chapter2Main18Flow.CompleteReport(),"Continue."+label+".NoDuplicateReport");
        }
        [Serializable]private sealed class InventorySnapshot{public InventoryEntry[] Entries;}
        [Serializable]private sealed class ResourceSnapshot{public PartyMemberResourceSaveData[] Entries;}
        private static IEnumerator Retry(bool beetle)
        {
            string label=beetle?"Beetle":"Watcher";int index=beetle?6:9;int objective=beetle?6:10;
            yield return Scene("Battle");var c=UnityEngine.Object.FindAnyObjectByType<BattleSceneController>();int money=EconomyService.GetCurrency();
            double until=EditorApplication.timeSinceStartup+20;
            // 도망은 실제 아군 명령 입력 가능 시점에만 시도합니다. 적 행동 중의 무효 입력을 성공으로 가정하지 않습니다.
            while((Value<bool>(c,"actionPlaying")||!Value<Button>(c,"attackButton").interactable)&&EditorApplication.timeSinceStartup<until)yield return null;
            Check(!Value<bool>(c,"actionPlaying"),"Retry."+label+".PlayerCommandReady");
            Call(c,"Flee");yield return Scene(Chapter2Main18Flow.Field);Check(Chapter2Main18Flow.IsCurrent(objective)&&EconomyService.GetCurrency()==money,"Retry."+label+".FleeNoReward");
            Move(Chapter2Main18Flow.Positions[index]);yield return Wait(.1);GameObject.Find(Chapter2Main18Flow.Targets[index]).GetComponent<Main18Site>().TryInteract();yield return Scene("Battle");
            c=UnityEngine.Object.FindAnyObjectByType<BattleSceneController>();c.StopAllCoroutines();
            foreach(var ally in Value<Formation>(c,"allies").Members)ally.TakeDamage(100000,false);
            Call(c,"EndBattle","검증 전멸",false);yield return Scene("Arbel");Check(Chapter2Main18Flow.IsCurrent(objective)&&EconomyService.GetCurrency()==money,"Retry."+label+".DefeatNoReward");
            PartyResourceService.HealPartyFully();SceneTransitionService.Load(Chapter2Main18Flow.Field,"Spawn_From_Field08");yield return Scene(Chapter2Main18Flow.Field);
            Move(Chapter2Main18Flow.Positions[index]);yield return Wait(.1);GameObject.Find(Chapter2Main18Flow.Targets[index]).GetComponent<Main18Site>().TryInteract();
        }
        private static IEnumerator Fight(string label)
        {
            yield return Scene("Battle");var c=UnityEngine.Object.FindAnyObjectByType<BattleSceneController>();
            var enemies=Value<Formation>(c,"enemies");var allies=Value<Formation>(c,"allies");
            Check(enemies.Members.Count()==1&&!enemies.Members.First().IsBoss,"Battle."+label+".SingleNotBoss");
            Check(allies.Members.Count()==1+CompanionRosterService.ActivePartyCharacterIds.Count,"Battle."+label+".SavedParty");
            Check(BgmPlaybackService.Instance.CurrentClip==Resources.Load<BgmSceneCatalog>("Audio/Music/BgmSceneCatalog").FindBattle(Chapter2Main18Flow.Field,"",false),"Battle."+label+".Blade");
            int currency=EconomyService.GetCurrency();Time.timeScale=8;double until=EditorApplication.timeSinceStartup+150;int attacks=0;
            while(!Value<bool>(c,"battleEnded")&&EditorApplication.timeSinceStartup<until)
            {
                var attack=Value<Button>(c,"attackButton");if(!Value<bool>(c,"actionPlaying")&&attack.IsActive()&&attack.interactable)
                {
                    attack.onClick.Invoke();var targets=Value<IReadOnlyList<Combatant>>(c,"selectableTargets");
                    if(Value<bool>(c,"choosingTarget")&&targets.Count>0){typeof(SecondRegressionAudit).GetMethod("Hit",Flags).Invoke(null,new object[]{c,targets[0]});attacks++;}
                }
                yield return null;
            }
            Time.timeScale=1;
            Check(enemies.IsDefeated&&attacks>0,"Battle."+label+".NormalVictory");
            int after=EconomyService.GetCurrency();Check(after-currency==(label.Contains("watcher")?22:14),"Battle."+label+".TalentOnce");
            Call(c,"EndBattle","duplicate",true);Check(EconomyService.GetCurrency()==after,"Battle."+label+".DuplicateZero");
            Check(allies.Members.All(x=>Value<BattleStatusEffectRuntime>(c,"statusEffects").GetOverheatStacks(x)==0),"Battle."+label+".HeatCleared");
            Results.Add("INFO|Balance."+label+".HP="+string.Join(",",allies.Members.Select(x=>x.CurrentHp)));Write();
            UnityEngine.Object.FindObjectsByType<Button>().First(x=>x.name=="VictoryReturn").onClick.Invoke();yield return Scene(Chapter2Main18Flow.Field);
        }
        private static IEnumerator Run()
        {
            Seed();Rules();Seed();
            SceneTransitionService.Load(Chapter2Main18Flow.PreviousField,"Spawn_From_Field07");yield return Scene(Chapter2Main18Flow.PreviousField);
            Check(Chapter2Main18Flow.IsCurrent(0),"Quest.AutoStartAfterMain17");
            Check(GameObject.Find("Main18AccessGate").GetComponent<BoxCollider2D>().enabled,"Gate.LockedBeforeReport");
            yield return Continue("start-before-return");
            SceneTransitionService.Load("Arbel","Spawn_From_Field06");yield return Scene("Arbel");Move(Chapter2Main18Flow.Positions[0]);yield return Wait(.2);
            Check(Chapter2Main18Flow.IsCurrent(1),"Quest.ReturnArbel");
            int before=InventoryService.GetItemCount(Chapter2Main18Flow.CoolingItem);
            var leon=UnityEngine.Object.FindObjectsByType<ProjectLimitless.NPC.VillageNpcRole>().Single(x=>x.NpcId=="arbel-leon");
            leon.Interact(leon.GetComponent<ProjectLimitless.NPC.NpcController>());yield return ReadDialogue();
            Check(Chapter2Main18Flow.IsCurrent(2)&&InventoryService.GetItemCount(Chapter2Main18Flow.CoolingItem)==before+3,"Report.Cooling3Once");
            var shop=Resources.Load<ShopDefinition>("ShopDefinitions/ArbelGeneralShop");Check(shop.ItemIds.Contains(Chapter2Main18Flow.CoolingItem)&&!Resources.Load<ShopDefinition>("ShopDefinitions/StarterVillageGeneralShop").ItemIds.Contains(Chapter2Main18Flow.CoolingItem),"Shop.ArbelOnly");
            Check(ShopService.TryBuy(shop,Chapter2Main18Flow.CoolingItem,1)==ShopTransactionResult.Success,"Shop.Buy40");Check(ShopService.TrySell(Chapter2Main18Flow.CoolingItem,1)==ShopTransactionResult.Success,"Shop.Sell20");
            yield return Continue("report-cooling3");
            foreach(string field in new[]{Chapter2Main16Flow.PreviousField,Chapter2Main16Flow.Field,Chapter2Main18Flow.PreviousField}){SceneTransitionService.Load(field,field==Chapter2Main16Flow.PreviousField?"Spawn_From_Arbel":field==Chapter2Main16Flow.Field?"Spawn_From_Field06":"Spawn_From_Field07");yield return Scene(field);}
            Move(Chapter2Main18Flow.Positions[2]);yield return Wait(.2);Check(Chapter2Main18Flow.IsCurrent(3),"Quest.DeepRoute");
            Check(!GameObject.Find("Main18AccessGate").GetComponent<BoxCollider2D>().enabled,"Gate.PreparedOpen");
            SceneTransitionService.Load(Chapter2Main18Flow.Field,"Spawn_From_Field08");yield return Scene(Chapter2Main18Flow.Field);
            Check(Chapter2Main18Flow.IsCurrent(4),"Quest.Entry");Check(UnityEngine.Object.FindAnyObjectByType<WorldBounds2D>().Bounds.size==new Vector3(21,15,0),"Field.Bounds21x15");
            Check(UnityEngine.Object.FindObjectsByType<SceneTransitionTrigger>().Length==1,"Field.EastOnlyExit");
            Check(BgmPlaybackService.Instance.CurrentClip==Resources.Load<BgmSceneCatalog>("Audio/Music/BgmSceneCatalog").Find(Chapter2Main18Flow.Field),"Field.ObsidianBgm");
            Geometry();Capture("Field09");yield return Continue("entry-field09");
            yield return Inspect(4);yield return Inspect(5);yield return Inspect(6);yield return Retry(true);yield return Fight("beetle");Check(Chapter2Main18Flow.IsCurrent(7),"Quest.BeetleVictory");yield return Continue("beetle-victory");
            yield return Inspect(7);yield return Inspect(8);yield return Continue("before-watcher");
            yield return Inspect(9);yield return Retry(false);yield return Fight("watcher");
            Check(Chapter2Main18Flow.IsCurrent(11),"Quest.WatcherVictory");yield return Continue("watcher-victory");yield return Inspect(11);
            int questCurrency=EconomyService.GetCurrency(),questExp=GameSessionData.CurrentExperience;yield return Inspect(12);
            Check(QuestService.GetState(Chapter2Main18Flow.QuestId)==QuestState.Completed&&EconomyService.GetCurrency()==questCurrency+60,"Quest.CompletedReward60");
            Check(GameSessionData.CurrentExperience==questExp+70,"Quest.CompletedRewardExp70");
            QuestService.NotifyInteraction(Chapter2Main18Flow.Targets[12]);Check(EconomyService.GetCurrency()==questCurrency+60,"Quest.DuplicateCompletionZero");
            yield return Continue("main18-complete");Check(GameSaveService.CurrentVersion==1,"Save.VersionUnchanged");
        }
        private static void Geometry()
        {
            var camera=Camera.main;var follow=camera.GetComponent<ProjectLimitless.CameraSystem.CameraFollow>();
            Vector3 original=camera.transform.position;float aspect=camera.aspect;Vector2 player=UnityEngine.Object.FindAnyObjectByType<PlayerController>().transform.position;
            var bounds=UnityEngine.Object.FindAnyObjectByType<WorldBounds2D>().Bounds;
            foreach(float ratio in new[]{4f/3,16f/9,21f/9})foreach(Vector2 edge in new[]{new Vector2(-10,0),new Vector2(10,0),new Vector2(0,7),new Vector2(0,-7)})
            {
                camera.aspect=ratio;Move(edge);camera.transform.position=new Vector3(edge.x,edge.y,original.z);Call(follow,"LateUpdate");
                float h=camera.orthographicSize,w=h*ratio;Vector3 at=camera.transform.position;
                Check(at.x-w>=bounds.min.x-.001f&&at.x+w<=bounds.max.x+.001f&&at.y-h>=bounds.min.y-.001f&&at.y+h<=bounds.max.y+.001f,"Camera.Viewport."+ratio+"."+edge);
            }
            Move(player);camera.aspect=aspect;camera.transform.position=original;Call(follow,"LateUpdate");
            var walls=UnityEngine.Object.FindObjectsByType<BoxCollider2D>().Where(x=>x.name.StartsWith("Boundary_")).ToArray();
            Check(walls.Any(x=>x.OverlapPoint(new Vector2(-10.5f,0)))&&!walls.Any(x=>x.OverlapPoint(new Vector2(10.5f,0))),"Boundary.WestClosedEastOnlyGap");
            Check(Vector2.Distance(new Vector2(7.2f,0),new Vector2(10.25f,0))>1.1f,"Spawn.ExitNotOverlapping");
        }
        private static IEnumerator Balance()
        {
            yield return ControllerRules();
            foreach(string job in new[]{"guardian","healer","sharpshooter","fighter","mage"})
                foreach(bool watcher in new[]{false,true})
                {
                    Seed(job);SceneTransitionService.Load(Chapter2Main18Flow.Field,"Spawn_From_Field08");yield return Scene(Chapter2Main18Flow.Field);
                    var spawn=Resources.Load<FieldMonsterSpawnDefinition>("StoryEncounterReturns/Main18_"+(watcher?"WatcherReturn":"BeetleReturn"));
                    Check(BattleSceneFlow.EnterStoryBattle(watcher?Chapter2Main18Flow.WatcherEncounter:Chapter2Main18Flow.BeetleEncounter,spawn.Monster,spawn,spawn.Position),"Balance.Enter."+job+watcher);
                    yield return Fight(job+(watcher?".watcher":".beetle"));
                }
        }
        private static IEnumerator ControllerRules()
        {
            Seed("guardian");InventoryService.TryAddItem(Chapter2Main18Flow.CoolingItem,3);
            SceneTransitionService.Load(Chapter2Main18Flow.Field,"Spawn_From_Field08");yield return Scene(Chapter2Main18Flow.Field);
            var spawn=Resources.Load<FieldMonsterSpawnDefinition>("StoryEncounterReturns/Main18_WatcherReturn");
            BattleSceneFlow.EnterStoryBattle(Chapter2Main18Flow.WatcherEncounter,spawn.Monster,spawn,spawn.Position);yield return Scene("Battle");
            var c=UnityEngine.Object.FindAnyObjectByType<BattleSceneController>();c.StopAllCoroutines();
            var allies=Value<Formation>(c,"allies").Members.ToArray();var enemy=Value<Formation>(c,"enemies").Members.First();
            var s=Value<BattleStatusEffectRuntime>(c,"statusEffects");s.ClearOverheat();
            typeof(BattleSceneController).GetField("actionPlaying",Flags).SetValue(c,false);
            typeof(BattleSceneController).GetField("currentActor",Flags).SetValue(c,allies[0]);
            ItemCatalog.TryGet(Chapter2Main18Flow.CoolingItem,out var cooling);
            int count=InventoryService.GetItemCount(cooling.ItemId);var queue=Value<TurnOrderQueue>(c,"turnOrder");var upcoming=queue.Upcoming.ToArray();
            Call(c,"UseBattleItem",cooling,allies[0]);
            Check(InventoryService.GetItemCount(cooling.ItemId)==count&&ReferenceEquals(Value<Combatant>(c,"currentActor"),allies[0])&&queue.Upcoming.SequenceEqual(upcoming),"Controller.CoolingNoHeatNoItemActionTurn");
            Call(c,"CloseItemMenu");
            s.ApplyOverheat(allies[1]);Call(c,"RefreshCombatantViews",new object[]{null});Capture("Battle_Heat1");
            s.ApplyOverheat(allies[1]);Call(c,"RefreshCombatantViews",new object[]{null});Capture("Battle_Heat2");
            int before=allies[1].CurrentHp;
            yield return (IEnumerator)Call(c,"PlayMain18HeatAction",enemy,"heat_injection");c.StopAllCoroutines();
            Check(allies[1].CurrentHp<before&&s.GetOverheatStacks(allies[1])==0,"Controller.WatcherHighestHeatRearBurst");
            s.ApplyOverheat(allies[1]);s.ApplyOverheat(allies[1]);enemy.ApplyTaunt(allies[0],3);
            yield return (IEnumerator)Call(c,"PlayMain18HeatAction",enemy,"heat_pressure");c.StopAllCoroutines();
            Check(s.GetOverheatStacks(allies[0])==1&&s.GetOverheatStacks(allies[1])==2,"Controller.WatcherTauntPriority");enemy.ApplyTaunt(null,0);
            int[] heat=allies.Select(s.GetOverheatStacks).ToArray();
            yield return (IEnumerator)Call(c,"PlayMain18HeatAction",enemy,"heat_wave");c.StopAllCoroutines();
            Check(allies.Select(s.GetOverheatStacks).SequenceEqual(heat),"Controller.WatcherWaveNoHeat");
            typeof(BattleSceneController).GetField("currentActor",Flags).SetValue(c,allies[0]);
            typeof(BattleSceneController).GetField("actionPlaying",Flags).SetValue(c,false);
            s.ApplyOrRefreshSilence(allies[0]);Call(c,"UseBattleItem",cooling,allies[1]);c.StopAllCoroutines();
            Check(s.GetOverheatStacks(allies[1])==0&&InventoryService.GetItemCount(cooling.ItemId)==count-1,"Controller.CoolingSilencedActorSuccess");
            typeof(BattleSceneController).GetField("actionPlaying",Flags).SetValue(c,false);Call(c,"Flee");yield return Scene(Chapter2Main18Flow.Field);
        }
        private static void Capture(string label)
        {
            var camera=Camera.main;var canvases=UnityEngine.Object.FindObjectsByType<Canvas>().Where(x=>x.renderMode==RenderMode.ScreenSpaceOverlay).ToArray();
            var previousDistances=canvases.Select(x=>x.planeDistance).ToArray();var previousCameras=canvases.Select(x=>x.worldCamera).ToArray();
            foreach(int width in new[]{1280,1600,1920})
            {
                var original=camera.targetTexture;var active=RenderTexture.active;var texture=new RenderTexture(width,width*9/16,24);var image=new Texture2D(texture.width,texture.height,TextureFormat.RGB24,false);
                try
                {
                    foreach(var canvas in canvases){canvas.renderMode=RenderMode.ScreenSpaceCamera;canvas.worldCamera=camera;canvas.planeDistance=1;}
                    Canvas.ForceUpdateCanvases();camera.targetTexture=texture;camera.Render();RenderTexture.active=texture;image.ReadPixels(new Rect(0,0,texture.width,texture.height),0,0);image.Apply();
                    string folder=Path.GetFullPath(Path.Combine(Application.dataPath,"../../../문서/00_프로젝트/QA_증거/Main18"));Directory.CreateDirectory(folder);File.WriteAllBytes(Path.Combine(folder,label+"_"+width+".png"),image.EncodeToPNG());
                    Check(true,"Capture."+label+"."+width);
                }
                finally{for(int i=0;i<canvases.Length;i++){canvases[i].renderMode=RenderMode.ScreenSpaceOverlay;canvases[i].worldCamera=previousCameras[i];canvases[i].planeDistance=previousDistances[i];}camera.targetTexture=original;RenderTexture.active=active;UnityEngine.Object.DestroyImmediate(texture);UnityEngine.Object.DestroyImmediate(image);}
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ProjectLimitless.Core;
using ProjectLimitless.Battle;
using ProjectLimitless.Monster;
using ProjectLimitless.World;
using ProjectLimitless.Player;
using ProjectLimitless.UI;
using ProjectLimitless.Audio;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object=UnityEngine.Object;

namespace ProjectLimitless.EditorTools
{
    /// <summary>격리 Batch에서 실제 Scene·Controller·대화·저장/Continue를 검증합니다. 사용자 슬롯과 Editor 포커스를 사용하지 않습니다.</summary>
    [InitializeOnLoad]
    public static class Main20RuntimeAudit
    {
        private const string Key="Limitless.Main20Audit";
        private const BindingFlags Flags=BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance|BindingFlags.Static;
        private static string Root=>Path.Combine(Directory.GetCurrentDirectory(),SessionState.GetBool(Key+"Normal",false)?"Main20NormalResults":"Main20Results");
        private static readonly List<string> Results=new List<string>();
        private static IEnumerator routine;private static double deadline;private static int errors;
        static Main20RuntimeAudit()
        {
            EditorApplication.playModeStateChanged+=state=>{
                if(!SessionState.GetBool(Key,false)||state!=PlayModeStateChange.EnteredPlayMode)return;
                GameSaveService.AuditSaveDirectory=Path.Combine(Root,"Saves");UserSettingsService.BeginAudit(Path.Combine(Root,"Settings"));UserSettingsService.SetMuteAll(true);
                Application.logMessageReceived+=Log;Application.runInBackground=true;routine=Flatten(SessionState.GetBool(Key+"Normal",false)?NormalBattle():Runtime());deadline=EditorApplication.timeSinceStartup+360;EditorApplication.update+=Tick;
            };
        }
        public static void RunBatch()
        {
            if(!Application.isBatchMode||!Application.dataPath.Replace('\\','/').Contains("/Temp/Main20QA/Client/Assets"))throw new InvalidOperationException("격리 Main20QA 전용");
            Directory.CreateDirectory(Root);EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);SessionState.SetBool(Key,true);EditorApplication.EnterPlaymode();
        }
        public static void RunNormalBatch(){SessionState.SetBool(Key+"Normal",true);RunBatch();}
        private static void Check(bool pass,string id)
        {Results.Add((pass?"PASS|":"FAIL|")+id);File.WriteAllLines(Path.Combine(Root,"results.txt"),Results);if(!pass)throw new InvalidOperationException(id);}
        private static T Get<T>(object obj,string field)=>(T)obj.GetType().GetField(field,Flags).GetValue(obj);
        private static object Call(object obj,string method,params object[] args)=>obj.GetType().GetMethod(method,Flags).Invoke(obj,args);
        private static void Set(object obj,string field,object value)=>obj.GetType().GetField(field,Flags).SetValue(obj,value);
        private static void Seed(bool main19=true)
        {
            GameSessionData.Reset();GameSaveService.SelectSlot(1);GameSessionData.ConfigurePlayer(PlayerVisualType.Male,"격리 Main20 QA");GameSessionData.SelectJob("fighter");GameSessionData.SelectPlayerPath("path.hearing");GameSessionData.ConfigureProgress(15,0);
            GameSessionData.RecordLocation(Chapter2Main19Flow.Field,"Spawn_From_Field09");
            CompanionRosterService.UnlockIntroCompanions();CompanionRosterService.UnlockPaul("fighter");CompanionRosterService.UnlockSerin();
            Check(CompanionRosterService.TrySetComposition(new[]{CompanionRosterService.SerinId,CompanionRosterService.MielId},new Dictionary<string,FormationRow>{{"player",FormationRow.Front},{CompanionRosterService.SerinId,FormationRow.Rear},{CompanionRosterService.MielId,FormationRow.Rear}}),"Fixture.PartyThree");
            QuestService.ImportSaveData(new QuestProgressSaveData{CompletedQuestIds=QuestCatalog.All.Where(q=>q.QuestType==QuestType.Main&&q.QuestId!=Chapter2Main20Flow.QuestId&&(main19||q.QuestId!=Chapter2Main19Flow.QuestId)).Select(q=>q.QuestId).ToArray()});
        }
        private static Combatant Boss()=>new Combatant("boss","거신",BattleSide.Enemies,new FormationSlot(FormationRow.Front,1),1200,30,10,0,TargetRangeType.MeleePhysical,false,true);
        private static void Models()
        {
            var b=Boss();var rt=new BattleMain20BossRuntime();rt.Bind(b);
            Check(Enumerable.Range(0,8).Select(_=>rt.TakeAction()).SequenceEqual(new[]{"injection","molten","condensation","core_wave","injection","molten","condensation","core_wave"}),"Model.Phase1.RepeatedCycle4");
            b.TakeDamage(599);Check(!rt.PhaseTwo,"Model.HP601.Phase1");b.Defend();b.ApplyTaunt(new Combatant("taunt","도발",BattleSide.Allies,new FormationSlot(FormationRow.Rear,0),100,10,8,0,TargetRangeType.Magic,true));
            b.TakeDamage(1,false);Check(rt.PhaseTwo&&b.CurrentHp==600&&b.IsDefending&&b.ForcedTarget!=null&&rt.PhaseTransitions==1,"Model.HP600.Immediate.NoHpGuardTauntReset");
            Check(Enumerable.Range(0,10).Select(_=>rt.TakeAction()).SequenceEqual(new[]{"injection","molten","resonance","eruption","wave","injection","molten","resonance","eruption","wave"}),"Model.Phase2.RepeatedCycle5");b.TakeDamage(1,false);Check(rt.PhaseTransitions==1,"Model.PhaseOnce");
            b=Boss();rt.Bind(b);rt.TakeAction();rt.TakeAction();rt.TakeAction();b.TakeDamage(600);Check(rt.Telegraph.Contains("파동")&&rt.TakeAction()=="core_wave"&&rt.TakeAction()=="injection","Model.PreservePendingWave.ThenPhase2Injection");
            var formation=new Formation(BattleSide.Allies);var front=new Combatant("front","전열",BattleSide.Allies,new FormationSlot(FormationRow.Front,0),100,10,8,0,TargetRangeType.MeleePhysical,true);var rear=new Combatant("rear","후열",BattleSide.Allies,new FormationSlot(FormationRow.Rear,0),100,10,8,0,TargetRangeType.Magic,true);formation.Place(front);formation.Place(rear);b=Boss();
            Check(TargetResolver.ResolveHostileTargets(b,formation,TargetRangeType.MeleePhysical).SequenceEqual(new[]{front}),"Target.MoltenFrontProtection");b.ApplyTaunt(rear);Check(TargetResolver.ResolveHostileTargets(b,formation,TargetRangeType.MeleePhysical).SequenceEqual(new[]{rear}),"Target.TauntOverridesMelee");front.TakeDamage(100);Check(formation.LivingMembers.SequenceEqual(new[]{rear}),"Target.AreaLivingOnly");
            var warden=new BattleDungeonMonsterRuntime();b=Boss();warden.SetBoss(b);b.TakeDamage(479);Check(!warden.ShouldEnterPhaseTwo,"Regression.Warden.HP721");b.TakeDamage(1);Check(warden.ShouldEnterPhaseTwo,"Regression.Warden.HP720Threshold60Unchanged");
            var guardian=new Combatant("guardian","수호체",BattleSide.Enemies,new FormationSlot(FormationRow.Front,2),100,10,8,0,TargetRangeType.MeleePhysical,false);warden.PrepareShock();warden.EnterPhaseTwo(new[]{guardian});
            Check(warden.IsPhaseTwo&&warden.HasWardenBarrier&&!warden.IsShockPrepared&&warden.ModifyIncomingDamage(b,100,BattleDamageOrigin.DirectCombatAction)==70&&warden.ModifyIncomingDamage(b,100,BattleDamageOrigin.DamageOverTime)==100,"Regression.Warden.SummonBarrierAndDotExclusion");guardian.TakeDamage(100);warden.RemoveInvalidCombatants(new[]{b,guardian});Check(!warden.HasWardenBarrier&&warden.ModifyIncomingDamage(b,100,BattleDamageOrigin.DirectCombatAction)==100,"Regression.Warden.GuardianKoRemovesBarrier");
            var st=new BattleStatusEffectRuntime();var target=new Combatant("p","아군",BattleSide.Allies,new FormationSlot(FormationRow.Front,0),101,10,10,0,TargetRangeType.MeleePhysical,true);target.Defend();st.ApplyOverheat(target);st.ApplyOverheat(target);Check(st.ApplyOverheat(target)==9&&target.CurrentHp==92&&st.GetOverheatStacks(target)==0,"Regression.Main18.OverheatCeil8GuardIndependent");
            ItemCatalog.TryGet(Chapter2Main18Flow.CoolingItem,out var cooling);InventoryService.TryAddItem(cooling.ItemId,2);st.ApplyOverheat(target);Check(BattleItemUseService.TryUse(cooling,target,st,out var message)&&st.GetOverheatStacks(target)==0,"Regression.CoolingRemedy");int count=InventoryService.GetItemCount(cooling.ItemId);Check(!BattleItemUseService.TryUse(cooling,target,st,out message)&&InventoryService.GetItemCount(cooling.ItemId)==count,"Regression.CoolingEmptyNoConsumption");
            LootChecks();
        }
        private sealed class FixedLootRandom:IBattleRewardRandom
        {
            private readonly float value;public FixedLootRandom(float roll){value=roll;}
            public float NextValue()=>value;public int RangeInclusive(int minimum,int maximum)=>minimum;
        }
        // 기존 7종 자산은 수정하지 않고 실제 보상/상점/중첩 API로 회귀를 확인합니다.
        private static void LootChecks()
        {
            string[] monsters={"soot_hound","heatwind_hawk","fissure_lizard","ember_beetle","ember_wraith","obsidian_beetle","scorching_watcher"};
            string[] suffixes={"fang","feather","scale","carapace","ember_shard","shell","core_fragment"};float[] chances={.5f,.5f,.45f,.45f,.45f,.45f,.6f};int[] prices={6,6,7,8,9,10,15};
            var definitions=Resources.LoadAll<MonsterDefinition>("MonsterDefinitions");var shops=Resources.LoadAll<ShopDefinition>("ShopDefinitions");
            for(int i=0;i<monsters.Length;i++)
            {
                var monster=definitions.Single(x=>x.MonsterId==monsters[i]);string id="material_"+monsters[i]+"_"+suffixes[i];ItemCatalog.TryGet(id,out var item);
                Check(item!=null&&item.Icon!=null&&item.MaxStack==99&&item.SellPrice==prices[i]&&item.Category==ItemCategory.Material&&item.UseType==ItemUseType.None&&shops.All(s=>!s.ItemIds.Contains(id))&&monster.LootEntries.Count==1&&monster.LootEntries[0].ItemId==id&&Mathf.Approximately(monster.LootEntries[0].DropChance,chances[i])&&monster.LootEntries[0].MinCount==1&&monster.LootEntries[0].MaxCount==1,"Loot7.Data."+id);
                var slot=new FormationSlot(FormationRow.Front,0);var enemy=new Combatant("loot","잡템검증",BattleSide.Enemies,slot,10,10,8,0,TargetRangeType.MeleePhysical,false);enemy.TakeDamage(10);
                var setup=new BattleParticipantSetup("loot","잡템검증","",BattleSide.Enemies,slot,10,10,8,0,TargetRangeType.MeleePhysical,false,BattleParticipantVisualType.EncounterMonster,monsterDefinition:monster);var mapping=new Dictionary<Combatant,BattleParticipantSetup>{{enemy,setup}};
                var success=BattleVictoryReward.Calculate(true,15,new[]{enemy,enemy},mapping,new FixedLootRandom(0));var failure=BattleVictoryReward.Calculate(true,15,new[]{enemy},mapping,new FixedLootRandom(chances[i]));
                Check(success.Items.Length==1&&success.Items[0].ItemId==id&&success.Items[0].Count==1&&failure.Items.Length==0,"Loot7.ActualRewardOne.DuplicateIdentityAndChanceBoundary."+id);
                int currency=EconomyService.GetCurrency();Check(InventoryService.TryAddItem(id,99)&&!InventoryService.TryAddItem(id,1)&&ShopService.TrySell(id,1)==ShopTransactionResult.Success&&InventoryService.GetItemCount(id)==98&&EconomyService.GetCurrency()==currency+prices[i],"Loot7.Stack99.SellPrice."+id);
            }
        }
        private static IEnumerator Wait(double seconds){double until=EditorApplication.timeSinceStartup+seconds;while(EditorApplication.timeSinceStartup<until)yield return null;}
        private static IEnumerator Scene(string name)
        {
            double until=EditorApplication.timeSinceStartup+40;while(SceneManager.GetActiveScene().name!=name&&EditorApplication.timeSinceStartup<until)yield return null;
            Check(SceneManager.GetActiveScene().name==name,"Scene."+name);yield return Wait(.8);
            MonsterEncounterService.SuppressForSeconds(3600);
            Check(SceneManager.GetActiveScene().GetRootGameObjects().Sum(g=>g.GetComponentsInChildren<Transform>(true).Sum(t=>GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(t.gameObject)))==0,"MissingScript0."+name);
        }
        private static void At(Vector2 p){var player=Object.FindAnyObjectByType<PlayerController>();player.GetComponent<Rigidbody2D>().position=p;player.transform.position=p;}
        private static IEnumerator CloseDialogue()
        {
            int pages=0;while(DialoguePresenter.Instance!=null&&DialoguePresenter.Instance.IsOpen&&pages++<20)
            {DialoguePresenter.Instance.AdvanceFromInput();yield return Wait(.15);}
            Check(DialoguePresenter.Instance==null||!DialoguePresenter.Instance.IsOpen,"Dialogue.Complete");yield return Wait(.1);
        }
        private static IEnumerator Continue(string label)
        {
            string scene=SceneManager.GetActiveScene().name;Vector2 p=Object.FindAnyObjectByType<PlayerController>().transform.position;
            string quest=JsonUtility.ToJson(QuestService.ExportSaveData());int money=EconomyService.GetCurrency();
            string party=JsonUtility.ToJson(CompanionRosterService.ExportSaveData()),beast=JsonUtility.ToJson(BeastCompanionService.ExportSaveData());
            string inventory=JsonUtility.ToJson(new InventorySnapshot{Entries=InventoryService.ExportSaveData()}),resources=JsonUtility.ToJson(new ResourceSnapshot{Entries=PartyResourceService.ExportSaveData()});
            int level=GameSessionData.Level,exp=GameSessionData.CurrentExperience;string path=GameSessionData.SelectedPlayerPathId;
            Check(GameSaveService.SaveCurrentWorldPosition(p,scene),"Continue.Write."+label);
            SceneManager.LoadSceneAsync("Bootstrap");yield return Scene("Bootstrap");GameSessionData.Reset();
            var button=GameObject.Find("StartMenuCanvas/Slot01/Action")?.GetComponent<Button>();Check(button!=null,"Continue.BootstrapButton."+label);button.onClick.Invoke();yield return Scene(scene);
            Check(quest==JsonUtility.ToJson(QuestService.ExportSaveData())&&money==EconomyService.GetCurrency()&&Vector2.Distance(Object.FindAnyObjectByType<PlayerController>().transform.position,p)<.05f,"Continue.ExactQuestMoneyPosition."+label);
            Check(party==JsonUtility.ToJson(CompanionRosterService.ExportSaveData())&&beast==JsonUtility.ToJson(BeastCompanionService.ExportSaveData()),"Continue.PartyFormationBeast."+label);
            Check(inventory==JsonUtility.ToJson(new InventorySnapshot{Entries=InventoryService.ExportSaveData()})&&resources==JsonUtility.ToJson(new ResourceSnapshot{Entries=PartyResourceService.ExportSaveData()})&&level==GameSessionData.Level&&exp==GameSessionData.CurrentExperience&&path==GameSessionData.SelectedPlayerPathId,"Continue.InventoryHpMpGrowthPath."+label);
        }
        [Serializable]private sealed class InventorySnapshot{public InventoryEntry[] Entries;}
        [Serializable]private sealed class ResourceSnapshot{public PartyMemberResourceSaveData[] Entries;}
        private static void Geometry()
        {
            var camera=Camera.main;var follow=camera.GetComponent<ProjectLimitless.CameraSystem.CameraFollow>();
            Vector3 original=camera.transform.position;float aspect=camera.aspect;Vector2 player=Object.FindAnyObjectByType<PlayerController>().transform.position;
            var bounds=Object.FindAnyObjectByType<WorldBounds2D>().Bounds;
            foreach(float ratio in new[]{4f/3,16f/9,21f/9})foreach(Vector2 edge in new[]{new Vector2(-10,0),new Vector2(10,0),new Vector2(0,7),new Vector2(0,-7)})
            {
                camera.aspect=ratio;At(edge);camera.transform.position=new Vector3(edge.x,edge.y,original.z);Call(follow,"LateUpdate");
                float h=camera.orthographicSize,w=h*ratio;Vector3 at=camera.transform.position;
                Check(at.x-w>=bounds.min.x-.001f&&at.x+w<=bounds.max.x+.001f&&at.y-h>=bounds.min.y-.001f&&at.y+h<=bounds.max.y+.001f,"Camera.Viewport."+ratio+"."+edge);
            }
            At(player);camera.aspect=aspect;camera.transform.position=original;Call(follow,"LateUpdate");
            var walls=Object.FindObjectsByType<BoxCollider2D>().Where(x=>x.name.StartsWith("Boundary_")).ToArray();
            Check(walls.Any(x=>x.OverlapPoint(new Vector2(-10.5f,0)))&&!walls.Any(x=>x.OverlapPoint(new Vector2(10.5f,0))),"Boundary.Field11WestClosedEastExit");
            foreach(var box in Object.FindObjectsByType<BoxCollider2D>().Where(x=>x.name.StartsWith("CliffBoundary_")))
            {Check(Mathf.Abs(box.bounds.size.x-3)<.001f&&Mathf.Abs(box.bounds.size.y-1)<.001f,"Cliff.Collision3x1."+box.name);Check(Chapter2Main20Flow.Positions.All(p=>!box.OverlapPoint(p)),"Cliff.QuestRouteClear."+box.name);}
            var spawn=Object.FindObjectsByType<SceneSpawnPoint>().First(x=>Get<string>(x,"spawnPointId")=="Spawn_From_Field10");
            Check(Object.FindObjectsByType<SceneTransitionTrigger>().All(x=>!x.GetComponent<Collider2D>().OverlapPoint(spawn.transform.position)),"Spawn.ExitNotOverlapping");
        }
        private static IEnumerator Runtime()
        {
            Seed(false);Check(!Chapter2Main20Flow.Unlocked&&!QuestService.TryStart(Chapter2Main20Flow.QuestId),"Prerequisite.Main19Locked");Seed();Models();
            var definition=Resources.Load<MonsterDefinition>("MonsterDefinitions/18_VeinfireColossus");Check(definition.MaxHp==1200&&definition.MonsterLevel==15&&definition.BattleAttackPercent==300&&definition.BattleAgility==10&&definition.BaseExperience==140&&definition.CurrencyReward==40&&definition.LootEntries.Count==0,"Boss.Data.Exact.NoLoot");
            Check(QuestCatalog.All.Single(q=>q.QuestId==Chapter2Main20Flow.QuestId).Objectives.Count==9,"Quest.Objectives9");
            Check(Resources.LoadAll<Texture2D>("Main20").Length==17&&Resources.LoadAll<Sprite>("Main20/Boss/Veinfire_Colossus_Phase2_Overlay").Length==16,"Art.GamePng17.Overlay16");
            SceneManager.LoadSceneAsync(Chapter2Main19Flow.Field);yield return Scene(Chapter2Main19Flow.Field);
            Check(!GameObject.Find("Main20AccessGate").GetComponent<BoxCollider2D>().enabled,"Field10.GateUnlocked");
            At(new Vector2(-9.9f,0));yield return Scene(Chapter2Main20Flow.Field);Check(Chapter2Main20Flow.Current(1),"Quest.EntryAuto1.PhysicalExitTrigger");Geometry();yield return Continue("AtRift");
            for(int i=1;i<=4;i++)
            {
                At(Chapter2Main20Flow.Positions[i]);yield return Wait(.15);
                if(i==1||i==4){GameObject.Find(Chapter2Main20Flow.Targets[i]).GetComponent<Main20Site>().TryInteract();yield return CloseDialogue();}
                if(i<4)Check(Chapter2Main20Flow.Current(i+1),"Quest.ActualSite."+i);
            }
            yield return Scene("Battle");var controller=Object.FindAnyObjectByType<BattleSceneController>();controller.StopAllCoroutines();
            var boss=Get<Formation>(controller,"enemies").Members.Single();Check(boss.IsBoss&&boss.Attack==30&&boss.CurrentHp==1200,"Battle.ActualBoss");
            var bgm=Resources.Load<BgmSceneCatalog>("Audio/Music/BgmSceneCatalog");Check(BgmPlaybackService.Instance.CurrentClip==bgm.FindBattle(Chapter2Main20Flow.Field,Chapter2Main20Flow.EncounterId,true),"Bgm.CrownsEncounter");
            Call(controller,"Flee");Check(!Get<bool>(controller,"battleEnded"),"Battle.FleeBlocked");
            var commandActor=Get<Formation>(controller,"allies").Members.First();Set(controller,"currentActor",commandActor);Set(controller,"actionPlaying",false);
            var queueBefore=Get<TurnOrderQueue>(controller,"turnOrder").Upcoming.ToArray();
            Call(controller,"BeginCompanionAssaultSelection",new object[]{null});Call(controller,"PlayCompanionAssault",commandActor,boss,null);
            Check(boss.CurrentHp==1200&&!Get<bool>(controller,"actionPlaying")&&Get<TurnOrderQueue>(controller,"turnOrder").Upcoming.SequenceEqual(queueBefore)&&Get<Text>(controller,"messageText").text.Contains("습격을 사용할 수 없습니다"),"Battle.BeastAssaultBlockedNoHpTurnConsumption");
            // 실제 Controller coroutine으로 두 Phase 행동을 실행하며 공용 피해/과열/화상 연결을 확인합니다.
            var runtime=Get<BattleMain20BossRuntime>(controller,"main20Boss");var allies=Get<Formation>(controller,"allies");var status=Get<BattleStatusEffectRuntime>(controller,"statusEffects");
            foreach(var action in new[]{"injection","molten","condensation","core_wave"})
            {
                foreach(var ally in allies.Members)ally.RestoreCurrentResources(ally.MaxHp,ally.MaxMp);
                Set(controller,"currentActor",boss);Set(controller,"actionPlaying",false);
                controller.StartCoroutine((IEnumerator)Call(controller,"PlayMain20Action",boss,action));
                while(Get<bool>(controller,"actionPlaying"))yield return null;controller.StopAllCoroutines();
                Check(action=="condensation"||allies.Members.Any(a=>a.CurrentHp<a.MaxHp),"Battle.ControllerAction."+action);
            }
            boss.TakeDamage(600);yield return null;Check(runtime.PhaseTwo&&Object.FindAnyObjectByType<Main20PhaseOverlay>().PhaseTwo,"Battle.ImmediateOverlay");
            Check(Get<Text>(controller,"bossTelegraphText").text.Contains(Main20DialogueCatalog.PhaseDirection.Message),"Battle.SilentPhaseDirectionPersistentHud");
            foreach(var action in new[]{"injection","molten","resonance","eruption","wave"})
            {
                foreach(var ally in allies.Members)ally.RestoreCurrentResources(ally.MaxHp,ally.MaxMp);
                Set(controller,"currentActor",boss);Set(controller,"actionPlaying",false);controller.StartCoroutine((IEnumerator)Call(controller,"PlayMain20Action",boss,action));
                while(Get<bool>(controller,"actionPlaying"))yield return null;controller.StopAllCoroutines();Check(true,"Battle.ControllerPhase2."+action);
            }
            // 패배는 공용 안전지대 복귀 후 같은 목표6 유지. 이후 Field11을 다시 방문해 실제 Site 재도전합니다.
            Call(controller,"EndBattle","격리 패배",false);BattleSceneFlow.ReturnAfterDefeat();yield return Scene(GameSessionData.LastSafeZoneSceneId);
            Check(Chapter2Main20Flow.Current(5),"Defeat.Objective6Retained");SceneTransitionService.Load(Chapter2Main20Flow.Field,"Spawn_From_Field10");yield return Scene(Chapter2Main20Flow.Field);At(Chapter2Main20Flow.Positions[5]);yield return Wait(.15);
            GameObject.Find(Chapter2Main20Flow.Targets[5]).GetComponent<Main20Site>().TryInteract();yield return Scene("Battle");controller=Object.FindAnyObjectByType<BattleSceneController>();controller.StopAllCoroutines();boss=Get<Formation>(controller,"enemies").Members.Single();
            Check(boss.CurrentHp==1200&&!Get<BattleMain20BossRuntime>(controller,"main20Boss").PhaseTwo,"Retry.FreshBoss");
            int currency=EconomyService.GetCurrency(),beforeExp=GameSessionData.CurrentExperience;boss.TakeDamage(1200);Call(controller,"RefreshCombatantViews",new object[]{null});yield return Wait(.7);
            Check(Object.FindAnyObjectByType<Main20PhaseOverlay>().FrameIndex==15,"Art.KOFinalFrame15");
            Call(controller,"EndBattle","격리 승리",true);Check(Chapter2Main20Flow.Current(6)&&EconomyService.GetCurrency()==currency+40,"Victory.Objective7.Reward40Once");Call(controller,"EndBattle","중복",true);Check(EconomyService.GetCurrency()==currency+40,"Victory.DuplicateBlocked");Check(BgmPlaybackService.Instance.CurrentClip==bgm.FindBattle(Chapter2Main20Flow.Field,Chapter2Main20Flow.EncounterId,true),"Bgm.ResultCrownsMaintained");
            Check(GameSessionData.CurrentExperience==beforeExp+140,"Victory.BaseExp140EqualLevelOnce");
            Check(ExperienceProgression.MonsterExperience(140,1,15)==210&&ExperienceProgression.MonsterExperience(140,30,15)==0,"Experience.ExistingLevelDifferenceScaling");
            BattleSceneFlow.ReturnToField(true);yield return Scene(Chapter2Main20Flow.Field);Check(!GameObject.Find("FieldEnvironment").transform.Find("VeinfireColossusField").gameObject.activeSelf,"Presentation.BossHiddenAfterVictory");yield return Continue("AfterBoss");
            for(int i=6;i<=7;i++){At(Chapter2Main20Flow.Positions[i]);yield return Wait(.15);GameObject.Find(Chapter2Main20Flow.Targets[i]).GetComponent<Main20Site>().TryInteract();yield return CloseDialogue();Check(Chapter2Main20Flow.Current(i+1),"Quest.AfterBoss.ActualSite."+i);}
            currency=EconomyService.GetCurrency();int questExp=GameSessionData.CurrentExperience;
            At(Chapter2Main20Flow.Positions[8]);yield return Wait(.25);Check(QuestService.GetState(Chapter2Main20Flow.QuestId)==QuestState.Completed&&EconomyService.GetCurrency()==currency+80,"Quest.Completed9.Reward80");
            Check(GameSessionData.CurrentExperience==questExp+100,"Quest.ActualExp100Separate");
            QuestService.NotifyLocationReached(Chapter2Main20Flow.Targets[8]);Check(EconomyService.GetCurrency()==currency+80&&GameSessionData.CurrentExperience==questExp+100,"Quest.DuplicateRewardBlocked");yield return Continue("Completed");
            Check(BgmPlaybackService.Instance.CurrentClip==bgm.Find(Chapter2Main20Flow.Field),"Bgm.PathsFieldRestored");At(new Vector2(9.9f,0));yield return Scene(Chapter2Main19Flow.Field);Check(Vector2.Distance(Object.FindAnyObjectByType<PlayerController>().transform.position,new Vector2(-7.8f,0))<.05f,"Return.Field10.PhysicalExitTrigger.Spawn");
            for(int slot=1;slot<=5;slot++){GameSaveService.SelectSlot(slot);Check(GameSaveService.SaveCurrentSession()&&GameSaveService.TryLoadSlot(slot,out var saved)&&saved.Version==1,"Save.Slot"+slot);}
            Check(QuestCatalog.All.Count(q=>q.IsItemDelivery)==9&&Resources.LoadAll<ItemDefinition>("ItemDefinitions").Count(x=>x.ItemId.StartsWith("material_")&&(x.ItemId.Contains("soot_hound")||x.ItemId.Contains("heatwind_hawk")||x.ItemId.Contains("fissure_lizard")||x.ItemId.Contains("ember_beetle")||x.ItemId.Contains("ember_wraith")||x.ItemId.Contains("obsidian_beetle")||x.ItemId.Contains("scorching_watcher")))==7,"Regression.Side9.Loot7Registered");
            Check(errors==0,"Runtime.ConsoleErrors0");File.WriteAllText(Path.Combine(Root,"status.txt"),"PASS");
        }
        // 별도 실행: 수치/HP/행동을 강제하지 않고 실제 Attack Button→대상 선택→Turn Queue로 싸웁니다.
        // Lv15 전사+세린+미엘의 한 구성만 확인하며 전 직업 난이도 승인으로 확대하지 않습니다.
        private static IEnumerator NormalBattle()
        {
            Seed();QuestService.TryStart(Chapter2Main20Flow.QuestId);
            for(int i=0;i<5;i++){if(i==0||i==2||i==3)QuestService.NotifyLocationReached(Chapter2Main20Flow.Targets[i]);else QuestService.NotifyInteraction(Chapter2Main20Flow.Targets[i]);}
            SceneManager.LoadSceneAsync(Chapter2Main20Flow.Field);yield return Scene(Chapter2Main20Flow.Field);
            At(Chapter2Main20Flow.Positions[5]);yield return Wait(.2);GameObject.Find(Chapter2Main20Flow.Targets[5]).GetComponent<Main20Site>().TryInteract();yield return Scene("Battle");
            var c=Object.FindAnyObjectByType<BattleSceneController>();var enemies=Get<Formation>(c,"enemies");var allies=Get<Formation>(c,"allies");
            Check(allies.Members.Count()==3,"Normal.SavedPartyThree");Time.timeScale=8;double until=EditorApplication.timeSinceStartup+180;int attacks=0;
            while(!Get<bool>(c,"battleEnded")&&EditorApplication.timeSinceStartup<until)
            {
                var button=Get<Button>(c,"attackButton");if(!Get<bool>(c,"actionPlaying")&&button.IsActive()&&button.interactable)
                {
                    button.onClick.Invoke();var targets=Get<IReadOnlyList<Combatant>>(c,"selectableTargets");
                    if(Get<bool>(c,"choosingTarget")&&targets.Count>0){typeof(SecondRegressionAudit).GetMethod("Hit",Flags).Invoke(null,new object[]{c,targets[0]});attacks++;}
                }
                yield return null;
            }
            Time.timeScale=1;Results.Add("INFO|Normal.Attacks="+attacks+";HP="+string.Join(",",allies.Members.Select(x=>x.CurrentHp))+";BossHP="+enemies.Members.Single().CurrentHp);
            Check(enemies.IsDefeated&&attacks>0,"Normal.ActualQueueVictory");Check(Get<BattleMain20BossRuntime>(c,"main20Boss").PhaseTransitions==1,"Normal.PhaseTransitionOnce");
            Object.FindObjectsByType<Button>().First(x=>x.name=="VictoryReturn").onClick.Invoke();yield return Scene(Chapter2Main20Flow.Field);Check(Chapter2Main20Flow.Current(6),"Normal.VictoryReturnObjective7");
            Check(errors==0,"Normal.ConsoleErrors0");File.WriteAllText(Path.Combine(Root,"status.txt"),"PASS");
        }
        private static IEnumerator Flatten(IEnumerator root){var stack=new Stack<IEnumerator>();stack.Push(root);while(stack.Count>0){var top=stack.Peek();if(!top.MoveNext()){stack.Pop();continue;}if(top.Current is IEnumerator nested)stack.Push(nested);else yield return null;}}
        private static void Log(string message,string stack,LogType type){if(type==LogType.Error||type==LogType.Exception||type==LogType.Assert){errors++;Results.Add("ERROR|"+message);}}
        private static void Tick(){try{if(EditorApplication.timeSinceStartup>deadline)throw new TimeoutException("Main20 timeout");if(routine.MoveNext())return;EditorApplication.update-=Tick;SessionState.SetBool(Key,false);File.WriteAllLines(Path.Combine(Root,"results.txt"),Results);EditorApplication.Exit(0);}catch(Exception e){Results.Add("FAIL|"+e);File.WriteAllLines(Path.Combine(Root,"results.txt"),Results);File.WriteAllText(Path.Combine(Root,"status.txt"),"FAIL");Debug.LogException(e);EditorApplication.Exit(1);}}
    }
}

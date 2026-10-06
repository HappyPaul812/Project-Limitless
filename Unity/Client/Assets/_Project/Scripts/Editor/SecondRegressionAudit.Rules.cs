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
using UnityEngine.UI;
using Object=UnityEngine.Object;

namespace ProjectLimitless.EditorTools
{
    public static partial class SecondRegressionAudit
    {
        public static string LaunchRules()
        {
            results.Clear();if(File.Exists(Output))results.AddRange(JsonUtility.FromJson<Report>(File.ReadAllText(Output)).results.Where(r=>r.status!="FAIL"));
            selectedSkill="";extended=false;rules=true;UnityEditor.SessionState.SetBool(Armed,true);return Partial9FixedSpriteAudit.Launch();
        }
        static bool rules;
        static Combatant Fixture(string id,BattleSide side=BattleSide.Allies,FormationRow row=FormationRow.Front,int hp=1000)=>new Combatant(id,id,side,new FormationSlot(row,0),hp,20,10,0,TargetRangeType.MeleePhysical,true);
        static void RuleFixtures()
        {
            var a=Fixture("a");var b=Fixture("b");var e=Fixture("e",BattleSide.Enemies);var s=new BattleStatusEffectRuntime();
            var cd=new BattleSkillCooldowns();var m=new BattleFighterResourceRuntime();var executor=new BattleSkillExecutor(cd,s,m);
            var fighter=Resources.LoadAll<JobDefinition>("JobDefinitions").Single(j=>j.JobId=="fighter");var slash=BattleSkillCatalog.GetSkills(fighter).Single(x=>x.Id==BattleSkillCatalog.FighterNandoId);
            for(int i=1;i<=3;i++){Check(executor.ExecuteSingleMeleePhysicalAttackWithMomentumGain(a,e,slash,out _,out _),"Fighter","slash-use"+i,"정식 Executor 직접 성공");Check(m.GetMomentum(a)==i&&cd.GetRemaining(a,slash.Id)==(i==3?slash.CooldownTurns:0),"Fighter","slash-third"+i,"기세/세 번째 직접 성공 CD 분리");}
            s.ApplyIronWall(a,2);a.Defend();Check(s.ApplyIncomingDamage(a,100)==30,"Guardian","iron-guard","70% 감소/Guard 중첩 없음");
            s.CompleteActorAction(a);Check(s.GetIronWallRemaining(a)==2,"Guardian","iron-cast-end","시전 행동 미소비");s.CompleteActorAction(b);s.CompleteActorAction(a);Check(s.GetIronWallRemaining(a)==1,"Guardian","iron-own-end","다른 행동 제외, 자기 다음 행동 종료 소비");s.CompleteActorAction(a);Check(s.GetIronWallRemaining(a)==0,"Guardian","iron-expire","다음 2회 행동 종료 만료");
            s.ApplyGaiaWall(b,2);b.Defend();Check(s.ApplyIncomingDamage(b,100)==40,"Mage","gaia-guard","60% 감소/Guard 중첩 없음");
            var cover=Fixture("cover");var ally=Fixture("ally");s.ApplyGuardianCover(cover);int budget=s.GetGuardianCoverRemainingBudget(cover),before=cover.CurrentHp;
            int direct=s.ApplyIncomingDamage(ally,100);Check(direct<100&&cover.CurrentHp<before&&s.GetGuardianCoverRemainingBudget(cover)<budget,"Guardian","vow-direct-budget","직접 보호/수호자 부담/예산 소비");before=cover.CurrentHp;budget=s.GetGuardianCoverRemainingBudget(cover);
            Check(s.ApplyIncomingDamage(ally,20,BattleDamageOrigin.DamageOverTime)==20&&cover.CurrentHp==before&&s.GetGuardianCoverRemainingBudget(cover)==budget,"Guardian","vow-dot","DoT 보호/전이/예산 적용 제외");s.BeginActorAction(cover);Check(!s.HasGuardianCover(cover),"Guardian","vow-end","수호자 다음 행동 시작 종료");
            var opponents=new Formation(BattleSide.Allies);opponents.Place(cover);var alternative=new Combatant("other","other",BattleSide.Allies,new FormationSlot(FormationRow.Rear,0),100,10,1,0,TargetRangeType.Magic,true);opponents.Place(alternative);
            e.ApplyTaunt(cover,2);Check(TargetResolver.ResolveHostileTargets(e,opponents,TargetRangeType.RangedPhysical).Single()==cover&&TargetResolver.ResolveHostileTargets(e,opponents,TargetRangeType.Magic,true).Count==2,"Guardian","taunt-single-aoe","단일 강제/광역 제외");e.CompleteAction();Check(e.ForcedTargetActionsRemaining==1,"Guardian","taunt-action1","적 실제 행동1");e.CompleteAction();Check(e.ForcedTargetActionsRemaining==0,"Guardian","taunt-action2","적 실제 행동2 만료");
            Check(s.ModifyOutgoingDamage(e,100)==100,"Status","shock-baseline","감전 전 피해");s.ApplyOrRefreshShock(e);Check(s.ModifyOutgoingDamage(e,100)==85,"Status","shock85","감전 피해85%");s.ApplyOrRefreshSilence(e);Check(s.HasSilence(e),"Status","silence","다음 성공 행동 침묵");s.CompleteActorAction(e);Check(!s.HasSilence(e)&&s.ModifyOutgoingDamage(e,100)==100,"Status","action-status-end","Shock/Silence 성공 행동 종료 해제");
            var poison=Fixture("poison",hp:100);s.ApplyOrRefreshPoison(poison,3);Check(s.ApplyPoisonTickAtActionEnd(poison,out _)==5,"Status","poison","최대 HP5%/기본 독");s.ApplyOrRefreshBurn(poison,7,2);Check(s.ApplyBurnTickAtActionEnd(poison,out _)==7,"Status","burn","저장 피해7 DoT");
            var boss=Fixture("boss",BattleSide.Enemies);var guard=Fixture("guard",BattleSide.Enemies);var dungeon=new BattleDungeonMonsterRuntime();dungeon.SetBoss(boss);boss.TakeDamage(400,false);Check(dungeon.ShouldEnterPhaseTwo,"Boss","phase60","HP60% Phase2 경계");dungeon.PrepareShock();Check(dungeon.IsShockPrepared,"Boss","telegraph","감전 예고 상태");dungeon.EnterPhaseTwo(new[]{guard});Check(dungeon.IsPhaseTwo&&dungeon.HasWardenBarrier&&!dungeon.IsShockPrepared&&dungeon.ModifyIncomingDamage(boss,100,BattleDamageOrigin.DirectCombatAction)==70&&dungeon.ModifyIncomingDamage(boss,100,BattleDamageOrigin.DamageOverTime)==100,"Boss","shield","수호체 소환/30%장막/DoT 제외/예고 리셋");guard.TakeDamage(1000,false);dungeon.RemoveInvalidCombatants(new[]{guard,boss});Check(!dungeon.HasWardenBarrier&&dungeon.ModifyIncomingDamage(boss,100,BattleDamageOrigin.DirectCombatAction)==100,"Boss","guardian-ko","수호체 KO→장막 즉시 해제");
            foreach(string path in new[]{"path.vision","path.hearing","path.intellectual","path.mobility","path.emotional-scar"})
            {
                var owner=Fixture("owner",row:FormationRow.Rear);var enemy=Fixture("enemy",BattleSide.Enemies);var traits=new PathCombatTraitRuntime(new[]{new KeyValuePair<Combatant,string>(owner,path)},new[]{owner,enemy});var statuses=new BattleStatusEffectRuntime();
                int expected=100;
                if(path=="path.vision"){traits.ApplyDirectDamage(statuses,owner,enemy,100);expected=103;}
                if(path=="path.hearing"){traits.ApplyDirectDamage(statuses,enemy,owner,100);traits.CompleteActorAction(enemy);expected=110;}
                if(path=="path.intellectual"){traits.ApplyDirectDamage(statuses,enemy,owner,100);traits.ApplyDirectDamage(statuses,enemy,owner,100);Check(traits.ModifyDirectHealing(owner,owner,100)==110&&traits.ApplyDirectDamage(statuses,enemy,owner,100)==90,"Path",path+"/pattern","반복 패턴 직접피해-10%/직접치유+10%");}
                if(path=="path.mobility")expected=105;
                if(path=="path.emotional-scar"){traits.ApplyDirectDamage(statuses,enemy,owner,501);expected=110;}
                Check(traits.ApplyDirectDamage(statuses,owner,enemy,100)==expected,"Path",path+"/direct","정본 직접 피해 기대="+expected);
                statuses.ApplyOrRefreshBurn(enemy,7,2);Check(traits.ObserveHealthChange(enemy,()=>statuses.ApplyBurnTickAtActionEnd(enemy,out _))==7,"Path",path+"/dot","길 배율 DoT 제외");
                int heal=path=="path.mobility"?105:path=="path.emotional-scar"?110:100;
                Check(traits.ModifyDirectHealing(owner,owner,100)==heal,"Path",path+"/heal","정본 직접 치유 기대="+heal);
            }
        }
        static void Progression()
        {
            GameSessionData.Reset();GameSessionData.ConfigurePlayer(PlayerVisualType.Male,"진행감사");GameSessionData.SelectJob("fighter");GameSessionData.RecordLocation("World_StarterVillage","Spawn_From_Field01");
            var quests=QuestCatalog.All.Where(q=>q.QuestType==QuestType.Main).OrderBy(q=>q.QuestId).ToArray();
            Check(quests.Length==16&&quests.Sum(q=>q.Objectives.Count)==103,"Quest","16/103","정식 Main16/Objective103");
            Check(quests.Select(q=>q.QuestId).Distinct().Count()==16&&quests.All(q=>q.Objectives.Select(o=>o.ObjectiveId).Distinct().Count()==q.Objectives.Count),"Quest","unique","Quest+Objective 복합 키103 고유. 전역 문자열101/2개 퀘스트별 재사용 허용");
            var notify=typeof(QuestService).GetMethod("Notify",Static);
            for(int i=0;i<quests.Length;i++)
            {
                var quest=quests[i];Check(QuestService.GetState(quest.QuestId)==QuestState.Available&&QuestService.TryStart(quest.QuestId),"Quest",quest.QuestId+"/unlock","선행 완료→다음 Main 시작");
                for(int j=0;j<quest.Objectives.Count;j++)
                {
                    var objective=quest.Objectives[j];Check(QuestService.ActiveMainQuest.CurrentObjectiveIndex==j&&!string.IsNullOrWhiteSpace(objective.TargetId),"Quest",quest.QuestId+"/sequence"+j,"정본 Objective 순서/Target ID");
                    notify.Invoke(null,new object[]{objective.ObjectiveType,"invalid_regression_target"});Check(QuestService.ActiveMainQuest.CurrentObjectiveIndex==j,"Quest",quest.QuestId+"/wrong"+j,"잘못된 Target으로 진행 없음");
                    var save=JsonUtility.ToJson(QuestService.ExportSaveData());QuestService.Reset();QuestService.ImportSaveData(JsonUtility.FromJson<QuestProgressSaveData>(save));
                    Check(JsonUtility.ToJson(QuestService.ExportSaveData())==save,"Quest",quest.QuestId+"/restore"+j,"103개 중간 상태 JSON 정확 복원");
                    for(int count=0;count<objective.RequiredCount;count++)notify.Invoke(null,new object[]{objective.ObjectiveType,objective.TargetId});
                }
                if(QuestService.GetState(quest.QuestId)!=QuestState.Completed)QuestService.NotifyNpcTalked(quest.TurnInNpcId);
                Check(QuestService.GetState(quest.QuestId)==QuestState.Completed,"Quest",quest.QuestId+"/complete","완료 조건/TurnIn/Reward 경계");
                bool next=i+1<quests.Length?QuestService.GetState(quests[i+1].QuestId)==QuestState.Available&&quests[i+1].PrerequisiteQuestIds.Contains(quest.QuestId)&&(string.IsNullOrEmpty(quest.NextMainQuestId)||quest.NextMainQuestId==quests[i+1].QuestId):string.IsNullOrEmpty(quest.NextMainQuestId);
                Check(next,"Quest",quest.QuestId+"/next","선행 prerequisite로 다음 Main 해금; optional NextMain ID 참조 유효/Main17 없음");
            }
        }
        static IEnumerator ItemUi()
        {
            Seed("healer",12);yield return Load("Battle");var c=Object.FindAnyObjectByType<BattleSceneController>();var actor=Get<Formation>(c,"allies").Members.First();var target=Get<Formation>(c,"allies").Members.Last();var statuses=Get<BattleStatusEffectRuntime>(c,"statusEffects");
            foreach(var effect in new[]{ItemEffectType.RemovePoison,ItemEffectType.RemoveBurn,ItemEffectType.RemoveShock,ItemEffectType.RemoveSilence})
            {
                ActorCommand(c,actor);statuses.RemoveAllHarmfulStatuses(target);var item=ItemCatalog.All.Single(i=>i.EffectType==effect);InventoryService.TryAddItem(item.ItemId,2);int before=InventoryService.GetItemCount(item.ItemId);
                Get<Button>(c,"itemButton").onClick.Invoke();Get<List<Button>>(c,"itemMenuButtons").Single(b=>b.name=="Item_"+item.ItemId).onClick.Invoke();Get<Button>(c,"cancelButton").onClick.Invoke();
                Check(Get<bool>(c,"choosingItem")&&InventoryService.GetItemCount(item.ItemId)==before&&Get<Combatant>(c,"currentActor")==actor,"Item",effect+"/cancel","실제 대상 취소/수량·행동 보존");
                Get<List<Button>>(c,"itemMenuButtons").Single(b=>b.name=="Item_"+item.ItemId).onClick.Invoke();Hit(c,target);
                Check(Get<bool>(c,"choosingItem")&&InventoryService.GetItemCount(item.ItemId)==before&&Get<Combatant>(c,"currentActor")==actor,"Item",effect+"/invalid","해당 상태 없는 대상/수량·행동 미소비");
                statuses.ApplyOrRefreshPoison(target,3);statuses.ApplyOrRefreshBurn(target,7,2);statuses.ApplyOrRefreshShock(target);statuses.ApplyOrRefreshSilence(target);
                Get<List<Button>>(c,"itemMenuButtons").Single(b=>b.name=="Item_"+item.ItemId).onClick.Invoke();Hit(c,target);
                double end=UnityEditor.EditorApplication.timeSinceStartup+3;while(Get<Combatant>(c,"currentActor")==actor&&UnityEditor.EditorApplication.timeSinceStartup<end)yield return null;
                Check(InventoryService.GetItemCount(item.ItemId)==before-1&&statuses.GetHarmfulStatuses(target).Count==3&&Get<Combatant>(c,"currentActor")!=actor,"Item",effect+"/success","개별 상태1만 제거/수량1/행동 소비, Cleanse 전체제거와 구분");c.StopAllCoroutines();
            }
        }
        static object M16(string method,params object[] args)=>typeof(ProjectLimitless.Editor.Main16RuntimeAudit).GetMethod(method,Static).Invoke(null,args);
        static IEnumerator Conditions()
        {
            Seed("sharpshooter",12);yield return Load("Battle");var c=Object.FindAnyObjectByType<BattleSceneController>();var actor=Get<Formation>(c,"allies").Members.First();ActorCommand(c,actor);
            foreach(var rear in Get<Formation>(c,"enemies").Members.Where(e=>e.Slot.Row==FormationRow.Rear))rear.TakeDamage(int.MaxValue,false);
            OpenSkills(c);SkillButton(c,BattleSkillCatalog.SharpshooterArrowRainId).onClick.Invoke();
            Check(Get<bool>(c,"choosingSkill")&&!Get<bool>(c,"actionPlaying")&&Get<Combatant>(c,"currentActor")==actor&&Get<BattleSkillCooldowns>(c,"skillCooldowns").GetRemaining(actor,BattleSkillCatalog.SharpshooterArrowRainId)==0,"Skill15","arrow-no-rear","실제 UI 후열 없음→Action/CD 미소비");
            Check(Get<Formation>(c,"allies").Members.Count()==3,"Beast","no-extra-turn","Assault 동물은 별도 Combatant/턴 없음");
            Seed("healer",12);yield return Load("Battle");c=Object.FindAnyObjectByType<BattleSceneController>();actor=Get<Formation>(c,"allies").Members.First();ActorCommand(c,actor);actor.RestoreCurrentResources(actor.CurrentHp,0);OpenSkills(c);SkillButton(c,BattleSkillCatalog.HealerHealingLightId).onClick.Invoke();
            Check(Get<bool>(c,"choosingSkill")&&!Get<bool>(c,"actionPlaying")&&actor.CurrentMp==0&&Get<Combatant>(c,"currentActor")==actor,"Resource","mp-zero","실제 MP 부족 UI 거절/행동 보존");Get<Button>(c,"cancelButton").onClick.Invoke();
            var statuses=Get<BattleStatusEffectRuntime>(c,"statusEffects");statuses.ApplyOrRefreshSilence(actor);Get<Button>(c,"skillButton").onClick.Invoke();
            Check(!Get<bool>(c,"choosingSkill")&&Get<Combatant>(c,"currentActor")==actor&&statuses.HasSilence(actor),"Status","silence-ui","실제 Skill 명령 차단/상태·행동 보존");statuses.RemoveAllHarmfulStatuses(actor);statuses.ApplyGaiaWall(actor,2);Get<Button>(c,"defendButton").onClick.Invoke();Check(!actor.IsDefending&&Get<Combatant>(c,"currentActor")==actor,"Mage","gaia-guard-ui","실제 Guard 버튼 중첩 거절");
        }
        static IEnumerator Defeat()
        {
            Seed("mage",12);PastQuests("main_10");CompanionRosterService.UnlockPaul("mage");GameSessionData.ConfigureProgress(12,23);EconomyService.Import(123);GameSessionData.ActivateSafeZone("safezone_catacomb_entrance","Field_03","Spawn_From_Dungeon01");
            // Field03의 정상 입장에서는 Main10이 자동 시작된다. 그 이후 Dungeon에 들어간 유효 상태를 준비한다.
            QuestService.TryStart(MainQuest10FieldFlow.QuestId);GameSessionData.RecordLocation("Dungeon_01_B2","");
            var spawn=Resources.LoadAll<FieldMonsterSpawnDefinition>("MonsterSpawns").First(s=>s.SceneName=="Dungeon_01_B2"&&!s.NonRespawningBoss);BattleEncounterContext.Set(spawn.Monster,spawn,null,null,Vector2.zero,Vector2.zero);yield return Load("Battle");var c=Object.FindAnyObjectByType<BattleSceneController>();var actor=Get<Formation>(c,"allies").Members.First();ActorCommand(c,actor);var statuses=Get<BattleStatusEffectRuntime>(c,"statusEffects");
            var cure=ItemCatalog.All.Single(i=>i.EffectType==ItemEffectType.RemoveSilence);InventoryService.TryAddItem(cure.ItemId,2);statuses.ApplyOrRefreshSilence(actor);Check(BattleItemUseService.TryUse(cure,actor,statuses,out _),"Defeat","consumed-before-wipe","실제 공용 아이템 효과/수량 소비 후 전멸 Fixture");int count=InventoryService.GetItemCount(cure.ItemId);string quest=JsonUtility.ToJson(QuestService.ExportSaveData()),party=JsonUtility.ToJson(CompanionRosterService.ExportSaveData());
            foreach(var ally in Get<Formation>(c,"allies").Members){statuses.ApplyOrRefreshPoison(ally,3);statuses.ApplyOrRefreshBurn(ally,7,2);statuses.ApplyOrRefreshShock(ally);statuses.ApplyOrRefreshSilence(ally);ally.TakeDamage(int.MaxValue,false);}Call(c,"AdvanceTurn");yield return WaitScene("Field_03");
            Check(PartyResourceService.ExportSaveData().All(r=>PartyResourceService.TryGet(r.CharacterId,out var resource)&&resource.CurrentHp==resource.MaxHp&&resource.CurrentMp==resource.MaxMp),"Defeat","restore-hp","Party wipe→최근 Field03 안전지대/HP/MP 완전 회복");
            Check(GameSessionData.Level==12&&GameSessionData.CurrentExperience==23&&EconomyService.GetCurrency()==123&&InventoryService.GetItemCount(cure.ItemId)==count&&JsonUtility.ToJson(QuestService.ExportSaveData())==quest&&JsonUtility.ToJson(CompanionRosterService.ExportSaveData())==party,"Defeat","preserve","EXP/Talent/소모품 미환불/영구진행/Party 유지");
            Check(Object.FindAnyObjectByType<BattleSceneController>()==null&&!MonsterEncounterService.IsSpawnAvailable(null),"Defeat","transient-scene","Battle 상태 객체 Scene 종료 폐기");
            Record("Defeat","equipment","NOT_VERIFIED","정식 장착 상태 저장 시스템 미구현 범위. Inventory 보존과 장착 검증을 구분");
            yield return ContinueAt("Field_03","DefeatSafezone");
            Seed("mage",12);var bossSpawn=Resources.LoadAll<FieldMonsterSpawnDefinition>("MonsterSpawns").Single(s=>s.SpawnId==MainQuest11DungeonFlow.BossId);BattleEncounterContext.Set(bossSpawn.Monster,bossSpawn,null,null,Vector2.zero,Vector2.zero);yield return Load("Battle");c=Object.FindAnyObjectByType<BattleSceneController>();actor=Get<Formation>(c,"allies").Members.First();ActorCommand(c,actor);Get<Button>(c,"fleeButton").onClick.Invoke();Check(!Get<bool>(c,"battleEnded")&&Get<Combatant>(c,"currentActor")==actor&&Get<Formation>(c,"enemies").Members.Any(e=>e.IsBoss),"Boss","no-flee-ui","정식 Boss Scene의 실제 도망 버튼 거절");
        }
        static IEnumerator SafeParty()
        {
            foreach(string scene in new[]{"Field_03","Arbel"})
            {
                Seed("mage",12);PastQuests("main_10");CompanionRosterService.UnlockPaul("mage");yield return Load(scene);var manager=Object.FindObjectsByType<ProjectLimitless.NPC.VillageNpcRole>(FindObjectsSortMode.None).First(n=>n.Role==ProjectLimitless.NPC.VillageNpcRoleType.PartyManager);manager.Interact(manager.GetComponent<ProjectLimitless.NPC.NpcController>());var ui=PartyManagementPresenter.Instance;Check(ui.IsOpen,"Party",scene+"/open","허용 안전지역 NPC 실제 편성 UI");var row=CompanionRosterService.GetRow("player");ui.ToggleRow("player");ui.Confirm();if(ui.IsWarningVisible)ui.Confirm();bool applied=!ui.IsOpen&&CompanionRosterService.GetRow("player")!=row;
                Check(applied,"Party",scene+"/confirm","허용 지역 확정→진형 적용/닫기/Save");
                yield return ContinueAt(scene,"Party"+scene);
                manager=Object.FindObjectsByType<ProjectLimitless.NPC.VillageNpcRole>(FindObjectsSortMode.None).First(n=>n.Role==ProjectLimitless.NPC.VillageNpcRoleType.PartyManager);
                manager.Interact(manager.GetComponent<ProjectLimitless.NPC.NpcController>());ui=PartyManagementPresenter.Instance;
                yield return Wait(.5);Check(ui.IsOpen,"Party",scene+"/stay-open","여러 프레임 뒤에도 안전지역 화면 유지");
                row=CompanionRosterService.GetRow("player");ui.ToggleRow("player");ui.Cancel();Check(!ui.IsOpen&&CompanionRosterService.GetRow("player")==row,"Party",scene+"/cancel","취소 시 저장 진형 보존");
            }
        }
        static IEnumerator Main16Representative()
        {
            foreach(bool hearing in new[]{true,false})
            {
                var type=typeof(ProjectLimitless.Editor.Main16RuntimeAudit);type.GetField("label",Static).SetValue(null,hearing?"Second/Hearing":"Second/Default");type.GetField("voiceReview",Static).SetValue(null,false);ProjectLimitless.Editor.Main16RuntimeAudit.Results.Clear();M16("Seed",hearing,true);
                SceneTransitionService.Load("Arbel","Spawn_From_Field06");yield return (IEnumerator)M16("WaitScene","Arbel");M16("Leon");yield return (IEnumerator)M16("Pages");
                yield return (IEnumerator)M16("Transition","Transition_northwest_to_field06",Chapter2Main16Flow.PreviousField);M16("Move",new Vector2(-8,0));yield return null;yield return null;
                yield return (IEnumerator)M16("Transition","Transition_west_to_field07",Chapter2Main16Flow.Field);
                yield return (IEnumerator)M16("Inspect","field07_main16_ash","ash",3,hearing);yield return (IEnumerator)M16("Continue","Second/ash");
                yield return (IEnumerator)M16("Inspect","field07_main16_vibration","vibration",4,hearing);yield return (IEnumerator)M16("Inspect","field07_main16_tracks","tracks",5,hearing);
                M16("Move",new Vector2(-3,0));yield return null;yield return null;yield return (IEnumerator)M16("Pages");yield return (IEnumerator)M16("WaitScene","Battle");
                yield return (IEnumerator)M16("WinBattle",true);yield return (IEnumerator)M16("Continue","Second/victory");
                yield return (IEnumerator)M16("Inspect","field07_main16_afterimage","afterimage",8,hearing);yield return (IEnumerator)M16("Inspect","field07_main16_canyon","canyon",9,hearing);
                yield return (IEnumerator)M16("Transition","Transition_east_to_field06",Chapter2Main16Flow.PreviousField);yield return (IEnumerator)M16("Transition","Transition_east_to_arbel","Arbel");M16("Leon");yield return (IEnumerator)M16("Pages");yield return (IEnumerator)M16("Continue","Second/final");
                Check(QuestService.GetState(Chapter2Main16Flow.QuestId)==QuestState.Completed,"Main16",hearing?"Hearing":"Default","실제 왕복/분기/정상 공격 승리/최종 보고/Continue; 내부 PASS="+ProjectLimitless.Editor.Main16RuntimeAudit.Results.Count);
                File.WriteAllText(Path.Combine(Root,"Temp/SecondRegression20261006/Main16_"+(hearing?"Hearing":"Default")+".txt"),string.Join("\n",ProjectLimitless.Editor.Main16RuntimeAudit.Results));
            }
        }
        static IEnumerator Rules()
        {
            if(!results.Any(r=>r.category=="Path"&&r.id=="path.emotional-scar/heal"))RuleFixtures();
            yield return Load("Bootstrap");if(!results.Any(r=>r.category=="Quest"&&r.id=="main_16_shape_in_the_ash/complete"))Progression();
            if(!results.Any(r=>r.category=="Item"&&r.id=="RemoveSilence/success"))yield return ItemUi();
            if(!results.Any(r=>r.category=="Mage"&&r.id=="gaia-guard-ui"))yield return Conditions();
            if(!results.Any(r=>r.category=="Defeat"&&r.id=="preserve"))yield return Defeat();
            if(!results.Any(r=>r.category=="Party"&&r.id=="Arbel/cancel"))yield return SafeParty();
            if(!results.Any(r=>r.category=="Main16"&&r.id=="Default"))yield return Main16Representative();
            if(!results.Any(r=>r.category=="UI"&&r.id=="Battle/opening-ended"))yield return RemainingViews();
            yield return Load("Bootstrap");
        }
    }
}

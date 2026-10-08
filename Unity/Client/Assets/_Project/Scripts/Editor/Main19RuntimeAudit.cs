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
    public static class Main19RuntimeAudit
    {
        private const string Armed="Limitless.Main19.Audit";
        private const BindingFlags Flags=BindingFlags.NonPublic|BindingFlags.Public|BindingFlags.Static|BindingFlags.Instance;
        public static readonly List<string> Results=new List<string>();
        private static string Output=>Path.GetFullPath(Path.Combine(Application.dataPath,"../../../문서/00_프로젝트/Main19_Runtime_Results.txt"));
        public static string Status="IDLE";
        private static bool balanceOnly;
        static Main19RuntimeAudit()
        {
            var initialized=Partial9FixedSpriteAudit.Status;
            EditorApplication.playModeStateChanged+=state=>
            {
                if(!SessionState.GetBool(Armed,false))return;
                if(state==PlayModeStateChange.EnteredPlayMode)
                    typeof(Partial9FixedSpriteAudit).GetField("routine",Flags).SetValue(null,Flatten(Run()));
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
            QuestService.ImportSaveData(new QuestProgressSaveData{CompletedQuestIds=QuestCatalog.All.Where(x=>x.QuestId.StartsWith("main_")&&string.CompareOrdinal(x.QuestId,"main_19")<0).Select(x=>x.QuestId).ToArray()});
            CompanionRosterService.UnlockPaul(job);CompanionRosterService.UnlockSerin();
            Check(CompanionRosterService.TrySetComposition(new[]{CompanionRosterService.SerinId,CompanionRosterService.MielId},new Dictionary<string,FormationRow>{{"player",FormationRow.Front},{CompanionRosterService.SerinId,FormationRow.Rear},{CompanionRosterService.MielId,FormationRow.Rear}}),"Fixture.Party."+job);
            GameSessionData.ActivateSafeZone("arbel","Arbel","Spawn_Arbel_Center");EconomyService.Import(200);
            InventoryService.TryAddItem("item_healing_potion_small",6);InventoryService.TryAddItem("item_mana_potion_small",4);
        }
        private static void Move(Vector2 at){UnityEngine.Object.FindAnyObjectByType<PlayerController>().transform.position=at;Physics2D.SyncTransforms();}
        private static IEnumerator Inspect(int index)
        {
            Move(Chapter2Main19Flow.Positions[index]);yield return Wait(.1);
            GameObject.Find(Chapter2Main19Flow.Targets[index]).GetComponent<Main19Site>().TryInteract();
            if(index==4||index==7)yield break;
            Check(DialoguePresenter.Instance.IsOpen,"Dialogue.Open."+index);Check(Chapter2Main19Flow.IsCurrent(index),"Dialogue.NoEarlyProgress."+index);
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

        }
        [Serializable]private sealed class InventorySnapshot{public InventoryEntry[] Entries;}
        [Serializable]private sealed class ResourceSnapshot{public PartyMemberResourceSaveData[] Entries;}
        private static IEnumerator Fight(string label)
        {
            yield return Scene("Battle");var c=UnityEngine.Object.FindAnyObjectByType<BattleSceneController>();
            var enemies=Value<Formation>(c,"enemies");var allies=Value<Formation>(c,"allies");
            Check(enemies.Members.Select(x=>x.Id).OrderBy(x=>x).SequenceEqual(new[]{"field10_obsidian_beetle",label=="A"?"field10_fissure_lizard":"field10_ember_wraith"}.OrderBy(x=>x)),"Battle."+label+".ExactSpecies");
            Check(enemies.Members.Count()==2&&!enemies.Members.First().IsBoss,"Battle."+label+".MixedTwoNotBoss");
            Check(allies.Members.Count()==1+CompanionRosterService.ActivePartyCharacterIds.Count,"Battle."+label+".SavedParty");
            Check(BgmPlaybackService.Instance.CurrentClip==Resources.Load<BgmSceneCatalog>("Audio/Music/BgmSceneCatalog").FindBattle(Chapter2Main19Flow.Field,"",false),"Battle."+label+".Blade");
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
            int after=EconomyService.GetCurrency();Check(after>currency,"Battle."+label+".TalentOnce");
            Call(c,"EndBattle","duplicate",true);Check(EconomyService.GetCurrency()==after,"Battle."+label+".DuplicateZero");
            Check(allies.Members.All(x=>Value<BattleStatusEffectRuntime>(c,"statusEffects").GetOverheatStacks(x)==0),"Battle."+label+".HeatCleared");
            Results.Add("INFO|Balance."+label+".HP="+string.Join(",",allies.Members.Select(x=>x.CurrentHp)));Write();
            UnityEngine.Object.FindObjectsByType<Button>().First(x=>x.name=="VictoryReturn").onClick.Invoke();yield return Scene(Chapter2Main19Flow.Field);
        }

        private static IEnumerator Run()
        {
            Seed();
            var monsters=Resources.LoadAll<MonsterDefinition>("MonsterDefinitions");
            var beetle=monsters.Single(x=>x.MonsterId=="obsidian_beetle");var lizard=monsters.Single(x=>x.MonsterId=="fissure_lizard");var wraith=monsters.Single(x=>x.MonsterId=="ember_wraith");
            foreach(string id in new[]{Chapter2Main19Flow.PatrolA,Chapter2Main19Flow.PatrolB,""})
            {
                var setup=BattlePrototypeEncounterFactory.CreateField10("검증","fighter","path.hearing",200,30,12,beetle,id,lizard,wraith);
                Check(setup.Enemies.Count==(id==""?1:2),"Factory.MixedOrOptional."+id);
                Check(setup.Enemies.All(x=>x.MaxHp==x.MonsterDefinition.MaxHp&&x.Attack==(int)Math.Max(1L,((long)10*x.MonsterDefinition.BattleAttackPercent+99)/100)&&x.Agility==x.MonsterDefinition.BattleAgility&&!x.IsBoss),"Factory.OriginalStats."+id);
                if(id==Chapter2Main19Flow.PatrolB)Check(setup.Enemies[1].Slot.Row==FormationRow.Rear,"Factory.WraithRear");
            }
            Check(Resources.LoadAll<FieldMonsterSpawnDefinition>("MonsterSpawns").Count(x=>x.SceneName==Chapter2Main19Flow.Field)==3,"Optional.ThreeDefinitions");
            var paths=new[]{"path.vision","path.hearing","path.intellectual","path.mobility","path.emotional-scar"};
            Check(paths.Select(p=>Main19DialogueCatalog.Get(5,p,true).Single(l=>l.IsPlayer).DialogueId).Distinct().Count()==5,"Path.FiveIndependentIds");
            var lines=paths.SelectMany(p=>new[]{2,3,5,8,10,11,12}.SelectMany(i=>Main19DialogueCatalog.Get(i,p,true))).GroupBy(l=>l.DialogueId).Select(g=>g.First()).ToArray();
            Check(lines.All(l=>Resources.Load<VoiceClipCatalog>("Audio/Voice/Story/StoryVoiceCatalog").Find(l.DialogueId)==null),"Voice.AllPendingNoExistingVoiceReuse");
            Check(lines.Where(l=>!l.IsPlayer&&!l.IsDirection).Count()==13,"Voice.Npc13Pending");
            SceneTransitionService.Load(Chapter2Main18Flow.Field,"Spawn_From_Field08");yield return Scene(Chapter2Main18Flow.Field);
            Check(Chapter2Main19Flow.IsCurrent(0),"Quest.AutoStartAfterMain18");
            Check(!GameObject.Find("Main19AccessGate").GetComponent<BoxCollider2D>().enabled,"Gate.Main18CompleteOpen");
            Move(Chapter2Main19Flow.Positions[0]);yield return Wait(.2);Check(Chapter2Main19Flow.IsCurrent(1),"Quest.GateLocation");
            SceneTransitionService.Load(Chapter2Main19Flow.Field,"Spawn_From_Field09");yield return Scene(Chapter2Main19Flow.Field);
            Check(Chapter2Main19Flow.IsCurrent(2),"Quest.EntryLocation");Geometry();
            Check(UnityEngine.Object.FindObjectsByType<SceneTransitionTrigger>().Length==1,"Field.EastOnlyExitMain20Closed");
            Check(BgmPlaybackService.Instance.CurrentClip==Resources.Load<BgmSceneCatalog>("Audio/Music/BgmSceneCatalog").Find(Chapter2Main19Flow.Field),"BGM.PathsOfCrackedEarth");
            yield return Inspect(2);yield return Inspect(3);yield return Continue("before-A");
            yield return Inspect(4);yield return Retry(4);yield return Fight("A");Check(Chapter2Main19Flow.IsCurrent(5),"Quest.AVictoryOnly");yield return Continue("after-A");
            yield return Inspect(5);Move(Chapter2Main19Flow.Positions[6]);yield return Wait(.2);Check(Chapter2Main19Flow.IsCurrent(7),"Quest.CorridorLocation");
            yield return Inspect(7);yield return Retry(7);yield return Fight("B");Check(Chapter2Main19Flow.IsCurrent(8),"Quest.BVictoryOnly");yield return Inspect(8);
            Move(Chapter2Main19Flow.Positions[9]);yield return Wait(.2);Check(Chapter2Main19Flow.IsCurrent(10),"Quest.RidgeLocation");yield return Continue("before-witness");
            yield return Inspect(10);int money=EconomyService.GetCurrency(),exp=GameSessionData.CurrentExperience;yield return Inspect(11);
            Check(QuestService.GetState(Chapter2Main19Flow.QuestId)==QuestState.Completed&&EconomyService.GetCurrency()==money+70&&GameSessionData.CurrentExperience==exp+80,"Quest.Reward80Exp70Currency");
            QuestService.NotifyInteraction(Chapter2Main19Flow.Targets[11]);Check(EconomyService.GetCurrency()==money+70,"Quest.DuplicateCompletionZero");yield return Continue("completed");
            Move(new Vector2(-8.8f,-2.8f));yield return Wait(.1);GameObject.Find("Main19ClosedDeepRoute").GetComponent<Main19Site>().TryInteract();yield return ReadDialogue();
            SceneTransitionService.Load(Chapter2Main18Flow.Field,"Spawn_From_Field10");yield return Scene(Chapter2Main18Flow.Field);
            Check(Vector2.Distance(UnityEngine.Object.FindAnyObjectByType<PlayerController>().transform.position,new Vector2(-7.8f,0))<.05f,"Field.ReturnField09Spawn");
            Check(GameSaveService.CurrentVersion==1,"Save.Version1Unchanged");
        }
        private static IEnumerator Retry(int index)
        {
            yield return Scene("Battle");var c=UnityEngine.Object.FindAnyObjectByType<BattleSceneController>();int money=EconomyService.GetCurrency();
            double until=EditorApplication.timeSinceStartup+20;
            while((Value<bool>(c,"actionPlaying")||!Value<Button>(c,"attackButton").interactable)&&EditorApplication.timeSinceStartup<until)yield return null;
            Call(c,"Flee");yield return Scene(Chapter2Main19Flow.Field);Check(Chapter2Main19Flow.IsCurrent(index)&&EconomyService.GetCurrency()==money,"Retry.FleeNoProgressNoReward");
            yield return Inspect(index);yield return Scene("Battle");c=UnityEngine.Object.FindAnyObjectByType<BattleSceneController>();c.StopAllCoroutines();
            foreach(var ally in Value<Formation>(c,"allies").Members)ally.TakeDamage(100000,false);
            Call(c,"EndBattle","검증 전멸",false);yield return Scene("Arbel");Check(Chapter2Main19Flow.IsCurrent(index)&&EconomyService.GetCurrency()==money,"Retry.DefeatNoProgressNoReward");
            PartyResourceService.HealPartyFully();SceneTransitionService.Load(Chapter2Main19Flow.Field,"Spawn_From_Field09");yield return Scene(Chapter2Main19Flow.Field);yield return Inspect(index);
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
    }
}

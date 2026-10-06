using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ProjectLimitless.Battle;
using ProjectLimitless.Core;
using ProjectLimitless.Monster;
using ProjectLimitless.Player;
using ProjectLimitless.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace ProjectLimitless.EditorTools
{
    /// <summary>기존 미검증 경로를 실제 Runtime UI와 격리 저장으로 보강한다. 사용자 Asset/Save는 수정하지 않는다.</summary>
    [InitializeOnLoad]
    public static partial class SecondRegressionAudit
    {
        [Serializable] public sealed class Result { public string category, id, status, evidence; }
        [Serializable] public sealed class Report { public Result[] results; }
        static readonly List<Result> results = new List<Result>();
        const BindingFlags Hidden=BindingFlags.Instance|BindingFlags.NonPublic;
        const BindingFlags Static=BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
        const string Armed="Limitless.SecondRegression.Armed";
        static string selectedSkill="";
        static bool extended;
        static string Root=>Path.GetFullPath(Path.Combine(Application.dataPath,"../../.."));
        static string Output=>Path.Combine(Root,"문서/00_프로젝트/LIMITLESS_SecondRegression_Runtime.json");
        public static string Status=>Partial9FixedSpriteAudit.Status;
        static SecondRegressionAudit()
        {
            EditorApplication.playModeStateChanged+=state=>
            {
                if(!SessionState.GetBool(Armed,false))return;
                if(state==PlayModeStateChange.EnteredPlayMode)
                    typeof(Partial9FixedSpriteAudit).GetField("routine",Static).SetValue(null,Flatten(Run()));
                if(state==PlayModeStateChange.EnteredEditMode){SessionState.SetBool(Armed,false);Save();}
            };
        }
        public static string Launch()
        {
            results.Clear();SessionState.SetBool(Armed,true);
            selectedSkill="";extended=false;rules=false;
            // 기존 launcher가 저장/설정 격리, PlayUnfocused 및 종료 복원을 책임진다. 기존 전체 Run은 호출하지 않는다.
            return Partial9FixedSpriteAudit.Launch();
        }
        public static string Resume(string skillId="",bool supplement=false)
        {
            results.Clear();if(File.Exists(Output))results.AddRange(JsonUtility.FromJson<Report>(File.ReadAllText(Output)).results.Where(r=>r.status!="FAIL"));
            selectedSkill=skillId;extended=supplement;rules=false;SessionState.SetBool(Armed,true);return Partial9FixedSpriteAudit.Launch();
        }
        static IEnumerator Flatten(IEnumerator first)
        {
            var stack=new Stack<IEnumerator>();stack.Push(first);
            while(stack.Count>0)
            {
                var top=stack.Peek();bool more;
                try{more=top.MoveNext();}
                catch(Exception e){Record("Harness","exception","FAIL",e.ToString());throw;}
                if(!more){stack.Pop();continue;}
                if(top.Current is IEnumerator nested)stack.Push(nested);else yield return top.Current;
            }
        }
        static T Get<T>(object o,string n)=>(T)o.GetType().GetField(n,Hidden).GetValue(o);
        static void Set(object o,string n,object v)=>o.GetType().GetField(n,Hidden).SetValue(o,v);
        static object Call(object o,string n,params object[] args)=>o.GetType().GetMethod(n,Hidden).Invoke(o,args);
        static void Record(string category,string id,string status,string evidence)
            {results.RemoveAll(r=>r.category==category&&r.id==id);results.Add(new Result{category=category,id=id,status=status,evidence=evidence});Save();}
        static void Check(bool pass,string category,string id,string evidence)
        {Record(category,id,pass?"PASS":"FAIL",evidence);if(!pass)throw new InvalidOperationException(category+"/"+id+": "+evidence);}
        static void Save()
        {
            File.WriteAllText(Output,JsonUtility.ToJson(new Report{results=results.ToArray()},true),new UTF8Encoding(false));
        }
        static IEnumerator Wait(double seconds)
        {double end=EditorApplication.timeSinceStartup+seconds;while(EditorApplication.timeSinceStartup<end)yield return null;}
        static IEnumerator Load(string scene)
        {
            var operation=SceneManager.LoadSceneAsync(scene);double end=EditorApplication.timeSinceStartup+30;
            while(!operation.isDone&&EditorApplication.timeSinceStartup<end)yield return null;
            if(!operation.isDone)throw new TimeoutException(scene);
            yield return Wait(.3);
            if(scene=="Battle")
            {
                double ready=EditorApplication.timeSinceStartup+10;
                while(GameObject.Find("TieBreakOverlay")!=null&&EditorApplication.timeSinceStartup<ready)yield return null;
                if(GameObject.Find("TieBreakOverlay")!=null)throw new TimeoutException("Battle 입장 연출 종료");
            }
        }
        static Button ButtonNamed(string name)=>Object.FindObjectsByType<Button>(FindObjectsInactive.Include,FindObjectsSortMode.None).First(b=>b.name==name&&b.gameObject.activeInHierarchy);
        static void Seed(string job,int level=1)
        {
            GameSessionData.Reset();GameSaveService.SelectSlot(1);
            GameSessionData.ConfigurePlayer(PlayerVisualType.Male,"회귀 감사");GameSessionData.SelectPlayerPath("path.vision");GameSessionData.SelectJob(job);GameSessionData.ConfigureProgress(level,0);
            CompanionRosterService.UnlockIntroCompanions();
            GameSessionData.RecordLocation("World_StarterVillage","Spawn_From_Field01");
            BattleEncounterContext.Set(null,null,null,null,Vector2.zero,Vector2.zero);
        }
        static void ActorCommand(BattleSceneController c,Combatant actor)
        {
            c.StopAllCoroutines();Set(c,"actionPlaying",false);Set(c,"currentActor",actor);Set(c,"choosingSkill",false);Set(c,"choosingTarget",false);Set(c,"battleEnded",false);
            var all=Get<Formation>(c,"allies").Members.Concat(Get<Formation>(c,"enemies").Members).ToArray();
            var order=new TurnOrderQueue();order.InitializeBattle(all);order.Build(all);
            for(int i=0;i<all.Length;i++)if(order.TakeNext(all)==actor)break;
            Set(c,"turnOrder",order);
            Call(c,"SetCommandButtons",true);
        }
        static Button SkillButton(BattleSceneController c,string id)=>Get<List<Button>>(c,"skillMenuButtons").Single(b=>b.name=="Skill_"+id);
        static void OpenSkills(BattleSceneController c)=>Get<Button>(c,"skillButton").onClick.Invoke();

        /// <summary>실제 Scene UI를 사용하되 특정 상태를 주입한 skill fixture다. 일반 조우 승리와 혼동하지 않는다.</summary>
        static IEnumerator Skills()
        {
            foreach(var job in Resources.LoadAll<JobDefinition>("JobDefinitions").OrderBy(j=>j.JobId))
            foreach(var skill in BattleSkillCatalog.GetSkills(job))
            {
                if(selectedSkill!=""&&skill.Id!=selectedSkill)continue;
                Seed(job.JobId);yield return Load("Battle");
                var c=Object.FindAnyObjectByType<BattleSceneController>();
                var allies=Get<Formation>(c,"allies");var enemies=Get<Formation>(c,"enemies");var actor=allies.Members.First();
                Get<Dictionary<Combatant,JobDefinition>>(c,"combatantJobs")[actor]=job;
                ActorCommand(c,actor);
                var cooldown=Get<BattleSkillCooldowns>(c,"skillCooldowns");var status=Get<BattleStatusEffectRuntime>(c,"statusEffects");var momentum=Get<BattleFighterResourceRuntime>(c,"fighterResources");
                string label=skill.Id;
                Check(GameObject.Find("TieBreakOverlay")==null,"Skill15",label+"/opening-ready","입장 우선순위 연출 자연 종료 후 UI 검사");
                OpenSkills(c);Check(Get<bool>(c,"choosingSkill")&&SkillButton(c,label)!=null,"Skill15",label+"/menu","Command Button→실제 직업 Skill Menu");
                int hp=actor.CurrentHp,mp=actor.CurrentMp;var sameActor=Get<Combatant>(c,"currentActor");
                Get<Button>(c,"cancelButton").onClick.Invoke();
                Check(!Get<bool>(c,"choosingSkill")&&Get<Combatant>(c,"currentActor")==sameActor&&actor.CurrentMp==mp&&cooldown.GetRemaining(actor,label)==0,"Skill15",label+"/menu-cancel","취소→기본 명령, 행동/MP/CD 미소비");
                // 재사용 대기 입력도 실제 버튼으로 거절한다. 설명 접근성을 위해 버튼은 포커스 가능하다.
                cooldown.Start(actor,label,2);OpenSkills(c);SkillButton(c,label).onClick.Invoke();
                Check(Get<bool>(c,"choosingSkill")&&!Get<bool>(c,"actionPlaying")&&cooldown.GetRemaining(actor,label)==2&&actor.CurrentMp==mp,"Skill15",label+"/blocked-cd","CD 입력 거절/행동·자원 보존");
                cooldown.Start(actor,label,0);Get<Button>(c,"cancelButton").onClick.Invoke();
                if(job.JobId=="healer")actor.TakeDamage(Math.Max(1,actor.MaxHp/2),false);
                if(label==BattleSkillCatalog.HealerCleanseId)
                {
                    OpenSkills(c);SkillButton(c,label).onClick.Invoke();Call(c,"SelectTarget",actor);
                    Check(Get<bool>(c,"choosingSkill")&&cooldown.GetRemaining(actor,label)==0&&Get<Combatant>(c,"currentActor")==actor,"Skill15",label+"/empty","제거할 상태 없음→스킬 메뉴/행동CD미소비");
                    status.ApplyOrRefreshPoison(actor,3);status.ApplyOrRefreshBurn(actor,3,2);status.ApplyOrRefreshShock(actor);status.ApplyOrRefreshSilence(actor);
                    // 자기 침묵은 사용을 막으므로 동료에게 4상태를 준비하고 치유사는 정상 상태로 둔다.
                    status.RemoveAllHarmfulStatuses(actor);var ally=allies.Members.Last();status.ApplyOrRefreshPoison(ally,3);status.ApplyOrRefreshBurn(ally,3,2);status.ApplyOrRefreshShock(ally);status.ApplyOrRefreshSilence(ally);
                }
                if(label==BattleSkillCatalog.FighterCriticalStrikeId)momentum.AddMomentum(actor,3);
                OpenSkills(c);SkillButton(c,label).onClick.Invoke();
                if(Get<bool>(c,"choosingTarget"))
                {
                    var targets=Get<IReadOnlyList<Combatant>>(c,"selectableTargets");
                    Check(targets.Count>0,"Skill15",label+"/targeting","실제 targeting 후보 및 포커스");
                    if(label==BattleSkillCatalog.SharpshooterAimId)Check(targets.All(t=>t.Slot.Row==FormationRow.Rear)||!enemies.LivingMembers.Any(t=>t.Slot.Row==FormationRow.Rear),"Skill15",label+"/rear-first","후열 우선 사거리");
                    Get<Button>(c,"cancelButton").onClick.Invoke();
                    Check(Get<bool>(c,"choosingSkill")&&!Get<bool>(c,"choosingTarget")&&Get<Combatant>(c,"currentActor")==actor&&cooldown.GetRemaining(actor,label)==0,"Skill15",label+"/target-cancel","대상 취소→Skill Menu/행동CD미소비");
                    SkillButton(c,label).onClick.Invoke();
                    targets=Get<IReadOnlyList<Combatant>>(c,"selectableTargets");
                    var target=label==BattleSkillCatalog.HealerCleanseId?targets.First(t=>status.GetHarmfulStatuses(t).Count>0):targets.First();
                    // 대상 버튼의 onClick 경로를 사용한다. SelectTarget 직접 호출은 empty 조건 검증에만 사용했다.
                    var views=Get<object>(c,"combatantViews") as System.Collections.IDictionary;var view=views[target];
                    ((Button)view.GetType().GetField("HitArea",BindingFlags.Instance|BindingFlags.Public).GetValue(view)).onClick.Invoke();
                }
                int hpBefore=actor.CurrentHp;int enemyBefore=enemies.Members.Sum(e=>e.CurrentHp);int allyBefore=allies.Members.Sum(e=>e.CurrentHp);
                double deadline=EditorApplication.timeSinceStartup+15;
                while(Get<Combatant>(c,"currentActor")==actor&&!Get<bool>(c,"battleEnded")&&EditorApplication.timeSinceStartup<deadline)yield return null;
                c.StopAllCoroutines();
                Check(Get<Combatant>(c,"currentActor")!=actor||Get<bool>(c,"battleEnded"),"Skill15",label+"/action-end","정상 연출 callback→FinishCurrentAction/턴 진행");
                Check(!Get<bool>(c,"choosingSkill")&&!Get<bool>(c,"choosingTarget"),"Skill15",label+"/ui-return","메뉴/대상 선택 종료, Battle UI 복귀");
                int expectedCd=label==BattleSkillCatalog.FighterNandoId?0:skill.CooldownTurns;
                Check(cooldown.GetRemaining(actor,label)==expectedCd,"Skill15",label+"/cooldown","정본 CD="+expectedCd+" 실제="+cooldown.GetRemaining(actor,label));
                bool effect=true;
                if(label==BattleSkillCatalog.GuardianTauntId)effect=enemies.LivingMembers.All(e=>e.ForcedTargetActionsRemaining==2);
                else if(label==BattleSkillCatalog.GuardianIronWallId)effect=status.GetIronWallRemaining(actor)==2;
                else if(label==BattleSkillCatalog.GuardianCoverAlliesId)effect=status.HasGuardianCover(actor);
                else if(label==BattleSkillCatalog.MageGaiaWallId)effect=status.GetGaiaWallRemaining(actor)>0;
                else if(label==BattleSkillCatalog.HealerCleanseId)effect=allies.Members.All(a=>status.GetHarmfulStatuses(a).Count==0);
                else if(job.JobId=="healer")effect=allies.Members.Sum(a=>a.CurrentHp)>allyBefore;
                else effect=enemies.Members.Sum(e=>e.CurrentHp)<enemyBefore;
                Check(effect,"Skill15",label+"/effect","실제 Damage/Heal/Status 결과");
                if(label==BattleSkillCatalog.FighterNandoId)Check(momentum.GetMomentum(actor)==1,"Skill15",label+"/momentum","직접 성공 난도 기세+1");
                if(label==BattleSkillCatalog.FighterCriticalStrikeId)Check(momentum.GetMomentum(actor)==0,"Skill15",label+"/momentum","회심 기세 전부 소비");
                if(label==BattleSkillCatalog.FighterWhirlwindId)Check(momentum.GetMomentum(actor)>=1&&momentum.GetMomentum(actor)<=3,"Skill15",label+"/momentum","전열 적중 기세 0~3 상한");
                Check(actor.CurrentMp==Math.Min(actor.MaxMp,mp-skill.MpCost+actor.MpRecoveryPerAction),"Skill15",label+"/resource","MP 비용="+skill.MpCost+" 행동 종료 회복="+actor.MpRecoveryPerAction+" 순사용="+(mp-actor.CurrentMp));
                yield return Load("Bootstrap");
            }
        }
        static IEnumerator Run()
        {
            yield return Wait(.5);
            if(rules)yield return Rules();else if(extended)yield return Supplement();else yield return Skills();
            Record("Input","physical-keyboard","USER_INPUT_REQUIRED","물리 키보드 입력은 이번 자동 UI 호출과 구분");
            Record("Input","physical-gamepad","USER_INPUT_REQUIRED","물리 게임패드 입력/기기 감각은 별도 확인");
            yield return Load("Bootstrap");Save();
        }
    }
}

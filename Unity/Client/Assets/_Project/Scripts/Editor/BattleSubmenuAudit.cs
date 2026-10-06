using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ProjectLimitless.Battle;
using ProjectLimitless.Audio;
using ProjectLimitless.Core;
using ProjectLimitless.Monster;
using ProjectLimitless.Player;
using ProjectLimitless.UI;
using ProjectLimitless.World;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace ProjectLimitless.EditorTools
{
    /// <summary>기존 격리 실행으로 변경된 메뉴 표시·입력과 대표 전투만 검증합니다.</summary>
    [InitializeOnLoad]
    public static class BattleSubmenuAudit
    {
        const BindingFlags Hidden=BindingFlags.Instance|BindingFlags.NonPublic;
        const BindingFlags Static=BindingFlags.Static|BindingFlags.NonPublic|BindingFlags.Public;
        const string Armed="Limitless.BattleSubmenu.Armed";
        public static readonly List<string> Results=new List<string>();
        static bool integrationOnly;
        static bool smokeOnly;
        static bool bindingsOnly;
        static string Root=>Path.GetFullPath(Path.Combine(Application.dataPath,"../../.."));
        static string Output=>Path.Combine(Root,"문서/00_프로젝트/LIMITLESS_BattleSubmenu_Runtime.txt");
        static string Capture=>Path.Combine(Root,"Temp/BattleSubmenu20261006");
        static BattleSubmenuAudit()
        {
            var initialized=Partial9FixedSpriteAudit.Status;
            EditorApplication.playModeStateChanged+=s=>
            {
                if(!SessionState.GetBool(Armed,false))return;
                if(s==PlayModeStateChange.EnteredPlayMode)typeof(Partial9FixedSpriteAudit).GetField("routine",Static).SetValue(null,Flatten(Run()));
                if(s==PlayModeStateChange.EnteredEditMode){SessionState.SetBool(Armed,false);Save();}
            };
        }
        public static string Launch(){Results.Clear();integrationOnly=false;smokeOnly=false;bindingsOnly=false;SessionState.SetBool(Armed,true);return Partial9FixedSpriteAudit.Launch();}
        public static string ResumeIntegration(){Results.Clear();if(File.Exists(Output))Results.AddRange(File.ReadAllLines(Output).Where(l=>l.StartsWith("PASS|")));integrationOnly=true;smokeOnly=false;bindingsOnly=false;SessionState.SetBool(Armed,true);return Partial9FixedSpriteAudit.Launch();}
        public static string LaunchSmoke(){Results.Clear();if(File.Exists(Output))Results.AddRange(File.ReadAllLines(Output).Where(l=>l.StartsWith("PASS|")));integrationOnly=false;smokeOnly=true;bindingsOnly=false;SessionState.SetBool(Armed,true);return Partial9FixedSpriteAudit.Launch();}
        public static string LaunchBindings(){Results.Clear();if(File.Exists(Output))Results.AddRange(File.ReadAllLines(Output).Where(l=>l.StartsWith("PASS|")));integrationOnly=false;smokeOnly=false;bindingsOnly=true;SessionState.SetBool(Armed,true);return Partial9FixedSpriteAudit.Launch();}
        static void Save()=>File.WriteAllLines(Output,Results);
        static void Check(bool pass,string id,string proof){Results.Add((pass?"PASS":"FAIL")+"|"+id+"|"+proof);Save();if(!pass)throw new InvalidOperationException(id+": "+proof);}
        static IEnumerator Flatten(IEnumerator first)
        {
            var stack=new Stack<IEnumerator>();stack.Push(first);
            while(stack.Count>0){var top=stack.Peek();bool more;try{more=top.MoveNext();}catch(Exception e){Results.Add("FAIL|Harness|"+e);Save();throw;}if(!more){stack.Pop();continue;}if(top.Current is IEnumerator nested)stack.Push(nested);else yield return top.Current;}
        }
        static T Get<T>(object o,string n)=>(T)o.GetType().GetField(n,Hidden).GetValue(o);
        static object Call(object o,string n,params object[] args)=>o.GetType().GetMethod(n,Hidden).Invoke(o,args);
        // 기존 helper의 준비·Scene 대기만 재사용합니다. 이전817 결과를 쓰는 Record/전체 Run은 호출하지 않습니다.
        static object Helper(string n,params object[] args)=>typeof(SecondRegressionAudit).GetMethod(n,Static).Invoke(null,args);
        static IEnumerator Load(string name)=>(IEnumerator)Helper("Load",name);
        static IEnumerator Wait(double seconds)=>(IEnumerator)Helper("Wait",seconds);
        static void Seed(string job,int level=1)=>Helper("Seed",job,level);
        static void Actor(BattleSceneController c)=>Helper("ActorCommand",c,Get<Formation>(c,"allies").Members.First());
        static List<Selectable> Commands(BattleSceneController c)=>Get<List<Selectable>>(c,"commandButtons");
        static void Click(BattleSceneController c,string field)=>Get<Button>(c,field).onClick.Invoke();
        static void HiddenCommands(BattleSceneController c,string id)
        {
            var commands=Commands(c);var es=EventSystem.current;
            Check(commands.Count==5&&commands.All(b=>!b.gameObject.activeInHierarchy&&!b.interactable&&!b.IsActive()),id+"/hidden","기본5명령 GameObject 비활성·입력 차단");
            Check(es.currentSelectedGameObject!=null&&!commands.Any(b=>b.gameObject==es.currentSelectedGameObject),id+"/focus","메뉴/대상 포커스 유지·기본 명령 유출 없음");
            var hits=new List<RaycastResult>();var pointer=new PointerEventData(es){position=RectTransformUtility.WorldToScreenPoint(null,commands[0].transform.position)};
            es.RaycastAll(pointer,hits);Check(!hits.Any(h=>commands.Any(b=>h.gameObject.transform.IsChildOf(b.transform))),id+"/raycast","비활성 기본명령 Graphic은 Raycast 후보에서 제외");
            bool skill=Get<bool>(c,"choosingSkill"),item=Get<bool>(c,"choosingItem");
            foreach(var b in commands)ExecuteEvents.Execute(b.gameObject,new BaseEventData(es),ExecuteEvents.submitHandler);
            Check(skill==Get<bool>(c,"choosingSkill")&&item==Get<bool>(c,"choosingItem")&&!Get<bool>(c,"actionPlaying"),id+"/hidden-submit","숨긴 버튼 Submit으로 다른 명령 시작 없음");
            foreach(var b in Object.FindObjectsByType<Selectable>().Where(b=>b.IsActive()&&b.IsInteractable()))
                Check(!new[]{b.FindSelectableOnLeft(),b.FindSelectableOnRight(),b.FindSelectableOnUp(),b.FindSelectableOnDown()}.Any(n=>n!=null&&commands.Contains(n)),id+"/nav/"+b.name,"활성 Selectable에서 숨긴 기본 명령으로 Navigation 없음");
        }
        static void Restored(BattleSceneController c,string id)
        {
            Check(Commands(c).All(b=>b.gameObject.activeInHierarchy&&b.interactable),id+"/restore","기본5명령 표시·입력 복구");
            Check(!Get<bool>(c,"choosingSkill")&&!Get<bool>(c,"choosingItem")&&!Get<Image>(c,"skillMenuPanel").gameObject.activeSelf&&!Get<Image>(c,"itemMenuPanel").gameObject.activeSelf,id+"/panels","닫힌 메뉴 잔류 없음");
        }
        static IEnumerator Ready(BattleSceneController c)
        {
            double end=EditorApplication.timeSinceStartup+25;
            while(!Get<bool>(c,"battleEnded")&&(!Commands(c).All(b=>b.gameObject.activeInHierarchy&&b.interactable)||Get<bool>(c,"actionPlaying"))&&EditorApplication.timeSinceStartup<end)yield return null;
            Check(Get<bool>(c,"battleEnded")||Commands(c).All(b=>b.gameObject.activeInHierarchy&&b.interactable),"action-ready","정상 행동 종료→다음 조작 가능한 명령 또는 승리");
        }
        static IEnumerator Views(string name)
        {
            Directory.CreateDirectory(Capture);
            foreach(int width in new[]{1920,1600,1280}){PlayModeWindow.SetCustomRenderingResolution((uint)width,(uint)(width*9/16),"Background QA");yield return Wait(.3);Canvas.ForceUpdateCanvases();Check(Screen.width==width,"resolution/"+name+width,"백그라운드 렌더링 해상도");ScreenCapture.CaptureScreenshot(Path.Combine(Capture,name+"_"+width+".png"));yield return Wait(.3);}
        }
        static IEnumerator Menus()
        {
            foreach(string job in new[]{"guardian","healer","sharpshooter","fighter","mage"})
            {
                Seed(job);yield return Load("Battle");var c=Object.FindAnyObjectByType<BattleSceneController>();Actor(c);var actor=Get<Combatant>(c,"currentActor");
                for(int repeat=0;repeat<2;repeat++)
                {
                    EventSystem.current.SetSelectedGameObject(Get<Button>(c,"skillButton").gameObject);Click(c,"skillButton");yield return null;HiddenCommands(c,job+"/skill"+repeat);
                    if(job=="healer"&&repeat==0)yield return Views("Skill");
                    var es=EventSystem.current;var selected=es.currentSelectedGameObject;ExecuteEvents.Execute(selected,new AxisEventData(es){moveDir=MoveDirection.Right},ExecuteEvents.moveHandler);
                    Check(es.currentSelectedGameObject!=null&&!Commands(c).Any(b=>b.gameObject==es.currentSelectedGameObject),job+"/move","방향 이벤트 후 숨긴 기본명령으로 유출 없음");
                    Click(c,"cancelButton");Restored(c,job+"/cancel"+repeat);Check(es.currentSelectedGameObject==Get<Button>(c,"skillButton").gameObject,job+"/return-focus"+repeat,"원래 스킬 명령으로 포커스 복구");
                }
                if(job=="healer")actor.TakeDamage(Math.Max(1,actor.MaxHp/2),false);
                Click(c,"skillButton");Get<List<Button>>(c,"skillMenuButtons").First(b=>b.name.StartsWith("Skill_")&&b.interactable).onClick.Invoke();
                if(Get<bool>(c,"choosingTarget"))
                {
                    HiddenCommands(c,job+"/target");Click(c,"cancelButton");Check(Get<bool>(c,"choosingSkill"),job+"/target-cancel","대상 취소 후 스킬 메뉴 복귀");HiddenCommands(c,job+"/target-return");
                    Get<List<Button>>(c,"skillMenuButtons").First(b=>b.name.StartsWith("Skill_")&&b.interactable).onClick.Invoke();Helper("Hit",c,Get<IReadOnlyList<Combatant>>(c,"selectableTargets").First());
                }
                yield return Ready(c);if(!Get<bool>(c,"battleEnded"))Restored(c,job+"/use");
            }
            Seed("healer",12);yield return Load("Battle");var controller=Object.FindAnyObjectByType<BattleSceneController>();Actor(controller);var healer=Get<Combatant>(controller,"currentActor");
            var potion=ItemCatalog.All.First(i=>i.EffectType==ItemEffectType.RecoverHp&&i.UseType!=ItemUseType.World);InventoryService.TryAddItem(potion.ItemId,3);
            Click(controller,"itemButton");HiddenCommands(controller,"item/open");yield return Views("Item");Click(controller,"cancelButton");Restored(controller,"item/cancel");
            Click(controller,"itemButton");Get<List<Button>>(controller,"itemMenuButtons").First(b=>b.name=="Item_"+potion.ItemId).onClick.Invoke();HiddenCommands(controller,"item/target");Click(controller,"cancelButton");HiddenCommands(controller,"item/target-return");
            Get<List<Button>>(controller,"itemMenuButtons").First(b=>b.name=="Item_"+potion.ItemId).onClick.Invoke();Helper("Hit",controller,healer);Check(Get<bool>(controller,"choosingItem"),"item/invalid","최대 HP 사용 거절 후 메뉴 유지");HiddenCommands(controller,"item/invalid-return");
            healer.TakeDamage(Math.Max(1,healer.MaxHp/2),false);int count=InventoryService.GetItemCount(potion.ItemId);
            Get<List<Button>>(controller,"itemMenuButtons").First(b=>b.name=="Item_"+potion.ItemId).onClick.Invoke();Helper("Hit",controller,healer);yield return Ready(controller);Restored(controller,"item/use");Check(InventoryService.GetItemCount(potion.ItemId)==count-1,"item/consume","실제 사용 수량1 소비");
            Actor(controller);Click(controller,"defendButton");yield return Ready(controller);Check(healer.IsDefending,"command/guard","정상 방어 행동");
        }
        static IEnumerator Win()
        {
            var c=Object.FindAnyObjectByType<BattleSceneController>();int attacks=0;double end=EditorApplication.timeSinceStartup+180;
            while(!Get<bool>(c,"battleEnded")&&EditorApplication.timeSinceStartup<end)
            {
                if(!Get<bool>(c,"actionPlaying")&&Get<Button>(c,"attackButton").IsActive()&&Get<Button>(c,"attackButton").interactable){Click(c,"attackButton");var targets=Get<IReadOnlyList<Combatant>>(c,"selectableTargets");if(Get<bool>(c,"choosingTarget")&&targets.Count>0){Helper("Hit",c,targets[0]);attacks++;}}
                yield return null;
            }
            Check(Get<Formation>(c,"enemies").IsDefeated&&attacks>0,"normal-victory","정상 공격/연출/턴 승리·강제 KO 없음; attacks="+attacks);
        }
        static IEnumerator Continue(string scene)
        {
            var player=Object.FindAnyObjectByType<PlayerController>();Check(GameSaveService.SaveCurrentWorldPosition(player.transform.position,scene),"save/write","격리 실제 Save");
            string beast=JsonUtility.ToJson(BeastCompanionService.ExportSaveData()),party=JsonUtility.ToJson(CompanionRosterService.ExportSaveData());int level=GameSessionData.Level;
            yield return Load("Bootstrap");GameSessionData.Reset();GameObject.Find("StartMenuCanvas/Slot01/Action").GetComponent<Button>().onClick.Invoke();yield return Wait(2);
            Check(SceneManager.GetActiveScene().name==scene&&GameSessionData.Level==level&&JsonUtility.ToJson(BeastCompanionService.ExportSaveData())==beast&&JsonUtility.ToJson(CompanionRosterService.ExportSaveData())==party,"save/continue","실제 Bootstrap Continue→Scene/성장/Party/Beast 보존");
        }
        static IEnumerator Flow()
        {
            Seed("mage",12);Helper("PastQuests","main_10");QuestService.TryStart(MainQuest10FieldFlow.QuestId);CompanionRosterService.UnlockPaul("mage");GameSessionData.RecordLocation("Dungeon_01","");yield return Load("Dungeon_01");
            var spawn=Resources.LoadAll<FieldMonsterSpawnDefinition>("MonsterSpawns").First(s=>s.SceneName=="Dungeon_01"&&!s.NonRespawningBoss);
            typeof(MonsterEncounterService).GetField("suppressedUntil",Static).SetValue(null,0f);Check(MonsterEncounterService.TryRaise(spawn.Monster,spawn),"encounter","실제 Encounter 서비스→Battle·물리접촉과 구분");yield return Wait(2);
            var c=Object.FindAnyObjectByType<BattleSceneController>();Check(c!=null,"enter-battle","일반 Battle 도착");yield return Wait(2);Actor(c);Click(c,"itemButton");HiddenCommands(c,"normal/item");Click(c,"cancelButton");Click(c,"fleeButton");yield return Wait(2);Check(SceneManager.GetActiveScene().name=="Dungeon_01","command/flee","도망 정상 복귀·승리 보상 없음");
            yield return Wait(2.1);MonsterEncounterService.TryRaise(spawn.Monster,spawn);yield return Wait(4);yield return Win();
            Object.FindObjectsByType<Button>().First(b=>b.name=="VictoryReturn").onClick.Invoke();yield return Wait(2);Check(SceneManager.GetActiveScene().name=="Dungeon_01","victory/return","실제 결과 버튼→Dungeon");yield return Continue("Dungeon_01");
        }
        // Story는 저작된 원문을 실제 Presenter에 보내 기술 연결만 확인합니다. 음성 의미 청취는 별도입니다.
        static IEnumerator Dialogue(Type actorType,string method,string label)
        {
            var lines=(DialogueLine[])actorType.GetMethod(method,Static).Invoke(null,null);var d=DialoguePresenter.Instance;
            d.ShowSequence(lines,null);
            for(int i=0;i<lines.Length;i++)
            {
                var line=lines[i];yield return null;
                Check(Get<Text>(d,"dialogueText").text==line.Message,label+"/text"+i,"저작된 전체 자막 일치");
                var expected=!line.IsPlayer&&!line.IsDirection?Get<VoiceClipCatalog>(d,"voiceCatalog").Find(line.DialogueId,line.SpeakerId):null;
                Check(Get<VoicePlaybackSource>(d,"voicePlayback").Clip==expected,label+"/voice"+i,"정본 Clip 연결/Player무음; 의미 청취 아님");
                Check(line.IsPlayer?!Get<GameObject>(d,"portraitRoot").activeSelf:Get<GameObject>(d,"portraitRoot").activeSelf,label+"/portrait"+i,"Player Portrait 없음/Character Portrait 표시");d.Advance();
            }
            Check(!d.IsOpen,label+"/close","대화 종료 후 modal 정리");
        }
        static IEnumerator Integration()
        {
            // 이전 실패는 QA 버튼 경로 오류였습니다. 새로운 격리 세션에서 그 Continue만 다시 준비합니다.
            if(!Results.Any(r=>r.StartsWith("PASS|save/continue|"))){Seed("mage",12);Helper("PastQuests","main_10");QuestService.TryStart(MainQuest10FieldFlow.QuestId);CompanionRosterService.UnlockPaul("mage");GameSessionData.RecordLocation("Dungeon_01","");yield return Load("Dungeon_01");yield return Continue("Dungeon_01");}
            foreach(int main in new[]{3,4})
            {
                Seed("mage",12);Helper("PastQuests",main==3?"main_03":"main_04");GameSessionData.RecordLocation("Field_01","");yield return Load("Field_01");
                // 자연 조우는 실제 NPC 근처에서 일어납니다. 테스트의 기본 입구 좌표에 복귀 이격을 더하면
                // 마을 Exit에 들어가므로, 포털과 떨어진 유효 전투 위치를 준비합니다.
                Object.FindAnyObjectByType<PlayerController>().transform.position=Vector2.zero;Physics2D.SyncTransforms();MonsterEncounterService.SuppressForSeconds(100f);
                Type actorType=main==3?typeof(MainQuest03TaeonActor):typeof(MainQuest04MielActor);string label="Main"+main;
                if(!Results.Any(r=>r.StartsWith("PASS|"+label+"/before/close|")))yield return Dialogue(actorType,"FirstConversation",label+"/before");
                string roster=JsonUtility.ToJson(CompanionRosterService.ExportSaveData());
                var spawn=Resources.LoadAll<FieldMonsterSpawnDefinition>("MonsterSpawns").First(s=>s.SceneName=="Field_01");
                Check(BattleSceneFlow.EnterStoryBattle(main==3?MainQuest03FieldFlow.EncounterId:MainQuest04FieldFlow.EncounterId,spawn.Monster,spawn,Vector2.right),label+"/enter","실제 Story 전환 서비스; 자연 Quest 전수 재생은 기존 근거 재사용");yield return Wait(4);
                var c=Object.FindAnyObjectByType<BattleSceneController>();Check(Get<Formation>(c,"allies").Members.Count()==(main==3?2:3),label+"/temporary-party","정식 Story 임시 편성 2/3명");Actor(c);Click(c,"skillButton");HiddenCommands(c,label+"/skill");Click(c,"cancelButton");Restored(c,label+"/cancel");Click(c,"itemButton");HiddenCommands(c,label+"/item");Click(c,"cancelButton");
                yield return Win();Object.FindObjectsByType<Button>().First(b=>b.name=="VictoryReturn").onClick.Invoke();yield return Wait(2);
                Check(SceneManager.GetActiveScene().name=="Field_01"&&JsonUtility.ToJson(CompanionRosterService.ExportSaveData())==roster,label+"/return","Scene="+SceneManager.GetActiveScene().name+" before="+roster+" after="+JsonUtility.ToJson(CompanionRosterService.ExportSaveData()));yield return Dialogue(actorType,"AfterBattleConversation",label+"/after");
            }
            Seed("mage",12);Helper("PastQuests","main_10");QuestService.TryStart(MainQuest10FieldFlow.QuestId);CompanionRosterService.UnlockPaul("mage");GameSessionData.ConfigureProgress(12,23);EconomyService.Import(123);GameSessionData.ActivateSafeZone("safezone_catacomb_entrance","Field_03","Spawn_From_Dungeon01");GameSessionData.RecordLocation("Dungeon_01_B2","");
            var dungeon=Resources.LoadAll<FieldMonsterSpawnDefinition>("MonsterSpawns").First(s=>s.SceneName=="Dungeon_01_B2"&&!s.NonRespawningBoss);BattleEncounterContext.Set(dungeon.Monster,dungeon,null,null,Vector2.zero,Vector2.zero);yield return Load("Battle");var battle=Object.FindAnyObjectByType<BattleSceneController>();Actor(battle);Click(battle,"itemButton");HiddenCommands(battle,"defeat/menu");Click(battle,"cancelButton");
            foreach(var ally in Get<Formation>(battle,"allies").Members)ally.TakeDamage(int.MaxValue,false);Call(battle,"AdvanceTurn");yield return Wait(3);
            Check(SceneManager.GetActiveScene().name=="Field_03"&&GameSessionData.Level==12&&GameSessionData.CurrentExperience==23&&EconomyService.GetCurrency()==123,"defeat/return","전멸 Fixture→최근 안전지대/성장·재화 보존");
            Check(PartyResourceService.ExportSaveData().All(r=>PartyResourceService.TryGet(r.CharacterId,out var p)&&p.CurrentHp==p.MaxHp&&p.CurrentMp==p.MaxMp),"defeat/resources","전멸 HP/MP 복원");yield return Continue("Field_03");
        }
        static IEnumerator Smoke()
        {
            Seed("healer");yield return Load("Battle");var c=Object.FindAnyObjectByType<BattleSceneController>();Actor(c);
            Click(c,"skillButton");for(int frame=0;frame<5;frame++){yield return null;Check(Commands(c).All(b=>!b.gameObject.activeInHierarchy&&!b.interactable),"frames/skill"+frame,"하위 메뉴 전환 직후5회 표본 숨김 유지");}
            Get<List<Button>>(c,"skillMenuButtons").Single(b=>b.name=="Back").onClick.Invoke();Restored(c,"skill/back");
            Click(c,"itemButton");for(int frame=0;frame<5;frame++){yield return null;Check(Commands(c).All(b=>!b.gameObject.activeInHierarchy&&!b.interactable),"frames/item"+frame,"빈 아이템 목록도 기본 버튼 노출 없음");}
            Get<List<Button>>(c,"itemMenuButtons").Single(b=>b.name=="ItemBack").onClick.Invoke();Restored(c,"item/back");
            for(int frame=0;frame<5;frame++){yield return null;Check(Commands(c).All(b=>b.gameObject.activeInHierarchy&&b.interactable)&&EventSystem.current.currentSelectedGameObject==Get<Button>(c,"itemButton").gameObject,"frames/restore"+frame,"복귀 뒤 표시·입력·원래 포커스 유지");}
            var module=EventSystem.current.GetComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
            Check(module!=null&&module.move.action.enabled&&module.submit.action.enabled&&module.cancel.action.enabled,"input/actions","실제 Move/Submit/Cancel Action 활성 연결; 실물 입력과 구분");
            CancelBindings(module);
        }
        static void CancelBindings(UnityEngine.InputSystem.UI.InputSystemUIInputModule module)
        {
            // 공식 Gamepad/Keyboard 레이아웃은 B와 Esc에 Cancel usage를 선언합니다. 기본 Action은
            // 기기별 path 대신 */{Cancel}로 두 입력을 묶으므로 명시적 path만 기대하면 안 됩니다.
            var paths=module.cancel.action.bindings.Select(b=>b.effectivePath??b.path).ToArray();
            Check(paths.Any(p=>p.Contains("{Cancel}"))||(paths.Any(p=>p.Contains("buttonEast"))&&paths.Any(p=>p.Contains("escape"))),"input/cancel-bindings","실제 Cancel 연결="+string.Join(",",paths)+"; 실물 입력 PASS 아님");
        }
        static IEnumerator Bindings()
        {
            Seed("healer");yield return Load("Battle");CancelBindings(EventSystem.current.GetComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>());
        }
        static IEnumerator Run()
        {
            uint width,height;PlayModeWindow.GetRenderingResolution(out width,out height);
            try{if(bindingsOnly)yield return Bindings();else if(smokeOnly)yield return Smoke();else if(integrationOnly)yield return Integration();else{yield return Menus();yield return Flow();}yield return Load("Bootstrap");Results.Add("USER_INPUT_REQUIRED|physical-keyboard|실물 입력 미실행");Results.Add("USER_INPUT_REQUIRED|physical-gamepad|실물 입력 미실행");Save();}
            finally{PlayModeWindow.SetCustomRenderingResolution(width,height,"Restored");}
        }
    }
}

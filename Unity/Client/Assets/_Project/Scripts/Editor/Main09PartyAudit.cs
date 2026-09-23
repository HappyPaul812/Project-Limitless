#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ProjectLimitless.Battle;
using ProjectLimitless.Core;
using ProjectLimitless.Monster;
using ProjectLimitless.NPC;
using ProjectLimitless.Player;
using ProjectLimitless.UI;
using ProjectLimitless.World;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace ProjectLimitless.Editor
{
    /// <summary>사용자 저장과 분리된 실제 Scene·대화·UI·Battle·Continue 회귀 감사입니다.</summary>
    [InitializeOnLoad]
    public static class Main09PartyAudit
    {
        public static readonly List<string> Results = new List<string>();
        public static string Status { get; private set; } = "Idle";
        private static IEnumerator routine;
        private static double nextTick;
        private static readonly BindingFlags Private = BindingFlags.Instance | BindingFlags.NonPublic;
        static Main09PartyAudit()
        {
            EditorApplication.update += Tick;
            EditorApplication.playModeStateChanged += state =>
            {
                if (state == PlayModeStateChange.EnteredEditMode) GameSaveService.FinishAudit();
            };
        }
        public static void Start()
        {
            if (!EditorApplication.isPlaying) throw new InvalidOperationException("Play Mode에서 실행해주세요.");
            Results.Clear(); Status = "Running";
            Application.runInBackground = true;
            GameSaveService.AuditSaveDirectory = System.IO.Path.Combine(Application.dataPath, "../UserData/Main09Audit", Guid.NewGuid().ToString("N"));
            GameSaveService.SelectSlot(1);
            routine = Run();
        }
        private static void Tick()
        {
            if (routine == null || EditorApplication.timeSinceStartup < nextTick) return;
            nextTick = EditorApplication.timeSinceStartup + .1;
            try
            {
                if (!routine.MoveNext()) { routine = null; Status = "PASS"; }
            }
            catch (Exception exception)
            { Results.Add("FAIL " + exception); routine = null; Status = "FAIL"; }
        }
        private static void Check(bool condition, string label)
        { if (!condition) throw new InvalidOperationException(label); Results.Add("PASS " + label); }
        private static object Field(object owner, string name) => owner.GetType().GetField(name, Private).GetValue(owner);
        private static object Call(object owner, string name, params object[] args) => owner.GetType().GetMethod(name, Private).Invoke(owner,args);
        private static IEnumerator WaitScene(string name)
        {
            int ticks = 0;
            while (SceneManager.GetActiveScene().name != name && ticks++ < 250) yield return null;
            if (SceneManager.GetActiveScene().name != name) throw new TimeoutException(name);
            for (int i=0;i<8;i++) yield return null;
        }
        private static string RosterJson() => JsonUtility.ToJson(CompanionRosterService.ExportSaveData());
        private static void Seed()
        {
            GameSessionData.Reset(); GameSessionData.ConfigurePlayer(PlayerVisualType.Male,"감사 플레이어");
            GameSessionData.SelectPlayerPath(PathCombatTraitRuntime.MobilityPathId); GameSessionData.SelectJob("guardian");
            GameSessionData.ConfigureProgress(12,0); CompanionRosterService.UnlockIntroCompanions();
            QuestService.ImportSaveData(new QuestProgressSaveData { CompletedQuestIds = QuestCatalog.All.Where(x=>x.QuestId != MainQuest09FieldFlow.QuestId).Select(x=>x.QuestId).ToArray() });
            GameSessionData.RecordLocation("Field_03","Spawn_From_Field02"); GameSessionData.SetPendingSpawnPoint("Spawn_From_Field02");
        }
        private static void ServiceChecks()
        {
            foreach (string job in new[]{"guardian","healer","sharpshooter","fighter","mage"})
            {
                CompanionRosterService.Reset(); CompanionRosterService.UnlockIntroCompanions(); CompanionRosterService.UnlockPaul(job);
                string expected = job == "guardian" ? "companion_miel,companion_paul" : job == "healer" ? "companion_taeon,companion_paul" : "companion_taeon,companion_miel";
                Check(string.Join(",",CompanionRosterService.ActivePartyCharacterIds) == expected, "직업 기본 편성 " + job);
                string before = RosterJson(); CompanionRosterService.UnlockPaul(job); Check(RosterJson() == before,"중복 해금 무변경 " + job);
                Check(CompanionRosterService.HasOffensiveRole(job,new[]{CompanionRosterService.TaeonId,CompanionRosterService.MielId}) == (job != "guardian" && job != "healer"),"Player 포함 공격 역할 " + job);
            }
            CompanionRosterService.Reset(); CompanionRosterService.UnlockIntroCompanions();
            var rows = new Dictionary<string,FormationRow>{{"player",FormationRow.Rear},{CompanionRosterService.TaeonId,FormationRow.Rear},{CompanionRosterService.MielId,FormationRow.Rear}};
            Check(CompanionRosterService.TrySetComposition(new[]{CompanionRosterService.TaeonId,CompanionRosterService.MielId},rows),"Main05 수동 편성");
            CompanionRosterService.UnlockPaul("guardian"); CompanionRosterService.UnlockIntroCompanions();
            Check(!CompanionRosterService.IsActivePartyMember(CompanionRosterService.PaulId) && CompanionRosterService.GetRow("player") == FormationRow.Rear,"Paul 자동 편성이 수동 선택 보존");
            Check(new[]{"player",CompanionRosterService.TaeonId,CompanionRosterService.MielId}.Select(CompanionRosterService.GetSlot).Distinct().Count() == 3,"동일 행 세 명 슬롯 중복 없음");
            Check(!CompanionRosterService.TrySetComposition(new[]{CompanionRosterService.PaulId,CompanionRosterService.PaulId},rows),"중복 선택 거부");
            CompanionRosterService.ImportSaveData(JsonUtility.FromJson<CompanionRosterSaveData>("{\"UnlockedCharacterIds\":[\"companion_taeon\",\"companion_miel\"],\"ActivePartyCharacterIds\":[\"companion_taeon\",\"companion_miel\"]}"));
            Check(CompanionRosterService.GetRow(CompanionRosterService.TaeonId)==FormationRow.Front && CompanionRosterService.GetRow(CompanionRosterService.MielId)==FormationRow.Rear,"구버전 Formation 누락 기본값");
            CompanionRosterService.ImportSaveData(null); Check(CompanionRosterService.UnlockedCharacterIds.Count==0,"구버전 Roster 누락 호환");
        }
        private static IEnumerator RoundTrip(string label)
        {
            string scene = SceneManager.GetActiveScene().name;
            string roster = RosterJson(); string quest = JsonUtility.ToJson(QuestService.ExportSaveData());
            var player = Object.FindAnyObjectByType<PlayerController>();
            Check(GameSaveService.SaveCurrentWorldPosition(player.transform.position,scene),label+" Save");
            SceneManager.LoadSceneAsync("Bootstrap"); var wait=WaitScene("Bootstrap"); while(wait.MoveNext()) yield return null;
            Check(GameObject.Find("StartMenuCanvas/Slot01/Action") != null,label+" Bootstrap");
            GameObject.Find("StartMenuCanvas/Slot01/Action").GetComponent<Button>().onClick.Invoke();
            wait=WaitScene(scene); while(wait.MoveNext()) yield return null;
            Check(RosterJson()==roster && JsonUtility.ToJson(QuestService.ExportSaveData())==quest,label+" Continue 명단·진형·목표 일치");
            if(QuestService.ActiveMainQuest?.Definition.QuestId==MainQuest09FieldFlow.QuestId)
                Check(Object.FindAnyObjectByType<QuestNavigationPresenter>().HasTarget,label+" Navigation 복원");
        }
        private static IEnumerator Run()
        {
            ServiceChecks(); Seed(); MonsterEncounterService.SuppressForSeconds(3600);
            SceneManager.LoadSceneAsync("Field_03"); var wait=WaitScene("Field_03"); while(wait.MoveNext()) yield return null;
            Check(QuestService.ActiveMainQuest?.Definition.QuestId==MainQuest09FieldFlow.QuestId,"Main08 완료 뒤 Main09 시작");
            wait=RoundTrip("A 폴 만나기 전"); while(wait.MoveNext()) yield return null;
            int currency = EconomyService.GetCurrency(); int exp = GameSessionData.CurrentExperience;
            for(int step=0;step<6;step++)
            {
                Check(QuestService.ActiveMainQuest.CurrentObjectiveIndex==step,"Main09 순차 목표 "+step);
                string targetId=QuestService.ActiveMainQuest.CurrentObjective.TargetId;
                Check(QuestNavigationTargetRegistry.TryGet(targetId,out var target),"목표 Registry "+step);
                Check(Object.FindAnyObjectByType<QuestNavigationPresenter>().HasTarget,"화면 목표 안내 "+step);
                var actor=Object.FindObjectsByType<MainQuest09Interactable>().First(x=>x.TargetId==targetId);
                if(step==0)Check(actor.transform.Find("QuestMarker/Text")?.GetComponent<Text>().text.Contains("E/F") == true,"조사 입력 안내");
                var player=Object.FindAnyObjectByType<PlayerController>();
                player.transform.position=actor.transform.position+Vector3.down*.65f;
                var body=player.GetComponent<Rigidbody2D>(); if(body!=null){body.position=player.transform.position;body.linearVelocity=Vector2.zero;}
                yield return null;
                Check(actor.TryInteract(),"실제 거리 상호작용 "+step);
                Check(WorldModalState.IsOpen && DialoguePresenter.Instance.IsOpen,"대화 Modal "+step);
                if(step==0)
                {
                    DialoguePresenter.Instance.Hide(); Check(QuestService.ActiveMainQuest.CurrentObjectiveIndex==0,"대화 취소 진행 없음");
                    Check(actor.TryInteract(),"취소 후 대화 재시작");
                }
                for(int page=0;page<MainQuest09Dialogue.Lines(step).Length;page++){DialoguePresenter.Instance.Advance();yield return null;}
                Check(!DialoguePresenter.Instance.IsOpen,"연속 대화 종료 "+step);
                if(step==2){wait=RoundTrip("B 폴 재회 후");while(wait.MoveNext())yield return null;}
            }
            Check(QuestService.GetState(MainQuest09FieldFlow.QuestId)==QuestState.Completed,"Main09 완료");
            Check(EconomyService.GetCurrency()==currency+35 && GameSessionData.CurrentExperience==exp+35,"EXP35·탈렌트35");
            Check(CompanionRosterService.IsUnlocked(CompanionRosterService.PaulId),"폴 영구 해금");
            QuestService.NotifyNpcTalked(CompanionRosterService.PaulId);
            Check(EconomyService.GetCurrency()==currency+35,"완료 중복 보상 없음");
            Check(Object.FindObjectsByType<MainQuest09Interactable>().Length==0,"완료 후 Story Actor 숨김");
            wait=RoundTrip("C Main09 완료");while(wait.MoveNext())yield return null;
            SceneManager.LoadSceneAsync("World_StarterVillage");wait=WaitScene("World_StarterVillage");while(wait.MoveNext())yield return null;
            var manager=Object.FindObjectsByType<VillageNpcRole>().First(x=>x.Role==VillageNpcRoleType.PartyManager);
            manager.Interact(manager.GetComponent<NpcController>()); var ui=PartyManagementPresenter.Instance;
            Check(ui.IsOpen && PlayerController.IsMovementLocked,"기존 PartyManager UI와 이동 잠금");
            QuestLogPresenter.Instance.Open();Check(!QuestLogPresenter.Instance.IsOpen,"QuestLog 동시 진입 차단");
            ui.ToggleCompanion(CompanionRosterService.PaulId);ui.ToggleCompanion(CompanionRosterService.TaeonId);
            string original=RosterJson();ui.Cancel();Check(RosterJson()==original,"UI 취소 원본 보존");
            manager.Interact(manager.GetComponent<NpcController>()); ui.ToggleCompanion(CompanionRosterService.PaulId);ui.ToggleCompanion(CompanionRosterService.TaeonId);
            ui.Confirm();Check(ui.IsWarningVisible,"공격 역할 없음 경고");ui.Cancel();Check(ui.IsOpen&&!ui.IsWarningVisible,"경고 돌아가기");
            ui.Confirm();ui.Confirm();Check(!ui.IsOpen&&CompanionRosterService.IsActivePartyMember(CompanionRosterService.TaeonId),"경고 승인 허용");
            string[][] pairs={new[]{CompanionRosterService.TaeonId,CompanionRosterService.MielId},new[]{CompanionRosterService.TaeonId,CompanionRosterService.PaulId},new[]{CompanionRosterService.MielId,CompanionRosterService.PaulId}};
            for(int pair=0;pair<pairs.Length;pair++)
            {
                manager=Object.FindObjectsByType<VillageNpcRole>().First(x=>x.Role==VillageNpcRoleType.PartyManager);
                manager.Interact(manager.GetComponent<NpcController>());ui=PartyManagementPresenter.Instance;
                foreach(string id in ui.DraftIds.ToArray())ui.ToggleCompanion(id);
                foreach(string id in pairs[pair])ui.ToggleCompanion(id);
                ui.ToggleRow("player");ui.ToggleRow(pairs[pair][0]);ui.Confirm();if(ui.IsWarningVisible)ui.Confirm();
                Check(CompanionRosterService.ActivePartyCharacterIds.SequenceEqual(pairs[pair]),"UI 편성 "+pair);
                if(pair>0){wait=RoundTrip(pair==1?"D 태온+폴":"E 미엘+폴");while(wait.MoveNext())yield return null;}
                string saved=RosterJson();
                SceneManager.LoadSceneAsync("Field_01");wait=WaitScene("Field_01");while(wait.MoveNext())yield return null;
                Check(!PartyManagementPresenter.OpenAt(Object.FindAnyObjectByType<PlayerController>().transform),"필드 편성 차단 "+pair);
                var spawn=Resources.LoadAll<FieldMonsterSpawnDefinition>("MonsterSpawns").First(x=>x.SceneName=="Field_01");
                var monster=Resources.LoadAll<MonsterDefinition>("MonsterDefinitions").First(x=>x.MonsterId=="grass_slime");
                typeof(BattleSceneFlow).GetMethod("EnterBattle",BindingFlags.Static|BindingFlags.NonPublic).Invoke(null,new object[]{monster,spawn});
                wait=WaitScene("Battle");while(wait.MoveNext())yield return null;
                var battle=Object.FindAnyObjectByType<BattleSceneController>();var allies=(Formation)Field(battle,"allies");
                Check(allies.Members.Select(x=>x.Id).OrderBy(x=>x).SequenceEqual(new[]{"player"}.Concat(pairs[pair]).OrderBy(x=>x)),"Normal Battle 실제 참가자 "+pair);
                Check(allies.Members.All(x=>x.Slot.Equals(CompanionRosterService.GetSlot(x.Id))),"Normal Battle 실제 진형 "+pair);
                int ticks=0;bool paulSkill=false;
                while(!(bool)Field(battle,"battleEnded") && ticks++<2400)
                {
                    if(!(bool)Field(battle,"actionPlaying"))
                    {
                        var actor=(Combatant)Field(battle,"currentActor");
                        if(actor!=null && actor.IsPlayerControlled)
                        {
                            if(actor.Id==CompanionRosterService.PaulId&&!paulSkill)
                            {
                                var job=Resources.LoadAll<JobDefinition>("JobDefinitions").First(x=>x.JobId=="mage");
                                var skill=BattleSkillCatalog.GetSkills(job).First(x=>x.Id==BattleSkillCatalog.MageFireballId);
                                if(skill!=null){Call(battle,"UseSkill",skill);paulSkill=true;}else Call(battle,"BeginAttack");
                            }
                            else Call(battle,"BeginAttack");
                            var targets=(IReadOnlyList<Combatant>)Field(battle,"selectableTargets");
                            if((bool)Field(battle,"choosingTarget")&&targets.Count>0)Call(battle,"SelectTarget",targets[0]);
                        }
                    }
                    yield return null;
                }
                Check((bool)Field(battle,"battleEnded") && ((Formation)Field(battle,"enemies")).IsDefeated,"Normal Battle 실제 승리 "+pair);
                Check(RosterJson()==saved,"Battle 후 편성 보존 "+pair);
                if(pair>0)Check(paulSkill,"Paul Mage 스킬 실행 "+pair);
                Object.FindObjectsByType<Button>().First(x=>x.name=="VictoryReturn").onClick.Invoke();
                wait=WaitScene("Field_01");while(wait.MoveNext())yield return null;
                SceneManager.LoadSceneAsync("World_StarterVillage");wait=WaitScene("World_StarterVillage");while(wait.MoveNext())yield return null;
            }
            string final=RosterJson();
            var story=BattlePrototypeEncounterFactory.CreateMain07ThreeVsThree("검증","guardian",PathCombatTraitRuntime.MobilityPathId,100,20,10,null,null);
            Check(story.Allies.Any(x=>x.Id==CompanionRosterService.PaulId)&&!story.Allies.Any(x=>x.Id==CompanionRosterService.MielId),"Main07 Story Override Player·태온·폴");
            Check(RosterJson()==final,"Story Factory 저장 편성 불변");
            // Main05 이전 안내인은 편성 화면을 열지 않고, 같은 Scene의 Main08 완료는 Main09를 시작합니다.
            QuestService.ImportSaveData(new QuestProgressSaveData());
            manager=Object.FindObjectsByType<VillageNpcRole>().First(x=>x.Role==VillageNpcRoleType.PartyManager);
            manager.Interact(manager.GetComponent<NpcController>());
            Check(PartyManagementPresenter.Instance==null || !PartyManagementPresenter.Instance.IsOpen,"Main05 이전 Party UI 차단");
            Check(DialoguePresenter.Instance.IsOpen,"Main05 이전 안내 대화"); DialoguePresenter.Instance.Hide();
            SceneManager.LoadSceneAsync("Field_03");wait=WaitScene("Field_03");while(wait.MoveNext())yield return null;
            var beforeNine=QuestCatalog.All.Where(x=>x.QuestId!=MainQuest09FieldFlow.QuestId && x.QuestId!=MainQuest08FieldFlow.QuestId).Select(x=>x.QuestId).ToArray();
            var main08=QuestCatalog.All.First(x=>x.QuestId==MainQuest08FieldFlow.QuestId);
            var progress=new ActiveQuestSaveData{QuestId=MainQuest08FieldFlow.QuestId,
                Objectives=main08.Objectives.Select((x,i)=>new QuestObjectiveProgressData{ObjectiveId=x.ObjectiveId,CurrentCount=i<6?1:0}).ToArray()};
            QuestService.ImportSaveData(new QuestProgressSaveData{CompletedQuestIds=beforeNine,ActiveQuests=new[]{progress}});
            Check(QuestService.ActiveMainQuest?.Definition.QuestId==MainQuest08FieldFlow.QuestId,"기존 Main08 마지막 단계");
            QuestService.NotifyLocationReached(MainQuest08FieldFlow.DeepZone);
            Check(QuestService.GetState(MainQuest08FieldFlow.QuestId)==QuestState.Completed && QuestService.ActiveMainQuest?.Definition.QuestId==MainQuest09FieldFlow.QuestId,
                "같은 Field03에서 Main08 완료→Main09 자동 시작");
            Results.Add("SAVE_DIRECTORY "+GameSaveService.AuditSaveDirectory);
        }
    }
}
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ProjectLimitless.Audio;
using ProjectLimitless.Battle;
using ProjectLimitless.Core;
using ProjectLimitless.Monster;
using ProjectLimitless.Player;
using ProjectLimitless.UI;
using ProjectLimitless.World;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjectLimitless.EditorTools
{
    /// <summary>사용자 Save와 포커스를 보호하며 재도전·동료 성장·PCM을 같은 격리 실행에서 검증합니다.</summary>
    [InitializeOnLoad]
    public static class Main07IntegratedAudit
    {
        const BindingFlags Flags=BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
        static bool armed,before,matrix; static Keyboard keyboard; static Gamepad gamepad;
        static InputSettings.BackgroundBehavior background; static InputSettings.EditorInputBehaviorInPlayMode editorInput;
        static bool inputChanged; static float volume;
        static List<string> checks=new List<string>();
        static string Root=>Path.GetFullPath(Path.Combine(Application.dataPath,"../../../Temp/Main07Integrated/"+(before?"before":"runtime")));
        public static string Status="Idle";
        static Main07IntegratedAudit()
        {
            EditorApplication.playModeStateChanged+=s=>
            {
                if(!armed)return;
                if(s==PlayModeStateChange.EnteredPlayMode)EditorApplication.delayCall+=()=>typeof(Partial9FixedSpriteAudit).GetField("routine",Flags).SetValue(null,Flatten(Run()));
                if(s==PlayModeStateChange.EnteredEditMode)
                {
                    if(keyboard!=null)InputSystem.RemoveDevice(keyboard);if(gamepad!=null)InputSystem.RemoveDevice(gamepad);
                    keyboard=null;gamepad=null;
                    if(inputChanged){InputSystem.settings.backgroundBehavior=background;InputSystem.settings.editorInputBehaviorInPlayMode=editorInput;inputChanged=false;}
                    AudioListener.volume=volume;armed=false;
                }
            };
        }
        // matrixOnly는 통과한 연속 Voice와 전투 행을 보존해, 실패한 fixture 이후부터 이어 검증합니다.
        // 이 결과는 단일 실행이라고 주장하지 않고 초기 실패 기록과 함께 보관합니다.
        public static string Launch(bool beforeFix=false,bool matrixOnly=false)
        {before=beforeFix;matrix=matrixOnly;checks.Clear();Directory.CreateDirectory(Root);if(matrix){checks.AddRange(File.ReadAllLines(Path.Combine(Root,"checks.txt")).TakeWhile(x=>!x.StartsWith("FAIL")));}else foreach(var file in Directory.GetFiles(Root))File.Delete(file);volume=AudioListener.volume;armed=true;Status="Running";return Partial9FixedSpriteAudit.Launch();}
        static object Field(object o,string n)=>o.GetType().GetField(n,Flags).GetValue(o);
        static object Call(object o,string n,params object[] a)=>o.GetType().GetMethod(n,Flags).Invoke(o,a);
        static void Check(bool yes,string label)
        {checks.Add((yes?"PASS ":"FAIL ")+label);File.WriteAllLines(Path.Combine(Root,"checks.txt"),checks);if(!yes){Status="FAIL";throw new Exception(label);}}
        static IEnumerator Flatten(IEnumerator r)
        {var stack=new Stack<IEnumerator>();stack.Push(r);while(stack.Count>0){var e=stack.Peek();if(!e.MoveNext()){stack.Pop();continue;}if(e.Current is IEnumerator)stack.Push((IEnumerator)e.Current);else yield return null;}}
        static IEnumerator Wait(int n=5){for(int i=0;i<n;i++)yield return null;}
        static IEnumerator Load(string scene)
        {GameSessionData.RecordLocation(scene,"");GameSessionData.ClearWorldPosition();SceneManager.LoadSceneAsync(scene);for(int i=0;i<600&&SceneManager.GetActiveScene().name!=scene;i++)yield return null;yield return Wait(15);Check(SceneManager.GetActiveScene().name==scene,"Loaded "+scene);}
        static void SetQuest(QuestDefinition q,int index)
        {
            QuestService.ImportSaveData(new QuestProgressSaveData{CompletedQuestIds=QuestCatalog.All.Where(x=>x.QuestId!=q.QuestId&&x.QuestType==QuestType.Main&&string.CompareOrdinal(x.QuestId,q.QuestId)<0).Select(x=>x.QuestId).Concat(q.PrerequisiteQuestIds).Distinct().ToArray(),ActiveQuests=new[]{new ActiveQuestSaveData{QuestId=q.QuestId,Objectives=q.Objectives.Select((o,i)=>new QuestObjectiveProgressData{ObjectiveId=o.ObjectiveId,CurrentCount=i<index?o.RequiredCount:0}).ToArray()}}});
            // 후반 목표 fixture의 완료 기록과 동료 해금도 실제 정상 진행처럼 일치시킵니다.
            // 이를 생략하면 구버전 Save 복원용 UnlockPaul이 Continue에서 처음 실행되는 QA 오류가 납니다.
            if(QuestService.GetState("main_05_return_of_three")==QuestState.Completed)CompanionRosterService.UnlockIntroCompanions();
            if(QuestService.GetState("main_09_reunion_in_silence")==QuestState.Completed)CompanionRosterService.UnlockPaul(GameSessionData.SelectedJobId);
            if(QuestService.GetState("main_15_burning_traces")==QuestState.Completed)CompanionRosterService.UnlockSerin();
        }
        static void Pulse(Key key)
        {InputSystem.QueueStateEvent(keyboard,new KeyboardState(key));InputSystem.Update();InputSystem.QueueStateEvent(keyboard,new KeyboardState());InputSystem.Update();}
        static void APulse()
        {InputSystem.QueueStateEvent(gamepad,new GamepadState().WithButton(GamepadButton.South));InputSystem.Update();InputSystem.QueueStateEvent(gamepad,new GamepadState());InputSystem.Update();}
        static MainQuest07Interactable Paul=>UnityEngine.Object.FindObjectsByType<MainQuest07Interactable>().First(x=>(string)Field(x,"targetId")==MainQuest07FieldFlow.PaulId);
        static IEnumerator Near07(string id)
        {var site=UnityEngine.Object.FindObjectsByType<MainQuest07Interactable>().First(x=>(string)Field(x,"targetId")==id);var p=UnityEngine.Object.FindAnyObjectByType<PlayerController>();p.transform.position=site.transform.position;yield return Wait();site.GetType().GetField("nearby",Flags).SetValue(site,p);}
        static IEnumerator Run()
        {
            yield return Wait(20);Check(!string.IsNullOrEmpty(GameSaveService.AuditSaveDirectory),"Isolated Save active");GameSaveService.SelectSlot(1);
            GameSessionData.ConfigurePlayer(PlayerVisualType.Male,"종합 QA");GameSessionData.SelectPlayerPath("path.vision");GameSessionData.SelectJob("mage");GameSessionData.ConfigureProgress(3,0);CompanionRosterService.UnlockIntroCompanions();
            UserSettingsService.SetMuteAll(false);AudioListener.volume=0;
            background=InputSystem.settings.backgroundBehavior;editorInput=InputSystem.settings.editorInputBehaviorInPlayMode;inputChanged=true;
            InputSystem.settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;InputSystem.settings.editorInputBehaviorInPlayMode=InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
            keyboard=InputSystem.AddDevice<Keyboard>();gamepad=InputSystem.AddDevice<Gamepad>();
            QuestDefinition q;QuestCatalog.TryGet(MainQuest07FieldFlow.QuestId,out q);SetQuest(q,4);yield return Load("Field_02");yield return Near07(MainQuest07FieldFlow.PaulId);
            if(before)
            {
                Check(!(bool)Call(Paul,"Current"),"BEFORE Encounter objective Paul Current=false reproduced");Pulse(Key.E);yield return Wait();Check(SceneManager.GetActiveScene().name=="Field_02"&&!DialoguePresenter.Instance.IsOpen,"BEFORE E cannot retry battle");
                var setup=BattlePrototypeEncounterFactory.CreateMain07ThreeVsThree("QA","mage","path.vision",104,40,12,null,null).Allies.First(x=>x.Id==CompanionRosterService.PaulId);
                Check(setup.Attack==14&&setup.MaxHp==96&&setup.Agility==10,"BEFORE Lv3 Paul fixed HP96 Attack14 Agility10");
            }
            else yield return FinalRun();
            yield return Load("Bootstrap");Status="PASS";File.WriteAllText(Path.Combine(Root,"final.txt"),"PASS\n"+string.Join("\n",checks));
        }
        static IEnumerator FinalRun()
        {
            if(!matrix){Growth();yield return Integrated07();}
            foreach(var q in QuestCatalog.All.Where(x=>x.QuestType==QuestType.Main).OrderBy(x=>x.QuestId))
                for(int i=0;i<q.Objectives.Count;i++)if(q.Objectives[i].ObjectiveType==QuestObjectiveType.DefeatEncounter)
                {
                    if(matrix&&File.Exists(Path.Combine(Root,"retry-matrix.csv"))&&File.ReadAllLines(Path.Combine(Root,"retry-matrix.csv")).Any(x=>x.Split(',')[1]==q.Objectives[i].TargetId))continue;
                    yield return Encounter(q,i);
                }
            yield return FleeVictory07();yield return PartyContinue();DamageFixture();
            Check(UnityEngine.Object.FindObjectsByType<Transform>().All(x=>GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(x.gameObject)==0),"Runtime Missing Script0");
        }
        // 입력과 실제 Scene 진입을 쓰되, 전수 승패 조건은 테스트 fixture로 만들어 모든 목표 경계를 검사합니다.
        static IEnumerator BattleReady()
        {
            for(int i=0;i<600&&SceneManager.GetActiveScene().name!="Battle";i++)yield return null;
            yield return Wait(30);Check(SceneManager.GetActiveScene().name=="Battle","Actual Battle scene");
        }
        static BattleSceneController Controller=>UnityEngine.Object.FindAnyObjectByType<BattleSceneController>();
        static Formation Allies=>(Formation)Field(Controller,"allies");
        static Formation Enemies=>(Formation)Field(Controller,"enemies");
        static IEnumerator Back(string scene)
        {for(int i=0;i<600&&SceneManager.GetActiveScene().name!=scene;i++)yield return null;yield return Wait(15);File.AppendAllText(Path.Combine(Root,"return-observations.txt"),"expected="+scene+" actual="+SceneManager.GetActiveScene().name+" controller="+(Controller==null?"none":"ended="+Field(Controller,"battleEnded")+" action="+Field(Controller,"actionPlaying")+" message="+((Text)Field(Controller,"messageText")).text)+"\n");Check(SceneManager.GetActiveScene().name==scene,"Actual return "+scene);}
        static IEnumerator Finish(bool voices=false)
        {
            var d=DialoguePresenter.Instance;
            for(int n=0;n<80&&d.IsOpen;n++)
            {
                var lines=(DialogueLine[])Field(d,"sequenceLines");int page=(int)Field(d,"sequencePageIndex");var line=lines[page];
                if(voices&&!string.IsNullOrEmpty(line.DialogueId))yield return Voice(line,page);
                if(line.IsPlayer)Check(((VoicePlaybackSource)Field(d,"voicePlayback")).Clip==null,"Player silent");
                yield return Wait(3);Pulse(Key.E);
            }
            Check(!d.IsOpen,"Explicit sequence completion");yield return Wait(4);
        }
        // 실제 Presenter의 Clip·페이지·AudioSource를 관찰하고 GetData를 WAV PCM과 비교할 파일로 내보냅니다.
        // 사람 청취의 의미 판정은 이 기술 검증과 별개입니다.
        static IEnumerator Voice(DialogueLine line,int page)
        {
            var d=DialoguePresenter.Instance;var v=(VoicePlaybackSource)Field(d,"voicePlayback");var clip=v.Clip;
            var catalog=Resources.Load<VoiceClipCatalog>("Audio/Voice/Story/StoryVoiceCatalog");
            Check(clip!=null&&clip==catalog.Find(line.DialogueId,line.SpeakerId)&&clip.name==line.DialogueId&&v.IsPlaying,"Voice resolve "+line.DialogueId);
            var pcm=new float[clip.samples*clip.channels];Check(clip.GetData(pcm,0),"PCM readable "+clip.name);
            using(var w=new BinaryWriter(File.Open(Path.Combine(Root,clip.name+".pcm-f32"),FileMode.Create)))foreach(float x in pcm)w.Write(x);
            var source=(AudioSource)Field(v,"source");int last=0;bool ended=false;double started=EditorApplication.timeSinceStartup;
            while(EditorApplication.timeSinceStartup-started<clip.length+5 && !ended)
            {
                if(!d.IsOpen||(int)Field(d,"sequencePageIndex")!=page||v.Clip!=clip)throw new Exception("Voice automatic page/clip change "+clip.name);
                if(source.isPlaying){if(ended||source.timeSamples<last)throw new Exception("Voice replay "+clip.name);last=source.timeSamples;}else ended=true;
                yield return null;
            }
            ended=ended||!source.isPlaying;
            Check(ended&&!v.IsPlaying&&UnityEngine.Object.FindObjectsByType<VoicePlaybackSource>().Length==1,"Natural voice stop singleSource "+clip.name);
            File.AppendAllText(Path.Combine(Root,"voice-pages.csv"),line.DialogueId+","+page+","+clip.length+"\n");
        }
        // 실제 Save/Restore API의 같은 경계를 사용하며 명단·행·Beast·현재 자원을 JSON 값으로 비교합니다.
        // 아래 PartyContinue는 Bootstrap의 실제 이어하기 진입점도 추가로 실행합니다.
        static void Continue(string label)
        {
            var expected=QuestService.ActiveMainQuest?.CurrentObjective?.TargetId;int level=GameSessionData.Level;
            string roster=JsonUtility.ToJson(CompanionRosterService.ExportSaveData()),beast=JsonUtility.ToJson(BeastCompanionService.ExportSaveData()),resources=JsonUtility.ToJson(new ResourcesProbe{values=PartyResourceService.ExportSaveData()});
            Check(GameSaveService.SaveCurrentSession(),"Save "+label);GameSaveData data;Check(GameSaveService.TryLoadSlot(1,out data),"Read Save "+label);GameSaveService.RestoreSession(1,data);
            Check(GameSessionData.Level==level&&QuestService.ActiveMainQuest?.CurrentObjective?.TargetId==expected,"Continue Level/Objective "+label);
            string actualRoster=JsonUtility.ToJson(CompanionRosterService.ExportSaveData()),actualBeast=JsonUtility.ToJson(BeastCompanionService.ExportSaveData()),actualResources=JsonUtility.ToJson(new ResourcesProbe{values=PartyResourceService.ExportSaveData()});
            if(roster!=actualRoster||beast!=actualBeast||resources!=actualResources)File.AppendAllText(Path.Combine(Root,"continue-differences.txt"),label+"\nRoster "+roster+" => "+actualRoster+"\nBeast "+beast+" => "+actualBeast+"\nResources "+resources+" => "+actualResources+"\n");
            Check(roster==actualRoster&&beast==actualBeast&&resources==actualResources,"Continue Party Formation Beast HP-MP "+label);
        }
        [Serializable] sealed class ResourcesProbe{public PartyMemberResourceSaveData[] values;}
        // 동료별/대표 레벨별 새 참가자를 매번 생성해 중복 성장·역전·정수 범위와 Player 기준값을 기록합니다.
        static void Growth()
        {
            var rows=new List<string>{"character,job,level,max_hp,attack,agility,player_hp,player_attack,player_agility"};
            foreach(var c in CompanionCatalog.All)
            {
                BattleParticipantSetup previous=null;
                foreach(int level in new[]{1,3,5,10,20,50})
                {
                    GameSessionData.ConfigureProgress(level,0);var p=c.CreateParticipant(new FormationSlot(c.DefaultRow,0));var g=CharacterGrowthCalculator.Calculate(c.JobId,level);
                    Check(c.EffectiveLevel==level&&p.MaxHp>=1&&p.Attack>=1&&p.Agility>=1,"EffectiveLevel bounds "+c.CharacterId+" Lv"+level);
                    if(previous!=null)Check(p.MaxHp>=previous.MaxHp&&p.Attack>=previous.Attack&&p.Agility>=previous.Agility,"Monotonic growth "+c.CharacterId+" Lv"+level);
                    Check(p.MaxHp<10000&&p.Attack<10000&&p.Agility<1000,"No overflow "+c.CharacterId+" Lv"+level);previous=p;
                    rows.Add(string.Join(",",c.CharacterId,c.JobId,level,p.MaxHp,p.Attack,p.Agility,CharacterGrowthCalculator.CalculateMaxHp(c.JobId,g),CharacterGrowthCalculator.CalculateAttack(c.JobId,g),g.Agility));
                }
            }
            File.WriteAllLines(Path.Combine(Root,"growth.csv"),rows);GameSessionData.ConfigureProgress(3,0);
            var paul=CompanionCatalog.Find(CompanionRosterService.PaulId);var beforeSetup=paul.CreateParticipant(new FormationSlot(FormationRow.Rear,0));
            new RewardBundle{Experience=170}.TryApply();var afterSetup=paul.CreateParticipant(new FormationSlot(FormationRow.Rear,0));
            Check(GameSessionData.Level==4&&afterSetup.Attack>beforeSetup.Attack&&afterSetup.MaxHp>beforeSetup.MaxHp&&afterSetup.Agility>beforeSetup.Agility,"Actual EXP level-up next participant grows");
            GameSessionData.RecordLocation("Field_02","");Continue("Growth");Check(paul.EffectiveLevel==4&&paul.CreateParticipant(new FormationSlot(FormationRow.Rear,0)).Attack==afterSetup.Attack,"Companion growth after Continue");GameSessionData.ConfigureProgress(3,0);
        }
        static IEnumerator Integrated07()
        {
            QuestDefinition q;QuestCatalog.TryGet(MainQuest06ForestFlow.QuestId,out q);SetQuest(q,0);yield return Load("Field_02");
            var anomaly=UnityEngine.Object.FindAnyObjectByType<MainQuest06AnomalyTrace>();var player=UnityEngine.Object.FindAnyObjectByType<PlayerController>();player.transform.position=anomaly.transform.position;yield return Wait();anomaly.GetType().GetField("nearbyPlayer",Flags).SetValue(anomaly,player);Pulse(Key.E);yield return Finish();
            Check(QuestService.GetState(q.QuestId)==QuestState.Completed,"Main06 actual completion");yield return Load("Field_02");
            yield return Near07(MainQuest07FieldFlow.WheelTracksId);Pulse(Key.E);yield return Finish(true);
            yield return Near07(MainQuest07FieldFlow.WoundedTravelerId);Pulse(Key.F);yield return Finish(true);
            player=UnityEngine.Object.FindAnyObjectByType<PlayerController>();player.transform.position=MainQuest07FieldFlow.PaulPosition;yield return Wait(8);
            Check(QuestService.ActiveMainQuest.CurrentObjective.TargetId==MainQuest07FieldFlow.PaulId,"Paul actual location trigger");yield return Near07(MainQuest07FieldFlow.PaulId);Pulse(Key.E);yield return Finish(true);yield return BattleReady();
            Check(Allies.Members.Select(x=>x.Id).OrderBy(x=>x).SequenceEqual(new[]{"companion_paul","companion_taeon","player"}),"Main07 temporary party excludes Miel");
            var paul=Allies.Members.First(x=>x.Id==CompanionRosterService.PaulId);Check(paul.Attack==44&&paul.MaxHp==104&&paul.Agility==12,"Main07 actual Lv3 Paul 104/44/12");
            yield return WaitForCommands();Call(Controller,"Flee");yield return Back("Field_02");
            Check(QuestService.ActiveMainQuest.CurrentObjective.TargetId==MainQuest07FieldFlow.EncounterId,"Main07 Flee objective unchanged");Continue("Main07 Flee");yield return Load("Field_02");yield return Near07(MainQuest07FieldFlow.PaulId);Pulse(Key.F);yield return BattleReady();
            Check(DialoguePresenter.Instance==null||!DialoguePresenter.Instance.IsOpen,"Main07 F Retry no first conversation");
            foreach(var a in Allies.Members)a.TakeDamage(int.MaxValue,false);Call(Controller,"EndBattle","Fixture defeat",false);yield return Back(GameSessionData.LastSafeZoneSceneId);
            Check(QuestService.ActiveMainQuest.CurrentObjective.TargetId==MainQuest07FieldFlow.EncounterId,"Main07 Defeat objective unchanged");Continue("Main07 Defeat");yield return Load("Field_02");yield return Near07(MainQuest07FieldFlow.PaulId);APulse();yield return BattleReady();
            Check(DialoguePresenter.Instance==null||!DialoguePresenter.Instance.IsOpen,"Main07 A Retry no first conversation");
            // Main07은 정상 명령·피해·적 행동으로 승리하고, 전수 Matrix의 강제 승패 fixture와 구분합니다.
            yield return NaturalWin07();yield return Back("Field_02");
            Check(QuestService.ActiveMainQuest.CurrentObjective.TargetId==MainQuest07FieldFlow.ReturnToMielId,"Main07 natural Victory advances once");Continue("Main07 Victory");
            player=UnityEngine.Object.FindAnyObjectByType<PlayerController>();player.transform.position=MainQuest07FieldFlow.WoundedPosition;yield return Wait(10);
            yield return Near07(MainQuest07FieldFlow.MielMeetingId);Pulse(Key.E);yield return Finish(true);
            Check(QuestService.ActiveMainQuest.CurrentObjective.TargetId==MainQuest07FieldFlow.PaulFarewellId,"MielMeeting completion");
            yield return Near07(MainQuest07FieldFlow.PaulFarewellId);Pulse(Key.F);yield return Finish();Check(QuestService.GetState(MainQuest07FieldFlow.QuestId)==QuestState.Completed,"Main07 actual completion");
            Continue("Main07 Complete");Check(QuestService.GetState(MainQuest07FieldFlow.QuestId)==QuestState.Completed,"Main07 completed Continue");
        }
        static IEnumerator WaitForCommands()
        {
            for(int i=0;i<600;i++){if(Controller!=null&&!(bool)Field(Controller,"actionPlaying")&&((Combatant)Field(Controller,"currentActor"))?.IsPlayerControlled==true)yield break;yield return null;}
            throw new Exception("Player command timeout");
        }
        static IEnumerator NaturalWin07()
        {
            int turns=0;var damageRows=new List<string>{"actor,command,damage,attack"};
            while(!(bool)Field(Controller,"battleEnded")&&turns++<4000)
            {
                if((bool)Field(Controller,"actionPlaying")){yield return null;continue;}
                var actor=(Combatant)Field(Controller,"currentActor");if(actor==null||!actor.IsPlayerControlled){yield return null;continue;}
                var target=Enemies.Members.FirstOrDefault(x=>x.IsAlive);if(target==null){yield return Wait();continue;}
                int hp=target.CurrentHp;bool fire=actor.Id==CompanionRosterService.PaulId&&damageRows.All(x=>!x.StartsWith(actor.Id+",Fireball"));
                if(fire)
                {
                    var skill=BattleSkillCatalog.GetSkills(Resources.LoadAll<JobDefinition>("JobDefinitions").First(x=>x.JobId=="mage")).First(x=>x.Id==BattleSkillCatalog.MageFireballId);
                    Call(Controller,"PlayFireball",actor,target,skill);
                }
                else Call(Controller,"PlayBasicAttack",actor,target,100,null,false,false);
                for(int n=0;n<600&&(bool)Field(Controller,"actionPlaying");n++)yield return null;
                damageRows.Add(actor.Id+","+(fire?"Fireball":"Basic")+","+(hp-target.CurrentHp)+","+actor.Attack);yield return Wait(25);
            }
            Check((bool)Field(Controller,"battleEnded")&&Enemies.IsDefeated,"Main07 natural command victory");
            File.WriteAllLines(Path.Combine(Root,"paul-damage.csv"),damageRows);
            var button=UnityEngine.Object.FindObjectsByType<Button>().First(x=>x.name=="VictoryReturn");button.onClick.Invoke();
        }
        static string SceneFor(string id)
        {
            if(id.StartsWith("field01"))return "Field_01";if(id.StartsWith("field02"))return "Field_02";if(id.StartsWith("dungeon"))return "Dungeon_01_B2";
            if(id.StartsWith("field04"))return "Field_04_WesternBorder";if(id.StartsWith("field05"))return "Field_05_WesternOutskirts";
            if(id.StartsWith("field06"))return "Field_06_ScorchedTrail";if(id.StartsWith("field07"))return "Field_07_AshenReach";return "Field_08_RedRift";
        }
        // Story는 각 기존 상호작용을, 일반 필수 조우는 지정 스폰 위치에서 공용 접촉 이벤트를 사용합니다.
        // Collision 자체를 조작한 실물 플레이와 이벤트 fixture는 구분합니다.
        static IEnumerator Enter(string id)
        {
            var player=UnityEngine.Object.FindAnyObjectByType<PlayerController>();
            if(id==MainQuest07FieldFlow.EncounterId){yield return Near07(MainQuest07FieldFlow.PaulId);Pulse(Key.E);}
            else if(id==MainQuest03FieldFlow.EncounterId){var site=UnityEngine.Object.FindAnyObjectByType<MainQuest03TaeonActor>();player.transform.position=site.transform.position;site.TryInteract(player.transform,3);yield return Finish();}
            else if(id==MainQuest04FieldFlow.EncounterId){var site=UnityEngine.Object.FindAnyObjectByType<MainQuest04MielActor>();player.transform.position=site.transform.position;site.TryInteract(player.transform,3);yield return Finish();}
            else if(id==Chapter2Main16Flow.StoryEncounterId){var site=UnityEngine.Object.FindObjectsByType<Main16Site>().First(x=>(string)Field(x,"targetId")==Chapter2Main16Flow.WitnessId);player.transform.position=site.transform.position;yield return Wait();site.TryInteract();yield return Finish();}
            else if(id==Chapter2Main17Flow.EncounterId){var site=UnityEngine.Object.FindObjectsByType<Main17Site>().First(x=>(int)Field(x,"index")==7);player.transform.position=site.transform.position;yield return Wait();site.TryInteract();}
            else if(id==Chapter2IntroFlow.Main13Battle||id==Chapter2IntroFlow.Main14Battle){var site=UnityEngine.Object.FindObjectsByType<Chapter2ObjectiveSite>().First(x=>(string)Field(x,"targetId")==id);player.transform.position=site.transform.position;yield return Wait();Call(site,"TryInteract");}
            else
            {
                var spawn=Resources.LoadAll<FieldMonsterSpawnDefinition>("MonsterSpawns").First(x=>x.SceneName==SceneFor(id)&&x.SpawnId==(id==MainQuest02FieldFlow.EncounterId?MainQuest02FieldFlow.QuestSpawnId:id));
                MonsterEncounterService.SuppressForSeconds(.3f);player.transform.position=spawn.Position+Vector2.up*.8f;yield return Wait(55);Check(MonsterEncounterService.IsSpawnAvailable(spawn),"Contact spawn available "+id);Check(MonsterEncounterService.TryRaise(spawn.Monster,spawn),"Actual contact event "+id);
            }
            yield return BattleReady();Check(BattleEncounterContext.StableEncounterId==id,"Stable encounter context "+id);
        }
        // 각 실제 전투 Scene의 도망/패배/승리 처리와 목표 경계를 검사합니다. 전수 승패 조건은
        // HP fixture이며, Main07의 정상 공격 명령 승리와 구분해 Matrix에 한 행씩 기록합니다.
        static IEnumerator Encounter(QuestDefinition q,int index)
        {
            string id=q.Objectives[index].TargetId,scene=SceneFor(id);SetQuest(q,index);GameSessionData.ConfigureProgress(10,0);PartyResourceService.Reset();yield return Load(scene);yield return Enter(id);
            bool boss=Enemies.Members.Any(x=>x.IsBoss);yield return WaitForCommands();Call(Controller,"Flee");Check(boss|| (bool)Field(Controller,"battleEnded"),"Flee accepted "+id);
            if(boss)Check(!(bool)Field(Controller,"battleEnded")&&SceneManager.GetActiveScene().name=="Battle","Boss Flee prohibited "+id);
            else
            {
                yield return Back(scene);Check(QuestService.ActiveMainQuest.CurrentObjectiveIndex==index,"Flee objective count0 "+id);Continue("Flee "+id);yield return Load(scene);yield return Enter(id);
            }
            foreach(var a in Allies.Members)a.TakeDamage(int.MaxValue,false);Call(Controller,"EndBattle","Fixture defeat",false);yield return Back(GameSessionData.LastSafeZoneSceneId);
            Check(QuestService.ActiveMainQuest.CurrentObjectiveIndex==index,"Defeat objective count0 "+id);Continue("Defeat "+id);yield return Load(scene);yield return Enter(id);
            foreach(var e in Enemies.Members)e.TakeDamage(int.MaxValue,false);Call(Controller,"EndBattle","Fixture victory",true);
            Check(QuestService.ActiveMainQuest.CurrentObjectiveIndex==index+1,"Victory exact one objective "+id);
            string progress=JsonUtility.ToJson(QuestService.ExportSaveData());var savedLevel=GameSessionData.Level;var exp=GameSessionData.CurrentExperience;
            Call(Controller,"EndBattle","Duplicate victory",true);QuestService.NotifyEncounterWon(id);
            Check(progress==JsonUtility.ToJson(QuestService.ExportSaveData())&&savedLevel==GameSessionData.Level&&exp==GameSessionData.CurrentExperience,"Duplicate victory/reward0 "+id);
            UnityEngine.Object.FindObjectsByType<Button>().First(x=>x.name=="VictoryReturn").onClick.Invoke();yield return Back(scene);Continue("Victory "+id);
            File.AppendAllText(Path.Combine(Root,"retry-matrix.csv"),q.QuestId+","+id+","+(boss?"NOT_APPLICABLE_BOSS_FLEE":"PASS")+",PASS,PASS,PASS,"+(id==MainQuest07FieldFlow.EncounterId?"FAIL_FIXED":"PASS")+"\n");
        }
        static IEnumerator PartyContinue()
        {
            GameSessionData.SelectJob("sharpshooter");GameSessionData.ConfigureProgress(5,0);CompanionRosterService.UnlockIntroCompanions();CompanionRosterService.UnlockPaul("sharpshooter");CompanionRosterService.UnlockSerin();
            var rows=new Dictionary<string,FormationRow>{{"player",FormationRow.Rear},{CompanionRosterService.SerinId,FormationRow.Front},{CompanionRosterService.MielId,FormationRow.Rear}};
            Check(CompanionRosterService.TrySetComposition(new[]{CompanionRosterService.SerinId,CompanionRosterService.MielId},rows),"Manual Serin Miel formation");
            Check(BeastCompanionService.TryEquip("player","fox")&&BeastCompanionService.TryEquip(CompanionRosterService.SerinId,"bear"),"Nonempty Player Fox Serin Bear");
            PartyResourceService.RecordBattleResult(CompanionRosterService.SerinId,45,0,128,0);PartyResourceService.RecordBattleResult(CompanionRosterService.MielId,61,7,124,40);
            yield return Load("Field_02");Continue("Manual Beast Resources");string roster=JsonUtility.ToJson(CompanionRosterService.ExportSaveData()),beast=JsonUtility.ToJson(BeastCompanionService.ExportSaveData());
            yield return Load("Bootstrap");var loader=UnityEngine.Object.FindAnyObjectByType<BootstrapLoader>();Call(loader,"ContinueGame",1);yield return Back("Field_02");
            Check(GameSessionData.Level==5&&roster==JsonUtility.ToJson(CompanionRosterService.ExportSaveData())&&beast==JsonUtility.ToJson(BeastCompanionService.ExportSaveData()),"Actual Bootstrap Continue manual Party Beast Level");
            Check(CompanionCatalog.All.All(x=>x.EffectiveLevel==5),"All companions effective Lv5 after Bootstrap Continue");
            var setup=CompanionCatalog.Find(CompanionRosterService.SerinId).CreateParticipant(CompanionRosterService.GetSlot(CompanionRosterService.SerinId));var resource=PartyResourceService.ResolveForBattle(setup.Id,BeastCompanionService.GetEffectiveMaxHp(setup.MaxHp,"bear"),0);
            Check(resource.CurrentHp==45&&setup.Slot.Row==FormationRow.Front,"Saved HP retained with growth Bear formation");
        }
        static IEnumerator FleeVictory07()
        {
            // 통합 경로의 도망→패배→승리와 별도로, 도망 직후 재도전 승리 경로도 실제 명령으로 확인합니다.
            GameSessionData.SelectJob("mage");GameSessionData.ConfigureProgress(3,0);PartyResourceService.Reset();QuestDefinition q;QuestCatalog.TryGet(MainQuest07FieldFlow.QuestId,out q);SetQuest(q,4);
            yield return Load("Field_02");yield return Near07(MainQuest07FieldFlow.PaulId);
            Check(((GameObject)Field(Paul,"marker")).GetComponentInChildren<Text>(true).text.Contains("폴 주변 몬스터 재도전"),"Main07 retry marker label");
            Pulse(Key.E);yield return BattleReady();yield return WaitForCommands();Call(Controller,"Flee");yield return Back("Field_02");
            Check(QuestService.ActiveMainQuest.CurrentObjectiveIndex==4,"RouteA Flee count0");Continue("RouteA Flee");yield return Load("Field_02");yield return Near07(MainQuest07FieldFlow.PaulId);Pulse(Key.E);yield return BattleReady();
            yield return NaturalWin07();yield return Back("Field_02");Check(QuestService.ActiveMainQuest.CurrentObjectiveIndex==5,"RouteA Flee Retry natural Victory");
            Check(!(bool)Call(Paul,"Current"),"Main07 victory retry site disabled");Continue("RouteA Victory");
        }
        static void DamageFixture()
        {
            var skill=BattleSkillCatalog.GetSkills(Resources.LoadAll<JobDefinition>("JobDefinitions").First(x=>x.JobId=="mage")).First(x=>x.Id==BattleSkillCatalog.MageFireballId);
            var rows=new List<string>{"condition,level,max_hp,attack,agility,basic_damage,fireball_damage"};
            foreach(int attack in new[]{14,44,42})
            {
                var actor=new Combatant("paul","폴",BattleSide.Allies,new FormationSlot(FormationRow.Rear,0),104,attack,12,0,TargetRangeType.Magic,true);
                var target=new Combatant("target","피해 fixture",BattleSide.Enemies,new FormationSlot(FormationRow.Front,0),5000,1,1,0,TargetRangeType.MeleePhysical,false);
                int basic=target.TakeDamage(actor.Attack);var executor=new BattleSkillExecutor(new BattleSkillCooldowns(),new BattleStatusEffectRuntime(),new BattleFighterResourceRuntime());int damage;string message;
                Check(executor.ExecuteSingleMagicAttackWithBurn(actor,target,skill,out damage,out message),"Actual Fireball executor attack"+attack);
                rows.Add((attack==14?"PaulBefore":attack==44?"PaulAfter":"PlayerMage")+",3,"+(attack==14?96:attack==44?104:108)+","+attack+","+(attack==14?10:12)+","+basic+","+damage);
            }
            File.WriteAllLines(Path.Combine(Root,"damage-comparison.csv"),rows);
        }
    }
}

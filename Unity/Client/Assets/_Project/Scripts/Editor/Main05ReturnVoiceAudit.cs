using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ProjectLimitless.Audio;
using ProjectLimitless.Core;
using ProjectLimitless.NPC;
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
    /// <summary>Main04→05→06을 격리 저장으로 연속 검사하며, 연결 정합과 발화 의미 판정을 구분합니다.</summary>
    [InitializeOnLoad]
    public static class Main05ReturnVoiceAudit
    {
        const BindingFlags Private=BindingFlags.Instance|BindingFlags.NonPublic;
        const BindingFlags Static=BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
        static bool armed, probe;
        static bool guardReaudit, beforeRepair;
        static int inputCallbacks;
        static float listenerVolume;
        static Keyboard keyboard;
        static Gamepad gamepad;
        static bool inputSettingsChanged;
        static InputSettings.BackgroundBehavior originalBackground;
        static InputSettings.EditorInputBehaviorInPlayMode originalEditorInput;
        static List<string> checks=new List<string>();
        static List<string> trace=new List<string>();
        static string Root=>Path.GetFullPath(Path.Combine(Application.dataPath,guardReaudit ? "../../../Temp/Main05GuardVoice/"+(beforeRepair?"before-runtime":"after-runtime") : "../../../Temp/Main05Final5/runtime"));
        public static string Status="Idle";
        static Main05ReturnVoiceAudit()
        {
            EditorApplication.playModeStateChanged+=state=>
            {
                if(!armed)return;
                if(state==PlayModeStateChange.EnteredPlayMode)
                    EditorApplication.delayCall+=()=>typeof(Partial9FixedSpriteAudit).GetField("routine",Static).SetValue(null,Flatten(Run()));
                if(state==PlayModeStateChange.EnteredEditMode)
                {
                    AudioListener.volume=listenerVolume;
                    if(keyboard!=null)InputSystem.RemoveDevice(keyboard);
                    if(gamepad!=null)InputSystem.RemoveDevice(gamepad);
                    keyboard=null;gamepad=null;armed=false;
                    RestoreInputSettings();
                }
            };
        }
        // 기존 Launch가 사용자 저장/설정과 GameView 진입 동작을 보존·복구합니다.
        public static string Launch(bool reproductionOnly=false)
        {
            guardReaudit=false;
            Directory.CreateDirectory(Root);probe=reproductionOnly;checks.Clear();trace.Clear();
            listenerVolume=AudioListener.volume;Status="Running";armed=true;return Partial9FixedSpriteAudit.Launch();
        }
        /// <summary>과거 QA를 덮어쓰지 않고 태온001의 무입력 전체 재생을 별도 로그로 재검사합니다.</summary>
        public static string LaunchGuardReaudit(bool before)
        {
            guardReaudit=true;beforeRepair=before;probe=false;inputCallbacks=0;
            Directory.CreateDirectory(Root);checks.Clear();trace.Clear();
            File.WriteAllText(Path.Combine(Root,"playback.jsonl"),string.Empty);
            listenerVolume=AudioListener.volume;Status="Running";armed=true;
            return Partial9FixedSpriteAudit.Launch();
        }
        static object Field(object owner,string name)=>owner.GetType().GetField(name,Private).GetValue(owner);
        static VoicePlaybackSource Voice(DialoguePresenter d)=>(VoicePlaybackSource)Field(d,"voicePlayback");
        static int Page(DialoguePresenter d)=>(int)Field(d,"sequencePageIndex");
        static string Objective()=>QuestService.ActiveMainQuest?.CurrentObjective?.ObjectiveId;
        static void Check(bool value,string label)
        {
            checks.Add((value?"PASS ":"FAIL ")+label);
            File.WriteAllText(Path.Combine(Root,probe?"probe.txt":"runtime.txt"),Status+"\n"+string.Join("\n",checks));
            if(!value){Status="FAIL";throw new InvalidOperationException(label);}
        }
        static IEnumerator Flatten(IEnumerator routine)
        {
            var stack=new Stack<IEnumerator>();stack.Push(routine);
            while(stack.Count>0){var r=stack.Peek();if(!r.MoveNext()){stack.Pop();continue;}if(r.Current is IEnumerator)stack.Push((IEnumerator)r.Current);else yield return null;}
        }
        static IEnumerator Wait(int count){for(int i=0;i<count;i++)yield return null;}
        static IEnumerator Load(string name)
        {
            // 직접 로드하는 QA도 실제 World 전환처럼 저장 위치를 먼저 기록해야 자동 저장에 빈 Scene이 들어가지 않습니다.
            if(guardReaudit&&name!="Bootstrap")
            {
                GameSessionData.RecordLocation(name,string.Empty);
                GameSessionData.ClearWorldPosition();
            }
            SceneManager.LoadSceneAsync(name);for(int i=0;i<300&&SceneManager.GetActiveScene().name!=name;i++)yield return null;
            Check(SceneManager.GetActiveScene().name==name,"Scene "+name);yield return Wait(12);
        }
        static DialogueLine[] Lines(Type type,string method)=>(DialogueLine[])type.GetMethod(method,Static).Invoke(null,null);
        // 실제 Bootstrap Continue 경로로 격리 저장을 복원하고 목표/동료/Voice 잔류를 확인합니다.
        static IEnumerator SaveContinue(string scene,string objective,bool unlocked)
        {
            // QA의 직접 Scene 로드는 실제 출구 전환을 우회하므로 저장 API에 현 Scene을 명시합니다.
            Check(GameSaveService.SaveCurrentSession(scene),"Save checkpoint "+objective);
            yield return Load("Bootstrap");
            typeof(BootstrapLoader).GetMethod("ContinueGame",Private).Invoke(UnityEngine.Object.FindAnyObjectByType<BootstrapLoader>(),new object[]{1});
            for(int i=0;i<300&&SceneManager.GetActiveScene().name!=scene;i++)yield return null;
            yield return Wait(12);
            Check(SceneManager.GetActiveScene().name==scene&&Objective()==objective,"Actual Continue objective "+objective);
            Check(CompanionRosterService.IsUnlocked(CompanionRosterService.TaeonId)==unlocked&&CompanionRosterService.IsUnlocked(CompanionRosterService.MielId)==unlocked,"Continue companion state "+objective);
            Check(DialoguePresenter.Instance==null||(!DialoguePresenter.Instance.IsOpen&&Voice(DialoguePresenter.Instance).Clip==null),"Continue no queued Voice "+objective);
        }
        static NpcController Npc(string id)=>UnityEngine.Object.FindObjectsByType<VillageNpcRole>().First(n=>n.NpcId==id).GetComponent<NpcController>();
        static void Interact(NpcController npc)
        {
            var player=UnityEngine.Object.FindAnyObjectByType<PlayerController>();
            player.transform.position=npc.transform.position+new Vector3(-.6f,0,0);
            npc.GetComponent<VillageNpcRole>().Interact(npc);
        }
        // 가상 장치 이벤트만 Input System에 넣습니다. OS 입력이나 실제 장치는 조작하지 않습니다.
        static void KeyPulse(Key key)
        {
            InputSystem.QueueStateEvent(keyboard,new KeyboardState(key));InputSystem.Update();
            InputSystem.QueueStateEvent(keyboard,new KeyboardState());InputSystem.Update();
        }
        static void APulse()
        {
            InputSystem.QueueStateEvent(gamepad,new GamepadState().WithButton(GamepadButton.South));InputSystem.Update();
            InputSystem.QueueStateEvent(gamepad,new GamepadState());InputSystem.Update();
        }
        static void RestoreInputSettings()
        {
            if(!inputSettingsChanged)return;
            InputSystem.settings.backgroundBehavior=originalBackground;
            InputSystem.settings.editorInputBehaviorInPlayMode=originalEditorInput;
            inputSettingsChanged=false;
        }
        static void ConfigureVirtualInput(DialoguePresenter d)
        {
            if(!inputSettingsChanged)
            {
                originalBackground=InputSystem.settings.backgroundBehavior;
                originalEditorInput=InputSystem.settings.editorInputBehaviorInPlayMode;
                inputSettingsChanged=true;
            }
            InputSystem.settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;
            InputSystem.settings.editorInputBehaviorInPlayMode=InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
            if(keyboard==null)keyboard=InputSystem.AddDevice<Keyboard>();
            if(gamepad==null)gamepad=InputSystem.AddDevice<Gamepad>();
            ((InputAction)Field(d,"advanceAction")).performed+=_=>inputCallbacks++;
            ((InputAction)Field(UnityEngine.Object.FindAnyObjectByType<InteractionSystem>(),"interactAction")).performed+=_=>inputCallbacks++;
        }
        static IEnumerator InputProbe(DialoguePresenter d)
        {
            // Input System은 Play 진입 시 기본 설정 객체를 교체할 수 있으므로 현재 런타임 설정값만
            // 잠시 변경하고 즉시 복구합니다. Asset 저장이나 기본 설정 객체 교체·제거는 하지 않습니다.
            originalBackground=InputSystem.settings.backgroundBehavior;
            originalEditorInput=InputSystem.settings.editorInputBehaviorInPlayMode;
            inputSettingsChanged=true;
            InputSystem.settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;
            InputSystem.settings.editorInputBehaviorInPlayMode=InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
            keyboard=InputSystem.AddDevice<Keyboard>();gamepad=InputSystem.AddDevice<Gamepad>();
            var lines=Lines(typeof(MainQuest05ReturnFlow),"GuardReport");
            d.ShowSequence(lines,null);yield return Wait(3);
            KeyPulse(Key.Enter);KeyPulse(Key.Space);
            checks.Add("OBSERVED sameFrame Enter+Space page="+Page(d)+" expectedInputPage=1 frame="+Time.frameCount);
            if(!probe)Check(Page(d)==1,"Same-frame Enter+Space advances one page");
            yield return Wait(3);KeyPulse(Key.Enter);Check(Page(d)==(probe?3:2),"Next-frame Enter still available");
            yield return Wait(3);KeyPulse(Key.Space);Check(Page(d)==(probe?4:3),"Space alone advances one page");d.Hide();yield return Wait(3);
            var guard=Npc(MainQuest01NpcFlow.GuardId);var player=UnityEngine.Object.FindAnyObjectByType<PlayerController>();
            player.transform.position=guard.transform.position+new Vector3(-.6f,0,0);yield return Wait(3);
            d.ShowSequence(new[]{new DialogueLine("player","플레이어","마지막 페이지")},null);yield return Wait(3);
            APulse();APulse();checks.Add("OBSERVED sameFrame A close+reopen dialogueOpen="+d.IsOpen+" page="+Page(d));
            if(!probe)Check(!d.IsOpen,"Same-frame A after closing cannot reopen NPC");
            d.Hide();yield return Wait(3);APulse();Check(d.IsOpen&&Page(d)==0,"New-frame A opens Guard page0");d.Hide();
            RestoreInputSettings();
        }
        static IEnumerator Sequence(DialoguePresenter d,DialogueLine[] lines,string sequence)
        {
            if(guardReaudit&&sequence!="AfterBattleConversation")ConfigureVirtualInput(d);
            string before=Objective();
            for(int i=0;i<lines.Length;i++)
            {
                var line=lines[i];var v=Voice(d);var clip=v.Clip;
                Check(d.IsOpen&&Page(d)==i,"Exact page "+sequence+":"+i);
                Check(UnityEngine.Object.FindObjectsByType<VoicePlaybackSource>().Length==1,"Single Voice Source "+sequence+":"+i);
                Check(((Text)Field(d,"dialogueText")).text==line.Message&&((Text)Field(d,"speakerText")).text==line.SpeakerName,"Text/Speaker "+sequence+":"+i);
                Check(Objective()==before,"Objective unchanged before completion "+sequence+":"+i);
                var portrait=DialoguePortraitCatalog.GetPortrait(line.SpeakerId);
                Check(((GameObject)Field(d,"portraitRoot")).activeSelf==(portrait!=null)&&((Image)Field(d,"portraitImage")).sprite==portrait,"Portrait policy "+sequence+":"+i);
                if(string.IsNullOrEmpty(line.DialogueId)||line.IsPlayer||line.IsDirection)
                    Check(clip==null&&!v.IsPlaying,"Intentional Silent "+sequence+":"+i);
                else
                {
                    var resolved=Resources.Load<VoiceClipCatalog>("Audio/Voice/Story/StoryVoiceCatalog").Find(line.DialogueId,line.SpeakerId);
                    Check(clip==resolved&&clip!=null&&clip.name==line.DialogueId&&v.IsPlaying,"Runtime Resolve/Play "+line.DialogueId);
                    string path=AssetDatabase.GetAssetPath(clip);
                    if(sequence=="GuardReport"||sequence=="RepresentativeReport")Check(path.Contains("/Main05/"),"No Main04 clip in Main05 "+line.DialogueId);
                    var pcm=new float[clip.samples*clip.channels];Check(clip.GetData(pcm,0),"Runtime PCM "+line.DialogueId);
                    using(var w=new BinaryWriter(File.Open(Path.Combine(Root,line.DialogueId+".pcm-f32"),FileMode.Create)))foreach(float sample in pcm)w.Write(sample);
                    double start=EditorApplication.timeSinceStartup;double stop=-1;
                    int callbacksBefore=inputCallbacks;
                    int samplesBefore=-1;bool sawPlaying=false;
                    while(EditorApplication.timeSinceStartup-start<clip.length+(guardReaudit?5:.5))
                    {
                        CheckContinuous(d,i,clip,line.DialogueId);
                        if(guardReaudit)
                        {
                            var source=(AudioSource)Field(v,"source");
                            sawPlaying|=source.isPlaying;
                            if(source.isPlaying&&samplesBefore>source.timeSamples)throw new InvalidOperationException("Unexpected loop "+line.DialogueId);
                            if(source.isPlaying)samplesBefore=source.timeSamples;
                            var sample=new PlaybackTrace{frame=Time.frameCount,page=Page(d),speakerId=line.SpeakerId,speakerName=((Text)Field(d,"speakerText")).text,text=((Text)Field(d,"dialogueText")).text,id=line.DialogueId,clip=source.clip==null?"":source.clip.name,timeSamples=source.timeSamples,length=clip.length,isPlaying=source.isPlaying,objective=Objective(),inputCallbacks=inputCallbacks};
                            File.AppendAllText(Path.Combine(Root,"playback.jsonl"),JsonUtility.ToJson(sample)+"\n");
                        }
                        if(!v.IsPlaying&&stop<0)stop=EditorApplication.timeSinceStartup;
                        yield return null;
                    }
                    Check(!v.IsPlaying&&(guardReaudit?sawPlaying:stop>=0&&stop-start>=clip.length-.2),"Full playback natural stop "+line.DialogueId);
                    if(guardReaudit)
                    {
                        Check(inputCallbacks==callbacksBefore,"No input callback during hold "+line.DialogueId);
                        Check(((Text)Field(d,"dialogueText")).text==line.Message&&((Text)Field(d,"speakerText")).text==line.SpeakerName&&Objective()==before,"No-input text/speaker/objective maintained "+line.DialogueId);
                        Check(v.Clip==clip,"No unrequested clip replacement "+line.DialogueId);
                    }
                }
                var pageTrace=new PageTrace{quest=QuestService.ActiveMainQuest?.Definition.QuestId,sequence=sequence,page=i,id=line.DialogueId,speaker=line.SpeakerId,text=line.Message,clip=clip==null?"":AssetDatabase.GetAssetPath(clip),objectiveBefore=before};
                yield return Wait(3);
                if(guardReaudit&&sequence!="AfterBattleConversation")
                {
                    int callbacksBefore=inputCallbacks;
                    KeyPulse(Key.Enter);
                    Check(inputCallbacks==callbacksBefore+1,"One explicit Enter callback "+sequence+":"+i);
                    if(i+1<lines.Length)Check(Page(d)==i+1,"One input advances one page "+sequence+":"+i);
                }
                else d.Advance();
                pageTrace.objectiveAfter=Objective();trace.Add(JsonUtility.ToJson(pageTrace));
                File.WriteAllLines(Path.Combine(Root,"pages.jsonl"),trace);
                Check(clip==null||Voice(d).Clip!=clip,"Previous Clip cleanup "+sequence+":"+i);
            }
            Check(!d.IsOpen&&Voice(d).Clip==null,"Sequence closed "+sequence);
        }
        static void CheckContinuous(DialoguePresenter d,int page,AudioClip clip,string id)
        {
            if(!d.IsOpen||Page(d)!=page||Voice(d).Clip!=clip)throw new InvalidOperationException("Unrequested skip/cache "+id);
        }
        [Serializable] sealed class PageTrace{public string quest,sequence,id,speaker,text,clip,objectiveBefore,objectiveAfter;public int page;}
        [Serializable] sealed class PlaybackTrace{public int frame,page,timeSamples,inputCallbacks;public string speakerId,speakerName,text,id,clip,objective;public float length;public bool isPlaying;}
        static IEnumerator Run()
        {
            yield return Wait(20);Check(!string.IsNullOrWhiteSpace(GameSaveService.AuditSaveDirectory),"Isolated Save/Settings");
            GameSaveService.SelectSlot(1);GameSessionData.ConfigurePlayer(PlayerVisualType.Male,"귀환 QA");GameSessionData.SelectPlayerPath("path.vision");GameSessionData.SelectJob("mage");
            UserSettingsService.SetMuteAll(false);UserSettingsService.SetAudioVolumes(70,60,20);AudioListener.volume=0f;
            CompanionRosterService.ImportSaveData(new CompanionRosterSaveData());
            QuestDefinition definition;QuestCatalog.TryGet(MainQuest04FieldFlow.QuestId,out definition);
            QuestService.ImportSaveData(new QuestProgressSaveData{CompletedQuestIds=definition.PrerequisiteQuestIds.ToArray(),ActiveQuests=new[]{new ActiveQuestSaveData{QuestId=definition.QuestId,Objectives=new[]{
                new QuestObjectiveProgressData{ObjectiveId="reach_miel_meeting",CurrentCount=1},new QuestObjectiveProgressData{ObjectiveId="talk_to_miel_first",CurrentCount=1},new QuestObjectiveProgressData{ObjectiveId="win_three_people_encounter",CurrentCount=1}}}}});
            if(guardReaudit&&beforeRepair)
            {
                QuestService.ImportSaveData(new QuestProgressSaveData{CompletedQuestIds=definition.PrerequisiteQuestIds.Concat(new[]{MainQuest04FieldFlow.QuestId}).ToArray()});
                yield return Load("World_StarterVillage");var d=DialoguePresenter.Instance;
                Check(Objective()=="report_to_south_gate_guard","Before repair Main05 Guard objective");
                Interact(Npc(MainQuest01NpcFlow.GuardId));
                yield return Sequence(d,Lines(typeof(MainQuest05ReturnFlow),"GuardReport"),"GuardReport");
                Check(Objective()=="report_to_village_representative"&&!d.IsOpen,"Before repair independent representative boundary");
            }
            else if(probe)
            {
                QuestService.ImportSaveData(new QuestProgressSaveData{CompletedQuestIds=definition.PrerequisiteQuestIds.Concat(new[]{MainQuest04FieldFlow.QuestId}).ToArray()});
                yield return Load("World_StarterVillage");yield return InputProbe(DialoguePresenter.Instance);
            }
            else
            {
                yield return Load("Field_01");var d=DialoguePresenter.Instance;
                var actor=UnityEngine.Object.FindAnyObjectByType<MainQuest04MielActor>();var player=UnityEngine.Object.FindAnyObjectByType<PlayerController>();player.transform.position=actor.transform.position+new Vector3(-.6f,0,0);
                Check(actor.TryInteract(player.transform,3),"Actual Main04 Late TryInteract");yield return Sequence(d,Lines(typeof(MainQuest04MielActor),"AfterBattleConversation"),"AfterBattleConversation");
                Check(QuestService.GetState(MainQuest04FieldFlow.QuestId)==QuestState.Completed,"Main04 completed");
                yield return Load("World_StarterVillage");d=DialoguePresenter.Instance;
                Check(Objective()=="report_to_south_gate_guard","Main05 village return objective");
                yield return InputProbe(d);yield return Wait(3);
                Interact(Npc(MainQuest01NpcFlow.GuardId));yield return Sequence(d,Lines(typeof(MainQuest05ReturnFlow),"GuardReport"),"GuardReport");
                Check(Objective()=="report_to_village_representative"&&!d.IsOpen,"Guard ends before independent representative interaction");yield return Wait(5);Check(!d.IsOpen,"No automatic next NPC dialogue");
                Check(!CompanionRosterService.IsUnlocked(CompanionRosterService.MielId),"No premature companion unlock");
                yield return SaveContinue("World_StarterVillage","report_to_village_representative",false);d=DialoguePresenter.Instance;
                Interact(Npc(MainQuest01NpcFlow.RepresentativeId));yield return Sequence(d,Lines(typeof(MainQuest05ReturnFlow),"RepresentativeReport"),"RepresentativeReport");
                Check(QuestService.GetState(MainQuest05ReturnFlow.QuestId)==QuestState.Completed,"Main05 complete");
                Check(CompanionRosterService.IsUnlocked(CompanionRosterService.TaeonId)&&CompanionRosterService.IsUnlocked(CompanionRosterService.MielId),"Official companion unlock");
                Check(QuestService.GetState(MainQuest06ForestFlow.QuestId)==QuestState.Available,"Main06 available in village");
                yield return SaveContinue("World_StarterVillage",null,true);
                Check(QuestService.GetState(MainQuest05ReturnFlow.QuestId)==QuestState.Completed&&QuestService.GetState(MainQuest06ForestFlow.QuestId)==QuestState.Available,"Continue completed Main05 and available Main06");
                yield return Load("Field_01");Check(QuestService.ActiveMainQuest.Definition.QuestId==MainQuest06ForestFlow.QuestId,"Main06 starts on Field01");
                yield return Load("Field_02");Check(Objective()=="inspect_anomaly_trace","Main06 Field02 objective");
                yield return SaveContinue("Field_02","inspect_anomaly_trace",true);
                var anomaly=UnityEngine.Object.FindAnyObjectByType<MainQuest06AnomalyTrace>();player=UnityEngine.Object.FindAnyObjectByType<PlayerController>();player.transform.position=anomaly.transform.position;
                typeof(MainQuest06AnomalyTrace).GetField("nearbyPlayer",Private).SetValue(anomaly,player);
                typeof(MainQuest06AnomalyTrace).GetMethod("OnInteract",Private).Invoke(anomaly,new object[]{default(InputAction.CallbackContext)});yield return Wait(3);d=DialoguePresenter.Instance;
                Check(Page(d)==0&&Voice(d).Clip.name=="main06_taeon_001"&&((Text)Field(d,"dialogueText")).text=="초원에서 봤던 움직임과 비슷합니다.","Main06 first boundary clean");
                Check(Voice(d).IsPlaying&&AssetDatabase.GetAssetPath(Voice(d).Clip).Contains("/Main06/"),"Main06 no previous Voice cache");
                Check(UnityEngine.Object.FindObjectsByType<VoicePlaybackSource>().Length==1,"Main06 Single Source");
                trace.Add(JsonUtility.ToJson(new PageTrace{quest=MainQuest06ForestFlow.QuestId,sequence="Main06FirstBoundary",page=0,id=Voice(d).Clip.name,speaker=((DialogueLine[])Field(d,"sequenceLines"))[0].SpeakerId,text=((Text)Field(d,"dialogueText")).text,clip=AssetDatabase.GetAssetPath(Voice(d).Clip),objectiveBefore=Objective(),objectiveAfter=Objective()}));
                File.WriteAllLines(Path.Combine(Root,"pages.jsonl"),trace);
                Check(UnityEngine.Object.FindObjectsByType<Transform>().All(t=>GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(t.gameObject)==0),"Runtime Missing Script0");d.Hide();
                d.ShowSequence(new[]{new DialogueLine("companion_taeon","태온","<지문> 상황을 살핀다.","main05_taeon_supp_002")},null);
                Check(Voice(d).Clip==null&&!Voice(d).IsPlaying&&!((GameObject)Field(d,"portraitRoot")).activeSelf&&string.IsNullOrEmpty(((Text)Field(d,"speakerText")).text),"Direction Speaker0 Portrait0 Voice0");d.Hide();
            }
            yield return Load("Bootstrap");Status="PASS";
            File.WriteAllText(Path.Combine(Root,probe?"probe-final.txt":"runtime-final.txt"),Status+"\n"+string.Join("\n",checks));
        }
    }
}

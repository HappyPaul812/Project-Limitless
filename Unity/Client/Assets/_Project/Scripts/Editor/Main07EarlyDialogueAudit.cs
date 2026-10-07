using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ProjectLimitless.Audio;
using ProjectLimitless.Core;
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
    /// <summary>사용자 저장과 화면 포커스를 보호하며 Main06→07의 실제 입력·페이지·Clip을 기록합니다.</summary>
    [InitializeOnLoad]
    public static class Main07EarlyDialogueAudit
    {
        const BindingFlags Private=BindingFlags.Instance|BindingFlags.NonPublic;
        const BindingFlags Static=BindingFlags.Static|BindingFlags.NonPublic|BindingFlags.Public;
        static bool armed,before;
        static Keyboard keyboard;static Gamepad gamepad;
        static float originalVolume;
        static InputSettings.BackgroundBehavior background;
        static InputSettings.EditorInputBehaviorInPlayMode editorInput;
        static bool settingsChanged;
        static string sequence;
        static List<string> checks=new List<string>(),trace=new List<string>();
        static string Root=>Path.GetFullPath(Path.Combine(Application.dataPath,"../../../Temp/Main07Early/"+(before?"before":"final")));
        public static string Status="Idle";
        static Main07EarlyDialogueAudit()
        {
            EditorApplication.playModeStateChanged+=s=>
            {
                if(!armed)return;
                if(s==PlayModeStateChange.EnteredPlayMode)EditorApplication.delayCall+=()=>typeof(Partial9FixedSpriteAudit).GetField("routine",Static).SetValue(null,Flatten(Run()));
                if(s==PlayModeStateChange.EnteredEditMode)
                {
                    InputSystem.onActionChange-=ActionChanged;
                    if(keyboard!=null)InputSystem.RemoveDevice(keyboard);if(gamepad!=null)InputSystem.RemoveDevice(gamepad);
                    keyboard=null;gamepad=null;
                    if(settingsChanged){InputSystem.settings.backgroundBehavior=background;InputSystem.settings.editorInputBehaviorInPlayMode=editorInput;settingsChanged=false;}
                    AudioListener.volume=originalVolume;armed=false;
                }
            };
        }
        // 공용 격리 Launch는 사용자 Save/Settings와 기존 GameView 설정을 종료 시 복원합니다.
        public static string Launch(bool beforeFix=false)
        {
            before=beforeFix;checks.Clear();trace.Clear();sequence="Setup";Directory.CreateDirectory(Root);
            originalVolume=AudioListener.volume;armed=true;Status="Running";return Partial9FixedSpriteAudit.Launch();
        }
        static object Field(object o,string n)=>o.GetType().GetField(n,Private).GetValue(o);
        static int Page=>DialoguePresenter.Instance==null?-1:(int)Field(DialoguePresenter.Instance,"sequencePageIndex");
        static VoicePlaybackSource Voice=>DialoguePresenter.Instance==null?null:(VoicePlaybackSource)Field(DialoguePresenter.Instance,"voicePlayback");
        static string Objective=>QuestService.ActiveMainQuest?.CurrentObjective?.ObjectiveId;
        static void Check(bool value,string label)
        {
            checks.Add((value?"PASS ":"FAIL ")+label);File.WriteAllLines(Path.Combine(Root,"checks.txt"),checks);
            if(!value){Status="FAIL";throw new InvalidOperationException(label);}
        }
        // Input System 실제 Action 이벤트와 직전/직후 snapshot을 QA 파일에만 보관합니다.
        static void ActionChanged(object o,InputActionChange c)
        {
            if(c==InputActionChange.ActionPerformed&&o is InputAction)Snapshot("ActionPerformed",(InputAction)o);
        }
        static void Snapshot(string stage,InputAction action=null,string target=null)
        {
            var d=DialoguePresenter.Instance;var clip=Voice?.Clip;var lines=d==null?null:(DialogueLine[])Field(d,"sequenceLines");
            var line=lines!=null&&lines.Length>Page&&Page>=0?lines[Page]:null;
            trace.Add(JsonUtility.ToJson(new Row{frame=Time.frameCount,time=EditorApplication.timeSinceStartup,sequence=sequence,stage=stage,action=action?.name,control=action?.activeControl?.path,target=target,page=Page,open=d!=null&&d.IsOpen,modal=WorldModalState.IsOpen,canBegin=d!=null&&d.CanBeginInteractionThisFrame,id=line?.DialogueId,speaker=line?.SpeakerId,text=line?.Message,clip=clip?.name,path=clip==null?null:AssetDatabase.GetAssetPath(clip),length=clip==null?0:clip.length,objective=Objective}));
            File.WriteAllLines(Path.Combine(Root,"trace.jsonl"),trace);
        }
        [Serializable] sealed class Row{public int frame,page;public double time;public bool open,modal,canBegin;public float length;public string sequence,stage,action,control,target,id,speaker,text,clip,path,objective;}
        static IEnumerator Flatten(IEnumerator r)
        {
            var stack=new Stack<IEnumerator>();stack.Push(r);
            while(stack.Count>0){var a=stack.Peek();if(!a.MoveNext()){stack.Pop();continue;}if(a.Current is IEnumerator)stack.Push((IEnumerator)a.Current);else yield return null;}
        }
        static IEnumerator Wait(int n){for(int i=0;i<n;i++)yield return null;}
        static IEnumerator Load(string name)
        {
            SceneManager.LoadSceneAsync(name);for(int i=0;i<300&&SceneManager.GetActiveScene().name!=name;i++)yield return null;
            yield return Wait(12);Check(SceneManager.GetActiveScene().name==name,"Scene "+name);
        }
        static void KeyState(params Key[] keys){InputSystem.QueueStateEvent(keyboard,new KeyboardState(keys));InputSystem.Update();}
        static void Pulse(Key key){Snapshot("InputBefore");KeyState(key);KeyState();Snapshot("InputAfter");}
        static void APulse(){Snapshot("InputBefore A");InputSystem.QueueStateEvent(gamepad,new GamepadState().WithButton(GamepadButton.South));InputSystem.Update();InputSystem.QueueStateEvent(gamepad,new GamepadState());InputSystem.Update();Snapshot("InputAfter A");}
        static MainQuest07Interactable Site(string id)=>UnityEngine.Object.FindObjectsByType<MainQuest07Interactable>().First(x=>(string)Field(x,"targetId")==id);
        static IEnumerator Near(string id)
        {
            var site=Site(id);var p=UnityEngine.Object.FindAnyObjectByType<PlayerController>();p.transform.position=site.transform.position;
            yield return Wait(5);typeof(MainQuest07Interactable).GetField("nearby",Private).SetValue(site,p);Snapshot("InteractionReady",target:id);
        }
        static void Dump(AudioClip clip)
        {
            var pcm=new float[clip.samples*clip.channels];Check(clip.GetData(pcm,0),"PCM readable "+clip.name);
            using(var w=new BinaryWriter(File.Open(Path.Combine(Root,clip.name+".pcm-f32"),FileMode.Create)))foreach(float v in pcm)w.Write(v);
        }
        static IEnumerator FinishSequence(int count,bool pending)
        {
            var d=DialoguePresenter.Instance;string objective=Objective;
            for(int i=0;i<count;i++)
            {
                Snapshot("PageDisplay");Check(d.IsOpen&&Page==i,"Page order "+sequence+":"+i);Check(Objective==objective,"Objective before completion "+sequence+":"+i);
                var line=((DialogueLine[])Field(d,"sequenceLines"))[i];
                if(pending&&!line.IsPlayer){Check(before?string.IsNullOrEmpty(line.DialogueId):!string.IsNullOrEmpty(line.DialogueId),"Stable ID state "+sequence+":"+i);Check(Voice.Clip==null,"TTS pending no fallback "+sequence+":"+i);}
                if(line.IsPlayer)Check(Voice.Clip==null&&!((GameObject)Field(d,"portraitRoot")).activeSelf,"Player Voice0 Portrait0");
                yield return Wait(3);Pulse(Key.E);Check(Page==i+1||!d.IsOpen,"Explicit E one page "+sequence+":"+i);
            }
            Check(!d.IsOpen&&Voice.Clip==null,"Sequence closed "+sequence);yield return Wait(3);
        }
        static IEnumerator PaulProbe()
        {
            var d=DialoguePresenter.Instance;var paul=Site(MainQuest07FieldFlow.PaulId);
            sequence="PaulFirst";Pulse(Key.E);Check(d.IsOpen&&Page==0,"Paul E interaction preserves page0");Snapshot("PageDisplay",target:MainQuest07FieldFlow.PaulId);
            var first=Voice.Clip;Check(first!=null&&first.name=="main07_paul_001","Paul001 runtime resolve");Dump(first);
            double start=EditorApplication.timeSinceStartup;int lastSamples=0;bool ended=false;
            while(EditorApplication.timeSinceStartup-start<Math.Max(10,first.length+5))
            {
                CheckIdle(first,0);var source=(AudioSource)Field(Voice,"source");
                if(source.isPlaying){if(ended||source.timeSamples<lastSamples)throw new InvalidOperationException("Paul001 unexpected replay");lastSamples=source.timeSamples;}else ended=true;
                yield return null;
            }
            Snapshot("NoInputWaitEnd");Check(Page==0&&Voice.Clip==first&&!Voice.IsPlaying,"No-input duration+5 page0 and no002");
            yield return Wait(3);Pulse(Key.E);Check(Page==1&&Voice.Clip.name=="main07_paul_002"&&Voice.IsPlaying,"Explicit E once Paul002 page1");Dump(Voice.Clip);Snapshot("PageDisplay");
            yield return Wait(3);Pulse(Key.F);Check(Page==2,"F advances exactly one page");
            yield return Wait(3);Pulse(Key.Enter);Check(Page==3,"Enter advances exactly one page");
            yield return Wait(3);Pulse(Key.Space);Check(Page==4,"Space advances exactly one page");
            yield return Wait(3);APulse();Check(Page==5,"A advances exactly one page");
            yield return Wait(3);Pulse(Key.Enter);Pulse(Key.Space);APulse();Pulse(Key.E);Check(Page==6,"Same frame E Enter Space A one page");d.Hide();yield return Wait(3);
            // 실제 custom action의 닫힘 프레임 누락을 Battle 시작 없는 마지막-page fixture로 검사합니다.
            sequence="CloseFrameProbe";d.ShowSequence(new[]{new DialogueLine("player","플레이어","마지막 페이지")},null);yield return Wait(3);Pulse(Key.E);Pulse(Key.E);
            Snapshot("CloseFrameResult");checks.Add("OBSERVED close sameframe dialogueOpen="+d.IsOpen+" page="+Page);
            if(!before)Check(!d.IsOpen,"Custom E no same-frame reopen");d.Hide();yield return Wait(3);
            d.ShowSequence(new[]{new DialogueLine("player","플레이어","마지막 페이지")},null);yield return Wait(3);APulse();APulse();
            Snapshot("CloseFrameAResult");checks.Add("OBSERVED close A sameframe dialogueOpen="+d.IsOpen+" page="+Page);
            if(!before)Check(!d.IsOpen,"Custom A no same-frame reopen");d.Hide();yield return Wait(3);
            sequence="HeldE";KeyState(Key.E);Check(d.IsOpen&&Page==0,"Held E initial open page0");
            double held=EditorApplication.timeSinceStartup;while(EditorApplication.timeSinceStartup-held<2){CheckIdle(Voice.Clip,0);KeyState(Key.E);yield return null;}
            KeyState();Check(Page==0,"Held E no repeated Next");Snapshot("HeldRelease");d.Hide();yield return Wait(3);
            sequence="OpenF";Pulse(Key.F);Check(d.IsOpen&&Page==0,"F opening no Next");d.Hide();yield return Wait(3);
            sequence="OpenA";APulse();Check(d.IsOpen&&Page==0,"A opening no Next");d.Hide();
            Check(UnityEngine.Object.FindObjectsByType<VoicePlaybackSource>().Length==1,"Single VoiceSource no overlap");
            Check(UnityEngine.Object.FindObjectsByType<Transform>().All(t=>GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(t.gameObject)==0),"Runtime Missing Script0");
        }
        static void CheckIdle(AudioClip clip,int page)
        {
            var d=DialoguePresenter.Instance;if(!d.IsOpen||Page!=page||Voice.Clip!=clip)throw new InvalidOperationException("Unrequested page/clip change");
        }
        static IEnumerator Run()
        {
            yield return Wait(20);Check(!string.IsNullOrEmpty(GameSaveService.AuditSaveDirectory),"Isolated Save");
            GameSaveService.SelectSlot(1);GameSessionData.ConfigurePlayer(PlayerVisualType.Male,"폴 QA");GameSessionData.SelectPlayerPath("path.vision");GameSessionData.SelectJob("mage");
            // QA의 직접 Scene 로드는 출구 전환을 우회하므로 실제 자동 저장 전에 월드 위치를 준비합니다.
            GameSessionData.RecordLocation("Field_02",string.Empty);
            UserSettingsService.SetMuteAll(false);UserSettingsService.SetAudioVolumes(70,60,20);AudioListener.volume=0;
            QuestDefinition q;QuestCatalog.TryGet(MainQuest06ForestFlow.QuestId,out q);
            QuestService.ImportSaveData(new QuestProgressSaveData{CompletedQuestIds=q.PrerequisiteQuestIds.ToArray()});
            yield return Load("Field_02");
            background=InputSystem.settings.backgroundBehavior;editorInput=InputSystem.settings.editorInputBehaviorInPlayMode;settingsChanged=true;
            InputSystem.settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;InputSystem.settings.editorInputBehaviorInPlayMode=InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
            keyboard=InputSystem.AddDevice<Keyboard>();gamepad=InputSystem.AddDevice<Gamepad>();InputSystem.onActionChange+=ActionChanged;
            var p=UnityEngine.Object.FindAnyObjectByType<PlayerController>();var anomaly=UnityEngine.Object.FindAnyObjectByType<MainQuest06AnomalyTrace>();p.transform.position=anomaly.transform.position;yield return Wait(6);
            typeof(MainQuest06AnomalyTrace).GetField("nearbyPlayer",Private).SetValue(anomaly,p);
            sequence="Main06Investigation";Pulse(Key.E);Check(DialoguePresenter.Instance.IsOpen&&Page==0,"Main06 real input open");yield return FinishSequence(4,false);
            Check(QuestService.GetState(MainQuest06ForestFlow.QuestId)==QuestState.Completed,"Actual Main06 complete");
            // 기존 Main07 설치는 SceneLoaded에서 시작합니다. 재진입 경계를 사용하며 Quest 로직은 바꾸지 않습니다.
            yield return Load("Field_02");Check(QuestService.ActiveMainQuest?.Definition.QuestId==MainQuest07FieldFlow.QuestId,"Main07 actual Scene installation start");
            sequence="WheelTracks";yield return Near(MainQuest07FieldFlow.WheelTracksId);Pulse(Key.E);yield return FinishSequence(4,true);
            Check(Objective=="help_wounded_traveler","WheelTracks completion boundary");
            sequence="WoundedTraveler";yield return Near(MainQuest07FieldFlow.WoundedTravelerId);Pulse(Key.F);yield return FinishSequence(4,true);
            Check(Objective=="follow_wheel_tracks","Wounded completion boundary");
            p=UnityEngine.Object.FindAnyObjectByType<PlayerController>();p.transform.position=MainQuest07FieldFlow.PaulPosition;yield return Wait(8);
            Check(Objective=="talk_to_paul","Actual PaulTrail location trigger");yield return Near(MainQuest07FieldFlow.PaulId);yield return PaulProbe();
            InputSystem.onActionChange-=ActionChanged;yield return Load("Bootstrap");Status="PASS";File.WriteAllText(Path.Combine(Root,"final.txt"),Status+"\n"+string.Join("\n",checks));
        }
    }
}

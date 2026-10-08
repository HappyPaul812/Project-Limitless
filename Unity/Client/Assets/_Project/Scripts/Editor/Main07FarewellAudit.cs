using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ProjectLimitless.Audio;
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
    /// <summary>폴 작별의 실제 무음 fallback과 Main08 기존 음성 경계를 격리 저장에서 확인합니다.</summary>
    [InitializeOnLoad]
    public static class Main07FarewellAudit
    {
        const BindingFlags Flags=BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance|BindingFlags.Static;
        const string Armed="Limitless.Main07Farewell.Audit";
        static readonly List<string> results=new List<string>();
        static Keyboard keyboard; static Gamepad gamepad; static float volume;
        static InputSettings.BackgroundBehavior background;
        static InputSettings.EditorInputBehaviorInPlayMode editorInput;
        static bool inputChanged;
        static bool approvedVoice;
        public static string Status="IDLE";
        static string Output=>Path.GetFullPath(Path.Combine(Application.dataPath,approvedVoice?"../../../문서/00_프로젝트/Main07_Paul_Farewell_Final_Runtime_Results.txt":"../../../문서/00_프로젝트/Main07_Paul_Farewell_Runtime_Results.txt"));
        static Main07FarewellAudit()
        {
            var initialized=Partial9FixedSpriteAudit.Status;
            EditorApplication.playModeStateChanged+=state=>
            {
                if(!SessionState.GetBool(Armed,false))return;
                if(state==PlayModeStateChange.EnteredPlayMode)
                    typeof(Partial9FixedSpriteAudit).GetField("routine",Flags).SetValue(null,Flatten(Run()));
                if(state==PlayModeStateChange.EnteredEditMode)
                {
                    if(keyboard!=null)InputSystem.RemoveDevice(keyboard);
                    if(gamepad!=null)InputSystem.RemoveDevice(gamepad);
                    keyboard=null;gamepad=null;
                    if(inputChanged){InputSystem.settings.backgroundBehavior=background;InputSystem.settings.editorInputBehaviorInPlayMode=editorInput;inputChanged=false;}
                    AudioListener.volume=volume;SessionState.SetBool(Armed,false);Write();
                }
            };
        }
        public static string Launch(bool verifyApprovedVoice=false)
        {approvedVoice=verifyApprovedVoice;results.Clear();Status="RUNNING";volume=AudioListener.volume;SessionState.SetBool(Armed,true);Write();return Partial9FixedSpriteAudit.Launch();}
        static void Write()=>File.WriteAllLines(Output,new[]{"STATUS|"+Status}.Concat(results));
        static void Check(bool ok,string label)
        {results.Add((ok?"PASS|":"FAIL|")+label);Write();if(!ok)throw new InvalidOperationException(label);}
        static T Field<T>(object owner,string name)=>(T)owner.GetType().GetField(name,Flags).GetValue(owner);
        static object Call(object owner,string name,params object[] args)=>owner.GetType().GetMethod(name,Flags).Invoke(owner,args);
        static DialoguePresenter D=>DialoguePresenter.Instance;
        static VoicePlaybackSource V=>Field<VoicePlaybackSource>(D,"voicePlayback");
        static int Page=>Field<int>(D,"sequencePageIndex");
        static IEnumerator Wait(double seconds)
        {double end=EditorApplication.timeSinceStartup+seconds;while(EditorApplication.timeSinceStartup<end)yield return null;}
        static IEnumerator Flatten(IEnumerator first)
        {
            var stack=new Stack<IEnumerator>();stack.Push(first);
            while(stack.Count>0)
            {
                var top=stack.Peek();bool moved;
                try{moved=top.MoveNext();}catch(Exception e){Status="FAIL";results.Add("FAIL|Exception."+e);Write();throw;}
                if(!moved){stack.Pop();continue;}if(top.Current is IEnumerator nested)stack.Push(nested);else yield return null;
            }
            Status="PASS";Write();
        }
        static IEnumerator Scene(string name)
        {
            double end=EditorApplication.timeSinceStartup+30;
            while(SceneManager.GetActiveScene().name!=name&&EditorApplication.timeSinceStartup<end)yield return null;
            Check(SceneManager.GetActiveScene().name==name,"Scene."+name);yield return Wait(.6);
            MonsterEncounterService.SuppressForSeconds(3600);
            Check(SceneManager.GetActiveScene().GetRootGameObjects().Sum(r=>r.GetComponentsInChildren<Transform>(true).Sum(t=>GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(t.gameObject)))==0,"MissingScript."+name);
        }
        static void E()
        {InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.E));InputSystem.Update();InputSystem.QueueStateEvent(keyboard,new KeyboardState());InputSystem.Update();}
        static void A()
        {InputSystem.QueueStateEvent(gamepad,new GamepadState().WithButton(GamepadButton.South));InputSystem.Update();InputSystem.QueueStateEvent(gamepad,new GamepadState());InputSystem.Update();}
        static MainQuest07Interactable Site(string id)=>UnityEngine.Object.FindObjectsByType<MainQuest07Interactable>().First(x=>Field<string>(x,"targetId")==id);
        static IEnumerator Near(MainQuest07Interactable site)
        {
            var p=UnityEngine.Object.FindAnyObjectByType<PlayerController>();p.transform.position=site.transform.position;
            yield return Wait(.3);site.GetType().GetField("nearby",Flags).SetValue(site,p);
        }
        [Serializable] sealed class Snapshot
        {public QuestProgressSaveData Quest;public CompanionRosterSaveData Party;public BeastCompanionSaveData Beast;public PartyMemberResourceSaveData[] Resources;}
        static string Snap()=>JsonUtility.ToJson(new Snapshot{Quest=QuestService.ExportSaveData(),Party=CompanionRosterService.ExportSaveData(),Beast=BeastCompanionService.ExportSaveData(),Resources=PartyResourceService.ExportSaveData()});
        // 사용자 슬롯 대신 실행기가 제공한 격리 슬롯에서 실제 Bootstrap 이어하기 버튼을 사용합니다.
        static IEnumerator Continue(string label)
        {
            var p=UnityEngine.Object.FindAnyObjectByType<PlayerController>();Vector2 at=p.transform.position;string expected=Snap();
            Check(GameSaveService.SaveCurrentWorldPosition(at,"Field_02"),"Continue.Write."+label);
            Check(GameSaveService.TryLoadSlot(1,out var data)&&data.Version==1,"Continue.Version1."+label);
            Check(!JsonUtility.ToJson(data).ToLowerInvariant().Contains("voice"),"Continue.NoVoiceState."+label);
            SceneManager.LoadSceneAsync("Bootstrap");yield return Scene("Bootstrap");GameSessionData.Reset();
            GameObject.Find("StartMenuCanvas/Slot01/Action").GetComponent<Button>().onClick.Invoke();yield return Scene("Field_02");
            Check(expected==Snap(),"Continue.QuestPartyFormationBeastHPMP."+label);
            Check(Vector2.Distance(UnityEngine.Object.FindAnyObjectByType<PlayerController>().transform.position,at)<.05f,"Continue.Position."+label);
            Check(!D.IsOpen&&V.Clip==null&&!V.IsPlaying,"Continue.NoAutoVoice."+label);
        }
        // 승인 원본의 전체 native PCM을 내보내고 실제 Source의 자연 종료를 기다립니다.
        // 자동 다음 문장·같은 Clip 재시작은 페이지/Clip/sample 위치로 감시합니다.
        static IEnumerator NaturalVoice(DialogueLine line,int page,VoiceClipCatalog catalog)
        {
            var clip=V.Clip;var source=Field<AudioSource>(V,"source");
            Check(clip!=null&&clip==catalog.Find(line.DialogueId,line.SpeakerId)&&clip.name==line.DialogueId&&V.IsPlaying,"Runtime.PlayExact."+line.DialogueId);
            Check(UnityEngine.Object.FindObjectsByType<VoicePlaybackSource>().Length==1,"Runtime.SingleVoiceSource."+line.DialogueId);
            Check(source.outputAudioMixerGroup!=null&&source.outputAudioMixerGroup.name=="Voice"&&source.volume==1&&source.pitch==1&&!source.loop,"Runtime.ExistingMixerNoProcessing."+line.DialogueId);
            var pcm=new float[clip.samples*clip.channels];Check(clip.GetData(pcm,0),"Runtime.PCMReadable."+line.DialogueId);
            string root=Path.GetFullPath(Path.Combine(Application.dataPath,"../../../Temp/Main07FarewellApplied"));Directory.CreateDirectory(root);
            using(var writer=new BinaryWriter(File.Open(Path.Combine(root,line.DialogueId+".pcm-f32"),FileMode.Create)))foreach(float sample in pcm)writer.Write(sample);
            if(page==0)Volume(V);
            int last=source.timeSamples;double end=EditorApplication.timeSinceStartup+clip.length+3;
            while(V.IsPlaying&&EditorApplication.timeSinceStartup<end)
            {
                if(!D.IsOpen||Page!=page||V.Clip!=clip||source.timeSamples<last)throw new InvalidOperationException("자동 진행/혼입/재시작 "+line.DialogueId);
                last=source.timeSamples;yield return null;
            }
            yield return Wait(.5);
            Check(!V.IsPlaying&&D.IsOpen&&Page==page&&V.Clip==clip,"Runtime.NaturalEndNoAutoAdvance."+line.DialogueId);
            results.Add("NATURAL_END|"+page+"|"+line.DialogueId+"|"+clip.length+"|pageHeld=true");Write();
        }
        // 실제 Mixer 노출 gain과 설정 유지 검증입니다. OS 출력 음색/청취 판정을 대체하지 않습니다.
        static void Volume(VoicePlaybackSource voice)
        {
            int sfx=UserSettingsService.SfxVolume,bgm=UserSettingsService.BgmVolume;
            foreach(int value in new[]{100,40,0})
            {
                UserSettingsService.SetAudioVolumes(value,sfx,bgm);AudioSettingsService.Apply();
                AudioSettingsService.Mixer.GetFloat("VoiceVolume",out float actual);
                Check(Mathf.Abs(actual-AudioSettingsService.ToDecibels(value))<.01f&&UserSettingsService.SfxVolume==sfx&&UserSettingsService.BgmVolume==bgm,"Volume.Mixer."+value);
                if(value==0)Check(!voice.IsAudible,"Volume.ZeroInaudible");
            }
            UserSettingsService.SetAudioVolumes(40,sfx,bgm);UserSettingsService.SetMuteAll(true);AudioSettingsService.Apply();
            AudioSettingsService.Mixer.GetFloat("MasterVolume",out float master);
            Check(master<=-79&&UserSettingsService.VoiceVolume==40&&!voice.IsAudible,"Volume.MasterMutePreservesChannels");
            UserSettingsService.SetMuteAll(false);UserSettingsService.SetAudioVolumes(100,sfx,bgm);AudioSettingsService.Apply();
            AudioSettingsService.Mixer.GetFloat("MasterVolume",out master);Check(Mathf.Abs(master)<.01f&&voice.IsPlaying,"Volume.RestoreDuringPlayback");
        }
        static IEnumerator Run()
        {
            yield return Wait(.5);Check(!string.IsNullOrEmpty(GameSaveService.AuditSaveDirectory),"IsolatedSave");
            GameSaveService.SelectSlot(1);GameSessionData.ConfigurePlayer(PlayerVisualType.Male,"작별 QA");
            GameSessionData.SelectPlayerPath("path.vision");GameSessionData.SelectJob("mage");GameSessionData.ConfigureProgress(3,0);
            CompanionRosterService.UnlockIntroCompanions();GameSessionData.RecordLocation("Field_02",string.Empty);AudioListener.volume=0;
            UserSettingsService.SetMuteAll(false);UserSettingsService.SetAudioVolumes(100,60,20);
            background=InputSystem.settings.backgroundBehavior;editorInput=InputSystem.settings.editorInputBehaviorInPlayMode;inputChanged=true;
            InputSystem.settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;
            InputSystem.settings.editorInputBehaviorInPlayMode=InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
            keyboard=InputSystem.AddDevice<Keyboard>();gamepad=InputSystem.AddDevice<Gamepad>();
            QuestCatalog.TryGet(MainQuest07FieldFlow.QuestId,out var quest);
            // 전투를 반복하지 않고 작별 직전 목표만 정상 완료 기록으로 준비합니다. 대화 완료는 실제 callback이 처리합니다.
            QuestService.ImportSaveData(new QuestProgressSaveData{
                CompletedQuestIds=QuestCatalog.All.Where(q=>q.QuestType==QuestType.Main&&string.CompareOrdinal(q.QuestId,quest.QuestId)<0).Select(q=>q.QuestId).Concat(quest.PrerequisiteQuestIds).Distinct().ToArray(),
                ActiveQuests=new[]{new ActiveQuestSaveData{QuestId=quest.QuestId,Objectives=quest.Objectives.Select((o,i)=>new QuestObjectiveProgressData{ObjectiveId=o.ObjectiveId,CurrentCount=i<quest.Objectives.Count-1?o.RequiredCount:0}).ToArray()}}});
            SceneManager.LoadSceneAsync("Field_02");yield return Scene("Field_02");
            var all=new List<DialogueLine>();
            foreach(string id in new[]{MainQuest07FieldFlow.WheelTracksId,MainQuest07FieldFlow.WoundedTravelerId,MainQuest07FieldFlow.PaulId,MainQuest07FieldFlow.MielMeetingId,MainQuest07FieldFlow.PaulFarewellId})all.AddRange((DialogueLine[])Call(Site(id),"GetLines"));
            Check(all.Count==40,"Audit.Pages40");Check(all.Count(l=>l.IsPlayer)==2,"Audit.PlayerSilent2");
            Check(all.Count(l=>!l.IsPlayer&&!l.IsDirection)==38,"Audit.Character38");
            Check(all.Where(l=>!l.IsPlayer&&!l.IsDirection).All(l=>!string.IsNullOrEmpty(l.DialogueId)),"Audit.CharacterMissingID0");
            Check(all.Where(l=>!l.IsPlayer).Select(l=>l.DialogueId).Distinct().Count()==38,"Audit.UniqueCharacterID38");
            var catalog=Resources.Load<VoiceClipCatalog>("Audio/Voice/Story/StoryVoiceCatalog");
            Check(all.Count(l=>catalog.Find(l.DialogueId,l.SpeakerId)!=null)==(approvedVoice?38:33),approvedVoice?"Audit.AllResolved38":"Audit.ExistingResolved33");
            foreach(var l in all.Where(l=>!l.IsPlayer))
            {
                var resolved=catalog.Find(l.DialogueId,l.SpeakerId);
                if(approvedVoice||!l.DialogueId.StartsWith("main07_paul_supp_"))Check(resolved!=null&&resolved.name==l.DialogueId,"Audit.ExactClip."+l.DialogueId);
            }
            foreach(var l in all)results.Add("PAGE|"+l.DialogueId+"|"+l.SpeakerId+"|"+l.Message);Write();
            Check(QuestService.ActiveMainQuest.CurrentObjective.TargetId==MainQuest07FieldFlow.PaulFarewellId,"Fixture.FarewellObjective");
            yield return Near(Site(MainQuest07FieldFlow.PaulFarewellId));yield return Continue("BeforeFarewell");
            yield return Near(Site(MainQuest07FieldFlow.PaulFarewellId));E();Check(D.IsOpen&&Page==0,"Farewell.RealInputOpen");
            string[] texts={"아무래도 저희가 보고 있는 게 같은 현상 같기는 하네요.","그런데 저는 확인해볼 곳이 하나 더 있습니다.","혼자 가시려고요?","이번에는 진흙 없는 길로요.","아까 충분히 배웠습니다. 헤헤.","다시 만나게 되면 그때 정보부터 맞춰보죠."};
            string[] ids={"main07_paul_supp_001","main07_paul_supp_002","","main07_paul_supp_003","main07_paul_supp_004","main07_paul_supp_005"};
            for(int i=0;i<6;i++)
            {
                var line=Field<DialogueLine[]>(D,"sequenceLines")[Page];
                Check(Page==i&&line.Message==texts[i]&&line.DialogueId==ids[i],"Farewell.OrderTextID."+i);
                Check(i==2?line.IsPlayer:line.SpeakerId==MainQuest07FieldFlow.PaulId,"Farewell.Speaker."+i);
                if(approvedVoice&&i!=2)yield return NaturalVoice(line,i,catalog);
                else Check(catalog.Find(line.DialogueId,line.SpeakerId)==null&&V.Clip==null&&!V.IsPlaying,"Farewell.TextFallback."+i);
                if(i==2)Check(!Field<GameObject>(D,"portraitRoot").activeSelf,"Farewell.PlayerPortraitUnchanged");
                yield return Wait(.5);Check(D.IsOpen&&Page==i,"Farewell.NoAutoAdvance."+i);
                results.Add("RUNTIME_PAGE|"+i+"|"+line.SpeakerId+"|"+line.Message+"|"+line.DialogueId+"|"+(V.Clip==null?"null":V.Clip.name)+"|playing="+V.IsPlaying);Write();
                E();A();E();Check(i==5?!D.IsOpen:Page==i+1,"Farewell.SameFrameOneNextNoReopen."+i);
                if(i<5)Check(V.Clip==catalog.Find(ids[i+1],i+1==2?"player":MainQuest07FieldFlow.PaulId),"Farewell.NextPreviousClipCleared."+i);
                yield return Wait(.2);
            }
            Check(QuestService.GetState(MainQuest07FieldFlow.QuestId)==QuestState.Completed,"Farewell.ActualMain07Completed");
            Check(!D.IsOpen&&V.Clip==null,"Farewell.NoAutoNPCOrResidual");
            // Main08의 기존 시작은 SceneLoaded에서 처리됩니다. 완료 직후에는 Available이고,
            // Field02 재진입 후 Active가 되는 정상 경계를 먼저 지나서 Continue 상태를 비교합니다.
            Check(QuestService.GetState(MainQuest08FieldFlow.QuestId)==QuestState.Available,"Boundary.Main08AvailableBeforeReentry");
            SceneManager.LoadSceneAsync("Field_02");yield return Scene("Field_02");yield return Continue("AfterFarewell");
            Check(QuestService.ActiveMainQuest?.Definition.QuestId==MainQuest08FieldFlow.QuestId,"Boundary.Main08Active");
            var trace=UnityEngine.Object.FindObjectsByType<MainQuest08Interactable>().First(x=>Field<string>(x,"id")==MainQuest08FieldFlow.Trace01);
            var player=UnityEngine.Object.FindAnyObjectByType<PlayerController>();player.transform.position=trace.transform.position;
            yield return Wait(.3);trace.GetType().GetField("nearby",Flags).SetValue(trace,player);E();
            var first=Field<DialogueLine[]>(D,"sequenceLines")[0];var clip=V.Clip;
            Check(D.IsOpen&&Page==0&&first.Message=="잠깐만요."&&first.DialogueId=="main08_taeon_supp_001"&&first.SpeakerId==CompanionRosterService.TaeonId,"Boundary.Main08ExactPage");
            Check(clip!=null&&clip==catalog.Find(first.DialogueId,first.SpeakerId)&&V.IsPlaying,"Boundary.Main08ExistingVoicePlaying");
            yield return Wait(clip.length+1);Check(D.IsOpen&&Page==0&&!V.IsPlaying,"Boundary.NaturalEndNoAutoAdvance");
            D.Hide();Check(V.Clip==null&&!V.IsPlaying,"Boundary.HideCleared");
            SceneManager.LoadSceneAsync("Bootstrap");yield return Scene("Bootstrap");
        }
    }
}

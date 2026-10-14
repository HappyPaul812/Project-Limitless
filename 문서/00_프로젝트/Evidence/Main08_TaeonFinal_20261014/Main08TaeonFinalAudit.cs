using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using ProjectLimitless.Audio;
using ProjectLimitless.Core;
using ProjectLimitless.World;
using ProjectLimitless.UI;
using ProjectLimitless.Player;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace ProjectLimitless.EditorTools
{
    // 원본 게임을 건드리지 않고 페이지·Clip·Mixer 경계를 실제 재생 로직으로 검사합니다.
    [InitializeOnLoad] public static class Main08TaeonFinalAudit
    {
        const string Key="Limitless.Main08TaeonFinalAudit";
        static string Root=>Path.GetFullPath(Path.Combine(Application.dataPath,"../../../Main08TaeonFinal20261014/Runtime"));
        static List<string> results=new List<string>();static List<string> errors=new List<string>();static IEnumerator work;static double deadline;
        static Main08TaeonFinalAudit(){EditorApplication.update+=Tick;Application.logMessageReceived+=Log;EditorApplication.playModeStateChanged+=state=>{if(state==PlayModeStateChange.EnteredPlayMode&&SessionState.GetBool(Key,false)){Isolate();work=Tests();deadline=EditorApplication.timeSinceStartup+180;}};}
        static void Log(string text,string stack,LogType kind){if(SessionState.GetBool(Key,false)&&(kind==LogType.Error||kind==LogType.Exception)){errors.Add(text+"\n"+stack);Directory.CreateDirectory(Root);File.WriteAllLines(Path.Combine(Root,"console_errors.txt"),errors);}}
        static void Check(bool okay,string label){results.Add((okay?"PASS|":"FAIL|")+label);File.WriteAllLines(Path.Combine(Root,"checks.txt"),results);if(!okay)throw new Exception(label);}
        static T Get<T>(object value,string name)=>(T)value.GetType().GetField(name,BindingFlags.Instance|BindingFlags.NonPublic).GetValue(value);
        static void Isolate(){Directory.CreateDirectory(Root);GameSaveService.AuditSaveDirectory=Path.Combine(Root,"Saves");UserSettingsService.BeginAudit(Path.Combine(Root,"Settings"));UserSettingsService.SetMuteAll(true);AudioListener.volume=0;}
        public static void Run(){if(!Application.isBatchMode||!Application.dataPath.Replace('\\','/').EndsWith("/Temp/ArbelV3Trial20261014/Client/Assets"))throw new Exception("Isolation required");Isolate();EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);SessionState.SetBool(Key,true);EditorApplication.EnterPlaymode();}
        static void Tick(){if(work==null)return;try{if(EditorApplication.timeSinceStartup>deadline)throw new Exception("Timeout");if(!work.MoveNext()){work=null;File.WriteAllText(Path.Combine(Root,"status.txt"),"APPROVED_EIGHT_RUNTIME_PASS_FINAL_TWO_CONNECTED");SessionState.SetBool(Key,false);EditorApplication.Exit(0);}}catch(Exception error){File.WriteAllText(Path.Combine(Root,"failure.txt"),error.ToString());work=null;SessionState.SetBool(Key,false);EditorApplication.Exit(1);}}
        static IEnumerator Wait(float duration){float until=Time.realtimeSinceStartup+duration;while(Time.realtimeSinceStartup<until)yield return null;}
        static IEnumerator Load(string scene){SceneManager.LoadScene(scene);for(int i=0;i<20;i++)yield return null;Check(SceneManager.GetActiveScene().name==scene,"Scene."+scene);}
        static void SeedAt(string target){var def=QuestCatalog.All.First(q=>q.QuestId==MainQuest08FieldFlow.QuestId);int index=def.Objectives.ToList().FindIndex(q=>q.TargetId==target);Check(index>=0,"Fixture.Target."+target);QuestService.ImportSaveData(new QuestProgressSaveData{CompletedQuestIds=QuestCatalog.All.Where(q=>q.QuestType==QuestType.Main&&string.CompareOrdinal(q.QuestId,def.QuestId)<0).Select(q=>q.QuestId).Concat(def.PrerequisiteQuestIds).Distinct().ToArray(),ActiveQuests=new[]{new ActiveQuestSaveData{QuestId=def.QuestId,Objectives=def.Objectives.Select((q,i)=>new QuestObjectiveProgressData{ObjectiveId=q.ObjectiveId,CurrentCount=i<index?q.RequiredCount:0}).ToArray()}},TrackedQuestId=def.QuestId});}

        // 실제 격리 Game Camera를 RenderTexture로 읽습니다. OS 창이나 원본 Editor의 포커스를 바꾸지 않습니다.
        static void Capture(string name){
            var camera=Camera.main;Check(camera!=null&&SystemInfo.graphicsDeviceType!=UnityEngine.Rendering.GraphicsDeviceType.Null,"Capture.GPU."+name);
            var rt=RenderTexture.GetTemporary(1280,720,24);var old=camera.targetTexture;var active=RenderTexture.active;
            var canvases=UnityEngine.Object.FindObjectsByType<Canvas>(FindObjectsSortMode.None).Where(c=>c.isRootCanvas&&c.renderMode==RenderMode.ScreenSpaceOverlay).ToArray();
            var cams=canvases.Select(c=>c.worldCamera).ToArray();var distances=canvases.Select(c=>c.planeDistance).ToArray();
            try{camera.targetTexture=rt;foreach(var canvas in canvases){canvas.renderMode=RenderMode.ScreenSpaceCamera;canvas.worldCamera=camera;canvas.planeDistance=1;}Canvas.ForceUpdateCanvases();camera.Render();RenderTexture.active=rt;var tex=new Texture2D(1280,720,TextureFormat.RGBA32,false);tex.ReadPixels(new Rect(0,0,1280,720),0,0);tex.Apply();Directory.CreateDirectory(Path.Combine(Root,"Images"));File.WriteAllBytes(Path.Combine(Root,"Images",name+".png"),tex.EncodeToPNG());UnityEngine.Object.DestroyImmediate(tex);}
            finally{for(int i=0;i<canvases.Length;i++){canvases[i].renderMode=RenderMode.ScreenSpaceOverlay;canvases[i].worldCamera=cams[i];canvases[i].planeDistance=distances[i];}camera.targetTexture=old;RenderTexture.active=active;RenderTexture.ReleaseTemporary(rt);Canvas.ForceUpdateCanvases();}
        }
        static IEnumerator Tests()
        {
            GameSessionData.Reset();GameSaveService.SelectSlot(1);GameSessionData.ConfigurePlayer(PlayerVisualType.Male,"Main08 격리 음성 QA");GameSessionData.SelectJob("fighter");GameSessionData.SelectPlayerPath("path.mobility");GameSessionData.ConfigureProgress(15,0);ProjectLimitless.Monster.MonsterEncounterService.SuppressForSeconds(3600);SeedAt(MainQuest08FieldFlow.Trace01);
            var load=Load("Field_02");while(load.MoveNext())yield return null;
            ProjectLimitless.Monster.MonsterEncounterService.SuppressForSeconds(3600);PlayerController.SetMovementLocked(typeof(Main08TaeonFinalAudit),true);AudioListener.volume=0;UserSettingsService.SetAudioVolumes(100,100,7);UserSettingsService.SetMuteAll(false);AudioSettingsService.Apply();yield return null;
            float level;Check(AudioSettingsService.Mixer.GetFloat("VoiceVolume",out level)&&Mathf.Abs(level)<.01f,"Mixer.Voice100.ZeroDb");Check(AudioSettingsService.Mixer.GetFloat("MasterVolume",out level)&&Mathf.Abs(level)<.01f,"Mixer.MasterNotMuted");
            var catalog=Resources.Load<VoiceClipCatalog>("Audio/Voice/Story/StoryVoiceCatalog");Check(catalog!=null,"Catalog.Exists");var dialogue=DialoguePresenter.Instance;Check(dialogue!=null,"Dialogue.Exists");var voice=Get<VoicePlaybackSource>(dialogue,"voicePlayback");Get<UnityEngine.InputSystem.InputAction>(dialogue,"advanceAction").Disable();var source=voice.GetComponent<AudioSource>();Check(source.outputAudioMixerGroup.name=="Voice"&&source.volume==1&&!source.mute&&source.spatialBlend==0,"Source.RouteGain2D");
            foreach(var target in new[]{MainQuest08FieldFlow.Trace01,MainQuest08FieldFlow.Trace02,MainQuest08FieldFlow.Trace03,MainQuest08FieldFlow.WheelTracks})
            {
                SeedAt(target);var lines=(DialogueLine[])typeof(MainQuest08FieldFlow).GetMethod("Lines",BindingFlags.Static|BindingFlags.NonPublic).Invoke(null,new object[]{target});double requestAt=0;var site=UnityEngine.Object.FindObjectsByType<MainQuest08Interactable>(FindObjectsInactive.Include,FindObjectsSortMode.None).First(q=>Get<string>(q,"id")==target);var player=UnityEngine.Object.FindAnyObjectByType<PlayerController>();player.transform.position=site.transform.position+new Vector3(0,.4f,0);player.GetComponent<Rigidbody2D>().position=player.transform.position;site.GetType().GetField("nearby",BindingFlags.Instance|BindingFlags.NonPublic).SetValue(site,player);requestAt=Time.realtimeSinceStartupAsDouble;site.GetType().GetMethod("Interact",BindingFlags.Instance|BindingFlags.NonPublic).Invoke(site,null);Check(dialogue.IsOpen,"ActualInteraction.Open."+target);
                for(int i=0;i<lines.Length;i++)
                {
                    var line=lines[i];Check(Get<int>(dialogue,"sequencePageIndex")==i,"Page.Index."+target+"."+i);Check(Get<Text>(dialogue,"dialogueText").text==line.Message,"Page.Subtitle."+line.DialogueId);Check(Get<Text>(dialogue,"speakerText").text==line.SpeakerName,"Page.Speaker."+line.DialogueId);var expected=catalog.Find(line.DialogueId,line.SpeakerId);Check(voice.Clip==expected,"Page.ExactCatalogClip."+line.DialogueId);Check(UnityEngine.Object.FindObjectsByType<VoicePlaybackSource>(FindObjectsSortMode.None).Length==1,"SingleVoiceSource."+i);
                    if(!line.IsPlayer){Check(expected!=null&&expected.name==line.DialogueId&&voice.IsPlaying,"Page.Playing."+line.DialogueId);float[] samples=new float[expected.samples*expected.channels];Check(expected.GetData(samples,0),"Clip.GetData."+line.DialogueId);double rms=Math.Sqrt(samples.Select(x=>(double)x*x).Average());results.Add("PCM|"+line.DialogueId+"|length="+expected.length+"|rms_dbfs="+20*Math.Log10(rms)+"|asset="+AssetDatabase.GetAssetPath(expected));Check(voice.IsAudible,"Source.AudibleSettings."+line.DialogueId);
                    var approved=File.ReadAllLines(Path.Combine(Root,"approved_pcm.tsv")).Where(x=>x.StartsWith(line.DialogueId+"\t",StringComparison.Ordinal)).ToArray();
                    if(approved.Length==1){byte[] bytes=new byte[samples.Length*2];for(int n=0;n<samples.Length;n++){short value=(short)Mathf.Clamp(Mathf.RoundToInt(samples[n]*32768f),-32768,32767);bytes[n*2]=(byte)(value&255);bytes[n*2+1]=(byte)((value>>8)&255);}string digest;using(var hash=System.Security.Cryptography.SHA256.Create())digest=BitConverter.ToString(hash.ComputeHash(bytes)).Replace("-","").ToLowerInvariant();Check(digest==approved[0].Split('\t')[1],"ApprovedPCM.ExactRuntime."+line.DialogueId);AssetDatabase.TryGetGUIDAndLocalFileIdentifier(expected,out string actualGuid,out long actualFileId);string wantedGuid=File.ReadAllLines(Path.Combine(Root,"approved_guid.tsv")).First(x=>x.StartsWith(line.DialogueId+"\t",StringComparison.Ordinal)).Split('\t')[1];Check(actualGuid==wantedGuid&&actualFileId==8300000,"ApprovedGUID.ExactRuntime."+line.DialogueId);Check(AssetDatabase.GetAssetPath(expected).Contains(line.DialogueId=="main08_taeon_supp_005"||line.DialogueId=="main08_taeon_supp_006"?"/ApprovedTaeon00500620261014/":"/ApprovedRecovery20261013/"),"ApprovedPath.ExactRuntime."+line.DialogueId);}
                    else if(line.DialogueId=="main08_taeon_supp_005"||line.DialogueId=="main08_taeon_supp_006")results.Add("SEMANTIC_OPEN|"+line.DialogueId+"|unapproved existing source content; unchanged");
                    Check(Get<GameObject>(dialogue,"portraitRoot").activeSelf,"Portrait.Character."+line.DialogueId);Check(Get<Image>(dialogue,"portraitImage").sprite==DialoguePortraitCatalog.GetPortrait(line.SpeakerId),"Portrait.ExactSpeaker."+line.DialogueId);
                    if(line.DialogueId=="main08_taeon_supp_005"||line.DialogueId=="main08_taeon_supp_006"){
                        double positiveAt=Time.realtimeSinceStartupAsDouble;float deadlineAt=Time.realtimeSinceStartup+1;
                        while(source.timeSamples<=0&&Time.realtimeSinceStartup<deadlineAt)yield return null;
                        positiveAt=Time.realtimeSinceStartupAsDouble;Check(source.timeSamples>0,"Playback.ClockStarted."+line.DialogueId);
                        Check(positiveAt-requestAt<.5,"Playback.NoLongStartDelay."+line.DialogueId);
                        AudioSettings.GetDSPBufferSize(out int buffer,out int count);
                        File.AppendAllText(Path.Combine(Root,"playback_timeline.csv"),line.DialogueId+","+(positiveAt-requestAt)+","+source.timeSamples+","+expected.frequency+","+AudioSettings.outputSampleRate+","+buffer+","+count+","+expected.loadState+"\n");
                        Capture(line.DialogueId);
                    }
                    if(target==MainQuest08FieldFlow.Trace03&&line.IsPlayer)Capture("Trace03_PlayerSilent");
                    if(line.DialogueId=="main08_miel_supp_004")Capture("WheelTracks_Miel004");
                    var natural=Wait(expected.length+.4f);while(natural.MoveNext())yield return null;float audioDeadline=Time.realtimeSinceStartup+2;while(voice.IsPlaying && Time.realtimeSinceStartup<audioDeadline)yield return null;results.Add("NATURAL_DIAG|"+line.DialogueId+"|playing="+voice.IsPlaying+"|page="+Get<int>(dialogue,"sequencePageIndex")+"|open="+dialogue.IsOpen+"|samples="+source.timeSamples+"|pitch="+source.pitch);Check(!voice.IsPlaying&&Get<int>(dialogue,"sequencePageIndex")==i,"NaturalEnd.NoAutoAdvance."+line.DialogueId);Check(catalog.Find(line.DialogueId,"wrong_speaker")==null,"Catalog.SpeakerGuard."+line.DialogueId);}
                    else Check(voice.Clip==null&&!voice.IsPlaying,"Player.SilentClearsPrevious");
                    if(line.DialogueId=="main08_taeon_supp_002"){var timer=Wait(.05f);while(timer.MoveNext())yield return null;Check(!voice.IsPlaying&&Get<int>(dialogue,"sequencePageIndex")==i,"QuietClip.FinishedNaturally.NoAutoNext");}
                    // 실제 Next 입력은 같은 프레임 중복을 차단합니다.
                    yield return null;requestAt=Time.realtimeSinceStartupAsDouble;dialogue.AdvanceFromInput();dialogue.AdvanceFromInput();Check(i==lines.Length-1?!dialogue.IsOpen:Get<int>(dialogue,"sequencePageIndex")==i+1,"Next.SingleAdvance."+target+"."+i);
                }
                Check(voice.Clip==null&&!voice.IsPlaying,"Close.NoResidual."+target);Check(QuestService.ActiveMainQuest.CurrentObjective.TargetId!=target,"Quest.ActualCompletion."+target);yield return null;
            }
            var fast=(DialogueLine[])typeof(MainQuest08FieldFlow).GetMethod("Lines",BindingFlags.Static|BindingFlags.NonPublic).Invoke(null,new object[]{MainQuest08FieldFlow.Trace02});dialogue.ShowSequence(fast,null);yield return null;Check(Get<VoicePlaybackSource>(dialogue,"voicePlayback").IsPlaying,"FastNext.OldStillPlaying");dialogue.AdvanceFromInput();dialogue.AdvanceFromInput();Check(Get<int>(dialogue,"sequencePageIndex")==1&&voice.Clip==catalog.Find(fast[1].DialogueId,fast[1].SpeakerId),"FastNext.OneNewClipAndNoOverlap");dialogue.Hide();Check(voice.Clip==null&&!voice.IsPlaying,"FastNext.CloseClears");
            var probe=catalog.Find("main08_miel_supp_003",CompanionRosterService.MielId);voice.Play(probe);yield return null;voice.Pause();Check(!voice.IsPlaying,"Pause.Stopped");voice.Resume();yield return null;Check(voice.Clip==probe&&voice.IsPlaying,"Resume.SameClip");voice.Stop();
            UserSettingsService.SetAudioVolumes(0,100,7);AudioSettingsService.Apply();Check(AudioSettingsService.Mixer.GetFloat("VoiceVolume",out level)&&level<=-79,"Mixer.VoiceZero");UserSettingsService.SetAudioVolumes(35,100,7);AudioSettingsService.Apply();Check(AudioSettingsService.Mixer.GetFloat("VoiceVolume",out level)&&Mathf.Abs(level-AudioSettingsService.ToDecibels(35))<.01f,"Mixer.Voice35SingleGain");UserSettingsService.SetMuteAll(true);AudioSettingsService.Apply();Check(AudioSettingsService.Mixer.GetFloat("MasterVolume",out level)&&level<=-79,"Mixer.MuteMasterOnly");UserSettingsService.SetMuteAll(false);
            var at=(Vector2)UnityEngine.Object.FindAnyObjectByType<PlayerController>().transform.position;string quest=JsonUtility.ToJson(QuestService.ExportSaveData());Check(GameSaveService.SaveCurrentWorldPosition(at,"Field_02"),"Save.Write");Check(GameSaveService.TryLoadSlot(1,out var saved)&&saved.Version==1,"Save.VersionUnchanged");var boot=Load("Bootstrap");while(boot.MoveNext())yield return null;GameSessionData.Reset();GameObject.Find("StartMenuCanvas/Slot01/Action").GetComponent<Button>().onClick.Invoke();for(int i=0;i<25;i++)yield return null;Check(SceneManager.GetActiveScene().name=="Field_02","Continue.ActualBootstrap");Check(JsonUtility.ToJson(QuestService.ExportSaveData())==quest,"Continue.QuestSame");Check(Get<VoicePlaybackSource>(DialoguePresenter.Instance,"voicePlayback").Clip==null,"Continue.NoStaleClip");
            SeedAt(MainQuest08FieldFlow.Trace03);var repeatSite=UnityEngine.Object.FindObjectsByType<MainQuest08Interactable>(FindObjectsInactive.Include,FindObjectsSortMode.None).First(q=>Get<string>(q,"id")==MainQuest08FieldFlow.Trace03);var repeatPlayer=UnityEngine.Object.FindAnyObjectByType<PlayerController>();repeatPlayer.transform.position=repeatSite.transform.position+new Vector3(0,.4f,0);repeatPlayer.GetComponent<Rigidbody2D>().position=repeatPlayer.transform.position;repeatSite.GetType().GetField("nearby",BindingFlags.Instance|BindingFlags.NonPublic).SetValue(repeatSite,repeatPlayer);repeatSite.GetType().GetMethod("Interact",BindingFlags.Instance|BindingFlags.NonPublic).Invoke(repeatSite,null);yield return null;
            var resumed=DialoguePresenter.Instance;Get<UnityEngine.InputSystem.InputAction>(resumed,"advanceAction").Disable();resumed.AdvanceFromInput();yield return null;resumed.AdvanceFromInput();yield return null;
            var resumedVoice=Get<VoicePlaybackSource>(resumed,"voicePlayback");Check(resumedVoice.Clip==catalog.Find("main08_taeon_supp_005",CompanionRosterService.TaeonId)&&resumedVoice.IsPlaying,"Continue.Trace03.Replays005");Capture("Continue_Trace03_005");resumed.Hide();Check(resumedVoice.Clip==null&&!resumedVoice.IsPlaying,"Continue.Cancel.ClearsVoice");
            Check(errors.All(x=>x.Contains("UnityEditor.Search.SearchDatabase")&&x.Contains("UnityEditor.Search.SearchInit.IndexationOnStartup")),"Console.NoNewRuntimeErrors");
        }
    }
}

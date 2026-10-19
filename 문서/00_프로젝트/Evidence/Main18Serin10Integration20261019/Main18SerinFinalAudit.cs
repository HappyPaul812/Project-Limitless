using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ProjectLimitless.Audio;
using ProjectLimitless.Core;
using ProjectLimitless.World;
using ProjectLimitless.Player;
using ProjectLimitless.Monster;
using ProjectLimitless.NPC;
using ProjectLimitless.UI;
using ProjectLimitless.Battle;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object=UnityEngine.Object;
namespace ProjectLimitless.EditorTools
{
    /// <summary>실제 Main18/19 상호작용·페이지·단일 음성 Source를 별도 Save로 검사합니다. TTS 호출은 없습니다.</summary>
    [InitializeOnLoad]
    public static class Main18SerinFinalAudit
    {
        const string Key="Limitless.Main18SerinFinalAudit20261019";
        [Serializable] public class Row { public int chapter,story_order;public string dialogue_id,speaker_id,text,source,sha256;public bool new_approved; }
        [Serializable] public class Rows { public Row[] rows; }
        static string Root=>Path.GetFullPath(Path.Combine(Application.dataPath,"../../../Main18Serin10Integration20261019/Evidence/Runtime"));
        static readonly BindingFlags Flags=BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
        static readonly List<string> Checks=new List<string>(),Errors=new List<string>();
        static readonly HashSet<string> Played=new HashSet<string>(),Silent=new HashSet<string>();
        static Row[] rows;static IEnumerator routine;static double deadline;
        static readonly string[] Approved="main18_ground_serin_01 main18_heat_vent_serin_01 main18_leon_serin_01 main18_overheat_serin_01 main18_overheat_serin_02 main18_overheat_serin_03 main18_overheat_serin_04 main18_vibration_serin_01 main18_watcher_witness_serin_01 main18_watcher_witness_serin_02".Split(' ');
        static Main18SerinFinalAudit(){EditorApplication.playModeStateChanged+=s=>{if(s!=PlayModeStateChange.EnteredPlayMode||!SessionState.GetBool(Key,false))return;
            GameSaveService.AuditSaveDirectory=Path.Combine(Root,"Saves");UserSettingsService.BeginAudit(Path.Combine(Root,"Settings"));UserSettingsService.SetMuteAll(false);UserSettingsService.SetAudioVolumes(100,0,0);
            AudioListener.volume=0;Application.runInBackground=true;Application.logMessageReceived+=(t,st,k)=>{if(k==LogType.Error||k==LogType.Exception){Errors.Add(t+"\n"+st);File.WriteAllLines(Path.Combine(Root,"errors.txt"),Errors);}};
            rows=JsonUtility.FromJson<Rows>(File.ReadAllText(Path.Combine(Root,"../RuntimeRows.json"))).rows;routine=Flatten(Run());deadline=EditorApplication.timeSinceStartup+1200;EditorApplication.update+=Tick;};}
        public static void RunBatch(){if(!Application.isBatchMode||!Application.dataPath.Replace('\\','/').Contains("/Temp/Field05Review20261019/Client/Assets"))throw new InvalidOperationException("격리 Batch만 허용");Directory.CreateDirectory(Root);File.WriteAllText(Path.Combine(Root,"status.txt"),"RUNNING");File.WriteAllText(Path.Combine(Root,"errors.txt"),"");File.WriteAllText(Path.Combine(Root,"failure.txt"),"");File.WriteAllText(Path.Combine(Root,"PCMResults.tsv"),"");GameSaveService.AuditSaveDirectory=Path.Combine(Root,"Saves");UserSettingsService.BeginAudit(Path.Combine(Root,"Settings"));EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);SessionState.SetBool(Key,true);EditorApplication.EnterPlaymode();}
        static void Check(bool b,string id){Checks.Add((b?"PASS|":"FAIL|")+id);File.WriteAllLines(Path.Combine(Root,"checks.txt"),Checks);if(!b)throw new InvalidOperationException(id);}
        static T Get<T>(object o,string n)=>(T)o.GetType().GetField(n,Flags).GetValue(o);
        static object Call(object o,string n,params object[] a)=>o.GetType().GetMethod(n,Flags).Invoke(o,a);
        static IEnumerator Wait(float t){float end=Time.realtimeSinceStartup+t;while(Time.realtimeSinceStartup<end)yield return null;}
        static VoiceClipCatalog Catalog=>Resources.Load<VoiceClipCatalog>("Audio/Voice/Story/StoryVoiceCatalog");
        static VoicePlaybackSource Voice=>Get<VoicePlaybackSource>(DialoguePresenter.Instance,"voicePlayback");
        static void NoResidual(string label){Check(Object.FindObjectsByType<VoicePlaybackSource>(FindObjectsSortMode.None).All(x=>!x.IsPlaying&&x.Clip==null),"NoResidual."+label);}
        static IEnumerator Load(string scene){
            // 직접 Scene을 여는 검수에서도 실제 월드 진입과 같은 세션 위치를 먼저 기록해 자동 저장을 보호합니다.
            if(scene!="Bootstrap"&&scene!="Battle")GameSessionData.RecordLocation(scene,scene=="Arbel"?"Spawn_Arbel_Center":scene==Chapter2Main18Flow.Field?"Spawn_From_Field08":scene==Chapter2Main19Flow.Field?"Spawn_From_Field09":"");
            SceneManager.LoadSceneAsync(scene);float end=Time.realtimeSinceStartup+40;while(SceneManager.GetActiveScene().name!=scene&&Time.realtimeSinceStartup<end)yield return null;Check(SceneManager.GetActiveScene().name==scene,"Scene."+scene);MonsterEncounterService.SuppressForSeconds(3600);yield return Wait(.4f);var p=Object.FindAnyObjectByType<PlayerController>();if(p!=null)p.enabled=false;
            Check(SceneManager.GetActiveScene().GetRootGameObjects().SelectMany(x=>x.GetComponentsInChildren<Transform>(true)).Sum(x=>GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(x.gameObject))==0,"MissingScript0."+scene);NoResidual("Load."+scene);}
        static void At(Vector2 at){var p=Object.FindAnyObjectByType<PlayerController>();p.enabled=false;p.transform.position=at;var b=p.GetComponent<Rigidbody2D>();b.linearVelocity=Vector2.zero;b.position=at;Physics2D.SyncTransforms();}
        static void Index(int chapter,int index){DialoguePresenter.Instance?.Hide();string id=chapter==18?Chapter2Main18Flow.QuestId:Chapter2Main19Flow.QuestId;QuestCatalog.TryGet(id,out var quest);
            QuestService.ImportSaveData(new QuestProgressSaveData{CompletedQuestIds=QuestCatalog.All.Where(x=>x.QuestType==QuestType.Main&&x.QuestId!=id).Select(x=>x.QuestId).ToArray(),ActiveQuests=new[]{new ActiveQuestSaveData{QuestId=id,Objectives=quest.Objectives.Select((x,i)=>new QuestObjectiveProgressData{ObjectiveId=x.ObjectiveId,CurrentCount=i<index?x.RequiredCount:0}).ToArray()}}});}
        static void Composition(string other){CompanionRosterService.ImportSaveData(new CompanionRosterSaveData{UnlockedCharacterIds=new[]{"companion_serin","companion_paul","companion_miel","companion_taeon"},ActivePartyCharacterIds=new[]{"companion_serin",other},HasManualComposition=true});}
        static IEnumerator Pages(string label)
        {
            var d=DialoguePresenter.Instance;Check(d!=null&&d.IsOpen,"Story.Open."+label);var lines=Get<DialogueLine[]>(d,"sequenceLines");int objective=QuestService.ActiveMainQuest.CurrentObjectiveIndex;string party=JsonUtility.ToJson(CompanionRosterService.ExportSaveData());
            for(int page=0;page<lines.Length;page++)
            {
                var line=lines[page];var voice=Voice;var source=Get<AudioSource>(voice,"source");AudioClip expected=Catalog.Find(line.DialogueId,line.SpeakerId);
                Check(Get<Text>(d,"dialogueText").text==line.Message&&Get<Text>(d,"speakerText").text==line.SpeakerName,"Page.TextSpeaker."+line.DialogueId);
                Check(Object.FindObjectsByType<VoicePlaybackSource>(FindObjectsSortMode.None).Count()==1,"Voice.SingleSource."+line.DialogueId);
                Check(QuestService.ActiveMainQuest.CurrentObjectiveIndex==objective,"Quest.NoEarlyAdvance."+line.DialogueId);
                if(expected!=null&&!line.IsPlayer&&!line.IsDirection)
                {
                    Check(voice.Clip==expected&&voice.IsPlaying,"Runtime.Playing."+line.DialogueId);
                    Check(source.pitch==1&&source.volume==1&&!source.loop&&source.spatialBlend==0&&source.outputAudioMixerGroup!=null&&source.outputAudioMixerGroup.name=="Voice","Voice.Contract."+line.DialogueId);
                    var row=rows.FirstOrDefault(x=>x.dialogue_id==line.DialogueId);if(row!=null){Check(row.text==line.Message.Replace("\r\n","\n").Trim()&&row.speaker_id==line.SpeakerId,"Runtime.ManifestExact."+line.DialogueId);Played.Add(line.DialogueId);File.WriteAllLines(Path.Combine(Root,"played_ids.txt"),Played.OrderBy(x=>x));}
                    if(line.DialogueId=="main18_leon_leon_01")Volume(voice);
                    yield return Wait(.12f);Check(source.timeSamples>0,"DSP.TimeSamplesAdvance."+line.DialogueId);
                    float end=Time.realtimeSinceStartup+expected.length+3;while(voice.IsPlaying&&Time.realtimeSinceStartup<end)yield return null;
                    Check(!voice.IsPlaying&&d.IsOpen&&Get<int>(d,"sequencePageIndex")==page&&QuestService.ActiveMainQuest.CurrentObjectiveIndex==objective,"NaturalEnd.NoAutoAdvance."+line.DialogueId);
                }
                else
                {
                    Check(voice.Clip==null&&!voice.IsPlaying,"Runtime.Silent."+line.DialogueId);
                    if(Approved.Contains(line.DialogueId)){Silent.Add(line.DialogueId);File.WriteAllLines(Path.Combine(Root,"unexpected_approved_silent_ids.txt"),Silent.OrderBy(x=>x));}
                }
                d.Advance();yield return Wait(.05f);if(d.IsOpen){var next=lines[page+1];Check(Voice.Clip==Catalog.Find(next.DialogueId,next.SpeakerId),"Next.ExactClip."+line.DialogueId);}
            }
            Check(JsonUtility.ToJson(CompanionRosterService.ExportSaveData())==party,"Party.Unchanged."+label);NoResidual("StoryClosed."+label);
        }
        static void Volume(VoicePlaybackSource voice){int sfx=UserSettingsService.SfxVolume,bgm=UserSettingsService.BgmVolume;foreach(int v in new[]{100,40,0}){UserSettingsService.SetAudioVolumes(v,sfx,bgm);AudioSettingsService.Apply();AudioSettingsService.Mixer.GetFloat("VoiceVolume",out float db);Check(Mathf.Abs(db-AudioSettingsService.ToDecibels(v))<.01f,"Mixer.Voice."+v);if(v==0)Check(!voice.IsAudible,"Mixer.ZeroInaudible");}UserSettingsService.SetAudioVolumes(40,sfx,bgm);UserSettingsService.SetMuteAll(true);AudioSettingsService.Apply();AudioSettingsService.Mixer.GetFloat("MasterVolume",out float master);Check(master<=-79&&UserSettingsService.VoiceVolume==40&&!voice.IsAudible,"Mixer.MutePreservesVoice");UserSettingsService.SetMuteAll(false);UserSettingsService.SetAudioVolumes(100,sfx,bgm);AudioSettingsService.Apply();Check(voice.IsAudible,"Mixer.UnmuteRestore");}
        static IEnumerator Site(int chapter,int index,string other="companion_miel")
        {
            Composition(other);Index(chapter,index);yield return Wait(.15f);
            if(chapter==18&&index==1){var role=Object.FindObjectsByType<VillageNpcRole>(FindObjectsSortMode.None).Single(x=>x.NpcId=="arbel-leon");At(role.transform.position);Check(Chapter2Main18Flow.TryHandleNpc("arbel-leon",role.GetComponent<NpcController>()),"Main18.LeonActualHandler");}
            else{Vector2 at=chapter==18?Chapter2Main18Flow.Positions[index]:Chapter2Main19Flow.Positions[index];At(at);yield return Wait(.12f);if(chapter==18)Object.FindObjectsByType<Main18Site>(FindObjectsSortMode.None).Single(x=>Get<int>(x,"index")==index).TryInteract();else Object.FindObjectsByType<Main19Site>(FindObjectsSortMode.None).Single(x=>Get<int>(x,"index")==index).TryInteract();}
            yield return Wait(.03f);if(index==9&&chapter==18||index==10&&chapter==19){var lines=Get<DialogueLine[]>(DialoguePresenter.Instance,"sequenceLines");Check(lines.Any(x=>x.SpeakerId==other)&&!lines.Any(x=>new[]{"companion_paul","companion_miel","companion_taeon"}.Contains(x.SpeakerId)&&x.SpeakerId!=other),"Conditional.ActualParty."+chapter+other);}
            yield return Pages(chapter+"."+index+"."+other);
            // 목격 대화 종료는 실제 Battle로 이어지므로 다음 편성 검사는 Field 복귀 뒤 수행합니다.
            if(chapter==18&&index==9){yield return Wait(.4f);if(SceneManager.GetActiveScene().name=="Battle")yield return Load(Chapter2Main18Flow.Field);}
        }
        static IEnumerator Continue(string label){DialoguePresenter.Instance?.Hide();At(new Vector2(0,0));yield return Wait(.1f);string quest=JsonUtility.ToJson(QuestService.ExportSaveData()),party=JsonUtility.ToJson(CompanionRosterService.ExportSaveData()),scene=SceneManager.GetActiveScene().name;var position=Object.FindAnyObjectByType<PlayerController>().transform.position;Check(GameSaveService.SaveCurrentWorldPosition(position,scene),"Save.Write."+label);yield return Load("Bootstrap");GameSessionData.Reset();GameObject.Find("StartMenuCanvas/Slot01/Action").GetComponent<Button>().onClick.Invoke();float end=Time.realtimeSinceStartup+40;while(SceneManager.GetActiveScene().name!=scene&&Time.realtimeSinceStartup<end)yield return null;yield return Wait(.4f);Check(SceneManager.GetActiveScene().name==scene&&Vector2.Distance(Object.FindAnyObjectByType<PlayerController>().transform.position,position)<.05f&&quest==JsonUtility.ToJson(QuestService.ExportSaveData())&&party==JsonUtility.ToJson(CompanionRosterService.ExportSaveData()),"Continue.ExactQuestPartyPosition."+label);NoResidual("Continue."+label);}
        static IEnumerator Boundaries()
        {
            Index(18,11);At(Chapter2Main18Flow.Positions[11]);yield return Wait(.15f);Object.FindObjectsByType<Main18Site>(FindObjectsSortMode.None).Single(x=>Get<int>(x,"index")==11).TryInteract();yield return Wait(.15f);Check(Voice.IsPlaying,"Boundary.BeforeRapidNext");DialoguePresenter.Instance.Advance();Check(Voice.Clip==null&&!Voice.IsPlaying,"Boundary.NextToPlayerStopsPrevious");DialoguePresenter.Instance.Advance();DialoguePresenter.Instance.Advance();Check(Voice.IsPlaying&&Voice.Clip.name=="main18_after_watcher_serin_03","Boundary.RapidNextOnlyLatest");yield return Load(Chapter2Main19Flow.Field);NoResidual("Boundary.SceneChange");
        }
        static IEnumerator StoryBattle(int chapter,int index)
        {
            yield return Load(chapter==18?Chapter2Main18Flow.Field:Chapter2Main19Flow.Field);Index(chapter,index);At(chapter==18?Chapter2Main18Flow.Positions[index]:Chapter2Main19Flow.Positions[index]);yield return Wait(.2f);if(chapter==18)Object.FindObjectsByType<Main18Site>(FindObjectsSortMode.None).Single(x=>Get<int>(x,"index")==index).TryInteract();else Object.FindObjectsByType<Main19Site>(FindObjectsSortMode.None).Single(x=>Get<int>(x,"index")==index).TryInteract();float end=Time.realtimeSinceStartup+30;while(SceneManager.GetActiveScene().name!="Battle"&&Time.realtimeSinceStartup<end)yield return null;yield return Wait(.4f);Check(SceneManager.GetActiveScene().name=="Battle","Battle.ActualStoryEnter."+chapter);NoResidual("Battle."+chapter);var c=Object.FindAnyObjectByType<BattleSceneController>();c.StopAllCoroutines();Call(c,"EndBattle","격리 음성 QA 승리",true);yield return Wait(.15f);Object.FindObjectsByType<Button>(FindObjectsSortMode.None).Single(x=>x.name=="VictoryReturn").onClick.Invoke();end=Time.realtimeSinceStartup+30;while(SceneManager.GetActiveScene().name=="Battle"&&Time.realtimeSinceStartup<end)yield return null;yield return Wait(.4f);Check(QuestService.ActiveMainQuest.CurrentObjectiveIndex==index+1,"Battle.QuestAdvancedOnce."+chapter);NoResidual("BattleReturn."+chapter);}
        // 승인 WAV의 RIFF data와 Unity GetData를 모든 샘플에서 대조해 추가 가공이 없는지 확인합니다.
        static void DecodeAndStoryOrder(){
            Check(rows.Length==35,"Input.New10PlusProtected25");var lines=new List<DialogueLine>();for(int i=0;i<14;i++)lines.AddRange(Main18DialogueCatalog.Get(i,"path.vision",true));var npc=lines.Where(x=>!x.IsPlayer&&!x.IsDirection&&!string.IsNullOrWhiteSpace(x.SpeakerId)).GroupBy(x=>x.DialogueId).Select(g=>g.First()).ToArray();Check(npc.Length==22,"Main18.StoryNPC22");File.WriteAllLines(Path.Combine(Root,"StoryOrder22.tsv"),npc.Select((x,i)=>(i+1)+"\t"+x.DialogueId+"\t"+x.SpeakerId+"\t"+x.Message));
            foreach(var row in rows){var clip=Catalog.Find(row.dialogue_id,row.speaker_id);Check(clip!=null,"Resolve35."+row.dialogue_id);if(row.chapter==18){var line=npc.Single(x=>x.DialogueId==row.dialogue_id);Check(line.SpeakerId==row.speaker_id&&line.Message.Trim()==row.text.Trim(),"ExactStory."+row.dialogue_id);if(row.story_order>0)Check(Array.FindIndex(npc,x=>x.DialogueId==row.dialogue_id)+1==row.story_order,"ApprovedStoryOrder."+row.dialogue_id);}
                Check(Catalog.Find(row.dialogue_id,"player")==null,"WrongSpeakerRejected."+row.dialogue_id);VerifyPCM(clip,row.source,row.dialogue_id);
                var path=AssetDatabase.GetAssetPath(clip);var ti=(AudioImporter)AssetImporter.GetAtPath(path);Check(ti.defaultSampleSettings.compressionFormat==AudioCompressionFormat.PCM&&ti.defaultSampleSettings.sampleRateSetting==AudioSampleRateSetting.PreserveSampleRate&&!ti.forceToMono&&!new SerializedObject(ti).FindProperty("m_Normalize").boolValue,"Importer.NoProcessing."+row.dialogue_id);}
        }
        static void VerifyPCM(AudioClip clip,string path,string id){clip.LoadAudioData();var samples=new float[clip.samples*clip.channels];Check(clip.GetData(samples,0),"GetData.Decode."+id);byte[] bytes=File.ReadAllBytes(path);int offset=12;byte[] pcm=null;while(offset+8<=bytes.Length){string chunk=System.Text.Encoding.ASCII.GetString(bytes,offset,4);int size=BitConverter.ToInt32(bytes,offset+4);if(chunk=="data"){pcm=new byte[size];Array.Copy(bytes,offset+8,pcm,0,size);break;}offset+=8+size+(size%2);}Check(pcm!=null&&pcm.Length==samples.Length*2&&clip.channels==1&&clip.frequency==24000,"PCM.Format."+id);float max=0;for(int i=0;i<samples.Length;i++)max=Mathf.Max(max,Mathf.Abs(samples[i]-BitConverter.ToInt16(pcm,i*2)/32768f));Check(max==0,"PCM.ExactZeroError."+id);File.AppendAllText(Path.Combine(Root,"PCMResults.tsv"),id+"\t"+samples.Length+"\t"+max+"\n");}
        // Main17 승인 세린11개도 현재 정본 대사로 재생해 기존 연결과 정리 경계를 확인합니다.
        static IEnumerator Main17Important(){var lines=new List<DialogueLine>();foreach(string path in new[]{"path.vision","path.hearing"})for(int i=0;i<12;i++)lines.AddRange(Main17DialogueCatalog.Get(i,path,true));
            var serin=lines.Where(x=>x.SpeakerId=="companion_serin").GroupBy(x=>x.DialogueId).Select(g=>g.First()).ToArray();Check(serin.Length==11,"Main17.SerinApproved11");foreach(var line in serin){var clip=Catalog.Find(line.DialogueId,line.SpeakerId);Check(clip!=null,"Main17.Resolve."+line.DialogueId);DialoguePresenter.Instance.ShowSequence(new[]{line},null);yield return Wait(.05f);Check(Voice.Clip==clip&&Voice.IsPlaying,"Main17.ActualDialoguePlay."+line.DialogueId);yield return Wait(.12f);Check(Get<AudioSource>(Voice,"source").timeSamples>0,"Main17.TimeSamples."+line.DialogueId);DialoguePresenter.Instance.Hide();NoResidual("Main17.Hide."+line.DialogueId);}}
        static IEnumerator Run()
        {
            GameSessionData.Reset();GameSaveService.SelectSlot(1);GameSessionData.ConfigurePlayer(PlayerVisualType.Male,"Main18·19 음성 격리 QA");GameSessionData.SelectJob("fighter");GameSessionData.SelectPlayerPath("path.vision");GameSessionData.ConfigureProgress(14,0);Composition("companion_miel");Index(18,1);yield return Load("Arbel");
            Check(Approved.All(x=>Catalog.Find(x,"companion_serin")!=null),"Catalog.Approved10Present");DecodeAndStoryOrder();Check(Catalog!=null,"Catalog.Load");yield return Site(18,1);yield return Continue("Main18Leon");
            yield return Load(Chapter2Main18Flow.Field);foreach(int i in new[]{4,5,7,8,11,12})yield return Site(18,i);
            foreach(string other in new[]{"companion_miel","companion_paul","companion_taeon"})yield return Site(18,9,other);
            Check(Played.Count(x=>x.StartsWith("main18_"))==22&&!Silent.Overlaps(Approved),"Main18.Play22NoApprovedSilence");yield return Boundaries();
            foreach(int i in new[]{2,3,5,8,11})yield return Site(19,i);foreach(string other in new[]{"companion_miel","companion_paul","companion_taeon"})yield return Site(19,10,other);
            Check(Played.Count(x=>x.StartsWith("main19_"))==13,"Main19.Play13");yield return Continue("Main19Witness");
            yield return StoryBattle(18,6);yield return StoryBattle(19,4);
            yield return Load("Arbel");var catalogObject=new SerializedObject(Catalog);var entries=catalogObject.FindProperty("entries");int legacy=entries.arraySize-35;Check(legacy==255&&entries.arraySize==290,"Legacy.Prefix255.Total290");
            for(int i=0;i<legacy;i++){var e=entries.GetArrayElementAtIndex(i);string id=e.FindPropertyRelative("id").stringValue,speaker=e.FindPropertyRelative("speakerId").stringValue;var clip=e.FindPropertyRelative("clip").objectReferenceValue as AudioClip;Check(clip!=null&&Catalog.Find(id,speaker)==clip,"Legacy.Resolve."+id);Voice.Play(clip);yield return Wait(.01f);Check(Voice.Clip==clip&&Voice.IsPlaying,"Legacy.ShortPlayback."+id);Voice.Stop();}
            yield return Main17Important();NoResidual("LegacyEnd");Check(Errors.All(x=>x.Contains("UnityEditor.Search")),"Console.NoNewRuntimeError");File.WriteAllText(Path.Combine(Root,"status.txt"),Errors.Count==0?"PASS":"PARTIAL_KNOWN_SEARCH");
        }
        static IEnumerator Flatten(IEnumerator r){var s=new Stack<IEnumerator>();s.Push(r);while(s.Count>0){var t=s.Peek();if(!t.MoveNext()){s.Pop();continue;}if(t.Current is IEnumerator c){s.Push(c);continue;}yield return t.Current;}}
        static void Tick(){try{if(EditorApplication.timeSinceStartup>deadline)throw new TimeoutException("음성 QA 시간 초과");if(routine.MoveNext())return;EditorApplication.update-=Tick;SessionState.SetBool(Key,false);EditorApplication.Exit(0);}catch(Exception ex){File.WriteAllText(Path.Combine(Root,"status.txt"),"FAIL");File.WriteAllText(Path.Combine(Root,"failure.txt"),ex.ToString());Debug.LogException(ex);EditorApplication.Exit(1);}}
    }
}

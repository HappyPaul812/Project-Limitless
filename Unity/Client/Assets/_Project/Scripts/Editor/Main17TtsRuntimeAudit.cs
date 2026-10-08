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
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjectLimitless.EditorTools
{
    /// <summary>승인된 Main17 음성만 실제 조사 대화에서 검증합니다. 기존 비포커스 실행기의
    /// 저장·설정 격리를 재사용하고 사용자 슬롯·과거 QA·Main18 파일을 쓰지 않습니다.</summary>
    [InitializeOnLoad]
    public static class Main17TtsRuntimeAudit
    {
        private const BindingFlags Flags=BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Static|BindingFlags.Instance;
        private const string Armed="Limitless.Main17.TtsAudit";
        private static readonly List<string> results=new List<string>();
        private static readonly HashSet<string> played=new HashSet<string>();
        private static readonly HashSet<string> naturallyEnded=new HashSet<string>();
        private static float listenerVolume;
        private static bool boundaryOnly;
        public static string Status="IDLE";
        private static string Root=>Path.GetFullPath(Path.Combine(Application.dataPath,"../../../Temp/Main17Voice"));
        private static string Output=>Path.GetFullPath(Path.Combine(Application.dataPath,boundaryOnly?"../../../문서/00_프로젝트/Main17_TTS_Boundary_Results.txt":"../../../문서/00_프로젝트/Main17_TTS_Runtime_Results.txt"));
        static Main17TtsRuntimeAudit()
        {
            var initialized=Partial9FixedSpriteAudit.Status;
            EditorApplication.playModeStateChanged+=state=>
            {
                if(!SessionState.GetBool(Armed,false))return;
                if(state==PlayModeStateChange.EnteredPlayMode)
                    typeof(Partial9FixedSpriteAudit).GetField("routine",Flags).SetValue(null,Flatten(boundaryOnly?Boundaries():Run()));
                if(state==PlayModeStateChange.EnteredEditMode)
                {AudioListener.volume=listenerVolume;SessionState.SetBool(Armed,false);Write();}
            };
        }
        public static string Launch(bool boundaries=false)
        {
            boundaryOnly=boundaries;
            results.Clear();played.Clear();naturallyEnded.Clear();Status="RUNNING";
            listenerVolume=AudioListener.volume;SessionState.SetBool(Armed,true);Write();
            return Partial9FixedSpriteAudit.Launch();
        }
        private static void Write()=>File.WriteAllLines(Output,new[]{"STATUS|"+Status}.Concat(results));
        private static void Check(bool value,string id)
        {results.Add((value?"PASS|":"FAIL|")+id);Write();if(!value)throw new InvalidOperationException(id);}
        private static T Value<T>(object owner,string field)=>(T)owner.GetType().GetField(field,Flags).GetValue(owner);
        private static object Call(object owner,string method,params object[] args)=>owner.GetType().GetMethod(method,Flags).Invoke(owner,args);
        private static IEnumerator Flatten(IEnumerator first)
        {
            var stack=new Stack<IEnumerator>();stack.Push(first);
            while(stack.Count>0)
            {
                var top=stack.Peek();bool moved;
                try{moved=top.MoveNext();}catch(Exception error){Status="FAIL";results.Add("FAIL|Exception."+error);Write();throw;}
                if(!moved){stack.Pop();continue;}if(top.Current is IEnumerator nested)stack.Push(nested);else yield return top.Current;
            }
            Status="PASS";Write();
        }
        private static IEnumerator Wait(double seconds)
        {double until=EditorApplication.timeSinceStartup+seconds;while(EditorApplication.timeSinceStartup<until)yield return null;}
        private static VoicePlaybackSource Voice(DialoguePresenter d)=>Value<VoicePlaybackSource>(d,"voicePlayback");
        private static void NoResidual(string label)
        {
            Check(UnityEngine.Object.FindObjectsByType<VoicePlaybackSource>().All(x=>x.Clip==null&&!x.IsPlaying),"Voice.Cleared."+label);
        }
        private static IEnumerator Scene(string name)
        {
            double until=EditorApplication.timeSinceStartup+30;
            while(SceneManager.GetActiveScene().name!=name&&EditorApplication.timeSinceStartup<until)yield return null;
            Check(SceneManager.GetActiveScene().name==name,"Scene."+name);yield return Wait(.7);MonsterEncounterService.SuppressForSeconds(3600);
            Check(SceneManager.GetActiveScene().GetRootGameObjects().Sum(x=>x.GetComponentsInChildren<Transform>(true).Sum(t=>GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(t.gameObject)))==0,"MissingScript."+name);
            NoResidual("Scene."+name);
            if(name==Chapter2Main17Flow.Field||name=="Battle")
            {
                var bgm=Resources.Load<BgmSceneCatalog>("Audio/Music/BgmSceneCatalog");
                Check(BgmPlaybackService.Instance.CurrentClip==(name=="Battle"?bgm.FindBattle(Chapter2Main17Flow.Field,Chapter2Main17Flow.EncounterId,false):bgm.Find(name)),"BGM.Exact."+name);
            }
        }
        private static void Composition(params string[] ids)
        {
            var formation=new Dictionary<string,FormationRow>{{"player",FormationRow.Front}};
            foreach(string id in ids)formation[id]=FormationRow.Rear;
            Check(CompanionRosterService.TrySetComposition(ids,formation),"Fixture.Composition."+string.Join("+",ids));
        }
        private static void Seed(string path,bool hearing)
        {
            // 검증용 세션만 새로 만듭니다. 사용자 파티에는 이 편성을 저장하지 않습니다.
            typeof(SecondRegressionAudit).GetMethod("Seed",Flags).Invoke(null,new object[]{"sharpshooter",14});
            GameSessionData.SelectPlayerPath(path);
            QuestService.ImportSaveData(new QuestProgressSaveData{CompletedQuestIds=QuestCatalog.All.Where(x=>x.QuestId.StartsWith("main_")&&string.CompareOrdinal(x.QuestId,"main_17")<0).Select(x=>x.QuestId).ToArray()});
            CompanionRosterService.UnlockPaul("fighter");CompanionRosterService.UnlockSerin();
            Composition(CompanionRosterService.PaulId,hearing?CompanionRosterService.TaeonId:CompanionRosterService.MielId);
            GameSessionData.ActivateSafeZone("arbel","Arbel","Spawn_From_Field06");
        }
        private static IEnumerator Continue(string label)
        {
            string scene=SceneManager.GetActiveScene().name;var player=UnityEngine.Object.FindAnyObjectByType<PlayerController>();Vector2 at=player.transform.position;
            var snapshot=JsonUtility.ToJson(new Snapshot{Quest=QuestService.ExportSaveData(),Party=CompanionRosterService.ExportSaveData(),Beast=BeastCompanionService.ExportSaveData(),Resources=PartyResourceService.ExportSaveData()});
            Check(GameSaveService.SaveCurrentWorldPosition(at,scene),"Continue."+label+".Write");
            Check(GameSaveService.TryLoadSlot(1,out var saved)&&saved.Version==1,"Continue."+label+".Version1");
            Check(!JsonUtility.ToJson(saved).ToLowerInvariant().Contains("voice"),"Continue."+label+".NoPlaybackSave");
            SceneManager.LoadSceneAsync("Bootstrap");yield return Scene("Bootstrap");GameSessionData.Reset();
            var button=GameObject.Find("StartMenuCanvas/Slot01/Action")?.GetComponent<Button>();Check(button!=null,"Continue."+label+".RealButton");button.onClick.Invoke();yield return Scene(scene);
            Check(Vector2.Distance(UnityEngine.Object.FindAnyObjectByType<PlayerController>().transform.position,at)<.05f,"Continue."+label+".ScenePosition");
            Check(snapshot==JsonUtility.ToJson(new Snapshot{Quest=QuestService.ExportSaveData(),Party=CompanionRosterService.ExportSaveData(),Beast=BeastCompanionService.ExportSaveData(),Resources=PartyResourceService.ExportSaveData()}),"Continue."+label+".QuestPartyFormationBeastHPMP");
            Check(DialoguePresenter.Instance==null||!DialoguePresenter.Instance.IsOpen,"Continue."+label+".NoAutoDialogue");NoResidual("Continue."+label);
        }
        [Serializable]private sealed class Snapshot
        {public QuestProgressSaveData Quest;public CompanionRosterSaveData Party;public BeastCompanionSaveData Beast;public PartyMemberResourceSaveData[] Resources;}
        private static void Move(int index)
        {UnityEngine.Object.FindAnyObjectByType<PlayerController>().transform.position=Chapter2Main17Flow.Positions[index];Physics2D.SyncTransforms();}
        private static Main17Site Site(int index)=>GameObject.Find("field08_main17_"+Chapter2Main17Flow.Sites[index]).GetComponent<Main17Site>();
        private static IEnumerator Inspect(int index,bool hearing)
        {
            Move(index);yield return Wait(.1);
            string missing=index==3&&hearing?"main17_tracks_miel_01":index==11&&!hearing?"main17_withdraw_taeon_01":null;
            if(missing!=null)
            {
                Site(index).TryInteract();yield return Wait(.03);
                Check(!Value<DialogueLine[]>(DialoguePresenter.Instance,"sequenceLines").Any(x=>x.DialogueId==missing),"Conditional.Absent."+missing);
                DialoguePresenter.Instance.Hide();yield return Wait(.1);NoResidual("AbsentHide."+missing);
            }
            if(index==5&&!hearing)
            {
                Composition(CompanionRosterService.MielId);Site(index).TryInteract();yield return Wait(.03);
                Check(!Value<DialogueLine[]>(DialoguePresenter.Instance,"sequenceLines").Any(x=>x.DialogueId=="main17_compare_paul_01"),"Conditional.Absent.main17_compare_paul_01");
                DialoguePresenter.Instance.Hide();yield return Wait(.1);Composition(CompanionRosterService.PaulId,CompanionRosterService.MielId);
            }
            string party=JsonUtility.ToJson(CompanionRosterService.ExportSaveData());
            Site(index).TryInteract();yield return Wait(.03);
            Check(DialoguePresenter.Instance.IsOpen,"Dialogue.Open."+(hearing?"hearing":"default")+index);
            var expected=Main17DialogueCatalog.Get(index);var d=DialoguePresenter.Instance;
            Check(Value<DialogueLine[]>(d,"sequenceLines").Select(x=>x.DialogueId).SequenceEqual(expected.Select(x=>x.DialogueId)),"Branch.Exact."+(hearing?"hearing":"default")+index);
            for(int page=0;page<expected.Length;page++)
            {
                var line=expected[page];var voice=Voice(d);var source=Value<AudioSource>(voice,"source");
                Check(Value<int>(d,"sequencePageIndex")==page&&Value<Text>(d,"dialogueText").text==line.Message&&Value<Text>(d,"speakerText").text==line.SpeakerName,"Page.TextSpeaker."+line.DialogueId);
                Check(Chapter2Main17Flow.IsCurrent(index),"Quest.NoEarlyProgress."+line.DialogueId);
                Check(UnityEngine.Object.FindObjectsByType<VoicePlaybackSource>().Length==1,"Voice.SingleSource."+line.DialogueId);
                if(line.IsPlayer||line.IsDirection)
                    Check(voice.Clip==null&&!voice.IsPlaying,"Voice.IntentionalSilent."+line.DialogueId);
                else
                {
                    var clip=Resources.Load<VoiceClipCatalog>("Audio/Voice/Story/StoryVoiceCatalog").Find(line.DialogueId,line.SpeakerId);
                    Check(clip!=null&&voice.Clip==clip&&clip.name==line.DialogueId&&voice.IsPlaying,"Runtime.PlayExact."+line.DialogueId);played.Add(line.DialogueId);
                    Check(source.outputAudioMixerGroup.name=="Voice"&&source.volume==1&&source.pitch==1&&!source.loop&&source.spatialBlend==0,"Voice.UnprocessedRouting."+line.DialogueId);
                    if(naturallyEnded.Add(line.DialogueId))
                    {
                        // 실제 재생이 끝날 때까지 무입력으로 기다립니다. 시간이 지났다는 이유로 Next를 호출하지 않습니다.
                        double until=EditorApplication.timeSinceStartup+clip.length+3;
                        while(voice.IsPlaying&&EditorApplication.timeSinceStartup<until)
                        {if(Value<int>(d,"sequencePageIndex")!=page)throw new InvalidOperationException("자동 진행: "+line.DialogueId);yield return null;}
                        yield return Wait(.25);
                        Check(!voice.IsPlaying&&d.IsOpen&&Value<int>(d,"sequencePageIndex")==page&&Chapter2Main17Flow.IsCurrent(index),"NaturalEnd.NoAutoAdvance."+line.DialogueId);
                    }
                    if(line.DialogueId=="main17_compare_serin_01"&&!hearing)Volume(voice);
                }
                d.Advance();yield return Wait(.03);
                if(d!=null&&d.IsOpen)
                {
                    var next=expected[page+1];var nextClip=next.IsPlayer||next.IsDirection?null:Resources.Load<VoiceClipCatalog>("Audio/Voice/Story/StoryVoiceCatalog").Find(next.DialogueId,next.SpeakerId);
                    Check(Voice(d).Clip==nextClip,"Next.PreviousCleared."+line.DialogueId);
                }
            }
            NoResidual("Closed."+index+(hearing?"H":"D"));
            Check(JsonUtility.ToJson(CompanionRosterService.ExportSaveData())==party,"Party.NotForced."+index+(hearing?"H":"D"));
        }
        private static void Volume(VoicePlaybackSource voice)
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
            AudioSettingsService.Mixer.GetFloat("MasterVolume",out master);Check(Mathf.Abs(master)<.01f,"Volume.Restore");
        }
        private static IEnumerator Fight()
        {
            yield return Scene("Battle");var c=UnityEngine.Object.FindAnyObjectByType<BattleSceneController>();var enemies=Value<Formation>(c,"enemies");
            double until=EditorApplication.timeSinceStartup+120;Time.timeScale=8;int attacks=0;
            while(!Value<bool>(c,"battleEnded")&&EditorApplication.timeSinceStartup<until)
            {
                var button=Value<Button>(c,"attackButton");
                if(!Value<bool>(c,"actionPlaying")&&button.IsActive()&&button.interactable)
                {
                    button.onClick.Invoke();var targets=Value<IReadOnlyList<Combatant>>(c,"selectableTargets");
                    if(Value<bool>(c,"choosingTarget")&&targets.Count>0){typeof(SecondRegressionAudit).GetMethod("Hit",Flags).Invoke(null,new object[]{c,targets[0]});attacks++;}
                }
                yield return null;
            }
            Time.timeScale=1;Check(enemies.IsDefeated&&attacks>0,"Battle.NormalVictory");NoResidual("BattleVictory");
            UnityEngine.Object.FindObjectsByType<Button>().First(x=>x.name=="VictoryReturn").onClick.Invoke();yield return Scene(Chapter2Main17Flow.Field);
            Check(Chapter2Main17Flow.IsCurrent(9),"Battle.AfterObjective");
        }
        private static IEnumerator StaticClips()
        {
            var catalog=Resources.Load<VoiceClipCatalog>("Audio/Voice/Story/StoryVoiceCatalog");var serialized=new SerializedObject(catalog);var entries=serialized.FindProperty("entries");
            var current=new List<string>();var ids=new List<string>();
            for(int i=0;i<entries.arraySize;i++)
            {
                var e=entries.GetArrayElementAtIndex(i);var clip=e.FindPropertyRelative("clip").objectReferenceValue;
                Check(clip!=null,"Catalog.NonNull."+i);ids.Add(e.FindPropertyRelative("id").stringValue);
                current.Add(ids.Last()+"|"+e.FindPropertyRelative("speakerId").stringValue+"|"+AssetDatabase.GetAssetPath(clip));
            }
            Check(entries.arraySize==250&&ids.Distinct().Count()==ids.Count,"Catalog.250Unique");
            Check(current.Take(236).SequenceEqual(File.ReadAllLines(Path.Combine(Root,"catalog_before.txt"))),"Catalog.Existing236PrefixProtected");
            Check(!ids.Any(x=>x.StartsWith("main18_")),"Main18.NoCatalogAdded");
            foreach(var row in File.ReadAllLines(Path.Combine(Root,"apply.tsv")))
            {
                var fields=row.Split('\t');var clip=catalog.Find(fields[0],fields[1]);
                Check(clip!=null&&catalog.Find(fields[0],"player")==null,"Catalog.ExactSpeaker."+fields[0]);
                clip.LoadAudioData();double until=EditorApplication.timeSinceStartup+10;
                while(clip.loadState!=AudioDataLoadState.Loaded&&EditorApplication.timeSinceStartup<until)yield return null;
                var pcm=new float[clip.samples*clip.channels];Check(clip.GetData(pcm,0),"PCM.Decoded."+fields[0]);
                using(var file=new BinaryWriter(File.Open(Path.Combine(Root,fields[0]+".pcm-f32"),FileMode.Create)))foreach(float sample in pcm)file.Write(sample);
                Check(clip.frequency==24000&&clip.channels==1,"PCM.Format."+fields[0]);
            }
        }
        private static IEnumerator Run()
        {
            // 음성은 정상 AudioSource에서 시간대로 재생하되 자동 검증이 사용자 스피커에 대사를 반복 출력하지 않게 합니다.
            AudioListener.volume=0;UserSettingsService.SetMuteAll(false);UserSettingsService.SetAudioVolumes(100,UserSettingsService.SfxVolume,UserSettingsService.BgmVolume);
            yield return StaticClips();
            foreach(bool hearing in new[]{false,true})
            {
                Seed(hearing?"path.hearing":"path.vision",hearing);SceneTransitionService.Load(Chapter2Main17Flow.Field,"Spawn_From_Field07");yield return Scene(Chapter2Main17Flow.Field);
                Check(Chapter2Main17Flow.IsCurrent(1),"Quest.Start."+hearing);
                if(!hearing)yield return Continue("A-before-voice");
                for(int index=1;index<=6;index++){yield return Inspect(index,hearing);if(index==4&&!hearing)yield return Continue("B-middle-objective");}
                yield return Inspect(7,hearing);yield return Fight();if(!hearing)yield return Continue("C-story-victory");
                for(int index=9;index<=11;index++)yield return Inspect(index,hearing);
                Check(QuestService.GetState(Chapter2Main17Flow.QuestId)==QuestState.Completed,"Quest.Completed."+hearing);NoResidual("Main17Completed");
                if(!hearing)yield return Continue("D-main17-completed");
            }
            Check(played.Count==14&&naturallyEnded.Count==14,"Runtime.All14ActualPagesNaturalEnd");
            File.WriteAllLines(Path.Combine(Root,"played_ids.txt"),played.OrderBy(x=>x));
        }
        private static IEnumerator Boundaries()
        {
            // 전체 Story 경로를 통과한 뒤, 재생 중 중단 경계만 별도 격리 세션에서 확인합니다.
            AudioListener.volume=0;UserSettingsService.SetMuteAll(false);Seed("path.vision",false);
            SceneTransitionService.Load(Chapter2Main17Flow.Field,"Spawn_From_Field07");yield return Scene(Chapter2Main17Flow.Field);
            var d=DialoguePresenter.Instance;d.ShowSequence(Main17DialogueCatalog.Get(4),null);yield return Wait(.1);
            Check(Voice(d).IsPlaying,"Interrupt.BeforeNext.Playing");Volume(Voice(d));
            d.Advance();Check(Voice(d).Clip==null&&!Voice(d).IsPlaying,"Interrupt.NextToPlayer.Cleared");d.Hide();
            d.ShowSequence(Main17DialogueCatalog.Get(4),null);yield return Wait(.1);
            Check(Voice(d).IsPlaying,"Interrupt.BeforeScene.Playing");
            SceneTransitionService.Load(Chapter2Main17Flow.PreviousField,"Spawn_From_Field08");yield return Scene(Chapter2Main17Flow.PreviousField);
            SceneTransitionService.Load(Chapter2Main17Flow.Field,"Spawn_From_Field07");yield return Scene(Chapter2Main17Flow.Field);
            d=DialoguePresenter.Instance;d.ShowSequence(Main17DialogueCatalog.Get(4),null);yield return Wait(.1);
            Check(Voice(d).IsPlaying,"Interrupt.BeforeContinue.Playing");yield return Continue("E-during-playback");
            d=DialoguePresenter.Instance;d.ShowSequence(Main17DialogueCatalog.Get(4),null);yield return Wait(.1);
            Check(Voice(d).IsPlaying,"Interrupt.BeforeBattle.Playing");
            var spawn=Resources.Load<FieldMonsterSpawnDefinition>("StoryEncounterReturns/Main17_ThreatReturn");
            Check(BattleSceneFlow.EnterStoryBattle(Chapter2Main17Flow.EncounterId,spawn.Monster,spawn,spawn.Position),"Interrupt.EnterActualBattle");yield return Scene("Battle");
            var c=UnityEngine.Object.FindAnyObjectByType<BattleSceneController>();double until=EditorApplication.timeSinceStartup+20;
            while((Value<bool>(c,"actionPlaying")||!Value<Button>(c,"attackButton").interactable)&&EditorApplication.timeSinceStartup<until)yield return null;
            Check(!Value<bool>(c,"actionPlaying"),"Interrupt.BattleCommandReady");Call(c,"Flee");yield return Scene(Chapter2Main17Flow.Field);
            Check(Chapter2Main17Flow.IsCurrent(1),"Interrupt.NoQuestMutation");NoResidual("BoundaryFinal");
        }
    }
}

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
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjectLimitless.EditorTools
{
    /// <summary>태온 첫 조우5페이지와 교체한3개 음성만 격리 검사합니다. 실제 Save/Settings는 공용 QA가 복원합니다.</summary>
    [InitializeOnLoad]
    public static class Main03TaeonRegenAudit
    {
        const BindingFlags Private = BindingFlags.Instance | BindingFlags.NonPublic;
        const BindingFlags Static = BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;
        static bool armed, listening;
        static readonly List<string> results = new List<string>();
        static string Root => Path.GetFullPath(Path.Combine(Application.dataPath, "../../../Temp/Main03Regen20261005"));
        public static string Status = "Idle";
        static Main03TaeonRegenAudit()
        {
            EditorApplication.playModeStateChanged += state =>
            {
                if (!armed) return;
                if (state == PlayModeStateChange.EnteredPlayMode)
                    EditorApplication.delayCall += () => typeof(Partial9FixedSpriteAudit).GetField("routine", Static).SetValue(null, Flatten(Run()));
                if (state == PlayModeStateChange.EnteredEditMode) armed = false;
            };
        }
        /// <summary>まず無音の技術QA、ユーザーに告知した後に実際の3Clipだけ再生します。意味のPASSは人の確認が必要です。</summary>
        public static string Launch(bool audible = false)
        {
            results.Clear(); listening = audible; armed = true; Status = "Running";
            return Partial9FixedSpriteAudit.Launch();
        }
        static object Field(object owner, string name) => owner.GetType().GetField(name, Private).GetValue(owner);
        static void Check(bool value, string text)
        {
            results.Add((value ? "PASS " : "FAIL ") + text); Write();
            if (!value) { Status = "FAIL"; throw new InvalidOperationException(text); }
        }
        static void Write() => File.WriteAllText(Path.Combine(Root, listening ? "runtime-listening.txt" : "runtime-background.txt"), Status + "\n" + string.Join("\n", results));
        static IEnumerator Flatten(IEnumerator routine)
        {
            var stack = new Stack<IEnumerator>(); stack.Push(routine);
            while (stack.Count > 0)
            {
                var current = stack.Peek();
                if (!current.MoveNext()) { stack.Pop(); continue; }
                if (current.Current is IEnumerator) stack.Push((IEnumerator)current.Current); else yield return null;
            }
        }
        static IEnumerator Wait(int ticks) { for (int i=0; i<ticks; i++) yield return null; }
        static IEnumerator Load(string scene)
        {
            SceneManager.LoadSceneAsync(scene);
            for (int i=0; i<300 && SceneManager.GetActiveScene().name!=scene; i++) yield return null;
            Check(SceneManager.GetActiveScene().name==scene, "Scene " + scene); yield return Wait(12);
        }
        static VoicePlaybackSource Voice(DialoguePresenter dialogue) => (VoicePlaybackSource)Field(dialogue, "voicePlayback");
        static void Verify(DialoguePresenter dialogue, DialogueLine line)
        {
            var voice=Voice(dialogue);
            var clip=Resources.Load<VoiceClipCatalog>("Audio/Voice/Story/StoryVoiceCatalog").Find(line.DialogueId,line.SpeakerId);
            Check(voice.Clip==(line.IsPlayer ? null : clip), "Runtime Clip " + line.DialogueId);
            Check(((Text)Field(dialogue,"dialogueText")).text.StartsWith(line.SpeakerName+"\n"+line.Message+"\n"), "Subtitle/Speaker " + line.Message.Replace("\n", " / "));
            var root=(GameObject)Field(dialogue,"portraitRoot"); var image=(Image)Field(dialogue,"portraitImage");
            if (line.IsPlayer)
                Check(!voice.IsPlaying && voice.Clip==null && !root.activeSelf && image.sprite==null,"Player Voice0 Portrait0 / prior NPC cleanup");
            else
                Check(root.activeSelf && image.sprite==DialoguePortraitCatalog.GetPortrait(MainQuest03FieldFlow.TaeonId) && voice.IsPlaying,"Taeon Voice/Portrait restore " + line.DialogueId);
            Check(dialogue.GetComponentsInChildren<AudioSource>(true).Length==1,"Voice Source count1");
        }
        // WAV 원본은 그대로 두고 Unity가 실제로 재생하는 PCM을 추출해 원본 내용과 비교합니다.
        static void ExportPcm(AudioClip clip, string id)
        {
            Check(clip.LoadAudioData() && clip.loadState==AudioDataLoadState.Loaded,"Loaded " + id);
            var samples=new float[clip.samples*clip.channels]; Check(clip.GetData(samples,0),"GetData " + id);
            using (var writer=new BinaryWriter(File.Open(Path.Combine(Root,id+".pcm-f32"),FileMode.Create)))
                foreach(float sample in samples) writer.Write(sample);
        }
        static IEnumerator Run()
        {
            yield return Wait(20);
            GameSessionData.ConfigurePlayer(PlayerVisualType.Male,"음성 QA 플레이어");
            QuestDefinition definition;
            Check(QuestCatalog.TryGet(MainQuest03FieldFlow.QuestId,out definition),"Existing Main03 definition");
            QuestService.ImportSaveData(new QuestProgressSaveData {
                CompletedQuestIds=definition.PrerequisiteQuestIds.ToArray(),
                ActiveQuests=new[]{new ActiveQuestSaveData {QuestId=MainQuest03FieldFlow.QuestId,
                    Objectives=new[]{new QuestObjectiveProgressData {ObjectiveId="reach_taeon_meeting",CurrentCount=1}}}},
                TrackedQuestId=MainQuest03FieldFlow.QuestId });
            Check(QuestService.ActiveMainQuest?.CurrentObjective?.ObjectiveId=="talk_to_taeon_first","Seed only isolated first encounter");
            yield return Load("Field_01");
            var player=UnityEngine.Object.FindAnyObjectByType<PlayerController>();
            var actor=UnityEngine.Object.FindAnyObjectByType<MainQuest03TaeonActor>();
            Check(player!=null && actor!=null,"Actual Field01 Player/Taeon actor");
            player.transform.position=actor.transform.position+new Vector3(-1f,0f,0f);
            var dialogue=DialoguePresenter.Instance;
            var lines=(DialogueLine[])typeof(MainQuest03TaeonActor).GetMethod("FirstConversation",Static).Invoke(null,null);
            Check(lines.Length==5 && lines[1].IsPlayer && lines[3].IsPlayer,"Actual first factory5 / Player2");
            UserSettingsService.SetAudioVolumes(72,63,listening?22:47);UserSettingsService.SetMuteAll(!listening);
            yield return Wait(6);
            Check(actor.TryInteract(player.transform,3f) && dialogue.IsOpen,"Actual TryInteract opens first sequence");
            var bgm=UnityEngine.Object.FindAnyObjectByType<BgmPlaybackService>().GetComponent<AudioSource>();
            for(int i=0;i<lines.Length;i++)
            {
                Verify(dialogue,lines[i]);
                if(!lines[i].IsPlayer)
                {
                    var clip=Voice(dialogue).Clip;
                    Check(clip.frequency==24000 && clip.channels==1,"Runtime PCM24k mono " + lines[i].DialogueId);
                    ExportPcm(clip,lines[i].DialogueId);
                    if(listening)
                    {
                        Check(Voice(dialogue).IsAudible && bgm.clip!=null && bgm.isPlaying,"BGM and Voice simultaneous " + lines[i].DialogueId);
                        var start=EditorApplication.timeSinceStartup;
                        while(EditorApplication.timeSinceStartup-start<clip.length+.3)yield return null;
                        Check(!Voice(dialogue).IsPlaying,"Voice finished before Next " + lines[i].DialogueId);
                    }
                    else yield return Wait(2); // 재생 중 Next로 이전 음성이 즉시 정리되는지도 검사합니다.
                }
                else yield return Wait(listening?20:1);
                dialogue.Advance();
            }
            Check(!dialogue.IsOpen && Voice(dialogue).Clip==null && !Voice(dialogue).IsPlaying,"Sequence end cleanup");
            Check(QuestService.ActiveMainQuest.CurrentObjective.ObjectiveId=="win_taeon_encounter","Actual sequence completion event, no full Main03 play");
            if(!listening)
            {
                dialogue.ShowSequence(lines,null);dialogue.Advance();dialogue.Advance();Verify(dialogue,lines[2]);
                dialogue.Advance();Verify(dialogue,lines[3]);dialogue.Advance();Verify(dialogue,lines[4]);dialogue.Advance();
                Check(!dialogue.IsOpen && Voice(dialogue).Clip==null,"Rapid continuous Next cleanup");
                UserSettingsService.SetAudioVolumes(0,63,47);UserSettingsService.SetMuteAll(false);yield return Wait(6);
                dialogue.ShowSequence(lines,null);Check(Voice(dialogue).IsPlaying&&!Voice(dialogue).IsAudible,"Voice0 silent playback");
                UserSettingsService.SetAudioVolumes(72,63,47);yield return Wait(6);Check(Voice(dialogue).IsAudible,"Volume restores Voice");
                UserSettingsService.SetMuteAll(true);yield return Wait(6);Check(!Voice(dialogue).IsAudible,"Mute ON");
                UserSettingsService.SetMuteAll(false);yield return Wait(6);Check(Voice(dialogue).IsAudible&&bgm.isPlaying,"Mute OFF / independent BGM and Voice");
                UserSettingsService.SetMuteAll(true);dialogue.Hide();Check(Voice(dialogue).Clip==null,"Hide clears Voice");
            }
            var previousVoice=Voice(dialogue);yield return Load("Bootstrap");Check(previousVoice==null,"Scene transition destroys Voice");
            Status="PASS";results.Add("PASS count="+results.Count+"; semantic listening must be confirmed by human");Write();
        }
    }
}

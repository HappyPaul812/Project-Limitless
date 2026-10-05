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
using Object = UnityEngine.Object;

namespace ProjectLimitless.EditorTools
{
    /// <summary>격리 Save/Settings에서 작성 대사 전수와 실제 factory, 화자 전환, Audio/Portrait 정리를 검사합니다.</summary>
    [InitializeOnLoad]
    public static class StoryDialogueConsistencyAudit
    {
        [Serializable] public class Row
        {
            public int main;
            public string audit_key, raw_speaker_id, raw_name, raw_text, dialogue_id;
            public string resolved_speaker, spoken_text, catalog_audioclip;
        }
        [Serializable] public class Matrix { public Row[] rows; }
        const BindingFlags Private = BindingFlags.Instance | BindingFlags.NonPublic;
        const BindingFlags Static = BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;
        static bool armed;
        public static string Status = "Idle";
        static readonly List<string> checks = new List<string>();
        static string Output => Path.GetFullPath(Path.Combine(Application.dataPath, "../../../Temp/StoryConsistency20261005/runtime.txt"));
        static StoryDialogueConsistencyAudit()
        {
            EditorApplication.playModeStateChanged += state =>
            {
                if (!armed) return;
                if (state == PlayModeStateChange.EnteredPlayMode)
                    EditorApplication.delayCall += () => typeof(Partial9FixedSpriteAudit).GetField("routine", Static).SetValue(null, Flatten(Run()));
                if (state == PlayModeStateChange.EnteredEditMode) armed = false;
            };
        }
        /// <summary>기존 공용 QA의 PlayUnfocused/격리/종료 복원 절차를 재사용합니다. 정상 슬롯에는 기록하지 않습니다.</summary>
        public static string Launch()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) throw new InvalidOperationException("기존 Play를 먼저 종료하세요.");
            checks.Clear(); Status = "Running"; armed = true;
            return Partial9FixedSpriteAudit.Launch();
        }
        static object Field(object owner,string name) => owner.GetType().GetField(name,Private).GetValue(owner);
        static void Call(object owner,string name,params object[] args) => owner.GetType().GetMethod(name,Private).Invoke(owner,args);
        static void Check(bool value,string name)
        {
            checks.Add((value ? "PASS " : "FAIL ")+name);
            File.WriteAllText(Output,string.Join("\n",checks));
            if (!value) { Status = "FAIL"; throw new InvalidOperationException(name); }
        }
        static IEnumerator Flatten(IEnumerator initial)
        {
            var stack = new Stack<IEnumerator>(); stack.Push(initial);
            while(stack.Count>0)
            {
                var current=stack.Peek();
                if(!current.MoveNext()) { stack.Pop();continue; }
                if(current.Current is IEnumerator) stack.Push((IEnumerator)current.Current); else yield return null;
            }
        }
        static IEnumerator Wait(int ticks) { for(int i=0;i<ticks;i++) yield return null; }
        static IEnumerator Scene(string scene)
        {
            SceneManager.LoadSceneAsync(scene);
            for(int i=0;i<300 && SceneManager.GetActiveScene().name!=scene;i++)yield return null;
            Check(SceneManager.GetActiveScene().name==scene,"Scene "+scene); yield return Wait(12);
        }
        static VoicePlaybackSource Voice(DialoguePresenter d) => (VoicePlaybackSource)Field(d,"voicePlayback");
        static bool PortraitVisible(DialoguePresenter d) => ((GameObject)Field(d,"portraitRoot")).activeSelf;
        static Sprite Portrait(DialoguePresenter d) => ((Image)Field(d,"portraitImage")).sprite;
        static void Verify(DialoguePresenter d, DialogueLine line,string key)
        {
            var catalog=Resources.Load<VoiceClipCatalog>("Audio/Voice/Story/StoryVoiceCatalog");
            var expected=line.IsPlayer?null:catalog.Find(line.DialogueId,line.SpeakerId);
            Check(Voice(d).Clip==expected,"Clip "+key);
            var portrait=DialoguePortraitCatalog.GetPortrait(line.SpeakerId);
            Check(Portrait(d)==portrait && PortraitVisible(d)==(portrait!=null),"Portrait "+key);
            Check(((Text)Field(d,"dialogueText")).text==line.Message && ((Text)Field(d,"speakerText")).text==line.SpeakerName,"Runtime Subtitle "+key);
            if(line.IsPlayer)Check(Voice(d).Clip==null&&!Voice(d).IsPlaying&&!PortraitVisible(d)&&Portrait(d)==null,"Player Voice/Portrait/Cleanup NONE "+key);
        }
        static DialogueLine[] Factory(Type type,string method,params object[] args)
            => (DialogueLine[])type.GetMethod(method,Static).Invoke(null,args);
        static void VerifyFactory(DialogueLine[] lines,Row[] rows,string name)
        {
            foreach(var line in lines)
            {
                // 동일 문장이 재시도에 다시 쓰여도 작성 Source 행 중 하나와 화자/ID/본문이 모두 맞아야 합니다.
                Check(rows.Any(r=>r.spoken_text==line.Message&&r.resolved_speaker==line.SpeakerId&&r.dialogue_id==line.DialogueId),"Actual factory "+name+"/"+line.DialogueId+"/"+line.Message);
            }
        }
        static IEnumerator Run()
        {
            yield return Wait(20);
            var input=Path.GetFullPath(Path.Combine(Application.dataPath,"../../../문서/00_프로젝트/Story_Dialogue_Audit.json"));
            var rows=JsonUtility.FromJson<Matrix>(File.ReadAllText(input)).rows;
            GameSessionData.ConfigurePlayer(PlayerVisualType.Male,"감사 플레이어");
            UserSettingsService.SetMuteAll(true);
            var first=Factory(typeof(MainQuest03TaeonActor),"FirstConversation");
            Check(first.Length==5&&first[1].IsPlayer&&first[3].IsPlayer,"Taeon first encounter Player identity");
            Check(first[3].Message=="저도 조금 전에 이상한 흔적을 발견했습니다.\n마을 쪽으로 몰려온 흔적이었습니다.","Reported Player subtitle unchanged");
            VerifyFactory(first,rows,"Main03 First");
            VerifyFactory(Factory(typeof(MainQuest03TaeonActor),"AfterBattleConversation"),rows,"Main03 After");
            foreach(var name in new[]{"FirstConversation","EncounterConversation","AfterBattleConversation"})VerifyFactory(Factory(typeof(MainQuest04MielActor),name),rows,"Main04 "+name);
            foreach(var name in new[]{"GuardReport","RepresentativeReport"})VerifyFactory(Factory(typeof(MainQuest05ReturnFlow),name),rows,"Main05 "+name);
            for(int step=0;step<12;step++)
            {
                VerifyFactory(MainQuest09Dialogue.Lines(step),rows,"Main09 step"+step);
                VerifyFactory(MainQuest10Dialogue.Lines(step),rows,"Main10 step"+step);
                VerifyFactory(MainQuest11Dialogue.Lines(step),rows,"Main11 step"+step);
            }
            VerifyFactory(MainQuest11Dialogue.AfterBoss,rows,"Main11 AfterBoss");
            foreach(string scene in new[]{"start","ash","vibration","tracks","witness_ground","witness_emerge","retry","afterimage","canyon","report"})
                foreach(bool hearing in new[]{false,true})VerifyFactory(Main16DialogueCatalog.Get(scene,hearing),rows,"Main16 "+scene+"/"+hearing);
            var dialogue=new GameObject("StoryConsistencyDialogue").AddComponent<DialoguePresenter>();
            int player=0;
            var resolvedRows = new List<Row>(rows.Where(r => r.main == 0));
            foreach(var row in rows.Where(r=>r.main>0))
            {
                string name=row.raw_name=="<PlayerName>"?GameSessionData.PlayerName:row.raw_name;
                var line=new DialogueLine(row.raw_speaker_id,name,row.raw_text,row.dialogue_id);
                Check(line.SpeakerId==row.resolved_speaker,"Resolved Speaker "+row.audit_key);
                Check(line.Message==row.spoken_text,"Resolved Text "+row.audit_key);
                // Matrix는 raw Source와 실제 중앙 처리 결과를 구분합니다. 감사 키는 게임 ID가 아닙니다.
                resolvedRows.Add(new Row { main=row.main, audit_key=row.audit_key,
                    raw_speaker_id=line.SpeakerId, raw_name=line.SpeakerName, raw_text=line.Message,
                    dialogue_id=line.DialogueId });
                dialogue.ShowSequence(new[]{first[0],line},null);
                Check(Voice(dialogue).Clip!=null&&Voice(dialogue).IsPlaying,"Previous NPC Voice playing "+row.audit_key);
                dialogue.Advance(); Verify(dialogue,line,row.audit_key); if(line.IsPlayer)player++;
                yield return null;
                dialogue.Advance();Check(!dialogue.IsOpen&&Voice(dialogue).Clip==null&&!Voice(dialogue).IsPlaying,"Next end cleanup "+row.audit_key);
            }
            Check(player==42,"All 42 authored Player pages have Voice0 Portrait0");
            dialogue.ShowSequence(first,null); Verify(dialogue,first[0],"NPC before Player"); dialogue.Advance(); Verify(dialogue,first[1],"NPC to Player"); dialogue.Advance();Verify(dialogue,first[2],"Player to NPC restore");
            dialogue.ShowSequence(new[]{first[0],first[1],first[3],first[4]},null); dialogue.Advance();dialogue.Advance();Verify(dialogue,first[3],"Consecutive Player");dialogue.Advance();Verify(dialogue,first[4],"Continuous Next NPC restore");
            dialogue.Show("player","플레이어","단일 Player 대화");Check(Voice(dialogue).Clip==null&&!PortraitVisible(dialogue)&&Portrait(dialogue)==null,"Single Show Player cleanup");
            dialogue.ShowSequence(first,null); dialogue.ShowConfirmation("player","플레이어","확인 Player 대화","예","아니오",null);Check(Voice(dialogue).Clip==null&&!PortraitVisible(dialogue),"Confirmation Player cleanup");dialogue.Hide();
            // 실제 Asset을 바꾸지 않고 잘못된 Player Voice/Portrait 등록의 중앙 차단을 검사합니다.
            var catalog=Resources.Load<VoiceClipCatalog>("Audio/Voice/Story/StoryVoiceCatalog");
            var clone=Object.Instantiate(catalog);var entries=clone.GetType().GetField("entries",Private);var entryType=entries.FieldType.GetElementType();
            var entry=Activator.CreateInstance(entryType);entryType.GetField("id",Private).SetValue(entry,"qa_player_voice");entryType.GetField("speakerId",Private).SetValue(entry,"player");entryType.GetField("clip",Private).SetValue(entry,catalog.Find(first[0].DialogueId,first[0].SpeakerId));
            var array=Array.CreateInstance(entryType,1);array.SetValue(entry,0);entries.SetValue(clone,array);dialogue.GetType().GetField("voiceCatalog",Private).SetValue(dialogue,clone);
            Check(clone.Find("qa_player_voice","player")!=null,"Corrupt Player catalog fixture valid");
            dialogue.ShowSequence(new[]{new DialogueLine("player","태온","잘못 등록된 Voice 차단","qa_player_voice")},null);Check(Voice(dialogue).Clip==null&&!Voice(dialogue).IsPlaying&&!PortraitVisible(dialogue),"Player catalog Voice fallback blocked");
            dialogue.GetType().GetField("voiceCatalog",Private).SetValue(dialogue,catalog);Object.Destroy(clone);dialogue.Hide();
            var definitionsField=typeof(DialoguePortraitCatalog).GetField("definitions",Static);var definitions=(Dictionary<string,DialoguePortraitDefinition>)definitionsField.GetValue(null);
            var fake=ScriptableObject.CreateInstance<DialoguePortraitDefinition>();fake.ConfigureContent("player","Player",DialoguePortraitCatalog.GetPortrait("companion_taeon"));
            definitions.Add("player",fake);Check(DialoguePortraitCatalog.GetPortrait("player")==null,"Registered Player Portrait fallback blocked");definitions.Remove("player");Object.Destroy(fake);
            GameSessionData.ConfigurePlayer(PlayerVisualType.Male,"태온");Check(!new DialogueLine("companion_taeon","태온","NPC").IsPlayer,"Same Player/NPC display name preserves NPC stable ID");GameSessionData.ConfigurePlayer(PlayerVisualType.Male,"감사 플레이어");
            foreach(var sid in new[]{"companion_taeon","companion_miel","companion_paul","companion_serin"})Check(DialoguePortraitCatalog.GetPortrait(sid)!=null,"Named portrait "+sid);
            Check(DialoguePortraitCatalog.GetPortrait("arbel-leon")==null,"Leon unmade Portrait remains text-only");
            UserSettingsService.SetAudioVolumes(0,63,47);UserSettingsService.SetMuteAll(false);yield return Wait(6);
            dialogue.ShowSequence(first,null);Check(Voice(dialogue).IsPlaying&&!Voice(dialogue).IsAudible,"Voice0 retains manual Next without audible Voice");
            var mixer=AudioSettingsService.Mixer;float volume;mixer.GetFloat("VoiceVolume",out volume);Check(volume<=-79,"Voice0 Mixer unchanged rule");
            UserSettingsService.SetAudioVolumes(72,63,47);UserSettingsService.SetMuteAll(true);yield return Wait(6);mixer.GetFloat("MasterVolume",out volume);Check(volume<=-79&&UserSettingsService.VoiceVolume==72&&UserSettingsService.SfxVolume==63&&UserSettingsService.BgmVolume==47,"Mute preserves independent channel values");
            UserSettingsService.SetMuteAll(false);yield return Wait(6);Check(Voice(dialogue).IsAudible,"Unmute restores Voice policy");dialogue.Hide();
            dialogue.ShowSequence(first,null);dialogue.gameObject.SetActive(false);Check(Voice(dialogue).Clip==null&&!Voice(dialogue).IsPlaying,"Disable cleanup");dialogue.gameObject.SetActive(true);dialogue.ShowSequence(first,null);
            var previous=Voice(dialogue);yield return Scene("CharacterCreation");Check(previous==null,"Scene transition destroys previous Voice");
            OpeningIntroLaunchContext.BeginReplay();yield return Scene("OpeningIntro");var intro=Object.FindAnyObjectByType<OpeningIntroController>();var introVoice=(VoicePlaybackSource)Field(intro,"voicePlayback");
            var introCatalog=Resources.Load<VoiceClipCatalog>("Audio/Voice/Opening/OpeningNarrationCatalog");
            foreach(var row in rows.Where(r=>r.main==0))Check(introCatalog.Find(row.dialogue_id)!=null,"Intro ID/Clip "+row.dialogue_id);
            for(int i=0;i<8;i++)
            {
                int index=(int)Field(intro,"slideIndex");Call(intro,"RequestAdvance");yield return Wait(1);Check((int)Field(intro,"slideIndex")==index+1,"Intro continuous Next "+i);
                Check(introVoice.Clip==introCatalog.Find(OpeningIntroSequence.Slides[index+1].VoiceClipId),"Intro Next Clip "+i);
            }
            Call(intro,"Finish");yield return Wait(20);Check(SceneManager.GetActiveScene().name=="Bootstrap"&&introVoice==null,"Intro Skip Scene cleanup");
            File.WriteAllText(Path.Combine(Path.GetDirectoryName(Output), "resolved-runtime.json"),
                JsonUtility.ToJson(new Matrix { rows=resolvedRows.ToArray() }, true));
            Status="PASS";File.WriteAllText(Output,"PASS "+checks.Count+" checks\n"+string.Join("\n",checks));
        }
    }
}

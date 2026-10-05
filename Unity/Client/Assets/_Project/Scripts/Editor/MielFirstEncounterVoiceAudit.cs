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
    /// <summary>미엘 첫 조우의 모든 대사를 실제 Actor 경로로 연속 검사하며 원본 내용과 재생 중단을 구분합니다. 실제 사용자 저장과 포커스는 보존합니다.</summary>
    [InitializeOnLoad]
    public static class MielFirstEncounterVoiceAudit
    {
        const BindingFlags Private=BindingFlags.Instance|BindingFlags.NonPublic;
        const BindingFlags Static=BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
        static bool armed, audible;
        static List<string> checks=new List<string>();
        static List<string> metrics=new List<string>();
        static string Root => Path.GetFullPath(Path.Combine(Application.dataPath,"../../../Temp/Main04Final20261006/runtime"));
        public static string Status="Idle";
        static MielFirstEncounterVoiceAudit()
        {
            EditorApplication.playModeStateChanged+=state=>
            {
                if(!armed)return;
                if(state==PlayModeStateChange.EnteredPlayMode)
                    EditorApplication.delayCall+=()=>typeof(Partial9FixedSpriteAudit).GetField("routine",Static).SetValue(null,Flatten(Run()));
                if(state==PlayModeStateChange.EnteredEditMode)armed=false;
            };
        }
        public static string Launch(bool listening=false){ Directory.CreateDirectory(Root);audible=listening; checks.Clear();metrics.Clear();Status="Running";armed=true;return Partial9FixedSpriteAudit.Launch(); }
        static void Check(bool value,string label)
        {
            checks.Add((value?"PASS ":"FAIL ")+label);File.WriteAllText(Path.Combine(Root,audible?"runtime-listening.txt":"runtime-background.txt"),Status+"\n"+string.Join("\n",checks));
            if(!value){Status="FAIL";throw new InvalidOperationException(label);}
        }
        static object Field(object owner,string name)=>owner.GetType().GetField(name,Private).GetValue(owner);
        static VoicePlaybackSource Voice(DialoguePresenter d)=>(VoicePlaybackSource)Field(d,"voicePlayback");
        static IEnumerator Flatten(IEnumerator routine)
        {
            var stack=new Stack<IEnumerator>();stack.Push(routine);
            while(stack.Count>0){var r=stack.Peek();if(!r.MoveNext()){stack.Pop();continue;}if(r.Current is IEnumerator)stack.Push((IEnumerator)r.Current);else yield return null;}
        }
        static IEnumerator Wait(int n){for(int i=0;i<n;i++)yield return null;}
        static IEnumerator Load(string scene)
        {
            SceneManager.LoadSceneAsync(scene);for(int i=0;i<300&&SceneManager.GetActiveScene().name!=scene;i++)yield return null;
            Check(SceneManager.GetActiveScene().name==scene,"Scene "+scene);yield return Wait(12);
        }
        static Rect Bounds(RectTransform rect)
        {
            var corners=new Vector3[4];rect.GetWorldCorners(corners);return Rect.MinMaxRect(corners[0].x,corners[0].y,corners[2].x,corners[2].y);
        }
        static bool Inside(Rect outer,Rect inner)=>inner.xMin>=outer.xMin-.5f&&inner.yMin>=outer.yMin-.5f&&inner.xMax<=outer.xMax+.5f&&inner.yMax<=outer.yMax+.5f;
        static void Layout(DialoguePresenter d,string id)
        {
            var panel=(RectTransform)Field(d,"panelRect");var canvas=(RectTransform)Field(d,"canvasRect");
            var body=(Text)Field(d,"dialogueText");var speaker=(Text)Field(d,"speakerText");var footer=(Text)Field(d,"footerText");
            var portrait=(GameObject)Field(d,"portraitRoot");var viewport=(RectTransform)Field(d,"bodyViewport");var scroll=(ScrollRect)Field(d,"bodyScroll");var p=Bounds(panel);var b=Bounds(viewport);var s=Bounds(speaker.rectTransform);var f=Bounds(footer.rectTransform);
            float preferred=body.cachedTextGeneratorForLayout.GetPreferredHeight(body.text,body.GetGenerationSettings(new Vector2(body.rectTransform.rect.width,0)))/body.pixelsPerUnit;
            Check(Inside(new Rect(0,0,Screen.width,Screen.height),p),"Panel in screen "+id);
            Check(Inside(p,b)&&Inside(p,s)&&Inside(p,f),"Text regions inside panel "+id);
            Check(!b.Overlaps(s)&&!b.Overlaps(f)&&!s.Overlaps(f),"Speaker/Body/Footer separated "+id);
            Check(preferred<=body.rectTransform.rect.height+.5f,"No body truncation "+id);
            Check(panel.rect.height<=canvas.rect.height*.4f+.5f&&body.fontSize>=24&&body.fontSize<=28,"Height40% / font24-28 "+id);
            Check(body.horizontalOverflow==HorizontalWrapMode.Wrap,"Word Wrap "+id);
            if(scroll.enabled)
            {
                scroll.verticalNormalizedPosition=0f;Canvas.ForceUpdateCanvases();
                Check(Math.Abs(Bounds(body.rectTransform).yMin-Bounds(scroll.viewport).yMin)<1f,"Overflow body last line reachable "+id);
                scroll.verticalNormalizedPosition=1f;Canvas.ForceUpdateCanvases();
            }
            if(portrait.activeSelf)
            {
                var r=Bounds(portrait.GetComponent<RectTransform>());
                Check(Inside(p,r)&&!r.Overlaps(b)&&!r.Overlaps(s)&&!r.Overlaps(f),"Portrait separate "+id);
            }
            else Check(viewport.offsetMin.x==28f,"No blank Player Portrait space "+id);
            metrics.Add(string.Join(",",Screen.width,Screen.height,id,body.text.Length,body.cachedTextGenerator.lineCount,body.fontSize,preferred,body.rectTransform.rect.height,panel.rect.height));
            File.WriteAllText(Path.Combine(Root,"layout.csv"),"width,height,id,characters,lines,font,preferred_height,body_height,panel_height\n"+string.Join("\n",metrics));
        }

        /// <summary>실제 미엘 첫 만남을 격리 저장으로 시작합니다. 완료 뒤 다음 전투 단계와 저장 결과만 확인합니다.</summary>
        static IEnumerator Run()
        {
            yield return Wait(20);
            Check(!string.IsNullOrWhiteSpace(GameSaveService.AuditSaveDirectory),"Isolated Save active");
            Check(GameSaveService.SelectSlot(1),"Audit-only slot1");
            GameSessionData.ConfigurePlayer(PlayerVisualType.Male,"미엘 QA 플레이어");GameSessionData.SelectPlayerPath("path.vision");GameSessionData.SelectJob("mage");GameSessionData.RecordLocation("Field_01","from_village");
            UserSettingsService.SetAudioVolumes(72,63,22);UserSettingsService.SetMuteAll(false);
            QuestDefinition definition;Check(QuestCatalog.TryGet(MainQuest04FieldFlow.QuestId,out definition),"Main04 definition");
            QuestService.ImportSaveData(new QuestProgressSaveData {CompletedQuestIds=definition.PrerequisiteQuestIds.ToArray(),ActiveQuests=new[]{new ActiveQuestSaveData{QuestId=definition.QuestId,Objectives=new[]{new QuestObjectiveProgressData{ObjectiveId="reach_miel_meeting",CurrentCount=1}}}}});
            yield return Load("Field_01");
            var d=DialoguePresenter.Instance;var actor=UnityEngine.Object.FindAnyObjectByType<MainQuest04MielActor>();var player=UnityEngine.Object.FindAnyObjectByType<PlayerController>();
            Check(actor!=null&&player!=null,"Actual Miel first appearance");player.transform.position=actor.transform.position+new Vector3(-1,0,0);
            Check(actor.TryInteract(player.transform,3),"Actual Miel TryInteract");
            var lines=(DialogueLine[])typeof(MainQuest04MielActor).GetMethod("FirstConversation",Static).Invoke(null,null);
            Check(lines.Length==10,"Full first sequence10");
            for(int i=0;i<lines.Length;i++)
            {
                var line=lines[i];var voice=Voice(d);var clip=voice.Clip;double start=EditorApplication.timeSinceStartup;
                Check(((Text)Field(d,"dialogueText")).text==line.Message&&((Text)Field(d,"speakerText")).text==line.SpeakerName,"Subtitle/speaker "+i);
                Check((int)Field(d,"sequencePageIndex")==i,"Sequence order "+i);
                if(line.IsPlayer)Check(clip==null&&!voice.IsPlaying&&!((GameObject)Field(d,"portraitRoot")).activeSelf,"Player Voice0 Portrait0");
                else
                {
                    var resolved=Resources.Load<VoiceClipCatalog>("Audio/Voice/Story/StoryVoiceCatalog").Find(line.DialogueId,line.SpeakerId);
                    Check(clip!=null&&clip.name==line.DialogueId&&resolved==clip&&voice.IsPlaying&&voice.IsAudible,"Exact Resolve Source Playing "+line.DialogueId);
                    Check(((GameObject)Field(d,"portraitRoot")).activeSelf&&((Image)Field(d,"portraitImage")).sprite==DialoguePortraitCatalog.GetPortrait(line.SpeakerId),"Character Portrait "+line.DialogueId);
                    var pcm=new float[clip.samples*clip.channels];Check(clip.GetData(pcm,0),"GetData "+line.DialogueId);
                    using(var writer=new BinaryWriter(File.Open(Path.Combine(Root,line.DialogueId+".pcm-f32"),FileMode.Create)))foreach(float sample in pcm)writer.Write(sample);
                    double natural=-1;int lastSample=0;bool previous=true;
                    while(EditorApplication.timeSinceStartup-start<clip.length+.5)
                    {
                        // 자연 종료 전까지 같은 페이지와 Source를 유지하는지 매 tick 확인합니다. 첫 문장 종료로 Next를 호출하지 않습니다.
                        if((int)Field(d,"sequencePageIndex")!=i||voice.Clip!=clip||!d.IsOpen)throw new InvalidOperationException("AUTO_ADVANCE_OR_CLEANUP "+line.DialogueId);
                        var source=(AudioSource)typeof(VoicePlaybackSource).GetField("source",Private).GetValue(voice);
                        if(source.isPlaying)lastSample=Math.Max(lastSample,source.timeSamples);
                        if(previous&&!source.isPlaying&&natural<0)natural=EditorApplication.timeSinceStartup;
                        previous=source.isPlaying;yield return null;
                    }
                    Check(!voice.IsPlaying&&natural>=0&&natural-start>=clip.length-.2,"Full natural playback no cutoff "+line.DialogueId);
                    checks.Add("TRACE order="+(i+1)+" id="+line.DialogueId+" speaker="+line.SpeakerId+" resolved="+resolved.name+" source="+clip.name+" start="+start+" duration="+clip.length+" naturalStop="+natural+" lastTimeSample="+lastSample+" manualNextAvailable=immediate_skip_policy autoNext=false");
                }
                yield return Wait(3);Layout(d,"page"+(i+1));
                ScreenCapture.CaptureScreenshot(Path.Combine(Root,"page"+(i+1)+".png"));
                // 캡처가 현재 프레임을 저장할 때까지 기다려 페이지와 이미지의 순서를 맞춥니다.
                yield return Wait(3);
                checks.Add("NEXT order="+(i+1)+" at="+EditorApplication.timeSinceStartup+" origin=QA_manual_after_full_playback");d.Advance();
                Check(line.IsPlayer||Voice(d).Clip!=clip,"Previous Source cleaned/replaced "+i);
            }
            Check(!d.IsOpen&&Voice(d).Clip==null&&!Voice(d).IsPlaying&&!((GameObject)Field(d,"portraitRoot")).activeSelf,"Final Dialogue Voice Portrait cleanup");
            Check(QuestService.ActiveMainQuest.CurrentObjective.ObjectiveId=="win_three_people_encounter","First conversation complete to encounter objective");
            GameSaveData saved;Check(GameSaveService.TryLoadSlot(1,out saved),"Isolated automatic Save readable");
            Check(saved.QuestProgress.ActiveQuests.Any(q=>q.QuestId==MainQuest04FieldFlow.QuestId&&q.Objectives.Any(o=>o.ObjectiveId=="talk_to_miel_first"&&o.CurrentCount>=1)),"Saved first conversation progress");
            yield return Load("Bootstrap");Status="PASS";File.WriteAllText(Path.Combine(Root,"runtime-listening.txt"),Status+"\n"+string.Join("\n",checks));
        }
    }
}

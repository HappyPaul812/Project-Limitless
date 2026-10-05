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
    /// <summary>실제 첫 조우→전투와 장문 UI의 영역/줄바꿈을 격리 프로필에서 검사합니다. 실제 사용자 저장과 포커스는 보존합니다.</summary>
    [InitializeOnLoad]
    public static class Main03SuppRegenAudit
    {
        const BindingFlags Private=BindingFlags.Instance|BindingFlags.NonPublic;
        const BindingFlags Static=BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
        static bool armed, audible;
        static List<string> checks=new List<string>();
        static List<string> metrics=new List<string>();
        static string Root => Path.GetFullPath(Path.Combine(Application.dataPath,"../../../Temp/Main03SuppRegen20261005"));
        public static string Status="Idle";
        static Main03SuppRegenAudit()
        {
            EditorApplication.playModeStateChanged+=state=>
            {
                if(!armed)return;
                if(state==PlayModeStateChange.EnteredPlayMode)
                    EditorApplication.delayCall+=()=>typeof(Partial9FixedSpriteAudit).GetField("routine",Static).SetValue(null,Flatten(Run()));
                if(state==PlayModeStateChange.EnteredEditMode)armed=false;
            };
        }
        public static string Launch(bool listening=false){ audible=listening; checks.Clear();metrics.Clear();Status="Running";armed=true;return Partial9FixedSpriteAudit.Launch(); }
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
        /// <summary>격리 저장에서 실제 Actor→Battle과 해상도별 본문 영역을 검사합니다.</summary>
        static IEnumerator Run()
        {
            yield return Wait(20);
            GameSessionData.ConfigurePlayer(PlayerVisualType.Male,"UI QA 플레이어");GameSessionData.SelectPlayerPath("path.vision");GameSessionData.SelectJob("mage");
            UserSettingsService.SetAudioVolumes(72,63,22);UserSettingsService.SetMuteAll(!audible);
            QuestDefinition definition;Check(QuestCatalog.TryGet(MainQuest03FieldFlow.QuestId,out definition),"Main03 definition");
            QuestService.ImportSaveData(new QuestProgressSaveData {CompletedQuestIds=definition.PrerequisiteQuestIds.ToArray(),ActiveQuests=new[]{new ActiveQuestSaveData{QuestId=definition.QuestId,Objectives=new[]{new QuestObjectiveProgressData{ObjectiveId="reach_taeon_meeting",CurrentCount=1}}}}});
            yield return Load("Field_01");var d=DialoguePresenter.Instance;var actor=UnityEngine.Object.FindAnyObjectByType<MainQuest03TaeonActor>();var player=UnityEngine.Object.FindAnyObjectByType<PlayerController>();player.transform.position=actor.transform.position+new Vector3(-1,0,0);
            Check(actor.TryInteract(player.transform,3),"Actual first TryInteract");
            var first=(DialogueLine[])typeof(MainQuest03TaeonActor).GetMethod("FirstConversation",Static).Invoke(null,null);
            foreach(var line in first)
            {
                Check(((Text)Field(d,"dialogueText")).text==line.Message,"First body preserved "+line.DialogueId);
                if(line.IsPlayer)Check(Voice(d).Clip==null&&!Voice(d).IsPlaying&&!((GameObject)Field(d,"portraitRoot")).activeSelf,"Player Voice0 Portrait0");
                else Check(Voice(d).Clip!=null&&Voice(d).Clip.name==line.DialogueId&&Voice(d).IsPlaying,"005-007 unchanged playback "+line.DialogueId);
                yield return Wait(3);Layout(d,line.DialogueId+"_first");
                if(audible&&!line.IsPlayer)
                {
                    double firstStart=EditorApplication.timeSinceStartup;
                    while(EditorApplication.timeSinceStartup-firstStart<Voice(d).Clip.length+.3)yield return null;
                    Check(!Voice(d).IsPlaying,"005-007 full playback before Next "+line.DialogueId);
                }
                d.Advance();
            }
            Check(QuestService.ActiveMainQuest.CurrentObjective.ObjectiveId=="win_taeon_encounter","First completes to encounter");
            Check(actor.TryInteract(player.transform,3),"Actual prebattle TryInteract");
            for(int index=0;index<2;index++)
            {
                var clip=Voice(d).Clip;string expected="main03_taeon_supp_00"+(index+3);
                Check(clip!=null&&clip.name==expected&&Voice(d).IsPlaying,"Prebattle Clip loaded/playing "+expected);
                Check(((Text)Field(d,"speakerText")).text=="태온","Prebattle speaker Taeon");
                Check(((Text)Field(d,"dialogueText")).text==(index==0?"옵니다.":"제가 앞을 막겠습니다.\n뒤를 부탁드리겠습니다."),"Exact prebattle subtitle "+expected);
                Check(((Image)Field(d,"portraitImage")).sprite==DialoguePortraitCatalog.GetPortrait("companion_taeon")&&((GameObject)Field(d,"portraitRoot")).activeSelf,"Prebattle correct Taeon Portrait "+expected);
                if(audible)Check(Voice(d).IsAudible,"Voice audible "+expected);
                yield return Wait(3);Layout(d,expected);
                ScreenCapture.CaptureScreenshot(Path.Combine(Root,expected+(audible?"_listening":"_background")+".png"));
                var samples=new float[clip.samples*clip.channels];Check(clip.GetData(samples,0),"PCM "+expected);
                using(var writer=new BinaryWriter(File.Open(Path.Combine(Root,expected+".pcm-f32"),FileMode.Create)))
                    foreach(float sample in samples)writer.Write(sample);
                Check(clip.frequency==24000&&clip.channels==1,"24k mono "+expected);
                double sum=0,peak=0;foreach(float sample in samples){sum+=sample*sample;peak=Math.Max(peak,Math.Abs(sample));}
                Check(20*Math.Log10(Math.Sqrt(sum/samples.Length))>-30,"Normal source level "+expected);
                checks.Add("AUDIO "+expected+" length="+clip.length+" rmsDb="+(20*Math.Log10(Math.Sqrt(sum/samples.Length)))+" peakDb="+(20*Math.Log10(peak)));
                if(!audible&&index==1)
                {
                    Check(Voice(d).IsPlaying,"Explicit early Next skips playing004 under existing policy");
                    d.Advance();break;
                }
                double start=EditorApplication.timeSinceStartup;
                while(EditorApplication.timeSinceStartup-start<clip.length+.3)yield return null;
                Check(SceneManager.GetActiveScene().name=="Field_01"&&d.IsOpen&&!Voice(d).IsPlaying,"No automatic early Battle/Voice cut "+expected);
                d.Advance();
            }
            var old=d;for(int i=0;i<300&&SceneManager.GetActiveScene().name!="Battle";i++)yield return null;
            Check(SceneManager.GetActiveScene().name=="Battle","Actual final Next enters Battle");yield return Wait(12);
            Check(old==null&&UnityEngine.Object.FindAnyObjectByType<DialoguePresenter>()==null&&UnityEngine.Object.FindAnyObjectByType<VoicePlaybackSource>()==null,"Battle no old Dialogue/Voice/Footer/Portrait");
            yield return Load("Bootstrap");
            Status="PASS";checks.Add("PASS count="+checks.Count);File.WriteAllText(Path.Combine(Root,audible?"runtime-listening.txt":"runtime-background.txt"),Status+"\n"+string.Join("\n",checks));
        }
    }
}

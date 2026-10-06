using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using ProjectLimitless.Audio;
using ProjectLimitless.Battle;
using ProjectLimitless.Core;
using ProjectLimitless.Monster;
using ProjectLimitless.World;
namespace ProjectLimitless.EditorTools
{
    /// <summary>새 전투곡의 일반 조우·전환·단일 출력과 실제 HUD 캡처만 격리 검증합니다.</summary>
    [InitializeOnLoad]
    public static class Main17ArtAudioAudit
    {
        const string Key="Limitless.Main17.ArtAudio";
        static readonly BindingFlags Hidden=BindingFlags.Static|BindingFlags.NonPublic;
        public static readonly List<string> Results=new List<string>();
        static Main17ArtAudioAudit()
        {
            var initialized=Partial9FixedSpriteAudit.Status;
            EditorApplication.playModeStateChanged+=state=>
            {
                if(!SessionState.GetBool(Key,false)) return;
                if(state==PlayModeStateChange.EnteredPlayMode) typeof(Partial9FixedSpriteAudit).GetField("routine",Hidden).SetValue(null,Flatten(Run()));
                if(state==PlayModeStateChange.EnteredEditMode) {SessionState.SetBool(Key,false); File.WriteAllLines(Path.GetFullPath(Application.dataPath+"/../../../문서/00_프로젝트/Main17_ArtAudio_Runtime.txt"),Results);}
            };
        }
        public static string Launch(){Results.Clear(); SessionState.SetBool(Key,true); return Partial9FixedSpriteAudit.Launch();}
        static void Check(bool good,string id){Results.Add((good?"PASS|":"FAIL|")+id); if(!good) throw new InvalidOperationException(id);}
        static IEnumerator Flatten(IEnumerator first){var stack=new Stack<IEnumerator>();stack.Push(first);while(stack.Count>0){var top=stack.Peek();if(!top.MoveNext()){stack.Pop();continue;}if(top.Current is IEnumerator)stack.Push((IEnumerator)top.Current);else yield return top.Current;}}
        static IEnumerator Wait(double seconds){double end=EditorApplication.timeSinceStartup+seconds;while(EditorApplication.timeSinceStartup<end)yield return null;}
        static IEnumerator Scene(string name){double end=EditorApplication.timeSinceStartup+30;while(SceneManager.GetActiveScene().name!=name&&EditorApplication.timeSinceStartup<end)yield return null;Check(SceneManager.GetActiveScene().name==name,"Scene."+name);yield return Wait(2);MonsterEncounterService.SuppressForSeconds(3600);}
        static void Music(string expected,string id)
        {
            var service=BgmPlaybackService.Instance;var source=service.GetComponent<AudioSource>();
            Check(service.CurrentClip!=null&&service.CurrentClip.name==expected,id+".Clip");
            Check(source.isPlaying&&source.loop&&Mathf.Abs(source.volume-1)<.001f,id+".PlayingLoopGain");
            Check(UnityEngine.Object.FindObjectsByType<BgmPlaybackService>().Length==1&&service.GetComponents<AudioSource>().Length==1,id+".SingleSource");
            Check(source.outputAudioMixerGroup!=null&&source.outputAudioMixerGroup.name=="BGM",id+".Mixer");
        }
        static IEnumerator Run()
        {
            typeof(SecondRegressionAudit).GetMethod("Seed",Hidden).Invoke(null,new object[]{"fighter",20});
            QuestService.ImportSaveData(new QuestProgressSaveData{CompletedQuestIds=QuestCatalog.All.Where(x=>x.QuestId.StartsWith("main_")&&string.CompareOrdinal(x.QuestId,"main_17")<0).Select(x=>x.QuestId).ToArray()});
            foreach(string field in new[]{"Field_07_AshenReach","Field_08_RedRift"})
            {
                GameSessionData.RecordLocation(field,field=="Field_07_AshenReach"?"Spawn_From_Field06":"Spawn_From_Field07");
                SceneManager.LoadSceneAsync(field);yield return Scene(field);
                string fieldClip=field=="Field_07_AshenReach"?"Where_the_Earth_Breathes":"Beneath_The_Cracked_Earth";
                Music(fieldClip,field+".Explore");
                if(field=="Field_08_RedRift") Capture();
                var spawn=Resources.LoadAll<FieldMonsterSpawnDefinition>("MonsterSpawns").First(x=>x.SceneName==field&&!x.NonRespawningBoss);
                typeof(BattleSceneFlow).GetMethod("EnterBattle",Hidden).Invoke(null,new object[]{spawn.Monster,spawn});
                yield return Scene("Battle"); Music("Blade_and_Gambit",field+".GeneralBattle");
                Check(!BattleEncounterContext.IsStoryEncounter,field+".ActualGeneralContext");
                BattleSceneFlow.ReturnToField(false);
                double until=EditorApplication.timeSinceStartup+30;
                while(SceneManager.GetActiveScene().name!=field&&EditorApplication.timeSinceStartup<until)yield return null;
                yield return Wait(.2);
                var fading=BgmPlaybackService.Instance.GetComponent<AudioSource>();
                Check(fading.clip!=null&&fading.clip.name=="Blade_and_Gambit"&&fading.volume>0&&fading.volume<1,field+".FadeOutIntermediate");
                yield return Wait(.7);
                Check(fading.clip!=null&&fading.clip.name==fieldClip&&fading.volume>0&&fading.volume<1,field+".FieldFadeInIntermediate");
                yield return Wait(1.1);Music(fieldClip,field+".Return");
            }
        }
        static void Capture()
        {
            var camera=Camera.main;var canvases=UnityEngine.Object.FindObjectsByType<Canvas>().Where(x=>x.renderMode==RenderMode.ScreenSpaceOverlay).ToArray();
            foreach(var canvas in canvases){canvas.renderMode=RenderMode.ScreenSpaceCamera;canvas.worldCamera=camera;canvas.planeDistance=1;}
            var original=camera.targetTexture;var active=RenderTexture.active;
            try{foreach(int width in new[]{1920,1600,1280})
            {
                var rt=new RenderTexture(width,width*9/16,24);var image=new Texture2D(rt.width,rt.height,TextureFormat.RGB24,false);
                camera.targetTexture=rt;Canvas.ForceUpdateCanvases();camera.Render();RenderTexture.active=rt;
                image.ReadPixels(new Rect(0,0,rt.width,rt.height),0,0);image.Apply();
                File.WriteAllBytes(Path.GetFullPath(Application.dataPath+"/../../../Temp/Main17ArtAudio/HUD_Field08_"+width+".png"),image.EncodeToPNG());
                Check(true,"Capture.HUD."+width); camera.targetTexture=original;RenderTexture.active=active;UnityEngine.Object.DestroyImmediate(image);UnityEngine.Object.DestroyImmediate(rt);
            }}finally{camera.targetTexture=original;RenderTexture.active=active;foreach(var canvas in canvases){canvas.renderMode=RenderMode.ScreenSpaceOverlay;canvas.worldCamera=null;}}
        }
    }
}

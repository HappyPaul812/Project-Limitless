using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// 기존 설정을 다시 직렬화하지 않습니다. 이미 준비한 파일을 읽고 메모리의 QA 시작 장면만 엽니다.
[InitializeOnLoad]
public static class F2RegistrationBatch
{
    const string Key="F2Registration86Armed";static double deadline;
    static F2RegistrationBatch(){EditorApplication.playModeStateChanged+=s=>{if(s==PlayModeStateChange.EnteredPlayMode&&SessionState.GetBool(Key,false)){deadline=EditorApplication.timeSinceStartup+150;EditorApplication.update+=Poll;}};}
    public static void Run(){
        if(!Application.isBatchMode||Application.unityVersion!="6000.6.5f1"||!Application.dataPath.Replace('\\','/').EndsWith("/Temp/F2BuildSettingsFinal20261103/QA/Client/Assets"))throw new Exception("F2 등록 전용 격리 프로젝트만 허용");
        var scenes=EditorBuildSettings.scenes;if(scenes.Length!=26||scenes.Select(x=>x.path).Distinct().Count()!=26||!scenes[25].enabled||scenes[25].path!="Assets/_Project/Scenes/World_NorthF2_CANDIDATE.unity")throw new Exception("26번째 활성 F2 단일 등록 조건 불일치");
        Directory.CreateDirectory(F2RegistrationProbe.Output);File.WriteAllText(Path.Combine(F2RegistrationProbe.Output,"EditorPreflight.txt"),"Unity="+Application.unityVersion+"\nSceneCount="+scenes.Length+"\nF2Guid="+AssetDatabase.AssetPathToGUID(scenes[25].path));
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);new GameObject("SyntheticF2RegistrationProbe").AddComponent<F2RegistrationProbe>();SessionState.SetBool(Key,true);EditorApplication.isPlaying=true;
    }
    static void Poll(){var p=UnityEngine.Object.FindAnyObjectByType<F2RegistrationProbe>();if(p!=null&&p.Done){SessionState.SetBool(Key,false);EditorApplication.update-=Poll;int code=p.Failed?1:0;EditorApplication.isPlaying=false;EditorApplication.delayCall+=()=>EditorApplication.Exit(code);}else if(EditorApplication.timeSinceStartup>deadline){File.WriteAllText(Path.Combine(F2RegistrationProbe.Output,"timeout.txt"),"Timeout");EditorApplication.Exit(2);}}
}

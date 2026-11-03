using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// 사용자 원본이 아닌 새 통합 복제본만 실행합니다. Scene/BuildSettings/Packages를 저장하지 않습니다.
[InitializeOnLoad]
public static class LocalSync86Batch
{
    const string Key="LocalSync86Armed";static double deadline;
    static LocalSync86Batch(){EditorApplication.playModeStateChanged+=s=>{if(s==PlayModeStateChange.EnteredPlayMode&&SessionState.GetBool(Key,false)){deadline=EditorApplication.timeSinceStartup+180;EditorApplication.update+=Poll;}};}
    public static void Run(){
        if(!Application.isBatchMode||Application.unityVersion!="6000.6.5f1"||!Application.dataPath.Replace('\\','/').EndsWith("/Temp/LocalSync20261103/QA/Client/Assets"))throw new Exception("통합86 격리 복제본만 허용");
        Directory.CreateDirectory(LocalSync86Probe.Output);int loaded=0;
        foreach(var path in Directory.GetFiles("Assets/_Project/Art/ApprovedReferences/ForestSpiderOrganicV5","*.png")){var s=AssetDatabase.LoadAssetAtPath<Sprite>(path.Replace('\\','/'));if(s==null||Mathf.Abs(s.pixelsPerUnit-256)>.01f)throw new Exception("승인 참조 Sprite 등록 오류 "+path);loaded++;}
        if(loaded!=9)throw new Exception("승인 거미 원화/Walk8 참조 수량 오류");
        File.WriteAllText(Path.Combine(LocalSync86Probe.Output,"EditorPreflight.txt"),"Unity="+Application.unityVersion+"\nSpiderRegisteredSprites="+loaded+"\nF2BuildSettingsProposalOnly="+EditorBuildSettings.scenes.Any(x=>x.path.EndsWith("World_NorthF2_CANDIDATE.unity")));
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);new GameObject("Sync86SyntheticIntegrationProbe").AddComponent<LocalSync86Probe>();SessionState.SetBool(Key,true);EditorApplication.isPlaying=true;
    }
    static void Poll(){var p=UnityEngine.Object.FindAnyObjectByType<LocalSync86Probe>();if(p!=null&&p.Done){SessionState.SetBool(Key,false);EditorApplication.update-=Poll;int code=p.Failed?1:0;EditorApplication.isPlaying=false;EditorApplication.delayCall+=()=>EditorApplication.Exit(code);}else if(EditorApplication.timeSinceStartup>deadline){File.WriteAllText(Path.Combine(LocalSync86Probe.Output,"timeout.txt"),"Timeout");EditorApplication.Exit(2);}}
}

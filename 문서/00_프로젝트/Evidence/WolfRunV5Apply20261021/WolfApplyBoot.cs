using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// 배치 전용 scratch 프로젝트만 허용하여 사용자 원본 Editor의 Scene/Play 상태를 보호합니다.
[InitializeOnLoad] public static class WolfApplyBoot
{
    const string Key="WolfApplyQA20261021";
    static WolfApplyBoot(){EditorApplication.playModeStateChanged+=state=>{if(state==PlayModeStateChange.EnteredEditMode&&SessionState.GetBool(Key,false)){SessionState.SetBool(Key,false);EditorApplication.Exit(0);}};}
    public static void Start()
    {
        if(!Application.isBatchMode||!Application.dataPath.Replace('\\','/').Contains("/Temp/WolfRunV5Apply20261021/UnityQA/Assets"))throw new Exception("격리 경로만 허용합니다.");
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);
        new GameObject("WolfApplyQA").AddComponent<WolfApplyQA>();
        EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene(),"Assets/WolfApplyQA.unity");
        SessionState.SetBool(Key,true);EditorApplication.isPlaying=true;
    }
}

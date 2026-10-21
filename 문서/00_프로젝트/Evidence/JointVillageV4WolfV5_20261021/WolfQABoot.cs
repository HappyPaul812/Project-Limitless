using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// 배치 프로젝트 안에서만 실행하며 원본 Editor에 명령을 보내지 않습니다.
[InitializeOnLoad] public static class WolfQABoot
{
    static WolfQABoot(){EditorApplication.playModeStateChanged+=Changed;}
    public static void Start()
    {
        SessionState.SetBool("WolfQA_Active",true);
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);
        new GameObject("Wolf_Playback_QA").AddComponent<VillageBlockHarness>();
        EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene(),"Assets/WolfPlaybackQA.unity");
        EditorApplication.isPlaying=true;
    }
    static void Changed(PlayModeStateChange state)
    {
        if(state==PlayModeStateChange.EnteredEditMode&&SessionState.GetBool("WolfQA_Active",false))
        {SessionState.SetBool("WolfQA_Active",false);EditorApplication.Exit(0);}
    }
}

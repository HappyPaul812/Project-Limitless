using UnityEditor;
using UnityEditor.SceneManagement;

namespace ProjectLimitless.Editor
{
    /// <summary>저장 슬롯을 만들지 않고 OpeningIntro를 다시 보기 모드로 재생하는 개발용 진입점입니다.</summary>
    public static class OpeningIntroEditorTools
    {
        [MenuItem("Project Limitless/Test/Play Opening Intro")]
        private static void PlayOpeningIntro()
        {
            if (EditorApplication.isPlaying) return;
            EditorPrefs.SetBool("ProjectLimitless.PlayOpeningIntroAsReplay", true);
            EditorSceneManager.OpenScene("Assets/_Project/Scenes/OpeningIntro.unity", OpenSceneMode.Single);
            EditorApplication.isPlaying = true;
        }
    }
}

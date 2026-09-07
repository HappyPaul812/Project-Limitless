using System;
using System.Reflection;
using ProjectLimitless.Core;
using ProjectLimitless.Player;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectLimitless.EditorTools
{
    /// <summary>저장 Scene을 수정하지 않는 빈 Play Mode에서 이름표와 EXP HUD의 실제 생성·갱신을 검사합니다.</summary>
    [InitializeOnLoad]
    public static class WorldProgressUiAudit
    {
        private const string BatchKey = "ProjectLimitless.WorldProgressUiAudit";

        static WorldProgressUiAudit()
        {
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
        }

        public static void RunBatch()
        {
            try
            {
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
                SessionState.SetBool(BatchKey, true);
                EditorApplication.EnterPlaymode();
            }
            catch (Exception error)
            {
                Debug.LogException(error);
                EditorApplication.Exit(1);
            }
        }

        private static void OnPlayModeChanged(PlayModeStateChange state)
        {
            if (!SessionState.GetBool(BatchKey, false)) return;
            try
            {
                if (state == PlayModeStateChange.EnteredPlayMode)
                {
                    Audit();
                    Debug.Log("WORLD_PROGRESS_UI_AUDIT Play Mode PASS");
                    EditorApplication.ExitPlaymode();
                }
                else if (state == PlayModeStateChange.EnteredEditMode)
                {
                    SessionState.EraseBool(BatchKey);
                    Debug.Log("WORLD_PROGRESS_UI_AUDIT ALL PASS");
                    EditorApplication.Exit(0);
                }
            }
            catch (Exception error)
            {
                SessionState.EraseBool(BatchKey);
                Debug.LogException(error);
                EditorApplication.Exit(1);
            }
        }

        private static void Audit()
        {
            GameSessionData.ConfigurePlayer(PlayerVisualType.Male, "마도바울이");
            GameSessionData.ConfigureProgress(1, 0);
            GameObject player = new GameObject("WorldProgressAuditPlayer");
            PlayerNameplate nameplate = player.AddComponent<PlayerNameplate>();

            GameObject canvasObject = GameObject.Find("PlayerNameOverlayCanvas");
            Check(canvasObject != null && canvasObject.GetComponent<Canvas>().renderMode == RenderMode.ScreenSpaceOverlay,
                "Screen Space Overlay Canvas 생성");
            Check(canvasObject.transform.Find("NameText").GetComponent<Text>().text == "Lv.1 마도바울이",
                "Lv1 이름표 초기화");
            Check(canvasObject.transform.Find("WorldExperienceHud/Identity").GetComponent<Text>().text == "Lv.1  마도바울이",
                "Lv1 HUD 이름");
            Check(canvasObject.transform.Find("WorldExperienceHud/Experience").GetComponent<Text>().text == "EXP 0 / 100",
                "Lv1 EXP 숫자");
            Check(WorldExperienceHud.CalculateProgress(3, 85) == .5f, "Lv3 EXP Bar 50%");

            GameSessionData.ConfigureProgress(3, 20);
            typeof(PlayerNameplate).GetMethod("LateUpdate", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(nameplate, null);
            WorldExperienceHud hud = canvasObject.transform.Find("WorldExperienceHud").GetComponent<WorldExperienceHud>();
            hud.Refresh();
            Check(canvasObject.transform.Find("NameText").GetComponent<Text>().text == "Lv.3 마도바울이", "레벨업 이름표 즉시 갱신");
            Check(canvasObject.transform.Find("WorldExperienceHud/Experience").GetComponent<Text>().text == "EXP 20 / 170",
                "초과 EXP 이월 HUD 갱신");

            GameSessionData.ConfigureProgress(CharacterGrowthCalculator.MaxLevel, 123);
            hud.Refresh();
            Check(canvasObject.transform.Find("WorldExperienceHud/Experience").GetComponent<Text>().text == "MAX LEVEL",
                "만렙 분모 숨김");
            Check(canvasObject.transform.Find("WorldExperienceHud/BarBackground/BarFill").GetComponent<Image>().fillAmount == 1f,
                "만렙 Bar 가득 참");
            Check(canvasObject.transform.childCount == 2, "이름표와 HUD 중복 없음");

            UnityEngine.Object.DestroyImmediate(player);
            // PlayerNameplate의 실제 OnDestroy는 Canvas를 지연 파괴합니다. Scene 전환 프레임 안에서는
            // Unity가 이를 정리하므로, 감사 종료 시 남은 예약 대상만 즉시 치워 빈 Scene을 유지합니다.
            if (canvasObject != null) UnityEngine.Object.DestroyImmediate(canvasObject);
        }

        private static void Check(bool condition, string message)
        {
            if (!condition) throw new InvalidOperationException("WORLD_PROGRESS_UI_AUDIT 실패: " + message);
        }
    }
}

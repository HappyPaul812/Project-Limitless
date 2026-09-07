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
            WorldExperienceHud hud = canvasObject.transform.Find("WorldExperienceHud").GetComponent<WorldExperienceHud>();
            Image fill = canvasObject.transform.Find("WorldExperienceHud/BarBackground/BarFill").GetComponent<Image>();
            CheckFill(hud, fill, 1, 0, 0f);
            CheckFill(hud, fill, 1, 24, .24f);
            CheckFill(hud, fill, 1, 50, .5f);
            CheckFill(hud, fill, 1, 75, .75f);
            CheckFill(hud, fill, 1, 99, .99f);
            CheckFill(hud, fill, 3, 85, .5f);
            ExperienceGain levelUp = ExperienceProgression.Add(1, 90, 30);
            Check(levelUp.Level == 2 && levelUp.CurrentExperience == 20, "90/100 + 30의 Lv2 EXP 20 이월");
            CheckFill(hud, fill, levelUp.Level, levelUp.CurrentExperience, 20f / 130f);

            GameSessionData.ConfigureProgress(3, 20);
            typeof(PlayerNameplate).GetMethod("LateUpdate", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(nameplate, null);
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

        /// <summary>숫자 속성뿐 아니라 CanvasRenderer가 만든 실제 Fill 메시 폭도 요청 비율인지 확인합니다.</summary>
        private static void CheckFill(WorldExperienceHud hud, Image fill, int level, int experience, float expected)
        {
            GameSessionData.ConfigureProgress(level, experience);
            hud.Refresh();
            Canvas.ForceUpdateCanvases();
            Check(Mathf.Abs(fill.fillAmount - expected) < .0001f, $"Lv{level} EXP {experience} fillAmount {expected:P0}");
            Check(fill.sprite != null && fill.type == Image.Type.Filled
                && fill.fillMethod == Image.FillMethod.Horizontal
                && fill.fillOrigin == (int)Image.OriginHorizontal.Left, "왼쪽 시작 Filled Image 구성");

            if (expected <= 0f) return;
            Mesh mesh = fill.canvasRenderer.GetMesh();
            float visualRatio = mesh.bounds.size.x / fill.rectTransform.rect.width;
            Check(Mathf.Abs(visualRatio - expected) < .01f,
                $"Lv{level} EXP {experience} 실제 메시 폭 {visualRatio:P1}, 예상 {expected:P1}");
        }

        private static void Check(bool condition, string message)
        {
            if (!condition) throw new InvalidOperationException("WORLD_PROGRESS_UI_AUDIT 실패: " + message);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ProjectLimitless.Core;
using ProjectLimitless.Player;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace ProjectLimitless.EditorTools
{
    /// <summary>사용자가 승인한 Foreground 검수를 위한 수동 단계 도구입니다. Scene과 원본 Asset을 저장하지 않습니다.</summary>
    [InitializeOnLoad]
    public static class ForegroundReviewQA
    {
        private const BindingFlags Private = BindingFlags.Instance | BindingFlags.NonPublic;
        private const string Key = "Limitless.ForegroundQA.";
        private static GameObject overlay;
        private static readonly List<Action<int>> animations = new List<Action<int>>();
        public static string DirectoryPath => Path.GetFullPath(Path.Combine(Application.dataPath, "../Temp/ForegroundReviewQA"));
        static ForegroundReviewQA()
        {
            EditorApplication.update += () => { if (EditorApplication.isPlaying) foreach (var action in animations) action((int)(Time.unscaledTime * 6) % 4); };
            EditorApplication.playModeStateChanged += state =>
            {
                if (state != PlayModeStateChange.EnteredEditMode || !SessionState.GetBool(Key + "Armed", false)) return;
                animations.Clear(); overlay = null;
                typeof(ProjectLimitless.Audio.AudioSettingsService).GetMethod("ResetRuntime", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, null);
                GameSaveService.FinishAudit(); UserSettingsService.FinishAudit(); GameSessionData.Reset();
                EditorSettings.enterPlayModeOptionsEnabled = SessionState.GetBool(Key + "Enabled", false);
                EditorSettings.enterPlayModeOptions = (EnterPlayModeOptions)SessionState.GetInt(Key + "Options", 0);
                Application.runInBackground = SessionState.GetBool(Key + "Background", false);
                SessionState.SetBool(Key + "Armed", false);
            };
        }
        public static string Launch()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode || EditorSceneManager.GetActiveScene().isDirty || SceneManager.GetActiveScene().name != "Bootstrap")
                throw new InvalidOperationException("clean Bootstrap Edit Mode 필요");
            Directory.CreateDirectory(DirectoryPath);
            SessionState.SetBool(Key + "Enabled", EditorSettings.enterPlayModeOptionsEnabled);
            SessionState.SetInt(Key + "Options", (int)EditorSettings.enterPlayModeOptions);
            SessionState.SetBool(Key + "Background", Application.runInBackground);
            SessionState.SetBool(Key + "Armed", true);
            EditorSettings.enterPlayModeOptionsEnabled = true; EditorSettings.enterPlayModeOptions = EnterPlayModeOptions.DisableDomainReload;
            GameSaveService.AuditSaveDirectory = Path.Combine(DirectoryPath, Guid.NewGuid().ToString("N"));
            UserSettingsService.BeginAudit(GameSaveService.AuditSaveDirectory);
            Directory.CreateDirectory(GameSaveService.AuditSaveDirectory);
            File.WriteAllText(UserSettingsService.SettingsFilePath, JsonUtility.ToJson(new UserSettingsData { MuteAll = false, SkipOpeningIntro = true }));
            Application.runInBackground = true; EditorApplication.EnterPlaymode(); return "Foreground QA isolated Save/Settings";
        }
        public static void HideSheets()
        { animations.Clear(); if (overlay != null) Object.Destroy(overlay); overlay = null; }

        // 보류 외형도 원본 Sprite 참조로만 보여 줍니다. ready 정책이나 실제 선택 목록은 건드리지 않습니다.
        public static string Sheets(string theme, string gender)
        {
            HideSheets();
            overlay = new GameObject("ForegroundAppearanceReview", typeof(Canvas), typeof(UnityEngine.UI.CanvasScaler));
            overlay.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            overlay.GetComponent<Canvas>().sortingOrder = 30000;
            var scaler = overlay.GetComponent<UnityEngine.UI.CanvasScaler>();
            scaler.uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1600, 850); scaler.matchWidthOrHeight = .5f;
            var background = Rect("Background", overlay.transform, 0, 0, 1600, 850);
            background.gameObject.AddComponent<UnityEngine.UI.Image>().color = new Color(.13f, .17f, .21f);
            Label(overlay.transform, "16 frames: Down 0-3 / Left 4-7 / Right 8-11 / Up 12-15. Animated D/L/R/U at right.", 12, 8, 1580, 28);
            var entries = PlayerAppearanceCatalog.Load().Entries.Where(e => e.theme == theme && e.gender == gender).OrderBy(e => e.jobTheme).ToArray();
            for (int row = 0; row < entries.Length; row++)
            {
                var entry = entries[row]; float y = 44 + row * 159;
                Label(overlay.transform, entry.appearanceId + (entry.readyForSelection ? " READY" : " REVIEW"), 12, y, 1580, 24);
                for (int frame = 0; frame < 16; frame++)
                {
                    float x = 12 + frame * 76;
                    var cell = Rect("Frame" + frame, overlay.transform, x, y + 25, 74, 110);
                    cell.gameObject.AddComponent<UnityEngine.UI.Image>().color = (frame / 4) % 2 == 0 ? new Color(.3f, .33f, .37f) : new Color(.4f, .43f, .47f);
                    var img = Rect("Sprite", cell, 0, 0, 74, 96).gameObject.AddComponent<UnityEngine.UI.Image>();
                    img.sprite = entry.frames[frame]; img.preserveAspect = true;
                    Label(cell, frame.ToString(), 0, 94, 74, 16);
                }
                for (int d = 0; d < 4; d++)
                {
                    var img = Rect("Walk", overlay.transform, 1234 + d * 88, y + 25, 86, 110).gameObject.AddComponent<UnityEngine.UI.Image>();
                    img.preserveAspect = true; int direction = d;
                    animations.Add(frame => { if (img != null) img.sprite = entry.frames[direction * 4 + frame]; });
                }
            }
            return string.Join("\n", entries.Select(e => e.appearanceId));
        }
        private static RectTransform Rect(string name, Transform parent, float x, float y, float width, float height)
        {
            var rect = new GameObject(name, typeof(RectTransform)).GetComponent<RectTransform>(); rect.SetParent(parent, false);
            rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(0, 1);
            rect.anchoredPosition = new Vector2(x, -y); rect.sizeDelta = new Vector2(width, height); return rect;
        }
        private static void Label(Transform parent, string value, float x, float y, float width, float height)
        {
            var text = Rect("Label", parent, x, y, width, height).gameObject.AddComponent<UnityEngine.UI.Text>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"); text.fontSize = 17; text.color = Color.white; text.text = value;
        }
        public static string Preview(string id)
        {
            var creation = Object.FindAnyObjectByType<ProjectLimitless.UI.CharacterCreationController>();
            var entry = PlayerAppearanceCatalog.Load().Find(id);
            if (creation == null || !entry.IsUsable) throw new InvalidOperationException("선택 가능한 실제 Creation Preview 필요");
            creation.GetType().GetMethod("SelectVisual", Private).Invoke(creation, new object[] { Enum.Parse(typeof(PlayerVisualType), entry.gender) });
            creation.GetType().GetField("selectedTheme", Private).SetValue(creation, "");
            creation.GetType().GetMethod("RefreshAppearances", Private).Invoke(creation, null);
            creation.GetType().GetField("selectedAppearanceId", Private).SetValue(creation, id);
            creation.GetType().GetMethod("RefreshAppearancePreview", Private).Invoke(creation, null);
            return GameObject.Find("AppearanceLabel").GetComponent<UnityEngine.UI.Text>().text;
        }
    }
}

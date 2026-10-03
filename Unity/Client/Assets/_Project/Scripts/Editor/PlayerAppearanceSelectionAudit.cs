using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ProjectLimitless.Battle;
using ProjectLimitless.Core;
using ProjectLimitless.Player;
using ProjectLimitless.UI;
using ProjectLimitless.World;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace ProjectLimitless.EditorTools
{
    /// <summary>실제 생성 Scene·공용 Animator·격리 Save·Bootstrap Continue를 백그라운드에서 검증합니다.</summary>
    [InitializeOnLoad]
    public static class PlayerAppearanceSelectionAudit
    {
        public static string Status { get; private set; } = "Idle";
        public static readonly List<string> Results = new List<string>();
        private static IEnumerator routine;
        private static double nextTick;
        private static Object[] originalWindows;
        private const BindingFlags Private = BindingFlags.Instance | BindingFlags.NonPublic;
        private const string Key = "Limitless.AppearanceAudit.";
        private static string Root => Path.GetFullPath(Path.Combine(Application.dataPath, "../Temp/AppearanceSelectionAudit"));
        static PlayerAppearanceSelectionAudit()
        {
            EditorApplication.update += Tick;
            EditorApplication.playModeStateChanged += OnPlayState;
        }

        /// <summary>기존 GameView의 진입 동작만 PlayUnfocused로 임시 설정합니다. 창 생성·활성화·OS 입력은 하지 않습니다.</summary>
        public static string Launch()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isCompiling ||
                EditorSceneManager.GetActiveScene().isDirty || EditorSceneManager.GetActiveScene().name != "Bootstrap")
                throw new InvalidOperationException("clean Bootstrap Edit Mode에서 실행하세요.");
            Directory.CreateDirectory(Root);
            var type = typeof(EditorWindow).Assembly.GetType("UnityEditor.GameView");
            var property = type.GetProperty("enterPlayModeBehavior", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            var windows = Resources.FindObjectsOfTypeAll(type);
            originalWindows = windows;
            SessionState.SetInt(Key + "WindowCount", windows.Length);
            for (int i = 0; i < windows.Length; i++)
            {
                SessionState.SetString(Key + "Behavior" + i, property.GetValue(windows[i]).ToString());
                property.SetValue(windows[i], Enum.Parse(property.PropertyType, "PlayUnfocused"));
            }
            SessionState.SetBool(Key + "OptionsEnabled", EditorSettings.enterPlayModeOptionsEnabled);
            SessionState.SetInt(Key + "Options", (int)EditorSettings.enterPlayModeOptions);
            SessionState.SetBool(Key + "Background", Application.runInBackground);
            SessionState.SetBool(Key + "Armed", true);
            // domain reload를 잠시 생략하여 첫 Bootstrap.Start 이전부터 격리 Save/Settings를 보장합니다.
            EditorSettings.enterPlayModeOptionsEnabled = true;
            EditorSettings.enterPlayModeOptions = EnterPlayModeOptions.DisableDomainReload;
            GameSaveService.AuditSaveDirectory = Path.Combine(Root, Guid.NewGuid().ToString("N"));
            UserSettingsService.BeginAudit(GameSaveService.AuditSaveDirectory);
            // Edit Mode에서 Mixer 설정 이벤트를 호출하지 않고 격리 설정 파일만 준비합니다.
            Directory.CreateDirectory(GameSaveService.AuditSaveDirectory);
            File.WriteAllText(UserSettingsService.SettingsFilePath, JsonUtility.ToJson(new UserSettingsData { MuteAll = true, SkipOpeningIntro = true }));
            Application.runInBackground = true;
            EditorApplication.EnterPlaymode();
            return "Launched PlayUnfocused with isolated Save/Settings";
        }

        private static void OnPlayState(PlayModeStateChange state)
        {
            if (!SessionState.GetBool(Key + "Armed", false)) return;
            if (state == PlayModeStateChange.EnteredPlayMode)
            {
                Results.Clear(); Status = "Running"; routine = Run();
            }
            if (state != PlayModeStateChange.EnteredEditMode) return;
            routine = null;
            // domain reload를 생략했으므로 Audio의 런타임 구독도 정리합니다. 종료된 Mixer에 Reload 이벤트가 전달되면
            // 외형과 무관한 Editor 종료 시점의 SetFloat 오류가 생길 수 있어 정상 초기화 경로를 먼저 호출합니다.
            typeof(ProjectLimitless.Audio.AudioSettingsService).GetMethod("ResetRuntime", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, null);
            GameSaveService.FinishAudit(); UserSettingsService.FinishAudit(); GameSessionData.Reset();
            EditorSettings.enterPlayModeOptionsEnabled = SessionState.GetBool(Key + "OptionsEnabled", false);
            EditorSettings.enterPlayModeOptions = (EnterPlayModeOptions)SessionState.GetInt(Key + "Options", 0);
            Application.runInBackground = SessionState.GetBool(Key + "Background", false);
            var type = typeof(EditorWindow).Assembly.GetType("UnityEditor.GameView");
            var property = type.GetProperty("enterPlayModeBehavior", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            for (int i = 0; i < SessionState.GetInt(Key + "WindowCount", 0); i++)
            {
                var window = originalWindows != null && i < originalWindows.Length ? originalWindows[i] : null;
                if (window != null) property.SetValue(window, Enum.Parse(property.PropertyType, SessionState.GetString(Key + "Behavior" + i, "PlayFocused")));
            }
            SessionState.SetBool(Key + "Armed", false);
        }

        private static void Tick()
        {
            if (routine == null || EditorApplication.timeSinceStartup < nextTick) return;
            nextTick = EditorApplication.timeSinceStartup + .04;
            try
            {
                if (!routine.MoveNext()) { routine = null; Status = "PASS"; }
            }
            catch (Exception error) { Results.Add("FAIL " + error); routine = null; Status = "FAIL"; }
            File.WriteAllText(Path.Combine(Root, "runtime.txt"), Status + "\n" + string.Join("\n", Results));
            if (routine == null) EditorApplication.ExitPlaymode();
        }
        private static void Check(bool value, string text)
        { if (!value) throw new InvalidOperationException(text); Results.Add("PASS " + text); }
        private static T Value<T>(object owner, string name) => (T)owner.GetType().GetField(name, Private).GetValue(owner);
        private static void Call(object owner, string method, params object[] args) => owner.GetType().GetMethod(method, Private).Invoke(owner, args);
        private static void Click(string name) => GameObject.Find(name).GetComponent<Button>().onClick.Invoke();
        private static IEnumerator WaitScene(string name)
        {
            int ticks = 0;
            while (SceneManager.GetActiveScene().name != name && ticks++ < 300) yield return null;
            Check(SceneManager.GetActiveScene().name == name, "Scene " + name);
            for (int i = 0; i < 6; i++) yield return null;
        }
        private static IEnumerator Load(string name)
        {
            SceneManager.LoadSceneAsync(name);
            var wait = WaitScene(name); while (wait.MoveNext()) yield return null;
        }

        private static IEnumerator Run()
        {
            for (int i = 0; i < 8; i++) yield return null;
            var catalog = PlayerAppearanceCatalog.Load();
            Check(catalog.Entries.Count == 50 && catalog.Entries.Select(e => e.appearanceId).Distinct().Count() == 50, "Catalog 50 / duplicate ID 0");
            var inventory = JsonUtility.FromJson<PlayerAppearanceCatalog.Inventory>(File.ReadAllText("Assets/_Project/Resources/PlayerAppearances/Validated50.json"));
            Check(catalog.Entries.All(e => e.readyForSelection == inventory.entries.Single(i => i.appearanceId == e.appearanceId).readyForSelection)
                && catalog.Entries.Count(e => e.IsUsable) == inventory.entries.Count(e => e.readyForSelection), "최신 Foreground 판정과 Catalog 일치");
            Check(catalog.Entries.All(e => e.frames.Length == 16 && e.frames.All(s => s != null) && e.sheet != null &&
                File.Exists(e.assetPath + ".meta") && !string.IsNullOrEmpty(e.gender) && !string.IsNullOrEmpty(e.theme)), "800 Sprite / meta / gender / theme 누락0");
            Click("StartMenuCanvas/Slot01/Action");
            var wait = WaitScene("CharacterCreation"); while (wait.MoveNext()) yield return null;
            var creation = Object.FindAnyObjectByType<CharacterCreationController>();
            Check(Value<InputField>(creation, "nameInputField").text == "", "새 캐릭터 이름 초기화");
            string[] themes = { "", "Vision", "Hearing", "Intellectual", "Mobility", "EmotionalScar" };
            foreach (var gender in new[] { PlayerVisualType.Male, PlayerVisualType.Female })
            {
                Click(gender == PlayerVisualType.Male ? "MaleButton" : "FemaleButton");
                for (int t = 0; t < themes.Length; t++)
                {
                    Click("Theme" + t);
                    var filtered = Value<List<PlayerAppearanceCatalog.Entry>>(creation, "filteredAppearances");
                    int expectedCount = inventory.entries.Count(e => e.readyForSelection && e.gender == gender.ToString()
                        && (t == 0 || e.theme == themes[t]));
                    Check(filtered.Count == expectedCount && filtered.All(e => e.gender == gender.ToString() &&
                        (t == 0 || e.theme == themes[t])), "실제 UI 필터 " + gender + "/" + themes[t]);
                    string before = Value<string>(creation, "selectedAppearanceId");
                    Click("NextAppearance"); Click("PreviousAppearance");
                    Check(Value<string>(creation, "selectedAppearanceId") == before, "Previous/Next cycle " + gender + "/" + themes[t]);
                    Click("NextAppearance");
                    var entry = filtered.Find(e => e.appearanceId == Value<string>(creation, "selectedAppearanceId"));
                    Sprite expected = entry?.frames[0] ?? Value<Sprite>(creation, gender == PlayerVisualType.Male ? "malePreviewSprite" : "femalePreviewSprite");
                    Check(GameObject.Find("AppearancePreview").GetComponent<Image>().sprite == expected, "큰 Preview " + gender + "/" + themes[t]);
                    Check(GameObject.Find("AppearanceLabel").GetComponent<Text>().text.Contains("✓ 선택됨"), "선택 텍스트 " + gender + "/" + themes[t]);
                }
            }
            Check(GameSessionData.SelectedAppearanceId == "" && GameSessionData.SelectedPlayerVisual == PlayerVisualType.Male,
                "필터 초안은 확정 전 Session/Save를 변경하지 않음");
            var controls = Value<List<Selectable>>(creation, "navigationControls");
            Check(controls.Count == 12 && controls.All(c => c.navigation.mode == Navigation.Mode.Explicit && c.navigation.selectOnDown != null), "버튼12개 키보드/Controller Navigation");
            foreach (var control in controls)
            {
                EventSystem.current.SetSelectedGameObject(control.gameObject);
                Call(creation, "MoveToNextControl", 1);
                Check(EventSystem.current.currentSelectedGameObject == controls[(controls.IndexOf(control) + 1) % controls.Count].gameObject, "Tab 순서 " + control.name);
                EventSystem.current.SetSelectedGameObject(control.gameObject);
                ExecuteEvents.Execute(control.gameObject, new AxisEventData(EventSystem.current) { moveDir = MoveDirection.Down }, ExecuteEvents.moveHandler);
                Check(EventSystem.current.currentSelectedGameObject == controls[(controls.IndexOf(control) + 1) % controls.Count].gameObject, "EventSystem 방향 이동 " + control.name);
            }
            // 판정 변경으로 특정 테마가 전부 보류돼도 실제 길과 다른 사용 가능한 외형으로 독립성을 검증합니다.
            string availableTheme = catalog.Entries.First(e => e.IsUsable && e.gender == "Male" && e.theme != "Vision").theme;
            Click("MaleButton"); Click("Theme" + Array.IndexOf(themes, availableTheme)); Click("NextAppearance");
            string chosen = Value<string>(creation, "selectedAppearanceId");
            Check(PlayerAppearanceCatalog.Resolve(chosen)?.theme == availableTheme, "사용 가능 외형 테마 선택");
            Value<InputField>(creation, "nameInputField").text = "!"; Click("StartButton");
            Check(SceneManager.GetActiveScene().name == "CharacterCreation" && GameSessionData.SelectedAppearanceId == "", "잘못된 이름 확정 차단");
            CaptureCreation("creation.png");
            Value<InputField>(creation, "nameInputField").text = "외형감사"; Click("StartButton");
            wait = WaitScene("PathSelection"); while (wait.MoveNext()) yield return null;
            Check(GameSessionData.SelectedAppearanceId == chosen && GameSessionData.PlayerName == "외형감사", "ID/이름 확정");
            var paths = Resources.LoadAll<PlayerPathDefinition>("PathDefinitions").OrderBy(p => p.name).ToArray();
            for (int p = 0; p < paths.Length; p++)
            {
                var pathUI = Object.FindAnyObjectByType<PathSelectionController>();
                var definitions = Value<PlayerPathDefinition[]>(pathUI, "pathDefinitions");
                Call(pathUI, "SelectPath", Array.FindIndex(definitions, x => x.Id == paths[p].Id));
                Check(GameSessionData.SelectedPlayerPathId == paths[p].Id && GameSessionData.SelectedAppearanceId == chosen, "Path 독립 " + paths[p].Id);
                Value<Button>(pathUI, "chooseButton").onClick.Invoke();
                wait = WaitScene("JobSelection"); while (wait.MoveNext()) yield return null;
                var jobUI = Object.FindAnyObjectByType<JobSelectionController>();
                foreach (var job in Resources.LoadAll<JobDefinition>("JobDefinitions"))
                {
                    Call(jobUI, "SelectJob", job);
                    Check(GameSessionData.SelectedJobId == job.JobId && GameSessionData.SelectedPlayerPathId == paths[p].Id &&
                        GameSessionData.SelectedAppearanceId == chosen, "Path×Job 독립 " + paths[p].Id + "/" + job.JobId);
                }
                Check(GameObject.Find("Preview").GetComponent<Image>().sprite == PlayerAppearanceCatalog.Resolve(chosen).frames[0], "Job 외형 Preview " + paths[p].Id);
                wait = Load("PathSelection"); while (wait.MoveNext()) yield return null;
            }
            var selectedPathUI = Object.FindAnyObjectByType<PathSelectionController>();
            Call(selectedPathUI, "SelectPath", Array.FindIndex(Value<PlayerPathDefinition[]>(selectedPathUI, "pathDefinitions"), p => p.Id == "path.vision"));
            Value<Button>(selectedPathUI, "chooseButton").onClick.Invoke(); wait = WaitScene("JobSelection"); while (wait.MoveNext()) yield return null;
            var selectedJobUI = Object.FindAnyObjectByType<JobSelectionController>();
            Call(selectedJobUI, "SelectJob", Resources.LoadAll<JobDefinition>("JobDefinitions").First(j => j.JobId == "fighter"));
            Value<Button>(selectedJobUI, "nextButton").onClick.Invoke(); wait = WaitScene("FinalConfirmation"); while (wait.MoveNext()) yield return null;
            Click("StartButton"); wait = WaitScene("World_StarterVillage"); while (wait.MoveNext()) yield return null;
            Check(!Chapter2Main16Flow.IsHearingPlayer, "다른 외형 테마+Vision 실제 Story는 Vision");
            var visual = Object.FindAnyObjectByType<PlayerVisualController>();
            Check(visual.ActiveDefaultSprite == PlayerAppearanceCatalog.Resolve(chosen).frames[0], "실제 World 진입 선택 Sprite");
            Check(GameSaveService.TryLoadSlot(1, out var firstSave) && firstSave.AppearanceId == chosen && firstSave.PathId == "path.vision", "최종 확정 신규 Save");
            foreach (var entry in catalog.Entries.Where(e => e.IsUsable))
            {
                GameSessionData.SelectAppearance(entry.appearanceId);
                GameSessionData.SelectPlayerVisual((PlayerVisualType)Enum.Parse(typeof(PlayerVisualType), entry.gender));
                visual.SetVisual(GameSessionData.SelectedPlayerVisual);
                Check(visual.ActiveDefaultSprite == entry.frames[0], "World 적용 " + entry.appearanceId);
                VerifyAnimation(visual, entry);
                var animator = Value<Animator>(visual, "visualAnimator");
                Check(BattleVisualResolver.ResolveIdleSprite(animator.runtimeAnimatorController, "Idle_Left", visual.ActiveDefaultSprite) == entry.frames[4], "Battle Left Idle " + entry.appearanceId);
                Check(GameSaveService.SaveCurrentSession("World_StarterVillage", GameSessionData.LastSpawnPointId), "Save " + entry.appearanceId);
                Check(GameSaveService.TryLoadSlot(1, out var save) && save.AppearanceId == entry.appearanceId, "JSON Stable ID " + entry.appearanceId);
                GameSaveService.RestoreSession(1, save); visual.SetVisual(GameSessionData.SelectedPlayerVisual);
                Check(visual.ActiveDefaultSprite == entry.frames[0] && GameSessionData.SelectedAppearanceId == entry.appearanceId, "Restore ID/Sprite " + entry.appearanceId);
                yield return null;
            }
            foreach (var gender in new[] { PlayerVisualType.Male, PlayerVisualType.Female })
            {
                foreach (var path in paths)
                {
                    foreach (string id in new[] { "", "appearance.missing", "appearance.external.v1.mobility.mage.female" })
                    {
                        GameSessionData.ConfigurePlayer(gender, "호환감사"); GameSessionData.SelectPlayerPath(path.Id); GameSessionData.SelectAppearance(id);
                        visual.SetVisual(gender);
                        var variant = PathVisualCatalog.Find(path.Id)?.GetVariant(gender) ?? default;
                        Sprite fallback = variant.IsReady ? variant.DefaultDownSprite : Value<Sprite>(visual, gender == PlayerVisualType.Male ? "maleDefaultSprite" : "femaleDefaultSprite");
                        Check(visual.ActiveDefaultSprite == fallback, "기존/invalid/blocked fallback " + gender + "/" + path.Id + "/" + id);
                    }
                }
                var entry = catalog.Entries.First(e => e.IsUsable && e.gender == gender.ToString());
                GameSessionData.ConfigurePlayer(gender, "이어하기"); GameSessionData.SelectAppearance(entry.appearanceId); GameSessionData.SelectPlayerPath("path.vision");
                visual.SetVisual(gender);
                wait = Continue(entry.appearanceId, entry.frames[0]); while (wait.MoveNext()) yield return null;
                visual = Object.FindAnyObjectByType<PlayerVisualController>();
            }
            foreach (var gender in new[] { PlayerVisualType.Male, PlayerVisualType.Female })
                foreach (bool legacy in new[] { false, true })
                {
                    // 실제 파일에서 필드를 제거한 Version1과 invalid ID도 Bootstrap 버튼을 통해 이어갑니다.
                    GameSessionData.ConfigurePlayer(gender, "호환감사"); GameSessionData.SelectPlayerPath("path.vision");
                    GameSessionData.SelectAppearance("appearance.missing");
                    Check(GameSaveService.SaveCurrentSession("World_StarterVillage", GameSessionData.LastSpawnPointId), "호환 Fixture Save " + gender + "/" + legacy);
                    if (legacy)
                    {
                        string oldJson = File.ReadAllText(GameSaveService.GetSaveFilePath(1));
                        oldJson = System.Text.RegularExpressions.Regex.Replace(oldJson, @"\s*""AppearanceId""\s*:\s*""[^""]*"",?", "");
                        File.WriteAllText(GameSaveService.GetSaveFilePath(1), oldJson);
                    }
                    wait = Load("Bootstrap"); while (wait.MoveNext()) yield return null;
                    Click("StartMenuCanvas/Slot01/Action"); wait = WaitScene("World_StarterVillage"); while (wait.MoveNext()) yield return null;
                    visual = Object.FindAnyObjectByType<PlayerVisualController>();
                    Check(visual.ActiveDefaultSprite == PathVisualCatalog.Find("path.vision").GetVariant(gender).DefaultDownSprite,
                        "구버전/invalid 실제 Continue fallback " + gender + "/" + legacy);
                    Check(GameSessionData.SelectedPlayerPathId == "path.vision" && GameSessionData.SelectedJobId == "fighter", "Continue Path/Job 회귀 " + gender + "/" + legacy);
                    if (legacy) Check(string.IsNullOrEmpty(GameSessionData.SelectedAppearanceId), "구버전 ID 누락 " + gender);
                }
        }

        private static IEnumerator Continue(string id, Sprite expected)
        {
            Check(GameSaveService.SaveCurrentWorldPosition(Object.FindAnyObjectByType<PlayerController>().transform.position, "World_StarterVillage"), "실제 Continue 전 Save " + id);
            var wait = Load("Bootstrap"); while (wait.MoveNext()) yield return null;
            Click("StartMenuCanvas/Slot01/Action"); wait = WaitScene("World_StarterVillage"); while (wait.MoveNext()) yield return null;
            Check(GameSessionData.SelectedAppearanceId == id && Object.FindAnyObjectByType<PlayerVisualController>().ActiveDefaultSprite == expected, "Bootstrap 실제 Continue " + id);
        }

        private static void VerifyAnimation(PlayerVisualController visual, PlayerAppearanceCatalog.Entry entry)
        {
            var animator = Value<Animator>(visual, "visualAnimator");
            var renderer = Value<SpriteRenderer>(visual, "visualRenderer");
            var spriteAnimator = animator.GetComponent<PlayerSpriteAnimator>();
            var player = visual.GetComponent<PlayerController>();
            string[] directions = { "Down", "Left", "Right", "Up" };
            Vector2[] movement = { Vector2.down, Vector2.left, Vector2.right, Vector2.up };
            for (int d = 0; d < 4; d++)
            {
                typeof(PlayerController).GetField("movement", Private).SetValue(player, movement[d]);
                spriteAnimator.RefreshVisual(); Call(spriteAnimator, "Update"); animator.Update(0);
                Check(animator.GetCurrentAnimatorStateInfo(0).IsName("Walk_" + directions[d]), "실제 이동 방향 상태 " + entry.appearanceId + "/" + directions[d]);
                for (int f = 0; f < 4; f++)
                {
                    animator.Play("Walk_" + directions[d], 0, (f + .1f) / 4f); animator.Update(0);
                    Check(renderer.sprite == entry.frames[d * 4 + f], "Walk 프레임 " + entry.appearanceId + "/" + d + "/" + f);
                }
                typeof(PlayerController).GetField("movement", Private).SetValue(player, Vector2.zero);
                Call(spriteAnimator, "Update"); animator.Update(0);
                Check(renderer.sprite == entry.frames[d * 4] && animator.GetCurrentAnimatorStateInfo(0).IsName("Idle_" + directions[d]), "실제 정지 Idle " + entry.appearanceId + "/" + directions[d]);
            }
        }

        /// <summary>GameView를 활성화하지 않고 임시 Camera/RenderTexture로 UI만 캡처한 뒤 원래 Overlay 설정을 복구합니다.</summary>
        private static void CaptureCreation(string filename)
        {
            var canvas = GameObject.Find("CharacterCreationCanvas").GetComponent<Canvas>();
            var cameraObject = new GameObject("AppearanceAuditCamera", typeof(Camera));
            var camera = cameraObject.GetComponent<Camera>(); camera.orthographic = true;
            var texture = new RenderTexture(1280, 720, 24);
            var previousActive = RenderTexture.active;
            var previousMode = canvas.renderMode; var previousCamera = canvas.worldCamera; float previousDistance = canvas.planeDistance;
            Texture2D image = null;
            try
            {
                camera.targetTexture = texture; canvas.renderMode = RenderMode.ScreenSpaceCamera; canvas.worldCamera = camera; canvas.planeDistance = 5;
                Canvas.ForceUpdateCanvases(); camera.Render(); RenderTexture.active = texture;
                image = new Texture2D(1280, 720, TextureFormat.RGBA32, false); image.ReadPixels(new Rect(0, 0, 1280, 720), 0, 0); image.Apply();
                File.WriteAllBytes(Path.Combine(Root, filename), image.EncodeToPNG());
            }
            finally
            {
                canvas.renderMode = previousMode; canvas.worldCamera = previousCamera; canvas.planeDistance = previousDistance;
                RenderTexture.active = previousActive; camera.targetTexture = null; texture.Release();
                Object.Destroy(texture); if (image != null) Object.Destroy(image); Object.Destroy(cameraObject);
            }
        }
    }
}

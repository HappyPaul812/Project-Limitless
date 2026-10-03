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
    public static class Partial9FixedSpriteAudit
    {
        public static string Status { get; private set; } = "Idle";
        public static readonly List<string> Results = new List<string>();
        private static IEnumerator routine;
        private static double nextTick;
        private static Object[] originalWindows;
        private const BindingFlags Private = BindingFlags.Instance | BindingFlags.NonPublic;
        private const string Key = "Limitless.Partial9FixedRuntime.";
        private static string Root => Path.GetFullPath(Path.Combine(Application.dataPath, "../Temp/Partial9FixedRuntime"));
        static Partial9FixedSpriteAudit()
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

        /// <summary>Intellectual4/Vision5 대상9조합을 실제 Path/Job UI에서 선택하고, 대표 남녀를 생성·이어하기·Battle까지 검증합니다.</summary>
        private static IEnumerator Run()
        {
            for (int i = 0; i < 8; i++) yield return null;
            var catalog = PlayerAppearanceCatalog.Load();
            var paths = Resources.LoadAll<PlayerPathDefinition>("PathDefinitions").Where(p => (p.Id == "path.intellectual" || p.Id == "path.vision")).OrderBy(p => p.name).ToArray();
            var jobs = Resources.LoadAll<JobDefinition>("JobDefinitions").OrderBy(j => j.name).ToArray();
            Check(catalog.Entries.Count == 50 && paths.Length == 2 && jobs.Length == 5, "50 definitions / Intellectual/Vision targets only / 5 Job");
            // 미술 수정판의 Ready 승격도 검사합니다. 기대값은 정식 Inventory를 사용합니다.
            var inventory = JsonUtility.FromJson<PlayerAppearanceCatalog.Inventory>(File.ReadAllText(
                Path.Combine(Application.dataPath, "_Project/Resources/PlayerAppearances/Validated50.json")));
            Check(catalog.Entries.Count(e => e.RuntimeReady) == inventory.entries.Count(e => e.readyForSelection), "Ready 집계와 Inventory 일치");
            Check(catalog.Entries.Select(e => e.GenderStableId + "/" + e.PathStableId + "/" + e.JobStableId).Distinct().Count() == 50, "Duplicate0");
            Check(catalog.Entries.All(e => e.frames.Length == 16 && e.frames.All(f => f != null)), "Sprite800 preserved");
            Check(GameSaveService.SelectSlot(1), "isolated slot1");
            foreach (var gender in new[] { PlayerVisualType.Male, PlayerVisualType.Female })
                foreach (var path in paths)
                {
                    GameSessionData.Reset(); GameSessionData.ConfigurePlayer(gender, "조합감사");
                    var wait = Load("PathSelection"); while (wait.MoveNext()) yield return null;
                    var pathUI = Object.FindAnyObjectByType<PathSelectionController>();
                    Call(pathUI, "SelectPath", Array.FindIndex(Value<PlayerPathDefinition[]>(pathUI, "pathDefinitions"), p => p.Id == path.Id));
                    Check(GameSessionData.SelectedAppearanceId == "" && GameSessionData.SelectedJobId == "", "Job 미선택 선점 없음 " + gender + path.Id);
                    Value<Button>(pathUI, "chooseButton").onClick.Invoke();
                    wait = WaitScene("JobSelection"); while (wait.MoveNext()) yield return null;
                    var jobUI = Object.FindAnyObjectByType<JobSelectionController>();
                    // 검수 통과 여부와 무관하게 이번 수정 대상9종을 검사해 Ready 승격 조합도 빠뜨리지 않습니다.
                    foreach (var job in jobs.Where(j => path.Id == "path.intellectual" ?
                        (j.JobId == "fighter" || j.JobId == "mage") :
                        (gender == PlayerVisualType.Male ? (j.JobId == "fighter" || j.JobId == "guardian" || j.JobId == "sharpshooter") :
                            (j.JobId == "mage" || j.JobId == "sharpshooter"))))
                    {
                        Call(jobUI, "SelectJob", job);
                        var entry = catalog.FindCombination(gender.ToString(), path.Id, job.JobId);
                        string metadataJob = job.JobId == "sharpshooter" ? "marksman" : job.JobId;
                        string theme = path.Id.Substring(5).Replace("-", "");
                        string expectedId = "appearance.external.v1." + theme + "." + metadataJob + "." + gender.ToString().ToLowerInvariant();
                        Check(entry != null && entry.appearanceId == expectedId && GameSessionData.SelectedAppearanceId == expectedId, "Mapping " + expectedId);
                        Sprite fallback = Value<Sprite>(jobUI, gender == PlayerVisualType.Female ? "femalePreviewSprite" : "malePreviewSprite");
                        Check(Value<Image>(jobUI, "characterPreview").sprite == (entry.RuntimeReady ? entry.frames[0] : fallback), "Job Preview " + expectedId);
                        Check(entry.RuntimeReady || Value<Text>(jobUI, "previewStatus").text.Contains("임시 fallback"), "Blocked 표시 " + expectedId);
                    }
                }
            string[] representativePaths = { "path.intellectual", "path.vision" };
            string[] representativeJobs = { "fighter", "mage" };
            var genders = new[] { PlayerVisualType.Male, PlayerVisualType.Female };
            for (int sample = 0; sample < representativePaths.Length; sample++)
            {
                GameSessionData.Reset(); GameSaveService.SelectSlot(1);
                var wait = Load("CharacterCreation"); while (wait.MoveNext()) yield return null;
                var creation = Object.FindAnyObjectByType<CharacterCreationController>();
                Click(genders[sample] == PlayerVisualType.Male ? "MaleButton" : "FemaleButton");
                Check(GameObject.Find("NextAppearance") == null && GameObject.Find("PreviousAppearance") == null && GameObject.Find("Theme0") == null, "수동 외형 UI 제거");
                var controls = Value<List<Selectable>>(creation, "navigationControls");
                Check(controls.Count == 4 && controls.All(c => c.navigation.mode == Navigation.Mode.Explicit), "Gender/Name/Next Navigation4");
                Check(GameSessionData.SelectedPlayerPathId == "" && GameSessionData.SelectedJobId == "", "기본 정보 조합 미선택");
                Value<InputField>(creation, "nameInputField").text = "조합감사"; Click("StartButton");
                wait = WaitScene("PathSelection"); while (wait.MoveNext()) yield return null;
                var pathUI = Object.FindAnyObjectByType<PathSelectionController>();
                Call(pathUI, "SelectPath", Array.FindIndex(Value<PlayerPathDefinition[]>(pathUI, "pathDefinitions"), p => p.Id == representativePaths[sample]));
                Value<Button>(pathUI, "chooseButton").onClick.Invoke();
                wait = WaitScene("JobSelection"); while (wait.MoveNext()) yield return null;
                var jobUI = Object.FindAnyObjectByType<JobSelectionController>();
                Call(jobUI, "SelectJob", jobs.Single(j => j.JobId == representativeJobs[sample]));
                Value<Button>(jobUI, "nextButton").onClick.Invoke();
                wait = WaitScene("FinalConfirmation"); while (wait.MoveNext()) yield return null;
                var final = Object.FindAnyObjectByType<FinalConfirmationController>();
                var entry = catalog.FindCombination(genders[sample].ToString(), representativePaths[sample], representativeJobs[sample]);
                Sprite fallback = Value<Sprite>(final, genders[sample] == PlayerVisualType.Female ? "femalePreviewSprite" : "malePreviewSprite");
                Sprite expected = entry.RuntimeReady ? entry.frames[0] : fallback;
                Check(GameObject.Find("CharacterImage").GetComponent<Image>().sprite == expected, "Confirm Preview " + entry.appearanceId);
                Click("StartButton"); wait = WaitScene("World_StarterVillage"); while (wait.MoveNext()) yield return null;
                var visual = Object.FindAnyObjectByType<PlayerVisualController>();
                Check(visual.ActiveDefaultSprite == expected, "World " + entry.appearanceId);
                Check(GameSaveService.TryLoadSlot(1, out var save) && save.AppearanceId == entry.appearanceId, "자동 Save ID " + entry.appearanceId);
                // 수동 ID가 누락·불일치한 과거 Save도 정본 조합을 먼저 복원합니다.
                foreach (string oldId in new[] { "", "appearance.missing", "appearance.external.v1.mobility.mage.male" })
                {
                    save.AppearanceId = oldId; GameSaveService.RestoreSession(1, save);
                    Check(GameSessionData.SelectedAppearanceId == entry.appearanceId, "구버전/불일치 재계산 " + oldId);
                }
                var ancient = JsonUtility.FromJson<GameSaveData>(JsonUtility.ToJson(save));
                ancient.PathId = ""; ancient.JobId = ""; GameSaveService.RestoreSession(1, ancient);
                visual.SetVisual(genders[sample]);
                Check(GameSessionData.SelectedAppearanceId == "" && visual.ActiveDefaultSprite == fallback, "Path/Job 없는 Save 기본 fallback");
                GameSaveService.RestoreSession(1, save); visual.SetVisual(genders[sample]);
                Check(GameSaveService.SaveCurrentWorldPosition(Object.FindAnyObjectByType<PlayerController>().transform.position, "World_StarterVillage"), "Continue 전 Save");
                wait = Load("Bootstrap"); while (wait.MoveNext()) yield return null;
                Click("StartMenuCanvas/Slot01/Action"); wait = WaitScene("World_StarterVillage"); while (wait.MoveNext()) yield return null;
                visual = Object.FindAnyObjectByType<PlayerVisualController>();
                Check(GameSessionData.SelectedAppearanceId == entry.appearanceId && visual.ActiveDefaultSprite == expected, "실제 Bootstrap Continue " + entry.appearanceId);
                var animator = Value<Animator>(visual, "visualAnimator");
                var battleSprite = BattleVisualResolver.ResolveIdleSprite(animator.runtimeAnimatorController, "Idle_Left", visual.ActiveDefaultSprite);
                Sprite expectedBattle = entry.RuntimeReady ? entry.frames[4] : BattleVisualResolver.ResolveIdleSprite(
                    Value<RuntimeAnimatorController>(visual, genders[sample] == PlayerVisualType.Female ? "femaleAnimatorController" : "maleAnimatorController"), "Idle_Left", fallback);
                Check(battleSprite == expectedBattle, "Battle Left Idle " + entry.appearanceId);
                var monster = Resources.LoadAll<ProjectLimitless.Monster.MonsterDefinition>("MonsterDefinitions").First(m => m != null);
                BattleEncounterContext.Set(monster, null, animator.runtimeAnimatorController, visual.ActiveDefaultSprite, Vector2.zero, Vector2.zero);
                wait = Load("Battle"); while (wait.MoveNext()) yield return null;
                Check(BattleEncounterContext.PlayerBattleSprite == expectedBattle && Object.FindAnyObjectByType<BattleSceneController>() != null, "실제 Battle Scene 전달 " + entry.appearanceId);
                BattleEncounterContext.Set(null, null, null, null, Vector2.zero, Vector2.zero);
            }
            var end = Load("Bootstrap"); while (end.MoveNext()) yield return null;
        }
    }
}

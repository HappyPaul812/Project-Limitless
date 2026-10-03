using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ProjectLimitless.Audio;
using ProjectLimitless.Battle;
using ProjectLimitless.Core;
using ProjectLimitless.Monster;
using ProjectLimitless.Player;
using ProjectLimitless.UI;
using ProjectLimitless.World;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace ProjectLimitless.EditorTools
{
    /// <summary>Main16 Audio만 기존 Scene·대사 Factory·전투 복귀로 확인합니다. Quest 전체 감사와 청취 품질 판정은 하지 않습니다.</summary>
    [InitializeOnLoad]
    public static class Main16AudioAudit
    {
        public static string Status { get; private set; } = "Idle";
        public static readonly List<string> Results = new List<string>();
        private static IEnumerator routine;
        private static double nextTick;
        private const string Key = "Limitless.Main16AudioAudit.";
        private const BindingFlags Private = BindingFlags.Instance | BindingFlags.NonPublic;
        private static string Output => Path.GetFullPath(Path.Combine(Application.dataPath, "../Temp/Main16AudioAudit/runtime.txt"));
        static Main16AudioAudit()
        {
            EditorApplication.update += Tick;
            EditorApplication.playModeStateChanged += s =>
            {
                if (s != PlayModeStateChange.EnteredEditMode) return;
                routine = null;
                if (!SessionState.GetBool(Key + "Armed", false)) return;
                // 사용자 Save/Settings를 복원하기 전에 제거된 Mixer로 연결된 이벤트를 정리합니다.
                typeof(AudioSettingsService).GetMethod("ResetRuntime", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, null);
                GameSaveService.FinishAudit(); UserSettingsService.FinishAudit(); GameSessionData.Reset();
                EditorSettings.enterPlayModeOptionsEnabled = SessionState.GetBool(Key + "Enabled", false);
                EditorSettings.enterPlayModeOptions = (EnterPlayModeOptions)SessionState.GetInt(Key + "Options", 0);
                Application.runInBackground = SessionState.GetBool(Key + "Background", false);
                SessionState.SetBool(Key + "Armed", false);
            };
        }
        /// <summary>Main16 Audio 전용 진입입니다. 외형 QA 도구/Asset/Scene을 호출하거나 변경하지 않고 저장·설정만 격리합니다.</summary>
        public static string Launch()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isCompiling ||
                SceneManager.GetActiveScene().isDirty || SceneManager.GetActiveScene().name != "Bootstrap")
                throw new InvalidOperationException("clean Bootstrap Edit Mode 필요");
            SessionState.SetBool(Key + "Enabled", EditorSettings.enterPlayModeOptionsEnabled);
            SessionState.SetInt(Key + "Options", (int)EditorSettings.enterPlayModeOptions);
            SessionState.SetBool(Key + "Background", Application.runInBackground);
            SessionState.SetBool(Key + "Armed", true);
            EditorSettings.enterPlayModeOptionsEnabled = true;
            EditorSettings.enterPlayModeOptions = EnterPlayModeOptions.DisableDomainReload;
            GameSaveService.AuditSaveDirectory = Path.Combine(Path.GetDirectoryName(Output), Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(GameSaveService.AuditSaveDirectory);
            UserSettingsService.BeginAudit(GameSaveService.AuditSaveDirectory);
            File.WriteAllText(UserSettingsService.SettingsFilePath, JsonUtility.ToJson(new UserSettingsData { MuteAll = false, SkipOpeningIntro = true }));
            Application.runInBackground = true; EditorApplication.EnterPlaymode(); return "Main16 Audio isolated Save/Settings";
        }
        public static void Start(bool resumeFromStory = false)
        {
            if (!EditorApplication.isPlaying || GameSaveService.AuditSaveDirectory == null)
                throw new InvalidOperationException("격리 Save/Settings Play Mode 필요");
            Directory.CreateDirectory(Path.GetDirectoryName(Output));
            Results.Clear(); Status = "Running"; routine = Run(resumeFromStory);
        }
        private static void Tick()
        {
            if (routine == null || EditorApplication.timeSinceStartup < nextTick) return;
            nextTick = EditorApplication.timeSinceStartup + .05;
            try { if (!routine.MoveNext()) { Status = "PASS"; routine = null; } }
            catch (Exception e) { Results.Add("FAIL " + e); Status = "FAIL"; routine = null; }
            File.WriteAllText(Output, Status + "\n" + string.Join("\n", Results));
        }
        private static T Value<T>(object o, string field) => (T)o.GetType().GetField(field, Private).GetValue(o);
        private static void Check(bool ok, string description)
        { if (!ok) throw new InvalidOperationException(description); Results.Add("PASS " + description); }
        private static IEnumerator Wait(double seconds)
        { double until = EditorApplication.timeSinceStartup + seconds; while (EditorApplication.timeSinceStartup < until) yield return null; }
        private static IEnumerator Scene(string name, string clip)
        {
            double until = EditorApplication.timeSinceStartup + 30;
            while (SceneManager.GetActiveScene().name != name && EditorApplication.timeSinceStartup < until) yield return null;
            var settle = Wait(.6); while (settle.MoveNext()) yield return null;
            Check(SceneManager.GetActiveScene().name == name, "Scene " + name);
            Check(BgmPlaybackService.Instance.CurrentClip?.name == clip, "BGM " + name + " -> " + (clip ?? "null/TBD"));
            Check(Object.FindObjectsByType<BgmPlaybackService>().Length == 1, "단일 BGM Service " + name);
            var source = BgmPlaybackService.Instance.GetComponent<AudioSource>();
            Check(source.loop && source.volume == 1 && source.pitch == 1 && source.outputAudioMixerGroup.name == "BGM", "Loop/gain/pitch/BGM Group " + name);
            Check(Object.FindObjectsByType<AudioListener>().Length == 1, "Listener 1 " + name);
            MonsterEncounterService.SuppressForSeconds(3600);
        }
        private static IEnumerator Load(string name, string clip, string spawn = "")
        { SceneTransitionService.Load(name, spawn); var wait = Scene(name, clip); while (wait.MoveNext()) yield return null; }
        private static IEnumerator Exit(string objectName, string scene, string clip)
        {
            var exit = GameObject.Find(objectName); Check(exit != null, "실제 Exit " + objectName);
            var p = Object.FindAnyObjectByType<PlayerController>(); p.transform.position = exit.transform.position;
            p.GetComponent<Rigidbody2D>().position = exit.transform.position; Physics2D.SyncTransforms();
            var wait = Scene(scene, clip); while (wait.MoveNext()) yield return null;
        }
        private static IEnumerator Voice(DialogueLine[] lines, string context)
        {
            var d = DialoguePresenter.Instance; d.ShowSequence(lines, null);
            var voice = Value<VoicePlaybackSource>(d, "voicePlayback");
            var catalog = Resources.Load<VoiceClipCatalog>("Audio/Voice/Story/StoryVoiceCatalog");
            foreach (var line in lines)
            {
                Check(voice.Clip == catalog.Find(line.DialogueId, line.SpeakerId), "Voice mapping " + context + " " + line.DialogueId + "/" + line.SpeakerId);
                Check(BgmPlaybackService.Instance.CurrentClip?.name == "Where_the_Earth_Breathes", "대사 중 Field07 BGM " + line.DialogueId);
                // 실제 대사 UI에서 자연 종료까지 재생합니다. 출력 수치는 발음/감정/밸런스의 청취 판정이 아닙니다.
                float peak = 0; var buffer = new float[256]; double until = EditorApplication.timeSinceStartup + Math.Max(.3, voice.ClipLength + .1);
                while (EditorApplication.timeSinceStartup < until)
                { AudioListener.GetOutputData(buffer, 0); peak = Math.Max(peak, buffer.Max(v => Mathf.Abs(v))); yield return null; }
                Results.Add("PLAYED " + context + " " + line.DialogueId + " outputPeak=" + peak + " hearing=UNREVIEWED");
                d.Advance(); yield return null;
            }
            Check(!d.IsOpen && voice.Clip == null, "Next/종료 cleanup " + context);
        }
        private static IEnumerator WinStoryBattle()
        {
            var b = Object.FindAnyObjectByType<BattleSceneController>(); int attacks = 0, ticks = 0;
            while (!Value<bool>(b, "battleEnded") && ticks++ < 2000)
            {
                Button attack = Value<Button>(b, "attackButton");
                if (!Value<bool>(b, "actionPlaying") && attack.interactable && Value<Combatant>(b, "currentActor")?.IsPlayerControlled == true)
                {
                    attack.onClick.Invoke(); foreach (DictionaryEntry pair in Value<IDictionary>(b, "combatantViews"))
                    {
                        var actor = (Combatant)pair.Key; if (actor.Side != BattleSide.Enemies || !actor.IsAlive) continue;
                        var hit = (Button)pair.Value.GetType().GetField("HitArea").GetValue(pair.Value);
                        if (hit.interactable) { hit.onClick.Invoke(); attacks++; break; }
                    }
                }
                yield return null;
            }
            Check(Value<bool>(b, "battleEnded") && Value<Formation>(b, "enemies").IsDefeated && attacks > 0, "Story 실제 공격 Victory");
            Object.FindObjectsByType<Button>().Single(v => v.name == "VictoryReturn").onClick.Invoke();
            var wait = Scene(Chapter2Main16Flow.Field, "Where_the_Earth_Breathes"); while (wait.MoveNext()) yield return null;
        }
        private static IEnumerator Run(bool resumeFromStory)
        {
            UserSettingsService.SetMuteAll(false); UserSettingsService.SetAudioVolumes(100, 100, 80);
            GameSessionData.Reset(); GameSessionData.ConfigurePlayer(PlayerVisualType.Male, "음성 검증");
            // Bootstrap 새 게임 버튼을 누르지 않는 감사 Fixture도 저장 대상 슬롯을 명시해야 합니다.
            GameSaveService.SelectSlot(1);
            GameSessionData.SelectJob("fighter"); GameSessionData.SelectPlayerPath("path.hearing"); GameSessionData.ConfigureProgress(20, 0);
            GameSessionData.ActivateSafeZone("arbel", "Arbel", "Spawn_From_Field06");
            // 완료 Fixture는 자동 조사/목격을 막기 위한 것으로, 실제 사용자 Quest/Save를 바꾸지 않습니다.
            QuestService.ImportSaveData(new QuestProgressSaveData { CompletedQuestIds = QuestCatalog.All.Where(q => q.QuestType == QuestType.Main).Select(q => q.QuestId).ToArray() });
            CompanionRosterService.Reset(); CompanionRosterService.UnlockIntroCompanions(); PartyResourceService.Reset();
            IEnumerator wait;
            if (!resumeFromStory)
            {
            wait = Scene("Bootstrap", "Before_the_First_Light"); while (wait.MoveNext()) yield return null;
            wait = Load("Arbel", "Morning_at_the_Gate", "Spawn_From_Field06"); while (wait.MoveNext()) yield return null;
            wait = Load(Chapter2Main16Flow.PreviousField, "Paths_of_Cracked_Earth", "Spawn_From_Field07"); while (wait.MoveNext()) yield return null;
            wait = Exit("Transition_west_to_field07", Chapter2Main16Flow.Field, "Where_the_Earth_Breathes"); while (wait.MoveNext()) yield return null;
            wait = Exit("Transition_east_to_field06", Chapter2Main16Flow.PreviousField, "Paths_of_Cracked_Earth"); while (wait.MoveNext()) yield return null;
            wait = Exit("Transition_west_to_field07", Chapter2Main16Flow.Field, "Where_the_Earth_Breathes"); while (wait.MoveNext()) yield return null;
            var p = Object.FindAnyObjectByType<PlayerController>(); Check(GameSaveService.SaveCurrentWorldPosition(p.transform.position, Chapter2Main16Flow.Field), "Field07 Save");
            SceneManager.LoadSceneAsync("Bootstrap"); wait = Scene("Bootstrap", "Before_the_First_Light"); while (wait.MoveNext()) yield return null;
            GameObject.Find("StartMenuCanvas/Slot01/Action").GetComponent<Button>().onClick.Invoke();
            wait = Scene(Chapter2Main16Flow.Field, "Where_the_Earth_Breathes"); while (wait.MoveNext()) yield return null;
            Check(GameSessionData.SelectedPlayerPathId == "path.hearing", "Continue Path 유지");
            var normal = Object.FindObjectsByType<MonsterFieldController>().First(m => m.SpawnDefinition != null && m.SpawnDefinition.SceneName == Chapter2Main16Flow.Field);
            // 일반 진입의 기존 공용 경로와 실제 Context를 사용하며 전투 음악만 확인합니다.
            typeof(BattleSceneFlow).GetMethod("EnterBattle", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { normal.SpawnDefinition.Monster, normal.SpawnDefinition });
            wait = Scene("Battle", null); while (wait.MoveNext()) yield return null;
            BattleSceneFlow.ReturnToField(false); wait = Scene(Chapter2Main16Flow.Field, "Where_the_Earth_Breathes"); while (wait.MoveNext()) yield return null;
            }
            else
            {
                // 앞서 통과한 Scene·Save·일반전 검사를 반복하지 않고 Story 복귀부터 재개합니다.
                wait = Load(Chapter2Main16Flow.Field, "Where_the_Earth_Breathes", "Spawn_From_Field06"); while (wait.MoveNext()) yield return null;
            }
            var story = Resources.Load<FieldMonsterSpawnDefinition>("StoryEncounterReturns/Main16_EmberWraithReturn");
            // 실제 목격 지점처럼 Exit에서 떨어뜨려 복귀 안전 이동이 출구를 다시 통과하지 않게 합니다.
            var player = Object.FindAnyObjectByType<PlayerController>(); player.transform.position = new Vector2(-3, 0);
            player.GetComponent<Rigidbody2D>().position = player.transform.position; Physics2D.SyncTransforms();
            Check(BattleSceneFlow.EnterStoryBattle(Chapter2Main16Flow.StoryEncounterId, story.Monster, story, player.transform.position), "Story Battle 진입");
            wait = Scene("Battle", null); while (wait.MoveNext()) yield return null;
            wait = WinStoryBattle(); while (wait.MoveNext()) yield return null;
            wait = Voice(Main16DialogueCatalog.Get("afterimage", true), "Victory afterimage/Hearing"); while (wait.MoveNext()) yield return null;
            Check(BattleSceneFlow.EnterStoryBattle(Chapter2Main16Flow.StoryEncounterId, story.Monster, story, Vector2.zero), "Defeat 복귀 Fixture 진입");
            wait = Scene("Battle", null); while (wait.MoveNext()) yield return null;
            // Audio 회귀는 기존 패배 복귀 API를 직접 호출합니다. 전투 손실이나 Quest 전수 검증으로 보고하지 않습니다.
            BattleSceneFlow.ReturnAfterDefeat(); wait = Scene("Arbel", "Morning_at_the_Gate"); while (wait.MoveNext()) yield return null;
            wait = Load(Chapter2Main16Flow.Field, "Where_the_Earth_Breathes", "Spawn_From_Field06"); while (wait.MoveNext()) yield return null;
            string[] contexts = { "start", "ash", "vibration", "tracks", "witness_ground", "witness_emerge", "retry", "afterimage", "canyon", "report" };
            var played = new HashSet<string>();
            foreach (bool hearing in new[] { true, false })
            {
                GameSessionData.SelectPlayerPath(hearing ? "path.hearing" : "path.vision");
                Check(Chapter2Main16Flow.IsHearingPlayer == hearing, "실제 Story Path 분기 " + hearing);
                foreach (string context in contexts)
                {
                    var lines = Main16DialogueCatalog.Get(context, hearing);
                    if (context == "ash") Check(lines.First(l => !string.IsNullOrEmpty(l.SpeakerId)).SpeakerId == (hearing ? "player" : "companion_serin"), "발견자 우선권 " + hearing);
                    var remaining = lines.Where(l => played.Add(l.DialogueId)).ToArray(); if (remaining.Length == 0) continue;
                    wait = Voice(remaining, context + "/" + (hearing ? "Hearing" : "Default")); while (wait.MoveNext()) yield return null;
                }
            }
            Check(played.Count == 44, "고유 대사44 / Voice28 실제 UI 재생");
            var source = BgmPlaybackService.Instance.GetComponent<AudioSource>();
            int at = source.timeSamples; BgmPlaybackService.Instance.Play(source.clip); Check(source.timeSamples >= at, "같은 곡 재요청 재시작 없음");
            source.time = source.clip.length - 1; wait = Wait(2.5); while (wait.MoveNext()) yield return null;
            Check(source.isPlaying && source.time < 5, "Runtime Loop 순환(청취 경계 미판정)");
            Results.Add("LISTENING 0/28; Voice/BGM masking, pronunciation, emotion, loop boundary UNREVIEWED");
        }
    }
}

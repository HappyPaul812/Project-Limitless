using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using ProjectLimitless.UI;
using ProjectLimitless.Audio;
using ProjectLimitless.Core;
using ProjectLimitless.Player;
using ProjectLimitless.World;
using UnityEditor;
using UnityEngine;

namespace ProjectLimitless.EditorTools
{
    /// <summary>음성 원본을 바꾸기 전에 실제 Story 함수가 생성하는 ID·화자·본문을 내보냅니다.
    /// 별도 검증 프로젝트의 batchmode에서만 호출하며 Scene·Save·Quest를 진행하지 않습니다.</summary>
    [InitializeOnLoad]
    public static class Tts162IntegrationAudit
    {
        [Serializable] public sealed class LineRow { public string id, speaker, text, method; }
        [Serializable] public sealed class Export { public LineRow[] rows; public string[] skipped; }
        [Serializable] public sealed class Target { public string id, speaker, text, voice, sourceSha, assetPath, action, originalGuid; }
        [Serializable] public sealed class Targets { public string status; public Target[] rows; }
        [Serializable] public sealed class Mapping { public string id, guid, path; public int samples, frequency, channels; }
        [Serializable] public sealed class Applied { public string status; public Mapping[] rows; public int catalogCount; }
        [Serializable] public sealed class RuntimeResult { public string status; public string[] checks, errors; public int playingObserved; }
        const string Armed = "Limitless.Tts162.BatchRuntime";
        static readonly List<string> Checks = new List<string>();
        static readonly List<string> Errors = new List<string>();
        static IEnumerator routine;
        static int playingObserved;
        static double deadline;
        static string Root => Path.GetFullPath(Path.Combine(Application.dataPath, "../../.."));
        static string Output => Path.Combine(Root, "Temp/TTS162");
        static Tts162IntegrationAudit()
        {
            EditorApplication.playModeStateChanged += state =>
            {
                if (!SessionState.GetBool(Armed, false)) return;
                if (state == PlayModeStateChange.EnteredPlayMode)
                {
                    Checks.Clear(); Errors.Clear(); playingObserved = 0;
                    deadline = EditorApplication.timeSinceStartup + 180;
                    routine = PlaybackChecks();
                    EditorApplication.update += Tick;
                }
                if (state == PlayModeStateChange.EnteredEditMode)
                {
                    SessionState.SetBool(Armed, false);
                    EditorApplication.delayCall += () => EditorApplication.Exit(SessionState.GetInt(Armed + ".Exit", 1));
                }
            };
        }
        static Targets ReadTargets() => JsonUtility.FromJson<Targets>(File.ReadAllText(Path.Combine(Root, "Tools/TTS/tts162_integration_manifest.json")));
        static void Require(bool value, string name)
        {
            if (!value) throw new InvalidOperationException(name);
            Checks.Add(name);
        }
        /// <summary>Unity가 직접 새 GUID를 만들고 기존 GUID는 유지한 채 Catalog에 연결합니다.</summary>
        public static void ApplyAndRun()
        {
            try
            {
                var targets = ReadTargets();
                Require(targets.status == "PASS" && targets.rows.Length == 162, "Approved162Manifest");
                var catalog = Resources.Load<VoiceClipCatalog>("Audio/Voice/Story/StoryVoiceCatalog");
                Require(catalog != null, "CatalogLoaded");
                var serialized = new SerializedObject(catalog);
                var entries = serialized.FindProperty("entries");
                var indexes = new Dictionary<string, int>();
                for (int i = 0; i < entries.arraySize; i++) indexes.Add(entries.GetArrayElementAtIndex(i).FindPropertyRelative("id").stringValue, i);
                var mapped = new List<Mapping>();
                foreach (var target in targets.rows)
                {
                    AssetDatabase.ImportAsset(target.assetPath, ImportAssetOptions.ForceSynchronousImport);
                    if (target.action == "ADD")
                    {
                        // 새 자산도 기존 승인 음성과 같이 무손실 PCM으로 읽습니다. 기존 meta는 건드리지 않습니다.
                        var importer = (AudioImporter)AssetImporter.GetAtPath(target.assetPath);
                        var settings = importer.defaultSampleSettings;
                        settings.compressionFormat = AudioCompressionFormat.PCM;
                        settings.sampleRateSetting = AudioSampleRateSetting.PreserveSampleRate;
                        settings.loadType = AudioClipLoadType.DecompressOnLoad;
                        settings.quality = 1f;
                        importer.defaultSampleSettings = settings;
                        importer.forceToMono = false;
                        importer.SaveAndReimport();
                    }
                    var clip = AssetDatabase.LoadAssetAtPath<AudioClip>(target.assetPath);
                    Require(clip != null && clip.samples > 0, "AudioClip." + target.id);
                    string guid = AssetDatabase.AssetPathToGUID(target.assetPath);
                    Require(!string.IsNullOrEmpty(guid), "Guid." + target.id);
                    if (target.action == "REPLACE") Require(guid == target.originalGuid, "PreservedGuid." + target.id);
                    if (!indexes.TryGetValue(target.id, out int index))
                    {
                        Require(target.action == "ADD", "ExistingIdRequired." + target.id);
                        index = entries.arraySize++;
                        indexes.Add(target.id, index);
                    }
                    var entry = entries.GetArrayElementAtIndex(index);
                    entry.FindPropertyRelative("id").stringValue = target.id;
                    entry.FindPropertyRelative("speakerId").stringValue = target.speaker;
                    entry.FindPropertyRelative("clip").objectReferenceValue = clip;
                    mapped.Add(new Mapping { id = target.id, guid = guid, path = target.assetPath, samples = clip.samples, frequency = clip.frequency, channels = clip.channels });
                }
                serialized.ApplyModifiedPropertiesWithoutUndo();
                EditorUtility.SetDirty(catalog);
                AssetDatabase.SaveAssets();
                Directory.CreateDirectory(Output);
                File.WriteAllText(Path.Combine(Output, "applied.json"), JsonUtility.ToJson(new Applied { status = "PASS", rows = mapped.ToArray(), catalogCount = entries.arraySize }, true));
                // 빈 검증 Scene의 Presenter만 검사하고 사용자 Scene이나 Save는 불러오지 않습니다.
                SessionState.SetBool(Armed, true);
                EditorApplication.EnterPlaymode();
            }
            catch (Exception ex)
            {
                Directory.CreateDirectory(Output);
                File.WriteAllText(Path.Combine(Output, "apply-failed.txt"), ex.ToString());
                Debug.LogException(ex);
                EditorApplication.Exit(1);
            }
        }
        static void Tick()
        {
            try
            {
                if (EditorApplication.timeSinceStartup > deadline) throw new TimeoutException("Runtime audit deadline");
                if (routine != null && routine.MoveNext()) return;
                FinishRuntime();
            }
            catch (Exception ex) { Errors.Add(ex.ToString()); FinishRuntime(); }
        }
        static void FinishRuntime()
        {
            routine = null; EditorApplication.update -= Tick;
            File.WriteAllText(Path.Combine(Output, "runtime.json"), JsonUtility.ToJson(new RuntimeResult { status = Errors.Count == 0 ? "PASS" : "FAIL", checks = Checks.ToArray(), errors = Errors.ToArray(), playingObserved = playingObserved }, true));
            SessionState.SetInt(Armed + ".Exit", Errors.Count == 0 ? 0 : 1);
            GameSaveService.FinishAudit(); UserSettingsService.FinishAudit();
            EditorApplication.ExitPlaymode();
        }
        /// <summary>현재 Presenter/Voice Mixer/무음 분기를 실제 Play Mode에서 검사합니다.
        /// 음질 청취 판정이나 전체 퀘스트 주행으로 확대 해석하지 않습니다.</summary>
        static IEnumerator PlaybackChecks()
        {
            Application.runInBackground = true;
            UserSettingsService.BeginAudit(Path.Combine(Output, "Settings"));
            GameSaveService.AuditSaveDirectory = Path.Combine(Output, "Saves");
            AudioListener.volume = 0; // 테스트 소리가 사용자 작업을 방해하지 않게 출력만 억제합니다.
            UserSettingsService.SetMuteAll(false); UserSettingsService.SetAudioVolumes(100, 100, 80);
            GameSessionData.ConfigurePlayer(PlayerVisualType.Male, "TTS검증");
            GameSessionData.SelectPlayerPath("path.vision"); GameSessionData.SelectJob("sharpshooter");
            var obj = new GameObject("Tts162AuditPresenter");
            var presenter = obj.AddComponent<DialoguePresenter>();
            var voice = obj.GetComponent<VoicePlaybackSource>();
            var catalog = Resources.Load<VoiceClipCatalog>("Audio/Voice/Story/StoryVoiceCatalog");
            yield return null;
            foreach (var target in ReadTargets().rows)
            {
                var clip = catalog.Find(target.id, target.speaker);
                Require(clip != null, "RuntimeClip." + target.id);
                Require(catalog.Find(target.id, "wrong_speaker") == null, "SpeakerGuard." + target.id);
                clip.LoadAudioData();
                yield return null;
                var sample = new float[Math.Min(clip.samples * clip.channels, 48000)];
                Require(clip.GetData(sample, 0), "Decoded." + target.id);
                presenter.ShowSequence(new[] { new DialogueLine(target.speaker, target.speaker, target.text, target.id) }, null);
                yield return null;
                Require(voice.Clip == clip, "PresenterMapping." + target.id);
                var text = (UnityEngine.UI.Text)typeof(DialoguePresenter).GetField("dialogueText", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(presenter);
                Require(text.text == target.text, "Subtitle." + target.id);
                if (voice.IsPlaying) playingObserved++;
                presenter.Hide(); Require(voice.Clip == null, "HideStops." + target.id);
            }
            bool completed = false;
            var first = ReadTargets().rows.First(r => r.id == "main21_haren_haren_01");
            presenter.ShowSequence(new[] { new DialogueLine(first.speaker, "하렌", first.text, first.id), new DialogueLine("player", "플레이어", "무음 검증", "main21_player_silent_audit") }, () => completed = true);
            presenter.Advance(); Require(voice.Clip == null && presenter.IsOpen, "NextToPlayerSilent");
            presenter.Advance(); Require(completed && !presenter.IsOpen, "NextCompletion");
            presenter.ShowSequence(new[] { new DialogueLine("story_haren", "하렌", "<지문> 무음", first.id) }, null);
            Require(voice.Clip == null, "DirectionSilentEvenWithRegisteredId"); presenter.Hide();
            presenter.ShowSequence(new[] { new DialogueLine(first.speaker, "하렌", first.text, first.id) }, null);
            UserSettingsService.SetAudioVolumes(0, 100, 80); Require(!voice.IsAudible, "VoiceOff");
            UserSettingsService.SetAudioVolumes(100, 100, 80); Require(voice.IsAudible, "VoiceOn");
            presenter.Hide(); Require(voice.Clip == null, "SkipHideStops");
            // 실제 Main20 보스 후 및 Main21 우회로/마차/형제 배열을 그대로 진행합니다.
            var sequences = new Dictionary<string, DialogueLine[]>
            {
                { "Main20AfterBoss", Main20DialogueCatalog.Get(6) },
                { "Main21Route", Main21DialogueCatalog.Get(3) },
                { "Main21Carriage", Main21DialogueCatalog.Get(9) },
                { "Main21HarenBrothers", Main21DialogueCatalog.Get(11) }
            };
            foreach (var sequence in sequences)
            {
                bool done = false;
                presenter.ShowSequence(sequence.Value, () => done = true);
                foreach (var line in sequence.Value)
                {
                    yield return null;
                    var expected = line.IsPlayer || line.IsDirection ? null : catalog.Find(line.DialogueId, line.SpeakerId);
                    Require(voice.Clip == expected, sequence.Key + "." + line.DialogueId);
                    presenter.Advance();
                }
                Require(done && !presenter.IsOpen && voice.Clip == null, sequence.Key + ".CompletedAndStopped");
            }
            foreach (var path in new[] { "path.vision", "path.hearing" })
            {
                GameSessionData.SelectPlayerPath(path);
                for (int index = 1; index <= 12; index++)
                {
                    var lines = Main21DialogueCatalog.Get(index);
                    foreach (var line in lines) if (!line.IsPlayer && !line.IsDirection && !string.IsNullOrEmpty(line.DialogueId)) Require(catalog.Find(line.DialogueId, line.SpeakerId) != null, "Main21PathCoverage." + path + "." + line.DialogueId);
                }
            }
            Require(GameSaveService.SelectSlot(1), "IsolatedSlot");
            Require(GameSaveService.SaveCurrentSession("World_StarterVillage", "Spawn_From_Field01"), "IsolatedSave");
            Require(GameSaveService.TryLoadSlot(1, out var data), "IsolatedLoad");
            GameSaveService.RestoreSession(1, data);
            Require(catalog.Find(first.id, first.speaker) != null && GameSessionData.PlayerName == "TTS검증", "VoiceAfterLoad");
            UnityEngine.Object.Destroy(obj);
            yield return null;
            Require(Time.frameCount > 3, "PlayModeFramesAdvanced");
        }
        public static void ExportRuntime()
        {
            var rows = new List<LineRow>();
            var skipped = new List<string>();
            // 반환형과 인수형을 제한해 실제 대사 배열 생성기만 조회합니다. 상호작용·전투 함수는 호출하지 않습니다.
            foreach (var type in typeof(DialogueLine).Assembly.GetTypes().Where(t => t.Namespace == "ProjectLimitless.World"))
            foreach (var method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (method.ReturnType != typeof(DialogueLine[]) || method.ContainsGenericParameters) continue;
                var parameters = method.GetParameters();
                if (parameters.Any(p => p.ParameterType != typeof(int) && p.ParameterType != typeof(string) && p.ParameterType != typeof(bool))) continue;
                if (parameters.Count(p => p.ParameterType == typeof(int)) > 1 || parameters.Count(p => p.ParameterType == typeof(string)) > 1) continue;
                var keys = new List<string> { "path.vision", "path.hearing", "path.intellectual", "path.mobility", "path.emotional_scar" };
                // scene/id를 받는 Main16·08 생성기의 실제 상수와 switch key를 사용합니다.
                keys.AddRange(type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Where(f => f.IsLiteral && f.FieldType == typeof(string)).Select(f => (string)f.GetRawConstantValue()));
                var sourceFile = Path.Combine(Application.dataPath, "_Project/Scripts/World/" + type.Name + ".cs");
                if (File.Exists(sourceFile)) keys.AddRange(Regex.Matches(File.ReadAllText(sourceFile), "case\\s+\"([^\"]+)\"").Cast<Match>().Select(m => m.Groups[1].Value));
                foreach (var path in keys.Distinct())
                foreach (var flag in new[] { false, true })
                for (int index = 0; index <= (parameters.Any(p => p.ParameterType == typeof(int)) ? 32 : 0); index++)
                {
                    try
                    {
                        var args = parameters.Select(p => p.ParameterType == typeof(int) ? (object)index : p.ParameterType == typeof(bool) ? flag : path).ToArray();
                        var lines = (DialogueLine[])method.Invoke(null, args);
                        if (lines == null) continue;
                        foreach (var line in lines)
                            if (!string.IsNullOrEmpty(line.DialogueId)) rows.Add(new LineRow { id = line.DialogueId, speaker = line.SpeakerId, text = line.Message, method = type.Name + "." + method.Name });
                    }
                    catch (Exception ex) { skipped.Add(type.Name + "." + method.Name + ":" + index + ":" + ex.GetBaseException().GetType().Name); }
                }
            }
            // 구형 Main07는 배열 getter가 인스턴스 메서드입니다. 비활성 테스트 객체에서
            // 대사 생성만 호출하고 상호작용/완료 콜백은 호출하지 않습니다.
            var legacy = new GameObject("TtsLegacyReadOnly");
            legacy.SetActive(false);
            var site = legacy.AddComponent<MainQuest07Interactable>();
            foreach (var id in new[] { MainQuest07FieldFlow.WheelTracksId, MainQuest07FieldFlow.WoundedTravelerId })
            {
                site.Configure(id, "읽기 전용 검증");
                var lines = (DialogueLine[])typeof(MainQuest07Interactable).GetMethod("GetLines", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(site, null);
                foreach (var line in lines) if (!string.IsNullOrEmpty(line.DialogueId)) rows.Add(new LineRow { id = line.DialogueId, speaker = line.SpeakerId, text = line.Message, method = "MainQuest07Interactable.GetLines" });
            }
            UnityEngine.Object.DestroyImmediate(legacy);
            // Main12는 ID와 본문을 각각 순수 함수로 조회하는 구조를 그대로 확인합니다.
            var objective = typeof(Chapter2ObjectiveSite);
            const string targetKey = "field03_main12_party_decision";
            rows.Add(new LineRow
            {
                id = (string)objective.GetMethod("VoiceId", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { targetKey }),
                speaker = CompanionRosterService.TaeonId,
                text = (string)objective.GetMethod("Line", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { targetKey }),
                method = "Chapter2ObjectiveSite.VoiceId/Line"
            });
            var output = Path.Combine(Root, "Temp/TTS162");
            Directory.CreateDirectory(output);
            File.WriteAllText(Path.Combine(output, "runtime_dialogues.json"), JsonUtility.ToJson(new Export { rows = rows.GroupBy(r => r.id + "|" + r.speaker + "|" + r.text).Select(g => g.First()).ToArray(), skipped = skipped.Distinct().ToArray() }, true));
            Debug.Log("TTS162_RUNTIME_EXPORT_COMPLETE");
        }
    }
}

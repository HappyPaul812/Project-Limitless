using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ProjectLimitless.Battle;
using ProjectLimitless.Audio;
using ProjectLimitless.Core;
using ProjectLimitless.Monster;
using ProjectLimitless.NPC;
using ProjectLimitless.Player;
using ProjectLimitless.UI;
using ProjectLimitless.World;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace ProjectLimitless.Editor
{
    /// <summary>실제 Scene·대화·전투·Bootstrap Continue를 격리 Save로 검증합니다. 창 활성화와 OS 입력은 사용하지 않습니다.</summary>
    [InitializeOnLoad]
    public static class Main16RuntimeAudit
    {
        public static readonly List<string> Results = new List<string>();
        public static string Status { get; private set; } = "Idle";
        private static IEnumerator routine;
        private static double nextTick;
        private static string label, roster;
        private static bool voiceReview;
        private const BindingFlags Private = BindingFlags.NonPublic | BindingFlags.Instance;
        private static string Output => System.IO.Path.Combine(Application.dataPath, "../Temp/Main16Audit/runtime.txt");
        static Main16RuntimeAudit()
        {
            EditorApplication.update += Tick;
            EditorApplication.playModeStateChanged += state =>
            { if (state == PlayModeStateChange.EnteredEditMode)
                {
                    // domain reload를 생략한 감사에서도 종료된 Mixer에 설정 이벤트가 전달되지 않게 구독부터 정리합니다.
                    typeof(AudioSettingsService).GetMethod("ResetRuntime", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, null);
                    GameSaveService.FinishAudit(); UserSettingsService.FinishAudit();
                }
            };
        }
        public static void Start()
        {
            if (!EditorApplication.isPlaying) throw new InvalidOperationException("Play Mode 필요");
            Results.Clear(); Status = "Running"; Application.runInBackground = true;
            voiceReview = false;
            string directory = System.IO.Path.Combine(Application.dataPath, "../Temp/Main16Audit", Guid.NewGuid().ToString("N"));
            GameSaveService.AuditSaveDirectory = directory; GameSaveService.SelectSlot(1);
            UserSettingsService.BeginAudit(directory); UserSettingsService.SetMuteAll(true);
            Results.Add("SAVE_DIRECTORY " + directory);
            routine = Run();
        }
        /// <summary>승인된 Foreground에서 Hearing/Default 두 경로의 실제 대사를 자연 종료까지 재생합니다. 청취 품질 판정은 하지 않습니다.</summary>
        public static void StartVoiceReview()
        {
            Start(); voiceReview = true;
            UserSettingsService.SetMuteAll(false); UserSettingsService.SetAudioVolumes(100, 100, 0);
        }
        private static void Tick()
        {
            if (routine == null || EditorApplication.timeSinceStartup < nextTick) return;
            nextTick = EditorApplication.timeSinceStartup + .08;
            try { if (!routine.MoveNext()) { routine = null; Status = "PASS"; } }
            catch (Exception error) { Results.Add("FAIL " + error); routine = null; Status = "FAIL"; }
            System.IO.File.WriteAllText(Output, Status + "\n" + string.Join("\n", Results));
        }
        private static void Check(bool condition, string message)
        { if (!condition) throw new InvalidOperationException(label + " " + message); Results.Add("PASS " + label + " " + message); }
        private static T Value<T>(object owner, string name) => (T)owner.GetType().GetField(name, Private).GetValue(owner);
        private static object Call(object owner, string name, params object[] args) => owner.GetType().GetMethod(name, Private).Invoke(owner, args);
        private static IEnumerator WaitScene(string scene)
        {
            int ticks = 0;
            while (SceneManager.GetActiveScene().name != scene && ticks++ < 300) yield return null;
            Check(SceneManager.GetActiveScene().name == scene, "Scene " + scene);
            for (int i = 0; i < 8; i++) yield return null;
            MonsterEncounterService.SuppressForSeconds(3600);
        }
        private static void Move(Vector2 at)
        {
            var player = Object.FindAnyObjectByType<PlayerController>();
            player.transform.position = at;
            var body = player.GetComponent<Rigidbody2D>(); body.position = at; body.linearVelocity = Vector2.zero;
            Physics2D.SyncTransforms();
        }
        private static IEnumerator Transition(string name, string scene)
        {
            var exit = GameObject.Find(name); Check(exit != null, "실제 출구 " + name);
            Move(exit.transform.position);
            var wait = WaitScene(scene); while (wait.MoveNext()) yield return null;
            Check(Vector2.Distance(Object.FindAnyObjectByType<PlayerController>().transform.position,
                Object.FindObjectsByType<SceneSpawnPoint>().First(point => Value<string>(point, "spawnPointId") == GameSessionData.LastSpawnPointId).transform.position) < .75f, "Spawn 근처 도착/역전환 없음");
        }
        private static IEnumerator Pages()
        {
            int page = 0;
            while (DialoguePresenter.Instance != null && DialoguePresenter.Instance.IsOpen && page++ < 40)
            {
                if (voiceReview)
                {
                    var presenter = DialoguePresenter.Instance;
                    var line = Value<DialogueLine[]>(presenter, "sequenceLines")[Value<int>(presenter, "sequencePageIndex")];
                    var voice = Value<VoicePlaybackSource>(presenter, "voicePlayback");
                    var expected = Resources.Load<VoiceClipCatalog>("Audio/Voice/Story/StoryVoiceCatalog").Find(line.DialogueId, line.SpeakerId);
                    Check(voice.Clip == expected, "실제 Voice ID/화자 " + line.DialogueId + "/" + line.SpeakerId);
                    Check(line.SpeakerId != "player" || voice.Clip == null, "Player 무음 " + line.DialogueId);
                    float peak = 0; var output = new float[256]; double until = EditorApplication.timeSinceStartup + Math.Max(.7, voice.ClipLength + .15);
                    while (EditorApplication.timeSinceStartup < until && presenter.IsOpen)
                    {
                        AudioListener.GetOutputData(output, 0); peak = Math.Max(peak, output.Max(v => Mathf.Abs(v)));
                        yield return null;
                    }
                    Results.Add("OUTPUT " + label + " " + line.DialogueId + " peak=" + peak);
                    if (expected != null) Check(peak > .0001f, "실제 음성 출력 " + line.DialogueId);
                }
                DialoguePresenter.Instance.Advance(); yield return null;
            }
            if (voiceReview && DialoguePresenter.Instance != null)
                Check(Value<VoicePlaybackSource>(DialoguePresenter.Instance, "voicePlayback").Clip == null, "대화 완료 Clip 정리");
        }
        private static void Objective(int index)
        {
            Check(QuestService.ActiveMainQuest?.Definition.QuestId == Chapter2Main16Flow.QuestId && QuestService.ActiveMainQuest.CurrentObjectiveIndex == index, "순차 목표 " + (index + 1));
            Check(QuestNavigationTargetRegistry.TryGet(QuestService.ActiveMainQuest.CurrentObjective.TargetId, out _), "목표 Navigation " + (index + 1));
            Check(JsonUtility.ToJson(CompanionRosterService.ExportSaveData()) == roster, "저장 Party/Formation 보존 " + (index + 1));
        }
        private static IEnumerator Inspect(string target, string dialogue, int index, bool hearing)
        {
            Objective(index); Move(GameObject.Find(target).transform.position); yield return null;
            GameObject.Find(target).GetComponent<Main16Site>().TryInteract();
            Check(DialoguePresenter.Instance.IsOpen, "조사 대화 " + target);
            DialogueLine[] lines = Value<DialogueLine[]>(DialoguePresenter.Instance, "sequenceLines");
            Check(lines.Select(line => line.DialogueId).SequenceEqual(Main16DialogueCatalog.Get(dialogue, hearing).Select(line => line.DialogueId)), "실제 분기 ID " + dialogue);
            if (dialogue == "ash")
            {
                Check(lines.First(line => !string.IsNullOrEmpty(line.SpeakerId)).SpeakerId == (hearing ? "player" : CompanionRosterService.SerinId), "발견자 우선권");
                DialoguePresenter.Instance.Hide(); yield return null;
                Check(QuestService.ActiveMainQuest.CurrentObjectiveIndex == index, "대화 취소 진행 없음");
                GameObject.Find(target).GetComponent<Main16Site>().TryInteract();
            }
            var pages = Pages(); while (pages.MoveNext()) yield return null;
            Check(QuestService.ActiveMainQuest.CurrentObjectiveIndex == index + 1, "완료 콜백 1회 " + dialogue);
        }
        private static IEnumerator Continue(string description)
        {
            string scene = SceneManager.GetActiveScene().name;
            string quest = JsonUtility.ToJson(QuestService.ExportSaveData());
            Vector2 at = Object.FindAnyObjectByType<PlayerController>().transform.position;
            string path = GameSessionData.SelectedPlayerPathId;
            Check(GameSaveService.SaveCurrentWorldPosition(at, scene), description + " Save");
            SceneManager.LoadSceneAsync("Bootstrap"); var wait = WaitScene("Bootstrap"); while (wait.MoveNext()) yield return null;
            var button = GameObject.Find("StartMenuCanvas/Slot01/Action")?.GetComponent<Button>(); Check(button != null, "Bootstrap Continue 버튼");
            button.onClick.Invoke(); wait = WaitScene(scene); while (wait.MoveNext()) yield return null;
            Check(JsonUtility.ToJson(QuestService.ExportSaveData()) == quest && JsonUtility.ToJson(CompanionRosterService.ExportSaveData()) == roster,
                description + " Quest/count/Party 복원");
            Check(GameSessionData.SelectedPlayerPathId == path && Vector2.Distance(Object.FindAnyObjectByType<PlayerController>().transform.position, at) < .2f,
                description + " Path/좌표 복원");
        }
        private static void Seed(bool hearing, bool serin)
        {
            GameSessionData.Reset(); GameSessionData.ConfigurePlayer(voiceReview ? PlayerVisualType.Female : PlayerVisualType.Male, "Main16 감사");
            GameSessionData.SelectPlayerPath(hearing ? "path.hearing" : PathCombatTraitRuntime.MobilityPathId);
            GameSessionData.SelectJob("fighter"); GameSessionData.ConfigureProgress(20, 0);
            if (voiceReview) GameSessionData.SelectAppearance("appearance.external.v1.vision.fighter.female");
            QuestService.ImportSaveData(new QuestProgressSaveData { CompletedQuestIds = QuestCatalog.All.Where(quest => quest.QuestType == QuestType.Main && quest.QuestId != Chapter2Main16Flow.QuestId).Select(quest => quest.QuestId).ToArray() });
            CompanionRosterService.Reset(); CompanionRosterService.UnlockIntroCompanions(); CompanionRosterService.UnlockPaul("fighter"); CompanionRosterService.UnlockSerin();
            string[] party = { serin ? CompanionRosterService.SerinId : CompanionRosterService.TaeonId, CompanionRosterService.MielId };
            Check(CompanionRosterService.TrySetComposition(party, new Dictionary<string, FormationRow> { { PartyResourceService.PlayerCharacterId, FormationRow.Front },
                { party[0], serin ? FormationRow.Rear : FormationRow.Front }, { party[1], FormationRow.Rear } }), "사용자 수동 Party fixture");
            roster = JsonUtility.ToJson(CompanionRosterService.ExportSaveData());
            GameSessionData.ActivateSafeZone("arbel", "Arbel", "Spawn_From_Field06");
            PartyResourceService.Reset(); BeastCompanionService.Reset();
        }
        private static void Leon()
        {
            var role = Object.FindObjectsByType<VillageNpcRole>().Single(npc => npc.NpcId == "arbel-leon");
            Move(role.transform.position + Vector3.down * .7f); role.Interact(role.GetComponent<NpcController>());
            Check(DialoguePresenter.Instance.IsOpen, "레온 실제 NPC 대화");
        }
        private static IEnumerator RetryBattle()
        {
            Move(GameObject.Find(Chapter2Main16Flow.WitnessId).transform.position); yield return null;
            GameObject.Find(Chapter2Main16Flow.WitnessId).GetComponent<Main16Site>().TryInteract();
            var pages = Pages(); while (pages.MoveNext()) yield return null;
            var wait = WaitScene("Battle"); while (wait.MoveNext()) yield return null;
        }
        private static IEnumerator WinBattle(bool serin)
        {
            var battle = Object.FindAnyObjectByType<BattleSceneController>();
            Formation foes = Value<Formation>(battle, "enemies"), allies = Value<Formation>(battle, "allies");
            Check(foes.Members.Count() == 1 && foes.Members.Single().Slot.Row == FormationRow.Rear && foes.Members.Single().BasicRange == TargetRangeType.Magic, "불씨망령 1체 후열 Magic");
            Check(allies.Members.Any(member => member.Id == CompanionRosterService.SerinId) == serin, "실제 전투 세린 편성 일치");
            Check(BattleEncounterContext.StoryEncounterId == Chapter2Main16Flow.StoryEncounterId, "Story ID 전달");
            var ai = new BattleChapter2MonsterRuntime(); Combatant enemy = foes.Members.Single();
            string[] actions = Enumerable.Range(0, 4).Select(_ => { ai.BeginActorAction(enemy); return ai.TakeAction(enemy, "ember_wraith"); }).ToArray();
            Check(actions.SequenceEqual(new[] { "scatter", "concentrate", "core", "basic" }), "기존 AI 순서 재사용");
            // 실제 명령 버튼과 실제 타겟 버튼을 호출합니다. 피해나 승리 목표를 감사 코드가 직접 주입하지 않습니다.
            int ticks = 0, attacks = 0;
            while (!Value<bool>(battle, "battleEnded") && ticks++ < 1500)
            {
                Button attack = Value<Button>(battle, "attackButton");
                if (!Value<bool>(battle, "actionPlaying") && attack.interactable && Value<Combatant>(battle, "currentActor")?.IsPlayerControlled == true)
                {
                    attack.onClick.Invoke();
                    var views = Value<IDictionary>(battle, "combatantViews");
                    foreach (DictionaryEntry pair in views)
                    {
                        if (((Combatant)pair.Key).Side != BattleSide.Enemies || !((Combatant)pair.Key).IsAlive) continue;
                        var hit = (Button)pair.Value.GetType().GetField("HitArea").GetValue(pair.Value);
                        if (hit.interactable) { hit.onClick.Invoke(); attacks++; break; }
                    }
                }
                yield return null;
            }
            Check(Value<bool>(battle, "battleEnded") && foes.IsDefeated && attacks > 0, "실제 공격 Victory " + attacks);
            Check(QuestService.ActiveMainQuest.CurrentObjectiveIndex == 8 && Chapter2Main16Flow.EmberGeneralSpawnUnlocked, "지정 승리만 9번/일반 Spawn 해금");
            Check(BeastCompanionService.DefeatedMonsterIds.Contains("ember_wraith") && !BeastCompanionService.IsMonsterPet("ember_wraith") &&
                !BeastCompanionService.IsUnlocked("ember_wraith"), "일반 승리 기록/펫 분양·해금 없음");
            Object.FindObjectsByType<Button>().Single(button => button.name == "VictoryReturn").onClick.Invoke();
            var wait = WaitScene(Chapter2Main16Flow.Field); while (wait.MoveNext()) yield return null;
        }
        private static void CameraBounds()
        {
            var camera = Camera.main; var follow = camera.GetComponent<ProjectLimitless.CameraSystem.CameraFollow>();
            Bounds bounds = Object.FindAnyObjectByType<WorldBounds2D>().Bounds;
            var player = Object.FindAnyObjectByType<PlayerController>();
            Vector3 playerAt = player.transform.position, cameraAt = camera.transform.position;
            float originalAspect = camera.aspect, originalSize = camera.orthographicSize;
            try
            {
                foreach (float ratio in new[] { 4f / 3f, 16f / 9f, 21f / 9f })
                    foreach (Vector2 corner in new[] { new Vector2(-9, -6), new Vector2(-9, 6), new Vector2(9, -6), new Vector2(9, 6) })
                    {
                        camera.aspect = ratio; player.transform.position = corner;
                        // 직전 비율의 중심이 새 viewport 범위 밖인 상태를 재현합니다. 첫 프레임부터 맵 안이어야 합니다.
                        camera.transform.position = new Vector3(8, 6, cameraAt.z); Call(follow, "LateUpdate");
                        float hh = camera.orthographicSize, hw = hh * camera.aspect; Vector3 at = camera.transform.position;
                        Check(at.x - hw >= bounds.min.x - .002f && at.x + hw <= bounds.max.x + .002f &&
                            at.y - hh >= bounds.min.y - .002f && at.y + hh <= bounds.max.y + .002f, "비율 변경 첫 프레임 viewport " + ratio + " " + corner);
                    }
            }
            finally
            {
                camera.aspect = originalAspect; camera.orthographicSize = originalSize; camera.transform.position = cameraAt;
                player.transform.position = playerAt; player.GetComponent<Rigidbody2D>().position = playerAt; Physics2D.SyncTransforms();
            }
            BoxCollider2D[] walls = Object.FindObjectsByType<BoxCollider2D>().Where(wall => wall.name.StartsWith("Boundary_") && !wall.isTrigger).ToArray();
            Check(new[] { new Vector2(-10.5f, 0), new Vector2(0, 7.5f), new Vector2(0, -7.5f), new Vector2(10.5f, 4) }.All(point => walls.Any(wall => wall.OverlapPoint(point))) &&
                !walls.Any(wall => wall.OverlapPoint(new Vector2(10.5f, 0))), "동쪽 실제 Exit만 Boundary 개방");
        }
        private static IEnumerator Run()
        {
            for (int caseIndex = 0; caseIndex < 4; caseIndex++)
            {
                if (voiceReview && caseIndex % 2 != 0) continue;
                bool hearing = caseIndex < 2, serin = caseIndex % 2 == 0;
                label = (hearing ? "Hearing" : "Default") + (serin ? "/SerinIn" : "/SerinOut"); Seed(hearing, serin);
                SceneTransitionService.Load("Arbel", "Spawn_From_Field06"); var wait = WaitScene("Arbel"); while (wait.MoveNext()) yield return null;
                Check(QuestService.GetState(Chapter2Main16Flow.QuestId) == QuestState.Available, "Main15 완료→Main16 레온 시작 가능");
                Leon(); var pages = Pages(); while (pages.MoveNext()) yield return null; Objective(1);
                wait = Transition("Transition_northwest_to_field06", Chapter2Main16Flow.PreviousField); while (wait.MoveNext()) yield return null;
                Objective(1); Move(new Vector2(-8, 0)); yield return null; yield return null; Objective(2);
                wait = Transition("Transition_west_to_field07", Chapter2Main16Flow.Field); while (wait.MoveNext()) yield return null;
                Objective(3); Check(GameObject.Find("Serin_Story_Main16") != null, "미편성 포함 현장 세린");
                if (caseIndex == 0) CameraBounds();
                Check(!Object.FindObjectsByType<MonsterFieldController>().Any(monster => monster.SpawnDefinition?.Monster.MonsterId == "ember_wraith"), "지정 승리 전 일반 불씨망령 없음");
                QuestService.NotifyEncounterWon("field07_ember_wraith_01"); Objective(3);
                wait = Inspect("field07_main16_ash", "ash", 3, hearing); while (wait.MoveNext()) yield return null;
                wait = Continue("재 조사 중간"); while (wait.MoveNext()) yield return null;
                wait = Inspect("field07_main16_vibration", "vibration", 4, hearing); while (wait.MoveNext()) yield return null;
                wait = Inspect("field07_main16_tracks", "tracks", 5, hearing); while (wait.MoveNext()) yield return null;
                Objective(6); Move(new Vector2(-3, 0)); yield return null; yield return null;
                Check(DialoguePresenter.Instance.IsOpen && GameObject.Find("EmberWraithStoryApparition") == null, "형상 전에 지면→재→불씨 수동 연출");
                if (caseIndex == 0 && !voiceReview)
                {
                    DialoguePresenter.Instance.Hide(); yield return null; yield return null;
                    Check(!DialoguePresenter.Instance.IsOpen && QuestService.ActiveMainQuest.CurrentObjectiveIndex == 6, "목격 취소 진행·자동 재열림 없음");
                    GameObject.Find(Chapter2Main16Flow.WitnessId).GetComponent<Main16Site>().TryInteract();
                    Check(DialoguePresenter.Instance.IsOpen, "목격 취소 후 수동 재확인");
                }
                for (int i = 0; i < 3; i++) { DialoguePresenter.Instance.Advance(); yield return null; }
                Check(GameObject.Find("EmberWraithStoryApparition") != null && DialoguePresenter.Instance.IsOpen, "재 모임 뒤 공식 외형 등장");
                pages = Pages(); while (pages.MoveNext()) yield return null;
                wait = WaitScene("Battle"); while (wait.MoveNext()) yield return null;
                if (caseIndex == 0 && !voiceReview)
                {
                    var battle = Object.FindAnyObjectByType<BattleSceneController>();
                    while (Value<bool>(battle, "actionPlaying")) yield return null;
                    Call(battle, "Flee"); wait = WaitScene(Chapter2Main16Flow.Field); while (wait.MoveNext()) yield return null;
                    Objective(7); Check(!Chapter2Main16Flow.EmberGeneralSpawnUnlocked && !DialoguePresenter.Instance.IsOpen, "도망 승리 없음/자동 재진입 없음");
                    wait = Continue("지정 전투 재도전 전"); while (wait.MoveNext()) yield return null;
                    wait = RetryBattle(); while (wait.MoveNext()) yield return null;
                    battle = Object.FindAnyObjectByType<BattleSceneController>();
                    foreach (Combatant ally in Value<Formation>(battle, "allies").Members) ally.TakeDamage(int.MaxValue, false);
                    Call(battle, "AdvanceTurn"); wait = WaitScene("Arbel"); while (wait.MoveNext()) yield return null;
                    Objective(7); Check(!Chapter2Main16Flow.EmberGeneralSpawnUnlocked && GameSessionData.LastSafeZoneSceneId == "Arbel" &&
                        Vector2.Distance(Object.FindAnyObjectByType<PlayerController>().transform.position, new Vector2(0, -1.5f)) < .3f,
                        "전멸 아르벨 중앙 안전지대/미승리 유지");
                    wait = Transition("Transition_northwest_to_field06", Chapter2Main16Flow.PreviousField); while (wait.MoveNext()) yield return null;
                    wait = Transition("Transition_west_to_field07", Chapter2Main16Flow.Field); while (wait.MoveNext()) yield return null;
                    wait = RetryBattle(); while (wait.MoveNext()) yield return null;
                }
                wait = WinBattle(serin); while (wait.MoveNext()) yield return null; Objective(8);
                Check(Object.FindObjectsByType<MonsterFieldController>().Any(monster => monster.SpawnDefinition?.SpawnId == "field07_ember_wraith_01"), "승리 후 일반 불씨망령 생성");
                wait = Continue("지정 승리 후"); while (wait.MoveNext()) yield return null;
                wait = Inspect("field07_main16_afterimage", "afterimage", 8, hearing); while (wait.MoveNext()) yield return null;
                wait = Inspect("field07_main16_canyon", "canyon", 9, hearing); while (wait.MoveNext()) yield return null;
                wait = Continue("협곡 입구 발견 후"); while (wait.MoveNext()) yield return null;
                Check(Object.FindObjectsByType<SceneTransitionTrigger>().Length == 1, "협곡 내부 Transition 없음");
                wait = Transition("Transition_east_to_field06", Chapter2Main16Flow.PreviousField); while (wait.MoveNext()) yield return null;
                Objective(10); wait = Transition("Transition_east_to_arbel", "Arbel"); while (wait.MoveNext()) yield return null;
                Objective(11); Leon(); pages = Pages(); while (pages.MoveNext()) yield return null;
                Check(QuestService.GetState(Chapter2Main16Flow.QuestId) == QuestState.Completed && QuestService.ActiveMainQuest == null, "12목표 완료/Main17 자동 시작 없음");
                wait = Continue("Main16 완료 후"); while (wait.MoveNext()) yield return null;
                Check(QuestService.GetState(Chapter2Main16Flow.QuestId) == QuestState.Completed && Chapter2Main16Flow.EmberGeneralSpawnUnlocked, "완료·지정승리 상태 유지");
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ProjectLimitless.Core;
using ProjectLimitless.NPC;
using ProjectLimitless.Player;
using ProjectLimitless.UI;
using ProjectLimitless.World;
using ProjectLimitless.Monster;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace ProjectLimitless.EditorTools
{
    /// <summary>격리 Batch에서만 실제 Scene·공용 펫 UI·별도 Save를 검증합니다. 사용자 슬롯과 설정은 읽지 않습니다.</summary>
    [InitializeOnLoad]
    public static class SafeZonePetServiceAudit
    {
        private const string Armed = "Limitless.SafeZonePetAudit";
        private static readonly BindingFlags Hidden = BindingFlags.Instance | BindingFlags.NonPublic;
        private static readonly string[] Scenes = { "World_StarterVillage", "Arbel", "Field_03" };
        private static readonly List<string> results = new List<string>(), errors = new List<string>();
        private static readonly List<string> knownEditorErrors = new List<string>();
        private static IEnumerator routine;
        private static double deadline;
        private static bool expectedSaveError;
        private static string Output => Path.Combine(Directory.GetCurrentDirectory(), "SafeZonePetResults");

        static SafeZonePetServiceAudit()
        {
            EditorApplication.playModeStateChanged += state =>
            {
                if (!SessionState.GetBool(Armed, false) || state != PlayModeStateChange.EnteredPlayMode) return;
                GameSaveService.AuditSaveDirectory = Path.Combine(Output, "Saves");
                UserSettingsService.BeginAudit(Path.Combine(Output, "Settings"));
                UserSettingsService.SetMuteAll(true);
                Application.logMessageReceived += Log;
                Application.runInBackground = true;
                routine = Flatten(Run());
                deadline = EditorApplication.timeSinceStartup + 360;
                EditorApplication.update += Tick;
            };
        }

        public static void RunBatch()
        {
            if (!Application.isBatchMode || !Application.dataPath.Replace('\\', '/')
                .Contains("/Temp/Field01RenewalQA/Client/Assets"))
                throw new InvalidOperationException("격리 Field01RenewalQA Batch만 허용합니다.");
            Directory.CreateDirectory(Output);
            // Play 진입 전과 진입 직후 모두 경로를 지정해 초기화 순서에 관계없이 사용자 슬롯을 보호합니다.
            GameSaveService.AuditSaveDirectory = Path.Combine(Output, "Saves");
            UserSettingsService.BeginAudit(Path.Combine(Output, "Settings"));
            UserSettingsService.SetMuteAll(true);
            if (!Path.GetFullPath(GameSaveService.GetSaveFilePath(1)).StartsWith(
                Path.GetFullPath(Path.Combine(Output, "Saves")) + Path.DirectorySeparatorChar,
                StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Save 격리 경로 검사에 실패했습니다.");
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            SessionState.SetBool(Armed, true);
            EditorApplication.EnterPlaymode();
        }

        private static void Check(bool pass, string id)
        {
            results.Add((pass ? "PASS|" : "FAIL|") + id);
            File.WriteAllLines(Path.Combine(Output, "results.txt"), results);
            if (!pass) throw new InvalidOperationException(id);
        }

        private static IEnumerator Wait(float seconds)
        { float end = Time.realtimeSinceStartup + seconds; while (Time.realtimeSinceStartup < end) yield return null; }
        private static IEnumerator Load(string name)
        {
            DialoguePresenter.Instance?.Hide();
            // SceneManager 직접 로드 fixture는 정식 SceneTransitionService의 위치 기록도 함께 준비합니다.
            // Bootstrap 진입은 마지막 월드 위치를 보존해야 실제 Continue가 그곳으로 돌아옵니다.
            if (name != "Bootstrap") GameSessionData.RecordLocation(name, string.Empty);
            SceneManager.LoadSceneAsync(name);
            float end = Time.realtimeSinceStartup + 30;
            while (SceneManager.GetActiveScene().name != name && Time.realtimeSinceStartup < end) yield return null;
            Check(SceneManager.GetActiveScene().name == name, "Scene." + name);
            yield return Wait(.5f);
            MonsterEncounterService.SuppressForSeconds(600);
            Check(SceneManager.GetActiveScene().GetRootGameObjects().Sum(g => g.GetComponentsInChildren<Transform>(true)
                .Sum(t => GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(t.gameObject))) == 0,
                "MissingScript0." + name);
        }

        private static void Seed(bool completed, string job = "sharpshooter", int slot = 1)
        {
            GameSessionData.Reset();
            GameSaveService.SelectSlot(slot);
            GameSessionData.ConfigurePlayer(PlayerVisualType.Male, "격리 안전지역 QA");
            GameSessionData.SelectPlayerPath("path.mobility");
            GameSessionData.SelectJob(job);
            GameSessionData.ConfigureProgress(15, 0);
            CompanionRosterService.UnlockIntroCompanions();
            EconomyService.Import(500);
            SetCompleted(completed);
            BeastCompanionService.RecordVictory(new[] { "venom_bee", "forest_spider" });
        }

        private static void SetCompleted(bool completed) => QuestService.ImportSaveData(new QuestProgressSaveData
        {
            CompletedQuestIds = QuestCatalog.All.Where(q => q.QuestType == QuestType.Main
                && (completed || q.QuestId != Chapter2Main21Flow.QuestId)).Select(q => q.QuestId).ToArray()
        });

        private static VillageNpcRole[] PetRoles() => Object.FindObjectsByType<VillageNpcRole>(FindObjectsSortMode.None)
            .Where(n => n.Role == VillageNpcRoleType.PetAdoption || n.Role == VillageNpcRoleType.PetManagement).ToArray();
        private static string NpcState(IEnumerable<VillageNpcRole> npcs) => string.Join("|", npcs.OrderBy(n => n.NpcId)
            .Select(n => n.NpcId + ":" + n.Role + ":" + n.transform.position + ":" + n.transform.localScale
                + ":" + (n.GetComponent<SpriteRenderer>()?.sprite?.name ?? "")
                + ":" + BodyState(n)));
        private static string BodyState(VillageNpcRole npc)
        {
            var body = npc.GetComponent<BoxCollider2D>();
            return body == null ? "no-body" : body.offset + ":" + body.size + ":" + body.isTrigger
                + ":" + body.enabled + ":" + body.edgeRadius;
        }
        private static T Get<T>(object target, string field) => (T)target.GetType().GetField(field, Hidden).GetValue(target);
        private static void Call(object target, string method) => target.GetType().GetMethod(method, Hidden).Invoke(target, null);
        private static void At(Vector2 position)
        {
            PlayerController player = Object.FindAnyObjectByType<PlayerController>();
            player.GetComponent<Rigidbody2D>().position = position;
            player.transform.position = position;
            Physics2D.SyncTransforms();
        }

        private static IEnumerator Run()
        {
            Seed(false);
            foreach (string scene in Scenes)
            {
                yield return Load(scene);
                Check(PetRoles().Length == (scene == "Arbel" ? 2 : 0), "BeforeClear.ExistingOnly." + scene);
                VillageNpcRole[] original = Object.FindObjectsByType<VillageNpcRole>(FindObjectsSortMode.None);
                string originalState = NpcState(original);
                SetCompleted(true); yield return Wait(.2f);
                Check(PetRoles().Count(n => n.Role == VillageNpcRoleType.PetAdoption) == 1
                    && PetRoles().Count(n => n.Role == VillageNpcRoleType.PetManagement) == 1,
                    "Completed.OnePerRole." + scene);
                Check(NpcState(original) == originalState, "ExistingNpc.ByteValuesPreserved." + scene);
                var originalPetNpcs = PetRoles().OrderBy(n => n.NpcId).ToArray();
                SetCompleted(true); SetCompleted(true); yield return Wait(.2f);
                Check(originalPetNpcs.SequenceEqual(PetRoles().OrderBy(n => n.NpcId)), "DuplicateCreation0." + scene);
                foreach (VillageNpcRole npc in PetRoles())
                {
                    Physics2D.SyncTransforms();
                    BoxCollider2D body = npc.GetComponent<BoxCollider2D>();
                    Check(body != null && !body.isTrigger && body.size == new Vector2(.5f, .34f)
                        && npc.GetComponent<SpriteRenderer>().sprite != null, "BodyAndApprovedSprite." + npc.NpcId);
                    Check(Physics2D.OverlapBoxAll(body.bounds.center, body.bounds.size * .95f, 0)
                        .All(c => c == body || c.isTrigger || c.GetComponentInParent<PlayerController>() != null),
                        "NoBodyObstacleOverlap." + npc.NpcId);
                    Vector2 approach = Approach(npc);
                    Check(Physics2D.OverlapCircleAll(approach, .24f).All(c => c.isTrigger
                        || c.GetComponentInParent<PlayerController>() != null), "ApproachClear." + npc.NpcId);
                    Check(Accessible(scene == "Field_03" ? new Vector2(.55f, -3.65f) : Vector2.zero, approach),
                        "WheelchairCollider.RouteFromCentralLane." + npc.NpcId);
                    if (scene == "Field_03")
                    {
                        Check(Vector2.Distance(npc.transform.position, new Vector2(.55f, -3.65f)) + .24f < 3.3f,
                            "InsideCatacombSafeZone." + npc.NpcId);
                        Check(Vector2.Distance(approach, new Vector2(.55f, -3.65f))
                            + Object.FindAnyObjectByType<PlayerController>().GetComponent<Collider2D>().bounds.extents.magnitude < 3.3f,
                            "ApproachInsideCatacombSafeZone." + npc.NpcId);
                    }
                    At(approach); yield return Wait(.15f);
                    var interaction = Object.FindAnyObjectByType<InteractionSystem>();
                    Check(interaction != null && interaction.CurrentTarget == npc.GetComponent<NpcController>(),
                        "ActualInteractionTarget." + npc.NpcId);
                    npc.Interact(npc.GetComponent<NpcController>());
                    yield return Wait(.1f);
                    if (npc.Role == VillageNpcRoleType.PetAdoption)
                    {
                        Button confirm = Object.FindObjectsByType<Button>(FindObjectsSortMode.None).First(b =>
                            b.gameObject.activeInHierarchy && b.GetComponentInChildren<Text>()?.text == "목록을 본다");
                        confirm.onClick.Invoke(); yield return Wait(.1f);
                    }
                    var ui = Object.FindAnyObjectByType<PetFacilityPresenter>();
                    Check(ui != null && Get<GameObject>(ui, "panel").activeSelf && WorldModalState.IsOpen
                        && PlayerController.IsMovementLocked, "ActualSharedUi.Open." + npc.NpcId);
                    ui.Close(); yield return Wait(.1f);
                    Check(!WorldModalState.IsOpen && !PlayerController.IsMovementLocked, "Ui.CloseReleasesInput." + npc.NpcId);
                }
                SetCompleted(false); yield return Wait(.2f);
                Check(PetRoles().Length == (scene == "Arbel" ? 2 : 0), "SameSceneOtherSlot.NoStaleNpc." + scene);
                Check(NpcState(original) == originalState, "ExistingNpc.AfterRemovalUnchanged." + scene);
            }
            Seed(true); yield return Load("Arbel");
            var adoption = PetRoles().Single(n => n.Role == VillageNpcRoleType.PetAdoption);
            At(Approach(adoption));
            PetFacilityPresenter.OpenAdoption(adoption.transform); yield return Wait(.1f);
            var facility = Object.FindAnyObjectByType<PetFacilityPresenter>();
            string party = JsonUtility.ToJson(CompanionRosterService.ExportSaveData());
            Call(facility, "ApplySelection");
            Check(BeastCompanionService.IsUnlocked("venom_bee") && EconomyService.GetCurrency() == 400,
                "ActualUi.Adopt100.SharedPurchase");
            Check(!BeastCompanionService.TryAdopt("venom_bee") && EconomyService.GetCurrency() == 400, "DoubleCharge0");
            facility.Close();
            PetFacilityPresenter.OpenManagement(PetRoles().Single(n => n.Role == VillageNpcRoleType.PetManagement).transform);
            yield return Wait(.1f);
            Check(BeastCompanionService.TryEquip("player", "venom_bee"), "SharedManagement.Equip");
            facility.Close();
            foreach (string scene in Scenes)
            {
                yield return Load(scene);
                Check(BeastCompanionService.IsUnlocked("venom_bee") && BeastCompanionService.GetEquippedId("player") == "venom_bee"
                    && EconomyService.GetCurrency() == 400, "CrossHub.SharedOwnedEquippedCurrency." + scene);
            }
            Check(JsonUtility.ToJson(CompanionRosterService.ExportSaveData()) == party, "PartyCompositionPreserved");
            Check(GameSaveService.SaveCurrentSession(), "Completed.Save");
            string expected = JsonUtility.ToJson(BeastCompanionService.ExportSaveData());
            yield return Load("Bootstrap"); GameSessionData.Reset();
            Button continueButton = GameObject.Find("StartMenuCanvas/Slot01/Action")?.GetComponent<Button>();
            Check(continueButton != null, "Continue.RealBootstrapButton");
            continueButton.onClick.Invoke();
            float end = Time.realtimeSinceStartup + 30;
            while (SceneManager.GetActiveScene().name == "Bootstrap" && Time.realtimeSinceStartup < end) yield return null;
            yield return Wait(.5f);
            Check(Chapter2Main21Flow.Completed && PetRoles().Length == 2
                && JsonUtility.ToJson(BeastCompanionService.ExportSaveData()) == expected
                && EconomyService.GetCurrency() == 400 && GameSaveService.CurrentVersion == 1,
                "Completed.RealContinue.ExistingSaveVersionAndPet");
            string slot1 = File.ReadAllText(GameSaveService.GetSaveFilePath(1));
            Seed(false, "fighter", 2); yield return Load("World_StarterVillage");
            Check(PetRoles().Length == 0 && !Chapter2Main21Flow.Completed, "ActualOtherSlot.NotCompleted");
            Check(GameSaveService.SaveCurrentSession(), "OtherSlot.Save");
            Check(!BeastCompanionService.TryAdopt("venom_bee") && EconomyService.GetCurrency() == 500, "NoSharpshooter.NoCharge");
            CompanionRosterService.UnlockSerin();
            Check(BeastCompanionService.GetSharpshooterCharacterIds().Contains(Chapter2IntroFlow.SerinId),
                "OtherJob.OwnedSharpshooterAllowed");
            Check(File.ReadAllText(GameSaveService.GetSaveFilePath(1)) == slot1, "OtherSlot.CompletedSaveUnchanged");
            yield return Load("Bootstrap"); GameSessionData.Reset();
            Button incompleteContinue = GameObject.Find("StartMenuCanvas/Slot02/Action")?.GetComponent<Button>();
            Check(incompleteContinue != null, "IncompleteContinue.RealBootstrapButton");
            incompleteContinue.onClick.Invoke();
            end = Time.realtimeSinceStartup + 30;
            while (SceneManager.GetActiveScene().name == "Bootstrap" && Time.realtimeSinceStartup < end) yield return null;
            yield return Wait(.5f);
            Check(!Chapter2Main21Flow.Completed && PetRoles().Length == 0
                && GameSaveService.CurrentSlotIndex == 2, "Incomplete.ActualContinue.NoExtraPetNpc");
            Check(GameSaveService.TryLoadSlot(1, out var saved), "ActualCompletedSlot.Load");
            GameSaveService.RestoreSession(1, saved); yield return Wait(.2f);
            Check(Chapter2Main21Flow.Completed && PetRoles().Length == 2
                && JsonUtility.ToJson(BeastCompanionService.ExportSaveData()) == expected, "SameScene.CompletedSlot.Restored");
            // 쓰기 실패에 대한 기존 원자성도 실제 파일 경계에서 확인합니다.
            string normal = GameSaveService.AuditSaveDirectory;
            string blocked = Path.Combine(Output, "BlockedSavePath");
            File.WriteAllText(blocked, "격리 실패주입");
            GameSaveService.AuditSaveDirectory = blocked; expectedSaveError = true;
            Check(!BeastCompanionService.TryAdopt("forest_spider") && !BeastCompanionService.IsUnlocked("forest_spider")
                && EconomyService.GetCurrency() == 400, "AdoptionFailedSave.CurrencyUnlockRollback");
            expectedSaveError = false; GameSaveService.AuditSaveDirectory = normal;
            Check(File.ReadAllText(GameSaveService.GetSaveFilePath(1)) == slot1, "FailedSave.PreviousFilePreserved");
            File.WriteAllLines(Path.Combine(Output, "console_errors.txt"), errors);
            File.WriteAllLines(Path.Combine(Output, "known_editor_search_errors.txt"), knownEditorErrors);
            Check(errors.Count == 0, "RuntimeConsole.Errors0");
            if (knownEditorErrors.Count > 0) results.Add("PARTIAL|KnownEditor.SearchDatabase|count=" + knownEditorErrors.Count);
            File.WriteAllLines(Path.Combine(Output, "results.txt"), results);
            File.WriteAllText(Path.Combine(Output, "status.txt"), knownEditorErrors.Count == 0 ? "PASS" : "PARTIAL_EDITOR_SEARCH");
        }

        private static void Log(string message, string stack, LogType type)
        {
            if (expectedSaveError && type == LogType.Error && message.Contains("쓰지 못했습니다"))
            { results.Add("EXPECTED_ERROR|SaveFaultInjection"); return; }
            // 기존 Editor Search 인덱스 예외만 정확히 분리하며 원문은 별도 파일에 남깁니다.
            // Runtime에서 같은 예외가 생기거나 다른 Search 오류면 정상 오류 집계에 포함합니다.
            if (type == LogType.Exception && message.StartsWith("ArgumentOutOfRangeException:", StringComparison.Ordinal)
                && stack.Contains("UnityEditor.Search.SearchDatabase")
                && stack.Contains("UnityEditor.Search.SearchInit.IndexationOnStartup"))
            { knownEditorErrors.Add(message + "\n" + stack); return; }
            if (type == LogType.Error || type == LogType.Exception || type == LogType.Assert)
                errors.Add(message + "\n" + stack);
        }
        private static bool Accessible(Vector2 start, Vector2 goal)
        {
            // 지체의 길 실제 Player Collider 크기를 사용한 공간 탐색입니다.
            // OS 입력을 흉내 내거나 순간이동을 플레이 이동 검증이라고 주장하지 않습니다.
            var player = Object.FindAnyObjectByType<PlayerController>();
            Vector2 size = player.GetComponent<Collider2D>().bounds.size;
            Vector2 offset = (Vector2)player.GetComponent<Collider2D>().bounds.center - (Vector2)player.transform.position;
            size += Vector2.one * .04f;
            Bounds bounds = Object.FindAnyObjectByType<WorldBounds2D>().Bounds;
            const float step = .2f;
            bool Clear(Vector2 point)
            {
                Vector2 center = point + offset;
                return center.x > bounds.min.x + size.x / 2 && center.x < bounds.max.x - size.x / 2
                    && center.y > bounds.min.y + size.y / 2 && center.y < bounds.max.y - size.y / 2
                    && Physics2D.OverlapBoxAll(center, size, 0)
                        .All(c => c.isTrigger || c.GetComponentInParent<PlayerController>() != null);
            }
            results.Add("INFO|PathGeometry|size=" + size + " offset=" + offset + " start=" + start + " goal=" + goal);
            if (!Clear(start) || !Clear(goal))
            {
                foreach (Vector2 point in new[] { start, goal })
                    results.Add("INFO|PathEndpoint|" + point + " bounds=" + bounds + " blockers=" + string.Join(",",
                        Physics2D.OverlapBoxAll(point + offset, size, 0).Where(c => !c.isTrigger
                            && c.GetComponentInParent<PlayerController>() == null).Select(c => c.name)));
                return false;
            }
            var queue = new Queue<Vector2Int>(); var seen = new HashSet<Vector2Int>();
            queue.Enqueue(Vector2Int.zero); seen.Add(Vector2Int.zero);
            Vector2Int[] moves = { Vector2Int.left, Vector2Int.right, Vector2Int.up, Vector2Int.down };
            while (queue.Count > 0 && seen.Count < 12000)
            {
                Vector2Int cell = queue.Dequeue(); Vector2 point = start + (Vector2)cell * step;
                if (Vector2.Distance(point, goal) < step * 1.5f && Clear((point + goal) * .5f)) return true;
                foreach (Vector2Int delta in moves)
                {
                    Vector2Int next = cell + delta;
                    if (seen.Contains(next)) continue;
                    Vector2 target = start + (Vector2)next * step;
                    if (!Clear(target) || !Clear((point + target) * .5f)) continue;
                    seen.Add(next); queue.Enqueue(next);
                }
            }
            results.Add("INFO|PathExhausted|visited=" + seen.Count);
            return false;
        }
        private static Vector2 Approach(VillageNpcRole npc)
        {
            var player = Object.FindAnyObjectByType<PlayerController>();
            float distance = Mathf.Max(.9f, npc.GetComponent<Collider2D>().bounds.extents.x
                + player.GetComponent<Collider2D>().bounds.extents.x + .1f);
            float laneX = npc.gameObject.scene.name == "Field_03" ? .55f : 0f;
            Vector2 direction = npc.transform.position.x < laneX ? Vector2.right : Vector2.left;
            return (Vector2)npc.transform.position + direction * distance;
        }
        private static IEnumerator Flatten(IEnumerator first)
        {
            var stack = new Stack<IEnumerator>(); stack.Push(first);
            while (stack.Count > 0)
            { var it = stack.Peek(); if (!it.MoveNext()) { stack.Pop(); continue; } if (it.Current is IEnumerator nested) stack.Push(nested); else yield return null; }
        }
        private static void Tick()
        {
            try
            {
                if (EditorApplication.timeSinceStartup > deadline) throw new TimeoutException("SafeZone QA timeout");
                if (routine.MoveNext()) return;
                EditorApplication.update -= Tick; SessionState.SetBool(Armed, false); EditorApplication.Exit(0);
            }
            catch (Exception e)
            {
                results.Add("FAIL|" + e); File.WriteAllLines(Path.Combine(Output, "results.txt"), results);
                File.WriteAllText(Path.Combine(Output, "status.txt"), "FAIL");
                EditorApplication.Exit(1);
            }
        }
    }
}

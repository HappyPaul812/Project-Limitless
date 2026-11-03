using System.Linq;
using ProjectLimitless.Core;
using ProjectLimitless.Player;
using ProjectLimitless.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectLimitless.World
{
    /// <summary>
    /// Main23의 마을 표식·이동·대화 UI를 재사용하는 비결제 Main24 첫 구간입니다.
    /// 후보 ID는 2026-11-03 바울 대표의 격리 정의 허용에 따라 추가했습니다.
    /// 상품·해제 아이템·F2 전투는 생성하지 않으며 마지막 대기 목표는 완료시키지 않습니다.
    /// </summary>
    public static class Chapter3Main24Flow
    {
        public const string QuestId = "main_24_first_snow_warning";
        public const string SkillNpc = "frostway-skill-shop", SupplyNpc = "frostway-supply";
        public const string NorthGate = "frostway_main24_north_gate";
        public static int Step => QuestService.ActiveMainQuest?.Definition.QuestId == QuestId
            ? QuestService.ActiveMainQuest.CurrentObjectiveIndex : -1;
        public static string Hint => Step == 0 ? "남동쪽 스킬상점에서 기본3·장착6 안내를 들으세요."
            : Step == 1 ? "남서쪽 보급 안내에서 한기 대비를 확인하세요. 구매 불필요."
            : Step == 2 ? "중앙 길을 따라 북문으로 이동하세요. 첫눈의 숲은 북쪽입니다."
            : Step == 3 ? "북문을 선택해 F2 첫눈의 숲에 들어가세요. 구매 불필요."
            : Step == 4 ? "F2 흙길을 따라 북동쪽 관찰 표식까지 이동해 조사하세요."
            : Step == 5 ? "탐험 확인 · 남쪽 출구로 돌아가 마을 북문에 복귀하세요."
            : Step >= 6 ? "안전 왕복 확인 · 첫 한기 전투 준비 중. 마을·숲 이동과 저장 가능."
            : "Main24 시작 가능 · 남동쪽 스킬상점 안내를 선택하세요.";

        // Main23 완료 후에도 대화를 강제로 열지 않고, 기존 저장의 선행 완료 기록으로 시작 가능 여부를 판정합니다.
        public static Main23Site CurrentSite()
        {
            if (SceneManager.GetActiveScene().name == Main24F2Traversal.SceneId)
                return Object.FindObjectsByType<Main23Site>(FindObjectsSortMode.None).FirstOrDefault(x => x.SiteKind == (Step == 4 ? Main23Site.Kind.F2Observe : Main23Site.Kind.F2Return));
            Main23Site.Kind kind = SceneManager.GetActiveScene().name != Chapter3Main23Flow.Village
                ? Main23Site.Kind.NorthGate : Step == 1 ? Main23Site.Kind.Supply
                : Step >= 2 ? Main23Site.Kind.LockedNorth : Main23Site.Kind.Skill;
            return Object.FindObjectsByType<Main23Site>(FindObjectsSortMode.None).FirstOrDefault(x => x.SiteKind == kind);
        }

        public static string SiteTitle(Main23Site.Kind kind)
        {
            switch (kind)
            {
                case Main23Site.Kind.Skill: return "스킬상점 · 안내 / 무료 6칸 장착";
                case Main23Site.Kind.Supply: return "보급 안내 · 한기 해제 수단 준비";
                case Main23Site.Kind.LockedNorth: return "북문 → F2 첫눈의 숲 · 안전 왕복";
                case Main23Site.Kind.F2Return: return "남쪽 출구 → 서릿길 마을 북문";
                case Main23Site.Kind.F2Observe: return "첫눈 관찰 · 탐험 경계 / 전투 없음";
                case Main23Site.Kind.Guide: return "주민 안내 · 북쪽 탐험 준비";
                default: return null;
            }
        }

        /// <summary>실제 Scene 도착과 북문 복귀만 인정합니다. PendingSpawn 처리 중에는 목표를 미리 채우지 않습니다.</summary>
        public static void UpdateArrival(PlayerController player)
        {
            if (player == null || WorldModalState.IsOpen
                || !string.IsNullOrEmpty(GameSessionData.PendingSpawnPointId)) return;
            string scene = SceneManager.GetActiveScene().name;
            if (scene == Main24F2Traversal.SceneId && Step == 3)
            {
                QuestService.NotifyLocationReached(Main24F2Traversal.EntryTarget); SaveProgress(); return;
            }
            if (scene != Chapter3Main23Flow.Village || (Step != 2 && Step != 5)) return;
            var gate = CurrentSite();
            if (gate == null || Vector2.Distance(player.transform.position, gate.transform.position) > Main23Site.Range) return;
            QuestService.NotifyLocationReached(Step == 2 ? NorthGate : Main24F2Traversal.ReturnTarget);
            SaveProgress();
        }

        /// <summary>표식의 기존 거리·입력 잠금 검사를 통과한 요청만 받습니다. 취소는 진행을 바꾸지 않습니다.</summary>
        public static bool TryInteract(Main23Site site)
        {
            switch (site.SiteKind)
            {
                case Main23Site.Kind.Skill:
                    Main21ChoicePanel.Show("스킬상점 · 상품 준비 중 / 구매 없음",
                        new[] { "기본 스킬·장착 안내 듣기", "무료 스킬 장착 / 해제", "닫기" }, choice =>
                        {
                            if (choice == 0) ShowSkillGuidance();
                            else if (choice == 1) SkillManagementPresenter.Instance?.Open();
                        });
                    return true;
                case Main23Site.Kind.Supply:
                    Show(new[] {
                        "첫눈의 숲은 마을 북문 너머에 있습니다. 중앙 길을 따라 북쪽으로 이동하세요.",
                        "숲의 한기는 2턴 동안 행동을 막을 수 있습니다. 전투에 들어가기 전에 한기를 해제할 수단을 준비해야 합니다.",
                        "스킬 구매는 진행 조건이 아닙니다. 현재는 숲의 안전한 첫 길을 탐험하고 돌아올 수 있습니다. 대응 물품 지급과 첫 전투는 준비 중입니다."
                    }, "main24_candidate_supply", () => AdvanceTalk(SupplyNpc, 1));
                    return true;
                case Main23Site.Kind.LockedNorth:
                    if (Step < 3)
                        Show(new[] { "스킬상점·한기 안내를 끝까지 확인한 뒤 북문으로 오세요. 구매할 필요는 없습니다." }, "main24_candidate_north", null);
                    else Main24F2Traversal.ChooseTravel(true);
                    return true;
                case Main23Site.Kind.F2Return:
                    Main24F2Traversal.ChooseTravel(false);
                    return true;
                case Main23Site.Kind.F2Observe:
                    Show(new[] {
                        "첫눈이 얇게 쌓였지만 가운데 흙길은 이어집니다. 돌아갈 길은 남쪽, 서릿길 마을 북문입니다.",
                        "여기서 더 깊은 숲과 서리올빼미의 기척을 살핍니다. 첫 한기 전투와 대응 물품은 준비 중입니다. 지금은 싸우지 않습니다.",
                        "관찰을 마쳤습니다. 남쪽 출구를 선택해 마을 북문으로 안전하게 돌아가세요. 구매는 필요 없습니다."
                    }, "main24_candidate_f2_observe", () => {
                        if (Step != 4) return;
                        QuestService.NotifyInteraction(Main24F2Traversal.ObserveTarget); SaveProgress();
                    });
                    return true;
                case Main23Site.Kind.Guide:
                    Show(new[] { "남동쪽 스킬상점에서 무료 기본 스킬과 장착법을, 남서쪽 보급 안내에서 한기 대비를 확인하세요. 첫눈의 숲 방향은 중앙 길을 따라 북쪽입니다." },
                        "main24_candidate_guide", null);
                    return true;
                default: return false;
            }
        }

        private static void ShowSkillGuidance()
        {
            Show(new[] {
                "직업의 기본 스킬 3개는 무료입니다. 전투에서 사용할 스킬은 최대 6칸까지 장착할 수 있습니다.",
                "전투 밖에서는 어디서든 K 또는 화면의 스킬 버튼으로 무료로 장착·해제할 수 있습니다. 매번 상점으로 돌아올 필요는 없습니다.",
                "강화 스킬은 기존 스킬을 대체하고 장착 칸을 이어받는 정책입니다. 현재 판매 상품은 준비 중이며 구매나 탈렌트 차감은 없습니다.",
                "스킬을 구매하지 않아도 이야기를 진행할 수 있습니다. 남서쪽 보급 안내에서 한기의 위험을 확인한 뒤 중앙 길을 따라 북문으로 가세요."
            }, "main24_candidate_skill", () => {
                // 끝까지 들은 뒤에만 시작·안내 목표를 함께 기록합니다. Esc 취소는 콜백을 호출하지 않습니다.
                if (QuestService.GetState(QuestId) == QuestState.Available) QuestService.TryStart(QuestId);
                AdvanceTalk(SkillNpc, 0);
            });
        }

        private static void AdvanceTalk(string npc, int expectedStep)
        {
            if (Step != expectedStep) return;
            QuestService.NotifyNpcTalked(npc);
            SaveProgress();
        }

        private static void SaveProgress()
        {
            if (GameSaveService.CurrentSlotIndex > 0 && !Chapter3Main23Flow.Save())
                QuestHudPresenter.Notify("진행 저장에 실패했습니다. 중앙 안전거점에서 다시 저장해 주세요.");
        }

        private static void Show(string[] pages, string candidateId, System.Action completed)
        {
            // TTS Catalog에 등록하지 않는 격리 대사 ID입니다. 기존 음성·공식 Story ID를 덮어쓰지 않습니다.
            var lines = pages.Select((text, index) => new DialogueLine(string.Empty,
                "Main24 · 탐험 안내", text, candidateId + "_" + (index + 1).ToString("00"))).ToArray();
            DialoguePresenter.Instance.ShowSequence(lines, completed);
        }
    }
}

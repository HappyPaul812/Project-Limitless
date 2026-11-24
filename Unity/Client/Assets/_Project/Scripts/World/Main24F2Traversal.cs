using ProjectLimitless.Core;
using ProjectLimitless.Player;
using ProjectLimitless.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectLimitless.World
{
    /// <summary>
    /// #83 격리 F2의 후보 Scene/Portal ID와 명시 선택 이동만 담당합니다.
    /// 저장기는 기존 서비스를 그대로 쓰며, 마지막 대기 목표나 전투를 완료하지 않습니다.
    /// </summary>
    public static class Main24F2Traversal
    {
        public const string SceneId = "World_NorthF2_CANDIDATE";
        public const string EntrySpawn = "Spawn_CANDIDATE_NorthF2_FromFrostway";
        public const string VillageSpawn = "Spawn_CANDIDATE_Frostway_FromNorthF2";
        public const string EntryTarget = "main24_candidate_f2_entry";
        public const string ObserveTarget = "main24_candidate_f2_observe";
        public const string ReturnTarget = "main24_candidate_f2_return";

        public static void InstallVillageSpawn(Scene scene)
        {
            var spawn = new GameObject(VillageSpawn, typeof(SceneSpawnPoint));
            SceneManager.MoveGameObjectToScene(spawn, scene);
            spawn.transform.position = new Vector2(0, 7);
            spawn.GetComponent<SceneSpawnPoint>().Configure(VillageSpawn);
        }

        public static void ChooseTravel(bool enterForest)
        {
            Main21ChoicePanel.Show(enterForest
                ? "F2 첫눈의 숲"
                : "남쪽 출구 · 서릿길 마을 북문으로", new[] {
                    enterForest ? "첫눈의 숲으로" : "마을 북문으로 돌아가기", "머무른다"
                }, choice => {
                    if (choice != 0) return;
                    string scene = enterForest ? SceneId : Chapter3Main23Flow.Village;
                    string spawn = enterForest ? EntrySpawn : VillageSpawn;
                    // 취소/Scene 누락은 Quest를 바꾸지 않습니다. 도착 목표는 실제 Spawn 후에만 인정합니다.
                    void Travel()
                    {
                        if (!SceneTransitionService.TryLoad(scene, spawn))
                            QuestHudPresenter.Notify("이동 경로를 확인할 수 없습니다. 다시 시도해 주세요.");
                    }
                    if (enterForest) Chapter3Main24Flow.ShowDeparture(Travel);
                    else Travel();
                });
        }
    }

}

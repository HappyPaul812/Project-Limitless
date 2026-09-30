using ProjectLimitless.Core;
using ProjectLimitless.NPC;
using ProjectLimitless.Player;
using ProjectLimitless.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectLimitless.World
{
    /// <summary>
    /// Field03의 기존 Quest 입구와 Scene을 건드리지 않고 준비 NPC를 설치합니다.
    /// 실제 플레이어가 안전 영역에 들어왔을 때만 전멸 복귀 지점을 갱신합니다.
    /// </summary>
    public sealed class CatacombEntranceSafeZone : MonoBehaviour
    {
        public const string SafeZoneId = "safezone_catacomb_entrance";
        private const string SceneId = "Field_03";
        private const string RespawnId = "Spawn_From_Dungeon01";
        private static readonly Vector2 Center = new Vector2(.55f, -3.65f);
        private const float ActivationRadius = 3.3f;
        private bool playerInside;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Register()
        {
            SceneManager.sceneLoaded -= Install;
            SceneManager.sceneLoaded += Install;
        }

        private static void Install(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != SceneId) return;
            GameObject root = new GameObject("CatacombEntranceSafeZone", typeof(CatacombEntranceSafeZone));
            SceneManager.MoveGameObjectToScene(root, scene);
        }

        private void Awake()
        {
            if (DialoguePresenter.Instance == null) new GameObject("DialogueSystem").AddComponent<DialoguePresenter>();
            CreateNpc("safezone-catacomb-supply", "보급 상인", "탐험에 필요한 물품을 준비해 두었습니다.",
                VillageNpcRoleType.GeneralShop, new Vector2(-2.05f, -2.55f));
            CreateNpc("safezone-catacomb-party", "파티 안내인", "내려가기 전에 동료와 진형을 점검하세요.",
                VillageNpcRoleType.PartyManager, new Vector2(2.85f, -2.55f));
            CreateNpc("safezone-catacomb-healer", "치유사", "다친 곳이 있다면 쉬었다 가세요.",
                VillageNpcRoleType.Healer, new Vector2(-2.05f, -4.55f));
            CreateNpc("safezone-catacomb-keeper", "묘지 감시인",
                "오래된 지하묘지가 이 아래에 있습니다. 요즘은 주변이 조금 불안정해졌습니다.",
                VillageNpcRoleType.GateGuard, new Vector2(2.85f, -4.55f));
        }

        private void CreateNpc(string id, string displayName, string dialogue, VillageNpcRoleType role, Vector2 position)
        {
            GameObject npc = new GameObject(id, typeof(SpriteRenderer), typeof(NpcController), typeof(VillageNpcRole));
            npc.transform.SetParent(transform, false);
            npc.transform.position = position;
            npc.GetComponent<NpcController>().Configure(displayName, dialogue);
            npc.GetComponent<VillageNpcRole>().Configure(id, role);
            GameObject label = new GameObject("Label", typeof(TextMesh));
            label.transform.SetParent(npc.transform, false);
            VillageNpcAppearanceCatalog.Apply(npc, id, displayName, role);
        }

        private void Update()
        {
            PlayerController player = FindAnyObjectByType<PlayerController>();
            bool inside = player != null && Vector2.SqrMagnitude((Vector2)player.transform.position - Center)
                <= ActivationRadius * ActivationRadius;
            if (inside != playerInside)
                ProjectLimitless.Audio.BgmPlaybackService.Instance?.SetSafeZoneMusic(inside);
            if (!inside) { playerInside = false; return; }
            if (playerInside) return;
            playerInside = true;
            // 다른 Field에서 걸어 들어올 때와 던전 복귀 Spawn에 도착할 때 모두 같은 거점을 활성화합니다.
            GameSessionData.ActivateSafeZone(SafeZoneId, SceneId, RespawnId);
            // Main11은 보스 처치나 석판 획득만으로 끝나지 않습니다. 실제 입구 안전지대에 도착했을 때
            // 마지막 목표를 알리면 QuestService의 완료 기록이 보상을 한 번만 지급하고 저장합니다.
            if (MainQuest11DungeonFlow.CurrentStep == 12)
                QuestService.NotifyLocationReached(SafeZoneId);
            if (GameSaveService.CurrentSlotIndex > 0) GameSaveService.SaveCurrentSession();
        }
    }
}

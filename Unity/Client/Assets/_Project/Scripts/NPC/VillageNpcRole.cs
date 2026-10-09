using ProjectLimitless.Core;
using ProjectLimitless.UI;
using ProjectLimitless.World;
using UnityEngine;

namespace ProjectLimitless.NPC
{
    /// <summary>표시 이름과 분리된 시작 마을 NPC의 안정적인 기능 역할입니다.</summary>
    public enum VillageNpcRoleType
    {
        Resident,
        MainQuestGuide,
        GeneralShop,
        EquipmentShop,
        Healer,
        Bank,
        PartyManager,
        GateGuard,
        TrainingGuide,
        VillageRepresentative,
        PetAdoption,
        PetManagement,
    }

    /// <summary>
    /// NPC 이름이 바뀌어도 기능을 잃지 않도록 stable ID와 역할을 보관합니다.
    /// 현재는 치유소만 실제 행동을 가지며 나머지 역할은 향후 전용 UI의 안전한 진입점입니다.
    /// </summary>
    public sealed class VillageNpcRole : MonoBehaviour
    {
        [SerializeField] private string npcId = "village-resident";
        [SerializeField] private VillageNpcRoleType role = VillageNpcRoleType.Resident;
        [SerializeField] private bool safeAreaOnly = true;

        public string NpcId => npcId;
        public VillageNpcRoleType Role => role;
        public bool SafeAreaOnly => safeAreaOnly;

        public void Configure(string stableNpcId, VillageNpcRoleType npcRole, bool isSafeAreaOnly = true)
        {
            npcId = stableNpcId;
            role = npcRole;
            safeAreaOnly = isSafeAreaOnly;
            NpcController npc = GetComponent<NpcController>();
            QuestNavigationTarget.Attach(gameObject, npcId, npc == null ? "NPC" : npc.DisplayName, new Vector3(0f, 1.9f, 0f));
        }

        /// <summary>상호작용은 표시 이름이 아니라 직렬화된 역할로 분기합니다.</summary>
        public void Interact(NpcController npc)
        {
            // 같은 NPC의 메인 대사와 여러 서브 의뢰를 선택창으로 나눕니다. 기존 업무 경로는 그대로 호출합니다.
            if (SideQuestNpcPresenter.TryOpen(npcId, npc, () => InteractUsual(npc))) return;
            InteractUsual(npc);
        }

        private void InteractUsual(NpcController npc)
        {
            if (Chapter2Main18Flow.TryHandleNpc(npcId, npc)) return;
            if (MainQuest05ReturnFlow.TryHandleNpc(npcId, npc)) return;
            if (MainQuest01NpcFlow.TryHandle(npcId, npc)) return;
            if (Chapter2Main15Flow.TryHandleNpc(npcId, npc)) return;
            if (Chapter2Main16Flow.TryHandleNpc(npcId, npc)) return;
            if (Chapter2IntroFlow.TryHandleNpc(npcId, npc)) return;
            if (role == VillageNpcRoleType.PetAdoption)
            {
                DialoguePresenter.Instance?.ShowConfirmation(npcId, npc.DisplayName,
                    "직접 상대해 본 종의 길들여진 개체를 맡겨 드립니다. 분양 목록을 보시겠습니까?",
                    "목록을 본다", "괜찮습니다", () => PetFacilityPresenter.OpenAdoption(npc.transform));
                return;
            }
            if (role == VillageNpcRoleType.PetManagement)
            {
                PetFacilityPresenter.OpenManagement(npc.transform);
                return;
            }
            if (role == VillageNpcRoleType.PartyManager)
            {
                if (npcId.StartsWith("safezone-catacomb-")
                    || QuestService.GetState(MainQuest05ReturnFlow.QuestId) == QuestState.Completed)
                    PartyManagementPresenter.OpenAt(npc.transform);
                else DialoguePresenter.Instance?.Show(npcId, npc.DisplayName, "동료와 함께 돌아오시면 편성을 도와드리겠습니다.");
                return;
            }
            if (role == VillageNpcRoleType.GeneralShop)
            {
                DialoguePresenter.Instance?.ShowConfirmation(
                    npcId, npc.DisplayName, "필요한 물건이 있으신가요?", "물건을 본다", "괜찮습니다",
                    () =>
                    {
                        if (npcId.StartsWith("safezone-catacomb-"))
                            ShopPresenter.OpenCatalog("ShopDefinitions/CatacombEntranceSupply", npc.transform);
                        else if (npcId == "arbel-shop" && Chapter2Main18Flow.CoolingShopUnlocked)
                            ShopPresenter.OpenCatalog("ShopDefinitions/ArbelGeneralShop", npc.transform);
                        else ShopPresenter.OpenStarterGeneralShop(npc.transform);
                    });
                return;
            }

            if (role != VillageNpcRoleType.Healer)
            {
                DialoguePresenter.Instance?.Show(npcId, npc.DisplayName, npc.Dialogue);
                return;
            }

            DialoguePresenter.Instance?.ShowConfirmation(
                npcId,
                npc.DisplayName,
                "상처를 치료하시겠습니까?",
                "치료한다",
                "괜찮습니다",
                () => HealAndSave(npcId, npc.DisplayName));
        }

        /// <summary>공용 자원 API로 파티 전체를 회복한 직후 현재 슬롯에 결과를 저장합니다.</summary>
        private static void HealAndSave(string npcId, string speaker)
        {
            PartyResourceService.HealPartyFully();
            GameSaveService.SaveCurrentSession();
            DialoguePresenter.Instance?.Show(npcId, speaker, "치료가 끝났습니다. 편안히 쉬었다 가세요.");
        }
    }
}

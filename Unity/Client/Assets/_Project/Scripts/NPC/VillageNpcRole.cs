using ProjectLimitless.Core;
using ProjectLimitless.UI;
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
        }

        /// <summary>상호작용은 표시 이름이 아니라 직렬화된 역할로 분기합니다.</summary>
        public void Interact(NpcController npc)
        {
            if (MainQuest01NpcFlow.TryHandle(npcId, npc)) return;
            if (role == VillageNpcRoleType.GeneralShop)
            {
                DialoguePresenter.Instance?.ShowConfirmation(
                    npcId, npc.DisplayName, "필요한 물건이 있으신가요?", "물건을 본다", "괜찮습니다",
                    () => ShopPresenter.OpenStarterGeneralShop(npc.transform));
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

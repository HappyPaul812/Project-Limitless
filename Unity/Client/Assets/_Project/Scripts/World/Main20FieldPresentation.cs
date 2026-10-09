using ProjectLimitless.Core;
using ProjectLimitless.Monster;
using UnityEngine;

namespace ProjectLimitless.World
{
    /// <summary>Quest 저장 진행에서 Boss/잔해/열기 완화 상태를 복원합니다. 완료한 대화나 전투 보상을 다시 실행하지 않습니다.</summary>
    public sealed class Main20FieldPresentation : MonoBehaviour
    {
        private GameObject boss,collapsed,recession,hotRift;
        private void Start()
        {
            boss=GameObject.Find("VeinfireColossusField");collapsed=GameObject.Find("CollapsedCore");recession=GameObject.Find("HeatRecession");hotRift=GameObject.Find("CoreRiftHot");
            if(boss!=null)
            {
                var definition=Resources.Load<MonsterDefinition>("MonsterDefinitions/18_VeinfireColossus");
                boss.AddComponent<MonsterSpriteSheetAnimation>().Configure(boss.GetComponent<SpriteRenderer>(),definition);
            }
            QuestService.Changed+=Refresh;Refresh();
        }
        private void OnDestroy()=>QuestService.Changed-=Refresh;
        private void Refresh()
        {
            bool won=Chapter2Main20Flow.Won;
            bool cooled=QuestService.GetState(Chapter2Main20Flow.QuestId)==QuestState.Completed||QuestService.ActiveMainQuest?.Definition.QuestId==Chapter2Main20Flow.QuestId&&QuestService.ActiveMainQuest.CurrentObjectiveIndex>=7;
            if(boss!=null)boss.SetActive(!won);if(collapsed!=null)collapsed.SetActive(won);if(recession!=null)recession.SetActive(cooled);
            if(hotRift!=null)hotRift.GetComponent<SpriteRenderer>().sprite=Resources.Load<Sprite>("Main20/Environment/"+(cooled?"DeepCore_Rift_01":"DeepCore_Rift_02"));
        }
    }
}

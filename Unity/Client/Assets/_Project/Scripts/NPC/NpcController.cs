using UnityEngine;

namespace ProjectLimitless.NPC
{
    /// <summary>고정 NPC의 표시 이름과 한 줄 대사를 보관한다.</summary>
    public sealed class NpcController : MonoBehaviour
    {
        [SerializeField] private string displayName = "마을 주민";
        [TextArea, SerializeField] private string dialogue = "어서 오세요. 여기는 우리의 첫 번째 마을입니다.";

        public string DisplayName => displayName;
        public string Dialogue => dialogue;

        public void Configure(string npcName, string npcDialogue)
        {
            displayName = npcName;
            dialogue = npcDialogue;
        }
    }
}

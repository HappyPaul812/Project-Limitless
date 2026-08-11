using UnityEngine;

namespace ProjectLimitless.NPC
{
    /// <summary>
    /// NPC GameObject에 붙어 화면에 표시할 이름과 대사 데이터를 보관합니다.
    /// InteractionSystem이 이 값을 읽어 DialoguePresenter에 전달합니다.
    /// </summary>
    public sealed class NpcController : MonoBehaviour
    {
        // NPC의 화면 표시 이름입니다. SerializeField는 private 필드도 Inspector에서 편집할 수 있게 합니다.
        [SerializeField] private string displayName = "마을 주민";
        // 여러 줄을 편하게 입력하도록 Inspector에 큰 입력 칸을 표시하는 대사입니다.
        [TextArea, SerializeField] private string dialogue = "어서 오세요. 여기는 우리의 첫 번째 마을입니다.";

        public string DisplayName => displayName;
        public string Dialogue => dialogue;

        /// <summary>Scene 생성 도구가 NPC 이름과 대사를 한 번에 설정할 때 사용합니다.</summary>
        public void Configure(string npcName, string npcDialogue)
        {
            displayName = npcName;
            dialogue = npcDialogue;
        }
    }
}

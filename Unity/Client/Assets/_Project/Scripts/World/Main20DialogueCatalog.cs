using ProjectLimitless.UI;
namespace ProjectLimitless.World
{
    /// <summary>승인된 Manifest 원문과 Stable Dialogue ID를 유지합니다. WAV 없이 텍스트로 진행 가능하며 음성을 생성하지 않습니다.</summary>
    public static class Main20DialogueCatalog
    {
        public static DialogueLine PhaseDirection => new DialogueLine("", "", "<지문> 열맥 거신의 몸체가 갈라지며 안쪽의 열핵이 드러납니다.", "main20_phase2_direction_01");
        public static DialogueLine[] Get(int index)
        {
            if(index==4)return new[]{new DialogueLine("companion_serin","세린","저 존재가 움직일 때마다 지면의 맥동이 따라옵니다.\n가까이 오니 더 분명해졌어요.","main20_confront_serin_01"),new DialogueLine("player","플레이어","그렇다고 저게 모든 일의 시작이라고 볼 수는 없겠죠.","main20_confront_player_01"),new DialogueLine("companion_serin","세린","네. 하지만 지금 이 지역의 열기를 크게 키우고 있는 건 확실해 보여요.","main20_confront_serin_02"),new DialogueLine("companion_serin","세린","열핵이 뛰는 간격을 보세요.\n공격하기 전마다 반응이 달라집니다.","main20_confront_serin_03")};
            if(index==6)return new[]{new DialogueLine("companion_serin","세린","열기가 내려가고 있어요.\n적어도 이 존재가 크게 증폭시키고 있던 건 맞았던 것 같습니다.","main20_after_colossus_serin_01"),new DialogueLine("player","플레이어","그런데 진동은 남아 있군요.","main20_after_colossus_player_01"),new DialogueLine("companion_serin","세린","네. 훨씬 약해졌지만…\n완전히 멈추지는 않았어요.","main20_after_colossus_serin_02")};
            return new[]{new DialogueLine("","",index==1?"심부의 균열에서 강한 열기가 올라옵니다.":"강한 열기는 완화됐지만 약한 지하 진동은 남아 있습니다.")};
        }
    }
}

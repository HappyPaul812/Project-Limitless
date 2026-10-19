using ProjectLimitless.Core;
namespace ProjectLimitless.World
{
    /// <summary>Main09의 로그·목표·토스트가 같은 단계와 행동을 안내하도록 묶은 표시 문구입니다. 진행 조건은 바꾸지 않습니다.</summary>
    public static class Main09QuestGuidance
    {
        public const float InteractionRadius = 1.6f;
        public const string Controls = "E/F · 게임패드 A";
        private static readonly string[] Instructions = {
            "숲길의 푸른 빛의 흔적 표시까지 이동해 [E/F · 게임패드 A]로 조사하세요.",
            "표시된 평행한 바퀴 자국에 다가가 [E/F · 게임패드 A]로 조사하세요.",
            "길 앞쪽의 폴에게 다가가 [E/F · 게임패드 A]로 대화하세요.",
            "폴에게 다시 말을 거세요. 조사 내용을 비교하세요. [E/F · 게임패드 A]",
            "표시된 가공된 석재에 다가가 [E/F · 게임패드 A]로 조사하세요.",
            "폴에게 돌아가 다시 말을 거세요. 다음 조사를 상의하세요. [E/F · 게임패드 A]"
        };
        private static readonly string[] Titles = {"푸른 빛의 흔적 조사","평행한 바퀴 자국 조사","폴에게 다가가 대화","폴에게 다시 말을 거세요","가공된 석재 조사","폴에게 돌아가 다시 대화"};
        public static bool Applies(QuestRuntimeState state) => state?.Definition.QuestId == MainQuest09FieldFlow.QuestId && state.CurrentObjective != null;
        public static string Instruction(int step) => Instructions[UnityEngine.Mathf.Clamp(step, 0, Instructions.Length - 1)];
        public static string Title(int step) => Titles[UnityEngine.Mathf.Clamp(step, 0, Titles.Length - 1)];
        public static string Action(int step) => step == 2 || step == 3 || step == 5 ? "대화" : "조사";
    }
}

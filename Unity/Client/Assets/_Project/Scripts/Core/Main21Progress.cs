namespace ProjectLimitless.Core
{
    /// <summary>Main21의 자유 순서 조사와 출발 수락만 보관합니다. 기존 Quest Save에 선택 필드로 저장하며 구버전은0입니다.</summary>
    public static class Main21Progress
    {
        public static int Flags { get; private set; }
        public static bool AllClues => (Flags & 7) == 7;
        public static bool ReturnAccepted => (Flags & 8) != 0;
        public static bool HasClue(int index) => index >= 0 && index < 3 && (Flags & (1 << index)) != 0;
        public static void Observe(int index) { if(index >= 0 && index < 3) Flags |= 1 << index; }
        public static void AcceptReturn() => Flags |= 8;
        public static void Restore(int flags) => Flags = flags & 15;
    }
}

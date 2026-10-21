namespace ProjectLimitless.Core
{
    /// <summary>같은 흔적을 반복 눌러2개 조사로 인정하지 않도록 서로 다른 흔적의 비트만 보관합니다.</summary>
    public static class Main22Progress
    {
        public static int Flags { get; private set; }
        public static bool HasClue(int index) => index >= 0 && index < 2 && (Flags & (1 << index)) != 0;
        public static bool Observe(int index)
        {
            if (index < 0 || index >= 2 || HasClue(index)) return false;
            Flags |= 1 << index; return true;
        }
        public static void Restore(int flags) => Flags = flags & 3;
    }
}

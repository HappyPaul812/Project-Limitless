namespace ProjectLimitless.Core
{
    /// <summary>월드 상호작용 화면이 동시에 둘 이상 열리지 않도록 현재 소유자를 한 곳에서 관리합니다.</summary>
    public static class WorldModalState
    {
        private static object owner;
        public static bool IsOpen => owner != null;
        public static bool IsOwnedBy(object candidate) => candidate != null && ReferenceEquals(owner, candidate);
        public static bool TryAcquire(object candidate)
        {
            if (candidate == null || (owner != null && !ReferenceEquals(owner, candidate))) return false;
            owner = candidate;
            return true;
        }
        public static void Release(object candidate)
        {
            if (ReferenceEquals(owner, candidate)) owner = null;
        }
        public static void Reset() => owner = null;
    }
}

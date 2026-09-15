using System;

namespace ProjectLimitless.Core
{
    /// <summary>현재 캐릭터의 공용 화폐를 음수와 정수 overflow 없이 관리합니다.</summary>
    public static class EconomyService
    {
        public static int Currency { get; private set; }
        public static int GetCurrency() => Currency;
        public static bool CanAfford(int amount) => amount >= 0 && Currency >= amount;
        public static bool AddCurrency(int amount)
        {
            if (amount < 0 || Currency > int.MaxValue - amount) return false;
            Currency += amount;
            return true;
        }
        /// <summary>잔액이 부족하면 false만 반환하고 기존 돈은 절대 바꾸지 않습니다.</summary>
        public static bool TrySpendCurrency(int amount)
        {
            if (!CanAfford(amount)) return false;
            Currency -= amount;
            return true;
        }
        public static void Import(int amount) => Currency = Math.Max(0, amount);
        public static void Reset() => Currency = 0;
    }
}

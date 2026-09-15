using UnityEngine;

namespace ProjectLimitless.Core
{
    /// <summary>상점·보상·인벤토리가 같은 화폐 이름과 아이콘을 쓰도록 표시 정책을 한곳에 둡니다.</summary>
    public static class CurrencyPresentation
    {
        public const string DisplayName = "탈렌트";
        public static readonly Color IconTint = new Color(0.9f, 0.7f, 0.22f, 1f);
        private const string IconResourcePath = "Currency/CurrencyIcon";
        private static Sprite icon;

        public static Sprite Icon
        {
            get
            {
                if (icon == null) icon = Resources.Load<Sprite>(IconResourcePath);
                return icon;
            }
        }

        public static string FormatAmount(int amount) => $"{Mathf.Max(0, amount):N0} {DisplayName}";
    }
}

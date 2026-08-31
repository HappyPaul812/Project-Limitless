using System;
using System.Linq;
using UnityEngine;

namespace ProjectLimitless.Battle
{
    /// <summary>
    /// PVFX Foundry의 Venom Ward 아이콘과 Acid Splash 독 틱 연출을 한곳에서 준비합니다.
    /// ThirdParty ZIP은 수정하지 않고 Resources의 프로젝트용 복사본만 런타임 Sprite로 나눕니다.
    /// </summary>
    public static class BattlePoisonVisuals
    {
        public const int VenomWardIconFrame = 6;
        public const float TickFrameDuration = .05f;
        public static readonly Vector2 TickEffectSize = new Vector2(52f, 52f);

        private const int CellSize = 96;
        private const int Columns = 5;
        private const string VenomWardPath = "BattleStatusEffects/Poison/venom_ward_sheet";
        private const string AcidSplashPath = "BattleStatusEffects/Poison/acid_splash_sheet";
        private static Sprite[] venomWardFrames;
        private static Sprite[] poisonTickFrames;

        public static Sprite LoadStatusIcon()
        {
            venomWardFrames ??= SliceGrid(Resources.Load<Texture2D>(VenomWardPath), 16, "VenomWard");
            return venomWardFrames.Length > VenomWardIconFrame ? venomWardFrames[VenomWardIconFrame] : null;
        }

        public static Sprite[] LoadTickFrames()
        {
            if (poisonTickFrames != null) return poisonTickFrames;
            Sprite[] allFrames = SliceGrid(Resources.Load<Texture2D>(AcidSplashPath), 14, "AcidSplash");
            // 낙하 전 빈 공간이 큰 0~2와 긴 잔상 9~13은 제외하고 접촉·확산 3~8만 짧게 사용합니다.
            poisonTickFrames = allFrames.Skip(3).Take(6).ToArray();
            return poisonTickFrames;
        }

        private static Sprite[] SliceGrid(Texture2D sheet, int frameCount, string prefix)
        {
            if (sheet == null || frameCount <= 0) return Array.Empty<Sprite>();
            Sprite[] frames = new Sprite[frameCount];
            for (int index = 0; index < frameCount; index++)
            {
                int column = index % Columns;
                int rowFromTop = index / Columns;
                float y = sheet.height - ((rowFromTop + 1) * CellSize);
                frames[index] = Sprite.Create(sheet,
                    new Rect(column * CellSize, y, CellSize, CellSize), Vector2.one * .5f,
                    CellSize, 0, SpriteMeshType.FullRect);
                frames[index].name = $"{prefix}_{index:00}";
            }
            return frames;
        }
    }
}

using UnityEngine;

namespace ProjectLimitless.Battle
{
    /// <summary>
    /// 파이어 볼이 사용하는 CC0 PVFX 시트의 경로와 프레임 규칙을 한곳에서 관리합니다.
    /// 전투 화면이 PNG 파일명을 직접 알지 않게 해, 연출 Asset을 바꿀 때 대상·피해 코드를 건드리지 않습니다.
    /// </summary>
    public static class BattleFireballVisuals
    {
        public const float FrameDuration = .05f;
        public const int ExplosionPeakFrame = 4;
        public static readonly Vector2 ChargeSize = new Vector2(128f, 128f);
        public static readonly Vector2 ProjectileSize = new Vector2(80f, 80f);
        public static readonly Vector2 ExplosionSize = new Vector2(192f, 192f);
        public static readonly Vector2 BurnTickSize = new Vector2(46f, 46f);

        private const int CellSize = 96;
        private const int Columns = 5;
        private const string ChargeResourcePath = "BattleSkillEffects/SolarShrapnelCharge/solar_shrapnel_charge_sheet";
        private const string ExplosionResourcePath = "BattleSkillEffects/WarmExplosion/warm_explosion_sheet";
        private static Sprite[] chargeFrames;
        private static Sprite[] explosionFrames;

        public static Sprite[] LoadChargeFrames() => chargeFrames ??= SliceGrid(
            Resources.Load<Texture2D>(ChargeResourcePath), 2, "SolarShrapnelCharge");

        public static Sprite[] LoadExplosionFrames() => explosionFrames ??= SliceGrid(
            Resources.Load<Texture2D>(ExplosionResourcePath), 15, "WarmExplosion");

        public static Sprite LoadSkillIcon()
        {
            Sprite[] frames = LoadExplosionFrames();
            return frames.Length > ExplosionPeakFrame ? frames[ExplosionPeakFrame] : null;
        }

        /// <summary>
        /// PVFX Grid는 96×96 프레임을 한 줄에 5개씩 놓습니다. JSON의 순서는 왼쪽 위부터지만
        /// Unity Sprite 좌표는 왼쪽 아래가 원점이므로 행의 Y를 뒤집어야 같은 0→1→2 순서가 됩니다.
        /// 원본 시트를 수정하지 않고 런타임 Sprite만 만들기 때문에 ThirdParty 원본도 그대로 보존됩니다.
        /// </summary>
        private static Sprite[] SliceGrid(Texture2D sheet, int frameCount, string namePrefix)
        {
            if (sheet == null || frameCount <= 0) return System.Array.Empty<Sprite>();
            Sprite[] frames = new Sprite[frameCount];
            for (int index = 0; index < frameCount; index++)
            {
                int column = index % Columns;
                int rowFromTop = index / Columns;
                float y = sheet.height - ((rowFromTop + 1) * CellSize);
                frames[index] = Sprite.Create(sheet,
                    new Rect(column * CellSize, y, CellSize, CellSize), Vector2.one * .5f,
                    CellSize, 0, SpriteMeshType.FullRect);
                frames[index].name = $"{namePrefix}_{index:00}";
            }
            return frames;
        }
    }
}

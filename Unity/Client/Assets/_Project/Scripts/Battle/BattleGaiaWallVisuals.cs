using UnityEngine;

namespace ProjectLimitless.Battle
{
    /// <summary>가이아 웰이 사용하는 CC0 arcane-parry Grid의 경로와 프레임 규칙입니다.</summary>
    public static class BattleGaiaWallVisuals
    {
        public const float FrameDuration = .05f;
        public const int PeakFrame = 8;
        public static readonly Vector2 EffectSize = new Vector2(150f, 150f);
        public static readonly Color EffectTint = new Color(.72f, 1f, 1f, 1f);

        private const int CellSize = 96;
        private const int Columns = 5;
        private const int FrameCount = 16;
        private const string ResourcePath = "BattleSkillEffects/ArcaneParry/arcane_parry_sheet";
        private static Sprite[] frames;

        public static Sprite[] LoadFrames()
        {
            if (frames != null) return frames;
            Texture2D sheet = Resources.Load<Texture2D>(ResourcePath);
            if (sheet == null) return frames = System.Array.Empty<Sprite>();

            frames = new Sprite[FrameCount];
            for (int index = 0; index < FrameCount; index++)
            {
                // 원본 Grid는 왼쪽 위부터 읽고 Unity는 왼쪽 아래를 원점으로 삼으므로 Y행을 뒤집습니다.
                int column = index % Columns;
                int rowFromTop = index / Columns;
                float y = sheet.height - ((rowFromTop + 1) * CellSize);
                frames[index] = Sprite.Create(sheet,
                    new Rect(column * CellSize, y, CellSize, CellSize), Vector2.one * .5f,
                    CellSize, 0, SpriteMeshType.FullRect);
                frames[index].name = $"ArcaneParry_{index:00}";
            }
            return frames;
        }
    }
}

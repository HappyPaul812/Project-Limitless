using UnityEngine;

namespace ProjectLimitless.Battle
{
    /// <summary>
    /// 수호자 철벽이 사용하는 CC0 Earth Rupture Grid의 경로와 프레임 규칙입니다.
    /// 이 클래스는 그림을 20장으로 나누는 역할만 하며, 70% 감소와 2회 행동 지속은 상태 런타임이 담당합니다.
    /// </summary>
    public static class BattleIronWallVisuals
    {
        public const float FrameDuration = .05f;
        public const int PeakFrame = 9;
        public static readonly Vector2 EffectSize = new Vector2(158f, 158f);

        private const int CellSize = 96;
        private const int Columns = 5;
        private const int FrameCount = 20;
        private const string ResourcePath = "BattleSkillEffects/EarthRupture/earth_rupture_sheet";
        private static Sprite[] frames;

        public static Sprite[] LoadFrames()
        {
            if (frames != null) return frames;
            Texture2D sheet = Resources.Load<Texture2D>(ResourcePath);
            if (sheet == null) return frames = System.Array.Empty<Sprite>();

            frames = new Sprite[FrameCount];
            for (int index = 0; index < FrameCount; index++)
            {
                // PVFX Grid는 왼쪽 위부터, Unity 좌표는 왼쪽 아래부터 시작하므로 행 위치만 뒤집습니다.
                int column = index % Columns;
                int rowFromTop = index / Columns;
                float y = sheet.height - ((rowFromTop + 1) * CellSize);
                frames[index] = Sprite.Create(sheet,
                    new Rect(column * CellSize, y, CellSize, CellSize), Vector2.one * .5f,
                    CellSize, 0, SpriteMeshType.FullRect);
                frames[index].name = $"EarthRupture_{index:00}";
            }
            return frames;
        }
    }
}

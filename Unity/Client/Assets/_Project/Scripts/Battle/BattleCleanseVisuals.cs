using UnityEngine;

namespace ProjectLimitless.Battle
{
    /// <summary>
    /// 정화에 사용하는 CC0 spectral-bloom 시트의 경로와 프레임 규칙입니다. 버튼은 Peak 한 장만 쓰고
    /// 전투 연출은 16장을 모두 쓰므로, 메뉴를 열어도 실제 애니메이션의 재생 상태에는 영향을 주지 않습니다.
    /// </summary>
    public static class BattleCleanseVisuals
    {
        public const float FrameDuration = .05f;
        public const int ReleaseFrame = 7;
        public const int IconPeakFrame = 5;
        public static readonly Vector2 EffectSize = new Vector2(132f, 132f);

        private const int CellSize = 96;
        private const int Columns = 5;
        private const int FrameCount = 16;
        private const string ResourcePath = "BattleSkillEffects/SpectralBloom/spectral_bloom_sheet";
        private static Sprite[] frames;

        public static Sprite[] LoadFrames()
        {
            if (frames != null) return frames;
            Texture2D sheet = Resources.Load<Texture2D>(ResourcePath);
            if (sheet == null) return frames = System.Array.Empty<Sprite>();

            frames = new Sprite[FrameCount];
            for (int index = 0; index < FrameCount; index++)
            {
                // PVFX Grid는 왼쪽 위부터 배열되고 Unity Sprite 좌표는 왼쪽 아래에서 시작하므로 행을 뒤집습니다.
                int column = index % Columns;
                int rowFromTop = index / Columns;
                float y = sheet.height - ((rowFromTop + 1) * CellSize);
                frames[index] = Sprite.Create(sheet,
                    new Rect(column * CellSize, y, CellSize, CellSize), Vector2.one * .5f,
                    CellSize, 0, SpriteMeshType.FullRect);
                frames[index].name = $"SpectralBloom_{index:00}";
            }
            return frames;
        }

        public static Sprite LoadSkillIcon()
        {
            Sprite[] loaded = LoadFrames();
            return loaded.Length > IconPeakFrame ? loaded[IconPeakFrame] : null;
        }
    }
}

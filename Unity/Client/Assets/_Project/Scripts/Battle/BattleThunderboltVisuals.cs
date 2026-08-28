using UnityEngine;

namespace ProjectLimitless.Battle
{
    /// <summary>CC0 electric-impact 시트의 경로·프레임 구조를 전투 화면 밖에서 관리합니다.</summary>
    public static class BattleThunderboltVisuals
    {
        public const float FrameDuration = .05f;
        public const int PeakFrame = 1;
        public static readonly Vector2 TelegraphSize = new Vector2(54f, 54f);
        public static readonly Vector2 ImpactSize = new Vector2(144f, 144f);

        private const int CellSize = 96;
        private const int Columns = 5;
        private const int FrameCount = 14;
        private const string ImpactResourcePath = "BattleSkillEffects/ElectricImpact/electric_impact_sheet";
        private static Sprite[] impactFrames;

        public static Sprite[] LoadImpactFrames()
        {
            if (impactFrames != null) return impactFrames;
            Texture2D sheet = Resources.Load<Texture2D>(ImpactResourcePath);
            if (sheet == null) return impactFrames = System.Array.Empty<Sprite>();

            impactFrames = new Sprite[FrameCount];
            for (int index = 0; index < FrameCount; index++)
            {
                // PVFX Grid는 왼쪽 위부터 96×96 칸을 읽지만 Unity의 Sprite 좌표는 왼쪽 아래부터입니다.
                // 그래서 행 Y를 뒤집어야 원본 0→13 애니메이션 순서가 정확히 유지됩니다.
                int column = index % Columns;
                int rowFromTop = index / Columns;
                float y = sheet.height - ((rowFromTop + 1) * CellSize);
                impactFrames[index] = Sprite.Create(sheet,
                    new Rect(column * CellSize, y, CellSize, CellSize), Vector2.one * .5f,
                    CellSize, 0, SpriteMeshType.FullRect);
                impactFrames[index].name = $"ElectricImpact_{index:00}";
            }
            return impactFrames;
        }

        public static Sprite LoadSkillIcon()
        {
            Sprite[] frames = LoadImpactFrames();
            return frames.Length > PeakFrame ? frames[PeakFrame] : null;
        }
    }
}

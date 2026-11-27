using UnityEngine;

namespace ProjectLimitless.Battle
{
    /// <summary>
    /// 수호의 맹세가 사용하는 승인 원화를 짧은 보호 파동으로 준비합니다.
    /// 승인 시트의 슬롯 4~10을 보호 대상 각각의 발밑에 재생합니다.
    /// 실제 보호 시간은 VFX가 아니라 상태 런타임이 관리하므로 큰 그림을 화면에 계속 남기지 않습니다.
    /// </summary>
    public static class BattleGuardianCoverVisuals
    {
        public const float FrameDuration = .045f;
        public const int ApplyFrame = 1;
        public static readonly Vector2 AllyEffectSize = new Vector2(172f, 172f);
        public static readonly Color ProtectiveTint = new Color(1f, .94f, .72f, .94f);

        private const int CellSize = 96;
        private const int Columns = 5;
        private const int FirstFrame = 4;
        private const int LastFrame = 10;
        private const string ResourcePath = "BattleSkillEffects/GuardianOath_Approved";
        private static Sprite[] frames;

        public static Sprite[] LoadProtectionFrames()
        {
            if (frames != null) return frames;
            Texture2D sheet = Resources.Load<Texture2D>(ResourcePath);
            if (sheet == null) return frames = System.Array.Empty<Sprite>();

            frames = new Sprite[LastFrame - FirstFrame + 1];
            for (int sourceIndex = FirstFrame; sourceIndex <= LastFrame; sourceIndex++)
            {
                int outputIndex = sourceIndex - FirstFrame;
                int column = sourceIndex % Columns;
                int rowFromTop = sourceIndex / Columns;
                float y = sheet.height - ((rowFromTop + 1) * CellSize);
                frames[outputIndex] = Sprite.Create(sheet,
                    new Rect(column * CellSize, y, CellSize, CellSize), Vector2.one * .5f,
                    CellSize, 0, SpriteMeshType.FullRect);
                frames[outputIndex].name = $"GuardianOath_{sourceIndex:00}";
            }
            return frames;
        }
    }
}
